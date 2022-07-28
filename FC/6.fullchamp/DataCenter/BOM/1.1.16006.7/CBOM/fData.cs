using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using SajetFilter;

namespace CBOM
{
    public partial class fData : Form
    {
        public fData()
        {
            InitializeComponent();
        }
        
        public string g_sProcessID;
        public string g_sItemPartID,g_sItemPartType,g_sItemSpec1;              
        public bool g_sChangeGroup;
        public string g_sBOM_ID, g_sProcess, g_sPurchase;
        public string g_sPartNo, g_sVer;
        public string g_sUpdateType, g_sRowid;
        string sSQL;
        DataSet dsTemp;

        private void bbtnOK_Click(object sender, EventArgs e)
        {
            if (editSubPartNo.Text.Trim() == "")
            {
                string sData = LabSubPart.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editSubPartNo.Focus();
                return;
            }

            if (LabPart.Text == editSubPartNo.Text.Trim())
            {
                SajetCommon.Show_Message("Sub Part No = Main Part No", 0);
                editSubPartNo.Focus();
                return;
            }
            
            if (editQty.Text.Trim() == "0" | editQty.Text.Trim() == "")
            {
                SajetCommon.Show_Message("Qty Error", 0);
                editQty.Focus();
                editQty.SelectAll();
                return;
            }

            if (editSubPartVer.Text.Trim() == "")
                editSubPartVer.Text = "N/A";

            if (combProcess.Text.Trim() == "")
                g_sProcessID = "0";
            else
            {
                g_sProcessID = fMain.GET_FIELD_ID("SAJET.SYS_PROCESS", "PROCESS_NAME", "PROCESS_ID", combProcess.Text);                
            }

            g_sPurchase = combPurchase.Text.Trim();

            if (GET_PART_ID(editSubPartNo.Text) == "0")
            {
                SajetCommon.Show_Message("Sub Part No Error", 0);
                editSubPartNo.Focus();
                editSubPartNo.SelectAll();
                return;
            }

            //若加入替代料,Group不可為0
            if ((g_sChangeGroup) & (editGroup.Text == "0" | editGroup.Text == ""))
            {
                SajetCommon.Show_Message("Please Change Relation", 0);
                editGroup.Focus();
                editGroup.SelectAll();
                return;
            }
            
            if (string.IsNullOrEmpty(g_sBOM_ID))
            {                
                sSQL = "Select NVL(Max(BOM_ID),0) + 1 BOM_ID "
                     + "From SAJET.SYS_BOM_INFO ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows[0]["BOM_ID"].ToString() == "1")
                {
                    sSQL = " Select RPAD(NVL(PARAM_VALUE,'1'),2,'0') || '000001' BOM_ID "
                         + " From SAJET.SYS_BASE "
                         + " Where PARAM_NAME = 'DBID' ";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);
                }
                g_sBOM_ID = dsTemp.Tables[0].Rows[0]["BOM_ID"].ToString();

                string sPartID = fMain.GET_FIELD_ID("SAJET.SYS_PART", "PART_NO", "PART_ID", g_sPartNo);
                sSQL = "INSERT INTO sajet.sys_bom_info "
                     + "(BOM_ID,PART_ID,VERSION,UPDATE_USERID)"
                     + "VALUES "
                     + "('" + g_sBOM_ID + "','" + sPartID + "','" + g_sVer + "','" + g_sBOM_ID + "')";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
            }

            sSQL = "Select ITEM_PART_ID from sajet.sys_bom "
                 + "Where BOM_ID='" + g_sBOM_ID + " '"
                 + "and NVL(Process_ID,'0') = '" + g_sProcessID + " '"
                 + "and ITEM_PART_ID = '" + g_sItemPartID + " '";
            if (g_sUpdateType == "Modify")
                sSQL = sSQL + "and rowid <> '" + g_sRowid + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                string sData = LabSubPart.Text + " : " + editSubPartNo.Text;
                string sMsg = SajetCommon.SetLanguage("Data Duplicate", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editSubPartNo.Focus();
                editSubPartNo.SelectAll();
                return;
            }
            DialogResult = DialogResult.OK;
        }        

        private string GET_PART_ID(string sPartNo)
        {
            g_sItemPartID = "";
            g_sItemPartType = "";
            g_sItemSpec1 = "";

            sSQL = " Select PART_ID,PART_TYPE,SPEC1 from SAJET.SYS_PART "
                 + " Where PART_NO = '" + sPartNo + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                g_sItemPartID = dsTemp.Tables[0].Rows[0]["PART_ID"].ToString();
                g_sItemPartType = dsTemp.Tables[0].Rows[0]["PART_TYPE"].ToString();
                g_sItemSpec1 = dsTemp.Tables[0].Rows[0]["SPEC1"].ToString();
                return g_sItemPartID;
            } 
            else
                return "0";
        }        

        private void fData_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            panel1.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            LabPart.Text = g_sPartNo;
            LabVer.Text = g_sVer;

            //Purchase
            combPurchase.Items.Clear();
            combPurchase.Items.Add("N");
            combPurchase.Items.Add("Y");

            if (g_sPurchase != "")
            {
                combPurchase.SelectedIndex = combPurchase.Items.IndexOf(g_sPurchase);
            }
            //else
            //{
            //    combPurchase.SelectedIndex = 0;
            //}

            //Process
            combProcess.Items.Clear();
            combProcess.Items.Add("");
            //sSQL = "Select * from Sajet.sys_process "    //Start Modify By Martin  2009/07/15
            //     + "where enabled = 'Y' "
            //     + "order by process_name ";
            //sSQL = " Select a.* from Sajet.sys_process A "
            sSQL = " Select A.FACTORY_ID,A.PROCESS_ID,A.PROCESS_NAME,A.STAGE_ID,A.PROCESS_CODE,A.PROCESS_DESC,A.OPERATE_ID,A.UPDATE_USERID,A.UPDATE_TIME,A.ENABLED,A.PROCESS_DESC2,A.WIP_ERP from Sajet.sys_process A "
                     + " ,SAJET.SYS_STAGE B "
                     + " Where B.enabled = 'Y' "
                     + " And A.enabled = 'Y' "
                     + " And A.STAGE_ID = B.STAGE_ID "
                     + " Order by process_name ";          //End Modify By Martin  2009/07/15
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                combProcess.Items.Add(dsTemp.Tables[0].Rows[i]["PROCESS_NAME"].ToString());
            }
            if (g_sProcess != "")
                combProcess.SelectedIndex = combProcess.Items.IndexOf(g_sProcess);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            sSQL = " select part_no,spec1,spec2 "
                 + " from sajet.sys_part "
                 + " where enabled = 'Y' "
                 + " and part_no Like '" + editSubPartNo.Text + "%' "
                 + " and part_no <> '" + LabPart.Text + "'"
                 + " Order By part_no ";
            fFilter f = new fFilter();          
            f.sSQL = sSQL;
            f.sPartNoMain = editSubPartNo.Text;
            f.sPartNoBOM = LabPart.Text;
            f.sFlag = "Y";
            if (f.ShowDialog() == DialogResult.OK)
            {
                editSubPartNo.Text = f.dgvData.CurrentRow.Cells["part_no"].Value.ToString();
                //KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);
                //editSubPartNo_KeyPress(sender, Key);
            }
        }        

        private void editSubPartNo_KeyPress(object sender, KeyPressEventArgs e)
        {            
            if (e.KeyChar != (char)Keys.Return)
            {
                return;
            }

            if (GET_PART_ID(editSubPartNo.Text) == "0")
            {
                SajetCommon.Show_Message("Sub Part No Error", 0);
                editSubPartNo.Focus();
                editSubPartNo.SelectAll();
                return;
            }
        }
        
        private void editSubPartNo_EnabledChanged(object sender, EventArgs e)
        {
            btnSearch.Enabled = editSubPartNo.Enabled;
        }
    }
}