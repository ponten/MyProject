using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using SajetClass;
using SajetTable;

namespace CProcess
{
    public partial class fDetailData : Form
    {
         private fMain fMainControl;
        public fDetailData()
        {
            InitializeComponent();
        }
        public fDetailData(fMain f)
        {
            InitializeComponent();
            fMainControl = f;
        }
        public string g_sUpdateType, g_sformText;
        public string g_sKeyID;
        public DataGridViewRow dataCurrentRow;
        string sSQL;
        DataSet dsTemp;
        bool bAppendSucess = false;
        public string g_sStageName, g_sStageID;

        private void fData_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.Text = g_sformText;
            LabStageName.Text = g_sStageName;

            combOperate.Items.Clear();
            sSQL = "Select TYPE_NAME from sajet.sys_operate_type order by type_name";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                combOperate.Items.Add(dsTemp.Tables[0].Rows[i]["TYPE_NAME"].ToString());
            }

            // 2017.3.19 By Jason
            combWIPERP.Items.Clear();
            combWIPERP.Items.Add("N");
            combWIPERP.Items.Add("Y");
            // 2017.3.19 End

            if (g_sUpdateType == "MODIFY")
            {
                g_sKeyID = dataCurrentRow.Cells[TableDefine.gsDef_DtlKeyField].Value.ToString();

                editName.Text = dataCurrentRow.Cells["PROCESS_NAME"].Value.ToString();
                editDesc.Text = dataCurrentRow.Cells["PROCESS_DESC"].Value.ToString();
                editDesc2.Text = dataCurrentRow.Cells["PROCESS_DESC2"].Value.ToString();
                editCode.Text = dataCurrentRow.Cells["PROCESS_CODE"].Value.ToString();
                combOperate.SelectedIndex = combOperate.Items.IndexOf(dataCurrentRow.Cells["TYPE_NAME"].Value.ToString());

                // 2017.3.19 By Jason
                combWIPERP.SelectedIndex = combWIPERP.Items.IndexOf(dataCurrentRow.Cells["WIP_ERP"].Value.ToString());
                // 2017.3.19 End
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= panelControl.Controls.Count - 1; i++)
            {
                if (panelControl.Controls[i] is TextBox)
                {
                    panelControl.Controls[i].Text = panelControl.Controls[i].Text.Trim();
                }
            }
           
            if (editName.Text == "")
            {
                string sData = LabName.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editName.Focus();
                editName.SelectAll();
                return;
            }
            if (combOperate.SelectedIndex==-1)
            {
                string sData = LabOperateType.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                combOperate.Focus();
                combOperate.SelectAll();
                return;
            }
            // 2017.3.19 By Jason
            if (combWIPERP.SelectedIndex == -1)
            {
                string sData = LabWIPERP.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                combWIPERP.Focus();
                combWIPERP.SelectAll();
                return;
            }
            // 2017.3.19 End


          
            //ÀË¬dName¬O§_­«½Æ
            sSQL = " Select * from SAJET.SYS_PROCESS "
                 + " Where PROCESS_NAME = '" + editName.Text + "' ";
            if (g_sUpdateType == "MODIFY")
                sSQL = sSQL + " and Process_id <> '" + g_sKeyID + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                string sData = LabName.Text + " : " + editName.Text;
                string sMsg = SajetCommon.SetLanguage("Data Duplicate", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editName.Focus();
                editName.SelectAll();
                return;
            }

            //Update DB
            try
            {
                if (g_sUpdateType == "APPEND")
                {
                    AppendData();
                    bAppendSucess = true;
                    string sMsg = SajetCommon.SetLanguage("Data Append OK", 2) + " !" + Environment.NewLine + SajetCommon.SetLanguage("Append Other Data", 2) + " ?";
                    if (fMainControl != null) fMainControl.ShowDetailData();
                    if (SajetCommon.Show_Message(sMsg, 2) == DialogResult.Yes)
                    {
                        ClearData();

                        editName.Focus();
                        return;
                    }
                    DialogResult = DialogResult.OK;
                }
                else if (g_sUpdateType == "MODIFY")
                {
                    ModifyData();
                    DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception : " + ex.Message, 0);
                return;
            }
        }

        private void AppendData()
        {
            string sMaxID = SajetCommon.GetMaxID("SAJET.SYS_PROCESS", "PROCESS_ID", 6);
            string sOperateID = GetOperateID(combOperate.Text);
            object[][] Params = new object[9][];
            sSQL = " Insert into SAJET.SYS_PROCESS "
                 + " (PROCESS_ID,PROCESS_NAME,PROCESS_DESC,PROCESS_DESC2,PROCESS_CODE "
                 + " ,STAGE_ID,OPERATE_ID "
                 + " ,ENABLED,UPDATE_USERID,UPDATE_TIME,WIP_ERP) "
                 + " Values "
                 + " (:PROCESS_ID,:PROCESS_NAME,:PROCESS_DESC,:PROCESS_DESC2,:PROCESS_CODE "
                 + " ,:STAGE_ID,:OPERATE_ID "
                 + ",'Y',:UPDATE_USERID,SYSDATE,:WIP_ERP) ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", sMaxID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_NAME", editName.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_DESC", editDesc.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_DESC2", editDesc2.Text };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_CODE", editCode.Text };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "STAGE_ID", g_sStageID };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPERATE_ID", sOperateID };
            Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WIP_ERP", combWIPERP.Text.Trim() };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            //if (sOperateID == "2" || sOperateID == "5" || sOperateID == "10")
            //{
            //    string sSheetName = "";

            //    if (sOperateID == "5")
            //    {
            //        sSheetName = "RC Output";
            //    }
            //    else
            //    {
            //        sSheetName = "RC Quality";
            //    }

            //    Params = new object[7][];
            //    sSQL = " Insert into SAJET.SYS_RC_PROCESS_SHEET "
            //         + " (PROCESS_ID,SHEET_SEQ,SHEET_NAME,SHEET_PHASE,NEXT_SHEET,LINK_NAME,UPDATE_USERID,UPDATE_TIME) "
            //         + " Values "
            //         + " (:PROCESS_ID,:SHEET_SEQ,:SHEET_NAME,:SHEET_PHASE,:NEXT_SHEET,:LINK_NAME,:UPDATE_USERID,SYSDATE) ";
            //    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", sMaxID };
            //    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_SEQ", 0 };
            //    Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_NAME", "RC Input" };
            //    Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_PHASE", "I" };
            //    Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NEXT_SHEET", sSheetName };
            //    Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LINK_NAME", "NEXT" };
            //    Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            //    dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            //    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", sMaxID };
            //    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_SEQ", 1 };
            //    Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_NAME", sSheetName };
            //    Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_PHASE", "O" };
            //    Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NEXT_SHEET", "END" };
            //    Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LINK_NAME", "NEXT" };
            //    Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            //    dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            //}

            if (sOperateID == "2" || sOperateID == "5" || sOperateID == "10")
            {
                string sSheetName = "";

                if (sOperateID == "5")
                {
                    sSheetName = "RC Output";
                }
                else
                {
                    sSheetName = "RC Quality";
                }

                Params = new object[7][];
                sSQL = " Insert into SAJET.SYS_RC_PROCESS_SHEET "
                     + " (PROCESS_ID,SHEET_SEQ,SHEET_NAME,SHEET_PHASE,NEXT_SHEET,LINK_NAME,UPDATE_USERID,UPDATE_TIME) "
                     + " Values "
                     + " (:PROCESS_ID,:SHEET_SEQ,:SHEET_NAME,:SHEET_PHASE,:NEXT_SHEET,:LINK_NAME,:UPDATE_USERID,SYSDATE) ";
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", sMaxID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_SEQ", 0 };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_NAME", sSheetName };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_PHASE", "O" };
                Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NEXT_SHEET", "END" };
                Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LINK_NAME", "NEXT" };
                Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            }

            fMain.CopyToDetailHistory(sMaxID);
        }
        private void ModifyData()
        {
            string sOperateID = GetOperateID(combOperate.Text);
            object[][] Params = new object[8][];
            sSQL = " Update SAJET.SYS_PROCESS "
                 + " set PROCESS_CODE = :PROCESS_CODE "
                 + "    ,PROCESS_NAME = :PROCESS_NAME "
                 + "    ,PROCESS_DESC = :PROCESS_DESC "
                 + "    ,PROCESS_DESC2 = :PROCESS_DESC2 "
                 + "    ,OPERATE_ID = :OPERATE_ID "
                 + "    ,UPDATE_USERID = :UPDATE_USERID "
                 + "    ,UPDATE_TIME = SYSDATE "
                 + "    ,WIP_ERP = :WIP_ERP "
                 + " where PROCESS_ID = :PROCESS_ID ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_CODE", editCode.Text };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_NAME", editName.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_DESC", editDesc.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_DESC2", editDesc2.Text };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPERATE_ID", sOperateID };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };            
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", g_sKeyID };
            Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WIP_ERP", combWIPERP.Text.Trim() };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            //if (sOperateID == "2" || sOperateID == "5" || sOperateID == "10")
            //{
            //    string sSheetName = "";

            //    if (sOperateID == "5")
            //    {
            //        sSheetName = "RC Output";
            //    }
            //    else
            //    {
            //        sSheetName = "RC Quality";
            //    }

            //    sSQL = " DELETE SAJET.SYS_RC_PROCESS_SHEET WHERE PROCESS_ID = " + g_sKeyID;
            //    ClientUtils.ExecuteSQL(sSQL);

            //    Params = new object[7][];
            //    sSQL = " Insert into SAJET.SYS_RC_PROCESS_SHEET "
            //         + " (PROCESS_ID,SHEET_SEQ,SHEET_NAME,SHEET_PHASE,NEXT_SHEET,LINK_NAME,UPDATE_USERID,UPDATE_TIME) "
            //         + " Values "
            //         + " (:PROCESS_ID,:SHEET_SEQ,:SHEET_NAME,:SHEET_PHASE,:NEXT_SHEET,:LINK_NAME,:UPDATE_USERID,SYSDATE) ";
            //    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", g_sKeyID };
            //    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_SEQ", 0 };
            //    Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_NAME", "RC Input" };
            //    Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_PHASE", "I" };
            //    Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NEXT_SHEET", sSheetName };
            //    Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LINK_NAME", "NEXT" };
            //    Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            //    dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            //    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", g_sKeyID };
            //    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_SEQ", 1 };
            //    Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_NAME", sSheetName };
            //    Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_PHASE", "O" };
            //    Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NEXT_SHEET", "END" };
            //    Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LINK_NAME", "NEXT" };
            //    Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            //    dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            //}

            if (sOperateID == "2" || sOperateID == "5" || sOperateID == "10")
            {
                string sSheetName = "";

                if (sOperateID == "5")
                {
                    sSheetName = "RC Output";
                }
                else
                {
                    sSheetName = "RC Quality";
                }

                sSQL = " DELETE SAJET.SYS_RC_PROCESS_SHEET WHERE PROCESS_ID = " + g_sKeyID;
                ClientUtils.ExecuteSQL(sSQL);

                Params = new object[7][];
                sSQL = " Insert into SAJET.SYS_RC_PROCESS_SHEET "
                     + " (PROCESS_ID,SHEET_SEQ,SHEET_NAME,SHEET_PHASE,NEXT_SHEET,LINK_NAME,UPDATE_USERID,UPDATE_TIME) "
                     + " Values "
                     + " (:PROCESS_ID,:SHEET_SEQ,:SHEET_NAME,:SHEET_PHASE,:NEXT_SHEET,:LINK_NAME,:UPDATE_USERID,SYSDATE) ";
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", g_sKeyID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_SEQ", 0 };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_NAME", sSheetName };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_PHASE", "O" };
                Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NEXT_SHEET", "END" };
                Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LINK_NAME", "NEXT" };
                Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            }

            fMain.CopyToDetailHistory(g_sKeyID);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (bAppendSucess)
                DialogResult = DialogResult.OK;
        }

        private string GetOperateID(string sTypeName)
        {
            sSQL = " Select OPERATE_ID from SAJET.SYS_OPERATE_TYPE "
                 + " Where TYPE_NAME = '" + sTypeName + "' ";          
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
                return dsTemp.Tables[0].Rows[0]["OPERATE_ID"].ToString();
            else
                return "0";
        }

        private void ClearData()
        {
            for (int i = 0; i <= panelControl.Controls.Count - 1; i++)
            {
                if (panelControl.Controls[i] is TextBox)
                {
                    panelControl.Controls[i].Text = "";
                }
                else if (panelControl.Controls[i] is ComboBox)
                {
                    ((ComboBox)panelControl.Controls[i]).SelectedIndex = -1;
                }
            }
        }
    }
}