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
    public partial class fLotMemo : Form
    {
        public string g_sType;
        public string g_sLotLevel;
        public string g_sExeName;
        string g_QCResult;

        public fLotMemo()
        {
            InitializeComponent();
        }

        public void initial(string sLotNo, string sResult, string sLotSize,int iOKItem,int iNGItem,string sInspDate,int iResult)
        {
            lablLotNo.Text = sLotNo;
            lablLotSize.Text = sLotSize;
            lablResult.Text = sResult;
            lablPassQty.Text = iOKItem.ToString();
            lablFailQty.Text = iNGItem.ToString();
            //editReceiveQty.Text = sLotSize;
            editReceiveQty.Text = "0";
            lablInspDate.Text = sInspDate;

            if (iResult == 1) //批退
            {
                lablAcceptQty.Text = "Receive Qty";
                //editReceiveQty.Enabled = false;  //凌華20120105有部分批退的物料，也能打批退數
            }

            if (iResult == 0 || iResult == 4 || iResult ==2  || iResult==8 ) //允收
            {
                editReceiveQty.Text = sLotSize;
                editReceiveQty.Enabled = false;
            }
            if (iResult == 1)
                lablAcceptQty.Text = "Receive Qty";
            g_QCResult = iResult.ToString();
            if (iResult==3)
                editInspWorkHours.BackColor = Color.Yellow;

            /*
            editReceiveQty.Enabled = false;
            if (sResult == "Sorting")
                editInspWorkHours.BackColor = Color.Yellow;
             
            else if (sResult == "Reject")
                lablAcceptQty.Text = "Reject Qty";
            if ((sResult == "Sorting") || (sResult == "Partial Waive"))
            {
                editReceiveQty.Enabled = true;
                editReceiveQty.BackColor = Color.Yellow;
                editReceiveQty.Text = Convert.ToString(float.Parse(lablLotSize.Text) - float.Parse(lablFailQty.Text));
            }
             */ 
        }

        private void btnSave_Click(object sender, EventArgs e) //儲存按鈕
        {
            editDateCode.Text = editDateCode.Text.Trim();
            editLot.Text = editLot.Text.Trim();
            editMaterialVer.Text = editMaterialVer.Text.Trim();
            editDMDA.Text = editDMDA.Text.Trim();
            editInspWorkHours.Text = editInspWorkHours.Text.Trim();
            editReceiveQty.Text = editReceiveQty.Text.Trim();
            editLotMemo.Text = editLotMemo.Text.Trim();

            if (string.IsNullOrEmpty(editInspWorkHours.Text))
            {
                if (g_QCResult == "3")
                {
                    SajetCommon.Show_Message("Please Input Insp Work Hours", 0);
                    editInspWorkHours.Focus();
                    editInspWorkHours.SelectAll();
                    return;
                }
                else
                    editInspWorkHours.Text = "0";
            }
            try
            {
                Convert.ToInt32(editInspWorkHours.Text);
            }
            catch
            {
                SajetCommon.Show_Message("Insp Work Hours Error", 0);
                editInspWorkHours.Focus();
                editInspWorkHours.SelectAll();
                return;
            }

            int iReceiveQty = 0;

            try
            {
                iReceiveQty = Convert.ToInt32(editReceiveQty.Text);
            }
            catch
            {
                SajetCommon.Show_Message(lablAcceptQty.Text+ " "+SajetCommon.SetLanguage("Error"), 0);
                editReceiveQty.Focus();
                editReceiveQty.SelectAll();
                return;
            }

            if (iReceiveQty > Convert.ToInt32(lablLotSize.Text)) //允收數量大於批量
            {
                SajetCommon.Show_Message(lablAcceptQty.Text + " > " + SajetCommon.SetLanguage("Lot Size"), 0);
                editReceiveQty.Focus();
                editReceiveQty.SelectAll();
                return;
            }

            if (rbtnNormal.Checked)
                g_sLotLevel = "0";
            else if (rbtnTight.Checked)
                g_sLotLevel = "1";
            else if (rbtnReduce.Checked)
                g_sLotLevel = "2";
            else if (rbtnByPass.Checked)
                g_sLotLevel = "3";
            else
            {
                SajetCommon.Show_Message("Please Select Lot Level", 0);
                return;
            }
            if (g_QCResult == "4" && g_sLotLevel != "3") //免驗
            {
                if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Lot No", 1) + " : " + lablLotNo.Text + "," + SajetCommon.SetLanguage("Result") + " : " + lablResult.Text + " " + Environment.NewLine + Environment.NewLine
                                            + SajetCommon.SetLanguage("Your choice is not By Pass,Continue ?"), 2) != DialogResult.Yes)
                    return;

            }

          
            if (editWaiveNo.Visible) 
            {
                if (string.IsNullOrEmpty(editWaiveNo.Text))
                {
                    SajetCommon.Show_Message("Please Input Waive No", 0);
                    editWaiveNo.Focus();
                    editWaiveNo.SelectAll();
                    return;
                }
            }
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void fLotMemo_Load(object sender, EventArgs e)
        {
            //SajetCommon.SetLanguageControl(this);
            //if (g_sType == "0")  //批號備註按鈕
            //{
            //    PanelLevel.Visible = false;
            //    editReceiveQty.Enabled = false;
            //}
            //string sSQL = "SELECT DATECODE,LOT,MATERIAL_VER,DMDA,INSP_WORKHOURS,LOT_MEMO,NVL(LOT_LEVEL,'0') LOT_LEVEL,RECEIVE_QTY,MRB_MEMO "
            //            + "  FROM SAJET.G_IQC_LOT "
            //            + " WHERE LOT_NO ='" + lablLotNo.Text + "' "
            //            + "   AND ROWNUM = 1 ";
            //DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            //if (dsTemp.Tables[0].Rows.Count > 0)
            //{
            //    DataRow dr = dsTemp.Tables[0].Rows[0];
            //    editDateCode.Text = dr["DATECODE"].ToString();
            //    editLot.Text = dr["LOT"].ToString();
            //    editMaterialVer.Text = dr["MATERIAL_VER"].ToString();
            //    editDMDA.Text = dr["DMDA"].ToString();
            //    editInspWorkHours.Text = dr["INSP_WORKHOURS"].ToString();
            //    editLotMemo.Text = dr["LOT_MEMO"].ToString();
            //    editMRB.Text = dr["MRB_MEMO"].ToString();
            //    if (dr["RECEIVE_QTY"].ToString() != "0")
            //        editReceiveQty.Text = dr["RECEIVE_QTY"].ToString();
            //    string sLotLevel = dr["LOT_LEVEL"].ToString();
            //    switch (g_sLotLevel)
            //    {
            //        case "0": rbtnNormal.Checked = true; break;
            //        case "1": rbtnTight.Checked = true; break;
            //        case "2": rbtnReduce.Checked = true; break;
            //        case "3": rbtnByPass.Checked = true; break;
            //    }

            SajetCommon.SetLanguageControl(this);

            if (g_sType == "0")  //批號備註按鈕
            {
                PanelLevel.Visible = false;
                editReceiveQty.Enabled = false;
            }
            lablWaiveNo.Visible = (g_QCResult == "2");
            editWaiveNo.Visible = (g_QCResult == "2");

            string sSQL = "SELECT DATECODE,LOT,MATERIAL_VER,DMDA,INSP_WORKHOURS,LOT_MEMO,NVL(LOT_LEVEL,'0') LOT_LEVEL,RECEIVE_QTY "
                        + "  FROM SAJET.G_IQC_LOT "
                        + " WHERE LOT_NO ='" + lablLotNo.Text + "' "
                        + "   AND ROWNUM = 1 ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);

            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsTemp.Tables[0].Rows[0];
                editDateCode.Text = dr["DATECODE"].ToString();
                editLot.Text = dr["LOT"].ToString();
                editMaterialVer.Text = dr["MATERIAL_VER"].ToString();
                editDMDA.Text = dr["DMDA"].ToString();
                editInspWorkHours.Text = dr["INSP_WORKHOURS"].ToString();
                editLotMemo.Text = dr["LOT_MEMO"].ToString();

                //凌華允收數預設代0
                //if (dr["RECEIVE_QTY"].ToString() != "0")
                //    editReceiveQty.Text = dr["RECEIVE_QTY"].ToString();
               // editReceiveQty.Text = "0";

                string sLotLevel = dr["LOT_LEVEL"].ToString();
                switch (g_sLotLevel)
                {
                    case "0": rbtnNormal.Checked = true; break;
                    case "1": rbtnTight.Checked = true; break;
                    case "2": rbtnReduce.Checked = true; break;
                    case "3": rbtnByPass.Checked = true; break;
                }
            }
        }

        private void editLotMemo_TextChanged(object sender, EventArgs e)
        {

        }

        private void editInspWHour_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == 8 || e.KeyChar == 13 || e.KeyChar == 46))
            {
                e.KeyChar = (char)Keys.None;
            }

            if (e.KeyChar != (char)Keys.Return)
                return;
        }

        private void editReceiveQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == 8 || e.KeyChar == 13 || e.KeyChar == 46))
            {
                e.KeyChar = (char)Keys.None;
            }

            if (e.KeyChar != (char)Keys.Return)
                return;
        }

        private void fLotMemo_Shown(object sender, EventArgs e)
        {
            editDateCode.Focus();
        }

        private void btnDefectImage_Click(object sender, EventArgs e)
        {
            fImage fData_Add = new fImage(); // 附加檔案
            try
            {
                fData_Add.g_sRecID = lablLotNo.Text;
                fData_Add.g_sflag = "Y";
                fData_Add.g_sExeName = g_sExeName;
                fData_Add.ShowDialog();
            }
            finally
            {
                fData_Add.Dispose();         // 附加檔案~
            }

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}