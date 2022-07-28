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
using SajetFilter;

namespace CPartIQCItem
{
    public partial class fData : Form
    {
        public fData()
        {
            InitializeComponent();
        }
        public string g_sUpdateType, g_sformText;
        public string g_sKeyID, g_sPartID;
        public DataGridViewRow dataCurrentRow;
        string sSQL;
        DataSet dsTemp;
        bool bAppendSucess = false;

        private void fData_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            this.Text = g_sformText;

            //Sampling Rule
            combSamplingRule.Items.Clear();
            combSamplingRule.Items.Add("");
            sSQL = "SELECT Sampling_Rule_Name "
                 + "FROM SAJET.SYS_QC_SAMPLING_RULE "
                 + "WHERE ENABLED = 'Y' "
                 + "ORDER BY Sampling_Rule_Name ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                combSamplingRule.Items.Add(dsTemp.Tables[0].Rows[i]["Sampling_Rule_Name"].ToString());
            }

            if (g_sUpdateType == "MODIFY")
            {
                g_sKeyID = dataCurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();

                editCode.Text = dataCurrentRow.Cells["VENDOR_CODE"].Value.ToString();
                combSamplingRule.SelectedIndex = combSamplingRule.Items.IndexOf(dataCurrentRow.Cells["SAMPLING_RULE_NAME"].Value.ToString());
                editCode.Enabled = false;
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
            string sVendorID = "0";

            if (editCode.Text == "")
            {
                string sData = LabCode.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editCode.Focus();
                editCode.SelectAll();
                return;
            }

            sVendorID = GetID("SAJET.SYS_VENDOR", "VENDOR_ID", "VENDOR_CODE", editCode.Text);
            if (sVendorID == "0")
            {
                SajetCommon.Show_Message("Vendor Error", 0);
                editCode.Focus();
                editCode.SelectAll();
                return;
            }

            //ÀË¬d¬O§_­«½Æ
            sSQL = " Select * from " + TableDefine.gsDef_Table + " "
                 + " Where PART_ID = '" + g_sPartID + "' "
                 + " AND VENDOR_ID = '" + sVendorID + "' ";
            if (g_sUpdateType == "MODIFY")
                sSQL = sSQL + " and RECID <> '" + g_sKeyID + "'";
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
            string sMaxID = SajetCommon.GetMaxID("SAJET.SYS_PART_IQC_VENDOR_RULE", "RECID", 8);
            string sSamplingRuleID = GetID("SAJET.SYS_QC_SAMPLING_RULE", "SAMPLING_RULE_ID", "SAMPLING_RULE_NAME", combSamplingRule.Text);
            string sVendorID = GetID("SAJET.SYS_VENDOR", "VENDOR_ID", "VENDOR_CODE", editCode.Text);
            
            object[][] Params = new object[5][];
            sSQL = " Insert into " + TableDefine.gsDef_Table + " "
                 + " (RECID,PART_ID,VENDOR_ID,SAMPLING_RULE_ID,ENABLED,UPDATE_USERID,UPDATE_TIME) "
                 + " Values "
                 + " (:RECID,:PART_ID,:VENDOR_ID,:SAMPLING_RULE_ID,'Y',:UPDATE_USERID,SYSDATE) ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RECID", sMaxID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", g_sPartID };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_ID", sVendorID };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SAMPLING_RULE_ID", sSamplingRuleID };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            fMain.CopyToHistory(sMaxID);
          //  fMain.CopyToHistory(g_sPartID, sVendorID);
        }
        private void ModifyData()
        {
            string sSamplingRuleID = GetID("SAJET.SYS_QC_SAMPLING_RULE", "SAMPLING_RULE_ID", "SAMPLING_RULE_NAME", combSamplingRule.Text);
            string sVendorID = GetID("SAJET.SYS_VENDOR", "VENDOR_ID", "VENDOR_CODE", editCode.Text);
            
            object[][] Params = new object[5][];
            sSQL = " Update " + TableDefine.gsDef_Table + " "
                 + " set PART_ID =:PART_ID "
                 + "    ,VENDOR_ID =:VENDOR_ID "
                 + "    ,SAMPLING_RULE_ID = :SAMPLING_RULE_ID "
                 + "    ,UPDATE_USERID = :UPDATE_USERID "
                 + "    ,UPDATE_TIME = SYSDATE "
                 + " where RECID =:RECID ";

            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", g_sPartID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_ID", sVendorID };

            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SAMPLING_RULE_ID", sSamplingRuleID };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RECID", g_sKeyID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            //fMain.CopyToHistory(g_sPartID, sVendorID);
            fMain.CopyToHistory(g_sKeyID);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (bAppendSucess)
                DialogResult = DialogResult.OK;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            sSQL = "Select VENDOR_CODE,VENDOR_NAME "
                 + "From SAJET.SYS_VENDOR "                 
                 + "where Enabled = 'Y' "
                 + "Order By VENDOR_CODE ";
            fFilter f = new fFilter();
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                editCode.Text = f.dgvData.CurrentRow.Cells["VENDOR_CODE"].Value.ToString();                
            }
        }

        private string GetID(string sTable, string sFieldID, string sFieldName, string sValue)
        {
            if (string.IsNullOrEmpty(sValue))
                return "0";
            sSQL = "select " + sFieldID + " from " + sTable + " "
                 + "where " + sFieldName + " = '" + sValue + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
                return dsTemp.Tables[0].Rows[0][sFieldID].ToString();
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

        private void editCode_EnabledChanged(object sender, EventArgs e)
        {
            btnSearch.Enabled = editCode.Enabled;
        }

        private void rbtnDefaultItem_Click(object sender, EventArgs e)
        {
            if (g_sUpdateType == "APPEND")
            {
                editCode.Text = string.Empty;
//                editCode.Enabled = rbtnByVendor.Checked;
//                btnSearch.Enabled = rbtnByVendor.Checked;
                editCode.BackColor = Color.White;
                if (editCode.Enabled)
                    editCode.BackColor = Color.Yellow;
            }
        }
    }
}