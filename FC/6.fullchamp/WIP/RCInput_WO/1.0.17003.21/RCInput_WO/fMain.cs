using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using System.Text.RegularExpressions;
using SajetClass;
using RCInput_WO.Enums;
using RCInput_WO.Models;
using T4Srv = RCInput_WO.Services.T4Service;
using OtSrv = RCInput_WO.Services.OtherService;
using RcSrv = RCInput_WO.Services.RuncardService;
using McSrv = RCInput_WO.Services.MachineService;
using PlSrv = RCInput_WO.Services.PrintLabelService;

namespace RCInput_WO
{
    public partial class fMain : Form
    {
        string m_nParam;

        /// <summary>
        /// Y: Lot Control, N:Piece
        /// </summary>
        public string g_SystemType;

        TProgramInfo programInfo = new TProgramInfo
        {
            iSNInput = new int[2],
            sFunction = "RC Input",
            sSQL = new Dictionary<string, string>(),
            iInputField = new Dictionary<string, List<int>>(),
            sOption = new Dictionary<string, string>(),
            iInputVisible = new Dictionary<string, int>(),
            slDefect = new Dictionary<string, int>(),
        };

        TControlData[] m_tControlData;

        /// <summary>
        /// 投入製程時間
        /// </summary>
        DateTime IN_PROCESS_TIME;

        public DataSet RCSource;

        /// <summary>
        /// 所有流程卡的資料
        /// </summary>
        public List<RC_DETAILS> RC_Details = new List<RC_DETAILS>();

        /// <summary>
        /// 資料收集清單，含有所有項目與設定，但不含輸入值。
        /// </summary>
        private List<DataCollectModel> CollectionItems = new List<DataCollectModel>();

        /// <summary>
        /// 製程機台清單
        /// </summary>
        private List<MachineDownModel> MachineList = new List<MachineDownModel>();

        List<string> DateGridViewComboBoxItems = new List<string>();

        /// <summary>
        /// 使用者目前選取的流程卡
        /// </summary>
        private RC_DETAILS CurrentRuncard = null;

        private string Current_RC_NO = "";

        private string EmpID = "";

        /// <summary>
        /// 是否共用機台、報工時間等資料設定
        /// </summary>
        private bool ShareSettings = false;

        /// <summary>
        /// 是不是使用到 T4 爐 或 T6 爐的製程。此類製程必須共用機台資料設定
        /// </summary>
        private bool usingT4OrT6stove = false;

        /// <summary>
        /// 是否列印防水標籤
        /// </summary>
        private bool toPrintLabel = false;

        public fMain() : this("", "", "", "") { }
        public fMain(string nParam, string strSN, string sEmp, string sTime)
        {
            InitializeComponent();

            m_nParam = nParam;

            this.Text = m_nParam;

            TstbInput.Text = strSN;

            TstbInput.Visible = false;

            tsEmp.Text = sEmp;

            cmbParam.Visible = false;

            toolStripSeparator2.Visible = false;

            statusStripMessage.Visible = false;

            IN_PROCESS_TIME = OtSrv.GetDBDateTimeNow();

            DtpInDate.Value = IN_PROCESS_TIME;

            TbCheckRC.KeyDown += TbCheckRC_KeyDown;

            #region 載入爐順序號碼的下拉選單

            //DateGridViewComboBoxItems = T4Srv.GetSequencesComboBoxItems();

            //sTOVESEQDataGridViewTextBoxColumn.DataSource = DateGridViewComboBoxItems;

            DgvMachine.EditingControlShowing += DgvMachine_EditingControlShowing;

            #endregion

            #region 報工時間

            DtpInDate.DropDown += DtpInDate_DropDown;

            #endregion
        }

        private void FMain_Load(object sender, EventArgs e)
        {
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

                EmpID = OtSrv.GetEmpID(tsEmp.Text);
            }

            string sSQL = @"
SELECT
    *
FROM
    SAJET.SYS_BASE_PARAM
WHERE
    PROGRAM = :PROGRAM
    AND PARAM_NAME = :PARAM_NAME
ORDER BY
    PARAM_TYPE
   ,DEFAULT_VALUE
";
            var Params = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PROGRAM", programInfo.sProgram },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PARAM_NAME", programInfo.sFunction }
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
                            string[] slValue = dr["PARAM_VALUE"].ToString().Split(',');

                            if (m_tControlData == null)
                            {
                                m_tControlData = new TControlData[slValue.Length];
                            }
                            else
                            {
                                Array.Resize(ref m_tControlData, m_tControlData.Length + slValue.Length);
                            }

                            TextBox txtTemp;

                            int iRow = 0, itableCount = TlpInfo.RowCount;

                            iRowCount = slValue.Length;

                            if (int.Parse(dr["DEFAULT_VALUE"].ToString()) > 1)
                            {
                                int iCount = int.Parse(dr["DEFAULT_VALUE"].ToString());

                                TlpInfo.ColumnCount += 2;

                                for (int i = 2; i < TlpInfo.ColumnCount; i += 2)
                                {
                                    if (i == TlpInfo.ColumnCount - 1)
                                    {
                                        TlpInfo.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

                                        TlpInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100 / iCount));
                                    }
                                    else
                                    {
                                        TlpInfo.ColumnStyles[i].Width = 100 / iCount;
                                    }
                                }
                            }

                            iRow = 0;

                            if (iRowCount > itableCount)
                            {
                                TlpInfo.RowCount = iRowCount;

                                TlpInfo2.Height = 25 * iRowCount + 10;

                                TlpInfo.Height = 25 * iRowCount + 10;

                                TlpInfo.RowStyles[0].Height = 25;

                                for (int i = itableCount; i < slValue.Length; i++)
                                {
                                    TlpInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
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

                                    TlpInfo.Controls.Add(txtTemp, iField, iRow);

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

                            string[] slValue = dr["PARAM_VALUE"].ToString().Split(',');

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

                                    TlpInfo.Controls.Add(lablTemp, iLabel, iRow);

                                    if (string.IsNullOrEmpty(m_tControlData[iLabelCount].sFieldName))
                                    {
                                        lablTemp.ForeColor = Color.Maroon;

                                        TlpInfo.SetColumnSpan(lablTemp, 2);
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

            // 取得流程卡各項設定項目，帶入到畫面
            // 獨立設定的項目，會在選取流程卡後，才帶入
            if (!string.IsNullOrEmpty(TstbInput.Text) && CheckInput())
            {
                ClearData();

                ShowData();
            }

            TstbInput.TextChanged += new EventHandler(TxtInput_TextChanged);

            // Depend system type to show tabpage
            if (OtSrv.SystemType(out g_SystemType))
            {
                // lot control
                this.TpSN.Parent = null;
            }

            #region 人工設定出站時間

            DtpInDate.Value = IN_PROCESS_TIME;

            bool CanSetWipTime = Check_Privilege(tsEmp.Text);

            if (CanSetWipTime)
            {
                TpInTime.Parent = TcParams;
            }
            else
            {
                TpInTime.Parent = null;
            }

            #endregion

            #region 強調顯示 舊編(OPTION2) 藍圖(OPTION4)

            sSQL = $@"
SELECT
    A.OPTION2 FORMER_NO
   ,A.OPTION4 BLUEPRINT
FROM
    SAJET.SYS_PART A
   ,SAJET.G_RC_STATUS B
WHERE
    A.PART_ID = B.PART_ID
AND B.RC_NO = '{TstbInput.Text}'
";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            LbBluePrint.Text = SajetCommon.SetLanguage("Blueprint") + ": ";

            LbFormerNO.Text = SajetCommon.SetLanguage("Former NO") + ": ";

            if (dsTemp != null && dsTemp.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrWhiteSpace(dsTemp.Tables[0].Rows[0]["FORMER_NO"].ToString()))
                {
                    LbFormerNO.Text += dsTemp.Tables[0].Rows[0]["FORMER_NO"].ToString();
                }
                else
                {
                    LbFormerNO.Text += " -- ";
                }
                if (!string.IsNullOrWhiteSpace(dsTemp.Tables[0].Rows[0]["BLUEPRINT"].ToString()))
                {
                    LbBluePrint.Text += dsTemp.Tables[0].Rows[0]["BLUEPRINT"].ToString();
                }
                else
                {
                    LbBluePrint.Text += " -- ";
                }
            }
            else
            {
                LbBluePrint.Text += "  --  ";

                LbFormerNO.Text += "  --  ";
            }

            #endregion

            #region 檢查製程是否使用 T4 / T6 爐，要收集日期碼、爐序
            // 會影像到資料是否共用的卡控。

            var messages = new List<string>();

            usingT4OrT6stove = T4Srv.CheckIfUsingT4orT6(
                rc_no: TstbInput.Text.Trim(),
                messages: ref messages);

            toPrintLabel = PlSrv.CheckIfPrintLabel(
                rc_no: TstbInput.Text.Trim(),
                messages: ref messages);

            // 設定使用 T4 / T6 爐的製程必須蒐集爐序、日期碼，不蒐集數量
            //sTOVESEQDataGridViewTextBoxColumn.Visible = usingT4OrT6stove;
            sTOVESEQDataGridViewTextBoxColumn.Visible = false;

            dATECODEDataGridViewTextBoxColumn.Visible = usingT4OrT6stove;

            //lOADQTYDataGridViewTextBoxColumn.Visible = !usingT4OrT6stove;

            ShareSettings = usingT4OrT6stove;

            ShareSettingSwitch(ShareSettings);

            LbT4T6.Text = string.Join(Environment.NewLine, messages);

            Color new_color = Color.FromArgb(255, 255, 192);

            sTOVESEQDataGridViewTextBoxColumn.DefaultCellStyle.BackColor = new_color;

            dATECODEDataGridViewTextBoxColumn.DefaultCellStyle.BackColor = new_color;

            #endregion

            MachineList = McSrv.GetModels(McSrv.GetMachineList(programInfo.dsRC.Tables[0].Rows[0]["PROCESS_ID"].ToString()));

            // 所有流程卡的資料模型
            RC_Details = CreateRCsList();

            // 選取第一筆流程卡的資料，帶入到右邊的設定畫面
            DgvRC_CellClick(null, null);
        }

        private void FMain_Shown(object sender, EventArgs e)
        {
            this.Text += $" ({SajetCommon.g_sFileName} : {SajetCommon.g_sFileVersion})";

            TControlData process_name_control_set =
                m_tControlData
                .First(x => x.sFieldName == "PROCESS_NAME");

            process_name_control_set.txtControl.BackColor = Color.Khaki;
            process_name_control_set.lablControl.ForeColor = Color.Red;

            #region 強調顯示的資訊

            var new_font_1 = new Font("Microsoft JhengHei", 30, FontStyle.Bold);

            var new_font_2 = new Font("Microsoft JhengHei", 20, FontStyle.Bold);

            LbBluePrint.Font = new_font_1;

            LbFormerNO.Font = new_font_1;

            TslForm.Font = new_font_2;

            #endregion

            #region 顯示工單備註

            string SQL = $@"
SELECT
    B.REMARK
FROM
    SAJET.G_RC_STATUS A
   ,SAJET.G_WO_BASE B
WHERE
    A.WORK_ORDER = B.WORK_ORDER
AND A.RC_NO = '{TstbInput.Text}'
";
            DataSet set = ClientUtils.ExecuteSQL(SQL);

            if (set != null &&
                set.Tables[0].Rows.Count > 0 &&
                Controls.Find("txtREMARK", true).Length > 0)
            {
                (Controls.Find("txtREMARK", true)[0] as TextBox).Text = set.Tables[0].Rows[0]["REMARK"].ToString();
            }

            #endregion

            #region 其他元件設定

            ScMain.SplitterDistance = 400;

            ScParam.SplitterDistance = Convert.ToInt32(ScParam.Height * 0.5);

            #endregion

            #region 製程參數欄位的寬度

            DgvInput.Columns["VALUE_DEFAULT"].HeaderText = SajetCommon.SetLanguage("Input Value");

            int w0 = DgvInput.Controls.OfType<VScrollBar>()
                .Where(x => x.Visible)
                .Select(x => x.Width)
                .DefaultIfEmpty(0)
                .FirstOrDefault();

            int w1 = GbCondition.Width - 110 - w0;
            int w2 = DgvInput.Columns["UNIT_NO"].Width = 43;
            int w3 = (w1 - w2) / 2;

            DgvInput.Columns["VALUE_DEFAULT"].Width
                = DgvInput.Columns["ITEM_NAME"].Width = w3;

            w0 = DgvCondition.Controls.OfType<VScrollBar>()
                .Where(x => x.Visible)
                .Select(x => x.Width)
                .DefaultIfEmpty(0)
                .FirstOrDefault();

            w1 = GbInput.Width - 110 - w0;
            w2 = DgvCondition.Columns["UNIT_NO"].Width = 43;
            w3 = (w1 - w2) / 2;

            DgvCondition.Columns["VALUE_DEFAULT"].Width
                = DgvCondition.Columns["ITEM_NAME"].Width = w3;

            #endregion

            #region 頁籤項目位置

            int i_position_1 = 0;
            int i_position_2 = 0;
            int i_default_distance = 10;

            // 報工時間頁籤
            {
                i_position_1 = label1.Right;

                if (CbReportTime.Right > i_position_1)
                {
                    i_position_1 = CbReportTime.Right;
                }

                i_position_2 = LbLastReportTime.Left;

                if (DtpInDate.Left > i_position_2)
                {
                    i_position_2 = DtpInDate.Left;
                }

                if (i_position_1 > i_position_2)
                {
                    LbLastReportTime.Left = i_position_1 + i_default_distance;

                    DtpInDate.Left = i_position_1 + i_default_distance;
                }
            }

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

            #region T4 / T6 不使用勾選功能

            TbCheckRC.Enabled = !usingT4OrT6stove;

            BtnSelectAll.Enabled = !usingT4OrT6stove;

            BtnReset.Enabled = !usingT4OrT6stove;

            #endregion
        }

        private void DgvSN_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int iCol = e.ColumnIndex - programInfo.iSNInput[0];

            DataGridViewCell cell = DgvSN.Rows[e.RowIndex].Cells[e.ColumnIndex];

            cell.ErrorText = "";

            switch (programInfo.dsSNParam.Tables[0].Rows[iCol]["CONVERT_TYPE"].ToString())
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

            switch (programInfo.dsSNParam.Tables[0].Rows[iCol]["INPUT_TYPE"].ToString())
            {
                case "R":
                    {
                        string[] slValue = programInfo.dsSNParam.Tables[0].Rows[iCol]["VALUE_LIST"].ToString().Split(',');

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
                            cell.ErrorText = SajetCommon.SetLanguage("Data Invalid", 1);
                        }

                        break;
                    }
                default:
                    {
                        if (programInfo.dsSNParam.Tables[0].Rows[iCol]["VALUE_TYPE"].ToString() == "N")
                        {
                            try
                            {
                                decimal dValue = decimal.Parse(cell.EditedFormattedValue.ToString());
                            }
                            catch
                            {
                                cell.ErrorText = SajetCommon.SetLanguage("Data Invalid", 1);
                            }
                        }

                        break;
                    }
            }

            if (string.IsNullOrEmpty(cell.ErrorText) &&
                programInfo.dsSNParam.Tables[0].Rows[iCol]["NECESSARY"].ToString() == "Y" &&
                string.IsNullOrEmpty(cell.EditedFormattedValue.ToString()))
            {
                cell.ErrorText = SajetCommon.SetLanguage("Data Empty", 1);
            }
        }

        private void DgvInput_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var row = DgvInput.Rows[e.RowIndex];

            DataGridViewCell cell = row.Cells[e.ColumnIndex];

            cell.ErrorText = "";

            if (CurrentRuncard != null)
            {
                DgvInputCheck(runcard: CurrentRuncard.RC_NO, itemName: row.Cells["ITEM_NAME"].Value.ToString(), value: cell.Value?.ToString());
            }

            switch (DgvInput.Rows[e.RowIndex].Cells["CONVERT_TYPE"].EditedFormattedValue.ToString())
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

            switch (DgvInput.Rows[e.RowIndex].Cells["INPUT_TYPE"].EditedFormattedValue.ToString())
            {
                case "R":
                    {
                        string[] slValue = DgvInput.Rows[e.RowIndex].Cells["VALUE_LIST"].EditedFormattedValue.ToString().Split(',');

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
                            if (DgvInput.Rows[e.RowIndex].Cells["NECESSARY"].EditedFormattedValue.ToString() == "Y")
                            {
                                cell.ErrorText = SajetCommon.SetLanguage("Data Invalid", 1);
                            }
                        }

                        break;
                    }
                default:
                    {
                        if (DgvInput.Rows[e.RowIndex].Cells["VALUE_TYPE"].EditedFormattedValue.ToString() == "N")
                        {
                            try
                            {
                                decimal dValue = decimal.Parse(cell.EditedFormattedValue.ToString());
                            }
                            catch
                            {
                                if (DgvInput.Rows[e.RowIndex].Cells["NECESSARY"].EditedFormattedValue.ToString() == "Y")
                                {
                                    cell.ErrorText = SajetCommon.SetLanguage("Data Invalid", 1);
                                }
                            }
                        }

                        break;
                    }
            }

            if (string.IsNullOrEmpty(cell.ErrorText)
                && DgvInput.Rows[e.RowIndex].Cells["NECESSARY"].EditedFormattedValue.ToString() == "Y"
                && string.IsNullOrEmpty(cell.EditedFormattedValue.ToString()))
            {
                cell.ErrorText = SajetCommon.SetLanguage("Data Empty", 1);
            }
        }

        #region 超連結按鈕

        private void DgvCondition_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow r in DgvCondition.Rows)
            {
                if (r.Cells["VALUE_TYPE"].Value.ToString() == "L" &&
                    !Uri.IsWellFormedUriString(r.Cells["VALUE_TYPE"].Value.ToString(), UriKind.Absolute))
                {
                    r.Cells["VALUE_DEFAULT"] = new DataGridViewLinkCell();
                }
            }
        }

        private void DgvCondition_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            /* 確認欄位型態 */
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            if (DgvCondition.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewLinkCell)
            {
                string url = DgvCondition.Rows[e.RowIndex].Cells["VALUE_DEFAULT"].Value.ToString();

                System.Diagnostics.Process.Start(url);
            }
        }

        #endregion

        #region 統計已選擇 RC 數量

        private void DgvRC_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (DgvRC.IsCurrentCellDirty)
            {
                DgvRC.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void DgvRC_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                CountRC();
            }
        }

        #endregion

        private void DgvRC_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // 沒有點選畫面上任何一個流程卡的情況
            // 就把用來放 "當前流程卡資訊" 的變數都重置
            if (DgvRC.CurrentRow == null)
            {
                Current_RC_NO = string.Empty;

                CurrentRuncard = null;

                LbCurrentRC1.Text
                    = LbCurrentRC2.Text
                    = LbCurrentRC4.Text
                    = SajetCommon.SetLanguage("No selected runcard.");

                DtpInDate.Value = IN_PROCESS_TIME;

                return;
            }

            DataGridViewRow current_row = DgvRC.CurrentRow;

            Current_RC_NO = current_row.Cells["RC_NO"].Value?.ToString() ?? "";

            if (e?.RowIndex > -1 &&
                e?.ColumnIndex == DgvRC.Columns["CHECK"].Index &&
                !string.IsNullOrWhiteSpace(Current_RC_NO))
            {
                bool selected = (bool)current_row.Cells["CHECK"].EditedFormattedValue;

                current_row.Cells["CHECK"].Value = !selected;
            }

            // 當前流程卡
            CurrentRuncard = RC_Details.First(x => x.RC_NO == Current_RC_NO);

            // 顯示流程卡號
            LbCurrentRC1.Text
                = LbCurrentRC2.Text
                = LbCurrentRC4.Text
                = SajetCommon.SetLanguage("RunCard")
                + SajetCommon.SetLanguage(":")
                + Current_RC_NO;

            // 顯示機台
            DgvMachine.DataSource = CurrentRuncard.Machines.ToList();

            // 資料收集
            foreach (DataGridViewRow row in DgvInput.Rows)
            {
                string itemName = row.Cells["ITEM_NAME"].Value.ToString();

                var item = CurrentRuncard.DataCollections.First(x => x.ITEM_NAME == itemName);

                //DgvInputCheck(runcard: CurrentRuncard.RC_NO, itemName: itemName, value: item.INPUT_VALUE);
                DgvInputCheck(item);

                row.Cells["VALUE_DEFAULT"].Value = item.INPUT_VALUE;

                row.Cells["VALUE_DEFAULT"].ErrorText = item.ERROR_TEXT;
            }

            // 顯示設定的報工時間
            LbLastReportTime.Text = RcSrv.GetLastReportTime(Runcard: Current_RC_NO);

            #region 先把事件拔掉，避免 DateTimePicker 的勾勾自己勾起來

            DtpInDate.ValueChanged -= DtpInDate_ValueChanged;

            CbReportTime.CheckStateChanged -= CbReportTime_CheckStateChanged;

            if (!ShareSettings)
            {
                DtpInDate.Value = CurrentRuncard.IN_PROCESS_TIME;

                CbReportTime.Checked = CurrentRuncard.DateTimePicker_set;
            }

            DtpInDate.ValueChanged += DtpInDate_ValueChanged;

            CbReportTime.CheckStateChanged += CbReportTime_CheckStateChanged;

            #endregion

            //RearrangeDataGridView(ref DgvMachine);
        }

        private void DtpInDate_DropDown(object sender, EventArgs e)
        {
            if (!ShareSettings)
            {
                CurrentRuncard.DateTimePicker_set = true;
            }

            CbReportTime.Checked = true;
        }

        private void DtpInDate_ValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Current_RC_NO))
            {
                return;
            }

            CbReportTime.CheckStateChanged -= CbReportTime_CheckStateChanged;

            RC_DETAILS current_rc_no = RC_Details.First(x => x.RC_NO == Current_RC_NO);

            if (!ShareSettings)
            {
                current_rc_no.IN_PROCESS_TIME = DtpInDate.Value;

                current_rc_no.DateTimePicker_set = true;
            }

            CbReportTime.Checked = true;

            CbReportTime.CheckStateChanged += CbReportTime_CheckStateChanged;
        }

        private void CbReportTime_CheckStateChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Current_RC_NO))
            {
                return;
            }

            DtpInDate.ValueChanged -= DtpInDate_ValueChanged;

            RC_DETAILS current_rc_no = RC_Details.First(x => x.RC_NO == Current_RC_NO);

            if (CbReportTime.Checked)
            {
                if (!ShareSettings)
                {
                    current_rc_no.IN_PROCESS_TIME = DtpInDate.Value;

                    current_rc_no.DateTimePicker_set = true;
                }
            }
            else
            {
                current_rc_no.IN_PROCESS_TIME = IN_PROCESS_TIME;

                current_rc_no.DateTimePicker_set = false;

                DtpInDate.Value = IN_PROCESS_TIME;
            }

            DtpInDate.ValueChanged += DtpInDate_ValueChanged;
        }

        /// <summary>
        /// Input Keypart sn trigger event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditKPSN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
            {
                return;
            }

            //確認KPSN有輸入值
            if (string.IsNullOrEmpty(editKPSN.Text))
            {
                string message = SajetCommon.SetLanguage("Keypart SN Error.");

                SajetCommon.Show_Message(message, 0);

                return;
            }
            else // 如果是廠內序號增加帶出目前數量
            {
                try
                {
                    string sSQL = @"
SELECT
    CURRENT_QTY
FROM
    SAJET.G_RC_STATUS
WHERE
    RC_NO = :RC_NO
AND ROWNUM = 1
";
                    var Params = new List<object[]>
                    {
                        new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", editKPSN.Text.Trim() }
                    };

                    DataSet ds = ClientUtils.ExecuteSQL(sSQL, Params.ToArray());

                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            editCount.Text = ds.Tables[0].Rows[0]["CURRENT_QTY"].ToString();
                        }
                        else
                        {
                            editCount.Text = "0";
                        }
                    }
                    else
                    {
                        editCount.Text = "0";
                    }
                }
                catch (Exception)
                {
                    editCount.Text = "0";
                }
            }

            editCount.SelectAll();

            editCount.Focus();
        }

        private void EditCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
            {
                return;
            }

            // 確認用量有輸入值
            if (string.IsNullOrEmpty(editCount.Text))
            {
                string message = SajetCommon.SetLanguage("Keypart SN Error.");

                SajetCommon.Show_Message(message, 0);

                editCount.SelectAll();

                editCount.Focus();

                return;
            }
            else
            {
                // 確認用量正確性
                if (!OtSrv.IsNumeric(3, editCount.Text))
                {
                    string message = SajetCommon.SetLanguage("Please Input Quantity in Positive number");

                    SajetCommon.Show_Message(message, 0);

                    editCount.SelectAll();

                    editCount.Focus();

                    return;
                }
            }
        }

        private void TxtInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
            {
                return;
            }

            if (CheckInput())
            {
                ClearData();

                ShowData();
            }

            TstbInput.SelectAll();
        }

        private void TxtInput_TextChanged(object sender, EventArgs e)
        {
            ClearData();
        }

        private void TbCheckRC_KeyDown(object sender, KeyEventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            string message;

            if (usingT4OrT6stove)
            {
                return;
            }

            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            if (DgvRC.Rows.Count <= 0)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(TbCheckRC.Text))
            {
                return;
            }

            if (!int.TryParse(TbCheckRC.Text, out int i_rc_number))
            {
                message = SajetCommon.SetLanguage(MessageEnum.PleaseKeyInNumbers.ToString());

                SajetCommon.Show_Message(message, 3);

                TbCheckRC.SelectAll();

                TbCheckRC.Focus();

                return;
            }

            CountAndTickCheckbox(i_rc_number);

            int i_check_total = CountRC();

            TbCheckRC.Text = "";

            Cursor = Cursors.Default;

            if (i_rc_number > i_check_total)
            {
                message
                    = SajetCommon.SetLanguage(MessageEnum.CheckQuantity.ToString())
                    + ": "
                    + i_check_total
                    ;

                SajetCommon.Show_Message(message, 3);
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            var Params = new List<object[]>();

            string message = string.Empty;

            IN_PROCESS_TIME = OtSrv.GetDBDateTimeNow();

            # region Keyparts Collection

            if (!CheckKPSNInput())
            {
                TcParams.SelectedTab = TpKeyparts;

                editKPSN.Focus();

                return;
            }

            string skeypart = string.Empty;

            foreach (DataGridViewRow dr in DgvKeypart.Rows)
            {
                skeypart += dr.Cells["PART_NO"].EditedFormattedValue.ToString() + (char)9
                    + dr.Cells["RC_NO"].EditedFormattedValue.ToString() + (char)9
                    + dr.Cells["ITEM_COUNT"].EditedFormattedValue.ToString() + (char)27;
            }

            #endregion

            # region Piece

            string sSN = string.Empty;

            if (programInfo.iSNInput[1] > -1)
            {
                foreach (DataGridViewRow dr in DgvSN.Rows)
                {
                    for (int i = programInfo.iSNInput[1]; i < DgvSN.Columns.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(dr.Cells[i].ErrorText))
                        {
                            SajetCommon.Show_Message(DgvSN.Columns[i].HeaderText + dr.Cells[i].ErrorText, 0);

                            DgvSN.CurrentCell = dr.Cells[i];

                            DgvSN.Focus();

                            TcParams.SelectedTab = TpSN;

                            return;
                        }

                        if (!string.IsNullOrEmpty(dr.Cells[0].EditedFormattedValue.ToString()))
                        {
                            sSN += dr.Cells["SERIAL_NUMBER"].EditedFormattedValue.ToString() + (char)9
                                + DgvSN.Columns[i].Tag.ToString() + (char)9
                                + dr.Cells[i].EditedFormattedValue.ToString() + (char)27;
                        }
                    }
                }
            }

            # endregion

            #region 檢查資料收集是否合格

            // 只檢查有沒有不合格的輸入值，進入迴圈裡面時再整理傳入參數
            foreach (var RC in RC_Details)
            {
                if (RC.SELECT)
                {
                    // 再檢查一次資料收集數值
                    RC.DataCollections.ForEach(x =>
                    {
                        DgvInputCheck(x);
                    });

                    string s = string.Empty;

                    // 組合提示訊息
                    RC.DataCollections.ForEach(x =>
                    {
                        if (!string.IsNullOrWhiteSpace(x.ERROR_TEXT))
                        {
                            s += $"    {x.ITEM_NAME} : {x.ERROR_TEXT}" + Environment.NewLine;
                        }
                    });

                    if (!string.IsNullOrEmpty(s))
                    {
                        message = $"{RC.RC_NO}" + Environment.NewLine + s;
                    }
                }
            }

            if (!string.IsNullOrEmpty(message))
            {
                message
                    = SajetCommon.SetLanguage("Collected data is not up to standard")
                    + SajetCommon.SetLanguage(":")
                    + Environment.NewLine
                    + message;

                SajetCommon.Show_Message(message, 0);

                return;
            }

            #endregion

            // 重新建立已選取 RC 的清單
            var ExecuteRCs = CreateRCsList()
                .Where(x => x.SELECT)
                .ToList();

            #region 自訂報工產出時間的檢查
            // 把共用不共用資料的情境也考慮進來

            foreach (var rc in ExecuteRCs)
            {
                DateTime in_process_time
                    = ShareSettings
                    ? DtpInDate.Value
                    : rc.IN_PROCESS_TIME;

                if (!RcSrv.IsOutputTimeValid(
                    runcard: RcSrv.GetRcNoInfo(Runcard: rc.RC_NO),
                    InProcessTime: in_process_time,
                    message: out message))
                {
                    message
                        += Environment.NewLine
                        + SajetCommon.SetLanguage(MessageEnum.Runcard.ToString())
                        + ": "
                        + rc.RC_NO;

                    SajetCommon.Show_Message(message, 0);

                    return;
                }
            }

            #endregion

            // 沒勾選流程卡就不繼續執行
            if (ExecuteRCs.Count <= 0)
            {
                message = SajetCommon.SetLanguage("Please select runcard(s).");

                SajetCommon.Show_Message(message, 1);

                return;
            }

            #region 每個流程卡都要選擇機台

            var RC_without_machine = new List<string>();

            if (MachineList.Count > 0)
            {
                ExecuteRCs.ForEach(x =>
                {
                    if (!x.Machines.Any(z => z.Select))
                    {
                        RC_without_machine.Add(x.RC_NO);
                    }
                });
            }

            if (RC_without_machine.Count > 0)
            {
                message
                    = SajetCommon.SetLanguage("No machine selected")
                    + Environment.NewLine
                    + SajetCommon.SetLanguage("RunCards")
                    + SajetCommon.SetLanguage(":")
                    + Environment.NewLine
                    + string.Join(", ", RC_without_machine);

                SajetCommon.Show_Message(message, 1);

                return;
            }

            #endregion

            #region 製程設定為 "會使用到 T4 爐 / T6 爐" 的製程，要做的檢查

            var t4NoPass = new List<string>();

            foreach (var rc in ExecuteRCs)
            {
                var dateCodeNotPass = new List<string>();

                foreach (var row in rc.Machines.Where(x => x.Select))
                {
                    // 機台代碼
                    string machineCode = row.MACHINE_CODE;

                    // 日期碼
                    string date_code = row.DATE_CODE?.ToString().Trim() ?? "";

                    if (usingT4OrT6stove || !string.IsNullOrEmpty(date_code))
                    {
                        if (!long.TryParse(date_code, out long l_date_code)
                            || l_date_code.ToString().Length != 8)
                        {
                            dateCodeNotPass.Add(machineCode);
                        }
                    }
                }

                if (dateCodeNotPass.Any())
                {
                    string s_msg = rc.RC_NO + ": " + string.Join(", ", dateCodeNotPass);

                    t4NoPass.Add(s_msg);
                }
            }

            if (t4NoPass.Any())
            {
                message
                    = SajetCommon.SetLanguage("Stove sequence required 8 digits number")
                    + Environment.NewLine
                    + string.Join(Environment.NewLine, t4NoPass);

                SajetCommon.Show_Message(message, 0);

                return;
            }

            #endregion

            #region 檢查加工數量（有機台的製程再檢查）

            var QtyNoPass_1 = new List<string>();
            var QtyNoPass_2 = new List<string>();

            if (MachineList.Count > 0)
            {
                foreach (var rc in ExecuteRCs)
                {
                    int i_current_qty = rc.CURRENT_QTY;

                    int i_load_qty = 0;

                    foreach (var m in rc.Machines.Where(x => x.Select))
                    {
                        // 負載量
                        if (m.LOAD_QTY < 0)
                        {
                            QtyNoPass_1.Add(rc.RC_NO);
                        }

                        i_load_qty += m.LOAD_QTY ?? 0;
                    }

                    // 加工數量等於流程卡數量
                    if (i_current_qty != i_load_qty)
                    {
                        QtyNoPass_2.Add(rc.RC_NO);
                    }
                }
            }

            if (QtyNoPass_1.Any())
            {
                message
                    = SajetCommon.SetLanguage("The load of the machine must be a non-negative number")
                    + Environment.NewLine
                    + SajetCommon.SetLanguage("RunCard")
                    + ": "
                    + string.Join(", ", QtyNoPass_1)
                    ;

                SajetCommon.Show_Message(message, 0);

                return;
            }

            if (QtyNoPass_2.Any())
            {
                message
                    = SajetCommon.SetLanguage("The total load of the machine must match the runcard's current quantity")
                    + Environment.NewLine
                    + SajetCommon.SetLanguage("RunCard")
                    + ": "
                    + string.Join(", ", QtyNoPass_2)
                    ;

                SajetCommon.Show_Message(message, 0);

                return;
            }

            #endregion

            if (RCSource.Tables[0].Rows.Count > ExecuteRCs.Count &&
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Only execute part of the runcards"), 2) != DialogResult.Yes)
            {
                return;
            }

            foreach (var RC in ExecuteRCs)
            {
                var RcInfo = GetRcNoInfo(RC.RC_NO);

                //var row = DgvRC.Rows.Cast<DataGridViewRow>()
                //    .FirstOrDefault(m => m.Cells["RC_NO"].Value.ToString() == RC.RC_NO);

                #region 製程機台

                string sMachine = string.Empty;

                var Machines = new List<MachineDownModel>();

                RC.Machines.ForEach(x =>
                {
                    if (x.Select)
                    {
                        sMachine += x.MACHINE_CODE + (char)9;

                        Machines.Add(new MachineDownModel
                        {
                            MACHINE_ID = x.MACHINE_ID,
                            LOAD_QTY = x.LOAD_QTY,
                            DATE_CODE = x.DATE_CODE,
                            STOVE_SEQ = x.STOVE_SEQ,
                        });
                    }
                });

                #endregion

                #region 檢查資料收集是否有輸入

                string sInput = string.Empty;

                RC.DataCollections.ForEach(x =>
                {
                    if (!string.IsNullOrWhiteSpace(x.INPUT_VALUE))
                    {
                        sInput += x.ITEM_ID + (char)9 + x.INPUT_VALUE + (char)27;
                    }
                });

                #endregion

                #region 報工時間

                DateTime time;

                if (ShareSettings)
                {
                    time = CbReportTime.Checked ? DtpInDate.Value : IN_PROCESS_TIME;
                }
                else
                {
                    time = RC.DateTimePicker_set ? RC.IN_PROCESS_TIME : IN_PROCESS_TIME;
                }

                #endregion

                Params = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "TEMP", tsEmp.Text },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "TRC", RC.RC_NO },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "TITEM", sInput },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "TSN", sSN },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "TMEMO", tsMemo.Text },
                    new object[] { ParameterDirection.Input, OracleType.DateTime, "TNOW", time },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "TMACHINE", sMachine },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "TKEYPART", skeypart },
                    new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" }
                };

                DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_RC_INPUT", Params.ToArray());

                string sMsg = ds.Tables[0].Rows[0]["TRES"].ToString();

                if (sMsg != "OK")
                {
                    tsMsg.ForeColor = Color.Red;

                    message
                        = SajetCommon.SetLanguage(sMsg)
                        + Environment.NewLine
                        + SajetCommon.SetLanguage("RunCard")
                        + ": " + RC.RC_NO;

                    SajetCommon.Show_Message(message, 0);

                    message
                        = SajetCommon.SetLanguage(ds.Tables[0].Rows[0]["TRES"].ToString(), 1)
                        + " "
                        + SajetCommon.SetLanguage("RunCard")
                        + SajetCommon.SetLanguage(":")
                        + RC.RC_NO;

                    tsMsg.Text = message;

                    return;
                }
                else
                {
                    RecordMachine(RcInfo: RcInfo, Machines: Machines, now: time);

                    tsMsg.ForeColor = Color.Blue;

                    message
                        = SajetCommon.SetLanguage(ds.Tables[0].Rows[0]["TRES"].ToString(), 1)
                        + " "
                        + SajetCommon.SetLanguage("RunCard")
                        + SajetCommon.SetLanguage(":")
                        + RC.RC_NO;

                    tsMsg.Text = message;
                }
            }

            if (toPrintLabel)
            {
                message = SajetCommon.SetLanguage(MessageEnum.PrintTheLabels.ToString());

                if (SajetCommon.Show_Message(message, 2) == DialogResult.Yes)
                {
                    if (!PlSrv.PrintLabel(RC_Details, out message))
                    {
                        SajetCommon.Show_Message(message, 1);
                    }
                }
            }

            DialogResult = DialogResult.Yes;

            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void BtnAppend_Click(object sender, EventArgs e)
        {
            //確認KPSN有輸入值
            if (string.IsNullOrEmpty(editKPSN.Text))
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Keypart SN Error."), 0);

                return;
            }

            // 確認用量有輸入值
            if (string.IsNullOrEmpty(editCount.Text))
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Item Count Error."), 0);

                editCount.SelectAll();

                editCount.Focus();

                return;
            }
            else
            {
                // 確認用量正確性
                if (!OtSrv.IsNumeric(3, editCount.Text))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Input Quantity in Positive number"), 0);

                    editCount.SelectAll();

                    editCount.Focus();

                    return;
                }
            }

            // 確認是否已點選料號
            if (string.IsNullOrEmpty(DgvBOM.CurrentRow.Cells["PART_NO"].Value.ToString()))
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Choose one Part NO in BOM."), 0);

                return;
            }

            CheckBOM(editKPSN.Text.Trim());

            editCount.Text = "0";

            editKPSN.SelectAll();

            editKPSN.Focus();
        }

        /// <summary>
        /// Delete datagridview data seleted keypart 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Delete the Keypart"), 2) == DialogResult.Yes)
            {
                int iSelect = DgvKeypart.SelectedRows.Count;

                for (int i = 0; i < iSelect; i++)
                {
                    DgvKeypart.Rows.Remove(DgvKeypart.SelectedRows[0]);
                }
            }

            CountKPSN();
        }

        #region 勾選 RC 按鈕
        private void BtnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DataRow row in RCSource.Tables[0].Rows)
            {
                row[0] = 1;
            }

            CountRC();
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            RCSource.Tables[0].Rows.Cast<DataRow>()
                .ToList()
                .ForEach(row => row[0] = 0);

            CountRC();
        }

        #endregion

        #region 共用資料的事件

        private void CbShareSetting_CheckedChanged(object sender, EventArgs e)
        {
            ShareSettings = CbShareSetting.Checked;

            ShareSettingSwitch(ShareSettings);

            if (ShareSettings)
            {
                // 改變共用不共用資料的設定時，清空已經填寫的機台資料
                RC_Details.ForEach(x =>
                {
                    x.Machines = MachineList
                    .Select(m => new MachineDownModel(m))
                    .ToList();
                });
            }

            DgvMachine.DataSource = CurrentRuncard.Machines.ToList();

            //RearrangeDataGridView(ref DgvMachine);

            /*
            lOADQTYDataGridViewTextBoxColumn.Visible = !ShareMachineSettings;

            if (ShareMachineSettings && CurrentRuncard != null)
            {
                RC_Details.ForEach(rc =>
                {
                    if (rc.SELECT)
                    {
                        rc.Machines = CurrentRuncard
                        .Machines
                        .Select(m => new MachineDownModel(m)
                        {
                            LOAD_QTY = 0,
                        }).ToList();
                    }
                });
            }
            //*/

        }

        private void CbShareSetting_2_CheckedChanged(object sender, EventArgs e)
        {
            ShareSettings = CbShareSetting_2.Checked;

            ShareSettingSwitch(ShareSettings);

            if (ShareSettings)
            {
                // 改變共用不共用資料的設定時，清空已經填寫的機台資料
                RC_Details.ForEach(x =>
                {
                    x.Machines = MachineList
                    .Select(m => new MachineDownModel(m))
                    .ToList();
                });
            }

            DgvMachine.DataSource = CurrentRuncard.Machines.ToList();

            //RearrangeDataGridView(ref DgvMachine);

        }

        #endregion

        #region 機台頁籤的事件

        private void DgvMachine_VisibleChanged(object sender, EventArgs e)
        {
            RearrangeDataGridView(ref DgvMachine);

            dATECODEDataGridViewTextBoxColumn.Width = 120;
        }

        private void DgvMachine_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 &&
                e.ColumnIndex == selectDataGridViewCheckBoxColumn.Index &&
                !string.IsNullOrWhiteSpace(Current_RC_NO))
            {
                DataGridViewRow machine_current_row = DgvMachine.Rows[e.RowIndex];

                bool selected
                    = !(bool)machine_current_row.Cells[nameof(selectDataGridViewCheckBoxColumn)].Value;

                string MachineCode
                    = machine_current_row.Cells[nameof(mACHINECODEDataGridViewTextBoxColumn)].Value.ToString();

                machine_current_row.Cells[nameof(selectDataGridViewCheckBoxColumn)].Value = selected;

                if (selected)
                {
                    string machine_load
                        = machine_current_row.Cells[nameof(lOADQTYDataGridViewTextBoxColumn)].Value.ToString();

                    // 填入加工數量，預設填入流程卡當前數量
                    if (machine_load == "0")
                    {
                        machine_current_row.Cells[nameof(lOADQTYDataGridViewTextBoxColumn)].Value
                            = CurrentRuncard.CURRENT_QTY;
                    }
                }

                // 是否共用機台資料
                if (ShareSettings)
                {
                    RC_Details.ForEach(rc =>
                    {
                        if (rc.SELECT)
                        {
                            MachineDownModel machine
                            = rc.Machines
                            .First(x => x.MACHINE_CODE == MachineCode);

                            // 一起勾選
                            machine.Select = selected;

                            // 填入加工數量，預設填入流程卡當前數量
                            if ((machine.LOAD_QTY ?? 0) == 0)
                            {
                                machine.LOAD_QTY = rc.CURRENT_QTY;
                            }
                        }
                    });
                }

            }

        }

        private void DgvMachine_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= NumericTextBoxColumn_KeyPress;

            e.Control.Leave -= DgvMachineEditColumn_Leave;

            if (DgvMachine.CurrentCell.ColumnIndex == dATECODEDataGridViewTextBoxColumn.Index ||
                DgvMachine.CurrentCell.ColumnIndex == lOADQTYDataGridViewTextBoxColumn.Index ||
                DgvMachine.CurrentCell.ColumnIndex == sTOVESEQDataGridViewTextBoxColumn.Index)
            {
                if (e.Control is TextBox x)
                {
                    x.KeyPress += NumericTextBoxColumn_KeyPress;

                    x.Leave += DgvMachineEditColumn_Leave;
                }

                if (e.Control is ComboBox c)
                {
                    c.Leave += DgvMachineEditColumn_Leave;
                }
            }
        }

        /// <summary>
        /// 只能輸入正整數
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumericTextBoxColumn_KeyPress(object sender, KeyPressEventArgs e)
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
        private void DgvMachineEditColumn_Leave(object sender, EventArgs e)
        {
            DataGridViewRow machine_current_row = DgvMachine.CurrentRow;

            DataGridViewCell machine_current_cell = DgvMachine.CurrentCell;

            string MachineCode
                = machine_current_row.Cells[nameof(mACHINECODEDataGridViewTextBoxColumn)].Value.ToString();

            bool selected
                = (bool)machine_current_row.Cells[selectDataGridViewCheckBoxColumn.Index].Value;

            if (sender is DataGridViewTextBoxEditingControl z)
            {
                // 加工數量
                if (machine_current_cell.ColumnIndex == lOADQTYDataGridViewTextBoxColumn.Index &&
                    int.TryParse(z.Text, out int load) && load > 0)
                {
                    selected = true;

                    machine_current_row.Cells[selectDataGridViewCheckBoxColumn.Index].Value = selected;

                    // 是否共用機台資料
                    if (CurrentRuncard.SELECT && ShareSettings)
                    {
                        RC_Details.ForEach(rc =>
                        {
                            if (rc.SELECT)
                            {
                                var machine = rc
                                 .Machines
                                 .First(x => x.MACHINE_CODE == MachineCode);

                                machine.LOAD_QTY = load;

                                machine.Select = selected;
                            }
                        });
                    }
                }

                // 日期碼 → 現在作為爐號使用
                if (machine_current_cell.ColumnIndex == dATECODEDataGridViewTextBoxColumn.Index &&
                    !string.IsNullOrWhiteSpace(z.Text))
                {
                    selected = true;

                    machine_current_row.Cells[selectDataGridViewCheckBoxColumn.Index].Value = selected;

                    string dateCode = z.Text.Trim();

                    // 填入加工數量，預設填入流程卡當前數量
                    if (selected)
                    {
                        string machine_load
                            = machine_current_row.Cells[nameof(lOADQTYDataGridViewTextBoxColumn)].Value.ToString();

                        if (machine_load == "0")
                        {
                            machine_current_row.Cells[nameof(lOADQTYDataGridViewTextBoxColumn)].Value
                                = CurrentRuncard.CURRENT_QTY;
                        }
                    }

                    // 是否共用機台資料
                    if (CurrentRuncard.SELECT && ShareSettings)
                    {
                        RC_Details.ForEach(rc =>
                        {
                            if (rc.SELECT)
                            {
                                var machine = rc
                                 .Machines
                                 .First(x => x.MACHINE_CODE == MachineCode);

                                machine.DATE_CODE = dateCode;

                                // 一起勾選
                                machine.Select = selected;

                                // 填入加工數量，預設填入流程卡當前數量
                                if ((machine.LOAD_QTY ?? 0) == 0)
                                {
                                    machine.LOAD_QTY = rc.CURRENT_QTY;
                                }
                            }
                        });
                    }
                }

            }
            // 爐序 → 現已廢棄不用
            else if (sender is DataGridViewComboBoxEditingControl c)
            {
                if (machine_current_cell.ColumnIndex == sTOVESEQDataGridViewTextBoxColumn.Index &&
                    !string.IsNullOrWhiteSpace(c.Text))
                {
                    selected = true;

                    machine_current_row.Cells[selectDataGridViewCheckBoxColumn.Index].Value = selected;

                    // 填入加工數量，預設填入流程卡當前數量
                    if (selected)
                    {
                        string machine_load
                            = machine_current_row.Cells[nameof(lOADQTYDataGridViewTextBoxColumn)].Value.ToString();

                        if (machine_load == "0")
                        {
                            machine_current_row.Cells[nameof(lOADQTYDataGridViewTextBoxColumn)].Value
                                = CurrentRuncard.CURRENT_QTY;
                        }
                    }

                    // 是否共用機台資料
                    if (CurrentRuncard.SELECT && ShareSettings)
                    {
                        RC_Details.ForEach(rc =>
                        {
                            if (rc.SELECT)
                            {
                                var machine = rc
                                 .Machines
                                 .First(x => x.MACHINE_CODE == MachineCode);

                                machine.STOVE_SEQ = c.Text;

                                // 一起勾選
                                machine.Select = selected;

                                // 填入加工數量，預設填入流程卡當前數量
                                if ((machine.LOAD_QTY ?? 0) == 0)
                                {
                                    machine.LOAD_QTY = rc.CURRENT_QTY;
                                }
                            }
                        });
                    }

                }
            }

        }

        #endregion

        private void ShowData()
        {
            string sSQL = programInfo.sSQL["SQL"];

            var Params = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "SN", TstbInput.Text }
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
                            // 工單報工模式顯示單個 RC_NO 沒有意義
                            if (m_tControlData[i].sFieldName.Equals("RC_NO"))
                            {
                                m_tControlData[i].txtControl.Text = "--";
                            }
                            else
                            {
                                m_tControlData[i].txtControl.Text = programInfo.dsRC.Tables[0].Rows[0][m_tControlData[i].sFieldName].ToString();
                            }

                            //if (m_tControlData[i].sFieldName.Equals("PROCESS_NAME"))
                            //{
                            //    LbProcess.Text = SajetCommon.SetLanguage("PROCESS_NAME") + ": " + programInfo.dsRC.Tables[0].Rows[0][m_tControlData[i].sFieldName].ToString();
                            //}
                        }
                    }

                    DspReportData();

                    RCsDataGrid();
                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);
            }
        }

        private void DspReportData()
        {
            #region Process Parameter

            var Params = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", programInfo.dsRC.Tables[0].Rows[0]["PROCESS_ID"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", programInfo.dsRC.Tables[0].Rows[0]["PART_ID"].ToString() }
            };

            // 製程條件
            DataSet ds = ClientUtils.ExecuteSQL(programInfo.sSQL["製程條件"], Params.ToArray());

            DgvCondition.DataSource = ds;

            DgvCondition.DataMember = ds.Tables[0].ToString();

            foreach (DataColumn dc in ds.Tables[0].Columns)
            {
                DgvCondition.Columns[dc.ColumnName].HeaderText = SajetCommon.SetLanguage(DgvCondition.Columns[dc.ColumnName].HeaderText, 1);
            }

            DgvCondition.Columns["VALUE_TYPE"].Visible = false;

            // 資料收集
            ds = ClientUtils.ExecuteSQL(programInfo.sSQL["資料收集"], Params.ToArray());

            if (DgvInput.ColumnCount == 0)
            {
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    DgvInput.Columns.Add(ds.Tables[0].Columns[i].ColumnName, SajetCommon.SetLanguage(ds.Tables[0].Columns[i].ColumnName, 1));

                    if (i > programInfo.iInputVisible["資料收集"])
                    {
                        DgvInput.Columns[i].Visible = false;
                    }
                    else
                    {
                        DgvInput.Columns[i].ReadOnly = programInfo.iInputField["資料收集"].IndexOf(i) == -1;
                    }
                }

                DgvInput.Columns["UNIT_NO"].Visible = true;

                DgvInput.Columns["UNIT_NO"].ReadOnly = true;
            }

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                DgvInput.Rows.Add(dr.ItemArray);

                if (dr["INPUT_TYPE"].ToString() == "S")
                {
                    DataGridViewComboBoxCell tCell = new DataGridViewComboBoxCell();

                    string[] slValue = dr["VALUE_LIST"].ToString().Split(',');

                    for (int i = 0; i < slValue.Length - 1; i++)
                    {
                        tCell.Items.Add(slValue[i]);
                    }

                    DgvInput["VALUE_DEFAULT", DgvInput.Rows.Count - 1] = tCell;
                }

                if (dr["NECESSARY"].ToString() == "Y")
                {
                    DgvInput.Rows[DgvInput.Rows.Count - 1].Cells["VALUE_DEFAULT"].Style.BackColor = Color.FromArgb(255, 255, 192);

                    DgvInput.Rows[DgvInput.Rows.Count - 1].Cells["VALUE_DEFAULT"].ErrorText = SajetCommon.SetLanguage("Data Empty", 1);
                }

                // 建立資料收集項目的清單
                CollectionItems.Add(new DataCollectModel
                {
                    ITEM_ID = dr[nameof(DataCollectModel.ITEM_ID)].ToString(),
                    ITEM_NAME = dr[nameof(DataCollectModel.ITEM_NAME)].ToString(),
                    VALUE_DEFAULT = dr[nameof(DataCollectModel.VALUE_DEFAULT)].ToString(),
                    INPUT_VALUE = dr[nameof(DataCollectModel.VALUE_DEFAULT)].ToString(),
                    VALUE_TYPE = dr[nameof(DataCollectModel.VALUE_TYPE)].ToString(),
                    INPUT_TYPE = dr[nameof(DataCollectModel.INPUT_TYPE)].ToString(),
                    NECESSARY = dr[nameof(DataCollectModel.NECESSARY)].ToString(),
                    CONVERT_TYPE = dr[nameof(DataCollectModel.CONVERT_TYPE)].ToString(),
                    VALUE_LIST = dr[nameof(DataCollectModel.VALUE_LIST)].ToString(),
                    UNIT_NO = dr[nameof(DataCollectModel.UNIT_NO)].ToString(),
                });
            }

            foreach (DataGridViewColumn col in DgvCondition.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            foreach (DataGridViewColumn col in DgvInput.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            #endregion

            #region Machine
            // 先點選流程卡，再帶入流程卡的機台資料
            /*
            Params = new List<object[]>();

            if (bOneSheet)
            {
                string s = @"
SELECT
    B.MACHINE_CODE
   ,D.STATUS_NAME
   ,D.RUN_FLAG
   ,D.DEFAULT_STATUS
   ,B.MACHINE_ID
FROM
    SAJET.SYS_RC_PROCESS_MACHINE A
   ,SAJET.SYS_MACHINE B
   ,SAJET.G_MACHINE_STATUS C
   ,SAJET.SYS_MACHINE_STATUS D
WHERE
    A.PROCESS_ID = :PROCESS_ID
AND A.MACHINE_ID = B.MACHINE_ID
AND A.MACHINE_ID = C.MACHINE_ID
AND C.CURRENT_STATUS_ID = D.STATUS_ID
AND A.ENABLED = 'Y'
AND B.ENABLED = 'Y'
ORDER BY
    B.MACHINE_CODE
";

                Params.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", programInfo.dsRC.Tables[0].Rows[0]["PROCESS_ID"].ToString() });

                //ds = ClientUtils.ExecuteSQL(programInfo.sSQL["OUTPUT MACHINE"], Params);
                ds = ClientUtils.ExecuteSQL(s, Params.ToArray());

                if (gvMachine.ColumnCount == 0)
                {
                    DataGridViewCheckBoxColumn dcCheck = new DataGridViewCheckBoxColumn
                    {
                        HeaderText = SajetCommon.SetLanguage("Select"),
                        Width = 40,
                        Name = "CHECKED"
                    };

                    gvMachine.Columns.Add(dcCheck);

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
            }
            else
            {
                string s = @"
SELECT
    B.MACHINE_CODE
   ,D.STATUS_NAME
   ,D.RUN_FLAG
   ,D.DEFAULT_STATUS
   ,B.MACHINE_ID
FROM
    SAJET.G_RC_TRAVEL_MACHINE A
   ,SAJET.G_RC_STATUS E
   ,SAJET.SYS_MACHINE B
   ,SAJET.G_MACHINE_STATUS C
   ,SAJET.SYS_MACHINE_STATUS D
WHERE
    A.TRAVEL_ID = E.TRAVEL_ID
AND A.RC_NO = E.RC_NO
AND E.RC_NO = :RC_NO
AND A.MACHINE_ID = B.MACHINE_ID
AND A.MACHINE_ID = C.MACHINE_ID
AND C.CURRENT_STATUS_ID = D.STATUS_ID
AND B.ENABLED = 'Y'
ORDER BY
    B.MACHINE_CODE
";

                Params = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", txtInput.Text }
                };

                //ds = ClientUtils.ExecuteSQL(programInfo.sSQL["機台"], Params);
                ds = ClientUtils.ExecuteSQL(s, Params.ToArray());
                
                if (gvMachine.ColumnCount == 0)
                {
                    for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                    {
                        gvMachine.Columns.Add(ds.Tables[0].Columns[i].ColumnName, SajetCommon.SetLanguage(ds.Tables[0].Columns[i].ColumnName, 1));
                        
                        gvMachine.Columns[gvMachine.Columns.Count - 1].HeaderText = SajetCommon.SetLanguage(gvMachine.Columns[i].HeaderText, 1);
                        
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
            }
            //*/
            #endregion

            #region Piece

            Params = new List<object[]>()
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", TstbInput.Text }
            };

            ds = ClientUtils.ExecuteSQL(programInfo.sSQL["元件"], Params.ToArray());

            foreach (DataColumn dc in ds.Tables[0].Columns)
            {
                DataGridViewTextBoxColumn dText = new DataGridViewTextBoxColumn
                {
                    Name = dc.ColumnName,
                    HeaderText = SajetCommon.SetLanguage(dc.ColumnName, 1),
                    ReadOnly = true
                };

                DgvSN.Columns.Add(dText);
            }

            Params = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", programInfo.dsRC.Tables[0].Rows[0]["PROCESS_ID"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", programInfo.dsRC.Tables[0].Rows[0]["PART_ID"].ToString() }
            };

            programInfo.dsSNParam = ClientUtils.ExecuteSQL(programInfo.sSQL["元件製程參數"], Params.ToArray());

            int iRow = 0;

            programInfo.iSNInput[0] = DgvSN.Columns.Count;

            programInfo.iSNInput[1] = -1;

            foreach (DataRow dr in programInfo.dsSNParam.Tables[0].Rows)
            {
                if (programInfo.iSNInput[1] == -1 && dr["ITEM_TYPE"].ToString() == "3")
                {
                    programInfo.iSNInput[1] = DgvSN.Columns.Count;
                }

                switch (dr["INPUT_TYPE"].ToString())
                {
                    case "S":
                        {
                            DataGridViewComboBoxColumn dComb = new DataGridViewComboBoxColumn
                            {
                                Name = dr["ITEM_NAME"].ToString(),
                                HeaderText = dr["ITEM_NAME"].ToString(),
                                ReadOnly = dr["ITEM_TYPE"].ToString() == "2",
                                Tag = dr["ITEM_ID"].ToString()
                            };

                            string[] slValue = dr["VALUE_LIST"].ToString().Split(',');

                            if (dr["NECESSARY"].ToString() == "Y")
                            {
                                dComb.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 192);
                            }
                            else
                            {
                                dComb.Items.Add("");
                            }

                            for (int i = 0; i < slValue.Length - 1; i++)
                            {
                                dComb.Items.Add(slValue[i]);
                            }

                            DgvSN.Columns.Add(dComb);

                            break;
                        }
                    default:
                        {
                            DataGridViewTextBoxColumn dText = new DataGridViewTextBoxColumn
                            {
                                Name = dr["ITEM_NAME"].ToString(),
                                HeaderText = dr["ITEM_NAME"].ToString(),
                                ReadOnly = dr["ITEM_TYPE"].ToString() == "2",
                                Tag = dr["ITEM_ID"].ToString()
                            };

                            if (dr["NECESSARY"].ToString() == "Y")
                            {
                                dText.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 192);
                            }

                            DgvSN.Columns.Add(dText);

                            break;
                        }
                }

                iRow++;
            }

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                DgvSN.Rows.Add();

                foreach (DataColumn dc in ds.Tables[0].Columns)
                {
                    DgvSN.Rows[DgvSN.Rows.Count - 1].Cells[dc.ColumnName].Value = dr[dc.ColumnName].ToString();
                }

                foreach (DataRow drParam in programInfo.dsSNParam.Tables[0].Rows)
                {
                    DgvSN.Rows[DgvSN.Rows.Count - 1].Cells[drParam["ITEM_NAME"].ToString()].Value = drParam["VALUE_DEFAULT"].ToString();

                    if (drParam["NECESSARY"].ToString() == "Y" && drParam["ITEM_TYPE"].ToString() == "3")
                    {
                        DgvSN.Rows[DgvSN.Rows.Count - 1].Cells[drParam["ITEM_NAME"].ToString()].ErrorText = SajetCommon.SetLanguage("Data Empty", 1);
                    }
                }
            }

            foreach (DataGridViewColumn col in DgvSN.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            #endregion

            #region Keyparts Collection

            // Show BOM information
            DgvBOM.Columns.Clear();

            DgvBOM.Columns.Add("PART_NO", SajetCommon.SetLanguage("PART_NO", 1));

            DgvBOM.Columns.Add("SPEC1", SajetCommon.SetLanguage("SPEC1", 1));

            DgvBOM.Columns.Add("ITEM_GROUP", SajetCommon.SetLanguage("ITEM_GROUP", 1));

            //gvBOM.Columns.Add("IS_MATERIAL", SajetCommon.SetLanguage("IS_MATERIAL", 1));    // 增加物料清單是否為物料FLAG欄位

            DgvBOM.Columns.Add("PURCHASE", SajetCommon.SetLanguage("PURCHASE", 1));           //  是否為外購作為檢查物料序號的依據

            DgvBOM.Columns.Add("QTY", SajetCommon.SetLanguage("QTY", 1));

            Params = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", programInfo.dsRC.Tables[0].Rows[0]["PROCESS_ID"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", programInfo.dsRC.Tables[0].Rows[0]["WO_OPTION2"].ToString() }
            };

            ds = ClientUtils.ExecuteSQL(programInfo.sSQL["KEYPARTS COLLECTION"], Params.ToArray());

            if (ds.Tables[0].Rows.Count == 0)
            {
                // if sys_bom doesn't have assembly data then invisible Keypart collection tabpage. 
                this.TpKeyparts.Parent = null;

                //SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Add BOM data."), 0);
            }
            else
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    DgvBOM.Rows.Add();

                    foreach (DataColumn dc in ds.Tables[0].Columns)
                    {
                        DgvBOM.Rows[DgvBOM.Rows.Count - 1].Cells[dc.ColumnName].Value = dr[dc.ColumnName].ToString();

                        DgvBOM.Columns[dc.ColumnName].ReadOnly = true;
                    }

                    DgvBOM.Rows[DgvBOM.Rows.Count - 1].Cells[DgvBOM.Columns.Count - 1].Value = "0";
                }
            }

            DgvBOM.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Build Keyparts column
            DgvKeypart.Columns.Clear();

            DgvKeypart.Columns.Add("PART_NO", SajetCommon.SetLanguage("PART_NO", 1));

            DgvKeypart.Columns.Add("RC_NO", SajetCommon.SetLanguage("RC_NO", 1));

            DgvKeypart.Columns.Add("ITEM_COUNT", SajetCommon.SetLanguage("ITEM_COUNT", 1));

            DgvKeypart.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            foreach (DataGridViewColumn col in DgvBOM.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            foreach (DataGridViewColumn col in DgvKeypart.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            # endregion
        }

        private bool CheckInput()
        {
            var Params = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TREV", TstbInput.Text },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TSHEET", programInfo.sFunction },
                new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" }
            };
            DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_RC_CHK_ROUTE", Params.ToArray());

            if (ds.Tables[0].Rows[0]["TRES"].ToString() != "OK")
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage(ds.Tables[0].Rows[0]["TRES"].ToString()), 0);

                return false;
            }

            return true;
        }

        private bool CheckBOM(string sRC)
        {
            try
            {
                // check input keypart is duplicate
                for (int i = 0; i < DgvKeypart.Rows.Count; i++)
                {
                    if (DgvKeypart.Rows[i].Cells["RC_NO"].Value.ToString() == sRC)
                    {
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("Keypart SN is Duplicate."), 0);

                        return false;
                    }
                }

                if (DgvBOM.CurrentRow.Cells["PURCHASE"].Value.ToString() == "Y")  // 不為外購要檢查料件序號規則
                {
                    var Params = new List<object[]>
                    {
                        new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", sRC },
                        new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_NO", DgvBOM.CurrentRow.Cells["PART_NO"].Value.ToString() }
                    };

                    DataSet ds = ClientUtils.ExecuteSQL(programInfo.sSQL["CHECK KEYPART STATUS"], Params.ToArray());

                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("Keypart SN Error."), 0);

                        return false;
                    }
                    else
                    {
                        Params = new List<object[]>
                        {
                            new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", sRC },
                            new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", programInfo.dsRC.Tables[0].Rows[0]["PART_ID"].ToString() },
                            new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", programInfo.dsRC.Tables[0].Rows[0]["PROCESS_ID"].ToString() }
                        };

                        ds = ClientUtils.ExecuteSQL(programInfo.sSQL["FIND KEYPART DATA"], Params.ToArray());

                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            SajetCommon.Show_Message(SajetCommon.SetLanguage("Keypart is not in this BOM."), 0);

                            return false;
                        }
                        else
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                DgvKeypart.Rows.Add();

                                foreach (DataColumn dc in ds.Tables[0].Columns)
                                {
                                    DgvKeypart.Rows[DgvKeypart.Rows.Count - 1].Cells[dc.ColumnName].Value = dr[dc.ColumnName].ToString();
                                    //gvKeypart.Columns[dc.ColumnName].ReadOnly = true;
                                }

                                DgvKeypart.Rows[DgvKeypart.Rows.Count - 1].Cells["ITEM_COUNT"].Value = editCount.Text.Trim();
                                //gvKeypart.Columns["ITEM_COUNT"].ReadOnly = true;
                            }
                        }
                    }
                }
                else
                {
                    DgvKeypart.Rows.Add();

                    DgvKeypart.Rows[DgvKeypart.Rows.Count - 1].Cells["PART_NO"].Value = DgvBOM.CurrentRow.Cells["PART_NO"].Value.ToString();

                    DgvKeypart.Rows[DgvKeypart.Rows.Count - 1].Cells["RC_NO"].Value = editKPSN.Text.Trim();

                    DgvKeypart.Rows[DgvKeypart.Rows.Count - 1].Cells["ITEM_COUNT"].Value = editCount.Text.Trim();
                }

                DgvKeypart.ReadOnly = true;

                // add quanity by item part
                CountKPSN();

                DgvKeypart.Rows[DgvKeypart.Rows.Count - 1].Selected = true;

                return true;
            }
            catch (Exception)
            {
                CountKPSN();

                return false;
            }
        }

        /// <summary>
        /// 全部KPSN item_count在此料號加總
        /// </summary>
        private void CountKPSN()
        {
            for (int i = 0; i < DgvBOM.Rows.Count; i++)
            {
                double cnt = 0;

                for (int j = 0; j < DgvKeypart.Rows.Count; j++)
                {
                    if (DgvKeypart.Rows[j].Cells["PART_NO"].Value.ToString() == DgvBOM.Rows[i].Cells["PART_NO"].Value.ToString())
                    {
                        cnt = cnt + Convert.ToDouble(DgvKeypart.Rows[j].Cells["ITEM_COUNT"].Value.ToString());
                    }
                }

                DgvBOM.Rows[i].Cells["QTY"].Value = cnt;
            }
        }

        /// <summary>
        /// CHECK MASTER PART QTY ADD ITEM PART QTY IS ZERO
        /// </summary>
        /// <returns>TRUE:NOT ZERO, FALSE:ZERO</returns>
        private bool CheckKPSNInput()
        {
            try
            {
                double iQty = 0;

                string sGroup = string.Empty;

                for (int i = 0; i < DgvBOM.Rows.Count; i++)
                {
                    if (DgvBOM.Rows[i].Cells["ITEM_GROUP"].Value.ToString() == "0")
                    {
                        // 在sys_bom.is_material為N
                        //if (gvBOM.Rows[i].Cells["QTY"].Value.ToString() == "0" && gvBOM.Rows[i].Cells["IS_MATERIAL"].Value.ToString() == "N")
                        if (DgvBOM.Rows[i].Cells["QTY"].Value.ToString() == "0")
                        {
                            string message
                                = DgvBOM.Rows[i].Cells["PART_NO"].EditedFormattedValue.ToString()
                                + " "
                                + SajetCommon.SetLanguage("does not input Keypart");

                            SajetCommon.Show_Message(message, 0);

                            return false;
                        }

                        iQty = 0;
                    }
                    else
                    {
                        sGroup = DgvBOM.Rows[i].Cells["ITEM_GROUP"].Value.ToString();

                        for (int j = 0; j < DgvBOM.Rows.Count; j++)
                        {
                            if (sGroup == DgvBOM.Rows[j].Cells["ITEM_GROUP"].Value.ToString())
                            {
                                iQty += Convert.ToDouble(DgvBOM.Rows[j].Cells["QTY"].Value.ToString());
                            }
                        }

                        if (iQty == 0)
                        {
                            string message
                                = DgvBOM.Rows[i].Cells["PART_NO"].EditedFormattedValue.ToString()
                                + " "
                                + SajetCommon.SetLanguage("does not input Keypart");

                            SajetCommon.Show_Message(message, 0);

                            return false;
                        }

                        iQty = 0;
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 檢查資料收集的數值
        /// </summary>
        /// <param name="data_item"></param>
        private void DgvInputCheck(DataCollectModel data_item)
        {
            data_item.ERROR_TEXT = string.Empty;

            string input_value = data_item.INPUT_VALUE;

            switch (data_item.CONVERT_TYPE)
            {
                case "U":
                    {
                        data_item.INPUT_VALUE = input_value.ToUpper();

                        break;
                    }
                case "L":
                    {
                        data_item.INPUT_VALUE = input_value.ToLower();

                        break;
                    }
            }

            switch (data_item.INPUT_TYPE)
            {
                case "R":
                    {
                        var valueList = data_item.VALUE_LIST.Split(',');

                        if (decimal.TryParse(valueList[0], out decimal dMin) &&
                            decimal.TryParse(valueList[1], out decimal dMax) &&
                            decimal.TryParse(input_value, out decimal dValue))
                        {
                            if (dValue < dMin || dValue > dMax)
                            {
                                data_item.ERROR_TEXT = string.Format(SajetCommon.SetLanguage("Over flow{0}~{1}", 1), dMin, dMax);
                            }
                        }
                        else if (data_item.NECESSARY.ToUpper() == "Y")
                        {
                            data_item.ERROR_TEXT = SajetCommon.SetLanguage("Data Invalid", 1);
                        }

                        break;
                    }
                default:
                    {
                        if (data_item.VALUE_TYPE.ToUpper() == "N")
                        {
                            if (!decimal.TryParse(input_value, out decimal dValue) &&
                                data_item.NECESSARY.ToUpper() == "Y")
                            {
                                data_item.ERROR_TEXT = SajetCommon.SetLanguage("Data Invalid", 1);
                            }
                        }

                        break;
                    }
            }

            if (string.IsNullOrEmpty(data_item.ERROR_TEXT) &&
                data_item.NECESSARY.ToUpper() == "Y" &&
                string.IsNullOrWhiteSpace(data_item.INPUT_VALUE))
            {
                data_item.ERROR_TEXT = SajetCommon.SetLanguage("Data Empty", 1);
            }
        }

        /// <summary>
        /// 檢查資料收集的數值
        /// </summary>
        private void DgvInputCheck(string runcard, string itemName, string value)
        {
            var Item = RC_Details.First(x => x.RC_NO == runcard)
                .DataCollections
                .First(x => x.ITEM_NAME == itemName);

            Item.ERROR_TEXT = string.Empty;

            Item.INPUT_VALUE = value;

            switch (Item.CONVERT_TYPE)
            {
                case "U":
                    {
                        Item.INPUT_VALUE = value.ToUpper();

                        break;
                    }
                case "L":
                    {
                        Item.INPUT_VALUE = value.ToLower();

                        break;
                    }
            }

            switch (Item.INPUT_TYPE)
            {
                case "R":
                    {
                        var valueList = Item.VALUE_LIST.Split(',');

                        if (decimal.TryParse(valueList[0], out decimal dMin) &&
                            decimal.TryParse(valueList[1], out decimal dMax) &&
                            decimal.TryParse(value, out decimal dValue))
                        {
                            if (dValue < dMin || dValue > dMax)
                            {
                                Item.ERROR_TEXT = string.Format(SajetCommon.SetLanguage("Over flow{0}~{1}", 1), dMin, dMax);
                            }
                        }
                        else if (Item.NECESSARY.ToUpper() == "Y")
                        {
                            Item.ERROR_TEXT = SajetCommon.SetLanguage("Data Invalid", 1);
                        }

                        break;
                    }
                default:
                    {
                        if (Item.VALUE_TYPE.ToUpper() == "N")
                        {
                            if (!decimal.TryParse(value, out decimal dValue) &&
                                Item.NECESSARY.ToUpper() == "Y")
                            {
                                Item.ERROR_TEXT = SajetCommon.SetLanguage("Data Invalid", 1);
                            }
                        }

                        break;
                    }
            }

            if (string.IsNullOrEmpty(Item.ERROR_TEXT) &&
                Item.NECESSARY.ToUpper() == "Y" &&
                string.IsNullOrWhiteSpace(Item.INPUT_VALUE))
            {
                Item.ERROR_TEXT = SajetCommon.SetLanguage("Data Empty", 1);
            }
        }

        private void ClearData()//無該筆SN時清空資料
        {
            for (int i = 0; i < m_tControlData.Length; i++)
            {
                m_tControlData[i].txtControl.Text = string.Empty;
            }

            DgvCondition.DataSource = null;

            DgvMachine.Rows.Clear();

            DgvSN.Rows.Clear();

            DgvSN.Columns.Clear();

            DgvInput.Rows.Clear();
        }

        /// <summary>
        /// 載入同工單同製程位置的流程卡
        /// </summary>
        public void RCsDataGrid()
        {
            string RC = TstbInput.Text;

            string SQL = $@"
SELECT
    WORK_ORDER
   ,PROCESS_ID
   ,ROUTE_ID
FROM
    SAJET.G_RC_STATUS
WHERE
    RC_NO = '{RC}'
";
            DataSet ds = ClientUtils.ExecuteSQL(SQL);

            string workOrder = ds.Tables[0].Rows[0]["WORK_ORDER"].ToString();

            string processID = ds.Tables[0].Rows[0]["PROCESS_ID"].ToString();

            string routeID = ds.Tables[0].Rows[0]["ROUTE_ID"].ToString();

            // 只要待生產狀態的流程卡 (Queue)
            SQL = $@"
SELECT
    1 ""CHECK""
   ,RC_NO
   ,CURRENT_QTY
   ,CURRENT_QTY ""GOOD_QTY""
   ,0 ""SCRAP_QTY""
   ,INITIAL_QTY
   ,0 ""REWORK_QTY""
   ,0 ""BONUS""
   ,1 ""WORK_HOUR""
   ,PROCESS_ID
FROM
    SAJET.G_RC_STATUS
WHERE
    WORK_ORDER = '{workOrder}'
AND ROUTE_ID     = {routeID}
AND PROCESS_ID   = {processID}
AND CURRENT_STATUS = 0
ORDER BY
    RC_NO
";
            RCSource = ClientUtils.ExecuteSQL(SQL);

            DgvRC.DataSource = RCSource;

            DgvRC.DataMember = RCSource.Tables[0].ToString();

            SelectRC(TstbInput.Text);
        }

        /// <summary>
        /// 把刷入的那張 RC 打勾
        /// </summary>
        /// <param name="RC"></param>
        private void SelectRC(string RC)
        {
            var row = RCSource.Tables[0].Rows.Cast<DataRow>()
                .FirstOrDefault(r => r["RC_NO"].ToString().Equals(RC));

            if (row != null)
            {
                row["CHECK"] = 1;
            }
            else
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("No such runcard."), 0);
            }

            CountRC();
        }

        /// <summary>
        /// 更新主畫面的數量資料
        /// </summary>
        private int CountRC()
        {
            int goodTotal = 0, scrapTotal = 0;

            LbCount.Text = DgvRC.Rows.OfType<DataGridViewRow>().Sum(r => r.Cells["CHECK"].Value.ToString() == "1" ? 1 : 0).ToString();

            if (!RC_Details.Any())
            {
                return 0;
            }

            foreach (DataGridViewRow r in DgvRC.Rows)
            {
                int iGood = 0, iScrap = 0;

                string RC = r.Cells["RC_NO"].Value.ToString();

                var row = RC_Details
                    .FirstOrDefault(m => m.RC_NO == RC);

                r.Cells["SCRAP_QTY"].Value = iScrap;

                iGood = int.Parse(r.Cells["CURRENT_QTY"].Value.ToString()) - iScrap;

                r.Cells["GOOD_QTY"].Value = iGood;

                if (r.Cells["CHECK"].Value.ToString() == "0")
                {
                    row.SELECT = false;
                }
                else
                {
                    row.SELECT = true;

                    goodTotal += iGood;

                    scrapTotal += iScrap;
                }
            }

            DgvRC.Refresh();

            return goodTotal;
        }

        /// <summary>
        /// 建立流程卡資料的清單
        /// </summary>
        /// <returns></returns>
        private List<RC_DETAILS> CreateRCsList()
        {
            // 建立已勾選的 RC 的新清單，每個 RC 有自己的不良現象清單
            var temp = (from row in RCSource.Tables[0].Rows.Cast<DataRow>()
                        select new RC_DETAILS
                        {
                            SELECT = row["CHECK"].ToString() == "1",

                            RC_NO = row["RC_NO"].ToString(),
                            CURRENT_QTY = int.Parse(row["CURRENT_QTY"].ToString()),
                            REWORK_QTY = decimal.Parse(row["REWORK_QTY"].ToString()),

                            IN_PROCESS_TIME = IN_PROCESS_TIME,

                            Machines = MachineList
                            .Select(x => new MachineDownModel(x))
                            .ToList(),

                            DataCollections = CollectionItems
                            .Select(e => new DataCollectModel(e))
                            .ToList(),

                        }).ToList();

            // 讓新建立的清單繼承已經填寫的資料
            if (RC_Details != null && RC_Details.Count > 0
                && temp != null && temp.Count > 0)
            {
                foreach (var RC in RC_Details)
                {
                    // 製程機台
                    temp.First(x => x.RC_NO == RC.RC_NO).Machines
                        = RC.Machines.Select(x => new MachineDownModel(x))
                        .ToList();

                    // 製程參數
                    temp.First(x => x.RC_NO == RC.RC_NO).DataCollections
                        = RC.DataCollections.Select(x => new DataCollectModel(x))
                        .ToList();

                    temp.First(x => x.RC_NO == RC.RC_NO).DateTimePicker_set
                        = RC.DateTimePicker_set;

                    temp.First(x => x.RC_NO == RC.RC_NO).IN_PROCESS_TIME
                        = RC.IN_PROCESS_TIME;
                }
            }

            return temp;
        }

        /// <summary>
        /// 取得流程卡的詳細資訊。
        /// </summary>
        /// <returns></returns>
        private DataRow GetRcNoInfo(string RC_NO)
        {
            string s = @"
SELECT
    *
FROM
    SAJET.G_RC_STATUS
WHERE
    RC_NO = :RC_NO
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", RC_NO },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            return d.Tables[0].Rows[0];
        }

        /// <summary>
        /// 機台資訊另外更新到變更機台記錄表。
        /// </summary>
        /// <param name="RcInfo">流程卡資訊</param>
        /// <param name="Machines">使用機台的清單</param>
        private void RecordMachine(DataRow RcInfo, List<MachineDownModel> Machines, DateTime now)
        {
            string s = string.Empty;

            int i = 0;

            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", RcInfo["RC_NO"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.Number, "TRAVEL_ID", RcInfo["TRAVEL_ID"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.Number, "NODE_ID", RcInfo["NODE_ID"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.DateTime, "NOW_TIME", now },
                new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", EmpID },
                new object[] { ParameterDirection.Input, OracleType.Number, "REPORT_TYPE", usingT4OrT6stove ? 2 : 1 },
            };

            int.TryParse(RcInfo["CURRENT_QTY"].ToString(), out int current_qty);

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
   ,:REPORT_TYPE
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
   ,2
   ,:REPORT_TYPE
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

        private bool Check_Privilege(string EmpNo)
        {
            string fun = "Set_WIP_Time";

            // 能不能使用工單報工模組
            try
            {
                string sPrivilege = ClientUtils.GetPrivilege(EmpID, fun, ClientUtils.fProgramName).ToString();

                string s = $@"
SELECT
    SAJET.SJ_PRIVILEGE_DEFINE('{fun}', '{sPrivilege}') TENABLED
FROM
    DUAL
";
                var d = ClientUtils.ExecuteSQL(s);

                string sRes = d.Tables[0].Rows[0]["TENABLED"].ToString();
                return (sRes == "Y");
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Function:SAJET.SJ_PRIVILEGE_DEFINE" + Environment.NewLine + ex.Message, 0);
                return false;
            }
        }

        /// <summary>
        /// 把共用不共用資料時，元件的屬性、事件設定，都放在這裡
        /// </summary>
        /// <param name="share_setting">共用資料與否（true：要共用資料/ false：不共用資料）</param>
        private void ShareSettingSwitch(bool share_setting)
        {
            CbShareSetting.CheckedChanged -= CbShareSetting_CheckedChanged;
            CbShareSetting_2.CheckedChanged -= CbShareSetting_2_CheckedChanged;

            CbShareSetting.Checked = share_setting;
            CbShareSetting_2.Checked = share_setting;

            CbShareSetting.CheckedChanged += CbShareSetting_CheckedChanged;
            CbShareSetting_2.CheckedChanged += CbShareSetting_2_CheckedChanged;
        }

        /// <summary>
        /// 重新佈置 DataGridView 各項屬性。
        /// </summary>
        /// <param name="x"></param>
        private void RearrangeDataGridView(ref DataGridView x)
        {
            x.Update();

            x.Refresh();

            x.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            x.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        /// <summary>
        /// 根據輸入的產品數量，勾選足夠的流程卡。未滿數量不勾
        /// </summary>
        /// <param name="i_rc_number"></param>
        private void CountAndTickCheckbox(int i_rc_number)
        {
            int i_total_click = 0;

            foreach (DataRow d_row in RCSource.Tables[0].Rows)
            {
                int.TryParse(d_row["CURRENT_QTY"].ToString(), out int i_current_qty);

                i_total_click += i_current_qty;

                if (i_rc_number >= i_total_click)
                {
                    d_row["CHECK"] = 1;
                }
                else
                {
                    d_row["CHECK"] = 0;
                }
            }
        }
    }
}