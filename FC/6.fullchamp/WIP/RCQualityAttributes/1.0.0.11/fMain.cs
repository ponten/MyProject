using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using SajetClass;
using System.IO;
using System.Data.OracleClient;
using System.Text.RegularExpressions;
using System.Collections;
using SPCConstantValue; // import SPCConstants.cs
using MathFormul;   // import MathFormula.cs

namespace RCQualityAttributes
{
    public partial class fMain : Form
    {
        string g_sFunction, g_sUserID, g_sProgram;
        string g_sFileName = Path.GetFileNameWithoutExtension(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileName);
        //int g_iPrivilege;

        string sSQL;
        DataSet dsTemp;

        int g_iDefectNum = 0;//记录测试小项sn的不良数量
        int g_NGCnt = 0;
        int g_iFlag = 0; //界面初始化标志
        bool bHaveSPC = false; //測試小項是否關聯SPC
        ArrayList g_alItemRule;
        //ArrayList g_alItemRuleDesc;

        ArrayList g_ControlLimit = new ArrayList(); // all test item control limit list
        //decimal[] arrData; // UCL, CL, LCL
        DateTime dSetTime;   // RCTravel 顯示的時間在sys_base的Closing Date設定
        bool bSetTime = false;  // 判定是否有設定關帳時間

        public struct TRCRecord
        {
            public bool bRCInput;
            public bool bEmpInput;
            public bool bDefecCodetInput;
            public bool bOutputQtyInput;
            public string sRC;
            public string sWorkOrder;
            public string sPartNo;
            public string sPartID;
            public string sVersion;
            public string sRouteID;
            public string sWIPProcessID;//制程ID
            public string sWIPProcessName;//制程名
            //public int iInputQty; //rc投入SN数量
            public double iInputQty; 
            public int iOutputQty;
            public string sWorkFlag;
            public string sEmpID;
            public string sEmpName;
            public string sNextProcessID;
            public string sCurrentStatus;
            public int iTotalOutputQtyPass;
            public int iTotalOutputQtyFail;
            public string sQCResult;
            public string sTravelCount;//RC在改制程出现的次数
            public bool bHaveSN;
            public string sProcessType; //判定製程別為IPQC或QC
            public string sTestTypeResult;
            public int iCriticalRej;            // 主缺數量
            public int iMajorRej;              // 重
            public int iMinorRej;              // 次
            public string sCPKResult;
        }

        public struct TSampleType
        {
            public String sSamplingID;//抽验计划ID
            public String sSamplingType;//抽验计划类型
            public String sSamplingLevelID;//抽验标准ID
            public String sSamplingLevelName;//抽验标准名称
            public int iSampleQty;//抽验数量
            public int iCriticalRej;//重缺
            public int iMajorRej;//主缺
            public int iMinorRej;//次缺
            public String sSamplingUnit;
            public int iNGcnt;
        }

        //string g_sSampleLevelID;
        string g_sSampleLevel = "0,1,2,3";//"正常,加嚴,減量,免驗"
        bool g_bChangeALL, g_bChangeAQL;
        
        public struct TQCLot
        {
            public string sLotNo;//检验批号
            public string sPartID;//料号
            public int iLotSize;//测试计划设定的检验数量
            public string[] lstSampleLevel;//检验标准（正常/加严/免检）
            public string sRECID;
            public string time;
        }
       
        public TQCLot G_tQCLot = new TQCLot();
        public TRCRecord G_tRCRecord = new TRCRecord();

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
        
        // SPC Data
        public struct TSPC
        {
            public string g_sChartType;
            public string g_sUCL1;
            public string g_sCL1;
            public string  g_sLCL1; 
            public decimal  g_AvgX;
            public decimal g_AvgR;
            public decimal Ca;
            public decimal Cp;
            public decimal Cpk;
            public string sCaLevel;
            public string sCpLevel;
            public string sCpkLevel;
        }
        TSPC tspc = new TSPC();

        //public fMain(string nParam, string strSN, string sEmp) yyyyMMddHHmmss
        public fMain(string nParam, string strSN, string sEmp, string sTime)
        {
            InitializeComponent();
            this.ClientSize = new System.Drawing.Size(893, 645);
            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
            // 過站時間
            //if (!string.IsNullOrEmpty(sTime))
            //{
                //if (DateTime.TryParse(sTime, out dSetTime))
                //if (DateTime.TryParse(sTime,System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out dSetTime))
                //{
                //    bSetTime = true;
                //}
                //else
                //{
                //    bSetTime = false;
                //}
            try
            {
                dSetTime =  DateTime.ParseExact(sTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                bSetTime = true;
            }
            catch (Exception)
            {
                bSetTime = false;
            }

            // 初始SPC Moniter Table
            Interface.tableData = new Dictionary<string, string>();
            Interface.InitTable();

            //正常窗体加载
            Form_Load();

            G_tQCLot.lstSampleLevel = g_sSampleLevel.Split(',');

            //员工ID带出
            this.Text = SajetCommon.SetLanguage(nParam) +"-" + Assembly.GetExecutingAssembly().GetName().Version;
            editEmployee.Text = sEmp;
            GetEditEmpName();
   
            //带出RC，通过RC带出其他相关资料
            editRC.Text = strSN;
            ShowDetail();

            SajetCommon.SetLanguageControl(this);

            //txtSN.Focus();
            //txtSN.SelectAll();
        }

        //modify by hidy 2015/10/16 制程内QC窗体加载调用该方法
        private void Form_Load()
        {
            g_sUserID = ClientUtils.UserPara1;
            g_sProgram = ClientUtils.fProgramName;
            g_sFunction = ClientUtils.fFunctionName;
            panel1.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            panel1.BackgroundImageLayout = ImageLayout.Stretch;

            this.tabPage2.Parent = null;
            this.tabPage3.Parent = null;
        }
        private void fMain_Shown(object sender, EventArgs e)
        {

        }
        //所有测试全部完成，关闭窗口
        private void btnFINISH_Click(object sender, EventArgs e)
        {
            if (!CheckIsFinished())
            {
                string sMsg = SajetCommon.SetLanguage("Test item type is not finished");//测试大项未完成
                SajetCommon.Show_Message(sMsg, 1);
                return;
            }

            //if (dgvItem.Rows.Count == 0 && int.Parse(txtCount.Text) > 0)
            //{
            //    string sMsg = SajetCommon.SetLanguage("Please Re-inspect");//跳站重工
            //    SajetCommon.Show_Message(sMsg, 1);
            //    return;
            //}

            if (editPassQty.Text == "" || editFailQty.Text == "" || editResult.Text == "")
            {
                string sMsg = SajetCommon.SetLanguage("Result Empty");
                SajetCommon.Show_Message(sMsg, 1);
                return;
            }

            if (!CheckDataValidation(editPassQty.Text, "^\\d+(\\.\\d+)?$"))    // if (!CheckDataValidation(editPassQty.Text, "^[0-9]+$"))               
            {
                string sMsg = SajetCommon.SetLanguage("Data invalid");
                SajetCommon.Show_Message(sMsg, 1);
                editPassQty.SelectAll();
                editPassQty.Focus();
                return;
            }

            if (!CheckDataValidation(editFailQty.Text, "^\\d+(\\.\\d+)?$"))     // if (!CheckDataValidation(editFailQty.Text, "^[0-9]+$"))
            {
                string sMsg = SajetCommon.SetLanguage("Data invalid");
                SajetCommon.Show_Message(sMsg, 1);
                editFailQty.SelectAll();
                editFailQty.Focus();
                return;
            }
            // 小數點
            //if (int.Parse(editPassQty.Text) + int.Parse(editFailQty.Text) != int.Parse(txtInputQty.Text))
            if (double.Parse(editPassQty.Text) + double.Parse(editFailQty.Text) != double.Parse(txtInputQty.Text))
            {
                string sMsg = SajetCommon.SetLanguage("Pass + Fail != Input");
                SajetCommon.Show_Message(sMsg, 1);
                return;
            }

            // 不良產出數比對新增的不良數量
            double dQty = 0;
            for (int i = 0; i < dgvDefect.Rows.Count; i++)
            {
                dQty = dQty + Convert.ToDouble(dgvDefect.Rows[i].Cells["DEFECT_QTY"].Value.ToString());
            }
            if (double.Parse(editFailQty.Text) < dQty)
            {
                string sMsg = SajetCommon.SetLanguage("Fail quantity less than defect quantity.");
                SajetCommon.Show_Message(sMsg, 1);
                return;
            }

            if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Finish QCLot Confirm") + " : " + "Y/N?", 2) == DialogResult.No)
                return;

            string sSNList = string.Empty;
            string sSNTemp = string.Empty;
            if (G_tRCRecord.bHaveSN)
            {
                string sSql = @"SELECT B.SERIAL_NUMBER,
                                                DECODE(A.SERIAL_NUMBER, NULL, 'OK', INSP_RESULT) INSP_RESULT
                                       FROM (SELECT SERIAL_NUMBER, MIN(INSP_RESULT) INSP_RESULT
                                                   FROM SAJET.G_QC_SN_TESTITEM_TEMP
                                                 WHERE QC_LOTNO = :QC_LOTNO
                                                 GROUP BY SERIAL_NUMBER) A,
                                               SAJET.G_SN_STATUS B
                                     WHERE B.SERIAL_NUMBER = A.SERIAL_NUMBER(+)
                                         AND B.RC_NO = :RC_NO
                                         AND B.CURRENT_STATUS IN ('0','1')";
                object[][] Params1 = new object[2][];
                Params1[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "QC_LOTNO", G_tQCLot.sLotNo };
                Params1[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", editRC.Text };
                DataSet ds1 = ClientUtils.ExecuteSQL(sSql, Params1);
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    sSNTemp = ds1.Tables[0].Rows[i]["SERIAL_NUMBER"].ToString() + (char)9 +
                        ds1.Tables[0].Rows[i]["INSP_RESULT"].ToString() + (char)9 +
                        ((ds1.Tables[0].Rows[i]["INSP_RESULT"].ToString() == "OK") ? "1" : "0") + (char)9 +
                        ((ds1.Tables[0].Rows[i]["INSP_RESULT"].ToString() == "NG") ? "1" : "0");
                    sSNList += sSNTemp + (char)27;
                }
            }
            string sDefect = string.Empty;
            foreach (DataGridViewRow dr in dgvDefect.Rows)
            {
                if (G_tRCRecord.bHaveSN)
                    sDefect += dr.Cells["SERIAL_NUMBER"].EditedFormattedValue.ToString() + (char)9
                        + dr.Cells["DEFECT_CODE"].EditedFormattedValue.ToString() + (char)9
                        + dr.Cells["DEFECT_QTY"].EditedFormattedValue.ToString() + (char)27;
                else
                    if (dr.Cells["DEFECT_QTY"].EditedFormattedValue.ToString() != "0")
                    {
                        sDefect += dr.Cells["DEFECT_CODE"].EditedFormattedValue.ToString() + (char)9
                            + dr.Cells["DEFECT_QTY"].EditedFormattedValue.ToString() + (char)27;
                    }
            }

            G_tRCRecord.sCurrentStatus = "0";
            // 測試大項NG或IPQC站CPK結果NG要停在當站 
            //if (G_tRCRecord.sTestTypeResult == "1" || (G_tRCRecord.sProcessType.ToUpper() == "IPQC" && G_tRCRecord.sCPKResult == "1"))
            if(editResult.Text == "NG")
            {
                G_tRCRecord.sCurrentStatus = "1";
                string sSQL = " SELECT * FROM SAJET.G_RC_STATUS WHERE RC_NO ='" + editRC.Text + "' ";
                DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    rcRoute.sNext_Process = dsTemp.Tables[0].Rows[0]["PROCESS_ID"].ToString();                   
                    rcRoute.sNext_Node = dsTemp.Tables[0].Rows[0]["NODE_ID"].ToString();
                    rcRoute.sSheet_Name = dsTemp.Tables[0].Rows[0]["SHEET_NAME"].ToString();
                }
                // IPQC回流不Hold，在PROCEDURE SJ_RC_OUTPUT先記錄不良率再把CURRENT STATUS = 0
                //if(G_tRCRecord.sProcessType.ToUpper() == "IPQC")
                //    G_tRCRecord.sCurrentStatus = "0";
            }
            else
            {
                // 選擇下一製程
                if (!setNextProcess())
                    return;
            }
            
            if(!bSetTime)
                dSetTime = System.DateTime.Now;

            object[][] Params = new object[17][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TEMP", editEmployee.Text };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TRC", editRC.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TITEM", "" };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TDEFECT", sDefect };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSN", sSNList };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TMEMO", "" };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TGOOD", editPassQty.Text };                                                                         // 
            Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSCRAP", editFailQty.Text };
            //Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TGOOD", dgvItemType.CurrentRow.Cells["PASS_QTY"].ToString() };
            //Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSCRAP", dgvItemType.CurrentRow.Cells["FAIL_QTY"].ToString() };
            Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TNEXTPROCESS", rcRoute.sNext_Process };
            Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TNEXTNODE", rcRoute.sNext_Node };
            Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TNEXTSHEET", rcRoute.sSheet_Name };
            Params[11] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TBONUS", "0" };
            Params[12] = new object[] { ParameterDirection.Input, OracleType.DateTime, "TNOW", dSetTime };
            Params[13] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSTATUS", G_tRCRecord.sCurrentStatus };
            Params[14] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TMACHINE", "" };
            Params[15] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TKEYPART", "" };
            Params[16] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
            DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_RC_OUTPUT", Params);  // V9
            string ssMsg = ds.Tables[0].Rows[0]["TRES"].ToString();
            if (ssMsg != "OK")
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage(ssMsg), 0);
            }
            else
            {
                // 清除temp data
                btnAgain_Click(sender, e);
            }

            this.Close();
        }

        //private void Initial_Form()
        //{
        //    g_sUserID = ClientUtils.UserPara1;
        //    g_sProgram = ClientUtils.fProgramName;
        //    g_sFunction = ClientUtils.fFunctionName;
        //    SajetCommon.SetLanguageControl(this);
        //}

        //private void checkprivilege()
        //{
        //    g_iPrivilege = ClientUtils.GetPrivilege(g_sUserID, "By Pass", g_sProgram);
        //}

        //modify by hidy 2015/10/16 获取员工ID对应的员工名
        private void GetEditEmpName()
        {
            editEmployee.Text = editEmployee.Text.Trim();
            string sMessage = string.Empty;
            try
            {
                if (!CheckEmployee(editEmployee.Text, ref sMessage))
                {
                    return;
                }
                //檢查有沒有模組權限
                int iPrivilege = ClientUtils.GetPrivilege(G_tRCRecord.sEmpID, g_sFunction, g_sProgram);
                if (iPrivilege == 0)
                {
                    sMessage = lablemp.Text + " : " + G_tRCRecord.sEmpName + " " + SajetCommon.SetLanguage("Can not use this function");
                    return;
                }
                G_tRCRecord.bEmpInput = true;
                editEmpName.Text = G_tRCRecord.sEmpName;
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message,1);
                return;
            }
        }

        private bool CheckEmployee(string sEmpNo, ref string sMessage)
        {
            try
            {
                sSQL = "SELECT A.EMP_ID,A.EMP_NAME,SYSDATE-(NVL(A.QUIT_DATE,SYSDATE)+1) QUIT_DATE  "
                    + "  FROM SAJET.SYS_EMP A "
                    + " WHERE A.EMP_NO =:EMP_NO "
                    + "   AND ROWNUM = 1 ";
                object[][] Params = new object[1][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "EMP_NO", sEmpNo };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                if (dsTemp.Tables[0].Rows.Count == 0)
                {
                    sMessage = "Employee Error";
                    return false;
                }
                if (Convert.ToDouble(dsTemp.Tables[0].Rows[0]["QUIT_DATE"].ToString()) > 0)
                {
                    sMessage = "Employee Terminate";
                    return false;
                }
                G_tRCRecord.sEmpID = dsTemp.Tables[0].Rows[0]["EMP_ID"].ToString();
                G_tRCRecord.sEmpName = dsTemp.Tables[0].Rows[0]["EMP_NAME"].ToString();
                return true;
            }
            catch (Exception ex)
            {
                sMessage = SajetCommon.SetLanguage("Error") + " : " + "(CheckEmployee) " + SajetCommon.SetLanguage("occur Exception") + Environment.NewLine
                          + ex.Message;
                return false;
            }
        }

        //给文本框赋值
        private void DisplayEditData()
        {
            txtPartNo.Text = G_tRCRecord.sPartNo;
            txtRCProcess.Text = G_tRCRecord.sWIPProcessName;
            txtInputQty.Text = G_tRCRecord.iInputQty.ToString();
            txtWO.Text = G_tRCRecord.sWorkOrder;
            txtCount.Text = G_tRCRecord.sTravelCount;
            txtLotNo.Text = G_tQCLot.sLotNo;
            txtType.Text = G_tRCRecord.sWIPProcessName;
            txtVersion.Text = G_tRCRecord.sVersion;
        }

        /// <summary>
        /// 检查RC是否有效，并且得到RC相关的信息
        /// </summary>
        /// <param name="rRC">RC</param>
        /// <param name="sMessage">报错信息</param>
        /// <returns>检查结果</returns>
        private bool CheckRCValid(ref TRCRecord rRC, ref string sMessage)
        {
            sMessage = string.Empty;
            try
            {
                sSQL = @" SELECT *
                                    FROM SAJET.G_RC_STATUS A, SAJET.SYS_PART B, SAJET.SYS_PROCESS C, SAJET.SYS_OPERATE_TYPE D
                                    WHERE A.RC_NO = :RC_NO
                                    AND A.PART_ID = B.PART_ID
                                    AND A.PROCESS_ID = C.PROCESS_ID
                                    AND D.OPERATE_ID = C.OPERATE_ID ";

                object[][] Params = new object[1][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", rRC.sRC };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                if (dsTemp.Tables[0].Rows.Count == 0)
                {
                    sMessage = SajetCommon.SetLanguage("R/C") + " : " + rRC.sRC + " " + SajetCommon.SetLanguage("Error");
                    return false;
                }
                DataRow dr = dsTemp.Tables[0].Rows[0];
                 
                
                rRC.sRC = dr["RC_NO"].ToString();
                rRC.sWorkOrder = dr["WORK_ORDER"].ToString();
                rRC.sPartNo = dr["PART_NO"].ToString();
                rRC.sPartID = dr["PART_ID"].ToString();
                //rRC.iInputQty = Convert.ToInt32(dr["CURRENT_QTY"].ToString());
                rRC.iInputQty = Convert.ToDouble(dr["CURRENT_QTY"].ToString());
                rRC.sWIPProcessID = dr["PROCESS_ID"].ToString();
                rRC.sWIPProcessName = dr["PROCESS_NAME"].ToString();
                rRC.sVersion = dr["VERSION"].ToString();
                rRC.bHaveSN = (dr["HAVE_SN"].ToString() == "0") ? false : true;
                rRC.sProcessType = dr["TYPE_NAME"].ToString();

                //获取该RC在该制程过站次数
                sSQL = @"SELECT COUNT(*) TRAVEL_COUNT FROM SAJET.G_RC_TRAVEL WHERE RC_NO = :RC_NO AND PROCESS_ID = :PROCESS_ID";
                Params = new object[2][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", rRC.sRC };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", rRC.sWIPProcessID };
                dsTemp = ClientUtils.ExecuteSQL(sSQL,Params);
                rRC.sTravelCount = dsTemp.Tables[0].Rows[0]["TRAVEL_COUNT"].ToString();

                //DisplayEditData();
                return true;
            }
            catch (Exception ex)
            {
                sMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 生成检验批号，检查检验批号是否测试过
        /// </summary>
        /// <param name="RC">组成部分之RC</param>
        /// <param name="Process">组成部分之Process</param>
        /// <returns>0:没有此类型批号，组成为RC+PROCESS+001；
        /// 1:有此类型批号且未完成测试，组成为此类型最大的一笔QCLOTNO；
        /// 2:有此类型批号且测试完成，组成为此类型最大的一笔QCLOTNO的最后三位流水+1</returns>
        private string GetQCNo(string sRC, string sProcess,  out string sQCNo)
        {
            sQCNo = sRC + sProcess;
            try
            {
                string sSQL = "select * from sajet.g_qc_lot where qc_lotno like '" + sQCNo + "%' order by qc_lotno desc";
                DataTable dt = ClientUtils.ExecuteSQL(sSQL).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["QC_RESULT"].ToString() == "N/A")
                    {
                        sQCNo = dt.Rows[0]["QC_LOTNO"].ToString();
                        return "1";
                    }
                    else
                    {
                        string sTemp = dt.Rows[0]["QC_LOTNO"].ToString();
                        sQCNo = sQCNo + (int.Parse(sTemp.Substring(sTemp.Length - 3)) + 1).ToString().PadLeft(3, '0');
                        return "2";
                    }
                }
                else
                {
                    sQCNo = sQCNo + "001";
                    return "0";
                }
            }
            catch
            {
                return "-1";
            }
        }

        public void ShowDetail()
        {
            string sRC = editRC.Text.Trim();
            string sMessage = string.Empty;
            try
            {
                G_tRCRecord.sRC = sRC;

                //检查并获取RC有关数据
                if (!CheckRCValid(ref G_tRCRecord, ref sMessage))
                    return;

                //获取检验批号
                string sQCLotNo = "";
                if (GetQCNo(G_tRCRecord.sRC, G_tRCRecord.sWIPProcessID, out sQCLotNo) == "1")
                {
                    //SetDefect(sQCLotNo, dgvItemType.Rows[0].Cells["ITEM_TYPE_ID"].Value.ToString());
                }
                G_tQCLot.sLotNo = sQCLotNo;

                if (G_tRCRecord.sProcessType == "IPQC")
                    btnCPK.Visible = true;

                DisplayEditData();

                //檢查抽驗計畫，给dgvItemType赋值
                GetSYSTestType();
                           
                SetResult();

                //如果没有sn，则不显示不良代码处的sn栏位
                //SetDefectVisible();
            }
            catch(Exception ex)
            {
                SajetCommon.Show_Message(ex.Message,1);
                return;
            }
        }

        # region 測試大項程式區域
        /// <summary>
        /// 获取当前RC在当前站绑定的测试大项和抽验计划
        /// </summary>
        private void GetSYSTestType()
        {
            //获取当前制程RC绑定的测试大项及默认抽验计划
            dgvItemType.Rows.Clear();
            sSQL = " SELECT A.*,B.ITEM_TYPE_ID,NVL(D.ITEM_TYPE_CODE,B.ITEM_TYPE_ID) ITEM_TYPE_CODE ,D.ITEM_TYPE_NAME "
                + "        ,NVL(C.SAMPLING_TYPE,'N/A') SAMPLING_TYPE ,NVL(C.SAMPLING_ID,'0') SAMPLING_ID "
                + "     FROM SAJET.SYS_PART_QC_PROCESS_RULE  A "
                + "       ,SAJET.SYS_PART_QC_TESTTYPE B "
                + "       ,SAJET.SYS_QC_SAMPLING_PLAN  C "
                + "       ,SAJET.SYS_TEST_ITEM_TYPE D "
                + "  WHERE A.PART_ID = :PART_ID "
                + "    AND A.PROCESS_ID =:PROCESS_ID "
                + "    AND A.ENABLED='Y' "
                + "    AND B.RECID = A.RECID "
                + "    AND B.SAMPLING_ID = C.SAMPLING_ID(+) "
                + "    AND B.ITEM_TYPE_ID = D.ITEM_TYPE_ID(+) "
                + "  ORDER BY D.ITEM_TYPE_CODE ";
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", G_tRCRecord.sPartID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", G_tRCRecord.sWIPProcessID };
            DataSet ddsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (ddsTemp.Tables[0].Rows.Count == 0)
                return;
            for (int i = 0; i < ddsTemp.Tables[0].Rows.Count; i++)
            {
                G_tQCLot.sRECID = ddsTemp.Tables[0].Rows[i]["RECID"].ToString();
                //G_tQCLot.time = ddsTemp.Tables[0].Rows[i]["UPDATE_TIME"].ToString();
                string sSamplingID = ddsTemp.Tables[0].Rows[i]["SAMPLING_ID"].ToString();
                string sSamplingType = ddsTemp.Tables[0].Rows[i]["SAMPLING_TYPE"].ToString();
                string sItemTypeCode = ddsTemp.Tables[0].Rows[i]["ITEM_TYPE_CODE"].ToString();
                string sItemTypeID = ddsTemp.Tables[0].Rows[i]["ITEM_TYPE_ID"].ToString();
                string sItemTypeName = ddsTemp.Tables[0].Rows[i]["ITEM_TYPE_NAME"].ToString();

                //如果此大项已经有记录，则获取当前的抽验计划
                sSQL = @"SELECT QC_RESULT 
                                   FROM SAJET.G_QC_LOT_TEST_TYPE A 
                                  WHERE A.QC_LOTNO = :LOT_NO AND A.ITEM_TYPE_ID = :ITEM_TYPE_ID";
                Params = new object[2][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", G_tQCLot.sLotNo };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", sItemTypeID };
                DataSet dsTemp1 = ClientUtils.ExecuteSQL(sSQL, Params);

                string sPass_Qty = string.Empty;
                string sFail_Qty = string.Empty;
                string sResult = string.Empty;
                string sSamplingLevelID = G_tQCLot.lstSampleLevel[0];
                string sSamplingLevelName;
                bool bCSP = false;

                if(dsTemp1.Tables[0].Rows.Count >0)
                {
                    if (dsTemp1.Tables[0].Rows[0]["QC_RESULT"].ToString() == "N/A")
                        bCSP = GetSamplingPlan_From_Rule(sItemTypeID, ref sSamplingType, ref sSamplingLevelID, ref sSamplingID); // sItemTypeName
                    else
                        bCSP = true;
                }
                //else
                //{
                //    bCSP = GetSamplingPlan_From_Rule(sItemTypeID, ref sItemTypeName, ref sSamplingLevelID, ref sSamplingID);
                //}

                DataSet dsTemp2 = new DataSet();
                if (dsTemp1.Tables[0].Rows.Count ==0)
                {
                    sPass_Qty = "0";
                    sFail_Qty = "0";
                    sResult = "N/A";
                }
                else
                {
                    // 增加抽驗次數資料
                    sSQL = @"SELECT NVL(A.PASS_QTY,0) PASS_QTY,NVL(A.FAIL_QTY,0)FAIL_QTY,
                                            DECODE(A.QC_RESULT,'0','OK','1','NG','N/A') QC_RESULT, SAMPLING_LEVEL
                                            , NG_CNT  
                                        FROM SAJET.G_QC_LOT_TEST_TYPE A 
                                        WHERE A.QC_LOTNO = :LOT_NO
                                        AND A.ITEM_TYPE_ID = :ITEM_TYPE_ID";
                    Params = new object[2][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", G_tQCLot.sLotNo };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", sItemTypeID };
                    dsTemp2 = ClientUtils.ExecuteSQL(sSQL, Params);
                    sPass_Qty = dsTemp2.Tables[0].Rows[0]["PASS_QTY"].ToString();
                    sFail_Qty = dsTemp2.Tables[0].Rows[0]["FAIL_QTY"].ToString();
                    sResult = dsTemp2.Tables[0].Rows[0]["QC_RESULT"].ToString();
                    g_NGCnt = Convert.ToInt32(dsTemp2.Tables[0].Rows[0]["NG_CNT"].ToString());
                    if (sResult == "N/A") // 測試大項已完成不可以再調整抽驗等級
                    {
                        if ((bCSP || g_iFlag != 0) && g_NGCnt >1)
                        {
                            sSamplingLevelID = dsTemp2.Tables[0].Rows[0]["SAMPLING_LEVEL"].ToString();
                        }
                    }
                    else
                    {
                        sSamplingLevelID = dsTemp2.Tables[0].Rows[0]["SAMPLING_LEVEL"].ToString();
                    }
                    sSamplingLevelName = GetSamplingLevelName(sSamplingLevelID);    
                }

                //根据当前的抽验计划，获取抽验数量，并且更新抽验计划
                TSampleType rSampleType = new TSampleType();
                rSampleType = getSamplingPlanRange(sSamplingID, sSamplingLevelID, G_tRCRecord.iInputQty);
                G_tQCLot.iLotSize = rSampleType.iSampleQty;

                //此处会跳转至selectionchanged事件,g_iFlag用来决定是否进入该事件
                g_iFlag = 0;
                dgvItemType.Rows.Add();
                dgvItemType.Rows[i].Cells["ITEM_TYPE_ID"].Value = sItemTypeID;
                dgvItemType.Rows[i].Cells["TYPE_CODE"].Value = sItemTypeCode;
                dgvItemType.Rows[i].Cells["TYPE_NAME"].Value = sItemTypeName;
                dgvItemType.Rows[i].Cells["SAMPLE_ID"].Value = sSamplingID;
                dgvItemType.Rows[i].Cells["SAMPLE_TYPE"].Value = sSamplingType;
                dgvItemType.Rows[i].Cells["SAMPLE_LEVEL"].Value = rSampleType.sSamplingLevelName;
                dgvItemType.Rows[i].Cells["SAMPLE_SIZE"].Value = rSampleType.iSampleQty;
                dgvItemType.Rows[i].Cells["PASS_QTY"].Value = sPass_Qty;
                dgvItemType.Rows[i].Cells["FAIL_QTY"].Value = sFail_Qty;
                dgvItemType.Rows[i].Cells["QC_RESULT"].Value = sResult;
                dgvItemType.Rows[i].Cells["SAMPLE_LEVEL_ID"].Value = rSampleType.sSamplingLevelID;
                //dgvItemType.Rows[i].Cells["FINISH_ITEM"].Value = SajetCommon.SetLanguage("Finish Item");

                if (dgvItemType.Rows[dgvItemType.Rows.Count - 1].Cells["QC_RESULT"].Value.ToString() == "OK")
                {
                    dgvItemType.Rows[dgvItemType.Rows.Count - 1].Cells["QC_RESULT"].Style.BackColor = Color.Green;
                    dgvItemType.Rows[dgvItemType.Rows.Count - 1].Cells["QC_RESULT"].Style.ForeColor = Color.White;
                }
                if (dgvItemType.Rows[dgvItemType.Rows.Count - 1].Cells["QC_RESULT"].Value.ToString() == "NG")
                {
                    dgvItemType.Rows[dgvItemType.Rows.Count - 1].Cells["QC_RESULT"].Style.BackColor = Color.Red;
                    dgvItemType.Rows[dgvItemType.Rows.Count - 1].Cells["QC_RESULT"].Style.ForeColor = Color.White;
                }

                if (g_iFlag != 1)
                    g_iFlag = 1;
            }

            SetItem(dgvItemType.Rows[0].Cells["ITEM_TYPE_ID"].Value.ToString());
            SetItemValue(G_tRCRecord.sWIPProcessID, dgvItemType.Rows[0].Cells["ITEM_TYPE_ID"].Value.ToString());
            //判断当前大项是否已完成测试，已完成的不能修改测试值
            SetTestItemVisible(dgvItemType.Rows[0].Cells["QC_RESULT"].Value.ToString());

            showProcessDefect(dgvItemType.Rows[0].Cells["ITEM_TYPE_ID"].Value.ToString());
            SetDefect(G_tQCLot.sLotNo, dgvItemType.CurrentRow.Cells["ITEM_TYPE_ID"].Value.ToString());
            SetDefectVisible(dgvItemType.Rows[0].Cells["QC_RESULT"].Value.ToString());
        }
        /// <summary>
        /// 根据抽验计划，抽验等级，样本总数获取抽验数量范围
        /// </summary>
        /// <param name="sSampleID">抽验计划</param>
        /// <param name="sSampleLevel">抽验等级</param>
        /// <param name="iLotSize">样本总数</param>
        /// <returns>抽验计划数量范围</returns>
        public TSampleType getSamplingPlanRange(string sSampleID, string sSampleLevel, double iLotSize)
        {
            TSampleType rSampleType = new TSampleType();
            rSampleType.sSamplingID = sSampleID;
            //rSampleType.iCriticalRej = 0;
            //rSampleType.iMajorRej = 0;
            //rSampleType.iMinorRej = 0;
            //rSampleType.iSampleQty = 0;
            //rSampleType.sSamplingType = "N/A";
            //rSampleType.sSamplingLevelID = sSampleLevel;
            //rSampleType.sSamplingLevelName = G_tQCLot.lstSampleLevel[Convert.ToInt32(sSampleLevel)];
            //rSampleType.sSamplingUnit = "Qty";

            sSQL = "SELECT A.SAMPLING_ID,A.SAMPLING_TYPE "
                + "       ,NVL(B.SAMPLE_SIZE,0) SAMPLE_SIZE ,NVL(B.CRITICAL_REJECT_QTY,0) CRITICAL_REJECT_QTY "
                + "       ,NVL(B.MAJOR_REJECT_QTY,0) MAJOR_REJECT_QTY ,NVL(B.MINOR_REJECT_QTY,0) MINOR_REJECT_QTY "
                + "       ,B.SAMPLING_LEVEL  "
                + "       ,NVL(B.SAMPLING_UNIT,'0') SAMPLING_UNIT "
                + " FROM SAJET.SYS_QC_SAMPLING_PLAN A  "
                + "     ,SAJET.SYS_QC_SAMPLING_PLAN_DETAIL B "
                + " WHERE A.SAMPLING_ID = :SAMPLING_ID "
                + "   AND A.SAMPLING_ID = B.SAMPLING_ID "
                + "   AND B.SAMPLING_LEVEL =:SAMPLING_LEVEL "
                + "   AND B.MIN_LOT_SIZE <= :LOT_SIZE1 "
                + "   AND B.MAX_LOT_SIZE >= :LOT_SIZE2 ";
            object[][] Params = new object[4][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SAMPLING_ID", sSampleID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SAMPLING_LEVEL", sSampleLevel };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.Int32, "LOT_SIZE1", iLotSize };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.Int32, "LOT_SIZE2", iLotSize };

            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                rSampleType.iSampleQty = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["SAMPLE_SIZE"].ToString());
                rSampleType.sSamplingUnit = "Qty";
                if (dsTemp.Tables[0].Rows[0]["SAMPLING_UNIT"].ToString() == "1")//使用%當作單位
                {
                    rSampleType.sSamplingUnit = "%";
                    Double dSampleSize = Convert.ToDouble(dsTemp.Tables[0].Rows[0]["SAMPLE_SIZE"].ToString()); ;
                    Double iSampleSize = Math.Round(G_tRCRecord.iInputQty * (dSampleSize / 100));
                    rSampleType.iSampleQty = (int)iSampleSize;
                    rSampleType.sSamplingUnit = dsTemp.Tables[0].Rows[0]["SAMPLE_SIZE"].ToString() + "%";
                }
                if (rSampleType.iSampleQty > Convert.ToInt32(Math.Round(iLotSize, MidpointRounding.AwayFromZero))) // if (rSampleType.iSampleQty > iLotSize)
                {
                    rSampleType.iSampleQty = Convert.ToInt32(Math.Round(iLotSize, MidpointRounding.AwayFromZero));   //rSampleType.iSampleQty = iLotSize;
                }
                rSampleType.iCriticalRej = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["CRITICAL_REJECT_QTY"].ToString());
                rSampleType.iMajorRej = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["MAJOR_REJECT_QTY"].ToString());
                rSampleType.iMinorRej = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["MINOR_REJECT_QTY"].ToString());
                rSampleType.sSamplingType = dsTemp.Tables[0].Rows[0]["SAMPLING_TYPE"].ToString();
                rSampleType.sSamplingLevelID = dsTemp.Tables[0].Rows[0]["SAMPLING_LEVEL"].ToString();
                rSampleType.sSamplingLevelName = GetSamplingLevelName(dsTemp.Tables[0].Rows[0]["SAMPLING_LEVEL"].ToString());
                G_tRCRecord.iCriticalRej = rSampleType.iCriticalRej;
                G_tRCRecord.iMajorRej = rSampleType.iMajorRej;
                G_tRCRecord.iMinorRej = rSampleType.iMinorRej;
            }
            return rSampleType;
        }
        /// <summary>
        /// 获取抽验计划等级名称
        /// </summary>
        /// <param name="sSamplingTypeID">抽验等级</param>
        /// <returns>等级名称</returns>
        ///  Flexible language settings 1.0.0.2 Lance
        public string GetSamplingLevelName(string sSamplingTypeID)
        {
            string sSamplingTypeLevelName = string.Empty;
            if (sSamplingTypeID == "0")
            {
                //sSamplingTypeLevelName = "正常";
                sSamplingTypeLevelName = "Normal";
            }
            else if (sSamplingTypeID == "1")
            {
                //sSamplingTypeLevelName = "加严";
                sSamplingTypeLevelName = "Tight";
            }
            else if (sSamplingTypeID == "2")
            {
                //sSamplingTypeLevelName = "减量";
                sSamplingTypeLevelName = "Reduced";
            }
            else if (sSamplingTypeID == "3")
            {
                //sSamplingTypeLevelName = "免检";
                sSamplingTypeLevelName = "No Inspect";
            }
            else
            {
                //sSamplingTypeLevelName = "正常";
                sSamplingTypeLevelName = "Normal";
            }
            return SajetCommon.SetLanguage(sSamplingTypeLevelName);
        }
        // 調整測試計畫更新測試大項資料
        private void UpdateTestTypeSamplePlan(string sTypeID, TSampleType rSampleType)
        {
            try
            {
                object[][] Params = new object[9][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TLOTNO", G_tQCLot.sLotNo };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TITEMTYPEID", sTypeID };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSAMPLELEVEL", rSampleType.sSamplingLevelID };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSAMPLEID", rSampleType.sSamplingID };
                Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSAMPLESIZE", rSampleType.iSampleQty.ToString() };
                Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TEMPID", g_sUserID };
                Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TPROCESSID", G_tRCRecord.sWIPProcessID };
                Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TPARTID", G_tRCRecord.sPartID };
                Params[8] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_RC_QC_UPDATE_SAMPLETYPE", Params);
                if (ds.Tables[0].Rows[0][0].ToString() != "OK")
                {
                    SajetCommon.Show_Message(ds.Tables[0].Rows[0][0].ToString(), 0);
                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);
            }
        }
        //测试大项完成写入数据库，更新测试大项的结果
        public void FinishItemType()
        {
            try
            {
                // Defect code條件數量大於不良數測試大項結果OK
                if (G_tRCRecord.iCriticalRej > g_iDefectNum || G_tRCRecord.iMajorRej > g_iDefectNum || G_tRCRecord.iMinorRej > g_iDefectNum)
                {
                    G_tRCRecord.sTestTypeResult = "0";
                }
                else
                {
                    G_tRCRecord.sTestTypeResult = "1";
                }
                // 任一測試小項result為NG時測試大項結果就為NG
                //if (g_iDefectNum > 0)
                //    sItemTypeResult = "1";
                //else
                //    sItemTypeResult = "0";

                string sSamplingID = dgvItemType.CurrentRow.Cells["SAMPLE_ID"].Value.ToString();
                string sSamplingSIZE = dgvItemType.CurrentRow.Cells["SAMPLE_SIZE"].Value.ToString();
                string sSamplingLEVEL = dgvItemType.CurrentRow.Cells["SAMPLE_LEVEL"].Value.ToString();
                string sItemTypeID = dgvItemType.CurrentRow.Cells["ITEM_TYPE_ID"].Value.ToString();
                int iSamplingID = Convert.ToInt32(dgvItemType.CurrentRow.Cells["SAMPLE_LEVEL_ID"].Value.ToString());

                int iFailQty = g_iDefectNum;

                int iPassQty = dgvItem.Rows.Count - 4 - iFailQty;
                if (G_tRCRecord.sProcessType.ToUpper() == "IPQC" && bHaveSPC)
                {
                    iPassQty = iPassQty - 5;
                }

                // 更新顯示介面
                dgvItemType.CurrentRow.Cells["PASS_QTY"].Value = iPassQty.ToString();
                dgvItemType.CurrentRow.Cells["FAIL_QTY"].Value = iFailQty.ToString();
                dgvItemType.CurrentRow.Cells["QC_RESULT"].Value = (G_tRCRecord.sTestTypeResult == "0") ? "OK" : "NG";
                dgvItem.ReadOnly = true;
                btnSave.Enabled = false;
                btnFinishItemType.Enabled = false;
                btnAgain.Enabled = false;

                if (G_tRCRecord.sProcessType.ToUpper() == "IPQC")
                {
                    if (G_tRCRecord.sTestTypeResult == "0" && G_tRCRecord.sCPKResult == "0") // OK
                    {
                        editPassQty.Text = txtInputQty.Text; // 整批良品過站
                        editFailQty.Text = "0";
                        editResult.Text = "OK";
                    }
                    else
                    {
                        editPassQty.Text = "0";
                        editFailQty.Text = txtInputQty.Text; // 整批不良品重測RC狀態不改
                        editResult.Text = "NG";
                        G_tRCRecord.sTestTypeResult = "1";
                    }
                }
                else // QC
                {
                    if (G_tRCRecord.sTestTypeResult == "0") // OK
                    {
                        editPassQty.Text = (Convert.ToDouble(txtInputQty.Text) - iFailQty).ToString();  // 維持檢測數據
                        editFailQty.Text = iFailQty.ToString();
                    }
                    else
                    {
                        editPassQty.Text = "0";
                        editFailQty.Text = txtInputQty.Text; // 整批不良品重測RC要Hold
                    }
                    editResult.Text = G_tRCRecord.sTestTypeResult == "0" ? "OK" : "NG";
                }
                editPassQty.Enabled = false;
                editFailQty.Enabled = false;
                //editResult.Text = G_tRCRecord.sTestTypeResult == "0" ? "OK" : "NG";


                object[][] Params = new object[13][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TLOTNO", G_tQCLot.sLotNo };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TRC_NO", G_tRCRecord.sRC };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TPROCESSID", G_tRCRecord.sWIPProcessID };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TLOTQTY", G_tRCRecord.iInputQty };
                Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TITEMTYPEID", sItemTypeID };
                Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSAMPLEID", sSamplingID };
                Params[6] = new object[] { ParameterDirection.Input, OracleType.Number, "TSAMPLESIZE", sSamplingSIZE };
                Params[7] = new object[] { ParameterDirection.Input, OracleType.Number, "TSAMPLELEVEL", iSamplingID };
                Params[8] = new object[] { ParameterDirection.Input, OracleType.Double, "TPASSQTY", iPassQty.ToString() };
                Params[9] = new object[] { ParameterDirection.Input, OracleType.Double, "TFAILQTY", iFailQty.ToString() };
                Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TQCRESULT", G_tRCRecord.sTestTypeResult };
                Params[11] = new object[] { ParameterDirection.Input, OracleType.Number, "TEMPID", g_sUserID };
                Params[12] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                dsTemp = ClientUtils.ExecuteProc("SAJET.SJ_RC_QC_TRANSFER_ITEMTYPE", Params);
                string sResult = dsTemp.Tables[0].Rows[0]["TRES"].ToString();
                if (sResult.ToUpper() != "OK")
                {
                    SajetCommon.Show_Message(sResult, 0);
                    return;
                }
                //g_iDefectNum = 0;
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);
            }
        }
        // 改變測試計畫觸發事件
        private void changeSampleTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvItemType.Rows.Count == 0 || dgvItemType.CurrentCell == null)
                return;

            //如果大项已经有结果则不能修改
            if (dgvItemType.CurrentRow.Cells["QC_RESULT"].Value.ToString() != "N/A")
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Item Type Finished"), 1);
                return;
            }

            string sCurrentItemTypeID = dgvItemType.CurrentRow.Cells["ITEM_TYPE_ID"].Value.ToString();
            string sSampleType = dgvItemType.CurrentRow.Cells["SAMPLE_TYPE"].Value.ToString();

            fAQL formAQL = new fAQL();
            formAQL.g_sItemTypeID = sCurrentItemTypeID;

            //formAQL.g_iLotSize = G_tQCLot.iLotSize;
            formAQL.g_iLotSize = Convert.ToInt32(Math.Round(G_tRCRecord.iInputQty, MidpointRounding.AwayFromZero));
            formAQL.g_sItemTypeName = dgvItemType.CurrentRow.Cells["TYPE_NAME"].Value.ToString();
            formAQL.initial(G_tQCLot.sLotNo, G_tQCLot.lstSampleLevel, sSampleType);

            if (formAQL.ShowDialog() != DialogResult.OK)
                return;
            //g_sSampleLevelID = formAQL.g_sSampleLeveID;
            //获取改变后的抽验计划范围
            TSampleType rSampleType = getSamplingPlanRange(formAQL.g_sSampleID, formAQL.g_sSampleLeveID, G_tRCRecord.iInputQty);
            g_bChangeALL = false;
            g_bChangeAQL = true;
            for (int i = 0; i < dgvItemType.Rows.Count; i++)
            {
                string sTypeID = dgvItemType.Rows[i].Cells["ITEM_TYPE_ID"].Value.ToString();
                string sQCResult = dgvItemType.Rows[i].Cells["QC_RESULT"].Value.ToString();
                //如果测试大项的抽验计划与要改变的抽验计划一样，并且该大项没有测试完成，并且不是被改变的测试大项，询问是否也改变其它测试大项的抽验计划
                if (dgvItemType.Rows[i].Cells["SAMPLE_TYPE"].Value.ToString() == sSampleType && sQCResult == "N/A" && sCurrentItemTypeID != sTypeID)
                {
                    if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Change ALL Test Type?"), 2) == DialogResult.Yes)
                    {
                        g_bChangeALL = true;
                    }
                    break;
                }
            }
            //如果所有测试项目的抽验计划都改变,则循环改变所有
            if (g_bChangeALL)
            {
                for (int i = 0; i < dgvItemType.Rows.Count; i++)
                {
                    string sTypeID = dgvItemType.Rows[i].Cells["ITEM_TYPE_ID"].Value.ToString();
                    string sQCResult = dgvItemType.Rows[i].Cells["QC_RESULT"].Value.ToString();
                    string sTypeName = dgvItemType.Rows[i].Cells["TYPE_NAME"].Value.ToString();
                    string sTypeCode = dgvItemType.Rows[i].Cells["TYPE_CODE"].Value.ToString();
                    if (dgvItemType.Rows[i].Cells["SAMPLE_TYPE"].Value.ToString() == sSampleType && sQCResult == "N/A")
                    {
                        UpdateTestTypeSamplePlan(sTypeID, rSampleType);
                    }
                }
            }
            //只改变当前
            else
            {
                UpdateTestTypeSamplePlan(sCurrentItemTypeID, rSampleType);
                dgvItemType.CurrentRow.Cells["SAMPLE_TYPE"].Value = rSampleType.sSamplingType;
                dgvItemType.CurrentRow.Cells["SAMPLE_LEVEL"].Value = rSampleType.sSamplingLevelName;
                dgvItemType.CurrentRow.Cells["SAMPLE_SIZE"].Value = rSampleType.iSampleQty;
            }

            //GetSYSTestType();
            SetItem(dgvItemType.Rows[0].Cells["ITEM_TYPE_ID"].Value.ToString());
            SetItemValue(G_tRCRecord.sWIPProcessID, dgvItemType.Rows[0].Cells["ITEM_TYPE_ID"].Value.ToString());
            //判断当前大项是否已完成测试，已完成的不能修改测试值
            SetTestItemVisible(dgvItemType.Rows[0].Cells["QC_RESULT"].Value.ToString());
        }
        /// <summary>
        /// 检查所有测试大项是否都完成
        /// </summary>
        /// <returns></returns>
        private bool CheckIsFinished()
        {
            bool bFinish = true;
            for (int i = 0; i < dgvItemType.Rows.Count; i++)
            {
                if (dgvItemType.Rows[i].Cells["QC_RESULT"].Value.ToString() == "N/A")
                {
                    bFinish = false;
                    break;
                }
            }
            return bFinish;
        }

        /// <summary>
        /// 检查测试大项是否完成
        /// </summary>
        /// <param name="sItemTypeID">测试大项ID</param>
        /// <returns></returns>
        private bool CheckIsFinished(string sItemTypeID)
        {
            bool bFinish = true;
            for (int i = 0; i < dgvItemType.Rows.Count; i++)
            {
                if (dgvItemType.Rows[i].Cells["ITEM_TYPE_ID"].Value.ToString() == sItemTypeID)
                {
                    if (dgvItemType.Rows[i].Cells["QC_RESULT"].Value.ToString() == "N/A")
                    {
                        bFinish = false;
                        break;
                    }
                }
            }
            return bFinish;
        }
        // 判定RC檢測最終結果
        private void SetResult()
        {
            bool bNG = false;
            bool bFinish = true;
            for (int i = 0; i < dgvItemType.Rows.Count; i++)
            {
                if (dgvItemType.Rows[i].Cells["QC_RESULT"].Value.ToString() == "N/A")
                {
                    bFinish = false;
                    continue;
                }
                if (dgvItemType.Rows[i].Cells["QC_RESULT"].Value.ToString() == "NG")
                {
                    bNG = true;
                }
            }
            if (bFinish)
            {
                editPassQty.Text = "";
                editFailQty.Text = "";
                editResult.Text = bNG ? "NG" : "OK";
                if (G_tRCRecord.bHaveSN)
                {
                    //                    string sSQL = @"SELECT INSP_RESULT, SUM(QTY) qty
                    //                                              FROM (select insp_result, count(1) QTY
                    //                                                      from (select serial_number, min(insp_result) insp_result
                    //                                                              from sajet.G_QC_SN_TESTITEM_TEMP
                    //                                                             where QC_LOTNO = '" + G_tQCLot.sLotNo + @"'
                    //                                                             group by serial_number) A
                    //                                                     group by insp_result
                    //                                                    UNION ALL
                    //                                                    SELECT 'OK', 0
                    //                                                      FROM DUAL
                    //                                                    UNION ALL
                    //                                                    SELECT 'NG', 0 FROM DUAL)
                    //                                             GROUP BY INSP_RESULT";
                    string sSQL = @"SELECT INSP_RESULT, COUNT(SERIAL_NUMBER) QTY
                                             FROM (SELECT B.SERIAL_NUMBER,
                                                DECODE(A.SERIAL_NUMBER, NULL, 'OK', INSP_RESULT) INSP_RESULT
                                       FROM (SELECT SERIAL_NUMBER, MIN(INSP_RESULT) INSP_RESULT
                                                   FROM SAJET.G_QC_SN_TESTITEM_TEMP
                                                 WHERE QC_LOTNO = :QC_LOTNO
                                                 GROUP BY SERIAL_NUMBER) A,
                                               SAJET.G_SN_STATUS B
                                     WHERE B.SERIAL_NUMBER = A.SERIAL_NUMBER(+)
                                         AND B.RC_NO = :RC_NO
                                         AND B.CURRENT_STATUS IN ('0','1')) GROUP BY INSP_RESULT";
                    object[][] Params1 = new object[2][];
                    Params1[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "QC_LOTNO", G_tQCLot.sLotNo };
                    Params1[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", editRC.Text };
                    DataTable dt = ClientUtils.ExecuteSQL(sSQL, Params1).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i]["insp_result"].ToString() == "OK")
                            {
                                editPassQty.Text = dt.Rows[i]["qty"].ToString();
                            }
                            if (dt.Rows[i]["insp_result"].ToString() == "NG")
                            {
                                editFailQty.Text = dt.Rows[i]["qty"].ToString();
                            }
                        }
                        if (editPassQty.Text == "")
                            editPassQty.Text = "0";
                        if (editFailQty.Text == "")
                            editFailQty.Text = "0";
                    }
                    //editResult.Text = editPassQty.Text == "0" ? "NG" : "OK";
                }
            }
        }
        // 替換測試大項所觸發的事件，詢問是否將舊的測試大項資料暫存，再重新載入新的測試大項暫存資料
        private void dgvItemType_SelectionChanged(object sender, EventArgs e)
        {
            //排除界面初始化
            if (g_iFlag != 0)
            {
                string sItemTypeID_Now = dgvItemType.CurrentRow.Cells["ITEM_TYPE_ID"].Value.ToString();

                //提示未保存的SN的检验结果进行保存
                if (dgvItem.Rows.Count > 0)
                {
                    //string sItemTypeID_Old = dgvItem.Rows[0].Cells["ITEM_TYPE"].Value.ToString();
                    string sItemTypeID_Old = dgvItem.Rows[4].Cells["ITEM_TYPE"].Value.ToString();
                    if (sItemTypeID_Old != sItemTypeID_Now)
                    {
                        //判断是否原本大项已经检验完成，已完成的不需要保存，显示切换至的大项信息
                        int iIDX = 0;
                        for (int i = 0; i < dgvItemType.Rows.Count; i++)
                        {
                            if (dgvItemType.Rows[i].Cells["ITEM_TYPE_ID"].Value.ToString() == sItemTypeID_Old)
                            {
                                iIDX = i;
                                break;
                            }
                        }

                        //如果原本测试大项没有完成，则看是否需要保存
                        if (dgvItemType.Rows[iIDX].Cells["QC_RESULT"].Value.ToString() == "N/A")
                        {
                            //是否有已经写入值的项目需要保存
                            bool bNeedSave = false;

                            // 檢測時把mean,ca,cp,cpk移除
                            int iRow = 0;
                            if (G_tRCRecord.sProcessType.ToUpper() == "IPQC" && bHaveSPC)
                                iRow = 5;

                            for (int i = 4; i < dgvItem.Rows.Count - iRow; i++)
                            {   //SN, Item...,Result,ItemTypeID
                                for (int j = 1; j < dgvItem.Columns.Count - 2; j++)
                                {
                                    if (dgvItem.Rows[i].Cells[j].Value == null)
                                        continue;
                                    if (string.IsNullOrEmpty(dgvItem.Rows[i].Cells[j].Value.ToString()))
                                        continue;
                                    bNeedSave = true;
                                    break;
                                }
                                if (bNeedSave)
                                    break;
                            }
                            if (bNeedSave)
                            {
                                if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Need Save"), 2) == DialogResult.Yes)
                                {
                                    btnSave_Click(sender, e);
                                    return;
                                }
                                else
                                {
                                    SetItem(sItemTypeID_Now);
                                    SetItemValue(G_tRCRecord.sWIPProcessID, sItemTypeID_Now);

                                    //判断当前大项是否已完成测试，已完成的不能修改测试值
                                    SetTestItemVisible(dgvItemType.CurrentRow.Cells["QC_RESULT"].Value.ToString());

                                    showProcessDefect(sItemTypeID_Now);
                                    SetDefect(G_tQCLot.sLotNo, sItemTypeID_Now);
                                    SetDefectVisible(dgvItemType.CurrentRow.Cells["QC_RESULT"].Value.ToString());
                                }
                            }
                            else
                            {
                                SetItem(sItemTypeID_Now);
                                SetItemValue(G_tRCRecord.sWIPProcessID, sItemTypeID_Now);

                                //判断当前大项是否已完成测试，已完成的不能修改测试值
                                SetTestItemVisible(dgvItemType.CurrentRow.Cells["QC_RESULT"].Value.ToString());

                                showProcessDefect(sItemTypeID_Now);
                                SetDefect(G_tQCLot.sLotNo, sItemTypeID_Now);
                                SetDefectVisible(dgvItemType.CurrentRow.Cells["QC_RESULT"].Value.ToString());
                            }
                        }
                        else
                        {
                            SetItem(sItemTypeID_Now);
                            SetItemValue(G_tRCRecord.sWIPProcessID, sItemTypeID_Now);

                            //判断当前大项是否已完成测试，已完成的不能修改测试值
                            SetTestItemVisible(dgvItemType.CurrentRow.Cells["QC_RESULT"].Value.ToString());

                            showProcessDefect(sItemTypeID_Now);
                            SetDefect(G_tQCLot.sLotNo, sItemTypeID_Now);
                            SetDefectVisible(dgvItemType.CurrentRow.Cells["QC_RESULT"].Value.ToString());
                        }
                    }
                }
                else
                {
                    SetItem(sItemTypeID_Now);
                    SetItemValue(G_tRCRecord.sWIPProcessID, sItemTypeID_Now);

                    //判断当前大项是否已完成测试，已完成的不能修改测试值
                    SetTestItemVisible(dgvItemType.CurrentRow.Cells["QC_RESULT"].Value.ToString());

                    showProcessDefect(sItemTypeID_Now);
                    SetDefect(G_tQCLot.sLotNo, sItemTypeID_Now);
                    SetDefectVisible(dgvItemType.CurrentRow.Cells["QC_RESULT"].Value.ToString());
                }
            }
        }
    
        // 按下測試大項完成按鈕，暫存測試小項、不良資料與判定測試大項結果
        private void btnFinishItemType_Click(object sender, EventArgs e)
        {
            if (dgvItemType.CurrentRow.Cells["QC_RESULT"].Value.ToString() != "N/A")
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Test Item Type Finished"), 0);
                return;
            }

            //检查测试小项是否满足规定
            if (!CheckItem(G_tRCRecord.bHaveSN))
                return;

            if (G_tRCRecord.sProcessType == "IPQC")
            {
                Get_Limit(null);

                // 檢查cpk結果
                G_tRCRecord.sCPKResult = "0";
                for (int i = 1; i < dgvItem.Columns.Count - 1; i += 2)
                {
                    if (dgvItem.Rows[0].Cells[i].Value.ToString() != "0") // 當測項與SPC關聯
                    {
                        if (dgvItem.Rows[dgvItem.Rows.Count - 2].Cells[i + 1].Value == null)
                        {
                            SajetCommon.Show_Message(SajetCommon.SetLanguage("Input CPK Result."), 0);
                            return;
                        }
                        //else
                        //{
                        //    if (dgvItem.Rows[dgvItem.Rows.Count - 2].Cells[i + 1].Value.ToString() == "NG")
                        //        G_tRCRecord.sCPKResult = "1";  // CPK結果設定為NG作為重測依據
                        //}
                    }
                }
            }

            if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Finish Item Type Confirm") + " : " + "Y/N?", 2) == DialogResult.No)
                return;

            //保存测试小项值进入临时表
            SaveInspValue();
            //记录测试小项的结果
            RecordItem();
            // 將有在產品測試設定SPC測項寫入SPC資料表內
            insertSPC();
            // 儲存SPC Moniter Table
            Interface.MoveSPC(editRC.Text, Convert.ToInt32(dgvItemType.CurrentRow.Cells["SAMPLE_SIZE"].Value));

            if (G_tRCRecord.sProcessType == "IPQC")
            {
                // 儲存CaCpCpk資料
                if (!SaveCaCpCpk()) return;
            }

            //完成测试大项
            FinishItemType();
            // 儲存目前使用的抽驗計畫及等級紀錄
            saveSampleRule();


            //记录不良信息进入临时表
            //RecordDefect();
            //SetResult();
        }

        private void saveSampleRule()
        {
            try
            {
                object[][] Params = new object[7][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TQCNO", G_tQCLot.sLotNo };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TPROCESSID", G_tRCRecord.sWIPProcessID };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TEMPID", G_tRCRecord.sEmpID };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TITEMTYPEID", dgvItemType.CurrentRow.Cells["ITEM_TYPE_ID"].Value.ToString() };
                Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSAMPLINGPlANID", dgvItemType.CurrentRow.Cells["SAMPLE_ID"].Value.ToString() };
                Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSAMPLINGLEVEL", dgvItemType.CurrentRow.Cells["SAMPLE_LEVEL_ID"].Value.ToString() };
                Params[6] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_RC_QC_SAMPLING_PLAN", Params);

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
        
        // follw AQL 調整Sample Level
        public bool GetSamplingPlan_From_Rule(string sItemTypeId, ref string sSampleType, ref string sSampleLevel, ref string sSampleId)
        {
            try
            {
                object[][] Params = new object[8][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TLOTNO", G_tQCLot.sLotNo };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TPARTNO", G_tRCRecord.sPartNo };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TPROCESSID", G_tRCRecord.sWIPProcessID };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TITEMTYPEID", sItemTypeId };
                Params[4] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TSAMPLINGPLAN", "" };
                Params[5] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TSAMPLINGLEVEL", "" };
                Params[6] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TSAMPLINGID", "" };
                Params[7] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };

                DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_RC_QC_GET_SAMPLE_RULE", Params);

                string sRes = ds.Tables[0].Rows[0]["TRES"].ToString();
                if (sRes != "OK")
                {
                    SajetCommon.Show_Message(sRes, 0);
                    return false;
                }
                else
                {
                    sSampleType = ds.Tables[0].Rows[0]["TSAMPLINGPLAN"].ToString();
                    sSampleLevel = ds.Tables[0].Rows[0]["TSAMPLINGLEVEL"].ToString();
                    sSampleId = ds.Tables[0].Rows[0]["TSAMPLINGID"].ToString();
                    return true;
                }
                
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);
                return false;
            }
            
        }

        # endregion

        # region 測試小項程式區域
        // 隱藏部分測試小項欄位資料
        private void SetTestItemVisible(string sItemTypeResult)
        {
            if (sItemTypeResult != "N/A")
            {
                dgvItem.ReadOnly = true;
                btnSave.Enabled = false;
                btnFinishItemType.Enabled = false;
                btnAgain.Enabled = false;
            }
            else
            {
                //dgvItem.ReadOnly = false;
                dgvItem.Columns["SN"].ReadOnly = true;
                btnSave.Enabled = true;
                btnFinishItemType.Enabled = true;
                btnAgain.Enabled = true;
            }
        }
        /// <summary>
        /// 显示测试大项对应的测试小项栏位
        /// </summary>
        /// <param name="sItemTypeID">测试大项ID</param>
        public void SetItem(string sItemTypeID)
        {
            dgvItem.ReadOnly = false;
            g_alItemRule = new ArrayList();
            //g_alItemRuleDesc = new ArrayList();
            dgvItem.Columns.Clear();
            dgvItem.Rows.Clear();
            //添加SN栏位
            DataGridViewTextBoxColumn SNText = new DataGridViewTextBoxColumn();
            SNText.Name = "SN";
            SNText.HeaderText = SajetCommon.SetLanguage("SN");
            SNText.ReadOnly = true;
            dgvItem.Columns.Add(SNText);

            sSQL = @" SELECT * "
                + "FROM SAJET.SYS_PART_QC_TESTITEM A "
                + "LEFT JOIN SAJET.SYS_TEST_ITEM C "
                + "ON A.ITEM_ID = C.ITEM_ID "
                + " LEFT JOIN SAJET.SYS_TEST_ITEM_TYPE D "
                + "  ON C.ITEM_TYPE_ID = D.ITEM_TYPE_ID "
                + " WHERE A.RECID = :RECID "
                + " AND C.ITEM_TYPE_ID = :ITEM_TYPE_ID "
                + " AND A.ENABLED = 'Y' "
                + " AND C.ENABLED = 'Y' "
                + "AND D.ENABLED = 'Y' "
                + "ORDER BY ITEM_TYPE_CODE, ITEM_CODE ";
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RECID", G_tQCLot.sRECID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", sItemTypeID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            //添加测试小项栏位
            int i = 0;
            foreach (DataRow dr in dsTemp.Tables[0].Rows)
            {
                DataGridViewTextBoxColumn dText = new DataGridViewTextBoxColumn();
                dText.Name = dr["ITEM_ID"].ToString();
                dText.HeaderText = SajetCommon.SetLanguage(dr["ITEM_NAME"].ToString(), 1);
                dText.Width = 200;
                dgvItem.Columns.Add(dText);
                
                g_alItemRule.Add(dr["UPPER_LIMIT"].ToString() + "," + dr["LOWER_LIMIT"].ToString());

                //每一個添加检验结果栏位
                DataGridViewComboBoxColumn cbbCol = new DataGridViewComboBoxColumn();
                //string[] s = { "OK", "NG", "N/A" };
                string[] s = { "OK", "NG" };
                cbbCol.Items.AddRange(s);
                cbbCol.Name = "RESULT";
                cbbCol.HeaderText = SajetCommon.SetLanguage("RESULT");
                dgvItem.Columns.Add(cbbCol);

                // SPC測值由系統判定
                if (dgvItem.Rows.Count == 0)
                {
                    dgvItem.Rows.Add(); // SPC flag
                    dgvItem.Rows[dgvItem.Rows.Count - 1].ReadOnly = true;
                    dgvItem.Rows[dgvItem.Rows.Count - 1].Visible = false;
                    dgvItem.Rows.Add(); // UCL
                    dgvItem.Rows[dgvItem.Rows.Count - 1].ReadOnly = true;
                    dgvItem.Rows[dgvItem.Rows.Count - 1].Visible = false;
                    dgvItem.Rows.Add();  // CL
                    dgvItem.Rows[dgvItem.Rows.Count - 1].ReadOnly = true;
                    dgvItem.Rows[dgvItem.Rows.Count - 1].Visible = false;
                    dgvItem.Rows.Add();  // LCL
                    dgvItem.Rows[dgvItem.Rows.Count - 1].ReadOnly = true;
                    dgvItem.Rows[dgvItem.Rows.Count - 1].Visible = false;
                }

                if (dr["SPC_ID"].ToString() != "0")
                {
                    dgvItem.Columns[dgvItem.Columns.Count - 1].ReadOnly = true;
                    bHaveSPC = true;

                    // 紀錄平均線、控制上下限
                    //arrData = new decimal[] { Convert.ToDecimal(dr["UPPER_CONTROL_LIMIT"].ToString()), 
                    //                                       Convert.ToDecimal(dr["MIDDLE_LIMIT"].ToString()), 
                    //                                       Convert.ToDecimal(dr["LOWER_CONTROL_LIMIT"].ToString())  };
                    //g_ControlLimit.Add(arrData);
                    dgvItem.Rows[0].Cells[i * 2 + 1].Value = dr["SPC_ID"].ToString();  // 此測項有與SPC關聯寫入SPC_ID
                    dgvItem.Rows[1].Cells[i * 2 + 1].Value = dr["UPPER_CONTROL_LIMIT"].ToString();
                    tspc.g_sUCL1 = dr["UPPER_CONTROL_LIMIT"].ToString();
                    dgvItem.Rows[2].Cells[i * 2 + 1].Value = dr["MIDDLE_LIMIT"].ToString();
                    tspc.g_sCL1 = dr["MIDDLE_LIMIT"].ToString();
                    dgvItem.Rows[3].Cells[i * 2 + 1].Value = dr["LOWER_CONTROL_LIMIT"].ToString();
                    tspc.g_sLCL1 = dr["LOWER_CONTROL_LIMIT"].ToString();
                }
                else
                {
                    dgvItem.Rows[0].Cells[i * 2 + 1].Value = "0";  // 此測項沒有與SPC關聯寫入0
                    if (dr["HAS_VALUE"].ToString() == "N")
                        dgvItem.Columns[i * 2 + 1].ReadOnly = true;
                }
                i++;
            }


            //添加测试大项ID栏位，用于判断大项改变时如何响应，栏位隐藏
            DataGridViewTextBoxColumn txtCol = new DataGridViewTextBoxColumn();
            txtCol.Name = "ITEM_TYPE";
            txtCol.HeaderText = SajetCommon.SetLanguage("ITEM_TYPE");
            txtCol.Visible = false;
            dgvItem.Columns.Add(txtCol);
        }
        // 顯示與檢驗批量相同測試小項樣本數
        private void showItem()
        {
            try
            {
                //dgvItem.Rows.Clear();

                //获取当前选取的测试大项
                string sTestItemTypeID = dgvItemType.CurrentRow.Cells["ITEM_TYPE_ID"].Value.ToString();
                int iSampleSize = Convert.ToInt32(dgvItemType.CurrentRow.Cells["SAMPLE_SIZE"].Value.ToString());

                for (int i = 0; i < iSampleSize; i++)
                {
                    dgvItem.Rows.Add();

                    // Set RC subitem
                    dgvItem.Rows[i+4].Cells[0].Value = G_tRCRecord.sRC + (i + 1).ToString("00000");
                    dgvItem.Rows[i+4].Cells["ITEM_TYPE"].Value = sTestItemTypeID;
                }

                // 判定製程別為IPQC時在測試小項抽樣數增加平均數與CPK欄位
                if (G_tRCRecord.sProcessType.ToUpper() == "IPQC" && bHaveSPC)
                {
                    dgvItem.Rows.Add();
                    dgvItem.Rows[dgvItem.Rows.Count - 1].ReadOnly = true;
                    dgvItem.Rows[dgvItem.Rows.Count - 1].Cells[0].Value = "Average";
                    dgvItem.Rows.Add();
                    dgvItem.Rows[dgvItem.Rows.Count - 1].ReadOnly = true;
                    dgvItem.Rows[dgvItem.Rows.Count - 1].Cells[0].Value = "Ca";
                    dgvItem.Rows.Add();
                    dgvItem.Rows[dgvItem.Rows.Count - 1].ReadOnly = true;
                    dgvItem.Rows[dgvItem.Rows.Count - 1].Cells[0].Value = "Cp";
                    dgvItem.Rows.Add();
                    dgvItem.Rows[dgvItem.Rows.Count - 1].ReadOnly = true;
                    dgvItem.Rows[dgvItem.Rows.Count - 1].Cells[0].Value = "CPK";
                    dgvItem.Rows.Add();
                    dgvItem.Rows[dgvItem.Rows.Count - 1].ReadOnly = true;
                    dgvItem.Rows[dgvItem.Rows.Count - 1].Cells[0].Value = "Basis"; 
                }

                for (int i = 1; i < dgvItem.Columns.Count - 1; i+=2)
                {
                    if (dgvItem.Rows[0].Cells[i].Value.ToString() != "0")
                    {
                        // CPK結果可以人為設定

                        dgvItem.Rows[dgvItem.Rows.Count - 2].Cells[i + 1].ReadOnly = false;
                        dgvItem.Rows[dgvItem.Rows.Count - 2].Cells[i + 1].ReadOnly = false;
                        // 結果依據可以人為設定
                        //DataGridViewCheckBoxCell dgcb = new DataGridViewCheckBoxCell();
                        //dgcb.Value = true;
                        //dgvItem.Rows[dgvItem.Rows.Count - 1].Cells[i] = dgcb;
                        dgvItem.Rows[dgvItem.Rows.Count - 1].Cells[i + 1].ReadOnly = false;
                        dgvItem.Rows[dgvItem.Rows.Count - 1].Cells[i + 1].ReadOnly = false;
                    }
                    else
                    {
                        if (dgvItem.Columns[i].ReadOnly)
                        {
                            for(int j=0;j<iSampleSize;j++)
                            {
                                dgvItem.Rows[j+4].Cells[i].Value = "N/A";
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 获取保存的SN,测试小项值及结果
        /// </summary>
        /// <param name="sSN">SN</param>
        /// <param name="sProcessID">制程</param>
        /// <param name="sItemTypeID">测试大项</param>
        /// <returns>是否有保存</returns>
        private bool SetItemValue(string sProcessID, string sItemTypeID)
        {
            try
            {
                // Show gridview construct
                showItem();

                // 檢測時把mean,ca,cp與cpk移除
                int iRow = 0;
                if (G_tRCRecord.sProcessType.ToUpper() == "IPQC" && bHaveSPC)
                    iRow = 5;

                // Input database data
                string sSQL = " SELECT * FROM SAJET.G_QC_SN_TESTITEM_TEMP "
                                      + " WHERE ITEM_TYPE_ID = '" + sItemTypeID + "' "
                                      + " AND PROCESS_ID = '" + sProcessID + "' "
                                      + " AND QC_LOTNO = '" + G_tQCLot.sLotNo + "' "
                                      + " ORDER BY SERIAL_NUMBER, ITEM_ID ";
                DataTable dt = ClientUtils.ExecuteSQL(sSQL).Tables[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 4; j < dgvItem.Rows.Count - iRow; j++)
                    {
                        if (dgvItem.Rows[j].Cells[0].Value.ToString() == dt.Rows[i]["SERIAL_NUMBER"].ToString())
                        {
                            for (int k = 1; k < dgvItem.Columns.Count - 1; k += 2)
                            {
                                if (dgvItem.Columns[k].Name == dt.Rows[i]["ITEM_ID"].ToString())
                                {
                                    dgvItem.Rows[j].Cells[k].Value = dt.Rows[i]["INSP_VALUE"].ToString();
                                    dgvItem.Rows[j].Cells[k + 1].Value = dt.Rows[i]["INSP_RESULT"].ToString();
                                    //j = dgvItem.Columns.Count;
                                    break;
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        //判断测试小项是否完成
        public bool CheckItem(bool bHaveSN)
        {
            string sMsg = "";
            if (dgvItem.Rows.Count == 0)
            {
                if (dgvItemType.CurrentRow.Cells["SAMPLE_SIZE"].Value.ToString() != "0")
                {
                    sMsg = SajetCommon.SetLanguage("Item not complete");
                    SajetCommon.Show_Message(sMsg, 1);
                    return false;
                }
            }

            if (dgvItem.Rows.Count < Convert.ToInt32(dgvItemType.CurrentRow.Cells["SAMPLE_SIZE"].Value.ToString()))
            {
                sMsg = SajetCommon.SetLanguage("test qty not enough");
                SajetCommon.Show_Message(sMsg, 1);
                return false;
            }

            // 檢測時把mean,ca,cp與cpk移除
            int iRow = 0;
            if (G_tRCRecord.sProcessType.ToUpper() == "IPQC" && bHaveSPC)
                iRow = 5;

            for (int i = 4; i < dgvItem.Rows.Count - iRow; i++)
            {
                bool bNG = false;
                for (int j = 1; j < dgvItem.Columns.Count - 2; j++)
                {
                    //判断检验值是否为空
                    if (dgvItem.Rows[i].Cells[j].Value == null)
                    {
                        sMsg = SajetCommon.SetLanguage("test value empty");
                        SajetCommon.Show_Message(sMsg, 1);
                        return false;
                    }
                    if (string.IsNullOrEmpty(dgvItem.Rows[i].Cells[j].Value.ToString()))
                    {
                        sMsg = SajetCommon.SetLanguage("test value empty");
                        SajetCommon.Show_Message(sMsg, 1);
                        return false;
                    }

                    //如果结果人为判断为OK，而该项目存在error的测试小项，则结果需为NG
                    if (dgvItem.Columns[j].Name == "RESULT")
                    {
                        if (dgvItem.Rows[i].Cells[j].Value.ToString() != "OK" && dgvItem.Rows[i].Cells[j].Value.ToString() != "NG")
                        {
                            sMsg = SajetCommon.SetLanguage("test value empty");
                            SajetCommon.Show_Message(sMsg, 1);
                            return false;
                        }
                    }
                    else
                    {
                        string sRule = g_alItemRule[(j + 1) / 2 - 1].ToString();
                        if (!CheckValue(dgvItem.Rows[i].Cells[j].Value.ToString(), sRule, out sMsg))
                        {
                            bNG = true;
                            dgvItem.Rows[i].Cells[j].ErrorText = SajetCommon.SetLanguage(sMsg);
                            //return false;
                        }
                    }
                }
                //if (dgvItem.Rows[i].Cells["RESULT"].Value.ToString() == "OK")
                //{
                //    if (bNG)
                //    {
                //        dgvItem.Rows[i].Cells["RESULT"].ErrorText = SajetCommon.SetLanguage("Result NG");
                //        SajetCommon.Show_Message(SajetCommon.SetLanguage("Result NG") + "\n" +
                //            SajetCommon.SetLanguage("SN") + " : " + dgvItem.Rows[i].Cells["SN"].Value.ToString(), 1);
                //        return false;
                //    }
                //}
            }

            g_iDefectNum = 0;
            //如果有SN
            if (bHaveSN)
            {
                string sResult, sSN, sDefectSN, sItemTypeID;

                for (int i = 4; i < dgvItem.Rows.Count - iRow; i++)
                {
                    sResult = dgvItem.Rows[i].Cells["RESULT"].Value.ToString();
                    sSN = dgvItem.Rows[i].Cells["SN"].Value.ToString();
                    if (sResult == "NG")
                    {
                        if (dgvDefect.Rows.Count == 0)
                        {
                            sMsg = SajetCommon.SetLanguage("Have no defect") + " : " + sSN;
                            SajetCommon.Show_Message(sMsg, 1);
                            return false;
                        }

                        bool bExist = false;//记录小项为NG 的SN是否有新增不良，且不良代码是在当前测试大项下面增加的
                        for (int j = 0; j < dgvDefect.Rows.Count; j++)
                        {
                            sDefectSN = dgvDefect.Rows[j].Cells["SERIAL_NUMBER"].Value.ToString();
                            sItemTypeID = dgvDefect.Rows[j].Cells["ITEM_TYPE_ID_DEFECT"].Value.ToString();
                            if (sSN == sDefectSN && sItemTypeID == dgvItem.Rows[0].Cells["ITEM_TYPE"].Value.ToString())
                            {
                                bExist = true;
                                break;
                            }
                        }
                        if (!bExist)
                        {
                            sMsg = SajetCommon.SetLanguage("Have no defect") + " : " + sSN;
                            SajetCommon.Show_Message(sMsg, 1);
                            return false;
                        }

                        g_iDefectNum++;
                    }
                }
            }
            //如果没有SN
            else
            {
                // 品檢序號中其中一個測試小項為NG，代表此品檢序號測試結果就是NG；一個品檢序號只要有一個defect code做紀錄
                bool bNG = false;
                for (int i = 4; i < dgvItem.Rows.Count - iRow; i++)
                {
                    bool bSNNG = false;
                    for (int j = 1; j < dgvItem.Columns.Count - 1; j += 2)
                    {
                        if (dgvItem.Rows[i].Cells[j + 1].Value == null)
                        {
                            sMsg = SajetCommon.SetLanguage("test value empty");
                            SajetCommon.Show_Message(sMsg, 1);
                            return false;
                        }

                        if (dgvItem.Rows[i].Cells[j + 1].Value.ToString() == "NG")
                        {
                            //g_iDefectNum++;
                            //bNG = true;                       
                            //break;
                            bSNNG = true;
                        }
                    }
                    if (bSNNG)
                    {
                        g_iDefectNum++;
                        bNG = true;
                    }
                }
                if (bNG)
                {
                    //判断是否添加不良；
                    //if (dgvDefect.Rows.Count == 0)
                    //{
                    //    sMsg = G_tRCRecord.sRC + ":" + SajetCommon.SetLanguage("no append defect");
                    //    SajetCommon.Show_Message(sMsg, 1);
                    //    return false;
                    //}
                    double iQty = 0;
                    for (int i = 0; i < dgvDefect.Rows.Count; i++)
                    {
                        if (dgvDefect.Rows[i].Cells["DEFECT_QTY"].Value == null)
                            continue;
                        iQty = iQty + Convert.ToDouble(dgvDefect.Rows[i].Cells["DEFECT_QTY"].Value.ToString());
                    }
                    // 未輸入不良
                    if (iQty == 0)
                    {
                        sMsg = G_tRCRecord.sRC + ":" + SajetCommon.SetLanguage("no append defect");
                        SajetCommon.Show_Message(sMsg, 1);
                        return false;
                    }
                    // 新增不良數少於檢驗不良數
                    if (iQty < g_iDefectNum) 
                    {
                        sMsg = G_tRCRecord.sRC + ":" + SajetCommon.SetLanguage(" append defect quantity less than NG quantity.");
                        SajetCommon.Show_Message(sMsg, 1);
                        return false;
                    }
                    //判断测试大项是否添加不良
                    //bool bDefect = false;
                    //for (int j = 0; j < dgvDefect.Rows.Count; j++)
                    //{
                    //    if (dgvDefect.Rows[j].Cells["ITEM_TYPE_ID_DEFECT"].Value.ToString() == dgvItem.Rows[0].Cells["ITEM_TYPE"].Value.ToString())
                    //    {
                    //        bDefect = true;
                    //        break;
                    //    }
                    //}
                    //if (!bDefect)
                    //{
                    //    sMsg = G_tRCRecord.sRC + ":" + SajetCommon.SetLanguage("Type no append defect");
                    //    SajetCommon.Show_Message(sMsg, 1);
                    //    return false;
                    //}
                }
            }
            return true;
        }
        // 檢查測試小項輸入資料的正確性與上下限
        private bool CheckValue(string sValue, string sRule, out string sMsg)
        {
            sMsg = "OK";

            if (string.IsNullOrEmpty(sValue))
            {
                sMsg = SajetCommon.SetLanguage("Value Empty");
                return false;
            }


            string sUpperLimit = sRule.Split(',')[0];
            string sLowerLimit = sRule.Split(',')[1];
            decimal dUpperLimit = 0, dLowerLimit = 0, dValue;

            if (string.IsNullOrEmpty(sUpperLimit) && !string.IsNullOrEmpty(sLowerLimit))
            {
                dLowerLimit = decimal.Parse(sLowerLimit);
                if (!decimal.TryParse(sValue, out dValue))
                {
                    sMsg = SajetCommon.SetLanguage("Value Type Error") + "\n" + SajetCommon.SetLanguage("Lower Limit") + ":" + sLowerLimit;
                    return false;
                }
                else
                {
                    if (dValue < dLowerLimit)
                    {
                        sMsg = SajetCommon.SetLanguage("Value Error") + "\n" + SajetCommon.SetLanguage("Lower Limit") + ":" + sLowerLimit;
                        return false;
                    }
                }
            }
            else if (!string.IsNullOrEmpty(sUpperLimit) && string.IsNullOrEmpty(sLowerLimit))
            {
                dUpperLimit = decimal.Parse(sUpperLimit);
                if (!decimal.TryParse(sValue, out dValue))
                {
                    sMsg = SajetCommon.SetLanguage("Value Type Error") + "\n" + SajetCommon.SetLanguage("Upper Limit") + ":" + sUpperLimit;
                    return false;
                }
                else
                {
                    if (dValue > dUpperLimit)
                    {
                        sMsg = SajetCommon.SetLanguage("Value Error") + "\n" + SajetCommon.SetLanguage("Upper Limit") + ":" + sUpperLimit;
                        return false;
                    }
                }
            }
            else if (!string.IsNullOrEmpty(sUpperLimit) && !string.IsNullOrEmpty(sLowerLimit))
            {
                dLowerLimit = decimal.Parse(sLowerLimit);
                dUpperLimit = decimal.Parse(sUpperLimit);
                if (!decimal.TryParse(sValue, out dValue))
                {
                    sMsg = SajetCommon.SetLanguage("Value Type Error") + "\n" + SajetCommon.SetLanguage("Lower Limit") + ":" + sLowerLimit
                            + "\n" + SajetCommon.SetLanguage("Upper Limit") + ":" + sUpperLimit;
                    return false;
                }
                else
                {
                    if (dValue > dUpperLimit || dValue < dLowerLimit)
                    {
                        sMsg = SajetCommon.SetLanguage("Value Error") + "\n" + SajetCommon.SetLanguage("Lower Limit") + ":" + sLowerLimit
                            + "\n" + SajetCommon.SetLanguage("Upper Limit") + ":" + sUpperLimit;
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 检查是否为指定类型的字符
        /// </summary>
        /// <param name="sData">字符</param>
        /// <param name="sPattern">指定类型</param>
        /// <returns>是否匹配i</returns>
        private bool CheckDataValidation(string sData, string sPattern)
        {
            //string sPattern = "^[0-9]+$";
            return Regex.IsMatch(sData, sPattern);
        }
        //将测试小项寫入正式資料表
        public void RecordItem()
        {
            try
            {
                string sItemTypeID = dgvItemType.CurrentRow.Cells["ITEM_TYPE_ID"].Value.ToString();
                // 檢測時把average與cpk移除
                int iRow = 0;
                if (G_tRCRecord.sProcessType.ToUpper() == "IPQC" && bHaveSPC)
                    iRow = 5;

                for (int m = 4; m < dgvItem.Rows.Count - iRow; m++)
                {
                    string sSN = dgvItem.Rows[m].Cells["SN"].Value.ToString();
                    //string sResult = dgvItem.Rows[m].Cells["RESULT"].Value.ToString();
                    for (int n = 1; n < dgvItem.Columns.Count - 2; n+=2)
                    {
                        string sItemID = dgvItem.Columns[n].Name;
                        string sValue = (dgvItem.Rows[m].Cells[n].Value == null) ? (string)Convert.DBNull : dgvItem.Rows[m].Cells[n].Value.ToString();
                        string sResult = dgvItem.Rows[m].Cells[n+1].Value.ToString();
                        object[][] Params = new object[7][];
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "I_ITEM_TYPE_ID", sItemTypeID };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "I_ITEM_ID", sItemID };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "I_SN", sSN };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "I_INFO", sValue };
                        Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "I_RESULT", sResult };
                        Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "I_QCLOT", txtLotNo.Text };
                        Params[6] = new object[] { ParameterDirection.Output, OracleType.VarChar, "O_RES", "" };
                        dsTemp = ClientUtils.ExecuteProc("SAJET.SJ_RC_QC_RECORD_ITEM", Params);
                        string sRes = dsTemp.Tables[0].Rows[0]["O_RES"].ToString();
                        if (sRes.ToUpper() != "OK")
                        {
                            SajetCommon.Show_Message(sResult, 0);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);
            }
        }
        // 重新抽检並且將此測試大項所有的暫存資料清除
        private void btnAgain_Click(object sender, EventArgs e)
        {
            //if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Re-Inspect Confirm") + " : " + "Y/N?", 2) == DialogResult.No)
            //    return;
            try
            {
                object[][] Params = new object[4][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "I_LOTNO", G_tQCLot.sLotNo };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "I_PROCESSID", G_tRCRecord.sWIPProcessID };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "I_EMPID", g_sUserID };
                Params[3] = new object[] { ParameterDirection.Output, OracleType.VarChar, "O_RES", "" };
                DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_RC_QC_REINSPECT", Params);

                dgvItem.Rows.Clear();
                dgvDefect.Rows.Clear();
                editPassQty.Clear();
                editFailQty.Clear();
                editResult.Clear();

                GetSYSTestType();
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Re-Inspect Error") + ":" + ex.Message, 0);
            }
        }
        // 按下測試小項儲存鍵
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!SaveInspValue())
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Save Insp Value Fail"), 0);
                return;
            }
            //Get_Limit(null);
            SajetCommon.Show_Message(SajetCommon.SetLanguage("Save OK"), 3);
        }
        // 儲存此測試大項的測試小項在暫存資料表，並且呼叫儲存不良資料function RecordDefect()
        private bool SaveInspValue()
        {
            try
            {
                if (dgvItem.Rows.Count > 0)
                {
                    // 檢測時把average與cpk移除
                    int iRow = 0;
                    if (G_tRCRecord.sProcessType.ToUpper() == "IPQC" && bHaveSPC)
                        iRow = 5;
                    //一个SN，一个测试大项，多个测试小项，一个结果
                    for (int i = 4; i < dgvItem.Rows.Count - iRow; i++)
                    {
                        string sSN = dgvItem.Rows[i].Cells["SN"].Value.ToString();
                        sSQL = "DELETE SAJET.G_QC_SN_TESTITEM_TEMP "
                                + " Where SERIAL_NUMBER ='" + sSN + "' "
                                + "    and PROCESS_ID ='" + G_tRCRecord.sWIPProcessID + "' "
                                + "    and item_type_id = '" + dgvItem.Rows[i].Cells["ITEM_TYPE"].Value.ToString() + "' "
                                + "    and qc_lotno = '" + G_tQCLot.sLotNo + "' ";
                        dsTemp = ClientUtils.ExecuteSQL(sSQL);

                        //for (int j = 1; j < dgvItem.Columns.Count - 2; j++)
                        for (int j = 1; j < dgvItem.Columns.Count - 2; j += 2)
                        {
                            //值为空的不保存
                            if (dgvItem.Rows[i].Cells[j].Value == null)
                                continue;
                            if (string.IsNullOrEmpty(dgvItem.Rows[i].Cells[j].Value.ToString()))
                                continue;
                            // 結果為空轉為空字串儲存
                            if (dgvItem.Rows[i].Cells[j + 1].Value == null)
                                dgvItem.Rows[i].Cells[j + 1].Value = "";

                            string sItemID = dgvItem.Columns[j].Name;
                            sSQL = "INSERT INTO SAJET.G_QC_SN_TESTITEM_TEMP "
                                 + "(SERIAL_NUMBER, PROCESS_ID, ITEM_TYPE_ID, ITEM_ID, INSP_VALUE, INSP_RESULT, QC_LOTNO) "
                                 + "values "
                                 + "('" + sSN + "','" + G_tRCRecord.sWIPProcessID + "','"
                                 + dgvItem.Rows[i].Cells["ITEM_TYPE"].Value.ToString() + "','" + sItemID + "','"
                                 + dgvItem.Rows[i].Cells[j].Value.ToString() + "','"
                                //+ (string.IsNullOrEmpty(dgvItem.Rows[i].Cells["RESULT"].Value.ToString()) ? Convert.DBNull : dgvItem.Rows[i].Cells["RESULT"].Value.ToString()) + "','"
                                 + (string.IsNullOrEmpty(dgvItem.Rows[i].Cells[j + 1].Value.ToString()) ? Convert.DBNull : dgvItem.Rows[i].Cells[j + 1].Value.ToString()) + "','"
                                 + G_tQCLot.sLotNo + "') ";
                            dsTemp = ClientUtils.ExecuteSQL(sSQL);
                        }
                    }

                    //记录不良信息进入临时表
                    RecordDefect();

                    //如果保存是在切换测试大项的时候发生的，则改变显示为切换至的测试大项的信息
                    if (dgvItemType.CurrentRow.Cells["ITEM_TYPE_ID"].Value.ToString() != dgvItem.Rows[4].Cells["ITEM_TYPE"].Value.ToString())
                    {
                        SetItem(dgvItemType.CurrentRow.Cells["ITEM_TYPE_ID"].Value.ToString());
                        SetItemValue(G_tRCRecord.sWIPProcessID, dgvItemType.CurrentRow.Cells["ITEM_TYPE_ID"].Value.ToString());
                        //判断当前大项是否已完成测试，已完成的不能修改测试值
                        SetTestItemVisible(dgvItemType.CurrentRow.Cells["QC_RESULT"].Value.ToString());

                        showProcessDefect(dgvItemType.CurrentRow.Cells["ITEM_TYPE_ID"].Value.ToString());
                        SetDefect(G_tQCLot.sLotNo, dgvItemType.CurrentRow.Cells["ITEM_TYPE_ID"].Value.ToString());
                        SetDefectVisible(dgvItemType.CurrentRow.Cells["QC_RESULT"].Value.ToString());
                    }
                    //else
                    //{
                    //    dgvItem.Rows.Clear();
                    //}
                    //txtSN.SelectAll();
                    //txtSN.Focus();
                }

                return true;
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);
                return false;
            }
        }
        // 移除測試小項樣本數觸發的事件
        //private void RemoveItemtoolStripMenuItem1_Click(object sender, EventArgs e)
        //{
        //    if (dgvItem.Rows.Count > 0 && dgvItem.CurrentRow != null)
        //    {
        //        //如果测试大项已完成那么不可以删除不良
        //        if (CheckIsFinished(dgvItemType.CurrentRow.Cells["ITEM_TYPE_ID"].Value.ToString()))
        //        {
        //            string sMsg = SajetCommon.SetLanguage("Test item type is finished");//测试大项已经完成
        //            SajetCommon.Show_Message(sMsg, 1);
        //            return;
        //        }
        //        else
        //        {
        //            string sSQL = " DELETE SAJET.G_QC_SN_TESTITEM_TEMP "
        //                                  + " WHERE SERIAL_NUMBER ='" + this.dgvItem.CurrentRow.Cells[0].Value.ToString() + "' "
        //                                  + " AND PROCESS_ID =" + G_tRCRecord.sWIPProcessID
        //                                  + " AND ITEM_TYPE_ID =" + dgvItemType.CurrentRow.Cells["ITEM_TYPE_ID"].Value.ToString()
        //                //+ " AND ITEM_ID =" + dgvItem.CurrentRow.Cells["ITEM_ID"].ToString()
        //                                  + " AND QC_LOTNO = '" + G_tQCLot.sLotNo + "' ";
        //            ClientUtils.ExecuteSQL(sSQL);
        //            this.dgvItem.Rows.Remove(this.dgvItem.CurrentRow);
        //        }
        //    }
        //}
        // 輸入完測項小項觸發的事件，
        private void dgvItem_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string sMsg = "";
            DataGridViewCell cell = dgvItem.Rows[e.RowIndex].Cells[e.ColumnIndex];
            cell.ErrorText = "";
            // 輸入ave,ca,cp,cpk不帶出結果
            if (G_tRCRecord.sProcessType.ToUpper() == "IPQC" && bHaveSPC)
            {
                if (e.RowIndex > dgvItem.Rows.Count -5)
                    return;
            }

            if (e.ColumnIndex > 0 && e.ColumnIndex % 2 == 1 ) //e.ColumnIndex < dgvItem.Columns.Count - 2
            {
                string sValue = (cell.Value == null) ? "" : cell.Value.ToString();
                
                string sRule = g_alItemRule[e.ColumnIndex - (e.ColumnIndex + 1) / 2].ToString();

                if (!CheckValue(sValue, sRule, out sMsg))
                {
                    dgvItem.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value = "NG";
                    cell.ErrorText = SajetCommon.SetLanguage(sMsg);
                    return;
                }
                else
                {
                    dgvItem.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value = "OK";
                }
                
            }
        }
        private void dgvItem_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvItem.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn && e.RowIndex != -1)
            {
                SendKeys.Send("{F4}");
            }
        }
        // 增加測試小項樣本數
        private void btnAdd_Click(object sender, EventArgs e)
        {

        }
        # endregion

        # region 不良現象程式區域
        /// <summary>
        /// 是否显示不良代码处的sn栏位
        /// </summary>
        private void SetDefectVisible(string sItemTypeResult)
        {
            if (G_tRCRecord.bHaveSN)
                dgvDefect.Columns["SERIAL_NUMBER"].Visible = true;
            else
                dgvDefect.Columns["SERIAL_NUMBER"].Visible = false;

            if (sItemTypeResult != "N/A")
            {
                dgvDefect.ReadOnly = true;
                btnSave.Enabled = false;
                btnFinishItemType.Enabled = false;
                btnAgain.Enabled = false;
            }
            else
            {
                dgvDefect.ReadOnly = false;
                btnSave.Enabled = true;
                btnFinishItemType.Enabled = true;
                btnAgain.Enabled = false;
            }
        }
        // 儲存不良資料在暫存資料表
        private void RecordDefect()
        {
            string sSQL = "";
            try
            {
                if (dgvDefect.Rows.Count > 0)
                {
                    sSQL = "DELETE FROM SAJET.g_qc_sn_defect_temp WHERE QC_LOTNO = '" + G_tQCLot.sLotNo + "' "
                               + " AND ITEM_TYPE_ID = '" + dgvDefect.Rows[0].Cells["ITEM_TYPE_ID_DEFECT"].Value.ToString() + "' ";
                    ClientUtils.ExecuteSQL(sSQL);
                    for (int i = 0; i < dgvDefect.Rows.Count; i++)
                    {
                        if (dgvDefect.Rows[i].Cells["DEFECT_QTY"].Value == null)
                            continue;
                        //Convert.ToInt32(Math.Round(iLotSize, MidpointRounding.AwayFromZero));
                        if (!IsNumeric(1, dgvDefect.Rows[i].Cells["DEFECT_QTY"].Value.ToString()))
                            continue;
                        if (Convert.ToInt32(dgvDefect.Rows[i].Cells["DEFECT_QTY"].Value.ToString()) > 0)    //if (Convert.ToInt32(dgvDefect.Rows[i].Cells["DEFECT_QTY"].Value.ToString()) > 0)
                        {
                            sSQL = "insert into sajet.g_qc_sn_defect_temp(qc_lotno,serial_number,defect_id,defect_qty,item_type_id) "
                                    + " values('" + G_tQCLot.sLotNo + "','"
                                //+ dgvDefect.Rows[i].Cells["SERIAL_NUMBER"].Value.ToString() + "','"
                                    + G_tRCRecord.sRC + "','"
                                    + dgvDefect.Rows[i].Cells["DEFECT_ID"].Value.ToString() + "','"
                                    + dgvDefect.Rows[i].Cells["DEFECT_QTY"].Value.ToString() + "','"
                                    + dgvDefect.Rows[i].Cells["ITEM_TYPE_ID_DEFECT"].Value.ToString() + "')";
                            ClientUtils.ExecuteSQL(sSQL);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);
            }
        }
        // 填入暫存資料表的不良資訊
        private void SetDefect(string sQCLotNo, string sItemTypeID)
        {
            //dgvDefect.Rows.Clear();
            string sSQL = "select a.*, b.item_type_name, c.defect_desc, c.defect_code,SAJET.SJ_DEFECT_LEVEL(c.defect_level) defect_level "
                                + " from sajet.g_qc_sn_defect_temp a, sajet.sys_test_item_type b, sajet.sys_defect c "
                                + " where qc_lotno = '" + sQCLotNo + "' "
                                + " and a.item_type_id = b.item_type_id "
                                + " and a.item_type_id = '" + sItemTypeID + "' "
                                + " and a.defect_id = c.defect_id  "
                                + " order by a.serial_number, b.item_type_name ";
            DataTable dt = ClientUtils.ExecuteSQL(sSQL).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dgvDefect.Rows.Count; j++)
                {
                    if (dgvDefect.Rows[j].Cells["DEFECT_ID"].Value.ToString() == dt.Rows[i]["DEFECT_ID"].ToString())
                    {
                        dgvDefect.Rows[j].Cells["SERIAL_NUMBER"].Value = dt.Rows[i]["SERIAL_NUMBER"].ToString();
                        dgvDefect.Rows[j].Cells["ITEM_TYPE_DEFECT"].Value = dt.Rows[i]["ITEM_TYPE_NAME"].ToString();
                        dgvDefect.Rows[j].Cells["ITEM_TYPE_ID_DEFECT"].Value = dt.Rows[i]["ITEM_TYPE_ID"].ToString();
                        dgvDefect.Rows[j].Cells["DEFECT_CODE"].Value = dt.Rows[i]["DEFECT_CODE"].ToString();
                        dgvDefect.Rows[j].Cells["DEFECT_LEVEL"].Value = dt.Rows[i]["DEFECT_LEVEL"].ToString();
                        dgvDefect.Rows[j].Cells["DEFECT_DESC"].Value = dt.Rows[i]["DEFECT_DESC"].ToString();
                        dgvDefect.Rows[j].Cells["DEFECT_QTY"].Value = dt.Rows[i]["DEFECT_QTY"].ToString();
                        dgvDefect.Rows[j].Cells["DEFECT_ID"].Value = dt.Rows[i]["DEFECT_ID"].ToString();
                        break;
                    }
                }
            }
        }
        // 顯示所有製程不良現象
        private void showProcessDefect(string sItemTypeID)
        {
            try
            {
                dgvDefect.Rows.Clear();

                string sItemType = GetID("SAJET.SYS_TEST_ITEM_TYPE", "ITEM_TYPE_NAME", "ITEM_TYPE_ID", sItemTypeID);
                object[][] Params = new object[1][];
                string sSQL = @" SELECT B.DEFECT_CODE, SAJET.SJ_DEFECT_LEVEL(B.DEFECT_LEVEL) DEFECT_LEVEL, B.DEFECT_DESC, B.DEFECT_ID
                                                FROM SAJET.SYS_RC_PROCESS_DEFECT A, SAJET.SYS_DEFECT B
                                                WHERE A.PROCESS_ID = :PROCESS_ID
                                                AND A.DEFECT_ID = B.DEFECT_ID
                                                AND A.ENABLED = 'Y'
                                                AND B.ENABLED = 'Y'
                                                ORDER BY B.DEFECT_CODE, B.DEFECT_DESC
                                                                                           ";
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", G_tRCRecord.sWIPProcessID };
                DataTable dt = ClientUtils.ExecuteSQL(sSQL, Params).Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dgvDefect.Rows.Add();
                    //dgvDefect.Rows[i].Cells["SERIAL_NUMBER"].Value = dt.Rows[i]["SERIAL_NUMBER"].ToString();
                    dgvDefect.Rows[i].Cells["ITEM_TYPE_DEFECT"].Value = sItemType;
                    dgvDefect.Rows[i].Cells["ITEM_TYPE_ID_DEFECT"].Value = sItemTypeID;
                    dgvDefect.Rows[i].Cells["DEFECT_CODE"].Value = dt.Rows[i]["DEFECT_CODE"].ToString();
                    dgvDefect.Rows[i].Cells["DEFECT_LEVEL"].Value = dt.Rows[i]["DEFECT_LEVEL"].ToString();
                    dgvDefect.Rows[i].Cells["DEFECT_DESC"].Value = dt.Rows[i]["DEFECT_DESC"].ToString();
                    dgvDefect.Rows[i].Cells["DEFECT_QTY"].Value = "0";
                    dgvDefect.Rows[i].Cells["DEFECT_ID"].Value = dt.Rows[i]["DEFECT_ID"].ToString();


                }
                dgvDefect.Columns["ITEM_TYPE_DEFECT"].ReadOnly = true;
                dgvDefect.Columns["DEFECT_CODE"].ReadOnly = true;
                dgvDefect.Columns["DEFECT_LEVEL"].ReadOnly = true;
                dgvDefect.Columns["DEFECT_DESC"].ReadOnly = true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        // 輸入完不良數量觸發的事件，檢查不良總數與驗證輸入資料
        private void dgvDefect_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDefect.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
            {
                string sMsg = SajetCommon.SetLanguage("Defect quantity is Empty.");
                SajetCommon.Show_Message(sMsg, 1);
                return;
            }

            //if (!CheckDataValidation(editPassQty.Text, "^[0-9]+$"))
            //{
            //    string sMsg = SajetCommon.SetLanguage("Data invalid");
            //    SajetCommon.Show_Message(sMsg, 1);
            //    editPassQty.SelectAll();
            //    editPassQty.Focus();
            //    return;
            //}
            if (dgvDefect.Rows.Count > 0 && dgvDefect.Columns[e.ColumnIndex].Name == "DEFECT_QTY")
            {
                if (!IsNumeric(1, dgvDefect.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Input Quantity in Positive integer"), 0);
                    dgvDefect.CurrentCell = dgvDefect.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    //dgvDefect.CurrentCell.Selected = true;
                    //dgvDefect.BeginEdit(true);
                    return;
                }
                else
                {
                    double iQty = 0;
                    for (int i = 0; i < dgvDefect.Rows.Count; i++)
                    {
                        if (dgvDefect.Rows[i].Cells["DEFECT_QTY"].Value == null)
                        {
                            string sMsg = SajetCommon.SetLanguage("Defect quantity is Empty.");
                            SajetCommon.Show_Message(sMsg, 1);
                            return;
                        }
                        if (!IsNumeric(2, dgvDefect.Rows[i].Cells["DEFECT_QTY"].Value.ToString()))
                            return;
                        iQty = iQty + Convert.ToDouble(dgvDefect.Rows[i].Cells["DEFECT_QTY"].Value.ToString());
                    }

                    //if (iQty > G_tQCLot.iLotSize)
                    //{
                    //    SajetCommon.Show_Message(SajetCommon.SetLanguage("Defect Quantity is greater than Sample Size."), 0);
                    //    return;
                    //}
                    if (iQty > Convert.ToDouble(txtInputQty.Text))  //if (iQty > dgvItem.Rows.Count)
                    {
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("Defect Quantity is greater than Input Quantity."), 0);
                        return;
                    }

                }
            }
        }
        private void dgvDefect_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDefect.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn && e.RowIndex != -1)
            {
                SendKeys.Send("{F4}");
            }
        }
        # endregion

        # region SPC計算

        // 將有在產品測試設定SPC測項寫入SPC資料表內
        private void insertSPC()
        {
            try
            {
                if(!bSetTime)
                    dSetTime = System.DateTime.Now;
                string sSPC = @" SELECT SPC_ID
                                                FROM 
                                                SAJET.SYS_PART_QC_PROCESS_RULE A,       
                                                SAJET.SYS_PART_QC_TESTTYPE B,
                                                SAJET.SYS_PART_QC_TESTITEM C
                                                WHERE 1=1 AND ROWNUM=1
                                                AND A.RECID = B.RECID
                                                AND B.RECID = C.RECID
                                                AND A.PART_ID = :PART_ID
                                                AND A.PROCESS_ID = :PROCESS_ID
                                                AND B.ITEM_TYPE_ID = :ITEM_TYPE_ID
                                                AND C.ITEM_ID = :ITEM_ID ";
                string sSQL = " INSERT INTO SAJET.G_SPC "
                                     + " (SPC_ID, SERIAL_NUMBER, UPDATE_TIME, SPC_VALUE, SPC_RESULT, "
                                     + "  WORK_ORDER, PART_ID, PDLINE_ID, STAGE_ID, PROCESS_ID, EMP_ID)  "
                                     + " SELECT "
                                     + "  :SPC_ID, :SERIAL_NUMBER, :UPDATE_TIME, :SPC_VALUE, :SPC_RESULT, "
                                     + "  WORK_ORDER, PART_ID, PDLINE_ID, STAGE_ID, :PROCESS_ID,:EMP_ID "
                                     + " FROM SAJET.G_RC_STATUS "
                                     + " WHERE ROWNUM = 1 AND RC_NO = :RC_NO ";
                string sItemType = dgvItemType.CurrentRow.Cells["ITEM_TYPE_ID"].Value.ToString();

                // 檢測時把average與cpk移除
                int iRow = 0;
                if (G_tRCRecord.sProcessType.ToUpper() == "IPQC" && bHaveSPC)
                    iRow = 5;

                for (int i = 4; i < dgvItem.Rows.Count - iRow; i++)
                {
                    string sSN = dgvItem.Rows[i].Cells["SN"].Value.ToString();
                    for (int j = 1; j < dgvItem.Columns.Count - 1; j += 2)
                    {
                        object[][] SPCs = new object[4][];
                        string sItemID = dgvItem.Columns[j].Name;
                        SPCs[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", G_tRCRecord.sPartID };
                        SPCs[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", G_tRCRecord.sWIPProcessID };
                        SPCs[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", sItemType };
                        SPCs[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_ID", sItemID };
                        DataSet dsTemp = ClientUtils.ExecuteSQL(sSPC, SPCs);
                        if (dsTemp != null)
                        {
                            if (dsTemp.Tables[0].Rows.Count > 0 && dsTemp.Tables[0].Rows[0]["SPC_ID"].ToString() != "0")
                            {
                                string sValue = (dgvItem.Rows[i].Cells[j].Value == null) ? (string)Convert.DBNull : dgvItem.Rows[i].Cells[j].Value.ToString();
                                //string sResult = dgvItem.Rows[i].Cells[j+1].Value.ToString();
                                string sResult = (dgvItem.Rows[i].Cells[j + 1].Value.ToString() == "OK") ? "0" : "1";
                                object[][] Params = new object[8][];
                                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SPC_ID", dsTemp.Tables[0].Rows[0]["SPC_ID"].ToString() };
                                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SERIAL_NUMBER", sSN };
                                Params[2] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", dSetTime };
                                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SPC_VALUE", sValue };
                                Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SPC_RESULT", sResult };
                                Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", G_tRCRecord.sWIPProcessID };
                                Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "EMP_ID", G_tRCRecord.sEmpID };
                                Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", G_tRCRecord.sRC };
                                DataSet ds = ClientUtils.ExecuteSQL(sSQL, Params);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        // 儲存CaCpCpk
        private bool SaveCaCpCpk()
        {
            try
            {
                if (!bSetTime)
                    dSetTime = System.DateTime.Now;
                for (int i = 1; i < dgvItem.Columns.Count - 1; i += 2)
                {
                    if (dgvItem.Rows[0].Cells[i].Value.ToString() != "0") // 當測項與SPC關聯
                    {
                        if (dgvItem.Rows[dgvItem.Rows.Count - 2].Cells[i + 1].Value == null)
                        {
                            SajetCommon.Show_Message(SajetCommon.SetLanguage("Input CPK Result."), 0);
                            return false;
                        }
                        else if (dgvItem.Rows[dgvItem.Rows.Count - 1].Cells[i + 1].Value == null)
                        {
                            SajetCommon.Show_Message(SajetCommon.SetLanguage("Input CPK Basis."), 0);
                            return false;
                        }
                        else
                        {
                            if (dgvItem.Rows[dgvItem.Rows.Count - 2].Cells[i + 1].Value.ToString() == "NG"
                                 && dgvItem.Rows[dgvItem.Rows.Count - 1].Cells[i + 1].Value.ToString() == "OK")
                                G_tRCRecord.sCPKResult = "1";  // CPK結果設定為NG作為重測依據
                        }

                        string sSQL = @" INSERT INTO SAJET.G_SPC_CPK 
                                                       (SPC_ID, UPDATE_TIME, PDLINE_ID, STAGE_ID, PROCESS_ID,
                                                        WORK_ORDER, PART_ID, 
                                                        MEAN, CA, CP, CPK, CPK_RESULT, BASIS, 
                                                        QC_LOTNO, ITEM_TYPE_ID, ITEM_ID, UPDATE_USERID, SERIAL_NUMBER)  
                                                       SELECT 
                                                        :SPC_ID, :UPDATE_TIME, PDLINE_ID, STAGE_ID, :PROCESS_ID, 
                                                        WORK_ORDER, PART_ID, 
                                                        :MEAN, :CA, :CP, :CPK,  :CPK_RESULT, :BASIS, 
                                                        :QC_LOTNO, :ITEM_TYPE_ID, :ITEM_ID, :UPDATE_USERID, RC_NO                                                        
                                                        FROM SAJET.G_RC_STATUS 
                                                        WHERE ROWNUM = 1 AND RC_NO = :RC_NO ";
                        object[][] Params = new object[14][];
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.Number, "SPC_ID", Convert.ToInt32(dgvItem.Rows[0].Cells[i].Value) };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", dSetTime };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", G_tRCRecord.sWIPProcessID };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.Float, "MEAN", Convert.ToDouble(dgvItem.Rows[dgvItem.Rows.Count - 5].Cells[i].Value) };
                        Params[4] = new object[] { ParameterDirection.Input, OracleType.Float, "CA", Convert.ToDouble(dgvItem.Rows[dgvItem.Rows.Count - 4].Cells[i].Value) };
                        Params[5] = new object[] { ParameterDirection.Input, OracleType.Float, "CP", Convert.ToDouble(dgvItem.Rows[dgvItem.Rows.Count - 3].Cells[i].Value) };
                        Params[6] = new object[] { ParameterDirection.Input, OracleType.Float, "CPK", Convert.ToDouble(dgvItem.Rows[dgvItem.Rows.Count - 2].Cells[i].Value) };
                        Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CPK_RESULT", (dgvItem.Rows[dgvItem.Rows.Count - 2].Cells[i + 1].Value.ToString() == "OK") ? "0" : "1" };
                        Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BASIS", (dgvItem.Rows[dgvItem.Rows.Count - 1].Cells[i + 1].Value.ToString() == "OK") ? "0" : "1" };
                        Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "QC_LOTNO", G_tQCLot.sLotNo };
                        Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", dgvItemType.CurrentRow.Cells["ITEM_TYPE_ID"].Value.ToString() };
                        Params[11] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_ID", dgvItem.Columns[i].Name };
                        Params[12] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", G_tRCRecord.sEmpID };
                        Params[13] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", G_tRCRecord.sRC };
                        DataSet ds = ClientUtils.ExecuteSQL(sSQL, Params);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // 計算上下限值
        private void Get_Limit(DataSet dsDataChart)
        {
            for (int j = 1; j < dgvItem.Columns.Count-1; j += 2)
            {
                if (dgvItem.Rows[0].Cells[j].Value.ToString() != "0") // SPC flag = Y
                {
                    decimal[] dX = new decimal[0];
                    decimal[] dR = new decimal[0];
                    //算平均值
                    for (int i = 4; i < dgvItem.Rows.Count - 5; i++)
                    {
                        if (dgvItem.Rows[i].Cells[j].Value == null)
                            continue;
                        string sXBar = dgvItem.Rows[i].Cells[j].Value.ToString();
                        //string sR = gvChartData.Rows[1].Cells[i].Value.ToString();
                        if (!string.IsNullOrEmpty(sXBar))
                        {
                            Array.Resize(ref dX, dX.Length + 1);
                            dX[dX.Length - 1] = decimal.Parse(sXBar);
                        }
                    }
                    tspc.g_AvgX = Formula.GetAverage(dX);
                    dgvItem.Rows[dgvItem.Rows.Count - 5].Cells[j].Value = Math.Round(tspc.g_AvgX, 2).ToString();

                    // 平均差
                    for (int i = 4; i < dgvItem.Rows.Count - 5; i++)
                    {
                        if (dgvItem.Rows[i].Cells[j].Value == null)
                            continue;

                        //string sR = Convert.ToString(tspc.g_AvgX - Convert.ToDecimal(dgvItem.Rows[i].Cells[j].Value.ToString()));
                        string sR = dgvItem.Rows[i].Cells[j].Value.ToString();
                        if (!string.IsNullOrEmpty(sR))
                        {
                            Array.Resize(ref dR, dR.Length + 1);
                            dR[dR.Length - 1] = decimal.Parse(sR);
                        }
                    }
                    if (dR.Length != 0)
                        tspc.g_AvgR = Formula.GetAverage(dR);

                    SPCConstant.GetConstant(Convert.ToInt32(dgvItemType.CurrentRow.Cells["SAMPLE_SIZE"].Value.ToString()));  //g_iSample_Size

                    GetCaCpCpk(dR);
                    dgvItem.Rows[dgvItem.Rows.Count - 4].Cells[j].Value = Math.Round(tspc.Ca,2).ToString(); //+ "% [" + tspc.sCaLevel + "]";
                    dgvItem.Rows[dgvItem.Rows.Count - 3].Cells[j].Value = Math.Round(tspc.Cp, 2).ToString(); //+" [" + tspc.sCpLevel + "]";
                    dgvItem.Rows[dgvItem.Rows.Count - 2].Cells[j].Value = Math.Round(tspc.Cpk, 2).ToString(); //+" [" + tspc.sCpkLevel + "]";
                }
            } 
        }

        // 計算Ca, Cp, Cpk
        private void GetCaCpCpk(decimal[] dRData)
        {
            string[] slValue = new string[3];
            tspc.g_sChartType = "XBRC";
            SPCConstant.GetCapability(tspc.g_sChartType, tspc.g_sUCL1, tspc.g_sCL1, tspc.g_sLCL1, tspc.g_AvgX, tspc.g_AvgR, dRData, ref slValue);

            string sCa = "N/A";
            if (string.IsNullOrEmpty(slValue[0]))
            {
                //LabCa.Text = "Ca = --  Cp = --  Cpk = --";
            }
            else if (slValue[0].ToString() != "N/A")
            {
                tspc.Ca = decimal.Parse(slValue[0].ToString());
                tspc.sCaLevel = SPCConstant.GetCapaLevel(tspc.Ca, "Ca");  //等級
                tspc.Ca = Math.Round(tspc.Ca * 100, 2);
                sCa = tspc.Ca + "% [" + tspc.sCaLevel + "]";


                tspc.Cp = decimal.Parse(slValue[1].ToString());
                tspc.Cpk = decimal.Parse(slValue[2].ToString());
                tspc.sCpLevel = SPCConstant.GetCapaLevel(tspc.Cp, "Cp");
                tspc.sCpkLevel = SPCConstant.GetCapaLevel(tspc.Cpk, "Cpk");

                string sCp = Math.Round(tspc.Cp, 2).ToString() + " [" + tspc.sCpLevel + "]";
                string sCpk = Math.Round(tspc.Cpk, 2).ToString() + " [" + tspc.sCpkLevel + "]";

                //LabCa.Text = "Ca = " + sCa + "  "
                //           + "Cp = " + sCp + "  "
                //           + "Cpk = " + sCpk;
            }
        }

        #endregion

        // 輸入條件值得到對應資料
        private string GetID(string sTable, string sFieldID, string sFieldName, string sValue)
        {
            if (string.IsNullOrEmpty(sValue))
                return "0";
            sSQL = "select " + sFieldID + " from " + sTable + " "
                 + "where " + sFieldName + " = '" + sValue + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
                return dsTemp.Tables[0].Rows[0][sFieldID].ToString();
            else
                return "0";
        }
        //定義一個函數,作用:判斷strNumber是否為數字,是數字返回True,不是數字返回False
        public bool IsNumeric(String strNumber)
        {
            Regex NumberPattern = new Regex("^\\d+$");  // ^\\d+$ 非負整數（正整數 + 0）;     "^[0-9]*[1-9][0-9]*$"//正整數
            return NumberPattern.IsMatch(strNumber);
            // ^(([0-9]+\\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\\.[0-9]+)|([0-9]*[1-9][0-9]*))$ 正浮點數
        }

        /// <summary>
        ///  判斷strNumber是否為指定類型的數字
        ///  1:正整數, 2:非負整數（正整數 + 0）, 3:正浮點數, 4:非負浮點數（正浮點數 + 0）, 5:浮點數
        /// </summary>
        /// <param name="iType"> 數值類型 </param>
        /// <param name="strNumber">判斷的字串</param>
        /// <returns>是返回True,否返回False</returns>
        public bool IsNumeric(int iType, String strNumber)
        {
            Regex NumberPattern = null;
            switch (iType)
            {
                case 1:   //正整數
                    NumberPattern = new Regex("^[0-9]*[1-9][0-9]*$");
                    break;
                case 2:   //非負整數（正整數 + 0）
                    NumberPattern = new Regex("^\\d+$");
                    break;
                case 3:   //正浮點數
                    NumberPattern = new Regex("^(([0-9]+\\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\\.[0-9]+)|([0-9]*[1-9][0-9]*))$");
                    break;
                case 4:   //非負浮點數（正浮點數 + 0）
                    NumberPattern = new Regex("^\\d+(\\.\\d+)?$");
                    break;
                case 5:    //浮點數
                    NumberPattern = new Regex("^(-?\\d+)(\\.\\d+)?$");
                    break;
                default:
                    return false;
                //break;
            }
            return NumberPattern.IsMatch(strNumber);
        }

        // 選擇下一製程
        public bool setNextProcess()
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

                string sSQL = " SELECT * FROM SAJET.G_RC_STATUS WHERE RC_NO ='" + editRC.Text + "' ";
                DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);

                if (dsTemp.Tables[0].Rows.Count > 0)
                {

                    rcRoute.sNext_Process = dsTemp.Tables[0].Rows[0]["NEXT_PROCESS"].ToString();
                    rcRoute.sSheet_Name = dsTemp.Tables[0].Rows[0]["SHEET_NAME"].ToString();
                    rcRoute.g_sRouteID = dsTemp.Tables[0].Rows[0]["ROUTE_ID"].ToString();
                    rcRoute.g_sProcessID = dsTemp.Tables[0].Rows[0]["PROCESS_ID"].ToString();
                }
                else
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("RC NO Error."), 0);
                    return false;
                }

                //sSQL = string.Format(@"SELECT NODE_ID,NODE_TYPE,NODE_CONTENT,NEXT_NODE_ID,LINK_NAME FROM SAJET.SYS_RC_ROUTE_DETAIL WHERE ROUTE_ID={0} AND XML_INDEX={1}", sRoute_Id, index);
                sSQL = string.Format(@"SELECT NODE_ID,NODE_TYPE,NODE_CONTENT,NEXT_NODE_ID,LINK_NAME FROM SAJET.SYS_RC_ROUTE_DETAIL WHERE ROUTE_ID=:ROUTE_ID AND NODE_CONTENT=:NODE_CONTENT ");
                object[][] Params = new object[2][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ROUTE_ID", rcRoute.g_sRouteID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NODE_CONTENT", rcRoute.g_sProcessID };
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

                if (rcRoute.sNode_type == "1" || rcRoute.sNode_type == "2" || rcRoute.sNode_type == "3") // GROUP
                {
                    TransferProcess f = new TransferProcess();

                    f.sNode_type = rcRoute.sNode_type;
                    f.sRoute_Id = rcRoute.g_sRouteID;
                    f.sNode_Id = rcRoute.sNode_Id;

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
                            return false;
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

        private void btnCPK_Click(object sender, EventArgs e)
        {
            // 計算平均數、Ca、Cp、Cpk
            Get_Limit(null);
        }

    }
}