using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using SajetTable;
using System.IO;
using System.Data.OracleClient;
using System.Reflection;
using System.Collections.Specialized;

namespace RCCreate
{
    public partial class fMain : Form
    {
        private MESGridView.Cache memoryCache;

        List<string> combFilterField = new List<string>();

        public String g_sUserID, g_sProgram, g_sFunction, g_sExeName;
        public String g_sOrderField;
        public static int g_iPrivilege, g_iReleasePri;
        string g_sDataSQL;

        public fMain()
        {
            InitializeComponent();
        }

        private void Initial_Form()
        {
            g_sUserID = ClientUtils.UserPara1;
            g_sProgram = ClientUtils.fProgramName;
            g_sFunction = ClientUtils.fFunctionName;
            g_sOrderField = TableDefine.gsDef_OrderField;

            SajetCommon.SetLanguageControl(this);
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            TableDefine.Initial_Table();
            Initial_Form();

            this.Text = this.Text + "(" + SajetCommon.g_sFileVersion + ")";

            //Filter
            combFilter.Items.Clear();
            combFilterField.Clear();

            for (int i = 0; i <= TableDefine.tGridField.Length - 1; i++)
            {
                combFilter.Items.Add(TableDefine.tGridField[i].sCaption);
                combFilterField.Add(TableDefine.tGridField[i].sFieldName);
            }

            if (combFilter.Items.Count > 0)
                combFilter.SelectedIndex = 0;

            Check_Privilege();

            editFilter.Text = SajetCommon.SetLanguage("Please enter condition", 1);
            editFilter.ForeColor = Color.Tomato;
            editFilter.GotFocus += new EventHandler(RemoveText);
        }

        private void RemoveText(object sender, EventArgs e)
        {
            if (editFilter.Text == SajetCommon.SetLanguage("Please enter condition", 1))
            {
                editFilter.Text = "";
                editFilter.ForeColor = Color.Black;
            }
        }

        private void Check_Privilege()
        {
            string sPrivilege = ClientUtils.GetPrivilege(g_sUserID, g_sFunction, g_sProgram).ToString();

            btnAppend.Enabled = SajetCommon.CheckEnabled("INSERT", sPrivilege);
            btnDelete.Enabled = SajetCommon.CheckEnabled("DELETE", sPrivilege);
        }

        public void ShowData()
        {
            string sSQL = "    SELECT A.WORK_ORDER,A.VERSION,A.RC_NO,A.CURRENT_QTY,A.PRIORITY_LEVEL,"
                        + "           B.WO_TYPE,B.WO_DUE_DATE,C.PART_NO,D.ROUTE_NAME,E.PDLINE_NAME,F.CUSTOMER_NAME,C.SPEC1,C.SPEC2 "
                        + "      FROM SAJET.G_RC_STATUS A "
                        + " LEFT JOIN SAJET.G_WO_BASE B ON A.WORK_ORDER = B.WORK_ORDER "
                        + " LEFT JOIN SAJET.SYS_PART C ON A.PART_ID = C.PART_ID "
                        + " LEFT JOIN SAJET.SYS_RC_ROUTE D ON A.ROUTE_ID = D.ROUTE_ID "
                        + " LEFT JOIN SAJET.SYS_PDLINE E ON A.PDLINE_ID = E.PDLINE_ID "
                        + " LEFT JOIN SAJET.SYS_CUSTOMER F ON B.CUSTOMER_ID = F.CUSTOMER_ID ";

            if (combFilter.SelectedIndex > -1 && editFilter.Text.Trim() != "")
            {
                string sFieldName = combFilterField[combFilter.SelectedIndex].ToString();
                sSQL = sSQL + " WHERE A." + sFieldName + " LIKE '%" + editFilter.Text.Trim() + "%'";
            }

            sSQL = sSQL + " ORDER BY " + g_sOrderField;

            g_sDataSQL = sSQL;

            // 2016.4.26 By Jason
            gvData.Rows.Clear();

            editWO.Text = "";
            editPart.Text = "";
            editSpec1.Text = "";
            editSpec2.Text = "";
            editVersion.Text = "";
            editDue.Text = "";
            editRCNo.Text = "";
            editQty.Text = "";
            editType.Text = "";
            editPDLine.Text = "";
            editRoute.Text = "";
            editCustomer.Text = "";
            editPriority.Text = "";
            // 2016.4.26 End

            (new MESGridView.DisplayGridView()).GetGridView(gvData, sSQL, out memoryCache);

            //欄位Title  
            for (int i = 0; i <= gvData.Columns.Count - 1; i++)
            {
                gvData.Columns[i].Visible = false;
            }
            for (int i = 0; i <= TableDefine.tGridField.Length - 1; i++)
            {
                string sGridField = TableDefine.tGridField[i].sFieldName;

                if (gvData.Columns.Contains(sGridField))
                {
                    gvData.Columns[sGridField].HeaderText = TableDefine.tGridField[i].sCaption;
                    gvData.Columns[sGridField].DisplayIndex = i; //欄位顯示順序
                    gvData.Columns[sGridField].Visible = true;
                }
            }

            gvData.Focus();
        }

        private void editFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
            {
                return;
            }

            ShowData();
            SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);
            editFilter.Focus();
        }

        private void SetSelectRow(DataGridView GridData, String sPrimaryKey, String sField)
        {
            if (GridData.Rows.Count > 0)
            {
                int iIndex = 0;
                string sShowField = GridData.Columns[0].Name;

                for (int i = 0; i <= GridData.Columns.Count - 1; i++)
                {
                    if (GridData.Columns[i].Visible)
                    {
                        //第一個有顯示的欄位(focus到隱藏欄位會錯誤)
                        sShowField = GridData.Columns[i].Name;
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(sPrimaryKey))
                {
                    string sCondition = "";
                    string[] tsField = sField.Split(',');
                    string[] tsValue = sPrimaryKey.Split(',');

                    for (int j = 0; j <= tsField.Length - 1; j++)
                    {
                        if (j == 0)
                            sCondition = " WHERE " + tsField[j].ToString() + "='" + tsValue[j].ToString() + "' ";
                        else
                            sCondition = sCondition + " AND " + tsField[j].ToString() + "='" + tsValue[j].ToString() + "' ";

                    }

                    //改用SQL找,不由Grid讀值,否則速度會慢
                    string sText = "select idx from ("
                                 + " select aa.*,rownum-1 idx from ("
                                 + g_sDataSQL
                                 + " ) aa ) "
                                 + sCondition;
                    DataSet ds = ClientUtils.ExecuteSQL(sText);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        iIndex = Convert.ToInt32(ds.Tables[0].Rows[0]["idx"].ToString());
                    }
                }

                GridData.Focus();
                GridData.CurrentCell = GridData.Rows[iIndex].Cells[sShowField];
                GridData.Rows[iIndex].Selected = true;
            }
        }

        private void gvData_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            e.Value = memoryCache.RetrieveElement(e.RowIndex, e.ColumnIndex);
        }

        private void gvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex > -1)
            {
                g_sOrderField = gvData.Columns[e.ColumnIndex].Name;
                ShowData();
                SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);
            }
        }

        private void ClearData()
        {
            for (int i = 0; i <= panel2.Controls.Count - 1; i++)
            {
                if (panel2.Controls[i] is TextBox)
                    panel2.Controls[i].Text = "";
            }

            gvComponent.DataSource = null;
        }

        private void ShowDetail()
        {
            string sRCNo = gvData.CurrentRow.Cells["RC_NO"].Value.ToString();
            string sSQL = "SELECT B.SERIAL_NUMBER "
                        + "  FROM SAJET.G_RC_STATUS A,SAJET.G_SN_STATUS B "
                        + " WHERE A.RC_NO = B.RC_NO "
                        + "   AND A.RC_NO ='" + sRCNo + "' ";

            DataSet dsWoData = ClientUtils.ExecuteSQL(sSQL);

            editWO.Text = gvData.CurrentRow.Cells["WORK_ORDER"].Value.ToString();
            editPart.Text = gvData.CurrentRow.Cells["PART_NO"].Value.ToString();
            editVersion.Text = gvData.CurrentRow.Cells["VERSION"].Value.ToString();
            if (string.IsNullOrEmpty(gvData.CurrentRow.Cells["WO_DUE_DATE"].Value.ToString()))
                editDue.Text = "";
            else
                editDue.Text = DateTime.Parse(gvData.CurrentRow.Cells["WO_DUE_DATE"].Value.ToString()).ToString("yyyy/MM/dd");
            editType.Text = gvData.CurrentRow.Cells["WO_TYPE"].Value.ToString();
            editPDLine.Text = gvData.CurrentRow.Cells["PDLINE_NAME"].Value.ToString();
            editRoute.Text = gvData.CurrentRow.Cells["ROUTE_NAME"].Value.ToString();
            editCustomer.Text = gvData.CurrentRow.Cells["CUSTOMER_NAME"].Value.ToString();
            editRCNo.Text = gvData.CurrentRow.Cells["RC_NO"].Value.ToString();
            editQty.Text = gvData.CurrentRow.Cells["CURRENT_QTY"].Value.ToString();

            // 2016.4.25 By Jason (帶出品名)
            editSpec1.Text = gvData.CurrentRow.Cells["SPEC1"].Value.ToString();
            editSpec2.Text = gvData.CurrentRow.Cells["SPEC2"].Value.ToString();
            // 2016.4.25 End

            switch (gvData.CurrentRow.Cells["PRIORITY_LEVEL"].Value.ToString())
            {
                case "0":
                    editPriority.Text = SajetCommon.SetLanguage("Normal", 1);
                    break;
                case "1":
                    editPriority.Text = SajetCommon.SetLanguage("Prior", 1);
                    break;
                case "2":
                    editPriority.Text = SajetCommon.SetLanguage("Urgent", 1);
                    break;
            }

            if (dsWoData.Tables[0].Rows.Count != 0)
            {
                gvComponent.DataSource = dsWoData;
                gvComponent.DataMember = dsWoData.Tables[0].ToString();
                gvComponent.Columns["SERIAL_NUMBER"].HeaderText = SajetCommon.SetLanguage("Serial Number", 1);
            }
            else gvComponent.DataSource = null;
        }

        private void btnAppend_Click(object sender, EventArgs e)
        {
            fData f = new fData(this);

            try
            {
                f.g_sUpdateType = "APPEND";
                f.g_sformText = btnAppend.Text;

                if (f.ShowDialog() == DialogResult.OK)
                {
                    ShowData();
                    SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);
                }
            }
            finally
            {
                f.Dispose();
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            saveFileDialog1.DefaultExt = "xls";
            saveFileDialog1.Filter = "All Files(*.xls)|*.xls";

            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            string sFileName = saveFileDialog1.FileName;

            ExportExcel.CreateExcel Export = new ExportExcel.CreateExcel(sFileName);
            Export.ExportToExcel(gvData);
        }

        private void gvData_SelectionChanged(object sender, EventArgs e)
        {
            if (gvData.RowCount == 0 || gvData.CurrentRow == null)
                return;

            ShowDetail();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.SelectedRows.Count == 0)
                return;

            string sMsg = btnDelete.Text + " " + gvData.SelectedRows.Count.ToString() + " "
                + SajetCommon.SetLanguage("record(s)", 1) + "?";

            if (SajetCommon.Show_Message(sMsg, 2) != DialogResult.Yes)
                return;

            sMsg = "";

            foreach (DataGridViewRow selectedRow in gvData.SelectedRows)
            {
                string sID = selectedRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();
                string sWO = selectedRow.Cells["WORK_ORDER"].Value.ToString();
                string sData = gvData.Columns[TableDefine.gsDef_KeyData].HeaderText + " : " + selectedRow.Cells[TableDefine.gsDef_KeyData].Value.ToString();

                string sSQL = " SELECT RC_NO FROM SAJET.G_RC_TRAVEL WHERE RC_NO = '" + sID + "'";
                DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);

                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    sMsg += Environment.NewLine + sData;
                    continue;
                }

                sSQL = " SELECT B.INPUT_QTY, A.CURRENT_QTY FROM " + TableDefine.gsDef_Table + " A, SAJET.G_WO_BASE B "
                     + "  WHERE " + TableDefine.gsDef_KeyField + " = '" + sID + "' AND A.WORK_ORDER = B.WORK_ORDER";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);

                decimal iCurrentQty = 0, iInputQty = 0;

                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    decimal.TryParse(dsTemp.Tables[0].Rows[0]["CURRENT_QTY"].ToString(), out iCurrentQty);
                    decimal.TryParse(dsTemp.Tables[0].Rows[0]["INPUT_QTY"].ToString(), out iInputQty);
                }

                iInputQty = iInputQty - iCurrentQty;

                if (iInputQty < 0) iInputQty = 0;

                // 2016.4.27 By Jason
                //sSQL = " UPDATE SAJET.G_WO_BASE SET INPUT_QTY = '" + iInputQty.ToString() + "',"
                //     + "                            UPDATE_USERID = '" + g_sUserID + "',"
                //     + "                            UPDATE_TIME = SYSDATE "
                //     + "  WHERE WORK_ORDER = '" + sWO + "'";
                //ClientUtils.ExecuteSQL(sSQL);
                //fData.CopyToWOHistory(sWO);

                if (iCurrentQty > 0)
                {
                    sSQL = " UPDATE SAJET.G_WO_BASE SET INPUT_QTY = '" + iInputQty.ToString() + "',"
                         + "                            UPDATE_USERID = '" + g_sUserID + "',"
                         + "                            UPDATE_TIME = SYSDATE "
                         + "  WHERE WORK_ORDER = '" + sWO + "'";
                    ClientUtils.ExecuteSQL(sSQL);
                    fData.CopyToWOHistory(sWO);
                }

                // 2016.4.27 End

                sSQL = " UPDATE SAJET.G_SN_STATUS SET RC_NO = 'N/A', "
                     + "                              UPDATE_USERID = '" + g_sUserID + "',"
                     + "                              UPDATE_TIME = SYSDATE "
                     + "  WHERE RC_NO = '" + sID + "'";
                ClientUtils.ExecuteSQL(sSQL);

                sSQL = " DELETE " + TableDefine.gsDef_Table + " "
                     + "  WHERE " + TableDefine.gsDef_KeyField + " = '" + sID + "'";
                ClientUtils.ExecuteSQL(sSQL);

                sSQL = " DELETE SAJET.G_RC_TRAVEL_DEFECT "
                     + "  WHERE RC_NO = '" + sID + "'";
                ClientUtils.ExecuteSQL(sSQL);

                sSQL = " DELETE SAJET.G_RC_TRAVEL_MACHINE "
                     + "  WHERE RC_NO = '" + sID + "'";
                ClientUtils.ExecuteSQL(sSQL);

                sSQL = " DELETE SAJET.G_RC_TRAVEL_MATERIAL "
                     + "  WHERE RC_NO = '" + sID + "'";
                ClientUtils.ExecuteSQL(sSQL);

                sSQL = " DELETE SAJET.G_RC_TRAVEL_PARAM "
                     + "  WHERE RC_NO = '" + sID + "'";
                ClientUtils.ExecuteSQL(sSQL);

                sSQL = " DELETE SAJET.G_RC_TRAVEL_TOOLING "
                     + "  WHERE RC_NO = '" + sID + "'";
                ClientUtils.ExecuteSQL(sSQL);
            }

            if (sMsg != "")
            {
                sMsg = SajetCommon.SetLanguage("Production record founded, not allow to delete the data", 1) + sMsg;
                SajetCommon.Show_Message(sMsg, 0);
            }

            ShowData();

            SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);
        }
    }
}
