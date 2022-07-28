using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SajetClass;
using RCOutput.Enums;
using RCOutput.Models;
using McSrv = RCOutput.Services.MachineService;
using OtSrv = RCOutput.Services.OtherService;
using QcSrv = RCOutput.Services.QCService;
using QoSrv = RCOutput.Services.QCOptionService;
using RcSrv = RCOutput.Services.RuncardService;
using SfSrv = RCOutput.Services.ShiftService;
using T4Srv = RCOutput.Services.T4Service;

namespace RCOutput
{
    public partial class fMain : Form
    {
        TProgramInfo programInfo;

        TControlData[] m_tControlData;

        TRCroute rcRoute = new TRCroute();

        /// <summary>
        /// 流程卡投入時選擇的機台，參考用
        /// </summary>
        List<MachineDownModel> WipInList = new List<MachineDownModel>();

        /// <summary>
        /// 流程卡產出時選擇的機台。
        /// </summary>
        List<MachineDownModel> WipOutList = new List<MachineDownModel>();

        /// <summary>
        /// 綁定 T4 機台用的清單
        /// </summary>
        List<T4MachineModel> T4List = new List<T4MachineModel>();

        List<string> DateGridViewComboBoxItems = new List<string>();

        DataRow RC_NO_INFO;

        /// <summary>
        /// 流程卡的投入時間
        /// </summary>
        DateTime IN_PROCESS_TIME;

        /// <summary>
        /// 流程卡的產出時間
        /// </summary>
        DateTime OUT_PROCESS_TIME;

        /// <summary>
        /// 判定良品數是否為手動輸入
        /// </summary>
        public bool btxtGood = false;

        /// <summary>
        /// 判定此製程sheet是否為In & Out值為false或者單獨Out值為True
        /// </summary>
        bool bOneSheet = false;

        readonly string m_nParam;

        /// <summary>
        /// Y: Lot Control, N:Piece
        /// </summary>
        public string g_SystemType;

        decimal reworkMaxValue = 0;

        string EmpID = string.Empty;

        string T4_ProcessID = string.Empty;

        string T6_ProcessID = string.Empty;

        bool usingT4OrT6stove = false;

        bool printWPLabel = false;

        #region Isaac 2021/01/20 AQL 元件宣告
        Button _button = new Button()
        {
            Width = 75,
            Dock = DockStyle.Left,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowOnly,
        };
        TextBox _box = new TextBox()
        {
            Dock = DockStyle.Left,
            ReadOnly = true,
        };
        Label _label = new Label()
        {
            Name = nameof(ControlNameEnum.QCOptionLabel),
            Text = string.Empty,
            TextAlign = ContentAlignment.MiddleLeft,
            Dock = DockStyle.Left,
            Margin = new Padding(0),
            ForeColor = Color.Red
        };
        /// <summary>
        /// 需要收集品保項目
        /// </summary>
        bool need_qc = false;
        /// <summary>
        /// 必須抽檢至少一個
        /// </summary>
        bool must_inspect = false;
        #endregion

        public fMain() : this("", "", "", "") { }
        public fMain(string nParam, string strSN, string sEmp, string sTime)
        {
            InitializeComponent();

            m_nParam = nParam;

            this.Text = m_nParam;

            txtInput.Text = strSN;

            tsEmp.Text = sEmp;

            txtInput.Visible = false;

            cmbParam.Visible = false;

            toolStripSeparator2.Visible = false;

            statusStrip1.Visible = false;

            OUT_PROCESS_TIME = OtSrv.GetDBDateTimeNow();

            DtpOutDate.Value = OUT_PROCESS_TIME;

            DgvDefect.MouseDown += DgvDefect_MouseDown;

            DgvDefect.EditingControlShowing += GvDefect_EditingControlShowing;

            #region 載入爐順序號碼的 下拉選單

            DateGridViewComboBoxItems = T4Srv.GetSequencesComboBoxItems();

            sTOVESEQDataGridViewTextBoxColumn.DataSource = DateGridViewComboBoxItems;

            DgvMachine.EditingControlShowing += GvMachine_EditingControlShowing;

            #endregion

            #region T4 頁籤

            var buttonColumn = new DataGridViewButtonColumn
            {
                Name = nameof(ControlNameEnum.T4RemoveDataGridViewButtonColumn),
                Text = SajetCommon.SetLanguage(MessageEnum.Remove.ToString()),
                HeaderText = SajetCommon.SetLanguage(MessageEnum.Remove.ToString()),
                UseColumnTextForButtonValue = true,
            };

            DgvT4.Columns.Add(buttonColumn);

            TcData.SelectedIndexChanged += TcData_SelectedIndexChanged;

            TbRcT4.KeyPress += TbRcT4_KeyPress;

            BtnAddT4.Click += BtnAddT4_Click;

            DgvT4.CellContentClick += DgvT4_CellContentClick;

            //BtnRemoveT4.Click += BtnRemoveT4_Click;

            #endregion

            #region 換班功能

            BtnShift.Text = SajetCommon.SetLanguage(ControlTextEnum.ShiftMachineSetting.ToString());

            BtnHandover.Text = SajetCommon.SetLanguage(ControlTextEnum.JobHandoverMachineSetting.ToString());

            BtnShift.Click += BtnShift_Click;

            BtnHandover.Click += BtnHandover_Click;

            BtnShiftHistory.Click += BtnShiftHistory_Click;

            //CbShift.CheckedChanged += CbShift_CheckedChanged;

            //TbWorkload.KeyPress += NumericTextBoxColumn_KeyPress;

            #endregion

            #region 設定報工時間

            DtpOutDate.DropDown += DtpOutDate_DropDown;

            DtpOutDate.ValueChanged += DtpOutDate_ValueChanged;

            CbReportTime.CheckStateChanged += CbReportTime_CheckStateChanged;

            #endregion

            #region 登記作業人數

            Tb_OP_count.KeyPress += NumericTextBoxColumn_KeyPress;

            #endregion
        }

        private void FMain_Load(object sender, EventArgs e)
        {
            //SajetCommon.SetLanguageControl(this);
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
                programInfo.sProgram = ClientUtils.fProgramName; // ClientUtils.fProgramName;

                programInfo.sFunction = m_nParam; // ClientUtils.fFunctionName;
            }

            EmpID = OtSrv.GetEmpID(emp_no: tsEmp.Text);

            programInfo.sSQL = new Dictionary<string, string>();

            programInfo.iInputField = new Dictionary<string, List<int>>();

            programInfo.sOption = new Dictionary<string, string>();

            programInfo.iInputVisible = new Dictionary<string, int>();

            programInfo.slDefect = new Dictionary<string, int>();

            string sSQL = @"
SELECT *
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
                            int iCount = int.Parse(dr["DEFAULT_VALUE"].ToString());

                            #region 接管流程卡資訊區塊的顯示項目

                            string[] slValue;

                            if (iCount == 0)
                            {
                                slValue
                                    = ("RC_NO,WORK_ORDER,PART_NO,VERSION,SPEC1,FORMER_NO,GOOD_QTY,PROCESSED_QTY,REMARK")
                                    .Split(',');
                            }
                            else if (iCount == 1)
                            {
                                slValue
                                    = ("QTY,ROUTE_NAME,FACTORY_CODE,PROCESS_NAME,SPEC2,BLUEPRINT,SCRAP_QTY,REMAINED_QTY")
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
                                // 為了避免「備註」欄位的高變形，增加一個自由的 row，做為緩衝
                                tableLayoutPanel1.RowCount = iRowCount + 1;

                                int i_row_height = 28;

                                int i_table_height = i_row_height * iRowCount + 15;

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
                                        ForeColor = Color.Navy
                                    };

                                    if (slEdit.IndexOf(slValue[i]) == -1)
                                    {
                                        txtTemp.ReadOnly = true;
                                    }
                                    else
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
                                    = ("流程卡,工單,料號,版本,品名,舊編,良品數,已加工數量,備註")
                                    .Split(',');
                            }
                            else if (iCount == 1)
                            {
                                slValue
                                    = ("流程卡數量,途程名稱,廠別代碼,製程名稱,規格,藍圖,不良品數,剩餘數量")
                                    .Split(',');
                            }
                            else
                            {
                                slValue = dr["PARAM_VALUE"].ToString().Split(',');
                            }

                            #endregion

                            Label lablTemp;

                            for (int i = 0; i < slValue.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(slValue[i]))
                                {
                                    lablTemp = new Label()
                                    {
                                        Font = new Font(this.Font, FontStyle.Bold),
                                        Text = SajetCommon.SetLanguage(slValue[i]),
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
            }

            #region 品保

            // 先檢查有沒有設定品檢項目
            int qc_items_count = QcSrv.GetQCItemsCount(rc_no: txtInput.Text);

            #region Isaac 2021/01/20 檢查是否有 AQL 設定

            need_qc = false;

            sSQL = $@"
SELECT
    A.SAMPLING_TYPE
FROM
    SAJET.SYS_QC_SAMPLING_PLAN A,
    (
        SELECT
            A.SAMPLING_ID
        FROM
            SAJET.SYS_PART_QC_PLAN A,
            (
                SELECT
                    PROCESS_ID,
                    PART_ID
                FROM
                    SAJET.G_RC_STATUS
                WHERE
                    RC_NO = :RC_NO
            ) B
        WHERE
            A.PROCESS_ID = B.PROCESS_ID
            AND A.PART_ID = B.PART_ID
            AND A.ENABLED = 'Y'
    ) B
WHERE
    A.SAMPLING_ID = B.SAMPLING_ID
";
            Params = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", txtInput.Text },
            };

            var ds = ClientUtils.ExecuteSQL(sSQL, Params.ToArray());

            need_qc = ds != null && ds.Tables[0].Rows.Count > 0;

            #endregion

            #region 首件 / 末件

            must_inspect = false;

            bool HasInspectOne = QoSrv.HasInspectOne(rc_no: txtInput.Text);

            var QCOption = QoSrv.GetInspectType(rc_no: txtInput.Text);

            if (!HasInspectOne)
            {
                if ((QCOption & QCOptionEnum.LastPieceInspection) == QCOptionEnum.LastPieceInspection)
                {
                    _label.Text = SajetCommon.SetLanguage(ControlTextEnum.LastPieceInspection.ToString());

                    must_inspect = true;
                }

                if ((QCOption & QCOptionEnum.FirstPieceInspection) == QCOptionEnum.FirstPieceInspection)
                {
                    _label.Text = SajetCommon.SetLanguage(ControlTextEnum.FirstPieceInspection.ToString());

                    must_inspect = true;
                }
            }

            #endregion

            if (qc_items_count > 0)
            {
                if (need_qc || must_inspect)
                {
                    #region 把品保列放到流程卡資訊框的右下區塊

                    int _tableRow = tableLayoutPanel1.RowCount - 2;
                    TableLayoutPanel _newPanel = new TableLayoutPanel();

                    _newPanel.ColumnCount = 4;
                    _newPanel.RowCount = 1;
                    _newPanel.Dock = DockStyle.Fill;
                    _newPanel.Margin = new Padding(0);
                    _newPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                    _newPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                    _newPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                    _newPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

                    var label_1 = new Label()
                    {
                        Text = SajetCommon.SetLanguage("Sample Type"),
                        TextAlign = ContentAlignment.MiddleLeft,
                        Dock = DockStyle.Fill,
                        Margin = new Padding(0),
                    };

                    tableLayoutPanel1.Controls.Add(label_1, 2, _tableRow);

                    this.tableLayoutPanel1.Controls.Add(_newPanel, 3, _tableRow);

                    #endregion

                    _box.GotFocus += new EventHandler(TextBox_GotFocus);

                    _button.Text = SajetCommon.SetLanguage("QC Data Collection");
                    _button.Click += new EventHandler(AQLButton_Click);

                    string text = string.Empty;

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        text = ds.Tables[0].Rows[0]["SAMPLING_TYPE"].ToString();
                    }

                    var label_sampling_type = new Label()
                    {
                        Text = text,
                        TextAlign = ContentAlignment.MiddleLeft,
                        Margin = new Padding(0),
                        Dock = DockStyle.Left
                    };

                    _newPanel.Controls.Add(label_sampling_type, 0, 0);

                    _box.BackColor = Color.Green;

                    if (!CheckSample())
                    {
                        _box.BackColor = Color.Red;
                    }

                    if (must_inspect)
                    {
                        _box.BackColor = Color.Red;
                    }

                    _newPanel.Controls.Add(_box, 1, 0);
                    _newPanel.Controls.Add(_label, 2, 0);
                    _newPanel.Controls.Add(_button, 3, 0);
                }
            }

            #endregion

            SajetCommon.SetLanguageControl(this);

            #region 檢查製程是否使用 T4 / T6 爐，要收集日期碼、爐序

            usingT4OrT6stove = false;

            printWPLabel = false;

            LbT4T6.Text = string.Empty;

            sSQL = @"
SELECT
    a.process_id,
    b.option1,
    b.option2,
    b.option3
FROM
    sajet.g_rc_status          a,
    sajet.sys_process_option   b
WHERE
    a.process_id = b.process_id
    AND ( b.option1 = 'Y'
            OR b.option2 = 'Y'
              OR b.option3 = 'Y' )
    AND rc_no = :rc_no
";
            Params = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "rc_no", txtInput.Text },
            };

            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params.ToArray());

            if (dsTemp != null &&
                dsTemp.Tables[0].Rows.Count > 0)
            {
                var messages = new List<string>();

                // T4: OPTION2
                if (dsTemp.Tables[0].Rows[0]["OPTION2"].ToString() == "Y")
                {
                    messages.Add(SajetCommon.SetLanguage("Has use T4 stove"));

                    usingT4OrT6stove = true;
                }

                // T6: OPTION3
                if (dsTemp.Tables[0].Rows[0]["OPTION3"].ToString() == "Y")
                {
                    messages.Add(SajetCommon.SetLanguage("Has use T6 stove"));

                    usingT4OrT6stove = true;
                }

                // 印防水標籤
                if (dsTemp.Tables[0].Rows[0]["OPTION1"].ToString() == "Y")
                {
                    messages.Add(SajetCommon.SetLanguage("To print WP label"));

                    printWPLabel = true;
                }

                if (usingT4OrT6stove || printWPLabel)
                {
                    LbT4T6.Text = string.Join(Environment.NewLine, messages);
                }
            }

            // 只有設定使用 T4 / T6 爐的製程需要選擇爐順序號碼、日期碼，而且必須收集此項目
            sTOVESEQDataGridViewTextBoxColumn.Visible = usingT4OrT6stove;

            dATECODEDataGridViewTextBoxColumn.Visible = usingT4OrT6stove;

            if (usingT4OrT6stove)
            {
                lOADQTYDataGridViewTextBoxColumn.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 192);
                sTOVESEQDataGridViewTextBoxColumn.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 192);
                dATECODEDataGridViewTextBoxColumn.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 192);
            }

            #endregion

            if (!string.IsNullOrEmpty(txtInput.Text))
            {
                CheckInput();
            }

            txtInput.TextChanged += new EventHandler(TxtInput_TextChanged);

            // Depend system type to show tabpage
            if (OtSrv.SystemType(out g_SystemType))
            {
                // lot control
                this.TpSN.Parent = null;
            }

            #region 自訂報工產出時間

            LbLastReportTime.Text = RcSrv.GetLastReportTime(Runcard: txtInput.Text);

            DtpOutDate.Value = OUT_PROCESS_TIME;

            bool CanSetWipTime = OtSrv.Check_Privilege(EmpID: EmpID, fun: "Set_WIP_Time");

            if (CanSetWipTime)
            {
                TpOutTime.Parent = TcData;
            }
            else
            {
                TpOutTime.Parent = null;
            }

            #endregion

            #region 只有設定有投入 / 產出兩階段的製程站，可以切換機台、設定機台異常代碼

            if (bOneSheet || !OtSrv.Check_Privilege(EmpID: EmpID, fun: "Machine_Change"))
            {
                BtnMachineDown.Enabled = false;

                //BtnMachineDown.Visible = false;

                //gvMachine.Columns[nameof(sTARTTIMEDataGridViewTextBoxColumn)].Visible = false;
            }

            #endregion

            #region 10C 投入製程才能使用
            // 20210316 線上測試，10C 投入時，先不強制綁定 T4 爐資訊
            /*
            sSQL = @"
SELECT
    a.process_id,
    b.option4
FROM
    sajet.g_rc_status          a,
    sajet.sys_process_option   b
WHERE
    a.process_id = b.process_id
    AND b.option4 = 'Y'
    AND rc_no = :rc_no
    AND rc_no LIKE '10C%'
";
            Params = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "rc_no", txtInput.Text },
            };

            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params.ToArray());

            if (dsTemp != null &&
                dsTemp.Tables[0].Rows.Count > 0)
            {
                this.TpT4.Parent = TcData;

                bool check_bom_auth = OtSrv.Check_Privilege(EmpID: EmpID, "10C_T4_CHECK_BOM");

                CbT4CheckBOM.Enabled = check_bom_auth;

                CbT4CheckBOM.Checked = true;

                CbT4CheckBOM.Text = SajetCommon.SetLanguage(ControlTextEnum.CheckTheBOM.ToString());

                DgvT4.VisibleChanged += DgvT4_VisibleChanged;

                DgvT4.EditingControlShowing += DgvT4_EditingControlShowing;

                DgvT4.CellClick += DgvT4_CellClick;
            }
            else
            {
                this.TpT4.Parent = null;
            }
            //*/

            this.TpT4.Parent = null;

            #endregion

            #region Bonus 頁設定
            //this.TpBonus.Parent = null;

            #endregion

            // 流程卡資訊
            RC_NO_INFO = RcSrv.GetRcNoInfo(txtInput.Text);

            #region 換班功能

            // 只有執行產出報工的製程，沒有換班的問題
            CbShift.Enabled
                = false;
            //= !bOneSheet
            //&& SfSrv.CanChangeShift(runcard: RC_NO_INFO, thisShiftEmpID: EmpID, InProcessTime: out IN_PROCESS_TIME);

            CbShift.Checked = false;

            DtpShift.Enabled = TbWorkload.Enabled = false;

            #endregion
        }

        private void FMain_Shown(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(m_nParam))
            {
                txtInput.Focus();
            }

            #region 強調顯示 舊編(OPTION2) 藍圖(OPTION4)

            var new_font = new Font("微軟正黑體", 20, FontStyle.Bold);

            /*
            tsLabelBluePrint.Font = new_font;

            tslabelFormerNO.Font = new_font;
            //*/
            tsLabelBluePrint.Visible = false;

            tslabelFormerNO.Visible = false;

            TslForm.Font = new_font;

            #endregion

            #region 機台按鈕的字、尺寸

            BtnMachineDown.Font = new Font("微軟正黑體", 15, FontStyle.Regular);

            BtnShift.Font = new Font("微軟正黑體", 15, FontStyle.Regular);

            BtnHandover.Font = new Font("微軟正黑體", 15, FontStyle.Regular);

            BtnShiftHistory.Font = new Font("微軟正黑體", 15, FontStyle.Regular);

            int w_button = 0;

            if (BtnMachineDown.Width > w_button)
            {
                w_button = BtnMachineDown.Width;
            }

            if (BtnShift.Width > w_button)
            {
                w_button = BtnShift.Width;
            }

            if (BtnHandover.Width > w_button)
            {
                w_button = BtnHandover.Width;
            }

            if (BtnShiftHistory.Width > w_button)
            {
                w_button = BtnShiftHistory.Width;
            }

            BtnMachineDown.AutoSize = false;

            BtnShift.AutoSize = false;

            BtnHandover.AutoSize = false;

            BtnShiftHistory.AutoSize = false;

            BtnMachineDown.Width = BtnShift.Width = BtnHandover.Width = BtnShiftHistory.Width = w_button;

            BtnMachineDown.Height = BtnShift.Height = BtnHandover.Height = BtnShiftHistory.Height = 35;

            #endregion

            #region 流程卡資訊區塊

            tableLayoutPanel1.RowStyles[5].Height = 30;
            tableLayoutPanel1.RowStyles[7].Height = 30;
            tableLayoutPanel1.RowStyles[8].Height = 35;

            var emphasis_font = new Font("微軟正黑體", 12, FontStyle.Bold);

            #region 顯示工單備註

            string SQL = @"
SELECT
    B.REMARK
FROM
    SAJET.G_RC_STATUS A
   ,SAJET.G_WO_BASE B
WHERE
    A.WORK_ORDER = B.WORK_ORDER
AND A.RC_NO = :RC_NO
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", txtInput.Text }
            };

            DataSet set = ClientUtils.ExecuteSQL(SQL, p.ToArray());

            TControlData remark_control_set =
                m_tControlData
                .First(x => x.sFieldName == "REMARK");

            if (set != null && set.Tables[0].Rows.Count > 0)
            {
                remark_control_set.txtControl.Text = set.Tables[0].Rows[0]["REMARK"].ToString();
            }

            #endregion

            #region 輪班累計加工數量

            ShowShiftQTY();

            TControlData processed_qty_control_set =
                m_tControlData
                .First(x => x.sFieldName == "PROCESSED_QTY");

            // 已加工數量
            processed_qty_control_set.txtControl.BackColor = Color.Khaki;
            processed_qty_control_set.lablControl.ForeColor = Color.Red;

            processed_qty_control_set.txtControl.Font = emphasis_font;
            processed_qty_control_set.lablControl.Font = emphasis_font;

            // 剩餘數量
            TControlData remained_qty_control_set =
                m_tControlData
                .First(x => x.sFieldName == "REMAINED_QTY");

            remained_qty_control_set.txtControl.BackColor = Color.Khaki;
            remained_qty_control_set.lablControl.ForeColor = Color.Red;

            remained_qty_control_set.txtControl.Font = emphasis_font;
            remained_qty_control_set.lablControl.Font = emphasis_font;

            // 舊編
            TControlData former_part_no_control_set =
                m_tControlData
                .First(x => x.sFieldName == "FORMER_NO");

            former_part_no_control_set.txtControl.BackColor = Color.Khaki;
            former_part_no_control_set.lablControl.ForeColor = Color.Red;

            former_part_no_control_set.txtControl.Font = emphasis_font;
            former_part_no_control_set.lablControl.Font = emphasis_font;

            // 藍圖
            TControlData blueprint_control_set =
                m_tControlData
                .First(x => x.sFieldName == "BLUEPRINT");

            blueprint_control_set.txtControl.BackColor = Color.Khaki;
            blueprint_control_set.lablControl.ForeColor = Color.Red;

            blueprint_control_set.txtControl.Font = emphasis_font;
            blueprint_control_set.lablControl.Font = emphasis_font;

            // 製程名稱
            TControlData process_name_control_set =
                m_tControlData
                .First(x => x.sFieldName == "PROCESS_NAME");

            process_name_control_set.txtControl.BackColor = Color.Khaki;
            process_name_control_set.lablControl.ForeColor = Color.Red;

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

            #endregion

            #region 頁籤項目位置

            int i_position_1 = 0;
            int i_position_2 = 0;
            int i_default_distance = 10;

            // 報工時間頁籤
            {
                i_position_1 = label7.Right;

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
                i_position_1 = LbRework.Right;

                i_position_2 = TbRework.Left;

                if (i_position_1 > i_position_2)
                {
                    TbRework.Left = i_position_1 + i_default_distance;
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

                if (TbWorkHour.Left > i_position_2)
                {
                    i_position_2 = TbWorkHour.Left;
                }

                if (i_position_1 > i_position_2)
                {
                    TbBonus.Left = i_position_1 + i_default_distance;

                    TbWorkHour.Left = i_position_1 + i_default_distance;
                }
            }

            // 作業人數
            {
                i_position_1 = label4.Right;

                i_position_2 = Tb_OP_count.Left;

                if (i_position_1 > i_position_2)
                {
                    Tb_OP_count.Left = i_position_1 + i_default_distance;
                }
            }

            #endregion

            this.Text += $" ( {SajetCommon.g_sFileName} : {SajetCommon.g_sFileVersion} )";
        }

        #region Isaac 2021/01/20 AQL Check

        /// <summary>
        /// 呼叫 RCAQL dll 視窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AQLButton_Click(object sender, EventArgs e)
        {
            try
            {
                // 呼叫 DLL
                string routing
                        = Application.StartupPath + Path.DirectorySeparatorChar
                        + ClientUtils.fCurrentProject + Path.DirectorySeparatorChar;

                string dllInfo = routing + "RCAQL.dll";

                Assembly assembly = Assembly.LoadFrom(dllInfo);


                object[] arg = new object[] { txtInput.Text, tsEmp.Text };
                string[] Name = assembly.FullName.ToString().Split(',');


                Type type = assembly.GetType(Name[0] + ".FData");

                object obj = assembly.CreateInstance(type.FullName, true, BindingFlags.CreateInstance, null, arg, null, null);

                Form formChild = (Form)obj;

                if (formChild.ShowDialog() == DialogResult.OK)
                {
                    var check_samples_pass = CheckSample();

                    var has_one_sample = QoSrv.HasInspectOne(rc_no: txtInput.Text);

                    if (has_one_sample)
                    {
                        _label.Text = string.Empty;
                    }

                    if (check_samples_pass && has_one_sample)
                    {
                        _box.BackColor = Color.Green;
                    }
                    else
                    {
                        _box.BackColor = Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                string error_message
                    = "RCAQL.dll Error."
                    + Environment.NewLine
                    + ex.Message
                    + Environment.NewLine
                    + ex.InnerException.Message
                    ;

                MessageBox.Show(error_message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 當點擊文字方塊時，會將焦點移至按鈕上
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_GotFocus(object sender, EventArgs e)
        {
            _button.Focus();
        }

        /// <summary>
        /// 檢查是否符合過站
        /// </summary>
        /// <returns></returns>
        private bool CheckSample()
        {
            bool result = true;

            // 先檢查總已抽數大於等於總應抽數
            string sTotalWipIn = QcSrv.SumTotal(txtInput.Text);
            string sTotalSampled = QcSrv.GetTotalData(txtInput.Text);
            string sTotalBeSampling = QcSrv.GetTotalBeSampling(sTotalWipIn, txtInput.Text);

            if (int.TryParse(sTotalSampled, out int iTotalSampled) &&
                int.TryParse(sTotalBeSampling, out int iTotalBeSampling))
            {
                if (iTotalSampled < iTotalBeSampling)
                {
                    result = false;
                }
            }

            if (!result)
            {
                // 檢查此 RC 可抽數是否為零

                // RC 已抽數
                string sRCSampled = QcSrv.GetRCSampled(txtInput.Text);

                string sRCBeSampling
                    = QcSrv.GetRCBeSampling(BeSampling: sTotalBeSampling,
                                            Sampled: sTotalSampled,
                                            RCSampled: sRCSampled,
                                            RC_NO: txtInput.Text);

                if (sRCBeSampling == "0")
                {
                    result = true;
                }
            }

            return result;
        }

        #endregion

        private void TxtInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
            {
                return;
            }

            CheckInput();

            txtInput.SelectAll();
        }

        /// <summary>
        /// 輸入良品數，剩餘不良品數為目前數量減良品數先放入第一個不良現象的不良品數剩餘不良原因讓使用者調配
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtGood_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
            {
                return;
            }

            if (!OtSrv.IsNumeric(4, programInfo.txtGood.Text))
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Data Invalid"), 0);

                programInfo.txtGood.SelectAll();

                programInfo.txtGood.Focus();

                return;
            }
            else
            {
                double dInput = Convert.ToDouble(programInfo.dsRC.Tables[0].Rows[0]["QTY"].ToString());

                double dGood = Convert.ToDouble(programInfo.txtGood.Text);

                if (dGood > dInput)
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Data Abnormal"), 0);

                    programInfo.txtGood.SelectAll();

                    programInfo.txtGood.Focus();

                    return;
                }
                else
                {
                    double dScrap = dInput - dGood;

                    programInfo.txtScrap.Text = dScrap.ToString();

                    btxtGood = true;

                    if (DgvDefect.Rows.Count > 0)
                    {
                        DgvDefect.Rows[0].Cells["QTY"].Value = programInfo.txtScrap.Text;
                    }
                }
            }
        }

        private void TxtInput_TextChanged(object sender, EventArgs e)
        {
            ClearData();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            string sInput = string.Empty;

            string message = string.Empty;

            List<object[]> Params;

            // 加工數量
            int current_qty = int.Parse(RC_NO_INFO["CURRENT_QTY"].ToString());

            #region 檢查良品數

            double dInput, dGood, dScrap;

            if (!OtSrv.IsNumeric(4, programInfo.txtGood.Text))
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Data Invalid"), 0);

                programInfo.txtGood.SelectAll();

                programInfo.txtGood.Focus();

                return;
            }
            else
            {
                dInput = Convert.ToDouble(programInfo.dsRC.Tables[0].Rows[0]["QTY"].ToString());

                dGood = Convert.ToDouble(programInfo.txtGood.Text);

                if (dGood > dInput)
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Data Abnormal"), 0);

                    programInfo.txtGood.SelectAll();

                    programInfo.txtGood.Focus();

                    return;
                }
                else
                {
                    dScrap = Convert.ToDouble(programInfo.txtScrap.Text);

                    if (dInput != dGood + dScrap)
                    {
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("Data Abnormal"), 0);

                        programInfo.txtGood.SelectAll();

                        programInfo.txtGood.Focus();

                        return;
                    }

                    double iQty = 0;

                    for (int i = 0; i < DgvDefect.Rows.Count; i++)
                    {
                        if (!OtSrv.IsNumeric(4, DgvDefect.Rows[i].Cells[DgvDefect.Columns.Count - 1].Value.ToString()))
                        {
                            return;
                        }

                        iQty += Convert.ToDouble(DgvDefect.Rows[i].Cells[DgvDefect.Columns.Count - 1].Value.ToString());

                        //iScrap = iScrap + iQty;
                    }

                    if (iQty != Convert.ToDouble(programInfo.txtScrap.Text) && DgvDefect.Rows.Count > 0)
                    {
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("Defect Quantity Invalid."), 0);

                        return;
                    }
                }
            }

            #endregion

            // 負載量
            int total_load_qty = 0;

            int total_load_before = SfSrv.GetMachineTotalLoadBefore(RC_NO_INFO);

            # region Process Machine

            string sMachine = string.Empty;

            var machines = new List<MachineDownModel>();

            if (DgvMachine.Rows.Count > 0) // 檢查是否有勾選
            {
                // 畫面上有的機台產出時都要記錄
                for (int i = 0; i < DgvMachine.Rows.Count; i++)
                {
                    sMachine
                        += DgvMachine.Rows[i]
                        .Cells[nameof(mACHINECODEDataGridViewTextBoxColumn)]
                        .EditedFormattedValue.ToString() + (char)9;

                    //machines.Add(gvMachine.Rows[i].Cells[nameof(mACHINEIDDataGridViewTextBoxColumn)].EditedFormattedValue.ToString());
                    machines.Add(new MachineDownModel
                    {
                        MACHINE_ID = int.Parse(DgvMachine.Rows[i].Cells[nameof(mACHINEIDDataGridViewTextBoxColumn)].EditedFormattedValue.ToString()),
                        LOAD_QTY = int.Parse(DgvMachine.Rows[i].Cells[nameof(lOADQTYDataGridViewTextBoxColumn)].EditedFormattedValue.ToString()),
                        DATE_CODE = DgvMachine.Rows[i].Cells[nameof(dATECODEDataGridViewTextBoxColumn)].EditedFormattedValue.ToString(),
                        STOVE_SEQ = DgvMachine.Rows[i].Cells[nameof(sTOVESEQDataGridViewTextBoxColumn)].EditedFormattedValue.ToString(),
                        START_TIME = DateTime.Parse(DgvMachine.Rows[i].Cells[nameof(sTARTTIMEDataGridViewTextBoxColumn)].EditedFormattedValue.ToString()),
                    });
                }
            }
            else if (McSrv.HaveSetProcessMachine(rc_no: txtInput.Text))
            {
                // 製程有綁定機台，則投入的時候一定有選機台
                // 如果產出的時候，沒有任何正在加工的機台，則不能產出
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please set working machine"), 0);

                return;
            }

            total_load_qty = machines.Sum(x => x.LOAD_QTY ?? 0);

            if (machines.Count == 1 && machines[0].LOAD_QTY <= 0)
            {
                WipOutList[0].LOAD_QTY = current_qty - total_load_before;

                machines[0].LOAD_QTY = current_qty - total_load_before;
            }

            if (total_load_before > 0
                || total_load_qty > 0
                || machines.Count > 1)
            {
                if (total_load_qty + total_load_before != current_qty)
                {
                    message = SajetCommon.SetLanguage("The total load of the machine must match the runcard's current quantity");

                    SajetCommon.Show_Message(message, 0);

                    return;
                }
            }

            #endregion

            # region Keyparts Collection

            string skeypart = string.Empty;

            if (bOneSheet)
            {
                if (!CheckKPSNInput())
                {
                    //tabControl1.SelectedIndex = 6;

                    TcData.SelectedTab = TpKeyparts;

                    editKPSN.Focus();

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

            #region 檢查非元件是否有輸入良品數, 不良品數

            if (programInfo.txtGood.Enabled)
            {
                if (string.IsNullOrEmpty(programInfo.txtGood.Text))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Good Qty Empty"), 0);

                    programInfo.txtGood.Focus();

                    programInfo.txtGood.SelectAll();

                    return;
                }

                if (string.IsNullOrEmpty(programInfo.txtScrap.Text))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Scrap Qty Empty"), 0);

                    programInfo.txtScrap.Focus();

                    programInfo.txtScrap.SelectAll();

                    return;
                }

                decimal ivalue, ivalue2;

                try
                {
                    ivalue = decimal.Parse(programInfo.txtGood.Text);

                    if (ivalue < 0)
                    {
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("Good Qty Invalid"), 0);

                        programInfo.txtGood.Focus();

                        programInfo.txtGood.SelectAll();

                        return;
                    }
                }
                catch
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Data Invalid"), 0);

                    programInfo.txtGood.Focus();

                    programInfo.txtGood.SelectAll();

                    return;
                }

                try
                {
                    ivalue2 = decimal.Parse(programInfo.txtScrap.Text);

                    if (ivalue2 < 0)
                    {
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("Scrap Qty Invalid"), 0);

                        programInfo.txtGood.Focus();

                        programInfo.txtGood.SelectAll();

                        return;
                    }
                }
                catch
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Data Invalid"), 0);

                    programInfo.txtScrap.Focus();

                    programInfo.txtScrap.SelectAll();

                    return;
                }

                if (ivalue + ivalue2 == 0)
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Sum=0"), 0);

                    programInfo.txtScrap.Focus();

                    programInfo.txtScrap.SelectAll();

                    return;
                }
            }

            // check defect qty valid again
            for (int i = 0; i < DgvDefect.Rows.Count; i++)
            {
                if (!OtSrv.IsNumeric(4, DgvDefect.Rows[i].Cells["QTY"].Value.ToString()))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Input Quantity in Positive integer"), 0);

                    return;
                }
            }

            #endregion

            #region 檢查資料收集是否有輸入

            foreach (DataGridViewRow dr in DgvInput.Rows)
            {
                if (!string.IsNullOrEmpty(dr.Cells["VALUE_DEFAULT"].ErrorText))
                {
                    SajetCommon.Show_Message(dr.Cells["ITEM_NAME"].EditedFormattedValue.ToString() + dr.Cells["VALUE_DEFAULT"].ErrorText, 0);

                    DgvInput.CurrentCell = dr.Cells["VALUE_DEFAULT"];

                    DgvInput.Focus();

                    TcData.SelectedIndex = 0;

                    return;
                }

                if (!string.IsNullOrEmpty(dr.Cells["VALUE_DEFAULT"].EditedFormattedValue.ToString()))
                {
                    sInput += dr.Cells["ITEM_ID"].EditedFormattedValue.ToString() + (char)9
                        + dr.Cells["VALUE_DEFAULT"].EditedFormattedValue.ToString() + (char)27;
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
                        message
                            = SajetCommon.SetLanguage("Defect Qty Empty")
                            + Environment.NewLine
                            + DgvSN.Columns[0].HeaderText
                            + ": "
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

                    TcData.SelectedTab = TpSN;

                    return;
                }

                // 2015/08/26, Aaron
                if (!string.IsNullOrEmpty(dr.Cells["SCRAP_QTY"].ErrorText))
                {
                    SajetCommon.Show_Message(DgvSN.Columns["SCRAP_QTY"].HeaderText + dr.Cells["SCRAP_QTY"].ErrorText, 0);

                    DgvSN.CurrentCell = dr.Cells["SCRAP_QTY"];

                    DgvSN.Focus();

                    TcData.SelectedTab = TpSN;

                    return;
                }

                string sSNTemp
                    = dr.Cells["SERIAL_NUMBER"].EditedFormattedValue.ToString() + (char)9
                    + dr.Cells["CURRENT_STATUS"].EditedFormattedValue.ToString() + (char)9
                    + dr.Cells["GOOD_QTY"].EditedFormattedValue.ToString() + (char)9
                    + dr.Cells["SCRAP_QTY"].EditedFormattedValue.ToString();

                if (programInfo.iSNInput[1] > -1)
                {
                    for (int i = programInfo.iSNInput[1]; i < DgvSN.Columns.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(dr.Cells[i].ErrorText))
                        {
                            SajetCommon.Show_Message(DgvSN.Columns[i].HeaderText + dr.Cells[i].ErrorText, 0);

                            DgvSN.CurrentCell = dr.Cells[i];

                            DgvSN.Focus();

                            TcData.SelectedTab = TpSN;

                            return;
                        }

                        if (!string.IsNullOrEmpty(dr.Cells[0].EditedFormattedValue.ToString()))
                        {
                            sSN += sSNTemp + (char)9
                                + DgvSN.Columns[i].Tag.ToString() + (char)9
                                + dr.Cells[i].EditedFormattedValue.ToString() + (char)27;
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

            #region 將元件或不良代碼組成字串

            string sDefect = string.Empty;

            foreach (DataGridViewRow dr in DgvDefect.Rows)
            {
                if (DgvDefect.Columns.Contains("SERIAL_NUMBER"))
                {
                    sDefect += dr.Cells["SERIAL_NUMBER"].EditedFormattedValue.ToString() + (char)9 + dr.Cells["DEFECT_CODE"].EditedFormattedValue.ToString() + (char)9 + dr.Cells["QTY"].EditedFormattedValue.ToString() + (char)27;
                }
                else if (Convert.ToDouble(dr.Cells["QTY"].EditedFormattedValue.ToString()) > 0)
                {
                    sDefect += dr.Cells["DEFECT_CODE"].EditedFormattedValue.ToString() + (char)9 + dr.Cells["QTY"].EditedFormattedValue.ToString() + (char)27;
                }
            }

            #endregion

            # region 如果 Bonus 欄位有輸入檢查數值是否正確

            if (!string.IsNullOrEmpty(TbBonus.Text))
            {
                if (!OtSrv.IsNumeric(5, TbBonus.Text))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Bouns Quantity Error."), 0);

                    TcData.SelectedTab = TpBonus;

                    TbBonus.SelectAll();

                    TbBonus.Focus();

                    return;
                }
                else
                {
                    if (!CheckBonus(TbBonus.Text))
                    {
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("Bouns Quantity Invalid."), 0);

                        TcData.SelectedTab = TpBonus;

                        TbBonus.SelectAll();

                        TbBonus.Focus();

                        return;
                    }
                }
            }
            else
            {
                TbBonus.Text = "0";
            }

            #endregion

            # region 如果 Work Hour 欄位有輸入檢查數值是否正確

            if (!string.IsNullOrEmpty(TbWorkHour.Text))
            {
                if (!OtSrv.IsNumeric(1, TbWorkHour.Text))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Work Hour Error."), 0);

                    TcData.SelectedTab = TpBonus;

                    TbWorkHour.SelectAll();

                    TbWorkHour.Focus();

                    return;
                }

                if (TbWorkHour.Text.Length > 5)
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Please enter up to five digits."), 0);

                    TcData.SelectedTab = TpBonus;

                    TbWorkHour.SelectAll();

                    TbWorkHour.Focus();

                    return;
                }
            }
            else
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Input Work Hour."), 0);

                TcData.SelectedTab = TpBonus;

                TbWorkHour.SelectAll();

                TbWorkHour.Focus();

                return;
            }

            #endregion

            # region 判斷是否重工

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
                Params = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", txtInput.Text },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", programInfo.dsRC.Tables[0].Rows[0]["PROCESS_ID"].ToString() }
                };

                DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params.ToArray());

                if (dsTemp != null &&
                    dsTemp.Tables[0].Rows.Count > 0)
                {
                    sTSTATUS = "1";
                }
            }
            catch (Exception)
            {
                sTSTATUS = "";
            }

            #endregion

            #region 重工數量

            if (!CheckReworkQty())
            {
                return;
            }

            #endregion

            #region 自訂報工產出時間

            OUT_PROCESS_TIME = OtSrv.GetDBDateTimeNow();

            var time = CbReportTime.Checked ? DtpOutDate.Value : OUT_PROCESS_TIME;

            if (!RcSrv.IsOutputTimeValid(runcard: RC_NO_INFO, OutProcessTime: time, message: out message))
            {
                SajetCommon.Show_Message(message, 0);

                return;
            }

            #endregion

            #region 檢查換班時間的設定值

            if (CbShift.Checked)
            {
                // 換班時間
                if (DtpShift.Value <= IN_PROCESS_TIME)
                {
                    message
                        = SajetCommon.SetLanguage(MessageEnum.TheShiftTimeMustBeLaterThanTheInProcessTime.ToString())
                        + Environment.NewLine
                        + SajetCommon.SetLanguage(MessageEnum.InProcessTime.ToString())
                        + ": "
                        + IN_PROCESS_TIME.ToString("yyyy/ MM/ dd HH: mm: ss")
                        ;
                    SajetCommon.Show_Message(message, 0);

                    return;
                }

                if (DtpShift.Value > OUT_PROCESS_TIME)
                {
                    message = SajetCommon.SetLanguage(MessageEnum.TheShiftTimeMustBeEarlierThanTheOutProcessTime.ToString())
                        + Environment.NewLine
                        + SajetCommon.SetLanguage(MessageEnum.OutProcessTime.ToString())
                        + ": "
                        + OUT_PROCESS_TIME.ToString("yyyy/ MM/ dd HH: mm: ss")
                        ;
                    SajetCommon.Show_Message(message, 0);

                    return;
                }

                int.TryParse(TbWorkload.Text, out int shift_workLoad);

                if (shift_workLoad < 0)
                {
                    message = SajetCommon.SetLanguage(MessageEnum.TheShiftWorkloadMustBeANonNegativeInteger.ToString());

                    SajetCommon.Show_Message(message, 0);

                    return;
                }
                else if (shift_workLoad > current_qty)
                {
                    message = SajetCommon.SetLanguage(MessageEnum.TheShiftWorkloadMustNotExceedTheCurrentQuantityOfTheRuncard.ToString());

                    SajetCommon.Show_Message(message, 0);

                    return;
                }

                foreach (DataGridViewRow row in DgvMachine.Rows)
                {
                    int.TryParse(row.Cells[nameof(lOADQTYDataGridViewTextBoxColumn)].Value.ToString(), out int load_qty);

                    if (load_qty < shift_workLoad)
                    {
                        message = SajetCommon.SetLanguage(MessageEnum.TheShiftWorkloadMustNotExceedTheMachineLoad.ToString());

                        SajetCommon.Show_Message(sKeyMsg: message, iType: 0);

                        return;
                    }
                }
            }

            #endregion

            #region 10C 的檢查

            // 重新讀取 T4 已綁定流程卡的數量等數據
            // 避免多個人員同時操作造成數據有些出入
            T4Renew();

            // 檢查綁定數量
            var T4Temp = T4List.Where(x => x.BINDING_QTY > 0).ToList();

            if (TpT4.Parent != null &&
                !T4Srv.CheckBindingQty(RcInFo: ref RC_NO_INFO, T4List: ref T4Temp, message: out message))
            {
                SajetCommon.Show_Message(message, 0);

                return;
            }

            // 檢查 BOM 表關聯
            var t4_not_pass = new List<string>();

            if (CbT4CheckBOM.Checked)
            {
                foreach (var t4_rc in T4Temp)
                {
                    if (!T4Srv.Is_TheBom_OfThePartNo_OfTheRuncards_Matches(parentRC: txtInput.Text, childRC: t4_rc.RC_NO_T4))
                    {
                        t4_not_pass.Add(t4_rc.RC_NO_T4);
                    }
                }
            }

            if (t4_not_pass.Count > 0)
            {
                message
                    = SajetCommon.SetLanguage(MessageEnum.IsNotChildBOM.ToString())
                    + " : "
                    + Environment.NewLine
                    + string.Join("," + Environment.NewLine, t4_not_pass);

                SajetCommon.Show_Message(message, 0);

                return;
            }

            #endregion

            #region 作業人數

            if (string.IsNullOrWhiteSpace(Tb_OP_count.Text))
            {
                message = SajetCommon.SetLanguage(MessageEnum.BindingQtyMoreThanT4Load.ToString());

                SajetCommon.Show_Message(message, 0);

                return;
            }

            if (Tb_OP_count.Text.Trim() == "1")
            {
                Tb_OP_count.Text = SfSrv.GetOperatorCount(
                    rc_info: RC_NO_INFO,
                    emp_id: EmpID).ToString();
            }

            #endregion

            #region Isaac 2021/01/20 AQL 抽驗檢查
            if (need_qc && !CheckSample())
            {
                MessageBox.Show(SajetCommon.SetLanguage("AQL_Error"),
                                SajetCommon.SetLanguage("Error"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                AQLButton_Click(null, null);

                return;
            }

            if (must_inspect && !QoSrv.HasInspectOne(rc_no: RC_NO_INFO["RC_NO"].ToString()))
            {
                MessageBox.Show(SajetCommon.SetLanguage("AQL_Error"),
                                SajetCommon.SetLanguage("Error"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                AQLButton_Click(null, null);

                return;
            }

            #endregion

            // 選擇下一製程
            if (!SetNextProcess())
            {
                return;
            }
            else
            {
                #region 檢查結束製程
                string sSQL = $@" SELECT COUNT(*) COUNT 
                                    FROM SAJET.SYS_END_PROCESS A, 
                                         SAJET.G_RC_STATUS     B
                                   WHERE A.PART_ID = B.PART_ID 
                                     AND A.ROUTE_ID = B.ROUTE_ID
                                     AND A.NODE_ID = B.NODE_ID 
                                     AND A.PROCESS_ID = B.PROCESS_ID 
                                     AND B.RC_NO = '{txtInput.Text}' 
                                     AND A.ENABLED = 'Y' ";

                var result = ClientUtils.ExecuteSQL(sSQL);
                if (result.Tables[0].Rows[0]["COUNT"].ToString() != "0")
                {
                    rcRoute.sNext_Process = "0";
                }
                #endregion
            }

            Params = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TEMP", tsEmp.Text },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TRC", txtInput.Text },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TITEM", sInput },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TDEFECT", sDefect },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TSN", sSN },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TMEMO", tsMemo.Text },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TGOOD", programInfo.txtGood.Text },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TSCRAP", programInfo.txtScrap.Text },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TNEXTPROCESS", rcRoute.sNext_Process },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TNEXTNODE", rcRoute.sNext_Node },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TNEXTSHEET", rcRoute.sSheet_Name },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TBONUS", TbBonus.Text },
                new object[] { ParameterDirection.Input, OracleType.DateTime, "TNOW", time },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TSTATUS", sTSTATUS },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TMACHINE", sMachine },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TKEYPART", skeypart },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TWORKHOUR", TbWorkHour.Text.Trim() },
                new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" }
            };

            DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_RC_OUTPUT", Params.ToArray());

            string sMsg = ds.Tables[0].Rows[0]["TRES"].ToString();

            if (sMsg == "OK")
            {
                // 有設定重工數量才需要記錄
                if (!string.IsNullOrWhiteSpace(TbRework.Text) &&
                    int.TryParse(TbRework.Text, out int iRework) &&
                    iRework > 0)
                {
                    RcSrv.SaveReworkQty(
                        rc_no: txtInput.Text,
                        emp_id: EmpID,
                        node_id: rcRoute.sNode_Id,
                        travel_id: rcRoute.sTravel_Id,
                        rework_qty: iRework);
                }

                // 記錄機台資訊到停機代碼表
                McSrv.RecordMachine(
                    rc_info: RC_NO_INFO,
                    machines: machines,
                    end_time: time,
                    changed_shift: CbShift.Checked,
                    shift_time: DtpShift.Value,
                    workload: TbWorkload.Text,
                    emp_id: EmpID,
                    is_one_sheet: bOneSheet);

                // 記錄 10C 流程卡 和 T4 機台加工數量 的綁定資訊
                if (TpT4.Parent != null)
                {
                    T4Srv.SaveT4Bindings(
                        now: time,
                        updateUserID: EmpID,
                        check_bom: CbT4CheckBOM.Checked,
                        T4List: ref T4Temp);
                }

                // 登記作業人數
                if (!int.TryParse(Tb_OP_count.Text, out int op_count))
                {
                    op_count = 1;
                }

                RcSrv.SaveOPCount(
                    rc_no_info: RC_NO_INFO,
                    emp_id: EmpID,
                    op_count: op_count,
                    update_time: time);

                // 列印標籤
                if (printWPLabel)
                {
                    OtSrv.OKtoPrint(
                        rc_no: txtInput.Text,
                        process_id: RC_NO_INFO["PROCESS_ID"].ToString(),
                        message: out message);
                }

                DialogResult = DialogResult.OK;

                this.Close();
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

        private void BtnMachineDown_Click(object sender, EventArgs e)
        {
            try
            {
                using (var f = new fMachineChange
                {
                    ProcessID = programInfo.dsRC.Tables[0].Rows[0]["PROCESS_ID"].ToString(),
                    Runcard = txtInput.Text,
                    EmpID = EmpID,
                    usingT4OrT6stove = usingT4OrT6stove,
                })
                {
                    f.ShowDialog();

                    var d = McSrv.GetWorkingMachineList(txtInput.Text);

                    WipOutList = McSrv.GetModels(d);

                    // 如果要設定爐序，預設先選擇第一項
                    if (usingT4OrT6stove)
                    {
                        WipOutList.ForEach(x =>
                        {
                            x.STOVE_SEQ = string.IsNullOrWhiteSpace(x.STOVE_SEQ) ? DateGridViewComboBoxItems[0] : x.STOVE_SEQ;
                        });
                    }

                    DgvMachine.DataSource = WipOutList;

                    DgvMachine.Update();

                    DgvMachine.Refresh();
                }

                OUT_PROCESS_TIME = DtpOutDate.Value = OtSrv.GetDBDateTimeNow();

                ShowShiftQTY();
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);
            }
        }

        private void DgvDefect_MouseDown(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hit = DgvDefect.HitTest(e.X, e.Y);

            if (hit.RowIndex < 0 | hit.ColumnIndex < 0)
            {
                DgvDefect.EndEdit();
            }
        }

        private void GvDefect_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= NumericTextBoxColumn_KeyPress;

            if (DgvDefect.CurrentCell.ColumnIndex == DgvDefect.Columns["QTY"].Index)
            {
                if (e.Control is TextBox x)
                {
                    x.KeyPress += NumericTextBoxColumn_KeyPress;
                }
            }
        }

        #region  在編輯 LOAD_QTY 欄位或是 DATE_CODE 欄位的內容時，為了能檢查輸入值內容綁定的事件

        private void GvMachine_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= NumericTextBoxColumn_KeyPress;

            e.Control.Leave -= DgvMachineTextBoxColumn_Leave;

            if (DgvMachine.CurrentCell.ColumnIndex == dATECODEDataGridViewTextBoxColumn.Index ||
                DgvMachine.CurrentCell.ColumnIndex == lOADQTYDataGridViewTextBoxColumn.Index)
            {
                if (e.Control is TextBox x)
                {
                    x.KeyPress += NumericTextBoxColumn_KeyPress;

                    x.Leave += DgvMachineTextBoxColumn_Leave;
                }
            }
        }

        /// <summary>
        /// 結束編輯狀態時的資料驗證
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvMachineTextBoxColumn_Leave(object sender, EventArgs e)
        {
            if (DgvMachine.CurrentCell.ColumnIndex == lOADQTYDataGridViewTextBoxColumn.Index &&
                sender is DataGridViewTextBoxEditingControl x &&
                string.IsNullOrWhiteSpace(x.Text))
            {
                x.Text = "0";
            }
        }

        #endregion

        #region T4 的事件

        /// <summary>
        /// 點擊 DataGridViewButtonColumn 上任意一項 T4 流程卡的移除按鈕，移除該項目的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvT4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DgvT4.Rows.Count <= 0)
            {
                return;
            }

            var dgvSender = sender as DataGridView;

            if (dgvSender.Columns[e.ColumnIndex] is DataGridViewButtonColumn b)
            {
                var remove_row = dgvSender.CurrentRow.DataBoundItem as T4MachineModel;

                T4List
                    = T4List
                    .Where(x =>
                    {
                        return
                        x.RC_NO_T4 != remove_row.RC_NO_T4 ||
                        x.MACHINE_ID != remove_row.MACHINE_ID;
                    }).ToList();

                BindingDataGridView(targetName: nameof(DgvT4), model: T4List);

                DgvT4_CellClick(null, null);
            }
        }

        private void TcData_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TcData.SelectedTab == TpT4)
            {
                TbRcT4.Focus();
            }
        }

        /// <summary>
        /// 在 TextBox 按下 Enter 也可以觸發查詢
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbRcT4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                BtnAddT4_Click(null, null);
            }
        }

        /// <summary>
        /// 檢查流程卡是否可以用、並且帶入資訊到 DataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddT4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbRcT4.Text)) return;

            string message = string.Empty;

            string T4Runcard = TbRcT4.Text.Trim();

            TbRcT4.Text = string.Empty;

            #region 檢查刷入的流程卡能不能用

            // 不能重複刷
            if (T4List.Any(x => x.RC_NO_T4 == T4Runcard))
            {
                message = SajetCommon.SetLanguage(MessageEnum.DuplicateRuncard.ToString());

                SajetCommon.Show_Message(message, 0);

                return;
            }

            // 必須是 10C 流程卡的料號的子階料號
            // 但是可以設定要不要檢查 BOM
            if (CbT4CheckBOM.Checked &&
                !T4Srv.Is_TheBom_OfThePartNo_OfTheRuncards_Matches(parentRC: txtInput.Text, childRC: T4Runcard))
            {
                message = SajetCommon.SetLanguage(MessageEnum.IsNotChildBOM.ToString());

                SajetCommon.Show_Message(message, 0);

                return;
            }

            // 10B 流程卡要跑完生產途程
            if (!T4Srv.IsProductionComplete(T4RC: T4Runcard))
            {
                message = SajetCommon.SetLanguage(MessageEnum.RuncardNotComplete.ToString());

                SajetCommon.Show_Message(message, 0);

                return;
            }

            // 有沒有 T4 可以綁定
            if (!T4Srv.HasT4inRoute(T4RC: T4Runcard, routeDetail: out DataTable routeDetail))
            {
                message = SajetCommon.SetLanguage(MessageEnum.NoT4ProcessInRoute.ToString());

                SajetCommon.Show_Message(message, 0);

                return;
            }

            // T4 是哪個製程
            T4_ProcessID = routeDetail.Rows.Cast<DataRow>()
                .FirstOrDefault(x => x["T4"].ToString() == "Y")
                ["PROCESS_ID"].ToString();

            // T6 是哪個製程
            var T6Process = routeDetail.Rows.Cast<DataRow>()
                .DefaultIfEmpty(null)
                .FirstOrDefault(x => x["T6"].ToString() == "Y");

            if (T6Process != null)
            {
                T6_ProcessID = T6Process["PROCESS_ID"].ToString();
            }

            #endregion

            #region 載入資料

            var T4RuncardInfo = RcSrv.GetRcNoInfo(Runcard: T4Runcard, processID: T4_ProcessID);

            var T4MachineTemp = T4Srv.GetT4MachineList(T4RC: T4Runcard, T4ProcessID: T4_ProcessID);

            var T4UsedTemp = T4Srv.GetT4MachineUsedRecords(T4RC: T4Runcard, T4ProcessID: T4_ProcessID);

            T4MachineTemp = T4Srv.Merge(T4Machine: T4MachineTemp, T4Used: T4UsedTemp, RcInFo: RC_NO_INFO);

            // 取得資料後再做的檢查
            // 數量是不是用完了
            var excludeNoQtyList = T4MachineTemp
                .Where(x => x.LOAD > x.BOUND_QTY)
                .ToList();

            if (excludeNoQtyList.Count <= 0)
            {
                message = SajetCommon.SetLanguage(MessageEnum.NoT4LoadCanBeBound.ToString());

                SajetCommon.Show_Message(message, 0);

                return;
            }

            T4List.AddRange(T4MachineTemp);

            T4List = T4Srv.Sort(T4List).ToList();

            BindingDataGridView(targetName: nameof(DgvT4), model: T4List);

            #endregion

            #region 點選第一張 T4 流程卡

            var T4Row = DgvT4.Rows.Cast<DataGridViewRow>()
                .DefaultIfEmpty(null)
                .FirstOrDefault(x =>
                {
                    string rc = x.Cells[nameof(rCNOT4DataGridViewTextBoxColumn)].Value.ToString();

                    return rc == T4Runcard;
                });

            if (T4Row != null)
            {
                DgvT4.CurrentCell = T4Row.Cells[DgvT4.FirstDisplayedCell.ColumnIndex];

                DgvT4_CellClick(null, null);
            }

            #endregion

            TbRcT4.Focus();
        }

        private void BtnRemoveT4_Click(object sender, EventArgs e)
        {
            var current_t4_rc = (DgvT4.CurrentRow.DataBoundItem as T4MachineModel);

            T4List = T4List.Where(x => x.RC_NO_T4 != current_t4_rc.RC_NO_T4).ToList();

            BindingDataGridView(targetName: nameof(DgvT4), model: T4List);

            DgvT4_CellClick(null, null);
        }

        private void DgvT4_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= NumericTextBoxColumn_KeyPress;

            e.Control.Leave -= DgvT4_Leave;

            if (DgvT4.CurrentCell.ColumnIndex == DgvT4.Columns[nameof(bINDINGQTYDataGridViewTextBoxColumn)].Index)
            {
                if (e.Control is TextBox x)
                {
                    x.KeyPress += NumericTextBoxColumn_KeyPress;

                    x.Leave += DgvT4_Leave;
                }
            }
        }

        /// <summary>
        /// 結束編輯狀態時的資料驗證
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvT4_Leave(object sender, EventArgs e)
        {
            if (DgvT4.CurrentCell.ColumnIndex == DgvT4.Columns[nameof(bINDINGQTYDataGridViewTextBoxColumn)].Index &&
                sender is DataGridViewTextBoxEditingControl x &&
                string.IsNullOrWhiteSpace(x.Text))
            {
                x.Text = "0";
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

        private void DgvT4_VisibleChanged(object sender, EventArgs e)
        {
            DgvT4.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            DgvT6.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void DgvT4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // 勾選行為
            if (sender != null && e != null &&
                DgvT4.Rows.Count > 0 &&
                DgvT4.CurrentRow != null &&
                DgvT4.CurrentCell.ColumnIndex == DgvT4.Columns[nameof(checkStateDataGridViewCheckBoxColumn)].Index)
            {
                bool selected = (bool)DgvT4.CurrentCell.EditedFormattedValue;

                DgvT4.CurrentCell.Value = !selected;
            }

            var T6MachineTemp = new List<T4MachineModel>();

            if (DgvT4.Rows.Count > 0 && DgvT4.CurrentRow != null)
            {
                // 更新 T6 的顯示資訊
                string T6Runcard = DgvT4.CurrentRow.Cells[nameof(rCNOT4DataGridViewTextBoxColumn)].Value.ToString();

                // 沿用取得 T4 機台的方式，改拿 T6 資訊
                T4Srv.HasT4inRoute(T4RC: T6Runcard, routeDetail: out DataTable routeDetail);

                var T6Process = routeDetail.Rows.Cast<DataRow>()
                    .DefaultIfEmpty(null)
                    .FirstOrDefault(x => x["T6"].ToString() == "Y");

                if (T6Process != null)
                {
                    T6_ProcessID = T6Process["PROCESS_ID"].ToString();

                    var T6RuncardInfo = RcSrv.GetRcNoInfo(Runcard: T6Runcard, processID: T6_ProcessID);

                    T6MachineTemp = T4Srv.GetT4MachineList(T4RC: T6Runcard, T4ProcessID: T6_ProcessID);

                    // 流程卡在 T4 製程產出時的良品數量
                    T6MachineTemp.ForEach(x =>
                    {
                        x.CURRENT_QTY_T4 = int.Parse(T6RuncardInfo["CURRENT_QTY"].ToString());
                    });
                }
            }

            T6MachineTemp = T4Srv.Sort(T6MachineTemp).ToList();

            BindingDataGridView(targetName: nameof(DgvT6), model: T6MachineTemp);
        }

        /// <summary>
        /// 更新 T4 頁籤的已綁定數量等資訊
        /// </summary>
        private void T4Renew()
        {
            // 考慮到多人同時執行產出的情境，執行產出之前必須再取一次已綁定數量。
            // 先確保資料狀態是最新的，再做檢查才會正確。

            // 1. T4List 過濾出不同的 T4 RC / T4 Process
            var T4Temp = new List<T4MachineModel>();

            T4List.ForEach(x =>
            {
                if (T4Temp.Count < 0 ||
                !T4Temp.Any(z => z.RC_NO_T4 == x.RC_NO_T4 && z.PROCESS_ID_T4 == x.PROCESS_ID_T4))
                {
                    T4Temp.Add(x);
                }
            });

            T4Temp.ForEach(x =>
            {
                // 2. 取得已綁定資料
                var T4UsedTemp = T4Srv.GetT4MachineUsedRecords(T4RC: x.RC_NO_T4, T4ProcessID: x.PROCESS_ID_T4.ToString());

                // 3. 綁回主清單
                T4List = T4Srv.Merge(T4Machine: T4List, T4Used: T4UsedTemp, RcInFo: RC_NO_INFO);
            });

            T4List = T4Srv.Sort(T4List).ToList();

            // 4. 更新到畫面上
            BindingDataGridView(targetName: nameof(DgvT4), model: T4List);
        }

        #endregion

        #region 換班功能

        /// <summary>
        /// 換班移出機台
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnShift_Click(object sender, EventArgs e)
        {
            int machine_count = DgvMachine.Rows.Count;

            if (machine_count <= 0)
            {
                return;
            }

            #region 支援多機台換班而移除的業務邏輯
            /*
            if (machine_count > 1)
            {
                string message = SajetCommon.SetLanguage(MessageEnum.MachineMoreThanOneCanNotShift.ToString());

                SajetCommon.Show_Message(message, 3);

                return;
            }

            int.TryParse(RC_NO_INFO["CURRENT_QTY"].ToString(), out int current_qty);

            int total_load_before = SfSrv.GetMachineTotalLoadBefore(RC_NO_INFO);

            var machine = DgvMachine.Rows[0];

            int.TryParse(machine.Cells[nameof(lOADQTYDataGridViewTextBoxColumn)].Value.ToString(), out int load_qty);

            if (load_qty <= 0)
            {
                string message = SajetCommon.SetLanguage(MessageEnum.TheMachineWorkloadMustBeANonNegativeInteger.ToString());

                SajetCommon.Show_Message(message, 3);

                return;
            }

            if (load_qty > current_qty)
            {
                string message
                    = SajetCommon.SetLanguage(MessageEnum.TheMachineloadExceededCurrentQuantity.ToString())
                    + " : "
                    + total_load_before;

                SajetCommon.Show_Message(message, 3);

                return;
            }

            if (load_qty > current_qty - total_load_before)
            {
                string message = SajetCommon.SetLanguage(MessageEnum.TheTotalProcessingQtyHasExceededTheCurrentQtyOfTheRuncard.ToString());

                SajetCommon.Show_Message(message, 3);

                return;
            }

            if (load_qty == current_qty - total_load_before)
            {
                string message = SajetCommon.SetLanguage(MessageEnum.TheTotalProcessingQtyIsEqualToTheCurrentQtyOfTheRuncard.ToString());

                SajetCommon.Show_Message(message, 3);

                return;
            }
            //*/
            #endregion

            using (var f = new fShift
            {
                Runcard = txtInput.Text.Trim(),
                ProcessID = programInfo.dsRC.Tables[0].Rows[0]["PROCESS_ID"].ToString(),
                EmpID = EmpID,
                Machines = WipOutList,
            })
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    var d = McSrv.GetWorkingMachineList(txtInput.Text);

                    WipOutList = McSrv.GetModels(d);

                    // 如果要設定爐序，預設先選擇第一項
                    if (usingT4OrT6stove)
                    {
                        WipOutList.ForEach(x =>
                        {
                            x.STOVE_SEQ = string.IsNullOrWhiteSpace(x.STOVE_SEQ) ? DateGridViewComboBoxItems[0] : x.STOVE_SEQ;
                        });
                    }

                    DgvMachine.DataSource = WipOutList;

                    OtSrv.RearrangeDataGridView(ref DgvMachine);
                }
            }

            ShowShiftQTY();
        }

        /// <summary>
        /// 交接班加入機台
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnHandover_Click(object sender, EventArgs e)
        {
            string message;

            if (DgvMachine.Rows.Count > 0)
            {
                message = SajetCommon.SetLanguage(MessageEnum.CanNotTakeOverWithoutShift.ToString());

                SajetCommon.Show_Message(message, 3);

                return;
            }

            var temp = new List<MachineDownModel>();

            if (!McSrv.GetMachineList(
                RC_NO_INFO,
                out temp,
                out message))
            {
                message = SajetCommon.SetLanguage(MessageEnum.ThisProcessHasNoMachine.ToString());

                SajetCommon.Show_Message(message, 3);

                return;
            }

            using (var f = new fMachineAdd
            {
                Runcard = txtInput.Text.Trim(),
                ProcessID = programInfo.dsRC.Tables[0].Rows[0]["PROCESS_ID"].ToString(),
                EmpID = EmpID,
            })
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    var d = McSrv.GetWorkingMachineList(txtInput.Text);

                    WipOutList = McSrv.GetModels(d);

                    // 如果要設定爐序，預設先選擇第一項
                    if (usingT4OrT6stove)
                    {
                        WipOutList.ForEach(x =>
                        {
                            x.STOVE_SEQ = string.IsNullOrWhiteSpace(x.STOVE_SEQ) ? DateGridViewComboBoxItems[0] : x.STOVE_SEQ;
                        });
                    }

                    #region 扣除前面班次已經加工的數量，將剩餘數量填入這個班次使用的機台的加工數量，當作預設值

                    /*
                    // 要支援多部機台交接班，就沒辦法定預設值
                    int.TryParse(RC_NO_INFO["CURRENT_QTY"].ToString(), out int current_qty);

                    int total_load_before = SfSrv.GetMachineTotalLoadBefore(RC_NO_INFO);

                    WipOutList[0].LOAD_QTY = current_qty - total_load_before;

                    SfSrv.UpdateLoadOfThisShift(RC_NO_INFO, WipOutList[0]);
                    //*/

                    #endregion

                    DgvMachine.DataSource = WipOutList;

                    OtSrv.RearrangeDataGridView(ref DgvMachine);
                }
            }
        }

        private void BtnShiftHistory_Click(object sender, EventArgs e)
        {
            using (var f = new fShiftHistory(RC_INFO: RC_NO_INFO))
            {
                f.ShowDialog();
            }
        }

        private void CbShift_CheckedChanged(object sender, EventArgs e)
        {
            bool check = CbShift.Checked;

            DtpShift.Enabled = check;

            TbWorkload.Enabled = check;
        }

        #endregion

        /// <summary>
        /// 有啟動日曆便視為有設定報工時間，打勾
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DtpOutDate_DropDown(object sender, EventArgs e)
        {
            CbReportTime.CheckStateChanged -= CbReportTime_CheckStateChanged;

            CbReportTime.Checked = true;

            CbReportTime.CheckStateChanged += CbReportTime_CheckStateChanged;
        }

        private void DtpOutDate_ValueChanged(object sender, EventArgs e)
        {
            CbReportTime.CheckStateChanged -= CbReportTime_CheckStateChanged;

            CbReportTime.Checked = true;

            CbReportTime.CheckStateChanged += CbReportTime_CheckStateChanged;
        }

        private void CbReportTime_CheckStateChanged(object sender, EventArgs e)
        {
            DtpOutDate.ValueChanged -= DtpOutDate_ValueChanged;

            if (!CbReportTime.Checked)
            {
                DtpOutDate.Value = OUT_PROCESS_TIME;
            }

            DtpOutDate.ValueChanged += DtpOutDate_ValueChanged;
        }

        private void GvSN_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            // 2015/08/26, Aaron
            if (e.ColumnIndex == -1)
            {
                return;
            }

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
                if (e.ColumnIndex < programInfo.iSNInput[1])
                {
                    return;
                }

                int iCol = e.ColumnIndex - programInfo.iSNInput[0] - 3;

                DataGridViewCell cell = DgvSN.Rows[e.RowIndex].Cells[e.ColumnIndex];

                cell.ErrorText = "";

                //20150602 Nancy Modify
                if (programInfo.dsSNParam.Tables[0].Rows.Count > 0)
                {
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
            }
        }

        private void GvInput_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell cell = DgvInput.Rows[e.RowIndex].Cells[e.ColumnIndex];

            cell.ErrorText = "";

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

            if (string.IsNullOrEmpty(cell.ErrorText) &&
                DgvInput.Rows[e.RowIndex].Cells["NECESSARY"].EditedFormattedValue.ToString() == "Y" &&
                string.IsNullOrEmpty(cell.EditedFormattedValue.ToString()))
            {
                cell.ErrorText = SajetCommon.SetLanguage("Data Empty", 1);
            }
        }

        private void GvCondition_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
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

        private void GvCondition_CellClick(object sender, DataGridViewCellEventArgs e)
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

        private void GvDefect_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (DgvDefect.Rows.Count > 0 && DgvDefect.Columns[e.ColumnIndex].Name == "QTY")
            {
                string q = DgvDefect.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() ?? "0";

                DgvDefect.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = q;

                if (!OtSrv.IsNumeric(4, DgvDefect.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Input Quantity in Positive integer"), 0);

                    DgvDefect.CurrentCell = DgvDefect.Rows[e.RowIndex].Cells[e.ColumnIndex];

                    DgvDefect.CurrentCell.Selected = true;

                    DgvDefect.BeginEdit(true);

                    return;
                }
                else
                {
                    //double iScrap = Convert.ToDouble(programInfo.txtScrap.Text);

                    //double iGood = Convert.ToDouble(programInfo.txtGood.Text);

                    double iInput = Convert.ToDouble(programInfo.dsRC.Tables[0].Rows[0]["QTY"].ToString());

                    double iQty = Convert.ToDouble(DgvDefect.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());

                    if (iQty > iInput) //   || iQty > iScrap    ((iQty + iScrap > iInput) && !btxtGood) || 
                    {
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("Defect Quantity Invalid."), 0);

                        return;
                    }
                    else
                    {
                        //iScrap = 0;

                        iQty = 0;

                        for (int i = 0; i < DgvDefect.Rows.Count; i++)
                        {
                            if (!OtSrv.IsNumeric(4, DgvDefect.Rows[i].Cells[DgvDefect.Columns.Count - 1].Value.ToString()))
                            {
                                return;
                            }

                            iQty += Convert.ToDouble(DgvDefect.Rows[i].Cells[DgvDefect.Columns.Count - 1].Value.ToString());

                            //iScrap = iScrap + iQty;
                        }

                        programInfo.txtScrap.Text = (iQty).ToString();

                        programInfo.txtGood.Text = (iInput - iQty).ToString();

                        if (iQty > iInput) // if (iQty > iInput || iQty > iScrap)
                        {
                            SajetCommon.Show_Message(SajetCommon.SetLanguage("Defect Quantity Invalid."), 0);

                            return;
                        }
                    }

                    btxtGood = false;
                }
            }
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
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Keypart SN Error."), 0);

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
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Keypart SN Error."), 0);

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
        }

        private void TbRework_Click(object sender, EventArgs e)
        {
            TbRework.SelectAll();
        }

        private void TbRework_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            if (e.KeyChar == (char)Keys.Enter)
            {
                CheckReworkQty();
            }
        }

        /// <summary>
        ///  從製程配置資料表搜尋此製程是否有設定RC Input
        /// </summary>
        private void ProcessSheet()
        {
            try
            {
                string sSQL = @"
SELECT
    PROCESS_ID
FROM
    SAJET.SYS_RC_PROCESS_SHEET
WHERE
    PROCESS_ID = :PROCESS_ID
AND SHEET_NAME = 'RC Input'
";
                var Params = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", programInfo.dsRC.Tables[0].Rows[0]["PROCESS_ID"].ToString() }
                };

                DataSet ds = ClientUtils.ExecuteSQL(sSQL, Params.ToArray());

                if (ds == null)
                {
                    bOneSheet = true;
                }
                else
                {
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        bOneSheet = true;
                    }
                    else
                    {
                        bOneSheet = false;
                    }
                }
            }
            catch (Exception)
            {
                bOneSheet = true;
            }
        }

        private void ShowData()
        {
            string sSQL = programInfo.sSQL["SQL"];

            sSQL = @"
SELECT
    A.RC_NO,
    A.WORK_ORDER,
    B.PART_NO,
    ROUTE_NAME,
    PROCESS_NAME,
    FACTORY_CODE,
    A.VERSION,
    A.PROCESS_ID,
    A.PART_ID,
    CURRENT_QTY   QTY,
    CURRENT_QTY   GOOD_QTY,
    0 SCRAP_QTY,
    F.WO_OPTION2,
    B.SPEC1,
    B.SPEC2,
    B.OPTION2     FORMER_NO,
    B.OPTION4     BLUEPRINT
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
    AND A.RC_NO = :SN
    AND A.WORK_ORDER = F.WORK_ORDER
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

                    ProcessSheet();

                    DspReportData();
                }

                programInfo.txtGood.KeyPress += new System.Windows.Forms.KeyPressEventHandler(TxtGood_KeyPress);
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);
            }
        }

        private void DspReportData()
        {
            #region DIsplay Process Parameter

            var Params = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", programInfo.dsRC.Tables[0].Rows[0]["PROCESS_ID"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", programInfo.dsRC.Tables[0].Rows[0]["PART_ID"].ToString() }
            };

            DataSet ds = ClientUtils.ExecuteSQL(programInfo.sSQL["製程條件"], Params.ToArray());

            DgvCondition.DataSource = ds;

            DgvCondition.DataMember = ds.Tables[0].ToString();

            foreach (DataGridViewColumn col in DgvCondition.Columns)
            {
                col.HeaderText = SajetCommon.SetLanguage(col.HeaderText);

                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            DgvCondition.Columns["VALUE_TYPE"].Visible = false;

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

                DgvInput.Columns["VALUE_DEFAULT"].HeaderText = SajetCommon.SetLanguage("Input value");
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
            }

            foreach (DataGridViewColumn col in DgvInput.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            #endregion

            #region Display Machine

            Params = new List<object[]>();

            if (bOneSheet)
            {
                string s = @"
SELECT
    B.MACHINE_ID
   ,TRIM(MACHINE_CODE) MACHINE_CODE
   ,TRIM(MACHINE_DESC) MACHINE_DESC
   ,D.STATUS_NAME
   ,SYSDATE ""START_TIME""
   ,RUN_FLAG
   ,DEFAULT_STATUS
   ,'' DATE_CODE
   ,0 LOAD_QTY
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
    MACHINE_CODE
";

                Params.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", programInfo.dsRC.Tables[0].Rows[0]["PROCESS_ID"].ToString() });

                //ds = ClientUtils.ExecuteSQL(programInfo.sSQL["OUTPUT MACHINE"], Params.ToArray());
                ds = ClientUtils.ExecuteSQL(s, Params.ToArray());
            }
            else
            {
                ds = McSrv.GetWorkingMachineList(txtInput.Text);
            }

            WipInList = McSrv.GetModels(ds);

            WipOutList = WipInList
                .Select(e => new MachineDownModel(e))
                .ToList();

            // 如果要設定爐順序號碼，預設先選擇第一項
            if (usingT4OrT6stove)
            {
                WipOutList.ForEach(x =>
                {
                    x.STOVE_SEQ = string.IsNullOrWhiteSpace(x.STOVE_SEQ) ? DateGridViewComboBoxItems[0] : x.STOVE_SEQ;
                });
            }

            DgvMachine.DataSource = WipOutList;

            #endregion

            #region Display Piece

            Params = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", txtInput.Text }
            };

            ds = ClientUtils.ExecuteSQL(programInfo.sSQL["元件"], Params.ToArray());

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

                Params = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", programInfo.dsRC.Tables[0].Rows[0]["PROCESS_ID"].ToString() },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", programInfo.dsRC.Tables[0].Rows[0]["PART_ID"].ToString() }
                };

                programInfo.dsSNParam = ClientUtils.ExecuteSQL(programInfo.sSQL["元件製程參數"], Params.ToArray());

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

                foreach (DataGridViewColumn col in DgvSN.Columns)
                {
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }

            #endregion

            #region Display Defect Code

            DgvDefect.Columns.Clear();

            if (DgvSN.Rows.Count > 0)
            {
                DgvDefect.Columns.Add("SERIAL_NUMBER", SajetCommon.SetLanguage("SERIAL_NUMBER", 1));
            }

            DgvDefect.Columns.Add("DEFECT_CODE", SajetCommon.SetLanguage("DEFECT_CODE", 1));

            DgvDefect.Columns.Add("DEFECT_DESC", SajetCommon.SetLanguage("DEFECT_DESC", 1));

            DgvDefect.Columns.Add("QTY", SajetCommon.SetLanguage("QTY", 1));

            Params = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", programInfo.dsRC.Tables[0].Rows[0]["PROCESS_ID"].ToString() }
            };

            ds = ClientUtils.ExecuteSQL(programInfo.sSQL["PROCESS DEFECT"], Params.ToArray());

            // 沒有製成不良不用跳提示 1.0.17003.19
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    DgvDefect.Rows.Add();

                    foreach (DataColumn dc in ds.Tables[0].Columns)
                    {
                        DgvDefect.Rows[DgvDefect.Rows.Count - 1].Cells[dc.ColumnName].Value = dr[dc.ColumnName].ToString();

                        DgvDefect.Columns[dc.ColumnName].ReadOnly = true;
                    }

                    DgvDefect.Rows[DgvDefect.Rows.Count - 1].Cells[DgvDefect.Columns.Count - 1].Value = "0";
                }
            }

            foreach (DataGridViewColumn col in DgvDefect.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            #endregion

            # region Keyparts Collection

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

                Params = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", programInfo.dsRC.Tables[0].Rows[0]["PROCESS_ID"].ToString() },
                    //new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", programInfo.dsRC.Tables[0].Rows[0]["PART_ID"].ToString() },
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
            }
            else
            {
                this.TpKeyparts.Parent = null;
            }

            # endregion
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

                string sSQL = @"
SELECT *
FROM
    SAJET.G_RC_STATUS
WHERE
    RC_NO = :RC_NO
";
                var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", txtInput.Text }
                };

                DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, p.ToArray());

                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    rcRoute.sTravel_Id = dsTemp.Tables[0].Rows[0]["TRAVEL_ID"].ToString();

                    rcRoute.sNext_Process = dsTemp.Tables[0].Rows[0]["NEXT_PROCESS"].ToString();

                    rcRoute.sSheet_Name = dsTemp.Tables[0].Rows[0]["SHEET_NAME"].ToString();

                    rcRoute.g_sRouteID = dsTemp.Tables[0].Rows[0]["ROUTE_ID"].ToString();

                    rcRoute.g_sProcessID = dsTemp.Tables[0].Rows[0]["PROCESS_ID"].ToString();

                    rcRoute.sNode_Id = dsTemp.Tables[0].Rows[0]["NODE_ID"].ToString();
                }
                else
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("RC NO Error."), 0);

                    return false;
                }

                //sSQL = string.Format(@"SELECT NODE_ID,NODE_TYPE,NODE_CONTENT,NEXT_NODE_ID,LINK_NAME FROM SAJET.SYS_RC_ROUTE_DETAIL WHERE ROUTE_ID={0} AND XML_INDEX={1}", sRoute_Id, index);
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
                var Params = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "ROUTE_ID", rcRoute.g_sRouteID },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "NODE_CONTENT", rcRoute.g_sProcessID },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "NODE_ID", rcRoute.sNode_Id }
                };

                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params.ToArray());

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

                if (rcRoute.sNode_type == "1" || rcRoute.sNode_type == "2" || rcRoute.sNode_type == "3") // GROUP
                {
                    var f = new FTransferProcess.TransferProcess
                    {
                        sNode_type = rcRoute.sNode_type,
                        sRoute_Id = rcRoute.g_sRouteID,
                        sNode_Id = rcRoute.sNode_Id,
                        sRc_No = txtInput.Text.Trim(),
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
                    sSQL = @"
SELECT
    SHEET_NAME
FROM
    SAJET.SYS_RC_PROCESS_SHEET
WHERE
    PROCESS_ID = :PROCESS_ID
AND SHEET_SEQ = '0'
";
                    p = new List<object[]>
                    {
                        new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", rcRoute.sNext_Process }
                    };

                    dsTemp = ClientUtils.ExecuteSQL(sSQL, p.ToArray());

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
        /// 檢查重工數量
        /// </summary>
        private bool CheckReworkQty()
        {
            if (string.IsNullOrWhiteSpace(TbRework.Text))
            {
                TbRework.Text = "0";
            }

            reworkMaxValue = decimal.Parse(programInfo.txtGood.Text);

            if (decimal.TryParse(TbRework.Text, out decimal input))
            {
                if (input > reworkMaxValue)
                {
                    ReworkMessage("Rework quantity exceed current quantity");

                    return false;
                }
                else if (input < 0)
                {
                    ReworkMessage("Rework quantity does not accept negative values");

                    return false;
                }
            }
            else
            {
                ReworkMessage("Input value is not a number");

                return false;
            }

            return true;
        }

        /// <summary>
        /// 如果製程的配置為必須執行投入與產出，則使用 UPDATE 指令；否則要使用 INSERT 指令。
        /// </summary>
        /// <param name="RC"></param>
        /// <returns></returns>
        private bool CheckProcessSheet(string RC)
        {
            string s = @"
SELECT
    SHEET_SEQ
FROM
    SAJET.SYS_RC_PROCESS_SHEET
WHERE
    PROCESS_ID = 
    (
        SELECT
            PROCESS_ID
        FROM
            SAJET.G_RC_STATUS
        WHERE
            RC_NO = :RC_NO
    )
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.Number, "RC_NO", RC }
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                // 只要執行產出的製程只查得到一筆設定
                return d.Tables[0].Rows.Count == 1;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 檢查 Bonus
        /// </summary>
        /// <param name="sBonus"></param>
        /// <returns></returns>
        private bool CheckBonus(string sBonus)
        {
            try
            {
                double dBonus = Convert.ToDouble(sBonus);

                string sSQL = @"
SELECT
    INITIAL_QTY
FROM
    SAJET.G_RC_STATUS
WHERE
    RC_NO = :RC_NO
AND ROWNUM = 1
";
                var Params = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", txtInput.Text }
                };

                DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params.ToArray());

                if (dsTemp != null)
                {
                    if (dsTemp.Tables[0].Rows.Count > 0)
                    {
                        double dIniQty = Convert.ToDouble(dsTemp.Tables[0].Rows[0]["INITIAL_QTY"].ToString());

                        double dInputQty = Convert.ToDouble(programInfo.dsRC.Tables[0].Rows[0]["QTY"].ToString());

                        double dGoodQty = Convert.ToDouble(programInfo.txtGood.Text);

                        //if (dIniQty > 0.0 && dIniQty < dInputQty + dBonus)
                        if (dIniQty > 0.0 && dIniQty < dGoodQty + dBonus)
                        {
                            return false;
                        }

                        // 定義 : 當Bonus數為負值，倒扣要大於0
                        //if (0 > dInputQty + dBonus)
                        if (0 > dIniQty + dBonus)
                        {
                            return false;
                        }
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
                                }

                                DgvKeypart.Rows[DgvKeypart.Rows.Count - 1].Cells["ITEM_COUNT"].Value = editCount.Text.Trim();
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

                        //if (iQty == 0 && gvBOM.Rows[i].Cells["IS_MATERIAL"].Value.ToString() == "N")
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
        /// 全部 KPSN item_count 在此料號加總
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
                        cnt += Convert.ToDouble(DgvKeypart.Rows[j].Cells["ITEM_COUNT"].Value.ToString());
                    }
                }

                DgvBOM.Rows[i].Cells["QTY"].Value = cnt;
            }
        }

        /// <summary>
        /// 重工數量檢查不合格的處理。
        /// </summary>
        /// <param name="message"></param>
        private void ReworkMessage(string message)
        {
            SajetCommon.Show_Message(SajetCommon.SetLanguage(message), 0);

            TcData.SelectedTab = TpRework;

            TbRework.Focus();

            TbRework.Select(0, TbRework.Text.Length);
        }

        /// <summary>
        /// 顯示輪班累計加工數量
        /// </summary>
        private void ShowShiftQTY()
        {
            DataSet d = SfSrv.GetShiftHistory(RC_NO_INFO);

            int.TryParse(RC_NO_INFO["CURRENT_QTY"].ToString(), out int current_qty);

            int processed_qty = 0;

            foreach (DataRow row in d.Tables[0].Rows)
            {
                int.TryParse(row["Load"].ToString(), out int load_qty);

                processed_qty += load_qty;
            }

            m_tControlData
                .First(x => x.sFieldName == "PROCESSED_QTY")
                .txtControl
                .Text = processed_qty.ToString();

            m_tControlData
                .First(x => x.sFieldName == "REMAINED_QTY")
                .txtControl
                .Text = (current_qty - processed_qty).ToString();
        }

        /// <summary>
        /// 無該筆 SN / RC 時清空資料
        /// </summary>
        private void ClearData()
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

            DgvDefect.Columns.Clear();
        }

        /// <summary>
        /// 將資料來源與 DataGridView 綁定。
        /// </summary>
        /// <param name="model"></param>
        private void BindingDataGridView(string targetName, object model)
        {
            switch (targetName)
            {
                case nameof(DgvT4):
                    {
                        var m = (List<T4MachineModel>)model ?? new List<T4MachineModel>();

                        var bindingList = new BindingList<T4MachineModel>(m);

                        var source = new BindingSource(bindingList, null);

                        DgvT4.DataSource = source;

                        DgvT4.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                        break;
                    }
                case nameof(DgvT6):
                    {
                        var m = (List<T4MachineModel>)model ?? new List<T4MachineModel>();

                        var bindingList = new BindingList<T4MachineModel>(m);

                        var source = new BindingSource(bindingList, null);

                        DgvT6.DataSource = source;

                        DgvT6.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                        break;
                    }
                default:
                    break;
            }
        }
    }
}