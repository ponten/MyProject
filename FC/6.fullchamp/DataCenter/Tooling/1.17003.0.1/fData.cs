using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using SajetTable;
using System.Data.OracleClient;

namespace CTooling
{
    public partial class fData : Form
    {
        public fData()
        {
            InitializeComponent();
        }

        public string g_sUserID, g_sUpdateType;
        public string g_sKeyID;
        public DataGridViewRow dataCurrentRow;
        string sSQL, g_sReminder, g_sCompany;
        DataSet dsTemp;
        bool bAppendSucess = false;
        public string g_sformText;


        private void fData_Load(object sender, EventArgs e)
        {
            combToolingType.Items.Clear();
            sSQL = " Select TOOLING_TYPE_ID,TOOLING_TYPE_NO from SAJET.SYS_TOOLING_TYPE "
                 + " WHERE Enabled = 'Y'  "

                 + " ORDER BY TOOLING_TYPE_NO ";
            DataSet ds1 = ClientUtils.ExecuteSQL(sSQL);

            combToolingType.DataSource = ds1.Tables[0];
            combToolingType.ValueMember = "TOOLING_TYPE_ID";
            combToolingType.DisplayMember = "TOOLING_TYPE_NO";
            //for (int i = 0; i <= ds1.Tables[0].Rows.Count - 1; i++)
            //{
            //    combToolingType.Items.Add(ds1.Tables[0].Rows[i]["TOOLING_TYPE_NO"].ToString());
            //}


            g_sCompany = "";
            SajetCommon.SetLanguageControl(this);
            this.Text = g_sformText;
            rbMonthly.Checked = true;
            g_sReminder = "M";

            if (g_sUpdateType == "MODIFY")
            {
                g_sKeyID = dataCurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();

                //if (dataCurrentRow.Cells["TOOLING_TYPE"].Value.ToString() == "Knife" || dataCurrentRow.Cells["TOOLING_TYPE"].Value.ToString() == "刀具")
                //    combToolingType.SelectedIndex = 0;
                //else
                //    combToolingType.SelectedIndex = 1;

                /*
               if (dataCurrentRow.Cells["REMINDER"].Value.ToString() == "Q" || dataCurrentRow.Cells["REMINDER"].Value.ToString() == "季")
               {
                   rbQuarterly.Checked = true;
                   g_sReminder = "Q";
               }
               else if (dataCurrentRow.Cells["REMINDER"].Value.ToString() == "M" || dataCurrentRow.Cells["REMINDER"].Value.ToString() == "月")
               {
                   rbMonthly.Checked = true;
                   g_sReminder = "M";
               }
               else if (dataCurrentRow.Cells["REMINDER"].Value.ToString() == "H" || dataCurrentRow.Cells["REMINDER"].Value.ToString() == "半年")
               {
                   rbHalfYear.Checked = true;
                   g_sReminder = "H";
               }


               if (dataCurrentRow.Cells["COMPANY"].Value.ToString() == "事欣")
               {
                   comboCompany.SelectedIndex = 0;
                   g_sCompany = "P";
               }
               else if (dataCurrentRow.Cells["COMPANY"].Value.ToString() == "富士亨")
               {
                   comboCompany.SelectedIndex = 1;
                   g_sCompany = "F";
               }
               */

                combToolingType.Text = dataCurrentRow.Cells["TOOLING_TYPE_NO"].Value.ToString();
                editToolingNo.Text = dataCurrentRow.Cells["TOOLING_NO"].Value.ToString();
                editToolingName.Text = dataCurrentRow.Cells["TOOLING_NAME"].Value.ToString();
                editToolingDesc.Text = dataCurrentRow.Cells["TOOLING_DESC"].Value.ToString();
                editMaxUseCnt.Text =  dataCurrentRow.Cells["MAX_USED_COUNT"].Value.ToString();
                //editEmail.Text = dataCurrentRow.Cells["EMAIL"].Value.ToString();
                //string sText = dataCurrentRow.Cells["EMAIL"].Value.ToString();
                //editEmail.Text = sText.Split('@')[0];
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

            if (combToolingType.SelectedIndex==-1)
            {
                string sData = labToolingType.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                return;
            }

            if (string.IsNullOrEmpty(editToolingNo.Text))
            {
                string sData = labelToolingNo.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editToolingNo.Focus();
                editToolingNo.SelectAll();
                return;
            }
            /*
            if (string.IsNullOrEmpty(editMaxUseCnt.Text))
            {
                string sData = labMaxUsedCnt.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editMaxUseCnt.Focus();
                editMaxUseCnt.SelectAll();
                return;
            }

            
            if (string.IsNullOrEmpty(editEmail.Text))
            {
                string sData = labEmail.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editEmail.Focus();
                editEmail.SelectAll();
                return;
            }

            if (string.IsNullOrEmpty(comboCompany.Text))
            {
                string sData = labCompany.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                return;
            }
            */

            //檢查TypeName是否重複
            sSQL = " Select * from " + TableDefine.gsDef_Table + " "
                 + " Where TOOLING_NO = '" + editToolingNo.Text + "' ";
            if (g_sUpdateType == "MODIFY")
                sSQL = sSQL + " and " + TableDefine.gsDef_KeyField + " <> '" + g_sKeyID + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                string sData = labelToolingNo.Text + " : " + editToolingNo.Text;
                string sMsg = SajetCommon.SetLanguage("Data Duplicate", 2)
                            + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editToolingNo.Focus();
                editToolingNo.SelectAll();
                return;
            }

            //Update DB
            try
            {
                if (g_sUpdateType == "APPEND")
                {
                    AppendData();
                    bAppendSucess = true;
                    string sMsg = SajetCommon.SetLanguage("Data Append OK", 2) + " !"
                                + Environment.NewLine
                                + SajetCommon.SetLanguage("Append Other Data", 2) + " ?";
                    if (SajetCommon.Show_Message(sMsg, 2) == DialogResult.Yes)
                    {
                        ClearData();

                        editToolingNo.Focus();
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
            string sMaxID = SajetCommon.GetMaxID(TableDefine.gsDef_Table, TableDefine.gsDef_KeyField, 8);
            string sEmail = editEmail.Text; //+ labEmailInfo.Text;
            string sToolingType = combToolingType.SelectedValue.ToString();
            //if (combToolingType.SelectedIndex == 0)
            //    sToolingType = "K";
            //else
            //    sToolingType = "T";


            object[][] Params = new object[7][];
            sSQL = " Insert into SAJET.SYS_TOOLING "
                 + " (TOOLING_ID, TOOLING_NO, TOOLING_NAME, TOOLING_DESC, TOOLING_TYPE_ID, "
                 + "  MAX_USED_COUNT, UPDATE_USERID, UPDATE_TIME )"
                 //+", REMINDER, EMAIL, COMPANY, LAST_MAINTAIN_TIME) "
                 + " Values "
                 + " (:TOOLING_ID, :TOOLING_NO, :TOOLING_NAME, :TOOLING_DESC, :TOOLING_TYPE_ID, "
                 + "  :MAX_USED_COUNT, :UPDATE_USERID, SYSDATE )";
                 //+ ", :REMINDER, :EMAIL, :COMPANY, SYSDATE) ";

            Params[0] = new object[] { ParameterDirection.Input, OracleType.Number, "TOOLING_ID", sMaxID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TOOLING_NO", editToolingNo.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TOOLING_NAME", editToolingName.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TOOLING_DESC", editToolingDesc.Text };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TOOLING_TYPE_ID", sToolingType };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MAX_USED_COUNT", editMaxUseCnt.Text };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", fMain.g_sUserID };
            //Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "REMINDER", g_sReminder };
            //Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "EMAIL", sEmail };
            //Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "COMPANY", g_sCompany };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            fMain.CopyToHistory(sMaxID);
        }

        private void ModifyData()
        {
            string sEmail = editEmail.Text;//+ labEmailInfo.Text;
            string sToolingType = combToolingType.SelectedValue.ToString();
            //if (combToolingType.SelectedIndex == 0)
            //    sToolingType = "K";
            //else
            //    sToolingType = "T";

            object[][] Params = new object[7][];
            sSQL = " Update SAJET.SYS_TOOLING "
                 + " set TOOLING_NO = :TOOLING_NO "
                 + "    ,TOOLING_NAME = :TOOLING_NAME "
                 + "    ,TOOLING_DESC = :TOOLING_DESC "
                 + "    ,TOOLING_TYPE_ID = :TOOLING_TYPE_ID "
                 + "    ,MAX_USED_COUNT = :MAX_USED_COUNT "
                 + "    ,UPDATE_USERID = :UPDATE_USERID "
                 + "    ,UPDATE_TIME = SYSDATE "
                 //+ "    ,REMINDER = :REMINDER "
                 //+ "    ,EMAIL = :EMAIL "
                 //+ "    ,COMPANY = :COMPANY "
                 + " where TOOLING_ID = :TOOLING_ID ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.Number, "TOOLING_ID", g_sKeyID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TOOLING_NO", editToolingNo.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TOOLING_NAME", editToolingName.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TOOLING_DESC", editToolingDesc.Text };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TOOLING_TYPE_ID", sToolingType };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MAX_USED_COUNT", editMaxUseCnt.Text };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", fMain.g_sUserID };
            //Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "REMINDER", g_sReminder };
            //Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "EMAIL", sEmail };
            //Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "COMPANY", g_sCompany };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            fMain.CopyToHistory(g_sKeyID);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (bAppendSucess)
                DialogResult = DialogResult.OK;
        }

        private string GetID(string sTable, string sID, string sField, string sValue)
        {
            string sResID = "0";
            if (sValue != "")
            {
                sSQL = " select " + sID + " from " + sTable
                     + " where " + sField + " = '" + sValue + "' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows.Count > 0)
                    sResID = dsTemp.Tables[0].Rows[0][sID].ToString();
            }
            return sResID;
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
                    if (((ComboBox)panelControl.Controls[i]).Name != "combToolingType")
                        ((ComboBox)panelControl.Controls[i]).SelectedIndex = -1;
                }
            }
        }

        private void rbMonthly_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMonthly.Checked)
                g_sReminder = "M";
            else if (rbQuarterly.Checked)
                g_sReminder = "Q";
            else if (rbHalfYear.Checked)
                g_sReminder = "H";
        }

        private void comboCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            g_sCompany = "";
            /*
            if (comboCompany.SelectedIndex == 0)
            {
                g_sCompany = "P";
                labEmailInfo.Text = "@pec.tw";
            }
            else if (comboCompany.SelectedIndex == 1)
            {
                g_sCompany = "F";
                labEmailInfo.Text = "@foxhunt.com.tw";
            }
            */
        }

        private void editEmail_KeyPress(object sender, KeyPressEventArgs e)
        {
            //輸入數字、字母、部分符號(.-_三種)                   
            if (((e.KeyChar >= '0') && (e.KeyChar <= '9') || (e.KeyChar == '\b')))
            {
                e.Handled = false;
            }
            else if ((e.KeyChar >= 'a' && e.KeyChar <= 'z') || (e.KeyChar >= 'A' && e.KeyChar <= 'Z'))
            {
                e.Handled = false;
            }
            else if (e.KeyChar == '.' || e.KeyChar == '-' || e.KeyChar == '_')
            {
                if (((TextBox)sender).Text.Length == 0)
                {
                    e.KeyChar = (char)Keys.None;
                }
            }
            else
            {
                e.Handled = true;
            }
        }

    }
}
