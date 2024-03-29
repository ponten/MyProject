﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using SajetClass;
using SajetTable;

namespace DefectTypeDll
{
    public partial class fData : Form
    {
        public fData()
        {
            InitializeComponent();
        }
        public string g_sUpdateType, g_sformText;
        public string g_sKeyID;
        public DataGridViewRow dataCurrentRow;
        string sSQL;
        DataSet dsTemp;
        bool bAppendSucess = false;

        private void fData_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.Text = g_sformText;
            if (g_sUpdateType == "MODIFY")
            {
                g_sKeyID = dataCurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();
                editCode.Text = dataCurrentRow.Cells["DEFECT_TYPE_CODE"].Value.ToString();
                editDesc.Text = dataCurrentRow.Cells["DEFECT_TYPE_DESC"].Value.ToString();
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

            if (string.IsNullOrEmpty(editCode.Text.Trim()))
            {
                string sData = LabCode.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editCode.Focus();
                editCode.SelectAll();
                return;
            }
            if (string.IsNullOrEmpty(editCode.Text.Trim()))
            {
                string sData = LabCode.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editCode.Focus();
                editCode.SelectAll();
                return;
            }

            ////檢查Code是否重複
            //sSQL = " Select * from SAJET.SYS_TEST_ITEM_TYPE "
            //     + " Where ITEM_TYPE_CODE = '" + editType.Text + "' ";

            //檢查Label Type是否重複
            sSQL = " Select DEFECT_TYPE_CODE from SAJET.SYS_DEFECT_TYPE "
                 + " Where DEFECT_TYPE_CODE = '" + editCode.Text + "' ";

            if (g_sUpdateType == "MODIFY")
                sSQL = sSQL + " AND DEFECT_TYPE_ID <> '" + g_sKeyID + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                string sData = LabCode.Text + " : " + editCode.Text;
                string sMsg = SajetCommon.SetLanguage("Data Duplicate", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editCode.Focus();
                editCode.SelectAll();
                return;
            }

            sSQL = " Select DEFECT_TYPE_DESC from SAJET.SYS_DEFECT_TYPE "
                 + " Where DEFECT_TYPE_DESC = '" + editDesc.Text + "' ";

            if (g_sUpdateType == "MODIFY")
                sSQL = sSQL + " AND DEFECT_TYPE_ID <> '" + g_sKeyID + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                string sData = LabDesc.Text + " : " + editDesc.Text;
                string sMsg = SajetCommon.SetLanguage("Data Duplicate", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editDesc.Focus();
                editDesc.SelectAll();
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
                    if (SajetCommon.Show_Message(sMsg, 2) == DialogResult.Yes)
                    {
                        ClearData();
                        editCode.Focus();
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
            string sMaxID = SajetCommon.GetMaxID("SAJET.SYS_DEFECT_TYPE", "DEFECT_TYPE_ID", 10);   //ITEM_TYPE_ID的值
            object[][] Params = new object[4][];
            sSQL = " Insert into SAJET.SYS_DEFECT_TYPE "
                 + " (DEFECT_TYPE_ID,DEFECT_TYPE_CODE,DEFECT_TYPE_DESC,UPDATE_USERID,UPDATE_TIME,ENABLED) "
                 + " VALUES "
                 + " (:DEFECT_TYPE_ID,:DEFECT_TYPE_CODE,:DEFECT_TYPE_DESC,:UPDATE_USERID,SYSDATE,'Y') ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DEFECT_TYPE_ID", sMaxID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DEFECT_TYPE_CODE", editCode.Text.Trim() };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DEFECT_TYPE_DESC", editDesc.Text.Trim() };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            fMain.CopyToHistory(sMaxID);
        }
        private void ModifyData()
        {
            object[][] Params = new object[4][];
            sSQL = " Update SAJET.SYS_DEFECT_TYPE "
                 + " SET DEFECT_TYPE_CODE= :DEFECT_TYPE_CODE "
                 + "    ,DEFECT_TYPE_DESC = :DEFECT_TYPE_DESC "
                 + "    ,UPDATE_USERID = :UPDATE_USERID "
                 + "    ,UPDATE_TIME = SYSDATE "
                 + " where DEFECT_TYPE_ID = :DEFECT_TYPE_ID ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DEFECT_TYPE_ID", g_sKeyID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DEFECT_TYPE_CODE", editCode.Text.Trim() };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DEFECT_TYPE_DESC", editDesc.Text.Trim() };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            fMain.CopyToHistory(g_sKeyID);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (bAppendSucess)
                DialogResult = DialogResult.OK;
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