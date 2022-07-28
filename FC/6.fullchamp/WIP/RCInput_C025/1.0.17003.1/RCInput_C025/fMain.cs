using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Data.OracleClient;
using System.Text.RegularExpressions;//導入命名空間(正則表達式)
using System.Linq;
using RCInput_C025.Models;
using RCInput_C025.Enums;

namespace RCInput_C025
{
    public partial class fMain : Form
    {
        public string g_SystemType; // Y: Lot Control, N:Piece

        string m_nParam;

        DateTime IN_PROCESS_TIME;   // RCTravel 顯示的時間在sys_base的Closing Date設定

        bool usingT4OrT6stove = false;

        List<string> DateGridViewComboBoxItems = new List<string>();

        struct TProgramInfo
        {
            public string sProgram;
            public string sFunction;
            public Dictionary<string, string> sSQL;
            public Dictionary<string, int> iInputVisible; // 顯示到第幾個欄位;
            public Dictionary<string, List<int>> iInputField; // 那些欄位可以維護(逗號分隔)
            public DataSet dsRC;
            public DataSet dsSNParam;
            public int[] iSNInput;
        }

        TProgramInfo programInfo;

        public struct TControlData
        {
            public string sFieldName;
            public Label lablControl;
            public TextBox txtControl;
        }

        TControlData[] m_tControlData;

        public fMain()
        {
            InitializeComponent();

            toolStripSeparator2.Visible = false;

            statusStrip1.Visible = false;

            LbMachineErrorMsg.Text = "";

            this.Load += FMain_Load;

            this.Shown += FMain_Shown;

            gvMachine.VisibleChanged += GvMachine_VisibleChanged;

            gvMachine.EditingControlShowing += GvMachine_EditingControlShowing;

            TbFindMachine.KeyPress += TbFindMachine_KeyPress;

            BtnFindMachine.Click += BtnFindMachine_Click;

            TcData.SelectedIndexChanged += TcData_SelectedIndexChanged;

            #region 報工時間

            DtpWipIn.DropDown += DtpWipIn_DropDown;

            DtpWipIn.ValueChanged += DtpWipIn_ValueChanged;

            CbReportTime.CheckStateChanged += CbReportTime_CheckStateChanged;

            #endregion

            Tsb_OK.Click += Tsb_OK_Click;

            txtInput.KeyPress += TxtInput_KeyPress;
        }

        private void FMain_Load(object sender, EventArgs e)
        {
            programInfo.iSNInput = new int[2];

            if (string.IsNullOrEmpty(tsEmp.Text))
            {
                tsEmp.Text = ClientUtils.fLoginUser;

                programInfo.sProgram = ClientUtils.fProgramName;

                programInfo.sFunction = ClientUtils.fFunctionName;

                BtnCancel.Visible = false;
            }
            else
            {
                programInfo.sProgram = ClientUtils.fProgramName;

                programInfo.sFunction = m_nParam; // ClientUtils.fFunctionName;
            }

            programInfo.sSQL = new Dictionary<string, string>();

            programInfo.iInputField = new Dictionary<string, List<int>>();

            programInfo.iInputVisible = new Dictionary<string, int>();

            string sSQL = "SELECT * FROM SAJET.SYS_BASE_PARAM WHERE PROGRAM = :PROGRAM AND PARAM_NAME = :PARAM_NAME ORDER BY PARAM_TYPE, DEFAULT_VALUE";

            var Params = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PROGRAM", programInfo.sProgram },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PARAM_NAME", "RC Input" }
            };

            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params.ToArray());

            int iField = 1, iLabel = 0;

            int iFieldCount = 0, iLabelCount = 0, iRowCount = 0;

            foreach (DataRow dr in dsTemp.Tables[0].Rows)
            {
                switch (dr["PARAM_TYPE"].ToString())
                {
                    case "FIELD":
                        {
                            int iCount = int.Parse(dr["DEFAULT_VALUE"].ToString());

                            #region 接管流程卡資訊區塊的顯示項目

                            string[] slValue;

                            if (iCount == 0)
                            {
                                slValue
                                    = ("RC_NO,WORK_ORDER,PART_NO,VERSION,FORMER_NO,SPEC1")
                                    .Split(',');
                            }
                            else if (iCount == 1)
                            {
                                slValue
                                    = ("QTY,ROUTE_NAME,FACTORY_CODE,PROCESS_NAME,BLUEPRINT,SPEC2")
                                    .Split(',');
                            }
                            else
                            {
                                slValue = dr["PARAM_VALUE"].ToString().Split(',');
                            }

                            #endregion

                            if (m_tControlData == null)
                            {
                                m_tControlData = new TControlData[slValue.Length];
                            }
                            else
                            {
                                Array.Resize(ref m_tControlData, m_tControlData.Length + slValue.Length);
                            }

                            TextBox txtTemp;

                            int iRow = 0, itableCount = tableLayoutPanel1.RowCount;

                            iRowCount = slValue.Length;

                            if (int.Parse(dr["DEFAULT_VALUE"].ToString()) > 1)
                            {
                                tableLayoutPanel1.ColumnCount += 2;

                                for (int i = 2; i < tableLayoutPanel1.ColumnCount; i += 2)
                                {
                                    if (i == tableLayoutPanel1.ColumnCount - 1)
                                    {
                                        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

                                        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100 / iCount));
                                    }
                                    else
                                    {
                                        tableLayoutPanel1.ColumnStyles[i].Width = 100 / iCount;
                                    }
                                }
                            }

                            iRow = 0;

                            if (iRowCount > itableCount)
                            {
                                tableLayoutPanel1.RowCount = iRowCount + 1;

                                int i_row_height = 28;

                                int i_table_height = i_row_height * iRowCount + 10;

                                tableLayoutPanel2.Height = i_table_height;

                                tableLayoutPanel1.Height = i_table_height;

                                tableLayoutPanel1.RowStyles[0].Height = i_row_height;

                                for (int i = itableCount; i < slValue.Length; i++)
                                {
                                    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, i_row_height));
                                }
                            }

                            List<string> slEdit = new List<string>();

                            slEdit.AddRange(dr["PARAM_DESC"].ToString().Split(','));

                            for (int i = 0; i < slValue.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(slValue[i]))
                                {
                                    txtTemp = new TextBox
                                    {
                                        ForeColor = Color.Navy,
                                        Dock = DockStyle.Fill,
                                        Name = "txt" + slValue[i],
                                        Font = new Font(this.Font, FontStyle.Bold),
                                    };

                                    if (slEdit.IndexOf(slValue[i]) == -1)
                                    {
                                        txtTemp.ReadOnly = true;
                                    }
                                    else
                                    {
                                        txtTemp.BackColor = Color.FromArgb(255, 255, 192);
                                    }

                                    tableLayoutPanel1.Controls.Add(txtTemp, iField, iRow);

                                    m_tControlData[iFieldCount].txtControl = txtTemp;
                                }

                                m_tControlData[iFieldCount].sFieldName = slValue[i];

                                iRow++;

                                iFieldCount++;
                            }

                            iField += 2;

                            break;
                        }
                    case "LABEL":
                        {
                            int iRow = 0;

                            int iCount = int.Parse(dr["DEFAULT_VALUE"].ToString());

                            #region 接管流程卡資訊區塊的顯示項目

                            string[] slValue;

                            if (iCount == 0)
                            {
                                slValue
                                    = ("流程卡,工單,料號,版本,舊編,品名")
                                    .Split(',');
                            }
                            else if (iCount == 1)
                            {
                                slValue
                                    = ("流程卡數量,途程名稱,廠別代碼,製程名稱,藍圖,規格")
                                    .Split(',');
                            }
                            else
                            {
                                slValue = dr["PARAM_VALUE"].ToString().Split(',');
                            }

                            #endregion

                            for (int i = 0; i < slValue.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(slValue[i]))
                                {
                                    Label lablTemp = new Label()
                                    {
                                        Font = new Font(this.Font, FontStyle.Bold),
                                        Text = slValue[i],
                                        TextAlign = ContentAlignment.MiddleLeft,
                                        Dock = DockStyle.Fill,
                                    };

                                    tableLayoutPanel1.Controls.Add(lablTemp, iLabel, iRow);

                                    if (string.IsNullOrEmpty(m_tControlData[iLabelCount].sFieldName))
                                    {
                                        lablTemp.ForeColor = Color.Maroon;

                                        tableLayoutPanel1.SetColumnSpan(lablTemp, 2);
                                    }
                                    else
                                    {
                                        m_tControlData[iLabelCount].lablControl = lablTemp;
                                    }
                                }

                                iRow++;

                                iLabelCount++;
                            }

                            iLabel += 2;

                            break;
                        }
                    case "SQL":
                        {
                            programInfo.sSQL.Add(dr["DEFAULT_VALUE"].ToString(), dr["PARAM_VALUE"].ToString());

                            if (!string.IsNullOrEmpty(dr["PARAM_DESC"].ToString()))
                            {
                                string[] sSplit = dr["PARAM_DESC"].ToString().Split(';');

                                programInfo.iInputVisible.Add(dr["DEFAULT_VALUE"].ToString(), int.Parse(sSplit[0]));

                                if (sSplit.Length == 2 && !string.IsNullOrEmpty(sSplit[1]))
                                {
                                    sSplit = sSplit[1].Split(',');

                                    programInfo.iInputField.Add(dr["DEFAULT_VALUE"].ToString(), new List<int>());

                                    foreach (string sValue in sSplit)
                                    {
                                        programInfo.iInputField[dr["DEFAULT_VALUE"].ToString()].Add(int.Parse(sValue));
                                    }
                                }
                            }

                            break;
                        }
                }
            }

            SajetCommon.SetLanguageControl(this);

            // Depend system type to show tabpage
            Services.SystemType(system_type: out g_SystemType);
        }

        private void FMain_Shown(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(m_nParam))
            {
                txtInput.Focus();
            }

            #region 強調顯示的資訊

            tableLayoutPanel1.RowStyles[4].Height = 30;

            var new_font_1 = new Font("新細明體", 20, FontStyle.Bold);

            var new_font_2 = new Font("新細明體", 12, FontStyle.Bold);

            TControlData process_name_control_set =
                m_tControlData
                .First(x => x.sFieldName == "PROCESS_NAME");

            process_name_control_set.txtControl.BackColor = Color.Khaki;
            process_name_control_set.lablControl.ForeColor = Color.Red;

            //process_name_control_set.txtControl.Font = new_font_2;
            //process_name_control_set.lablControl.Font = new_font_2;

            TControlData former_part_no_control_set =
                m_tControlData
                .First(x => x.sFieldName == "FORMER_NO");

            former_part_no_control_set.txtControl.BackColor = Color.Khaki;
            former_part_no_control_set.lablControl.ForeColor = Color.Red;

            former_part_no_control_set.txtControl.Font = new_font_2;
            former_part_no_control_set.lablControl.Font = new_font_2;

            TControlData blueprint_control_set =
                m_tControlData
                .First(x => x.sFieldName == "BLUEPRINT");

            blueprint_control_set.txtControl.BackColor = Color.Khaki;
            blueprint_control_set.lablControl.ForeColor = Color.Red;

            blueprint_control_set.txtControl.Font = new_font_2;
            blueprint_control_set.lablControl.Font = new_font_2;

            this.Text += $" ({SajetCommon.g_sFileName} : {SajetCommon.g_sFileVersion})";

            #endregion

            #region 調整 Label 寬度

            if (tableLayoutPanel1.ColumnCount == 4)
            {
                float[] f_label_width = new float[2] { 0f, 0f };

                int i_half = m_tControlData.Length / 2;

                for (int i = 0; i < m_tControlData.Length; i++)
                {
                    int i_count = 0;

                    if (i >= i_half)
                    {
                        i_count = 1;
                    }

                    Label label = m_tControlData[i].lablControl;

                    SizeF size = label.CreateGraphics().MeasureString(label.Text, label.Font);

                    if (size.Width > f_label_width[i_count])
                    {
                        f_label_width[i_count] = size.Width;
                    }
                }

                // 嘗試調整流程卡資訊區塊中，兩個標題列的寬度
                if (tableLayoutPanel1.ColumnCount == 4)
                {
                    int i_default_width = 25;
                    tableLayoutPanel1.ColumnStyles.Clear();
                    tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, f_label_width[0] + i_default_width));
                    tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                    tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, f_label_width[1] + i_default_width));
                    tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                }

            }

            #endregion

            #region 調整一些可能擋到字的項目

            int i_position_1 = 0;
            int i_position_2 = 0;
            int i_default_distance = 10;

            // 報工時間頁籤
            {
                i_position_1 = label4.Right;

                if (CbReportTime.Right > i_position_1)
                {
                    i_position_1 = CbReportTime.Right;
                }

                i_position_2 = LbLastReportTime.Left;

                if (DtpWipIn.Left > i_position_2)
                {
                    i_position_2 = DtpWipIn.Left;
                }

                if (i_position_1 > i_position_2)
                {
                    LbLastReportTime.Left = i_position_1 + i_default_distance;

                    DtpWipIn.Left = i_position_1 + i_default_distance;
                }
            }

            #endregion
        }

        /// <summary>
        /// 刷入流程卡，點擊 OK 按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tsb_OK_Click(object sender, EventArgs e)
        {
            // 檢查流程卡資訊

            CheckInput();

            #region 自訂報工投入時間

            IN_PROCESS_TIME = Services.GetDBDateTimeNow();

            DtpWipIn.Value = IN_PROCESS_TIME;

            LbLastReportTime.Text = Services.GetLastReportTime(Runcard: txtInput.Text.Trim());

            bool CanSetWipTime = Services.Check_Privilege(EmpID: Services.GetEmpID(tsEmp.Text), fun: "Set_WIP_Time");

            if (CanSetWipTime)
            {
                TpInTime.Parent = TcData;
            }
            else
            {
                TpInTime.Parent = null;
            }

            #endregion

            txtInput.SelectAll();
        }

        /// <summary>
        /// 刷入流程卡，點擊 Enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
            {
                return;
            }

            Tsb_OK_Click(null, null);
        }

        private void TcData_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TcData.SelectedTab == TpMachine)
            {
                TbFindMachine.Focus();
            }
        }

        private void GvInput_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell cell = gvInput.Rows[e.RowIndex].Cells[e.ColumnIndex];

            cell.ErrorText = "";

            switch (gvInput.Rows[e.RowIndex].Cells["CONVERT_TYPE"].EditedFormattedValue.ToString())
            {
                case "U":
                    {
                        cell.Value = cell.EditedFormattedValue.ToString().ToUpper();

                        break;
                    }
                case "L":
                    {
                        cell.Value = cell.EditedFormattedValue.ToString().ToUpper();

                        break;
                    }
            }

            switch (gvInput.Rows[e.RowIndex].Cells["INPUT_TYPE"].EditedFormattedValue.ToString())
            {
                case "R":
                    {
                        string[] slValue = gvInput.Rows[e.RowIndex].Cells["VALUE_LIST"].EditedFormattedValue.ToString().Split(',');

                        decimal dMin = decimal.Parse(slValue[0]);

                        decimal dMax = decimal.Parse(slValue[1]);

                        try
                        {
                            decimal dValue = decimal.Parse(cell.EditedFormattedValue.ToString());

                            if (dValue >= dMin && dValue <= dMax) { }
                            else
                            {
                                cell.ErrorText = string.Format(SajetCommon.SetLanguage("Over flow{0}~{1}", 1), dMin, dMax);
                            }
                        }
                        catch
                        {
                            if (gvInput.Rows[e.RowIndex].Cells["NECESSARY"].EditedFormattedValue.ToString() == "Y")
                            {
                                cell.ErrorText = SajetCommon.SetLanguage("Data Invalid", 1);
                            }
                        }
                        break;
                    }
                default:
                    {
                        if (gvInput.Rows[e.RowIndex].Cells["VALUE_TYPE"].EditedFormattedValue.ToString() == "N")
                        {
                            try
                            {
                                decimal dValue = decimal.Parse(cell.EditedFormattedValue.ToString());
                            }
                            catch
                            {
                                if (gvInput.Rows[e.RowIndex].Cells["NECESSARY"].EditedFormattedValue.ToString() == "Y")
                                {
                                    cell.ErrorText = SajetCommon.SetLanguage("Data Invalid", 1);
                                }
                            }
                        }
                        break;
                    }
            }

            if (string.IsNullOrEmpty(cell.ErrorText) &&
                gvInput.Rows[e.RowIndex].Cells["NECESSARY"].EditedFormattedValue.ToString() == "Y" &&
                string.IsNullOrEmpty(cell.EditedFormattedValue.ToString()))
            {
                cell.ErrorText = SajetCommon.SetLanguage("Data Empty", 1);
            }
        }

        private void GvCondition_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow r in gvCondition.Rows)
            {
                if (r.Cells["VALUE_TYPE"].Value.ToString() == "L" &&
                    !System.Uri.IsWellFormedUriString(r.Cells["VALUE_TYPE"].Value.ToString(), UriKind.Absolute))
                {
                    r.Cells["VALUE_DEFAULT"] = new DataGridViewLinkCell();
                }
            }
        }

        private void GvCondition_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            /* 確認欄位型態 */
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            if (gvCondition.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewLinkCell)
            {
                string url = gvCondition.Rows[e.RowIndex].Cells["VALUE_DEFAULT"].Value.ToString();

                System.Diagnostics.Process.Start(url);
            }
        }

        private void GvMachine_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex == gvMachine.Columns["CHECKED"].DisplayIndex)
            {
                bool check = (bool)gvMachine.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue;

                gvMachine.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = !check;
            }
        }

        private void GvMachine_VisibleChanged(object sender, EventArgs e)
        {
            if (gvMachine.Visible)
            {
                gvMachine.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
        }

        private void GvMachine_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= DgvMachineTextBoxColumn_KeyPress;

            e.Control.Leave -= DgvMachineTextBoxColumn_Leave;

            e.Control.Leave -= DgvMachineComboBoxColumn_Leave;

            if (gvMachine.CurrentCell.ColumnIndex == gvMachine.Columns["DATE_CODE"].Index ||
                gvMachine.CurrentCell.ColumnIndex == gvMachine.Columns["LOAD_QTY"].Index ||
                gvMachine.CurrentCell.ColumnIndex == gvMachine.Columns["STOVE_SEQ"].Index)
            {
                if (e.Control is TextBox x)
                {
                    x.KeyPress += DgvMachineTextBoxColumn_KeyPress;

                    x.Leave += DgvMachineTextBoxColumn_Leave;
                }

                if (e.Control is ComboBox z)
                {
                    z.Leave += DgvMachineComboBoxColumn_Leave;
                }
            }
        }

        /// <summary>
        /// 只能輸入正整數
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvMachineTextBoxColumn_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// 結束編輯狀態時的資料驗證
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvMachineTextBoxColumn_Leave(object sender, EventArgs e)
        {
            if (sender is DataGridViewTextBoxEditingControl x)
            {
                if (gvMachine.CurrentCell.ColumnIndex == gvMachine.Columns["LOAD_QTY"].Index)
                {
                    if (string.IsNullOrWhiteSpace(x.Text))
                    {
                        x.Text = "0";
                    }
                    else
                    {
                        gvMachine.CurrentRow.Cells["CHECKED"].Value = true;
                    }
                }

                if (gvMachine.CurrentCell.ColumnIndex == gvMachine.Columns["DATE_CODE"].Index)
                {
                    if (!string.IsNullOrWhiteSpace(x.Text))
                    {
                        gvMachine.CurrentRow.Cells["CHECKED"].Value = true;
                    }
                }

            }
        }

        private void DgvMachineComboBoxColumn_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(gvMachine.CurrentRow.Cells["STOVE_SEQ"].Value.ToString()))
            {
                gvMachine.CurrentRow.Cells["CHECKED"].Value = true;
            }
        }

        private void TbFindMachine_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter &&
                !string.IsNullOrWhiteSpace(TbFindMachine.Text) &&
                gvMachine.Rows.Count > 0)
            {
                BtnFindMachine_Click(null, null);
            }
        }

        private void BtnFindMachine_Click(object sender, EventArgs e)
        {
            LbMachineErrorMsg.Text = "";

            if (!string.IsNullOrWhiteSpace(TbFindMachine.Text) &&
                gvMachine.Rows.Count > 0)
            {
                string key_word = TbFindMachine.Text.Trim().ToUpper();

                var row = gvMachine.Rows
                    .Cast<DataGridViewRow>()
                    .DefaultIfEmpty(null)
                    .FirstOrDefault(x => x.Cells["MACHINE_CODE"].Value.ToString().Trim().ToUpper() == key_word);

                if (row != null)
                {
                    row.Cells["CHECKED"].Value = true;

                    gvMachine.CurrentCell = gvMachine[gvMachine.FirstDisplayedCell.ColumnIndex, row.Index];
                }
                else
                {
                    LbMachineErrorMsg.Text = SajetCommon.SetLanguage(MessageEnum.MachineNotFound.ToString());
                }
            }

            TbFindMachine.Text = "";

            TbFindMachine.Focus();
        }

        #region 

        private void DtpWipIn_ValueChanged(object sender, EventArgs e)
        {
            CbReportTime.CheckStateChanged -= CbReportTime_CheckStateChanged;

            CbReportTime.Checked = true;

            CbReportTime.CheckStateChanged += CbReportTime_CheckStateChanged;
        }

        private void DtpWipIn_DropDown(object sender, EventArgs e)
        {
            CbReportTime.CheckStateChanged -= CbReportTime_CheckStateChanged;

            CbReportTime.Checked = true;

            CbReportTime.CheckStateChanged += CbReportTime_CheckStateChanged;
        }

        private void CbReportTime_CheckStateChanged(object sender, EventArgs e)
        {
            DtpWipIn.ValueChanged -= DtpWipIn_ValueChanged;

            if (!CbReportTime.Checked)
            {
                DtpWipIn.Value = IN_PROCESS_TIME;
            }

            DtpWipIn.ValueChanged += DtpWipIn_ValueChanged;
        }

        #endregion

        private void BtnOK_Click(object sender, EventArgs e)
        {
            string sInput = string.Empty;

            string message = string.Empty;

            #region G_RC_TRAVEL_MACHINE_DOWN

            var RcInfo = Services.GetRcInfo(txtInput.Text);

            #endregion

            # region Parameter Collection 

            // 檢查資料收集是否有輸入
            foreach (DataGridViewRow dr in gvInput.Rows)
            {
                if (!string.IsNullOrEmpty(dr.Cells["VALUE_DEFAULT"].ErrorText))
                {
                    SajetCommon.Show_Message(dr.Cells["ITEM_NAME"].EditedFormattedValue.ToString() + dr.Cells["VALUE_DEFAULT"].ErrorText, 0);

                    gvInput.CurrentCell = dr.Cells["VALUE_DEFAULT"];

                    gvInput.Focus();

                    TcData.SelectedIndex = 0;

                    return;
                }

                if (!string.IsNullOrEmpty(dr.Cells["VALUE_DEFAULT"].EditedFormattedValue.ToString()))
                {
                    sInput += dr.Cells["ITEM_ID"].EditedFormattedValue.ToString() + (char)9 + dr.Cells["VALUE_DEFAULT"].EditedFormattedValue.ToString() + (char)27;
                }
            }

            #endregion

            #region 加工數量量與日期碼、爐序號

            var qtyNotPass = new List<string>();

            var dateCodeNotPass = new List<string>();

            var notCollectTwo = new List<string>();

            foreach (DataGridViewRow row in gvMachine.Rows)
            {
                bool bCheck = (Convert.ToBoolean(row.Cells["CHECKED"].EditedFormattedValue));

                string machineCode = row.Cells["MACHINE_CODE"].Value.ToString();

                if (bCheck)
                {
                    string loadQty = row.Cells["LOAD_QTY"].EditedFormattedValue?.ToString() ?? "";

                    string dateCode = row.Cells["DATE_CODE"].EditedFormattedValue?.ToString() ?? "";

                    string stoveSeq = row.Cells["STOVE_SEQ"].EditedFormattedValue?.ToString() ?? "";

                    if (!string.IsNullOrWhiteSpace(loadQty) &&
                        (!int.TryParse(loadQty, out int load) || load < 0))
                    {
                        qtyNotPass.Add(machineCode);
                    }

                    if (!string.IsNullOrWhiteSpace(dateCode))
                    {
                        if (dateCode.Length == 6 || dateCode.Length == 8)
                        {
                            // 6 碼檢查
                            if (dateCode.Length == 6
                                && !DateTime.TryParseExact(dateCode, "yyMMdd", null,
                                System.Globalization.DateTimeStyles.None, out DateTime d1))
                            {
                                dateCodeNotPass.Add(machineCode);
                            }
                            // 8 碼檢查
                            else if (dateCode.Length == 8
                                && !DateTime.TryParseExact(dateCode, "yyyyMMdd", null,
                                System.Globalization.DateTimeStyles.None, out DateTime d2))
                            {
                                dateCodeNotPass.Add(machineCode);
                            }
                        }
                        // 都不符合
                        else
                        {
                            dateCodeNotPass.Add(machineCode);
                        }
                    }

                    if (string.IsNullOrWhiteSpace(stoveSeq) ^ string.IsNullOrWhiteSpace(dateCode))
                    {
                        notCollectTwo.Add(machineCode);
                    }
                }
            }

            if (dateCodeNotPass.Any())
            {
                message
                    = SajetCommon.SetLanguage("Invalid DateCode format")
                    + Environment.NewLine
                    + SajetCommon.SetLanguage("Require 6 or 8 digits DateTime format")
                    + Environment.NewLine
                    + SajetCommon.SetLanguage("Machine")
                    + ": "
                    + string.Join(", ", dateCodeNotPass)
                    ;

                SajetCommon.Show_Message(message, 0);

                return;
            }

            if (qtyNotPass.Any())
            {
                message = SajetCommon.SetLanguage("The load of the machine must be a non-negative number");

                SajetCommon.Show_Message(message, 0);

                return;
            }

            if (notCollectTwo.Any())
            {
                message
                    = SajetCommon.SetLanguage("DateCode and stove sequence should be collected at the same time if needed")
                    + Environment.NewLine
                    + SajetCommon.SetLanguage("Machine")
                    + ": "
                    + string.Join(", ", notCollectTwo)
                    ;

                SajetCommon.Show_Message(message, 0);

                return;
            }

            #endregion

            #region Process Machine

            string sMachine = string.Empty;

            var machines = new List<MachineModel>();

            if (gvMachine.Rows.Count > 0) // 檢查是否有勾選
            {
                for (int i = 0; i < gvMachine.Rows.Count; i++)
                {
                    bool bCheck = (Convert.ToBoolean(gvMachine.Rows[i].Cells[0].EditedFormattedValue));

                    if (bCheck)
                    {
                        sMachine += gvMachine.Rows[i].Cells["MACHINE_CODE"].EditedFormattedValue.ToString() + (char)9;

                        machines.Add(new MachineModel
                        {
                            MACHINE_ID = int.Parse(gvMachine.Rows[i].Cells["MACHINE_ID"].EditedFormattedValue.ToString()),
                            LOAD_QTY = int.TryParse(gvMachine.Rows[i].Cells["LOAD_QTY"].EditedFormattedValue.ToString(), out int loadQty) ? loadQty : 0,
                            DATE_CODE = gvMachine.Rows[i].Cells["DATE_CODE"].EditedFormattedValue.ToString(),
                            STOVE_SEQ = gvMachine.Rows[i].Cells["STOVE_SEQ"].EditedFormattedValue.ToString(),
                        });
                    }
                }

                if (string.IsNullOrEmpty(sMachine))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Machine Unselect"), 0);

                    TcData.SelectedIndex = 1;

                    return;
                }
            }
            # endregion

            # region Piece

            string sSN = string.Empty;

            # endregion

            # region Keyparts Collection

            string skeypart = string.Empty;

            #endregion

            // v1.0.17003.12
            #region 檢查投入時間

            IN_PROCESS_TIME = Services.GetDBDateTimeNow();

            var time = CbReportTime.Checked ? DtpWipIn.Value : IN_PROCESS_TIME;

            if (!Services.IsInputTimeValid(runcard: RcInfo, InProcessTime: time, message: out message))
            {
                SajetCommon.Show_Message(message, 0);

                return;
            }

            #endregion

            var Params = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TEMP", tsEmp.Text },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TRC", txtInput.Text },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TITEM", sInput },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TMACHINE", sMachine },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TSN", sSN },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TMEMO", tsMemo.Text },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TKEYPART", skeypart },
                new object[] { ParameterDirection.Input, OracleType.DateTime, "TNOW", time },
                new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" }
            };

            DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_RC_INPUT", Params.ToArray());

            string sMsg = ds.Tables[0].Rows[0]["TRES"].ToString();

            if (sMsg == "OK")
            {
                RecordMachine(RcInfo: RcInfo, Machines: machines, now: time);

                if (string.IsNullOrEmpty(m_nParam))
                {
                    ClearData();
                }
                else
                {
                    DialogResult = DialogResult.OK;

                    this.Close();
                }

                tsMsg.ForeColor = Color.FromArgb(0, 0, 192);
            }
            else
            {
                tsMsg.ForeColor = Color.Red;

                SajetCommon.Show_Message(SajetCommon.SetLanguage(sMsg), 0);
            }

            tsMsg.Text = SajetCommon.SetLanguage(ds.Tables[0].Rows[0]["TRES"].ToString(), 1);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void ShowData()
        {
            string sSQL = programInfo.sSQL["SQL"];

            // 接管顯示資訊
            sSQL = @"
SELECT
    A.RC_NO,
    A.WORK_ORDER,
    B.PART_NO,
    B.SPEC1,
    B.SPEC2,
    B.OPTION2       FORMER_NO,
    B.OPTION4       BLUEPRINT,
    C.ROUTE_NAME,
    D.PROCESS_NAME,
    E.FACTORY_CODE,
    A.CURRENT_QTY   QTY,
    A.VERSION,
    A.PROCESS_ID,
    A.PART_ID,
    F.WO_OPTION2
FROM
    SAJET.G_RC_STATUS    A,
    SAJET.SYS_PART       B,
    SAJET.SYS_RC_ROUTE   C,
    SAJET.SYS_PROCESS    D,
    SAJET.SYS_FACTORY    E,
    SAJET.G_WO_BASE      F
WHERE
    A.PART_ID = B.PART_ID
    AND A.ROUTE_ID = C.ROUTE_ID
    AND A.PROCESS_ID = D.PROCESS_ID
    AND A.FACTORY_ID = E.FACTORY_ID
    AND A.WORK_ORDER = F.WORK_ORDER
    AND RC_NO = :SN
";

            var Params = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "SN", txtInput.Text }
            };

            try
            {
                programInfo.dsRC = ClientUtils.ExecuteSQL(sSQL, Params.ToArray());

                if (programInfo.dsRC.Tables[0].Rows.Count == 0)
                {
                    //SN錯誤時清空
                    ClearData();
                }
                else
                {
                    for (int i = 0; i < m_tControlData.Length; i++)
                    {
                        if (programInfo.dsRC.Tables[0].Columns.Contains(m_tControlData[i].sFieldName))
                        {
                            m_tControlData[i].txtControl.Text = programInfo.dsRC.Tables[0].Rows[0][m_tControlData[i].sFieldName].ToString();
                        }
                    }

                    DspReportData();
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private void DspReportData()
        {
            # region Process Parameter

            var Params = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", programInfo.dsRC.Tables[0].Rows[0]["PROCESS_ID"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", programInfo.dsRC.Tables[0].Rows[0]["PART_ID"].ToString() }
            };

            DataSet ds = ClientUtils.ExecuteSQL(programInfo.sSQL["製程條件"], Params.ToArray());

            gvCondition.DataSource = ds;

            gvCondition.DataMember = ds.Tables[0].ToString();

            foreach (DataColumn dc in ds.Tables[0].Columns)
            {
                gvCondition.Columns[dc.ColumnName].HeaderText = SajetCommon.SetLanguage(gvCondition.Columns[dc.ColumnName].HeaderText, 1);
            }

            gvCondition.Columns["VALUE_TYPE"].Visible = false;

            ds = ClientUtils.ExecuteSQL(programInfo.sSQL["資料收集"], Params.ToArray());

            if (gvInput.ColumnCount == 0)
            {
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    gvInput.Columns.Add(ds.Tables[0].Columns[i].ColumnName, SajetCommon.SetLanguage(ds.Tables[0].Columns[i].ColumnName, 1));

                    if (i > programInfo.iInputVisible["資料收集"])
                    {
                        gvInput.Columns[i].Visible = false;
                    }
                    else
                    {
                        gvInput.Columns[i].ReadOnly = programInfo.iInputField["資料收集"].IndexOf(i) == -1;
                    }
                }

                gvInput.Columns["UNIT_NO"].Visible = true;

                gvInput.Columns["UNIT_NO"].ReadOnly = true;
            }

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                gvInput.Rows.Add(dr.ItemArray);

                if (dr["INPUT_TYPE"].ToString() == "S")
                {
                    DataGridViewComboBoxCell tCell = new DataGridViewComboBoxCell();

                    string[] slValue = dr["VALUE_LIST"].ToString().Split(',');

                    for (int i = 0; i < slValue.Length - 1; i++)
                    {
                        tCell.Items.Add(slValue[i]);
                    }

                    gvInput["VALUE_DEFAULT", gvInput.Rows.Count - 1] = tCell;
                }

                if (dr["NECESSARY"].ToString() == "Y")
                {
                    gvInput.Rows[gvInput.Rows.Count - 1].Cells["VALUE_DEFAULT"].Style.BackColor = Color.FromArgb(255, 255, 192);

                    gvInput.Rows[gvInput.Rows.Count - 1].Cells["VALUE_DEFAULT"].ErrorText = SajetCommon.SetLanguage("Data Empty", 1);
                }
            }

            foreach (DataGridViewColumn col in gvCondition.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            foreach (DataGridViewColumn col in gvInput.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            # endregion

            # region Process Machine

            Params = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", 100053 }
            };

            string s = @"
SELECT
    b.machine_code,
    b.machine_desc,
    d.status_name,
    d.run_flag,
    d.default_status,
    b.machine_id
FROM
    sajet.sys_rc_process_machine   a,
    sajet.sys_machine              b,
    sajet.g_machine_status         c,
    sajet.sys_machine_status       d
WHERE
    a.process_id = :process_id
    AND a.machine_id = b.machine_id
    AND a.machine_id = c.machine_id
    AND c.current_status_id = d.status_id
    AND a.enabled = 'Y'
    AND b.enabled = 'Y'
ORDER BY
    b.machine_code
";
            //ds = ClientUtils.ExecuteSQL(programInfo.sSQL["機台"], Params.ToArray());
            ds = ClientUtils.ExecuteSQL(s, Params.ToArray());

            if (gvMachine.ColumnCount == 0)
            {
                DataGridViewCheckBoxColumn dcCheck = new DataGridViewCheckBoxColumn
                {
                    HeaderText = SajetCommon.SetLanguage("Select"),
                    Width = 40,
                    Name = "CHECKED",
                    ReadOnly = true,
                };

                gvMachine.Columns.Add(dcCheck);

                gvMachine.CellClick += GvMachine_CellClick;

                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    gvMachine.Columns.Add(ds.Tables[0].Columns[i].ColumnName, SajetCommon.SetLanguage(ds.Tables[0].Columns[i].ColumnName, 1));

                    if (i > programInfo.iInputVisible["機台"])
                    {
                        gvMachine.Columns[gvMachine.Columns.Count - 1].Visible = false;
                    }
                    else
                    {
                        gvMachine.Columns[gvMachine.Columns.Count - 1].ReadOnly = programInfo.iInputField["機台"].IndexOf(i) == -1;
                    }
                }
            }

            // 加入蒐集機台資料的欄位
            if (gvMachine.Columns.Count > 0)
            {
                DataGridViewTextBoxColumn loadQty = new DataGridViewTextBoxColumn
                {
                    HeaderText = SajetCommon.SetLanguage("LOAD_QTY"),
                    Name = "LOAD_QTY",
                    DisplayIndex = gvMachine.Columns.Count,
                    Visible = true,
                    ReadOnly = false,
                };

                gvMachine.Columns.Add(loadQty);

                DataGridViewTextBoxColumn dateCode = new DataGridViewTextBoxColumn
                {
                    HeaderText = SajetCommon.SetLanguage("DATE_CODE"),
                    Name = "DATE_CODE",
                    DisplayIndex = gvMachine.Columns.Count,
                    Visible = usingT4OrT6stove,
                    ReadOnly = false,
                };

                gvMachine.Columns.Add(dateCode);

                DataGridViewComboBoxColumn stoveSeq = new DataGridViewComboBoxColumn
                {
                    HeaderText = SajetCommon.SetLanguage("STOVE_SEQ"),
                    Name = "STOVE_SEQ",
                    DisplayIndex = gvMachine.Columns.Count,
                    Visible = usingT4OrT6stove,
                    ReadOnly = false,
                    DataSource = DateGridViewComboBoxItems,
                };

                gvMachine.Columns.Add(stoveSeq);
            }

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                gvMachine.Rows.Add();

                gvMachine.Rows[gvMachine.Rows.Count - 1].Cells[0].Value = false;

                foreach (DataColumn dc in ds.Tables[0].Columns)
                {
                    gvMachine.Rows[gvMachine.Rows.Count - 1].Cells[dc.ColumnName].Value = dr[dc.ColumnName].ToString();
                }

                if (!(dr["DEFAULT_STATUS"].ToString() == "Y" || dr["RUN_FLAG"].ToString() == "1"))
                {
                    gvMachine.Rows[gvMachine.Rows.Count - 1].ReadOnly = true;

                    gvMachine.Rows[gvMachine.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Silver;
                }
            }

            if (usingT4OrT6stove)
            {
                foreach (DataGridViewRow row in gvMachine.Rows)
                {
                    string seq = row.Cells["STOVE_SEQ"].Value?.ToString();

                    if (string.IsNullOrWhiteSpace(seq))
                    {
                        row.Cells["STOVE_SEQ"].Value = DateGridViewComboBoxItems[0];
                    }
                }
            }

            foreach (DataGridViewColumn col in gvMachine.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            if (gvMachine.Rows.Count == 1)
            {
                gvMachine.Rows[0].Cells["CHECKED"].Value = true;
            }

            #endregion
        }

        private void CheckInput()
        {
            var Params = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TREV", txtInput.Text },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TSHEET", programInfo.sFunction },
                new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" }
            };

            DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_RC_CHK_ROUTE", Params.ToArray());

            if (ds.Tables[0].Rows[0]["TRES"].ToString() != "OK")
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage(ds.Tables[0].Rows[0]["TRES"].ToString()), 0);
            }
            else
            {
                ClearData();

                ShowData();
            }
        }

        /// <summary>
        /// 機台資訊另外抄寫到變更機台記錄表。
        /// </summary>
        /// <param name="RcInfo">流程卡資訊</param>
        /// <param name="Machines">使用機台的清單</param>
        private void RecordMachine(DataRow RcInfo, List<MachineModel> Machines, DateTime now)
        {
            string s = string.Empty;

            int i = 0;

            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", RcInfo["RC_NO"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.Number, "TRAVEL_ID", RcInfo["TRAVEL_ID"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.Number, "NODE_ID", RcInfo["NODE_ID"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.DateTime, "NOW_TIME", now },
                new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", ClientUtils.UserPara1 },
            };

            if (Machines.Any())
            {
                foreach (var machine in Machines)
                {
                    s += $@"
INTO
    SAJET.G_RC_TRAVEL_MACHINE_DOWN
(
    RC_NO
   ,NODE_ID
   ,TRAVEL_ID
   ,MACHINE_ID
   ,START_TIME
   ,REASON_ID
   ,LOAD_QTY
   ,DATE_CODE
   ,STOVE_SEQ
   ,UPDATE_USERID
   ,UPDATE_TIME
   ,WORK_TIME_MINUTE
   ,WORK_TIME_SECOND
   ,DATA_STATUS
   ,REPORT_TYPE
)
VALUES
(
    :RC_NO
   ,:NODE_ID
   ,:TRAVEL_ID
   ,:MACHINE_ID{i}
   ,:NOW_TIME
   ,0
   ,:LOAD_QTY{i}
   ,:DATE_CODE{i}
   ,:STOVE_SEQ{i}
   ,:UPDATE_USERID
   ,:NOW_TIME
   ,0
   ,0
   ,0
   ,1
)
";
                    p.Add(new object[] { ParameterDirection.Input, OracleType.Number, $"MACHINE_ID{i}", machine.MACHINE_ID });
                    p.Add(new object[] { ParameterDirection.Input, OracleType.Number, $"LOAD_QTY{i}", machine.LOAD_QTY });
                    p.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, $"DATE_CODE{i}", machine.DATE_CODE });
                    p.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, $"STOVE_SEQ{i}", machine.STOVE_SEQ });

                    i++;
                }
            }
            else
            {
                s += $@"
INTO
    SAJET.G_RC_TRAVEL_MACHINE_DOWN
(
    RC_NO
   ,NODE_ID
   ,TRAVEL_ID
   ,MACHINE_ID
   ,START_TIME
   ,REASON_ID
   ,LOAD_QTY
   ,UPDATE_USERID
   ,UPDATE_TIME
   ,WORK_TIME_MINUTE
   ,WORK_TIME_SECOND
   ,DATA_STATUS
   ,REPORT_TYPE
)
VALUES
(
    :RC_NO
   ,:NODE_ID
   ,:TRAVEL_ID
   ,0
   ,:NOW_TIME
   ,0
   ,0
   ,:UPDATE_USERID
   ,:NOW_TIME
   ,0
   ,0
   ,0
   ,1
)
";
            }

            s = $@"
INSERT ALL
{s}
SELECT 1 FROM DUAL
";
            ClientUtils.ExecuteSQL(s, p.ToArray());
        }

        private void ClearData()//無該筆SN時清空資料
        {
            for (int i = 0; i < m_tControlData.Length; i++)
            {
                m_tControlData[i].txtControl.Text = string.Empty;
            }

            gvCondition.DataSource = null;

            gvMachine.Rows.Clear();

            gvInput.Rows.Clear();
        }

    }
}