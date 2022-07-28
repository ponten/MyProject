using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Data.OracleClient;

namespace RC_ChangeRoute
{
    public partial class fAlert : Form
    {
        public fAlert()
        {
            InitializeComponent();
        }

        public static String sUserID = ClientUtils.UserPara1;
        public string sRC, s_RC_End;

        string sSQL;
        DataSet dsTemp;

        private void fAlert_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if ((RBtn_Release.Checked == false) && (RBtn_ScrapChange.Checked == false) && (RBtn_Scrap.Checked == false))
            {
                return;
            }

            try
            {
                DateTime datExeTime = DateTime.Now;
                long lTravel_Id = Convert.ToInt64(datExeTime.ToString("yyyyMMddHHmmssf"));

                int iProcess_Id = 0;
                decimal iCurrentQty = 0;

                sSQL = " SELECT PROCESS_ID,CURRENT_QTY "
                     + "   FROM SAJET.G_RC_STATUS "
                     + "  WHERE RC_NO = '" + sRC + "'"
                     + "    AND RELEASE = 'N'";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);

                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    iProcess_Id = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["PROCESS_ID"].ToString());
                    iCurrentQty = Convert.ToDecimal(dsTemp.Tables[0].Rows[0]["CURRENT_QTY"].ToString());
                }

                // 回復
                if (RBtn_Release.Checked)
                {
                    // New RC No
                    s_RC_End = "Release";

                    // Update SAJET.G_RC_STATUS
                    sSQL = @"UPDATE SAJET.G_RC_STATUS
                                SET TRAVEL_ID = :TRAVEL_ID,
                                    CURRENT_STATUS = :CURRENT_STATUS,
                                    UPDATE_USERID = :UPDATE_USERID,
                                    UPDATE_TIME = :UPDATE_TIME
                              WHERE RC_NO = :RC_NO";

                    object[][] Params = new object[5][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.Number, "TRAVEL_ID", lTravel_Id };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.Number, "CURRENT_STATUS", 0 };
                    Params[2] = new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", sUserID };
                    Params[3] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime };
                    Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", sRC };
                    dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

                    DialogResult = DialogResult.OK;
                }

                // 報廢轉換
                if (RBtn_ScrapChange.Checked)
                {
                    // New RC No
                    s_RC_End = "";

                    sSQL = " SELECT MAX(SUBSTR(RC_NO,-1,1) + 1) AS RC_END "
                         + "   FROM SAJET.G_RC_STATUS "
                         + "  WHERE RC_NO LIKE '" + sRC.Substring(0, sRC.Length - 1) + "%'";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);

                    if (dsTemp.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToInt32(dsTemp.Tables[0].Rows[0]["RC_END"].ToString()) > 9)
                        {
                            SajetCommon.Show_Message("RC lotsize is full", 0);
                            return;
                        }

                        s_RC_End = sRC.Substring(0, sRC.Length - 1) + dsTemp.Tables[0].Rows[0]["RC_END"].ToString();
                    }

                    // Insert SAJET.G_RC_SPLIT
                    for (int i = 0; i < 2; i++)
                    {
                        sSQL = @"INSERT INTO SAJET.G_RC_SPLIT
                                 (RC_NO,RC_QTY,SOURCE_RC_NO,SOURCE_RC_QTY,TRAVEL_ID,PROCESS_ID,UPDATE_USERID,UPDATE_TIME)
                                 VALUES
                                 (:RC_NO,:RC_QTY,:SOURCE_RC_NO,:SOURCE_RC_QTY,:TRAVEL_ID,:PROCESS_ID,:UPDATE_USERID,:UPDATE_TIME)";

                        object[][] ParamsSplit = new object[8][];
                        ParamsSplit[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SOURCE_RC_NO", sRC };
                        ParamsSplit[1] = new object[] { ParameterDirection.Input, OracleType.Number, "SOURCE_RC_QTY", iCurrentQty };
                        ParamsSplit[2] = new object[] { ParameterDirection.Input, OracleType.Number, "PROCESS_ID", iProcess_Id };
                        ParamsSplit[3] = new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", sUserID };
                        ParamsSplit[4] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime };

                        if (i == 0)
                        {
                            ParamsSplit[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", sRC };
                            ParamsSplit[6] = new object[] { ParameterDirection.Input, OracleType.Number, "RC_QTY", 0 };
                            ParamsSplit[7] = new object[] { ParameterDirection.Input, OracleType.Number, "TRAVEL_ID", lTravel_Id };
                        }
                        else
                        {
                            ParamsSplit[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", s_RC_End };
                            ParamsSplit[6] = new object[] { ParameterDirection.Input, OracleType.Number, "RC_QTY", iCurrentQty };
                            ParamsSplit[7] = new object[] { ParameterDirection.Input, OracleType.Number, "TRAVEL_ID", lTravel_Id };
                        }

                        dsTemp = ClientUtils.ExecuteSQL(sSQL, ParamsSplit);
                    }

                    // Insert SAJET.G_RC_STATUS
                    sSQL = @"INSERT INTO SAJET.G_RC_STATUS (WORK_ORDER,RC_NO,PART_ID,VERSION,ROUTE_ID,FACTORY_ID,PDLINE_ID,STAGE_ID,NODE_ID,
                                                            PROCESS_ID,SHEET_NAME,TERMINAL_ID,TRAVEL_ID,NEXT_NODE,NEXT_PROCESS,CURRENT_STATUS,
                                                            CURRENT_QTY,IN_PROCESS_EMPID,IN_PROCESS_TIME,WIP_IN_QTY,WIP_IN_EMPID,WIP_IN_MEMO,
                                                            WIP_IN_TIME,WIP_OUT_GOOD_QTY,WIP_OUT_SCRAP_QTY,WIP_OUT_EMPID,WIP_OUT_MEMO,
                                                            WIP_OUT_TIME,OUT_PROCESS_EMPID,OUT_PROCESS_TIME,HAVE_SN,PRIORITY_LEVEL,UPDATE_USERID,
                                                            UPDATE_TIME,CREATE_TIME,BATCH_ID,EMP_ID,BONUS_QTY,WORKTIME,INITIAL_QTY,RELEASE)
                                                    SELECT  WORK_ORDER,:RC_NO_NEW,PART_ID,VERSION,ROUTE_ID,FACTORY_ID,PDLINE_ID,STAGE_ID,NODE_ID,
                                                            PROCESS_ID,SHEET_NAME,TERMINAL_ID,:TRAVEL_ID,NEXT_NODE,NEXT_PROCESS,:CURRENT_STATUS,
                                                            CURRENT_QTY,IN_PROCESS_EMPID,IN_PROCESS_TIME,WIP_IN_QTY,WIP_IN_EMPID,WIP_IN_MEMO,
                                                            WIP_IN_TIME,WIP_OUT_GOOD_QTY,WIP_OUT_SCRAP_QTY,WIP_OUT_EMPID,WIP_OUT_MEMO,
                                                            WIP_OUT_TIME,OUT_PROCESS_EMPID,OUT_PROCESS_TIME,HAVE_SN,PRIORITY_LEVEL,:UPDATE_USERID,
                                                            :UPDATE_TIME,:CREATE_TIME,BATCH_ID,:EMP_ID,:BONUS_QTY,:WORKTIME,INITIAL_QTY,:RELEASE
                                                      FROM  SAJET.G_RC_STATUS
                                                     WHERE  RC_NO = :RC_NO";

                    object[][] ParamsNew = new object[11][];
                    ParamsNew[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO_NEW", s_RC_End };
                    ParamsNew[1] = new object[] { ParameterDirection.Input, OracleType.Number, "TRAVEL_ID", lTravel_Id };
                    ParamsNew[2] = new object[] { ParameterDirection.Input, OracleType.Number, "CURRENT_STATUS", 0 };
                    ParamsNew[3] = new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", sUserID };
                    ParamsNew[4] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime };
                    ParamsNew[5] = new object[] { ParameterDirection.Input, OracleType.DateTime, "CREATE_TIME", datExeTime };
                    ParamsNew[6] = new object[] { ParameterDirection.Input, OracleType.Number, "EMP_ID", sUserID };
                    ParamsNew[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", sRC };
                    ParamsNew[8] = new object[] { ParameterDirection.Input, OracleType.Number, "BONUS_QTY", 0 };
                    ParamsNew[9] = new object[] { ParameterDirection.Input, OracleType.Number, "WORKTIME", 0 };
                    ParamsNew[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RELEASE", "N" };
                    dsTemp = ClientUtils.ExecuteSQL(sSQL, ParamsNew);

                    // Update SAJET.G_RC_STATUS
                    sSQL = @"UPDATE SAJET.G_RC_STATUS
                                SET TRAVEL_ID = :TRAVEL_ID,
                                    CURRENT_STATUS = :CURRENT_STATUS,
                                    CURRENT_QTY = :CURRENT_QTY,
                                    UPDATE_USERID = :UPDATE_USERID,
                                    UPDATE_TIME = :UPDATE_TIME,
                                    BONUS_QTY = :BONUS_QTY,
                                    WORKTIME = :WORKTIME,
                                    INITIAL_QTY = :INITIAL_QTY,
                                    RELEASE = :RELEASE
                              WHERE RC_NO = :RC_NO";

                    object[][] Params = new object[10][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.Number, "TRAVEL_ID", lTravel_Id };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.Number, "CURRENT_STATUS", 8 };
                    Params[2] = new object[] { ParameterDirection.Input, OracleType.Number, "CURRENT_QTY", 0 };
                    Params[3] = new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", sUserID };
                    Params[4] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime };
                    Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", sRC };
                    Params[6] = new object[] { ParameterDirection.Input, OracleType.Number, "BONUS_QTY", 0 };
                    Params[7] = new object[] { ParameterDirection.Input, OracleType.Number, "WORKTIME", 0 };
                    Params[8] = new object[] { ParameterDirection.Input, OracleType.Number, "INITIAL_QTY", 0 };
                    Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RELEASE", "N" };
                    dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

                    DialogResult = DialogResult.OK;
                }

                // 報廢
                if (RBtn_Scrap.Checked)
                {
                    // New RC No
                    s_RC_End = "Scrap";

                    // Update SAJET.G_RC_STATUS
                    sSQL = @"UPDATE SAJET.G_RC_STATUS
                                SET TRAVEL_ID = :TRAVEL_ID,
                                    CURRENT_STATUS = :CURRENT_STATUS,
                                    CURRENT_QTY = :CURRENT_QTY,
                                    UPDATE_USERID = :UPDATE_USERID,
                                    UPDATE_TIME = :UPDATE_TIME,
                                    BONUS_QTY = :BONUS_QTY,
                                    WORKTIME = :WORKTIME,
                                    INITIAL_QTY = :INITIAL_QTY,
                                    RELEASE = :RELEASE
                              WHERE RC_NO = :RC_NO";

                    object[][] Params = new object[10][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.Number, "TRAVEL_ID", lTravel_Id };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.Number, "CURRENT_STATUS", 8 };
                    Params[2] = new object[] { ParameterDirection.Input, OracleType.Number, "CURRENT_QTY", 0 };
                    Params[3] = new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", sUserID };
                    Params[4] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime };
                    Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", sRC };
                    Params[6] = new object[] { ParameterDirection.Input, OracleType.Number, "BONUS_QTY", 0 };
                    Params[7] = new object[] { ParameterDirection.Input, OracleType.Number, "WORKTIME", 0 };
                    Params[8] = new object[] { ParameterDirection.Input, OracleType.Number, "INITIAL_QTY", 0 };
                    Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RELEASE", "N" };
                    dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

                    sSQL = @"INSERT INTO SAJET.G_RC_SPLIT
                             (RC_NO,RC_QTY,SOURCE_RC_NO,SOURCE_RC_QTY,TRAVEL_ID,PROCESS_ID,UPDATE_USERID,UPDATE_TIME)
                             VALUES
                             (:RC_NO,:RC_QTY,:SOURCE_RC_NO,:SOURCE_RC_QTY,:TRAVEL_ID,:PROCESS_ID,:UPDATE_USERID,:UPDATE_TIME)";

                    object[][] ParamsSplit = new object[8][];
                    ParamsSplit[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SOURCE_RC_NO", sRC };
                    ParamsSplit[1] = new object[] { ParameterDirection.Input, OracleType.Number, "SOURCE_RC_QTY", iCurrentQty };
                    ParamsSplit[2] = new object[] { ParameterDirection.Input, OracleType.Number, "PROCESS_ID", iProcess_Id };
                    ParamsSplit[3] = new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", sUserID };
                    ParamsSplit[4] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime };
                    ParamsSplit[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", sRC };
                    ParamsSplit[6] = new object[] { ParameterDirection.Input, OracleType.Number, "RC_QTY", 0 };
                    ParamsSplit[7] = new object[] { ParameterDirection.Input, OracleType.Number, "TRAVEL_ID", lTravel_Id };
                    dsTemp = ClientUtils.ExecuteSQL(sSQL, ParamsSplit);

                    sSQL = " SELECT COUNT(*) AS COUNTS"
                         + "   FROM SAJET.G_RC_STATUS "
                         + "  WHERE CURRENT_STATUS IN ('0','1','2','11') "
                         + "    AND WORK_ORDER IN (SELECT WORK_ORDER FROM SAJET.G_RC_STATUS WHERE RC_NO = '" + sRC + "') ";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);

                    if (dsTemp.Tables[0].Rows[0]["COUNTS"].ToString() == "0")
                    {
                        object[][] woParams = new object[2][];
                        sSQL = "UPDATE SAJET.G_WO_BASE"
                             + "   SET UPDATE_USERID = :UPDATE_USERID,"
                             + "       UPDATE_TIME = :UPDATE_TIME,"
                             + "       WO_STATUS = 6"
                             + " WHERE WORK_ORDER IN (SELECT WORK_ORDER FROM SAJET.G_RC_STATUS WHERE RC_NO = '" + sRC + "') ";
                        woParams[0] = new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", sUserID };
                        woParams[1] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime };
                        ClientUtils.ExecuteSQL(sSQL, woParams);

                        sSQL = " INSERT INTO SAJET.G_HT_WO_BASE "
                             + " SELECT * FROM SAJET.G_WO_BASE "
                             + "  WHERE WORK_ORDER IN (SELECT WORK_ORDER FROM SAJET.G_RC_STATUS WHERE RC_NO = '" + sRC + "') ";
                        ClientUtils.ExecuteSQL(sSQL);

                        sSQL = " INSERT INTO MESUSER.G_WO_STATUS (JOB_NUMBER,JOB_STATUS) "
                             + " SELECT WORK_ORDER,6 FROM  SAJET.G_RC_STATUS WHERE RC_NO = '" + sRC + "'";
                        ClientUtils.ExecuteSQL(sSQL);
                    }

                    DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception:" + ex.Message, 0);
            }
        }

        private void btnCANCEL_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
