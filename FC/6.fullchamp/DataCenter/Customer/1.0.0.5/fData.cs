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

namespace CCustomer
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

                editCode.Text = dataCurrentRow.Cells["CUSTOMER_CODE"].Value.ToString();
                editName.Text = dataCurrentRow.Cells["CUSTOMER_NAME"].Value.ToString();
                editDesc.Text = dataCurrentRow.Cells["CUSTOMER_DESC"].Value.ToString();
                editAdd.Text = dataCurrentRow.Cells["CUSTOMER_ADDR"].Value.ToString();
                editTel.Text = dataCurrentRow.Cells["CUSTOMER_TEL"].Value.ToString();
                editWarrantyBuffer.Text = dataCurrentRow.Cells["WARRANTY_BUFFER"].Value.ToString();
                editCostomerPostal.Text = dataCurrentRow.Cells["CUSTOMER_POSTAL"].Value.ToString();
                editPayAddr.Text = dataCurrentRow.Cells["PAY_ADDR"].Value.ToString();
                editShippingAddr.Text = dataCurrentRow.Cells["SHIPPING_ADDR"].Value.ToString();
                editGUINumber.Text = dataCurrentRow.Cells["GUI_NUMBER"].Value.ToString();
                editCustomerFax.Text = dataCurrentRow.Cells["CUSTOMER_FAX"].Value.ToString();
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
            string sWarrantyBuffer = editWarrantyBuffer.Text;
            int iWarrantyBuffer = 0;
            if (!string.IsNullOrEmpty(sWarrantyBuffer))
            {
                try
                {
                    iWarrantyBuffer = Convert.ToInt32(sWarrantyBuffer);
                }
                catch
                {
                    SajetCommon.Show_Message("Warranty Buffer Error", 0);
                    editWarrantyBuffer.Focus();
                    editWarrantyBuffer.SelectAll();
                    return;
                }
            }
            editWarrantyBuffer.Text = iWarrantyBuffer.ToString();
            
            //ÀË¬dCode¬O§_­«½Æ
            sSQL = " Select * from SAJET.SYS_CUSTOMER "
                 + " Where CUSTOMER_CODE = '" + editCode.Text + "' ";
            if (g_sUpdateType == "MODIFY")
                sSQL = sSQL + " and customer_id <> '" + g_sKeyID + "'";
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
            string sMaxID = SajetCommon.GetMaxID("SAJET.SYS_CUSTOMER", "CUSTOMER_ID", 8);

            object[][] Params = new object[13][];
            sSQL = " Insert into SAJET.SYS_CUSTOMER "
                 + " (CUSTOMER_ID,CUSTOMER_CODE,CUSTOMER_DESC,CUSTOMER_NAME,CUSTOMER_ADDR,CUSTOMER_TEL,ENABLED,UPDATE_USERID,UPDATE_TIME,WARRANTY_BUFFER,CUSTOMER_POSTAL,PAY_ADDR,SHIPPING_ADDR,GUI_NUMBER,CUSTOMER_FAX) "
                 + " Values "
                 + " (:CUSTOMER_ID,:CUSTOMER_CODE,:CUSTOMER_DESC,:CUSTOMER_NAME,:CUSTOMER_ADDR,:CUSTOMER_TEL,'Y',:UPDATE_USERID,SYSDATE,:WARRANTY_BUFFER,:CUSTOMER_POSTAL,:PAY_ADDR,:SHIPPING_ADDR,:GUI_NUMBER,:CUSTOMER_FAX) ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_ID", sMaxID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_CODE", editCode.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_DESC", editDesc.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_NAME", editName.Text };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_ADDR", editAdd.Text };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_TEL", editTel.Text };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WARRANTY_BUFFER", editWarrantyBuffer.Text };
            Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_POSTAL", editCostomerPostal.Text };
            Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PAY_ADDR", editPayAddr.Text };
            Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHIPPING_ADDR", editShippingAddr.Text };
            Params[11] = new object[] { ParameterDirection.Input, OracleType.VarChar, "GUI_NUMBER", editGUINumber.Text };
            Params[12] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_FAX", editCustomerFax.Text };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            fMain.CopyToHistory(sMaxID);
        }
        private void ModifyData()
        {
            object[][] Params = new object[13][];
            sSQL = " Update SAJET.SYS_CUSTOMER "
                 + " set CUSTOMER_CODE = :CUSTOMER_CODE "
                 + "    ,CUSTOMER_DESC = :CUSTOMER_DESC "
                 + "    ,CUSTOMER_NAME = :CUSTOMER_NAME "
                 + "    ,CUSTOMER_ADDR = :CUSTOMER_ADDR "
                 + "    ,CUSTOMER_TEL = :CUSTOMER_TEL "
                 + "    ,UPDATE_USERID = :UPDATE_USERID "
                 + "    ,UPDATE_TIME = SYSDATE "
                 + "    ,WARRANTY_BUFFER = :WARRANTY_BUFFER "
                 + "    ,CUSTOMER_POSTAL = :CUSTOMER_POSTAL "
                 + "    ,PAY_ADDR = :PAY_ADDR "
                 + "    ,SHIPPING_ADDR = :SHIPPING_ADDR "
                 + "    ,GUI_NUMBER = :GUI_NUMBER "
                 + "    ,CUSTOMER_FAX = :CUSTOMER_FAX "
                 + " where CUSTOMER_ID = :CUSTOMER_ID ";

            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_CODE", editCode.Text };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_DESC", editDesc.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_NAME", editName.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_ADDR", editAdd.Text };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_TEL", editTel.Text };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WARRANTY_BUFFER", editWarrantyBuffer.Text };
            Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_ID", g_sKeyID };
            Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_POSTAL", editCostomerPostal.Text };
            Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PAY_ADDR", editPayAddr.Text };
            Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHIPPING_ADDR", editShippingAddr.Text };
            Params[11] = new object[] { ParameterDirection.Input, OracleType.VarChar, "GUI_NUMBER", editGUINumber.Text };
            Params[12] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_FAX", editCustomerFax.Text };
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