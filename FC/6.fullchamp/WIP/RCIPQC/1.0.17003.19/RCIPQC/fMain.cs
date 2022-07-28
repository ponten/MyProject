using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using System.Text.RegularExpressions;//導入命名空間(正則表達式)
using System.Linq;
using System.IO;
using SajetClass;
using RCIPQC.References;
using RCIPQC.Enums;
using OtSrv = RCIPQC.Services.OtherService;
using QcSrv = RCIPQC.Services.QCService;

namespace RCIPQC
{
    public partial class fMain : Form
    {
        TProgramInfo programInfo;

        TControlData[] m_tControlData;

        TRCroute rcRoute = new TRCroute();

        List<MachineDownModel> WipInList = new List<MachineDownModel>();

        List<MachineDownModel> WipOutList = new List<MachineDownModel>();

        List<string> DateGridViewComboBoxItems = new List<string>();

        DataRow RC_NO_INFO;

        //DataRow RC_TRAVEL;

        //DateTime WIP_IN_TIME;

        static string BasePath = Directory.GetCurrentDirectory().Contains("WIP") ?
            Directory.GetCurrentDirectory() : Path.Combine(Directory.GetCurrentDirectory(), "WIP");

        static string fileName = Path.Combine(BasePath, "RCIPQC.ini");
        IniManager iniManager = new IniManager(fileName);

        string IPQC_ProcessID = string.Empty;

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

        DateTime REPORT_TIME; // RCTravel 顯示的時間在 sys_base 的 Closing Date 設定

        string EmpID = string.Empty;

        /// <summary>
        /// 啟動程式的時候傳進來的 FUN_PARAM 參數
        /// </summary>
        string s_function_parameter;

        /// <summary>
        /// 可以抽驗
        /// </summary>
        bool has_qc_sampling = false;

        #region FFilter 的參數

        /// <summary>
        /// 查詢指令
        /// </summary>
        string process_sql = string.Empty;

        /// <summary>
        /// 查詢參數
        /// </summary>
        List<object[]> process_params = new List<object[]>();

        /// <summary>
        /// 隱藏欄位
        /// </summary>
        List<string> h = new List<string>
        {
            "PROCESS_ID",
        };

        #endregion

        public fMain() : this("", "", "", "") { }
        public fMain(string nParam, string strSN, string sEmp, string sTime)
        {
            InitializeComponent();

            m_nParam = nParam;

            this.Text = m_nParam;

            txtInput.Text = strSN;

            tsEmp.Text = sEmp;

            //txtInput.Visible = false;

            //cmbParam.Visible = false;

            statusStrip1.Visible = false;

            s_function_parameter = ClientUtils.fParameter;

            REPORT_TIME = OtSrv.GetDBDateTimeNow();

            #region 載入爐順序號碼的 下拉選單
            DateGridViewComboBoxItems = OtSrv.GetSequencesComboBoxItems();

            sTOVESEQDataGridViewTextBoxColumn.DataSource = DateGridViewComboBoxItems;

            gvMachine.EditingControlShowing += GvMachine_EditingControlShowing;
            #endregion

            #region 不良代碼編輯檢查
            gvDefect.EditingControlShowing += GvDefect_EditingControlShowing;
            #endregion

            #region 品保抽驗項目的檢查
            DgvQCItem.CellEndEdit += DgvQCItem_CellEndEdit;
            #endregion
        }

        private void FMain_Load(object sender, EventArgs e)
        {
            //SajetCommon.SetLanguageControl(this);  

            BtnOK.Enabled = true;

            this.TpKeyparts.Parent = null;

            switch (s_function_parameter)
            {
                case "0": // IPQC
                    process_sql = @"
SELECT
    PROCESS_ID,
    PROCESS_CODE,
    PROCESS_NAME
FROM
    SAJET.SYS_PROCESS
WHERE
    OPERATE_ID = '4'
    AND ENABLED = 'Y'
ORDER BY
    PROCESS_CODE,
    PROCESS_NAME
";

                    // 隱藏 Bonus 頁籤
                    this.TpBonus.Parent = null;
                    this.TpRework.Parent = null;
                    this.TpMachine.Parent = null;
                    this.TpParams.Parent = null;
                    this.TpQCMark.Parent = null;

                    break;
                case "1": // 維修
                    process_sql = @"
SELECT
    PROCESS_ID,
    PROCESS_CODE,
    PROCESS_NAME
FROM
    SAJET.SYS_PROCESS
WHERE
    OPERATE_ID = '3'
    AND ENABLED = 'Y'
ORDER BY
    PROCESS_CODE,
    PROCESS_NAME
";

                    txtBonus.Text = "0";
                    this.TpMachine.Parent = null;
                    this.TpParams.Parent = null;
                    this.TpQCItems.Parent = null;
                    this.TpQCMark.Parent = null;

                    break;

                case "2": // 補登不良
                case "3": // 檢驗員註記

                    process_sql = @"
SELECT DISTINCT
    PROCESS_ID,
    PROCESS_CODE,
    PROCESS_NAME
FROM
    SAJET.SYS_PROCESS
WHERE
    PROCESS_ID IN (
        SELECT
            A.PROCESS_ID
        FROM
            SAJET.SYS_ROLE_OP_PRIVILEGE   A,
            SAJET.SYS_EMP                 B,
            SAJET.SYS_ROLE_EMP            C
        WHERE
            TRIM(A.ROLE_ID) = TRIM(C.ROLE_ID)
            AND TRIM(B.EMP_ID) = TRIM(C.EMP_ID)
            AND TRIM(B.EMP_NO) = TRIM(:EMP_NO)
        UNION
        SELECT
            A.PROCESS_ID
        FROM
            SAJET.SYS_EMP_PROCESS_PRIVILEGE   A,
            SAJET.SYS_EMP                     B
        WHERE
            TRIM(A.EMP_ID) = TRIM(B.EMP_ID)
            AND TRIM(B.EMP_NO) = TRIM(:EMP_NO)
    )
    AND ENABLED = 'Y'
ORDER BY
    PROCESS_CODE,
    PROCESS_NAME
";
                    // 隱藏 Bonus 頁籤
                    this.TpBonus.Parent = null;
                    this.TpRework.Parent = null;
                    this.TpMachine.Parent = null;
                    this.TpParams.Parent = null;
                    this.TpQCItems.Parent = null;
                    this.TpQCMark.Parent = null;

                    if (s_function_parameter == "3")
                    {
                        this.TpQCMark.Parent = tabPanelControl;
                        this.TpDefect.Parent = null;
                        this.TpOutTime.Parent = null;

                        BtnOK.Enabled = false;
                    }
                    break;
            }

            process_params = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "EMP_NO", ClientUtils.fLoginUser },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "EMP_ID", ClientUtils.UserPara1 },
            };

            programInfo.iSNInput = new int[2];

            if (string.IsNullOrEmpty(tsEmp.Text))
            {
                tsEmp.Text = ClientUtils.fLoginUser;

                programInfo.sProgram = ClientUtils.fProgramName;

                //programInfo.sProgram = "WIP";

                programInfo.sFunction = ClientUtils.fFunctionName;

                //programInfo.sFunction = "RC";

                BtnCancel.Visible = false;
            }
            else
            {
                programInfo.sProgram = ClientUtils.fProgramName; // ClientUtils.fProgramName;
                //programInfo.sProgram = "WIP";

                programInfo.sFunction = m_nParam; // ClientUtils.fFunctionName;
            }

            GetEmpID();

            programInfo.sSQL = new Dictionary<string, string>();

            programInfo.iInputField = new Dictionary<string, List<int>>();

            programInfo.sOption = new Dictionary<string, string>();

            programInfo.iInputVisible = new Dictionary<string, int>();

            programInfo.slDefect = new Dictionary<string, int>();

            SajetCommon.SetLanguageControl(this);

            cmbParam.Text += ":";

            if (!string.IsNullOrEmpty(txtInput.Text))
            {
                CheckInput();
            }

            // Depend system type to show tabpage
            if (SystemType())
            {
                // lot control
                this.TpSN.Parent = null;
            }

            RC_Param_Load();

            #region 自訂報工產出時間

            dtpOutDate.Value = REPORT_TIME;

            bool CanSetWipTime = Check_Privilege(EmpNo: tsEmp.Text, fun: "Set_WIP_Time");

            if (s_function_parameter != "3" && CanSetWipTime)
            {
                TpOutTime.Parent = tabPanelControl;
            }
            else
            {
                TpOutTime.Parent = null;
            }

            #endregion

            #region 只有設定有投入 / 產出兩階段的製程站，可以切換機台、設定機台異常代碼

            if (bOneSheet || !Check_Privilege(EmpNo: tsEmp.Text, fun: "Machine_Change"))
            {
                BtnMachineDown.Enabled = false;

                BtnMachineDown.Visible = false;

                gvMachine.Columns[nameof(sTARTTIMEDataGridViewTextBoxColumn)].Visible = false;
            }

            #endregion 
        }

        private void RC_Param_Load()
        {
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
                //new object[] { ParameterDirection.Input, OracleType.VarChar, "PROGRAM", programInfo.sProgram },
                //new object[] { ParameterDirection.Input, OracleType.VarChar, "PARAM_NAME", programInfo.sFunction }
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PROGRAM", programInfo.sProgram },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PARAM_NAME", "IPQC" },
            };

            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params.ToArray());

            int iField = 1, iLabel = 0;

            int iFieldCount = 0, iLabelCount = 0, iRowCount = 0;

            programInfo.iInputField = new Dictionary<string, List<int>>();
            programInfo.iInputVisible = new Dictionary<string, int>();
            programInfo.slDefect = new Dictionary<string, int>();
            programInfo.sOption = new Dictionary<string, string>();
            programInfo.sSQL = new Dictionary<string, string>();
            m_tControlData = null;
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.RowCount = 1;

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
                                    = ("RC_NO,WORK_ORDER,PART_NO,VERSION,SPEC1,FORMER_NO,GOOD_QTY,REMARK")
                                    .Split(',');
                            }
                            else if (iCount == 1)
                            {
                                slValue
                                    = ("QTY,ROUTE_NAME,FACTORY_CODE,PROCESS_NAME,SPEC2,BLUEPRINT,SCRAP_QTY")
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
                                tableLayoutPanel1.RowCount = iRowCount;

                                tableLayoutPanel2.Height = 25 * iRowCount + 10;
                                tableLayoutPanel1.Height = 25 * iRowCount + 10;

                                tableLayoutPanel1.RowStyles[0].Height = 25;

                                for (int i = itableCount; i < slValue.Length; i++)
                                {
                                    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
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
                                    = ("流程卡,工單,料號,版本,品名,舊編,良品數,備註")
                                    .Split(',');
                            }
                            else if (iCount == 1)
                            {
                                slValue
                                    = ("流程卡數量,途程名稱,廠別代碼,製程名稱,規格,藍圖,不良品數")
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

                    default:
                        break;
                }
            }

            #region 強調顯示 舊編(OPTION2) 藍圖(OPTION4)

            var emphasis_font = new Font("新細明體", 12, FontStyle.Regular);

            // 舊編
            TControlData former_no_control_set =
                m_tControlData
                .First(x => x.sFieldName == "FORMER_NO");

            former_no_control_set.txtControl.BackColor = Color.Khaki;
            former_no_control_set.lablControl.ForeColor = Color.Red;

            former_no_control_set.txtControl.Font = emphasis_font;
            former_no_control_set.lablControl.Font = emphasis_font;

            // 藍圖
            TControlData blueprint_control_set =
                m_tControlData
                .First(x => x.sFieldName == "BLUEPRINT");

            blueprint_control_set.txtControl.BackColor = Color.Khaki;
            blueprint_control_set.lablControl.ForeColor = Color.Red;

            blueprint_control_set.txtControl.Font = emphasis_font;
            blueprint_control_set.lablControl.Font = emphasis_font;

            #endregion
        }

        private void FMain_Shown(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(m_nParam))
            {
                txtInput.Focus();
            }

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

            if (set != null && set.Tables[0].Rows.Count > 0)
            {
                (Controls.Find("txtREMARK", true)[0] as TextBox).Text = set.Tables[0].Rows[0]["REMARK"].ToString();
            }
            #endregion

            #region 顯示〈製程設定〉視窗

            string BasePath = Directory.GetCurrentDirectory().Contains("WIP") ?
            Directory.GetCurrentDirectory() : Path.Combine(Directory.GetCurrentDirectory(), "WIP");

            string fileName = Path.Combine(BasePath, "RCIPQC.ini");
            IniManager iniManager = new IniManager(fileName);

            string lblProcessName = string.Empty;

            // 讀取 ini 檔
            switch (s_function_parameter)
            {
                case "0":
                    IPQC_ProcessID = iniManager.ReadIniFile("IPQC", "PROCESS_ID", "");

                    break;
                case "1":
                    IPQC_ProcessID = iniManager.ReadIniFile("Repair", "PROCESS_ID", "");

                    break;
                case "2":
                    IPQC_ProcessID = iniManager.ReadIniFile("ProcessInputAAR", "PROCESS_ID", "");

                    break;
                case "3":
                    IPQC_ProcessID = iniManager.ReadIniFile("INSPECTOR_MARK", "PROCESS_ID", "");

                    break;
            }

            if (string.IsNullOrEmpty(IPQC_ProcessID))
            {
                using (var f = new SajetFilter.FFilter(sqlCommand: process_sql,
                                                       @params: process_params,
                                                       hiddenColumns: h))
                {
                    f.Text = SajetCommon.SetLanguage("Process Set");

                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        DataRow result = f.ResultSets[0];

                        string ProcessCode = result["PROCESS_CODE"].ToString();
                        string ProcessName = result["PROCESS_NAME"].ToString();
                        IPQC_ProcessID = result["PROCESS_ID"].ToString();

                        // 寫入 ini 檔
                        switch (s_function_parameter)
                        {
                            case "0":

                                iniManager.WriteIniFile("IPQC", "PROCESS_ID", IPQC_ProcessID);

                                break;
                            case "1":

                                iniManager.WriteIniFile("Repair", "PROCESS_ID", IPQC_ProcessID);

                                break;

                            case "2":

                                iniManager.WriteIniFile("ProcessInputAAR", "PROCESS_ID", IPQC_ProcessID);

                                break;

                            case "3":

                                iniManager.WriteIniFile("INSPECTOR_MARK", "PROCESS_ID", IPQC_ProcessID);

                                break;
                        }

                        lblProcess.Text = SajetCommon.SetLanguage("Process") + ":" + ProcessName;

                        if (!string.IsNullOrEmpty(lblProcess.Text))
                        {
                            btnClear.Enabled = true;
                            btnSearch.Enabled = true;
                            txtInput.Enabled = true;
                            if (BtnCancel.Visible)
                            {
                                BtnCancel.Enabled = true;
                            }

                            if (s_function_parameter != "3")
                            {
                                tsMemo.Enabled = true;
                            }

                            if (this.TpParams.Parent != null)
                            {
                                tabPanelControl.SelectedTab = this.TpParams;
                            }
                            else if (this.TpMachine.Parent != null)
                            {
                                tabPanelControl.SelectedTab = this.TpMachine;
                            }
                            else if (this.TpDefect.Parent != null)
                            {
                                tabPanelControl.SelectedTab = this.TpDefect;
                            }
                        }
                    }
                }
            }
            else
            {
                string sSQL = "SELECT PROCESS_NAME " +
                           " FROM SAJET.SYS_PROCESS " +
                          " WHERE PROCESS_ID = :PROCESSID" +
                          " AND ENABLED = 'Y' ";

                var Params = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESSID", IPQC_ProcessID }
                };

                var ds = ClientUtils.ExecuteSQL(sSQL, Params.ToArray());
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow _row = ds.Tables[0].Rows[0];
                    lblProcessName = _row["PROCESS_NAME"].ToString();
                }
                else
                {
                    using (var f = new SajetFilter.FFilter(sqlCommand: process_sql,
                                                           @params: process_params,
                                                           hiddenColumns: h))
                    {
                        f.Text = SajetCommon.SetLanguage("Process Set");

                        if (f.ShowDialog() == DialogResult.OK)
                        {
                            DataRow result = f.ResultSets[0];

                            string ProcessCode = result["PROCESS_CODE"].ToString();
                            string ProcessName = result["PROCESS_NAME"].ToString();
                            IPQC_ProcessID = result["PROCESS_ID"].ToString();

                            // 寫入 ini 檔
                            switch (s_function_parameter)
                            {
                                case "0":

                                    iniManager.WriteIniFile("IPQC", "PROCESS_ID", IPQC_ProcessID);

                                    break;
                                case "1":

                                    iniManager.WriteIniFile("Repair", "PROCESS_ID", IPQC_ProcessID);

                                    break;

                                case "2":

                                    iniManager.WriteIniFile("ProcessInputAAR", "PROCESS_ID", IPQC_ProcessID);

                                    break;

                                case "3":

                                    iniManager.WriteIniFile("INSPECTOR_MARK", "PROCESS_ID", IPQC_ProcessID);

                                    break;
                            }

                            lblProcessName = ProcessName;
                        }
                    }
                }

                btnClear.Enabled = true;
                btnSearch.Enabled = true;
                txtInput.Enabled = true;
                if (BtnCancel.Visible)
                {
                    BtnCancel.Enabled = true;
                }

                if (s_function_parameter != "3")
                {
                    tsMemo.Enabled = true;
                }

                lblProcess.Text = SajetCommon.SetLanguage("Process") + ":" + lblProcessName;

                if (this.TpParams.Parent != null)
                {
                    tabPanelControl.SelectedTab = this.TpParams;
                }
                else if (this.TpMachine.Parent != null)
                {
                    tabPanelControl.SelectedTab = this.TpMachine;
                }
                else if (this.TpDefect.Parent != null)
                {
                    tabPanelControl.SelectedTab = this.TpDefect;
                }
            }

            #endregion

            TControlData process_name_control_set =
                m_tControlData
                .First(x => x.sFieldName == "PROCESS_NAME");

            process_name_control_set.txtControl.BackColor = Color.Khaki;
            process_name_control_set.lablControl.ForeColor = Color.Red;

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

            #region 頁籤項目位置

            int i_position_1 = 0;
            int i_position_2 = 0;
            int i_default_distance = 10;

            // 報工時間頁籤
            {
                i_position_1 = label4.Right;

                i_position_2 = dtpOutDate.Left;

                if (i_position_1 > i_position_2)
                {
                    dtpOutDate.Left = i_position_1 + i_default_distance;
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

                if (lblWorkHour.Right > i_position_1)
                {
                    i_position_1 = lblWorkHour.Right;
                }

                i_position_2 = txtBonus.Left;

                if (txtWorkHour.Left > i_position_2)
                {
                    i_position_2 = txtWorkHour.Left;
                }

                if (i_position_1 > i_position_2)
                {
                    txtBonus.Left = i_position_1 + i_default_distance;

                    txtWorkHour.Left = i_position_1 + i_default_distance;
                }
            }

            // 品檢標器
            {
                i_position_1 = label2.Right;

                if (lblWorkHour.Right > i_position_1)
                {
                    i_position_1 = lblWorkHour.Right;
                }

                i_position_2 = txtBonus.Left;

                if (txtWorkHour.Left > i_position_2)
                {
                    i_position_2 = txtWorkHour.Left;
                }

                if (i_position_1 > i_position_2)
                {
                    txtBonus.Left = i_position_1 + i_default_distance;

                    txtWorkHour.Left = i_position_1 + i_default_distance;
                }
            }

            #endregion
        }

        private void TxtInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
            {
                return;
            }

            ClearData();
            CheckInput();
            if (s_function_parameter != "3")
            {
                BtnOK.Enabled = true;
                tsMemo.Enabled = true;
            }
            tabPanelControl.Enabled = true;
            txtInput.SelectAll();
        }

        //txtBonus
        private void txtBonus_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
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

            if (!IsNumeric(4, programInfo.txtGood.Text))
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

                    if (gvDefect.Rows.Count > 0)
                    {
                        gvDefect.Rows[0].Cells["QTY"].Value = programInfo.txtScrap.Text;
                    }
                }
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            string sInput = string.Empty;

            string message = string.Empty;

            List<object[]> Params;

            // 流程卡資訊
            RC_NO_INFO = GetRcNoInfo(txtInput.Text);
            if (RC_NO_INFO.ItemArray.Length == 0)
            {
                return;
            }

            #region 檢查良品數

            double dInput, dGood, dScrap;

            if (!IsNumeric(4, programInfo.txtGood.Text))
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

                    for (int i = 0; i < gvDefect.Rows.Count; i++)
                    {
                        if (!IsNumeric(4, gvDefect.Rows[i].Cells[gvDefect.Columns.Count - 1].Value.ToString()))
                        {
                            return;
                        }

                        iQty += Convert.ToDouble(gvDefect.Rows[i].Cells[gvDefect.Columns.Count - 1].Value.ToString());

                        //iScrap = iScrap + iQty;
                    }

                    if (iQty != Convert.ToDouble(programInfo.txtScrap.Text) && gvDefect.Rows.Count > 0)
                    {
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("Defect Quantity Invalid."), 0);

                        return;
                    }
                }
            }

            #endregion

            # region Process Machine

            string sMachine = string.Empty;

            var machines = new List<MachineDownModel>();

            if (this.TpMachine.Parent != null)
            {
                if (gvMachine.Rows.Count > 0) // 檢查是否有勾選
                {
                    // 畫面上有的機台產出時都要記錄
                    for (int i = 0; i < gvMachine.Rows.Count; i++)
                    {
                        sMachine += gvMachine.Rows[i].Cells[nameof(mACHINECODEDataGridViewTextBoxColumn)].EditedFormattedValue.ToString() + (char)9;

                        //machines.Add(gvMachine.Rows[i].Cells[nameof(mACHINEIDDataGridViewTextBoxColumn)].EditedFormattedValue.ToString());
                        machines.Add(new MachineDownModel
                        {
                            MACHINE_ID = int.Parse(gvMachine.Rows[i].Cells[nameof(mACHINEIDDataGridViewTextBoxColumn)].EditedFormattedValue.ToString()),
                            LOAD_QTY = int.Parse(gvMachine.Rows[i].Cells[nameof(lOADQTYDataGridViewTextBoxColumn)].EditedFormattedValue.ToString()),
                            DATE_CODE = gvMachine.Rows[i].Cells[nameof(dATECODEDataGridViewTextBoxColumn)].EditedFormattedValue.ToString(),
                            STOVE_SEQ = gvMachine.Rows[i].Cells[nameof(sTOVESEQDataGridViewTextBoxColumn)].EditedFormattedValue.ToString(),
                        });
                    }
                }
                else if (WipInList.Count > 0)
                {
                    // 製程有綁定機台，則投入的時候一定有選機台
                    // 如果產出的時候，沒有任何正在加工的機台，則不能產出
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Please set working machine"), 0);

                    return;
                }
            }

            #endregion

            #region Keyparts Collection

            string skeypart = string.Empty;

            if (bOneSheet)
            {
                if (!CheckKPSNInput())
                {
                    //tabControl1.SelectedIndex = 6;

                    tabPanelControl.SelectedTab = TpKeyparts;

                    editKPSN.Focus();

                    return;
                }

                foreach (DataGridViewRow dr in gvKeypart.Rows)
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
            for (int i = 0; i < gvDefect.Rows.Count; i++)
            {
                if (!IsNumeric(4, gvDefect.Rows[i].Cells["QTY"].Value.ToString()))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Input Quantity in Positive integer"), 0);

                    return;
                }
            }

            #endregion

            #region 檢查資料收集是否有輸入

            if (this.TpParams.Parent != null)
            {
                foreach (DataGridViewRow dr in gvInput.Rows)
                {
                    if (!string.IsNullOrEmpty(dr.Cells["VALUE_DEFAULT"].ErrorText))
                    {
                        SajetCommon.Show_Message(dr.Cells["ITEM_NAME"].EditedFormattedValue.ToString() + dr.Cells["VALUE_DEFAULT"].ErrorText, 0);

                        gvInput.CurrentCell = dr.Cells["VALUE_DEFAULT"];

                        gvInput.Focus();

                        tabPanelControl.SelectedIndex = 0;

                        return;
                    }

                    if (!string.IsNullOrEmpty(dr.Cells["VALUE_DEFAULT"].EditedFormattedValue.ToString()))
                    {
                        sInput += dr.Cells["ITEM_ID"].EditedFormattedValue.ToString() + (char)9
                            + dr.Cells["VALUE_DEFAULT"].EditedFormattedValue.ToString() + (char)27;
                    }
                }

            }

            #endregion

            #region 檢查元件是否有輸入良品數, 不良品數

            string sSN = string.Empty;

            foreach (DataGridViewRow dr in gvSN.Rows)
            {
                if (dr.Cells["CURRENT_STATUS"].EditedFormattedValue.ToString() == "NG")
                {
                    if (programInfo.slDefect[dr.Cells["SERIAL_NUMBER"].EditedFormattedValue.ToString()] == 0)
                    {
                        message
                            = SajetCommon.SetLanguage("Defect Qty Empty")
                            + Environment.NewLine
                            + gvSN.Columns[0].HeaderText
                            + ": "
                            + dr.Cells["SERIAL_NUMBER"].EditedFormattedValue.ToString();

                        SajetCommon.Show_Message(message, 0);

                        gvSN.CurrentCell = dr.Cells[0];

                        gvSN.Focus();

                        return;
                    }
                }

                // 2015/08/26, Aaron
                if (!string.IsNullOrEmpty(dr.Cells["GOOD_QTY"].ErrorText))
                {
                    SajetCommon.Show_Message(gvSN.Columns["GOOD_QTY"].HeaderText + dr.Cells["GOOD_QTY"].ErrorText, 0);

                    gvSN.CurrentCell = dr.Cells["GOOD_QTY"];

                    gvSN.Focus();

                    tabPanelControl.SelectedTab = TpSN;

                    return;
                }

                // 2015/08/26, Aaron
                if (!string.IsNullOrEmpty(dr.Cells["SCRAP_QTY"].ErrorText))
                {
                    SajetCommon.Show_Message(gvSN.Columns["SCRAP_QTY"].HeaderText + dr.Cells["SCRAP_QTY"].ErrorText, 0);

                    gvSN.CurrentCell = dr.Cells["SCRAP_QTY"];

                    gvSN.Focus();

                    tabPanelControl.SelectedTab = TpSN;

                    return;
                }

                string sSNTemp
                    = dr.Cells["SERIAL_NUMBER"].EditedFormattedValue.ToString() + (char)9
                    + dr.Cells["CURRENT_STATUS"].EditedFormattedValue.ToString() + (char)9
                    + dr.Cells["GOOD_QTY"].EditedFormattedValue.ToString() + (char)9
                    + dr.Cells["SCRAP_QTY"].EditedFormattedValue.ToString();

                if (programInfo.iSNInput[1] > -1)
                {
                    for (int i = programInfo.iSNInput[1]; i < gvSN.Columns.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(dr.Cells[i].ErrorText))
                        {
                            SajetCommon.Show_Message(gvSN.Columns[i].HeaderText + dr.Cells[i].ErrorText, 0);

                            gvSN.CurrentCell = dr.Cells[i];

                            gvSN.Focus();

                            tabPanelControl.SelectedTab = TpSN;

                            return;
                        }

                        if (!string.IsNullOrEmpty(dr.Cells[0].EditedFormattedValue.ToString()))
                        {
                            sSN += sSNTemp + (char)9
                                + gvSN.Columns[i].Tag.ToString() + (char)9
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

            foreach (DataGridViewRow dr in gvDefect.Rows)
            {
                if (gvDefect.Columns.Contains("SERIAL_NUMBER"))
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
            string test = string.Empty;

            if (this.TpBonus.Parent != null)
            {
                if (!string.IsNullOrEmpty(txtBonus.Text))
                {
                    try
                    {

                        int _bonus = Convert.ToInt32(txtBonus.Text);

                        if (_bonus < 0)
                        {
                            throw new Exception();
                        }

                        if (!CheckBonus(txtBonus.Text))
                        {
                            SajetCommon.Show_Message(SajetCommon.SetLanguage("Bouns Quantity Invalid."), 0);

                            tabPanelControl.SelectedTab = TpBonus;

                            txtBonus.SelectAll();

                            txtBonus.Focus();

                            return;
                        }

                    }
                    catch (Exception ex)
                    {
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("Bouns Quantity Error."), 0);

                        tabPanelControl.SelectedTab = TpBonus;

                        txtBonus.SelectAll();

                        txtBonus.Focus();

                        return;
                    }
                }
                else
                {
                    txtBonus.Text = "0";
                }
            }

            #endregion

            #region 如果 Work Hour 欄位有輸入檢查數值是否正確

            if (!string.IsNullOrEmpty(txtWorkHour.Text))
            {
                if (!IsNumeric(1, txtWorkHour.Text))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Work Hour Error."), 0);

                    tabPanelControl.SelectedTab = TpBonus;

                    txtWorkHour.SelectAll();

                    txtWorkHour.Focus();

                    return;
                }

                if (txtWorkHour.Text.Length > 5)
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Please enter up to five digits."), 0);

                    tabPanelControl.SelectedTab = TpBonus;

                    txtWorkHour.SelectAll();

                    txtWorkHour.Focus();

                    return;
                }
            }
            else
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Input Work Hour."), 0);

                tabPanelControl.SelectedTab = TpBonus;

                txtWorkHour.SelectAll();

                txtWorkHour.Focus();

                return;
            }

            #endregion

            #region 檢查品保抽驗數據

            var value_list = new List<KeyValuePair<string, string>>();

            if (has_qc_sampling && CbQCItem.Checked)
            {
                foreach (DataGridViewRow dr in DgvQCItem.Rows)
                {
                    if (!string.IsNullOrEmpty(dr.Cells["VALUE_DEFAULT"].ErrorText))
                    {
                        SajetCommon.Show_Message(dr.Cells["ITEM_NAME"].EditedFormattedValue.ToString() + dr.Cells["VALUE_DEFAULT"].ErrorText, 0);

                        DgvQCItem.CurrentCell = dr.Cells["VALUE_DEFAULT"];

                        DgvQCItem.Focus();

                        return;
                    }

                    if (!string.IsNullOrWhiteSpace(dr.Cells["VALUE_DEFAULT"].EditedFormattedValue.ToString()))
                    {
                        var value = new KeyValuePair<string, string>
                        (
                            key: dr.Cells["ITEM_ID"].EditedFormattedValue.ToString(),
                            value: dr.Cells["VALUE_DEFAULT"].EditedFormattedValue.ToString()
                        );

                        value_list.Add(value);
                    }
                }
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

            if (TpOutTime.Parent != null &&
                dtpOutDate.Checked)
            {
                REPORT_TIME = dtpOutDate.Value;
            }
            else
            {
                REPORT_TIME = dtpOutDate.Value = OtSrv.GetDBDateTimeNow();
            }

            if (!OtSrv.IsOutputTimeValid(runcard: RC_NO_INFO, OutProcessTime: REPORT_TIME, message: out message))
            {
                SajetCommon.Show_Message(message, 0);

                return;
            }

            #endregion

            // 選擇下一製程
            if (!SetNextProcess())
            {
                return;
            }

            //if (!DateTime.TryParse(RC_NO_INFO["WIP_IN_TIME"].ToString(), out WIP_IN_TIME))
            //{
            //    WIP_IN_TIME = DateTime.Parse(RC_NO_INFO["UPDATE_TIME"].ToString());
            //}

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
                //new object[] { ParameterDirection.Input, OracleType.VarChar, "TNEXTPROCESS", rcRoute.sNext_Process },
                //new object[] { ParameterDirection.Input, OracleType.VarChar, "TNEXTNODE", rcRoute.sNext_Node },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TNEXTPROCESS", IPQC_ProcessID },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TNEXTNODE", ""},
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TNEXTSHEET", rcRoute.sSheet_Name },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TBONUS", txtBonus.Text },
                new object[] { ParameterDirection.Input, OracleType.DateTime, "TNOW", REPORT_TIME },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TSTATUS", sTSTATUS },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TMACHINE", sMachine },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TKEYPART", skeypart },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TWORKHOUR", txtWorkHour.Text.Trim() },
                new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" }
            };

            //DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_RC_OUTPUT", Params.ToArray());

            DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_RC_IPQC", Params.ToArray());

            string sMsg = ds.Tables[0].Rows[0]["TRES"].ToString();

            if (sMsg == "OK")
            {
                int _bonus = Convert.ToInt32(txtBonus.Text);
                int iRework = Convert.ToInt32(TbRework.Text);
                // 有設定重工數量才需要記錄
                if (iRework > 0 || _bonus > 0)
                {
                    SaveReworkQty(txtInput.Text, IPQC_ProcessID, "", rcRoute.sTravel_Id, iRework, _bonus);
                }

                //RecordMachine(RcInfo: RC_NO_INFO, Machines: machines);

                if (has_qc_sampling && CbQCItem.Checked)
                {
                    QcSrv.SaveData(ClientUtils.UserPara1, value_list);
                }

                txtInput.Text = "";
                ClearData();

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
                if (!IsNumeric(3, editCount.Text))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Input Quantity in Positive number"), 0);

                    editCount.SelectAll();

                    editCount.Focus();

                    return;
                }
            }

            // 確認是否已點選料號
            if (string.IsNullOrEmpty(gvBOM.CurrentRow.Cells["PART_NO"].Value.ToString()))
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
                int iSelect = gvKeypart.SelectedRows.Count;

                for (int i = 0; i < iSelect; i++)
                {
                    gvKeypart.Rows.Remove(gvKeypart.SelectedRows[0]);
                }
            }

            CountKPSN();
        }

        private void BtnMachineDown_Click(object sender, EventArgs e)
        {
            try
            {
                using (var f = new fChangeMachine
                {
                    ProcessID = programInfo.dsRC.Tables[0].Rows[0]["PROCESS_ID"].ToString(),
                    Runcard = txtInput.Text,
                    EmpID = EmpID,
                    usingT4OrT6stove = false,
                })
                {
                    f.ShowDialog();

                    var d = OtSrv.GetMachineList(txtInput.Text);

                    WipOutList = OtSrv.GetModels(d);

                    gvMachine.DataSource = WipOutList;

                    gvMachine.Update();

                    gvMachine.Refresh();
                }

                REPORT_TIME = dtpOutDate.Value = OtSrv.GetDBDateTimeNow();
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);
            }
        }

        /// <summary>
        /// 只能輸入正整數
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridViewTextBoxColumn_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void GvDefect_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= DataGridViewTextBoxColumn_KeyPress;

            if (gvDefect.CurrentCell.ColumnIndex == gvDefect.Columns["QTY"].Index)
            {
                if (e.Control is TextBox x)
                {
                    x.KeyPress += DataGridViewTextBoxColumn_KeyPress;
                }
            }
        }

        #region  在編輯 LOAD_QTY 欄位或是 DATE_CODE 欄位的內容時，為了能檢查輸入值內容綁定的事件

        private void GvMachine_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= DgvMachineTextBoxColumn_KeyPress;

            e.Control.Leave -= DgvMachineTextBoxColumn_Leave;

            if (gvMachine.CurrentCell.ColumnIndex == dATECODEDataGridViewTextBoxColumn.Index ||
                gvMachine.CurrentCell.ColumnIndex == lOADQTYDataGridViewTextBoxColumn.Index)
            {
                if (e.Control is TextBox x)
                {
                    x.KeyPress += DgvMachineTextBoxColumn_KeyPress;

                    x.Leave += DgvMachineTextBoxColumn_Leave;
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
            if (gvMachine.CurrentCell.ColumnIndex == lOADQTYDataGridViewTextBoxColumn.Index &&
                sender is DataGridViewTextBoxEditingControl x &&
                string.IsNullOrWhiteSpace(x.Text))
            {
                x.Text = "0";
            }
        }

        #endregion

        private void DgvQCItem_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = DgvQCItem.Rows[e.RowIndex];

            DataGridViewCell cell = row.Cells[e.ColumnIndex];

            cell.ErrorText = "";

            switch (row.Cells["CONVERT_TYPE"].EditedFormattedValue.ToString())
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

            switch (row.Cells["INPUT_TYPE"].EditedFormattedValue.ToString())
            {
                case "R":
                    {
                        string[] slValue = row.Cells["VALUE_LIST"].EditedFormattedValue.ToString().Split(',');

                        decimal dMin = decimal.Parse(slValue[0]);

                        decimal dMax = decimal.Parse(slValue[1]);

                        try
                        {
                            decimal dValue = decimal.Parse(cell.EditedFormattedValue.ToString());

                            if (dValue >= dMin && dValue <= dMax) { }
                            else
                            {
                                cell.ErrorText = string.Format(SajetCommon.SetLanguage("Over flow{0}~{1}"), dMin, dMax);
                            }
                        }
                        catch
                        {
                            if (row.Cells["NECESSARY"].EditedFormattedValue.ToString() == "Y")
                            {
                                cell.ErrorText = SajetCommon.SetLanguage("Data Invalid");
                            }
                        }

                        break;
                    }
                default:
                    {
                        if (row.Cells["VALUE_TYPE"].EditedFormattedValue.ToString() == "N")
                        {
                            try
                            {
                                decimal dValue = decimal.Parse(cell.EditedFormattedValue.ToString());
                            }
                            catch
                            {
                                if (row.Cells["NECESSARY"].EditedFormattedValue.ToString() == "Y")
                                {
                                    cell.ErrorText = SajetCommon.SetLanguage("Data Invalid");
                                }
                            }
                        }

                        break;
                    }
            }

            if (string.IsNullOrEmpty(cell.ErrorText) &&
                row.Cells["NECESSARY"].EditedFormattedValue.ToString() == "Y" &&
                string.IsNullOrWhiteSpace(cell.EditedFormattedValue.ToString()))
            {
                cell.ErrorText = SajetCommon.SetLanguage("Data Empty");
            }

            // 只要有任何一個抽驗項目收集到合格的資料，就判斷這次操作要收集品保項目
            // 按下確認按鈕時才會檢查
            if (string.IsNullOrWhiteSpace(cell.ErrorText))
            {
                CbQCItem.Checked = true;
            }
        }

        private void GvSN_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            // 2015/08/26, Aaron
            if (e.ColumnIndex == -1)
            {
                return;
            }

            if (gvSN.Columns[e.ColumnIndex].Name == "CURRENT_STATUS")
            {
                decimal iGood = 0, iScrap = 0;

                foreach (DataGridViewRow dr in gvSN.Rows)
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
            else if (gvSN.Columns[e.ColumnIndex].Name == "GOOD_QTY")
            {
                DataGridViewCell cell = gvSN.Rows[e.RowIndex].Cells[e.ColumnIndex];

                cell.ErrorText = "";

                try
                {
                    decimal dValue = decimal.Parse(cell.EditedFormattedValue.ToString());
                }
                catch
                {
                    cell.ErrorText = SajetCommon.SetLanguage("Data Invalid");
                }
            }
            else if (gvSN.Columns[e.ColumnIndex].Name == "SCRAP_QTY")
            {
                DataGridViewCell cell = gvSN.Rows[e.RowIndex].Cells[e.ColumnIndex];

                cell.ErrorText = "";

                try
                {
                    decimal dValue = decimal.Parse(cell.EditedFormattedValue.ToString());
                }
                catch
                {
                    cell.ErrorText = SajetCommon.SetLanguage("Data Invalid");
                }
            }
            else
            {
                if (e.ColumnIndex < programInfo.iSNInput[1])
                {
                    return;
                }

                int iCol = e.ColumnIndex - programInfo.iSNInput[0] - 3;

                DataGridViewCell cell = gvSN.Rows[e.RowIndex].Cells[e.ColumnIndex];

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
                                        cell.ErrorText = string.Format(SajetCommon.SetLanguage("Over flow{0}~{1}"), dMin, dMax);
                                    }
                                }
                                catch
                                {
                                    cell.ErrorText = SajetCommon.SetLanguage("Data Invalid");
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
                                        cell.ErrorText = SajetCommon.SetLanguage("Data Invalid");
                                    }
                                }

                                break;
                            }
                    }

                    if (string.IsNullOrEmpty(cell.ErrorText) &&
                        programInfo.dsSNParam.Tables[0].Rows[iCol]["NECESSARY"].ToString() == "Y" &&
                        string.IsNullOrEmpty(cell.EditedFormattedValue.ToString()))
                    {
                        cell.ErrorText = SajetCommon.SetLanguage("Data Empty");
                    }
                }
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
                                cell.ErrorText = string.Format(SajetCommon.SetLanguage("Over flow{0}~{1}"), dMin, dMax);
                            }
                        }
                        catch
                        {
                            if (gvInput.Rows[e.RowIndex].Cells["NECESSARY"].EditedFormattedValue.ToString() == "Y")
                            {
                                cell.ErrorText = SajetCommon.SetLanguage("Data Invalid");
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
                                    cell.ErrorText = SajetCommon.SetLanguage("Data Invalid");
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
                cell.ErrorText = SajetCommon.SetLanguage("Data Empty");
            }
        }

        private void GvCondition_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow r in gvCondition.Rows)
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

            if (gvCondition.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewLinkCell)
            {
                string url = gvCondition.Rows[e.RowIndex].Cells["VALUE_DEFAULT"].Value.ToString();

                System.Diagnostics.Process.Start(url);
            }
        }

        private void GvDefect_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (gvDefect.Rows.Count > 0 && gvDefect.Columns[e.ColumnIndex].Name == "QTY")
            {
                string q = gvDefect.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() ?? "0";

                gvDefect.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = q;

                if (!IsNumeric(4, gvDefect.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Input Quantity in Positive integer"), 0);

                    gvDefect.CurrentCell = gvDefect.Rows[e.RowIndex].Cells[e.ColumnIndex];

                    gvDefect.CurrentCell.Selected = true;

                    gvDefect.BeginEdit(true);

                    return;
                }
                else
                {
                    //double iScrap = Convert.ToDouble(programInfo.txtScrap.Text);

                    //double iGood = Convert.ToDouble(programInfo.txtGood.Text);

                    double iInput = Convert.ToDouble(programInfo.dsRC.Tables[0].Rows[0]["QTY"].ToString());

                    double iQty = Convert.ToDouble(gvDefect.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());

                    if (iQty > iInput) //   || iQty > iScrap    ((iQty + iScrap > iInput) && !btxtGood) || 
                    {
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("Defect Quantity Invalid."), 0);

                        return;
                    }
                    else
                    {
                        //iScrap = 0;

                        iQty = 0;

                        for (int i = 0; i < gvDefect.Rows.Count; i++)
                        {
                            if (!IsNumeric(4, gvDefect.Rows[i].Cells[gvDefect.Columns.Count - 1].Value.ToString()))
                            {
                                return;
                            }

                            iQty += Convert.ToDouble(gvDefect.Rows[i].Cells[gvDefect.Columns.Count - 1].Value.ToString());

                            //iScrap = iScrap + iQty;
                        }

                        programInfo.txtScrap.Text = (iQty).ToString();

                        programInfo.txtGood.Text = (iInput - iQty).ToString();

                        //programInfo.txtGood.Enabled = true;

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
                if (!IsNumeric(3, editCount.Text))
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

        private void BtnMark_Click(object sender, EventArgs e)
        {
            string rc_no = txtInput.Text.Trim();

            string process_id = IPQC_ProcessID;

            string part_id = programInfo.dsRC.Tables[0].Rows[0]["PART_ID"].ToString();

            string inspect_result = "INSPECT_OK";

            if (sender is Button b && b.Name == BtnMark_NG.Name)
            {
                inspect_result = "INSPECT_NG";
            }

            QcSrv.MarkAsInspected(
                rc_no: rc_no,
                part_id: part_id,
                process_id: process_id,
                inspect_result: inspect_result,
                inspect_time: DtpInspectTime.Value);

            if (QcSrv.IsRcInspected(
                rc_no: txtInput.Text.Trim(),
                process_id: IPQC_ProcessID,
                inspector_info: out DataRow inspector_info))
            {
                LbInspector.Text
                    = SajetCommon.SetLanguage("Inspector")
                    + " : "
                    + $"[{inspector_info["EMP_NO"]}] {inspector_info["EMP_NAME"]}";

                DtpInspectTime.Value = DateTime.Parse(inspector_info["UPDATE_TIME"].ToString());

                inspect_result = "INSPECT_OK";

                if (inspector_info["INSPECT_RESULT"].ToString() == "0")
                {
                    inspect_result = "INSPECT_NG";
                }

                Lb_Status.Text = SajetCommon.SetLanguage(inspect_result);

                BtnMark_OK.Enabled = false;
                BtnMark_NG.Enabled = false;

                DtpInspectTime.Enabled = false;

                BtnMark_OK.Click -= BtnMark_Click;
                BtnMark_NG.Click -= BtnMark_Click;
            }

        }

        private void OKtoPrint()
        {
            PrintRCLabel print = new PrintRCLabel();
            string message = string.Empty;
            print.Print(txtInput.Text, "1", out message);
        }

        /// <summary>
        /// Analyzing System Type - Y:lot control, N:piece control
        /// </summary>
        /// <returns>true:lot control, false:piece control</returns>
        private bool SystemType()
        {
            try
            {
                string sSQL = @"
SELECT
    PARAM_VALUE
FROM
    SAJET.SYS_BASE
WHERE
    PROGRAM = 'RC Manager'
AND PARAM_NAME = 'Lot Control Checked'
AND ROWNUM = 1
";
                DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);

                if (dsTemp != null)
                {
                    if (dsTemp.Tables[0].Rows.Count > 0)
                    {
                        g_SystemType = dsTemp.Tables[0].Rows[0]["PARAM_VALUE"].ToString();

                        if (g_SystemType == "Y")
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                return false;
            }
            catch (Exception)
            {
                return false;
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

        /// <summary>
        /// 檢查流程卡，合格的流程卡才能使用這個模組補資料
        /// </summary>
        /// <param name="RCNo"></param>
        /// <param name="sMessage"></param>
        /// <returns></returns>
        private bool CheckRCNo(string RCNo, out string sMessage)
        {
            sMessage = SajetCommon.SetLanguage("Success");

            // 檢查流程卡的狀態，不能正在加工中
            string sSQL = @"
SELECT
    WORK_ORDER,
    PART_ID,
    ROUTE_ID,
    NODE_ID,
    PROCESS_ID,
    CURRENT_STATUS
FROM
    SAJET.G_RC_STATUS
WHERE
    RC_NO = :RCNO
";
            var Params = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RCNO", RCNo }
            };

            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params.ToArray());

            if (dsTemp != null &&
                dsTemp.Tables[0].Rows.Count > 0)
            {
                string current_status = dsTemp.Tables[0].Rows[0]["CURRENT_STATUS"].ToString().Trim();

                if (s_function_parameter != "3")
                {
                    if (current_status == "9")
                    {
                        sMessage = SajetCommon.SetLanguage("Production of the runcard has been completed");

                        return false;
                    }
                    else if (current_status != "0")
                    {
                        sMessage = SajetCommon.SetLanguage("RC NO is not in queue.");

                        return false;
                    }
                }
            }
            else
            {
                sMessage = SajetCommon.SetLanguage("Can not find runcard");

                return false;
            }

            DataRow dr = dsTemp.Tables[0].Rows[0];

            string workOrder = dr["WORK_ORDER"].ToString();

            // 檢查流程卡所屬的工單的狀態，必須是「WIP」
            sSQL = @"
SELECT
    *
FROM
    SAJET.G_WO_STATUS
WHERE
    WO_STATUS = 3
    AND WORK_ORDER = :WORKORDER
    AND ROWNUM = 1
";
            Params = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "WORKORDER", workOrder },
            };

            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params.ToArray());

            if (dsTemp.Tables[0].Rows.Count <= 0)
            {
                sMessage = SajetCommon.SetLanguage("Work Order is not WIP.");

                return false;
            }

            // 檢查流程卡的生產紀錄，必須至少完成一個製程的加工
            // 檢驗標記除外
            sSQL = " SELECT * FROM SAJET.G_RC_TRAVEL WHERE RC_NO = :RCNO ";
            Params = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RCNO", RCNo },
            };

            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params.ToArray());

            if (dsTemp.Tables[0].Rows.Count <= 0 && s_function_parameter != "3")
            {
                sMessage = SajetCommon.SetLanguage("RC NO has not executed yet.");

                return false;
            }

            // 不同的操作模式，各自的檢查
            // 補登不良
            if (s_function_parameter == "2")
            {
                sSQL = @"
SELECT
    PROCESS_ID
FROM
    (
        SELECT
            *
        FROM
            SAJET.G_RC_TRAVEL
        WHERE
            RC_NO = TRIM(:RCNO)
        ORDER BY
            WIP_OUT_TIME DESC
    )
WHERE
    ROWNUM = 1
";
                Params = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "RCNO", RCNo },
                };

                DataSet ds = ClientUtils.ExecuteSQL(sSQL, Params.ToArray());

                if (ds.Tables[0].Rows.Count < 0 || ds.Tables[0].Rows[0]["PROCESS_ID"].ToString() != IPQC_ProcessID)
                {
                    sMessage = SajetCommon.SetLanguage("Process ID is not match up with RC NO");

                    return false;
                }

            }
            // 檢驗員標記
            else if (s_function_parameter == "3")
            {
                sSQL = @"
SELECT
    NODE_CONTENT
FROM
    SAJET.SYS_RC_ROUTE_DETAIL
WHERE
    ROUTE_ID = (
        SELECT
            ROUTE_ID
        FROM
            SAJET.G_RC_STATUS
        WHERE
            RC_NO = :RC_NO
    )
    AND NODE_CONTENT = :PROCESS_ID
";
                Params = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", RCNo },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", IPQC_ProcessID },
                };

                DataSet ds = ClientUtils.ExecuteSQL(sSQL, Params.ToArray());

                bool production_route_has_this_process
                    = ds != null
                    && ds.Tables[0].Rows.Count > 0;

                if (!production_route_has_this_process)
                {
                    sMessage = SajetCommon.SetLanguage(MessageEnum.TheProductionRouteOfTheRuncardDoesNotHaveThisProcess.ToString());

                    return false;
                }
            }

            return true;
        }

        private void ShowData()
        {
            string _message = string.Empty;
            if (!CheckRCNo(txtInput.Text, out _message))
            {
                MessageBox.Show(_message);
                txtInput.SelectAll();
                txtInput.Focus();
            }
            else
            {
                string sSQL = programInfo.sSQL["SQL"];

                if (s_function_parameter == "3")
                {
                    sSQL = QcSrv.GetSQLCommand();
                }

                var Params = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "SN", txtInput.Text },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", IPQC_ProcessID },
                };

                try
                {
                    programInfo.dsRC = ClientUtils.ExecuteSQL(sSQL, Params.ToArray());

                    if (programInfo.dsRC.Tables[0].Rows.Count == 0)
                    {
                        //SN錯誤時清空
                        ClearData();

                        string message = SajetCommon.SetLanguage("Can not find runcard");

                        SajetCommon.Show_Message(message, 0);

                        return;
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

                        #region 舊編、藍圖

                        // 舊編
                        TControlData former_no_control_set =
                            m_tControlData
                            .First(x => x.sFieldName == "FORMER_NO");

                        // 藍圖
                        TControlData blueprint_control_set =
                            m_tControlData
                            .First(x => x.sFieldName == "BLUEPRINT");

                        sSQL = $@"
SELECT
    A.OPTION2 FORMER_NO
   ,A.OPTION4 BLUEPRINT
FROM
    SAJET.SYS_PART A
   ,SAJET.G_RC_STATUS B
WHERE
    A.PART_ID = B.PART_ID
AND B.RC_NO = :RC_NO
";
                        Params = new List<object[]>()
                        {
                            new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", txtInput.Text }
                        };

                        DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params.ToArray());

                        if (dsTemp != null && dsTemp.Tables[0].Rows.Count > 0)
                        {
                            string former_no = dsTemp.Tables[0].Rows[0]["FORMER_NO"].ToString();

                            string blueprint = dsTemp.Tables[0].Rows[0]["BLUEPRINT"].ToString();

                            former_no_control_set.txtControl.Text = former_no;

                            blueprint_control_set.txtControl.Text = blueprint;
                        }

                        #endregion
                    }

                    programInfo.txtGood.KeyPress += new System.Windows.Forms.KeyPressEventHandler(TxtGood_KeyPress);
                }
                catch (Exception ex)
                {
                    SajetCommon.Show_Message(ex.Message, 0);
                }
            }
        }

        private void DspReportData()
        {
            if (string.IsNullOrEmpty(IPQC_ProcessID))
            {
                // 讀取 ini 檔
                switch (s_function_parameter)
                {
                    case "0":
                        IPQC_ProcessID = iniManager.ReadIniFile("IPQC", "PROCESS_ID", "");

                        break;
                    case "1":
                        IPQC_ProcessID = iniManager.ReadIniFile("Repair", "PROCESS_ID", "");

                        break;
                    case "2":
                        IPQC_ProcessID = iniManager.ReadIniFile("ProcessInputAAR", "PROCESS_ID", "");

                        break;
                    case "3":
                        IPQC_ProcessID = iniManager.ReadIniFile("INSPECTOR_MARK", "PROCESS_ID", "");

                        break;
                }

                if (string.IsNullOrEmpty(IPQC_ProcessID))
                {
                    using (var f = new SajetFilter.FFilter(sqlCommand: process_sql,
                                                           @params: process_params,
                                                           hiddenColumns: h))
                    {
                        f.Text = SajetCommon.SetLanguage("Process Set");

                        if (f.ShowDialog() == DialogResult.OK)
                        {
                            DataRow result = f.ResultSets[0];

                            string ProcessCode = result["PROCESS_CODE"].ToString();
                            string ProcessName = result["PROCESS_NAME"].ToString();
                            IPQC_ProcessID = result["PROCESS_ID"].ToString();

                            // 寫入 ini 檔
                            switch (s_function_parameter)
                            {
                                case "0":

                                    iniManager.WriteIniFile("IPQC", "PROCESS_ID", IPQC_ProcessID);

                                    break;
                                case "1":

                                    iniManager.WriteIniFile("Repair", "PROCESS_ID", IPQC_ProcessID);

                                    break;
                                case "2":

                                    iniManager.WriteIniFile("ProcessInputAAR", "PROCESS_ID", IPQC_ProcessID);

                                    break;
                                case "3":

                                    iniManager.WriteIniFile("INSPECTOR_MARK", "PROCESS_ID", IPQC_ProcessID);

                                    break;
                            }

                            lblProcess.Text = SajetCommon.SetLanguage("Process") + ":" + ProcessName;

                            if (!string.IsNullOrEmpty(lblProcess.Text))
                            {
                                btnClear.Enabled = true;
                                btnSearch.Enabled = true;
                                txtInput.Enabled = true;
                                if (BtnCancel.Visible)
                                {
                                    BtnCancel.Enabled = true;
                                }

                                if (s_function_parameter != "3")
                                {
                                    tsMemo.Enabled = true;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                programInfo.txtGood.Enabled = false;

                programInfo.txtScrap.Enabled = false;

                #region DIsplay Process Parameter 不用
                //if(this.tabPageParams.Parent != null)
                //{
                //    var _Params = new List<object[]>
                //    {
                //        new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", IPQC_ProcessID },
                //        new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", programInfo.dsRC.Tables[0].Rows[0]["PART_ID"].ToString() }
                //    };

                //    var _ds = ClientUtils.ExecuteSQL(programInfo.sSQL["製程條件"], _Params.ToArray());

                //    gvCondition.DataSource = _ds;

                //    gvCondition.DataMember = _ds.Tables[0].ToString();

                //    foreach (DataColumn dc in _ds.Tables[0].Columns)
                //    {
                //        gvCondition.Columns[dc.ColumnName].HeaderText = SajetCommon.SetLanguage(gvCondition.Columns[dc.ColumnName].HeaderText, 1);
                //    }

                //    gvCondition.Columns["VALUE_TYPE"].Visible = false;

                //    _ds = ClientUtils.ExecuteSQL(programInfo.sSQL["資料收集"], _Params.ToArray());

                //    if (gvInput.ColumnCount == 0)
                //    {
                //        for (int i = 0; i < _ds.Tables[0].Columns.Count; i++)
                //        {
                //            gvInput.Columns.Add(_ds.Tables[0].Columns[i].ColumnName, SajetCommon.SetLanguage(_ds.Tables[0].Columns[i].ColumnName, 1));

                //            if (i > programInfo.iInputVisible["資料收集"])
                //            {
                //                gvInput.Columns[i].Visible = false;
                //            }
                //            else
                //            {
                //                gvInput.Columns[i].ReadOnly = programInfo.iInputField["資料收集"].IndexOf(i) == -1;
                //            }
                //        }

                //        gvInput.Columns["UNIT_NO"].Visible = true;

                //        gvInput.Columns["UNIT_NO"].ReadOnly = true;

                //        gvInput.Columns["VALUE_DEFAULT"].HeaderText = SajetCommon.SetLanguage("Input value");
                //    }

                //    foreach (DataRow dr in _ds.Tables[0].Rows)
                //    {
                //        gvInput.Rows.Add(dr.ItemArray);

                //        if (dr["INPUT_TYPE"].ToString() == "S")
                //        {
                //            DataGridViewComboBoxCell tCell = new DataGridViewComboBoxCell();

                //            string[] slValue = dr["VALUE_LIST"].ToString().Split(',');

                //            for (int i = 0; i < slValue.Length - 1; i++)
                //            {
                //                tCell.Items.Add(slValue[i]);
                //            }

                //            gvInput["VALUE_DEFAULT", gvInput.Rows.Count - 1] = tCell;
                //        }

                //        if (dr["NECESSARY"].ToString() == "Y")
                //        {
                //            gvInput.Rows[gvInput.Rows.Count - 1].Cells["VALUE_DEFAULT"].Style.BackColor = Color.FromArgb(255, 255, 192);

                //            gvInput.Rows[gvInput.Rows.Count - 1].Cells["VALUE_DEFAULT"].ErrorText = SajetCommon.SetLanguage("Data Empty");
                //        }
                //    }
                //}

                #endregion

                #region Display Machine 不用

                //                var Params = new List<object[]>();
                //                DataSet ds;
                //                if (bOneSheet)
                //                {
                //                    string s = @"
                //SELECT
                //    B.MACHINE_ID
                //   ,MACHINE_CODE
                //   ,MACHINE_DESC
                //   ,D.STATUS_NAME
                //   ,SYSDATE ""START_TIME""
                //   ,RUN_FLAG
                //   ,DEFAULT_STATUS
                //   ,'' DATE_CODE
                //   ,0 LOAD_QTY
                //FROM
                //    SAJET.SYS_RC_PROCESS_MACHINE A
                //   ,SAJET.SYS_MACHINE B
                //   ,SAJET.G_MACHINE_STATUS C
                //   ,SAJET.SYS_MACHINE_STATUS D
                //WHERE
                //    A.PROCESS_ID = :PROCESS_ID
                //AND A.MACHINE_ID = B.MACHINE_ID
                //AND A.MACHINE_ID = C.MACHINE_ID
                //AND C.CURRENT_STATUS_ID = D.STATUS_ID
                //AND A.ENABLED = 'Y'
                //AND B.ENABLED = 'Y'
                //ORDER BY
                //    MACHINE_CODE
                //";

                //                    Params.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", IPQC_ProcessID });

                //                    //ds = ClientUtils.ExecuteSQL(programInfo.sSQL["OUTPUT MACHINE"], Params.ToArray());
                //                    ds = ClientUtils.ExecuteSQL(s, Params.ToArray());
                //                }
                //                else
                //                {
                //                    ds = Rfrns.GetMachineList(txtInput.Text);
                //                }

                //                WipInList = Rfrns.GetModels(ds);

                //                WipOutList = WipInList
                //                    .Select(e => new MachineDownModel(e))
                //                    .ToList();

                //                gvMachine.DataSource = WipOutList;

                #endregion

                #region Display Piece 不用

                //                Params = new List<object[]>
                //                    {
                //                        new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", txtInput.Text }
                //                    };

                //                ds = ClientUtils.ExecuteSQL(programInfo.sSQL["元件"], Params.ToArray());

                //                if (ds.Tables[0].Rows.Count > 0)
                //                {
                //                    DataGridViewTextBoxColumn dText = new DataGridViewTextBoxColumn
                //                    {
                //                        Name = "SERIAL_NUMBER",
                //                        HeaderText = SajetCommon.SetLanguage("SERIAL_NUMBER"),
                //                        ReadOnly = true
                //                    };

                //                    gvSN.Columns.Add(dText);

                //                    Params = new List<object[]>
                //                {
                //                    new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", IPQC_ProcessID},
                //                    new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", programInfo.dsRC.Tables[0].Rows[0]["PART_ID"].ToString() }
                //                };

                //                    programInfo.dsSNParam = ClientUtils.ExecuteSQL(programInfo.sSQL["元件製程參數"], Params.ToArray());

                //                    programInfo.iSNInput[0] = gvSN.Columns.Count;

                //                    programInfo.iSNInput[1] = -1;

                //                    if (programInfo.dsSNParam.Tables[0].Rows.Count > 0)
                //                    {
                //                        foreach (DataRow dr in programInfo.dsSNParam.Tables[0].Rows)
                //                        {
                //                            if (programInfo.iSNInput[1] == -1 && dr["ITEM_TYPE"].ToString() == "3")
                //                            {
                //                                for (int i = 1; i < ds.Tables[0].Columns.Count; i++)
                //                                {
                //                                    if (ds.Tables[0].Columns[i].ColumnName == "CURRENT_STATUS")
                //                                    {
                //                                        DataGridViewComboBoxColumn dComb = new DataGridViewComboBoxColumn
                //                                        {
                //                                            Name = ds.Tables[0].Columns[i].ColumnName,
                //                                            HeaderText = SajetCommon.SetLanguage(ds.Tables[0].Columns[i].ColumnName, 1),
                //                                            ReadOnly = false
                //                                        };

                //                                        dComb.Items.Add("OK");

                //                                        dComb.Items.Add("NG");

                //                                        gvSN.Columns.Add(dComb);
                //                                    }
                //                                    else
                //                                    {
                //                                        dText = new DataGridViewTextBoxColumn
                //                                        {
                //                                            Name = ds.Tables[0].Columns[i].ColumnName,
                //                                            HeaderText = SajetCommon.SetLanguage(ds.Tables[0].Columns[i].ColumnName, 1),
                //                                            ReadOnly = programInfo.iInputField["元件"].IndexOf(i) == -1
                //                                        };

                //                                        gvSN.Columns.Add(dText);
                //                                    }
                //                                }

                //                                programInfo.iSNInput[1] = gvSN.ColumnCount;
                //                            }

                //                            switch (dr["INPUT_TYPE"].ToString())
                //                            {
                //                                case "S":
                //                                    {
                //                                        DataGridViewComboBoxColumn dComb = new DataGridViewComboBoxColumn
                //                                        {
                //                                            Name = dr["ITEM_NAME"].ToString(),
                //                                            HeaderText = dr["ITEM_NAME"].ToString(),
                //                                            ReadOnly = dr["ITEM_TYPE"].ToString() == "2",
                //                                            Tag = dr["ITEM_ID"].ToString()
                //                                        };

                //                                        string[] slValue = dr["VALUE_LIST"].ToString().Split(',');

                //                                        if (dr["NECESSARY"].ToString() == "Y")
                //                                        {
                //                                            dComb.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 192);
                //                                        }
                //                                        else
                //                                        {
                //                                            dComb.Items.Add("");
                //                                        }

                //                                        for (int i = 0; i < slValue.Length - 1; i++)
                //                                        {
                //                                            dComb.Items.Add(slValue[i]);
                //                                        }

                //                                        gvSN.Columns.Add(dComb);

                //                                        break;
                //                                    }
                //                                default:
                //                                    {
                //                                        dText = new DataGridViewTextBoxColumn
                //                                        {
                //                                            Name = dr["ITEM_NAME"].ToString(),
                //                                            HeaderText = dr["ITEM_NAME"].ToString(),
                //                                            ReadOnly = dr["ITEM_TYPE"].ToString() == "2",
                //                                            Tag = dr["ITEM_ID"].ToString()
                //                                        };

                //                                        if (dr["NECESSARY"].ToString() == "Y")
                //                                        {
                //                                            dText.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 192);
                //                                        }

                //                                        gvSN.Columns.Add(dText);

                //                                        break;
                //                                    }
                //                            }
                //                        }
                //                    }
                //                    else
                //                    {
                //                        for (int i = 1; i < ds.Tables[0].Columns.Count; i++)
                //                        {
                //                            if (ds.Tables[0].Columns[i].ColumnName == "CURRENT_STATUS")
                //                            {
                //                                DataGridViewComboBoxColumn dComb = new DataGridViewComboBoxColumn
                //                                {
                //                                    Name = ds.Tables[0].Columns[i].ColumnName,
                //                                    HeaderText = SajetCommon.SetLanguage(ds.Tables[0].Columns[i].ColumnName, 1),
                //                                    ReadOnly = false
                //                                };

                //                                dComb.Items.Add("OK");

                //                                dComb.Items.Add("NG");

                //                                gvSN.Columns.Add(dComb);
                //                            }
                //                            else
                //                            {
                //                                dText = new DataGridViewTextBoxColumn
                //                                {
                //                                    Name = ds.Tables[0].Columns[i].ColumnName,
                //                                    HeaderText = SajetCommon.SetLanguage(ds.Tables[0].Columns[i].ColumnName, 1),
                //                                    ReadOnly = programInfo.iInputField["元件"].IndexOf(i) == -1
                //                                };

                //                                gvSN.Columns.Add(dText);
                //                            }
                //                        }
                //                    }

                //                    foreach (DataRow dr in ds.Tables[0].Rows)
                //                    {
                //                        gvSN.Rows.Add();

                //                        programInfo.slDefect.Add(dr["SERIAL_NUMBER"].ToString(), 0);

                //                        foreach (DataColumn dc in ds.Tables[0].Columns)
                //                        {
                //                            gvSN.Rows[gvSN.Rows.Count - 1].Cells[dc.ColumnName].Value = dr[dc.ColumnName].ToString();
                //                        }

                //                        foreach (DataRow drParam in programInfo.dsSNParam.Tables[0].Rows)
                //                        {
                //                            gvSN.Rows[gvSN.Rows.Count - 1].Cells[drParam["ITEM_NAME"].ToString()].Value = drParam["VALUE_DEFAULT"].ToString();

                //                            if (drParam["NECESSARY"].ToString() == "Y" && drParam["ITEM_TYPE"].ToString() == "3")
                //                            {
                //                                gvSN.Rows[gvSN.Rows.Count - 1].Cells[drParam["ITEM_NAME"].ToString()].ErrorText = SajetCommon.SetLanguage("Data Empty");
                //                            }
                //                        }
                //                    }

                //                    programInfo.txtGood.Text = ds.Tables[0].Rows.Count.ToString();
                //                }

                #endregion

                #region Display Defect Code

                if (TpDefect.Parent != null)
                {
                    gvDefect.Columns.Clear();

                    if (gvSN.Rows.Count > 0)
                    {
                        gvDefect.Columns.Add("SERIAL_NUMBER", SajetCommon.SetLanguage("SERIAL_NUMBER"));
                        gvDefect.Columns["SERIAL_NUMBER"].SortMode = DataGridViewColumnSortMode.NotSortable;
                    }

                    gvDefect.Columns.Add("DEFECT_CODE", SajetCommon.SetLanguage("DEFECT_CODE"));
                    gvDefect.Columns["DEFECT_CODE"].SortMode = DataGridViewColumnSortMode.NotSortable;

                    gvDefect.Columns.Add("DEFECT_DESC", SajetCommon.SetLanguage("DEFECT_DESC"));
                    gvDefect.Columns["DEFECT_DESC"].SortMode = DataGridViewColumnSortMode.NotSortable;

                    gvDefect.Columns.Add("QTY", SajetCommon.SetLanguage("QTY"));
                    gvDefect.Columns["QTY"].SortMode = DataGridViewColumnSortMode.NotSortable;

                    var Params = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", IPQC_ProcessID }
                };

                    var ds = ClientUtils.ExecuteSQL(programInfo.sSQL["PROCESS DEFECT"], Params.ToArray());

                    // 沒有製成不良不用跳提示 1.0.17003.19
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            gvDefect.Rows.Add();

                            foreach (DataColumn dc in ds.Tables[0].Columns)
                            {
                                gvDefect.Rows[gvDefect.Rows.Count - 1].Cells[dc.ColumnName].Value = dr[dc.ColumnName].ToString();

                                gvDefect.Columns[dc.ColumnName].ReadOnly = true;
                            }

                            gvDefect.Rows[gvDefect.Rows.Count - 1].Cells[gvDefect.Columns.Count - 1].Value = "0";
                        }
                    }

                    gvDefect.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

                    gvDefect.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);

                }

                #endregion

                #region Keyparts Collection

                if (bOneSheet)
                {
                    // Show BOM information
                    gvBOM.Columns.Clear();

                    gvBOM.Columns.Add("PART_NO", SajetCommon.SetLanguage("PART_NO"));
                    gvBOM.Columns["PART_NO"].SortMode = DataGridViewColumnSortMode.NotSortable;

                    gvBOM.Columns.Add("SPEC1", SajetCommon.SetLanguage("SPEC1"));
                    gvBOM.Columns["SPEC1"].SortMode = DataGridViewColumnSortMode.NotSortable;

                    gvBOM.Columns.Add("ITEM_GROUP", SajetCommon.SetLanguage("ITEM_GROUP"));
                    gvBOM.Columns["ITEM_GROUP"].SortMode = DataGridViewColumnSortMode.NotSortable;

                    //gvBOM.Columns.Add("IS_MATERIAL", SajetCommon.SetLanguage("IS_MATERIAL"));    // 增加物料清單是否為物料FLAG欄位

                    gvBOM.Columns.Add("PURCHASE", SajetCommon.SetLanguage("PURCHASE"));           //  是否為外購作為檢查物料序號的依據
                    gvBOM.Columns["PURCHASE"].SortMode = DataGridViewColumnSortMode.NotSortable;

                    gvBOM.Columns.Add("QTY", SajetCommon.SetLanguage("QTY"));
                    gvBOM.Columns["QTY"].SortMode = DataGridViewColumnSortMode.NotSortable;

                    var Params = new List<object[]>
                    {
                        new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", IPQC_ProcessID },
                        //new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", programInfo.dsRC.Tables[0].Rows[0]["PART_ID"].ToString() },
                        new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", programInfo.dsRC.Tables[0].Rows[0]["WO_OPTION2"].ToString() }
                    };

                    DataSet ds = ClientUtils.ExecuteSQL(programInfo.sSQL["KEYPARTS COLLECTION"], Params.ToArray());

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
                            gvBOM.Rows.Add();

                            foreach (DataColumn dc in ds.Tables[0].Columns)
                            {
                                gvBOM.Rows[gvBOM.Rows.Count - 1].Cells[dc.ColumnName].Value = dr[dc.ColumnName].ToString();

                                gvBOM.Columns[dc.ColumnName].ReadOnly = true;
                                gvBOM.Columns[dc.ColumnName].SortMode = DataGridViewColumnSortMode.NotSortable;
                            }

                            gvBOM.Rows[gvBOM.Rows.Count - 1].Cells[gvBOM.Columns.Count - 1].Value = "0";
                        }
                    }

                    gvBOM.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    // Build Keyparts column
                    gvKeypart.Columns.Clear();

                    gvKeypart.Columns.Add("PART_NO", SajetCommon.SetLanguage("PART_NO"));
                    gvKeypart.Columns["PART_NO"].SortMode = DataGridViewColumnSortMode.NotSortable;

                    gvKeypart.Columns.Add("RC_NO", SajetCommon.SetLanguage("RC_NO"));
                    gvKeypart.Columns["RC_NO"].SortMode = DataGridViewColumnSortMode.NotSortable;

                    gvKeypart.Columns.Add("ITEM_COUNT", SajetCommon.SetLanguage("ITEM_COUNT"));
                    gvKeypart.Columns["ITEM_COUNT"].SortMode = DataGridViewColumnSortMode.NotSortable;

                    gvKeypart.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
                else
                {
                    this.TpKeyparts.Parent = null;
                }

                #endregion

                #region 品保抽驗項目

                if (TpQCItems.Parent != null)
                {
                    TpQCItems.Text = SajetCommon.SetLanguage(FormTextEnum.QCSampling.ToString());

                    CbQCItem.Text = SajetCommon.SetLanguage(FormTextEnum.HasCollectData.ToString());

                    QcSrv.RC_NO = programInfo.dsRC.Tables[0].Rows[0]["RC_NO"].ToString();
                    QcSrv.WORK_ORDER = programInfo.dsRC.Tables[0].Rows[0]["WORK_ORDER"].ToString();
                    QcSrv.PROCESS_ID = programInfo.dsRC.Tables[0].Rows[0]["PROCESS_ID"].ToString();

                    int.TryParse(programInfo.dsRC.Tables[0].Rows[0]["QTY"].ToString(), out int current_qty);

                    int rc_sampled_qty = int.Parse(QcSrv.GetRCSampled());

                    if (current_qty > rc_sampled_qty)
                    {
                        QcSrv.LoadInput(ref DgvQCItem, ref programInfo);

                        LbQCInfo.Text = string.Empty;

                        if (DgvQCItem.Rows.Count > 0)
                        {
                            has_qc_sampling = true;

                            LbQCInfo.Visible = false;

                            CbQCItem.Enabled = true;

                            CbQCItem.Checked = false;
                        }
                        else
                        {
                            has_qc_sampling = false;

                            CbQCItem.Enabled = false;

                            CbQCItem.Checked = false;
                        }
                    }
                    else
                    {
                        has_qc_sampling = false;

                        LbQCInfo.Text = SajetCommon.SetLanguage(FormTextEnum.CompleteSampling.ToString());

                        LbQCInfo.Visible = true;

                        CbQCItem.Enabled = false;

                        CbQCItem.Checked = false;
                    }
                }
                else
                {
                    has_qc_sampling = false;

                    CbQCItem.Enabled = false;

                    CbQCItem.Checked = false;
                }

                if (!has_qc_sampling)
                {
                    TpQCItems.Parent = null;
                }

                #endregion

                #region 檢驗員註記

                if (this.TpQCMark.Parent != null)
                {
                    LbInspector.Text = SajetCommon.SetLanguage("Inspector") + " : ";

                    BtnMark_OK.Enabled = false;
                    BtnMark_NG.Enabled = false;

                    DtpInspectTime.Enabled = false;

                    BtnMark_OK.Click -= BtnMark_Click;
                    BtnMark_NG.Click -= BtnMark_Click;

                    if (QcSrv.IsRcInspected(
                        rc_no: txtInput.Text.Trim(),
                        process_id: IPQC_ProcessID,
                        inspector_info: out DataRow inspector_info))
                    {
                        LbInspector.Text += $"[{inspector_info["EMP_NO"]}] {inspector_info["EMP_NAME"]}";

                        DtpInspectTime.Value = DateTime.Parse(inspector_info["UPDATE_TIME"].ToString());

                        string inspect_result = "INSPECT_OK";

                        if (inspector_info["INSPECT_RESULT"].ToString() == "0")
                        {
                            inspect_result = "INSPECT_NG";
                        }

                        Lb_Status.Text = SajetCommon.SetLanguage(inspect_result);
                    }
                    else
                    {
                        BtnMark_OK.Enabled = true;
                        BtnMark_NG.Enabled = true;

                        DtpInspectTime.Enabled = true;

                        Lb_Status.Text = SajetCommon.SetLanguage("Mark as inspected");

                        DtpInspectTime.Value = OtSrv.GetDBDateTimeNow();

                        BtnMark_OK.Click += BtnMark_Click;
                        BtnMark_NG.Click += BtnMark_Click;
                    }

                }

                #endregion
            }
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

                #region 沒有要過站，繼承資訊就好
                /*
                //sSQL = string.Format(@"SELECT NODE_ID,NODE_TYPE,NODE_CONTENT,NEXT_NODE_ID,LINK_NAME FROM SAJET.SYS_RC_ROUTE_DETAIL WHERE ROUTE_ID={0} AND XML_INDEX={1}", sRoute_Id, index);
                sSQL = @"
SELECT
    NODE_ID
   ,NODE_TYPE
   ,NODE_CONTENT
   ,NEXT_NODE_ID
   ,LINK_NAME
FROM
    SAJET.SYS_RC_ROUTE_DETAIL
WHERE
    ROUTE_ID = :ROUTE_ID
AND NODE_ID = :NODE_ID
AND NODE_CONTENT = :NODE_CONTENT
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
                    TransferProcess f = new TransferProcess
                    {
                        sNode_type = rcRoute.sNode_type,
                        sRoute_Id = rcRoute.g_sRouteID,
                        sNode_Id = rcRoute.sNode_Id
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
                //*/
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);

                return false;
            }
        }

        /// <summary>
        /// 把重工數量記錄在另外一張資料表。
        /// </summary>
        /// <param name="rc_no"></param>
        /// <param name="node_id"></param>
        /// <param name="rework_qty"></param>
        private void SaveReworkQty(string rc_no, string process_id, string node_id, string travel_id, int rework_qty = 0, int bonus_qty = 0)
        {
            string S = @"
INSERT INTO
    SAJET.G_RC_TRAVEL_REWORK
(
    RC_NO
   ,PROCESS_ID
   ,NODE_ID
   ,REWORK_QTY
   ,UPDATE_TIME
   ,UPDATE_USERID
   ,TRAVEL_ID
   ,BONUS
)
VALUES
(
    :RC_NO
   ,:PROCESS_ID
   ,:NODE_ID
   ,:REWORK_QTY
   ,SYSDATE
   ,:UPDATE_USERID
   ,:TRAVEL_ID
   ,:BONUS
)";
            var P = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", rc_no },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", process_id },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "NODE_ID", node_id },
                new object[] { ParameterDirection.Input, OracleType.Number, "REWORK_QTY", rework_qty },
                new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", ClientUtils.UserPara1 },
                new object[] { ParameterDirection.Input, OracleType.Number, "TRAVEL_ID", travel_id },
                new object[] { ParameterDirection.Input, OracleType.Number, "BONUS", bonus_qty }
            };

            ClientUtils.ExecuteSQL(S, P.ToArray());
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
        private void RecordMachine(DataRow RcInfo, List<MachineDownModel> Machines)
        {
            string s = string.Empty;

            int i = 0;

            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", RcInfo["RC_NO"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.Number, "TRAVEL_ID", RcInfo["TRAVEL_ID"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.Number, "NODE_ID", RcInfo["NODE_ID"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.DateTime, "NOW_TIME", REPORT_TIME },
                new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", ClientUtils.UserPara1 },
            };

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

-- 未選機台的記錄也更新結束時間
UPDATE
    SAJET.G_RC_TRAVEL_MACHINE_DOWN
SET
    END_TIME = :NOW_TIME
   ,WORK_TIME_MINUTE = NVL(ROUND((TO_NUMBER(:NOW_TIME - START_TIME) * 24 * 60 * 60 + 29) / 60), 0)
   ,WORK_TIME_SECOND = NVL(ROUND(TO_NUMBER(:NOW_TIME - START_TIME) * 24 * 60 * 60, 0), 3)
   ,DATA_STATUS = 0
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
        /// 取得報工人員 ID
        /// </summary>
        private void GetEmpID()
        {
            string s = @"
SELECT
    EMP_ID
FROM
    SAJET.SYS_EMP
WHERE
    EMP_NO = :EMP_NO
";
            var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "EMP_NO", tsEmp.Text }
                };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            EmpID = d.Tables[0].Rows[0]["EMP_ID"].ToString();
        }

        private bool Check_Privilege(string EmpNo, string fun)
        {
            try
            {
                string sPrivilege = ClientUtils.GetPrivilege(EmpID, fun, ClientUtils.fProgramName).ToString();

                string sSQL = $@"
SELECT
    SAJET.SJ_PRIVILEGE_DEFINE('{fun}', '{sPrivilege}') TENABLED
FROM
    DUAL
";
                DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);

                string sRes = dsTemp.Tables[0].Rows[0]["TENABLED"].ToString();
                return (sRes == "Y");
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Function:SAJET.SJ_PRIVILEGE_DEFINE" + Environment.NewLine + ex.Message, 0);
                return false;
            }
        }

        private void CheckInput()
        {
            //var Params = new List<object[]>
            //{
            //    new object[] { ParameterDirection.Input, OracleType.VarChar, "TREV", txtInput.Text },
            //    new object[] { ParameterDirection.Input, OracleType.VarChar, "TSHEET", programInfo.sFunction },
            //    new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" }
            //};

            //DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_RC_CHK_ROUTE", Params.ToArray());

            //if (ds.Tables[0].Rows[0]["TRES"].ToString() != "OK")
            //{
            //    SajetCommon.Show_Message(SajetCommon.SetLanguage(ds.Tables[0].Rows[0]["TRES"].ToString()), 0);
            //}
            //else
            //{
            //    ClearData();

            //    ShowData();
            //}

            ShowData();
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
                for (int i = 0; i < gvKeypart.Rows.Count; i++)
                {
                    if (gvKeypart.Rows[i].Cells["RC_NO"].Value.ToString() == sRC)
                    {
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("Keypart SN is Duplicate."), 0);

                        return false;
                    }
                }

                if (gvBOM.CurrentRow.Cells["PURCHASE"].Value.ToString() == "Y")  // 不為外購要檢查料件序號規則
                {
                    var Params = new List<object[]>
                    {
                        new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", sRC },
                        new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_NO", gvBOM.CurrentRow.Cells["PART_NO"].Value.ToString() }
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
                                gvKeypart.Rows.Add();

                                foreach (DataColumn dc in ds.Tables[0].Columns)
                                {
                                    gvKeypart.Rows[gvKeypart.Rows.Count - 1].Cells[dc.ColumnName].Value = dr[dc.ColumnName].ToString();
                                }

                                gvKeypart.Rows[gvKeypart.Rows.Count - 1].Cells["ITEM_COUNT"].Value = editCount.Text.Trim();
                            }
                        }
                    }
                }
                else
                {
                    gvKeypart.Rows.Add();

                    gvKeypart.Rows[gvKeypart.Rows.Count - 1].Cells["PART_NO"].Value = gvBOM.CurrentRow.Cells["PART_NO"].Value.ToString();

                    gvKeypart.Rows[gvKeypart.Rows.Count - 1].Cells["RC_NO"].Value = editKPSN.Text.Trim();

                    gvKeypart.Rows[gvKeypart.Rows.Count - 1].Cells["ITEM_COUNT"].Value = editCount.Text.Trim();
                }

                gvKeypart.ReadOnly = true;

                // add quanity by item part
                CountKPSN();

                gvKeypart.Rows[gvKeypart.Rows.Count - 1].Selected = true;

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

                for (int i = 0; i < gvBOM.Rows.Count; i++)
                {
                    if (gvBOM.Rows[i].Cells["ITEM_GROUP"].Value.ToString() == "0")
                    {
                        // 在sys_bom.is_material為N
                        //if (gvBOM.Rows[i].Cells["QTY"].Value.ToString() == "0" && gvBOM.Rows[i].Cells["IS_MATERIAL"].Value.ToString() == "N")
                        if (gvBOM.Rows[i].Cells["QTY"].Value.ToString() == "0")
                        {
                            string message
                                = gvBOM.Rows[i].Cells["PART_NO"].EditedFormattedValue.ToString()
                                + " "
                                + SajetCommon.SetLanguage("does not input Keypart");

                            SajetCommon.Show_Message(message, 0);

                            return false;
                        }

                        iQty = 0;
                    }
                    else
                    {
                        sGroup = gvBOM.Rows[i].Cells["ITEM_GROUP"].Value.ToString();

                        for (int j = 0; j < gvBOM.Rows.Count; j++)
                        {
                            if (sGroup == gvBOM.Rows[j].Cells["ITEM_GROUP"].Value.ToString())
                            {
                                iQty += Convert.ToDouble(gvBOM.Rows[j].Cells["QTY"].Value.ToString());
                            }
                        }

                        //if (iQty == 0 && gvBOM.Rows[i].Cells["IS_MATERIAL"].Value.ToString() == "N")
                        if (iQty == 0)
                        {
                            string message
                                = gvBOM.Rows[i].Cells["PART_NO"].EditedFormattedValue.ToString()
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
            for (int i = 0; i < gvBOM.Rows.Count; i++)
            {
                double cnt = 0;

                for (int j = 0; j < gvKeypart.Rows.Count; j++)
                {
                    if (gvKeypart.Rows[j].Cells["PART_NO"].Value.ToString() == gvBOM.Rows[i].Cells["PART_NO"].Value.ToString())
                    {
                        cnt += Convert.ToDouble(gvKeypart.Rows[j].Cells["ITEM_COUNT"].Value.ToString());
                    }
                }

                gvBOM.Rows[i].Cells["QTY"].Value = cnt;
            }
        }

        /// <summary>
        /// 重工數量檢查不合格的處理。
        /// </summary>
        /// <param name="message"></param>
        private void ReworkMessage(string message)
        {
            SajetCommon.Show_Message(SajetCommon.SetLanguage(message), 0);

            tabPanelControl.SelectedTab = TpRework;

            TbRework.Focus();

            TbRework.Select(0, TbRework.Text.Length);
        }

        /// <summary>
        ///  判斷strNumber是否為指定類型的數字
        ///  1:正整數, 2:非負整數（正整數 + 0）, 3:正浮點數, 4:非負浮點數（正浮點數 + 0）, 5:浮點數
        /// </summary>
        /// <param name="iType"> 數值類型 </param>
        /// <param name="strNumber">判斷的字串</param>
        /// <returns>是返回True,否返回False</returns>
        public bool IsNumeric(int iType, string strNumber)
        {
            Regex NumberPattern;

            switch (iType)
            {
                case 1:   //正整數
                    {
                        NumberPattern = new Regex("^[0-9]*[1-9][0-9]*$");

                        break;
                    }
                case 2:   //非負整數（正整數 + 0）
                    {
                        NumberPattern = new Regex("^\\d+$");

                        break;
                    }
                case 3:   //正浮點數
                    {
                        NumberPattern = new Regex("^(([0-9]+\\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\\.[0-9]+)|([0-9]*[1-9][0-9]*))$");

                        break;
                    }
                case 4:   //非負浮點數（正浮點數 + 0）
                    {
                        NumberPattern = new Regex("^\\d+(\\.\\d+)?$");

                        break;
                    }
                case 5:    //浮點數
                    {
                        NumberPattern = new Regex("^(-?\\d+)(\\.\\d+)?$");

                        break;
                    }
                default:
                    return false;
            }

            return NumberPattern.IsMatch(strNumber);
        }

        /// <summary>
        /// 無該筆 SN / RC 時清空資料
        /// </summary>
        private void ClearData()
        {
            if (m_tControlData != null)
            {
                for (int i = 0; i < m_tControlData.Length; i++)
                {
                    m_tControlData[i].txtControl.Text = string.Empty;
                }
            }

            gvCondition.DataSource = null;

            gvMachine.DataSource = null;

            gvMachine.Refresh();

            gvSN.DataSource = null;

            gvSN.Refresh();

            gvInput.Rows.Clear();

            programInfo.slDefect.Clear();

            gvDefect.Rows.Clear();

            gvDefect.Columns.Clear();

            LbQCInfo.Text = string.Empty;

            CbQCItem.Enabled = false;

            CbQCItem.Checked = false;

            DgvQCItem.Rows.Clear();

            DgvQCItem.Columns.Clear();

            tsMemo.Text = "";

            tsMemo.Enabled = false;

            txtBonus.Text = "0";

            dtpOutDate.Value = OtSrv.GetDBDateTimeNow();

            dtpOutDate.Checked = false;

            if (this.TpRework.Parent != null)
            {
                TbRework.Text = "0";
            }

            if (this.TpParams.Parent != null)
            {
                tabPanelControl.SelectedTab = this.TpParams;
            }
            else if (this.TpQCItems.Parent != null)
            {
                tabPanelControl.SelectedTab = this.TpQCItems;
            }
            else if (this.TpMachine.Parent != null)
            {
                tabPanelControl.SelectedTab = this.TpMachine;
            }
            else if (this.TpDefect.Parent != null)
            {
                tabPanelControl.SelectedTab = this.TpDefect;
            }

            tabPanelControl.Enabled = false;

            BtnOK.Enabled = false;

            BtnMark_OK.Enabled = false;
            BtnMark_NG.Enabled = false;

            DtpInspectTime.Enabled = false;

            LbInspector.Text = "--";

            txtInput.Focus();
        }

        /// <summary>
        /// 呼叫過濾視窗執行製程資料篩選
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>       
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            string BasePath = Directory.GetCurrentDirectory().Contains("WIP") ?
            Directory.GetCurrentDirectory() : Path.Combine(Directory.GetCurrentDirectory(), "WIP");

            string fileName = Path.Combine(BasePath, "RCIPQC.ini");
            IniManager iniManager = new IniManager(fileName);

            using (var f = new SajetFilter.FFilter(sqlCommand: process_sql,
                                                   @params: process_params,
                                                   hiddenColumns: h))
            {
                f.Text = SajetCommon.SetLanguage("Process Set");

                if (f.ShowDialog() == DialogResult.OK)
                {
                    DataRow result = f.ResultSets[0];

                    string ProcessCode = result["PROCESS_CODE"].ToString();
                    string ProcessName = result["PROCESS_NAME"].ToString();
                    IPQC_ProcessID = result["PROCESS_ID"].ToString();

                    // 寫入 ini 檔
                    switch (s_function_parameter)
                    {
                        case "0":

                            iniManager.WriteIniFile("IPQC", "PROCESS_ID", IPQC_ProcessID);

                            break;
                        case "1":

                            iniManager.WriteIniFile("Repair", "PROCESS_ID", IPQC_ProcessID);

                            break;
                        case "2":

                            iniManager.WriteIniFile("ProcessInputAAR", "PROCESS_ID", IPQC_ProcessID);

                            break;
                        case "3":

                            iniManager.WriteIniFile("INSPECTOR_MARK", "PROCESS_ID", IPQC_ProcessID);

                            break;
                    }

                    lblProcess.Text = SajetCommon.SetLanguage("Process") + ":" + ProcessName;

                    if (!string.IsNullOrEmpty(lblProcess.Text))
                    {
                        btnClear.Enabled = true;
                        btnSearch.Enabled = true;
                        txtInput.Enabled = true;
                        if (BtnCancel.Visible)
                        {
                            BtnCancel.Enabled = true;
                        }

                        if (s_function_parameter != "3")
                        {
                            tsMemo.Enabled = true;
                        }
                    }
                }
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearData();
            txtInput.Text = "";
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            ClearData();
            CheckInput();
            if (s_function_parameter != "3")
            {
                tsMemo.Enabled = true;
                BtnOK.Enabled = true;
            }
            tabPanelControl.Enabled = true;
            txtInput.SelectAll();
        }
    }
}