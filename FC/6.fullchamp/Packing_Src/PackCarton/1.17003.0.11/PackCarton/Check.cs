using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace LabelCheck
{
    public class Check
    {
        string sSQL;
        DataSet dsTemp;
        DataSet dsWoData;
        public bool Get_RuleData(string sLabelType, string sWO, ref string[] sParam, ref object[] objRuleData)
        {
            //===找工單編碼規則內容====
            string sCode = "";
            string sDefault = "";
            string sCarry = "";
            string sCheckSum = "";
            string sResetSeqCycle = "";
            string sResetSeq = "N";
            ListView LVUserDay = new ListView();
            ListView LVUserSeq = new ListView();
            ListView LVFun = new ListView();

            sSQL = " Select * From SAJET.G_WO_PARAM "
                 + " Where WORK_ORDER = '" + sWO + "' "
                 + " and MODULE_NAME = '" + sLabelType.ToUpper() + " RULE'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                //1.0.0.8 add - 因為Packing 傳CSN,但DB中設定是填Customer SN,因此特別判斷
                if (sLabelType.ToUpper() == "CSN")
                {
                    sSQL = " Select * From SAJET.G_WO_PARAM "
                         + " Where WORK_ORDER = '" + sWO + "' "
                         + " and MODULE_NAME = 'CUSTOMER SN RULE'";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);
                    if (dsTemp.Tables[0].Rows.Count == 0)
                        return false;
                    sLabelType = "Customer SN";
                }
                else
                    return false;
            }
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                string sParam_Name = dsTemp.Tables[0].Rows[i]["PARAME_NAME"].ToString();
                string sParam_Item = dsTemp.Tables[0].Rows[i]["PARAME_ITEM"].ToString();
                string sParam_Value = dsTemp.Tables[0].Rows[i]["PARAME_VALUE"].ToString();
                //編碼規則與Default值
                if (sParam_Name == sLabelType + " Code")
                {
                    if (sParam_Item == "Code")
                        sCode = sParam_Value;
                    else if (sParam_Item == "Default")
                        sDefault = sParam_Value;
                    else if (sParam_Item == "Code Type")
                        sCarry = sParam_Value;
                    continue;
                }
                //自行定義的日期代碼
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
                            sResetSeq = "N"; //不需Reset
                        sResetSeqCycle = sParam_Value;
                        continue;
                }
                //自行定義的Function                
                if (sParam_Name.IndexOf("Digit Type & Field") != -1)
                {
                    LVFun.Items.Add(sParam_Name.Substring(0, 1));
                    LVFun.Items[LVFun.Items.Count - 1].SubItems.Add(sParam_Item);
                    LVFun.Items[LVFun.Items.Count - 1].SubItems.Add(sParam_Value);
                    //取得Function傳出來的值
                    GetWoData(sWO);
                    string sData = "N/A";
                    if (sParam_Item != "N/A")
                        sData = sData = dsWoData.Tables[0].Rows[0][sParam_Item].ToString();
                    sSQL = " select " + sParam_Value + "('" + sData + "') fundata from dual ";
                    DataSet ds = ClientUtils.ExecuteSQL(sSQL);
                    string sValue = ds.Tables[0].Rows[0]["fundata"].ToString();
                    LVFun.Items[LVFun.Items.Count - 1].SubItems.Add(sValue);

                    continue;
                }
                //自行定義的流水號進位方式
                if (sParam_Name == sLabelType + " User Define")
                {
                    LVUserSeq.Items.Add(sParam_Item);
                    LVUserSeq.Items[LVUserSeq.Items.Count - 1].SubItems.Add(sParam_Value);
                    continue;
                }
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
            return true;
        }

        public void GetWoData(string sWo)
        {
            //工單資料
            sSQL = "Select A.*,B.*,C.ROUTE_NAME,D.PDLINE_NAME,E.CUSTOMER_CODE,E.CUSTOMER_NAME,F.PROCESS_NAME START_PROCESS,G.PROCESS_NAME END_PROCESS "
                 + " from SAJET.G_WO_BASE A "
                 + " left join SAJET.SYS_PART B on A.PART_ID = B.PART_ID "
                 + " left join SAJET.SYS_ROUTE C on A.ROUTE_ID = C.ROUTE_ID "
                 + " left join SAJET.SYS_PDLINE D on A.DEFAULT_PDLINE_ID = D.PDLINE_ID "
                 + " left join SAJET.SYS_CUSTOMER E on A.CUSTOMER_ID = E.CUSTOMER_ID "
                 + " left join SAJET.SYS_PROCESS F on A.START_PROCESS_ID = F.PROCESS_ID "
                 + " left join SAJET.SYS_PROCESS G on A.END_PROCESS_ID = G.PROCESS_ID "
                 + " WHERE A.WORK_ORDER='" + sWo + "' ";
            dsWoData = ClientUtils.ExecuteSQL(sSQL);
        }

        public string CheckRule_NewNo(string sInputNo, string[] sParam, object[] objData, bool bCheckDef)
        {
            //====檢查是否符合編碼規則====
            /* sInputNo:產生的號碼; bCheckDef:是否要強制日期要與Default相同 */
            ListView LVDayCode = new ListView();
            ListView LVDefSeq = new ListView();
            ListView LVFun = new ListView();

            string sDefault = objData[Array.IndexOf(sParam, "Default")].ToString();
            string sCode = objData[Array.IndexOf(sParam, "Code")].ToString(); //編碼規則
            string sCarry = objData[Array.IndexOf(sParam, "Code Type")].ToString(); //10 or 16進位
            LVDayCode = (ListView)objData[Array.IndexOf(sParam, "User DayCode")]; //自定義的DayCode
            LVDefSeq = (ListView)objData[Array.IndexOf(sParam, "User Seq")]; //User Define進位方式
            LVFun = (ListView)objData[Array.IndexOf(sParam, "User Function")];


            //找自定義的DayCode
            string sCarryM = ""; string sCarryD = ""; string sCarryW = ""; string sCarryK = ""; ;
            for (int j = 0; j <= LVDayCode.Items.Count - 1; j++)
            {
                switch (LVDayCode.Items[j].Text)
                {
                    case "m":
                        sCarryM = LVDayCode.Items[j].SubItems[1].Text;
                        continue;
                    case "d":
                        sCarryD = LVDayCode.Items[j].SubItems[1].Text;
                        continue;
                    case "w":
                        sCarryW = LVDayCode.Items[j].SubItems[1].Text;
                        continue;
                    case "k":
                        sCarryK = LVDayCode.Items[j].SubItems[1].Text;
                        continue;
                }
            }

            //檢查長度
            if (sCode.Length != sInputNo.Length)
            {
                return "Rule not match (Length:" + sCode.Length.ToString() + ")";
            }
            //檢查固定碼
            string sData = ""; int iR;
            string sM = "", sD = "", sW = "", sF = "";
            string uM = "", uD = "", uW = "", uK = "";

            string sMDef = "", sDDef = "", sWDef = "", sFDef = "";
            string uMDef = "", uDDef = "", uWDef = "", uKDef = "";

            //檢查Function
            string uF = "";
            string uFDef = "";

            for (int i = 0; i <= sCode.Length - 1; i++)
            {
                switch (sCode[i].ToString())
                {
                    case "L":
                        if (sDefault[i].ToString() != " " && sDefault[i].ToString() != sInputNo[i].ToString())
                            return "Rule not match (Fix Character)";
                        continue;
                    case "C":
                        if (sDefault[i].ToString() != " " && sDefault[i].ToString() != sInputNo[i].ToString())
                            return "Rule not match (Fix Character)";
                        continue;
                    case "9":
                        if (sDefault[i].ToString() != " " && sDefault[i].ToString() != sInputNo[i].ToString())
                            return "Rule not match (Fix Character)";
                        continue;
                    case "Y":
                        sData = "0123456789";
                        if (sData.IndexOf(sInputNo[i].ToString()) == -1)
                            return "Rule not match (Year)";
                        continue;
                    case "K": //Day of Week
                        sData = "1234567";
                        if (sData.IndexOf(sInputNo[i].ToString()) == -1)
                            return "Rule not match (Day of Week)";
                        continue;
                    case "S":
                        if (sCarry == "10")
                        {
                            sData = "0123456789";
                            if (sData.IndexOf(sInputNo[i].ToString()) == -1)
                                return "Rule not match (Sequence)";
                        }
                        else if (sCarry == "16")
                        {
                            sData = "0123456789ABCDEF";
                            if (sData.IndexOf(sInputNo[i].ToString()) == -1)
                                return "Rule not match (Sequence)";
                        }
                        continue;
                    case "M":
                        sM = sM + sInputNo[i].ToString();
                        sMDef = sMDef + sDefault[i].ToString(); //實際月份
                        continue;
                    case "D":
                        sD = sD + sInputNo[i].ToString();
                        sDDef = sDDef + sDefault[i].ToString();
                        continue;
                    case "W":
                        sW = sW + sInputNo[i].ToString();
                        sWDef = sWDef + sDefault[i].ToString();
                        continue;
                    case "F": //Day of Year
                        sF = sF + sInputNo[i].ToString();
                        sFDef = sFDef + sDefault[i].ToString();
                        continue;
                    case "m":
                        uM = uM + sInputNo[i].ToString();
                        uMDef = uMDef + sDefault[i].ToString();
                        continue;
                    case "d":
                        uD = uD + sInputNo[i].ToString();
                        uDDef = uDDef + sDefault[i].ToString();
                        continue;
                    case "w":
                        uW = uW + sInputNo[i].ToString();
                        uWDef = uWDef + sDefault[i].ToString();
                        continue;
                    case "k":
                        uK = uK + sInputNo[i].ToString();
                        uKDef = uKDef + sDefault[i].ToString();
                        continue;
                }

                //檢查自定義流水號
                if (LVDefSeq.Items.Count > 0)
                {
                    //int iIndex = LVDefSeq.FindItemWithText(sCode[i].ToString(), false, 0).Index;
                    ListViewItem Item = LVDefSeq.FindItemWithText(sCode[i].ToString(), false, 0);
                    int iIndex = LVDefSeq.Items.IndexOf(Item);
                    if (iIndex > -1)
                    {
                        string sDefNo = LVDefSeq.Items[iIndex].SubItems[1].Text;
                        if (sDefNo.IndexOf(sInputNo[i].ToString()) == -1)
                            return "Rule not match (User Define Sequence)";
                    }
                }
                //檢查自訂Function
                if (LVFun.Items.Count > 0)
                {
                    ListViewItem Item = LVFun.FindItemWithText(sCode[i].ToString(), false, 0);
                    int iIndex = LVFun.Items.IndexOf(Item);
                    if (iIndex != -1)
                    {
                        uF = uF + sInputNo[i].ToString();
                        uFDef = LVFun.Items[iIndex].SubItems[3].Text;
                        //if(sDefNo !=  sInputNo[i].ToString())
                        //return "Rule not match (User Define Functioon)";
                    }
                }
            }

            //自訂Function
            if (uF != "")
            {
                if (uF != uFDef)
                    return "Rule not match (User Define Functioon)";
            }

            //DayCode
            if (sM != "")
            {
                try { iR = Convert.ToInt32(sM); }
                catch { return "Rule not match (Month)"; }
                if (iR < 1 || iR > 12)
                    return "Rule not match (Month)";
                //是否要檢查年月日週與Default的值需相同
                if (bCheckDef && sM != sMDef)
                    return "Rule not match (Month:" + sM + "=" + sMDef + ")";
            }
            if (sD != "")
            {
                try { iR = Convert.ToInt32(sD); }
                catch { return "Rule not match (Day)"; }
                if (iR < 1 || iR > 31)
                    return "Rule not match (Day)";
                if (bCheckDef && sD != sDDef)
                    return "Rule not match (Day:" + sD + "=" + sDDef + ")";
            }
            if (sW != "")
            {
                try { iR = Convert.ToInt32(sW); }
                catch { return "Rule not match (Week)"; }
                if (iR < 1 || iR > 53)
                    return "Rule not match (Week)";
                if (bCheckDef && sW != sWDef)
                    return "Rule not match (Week:" + sW + "=" + sWDef + ")";
            }
            if (sF != "")
            {
                try { iR = Convert.ToInt32(sF); }
                catch { return "Rule not match (Day of Year)"; }
                if (iR < 1 || iR > 366)
                    return "Rule not match (Day of Year)";
                if (bCheckDef && sF != sFDef)
                    return "Rule not match (Day of Year:" + sF + "=" + sFDef + ")";
            }

            //User Define DayCode
            if (uM != "")
            {
                if (sCarryM.IndexOf(uM) == -1)
                    return "Rule not match (Month User Define)";
                if (bCheckDef && uM != uMDef)
                    return "Rule not match (Month User Define:" + uM + "=" + uMDef + ")";
            }
            if (uD != "")
            {
                if (sCarryD.IndexOf(uD) == -1)
                    return "Rule not match (Day User Define)";
                if (bCheckDef && uD != uDDef)
                    return "Rule not match (Day User Define:" + uD + "=" + uDDef + ")";
            }
            if (uW != "")
            {
                if (sCarryW.IndexOf(uW) == -1)
                    return "Rule not match (Week User Define)";
                if (bCheckDef && uW != uWDef)
                    return "Rule not match (Week User Define:" + uW + "=" + uWDef + ")";
            }
            if (uK != "")
            {
                if (sCarryK.IndexOf(uK) == -1)
                    return "Rule not match (Day of Week User Define)";
                if (bCheckDef && uK != uKDef)
                    return "Rule not match (Day of Week User Define:" + uK + "=" + uKDef + ")";
            }
            return "OK";
        }

        public bool Create_NewNo(out string sNewNo, string sSeqName, ref string sResetMark, string[] sParam, object[] objData)
        {
            //===系統產生新號碼===
            ListView LVDayCode = new ListView();
            ListView LVDefSeq = new ListView();
            ListView LVFun = new ListView();

            //先讀取傳進來的工單編碼規則
            string sDefault = objData[Array.IndexOf(sParam, "Default")].ToString();
            string sCode = objData[Array.IndexOf(sParam, "Code")].ToString(); //編碼規則
            string sCarry = objData[Array.IndexOf(sParam, "Code Type")].ToString(); //10 or 16進位
            LVDayCode = (ListView)objData[Array.IndexOf(sParam, "User DayCode")]; //自定義的DayCode
            LVDefSeq = (ListView)objData[Array.IndexOf(sParam, "User Seq")]; //User Define進位方式
            LVFun = (ListView)objData[Array.IndexOf(sParam, "User Function")];
            string sCheckSum = objData[Array.IndexOf(sParam, "Check Sum")].ToString();
            string sResetSeq = objData[Array.IndexOf(sParam, "Reset Sequence")].ToString();
            string sResetCycle = objData[Array.IndexOf(sParam, "Reset Cycle")].ToString();

            //讀取User Define的日期編碼
            string[] g_CarryM = { "" };
            string[] g_CarryD = { "" };
            string[] g_CarryW = { "" };
            string[] g_CarryDW = { "" };
            for (int i = 0; i <= LVDayCode.Items.Count - 1; i++)
            {
                string sParam_Value = LVDayCode.Items[i].SubItems[1].Text;
                switch (LVDayCode.Items[i].Text)
                {
                    //case "Month User Define":
                    case "m":
                        g_CarryM = sParam_Value.Split(new Char[] { ',' });
                        continue;
                    //case "Day User Define":
                    case "d":
                        g_CarryD = sParam_Value.Split(new Char[] { ',' }); ;
                        continue;
                    //case "Week User Define":
                    case "w":
                        g_CarryW = sParam_Value.Split(new Char[] { ',' }); ;
                        continue;
                    //case "Day of Week User Define":
                    case "k":
                        g_CarryDW = sParam_Value.Split(new Char[] { ',' }); ;
                        continue;
                }
            }
            //目前真正日期
            sSQL = " Select TO_CHAR(SYSDATE,'YYYY/MM/DD/IW/DDD/D') YMD, sysdate From DUAL ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            string mDateCode = dsTemp.Tables[0].Rows[0]["YMD"].ToString();
            string mSysDate = dsTemp.Tables[0].Rows[0]["sysdate"].ToString();
            string mDY = mDateCode.Substring(0, 4);
            string mDM = mDateCode.Substring(5, 2);
            string mDD = mDateCode.Substring(8, 2);
            string mDW = mDateCode.Substring(11, 2);
            string mDYW = mDateCode.Substring(14, 3); //Day Of Year
            string mDK = mDateCode.Substring(18, 1);  //Day Of Week                        
            //User Define Day Code            
            string mcDM = "";
            if (g_CarryM[0].ToString() != "" && g_CarryM.Length >= Convert.ToInt32(mDM))
                mcDM = g_CarryM[Convert.ToInt32(mDM) - 1].ToString();
            string mcDD = "";
            if (g_CarryD[0].ToString() != "" && g_CarryD.Length >= Convert.ToInt32(mDD))
                mcDD = g_CarryD[Convert.ToInt32(mDD) - 1].ToString();
            string mcDW = "";
            if (g_CarryW[0].ToString() != "" && g_CarryW.Length >= Convert.ToInt32(mDW))
                mcDW = g_CarryW[Convert.ToInt32(mDW) - 1].ToString();
            string mcDK = "";
            if (g_CarryDW[0].ToString() != "" && g_CarryDW.Length >= Convert.ToInt32(mDK))
                mcDK = g_CarryDW[Convert.ToInt32(mDK) - 1].ToString();

            string[] sDayValue = new string[8];
            sDayValue[0] = mDD;
            sDayValue[1] = mDM;
            sDayValue[2] = mDK;
            sDayValue[3] = mDW;
            sDayValue[4] = mDY;
            sDayValue[5] = mcDD;
            sDayValue[6] = mcDW;
            sDayValue[7] = mcDM;

            //====開始轉換===================================================
            string sMask = "";
            for (int i = sCode.Length - 1; i >= 0; i--)
            {
                string sReplace = "0";
                //===轉換Function===
                if (LVFun.Items.Count > 0)
                {
                    ListViewItem Item = LVFun.FindItemWithText(sCode[i].ToString(), false, 0);
                    int iIndex = LVFun.Items.IndexOf(Item);
                    if (iIndex != -1)
                    {
                        string sValue = LVFun.Items[iIndex].SubItems[3].Text;
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
                                sMask = "L" + sMask;
                            }
                            sValue = sValue.Substring(0, sValue.Length - 1);
                            LVFun.Items[iIndex].SubItems[3].Text = sValue;
                        }
                        else //2012/7/8 add begin ==
                        {
                            sReplace = "0";
                            sMask = "9" + sMask;
                        }
                        sDefault = sDefault.Remove(i, 1);
                        sDefault = sDefault.Insert(i, sReplace);
                        //2012/7/8 add end ==
                        continue;
                    }
                }
                //===轉換Day Code及固定碼=== 
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
                    case "C":
                        sMask = "C" + sMask;
                        sReplace = sDefault[i].ToString();
                        break;
                    case "9":
                        sMask = "9" + sMask;
                        sReplace = sDefault[i].ToString();
                        break;
                    default:
                        sMask = "L" + sMask;
                        sReplace = sDefault[i].ToString();
                        break;
                }

                sDefault = sDefault.Remove(i, 1);
                sDefault = sDefault.Insert(i, sReplace);
            }

            //===轉換流水號============            
            //代表流水號的字母            
            string sSeqText = "S";
            for (int i = 0; i <= LVDefSeq.Items.Count - 1; i++)
            {
                sSeqText = sSeqText + LVDefSeq.Items[i].Text;
            }

            //是否要先Reset Sequence                 
            sSQL = "select Last_Number from all_sequences "
                 + "where sequence_name = '" + sSeqName.ToUpper() + "' "
                 + "and sequence_owner = user ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                Reset_Sequence(sSeqName, sCode, sResetCycle, sDayValue, ref sResetMark, false);
            }
            else
            {
                if (sResetSeq == "Y")
                {
                    Reset_Sequence(sSeqName, sCode, sResetCycle, sDayValue, ref sResetMark, true);
                }
            }

            //2012/12/17:1.0.0.7 --修改當自定義流水號中沒有1時會出現Start WITH cannot be less then minvalue錯誤
            string sSrt = SeqTran(1, sCode, sSeqText, sCarry, LVDefSeq);
            Create_Rule_Seq(sSeqName, sSrt, sCode, sSeqText, sCarry, LVDefSeq);
            //Create_Rule_Seq(sSeqName, "1", sCode, sSeqText, sCarry, LVDefSeq);

            //找此次使用的Sequence
            sSQL = "SELECT " + sSeqName + ".NEXTVAL SNID FROM DUAL ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            int iStart = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["SNID"].ToString());
            //BarcodeCenter會把INCREMENT By改成大於1,因此此處需改為1,否則找下個號碼時會跳號
            sSQL = "alter sequence " + sSeqName + " INCREMENT BY 1 ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            //                                   
            string sSeq = SeqTran(iStart, sCode, sSeqText, sCarry, LVDefSeq);
            int j = 0;
            for (int i = 0; i <= sCode.Length - 1; i++)
            {
                if (sSeqText.IndexOf(sCode[i]) > -1)
                {
                    sDefault = sDefault.Remove(i, 1);
                    sDefault = sDefault.Insert(i, sSeq[j].ToString());
                    j++;
                }
            }

            //===轉換Check Sum值===           
            if (sCode.IndexOf("X") != -1)
            {
                sSQL = " select " + sCheckSum + "('" + sDefault + "') SNID from dual ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                sDefault = dsTemp.Tables[0].Rows[0]["SNID"].ToString();
            }

            sNewNo = sDefault;
            return true;
        }

        public bool Reset_Sequence(string sSeqName, string sCode, string sResetCycle, string[] sDayValue, ref string sMark, bool b_SeqExist)
        {
            //最後一次Reset Sequence日期           
            string mDD = sDayValue[0];
            string mDM = sDayValue[1];
            string mDK = sDayValue[2];
            string mDW = sDayValue[3];
            string mDY = sDayValue[4];
            string mcDD = sDayValue[5];
            string mcDW = sDayValue[6];
            string mcDM = sDayValue[7];

            //Reset Seq               
            bool b_Reset = false;
            switch (sResetCycle)
            {
                case "0": //Reset By Day
                    if (sCode.ToUpper().IndexOf("D") > -1)
                    {
                        if (sMark == "" || sMark != mDD)
                        {
                            sMark = mDD;
                            b_Reset = true;
                        }
                    }
                    else if (sCode.ToUpper().IndexOf("K") > -1)
                    {
                        if (sMark == "" || sMark != mDK)
                        {
                            sMark = mDK;
                            b_Reset = true;
                        }
                    }
                    else
                    {
                        if (sMark == "" || sMark != mcDD)
                        {
                            sMark = mcDD;
                            b_Reset = true;
                        }
                    }
                    break;
                case "1": //Reset By Week
                    if (sCode.ToUpper().IndexOf("W") > -1)
                    {
                        if (sMark == "" || sMark != mDW)
                        {
                            sMark = mDW;
                            b_Reset = true;
                        }
                    }
                    else
                    {
                        if (sMark == "" || sMark != mcDW)
                        {
                            sMark = mcDW;
                            b_Reset = true;
                        }
                    }
                    break;
                case "2": //Reset By Month
                    if (sCode.ToUpper().IndexOf("M") > -1)
                    {
                        if (sMark == "" || sMark != mDM)
                        {
                            sMark = mDM;
                            b_Reset = true;
                        }
                    }
                    else
                    {
                        if (sMark == "" || sMark != mcDM)
                        {
                            sMark = mcDM;
                            b_Reset = true;
                        }
                    }
                    break;
                case "3": //Reset By Year
                    if (sMark == "" || sMark != mDY)
                    {
                        sMark = mDY;
                        b_Reset = true;
                    }
                    break;
            }

            if (b_SeqExist && b_Reset)
            {
                string sSQL = "Drop Sequence " + sSeqName;
                DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            }
            return b_Reset;
        }

        //===============================================================================        
        public string CheckRule(string sInputNo, string sDefault, string sCode, string sCarry, ListView LV, bool bCheckDef)
        {
            //sInputNo:產生的號碼; sCode:編碼規則;
            //sCarry:10/16進位; LV:自定義的DayCode; bCheckDef:是否要強制日期要與Default相同
            string sCarryM = ""; string sCarryD = ""; string sCarryW = ""; string sCarryK = ""; ;
            for (int j = 0; j <= LV.Items.Count - 1; j++)
            {
                switch (LV.Items[j].Text)
                {
                    case "m":
                        sCarryM = LV.Items[j].SubItems[1].Text;
                        continue;
                    case "d":
                        sCarryD = LV.Items[j].SubItems[1].Text;
                        continue;
                    case "w":
                        sCarryW = LV.Items[j].SubItems[1].Text;
                        continue;
                    case "k":
                        sCarryK = LV.Items[j].SubItems[1].Text;
                        continue;
                }
            }

            //檢查長度
            if (sCode.Length != sInputNo.Length)
            {
                return "Rule not match (Length)";
            }
            //檢查固定碼
            string sData = ""; int iR;
            string sM = ""; string sD = ""; string sW = ""; string sF = "";
            string uM = ""; string uD = ""; string uW = ""; string uK = "";

            string sMDef = ""; string sDDef = ""; string sWDef = ""; string sFDef = "";
            string uMDef = ""; string uDDef = ""; string uWDef = ""; string uKDef = "";

            for (int i = 0; i <= sCode.Length - 1; i++)
            {
                switch (sCode[i].ToString())
                {
                    case "L":
                        if (sDefault[i].ToString() != " " && sDefault[i].ToString() != sInputNo[i].ToString())
                            return "Rule not match (Fix Character)";
                        continue;
                    case "C":
                        if (sDefault[i].ToString() != " " && sDefault[i].ToString() != sInputNo[i].ToString())
                            return "Rule not match (Fix Character)";
                        continue;
                    case "9":
                        if (sDefault[i].ToString() != " " && sDefault[i].ToString() != sInputNo[i].ToString())
                            return "Rule not match (Fix Character)";
                        continue;
                    case "Y":
                        sData = "0123456789";
                        if (sData.IndexOf(sInputNo[i].ToString()) == -1)
                            return "Rule not match (Year)";
                        continue;
                    case "K": //Day of Week
                        sData = "1234567";
                        if (sData.IndexOf(sInputNo[i].ToString()) == -1)
                            return "Rule not match (Day of Week)";
                        continue;
                    case "S":
                        if (sCarry == "10")
                        {
                            sData = "0123456789";
                            if (sData.IndexOf(sInputNo[i].ToString()) == -1)
                                return "Rule not match (Sequence)";
                        }
                        else if (sCarry == "16")
                        {
                            sData = "0123456789ABCDEF";
                            if (sData.IndexOf(sInputNo[i].ToString()) == -1)
                                return "Rule not match (Sequence)";
                        }
                        continue;
                    case "M":
                        sM = sM + sInputNo[i].ToString();
                        sMDef = sMDef + sDefault[i].ToString(); //實際月份
                        continue;
                    case "D":
                        sD = sD + sInputNo[i].ToString();
                        sDDef = sDDef + sDefault[i].ToString();
                        continue;
                    case "W":
                        sW = sW + sInputNo[i].ToString();
                        sWDef = sWDef + sDefault[i].ToString();
                        continue;
                    case "F": //Day of Year
                        sF = sF + sInputNo[i].ToString();
                        sFDef = sFDef + sDefault[i].ToString();
                        continue;
                    case "m":
                        uM = uM + sInputNo[i].ToString();
                        uMDef = uMDef + sDefault[i].ToString();
                        continue;
                    case "d":
                        uD = uD + sInputNo[i].ToString();
                        uDDef = uDDef + sDefault[i].ToString();
                        continue;
                    case "w":
                        uW = uW + sInputNo[i].ToString();
                        uWDef = uWDef + sDefault[i].ToString();
                        continue;
                    case "k":
                        uK = uK + sInputNo[i].ToString();
                        uKDef = uKDef + sDefault[i].ToString();
                        continue;
                }
            }
            if (sM != "")
            {
                try { iR = Convert.ToInt32(sM); }
                catch { return "Rule not match (Month)"; }
                if (iR < 1 || iR > 12)
                    return "Rule not match (Month)";
                //是否要檢查年月日週與Default的值需相同
                if (bCheckDef && sM != sMDef)
                    return "Rule not match (Month:" + sM + "=" + sMDef + ")";
            }
            if (sD != "")
            {
                try { iR = Convert.ToInt32(sD); }
                catch { return "Rule not match (Day)"; }
                if (iR < 1 || iR > 31)
                    return "Rule not match (Day)";
                if (bCheckDef && sD != sDDef)
                    return "Rule not match (Day:" + sD + "=" + sDDef + ")";
            }
            if (sW != "")
            {
                try { iR = Convert.ToInt32(sW); }
                catch { return "Rule not match (Week)"; }
                if (iR < 1 || iR > 53)
                    return "Rule not match (Week)";
                if (bCheckDef && sW != sWDef)
                    return "Rule not match (Week:" + sW + "=" + sWDef + ")";
            }
            if (sF != "")
            {
                try { iR = Convert.ToInt32(sF); }
                catch { return "Rule not match (Day of Year)"; }
                if (iR < 1 || iR > 366)
                    return "Rule not match (Day of Year)";
                if (bCheckDef && sF != sFDef)
                    return "Rule not match (Day of Year:" + sF + "=" + sFDef + ")";
            }

            if (uM != "")
            {
                if (sCarryM.IndexOf(uM) == -1)
                    return "Rule not match (Month User Define)";
                if (bCheckDef && uM != uMDef)
                    return "Rule not match (Month User Define:" + uM + "=" + uMDef + ")";
            }
            if (uD != "")
            {
                if (sCarryD.IndexOf(uD) == -1)
                    return "Rule not match (Day User Define)";
                if (bCheckDef && uD != uDDef)
                    return "Rule not match (Day User Define:" + uD + "=" + uDDef + ")";
            }
            if (uW != "")
            {
                if (sCarryW.IndexOf(uW) == -1)
                    return "Rule not match (Week User Define)";
                if (bCheckDef && uW != uWDef)
                    return "Rule not match (Week User Define:" + uW + "=" + uWDef + ")";
            }
            if (uK != "")
            {
                if (sCarryK.IndexOf(uK) == -1)
                    return "Rule not match (Day of Week User Define)";
                if (bCheckDef && uK != uKDef)
                    return "Rule not match (Day of Week User Define:" + uK + "=" + uKDef + ")";
            }
            return "OK";
        }

        //以下為BCLabel與BCUDLabel共用的Function
        public string g_sCarry16 = "0123456789ABCDEF";

        public bool Check_Code(string sStart, string sRuleCode, string g_sCarry, ListView LVUserSeq, out string sMsg)
        {
            //sStart:流水號或全部字元,sRuleCode:規則代碼, sSeqText:流水號字元, g_sCarry:10/16進位, LVUserSeq:自定義流水號
            //檢查流水號部分是否合法
            string sSeqText = "S";
            string sCode = "";
            string sSeqNo = "";
            sMsg = "";

            for (int i = 0; i <= LVUserSeq.Items.Count - 1; i++)
            {
                sSeqText = sSeqText + LVUserSeq.Items[i].Text;
            }
            //取出屬於流水號的Code
            for (int i = 0; i <= sRuleCode.Length - 1; i++)
            {
                if (sSeqText.IndexOf(sRuleCode[i]) > -1)
                {
                    sCode = sCode + sRuleCode[i]; //取出流水號的字元
                    if (sStart.Length == sRuleCode.Length)
                        sSeqNo = sSeqNo + sStart[i];
                    else if (sStart.Length >= sCode.Length)
                        sSeqNo = sSeqNo + sStart[sCode.Length - 1]; //取出Label的流水號碼
                }
            }
            if (sCode.Length == sStart.Length)
                sSeqNo = sStart;
            else
            {
                //流水號長度不符合規則所定義的長度
                if (sRuleCode.Length != sStart.Length)
                {
                    sMsg = "Length Error";
                    return false;
                }
            }

            for (int i = sCode.Length - 1; i >= 0; i--)
            {
                if (sCode[i].ToString() == "S")
                {
                    if (g_sCarry == "16")
                    {
                        if (g_sCarry16.IndexOf(sSeqNo[i].ToString()) == -1)
                        {
                            sMsg = "Hexadecimal Error";
                            return false;
                        }
                    }
                    else
                    {
                        try
                        {
                            Convert.ToInt32(sSeqNo[i].ToString());
                        }
                        catch
                        {
                            sMsg = "Decimal Error";
                            return false;
                        }
                    }
                }
                else
                {
                    for (int j = 0; j <= LVUserSeq.Items.Count - 1; j++)
                    {
                        if (sCode[i].ToString() == LVUserSeq.Items[j].Text)
                        {
                            string sSeqValue = LVUserSeq.Items[j].SubItems[1].Text;
                            if (sSeqValue.IndexOf(sSeqNo[i].ToString()) == -1)
                            {
                                sMsg = "User Define Error";
                                return false;
                            }
                            break;
                        }
                    }
                }
            }
            return true;
        }

        public int SeqCode(string sStart, string sRuleCode, string sSeqText, string g_sCarry, ListView LVUserSeq)
        {
            //sStart:流水號或全部字元,sRuleCode:規則代碼, sSeqText:流水號字元, g_sCarry:10/16進位, LVUserSeq:自定義流水號
            //轉成10進位           
            int iResult = 0;
            int iBase;

            string sCode = "";
            string sSeqNo = ""; //add
            for (int i = 0; i <= sRuleCode.Length - 1; i++)
            {
                //if (sSeqText.IndexOf(sRuleCode[i]) > -1)
                //    sCode = sCode + sRuleCode[i];
                if (sSeqText.IndexOf(sRuleCode[i]) > -1)
                {
                    sCode = sCode + sRuleCode[i]; //取出流水號的字元
                    if (sStart.Length == sRuleCode.Length)
                        sSeqNo = sSeqNo + sStart[i];
                    else if (sStart.Length >= sCode.Length)
                        sSeqNo = sSeqNo + sStart[sCode.Length - 1]; //取出Label的流水號碼
                }
            }
            if (sCode.Length == sStart.Length)
                sSeqNo = sStart;

            for (int i = 0; i <= sCode.Length - 1; i++)
            {
                if (sCode[i].ToString() == "S")
                {
                    if (g_sCarry == "16")
                        sRuleCode = g_sCarry16;
                    else
                        sRuleCode = g_sCarry16.Substring(0, 10);
                }
                else
                {
                    for (int j = 0; j <= LVUserSeq.Items.Count - 1; j++)
                    {
                        if (sCode[i].ToString() == LVUserSeq.Items[j].Text)
                        {
                            sRuleCode = LVUserSeq.Items[j].SubItems[1].Text.ToString();
                            break;
                        }
                    }
                }
                iBase = sRuleCode.Length;
                //iResult = iResult * iBase + (sRuleCode.IndexOf(sStart[i]));
                iResult = iResult * iBase + (sRuleCode.IndexOf(sSeqNo[i]));
            }
            return iResult;
        }
        public Int64 SeqCode(string sStart, string sRuleCode, string sSeqText, string g_sCarry, ListView LVUserSeq, string s)
        {
            //sStart:流水號或全部字元,sRuleCode:規則代碼, sSeqText:流水號字元, g_sCarry:10/16進位, LVUserSeq:自定義流水號
            //轉成10進位           

            Int64 iResult = 0;
            int iBase;

            string sCode = "";
            string sSeqNo = ""; //add
            for (int i = 0; i <= sRuleCode.Length - 1; i++)
            {
                //if (sSeqText.IndexOf(sRuleCode[i]) > -1)
                //    sCode = sCode + sRuleCode[i];

                if (sSeqText.IndexOf(sRuleCode[i]) > -1)
                {
                    sCode = sCode + sRuleCode[i]; //取出流水號的字元
                    if (sStart.Length == sRuleCode.Length)
                        sSeqNo = sSeqNo + sStart[i];
                    else if (sStart.Length >= sCode.Length)
                        sSeqNo = sSeqNo + sStart[sCode.Length - 1]; //取出Label的流水號碼
                }
            }
            if (sCode.Length == sStart.Length)
                sSeqNo = sStart;

            for (int i = 0; i <= sCode.Length - 1; i++)
            {
                if (sCode[i].ToString() == "S")
                {
                    if (g_sCarry == "16")
                        sRuleCode = g_sCarry16;
                    else
                        sRuleCode = g_sCarry16.Substring(0, 10);
                }
                else
                {
                    for (int j = 0; j <= LVUserSeq.Items.Count - 1; j++)
                    {
                        if (sCode[i].ToString() == LVUserSeq.Items[j].Text)
                        {
                            sRuleCode = LVUserSeq.Items[j].SubItems[1].Text.ToString();
                            break;
                        }
                    }
                }
                iBase = sRuleCode.Length;
                //iResult = iResult * iBase + (sRuleCode.IndexOf(sStart[i]));
                iResult = iResult * iBase + (sRuleCode.IndexOf(sSeqNo[i]));
            }
            return iResult;
        }

        public string SeqTran(int iSeq, string sRuleCode, string sSeqText, string g_sCarry, ListView LVUserSeq)
        {
            //sRuleCode:規則代碼, sSeqText:流水號字元, g_sCarry:10/16進位, LVUserSeq:自定義流水號
            //計算流水號                    
            string sCode = "";
            string sSEQ = "";

            for (int i = 0; i <= sRuleCode.Length - 1; i++)
            {
                if (sSeqText.IndexOf(sRuleCode[i]) > -1)
                {
                    sCode = sCode + sRuleCode[i];
                }
            }
            int mDiv = iSeq;

            for (int i = sCode.Length - 1; i >= 0; i--)
            {
                //S:進位方式
                if (sCode[i].ToString() == "S")
                {
                    if (g_sCarry == "16")
                        sRuleCode = g_sCarry16;
                    else
                        sRuleCode = g_sCarry16.Substring(0, 10);
                }
                else //User Define Sequence
                {
                    for (int j = 0; j <= LVUserSeq.Items.Count - 1; j++)
                    {
                        if (sCode[i].ToString() == LVUserSeq.Items[j].Text)
                        {
                            sRuleCode = LVUserSeq.Items[j].SubItems[1].Text;
                            break;
                        }
                    }
                }

                if (mDiv != 0)
                {
                    int mMod = mDiv % sRuleCode.Length; //癓㩞
                    mDiv = mDiv / sRuleCode.Length;
                    if (mMod == 0)
                        sSEQ = sRuleCode.Substring(0, 1) + sSEQ;
                    else
                        sSEQ = sRuleCode.Substring(mMod, 1) + sSEQ;
                }
                else
                {
                    if (sCode[i].ToString() == "S")
                        sSEQ = "0" + sSEQ;
                    else
                        sSEQ = sRuleCode.Substring(0, 1) + sSEQ;
                }
            }

            //流水號是否會超過數[規則設定的位溢位](ex:設SS卻要展100個)  
            if (mDiv > 0)
            {
                return "-1";
            }
            return sSEQ;
        }
        public string SeqTran(Int64 iSeq, string sRuleCode, string sSeqText, string g_sCarry, ListView LVUserSeq)
        {
            //sRuleCode:規則代碼, sSeqText:流水號字元, g_sCarry:10/16進位, LVUserSeq:自定義流水號
            //計算流水號              
            string sCode = "";
            string sSEQ = "";

            for (int i = 0; i <= sRuleCode.Length - 1; i++)
            {
                if (sSeqText.IndexOf(sRuleCode[i]) > -1)
                {
                    sCode = sCode + sRuleCode[i];
                }
            }
            Int64 mDiv = iSeq;

            for (int i = sCode.Length - 1; i >= 0; i--)
            {
                //S:進位方式
                if (sCode[i].ToString() == "S")
                {
                    if (g_sCarry == "16")
                        sRuleCode = g_sCarry16;
                    else
                        sRuleCode = g_sCarry16.Substring(0, 10);
                }
                else //User Define Sequence
                {
                    for (int j = 0; j <= LVUserSeq.Items.Count - 1; j++)
                    {
                        if (sCode[i].ToString() == LVUserSeq.Items[j].Text)
                        {
                            sRuleCode = LVUserSeq.Items[j].SubItems[1].Text;
                            break;
                        }
                    }
                }

                if (mDiv != 0)
                {
                    Int64 mMod = (mDiv) % sRuleCode.Length; //餘數
                    mDiv = mDiv / sRuleCode.Length;
                    if (mMod == 0)
                        sSEQ = sRuleCode.Substring(0, 1) + sSEQ;
                    else
                        sSEQ = sRuleCode.Substring(Convert.ToInt32(mMod), 1) + sSEQ;
                }
                else
                {
                    if (sCode[i].ToString() == "S")
                        sSEQ = "0" + sSEQ;
                    else
                        sSEQ = sRuleCode.Substring(0, 1) + sSEQ;
                }
            }

            //流水號是否會超過規則設定的位數[溢位](ex:設SS卻要展100個)
            if (mDiv > 0)
            {
                return "-1";
            }
            return sSEQ;
        }
        public string GetSeq_MaxValue(string sRuleCode, string g_sCarry, ListView LVUserSeq)
        {
            //計算Sequence最大的10進位值(MaxValue)            
            int iSeq = 1;
            for (int i = 0; i <= sRuleCode.Length - 1; i++)
            {
                if (sRuleCode[i].ToString() == "S")
                {
                    if (g_sCarry == "16")
                        iSeq = iSeq * 16;
                    else
                        iSeq = iSeq * 10;
                }
                else
                {
                    for (int j = 0; j <= LVUserSeq.Items.Count - 1; j++)
                    {
                        if (sRuleCode[i].ToString() == LVUserSeq.Items[j].Text)
                        {
                            iSeq = iSeq * LVUserSeq.Items[j].SubItems[1].Text.Length;
                            break;
                        }
                    }
                }
            }
            return Convert.ToString(iSeq - 1);
        }
        public object[] CheckSeqMaxValue(string sSeqName, int iQty)
        {
            string sSQL = "select Max_Value,Last_Number from all_sequences "
                 + "where Upper(sequence_name) = '" + sSeqName.ToUpper() + "'  and sequence_owner = user";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            int iMaxQty = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["MAX_VALUE"].ToString());
            int iLastQty = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["LAST_NUMBER"].ToString());
            int iAvailableQty = iMaxQty - iLastQty;
            object[] Params = new object[4];
            if (iQty > iAvailableQty)
            {
                Params[0] = "Over Max Value(Sequence)";
                Params[1] = iMaxQty;
                Params[2] = iLastQty;
                Params[3] = iAvailableQty;
            }
            else
                Params[0] = "OK";
            return Params;
        }
        public void Create_Rule_Seq(string sSeqName, string sStartSeq, string sRuleCode, string sSeqText, string g_sCarry, ListView LVUserSeq)
        {
            string sSQL;
            DataSet dsTemp;
            sSQL = " select PARAM_VALUE FROM SAJET.SYS_BASE_PARAM WHERE PROGRAM = 'Barcode Center' AND PARAM_NAME = 'Sequence Min Value' AND ROWNUM = 1";
            string sMin = "0";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
                sMin = dsTemp.Tables[0].Rows[0][0].ToString();
            sSQL = " select * from user_objects "
                 + " where object_type = 'SEQUENCE' "
                 + " and object_name = '" + sSeqName + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                int iStart = SeqCode(sStartSeq, sRuleCode, sSeqText, g_sCarry, LVUserSeq);

                string sStart = Convert.ToString(iStart);
                string sEnd = GetSeq_MaxValue(sRuleCode, g_sCarry, LVUserSeq);
                //append by hidy 2016/3/18 
                if (sEnd == "0")
                {
                    sEnd = "99999";
                }
                //end by hidy 2106/3/18
                //if (sStart != "0")
                //sSQL = "CREATE SEQUENCE " + sSeqName + " INCREMENT BY 1 START WITH " + sStart + " MAXVALUE " + sEnd + " CYCLE NOCACHE ORDER ";
                //else
                sSQL = "CREATE SEQUENCE " + sSeqName + " INCREMENT BY 1 START WITH " + sStart + " minvalue " + sMin + " MAXVALUE " + sEnd + " CYCLE NOCACHE ORDER ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);

                sSQL = "GRANT SELECT ON " + sSeqName + " TO SYS_USER";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
            }
        }

        public int GetSeq_NextVal(string sServerName, string sSeqName, int sQty)
        {
            string sSQL = "Select " + sSeqName + ".NEXTVAL SNID From Dual";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            int iSeq = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["SNID"].ToString());

            //將Sequence的INCREMENT BY改為此次需展的數量(讓號碼可以連續)
            sSQL = " alter sequence " + sSeqName + " INCREMENT BY " + sQty;
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            return iSeq;
        }
    }
}