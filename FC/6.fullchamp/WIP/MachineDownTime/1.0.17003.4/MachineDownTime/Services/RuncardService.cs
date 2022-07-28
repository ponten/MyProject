using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using MachineDownTime.Enums;
using SajetClass;

namespace MachineDownTime.Services
{
    /// <summary>
    /// 流程卡相關的業務邏輯
    /// </summary>
    internal static class RuncardService
    {
        /// <summary>
        /// 取得流程卡資訊
        /// </summary>
        /// <param name="rc_no">流程卡號</param>
        /// <param name="rc_no_info">流程卡資訊</param>
        /// <param name="message">訊息</param>
        /// <returns></returns>
        internal static bool GetInformation(
            string rc_no,
            out DataSet rc_no_info,
            out string message)
        {
            string s = @"
SELECT
    A.RC_NO,
    A.WORK_ORDER,
    B.PART_NO,
    A.NODE_ID,
    C.ROUTE_NAME,
    D.PROCESS_NAME,
    E.FACTORY_CODE,
    A.VERSION,
    A.PROCESS_ID,
    A.PART_ID,
    A.CURRENT_QTY   QTY,
    A.CURRENT_QTY   GOOD_QTY,
    0 SCRAP_QTY,
    A.CURRENT_STATUS,
    F.WO_OPTION2,
    B.SPEC1,
    B.SPEC2,
    B.OPTION2     FORMER_NO,
    B.OPTION4     BLUEPRINT,
    TO_CHAR(COALESCE(A.IN_PROCESS_TIME, A.WIP_IN_TIME, A.UPDATE_TIME), 'YYYY/ MM/DD HH24:MI: SS') IN_PROCESS_TIME
FROM
    SAJET.G_RC_STATUS    A,
    SAJET.SYS_PART       B,
    SAJET.SYS_RC_ROUTE   C,
    SAJET.SYS_PROCESS    D,
    SAJET.SYS_FACTORY    E,
    SAJET.G_WO_BASE      F
WHERE
    A.RC_NO = :RC_NO
    AND A.PART_ID = B.PART_ID
    AND A.ROUTE_ID = C.ROUTE_ID
    AND A.PROCESS_ID = D.PROCESS_ID
    AND A.FACTORY_ID = E.FACTORY_ID
    AND A.WORK_ORDER = F.WORK_ORDER
";
            object[][] p = new object[1][];
            p[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", rc_no };
            rc_no_info = ClientUtils.ExecuteSQL(s, p);

            if (rc_no_info != null &&
                rc_no_info.Tables[0].Rows.Count > 0)
            {
                message = string.Empty;

                return true;
            }
            else
            {
                message = SajetCommon.SetLanguage(MessageEnum.RuncardNotFound.ToString());

                return false;
            }
        }

        /// <summary>
        /// 檢查使用者的製程報工權限
        /// </summary>
        /// <param name="emp_id"></param>
        /// <param name="rc_no"></param>
        /// <returns></returns>
        internal static bool CheckAccessToProcess(
            string emp_id,
            string rc_no,
            out string message)
        {
            string s = @"
SELECT
    RC_NO
FROM
    SAJET.G_RC_STATUS
WHERE
    RC_NO = :RC_NO
    AND PROCESS_ID IN (
        SELECT
            A.PROCESS_ID
        FROM
            SAJET.SYS_ROLE_OP_PRIVILEGE   A,
            SAJET.SYS_EMP                 B,
            SAJET.SYS_ROLE_EMP            C
        WHERE
            B.EMP_ID = :EMP_ID
            AND B.EMP_ID = C.EMP_ID
            AND A.ROLE_ID = C.ROLE_ID
        UNION
        SELECT
            A.PROCESS_ID
        FROM
            SAJET.SYS_EMP_PROCESS_PRIVILEGE   A,
            SAJET.SYS_EMP                     B
        WHERE
            B.EMP_ID = :EMP_ID
            AND A.EMP_ID = B.EMP_ID
    )
";
            object[][] p = new object[2][];
            p[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "EMP_ID", emp_id };
            p[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", rc_no };
            var d = ClientUtils.ExecuteSQL(s, p);


            if (d != null
                && d.Tables[0].Rows.Count > 0)
            {
                message = string.Empty;

                return true;
            }
            else
            {
                message = SajetCommon.SetLanguage(MessageEnum.NoAccessToProcess.ToString());

                return false;
            }
        }

        /// <summary>
        /// 檢查流程卡當前狀態
        /// </summary>
        /// <param name="rc_no_info"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        internal static bool CheckState(
            DataRow rc_no_info,
            out string message)
        {
            string s = @"
SELECT
    RC_NO
FROM
    SAJET.G_RC_STATUS
WHERE
    RC_NO = :RC_NO
    AND PROCESS_ID = :PROCESS_ID
    AND NODE_ID = :NODE_ID
";
            object[][] p = new object[3][];
            p[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", rc_no_info["RC_NO"].ToString() };
            p[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", rc_no_info["PROCESS_ID"].ToString() };
            p[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NODE_ID", rc_no_info["NODE_ID"].ToString() };
            var d = ClientUtils.ExecuteSQL(s, p);

            if (d != null &&
                d.Tables[0].Rows.Count > 0)
            {
                message = string.Empty;

                return true;
            }
            else
            {
                message = SajetCommon.SetLanguage(MessageEnum.RuncardHasLeftThisProcess.ToString());

                return false;
            }
        }
    }
}
