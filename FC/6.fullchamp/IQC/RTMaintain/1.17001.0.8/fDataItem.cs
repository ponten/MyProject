using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using SajetFilter;
namespace RTMaintain
{
    public partial class fDataItem : Form
    {
        public fDataItem()
        {
            InitializeComponent();
        }
        public string G_sRTID;
        public bool G_bAppendOK;
        public string G_sType;
        public string G_sRowID;

        private string GetPartID(string sPartNO)
        {
            try
            {
                string sSQL = "SELECT PART_ID FROM SAJET.SYS_PART "
                           + " WHERE PART_NO ='" + sPartNO + "' "
                           + "   AND ROWNUM = 1 ";
                DataSet dstemp = new DataSet();
                dstemp = ClientUtils.ExecuteSQL(sSQL);
                if (dstemp.Tables[0].Rows.Count == 0)
                {
                    SajetCommon.Show_Message("'Part No' Error", 0);
                    return "0";
                }
                else
                    return dstemp.Tables[0].Rows[0]["PART_ID"].ToString();
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("GetPartID," + ex.Message, 0);
                return "0";
            }
        }
        private string GetWareHouseID(string sWH)
        {
            try
            {
                string sSQL = "SELECT WAREHOUSE_ID FROM SAJET.SYS_WAREHOUSE "
                           + " WHERE WAREHOUSE_NAME ='" + sWH + "' "
                           + "   AND ROWNUM = 1 ";
                DataSet dstemp = new DataSet();
                dstemp = ClientUtils.ExecuteSQL(sSQL);
                if (dstemp.Tables[0].Rows.Count == 0)
                {
                    SajetCommon.Show_Message("Warehouse Name Error", 0);
                    return "0";
                }
                else
                    return dstemp.Tables[0].Rows[0]["WAREHOUSE_ID"].ToString();
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("GetWarehouseID," + ex.Message, 0);
                return "0";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string sSQL = "";
            DataSet dsTemp = new DataSet();
            editQty.Text = editQty.Text.Trim();
            editPartNo.Text = editPartNo.Text.Trim();
            editWarehouse.Text = editWarehouse.Text.Trim();
            if (string.IsNullOrEmpty(editPartNo.Text))
            {
                SajetCommon.Show_Message("Please Input Part No", 0);
                editPartNo.Focus();
                editPartNo.SelectAll();
                return;
            }
            if (string.IsNullOrEmpty(editQty.Text))
            {
                SajetCommon.Show_Message("Please input Qty", 0);
                editQty.Focus();
                editQty.SelectAll();
                return;
            }

            int iQty = 0;
            try
            {
                iQty = Convert.ToInt32(editQty.Text);
            }
            catch
            {
                SajetCommon.Show_Message("Qty Error", 0);
                editQty.Focus();
                editQty.SelectAll();
                return;
            }
            try
            {
                string sLotNo = editLot.Text.Trim();
                string sPartID = GetPartID(editPartNo.Text);
                if (sPartID == "0")
                {
                    editPartNo.Focus();
                    editPartNo.SelectAll();
                    return;
                }
                string sWarehouseID = "0";
                if (!string.IsNullOrEmpty(editWarehouse.Text))
                {
                    sWarehouseID = GetWareHouseID(editWarehouse.Text);
                }
                string sUrgentType = "N";
                if (rbtnUrgentYes.Checked)
                    sUrgentType = "Y";
                string sRDFlag = "N";
                if (rbtnRDFlagYes.Checked)
                    sRDFlag = "Y";
                if (G_sType == "0") //append
                {
                    /* mark by rita 2016/07/07 新增時,項次會不同,應允許同一張RT單下相同料號存在                     * 
                    sSQL = "SELECT RT_ID "
                        + "  FROM SAJET.G_ERP_RT_ITEM "
                        + " WHERE RT_ID ='" + G_sRTID + "' "
                        + "   AND PART_ID ='" + sPartID + "' "
                        + "   AND ROWNUM = 1 ";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);
                    if (dsTemp.Tables[0].Rows.Count > 0)
                    {
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("RT Item Duplicate") + Environment.NewLine + SajetCommon.SetLanguage("Part No")+" : " + editPartNo.Text, 0);
                        editPartNo.Focus();
                        editPartNo.SelectAll();
                        return;
                    }
                     */ 
                    sSQL = "select nvl(max(rt_seq), 0) + 1 from sajet.g_erp_rt_item "
                        + "where rt_id = '" + G_sRTID + "' ";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);
                    string sRTSeq = dsTemp.Tables[0].Rows[0][0].ToString();
                    sSQL = "Insert Into SAJET.G_ERP_RT_ITEM "
                        + "(RT_ID, RT_SEQ, PART_ID, PART_VERSION, INCOMING_QTY, DATECODE, VENDOR_LOTNO, VENDOR_PARTNO, UPDATE_USERID,LOCATION,URGENT_TYPE ,LOT_NO, RD_FLAG, PO_NO ) "
                        + " values "
                        + "(:RTID, :RTSeq, :PartID, :Ver, :Qty, :DateCode, :VendorLot, :VendorPN, :UserID, :WarehouseID, :UrgentType, :LotNo, :RDFlag, :PoNo)";

                    object[][] Params = new object[14][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RTID", G_sRTID };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RTSeq", sRTSeq };
                    Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PartID", sPartID };
                    Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "Ver", cmbVer.Text };
                    Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "Qty", editQty.Text };
                    Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DateCode", editDateCode.Text };
                    Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VendorLot", editVendorLot.Text };
                    Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VendorPN", editVendorPN.Text };
                    Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UserID", fMain.g_sUserID };
                    Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WarehouseID", sWarehouseID };
                    Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UrgentType", sUrgentType };
                    Params[11] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LotNo", sLotNo };
                    Params[12] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RDFlag", sRDFlag };
                    Params[13] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PoNo", editPONo.Text };

                    dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

                    sSQL = " INSERT INTO SAJET.G_HT_ERP_RT_ITEM "
                    + " SELECT * FROM SAJET.G_ERP_RT_ITEM "
                    + " WHERE RT_ID ='" + G_sRTID + "' "
                    + "  AND RT_SEQ = '" + sRTSeq + "' ";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);


                    if (SajetCommon.Show_Message(SajetCommon.SetLanguage("RT Item Append OK") + Environment.NewLine
                                               + SajetCommon.SetLanguage("Append Other RT Item")+" ?", 2) != DialogResult.Yes)
                    {
                        DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        editPartNo.Text = "";
                        editQty.Text = "";
                        editVendorLot.Text = "";
                        editVendorPN.Text = "";
                        cmbVer.Text = "";
                        cmbVer.SelectedIndex = -1;
                        editPartNo.Focus();
                        editPartNo.SelectAll();
                        G_bAppendOK = true;
                    }
                }
                if (G_sType == "1")
                {
                    sSQL = "SELECT RT_ID "
                        + "  FROM SAJET.G_ERP_RT_ITEM "
                        + " WHERE RT_ID ='" + G_sRTID + "' "
                        + "   AND PART_ID ='" + sPartID + "' "
                        + "   AND ROWNUM = 1 "
                        + "   AND ROWID <>'" + G_sRowID + "' ";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);
                    if (dsTemp.Tables[0].Rows.Count > 0)
                    {
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("RT Item Duplicate")+ Environment.NewLine + SajetCommon.SetLanguage("Part No")+" : " + editQty.Text, 0);
                        editQty.Focus();
                        editQty.SelectAll();
                        return;

                    }
                    sSQL = " update SAJET.G_ERP_RT_ITEM "
                        + "    SET PART_VERSION =:Ver "
                        + "       ,PART_ID =:PartID "
                        + "       ,INCOMING_QTY =:Qty "
                        + "       ,DATECODE = :DateCode "
                        + "       ,VENDOR_LOTNO =:VendorLot "
                        + "       ,VENDOR_PARTNO = :VendorPN "
                        + "       ,UPDATE_USERID =:UserID "
                        + "       ,UPDATE_TIME = SYSDATE "
                        + "       ,LOCATION =:WarehouseID "
                        + "       ,URGENT_TYPE =:UrgentType "
                        + "       ,LOT_NO =:LotNo "
                        + "       ,RD_FLAG =:RDFlag "
                        + "       ,PO_NO =:PoNo "
                        + " WHERE ROWID =:unID ";

                    object[][] Params = new object[13][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "Ver", cmbVer.Text };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PartID", sPartID };
                    Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "Qty", iQty };
                    Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DateCode", editDateCode.Text };
                    Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VendorLot", editVendorLot.Text };
                    Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VendorPN", editVendorPN.Text };
                    Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UserID", fMain.g_sUserID };
                    Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WarehouseID", sWarehouseID };
                    Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UrgentType", sUrgentType };
                    Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LotNo", sLotNo };
                    Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RDFlag", sRDFlag };
                    Params[11] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PoNo", editPONo.Text };
                    Params[12] = new object[] { ParameterDirection.Input, OracleType.VarChar, "unID", G_sRowID };

                    dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

                    sSQL = " INSERT INTO SAJET.G_HT_ERP_RT_ITEM "
                     + " SELECT * FROM SAJET.G_ERP_RT_ITEM "
                     + " WHERE ROWID ='" + G_sRowID + "' ";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);

                    SajetCommon.Show_Message(SajetCommon.SetLanguage("RT Item Update OK"), 3);
                    DialogResult = DialogResult.OK;
                }
            }

            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);
            }
        }
        public void initial(String sType, String sRTID, string sRTNo, string sVendor, string sRowID)
        {
            G_bAppendOK = false;
            G_sType = sType;
            G_sRTID = sRTID;
            G_sRowID = sRowID;
            lablRTNo.Text = sRTNo;
            lablVendor.Text = sVendor;
            string sSQL = "";
            DataSet dsTemp = new DataSet();
            switch (sType)
            {
                case "0":
                    this.Text = "RT Item Data Maintain Append";
                    break;
                case "1":
                    this.Text = "RT Item Data Maintain Modify";
                    sSQL = "SELECT A.*,B.PART_NO,C.WAREHOUSE_NAME "
                          + " FROM SAJET.G_ERP_RT_ITEM A ,SAJET.SYS_PART B,SAJET.SYS_WAREHOUSE C  "
                          + " WHERE A.ROWID ='" + sRowID + "' "
                          + "   AND A.PART_ID = B.PART_ID(+) "
                          + "   AND A.LOCATION = C.WAREHOUSE_ID(+) "
                          + "   AND ROWNUM = 1 ";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);
                    editPartNo.Text = dsTemp.Tables[0].Rows[0]["PART_NO"].ToString();
                    ShowVersion();
                    cmbVer.SelectedIndex = cmbVer.Items.IndexOf(dsTemp.Tables[0].Rows[0]["PART_VERSION"].ToString());
                    editQty.Text = dsTemp.Tables[0].Rows[0]["INCOMING_QTY"].ToString();
                    editDateCode.Text = dsTemp.Tables[0].Rows[0]["DATECODE"].ToString();
                    editVendorLot.Text = dsTemp.Tables[0].Rows[0]["VENDOR_LOTNO"].ToString();
                    editVendorPN.Text = dsTemp.Tables[0].Rows[0]["VENDOR_PARTNO"].ToString();
                    editWarehouse.Text = dsTemp.Tables[0].Rows[0]["WAREHOUSE_NAME"].ToString();
                    editLot.Text = dsTemp.Tables[0].Rows[0]["LOT_NO"].ToString();
                    editPONo.Text = dsTemp.Tables[0].Rows[0]["PO_NO"].ToString();
                    string sUrgenttype = dsTemp.Tables[0].Rows[0]["URGENT_TYPE"].ToString();
                    if (sUrgenttype == "Y")
                        rbtnUrgentYes.Checked = true;
                    else
                        rbtnUrgentNo.Checked = true;
                    string sRDFlag = dsTemp.Tables[0].Rows[0]["RD_FLAG"].ToString();
                    if (sRDFlag == "Y")
                        rbtnRDFlagYes.Checked = true;
                    else
                        rbtnRDFlagNo.Checked = true;

                    break;
            }
            editQty.Focus();
            editQty.SelectAll();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            String sSQL = "";
            editPartNo.Text = editPartNo.Text.Trim();
            if (string.IsNullOrEmpty(editPartNo.Text))
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Input Part No Prefix"),0);
                editPartNo.Focus();
                editPartNo.SelectAll();
                return;
            }
            sSQL = "  select PART_NO, SPEC1, SPEC2, VERSION "
                 + "    from SAJET.SYS_PART "
                 + "   where PART_NO LIKE '"+editPartNo.Text+"%' "
                 + "     and ENABLED='Y' "
                 + "  order by PART_NO ";
            fFilter f = new fFilter();     
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                editPartNo.Text = f.dgvData.CurrentRow.Cells["PART_NO"].Value.ToString();
                string sVer = f.dgvData.CurrentRow.Cells["VERSION"].Value.ToString();
                ShowVersion();
                cmbVer.SelectedIndex = cmbVer.Items.IndexOf(sVer);
            }
        }

        private void editQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == 8 ||e.KeyChar == 13 || e.KeyChar == 46))
            {
                e.KeyChar = (char)Keys.None;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void fDataItem_Load(object sender, EventArgs e)
        {
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            ClientUtils.SetLanguage(this, fMain.g_sExeName);
        }

        private void editPartNo_KeyDown(object sender, KeyEventArgs e)
        {
            cmbVer.Items.Clear();
            if (e.KeyCode == Keys.Enter)
            {
                ShowVersion();
                if (cmbVer.Items.Count == 1)
                    cmbVer.SelectedIndex = 0;
            }
        }

        private void ShowVersion()
        {
            string sSQL = "Select VERSION "
                + "From SAJET.SYS_PART "
                + "Where PART_NO = '" + editPartNo.Text + "' and Enabled = 'Y' "
                + "Order By VERSION ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                cmbVer.Items.Add(dsTemp.Tables[0].Rows[0]["VERSION"].ToString());
            }
        }

        private void btnWarehouse_Click(object sender, EventArgs e)
        {
            String sSQL = "";
            sSQL = "  select WAREHOUSE_NAME "
                 + "    from SAJET.SYS_WAREHOUSE "
                 + "   WHERE ENABLED='Y' "
                 + "  order by WAREHOUSE_NAME ";
            fFilter f = new fFilter();
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                editWarehouse.Text = f.dgvData.CurrentRow.Cells["WAREHOUSE_NAME"].Value.ToString();
            }
        }

        private void editPartNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            string sTag = (sender as TextBox).TabIndex.ToString();
            try
            {
                int iTag = Convert.ToInt32(sTag);
                iTag += 1;
                for (int i = 0; i <= panel1.Controls.Count - 1; i++)
                {
                    object a = panel1.Controls[i];
                    if (a is TextBox)
                    {
                        if ((a as TextBox).TabIndex.ToString() == iTag.ToString())
                        {
                            (a as TextBox).Focus();
                            (a as TextBox).SelectAll();
                            break;
                        }
                    }
                }
            }
            catch
            {

            }


        }

    }
}