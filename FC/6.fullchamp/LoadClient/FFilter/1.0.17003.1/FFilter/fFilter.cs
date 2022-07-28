using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using FFilter.Enums;
using System.Reflection;

namespace SajetFilter
{
    /// <summary>
    /// Sajet 查詢視窗
    /// </summary>
    public partial class FFilter : Form
    {
        #region 屬性與變數

        /// <summary>
        /// 查詢結果
        /// </summary>
        public List<DataRow> ResultSets = new List<DataRow>();

        /// <summary>
        /// 不顯示的欄位名稱
        /// </summary>
        private readonly List<string> HiddenColumnNames = new List<string>();

        /// <summary>
        /// 純檢視模式
        /// </summary>
        private readonly bool ReadOnly = false;

        /// <summary>
        /// 項目為多選
        /// </summary>
        private readonly bool MultiSelect = false;

        /// <summary>
        /// 顯示勾選欄位
        /// </summary>
        private readonly bool ShowCheckBoxColumn = false;

        /// <summary>
        /// 勾選欄的名稱
        /// </summary>
        private string CheckBoxColumnName = string.Empty;

        /// <summary>
        /// 進階搜尋列元件
        /// </summary>
        private readonly bool AdvancedSearch = true;

        /// <summary>
        /// SQL 查詢指令
        /// </summary>
        private readonly string S;

        /// <summary>
        /// SQL 查詢參數
        /// </summary>
        private readonly List<object[]> P;

        /// <summary>
        /// 查詢欄位
        /// </summary>
        private readonly Dictionary<string, string> Fields = new Dictionary<string, string>();

        #endregion

        /// <summary>
        /// 查詢視窗
        /// </summary>
        /// <param name="sqlCommand">SQL 查詢指令</param>
        /// <param name="params">SQL 查詢參數</param>
        /// <param name="hiddenColumns">不顯示的欄位名稱</param>
        /// <param name="advancedSearch">進階搜尋列</param>
        /// <param name="readOnly">是否為檢視資料模式</param>
        /// <param name="multiSelect">項目為多選</param>
        /// <param name="checkBox">需要勾選欄位</param>
        /// <param name="checkBoxColumnName">勾選欄位的名稱</param>
        public FFilter(
            string sqlCommand = "",
            List<object[]> @params = null,
            List<string> hiddenColumns = null,
            bool advancedSearch = true,
            bool readOnly = false,
            bool multiSelect = false,
            bool checkBox = false,
            string checkBoxColumnName = "")
        {
            #region 設定屬性

            InitializeComponent();

            SajetCommon.g_sCallerName = Assembly.GetCallingAssembly().GetName().Name;

            SajetCommon.SetLanguageControl(this);

            P = @params;

            S = sqlCommand;

            ReadOnly = readOnly;

            HiddenColumnNames = hiddenColumns;

            if (ReadOnly)
            {
                MultiSelect = false;

                AdvancedSearch = false;

                ShowCheckBoxColumn = false;

                BtnOk.Visible = BtnOk.Enabled = false;

                BtnCancel.Text = SajetCommon.SetLanguage(MessageEnum.Close.ToString());
            }
            else
            {
                MultiSelect = multiSelect;

                ShowCheckBoxColumn = checkBox;

                AdvancedSearch = advancedSearch;
            }

            ShowCheckBoxColumn = MultiSelect || ShowCheckBoxColumn;

            CheckBoxColumnName = checkBoxColumnName;

            AdvancedSearchControls(enable: AdvancedSearch);

            BtnAll.Enabled = BtnAll.Visible = MultiSelect;

            BtnNone.Enabled = BtnNone.Visible = MultiSelect;

            #endregion

            #region 設定事件

            Load += FFilter_Load;

            BtnSearch.Click += BtnSearch_Click;

            BtnOk.Click += BtnOk_Click;

            BtnCancel.Click += BtnCancel_Click;

            TbField.KeyPress += TbField_KeyPress;

            if (MultiSelect)
            {
                BtnAll.Click += BtnAll_Click;

                BtnNone.Click += BtnNone_Click;
            }

            #endregion
        }

        private void FFilter_Load(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            ActivateControls(false);

            DataSet d = null;

            if (string.IsNullOrWhiteSpace(S))
            {
                string message = SajetCommon.SetLanguage("No result");

                SajetCommon.Show_Message(message, 1);

                return;
            }

            try
            {
                string s = $@"
SELECT
    *
FROM
    (
        {S}
    ) base_query
WHERE
    0 = 1
";
                var p = new List<object[]>();

                if (P != null)
                {
                    p.AddRange(P);
                }

                d = ClientUtils.ExecuteSQL(s, P?.ToArray());
            }
            catch (OracleException ex)
            {
                SajetCommon.Show_Message(ex.Message, 1);
            }

            if (d != null)
            {
                ActivateControls(true);

                SelectionMode(multiSelect: MultiSelect, showCheckBoxColumn: ShowCheckBoxColumn);

                LoadDataIntoComboBox(ref d);

                CbField.SelectedIndex = 0;

                d = DoQuery();

                LoadDataIntoDataGridView(ref d);
            }

            Cursor = Cursors.Default;
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            var d = DoQuery();

            LoadDataIntoDataGridView(ref d);

            Cursor = Cursors.Default;
        }

        private void BtnAll_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (DgvData.Rows.Count < 0)
            {
                return;
            }

            foreach (DataGridViewRow row in DgvData.Rows)
            {
                row.Cells[CheckBoxColumnName].Value = true;

                row.DefaultCellStyle.BackColor = Color.Lavender;
            }

            Cursor = Cursors.Default;
        }

        private void BtnNone_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (DgvData.Rows.Count < 0)
            {
                return;
            }

            foreach (DataGridViewRow row in DgvData.Rows)
            {
                row.Cells[CheckBoxColumnName].Value = false;

                row.DefaultCellStyle.BackColor = Color.White;
            }

            Cursor = Cursors.Default;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (!GetResultSets())
            {
                string message = SajetCommon.SetLanguage("No item selected");

                Cursor = Cursors.Default;

                SajetCommon.Show_Message(message, 1);

                return;
            }

            Cursor = Cursors.Default;

            DialogResult = DialogResult.OK;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void TbField_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return ||
                CbField.SelectedIndex < 0 ||
                DgvData.Rows.Count <= 0 ||
                string.IsNullOrWhiteSpace(TbField.Text))
            {
                return;
            }

            int currentRowTndex = DgvData.CurrentRow?.Index ?? 0;

            if (currentRowTndex == DgvData.Rows.Count - 1)
            {
                currentRowTndex = 0;
            }

            string searchText = TbField.Text.Trim().ToUpper();

            string columnName = Fields[CbField.Text];

            var rowIndex
                = DgvData.Rows.Cast<DataGridViewRow>()
                .Skip(currentRowTndex + 1)
                .FirstOrDefault(x =>
                {
                    string cellValue = x.Cells[columnName].Value.ToString();

                    return cellValue.ToUpper().Contains(searchText);
                })?.Index;

            if (rowIndex == null)
            {
                rowIndex
                = DgvData.Rows.Cast<DataGridViewRow>()
                .FirstOrDefault(x =>
                {
                    string cellValue = x.Cells[columnName].Value.ToString();

                    return cellValue.ToUpper().Contains(searchText);
                })?.Index;
            }

            int columnIndex = DgvData.FirstDisplayedCell.ColumnIndex;

            if (DgvData.Columns[columnName]?.Visible ?? false)
            {
                columnIndex = DgvData.Columns[columnName].DisplayIndex;
            }

            DgvData.CurrentCell = DgvData.Rows[rowIndex ?? 0]
                .Cells[columnIndex];
        }

        /// <summary>
        /// 處理勾選行為。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (ShowCheckBoxColumn &&
                DgvData.CurrentCell.ColumnIndex == DgvData.Columns[CheckBoxColumnName].DisplayIndex)
            {
                var currentRow = DgvData.CurrentRow;

                bool checkState = (bool)currentRow.Cells[CheckBoxColumnName].EditedFormattedValue;

                if (!MultiSelect)
                {
                    foreach (DataGridViewRow row in DgvData.Rows)
                    {
                        row.Cells[CheckBoxColumnName].Value = false;
                    }
                }

                currentRow.Cells[CheckBoxColumnName].Value = !checkState;

                if (!checkState)
                {
                    currentRow.DefaultCellStyle.BackColor = Color.Lavender;
                }
                else
                {
                    currentRow.DefaultCellStyle.BackColor = Color.White;
                }
            }

            Cursor = Cursors.Default;
        }

        /// <summary>
        /// 單選模式下，處理點擊兩下的行為。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DgvData.CurrentRow != null && e.RowIndex > -1)
            {
                BtnOk.PerformClick();
            }
        }

        /// <summary>
        /// 啟用查詢視窗上的控制項目。
        /// </summary>
        /// <param name="active"></param>
        private void ActivateControls(bool active = false)
        {
            CbField.Enabled = active;

            TbField.Enabled = active;

            BtnSearch.Enabled = active;

            BtnOk.Enabled = active;

            DgvData.Enabled = active;
        }

        /// <summary>
        /// 啟用搜尋列元件
        /// </summary>
        /// <param name="enable"></param>
        private void AdvancedSearchControls(bool enable = false)
        {
            LbSearch.Visible = LbSearch.Enabled = enable;

            CbField.Visible = CbField.Enabled = enable;

            TbField.Visible = TbField.Enabled = enable;

            BtnSearch.Visible = BtnSearch.Enabled = enable;
        }

        /// <summary>
        /// 決定查詢視窗的篩選模式。
        /// </summary>
        /// <param name="multiSelect">是否為多選</param>
        /// <param name="showCheckBoxColumn">顯示勾選欄位</param>
        private void SelectionMode(bool multiSelect = false, bool showCheckBoxColumn = false)
        {
            CheckBoxColumnName = string.IsNullOrWhiteSpace(CheckBoxColumnName) ? nameof(MultiSelect) : CheckBoxColumnName;

            var checkBoxColumn = new DataGridViewCheckBoxColumn
            {
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DisplayIndex = 0,
                Name = CheckBoxColumnName,
                HeaderText = SajetCommon.SetLanguage(CheckBoxColumnName),
                DataPropertyName = CheckBoxColumnName,
                Visible = true,
            };

            DgvData.Columns.Add(checkBoxColumn);

            if (showCheckBoxColumn)
            {
                DgvData.CellClick += DgvData_CellClick;
            }

            if (!multiSelect)
            {
                DgvData.CellDoubleClick += DgvData_CellDoubleClick;
            }
        }

        /// <summary>
        /// 將資料來源載入到 DataGridView 控制項。
        /// </summary>
        /// <param name="d"></param>
        private void LoadDataIntoDataGridView(ref DataSet d)
        {
            Cursor = Cursors.WaitCursor;

            DgvData.DataSource = d;

            DgvData.DataMember = d.Tables[0].ToString();

            var sl = new List<string>();

            if (ShowCheckBoxColumn)
            {
                foreach (DataGridViewRow row in DgvData.Rows)
                {
                    if ((bool)row.Cells[CheckBoxColumnName].EditedFormattedValue)
                    {
                        row.DefaultCellStyle.BackColor = Color.Lavender;
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.White;
                    }
                }
            }
            else
            {
                sl.Add(CheckBoxColumnName);
            }

            if (HiddenColumnNames != null && HiddenColumnNames.Any())
            {
                sl.AddRange(HiddenColumnNames);
            }

            foreach (DataGridViewColumn column in DgvData.Columns)
            {
                if (sl.Any(x =>
                {
                    return x.Trim().ToUpper() == column.Name.Trim().ToUpper();
                }))
                {
                    column.Visible = false;
                }

                column.HeaderText = SajetCommon.SetLanguage(column.Name);
            }

            DgvData.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            Cursor = Cursors.Default;
        }

        /// <summary>
        /// 將資料來源載入到下拉選單控制項。
        /// </summary>
        /// <param name="d"></param>
        private void LoadDataIntoComboBox(ref DataSet d)
        {
            var sl = new List<string>
            {
                CheckBoxColumnName
            };

            if (HiddenColumnNames != null && HiddenColumnNames.Any())
            {
                sl.AddRange(HiddenColumnNames);
            }

            foreach (DataColumn column in d.Tables[0].Columns)
            {
                if (!sl.Any(x =>
                {
                    return x.Trim().ToUpper() == column.ColumnName.Trim().ToUpper();
                }))
                {
                    string field = SajetCommon.SetLanguage(column.ColumnName);

                    Fields.Add(key: field, value: column.ColumnName);

                    CbField.Items.Add(field);
                }
            }
        }

        /// <summary>
        /// 根據搜尋條件，執行 SQL 查詢。
        /// </summary>
        /// <returns></returns>
        private DataSet DoQuery()
        {
            string s = $@"
SELECT
    *
FROM
    (
        {S}
    ) base_query
WHERE
    1 = 1
";


            var p = new List<object[]>();

            if (P != null)
            {
                p.AddRange(P);
            }

            if (CbField.SelectedIndex > -1 && !string.IsNullOrWhiteSpace(TbField.Text))
            {
                s += $@"
    AND upper(base_query.{Fields[CbField.Text]}) LIKE :field_value || '%'
";
                p.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "field_value", TbField.Text.Trim().ToUpper() });
            }

            var d = ClientUtils.ExecuteSQL(s, p?.ToArray());

            return d;
        }

        /// <summary>
        /// 依據操作模式，取得選取項目。
        /// </summary>
        private bool GetResultSets()
        {
            if (DgvData.Rows.Count < 0 || DgvData.CurrentRow == null)
            {
                return false;
            }

            if (ShowCheckBoxColumn)
            {
                ResultSets = DgvData.Rows
                    .Cast<DataGridViewRow>()
                    .Where(x =>
                    {
                        var cell = x.Cells[CheckBoxColumnName];

                        bool IsSelected = (bool)cell.EditedFormattedValue;

                        return IsSelected;
                    }).Select(x => (x.DataBoundItem as DataRowView).Row)
                    .ToList();
            }
            else
            {
                var dataRowView = DgvData.CurrentRow.DataBoundItem as DataRowView;

                ResultSets = new List<DataRow>
                {
                    dataRowView.Row,
                };
            }

            return ShowCheckBoxColumn || ResultSets.Count > 0;
        }
    }
}
