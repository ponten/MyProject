using RCOutput.Enums;
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
    /// 流程卡的商業邏輯
    /// </summary>
    public static class RuncardService
    {
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

        /// <summary>
        /// 取得流程卡在指定製程時的詳細資訊。
        /// </summary>
        /// <param name="Runcard"></param>
        /// <param name="processID"></param>
        /// <returns></returns>
        public static DataRow GetRcNoInfo(string Runcard, string processID)
        {
            string s = @"
SELECT
    *
FROM
    SAJET.G_RC_STATUS
WHERE
    RC_NO = :RC_NO
    AND PROCESS_ID = :PROCESS_ID
UNION
SELECT
    *
FROM
    SAJET.G_RC_TRAVEL
WHERE
    RC_NO = :RC_NO
    AND PROCESS_ID = :PROCESS_ID
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", Runcard },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", processID },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            return d.Tables[0].Rows[0];
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
        /// 檢查投入時間設定正不正常
        /// </summary>
        /// <param name="runcard">流程卡的現在狀態的資訊</param>
        /// <param name="InProcessTime">投入時間</param>
        /// <param name="message">檢查結果</param>
        /// <returns></returns>
        public static bool IsOutputTimeValid(DataRow runcard, DateTime OutProcessTime, out string message)
        {
            message = string.Empty;

            DateTime now = OtSrv.GetDBDateTimeNow();

            if (DateTime.TryParse(runcard["WIP_IN_TIME"].ToString(), out DateTime DtTemp))
            {
                if (DtTemp > OutProcessTime)
                {
                    message = SajetCommon.SetLanguage(MessageEnum.TheOutProcessTimeIsSetEarlierThanTheInProcessTime.ToString());

                    return false;
                }
            }
            else
            if (DateTime.TryParse(runcard["OUT_PROCESS_TIME"].ToString(), out DtTemp))
            {
                if (DtTemp > OutProcessTime)
                {
                    message = SajetCommon.SetLanguage(MessageEnum.TheOutProcessTimeIsSetEarlierThanTheOutPutTimeOfPreviousProcess.ToString());

                    return false;
                }
            }
            else
            if (DateTime.TryParse(runcard["UPDATE_TIME"].ToString(), out DtTemp))
            {
                if (DtTemp > OutProcessTime)
                {
                    message = SajetCommon.SetLanguage(MessageEnum.TheOutProcessTimeIsSetEarlierThanTheLastUpdateTime.ToString());

                    return false;
                }
            }

            if (OutProcessTime > now)
            {
                message = SajetCommon.SetLanguage(MessageEnum.TheOutProcessTimeMustBeEarlierThanNow.ToString());

                return false;
            }

            return true;
        }

        /// <summary>
        /// 把重工數量記錄在另外一張資料表。
        /// </summary>
        /// <param name="rc_no">流程卡號</param>
        /// <param name="emp_id">員工 ID</param>
        /// <param name="node_id">製程的生產途程節點 ID</param>
        /// <param name="travel_id">流程卡歷程流水號</param>
        /// <param name="rework_qty">重工數量</param>
        public static void SaveReworkQty(
            string rc_no,
            string emp_id,
            string node_id,
            string travel_id,
            int rework_qty = 0)
        {
            string S = @"
INSERT INTO
    SAJET.G_RC_TRAVEL_REWORK
(
    RC_NO
   ,NODE_ID
   ,REWORK_QTY
   ,UPDATE_TIME
   ,UPDATE_USERID
   ,TRAVEL_ID
)
VALUES
(
    :RC_NO
   ,:NODE_ID
   ,:REWORK_QTY
   ,SYSDATE
   ,:UPDATE_USERID
   ,:TRAVEL_ID
)";
            var P = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", rc_no },
                new object[] { ParameterDirection.Input, OracleType.Number, "NODE_ID", node_id },
                new object[] { ParameterDirection.Input, OracleType.Number, "REWORK_QTY", rework_qty },
                new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", emp_id },
                new object[] { ParameterDirection.Input, OracleType.Number, "TRAVEL_ID", travel_id }
            };

            ClientUtils.ExecuteSQL(S, P.ToArray());
        }

        /// <summary>
        /// 儲存作業人數資訊
        /// </summary>
        /// <param name="rc_no_info"></param>
        /// <param name="emp_id"></param>
        /// <param name="op_count"></param>
        /// <param name="update_time"></param>
        public static void SaveOPCount(
            DataRow rc_no_info,
            string emp_id,
            int op_count,
            DateTime update_time)
        {
            string s = @"
INSERT INTO SAJET.G_RC_OP_LOG (
    WORK_ORDER,
    RC_NO,
    PROCESS_ID,
    NODE_ID,
    OP_EMP_ID,
    OP_COUNT,
    UPDATE_USERID,
    UPDATE_TIME
) VALUES (
    :WORK_ORDER,
    :RC_NO,
    :PROCESS_ID,
    :NODE_ID,
    :OP_EMP_ID,
    :OP_COUNT,
    :UPDATE_USERID,
    :UPDATE_TIME
)
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", rc_no_info["WORK_ORDER"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", rc_no_info["RC_NO"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.Number, "PROCESS_ID", rc_no_info["PROCESS_ID"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.Number, "NODE_ID", rc_no_info["NODE_ID"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.Number, "OP_EMP_ID", emp_id },
                new object[] { ParameterDirection.Input, OracleType.Number, "OP_COUNT", op_count },
                new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", emp_id },
                new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", update_time },
            };

            ClientUtils.ExecuteSQL(s, p.ToArray());
        }
    
    }
}
