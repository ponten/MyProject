using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;

namespace BCReprintDll.Services
{
    /// <summary>
    /// 沒分類的商業邏輯
    /// </summary>
    internal static class OtherServices
    {
        /// <summary>
        /// 檢查工單號碼
        /// </summary>
        /// <param name="work_order"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        internal static bool CheckForWorkOrder(string work_order, out string message)
        {
            string s = @"
SELECT
    *
FROM
    SAJET.G_WO_BASE
WHERE
    WORK_ORDER = :WORK_ORDER
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", work_order },
            };

            DataSet d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                message = "OK";

                return true;
            }
            else
            {
                message = "WorkOrder not found";

                return false;
            }
        }
    }
}
