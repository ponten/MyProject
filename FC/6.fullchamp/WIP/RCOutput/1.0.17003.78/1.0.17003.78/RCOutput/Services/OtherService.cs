using SajetClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RCOutput.Services
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
        /// 列印防水標籤
        /// </summary>
        /// <param name="rc_no"></param>
        /// <param name="message"></param>
        public static void OKtoPrint(string rc_no, string process_id, out string message)
        {
            PrintRCLabel print = new PrintRCLabel();
            message = string.Empty;
            print.Print(RC_NO: rc_no,
                        PROCESS_ID: process_id,
                        PrintQauntity: "1",
                        sMessage: out message);
        }

        /// <summary>
        /// Analyzing System Type - Y:lot control, N:piece control
        /// </summary>
        /// <returns>true:lot control, false:piece control</returns>
        public static bool SystemType(out string system_type)
        {
            system_type = string.Empty;

            try
            {
                string sSQL = @"
SELECT
    PARAM_VALUE
FROM
    SAJET.SYS_BASE
WHERE
    PROGRAM = 'RC Manager'
AND PARAM_NAME = 'Lot Control Checked'
AND ROWNUM = 1
";
                DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);

                if (dsTemp != null)
                {
                    if (dsTemp.Tables[0].Rows.Count > 0)
                    {
                        system_type = dsTemp.Tables[0].Rows[0]["PARAM_VALUE"].ToString();

                        return system_type == "Y";
                    }
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
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
    SAJET.SJ_PRIVILEGE_DEFINE('INSERT', '{sPrivilege}') TENABLED
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
        ///  判斷strNumber是否為指定類型的數字
        ///  1:正整數, 2:非負整數（正整數 + 0）, 3:正浮點數, 4:非負浮點數（正浮點數 + 0）, 5:浮點數
        /// </summary>
        /// <param name="iType"> 數值類型 </param>
        /// <param name="strNumber">判斷的字串</param>
        /// <returns>是返回True,否返回False</returns>
        public static bool IsNumeric(int iType, string strNumber)
        {
            Regex NumberPattern;

            switch (iType)
            {
                case 1:   //正整數
                    {
                        NumberPattern = new Regex("^[0-9]*[1-9][0-9]*$");

                        break;
                    }
                case 2:   //非負整數（正整數 + 0）
                    {
                        NumberPattern = new Regex("^\\d+$");

                        break;
                    }
                case 3:   //正浮點數
                    {
                        NumberPattern = new Regex("^(([0-9]+\\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\\.[0-9]+)|([0-9]*[1-9][0-9]*))$");

                        break;
                    }
                case 4:   //非負浮點數（正浮點數 + 0）
                    {
                        NumberPattern = new Regex("^\\d+(\\.\\d+)?$");

                        break;
                    }
                case 5:    //浮點數
                    {
                        NumberPattern = new Regex("^(-?\\d+)(\\.\\d+)?$");

                        break;
                    }
                default:
                    return false;
            }

            return NumberPattern.IsMatch(strNumber);
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

        /// <summary>
        /// 取得OP 人數
        /// </summary>
        /// <param name="RC_NO"></param>
        /// <returns></returns>
        public static string GetOPCount(string sWork_order,string sPorcess_id)
        {
            string OP_CONUT = "1";
            string s = @"
select * from  sajet.G_RC_OP_LOG
where WORK_ORDER =:WORK_ORDER
and PROCESS_ID=:PROCESS_ID
order by update_time desc
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWork_order },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", sPorcess_id },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d.Tables[0].Rows.Count > 0)
            {
                 OP_CONUT = d.Tables[0].Rows[0]["op_count"].ToString();
            }
            else
            {
                OP_CONUT = "1";
            }

            return OP_CONUT;
        }

    }
}
