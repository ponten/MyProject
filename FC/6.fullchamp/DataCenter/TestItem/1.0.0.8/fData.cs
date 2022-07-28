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

namespace CTestItem
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
                
                editName.Text = dataCurrentRow.Cells["ITEM_TYPE_NAME"].Value.ToString();                
                editCode.Text = dataCurrentRow.Cells["ITEM_TYPE_CODE"].Value.ToString();
                editDesc.Text = dataCurrentRow.Cells["ITEM_TYPE_DESC"].Value.ToString();
                editDesc2.Text = dataCurrentRow.Cells["ITEM_TYPE_DESC2"].Value.ToString();
                editInspQty.Text = dataCurrentRow.Cells["MIN_INSP_QTY"].Value.ToString();
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
            sSQL = " Select * from SAJET.SYS_TEST_ITEM_TYPE "
                 + " Where ITEM_TYPE_CODE = '" + editName.Text + "' ";
            if (g_sUpdateType == "MODIFY")
                sSQL = sSQL + " and ITEM_TYPE_id <> '" + g_sKeyID + "'";
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
            string sMaxID = SajetCommon.GetMaxID("SAJET.SYS_TEST_ITEM_TYPE", "ITEM_TYPE_ID", 10);   //ITEM_TYPE_ID的值
            object[][] Params = new object[7][];
            sSQL = " Insert into SAJET.SYS_TEST_ITEM_TYPE "
                 + " (ITEM_TYPE_ID,ITEM_TYPE_NAME,ITEM_TYPE_CODE,ITEM_TYPE_DESC,ITEM_TYPE_DESC2 "
                 + " ,MIN_INSP_QTY " 
                 + " ,ENABLED,UPDATE_USERID,UPDATE_TIME) "
                 + " Values "
                 + " (:ITEM_TYPE_ID,:ITEM_TYPE_NAME,:ITEM_TYPE_CODE,:ITEM_TYPE_DESC,:ITEM_TYPE_DESC2 "
                 + " ,:MIN_INSP_QTY " 
                 + " ,'Y',:UPDATE_USERID,SYSDATE) ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", sMaxID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_NAME", editName.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_CODE", editCode.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_DESC", editDesc.Text };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_DESC2", editDesc2.Text };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MIN_INSP_QTY", editInspQty.Text };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };            
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            fMain.CopyToHistory(sMaxID);
        }
        private void ModifyData()
        {
            object[][] Params = new object[7][];
            sSQL = " Update SAJET.SYS_TEST_ITEM_TYPE "
                 + " set ITEM_TYPE_CODE = :ITEM_TYPE_CODE "
                 + "    ,ITEM_TYPE_NAME = :ITEM_TYPE_NAME "
                 + "    ,ITEM_TYPE_DESC = :ITEM_TYPE_DESC "
                 + "    ,ITEM_TYPE_DESC2 = :ITEM_TYPE_DESC2 "
                 + "    ,MIN_INSP_QTY = :MIN_INSP_QTY "
                 + "    ,UPDATE_USERID = :UPDATE_USERID "
                 + "    ,UPDATE_TIME = SYSDATE "
                 + " where ITEM_TYPE_ID = :ITEM_TYPE_ID ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_CODE", editCode.Text };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_NAME", editName.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_DESC", editDesc.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_DESC2", editDesc2.Text };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MIN_INSP_QTY", editInspQty.Text };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", g_sKeyID };
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

        private void editInspQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == 8 || e.KeyChar == '.' || e.KeyChar == 13 || e.KeyChar == 46))
            {
                e.KeyChar = (char)Keys.None;
            }
        }
    }
}