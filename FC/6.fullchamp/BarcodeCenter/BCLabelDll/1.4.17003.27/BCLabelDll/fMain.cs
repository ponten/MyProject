using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections.Specialized;
using SajetClass;
using System.Data.OracleClient;
using System.Globalization;
using SajetFilter;
using PrintLabel;
using RlSrv = Print_RCList.RCListService;

namespace BCLabelDll
{
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();
        }

        public int iPart_Id = 0, iRoute_Id = 0, iLine_Id = 0, iFactory_Id = 0;
        public int iProcess_Id = 0, iStage_Id = 0;
        public long iNode_Id = 0, iNext_Node = 0;
        public string sVersion = "", sNext_Process = "", sSheet_Name = "";
        public int n = 0;

        public decimal dUnitTotalQty = 0;
        public decimal dTargetQty = 0;
        public decimal dUnitQty = 0;
        public decimal dUnitMod = 0;
        public int iUnit = 0;

        public DateTime datExeTime;
        public long lTravel_Id;

        public int g_iPrivilege = 0;
        public string g_sUserID;
        public string g_sProgram;
        public static string g_sExeName;

        //Config中設定的列印相關方式===========
        public bool g_bSetup_Print;
        public int g_iSetup_PrintQty;
        public string g_sSetup_PrintMethod;
        public string g_sSetup_PrintPort;
        public string g_sSetup_CodeSoftVer;
        public int g_sSetup_iLabelQty;
        //sys_label定義的欄位值========
        public string g_sSeq;
        public string g_sLabType;
        public string g_sFieldName;
        public string g_sTable;
        public string g_sQtyField;
        public string g_sType;
        public string g_sFileName;
        public string g_sLabelFunction;
        int g_iSeqFlag;
        //與Rule有關====================
        public struct TRULE
        {
            public string g_sRule;
            public string g_sFRule;
            public string g_sMark;
            public string g_sCarry;
            public string g_sCheckkSum;
            public int g_iResetCycle;
            public bool g_bReset;
            public bool g_bSeq_Manual;
            public string g_sSeqText;
            public string[] g_CarryM;
            public string[] g_CarryD;
            public string[] g_CarryW;
            public string[] g_CarryDW;
            public int g_iQtyType;
        }
        public static TRULE TWoRule = new TRULE();

        //SYSDATE日期===========================        
        public struct TDATECODE
        {
            public string sDY;
            public string sDM;
            public string sDD;
            public string sDW;
            public string sDYW;
            public string sDK;
            public string scDM;
            public string scDD;
            public string scDW;
            public string scDK;
        }
        public static TDATECODE TSysDateCode = new TDATECODE();

        //rita 2011/03/11     
        public struct TMACRecord
        {
            public string sReleaseType;
            public string sMacPartID;
            public string sMACPartNo;
        }
        public static TMACRecord TMAC = new TMACRecord();

        //=============================        
        int g_iOptionQty;  //設定的連板數(SYS_PART中定義的Option值)
        string g_sDefault;
        string g_sSeqCode; //編碼中屬於流水號的Code
        string sSQL;
        DataSet dsTemp;
        DataSet dsWoData;
        LabelCheck.Check LabelCheckDll;
        static bool g_bPrintSortDesc;
        double g_iReleaseTimes;

        public void check_privilege()
        {
            g_iPrivilege = 0;
            btnRelease.Enabled = false;
            string sSQL = " SELECT PROGRAM,FUNCTION  "
                        + " FROM SAJET.SYS_PROGRAM_FUN "
                        + " WHERE Upper(DLL_FILENAME) = '" + SajetCommon.g_sFileName.ToUpper() + "' "
                        + " AND fun_param = '" + g_sLabType + "' "
                        + " AND ROWNUM=1 ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                string sFunction = dsTemp.Tables[0].Rows[0]["FUNCTION"].ToString();
                g_iPrivilege = ClientUtils.GetPrivilege(g_sUserID, sFunction, g_sProgram);
            }
            btnRelease.Enabled = (g_iPrivilege >= 1);
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            g_sExeName = ClientUtils.fCurrentProject;
            g_sUserID = ClientUtils.UserPara1;
            g_sProgram = ClientUtils.fProgramName;
            //讀取傳入的參數
            g_sLabType = ClientUtils.fParameter;

            //找Label的種類
            sSQL = " select * from sajet.sys_label "
                 + " where label_name = '" + g_sLabType + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                g_sSeq = dsTemp.Tables[0].Rows[0]["SEQ_NAME"].ToString();
                g_sType = dsTemp.Tables[0].Rows[0]["TYPE"].ToString();
                g_sFieldName = dsTemp.Tables[0].Rows[0]["Field_Name"].ToString();
                g_sTable = dsTemp.Tables[0].Rows[0]["Table_Name"].ToString();
                g_sQtyField = dsTemp.Tables[0].Rows[0]["Qty_Field"].ToString();
                g_sFileName = dsTemp.Tables[0].Rows[0]["file_name"].ToString();
                g_sLabelFunction = dsTemp.Tables[0].Rows[0]["function"].ToString();
                g_iSeqFlag = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["Sub_Flag"].ToString());
            }

            LabInput.Text = SajetCommon.SetLanguage("Qty");
            LabStart.Text = SajetCommon.SetLanguage("Start Number");
            if (g_sType == "S") //先只展號碼,後再跟工單關聯的方式
            {
                LabInput.Text = SajetCommon.SetLanguage("Start Number");
                LabStart.Text = SajetCommon.SetLanguage("End Number");

                editDefault.Visible = false;
                LabDefault.Visible = false;
                LabSeq.Visible = false;
                btnAuto.Visible = true;
                gbPrintInfo.Visible = false;
            }
            else
            {
                LabSeq.Visible = true;
                if (g_sType == "Y")
                    LabSeq.Text = SajetCommon.SetLanguage("(Sequence)");
                else
                    LabSeq.Text = SajetCommon.SetLanguage("(All Character)");
                btnAuto.Visible = false;
                gbPrintInfo.Visible = true;
            }

            //Label檔名
            LabDesc.Text = g_sFileName + " + Part No (Default)";
            sSQL = "select file_name from sajet.sys_label "
                 + " where label_name = '" + g_sLabType + "-Box' and rownum = 1 ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                LabDesc.Text = LabDesc.Text + " / " + dsTemp.Tables[0].Rows[0]["file_name"].ToString() + " + Part No (Default)";
            }

            check_privilege();

            //讀取SYS_BASE中設定值
            string sErrMsg = "";
            g_bPrintSortDesc = (SajetCommon.GetSysBaseData(g_sProgram, "Barcode Print Sort", ref sErrMsg) == "Sort Desc");
            if (sErrMsg != "")
            {
                sErrMsg = SajetCommon.SetLanguage("Please Setup System Parameter") + " : " + Environment.NewLine + Environment.NewLine + sErrMsg;
                SajetCommon.Show_Message(sErrMsg, 0);
                btnRelease.Enabled = false;
                return;
            }
            //允許多展Target數量的幾倍
            string sTimes = (SajetCommon.GetSysBaseData(g_sProgram, "Multiples Of Target", ref sErrMsg));
            if (String.IsNullOrEmpty(sTimes))
                sTimes = "1";
            g_iReleaseTimes = Convert.ToDouble(sTimes);


            //0:不做任何限制,可用的MAC即可
            //1:限制工單BOM表內要有"MAC"類型的料號
            TMAC.sReleaseType = (SajetCommon.GetSysBaseData(g_sProgram, "MAC Release Type", ref sErrMsg));
            if (string.IsNullOrEmpty(TMAC.sReleaseType))
                TMAC.sReleaseType = "0";
            TMAC.sMacPartID = "0";
            TMAC.sMACPartNo = "";

            //讀取Config中設定的資料
            LoadConfigData();
            LabelCheckDll = new LabelCheck.Check();

            ClearData();

            TWoRule.g_CarryM = new string[] { "" };
            TWoRule.g_CarryD = new string[] { "" };
            TWoRule.g_CarryW = new string[] { "" };
            TWoRule.g_CarryDW = new string[] { "" };
            SajetCommon.SetLanguageControl(this);
            this.Text = this.Text + " (" + SajetCommon.g_sFileVersion + ")";

            //LoadImage
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            panel1.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
            gbPrintInfo.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            groupBox2.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");


        }

        public void LoadConfigData()
        {
            string sFile = Application.StartupPath + "\\Sajet.ini";
            SajetInifile sajetInifile1 = new SajetInifile();
            string sPrint = sajetInifile1.ReadIniFile(sFile, g_sProgram, "Print " + g_sLabType, "0");

            g_bSetup_Print = (sPrint == "1");
            g_iSetup_PrintQty = Convert.ToInt32(sajetInifile1.ReadIniFile(sFile, g_sProgram, g_sLabType + " Qty", "1"));
            g_sSetup_PrintMethod = sajetInifile1.ReadIniFile(sFile, g_sProgram, "Print " + g_sLabType + " Method", "CodeSoft");
            g_sSetup_PrintPort = sajetInifile1.ReadIniFile(sFile, g_sProgram, g_sLabType + " ComPort", "");
            g_sSetup_CodeSoftVer = sajetInifile1.ReadIniFile(sFile, g_sProgram, "Code Soft Version", "6");
            g_sSetup_iLabelQty = Convert.ToInt32(sajetInifile1.ReadIniFile(sFile, g_sProgram, g_sLabType + " Label Qty", "1"));

            //顯示在畫面上
            if (g_bSetup_Print)
                LabPrint.Text = "Yes";
            else
                LabPrint.Text = "No";
            LabPrintQty.Text = g_iSetup_PrintQty.ToString();
            LabPrintMethod.Text = g_sSetup_PrintMethod;
        }

        public bool Show_WoData(string sWO, bool bRelease, string sRule)
        {
            //工單資料
            sSQL = $@"
SELECT
    a.*,
    b.*,
    c.route_name,
    d.pdline_name,
    e.customer_code,
    e.customer_name,
    f.process_name   start_process,
    g.process_name   end_process,
    nvl(a.pallets, 12) AS unit_qty,
    (
        SELECT
            MAX({g_sFieldName})
        FROM
            {g_sTable}
        WHERE
            work_order = a.work_order
    ) max_number,
    (
        SELECT
            MIN({g_sFieldName})
        FROM
            {g_sTable}
        WHERE
            work_order = a.work_order
    ) min_number
    /* 工單 Release 的最大及最小號碼 */
FROM
    sajet.g_wo_base      a
    LEFT JOIN sajet.sys_part       b ON a.part_id = b.part_id
    LEFT JOIN sajet.sys_rc_route   c ON a.route_id = c.route_id
    LEFT JOIN sajet.sys_pdline     d ON a.default_pdline_id = d.pdline_id
    LEFT JOIN sajet.sys_customer   e ON a.customer_id = e.customer_id
    LEFT JOIN sajet.sys_process    f ON a.start_process_id = f.process_id
    LEFT JOIN sajet.sys_process    g ON a.end_process_id = g.process_id
WHERE
    a.work_order = '{sWO}'
";
            dsWoData = ClientUtils.ExecuteSQL(sSQL);

            if (dsWoData.Tables[0].Rows.Count == 0)
            {
                SajetCommon.Show_Message("Work Order Error", 0);
                editWO.Focus();
                editWO.SelectAll();
                return false;
            }

            LabWO.Text = dsWoData.Tables[0].Rows[0]["WORK_ORDER"].ToString();
            LabPart.Text = dsWoData.Tables[0].Rows[0]["PART_NO"].ToString();
            LabPartDesc.Text = dsWoData.Tables[0].Rows[0]["SPEC1"].ToString();
            LabVer.Text = dsWoData.Tables[0].Rows[0]["VERSION"].ToString();
            LabTargetQty.Text = dsWoData.Tables[0].Rows[0]["TARGET_QTY"].ToString();
            LabInputQty.Text = dsWoData.Tables[0].Rows[0]["INPUT_QTY"].ToString();
            if (string.IsNullOrEmpty(dsWoData.Tables[0].Rows[0]["WO_DUE_DATE"].ToString()))
                LabWoDueDate.Text = "";
            else
                LabWoDueDate.Text = ((DateTime)dsWoData.Tables[0].Rows[0]["WO_DUE_DATE"]).ToString("yyyy/MM/dd");
            LabPO.Text = dsWoData.Tables[0].Rows[0]["PO_NO"].ToString();
            LabCustomer.Text = dsWoData.Tables[0].Rows[0]["CUSTOMER_NAME"].ToString();
            LabRemark.Text = dsWoData.Tables[0].Rows[0]["REMARK"].ToString();
            LabWoType.Text = dsWoData.Tables[0].Rows[0]["WO_TYPE"].ToString();
            LabRoute.Text = dsWoData.Tables[0].Rows[0]["ROUTE_NAME"].ToString();
            LabLine.Text = dsWoData.Tables[0].Rows[0]["PDLINE_NAME"].ToString();
            LabMasterWO.Text = dsWoData.Tables[0].Rows[0]["MASTER_WO"].ToString();
            LabStartProcess.Text = dsWoData.Tables[0].Rows[0]["START_PROCESS"].ToString();
            LabEndProcess.Text = dsWoData.Tables[0].Rows[0]["END_PROCESS"].ToString();
            LabReleaseMin.Text = dsWoData.Tables[0].Rows[0]["MIN_NUMBER"].ToString();
            LabReleaseMax.Text = dsWoData.Tables[0].Rows[0]["MAX_NUMBER"].ToString();

            // 2016.5.12 By Jason
            //int iWoInputQty = Convert.ToInt32(dsWoData.Tables[0].Rows[0]["INPUT_QTY"].ToString());
            decimal iWoInputQty = Convert.ToDecimal(dsWoData.Tables[0].Rows[0]["INPUT_QTY"].ToString());
            // 2016.5.12 End
            int iWoTargetQty = Convert.ToInt32(dsWoData.Tables[0].Rows[0]["TARGET_QTY"].ToString());

            // 2016.5.27 By Jason
            iPart_Id = 0;
            iRoute_Id = 0;
            iLine_Id = 0;
            iFactory_Id = 0;
            iProcess_Id = 0;
            iStage_Id = 0;
            iNode_Id = 0;
            iNext_Node = 0;
            sVersion = "";
            sNext_Process = "";
            sSheet_Name = "";

            sSQL = " SELECT A.PART_ID,A.VERSION,A.ROUTE_ID,A.DEFAULT_PDLINE_ID,A.FACTORY_ID"
                 + "   FROM SAJET.G_WO_BASE A,SAJET.SYS_PDLINE B"
                 + "  WHERE A.DEFAULT_PDLINE_ID = B.PDLINE_ID"
                 + "    AND WORK_ORDER = '" + sWO + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                iPart_Id = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["PART_ID"].ToString());
                sVersion = dsTemp.Tables[0].Rows[0]["VERSION"].ToString();
                iRoute_Id = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["ROUTE_ID"].ToString());
                iLine_Id = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["DEFAULT_PDLINE_ID"].ToString());
                iFactory_Id = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["FACTORY_ID"].ToString());
            }

            object[][] ParamsProc = new object[7][];
            ParamsProc[0] = new object[] { ParameterDirection.Input, OracleType.Number, "prouteid", iRoute_Id };
            ParamsProc[1] = new object[] { ParameterDirection.Output, OracleType.VarChar, "vprocessid", "" };
            ParamsProc[2] = new object[] { ParameterDirection.Output, OracleType.VarChar, "vnextprocess", "" };
            ParamsProc[3] = new object[] { ParameterDirection.Output, OracleType.VarChar, "vsheetname", "" };
            ParamsProc[4] = new object[] { ParameterDirection.Output, OracleType.VarChar, "vstageid", "" };
            ParamsProc[5] = new object[] { ParameterDirection.Output, OracleType.VarChar, "vnodeid", "" };
            ParamsProc[6] = new object[] { ParameterDirection.Output, OracleType.VarChar, "vnextnode", "" };
            DataSet DSProc = ClientUtils.ExecuteProc("SAJET.SJ_GET_FIRSTPROCESS", ParamsProc);

            iProcess_Id = Convert.ToInt32(DSProc.Tables[0].Rows[0]["vprocessid"].ToString());

            if (DSProc.Tables[0].Rows[0]["vnextprocess"].ToString() != "0")
            {
                sNext_Process = DSProc.Tables[0].Rows[0]["vnextprocess"].ToString();
            }

            sSheet_Name = DSProc.Tables[0].Rows[0]["vsheetname"].ToString();
            iStage_Id = Convert.ToInt32(DSProc.Tables[0].Rows[0]["vstageid"].ToString());
            iNode_Id = Convert.ToInt64(DSProc.Tables[0].Rows[0]["vnodeid"].ToString());
            iNext_Node = Convert.ToInt64(DSProc.Tables[0].Rows[0]["vnextnode"].ToString());
            // 2016.5.27 End

            // 2016.5.12 By Jason (防呆)
            if (LabTargetQty.Text == LabInputQty.Text)
            {
                editDefault.Enabled = false;
                editQty.Enabled = false;
                editStart.Enabled = false;
                editUnitQty.Enabled = false;
            }
            else
            {
                editDefault.Enabled = true;
                editQty.Enabled = false;
                editStart.Enabled = true;
                editUnitQty.Enabled = true;
            }
            // 2016.5.12 End

            //計算已展的數量
            sSQL = " Select Count(*) CNT "
                 + " From " + g_sTable
                 + " Where WORK_ORDER = '" + sWO + "'";
            if (g_sLabType.ToUpper() == "SERIAL NUMBER")
                sSQL = sSQL + " AND PROCESS_ID = 0 ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            LabReleaseQty.Text = dsTemp.Tables[0].Rows[0]["CNT"].ToString();
            if (g_sLabType.ToUpper() == "SERIAL NUMBER")
            {
                iWoInputQty = iWoInputQty + Convert.ToInt32(dsTemp.Tables[0].Rows[0]["CNT"].ToString());
                LabReleaseQty.Text = iWoInputQty.ToString();
                editQty.Text = Convert.ToString(iWoTargetQty - iWoInputQty);
            }
            // 2016.5.12 By Jason (系統帶出預設展開數量)
            else if (g_sLabType.ToUpper() == "RC NO")
            {
                // 2016.6.23 By Jason

                //dTargetQty = 0;
                //dUnitQty = 0;
                //dUnitMod = 0;
                //iUnit = 0;

                //dTargetQty = Convert.ToDecimal(LabTargetQty.Text) - Convert.ToDecimal(LabInputQty.Text);
                //dUnitQty = Convert.ToDecimal(dsWoData.Tables[0].Rows[0]["OPTION1"].ToString());
                //dUnitMod = dTargetQty % dUnitQty;
                //iUnit = Convert.ToInt32(Math.Floor(dTargetQty / dUnitQty));

                //editQty.Text = iUnit.ToString();

                dTargetQty = 0;
                dUnitQty = 0;
                dUnitMod = 0;
                iUnit = 0;

                dTargetQty = Convert.ToDecimal(LabTargetQty.Text) - Convert.ToDecimal(LabInputQty.Text);

                // 使用開工單 "棧板" 欄位設定的數量為單位，展開流程卡

                string s = $@"
SELECT
    nvl(pallets, 12) unit_qty
FROM
    sajet.g_wo_base
WHERE
    work_order = '{sWO}'
";
                var d = ClientUtils.ExecuteSQL(s);

                decimal.TryParse(d.Tables[0].Rows[0]["UNIT_QTY"].ToString(), out dUnitQty);
                //*/

                decimal.TryParse(dsWoData.Tables[0].Rows[0]["UNIT_QTY"].ToString(), out dUnitQty);

                // 流程卡預設用一張 12 個產品計算
                if (dUnitQty <= 0)
                {
                    dUnitQty = 12;
                }

                dUnitMod = dTargetQty % dUnitQty;
                // 2016.7.15 By Jason
                iUnit = Convert.ToInt32(Math.Ceiling(dTargetQty / dUnitQty));
                // 2016.7.15 End

                if (dUnitQty > dTargetQty)
                {
                    editQty.Text = "1";
                    editUnitQty.Text = dTargetQty.ToString();
                }
                else
                {
                    editQty.Text = iUnit.ToString();
                    editUnitQty.Text = dUnitQty.ToString();
                }
                // 2016.6.23 End
            }
            // 2016.5.12 End
            else
                editQty.Text = "0";

            //設定的連板數(SYS_PART中的OPTION欄位)
            g_iOptionQty = 0;
            if (!string.IsNullOrEmpty(g_sQtyField))
            {
                if (!string.IsNullOrEmpty(dsWoData.Tables[0].Rows[0][g_sQtyField].ToString()))
                    g_iOptionQty = Convert.ToInt32(dsWoData.Tables[0].Rows[0][g_sQtyField].ToString());
            }
            lablSNCount.Visible = (g_iOptionQty != 0);
            lablSub.Visible = lablSNCount.Visible;
            editSNCount.Visible = lablSNCount.Visible;
            editSNCount.Text = g_iOptionQty.ToString();

            //算可展的大板數量
            if (g_iOptionQty > 0)
            {
                lablSNCount.Text = g_iOptionQty.ToString();
                if (g_sLabType.ToUpper() == "SERIAL NUMBER")
                {
                    editQty.Text = Convert.ToString(Convert.ToInt32(editQty.Text) / g_iOptionQty);
                }
            }

            //檢查數量是否已滿
            int iQty = 1;
            if ((g_sType == "Y") || (editSNCount.Text == "0"))
                iQty = 1;
            else
                iQty = Convert.ToInt32(editSNCount.Text);
            //int iMaxQty = (Convert.ToInt32(LabTargetQty.Text) * iQty * g_iReleaseTimes)

            // 2016.6.23 By Jason
            //if (Convert.ToInt32(LabReleaseQty.Text) >= Convert.ToInt32(LabTargetQty.Text) * iQty * g_iReleaseTimes)
            if (Convert.ToDouble(LabInputQty.Text) >= Convert.ToInt32(LabTargetQty.Text) * iQty * g_iReleaseTimes)
            // 2016.6.23 End
            {
                // 2016.6.23 By Jason
                //string sErrMsg = SajetCommon.SetLanguage("Release Qty") + " = " + SajetCommon.SetLanguage("Target Qty");
                string sErrMsg = SajetCommon.SetLanguage("Input Qty") + " = " + SajetCommon.SetLanguage("Target Qty");
                // 2016.6.23 End
                if (bRelease)
                    SajetCommon.Show_Message(sErrMsg, -1);
                else
                    SajetCommon.Show_Message(sErrMsg, 1);
                editQty.Text = "";
                editStart.Text = "";
                editWO.Focus();
                return false;
            }

            //讀取編碼規則
            if (!Show_Rule(sWO, bRelease, sRule))
                return false;

            //1.0.0.18 add
            if (g_sLabType.ToUpper() == "SERIAL NUMBER")
            {
                if (TWoRule.g_iQtyType == 1)
                    editQty.Text = "0";
            }

            Get_SysDateCode();
            //計算起始號碼
            Get_StartNumber();
            Get_Default(sWO, bRelease);
            return true;
        }

        public bool Show_Rule(string sWO, bool bRelease, string sRule)
        {
            LVFun.Items.Clear();
            LVUserSeq.Items.Clear();

            editDefault.Mask = "";

            if (g_sLabType.ToUpper() == "MAC" && g_sType == "S" && TMAC.sReleaseType == "1")
            {
                //由MAC料號定義的RuleSet中找使用的MAC RULE
                string sRuleSet = "";
                sSQL = "select rule_set from sajet.sys_part "
                     + "where part_id='" + TMAC.sMacPartID + "'";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (!string.IsNullOrEmpty(dsTemp.Tables[0].Rows[0]["rule_set"].ToString()))
                {
                    sRuleSet = dsTemp.Tables[0].Rows[0]["rule_set"].ToString();
                }
                if (string.IsNullOrEmpty(sRuleSet))
                {
                    string sMsg = SajetCommon.SetLanguage("Rule not Define") + Environment.NewLine
                                + SajetCommon.SetLanguage(g_sLabType) + Environment.NewLine
                                + SajetCommon.SetLanguage("Part No") + ":" + TMAC.sMACPartNo;
                    SajetCommon.Show_Message(sMsg, 0);
                    return false;
                }
                sSQL = "Select parame_item from sajet.sys_module_param "
                     + "where module_name='W/O RULE' "
                     + "and function_name='" + sRuleSet + "' "
                     + "and parame_name='MAC Rule'";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                string sRuleName = dsTemp.Tables[0].Rows[0]["parame_item"].ToString();

                sSQL = "Select a.*,b.RULE_NAME FUNCTION_NAME "
                     + "from sajet.sys_rule_param a,sajet.sys_rule_name b "
                     + "where b.rule_name='" + sRuleName + "' "
                     + "and b.rule_type='" + g_sLabType.ToUpper() + "' "
                     + "and a.rule_id = b.rule_id ";
            }
            else
            {
                sSQL = " Select * From SAJET.G_WO_PARAM "
                     + " Where WORK_ORDER = '" + sWO + "' "
                     + " and MODULE_NAME = '" + g_sLabType.ToUpper() + " RULE'";
            }
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                string sMsg = SajetCommon.SetLanguage("Rule not Define") + Environment.NewLine
                            + SajetCommon.SetLanguage(g_sLabType);
                SajetCommon.Show_Message(sMsg, 0);
                return false;
            }
            LabRuleName.Text = dsTemp.Tables[0].Rows[0]["FUNCTION_NAME"].ToString();
            TWoRule.g_sRule = LabRuleName.Text;
            TWoRule.g_sFRule = "";
            if (sRule == "")
                TWoRule.g_sFRule = TWoRule.g_sRule;
            else
                TWoRule.g_sFRule = sRule;

            TWoRule.g_iQtyType = 0;

            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                string sParam_Name = dsTemp.Tables[0].Rows[i]["PARAME_NAME"].ToString();
                string sParam_Item = dsTemp.Tables[0].Rows[i]["PARAME_ITEM"].ToString();
                string sParam_Value = dsTemp.Tables[0].Rows[i]["PARAME_VALUE"].ToString();
                //編碼規則與Default值
                if (sParam_Name == g_sLabType + " Code")
                {
                    if (sParam_Item == "Code")
                        LabCode.Text = sParam_Value;
                    else if ((sParam_Item == "Default") && (!bRelease))
                        editDefault.Text = sParam_Value;
                    else if (sParam_Item == "Code Type")
                        TWoRule.g_sCarry = sParam_Value;
                    continue;
                }
                //自行定義的日期代碼
                switch (sParam_Name)
                {
                    case "Month User Define":
                        TWoRule.g_CarryM = sParam_Value.Split(new Char[] { ',' });
                        LVCheck.Items.Add("m");
                        LVCheck.Items[LVCheck.Items.Count - 1].SubItems.Add(sParam_Value);
                        continue;
                    case "Day User Define":
                        TWoRule.g_CarryD = sParam_Value.Split(new Char[] { ',' }); ;
                        LVCheck.Items.Add("d");
                        LVCheck.Items[LVCheck.Items.Count - 1].SubItems.Add(sParam_Value);
                        continue;
                    case "Week User Define":
                        TWoRule.g_CarryW = sParam_Value.Split(new Char[] { ',' }); ;
                        LVCheck.Items.Add("w");
                        LVCheck.Items[LVCheck.Items.Count - 1].SubItems.Add(sParam_Value);
                        continue;
                    case "Day of Week User Define":
                        TWoRule.g_CarryDW = sParam_Value.Split(new Char[] { ',' }); ;
                        LVCheck.Items.Add("k");
                        LVCheck.Items[LVCheck.Items.Count - 1].SubItems.Add(sParam_Value);
                        continue;
                    case "Check Sum":
                        TWoRule.g_sCheckkSum = sParam_Value;
                        continue;
                    case "Reset Sequence":
                        TWoRule.g_iResetCycle = Convert.ToInt32(sParam_Value);
                        TWoRule.g_bReset = (sParam_Item == "1");
                        continue;
                    case "Sequence Mode":
                        TWoRule.g_bSeq_Manual = (sParam_Value == "Manual");
                        continue;
                    case "QTY":
                        TWoRule.g_iQtyType = Convert.ToInt32(sParam_Value);
                        continue;
                }
                //自行定義的Function                
                if (sParam_Name.IndexOf("Digit Type & Field") != -1)
                {
                    LVFun.Items.Add(sParam_Name.Substring(0, 1));
                    LVFun.Items[LVFun.Items.Count - 1].SubItems.Add(sParam_Item);
                    LVFun.Items[LVFun.Items.Count - 1].SubItems.Add(sParam_Value);
                    continue;
                }
                //自行定義的流水號進位方式
                if (sParam_Name == g_sLabType + " User Define")
                {
                    LVUserSeq.Items.Add(sParam_Item);
                    LVUserSeq.Items[LVUserSeq.Items.Count - 1].SubItems.Add(sParam_Value);
                    LVUserSeq.Items[LVUserSeq.Items.Count - 1].Name = sParam_Item;
                    continue;
                }
            }

            //最後一次Reset Sequence時間
            GetSequenceTime();
            //TWoRule.g_sMark = "";
            //sSQL = " Select PARAME_VALUE From SAJET.SYS_MODULE_PARAM "
            //     + " Where MODULE_NAME = '" + g_sSeq.ToUpper() + "' "
            //     + " and FUNCTION_NAME ='" + TWoRule.g_sRule.ToUpper() + "' "
            //     + " and PARAME_NAME = 'Reset Sequence Mark' ";
            //if (TWoRule.g_sFRule != "")
            //    sSQL = sSQL + " and PARAME_ITEM = '" + TWoRule.g_sFRule.ToUpper() + "' ";
            //else
            //    sSQL = sSQL + " and PARAME_ITEM is Null ";
            //dsTemp = ClientUtils.ExecuteSQL(sSQL);
            //if (dsTemp.Tables[0].Rows.Count>0)
            //    TWoRule.g_sMark = dsTemp.Tables[0].Rows[0]["PARAME_VALUE"].ToString();

            //代表流水號的字母
            TWoRule.g_sSeqText = "S";
            for (int i = 0; i <= LVUserSeq.Items.Count - 1; i++)
            {
                TWoRule.g_sSeqText = TWoRule.g_sSeqText + LVUserSeq.Items[i].Text;
            }

            g_sSeqCode = "";
            for (int i = 0; i <= LabCode.Text.Length - 1; i++)
            {
                if (TWoRule.g_sSeqText.IndexOf(LabCode.Text.Substring(i, 1)) > -1)
                    g_sSeqCode = g_sSeqCode + LabCode.Text.Substring(i, 1);
            }

            editStart.Visible = !(TWoRule.g_bSeq_Manual && g_sSeqCode.Length == 0);
            LabStart.Visible = editStart.Visible;
            LabSeq.Visible = LabStart.Visible;

            btnRelease.Enabled = (g_iPrivilege >= 1);
            return true;
        }

        public void Get_SysDateCode()
        {
            //日期格式=============================
            sSQL = " Select TO_CHAR(SYSDATE,'YYYY/MM/DD/IW/DDD/D') YMD, sysdate From DUAL ";
            if ((LabCode.Text.ToUpper().IndexOf("W") != -1) && (LabCode.Text.ToUpper().IndexOf("M") == -1) && (LabCode.Text.ToUpper().IndexOf("D") == -1))
                sSQL = " Select TO_CHAR(SYSDATE,'IYYY/MM/DD/IW/DDD/D') YMD, sysdate From DUAL ";

            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            string mDateCode = dsTemp.Tables[0].Rows[0]["YMD"].ToString();
            string mSysDate = dsTemp.Tables[0].Rows[0]["sysdate"].ToString();
            TSysDateCode.sDY = mDateCode.Substring(0, 4);
            TSysDateCode.sDM = mDateCode.Substring(5, 2);
            TSysDateCode.sDD = mDateCode.Substring(8, 2);
            TSysDateCode.sDW = mDateCode.Substring(11, 2);
            TSysDateCode.sDYW = mDateCode.Substring(14, 3); //Day Of Year
            TSysDateCode.sDK = mDateCode.Substring(18, 1);  //Day Of Week

            //對應的User Define Day Code
            TSysDateCode.scDM = "";
            if (TWoRule.g_CarryM[0].ToString() != "" && TWoRule.g_CarryM.Length >= Convert.ToInt32(TSysDateCode.sDM))
                TSysDateCode.scDM = TWoRule.g_CarryM[Convert.ToInt32(TSysDateCode.sDM) - 1].ToString();
            TSysDateCode.scDD = "";
            if (TWoRule.g_CarryD[0].ToString() != "" && TWoRule.g_CarryD.Length >= Convert.ToInt32(TSysDateCode.sDD))
                TSysDateCode.scDD = TWoRule.g_CarryD[Convert.ToInt32(TSysDateCode.sDD) - 1].ToString();
            TSysDateCode.scDW = "";
            if (TWoRule.g_CarryW[0].ToString() != "" && TWoRule.g_CarryW.Length >= Convert.ToInt32(TSysDateCode.sDW))
                TSysDateCode.scDW = TWoRule.g_CarryW[Convert.ToInt32(TSysDateCode.sDW) - 1].ToString();
            TSysDateCode.scDK = "";
            if (TWoRule.g_CarryDW[0].ToString() != "" && TWoRule.g_CarryDW.Length >= Convert.ToInt32(TSysDateCode.sDK))
                TSysDateCode.scDK = TWoRule.g_CarryDW[Convert.ToInt32(TSysDateCode.sDK) - 1].ToString();
        }

        public void Get_Default(string sWO, bool bRelease)
        {
            //轉換非流水號部分=====
            //若有Function,找出實際Function值
            for (int i = 0; i <= LVFun.Items.Count - 1; i++)
            {
                string sField = LVFun.Items[i].SubItems[1].Text;
                string sFunction = LVFun.Items[i].SubItems[2].Text;
                string sData = "N/A";
                if (sField != "N/A")
                    sData = dsWoData.Tables[0].Rows[0][sField].ToString();
                sSQL = " select " + sFunction + "('" + sData + "') fundata from dual ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                string sValue = dsTemp.Tables[0].Rows[0]["fundata"].ToString();
                LVFun.Items[i].SubItems.Add(sValue);
            }

            //日期與固定碼                                
            string mDY = TSysDateCode.sDY;
            string mDM = TSysDateCode.sDM;
            string mDD = TSysDateCode.sDD;
            string mDW = TSysDateCode.sDW;
            string mDYW = TSysDateCode.sDYW;
            string mDK = TSysDateCode.sDK;
            string mcDM = TSysDateCode.scDM;
            string mcDD = TSysDateCode.scDD;
            string mcDW = TSysDateCode.scDW;
            string mcDK = TSysDateCode.scDK;

            string sCode = LabCode.Text;
            string sDefault = editDefault.Text;
            string sMask = "";
            for (int i = sCode.Length - 1; i >= 0; i--)
            {
                string sReplace = "0";
                bool bFunction = false;
                //Function
                for (int j = 0; j <= LVFun.Items.Count - 1; j++)
                {
                    if (sCode[i].ToString() == LVFun.Items[j].Text)
                    {
                        string sValue = LVFun.Items[j].SubItems[3].Text;
                        bFunction = true;
                        if (sValue.Length > 0)
                        {
                            sReplace = sValue[sValue.Length - 1].ToString();
                            //若為數字,Mask格式需為9(數字)
                            try
                            {
                                Convert.ToInt32(sReplace);
                                sMask = "9" + sMask;
                            }
                            catch
                            {
                                sMask = "C" + sMask;
                            }
                            sValue = sValue.Substring(0, sValue.Length - 1);
                            LVFun.Items[j].SubItems[3].Text = sValue;
                        }
                        else
                        {
                            sReplace = "0";
                            sMask = "9" + sMask;
                        }
                        break;
                    }
                }

                // 
                if (!bFunction)
                {
                    switch (sCode[i].ToString())
                    {
                        case "Y":
                            sMask = "0" + sMask;
                            if (mDY.Length > 0)
                            {
                                sReplace = mDY[mDY.Length - 1].ToString();
                                mDY = mDY.Substring(0, mDY.Length - 1);
                            }
                            break;
                        case "M":
                            sMask = "0" + sMask;
                            if (mDM.Length > 0)
                            {
                                sReplace = mDM[mDM.Length - 1].ToString();
                                mDM = mDM.Substring(0, mDM.Length - 1);
                            }
                            break;
                        case "D":
                            sMask = "0" + sMask;
                            if (mDD.Length > 0)
                            {
                                sReplace = mDD[mDD.Length - 1].ToString();
                                mDD = mDD.Substring(0, mDD.Length - 1);
                            }
                            break;
                        case "W":
                            sMask = "0" + sMask;
                            if (mDW.Length > 0)
                            {
                                sReplace = mDW[mDW.Length - 1].ToString();
                                mDW = mDW.Substring(0, mDD.Length - 1);
                            }
                            break;
                        case "F":
                            sMask = "0" + sMask;
                            if (mDYW.Length > 0)
                            {
                                sReplace = mDYW[mDYW.Length - 1].ToString();
                                mDYW = mDYW.Substring(0, mDYW.Length - 1);
                            }
                            break;
                        case "K":
                            sMask = "0" + sMask;
                            if (mDK.Length > 0)
                            {
                                sReplace = mDK[mDK.Length - 1].ToString();
                                mDK = mDK.Substring(0, mDK.Length - 1);
                            }
                            break;
                        case "m":
                            sMask = "A" + sMask;
                            if (mcDM.Length > 0)
                            {
                                sReplace = mcDM[mcDM.Length - 1].ToString();
                                mcDM = mcDM.Substring(0, mcDM.Length - 1);
                            }
                            break;
                        case "d":
                            sMask = "A" + sMask;
                            if (mcDD.Length > 0)
                            {
                                sReplace = mcDD[mcDD.Length - 1].ToString();
                                mcDD = mcDD.Substring(0, mcDD.Length - 1);
                            }
                            break;
                        case "w":
                            sMask = "A" + sMask;
                            if (mcDW.Length > 0)
                            {
                                sReplace = mcDW[mcDW.Length - 1].ToString();
                                mcDW = mcDW.Substring(0, mcDW.Length - 1);
                            }
                            break;
                        case "k":
                            sMask = "A" + sMask;
                            if (mcDK.Length > 0)
                            {
                                sReplace = mcDK[mcDK.Length - 1].ToString();
                                mcDK = mcDK.Substring(0, mcDK.Length - 1);
                            }
                            break;
                        case "L":
                            sMask = "L" + sMask;
                            sReplace = sDefault[i].ToString();
                            break;
                        case "9":
                            sMask = "9" + sMask;
                            sReplace = sDefault[i].ToString();
                            break;
                        case "C":
                            sMask = "C" + sMask;
                            sReplace = sDefault[i].ToString();
                            break;
                        default:
                            sMask = "L" + sMask;
                            sReplace = sDefault[i].ToString();
                            break;
                    }
                }
                sDefault = sDefault.Remove(i, 1);
                sDefault = sDefault.Insert(i, sReplace);
            }

            editDefault.Mask = sMask;
            editDefault.Text = sDefault;
            if (!bRelease)
                g_sDefault = sDefault;
            if (g_sType != "S")
                editDefault.ReadOnly = !(g_sDefault.IndexOf(" ") > -1);
        }

        public void Get_StartNumber()
        {
            //計算此次起始號碼===============================            
            if (g_sType == "S") //先展再與工單關聯
            {
                editStart.Enabled = true;
                editDefault.ReadOnly = true;
                editQty.Text = "";
                editStart.Text = "";
            }
            else if (TWoRule.g_bSeq_Manual) //手動輸入時需自行決定起始號
            {
                editStart.Text = Convert.ToString(1).PadLeft(g_sSeqCode.Length, '0');
                editStart.Enabled = true;
            }
            else if (g_sType == "Y") //使用Sequence方式展
            {
                //算流水號
                sSQL = " select Last_Number from all_sequences "
                     + " where sequence_name ='" + g_sSeq + TWoRule.g_sFRule.ToUpper() + "'"
                     + " and sequence_owner = user ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    Int64 iStart = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["Last_Number"].ToString());
                    bool bRS = Reset_Seq(TWoRule.g_bReset);
                    editStart.Enabled = bRS;
                    if (bRS)
                    {
                        editStart.Text = LabelCheckDll.SeqTran(1, LabCode.Text, TWoRule.g_sSeqText, TWoRule.g_sCarry, LVUserSeq);
                    }
                    else
                    {
                        editStart.Text = LabelCheckDll.SeqTran(iStart, LabCode.Text, TWoRule.g_sSeqText, TWoRule.g_sCarry, LVUserSeq);
                    }
                }
                else
                {
                    editStart.Enabled = true;
                    editStart.Text = LabelCheckDll.SeqTran(1, LabCode.Text, TWoRule.g_sSeqText, TWoRule.g_sCarry, LVUserSeq);
                    Reset_Seq(false);
                }
            }
        }

        public bool Reset_Seq(bool b_SeqExist)
        {
            //最後一次Reset Sequence日期
            //Reset Seq   
            string sCode = LabCode.Text;
            bool b_Reset = false;
            switch (TWoRule.g_iResetCycle)
            {
                case 0: //Reset By Day
                    if (sCode.ToUpper().IndexOf("D") > -1)
                    {
                        if (TWoRule.g_sMark == "" || TWoRule.g_sMark != TSysDateCode.sDD)
                        {
                            TWoRule.g_sMark = TSysDateCode.sDD;
                            b_Reset = true;
                        }
                    }
                    else if (sCode.ToUpper().IndexOf("K") > -1)
                    {
                        if (TWoRule.g_sMark == "" || TWoRule.g_sMark != TSysDateCode.sDK)
                        {
                            TWoRule.g_sMark = TSysDateCode.sDK;
                            b_Reset = true;
                        }
                    }
                    else if (sCode.ToUpper().IndexOf("F") > -1) //1.0.0.18 Add
                    {
                        if (TWoRule.g_sMark == "" || TWoRule.g_sMark != TSysDateCode.sDYW)
                        {
                            TWoRule.g_sMark = TSysDateCode.sDYW;
                            b_Reset = true;
                        }
                    }
                    else
                    {
                        if (TWoRule.g_sMark == "" || TWoRule.g_sMark != TSysDateCode.scDD) //User Define
                        {
                            TWoRule.g_sMark = TSysDateCode.scDD;
                            b_Reset = true;
                        }
                    }
                    break;
                case 1: //Reset By Week
                    if (sCode.ToUpper().IndexOf("W") > -1)
                    {
                        if (TWoRule.g_sMark == "" || TWoRule.g_sMark != TSysDateCode.sDW)
                        {
                            TWoRule.g_sMark = TSysDateCode.sDW;
                            b_Reset = true;
                        }
                    }
                    else
                    {
                        if (TWoRule.g_sMark == "" || TWoRule.g_sMark != TSysDateCode.scDW)
                        {
                            TWoRule.g_sMark = TSysDateCode.scDW;
                            b_Reset = true;
                        }
                    }
                    break;
                case 2: //Reset By Month
                    if (sCode.ToUpper().IndexOf("M") > -1)
                    {
                        if (TWoRule.g_sMark == "" || TWoRule.g_sMark != TSysDateCode.sDM)
                        {
                            TWoRule.g_sMark = TSysDateCode.sDM;
                            b_Reset = true;
                        }
                    }
                    else
                    {
                        if (TWoRule.g_sMark == "" || TWoRule.g_sMark != TSysDateCode.scDM)
                        {
                            TWoRule.g_sMark = TSysDateCode.scDM;
                            b_Reset = true;
                        }
                    }
                    break;
                case 3: //Reset By Year
                    if (TWoRule.g_sMark == "" || TWoRule.g_sMark != TSysDateCode.sDY)
                    {
                        TWoRule.g_sMark = TSysDateCode.sDY;
                        b_Reset = true;
                    }
                    break;
            }

            if (b_SeqExist && b_Reset)
            {
                string sSQL = "Drop Sequence " + g_sSeq + TWoRule.g_sFRule;
                DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            }
            return b_Reset;

        }

        public bool CheckRule(string sLabel, string sDefault, string sCode, ListView LV1)
        {
            //檢查產生的號碼是否符合規則
            //呼叫外部DLL(.dll)            
            string sResult = LabelCheckDll.CheckRule(sLabel, sDefault, sCode, TWoRule.g_sCarry, LV1, false);
            if (sResult == "OK")
                return true;
            else
            {
                SajetCommon.Show_Message(sResult, 0);
                return false;
            }
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                DateTime dtStart = ClientUtils.GetSysDate();

                progressBar1.Visible = false;

                if (LabWO.Text == "" || LabWO.Text == "N/A")
                    return;

                if (editDefault.Text == "")
                    return;

                if (g_sType != "S")
                {
                    int iNum = 0;
                    if (!int.TryParse(editQty.Text, out iNum))
                    {
                        string sMsg = SajetCommon.SetLanguage("Value must be the integer") + Environment.NewLine + LabInput.Text;
                        SajetCommon.Show_Message(sMsg, 0);
                        editQty.Focus();
                        return;
                    }
                    if (iNum <= 0)
                    {
                        string sMsg = SajetCommon.SetLanguage("Value must be bigger than 0") + Environment.NewLine + LabInput.Text;
                        SajetCommon.Show_Message(sMsg, 0);
                        editQty.Focus();
                        return;
                    }

                    // 2016.6.23 By Jason
                    decimal dNum = 0;
                    if (!int.TryParse(editUnitQty.Text, out iNum))
                    {
                        if (!decimal.TryParse(editUnitQty.Text, out dNum))
                        {
                            string sMsg = SajetCommon.SetLanguage("Value must be the integer") + Environment.NewLine + LabUnit.Text;
                            SajetCommon.Show_Message(sMsg, 0);
                            editUnitQty.Focus();
                            return;
                        }
                        else
                        {
                            string s_inputQty = editUnitQty.Text.Trim();

                            if (s_inputQty.Length > s_inputQty.IndexOf(".") + 3)
                            {
                                string sMsg = SajetCommon.SetLanguage("Value must be the second decimal place") + Environment.NewLine + LabUnit.Text;
                                SajetCommon.Show_Message(sMsg, 0);
                                editUnitQty.Focus();
                                return;
                            }
                        }
                    }
                    if (iNum <= 0)
                    {
                        if (dNum <= 0)
                        {
                            string sMsg = SajetCommon.SetLanguage("Value must be bigger than 0") + Environment.NewLine + LabUnit.Text;
                            SajetCommon.Show_Message(sMsg, 0);
                            editUnitQty.Focus();
                            return;
                        }
                    }
                    // 2016.6.23 End
                }

                // 2016.5.27 By Jason
                if (g_sLabType.ToUpper() == "RC NO" && Convert.ToDecimal(editUnitQty.Text) > dTargetQty)
                {
                    editUnitQty.Text = dTargetQty.ToString();

                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Unit Qty Cant't More Than ") + editUnitQty.Text, 0);
                    editUnitQty.Focus();
                    return;
                }
                // 2016.5.27 End

                // 2016.6.23 By Jason
                dUnitQty = Convert.ToDecimal(editUnitQty.Text);
                dUnitMod = dTargetQty % dUnitQty;
                // 2016.7.15 By Jason
                //iUnit = Convert.ToInt32(Math.Floor(dTargetQty / dUnitQty));
                iUnit = Convert.ToInt32(Math.Ceiling(dTargetQty / dUnitQty));
                // 2016.7.15 End

                if (dUnitQty > dTargetQty)
                {
                    iUnit = 1;
                    editQty.Text = "1";
                    editUnitQty.Text = dTargetQty.ToString();
                }
                else
                {
                    editQty.Text = iUnit.ToString();
                    editUnitQty.Text = dUnitQty.ToString();
                }
                // 2016.6.23 End

                Int64 iFirstStart = 1;
                if (g_sType == "Y" && !TWoRule.g_bSeq_Manual)
                    iFirstStart = LabelCheckDll.SeqCode(editStart.Text, LabCode.Text, TWoRule.g_sSeqText, TWoRule.g_sCarry, LVUserSeq, "S");

                //===先 Refresh 工單畫面資料,避免多台電腦開啟同張工單而多展序號
                string sQtyTemp = editQty.Text;
                string sTempStart = editStart.Text;
                string sTempSNCount = editSNCount.Text;
                string sTempQty = LabReleaseQty.Text;
                if (!btnRelease.Enabled)
                    return;
                panel3.Enabled = false;

                if (g_sType != "S")
                {
                    if (LabReleaseQty.Text != sTempQty)
                    {
                        string sMsg = SajetCommon.SetLanguage("Release Qty Different") + "!!" + Environment.NewLine
                                    + SajetCommon.SetLanguage("Please Waiting Another Release");
                        SajetCommon.Show_Message(sMsg, 0);
                        return;
                    }
                }
                editQty.Text = sQtyTemp;
                editStart.Text = sTempStart;
                editSNCount.Text = sTempSNCount;
                //==============================================================

                //有連板數
                int iSubBoardQty = 1;
                if (editSNCount.Visible)
                {
                    if (string.IsNullOrEmpty(editSNCount.Text))
                        iSubBoardQty = 1;
                    else
                        iSubBoardQty = Convert.ToInt32(editSNCount.Text);
                    if (iSubBoardQty <= 0)
                    {
                        string sErrMsg = SajetCommon.SetLanguage("Value must be bigger than 0") + Environment.NewLine
                                       + SajetCommon.SetLanguage(g_sLabType);
                        SajetCommon.Show_Message(sErrMsg, 0);
                        editSNCount.Focus();
                        editSNCount.SelectAll();
                        return;
                    }
                    //連板數不可以大於料號中定義的
                    if (iSubBoardQty > g_iOptionQty)
                    {
                        string sErrMsg = lablSub.Text + " <= \"" + g_iOptionQty.ToString() + "\"";
                        SajetCommon.Show_Message(sErrMsg, 0);
                        editSNCount.Focus();
                        editSNCount.SelectAll();
                        return;
                    }
                }

                //使用Sequence展號碼
                if (g_sType == "Y")
                {
                    //有空白字元,詢問是否忽略(產生的號碼會有空白字元)
                    if (editDefault.Text.IndexOf(" ") > -1)
                    {
                        string sErrMsg = SajetCommon.SetLanguage("Ignore Empty Character") + " ?";
                        if (SajetCommon.Show_Message(sErrMsg, 2) != DialogResult.Yes)
                            return;
                    }

                    if (Convert.ToInt32(LabReleaseQty.Text) + (Convert.ToInt32(editQty.Text) * iSubBoardQty) > Convert.ToInt32(LabTargetQty.Text) * g_iReleaseTimes)
                    {
                        string sErrMsg = SajetCommon.SetLanguage("Release Qty") + " > " + SajetCommon.SetLanguage("Target Qty");
                        SajetCommon.Show_Message(sErrMsg, 0);
                        editQty.Focus();
                        editQty.SelectAll();
                        return;
                    }

                    if (TWoRule.g_bSeq_Manual) //No Seq
                    {
                        if (g_sSeqCode.Length != editStart.Text.Length)
                        {
                            string sErrMsg = SajetCommon.SetLanguage("Length Error") + Environment.NewLine + LabStart.Text;
                            SajetCommon.Show_Message(sErrMsg, 0);
                            return;
                        }

                        string sFix = editDefault.Text;
                        int iNo = 0;
                        for (int i = 0; i <= LabCode.Text.Length - 1; i++)
                        {
                            if (g_sSeqCode.Contains(LabCode.Text.Substring(i, 1)))
                            {
                                sFix = sFix.Remove(i, 1);
                                sFix = sFix.Insert(i, editStart.Text[iNo].ToString());
                                iNo = iNo + 1;
                            }
                        }
                        if (!CheckRule(sFix, g_sDefault, LabCode.Text, LVCheck))
                            return;
                    }
                    else
                    {
                        //檢查流水號部份是否合法
                        string sMsg = "";
                        bool bResult = LabelCheckDll.Check_Code(editStart.Text, LabCode.Text, TWoRule.g_sCarry, LVUserSeq, out sMsg);
                        if (!bResult)
                        {
                            string sErrMsg = SajetCommon.SetLanguage("Start Number NG") + Environment.NewLine
                                           + sMsg;
                            SajetCommon.Show_Message(sErrMsg, 0);
                            return;
                        }

                        //若規則中有空白,會根據輸入的值產生不同的Sequence
                        TWoRule.g_sFRule = TWoRule.g_sRule;
                        if (g_sDefault.IndexOf(" ") > -1)
                        {
                            string sEmpty = "";
                            for (int i = 0; i <= g_sDefault.Length - 1; i++)
                            {
                                if (g_sDefault[i].ToString() == " " && editDefault.Text[i].ToString() != " ")
                                    sEmpty = sEmpty + editDefault.Text.Substring(i, 1);
                            }
                            if (sEmpty != "")
                                TWoRule.g_sFRule = TWoRule.g_sRule + "_" + sEmpty;
                        }
                    }
                }
                else
                {
                    if (g_sType == "S") //起始號也要檢查Rule
                    {
                        if (!CheckRule(editQty.Text, g_sDefault, LabCode.Text, LVCheck))
                            return;
                    }
                    if (!CheckRule(editStart.Text, g_sDefault, LabCode.Text, LVCheck)) //editStart是填全部的號碼
                        return;
                }

                //==============開始產生號碼,Update DB====================================                
                listbPrint.Items.Clear();
                //先產生號碼在關聯至工單
                if (g_sType == "S")
                {
                    //2011/03/11若料號不一致
                    if (TMAC.sReleaseType == "1")
                    {
                        sSQL = " select COUNT(*) QTY from " + g_sTable
                             + " where " + g_sFieldName + " >= '" + editQty.Text + "' "
                             + " and " + g_sFieldName + " <= '" + editStart.Text + "' "
                             + " and NVL(part_id,0) <> '" + TMAC.sMacPartID + "' ";
                        dsTemp = ClientUtils.ExecuteSQL(sSQL);
                        if (Convert.ToInt32(dsTemp.Tables[0].Rows[0]["QTY"].ToString()) > 0)
                        {
                            SajetCommon.Show_Message(SajetCommon.SetLanguage("Start Number") + " : " + editQty.Text + Environment.NewLine
                                                    + SajetCommon.SetLanguage("End Number") + " : " + editStart.Text + Environment.NewLine
                                                    + dsTemp.Tables[0].Rows[0]["QTY"].ToString() + " " + SajetCommon.SetLanguage(g_sLabType) + Environment.NewLine
                                                    + SajetCommon.SetLanguage("Part No Different"), 0);
                            return;
                        }
                    }

                    int iOverQty = 0;
                    sSQL = " select work_order, count(*) cnt from " + g_sTable
                         + " where " + g_sFieldName + " >= '" + editQty.Text + "' "
                         + " and " + g_sFieldName + " <= '" + editStart.Text + "' "
                         + " group by work_order "
                         + " order by work_order ";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);
                    if (dsTemp.Tables[0].Rows.Count == 0) //沒有先展號碼
                    {
                        string sMsg = SajetCommon.SetLanguage("Data not Exist") + Environment.NewLine
                                    + SajetCommon.SetLanguage(g_sLabType);
                        SajetCommon.Show_Message(sMsg, 0);
                        return;
                    }
                    //有其他工單已使用
                    else if (dsTemp.Tables[0].Rows.Count > 1
                             || !string.IsNullOrEmpty(dsTemp.Tables[0].Rows[0]["WORK_ORDER"].ToString()))
                    {
                        string sMsg = SajetCommon.SetLanguage(g_sLabType) + " " + SajetCommon.SetLanguage("had been Used") + Environment.NewLine
                                    + SajetCommon.SetLanguage("Work Order") + ":" + dsTemp.Tables[0].Rows[0]["WORK_ORDER"].ToString();
                        SajetCommon.Show_Message(sMsg, 0);
                        return;
                    }
                    else
                    {
                        int iCnt = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["Cnt"].ToString());
                        int iTarget = Convert.ToInt32(dsWoData.Tables[0].Rows[0]["TARGET_QTY"].ToString());
                        //大於可超領數
                        if (iCnt + Convert.ToInt32(LabReleaseQty.Text) > iTarget * iSubBoardQty * g_iReleaseTimes)
                        {
                            string sErrMsg = SajetCommon.SetLanguage("Release Qty") + " > " + SajetCommon.SetLanguage("Target Qty");
                            SajetCommon.Show_Message(sErrMsg, 0);
                            return;
                        }
                        //大於工單可領數
                        else if (iCnt + Convert.ToInt32(LabReleaseQty.Text) > iTarget * iSubBoardQty)
                        {
                            int iPri = ClientUtils.GetPrivilege(g_sUserID, "Over Release", g_sProgram);
                            if (iPri == 0) //無權限不可超領
                            {
                                string sErrMsg = SajetCommon.SetLanguage("Release Qty") + " > " + SajetCommon.SetLanguage("Target Qty") + Environment.NewLine
                                               + SajetCommon.SetLanguage("Can not Over Release");
                                SajetCommon.Show_Message(sErrMsg, 0);
                                return;
                            }
                            else
                            {
                                string sConfirmMsg = SajetCommon.SetLanguage("Release Qty") + " > " + SajetCommon.SetLanguage("Target Qty") + Environment.NewLine
                                                   + SajetCommon.SetLanguage("Sure to Over Release") + "?";
                                if (SajetCommon.Show_Message(sConfirmMsg, 2) != DialogResult.Yes)
                                {
                                    return;
                                }
                                //2011/04/28超領時須輸入Memo                            
                                fRemark f = new fRemark();
                                if (f.ShowDialog() != DialogResult.OK)
                                    return;

                                iOverQty = (iCnt + Convert.ToInt32(LabReleaseQty.Text)) - (iTarget * iSubBoardQty);//超領數
                                sSQL = "Insert Into SAJET.G_WO_RELEASE_MEMO"
                                     + "(WORK_ORDER,TYPE,START_NO,END_NO,REMARK "
                                     + ",APP_UNIT,APP_USER,APP_REASON "
                                     + ",UPDATE_TIME,UPDATE_USERID)"
                                     + "Values(:WORK_ORDER,:TYPE,:START_NO,:END_NO,:REMARK "
                                     + ",:APP_UNIT,:APP_USER,:APP_REASON "
                                     + ",SYSDATE,:UPDATE_USERID) ";

                                object[][] Params = new object[9][];
                                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", editWO.Text };
                                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TYPE", g_sLabType };
                                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "START_NO", editQty.Text };
                                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "END_NO", editStart.Text };
                                Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "APP_UNIT", f.editUnit.Text };
                                Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "APP_USER", f.editUser.Text };
                                Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "APP_REASON", f.editReason.Text };
                                Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "REMARK", f.richTextBox1.Text };
                                Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                                ClientUtils.ExecuteSQL(sSQL, Params);

                                f.Dispose();
                            }
                        }
                    }

                    sSQL = " update " + g_sTable
                         + " set work_order = '" + editWO.Text + "' "
                         + "    ,update_userid = '" + g_sUserID + "' "
                         + "    ,update_time = sysdate "
                         + " where " + g_sFieldName + " >= '" + editQty.Text + "' "
                         + " and " + g_sFieldName + " <= '" + editStart.Text + "' ";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);


                    sSQL = " select " + g_sFieldName + " FIELD from " + g_sTable
                         + " where " + g_sFieldName + " >= '" + editQty.Text + "' "
                         + " and " + g_sFieldName + " <= '" + editStart.Text + "' "
                         + " Order By " + g_sFieldName;
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);
                    for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
                    {
                        string sMAC = dsTemp.Tables[0].Rows[i]["FIELD"].ToString();
                        if (iOverQty > 0 && i >= (dsTemp.Tables[0].Rows.Count - iOverQty))
                        {
                            //記錄超領的號碼
                            string sSQL1 = " Update " + g_sTable
                                         + " Set Over_Flag = 'Y' "
                                         + " where " + g_sFieldName + " = '" + sMAC + "' ";
                            ClientUtils.ExecuteSQL(sSQL1);
                        }

                        //先將要列印號碼暫存
                        if (g_bSetup_Print)
                        {
                            listbPrint.Items.Add(dsTemp.Tables[0].Rows[i]["FIELD"].ToString());
                        }
                    }

                    /*
                    //先將要列印號碼暫存
                    if (g_bSetup_Print)
                    {
                        sSQL = " select " + g_sFieldName + " FIELD from " + g_sTable
                             + " where " + g_sFieldName + " >= '" + editQty.Text + "' "
                             + " and " + g_sFieldName + " <= '" + editStart.Text + "' ";                             
                        dsTemp = ClientUtils.ExecuteSQL(sSQL);
                        for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
                        {
                            listbPrint.Items.Add(dsTemp.Tables[0].Rows[i]["FIELD"].ToString());
                        }
                    }
                     */
                }
                else
                {
                    Int64 iStart = 1;
                    int iQty = Convert.ToInt32(editQty.Text);
                    if ((g_iSeqFlag == 1) && (editSNCount.Text != "0"))
                        iQty = iQty * Convert.ToInt32(editSNCount.Text);

                    if (g_sType == "Y")
                    {
                        if (!TWoRule.g_bSeq_Manual)
                        {
                            //動態建立此規則的Sequence                   
                            LabelCheckDll.Create_Rule_Seq(g_sSeq + TWoRule.g_sFRule, editStart.Text, LabCode.Text, TWoRule.g_sSeqText, TWoRule.g_sCarry, LVUserSeq);
                            object[] sMsg = new object[1][];
                            sMsg = LabelCheckDll.CheckSeqMaxValue(g_sSeq + TWoRule.g_sFRule, Convert.ToInt32(editQty.Text));
                            if (sMsg[0].ToString() != "OK")
                            {
                                string msg = SajetCommon.SetLanguage(sMsg[0].ToString()) + Environment.NewLine
                                    + SajetCommon.SetLanguage("Available") + ":" + sMsg[1].ToString() + Environment.NewLine
                                    + SajetCommon.SetLanguage("Maximum") + ":" + sMsg[2].ToString() + Environment.NewLine
                                    + SajetCommon.SetLanguage("Released") + ":" + sMsg[3].ToString();
                                ClientUtils.ShowMessage(msg, 0);
                                return;
                            }
                            SaveToDB("Reset Sequence Mark", TWoRule.g_sFRule, TWoRule.g_sMark);
                            iStart = LabelCheckDll.GetSeq_NextVal("SAPServer", g_sSeq + TWoRule.g_sFRule, iQty); //editQty.Text); //讀取目前的Sequence號碼
                            //避免兩台同時展
                            if (iStart != iFirstStart)
                            {
                                string sErrMsg = SajetCommon.SetLanguage("Start Number NG") + Environment.NewLine + SajetCommon.SetLanguage("Start Sequence Different");
                                SajetCommon.Show_Message(sErrMsg, 0);
                                return;
                            }
                        }
                        else
                        {
                            iStart = LabelCheckDll.SeqCode(editStart.Text, LabCode.Text, TWoRule.g_sSeqText, TWoRule.g_sCarry, LVUserSeq, "S");
                            //string sStart = Get_LabelNo(iStart); //Label號碼
                            //string sEnd = Get_LabelNo(iStart + Convert.ToInt32(editQty.Text) - 1);

                            //sSQL = " select nvl(max(" + g_sFieldName + "),'N/A') LABEL_NO from " + g_sTable
                            //     + " where " + g_sFieldName + " >= '" + sStart + "' "
                            //     + " and " + g_sFieldName + " <= '" + sEnd + "' ";                                 
                            //dsTemp = ClientUtils.ExecuteSQL(sSQL);
                            //if (dsTemp.Tables[0].Rows[0]["LABEL_NO"].ToString() != "N/A")
                            //{
                            //    string sMsg = SajetCommon.SetLanguage("Data Duplicate") + Environment.NewLine
                            //                + SajetCommon.SetLanguage("Max") + " : " + dsTemp.Tables[0].Rows[0]["LABEL_NO"].ToString();
                            //    SajetCommon.Show_Message(sMsg, 0);
                            //    editStart.Focus();
                            //    return;
                            //}
                            Int64 iStartTemp = iStart;
                            ListBox listDupDate = new ListBox();
                            fDupData fData = new fDupData();
                            listDupDate.Items.Clear();
                            for (int i = 1; i <= iQty; i++)
                            {
                                try
                                {
                                    string sLabel = Get_LabelNo(iStartTemp); //Label號碼
                                    sSQL = " select " + g_sFieldName + " LABEL_NO from " + g_sTable
                                         + " where " + g_sFieldName + " = '" + sLabel + "' ";
                                    dsTemp = ClientUtils.ExecuteSQL(sSQL);
                                    if (dsTemp.Tables[0].Rows.Count > 0)
                                    {
                                        listDupDate.Items.Add(sLabel);
                                        fData.dgvData.Rows.Add();
                                        fData.dgvData.Rows[fData.dgvData.Rows.Count - 1].Cells["Data"].Value = sLabel;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    SajetCommon.Show_Message(ex.Message, 0);
                                    if (TWoRule.g_bSeq_Manual)
                                        return;
                                }
                                iStartTemp++;
                            }
                            if (listDupDate.Items.Count > 0)
                            {
                                fData.ShowDialog();
                                return;
                            }
                        }
                    }
                    else
                    {
                        //找屬於流水號的部份
                        string sStart = "";
                        for (int i = 0; i <= LabCode.Text.Length - 1; i++)
                        {
                            if (LabCode.Text[i].ToString() == "S")
                                sStart = sStart + editStart.Text[i].ToString();
                        }

                        iStart = LabelCheckDll.SeqCode(sStart, LabCode.Text, TWoRule.g_sSeqText, TWoRule.g_sCarry, LVUserSeq, "S");

                        string sEnd = Get_LabelNo(iStart + Convert.ToInt32(editQty.Text) - 1);
                        sSQL = " select work_order, " + g_sFieldName + " from " + g_sTable
                             + " where " + g_sFieldName + " >= '" + editStart.Text + "' "
                             + " and " + g_sFieldName + " <= '" + sEnd + "' "
                             + " and rownum=1";
                        dsTemp = ClientUtils.ExecuteSQL(sSQL);
                        if (dsTemp.Tables[0].Rows.Count > 0)
                        {
                            string sMsg = SajetCommon.SetLanguage(g_sLabType) + " " + SajetCommon.SetLanguage("had been Used") + Environment.NewLine
                                        + SajetCommon.SetLanguage("Work Order") + ":" + dsTemp.Tables[0].Rows[0]["WORK_ORDER"].ToString();
                            SajetCommon.Show_Message(sMsg, 0);
                            editStart.Focus();
                            return;
                        }
                    }

                    //產生號碼 
                    //fDupData fDataDup = new fDupData();
                    try
                    {
                        progressBar1.Minimum = 0;
                        progressBar1.Maximum = iQty;
                        progressBar1.Visible = true;

                        //2013/1/25 增加大量資料填入DB方式(SN無大板且要展5000筆以上,就用此方式) ==
                        string sSNType = "N/A";
                        if (g_sLabType.ToUpper() == "SERIAL NUMBER")
                        {
                            if (!editSNCount.Visible && iQty >= 5000)
                            {
                                sSNType = "BATCH";
                                TableStructure("SAJET.G_SN_STATUS", "WORK_ORDER" + (char)9 + "SERIAL_NUMBER" + (char)9 + "PART_ID" + (char)9 + "ROUTE_ID" + (char)9 + "VERSION" + (char)9 + "EMP_ID");
                            }
                        }
                        int iRowIndex = 0;
                        int iBatchQty = 100000; //一次填入DB的數量,超過會分次填入
                        string[] slRow = new string[iQty];
                        string sPartID = dsWoData.Tables[0].Rows[0]["PART_ID"].ToString();
                        string sRouteID = dsWoData.Tables[0].Rows[0]["ROUTE_ID"].ToString();
                        string sVersion = dsWoData.Tables[0].Rows[0]["VERSION"].ToString();
                        string[] DateTimeList = { "yyyy/M/d tt hh:mm:ss", "yyyy/MM/dd tt hh:mm:ss", "yyyy/MM/dd HH:mm:ss", "yyyy/M/d HH:mm:ss", "yyyy/M/d", "yyyy/MM/dd" };
                        //=====

                        // 2016.5.31 By Jason
                        n = 0;
                        dUnitTotalQty = 0;
                        datExeTime = DateTime.Now;
                        lTravel_Id = Convert.ToInt64(datExeTime.ToString("yyyyMMddHHmmssf"));
                        // 2016.5.31 End

                        for (int i = 1; i <= iQty; i++)
                        {
                            try
                            {
                                string sLabel = Get_LabelNo(iStart); //Label號碼
                                if (g_iOptionQty > 0) //有連板數,小板號為大板後+01(由Procedure控制)
                                    Save_SubBoard(sLabel, iSubBoardQty);
                                else if (g_sLabType.ToUpper() == "SERIAL NUMBER")
                                {
                                    if (sSNType != "BATCH")
                                    {
                                        Save_SN(sLabel);
                                    }
                                    else //2013/1/25 add 
                                    {
                                        /*
                                        sSQL = " select " + g_sFieldName + " LABEL_NO from " + g_sTable
                                             + " where " + g_sFieldName + " = '" + sLabel + "' ";
                                        dsTemp = ClientUtils.ExecuteSQL(sSQL);
                                        if (dsTemp.Tables[0].Rows.Count > 0)
                                        {
                                            fDataDup.dgvData.Rows.Add();
                                            fDataDup.dgvData.Rows[fDataDup.dgvData.Rows.Count - 1].Cells["Data"].Value = sLabel;
                                        }
                                        */
                                        //大量資料一次填入DB,加快時間(for 11G)                                       
                                        slRow[iRowIndex] = editWO.Text + (char)9 + sLabel + (char)9 + sPartID + (char)9 + sRouteID + (char)9 + sVersion + (char)9 + g_sUserID;
                                        if (i % iBatchQty == 0)
                                        {
                                            //if (fDataDup.dgvData.Rows.Count > 0)
                                            //{
                                            //    fDataDup.ShowDialog();
                                            //    return;
                                            //}
                                            bool bResult = Save_SN_Batch(iBatchQty, slRow);
                                            if (!bResult)
                                                return;
                                            iRowIndex = 0;
                                        }
                                        else
                                            iRowIndex++;
                                    } //2013/1/25 add end===================
                                }
                                else
                                    Save_Label(sLabel, iStart + i);

                                //先將要列印號碼暫存
                                if (g_bSetup_Print)
                                {
                                    listbPrint.Items.Add(sLabel);
                                }
                            }
                            catch (Exception ex)
                            {
                                SajetCommon.Show_Message(ex.Message, 0);
                                if (TWoRule.g_bSeq_Manual) //若為手動,有錯就中斷
                                    return;
                            }

                            iStart++;
                            progressBar1.Value = i;
                        }

                        //2013/1/25 add ===================
                        if (sSNType == "BATCH")
                        {
                            if (iQty % iBatchQty != 0)
                            {
                                //if (fDataDup.dgvData.Rows.Count > 0)
                                //{
                                //    fDataDup.ShowDialog();
                                //    return;
                                //}
                                bool bResult = Save_SN_Batch(iQty % iBatchQty, slRow);
                                if (!bResult)
                                    return;
                                iRowIndex = 0;
                            }
                        }
                        //2013/1/25 add end===================
                    }
                    catch (Exception ex)
                    {
                        return;
                    }
                    finally
                    {
                        progressBar1.Visible = false;
                        //fDataDup.Dispose();                     
                        DateTime dtEnd = ClientUtils.GetSysDate();
                        TimeSpan dt = (dtEnd - dtStart);
                        //MessageBox.Show(dt.TotalSeconds.ToString());
                    }

                    //將工單狀態改為"Release"
                    if (g_sLabType.ToUpper() == "SERIAL NUMBER")
                    {
                        string sWoStatus = dsWoData.Tables[0].Rows[0]["WO_STATUS"].ToString();

                        if (Convert.ToInt32(editQty.Text) >= 1 && Convert.ToInt32(sWoStatus) <= 1)
                        {
                            sSQL = " Update SAJET.G_WO_BASE "
                                 + " Set WO_STATUS = '2' "
                                 + "    ,UPDATE_USERID = '" + g_sUserID + "' "
                                 + "    ,UPDATE_TIME = SYSDATE "
                                 + "Where WORK_ORDER = '" + editWO.Text + "' ";
                            dsTemp = ClientUtils.ExecuteSQL(sSQL);

                            //Copy To History
                            sSQL = " Insert Into SAJET.G_HT_WO_BASE "
                                 + " Select * from SAJET.G_WO_BASE "
                                 + "Where WORK_ORDER = '" + editWO.Text + "' ";
                            dsTemp = ClientUtils.ExecuteSQL(sSQL);
                        }
                    }

                    // 2016.5.31 By Jason
                    if (g_sLabType.ToUpper() == "RC NO")
                    {
                        string sWoStatus = dsWoData.Tables[0].Rows[0]["WO_STATUS"].ToString();

                        if (Convert.ToInt32(editQty.Text) >= 1 && Convert.ToInt32(sWoStatus) <= 3)
                        {
                            sSQL = @"
UPDATE SAJET.G_WO_BASE
SET WO_STATUS     = :WO_STATUS
   ,INPUT_QTY     = INPUT_QTY + :INPUT_QTY
   ,UPDATE_USERID = :UPDATE_USERID
   ,UPDATE_TIME   = :UPDATE_TIME
WHERE WORK_ORDER  = :WORK_ORDER
";

                            object[][] Params = new object[5][];
                            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WO_STATUS", 2 };
                            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "INPUT_QTY", dUnitTotalQty };
                            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                            Params[3] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime };
                            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", editWO.Text };
                            ClientUtils.ExecuteSQL(sSQL, Params);

                            //Copy To History
                            sSQL = @"
INSERT INTO SAJET.G_HT_WO_BASE
SELECT *
FROM SAJET.G_WO_BASE
WHERE WORK_ORDER = :WORK_ORDER
";
                            Params = new object[1][];
                            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", editWO.Text };
                            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

                            sSQL = @"
INSERT INTO SAJET.G_WO_STATUS
(
    WORK_ORDER
   ,WO_STATUS
   ,MEMO
   ,UPDATE_USERID
   ,UPDATE_TIME
)
VALUES
(
    :WORK_ORDER
   ,:WO_STATUS
   ,:MEMO
   ,:UPDATE_USERID
   ,:UPDATE_TIME
)";
                            Params = new object[5][];
                            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", editWO.Text };
                            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WO_STATUS", 2 };
                            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MEMO", "R/C RELEASE" };
                            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                            Params[4] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime };
                            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                        }
                    }
                    // 2016.5.31 End
                }

                //列印條碼========================
                if (g_bSetup_Print)
                {
                    if (g_bPrintSortDesc) //由大到小列印
                    {
                        string[] sPrintData = new string[listbPrint.Items.Count];
                        for (int i = 0; i <= listbPrint.Items.Count - 1; i++)
                            sPrintData[i] = listbPrint.Items[i].Text;
                        Array.Reverse(sPrintData);

                        listbPrint.Items.Clear();
                        for (int i = 0; i <= sPrintData.Length - 1; i++)
                            listbPrint.Items.Add(sPrintData[i].ToString());
                    }
                    if (!Print_Label(listbPrint))
                        return;
                }
                Show_WoData(editWO.Text, true, TWoRule.g_sFRule);
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);
                return;
            }
            finally
            {
                panel3.Enabled = true;

                Cursor = Cursors.Default;
            }
        }

        public bool Print_Label(ListView listbPrint)
        {
            string sFile = Application.StartupPath + "\\" + g_sExeName + "\\BarcodeCenter.ini";
            //Sleep幾秒
            SajetInifile sajetInifile1 = new SajetInifile();
            int iSleepSec = Convert.ToInt32(sajetInifile1.ReadIniFile(sFile, "Print Label", "Delay Second", "0"));
            //每印幾張就Sleep一次
            int iPrintCount = Convert.ToInt32(sajetInifile1.ReadIniFile(sFile, "Print Label", "Print Count", "100"));
            //對應S_SYS_PRINT_DATA的DATA_TYPE欄位 
            string sDataType = "BC_" + g_sLabType;
            string sLabType = g_sLabType;

            // 向使用者詢問需不需要列印 QR Code 綜合清單
            string sMessage = SajetCommon.SetLanguage("Print QR code runcard list?");

            bool b_print_rc_list = SajetCommon.Show_Message(sMessage, 2) == DialogResult.Yes;

            //先Link====
            PrintLabel.Setup PrintLabelDll = new PrintLabel.Setup();
            if (g_sSetup_PrintMethod.ToUpper() == "CODESOFT")
            {
                try
                {
                    PrintLabelDll.Open(g_sSetup_PrintMethod.ToUpper());
                }
                catch
                {
                    SajetCommon.Show_Message("Open CODESOFT Fail", 0);
                    return false;
                }
            }
            else if (g_sSetup_PrintMethod.ToUpper() == "BARTENDER" && g_sSetup_PrintPort.ToUpper() == "STANDARD")
                PrintLabelDll.Open(g_sSetup_PrintMethod.ToUpper());
            else if (g_sSetup_PrintMethod.ToUpper() == "DLL")
                PrintLabelDll.Open(g_sSetup_PrintPort.ToUpper());

            //Print=====
            try
            {
                progressBar1.Minimum = 0;
                progressBar1.Maximum = listbPrint.Items.Count - 1;
                progressBar1.Visible = true;
                if (g_sSetup_PrintMethod.ToUpper() == "BARTENDER" && g_sSetup_PrintPort.ToUpper() == "DATASOURCE")
                {
                    ListParam.Items.Clear();
                    ListData.Items.Clear();

                    string base_path = Path.Combine(Directory.GetCurrentDirectory(), "BarcodeCenter");

                    string s_print_command_bat_file = Path.Combine(base_path, "PrintGo_Release.bat");

                    string s_command_content = string.Empty;

                    if (File.Exists(s_print_command_bat_file))
                    {
                        File.Delete(s_print_command_bat_file);
                    }

                    for (int i = 0; i <= listbPrint.Items.Count - 1; i++)
                    {
                        ListData.Items.Add(listbPrint.Items[i].Text);
                    }

                    bool get_printer_command_success
                        = PrintLabelDll.Print_Bartender_DataSource(
                        sExeName: g_sExeName,
                        sType: sDataType,
                        sFileTitle: g_sFileName,
                        sFileName: "",
                        iPrintQty: g_iSetup_PrintQty,
                        sPrintMethod: g_sSetup_PrintMethod,
                        sPrintPort: g_sSetup_PrintPort,
                        ListParam: ListParam,
                        ListData: ListData,
                        sMessage: out sMessage,
                        direct_print: false);

                    if (!get_printer_command_success)
                    {
                        SajetCommon.Show_Message(sMessage, 0);
                        return false;
                    }

                    s_command_content += sMessage;

                    // QR Code 綜合清單的處理
                    if (b_print_rc_list)
                    {
                        get_printer_command_success
                            = RlSrv.Print(
                                work_order: editWO.Text.Trim(),
                                direct_print: false,
                                out sMessage);

                        if (!get_printer_command_success)
                        {
                            SajetCommon.Show_Message(sMessage, 0);
                            return false;
                        }

                        s_command_content += Environment.NewLine + sMessage;
                    }

                    Setup sss = new Setup();
                    sss.WriteToPrintGo(s_print_command_bat_file, s_command_content);

                    Setup.WinExec(s_print_command_bat_file, 0);

                    return true;
                }


                for (int i = 0; i <= listbPrint.Items.Count - 1; i++)
                {
                    //因應一張Label有多個號碼,每幾個才去Print一次   
                    if (g_sSetup_iLabelQty > 1)
                    {
                        int iNo = (i % g_sSetup_iLabelQty) + 1;
                        //第一張才需去Select Data
                        if (iNo == 1)
                        {
                            ListParam.Items.Clear();
                            ListData.Items.Clear();
                            ListData.Items.Add(listbPrint.Items[i].Text);
                            //各變數值                        
                            PrintLabelDll.GetPrintData(sDataType, ref ListParam, ref ListData);
                        }

                        ListParam.Items.Add(sLabType.ToUpper() + "_" + iNo);
                        ListData.Items.Add(listbPrint.Items[i].Text);

                        //每幾張才去列印一次
                        if (iNo == g_sSetup_iLabelQty || (iNo < g_sSetup_iLabelQty && i == listbPrint.Items.Count - 1))
                        {
                            //開始列印
                            if (g_sSetup_PrintMethod.ToUpper() == "CODESOFT")
                                PrintLabelDll.Print_MultiLabel(g_sExeName, sLabType, g_sFileName, "", g_iSetup_PrintQty, g_sSetup_PrintMethod, g_sSetup_CodeSoftVer, ListParam, ListData, out sMessage);
                            else
                                PrintLabelDll.Print_MultiLabel(g_sExeName, sLabType, g_sFileName, "", g_iSetup_PrintQty, g_sSetup_PrintMethod, g_sSetup_PrintPort, ListParam, ListData, out sMessage);
                            if (sMessage != "OK")
                            {
                                SajetCommon.Show_Message(sMessage, 0);
                                return false;
                            }
                        }
                    }
                    else
                    {
                        ListParam.Items.Clear();
                        ListData.Items.Clear();
                        ListData.Items.Add(listbPrint.Items[i].Text);

                        //各變數值                        
                        PrintLabelDll.GetPrintData(sDataType, ref ListParam, ref ListData);
                        //開始列印
                        if (g_sSetup_PrintMethod.ToUpper() == "CODESOFT")
                            PrintLabelDll.Print(g_sExeName, sDataType, g_sFileName, "", g_iSetup_PrintQty, g_sSetup_PrintMethod, g_sSetup_CodeSoftVer, ListParam, ListData, out sMessage);
                        else
                            PrintLabelDll.Print(g_sExeName, sDataType, g_sFileName, "", g_iSetup_PrintQty, g_sSetup_PrintMethod, g_sSetup_PrintPort, ListParam, ListData, out sMessage);
                        if (sMessage != "OK")
                        {
                            SajetCommon.Show_Message(sMessage, 0);
                            return false;
                        }
                    }
                    progressBar1.Value = i;
                    //為了避免CodeSoft列印會亂跳,每印100張就Sleep,等待CodeSoft清除Buffer
                    if ((iSleepSec > 0) && (i < listbPrint.Items.Count - 1))
                    {
                        if ((i + 1) % iPrintCount == 0)
                            System.Threading.Thread.Sleep(iSleepSec * 1000);
                    }
                }
                return true;
            }
            finally
            {
                progressBar1.Visible = false;
                if (g_sSetup_PrintMethod.ToUpper() == "CODESOFT")
                    PrintLabelDll.Close(g_sSetup_PrintMethod.ToUpper());
                else if (g_sSetup_PrintMethod.ToUpper() == "BARTENDER" && g_sSetup_PrintPort.ToUpper() == "STANDARD")
                    PrintLabelDll.Close(g_sSetup_PrintMethod.ToUpper());
                else if (g_sSetup_PrintMethod.ToUpper() == "DLL")
                    PrintLabelDll.Close(g_sSetup_PrintPort.ToUpper());
            }
        }
        public void Save_SubBoard(string sValue, int iSubBoardQty)
        {
            //呼叫Server Method來執行stored procedure            
            try
            {
                object[][] Params = new object[5][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TWO", editWO.Text };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TVALUE", sValue };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.Int32, "TQTY", iSubBoardQty };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.Int32, "TEMPID", g_sUserID };
                Params[4] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                DataSet ds;
                if (!String.IsNullOrEmpty(g_sLabelFunction))
                    ds = ClientUtils.ExecuteProc(g_sLabelFunction, Params);
                else
                    ds = ClientUtils.ExecuteProc("SAJET.BC_" + g_sFieldName.ToUpper(), Params);

                string sRes = ds.Tables[0].Rows[0]["TRES"].ToString();
                if (sRes != "OK")
                {
                    SajetCommon.Show_Message(sRes, 0);
                    return;
                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);
                return;
            }
        }

        public void Save_SN(string sValue)
        {
            try
            {
                if (!String.IsNullOrEmpty(g_sLabelFunction))
                {
                    object[][] Params = new object[5][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TWO", editWO.Text };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TVALUE", sValue };
                    Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TQTY", editSNCount.Text };
                    Params[3] = new object[] { ParameterDirection.Input, OracleType.Int32, "TEMPID", ClientUtils.UserPara1 };
                    Params[4] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                    DataSet ds = ClientUtils.ExecuteProc(g_sLabelFunction, Params);
                    string sRes = ds.Tables[0].Rows[0]["TRES"].ToString();
                    if (sRes != "OK")
                    {
                        SajetCommon.Show_Message(sRes, 0);
                        return;
                    }
                }
                else
                {
                    string sPartID = dsWoData.Tables[0].Rows[0]["PART_ID"].ToString();
                    string sRouteID = dsWoData.Tables[0].Rows[0]["ROUTE_ID"].ToString();
                    string sVersion = dsWoData.Tables[0].Rows[0]["VERSION"].ToString();
                    string sSQL = " Insert Into SAJET.G_SN_STATUS "
                                + " (WORK_ORDER,PART_ID,SERIAL_NUMBER,ROUTE_ID,Version,EMP_ID) "
                                + " Values "
                                + " ('" + editWO.Text + "','" + sPartID + "','" + sValue + "','" + sRouteID + "','" + sVersion + "','" + g_sUserID + "') ";
                    DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);
                return;
            }
        }

        public void Save_Label(string sValue, Int64 iSeq)
        {
            try
            {
                if (!String.IsNullOrEmpty(g_sLabelFunction))
                {
                    object[][] Params = new object[5][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TWO", editWO.Text };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TVALUE", sValue };
                    Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSEQ", iSeq };
                    Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TEMPID", ClientUtils.UserPara1 };
                    Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TRES", "" };
                    DataSet ds = ClientUtils.ExecuteProc(g_sLabelFunction, Params);
                    string sRes = ds.Tables[0].Rows[0]["TRES"].ToString();
                    if (sRes != "OK")
                    {
                        SajetCommon.Show_Message(sRes, 0);
                        return;
                    }
                }
                else
                {
                    // 2016.5.27 By Jason
                    //string sSQL = " Insert Into " + g_sTable
                    //    + " (WORK_ORDER," + g_sFieldName + ",EMP_ID) "
                    //    + " Values "
                    //    + " ('" + editWO.Text + "','" + sValue + "','" + g_sUserID + "') ";
                    //DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);

                    n++;

                    if (g_sFieldName.ToUpper() == "RC_NO")
                    {
                        // Insert SAJET.G_RC_STATUS
                        sSQL = @"INSERT INTO SAJET.G_RC_STATUS (WORK_ORDER,RC_NO,PART_ID,VERSION,ROUTE_ID,FACTORY_ID,PDLINE_ID,STAGE_ID,NODE_ID,
                                                                PROCESS_ID,SHEET_NAME,TERMINAL_ID,TRAVEL_ID,NEXT_NODE,NEXT_PROCESS,CURRENT_STATUS,
                                                                CURRENT_QTY,IN_PROCESS_EMPID,IN_PROCESS_TIME,WIP_IN_QTY,WIP_IN_EMPID,WIP_IN_MEMO,
                                                                WIP_IN_TIME,WIP_OUT_GOOD_QTY,WIP_OUT_SCRAP_QTY,WIP_OUT_EMPID,WIP_OUT_MEMO,
                                                                WIP_OUT_TIME,OUT_PROCESS_EMPID,OUT_PROCESS_TIME,HAVE_SN,PRIORITY_LEVEL,UPDATE_USERID,
                                                                UPDATE_TIME,CREATE_TIME,BATCH_ID,EMP_ID,INITIAL_QTY,RELEASE)
                             VALUES (:WORK_ORDER,:RC_NO,:PART_ID,:VERSION,:ROUTE_ID,:FACTORY_ID,:PDLINE_ID,:STAGE_ID,:NODE_ID,
                                     :PROCESS_ID,:SHEET_NAME,:TERMINAL_ID,:TRAVEL_ID,:NEXT_NODE,:NEXT_PROCESS,:CURRENT_STATUS,
                                     :CURRENT_QTY,NULL,NULL,NULL,NULL,NULL,
                                     NULL,NULL,NULL,NULL,NULL,
                                     NULL,NULL,NULL,:HAVE_SN,:PRIORITY_LEVEL,:UPDATE_USERID,
                                     :UPDATE_TIME,:CREATE_TIME,NULL,:EMP_ID,:INITIAL_QTY,:RELEASE)";

                        object[][] ParamsNew = new object[25][];
                        ParamsNew[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", editWO.Text };
                        ParamsNew[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", sValue };
                        ParamsNew[2] = new object[] { ParameterDirection.Input, OracleType.Number, "PART_ID", iPart_Id };
                        ParamsNew[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VERSION", sVersion };
                        ParamsNew[4] = new object[] { ParameterDirection.Input, OracleType.Number, "ROUTE_ID", iRoute_Id };
                        ParamsNew[5] = new object[] { ParameterDirection.Input, OracleType.Number, "FACTORY_ID", iFactory_Id };
                        ParamsNew[6] = new object[] { ParameterDirection.Input, OracleType.Number, "PDLINE_ID", iLine_Id };
                        ParamsNew[7] = new object[] { ParameterDirection.Input, OracleType.Number, "STAGE_ID", iStage_Id };
                        ParamsNew[8] = new object[] { ParameterDirection.Input, OracleType.Number, "NODE_ID", iNode_Id };
                        ParamsNew[9] = new object[] { ParameterDirection.Input, OracleType.Number, "PROCESS_ID", iProcess_Id };
                        ParamsNew[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_NAME", sSheet_Name };
                        ParamsNew[11] = new object[] { ParameterDirection.Input, OracleType.Number, "TERMINAL_ID", 0 };
                        ParamsNew[12] = new object[] { ParameterDirection.Input, OracleType.Number, "TRAVEL_ID", lTravel_Id };
                        ParamsNew[13] = new object[] { ParameterDirection.Input, OracleType.Number, "NEXT_NODE", iNext_Node };
                        ParamsNew[14] = new object[] { ParameterDirection.Input, OracleType.Number, "NEXT_PROCESS", sNext_Process };
                        ParamsNew[15] = new object[] { ParameterDirection.Input, OracleType.Number, "CURRENT_STATUS", 0 };

                        // 2016.7.15 By Jason
                        //if (iUnit == n && (dUnitQty > dTargetQty - (dUnitQty * iUnit)))
                        //{
                        //    dUnitTotalQty = dUnitTotalQty + (dTargetQty - (dUnitQty * (iUnit - 1)));
                        //    ParamsNew[16] = new object[] { ParameterDirection.Input, OracleType.Number, "CURRENT_QTY", dTargetQty - (dUnitQty * (iUnit - 1)) };
                        //    ParamsNew[17] = new object[] { ParameterDirection.Input, OracleType.Number, "INITIAL_QTY", dTargetQty - (dUnitQty * (iUnit - 1)) };
                        //}
                        if (iUnit == n)
                        {
                            dUnitTotalQty = dUnitTotalQty + (dTargetQty - (dUnitQty * (iUnit - 1)));
                            ParamsNew[16] = new object[] { ParameterDirection.Input, OracleType.Number, "CURRENT_QTY", dTargetQty - (dUnitQty * (iUnit - 1)) };
                            ParamsNew[17] = new object[] { ParameterDirection.Input, OracleType.Number, "INITIAL_QTY", dTargetQty - (dUnitQty * (iUnit - 1)) };
                        }
                        // 2016.7.15 End
                        else
                        {
                            dUnitTotalQty = dUnitTotalQty + dUnitQty;
                            ParamsNew[16] = new object[] { ParameterDirection.Input, OracleType.Number, "CURRENT_QTY", dUnitQty };
                            ParamsNew[17] = new object[] { ParameterDirection.Input, OracleType.Number, "INITIAL_QTY", dUnitQty };
                        }

                        ParamsNew[18] = new object[] { ParameterDirection.Input, OracleType.Number, "HAVE_SN", 0 };
                        ParamsNew[19] = new object[] { ParameterDirection.Input, OracleType.Number, "PRIORITY_LEVEL", 0 };
                        ParamsNew[20] = new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", g_sUserID };
                        ParamsNew[21] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime };
                        ParamsNew[22] = new object[] { ParameterDirection.Input, OracleType.DateTime, "CREATE_TIME", datExeTime };
                        ParamsNew[23] = new object[] { ParameterDirection.Input, OracleType.Number, "EMP_ID", g_sUserID };
                        ParamsNew[24] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RELEASE", 'Y' };
                        ClientUtils.ExecuteSQL(sSQL, ParamsNew);
                    }
                    else
                    {
                        string sSQL = " Insert Into " + g_sTable
                            + " (WORK_ORDER," + g_sFieldName + ",EMP_ID) "
                            + " Values "
                            + " ('" + editWO.Text + "','" + sValue + "','" + g_sUserID + "') ";
                        DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
                    }
                    // 2016.5.27 End
                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);
                return;
            }
        }

        public void SaveToDB(string ParamName, string ParamItem, string ParamValue)
        {
            //更新Reset Sequence日期
            string sSQL;
            DataSet dsTemp;
            sSQL = " select rowid from sajet.SYS_MODULE_PARAM "
                 + " where MODULE_NAME = '" + g_sSeq.ToUpper() + "' "
                 + " and FUNCTION_NAME = '" + TWoRule.g_sRule.ToUpper() + "' "
                 + " and parame_name = '" + ParamName + "' ";
            if (ParamItem != "")
                sSQL = sSQL + "and parame_item ='" + ParamItem.ToUpper() + "'";
            else
                sSQL = sSQL + "and parame_item is null ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                sSQL = " Insert Into SAJET.SYS_MODULE_PARAM "
                     + " (MODULE_NAME,FUNCTION_NAME,PARAME_NAME,PARAME_ITEM,PARAME_VALUE,UPDATE_USERID ) "
                     + " Values "
                     + " ('" + g_sSeq.ToUpper() + "','" + TWoRule.g_sRule.ToUpper() + "','" + ParamName + "','" + ParamItem.ToUpper() + "','" + ParamValue + "','" + g_sUserID + "')";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
            }
            else
            {
                string sRowid = dsTemp.Tables[0].Rows[0]["rowid"].ToString();
                sSQL = " update sajet.SYS_MODULE_PARAM "
                     + " set parame_value =  '" + ParamValue + "' "
                     + " where rowid = '" + sRowid + "' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
            }
        }

        public string Get_LabelNo(Int64 iSeq)
        {
            //產生label號碼           
            string sDefault = "";
            string sSeq = LabelCheckDll.SeqTran(iSeq, LabCode.Text, TWoRule.g_sSeqText, TWoRule.g_sCarry, LVUserSeq);//SeqTran(iSeq);
            string sLabCode = LabCode.Text;
            if (g_sType == "Y")
                sDefault = editDefault.Text;
            else
                sDefault = editStart.Text;

            int j = 0;
            for (int i = 0; i <= sLabCode.Length - 1; i++)
            {
                if (TWoRule.g_sSeqText.IndexOf(sLabCode[i]) > -1)
                {
                    sDefault = sDefault.Remove(i, 1);
                    sDefault = sDefault.Insert(i, sSeq[j].ToString());
                    j++;
                }
            }
            //替換Check Sum
            if (LabCode.Text.IndexOf("X") > -1)
            {
                string sSQL = "select " + TWoRule.g_sCheckkSum + "('" + sDefault + "') SNID from dual ";
                DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
                sDefault = dsTemp.Tables[0].Rows[0]["SNID"].ToString();
            }
            return sDefault;
        }

        private void btnSearchWO_Click(object sender, EventArgs e)
        {
            sSQL = " select A.work_order, A.target_qty,B.part_no,B.spec1,B.spec2 "
                 + " from sajet.g_wo_base A, sajet.sys_part B "
                 + " where A.work_order like '" + editWO.Text + "%' "
                 + " and A.part_id = B.part_id "
                 + " AND A.WO_TYPE <> '轉投工單'"
                 + " and A.wo_status > 0 and A.wo_status < 5 ";
            if (g_sLabType.ToUpper() == "SERIAL NUMBER" || g_sLabType.ToUpper() == "RC NO")
                sSQL = sSQL + " and input_qty <> target_qty";
            sSQL = sSQL + " order by A.work_order ";

            fFilter f = new fFilter();
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                editWO.Text = f.dgvData.CurrentRow.Cells["WORK_ORDER"].Value.ToString();
                KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);
                editWO_KeyPress(sender, Key);
            }
        }

        public void ClearData()
        {
            LabWO.Text = "";
            LabPart.Text = "";
            LabPartDesc.Text = "";
            LabVer.Text = "";
            LabTargetQty.Text = "";
            LabInputQty.Text = "";
            LabWoDueDate.Text = "";
            LabPO.Text = "";
            LabCustomer.Text = "";
            LabRemark.Text = "";
            LabWoType.Text = "";
            LabRoute.Text = "";
            LabLine.Text = "";
            LabMasterWO.Text = "";
            LabStartProcess.Text = "";
            LabEndProcess.Text = "";
            LabReleaseMin.Text = "";
            LabReleaseMax.Text = "";

            btnRelease.Enabled = false;
            editDefault.Text = "";
            LabCode.Text = "";
            LabReleaseQty.Text = "";
            editQty.Text = "";
            editStart.Text = "";
            progressBar1.Visible = false;
            LabRuleName.Text = "";
            editUnitQty.Text = "";
        }
        private void editWO_KeyPress(object sender, KeyPressEventArgs e)
        {
            ClearData();
            if (e.KeyChar == (char)Keys.Return)
            {
                #region 每次展流程卡前先重置 SEQUENCE

                string sSQL = @"
DECLARE
    FOUND NUMBER;
BEGIN
    SELECT
        COUNT(1)
    INTO FOUND
    FROM
        ALL_SEQUENCES
    WHERE
        SEQUENCE_OWNER = 'SJ'
        AND SEQUENCE_NAME = 'S_RC_WOID';

    IF FOUND > 0 THEN
        EXECUTE IMMEDIATE 'DROP SEQUENCE S_RC_WOID';
    END IF;
END;
";
                ClientUtils.ExecuteSQL(sSQL);

                #endregion

                if (g_sLabType.ToUpper() == "MAC" && g_sType == "S")
                {
                    //子件在BOM表內
                    if (TMAC.sReleaseType == "1")
                    {
                        TMAC.sMacPartID = GetWOBOM_MACDefine(editWO.Text, ref TMAC.sMACPartNo);
                        if (TMAC.sMacPartID == "0")
                        {
                            SajetCommon.Show_Message("WO BOM Not Define MAC Part", 0);
                            return;
                        }
                    }
                }

                if (!Show_WoData(editWO.Text, false, ""))
                    return;

                //2010/03/11 rita MAC類型,先展序號再指給工單
                if (g_sLabType.ToUpper() == "MAC" && g_sType == "S")
                {
                    //子件在BOM表內
                    if (TMAC.sReleaseType == "1")
                    {
                        //TMAC.sMacPartID = GetWOBOM_MACDefine(editWO.Text,ref TMAC.sMACPartNo);
                        //if (TMAC.sMacPartID == "0")
                        //{
                        //    SajetCommon.Show_Message("WO BOM Not Define MAC Part", 0);
                        //    return;
                        //}
                        string sStartMAC = GetWOStartMAC(TMAC.sMacPartID);
                        if (string.IsNullOrEmpty(sStartMAC))
                        {
                            SajetCommon.Show_Message("No Available MAC", 0);
                            return;
                        }
                        editQty.Text = sStartMAC;
                    }
                }
            }
        }
        private string GetWOStartMAC(string sPartID)
        {
            sSQL = "SELECT MIN(" + g_sFieldName + ") MAC "
                 + "  FROM " + g_sTable
                 + " WHERE PART_ID ='" + sPartID + "' "
                 + "   AND RULE_NAME ='" + LabRuleName.Text + "' "
                 + "   AND WORK_ORDER IS NULL ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
                return dsTemp.Tables[0].Rows[0]["MAC"].ToString();
            else
                return "";
        }
        private string GetWOBOM_MACDefine(string sWO, ref string sPartNo)
        {
            sSQL = "SELECT D.PART_ID ,D.PART_NO "
                + "  FROM SAJET.G_WO_BASE A "
                + "      ,SAJET.G_WO_BOM B "
                + "      ,SAJET.SYS_PART D "
                + " WHERE A.WORK_ORDER =:WORK_ORDER "
                + "   AND B.WORK_ORDER = A.WORK_ORDER "
                + "   AND B.ITEM_PART_ID = D.PART_ID "
                + "   AND D.PART_TYPE ='MAC' "
                + "   AND ROWNUM = 1 ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWO };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                sPartNo = dsTemp.Tables[0].Rows[0]["PART_NO"].ToString();
                return dsTemp.Tables[0].Rows[0]["PART_ID"].ToString();
            }
            else
                return "0";
        }

        private void editDefault_Click(object sender, EventArgs e)
        {
            if (editDefault.Text == "")
                return;
            //規則中有空白時,跳出畫面供user修改Default值
            if (g_sType != "S" && g_sDefault.IndexOf(" ") > -1)
            {
                fChange fChg = new fChange();
                fChg.Text = "Change " + g_sLabType + " Label";
                fChg.sTitle = g_sLabType + " Label";
                fChg.editDefValue.Mask = editDefault.Mask;
                fChg.editDefValue.Text = g_sDefault;
                if (fChg.ShowDialog() == DialogResult.OK)
                {
                    editDefault.Text = fChg.editDefValue.Text;
                    Get_StartSeq();
                }
                fChg.Dispose();
            }
        }
        public void Get_StartSeq()
        {
            //日期格式=============================            
            string mDY = TSysDateCode.sDY;
            string mDM = TSysDateCode.sDM;
            string mDD = TSysDateCode.sDD;
            string mDW = TSysDateCode.sDW;
            string mDYW = TSysDateCode.sDYW;
            string mDK = TSysDateCode.sDK;
            //對應的User Define Day Code
            string mcDM = TSysDateCode.scDM;
            string mcDD = TSysDateCode.scDD;
            string mcDW = TSysDateCode.scDW;
            string mcDK = TSysDateCode.scDK;

            //若規則中有空白,會根據輸入的值產生不同的Sequence            
            TWoRule.g_sFRule = TWoRule.g_sRule;
            string sEmpty = "";
            for (int i = 0; i <= g_sDefault.Length - 1; i++)
            {
                if (g_sDefault[i].ToString() == " " && editDefault.Text[i].ToString() != " ")
                    sEmpty = sEmpty + editDefault.Text.Substring(i, 1);
            }
            if (sEmpty != "")
                TWoRule.g_sFRule = TWoRule.g_sRule + "_" + sEmpty;

            //最後一次Reset Sequence時間
            GetSequenceTime();

            //找目前的起始流水號            
            sSQL = " select Last_Number from all_sequences "
                 + " where sequence_name ='" + g_sSeq + TWoRule.g_sFRule.ToUpper() + "'"
                 + " and sequence_owner = user ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                Int64 iStart = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["Last_Number"].ToString());
                if (TWoRule.g_bReset)
                {
                    bool bRS = Reset_Seq(true);
                    editStart.Enabled = bRS;
                    if (bRS)
                    {
                        editStart.Text = LabelCheckDll.SeqTran(1, LabCode.Text, TWoRule.g_sSeqText, TWoRule.g_sCarry, LVUserSeq);
                    }
                    else
                    {
                        editStart.Text = LabelCheckDll.SeqTran(iStart, LabCode.Text, TWoRule.g_sSeqText, TWoRule.g_sCarry, LVUserSeq);//SeqTran(iStart);                        
                    }
                }
                else //2012/1/18 add
                {
                    editStart.Text = LabelCheckDll.SeqTran(iStart, LabCode.Text, TWoRule.g_sSeqText, TWoRule.g_sCarry, LVUserSeq);
                    editStart.Enabled = false;
                }
            }
            else
            {
                editStart.Enabled = true;
                editStart.Text = LabelCheckDll.SeqTran(1, LabCode.Text, TWoRule.g_sSeqText, TWoRule.g_sCarry, LVUserSeq);
                Reset_Seq(false);
            }
        }

        private void btnAuto_Click(object sender, EventArgs e)
        {
            if (editQty.Text == "")
            {
                //找此Rule尚未使用的號碼
                sSQL = " select " + g_sFieldName + " START_NO from " + g_sTable
                     + " where rule_name = '" + TWoRule.g_sRule + "' "
                     + " and Work_order is Null "
                     + " and NVL(PART_ID,0)  ='" + TMAC.sMacPartID + "' "
                     + " order by " + g_sFieldName;
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows.Count == 0)
                {
                    SajetCommon.Show_Message("All Number had been Used", 0);
                    return;
                }

                fFilter f = new fFilter();
                f.sSQL = sSQL;
                if (f.ShowDialog() == DialogResult.OK)
                {
                    editQty.Text = f.dgvData.CurrentRow.Cells["START_NO"].Value.ToString();
                }
                else
                {
                    return;
                }
            }

            //輸入數量後自動找出End No
            sSQL = " select " + g_sFieldName + ",PART_ID from " + g_sTable
                 + " where " + g_sFieldName + " = '" + editQty.Text + "' "
                 + " and rule_name = '" + TWoRule.g_sRule + "' ";

            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                SajetCommon.Show_Message("Start Number NG", 0);
                editQty.Focus();
                editQty.SelectAll();
                return;
            }
            if (TMAC.sReleaseType == "1")
            {
                if (dsTemp.Tables[0].Rows[0]["PART_ID"].ToString() != TMAC.sMacPartID)
                {
                    string sPartNo = SajetCommon.GetID("SAJET.SYS_PART", "PART_NO", "PART_ID", dsTemp.Tables[0].Rows[0]["PART_ID"].ToString());
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Start Number") + " : " + editQty.Text + Environment.NewLine
                                           + SajetCommon.SetLanguage("MAC is assigned to Part No") + " : " + sPartNo, 0);
                    editQty.Focus();
                    editQty.SelectAll();
                    return;
                }
            }

            fChange fChg = new fChange();
            fChg.Text = "Auto Calculate";
            fChg.sTitle = "Qty:";
            fChg.editDefValue.Text = "";
            fChg.editDefValue.Mask = "";
            if (fChg.ShowDialog() != DialogResult.OK)
                return;

            string sInputQty = fChg.editDefValue.Text;
            if (string.IsNullOrEmpty(sInputQty))
                return;
            try
            {
                Convert.ToInt32(sInputQty);
            }
            catch
            {
                string sErrMsg = SajetCommon.SetLanguage("Value must be the integer") + Environment.NewLine
                               + fChg.sTitle;
                SajetCommon.Show_Message(sErrMsg, 0); ;
                return;
            }

            sSQL = "Select " + g_sFieldName + " endmac from "
                + "  (select " + g_sFieldName + " from " + g_sTable
                + "   where " + g_sFieldName + " >= '" + editQty.Text + "' "
                + "   and rule_name = '" + TWoRule.g_sRule + "' "
                + "   and NVL(part_id,0) ='" + TMAC.sMacPartID + "' "
                + "   order by " + g_sFieldName + ") "
                + "where rownum <= '" + sInputQty + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            int iCount = dsTemp.Tables[0].Rows.Count;
            if (iCount < Convert.ToInt32(sInputQty))
            {
                string sErrMsg = SajetCommon.SetLanguage("Qty is not enough") + " (" + SajetCommon.SetLanguage("Max") + " : " + iCount.ToString() + ")" + Environment.NewLine
                               + SajetCommon.SetLanguage(g_sFieldName);
                SajetCommon.Show_Message(sErrMsg, 0);
                return;
            }
            editStart.Text = dsTemp.Tables[0].Rows[iCount - 1]["endmac"].ToString();
        }

        private void btnRelease_EnabledChanged(object sender, EventArgs e)
        {
            btnAuto.Enabled = btnRelease.Enabled;
        }

        private void btnPrintTestPage_Click(object sender, EventArgs e)
        {
            if (LabPart.Text == "N/A" || LabPart.Text == "")
            {
                SajetCommon.Show_Message("Please input Work Order", 1);
                return;
            }

            //首張列印測試                        
            listbPrint.Items.Clear();
            listbPrint.Items.Add(editDefault.Text);

            //先Link====
            PrintLabel.Setup PrintLabelDll = new PrintLabel.Setup();
            if (g_sSetup_PrintMethod.ToUpper() == "CODESOFT")
                PrintLabelDll.Open(g_sSetup_PrintMethod.ToUpper());
            else if (g_sSetup_PrintMethod.ToUpper() == "BARTENDER" && g_sSetup_PrintPort.ToUpper() == "STANDARD")
                PrintLabelDll.Open(g_sSetup_PrintMethod.ToUpper());
            else if (g_sSetup_PrintMethod.ToUpper() == "DLL")
                PrintLabelDll.Open(g_sSetup_PrintPort.ToUpper());

            //Print=====
            try
            {
                for (int i = 0; i <= listbPrint.Items.Count - 1; i++)
                {
                    string sMessage = "TestPage";
                    ListParam.Items.Clear();
                    ListData.Items.Clear();

                    ListParam.Items.Add(g_sFieldName);
                    ListData.Items.Add(listbPrint.Items[i].Text);
                    ListParam.Items.Add("WORK_ORDER");
                    ListData.Items.Add(editWO.Text);
                    ListParam.Items.Add("PART_NO");
                    ListData.Items.Add(LabPart.Text);

                    //開始列印
                    if (g_sSetup_PrintMethod.ToUpper() == "CODESOFT")
                        PrintLabelDll.Print_TestPage(g_sExeName, "PAGE", g_sFileName, "", 1, g_sSetup_PrintMethod, g_sSetup_CodeSoftVer, ListParam, ListData, out sMessage);
                    else
                        PrintLabelDll.Print_TestPage(g_sExeName, "PAGE", g_sFileName, "", 1, g_sSetup_PrintMethod, g_sSetup_PrintPort, ListParam, ListData, out sMessage);
                    if (sMessage != "OK")
                    {
                        SajetCommon.Show_Message(sMessage, 0);
                        return;
                    }
                }
            }
            finally
            {
                if (g_sSetup_PrintMethod.ToUpper() == "CODESOFT")
                    PrintLabelDll.Close(g_sSetup_PrintMethod.ToUpper());
                else
                    PrintLabelDll.Close(g_sSetup_PrintPort.ToUpper());
            }
        }
        private void fMain_Shown(object sender, EventArgs e)
        {
            editWO.Focus();
        }

        private void LabRange_Click(object sender, EventArgs e)
        {
            if (LabWO.Text == "" || Convert.ToInt32(LabReleaseQty.Text) == 0)
                return;

            if (g_sTable.ToUpper() == "SAJET.G_SN_STATUS" && g_sQtyField != "")
            {
                //檢視每個Panel下已展的序號範圍
                sSQL = " Select PANEL_NO,MIN(" + g_sFieldName + ") \"Start No\" "
                     + "       ,MAX(" + g_sFieldName + ") \"End No\" "
                     + "       ,COUNT(*) \"Total\" "
                     + " FROM " + g_sTable
                     + " WHERE WORK_ORDER = '" + LabWO.Text + "' "
                     + " group by panel_no "
                     + " order by panel_no ";
            }
            else
            {
                sSQL = " Select " + g_sFieldName + "\"List\" "
                     + " FROM " + g_sTable
                     + " WHERE WORK_ORDER = '" + LabWO.Text + "' "
                     + " order by " + g_sFieldName;
            }
            fList f = new fList();
            f.sSQL = sSQL;
            f.ShowDialog();
        }



        string[] slFields, slTypes;
        string sOracleTable;
        string[] DateTimeList = { "yyyy/M/d tt hh:mm:ss", "yyyy/MM/dd tt hh:mm:ss", "yyyy/MM/dd HH:mm:ss", "yyyy/M/d HH:mm:ss", "yyyy/M/d", "yyyy/MM/dd" };
        public byte TableStructure(string sTableName, string sColumns)
        {
            try
            {
                sOracleTable = sTableName;
                string[] slCoumns = sColumns.Split((char)9);
                string sSQL = "select column_name, data_type from all_tab_columns "
                            + "where owner || '.' || table_name = '" + sTableName + "'";
                object[] backparam = ClientUtils.ExecuteObjectSQL(sSQL);
                DataSet ds = new DataSet();
                if ((int)backparam[0] == 0)
                    ds = (DataSet)backparam[1];
                else
                {
                    return 1;
                }

                slFields = new string[ds.Tables[0].Rows.Count];
                slTypes = new string[ds.Tables[0].Rows.Count];
                int iCol = 0;
                foreach (string sCol in slCoumns)
                {
                    bool bExist = false;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (sCol == dr[0].ToString())
                        {
                            slTypes[iCol] = dr[1].ToString();
                            slFields[iCol] = sCol;
                            bExist = true;
                            iCol++;
                            break;
                        }
                    }
                    if (!bExist)
                    {
                        ClientUtils.ShowMessage("Field not Exist: " + sCol, 0);
                        return 2;
                    }
                }
                Array.Resize(ref slFields, iCol);
                Array.Resize(ref slTypes, iCol);
                return 0;
            }
            catch (Exception ex)
            {
                ClientUtils.ShowMessage(ex.Message, 0);
                return 3;
            }
        }

        private bool Save_SN_Batch(int iBatchQty, string[] slRow)
        {
            try
            {
                Array.Resize(ref slRow, iBatchQty);
                int iRow = 0, iCol = 0;
                string[] slData;

                object[][] Params = new object[slFields.Length][];
                for (int i = 0; i < slFields.Length; i++)
                    Params[i] = new object[slRow.Length];

                foreach (string sRow in slRow)
                {
                    slData = sRow.Split((char)9);
                    iCol = 0;
                    foreach (string sData in slData)
                    {
                        switch (slTypes[iCol])
                        {
                            case "DATE":
                                Params[iCol][iRow] = DateTime.ParseExact(sData, DateTimeList, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces);
                                break;
                            default:
                                Params[iCol][iRow] = sData;
                                break;
                        }
                        iCol++;
                    }
                    iRow++;
                }

                object[] backparam = ClientUtils.ExecuteBatch(Params, sOracleTable, slFields, slTypes);
                if ((int)backparam[0] != 0)
                {
                    string sError = backparam[1].ToString();

                    string sStartSN = slRow[0].Split((char)9)[1].ToString();
                    string sEndSN = slRow[slRow.Length - 1].Split((char)9)[1].ToString();

                    fMsg fError = new fMsg();
                    fError.richTextBox1.Text = sStartSN + " ~ " + sEndSN + Environment.NewLine
                                             + sError;
                    fError.ShowDialog();
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);
                return false;
            }
        }

        private void GetSequenceTime()
        {
            //最後一次Reset Sequence時間
            TWoRule.g_sMark = "";
            sSQL = " Select PARAME_VALUE From SAJET.SYS_MODULE_PARAM "
                 + " Where MODULE_NAME = '" + g_sSeq.ToUpper() + "' "
                 + " and FUNCTION_NAME ='" + TWoRule.g_sRule.ToUpper() + "' "
                 + " and PARAME_NAME = 'Reset Sequence Mark' ";
            if (TWoRule.g_sFRule != "")
                sSQL = sSQL + " and PARAME_ITEM = '" + TWoRule.g_sFRule.ToUpper() + "' ";
            else
                sSQL = sSQL + " and PARAME_ITEM is Null ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
                TWoRule.g_sMark = dsTemp.Tables[0].Rows[0]["PARAME_VALUE"].ToString();
        }
    }
}

