using QualityInspectionAAR.Enums;
using QualityInspectionAAR.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;

namespace QualityInspectionAAR.Services
{
    /// <summary>
    /// 未歸類的商業邏輯
    /// </summary>
    static class OtherService
    {
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
        /// 取得報工人員 ID
        /// </summary>
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
                new object[] { ParameterDirection.Input, OracleType.VarChar, "EMP_NO", emp_no },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            string EmpID = d.Tables[0].Rows[0]["EMP_ID"].ToString();

            return EmpID;
        }

        /// <summary>
        /// 取得特殊功能的權限設定
        /// </summary>
        /// <param name="EmpID"></param>
        /// <param name="fun"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool Check_Privilege(string EmpID, string fun, OperationTypeEnum type)
        {
            string sPrivilege = ClientUtils.GetPrivilege(EmpID, fun, ClientUtils.fProgramName).ToString();

            return SajetClass.SajetCommon.CheckEnabled(type.ToString(), sPrivilege);
        }

        /// <summary>
        /// 彙整流程卡資料的區段與製程，用作搜尋下拉選單的資料來源。
        /// </summary>
        /// <param name="d"></param>
        /// <param name="stagesList"></param>
        /// <returns></returns>
        public static bool GetStageAndProcess(DataSet d, ref List<StageModel> stagesList)
        {
            // To avoid CS1628:
            // Cannot use in ref or out parameter 'parameter' inside an anonymous method, lambda expression, or query expression.
            var stagesListCopy = stagesList.Select(x => x).ToList();

            if (d != null &&
                d.Tables[0].Rows.Count > 0)
            {
                var stages = d.Tables[0].Rows
                    .Cast<DataRow>()
                    .ToList();

                stages.ForEach(x =>
                {
                    if (!stagesListCopy.Any(z => z?.ID == x["STAGE_ID"].ToString()))
                    {
                        stagesListCopy.Add(new StageModel
                        {
                            ID = x["STAGE_ID"].ToString(),
                            CODE = x["STAGE_CODE"].ToString(),
                            NAME = x["STAGE_NAME"].ToString(),
                            ProcessModel = new List<ProcessModel>(),
                        });
                    }

                    var processes = stagesListCopy
                    .First(z => z.ID == x["STAGE_ID"].ToString())
                    .ProcessModel;

                    if (!processes.Any(z => z?.ID == x["PROCESS_ID"].ToString()))
                    {
                        processes.Add(new ProcessModel
                        {
                            ID = x["PROCESS_ID"].ToString(),
                            CODE = x["PROCESS_CODE"].ToString(),
                            NAME = x["PROCESS_NAME"].ToString(),
                            NODE_ID = x["NODE_ID"].ToString(),
                        });
                    }
                });

                stagesList = stagesListCopy.Select(x => x).ToList();

                return true;
            }

            stagesList = null;

            return false;
        }

        /// <summary>
        /// 使用搜尋欄位值找符合條件的流程卡
        /// </summary>
        /// <returns></returns>
        public static List<RuncardModel> FindRuncardsWithConditions(FilterValueModel filter, List<RuncardModel> runcard_list)
        {
            if (runcard_list.Count > 0)
            {
                var result = runcard_list
                    .Where(x => filter.ProcessID == 0 || x.PROCESS_ID == filter.ProcessID.ToString())
                    .ToList();

                result = result
                    .Where(x => string.IsNullOrWhiteSpace(filter.WorkOrder) || x.WORK_ORDER.Contains(filter.WorkOrder))
                    .ToList();

                result = result
                    .Where(x => string.IsNullOrWhiteSpace(filter.Runcard) || x.RC_NO.Contains(filter.Runcard))
                    .ToList();

                result = result
                    .Where(x => x.CURRENT_QTY > x.COLLECTED_QTY)
                    .ToList();

                return result;
            }

            return null;
        }
    }
}
