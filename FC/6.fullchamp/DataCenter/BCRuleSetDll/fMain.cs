using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Globalization;
using Microsoft.Win32;

namespace BCRuleSetDll
{
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();
        }              

        public int g_iPrivilege = 0;
        public string g_sUserID;
        public string g_sProgram;
        public static string g_sExeName;
        string sSQL;
        DataSet dsTemp;
        //public static DataGridView dvGrid; 
        public void check_privilege()
        {
            btnSave.Enabled = false;
            g_iPrivilege = SajetCommon.Get_Privilege(g_sUserID, g_sExeName, out g_sProgram);            
            btnSave.Enabled = (g_iPrivilege >= 1);                      
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            g_sExeName = ClientUtils.fCurrentProject;
            ClientUtils.SetLanguage(this, g_sExeName);
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
            panel1.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");

            this.Text = this.Text + " (" + SajetCommon.g_sFileVersion + ")";   

            g_sUserID = ClientUtils.UserPara1;
            check_privilege(); 
            
            //所有Label Type及此Type有定義的Rule
            Show_LabelRule();            
            Show_RuleName();

            editRuleName.Focus();
        }

        public void Show_LabelRule()
        {
            string sSQL = "select label_name from sajet.sys_label "
                        + "where Type <> 'U' "
                        + "order by label_name ";
            dsTemp = SajetCommon.ExecuteSQL(sSQL);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                string sLabelType = dsTemp.Tables[0].Rows[i]["label_name"].ToString();
                dvGrid.Rows.Add();                
                dvGrid.Rows[dvGrid.Rows.Count - 1].Cells[0].Value = sLabelType;

                //每種Label有定義的RULE
                sSQL = " Select RULE_NAME "
                     + " From SAJET.SYS_RULE_NAME "
                     + " Where RULE_TYPE = '" + sLabelType.ToUpper() + "' "                     
                     + " Order By RULE_NAME ";
                DataSet dsTemp1 = SajetCommon.ExecuteSQL(sSQL);
                DataGridViewComboBoxCell dvGridCombCell = new DataGridViewComboBoxCell();
                dvGridCombCell.Items.Clear();
                dvGridCombCell.Items.Add(" ");
                for (int j = 0; j <= dsTemp1.Tables[0].Rows.Count - 1; j++)
                {
                    dvGridCombCell.Items.Add(dsTemp1.Tables[0].Rows[j]["RULE_NAME"].ToString());                   
                }                                              
                dvGrid.Rows[dvGrid.Rows.Count - 1].Cells[1] = dvGridCombCell;
                
            }
        }

        private void dvGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {            
            if (e.ColumnIndex == 1 && e.RowIndex > 0)
            {
                string sCode = this.dvGrid.Rows[e.RowIndex].Cells[1].Value.ToString();
                string sType = this.dvGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
                this.dvGrid.Rows[e.RowIndex].Cells[2].Value = Show_RuleDefault(sType.ToUpper(), sCode, sType + " Code");
            }             
        }

        public void Show_RuleName()
        {                        
            //顯示所有Rule Name
            chkbCust.Checked = false;
            chkbPo.Checked = false;
            chkbMasterWo.Checked = false;
            chkbRemark.Checked = false;
            chkbSo.Checked = false;
            chkbWoType.Checked = false;
            chkbLine.Checked = false;
            editRuleName.Text = "";

            string sFilter = editRuleFilter.Text.Trim().ToUpper();
            LVRule.Items.Clear();

            string sSQL = "Select FUNCTION_NAME "
                        + " From SAJET.SYS_MODULE_PARAM "
                        + " Where MODULE_NAME = 'W/O RULE' ";    
            if (sFilter != "" && sFilter != "%")
                sSQL = sSQL + " and UPPER(FUNCTION_NAME) like '" + sFilter + "%'";
            sSQL = sSQL + "Group By FUNCTION_NAME "
                        + "Order By FUNCTION_NAME ";
            dsTemp = SajetCommon.ExecuteSQL(sSQL);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1;i++ )
            {
                LVRule.Items.Add(dsTemp.Tables[0].Rows[i]["FUNCTION_NAME"].ToString());
                LVRule.Items[i].ImageIndex = 0;
            }

            for (int i = 0; i <= dvGrid.Rows.Count-1; i++)
            {
                dvGrid.Rows[i].Cells[1].Value = "";
                dvGrid.Rows[i].Cells[2].Value = "";
            }
        }

        public void Show_RuleData(string sRule)
        {
            //顯示Rule的各項設定資料            
            dvGrid.RefreshEdit();           
            string sSQL = " Select * "
                        + " From SAJET.SYS_MODULE_PARAM "
                        + " Where FUNCTION_NAME = '" + sRule + "' "
                        + " and MODULE_NAME = 'W/O RULE' ";
            DataSet dsTempData = SajetCommon.ExecuteSQL(sSQL);
            for (int i = 0; i <= dsTempData.Tables[0].Rows.Count - 1; i++)
            {
                string sParamName = dsTempData.Tables[0].Rows[i]["PARAME_NAME"].ToString();
                string sParamItem = dsTempData.Tables[0].Rows[i]["PARAME_ITEM"].ToString();
                string sParamValue = dsTempData.Tables[0].Rows[i]["PARAME_VALUE"].ToString();
                if (sParamName == "Necessary Information")
                {
                    switch (sParamItem)
                    {
                        case "Customer":
                            chkbCust.Checked = (sParamValue == "Y");
                            break;
                        case "PO Number":
                            chkbPo.Checked = (sParamValue == "Y");
                            break;
                        case "Master Work Order":
                            chkbMasterWo.Checked = (sParamValue == "Y");
                            break;
                        case "Remark":
                            chkbRemark.Checked = (sParamValue == "Y");
                            break;
                        case "Sales Order":
                            chkbSo.Checked = (sParamValue == "Y");
                            break;
                        case "WO Type":
                            chkbWoType.Checked = (sParamValue == "Y");
                            break;
                        case "Line":
                            chkbLine.Checked = (sParamValue == "Y");
                            break;
                    }
                }
                else
                {
                    for (int j = 0; j <= dvGrid.Rows.Count - 1; j++)
                    {
                        string sLabelType = dvGrid.Rows[j].Cells[0].Value.ToString();
                        if (sLabelType + " Rule" == sParamName)
                        {                                                      
                            dvGrid.Rows[j].Cells[1].Value = sParamItem;
                            
                            if (sParamItem == "")
                                dvGrid.Rows[j].Cells[2].Value = "";
                            else
                                dvGrid.Rows[j].Cells[2].Value = Show_RuleDefault(sLabelType.ToUpper(), sParamItem, sLabelType + " Code");                                                             
                            break;
                        }                        
                    }
                }
            }                         
        }

        public string Show_RuleDefault(string RuleType,string RuleName, string ParamName)
        {
            string sSQL = " Select a.PARAME_VALUE "
                        + " From SAJET.SYS_RULE_PARAM a,sajet.sys_rule_name b"
                        + " Where b.RULE_TYPE = '" + RuleType + "' "
                        + " and b.RULE_NAME = '" + RuleName + "' "
                        + " and a.rule_id = b.rule_id "
                        + " and a.PARAME_NAME = '" + ParamName + "' "
                        + " and a.PARAME_ITEM = 'Default' ";
            dsTemp = SajetCommon.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count>=1)
                return dsTemp.Tables[0].Rows[0]["PARAME_VALUE"].ToString();
            else
                return "";
        }

        private void editRuleFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
                Show_RuleName();
        }                 
        
        private void LVRule_Click(object sender, EventArgs e)
        {
            if (LVRule.SelectedItems[0] == null)
                return;
            Clear_Data();
            editRuleName.Text = LVRule.SelectedItems[0].Text;
            Show_RuleData(editRuleName.Text);           
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (editRuleName.Text == "")
            {
                SajetCommon.Show_Message("Rule Name is Empty", 0);
                editRuleName.Focus();
                return;
            }

            /*
            bool bFlag = false;
            for (int i = 0; i <= dvGrid.Rows.Count - 1; i++)
            {
                string sData = dvGrid.Rows[i].Cells[1].Value.ToString().Trim();
                if (!string.IsNullOrEmpty(sData))
                {
                    bFlag = true;
                    break;
                }
            }
            if (!bFlag)
            {
                SajetCommon.Show_Message("Please Define Label Rule", 0);
                return;
            }
            */
            string sRule = editRuleName.Text;
            SaveRuleData(sRule);
            SajetCommon.Show_Message("Save OK", -1);

            Show_RuleName();
            if (LVRule.FindItemWithText(sRule) != null)
                LVRule.FindItemWithText(sRule).Selected = true;
            LVRule.Focus();
            editRuleName.Text = sRule;
            Show_RuleData(sRule);
        }

        public void SaveRuleData(string sRule)
        {
            sSQL = " Select to_char(sysdate,'yyyy/mm/dd hh24:mi:ss') systime from dual ";
            dsTemp = SajetCommon.ExecuteSQL(sSQL);
            string sSysdate = dsTemp.Tables[0].Rows[0]["systime"].ToString();

            sSQL = " Delete SAJET.SYS_MODULE_PARAM "
                 + " Where MODULE_NAME = 'W/O RULE' "
                 + " and FUNCTION_NAME = '" + sRule + "' ";
            dsTemp = SajetCommon.ExecuteSQL(sSQL);

            string S = "N";
            if (chkbCust.Checked)
                S = "Y";
            SavetoDB("Necessary Information", "Customer", S, sSysdate);

            S = "N";
            if (chkbPo.Checked) 
                S = "Y";           
            SavetoDB("Necessary Information", "PO Number", S, sSysdate);

            S = "N";
            if (chkbMasterWo.Checked)
                S = "Y";
            SavetoDB("Necessary Information", "Master Work Order", S, sSysdate);

            S = "N";
            if (chkbRemark.Checked)
                S = "Y";
            SavetoDB("Necessary Information", "Remark", S, sSysdate);

            S = "N";
            if (chkbSo.Checked)
                S = "Y";
            SavetoDB("Necessary Information", "Sales Order", S, sSysdate);

            S = "N";
            if (chkbWoType.Checked)
                S = "Y";
            SavetoDB("Necessary Information", "WO Type", S, sSysdate);

            S = "N";
            if (chkbLine.Checked)
                S = "Y";
            SavetoDB("Necessary Information", "Line", S, sSysdate);

            for (int i = 0 ;i<= dvGrid.Rows.Count - 1 ;i++)
            {                
                string sCode = dvGrid.Rows[i].Cells[1].Value.ToString().Trim();
                SavetoDB(dvGrid.Rows[i].Cells[0].Value.ToString() + " Rule", sCode, "", sSysdate);
            }
        }

        public void SavetoDB(string ParamName, string ParamItem, string ParamValue, string sDate)
        {
            sSQL = " INSERT INTO  SAJET.SYS_MODULE_PARAM "
                 + " (MODULE_NAME,FUNCTION_NAME,PARAME_NAME,PARAME_ITEM,PARAME_VALUE,UPDATE_USERID,UPDATE_TIME)"
                 + " VALUES "
                 + " ('W/O RULE','" + editRuleName.Text + "','" + ParamName + "','" + ParamItem + "','" + ParamValue + "','" + g_sUserID + "',to_date('" + sDate + "','yyyy/mm/dd hh24:mi:ss'))";
            dsTemp = SajetCommon.ExecuteSQL(sSQL);
        }                      
        
        private void MenuItemDeleteRule_Click(object sender, EventArgs e)
        {            
            if (LVRule.SelectedItems.Count <= 0)
                return;
            string sRule = LVRule.SelectedItems[0].Text;

            if (SajetCommon.Show_Message("Delete this Rule ?" + Environment.NewLine + sRule, 2) == DialogResult.Yes)
            {
                sSQL = " DELETE SAJET.SYS_MODULE_PARAM "
                     + " Where MODULE_NAME = 'W/O RULE' "
                     + " And FUNCTION_NAME = '" + sRule + "' ";
                dsTemp = SajetCommon.ExecuteSQL(sSQL);

                LVRule.SelectedItems[0].Remove();
            }            
        }        

        private void dvGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //避免Grid中的Combobox找不到值時會出現Exception
            e.Cancel = true;
        }

        public void Clear_Data()
        {
            //顯示所有Rule Name
            chkbCust.Checked = false;
            chkbPo.Checked = false;
            chkbMasterWo.Checked = false;
            chkbRemark.Checked = false;
            chkbSo.Checked = false;
            chkbWoType.Checked = false;
            chkbLine.Checked = false;

            for (int i = 0; i <= dvGrid.Rows.Count - 1; i++)
            {
                dvGrid.Rows[i].Cells[1].Value = "";
                dvGrid.Rows[i].Cells[2].Value = "";
            }
        }

        private void editRuleName_KeyPress(object sender, KeyPressEventArgs e)
        {
            Clear_Data();
            if (e.KeyChar == (char)Keys.Return)
            {
                Show_RuleData(editRuleName.Text); 
            }
        }

        private void btnAppend_Click(object sender, EventArgs e)
        {
            fRuleMainTain fRMT = new fRuleMainTain();
            try 
            {
                fRMT.g_sMaintainType = "Append";
                if (fRMT.ShowDialog() == DialogResult.OK)
                {
                    string sRulefRMT = fRMT.g_sRuleName;
                    editRuleName.Text = sRulefRMT;
                    SaveRuleData(sRulefRMT);
                    SajetCommon.Show_Message("Save OK", -1);
                    Show_RuleName();

                    
                    if (LVRule.FindItemWithText(sRulefRMT) != null)
                        LVRule.FindItemWithText(sRulefRMT).Selected = true;
                    LVRule.Focus();
                    editRuleName.Text = sRulefRMT;
                    Show_RuleData(sRulefRMT);
                }
            }
            finally 
            { 
                fRMT.Dispose(); 
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (LVRule.SelectedItems.Count <= 0 || LVRule.Items.Count==0)
                return;
            fRuleMainTain fRMT = new fRuleMainTain();
            try
            {               
                fRMT.g_sMaintainType = "Modify";
                fRMT.g_sRuleName = editRuleName.Text.Trim();
                if (fRMT.ShowDialog() == DialogResult.OK)
                {
                    Show_RuleName();
                }
            }
            finally 
            {
                fRMT.Dispose();
            }            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
             if (LVRule.SelectedItems.Count <= 0 || LVRule.Items.Count==0)
                return;
            string sRule = LVRule.SelectedItems[0].Text;
            if (SajetCommon.Show_Message("Delete this Rule ?" + Environment.NewLine + sRule, 2) == DialogResult.Yes)
            {
                sSQL = " DELETE SAJET.SYS_MODULE_PARAM "
                     + " WHERE FUNCTION_NAME = '" + sRule + "' "
                     + " AND MODULE_NAME = 'W/O RULE'";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                LVRule.SelectedItems[0].Remove();
            }
        }

        private void MenuItemModifyRule_Click(object sender, EventArgs e)
        {
            btnModify_Click(sender,e );
        }

        private void MenuItemAppendRule_Click(object sender, EventArgs e)
        {

        }

    }
}

