using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data.OleDb;
using System.Reflection;
using System.Runtime.InteropServices;
using SajetClass;
using System.IO;
using System.Data.OracleClient;
using System.Collections;
// 拿掉關帳時間
namespace RCTravel
{
    public partial class fMain : Form
    {
        struct TProgramInfo
        {
            public string sProgram;
            public string sFunction;
            public string sExeName;
            public Dictionary<string, string> slSQL;
            public List<int> slWidth;
        }

        TProgramInfo programInfo;

        string sIniFile = Application.StartupPath + Path.DirectorySeparatorChar + "Sajet.Ini";
        int iLineID = 0;
        string sOutput = "0";
        public static string sGroup = "N/A";
        string g_sHourMinSec = "000000";

        /// <summary>
        /// 是不是執行區塊
        /// </summary>
        bool IsExecuteRegion = true;

        /// <summary>
        /// 允許操作下半部查詢介面
        /// </summary>
        bool AllowToQuery = false;

        public fMain()
        {
            InitializeComponent();

            SajetCommon.SetLanguageControl(this);

            CkbStage.CheckedChanged += CkbStage_CheckedChanged;

            CkbProcess.CheckedChanged += CkbProcess_CheckedChanged;
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            tsType.Items.Add(SajetCommon.SetLanguage("RC_NO"));
            tsType.SelectedIndex = 0;

            programInfo.sFunction = ClientUtils.fFunctionName;
            programInfo.sProgram = ClientUtils.fProgramName;
            programInfo.sExeName = ClientUtils.fCurrentProject;

            SajetInifile sini = new SajetInifile();
            string sDefault = sini.ReadIniFile(sIniFile, "RC Travel", "表格欄寬", "");
            sini.Dispose();
            programInfo.slWidth = new List<int>();

            if (!string.IsNullOrEmpty(sDefault))
            {
                string[] slCol = sDefault.Split(',');

                for (int i = 0; i < slCol.Length; i++)
                    if (!string.IsNullOrEmpty(slCol[i]))
                        programInfo.slWidth.Add(int.Parse(slCol[i]));
            }

            string sSQL = @"
SELECT DEFAULT_VALUE
      ,PARAM_VALUE
FROM SAJET.SYS_BASE_PARAM
WHERE PROGRAM = :PROGRAM
AND PARAM_NAME = :PARAM_NAME
AND PARAM_TYPE = 'SQL'
";
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROGRAM", programInfo.sProgram };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PARAM_NAME", programInfo.sFunction };
            DataSet ds = ClientUtils.ExecuteSQL(sSQL, Params);
            programInfo.slSQL = new Dictionary<string, string>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                programInfo.slSQL.Add(dr[0].ToString(), dr[1].ToString());
            }
            ds.Dispose();

            Check_Privilege();
        }

        private void fMain_Shown(object sender, EventArgs e)
        {
            DisableFrom();
            CkbStage_CheckedChanged(null, null);
            CkbProcess_CheckedChanged(null, null);

            var new_font = new Font("微軟正黑體", 12, FontStyle.Regular);

            LbRuncard.Font = new_font;
            LbProcess.Font = new_font;
            LbSheet.Font = new_font;

            txtEmp.Focus();
        }

        private void combStage_SelectedIndexChanged(object sender, EventArgs e)
        {
            combProcess.Items.Clear();

            // 只顯示使用者有報工權限的製程站
            string sSQL = $@"
SELECT A.PROCESS_NAME
FROM SAJET.SYS_PROCESS A
    ,SAJET.SYS_STAGE   B
WHERE A.STAGE_ID = B.STAGE_ID
AND A.ENABLED    = 'Y'
AND B.STAGE_NAME = '{combStage.Text}'
AND A.PROCESS_ID IN
(
    SELECT A.PROCESS_ID
    FROM SAJET.SYS_ROLE_OP_PRIVILEGE A
        ,SAJET.SYS_EMP B
        ,SAJET.SYS_ROLE_EMP C
    WHERE A.ROLE_ID = C.ROLE_ID
    AND B.EMP_ID = C.EMP_ID
    AND B.EMP_NO = '{txtEmp.Text}'

    UNION

    SELECT A.PROCESS_ID
    FROM SAJET.SYS_EMP_PROCESS_PRIVILEGE A
        ,SAJET.SYS_EMP B
    WHERE A.EMP_ID = B.EMP_ID
    AND B.EMP_NO = '{txtEmp.Text}'
)
ORDER BY A.PROCESS_ID
";
            int dropDownHeight = 0;
            DataSet ds = ClientUtils.ExecuteSQL(sSQL);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                combProcess.Items.Add(dr[0].ToString());
                dropDownHeight++;
            }
            combProcess.DropDownHeight = 20 + dropDownHeight * 20;

            if (combProcess.DropDownHeight > 200)
            {
                combProcess.DropDownHeight = 200;
            }

            if (combProcess.Items.Count > 0)
            {
                combProcess.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// CheckBox 控制，是否使用 區段 作為過濾條件。不用的時候鎖定控制項。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CkbStage_CheckedChanged(object sender, EventArgs e)
        {
            bool enabled = CkbStage.Checked;

            combStage.Enabled = CkbProcess.Enabled = enabled;
        }

        /// <summary>
        /// CheckBox 控制，是否使用 製程 作為過濾條件。不用的時候鎖定控制項。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CkbProcess_CheckedChanged(object sender, EventArgs e)
        {
            bool enabled = CkbProcess.Checked;

            combProcess.Enabled = enabled;
        }

        private void txtEmp_KeyPress(object sender, KeyPressEventArgs e)
        {
            #region 2016.7.4 By Jason
            if (e.KeyChar != 3)
            {
                txtSN.Text = "";
                txtData.Text = "";
                txtWo.Text = "";
                dgvRC.DataSource = null;
            }
            #endregion// 2016.7.4 End

            if (e.KeyChar != (char)Keys.Return &&
                e.KeyChar != (char)Keys.Enter &&
                e.KeyChar != (char)Keys.Tab
                )
            {
                return;
            }

            Check_Privilege();

            if (CheckEmp())
            {
                BtnReset.Enabled = true;
                this.GetEMPName();
                if (ReloadStage())
                {
                    txtEmp.Enabled = false;
                    EnableForm();
                    txtSN.Text = string.Empty;
                    txtSN.Focus();
                }
                else
                {
                    BtnReset.Enabled = false;
                    DisableFrom();
                    txtEmp.Text = string.Empty;
                    label7.Text = "N/A";
                    txtEmp.Focus();
                }
            }
        }

        private void GetEMPName()
        {
            string SQL = @"
SELECT EMP_NAME
      ,PDLINE_ID
FROM SAJET.SYS_EMP
WHERE EMP_NO = :EMP_NO
AND ENABLED  = 'Y'
AND ROWNUM   = 1
";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "EMP_NO", txtEmp.Text };
            DataSet ds1 = ClientUtils.ExecuteSQL(SQL, Params);

            if (ds1.Tables[0].Rows.Count > 0)
            {
                this.label7.Text = ds1.Tables[0].Rows[0][0].ToString();

                #region 2016.7.4 By Jason
                iLineID = Convert.ToInt32(ds1.Tables[0].Rows[0][1].ToString());
                #endregion// 2016.7.4 End
            }
        }

        private bool CheckEmp()
        {
            txtEmp.Text = txtEmp.Text.Trim().ToUpper();

            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TREV", txtEmp.Text };
            Params[1] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
            DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_CKSYS_EMP", Params);

            if (ds.Tables[0].Rows[0]["TRES"].ToString() != "OK")
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage(ds.Tables[0].Rows[0]["TRES"].ToString()), 0);
                if (txtEmp.Text != ClientUtils.fLoginUser)
                {
                    DisableFrom();
                    txtEmp.Enabled = true;
                    txtEmp.Text = "";
                    label7.Text = "N/A";
                }
                txtEmp.SelectAll();
                txtEmp.Focus();
                return false;
            }
            else
            {
                return true;
            }
        }

        private void txtSN_KeyPress(object sender, KeyPressEventArgs e)
        {
            string message = string.Empty;

            if (e.KeyChar != (char)Keys.Return ||
                string.IsNullOrWhiteSpace(txtSN.Text))
            {
                return;
            }
            else
            {
                txtSN.Text = txtSN.Text.Trim().ToUpper();
            }

            if (!CheckEmp()) return;

            //添加修改 
            //获取所选列的流程卡号
            string sRC = txtSN.Text;
            List<string> RCSets = new List<string>();

            string sSQL2;
            DataSet dsTemp2;

            sSQL2 = $@"
SELECT B.WO_STATUS
FROM SAJET.G_RC_STATUS A,SAJET.G_WO_BASE B
WHERE A.WORK_ORDER = B.WORK_ORDER
AND A.RC_NO = '{sRC}'
";
            dsTemp2 = ClientUtils.ExecuteSQL(sSQL2);

            if (dsTemp2 == null || dsTemp2.Tables[0].Rows.Count <= 0)
            {
                message = SajetCommon.SetLanguage("RC or SN not found");
                SajetCommon.Show_Message(message, 1);

                txtSN.SelectAll();
                txtSN.Focus();
                return;
            }

            #region 依照工單狀態顯示訊息

            string wo_status = dsTemp2.Tables[0].Rows[0]["WO_STATUS"].ToString();

            if (wo_status != "3")
            {
                message = SajetCommon.SetLanguage("Work Order Status must be WIP");
                SajetCommon.Show_Message(message, 1);
                return;
            }

            #endregion

            if (CheckEmp())
            {
                this.GetEMPName();
                txtSN.SelectAll();
                txtSN.Focus();
            }

            RCSets.Add(sRC);

            //获取制程名
            string SQL = $@"
SELECT decode(PROCESS_NAME, '', 'Group', PROCESS_NAME) PROCESS_NAME
      ,A.current_status
FROM SAJET.G_RC_STATUS  A
    ,SAJET.SYS_PROCESS  B
    ,SAJET.SYS_RC_ROUTE C
    ,SAJET.SYS_STAGE    D
WHERE A.PROCESS_ID    = B.PROCESS_ID(+)
AND A.ROUTE_ID        = C.ROUTE_ID
AND A.STAGE_ID        = D.STAGE_ID
AND A.CURRENT_STATUS <> 8
AND A.RC_NO           = '{sRC}'
";
            DataSet sProcessType = ClientUtils.ExecuteSQL(SQL);

            //添加判断是否为SN
            if (sProcessType.Tables[0].Rows.Count == 0)
            {
                SQL = $@"
select a.rc_no
from sajet.g_sn_status a
where a.serial_number='{sRC}'
";
                sProcessType = ClientUtils.ExecuteSQL(SQL);

                if (sProcessType.Tables[0].Rows.Count == 0)
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("RC or SN not found"), 1);
                    txtSN.SelectAll();
                    txtSN.Focus();
                    return;
                }
                else
                {
                    if (sProcessType.Tables[0].Rows[0][0].ToString() != "N/A")
                    {
                        message = SajetCommon.SetLanguage("Please enter the SN binding RC");//请输入SN绑定的RC执行
                        SajetCommon.Show_Message(message, 1);
                        txtSN.SelectAll();
                        txtSN.Focus();
                        return;
                    }

                    SQL = $@"
SELECT decode(PROCESS_NAME, '', 'Group', PROCESS_NAME) PROCESS_NAME
      ,A.current_status
FROM SAJET.g_sn_status  A
    ,SAJET.SYS_PROCESS  B
    ,SAJET.SYS_RC_ROUTE C
    ,SAJET.SYS_STAGE    D
WHERE A.PROCESS_ID  = B.PROCESS_ID
AND A.ROUTE_ID      = C.ROUTE_ID
AND A.STAGE_ID      = D.STAGE_ID
AND A.SERIAL_NUMBER = '{sRC}'
";

                    sProcessType = ClientUtils.ExecuteSQL(SQL);
                }
            }

            string sProcess = sProcessType.Tables[0].Rows[0][0].ToString();
            string ifHold = sProcessType.Tables[0].Rows[0][1].ToString();
            string group = "Group";

            if (ifHold == "2")
            {
                message = SajetCommon.SetLanguage("Has been Hold, can not be executed");//已被HOLD，无法执行
                SajetCommon.Show_Message(message, 1);
                txtSN.SelectAll();
                txtSN.Focus();
                return;
            }

            #region 2017.3.18 By Jason
            if (ifHold == "7")
            {
                message = SajetCommon.SetLanguage("Has been Merge, can not be executed");
                SajetCommon.Show_Message(message, 1);
                return;
            }

            if (ifHold == "8")
            {
                message = SajetCommon.SetLanguage("Has been Scrap, can not be executed");
                SajetCommon.Show_Message(message, 1);
                return;
            }

            if (ifHold == "9")
            {
                message = SajetCommon.SetLanguage("Has been Finish, can not be executed");
                SajetCommon.Show_Message(message, 1);
                return;
            }

            if (ifHold == "10")
            {
                message = SajetCommon.SetLanguage("Has been Inv, can not be executed");
                SajetCommon.Show_Message(message, 1);
                return;
            }

            if (ifHold == "11")
            {
                message = SajetCommon.SetLanguage("Has been Wait Transfer, can not be executed");
                SajetCommon.Show_Message(message, 1);
                return;
            }

            if (ifHold == "12")
            {
                message = SajetCommon.SetLanguage("Has been Transfer, can not be executed");
                SajetCommon.Show_Message(message, 1);
                return;
            }
            #endregion

            #region 檢查 RC 所在製程的 OPERATE_TYPE = INPUT 才可以使用 WIP 模組報工

            sSQL2 = "SELECT B.OPERATE_ID FROM SAJET.G_RC_STATUS A, SAJET.SYS_PROCESS B WHERE A.RC_NO = '" + sRC + "' AND A.PROCESS_ID = B.PROCESS_ID";
            dsTemp2 = ClientUtils.ExecuteSQL(sSQL2);
            if (dsTemp2 == null ||
                dsTemp2.Tables[0].Rows.Count <= 0 ||
                dsTemp2.Tables[0].Rows[0]["OPERATE_ID"].ToString() != "5")
            {
                message = SajetCommon.SetLanguage("The process station where the runcard is located cannot report work with WIP module");
                SajetCommon.Show_Message(message, 1);

                txtSN.SelectAll();
                txtSN.Focus();
                return;
            }

            #endregion

            string gStatus = "";
            //如果是群组
            if (sProcess == group)
            {
                DataSet dsGrid;

                //如果已經結束了，彈出提示
                string sSQL = $@"
SELECT SAJET.SJ_RC_STATUS(A.CURRENT_STATUS) RC_STATUS
FROM SAJET.G_RC_STATUS  A
    ,SAJET.SYS_PROCESS  B
    ,SAJET.SYS_RC_ROUTE C
    ,SAJET.SYS_STAGE    D
WHERE A.PROCESS_ID    = B.PROCESS_ID(+)
AND A.ROUTE_ID        = C.ROUTE_ID
AND A.STAGE_ID        = D.STAGE_ID
AND A.CURRENT_STATUS <> 8
AND A.RC_NO = '{sRC}'
";
                dsGrid = ClientUtils.ExecuteSQL(sSQL);
                string finish = dsGrid.Tables[0].Rows[0][0].ToString();

                if (finish == "Finish")
                {
                    message = SajetCommon.SetLanguage("Process cards or components have been completed");//流程卡或元件已完工
                    SajetCommon.Show_Message(message, 1);
                    //MessageBox.Show("流程卡或元件已完工！","提示");
                    txtSN.SelectAll();
                    txtSN.Focus();
                    return;
                }//彈出結束

                DataSet dsTemp = new DataSet();
                sSQL = $@"
select sheet_phase
      ,process_type
      ,status
from sajet.g_rc_process_group
where rc_no='{sRC}'
";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);

                if (dsTemp != null && dsTemp.Tables[0].Rows.Count > 0)
                {
                    string SHEET_PHASE = dsTemp.Tables[0].Rows[0][0].ToString();
                    sGroup = dsTemp.Tables[0].Rows[0][1].ToString();
                    gStatus = dsTemp.Tables[0].Rows[0][2].ToString();
                }

                if (gStatus.ToLower() == "N/A".ToLower())
                {
                    showGroupProcess show = new showGroupProcess(sRC);

                    if (show.ShowDialog() != DialogResult.OK)
                    {
                        txtSN.SelectAll();
                        txtSN.Focus();
                        return;
                    }
                }
                else
                {
                    showGroupProcess show = new showGroupProcess(sRC);

                    if (show.ShowDialog() != DialogResult.OK)
                    {
                        txtSN.SelectAll();
                        txtSN.Focus();
                        return;
                    }
                }
            }

            CheckInput(RCSets: RCSets, RC: sRC);

            // 1.3.17003.22 做完每個動作都要重新確認員工號
            if (txtEmp.Text != ClientUtils.fLoginUser)
            {
                DisableFrom();
                txtEmp.Enabled = true;
                txtEmp.Text = "";
                label7.Text = "N/A";
            }

            dgvRC.DataSource = null;

            txtEmp.SelectAll();
            txtEmp.Focus();
        }

        /// <summary>
        /// 按按鈕等於按 Enter 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEnter_Click(object sender, EventArgs e)
        {
            txtSN_KeyPress(null, new KeyPressEventArgs(keyChar: (char)Keys.Return));
        }

        /// <summary>
        /// 搜尋流程卡歷程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTravel_Click(object sender, EventArgs e)
        {
            ShowTravel(txtSN.Text.Trim().ToUpper());

            txtSN.Focus();
        }

        /// <summary>
        /// 清空查詢歷程記錄
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClear_Click(object sender, EventArgs e)
        {
            ShowTravel();

            txtSN.Text = "";

            txtSN.Focus();
        }

        private void CheckInput(List<string> RCSets, string RC)
        {
            object[][] Params;
            string sTSHEET_SN = string.Empty;

            foreach (var sSN in RCSets)
            {
                // 檢查 EMP 或 ROLE 有沒有權限報工
                Params = new object[3][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TEMP", txtEmp.Text };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TRC", sSN };
                Params[2] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };

                DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_CKSYS_PROCESS_OP_PERMISSION", Params);

                string sTRES2 = ds.Tables[0].Rows[0]["TRES"].ToString();

                if (sTRES2 != "OK")
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("User does not have permission on this process station"), 0);
                    txtEmp.Text = "";
                    label7.Text = "N/A";
                    txtEmp.Focus();
                    return;
                }

                Params = new object[4][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TREV", sSN };
                Params[1] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                Params[2] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TTYPE", "" };
                Params[3] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TSHEET_SN", "" };

                ds = ClientUtils.ExecuteProc("SAJET.SJ_RC_CHK_INPUT", Params);

                string sTRES1 = ds.Tables[0].Rows[0]["TRES"].ToString();

                if (sSN == RC)
                {
                    sTSHEET_SN = ds.Tables[0].Rows[0]["TSHEET_SN"].ToString();
                }

                if (sTRES1 != "OK")
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage(sTRES1), 0);
                    return;
                }
            }

            //Call Dll                
            string sSQL = "SELECT DLL_FILENAME, FORM_NAME FROM SAJET.SYS_RC_SHEET WHERE SHEET_NAME = :SHEET_NAME AND ROWNUM = 1";
            Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_NAME", sTSHEET_SN };
            DataSet ds1 = ClientUtils.ExecuteSQL(sSQL, Params);

            if (ds1.Tables[0].Rows.Count > 0)
            {
                try
                {
                    //工單報工改呼叫 RCOutput_WO 時, dll 名稱加上 "_WO"
                    bool ReportByWO = CbByWorkOrder.CheckState == CheckState.Checked;
                    bool ReportByRoute = CbByRoute.CheckState == CheckState.Checked;
                    // 使用 T4 或 T6 的製程，要執行工單投入 / 工單產出 模組
                    bool IsT4T6 = IsUsingT4T6(rc_no: RC);
                    // 10B 的第一站（I010 領料），要使用工單產出模組
                    bool IsFirst10B = IsFirstProcessOf10B(rc_no: RC);

                    bool OnlyOneSheet = CheckProcessSheet(RC);
                    bool NoRuncardToWipOut = !FindRuncardsToReportByRoute(targetRuncard: RC);

                    // 製程配置為 先投入再產出 的製程，才可以使用工單報工模組
                    if (ReportByWO && OnlyOneSheet)
                    {
                        string message = SajetCommon.SetLanguage("This process has been set to execute wip-out only, so job report by WO is not allowed to use");

                        SajetCommon.Show_Message(message, 1);

                        return;
                    }

                    if (ReportByRoute && NoRuncardToWipOut)
                    {
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("No runcard to wip out"), 1);

                        return;
                    }

                    #region 呼叫的組件的資訊

                    string CallDllSuffix = (ReportByWO || IsT4T6 || IsFirst10B) ? "_WO" : "";

                    string routing
                        = Application.StartupPath + Path.DirectorySeparatorChar
                        + programInfo.sExeName + Path.DirectorySeparatorChar;

                    string DllName = ds1.Tables[0].Rows[0]["DLL_FILENAME"].ToString() + CallDllSuffix;

                    // 特例處理
                    // 途程報工
                    if (ReportByRoute)
                    {
                        DllName = "RCOutput_Route";

                        sTSHEET_SN = "RC Output";
                    }

                    // 10B 領料
                    if (IsFirst10B)
                    {
                        sTSHEET_SN = "RC Output";
                    }

                    string dllInfo = routing + DllName + ".dll";

                    #endregion

                    sOutput = DateTime.Now.ToString("yyyyMMddHHmmss");

                    Assembly assembly = Assembly.LoadFrom(dllInfo);

                    string[] Name = assembly.FullName.ToString().Split(',');
                    Type type = assembly.GetType(Name[0] + "." + ds1.Tables[0].Rows[0]["FORM_NAME"].ToString());

                    object[] arg = new object[] { sTSHEET_SN, RC, txtEmp.Text, sOutput };
                    ClientUtils.fCurrentProject = programInfo.sExeName;
                    ClientUtils.fProgramName = programInfo.sProgram;
                    object obj = assembly.CreateInstance(type.FullName, true, BindingFlags.CreateInstance, null, arg, null, null);
                    Form formChild = (Form)obj;

                    formChild.ShowDialog();

                    if (IsExecuteRegion)
                    {
                        ShowTravel(SelectedRuncard: RC);
                    }
                    else
                    {
                        ShowTravel();
                    }
                }
                catch (Exception ex)
                {
                    if (CbByWorkOrder.CheckState == CheckState.Checked ||
                        CbByRoute.CheckState == CheckState.Checked)
                    {
                        CbByWorkOrder.CheckState = CbByRoute.CheckState
                            = CheckState.Unchecked;

                        SajetCommon.Show_Message(SajetCommon.SetLanguage("Do not support job report by WO / Route module"), 1);
                    }
                    else
                    {
                        SajetCommon.Show_Message(ex.Message, 1);
                    }
                }
            }
            else
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Undefine"), 0);
            }

            txtSN.Text = "";
            txtEmp.Focus();
        }

        /// <summary>
        /// 根据条件查询符合条件的RC_NO信息
        /// </summary>
        private void QueryDate()
        {
            string sSQL = string.Empty, sTemp = string.Empty;
            List<object[]> slParam = new List<object[]>();

            if (!string.IsNullOrEmpty(txtWo.Text))
            {
                sTemp = " AND A.WORK_ORDER = :WORK_ORDER ";
                slParam.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", txtWo.Text });
            }

            #region 2016.7.4 By Jason
            if (iLineID > 0)
            {
                sTemp += " AND A.PDLINE_ID = :PDLINE_ID ";
                slParam.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "PDLINE_ID", iLineID });
            }
            #endregion// 2016.7.4 End

            // 不強制使用區段、製程過濾流程卡
            // 不勾選的時候就把有報工權限的製程的流程卡都列出來
            if (CkbStage.Checked)
            {
                if (!string.IsNullOrEmpty(combStage.Text))
                {
                    sTemp += " AND STAGE_NAME = :STAGE_NAME ";
                    slParam.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "STAGE_NAME", combStage.Text });
                }
                else
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("User does not have permission on this stage"), 0);
                    dgvRC.DataSource = null;
                    return;
                }
            }

            if (CkbProcess.Checked)
            {
                if (!string.IsNullOrEmpty(combProcess.Text))
                {
                    sTemp += " AND PROCESS_NAME = :PROCESS_NAME ";
                    slParam.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_NAME", combProcess.Text });
                }
                else
                {
                    dgvRC.DataSource = null;
                    return;
                }
            }
            else
            {
                sTemp += @"
    AND a.process_id IN (
        SELECT
            a.process_id
        FROM
            sajet.sys_role_op_privilege   a,
            sajet.sys_emp                 b,
            sajet.sys_role_emp            c
        WHERE
            a.role_id = c.role_id
            AND b.emp_id = c.emp_id
            AND b.emp_no = :emp_no
        UNION
        SELECT
            a.process_id
        FROM
            sajet.sys_emp_process_privilege   a,
            sajet.sys_emp                     b
        WHERE
            a.emp_id = b.emp_id
            AND b.emp_no = :emp_no
    )";
                slParam.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "EMP_NO", txtEmp.Text });
            }

            switch (tsType.SelectedIndex)
            {
                case 0:
                    sSQL = programInfo.slSQL["RC"];

                    if (!string.IsNullOrEmpty(txtData.Text) && (string.IsNullOrEmpty(txtSN.Text)))
                    {
                        sTemp += " AND A.RC_NO = :RC_NO ";
                        slParam.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", txtData.Text });
                    }
                    break;
                default:
                    sSQL = programInfo.slSQL["SN"];

                    if (!string.IsNullOrEmpty(txtData.Text))
                    {
                        sTemp += " AND A.SERIAL_NUMBER = :RC_NO ";
                        slParam.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", txtData.Text });
                    }
                    break;
            }

            // 直接過濾出可以使用工單報工模組的流程卡
            if (CbByWorkOrder.Checked)
            {
                sTemp += @"
    AND ( ( a.current_status = 0
            AND a.sheet_name = 'RC Input' )
          OR ( a.current_status = 1
               AND a.sheet_name = 'RC Output' ) )
";
            }

            // 直接過濾出可以使用途程報工模組的流程卡
            if (CbByRoute.Checked)
            {
                sTemp += @"
    AND a.rc_no IN (
        SELECT
            rc_no
        FROM
            sajet.g_rc_status            a,
            sajet.sys_rc_process_sheet   b
        WHERE
            a.process_id = b.process_id
            AND a.sheet_name = b.sheet_name
            AND b.sheet_phase = 'I'
    )
";
            }

            if (CbByRoute.Checked || CbByWorkOrder.Checked)
            {
                sTemp += @"
    AND a.process_id NOT IN (
        SELECT
            process_id
        FROM
            sajet.sys_process_option
        WHERE
            -- OR option1 = 'Y'
            -- OR option2 = 'Y'
            -- OR option3 = 'Y'
            option4 = 'Y'
            OR option5 = 'Y'
            OR option6 = 'Y'
            OR option7 = 'Y'
            OR option8 = 'Y'
            OR option9 = 'Y'
            OR option10 = 'Y'
    )
";
            }

            // 2015/08/24, Aaron
            //sTemp += " AND A.CURRENT_STATUS!='9' ";
            sSQL = sSQL.Replace("[CONDITION]", sTemp);
            DataSet dsGrid;

            if (slParam.Count == 0)
            {
                dsGrid = ClientUtils.ExecuteSQL(sSQL);
            }
            else
            {
                object[][] Params = new object[slParam.Count][];
                for (int i = 0; i < slParam.Count; i++)
                {
                    Params[i] = slParam[i];
                }
                dsGrid = ClientUtils.ExecuteSQL(sSQL, Params);
            }

            bsGrid.DataSource = dsGrid;
            bsGrid.DataMember = dsGrid.Tables[0].ToString();
            dgvRC.DataSource = dsGrid;
            dgvRC.DataMember = dsGrid.Tables[0].ToString();
            dgvRC.VirtualMode = false;
            dgvRC.ColumnWidthChanged -= dataGridView1_ColumnWidthChanged;

            foreach (DataGridViewColumn column in dgvRC.Columns)
            {
                if (column.HeaderText == "PROCESS_GROUP")
                {
                    column.Visible = false;
                }

                column.HeaderText = SajetCommon.SetLanguage(column.HeaderText);
            }

            foreach (DataGridViewRow row in dgvRC.Rows)
            {
                row.Cells["RC_STATUS"].Value = SajetCommon.SetLanguage(row.Cells["RC_STATUS"].Value.ToString());
                row.Cells["SHEET_NAME"].Value = SajetCommon.SetLanguage(row.Cells["SHEET_NAME"].Value.ToString());
            }

            for (int i = 0; i < dsGrid.Tables[0].Columns.Count; i++)
            {
                if (i < programInfo.slWidth.Count)
                {
                    dgvRC.Columns[i].Width = programInfo.slWidth[i];
                }
            }

            dgvRC.ColumnWidthChanged += new DataGridViewColumnEventHandler(dataGridView1_ColumnWidthChanged);
            dsGrid.Dispose();
        }

        private void BtnExecute_Click(object sender, EventArgs e)
        {
            if (dgvRC.CurrentRow == null)
            {
                if (!string.IsNullOrWhiteSpace(txtData.Text))
                {
                    QueryDate();

                    return;
                }

                /*
                if (!string.IsNullOrWhiteSpace(txtData.Text) ||
                    !string.IsNullOrWhiteSpace(txtWo.Text))
                {
                    QueryDate();

                    if (dgvRC.Rows.Count != 1)
                    {
                        return;
                    }

                    dgvRC.CurrentCell = dgvRC.Rows[0].Cells[0];
                }
                //*/
            }

            if (dgvRC.CurrentRow == null) return;

            if (!CheckEmp()) return;

            this.GetEMPName();

            string sRC;

            //工單號
            string work_order = dgvRC.CurrentRow.Cells["WORK_ORDER"].Value.ToString();
            //获取制程名
            string sProcess = dgvRC.CurrentRow.Cells["PROCESS_NAME"].Value.ToString();

            string group = "Group";
            DataSet dsTemp2;
            DataSet hold;

            List<string> RCSets = new List<string>();
            string SQL;

            switch (tsType.SelectedIndex)
            {
                case 0:
                    sRC = dgvRC.CurrentRow.Cells["RC_NO"].Value.ToString();
                    //添加hold判断
                    SQL = "select t.current_status from sajet.g_rc_status t where t.rc_no='" + sRC + "'";
                    hold = ClientUtils.ExecuteSQL(SQL);
                    SQL = hold.Tables[0].Rows[0][0].ToString();

                    if (SQL == "2")
                    {
                        string sMsg = SajetCommon.SetLanguage("Has been HOLD, can not be executed");
                        SajetCommon.Show_Message(sMsg, 1);
                        return;
                    }

                    break;
                default:
                    sRC = dgvRC.CurrentRow.Cells["SERIAL_NUMBER"].Value.ToString();
                    //添加hold判断
                    SQL = "select t.current_status from sajet.G_SN_STATUS t where t.SERIAL_NUMBER='" + sRC + "'";
                    hold = ClientUtils.ExecuteSQL(SQL);
                    SQL = hold.Tables[0].Rows[0][0].ToString();

                    if (SQL == "2")
                    {
                        string sMsg = SajetCommon.SetLanguage("Has been HOLD, can not be executed");
                        SajetCommon.Show_Message(sMsg, 1);
                        return;
                    }

                    break;
            }

            if (CheckEmp())
            {
                this.GetEMPName();
                txtSN.Text = string.Empty;
                txtSN.Focus();
            }

            bool ReportByWO = CbByWorkOrder.CheckState == CheckState.Checked;
            if (ReportByWO)
            {
                SQL = $@"
SELECT
    rc_no
FROM
    sajet.g_rc_status
WHERE
    ( ( current_status = 0
        AND sheet_name = 'RC Input' )
      OR ( current_status = 1
           AND sheet_name = 'RC Output' ) )
    AND work_order = :work_order
    AND process_id = (
        SELECT
            process_id
        FROM
            sajet.sys_process
        WHERE
            process_name = :process
            AND ROWNUM <= 1
    )
";
                var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.Number, "work_order", work_order },
                    new object[] { ParameterDirection.Input, OracleType.Number, "process", sProcess },
                };

                dsTemp2 = ClientUtils.ExecuteSQL(SQL, p.ToArray());

                foreach (DataRow row in dsTemp2.Tables[0].Rows)
                {
                    RCSets.Add(row["rc_no"].ToString());
                }

                if (RCSets.Count <= 0)
                {
                    string message = SajetCommon.SetLanguage("Do not support job-report-by-WorkOrder module");

                    SajetCommon.Show_Message(message, 1);

                    return;
                }
            }
            else
            {
                RCSets.Add(sRC);
            }

            foreach (string RC in RCSets)
            {
                #region 2016.7.4 By Jason
                if (iLineID > 0)
                {
                    SQL = " SELECT COUNT(*) AS COUNTS "
                          + "   FROM SAJET.G_RC_STATUS "
                          + "  WHERE RC_NO = '" + RC + "' "
                          + "    AND PDLINE_ID = " + iLineID;
                    dsTemp2 = ClientUtils.ExecuteSQL(SQL);

                    if (dsTemp2.Tables[0].Rows[0]["COUNTS"].ToString() == "0")
                    {
                        txtSN.Text = "";
                        dgvRC.DataSource = null;

                        string sMsg = SajetCommon.SetLanguage("The line does not have permission");
                        SajetCommon.Show_Message(sMsg, 1);
                        return;
                    }
                }
                #endregion// 2016.7.4 End

                string gStatus = "";

                //如果是群组
                if (sProcess == group)
                {
                    DataSet dsTemp;
                    string sSQL = "select sheet_phase,process_type,status from sajet.g_rc_process_group where rc_no='" + RC + "'";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);

                    if (dsTemp != null && dsTemp.Tables[0].Rows.Count > 0)
                    {
                        string SHEET_PHASE = dsTemp.Tables[0].Rows[0][0].ToString();
                        sGroup = dsTemp.Tables[0].Rows[0][1].ToString();
                        gStatus = dsTemp.Tables[0].Rows[0][2].ToString();
                    }

                    if (gStatus.ToLower() == "N/A".ToLower())
                    {
                        showGroupProcess show = new showGroupProcess(RC);

                        if (show.ShowDialog() != DialogResult.OK)
                        {
                            return;
                        }
                    }
                    else
                    {
                        showGroupProcess show = new showGroupProcess(RC);

                        if (show.ShowDialog() != DialogResult.OK)
                        {
                            return;
                        }
                    }
                }
            }

            CheckInput(RCSets: RCSets, RC: sRC);

            QueryDate();

            // 1.3.17003.22 做完每個動作都要重新確認員工號
            if (txtEmp.Text != ClientUtils.fLoginUser)
            {
                DisableFrom();
                txtEmp.Enabled = true;
                txtEmp.Text = "";
                label7.Text = "N/A";
            }
            txtEmp.SelectAll();
            txtEmp.Focus();

            int iIndex;

            switch (tsType.SelectedIndex)
            {
                case 0:
                    iIndex = bsGrid.Find("RC_NO", sRC);
                    break;
                default:
                    iIndex = bsGrid.Find("SERIAL_NUMBER", sRC);
                    break;
            }

            if (iIndex > -1 && dgvRC.Rows.Count > 0)
            {
                dgvRC.Rows[iIndex].Selected = true;
                dgvRC.CurrentCell = dgvRC.Rows[iIndex].Cells[0];
            }
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            if (CheckEmp())
            {
                txtData.Text = txtData.Text.Trim().ToUpper();

                txtWo.Text = txtWo.Text.Trim().ToUpper();

                this.GetEMPName();
                txtSN.Text = string.Empty;
                txtSN.Focus();

                this.QueryDate();

                ShowTravel();
            }
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            string s = "";

            for (int i = 0; i < dgvRC.Columns.Count; i++)
            {
                s += dgvRC.Columns[i].Width + ",";
            }

            SajetInifile sini = new SajetInifile();

            sini.WriteIniFile(sIniFile, "RC Travel", "表格欄寬", s);
            sini.Dispose();
        }

        private void CbByWorkOrder_CheckStateChanged(object sender, EventArgs e)
        {
            ShowTravel();

            if (CbByWorkOrder.CheckState == CheckState.Checked)
            {
                CbByRoute.CheckStateChanged -= CbByRoute_CheckStateChanged;

                CbByRoute.CheckState = CheckState.Unchecked;

                CbByRoute.CheckStateChanged += CbByRoute_CheckStateChanged;

                IsExecuteRegion = false;

                SwitchRegion(IsExecute: IsExecuteRegion);
            }
            else
            {
                if (CbByRoute.CheckState == CheckState.Unchecked)
                {
                    dgvRC.DataSource = null;
                }

                IsExecuteRegion = true;

                SwitchRegion(IsExecute: IsExecuteRegion);

                txtSN.Focus();
            }

            txtSN.Text = "";
        }

        private void CbByRoute_CheckStateChanged(object sender, EventArgs e)
        {
            ShowTravel();

            txtSN.Text = "";

            if (CbByRoute.CheckState == CheckState.Checked)
            {
                CbByWorkOrder.CheckStateChanged -= CbByWorkOrder_CheckStateChanged;

                CbByWorkOrder.CheckState = CheckState.Unchecked;

                CbByWorkOrder.CheckStateChanged += CbByWorkOrder_CheckStateChanged;

                IsExecuteRegion = false;

                SwitchRegion(IsExecute: IsExecuteRegion);
            }
            else
            {
                if (CbByWorkOrder.CheckState == CheckState.Unchecked)
                {
                    dgvRC.DataSource = null;
                }

                IsExecuteRegion = true;

                SwitchRegion(IsExecute: IsExecuteRegion);

                txtSN.Focus();
            }
        }

        private bool ReloadStage()
        {
            combStage.Items.Clear();

            int dropDownHeight = 0;

            string sSQL = @"
SELECT
    stage_name
FROM
    sajet.sys_stage
WHERE
    enabled = 'Y'
    AND stage_id IN (
        SELECT DISTINCT
            ( stage_id )
        FROM
            sajet.sys_process
        WHERE
            process_id IN (
                SELECT
                    a.process_id
                FROM
                    sajet.sys_role_op_privilege   a,
                    sajet.sys_emp                 b,
                    sajet.sys_role_emp            c
                WHERE
                    a.role_id = c.role_id
                    AND b.emp_id = c.emp_id
                    AND b.emp_no = :emp
                UNION
                SELECT
                    a.process_id
                FROM
                    sajet.sys_emp_process_privilege   a,
                    sajet.sys_emp                     b
                WHERE
                    a.emp_id = b.emp_id
                    AND b.emp_no = :emp
            )
    )
ORDER BY
    stage_id
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.Number, "emp", txtEmp.Text },
            };

            DataSet ds = ClientUtils.ExecuteSQL(sSQL, p.ToArray());

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                combStage.Items.Add(dr[0].ToString());
                dropDownHeight++;
            }

            combStage.DropDownHeight = 20 + (dropDownHeight) * 20;

            if (combStage.Items.Count > 0)
            {
                combStage.SelectedIndex = 0;
                return true;
            }
            else
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("User does not have permission to use WIP module"), 0);

                combProcess.Items.Clear();
                combProcess.DropDownHeight = 20;

                return false;
            }
        }

        private void DisableFrom()
        {
            BtnReset.Enabled = false;
            BtnEnter.Enabled = BtnTravel.Enabled = BtnClear.Enabled = false;
            txtSN.Enabled = false;

            CbByWorkOrder.Enabled = false;
            CbByWorkOrder.Checked = false;
            //CbByWorkOrder.Visible = false;

            CbByRoute.Enabled = false;
            CbByRoute.Checked = false;
            //CbByRoute.Visible = false;

            CkbStage.Checked = CkbStage.Enabled = false;
            CkbProcess.Checked = CkbProcess.Enabled = false;
            tsType.Enabled = false;
            txtData.Enabled = false;
            txtWo.Enabled = false;
            BtnQuery.Enabled = false;
            BtnExecute.Enabled = false;
            dgvRC.DataSource = null;

            txtData.Text = "";
            txtWo.Text = "";
            txtSN.Text = "";
        }

        private void EnableForm()
        {
            BtnEnter.Enabled = BtnTravel.Enabled = BtnClear.Enabled = true;

            txtSN.Enabled = true;
            txtSN.Focus();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            DisableFrom();
            ShowTravel();
            txtEmp.Enabled = true;
            txtEmp.Text = "";
            label7.Text = "N/A";
            txtEmp.Focus();
        }

        private bool IsUsingT4T6(string rc_no)
        {
            string s = @"
SELECT
    B.OPTION2,
    B.OPTION3
FROM
    SAJET.G_RC_STATUS    A,
    SAJET.SYS_PROCESS_OPTION   B
WHERE
    RC_NO = :RC_NO
    AND A.PROCESS_ID = B.PROCESS_ID
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.Number, "RC_NO", rc_no }
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d != null &&
                d.Tables[0].Rows.Count > 0)
            {
                return d.Tables[0].Rows[0]["OPTION2"].ToString() == "Y"
                    || d.Tables[0].Rows[0]["OPTION3"].ToString() == "Y";
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 是不是 10B 的第一站（I010 領料）
        /// </summary>
        /// <param name="rc_no">流程卡號</param>
        /// <returns></returns>
        private bool IsFirstProcessOf10B(string rc_no)
        {
            // ID: 100156 Name:I010領料
            string s = @"
SELECT
    RC_NO
FROM
    SAJET.G_RC_STATUS
WHERE
    RC_NO = :RC_NO
    AND RC_NO LIKE '10B%'
    AND PROCESS_ID = 100156
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.Number, "RC_NO", rc_no },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            return d != null && d.Tables[0].Rows.Count > 0;
        }

        /// <summary>
        /// 檢查模組的權限
        /// </summary>
        private void Check_Privilege()
        {
            string SQL = $@"
SELECT EMP_ID
FROM SAJET.SYS_EMP
WHERE EMP_NO = '{txtEmp.Text}'
";
            DataSet ds = ClientUtils.ExecuteSQL(SQL);
            string EMP_ID = "";
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                EMP_ID = ds.Tables[0].Rows[0]["EMP_ID"].ToString();
            }
            string sPrivilege = ClientUtils.GetPrivilege(EMP_ID, "RCOutput_WO", ClientUtils.fProgramName).ToString();

            // 能不能使用工單報工模組
            bool access_WIP_WO = SajetCommon.CheckEnabled("WIP_WO", sPrivilege);
            CbByWorkOrder.Enabled = access_WIP_WO;
            //CbByWorkOrder.Visible = access_WIP_WO;
            CbByWorkOrder.Checked = false;

            // 能不能使用途程報工模組
            sPrivilege = ClientUtils.GetPrivilege(EMP_ID, "RCOutput_Route", ClientUtils.fProgramName).ToString();
            bool access_WIP_Route = SajetCommon.CheckEnabled("WIP_Route", sPrivilege);
            CbByRoute.Enabled = access_WIP_Route;
            //CbByRoute.Visible = access_WIP_Route;
            CbByRoute.Checked = false;

            AllowToQuery = access_WIP_WO | access_WIP_Route;
        }

        /// <summary>
        /// 製程的配置為必須執行投入與產出，才能使用工單報工。
        /// </summary>
        /// <param name="RC"></param>
        /// <returns></returns>
        private bool CheckProcessSheet(string RC)
        {
            string s = @"
SELECT
    PROCESS_ID
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
AND SHEET_NAME = 'RC Input'
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.Number, "RC_NO", RC }
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            // 只要執行產出的製程會查不到設定
            if (d == null)
            {
                return true;
            }
            else
            {
                return d.Tables[0].Rows.Count == 0;
            }
        }

        /// <summary>
        /// 使用流程卡查詢，找同工單同製程有沒有待產出的流程卡，可以執行途程報工
        /// </summary>
        /// <param name="targetRuncard"></param>
        /// <returns></returns>
        private bool FindRuncardsToReportByRoute(string targetRuncard)
        {
            string s = @"
SELECT
    a.rc_no,
    a.current_qty
FROM
    sajet.g_rc_status            a,
    sajet.sys_rc_process_sheet   b,
    (
        SELECT
            work_order,
            route_id,
            process_id
        FROM
            sajet.g_rc_status
        WHERE
            rc_no = :RC_NO
    ) c
WHERE
    a.process_id = b.process_id
    AND a.sheet_name = b.sheet_name
    AND b.sheet_phase = 'I'
    AND a.work_order = c.work_order
    AND a.route_id = c.route_id
    AND a.process_id = c.process_id
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.Number, "RC_NO", targetRuncard }
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            return d != null && d.Tables[0].Rows.Count > 0;
        }

        /// <summary>
        /// 顯示指定流程卡的生產歷程
        /// </summary>
        /// <param name="SelectedRuncard"></param>
        private void ShowTravel(string SelectedRuncard = "")
        {
            string process = "";

            string sheet = "";

            string current_status = "";

            Dictionary<string, string> status_text = new Dictionary<string, string>
            {
                ["Queue"] = SajetCommon.SetLanguage("Queue"),
                ["Running"] = SajetCommon.SetLanguage("Running"),
                ["Hold"] = SajetCommon.SetLanguage("Hold"),
                ["Passed"] = SajetCommon.SetLanguage("Passed"),
            };

            string s = $@"
SELECT
    D.PROCESS_NAME ""PROCESS NAME""
   ,NVL(A.WIP_OUT_GOOD_QTY, A.CURRENT_QTY ) ""RC QTY""
   ,A.CURRENT_STATUS ""RC STATUS""
   ,E.EMP_NAME
   ,TO_CHAR(A.WIP_IN_TIME, 'YYYY/ MM/ DD HH24: MI: SS') ""WIP_IN_TIME""
   ,TO_CHAR(A.OUT_PROCESS_TIME, 'YYYY/ MM/ DD HH24: MI: SS') ""WIP_OUT_TIME""
   ,A.SHEET_NAME
FROM
    (
        SELECT
            WORK_ORDER
           ,RC_NO
           ,WIP_OUT_GOOD_QTY
           ,CURRENT_QTY
           ,WIP_OUT_SCRAP_QTY
           ,CASE CURRENT_STATUS
                WHEN 0   THEN
                    '{status_text["Queue"]}'
                WHEN 1   THEN
                    '{status_text["Running"]}'
                WHEN 2   THEN
                    '{status_text["Hold"]}'
                ELSE
                    SAJET.SJ_RC_STATUS(CURRENT_STATUS)
            END ""CURRENT_STATUS""
           ,WIP_IN_TIME
           ,NULL OUT_PROCESS_TIME
           ,UPDATE_TIME
           ,PROCESS_ID
           ,PART_ID
           ,SHEET_NAME
           ,0 ""SEQ""
           ,UPDATE_USERID
        FROM
            SAJET.G_RC_STATUS
        WHERE
            RC_NO = :RC_NO
            AND CURRENT_STATUS IN (
                0,
                1,
                2,
                6,
                7,
                8,
                10,
                11,
                12
            )
        UNION
        SELECT
            WORK_ORDER
           ,RC_NO
           ,WIP_OUT_GOOD_QTY
           ,CURRENT_QTY
           ,WIP_OUT_SCRAP_QTY
           ,'{status_text["Passed"]}' ""CURRENT_STATUS""
           ,WIP_IN_TIME
           ,OUT_PROCESS_TIME
           ,UPDATE_TIME
           ,PROCESS_ID
           ,PART_ID
           ,SHEET_NAME
           ,1 ""SEQ""
           ,UPDATE_USERID
        FROM
            SAJET.G_RC_TRAVEL
        WHERE
            RC_NO = :RC_NO
            AND CURRENT_STATUS IN (
                0,
                1,
                2,
                6,
                7,
                8,
                9,
                10,
                11,
                12
            )
    ) A
   ,SAJET.G_WO_BASE B
   ,SAJET.SYS_PART C
   ,SAJET.SYS_PROCESS D
   ,SAJET.SYS_EMP E
WHERE
    A.WORK_ORDER = B.WORK_ORDER
AND A.PART_ID = C.PART_ID
AND A.PROCESS_ID = D.PROCESS_ID (+)
AND A.UPDATE_USERID = E.EMP_ID
AND A.RC_NO = :RC_NO
ORDER BY
    A.SEQ DESC
   ,A.UPDATE_TIME ASC
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.Number, "RC_NO", SelectedRuncard }
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            // 顯示生產歷程
            DgvTravel.DataSource = d;

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                DgvTravel.DataMember = d.Tables[0].ToString();

                foreach (DataGridViewColumn column in DgvTravel.Columns)
                {
                    column.HeaderText = SajetCommon.SetLanguage(column.HeaderText);
                }

                DgvTravel.Columns["SHEET_NAME"].Visible = false;

                DgvTravel.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                DgvTravel.CurrentCell = DgvTravel[DgvTravel.FirstDisplayedCell.ColumnIndex, DgvTravel.Rows.Count - 1];
            }
            else
            if (!string.IsNullOrWhiteSpace(SelectedRuncard))
            {
                string message = SajetCommon.SetLanguage(MessageEnum.RuncardNotFound.ToString());

                SajetCommon.Show_Message(message, 3);
            }

            s = @"
SELECT
    current_status,
    process_name,
    sheet_name
FROM
    sajet.g_rc_status   a,
    sajet.sys_process   b
WHERE
    rc_no = :rc_no
    AND a.process_id = b.process_id (+)
";
            d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                current_status = d.Tables[0].Rows[0]["current_status"].ToString();

                if (current_status == "9")
                {
                    process = SajetCommon.SetLanguage("Route End");

                    sheet = "";
                }
                else
                {
                    process = d.Tables[0].Rows[0]["process_name"].ToString();

                    sheet = d.Tables[0].Rows[0]["sheet_name"].ToString();
                }
            }
            else
            {
                SelectedRuncard = "";

                process = "";

                sheet = "";
            }

            LbRuncard.Text
                = SajetCommon.SetLanguage("Runcard")
                + SajetCommon.SetLanguage(":")
                + Environment.NewLine
                + SelectedRuncard;

            LbProcess.Text
                = SajetCommon.SetLanguage("PROCESS NAME")
                + SajetCommon.SetLanguage(":")
                + Environment.NewLine
                + process;

            LbSheet.Text
                = SajetCommon.SetLanguage("SHEET_NAME")
                + SajetCommon.SetLanguage(":")
                + Environment.NewLine
                + SajetCommon.SetLanguage(sheet);

            if (current_status == "9")
            {
                LbProcess.ForeColor = Color.Red;
            }
            else
            {
                LbProcess.ForeColor = Color.Black;
            }
        }

        /// <summary>
        /// 切換啟用區域
        /// </summary>
        /// <param name="IsExecute"></param>
        private void SwitchRegion(bool IsExecute)
        {
            #region 查詢區域

            CkbStage.Enabled = tsType.Enabled = txtWo.Enabled = txtData.Enabled = BtnQuery.Enabled = BtnExecute.Enabled = !IsExecute;

            if (IsExecute)
            {
                CkbStage.Checked = false;
            }

            #endregion

            #region 執行區域

            txtSN.Enabled = BtnEnter.Enabled = BtnTravel.Enabled = BtnClear.Enabled = IsExecute;

            #endregion

            txtSN.Text = "";
            txtData.Text = "";
            txtWo.Text = "";

            dgvRC.DataSource = null;
        }
    }
}