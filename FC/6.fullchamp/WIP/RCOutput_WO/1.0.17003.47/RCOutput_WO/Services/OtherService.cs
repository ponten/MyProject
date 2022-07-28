using RCOutput_WO.Models;
using SajetClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RCOutput_WO.Services
{
    /// <summary>
    /// 未分類的商業邏輯
    /// </summary>
    public static class OtherService
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

            for (int i = 1; i <= StoveSequenceCount; i++)
            {
                result.Add(i.ToString().PadLeft(2, '0'));
            }

            return result;
        }

        /// <summary>
        /// 取得報工人員 ID
        /// </summary>
        public static string GetEmpID(string EmpNO)
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
                new object[] { ParameterDirection.Input, OracleType.VarChar, "EMP_NO", EmpNO }
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            return d.Tables[0].Rows[0]["EMP_ID"].ToString();
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
                SajetClass.SajetCommon.Show_Message("Function:SAJET.SJ_PRIVILEGE_DEFINE" + Environment.NewLine + ex.Message, 0);
                return false;
            }

        }

        /// <summary>
        /// 取得使用中的機台的清單。
        /// </summary>
        /// <param name="RC_NO">指定流程卡號</param>
        /// <returns></returns>
        public static DataSet GetMachineList(string RC_NO)
        {
            string s = @"
SELECT
    SM.MACHINE_ID
   ,SM.MACHINE_CODE
   ,SM.MACHINE_DESC
   ,SMS.STATUS_NAME
   ,COALESCE(RTMD.START_TIME, GRS.WIP_IN_TIME, GRS.UPDATE_TIME) ""START_TIME""
   ,SMS.RUN_FLAG
   ,SMS.DEFAULT_STATUS
   ,RTMD.LOAD_QTY
   ,RTMD.DATE_CODE
   ,RTMD.STOVE_SEQ
FROM
    SAJET.G_RC_STATUS              GRS
   ,SAJET.G_RC_TRAVEL_MACHINE      GRTM
   ,SAJET.G_RC_TRAVEL_MACHINE_DOWN RTMD
   ,SAJET.G_MACHINE_STATUS         GMS
   ,SAJET.SYS_MACHINE              SM
   ,SAJET.SYS_MACHINE_STATUS       SMS
WHERE
    GRTM.TRAVEL_ID = GRS.TRAVEL_ID
AND GRTM.RC_NO = GRS.RC_NO
AND GRS.RC_NO = :RC_NO

AND GRTM.MACHINE_ID = SM.MACHINE_ID
AND SM.ENABLED = 'Y'

AND GRTM.MACHINE_ID = GMS.MACHINE_ID

AND GMS.CURRENT_STATUS_ID = SMS.STATUS_ID

AND GRTM.RC_NO = RTMD.RC_NO(+)
AND GRTM.TRAVEL_ID = RTMD.TRAVEL_ID(+)
AND GRTM.MACHINE_ID = RTMD.MACHINE_ID
AND RTMD.END_TIME IS NULL

ORDER BY
    SM.MACHINE_CODE
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", RC_NO }
            };

            //ds = ClientUtils.ExecuteSQL(programInfo.sSQL["機台"], Params.ToArray());
            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            return d;
        }

        /// <summary>
        /// DataSet 轉換成資料模型。
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static List<MachineDownModel> GetModels(DataSet d)
        {
            return d.Tables[0].Rows
                .Cast<DataRow>()
                .Select(row => new MachineDownModel
                {
                    Select = true,
                    MACHINE_ID = int.Parse(row["MACHINE_ID"].ToString()),
                    MACHINE_CODE = row["MACHINE_CODE"].ToString(),
                    MACHINE_DESC = $"[{row["MACHINE_CODE"]}] {row["MACHINE_DESC"]}",
                    STATUS_NAME = row["STATUS_NAME"].ToString(),
                    START_TIME = DateTime.Parse(row["START_TIME"].ToString()),
                    LOAD_QTY = int.Parse(row["LOAD_QTY"].ToString()),
                    DATE_CODE = row["DATE_CODE"].ToString(),
                    STOVE_SEQ = row["STOVE_SEQ"].ToString(),
                }).ToList();
        }

        internal static DataSet GetProcessMachineList(string processID)
        {
            string s = @"
SELECT
    *
FROM
    sajet.sys_rc_process_machine
WHERE
    process_id = :process_id
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.Number, "process_id", processID }
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            return d;
        }

        /// <summary>
        /// 取得同批執行的所有流程卡的資訊
        /// </summary>
        /// <param name="rc_no"></param>
        /// <returns></returns>
        public static DataSet GetRelatedRuncard(string rc_no, int report_type)
        {
            string s = @"
SELECT DISTINCT
    d.rc_no,
    d.travel_id,
    d.node_id
FROM
    sajet.g_rc_status                a,
    sajet.g_rc_travel_machine_down   b,
    sajet.g_rc_status                c,
    sajet.g_rc_travel_machine_down   d
WHERE
    a.rc_no = :rc_no
    AND a.rc_no = b.rc_no
    AND a.node_id = b.node_id
    AND b.report_type = :report_type
    AND a.work_order = c.work_order
    AND c.rc_no = d.rc_no
    AND c.node_id = d.node_id
    AND d.report_type = :report_type
    AND b.machine_id = d.machine_id
    AND b.start_time = d.start_time
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "rc_no", rc_no },
                new object[] { ParameterDirection.Input, OracleType.Number, "report_type", report_type },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            return d;
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
        /// 從製程配置資料表搜尋此製程是否有設定RC Input
        /// </summary>
        /// <param name="process_id">製程 ID</param>
        public static bool ProcessSheet(string process_id)
        {
            try
            {
                string s = @"
SELECT
    PROCESS_ID
FROM
    SAJET.SYS_RC_PROCESS_SHEET
WHERE
    PROCESS_ID = :PROCESS_ID
    AND SHEET_NAME = 'RC Input'
";
                var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.Number, "PROCESS_ID", process_id },
                };

                DataSet ds = ClientUtils.ExecuteSQL(s, p.ToArray());

                return (ds == null || ds.Tables[0].Rows.Count == 0);
            }
            catch (Exception)
            {
                return true;
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
            Regex NumberPattern = null;
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
    }
}
