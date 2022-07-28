using RCInput_WO.Enums;
using SajetClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;

namespace RCInput_WO.Services
{
    /// <summary>
    /// 使用 T4 / T6 爐製程的商業邏輯
    /// </summary>
    public static class T4Service
    {
        /// <summary>
        /// 爐子序號的選項要列印幾個
        /// </summary>
        private const int StoveSequenceCount = 99;

        /// <summary>
        /// 取得爐子順序號碼的下拉選單
        /// </summary>
        /// <returns></returns>
        public static List<string> GetSequencesComboBoxItems()
        {
            var result = new List<string>();

            result.Add("");

            for (int i = 1; i <= StoveSequenceCount; i++)
            {
                result.Add(i.ToString().PadLeft(2, '0'));
            }

            return result;
        }

        /// <summary>
        /// 檢查流程卡所在製程有沒有使用到 T4 爐 或 T6 爐
        /// </summary>
        /// <param name="rc_no">流程卡號</param>
        /// <param name="messages">檢查結果</param>
        /// <returns></returns>
        public static bool CheckIfUsingT4orT6(string rc_no, ref List<string> messages)
        {
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
    a.process_id = b.process_id
    AND ( b.option1 = 'Y'
            OR b.option2 = 'Y'
              OR b.option3 = 'Y' )
    AND rc_no = :rc_no
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
    }
}
