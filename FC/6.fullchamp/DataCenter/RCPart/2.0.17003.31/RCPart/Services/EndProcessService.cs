using RCPart.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;

namespace RCPart.Services
{
    /// <summary>
    /// 生產途程設定報工結束製程的商業邏輯
    /// </summary>
    public static class EndProcessService
    {
        /// <summary>
        /// 取得料號預設生產途程的報工結束製程
        /// </summary>
        /// <param name="PART_ID">料號 ID</param>
        /// <returns></returns>
        public static string GetCurrentEndProcess(string PART_ID)
        {
            string s = @"
SELECT
    C.PROCESS_NAME
FROM
    SAJET.SYS_PART          A,
    SAJET.SYS_END_PROCESS   B,
    SAJET.SYS_PROCESS       C
WHERE
    A.PART_ID = :PART_ID
    AND B.PART_ID = :PART_ID
    AND A.ROUTE_ID = B.ROUTE_ID
    AND B.PROCESS_ID = C.PROCESS_ID
    AND B.ENABLED = 'Y'
    AND C.ENABLED = 'Y'
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.Number, "PART_ID", PART_ID ?? "" },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                return d.Tables[0].Rows[0]["PROCESS_NAME"].ToString();
            }
            else
            {
                return " -- ";
            }
        }

        /// <summary>
        /// 取得料號可用生產途程的報工結束製程
        /// </summary>
        /// <param name="PART_ID">料號 ID</param>
        /// <param name="ROUTE_NAME">可用途程名稱</param>
        /// <returns></returns>
        public static string GetCurrentEndProcess(string PART_ID, string ROUTE_NAME)
        {
            string s = @"
SELECT
    C.PROCESS_NAME
FROM
    SAJET.SYS_END_PROCESS   A
    LEFT JOIN SAJET.SYS_RC_ROUTE      B ON A.ROUTE_ID = B.ROUTE_ID
    LEFT JOIN SAJET.SYS_PROCESS       C ON A.PROCESS_ID = C.PROCESS_ID
WHERE
    A.PART_ID = :PART_ID
    AND B.ROUTE_NAME = :ROUTE_NAME
    AND A.ENABLED = 'Y'
    AND B.ENABLED = 'Y'
    AND C.ENABLED = 'Y'
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.Number, "PART_ID", PART_ID },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "ROUTE_NAME", ROUTE_NAME },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                return d.Tables[0].Rows[0]["PROCESS_NAME"].ToString();
            }
            else
            {
                return " -- ";
            }
        }

        /// <summary>
        /// 取得所有料號可用生產途程的報工結束製程的設定
        /// </summary>
        /// <param name="PART_ID">料號 ID</param>
        /// <returns></returns>
        public static List<SysEndProcessModel> GetEndProcessSettings(string PART_ID)
        {
            string s = @"
SELECT
    A.PART_ID,
    A.ROUTE_ID,
    A.NODE_ID,
    A.PROCESS_ID,
    B.PROCESS_NAME,
    B.PROCESS_DESC,
    A.UPDATE_USERID,
    A.UPDATE_TIME,
    A.ENABLED
FROM
    SAJET.SYS_END_PROCESS   A,
    SAJET.SYS_PROCESS       B
WHERE
    A.PART_ID = :PART_ID
    AND A.PROCESS_ID = B.PROCESS_ID
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", PART_ID },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                var result = new List<SysEndProcessModel>();

                foreach (DataRow row in d.Tables[0].Rows)
                {
                    result.Add(new SysEndProcessModel
                    {
                        ROUTE_ID = row["ROUTE_ID"].ToString(),
                        ROUTE_NAME = row["ROUTE_NAME"].ToString(),
                        ROUTE_DESC = row["ROUTE_DESC"].ToString(),
                        NODE_ID = row["NODE_ID"].ToString(),
                        PROCESS_ID = row["PROCESS_ID"].ToString(),
                        PROCESS_CODE = row["PROCESS_CODE"].ToString(),
                        PROCESS_NAME = row["PROCESS_NAME"].ToString(),
                    });
                }

                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 轉換成報工結束製程的資料模型
        /// </summary>
        /// <param name="row">資料列</param>
        /// <returns></returns>
        public static SysEndProcessModel GetModel(DataRow row)
        {
            return new SysEndProcessModel
            {
                ROUTE_ID = row["ROUTE_ID"].ToString(),
                ROUTE_NAME = row["ROUTE_NAME"].ToString(),
                ROUTE_DESC = row["ROUTE_DESC"].ToString(),
                NODE_ID = row["NODE_ID"].ToString(),
                PROCESS_ID = row["PROCESS_ID"].ToString(),
                PROCESS_CODE = row["PROCESS_CODE"].ToString(),
                PROCESS_NAME = row["PROCESS_NAME"].ToString(),
            };
        }

        /// <summary>
        /// 儲存生產途程的報工結束製程的設定值
        /// </summary>
        /// <param name="PART_ID">料號 ID</param>
        /// <param name="ROUTE_ID">可用生產途程 ID</param>
        /// <param name="node">（結束製程）節點資訊</param>
        public static void SaveEndProcessSetting(string PART_ID, string ROUTE_ID, SysEndProcessModel node = null)
        {
            DateTime now = OtherService.GetDBDateTimeNow();

            string s = $@"
DECLARE
    FOUND_ROW             NUMBER;
    FOUND_ROW_STATUS      VARCHAR(5);
    PARAM_PART_ID         NUMBER;
    PARAM_ROUTE_ID        NUMBER;
    PARAM_NODE_ID         NUMBER;
    PARAM_PROCESS_ID      NUMBER;
    PARAM_ENABLED         VARCHAR(5);
    PARAM_UPDATE_USERID   NUMBER;
    PARAM_UPDATE_TIME     DATE;
BEGIN
    --
    FOUND_ROW := 0;
    PARAM_PART_ID := :PART_ID;
    PARAM_ROUTE_ID := :ROUTE_ID;
    PARAM_UPDATE_USERID := :UPDATE_USERID;
    PARAM_UPDATE_TIME := :UPDATE_TIME;
    PARAM_NODE_ID := :NODE_ID;
    PARAM_PROCESS_ID := :PROCESS_ID;
    PARAM_ENABLED := :ENABLED;
    --
    SELECT
        COUNT(NODE_ID)
    INTO FOUND_ROW
    FROM
        SAJET.SYS_END_PROCESS
    WHERE
        PART_ID = PARAM_PART_ID
        AND ROUTE_ID = PARAM_ROUTE_ID
        AND NODE_ID = PARAM_NODE_ID
        AND PROCESS_ID = PARAM_PROCESS_ID;

    IF FOUND_ROW > 0 THEN
        SELECT
            UPPER(TRIM(ENABLED))
        INTO FOUND_ROW_STATUS
        FROM
            SAJET.SYS_END_PROCESS
        WHERE
            PART_ID = PARAM_PART_ID
            AND ROUTE_ID = PARAM_ROUTE_ID
            AND NODE_ID = PARAM_NODE_ID
            AND PROCESS_ID = PARAM_PROCESS_ID;

        UPDATE SAJET.SYS_END_PROCESS
        SET
            ENABLED = 'N',
            UPDATE_USERID = PARAM_UPDATE_USERID,
            UPDATE_TIME = PARAM_UPDATE_TIME
        WHERE
            PART_ID = PARAM_PART_ID
            AND ROUTE_ID = PARAM_ROUTE_ID
            AND ENABLED = 'Y'
            AND ( NODE_ID <> PARAM_NODE_ID
                  OR PROCESS_ID <> PARAM_PROCESS_ID );

        IF FOUND_ROW_STATUS = 'N' THEN

            UPDATE SAJET.SYS_END_PROCESS
            SET
                ENABLED = 'Y',
                UPDATE_USERID = PARAM_UPDATE_USERID,
                UPDATE_TIME = PARAM_UPDATE_TIME
            WHERE
                PART_ID = PARAM_PART_ID
                AND ROUTE_ID = PARAM_ROUTE_ID
                AND NODE_ID = PARAM_NODE_ID
                AND PROCESS_ID = PARAM_PROCESS_ID;

        END IF;

    ELSE
        UPDATE SAJET.SYS_END_PROCESS
        SET
            ENABLED = 'N',
            UPDATE_USERID = PARAM_UPDATE_USERID,
            UPDATE_TIME = PARAM_UPDATE_TIME
        WHERE
            PART_ID = PARAM_PART_ID
            AND ROUTE_ID = PARAM_ROUTE_ID
            AND ENABLED = 'Y'
            AND NODE_ID <> PARAM_NODE_ID
            AND PROCESS_ID <> PARAM_PROCESS_ID;

        IF PARAM_PROCESS_ID <> 0 AND PARAM_NODE_ID <> 0 THEN
            INSERT INTO SAJET.SYS_END_PROCESS (
                PART_ID,
                ROUTE_ID,
                NODE_ID,
                PROCESS_ID,
                UPDATE_USERID,
                UPDATE_TIME,
                ENABLED
            ) VALUES (
                PARAM_PART_ID,
                PARAM_ROUTE_ID,
                PARAM_NODE_ID,
                PARAM_PROCESS_ID,
                PARAM_UPDATE_USERID,
                PARAM_UPDATE_TIME,
                PARAM_ENABLED
            );

        END IF;

    END IF;

END;
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.Number, "PART_ID", PART_ID },
                new object[] { ParameterDirection.Input, OracleType.Number, "ROUTE_ID", ROUTE_ID },
                new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", ClientUtils.UserPara1 },
                new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", now },
            };

            if (node != null)
            {
                p.Add(new object[] { ParameterDirection.Input, OracleType.Number, "NODE_ID", node.NODE_ID });
                p.Add(new object[] { ParameterDirection.Input, OracleType.Number, "PROCESS_ID", node.PROCESS_ID });
                p.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "ENABLED", node.ENABLED });
            }

            ClientUtils.ExecuteSQL(s, p.ToArray());
        }
    }
}
