using RCOutput.Models;
using SajetClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using OtSrv = RCOutput.Services.OtherService;

namespace RCOutput.Services
{
    /// <summary>
    /// 換班的商業邏輯
    /// </summary>
    public static class ShiftService
    {
        /// <summary>
        /// 如果執行流程卡投入的人員與現在再執行流程卡產出的人員不是同一個，
        /// 代表流程卡加工期間有換班，可以設定換班時間
        /// </summary>
        /// <param name="thisShiftEmpID"></param>
        /// <returns></returns>
        public static bool CanChangeShift(DataRow rc_info, string thisShiftEmpID, out DateTime InProcessTime)
        {
            string lastShiftEmpID = rc_info["UPDATE_USERID"].ToString();

            if (DateTime.TryParse(rc_info["IN_PROCESS_TIME"].ToString(), out DateTime DtTemp) ||
                DateTime.TryParse(rc_info["WIP_IN_TIME"].ToString(), out DtTemp) ||
                DateTime.TryParse(rc_info["UPDATE_TIME"].ToString(), out DtTemp) ||
                DateTime.TryParse(rc_info["CREATE_TIME"].ToString(), out DtTemp)
                )
            {
                InProcessTime = DtTemp;
            }
            else
            {
                InProcessTime = DtTemp;

                throw new ArgumentException("找不到流程卡投入製程開始加工的合理可能時間。");
            }

            if (!string.IsNullOrWhiteSpace(lastShiftEmpID) &&
                lastShiftEmpID != thisShiftEmpID)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 檢查加工結束時間
        /// </summary>
        /// <param name="rc_info">流程卡資訊</param>
        /// <param name="machine">機台</param>
        /// <param name="end_time">結束加工時間</param>
        /// <param name="message">檢查結果</param>
        /// <returns></returns>
        public static bool Check_End_time(DataRow rc_info, MachineDownModel machine, DateTime end_time, out string message)
        {
            DateTime now = OtSrv.GetDBDateTimeNow();

            message = string.Empty;

            if (!DateTime.TryParse(rc_info["WIP_IN_TIME"].ToString(), out DateTime WipInTime))
            {
                WipInTime = DateTime.Parse(rc_info["UPDATE_TIME"].ToString());
            }

            string s = @"
SELECT
    START_TIME
FROM
    SAJET.G_RC_TRAVEL_MACHINE_DOWN
WHERE
    RC_NO = :RC_NO
AND TRAVEL_ID = :TRAVEL_ID
AND NODE_ID = :NODE_ID
AND MACHINE_ID = :MACHINE_ID
AND END_TIME IS NULL
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", rc_info["RC_NO"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.Number, "TRAVEL_ID", rc_info["TRAVEL_ID"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.Number, "NODE_ID", rc_info["NODE_ID"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.Number, "MACHINE_ID", machine.MACHINE_ID },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                WipInTime = DateTime.Parse(d.Tables[0].Rows[0]["START_TIME"].ToString());
            }

            if (end_time <= WipInTime)
            {
                message
                    = SajetCommon.SetLanguage("The end time of the machine must be later than the start time")
                    + ": "
                    + WipInTime.ToString("yyyy/ MM/ dd HH: mm: ss");

                return false;
            }

            if (end_time > now)
            {
                message
                    = SajetCommon.SetLanguage("The end time of the machine must be earlier than now");

                return false;
            }

            machine.START_TIME = WipInTime;

            machine.END_TIME = end_time;

            return true;
        }

        /// <summary>
        /// 取得流程卡在現在這個製程，以前的班次已經生產的加工數量的總和。
        /// </summary>
        /// <param name="rc_info">當前流程卡的資訊</param>
        /// <returns></returns>
        public static int GetMachineTotalLoadBefore(DataRow rc_info)
        {
            string rc_no = rc_info["RC_NO"].ToString();

            string node_id = rc_info["NODE_ID"].ToString();

            string s = @"
SELECT
    SUM(LOAD_QTY) TOTAL_LOAD
FROM
    SAJET.G_RC_TRAVEL_MACHINE_DOWN
WHERE
    RC_NO = :RC_NO 
    AND NODE_ID = :NODE_ID
    AND MACHINE_ID <> 0
    AND REASON_ID = 0
    AND COUNT_WORKTIME = 0
    AND END_TIME IS NOT NULL
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", rc_info["RC_NO"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.Number, "NODE_ID", rc_info["NODE_ID"].ToString() },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                int.TryParse(d.Tables[0].Rows[0]["TOTAL_LOAD"].ToString(), out int total_load);

                return total_load;
            }

            return 0;
        }

        /// <summary>
        /// 取得換班的總作業人數
        /// </summary>
        /// <param name="rc_info"></param>
        /// <param name="emp_id"></param>
        /// <returns></returns>
        public static int GetOperatorCount(DataRow rc_info, string emp_id)
        {
            string s = @"
SELECT DISTINCT
    UPDATE_USERID
FROM
    SAJET.G_RC_TRAVEL_MACHINE_DOWN
WHERE
    RC_NO = :RC_NO
    AND NODE_ID = :NODE_ID
    AND REASON_ID = 0
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", rc_info["RC_NO"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.Number, "NODE_ID", rc_info["NODE_ID"].ToString() },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                var emp_id_list = new List<string>();

                foreach (DataRow row in d.Tables[0].Rows)
                {
                    emp_id_list.Add(row["UPDATE_USERID"].ToString());
                }

                int emp_count = emp_id_list.Count;

                if (!emp_id_list.Contains(emp_id))
                {
                    emp_count++;
                }

                return emp_count;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// 取得換班歷程
        /// </summary>
        /// <param name="rc_info">流程卡資訊</param>
        /// <returns></returns>
        public static DataSet GetShiftHistory(DataRow rc_info)
        {
            string s = @"
SELECT
    '['
    || TRIM(C.MACHINE_CODE)
    || '] '
    || TRIM(C.MACHINE_DESC) ""Machine"",
    TO_CHAR(A.START_TIME, 'YYYY/ MM/ DD HH24: MI: SS') ""Start time"",
    TO_CHAR(A.END_TIME, 'YYYY/ MM/ DD HH24: MI: SS') ""End time"",
    A.LOAD_QTY           ""Load"",
    '['
    || B.EMP_NO
    || '] '
    || B.EMP_NAME ""EMP"",
    A.REASON_ID, REASON_DESC
FROM
    SAJET.G_RC_TRAVEL_MACHINE_DOWN   A,
    SAJET.SYS_EMP                    B,
    SAJET.SYS_MACHINE                C,
    SAJET.SYS_MACHINE_DOWN_DETAIL    D
WHERE
    A.RC_NO = :RC_NO
    AND A.NODE_ID = :NODE_ID
    AND C.MACHINE_ID <> 0
   --AND A.REASON_ID = 0
    AND A.COUNT_WORKTIME = 0
    AND A.UPDATE_USERID = B.EMP_ID
    AND A.MACHINE_ID = C.MACHINE_ID
    AND A.REASON_ID = D.REASON_ID
    --AND A.END_TIME IS NOT NULL
ORDER BY
    END_TIME
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", rc_info["RC_NO"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.Number, "NODE_ID", rc_info["NODE_ID"].ToString() },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            return d;
        }

        /// <summary>
        /// 扣除前面班次已經加工的數量，將剩餘數量填入這個班次使用的機台的加工數量，當作預設值
        /// </summary>
        /// <param name="rc_info"></param>
        /// <param name="machine"></param>
        public static void UpdateLoadOfThisShift(DataRow rc_info, MachineDownModel machine)
        {
            string s = @"
UPDATE SAJET.G_RC_TRAVEL_MACHINE_DOWN
SET
    LOAD_QTY = :LOAD_QTY
WHERE
    RC_NO = :RC_NO
    AND NODE_ID = :NODE_ID
    AND MACHINE_ID = :MACHINE_ID
    AND END_TIME IS NULL
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", rc_info["RC_NO"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.Number, "NODE_ID", rc_info["NODE_ID"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.Number, "MACHINE_ID", machine.MACHINE_ID },
                new object[] { ParameterDirection.Input, OracleType.Number, "LOAD_QTY", machine.LOAD_QTY },
            };

            ClientUtils.ExecuteSQL(s, p.ToArray());
        }
    }
}
