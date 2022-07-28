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
using SajetFilter;

namespace RCCreate
{
    public partial class fData : Form
    {
        private fMain fMainControl;
        public fData()
        {
            InitializeComponent();
        }
        public fData(fMain f)
        {
            InitializeComponent();
            fMainControl = f;
        }

        decimal d_inputQty = 0;

        public string g_sUpdateType, g_sformText;
        public DataGridViewRow dataCurrentRow;
        string sSQL;
        DataSet dsTemp, dsWoData, dsParamValue;
        bool bAppendSucess = false;
        //int iTargetQty = 0, iInputQty = 0, iAvailableQty = 0;
        decimal iTargetQty = 0, iInputQty = 0, iAvailableQty = 0;
        List<string> listSerialNo = new List<string>();

        private void fData_Load(object sender, EventArgs e)
        {
            combPriority.Items.Add("Normal");
            combPriority.Items.Add("Prior");
            combPriority.Items.Add("Urgent");
            combPriority.SelectedIndex = 0;
            DisablegvComponent();
            SajetCommon.SetLanguageControl(this);
            this.Text = g_sformText;

            // 2016.4.19 By Jason (隱藏部分欄位)
            string sSQL = "SELECT PARAM_VALUE FROM SAJET.SYS_BASE WHERE PROGRAM = 'RC Manager' AND PARAM_NAME = 'Lot Control Checked'";
            dsParamValue = ClientUtils.ExecuteSQL(sSQL);
            string sLotControlChecked = dsParamValue.Tables[0].Rows[0]["PARAM_VALUE"].ToString();

            if (sLotControlChecked == "N")
            {
                editRCNo.Enabled = false;
                btnSearchRC.Visible = false;

                btnSelectQty.Visible = true;
                btnSelectAll.Visible = true;
                btnSelectNone.Visible = true;
                tabControl1.Visible = true;
            }
            else
            {
                editRCNo.Enabled = false;
                btnSearchRC.Visible = true;

                btnSelectQty.Visible = false;
                btnSelectAll.Visible = false;
                btnSelectNone.Visible = false;
                tabControl1.Visible = false;
            }
        }

        private void btnSearchWO_Click(object sender, EventArgs e)
        {
            string sSQL = " select A.work_order,B.part_no, (A.TARGET_QTY - A.INPUT_QTY) \"Available Qty\", B.spec1,B.spec2  "
                        + " from sajet.g_wo_base A, sajet.sys_part B "
                        + " where A.part_id = B.part_id "
                        + " and A.wo_status > 1 and A.wo_status < 4 and A.TARGET_QTY - A.INPUT_QTY > 0 and a.wo_type <> '轉投工單' "
                        + " order by A.work_order ";
            fFilter f = new fFilter();
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                editWO.Text = f.dgvData.CurrentRow.Cells["WORK_ORDER"].Value.ToString();
                KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);
                editWO_KeyPress(sender, Key);
            }
        }

        // 2016.4.19 By Jason (帶出工單隸屬的流程卡)
        private void btnSearchRC_Click(object sender, EventArgs e)
        {
            if (editPart.Text != "")
            {
                string sSQL = "SELECT WORK_ORDER,RC_NO FROM SAJET.G_RC_STATUS WHERE WORK_ORDER = '" + editWO.Text + "' AND ROUTE_ID = 0 ORDER BY RC_NO ASC";
                fFilter f = new fFilter();
                f.sSQL = sSQL;
                if (f.ShowDialog() == DialogResult.OK)
                {
                    editRCNo.Text = f.dgvData.CurrentRow.Cells["RC_NO"].Value.ToString();
                }
            }
        }

        private void editWO_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 3)
                ClearData();
            if (e.KeyChar != (char)Keys.Return)
            {
                return;
            }

            sSQL = "SELECT *"
                 + "  FROM SAJET.G_WO_BASE"
                 + " WHERE WORK_ORDER = '" + editWO.Text + "'";
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

        private void ShowData()
        {
            string sSQL = "SELECT A.*,"
                 + "              B.PART_ID, B.PART_NO, C.ROUTE_ID, C.ROUTE_NAME, D.PDLINE_ID, D.PDLINE_NAME,"
                 + "              E.CUSTOMER_CODE, E.CUSTOMER_NAME,"
                 + "              F.SERIAL_NUMBER, F.RC_NO, B.SPEC1, A.WO_TYPE "
                 + "         FROM SAJET.G_WO_BASE A"
                 + "    LEFT JOIN SAJET.SYS_PART B ON A.PART_ID = B.PART_ID"
                 + "    LEFT JOIN SAJET.SYS_RC_ROUTE C ON A.ROUTE_ID = C.ROUTE_ID"
                 + "    LEFT JOIN SAJET.SYS_PDLINE D ON A.DEFAULT_PDLINE_ID = D.PDLINE_ID"
                 + "    LEFT JOIN SAJET.SYS_CUSTOMER E ON A.CUSTOMER_ID = E.CUSTOMER_ID"
                 + "    LEFT JOIN SAJET.G_SN_STATUS F ON A.WORK_ORDER = F.WORK_ORDER AND F.RC_NO = 'N/A'"
                 + "        WHERE A.WORK_ORDER = '" + editWO.Text + "'"
                 // 2016.4.19 By Jason (防呆)
                 + "          AND A.WO_STATUS > 1 AND A.WO_STATUS < 4"
                 + "          AND A.TARGET_QTY - A.INPUT_QTY > 0";
                 // 2016.4.19 End
            dsWoData = ClientUtils.ExecuteSQL(sSQL);

            // 2016.4.19 By Jason (防呆)
            if (dsWoData.Tables[0].Rows.Count == 0)
            {
                SajetCommon.Show_Message("Work Order Status (Available Qty) Close", 0);
                editWO.Focus();
                editWO.SelectAll();
                return;
            }
            // 2016.4.19 End

            // 2016.5.26 By Jason
            if (dsWoData.Tables[0].Rows[0]["WO_TYPE"].ToString() == "轉投工單")
            {
                SajetCommon.Show_Message("Work Order Type Can't Select", 0);
                editWO.Focus();
                editWO.SelectAll();
                return;
            }
            // 2016.5.26 End

            editWO.Text = dsWoData.Tables[0].Rows[0]["WORK_ORDER"].ToString();
            //editPart.Text = dsWoData.Tables[0].Rows[0]["PART_NO"].ToString();
            editPart.Text = dsWoData.Tables[0].Rows[0]["SPEC1"].ToString();
            editVersion.Text = dsWoData.Tables[0].Rows[0]["VERSION"].ToString();
            if (string.IsNullOrEmpty(dsWoData.Tables[0].Rows[0]["WO_DUE_DATE"].ToString()))
                editDue.Text = "";
            else
                editDue.Text = ((DateTime)dsWoData.Tables[0].Rows[0]["WO_DUE_DATE"]).ToString("yyyy/MM/dd");
            editType.Text = dsWoData.Tables[0].Rows[0]["WO_TYPE"].ToString();
            editPDLine.Text = dsWoData.Tables[0].Rows[0]["PDLINE_NAME"].ToString();
            editRoute.Text = dsWoData.Tables[0].Rows[0]["ROUTE_NAME"].ToString();
            editCustomer.Text = dsWoData.Tables[0].Rows[0]["CUSTOMER_NAME"].ToString();

            //if (Int32.TryParse(dsWoData.Tables[0].Rows[0]["TARGET_QTY"].ToString(), out iTargetQty) &&
            //    Int32.TryParse(dsWoData.Tables[0].Rows[0]["INPUT_QTY"].ToString(), out iInputQty))
            if (decimal.TryParse(dsWoData.Tables[0].Rows[0]["TARGET_QTY"].ToString(), out iTargetQty) &&
                decimal.TryParse(dsWoData.Tables[0].Rows[0]["INPUT_QTY"].ToString(), out iInputQty))
            {
                if (iTargetQty >= iInputQty)
                    iAvailableQty = iTargetQty - iInputQty;
            }

            editAvailableQty.Text = iAvailableQty.ToString();

            if (dsWoData.Tables[0].Rows.Count == 1 && dsWoData.Tables[0].Rows[0]["RC_NO"].ToString() != "N/A")
            {
                DisablegvComponent();
            }
            else
            {
                btnSelectQty.Enabled = true;
                panel3.Enabled = true;
                gvComponent.Visible = true;
                tabControl1.TabPages[0].BackColor = Color.White;
                gvComponent.DataSource = dsWoData;
                gvComponent.DataMember = dsWoData.Tables[0].ToString();

                for (int i = 0; i <= gvComponent.Columns.Count - 1; i++)
                {
                    if (gvComponent.Columns[i].Name == "SERIAL_NUMBER")
                    {
                        gvComponent.Columns[i].HeaderText = SajetCommon.SetLanguage("Serial Number", 1);
                        gvComponent.Columns[i].Width = 200;
                        gvComponent.Columns[i].ReadOnly = true;
                        continue;
                    }
                    gvComponent.Columns[i].Visible = false;
                }
                DataGridViewCheckBoxColumn cbCol = new DataGridViewCheckBoxColumn();
                cbCol.Width = 45;
                cbCol.Name = "SELECT";
                cbCol.HeaderText = "";
                cbCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                gvComponent.Columns.Insert(0, cbCol);
            }
            if (editWO.Text != "")
                editRCNo.Focus();
        }

        private void ClearData()
        {
            for (int i = 0; i <= tableLayoutPanel1.Controls.Count - 1; i++)
            {
                if (tableLayoutPanel1.Controls[i] is TextBox && tableLayoutPanel1.Controls[i].Name != "editWO")
                {
                    tableLayoutPanel1.Controls[i].Text = "";
                }
            }
            editRCNo.Text = "";
            editInputQty.Text = "";
            combPriority.SelectedIndex = 0;
            gvComponent.DataSource = null;
            gvComponent.Columns.Clear();
            DisablegvComponent();
        }

        private void DisablegvComponent()
        {
            btnSelectQty.Enabled = false;
            panel3.Enabled = false;
            gvComponent.Visible = false;
            tabControl1.TabPages[0].BackColor = Color.FromKnownColor(KnownColor.Control);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= tableLayoutPanel2.Controls.Count - 1; i++)
            {
                if (tableLayoutPanel2.Controls[i] is TextBox)
                {
                    tableLayoutPanel2.Controls[i].Text = tableLayoutPanel2.Controls[i].Text.Trim();
                }
            }
            if (!Check_Keyin())
            {
                return;
            }

            // 2016.4.19 By Jason (取消流程卡檢查:工單展流程卡時,已經預先寫入sajet.g_rc_status)

            ////檢查Name是否重複
            //sSQL = " Select RC_NO from SAJET.G_RC_STATUS "
            //     + " Where RC_NO = '" + editRCNo.Text + "' ";
            //dsTemp = ClientUtils.ExecuteSQL(sSQL);
            //if (dsTemp.Tables[0].Rows.Count > 0)
            //{
            //    string sData = LabRCNo.Text + " : " + editRCNo.Text;
            //    string sMsg = SajetCommon.SetLanguage("Data Duplicate", 2) + Environment.NewLine + sData;
            //    SajetCommon.Show_Message(sMsg, 0);
            //    editRCNo.Focus();
            //    editRCNo.SelectAll();
            //    return;
            //}
            ////Check routeID exist in sajet.sys_rc_route_detail

            // 2016.4.19 End

            sSQL = " Select * from sajet.sys_rc_route_detail "
                 + " Where route_id = '" + dsWoData.Tables[0].Rows[0]["ROUTE_ID"].ToString() + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count <= 0)
            {
                string sData = LabRoute.Text + " : " + editRoute.Text;
                string sMsg = SajetCommon.SetLanguage("Route error", 1) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editRCNo.Focus();
                editRCNo.SelectAll();
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
                    if (fMainControl != null) fMainControl.ShowData();
                    if (SajetCommon.Show_Message(sMsg, 2) == DialogResult.Yes)
                    {
                        ClearData();
                        editWO.Text = "";
                        editWO.Focus();
                        return;
                    }
                    DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception : " + ex.Message, 0);
                return;
            }
        }

        private bool Check_Keyin()
        {
            string sData, sMsg;
            if (string.IsNullOrEmpty(editWO.Text.Trim()))
            {
                sData = LabWO.Text;
                sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editWO.Focus();
                editWO.SelectAll();
                return false;
            }

            if (string.IsNullOrEmpty(editRCNo.Text.Trim()))
            {
                sData = LabRCNo.Text;
                sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editRCNo.Focus();
                editRCNo.SelectAll();
                return false;
            }

            uint inputQty = 0;
            decimal iAvailableQty = 0;

            // 2016.4.26 By Jason

            //Int32.TryParse(editAvailableQty.Text, out iAvailableQty);
            decimal.TryParse(editAvailableQty.Text, out iAvailableQty);

            //if (!UInt32.TryParse(editInputQty.Text, out inputQty) || inputQty == 0)
            //{
            //        SajetCommon.Show_Message("Input Qty error", 0);
            //        editInputQty.Focus();
            //        editInputQty.SelectAll();
            //        return false;
            //}

            d_inputQty = 0;

            if (!UInt32.TryParse(editInputQty.Text, out inputQty) || inputQty == 0)
            {
                if (!decimal.TryParse(editInputQty.Text, out d_inputQty) || d_inputQty <= 0)
                {
                    SajetCommon.Show_Message("Input Qty error", 0);
                    editInputQty.Focus();
                    editInputQty.SelectAll();
                    return false;
                }
                else
                {
                    string s_inputQty = d_inputQty.ToString().Trim();
                    if (s_inputQty.Length > s_inputQty.IndexOf(".") + 3)
                    {
                        SajetCommon.Show_Message("Input Qty error", 0);
                        editInputQty.Focus();
                        editInputQty.SelectAll();
                        return false;
                    }
                }
            }
            // 2016.4.26 End

            // 2016.4.26 By Jason
            if (iAvailableQty - inputQty < 0 || iAvailableQty - d_inputQty < 0)
            //if (iAvailableQty - inputQty < 0)
            // 2016.4.26 End
            {
                SajetCommon.Show_Message("Input Qty must less than or equal to Available Qty", 0);
                editInputQty.Focus();
                editInputQty.SelectAll();
                return false;
            }

            if (gvComponent.Visible)
            {
                int componentNum = 0;
                listSerialNo.Clear();
                foreach (DataGridViewRow row in gvComponent.Rows)
                {
                    if (row.Cells[0].Value != null)
                    {
                        if ((bool)row.Cells[0].Value != false)
                        {
                            listSerialNo.Add(row.Cells["SERIAL_NUMBER"].Value.ToString());
                            componentNum += 1;
                        }
                    }
                }

                // 2016.4.26 By Jason
                if (componentNum != inputQty || componentNum != d_inputQty)
                //if (componentNum != inputQty)
                // 2016.4.26 End
                {
                    SajetCommon.Show_Message("Input Qty must equal to the number of component you pick", 0);
                    editInputQty.Focus();
                    editInputQty.SelectAll();
                    return false;
                }
            }

            return true;
        }

        private void AppendData()
        {
            string sHaveSN = "0";
            if (gvComponent.Visible)
            {
                sHaveSN = "1";
                object[][] Params = new object[4][];
                sSQL = " Update SAJET.G_SN_STATUS "
                     + " set RC_NO = :RC_NO "
                     + " ,UPDATE_USERID = :UPDATE_USERID "
                     + " ,UPDATE_TIME = SYSDATE "
                     + " where WORK_ORDER = :WORK_ORDER "
                     + " and SERIAL_NUMBER = :SERIAL_NUMBER ";
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", editRCNo.Text };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMainControl.g_sUserID };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", editWO.Text };
                for (int i = 0; i < listSerialNo.Count; i++)
                {
                    Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SERIAL_NUMBER", listSerialNo[i] };
                    ClientUtils.ExecuteSQL(sSQL, Params);
                }
            }
            string sPartID = dsWoData.Tables[0].Rows[0]["PART_ID"].ToString();
            string sRouteID = dsWoData.Tables[0].Rows[0]["ROUTE_ID"].ToString();
            string sPDLineID = dsWoData.Tables[0].Rows[0]["PDLINE_ID"].ToString();
            string sFactoryID = dsWoData.Tables[0].Rows[0]["FACTORY_ID"].ToString();

            object[][] procParams = new object[7][];
            procParams[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "prouteid", sRouteID };
            procParams[1] = new object[] { ParameterDirection.Output, OracleType.VarChar, "vprocessid", "" };
            procParams[2] = new object[] { ParameterDirection.Output, OracleType.VarChar, "vnextprocess", "" };
            procParams[3] = new object[] { ParameterDirection.Output, OracleType.VarChar, "vsheetname", "" };
            procParams[4] = new object[] { ParameterDirection.Output, OracleType.VarChar, "vstageid", "" };
            procParams[5] = new object[] { ParameterDirection.Output, OracleType.VarChar, "vnodeid", "" };
            procParams[6] = new object[] { ParameterDirection.Output, OracleType.VarChar, "vnextnode", "" };
            DataSet dsProcess = ClientUtils.ExecuteProc("SAJET.SJ_GET_FIRSTPROCESS", procParams);

            string sStageID = dsProcess.Tables[0].Rows[0]["vstageid"].ToString();
            string sNodeID = dsProcess.Tables[0].Rows[0]["vnodeid"].ToString();
            string sProcessID = dsProcess.Tables[0].Rows[0]["vprocessid"].ToString();
            string sSheetName = dsProcess.Tables[0].Rows[0]["vsheetname"].ToString();
            string sNextNode = dsProcess.Tables[0].Rows[0]["vnextnode"].ToString();
            string sNextProcess = "";
            if (dsProcess.Tables[0].Rows[0]["vnextprocess"].ToString() != "0")
                sNextProcess = dsProcess.Tables[0].Rows[0]["vnextprocess"].ToString();

            // 2016.4.19 By Jason (取消流程卡檢查:工單展流程卡時,已經預先寫入sajet.g_rc_status)

            //object[][] Params2 = new object[17][];
            //sSQL = " Insert into SAJET.G_RC_STATUS "
            //     + " (WORK_ORDER, RC_NO, PART_ID, VERSION, ROUTE_ID, FACTORY_ID "
            //     + ", PDLINE_ID, STAGE_ID, NODE_ID, PROCESS_ID, SHEET_NAME "
            //     + ", NEXT_NODE, NEXT_PROCESS "
            //     + ", CURRENT_QTY, HAVE_SN, PRIORITY_LEVEL "
            //     + ", UPDATE_USERID, UPDATE_TIME) "
            //     + " Values "
            //     + " (:WORK_ORDER, :RC_NO, :PART_ID, :VERSION, :ROUTE_ID, :FACTORY_ID "
            //     + ", :PDLINE_ID, :STAGE_ID, :NODE_ID, :PROCESS_ID, :SHEET_NAME "
            //     + ", :NEXT_NODE, :NEXT_PROCESS "
            //     + ", :CURRENT_QTY, :HAVE_SN, :PRIORITY_LEVEL "
            //     + ", :UPDATE_USERID, SYSDATE) ";
            //Params2[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", editWO.Text };
            //Params2[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", editRCNo.Text };
            //Params2[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sPartID };
            //Params2[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VERSION", editVersion.Text };
            //Params2[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ROUTE_ID", sRouteID };
            //Params2[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FACTORY_ID", sFactoryID };
            //Params2[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PDLINE_ID", sPDLineID };
            //Params2[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "STAGE_ID", sStageID };
            //Params2[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NODE_ID", sNodeID };
            //Params2[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", sProcessID };
            //Params2[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_NAME", sSheetName };
            //Params2[11] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NEXT_NODE", sNextNode };
            //Params2[12] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NEXT_PROCESS", sNextProcess };
            //Params2[13] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CURRENT_QTY", editInputQty.Text };
            //Params2[14] = new object[] { ParameterDirection.Input, OracleType.VarChar, "HAVE_SN", sHaveSN };
            //Params2[15] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PRIORITY_LEVEL", combPriority.SelectedIndex.ToString() };
            //Params2[16] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMainControl.g_sUserID };
            //ClientUtils.ExecuteSQL(sSQL, Params2);

            // 2016.5.25 By Jason
            DateTime datExeTime = DateTime.Now;
            long lTravel_Id = Convert.ToInt64(datExeTime.ToString("yyyyMMddHHmmssf"));
            // 2016.5.25 End

            object[][] Params2 = new object[19][];
            sSQL = "UPDATE SAJET.G_RC_STATUS"
                 + "   SET PART_ID = :PART_ID,"
                 + "       VERSION = :VERSION,"
                 + "       ROUTE_ID = :ROUTE_ID,"
                 + "       FACTORY_ID = :FACTORY_ID,"
                 + "       PDLINE_ID = :PDLINE_ID,"
                 + "       STAGE_ID = :STAGE_ID,"
                 + "       NODE_ID = :NODE_ID,"
                 + "       PROCESS_ID = :PROCESS_ID,"
                 + "       SHEET_NAME = :SHEET_NAME,"
                 + "       TRAVEL_ID = :TRAVEL_ID,"
                 + "       NEXT_NODE = :NEXT_NODE,"
                 + "       NEXT_PROCESS = :NEXT_PROCESS,"
                 + "       CURRENT_QTY = :CURRENT_QTY,"
                 + "       HAVE_SN = :HAVE_SN,"
                 + "       PRIORITY_LEVEL = :PRIORITY_LEVEL,"
                 + "       UPDATE_USERID = :UPDATE_USERID,"
                 + "       UPDATE_TIME = :UPDATE_TIME"
                 + " WHERE WORK_ORDER = :WORK_ORDER"
                 + "   AND RC_NO = :RC_NO";
            Params2[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", editWO.Text };
            Params2[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", editRCNo.Text };
            Params2[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sPartID };
            Params2[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VERSION", editVersion.Text };
            Params2[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ROUTE_ID", sRouteID };
            Params2[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FACTORY_ID", sFactoryID };
            Params2[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PDLINE_ID", sPDLineID };
            Params2[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "STAGE_ID", sStageID };
            Params2[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NODE_ID", sNodeID };
            Params2[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", sProcessID };
            Params2[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_NAME", sSheetName };
            // 2016.5.25 By Jason
            Params2[11] = new object[] { ParameterDirection.Input, OracleType.Number, "TRAVEL_ID", lTravel_Id };
            // 2016.5.25 End
            Params2[12] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NEXT_NODE", sNextNode };
            Params2[13] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NEXT_PROCESS", sNextProcess };
            Params2[14] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CURRENT_QTY", editInputQty.Text };
            Params2[15] = new object[] { ParameterDirection.Input, OracleType.VarChar, "HAVE_SN", sHaveSN };
            Params2[16] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PRIORITY_LEVEL", combPriority.SelectedIndex.ToString() };
            Params2[17] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMainControl.g_sUserID };
            // 2016.5.25 By Jason
            Params2[18] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime };
            // 2016.5.25 End
            ClientUtils.ExecuteSQL(sSQL, Params2);

            //sSQL = " INSERT INTO SAJET.G_RC_TRAVEL "
            //     + " SELECT * FROM SAJET.G_RC_STATUS "
            //     + "         WHERE RC_NO = '" + editRCNo.Text + "' ";
            //ClientUtils.ExecuteSQL(sSQL);

            // 2016.4.19 End

            // 2016.4.26 By Jason
            //string sInputQty = (Int32.Parse(editInputQty.Text) + iInputQty).ToString();
            string sInputQty = "";
            //if (editInputQty.Text.Trim().IndexOf(".") == -1)
            //{
            //    sInputQty = (Int32.Parse(editInputQty.Text) + iInputQty).ToString();
            //}
            //else
            //{
            sInputQty = (decimal.Parse(editInputQty.Text) + iInputQty).ToString();
            //}
            // 2016.4.26 End

            object[][] woParams = new object[3][];
            sSQL = "UPDATE SAJET.G_WO_BASE"
                 + "   SET INPUT_QTY = :INPUT_QTY,"
                 + "       UPDATE_USERID = :UPDATE_USERID,"
                 + "       UPDATE_TIME = SYSDATE,"
                 + "       WO_STATUS = 3"
                 + " WHERE WORK_ORDER = :WORK_ORDER";
            woParams[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "INPUT_QTY", sInputQty };
            woParams[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMainControl.g_sUserID };
            woParams[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", editWO.Text };
            ClientUtils.ExecuteSQL(sSQL, woParams);
            CopyToWOHistory(editWO.Text);
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            int iQty = 0;

            for (int i = 0; i <= gvComponent.Rows.Count - 1; i++)
            {
                gvComponent.Rows[i].Cells[0].Value = true;
                iQty++;
            }

            editInputQty.Text = iQty.ToString();
        }

        private void btnSelectQty_Click(object sender, EventArgs e)
        {
            int inputQty = 0;
            if (Int32.TryParse(editInputQty.Text, out inputQty) && inputQty > 0)
            {
                if (inputQty >= gvComponent.Rows.Count)
                {
                    inputQty = gvComponent.Rows.Count;
                }
                for (int i = 0; i <= inputQty - 1; i++)
                {
                    gvComponent.Rows[i].Cells[0].Value = true;
                }
                if (gvComponent.Rows.Count > inputQty)
                {
                    for (int j = inputQty; j <= gvComponent.Rows.Count - 1; j++)
                    {
                        gvComponent.Rows[j].Cells[0].Value = false;
                    }
                }
            }
        }

        private void btnSelectNone_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= gvComponent.Rows.Count - 1; i++)
            {
                gvComponent.Rows[i].Cells[0].Value = false;
            }
            editInputQty.Text = "0";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (bAppendSucess)
                DialogResult = DialogResult.OK;
        }

        public static void CopyToWOHistory(string sID)
        {
            string sSQL = " INSERT INTO SAJET.G_HT_WO_BASE "
                        + " SELECT * FROM SAJET.G_WO_BASE "
                        + "         WHERE WORK_ORDER = '" + sID + "' ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
        }
    }
}
