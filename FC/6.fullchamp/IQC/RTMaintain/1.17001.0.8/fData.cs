using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Data.OracleClient;
using SajetFilter;
namespace RTMaintain
{
    public partial class fData : Form
    {
        public fData()
        {
            InitializeComponent();
        }
        String G_sType;
        public String G_sRTID;
        public String G_sEmpID;
        String sSQL;
        public bool G_bAppendOK;
        private string GetVendorID(string sVendorCode)
        {
            sSQL = "select vendor_id,vendor_name "
                  +" from sajet.sys_vendor where vendor_code=:VENDOR_CODE and enabled='Y' and rownum = 1  ";

            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_CODE", sVendorCode };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                lablVendorName.Text = dsTemp.Tables[0].Rows[0]["VENDOR_NAME"].ToString();
                return dsTemp.Tables[0].Rows[0]["VENDOR_ID"].ToString();
            }
            else
                return "0";
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            String sSQL = "";
            DataSet dsTemp;
            String sMaxID;
            editRTNo.Text = editRTNo.Text.Trim();
            editPONo.Text = editPONo.Text.Trim();
            if (string.IsNullOrEmpty(editRTNo.Text))
            {
                SajetCommon.Show_Message("RT No Error!!", 0);
                editRTNo.Focus();
                editRTNo.SelectAll();
                return;
            }
            string sVendorCode = editVendor.Text.Trim();
            if (string.IsNullOrEmpty(sVendorCode))
            {
                SajetCommon.Show_Message("Vendor Error", 0);
                editVendor.Focus();
                editVendor.SelectAll();
                return;
            }
            string sVendorID = GetVendorID(sVendorCode);
            if (sVendorID == "0")
            {
                SajetCommon.Show_Message("Vendor Error", 0);
                editVendor.Focus();
                editVendor.SelectAll();
                return;
            }
            
            try
            {
                //ÀË¬d¬O§_­«½Æ
                sSQL = "select RT_NO from SAJET.G_ERP_RTNO "
                     + "where RT_NO = '" + editRTNo.Text + "' "
                     + "and ENABLED <> 'D' ";
                if (G_sType == "1")
                {
                    sSQL = sSQL + " and RT_ID <> '" + G_sRTID + "'";
                }
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    SajetCommon.Show_Message("RT No Duplicate", 0);
                    return;
                }

                if (G_sType == "0") //append
                {
                    sSQL = "Select RPAD(NVL(PARAM_VALUE,'1'),2,'0') || TO_CHAR(SYSDATE,'YYMMDD') || '001' RTID "
                        + "From SAJET.SYS_BASE Where PARAM_NAME = 'DBID'";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);
                    sMaxID = dsTemp.Tables[0].Rows[0]["RTID"].ToString();
                    sSQL = "Select NVL(Max(RT_ID),0) + 1 RTID "
                        + "From SAJET.G_ERP_RTNO Where RT_ID >= '" + sMaxID + "'";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);
                    if (dsTemp.Tables[0].Rows[0]["RTID"].ToString() != "1")
                    {
                        sMaxID = dsTemp.Tables[0].Rows[0]["RTID"].ToString();
                    }
                    sSQL = "INSERT INTO SAJET.G_ERP_RTNO "
                        + "(RT_ID, RT_NO, VENDOR_ID, PO_NO, UPDATE_USERID)"
                        + " values ('" + sMaxID + "','" + editRTNo.Text + "'," + sVendorID + ",'" + editPONo.Text + "','" + G_sEmpID + "')";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);
                    G_sRTID = sMaxID;
                    if (SajetCommon.Show_Message(SajetCommon.SetLanguage("RT Data Append OK") + " !" + Environment.NewLine
                                               + SajetCommon.SetLanguage("Append Other RT Data") + " ?", 2) != DialogResult.Yes)
                    {
                        DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        editRTNo.Text = "";
                        editPONo.Text = "";
                        editVendor.Text = "";
                        editRTNo.Focus();
                        editRTNo.SelectAll();
                        G_bAppendOK = true;
                    }
                }
                else if (G_sType == "1")
                {
                    sSQL = "UPDATE SAJET.G_ERP_RTNO"
                        + "   set RT_NO='" + editRTNo.Text + "' "
                        + "      ,VENDOR_ID = '" + sVendorID + "' "
                        + "      ,PO_NO ='" + editPONo.Text + "' "
                        + "      ,UPDATE_USERID ='" + G_sEmpID + "' "
                        + "      ,UPDATE_TIME = SYSDATE "
                        + " WHERE RT_ID ='" + G_sRTID + "' ";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);
                    DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);
            }
        }
        public void initial(String sType, String sRTID)
        {
            G_sType = sType;
            G_sRTID = sRTID;
            DataSet dsTemp;
            switch (sType)
            {
                case "0":
                    this.Text = "RT No Data Append";
                    break;
                case "1":
                    this.Text = "RT No Data Modify";
                    sSQL = " SELECT A.RT_NO,A.PO_NO,B.VENDOR_CODE,B.VENDOR_NAME "
                         + "   FROM SAJET.G_ERP_RTNO A,SAJET.SYS_VENDOR B  "
                         + "  WHERE A.RT_ID ='" + G_sRTID + "' "
                         + "    AND A.VENDOR_ID = B.VENDOR_ID(+) "
                         + "    AND ROWNUM = 1 ";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);
                    if (dsTemp.Tables[0].Rows.Count > 0)
                    {
                        editRTNo.Text = dsTemp.Tables[0].Rows[0]["RT_NO"].ToString();
                        editPONo.Text = dsTemp.Tables[0].Rows[0]["PO_NO"].ToString();
                        editVendor.Text = dsTemp.Tables[0].Rows[0]["VENDOR_CODE"].ToString();
                        lablVendorName.Text = dsTemp.Tables[0].Rows[0]["VENDOR_NAME"].ToString();
                    }
                    break;
            }
            editRTNo.Focus();
            editRTNo.SelectAll();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void fData_Load(object sender, EventArgs e)
        {
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            ClientUtils.SetLanguage(this, fMain.g_sExeName);
        }

        private void btnFilterVendor_Click(object sender, EventArgs e)
        {
            try
            {

                string sVendorCode = editVendor.Text.Trim();
                sVendorCode = sVendorCode + "%";
                string sSQL = "Select VENDOR_CODE,VENDOR_NAME FROM SAJET.SYS_VENDOR "
                      + " WHERE VENDOR_CODE LIKE  :VENDOR_CODE "
                      + "  AND ENABLED='Y' "
                      + " ORDER BY VENDOR_CODE ";


                Object[][] Params = new object[1][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_CODE", sVendorCode };
                fFilter f = new fFilter(Params, "1");
                try
                {
                    f.sSQL = sSQL;
                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        if (f.dgvData.CurrentRow != null)
                        {
                            editVendor.Text = f.dgvData.CurrentRow.Cells[0].Value.ToString();
                            lablVendorName.Text = f.dgvData.CurrentRow.Cells[1].Value.ToString();
                        }
                    }
                }
                finally
                {
                    f.Dispose();
                }
            }
            catch
            {
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void editPONo_TextChanged(object sender, EventArgs e)
        {

        }

        private void editVendor_TextChanged(object sender, EventArgs e)
        {
            lablVendorName.Text = "";
        }

        private void editVendor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            string sVendorCode = editVendor.Text.Trim();
            string sVendorID = GetVendorID(sVendorCode);
            if (sVendorID == "0")
            {
                SajetCommon.Show_Message("Vendor Error", 0);
                editVendor.Focus();
                editVendor.SelectAll();
            }
        }
    }
}