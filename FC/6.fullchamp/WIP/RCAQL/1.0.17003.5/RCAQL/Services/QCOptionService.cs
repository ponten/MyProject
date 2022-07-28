using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;

namespace RCAQL.Services
{
    /// <summary>
    /// 品保設定的商業邏輯
    /// </summary>
    internal static class QCOptionService
    {
        /// <summary>
        /// 製程有首件檢、末件檢的品保設定，必須抽驗至少一個
        /// </summary>
        /// <param name="rc_no"></param>
        /// <returns></returns>
        internal static bool MustInspect(string rc_no)
        {
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", rc_no },
            };

            string s = @"
SELECT
    B.OPTION1,
    B.OPTION2,
    A.INITIAL_QTY CURRENT_QTY
FROM
    SAJET.G_RC_STATUS          A,
    SAJET.SYS_PART_QC_OPTION   B
WHERE
    A.RC_NO = :RC_NO
    AND A.PART_ID = B.PART_ID
    AND A.PROCESS_ID = B.PROCESS_ID
    AND ( B.OPTION1 = 'Y'
          OR B.OPTION2 = 'Y' )
";
            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                // 首件檢
                bool FPI = d.Tables[0].Rows[0]["OPTION1"].ToString() == "Y";
                // 末件檢
                bool LPI = d.Tables[0].Rows[0]["OPTION2"].ToString() == "Y";

                int.TryParse(d.Tables[0].Rows[0]["CURRENT_QTY"].ToString(), out int current_qty);

                s = @"
WITH RC_INFO AS (
    SELECT
        WORK_ORDER,
        PROCESS_ID
    FROM
        SAJET.G_RC_STATUS
    WHERE
        RC_NO = :RC_NO
)
SELECT
    NVL(SUM(INITIAL_QTY), 0) TOTAL_OUTPUT_QTY
FROM
    SAJET.G_RC_TRAVEL   A,
    RC_INFO             B
WHERE
    A.WORK_ORDER = B.WORK_ORDER
    AND A.PROCESS_ID = B.PROCESS_ID
";
                d = ClientUtils.ExecuteSQL(s, p.ToArray());

                int.TryParse(d.Tables[0].Rows[0]["TOTAL_OUTPUT_QTY"].ToString(), out int total_output_qty);

                if (FPI)
                {
                    return total_output_qty == 0;
                }

                if (LPI)
                {
                    s = @"
SELECT
    B.TARGET_QTY
FROM
    SAJET.G_RC_STATUS   A,
    SAJET.G_WO_BASE     B
WHERE
    A.RC_NO = :RC_NO
    AND A.WORK_ORDER = B.WORK_ORDER
";
                    d = ClientUtils.ExecuteSQL(s, p.ToArray());

                    int.TryParse(d.Tables[0].Rows[0]["TARGET_QTY"].ToString(), out int target_qty);

                    return current_qty + total_output_qty == target_qty;
                }
            }

            return false;
        }
    }
}
