using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using SajetClass;

namespace IQCbyLot
{
    public partial class fWaive : Form
    {
        public fWaive()
        {
            InitializeComponent();
        }

        
        public string g_LotNo;
        public string g_Tag = "0";
        private bool _CheckChange = false;
        public int iReceiveQty;

        private void fWaive_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            labLot.Text = g_LotNo;
            getData();
            this.numericUpDown1.Height = this.dgRT.Height;
        }

        private void getData()
        {
            object[][] Params;
            DataSet ds;
            string sSQL = @"SELECT C.VENDOR_CODE,C.VENDOR_NAME,A.PART_ID,B.PART_NO,A.LOT_SIZE,
                            A.RECEIVE_QTY PASS_QTY,(A.LOT_SIZE - A.RECEIVE_QTY) FAIL_QTY,
                            A.DATECODE,A.MATERIAL_VER,E.RT_NO,E.RT_ID,D.INCOMING_QTY,D.RT_SEQ,D.RECEIVE_QTY,D.REJECT_QTY
                            FROM SAJET.G_IQC_LOT A,SAJET.SYS_PART B,SAJET.SYS_VENDOR C,SAJET.G_ERP_RT_ITEM D,SAJET.G_ERP_RTNO E
                            WHERE A.PART_ID = B.PART_ID AND A.VENDOR_ID = C.VENDOR_ID AND A.LOT_NO = D.LOT_NO AND D.RT_ID =E.RT_ID
                            AND A.LOT_NO = :LOT_NO";
            Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", g_LotNo };
            ds = ClientUtils.ExecuteSQL(sSQL, Params);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dgRT.Rows.Add();
                dgRT.Rows[dgRT.Rows.Count - 1].Cells[0].Value = dr["RT_NO"].ToString();
                dgRT.Rows[dgRT.Rows.Count - 1].Cells[1].Value = dr["INCOMING_QTY"].ToString();
                dgRT.Rows[dgRT.Rows.Count - 1].Cells[2].Value = dr["RECEIVE_QTY"].ToString();
                dgRT.Rows[dgRT.Rows.Count - 1].Cells[3].Value = dr["REJECT_QTY"].ToString();
                dgRT.Rows[dgRT.Rows.Count - 1].Cells[4].Value = dr["RT_ID"].ToString();
                dgRT.Rows[dgRT.Rows.Count - 1].Cells[5].Value = dr["RT_SEQ"].ToString();

                if (ds.Tables[0].Rows.Count == 1)
                {
                    dgRT.Rows[dgRT.Rows.Count - 1].Cells[2].Value = dr["PASS_QTY"].ToString();
                    dgRT.Rows[dgRT.Rows.Count - 1].Cells[3].Value = dr["FAIL_QTY"].ToString();

                    if (g_Tag != "1") //g_Tag == 1 ----> 輸入特採結果按鈕
                    {
                        Save();  //只有一張RT單直接儲存特採結果
                        DialogResult = DialogResult.OK;
                    }
                }
                else if (Convert.ToInt32(dr["LOT_SIZE"].ToString()) == Convert.ToInt32(dr["PASS_QTY"].ToString()) && g_Tag != "1") 
                {
                    for (int i = dgRT.Rows.Count - 1; i >= 0; i--)
                    {
                        dgRT.Rows[i].Cells[2].Value = dgRT.Rows[i].Cells[1].Value;
                        dgRT.Rows[i].Cells[3].Value = 0;
                    }
                    Save(); //特採允收數量=批量，直接儲存特採結果
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    dgRT.Rows[dgRT.Rows.Count - 1].Cells[2].ReadOnly = false;
                    dgRT.Rows[dgRT.Rows.Count - 1].Cells[3].ReadOnly = false;
                }
                labLotQTY.Text = dr["LOT_SIZE"].ToString();
                labPN.Text = dr["PART_NO"].ToString();
                labPartID.Text = dr["PART_ID"].ToString();
                labVendor.Text = dr["VENDOR_NAME"].ToString();
                labVendorCode.Text = dr["VENDOR_CODE"].ToString();
                labDC.Text = dr["DATECODE"].ToString();
                labVer.Text = dr["MATERIAL_VER"].ToString();
                labPass.Text = iReceiveQty.ToString();
                labFail.Text = (Convert.ToInt32(dr["LOT_SIZE"].ToString()) - iReceiveQty).ToString();
            }
        }

        private void dgRT_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //if (this.dgRT.Columns[e.ColumnIndex].ReadOnly == false)
            //{
            //    Rectangle r = this.dgRT.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            //    r = this.dgRT.RectangleToScreen(r);
            //    this.numericUpDown1.Location = this.RectangleToClient(r).Location;
            //    this.numericUpDown1.Size = r.Size;
            //    this._CheckChange = true;
            //    if (dgRT.CurrentCell.Value != null)
            //        this.numericUpDown1.Text = this.dgRT.CurrentCell.Value.ToString();
            //    else
            //        numericUpDown1.Value = 0;
            //    this._CheckChange = false;
            //    this.numericUpDown1.Visible = true;
            //}
            //else
            //{
            //    this.numericUpDown1.Visible = false;
            //}
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            //if (_CheckChange) return;
            //this.dgRT.CurrentCell.Value = this.numericUpDown1.Text;  
        }

        private void btSave_Click(object sender, EventArgs e) //儲存
        {
            int iPassQty;
            int iFailQty;
            int totalPass = 0;
            int totalFail = 0;
            int flag = 0;

            try
            {
                for (int i = dgRT.Rows.Count - 1; i >= 0; i--)
                {
                    iPassQty = Convert.ToInt32(dgRT.Rows[i].Cells[2].Value);
                    iFailQty = Convert.ToInt32(dgRT.Rows[i].Cells[3].Value);
                    totalPass += iPassQty;
                    totalFail += iFailQty;

                    if (dgRT.Rows[i].Cells[2].Value == null || dgRT.Rows[i].Cells[3].Value == null)
                    {
                        SajetCommon.Show_Message("Qty Error", 0);
                        flag = 1;
                        return;
                    }

                    if (dgRT.Rows[i].Cells[2].Value.ToString().Trim() == "" || dgRT.Rows[i].Cells[3].Value.ToString().Trim() == "")
                    {
                        SajetCommon.Show_Message("Qty Error", 0);
                        flag = 1;
                        return;
                    }

                    if (iPassQty + iFailQty != Convert.ToInt32(dgRT.Rows[i].Cells[1].Value))
                    {
                        SajetCommon.Show_Message("Qty Error", 0);
                        flag = 1;
                        return;
                    }
                }
                if (totalPass != Convert.ToInt32(labPass.Text) || totalFail != Convert.ToInt32(labFail.Text))
                {
                    SajetCommon.Show_Message("Qty Error", 0);
                    flag = 1;
                    return;
                }
            }
            catch
            {
                SajetCommon.Show_Message("Qty Error", 0);
                flag = 1;
                return;
            }

            if (flag == 0)
            {
                if (SajetCommon.Show_Message("Save?", 2) == DialogResult.Yes)
                {
                    Save();
                    SajetCommon.Show_Message("Save OK", 3);
                    DialogResult = DialogResult.OK;
                }
            }
        }

        private void Save()
        {
            string sRes = "";
               DataSet ds;

               for (int i = dgRT.Rows.Count - 1; i >= 0; i--)
               {

                   object[][] Params = new object[5][];
                   Params[0] = new object[] { "INPUT", "1", "TRTNO", dgRT.Rows[i].Cells[0].Value.ToString() };
                   Params[1] = new object[] { "INPUT", "1", "TRTSEQ", dgRT.Rows[i].Cells[5].Value.ToString() };
                   Params[2] = new object[] { "INPUT", "1", "TACCEPT", Convert.ToInt32(dgRT.Rows[i].Cells[2].Value) };
                   Params[3] = new object[] { "INPUT", "1", "TREJECT", Convert.ToInt32(dgRT.Rows[i].Cells[3].Value) };
                   Params[4] = new object[] { "OUTPUT", "1", "TRES", "" };
                   ds = ClientUtils.ExecuteProc("SAJET.SJ_IQC_SAVE_QTY", Params);
                   sRes = ds.Tables[0].Rows[0]["TRES"].ToString();
                   if (sRes != "OK")
                   {
                       SajetCommon.Show_Message(sRes, 0);
                       return;
                   }
               }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel; 
        }
    }
}
