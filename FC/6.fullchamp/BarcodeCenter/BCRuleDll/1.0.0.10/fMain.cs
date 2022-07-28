using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using SajetClass;
using System.Data.OracleClient;
using System.Globalization;
using Microsoft.Win32;

namespace BCRuleDll
{
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();
        }              

        public int g_iPrivilege = 0;
        public string g_sUserID;                
        public string g_sRuleType;
        public bool gb_ResetSeq = false;
        public string g_sSeq;
        public string g_sLabType;
        public string g_sPreRuleCode;
        public string g_sRuleID;
        public string g_sTable;
        public static string g_sExeName;
        public string g_sPartField = "PART_ID";
        //已經被定義的代碼,不可以再被使用
        public string g_sDefKey = "CYMDL9XWKFmdwkS";
        public string g_sDefKeyAll;        
        public string[] sArray_ResetType = new string[] { "Day", "Week", "Month", "Year" };
        
        fResetSeq fReset = new fResetSeq();
        string sSQL;
        DataSet dsTemp;

        public string g_sDef;             

        public void check_privilege()
        {
            btnSave.Enabled = false;
            btnResetSeq.Visible = false;
            panelMaintain.Enabled = false;
            popMenuRule.Enabled = false;

            string sFileName = SajetCommon.g_sFileName.ToUpper();
            sSQL = " SELECT PROGRAM,FUNCTION  "
                 + " FROM SAJET.SYS_PROGRAM_FUN_NAME "
                 + " WHERE Upper(DLL_FILENAME) = '" + sFileName + "' "
                 + " AND fun_param='" + g_sRuleType + "' "
                 + " AND ROWNUM=1 ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                string sProgram = dsTemp.Tables[0].Rows[0]["PROGRAM"].ToString();
                string sFunction = dsTemp.Tables[0].Rows[0]["FUNCTION"].ToString();
                g_iPrivilege = ClientUtils.GetPrivilege(g_sUserID, sFunction, sProgram);
//                SajetCommon.Get_OtherPrivilege(g_sUserID, sProgram, sFunction); 
                
                //讀取Reset Sequence權限
                int iResetPrivilege = ClientUtils.GetPrivilege(g_sUserID, "Reset Sequence", sProgram);
                gb_ResetSeq = (iResetPrivilege > 0);
            }
            else
            {
                g_iPrivilege = 0;
                gb_ResetSeq = false;
            }
            btnSave.Enabled = (g_iPrivilege >= 1);
            panelMaintain.Enabled = (g_iPrivilege >= 1);
            popMenuRule.Enabled = (g_iPrivilege >= 1);
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            g_sExeName = ClientUtils.fCurrentProject;
            g_sUserID = ClientUtils.UserPara1;

            //讀取傳入的參數
            g_sRuleType = ClientUtils.fParameter;
            //this.Text = g_sRuleType + " Rule Define";
                      
            //找Label的種類
            sSQL = " select * from sajet.sys_label "
                 + " where label_name = '" + g_sRuleType + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                g_sSeq = dsTemp.Tables[0].Rows[0]["SEQ_NAME"].ToString();
                g_sLabType = dsTemp.Tables[0].Rows[0]["TYPE"].ToString();
                g_sTable = dsTemp.Tables[0].Rows[0]["TABLE_NAME"].ToString();
                try
                {
                    if (!string.IsNullOrEmpty(dsTemp.Tables[0].Rows[0]["PART_FIELD_NAME"].ToString()))
                        g_sPartField = dsTemp.Tables[0].Rows[0]["PART_FIELD_NAME"].ToString();
                }
                catch { };
                //若Label Type是S(先展再與工單結合),不可設定Function
                if (g_sLabType == "S")
                    gbFunction.Visible = false;
            }

            check_privilege();
            Show_DefKey();            
            Show_RuleName();
            Show_Function("X_", combCheckSum); //Show Check Sum的Function(X_)
            ClientUtils.SetLanguage(this, g_sExeName);
            this.Text = this.Text + " (" + SajetCommon.g_sFileVersion + ")";

            //Load
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            panelTitle.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
            panelMaintain.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");

            if (g_sRuleType.ToUpper() != "MAC")
            {
                LVRule.Columns[2].Width = 0;
                LVRule.Columns[3].Width = 0;
            }            
        }        

        public void Show_RuleName()
        {
            //顯示所有Rule Name
            string sFilter = editRuleFilter.Text.Trim().ToUpper();
            LVRule.Items.Clear();

            sSQL = " Select * "
                 + " From SAJET.SYS_RULE_NAME "
                 + " Where RULE_TYPE = '" + g_sRuleType.ToUpper() + "' ";
            if (sFilter != "" && sFilter != "%")
                sSQL = sSQL + " and UPPER(RULE_NAME) like '" + sFilter + "%'";
            sSQL = sSQL + "Order By RULE_NAME ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1;i++ )
            {
                LVRule.Items.Add(dsTemp.Tables[0].Rows[i]["RULE_NAME"].ToString());
                LVRule.Items[i].SubItems.Add(dsTemp.Tables[0].Rows[i]["RULE_DESC"].ToString());
                LVRule.Items[i].SubItems.Add(dsTemp.Tables[0].Rows[i]["GROUP_QTY"].ToString());
                LVRule.Items[i].SubItems.Add(dsTemp.Tables[0].Rows[i]["SAFETY_STOCK"].ToString());

                LVRule.Items[i].Tag = dsTemp.Tables[0].Rows[i]["RULE_ID"].ToString();
                LVRule.Items[i].ImageIndex = 0;
            }
        }

        private void Clear_Data()
        {
            editRuleCode.Text = "";
            editDefault.Text = "";
            editDefault.Mask = "";
            LVUserSeq.Items.Clear();
            LVFun.Items.Clear();
            editMonth.Text = "";
            editDay.Text = "";
            editWeek.Text = "";
            editWeekYear.Text = "";
            combCheckSum.SelectedIndex = -1;
            rbCarry10.Checked = true;
            rbCarry16.Checked = false;
            combResetType.Items.Clear();
            for (int i = 0; i < sArray_ResetType.Length; i++)
                combResetType.Items.Add(sArray_ResetType[i].ToString());

            btnResetSeq.Visible = false;
            lablSeq.Visible = btnResetSeq.Visible;

            rbSeq.Checked = true;
        }

        public void Show_RuleData(string sRuleID)
        {
            //顯示Rule的各項設定資料
            Clear_Data();  
            
            string sParam_Name = "";
            string sParam_Item = "";
            string sParam_Value = "";
            sSQL = " Select * "
                 + " From SAJET.SYS_RULE_PARAM "
                 + " Where RULE_ID = '" + sRuleID + "' "                        
                 + " Order By PARAME_NAME,PARAME_ITEM,PARAME_VALUE ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
                return;
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                sParam_Name = dsTemp.Tables[0].Rows[i]["PARAME_NAME"].ToString();
                sParam_Item = dsTemp.Tables[0].Rows[i]["PARAME_ITEM"].ToString();
                sParam_Value = dsTemp.Tables[0].Rows[i]["PARAME_VALUE"].ToString();
                //編碼規則與Default值
                if (sParam_Name == g_sRuleType + " Code")
                {
                    if (sParam_Item == "Code")
                        editRuleCode.Text = sParam_Value;
                    else if (sParam_Item == "Default")
                        editDefault.Text = sParam_Value;
                    else if (sParam_Item == "Code Type")
                    {
                        rbCarry10.Checked = (sParam_Value == "10");
                        rbCarry16.Checked = (sParam_Value == "16");
                    }
                    continue;
                }
                //自行定義的日期代碼
                switch (sParam_Name)
                {                                   
                    case "Month User Define":
                        editMonth.Text = sParam_Value;
                        continue;
                    case "Day User Define":
                        editDay.Text = sParam_Value;
                        continue;
                    case "Week User Define":
                        editWeek.Text = sParam_Value;
                        continue;
                    case "Day of Week User Define":
                        editWeekYear.Text = sParam_Value;
                        continue;
                    case "Check Sum":
                        combCheckSum.SelectedIndex = combCheckSum.FindString(sParam_Value);
                        continue;
                    case "Reset Sequence":
                        combResetType.SelectedIndex = Convert.ToInt32(sParam_Value);
                        chkbReset.Checked = (sParam_Item == "1");
                        continue;
                    case "Sequence Mode":
                        rbNoSeq.Checked = (sParam_Value == "Manual");
                        rbSeq.Checked = (sParam_Value == "Auto");                        
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
                if (sParam_Name == g_sRuleType + " User Define")
                {
                    LVUserSeq.Items.Add(sParam_Item);
                    LVUserSeq.Items[LVUserSeq.Items.Count-1].SubItems.Add(sParam_Value);
                    continue;
                }                
            }
            Show_DefKey();
            Show_ResetSeq(editRuleName.Text);

            //讀取及設定Mask
            string sDefReplace = "";            
            string sMask = Get_Mask(editRuleCode.Text, editDefault.Text, ref sDefReplace);
            editDefault.Mask = sMask;
            editDefault.Text = sDefReplace;
        }

        private void editRuleFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
                Show_RuleName();
        }   

        private void editMonth_Click(object sender, EventArgs e)
        {
            Show_User_DayCode(editMonth,"Month",12);
        }

        private void editDay_Click(object sender, EventArgs e)
        {
            Show_User_DayCode(editDay, "Day", 31);
        }

        private void editWeek_Click(object sender, EventArgs e)
        {
            Show_User_DayCode(editWeek, "Week", 53);
        }

        private void editWeekYear_Click(object sender, EventArgs e)
        {
            Show_User_DayCode(editWeekYear, "Day Of Week", 7);
        }

        public void Show_User_DayCode(TextBox editTemp, string sType, int iMaxCount)
        {
            //自行定義日期代碼
            string sCode = editTemp.Text;
            string[] split = sCode.Split(new Char[] { ',' });
            fDayCode f1 = new fDayCode();
            try
            {
                f1.editCode.Lines = split;
                f1.iMaxCount = iMaxCount;
                f1.Text = sType;
                if (f1.ShowDialog() == DialogResult.OK)
                {
                    editTemp.Text = f1.editCode.Text;
                }
            }
            finally
            {
                f1.Dispose();
            }
        }

        public void Show_Function(string sFix, ComboBox combTemp)
        {
            //找出系統中建立的Function
            combTemp.Items.Clear();
            combTemp.Items.Add("");
            sSQL = " select owner || '.' || object_name object_name "
                 + " from all_objects "
                 + " where object_type='FUNCTION' "
                 + " and owner='SAJET' "
                 + " and substr(object_name,1,length('" + sFix + "'))='" + sFix + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                combTemp.Items.Add(dsTemp.Tables[0].Rows[i]["object_name"].ToString());               
            }
        }

        private void MenuItemDelete_Click(object sender, EventArgs e)
        {
            if (LVUserSeq.SelectedItems.Count > 0)
            {
                LVUserSeq.SelectedItems[0].Remove();
                Show_DefKey();
            }
        }

        private void MenuItemAdd_Click(object sender, EventArgs e)
        {
            //增加自訂的流水號進位方式
            fData fData = new fData();
            try
            {
                fData.g_iPageIdx=1;
              //  fData.sServerName = SajetCommon.g_sServerName;
                //fData.tabPageFun.Parent = null; //隱藏第二頁
                fData.editUSeqCode.Focus();
                fData.editUSeqCode.Select();
                fData.g_sKeyAll = g_sDefKeyAll;                
                if (fData.ShowDialog() == DialogResult.OK)
                {
                    string sCode = fData.editUSeqCode.Text;
                    string sSeq = fData.editUSeq.Text;
                    LVUserSeq.Items.Add(sCode);
                    LVUserSeq.Items[LVUserSeq.Items.Count - 1].SubItems.Add(sSeq);
                    Show_DefKey();
                }
            }
            finally
            {
                fData.Dispose();
            }
        }

        private void MenuItemModify_Click(object sender, EventArgs e)
        {
            //修改自訂的流水號進位方式
            if (LVUserSeq.SelectedItems.Count <= 0)
                return;

            fData fData = new fData();
            try
            {
                fData.g_iPageIdx = 1;
             //   fData.sServerName = SajetCommon.g_sServerName;
                //fData.tabPageFun.Parent = null;
                fData.editUSeqCode.Text = LVUserSeq.SelectedItems[0].Text;
                fData.editUSeq.Text = LVUserSeq.SelectedItems[0].SubItems[1].Text;
                fData.editUSeqCode.Select();
                fData.editUSeqCode.Focus();
                fData.editUSeqCode.SelectAll();
                fData.g_sKeyAll = g_sDefKeyAll;
                fData.g_sModiCode = LVUserSeq.SelectedItems[0].Text;
                if (fData.ShowDialog() == DialogResult.OK)
                {
                    string sCode = fData.editUSeqCode.Text;
                    string sSeq = fData.editUSeq.Text;
                    LVUserSeq.SelectedItems[0].Text = sCode;
                    LVUserSeq.SelectedItems[0].SubItems[1].Text = sSeq;
                    Show_DefKey();
                }
            }
            finally
            {
                fData.Dispose();
            }
        }
//Function====================================================================
        private void MenuItemAddFun_Click(object sender, EventArgs e)
        {
            //加入自定的Function           
            fData fData = new fData();
            try
            {
                fData.g_iPageIdx = 2;
            //    fData.sServerName = SajetCommon.g_sServerName; ;
                fData.combField.Items.Clear();                
                sSQL = "select * from sajet.sys_rule_field "
                     + "order by field_name ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);            
                for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
                {                    
                    fData.combField.Items.Add(dsTemp.Tables[0].Rows[i]["field_name"].ToString());
                }

                //fData.tabPageSeq.Parent = null;
                fData.editFunCode.Focus();
                fData.editFunCode.Select();
                fData.g_sKeyAll = g_sDefKeyAll;
                if (fData.ShowDialog() == DialogResult.OK)
                {
                    string sCode = fData.editFunCode.Text;
                    string sField = fData.combField.Text;                    
                    string sFunName = fData.combFunction.Text;                                       
                    LVFun.Items.Add(sCode);
                    LVFun.Items[LVFun.Items.Count - 1].SubItems.Add(sField);
                    LVFun.Items[LVFun.Items.Count - 1].SubItems.Add(sFunName);                    
                    Show_DefKey();
                }
            }
            finally
            {
                fData.Dispose();
            }
        }

        private void MenuItemModifyFun_Click(object sender, EventArgs e)
        {
            //修改自定的Function 
            if (LVFun.SelectedItems.Count <= 0)
                return;

            fData fData = new fData();
            try
            {
                fData.g_iPageIdx = 2;
               // fData.sServerName = SajetCommon.g_sServerName; ;
                fData.combField.Items.Clear();
                sSQL = "select * from sajet.sys_rule_field "
                     + "order by field_name ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
                {                    
                    fData.combField.Items.Add(dsTemp.Tables[0].Rows[i]["field_name"].ToString());
                }                              

                //fData.tabPageSeq.Parent = null;
                fData.editFunCode.Text = LVFun.SelectedItems[0].Text;                
                fData.combField.SelectedIndex = fData.combField.FindString(LVFun.SelectedItems[0].SubItems[1].Text);
                fData.combFunction.SelectedIndex = fData.combFunction.FindString(LVFun.SelectedItems[0].SubItems[2].Text); 
                fData.editFunCode.Focus();
                fData.editFunCode.Select();
                fData.editUSeqCode.SelectAll();
                fData.g_sKeyAll = g_sDefKeyAll;
                fData.g_sModiCode = LVFun.SelectedItems[0].Text;
                if (fData.ShowDialog() == DialogResult.OK)
                {
                    string sCode = fData.editFunCode.Text;
                    string sField = fData.combField.Text;                    
                    string sFunName = fData.combFunction.Text;                    
                    LVFun.SelectedItems[0].Text = sCode;
                    LVFun.SelectedItems[0].SubItems[1].Text = sField;
                    LVFun.SelectedItems[0].SubItems[2].Text = sFunName;                    
                    Show_DefKey();
                }
            }
            finally
            {
                fData.Dispose();
            }
        }

        private void MenuItemDeleteFun_Click(object sender, EventArgs e)
        {
            if (LVFun.SelectedItems.Count > 0)
            {
                LVFun.SelectedItems[0].Remove();
                Show_DefKey();
            }
        }
//Function End====================================================================
        public void Show_DefKey()
        {
            //紀錄已經有定義的代碼,不可以重複定義
            g_sDefKeyAll = g_sDefKey;

            for (int j = 0; j < LVFun.Items.Count; j++)
            {
                g_sDefKeyAll = g_sDefKeyAll + LVFun.Items[j].Text;
            }

            for (int j = 0; j < LVUserSeq.Items.Count; j++)
            {
                g_sDefKeyAll = g_sDefKeyAll + LVUserSeq.Items[j].Text;
            }
        }

        private void LVRule_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LVRule.SelectedItems.Count <= 0)
                return;            
            editRuleName.Text = LVRule.SelectedItems[0].Text;
            g_sRuleID = LVRule.SelectedItems[0].Tag.ToString();
            Show_RuleData(g_sRuleID);
        }       

        private void btnSave_Click(object sender, EventArgs e)
        {             
            if (editRuleName.Text == "")
             {
                 SajetCommon.Show_Message("Rule Name is Empty", 0);
                 editRuleName.Focus();
                 return;
             }
             if (editRuleCode.Text == "")
             {
                 SajetCommon.Show_Message("Rule Code is Empty", 0);
                 editRuleCode.Focus();
                 return;
             }

             /*
             若MaskedTextBox最後字元為空白,Text會移除此空白,因此改TextMaskFormat屬性為IncludePrompt 
             屬性改IncludePrompt後,會把沒輸入的字元當成'_',因此先Replace 
             */
             g_sDef = editDefault.Text;
             g_sDef = g_sDef.Replace('_', ' ');

             string sRule = editRuleName.Text.Trim();
             string sRuleCode = editRuleCode.Text;             
             
            //因為RuleName會組成SEQ的名稱,因此不可為特殊符號
             string sCharacter = "!@%^&*()-+={}[]\\|~`\"';:?/><,.";
             for (int i = 0; i <= sRule.Length-1; i++)
             {
                 if (sCharacter.IndexOf(sRule[i].ToString()) != -1 || sRule[i].ToString()==" ")
                 {
                     SajetCommon.Show_Message("Rule Name Fail (Invalid Character : " + sRule[i].ToString() + ")", 0);
                     editRuleName.Focus();
                     return;
                 }
             }

             string sMessage = "";
             int iY = 0; int iM = 0; int iD = 0; int iW = 0; int iK = 0; int iF = 0;
             int im = 0; int id = 0; int iw = 0; int ik = 0; int iX = 0;
             try
             {
                 //檢查Code是否正確======
                 for (int i = 1; i <= sRuleCode.Length; i++)
                 {
                     string sCode = sRuleCode[i-1].ToString();
                     //Code中有未定義的字元
                     if (g_sDefKeyAll.IndexOf(sCode) == -1)
                     {
                         sMessage = "Rule Code Error (" + sCode + ")";
                         return;
                     }
                     //檢查日期格式是否正確
                     switch (sCode)
                     {
                         case "Y":
                             iY++;
                             continue;
                         case "M":
                             iM++;
                             continue;
                         case "D":
                             iD++;
                             continue;
                         case "W":
                             iW++;
                             continue;
                         case "K":
                             iK++;
                             continue;
                         case "F":
                             iF++;
                             continue;
                         case "m":
                             im++;
                             if (editMonth.Text == "")
                             {
                                 sMessage = "Please define \"Month Code\"";
                                 return;
                             }
                             continue;
                         case "d":
                             id++;
                             if (editDay.Text == "")
                             {
                                 sMessage = "Please define \"Day Code\"";
                                 return;
                             }
                             continue;
                         case "w":
                             iw++;
                             if (editWeek.Text == "")
                             {
                                 sMessage = "Please define \"Week Code\"";
                                 return;
                             }
                             continue;
                         case "k":
                             ik++;
                             if (editWeekYear.Text == "")
                             {
                                 sMessage = "Please define \"Day of Week Code\"";
                                 return;
                             }
                             continue;
                         case "X":
                             iX++;
                             if (combCheckSum.Text == "")
                             {
                                 sMessage = "Please define \"Check Sum\"";
                                 return;
                             }
                             continue;
                         case "S":
                             if (!rbCarry10.Checked && !rbCarry16.Checked)
                             {
                                 sMessage = "Please define \"Sequence(10 or 16)\"";
                                 return;
                             }
                             continue;
                     }
                 }
                 //檢查日期Code的長度是否超過                 
                 if (iY > 4)
                     sMessage = sMessage + "Y: Length Error (Max: 4)" + Environment.NewLine;
                 if (iM > 2)
                     sMessage = sMessage + "M: Length Error (Max: 2)" + Environment.NewLine;
                 if (iD != 2 && iD != 0)
                     sMessage = sMessage + "D: Length Error (Length: 2)" + Environment.NewLine;
                 if (iW != 2 && iW != 0)
                     sMessage = sMessage + "W: Length Error (Length: 2)" + Environment.NewLine;
                 if (iK > 1)
                     sMessage = sMessage + "K: Length Error (Length: 1)" + Environment.NewLine;
                 if (iF != 3 && iF != 0)
                     sMessage = sMessage + "F: Length Error (Length: 3)" + Environment.NewLine;
                 if (iX > 1)
                     sMessage = sMessage + "X: Length Error (Length: 1)" + Environment.NewLine;
                 if (im > 0)
                 {
                     int iLen = editMonth.Text.IndexOf(",");
                     if (im != iLen)
                         sMessage = sMessage + "m: Length Error (Length: " + iLen.ToString() + ")" + Environment.NewLine;
                 }
                 else
                     editMonth.Text = "";
                 if (id > 0)
                 {
                     int iLen = editDay.Text.IndexOf(",");
                     if (id != iLen)
                         sMessage = sMessage + "d: Length Error (Length: " + iLen.ToString() + ")" + Environment.NewLine;
                 }
                 else
                     editDay.Text = "";
                 if (iw > 0)
                 {
                     int iLen = editWeek.Text.IndexOf(",");
                     if (iw != iLen)
                         sMessage = sMessage + "w: Length Error (Length: " + iLen.ToString() + ")" + Environment.NewLine;
                 }
                 else
                     editWeek.Text = "";
                 if (ik > 0)
                 {
                     int iLen = editWeekYear.Text.IndexOf(",");
                     if (ik != iLen)
                         sMessage = sMessage + "k: Length Error (Length: " + iLen.ToString() + ")" + Environment.NewLine;
                 }
                 else
                     editWeekYear.Text = "";

                 if (sMessage != "")
                 {
                     editRuleCode.Focus();
                     return;
                 }
             }
             finally
             {
                 if (sMessage != "")
                 {
                     SajetCommon.Show_Message(sMessage, 0);
                     editRuleCode.Focus();
                 }
             }

             SaveRuleData();
             UpdateWO(sRule, g_sRuleType.ToUpper() + " RULE");

             
             SajetCommon.Show_Message("Save OK", -1);
             Show_RuleName();
             ListViewItem LVItem = LVRule.FindItemWithText(sRule);
             if (LVItem != null)
                 LVItem.Selected = true;
             LVRule.Focus();
             Show_RuleData(g_sRuleID);
        }
        private void UpdateWO(string sRule,string sRuleType)
        {
            sSQL = "SELECT A.WORK_ORDER ,C.PART_NO,A.TARGET_QTY,A.INPUT_QTY,A.OUTPUT_QTY,A.WO_RULE  "
                 + "  FROM (SELECT WORK_ORDER FROM SAJET.G_WO_PARAM "
                 + "         WHERE MODULE_NAME =:MODULE_NAME "
                 + "           AND FUNCTION_NAME =:FUNCTION_NAME "
                 + "         GROUP BY WORK_ORDER ) B "
                 + "       ,SAJET.G_WO_BASE A "
                 + "       ,SAJET.SYS_PART C "
                 + " WHERE  A.WORK_ORDER = B.WORK_ORDER "
                 + "   AND A.WO_STATUS <>'6' "
                 + "   AND A." + g_sPartField + " = C.PART_ID(+) "
                 + " ORDER BY A.WORK_ORDER ";
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MODULE_NAME", sRuleType };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FUNCTION_NAME", sRule};
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count == 0)
                return;

            fWOData fMaintain = new fWOData();
            try
            {
                fMaintain.lablRuleName.Text = sRule;
                fMaintain.lablType.Text = sRuleType;
                for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
                {
                    DataRow dr = dsTemp.Tables[0].Rows[i];
                    fMaintain.dgvSNList.Rows.Add();
                    fMaintain.dgvSNList.Rows[fMaintain.dgvSNList.Rows.Count - 1].Cells["WORK_ORDER"].Value = dr["WORK_ORDER"].ToString(); 
                    fMaintain.dgvSNList.Rows[fMaintain.dgvSNList.Rows.Count - 1].Cells["PART_NO"].Value = dr["PART_NO"].ToString();
                    fMaintain.dgvSNList.Rows[fMaintain.dgvSNList.Rows.Count - 1].Cells["WO_RULE"].Value = dr["WO_RULE"].ToString();
                    fMaintain.dgvSNList.Rows[fMaintain.dgvSNList.Rows.Count - 1].Cells["TARGET_QTY"].Value = dr["TARGET_QTY"].ToString(); 
                    fMaintain.dgvSNList.Rows[fMaintain.dgvSNList.Rows.Count - 1].Cells["INPUT_QTY"].Value = dr["INPUT_QTY"].ToString(); 
                    fMaintain.dgvSNList.Rows[fMaintain.dgvSNList.Rows.Count - 1].Cells["OUTPUT_QTY"].Value = dr["OUTPUT_QTY"].ToString(); 
                    fMaintain.dgvSNList.Rows[fMaintain.dgvSNList.Rows.Count - 1].Cells["CHECKED"].Value = "N";
                }
                fMaintain.ShowDialog();
            }
            finally
            {
                fMaintain.Dispose();
            }
        }

        public void SaveRuleData()
        {
            sSQL = " Select to_char(sysdate,'yyyy/mm/dd hh24:mi:ss') systime from dual ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            string sSysdate = dsTemp.Tables[0].Rows[0]["systime"].ToString();

            sSQL = "DELETE SAJET.SYS_RULE_PARAM "
                 + "Where RULE_ID = '" + g_sRuleID + "' ";                 
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            SavetoDB(g_sRuleType + " Code", "Code", editRuleCode.Text, sSysdate);
            SavetoDB(g_sRuleType + " Code", "Default", g_sDef, sSysdate);
            //User Define Day Code
            if (editMonth.Text != "")
                SavetoDB("Month User Define", "Month", editMonth.Text, sSysdate);
            if (editDay.Text != "")
                SavetoDB("Day User Define", "Day", editDay.Text, sSysdate);
            if (editWeek.Text != "")
                SavetoDB("Week User Define", "Week", editWeek.Text, sSysdate);
            if (editWeekYear.Text != "")
                SavetoDB("Day of Week User Define", "Week", editWeekYear.Text, sSysdate);
            if (combCheckSum.Text != "")
                SavetoDB("Check Sum", "Function", combCheckSum.Text, sSysdate);
            //Function
            for (int i = 0; i <= LVFun.Items.Count - 1; i++)
            {
                string sParamName = LVFun.Items[i].Text+"-Digit Type & Field";
                string sParamItem = LVFun.Items[i].SubItems[1].Text;
                string sParamValue = LVFun.Items[i].SubItems[2].Text;
                SavetoDB(sParamName, sParamItem, sParamValue, sSysdate);
            }
            //Sequence  
            string S = "";
            if (rbCarry16.Checked)
                S = "16";
            else if (rbCarry10.Checked)
                S = "10";
            if (S != "")
                SavetoDB(g_sRuleType + " Code", "Code Type", S, sSysdate);
            //User Define Sequence 
            for (int i = 0; i <= LVUserSeq.Items.Count - 1; i++)
            {
                string sCode = LVUserSeq.Items[i].Text;
                string sSeq = LVUserSeq.Items[i].SubItems[1].Text;
                SavetoDB(g_sRuleType + " User Define", sCode, sSeq, sSysdate);
            }
            //Reset Sequence
            int j = 3;
            for (int i = 0; i < sArray_ResetType.Length; i++)
            {
                if (combResetType.Text == sArray_ResetType[i].ToString())
                {
                    j = i;
                    break;
                }
            }            
            if (chkbReset.Checked)
                SavetoDB("Reset Sequence", "1", j.ToString(), sSysdate);
            else
                SavetoDB("Reset Sequence", "0", j.ToString(), sSysdate);

            //Sequence Mode
            if (rbNoSeq.Checked)
                SavetoDB("Sequence Mode", "", "Manual", sSysdate);
            else
                SavetoDB("Sequence Mode", "", "Auto", sSysdate);
        }

        public void SavetoDB(string ParamName, string ParamItem, string ParamValue, string sDate)
        {
            sSQL = " INSERT INTO SAJET.SYS_RULE_PARAM "
                 + " (RULE_ID,PARAME_NAME,PARAME_ITEM,PARAME_VALUE,UPDATE_USERID,UPDATE_TIME)"
                 + " VALUES "
                 + " ('" + g_sRuleID + "','" + ParamName + "','" + ParamItem + "','" + ParamValue + "','" + g_sUserID + "',to_date('" + sDate + "','yyyy/mm/dd hh24:mi:ss'))";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);            
        }

        private void combResetType_DropDown(object sender, EventArgs e)
        {            
            string sTemp1 = combResetType.Text;
            string sRuleCode = editRuleCode.Text.ToUpper();
            combResetType.Items.Clear();
            if (sRuleCode.IndexOf("D") != -1 || sRuleCode.IndexOf("K") != -1)
                combResetType.Items.Add(sArray_ResetType[0].ToString());
            if (sRuleCode.IndexOf("W") != -1 )
                combResetType.Items.Add(sArray_ResetType[1].ToString());
            if (sRuleCode.IndexOf("M") != -1)
                combResetType.Items.Add(sArray_ResetType[2].ToString());
            if (sRuleCode.IndexOf("Y") != -1)
                combResetType.Items.Add(sArray_ResetType[3].ToString());

            combResetType.SelectedIndex = combResetType.FindString(sTemp1);
        }

        public void Show_ResetSeq(string sRuleName)
        {
            btnResetSeq.Visible = false;
            lablSeq.Visible = btnResetSeq.Visible;            
            //顯示使用的Sequence及手動Reset按鈕            
            string sRule = sRuleName.ToUpper();
            if (gb_ResetSeq)
            {                                
                fReset.LVReset.Items.Clear();
                sSQL = "select sequence_name, Last_Number from all_sequences "
                     + "where sequence_name like '" + g_sSeq + sRule + "%' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)                
                {
                    btnResetSeq.Visible = true;
                    string sSeqName = dsTemp.Tables[0].Rows[i]["sequence_name"].ToString();
                    string sLastNumber = dsTemp.Tables[0].Rows[i]["Last_Number"].ToString();
                    string sItem = sSeqName.Substring(g_sSeq.Length , sSeqName.Length - g_sSeq.Length);                    
                    if (dsTemp.Tables[0].Rows.Count == 1 && sRule == sItem)
                    {
                        lablSeq.Text = "(Sequence: " + sLastNumber + ")";
                        lablSeq.Visible = true;
                    }
                    else
                    {                                                                       
                        //同一Rule有多個Sequence (有空白的填值會產生不同的Sequence, 原Sequence_填入的字值)
                        string s = sSeqName.Replace(g_sSeq + sRule, "");//取空白規則自行輸入的值
                        s = s.Replace("_", "").Trim();
                        fReset.LVReset.Items.Add(s);
                        fReset.LVReset.Items[fReset.LVReset.Items.Count - 1].SubItems.Add(sLastNumber);                        
                    }
                }
            }            
        }

        private void btnResetSeq_Click(object sender, EventArgs e)
        {            
            if (fReset.LVReset.Items.Count == 0)
            {
                string sSeqName = g_sSeq + editRuleName.Text.ToUpper();
                if (SajetCommon.Show_Message("Reset Sequence ? " + Environment.NewLine + editRuleName.Text, 2) != DialogResult.Yes)
                    return;
                sSQL = "drop sequence " + sSeqName;
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                btnResetSeq.Visible = false;
                lablSeq.Visible = false;
            }
            else
            {
                //Reset 多筆Sequence
                fReset.gsSeqName = g_sSeq + editRuleName.Text.ToUpper();
              //  fReset.sServerName = SajetCommon.g_sServerName; ;
                if (fReset.ShowDialog() == DialogResult.OK)
                    Show_ResetSeq(editRuleName.Text);

            }
        }        

        private void editRuleCode_KeyUp(object sender, KeyEventArgs e)
        {            
            editDefault.Mask = "";           
            string sMask = "";
            string sDefReplace = "";
            string sDefault = editDefault.Text;
            
            int iKeyStart = editRuleCode.SelectionStart; //滑鼠目前在第幾碼
            //目前輸入的是屬於中間的位數
            if (iKeyStart < editRuleCode.Text.Length)
            {
                if (sDefault.Length < editRuleCode.Text.Length)
                {
                    //Code從中加入一碼,所以需在Default相同位置先插入一空白字元
                    sDefault = sDefault.Insert(iKeyStart - 1, " ");
                }
                else if (sDefault.Length > editRuleCode.Text.Length)
                {
                    //Code從中刪除一碼,所以需在Default先刪除相同位置的字元
                    sDefault = sDefault.Remove(iKeyStart,1);
                }
            }
            sMask = Get_Mask(editRuleCode.Text, sDefault, ref sDefReplace);           
            
            editDefault.Mask = sMask;
            editDefault.Text = sDefReplace;            
            
            //Reset Sequence           
            combResetType.Items.Clear();
            for (int i = 0; i < sArray_ResetType.Length; i++)
                combResetType.Items.Add(sArray_ResetType[i].ToString());

            string sRuleCode = editRuleCode.Text.ToUpper();
            if (sRuleCode.IndexOf("D") != -1 || sRuleCode.IndexOf("K") != -1)
                combResetType.SelectedIndex = 0;
            else if (sRuleCode.IndexOf("W") != -1)
                combResetType.SelectedIndex = 1;
            else if (sRuleCode.IndexOf("M") != -1)
                combResetType.SelectedIndex = 2;
            else if (sRuleCode.IndexOf("Y") != -1)
                combResetType.SelectedIndex = 3;             
        }

        private string Get_Mask(string sCode, string sDefault,ref string sDefReplace)
        {
            //設定Mask值 
            string sMask = "";
            sDefReplace = sCode;                      
            for (int i = sCode.Length - 1; i >= 0; i--)
            {               
                if (sCode[i].ToString() == "9" || sCode[i].ToString() == "L" || sCode[i].ToString() == "C")
                {
                    sMask = sCode[i] + sMask;
                    if (sDefault.Length - 1 >= i)
                    {
                        sDefReplace = sDefReplace.Remove(i, 1);
                        sDefReplace = sDefReplace.Insert(i, sDefault[i].ToString());
                    }
                    else
                    {
                        sDefReplace = sDefReplace.Remove(i, 1);
                        sDefReplace = sDefReplace.Insert(i, " ");
                    }                                        
                }
                else
                {
                    sMask = "L" + sMask;
                }
            }              
            return sMask;
        }        
        
        private void editRuleName_KeyPress(object sender, KeyPressEventArgs e)
        {
            Clear_Data();
            btnResetSeq.Visible = false;
            lablSeq.Visible = btnResetSeq.Visible;
            if (e.KeyChar == (char)Keys.Return)
            {
                Show_RuleData(g_sRuleID);               
            }
        }

        private void MenuItemModifyRule_Click(object sender, EventArgs e)
        {
            if (LVRule.SelectedItems.Count <= 0)
                return;
            string sRuleID = LVRule.SelectedItems[0].Tag.ToString();
            fModifyRule fMaintain = new fModifyRule();
            try
            {
                string sOldRuleName = LVRule.SelectedItems[0].Text;
                
                fMaintain.g_sUpdateRuleID = sRuleID;
                fMaintain.g_MaintainType = "Modify";
                fMaintain.combRule.Enabled = false;
                fMaintain.editRuleName.Text = LVRule.SelectedItems[0].Text;
                fMaintain.editRuleDesc.Text = LVRule.SelectedItems[0].SubItems[1].Text;

                if (g_sRuleType.ToUpper() != "MAC")
                {
                    fMaintain.labGroupQty.Visible = false;
                    fMaintain.LabSafetyStock.Visible = false;
                    fMaintain.editGroupQty.Visible = false;
                    fMaintain.editSafetyStock.Visible = false;
                }
                else
                {
                    fMaintain.editGroupQty.Text = LVRule.SelectedItems[0].SubItems[2].Text;
                    fMaintain.editSafetyStock.Text = LVRule.SelectedItems[0].SubItems[3].Text;
                }

                if (fMaintain.ShowDialog() == DialogResult.OK)
                {
                    string sRuleName = fMaintain.editRuleName.Text;
                    string sRuleDesc = fMaintain.editRuleDesc.Text;

                    string sGroupQty = fMaintain.editGroupQty.Text;
                    string sSafetyStock = fMaintain.editSafetyStock.Text;
                    //更改Rule Set中有用到此Rule的名稱
                    sSQL = " Update SAJET.SYS_MODULE_PARAM "
                         + " Set parame_item = '" + sRuleName + "' "
                         + " where module_name = 'W/O RULE' "
                         + " and parame_name = '" + g_sRuleType + " Rule' "
                         + " and parame_item = '" + sOldRuleName + "' ";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);

                    //先展範圍,如MAC
                    if (g_sLabType.ToUpper() == "S")
                    {
                        sSQL = " Update  " + g_sTable
                          + " Set rule_name = '" + sRuleName + "' "                         
                          + " where rule_name = '" + sOldRuleName + "' ";
                        dsTemp = ClientUtils.ExecuteSQL(sSQL);

                        sSQL = " Update "+g_sTable+"_create "
                          + " Set rule_name = '" + sRuleName + "' "
                          + " where rule_name = '" + sOldRuleName + "' ";
                        dsTemp = ClientUtils.ExecuteSQL(sSQL);
                    }


                    sSQL = "Update SAJET.SYS_RULE_NAME "
                         + "Set Rule_Name = '" + sRuleName + "' "
                         + "   ,RULE_DESC = '" + sRuleDesc + "' "
                         + "   ,Update_UserID = '" + g_sUserID + "' "
                         + "   ,Update_Time = SYSDATE ";
                    if (g_sRuleType.ToUpper() == "MAC")
                    {
                        sSQL = sSQL
                             + "   ,GROUP_QTY = '" + sGroupQty + "' "
                             + "   ,SAFETY_STOCK = '" + sSafetyStock + "' ";
                    }
                    sSQL = sSQL
                         + "Where RULE_ID = '" + sRuleID + "' ";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);
                    LVRule.SelectedItems[0].Text = sRuleName;
                    LVRule.SelectedItems[0].SubItems[1].Text = sRuleDesc;
                    LVRule.SelectedItems[0].SubItems[2].Text = sGroupQty;
                    LVRule.SelectedItems[0].SubItems[3].Text = sSafetyStock;
                    editRuleName.Text = sRuleName;                    
                }
            }
            finally
            {
                fMaintain.Dispose();
            }
        }

        private void MenuItemDeleteRule_Click(object sender, EventArgs e)
        {
            if (LVRule.SelectedItems.Count <= 0)
                return;
            string sRule = LVRule.SelectedItems[0].Text;
            string sRuleID = LVRule.SelectedItems[0].Tag.ToString();

            if (SajetCommon.Show_Message("Delete this Rule ?" + Environment.NewLine + sRule, 2) == DialogResult.Yes)
            {
                sSQL = " DELETE SAJET.SYS_RULE_NAME "
                     + " Where RULE_ID = '" + sRuleID + "' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);

                sSQL = " DELETE SAJET.SYS_RULE_PARAM "
                     + " Where RULE_ID = '" + sRuleID + "' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);

                LVRule.SelectedItems[0].Remove();
                if (LVRule.Items.Count > 0)
                {
                    LVRule.Items[0].Focused = true;
                    LVRule.Items[0].Selected = true;
                }
            }
        }

        private void MenuItemAddRule_Click(object sender, EventArgs e)
        {
            fModifyRule fMaintain = new fModifyRule();
            try
            {
                fMaintain.g_sUpdateRuleID = "0";
                fMaintain.g_MaintainType = "Append";
                fMaintain.combRule.Enabled = true;
                if (g_sRuleType.ToUpper() != "MAC")
                {
                    fMaintain.labGroupQty.Visible = false;
                    fMaintain.LabSafetyStock.Visible = false;
                    fMaintain.editGroupQty.Visible = false;
                    fMaintain.editSafetyStock.Visible = false;
                }

                object[][] Params = new object[1][];               
                sSQL = " Select * "
                     + " From SAJET.SYS_RULE_NAME "
                     + " Where RULE_TYPE = :RULE_TYPE " 
                     + " Order By RULE_NAME ";
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RULE_TYPE", g_sRuleType.ToUpper() };
                dsTemp = ClientUtils.ExecuteSQL(sSQL,Params);
                fMaintain.combRule.Items.Add("");
                for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
                {
                    fMaintain.combRule.Items.Add(dsTemp.Tables[0].Rows[i]["RULE_NAME"].ToString());
                }

                if (fMaintain.ShowDialog() == DialogResult.OK)
                {
                    string sRuleName = fMaintain.editRuleName.Text;
                    string sRuleDesc = fMaintain.editRuleDesc.Text;
                    string sCopyRuleName = fMaintain.combRule.Text;

                    string sGroupQty = fMaintain.editGroupQty.Text;
                    string sSafetyStock = fMaintain.editSafetyStock.Text;

                    string sRuleID = GetMaxID();
                    if (sRuleID == "0" || sRuleID == "")
                        return;
                    sSQL = "Insert Into SAJET.SYS_RULE_NAME "
                         + "(RULE_ID,RULE_TYPE,RULE_NAME,RULE_DESC,Update_UserID,Update_Time "
                         + ",Group_Qty,Safety_Stock) "
                         + "Values ('" + sRuleID + "','" + g_sRuleType.ToUpper() + "','" + sRuleName + "','" + sRuleDesc + "','" + g_sUserID + "',SYSDATE "
                         + ",'" + sGroupQty + "','" + sSafetyStock + "' "
                         + ") ";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);

                    if (!string.IsNullOrEmpty(sCopyRuleName))
                    {
                        sSQL = " Select RULE_ID "
                             + " From SAJET.SYS_RULE_NAME "
                             + " Where RULE_TYPE = '" + g_sRuleType.ToUpper() + "' "
                             + "   and RULE_NAME ='" + sCopyRuleName + "' ";
                        dsTemp = ClientUtils.ExecuteSQL(sSQL);
                        if (dsTemp.Tables[0].Rows.Count > 0)
                        {
                            string sCopyRuleID = dsTemp.Tables[0].Rows[0]["RULE_ID"].ToString();
                            sSQL = " INSERT INTO SAJET.SYS_RULE_PARAM "
                                 + " (RULE_ID,PARAME_NAME,PARAME_ITEM,PARAME_VALUE,UPDATE_USERID,UPDATE_TIME)"
                                 + " SELECT :RULE_ID,PARAME_NAME,PARAME_ITEM,PARAME_VALUE,:UPDATE_USERID,SYSDATE "
                                 + "  FROM SAJET.SYS_RULE_PARAM "
                                 + "  WHERE RULE_ID =:COPY_RULE_ID ";
                            Params = new object[3][];
                            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RULE_ID", sRuleID };
                            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "COPY_RULE_ID", sCopyRuleID };
                            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                        }
                        Show_RuleData(sRuleID);
                    }
                    Show_RuleName();
                    ListViewItem LVItem = LVRule.FindItemWithText(sRuleName, true, 0);
                    LVItem.Focused = true;
                    LVItem.Selected = true;
                }
            }
            finally
            {
                fMaintain.Dispose();
            }
        }

        private string GetMaxID()
        {
            try
            {
                object[][] Params = new object[5][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TFIELD", "RULE_ID" };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TTABLE", "SAJET.SYS_RULE_NAME" };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TNUM", "9" };
                Params[3] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                Params[4] = new object[] { ParameterDirection.Output, OracleType.VarChar, "T_MAXID", "" };
                DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_GET_MAXID", Params);

                string sRes = ds.Tables[0].Rows[0]["TRES"].ToString();
                if (sRes != "OK")
                {
                    SajetCommon.Show_Message(sRes, 0);
                    return "0";
                }

                return ds.Tables[0].Rows[0]["T_MAXID"].ToString();
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);
                return "0";
            }
        }

        private void rbNoSeq_CheckedChanged(object sender, EventArgs e)
        {
            if (rbNoSeq.Checked)
            {
                chkbReset.Checked = false;
                chkbReset.Enabled = false;                
            }
            else
            {
                chkbReset.Enabled = true;  
            }
            combResetType.Enabled = chkbReset.Enabled;
        }           
    }
}

