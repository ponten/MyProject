using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OracleClient;
using SajetClass;
using ExportExcel;
using SajetFilter;

namespace RCTransfer
{
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();
        }

        public static String rc_UserID = ClientUtils.UserPara1;

        string sSQL;
        DataSet dsTemp;

        string sRC, sTransferDesc;

        private void Initial_Form()
        {
            SajetCommon.SetLanguageControl(this);
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);

            label2.Visible = false;
            RC_dgv.ReadOnly = false;

            SetCombRCSN();
        }

        public void SetCombRCSN()
        {
            combRCSN.Items.Add(SajetCommon.SetLanguage("RC_NO"));
            combRCSN.SelectedIndex = 0;
        }

        private void Excute_button_Click(object sender, EventArgs e)
        {
            int iPart_Id = 0, iRoute_Id = 0, iLine_Id = 0, iFactory_Id = 0;
            int iProcess_Id = 0, iStage_Id = 0;
            long iNode_Id = 0, iNext_Node = 0;
            string sVersion = "", sNext_Process = "", sSheet_Name = "";

            bool IsChecked;
            int n = 0;
            decimal dTransferQty = 0;

            //sTransferDesc = HD_richTextBox.Text.Trim();

            //if (sTransferDesc == "" || sTransferDesc == null)
            //{
            //    string sMsg = SajetCommon.SetLanguage("Transfer Desc is Null");
            //    SajetCommon.Show_Message(sMsg, 1);
            //    return;
            //}

            if (TB_WorkOrder.Text == "")
            {
                string sMsg = SajetCommon.SetLanguage("Please Select Work Order");
                SajetCommon.Show_Message(sMsg, 1);
                return;
            }

            sSQL = " SELECT A.PART_ID,A.VERSION,A.ROUTE_ID,A.DEFAULT_PDLINE_ID,A.FACTORY_ID"
                 + "   FROM SAJET.G_WO_BASE A,SAJET.SYS_PDLINE B"
                 + "  WHERE A.DEFAULT_PDLINE_ID = B.PDLINE_ID"
                 + "    AND WORK_ORDER = '" + TB_WorkOrder.Text + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                iPart_Id = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["PART_ID"].ToString());
                sVersion = dsTemp.Tables[0].Rows[0]["VERSION"].ToString();
                iRoute_Id = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["ROUTE_ID"].ToString());
                iLine_Id = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["DEFAULT_PDLINE_ID"].ToString());
                iFactory_Id = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["FACTORY_ID"].ToString());
            }

            object[][] ParamsProc = new object[7][];
            ParamsProc[0] = new object[] { ParameterDirection.Input, OracleType.Number, "prouteid", iRoute_Id };
            ParamsProc[1] = new object[] { ParameterDirection.Output, OracleType.Number, "vprocessid", "" };
            ParamsProc[2] = new object[] { ParameterDirection.Output, OracleType.Number, "vnextprocess", "" };
            ParamsProc[3] = new object[] { ParameterDirection.Output, OracleType.VarChar, "vsheetname", "" };
            ParamsProc[4] = new object[] { ParameterDirection.Output, OracleType.Number, "vstageid", "" };
            ParamsProc[5] = new object[] { ParameterDirection.Output, OracleType.Number, "vnodeid", "" };
            ParamsProc[6] = new object[] { ParameterDirection.Output, OracleType.Number, "vnextnode", "" };
            DataSet DSProc = ClientUtils.ExecuteProc("SAJET.SJ_GET_FIRSTPROCESS", ParamsProc);

            iProcess_Id = Convert.ToInt32(DSProc.Tables[0].Rows[0]["vprocessid"].ToString());

            if (DSProc.Tables[0].Rows[0]["vnextprocess"].ToString() != "0")
            {
                sNext_Process = DSProc.Tables[0].Rows[0]["vnextprocess"].ToString();
            }

            sSheet_Name = DSProc.Tables[0].Rows[0]["vsheetname"].ToString();
            iStage_Id = Convert.ToInt32(DSProc.Tables[0].Rows[0]["vstageid"].ToString());
            iNode_Id = Convert.ToInt64(DSProc.Tables[0].Rows[0]["vnodeid"].ToString());
            iNext_Node = Convert.ToInt64(DSProc.Tables[0].Rows[0]["vnextnode"].ToString());

            // Check
            for (int i = 0; i < RC_dgv.Rows.Count; i++)
            {
                IsChecked = Convert.ToBoolean(RC_dgv.Rows[i].Cells[0].FormattedValue);

                if (IsChecked)
                {
                    sRC = RC_dgv.Rows[i].Cells[1].Value.ToString();

                    if (Convert.ToInt32(sRC.Substring(sRC.Length - 2, 1)) == 9)
                    {
                        SajetCommon.Show_Message("RC lotsize is full", 0);
                        return;
                    }
                    else
                    {
                        dTransferQty += Convert.ToDecimal(RC_dgv.Rows[i].Cells[3].Value.ToString());
                    }
                }
            }

            if (dTransferQty > Convert.ToDecimal(TB_AvailableQty.Text))
            {
                SajetCommon.Show_Message("Total Current Qty More Than Available Qty", 0);
                return;
            } 

            for (int i = 0; i < RC_dgv.Rows.Count; i++)
            {
                IsChecked = Convert.ToBoolean(RC_dgv.Rows[i].Cells[0].FormattedValue);

                if (IsChecked)
                {
                    n++;

                    sRC = RC_dgv.Rows[i].Cells[1].Value.ToString();

                    // New RC No
                    string s_RC_End = "";

                    sSQL = " SELECT MAX(SUBSTR(RC_NO,-2,1) + 1) AS RC_END "
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

                        s_RC_End = sRC.Substring(0, sRC.Length - 2) + dsTemp.Tables[0].Rows[0]["RC_END"].ToString() + sRC.Substring(sRC.Length - 1, 1);
                    }

                    DateTime datExeTime = DateTime.Now;
                    long lTravel_Id = Convert.ToInt64(datExeTime.ToString("yyyyMMddHHmmssf"));

                    // Insert SAJET.G_RC_SPLIT
                    for (int j = 0; j < 2; j++)
                    {
                        sSQL = @"INSERT INTO SAJET.G_RC_SPLIT
                                 (RC_NO,RC_QTY,SOURCE_RC_NO,SOURCE_RC_QTY,TRAVEL_ID,PROCESS_ID,UPDATE_USERID,UPDATE_TIME)
                                 VALUES
                                 (:RC_NO,:RC_QTY,:SOURCE_RC_NO,:SOURCE_RC_QTY,:TRAVEL_ID,:PROCESS_ID,:UPDATE_USERID,:UPDATE_TIME)";

                        object[][] ParamsSplit = new object[8][];
                        ParamsSplit[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SOURCE_RC_NO", sRC };
                        ParamsSplit[1] = new object[] { ParameterDirection.Input, OracleType.Number, "SOURCE_RC_QTY", Convert.ToDecimal(RC_dgv.Rows[i].Cells[3].Value.ToString()) };
                        ParamsSplit[2] = new object[] { ParameterDirection.Input, OracleType.Number, "PROCESS_ID", iProcess_Id };
                        ParamsSplit[3] = new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", rc_UserID };
                        ParamsSplit[4] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime };

                        if (j == 0)
                        {
                            ParamsSplit[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", sRC };
                            ParamsSplit[6] = new object[] { ParameterDirection.Input, OracleType.Number, "RC_QTY", 0 };
                            ParamsSplit[7] = new object[] { ParameterDirection.Input, OracleType.Number, "TRAVEL_ID", lTravel_Id };
                        }
                        else
                        {
                            ParamsSplit[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", s_RC_End };
                            ParamsSplit[6] = new object[] { ParameterDirection.Input, OracleType.Number, "RC_QTY", Convert.ToDecimal(RC_dgv.Rows[i].Cells[3].Value.ToString()) };
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
                             VALUES (:WORK_ORDER,:RC_NO,:PART_ID,:VERSION,:ROUTE_ID,:FACTORY_ID,:PDLINE_ID,:STAGE_ID,:NODE_ID,
                                     :PROCESS_ID,:SHEET_NAME,:TERMINAL_ID,:TRAVEL_ID,:NEXT_NODE,:NEXT_PROCESS,:CURRENT_STATUS,
                                     :CURRENT_QTY,NULL,NULL,NULL,NULL,NULL,
                                     NULL,NULL,NULL,NULL,NULL,
                                     NULL,NULL,NULL,:HAVE_SN,:PRIORITY_LEVEL,:UPDATE_USERID,
                                     :UPDATE_TIME,:CREATE_TIME,NULL,:EMP_ID,:BONUS_QTY,:WORKTIME,:INITIAL_QTY,:RELEASE)";

                    object[][] ParamsNew = new object[27][];
                    ParamsNew[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", TB_WorkOrder.Text };
                    ParamsNew[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", s_RC_End };
                    ParamsNew[2] = new object[] { ParameterDirection.Input, OracleType.Number, "PART_ID", iPart_Id };
                    ParamsNew[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VERSION", sVersion };
                    ParamsNew[4] = new object[] { ParameterDirection.Input, OracleType.Number, "ROUTE_ID", iRoute_Id };
                    ParamsNew[5] = new object[] { ParameterDirection.Input, OracleType.Number, "FACTORY_ID", iFactory_Id };
                    ParamsNew[6] = new object[] { ParameterDirection.Input, OracleType.Number, "PDLINE_ID", iLine_Id };
                    ParamsNew[7] = new object[] { ParameterDirection.Input, OracleType.Number, "STAGE_ID", iStage_Id };
                    ParamsNew[8] = new object[] { ParameterDirection.Input, OracleType.Number, "NODE_ID", iNode_Id };
                    ParamsNew[9] = new object[] { ParameterDirection.Input, OracleType.Number, "PROCESS_ID", iProcess_Id };
                    ParamsNew[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_NAME", sSheet_Name };
                    ParamsNew[11] = new object[] { ParameterDirection.Input, OracleType.Number, "TERMINAL_ID", 0 };
                    ParamsNew[12] = new object[] { ParameterDirection.Input, OracleType.Number, "TRAVEL_ID", lTravel_Id };
                    ParamsNew[13] = new object[] { ParameterDirection.Input, OracleType.Number, "NEXT_NODE", iNext_Node };
                    ParamsNew[14] = new object[] { ParameterDirection.Input, OracleType.Number, "NEXT_PROCESS", sNext_Process };
                    ParamsNew[15] = new object[] { ParameterDirection.Input, OracleType.Number, "CURRENT_STATUS", 0 };
                    ParamsNew[16] = new object[] { ParameterDirection.Input, OracleType.Number, "CURRENT_QTY", Convert.ToDecimal(RC_dgv.Rows[i].Cells[3].Value.ToString()) };
                    ParamsNew[17] = new object[] { ParameterDirection.Input, OracleType.Number, "HAVE_SN", 0 };
                    ParamsNew[18] = new object[] { ParameterDirection.Input, OracleType.Number, "PRIORITY_LEVEL", Convert.ToInt32(RC_dgv.Rows[i].Cells[5].Value.ToString()) };
                    ParamsNew[19] = new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", rc_UserID };
                    ParamsNew[20] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime };
                    ParamsNew[21] = new object[] { ParameterDirection.Input, OracleType.DateTime, "CREATE_TIME", datExeTime };
                    ParamsNew[22] = new object[] { ParameterDirection.Input, OracleType.Number, "EMP_ID", rc_UserID };
                    ParamsNew[23] = new object[] { ParameterDirection.Input, OracleType.Number, "BONUS_QTY", 0 };
                    ParamsNew[24] = new object[] { ParameterDirection.Input, OracleType.Number, "WORKTIME", 0 };
                    ParamsNew[25] = new object[] { ParameterDirection.Input, OracleType.Number, "INITIAL_QTY", Convert.ToDecimal(RC_dgv.Rows[i].Cells[3].Value.ToString()) };
                    ParamsNew[26] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RELEASE", "N" };
                    ClientUtils.ExecuteSQL(sSQL, ParamsNew);

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
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.Number, "CURRENT_STATUS", 12 };
                    Params[2] = new object[] { ParameterDirection.Input, OracleType.Number, "CURRENT_QTY", 0 };
                    Params[3] = new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", rc_UserID };
                    Params[4] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime };
                    Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", sRC };
                    Params[6] = new object[] { ParameterDirection.Input, OracleType.Number, "BONUS_QTY", 0 };
                    Params[7] = new object[] { ParameterDirection.Input, OracleType.Number, "WORKTIME", 0 };
                    Params[8] = new object[] { ParameterDirection.Input, OracleType.Number, "INITIAL_QTY", 0 };
                    Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RELEASE", "N" };
                    dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

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
                        woParams[0] = new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", rc_UserID };
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
                }
            }

            if (n <= 0)
            {
                string sMsg = SajetCommon.SetLanguage("Please select RC/SN");
                SajetCommon.Show_Message(sMsg, 1);
                return;
            }
            else
            {
                object[][] woParams = new object[2][];
                sSQL = "UPDATE SAJET.G_WO_BASE"
                     + "   SET INPUT_QTY = INPUT_QTY + " + dTransferQty + ","
                     + "       UPDATE_USERID = :UPDATE_USERID,"
                     + "       UPDATE_TIME = SYSDATE,"
                     + "       WO_STATUS = 3"
                     + " WHERE WORK_ORDER = :WORK_ORDER";
                woParams[0] = new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", rc_UserID };
                woParams[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", TB_WorkOrder.Text };
                ClientUtils.ExecuteSQL(sSQL, woParams);

                sSQL = " INSERT INTO SAJET.G_HT_WO_BASE "
                     + " SELECT * FROM SAJET.G_WO_BASE "
                     + "  WHERE WORK_ORDER = '" + TB_WorkOrder.Text + "' ";
                ClientUtils.ExecuteSQL(sSQL);
            }

            RC_dgv.Rows.Clear();
            HD_richTextBox.Clear();
            TB_WorkOrder.Clear();
            TB_AvailableQty.Clear();
            TB_Spec1.Clear();
            TB_Spec2.Clear();
            TB_Version.Clear();
            TB_RouteName.Clear();

            ShowData();

            string sMsg1 = SajetCommon.SetLanguage("Transfer was completed");
            SajetCommon.Show_Message(sMsg1, 3);
        }

        private void QUERY_button_Click(object sender, EventArgs e)
        {
            RC_dgv.Rows.Clear();

            Spec_textBox.Focus();
            HD_richTextBox.Clear();

            TB_WorkOrder.Clear();
            TB_AvailableQty.Clear();
            TB_Spec1.Clear();
            TB_Spec2.Clear();
            TB_Version.Clear();
            TB_RouteName.Clear();

            ShowData();
        }

        public void ShowData()
        {
            if (!string.IsNullOrEmpty(txt_RCSN.Text))
            {
                switch (combRCSN.SelectedIndex)
                {
                    case 0:
                        sSQL = @"SELECT A.RC_NO,SAJET.SJ_RC_STATUS(A.CURRENT_STATUS) RC_STATUS,
                                        DECODE(PROCESS_NAME, '', 'Group', PROCESS_NAME) PROCESS_NAME,A.CURRENT_QTY,A.PRIORITY_LEVEL
                                   FROM SAJET.G_RC_STATUS A,
                                        SAJET.SYS_PROCESS B,
                                        SAJET.SYS_PART C,
                                        SAJET.SYS_PDLINE D,
                                        SAJET.G_WO_BASE E
                                  WHERE A.PROCESS_ID = B.PROCESS_ID(+) 
                                    AND A.PROCESS_ID <> '0'  
                                    AND A.PROCESS_ID IS NOT NULL 
                                    AND A.PART_ID = C.PART_ID 
                                    AND A.CURRENT_STATUS IN (11)
                                    AND A.PDLINE_ID = D.PDLINE_ID
                                    AND A.WORK_ORDER = E.WORK_ORDER
                                    AND A.RELEASE = 'N'
                                    AND E.WO_TYPE <> '轉投工單'                                   
                                    AND A.RC_NO LIKE '%" + txt_RCSN.Text + "%'";
                        break;
                }
            }
            else
            {
                switch (combRCSN.SelectedIndex)
                {
                    case 0:
                        sSQL = @"SELECT RC_NO,SAJET.SJ_RC_STATUS(A.CURRENT_STATUS) RC_STATUS,
                                        DECODE(PROCESS_NAME, '', 'Group', PROCESS_NAME) PROCESS_NAME,A.CURRENT_QTY,A.PRIORITY_LEVEL
                                   FROM SAJET.G_RC_STATUS A,
                                        SAJET.SYS_PROCESS B,
                                        SAJET.SYS_PART C,
                                        SAJET.SYS_PDLINE D,
                                        SAJET.G_WO_BASE E
                                  WHERE A.PROCESS_ID = B.PROCESS_ID(+) 
                                    AND A.PROCESS_ID <> '0'  
                                    AND A.PROCESS_ID IS NOT NULL 
                                    AND A.PART_ID = C.PART_ID
                                    AND A.CURRENT_STATUS IN (11)
                                    AND A.PDLINE_ID = D.PDLINE_ID
                                    AND A.WORK_ORDER = E.WORK_ORDER
                                    AND A.RELEASE = 'N'
                                    AND E.WO_TYPE <> '轉投工單'";
                        break;
                }
            }

            if (PN_textBox.Text != "")
            {
                sSQL = sSQL + " AND D.PDLINE_ID = " + PN_textBox.Text;
            }

            if (WO_textBox.Text.Trim() != "")
            {
                sSQL = sSQL + " AND UPPER(A.WORK_ORDER) LIKE UPPER('%" + WO_textBox.Text + "%')";
            }

            if (PN_textBox.Text == "" && WO_textBox.Text == "" && txt_RCSN.Text == "")
            {
                string sMsg = SajetCommon.SetLanguage("QUERY BUILDER IS NULL") + Environment.NewLine;
                SajetCommon.Show_Message(sMsg, 1);
                return;
            }

            if (PN_textBox.Text == "")
            {

                string sMsg = SajetCommon.SetLanguage("Please Select Line Name") + Environment.NewLine;
                SajetCommon.Show_Message(sMsg, 1);
                return;
            }

            if (combRCSN.SelectedIndex == 0)
            {
                sSQL += " ORDER BY A.RC_NO";
            }
            else
            {
                sSQL += " ORDER BY A.SERIAL_NUMBER";
            }

            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                string sMsg = SajetCommon.SetLanguage("No relevant data");
                label2.Text = sMsg;
                return;
            }
            else
            {
                label2.Visible = false;
            }

            //RC_dgv RC_NO赋值
            if (combRCSN.SelectedIndex == 0)
            {
                RC_dgv.Columns[1].HeaderText = SajetCommon.SetLanguage("RC_NO");

                for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                {
                    RC_dgv.Rows.Add();
                    RC_dgv.Rows[i].Cells[0].Value = false;
                    RC_dgv.Rows[i].Cells[1].Value = dsTemp.Tables[0].Rows[i]["RC_NO"];
                    RC_dgv.Rows[i].Cells[2].Value = dsTemp.Tables[0].Rows[i]["PROCESS_NAME"];
                    RC_dgv.Rows[i].Cells[3].Value = dsTemp.Tables[0].Rows[i]["CURRENT_QTY"];
                    RC_dgv.Rows[i].Cells[4].Value = dsTemp.Tables[0].Rows[i]["RC_STATUS"];
                    RC_dgv.Rows[i].Cells[5].Value = dsTemp.Tables[0].Rows[i]["PRIORITY_LEVEL"];
                }
            }
        }

        private void btnPN_Click(object sender, EventArgs e)
        {
            Spec_textBox.Clear();
            PN_textBox.Clear();
            RC_dgv.Rows.Clear();

            string sSQL = "   SELECT PDLINE_ID,PDLINE_NAME"
                        + "     FROM SAJET.SYS_PDLINE"
                        + "    WHERE ENABLED = 'Y'"
                        + " ORDER BY PDLINE_ID ASC";

            fFilter f = new fFilter();
            f.sSQL = sSQL;
            f.sStatus = "PDLine";

            if (f.ShowDialog() == DialogResult.OK)
            {
                PN_textBox.Text = f.dgvData.CurrentRow.Cells["PDLINE_ID"].Value.ToString();
                Spec_textBox.Text = f.dgvData.CurrentRow.Cells["PDLINE_NAME"].Value.ToString();
            }
        }

        private void Btn_WorkOrder_Click(object sender, EventArgs e)
        {
            TB_WorkOrder.Text = "";
            TB_AvailableQty.Text = "";
            TB_Spec1.Text = "";
            TB_Spec2.Text = "";
            TB_Version.Text = "";
            TB_RouteName.Text = "";

            string sSQL = "   SELECT A.WORK_ORDER,A.WO_TYPE,A.TARGET_QTY-A.INPUT_QTY AS AVAILABLE_QTY,B.SPEC1,B.SPEC2,B.VERSION,C.ROUTE_NAME"
                        + "     FROM SAJET.G_WO_BASE A,SAJET.SYS_PART B,SAJET.SYS_RC_ROUTE C"
                        + "    WHERE A.PART_ID = B.PART_ID"
                        + "      AND A.ROUTE_ID = C.ROUTE_ID"
                        + "      AND A.WO_TYPE = '轉投工單'"
                        + "      AND A.WO_STATUS > 1"
                        + "      AND A.WO_STATUS < 4"
                        + "      AND A.TARGET_QTY - A.INPUT_QTY > 0"
                        + " ORDER BY A.WORK_ORDER ASC";

            fFilter f = new fFilter();
            f.sSQL = sSQL;
            f.sStatus = "WorkOrder";

            if (f.ShowDialog() == DialogResult.OK)
            {
                TB_WorkOrder.Text = f.dgvData.CurrentRow.Cells["WORK_ORDER"].Value.ToString();
                TB_AvailableQty.Text = f.dgvData.CurrentRow.Cells["AVAILABLE_QTY"].Value.ToString();
                TB_Spec1.Text = f.dgvData.CurrentRow.Cells["SPEC1"].Value.ToString();
                TB_Spec2.Text = f.dgvData.CurrentRow.Cells["SPEC2"].Value.ToString();
                TB_Version.Text = f.dgvData.CurrentRow.Cells["VERSION"].Value.ToString();
                TB_RouteName.Text = f.dgvData.CurrentRow.Cells["ROUTE_NAME"].Value.ToString();
            }
        }

        private void btnALLSELECT_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < RC_dgv.Rows.Count; i++)
            {
                RC_dgv.Rows[i].Cells[0].Value = true;
            }
        }

        private void btnALLCANCEL_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < RC_dgv.Rows.Count; i++)
            {
                RC_dgv.Rows[i].Cells[0].Value = false;
            }
        }
    }
}
