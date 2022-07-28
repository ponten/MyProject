using SajetClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CWoManager.Services
{
    /// <summary>
    /// 未分類的商業邏輯
    /// </summary>
    public static class OtherService
    {
        /// <summary>
        /// 取得資料庫現在時間
        /// </summary>
        /// <returns></returns>
        public static DateTime GetDBDateTimeNow()
        {
            string s = @"
SELECT
    SYSDATE
FROM
    DUAL
";
            var d = ClientUtils.ExecuteSQL(s);

            if (d != null &&
                d.Tables[0].Rows.Count > 0 &&
                DateTime.TryParse(d.Tables[0].Rows[0]["SYSDATE"].ToString(), out DateTime now))
            {
                return now;
            }
            else
            {
                return DateTime.Now;
            }

        }

        /// <summary>
        /// 查詢指定的工單，是否有任何投產紀錄
        /// </summary>
        /// <param name="work_order"></param>
        /// <returns></returns>
        public static bool FindProductionRecords(string work_order)
        {
            string s = @"
SELECT
    COUNT(1) TOTAL_RECORDS
FROM
    SAJET.G_RC_TRAVEL
WHERE
    WORK_ORDER = :WORK_ORDER
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", work_order },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            return d != null
                && d.Tables[0].Rows.Count > 0
                && int.TryParse(d.Tables[0].Rows[0]["TOTAL_RECORDS"].ToString(), out int total_records)
                && total_records > 0;
        }

        /// <summary>
        /// 查詢指定的工單，是否有任何流程卡紀錄
        /// </summary>
        /// <param name="work_order"></param>
        /// <returns></returns>
        public static bool FindRuncardRecords(string work_order)
        {
            string s = @"
SELECT
    SUM(AA) TOTAL_RECORDS
FROM
    (
        SELECT
            COUNT(1) AA
        FROM
            SAJET.G_RC_STATUS
        WHERE
            WORK_ORDER = :WORK_ORDER
        UNION ALL
        SELECT
            COUNT(1) AA
        FROM
            SAJET.G_RC_TRAVEL
        WHERE
            WORK_ORDER = :WORK_ORDER
    ) A
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", work_order },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            return d != null
                && d.Tables[0].Rows.Count > 0
                && int.TryParse(d.Tables[0].Rows[0]["TOTAL_RECORDS"].ToString(), out int total_records)
                && total_records > 0;
        }

        /// <summary>
        /// 取得特殊功能的權限設定
        /// </summary>
        /// <param name="EmpID"></param>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static bool Check_Privilege(string EmpID, string fun)
        {
            try
            {
                string sPrivilege = ClientUtils.GetPrivilege(EmpID, fun, ClientUtils.fProgramName).ToString();

                string sSQL = $@"
SELECT
    SAJET.SJ_PRIVILEGE_DEFINE('{fun}', '{sPrivilege}') TENABLED
FROM
    DUAL
";
                DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);

                string sRes = dsTemp.Tables[0].Rows[0]["TENABLED"].ToString();
                return (sRes == "Y");
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Function:SAJET.SJ_PRIVILEGE_DEFINE" + Environment.NewLine + ex.Message, 0);
                return false;
            }
        }

        /// <summary>
        /// 重新繪製 DataGridView。
        /// </summary>
        /// <param name="x">DataGridView</param>
        public static void RearrangeDataGridView(ref DataGridView x)
        {
            x.Update();

            x.Refresh();

            x.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }
    }
}
