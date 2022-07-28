using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RCProcessParam.Services
{
    internal static class OtherService
    {
        /// <summary>
        /// 顯示抽驗計畫的細節
        /// </summary>
        /// <param name="SamplingType"></param>
        internal static void ShowSamplingPlanDetail(string SamplingType = "")
        {
            string s = @"
SELECT
    A.MIN_LOT_SIZE,
    A.MAX_LOT_SIZE,
    A.SAMPLE_SIZE,
    A.CRITICAL_REJECT_QTY,
    A.MAJOR_REJECT_QTY,
    A.MINOR_REJECT_QTY
FROM
    SAJET.SYS_QC_SAMPLING_PLAN_DETAIL   A,
    SAJET.SYS_QC_SAMPLING_PLAN          B,
    SAJET.SYS_UNIT                      C
WHERE
    B.SAMPLING_TYPE = :SAMPLING_TYPE
    AND A.SAMPLING_LEVEL = 0
    AND A.SAMPLING_ID = B.SAMPLING_ID
    AND A.SAMPLING_UNIT = C.UNIT_ID (+)
ORDER BY
    1,
    2
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "SAMPLING_TYPE", SamplingType },
            };

            using (var f = new SajetFilter.FFilter(sqlCommand: s, @params: p, readOnly: true))
            {
                f.Text = SajetClass.SajetCommon.SetLanguage("Sampling plan detail");

                f.Width = 600;

                f.ShowDialog();
            }
        }

        /// <summary>
        /// 取得製程 ID
        /// </summary>
        /// <param name="process_name"></param>
        /// <returns></returns>
        internal static string GetProcessID(string process_name)
        {
            string s = @"
SELECT
    PROCESS_ID
FROM
    SAJET.SYS_PROCESS
WHERE
    TRIM(PROCESS_NAME) = TRIM(:PROCESS_NAME)
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_NAME", process_name.Trim() },
            };

            DataSet DS = ClientUtils.ExecuteSQL(s, p.ToArray()); ;

            if (DS.Tables[0].Rows.Count > 0)
            {
                return DS.Tables[0].Rows[0]["PROCESS_ID"].ToString();
            }

            return "0";
        }

        internal static DataSet GetPartInfo(string part_id)
        {
            string s = $@"
SELECT
    T.VERSION,
    T.OPTION2 FORMER_PART_NO,
    T.SPEC1,
    T.SPEC2,
    T.OPTION4 BLUEPRINT
FROM
    SAJET.SYS_PART T
WHERE
    T.PART_ID = :PART_ID
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.Number, "PART_ID", part_id },
            };

            DataSet d = ClientUtils.ExecuteSQL(s, p.ToArray());

            return d;
        }

        internal static void LoadPartInfo(ref DataGridView dgv, DataSet d)
        {
            dgv.DataSource = d;

            dgv.DataMember = d.Tables[0].ToString();

            foreach (DataGridViewColumn column in dgv.Columns)
            {
                column.HeaderText = SajetClass.SajetCommon.SetLanguage(column.Name);

                column.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            }

            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        /// <summary>
        /// 測量，並回傳顯示下拉選單內容所需的寬度
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        internal static int CalculateDropDownWidth(ComboBox c)
        {
            int maxWidth = 0, temp = 0;

            foreach (var obj in c.Items)
            {
                temp = TextRenderer.MeasureText(obj.ToString(), c.Font).Width;

                if (temp > maxWidth)
                {
                    maxWidth = temp;
                }
            }

            if (maxWidth < c.Width)
            {
                maxWidth = c.Width;
            }

            return maxWidth;
        }

        /// <summary>
        /// 檢查料號使用的生產途程是否是啟用的
        /// </summary>
        /// <param name="part_id"></param>
        /// <returns></returns>
        internal static bool CheckForRouteEnable(string part_id, out string route_name)
        {
            route_name = "";

            string s = @"
SELECT
    ROUTE_NAME,
    ENABLED
FROM
    SAJET.SYS_RC_ROUTE
WHERE
    ROUTE_ID = (
        SELECT
            ROUTE_ID
        FROM
            SAJET.SYS_PART
        WHERE
            PART_ID = :PART_ID
    )
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.Number, "PART_ID", part_id },
            };

            DataSet d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d != null &&
                d.Tables[0].Rows.Count > 0)
            {
                route_name = d.Tables[0].Rows[0]["ROUTE_NAME"].ToString();
            }

            return d != null
                && d.Tables[0].Rows.Count > 0
                && d.Tables[0].Rows[0]["ENABLED"].ToString() == "Y";
        }
    }
}
