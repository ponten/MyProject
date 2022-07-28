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
using MachineDown;
using System.Data.OracleClient;

namespace CMachineDown
{
    public partial class fMain : Form
    {
        private MESGridView.Cache memoryCache;

        private MESGridView.Cache memoryCacheDetail;

        public string g_sUserID;

        public string g_sProgram, g_sFunction;

        public string g_sOrderField, g_sOrderDetailField;

        string sSQL;

        string g_sDataSQL, g_sDetailDataSQL;

        string sYes = "Yes";
        string sNo = "No";

        DataSet dsTemp;

        #region Form 事件

        public fMain()
        {
            InitializeComponent();

            Initial_Form();

            Check_Privilege();

            TableDefine.Initial_Table();

            LoadComboBoxItems(nameof(combShow));

            LoadComboBoxItems(nameof(combDetailShow));

            LoadComboBoxItems(nameof(combFilter));

            LoadComboBoxItems(nameof(combDetailFilter));
        }

        private void FMain_Load(object sender, EventArgs e)
        {
            // 預設選項
            combShow.SelectedIndex = 0;

            combDetailShow.SelectedIndex = 0;

            if (combFilter.Items.Count > 0)
            {
                combFilter.SelectedIndex = 0;
            }

            if (combDetailFilter.Items.Count > 0)
            {
                combDetailFilter.SelectedIndex = 0;
            }

            sYes = SajetCommon.SetLanguage(sYes);

            sNo = SajetCommon.SetLanguage(sNo);

            this.Text = this.Text + "(" + SajetCommon.g_sFileVersion + ")";

            ShowData();
        }

        private void MenuHistory_Click(object sender, EventArgs e)
        {
            sSQL = "";

            DataGridView dvControl = (DataGridView)contextMenuStrip1.SourceControl;

            if (dvControl == gvData)
            {
                if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                {
                    return;
                }

                string sFieldID = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();

                sSQL = TableDefine.History_SQL(sFieldID);
            }
            else if (dvControl == gvDetail)
            {
                if (gvDetail.Rows.Count == 0 || gvDetail.CurrentRow == null)
                {
                    return;
                }

                string sFieldID = gvDetail.CurrentRow.Cells[TableDefine.gsDef_DtlKeyField].Value.ToString();

                sSQL = TableDefine.DetailHistory_SQL(sFieldID);
            }

            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);

            fHistory fHistory = new fHistory();

            fHistory.dgvHistory.DataSource = dsTemp;

            fHistory.dgvHistory.DataMember = dsTemp.Tables[0].ToString();

            foreach (DataGridViewColumn column in fHistory.dgvHistory.Columns)
            {
                column.HeaderText = SajetCommon.SetLanguage(column.Name, 1);
            }

            fHistory.ShowDialog();

            fHistory.Dispose();
        }

        #endregion

        #region 主表事件

        private void EditFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
            {
                return;
            }

            ShowData();

            SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);

            editFilter.Focus();
        }

        private void BtnAppend_Click(object sender, EventArgs e)
        {
            using (fData f = new fData(this)
            {
                g_sUpdateType = "APPEND",
                g_sformText = btnAppend.Text
            })
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    ShowData();

                    SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);
                }
            }
        }

        private void BtnModify_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
            {
                return;
            }

            using (fData f = new fData
            {
                g_sUpdateType = "MODIFY",
                g_sformText = btnModify.Text,
                dataCurrentRow = gvData.CurrentRow
            })
            {
                string sSelectKeyValue = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();

                if (f.ShowDialog() == DialogResult.OK)
                {
                    ShowData();

                    SetSelectRow(gvData, sSelectKeyValue, TableDefine.gsDef_KeyField);
                }
            }
        }

        private void BtnDisable_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
            {
                return;
            }

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
            {
                return;
            }

            sSQL = $@"
UPDATE
    {TableDefine.gsDef_Table}
SET
    ENABLED = :ENABLED
   ,UPDATE_USERID = :UPDATE_USERID
   ,UPDATE_TIME = SYSDATE
WHERE
    {TableDefine.gsDef_KeyField} = :SID
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "ENABLED", sEnabled },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "SID", sID }
            };

            ClientUtils.ExecuteSQL(sSQL, p.ToArray());

            CopyToHistory(sID);

            ShowData();

            SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
            {
                return;
            }

            try
            {
                string sID = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();

                string sData = gvData.Columns[TableDefine.gsDef_KeyData].HeaderText + " : " + gvData.CurrentRow.Cells[TableDefine.gsDef_KeyData].Value.ToString();

                //此Stage下有Process時,不可刪除
                sSQL = $@"
SELECT *
FROM
    SAJET.SYS_MACHINE_DOWN_DETAIL
WHERE
    TYPE_ID = :SID
";
                var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "SID", sID }
                };

                dsTemp = ClientUtils.ExecuteSQL(sSQL, p.ToArray());

                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    string sErrMsg1 = SajetCommon.SetLanguage("has been referenced", 1);

                    string sErrMsg2 = SajetCommon.SetLanguage("Can not delete it", 1);

                    SajetCommon.Show_Message($"{sData} {sErrMsg1}\n{sErrMsg2}!", 0);

                    return;
                }

                //=====                
                string sMsg = btnDelete.Text + " ?" + Environment.NewLine + sData;

                if (SajetCommon.Show_Message(sMsg, 2) != DialogResult.Yes)
                {
                    return;
                }

                sSQL = $@"
UPDATE
    {TableDefine.gsDef_Table}
SET
    ENABLED = 'Drop'
   ,UPDATE_USERID = :UPDATE_USERID
   ,UPDATE_TIME = SYSDATE
WHERE
    {TableDefine.gsDef_KeyField} = :SID
";
                p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "SID", sID }
                };

                dsTemp = ClientUtils.ExecuteSQL(sSQL, p.ToArray());

                CopyToHistory(sID);

                ShowData();

                SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception:" + ex.Message, 0);
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            saveFileDialog1.DefaultExt = "xls";

            saveFileDialog1.Filter = "All Files(*.xls)|*.xls";

            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string sFileName = saveFileDialog1.FileName;

            ExportExcel.CreateExcel Export = new ExportExcel.CreateExcel(sFileName);

            if (sender == btnExport)
            {
                Export.ExportToExcel(gvData);
            }
            else if (sender == btnDetailExport)
            {
                Export.ExportToExcel(gvDetail);
            }
        }

        private void CombShow_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDelete.Visible = (combShow.SelectedIndex == 1);

            btnDisabled.Visible = (combShow.SelectedIndex == 0);

            btnEnabled.Visible = (combShow.SelectedIndex == 1);

            ShowData();

            SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);
        }

        private void GvData_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            e.Value = memoryCache.RetrieveElement(e.RowIndex, e.ColumnIndex);
        }

        private void GvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex > -1)
            {
                g_sOrderField = gvData.Columns[e.ColumnIndex].Name;

                ShowData();

                SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);
            }
        }

        private void GvData_SelectionChanged(object sender, EventArgs e)
        {
            ShowDetailData();

            gvData.Focus();
        }

        #endregion

        #region 明細表事件

        private void EditDetailFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
            {
                return;
            }

            ShowDetailData();

            SetSelectRow(gvDetail, "", TableDefine.gsDef_DtlKeyField);

            editDetailFilter.Focus();
        }

        private void BtnDetailAppend_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
            {
                return;
            }

            using (fDetailData f = new fDetailData(this)
            {
                g_sUpdateType = "APPEND",
                g_sformText = btnAppend.Text,
                g_sTypeID = gvData.CurrentRow.Cells["TYPE_ID"].Value.ToString(),
                g_sDesc = gvData.CurrentRow.Cells["TYPE_CODE"].Value.ToString()
                + " : " + gvData.CurrentRow.Cells["TYPE_DESC"].Value.ToString(),
            })
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    ShowDetailData();

                    SetSelectRow(gvDetail, "", TableDefine.gsDef_DtlKeyField);
                }
            }
        }

        private void BtnDetailModify_Click(object sender, EventArgs e)
        {
            if (gvDetail.Rows.Count == 0 || gvDetail.CurrentRow == null)
            {
                return;
            }

            using (fDetailData f = new fDetailData
            {
                g_sUpdateType = "MODIFY",
                g_sformText = btnModify.Text,
                dataCurrentRow = gvDetail.CurrentRow,
                g_sTypeID = gvData.CurrentRow.Cells["TYPE_ID"].Value.ToString(),
                g_sDesc = gvData.CurrentRow.Cells["TYPE_CODE"].Value.ToString()
                + " : "
                + gvData.CurrentRow.Cells["TYPE_DESC"].Value.ToString()
            })
            {
                string sSelectKeyValue = gvDetail.CurrentRow.Cells[TableDefine.gsDef_DtlKeyField].Value.ToString();

                if (f.ShowDialog() == DialogResult.OK)
                {
                    ShowDetailData();

                    SetSelectRow(gvDetail, sSelectKeyValue, TableDefine.gsDef_DtlKeyField);
                }
            }
        }

        private void BtnDetailDisabled_Click(object sender, EventArgs e)
        {
            if (gvDetail.Rows.Count == 0 || gvDetail.CurrentRow == null)
            {
                return;
            }

            string sID = gvDetail.CurrentRow.Cells[TableDefine.gsDef_DtlKeyField].Value.ToString();

            string sType = "";

            string sEnabled = "";

            if (sender == btnDetailDisabled)
            {
                sType = btnDetailDisabled.Text;

                sEnabled = "N";
            }
            else if (sender == btnDetailEnabled)
            {
                sType = btnDetailEnabled.Text;

                sEnabled = "Y";
            }

            string sData = gvDetail.Columns[TableDefine.gsDef_DtlKeyData].HeaderText + " : " + gvDetail.CurrentRow.Cells[TableDefine.gsDef_DtlKeyData].Value.ToString();

            string sMsg = sType + " ?" + Environment.NewLine + sData;

            if (SajetCommon.Show_Message(sMsg, 2) != DialogResult.Yes)
            {
                return;
            }

            sSQL = $@"
UPDATE
    {TableDefine.gsDef_DtlTable}
SET
    ENABLED = :ENABLED
   ,UPDATE_USERID = :UPDATE_USERID
   ,UPDATE_TIME = SYSDATE
WHERE
    {TableDefine.gsDef_DtlKeyField} = :SID
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "ENABLED", sEnabled },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "SID", sID }
            };

            ClientUtils.ExecuteSQL(sSQL, p.ToArray());

            CopyToDetailHistory(sID);

            ShowDetailData();

            SetSelectRow(gvDetail, "", TableDefine.gsDef_DtlKeyField);
        }

        private void BtnDetailDelete_Click(object sender, EventArgs e)
        {
            if (gvDetail.Rows.Count == 0 || gvDetail.CurrentRow == null)
            {
                return;
            }

            try
            {
                string sID = gvDetail.CurrentRow.Cells[TableDefine.gsDef_DtlKeyField].Value.ToString();

                string sData = gvDetail.Columns[TableDefine.gsDef_DtlKeyData].HeaderText + " : " + gvDetail.CurrentRow.Cells[TableDefine.gsDef_DtlKeyData].Value.ToString();

                //=====                
                string sMsg = btnDetailDelete.Text + " ?" + Environment.NewLine + sData;

                if (SajetCommon.Show_Message(sMsg, 2) != DialogResult.Yes)
                {
                    return;
                }

                sSQL = $@"
UPDATE
    {TableDefine.gsDef_DtlTable}
SET
    ENABLED = 'Drop'
   ,UPDATE_USERID = :UPDATE_USERID
   ,UPDATE_TIME = SYSDATE
WHERE
    {TableDefine.gsDef_DtlKeyField} = :SID
";
                var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "SID", sID }
                };

                dsTemp = ClientUtils.ExecuteSQL(sSQL, p.ToArray());

                CopyToDetailHistory(sID);

                ShowDetailData();

                SetSelectRow(gvDetail, "", TableDefine.gsDef_DtlKeyField);
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception:" + ex.Message, 0);
            }
        }

        private void CombDetailShow_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDetailDelete.Visible = (combDetailShow.SelectedIndex == 1);

            btnDetailDisabled.Visible = (combDetailShow.SelectedIndex == 0);

            btnDetailEnabled.Visible = (combDetailShow.SelectedIndex == 1);

            ShowDetailData();

            SetSelectRow(gvDetail, "", TableDefine.gsDef_DtlKeyField);
        }

        private void GvDetail_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            e.Value = memoryCacheDetail.RetrieveElement(e.RowIndex, e.ColumnIndex);
        }

        private void GvDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex > -1)
            {
                g_sOrderDetailField = gvDetail.Columns[e.ColumnIndex].Name;

                ShowDetailData();

                SetSelectRow(gvDetail, "", TableDefine.gsDef_DtlKeyField);
            }
        }

        #endregion

        #region 方法

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

        private void Check_Privilege()
        {
            string sPrivilege = ClientUtils.GetPrivilege(g_sUserID, g_sFunction, g_sProgram).ToString();

            btnAppend.Enabled = SajetCommon.CheckEnabled("INSERT", sPrivilege);

            btnModify.Enabled = SajetCommon.CheckEnabled("UPDATE", sPrivilege);

            btnEnabled.Enabled = SajetCommon.CheckEnabled("ENABLED", sPrivilege);

            btnDisabled.Enabled = SajetCommon.CheckEnabled("DISABLED", sPrivilege);

            btnDelete.Enabled = SajetCommon.CheckEnabled("DELETE", sPrivilege);

            btnDetailAppend.Enabled = btnAppend.Enabled;

            btnDetailModify.Enabled = btnModify.Enabled;

            btnDetailEnabled.Enabled = btnEnabled.Enabled;

            btnDetailDisabled.Enabled = btnDisabled.Enabled;

            btnDetailDelete.Enabled = btnDelete.Enabled;
        }

        private void LoadComboBoxItems(string comboBoxName)
        {
            switch (comboBoxName)
            {
                case nameof(combFilter):
                    {
                        var items = new List<ComboBoxItem>
                        {
                            new ComboBoxItem(text: SajetCommon.SetLanguage(nameof(DownTypeColumnsEnum.TypeCode), 1), value: nameof(DownTypeColumnsEnum.TypeCode)),
                            new ComboBoxItem(text: SajetCommon.SetLanguage(nameof(DownTypeColumnsEnum.TypeDesc), 1), value: nameof(DownTypeColumnsEnum.TypeDesc)),
                        };

                        combFilter.Items.AddRange(items.ToArray());

                        break;
                    }
                case nameof(combDetailFilter):
                    {
                        var items = new List<ComboBoxItem>
                        {
                            new ComboBoxItem(text: SajetCommon.SetLanguage(nameof(DownDetailColumnsEnum.ReasonCode), 1), value: nameof(DownDetailColumnsEnum.ReasonCode)),
                            new ComboBoxItem(text: SajetCommon.SetLanguage(nameof(DownDetailColumnsEnum.ReasonDesc), 1), value: nameof(DownDetailColumnsEnum.ReasonDesc)),
                        };

                        combDetailFilter.Items.AddRange(items.ToArray());

                        break;
                    }
                case nameof(combShow):
                    {
                        var items = new List<ComboBoxItem>
                        {
                            new ComboBoxItem(text: SajetCommon.SetLanguage(nameof(StatusEnum.Enabled), 1), value: nameof(StatusEnum.Enabled)),
                            new ComboBoxItem(text: SajetCommon.SetLanguage(nameof(StatusEnum.Disabled), 1), value: nameof(StatusEnum.Disabled)),
                            new ComboBoxItem(text: SajetCommon.SetLanguage(nameof(StatusEnum.All), 1), value: nameof(StatusEnum.All))
                        };

                        combShow.Items.AddRange(items.ToArray());

                        break;
                    }
                case nameof(combDetailShow):
                    {
                        var items = new List<ComboBoxItem>
                        {
                            new ComboBoxItem(text: SajetCommon.SetLanguage(nameof(StatusEnum.Enabled), 1), value: nameof(StatusEnum.Enabled)),
                            new ComboBoxItem(text: SajetCommon.SetLanguage(nameof(StatusEnum.Disabled), 1), value: nameof(StatusEnum.Disabled)),
                            new ComboBoxItem(text: SajetCommon.SetLanguage(nameof(StatusEnum.All), 1), value: nameof(StatusEnum.All))
                        };

                        combDetailShow.Items.AddRange(items.ToArray());

                        break;
                    }
                default:
                    break;
            }
        }

        public void ShowData()
        {
            sSQL = $" SELECT * FROM {TableDefine.gsDef_Table} ";

            if (combShow.SelectedIndex == 0)
            {
                sSQL += " WHERE ENABLED = 'Y' ";
            }
            else if (combShow.SelectedIndex == 1)
            {
                sSQL += " WHERE ENABLED = 'N' ";
            }
            else
            {
                sSQL += " WHERE ENABLED IN ('Y', 'N') ";
            }

            if (combFilter.SelectedIndex > -1 && editFilter.Text.Trim() != "")
            {
                string sFieldName = ((ComboBoxItem)combFilter.SelectedItem).Text;

                if (combShow.SelectedIndex <= 1)
                {
                    sSQL += " AND ";
                }
                else
                {
                    sSQL += " WHERE ";
                }

                sSQL += $" {sFieldName} LIKE '{editFilter.Text.Trim()}%' ";
            }

            sSQL += $" ORDER BY {g_sOrderField} ";

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
        }

        public void ShowDetailData()
        {
            gvDetail.Rows.Clear();

            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
            {
                return;
            }

            string sStageID = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();

            sSQL = $@"
SELECT
    a.reason_id,
    a.reason_code,
    a.reason_desc,
    a.desc2,
    CASE a.count_worktime
        WHEN 0 THEN
            '{sYes}'
        ELSE
            '{sNo}'
    END count_worktime,
    b.type_code,
    b.type_desc
FROM
    {TableDefine.gsDef_DtlTable} A
   ,{TableDefine.gsDef_Table} B
WHERE
    A.{TableDefine.gsDef_KeyField} = B.{TableDefine.gsDef_KeyField}(+)
AND A.{TableDefine.gsDef_KeyField} = '{sStageID}'
";

            if (combDetailShow.SelectedIndex == 0)
            {
                sSQL += " and a.Enabled = 'Y' ";
            }
            else if (combDetailShow.SelectedIndex == 1)
            {
                sSQL += " and a.Enabled = 'N' ";
            }
            else
            {
                sSQL += " and a.Enabled in ('Y', 'N')";
            }

            if (combDetailFilter.SelectedIndex > -1 && editDetailFilter.Text.Trim() != "")
            {
                string sFieldName = ((ComboBoxItem)combDetailFilter.SelectedItem).Text;

                sSQL += " and " + sFieldName + " like '" + editDetailFilter.Text.Trim() + "%'";
            }

            sSQL += " order by " + g_sOrderDetailField;

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

                    gvDetail.Columns[sGridField].Visible = true;
                }
            }

            gvDetail.Focus();
        }

        public static void CopyToHistory(string sID)
        {
            string sSQL = $@"
INSERT INTO
    {TableDefine.gsDef_HTTable}
SELECT *
FROM
    {TableDefine.gsDef_Table}
WHERE
    {TableDefine.gsDef_KeyField} = '{sID}'
";
            ClientUtils.ExecuteSQL(sSQL);
        }

        public static void CopyToDetailHistory(string sID)
        {
            string sSQL = $@"
INSERT INTO
    {TableDefine.gsDef_DtlHTTable}
SELECT *
FROM
    {TableDefine.gsDef_DtlTable}
WHERE
    {TableDefine.gsDef_DtlKeyField} = '{sID}'
";
            ClientUtils.ExecuteSQL(sSQL);
        }

        private void SetSelectRow(DataGridView GridData, string sPrimaryKey, string sField)
        {
            if (GridData.Rows.Count > 0)
            {
                int iIndex = 0;

                if (!string.IsNullOrEmpty(sPrimaryKey))
                {
                    string sCondition = "";

                    string[] tsField = sField.Split(',');

                    string[] tsValue = sPrimaryKey.Split(',');

                    for (int j = 0; j <= tsField.Length - 1; j++)
                    {
                        if (j == 0)
                        {
                            sCondition = $" WHERE {tsField[j]} = '{tsValue[j]}' ";
                        }
                        else
                        {
                            sCondition += $" AND {tsField[j]} = '{tsValue[j]}' ";
                        }

                    }

                    string sDataSQL = g_sDataSQL;

                    if (GridData != gvData)
                    {
                        sDataSQL = g_sDetailDataSQL;
                    }

                    //改用SQL找,不由Grid讀值,否則速度會慢
                    string sText = $@"
SELECT
    IDX
FROM
(
    SELECT
        AA.*
       ,ROWNUM-1 IDX
    FROM
    (
        {sDataSQL}
    ) AA
)
{sCondition}
";
                    DataSet ds = ClientUtils.ExecuteSQL(sText);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        iIndex = Convert.ToInt32(ds.Tables[0].Rows[0]["IDX"].ToString());
                    }
                }

                GridData.Focus();

                GridData.CurrentCell = GridData.Rows[iIndex].Cells[GridData.FirstDisplayedCell.ColumnIndex];

                GridData.Rows[iIndex].Selected = true;
            }
        }
        #endregion
    }
}