using RCIPQC.References;
using SajetClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.Windows.Forms;

namespace RCIPQC.Services
{
    /// <summary>
    /// 品保資料收集的商業邏輯，參考自 RCAQL
    /// </summary>
    internal static class QCService
    {
        /// <summary>
        /// 流程卡號碼
        /// </summary>
        public static string RC_NO { get; set; }

        /// <summary>
        /// 工單號碼
        /// </summary>
        public static string WORK_ORDER { get; set; }

        /// <summary>
        /// 製程 ID
        /// </summary>
        public static string PROCESS_ID { get; set; }

        /// <summary>
        /// 載入檢驗項目的規則
        /// </summary>        
        /// <param name="dgv"></param>
        public static void LoadCondition(ref DataGridView dgv)
        {
            string s = @"
SELECT
    A.ITEM_NAME,
    A.VALUE_DEFAULT,
    A.VALUE_TYPE,
    B.UNIT_NO
FROM
    SAJET.SYS_PART_QC_ITEM   A,
    SAJET.SYS_UNIT           B,
    SAJET.G_RC_STATUS        C
WHERE
    C.RC_NO = :RC_NO
    AND A.PART_ID = C.PART_ID
    AND A.PROCESS_ID = C.PROCESS_ID
    AND A.ITEM_PHASE IN (
        'A',
        'O'
    )
    AND A.ITEM_TYPE = 0
    AND A.ENABLED = 'Y'
    AND A.UNIT_ID = B.UNIT_ID (+)
ORDER BY
    A.ITEM_SEQ
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", RC_NO },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            dgv.DataSource = d;

            dgv.DataMember = d.Tables[0].ToString();

            foreach (DataGridViewColumn column in dgv.Columns)
            {
                column.HeaderText = SajetCommon.SetLanguage(column.Name);
            }

            dgv.Columns["VALUE_TYPE"].Visible = false;
        }

        /// <summary>
        /// 載入檢驗項目
        /// </summary>        
        /// <param name="dgv">DataGridView</param>
        /// <param name="programInfo">流程卡、模組功能的基本資訊</param>
        public static void LoadInput(ref DataGridView dgv, ref TProgramInfo programInfo)
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
    SAJET.SYS_UNIT           B
WHERE
    A.PART_ID = :PART_ID
    AND A.PROCESS_ID = :PROCESS_ID
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
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", programInfo.dsRC.Tables[0].Rows[0]["RC_NO"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", programInfo.dsRC.Tables[0].Rows[0]["PART_ID"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", programInfo.dsRC.Tables[0].Rows[0]["PROCESS_ID"].ToString() },
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

                dgv.Columns["VALUE_DEFAULT"].HeaderText = SajetCommon.SetLanguage("Input value");

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

                    dgv.Rows[dgv.Rows.Count - 1].Cells["VALUE_DEFAULT"].ErrorText = SajetCommon.SetLanguage("Data Empty");
                }
            }
        }

        /// <summary>
        /// 將輸入值寫入資料表
        /// </summary>
        public static void SaveData(string emp_id, List<KeyValuePair<string, string>> s_input)
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

            s = @"
SELECT
    NODE_ID
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

            d = ClientUtils.ExecuteSQL(s, p.ToArray());

            string NODE_ID = d.Tables[0].Rows[0]["NODE_ID"].ToString();

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
    INSPECTION_TIME,
    UPDATE_TIME
) VALUES (
    :RC_NO,
    :PROCESS_ID,
    :NODE_ID,
    :SN_ID,
    :ITEM_ID,
    :ITEM_VALUE,
    :UPDATE_USERID,
    :UPDATE_TIME,
    :UPDATE_TIME
)";
                p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", RC_NO },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", PROCESS_ID },
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
        /// 讀取已完成輸入的資料
        /// </summary>
        /// <param name="dgv"></param>
        public static void LoadData(ref DataGridView dgv)
        {
            var dt = new DataTable();

            string s = @"
SELECT DISTINCT
    ITEM_SEQ,
    ITEM_NAME
FROM
    SAJET.SYS_PART_QC_ITEM   A,
    SAJET.G_RC_STATUS        B
WHERE
    B.RC_NO = :RC_NO
    AND A.PART_ID = B.PART_ID
    AND A.PROCESS_ID = B.PROCESS_ID
ORDER BY
    ITEM_SEQ
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", RC_NO },
            };

            var ds = ClientUtils.ExecuteSQL(s, p.ToArray());
            dt.Columns.Add("No");

            foreach (DataRow _item in ds.Tables[0].Rows)
            {
                dt.Columns.Add(_item["ITEM_NAME"].ToString());
            }

            dt.Columns.Add("INSPECTION_TIME");
            dt.Columns.Add("EMP_NAME");

            s = $@"
SELECT
    A.SN_ID,
    C.ITEM_NAME,
    A.ITEM_VALUE,
    TO_CHAR(A.INSPECTION_TIME, 'YYYY/MM/DD HH24:MI:SS') INSPECTION_TIME,
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
    A.INSPECTION_TIME
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
                    _newRow["INSPECTION_TIME"] = item["INSPECTION_TIME"].ToString();
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

            dgv.Columns[0].Frozen = true;
        }

        /// <summary>
        /// 完全照著資料表呈現
        /// </summary>
        /// <param name="dgv"></param>
        public static void LoadRawData(ref DataGridView dgv)
        {
            string s = @"
SELECT
    A.SN_ID,
    B.ITEM_NAME,
    B.ITEM_VALUE
FROM
    SAJET.G_QC_VALUE         A,
    SAJET.SYS_PART_QC_ITEM   B
WHERE
    A.RC_NO = :RC_NO
    AND A.ITEM_ID = B.ITEM_ID
    AND B.ITEM_PHASE IN (
        'A',
        'O'
    )
ORDER BY
    A.SN_ID DESC
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", RC_NO },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            dgv.DataSource = d.Tables[0];
        }

        /// <summary>
        /// 讀取流程卡的 AQL 規則
        /// </summary>
        public static string LoadAQL()
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
    AND B.ENABLED = 'Y'
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
        public static string GetWONumber()
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
                WORK_ORDER = d.Tables[0].Rows[0]["WORK_ORDER"].ToString();
                PROCESS_ID = d.Tables[0].Rows[0]["PROCESS_ID"].ToString();
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
        public static string SumTotal()
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
        public static string GetTotalData()
        {
            string Result = string.Empty;

            if (!string.IsNullOrEmpty(WORK_ORDER))
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
        public static string GetTotalBeSampling(string TotalNum)
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
        public static string GetRCSampled()
        {
            string sSQL = $@"
WITH A AS (
    SELECT DISTINCT
        ( SN_ID ) SN_ID
    FROM
        SAJET.G_QC_VALUE
    WHERE
        RC_NO = :RC_NO
        AND PROCESS_ID = :PROCESS_ID
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
        public static string GetRCBeSampling(string BeSampling, string Sampled, string RCSampled)
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
        /// 流程卡是否已經檢驗過了
        /// </summary>
        /// <param name="rc_no">流程卡號</param>
        /// <param name="process_id">製程 ID</param>
        /// <param name="inspector_info">檢驗員的資訊</param>
        /// <returns></returns>
        public static bool IsRcInspected(string rc_no, string process_id, out DataRow inspector_info)
        {
            string s = @"
SELECT
    A.RC_NO,
    A.PART_ID,
    A.PROCESS_ID,
    A.NODE_ID,
    A.QC_EMP_ID,
    B.EMP_NO,
    B.EMP_NAME,
    TO_CHAR(A.UPDATE_TIME, 'YYYY/ MM/ DD HH24: MI: SS')  UPDATE_TIME
FROM
    SAJET.G_RC_QC_LOG   A,
    SAJET.SYS_EMP       B
WHERE
    A.RC_NO = :RC_NO
    AND A.PROCESS_ID = :PROCESS_ID
    AND A.QC_EMP_ID = B.EMP_ID
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", rc_no },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", process_id },
            };

            DataSet d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                inspector_info = d.Tables[0].Rows[0];

                return true;
            }
            else
            {
                inspector_info = null;

                return false;
            }
        }

        /// <summary>
        /// 標記為已經完成檢驗
        /// </summary>
        /// <param name="rc_no"></param>
        /// <param name="part_id"></param>
        /// <param name="process_id"></param>
        /// <param name="inspect_time"></param>
        public static void MarkAsInspected(
            string rc_no,
            string part_id,
            string process_id,
            DateTime inspect_time)
        {
            string s = @"
INSERT INTO SAJET.G_RC_QC_LOG (
    RC_NO,
    PART_ID,
    PROCESS_ID,
    NODE_ID,
    QC_EMP_ID,
    UPDATE_TIME
) VALUES (
    :RC_NO,
    :PART_ID,
    :PROCESS_ID,
    0,
    :QC_EMP_ID,
    :UPDATE_TIME
)";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", rc_no },
                new object[] { ParameterDirection.Input, OracleType.Number, "PART_ID", part_id },
                new object[] { ParameterDirection.Input, OracleType.Number, "PROCESS_ID", process_id },
                new object[] { ParameterDirection.Input, OracleType.Number, "QC_EMP_ID", ClientUtils.UserPara1 },
                new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", inspect_time },
            };

            ClientUtils.ExecuteSQL(s, p.ToArray());
        }

        /// <summary>
        /// 檢驗標記使用的 SQL 指令。查詢參數需要兩個：流程卡號 與 設定的製程 ID。
        /// </summary>
        /// <returns></returns>
        public static string GetSQLCommand()
        {
            string s = @"
WITH RC_LIST AS (
    SELECT
        A.WORK_ORDER,
        A.RC_NO,
        A.PART_ID,
        A.VERSION,
        A.ROUTE_ID,
        A.PROCESS_ID,
        A.FACTORY_ID,
        A.CURRENT_QTY
    FROM
        SAJET.G_RC_STATUS   A,
        SAJET.SYS_PROCESS   B
    WHERE
        A.RC_NO = :SN
        AND B.OPERATE_ID = 5
        AND B.ENABLED = 'Y'
        AND A.PROCESS_ID = B.PROCESS_ID
    ORDER BY
        A.UPDATE_TIME DESC
), RC_INFO AS (
    SELECT
        *
    FROM
        RC_LIST
    WHERE
        ROWNUM = 1
)
SELECT
    A.RC_NO,
    A.WORK_ORDER,
    B.PART_NO,
    ROUTE_NAME,
    PROCESS_NAME,
    FACTORY_CODE,
    A.VERSION,
    A.PROCESS_ID,
    A.PART_ID,
    CURRENT_QTY   QTY,
    CURRENT_QTY   GOOD_QTY,
    0 SCRAP_QTY,
    F.WO_OPTION2,
    B.SPEC1,
    B.SPEC2
FROM
    RC_INFO              A,
    SAJET.SYS_PART       B,
    SAJET.SYS_RC_ROUTE   C,
    SAJET.SYS_PROCESS    D,
    SAJET.SYS_FACTORY    E,
    SAJET.G_WO_BASE      F
WHERE
    A.PART_ID = B.PART_ID
    AND A.ROUTE_ID = C.ROUTE_ID
    AND D.PROCESS_ID = :PROCESS_ID
    AND A.FACTORY_ID = E.FACTORY_ID
    AND A.WORK_ORDER = F.WORK_ORDER
";
            return s;
        }
    }
}
