using RCOutput_WO.Enums;
using RCOutput_WO.Models;
using SajetClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;

namespace RCOutput_WO.Services
{
    public static class T4Service
    {

        /// <summary>
        /// 檢查流程卡所在製程有沒有使用到 T4 爐 或 T6 爐
        /// </summary>
        /// <param name="rc_no">流程卡號</param>
        /// <param name="messages">檢查結果</param>
        /// <returns></returns>
        public static bool CheckIfUsingT4orT6(string rc_no, out List<string> messages)
        {
            messages = new List<string>();

            string s = @"
SELECT
    a.process_id,
    b.option1,
    b.option2,
    b.option3
FROM
    sajet.g_rc_status          a,
    sajet.sys_process_option   b
WHERE
    rc_no = :rc_no
    AND a.process_id = b.process_id
    AND ( b.option1 = 'Y'
            OR b.option2 = 'Y'
              OR b.option3 = 'Y' )
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "rc_no", rc_no },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d != null &&
                d.Tables[0].Rows.Count > 0)
            {
                // T4: OPTION2
                if (d.Tables[0].Rows[0]["OPTION2"].ToString() == "Y")
                {
                    messages.Add(SajetCommon.SetLanguage(MessageEnum.HasUseT4.ToString()));

                    return true;
                }

                // T6: OPTION3
                if (d.Tables[0].Rows[0]["OPTION3"].ToString() == "Y")
                {
                    messages.Add(SajetCommon.SetLanguage(MessageEnum.HasUseT6.ToString()));

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 檢查 日期碼 和 爐序
        /// </summary>
        /// <param name="runcards">流程卡資料清單</param>
        /// <param name="usingT4OrT6stove">製程是否使用到 T4 爐 或 T6 爐</param>
        /// <param name="message">檢查結果</param>
        /// <returns></returns>
        public static bool CheckForDateCodeAndStoveSeq(
            ref List<RC_DETAILS> runcards,
            bool usingT4OrT6stove,
            out string message)
        {
            message = string.Empty;

            var dateCodeNotPass = new List<string>();

            //var stoveSeqNotSet = new List<string>();

            //var notCollectTwo = new List<string>();

            foreach (var rc in runcards)
            {
                foreach (var machine in rc.Machines.Where(x => x.Select))
                {
                    // 機台代碼
                    string machineCode = machine.MACHINE_CODE;

                    // 日期碼
                    string date_code = machine.DATE_CODE?.ToString().Trim() ?? "";

                    // 爐序號
                    //string stoveSeq = machine.STOVE_SEQ?.ToString().Trim() ?? "";

                    if (usingT4OrT6stove || !string.IsNullOrEmpty(date_code))
                    {
                        if (!long.TryParse(date_code, out long l_date_code)
                            || l_date_code.ToString().Length != 12)
                        {
                            dateCodeNotPass.Add(machineCode);
                        }
                    }
                }
            }

            if (dateCodeNotPass.Any())
            {
                message = SajetCommon.SetLanguage("Stove sequence required 12 digits number");

                return false;
            }

            return true;
        }

        /// <summary>
        /// 檢查爐號
        /// </summary>
        /// <param name="runcards">流程卡資料清單</param>
        /// <param name="usingT4OrT6stove">製程是否使用到 T4 爐 或 T6 爐</param>
        /// <param name="message">檢查結果</param>
        /// <returns></returns>
        public static bool CheckForDateCode(
            ref List<RC_DETAILS> runcards,
            bool usingT4OrT6stove,
            out string message)
        {
            message = string.Empty;

            var machineNotPass = new List<string>();

            foreach (var rc in runcards)
            {
                var dateCodeNoPass = new List<string>();

                foreach (var machine in rc.Machines.Where(x => x.Select))
                {
                    // 機台代碼
                    string machineCode = machine.MACHINE_CODE;

                    // 日期碼
                    string date_code = machine.DATE_CODE?.ToString().Trim() ?? "";

                    if (usingT4OrT6stove || !string.IsNullOrEmpty(date_code))
                    {
                        if (!long.TryParse(date_code, out long l_date_code)
                            || l_date_code.ToString().Length != 12)
                        {
                            dateCodeNoPass.Add(machineCode);
                        }
                    }
                }

                if (dateCodeNoPass.Any())
                {
                    message
                        = SajetCommon.SetLanguage(MessageEnum.Runcard.ToString())
                        + ":" + rc.RC_NO
                        + Environment.NewLine
                        + SajetCommon.SetLanguage("Machine code")
                        + ":" + string.Join(", ", dateCodeNoPass)
                        ;

                    machineNotPass.Add(message);

                    message = string.Empty;
                }
            }

            if (machineNotPass.Any())
            {
                message
                    = SajetCommon.SetLanguage("Stove sequence required 12 digits number")
                    + Environment.NewLine
                    + string.Join(Environment.NewLine, machineNotPass)
                    ;

                return false;
            }

            return true;
        }
    }
}
