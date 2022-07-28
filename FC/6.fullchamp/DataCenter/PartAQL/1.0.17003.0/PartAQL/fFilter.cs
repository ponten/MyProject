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

namespace SajetFilter
{
    public partial class fFilter : Form
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
        /// 項目為多選
        /// </summary>
        private readonly bool MultiSelect = false;

        private readonly bool ShowCheckBoxColumn = false;

        /// <summary>
        /// 勾選欄的名稱
        /// </summary>
        private string CheckBoxColumnName = string.Empty;

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
        /// <param name="params">SQL 查詢參數</param>
        /// <param name="sqlCommand">SQL 查詢指令</param>
        /// <param name="multiSelect">項目為多選</param>
        /// <param name="checkBoxColumnName">勾選欄的名稱</param>
        /// <param name="hiddenColumnNames">不顯示的欄位名稱</param>
        public fFilter(string sqlCommand = "", List<object[]> @params = null, bool multiSelect = false, bool showCheckBoxColumn = false, string checkBoxColumnName = "", List<string> hiddenColumnNames = null)
        {
            #region 設定屬性

            InitializeComponent();

            SajetCommon.SetLanguageControl(this);

            P = @params;

            S = sqlCommand;

            MultiSelect = multiSelect;

            ShowCheckBoxColumn = multiSelect || showCheckBoxColumn;

            CheckBoxColumnName = checkBoxColumnName;

            HiddenColumnNames = hiddenColumnNames;

            #endregion

            #region 設定事件

            Load += FFilter_Load;

            BtnSearch.Click += BtnSearch_Click;

            BtnOk.Click += BtnOk_Click;

            BtnCancel.Click += BtnCancel_Click;

            TbField.KeyPress += TbField_KeyPress;

            #endregion
        }

        #region 事件

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

                SelectionMode(MultiSelect);

                LoadDataIntoComboBox(ref d);

                CbField.SelectedIndex = 0;
            }

            if (P != null)
            {
                d = DoQuery();

                LoadDataIntoDataGridView(ref d);
            }

            Cursor = Cursors.Default;
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (CbField.SelectedIndex < 0 || string.IsNullOrWhiteSpace(TbField.Text))
            {
                string message = SajetCommon.SetLanguage("Specific searching condition required");

                Cursor = Cursors.Default;

                SajetCommon.Show_Message(message, 1);

                return;
            }

            var d = DoQuery();

            LoadDataIntoDataGridView(ref d);

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
            TbField.BackColor = SystemColors.Window;

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

            if (rowIndex == null)
            {
                TbField.BackColor = Color.Salmon;
            }

            DgvData.CurrentCell = DgvData.Rows[rowIndex ?? 0]
                .Cells[columnIndex];
        }

        /// <summary>
        /// 多選模式下，處理勾選行為。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var currentRow = DgvData.CurrentRow;

            bool checkState = (bool)currentRow.Cells[CheckBoxColumnName].EditedFormattedValue;

            if (ShowCheckBoxColumn)
            {
                foreach (DataGridViewRow row in DgvData.Rows)
                {
                    row.Cells[CheckBoxColumnName].Value = false;
                }
            }

            currentRow.Cells[CheckBoxColumnName].Value = !checkState;
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

        #endregion

        #region 方法

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
        /// 決定查詢結果為單選、或多選。
        /// </summary>
        /// <param name="MultiSelect"></param>
        private void SelectionMode(bool MultiSelect = false)
        {
            if (ShowCheckBoxColumn)
            {
                CheckBoxColumnName = string.IsNullOrWhiteSpace(CheckBoxColumnName) ? nameof(MultiSelect) : CheckBoxColumnName;

                var checkBoxColumn = new DataGridViewCheckBoxColumn
                {
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                    DisplayIndex = 0,
                    Name = CheckBoxColumnName,
                    HeaderText = SajetCommon.SetLanguage(CheckBoxColumnName),
                    DataPropertyName = CheckBoxColumnName,
                };

                DgvData.Columns.Add(checkBoxColumn);

                DgvData.CellClick += DgvData_CellClick;
            }
            else
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
            DgvData.DataSource = d;

            DgvData.DataMember = d.Tables[0].ToString();

            foreach (DataGridViewColumn column in DgvData.Columns)
            {
                if (HiddenColumnNames?.Any(x => x.Trim().ToUpper().Contains(column.Name.Trim().ToUpper())) ?? false)
                {
                    column.Visible = false;
                }

                column.HeaderText = SajetCommon.SetLanguage(column.Name);
            }

            DgvData.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        /// <summary>
        /// 將資料來源載入到下拉選單控制項。
        /// </summary>
        /// <param name="d"></param>
        private void LoadDataIntoComboBox(ref DataSet d)
        {
            var sl = new List<string>();

            sl.Add(CheckBoxColumnName);

            if (HiddenColumnNames.Any())
            {
                sl.AddRange(HiddenColumnNames);
            }

            foreach (DataColumn column in d.Tables[0].Columns)
            {
                if (!sl?.Any(x => x.Trim().ToUpper().Contains(column.ColumnName.Trim().ToUpper())) ?? true)
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

            if (MultiSelect)
            {
                ResultSets = DgvData.Rows
                    .Cast<DataGridViewRow>()
                    .Where(x =>
                    {
                        var cell = x.Cells[CheckBoxColumnName];

                        bool IsSelected = (bool)cell.EditedFormattedValue;

                        return IsSelected;
                    }).Select(x =>
                    {
                        var dataRowView = DgvData.CurrentRow.DataBoundItem as DataRowView;

                        return dataRowView.Row;
                    }).ToList();
            }
            else
            {
                var dataRowView = DgvData.CurrentRow.DataBoundItem as DataRowView;

                ResultSets = new List<DataRow>
                {
                    dataRowView.Row,
                };
            }

            return ResultSets.Count > 0;
        }

        #endregion
    }
}
