using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;

namespace CRole.Services
{
    /// <summary>
    /// 綁定使用者角色的商業邏輯
    /// </summary>
    static class RoleBindingService
    {
        /// <summary>
        /// 把角色與使用者關聯起來
        /// </summary>
        /// <param name="role_id">角色 ID</param>
        /// <param name="update_userid">更新資料的人員 ID</param>
        /// <param name="list">使用者 ID 的清單</param>
        internal static void DoBindings(string role_id, string update_userid, List<DataRow> list)
        {
            string s = @"
DECLARE
    PARAM_ROLE_ID         NUMBER;
    PARAM_UPDATE_USERID   NUMBER;
    PARAM_UPDATE_TIME     DATE;
BEGIN
    PARAM_ROLE_ID := :ROLE_ID;
    PARAM_UPDATE_USERID := :UPDATE_USERID;
    PARAM_UPDATE_TIME := SYSDATE;
    --
    DELETE FROM SAJET.SYS_ROLE_EMP
    WHERE
        ROLE_ID = PARAM_ROLE_ID;

    {0}

END;
";

            string sINSERT = string.Empty;

            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.Number, "ROLE_ID", role_id },
                new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", update_userid },
            };

            if (list.Count > 0)
            {
                int i = 1;

                foreach (var row in list)
                {
                    sINSERT += $@"
    INTO SAJET.SYS_ROLE_EMP (
        ROLE_ID,
        EMP_ID,
        UPDATE_USERID,
        UPDATE_TIME
    ) VALUES (
        PARAM_ROLE_ID,
        :EMP_ID_{i},
        PARAM_UPDATE_USERID,
        PARAM_UPDATE_TIME
    )
";
                    p.Add(new object[] { ParameterDirection.Input, OracleType.Number, $"EMP_ID_{i}", row["EMP_ID"].ToString() });

                    i++;
                }

                sINSERT = $@"
    INSERT ALL

{sINSERT}

    SELECT
        1
    FROM
        DUAL;
";
            }

            s = string.Format(s, sINSERT);

            ClientUtils.ExecuteSQL(s, p.ToArray());
        }
    }
}
