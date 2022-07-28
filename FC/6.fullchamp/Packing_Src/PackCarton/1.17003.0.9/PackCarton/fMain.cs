using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Data.OracleClient;
using System.IO;
using System.Reflection;
using ExportOfficeExcel;

namespace PackCarton
{
    public enum MSGType
    {
        Error,
        Warning,
        OK
    }
    public enum PACKBASE
    {
        WO,
        PartNo,
        Level
    }
    public enum PACKACTION
    {
        RCToCarton,
        BoxToCarton
    }
    public struct RCPackConfig
    {
        public bool WeightCarton;
        public bool PrintLabel;
        public string COM;
        public PACKBASE PackBase;
        public PACKACTION PackAction;
        public int CartonCapacity;
        public string sOutListPath;
        public string sProcessID;
    };
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();
        }

        string g_sProgram, g_sPartID, g_sSPEC1;
        public static string g_sExeName;
        public static string g_sUserID;
        public static string g_sUserNo;
        //public static string g_sProcessID;
        public static string g_sFunction = "";
        public ListView listbPrint = new ListView();
        string g_sLevel, g_sModelType, g_sCustomer, g_sSize;
        DataTable g_dtConfig;

        RCPackConfig PackConfig = new RCPackConfig();

        /// <summary>
        /// 是否为箱内第一笔输入
        /// </summary>
        bool bFirstPackFlag = true;

        private void fMain_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            this.WindowState = FormWindowState.Maximized;

            g_sProgram = ClientUtils.fProgramName;
            g_sUserID = ClientUtils.UserPara1;
            g_sUserNo = ClientUtils.fLoginUser;
            g_sExeName = ClientUtils.fCurrentProject;
            g_sFunction = ClientUtils.fFunctionName;
            GetPackType();
            try
            {

                string sSQL = "select * from sajet.sys_module_param p where p.module_name='PACKING' and p.function_name='PACK CARTON'";
                g_dtConfig = ClientUtils.ExecuteSQL(sSQL).Tables[0];
                if (g_dtConfig.Rows.Count == 0)
                {
                    SajetCommon.Show_Message("Get Config Error", 1);
                }
                else
                {
                    if (!GetConfig())
                        return;
                }
            }
            catch { }

            ClearForm();

            GetLotNo();

            SetFocus(txtInput);
            txtMsg.Font = new Font(txtMsg.Font.FontFamily, txtMsg.Height, txtMsg.Font.Style);
        }

        private bool GetLotNo()
        {
            try
            {
                object[][] Params = new object[2][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "i_empid", g_sUserID };
                Params[1] = new object[] { ParameterDirection.Output, OracleType.VarChar, "o_lotno", "" };
                DataTable dt = ClientUtils.ExecuteProc("sajet.sj_rc_pack_get_lotno", Params).Tables[0];
                txtLotNo.Text = dt.Rows[0][0].ToString();
                if (txtLotNo.Text == "NG")
                {
                    Show_Message(SajetCommon.SetLanguage("Get Lot No Error"), MSGType.Error);
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void fMain_Shown(object sender, EventArgs e)
        {
            txtInput.Focus();
        }

        private void txtInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar != (char)Keys.Return)
                    return;

                Show_Message(SajetCommon.SetLanguage(""), MSGType.OK);

                txtInput.Text = txtInput.Text.Trim();
                txtLotNo.Text = txtLotNo.Text.Trim();

                if (string.IsNullOrEmpty(txtLotNo.Text))
                {
                    Show_Message(SajetCommon.SetLanguage("Lot No Empty"), MSGType.Error);
                    return;
                }

                if (cmbtype.SelectedIndex < 0)
                {
                    Show_Message(SajetCommon.SetLanguage("TYPE ERROR"), MSGType.Error);
                    return;
                }
                else
                {
                    txtPackAction.Text = cmbtype.Text;
                    if (txtPackAction.Text.Equals("RC->CARTON"))
                    {
                        PackConfig.PackAction = PACKACTION.RCToCarton;
                    }
                    else if (txtPackAction.Text.Equals("BOX->CARTON"))
                    {
                        PackConfig.PackAction = PACKACTION.BoxToCarton;
                    }
                }

                //是否考虑拆箱后重新包装的?
                if (!CheckInput())
                    return;

                //如果不是箱内的第一笔,则表示存在未关箱的符合当前输入的箱子
                if (!bFirstPackFlag)
                {
                    RefreshCarton();

                    if (int.Parse(txtCartonQty.Text) >= int.Parse(txtCartonCapacity.Text))
                    {
                        Show_Message(SajetCommon.SetLanguage("The carton is full"), MSGType.Error);
                        return;
                    }
                }
                else
                {
                    string CartonNo = "";
                    if (!GetNewCartonNo(out CartonNo))
                    {
                        SetFocus(txtInput);
                        return;
                    }
                    txtCartonNo.Text = CartonNo;
                }

                //执行包装作业,更改工单状态,写入ERP接口,箱子已满则关箱
                if (!PackingExecute())
                {
                    SetFocus(txtInput);
                    return;
                }

                for (int i = 0; i < dgvRemainder.Rows.Count; i++)
                {
                    if (txtInput.Text == dgvRemainder.Rows[i].Cells["RC_NO"].Value.ToString())
                    {
                        dgvRemainder.Rows.RemoveAt(i);
                        break;
                    }
                }

                SetFocus(txtInput);
                txtInput.Text = "";
            }
            catch (Exception ex)
            {
                Show_Message(SajetCommon.SetLanguage("Pack Carton Error:") + ex.Message, MSGType.Error);
            }
        }

        private bool CheckInput()
        {
            if (string.IsNullOrEmpty(txtInput.Text))
            {
                Show_Message("Data Is Null", MSGType.Error);
                SetFocus(txtInput);
                return false;
            }

            try
            {
                object[][] Params = new object[8][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "I_INPUT", txtInput.Text };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "I_ACTION", (PackConfig.PackAction == PACKACTION.BoxToCarton) ? 1 : 0 };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "I_BASE", (PackConfig.PackBase == PACKBASE.PartNo) ? 1 : 0 };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "I_CARTON", txtCartonNo.Text == "N/A" ? Convert.DBNull : txtCartonNo.Text };
                Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "I_PROCESS", PackConfig.sProcessID };
                Params[5] = new object[] { ParameterDirection.Output, OracleType.VarChar, "O_RES", "" };
                Params[6] = new object[] { ParameterDirection.Output, OracleType.VarChar, "O_FLAG", "" };
                Params[7] = new object[] { ParameterDirection.Output, OracleType.VarChar, "O_CARTON", "" };
                DataTable dt = ClientUtils.ExecuteProc("sajet.sj_pack_carton_chk_input", Params).Tables[0];
                if (dt.Rows[0]["o_res"].ToString().ToUpper() != "OK")
                {
                    Show_Message(dt.Rows[0]["o_res"].ToString().ToUpper(), MSGType.Error);
                    SetFocus(txtInput);
                    return false;
                }
                else
                {
                    //获取料号,工单等信息
                    string sSQL = @"select A.WORK_ORDER, B.PART_NO, B.SPEC1, C.LEVEL_CODE, A.PART_ID, B.OPTION12, B.CUST_PART_NO, b.option1
                                                from SAJET.G_RC_CARTON A, SAJET.SYS_PART B, SAJET.G_RC_LEVEL C
                                                WHERE A.BOX_NO = '" + txtInput.Text + @"'
                                                AND A.PART_ID = B.PART_ID
                                                AND A.RC_NO = C.RC_NO(+) ";
                    DataTable dtInfo = ClientUtils.ExecuteSQL(sSQL).Tables[0];
                    txtWO.Text = dtInfo.Rows[0]["work_order"].ToString();
                    txtPartNo.Text = dtInfo.Rows[0]["part_no"].ToString();
                    g_sSPEC1 = dtInfo.Rows[0]["SPEC1"].ToString();
                    g_sLevel = dtInfo.Rows[0]["level_code"].ToString();
                    g_sPartID = dtInfo.Rows[0]["PART_ID"].ToString();
                    g_sModelType = dtInfo.Rows[0]["OPTION12"].ToString();
                    g_sCustomer = dtInfo.Rows[0]["CUST_PART_NO"].ToString();
                    g_sSize = dtInfo.Rows[0]["OPTION1"].ToString();

                    //没有可用箱号
                    if (dt.Rows[0]["o_flag"].ToString().ToUpper() == "Y")
                        bFirstPackFlag = true;
                    else
                    {
                        //有可用箱号
                        bFirstPackFlag = false;
                        txtCartonNo.Text = dt.Rows[0]["o_carton"].ToString();
                    }

                    if (!GetCapacity())
                        return false;

                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }

        }

        /// <summary>
        /// 根据类型和Text显示消息
        /// </summary>
        /// <param name="sText">要显示的消息</param>
        /// <param name="type">消息类型</param>
        /// <param name="autotranslate">自动翻译</param>
        /// <returns></returns>
        public void Show_Message(string sText, MSGType type)
        {
            int ifreq = 800; int iduration = 200;
            txtMsg.Text = SajetCommon.SetLanguage(sText);

            switch (type)
            {
                case MSGType.Error: //Error                        
                    txtMsg.ForeColor = Color.Red;
                    txtMsg.BackColor = Color.Silver;
                    Console.Beep(ifreq, iduration);
                    break;
                case MSGType.Warning: //Warning                        
                    txtMsg.ForeColor = Color.Blue;
                    txtMsg.BackColor = Color.FromArgb(255, 255, 128);
                    Console.Beep(ifreq, iduration);
                    break;
                default:
                    txtMsg.ForeColor = Color.Green;
                    txtMsg.BackColor = Color.White;
                    break;
            }
        }
        /// <summary>
        /// 添加一笔包装箱记录
        /// </summary>
        /// <param name="sType"></param>
        /// <param name="sNo"></param>
        public bool Append_PackNo(string sNo)
        {
            if (string.IsNullOrEmpty(sNo) || sNo == "N/A")
            {
                return false;
            }

            try
            {
                string sSQL = " SELECT carton_no From sajet.g_pack_carton "
                      + " Where carton_no = '" + sNo + "' and rownum=1";
                DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows.Count == 0)
                {
                    //获取当前批次的最大序号+1
                    sSQL = "select nvl(max(lot_seq),0) + 1 from sajet.g_pack_carton where lot_no = '" + txtLotNo.Text + "'";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);
                    string sSeq = dsTemp.Tables[0].Rows[0][0].ToString();

                    sSQL = " INSERT INTO sajet.g_pack_carton "
                         + " (carton_no,WORK_ORDER,PART_ID,CLOSE_FLAG,CREATE_EMP_ID,PROCESS_ID,LOT_NO,LOT_SEQ,LEVEL_CODE) "
                         + " VALUES "
                         + " ('" + sNo + "','" + txtWO.Text + "','" + g_sPartID + "','N','" + g_sUserID + "','" + PackConfig.sProcessID + "','" + txtLotNo.Text + "','" + sSeq + "','" + g_sLevel + "')";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);

                }
                return true;
            }
            catch (Exception ex)
            {
                Show_Message(SajetCommon.SetLanguage("Add Pack Record Error:") + ex.Message, MSGType.Error);
                return false;
            }
        }

        /// <summary>
        /// 获取箱子是否已满，是否已关箱
        /// </summary>
        /// <param name="CartonNo"></param>
        /// <param name="isClose"></param>
        /// <returns></returns>
        //        bool GetCartonInfo(string CartonNo, out bool isFull, out bool isClose)
        //        {
        //            isClose = false;
        //            isFull = false;
        //            if (string.IsNullOrEmpty(CartonNo))
        //            {
        //                Show_Message("Carton No is Empty", MSGType.Error);
        //                return false;
        //            }
        //            try
        //            {
        //                string cmd = @"  select pc.carton_no,pc.close_flag,p.carton_qty
        //                                 from sajet.g_pack_carton pc,sajet.sys_part_pkspec pp ,sajet.sys_pkspec p 
        //                                  where pc.part_id=pp.part_id and p.pkspec_id=pp.pkspec_id and pc.carton_no='" + CartonNo + "'";
        //                DataTable dt = ClientUtils.ExecuteSQL(cmd).Tables[0];
        //                if (dt.Rows.Count == 0)
        //                {
        //                    Show_Message(SajetCommon.SetLanguage("No This Carton Record"), MSGType.Error);
        //                    return false;
        //                }
        //                isClose = (dt.Rows[0]["Close_Flag"].ToString() == "Y");
        //                cmd = @"select count(ss.serial_number) as sncount from sajet.g_sn_status ss where ss.carton_no='" + CartonNo + "'";
        //                DataTable dt2 = ClientUtils.ExecuteSQL(cmd).Tables[0];
        //                isFull = (int.Parse(dt.Rows[0]["CARTON_QTY"].ToString()) == int.Parse(dt2.Rows[0]["SNCOUNT"].ToString()));
        //                return true;
        //            }
        //            catch (Exception ex)
        //            {
        //                Show_Message(SajetCommon.SetLanguage("Get Carton Info Error:") + ex.Message, MSGType.Error);
        //                return false;
        //            }
        //        }

        /// <summary>
        /// 刷新Carton,获取当前Carton下BOX,RC数量,并刷新界面
        /// </summary>
        /// <returns></returns>
        bool RefreshCarton()
        {
            string CartonNo = txtCartonNo.Text;
            if (string.IsNullOrEmpty(CartonNo) || CartonNo.Equals("N/A"))
            {
                Show_Message("Carton No Is Empty", MSGType.Error);
                return false;
            }
            try
            {//若没有SN则不关联g_sn_status    左连接  Alisa    20170111
                string cmd = @"
SELECT 
    BOX_NO, PART_NO,QTY, LEVEL_CODE, REMAINDER, LEVEL_CODE LEVEL_2, CARTON_QTY
FROM (
    SELECT
        A2.BOX_NO, C.PART_NO, A2.QTY, B.LEVEL_CODE, (E.CURRENT_QTY - A2.QTY) REMAINDER, B.LEVEL_CODE LEVEL_2, A2.CREATECARTON_TIME, A1.CARTON_QTY
    FROM 
        (SELECT CARTON_NO, SUM(QTY) CARTON_QTY FROM SAJET.G_RC_CARTON  WHERE CARTON_NO = '" + CartonNo + @"' GROUP BY CARTON_NO) A1,
        SAJET.G_RC_CARTON A2,
        SAJET.G_PACK_CARTON B,
        SAJET.SYS_PART C,
        SAJET.G_SN_STATUS D,
        SAJET.G_RC_STATUS E
    WHERE
        A1.CARTON_NO = A2.CARTON_NO
        AND A1.CARTON_NO = B.CARTON_NO
        AND A2.PART_ID = C.PART_ID
        AND A2.RC_NO = D.RC_NO(+)
        AND A2.RC_NO = E.RC_NO(+)
    GROUP BY A2.BOX_NO, A1.CARTON_QTY, A2.QTY, C.PART_NO, B.LEVEL_CODE, A2.CREATECARTON_TIME, (E.CURRENT_QTY - A2.QTY)
    ORDER BY A2.CREATECARTON_TIME ASC)";



                DataTable dt = ClientUtils.ExecuteSQL(cmd).Tables[0];
                dgvCartonContent.DataSource = dt;
                dgvCartonContent.Columns["carton_qty"].Visible = false;
                dgvCartonContent.Columns["LEVEL_2"].Visible = false;
                SajetCommon.SetLanguageControl(this);
                //int iQTY = 0;
                //for (int i = 0; i < dgvCartonContent.Rows.Count; i++)
                //{
                //    iQTY += int.Parse(dgvCartonContent.Rows[i].Cells["qty"].Value.ToString());
                //}
                //txtCartonQty.Text = iQTY.ToString();
                txtCartonQty.Text = dgvCartonContent.Rows[0].Cells["carton_qty"].Value.ToString();
                return true;
            }
            catch (Exception ex)
            {
                Show_Message(SajetCommon.SetLanguage("Refresh Carton Error:") + ex.Message, MSGType.Error);
                return false;
            }
        }

        void SetFocus(Control c)
        {
            c.Enabled = true;
            c.Focus();
            c.BackColor = Color.Khaki;
            if (c is TextBox)
                ((TextBox)c).SelectAll();
        }

        /// <summary>
        /// 获取新的Carton序号并添加一笔新Carton记录
        /// </summary>
        /// <returns></returns>
        bool GetNewCartonNo(out string CartonNo)
        {
            CartonNo = "";
            LabelCheck.Check LabelCheckDll = new LabelCheck.Check();
            try
            {
                string sSQL = "";
                if (PackConfig.PackBase == PACKBASE.WO)
                {
                    sSQL = @"
                     select p.work_order,p.wo_rule ,rn.rule_name,rn.rule_type,rp.parame_name,rp.parame_item,rp.parame_value
                     from sajet.g_wo_base p,sajet.sys_module_param mp,sajet.sys_rule_name rn,sajet.g_wo_param rp
                     where p.wo_rule =mp.function_name
                            and p.work_order ='" + txtWO.Text + @"'
                            and mp.parame_item=rn.rule_name
                            and rn.rule_type='CARTON NO'
                            and p.work_order=rp.work_order and rp.module_name='CARTON NO RULE' 
                           ORDER BY RP.parame_ITEM ";//获取编码规格
                }

                if (PackConfig.PackBase == PACKBASE.PartNo)
                {
                    sSQL = @"
                    select p.part_no,p.rule_set,rn.rule_name,rn.rule_type,rp.parame_name,rp.parame_item,rp.parame_value
                     from sajet.sys_part p,sajet.sys_module_param mp,sajet.sys_rule_name rn,sajet.sys_rule_param rp
                     where p.rule_set=mp.function_name
                            and p.part_no='" + txtPartNo.Text + @"'
                            and mp.parame_item=rn.rule_name
                            and rn.rule_type='CARTON NO'
                            and rn.rule_id=rp.rule_id 
                     ORDER BY RP.parame_ITEM ";//获取编码规格
                }

                DataTable dt = ClientUtils.ExecuteSQL(sSQL).Tables[0];
                if (dt.Rows.Count == 0)//此料号没有维护Carton编码规则，使用默认编码规则：线别+YYMMDDXXXX
                {
                    Show_Message(SajetCommon.SetLanguage("No This Part No Rule"), MSGType.Error);
                    return false;
                }
                else
                {
                    string sCode = "";
                    string sDefault = "";
                    string sCarry = "";
                    string sCheckSum = "";
                    string sResetSeqCycle = "";
                    string sResetSeq = "N";
                    string sRuleName = dt.Rows[0]["RULE_NAME"].ToString();
                    ListView LVUserDay = new ListView();
                    ListView LVUserSeq = new ListView();
                    ListView LVFun = new ListView();
                    object[] objRuleData = new object[3];
                    string[] sParam = new string[1];
                    bool flag = false;//是否自定义函数
                    string param1 = "";
                    string param2 = "";
                    string param3 = "";
                    //string sValue = "";
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        string sParam_Name = dt.Rows[i]["PARAME_NAME"].ToString();
                        string sParam_Item = dt.Rows[i]["PARAME_ITEM"].ToString();
                        string sParam_Value = dt.Rows[i]["PARAME_VALUE"].ToString();
                        //Default
                        if (sParam_Name == "Carton No Code")//Carton No Code
                        {
                            if (sParam_Item == "Code")//CYWWRTTT,编码规则
                                sCode = sParam_Value;
                            else if (sParam_Item == "Default")//MYWWRTTT,默认编码规则(填充了C(字符串常量))
                                sDefault = sParam_Value;
                            else if (sParam_Item == "Code Type")//10进制
                                sCarry = sParam_Value;
                            continue;
                        }
                        //
                        switch (sParam_Name)
                        {
                            case "Month User Define":
                                LVUserDay.Items.Add("m");
                                LVUserDay.Items[LVUserDay.Items.Count - 1].SubItems.Add(sParam_Value);
                                continue;
                            case "Day User Define":
                                LVUserDay.Items.Add("d");
                                LVUserDay.Items[LVUserDay.Items.Count - 1].SubItems.Add(sParam_Value);
                                continue;
                            case "Week User Define":
                                LVUserDay.Items.Add("w");
                                LVUserDay.Items[LVUserDay.Items.Count - 1].SubItems.Add(sParam_Value);
                                continue;
                            case "Day of Week User Define":
                                LVUserDay.Items.Add("k");
                                LVUserDay.Items[LVUserDay.Items.Count - 1].SubItems.Add(sParam_Value);
                                continue;
                            case "Check Sum":
                                sCheckSum = sParam_Value;
                                continue;
                            case "Reset Sequence":
                                if (sParam_Item == "1")
                                    sResetSeq = "Y";
                                else
                                    sResetSeq = "N"; //不要Reset
                                sResetSeqCycle = sParam_Value;//0
                                continue;
                        }
                        //自行定義的Function             
                        if (sParam_Name.IndexOf("Digit Type & Field") != -1)//用户自定义函数
                        {

                            flag = true;
                            param1 = sParam_Name.Substring(0, 1);
                            param2 = sParam_Value;
                            param3 = sParam_Item;

                            //ZIWEN
                            //string sData = "N/A";
                            //if (param3 != "N/A")
                            //{
                            //    if (param3 == "RC")
                            //    {
                            //        sData = txtInput.Text + "," + param1 + "," + sDefault;
                            //    }
                            //    else if (param3 == "WATER_CODE")
                            //    {
                            //        sData = txtInput.Text + "," + param1 + "," + sDefault;
                            //    }
                            //    else
                            //        sData = txtWO.Text + "," + param1 + "," + sDefault;
                            //}
                            //sSQL = " select " + param2 + "('" + sData + "') fundata from dual ";
                            //DataSet ds = ClientUtils.ExecuteSQL(sSQL);
                            //if (sValue == "")
                            //    sValue = ds.Tables[0].Rows[0]["fundata"].ToString();
                            //else
                            //    sValue = sValue + ds.Tables[0].Rows[0]["fundata"].ToString();

                            continue;
                        }
                        //用户自定义编码规则
                        if (sParam_Name == "Carton No User Define")
                        {
                            LVUserSeq.Items.Add(sParam_Item);
                            LVUserSeq.Items[LVUserSeq.Items.Count - 1].SubItems.Add(sParam_Value);
                            continue;
                        }
                    }

                    if (flag)
                    {
                        string sData = "N/A";
                        if (param3 != "N/A")
                        {
                            //sData = sData = dsWoData.Tables[0].Rows[0][sParam_Item].ToString();
                            sData = sDefault;
                        }
                        //sSQL = " select " + param2 + " fundata from dual ";
                        sSQL = " select " + param2 + "('" + sData + "') fundata from dual ";
                        DataSet ds = ClientUtils.ExecuteSQL(sSQL);
                        string sValue = ds.Tables[0].Rows[0]["fundata"].ToString();

                        //LVFun.Items[LVFun.Items.Count - 1].SubItems.Add(sValue);
                        sDefault = sValue;
                    }

                    Array.Clear(objRuleData, 0, objRuleData.Length);
                    Array.Resize(ref objRuleData, 9);
                    Array.Clear(sParam, 0, sParam.Length);
                    Array.Resize(ref sParam, 9);
                    sParam[0] = "Code";
                    objRuleData[0] = sCode;
                    sParam[1] = "Default";
                    objRuleData[1] = sDefault;
                    sParam[2] = "Code Type"; //10/16進位
                    objRuleData[2] = sCarry;
                    sParam[3] = "User DayCode";
                    objRuleData[3] = LVUserDay;
                    sParam[4] = "User Seq"; //進位方式
                    objRuleData[4] = LVUserSeq;
                    sParam[5] = "User Function";
                    objRuleData[5] = LVFun;
                    sParam[6] = "Check Sum";
                    objRuleData[6] = sCheckSum;
                    sParam[7] = "Reset Sequence";
                    objRuleData[7] = sResetSeq;
                    sParam[8] = "Reset Cycle";
                    objRuleData[8] = sResetSeqCycle;
                    //重置依据
                    string sSeqFix = "";
                    sSQL = "SELECT SEQ_NAME from sajet.sys_label "
                           + "where Upper(label_name) = 'CARTON NO' ";
                    DataTable dtTemp = ClientUtils.ExecuteSQL(sSQL).Tables[0];//找到Sequence前缀
                    if (dtTemp.Rows.Count > 0)
                        sSeqFix = dtTemp.Rows[0]["SEQ_NAME"].ToString();//Carton No,S_CTN_
                    else
                        sSeqFix = "S_CTN_";
                    string sResetMark = "";//上一次重置的记录
                    sSQL = "Select PARAME_VALUE "
                          + "From SAJET.SYS_MODULE_PARAM "
                          + "Where UPPER(MODULE_NAME) = '" + sSeqFix.ToUpper() + "' "
                          + "and FUNCTION_NAME = '" + sRuleName + "' "
                          + "and PARAME_NAME = 'Reset Sequence Mark' ";

                    dtTemp = ClientUtils.ExecuteSQL(sSQL).Tables[0];
                    if (dtTemp.Rows.Count > 0)
                        sResetMark = dtTemp.Rows[0]["PARAME_VALUE"].ToString();

                    //获取Sequence
                    string sSeqName = sSeqFix + sRuleName;//Sequence名称，根据Carton编码规则命名
                    // sSQL = "select * from all_sequence";
                    //产生新箱号ziwen
                    //LabelCheck.Check lc = new LabelCheck.Check();
                    if (!LabelCheckDll.Create_NewNo(out CartonNo, sSeqName, ref sResetMark, sParam, objRuleData))
                    {
                        Show_Message("Create New Carton No Error", MSGType.Error);
                        return false;
                    }
                    sSQL = "Select rowid "
                            + "From SAJET.SYS_MODULE_PARAM "
                            + "Where UPPER(MODULE_NAME) = '" + sSeqFix.ToUpper() + "' "
                            + "and FUNCTION_NAME = '" + sRuleName + "' "
                            + "and PARAME_NAME = 'Reset Sequence Mark' ";
                    dtTemp = ClientUtils.ExecuteSQL(sSQL).Tables[0];
                    if (dtTemp.Rows.Count == 0)
                    {
                        sSQL = " Insert Into SAJET.SYS_MODULE_PARAM "
                              + " (MODULE_NAME,FUNCTION_NAME,PARAME_NAME,PARAME_ITEM,PARAME_VALUE,UPDATE_USERID ) "
                              + " Values "
                              + " ('" + sSeqFix.ToUpper() + "','" + sRuleName + "','Reset Sequence Mark','" + sRuleName + "','" + sResetMark + "','" + g_sUserID + "' )";
                        ClientUtils.ExecuteSQL(sSQL);
                    }
                    else
                    {
                        string sRowid = dtTemp.Rows[0]["rowid"].ToString();
                        sSQL = " update SAJET.SYS_MODULE_PARAM "
                              + " set parame_value = '" + sResetMark + "' "
                              + " where rowid = '" + sRowid + "' ";
                        ClientUtils.ExecuteSQL(sSQL);
                    }
                }

                //添加记录
                Append_PackNo(CartonNo);

                return true;
            }
            catch (Exception ex)
            {
                Show_Message(SajetCommon.SetLanguage("Get New Carton No Error:") + ex.Message, MSGType.Error);
                return false;
            }

        }

        /// <summary>
        /// 获取RC的工单，料号信息,RC剩余数量
        /// </summary>
        /// <returns></returns>
        //        bool GetRCInfo(string rc, out string WO, out string PartNo, out string SurplusQty, out string TargetQty)
        //        {
        //            WO = "";
        //            PartNo = "";
        //            SurplusQty = "0";
        //            TargetQty = "0";
        //            if (string.IsNullOrEmpty(rc.Trim()))
        //            {
        //                Show_Message("RC NO IS NULL", MSGType.Error);
        //                return false;
        //            }

        //            string cmd = @"select wo.work_order,p.part_no,p.part_id,wo.target_qty
        //                            from sajet.g_rc_status ss,sajet.sys_part p,sajet.g_wo_base wo
        //                            where ss.part_id=p.part_id
        //                                and ss.work_order=wo.work_order
        //                           and ss.rc_no='" + rc.Trim() + "'";
        //            try
        //            {
        //                DataTable dt = ClientUtils.ExecuteSQL(cmd).Tables[0];
        //                if (dt.Rows.Count == 0)
        //                {
        //                    Show_Message(SajetCommon.SetLanguage("Can not Find RC No(") + txtInput.Text.Trim() + SajetCommon.SetLanguage(")Info"), MSGType.Error);
        //                    return false;
        //                }
        //                WO = dt.Rows[0]["WORK_ORDER"].ToString();
        //                PartNo = dt.Rows[0]["PART_NO"].ToString();
        //                g_sPartID = dt.Rows[0]["PART_ID"].ToString();
        //                TargetQty = dt.Rows[0]["TARGET_QTY"].ToString();
        //            }
        //            catch (Exception ex)
        //            {
        //                Show_Message(SajetCommon.SetLanguage("Get SN Info Error:") + ex.Message, MSGType.Error);
        //                return false;
        //            }
        //            return true;
        //        }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <returns></returns>
        private bool GetConfig()
        {
            PackConfig.COM = "";
            PackConfig.PrintLabel = false;
            PackConfig.WeightCarton = false;
            PackConfig.PackBase = PACKBASE.PartNo;
            PackConfig.PackAction = PACKACTION.BoxToCarton;
            PackConfig.CartonCapacity = 300;
            PackConfig.sOutListPath = @"C:\CartonList";

            for (int i = 0; i < g_dtConfig.Rows.Count; i++)
            {
                string sParameItem = g_dtConfig.Rows[i]["PARAME_ITEM"].ToString();
                string sParameValue = g_dtConfig.Rows[i]["PARAME_VALUE"].ToString();
                string sParameName = g_dtConfig.Rows[i]["PARAME_NAME"].ToString();
                switch (sParameItem)
                {
                    case "WEIGHT CARTON":
                        {
                            if (string.IsNullOrEmpty(sParameValue))//为空
                            {
                                Show_Message("PACK CARTON:NOT CONFIG WEIGHT CARTON", MSGType.Error);
                                return false;
                            }
                            cbWeightCarton.Checked = (sParameValue == "Y");
                            PackConfig.WeightCarton = cbWeightCarton.Checked;
                            if (cbWeightCarton.Checked)
                            {
                                cbWeightCarton.BackColor = Color.LightGreen;
                            }
                            break;
                        }
                    case "PACK BASE":
                        {
                            if (string.IsNullOrEmpty(sParameValue))
                            {
                                Show_Message("PACK CARTON:NOT CONFIG PACK BASE", MSGType.Error);
                                return false;
                            }
                            txtPackBase.Text = sParameValue;
                            if (txtPackBase.Text.Equals("WO"))
                            {
                                PackConfig.PackBase = PACKBASE.WO;
                            }
                            else if (txtPackBase.Text.Equals("PARTNO"))
                            {
                                PackConfig.PackBase = PACKBASE.PartNo;
                            }
                            else
                            {
                                Show_Message(SajetCommon.SetLanguage("PACK CARTON:Unknown Pack Base:") + txtPackBase.Text, MSGType.Error);
                                return false;
                            }
                            break;
                        }
                    case "PACK ACTION":
                        {
                            if (string.IsNullOrEmpty(sParameValue))
                            {
                                Show_Message("PACK CARTON:NOT CONFIG PACK ACTION", MSGType.Error);
                                return false;
                            }
                            txtPackAction.Text = sParameValue;
                            if (txtPackAction.Text.Equals("RC->CARTON"))
                            {
                                PackConfig.PackAction = PACKACTION.RCToCarton;
                            }
                            else if (txtPackAction.Text.Equals("BOX->CARTON"))
                            {
                                PackConfig.PackAction = PACKACTION.BoxToCarton;
                            }
                            else
                            {
                                Show_Message("Unknown Pack Action", MSGType.Error);
                                return false;
                            }
                            break;
                        }
                    case "PRINT LABEL":
                        {
                            if (string.IsNullOrEmpty(sParameValue))
                            {
                                Show_Message("PACK CARTON:NOT CONFIG PRINT LABEL", MSGType.Error);
                                return false;
                            }
                            cbPrintLabel.Checked = (sParameValue == "Y");
                            PackConfig.PrintLabel = cbPrintLabel.Checked;
                            if (cbPrintLabel.Checked)
                            {
                                cbPrintLabel.BackColor = Color.LightGreen;
                            }
                            break;
                        }
                    case "CAPACITY":
                        {
                            if (string.IsNullOrEmpty(sParameValue))
                            {
                                Show_Message("PACK CARTON:NOT CONFIG CARTON CAPACITY", MSGType.Error);
                                return false;
                            }
                            if (sParameName == g_sSize)
                            {
                                PackConfig.CartonCapacity = int.Parse(sParameValue);
                                txtCartonCapacity.Text = sParameValue;
                            }
                            break;
                        }
                    case "PATH":
                        {
                            if (!string.IsNullOrEmpty(sParameValue))
                            {
                                PackConfig.sOutListPath = sParameValue;
                            }
                            break;
                        }
                    case "PROCESS":
                        {
                            if (string.IsNullOrEmpty(sParameValue))
                            {
                                Show_Message("PACK CARTON:NOT CONFIG PACKCARTON PROCESS", MSGType.Error);
                                return false;
                            }
                            PackConfig.sProcessID = sParameValue;
                            break;
                        }
                    default:
                        {
                            Show_Message(SajetCommon.SetLanguage("PACK CARTON:Unknown Config Item:") + sParameItem, MSGType.Error);
                            return false;

                        }
                }
            }
            return true;
        }

        /// <summary>
        /// 获取容量的配置
        /// </summary>
        /// <param name="sParameItem"></param>
        /// <returns></returns>
        private bool GetCapacity()
        {
            for (int i = 0; i < g_dtConfig.Rows.Count; i++)
            {
                string sParameValue = g_dtConfig.Rows[i]["PARAME_VALUE"].ToString();
                string sParameName = g_dtConfig.Rows[i]["PARAME_NAME"].ToString();
                string sParameItem = g_dtConfig.Rows[i]["PARAME_ITEM"].ToString();

                if (sParameItem == "CAPACITY")
                {
                    if (string.IsNullOrEmpty(sParameValue))
                    {
                        Show_Message("PACK CARTON:NOT CONFIG CARTON CAPACITY", MSGType.Error);
                        return false;
                    }
                    //不同尺寸容量不同
                    if (sParameName == g_sSize)
                    {
                        PackConfig.CartonCapacity = int.Parse(sParameValue);
                        txtCartonCapacity.Text = sParameValue;
                        break;
                    }
                }
            }
            return true;
        }

        private void btnForceClose_Click(object sender, EventArgs e)
        {
            if (txtCartonQty.Text == "0" || txtCartonQty.Text == "N/A")
            {
                SetFocus(txtInput);
                return;
            }
            else
            {
                if (Close_Carton(txtCartonNo.Text.Trim(), "Y"))
                {
                    int qty = 0;
                    for (int i = 0; i < dgvCartonContent.Rows.Count; i++)
                    {
                        qty += Convert.ToInt32(dgvCartonContent.Rows[i].Cells["QTY"].Value.ToString());
                    }
                    dgvCartonList.Rows.Insert(0, new object[] { txtPartNo.Text, txtWO.Text, txtCartonNo.Text, qty.ToString(), txtLotNo.Text, g_sLevel });
                    ClearForm();
                    bFirstPackFlag = true;
                    Show_Message("Force Close Carton OK", MSGType.OK);
                }
            }
        }

        private bool Close_Carton(string sCarton, string sForceClose)
        {
            try
            {
                if (string.IsNullOrEmpty(sCarton) || sCarton == "N/A")
                {
                    Show_Message("Carton Is Empty", MSGType.Error);
                    return false;
                }

                if (txtCartonQty.Text.Trim() == "0")
                {
                    Show_Message("Carton Qty Is 0", MSGType.Error);
                    return false;
                }

                string cmd = "select * from sajet.g_rc_carton where carton_no='" + sCarton + "' and Rownum=1 ";
                DataTable dt = ClientUtils.ExecuteSQL(cmd).Tables[0];
                int SNCount = dt.Rows.Count;
                string sRemainder_RC = string.Empty;
                int iRemainder_QTY = 0;

                if (SNCount == 0)//没有此Carton包装记录
                {
                    Show_Message("No This Carton Record", MSGType.Error);
                    return false;
                }

                if (PackConfig.PackAction == PACKACTION.RCToCarton)
                {
                    object[][] Params = new object[5][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "I_CARTON", sCarton };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "I_EMPID", g_sUserID };
                    Params[2] = new object[] { ParameterDirection.Output, OracleType.VarChar, "O_RES", string.Empty };
                    Params[3] = new object[] { ParameterDirection.Output, OracleType.VarChar, "O_REMAINDER_RC_NO", string.Empty };
                    Params[4] = new object[] { ParameterDirection.Output, OracleType.Number, "O_REMAINDER_QTY", 0 };
                    DataSet dsTemp = ClientUtils.ExecuteProc("SAJET.SJ_RC_PACK_CARTON_RCTRAVEL", Params);

                    if (dsTemp.Tables[0].Rows[0]["O_RES"].ToString() != "OK")
                    {
                        Show_Message(dsTemp.Tables[0].Rows[0]["O_RES"].ToString(), MSGType.Error);
                        return false;
                    }
                    else
                    {
                        if (!(string.IsNullOrEmpty(dsTemp.Tables[0].Rows[0]["O_REMAINDER_RC_NO"].ToString()) || dsTemp.Tables[0].Rows[0]["O_REMAINDER_RC_NO"].ToString().ToLower() == "null"))
                        {
                            sRemainder_RC = dsTemp.Tables[0].Rows[0]["O_REMAINDER_RC_NO"].ToString();
                            iRemainder_QTY = int.Parse(dsTemp.Tables[0].Rows[0]["O_REMAINDER_QTY"].ToString());
                        }
                    }
                }

                //关箱
                cmd = "UPDATE SAJET.G_RC_CARTON SET CLOSECARTON_EMP_ID = '" + g_sUserID + "', CLOSECARTON_TIME = SYSDATE WHERE CARTON_NO = '" + sCarton + "'";
                ClientUtils.ExecuteSQL(cmd);

                cmd = " UPDATE SAJET.G_PACK_CARTON "
                     + " SET CLOSE_FLAG = 'Y',CLOSE_TIME=sysdate,CLOSE_EMP_ID='" + g_sUserID + "'"
                     + " WHERE CARTON_NO = '" + sCarton + "' AND CLOSE_FLAG='N' ";
                ClientUtils.ExecuteSQL(cmd);
                if (sForceClose == "Y")
                {
                    cmd = " INSERT INTO SAJET.G_PACK_FORCECLOSE "
                         + " (PACK_NO, PACK_TYPE, EMP_ID,UPDATE_TIME) "
                         + " VALUES "
                         + " ('" + sCarton + "', 'Carton', '" + g_sUserID + "',SYSDATE)";
                    ClientUtils.ExecuteSQL(cmd);
                }

                string pPrintGoFile = Application.StartupPath + "\\" + ClientUtils.fCurrentProject + "\\" + "PrintGo_Total.bat";
                string command = "";

                txtCartonQty.Text = "0";
                Show_Message(SajetCommon.SetLanguage("Close Carton") + " [" + sCarton + "] !", MSGType.OK);

                if (PackConfig.PrintLabel)
                {
                    string command_carton = PrintLabel_close(sCarton);
                    command += command_carton + Environment.NewLine;
                }

                if (PackConfig.PackAction == PACKACTION.RCToCarton && !string.IsNullOrEmpty(sRemainder_RC))
                {
                    fPrintNewRC f = new fPrintNewRC(sRemainder_RC, iRemainder_QTY);
                    if (f.ShowDialog() == DialogResult.Yes)
                    {
                        string command_rc = PrintLabel_RC(sRemainder_RC);
                        command += command_rc;
                    }
                    f.Dispose();

                    dgvRemainder.Rows.Add(new object[] { sRemainder_RC, iRemainder_QTY.ToString(), SajetCommon.SetLanguage("Input Data"), SajetCommon.SetLanguage("RePrint") });
                }

                if (File.Exists(pPrintGoFile))
                    File.Delete(pPrintGoFile);
                File.AppendAllText(pPrintGoFile, command, Encoding.Default);
                PrintLabelByBarTender.WinExec(pPrintGoFile, 0);

                return true;
            }
            catch (Exception ex)
            {
                Show_Message(SajetCommon.SetLanguage("Close Carton Error:") + ex.Message, MSGType.Error);
                return false;
            }
        }

        /// <summary>
        /// 执行包装作业
        /// </summary>
        /// <returns></returns>
        public bool PackingExecute()
        {
            try
            {
                object[][] Params = new object[7][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "I_ACTION", PackConfig.PackAction == PACKACTION.BoxToCarton ? 1 : 0 };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "I_INPUT", txtInput.Text };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "I_CARTON", txtCartonNo.Text };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "I_EMPID", g_sUserID };
                Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "I_LOTNO", txtLotNo.Text };
                Params[5] = new object[] { ParameterDirection.Output, OracleType.VarChar, "O_RES", "" };
                Params[6] = new object[] { ParameterDirection.Output, OracleType.VarChar, "O_FLAG", "" };

                DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_RC_PACK_CARTON", Params);
                string sRes = ds.Tables[0].Rows[0]["O_RES"].ToString().ToUpper();
                if (sRes != "OK")
                {
                    Show_Message(sRes, MSGType.Error);
                    return false;
                }
                else
                {
                    RefreshCarton();
                    //关箱
                    if (ds.Tables[0].Rows[0]["O_FLAG"].ToString().ToUpper() == "Y")
                    {
                        //打印标签
                        if (PackConfig.PrintLabel)
                        {
                            PrintLabel(txtCartonNo.Text);
                        }

                        //生成装箱清单excel
                        //CreatePackList(txtCartonNo.Text);

                        dgvCartonList.Rows.Insert(0,
                            new object[] { txtPartNo.Text, txtWO.Text, txtCartonNo.Text, txtCartonQty.Text, txtLotNo.Text, g_sLevel });
                        gbCartonList.Text = SajetCommon.SetLanguage("Carton List:") + dgvCartonList.Rows.Count.ToString();

                        ClearForm();
                        bFirstPackFlag = true;
                    }
                    Show_Message("RC Pack Carton OK", MSGType.OK);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Show_Message("Pack Carton Error:" + ex.Message, MSGType.Error);
                return false;
            }
        }

        /// <summary>
        /// 装箱清单
        /// </summary>
        /// <param name="sCartonNo"></param>
        private void CreatePackList(string sCartonNo)
        {
            try
            {
                if (dgvCartonContent.Rows.Count == 0)
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Create Pack List Error"), 0);
                    return;
                }

                string sPath = PackConfig.sOutListPath;

                //如果路径不存在,则创建
                if (!Directory.Exists(sPath))
                {
                    Directory.CreateDirectory(sPath);
                }

                //获取装箱单模版
                string sSampleFile = "";
                if (!GetExcel(ref sSampleFile))
                    return;

                //alisa    20170323  装箱清单显示厚度和BOW
                string sql = @"select BOX_NO,PART_NO,QTY,THICKNESS,BOW,START_NO,END_NO,CARTON_QTY
                                       from (SELECT A2.BOX_NO,C.PART_NO,A2.QTY,E.THICKNESS,E.BOW,B.LEVEL_CODE,A2.CREATECARTON_TIME,
                                                 MIN(D.LASER_NO) START_NO,MAX(D.LASER_NO) END_NO,A1.CARTON_QTY
                                               FROM (SELECT CARTON_NO, SUM(QTY) CARTON_QTY
                                                          FROM SAJET.G_RC_CARTON WHERE CARTON_NO = '" + txtCartonNo.Text + @"'  GROUP BY CARTON_NO) A1,
                                                        SAJET.G_RC_CARTON A2,SAJET.G_PACK_CARTON B,SAJET.SYS_PART C,SAJET.G_SN_STATUS D,
                                                        SAJET.G_PART_THICKNESS_LEVEL E
                                                        WHERE A1.CARTON_NO = A2.CARTON_NO AND A1.CARTON_NO = B.CARTON_NO
                                                                   AND A2.PART_ID = C.PART_ID AND A2.RC_NO = D.RC_NO(+)
                                                                   AND C.PART_ID = E.PART_ID   AND B.LEVEL_CODE = E.LEVEL_CODE
                                                       GROUP BY A2.BOX_NO,A1.CARTON_QTY,A2.QTY,C.PART_NO, E.THICKNESS,E.BOW,
                                                                  B.LEVEL_CODE,A2.CREATECARTON_TIME
                                                      ORDER BY A2.CREATECARTON_TIME ASC)";
                DataSet dt = ClientUtils.ExecuteSQL(sql);
                ExcelEdit ExcelClass = new ExcelEdit();
                ExcelClass.Open(sSampleFile);
                ExcelClass.SetCellValue("CartonNo", 2, 5, sCartonNo);
                ExcelClass.SetCellValue("CartonNo", 2, 6, dgvCartonContent.Rows[0].Cells["carton_qty"].Value.ToString());
                //for (int i = 0; i < dgvCartonContent.Rows.Count; i++)
                //{
                //    //箱子总量不需要
                //    for (int j = 0; j < dgvCartonContent.Columns.Count - 1; j++)
                //    {
                //        ExcelClass.SetCellValue("CartonNo", j + 2, i + 10, dgvCartonContent.Rows[i].Cells[j].Value.ToString());
                //    }
                //}
                for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                {
                    //箱子总量不需要
                    for (int j = 0; j < dt.Tables[0].Columns.Count - 1; j++)
                    {
                        ExcelClass.SetCellValue("CartonNo", j + 2, i + 10, dt.Tables[0].Rows[i][j].ToString());
                    }
                }

                ExcelClass.SaveAs(sPath + "\\" + sCartonNo + ".xlsx");
                ExcelClass.Close();

            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Create Pack List Error") + ":" + ex.Message, 0);
            }
        }

        private bool GetExcel(ref string sSampleFile)
        {
            sSampleFile = System.Windows.Forms.Application.StartupPath + "\\" + g_sExeName + "\\CartonNo.xlt";
            if (!File.Exists(sSampleFile))
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Get Excel Sample File Error") + " ( " + sSampleFile + " )", 0);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 打印箱号标签
        /// </summary>
        /// <param name="sCartonNo"></param>
        private void PrintLabel(string sCartonNo)
        {
            try
            {
                listbPrint.Items.Clear();
                string sLabelName = "";
                string sRCs = string.Empty;
                if (g_sModelType.ToUpper() == "有LOGO")
                    sLabelName = "PACKING_CARTON_Y";
                else
                    sLabelName = "PACKING_CARTON_N";

                int qty = 0;
                for (int i = 0; i < dgvCartonContent.Rows.Count; i++)
                {
                    qty += Convert.ToInt32(dgvCartonContent.Rows[i].Cells["QTY"].Value.ToString());
                    sRCs += string.IsNullOrEmpty(sRCs) ? dgvCartonContent.Rows[i].Cells["BOX_NO"].Value.ToString() : ";" + dgvCartonContent.Rows[i].Cells["BOX_NO"].Value.ToString();
                }

                string sSeq = "";
                string sDate = DateTime.Now.ToString("yyyy-MM-dd");

                #region Alisa    20170322    打印时显示厚度和BOW 
                //string sThickness = "";
                //string sBOW = "";
                //string sBT = @"select a.thickness,a.bow 
                //                    from sajet.g_part_thickness_level a,sajet.sys_part b
                //                    where a.part_id=b.part_id and b.part_no='" + txtPartNo.Text + "' and  a.level_code='" + g_sLevel + "' and a.enabled='Y'";
                //DataSet dBT = ClientUtils.ExecuteSQL(sBT);
                //sThickness = dBT.Tables[0].Rows[0]["THICKNESS"].ToString();
                //sBOW = dBT.Tables[0].Rows[0]["BOW"].ToString();
                #endregion
                //string sTemp = sCartonNo + "," + txtPartNo.Text + "," + g_sCustomer + "," + txtCartonCapacity.Text
                //    + "," + sSeq + "," + sThickness+","+sBOW + "," + sDate;
                string sTemp = txtWO.Text + "," + sRCs + "," + txtPartNo.Text + "," + sCartonNo + "," + qty.ToString() + "," + g_sSPEC1;

                listbPrint.Items.Add(sTemp);

                string printCommand = PrintLabelByBarTender.Print_Label(listbPrint, sLabelName, g_sProgram);
                if (printCommand == "")
                {
                    Show_Message(SajetCommon.SetLanguage("Print RC_NO Fail"), MSGType.Error);
                }
                else
                {
                    string pPrintGoFile = Application.StartupPath + "\\" + ClientUtils.fCurrentProject + "\\" + "PrintGo_Carton.bat";
                    if (File.Exists(pPrintGoFile))
                        File.Delete(pPrintGoFile);
                    File.AppendAllText(pPrintGoFile, printCommand, Encoding.Default);
                    PrintLabelByBarTender.WinExec(pPrintGoFile, 0);
                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);
                return;
            }
        }

        void ClearForm()
        {
            txtInput.Enabled = false;
            txtInput.BackColor = Color.White;
            txtInput.Text = "";

            txtMsg.Text = "";
            txtMsg.BackColor = Color.White;

            bFirstPackFlag = true;
            txtWO.Text = "N/A";
            txtPartNo.Text = "N/A";
            g_sSPEC1 = string.Empty;
            txtCartonNo.Text = "N/A";
            txtCartonQty.Text = "0";
            txtWOQty.Text = "0";
            txtInput.Text = "";

            txtWOQty.Text = "0";
            dgvCartonContent.DataSource = null;
            SetFocus(txtInput);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            bFirstPackFlag = true;
            txtWO.Text = "N/A";
            txtPartNo.Text = "N/A";
            g_sSPEC1 = string.Empty;
            txtCartonNo.Text = "N/A";
            txtCartonQty.Text = "0";
            txtWOQty.Text = "0";
            txtInput.Text = "";

            txtWOQty.Text = "0";
            dgvCartonContent.DataSource = null;
            SetFocus(txtInput);
        }

        private void dgvCartonList_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCartonList.Rows.Count == 0 || dgvCartonList.CurrentRow == null || dgvCartonList.SelectedCells.Count == 0)
                return;
            ShowDetailData();
            dgvCartonList.Focus();
        }

        private void ShowDetailData()
        {
            dgvCartonContent.DataSource = null;
            string CartonNo = "";
            CartonNo = dgvCartonList.Rows[dgvCartonList.CurrentRow.Index].Cells[2].Value.ToString();
            if (string.IsNullOrEmpty(CartonNo) || CartonNo.Equals("N/A"))
            {
                Show_Message("Carton No Is Empty", MSGType.Error);
            }
            try
            {
                if (PackConfig.PackAction == PACKACTION.RCToCarton)
                {
                    string cmd = @"select s.carton_no,s.rc_no,s.qty from sajet.g_rc_carton s "
                        + "where s.carton_no='" + CartonNo + "'";

                    DataTable dt = ClientUtils.ExecuteSQL(cmd).Tables[0];
                    dgvCartonContent.DataSource = dt;
                    dgvCartonContent.Columns["CARTON_NO"].HeaderText = SajetCommon.SetLanguage("CARTON_NO");
                    dgvCartonContent.Columns["RC_NO"].HeaderText = SajetCommon.SetLanguage("RC_NO");
                    dgvCartonContent.Columns["QTY"].HeaderText = SajetCommon.SetLanguage("QTY");
                }
            }
            catch (Exception ex)
            {
                Show_Message(SajetCommon.SetLanguage("Refresh Carton Error:") + ex.Message, MSGType.Error);
            }
        }

        private void dgvRemainder_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvRemainder.Columns.IndexOf(dgvRemainder.Columns["Input"]))
            {
                txtInput.Text = dgvRemainder.Rows[e.RowIndex].Cells["RC_NO"].Value.ToString();
                txtInput_KeyPress(sender, new KeyPressEventArgs((char)Keys.Return));
            }
            else if (e.RowIndex >= 0 && e.ColumnIndex == dgvRemainder.Columns.IndexOf(dgvRemainder.Columns["Reprint"]))
            {
                string command = PrintLabel_RC(dgvRemainder.Rows[e.RowIndex].Cells["RC_NO"].Value.ToString());
                string pPrintGoFile = Application.StartupPath + "\\" + ClientUtils.fCurrentProject + "\\" + "PrintGo_RC.bat";
                if (File.Exists(pPrintGoFile))
                    File.Delete(pPrintGoFile);
                File.AppendAllText(pPrintGoFile, command, Encoding.Default);
                PrintLabelByBarTender.WinExec(pPrintGoFile, 0);
            }
        }

        /// <summary>
        /// 显示被装箱的RC及Cassette列表
        /// </summary>
        //private void ShowCartonContent()
        //{
        //    string cmd = @"select s.carton_no,s.rc_no,s.box_no, s.qty from sajet.g_rc_carton s "
        //           + "where s.carton_no='" + txtCartonNo.Text + "'";

        //    DataTable dt = ClientUtils.ExecuteSQL(cmd).Tables[0];
        //    dgvCartonContent.DataSource = dt;

        //    string str = "";
        //    for (int i = 0; i <= dgvCartonContent.Columns.Count - 1; i++)
        //    {
        //        str = dgvCartonContent.Columns[i].HeaderCell.Value.ToString();
        //        dgvCartonContent.Columns[i].HeaderText = SajetCommon.SetLanguage(str, 1);
        //    }
        //}

        private void btnReprint_Click(object sender, EventArgs e)
        {
            listbPrint.Items.Clear();
            string sLabel = "";
            fReprint fp = new fReprint();
            if (fp.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string sCarton = fp.txtCarton.Text;
                    //string sSQL = @"select a1.carton_no,
                    //                       a1.part_no,
                    //                       a1.level_code,
                    //                       a3.thickness,
                    //                       a3.bow,
                    //                       a1.lot_seq,
                    //                       a1.option12,
                    //                       a2.qty,
                    //                       a1.cust_part_no
                    //                  from (select a.carton_no, a.level_code, a.lot_seq, b.part_id,b.part_no, b.option12, b.cust_part_no
                    //                          from sajet.g_pack_carton a, sajet.sys_part b
                    //                         where a.part_id = b.part_id
                    //                           and carton_no = '" + sCarton + @"'
                    //                           and close_flag = 'Y') a1,
                    //                       (select sum(qty) qty, carton_no
                    //                          from sajet.g_rc_carton
                    //                         where carton_no = '" + sCarton + @"'
                    //                         group by carton_no) a2,
                    //                     sajet.g_part_thickness_level a3
                    //                 where a1.carton_no = a2.carton_no and a1.part_id=a3.part_id";
                    string sSQL = @"select a1.work_order,
                                           a1.carton_no,
                                           a1.part_no,
                                           a1.spec1,
                                           a1.level_code,
                                           a1.lot_seq,
                                           a1.option12,
                                           a2.qty,
                                           a1.cust_part_no
                                      from (select a.work_order, a.carton_no, a.level_code, a.lot_seq, b.part_id,b.part_no, b.spec1, b.option12, b.cust_part_no
                                              from sajet.g_pack_carton a, sajet.sys_part b
                                             where a.part_id = b.part_id
                                               and carton_no = '" + sCarton + @"'
                                               and close_flag = 'Y') a1,
                                           (select sum(qty) qty, carton_no
                                              from sajet.g_rc_carton
                                             where carton_no = '" + sCarton + @"'
                                             group by carton_no) a2
                                     where a1.carton_no = a2.carton_no";
                    DataTable dtTemp = ClientUtils.ExecuteSQL(sSQL).Tables[0];
                    if (dtTemp.Rows.Count > 0)
                    {
                        if (dtTemp.Rows[0]["option12"].ToString().ToUpper() == "有LOGO")
                            sLabel = "PACKING_CARTON_Y";
                        else
                            sLabel = "PACKING_CARTON_N";

                        string sRCs = string.Empty;
                        sSQL = "SELECT RC_NO FROM SAJET.G_RC_CARTON WHERE CARTON_NO = '" + sCarton + "'";
                        DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
                        if (dsTemp.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                            {
                                sRCs += string.IsNullOrEmpty(sRCs) ? dsTemp.Tables[0].Rows[i]["RC_NO"].ToString() : ";" + dsTemp.Tables[0].Rows[i]["RC_NO"].ToString();
                            }
                        }

                        //string sTemp = sCarton + "," + dtTemp.Rows[0]["part_no"].ToString() + ","
                        //                  + dtTemp.Rows[0]["cust_part_no"].ToString() + "," + dtTemp.Rows[0]["qty"].ToString() + ","
                        //                  + "" + "," + dtTemp.Rows[0]["thickness"].ToString() + ","
                        //                  + dtTemp.Rows[0]["bow"].ToString() + ","+ DateTime.Now.ToString("yyyy-MM-dd");

                        string sTemp = dtTemp.Rows[0]["WORK_ORDER"].ToString() + "," + sRCs + "," + dtTemp.Rows[0]["part_no"].ToString() + "," + sCarton + "," + dtTemp.Rows[0]["qty"].ToString() + "," + dtTemp.Rows[0]["spec1"].ToString();

                        listbPrint.Items.Add(sTemp);

                        string printCommand = PrintLabelByBarTender.Print_Label(listbPrint, sLabel, g_sProgram);
                        if (printCommand == "")
                        {
                            Show_Message(SajetCommon.SetLanguage("Print RC_NO Fail"), MSGType.Error);
                        }
                        else
                        {
                            string pPrintGoFile = Application.StartupPath + "\\" + ClientUtils.fCurrentProject + "\\" + "PrintGo_Carton.bat";
                            if (File.Exists(pPrintGoFile))
                                File.Delete(pPrintGoFile);
                            File.AppendAllText(pPrintGoFile, printCommand, Encoding.Default);
                            PrintLabelByBarTender.WinExec(pPrintGoFile, 0);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Show_Message(SajetCommon.SetLanguage("Reprint Carton Error:") + ex.Message, MSGType.Error);
                }
            }
        }

        private void cbPrintLabel_CheckedChanged(object sender, EventArgs e)
        {
            PackConfig.PrintLabel = cbPrintLabel.Checked;
        }

        private void btnEndLot_Click(object sender, EventArgs e)
        {
            try
            {
                string sSQL = "update sajet.g_pack_carton_lotno "
                                + "     set end_flag='Y', emp_id = '" + g_sUserID + "' "
                                + " where lot_no = '" + txtLotNo.Text + "'";
                ClientUtils.ExecuteSQL(sSQL);
                GetLotNo();
            }
            catch
            {
            }
        }



        /// <summary>
        /// 强制关闭时打印箱号标签
        /// </summary>
        /// <param name="sCartonNo"></param>
        private string PrintLabel_close(string sCartonNo)
        {
            listbPrint.Items.Clear();
            string sRCs = string.Empty;

            string sLabelName = "";
            if (g_sModelType.ToUpper() == "有LOGO")
                sLabelName = "PACKING_CARTON_Y";
            else
                sLabelName = "PACKING_CARTON_N";

            int qty = 0;
            for (int i = 0; i < dgvCartonContent.Rows.Count; i++)
            {
                qty += Convert.ToInt32(dgvCartonContent.Rows[i].Cells["QTY"].Value.ToString());
                sRCs += string.IsNullOrEmpty(sRCs) ? dgvCartonContent.Rows[i].Cells["BOX_NO"].Value.ToString() : ";" + dgvCartonContent.Rows[i].Cells["BOX_NO"].Value.ToString();
            }

            #region Alisa    20170322    打印时显示厚度和BOW
            //string sThickness = "";
            //string sBOW = "";
            //string sBT = @"select a.thickness,a.bow 
            //                        from sajet.g_part_thickness_level a,sajet.sys_part b
            //                        where a.part_id=b.part_id and b.part_no='" + txtPartNo.Text + "' and  a.level_code='"+g_sLevel+"'  and a.enabled='Y'";
            //DataSet dBT = ClientUtils.ExecuteSQL(sBT);
            //sThickness = dBT.Tables[0].Rows[0]["THICKNESS"].ToString();
            //sBOW = dBT.Tables[0].Rows[0]["BOW"].ToString();
            #endregion

            string sSeq = "";
            string sDate = DateTime.Now.ToString("yyyy-MM-dd");
            //string sTemp = sCartonNo + "," + txtPartNo.Text + "," + g_sCustomer + "," + qty
            //    + "," + sSeq + "," + sThickness+","+sBOW+"," + sDate;

            string sTemp = txtWO.Text + "," + sRCs + "," + txtPartNo.Text + "," + sCartonNo + "," + qty.ToString() + "," + g_sSPEC1;

            listbPrint.Items.Add(sTemp);
            string printCommand = PrintLabelByBarTender.Print_Label(listbPrint, sLabelName, g_sProgram);
            return printCommand;
        }

        /// <summary>
        /// 列印 RC 標籤
        /// </summary>
        /// <param name="RC_NO"></param>
        private string PrintLabel_RC(string RC_NO)
        {
            try
            {
                // 取得列印參數
                PrintLabel.Setup PrintLabelDll = new PrintLabel.Setup();

                // 根據途程有幾個製程選擇標籤樣板
                string SQL = "SELECT COUNT(*) COUNT FROM SAJET.G_RC_STATUS A,SAJET.SYS_RC_ROUTE_DETAIL B WHERE RC_NO = '" + RC_NO + "' AND A.ROUTE_ID = B.ROUTE_ID ";
                DataSet dsCount = ClientUtils.ExecuteSQL(SQL);
                string sLabelName = "BR_DEFAULT";
                if (Convert.ToInt32(dsCount.Tables[0].Rows[0]["COUNT"].ToString()) > 10)
                {
                    sLabelName = "BR_DEFAULT";
                }
                else
                {
                    sLabelName = "BR_DEFAULT_10";
                }

                string sType = "BC_RC No";
                ListBox ListParam = new ListBox();
                ListBox ListData = new ListBox();
                ListParam.Items.Clear();
                ListData.Items.Add(RC_NO);
                PrintLabelDll.GetPrintData(sType, ref ListParam, ref ListData);

                string sStartPath = Application.StartupPath;
                string sExeName = ClientUtils.fCurrentProject;
                string sLabelFieldFile = sStartPath + "\\" + "BarcodeCenter\\BR_DEFAULT.dat";
                string sMessage = "";
                string sSplitValue = ",";
                // 讀取登錄在 .dat 的參數欄位 
                ListBox listField = LoadFileHeader(sLabelFieldFile, ref sMessage, sSplitValue);

                // 產生 .lst 文件的方法
                string sData = string.Empty;
                for (int j = 0; j <= listField.Items.Count - 1; j++)
                {
                    string sField = listField.Items[j].ToString();
                    int iIndex = ListParam.Items.IndexOf(sField);
                    if (iIndex >= 0)
                    {
                        if (sSplitValue == "1")
                        {
                            sData = sData + ListData.Items[iIndex].ToString() + (char)Keys.Tab;
                        }
                        else
                        {
                            sData = sData + ListData.Items[iIndex].ToString() + sSplitValue;
                        }
                    }
                    else
                    {
                        if (sSplitValue == "1")
                        {
                            sData += (char)Keys.Tab;
                        }
                        else
                        {
                            sData += sSplitValue;//若找不到則給空值
                        }
                    }
                }
                if (!string.IsNullOrEmpty(sData))
                {
                    if (sSplitValue == "1")
                        sData = sData.TrimEnd((char)Keys.Tab);
                    else
                        sData = sData.Substring(0, sData.Length - 1);
                }

                listbPrint.Items.Clear();
                listbPrint.Items.Add(sData);
                // 取得列印參數

                string printCommand = PrintLabelByBarTender.Print_Label(listbPrint, sLabelName, g_sProgram);
                return printCommand;
            }
            catch (Exception ex)
            {
                Show_Message(SajetCommon.SetLanguage("Print RC_NO Error:") + ex.Message, MSGType.Error);
                return "";
            }
        }

        /// <summary>
        /// 获取包箱形式   Alisa 20170110
        /// </summary>
        public void GetPackType()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Value");
            //dt.Rows.Add("");
            //dt.Rows.Add("BOX->CARTON");
            dt.Rows.Add("RC->CARTON");
            cmbtype.DisplayMember = "Value";
            cmbtype.ValueMember = "Value";
            cmbtype.DataSource = dt;
        }

        private ListBox LoadFileHeader(string sFile, ref string sMessage, string sSplitType)
        {
            sMessage = string.Empty;
            ListBox ListParams = new ListBox();
            if (!File.Exists(sFile))
            {
                sMessage = "File not exist - " + sFile;
            }
            else
            {
                StreamReader sr = new StreamReader(sFile);
                try
                {
                    char keySplit = (char)Keys.Tab;
                    char[] sKeyValue = sSplitType.ToCharArray();
                    string[] sValue;
                    if (sSplitType == "1")
                    {
                        sValue = sr.ReadLine().Trim().Split(new Char[] { keySplit });
                    }
                    else
                    {
                        sValue = sr.ReadLine().Trim().Split(new Char[] { sKeyValue[0] });
                    }

                    for (int i = 0; i <= sValue.Length - 1; i++)
                    {
                        ListParams.Items.Add(sValue[i].ToString());
                    }
                }
                finally
                {
                    sr.Close();
                }
            }
            return ListParams;
        }
    }
}
