using RCInput_WO.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;

namespace RCInput_WO.Services
{
    public static class MachineService
    {
        /// <summary>
        /// 取得使用中的機台的清單。
        /// </summary>
        /// <param name="RC_NO">指定流程卡號</param>
        /// <returns></returns>
        public static DataSet GetMachineList(string PROCESS_ID)
        {
            string s = @"
SELECT
    B.MACHINE_CODE
   ,D.STATUS_NAME
   ,D.RUN_FLAG
   ,D.DEFAULT_STATUS
   ,B.MACHINE_ID
   ,B.MACHINE_DESC
   ,0 LOAD_QTY
   ,'' DATE_CODE
   ,'' STOVE_SEQ
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
ORDER BY
    B.MACHINE_CODE
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.Number, "PROCESS_ID", PROCESS_ID }
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
                    Select = false,
                    MACHINE_ID = int.Parse(row["MACHINE_ID"].ToString()),
                    MACHINE_CODE = row["MACHINE_CODE"].ToString(),
                    MACHINE_DESC = $"[{row["MACHINE_CODE"]}] {row["MACHINE_DESC"]}",
                    STATUS_NAME = row["STATUS_NAME"].ToString(),
                    LOAD_QTY = int.TryParse(row["LOAD_QTY"].ToString(), out int load) ? load : 0,
                    DATE_CODE = row["DATE_CODE"].ToString(),
                    STOVE_SEQ = row["STOVE_SEQ"].ToString(),
                }).ToList();
        }
    }
}
