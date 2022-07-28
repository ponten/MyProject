using QualityInspectionAAR.Models;
using SajetClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;

namespace QualityInspectionAAR.Services
{
    /// <summary>
    /// 品保資料收集的商業邏輯
    /// </summary>
    public static partial class QCService
    {
        /// <summary>
        /// 取得使用者有權限補品保資料的流程卡的資訊
        /// </summary>
        /// <param name="user_id">使用者的員工 ID</param>
        /// <param name="runcards_set">符合條件的流程卡資訊</param>
        /// <returns></returns>
        public static bool FindRuncardToInspect(string user_id, out DataSet runcards_set)
        {
            string s = @"
WITH
/* 使用者有權限操作的製程 */
 ACCESSIBLE_PROCESSES AS (
    SELECT
        A.PROCESS_ID
    FROM
        SAJET.SYS_ROLE_OP_PRIVILEGE   A,
        SAJET.SYS_ROLE_EMP            B
    WHERE
        A.ROLE_ID = B.ROLE_ID
        AND B.EMP_ID = :EMP_ID
    UNION
    SELECT
        A.PROCESS_ID
    FROM
        SAJET.SYS_EMP_PROCESS_PRIVILEGE   A,
        SAJET.SYS_EMP                     B
    WHERE
        A.EMP_ID = B.EMP_ID
        AND B.EMP_ID = :EMP_ID
),
/* 所有區段、製程資料 */
 STAGE_PROCESS_INFO AS (
    SELECT
        B.STAGE_ID,
        TRIM(B.STAGE_CODE) STAGE_CODE,
        TRIM(B.STAGE_NAME) STAGE_NAME,
        A.PROCESS_ID,
        TRIM(A.PROCESS_CODE) PROCESS_CODE,
        TRIM(A.PROCESS_NAME) PROCESS_NAME
    FROM
        SAJET.SYS_PROCESS   A,
        SAJET.SYS_STAGE     B
    WHERE
        A.STAGE_ID = B.STAGE_ID
        AND A.ENABLED = 'Y'
        AND B.ENABLED = 'Y'
),
/* 有收集品保資料的流程卡、製程，以及已收集資料筆數 */
 SN_ID_GROUP AS (
    SELECT DISTINCT
        RC_NO,
        PROCESS_ID,
        NODE_ID,
        SN_ID
    FROM
        SAJET.G_QC_VALUE
), COLLECTED_QTY AS (
    SELECT
        RC_NO,
        PROCESS_ID,
        NODE_ID,
        COUNT(SN_ID) QTY
    FROM
        SN_ID_GROUP
    GROUP BY
        RC_NO,
        PROCESS_ID,
        NODE_ID
),
/* 已綁定抽驗計畫的料號 - 製程 */
 BINDED_INFO AS (
    SELECT
        PART_ID,
        PROCESS_ID
    FROM
        SAJET.SYS_PART_QC_PLAN
    WHERE
        ENABLED = 'Y'
), BINDED_PROCESS_INFO AS (
    SELECT
        A.PART_ID,
        A.PROCESS_ID,
        C.NODE_ID
    FROM
        BINDED_INFO                 A,
        SAJET.SYS_PART              B,
        SAJET.SYS_RC_ROUTE_DETAIL   C
    WHERE
        A.PART_ID = B.PART_ID
        AND B.ROUTE_ID = C.ROUTE_ID
        AND TO_CHAR(A.PROCESS_ID) = C.NODE_CONTENT
),
/* 所有流程卡，以及可以收集資料的製程 */
 RC_LIST AS (
    SELECT
        C.WORK_ORDER,
        B.RC_NO,
        A.PART_ID,
        A.PROCESS_ID,
        A.NODE_ID,
        B.INITIAL_QTY CURRENT_QTY
    FROM
        BINDED_PROCESS_INFO   A,
        SAJET.G_RC_STATUS     B,
        SAJET.G_WO_BASE       C
    WHERE
        A.PART_ID = B.PART_ID
        AND A.PART_ID = C.PART_ID
        AND B.WORK_ORDER = C.WORK_ORDER
        AND C.WO_STATUS = 3
    ORDER BY
        A.PROCESS_ID
)
SELECT
    B.STAGE_CODE,
    B.STAGE_NAME,
    B.STAGE_ID,
    B.PROCESS_CODE,
    B.PROCESS_NAME,
    A.PROCESS_ID,
    A.NODE_ID,
    A.WORK_ORDER,
    A.RC_NO,
    E.PART_ID,
    E.PART_NO,
    E.OPTION2 FORMER_PART_NO,
    TRIM(E.SPEC1)
    || ' '
    || TRIM(E.SPEC2) SPEC,
    A.CURRENT_QTY,
    NVL(C.QTY, 0) COLLECTED_QTY
FROM
    RC_LIST                A
    LEFT JOIN STAGE_PROCESS_INFO     B ON A.PROCESS_ID = B.PROCESS_ID
    LEFT JOIN COLLECTED_QTY          C ON A.RC_NO = C.RC_NO
                                 AND A.NODE_ID = C.NODE_ID
                                 AND A.PROCESS_ID = C.PROCESS_ID,
    ACCESSIBLE_PROCESSES   D,
    SAJET.SYS_PART         E
WHERE
    A.PROCESS_ID = D.PROCESS_ID
    AND A.PART_ID = E.PART_ID
ORDER BY
    B.STAGE_ID,
    A.PROCESS_ID,
    A.RC_NO
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.Number, "EMP_ID", user_id },
            };

            runcards_set = ClientUtils.ExecuteSQL(s, p.ToArray());

            return runcards_set != null
                && runcards_set.Tables[0].Rows.Count > 0;
        }

        public static List<RuncardModel> GetModel(DataSet runcard_set)
        {
            var runcard_list = new List<RuncardModel>();

            foreach (DataRow row in runcard_set.Tables[0].Rows)
            {
                var model = new RuncardModel
                {
                    WORK_ORDER = row["WORK_ORDER"].ToString(),
                    RC_NO = row["RC_NO"].ToString(),
                    PART_ID = row["PART_ID"].ToString(),
                    PART_NO = row["PART_NO"].ToString(),
                    FORMER_PART_NO = row["FORMER_PART_NO"].ToString(),
                    SPEC = row["SPEC"].ToString(),
                    CURRENT_QTY = int.Parse(row["CURRENT_QTY"].ToString()),
                    COLLECTED_QTY = int.Parse(row["COLLECTED_QTY"].ToString()),
                    STAGE_ID = row["STAGE_ID"].ToString(),
                    PROCESS_ID = row["PROCESS_ID"].ToString(),
                    NODE_ID = row["NODE_ID"].ToString(),
                    STAGE_CODE = row["STAGE_CODE"].ToString(),
                    STAGE_NAME = row["STAGE_NAME"].ToString(),
                    PROCESS_CODE = row["PROCESS_CODE"].ToString(),
                    PROCESS_NAME = row["PROCESS_NAME"].ToString(),
                };

                runcard_list.Add(model);
            }

            return runcard_list;
        }
    }
}
