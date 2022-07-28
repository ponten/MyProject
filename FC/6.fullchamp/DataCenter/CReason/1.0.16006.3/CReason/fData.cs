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

namespace CReason
{
    public partial class fData : Form
    {
        private fMain fMainControl;
        public fData()
        {
            InitializeComponent();
        }
        public fData(fMain f)
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

        private void fData_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.Text = g_sformText;

            combLevel.SelectedIndex = 0;
            if (g_sUpdateType == "MODIFY")
            {
                g_sKeyID = dataCurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();

                editCode.Text = dataCurrentRow.Cells["REASON_CODE"].Value.ToString();
                editDesc.Text = dataCurrentRow.Cells["REASON_DESC"].Value.ToString();
                editDesc2.Text = dataCurrentRow.Cells["REASON_DESC2"].Value.ToString();               
                combLevel.SelectedIndex = combLevel.Items.IndexOf(dataCurrentRow.Cells["CODE_LEVEL"].Value.ToString());
                editParentReason.Text = dataCurrentRow.Cells["PARENT_REASON_CODE"].Value.ToString();
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

            if (editCode.Text == "")
            {
                string sData = LabCode.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);

                editCode.Focus();
                editCode.SelectAll();
                return;
            }

            //檢查Code是否重複
            sSQL = " Select * from " + TableDefine.gsDef_Table
                 + " Where REASON_CODE = '" + editCode.Text + "' ";
            if (g_sUpdateType == "MODIFY")
                sSQL = sSQL + " and " + TableDefine.gsDef_KeyField + " <> '" + g_sKeyID + "'";
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

            //檢查上下階關係
            string sLevel = combLevel.Text;
            string sParentCode = editParentReason.Text;            
            if (!Check_Level(sParentCode, sLevel))
            {                
                return;
            }
            if (editParentReason.Text != "")
            {
                if (!Check_ParentReason(editParentReason.Text))
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
                    if (fMainControl != null) fMainControl.ShowData();
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
            string sMaxID = SajetCommon.GetMaxID("SAJET.SYS_REASON", "REASON_ID", 8);
            string sParentReasonID = GetID("SAJET.SYS_REASON", "REASON_ID", "REASON_CODE", editParentReason.Text);

            object[][] Params = new object[7][];
            sSQL = " Insert into SAJET.SYS_REASON "
                 + " (REASON_ID ,REASON_CODE ,REASON_DESC,REASON_DESC2 "
                 + " ,CODE_LEVEL,PARENT_REASON_ID "
                 + " ,ENABLED,UPDATE_USERID,UPDATE_TIME "
                 + " ) "
                 + " Values "
                 + " (:REASON_ID, :REASON_CODE ,:REASON_DESC,:REASON_DESC2 "
                 + " ,:CODE_LEVEL,:PARENT_REASON_ID "
                 + " ,'Y',:UPDATE_USERID,SYSDATE "
                 + " ) ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "REASON_ID", sMaxID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "REASON_CODE", editCode.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "REASON_DESC", editDesc.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "REASON_DESC2", editDesc2.Text };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CODE_LEVEL", combLevel.Text };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PARENT_REASON_ID", sParentReasonID };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);            

            fMain.CopyToHistory(sMaxID);
        }
        private void ModifyData()
        {
            string sParentReasonID = GetID("SAJET.SYS_REASON", "REASON_ID", "REASON_CODE", editParentReason.Text);

            object[][] Params = new object[7][];
            sSQL = " Update SAJET.SYS_REASON "
                 + " set REASON_CODE = :REASON_CODE "
                 + "    ,REASON_DESC = :REASON_DESC "
                 + "    ,REASON_DESC2 = :REASON_DESC2 "                 
                 + "    ,CODE_LEVEL = :CODE_LEVEL "
                 + "    ,PARENT_REASON_ID = :PARENT_REASON_ID "
                 + "    ,UPDATE_USERID = :UPDATE_USERID "
                 + "    ,UPDATE_TIME = SYSDATE "
                 + " where REASON_ID = :REASON_ID ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "REASON_CODE", editCode.Text };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "REASON_DESC", editDesc.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "REASON_DESC2", editDesc2.Text };            
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CODE_LEVEL", combLevel.Text };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PARENT_REASON_ID", sParentReasonID };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "REASON_ID", g_sKeyID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            fMain.CopyToHistory(g_sKeyID);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (bAppendSucess)
                DialogResult = DialogResult.OK;
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string sLevel = "1";
            if (combLevel.SelectedIndex == -1)
                sLevel = "1";
            else
                sLevel = Convert.ToString(Convert.ToInt32(combLevel.Text) - 1);

            sSQL = "Select Reason_Code,Reason_Desc,Code_Level "
                 + "From SAJET.SYS_REASON "
                 + "Where REASON_CODE Like '" + editParentReason.Text + "%' "
                 + "and CODE_LEVEL = '" + sLevel + "' "
                 + "and ENABLED = 'Y' "
                 + "Order By Reason_Code ";
            fFilter f = new fFilter();           
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                editParentReason.Text = f.dgvData.CurrentRow.Cells["Reason_Code"].Value.ToString();
                KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);
                editParentReason_KeyPress(sender, Key);
            }
            f.Dispose();
        }

        private void editParentReason_KeyPress(object sender, KeyPressEventArgs e)
        {            
            if (e.KeyChar != (char)Keys.Return)
                return;

            if (!Check_ParentReason(editParentReason.Text))
                return;
            Check_Level(editParentReason.Text, combLevel.Text);
        }

        public bool Check_ParentReason(string sReasonCode)
        {
            sSQL = " Select Reason_Id, Reason_Code, Reason_Desc "
                 + " From SAJET.SYS_Reason "
                 + " Where Enabled = 'Y' "
                 + " and Reason_Code = '" + sReasonCode + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);           
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                string sData = LabParentReason.Text + " : " + sReasonCode;
                string sMsg = SajetCommon.SetLanguage("Parent Reason Code Error", 1)
                            + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editParentReason.Focus();
                editParentReason.SelectAll();
                return false;
            }           
            return true;
        }
        public bool Check_Level(string sParentCode, string sLevel)
        {
            //檢查上下階關係
            if (sLevel == "1")
            {
                //若為第一層,Parent Code不可輸入
                if (!string.IsNullOrEmpty(sParentCode))
                {
                    SajetCommon.Show_Message("Level-1 Can not input Parent Reason Code", 0);
                    return false;
                }
            }
            else
            {
                //若不為第一層,Parent Code要輸入
                if (string.IsNullOrEmpty(sParentCode))
                {
                    SajetCommon.Show_Message("Please input Parent Reason Code", 0);
                    return false;
                }
            }
            return true;
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