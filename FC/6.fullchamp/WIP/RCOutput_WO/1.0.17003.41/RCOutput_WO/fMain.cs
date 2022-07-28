using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SajetClass;
using System.Data.OracleClient;
using System.Text.RegularExpressions;//導入命名空間(正則表達式)
using RCOutput_WO.Models;
using OtSrv = RCOutput_WO.Services.OtherService;
using T4Srv = RCOutput_WO.Services.T4Service;
using RcSrv = RCOutput_WO.Services.RuncardService;
using RCOutput_WO.Enums;

namespace RCOutput_WO
{
    public partial class fMain : Form
    {
        string m_nParam;

        /// <summary>
        /// Y: Lot Control, N:Piece
        /// </summary>
        public string g_SystemType;

        /// <summary>
        /// 判定良品數是否為手動輸入
        /// </summary>
        public bool btxtGood = false;

        TProgramInfo programInfo = new TProgramInfo
        {
            iSNInput = new int[2],
            sFunction = "RC Output",
            sSQL = new Dictionary<string, string>(),
            iInputField = new Dictionary<string, List<int>>(),
            sOption = new Dictionary<string, string>(),
            iInputVisible = new Dictionary<string, int>(),
            slDefect = new Dictionary<string, int>(),
        };

        TControlData[] m_tControlData;

        TRCroute rcRoute = new TRCroute();

        DateTime OUT_PROCESS_TIME;

        /// <summary>
        /// 判定此製程 sheet 是否為 In & Out, 值為 false; 或者單獨 Out, 值為 True
        /// </summary>
        bool bOneSheet = false;

        public DataSet RCSource;

        /// <summary>
        /// 所有流程卡的資料
        /// </summary>
        public List<RC_DETAILS> RcDetails = new List<RC_DETAILS>();

        /// <summary>
        /// 資料收集清單，含有所有項目與設定，但不含輸入值。
        /// </summary>
        private List<DataCollectModel> CollectionItems = new List<DataCollectModel>();

        /// <summary>
        /// 不良現象清單
        /// </summary>
        private List<Defect> DefectItems = new List<Defect>();

        List<string> DateGridViewComboBoxItems = new List<string>();

        /// <summary>
        /// 使用者目前選取的流程卡
        /// </summary>
        private RC_DETAILS CurrentRuncard = null;

        private string Current_RC_NO = "";

        private string EmpID = "";

        /// <summary>
        /// 是否共用機台設定
        /// </summary>
        private bool ShareMachineSettings = false;

        /// <summary>
        /// 使否使用到 T4 / T6 爐
        /// </summary>
        private bool usingT4OrT6stove = false;

        public fMain() : this("", "", "", "") { }
        public fMain(string nParam, string strSN, string sEmp, string sTime)
        {
            InitializeComponent();
            m_nParam = nParam;
            this.Text = m_nParam;
            TbInput.Text = strSN;
            TslEmp.Text = sEmp;
            TbInput.Visible = false;
            TslParam.Visible = false;
            toolStripSeparator2.Visible = false;
            SsMessage.Visible = false;

            #region 依照數量勾選流程卡

            TbCheckRC.KeyDown += TbCheckRC_KeyDown;

            #endregion

            #region 載入爐順序號碼的 下拉選單

            DateGridViewComboBoxItems = OtSrv.GetSequencesComboBoxItems();

            sTOVESEQDataGridViewTextBoxColumn.DataSource = DateGridViewComboBoxItems;

            #endregion

            DgvMachine.EditingControlShowing += DgvMachine_EditingControlShowing;

            DgvRC.EditingControlShowing += DgvRC_EditingControlShowing;

            #region 報工時間

            DtpOutDate.DropDown += DtpOutDate_DropDown;

            CbReportTime.CheckStateChanged += CbReportTime_CheckStateChanged;

            #endregion
        }

        private void FMain_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TslEmp.Text))
            {
                TslEmp.Text = ClientUtils.fLoginUser;
                programInfo.sProgram = ClientUtils.fProgramName;
                programInfo.sFunction = ClientUtils.fFunctionName;
                BtnCancel.Visible = false;
            }
            else
            {
                programInfo.sProgram = ClientUtils.fProgramName;
                programInfo.sFunction = m_nParam; // ClientUtils.fFunctionName;
                EmpID = OtSrv.GetEmpID(TslEmp.Text);
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
                                    ReadOnly = true
                                };
                                if (slEdit.IndexOf(slValue[i]) != -1)
                                {
                                    txtTemp.BackColor = Color.FromArgb(255, 255, 192);
                                    if (iField == 1)
                                    {
                                        programInfo.txtGood = txtTemp;
                                    }
                                    else
                                    {
                                        programInfo.txtScrap = txtTemp;
                                    }
                                }
                                txtTemp.Dock = DockStyle.Fill;
                                txtTemp.Name = "txt" + slValue[i];
                                txtTemp.Font = new Font(txtTemp.Font, FontStyle.Bold);
                                TlpInfo.Controls.Add(txtTemp, iField, iRow);
                                m_tControlData[iFieldCount].txtControl = txtTemp;
                            }
                            m_tControlData[iFieldCount].sFieldName = slValue[i];
                            iRow++;
                            iFieldCount++;
                        }
                        iField += 2;
                        break;
                    case "LABEL":
                        iRow = 0;
                        slValue = dr["PARAM_VALUE"].ToString().Split(',');
                        Label lablTemp;
                        for (int i = 0; i < slValue.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(slValue[i]))
                            {
                                lablTemp = new Label();
                                lablTemp.Font = new Font(lablTemp.Font, FontStyle.Bold);
                                lablTemp.Text = slValue[i];
                                lablTemp.TextAlign = ContentAlignment.MiddleLeft;
                                lablTemp.Dock = DockStyle.Fill;
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
                    case "SQL":
                        programInfo.sSQL.Add(dr["DEFAULT_VALUE"].ToString(), dr["PARAM_VALUE"].ToString());
                        if (!string.IsNullOrEmpty(dr["PARAM_DESC"].ToString()))
                        {
                            string[] sSplit = dr["PARAM_DESC"].ToString().Split(';');
                            programInfo.iInputVisible.Add(dr["DEFAULT_VALUE"].ToString(), int.Parse(sSplit[0]));
                            if (sSplit.Length == 3)
                            {
                                programInfo.sOption.Add(dr["DEFAULT_VALUE"].ToString(), sSplit[2]);
                            }
                            if (sSplit.Length >= 2 && !string.IsNullOrEmpty(sSplit[1]))
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

            SajetCommon.SetLanguageControl(this);

            TbInput.TextChanged += new EventHandler(TbInput_TextChanged);

            // Depend system type to show tabpage
            if (OtSrv.SystemType(system_type: out g_SystemType))
            {
                // lot control
                this.TpSN.Parent = null;
            }

            #region 人工設定出站時間

            OUT_PROCESS_TIME = OtSrv.GetDBDateTimeNow();

            DtpOutDate.Value = OUT_PROCESS_TIME;

            bool CanSetWipTime = OtSrv.Check_Privilege(EmpID: EmpID, fun: "Set_WIP_Time");

            if (CanSetWipTime)
            {
                TpOutTime.Parent = TcParams;
            }
            else
            {
                TpOutTime.Parent = null;
            }

            #endregion

            #region 強調顯示 舊編(OPTION2) 藍圖(OPTION4)

            sSQL = $@"
SELECT A.OPTION2 FORMER_NO
      ,A.OPTION4 BLUEPRINT
FROM SAJET.SYS_PART A
    ,SAJET.G_RC_STATUS B
WHERE A.PART_ID = B.PART_ID
AND B.RC_NO = '{TbInput.Text}'
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

            #region 檢查製程是否使用 T4 / T6 爐，要收集爐號

            var messages = new List<string>();

            usingT4OrT6stove = T4Srv.CheckIfUsingT4orT6(rc_no: TbInput.Text, messages: out messages);

            #region 共用資料設定的功能先停用
            /*
            ShareMachineSettings = IsMachineSettingShared();

            CbShareSetting.Checked = ShareMachineSettings;

            CHECK.Visible = !ShareMachineSettings;
            //*/
            #endregion

            // 設定使用 T4 / T6 爐的製程必須蒐集爐序、日期碼，不蒐集數量
            sTOVESEQDataGridViewTextBoxColumn.Visible = false;

            dATECODEDataGridViewTextBoxColumn.Visible = usingT4OrT6stove;

            //CbShareSetting.Checked = usingT4OrT6stove;

            //ShareMachineSettings = usingT4OrT6stove;

            CbShareSetting.Enabled = false;

            LbT4T6.Text = string.Join(Environment.NewLine, messages);

            sTOVESEQDataGridViewTextBoxColumn.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 192);

            dATECODEDataGridViewTextBoxColumn.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 192);

            #endregion

            // 取得流程卡各項設定項目，帶入到畫面
            // 獨立設定的項目，會在選取流程卡後，才帶入
            if (!string.IsNullOrEmpty(TbInput.Text) && CheckInput())
            {
                ClearData();
                ShowData();
            }

            // 取得所有製程不良現象（項目，與設定數量無關）
            DefectItems = CreateDefectList();

            // 所有流程卡的資料模型
            RcDetails = CreateRCsList(DefectItems);

            // 選取第一筆流程卡的資料，帶入到右邊的設定畫面
            DgvRC_CellClick(null, null);

            #region 只有設定有投入 / 產出兩階段的製程站，可以切換機台、設定機台異常代碼

            if (bOneSheet || !OtSrv.Check_Privilege(EmpID: EmpID, fun: "Machine_Change"))
            {
                BtnMachineChange.Enabled = false;

                DgvMachine.Columns[nameof(sTARTTIMEDataGridViewTextBoxColumn)].Visible = false;
            }

            #endregion

            // 用不到 Bonus 和 工時 的設定，所以把頁籤隱藏起來
            TpBonus.Parent = null;
        }

        private void FMain_Shown(object sender, EventArgs e)
        {
            ScMain.SplitterDistance = 500;
            ScParam.SplitterDistance = Convert.ToInt32(ScParam.Height * 0.5);

            this.Text += $" ({SajetCommon.g_sFileName} : {SajetCommon.g_sFileVersion})";

            #region 強調顯示的字

            var new_font_1 = new Font("Microsoft JhengHei", 30, FontStyle.Bold);
            var new_font_2 = new Font("Microsoft JhengHei", 20, FontStyle.Bold);

            //LbProcess.Font = new_font_1;
            LbBluePrint.Font = new_font_1;
            LbFormerNO.Font = new_font_1;

            TslForm.Font = new_font_2;

            TControlData process_name_control_set =
                m_tControlData
                .First(x => x.sFieldName == "PROCESS_NAME");

            process_name_control_set.txtControl.BackColor = Color.Khaki;
            process_name_control_set.lablControl.ForeColor = Color.Red;

            #endregion

            #region 顯示工單備註
            string SQL = $@"
SELECT B.REMARK
FROM SAJET.G_RC_STATUS A
    ,SAJET.G_WO_BASE B
WHERE A.WORK_ORDER = B.WORK_ORDER
AND A.RC_NO = '{TbInput.Text}'
";
            DataSet set = ClientUtils.ExecuteSQL(SQL);
            if (set != null && set.Tables[0].Rows.Count > 0)
            {
                TControlData remark_control_set =
                    m_tControlData
                    .First(x => x.sFieldName == "REMARK");

                remark_control_set.txtControl.Text = set.Tables[0].Rows[0]["REMARK"].ToString();
            }

            #endregion

            #region 製程參數欄位的寬度

            // 資料收集
            DgvInput.Columns["VALUE_DEFAULT"].HeaderText = SajetCommon.SetLanguage("Input Value");

            int w0 = DgvInput.Controls.OfType<VScrollBar>()
                .Where(x => x.Visible)
                .Select(x => x.Width)
                .DefaultIfEmpty(0)
                .FirstOrDefault();

            int w1 = groupBox1.Width - 45 - w0;
            int w2 = DgvInput.Columns["UNIT_NO"].Width = 43;

            DgvInput.Columns["UNIT_NO"].Width = w2;

            int w3 = (w1 - w2) / 2;

            DgvInput.Columns["VALUE_DEFAULT"].Width
                = DgvInput.Columns["ITEM_NAME"].Width = w3;

            // 製程條件
            w0 = DgvCondition.Controls.OfType<VScrollBar>()
                .Where(x => x.Visible)
                .Select(x => x.Width)
                .DefaultIfEmpty(0)
                .FirstOrDefault();

            w1 = groupBox2.Width - 45 - w0;

            w2 = DgvCondition.Columns["UNIT_NO"].Width = 43;

            DgvCondition.Columns["UNIT_NO"].Width = w2;

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
                i_position_1 = label8.Right;

                if (CbReportTime.Right > i_position_1)
                {
                    i_position_1 = CbReportTime.Right;
                }

                i_position_2 = LbLastReportTime.Left;

                if (DtpOutDate.Left > i_position_2)
                {
                    i_position_2 = DtpOutDate.Left;
                }

                if (i_position_1 > i_position_2)
                {
                    LbLastReportTime.Left = i_position_1 + i_default_distance;

                    DtpOutDate.Left = i_position_1 + i_default_distance;
                }
            }

            // 重工數量
            {
                i_position_1 = label10.Right;

                i_position_2 = TbReworkQty.Left;

                if (i_position_1 > i_position_2)
                {
                    TbReworkQty.Left = i_position_1 + i_default_distance;
                }
            }

            // Bonus
            {
                i_position_1 = label2.Right;

                if (label3.Right > i_position_1)
                {
                    i_position_1 = label3.Right;
                }

                i_position_2 = TbBonus.Left;

                if (TbWH.Left > i_position_2)
                {
                    i_position_2 = TbWH.Left;
                }

                if (i_position_1 > i_position_2)
                {
                    TbBonus.Left = i_position_1 + i_default_distance;

                    TbWH.Left = i_position_1 + i_default_distance;
                }
            }

            #endregion

            #region 調整 Label 寬度

            if (TlpInfo.ColumnCount == 4)
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
                if (TlpInfo.ColumnCount == 4)
                {
                    int i_default_width = 25;
                    TlpInfo.ColumnStyles.Clear();
                    TlpInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, f_label_width[0] + i_default_width));
                    TlpInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                    TlpInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, f_label_width[1] + i_default_width));
                    TlpInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                }

            }

            #endregion

            #region T4 / T6 不使用勾選功能

            TbCheckRC.Enabled = !usingT4OrT6stove;

            BtnSelectAll.Enabled = !usingT4OrT6stove;

            BtnReset.Enabled = !usingT4OrT6stove;

            #endregion

            CountRC();
        }

        private void DgvSN_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // 2015/08/26, Aaron
            if (e.ColumnIndex == -1) return;
            if (DgvSN.Columns[e.ColumnIndex].Name == "CURRENT_STATUS")
            {
                decimal iGood = 0, iScrap = 0;
                foreach (DataGridViewRow dr in DgvSN.Rows)
                {
                    if (dr.Cells["CURRENT_STATUS"].EditedFormattedValue.ToString() == "OK")
                    {
                        iGood++;
                    }
                    else
                    {
                        iScrap++;
                    }
                }
                programInfo.txtGood.Text = iGood.ToString();
                programInfo.txtScrap.Text = iScrap.ToString();
            }
            else if (DgvSN.Columns[e.ColumnIndex].Name == "GOOD_QTY")
            {
                DataGridViewCell cell = DgvSN.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.ErrorText = "";
                try
                {
                    decimal dValue = decimal.Parse(cell.EditedFormattedValue.ToString());
                }
                catch
                {
                    cell.ErrorText = SajetCommon.SetLanguage("Data Invalid", 1);
                }
            }
            else if (DgvSN.Columns[e.ColumnIndex].Name == "SCRAP_QTY")
            {
                DataGridViewCell cell = DgvSN.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.ErrorText = "";
                try
                {
                    decimal dValue = decimal.Parse(cell.EditedFormattedValue.ToString());
                }
                catch
                {
                    cell.ErrorText = SajetCommon.SetLanguage("Data Invalid", 1);
                }
            }
            else
            {
                if (e.ColumnIndex < programInfo.iSNInput[1]) return;
                int iCol = e.ColumnIndex - programInfo.iSNInput[0] - 3;
                DataGridViewCell cell = DgvSN.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.ErrorText = "";
                //20150602 Nancy Modify
                if (programInfo.dsSNParam.Tables[0].Rows.Count > 0)
                {
                    switch (programInfo.dsSNParam.Tables[0].Rows[iCol]["CONVERT_TYPE"].ToString())
                    {
                        case "U":
                            cell.Value = cell.EditedFormattedValue.ToString().ToUpper();
                            break;
                        case "L":
                            cell.Value = cell.EditedFormattedValue.ToString().ToUpper();
                            break;
                    }
                    switch (programInfo.dsSNParam.Tables[0].Rows[iCol]["INPUT_TYPE"].ToString())
                    {
                        case "R":
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
                        default:
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

                    if (string.IsNullOrEmpty(cell.ErrorText)
                        && programInfo.dsSNParam.Tables[0].Rows[iCol]["NECESSARY"].ToString() == "Y"
                        && string.IsNullOrEmpty(cell.EditedFormattedValue.ToString()))
                    {
                        cell.ErrorText = SajetCommon.SetLanguage("Data Empty", 1);
                    }
                }
            }
        }

        private void DgvInput_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var row = DgvInput.Rows[e.RowIndex];
            DataGridViewCell cell = DgvInput.Rows[e.RowIndex].Cells[e.ColumnIndex];
            cell.ErrorText = "";

            if (CurrentRuncard != null)
            {
                DgvInputCheck(
                    runcard: CurrentRuncard.RC_NO,
                    itemName: row.Cells["ITEM_NAME"].Value.ToString(),
                    value: cell.Value?.ToString());
            }

            switch (DgvInput.Rows[e.RowIndex].Cells["CONVERT_TYPE"].EditedFormattedValue.ToString())
            {
                case "U":
                    cell.Value = cell.EditedFormattedValue.ToString().ToUpper();
                    break;
                case "L":
                    cell.Value = cell.EditedFormattedValue.ToString().ToUpper();
                    break;
            }
            switch (DgvInput.Rows[e.RowIndex].Cells["INPUT_TYPE"].EditedFormattedValue.ToString())
            {
                case "R":
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
                default:
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
                if (r.Cells["VALUE_TYPE"].Value.ToString() == "L")
                {
                    if (!Uri.IsWellFormedUriString(r.Cells["VALUE_TYPE"].Value.ToString(), UriKind.Absolute))
                    {
                        r.Cells["VALUE_DEFAULT"] = new DataGridViewLinkCell();
                    }
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
                    = LbCurrentRC3.Text
                    = LbCurrentRC4.Text
                    = LbCurrentRC5.Text
                    = LbCurrentRC6.Text
                    = SajetCommon.SetLanguage("No selected runcard.");

                TbBonus.Text = "0";
                TbWH.Text = "1";
                TbReworkQty.Text = "0";

                DtpOutDate.Value = OUT_PROCESS_TIME;

                return;
            }

            Current_RC_NO = DgvRC.CurrentRow.Cells["RC_NO"].Value?.ToString() ?? "";

            if (e?.RowIndex > -1 &&
                e?.ColumnIndex == CHECK.Index &&
                !string.IsNullOrWhiteSpace(Current_RC_NO) &&
                !ShareMachineSettings)
            {
                bool selected = (bool)DgvRC.Rows[e.RowIndex].Cells[CHECK.Index].EditedFormattedValue;

                DgvRC.Rows[e.RowIndex].Cells[CHECK.Index].Value = !selected;
            }

            var row = DgvRC.CurrentRow;

            // 當前流程卡
            CurrentRuncard = RcDetails.First(x => x.RC_NO == Current_RC_NO);

            // 顯示流程卡號
            LbCurrentRC1.Text
                = LbCurrentRC2.Text
                = LbCurrentRC3.Text
                = LbCurrentRC4.Text
                = LbCurrentRC5.Text
                = LbCurrentRC6.Text
                = SajetCommon.SetLanguage("RunCard")
                + SajetCommon.SetLanguage(":")
                + Current_RC_NO;

            // 顯示機台
            DgvMachine.DataSource = CurrentRuncard.Machines.ToList();

            // 顯示不良品
            DgvDefect.DataSource = CurrentRuncard.Defects.ToList();

            List<DataCollectModel> data_collection = CurrentRuncard.DataCollections;

            // 資料收集
            foreach (DataGridViewRow r in DgvInput.Rows)
            {
                string itemName = r.Cells["ITEM_NAME"].Value.ToString();

                DataCollectModel data_item = data_collection.First(x => x.ITEM_NAME == itemName);

                //DgvInputCheck(runcard: CurrentRuncard.RC_NO, itemName: itemName, value: data_item.INPUT_VALUE);
                DgvInputCheck(data_item: data_item);

                r.Cells["VALUE_DEFAULT"].Value = data_item.INPUT_VALUE;

                r.Cells["VALUE_DEFAULT"].ErrorText = data_item.ERROR_TEXT;
            }

            // 顯示設定的報工時間
            LbLastReportTime.Text = RcSrv.GetLastReportTime(Runcard: Current_RC_NO);

            #region 先把事件拔掉，避免 DateTimePicker 的值影響到其他資料

            DtpOutDate.ValueChanged -= DtpOutDate_ValueChanged;

            CbReportTime.CheckStateChanged -= CbReportTime_CheckStateChanged;

            DtpOutDate.Value = CurrentRuncard.OUTPUT_TIME;

            CbReportTime.Checked = CurrentRuncard.DateTimePicker_set;

            DtpOutDate.ValueChanged += DtpOutDate_ValueChanged;

            CbReportTime.CheckStateChanged += CbReportTime_CheckStateChanged;

            #endregion

            // Bonus / 工時
            TbBonus.Text = row.Cells["BONUS"].Value?.ToString();
            TbWH.Text = row.Cells["WORK_HOUR"].Value?.ToString();

            // 重工數量
            TbReworkQty.Text = row.Cells["REWORK_QTY"].Value?.ToString();

            DgvRC.RefreshEdit();
        }

        private void DgvRC_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= NumericTextBoxColumn_KeyPress;

            if (DgvRC.CurrentCell.ColumnIndex == DgvRC.Columns["REWORK_QTY"].Index)
            {
                if (e.Control is TextBox x)
                {
                    x.KeyPress += NumericTextBoxColumn_KeyPress;
                }
            }
        }

        #region  在編輯 LOAD_QTY 欄位或是 DATE_CODE 欄位的內容時，為了能檢查輸入值內容綁定的事件

        private void DgvMachine_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= NumericTextBoxColumn_KeyPress;

            e.Control.Leave -= DgvMachineTextBoxColumn_Leave;

            if (DgvMachine.CurrentCell.ColumnIndex == dATECODEDataGridViewTextBoxColumn.Index ||
                DgvMachine.CurrentCell.ColumnIndex == lOADQTYDataGridViewTextBoxColumn.Index ||
                DgvMachine.CurrentCell.ColumnIndex == sTOVESEQDataGridViewTextBoxColumn.Index)
            {
                if (e.Control is TextBox x)
                {
                    x.KeyPress += NumericTextBoxColumn_KeyPress;

                    x.Leave += DgvMachineTextBoxColumn_Leave;
                }

                if (e.Control is ComboBox z)
                {
                    z.Leave += DgvMachineTextBoxColumn_Leave;
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
        private void DgvMachineTextBoxColumn_Leave(object sender, EventArgs e)
        {
            string MachineCode = DgvMachine.Rows[DgvMachine.CurrentCell.RowIndex].Cells[nameof(mACHINECODEDataGridViewTextBoxColumn)].Value.ToString();

            bool selected = (bool)DgvMachine.CurrentRow.Cells[selectDataGridViewCheckBoxColumn.Index].Value;

            if (sender is DataGridViewTextBoxEditingControl z)
            {
                if (DgvMachine.CurrentCell.ColumnIndex == lOADQTYDataGridViewTextBoxColumn.Index &&
                    int.TryParse(z.Text, out int load) && load > 0)
                {
                    selected = true;

                    DgvMachine.CurrentRow.Cells[selectDataGridViewCheckBoxColumn.Index].Value = selected;

                    DgvRC.CurrentRow.Cells[CHECK.Index].Value = selected;

                    RcDetails
                        .First(x => x.RC_NO == Current_RC_NO)
                        .Machines
                        .First(x => x.MACHINE_CODE == MachineCode)
                        .LOAD_QTY = load;

                    // 是否共用機台資料
                    if (ShareMachineSettings)
                    {
                        RcDetails.ForEach(rc =>
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

                if (DgvMachine.CurrentCell.ColumnIndex == dATECODEDataGridViewTextBoxColumn.Index &&
                    !string.IsNullOrWhiteSpace(z.Text))
                {
                    selected = true;

                    DgvMachine.CurrentRow.Cells[selectDataGridViewCheckBoxColumn.Index].Value = selected;

                    DgvRC.CurrentRow.Cells[CHECK.Index].Value = selected;

                    string dateCode = z.Text.Trim();

                    RcDetails
                        .First(x => x.RC_NO == Current_RC_NO)
                        .Machines
                        .First(x => x.MACHINE_CODE == MachineCode)
                        .DATE_CODE = dateCode.ToString();

                    // 是否共用機台資料
                    if (ShareMachineSettings)
                    {
                        RcDetails.ForEach(rc =>
                        {
                            if (rc.SELECT)
                            {
                                var machine = rc
                                 .Machines
                                 .First(x => x.MACHINE_CODE == MachineCode);

                                machine.DATE_CODE = dateCode;

                                machine.Select = selected;
                            }
                        });
                    }
                }
            }
            else if (sender is DataGridViewComboBoxEditingControl c)
            {
                if (DgvMachine.CurrentCell.ColumnIndex == sTOVESEQDataGridViewTextBoxColumn.Index &&
                    !string.IsNullOrWhiteSpace(c.Text))
                {
                    selected = true;

                    DgvMachine.CurrentRow.Cells[selectDataGridViewCheckBoxColumn.Index].Value = selected;

                    DgvRC.CurrentRow.Cells[CHECK.Index].Value = selected;

                    RcDetails
                        .First(x => x.RC_NO == Current_RC_NO)
                        .Machines
                        .First(x => x.MACHINE_CODE == MachineCode)
                        .STOVE_SEQ = c.Text;

                    // 是否共用機台資料
                    if (ShareMachineSettings)
                    {
                        RcDetails.ForEach(rc =>
                        {
                            if (rc.SELECT)
                            {
                                var machine = rc
                                 .Machines
                                 .First(x => x.MACHINE_CODE == MachineCode);

                                machine.STOVE_SEQ = c.Text;

                                machine.Select = selected;
                            }
                        });
                    }
                }
            }

        }

        #endregion

        private void DtpOutDate_DropDown(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Current_RC_NO))
            {
                CurrentRuncard.DateTimePicker_set = true;

                CbReportTime.Checked = true;
            }
        }

        private void DtpOutDate_ValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Current_RC_NO))
            {
                return;
            }

            RcDetails.First(x => x.RC_NO == Current_RC_NO).OUTPUT_TIME = DtpOutDate.Value;

            RcDetails.First(x => x.RC_NO == Current_RC_NO).DateTimePicker_set = true;

            CbReportTime.CheckStateChanged -= CbReportTime_CheckStateChanged;

            CbReportTime.Checked = true;

            CbReportTime.CheckStateChanged += CbReportTime_CheckStateChanged;
        }

        private void CbReportTime_CheckStateChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Current_RC_NO))
            {
                return;
            }

            DtpOutDate.ValueChanged -= DtpOutDate_ValueChanged;

            if (CbReportTime.Checked)
            {
                RcDetails.First(x => x.RC_NO == Current_RC_NO).OUTPUT_TIME = DtpOutDate.Value;

                RcDetails.First(x => x.RC_NO == Current_RC_NO).DateTimePicker_set = true;
            }
            else
            {
                RcDetails.First(x => x.RC_NO == Current_RC_NO).OUTPUT_TIME = OUT_PROCESS_TIME;

                RcDetails.First(x => x.RC_NO == Current_RC_NO).DateTimePicker_set = false;

                DtpOutDate.Value = OUT_PROCESS_TIME;
            }

            DtpOutDate.ValueChanged += DtpOutDate_ValueChanged;
        }

        /// <summary>
        /// Input Keypart sn trigger event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditKPSN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return) return;
            //確認KPSN有輸入值
            if (string.IsNullOrEmpty(EditKPSN.Text))
            {
                string message = SajetCommon.SetLanguage("Keypart SN Error.");

                SajetCommon.Show_Message(message, 0);

                return;
            }
            else // 如果是廠內序號增加帶出目前數量
            {
                try
                {
                    string sSQL = @" SELECT CURRENT_QTY 
                                     FROM SAJET.G_RC_STATUS 
                                     WHERE  RC_NO = :RC_NO 
                                     AND ROWNUM = 1 ";
                    object[][] Params = new object[1][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", EditKPSN.Text.Trim() };
                    DataSet ds = ClientUtils.ExecuteSQL(sSQL, Params);
                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            EditCount.Text = ds.Tables[0].Rows[0]["CURRENT_QTY"].ToString();
                        }
                        else
                        {
                            EditCount.Text = "0";
                        }
                    }
                    else
                    {
                        EditCount.Text = "0";
                    }
                }
                catch (Exception)
                {
                    EditCount.Text = "0";
                }
            }
            EditCount.SelectAll();
            EditCount.Focus();
        }

        private void EditCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return) return;

            // 確認用量有輸入值
            if (string.IsNullOrEmpty(EditCount.Text))
            {
                string message = SajetCommon.SetLanguage("Keypart SN Error.");

                SajetCommon.Show_Message(message, 0);

                EditCount.SelectAll();
                EditCount.Focus();

                return;
            }
            else
            {
                // 確認用量正確性
                if (!OtSrv.IsNumeric(3, EditCount.Text))
                {
                    string message = SajetCommon.SetLanguage("Please Input Quantity in Positive number");

                    SajetCommon.Show_Message(message, 0);

                    EditCount.SelectAll();
                    EditCount.Focus();

                    return;
                }
            }
        }

        #region Bonus / 工時 只能填數字 按下 ENTER 檢查
        private void EditBonus_Click(object sender, EventArgs e)
        {
            TbBonus.SelectAll();
            TbBonus.Focus();
        }

        private void EditBonus_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)
                && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }

            // 小數點 / 負號只能有 1 個
            if (e.KeyChar == '-'
                && (sender as TextBox).Text.IndexOf('-') > -1)
            {
                e.Handled = true;
            }

            if (e.KeyChar == (char)Keys.Return)
            {
                // chkBonus 的檢查
                if (double.TryParse(new DataTable().Compute(TbBonus.Text, null).ToString(), out double dBonus))
                {
                    var row = DgvRC.Rows.Cast<DataGridViewRow>()
                         .First(m => m.Cells["RC_NO"].Value.ToString() == Current_RC_NO);
                    if (row != null)
                    {
                        double initialQty = double.Parse(row.Cells["INITIAL_QTY"].Value.ToString());
                        double currentQty = double.Parse(row.Cells["CURRENT_QTY"].Value.ToString());
                        double goodQty = double.Parse(row.Cells["GOOD_QTY"].Value.ToString());

                        if (initialQty > 0.0d && initialQty < goodQty + dBonus)
                        {
                            string message = SajetCommon.SetLanguage("More than initial quantity") + ": " + initialQty;

                            SajetCommon.Show_Message(message, 0);

                            row.Cells["BONUS"].Value = TbBonus.Text = "0";

                            return;
                        }

                        if (0 > initialQty + dBonus) // 定義 : 當Bonus數為負值，倒扣要大於0
                        {
                            string message = SajetCommon.SetLanguage("Result quantity less than zero.");

                            SajetCommon.Show_Message(message, 0);

                            row.Cells["BONUS"].Value = TbBonus.Text = "0";

                            return;
                        }

                        row.Cells["BONUS"].Value = TbBonus.Text = dBonus.ToString();
                    }
                    else
                    {
                        string message = SajetCommon.SetLanguage("No selected runcard.");

                        SajetCommon.Show_Message(message, 0);

                        TbBonus.Text = "0";

                        return;
                    }
                }
                else
                {
                    TbBonus.Text = dBonus.ToString();

                    string message = SajetCommon.SetLanguage("Not allowed input, required integer.");

                    SajetCommon.Show_Message(message, 0);

                    return;
                }
            }
        }

        private void EditWH_Click(object sender, EventArgs e)
        {
            TbWH.SelectAll();
            TbWH.Focus();
        }

        private void EditWH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            if (e.KeyChar == (char)Keys.Return)
            {
                if (!int.TryParse(TbWH.Text, out int dWH) || dWH <= 0)
                {
                    dWH = 1;
                    TbWH.Text = dWH.ToString();

                    string message = SajetCommon.SetLanguage("Not allowed input, required positive integer.");

                    SajetCommon.Show_Message(message, 0);
                }

                DgvRC.Rows.Cast<DataGridViewRow>()
                    .First(m => m.Cells["RC_NO"].Value.ToString() == Current_RC_NO)
                    .Cells["WORK_HOUR"].Value = TbWH.Text = dWH.ToString();
            }
        }

        #endregion

        #region 重工數量

        private void TbReworkQty_Click(object sender, EventArgs e)
        {
            TbReworkQty.SelectAll();
            TbReworkQty.Focus();
        }

        private void TbReworkQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TbReworkQty_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Current_RC_NO))
                CheckReworkQty(Current_RC_NO);
        }

        /// <summary>
        /// 檢查重工數量
        /// </summary>
        private bool CheckReworkQty(string rc)
        {
            var row = DgvRC.Rows.Cast<DataGridViewRow>()
                    .First(m => m.Cells["RC_NO"].Value.ToString() == rc);

            if (row == null)
            {
                string message = SajetCommon.SetLanguage("No selected runcard.");

                SajetCommon.Show_Message(message, 0);

                return false;
            }

            decimal reworkMaxValue = decimal.Parse(row.Cells["CURRENT_QTY"].Value.ToString());

            if (string.IsNullOrWhiteSpace(TbReworkQty.Text))
            {
                TbReworkQty.Text = "0";
            }

            if (decimal.TryParse(TbReworkQty.Text, out decimal input))
            {
                if (input > reworkMaxValue)
                {
                    ReworkMessage(rc, "Rework quantity exceed current quantity");

                    return false;
                }
                else if (input < 0)
                {
                    ReworkMessage(rc, "Rework quantity does not accept negative values");

                    return false;
                }
            }
            else
            {
                ReworkMessage(rc, "Input value is not a number");

                return false;
            }

            row.Cells["REWORK_QTY"].Value = TbReworkQty.Text = input.ToString();

            return true;
        }

        /// <summary>
        /// 把重工數量記錄在另外一張資料表
        /// </summary>
        /// <param name="rc_no"></param>
        /// <param name="node_id"></param>
        /// <param name="rework_qty"></param>
        private void SaveReworkQty(string rc_no, string node_id, string travel_id, decimal rework_qty = 0)
        {
            string S = @"
INSERT INTO
    SAJET.G_RC_TRAVEL_REWORK
(
    RC_NO
   ,NODE_ID
   ,TRAVEL_ID
   ,REWORK_QTY
   ,UPDATE_TIME
   ,UPDATE_USERID
)
VALUES
(
    :RC_NO
   ,:NODE_ID
   ,:TRAVEL_ID
   ,:REWORK_QTY
   ,SYSDATE
   ,:UPDATE_USERID
)";
            var P = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", rc_no },
                new object[] { ParameterDirection.Input, OracleType.Number, "NODE_ID", node_id },
                new object[] { ParameterDirection.Input, OracleType.Number, "TRAVEL_ID", travel_id },
                new object[] { ParameterDirection.Input, OracleType.Number, "REWORK_QTY", rework_qty },
                new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", EmpID }
            };

            ClientUtils.ExecuteSQL(S, P.ToArray());
        }

        /// <summary>
        /// 重工數量檢查不合格的處理
        /// </summary>
        /// <param name="message"></param>
        private void ReworkMessage(string rc, string message)
        {
            message = SajetCommon.SetLanguage(message) + " : " + rc;

            SajetCommon.Show_Message(message, 0);

            TbReworkQty.Text = "0";

            TbReworkQty.Focus();

            TbReworkQty.SelectAll();
        }

        #endregion

        private void TbInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return) return;

            if (CheckInput())
            {
                ClearData();
                ShowData();
            }

            TbInput.SelectAll();
        }

        private void TbInput_TextChanged(object sender, EventArgs e)
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
            object[][] Params;

            string message = string.Empty;

            # region Keyparts Collection
            string skeypart = string.Empty;
            if (bOneSheet)
            {
                if (!CheckKPSNInput())
                {
                    //tabControl1.SelectedIndex = 6;
                    TcParams.SelectedTab = TpKeyparts;
                    EditKPSN.Focus();
                    return;
                }

                foreach (DataGridViewRow dr in DgvKeypart.Rows)
                {
                    //skeypart += dr.Cells["PART_NO"].EditedFormattedValue.ToString() + (char)9 
                    //    + dr.Cells["RC_NO"].EditedFormattedValue.ToString() + (char)27;
                    skeypart += dr.Cells["PART_NO"].EditedFormattedValue.ToString() + (char)9
                        + dr.Cells["RC_NO"].EditedFormattedValue.ToString() + (char)9
                        + dr.Cells["ITEM_COUNT"].EditedFormattedValue.ToString() + (char)27;
                }
            }
            #endregion

            #region 檢查資料收集是否合格

            // 只檢查有沒有不合格的輸入值，進入迴圈裡面時再整理傳入參數
            foreach (var RC in RcDetails)
            {
                if (RC.SELECT)
                {
                    // 再檢查一次資料收集數值
                    RC.DataCollections.ForEach(x =>
                    {
                        DgvInputCheck(data_item: x);
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

            #region 檢查機台。有綁定機台的製程，不允許流程卡產出時，沒有正在加工的機台。

            var processMachineList = OtSrv.GetProcessMachineList(programInfo.dsRC.Tables[0].Rows[0]["PROCESS_ID"].ToString());

            bool hasProcessMachine = processMachineList.Tables[0].Rows.Count > 0;

            if (hasProcessMachine)
            {
                var machineSettingIsInvalidList
                    = RcDetails
                    .Where(x => x.SELECT)
                    .Where(x => x.Machines.Count <= 0)
                    .Select(x => x.RC_NO)
                    .ToList();

                if (machineSettingIsInvalidList.Any())
                {
                    message
                        = SajetCommon.SetLanguage("Please set working machine")
                        + ":"
                        + Environment.NewLine
                        + string.Join("," + Environment.NewLine, machineSettingIsInvalidList);

                    SajetCommon.Show_Message(message, 0);

                    return;
                }
            }

            #endregion

            #region 檢查元件是否有輸入良品數, 不良品數
            string sSN = string.Empty;
            foreach (DataGridViewRow dr in DgvSN.Rows)
            {
                if (dr.Cells["CURRENT_STATUS"].EditedFormattedValue.ToString() == "NG")
                {
                    if (programInfo.slDefect[dr.Cells["SERIAL_NUMBER"].EditedFormattedValue.ToString()] == 0)
                    {
                        message =
                            SajetCommon.SetLanguage("Defect Qty Empty") +
                            Environment.NewLine
                            + DgvSN.Columns[0].HeaderText + ": "
                            + dr.Cells["SERIAL_NUMBER"].EditedFormattedValue.ToString();

                        SajetCommon.Show_Message(message, 0);
                        DgvSN.CurrentCell = dr.Cells[0];
                        DgvSN.Focus();
                        return;
                    }
                }

                // 2015/08/26, Aaron
                if (!string.IsNullOrEmpty(dr.Cells["GOOD_QTY"].ErrorText))
                {
                    SajetCommon.Show_Message(DgvSN.Columns["GOOD_QTY"].HeaderText + dr.Cells["GOOD_QTY"].ErrorText, 0);
                    DgvSN.CurrentCell = dr.Cells["GOOD_QTY"];
                    DgvSN.Focus();
                    TcParams.SelectedTab = TpSN;
                    return;
                }

                // 2015/08/26, Aaron
                if (!string.IsNullOrEmpty(dr.Cells["SCRAP_QTY"].ErrorText))
                {
                    SajetCommon.Show_Message(DgvSN.Columns["SCRAP_QTY"].HeaderText + dr.Cells["SCRAP_QTY"].ErrorText, 0);
                    DgvSN.CurrentCell = dr.Cells["SCRAP_QTY"];
                    DgvSN.Focus();
                    TcParams.SelectedTab = TpSN;
                    return;
                }

                string sSNTemp = dr.Cells["SERIAL_NUMBER"].EditedFormattedValue.ToString() + (char)9 + dr.Cells["CURRENT_STATUS"].EditedFormattedValue.ToString()
                    + (char)9 + dr.Cells["GOOD_QTY"].EditedFormattedValue.ToString()
                    + (char)9 + dr.Cells["SCRAP_QTY"].EditedFormattedValue.ToString();

                if (programInfo.iSNInput[1] > -1)
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
                            sSN += sSNTemp + (char)9 + DgvSN.Columns[i].Tag.ToString() + (char)9 + dr.Cells[i].EditedFormattedValue.ToString() + (char)27;
                        }
                    }
                }
                else
                {
                    // 2015/08/26, Aaron
                    sSN += sSNTemp + (char)9 + (char)27;
                }
            }
            #endregion

            // 統一檢查 Bonus 欄位
            if (!CheckBonus())
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Bouns Quantity Invalid."), 0);
                return;
            }

            #region 自訂報工產出時間的檢查

            OUT_PROCESS_TIME = OtSrv.GetDBDateTimeNow();

            foreach (var rc in RcDetails)
            {
                if (!RcSrv.IsOutputTimeValid(runcard: RcSrv.GetRcNoInfo(Runcard: rc.RC_NO), OutProcessTime: rc.OUTPUT_TIME, message: out message))
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

            if (!SetNextProcess())
            {
                return;
            }
            else
            {
                #region 檢查結束製程
                string sSQL = $@"
SELECT
    COUNT(*) COUNT
FROM
    SAJET.SYS_END_PROCESS   A,
    SAJET.G_RC_STATUS       B
WHERE
    A.PART_ID = B.PART_ID
    AND A.ROUTE_ID = B.ROUTE_ID
    AND A.NODE_ID = B.NODE_ID
    AND A.PROCESS_ID = B.PROCESS_ID
    AND B.RC_NO = '{TbInput.Text}'
    AND A.ENABLED = 'Y'
";
                var result = ClientUtils.ExecuteSQL(sSQL);
                if (result.Tables[0].Rows[0]["COUNT"].ToString() != "0")
                {
                    rcRoute.sNext_Process = "0";
                }
                #endregion
            }

            //重新建立已選取 RC 的清單
            var ExecuteRCs = CreateRCsList(DefectItems)
                .Where(x => x.SELECT)
                .ToList();

            #region 製程設定為 "會使用到 T4 爐 / T6 爐" 的製程，要做的檢查

            if (!T4Srv.CheckForDateCodeAndStoveSeq(runcards: ref ExecuteRCs, usingT4OrT6stove: usingT4OrT6stove, message: out message))
            {
                SajetCommon.Show_Message(message, 0);

                return;
            }

            #endregion

            #region 機台加工數量

            var QtyNoPass_1 = new List<string>();
            var QtyNoPass_2 = new List<string>();

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
                if (i_load_qty > 0 && i_current_qty != i_load_qty)
                {
                    QtyNoPass_2.Add(rc.RC_NO);
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
                double dInput = RC.CURRENT_QTY;
                double dScrap = (RC.Defects.Count == 0) ? 0 : RC.Defects.Sum(m => m.QTY);
                double dGood = dInput - dScrap;
                var row = DgvRC.Rows.Cast<DataGridViewRow>()
                    .FirstOrDefault(m => m.Cells["RC_NO"].Value.ToString() == RC.RC_NO);
                string sBonus = row.Cells["BONUS"].Value.ToString();
                string sWH = row.Cells["WORK_HOUR"].Value.ToString();

                #region 檢查良品數
                if (!OtSrv.IsNumeric(4, dGood.ToString()))
                {
                    message =
                        SajetCommon.SetLanguage("Data Invalid") +
                        Environment.NewLine +
                        SajetCommon.SetLanguage("RunCard") + ": " + RC.RC_NO;

                    SajetCommon.Show_Message(message, 0);

                    return;
                }

                if (dGood > dInput || dInput != dGood + dScrap)
                {
                    message =
                        SajetCommon.SetLanguage("Data Abnormal") +
                        Environment.NewLine +
                        SajetCommon.SetLanguage("RunCard") + ": " + RC.RC_NO;

                    SajetCommon.Show_Message(message, 0);

                    return;
                }
                #endregion

                #region 檢查非元件是否有輸入良品數, 不良品數
                if (string.IsNullOrEmpty(dGood.ToString()))
                {
                    message =
                        SajetCommon.SetLanguage("Good Qty Empty") +
                        Environment.NewLine +
                        SajetCommon.SetLanguage("RunCard") + ": " + RC.RC_NO;

                    SajetCommon.Show_Message(message, 0);

                    return;
                }
                if (string.IsNullOrEmpty(dScrap.ToString()))
                {
                    message =
                        SajetCommon.SetLanguage("Scrap Qty Empty") +
                        Environment.NewLine +
                        SajetCommon.SetLanguage("RunCard") + ": " + RC.RC_NO;

                    SajetCommon.Show_Message(message, 0);

                    return;
                }

                try
                {
                    if (dGood < 0)
                    {
                        message =
                            SajetCommon.SetLanguage("Good Qty Invalid") +
                            Environment.NewLine +
                            SajetCommon.SetLanguage("RunCard") + ": " + RC.RC_NO;

                        SajetCommon.Show_Message(message, 0);

                        return;
                    }

                    if (dScrap < 0)
                    {
                        message =
                            SajetCommon.SetLanguage("Scrap Qty Invalid") +
                            Environment.NewLine +
                            SajetCommon.SetLanguage("RunCard") + ": " + RC.RC_NO;

                        SajetCommon.Show_Message(message, 0);
                        return;
                    }

                    if (dGood + dScrap == 0)
                    {
                        message =
                           SajetCommon.SetLanguage("Sum=0") +
                           Environment.NewLine +
                           SajetCommon.SetLanguage("RunCard") + ": " + RC.RC_NO;

                        SajetCommon.Show_Message(message, 0);

                        return;
                    }
                }
                catch
                {
                    message =
                       SajetCommon.SetLanguage("Data Invalid") +
                       Environment.NewLine +
                       SajetCommon.SetLanguage("RunCard") + ": " + RC.RC_NO;

                    SajetCommon.Show_Message(message, 0);
                    return;
                }

                // check defect qty valid again
                for (int i = 0; i < RC.Defects.Count; i++)
                {
                    if (!OtSrv.IsNumeric(4, RC.Defects[i].QTY.ToString()))
                    {
                        message =
                           SajetCommon.SetLanguage("Please Input Quantity in Positive integer") +
                           Environment.NewLine +
                           SajetCommon.SetLanguage("RunCard") + ": " + RC.RC_NO;

                        SajetCommon.Show_Message(message, 0);
                        return;
                    }
                }
                #endregion

                #region 將元件或不良代碼組成字串
                string sDefect = string.Empty;
                if (RC.Defects.Count > 0)
                {
                    foreach (var defect in RC.Defects)
                    {
                        if (defect.QTY > 0)
                        {
                            sDefect += defect.DEFECT_CODE + (char)9 + defect.QTY + (char)27;
                        }
                    }
                }
                #endregion

                #region 製程機台

                string sMachine = string.Empty;

                var Machines = new List<MachineDownModel>();

                RC.Machines.ForEach(x =>
                {
                    sMachine += x.MACHINE_CODE + (char)9;

                    Machines.Add(new MachineDownModel
                    {
                        MACHINE_ID = x.MACHINE_ID,
                        LOAD_QTY = x.LOAD_QTY,
                        DATE_CODE = x.DATE_CODE,
                        STOVE_SEQ = x.STOVE_SEQ,
                    });
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

                #region 如果Bonus欄位有輸入檢查數值是否正確
                if (!string.IsNullOrEmpty(sBonus))
                {
                    if (!OtSrv.IsNumeric(5, sBonus))
                    {
                        SajetCommon.Show_Message(
                            SajetCommon.SetLanguage("Bouns Quantity Error.") +
                            Environment.NewLine +
                            SajetCommon.SetLanguage("RunCard") + ": " + RC.RC_NO, 0);
                        return;
                    }
                }
                else
                {
                    sBonus = "0";
                }
                #endregion

                #region 如果 Work Hour 欄位有輸入檢查數值是否正確
                if (!string.IsNullOrEmpty(sWH))
                {
                    if (!OtSrv.IsNumeric(1, sWH))
                    {
                        SajetCommon.Show_Message(
                            SajetCommon.SetLanguage("Work Hour Error.") +
                            Environment.NewLine +
                            SajetCommon.SetLanguage("RunCard") + ": " + RC.RC_NO, 0);
                        return;
                    }

                    // V 1.0.0.16 加入判斷上限
                    if (sWH.Length > 5)
                    {
                        SajetCommon.Show_Message(
                            SajetCommon.SetLanguage("Please enter up to five digits.") +
                            Environment.NewLine +
                            SajetCommon.SetLanguage("RunCard") + ": " + RC.RC_NO, 0);
                        return;
                    }
                }
                else
                {
                    SajetCommon.Show_Message(
                        SajetCommon.SetLanguage("Please Input Work Hour.") +
                        Environment.NewLine +
                        SajetCommon.SetLanguage("RunCard") + ": " + RC.RC_NO, 0);
                    return;
                }
                #endregion

                #region 判斷是否重工
                string sTSTATUS = string.Empty;
                try
                {
                    string sSQL = @"
SELECT
    RC_NO
FROM
    SAJET.G_RC_TRAVEL
WHERE
    RC_NO = :RC_NO
    AND PROCESS_ID = :PROCESS_ID
";
                    Params = new object[2][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", RC.RC_NO };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", programInfo.dsRC.Tables[0].Rows[0]["PROCESS_ID"].ToString() };
                    DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

                    if (dsTemp != null && dsTemp.Tables[0].Rows.Count > 0)
                    {
                        sTSTATUS = "1";
                    }
                }
                catch (Exception)
                {
                    sTSTATUS = "";
                }
                #endregion

                #region 判斷重工數量

                if (!CheckReworkQty(RC.RC_NO))
                {
                    return;
                }

                #endregion

                var RcInfo = GetRcNoInfo(RC.RC_NO);

                var time = RC.DateTimePicker_set ? RC.OUTPUT_TIME : OUT_PROCESS_TIME;

                Params = new object[18][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TEMP", TslEmp.Text };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TRC", RC.RC_NO };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TITEM", sInput };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TDEFECT", sDefect };
                Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSN", sSN };
                Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TMEMO", TbMemo.Text };
                Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TGOOD", dGood };
                Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSCRAP", dScrap };
                Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TNEXTPROCESS", rcRoute.sNext_Process };
                Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TNEXTNODE", rcRoute.sNext_Node };
                Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TNEXTSHEET", rcRoute.sSheet_Name };
                Params[11] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TBONUS", sBonus };
                Params[12] = new object[] { ParameterDirection.Input, OracleType.DateTime, "TNOW", time };
                Params[13] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSTATUS", sTSTATUS };
                Params[14] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TMACHINE", sMachine };
                Params[15] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TKEYPART", skeypart };
                Params[16] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TWORKHOUR", sWH.Trim() };
                Params[17] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_RC_OUTPUT", Params); // V12
                string sMsg = ds.Tables[0].Rows[0]["TRES"].ToString();

                if (sMsg != "OK")
                {
                    TsslMsg.ForeColor = Color.Red;
                    SajetCommon.Show_Message(
                        SajetCommon.SetLanguage(sMsg) +
                        Environment.NewLine +
                        SajetCommon.SetLanguage("RunCard") + ": " + RC.RC_NO, 0);

                    TsslMsg.Text
                        = SajetCommon.SetLanguage(sMsg)
                        + Environment.NewLine
                        + SajetCommon.SetLanguage("RunCard")
                        + ": "
                        + RC.RC_NO;
                    return;
                }
                else
                {
                    if (RC.REWORK_QTY > 0)
                    {
                        SaveReworkQty(rc_no: RC.RC_NO, node_id: rcRoute.sNode_Id, travel_id: rcRoute.sTravel_Id, rework_qty: RC.REWORK_QTY);
                    }

                    RecordMachine(RcInfo: RcInfo, Machines: Machines, now: time);

                    TsslMsg.ForeColor = Color.Blue;
                    TsslMsg.Text
                        = SajetCommon.SetLanguage(sMsg)
                        + Environment.NewLine
                        + SajetCommon.SetLanguage("RunCard")
                        + ": "
                        + RC.RC_NO;
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
            if (string.IsNullOrEmpty(EditKPSN.Text))
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Keypart SN Error."), 0);
                return;
            }
            // 確認用量有輸入值
            if (string.IsNullOrEmpty(EditCount.Text))
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Item Count Error."), 0);
                EditCount.SelectAll();
                EditCount.Focus();
                return;
            }
            else
            {
                // 確認用量正確性
                if (!OtSrv.IsNumeric(3, EditCount.Text))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Input Quantity in Positive number"), 0);
                    EditCount.SelectAll();
                    EditCount.Focus();
                    return;
                }
            }
            // 確認是否已點選料號
            if (string.IsNullOrEmpty(DgvBOM.CurrentRow.Cells["PART_NO"].Value.ToString()))
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Choose one Part NO in BOM."), 0);
                return;
            }
            CheckBOM(EditKPSN.Text.Trim());
            EditCount.Text = "0";
            EditKPSN.SelectAll();
            EditKPSN.Focus();
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
            RCSource.Tables[0].Rows.Cast<DataRow>().ToList()
                .ForEach(row => row[0] = 0);
            CountRC();
        }
        #endregion

        /// <summary>
        /// 切換機台
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMachineChange_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Current_RC_NO))
            {
                return;
            }

            using (var f = new fChangeMachine
            {
                ProcessID = programInfo.dsRC.Tables[0].Rows[0]["PROCESS_ID"].ToString(),
                Runcard = CurrentRuncard.RC_NO,
                EmpID = EmpID,
                ShareMachineSettings = ShareMachineSettings,
                usingT4OrT6stove = usingT4OrT6stove,
            })
            {
                f.ShowDialog();

                // 重新取得生產中的機台的清單
                var d = OtSrv.GetMachineList(CurrentRuncard.RC_NO);

                var WipMachineList = OtSrv.GetModels(d);

                CurrentRuncard.Machines = WipMachineList.ToList();

                if (ShareMachineSettings)
                {
                    RcDetails.ForEach(x =>
                    {
                        x.Machines = WipMachineList
                        .Select(m => new MachineDownModel(m))
                        .ToList();
                    });
                }
                else
                {
                    RcDetails.First(x => x.RC_NO == CurrentRuncard.RC_NO)
                        .Machines = WipMachineList
                        .Select(m => new MachineDownModel(m))
                        .ToList();
                }

                // 更新畫面
                DgvMachine.DataSource = WipMachineList.ToList();
            }

            OUT_PROCESS_TIME = DtpOutDate.Value = DateTime.Now;
        }

        private void DgvMachine_VisibleChanged(object sender, EventArgs e)
        {
            RearrangeDataGridView(ref DgvMachine);

            dATECODEDataGridViewTextBoxColumn.Width = 120;
        }

        private void DgvDefect_VisibleChanged(object sender, EventArgs e)
        {
            RearrangeDataGridView(ref DgvDefect);
        }

        #region 製程不良現象

        /// <summary>
        /// 只能輸入數字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvDefect_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        #region 觸發資料檢查

        private void DgvDefect_MouseDown(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hit = DgvDefect.HitTest(e.X, e.Y);

            if (hit.RowIndex < 0 | hit.ColumnIndex < 0)
            {
                DgvDefect.EndEdit();
            }
        }

        private void DgvDefect_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Current_RC_NO) || CurrentRuncard == null)
            {
                return;
            }

            int.TryParse(DgvDefect.CurrentRow.Cells[nameof(qTYDataGridViewTextBoxColumn)].Value?.ToString(), out int value);

            DgvDefect.CurrentRow.Cells[nameof(qTYDataGridViewTextBoxColumn)].Value = value;

            //檢查不良數量
            int defectQTY = 0;
            foreach (DataGridViewRow row in DgvDefect.Rows)
            {
                defectQTY += int.Parse(row.Cells[nameof(qTYDataGridViewTextBoxColumn)].Value.ToString());
            }

            if (defectQTY > CurrentRuncard.CURRENT_QTY)
            {
                string message = SajetCommon.SetLanguage("More than the current quantity")
                    + Environment.NewLine
                    + CurrentRuncard.RC_NO;

                SajetCommon.Show_Message(message, 0);

                DgvDefect.CurrentRow.Cells[nameof(qTYDataGridViewTextBoxColumn)].Value = value = 0;
            }

            string defectCode = DgvDefect.CurrentRow.Cells[nameof(dEFECTCODEDataGridViewTextBoxColumn)].Value.ToString();

            // 把設定的不良數量存入模型
            CurrentRuncard
                .Defects
                .First(m => m.DEFECT_CODE == defectCode)
                .QTY = value;

            RcDetails
                .First(x => x.RC_NO == CurrentRuncard.RC_NO)
                .Defects
                .First(x => x.DEFECT_CODE == defectCode)
                .QTY = value;

            CountRC();
        }

        #endregion

        #endregion

        private bool CheckInput()
        {
            object[][] Params = new object[3][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TREV", TbInput.Text };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSHEET", programInfo.sFunction };
            Params[2] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
            DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_RC_CHK_ROUTE", Params);
            if (ds.Tables[0].Rows[0]["TRES"].ToString() != "OK")
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage(ds.Tables[0].Rows[0]["TRES"].ToString()), 0);

                return false;
            }

            return true;
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
            var Item = RcDetails.First(x => x.RC_NO == runcard)
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
            programInfo.slDefect.Clear();
            DgvDefect.Rows.Clear();
        }

        private void ShowData()
        {
            string sSQL = programInfo.sSQL["SQL"];
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SN", TbInput.Text };
            try
            {
                programInfo.dsRC = ClientUtils.ExecuteSQL(sSQL, Params);
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

                        }
                    }

                    bOneSheet = OtSrv.ProcessSheet(process_id: programInfo.dsRC.Tables[0].Rows[0]["PROCESS_ID"].ToString());

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
            #region Display Process Parameter
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", programInfo.dsRC.Tables[0].Rows[0]["PROCESS_ID"].ToString() };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", programInfo.dsRC.Tables[0].Rows[0]["PART_ID"].ToString() };
            DataSet ds = ClientUtils.ExecuteSQL(programInfo.sSQL["製程條件"], Params);
            DgvCondition.DataSource = ds;
            DgvCondition.DataMember = ds.Tables[0].ToString();
            foreach (DataColumn dc in ds.Tables[0].Columns)
                DgvCondition.Columns[dc.ColumnName].HeaderText = SajetCommon.SetLanguage(DgvCondition.Columns[dc.ColumnName].HeaderText, 1);
            DgvCondition.Columns["VALUE_TYPE"].Visible = false;

            ds = ClientUtils.ExecuteSQL(programInfo.sSQL["資料收集"], Params);
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

            #region Display Piece
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", TbInput.Text };
            ds = ClientUtils.ExecuteSQL(programInfo.sSQL["元件"], Params);
            programInfo.txtGood.Enabled = false;
            programInfo.txtScrap.Enabled = false;
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataGridViewTextBoxColumn dText = new DataGridViewTextBoxColumn
                {
                    Name = "SERIAL_NUMBER",
                    HeaderText = SajetCommon.SetLanguage("SERIAL_NUMBER", 1),
                    ReadOnly = true
                };
                DgvSN.Columns.Add(dText);
                Params = new object[2][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", programInfo.dsRC.Tables[0].Rows[0]["PROCESS_ID"].ToString() };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", programInfo.dsRC.Tables[0].Rows[0]["PART_ID"].ToString() };
                programInfo.dsSNParam = ClientUtils.ExecuteSQL(programInfo.sSQL["元件製程參數"], Params);
                programInfo.iSNInput[0] = DgvSN.Columns.Count;
                programInfo.iSNInput[1] = -1;
                if (programInfo.dsSNParam.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in programInfo.dsSNParam.Tables[0].Rows)
                    {
                        if (programInfo.iSNInput[1] == -1 && dr["ITEM_TYPE"].ToString() == "3")
                        {
                            for (int i = 1; i < ds.Tables[0].Columns.Count; i++)
                            {
                                if (ds.Tables[0].Columns[i].ColumnName == "CURRENT_STATUS")
                                {
                                    DataGridViewComboBoxColumn dComb = new DataGridViewComboBoxColumn
                                    {
                                        Name = ds.Tables[0].Columns[i].ColumnName,
                                        HeaderText = SajetCommon.SetLanguage(ds.Tables[0].Columns[i].ColumnName, 1),
                                        ReadOnly = false
                                    };
                                    dComb.Items.Add("OK");
                                    dComb.Items.Add("NG");
                                    DgvSN.Columns.Add(dComb);
                                }
                                else
                                {
                                    dText = new DataGridViewTextBoxColumn
                                    {
                                        Name = ds.Tables[0].Columns[i].ColumnName,
                                        HeaderText = SajetCommon.SetLanguage(ds.Tables[0].Columns[i].ColumnName, 1),
                                        ReadOnly = programInfo.iInputField["元件"].IndexOf(i) == -1
                                    };
                                    DgvSN.Columns.Add(dText);
                                }
                            }
                            programInfo.iSNInput[1] = DgvSN.ColumnCount;
                        }
                        switch (dr["INPUT_TYPE"].ToString())
                        {
                            case "S":
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
                            default:
                                dText = new DataGridViewTextBoxColumn
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
                }
                else
                {
                    for (int i = 1; i < ds.Tables[0].Columns.Count; i++)
                    {
                        if (ds.Tables[0].Columns[i].ColumnName == "CURRENT_STATUS")
                        {
                            DataGridViewComboBoxColumn dComb = new DataGridViewComboBoxColumn
                            {
                                Name = ds.Tables[0].Columns[i].ColumnName,
                                HeaderText = SajetCommon.SetLanguage(ds.Tables[0].Columns[i].ColumnName, 1),
                                ReadOnly = false
                            };
                            dComb.Items.Add("OK");
                            dComb.Items.Add("NG");
                            DgvSN.Columns.Add(dComb);
                        }
                        else
                        {
                            dText = new DataGridViewTextBoxColumn
                            {
                                Name = ds.Tables[0].Columns[i].ColumnName,
                                HeaderText = SajetCommon.SetLanguage(ds.Tables[0].Columns[i].ColumnName, 1),
                                ReadOnly = programInfo.iInputField["元件"].IndexOf(i) == -1
                            };
                            DgvSN.Columns.Add(dText);
                        }
                    }
                }
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    DgvSN.Rows.Add();
                    programInfo.slDefect.Add(dr["SERIAL_NUMBER"].ToString(), 0);
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
                programInfo.txtGood.Text = ds.Tables[0].Rows.Count.ToString();
            }

            foreach (DataGridViewColumn col in DgvSN.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            #endregion

            #region Keyparts Collection
            if (bOneSheet)
            {
                // Show BOM information
                DgvBOM.Columns.Clear();
                DgvBOM.Columns.Add("PART_NO", SajetCommon.SetLanguage("PART_NO", 1));
                DgvBOM.Columns.Add("SPEC1", SajetCommon.SetLanguage("SPEC1", 1));
                DgvBOM.Columns.Add("ITEM_GROUP", SajetCommon.SetLanguage("ITEM_GROUP", 1));
                //gvBOM.Columns.Add("IS_MATERIAL", SajetCommon.SetLanguage("IS_MATERIAL", 1));    // 增加物料清單是否為物料FLAG欄位
                DgvBOM.Columns.Add("PURCHASE", SajetCommon.SetLanguage("PURCHASE", 1));           //  是否為外購作為檢查物料序號的依據
                DgvBOM.Columns.Add("QTY", SajetCommon.SetLanguage("QTY", 1));

                Params = new object[2][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", programInfo.dsRC.Tables[0].Rows[0]["PROCESS_ID"].ToString() };
                //Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", programInfo.dsRC.Tables[0].Rows[0]["PART_ID"].ToString() };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", programInfo.dsRC.Tables[0].Rows[0]["WO_OPTION2"].ToString() };
                ds = ClientUtils.ExecuteSQL(programInfo.sSQL["KEYPARTS COLLECTION"], Params);

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
            }
            else
            {
                this.TpKeyparts.Parent = null;
            }

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

        /// <summary>
        /// 載入同工單同製程位置的流程卡
        /// </summary>
        public void RCsDataGrid()
        {
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "rc_no", TbInput.Text },
                new object[] { ParameterDirection.Input, OracleType.Number, "report_type", usingT4OrT6stove ? 2 : 1 },
            };

            // 只要已投入狀態的流程卡 (Running)
            string s = $@"
WITH RUNCARD_FILTER AS (
    SELECT DISTINCT
        E.RC_NO
    FROM
        SAJET.G_RC_STATUS                A,
{(usingT4OrT6stove ? "--" : "/*")}
        SAJET.G_RC_TRAVEL_MACHINE_DOWN   B,
        SAJET.G_RC_TRAVEL_MACHINE_DOWN   D,
--*/
        SAJET.G_RC_STATUS                E
    WHERE
        A.RC_NO = :RC_NO
        AND A.WORK_ORDER = E.WORK_ORDER
        AND A.PROCESS_ID = E.PROCESS_ID
        AND A.NODE_ID = E.NODE_ID
{(usingT4OrT6stove ? "--" : "/*")}
        AND B.REPORT_TYPE = :REPORT_TYPE
        AND A.RC_NO = B.RC_NO
        AND A.NODE_ID = B.NODE_ID
        AND E.RC_NO = D.RC_NO
        AND E.NODE_ID = D.NODE_ID
        AND D.REPORT_TYPE = :REPORT_TYPE
        AND B.MACHINE_ID = D.MACHINE_ID
--        AND B.START_TIME = D.START_TIME
--*/
), RUNCARD_STATUS AS (
    SELECT
        CURRENT_STATUS
    FROM
        SAJET.G_RC_STATUS
    WHERE
        RC_NO = :RC_NO
)
SELECT
    1 ""CHECK"",
    A.RC_NO,
    A.CURRENT_QTY,
    A.CURRENT_QTY ""GOOD_QTY"",
    0 ""SCRAP_QTY"",
    A.INITIAL_QTY,
    0 ""REWORK_QTY"",
    0 ""BONUS"",
    1 ""WORK_HOUR""
FROM
    SAJET.G_RC_STATUS   A,
    RUNCARD_FILTER      B,
    RUNCARD_STATUS      C
WHERE
    A.RC_NO = B.RC_NO
    AND ( A.CURRENT_STATUS = 1
          OR A.CURRENT_STATUS = C.CURRENT_STATUS )
ORDER BY
    RC_NO
";
            RCSource = ClientUtils.ExecuteSQL(s, p.ToArray());

            DgvRC.DataSource = RCSource;
            DgvRC.DataMember = RCSource.Tables[0].ToString();
            SelectRC(TbInput.Text);
        }

        /// <summary>
        /// 選擇下一製程
        /// </summary>
        /// <returns></returns>
        public bool SetNextProcess()
        {
            try
            {
                rcRoute.sNode_Id = "0";
                rcRoute.sNext_Node = "0";
                rcRoute.sNext_Process = "0";
                rcRoute.sSheet_Name = "0";
                rcRoute.sNode_type = "0";
                rcRoute.g_sRouteID = "0";
                rcRoute.g_sProcessID = "0";
                rcRoute.sLink_Name = "";

                string sSQL = " SELECT * FROM SAJET.G_RC_STATUS WHERE RC_NO ='" + TbInput.Text + "' ";
                DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);

                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    rcRoute.sNext_Process = dsTemp.Tables[0].Rows[0]["NEXT_PROCESS"].ToString();
                    rcRoute.sSheet_Name = dsTemp.Tables[0].Rows[0]["SHEET_NAME"].ToString();
                    rcRoute.g_sRouteID = dsTemp.Tables[0].Rows[0]["ROUTE_ID"].ToString();
                    rcRoute.g_sProcessID = dsTemp.Tables[0].Rows[0]["PROCESS_ID"].ToString();
                    rcRoute.sNode_Id = dsTemp.Tables[0].Rows[0]["NODE_ID"].ToString();
                    rcRoute.sTravel_Id = dsTemp.Tables[0].Rows[0]["TRAVEL_ID"].ToString();
                }
                else
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("RC NO Error."), 0);
                    return false;
                }

                sSQL = @"
SELECT
    NODE_ID,
    NODE_TYPE,
    NODE_CONTENT,
    NEXT_NODE_ID,
    LINK_NAME
FROM
    SAJET.SYS_RC_ROUTE_DETAIL
WHERE
    ROUTE_ID = :ROUTE_ID
    AND NODE_CONTENT = :NODE_CONTENT
    AND ( NODE_ID = :NODE_ID
          OR GROUP_ID = :NODE_ID )
";
                object[][] Params = new object[3][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ROUTE_ID", rcRoute.g_sRouteID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NODE_CONTENT", rcRoute.g_sProcessID };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NODE_ID", rcRoute.sNode_Id };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    rcRoute.sNode_Id = dsTemp.Tables[0].Rows[0]["NODE_ID"].ToString();
                    rcRoute.sNext_Node = dsTemp.Tables[0].Rows[0]["NEXT_NODE_ID"].ToString();
                    rcRoute.sNode_type = dsTemp.Tables[0].Rows[0]["NODE_TYPE"].ToString();
                    rcRoute.sLink_Name = dsTemp.Tables[0].Rows[0]["LINK_NAME"].ToString();
                }
                else
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Route Error."), 0);
                    return false;
                }

                if (rcRoute.sNode_type == "1"
                    || rcRoute.sNode_type == "2"
                    || rcRoute.sNode_type == "3") // GROUP
                {
                    var f = new FTransferProcess.TransferProcess
                    {
                        sNode_type = rcRoute.sNode_type,
                        sRoute_Id = rcRoute.g_sRouteID,
                        sNode_Id = rcRoute.sNode_Id,
                        sRc_No = TbInput.Text,
                    };

                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        rcRoute.g_sProcessID = f.sProcess_Id;
                        rcRoute.sNext_Node = f.sNext_Node;
                        rcRoute.sNext_Process = f.sNext_Process;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (rcRoute.sNode_type == "0" || rcRoute.sNode_type == "9") // START & END
                {
                    string sMsg = SajetCommon.SetLanguage("The process can not be selected.");
                    SajetCommon.Show_Message(sMsg, 1);
                    return false;
                }

                if (rcRoute.sNode_type == "1" || rcRoute.sNode_type == "2" || rcRoute.sNode_type == "3")
                {
                    sSQL = "select sheet_name from sajet.sys_rc_process_sheet where process_id = '" + rcRoute.sNext_Process + "' and sheet_seq = '0'";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);

                    if (dsTemp.Tables[0].Rows.Count == 0)
                    {
                        if (rcRoute.sNext_Process != "0")
                        {
                            // v 1.0.0.14
                            string sMsg = SajetCommon.SetLanguage("Please Set up next process sheet first.");
                            SajetCommon.Show_Message(sMsg, 1);
                            return false;
                        }
                        else
                        {
                            rcRoute.sSheet_Name = "END";    // 最後產出製程
                            return true;
                        }
                    }

                    rcRoute.sSheet_Name = dsTemp.Tables[0].Rows[0]["sheet_name"].ToString();
                }
                return true;
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);
                return false;
            }
        }

        /// <summary>
        /// 檢查所有 RC 的 Bonus
        /// </summary>
        /// <returns></returns>
        private bool CheckBonus()
        {
            try
            {
                return DgvRC.Rows.Cast<DataGridViewRow>()
                    .Where(row => row.Cells["CHECK"].Value.ToString() == "1")
                    .All(row =>
                    {
                        double initialQty = double.Parse(row.Cells["INITIAL_QTY"].Value.ToString());
                        double currentQty = double.Parse(row.Cells["CURRENT_QTY"].Value.ToString());
                        double goodQty = double.Parse(row.Cells["GOOD_QTY"].Value.ToString());
                        double dBonus = double.Parse(row.Cells["BONUS"].Value.ToString());

                        return !((initialQty > 0.0d && initialQty < goodQty + dBonus)
                        || 0 > initialQty + dBonus);  // 定義：當 Bonus 數為負值，倒扣要大於 0
                    });
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 檢查Bonus
        /// </summary>
        /// <param name="sBonus"></param>
        /// <param name="RC_NO"></param>
        /// <returns></returns>
        private bool CheckBonus(string sBonus, string RC_NO)
        {
            try
            {
                double dBonus = Convert.ToDouble(sBonus);
                object[][] Params = new object[1][];
                string sSQL = @" SELECT INITIAL_QTY 
                                 FROM SAJET.G_RC_STATUS 
                                 WHERE RC_NO = :RC_NO 
                                 AND ROWNUM = 1 ";
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", RC_NO };
                DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                if (dsTemp != null)
                {
                    if (dsTemp.Tables[0].Rows.Count > 0)
                    {
                        double dIniQty = Convert.ToDouble(dsTemp.Tables[0].Rows[0]["INITIAL_QTY"].ToString());
                        double dInputQty = Convert.ToDouble(programInfo.dsRC.Tables[0].Rows[0]["QTY"].ToString());
                        double dGoodQty = Convert.ToDouble(programInfo.txtGood.Text);

                        if (dIniQty > 0.0 && dIniQty < dGoodQty + dBonus)  //if (dIniQty > 0.0 && dIniQty < dInputQty + dBonus)                          
                        {
                            return false;
                        }
                        if (0 > dIniQty + dBonus)     //if (0 > dInputQty + dBonus)
                        {
                            return false;
                        }  // 定義 : 當Bonus數為負值，倒扣要大於0
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
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
                    object[][] Params = new object[2][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", sRC };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_NO", DgvBOM.CurrentRow.Cells["PART_NO"].Value.ToString() };
                    DataSet ds = ClientUtils.ExecuteSQL(programInfo.sSQL["CHECK KEYPART STATUS"], Params);
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("Keypart SN Error."), 0);
                        return false;
                    }
                    else
                    {
                        Params = new object[3][];
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", sRC };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", programInfo.dsRC.Tables[0].Rows[0]["PART_ID"].ToString() };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", programInfo.dsRC.Tables[0].Rows[0]["PROCESS_ID"].ToString() };
                        ds = ClientUtils.ExecuteSQL(programInfo.sSQL["FIND KEYPART DATA"], Params);

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
                                DgvKeypart.Rows[DgvKeypart.Rows.Count - 1].Cells["ITEM_COUNT"].Value = EditCount.Text.Trim();
                                //gvKeypart.Columns["ITEM_COUNT"].ReadOnly = true;
                            }
                        }
                    }
                }
                else
                {
                    DgvKeypart.Rows.Add();
                    DgvKeypart.Rows[DgvKeypart.Rows.Count - 1].Cells["PART_NO"].Value = DgvBOM.CurrentRow.Cells["PART_NO"].Value.ToString();
                    DgvKeypart.Rows[DgvKeypart.Rows.Count - 1].Cells["RC_NO"].Value = EditKPSN.Text.Trim();
                    DgvKeypart.Rows[DgvKeypart.Rows.Count - 1].Cells["ITEM_COUNT"].Value = EditCount.Text.Trim();
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

        // 全部KPSN item_count在此料號加總
        private void CountKPSN()
        {
            for (int i = 0; i < DgvBOM.Rows.Count; i++)
            {
                double cnt = 0;
                for (int j = 0; j < DgvKeypart.Rows.Count; j++)
                {
                    if (DgvKeypart.Rows[j].Cells["PART_NO"].Value.ToString() == DgvBOM.Rows[i].Cells["PART_NO"].Value.ToString())
                    {
                        cnt += Convert.ToDouble(DgvKeypart.Rows[j].Cells["ITEM_COUNT"].Value.ToString());
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
                    {  // 在sys_bom.is_material為N
                        //if (gvBOM.Rows[i].Cells["QTY"].Value.ToString() == "0" && gvBOM.Rows[i].Cells["IS_MATERIAL"].Value.ToString() == "N")
                        if (DgvBOM.Rows[i].Cells["QTY"].Value.ToString() == "0")
                        {
                            SajetCommon.Show_Message(
                                DgvBOM.Rows[i].Cells["PART_NO"].EditedFormattedValue.ToString()
                                + " " + SajetCommon.SetLanguage("does not input Keypart"), 0);
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
                            SajetCommon.Show_Message(
                                DgvBOM.Rows[i].Cells["PART_NO"].EditedFormattedValue.ToString()
                                + " " + SajetCommon.SetLanguage("does not input Keypart"), 0);
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
        }

        /// <summary>
        /// 更新主畫面的數量資料
        /// </summary>
        private int CountRC()
        {
            int goodTotal = 0, scrapTotal = 0;

            LbCount.Text = DgvRC.Rows.OfType<DataGridViewRow>().Sum(r => r.Cells["CHECK"].Value.ToString() == "1" ? 1 : 0).ToString();

            if (!RcDetails.Any())
            {
                return 0;
            }

            foreach (DataGridViewRow r in DgvRC.Rows)
            {
                int iGood = 0, iScrap = 0;
                string RC = r.Cells["RC_NO"].Value.ToString();

                var row = RcDetails
                    .FirstOrDefault(m => m.RC_NO == RC);

                foreach (var defect in row.Defects)
                {
                    if (defect.QTY > 0)
                    {
                        iScrap += defect.QTY;
                    }
                }

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

            if (m_tControlData != null)
            {
                programInfo.txtGood.Text = goodTotal.ToString();
                programInfo.txtScrap.Text = scrapTotal.ToString();
            }

            DgvRC.Refresh();

            return goodTotal;
        }

        /// <summary>
        /// 建立流程卡資料的清單
        /// </summary>
        /// <param name="defects">製程不良清單</param>
        /// <returns></returns>
        private List<RC_DETAILS> CreateRCsList(List<Defect> defects)
        {
            // 建立已勾選的 RC 的新清單，每個 RC 有自己的不良現象清單
            var temp = (from row in RCSource.Tables[0].Rows.Cast<DataRow>()
                        select new RC_DETAILS
                        {
                            SELECT = ShareMachineSettings ? true : row["CHECK"].ToString() == "1",

                            RC_NO = row["RC_NO"].ToString(),
                            CURRENT_QTY = int.Parse(row["CURRENT_QTY"].ToString()),
                            REWORK_QTY = decimal.Parse(row["REWORK_QTY"].ToString()),

                            OUTPUT_TIME = OUT_PROCESS_TIME,

                            Defects = defects
                            .Select(x => new Defect(x))
                            .ToList(),

                            DataCollections = CollectionItems
                            .Select(e => new DataCollectModel(e))
                            .ToList(),

                            Machines = OtSrv.GetModels(OtSrv.GetMachineList(row["RC_NO"].ToString())),

                        }).ToList();

            // 讓新建立的清單繼承已經填寫的資料
            if (RcDetails != null && RcDetails.Count > 0
                && temp != null && temp.Count > 0)
            {
                foreach (var RC in RcDetails)
                {
                    // 不良數量
                    temp.First(x => x.RC_NO == RC.RC_NO).Defects
                        = RC.Defects.Select(x => new Defect(x))
                        .ToList();

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

                    temp.First(x => x.RC_NO == RC.RC_NO).OUTPUT_TIME
                        = RC.OUTPUT_TIME;
                }
            }

            return temp;
        }

        /// <summary>
        /// 建立不良類型清單
        /// </summary>
        /// <returns></returns>
        private List<Defect> CreateDefectList()
        {
            // 找到製程不良現象
            object[][] param = new object[1][];

            param[0] =
                new object[]
                {
                    ParameterDirection.Input,
                    OracleType.VarChar,
                    "PROCESS_ID",
                    programInfo.dsRC.Tables[0].Rows[0]["PROCESS_ID"].ToString()
                };

            DataSet defectTypes = ClientUtils.ExecuteSQL(programInfo.sSQL["PROCESS DEFECT"], param);

            var defects = new List<Defect>();

            foreach (DataRow row in defectTypes.Tables[0].Rows)
            {
                if (!defects.Any(m => m.DEFECT_CODE == row["DEFECT_CODE"].ToString()))
                {
                    defects.Add(
                        new Defect
                        {
                            DEFECT_CODE = row["DEFECT_CODE"].ToString(),
                            DEFECT_DESC = row["DEFECT_DESC"].ToString(),
                            QTY = 0
                        });
                }
            }

            return defects;
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
        /// 是否為共用機台資料的流程卡
        /// </summary>
        /// <returns></returns>
        private bool IsMachineSettingShared()
        {
            string s = @"
SELECT
    b.work_order,
    a.rc_no,
    a.node_id,
    a.start_time,
    a.report_type
FROM
    sajet.g_rc_travel_machine_down   a,
    sajet.g_rc_status                b
WHERE
    b.rc_no = :rc_no
    AND a.rc_no = b.rc_no
    AND a.node_id = b.node_id
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", TbInput.Text },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            return (d != null && d.Tables[0].Rows.Count > 0 && d.Tables[0].Rows[0]["REPORT_TYPE"].ToString() == "2");
        }

        /// <summary>
        /// 機台資訊另外更新到變更機台記錄表。
        /// </summary>
        /// <param name="RcInfo">流程卡資訊</param>
        /// <param name="Machines">使用機台的清單</param>
        private void RecordMachine(
            DataRow RcInfo,
            List<MachineDownModel> Machines,
            DateTime now)
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

            foreach (var machine in Machines)
            {
                if (bOneSheet)
                {
                    s += $@"
INSERT INTO
    SAJET.G_RC_TRAVEL_MACHINE_DOWN
(
    RC_NO
   ,NODE_ID
   ,TRAVEL_ID
   ,MACHINE_ID
   ,START_TIME
   ,END_TIME
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
);";
                }
                else
                {
                    s += $@"

UPDATE
    SAJET.G_RC_TRAVEL_MACHINE_DOWN
SET
    END_TIME = :NOW_TIME
   ,WORK_TIME_MINUTE = NVL(ROUND((TO_NUMBER(:NOW_TIME - START_TIME) * 24 * 60 * 60 + 29) / 60), 0)
   ,WORK_TIME_SECOND = NVL(ROUND(TO_NUMBER(:NOW_TIME - START_TIME) * 24 * 60 * 60, 0), 3)
   ,UPDATE_USERID = :UPDATE_USERID
   ,UPDATE_TIME = :NOW_TIME
   ,DATA_STATUS = 0
   ,LOAD_QTY = :LOAD_QTY{i}
   ,DATE_CODE = :DATE_CODE{i}
   ,STOVE_SEQ = :STOVE_SEQ{i}
   ,REPORT_TYPE = :REPORT_TYPE
WHERE
    RC_NO = :RC_NO
AND TRAVEL_ID = :TRAVEL_ID
AND NODE_ID = :NODE_ID
AND MACHINE_ID = :MACHINE_ID{i}
AND END_TIME IS NULL
;";
                }

                p.Add(new object[] { ParameterDirection.Input, OracleType.Number, $"MACHINE_ID{i}", machine.MACHINE_ID });
                p.Add(new object[] { ParameterDirection.Input, OracleType.Number, $"LOAD_QTY{i}", machine.LOAD_QTY });
                p.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, $"DATE_CODE{i}", machine.DATE_CODE });
                p.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, $"STOVE_SEQ{i}", machine.STOVE_SEQ });

                i++;
            }

            s = $@"
BEGIN
{s}

/* 未選機台的記錄也更新結束時間 */
UPDATE
    SAJET.G_RC_TRAVEL_MACHINE_DOWN
SET
    END_TIME = :NOW_TIME
   ,WORK_TIME_MINUTE = NVL(ROUND((TO_NUMBER(:NOW_TIME - START_TIME) * 24 * 60 * 60 + 29) / 60), 0)
   ,WORK_TIME_SECOND = NVL(ROUND(TO_NUMBER(:NOW_TIME - START_TIME) * 24 * 60 * 60, 0), 0)
   ,DATA_STATUS = 2
   ,REPORT_TYPE = :REPORT_TYPE
WHERE
    RC_NO = :RC_NO
AND TRAVEL_ID = :TRAVEL_ID
AND NODE_ID = :NODE_ID
AND MACHINE_ID = 0
;
END;
";
            ClientUtils.ExecuteSQL(s, p.ToArray());
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