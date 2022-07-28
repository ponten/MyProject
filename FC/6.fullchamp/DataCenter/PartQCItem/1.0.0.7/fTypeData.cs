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

namespace CPartQCItem
{
    public partial class fTypeData : Form
    {
        public fTypeData()
        {
            InitializeComponent();
        }
        public string g_sUpdateType, g_sformText;
        public string g_sKeyID, g_sRECID;
        public DataGridViewRow dataCurrentRow;
        string sSQL;
        DataSet dsTemp;
        bool bAppendSucess = false;

        private void fTypeData_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.Text = g_sformText;

            //Sampling Plan
            combSamplingPlan.Items.Clear();
            combSamplingPlan.Items.Add("");
            sSQL = "SELECT Sampling_Type "
                 + "FROM SAJET.SYS_QC_SAMPLING_PLAN "
                 + "WHERE ENABLED= 'Y' "
                 + "ORDER BY Sampling_Type ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                combSamplingPlan.Items.Add(dsTemp.Tables[0].Rows[i]["Sampling_Type"].ToString());
            }

            if (g_sUpdateType == "MODIFY")
            {
                g_sKeyID = dataCurrentRow.Cells[TableDefine.gsDef_TypeKeyField].Value.ToString();

                editName.Text = dataCurrentRow.Cells["ITEM_TYPE_CODE"].Value.ToString();
                combSamplingPlan.SelectedIndex = combSamplingPlan.Items.IndexOf(dataCurrentRow.Cells["SAMPLING_TYPE"].Value.ToString());
                LabTypeName.Text = dataCurrentRow.Cells["ITEM_TYPE_NAME"].Value.ToString();

                editName.Enabled = false;
                btnSearch.Enabled = false;
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

            string sItemTypeID = GetID("SAJET.SYS_TEST_ITEM_TYPE", "ITEM_TYPE_ID", "ITEM_TYPE_CODE", editName.Text);
            if (sItemTypeID == "0")
            {
                SajetCommon.Show_Message("Item Type Code Error", 0);
                editName.Focus();
                editName.SelectAll();
                return;
            }
            
            if (combSamplingPlan.SelectedIndex >= 1)
            {
                string sSamplingID = GetID("SAJET.SYS_QC_SAMPLING_PLAN", "SAMPLING_ID", "SAMPLING_TYPE", combSamplingPlan.Text);
                if (sSamplingID == "0")
                {
                    SajetCommon.Show_Message("Sampling Plan Error", 0);
                    editName.Focus();
                    editName.SelectAll();
                    return;
                }
            }

            //ÀË¬dName¬O§_­«½Æ
            if (g_sUpdateType == "APPEND")
            {
                sSQL = " Select * from " + TableDefine.gsDef_TypeTable + " "
                     + " Where RECID = '" + g_sRECID + "' "
                     + " AND ITEM_TYPE_ID = '" + sItemTypeID + "' ";             
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
            string sSamplingPlanID = GetID("SAJET.SYS_QC_SAMPLING_PLAN", "SAMPLING_ID", "SAMPLING_TYPE", combSamplingPlan.Text);
            string sItemTypeID = GetID("SAJET.SYS_TEST_ITEM_TYPE", "ITEM_TYPE_ID", "ITEM_TYPE_CODE", editName.Text);
            
            object[][] Params = new object[4][];
            sSQL = " Insert into SAJET.SYS_PART_QC_TESTTYPE "
                 + " (RECID,ITEM_TYPE_ID,SAMPLING_ID,ENABLED,UPDATE_USERID,UPDATE_TIME) "
                 + " Values "
                 + " (:RECID,:ITEM_TYPE_ID,:SAMPLING_ID,'Y',:UPDATE_USERID,SYSDATE) ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RECID", g_sRECID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", sItemTypeID };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SAMPLING_ID", sSamplingPlanID };            
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            fMain.CopyToTypeHistory(sItemTypeID,g_sRECID);
        }
        private void ModifyData()
        {
            string sSamplingPlanID = GetID("SAJET.SYS_QC_SAMPLING_PLAN", "SAMPLING_ID", "SAMPLING_TYPE", combSamplingPlan.Text);            
            
            object[][] Params = new object[4][];
            sSQL = " Update SAJET.SYS_PART_QC_TESTTYPE "
                 + " set SAMPLING_ID = :SAMPLING_ID "                   
                 + "    ,UPDATE_USERID = :UPDATE_USERID "
                 + "    ,UPDATE_TIME = SYSDATE "
                 + " where RECID = :RECID "
                 + " and ITEM_TYPE_ID = :ITEM_TYPE_ID ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SAMPLING_ID", sSamplingPlanID };            
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RECID", g_sRECID };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", g_sKeyID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            fMain.CopyToTypeHistory(g_sKeyID, g_sRECID);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (bAppendSucess)
                DialogResult = DialogResult.OK;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            sSQL = "select a.Item_Type_Code,a.Item_Type_Name "
                 + "from SAJET.SYS_TEST_ITEM_TYPE a "
                 + "where a.Item_Type_Code like '" + editName.Text + "%' "
                 + "and a.enabled = 'Y' "
                 + "Order By a.Item_Type_Code,a.Item_Type_Name ";
            fFilter f = new fFilter();
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                editName.Text = f.dgvData.CurrentRow.Cells["Item_Type_Code"].Value.ToString();
                KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);
                editName_KeyPress(sender, Key);
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

        private void editName_KeyPress(object sender, KeyPressEventArgs e)
        {
            LabTypeName.Text = "";
            if (e.KeyChar != (char)Keys.Return)
                return;

            sSQL = "select Item_Type_Name "
                + "from SAJET.SYS_TEST_ITEM_TYPE "
                + "where Item_Type_Code = '" + editName.Text + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
                LabTypeName.Text = dsTemp.Tables[0].Rows[0]["Item_Type_Name"].ToString();
            else
            {
                SajetCommon.Show_Message("Item Type Code Error", 0);
                editName.Focus();
                editName.SelectAll();
            }
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