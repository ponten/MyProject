using RCIPQC.References;
using SajetClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;

namespace RCIPQC.Services
{
    public static class OtherService
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

        /// <summary>
        /// 取得使用中的機台的清單。
        /// </summary>
        /// <param name="RC_NO">指定流程卡號</param>
        /// <returns></returns>
        public static DataSet GetMachineList(string RC_NO)
        {
            string s = @"
SELECT
    sm.machine_id,
    sm.machine_code,
    sm.machine_desc,
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
        /// 檢查報工時間設定正不正常
        /// </summary>
        /// <param name="runcard">流程卡的現在狀態的資訊</param>
        /// <param name="InProcessTime">投入時間</param>
        /// <param name="message">檢查結果</param>
        /// <returns></returns>
        public static bool IsOutputTimeValid(DataRow runcard, DateTime OutProcessTime, out string message)
        {
            message = string.Empty;

            DateTime now = GetDBDateTimeNow();

            if (DateTime.TryParse(runcard["WIP_IN_TIME"].ToString(), out DateTime DtTemp))
            {
                if (DtTemp > OutProcessTime)
                {
                    message = SajetCommon.SetLanguage("TheOutProcessTimeIsSetEarlierThanTheInProcessTime");

                    return false;
                }
            }
            else
            if (DateTime.TryParse(runcard["OUT_PROCESS_TIME"].ToString(), out DtTemp))
            {
                if (DtTemp > OutProcessTime)
                {
                    message = SajetCommon.SetLanguage("TheOutProcessTimeIsSetEarlierThanTheOutPutTimeOfPreviousProcess");

                    return false;
                }
            }
            else
            if (DateTime.TryParse(runcard["UPDATE_TIME"].ToString(), out DtTemp))
            {
                if (DtTemp > OutProcessTime)
                {
                    message = SajetCommon.SetLanguage("TheOutProcessTimeIsSetEarlierThanTheLastUpdateTime");

                    return false;
                }
            }

            if (OutProcessTime > now)
            {
                message = SajetCommon.SetLanguage("TheOutProcessTimeMustBeEarlierThanNow");

                return false;
            }

            return true;
        }
    }
}
