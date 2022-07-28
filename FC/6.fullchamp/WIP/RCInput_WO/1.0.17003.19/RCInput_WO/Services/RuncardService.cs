using RCInput_WO.Enums;
using SajetClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using OtSrv = RCInput_WO.Services.OtherService;

namespace RCInput_WO.Services
{
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
        public static bool IsOutputTimeValid(DataRow runcard, DateTime InProcessTime, out string message)
        {
            message = string.Empty;

            DateTime now = OtSrv.GetDBDateTimeNow();

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
    }
}
