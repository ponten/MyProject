using QualityInspectionAAR.Enums;
using QualityInspectionAAR.Models;
using SajetClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QualityInspectionAAR.Services
{
    // 從 RCAQL 搬過來的商業邏輯
    public static partial class QCService
    {
        /// <summary>
        /// 將輸入值寫入資料表
        /// </summary>
        public static void SaveData(string emp_id, List<KeyValuePair<string, string>> s_input, string RC_NO, string NODE_ID)
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
        SAJET.G_RC_TRAVEL
    WHERE
        RC_NO = :RC_NO
        AND NODE_ID = :NODE_ID
";
                var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", RC_NO },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "NODE_ID", NODE_ID },
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
        public static string LoadAQL(string RC_NO, string PROCESS_ID)
        {
            string s = @"
SELECT
    C.SAMPLING_TYPE
FROM
    SAJET.G_RC_TRAVEL            A,
    SAJET.SYS_PART_QC_PLAN       B,
    SAJET.SYS_QC_SAMPLING_PLAN   C
WHERE
    A.RC_NO = :RC_NO
    AND A.PROCESS_ID = :PROCESS_ID
    AND A.PART_ID = B.PART_ID
    AND A.PROCESS_ID = B.PROCESS_ID
    AND B.SAMPLING_ID = C.SAMPLING_ID
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", RC_NO },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", PROCESS_ID },
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
        /// 載入檢驗項目
        /// </summary>        
        /// <param name="dgv">DataGridView</param>
        /// <param name="programInfo">流程卡、模組功能的基本資訊</param>
        public static void LoadInput(ref DataGridView dgv, ref TProgramInfo programInfo, string RC_NO, string PROCESS_ID)
        {
            string s = @"
SELECT
    A.ITEM_NAME,
    A.VALUE_DEFAULT,
    A.VALUE_TYPE,
    A.INPUT_TYPE,
    A.NECESSARY,
    A.CONVERT_TYPE,
    A.VALUE_LIST,
    A.ITEM_ID,
    B.UNIT_NO
FROM
    SAJET.SYS_PART_QC_ITEM   A,
    SAJET.SYS_UNIT           B,
    SAJET.G_RC_TRAVEL        C
WHERE
    C.RC_NO = :RC_NO
    AND A.PROCESS_ID = :PROCESS_ID
    AND A.PART_ID = C.PART_ID
    AND A.PROCESS_ID = C.PROCESS_ID
    AND A.ITEM_PHASE IN (
        'A',
        'O'
    )
    AND A.ITEM_TYPE = 1
    AND A.ENABLED = 'Y'
    AND A.UNIT_ID = B.UNIT_ID (+)
ORDER BY
    A.ITEM_SEQ
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", RC_NO },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", PROCESS_ID },
            };

            var ds = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (dgv.ColumnCount == 0)
            {
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    dgv.Columns.Add(ds.Tables[0].Columns[i].ColumnName, SajetCommon.SetLanguage(ds.Tables[0].Columns[i].ColumnName));

                    dgv.Columns[ds.Tables[0].Columns[i].ColumnName].SortMode = DataGridViewColumnSortMode.NotSortable;

                    if (i > programInfo.iInputVisible["資料收集"])
                    {
                        dgv.Columns[i].Visible = false;
                    }
                    else
                    {
                        dgv.Columns[i].ReadOnly = programInfo.iInputField["資料收集"].IndexOf(i) == -1;
                    }
                }

                dgv.Columns["UNIT_NO"].Visible = true;

                dgv.Columns["UNIT_NO"].ReadOnly = true;

                dgv.Columns["VALUE_DEFAULT"].HeaderText = SajetCommon.SetLanguage(FormTextEnum.InputValue.ToString());

            }

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dgv.Rows.Add(dr.ItemArray);

                if (dr["INPUT_TYPE"].ToString() == "S")
                {
                    DataGridViewComboBoxCell tCell = new DataGridViewComboBoxCell();

                    string[] slValue = dr["VALUE_LIST"].ToString().Split(',');

                    for (int i = 0; i < slValue.Length - 1; i++)
                    {
                        tCell.Items.Add(slValue[i]);
                    }

                    dgv["VALUE_DEFAULT", dgv.Rows.Count - 1] = tCell;
                }

                if (dr["NECESSARY"].ToString() == "Y")
                {
                    dgv.Rows[dgv.Rows.Count - 1].Cells["VALUE_DEFAULT"].Style.BackColor = Color.FromArgb(255, 255, 192);

                    dgv.Rows[dgv.Rows.Count - 1].Cells["VALUE_DEFAULT"].ErrorText = SajetCommon.SetLanguage(MessageEnum.EmptyData.ToString());
                }
            }
        }

        /// <summary>
        /// 讀取已完成輸入的資料
        /// </summary>
        /// <param name="dgv"></param>
        public static void LoadData(ref DataGridView dgv, string RC_NO, string PROCESS_ID)
        {
            var dt = new DataTable();

            string s = @"
SELECT DISTINCT
    ITEM_SEQ,
    ITEM_NAME
FROM
    SAJET.SYS_PART_QC_ITEM   A,
    SAJET.G_RC_TRAVEL        B
WHERE
    B.RC_NO = :RC_NO
    AND A.PROCESS_ID = :PROCESS_ID
    AND A.PART_ID = B.PART_ID
    AND A.PROCESS_ID = B.PROCESS_ID
ORDER BY
    ITEM_SEQ
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", RC_NO },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", PROCESS_ID },
            };

            var ds = ClientUtils.ExecuteSQL(s, p.ToArray());
            dt.Columns.Add("No");

            foreach (DataRow _item in ds.Tables[0].Rows)
            {
                dt.Columns.Add(_item["ITEM_NAME"].ToString());
            }

            dt.Columns.Add("UPDATE_TIME");
            dt.Columns.Add("EMP_NAME");

            s = $@"
SELECT
    A.SN_ID,
    C.ITEM_NAME,
    A.ITEM_VALUE,
    TO_CHAR(A.UPDATE_TIME, 'YYYY/MM/DD HH24:MI:SS') UPDATE_TIME,
    B.EMP_NAME
FROM
    SAJET.G_QC_VALUE         A,
    SAJET.SYS_EMP            B,
    SAJET.SYS_PART_QC_ITEM   C
WHERE
    A.RC_NO = :RC_NO
    AND A.PROCESS_ID = :PROCESS_ID
    AND A.PROCESS_ID = C.PROCESS_ID
    AND A.ITEM_ID = C.ITEM_ID
    AND C.ITEM_PHASE IN (
        'A',
        'O'
    )
    AND A.UPDATE_USERID = B.EMP_ID
ORDER BY
    A.UPDATE_TIME
";
            p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", RC_NO },
                new object[] { ParameterDirection.Input, OracleType.Number, "PROCESS_ID", PROCESS_ID },
            };
            ds = ClientUtils.ExecuteSQL(s, p.ToArray());

            string _SN_ID = string.Empty;
            DataRow _newRow = null;
            int i = 1;

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                if (item["SN_ID"].ToString() != _SN_ID)
                {
                    if (_newRow != null)
                    {
                        dt.Rows.Add(_newRow);
                    }
                    _newRow = dt.NewRow();
                    _SN_ID = item["SN_ID"].ToString();
                    _newRow["No"] = i++;
                    _newRow["UPDATE_TIME"] = item["UPDATE_TIME"].ToString();
                    _newRow["EMP_NAME"] = item["EMP_NAME"];
                }

                string _item_name = item["ITEM_NAME"].ToString();
                _newRow[_item_name] = item["ITEM_VALUE"].ToString();
            }

            // 加入最後一筆資料
            if (_newRow != null)
            {
                dt.Rows.Add(_newRow);
            }

            dgv.DataSource = dt;

            foreach (DataGridViewColumn column in dgv.Columns)
            {
                column.HeaderText = SajetCommon.SetLanguage(column.Name);
            }
        }

        /// <summary>
        /// 取得流程卡的工單號碼
        /// </summary>
        /// <returns></returns>
        public static string GetWO(string RC_NO)
        {
            string s = $@"
SELECT
    WORK_ORDER,
    PROCESS_ID
FROM
    SAJET.G_RC_TRAVEL
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
                return d.Tables[0].Rows[0]["WORK_ORDER"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 計算RC所屬工單的目前RC製程總數
        /// </summary>
        /// <returns></returns>
        public static string SumTotal(string RC_NO, string PROCESS_ID)
        {
            string s = @"
WITH RC_INFO AS (
    SELECT
        WORK_ORDER,
        PROCESS_ID
    FROM
        SAJET.G_RC_TRAVEL
    WHERE
        RC_NO = :RC_NO
        AND PROCESS_ID = :PROCESS_ID
        AND ROWNUM = 1
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
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", PROCESS_ID },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            return d.Tables[0].Rows[0]["WIP_IN_QTY"].ToString();
        }

        /// <summary>
        /// 取得所有資料收集筆數
        /// </summary>
        /// <returns></returns>
        public static string GetTotalData(string RC_NO, string PROCESS_ID)
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
        SAJET.G_RC_TRAVEL
    WHERE
        RC_NO = :RC_NO
        AND PROCESS_ID = :PROCESS_ID
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
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", PROCESS_ID },
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
        public static string GetTotalBeSampling(string TotalNum, string RC_NO, string PROCESS_ID)
        {
            if (string.IsNullOrEmpty(TotalNum))
            {
                TotalNum = "0";
            }

            string s = $@"
SELECT
    C.SAMPLE_SIZE
FROM
    SAJET.G_RC_TRAVEL                   A,
    SAJET.SYS_PART_QC_PLAN              B,
    SAJET.SYS_QC_SAMPLING_PLAN_DETAIL   C
WHERE
    A.RC_NO = :RC_NO
    AND A.PROCESS_ID = :PROCESS_ID
    AND A.PART_ID = B.PART_ID
    AND A.PROCESS_ID = B.PROCESS_ID
    AND B.SAMPLING_ID = C.SAMPLING_ID
    AND C.SAMPLING_LEVEL = '0'
    AND C.MAX_LOT_SIZE >= :NUM
    AND C.MIN_LOT_SIZE <= :NUM
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", RC_NO },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", PROCESS_ID },
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
        public static string GetRCSampled(string RC_NO, string PROCESS_ID)
        {
            string sSQL = $@"
WITH A AS (
    SELECT DISTINCT
        ( SN_ID ) SN_ID
    FROM
        SAJET.G_QC_VALUE    A,
        SAJET.G_RC_TRAVEL   B
    WHERE
        B.RC_NO = :RC_NO
        AND B.PROCESS_ID = :PROCESS_ID
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
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", PROCESS_ID },
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
        public static string GetRCBeSampling(string BeSampling, string Sampled, string RCSampled, string RC_NO, string PROCESS_ID)
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
    SAJET.G_RC_TRAVEL
WHERE
    RC_NO = :RC_NO
    AND PROCESS_ID = :PROCESS_ID
";
                var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", RC_NO },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", PROCESS_ID },
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
    }
}
