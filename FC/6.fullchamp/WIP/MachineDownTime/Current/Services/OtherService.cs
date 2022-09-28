using MachineDownTime.Enums;
using SajetClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MachineDownTime.Services
{
    /// <summary>
    /// 未歸類的業務邏輯
    /// </summary>
    internal static class OtherService
    {
        /// <summary>
        /// 取得作業人員的員工 ID
        /// </summary>
        /// <param name="emp_no"></param>
        /// <param name="emp_id"></param>
        /// <returns></returns>
        internal static bool GetEmpID(string emp_no, out int emp_id)
        {
            emp_id = 0;

            string s = @"
SELECT
    EMP_ID
FROM
    SAJET.SYS_EMP
WHERE
    ENABLED = 'Y'
    AND EMP_NO = :EMP_NO
";
            var p = new object[1][];
            p[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "EMP_NO", emp_no };
            DataSet d = ClientUtils.ExecuteSQL(s, p);

            if (d != null &&
                d.Tables[0].Rows.Count > 0 &&
                int.TryParse(d.Tables[0].Rows[0]["EMP_ID"].ToString(), out emp_id))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 取得資料庫的現在時間
        /// </summary>
        /// <returns></returns>
        internal static DateTime GetDBDateTimeNow()
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
        /// 判斷指定功能的使用權限
        /// </summary>
        /// <param name="emp_id">員工 ID</param>
        /// <param name="function_name">功能名稱</param>
        /// <param name="operation">操作類型</param>
        /// <returns></returns>
        internal static bool Check_Privilege(string emp_id, string function_name, OperationEnum operation)
        {
            try
            {
                string sPrivilege
                    = ClientUtils.GetPrivilege(emp_id, function_name, ClientUtils.fProgramName).ToString();

                string s = @"
SELECT
    SAJET.SJ_PRIVILEGE_DEFINE(:OPERATION, :PRIVILEGE) TENABLED
FROM
    DUAL
";
                var p = new object[2][];
                p[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPERATION", operation.ToString().ToUpper() };
                p[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PRIVILEGE", sPrivilege };
                DataSet d = ClientUtils.ExecuteSQL(s, p);

                return d != null
                    && d.Tables[0].Rows.Count > 0
                    && d.Tables[0].Rows[0]["TENABLED"].ToString() == "Y";
            }
            catch (Exception ex)
            {
                string message
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

            x.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            x.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }


        public static void GetNowAndBeforeProcess(string rc_no, out DataSet ds)
        {
            ds = new DataSet();
            string sSQL = $"select sajet.F_GET_PAST_PROCESS(r.route_id, r.process_id) proc from sajet.g_rc_status r where r.rc_no='{rc_no}' and rownum =1";
            string sProcess = default;
            using (DataTable dt = ClientUtils.ExecuteSQL(sSQL).Tables[0])
            {
                sProcess = Convert.ToString( dt.Rows[0][0] );
            }

            sProcess = string.Join("','", sProcess.Split(','));

            sSQL = $"select process_id, process_code, process_name from sajet.sys_process r where r.process_id in ('{sProcess}')";
            ds = ClientUtils.ExecuteSQL(sSQL);
        }
    }
}
