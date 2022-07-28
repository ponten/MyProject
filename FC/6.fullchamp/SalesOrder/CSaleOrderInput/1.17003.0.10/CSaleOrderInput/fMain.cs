using System;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Windows.Forms;

namespace CSaleOrderInput
{
    public partial class fMain : Form
    {
        private string sSQL;
        private DataSet dsTemp;
        private string g_sDataSQL, g_sDetailDataSQL;
        private MESGridView.Cache memoryCache;
        private MESGridView.Cache memoryCacheDetail;
        private DataStatus m_DataStatus = DataStatus.Enabled;

        public static string g_sUserID;
        public string g_sProgram, g_sFunction;
        public string g_sOrderField, g_sOrderDetailField;

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
            g_sOrderDetailField = TableDefine.gsDef_DtlOrderField;

            SajetCommon.SetLanguageControl(this);
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            panel1.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            TableDefine.Initial_Table();
            Initial_Form();

            //
            combShow.SelectedIndex = 0;

            Text = Text + @"(" + SajetCommon.g_sFileVersion + @")";

            //Filter - Master
            combFilter.Items.Clear();
            combFilterField.Items.Clear();
            for (int i = 0; i <= TableDefine.tGridField.Length - 1; i++)
            {
                combFilter.Items.Add(TableDefine.tGridField[i].sCaption);
                combFilterField.Items.Add(TableDefine.tGridField[i].sFieldName);
            }
            if (combFilter.Items.Count > 0)
                combFilter.SelectedIndex = 0;
            //Filter - Detail
            combDetailFilter.Items.Clear();
            combDetailFilter.Items.Clear();
            for (int i = 0; i <= TableDefine.tGridDetailField.Length - 1; i++)
            {
                combDetailFilter.Items.Add(TableDefine.tGridDetailField[i].sCaption);
                combDetailFilterField.Items.Add(TableDefine.tGridDetailField[i].sFieldName);
            }
            if (combDetailFilter.Items.Count > 0)
                combDetailFilter.SelectedIndex = 0;

            Check_Privilege();
        }
        private void Check_Privilege()
        {
            string sPrivilege = ClientUtils.GetPrivilege(g_sUserID, g_sFunction, g_sProgram).ToString();

            btnAppend.Enabled = SajetCommon.CheckEnabled("INSERT", sPrivilege);
            btnModify.Enabled = SajetCommon.CheckEnabled("UPDATE", sPrivilege);
            btnEnabled.Enabled = SajetCommon.CheckEnabled("ENABLED", sPrivilege);
            btnDisabled.Enabled = SajetCommon.CheckEnabled("DISABLED", sPrivilege);
            btnDelete.Enabled = false;
            btnDelete.Visible = false;

            btnDetailAppend.Enabled = btnAppend.Enabled;
            btnDetailModify.Enabled = btnModify.Enabled;
            btnDetailEnabled.Enabled = false;
            btnDetailDisabled.Enabled = false;
            btnDetailDisabled.Visible = false;

            // 允許刪除銷售明細 1.17003.0.
            btnDetailDelete.Enabled = btnDetailDelete.Visible
                = false;
                //= SajetCommon.CheckEnabled("DELETE", sPrivilege);
            // 1.17003.0.

            combDetailShow.Visible = false;
        }

        private void combShow_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combShow.SelectedIndex == 0)
            {
                m_DataStatus = DataStatus.Enabled;
            }
            else if (combShow.SelectedIndex == 1)
            {
                m_DataStatus = DataStatus.Disabled;
            }
            else
            {
                m_DataStatus = DataStatus.All;
            }

            btnDelete.Visible = false;
            btnDisabled.Visible = m_DataStatus == DataStatus.Enabled;
            btnEnabled.Visible = m_DataStatus == DataStatus.Disabled;

            btnDetailAppend.Enabled = m_DataStatus == DataStatus.Enabled;
            btnDetailModify.Enabled = m_DataStatus == DataStatus.Enabled;
            btnDetailEnabled.Visible = false;

            ShowData();
            SetSelectRow(gvData, "", "");
        }

        public void ShowData()
        {
            sSQL = "Select * from " + TableDefine.gsDef_Table + " ";
            if (m_DataStatus == DataStatus.Enabled)
                sSQL += " where FLAG = 'Y' ";
            else if (m_DataStatus == DataStatus.Disabled)
                sSQL += " where FLAG = 'N' ";

            if (combFilter.SelectedIndex > -1 && editFilter.Text.Trim() != "")
            {
                string sFieldName = combFilterField.Items[combFilter.SelectedIndex].ToString();
                if (m_DataStatus == DataStatus.Enabled || m_DataStatus == DataStatus.Disabled)
                    sSQL += " and ";
                else
                    sSQL += " where ";
                sSQL = sSQL + sFieldName + " like '" + editFilter.Text.Trim() + "%'";
            }

            sSQL += " order by NUMBER1 DESC, NUMBER2 ASC";
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

            gvData.Focus();

            // Should be a better location to check the button enabled/disabled
            if (gvData.RowCount == 0)
            {
                ShowDetailData();
            }

            bool isEnabled = gvData.RowCount != 0;
            btnModify.Enabled = isEnabled;
            btnDisabled.Enabled = isEnabled;
            btnEnabled.Enabled = isEnabled;
            btnExport.Enabled = isEnabled;
            btnExportAll.Enabled = isEnabled;

            btnDetailAppend.Enabled = isEnabled;
            btnDetailModify.Enabled = isEnabled;
            btnDetailExport.Enabled = isEnabled;
        }

        private void editFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            ShowData();
            SetSelectRow(gvData, "", "");

            editFilter.Focus();
        }

        private void btnAppend_Click(object sender, EventArgs e)
        {
            fData f = null;
            try
            {
                f = new fData(this)
                {
                    UpdateTypeEnum = UpdateType.Append,
                    g_sformText = SajetCommon.SetLanguage(UpdateType.Append.ToString(), 1)
                };
                if (f.ShowDialog() == DialogResult.OK)
                {
                    ShowData();
                    SetSelectRow(gvData, f.Number1, f.Number2);
                }
            }
            finally
            {
                f?.Dispose();
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            fData f = null;
            try
            {
                if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                    return;

                f = UpdateFData(UpdateType.Modify);
                if (f.ShowDialog() == DialogResult.OK)
                {
                    ShowData();
                    SetSelectRow(gvData, f.Number1, f.Number2);
                }
            }
            finally
            {
                f?.Dispose();
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            fData f = null;
            try
            {
                if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                    return;

                f = UpdateFData(UpdateType.Copy);
                if (f.ShowDialog() == DialogResult.OK)
                {
                    ShowData();
                    SetSelectRow(gvData, f.Number1, f.Number2);
                }
            }
            finally
            {
                f?.Dispose();
            }
        }

        private fData UpdateFData(UpdateType updateType)
        {
            fData f = new fData
            {
                UpdateTypeEnum = updateType,
                g_sformText = SajetCommon.SetLanguage(updateType.ToString(), 1),
                dataCurrentRow = gvData.CurrentRow
            };
            return f;
        }

        private void btnDisable_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;

            string n1 = gvData.CurrentRow.Cells["NUMBER1"].Value.ToString();
            string n2 = gvData.CurrentRow.Cells["NUMBER2"].Value.ToString();
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

            string number1Data = gvData.Columns["NUMBER1"].HeaderText + " : " + gvData.CurrentRow.Cells["NUMBER1"].Value.ToString();
            string number2Data = gvData.Columns["NUMBER2"].HeaderText + " : " + gvData.CurrentRow.Cells["NUMBER2"].Value.ToString();
            string sMsg = sType + " ?" + Environment.NewLine + number1Data + "   " + number2Data;

            if (SajetCommon.Show_Message(sMsg, 2) != DialogResult.Yes)
                return;

            sSQL = $" Update {TableDefine.gsDef_Table} set FLAG = '{sEnabled}' where NUMBER1 = '{n1}' and NUMBER2 = '{n2}' ";
            ClientUtils.ExecuteSQL(sSQL);

            ShowData();
            SetSelectRow(gvData, "", "");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // do nothing
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            var fileName = GetFileName();
            if (!string.IsNullOrEmpty(fileName))
            {
                ExportExcel.CreateExcel excelInstance = new ExportExcel.CreateExcel(fileName);
                if (sender == btnExport)
                    excelInstance.ExportToExcel(gvData);
                else if (sender == btnDetailExport)
                    excelInstance.ExportToExcel(gvDetail);
            }
        }

        private void btnExportAll_Click(object sender, EventArgs e)
        {
            try
            {
                var fileName = GetFileName();
                if (!string.IsNullOrEmpty(fileName))
                {
                    Cursor = Cursors.WaitCursor;
                    this.Enabled = false;
                    // make 2 DataTable copies of the 2 datagridviews
                    var headerTable = CreateHeaderTable(gvData);
                    var detailTable = CreateDetailTable();

                    // left outer join the 2 DataTables
                    var joinedTable = JoinTwoDataTablesWithKeys(headerTable, detailTable, "NUMBER1", "NUMBER2");

                    // Export to Excel
                    ExportExcel.CreateExcel excelInstance = new ExportExcel.CreateExcel(fileName);
                    excelInstance.ExportToExcel(joinedTable);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                this.Enabled = true;
                Cursor = Cursors.Default;
            }
        }



        /// <summary>
        /// refer: https://stackoverflow.com/questions/17684448/how-to-left-outer-join-two-datatables-in-c?utm_medium=organic&utm_source=google_rich_qa&utm_campaign=google_rich_qa
        /// </summary>
        private static DataTable JoinTwoDataTablesWithKeys(DataTable dtblLeft, DataTable dtblRight, string colToJoinOn1, string colToJoinOn2)
        {
            //Change column name to a temp name so the LINQ for getting row data will work properly.
            string strTempColName1 = colToJoinOn1 + "_2";
            if (dtblRight.Columns.Contains(colToJoinOn1))
                dtblRight.Columns[colToJoinOn1].ColumnName = strTempColName1;

            string strTempColName2 = colToJoinOn2 + "_2";
            if (dtblRight.Columns.Contains(colToJoinOn2))
                dtblRight.Columns[colToJoinOn2].ColumnName = strTempColName2;

            string realDate = "REAL_DATE";
            string strTempColName3 = realDate + "_Detail";
            if (dtblRight.Columns.Contains(realDate))
                dtblRight.Columns[realDate].ColumnName = strTempColName3;

            string realDueDate = "REAL_DUE_DATE";
            string strTempColName4 = realDueDate + "_Detail";
            if (dtblRight.Columns.Contains(realDueDate))
                dtblRight.Columns[realDueDate].ColumnName = strTempColName4;

            //Get columns from dtblLeft
            DataTable dtblResult = dtblLeft.Clone();

            //Get columns from dtblRight
            var dt2Columns = dtblRight.Columns.OfType<DataColumn>().Select(dc => new DataColumn(dc.ColumnName, dc.DataType, dc.Expression, dc.ColumnMapping));

            //Get columns from dtblRight that are not in dtblLeft
            var dt2FinalColumns = from dc in dt2Columns.AsEnumerable()
                                  where !dtblResult.Columns.Contains(dc.ColumnName)
                                  select dc;

            //Add the rest of the columns to dtblResult
            dtblResult.Columns.AddRange(dt2FinalColumns.ToArray());

            var rowDataLeftOuter = from rowLeft in dtblLeft.AsEnumerable()
                                   join rowRight in dtblRight.AsEnumerable() on new { a = rowLeft["NUMBER1"], b = rowLeft["NUMBER2"] } equals new { a = rowRight["NUMBER1_2"], b = rowRight["NUMBER2_2"] } into gj
                                   from subRight in gj.DefaultIfEmpty()
                                   select rowLeft.ItemArray.Concat(subRight?.ItemArray ?? (dtblRight.NewRow().ItemArray)).ToArray();

            //Add row data to dtblResult
            foreach (object[] values in rowDataLeftOuter)
                dtblResult.Rows.Add(values);

            //Change column name back to original
            dtblRight.Columns[strTempColName1].ColumnName = colToJoinOn1;
            dtblRight.Columns[strTempColName2].ColumnName = colToJoinOn2;
            dtblRight.Columns[strTempColName3].ColumnName = realDate;
            dtblRight.Columns[strTempColName4].ColumnName = realDueDate;

            //Remove extra column from result
            dtblResult.Columns.Remove(strTempColName1);
            dtblResult.Columns.Remove(strTempColName2);
            dtblResult.Columns.Remove("FLAG");

            // set language on ColumnName
            for (int i = 0; i < dtblResult.Columns.Count; ++i)
            {
                var caption = TableDefine.GetCaption(dtblResult.Columns[i].ColumnName);
                var name = SajetCommon.SetLanguage(caption, 1);
                dtblResult.Columns[i].ColumnName = name;
            }

            return dtblResult;
        }

        private DataTable CreateHeaderTable(DataGridView dgv)
        {
            DataTable dt = new DataTable();
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                dt.Columns.Add(col.Name);
            }

            foreach (DataGridViewRow row in dgv.Rows)
            {
                DataRow dRow = dt.NewRow();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    dRow[cell.ColumnIndex] = cell.Value;
                }
                dt.Rows.Add(dRow);
            }

            return dt;
        }

        private DataTable CreateDetailTable()
        {
            var realDate = "\"REAL_DATE\"";
            var realDueDate = "\"REAL_DUE_DATE\"";

            var sql = "select d.SEQUENCE, d.QUOTES1, d.QUOTES2, d.QUOTES3, d.CURRENCY, d.EXCHANGE_RATE, " +
                     "d.PAYMENT_METHOD, d.ADDRESS, d.PRODUCE_NUMBER, t.OPTION5, d.UNIT, d.QUANTITY, d.WEIGHT, " +
                     $"d.UNIT_PRICE, d.AMOUNT, to_char(d.REAL_DATE, 'YYYY/MM/DD') as {realDate}, to_char(d.REAL_DUE_DATE, 'YYYY/MM/DD') as {realDueDate} , d.CLIENT_PRODUCE_NUMBER, d.SPEC1, d.SPEC2, d.SPEC3, " +
                     "d.NUMBER1, d.NUMBER2 " +
                     "FROM SAJET.SYS_ORD_D d, SAJET.SYS_PART t " +
                     "WHERE d.PRODUCE_NUMBER = t.PART_NO";
            var ds = ClientUtils.ExecuteSQL(sql);
            return ds.Tables[0];
        }

        private string GetFileName()
        {
            saveFileDialog1.DefaultExt = "xls";
            saveFileDialog1.Filter = "All Files(*.xls)|*.xls";
            return saveFileDialog1.ShowDialog() == DialogResult.OK ? saveFileDialog1.FileName.Trim() : string.Empty;
        }

        private void SetSelectRow(DataGridView GridData, string pk1, string pk2, string pk3 = null)
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

                if (!string.IsNullOrEmpty(pk1) && !string.IsNullOrEmpty(pk2))
                {
                    var sCondition = $" Where NUMBER1 = '{pk1}' and NUMBER2 = '{pk2}'";

                    if (!string.IsNullOrEmpty(pk3))
                    {
                        sCondition += $" and SEQUENCE = '{pk3}'";
                    }

                    string sDataSQL = g_sDataSQL;
                    if (GridData != gvData)
                        sDataSQL = g_sDetailDataSQL;

                    //改用SQL找,不由Grid讀值,否則速度會慢
                    string sText = "select idx from ("
                                 + " Select aa.*,rownum-1 idx from ("
                                 + sDataSQL
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
            var rowIndex = e.RowIndex;
            var columnIndex = e.ColumnIndex;
            if (columnIndex == 12 || columnIndex == 13) // #12 is REAL_DATE, #13 is REAL_DUE_DATE
            {
                e.Value = DateTime.Parse(memoryCache.RetrieveElement(rowIndex, columnIndex)).ToString("yyyy/MM/dd");
            }
            else
            {
                e.Value = memoryCache.RetrieveElement(rowIndex, columnIndex);
            }
        }

        private void gvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex > -1)
            {
                g_sOrderField = gvData.Columns[e.ColumnIndex].Name;
                ShowData();
                SetSelectRow(gvData, "", "");
            }
        }

        #region The Detail table operations 
        public void ShowDetailData()
        {
            gvDetail.Rows.Clear();
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;

            var number1 = gvData.CurrentRow.Cells["NUMBER1"].Value.ToString();
            var number2 = gvData.CurrentRow.Cells["NUMBER2"].Value.ToString();

            sSQL = "Select b.NUMBER1, b.NUMBER2, b.PRODUCE_NUMBER, b.SEQUENCE, b.QUANTITY, b.AMOUNT, b.REAL_DUE_DATE "
                   + "from " + TableDefine.gsDef_Table + " a,"
                   + TableDefine.gsDef_DtlTable + " b "
                   + "where a.NUMBER1 = b.NUMBER1(+) "
                   + "and a.NUMBER2 = b.NUMBER2(+) "
                   + $"and a.NUMBER1 = {number1} and a.NUMBER2 = {number2}";

            if (combDetailShow.SelectedIndex == 0)
                sSQL += " and a.FLAG = 'Y' ";
            else if (combDetailShow.SelectedIndex == 1)
                sSQL += " and a.FLAG = 'N' ";

            if (combDetailFilter.SelectedIndex > -1 && editDetailFilter.Text.Trim() != "")
            {
                string sFieldName = combDetailFilterField.Items[combDetailFilter.SelectedIndex].ToString();
                sSQL = sSQL + " and " + sFieldName + " like '" + editDetailFilter.Text.Trim() + "%'";
            }

            sSQL += " order by b.SEQUENCE ASC";
            g_sDetailDataSQL = sSQL;
            (new MESGridView.DisplayGridView()).GetGridView(gvDetail, sSQL, out memoryCacheDetail);

            //欄位Title  
            for (int i = 0; i <= gvDetail.Columns.Count - 1; i++)
            {
                gvDetail.Columns[i].Visible = false;
            }
            for (int i = 0; i <= TableDefine.tGridDetailField.Length - 1; i++)
            {
                string sGridField = TableDefine.tGridDetailField[i].sFieldName;

                if (gvDetail.Columns.Contains(sGridField))
                {
                    gvDetail.Columns[sGridField].HeaderText = TableDefine.tGridDetailField[i].sCaption;
                    gvDetail.Columns[sGridField].DisplayIndex = i; //欄位顯示順序
                    gvDetail.Columns[sGridField].Visible = TableDefine.tGridDetailField[i].Visible;
                }
            }
            gvDetail.Focus();

            // This is a trick
            // To make the gvDetail have the focus when gvDetail is first shown
            if (gvDetail.RowCount > 0 && gvDetail.ColumnCount > 0)
            {
                gvDetail.CurrentCell = gvDetail[0, 0];
                gvDetail.CurrentCell.Selected = true;
            }
        }

        private void gvDetail_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (memoryCacheDetail != null)
            {
                var rowIndex = e.RowIndex;
                var columnIndex = e.ColumnIndex;
                if (columnIndex == 6) // #6 is REAL_DUE_DATE
                {
                    // 避免空字串轉型日期格式發生錯誤 1.17003.0.3
                    if (DateTime.TryParse(memoryCacheDetail.RetrieveElement(rowIndex, columnIndex).ToString(), out DateTime dateTime))
                    {
                        e.Value = dateTime.ToString("yyyy/MM/dd");
                    }
                    //e.Value = DateTime.Parse(memoryCacheDetail.RetrieveElement(rowIndex, columnIndex))
                    //    .ToString("yyyy/MM/dd");
                    // 1.17003.0.3
                }
                else
                {
                    e.Value = memoryCacheDetail.RetrieveElement(rowIndex, columnIndex);
                }
            }
        }

        private void gvData_SelectionChanged(object sender, EventArgs e)
        {
            ShowDetailData();
            gvData.Focus();
        }

        private void combDetailShow_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDetailDelete.Visible = false;
            btnDetailDisabled.Visible = false;
            btnDetailEnabled.Visible = false;
        }

        private void editDetailFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            ShowDetailData();
            SetSelectRow(gvDetail, "", "");

            editDetailFilter.Focus();
        }

        private void gvDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex > -1)
            {
                g_sOrderDetailField = gvDetail.Columns[e.ColumnIndex].Name;
                ShowDetailData();
                SetSelectRow(gvDetail, "", "");
            }
        }

        private void btnDetailAppend_Click(object sender, EventArgs e)
        {
            fDetailData f = null;
            try
            {
                if (gvData.Rows.Count == 0 || gvData.CurrentRow == null) { return; }

                f = new fDetailData(this)
                {
                    UpdateTypeEnum = UpdateType.Append,
                    g_sformText = btnAppend.Text,
                    Number1 = gvData.CurrentRow.Cells["NUMBER1"].Value.ToString(),
                    Number2 = gvData.CurrentRow.Cells["NUMBER2"].Value.ToString(),
                    RealDate = gvData.CurrentRow.Cells["REAL_DATE"].Value.ToString(),
                    RealDueDate = gvData.CurrentRow.Cells["REAL_DUE_DATE"].Value.ToString()
                };
                if (f.ShowDialog() == DialogResult.OK)
                {
                    ShowDetailData();
                    SetSelectRow(gvDetail, f.Number1, f.Number2, f.Sequence);
                }
            }
            finally
            {
                f?.Dispose();
            }
        }

        private void btnDetailModify_Click(object sender, EventArgs e)
        {
            fDetailData f = null;
            try
            {
                if (!NeedUpdateFData()) { return; }

                f = UpdateFDetailData(UpdateType.Modify);
                if (f.ShowDialog() == DialogResult.OK)
                {
                    ShowDetailData();
                    SetSelectRow(gvDetail, f.Number1, f.Number2, f.Sequence);
                }
            }
            finally
            {
                f?.Dispose();
            }
        }

        private void btnDetailCopy_Click(object sender, EventArgs e)
        {
            fDetailData f = null;
            try
            {
                if (!NeedUpdateFData()) { return; }

                f = UpdateFDetailData(UpdateType.Copy);
                if (f.ShowDialog() == DialogResult.OK)
                {
                    ShowDetailData();
                    SetSelectRow(gvDetail, f.Number1, f.Number2, f.Sequence);
                }
            }
            finally
            {
                f?.Dispose();
            }
        }

        private fDetailData UpdateFDetailData(UpdateType updateType)
        {
            var f = new fDetailData
            {
                UpdateTypeEnum = updateType,
                g_sformText = SajetCommon.SetLanguage(updateType.ToString(), 1),
                dataCurrentRow = gvDetail.CurrentRow
            };
            return f;
        }

        private bool NeedUpdateFData()
        {
            if (gvDetail.Rows.Count == 0 || gvDetail.CurrentRow == null)
            {
                return false;
            }

            // special handle 
            // when the first Detail row is empty, clicking Modify will do nothing
            var valueNumber1 = gvDetail.CurrentRow.Cells["NUMBER1"].Value;
            var valueNumber2 = gvDetail.CurrentRow.Cells["NUMBER2"].Value;
            if (valueNumber1 == null || valueNumber2 == null)
            {
                return false;
            }

            var n1 = valueNumber1.ToString().Trim();
            var n2 = valueNumber2.ToString().Trim();
            if (string.IsNullOrEmpty(n1) || string.IsNullOrEmpty(n2))
            {
                return false;
            }

            return true;
        }

        private void btnDetailDisabled_Click(object sender, EventArgs e)
        {
            // do nothing
        }

        private void btnDetailDelete_Click(object sender, EventArgs e)
        {
            /*
            // 1.17003.0.
            // 可以刪除銷售明細的條件
            // 1.沒有開工單紀錄
            // 2.序號是最後一筆
            string number1 = gvDetail.CurrentRow.Cells["NUMBER1"].Value.ToString();
            string number2 = gvDetail.CurrentRow.Cells["NUMBER2"].Value.ToString();
            string produceNumber = gvDetail.CurrentRow.Cells["PRODUCE_NUMBER"].Value.ToString();
            string sequence = gvDetail.CurrentRow.Cells["SEQUENCE"].Value.ToString();


            string SQL = @"
SELECT MAX(SEQUENCE)
FROM SAJET.SYS_ORD_D
WHERE NUMBER1 = :NUMBER1
AND NUMBER2 = :NUMBER2
AND SEQUENCE > :SEQUENCE
";

            if (!string.IsNullOrWhiteSpace(number1) &&
                !string.IsNullOrWhiteSpace(number2) &&
                !string.IsNullOrWhiteSpace(produceNumber) &&
                !string.IsNullOrWhiteSpace(sequence))
            {
                SQL = @"
SELECT *
FROM SAJET.G_WO_BASE
WHERE DATENUMBER = :NUMBER1
AND NUMBER1 = :NUMBER2
AND NUMBER2 = :SEQUENCE
AND PRODUCE_NUMBER = :PRODUCE_NUMBER
";
                object[][] param = new object[4][]
                {
                    new object[]{ ParameterDirection.Input, OracleType.VarChar, "NUMBER1", number1 },
                    new object[]{ ParameterDirection.Input, OracleType.VarChar, "NUMBER2", number2 },
                    new object[]{ ParameterDirection.Input, OracleType.VarChar, "SEQUENCE", sequence },
                    new object[]{ ParameterDirection.Input, OracleType.VarChar, "PRODUCE_NUMBER", produceNumber }
                };
                DataSet ds = ClientUtils.ExecuteSQL(SQL,param);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Found work order belong to this sale detail, therefore can not delete"), 1);
                    return;
                }
            }
            //1.17003.0.
            //*/
        }

        #endregion The Detail table operations
    }

    public enum DataStatus
    {
        Enabled,
        Disabled,
        All,
    }

    public enum UpdateType
    {
        Append = 0,
        Modify = 1,
        Copy = 2,
    }
}