using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;

namespace RCAQL.Services
{
    /// <summary>
    /// 品保設定的商業邏輯
    /// </summary>
    internal static class QCOptionService
    {
        /// <summary>
        /// 製程有首件檢、末件檢的品保設定，必須抽驗至少一個
        /// </summary>
        /// <param name="rc_no"></param>
        /// <returns></returns>
        internal static bool MustInspect(string rc_no)
        {
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", rc_no },
            };

            string s = @"
SELECT
    B.OPTION1,
    B.OPTION2,
    A.INITIAL_QTY CURRENT_QTY
FROM
    SAJET.G_RC_STATUS          A,
    SAJET.SYS_PART_QC_OPTION   B
WHERE
    A.RC_NO = :RC_NO
    AND A.PART_ID = B.PART_ID
    AND A.PROCESS_ID = B.PROCESS_ID
    AND ( B.OPTION1 = 'Y'
          OR B.OPTION2 = 'Y' )
";
            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                // 首件檢
                bool FPI = d.Tables[0].Rows[0]["OPTION1"].ToString() == "Y";
                // 末件檢
                bool LPI = d.Tables[0].Rows[0]["OPTION2"].ToString() == "Y";

                int.TryParse(d.Tables[0].Rows[0]["CURRENT_QTY"].ToString(), out int current_qty);

                s = @"
WITH RC_INFO AS (
    SELECT
        WORK_ORDER,
        PROCESS_ID,
        NODE_ID,
        UPDATE_TIME
    FROM
        SAJET.G_RC_STATUS
    WHERE
        RC_NO = :RC_NO
)
SELECT
    COUNT(A.SN_ID) INSPECT_QTY
FROM
    SAJET.G_QC_VALUE    A,
    SAJET.G_RC_STATUS   B,
    RC_INFO             C
WHERE
    B.WORK_ORDER = C.WORK_ORDER
    AND A.RC_NO = B.RC_NO
    AND A.PROCESS_ID = C.PROCESS_ID
    AND A.NODE_ID = C.NODE_ID
";
                int inspect_qty;

                if (FPI)
                {
                    d = ClientUtils.ExecuteSQL(s, p.ToArray());

                    int.TryParse(d.Tables[0].Rows[0]["INSPECT_QTY"].ToString(), out inspect_qty);

                    if (inspect_qty <= 0)
                    {
                        //return QCOptionEnum.FirstPieceInspection;
                        return true;
                    }
                }

                if (LPI)
                {
                    s += @"
    AND A.UPDATE_TIME > C.UPDATE_TIME
";
                    d = ClientUtils.ExecuteSQL(s, p.ToArray());

                    int.TryParse(d.Tables[0].Rows[0]["INSPECT_QTY"].ToString(), out inspect_qty);

                    if (inspect_qty <= 0)
                    {
                        // 檢查是不是最後一張流程卡
                        s = @"
WITH
/* 流程卡資訊 */
 RC_INFO AS (
    SELECT
        WORK_ORDER,
        RC_NO,
        PROCESS_ID,
        NODE_ID,
        PART_ID,
        UPDATE_TIME
    FROM
        SAJET.G_RC_STATUS
    WHERE
        RC_NO = :RC_NO
),
/* 指定料號的基本資訊 */
 PART_INFO AS (
    SELECT
        A.PART_ID,
        A.ROUTE_ID
    FROM
        SAJET.SYS_PART   A,
        RC_INFO          B
    WHERE
        A.PART_ID = B.PART_ID
),
/* 預設生產途程的製程按照順序排列 */
 ROUTE_NODES AS (
    SELECT
        ROWNUM IDX,
        A.ROUTE_ID,
        A.NODE_CONTENT,
        A.NODE_ID,
        B.PART_ID
    FROM
        SAJET.SYS_RC_ROUTE_DETAIL   A,
        PART_INFO                   B
    START WITH A.ROUTE_ID = B.ROUTE_ID
               AND NODE_CONTENT = 'START' CONNECT BY PRIOR NEXT_NODE_ID = NODE_ID
                                                     OR PRIOR NEXT_NODE_ID = GROUP_ID
),/* 同工單的流程卡資訊，以及所在製程在生產途程中的相對位置，用來判斷是不是已經從此製程產出 */
 WO_RC_INFO AS (
    SELECT
        A.RC_NO,
        A.PROCESS_ID,
        A.NODE_ID,
        A.INITIAL_QTY,
        C.IDX
    FROM
        SAJET.G_RC_STATUS   A,
        RC_INFO             B,
        ROUTE_NODES         C
    WHERE
        A.WORK_ORDER = B.WORK_ORDER
        AND TO_CHAR(A.PROCESS_ID) = C.NODE_CONTENT
), RC_INFO_2 AS (
    SELECT
        A.RC_NO,
        A.IDX
    FROM
        WO_RC_INFO   A,
        RC_INFO      B
    WHERE
        A.RC_NO = B.RC_NO
)
SELECT
    SUM(A.INITIAL_QTY) TOTAL_OUTPUT_QTY
FROM
    WO_RC_INFO   A,
    RC_INFO_2    B
WHERE
    A.IDX > B.IDX
";
                        d = ClientUtils.ExecuteSQL(s, p.ToArray());

                        int.TryParse(d.Tables[0].Rows[0]["TOTAL_OUTPUT_QTY"].ToString(), out int total_output_qty);

                        // 工單目標生產量
                        s = @"
SELECT
    B.TARGET_QTY
FROM
    SAJET.G_RC_STATUS   A,
    SAJET.G_WO_BASE     B
WHERE
    A.RC_NO = :RC_NO
    AND A.WORK_ORDER = B.WORK_ORDER
";
                        d = ClientUtils.ExecuteSQL(s, p.ToArray());

                        int.TryParse(d.Tables[0].Rows[0]["TARGET_QTY"].ToString(), out int target_qty);

                        if (total_output_qty + current_qty == target_qty)
                        {
                            //return QCOptionEnum.LastPieceInspection;
                            return true;
                        }
                    }
                }
            }

            //return QCOptionEnum.None;
            return false;
        }
    }
}
