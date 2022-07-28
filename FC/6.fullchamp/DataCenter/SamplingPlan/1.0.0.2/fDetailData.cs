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

namespace CSamplingPlan
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
        public int g_nLevel;

        private void fData_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.Text = g_sformText;
            LabPlanName.Text = g_sSamplingName;

            cmbLevel.SelectedIndex = g_nLevel;
            cmbUnit.SelectedIndex = 0;            
            if (g_sUpdateType == "MODIFY")
            {
                g_sKeyID = dataCurrentRow.Cells[TableDefine.gsDef_DtlKeyField].Value.ToString();

                cmbLevel.SelectedIndex = int.Parse(dataCurrentRow.Cells["sampling_level"].Value.ToString());
                txtMinSize.Text = dataCurrentRow.Cells["MIN_LOT_SIZE"].Value.ToString();
                txtMaxSize.Text = dataCurrentRow.Cells["MAX_LOT_SIZE"].Value.ToString();
                txtSize.Text = dataCurrentRow.Cells["SAMPLE_SIZE"].Value.ToString();
                cmbUnit.SelectedIndex = int.Parse(dataCurrentRow.Cells["sampling_unit"].Value.ToString());

                txtCritical.Text = dataCurrentRow.Cells["CRITICAL_REJECT_QTY"].Value.ToString();
                txtMajor.Text = dataCurrentRow.Cells["MAJOR_REJECT_QTY"].Value.ToString();
                txtMinor.Text = dataCurrentRow.Cells["MINOR_REJECT_QTY"].Value.ToString();
            }
            LabLevel.Text = cmbLevel.Text;
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

            if (txtMinSize.Text == "")
            {
                string sData = labLotSize.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                txtMinSize.Focus();
                txtMinSize.SelectAll();
                return;
            }
            if (txtMaxSize.Text == "")
            {
                string sData = labLotSize.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                txtMaxSize.Focus();
                txtMaxSize.SelectAll();
                return;
            }
            if (txtSize.Text == "")
            {
                string sData = labSampleSize.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                txtSize.Focus();
                txtSize.SelectAll();
                return;
            }
            if (txtCritical.Text == "")
            {
                string sData = labCritical.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                txtCritical.Focus();
                txtCritical.SelectAll();
                return;
            }
            if (txtMajor.Text == "")
            {
                string sData = labMajor.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                txtMajor.Focus();
                txtMajor.SelectAll();
                return;
            }
            if (txtMinor.Text == "")
            {
                string sData = labMinor.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                txtMinor.Focus();
                txtMinor.SelectAll();
                return;
            }

            if (cmbUnit.SelectedIndex == 1)
            {
                if (int.Parse(txtSize.Text) > 100 || int.Parse(txtSize.Text) < 1)//數量為%時需介於1-100之間
                {
                    SajetCommon.Show_Message("When unit = %, Sample Size need to between 0 to 100", 0);
                    return;
                }
            }
            if (int.Parse(txtMaxSize.Text) <= int.Parse(txtMinSize.Text))
            {
                SajetCommon.Show_Message("Min Size can't bigger than Max Size", 0);
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
                    g_nLevel = cmbLevel.SelectedIndex;
                    DialogResult = DialogResult.OK;
                }
                else if (g_sUpdateType == "MODIFY")
                {
                    ModifyData();
                    g_nLevel = cmbLevel.SelectedIndex;
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
            object[][] Params = new object[10][];
            sSQL = " Insert into SAJET.SYS_QC_SAMPLING_PLAN_DETAIL "
                 + " (SAMPLING_ID, MIN_LOT_SIZE, MAX_LOT_SIZE, SAMPLE_SIZE"
                 + " , CRITICAL_REJECT_QTY, MAJOR_REJECT_QTY, MINOR_REJECT_QTY"
                 + " , UPDATE_USERID,UPDATE_TIME"
                 + " , SAMPLING_LEVEL, SAMPLING_UNIT) "
                 + " Values "
                 + " (:SAMPLING_ID, :MIN_LOT_SIZE, :MAX_LOT_SIZE, :SAMPLE_SIZE"
                 + " , :CRITICAL_REJECT_QTY, :MAJOR_REJECT_QTY, :MINOR_REJECT_QTY"
                 + " , :UPDATE_USERID,SYSDATE"
                 + " , :SAMPLING_LEVEL, :SAMPLING_UNIT) ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SAMPLING_ID", g_sSamplingID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MIN_LOT_SIZE", txtMinSize.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MAX_LOT_SIZE", txtMaxSize.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SAMPLE_SIZE", txtSize.Text };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CRITICAL_REJECT_QTY", txtCritical.Text };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MAJOR_REJECT_QTY", txtMajor.Text };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MINOR_REJECT_QTY", txtMinor.Text };
            Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SAMPLING_LEVEL", cmbLevel.SelectedIndex };
            Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SAMPLING_UNIT", cmbUnit.SelectedIndex };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
        }
        private void ModifyData()
        {
            object[][] Params = new object[10][];
            sSQL = " Update SAJET.SYS_QC_SAMPLING_PLAN_DETAIL "
                 + " set SAMPLING_ID = :SAMPLING_ID "
                 + "    ,MIN_LOT_SIZE = :MIN_LOT_SIZE "
                 + "    ,MAX_LOT_SIZE = :MAX_LOT_SIZE "
                 + "    ,SAMPLE_SIZE = :SAMPLE_SIZE "
                 + "    ,CRITICAL_REJECT_QTY = :CRITICAL_REJECT_QTY "
                 + "    ,MAJOR_REJECT_QTY = :MAJOR_REJECT_QTY "
                 + "    ,MINOR_REJECT_QTY = :MINOR_REJECT_QTY "
                 + "    ,UPDATE_USERID = :UPDATE_USERID "
                 + "    ,UPDATE_TIME = SYSDATE "
                 + "    ,SAMPLING_LEVEL = :SAMPLING_LEVEL "
                 + "    ,SAMPLING_UNIT = :SAMPLING_UNIT "
                 + " where ROWID = '" + dataCurrentRow.Cells["ROWID"].Value.ToString() + "'";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SAMPLING_ID", g_sSamplingID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MIN_LOT_SIZE", txtMinSize.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MAX_LOT_SIZE", txtMaxSize.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SAMPLE_SIZE", txtSize.Text };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CRITICAL_REJECT_QTY", txtCritical.Text };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MAJOR_REJECT_QTY", txtMajor.Text };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MINOR_REJECT_QTY", txtMinor.Text };
            Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SAMPLING_LEVEL", cmbLevel.SelectedIndex };
            Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SAMPLING_UNIT", cmbUnit.SelectedIndex };
            //Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SAMPLING_UNIT", dataCurrentRow.Cells["ROWID"].Value.ToString() };
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
    }
}