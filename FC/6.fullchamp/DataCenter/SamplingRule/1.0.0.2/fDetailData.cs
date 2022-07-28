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

namespace CSamplingRule
{
    public partial class fDetailData : Form
    {
        public fDetailData()
        {
            InitializeComponent();
        }
        public string g_sUpdateType, g_sformText;
        public string g_sKeyID;
        public DataGridViewRow dataCurrentRow;
        string sSQL;
        DataSet dsTemp;
        bool bAppendSucess = false;
        public string g_sSamplingName, g_sSamplingID;

        private void fData_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.Text = g_sformText;
            LabStageName.Text = g_sSamplingName;

            cmbLevel.SelectedIndex = 0;
            cmbNextLevel.SelectedIndex = 0;
                
            if (g_sUpdateType == "MODIFY")
            {
                g_sKeyID = dataCurrentRow.Cells[TableDefine.gsDef_DtlKeyField].Value.ToString();

                cmbLevel.SelectedIndex = cmbLevel.Items.IndexOf(dataCurrentRow.Cells["sampling_level_desc"].Value.ToString());
                cmbNextLevel.SelectedIndex = cmbLevel.Items.IndexOf(dataCurrentRow.Cells["NEXT_SAMPLING_LEVEL_DESC"].Value.ToString());

                txtContinuous.Text = dataCurrentRow.Cells["CONTINUE_CNT"].Value.ToString();
                txtReject.Text = dataCurrentRow.Cells["REJECT_CNT"].Value.ToString();
                txtPass.Text = dataCurrentRow.Cells["PASS_CNT"].Value.ToString();
            }
            cmbLevel.Focus();
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

            if (txtContinuous.Text == "")
            {
                string sData = labContinuous.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                txtContinuous.Focus();
                txtContinuous.SelectAll();
                return;
            }
            if (txtReject.Text == "")
            {
                string sData = labReject.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                txtReject.Focus();
                txtReject.SelectAll();
                return;
            }
            if (txtPass.Text == "")
            {
                string sData = labPass.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                txtPass.Focus();
                txtPass.SelectAll();
                return;
            }

            //Pass Cnt. and Reject Cnt.不能同時為0
            if (txtPass.Text == "0" && txtReject.Text == "0")
            {
                string sMsg = SajetCommon.SetLanguage("Value must be bigger then 0", 1)
                            + Environment.NewLine
                            + labReject.Text +" -- "+ labPass.Text;
                SajetCommon.Show_Message(sMsg, 0);
                txtPass.Focus();
                txtPass.SelectAll();
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

                        cmbLevel.Focus();
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
            string sMaxID = SajetCommon.GetMaxID("SAJET.SYS_QC_SAMPLING_RULE_DETAIL", "DETAIL_ID", 10);
            object[][] Params = new object[8][];
            sSQL = " Insert into SAJET.SYS_QC_SAMPLING_RULE_DETAIL "
                 + " (SAMPLING_RULE_ID, DETAIL_ID "
                 + " , CONTINUE_CNT, REJECT_CNT, PASS_CNT "
                 + " , UPDATE_USERID,UPDATE_TIME"
                 + " , SAMPLING_LEVEL, NEXT_SAMPLING_LEVEL) "
                 + " Values "
                 + " (:SAMPLING_RULE_ID, :DETAIL_ID "
                 + " , :CONTINUE_CNT, :REJECT_CNT, :PASS_CNT "
                 + " , :UPDATE_USERID,SYSDATE "
                 + " , :SAMPLING_LEVEL, :NEXT_SAMPLING_LEVEL) ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SAMPLING_RULE_ID", g_sSamplingID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DETAIL_ID", sMaxID };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CONTINUE_CNT", txtContinuous.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "REJECT_CNT", txtReject.Text };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PASS_CNT", txtPass.Text };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SAMPLING_LEVEL", cmbLevel.SelectedIndex };
            Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NEXT_SAMPLING_LEVEL", cmbNextLevel.SelectedIndex };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
        }
        private void ModifyData()
        {
            object[][] Params = new object[7][];
            sSQL = " Update SAJET.SYS_QC_SAMPLING_RULE_DETAIL "
                 + " set SAMPLING_RULE_ID = :SAMPLING_RULE_ID "
                 + "    ,CONTINUE_CNT = :CONTINUE_CNT "
                 + "    ,REJECT_CNT = :REJECT_CNT "
                 + "    ,PASS_CNT = :PASS_CNT "
                 + "    ,UPDATE_USERID = :UPDATE_USERID "
                 + "    ,UPDATE_TIME = SYSDATE "
                 + "    ,SAMPLING_LEVEL = :SAMPLING_LEVEL "
                 + "    ,NEXT_SAMPLING_LEVEL = :NEXT_SAMPLING_LEVEL "
                 + " where DETAIL_ID = '" + dataCurrentRow.Cells["DETAIL_ID"].Value.ToString() + "'";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SAMPLING_RULE_ID", g_sSamplingID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CONTINUE_CNT", txtContinuous.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "REJECT_CNT", txtReject.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PASS_CNT", txtPass.Text };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SAMPLING_LEVEL", cmbLevel.SelectedIndex };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NEXT_SAMPLING_LEVEL", cmbNextLevel.SelectedIndex };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (bAppendSucess)
                DialogResult = DialogResult.OK;
        }
        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == 8 || e.KeyChar == '.' || e.KeyChar == 13 || e.KeyChar == 46))
            {
                e.KeyChar = (char)Keys.None;
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

        private void txtReject_TextChanged(object sender, EventArgs e)
        {
            if (txtReject.Text != "0" && txtReject.Text != "")
                txtPass.Text = "0";
        }

        private void txtPass_TextChanged(object sender, EventArgs e)
        {
            if (txtPass.Text != "0" && txtPass.Text != "")
                txtReject.Text = "0";
        }
    }
}