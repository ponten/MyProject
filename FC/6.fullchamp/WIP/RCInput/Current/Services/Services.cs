using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SajetClass;
using RCInput.Enums;
using System.Data.OracleClient;
using System.Text.RegularExpressions;

namespace RCInput
{
    internal class Services
    {
        /// <summary>
        /// 爐子序號的選項要列印幾個
        /// </summary>
        private const int StoveSequenceCount = 10;

        /// <summary>
        /// 取得爐子順序號碼的下拉選單
        /// </summary>
        /// <returns></returns>
        public static List<string> GetSequencesComboBoxItems()
        {
            var result = new List<string>();

            for (int i = 1; i <= StoveSequenceCount; i++)
            {
                result.Add(i.ToString().PadLeft(2, '0'));
            }

            return result;
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

        public static string GetLastReportTime(string Runcard)
        {
            string s = @"
SELECT
    WIP_IN_TIME,
    OUT_PROCESS_TIME,
    UPDATE_TIME
FROM
    SAJET.G_RC_STATUS
WHERE
    RC_NO = :RC_NO
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", Runcard },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                var row = d.Tables[0].Rows[0];

                if (DateTime.TryParse(row["WIP_IN_TIME"].ToString(), out DateTime DtTemp))
                {
                    return DtTemp.ToString("yyyy/ MM/ dd HH: mm: ss");
                }
                else
                if (DateTime.TryParse(row["OUT_PROCESS_TIME"].ToString(), out DtTemp))
                {
                    return DtTemp.ToString("yyyy/ MM/ dd HH: mm: ss");
                }
                else
                if (DateTime.TryParse(row["UPDATE_TIME"].ToString(), out DtTemp))
                {
                    return DtTemp.ToString("yyyy/ MM/ dd HH: mm: ss");
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 取得報工人員 ID
        /// </summary>
        /// <param name="emp_no">員工編號</param>
        /// <returns></returns>
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
                new object[] { ParameterDirection.Input, OracleType.VarChar, "EMP_NO", emp_no }
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            return d.Tables[0].Rows[0]["EMP_ID"].ToString();
        }

        /// <summary>
        /// 取得流程卡的詳細資訊。
        /// </summary>
        /// <param name="Runcard"></param>
        /// <returns></returns>
        public static DataRow GetRcInfo(string Runcard)
        {
            string s = @"
SELECT
    *
FROM
    SAJET.G_RC_STATUS
WHERE
    RC_NO = :RC_NO
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", Runcard },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            return d.Tables[0].Rows[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rc_no"></param>
        /// <param name="process_id"></param>
        /// <returns></returns>
        public static List<string> GetPreviouslyUsedMachineList(string rc_no)
        {
            string s = @"
WITH WO_AND_PROCESS AS (
    SELECT
        WORK_ORDER,
        PROCESS_ID
    FROM
        SAJET.G_RC_STATUS
    WHERE
        RC_NO = :RC_NO
), MATCH_RCS AS (
    SELECT
        A.RC_NO,
        A.PROCESS_ID,
        A.OUT_PROCESS_TIME
    FROM
        SAJET.G_RC_TRAVEL   A,
        WO_AND_PROCESS      B
    WHERE
        A.WORK_ORDER = B.WORK_ORDER
        AND A.PROCESS_ID = B.PROCESS_ID
    ORDER BY
        A.OUT_PROCESS_TIME DESC
), LATEST_RC AS (
    SELECT
        RC_NO,
        PROCESS_ID
    FROM
        MATCH_RCS
    WHERE
        ROWNUM = 1
)
SELECT
    A.MACHINE_ID
FROM
    SAJET.G_RC_TRAVEL_MACHINE_DOWN   A,
    SAJET.SYS_PROCESS                B,
    SAJET.SYS_RC_ROUTE_DETAIL        C,
    LATEST_RC                        D
WHERE
    A.RC_NO = D.RC_NO
    AND D.RC_NO LIKE '10B%'
    AND ( A.NODE_ID = C.NODE_ID
          OR A.NODE_ID = C.GROUP_ID )
    AND C.NODE_CONTENT = TO_CHAR(B.PROCESS_ID)
    AND B.PROCESS_ID = D.PROCESS_ID
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", rc_no },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d != null &&
                d.Tables[0].Rows.Count > 0)
            {
                var machines = new List<string>();

                foreach (DataRow row in d.Tables[0].Rows)
                {
                    machines.Add(row["MACHINE_ID"].ToString());
                }

                return machines;
            }
            else
            {
                return new List<string>();
            }
        }

        /// <summary>
        /// 檢查指定的權限的設定值
        /// </summary>
        /// <param name="EmpID">員工 ID</param>
        /// <param name="fun">權限 / 功能名稱</param>
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
        /// 檢查投入時間設定正不正常
        /// </summary>
        /// <param name="runcard">流程卡的現在狀態的資訊</param>
        /// <param name="InProcessTime">投入時間</param>
        /// <param name="message">檢查結果</param>
        /// <returns></returns>
        public static bool IsInputTimeValid(DataRow runcard, DateTime InProcessTime, out string message)
        {
            message = string.Empty;

            DateTime now = GetDBDateTimeNow();

            if (DateTime.TryParse(runcard["OUT_PROCESS_TIME"].ToString(), out DateTime DtTemp))
            {
                if (DtTemp > InProcessTime)
                {
                    message = SajetCommon.SetLanguage(MessageEnum.TheInProcessTimeIsSetEarlierThanTheOutPutTimeOfPreviousProcess.ToString());

                    return false;
                }
            }
            else
            if (DateTime.TryParse(runcard["UPDATE_TIME"].ToString(), out DtTemp))
            {
                if (DtTemp > InProcessTime)
                {
                    message = SajetCommon.SetLanguage(MessageEnum.TheInProcessTimeIsSetEarlierThanTheLastUpdateTime.ToString());

                    return false;
                }
            }

            if (InProcessTime > now)
            {
                message = SajetCommon.SetLanguage(MessageEnum.TheInProcessTimeMustBeEarlierThanNow.ToString());

                return false;
            }

            return true;
        }

        /// <summary>
        /// 定義一個函數，作用：判斷 strNumber 是否為數字，是數字返回 True，不是數字返回 False
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public static bool IsNumeric(string strNumber)
        {   // 正浮點數 
            Regex NumberPattern = new Regex("^(([0-9]+\\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\\.[0-9]+)|([0-9]*[1-9][0-9]*))$");  // ^\\d+(\\.\\d+)?$ 非負浮點數（正浮點數 + 0）          // ^\\d+$ 非負整數（正整數 + 0）;     "^[0-9]*[1-9][0-9]*$"//正整數

            return NumberPattern.IsMatch(strNumber);
        }

        /// <summary>
        ///  判斷 strNumber 是否為指定類型的數字：
        ///  1：正整數；2：非負整數（正整數 + 0）；3：正浮點數；4：非負浮點數（正浮點數 + 0）；5：浮點數
        /// </summary>
        /// <param name="iType">數值類型</param>
        /// <param name="strNumber">判斷的字串</param>
        /// <returns>是返回True,否返回False</returns>
        public static bool IsNumeric(int iType, string strNumber)
        {
            Regex NumberPattern;

            switch (iType)
            {
                case 1:   //正整數
                    NumberPattern = new Regex("^[0-9]*[1-9][0-9]*$");
                    break;
                case 2:   //非負整數（正整數 + 0）
                    NumberPattern = new Regex("^\\d+$");
                    break;
                case 3:   //正浮點數
                    NumberPattern = new Regex("^(([0-9]+\\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\\.[0-9]+)|([0-9]*[1-9][0-9]*))$");
                    break;
                case 4:   //非負浮點數（正浮點數 + 0）
                    NumberPattern = new Regex("^\\d+(\\.\\d+)?$");
                    break;
                case 5:    //浮點數
                    NumberPattern = new Regex("^(-?\\d+)(\\.\\d+)?$");
                    break;
                default:
                    return false;
                    //break;
            }

            return NumberPattern.IsMatch(strNumber);
        }
        /// <summary>
        /// 取得流程卡的詳細資訊。
        /// </summary>
        /// <returns></returns>
        public static DataRow GetRcNoInfo(string Runcard)
        {
            string s = @"
SELECT
    *
FROM
    SAJET.G_RC_STATUS
WHERE
    RC_NO = :RC_NO
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", Runcard },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            return d.Tables[0].Rows[0];
        }
    }
}
