using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;

namespace IQCbyLot
{
    public partial class fMPN : Form
    {
        public fMPN()
        {
            InitializeComponent();
        }

        private void fMPN_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            GetIQC_Vendor_Part();
        }

        private void editMPNNo_TextChanged(object sender, EventArgs e)
        {
        }

        private void GetIQC_Vendor_Part()
        {
            string sSQL = "";
            DataSet dsTemp = new DataSet();

            sSQL = " SELECT A.VENDOR_PART_NO,B.EMP_NAME,A.CREATE_TIME,A.STATUS "
                 + " FROM   SAJET.G_IQC_VENDOR_PART A,SAJET.SYS_EMP B "
                 + " WHERE  A.CREATE_USERID=B.EMP_ID "
                 + " AND    LOT_NO='" + lblLotNo_show.Text.Trim() + "'"
                 + " ORDER  BY CREATE_TIME DESC ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            dgvMPN.Rows.Clear();

            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dr = dsTemp.Tables[0].Rows[i];

                dgvMPN.Rows.Add();

                dgvMPN.Rows[dgvMPN.Rows.Count - 1].Cells["VENDOR_PART_NO"].Value = dr["VENDOR_PART_NO"].ToString();
                dgvMPN.Rows[dgvMPN.Rows.Count - 1].Cells["CREATE_USER"].Value = dr["EMP_NAME"].ToString();
                dgvMPN.Rows[dgvMPN.Rows.Count - 1].Cells["CREATE_TIME"].Value = dr["CREATE_TIME"].ToString();
                dgvMPN.Rows[dgvMPN.Rows.Count - 1].Cells["STATUS"].Value = dr["STATUS"].ToString();
                if (dr["STATUS"].ToString() == "NG")
                {
                    dgvMPN.Rows[dgvMPN.Rows.Count - 1].Cells["STATUS"].Style.BackColor = Color.Red;
                }
            }
        }

        private void editMPNNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            string sSQL = "";
            string sPart_No = "";
            string sVendor_Part = "";
            string sStatus = "";
            DataSet dsTemp = new DataSet();
            editMPNNo.Text = editMPNNo.Text.ToString().Trim();
            try
            {
                try
                {
                    // 找出'外部料號'相對應的料號和供應商
                    sSQL = "SELECT B.VENDOR_PART_NO "
                        + "  FROM SAJET.G_IQC_LOT A "
                        + "      ,SAJET.SYS_VENDOR_PART B "
                        + "  WHERE A.LOT_NO ='" + lblLotNo_show.Text + "' "
                        + "    AND A.PART_ID = B.PART_ID "
                        + "    AND A.VENDOR_ID = B.VENDOR_ID ";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);
                    if (dsTemp.Tables[0].Rows.Count == 0)
                    {
                        sSQL = "SELECT B.VENDOR_PART_NO "
                            + "  FROM SAJET.G_IQC_LOT A "
                            + "      ,SAJET.SYS_VENDOR_PART B "
                            + "      ,SAJET.SYS_VENDOR C "
                            + "  WHERE A.LOT_NO ='" + lblLotNo_show.Text + "' "
                            + "    AND A.VENDOR_ID = C.VENDOR_ID "
                            + "    AND B.VENDOR_NAME = C.VENDOR_NAME_ABBR ";
                        dsTemp = ClientUtils.ExecuteSQL(sSQL);
                        if (dsTemp.Tables[0].Rows.Count == 0)
                        {
                            SajetCommon.Show_Message("Not Define Vendor Part No", 0);
                            return;
                        }
                    }
                    sVendor_Part = dsTemp.Tables[0].Rows[0]["VENDOR_PART_NO"].ToString();
                    sStatus = "OK";
                    if (editMPNNo.Text != sVendor_Part)
                    {
                        sStatus = "NG";
                    }
                    sSQL = " INSERT INTO SAJET.G_IQC_VENDOR_PART "
                         + " (VENDOR_PART_NO,CREATE_USERID,CREATE_TIME,STATUS,LOT_NO) "
                         + " VALUES "
                         + " ('" + editMPNNo.Text + "','" + ClientUtils.UserPara1 + "',SYSDATE,'" + sStatus + "','" + lblLotNo_show.Text.Trim() + "') ";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);
                    GetIQC_Vendor_Part();
                }

                finally
                {
                    editMPNNo.Focus();
                    editMPNNo.SelectAll();
                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 1);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void editMPNNo_KeyUp(object sender, KeyEventArgs e)
        {

        }
    }
}