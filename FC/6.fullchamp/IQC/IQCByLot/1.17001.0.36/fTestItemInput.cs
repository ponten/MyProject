using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Data.OracleClient;

namespace IQCbyLot
{
    public partial class fTestItemInput : Form
    {
        public fTestItemInput()
        {
            InitializeComponent();
        }
        public string g_sLotNo,g_sUserID,g_sType,g_sfSerialNumber;
        string sSQL;
        //string f_sSerialNumber, f_sDateCode, f_sTestItemValue, f_sRemark;

        private void btnOK_Click(object sender, EventArgs e)
        {
            g_sfSerialNumber = txtSN.Text.Trim();
            if (string.IsNullOrEmpty(txtSN.Text))
            {
                SajetCommon.Show_Message("Please Input Serial Number", 0);
                return;
            }
            try
            {
                if (g_sType == "INSERT")
                {
                    sSQL = "INSERT INTO SAJET.G_IQC_SN (LOT_NO,SERIAL_NUMBER,DATECODE,TEST_VALUE,REMARK,CREATE_TIME,CREATE_USERID) "
                         + "VALUES (:LOT_NO,:SERIAL_NUMBER,:DATECODE,:TEST_VALUE,:REMARK,:CREATE_TIME,:CREATE_USERID) ";
                }

                if (g_sType == "MODIFY")
                {
                    sSQL = "UPDATE SAJET.G_IQC_SN "
                         + "SET SERIAL_NUMBER = :SERIAL_NUMBER,DATECODE=:DATECODE,TEST_VALUE=:TEST_VALUE,REMARK=:REMARK,CREATE_TIME=:CREATE_TIME,CREATE_USERID=:CREATE_USERID "
                         + "WHERE SERIAL_NUMBER = :SERIAL_NUMBER AND LOT_NO = :LOT_NO ";
                }
                object[][] Params = new object[7][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", g_sLotNo };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SERIAL_NUMBER", g_sfSerialNumber };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DATECODE", txtDateCode.Text };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TEST_VALUE", txtTestValue.Text };
                Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "REMARK", txtRemark.Text };
                Params[5] = new object[] { ParameterDirection.Input, OracleType.DateTime, "CREATE_TIME", DateTime.Now.ToString() };
                Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CREATE_USERID", g_sUserID };
                DataSet ds = ClientUtils.ExecuteSQL(sSQL, Params);
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message,0);                
            }
            
           
            if (g_sType == "INSERT")
            {
                ClearData();
            }                
            
            if (g_sType == "MODIFY")
            {
                SajetCommon.Show_Message("Update Complete", 4);
                DialogResult = DialogResult.OK;
            }
            txtSN.Focus();
        }
        private void ClearData()
        {
            txtSN.Text = string.Empty;
            txtDateCode.Text = string.Empty;
            txtTestValue.Text = string.Empty;
            txtRemark.Text = string.Empty;
        }
        
        private void fTestItemInput_Load(object sender, EventArgs e)
        {            
            SajetCommon.SetLanguageControl(this);
            this.Text = SajetCommon.SetLanguage(g_sType);
            if (g_sType == "MODIFY")
            {
                txtSN.Enabled = false;
                string sSQL = "SELECT SERIAL_NUMBER,DATECODE,TEST_VALUE,REMARK FROM SAJET.G_IQC_SN "
                            + "WHERE SERIAL_NUMBER = :SERIAL_NUMBER "
                            + "AND LOT_NO = :LOT_NO "; 
                object[][] Params = new object[2][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SERIAL_NUMBER", g_sfSerialNumber };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", g_sLotNo };
                DataSet ds = ClientUtils.ExecuteSQL(sSQL, Params);
                txtSN.Text = ds.Tables[0].Rows[0][0].ToString();
                txtDateCode.Text = ds.Tables[0].Rows[0][1].ToString();
                txtTestValue.Text = ds.Tables[0].Rows[0][2].ToString();
                txtRemark.Text = ds.Tables[0].Rows[0][3].ToString();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void txtSN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            txtDateCode.Focus();
        }

        private void txtDateCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            txtTestValue.Focus();
        }

        private void txtTestValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            txtRemark.Focus();
        }

        private void txtRemark_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            btnOK.Focus();
        }

        private void fTestItemInput_Shown(object sender, EventArgs e)
        {
            txtSN.Focus();
            txtSN.SelectAll();
        }
    }
}