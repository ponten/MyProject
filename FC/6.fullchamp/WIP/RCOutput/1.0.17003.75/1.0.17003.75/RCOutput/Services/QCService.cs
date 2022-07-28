using RCOutput.Models;
using SajetClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.Windows.Forms;

namespace RCOutput.Services
{
    /// <summary>
    /// 品保資料收集的商業邏輯
    /// </summary>
    public static class QCService
    {
        /// <summary>
        /// 將輸入值寫入資料表
        /// </summary>
        public static void SaveData(string emp_id, List<KeyValuePair<string, string>> s_input, string RC_NO)
        {
            string s = @"
SELECT
    TO_CHAR(SYSDATE, 'yyyyMMddHH24miss')
    || SUBSTR(TO_CHAR(SYSTIMESTAMP, 'ff'), 1, 1) SN_ID
FROM
    DUAL
";
            var d = ClientUtils.ExecuteSQL(s);

            string SN_ID = d.Tables[0].Rows[0]["SN_ID"].ToString();

            DateTime now = OtherService.GetDBDateTimeNow();

            foreach (var item in s_input)
            {
                s = @"
INSERT INTO SAJET.G_QC_VALUE (
    RC_NO,
    PROCESS_ID,
    NODE_ID,
    SN_ID,
    ITEM_ID,
    ITEM_VALUE,
    UPDATE_USERID,
    UPDATE_TIME
)
    SELECT
        RC_NO,
        PROCESS_ID,
        NODE_ID,
        :SN_ID,
        :ITEM_ID,
        :ITEM_VALUE,
        :UPDATE_USERID,
        :UPDATE_TIME
    FROM
        SAJET.G_RC_STATUS
    WHERE
        A.RC_NO = :RC_NO
";
                var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", RC_NO },
                    new object[] { ParameterDirection.Input, OracleType.Number, "SN_ID", SN_ID },
                    new object[] { ParameterDirection.Input, OracleType.Number, "ITEM_ID", item.Key },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_VALUE", item.Value },
                    new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", emp_id },
                    new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", now },
                };

                ClientUtils.ExecuteSQL(s, p.ToArray());
            }
        }

        /// <summary>
        /// 讀取流程卡的 AQL 規則
        /// </summary>
        public static string LoadAQL(string RC_NO)
        {
            string s = @"
SELECT
    C.SAMPLING_TYPE
FROM
    SAJET.G_RC_STATUS            A,
    SAJET.SYS_PART_QC_PLAN       B,
    SAJET.SYS_QC_SAMPLING_PLAN   C
WHERE
    A.RC_NO = :RC_NO
    AND A.PART_ID = B.PART_ID
    AND A.PROCESS_ID = B.PROCESS_ID
    AND B.SAMPLING_ID = C.SAMPLING_ID
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", RC_NO },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d.Tables[0].Rows.Count > 0)
            {
                return d.Tables[0].Rows[0]["SAMPLING_TYPE"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 取得流程卡的工單號碼
        /// </summary>
        /// <returns></returns>
        public static string GetWONumber(string RC_NO, out string PROCESS_ID)
        {
            string s = $@"
SELECT
    WORK_ORDER,
    PROCESS_ID
FROM
    SAJET.G_RC_STATUS
WHERE
    RC_NO = :RC_NO
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", RC_NO },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d.Tables[0].Rows.Count > 0)
            {
                PROCESS_ID = d.Tables[0].Rows[0]["WORK_ORDER"].ToString();

                return d.Tables[0].Rows[0]["WORK_ORDER"].ToString();
            }
            else
            {
                PROCESS_ID = "";

                return string.Empty;
            }
        }

        /// <summary>
        /// 計算RC所屬工單的目前RC製程總數
        /// </summary>
        /// <returns></returns>
        public static string SumTotal(string RC_NO)
        {
            string s = @"
WITH RC_INFO AS (
    SELECT
        WORK_ORDER,
        PROCESS_ID
    FROM
        SAJET.G_RC_STATUS
    WHERE
        RC_NO = :RC_NO
)
SELECT
    SUM(WIP_IN_QTY) WIP_IN_QTY
FROM
    (
        SELECT
            SUM(NVL(A.WIP_IN_QTY, 0)) WIP_IN_QTY
        FROM
            SAJET.G_RC_STATUS   A,
            RC_INFO             B
        WHERE
            A.WORK_ORDER = B.WORK_ORDER
            AND A.PROCESS_ID = B.PROCESS_ID
        UNION ALL
        SELECT
            SUM(NVL(A.WIP_IN_QTY, 0)) WIP_IN_QTY
        FROM
            SAJET.G_RC_TRAVEL   A,
            RC_INFO             B
        WHERE
            A.WORK_ORDER = B.WORK_ORDER
            AND A.PROCESS_ID = B.PROCESS_ID
    )
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", RC_NO },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            return d.Tables[0].Rows[0]["WIP_IN_QTY"].ToString();
        }

        /// <summary>
        /// 取得所有資料收集筆數
        /// </summary>
        /// <returns></returns>
        public static string GetTotalData(string RC_NO)
        {
            string Result = string.Empty;

            if (!string.IsNullOrEmpty(RC_NO))
            {
                string s = @"
/* 指定流程卡的基本資訊 */
WITH RC_INFO AS (
    SELECT
        WORK_ORDER,
        RC_NO,
        PROCESS_ID,
        NODE_ID,
        PART_ID
    FROM
        SAJET.G_RC_STATUS
    WHERE
        RC_NO = :RC_NO
),
/* 所有屬於同工單的流程卡 */
ALL_RC AS (
    SELECT
        RC_NO
    FROM
        RC_INFO
    UNION
    SELECT
        A.RC_NO
    FROM
        SAJET.G_RC_TRAVEL   A,
        RC_INFO             B
    WHERE
        A.WORK_ORDER = B.WORK_ORDER
        AND B.PROCESS_ID = B.PROCESS_ID
),
/* 該工單在指定製程蒐集到的資料 ID （一組資料共用一個 ID）*/
SN_IDS AS (
    SELECT DISTINCT
        SN_ID
    FROM
        SAJET.G_QC_VALUE   A,
        RC_INFO            B,
        ALL_RC             C
    WHERE
        A.PROCESS_ID = B.PROCESS_ID
        AND A.RC_NO = C.RC_NO
)
/* 共有幾筆 */
SELECT
    COUNT(SN_ID) SAMPLED_QTY
FROM
    SN_IDS
";
                var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", RC_NO },
                };

                var d = ClientUtils.ExecuteSQL(s, p.ToArray());

                if (d != null &&
                    d.Tables[0].Rows.Count > 0)
                {
                    Result = d.Tables[0].Rows[0]["SAMPLED_QTY"].ToString();
                }
                else
                {
                    Result = "0";
                }
            }

            return Result;
        }

        /// <summary>
        /// 取得總應抽數
        /// </summary>
        /// <param name="TotalNum"></param>
        /// <returns></returns>
        public static string GetTotalBeSampling(string TotalNum, string RC_NO)
        {
            if (string.IsNullOrEmpty(TotalNum))
            {
                TotalNum = "0";
            }

            string s = $@"
SELECT
    C.SAMPLE_SIZE
FROM
    SAJET.G_RC_STATUS                   A,
    SAJET.SYS_PART_QC_PLAN              B,
    SAJET.SYS_QC_SAMPLING_PLAN_DETAIL   C
WHERE
    A.RC_NO = :RC_NO
    AND A.PART_ID = B.PART_ID
    AND A.PROCESS_ID = B.PROCESS_ID
    AND B.SAMPLING_ID = C.SAMPLING_ID
    AND C.SAMPLING_LEVEL = '0'
    AND B.ENABLED = 'Y'
    AND C.MAX_LOT_SIZE >= :NUM
    AND C.MIN_LOT_SIZE <= :NUM
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", RC_NO },
                new object[] { ParameterDirection.Input, OracleType.Number, "NUM", TotalNum },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d.Tables[0].Rows.Count > 0)
            {
                int _total = Convert.ToInt32(TotalNum);

                int _sample = Convert.ToInt32(d.Tables[0].Rows[0]["SAMPLE_SIZE"].ToString());

                if (_total < _sample)
                {
                    return _total.ToString();
                }
                else
                {
                    return d.Tables[0].Rows[0]["SAMPLE_SIZE"].ToString();
                }
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// 取得 RC 已抽數
        /// </summary>
        /// <returns></returns>
        public static string GetRCSampled(string RC_NO)
        {
            string sSQL = $@"
WITH A AS (
    SELECT DISTINCT
        ( SN_ID ) SN_ID
    FROM
        SAJET.G_QC_VALUE    A,
        SAJET.G_RC_STATUS   B
    WHERE
        B.RC_NO = :RC_NO
        AND A.RC_NO = B.RC_NO
        AND A.PROCESS_ID = B.PROCESS_ID
)
SELECT
    COUNT(SN_ID) SAMPLED_QTY
FROM
    A
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", RC_NO },
            };

            var ds = ClientUtils.ExecuteSQL(sSQL, p.ToArray());

            return ds.Tables[0].Rows[0]["SAMPLED_QTY"].ToString();
        }

        /// <summary>
        /// 取得 RC 可抽數
        /// </summary>
        /// <param name="BeSampling">總應抽數</param>
        /// <param name="Sampled">總已抽數</param>
        /// <param name="RCSampled">RC已抽數</param>
        /// <returns></returns>
        public static string GetRCBeSampling(string BeSampling, string Sampled, string RCSampled, string RC_NO)
        {
            string result = "0";

            if (int.TryParse(BeSampling, out int _beSampling) &&
                int.TryParse(Sampled, out int _sampled) &&
                int.TryParse(RCSampled, out int _rcSampled))
            {
                // 取得 RC 投入總數量
                string s = @"
SELECT
    NVL(CURRENT_QTY,0) CURRENT_QTY
FROM
    SAJET.G_RC_STATUS 
WHERE
    RC_NO = :RC_NO
";
                var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", RC_NO },
                };

                var d = ClientUtils.ExecuteSQL(s, p.ToArray());

                result = d.Tables[0].Rows[0]["CURRENT_QTY"].ToString();

                int rcTotal = int.Parse(result);

                int rcDiff = rcTotal - _rcSampled;

                if (rcDiff < rcTotal)
                {
                    result = rcDiff.ToString();
                }
                else
                {
                    result = rcTotal.ToString();
                }
            }

            return result;
        }

        /// <summary>
        /// 流程所在製程設定了幾項抽驗項目
        /// </summary>
        /// <param name="rc_no"></param>
        /// <returns></returns>
        public static int GetQCItemsCount(string rc_no)
        {
            string s = @"
SELECT
    COUNT(B.ITEM_ID) ITEMS
FROM
    SAJET.G_RC_STATUS        A,
    SAJET.SYS_PART_QC_ITEM   B
WHERE
    A.RC_NO = :RC_NO
    AND A.PART_ID = B.PART_ID
    AND A.PROCESS_ID = B.PROCESS_ID
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", rc_no },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d != null &&
                d.Tables[0].Rows.Count > 0 &&
                int.TryParse(d.Tables[0].Rows[0]["ITEMS"].ToString(), out int count))
            {
                return count;
            }
            else
            {
                return 0;
            }
        }
    }
}
