using RCProcessParam.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;

namespace RCProcessParam.Services
{
    internal static class QCOptionService
    {
        /// <summary>
        /// 取得品保設定資訊
        /// </summary>
        /// <param name="process_info">指定製程節點</param>
        /// <returns></returns>
        internal static DataSet GetData(ProcessViewModel process_info)
        {
            string s = @"
SELECT
    OPTION1 FPI,
    OPTION2 LPI
FROM
    SAJET.SYS_PART_QC_OPTION
WHERE
    PART_ID = :PART_ID
    AND PROCESS_ID = :PROCESS_ID
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", process_info.PART_ID },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", process_info.PROCESS_ID },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            return d;
        }

        /// <summary>
        /// 取得品保設定資訊
        /// </summary>
        /// <param name="part_id">指定料號 ID</param>
        /// <returns></returns>
        internal static DataSet GetData(string part_id)
        {
            string s = @"
WITH
/* 指定料號的基本資訊 */
 PART_INFO AS (
    SELECT
        PART_ID,
        ROUTE_ID
    FROM
        SAJET.SYS_PART
    WHERE
        PART_ID = :PART_ID
),
/* 預設生產途程的製程按照順序排列 */
 ROUTE_NODES AS (
    SELECT
        ROWNUM IDX,
        A.ROUTE_ID,
        NODE_CONTENT,
        NODE_ID,
        B.PART_ID
    FROM
        SAJET.SYS_RC_ROUTE_DETAIL   A,
        PART_INFO                   B,
        SAJET.SYS_RC_ROUTE          C
    WHERE
        B.ROUTE_ID = A.ROUTE_ID
        AND B.ROUTE_ID = C.ROUTE_ID
        AND C.ENABLED = 'Y'
    START WITH A.ROUTE_ID = B.ROUTE_ID
               AND NODE_CONTENT = 'START' CONNECT BY PRIOR NEXT_NODE_ID = NODE_ID
                                                     OR PRIOR NEXT_NODE_ID = GROUP_ID
)
/* 排列好的生產途程與分好組的製程參數關聯起來 */
SELECT
    --NVL(TRIM(B.ROUTE_NAME), '0') ROUTE_NAME,
    B.ROUTE_ID,
    NVL(TRIM(A.PROCESS_CODE), '0') PROCESS_CODE,
    TRIM(A.PROCESS_NAME) PROCESS_NAME,
    A.PROCESS_ID,
    C.NODE_ID,
    C.PART_ID,
    D.OPTION1   FPI,
    D.OPTION2   LPI
FROM
    SAJET.SYS_PROCESS          A,
    SAJET.SYS_RC_ROUTE         B,
    ROUTE_NODES                C,
    SAJET.SYS_PART_QC_OPTION   D
WHERE
    B.ROUTE_ID = C.ROUTE_ID
    AND C.PART_ID = D.PART_ID
    AND TO_CHAR(A.PROCESS_ID) = C.NODE_CONTENT
    AND A.PROCESS_ID = D.PROCESS_ID
    AND A.ENABLED = 'Y'
    AND B.ENABLED = 'Y'
ORDER BY
    C.IDX
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", part_id },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            return d;
        }

        /// <summary>
        /// 預覽區塊不需要顯示的欄位名稱
        /// </summary>
        /// <returns></returns>
        internal static List<string> PreviewHiddenColumnNames()
        {
            return new List<string>
            {
                nameof(ProcessViewModel.NODE_ID),
                nameof(ProcessViewModel.PROCESS_ID),
                nameof(ProcessViewModel.PROCESS_CODE),
                nameof(ProcessViewModel.ROUTE_ID),
                nameof(ProcessViewModel.PART_ID),
            };
        }

        /// <summary>
        /// 儲存品保設定
        /// </summary>
        /// <param name="process_info">製程節點資訊</param>
        /// <param name="qc_option">品保設定</param>
        internal static void Save(ProcessViewModel process_info, QCOptionModel qc_option)
        {
            string s = @"
DECLARE
    FOUND                 NUMBER;
    PARAM_PART_ID         SAJET.SYS_PART.PART_ID%TYPE;
    PARAM_PROCESS_ID      SAJET.SYS_PROCESS.PROCESS_ID%TYPE;
    PARAM_NODE_ID         SAJET.SYS_RC_ROUTE_DETAIL.NODE_ID%TYPE;
    PARAM_FPI             SAJET.SYS_PART_QC_OPTION.OPTION1%TYPE;
    PARAM_LPI             SAJET.SYS_PART_QC_OPTION.OPTION2%TYPE;
    PARAM_UPDATE_USERID   SAJET.SYS_EMP.EMP_ID%TYPE;
    PARAM_UPDATE_TIME     DATE;
BEGIN
    FOUND := 0;

    PARAM_PART_ID := :PART_ID;
    PARAM_PROCESS_ID := :PROCESS_ID;
    PARAM_NODE_ID := :NODE_ID;
    PARAM_FPI := :OPTION1;
    PARAM_LPI := :OPTION2;
    PARAM_UPDATE_USERID := :UPDATE_USERID;
    PARAM_UPDATE_TIME := SYSDATE;

    SELECT
        COUNT(1)
    INTO FOUND
    FROM
        SAJET.SYS_PART_QC_OPTION
    WHERE
        PART_ID = PARAM_PART_ID
        AND PROCESS_ID = PARAM_PROCESS_ID;

    IF FOUND > 0 THEN
        UPDATE SAJET.SYS_PART_QC_OPTION
        SET
            OPTION1 = PARAM_FPI,
            OPTION2 = PARAM_LPI,
            UPDATE_USERID = PARAM_UPDATE_USERID,
            UPDATE_TIME = PARAM_UPDATE_TIME
        WHERE
            PART_ID = PARAM_PART_ID
            AND PROCESS_ID = PARAM_PROCESS_ID;

    ELSE
        INSERT INTO SAJET.SYS_PART_QC_OPTION (
            PART_ID,
            PROCESS_ID,
            NODE_ID,
            OPTION1,
            OPTION2,
            UPDATE_USERID,
            UPDATE_TIME
        ) VALUES (
            PARAM_PART_ID,
            PARAM_PROCESS_ID,
            PARAM_NODE_ID,
            PARAM_FPI,
            PARAM_LPI,
            PARAM_UPDATE_USERID,
            PARAM_UPDATE_TIME
        );

    END IF;

END;
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.Number, "PART_ID", process_info.PART_ID },
                new object[] { ParameterDirection.Input, OracleType.Number, "PROCESS_ID", process_info.PROCESS_ID },
                new object[] { ParameterDirection.Input, OracleType.Number, "NODE_ID", process_info.NODE_ID },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION1", qc_option.OPTION1 },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION2", qc_option.OPTION2 },
                new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", ClientUtils.UserPara1 },
            };

            ClientUtils.ExecuteSQL(s, p.ToArray());
        }
    }
}
