using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using SajetFilter;
using System.Data.OracleClient;

namespace CWOStatus
{
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();
        }

        String[] aryStatus = new String[9];
        //aryStatus = new String[9] { "Initial", "Prepare", "Release", "Work In Process", "Hold", "Cancel", "Complete", "Delete", "Resume" };
        static String g_sUserID;
        String g_sProgram, g_sFunction;
        String g_sUserFcID, g_sChangeStatus;
        String g_sFcID, g_sWhere;
        DataSet dsTemp;
        String sSQL;

        private void fWOStatus_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            g_sUserID = ClientUtils.UserPara1;
            g_sProgram = ClientUtils.fProgramName;
            g_sFunction = ClientUtils.fFunctionName;
            g_sChangeStatus = ClientUtils.fParameter;

            aryStatus[0] = SajetCommon.SetLanguage("Initial", 1);
            aryStatus[1] = SajetCommon.SetLanguage("Prepare", 1);
            aryStatus[2] = SajetCommon.SetLanguage("Release", 1);
            aryStatus[3] = SajetCommon.SetLanguage("Work In Process", 1);
            aryStatus[4] = SajetCommon.SetLanguage("Hold", 1);
            aryStatus[5] = SajetCommon.SetLanguage("Cancel", 1);
            aryStatus[6] = SajetCommon.SetLanguage("Complete", 1);
            aryStatus[7] = SajetCommon.SetLanguage("Delete", 1);
            aryStatus[8] = SajetCommon.SetLanguage("Resume", 1);

            LabStatus.Text = aryStatus[int.Parse(g_sChangeStatus)].ToString();
            LabStatus.BackColor = Color.Red;
            this.Text = this.Text + " (" + SajetCommon.g_sFileVersion + ")";

            string sSQL = " SELECT EMP_ID,NVL(FACTORY_ID,0) FACTORY_ID "
                        + " FROM SAJET.SYS_EMP "
                        + " WHERE EMP_ID ='" + g_sUserID + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            g_sUserFcID = dsTemp.Tables[0].Rows[0]["FACTORY_ID"].ToString();//Get User Factory
            g_sFcID = g_sUserFcID;


            dsTemp.Clear();
            sSQL = " Select FACTORY_ID,FACTORY_CODE,FACTORY_NAME,FACTORY_DESC "
                 + " From SAJET.SYS_FACTORY "
                 + " Where ENABLED = 'Y' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
            {
                cmbFactory.Items.Add(dsTemp.Tables[0].Rows[i]["FACTORY_CODE"].ToString());
                if (dsTemp.Tables[0].Rows[i]["FACTORY_ID"].ToString() == g_sUserFcID)
                {
                    cmbFactory.SelectedIndex = cmbFactory.Items.Count - 1;
                }
            }
            cmbFactory.Enabled = (g_sUserFcID == "0");
            if (g_sUserFcID == "0")
                cmbFactory.SelectedIndex = 0;

            editWO.Focus();
        }

        private void cmbFactory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();
            LabFactoryName.Text = "";
            if (cmbFactory.SelectedIndex == -1)
            {
                return;
            }

            g_sFcID = "";
            string strFactoryCode = cmbFactory.Text;
            sSQL = " Select FACTORY_ID,FACTORY_CODE,FACTORY_NAME,FACTORY_DESC "
                 + "From SAJET.SYS_FACTORY "
                 + "Where FACTORY_CODE = '" + strFactoryCode + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                g_sFcID = dsTemp.Tables[0].Rows[0]["FACTORY_ID"].ToString();
                LabFactoryName.Text = dsTemp.Tables[0].Rows[0]["FACTORY_NAME"].ToString();
            }

            //根據狀態顯示符合的工單 
            g_sWhere = "";
            switch (int.Parse(g_sChangeStatus))
            {
                case 2:
                    g_sWhere = " and A.WO_STATUS = '1' ";
                    break;
                case 3:
                    g_sWhere = " and A.WO_STATUS = '2' ";
                    break;
                case 4:
                    g_sWhere = " and A.WO_STATUS IN ('1','2','3') ";
                    break;
                case 5:
                    g_sWhere = " and A.WO_STATUS <= '4' ";
                    break;
                case 6:
                    g_sWhere = " and A.WO_STATUS IN ('3','4') ";
                    break;
                case 7:
                    g_sWhere = " and A.WO_STATUS >= '0' ";
                    break;
                case 8:
                    g_sWhere = " and A.WO_STATUS = '4' ";
                    break;
                default:
                    break;
            }
            g_sWhere = g_sWhere + " AND A.Factory_ID = '" + g_sFcID + "' ";

            editWO.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Boolean bWIP;
            string sQty = "";
            string sWOStatus;

            if (string.IsNullOrEmpty(editWO.Text))
            {
                string sData = LabWorkOrder.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                return;
            }

            if (LabWO.Text == "")
                return;

            if (!string.IsNullOrWhiteSpace(txtMemo.Text) && txtMemo.Lines.Length > 100)
            {
                SajetCommon.Show_Message("Memo length must less 100", 0);
                return;
            }

            string sWO = editWO.Text;
            sWOStatus = g_sChangeStatus;

            //WO Complete前檢查該工單是否有WIP的產品, 若有需要提示訊息並要求確認
            bWIP = false;
            if (sWOStatus == "6")
            {
                sSQL = " Select count(*) cnt from SAJET.G_SN_STATUS "
                     + " Where WORK_ORDER = '" + sWO + "'"
                     + " And OUT_PDLINE_TIME IS NULL "
                     + " And WIP_PROCESS <> 0 ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows[0][0].ToString() != "0")
                {
                    bWIP = true;
                    sQty = dsTemp.Tables[0].Rows[0][0].ToString();
                }
            }
            string sMsg2 = SajetCommon.SetLanguage("Change W/O Status to", 1) + " \"" + aryStatus[int.Parse(g_sChangeStatus)] + "\" ?";
            if (bWIP == true)
            {
                string sMsg1 = SajetCommon.SetLanguage("SN - Work In Process", 1);
                if (SajetCommon.Show_Message(sMsg1 + " : " + sQty + Environment.NewLine + sMsg2, 2) != DialogResult.Yes)
                {
                    return;
                }
            }
            else
            {
                if (SajetCommon.Show_Message(sMsg2, 2) != DialogResult.Yes)
                {
                    return;
                }
            }

            //更改狀態
            sSQL = "Update SAJET.G_WO_BASE "
                 + "Set WO_STATUS = :WO_STATUS ";

            if (sWOStatus == "6")
            {
                sSQL = sSQL + ",WO_CLOSE_DATE = SYSDATE ";
            }
            sSQL = sSQL + " ,UPDATE_USERID = '" + g_sUserID + "' "
                 + " ,UPDATE_TIME = SYSDATE "
                 + " Where WORK_ORDER = '" + sWO + "' ";
            if (sWOStatus == "2")
            {
                if (int.Parse(labInputQty.Text) > 0)
                {
                    sWOStatus = "3";
                }
            }
            else if (sWOStatus == "8")
            {
                sWOStatus = "2";
                if (int.Parse(labInputQty.Text) > 0)
                {
                    sWOStatus = "3";
                }
            }
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WO_STATUS", sWOStatus };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            dsTemp.Clear();

            // 紀錄狀態變更
            sWOStatus = g_sChangeStatus;
            if (g_sChangeStatus == "2")
            {
                if (int.Parse(labInputQty.Text) > 0)
                {
                    sWOStatus = "3";
                }
            }
            sSQL = "Insert into SAJET.G_WO_STATUS (Work_Order,WO_Status,Memo,update_userid) "
                 + "values ('" + sWO + "','" + sWOStatus + "','" + txtMemo.Text.ToString() + "','" + g_sUserID + "')";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            dsTemp.Clear();

            CopyToHistory(sWO);

            if (g_sChangeStatus == "7")
            {
                //====SAJET.SJ_DELETE_WO=====                 
                try
                {
                    Params = new object[2][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WO", sWO };
                    Params[1] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                    dsTemp = ClientUtils.ExecuteProc("SAJET.SJ_DELETE_WO", Params);

                    string sRes = dsTemp.Tables[0].Rows[0]["TRES"].ToString();
                    if (sRes != "OK")
                    {
                        SajetCommon.Show_Message(sRes, 0);
                        editWO.SelectAll();
                        return;
                    }
                    SajetCommon.Show_Message("Delete Work Order OK", -1);
                }
                catch (Exception ex)
                {
                    SajetCommon.Show_Message("SAJET.SJ_DELETE_WO" + Environment.NewLine + ex.Message, 0);
                    editWO.SelectAll();
                    return;
                }
            }
            else
            {
                SajetCommon.Show_Message("Change Work Order Status OK", -1);
            }

            ClearData();
            txtMemo.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CopyToHistory(string strRecordID)
        {
            sSQL = " Insert Into SAJET.G_HT_WO_BASE "
                 + " Select * from SAJET.G_WO_BASE "
                 + " Where WORK_ORDER = '" + strRecordID + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
        }

        public void ShowData()
        {
            string sSQL = "SELECT A.* "
                 + "  ,SAJET.SJ_WOSTATUS_RESULT(A.WO_STATUS) WOSTATUS "
                 + "   ,B.PART_NO, C.ROUTE_NAME, D.PDLINE_NAME "
                 + "   ,E.CUSTOMER_CODE, E.CUSTOMER_NAME "
                 + "   ,F.PROCESS_NAME AS START_PROCESS "
                 + "   ,G.PROCESS_NAME AS END_PROCESS "
                 + " FROM  SAJET.G_WO_BASE A  "
                 + " LEFT OUTER JOIN SAJET.SYS_PART B ON A.PART_ID = B.PART_ID "
                 + " LEFT OUTER JOIN SAJET.SYS_ROUTE C ON A.ROUTE_ID = C.ROUTE_ID "
                 + " LEFT OUTER JOIN SAJET.SYS_PDLINE D ON A.DEFAULT_PDLINE_ID = D.PDLINE_ID "
                 + " LEFT OUTER JOIN SAJET.SYS_CUSTOMER E ON A.CUSTOMER_ID = E.CUSTOMER_ID "
                 + " LEFT OUTER JOIN SAJET.SYS_PROCESS F ON A.START_PROCESS_ID = F.PROCESS_ID "
                 + " LEFT OUTER JOIN SAJET.SYS_PROCESS G ON A.END_PROCESS_ID = G.PROCESS_ID "
                 + " Where A.WORK_ORDER = '" + editWO.Text + "' "
                 + g_sWhere;
            DataSet DsData = ClientUtils.ExecuteSQL(sSQL);
            if (DsData.Tables[0].Rows.Count == 0)
            {
                string sMsg = SajetCommon.SetLanguage("W/O Status can not change to", 1);
                SajetCommon.Show_Message(sMsg + " \"" + aryStatus[int.Parse(g_sChangeStatus)] + "\"", 0);
                return;
            }
            LabWO.Text = DsData.Tables[0].Rows[0]["WORK_ORDER"].ToString();
            LabPart.Text = DsData.Tables[0].Rows[0]["PART_NO"].ToString();
            labTargetQty.Text = DsData.Tables[0].Rows[0]["TARGET_QTY"].ToString();
            labInputQty.Text = DsData.Tables[0].Rows[0]["INPUT_QTY"].ToString();
            labScheduleDate.Text = DsData.Tables[0].Rows[0]["WO_SCHEDULE_DATE"].ToString();
            labLine.Text = DsData.Tables[0].Rows[0]["PDLINE_NAME"].ToString();
            labRouteName.Text = DsData.Tables[0].Rows[0]["ROUTE_NAME"].ToString();
            labStartProcess.Text = DsData.Tables[0].Rows[0]["START_PROCESS"].ToString();
            labCustomer.Text = DsData.Tables[0].Rows[0]["CUSTOMER_CODE"].ToString();
            labSalesOrder.Text = DsData.Tables[0].Rows[0]["SALES_ORDER"].ToString();
            labRemark.Text = DsData.Tables[0].Rows[0]["REMARK"].ToString();
            labWOStatus.Text = DsData.Tables[0].Rows[0]["WOSTATUS"].ToString();
            labVersion.Text = DsData.Tables[0].Rows[0]["VERSION"].ToString();
            labScrapQty.Text = DsData.Tables[0].Rows[0]["SCRAP_QTY"].ToString();
            labOutputQty.Text = DsData.Tables[0].Rows[0]["OUTPUT_QTY"].ToString();
            labDueDate.Text = DsData.Tables[0].Rows[0]["WO_DUE_DATE"].ToString();
            labEndProcess.Text = DsData.Tables[0].Rows[0]["END_PROCESS"].ToString();
            labPONumber.Text = DsData.Tables[0].Rows[0]["PO_NO"].ToString();
            labMasterWO.Text = DsData.Tables[0].Rows[0]["MASTER_WO"].ToString();

            if (LabWO.Text != "")
                txtMemo.Focus();
        }

        private void editWO_KeyPress(object sender, KeyPressEventArgs e)
        {
            ClearData();
            if (e.KeyChar != (char)Keys.Return)
            {
                return;
            }

            sSQL = "SELECT A.* "
                 + " FROM  SAJET.G_WO_BASE A  "
                 + " Where A.WORK_ORDER = '" + editWO.Text + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                SajetCommon.Show_Message("Work Order Error", 0);
                editWO.Focus();
                editWO.SelectAll();
                return;
            }
            ShowData();
        }

        private void btnSearchWo_Click(object sender, EventArgs e)
        {
            string sSQL = " SELECT A.WORK_ORDER,B.PART_NO "
                        + "       ,SAJET.SJ_WOSTATUS_RESULT(WO_STATUS) STATUS "
                        + " FROM SAJET.G_WO_BASE A "
                        + "     ,SAJET.SYS_PART B "
                        + " WHERE A.PART_ID = B.PART_ID(+) "
                        + g_sWhere
                        + " ORDER BY A.WORK_ORDER ";
            fFilter f = new fFilter();
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                editWO.Text = f.dgvData.CurrentRow.Cells["WORK_ORDER"].Value.ToString();
                KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);
                editWO_KeyPress(sender, Key);
            }
        }

        public void ClearData()
        {
            for (int i = 0; i <= panel1.Controls.Count - 1; i++)
            {
                if (panel1.Controls[i] is Label)
                {
                    if (panel1.Controls[i].ForeColor == Color.FromArgb(192, 0, 0))
                        panel1.Controls[i].Text = "";
                }
            }
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void labRemark_Click(object sender, EventArgs e)
        {

        }
    }
}

