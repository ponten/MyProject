using RCProcessParam.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;

namespace RCProcessParam.Services
{
    /// <summary>
    /// 物料製程設定的業務邏輯
    /// </summary>
    internal static class PartOptionService
    {
        /// <summary>
        /// 取得物料製程設定
        /// </summary>
        /// <param name="part_id"></param>
        /// <param name="route_id"></param>
        /// <param name="node_id"></param>
        /// <returns></returns>
        internal static DataSet GetData(string part_id, string route_id, string node_id)
        {
            string s = @"
SELECT
    OPTION1 STD_WORKTIME
FROM
    SAJET.SYS_PROCESS_OPTION_PART
WHERE
    PART_ID = :PART_ID
    AND ROUTE_ID = :ROUTE_ID
    AND NODE_ID = :NODE_ID
";
            object[][] p = new object[3][];
            p[0] = new object[] { ParameterDirection.Input, OracleType.Number, "PART_ID", part_id };
            p[1] = new object[] { ParameterDirection.Input, OracleType.Number, "ROUTE_ID", route_id };
            p[2] = new object[] { ParameterDirection.Input, OracleType.Number, "NODE_ID", node_id };

            DataSet d = ClientUtils.ExecuteSQL(s, p);

            return d;
        }

        internal static DataSet GetData(string part_id,string route_id)
        {
            string s = @"
WITH ROUTE_DETAIL AS (
    SELECT
        ROWNUM IDX,
        NODE_CONTENT,
        NODE_ID
    FROM
        SAJET.SYS_RC_ROUTE_DETAIL
    START WITH ROUTE_ID = :ROUTE_ID
               AND NODE_CONTENT = 'START' CONNECT BY PRIOR NEXT_NODE_ID = NODE_ID
                                                     OR PRIOR NEXT_NODE_ID = GROUP_ID
), PROCESSES AS (
    SELECT
        B.IDX,
        C.PROCESS_NAME,
        B.NODE_ID
    FROM
        ROUTE_DETAIL        B,
        SAJET.SYS_PROCESS   C
    WHERE
        B.NODE_CONTENT = TO_CHAR(C.PROCESS_ID)
), PROCESS_OPTIONS AS (
    SELECT
        PART_ID,
        ROUTE_ID,
        NODE_ID,
        PROCESS_ID,
        OPTION1
    FROM
        SAJET.SYS_PROCESS_OPTION_PART
    WHERE
        PART_ID = :PART_ID
        AND ROUTE_ID = :ROUTE_ID
)
SELECT
    A.PROCESS_NAME,
    A.NODE_ID,
    NVL(B.OPTION1, 0) STD_WORKTIME
FROM
    PROCESSES         A,
    PROCESS_OPTIONS   B
WHERE
    A.NODE_ID = B.NODE_ID (+)
ORDER BY
    IDX
";
            object[][] p = new object[2][];
            p[0] = new object[] { ParameterDirection.Input, OracleType.Number, "PART_ID", part_id };
            p[1] = new object[] { ParameterDirection.Input, OracleType.Number, "ROUTE_ID", route_id };

            DataSet d = ClientUtils.ExecuteSQL(s, p);

            return d;
        }

        /// <summary>
        /// 取得隱藏欄位名稱
        /// </summary>
        /// <returns></returns>
        internal static List<string> GetHiddenColumnNames()
        {
            return new List<string>
            {
                "NODE_ID"
            };
        }

        /// <summary>
        /// 儲存物料製程設定
        /// </summary>
        /// <param name="optionModel"></param>
        internal static void PutData(PartOptionModel optionModel)
        {
            string s = @"
DECLARE
    FOUND_ROW         NUMBER;
    P_PART_ID         SAJET.SYS_PART.PART_ID%TYPE;
    P_ROUTE_ID        SAJET.SYS_RC_ROUTE.ROUTE_ID%TYPE;
    P_NODE_ID         SAJET.SYS_RC_ROUTE_DETAIL.NODE_ID%TYPE;
    P_PROCESS_ID      SAJET.SYS_PROCESS.PROCESS_ID%TYPE;
    P_OPTION1         SAJET.SYS_PROCESS_OPTION_PART.OPTION1%TYPE;
    P_UPDATE_USERID   SAJET.SYS_EMP.EMP_ID%TYPE;
    P_UPDATE_TIME     DATE;
BEGIN
    P_PART_ID := :PART_ID;
    P_ROUTE_ID := :ROUTE_ID;
    P_NODE_ID := :NODE_ID;
    P_PROCESS_ID := :PROCESS_ID;
    P_OPTION1 := :OPTION1;
    P_UPDATE_USERID := :UPDATE_USERID;
    P_UPDATE_TIME := :UPDATE_TIME;
    SELECT
        COUNT(*)
    INTO FOUND_ROW
    FROM
        SAJET.SYS_PROCESS_OPTION_PART
    WHERE
        PART_ID = P_PART_ID
        AND NODE_ID = P_NODE_ID;

    IF FOUND_ROW = 0 THEN
        INSERT INTO SAJET.SYS_PROCESS_OPTION_PART (
            PART_ID,
            ROUTE_ID,
            NODE_ID,
            PROCESS_ID,
            OPTION1,
            UPDATE_USERID,
            UPDATE_TIME
        ) VALUES (
            P_PART_ID,
            P_ROUTE_ID,
            P_NODE_ID,
            P_PROCESS_ID,
            P_OPTION1,
            P_UPDATE_USERID,
            P_UPDATE_TIME
        );

    ELSIF FOUND_ROW = 1 THEN
        UPDATE SAJET.SYS_PROCESS_OPTION_PART
        SET
            OPTION1 = P_OPTION1,
            UPDATE_USERID = P_UPDATE_USERID,
            UPDATE_TIME = P_UPDATE_TIME
        WHERE
            PART_ID = P_PART_ID
            AND ROUTE_ID = P_ROUTE_ID
            AND NODE_ID = P_NODE_ID
            AND PROCESS_ID = P_PROCESS_ID;

    END IF;

END;
";
            object[][] p = new object[7][];
            p[0] = new object[] { ParameterDirection.Input, OracleType.Number, "PART_ID", optionModel.part_id };
            p[1] = new object[] { ParameterDirection.Input, OracleType.Number, "ROUTE_ID", optionModel.route_id };
            p[2] = new object[] { ParameterDirection.Input, OracleType.Number, "NODE_ID", optionModel.node_id };
            p[3] = new object[] { ParameterDirection.Input, OracleType.Number, "PROCESS_ID", optionModel.process_id };
            p[4] = new object[] { ParameterDirection.Input, OracleType.Number, "OPTION1", optionModel.option1 };
            p[5] = new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", optionModel.update_userid };
            p[6] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", optionModel.update_time };

            ClientUtils.ExecuteSQL(s, p);
        }
    }
}
