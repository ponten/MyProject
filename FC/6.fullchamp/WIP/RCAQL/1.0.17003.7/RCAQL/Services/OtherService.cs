using SajetClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RCAQL.Services
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
        /// 取得報工人員 ID
        /// </summary>
        public static string GetEmpID(string emp_no)
        {
            string s = @"
SELECT
    EMP_ID
FROM
    SAJET.SYS_EMP
WHERE
    EMP_NO = :EMP_NO
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "EMP_NO", emp_no },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            string EmpID = d.Tables[0].Rows[0]["EMP_ID"].ToString();

            return EmpID;
        }

        /// <summary>
        /// 檢查特殊功能的權限設定
        /// </summary>
        /// <param name="EmpID"></param>
        /// <param name="fun"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool Check_Privilege(string EmpID, string fun, out string message)
        {
            try
            {
                message = string.Empty;

                string sPrivilege = ClientUtils.GetPrivilege(EmpID, fun, ClientUtils.fProgramName).ToString();

                string s = $@"
SELECT
    SAJET.SJ_PRIVILEGE_DEFINE(:P_1, :P_2) TENABLED
FROM
    DUAL
";
                var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "P_1", fun },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "P_2", sPrivilege },
                };

                var d = ClientUtils.ExecuteSQL(s, p.ToArray());

                return d != null
                    && d.Tables[0].Rows.Count > 0
                    && d.Tables[0].Rows[0]["TENABLED"].ToString() == "Y";
            }
            catch (Exception ex)
            {
                message
                    = "Function:SAJET.SJ_PRIVILEGE_DEFINE"
                    + Environment.NewLine
                    + ex.Message;

                SajetCommon.Show_Message(message, 0);
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
