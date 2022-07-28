using System;
using System.Data;
using System.Windows.Forms;
using SajetClass;
using SajetTable;
using OtSrv = RCPart.Services.OtherService;

namespace RCPart
{
    public partial class fMain : Form
    {
        private MESGridView.Cache memoryCache;

        public fMain()
        {
            InitializeComponent();
        }

        string sSQL;
        public static string g_sUserID;
        public string g_sProgram, g_sFunction;
        public string g_sOrderField;
        public static int g_iPlanPrivilege;
        string g_sDataSQL;
        bool g_bFilter;

        private void Initial_Form()
        {
            g_sUserID = ClientUtils.UserPara1;
            g_sProgram = ClientUtils.fProgramName;
            g_sFunction = ClientUtils.fFunctionName;
            g_sOrderField = TableDefine.gsDef_OrderField;

            SajetCommon.SetLanguageControl(this);
            panel1.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
        }
        private void fMain_Load(object sender, EventArgs e)
        {
            TableDefine.Initial_Table();
            Initial_Form();
            this.Text = this.Text + "(" + SajetCommon.g_sFileVersion + ")";

            //Filter
            combFilter1.Items.Clear();
            combFilter2.Items.Clear();
            combFilter3.Items.Clear();
            combFilterField.Items.Clear();
            for (int i = 0; i <= TableDefine.tGridField.Length - 1; i++)
            {
                combFilter1.Items.Add(TableDefine.tGridField[i].sCaption);
                combFilter2.Items.Add(TableDefine.tGridField[i].sCaption);
                combFilter3.Items.Add(TableDefine.tGridField[i].sCaption);
                combFilterField.Items.Add(TableDefine.tGridField[i].sFieldName);
            }

            Check_Privilege();
            ToolItemProcess.Visible = CheckTableExist();

            g_bFilter = false;
            sSQL = " select count(*) cnt from sajet.sys_part ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (Convert.ToInt32(dsTemp.Tables[0].Rows[0]["cnt"].ToString()) > 5000)
            {
                g_bFilter = true;
            }


            combShow.SelectedIndex = 0;
        }
        private bool CheckTableExist()
        {
            sSQL = " select * from all_tables where table_name='SYS_DFI_PART_PROCESS' ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            return (dsTemp.Tables[0].Rows.Count > 0);
        }
        private void Check_Privilege()
        {
            string sPrivilege = ClientUtils.GetPrivilege(g_sUserID, g_sFunction, g_sProgram).ToString();

            btnAppend.Enabled = SajetCommon.CheckEnabled("INSERT", sPrivilege);
            btnModify.Enabled = SajetCommon.CheckEnabled("UPDATE", sPrivilege);
            btnEnabled.Enabled = SajetCommon.CheckEnabled("ENABLED", sPrivilege);
            btnDisabled.Enabled = SajetCommon.CheckEnabled("DISABLED", sPrivilege);
            btnDelete.Enabled = SajetCommon.CheckEnabled("DELETE", sPrivilege);

            //找Sampling Plan Default權限(有此權限才可設定Sampling Plan)
            g_iPlanPrivilege = ClientUtils.GetPrivilege(g_sUserID, "Sampling Plan Default", g_sProgram);
            ToolItemProcess.Enabled = SajetCommon.CheckEnabled("UPDATE", sPrivilege);
        }

        private void combShow_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDelete.Visible = (combShow.SelectedIndex == 1);
            btnDisabled.Visible = (combShow.SelectedIndex == 0);
            btnEnabled.Visible = (combShow.SelectedIndex == 1);

            ShowData();
            SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);
        }

        public void ShowData()
        {
            if (g_bFilter &&
                string.IsNullOrEmpty(editFilter1.Text) &&
                string.IsNullOrEmpty(editFilter2.Text) &&
                string.IsNullOrEmpty(editFilter3.Text)
                )
            {
                return;
            }

            Cursor = Cursors.WaitCursor;

            sSQL = @"
Select
    *
from
    SAJET.V_PART
where
    1 = 1
";
            if (combShow.SelectedIndex == 0)
            {
                sSQL += " and Enabled = 'Y' ";
            }
            else if (combShow.SelectedIndex == 1)
            {
                sSQL += " and Enabled = 'N' ";
            }

            #region 搜尋欄位增加到 3 個

            if (combFilter1.SelectedIndex > -1 && !string.IsNullOrWhiteSpace(editFilter1.Text))
            {
                string sFieldName = combFilterField.Items[combFilter1.SelectedIndex].ToString();

                sSQL += $" and upper({sFieldName}) like '{editFilter1.Text.Trim().ToUpper()}%' ";
            }
            if (combFilter2.SelectedIndex > -1 && !string.IsNullOrWhiteSpace(editFilter2.Text))
            {
                string sFieldName = combFilterField.Items[combFilter2.SelectedIndex].ToString();

                sSQL += $" and upper({sFieldName}) like '{editFilter2.Text.Trim().ToUpper()}%' ";
            }
            if (combFilter3.SelectedIndex > -1 && !string.IsNullOrWhiteSpace(editFilter3.Text))
            {
                string sFieldName = combFilterField.Items[combFilter3.SelectedIndex].ToString();

                sSQL += $" and upper({sFieldName}) like '{editFilter3.Text.Trim().ToUpper()}%' ";
            }

            #endregion

            sSQL = sSQL + " order by " + g_sOrderField;
            g_sDataSQL = sSQL;
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

            //OtSrv.RearrangeDataGridView(ref gvData);

            Cursor = Cursors.Default;

            gvData.Focus();
        }

        private void btnDisable_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;

            string sID = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();
            string sType = "";
            string sEnabled = "";
            if (sender == btnDisabled)
            {
                sType = btnDisabled.Text;
                sEnabled = "N";
            }
            else if (sender == btnEnabled)
            {
                sType = btnEnabled.Text;
                sEnabled = "Y";
            }

            string sData = gvData.Columns[TableDefine.gsDef_KeyData].HeaderText + " : " + gvData.CurrentRow.Cells[TableDefine.gsDef_KeyData].Value.ToString();
            string sMsg = sType + " ?" + Environment.NewLine + sData;

            if (SajetCommon.Show_Message(sMsg, 2) != DialogResult.Yes)
                return;

            sSQL = " Update " + TableDefine.gsDef_Table + " "
                 + " set Enabled = '" + sEnabled + "'  "
                 + "    ,UPDATE_USERID = '" + g_sUserID + "'  "
                 + "    ,UPDATE_TIME = SYSDATE  "
                 + " where " + TableDefine.gsDef_KeyField + " = '" + sID + "'";
            ClientUtils.ExecuteSQL(sSQL);
            CopyToHistory(sID);

            ShowData();
            SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);
        }

        private void editFilter1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            ShowData();
            SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);

            editFilter1.Focus();
        }

        private void editFilter2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            ShowData();
            SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);

            editFilter2.Focus();
        }

        private void editFilter3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            ShowData();
            SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);

            editFilter3.Focus();
        }

        private void btnAppend_Click(object sender, EventArgs e)
        {
            fData f = null;
            try
            {
                f = new fData(this)
                {
                    UpdateTypeEnum = UpdateType.Append,
                    g_sformText = btnAppend.Text
                };
                if (f.ShowDialog() == DialogResult.OK)
                {
                    ShowData();
                    SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);
                }
            }
            finally
            {
                f?.Dispose();
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
            {
                return;
            }

            string sSelectKeyValue = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();

            using (var f = new fData
            {
                UpdateTypeEnum = UpdateType.Modify,
                g_sformText = btnModify.Text,
                dataCurrentRow = gvData.CurrentRow
            })
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    ShowData();
                    SetSelectRow(gvData, sSelectKeyValue, TableDefine.gsDef_KeyField);
                }
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            fData f = null;
            try
            {
                if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                    return;
                f = new fData
                {
                    UpdateTypeEnum = UpdateType.Copy,
                    g_sformText = btnCopy.Text,
                    dataCurrentRow = gvData.CurrentRow
                };
                string sSelectKeyValue = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();
                if (f.ShowDialog() == DialogResult.OK)
                {
                    ShowData();
                    SetSelectRow(gvData, sSelectKeyValue, TableDefine.gsDef_KeyField);
                }
            }
            finally
            {
                f?.Dispose();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;

            string sID = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();

            string sData = gvData.Columns[TableDefine.gsDef_KeyData].HeaderText + " : " + gvData.CurrentRow.Cells[TableDefine.gsDef_KeyData].Value.ToString();
            string sMsg = btnDelete.Text + " ?" + Environment.NewLine + sData;
            if (SajetCommon.Show_Message(sMsg, 2) != DialogResult.Yes)
                return;

            //同時刪除Sampling Plan和Packing Spec
            sSQL = $@"
DELETE SAJET.SYS_PART_PKSPEC
WHERE PART_ID = '{sID}'
";
            ClientUtils.ExecuteSQL(sSQL);
            sSQL = $@"
DELETE SAJET.SYS_QC_SAMPLING_DEFAULT
WHERE PART_ID = '{sID}'
";
            ClientUtils.ExecuteSQL(sSQL);

            sSQL = $@"
DELETE SAJET.SYS_PART_ROUTE
WHERE PART_ID = '{sID}'
";
            ClientUtils.ExecuteSQL(sSQL);

            sSQL = $@"
UPDATE {TableDefine.gsDef_Table}
SET ENABLED = 'Drop'
   ,UPDATE_USERID = '{g_sUserID}'
   ,UPDATE_TIME = SYSDATE
WHERE {TableDefine.gsDef_KeyField} = '{sID}'
";
            ClientUtils.ExecuteSQL(sSQL);
            CopyToHistory(sID);
            sSQL = $@"
DELETE {TableDefine.gsDef_Table}
WHERE {TableDefine.gsDef_KeyField} = '{sID}'
";
            ClientUtils.ExecuteSQL(sSQL);

            // 刪除相關的製程參數 2.0.17003.13
            sSQL = $@"
DELETE SAJET.SYS_RC_PROCESS_PARAM_PART
WHERE PART_ID = '{sID}'
";
            ClientUtils.ExecuteSQL(sSQL);
            // 2.0.17003.13

            ShowData();
            SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);
        }

        public static void CopyToHistory(string sID)
        {
            string sSQL = " Insert into " + TableDefine.gsDef_HTTable + " "
                        + " Select * from " + TableDefine.gsDef_Table + " "
                        + " where " + TableDefine.gsDef_KeyField + " = '" + sID + "' ";
            ClientUtils.ExecuteSQL(sSQL);
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

        private void SetSelectRow(DataGridView GridData, string sPrimaryKey, string sField)
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
                            sCondition = " Where " + tsField[j].ToString() + "='" + tsValue[j].ToString() + "' ";
                        else
                            sCondition = sCondition + " and " + tsField[j].ToString() + "='" + tsValue[j].ToString() + "' ";

                    }
                    //改用SQL找,不由Grid讀值,否則速度會慢
                    string sText = "select idx from ("
                                 + " Select aa.*,rownum-1 idx from ("
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

        private void MenuHistory_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;

            string sFieldID = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();
            string sSQL = TableDefine.History_SQL(sFieldID);
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);

            fHistory fHistory = new fHistory();
            fHistory.dgvHistory.DataSource = dsTemp;
            fHistory.dgvHistory.DataMember = dsTemp.Tables[0].ToString();

            //替換欄位名稱
            for (int i = 0; i <= fHistory.dgvHistory.Columns.Count - 1; i++)
            {
                string sGridField = fHistory.dgvHistory.Columns[i].HeaderText;
                string sField = "";
                for (int j = 0; j <= gvData.Columns.Count - 1; j++)
                {
                    sField = gvData.Columns[j].Name;
                    if (sGridField == sField)
                    {
                        fHistory.dgvHistory.Columns[i].HeaderText = gvData.Columns[j].HeaderText;
                        break;
                    }
                }
            }

            fHistory.ShowDialog();
            fHistory.Dispose();
        }

        private void MenuModify_Click(object sender, EventArgs e)
        {
            btnModify.PerformClick();
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

        private void gvData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvData.CurrentRow != null && e.RowIndex > -1)
            {
                btnModify.PerformClick();
            }
        }

        private void fMain_Shown(object sender, EventArgs e)
        {
            if (combFilter1.Items.Count > 0)
                combFilter1.SelectedIndex = 0;
            if (combFilter2.Items.Count > 0)
                combFilter2.SelectedIndex = 3;
            if (combFilter3.Items.Count > 0)
                combFilter3.SelectedIndex = 8;

            gvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            gvData.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            editFilter1.Focus();
        }

        private void ToolItemProcess_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;
            string sPartNo = gvData.CurrentRow.Cells["PART_NO"].Value.ToString();
            string sPartID = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();
            ListBox listbData = new ListBox();
            sSQL = " SELECT B.PROCESS_NAME "
                  + "   FROM SAJET.SYS_DFI_PART_PROCESS A , SAJET.SYS_PROCESS B "
                  + " WHERE A.PART_ID ='" + sPartID + "' "
                  + "  AND A.PROCESS_ID = B.PROCESS_ID ";

            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                listbData.Items.Add(dsTemp.Tables[0].Rows[i]["PROCESS_NAME"].ToString());
            }

            fSQLMultiList fList = new fSQLMultiList(0, listbData)
            {
                g_sPartNo = sPartNo
            };
            fList.lstAll.Items.Clear();
            int nTmp = 0;
            sSQL = " SELECT PROCESS_NAME FROM SAJET.SYS_PROCESS WHERE ENABLED='Y' ORDER BY PROCESS_NAME ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
            {
                //在新視窗的listbox顯示內容
                for (int j = 0; j < listbData.Items.Count; j++)
                {
                    if (dsTemp.Tables[0].Rows[i][0].ToString() == listbData.Items[j].ToString())
                    {
                        fList.lstSelect.Items.Add(dsTemp.Tables[0].Rows[i][0].ToString());
                        nTmp = 1;
                        break;
                    }
                    nTmp = 0;
                }
                if (nTmp == 0)
                {
                    fList.lstAll.Items.Add(dsTemp.Tables[0].Rows[i][0].ToString());
                }
            }
            if (fList.ShowDialog() == DialogResult.OK)
            {
                sSQL = "DELETE SAJET.SYS_DFI_PART_PROCESS "
                    + " WHERE PART_ID ='" + sPartID + "' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                for (int i = 0; i <= listbData.Items.Count - 1; i++)
                {
                    string sProcessID = SajetCommon.GetID("SAJET.SYS_PROCESS", "PROCESS_ID", "PROCESS_NAME", listbData.Items[i].ToString());
                    sSQL = "INSERT INTO SAJET.SYS_DFI_PART_PROCESS "
                       + "(PART_ID,PROCESS_ID,UPDATE_USERID ) "
                       + " VALUES "
                       + " ('" + sPartID + "','" + sProcessID + "','" + g_sUserID + "' ) ";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);
                }
            }
            fList.Dispose();
        }
    }

    public enum UpdateType
    {
        None,
        Append,
        Modify,
        Copy,
    }
}