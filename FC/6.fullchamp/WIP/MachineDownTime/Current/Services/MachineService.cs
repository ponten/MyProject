using MachineDownTime.Enums;
using MachineDownTime.Models;
using SajetClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using RcSrv = MachineDownTime.Services.RuncardService;

namespace MachineDownTime.Services
{
    /// <summary>
    /// 機台的業務邏輯
    /// </summary>
    internal static class MachineService
    {
        /// <summary>
        /// 載入停機代碼清單。
        /// </summary>
        internal static List<DownTypeModel> GetDownCodes()
        {
            var MachineDownTypes = new List<DownTypeModel>();

            string s = @"
SELECT
    TYPE_ID
   ,TYPE_CODE
   ,TYPE_DESC
FROM
    SAJET.SYS_MACHINE_DOWN_TYPE
WHERE
    ENABLED = 'Y'
ORDER BY
    TYPE_CODE
";
            var downTypes = ClientUtils.ExecuteSQL(s);

            MachineDownTypes.Add(new DownTypeModel
            {
                ID = "0",
                CODE = "",
                NAME = SajetCommon.SetLanguage("None"),
                DownCodes = new List<DownCodeModel>
                {
                    new DownCodeModel
                    {
                        ID = "0",
                        CODE = "",
                        NAME = SajetCommon.SetLanguage("None"),
                    }
                }
            });

            if (downTypes != null)
            {
                foreach (DataRow row in downTypes.Tables[0].Rows)
                {
                    MachineDownTypes.Add(new DownTypeModel
                    {
                        ID = row["TYPE_ID"].ToString(),
                        CODE = row["TYPE_CODE"].ToString(),
                        NAME = $"[{row["TYPE_CODE"]}] {row["TYPE_DESC"]}",
                        DownCodes = new List<DownCodeModel>(),
                    });
                }
            }

            s = @"
SELECT
    TYPE_ID
   ,REASON_ID
   ,REASON_CODE
   ,REASON_DESC
   ,COUNT_WORKTIME
FROM
    SAJET.SYS_MACHINE_DOWN_DETAIL
WHERE
    ENABLED = 'Y'
ORDER BY
    TYPE_ID
   ,REASON_CODE
";
            var downDetails = ClientUtils.ExecuteSQL(s);

            if (downDetails != null)
            {
                foreach (DataRow row in downDetails.Tables[0].Rows)
                {
                    MachineDownTypes
                        .FirstOrDefault(e => e.ID == row["TYPE_ID"].ToString())
                        .DownCodes.Add(new DownCodeModel
                        {
                            ID = row["REASON_ID"].ToString(),
                            CODE = row["REASON_CODE"].ToString(),
                            NAME = $"[{row["REASON_CODE"]}] {row["REASON_DESC"]}",
                        });
                }
            }

            return MachineDownTypes;
        }

        /// <summary>
        /// 取得使用中的機台
        /// </summary>
        /// <param name="rc_no"></param>
        /// <param name="machine_in_use"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        internal static bool GetMachineInUse(
            string rc_no,
            string current_status,
            out DataSet machine_in_use,
            out string message)
        {
            string s = default;
            if (current_status== "1")
             s = @"
SELECT
    A.RC_NO,
    B.MACHINE_ID,
    C.MACHINE_CODE,
    '['
    || TRIM(C.MACHINE_CODE)
    || ']'
    || TRIM(C.MACHINE_DESC) MACHINE_NAME,
    TO_CHAR(B.START_TIME, 'YYYY/  MM/DD HH24:MI:  SS') START_TIME
FROM
    SAJET.G_RC_STATUS                A,
    SAJET.G_RC_TRAVEL_MACHINE_DOWN   B,
    SAJET.SYS_MACHINE                C
WHERE
    A.RC_NO = :RC_NO
    AND A.RC_NO = B.RC_NO
    AND A.NODE_ID = B.NODE_ID
    AND B.END_TIME IS NULL
    AND B.MACHINE_ID = C.MACHINE_ID
";
            else
                s = @"
SELECT
    A.RC_NO,
    B.MACHINE_ID,
    C.MACHINE_CODE,
    '['
    || TRIM(C.MACHINE_CODE)
    || ']'
    || TRIM(C.MACHINE_DESC) MACHINE_NAME,
    '' START_TIME
FROM
    SAJET.G_RC_STATUS A,
    SAJET.sys_rc_process_machine B,
    SAJET.SYS_MACHINE c,

    sajet.Sys_Rc_Route_Detail r ,
    sajet.sys_process p
WHERE
    A.RC_NO = :RC_NO
    AND A.Process_Id = b.process_id
    AND B.MACHINE_ID = C.MACHINE_ID

    AND a.route_id = r.route_id
    AND r.node_type =1 
    AND r.node_content = p.process_id

order by c.machine_code";

            object[][] p = new object[1][];
            p[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", rc_no };
            machine_in_use = ClientUtils.ExecuteSQL(s, p);

            if (machine_in_use != null &&
                machine_in_use.Tables[0].Rows.Count > 0)
            {
                message = string.Empty;

                return true;
            }
            else
            {
                message = SajetCommon.SetLanguage(MessageEnum.MachineNotFoundOrNoMachineInUse.ToString());

                return false;
            }
        }

        /// <summary>
        /// 取得製程的可用機台
        /// </summary>
        /// <param name="process_id"></param>
        /// <param name="machine_in_use"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        internal static bool GetProcessMachine(
            string RC_NO,
            string process_id,
            out DataSet machine_in_use,
            out string message)
        {

            string sSQL = $"select r.route_id from sajet.g_rc_status r where r.rc_no ='{RC_NO}'";
            DataTable rc_no_info = ClientUtils.ExecuteSQL(sSQL).Tables[0];
              string s = $@"
SELECT
    '{RC_NO}' RC_NO, 
    B.MACHINE_ID,
    C.MACHINE_CODE,
    '['
    || TRIM(C.MACHINE_CODE)
    || ']'
    || TRIM(C.MACHINE_DESC) MACHINE_NAME,
    '' START_TIME
FROM
    SAJET.sys_rc_process_machine B,
    SAJET.SYS_MACHINE c
WHERE   b.Process_Id = :Process_Id
    AND B.MACHINE_ID = C.MACHINE_ID
order by c.machine_code";

            object[][] p = new object[1][];
            p[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "Process_Id", process_id };
            machine_in_use = ClientUtils.ExecuteSQL(s, p);

            if (machine_in_use != null &&
                machine_in_use.Tables[0].Rows.Count > 0)
            {
                message = string.Empty;

                return true;
            }
            else
            {
                message = SajetCommon.SetLanguage(MessageEnum.MachineNotFoundOrNoMachineInUse.ToString());

                return false;
            }
        }

        /// <summary>
        /// 取得機台異常工時紀錄
        /// </summary>
        /// <param name="rc_no"></param>
        /// <param name="node_id"></param>
        /// <param name="machine_id"></param>
        /// <param name="machine_down_log"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        internal static bool GetDownLog(
            string rc_no,
            string node_id,
            string machine_id,
            out DataSet machine_down_log,
            out string message)
        {
            string s = @"
SELECT
    '['
    || TRIM(B.MACHINE_CODE)
    || ']'
    || TRIM(B.MACHINE_DESC) MACHINE,
    TO_CHAR(A.START_TIME, 'YYYY/  MM/DD HH24:MI:  SS') START_TIME,
    TO_CHAR(A.END_TIME, 'YYYY/  MM/DD HH24:MI:  SS') END_TIME,
    A.WORK_TIME_MINUTE DOWNTIME,
    '['
    || D.REASON_CODE
    || ']'
    || D.REASON_DESC
    || ' '
    || D.DESC2 DOWN_CODE,
    A.REMARK,
    C.EMP_NAME
FROM
    SAJET.G_MACHINE_DOWN_EXCLUDE    A,
    SAJET.SYS_MACHINE               B,
    SAJET.SYS_EMP                   C,
    SAJET.SYS_MACHINE_DOWN_DETAIL   D
WHERE
    A.RC_NO = :RC_NO
    AND A.NODE_ID = :NODE_ID
    AND A.MACHINE_ID = :MACHINE_ID
    AND A.MACHINE_ID = B.MACHINE_ID
    AND A.UPDATE_USERID = C.EMP_ID
    AND A.REASON_ID = D.REASON_ID
ORDER BY
    A.START_TIME ASC
";
            object[][] p = new object[3][];
            p[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", rc_no };
            p[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NODE_ID", node_id };
            p[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MACHINE_ID", machine_id };
            machine_down_log = ClientUtils.ExecuteSQL(s, p);

            if (machine_down_log != null &&
                machine_down_log.Tables[0].Rows.Count > 0)
            {
                message = string.Empty;

                return true;
            }
            else
            {
                message = string.Empty;

                return false;
            }
        }

        /// <summary>
        /// 檢查停機時間的區段
        /// </summary>
        /// <param name="down_log"></param>
        /// <param name="start_time"></param>
        /// <param name="end_time"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        internal static bool CheckDownTimeSpan(
            DataSet down_log,
            DateTime start_time,
            DateTime end_time,
            out string message)
        {
            int record_count = down_log.Tables[0].Rows.Count;

            bool pass_examine = false;

            message = string.Empty;

            DateTime start_time_i;

            DateTime end_time_i;

            // 從最後一筆停機時間段開始檢查，往前檢查
            for (int i = record_count - 1; i >= 0; i--)
            {
                // 取得時間段的結束時間 i，比較新紀錄的開始時間
                if (!DateTime.TryParse(down_log.Tables[0].Rows[i]["END_TIME"].ToString(), out end_time_i))
                {
                    message = SajetCommon.SetLanguage(MessageEnum.TimeSpanCheckError.ToString());

                    return false;
                }

                // 新紀錄的開始時間在結束時間 i 之後，第一關合格
                if (start_time > end_time_i)
                {
                    // 取得後一筆時間段的開始時間，取不到就直接通過
                    if (i + 1 == record_count)
                    {
                        return true;
                    }

                    // 取得後一筆時間段的開始時間，比較新紀錄的結束時間
                    if (!DateTime.TryParse(down_log.Tables[0].Rows[i + 1]["START_TIME"].ToString(), out start_time_i))
                    {
                        message = SajetCommon.SetLanguage(MessageEnum.TimeSpanCheckError.ToString());

                        return false;
                    }

                    // 時間點重疊代表時間段也會重疊，反之通過
                    if (start_time_i > end_time)
                    {
                        return true;
                    }
                }
            }

            // 最後一個檢查。比較新紀錄的結束時間與最早一筆紀錄的開始時間
            {
                if (!DateTime.TryParse(down_log.Tables[0].Rows[0]["START_TIME"].ToString(), out start_time_i))
                {
                    message = SajetCommon.SetLanguage(MessageEnum.TimeSpanCheckError.ToString());

                    return false;
                }

                // 新紀錄的時間段與最早的一筆紀錄也沒有重疊
                if (start_time_i > end_time)
                {
                    return true;
                }
            }

            // 檢查從尾到頭，都沒有找到不重疊的時間段，則新增失敗
            message = SajetCommon.SetLanguage(MessageEnum.MachineDownTimeSpanOverlapping.ToString());

            return pass_examine;
        }

        /// <summary>
        /// 新增停機記錄
        /// </summary>
        /// <param name="rc_no_info"></param>
        /// <param name="machine_id"></param>
        /// <param name="start_time"></param>
        /// <param name="end_time"></param>
        /// <param name="reason_id"></param>
        /// <param name="remark"></param>
        /// <param name="update_userid"></param>
        internal static void InsertNewRecord(
            DataRow rc_no_info,
            string machine_id,
            DateTime start_time,
            DateTime end_time,
            int reason_id,
            string remark,
            int update_userid)
        {
            DateTime now = OtherService.GetDBDateTimeNow();

            string s = @"
            SELECT RC_NO, CURRENT_STATUS
            FROM SAJET.G_RC_STATUS
            WHERE    RC_NO = :RC_NO";
            object[][] p = new object[][] { new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", rc_no_info["RC_NO"].ToString() } };
            DataTable dt = ClientUtils.ExecuteSQL(s, p).Tables[0];
            if (dt.Rows[0]["CURRENT_STATUS"].ToString() == "1")
            {
                s = @"
INSERT INTO SAJET.G_MACHINE_DOWN_EXCLUDE (
    RC_NO,
    NODE_ID,
    TRAVEL_ID,
    MACHINE_ID,
    START_TIME,
    END_TIME,
    REASON_ID,
    LOAD_QTY,
    DATE_CODE,
    STOVE_SEQ,
    WORK_TIME_MINUTE,
    WORK_TIME_SECOND,
    COUNT_WORKTIME,
    REMARK,
    REPORT_TYPE,
    DATA_STATUS,
    UPDATE_USERID,
    UPDATE_TIME
)
    SELECT
        RC_NO,
        NODE_ID,
        TRAVEL_ID,
        MACHINE_ID,
        :START_TIME,
        :END_TIME,
        :REASON_ID,
        LOAD_QTY,
        DATE_CODE,
        STOVE_SEQ,
        NVL(ROUND((TO_NUMBER(:END_TIME - :START_TIME) * 24 * 60 * 60 + 29) / 60), 0),
        NVL(ROUND(TO_NUMBER(:END_TIME - :START_TIME) * 24 * 60 * 60, 0), 0),
        :COUNT_WORKTIME,/*1*/
        :REMARK,
        :REPORT_TYPE,/*1*/
        :DATA_STATUS,/*0*/
        :UPDATE_USERID,
        :UPDATE_TIME
    FROM
        SAJET.G_RC_TRAVEL_MACHINE_DOWN
    WHERE
        RC_NO = :RC_NO
        AND NODE_ID = :NODE_ID
        AND MACHINE_ID = :MACHINE_ID
        AND END_TIME IS NULL
";
                p = new object[12][];
                p[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", rc_no_info["RC_NO"].ToString() };
                p[1] = new object[] { ParameterDirection.Input, OracleType.Number, "NODE_ID", rc_no_info["NODE_ID"].ToString() };
                p[2] = new object[] { ParameterDirection.Input, OracleType.Number, "MACHINE_ID", machine_id };
                p[3] = new object[] { ParameterDirection.Input, OracleType.DateTime, "START_TIME", start_time };
                p[4] = new object[] { ParameterDirection.Input, OracleType.DateTime, "END_TIME", end_time };
                p[5] = new object[] { ParameterDirection.Input, OracleType.Number, "REASON_ID", reason_id };
                p[6] = new object[] { ParameterDirection.Input, OracleType.Number, "COUNT_WORKTIME", 1 };
                p[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "REMARK", remark };
                p[8] = new object[] { ParameterDirection.Input, OracleType.Number, "REPORT_TYPE", 1 };
                p[9] = new object[] { ParameterDirection.Input, OracleType.Number, "DATA_STATUS", 0 };
                p[10] = new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", update_userid };
                p[11] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", now };
            }
            else
            {
                s = @"
INSERT INTO SAJET.G_MACHINE_DOWN_EXCLUDE (
    RC_NO,
    NODE_ID,
    TRAVEL_ID,
    MACHINE_ID,
    START_TIME,
    END_TIME,
    REASON_ID,
    --LOAD_QTY,
    --DATE_CODE,
    --STOVE_SEQ,
    WORK_TIME_MINUTE,
    WORK_TIME_SECOND,
    COUNT_WORKTIME,
    REMARK,
    REPORT_TYPE,
    DATA_STATUS,
    UPDATE_USERID,
    UPDATE_TIME
)
    SELECT
        RC_NO,
        NODE_ID,
        TRAVEL_ID,
        :MACHINE_ID,
        :START_TIME,
        :END_TIME,
        :REASON_ID,
       -- LOAD_QTY,
       -- DATE_CODE,
       -- STOVE_SEQ,
        NVL(ROUND((TO_NUMBER(:END_TIME - :START_TIME) * 24 * 60 * 60 + 29) / 60), 0),
        NVL(ROUND(TO_NUMBER(:END_TIME - :START_TIME) * 24 * 60 * 60, 0), 0),
        :COUNT_WORKTIME,/*1*/
        :REMARK,
        :REPORT_TYPE,/*1*/
        :DATA_STATUS,/*0*/
        :UPDATE_USERID,
        :UPDATE_TIME
    FROM
        SAJET.G_RC_STATUS
    WHERE
        RC_NO = :RC_NO
";
                p = new object[][] {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", rc_no_info["RC_NO"].ToString() }  ,
                    new object[] { ParameterDirection.Input, OracleType.Number, "MACHINE_ID", machine_id },
                    new object[] { ParameterDirection.Input, OracleType.DateTime, "START_TIME", start_time },
                    new object[] { ParameterDirection.Input, OracleType.DateTime, "END_TIME", end_time }  ,
                    new object[] { ParameterDirection.Input, OracleType.Number, "REASON_ID", reason_id }  ,
                    new object[] { ParameterDirection.Input, OracleType.Number, "COUNT_WORKTIME", 1 } ,
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "REMARK", remark }  ,
                    new object[] { ParameterDirection.Input, OracleType.Number, "REPORT_TYPE", 1 }  ,
                    new object[] { ParameterDirection.Input, OracleType.Number, "DATA_STATUS", 0 }    ,
                    new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", update_userid }  ,
                    new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", now }               
                };
            }
                ClientUtils.ExecuteSQL(s, p);
        }
    }
}
