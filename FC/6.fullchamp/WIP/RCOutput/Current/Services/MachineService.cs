using RCOutput.Models;
using SajetClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;

namespace RCOutput.Services
{
    /// <summary>
    /// 機台的商業邏輯
    /// </summary>
    public static class MachineService
    {
        /// <summary>
        /// 流程卡所在製程是否有設定製程機台
        /// </summary>
        /// <param name="rc_no"></param>
        /// <returns></returns>
        public static bool HaveSetProcessMachine(string rc_no)
        {
            string s = @"
SELECT
    COUNT(B.MACHINE_ID) MACHINE_COUNT
FROM
    SAJET.G_RC_STATUS              A,
    SAJET.SYS_RC_PROCESS_MACHINE   B
WHERE
    A.RC_NO = :RC_NO
    AND A.PROCESS_ID = B.PROCESS_ID
    AND B.ENABLED = 'Y'
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", rc_no }
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            return d != null
                && d.Tables[0].Rows.Count > 0
                && int.TryParse(d.Tables[0].Rows[0]["MACHINE_COUNT"].ToString(), out int machine_count)
                && machine_count > 0;
        }

        /// <summary>
        /// 取得使用中的機台的清單。
        /// </summary>
        /// <param name="RC_NO">指定流程卡號</param>
        /// <returns></returns>
        public static DataSet GetWorkingMachineList(string RC_NO)
        {
            string s = @"
SELECT
    sm.machine_id,
    TRIM(sm.machine_code) machine_code,
    TRIM(sm.machine_desc) machine_desc,
    sms.status_name,
    coalesce(rtmd.start_time, grs.wip_in_time, grs.update_time) ""START_TIME"",
    sms.run_flag,
    sms.default_status,
    rtmd.load_qty,
    rtmd.date_code,
    rtmd.stove_seq
FROM
    sajet.g_rc_status                grs,
    sajet.g_rc_travel_machine        grtm,
    sajet.g_rc_travel_machine_down   rtmd,
    sajet.g_machine_status           gms,
    sajet.sys_machine                sm,
    sajet.sys_machine_status         sms
WHERE
    grtm.travel_id = grs.travel_id
    AND grtm.rc_no = grs.rc_no
    AND grs.rc_no = :rc_no
    AND grtm.machine_id = sm.machine_id
    AND sm.enabled = 'Y'
    AND grtm.machine_id = gms.machine_id
    AND gms.current_status_id = sms.status_id
    AND grtm.rc_no = rtmd.rc_no (+)
    AND grtm.travel_id = rtmd.travel_id (+)
    AND grtm.machine_id = rtmd.machine_id
    AND rtmd.end_time IS NULL
ORDER BY
    sm.machine_code
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
        /// 載入可用的製程機台清單。
        /// </summary>
        /// <param name="rc_info">流程卡資訊</param>
        /// <param name="machine_list">可用機台清單</param>
        /// <param name="message">訊息</param>
        /// <returns></returns>
        public static bool GetMachineList(DataRow rc_info, out List<MachineDownModel> machine_list, out string message)
        {
            message = string.Empty;

            machine_list = new List<MachineDownModel>();

            string s = @"
SELECT
    B.MACHINE_ID
   ,TRIM(B.MACHINE_CODE) MACHINE_CODE
   ,TRIM(B.MACHINE_DESC) MACHINE_DESC
   ,D.STATUS_NAME
   ,D.RUN_FLAG
   ,D.DEFAULT_STATUS
FROM
    SAJET.SYS_RC_PROCESS_MACHINE A
   ,SAJET.SYS_MACHINE B
   ,SAJET.G_MACHINE_STATUS C
   ,SAJET.SYS_MACHINE_STATUS D
WHERE
    A.PROCESS_ID = :PROCESS_ID
AND A.MACHINE_ID = B.MACHINE_ID
AND A.MACHINE_ID = C.MACHINE_ID
AND C.CURRENT_STATUS_ID = D.STATUS_ID
AND A.ENABLED = 'Y'
AND B.ENABLED = 'Y'
AND B.MACHINE_ID NOT IN
(
    SELECT
       GRTM.MACHINE_ID
    FROM
        SAJET.G_RC_STATUS GRS
       ,SAJET.G_RC_TRAVEL_MACHINE GRTM
       ,SAJET.SYS_MACHINE SM
    WHERE
        GRS.RC_NO = :RC_NO
    AND GRS.RC_NO = GRTM.RC_NO
    AND GRS.TRAVEL_ID = GRTM.TRAVEL_ID
    AND GRTM.MACHINE_ID = SM.MACHINE_ID
)
ORDER BY
    MACHINE_CODE
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", rc_info["RC_NO"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.Number, "PROCESS_ID", rc_info["PROCESS_ID"].ToString() },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in d.Tables[0].Rows)
                {
                    machine_list.Add(new MachineDownModel
                    {
                        MACHINE_ID = int.Parse(row["MACHINE_ID"].ToString()),
                        MACHINE_CODE = row["MACHINE_CODE"].ToString().Trim(),
                        MACHINE_DESC = row["MACHINE_DESC"].ToString().Trim(),
                        STATUS_NAME = row["STATUS_NAME"].ToString(),
                        LOAD_QTY = 0,
                        DATE_CODE = string.Empty
                    });
                }

                return true;
            }
            else
            {
                message = SajetCommon.SetLanguage("No option");

                return false;
            }
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
                    LOAD_QTY = int.TryParse(row["LOAD_QTY"].ToString(), out int loadQty) ? loadQty : 0,
                    DATE_CODE = row["DATE_CODE"].ToString(),
                    STOVE_SEQ = row["STOVE_SEQ"].ToString(),
                }).ToList();
        }

        /// <summary>
        /// 機台資訊另外更新到變更機台記錄表。
        /// </summary>
        /// <param name="rc_info">流程卡資訊</param>
        /// <param name="machines">使用機台的清單</param>
        /// <param name="end_time">結束加工時間</param>
        /// <param name="changed_shift">是否換班</param>
        /// <param name="shift_time">換班時間</param>
        /// <param name="workload">前一班的工作量</param>
        /// <param name="emp_id">員工 ID</param>
        /// <param name="is_one_sheet">製程是否配置為只有產出</param>
        public static void RecordMachine(
            DataRow rc_info,
            List<MachineDownModel> machines,
            DateTime end_time,
            bool changed_shift,
            DateTime shift_time,
            string workload,
            string emp_id,
            bool is_one_sheet)
        {
            string s = string.Empty;

            int i = 0;

            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", rc_info["RC_NO"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.Number, "TRAVEL_ID", rc_info["TRAVEL_ID"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.Number, "NODE_ID", rc_info["NODE_ID"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.DateTime, "NOW_TIME", end_time },
                // 現在在產出報工的人員
                new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", emp_id },
                // （換班前）執行投入報工的人員
                new object[] { ParameterDirection.Input, OracleType.Number, "BEFORE_SHIFT_ID", rc_info["UPDATE_USERID"].ToString() },
                // 換班時間
                new object[] { ParameterDirection.Input, OracleType.DateTime, "SHIFT_TIME", shift_time },
            };

            foreach (var machine in machines)
            {
                p.Add(new object[] { ParameterDirection.Input, OracleType.Number, $"MACHINE_ID{i}", machine.MACHINE_ID });
                p.Add(new object[] { ParameterDirection.Input, OracleType.Number, $"LOAD_QTY{i}", machine.LOAD_QTY });
                p.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, $"DATE_CODE{i}", machine.DATE_CODE });
                p.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, $"STOVE_SEQ{i}", machine.STOVE_SEQ });

                if (is_one_sheet)
                {
                    s += $@"
INSERT INTO
    SAJET.G_RC_TRAVEL_MACHINE_DOWN
(
    RC_NO
   ,NODE_ID
   ,TRAVEL_ID
   ,MACHINE_ID
   ,START_TIME
   ,END_TIME
   ,REASON_ID
   ,LOAD_QTY
   ,DATE_CODE
   ,STOVE_SEQ
   ,UPDATE_USERID
   ,UPDATE_TIME
   ,WORK_TIME_MINUTE
   ,WORK_TIME_SECOND
   ,DATA_STATUS
   ,REPORT_TYPE
)
VALUES
(
    :RC_NO
   ,:NODE_ID
   ,:TRAVEL_ID
   ,:MACHINE_ID{i}
   ,:NOW_TIME
   ,:NOW_TIME
   ,0
   ,:LOAD_QTY{i}
   ,:DATE_CODE{i}
   ,:STOVE_SEQ{i}
   ,:UPDATE_USERID
   ,:NOW_TIME
   ,0
   ,0
   ,1
);";
                }
                // 只有開始加工時間比換班時間早的那些機台，要把工時和加工數量分成兩段記錄
                else if (changed_shift && machine.START_TIME < shift_time)
                {
                    int.TryParse(workload, out int shift_workload);

                    p.Add(new object[] { ParameterDirection.Input, OracleType.Number, $"LOAD_QTY_A{i}", shift_workload });
                    p.Add(new object[] { ParameterDirection.Input, OracleType.Number, $"LOAD_QTY_B{i}", machine.LOAD_QTY - shift_workload });

                    s += $@"
/* 換班前的加工時間、加工數量 */
UPDATE
    SAJET.G_RC_TRAVEL_MACHINE_DOWN
SET
    END_TIME = :SHIFT_TIME
   ,WORK_TIME_MINUTE = NVL(ROUND((TO_NUMBER(:SHIFT_TIME - START_TIME) * 24 * 60 * 60 + 29) / 60), 0)
   ,WORK_TIME_SECOND = NVL(ROUND(TO_NUMBER(:SHIFT_TIME - START_TIME) * 24 * 60 * 60, 0), 0)
   ,UPDATE_USERID = :BEFORE_SHIFT_ID
   ,UPDATE_TIME = :NOW_TIME
   ,DATA_STATUS = 0
   ,LOAD_QTY = :LOAD_QTY_A{i}
   ,DATE_CODE = :DATE_CODE{i}
   ,STOVE_SEQ = :STOVE_SEQ{i}
   ,REPORT_TYPE = 1
WHERE
    RC_NO = :RC_NO
AND TRAVEL_ID = :TRAVEL_ID
AND NODE_ID = :NODE_ID
AND MACHINE_ID = :MACHINE_ID{i}
AND END_TIME IS NULL
;

/* 換班後的加工時間、加工數量 */
INSERT INTO SAJET.G_RC_TRAVEL_MACHINE_DOWN (
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
    REMARK,
    UPDATE_USERID,
    UPDATE_TIME,
    DATA_STATUS,
    REPORT_TYPE,
    COUNT_WORKTIME
)
    SELECT
        RC_NO,
        NODE_ID,
        TRAVEL_ID,
        MACHINE_ID,
        :SHIFT_TIME,
        :NOW_TIME,
        REASON_ID,
        :LOAD_QTY_B{i},
        DATE_CODE,
        STOVE_SEQ,
        NVL(ROUND((TO_NUMBER(:NOW_TIME - :SHIFT_TIME) * 24 * 60 * 60 + 29) / 60), 0),
        NVL(ROUND(TO_NUMBER(:NOW_TIME - :SHIFT_TIME) * 24 * 60 * 60, 0), 0),
        REMARK,
        :UPDATE_USERID,
        :NOW_TIME,
        0,
        1,
        COUNT_WORKTIME
    FROM
        SAJET.G_RC_TRAVEL_MACHINE_DOWN
    WHERE
        RC_NO = :RC_NO
        AND TRAVEL_ID = :TRAVEL_ID
        AND NODE_ID = :NODE_ID
        AND MACHINE_ID = :MACHINE_ID{i}
        AND END_TIME = :SHIFT_TIME
;";
                }
                else
                {
                    s += $@"
UPDATE
    SAJET.G_RC_TRAVEL_MACHINE_DOWN
SET
    END_TIME = :NOW_TIME
   ,WORK_TIME_MINUTE = NVL(ROUND((TO_NUMBER(:NOW_TIME - START_TIME) * 24 * 60 * 60 + 29) / 60), 0)
   ,WORK_TIME_SECOND = NVL(ROUND(TO_NUMBER(:NOW_TIME - START_TIME) * 24 * 60 * 60, 0), 0)
   ,UPDATE_USERID = :UPDATE_USERID
   ,UPDATE_TIME = :NOW_TIME
   ,DATA_STATUS = 0
   ,LOAD_QTY = :LOAD_QTY{i}
   ,DATE_CODE = :DATE_CODE{i}
   ,STOVE_SEQ = :STOVE_SEQ{i}
   ,REPORT_TYPE = 1
WHERE
    RC_NO = :RC_NO
AND TRAVEL_ID = :TRAVEL_ID
AND NODE_ID = :NODE_ID
AND MACHINE_ID = :MACHINE_ID{i}
AND END_TIME IS NULL
;";
                }

                i++;
            }

            s = $@"
BEGIN
{s}

-- 未選機台的記錄也更新結束時間
UPDATE
    SAJET.G_RC_TRAVEL_MACHINE_DOWN
SET
    END_TIME = :NOW_TIME
   ,WORK_TIME_MINUTE = NVL(ROUND((TO_NUMBER(:NOW_TIME - START_TIME) * 24 * 60 * 60 + 29) / 60), 0)
   ,WORK_TIME_SECOND = NVL(ROUND(TO_NUMBER(:NOW_TIME - START_TIME) * 24 * 60 * 60, 0), 3)
   ,DATA_STATUS = 0
   ,REPORT_TYPE = 1
WHERE
    RC_NO = :RC_NO
AND TRAVEL_ID = :TRAVEL_ID
AND NODE_ID = :NODE_ID
AND MACHINE_ID = 0
;
END;
";
            ClientUtils.ExecuteSQL(s, p.ToArray());
        }
    }
}
