using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using SajetClass;
using SajetTable;
using System.IO;
using System.Data.OracleClient;
using System.Linq;
using System.Reflection;
using Braincase.GanttChart;
using OtSrv = CWoManager.Services.OtherService;

namespace CWoManager
{
    public partial class fMain : Form
    {
        private MESGridView.Cache memoryCache;
        private DateTimePicker datePickerFrom = new DateTimePicker();
        private DateTimePicker datePickerTo = new DateTimePicker();
        private ToolStripComboBox cbtimeScale = new ToolStripComboBox();
        private Dictionary<string, GRcTravel> m_DicGRcTravel = new Dictionary<string, GRcTravel>(); // key is Work_Order Id


        struct TControlData
        {
            public string sFieldName;
            public TextBox txtControl;
            public ComboBox combControl;
        }
        TControlData[] m_tControlData;
        string sIniFile = Application.StartupPath + Path.DirectorySeparatorChar + "Sajet.Ini";
        List<string> combFilterField = new List<string>();

        public struct TDBInitial
        {
            public string sValue;
            public string sDefault;
            public string sType;
            public List<string> slValue;
        }
        Dictionary<string, TDBInitial> g_DBInitial = new Dictionary<string, TDBInitial>();
        Dictionary<string, string[]> g_MainField = new Dictionary<string, string[]>();

        public fMain()
        {
            InitializeComponent();
            AddControlToToolStrip();
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            g_sProgram = ClientUtils.fProgramName;
            g_sFunction = ClientUtils.fFunctionName;
            g_sOrderField = TableDefine.gsDef_OrderField;
            g_sExeName = ClientUtils.fCurrentProject;
            string sSQL = "SELECT * FROM SAJET.SYS_PROGRAM_FUN_MAINTAIN WHERE PROGRAM = :PROGRAM AND FUN_NAME = :FUN_NAME";
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROGRAM", g_sProgram };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FUN_NAME", "W/O Main Field" };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            foreach (DataRow dr in dsTemp.Tables[0].Rows)
                g_MainField.Add(dr["FIELD_NAME"].ToString(), new string[] { dr["FIELD_VALUE"].ToString(), dr["PARAM_FIELD"].ToString(), dr["SELECT_LAST_INDEX"].ToString() });
            sSQL = "SELECT PARAM_NAME, PARAM_VALUE, DEFAULT_VALUE FROM SAJET.SYS_BASE_PARAM WHERE PROGRAM = :PROGRAM AND PARAM_TYPE = 'Button' ORDER BY PARAM_NAME";
            Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROGRAM", g_sProgram };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            ToolStripButton ts;
            foreach (DataRow dr in dsTemp.Tables[0].Rows)
            {
                ts = new ToolStripButton(dr[0].ToString(), btnSN.Image)
                {
                    TextImageRelation = TextImageRelation.ImageAboveText,
                    Tag = dr[1].ToString(),
                    ToolTipText = dr[2].ToString()
                };
                ts.Click += new EventHandler(ts_Click);
                bindingNavigator1.Items.Add(ts);
            }
            sSQL = "SELECT * FROM SAJET.SYS_BASE_PARAM WHERE PROGRAM = :PROGRAM AND NVL(PARAM_TYPE, 'N/A') <> 'Button'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            combFilter.Items.Clear();
            combFilterField.Clear();
            string sField = string.Empty, sLabel = string.Empty;
            foreach (DataRow dr in dsTemp.Tables[0].Rows)
            {
                TDBInitial dbInitial = new TDBInitial
                {
                    sValue = dr["PARAM_VALUE"].ToString(),
                    sDefault = dr["DEFAULT_VALUE"].ToString(),
                    sType = dr["PARAM_TYPE"].ToString(),
                    slValue = new List<string>()
                };
                dbInitial.slValue.AddRange(dr["PARAM_VALUE"].ToString().Split(','));
                g_DBInitial.Add(dr["PARAM_NAME"].ToString(), dbInitial);
            }

            {
                if (g_DBInitial.ContainsKey("Grid"))
                {
                    g_DBInitial.Remove("Grid");
                }
                TDBInitial dbInitial = new TDBInitial
                {
                    sValue
                    = "WORK_ORDER,WOSTATUS,PART_NO,OLD_NUMBER,VERSION,WO_RULE,"
                    + "WO_TYPE,TARGET_QTY,PDLINE_NAME,ROUTE_NAME,MADE_CATEGORY,CUSTOMER1,"
                    + "CUSTOMER3,SALES,CUSTOMIZE,PRODUCT_RATE,OUTSOURCE,"
                    + "CUSTOMER_DATE,CUSTOMER_DUE_DATE,PALLETS,WO_SCHEDULE_DATE,WO_DUE_DATE"
                    + "RULE1,UNIT,BLUEPRINT,REMARK",

                    slValue = new List<string>()
                };
                dbInitial.slValue.AddRange(new string[]
                {
                        "WORK_ORDER", "WOSTATUS", "PART_NO", "OLD_NUMBER", "VERSION", "WO_RULE",
                        "WO_TYPE", "TARGET_QTY", "PDLINE_NAME", "ROUTE_NAME", "MADE_CATEGORY", "CUSTOMER1",
                        "CUSTOMER3", "SALES", "CUSTOMIZE", "PRODUCT_RATE", "OUTSOURCE",
                        "CUSTOMER_DATE", "CUSTOMER_DUE_DATE", "PALLETS", "WO_SCHEDULE_DATE", "WO_DUE_DATE",
                        "RULE1",  "UNIT", "BLUEPRINT", "REMARK"
                });
                g_DBInitial.Add("Grid", dbInitial);

            }
            if (g_DBInitial.ContainsKey("Initial_Table FIELD") && g_DBInitial.ContainsKey("Initial_Table LABEL"))
            {
                sField = g_DBInitial["Initial_Table FIELD"].sValue;
                sLabel = g_DBInitial["Initial_Table LABEL"].sValue;
            }
            TableDefine.Initial_Table(sField, sLabel);

            m_tControlData = new TControlData[TableDefine.tGridField.Length];
            if (!g_DBInitial.ContainsKey("Packing Spec WO"))
            {
                TDBInitial dbInitial = new TDBInitial
                {
                    sValue = @"SELECT B.PKSPEC_NAME, BOX_CAPACITY BOX_QTY, CARTON_CAPACITY CARTON_QTY, PALLET_CAPACITY PALLET_QTY, A.PKSPEC_ID
                    FROM SAJET.G_PACK_SPEC A, SAJET.SYS_PKSPEC B 
                    WHERE WORK_ORDER = :WORK_ORDER AND A.PKSPEC_ID = B.PKSPEC_ID
                    ORDER BY BOX_CAPACITY DESC, CARTON_CAPACITY DESC, PALLET_CAPACITY DESC"
                };
                g_DBInitial.Add("Packing Spec WO", dbInitial);
            }
            //lstPack.Columns.Clear();
            string[] slValue, slDefault;
            if (g_DBInitial.ContainsKey("Packing Spec Title"))
            {
                slValue = g_DBInitial["Packing Spec Title"].sValue.ToString().Split(',');
                slDefault = g_DBInitial["Packing Spec Title"].sDefault.ToString().Split(',');
            }
            else
            {
                slValue = new string[] { "PKSPEC_NAME", "BOX_QTY", "CARTON_QTY", "PALLET_QTY" };
                slDefault = new string[] { "200", "90", "90", "90" };
            }
            for (int i = 0; i < slValue.Length; i++)
            {
                ColumnHeader ch = new ColumnHeader
                {
                    Name = slValue[i],
                    Text = slValue[i],
                    Width = int.Parse(slDefault[i])
                };
                //lstPack.Columns.Add(ch);
            }
            if (g_DBInitial.ContainsKey("Filter"))
            {
                slValue = g_DBInitial["Filter"].sValue.ToString().Split(',');
                foreach (string sValue in slValue)
                {
                    combFilter.Items.Add(sValue);
                    combFilterField.Add(sValue);
                }
            }
            else
            {
                combFilter.Items.Add("Work Order"); //Part
                combFilterField.Add("WORK_ORDER");
                combFilter.Items.Add("Part No"); //Part
                combFilterField.Add("PART_NO");
                combFilter.Items.Add("W/O Type"); //WO type
                combFilterField.Add("WO_TYPE");
                combFilter.Items.Add("Customer Code"); //Customer
                combFilterField.Add("CUSTOMER_CODE");
                combFilter.Items.Add("Default Line"); //Line
                combFilterField.Add("PDLINE_NAME");
                combFilter.Items.Add("Route Name"); //Route
                combFilterField.Add("ROUTE_NAME");
            }
            Label lablTemp;
            TextBox txtTemp;
            ComboBox ddlTemp;

            int iRow = 0;
            for (int i = 0; i < TableDefine.tGridField.Length; i++)
            {
                if (!string.IsNullOrEmpty(TableDefine.tGridField[i].sFieldName))
                {
                    lablTemp = new Label
                    {
                        Font = new Font("新細明體", 11),
                        Text = TableDefine.tGridField[i].sCaption,
                        TextAlign = ContentAlignment.MiddleLeft,
                        Dock = DockStyle.Fill
                    };

                    if (g_MainField.ContainsKey(TableDefine.tGridField[i].sFieldName))
                    {
                        ddlTemp = new ComboBox
                        {
                            Dock = DockStyle.Fill,
                            DropDownStyle = ComboBoxStyle.DropDownList,
                            Font = new Font("新細明體", 11)
                        };
                        m_tControlData[i].combControl = ddlTemp;
                    }
                    else
                    {
                        txtTemp = new TextBox
                        {
                            ForeColor = Color.Maroon,
                            ReadOnly = true,
                            BackColor = SystemColors.Window,
                            Dock = DockStyle.Fill,
                            Name = "labl" + TableDefine.tGridField[i].sFieldName,
                            Font = new Font("新細明體", 11, FontStyle.Bold)
                        };
                        m_tControlData[i].txtControl = txtTemp;
                    }
                }
                m_tControlData[i].sFieldName = TableDefine.tGridField[i].sFieldName;
                iRow++;
            }

            //gbPack.Dock = DockStyle.Fill;
            Initial_Form();
            Text = Text + "(" + SajetCommon.g_sFileVersion + ")";

            //Select Emp ID
            sSQL = @" SELECT EMP_ID,NVL(FACTORY_ID,0) FACTORY_ID
                 FROM SAJET.SYS_EMP
                 WHERE EMP_ID = :EMP_ID";
            Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "EMP_ID", ClientUtils.UserPara1 };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            string sUserFacID = dsTemp.Tables[0].Rows[0]["FACTORY_ID"].ToString();

            //Select Factory
            combFactory.Items.Clear();
            sSQL = @" SELECT FACTORY_ID,FACTORY_CODE 
                 FROM SAJET.SYS_FACTORY 
                 WHERE ENABLED = 'Y' 
                 ORDER BY FACTORY_CODE ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                g_sFactoryID = dsTemp.Tables[0].Rows[0]["FACTORY_ID"].ToString();
                for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
                {
                    combFactory.Items.Add(dsTemp.Tables[0].Rows[i]["FACTORY_CODE"].ToString());
                    if (sUserFacID == dsTemp.Tables[0].Rows[i]["FACTORY_ID"].ToString())
                    {
                        g_sFactoryID = dsTemp.Tables[0].Rows[i]["FACTORY_ID"].ToString();
                        combFactory.SelectedIndex = i;
                        combFactory.Enabled = false;
                    }
                }
            }
            if (sUserFacID == "0")
            {
                combFactory.SelectedIndex = 0;
                combFactory.Enabled = true;
            }

            //讀取SYS_BASE設定值
            int iSelectInx = combWoStatus.Items.Count - 1;
            dsTemp = ClientUtils.GetSysBaseData(g_sProgram, "Default Search Wo Status"); //預設選擇WO Status
            string sDefaultWoStatus = iSelectInx.ToString();
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                sDefaultWoStatus = dsTemp.Tables[0].Rows[0]["PARAM_VALUE"].ToString();

                if (!string.IsNullOrEmpty(sDefaultWoStatus))
                {
                    bool bResult = int.TryParse(sDefaultWoStatus, out iSelectInx);
                    if (!bResult)
                        iSelectInx = combWoStatus.Items.Count - 1;
                }
            }
            combWoStatus.SelectedIndex = iSelectInx;

            dsTemp.Dispose();
            Check_Privilege();

            btnAssignMAC.Visible = GetAssignMac();
            MenuMACRequest.Visible = btnAssignMAC.Visible;
            if (g_DBInitial.ContainsKey("Unvisible Button"))
            {
                slValue = g_DBInitial["Unvisible Button"].sValue.Split(',');
                int iUnvisible = 0;
                foreach (string sValue in slValue)
                {
                    switch (sValue)
                    {
                        case "SN":
                            btnSN.Visible = false;
                            MenuViewSN.Visible = false;
                            iUnvisible++;
                            break;
                        case "PANEL SN":
                            btnPanel.Visible = false;
                            viewToolStripMenuItem.Visible = false;
                            iUnvisible++;
                            break;
                        case "BOM":
                            btnBom.Visible = false;
                            MenuViewBOM.Visible = false;
                            iUnvisible++;
                            break;
                    }
                }
                if (iUnvisible == 3)
                {
                    tsView.Visible = false;
                    toolStripSeparator1.Visible = MenuMACRequest.Visible;
                }
            }
            SajetInifile sini = new SajetInifile();
            string sWidth = sini.ReadIniFile(sIniFile, g_sProgram, g_sFunction + " Splitter", "");
            sini.Dispose();
            if (!string.IsNullOrEmpty(sWidth))
            {
                panel2.Width = int.Parse(sWidth);
            }
            else
            {
                panel2.Width = 500;
            }
            splitter1.SplitterMoved += new SplitterEventHandler(splitter1_SplitterMoved);

            #region 設定時間區間

            datePickerFrom.Checked = true;
            datePickerFrom.Value = OtSrv.GetDBDateTimeNow().AddDays(-30);
            datePickerTo.Checked = true;
            datePickerTo.Value = OtSrv.GetDBDateTimeNow();

            #endregion

            ShowData(true);
            contextMenuStrip1.Visible = false;
        }

        private void AddControlToToolStrip()
        {
            var lblWoDate = new ToolStripLabel("WorkOrderDate");
            datePickerFrom.ValueChanged += DatePicker_ValueChanged;
            datePickerFrom.ShowCheckBox = true;

            //datePickerFrom
            datePickerTo.ValueChanged += DatePicker_ValueChanged;
            datePickerTo.ShowCheckBox = true;

            var datePickerFromControlHost = new ToolStripControlHost(datePickerFrom)
            {
                Size = new Size(150, 22)
            };
            var lblTo = new ToolStripLabel(" ～ ");
            var datePickerToControlHost = new ToolStripControlHost(datePickerTo)
            {
                Size = new Size(150, 22)
            };

            var btnSearch = new ToolStripButton("SearchByCondition")
            {
                Image = Resource1.search,
                DisplayStyle = ToolStripItemDisplayStyle.ImageAndText
            };
            var lblSpace = new ToolStripLabel(" ");
            btnSearch.AutoSize = false;
            btnSearch.Size = new Size(100, 23);
            btnSearch.Click += btnSearch_Click;

            var lblTimeScale = new ToolStripLabel("TimeScale");

            cbtimeScale.Items.Add("DayOfMonth");
            cbtimeScale.Items.Add("WeekOfYear");
            cbtimeScale.DropDownStyle = ComboBoxStyle.DropDownList;
            cbtimeScale.SelectedIndex = 0;
            cbtimeScale.SelectedIndexChanged += CbtimeScale_SelectedIndexChanged;

            // set the Search button image size
            Bitmap b = new Bitmap(54, 30);
            using (Graphics g = Graphics.FromImage(b))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(btnSearch.Image, 0, 0, 54, 30);
            }
            Image myResizedImg = b;

            //put the resized image back to the button and change toolstrip's ImageScalingSize property 
            btnSearch.Image = myResizedImg;
            toolStrip1.ImageScalingSize = new Size(54, 30);
            var separator = new ToolStripSeparator();

            toolStrip1.Items.Add(lblWoDate);
            toolStrip1.Items.Add(datePickerFromControlHost);
            toolStrip1.Items.Add(lblTo);
            toolStrip1.Items.Add(datePickerToControlHost);
            toolStrip1.Items.Add(btnSearch);
            toolStrip1.Items.Add(separator);
            toolStrip1.Items.Add(lblSpace);
            toolStrip1.Items.Add(lblTimeScale);
            toolStrip1.Items.Add(cbtimeScale);
        }

        private void Initial_Form()
        {
            SajetCommon.SetLanguageControl(this);
        }

        private void CbtimeScale_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbtimeScale.SelectedIndex == 0)
            {
                ShowGanttChart(TimeScale.Day);
            }
            else
            {
                ShowGanttChart(TimeScale.Week);
            }
        }

        private void DatePicker_ValueChanged(object sender, EventArgs e)
        {
            if (((DateTimePicker)sender).Checked == false)
            {
                ((DateTimePicker)sender).CustomFormat = " ";
            }
            else
            {
                ((DateTimePicker)sender).CustomFormat = "yyyy/MM/dd";
            }
            ((DateTimePicker)sender).Format = DateTimePickerFormat.Custom;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                ShowData(true);
            }
            finally
            {
                Cursor = Cursors.Default;
            }

        }

        public static String g_sProgram, g_sFunction, g_sExeName;
        public String g_sOrderField;
        object[][] g_Params;
        public static String g_sFactoryID;
        public static int g_iPrivilege, g_iReleasePri;
        string g_sDataSQL;

        private void ShowGanttChart(TimeScale timeScale)
        {
            if (ganttChart != null)
            {
                ganttChart.Refresh();
                ganttChart.ClearToolTips();
            }

            // get earlest & latest datetime
            var startDateList = (from DataGridViewRow row
                                 in gvData.Rows
                                 select DateTime.Parse(row.Cells["WO_SCHEDULE_DATE"].Value.ToString())
                                 ).ToList();

            var earliestTaskStartDate = startDateList.Any() ? DateTime.Parse((startDateList.Min()).ToString("yyyy/MM/dd")) : DateTime.Now.Date;
            var earliestTaskRealStartDate = m_DicGRcTravel.Any() ? m_DicGRcTravel.Min(pair => pair.Value.RealStart) : DateTime.Now.Date;

            var earliestStartDate = earliestTaskStartDate > earliestTaskRealStartDate
                ? earliestTaskRealStartDate
                : earliestTaskStartDate;

            // assign Task timespan to ProjectManager
            var projectManager = new ProjectManager();

            foreach (DataGridViewRow row in gvData.Rows)
            {
                var task = new Task
                {
                    Name = row.Cells["WORK_ORDER"].Value.ToString()
                };
                var taskStartDate = DateTime.Parse(row.Cells["WO_SCHEDULE_DATE"].Value.ToString());
                var taskDueDate = DateTime.Parse(row.Cells["WO_Due_DATE"].Value.ToString());


                var startDateTimeSpan = taskStartDate - earliestStartDate;
                var startingPoint = timeScale == TimeScale.Day
                    ? (int)Math.Round(startDateTimeSpan.TotalDays)
                    : (int)Math.Ceiling(startDateTimeSpan.TotalDays) / 7;

                var span = taskDueDate - taskStartDate;
                var duration = timeScale == TimeScale.Day
                    ? (int)Math.Round(span.TotalDays)
                    : GetWeeks(taskStartDate, taskDueDate);

                // Set real start, real due, etc... 
                var realStartStr = "NA";
                var realEndStr = "NA";
                var rcNoStr = "NA";
                var ngQtyNoStr = "NA";
                if (m_DicGRcTravel.ContainsKey(task.Name))
                {
                    var gRctravel = m_DicGRcTravel[task.Name];
                    var realStartDateTimeSpan = gRctravel.RealStart - earliestStartDate;
                    int realStartingPoint = timeScale == TimeScale.Day
                        ? (int)Math.Round(realStartDateTimeSpan.TotalDays)
                        : (int)Math.Ceiling(realStartDateTimeSpan.TotalDays) / 7;
                    var realSpan = gRctravel.RealEnd - gRctravel.RealStart;
                    var realDuration = timeScale == TimeScale.Day
                        ? (int)Math.Round(realSpan.TotalDays)
                        : GetWeeks(gRctravel.RealStart, gRctravel.RealEnd);
                    task.RealStart = realStartingPoint;
                    task.RealDuration = realDuration == 0 ? 1 : realDuration; // min=1 
                    task.RealEnd = task.RealStart + task.RealDuration;

                    realStartStr = gRctravel.RealStart.ToString("yyyy/MM/dd");
                    realEndStr = gRctravel.RealEnd.ToString("yyyy/MM/dd");
                    rcNoStr = gRctravel.RcNo.ToString();
                    ngQtyNoStr = gRctravel.NgQty.ToString();
                }

                projectManager.Add(task);
                projectManager.SetStart(task, startingPoint);
                projectManager.SetDuration(task, duration);

                // set tooltips
                var toolTipstring = $"         {task.Name} \r\n" +
                                    $"--------------------------------- \r\n" +
                                    $"預計開始日期: {taskStartDate.ToString("yyyy/MM/dd")} \r\n" +
                                    $"預計結束日期: {taskDueDate.ToString("yyyy/MM/dd")} \r\n" +
                                    $"目標量: {row.Cells["TARGET_QTY"].Value} \r\n" +
                                    $"實際開始日期: {realStartStr} \r\n" +
                                    $"實際結束日期: {realEndStr} \r\n" +
                                    $"已執行流程卡數/不良品數: {rcNoStr} / {ngQtyNoStr}";

                ganttChart.SetToolTip(task, toolTipstring);
            }

            // init GanttChart
            ganttChart.Init(projectManager);

            // set time information
            projectManager.TimeScale = timeScale;
            projectManager.Start = earliestStartDate;
            ganttChart.TimeScaleDisplay = timeScale == TimeScale.Day ? TimeScaleDisplay.DayOfMonth : TimeScaleDisplay.WeekOfYear;
            ganttChart.Refresh();
        }

        #region calculate week count
        ///
        /// refer to : https://stackoverflow.com/questions/39788467/count-number-of-including-weeks-between-2-dates
        /// 
        private static int GetWeeks(DateTime start, DateTime end)
        {
            start = GetStartOfWeek(start);
            end = GetStartOfWeek(end);
            int days = (int)(end - start).TotalDays;
            return (days / 7) + 1;
        }

        private static DateTime GetStartOfWeek(DateTime input)
        {
            // Using +6 here leaves Monday as 0, Tuesday as 1 etc.
            int dayOfWeek = (((int)input.DayOfWeek) + 6) % 7;
            return input.Date.AddDays(-dayOfWeek);
        }

        #endregion calculate week count

        private void ts_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null) return;
            ToolStripButton ts = (ToolStripButton)sender;
            Assembly assembly = null;
            object obj = null;
            Type type = null;
            try
            {
                assembly = Assembly.LoadFrom(Application.StartupPath + Path.DirectorySeparatorChar + g_sExeName + Path.DirectorySeparatorChar + ts.Tag.ToString());
                string[] Name = assembly.FullName.ToString().Split(',');
                type = assembly.GetType(Name[0] + ".fMain");
                DataSet dsMaster = new DataSet();
                dsMaster.Tables.Add();
                dsMaster.Tables[0].Rows.Add();
                for (int i = 0; i < gvData.Columns.Count; i++)
                {
                    dsMaster.Tables[0].Columns.Add(gvData.Columns[i].Name);
                    dsMaster.Tables[0].Rows[0][i] = gvData.CurrentRow.Cells[i].Value;
                }
                object[] arg = new object[] { dsMaster, g_sExeName };
                obj = assembly.CreateInstance(type.FullName, true, BindingFlags.CreateInstance, null, arg, null, null);
                Form formChild = (Form)obj;
                DialogResult dr = formChild.ShowDialog();
                if (ts.ToolTipText == "Y" && (dr == DialogResult.OK || dr == DialogResult.Yes))
                {
                    string sSelectKeyValue = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();
                    ShowData();
                    SetSelectRow(g_sDataSQL, g_Params, gvData, sSelectKeyValue, TableDefine.gsDef_KeyField);
                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 1);
            }
        }

        private void Check_Privilege()
        {
            g_iPrivilege = ClientUtils.GetPrivilege(ClientUtils.UserPara1, g_sFunction, g_sProgram);
            btnAppend.Enabled = (g_iPrivilege >= 1);
            btnModify.Enabled = (g_iPrivilege >= 0);
            btnDelete.Enabled = btnDelete.Visible = (g_iPrivilege > 1);

            g_iReleasePri = ClientUtils.GetPrivilege(ClientUtils.UserPara1, "W/O Release", g_sProgram);
        }

        private bool GetAssignMac()
        {
            DataSet dsTemp = new DataSet();
            try
            {
                string sSQL = @"SELECT * FROM SAJET.SYS_LABEL 
                    WHERE LABEL_NAME = 'MAC' 
                    AND TYPE = 'S' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows.Count == 0)
                {
                    return false;
                }

                sSQL = @"Select SQL_NAME from sajet.sys_sql 
                    where SYSUSE_NAME='MAC REQUEST' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows.Count == 0)
                {
                    return false;
                }

                string sFile = Application.StartupPath + "\\" + fMain.g_sExeName + "\\AssignMAC.dll";
                if (!File.Exists(sFile))
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                dsTemp.Dispose();
            }
        }

        public void ShowData(bool isSerachButtonClicked = false)
        {
            string sSQL;
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FACTORY_ID", g_sFactoryID };
            string sCondition = string.Empty;
            if (combWoStatus.SelectedIndex != combWoStatus.Items.Count - 1)
            {
                sCondition = " AND A.WO_STATUS = :WO_STATUS ";
                Array.Resize(ref Params, Params.Length + 1);
                Params[Params.Length - 1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WO_STATUS", combWoStatus.SelectedIndex.ToString() };
            }

            if (isSerachButtonClicked)
            {
                // add filter for DateTime
                if (datePickerFrom.Checked)
                {
                    var from = datePickerFrom.Value.Date.ToString("yyyyMMdd");
                    sCondition += $" AND A.WO_SCHEDULE_DATE >= To_Date('{from} 00:00:00', 'yyyymmdd HH24:MI:SS') ";
                }

                if (datePickerTo.Checked)
                {
                    var to = datePickerTo.Value.Date.ToString("yyyyMMdd");
                    sCondition += $" And A.WO_DUE_DATE <= To_Date('{to} 23:59:59', 'yyyymmdd HH24:MI:SS') ";
                }
            }


            if (!string.IsNullOrEmpty(editFilter.Text.Trim()))
            {
                string sFieldName = combFilterField[combFilter.SelectedIndex].ToString();

                sCondition += " AND " + sFieldName + " LIKE :FILTER || '%' ";
                Array.Resize(ref Params, Params.Length + 1);
                Params[Params.Length - 1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FILTER", editFilter.Text.Trim() };
            }


            sSQL =
                @"
SELECT
    A.WORK_ORDER,
    SAJET.SJ_WOSTATUS_RESULT(A.WO_STATUS) WOSTATUS,
    B.PART_NO,
    A.OLD_NUMBER,
    A.VERSION,
    A.WO_RULE,
    A.WO_TYPE,
    A.TARGET_QTY,
    D.PDLINE_NAME,
    C.ROUTE_NAME,
    A.MADE_CATEGORY,
    A.CUSTOMER1,
    A.CUSTOMER3,
    A.SALES,
    A.CUSTOMIZE,
    A.PRODUCT_RATE,
    A.OUTSOURCE,
    A.CUSTOMER_DATE,
    A.CUSTOMER_DUE_DATE,
    A.PALLETS,
    /* B.OPTION1 PALLETS, --*/
    A.WO_SCHEDULE_DATE,
    A.WO_DUE_DATE,
    A.RULE1,
    A.UNIT,
    A.BLUEPRINT,
    A.FINISHED,
    A.BAD,
    A.NOTPAID,
    A.REMARK,
    A.OPERATION_ID,
    A.PART_ID,
    A.SCRAP_QTY,
    A.PRODUCE_NUMBER,
    A.PRODUCENO2,
    A.PRODUCENO1,
    A.ROUTE_ID,
    A.DEFAULT_PDLINE_ID,
    A.WO_OPTION2
FROM
    SAJET.G_WO_BASE      A
    LEFT JOIN SAJET.SYS_PART       B ON A.PART_ID = B.PART_ID
    LEFT JOIN SAJET.SYS_RC_ROUTE   C ON A.ROUTE_ID = C.ROUTE_ID
    LEFT JOIN SAJET.SYS_PDLINE     D ON A.DEFAULT_PDLINE_ID = D.PDLINE_ID
    LEFT JOIN SAJET.SYS_PROCESS    E ON A.START_PROCESS_ID = E.PROCESS_ID
    LEFT JOIN SAJET.SYS_PROCESS    F ON A.END_PROCESS_ID = F.PROCESS_ID
    LEFT JOIN SAJET.SYS_CUSTOMER   G ON A.CUSTOMER_ID = G.CUSTOMER_ID
WHERE
    A.FACTORY_ID = :FACTORY_ID
";
            sSQL += sCondition + " ORDER BY " + g_sOrderField;

            g_sDataSQL = sSQL;
            g_Params = Params;

            (new MESGridView.DisplayGridView()).GetGridView(gvData, sSQL, Params, out memoryCache);
            //欄位Title 
            if (gvData.Columns.Count > 0)
            {
                foreach (DataGridViewColumn dc in gvData.Columns)
                {
                    if (g_DBInitial["Grid"].slValue.IndexOf(dc.Name) > -1) // Grid
                    {
                        dc.HeaderText = SajetCommon.SetLanguage(dc.HeaderText);
                        dc.DisplayIndex = g_DBInitial["Grid"].slValue.IndexOf(dc.Name);
                    }
                    else
                        dc.Visible = false;
                }

                gvData.Columns["WORK_ORDER"].Frozen = true;

                gvData.Focus();
                if (gvData.Rows.Count == 0)
                {
                    ClearData();
                }

                // select data from G_RC_TRAVEL table
                SelectRealStartEndDate();

                // show Gantt Chart based on the time scale
                ShowGanttChart(cbtimeScale.SelectedIndex == 0 ? TimeScale.Day : TimeScale.Week);

                tsCount.Text = gvData.Rows.Count.ToString();
            }
            else
            {
                throw new Exception($@"No work order data found!");
            }
        }

        private void SelectRealStartEndDate()
        {
            m_DicGRcTravel.Clear();
            var sql = @"
SELECT
    work_order,
    MIN(nvl(out_process_time, create_time)),
    MAX(nvl(out_process_time, update_time)),
    COUNT(DISTINCT rc_no),
    SUM(wip_out_scrap_qty)
FROM
    sajet.g_rc_travel
GROUP BY
    work_order
";
            DataSet dsGRcTravel = ClientUtils.ExecuteSQL(sql);
            if (dsGRcTravel.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsGRcTravel.Tables[0].Rows)
                {
                    var workOrderId = dr[0].ToString();
                    var workOrderIdExist = CheckExistence(workOrderId);
                    if (workOrderIdExist)
                    {
                        var grcTravel = new GRcTravel()
                        {
                            RealStart = DateTime.Parse(dr[1].ToString()),
                            RealEnd = DateTime.Parse(dr[2].ToString()),
                            RcNo = int.TryParse(dr[3].ToString(), out int i_rc_count)
                                ? i_rc_count
                                : 0,
                            NgQty = int.TryParse(dr[4].ToString(), out int i_out_scrap_qty_sum)
                                ? i_out_scrap_qty_sum
                                : 0
                        };

                        m_DicGRcTravel.Add(workOrderId, grcTravel);
                    }
                }
            }
            dsGRcTravel.Dispose();
        }

        private bool CheckExistence(string workorderId)
        {
            foreach (DataGridViewRow vr in gvData.Rows)
            {
                var id = vr.Cells[0].Value.ToString();
                if (string.Compare(workorderId, id, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        private void btnAppend_Click(object sender, EventArgs e)
        {
            fData f = new fData(g_DBInitial);
            try
            {
                Cursor = Cursors.WaitCursor;
                f.g_sUpdateType = "APPEND";
                f.g_sformText = btnAppend.Text;
                f.g_sFactory = combFactory.Text;

                if (f.ShowDialog() == DialogResult.OK)
                {
                    ShowData(true);
                    SetSelectRow(g_sDataSQL, g_Params, gvData, f.NewWorkOrder, TableDefine.gsDef_KeyField);
                }
            }
            finally
            {
                Cursor = Cursors.Default;
                f.Dispose();
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            fData f = null;
            try
            {
                Cursor = Cursors.WaitCursor;

                if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                    return;
                f = new fData(g_DBInitial)
                {
                    g_sUpdateType = "MODIFY",
                    g_sformText = btnModify.Text,
                    dataCurrentRow = gvData.CurrentRow,
                    dataGridColumn = gvData.Columns,
                    g_sFactory = combFactory.Text
                };

                string sSelectKeyValue = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();
                if (f.ShowDialog() == DialogResult.OK)
                {
                    ShowData();
                    SetSelectRow(g_sDataSQL, g_Params, gvData, sSelectKeyValue, TableDefine.gsDef_KeyField);
                }
            }
            finally
            {
                Cursor = Cursors.Default;
                if (f != null)
                {
                    f.Dispose();
                }
            }
        }

        public static void CopyToHistory(string sID)
        {
            string sSQL = string.Format(@" Insert into {0}
                Select * from {1}
                where {2} = :sID", TableDefine.gsDef_HTTable, TableDefine.gsDef_Table, TableDefine.gsDef_KeyField);
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "sID", sID };
            ClientUtils.ExecuteSQL(sSQL, Params);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            saveFileDialog1.DefaultExt = "xls";
            saveFileDialog1.Filter = "All Files(*.xls)|*.xls";
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            string sFileName = saveFileDialog1.FileName;

            ExportExcel.CreateExcel Export = new ExportExcel.CreateExcel(sFileName);
            Export.ExportToExcel(gvData);
        }

        private void SetSelectRow(string sSQL, object[][] Params, DataGridView GridData, String sPrimaryKey, String sField)
        {
            if (GridData.Rows.Count > 0)
            {
                int iIndex = 0;
                string sShowField = GridData.Columns[0].Name;
                for (int i = 0; i <= GridData.Columns.Count - 1; i++)
                {
                    if (GridData.Columns[i].Visible)
                    {
                        //第一個有顯示的欄位(focus到隱藏欄位會錯誤)
                        sShowField = GridData.Columns[i].Name;
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(sPrimaryKey))
                {
                    string sCondition = "";
                    string[] tsField = sField.Split(',');
                    string[] tsValue = sPrimaryKey.Split(',');
                    for (int j = 0; j <= tsField.Length - 1; j++)
                    {
                        if (j == 0)
                            sCondition = " Where " + tsField[j].ToString() + "='" + tsValue[j].ToString() + "' ";
                        else
                            sCondition = sCondition + " and " + tsField[j].ToString() + "='" + tsValue[j].ToString() + "' ";

                    }
                    //改用SQL找,不由Grid讀值,否則速度會慢
                    string sText = "select idx from ("
                                 + " Select aa.*,rownum-1 idx from ("
                                 + sSQL
                                 + " ) aa ) "
                                 + sCondition;
                    DataSet ds = ClientUtils.ExecuteSQL(sText, Params);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        iIndex = Convert.ToInt32(ds.Tables[0].Rows[0]["idx"].ToString());
                    }
                }
                GridData.Focus();
                GridData.CurrentCell = GridData.Rows[iIndex].Cells[sShowField];
                GridData.Rows[iIndex].Selected = true;
            }
        }

        private void gvData_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            try
            {
                // ColumnIndex = 17 is CUSTOMER_DATE,  ColumnIndex = 18 is CUSTOMER_DUE_DATE
                // ColumnIndex = 20 is WO_SCHEDULE_DATE,  ColumnIndex = 21 is WO_DUE_DATE
                var str = memoryCache.RetrieveElement(e.RowIndex, e.ColumnIndex);

                string this_column_name = gvData.Columns[e.ColumnIndex].Name;
                var column_names = new string[]
                {
                    "CUSTOMER_DATE",
                    "CUSTOMER_DUE_DATE",
                    "WO_SCHEDULE_DATE",
                    "WO_DUE_DATE",
                };

                if (!string.IsNullOrEmpty(str) &&
                    column_names.Contains(this_column_name))
                {
                    e.Value = DateTime.Parse(str).ToString("yyyy/MM/dd");
                }
                else if (!string.IsNullOrEmpty(str) && this_column_name == "MADE_CATEGORY") // Made_Category
                {
                    e.Value = ConvertMadeCategoryToString(str);
                }
                else
                {
                    e.Value = str;
                }
            }
            catch
            {
                throw;
            }
        }

        private string ConvertMadeCategoryToString(string category)
        {
            switch (category)
            {
                case "1":
                    return "試製";
                case "2":
                    return "樣試";
                case "3":
                    return "量試";
                case "4":
                    return "量產";
                case "5":
                    return "重工";
                default:
                    throw new Exception($"Unsupported made category number: {category}");
            }
        }

        private void MenuHistory_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null) return;
            string sWO = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();
            string sSQL = string.Empty;
            if (g_DBInitial.ContainsKey("History SQL"))
                sSQL = g_DBInitial["History SQL"].sValue;
            else
                sSQL = TableDefine.History_SQL();
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWO };
            fLog fH = new fLog(sSQL, Params);
            fH.txtWo.Text = sWO;
            fH.ShowDialog();
            fH.Dispose();
        }

        private void MenuViewSN_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;
            string sSQL = string.Empty;
            if (g_DBInitial.ContainsKey("SN SQL"))
                sSQL = g_DBInitial["SN SQL"].sValue;
            else


                sSQL = @" Select A.SERIAL_NUMBER,
                      (CASE WHEN A.CURRENT_STATUS = 0 THEN 'Queue' WHEN A.CURRENT_STATUS =1 THEN 'Runnung' WHEN A.CURRENT_STATUS =2 THEN 'Hold' WHEN A.CURRENT_STATUS =8 THEN 'Scrap' WHEN A.CURRENT_STATUS =9 THEN 'Finish' END) CURRENT_STATUS,
                      B.PROCESS_NAME, A.GOOD_QTY, A.SCRAP_QTY, A.RC_NO
                      From SAJET.G_SN_STATUS A
                      LEFT JOIN SAJET.SYS_PROCESS B ON A.PROCESS_ID = B.PROCESS_ID
                      Where A.WORK_ORDER = :WORK_ORDER
                      Order by A.SERIAL_NUMBER ";



            string sFieldID = gvData.CurrentRow.Cells["WORK_ORDER"].Value.ToString();
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sFieldID };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            fHistory fH = new fHistory();
            try
            {
                fH.Text = "View SN";
                fH.dgvHistory.DataSource = dsTemp;
                fH.dgvHistory.DataMember = dsTemp.Tables[0].ToString();
                fH.LabCount.Text = dsTemp.Tables[0].Rows.Count.ToString();
                fH.LabWO.Text = sFieldID;
                fH.ShowDialog();
            }
            finally
            {
                fH.Dispose();
            }
            dsTemp.Dispose();
        }

        private void MenuViewBOM_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;

            string sPartID = gvData.CurrentRow.Cells["PART_ID"].Value.ToString();
            string sVer = gvData.CurrentRow.Cells["VERSION"].Value.ToString();

            fWoBom fB = new fWoBom();
            try
            {
                fB.LabWO.Text = gvData.CurrentRow.Cells["WORK_ORDER"].Value.ToString();
                fB.LabPartNo.Text = gvData.CurrentRow.Cells["PART_NO"].Value.ToString();
                fB.LabVer.Text = sVer;
                fB.g_sPartID = sPartID;

                //只可以讀,不可修改
                fB.LabType.Text = "Read Only";
                fB.TreeBomData.AllowDrop = false;
                fB.LVPart.AllowDrop = false;
                fB.MenuItemDelete.Visible = false;

                fB.ShowBom(sPartID, sVer);
                fB.ShowDialog();
            }
            finally
            {
                fB.Dispose();
            }
        }

        private void MenuStatus_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;

            string sFieldID = gvData.CurrentRow.Cells["WORK_ORDER"].Value.ToString();
            string sSQL = @" Select A.WORK_ORDER ,A.UPDATE_TIME 
                ,SAJET.SJ_WOStatus_Result(A.WO_STATUS) WOSTATUS 
                ,A.MEMO REMARK,B.EMP_NAME UPDATE_USER  
                From SAJET.G_WO_STATUS A 
                LEFT JOIN SAJET.SYS_EMP B ON A.update_userid = b.emp_id
                Where A.WORK_ORDER = :WORK_ORDER 
                Order by UPDATE_TIME ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sFieldID };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            fHistory fH = new fHistory();
            try
            {
                fH.Text = "Status Log";
                fH.dgvHistory.DataSource = dsTemp;
                fH.dgvHistory.DataMember = dsTemp.Tables[0].ToString();
                fH.LabCount.Text = dsTemp.Tables[0].Rows.Count.ToString();
                fH.LabWO.Text = sFieldID;

                //替換欄位名稱
                for (int i = 0; i <= fH.dgvHistory.Columns.Count - 1; i++)
                {
                    string sGridField = fH.dgvHistory.Columns[i].HeaderText;
                    string sField = "";
                    for (int j = 0; j <= gvData.Columns.Count - 1; j++)
                    {
                        sField = gvData.Columns[j].Name;
                        if (sGridField == sField)
                        {
                            fH.dgvHistory.Columns[i].HeaderText = gvData.Columns[j].HeaderText;
                            break;
                        }
                    }
                }

                fH.ShowDialog();
            }
            finally
            {
                fH.Dispose();
            }
            dsTemp.Dispose();
        }

        private void MenuViewWoData_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;
            fData f = new fData(g_DBInitial);
            try
            {
                f.g_sUpdateType = "VIEW";
                f.g_sformText = MenuViewWoData.Text;
                f.dataCurrentRow = gvData.CurrentRow;
                f.g_sFactory = combFactory.Text;
                f.ShowDialog();
            }
            finally
            {
                f.Dispose();
            }
        }

        private void ClearData()
        {
            for (int i = 0; i < m_tControlData.Length; i++)
                if (!string.IsNullOrEmpty(m_tControlData[i].sFieldName))
                {
                    if (m_tControlData[i].txtControl != null)
                        m_tControlData[i].txtControl.Text = string.Empty;
                    else if (m_tControlData[i].combControl != null)
                    {
                        m_tControlData[i].combControl.Items.Clear();
                        m_tControlData[i].combControl.Text = string.Empty;
                    }
                }
            //lstPack.Items.Clear();
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;

            string message;

            string sWO = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();

            bool found_any_production_records = OtSrv.FindProductionRecords(sWO);

            if (found_any_production_records)
            {
                message = SajetCommon.SetLanguage("The workorder is already in production, so it can not be switched to 'Release' status");

                SajetCommon.Show_Message(message, 0);

                return;
            }

            message = SajetCommon.SetLanguage("Change Work Order Status to Release")
                + " ?"
                + Environment.NewLine
                + gvData.Columns[TableDefine.gsDef_KeyData].HeaderText
                + " : "
                + sWO;

            if (SajetCommon.Show_Message(message, 2) != DialogResult.Yes) return;

            //更改狀態
            string sSQL = @"Update SAJET.G_WO_BASE 
                Set WO_STATUS = '2' 
                ,UPDATE_USERID = :UPDATE_USERID
                ,UPDATE_TIME = SYSDATE 
                Where WORK_ORDER = :WORK_ORDER ";
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", ClientUtils.UserPara1 };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWO };
            ClientUtils.ExecuteSQL(sSQL, Params);

            // 紀錄狀態變更                     
            sSQL = @"Insert into SAJET.G_WO_STATUS 
                 (Work_Order,WO_Status,Memo,update_userid) 
                 values 
                 (:WORK_ORDER,'2','',:UPDATE_USERID)";
            ClientUtils.ExecuteSQL(sSQL, Params);

            CopyToHistory(sWO);
            ShowData(true);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;

            string sWO = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();

            // 先檢查工單可不可以取消流程卡
            // 1.工單處於下線狀態前
            // 2.此工單的所有流程卡都還沒有進入生產
            string SQL = @"
SELECT WO_STATUS
FROM SAJET.G_WO_BASE
WHERE WORK_ORDER = :WORK_ORDER
";
            object[][] param = new object[1][];
            param[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWO };

            DataSet ds = ClientUtils.ExecuteSQL(SQL, param);

            // 1.工單處於下線狀態前
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                if (int.TryParse(ds.Tables[0].Rows[0]["WO_STATUS"].ToString(), out int WO_STATUS))
                {
                    if (WO_STATUS > 2)
                    {
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("Work order already in process, cannot delete runcards"), 0);
                        return;
                    }
                }
                else
                {
                    SajetCommon.Show_Message("Parse WO status error", 0);
                    return;
                }
            }
            else
            {
                SajetCommon.Show_Message("Get WO status error", 0);
                return;
            }

            // 2.此工單的所有流程卡都還沒有進入生產
            SQL = @"
SELECT RC_NO
FROM SAJET.G_RC_TRAVEL
WHERE WORK_ORDER = :WORK_ORDER
";
            param[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWO };
            ds = ClientUtils.ExecuteSQL(SQL, param);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Work order already in process, cannot delete runcards"), 0);
                return;
            }

            // 再詢問使用者是否要取消
            string sMsg = SajetCommon.SetLanguage("Delete runcards release and work order");
            string sData = gvData.Columns[TableDefine.gsDef_KeyData].HeaderText + " : " + sWO;
            if (SajetCommon.Show_Message(sMsg + " ?" + Environment.NewLine + sData, 2) != DialogResult.Yes)
            {
                ShowData();
                SetSelectRow(g_sDataSQL, g_Params, gvData, sWO, TableDefine.gsDef_KeyField);
                return;
            }

            // 刪除已經生產的流程卡
            SQL = @"
DELETE FROM SAJET.G_RC_STATUS
WHERE WORK_ORDER = :WORK_ORDER
";
            param[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWO };
            ClientUtils.ExecuteSQL(SQL, param);

            // 刪除工單狀態
            SQL = @"
DELETE FROM SAJET.G_WO_STATUS
WHERE WORK_ORDER = :WORK_ORDER
";
            param[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWO };
            ClientUtils.ExecuteSQL(SQL, param);

            // 刪除工單
            SQL = @"
DELETE FROM SAJET.G_WO_BASE
WHERE WORK_ORDER = :WORK_ORDER
";
            param[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWO };
            ClientUtils.ExecuteSQL(SQL, param);

            // 更新畫面
            ShowData();
        }

        private void fMain_Shown(object sender, EventArgs e)
        {
            combFilter.SelectedIndex = 0;
            editFilter.Focus();
        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;

            string sFieldID = gvData.CurrentRow.Cells["WORK_ORDER"].Value.ToString();
            string sSQL;
            if (g_DBInitial.ContainsKey("PANEL SN SQL"))
                sSQL = g_DBInitial["PANEL SN SQL"].sValue;
            else
                sSQL = @" Select A.WORK_ORDER,A.SERIAL_NUMBER,B.EMP_NAME
                From SAJET.G_WO_SN A LEFT JOIN SAJET.SYS_EMP B ON A.emp_id = B.emp_id
                Where A.WORK_ORDER = :WORK_ORDER 
                Order by A.SERIAL_NUMBER ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sFieldID };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            fHistory fH = new fHistory();
            try
            {
                fH.Text = "View Panel SN";
                fH.dgvHistory.DataSource = dsTemp;
                fH.dgvHistory.DataMember = dsTemp.Tables[0].ToString();
                fH.LabCount.Text = dsTemp.Tables[0].Rows.Count.ToString();
                fH.LabWO.Text = sFieldID;
                fH.ShowDialog();
            }
            finally
            {
                fH.Dispose();
            }
            dsTemp.Dispose();
        }

        private void btnAssignMAC_Click(object sender, EventArgs e)
        {
            string sFieldName = "";
            string sSQL = @"Select SQL_NAME from sajet.sys_sql
                where SYSUSE_NAME='MAC REQUEST' ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                sFieldName = dsTemp.Tables[0].Rows[0]["SQL_NAME"].ToString();
            }
            dsTemp.Dispose();
            fAssignMAC f = new fAssignMAC
            {
                g_sFieldName = sFieldName
            };
            f.ShowDialog();
            f.Dispose();
        }

        private string GetWOMACRequest(string sWorkOrder)
        {
            string sSQL = @"SELECT WORK_ORDER,NVL(WO_OPTION7,'N') FLAG FROM SAJET.G_WO_BASE
                WHERE WORK_ORDER =:WORK_ORDER ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWorkOrder };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count == 0)
                return "N/A";
            else
                return dsTemp.Tables[0].Rows[0]["FLAG"].ToString();
        }

        private void MenuMACRequestYes_Click(object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem))
                return;
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;

            string sTag = (sender as ToolStripMenuItem).Tag.ToString();
            string sValue = (sender as ToolStripMenuItem).Text.ToString();
            string sWorkOrder = gvData.CurrentRow.Cells["WORK_ORDER"].Value.ToString();
            string sFlag = GetWOMACRequest(sWorkOrder);
            if (sFlag == "N/A")
                return;
            if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Work Order") + " : " + sWorkOrder + Environment.NewLine
                                       + SajetCommon.SetLanguage("MAC Request Change to") + " : " + SajetCommon.SetLanguage(sValue) + " ?", 2) != DialogResult.Yes)
                return;

            string sMaxID = "";
            string sSQL = @"SELECT * FROM SAJET.G_WO_BASE_TEMP
                WHERE WORK_ORDER = :WORK_ORDER ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWorkOrder };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            DateTime dtSysdate = ClientUtils.GetSysDate();
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                sSQL = @"UPDATE SAJET.G_WO_BASE_TEMP 
                      SET UPDATE_TIME = :UPDATE_TIME 
                         ,FLAG =:FLAG 
                    WHERE WORK_ORDER =:WORK_ORDER ";
                Params = new object[3][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", dtSysdate };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FLAG", sTag };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWorkOrder };
                ClientUtils.ExecuteSQL(sSQL, Params);
            }
            else
            {
                sMaxID = SajetCommon.GetMaxID("SAJET.G_WO_BASE_TEMP", "TXN_ID", 10);
                sSQL = @"INSERT INTO SAJET.G_WO_BASE_TEMP 
                    (TXN_ID,WORK_ORDER,UPDATE_TIME,FLAG ) 
                    VALUES 
                    (:TXN_ID,:WORK_ORDER,:UPDATE_TIME,:FLAG ) ";
                Params = new object[4][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TXN_ID", sMaxID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWorkOrder };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", dtSysdate };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FLAG", sTag };
                ClientUtils.ExecuteSQL(sSQL, Params);
            }
            sSQL = @"Select SQL_NAME from sajet.sys_sql
                where SYSUSE_NAME='MAC REQUEST' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                string sFieldName = dsTemp.Tables[0].Rows[0]["SQL_NAME"].ToString();
                sSQL = string.Format(@"UPDATE SAJET.G_WO_BASE 
                      SET {0}=:FLAG 
                         ,UPDATE_USERID =:UPDATE_USERID 
                         ,UPDATE_TIME =:UPDATE_TIME 
                    WHERE WORK_ORDER =:WORK_ORDER ", sFieldName);
                Params = new object[4][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FLAG", sTag };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", ClientUtils.UserPara1 };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", dtSysdate };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWorkOrder };
                ClientUtils.ExecuteSQL(sSQL, Params);
                fMain.CopyToHistory(sWorkOrder);
                ShowData();
                SetSelectRow(g_sDataSQL, g_Params, gvData, sWorkOrder, TableDefine.gsDef_KeyField);
            }
            dsTemp.Dispose();
        }

        private void MenuMACRequest_DropDownOpened(object sender, EventArgs e)
        {
            string sWorkOrder = gvData.CurrentRow.Cells["WORK_ORDER"].Value.ToString();
            string sFlag = GetWOMACRequest(sWorkOrder);
            MenuMACRequestYes.Enabled = false;
            MenuMACRequestNo.Enabled = false;
            if (sFlag == "N")
            {
                MenuMACRequestYes.Enabled = true;
            }
            if (sFlag == "Y")
                MenuMACRequestNo.Enabled = true;

        }

        private void splitter1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            SajetInifile sini = new SajetInifile();
            sini.WriteIniFile(sIniFile, g_sProgram, g_sFunction + " Splitter", panel2.Width.ToString());
            sini.Dispose();
        }
    }

    #region custom task and resource
    /// <summary>
    /// A custom resource of your own type (optional)
    /// </summary>
    [Serializable]
    public class MyResource
    {
        public string Name { get; set; }
    }
    /// <summary>
    /// A custom task of your own type deriving from the Task interface (optional)
    /// </summary>
    [Serializable]
    public class MyTask : Task
    {
        public MyTask(ProjectManager manager)
            : base()
        {
            Manager = manager;
        }

        private ProjectManager Manager { get; set; }

        public new int Start { get { return base.Start; } set { Manager.SetStart(this, value); } }
        public new int End { get { return base.End; } set { Manager.SetEnd(this, value); } }
        public new int Duration { get { return base.Duration; } set { Manager.SetDuration(this, value); } }
        public new float Complete { get { return base.Complete; } set { Manager.SetComplete(this, value); } }
    }
    #endregion custom task and resource

    public class GRcTravel
    {
        public DateTime RealStart { get; set; }
        public DateTime RealEnd { get; set; }
        public int RcNo { get; set; }
        public int NgQty { get; set; }
    }
}