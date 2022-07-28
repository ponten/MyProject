using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Data.OracleClient;
using System.Text.RegularExpressions;//導入命名空間(正則表達式)
using RCOutput_Route.Models;

namespace RCOutput_Route
{
    public partial class fMain : Form
    {
        string m_nParam;
        public string g_SystemType; // Y: Lot Control, N:Piece
        public bool btxtGood = false; // 判定良品數是否為手動輸入
        string EMP_ID = string.Empty;

        struct TProgramInfo
        {
            public string sProgram;
            public string sFunction;
            public Dictionary<string, string> sSQL;
            public Dictionary<string, int> iInputVisible; // 顯示到第幾個欄位;
            public Dictionary<string, List<int>> iInputField; // 那些欄位可以維護(逗號分隔)
            public Dictionary<string, string> sOption; // 其他設定
            public DataSet dsRC;
            public DataSet dsSNParam;
            public int[] iSNInput;
            public TextBox txtGood;
            public TextBox txtScrap;
            public Dictionary<string, int> slDefect;
        }
        TProgramInfo programInfo;

        public struct TRCroute
        {
            public string sNode_Id;
            public string sNext_Node;
            public string sNext_Process;
            public string sSheet_Name;
            public string sNode_type;
            public string g_sRouteID;
            public string g_sProcessID;
            public string sLink_Name;
        }
        TRCroute rcRoute = new TRCroute();

        DateTime dSetTime;   // RCTravel 顯示的時間在sys_base的Closing Date設定
        bool bSetTime = false;  // 判定是否有設定關帳時間

        public DataSet RCSource;
        public List<RC_DEFECT> RC_DEFECTs = new List<RC_DEFECT>();

        public fMain()
        {
            InitializeComponent();
        }

        public fMain(string nParam, string strSN, string sEmp, string sTime)
        {
            InitializeComponent();
            m_nParam = nParam; // RC Output
            this.Text = m_nParam;
            txtInput.Text = strSN; // RC_NO
            tsEmp.Text = sEmp;
            txtInput.Visible = false;
            cmbParam.Visible = false;
            toolStripSeparator2.Visible = false;
            statusStripMessage.Visible = false;

            try
            {
                dSetTime = DateTime.ParseExact(sTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                bSetTime = true;
            }
            catch (Exception)
            {
                dSetTime = DateTime.Now;
            }

            dgvRC.CellClick += DgvRC_CellClick;

            BtnSelectAll.Click += BtnSelectAll_Click;

            BtnResetAll.Click += BtnResetAll_Click;
        }

        private void FMain_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);

            if (!string.IsNullOrEmpty(txtInput.Text) && ShowData())
            {
                EMP_ID = GetEmpID();

                RCsDataGrid();

                ClickRuncard(txtInput.Text);

                GetRouteProcess();
            }
        }

        private void FMain_Shown(object sender, EventArgs e)
        {
            splitContainerMain.SplitterDistance = 400;

            // 強調顯示 舊編(OPTION2) 藍圖(OPTION4) 所在製程
            LbProcessName.Font = new Font("微軟正黑體", 30, FontStyle.Bold);
            LbBluePrint.Font = new Font("微軟正黑體", 20, FontStyle.Bold);
            LbFormerNO.Font = new Font("微軟正黑體", 20, FontStyle.Bold);
        }

        private void BtnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvRC.Rows)
            {
                row.Cells[SelectRuncards.Name].Value = true;
            }
        }

        private void BtnResetAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvRC.Rows)
            {
                row.Cells[SelectRuncards.Name].Value = false;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            object[][] Params;

            int WIPCount = (from p in dgvProcess.Rows.Cast<DataGridViewRow>()
                            where p.Cells["CHECK"].Value.ToString() == "1"
                            select p).First().Index;

            string message
                = SajetCommon.SetLanguage("Job-reporting processes does NOT include current selected process");

            if (SajetCommon.Show_Message(message, 2) != DialogResult.Yes)
            {
                return;
            }

            for (int i = 0; i < WIPCount; i++)
            {
                if (!SetNextProcess())
                    return;

                //重新建立已選取 RC 的清單
                RC_DEFECTs = CreateSelectedRCsList(CreateDefectList());

                foreach (var RC in RC_DEFECTs)
                {
                    var row = dgvRC.Rows.Cast<DataGridViewRow>()
                        .FirstOrDefault(m => m.Cells["RC_NO"].Value.ToString() == RC.RC_NO);

                    #region 判斷是否重工
                    string sTSTATUS = string.Empty;
                    try
                    {
                        string sSQL = @"
SELECT RC_NO 
FROM SAJET.G_RC_TRAVEL 
WHERE RC_NO = :RC_NO 
AND PROCESS_ID = :PROCESS_ID ";
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

                    var RcInfo = GetRcNoInfo(RC.RC_NO);

                    Params = new object[18][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TEMP", tsEmp.Text };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TRC", RC.RC_NO };
                    Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TITEM", "" };
                    Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TDEFECT", "" };
                    Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSN", "" };
                    Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TMEMO", tsMemo.Text };
                    Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TGOOD", RC.CURRENT_QTY };
                    Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSCRAP", 0 };
                    Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TNEXTPROCESS", rcRoute.sNext_Process };
                    Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TNEXTNODE", rcRoute.sNext_Node };
                    Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TNEXTSHEET", rcRoute.sSheet_Name };
                    Params[11] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TBONUS", 0 };
                    Params[12] = new object[] { ParameterDirection.Input, OracleType.DateTime, "TNOW", dSetTime };
                    Params[13] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSTATUS", sTSTATUS };
                    Params[14] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TMACHINE", "" };
                    Params[15] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TKEYPART", "" };
                    Params[16] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TWORKHOUR", 1 };
                    Params[17] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                    DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_RC_OUTPUT", Params); // V12
                    string sMsg = ds.Tables[0].Rows[0]["TRES"].ToString();

                    if (sMsg != "OK")
                    {
                        tsMsg.ForeColor = Color.Red;
                        SajetCommon.Show_Message(
                            SajetCommon.SetLanguage(sMsg) +
                            Environment.NewLine +
                            SajetCommon.SetLanguage("RunCard") + ": " + RC.RC_NO, 0);

                        tsMsg.Text =
                            SajetCommon.SetLanguage(ds.Tables[0].Rows[0]["TRES"].ToString() + " " +
                            SajetCommon.SetLanguage("RunCard") + ": " + RC.RC_NO, 1);
                        return;
                    }
                    else
                    {
                        RecordMachine(RcInfo);

                        tsMsg.ForeColor = Color.Blue;
                        tsMsg.Text =
                            SajetCommon.SetLanguage(ds.Tables[0].Rows[0]["TRES"].ToString() + " " +
                            SajetCommon.SetLanguage("RunCard") + ": " + RC.RC_NO, 1);
                    }
                }
            }

            DialogResult = DialogResult.Yes;

            this.Close();
        }

        private void DgvRC_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != SelectRuncards.Index)
            {
                return;
            }

            bool selection = (bool)dgvRC.Rows[e.RowIndex].Cells[SelectRuncards.Name].EditedFormattedValue;

            dgvRC.Rows[e.RowIndex].Cells[SelectRuncards.Name].Value = !selection;
        }

        private void dgvProcess_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvProcess.IsCurrentCellDirty)
            {
                dgvProcess.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvProcess_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int columnIndex = 0;
            if (e.RowIndex > -1 && e.ColumnIndex == columnIndex)
            {
                bool isChecked = dgvProcess.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "1";
                if (isChecked)
                {
                    foreach (DataGridViewRow row in dgvProcess.Rows)
                    {
                        if (row.Index != e.RowIndex)
                        {
                            row.Cells[columnIndex].Value = !isChecked;
                        }
                    }
                }
            }
        }

        private void ClearData()
        {
            LbWorkOrder.Text = "--";
            LbPart.Text = "--";
            LbVersion.Text = "--";
            LbProductName.Text = "--";
            LbSpecification.Text = "--";
            LbRouteName.Text = "--";
            LbRemark.Text = "--";
            LbProcessName.Text = "--";
            LbFormerNO.Text = "--";
            LbBluePrint.Text = "--";

            dgvRC.Rows.Clear();

            dgvProcess.Rows.Clear();
        }

        private bool ShowData()
        {
            string sSQL = @"
SELECT
    f.work_order,
    f.remark,
    a.route_id,
    a.process_id,
    a.node_id,
    a.part_id,
    b.part_no,
    b.version,
    b.option2 former_no,
    b.option4 blueprint,
    b.spec1 product_name,
    b.spec2 specification,
    c.route_name,
    d.process_name
FROM
    sajet.g_rc_status    a,
    sajet.sys_part       b,
    sajet.sys_rc_route   c,
    sajet.sys_process    d,
    sajet.g_wo_base      f
WHERE
    a.part_id = b.part_id
    AND a.route_id = c.route_id
    AND a.process_id = d.process_id
    AND a.work_order = f.work_order
    AND a.rc_no = :rc_no
";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "rc_no", txtInput.Text };

            var d = ClientUtils.ExecuteSQL(sSQL, Params);

            programInfo.dsRC = d;

            if (d == null || d.Tables[0].Rows.Count <= 0)
            {
                ClearData();

                return false;
            }
            else
            {
                LbWorkOrder.Text = d.Tables[0].Rows[0]["WORK_ORDER"].ToString();
                LbPart.Text = d.Tables[0].Rows[0]["PART_NO"].ToString();
                LbVersion.Text = d.Tables[0].Rows[0]["VERSION"].ToString();
                LbProductName.Text = d.Tables[0].Rows[0]["PRODUCT_NAME"].ToString();
                LbSpecification.Text = d.Tables[0].Rows[0]["SPECIFICATION"].ToString();
                LbRouteName.Text = d.Tables[0].Rows[0]["ROUTE_NAME"].ToString();
                LbRemark.Text = d.Tables[0].Rows[0]["REMARK"].ToString();
                LbProcessName.Text = d.Tables[0].Rows[0]["PROCESS_NAME"].ToString();
                LbFormerNO.Text = SajetCommon.SetLanguage("Former NO") + ": " + d.Tables[0].Rows[0]["FORMER_NO"].ToString();
                LbBluePrint.Text = SajetCommon.SetLanguage("Blueprint") + ": " + d.Tables[0].Rows[0]["BLUEPRINT"].ToString();

                return true;
            }
        }

        /// <summary>
        /// 取得流程卡的途程製程
        /// </summary>
        private void GetRouteProcess()
        {
            string ROUTE_ID = programInfo.dsRC.Tables[0].Rows[0]["ROUTE_ID"].ToString();
            string PROCESS_ID = programInfo.dsRC.Tables[0].Rows[0]["PROCESS_ID"].ToString();
            string NODE_ID = programInfo.dsRC.Tables[0].Rows[0]["NODE_ID"].ToString();
            string sYes = SajetCommon.SetLanguage("Yes");
            string sNo = SajetCommon.SetLanguage("No");
            string SQL = @"
/* 遞迴找生產途程前後順序 */
WITH route_detail AS (
    SELECT
        ROWNUM idx,
        node_content,
        node_id,
        next_node_id
    FROM
        sajet.sys_rc_route_detail
    WHERE
        route_id = :route_id
    START WITH
        node_id = :node_id
    CONNECT BY
        PRIOR next_node_id = node_id
),
 /* 條列製程特殊設定 */
 process_detail AS (
    SELECT
        0 ""CHECK"",
        b.process_id,
        b.process_name,
        b.operate_id,
        a.node_id,
        a.next_node_id,
        nvl(c.option1, 'N') ""OPTION1"",
        nvl(c.option2, 'N') ""OPTION2"",
        nvl(c.option3, 'N') ""OPTION3"",
        idx
    FROM
        route_detail               a,
        sajet.sys_process          b,
        sajet.sys_process_option   c
    WHERE
        a.node_content = to_char(b.process_id)
        AND b.process_id = c.process_id (+)
        -- AND node_id <> :node_id
    ORDER BY
        a.idx
),
 /* 含有特殊設定的製程作為途程報工可以執行的最後一站，否則列出生產途程中所有的製程 */
 last_node_idx AS (
    SELECT
        idx
    FROM
        process_detail
    WHERE
        ( option1 = 'Y'
          OR option2 = 'Y'
          OR option3 = 'Y' )
        AND ROWNUM = 1
    UNION
    SELECT
        MAX(idx) idx
    FROM
        process_detail
)
SELECT
    a.""CHECK""      ""CHECK"",
    a.process_id     process_id,
    a.process_name   process_name,
    a.operate_id     operate_id,
    a.node_id        node_id,
    a.next_node_id   next_node_id,
    a.""OPTION1""    ""OPTION1"",
    a.""OPTION2""    ""OPTION2"",
    a.""OPTION3""    ""OPTION3""
FROM
    process_detail a,
    (
        SELECT
            MIN(idx) idx
        FROM
            last_node_idx
    ) b
WHERE
    a.idx <= b.idx
ORDER BY
    a.idx
";
            object[][] param = new object[2][];
            param[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ROUTE_ID", ROUTE_ID };
            param[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NODE_ID", NODE_ID };

            DataSet ds = ClientUtils.ExecuteSQL(SQL, param);

            dgvProcess.DataSource = ds;
            dgvProcess.DataMember = ds.Tables[0].ToString();

            #region 包裝站、或著 operate type 不是 "input" 的製程，都不顯示

            bool removeRow = false;
            List<DataGridViewRow> selectedRows = new List<DataGridViewRow>();

            foreach (DataGridViewRow row in dgvProcess.Rows)
            {
                if (row.Cells["PROCESS_ID"].Value.ToString() == PROCESS_ID)
                {
                    row.Cells["CHECK"].Value = true;
                }

                if (row.Cells["OPERATE_ID"].Value.ToString() != "5")
                {
                    removeRow = true;
                }

                if (removeRow)
                {
                    selectedRows.Add(row);
                }
            }

            foreach (DataGridViewRow row in selectedRows)
            {
                dgvProcess.Rows.Remove(row);
            }

            #endregion
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

                string sSQL = " SELECT * FROM SAJET.G_RC_STATUS WHERE RC_NO ='" + txtInput.Text + "' ";
                DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);

                if (dsTemp.Tables[0].Rows.Count > 0)
                {
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

                if (rcRoute.sNode_type == "1" ||
                    rcRoute.sNode_type == "2" ||
                    rcRoute.sNode_type == "3")
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

        public void RCsDataGrid()
        {
            string workOrder = programInfo.dsRC.Tables[0].Rows[0]["WORK_ORDER"].ToString();
            string processID = programInfo.dsRC.Tables[0].Rows[0]["PROCESS_ID"].ToString();
            string routeID = programInfo.dsRC.Tables[0].Rows[0]["ROUTE_ID"].ToString();

            // 待產出的流程卡才可以執行途程報工
            string SQL = $@"
SELECT
    1 ""SELECT""
   ,RC_NO
   ,CURRENT_QTY
FROM
    SAJET.G_RC_STATUS A
   ,SAJET.SYS_RC_PROCESS_SHEET B
WHERE
    A.PROCESS_ID = B.PROCESS_ID
AND A.SHEET_NAME = B.SHEET_NAME
AND B.SHEET_PHASE = 'I'
AND A.WORK_ORDER = '{workOrder}'
AND A.ROUTE_ID     = {routeID}
AND A.PROCESS_ID   = {processID}
ORDER BY 1
";
            RCSource = ClientUtils.ExecuteSQL(SQL);
            dgvRC.DataSource = RCSource;
            dgvRC.DataMember = RCSource.Tables[0].ToString();
        }

        /// <summary>
        /// 勾選指定的流程卡
        /// </summary>
        /// <param name="rc_no"></param>
        private void ClickRuncard(string rc_no)
        {
            dgvRC.Rows.Cast<DataGridViewRow>()
                .FirstOrDefault(x => x.Cells["RC_NO"].Value.ToString() == rc_no)
                .Cells[SelectRuncards.Name].Value = true;
        }

        /// <summary>
        /// 建立已選取 RC 的清單
        /// </summary>
        /// <param name="defects">製程不良清單</param>
        /// <returns></returns>
        private List<RC_DEFECT> CreateSelectedRCsList(List<Defect> defects)
        {
            // 建立已勾選的 RC 的新清單，每個 RC 有自己的不良現象清單
            var temp = (from row in dgvRC.Rows.Cast<DataGridViewRow>()
                        where row.Cells[SelectRuncards.Name].Value.ToString() == "1"
                        select new RC_DEFECT
                        {
                            RC_NO = row.Cells["RC_NO"].Value.ToString(),
                            CURRENT_QTY = int.Parse(row.Cells["CURRENT_QTY"].Value.ToString()),
                            DEFECTS = (from d in defects
                                       select new Defect
                                       {
                                           DEFECT_CODE = d.DEFECT_CODE,
                                           DEFECT_DESC = d.DEFECT_DESC,
                                           QTY = d.QTY
                                       }).ToList()
                        }).ToList();

            // 讓新建立的清單繼承已經填寫的 RC 不良數量
            if (RC_DEFECTs != null && RC_DEFECTs.Count > 0
                && temp != null && temp.Count > 0)
            {
                foreach (var RC in RC_DEFECTs)
                {
                    foreach (var df in RC.DEFECTS)
                    {
                        temp.First(m => m.RC_NO == RC.RC_NO).DEFECTS
                            .First(m => m.DEFECT_CODE == df.DEFECT_CODE)
                            .QTY = df.QTY;
                    }
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
            string SQL = @"
SELECT B.DEFECT_CODE
      ,B.DEFECT_DESC
FROM SAJET.SYS_RC_PROCESS_DEFECT A
    ,SAJET.SYS_DEFECT B
WHERE A.PROCESS_ID = :PROCESS_ID
AND A.DEFECT_ID = B.DEFECT_ID
AND A.ENABLED = 'Y'
AND B.ENABLED = 'Y'
ORDER BY B.DEFECT_CODE
        ,B.DEFECT_DESC
";
            object[][] param = new object[1][];
            param[0] =
                new object[]
                {
                    ParameterDirection.Input,
                    OracleType.VarChar,
                    "PROCESS_ID",
                    programInfo.dsRC.Tables[0].Rows[0]["PROCESS_ID"].ToString()
                };
            DataSet defectTypes = ClientUtils.ExecuteSQL(SQL, param);

            var defects = new List<Defect>();
            foreach (DataRow row in defectTypes.Tables[0].Rows)
            {
                if (!defects.Any(m => m.DEFECT_CODE == row["DEFECT_CODE"].ToString()))
                    defects.Add(
                        new Defect
                        {
                            DEFECT_CODE = row["DEFECT_CODE"].ToString(),
                            DEFECT_DESC = row["DEFECT_DESC"].ToString(),
                            QTY = 0
                        });
            }
            return defects;
        }

        /// <summary>
        /// 取得報工人員 ID
        /// </summary>
        private string GetEmpID()
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
                new object[] { ParameterDirection.Input, OracleType.VarChar, "EMP_NO", tsEmp.Text },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            string EmpID = d.Tables[0].Rows[0]["EMP_ID"].ToString();

            return EmpID;
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
        private void RecordMachine(DataRow RcInfo)
        {
            #region 設定資料狀態，有綁定機台的製程才能在 機台時間補登模組 補資料

            int data_status;

            string s = @"
SELECT
    A.MACHINE_ID
FROM
    SAJET.SYS_RC_PROCESS_MACHINE   A,
    SAJET.SYS_RC_ROUTE_DETAIL      B
WHERE
    TO_CHAR(A.PROCESS_ID) = B.NODE_CONTENT
    AND B.NODE_ID = :NODE_ID
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.Number, "NODE_ID", RcInfo["NODE_ID"].ToString() },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d == null || d.Tables[0].Rows.Count <= 0)
            {
                data_status = 2;
            }
            else
            {
                data_status = 1;
            }

            #endregion

            s = @"
INSERT INTO sajet.g_rc_travel_machine_down (
    rc_no,
    node_id,
    travel_id,
    machine_id,
    start_time,
    end_time,
    reason_id,
    load_qty,
    update_userid,
    update_time,
    work_time_minute,
    work_time_second,
    data_status
) VALUES (
    :rc_no,
    :node_id,
    :travel_id,
    0,
    :now_time,
    :now_time,
    0,
    0,
    :update_userid,
    :now_time,
    0,
    0,
    :data_status
)";
            p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "rc_no", RcInfo["RC_NO"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.Number, "travel_id", RcInfo["TRAVEL_ID"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.Number, "node_id", RcInfo["NODE_ID"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.Number, "data_status", data_status },
                new object[] { ParameterDirection.Input, OracleType.DateTime, "now_time", dSetTime },
                new object[] { ParameterDirection.Input, OracleType.Number, "update_userid", EMP_ID },
            };

            ClientUtils.ExecuteSQL(s, p.ToArray());
        }
    }
}