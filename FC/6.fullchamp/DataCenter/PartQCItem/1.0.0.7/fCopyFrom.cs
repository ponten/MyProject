using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetFilter;
using SajetClass;

namespace CPartQCItem
{
    public partial class fCopyFrom : Form
    {
        public fCopyFrom()
        {
            InitializeComponent();
        }
        string sSQL;
        DataSet dsTemp;
        public string g_sCopyPartID = "0";
        public string g_sCopyProcessID = "0";
        public string g_sCopyRECID = "0";
        public string g_sCopyProcessRule = "";
        public string g_sSamplingRuleID = "0";
        public string g_sCopyItemTypeID = "", g_sCopySampleID = "", g_sDBNewRecid;
        public string sNewRecID = "",sMsg = "";
        bool lastData; //紀錄datagridDetail的最後一筆資訊是否被Foucs
        fMain fM = new fMain();

        //下拉式選單      
        public void Get_Master()
        {
            sSQL = " SELECT A.RECID,NVL(C.PROCESS_NAME,'N/A') PROCESS_NAME,B.SAMPLING_RULE_NAME,C.PROCESS_ID "
                 + " FROM SAJET.SYS_PART_QC_PROCESS_RULE A "
                 + "     ,SAJET.SYS_QC_SAMPLING_RULE B "
                 + "     ,SAJET.SYS_PROCESS C "
                 + " WHERE A.PART_ID= '" + g_sCopyPartID + "' ";
            sSQL = sSQL 
                 + " AND A.SAMPLING_RULE_ID = B.SAMPLING_RULE_ID(+) "
                 + " AND A.PROCESS_ID = C.PROCESS_ID(+) "
                 + " order by C.PROCESS_NAME ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                coboMaster.Items.Add(dsTemp.Tables[0].Rows[i][1].ToString());
            }
        }

        public void Get_Detail()
        {
            sSQL = "select d.item_type_id,d.item_type_code,d.item_type_name "
                  + "     ,e.sampling_id "
                  + "     ,c.item_code,c.item_name "
                  + "     ,a.item_id,a.upper_limit,a.upper_control_limit,middle_limit,lower_control_limit,lower_limit,unit,sort_index "
                  + "from SAJET.SYS_PART_QC_TESTITEM a, "
                  + "     SAJET.SYS_PART_QC_PROCESS_RULE b, "
                  + "     SAJET.SYS_TEST_ITEM c, "
                  + "     SAJET.SYS_TEST_ITEM_TYPE d, "
                  + "     SAJET.SYS_PART_QC_TESTTYPE e "
                  + "where a.recid = '" + g_sCopyRECID + "' "
                  + "  and a.recid = b.recid "
                  + "  and a.recid = e.recid "
                  + "  and a.ITEM_ID = c.ITEM_ID "
                  + "  and d.item_type_id = e.item_type_id "
                  + "  and c.item_type_id = d.item_type_id "
                  + "  and c.enabled = 'Y' "
                  + "  and d.enabled = 'Y' "
                  + "ORDER BY d.item_type_code,c.ITEM_CODE ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dr = dsTemp.Tables[0].Rows[i];
                datagridDetail.Rows.Add();
                datagridDetail.Rows[datagridDetail.Rows.Count - 1].Cells["ITEM_TYPE_ID1"].Value = dr["ITEM_TYPE_ID"].ToString();
                datagridDetail.Rows[datagridDetail.Rows.Count - 1].Cells["ITEM_TYPE_CODE1"].Value = dr["ITEM_TYPE_CODE"].ToString();
                datagridDetail.Rows[datagridDetail.Rows.Count - 1].Cells["ITEM_TYPE_NAME1"].Value = dr["ITEM_TYPE_NAME"].ToString();
                datagridDetail.Rows[datagridDetail.Rows.Count - 1].Cells["SAMPLING_ID1"].Value = dr["SAMPLING_ID"].ToString();
                datagridDetail.Rows[datagridDetail.Rows.Count - 1].Cells["ITEM_ID1"].Value = dr["ITEM_ID"].ToString();
                datagridDetail.Rows[datagridDetail.Rows.Count - 1].Cells["ITEM_CODE1"].Value = dr["ITEM_CODE"].ToString();
                datagridDetail.Rows[datagridDetail.Rows.Count - 1].Cells["ITEM_NAME1"].Value = dr["ITEM_NAME"].ToString();
                datagridDetail.Rows[datagridDetail.Rows.Count - 1].Cells["UPPER_LIMIT1"].Value = dr["UPPER_LIMIT"].ToString();
                datagridDetail.Rows[datagridDetail.Rows.Count - 1].Cells["UPPER_CONTROL_LIMIT1"].Value = dr["UPPER_CONTROL_LIMIT"].ToString();
                datagridDetail.Rows[datagridDetail.Rows.Count - 1].Cells["MIDDLE_LIMIT1"].Value = dr["MIDDLE_LIMIT"].ToString();
                datagridDetail.Rows[datagridDetail.Rows.Count - 1].Cells["LOWER_CONTROL_LIMIT1"].Value = dr["LOWER_CONTROL_LIMIT"].ToString();
                datagridDetail.Rows[datagridDetail.Rows.Count - 1].Cells["LOWER_LIMIT1"].Value = dr["LOWER_LIMIT"].ToString();
                datagridDetail.Rows[datagridDetail.Rows.Count - 1].Cells["UNIT1"].Value = dr["UNIT"].ToString();
                datagridDetail.Rows[datagridDetail.Rows.Count - 1].Cells["SORT_INDEX1"].Value = dr["SORT_INDEX"].ToString();
            }
            grpSource.Text = "Source Part" + " : " + editCopyPart.Text.Trim();     
        }
        //查詢料號主檔
        private void btnSearchPart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(editCopyPart.Text))
            {
                SajetCommon.Show_Message("Please Input Part No Prefix", 0);
                return;
            }
            string sSQL = "Select PART_NO, SPEC1, SPEC2, VERSION "
                        + "From SAJET.SYS_PART "
                        + "Where PART_NO Like '" + editCopyPart.Text + "%' "
                        + "and Enabled = 'Y' "
                        + "Order By PART_NO ";
            fFilter f = new fFilter();           
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                editCopyPart.Text = f.dgvData.CurrentRow.Cells["PART_NO"].Value.ToString();
                KeyPressEventArgs key = new KeyPressEventArgs((char)Keys.Return);
                editCopyPart_KeyPress(sender, key);
            }
        }
        //料號Key_Press產生狀況
        private void editCopyPart_KeyPress(object sender, KeyPressEventArgs e)
        {
            g_sCopyPartID = "0";
            if (e.KeyChar != (char)Keys.Return)
            {
                return;
            }
            if (e.KeyChar == (char)Keys.Return)
            {
                datagridDetail.Rows.Clear();
                coboMaster.Items.Clear();
                //return;
            }
            sSQL = "Select Part_ID from Sajet.sys_part "
                 + "Where Part_NO = '" + editCopyPart.Text + "' "
                 + "and Enabled = 'Y' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {  
                SajetCommon.Show_Message("Part No Error", 0);
                editCopyPart.Focus();
                editCopyPart.SelectAll();
                return;
            }
            g_sCopyPartID = dsTemp.Tables[0].Rows[0]["PART_ID"].ToString();            
            lablSampleRule.Text = "N/A";
            Get_Master();   //下拉選單資訊
        }

        //查詢製程主檔
        private void btnSearchProcess_Click(object sender, EventArgs e)
        {
            sSQL = "Select b.Process_Name,b.Process_desc "
                 + "From SAJET.SYS_PART_QC_PROCESS_RULE a "
                 + "    ,SAJET.SYS_PROCESS b "
                 + "Where b.Process_Name Like '" + editCopyProcess.Text + "%' "
                 + "and a.part_id = '" + g_sCopyPartID + "' "
                 + "and a.Process_id = b.Process_id "
                 + "and b.enabled = 'Y' "
                 + "Order By b.Process_Name ";
            fFilter f = new fFilter();         
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                editCopyProcess.Text = f.dgvData.CurrentRow.Cells["Process_Name"].Value.ToString();
                KeyPressEventArgs key = new KeyPressEventArgs((char)Keys.Return);
                editCopyProcess_KeyPress(sender, key);
            }
        }
        //製程Key_Press產生狀況
        private void editCopyProcess_KeyPress(object sender, KeyPressEventArgs e)
        {
            g_sCopyProcessID = "0";
            if (e.KeyChar != (char)Keys.Return)
                return;
            sSQL = "Select Process_ID from Sajet.sys_process "
                 + "Where PROCESS_NAME = '" + editCopyProcess.Text + "' "
                 + "and Enabled = 'Y' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                SajetCommon.Show_Message("Process Error", 0);
                editCopyProcess.Focus();
                editCopyProcess.SelectAll();
                return;
            }
            g_sCopyProcessID = dsTemp.Tables[0].Rows[0]["Process_ID"].ToString();
            
            Get_Master(); 
        }

        private void datagridMaster_SelectionChanged(object sender, EventArgs e)
        {
            //if (datagridMaster.CurrentRow != null)
            //{
            //    g_sCopyRECID = datagridMaster.CurrentRow.Cells["RECID"].Value.ToString();
            //    Get_Detail();
            //}
        }

        private void btnCopyOK_Click(object sender, EventArgs e)
        {
            if (g_sCopyPartID == "0")
            {
                SajetCommon.Show_Message("Part No Error", 0);
                editCopyPart.Focus();
                editCopyPart.SelectAll();
                return;
            }

            if (!string.IsNullOrEmpty(coboMaster.Text) && g_sCopyProcessID == "0")
            {
                SajetCommon.Show_Message("Part No Error", 0);
                editCopyProcess.Focus();
                editCopyProcess.SelectAll();
                return;
            }

            if (SajetCommon.Show_Message("Are You Sure Full Copy ?", 2) != DialogResult.Yes)
                return;
            DialogResult = DialogResult.OK;
        }

        private void fCopyFrom_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            panel1.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
            grpDestination.Text = "Destination Part" + " : " + fMain.g_editPartNo;
            sNewRecID = SajetCommon.GetMaxID("SAJET.SYS_PART_QC_PROCESS_RULE", "RECID", 8);
        }

        private void coboMaster_SelectedIndexChanged(object sender, EventArgs e)
        {            
            datagridDetail.Rows.Clear();
            comboChangParams();         
        }

        private void btnAddAll_Click(object sender, EventArgs e)
        {
            
            if ((sender as Button).Tag.ToString() == "0")
            {
                datagridDetail.SelectAll();
            }
            else 
            {
                for (int i = 0; i < datagridDetail.Rows.Count; i++)
                {
                    datagridDetail.Rows[i].Selected = false;
                }
            }
    
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (datagridSelect.Rows.Count == 0 || datagridSelect.SelectedCells.Count == 0 || datagridSelect.CurrentCell == null)
                { return; }
            int iIndex = datagridSelect.CurrentRow.Index;
            datagridSelect.Rows.RemoveAt(iIndex);           
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {            
            if (datagridDetail.Rows.Count == 0)
            {
                SajetCommon.Show_Message("No TestItem Data Can be Added", 0);
                return;
            }
            for (int i = datagridDetail.Rows.Count - 1; i >= 0; i--)
            {   
                 if (i >= 1)
                    {if (datagridDetail.Rows[i - 1].Selected.ToString() == "True"){lastData = true;} else{lastData = false;}}
                //------------Copy datagridDetail to datagridMaster (Process_id > Depli)
                string s_gRecStatus = "";
                for (int k = 0; k < datagridMaster.Rows.Count; k++)
			    {
			        if (datagridMaster.Rows[k].Cells["PROCESS_ID1"].Value.ToString() == g_sCopyProcessID)
                    { s_gRecStatus = "Depli"; }
		        }
                if (s_gRecStatus == "")
	            {
		            datagridMaster.Rows.Add();
                    datagridMaster.Rows[datagridMaster.Rows.Count - 1].Cells["NEWRECID"].Value = sNewRecID.ToString();
                    datagridMaster.Rows[datagridMaster.Rows.Count - 1].Cells["PROCESS_ID1"].Value = g_sCopyProcessID;
                    datagridMaster.Rows[datagridMaster.Rows.Count - 1].Cells["SAMPLING_RULE_ID1"].Value = g_sSamplingRuleID;
                    datagridMaster.Rows[datagridMaster.Rows.Count - 1].Cells["PROCESS_NAME1"].Value = coboMaster.Text;
                    int sNewRecIDTemp = int.Parse(sNewRecID) + 1;
                    sNewRecID = sNewRecIDTemp.ToString();
	            }

                //-----------Copy datagridDetail to datgridSelect (Add Item > Depli)
                string s_gStatus = "";
                if (datagridDetail.Rows[i].Selected.ToString() == "True")
                {
                    for (int j = 0; j < datagridSelect.Rows.Count; j++)
                    {
                        if (datagridSelect.Rows[j].Cells["PROCESS_NAME"].Value.ToString() == coboMaster.Text
                            && datagridSelect.Rows[j].Cells["ITEM_TYPE_CODE"].Value.ToString() == datagridDetail.Rows[i].Cells["ITEM_TYPE_CODE1"].Value.ToString()
                            && datagridSelect.Rows[j].Cells["ITEM_CODE"].Value.ToString() == datagridDetail.Rows[i].Cells["ITEM_CODE1"].Value.ToString())
                        { s_gStatus = "Depli"; }
                    }
                    if (s_gStatus == "")
                    {
                        g_sDBNewRecid = "";
                        for (int l = 0; l < datagridMaster.Rows.Count; l++)
                        {
                            if (g_sCopyProcessID == datagridMaster.Rows[l].Cells["PROCESS_ID1"].Value.ToString())
                            {
                                g_sDBNewRecid = datagridMaster.Rows[l].Cells["NEWRECID"].Value.ToString();
                            } 
                        }                        
                        datagridSelect.Rows.Add();
                        datagridSelect.Rows[datagridSelect.Rows.Count - 1].Cells["NEWRECID1"].Value = g_sDBNewRecid;
                        datagridSelect.Rows[datagridSelect.Rows.Count - 1].Cells["RECID"].Value = g_sCopyRECID;
                        datagridSelect.Rows[datagridSelect.Rows.Count - 1].Cells["Process_ID"].Value = g_sCopyProcessID;
                        datagridSelect.Rows[datagridSelect.Rows.Count - 1].Cells["Process_Name"].Value = coboMaster.Text;
                        datagridSelect.Rows[datagridSelect.Rows.Count - 1].Cells["SAMPLING_RULE_ID"].Value = g_sSamplingRuleID;
                        datagridSelect.Rows[datagridSelect.Rows.Count - 1].Cells["SAMPLING_RULE_NAME"].Value = g_sCopyProcessRule;
                        datagridSelect.Rows[datagridSelect.Rows.Count - 1].Cells["ITEM_TYPE_ID"].Value = datagridDetail.Rows[i].Cells["ITEM_TYPE_ID1"].Value.ToString();
                        datagridSelect.Rows[datagridSelect.Rows.Count - 1].Cells["ITEM_TYPE_CODE"].Value = datagridDetail.Rows[i].Cells["ITEM_TYPE_CODE1"].Value.ToString();
                        datagridSelect.Rows[datagridSelect.Rows.Count - 1].Cells["ITEM_TYPE_NAME"].Value = datagridDetail.Rows[i].Cells["ITEM_TYPE_NAME1"].Value.ToString();
                        datagridSelect.Rows[datagridSelect.Rows.Count - 1].Cells["SAMPLING_ID"].Value = datagridDetail.Rows[i].Cells["SAMPLING_ID1"].Value.ToString();
                        datagridSelect.Rows[datagridSelect.Rows.Count - 1].Cells["ITEM_ID"].Value = datagridDetail.Rows[i].Cells["ITEM_ID1"].Value.ToString();
                        datagridSelect.Rows[datagridSelect.Rows.Count - 1].Cells["ITEM_CODE"].Value = datagridDetail.Rows[i].Cells["ITEM_CODE1"].Value.ToString();
                        datagridSelect.Rows[datagridSelect.Rows.Count - 1].Cells["ITEM_NAME"].Value = datagridDetail.Rows[i].Cells["ITEM_NAME1"].Value.ToString();
                        datagridSelect.Rows[datagridSelect.Rows.Count - 1].Cells["UPPER_LIMIT"].Value = datagridDetail.Rows[i].Cells["UPPER_LIMIT1"].Value.ToString();
                        datagridSelect.Rows[datagridSelect.Rows.Count - 1].Cells["UPPER_CONTROL_LIMIT"].Value = datagridDetail.Rows[i].Cells["UPPER_CONTROL_LIMIT1"].Value.ToString();
                        datagridSelect.Rows[datagridSelect.Rows.Count - 1].Cells["MIDDLE_LIMIT"].Value = datagridDetail.Rows[i].Cells["MIDDLE_LIMIT1"].Value.ToString();
                        datagridSelect.Rows[datagridSelect.Rows.Count - 1].Cells["LOWER_CONTROL_LIMIT"].Value = datagridDetail.Rows[i].Cells["LOWER_CONTROL_LIMIT1"].Value.ToString();
                        datagridSelect.Rows[datagridSelect.Rows.Count - 1].Cells["LOWER_LIMIT"].Value = datagridDetail.Rows[i].Cells["LOWER_LIMIT1"].Value.ToString();
                        datagridSelect.Rows[datagridSelect.Rows.Count - 1].Cells["UNIT"].Value = datagridDetail.Rows[i].Cells["UNIT1"].Value.ToString();
                        datagridSelect.Rows[datagridSelect.Rows.Count - 1].Cells["SORT_INDEX"].Value = datagridDetail.Rows[i].Cells["SORT_INDEX1"].Value.ToString();
                        datagridDetail.Rows.RemoveAt(i);
                    }
                    else { datagridDetail.Rows.RemoveAt(i); }                   
                }
                if (i>=1) { datagridDetail.Rows[i - 1].Selected = lastData; }                              
            }
           
        }

        private void btnCopyCancel_Click(object sender, EventArgs e)
        {

        }

        private void btnCopySave_Click(object sender, EventArgs e)
        {
            string g_sProName="";
            if (datagridSelect.Rows.Count == 0)
            {
                SajetCommon.Show_Message("No TestItem Data Can be Save", 0);
                editCopyPart.Focus();
                return;
            }

            if (fMain.g_sStaticPartID == g_sCopyPartID)
            {
                sMsg = SajetCommon.SetLanguage("Can't copy from the same Part", 1);
                SajetCommon.Show_Message(sMsg + Environment.NewLine + fMain.g_editPartNo , 0);
                return;
            }            
            //取得系統時間
            sSQL = "Select SYSDATE from dual ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            DateTime dtNow = (DateTime)dsTemp.Tables[0].Rows[0]["SYSDATE"]; //轉型成日期型別
            string sNow = dtNow.ToString("yyyy/MM/dd HH:mm:ss"); //定義日期格式            
            //先刪除原來的資料
            for (int o = 0; o < datagridMaster.Rows.Count; o++)
            {                           
                string sCondition = " select distinct recid from SAJET.SYS_PART_QC_PROCESS_RULE "
                                  + " where part_id = '" + fMain.g_sStaticPartID + "' ";
                if (g_sCopyProcessID != "0")
                    sCondition = sCondition + " and process_id = '" + datagridMaster.Rows[o].Cells["PROCESS_ID1"].Value.ToString() + "' ";
                dsTemp = ClientUtils.ExecuteSQL(sCondition);
                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    g_sProName = g_sProName + " : " + datagridMaster.Rows[o].Cells["PROCESS_NAME1"].Value.ToString();
                    sMsg = SajetCommon.SetLanguage("Test Item had existed,Replace", 1) + " ? " + Environment.NewLine;
                    if (g_sCopyProcessID != "0")
                        sMsg = sMsg + SajetCommon.SetLanguage("Process Name", 1) + g_sProName;
                }
                else { sMsg = "Are You Sure Copy ?"; }
            }

            if (SajetCommon.Show_Message(sMsg, 2) == DialogResult.Yes)
            {
                for (int p = 0; p < datagridMaster.Rows.Count; p++)
                {
                    string sCondition = " select distinct recid from SAJET.SYS_PART_QC_PROCESS_RULE "
                                  + " where part_id = '" + fMain.g_sStaticPartID + "' ";
                    if (g_sCopyProcessID != "0")
                    sCondition = sCondition + " and process_id = '" + datagridMaster.Rows[p].Cells["PROCESS_ID1"].Value.ToString() + "' ";

                sSQL = "DELETE SAJET.SYS_PART_QC_TESTITEM "
                       + "WHERE RECID in (" + sCondition + ") ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);

                sSQL = "DELETE SAJET.SYS_PART_QC_TESTTYPE "
                      + "WHERE RECID in (" + sCondition + ") ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);

                sSQL = "DELETE SAJET.SYS_PART_QC_PROCESS_RULE "
                      + "WHERE RECID in (" + sCondition + ") ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                }            
            }
            else { return; }
            //-------------------------------------362~402行 刪除料號原現存測試項目資料--------------------------------
            string sSQL1;
            DataSet dsTemp1;
            for (int m = 0; m < datagridMaster.Rows.Count; m++)
            {
                if (sNewRecID == "0")
                {
                    SajetCommon.Show_Message("Get RECID Error", 0);
                    return;
                }
                string s_gStatusMaster = "";
                for (int i = 0; i < datagridSelect.Rows.Count ; i++)
                {
                    if (datagridSelect.Rows[i].Cells["PROCESS_ID"].Value.ToString() == datagridMaster.Rows[m].Cells["PROCESS_ID1"].Value.ToString())
                    {
                        s_gStatusMaster = "Depli";                        
                    }
                }
                if (s_gStatusMaster == "Depli")
                {

                    sSQL1 = " select recid,PART_ID from SAJET.SYS_PART_QC_PROCESS_RULE "
                      + " WHERE RECID = '" + datagridMaster.Rows[m].Cells["NEWRECID"].Value.ToString() + "'";
                     dsTemp1 = ClientUtils.ExecuteSQL(sSQL1);
                     //SajetCommon.Show_Message("xx----xx", 0);
                     if (dsTemp1.Tables[0].Rows.Count > 0 )
                     {
                         if (fMain.g_sStaticPartID != dsTemp1.Tables[0].Rows[0][1].ToString())
                         { 
                              SajetCommon.Show_Message("Insert RECID of Process is be used" + Environment.NewLine
                                                      + "Process Name" + ": " + datagridMaster.Rows[m].Cells["PROCESS_NAME1"].Value.ToString(), 0);
                         } 
                         else
                         { instgridMaster(ref m,ref sNow); }
                     }
                     else
                     {
                      instgridMaster(ref m,ref sNow);   
                     }                    
                }
            }
            //新增製程xxxx的RECID已被使用，請重新啟動視窗重選加入
            //--------------------------404~433新增資料至SAJET.SYS_PART_QC_PROCESS_RULE，並判斷使用者所選之測試項目於主檔中有此RECID資訊，如有才INSERT
            for (int n = 0; n < datagridSelect.Rows.Count; n++)  
            {
                sSQL1 = " select recid,PART_ID from SAJET.SYS_PART_QC_PROCESS_RULE "
                      + " WHERE RECID = '" + datagridSelect.Rows[n].Cells["NEWRECID1"].Value.ToString() + "'";
                     dsTemp1 = ClientUtils.ExecuteSQL(sSQL1);
                     if (dsTemp1.Tables[0].Rows.Count > 0 )
                     {
                         if (fMain.g_sStaticPartID != dsTemp1.Tables[0].Rows[0][1].ToString())
                         {
                             //SajetCommon.Show_Message("XXXX請重新啟動視窗重選加入", 0);
                         }
                         else
                         { 
                            instgridSelect(ref n, ref sNow);
                         }
                     }
                     else
                     {
                         instgridSelect(ref n, ref sNow);   
                     }
            }
        }
        public void comboChangParams()
        {
            sSQL = " SELECT A.RECID,NVL(C.PROCESS_NAME,'N/A') PROCESS_NAME,B.SAMPLING_RULE_NAME,C.PROCESS_ID,A.SAMPLING_RULE_ID "
                + " FROM SAJET.SYS_PART_QC_PROCESS_RULE A "
                + "     ,SAJET.SYS_QC_SAMPLING_RULE B "
                + "     ,SAJET.SYS_PROCESS C "
                + " WHERE A.PART_ID= '" + g_sCopyPartID + "' " 
                + " AND C.PROCESS_NAME = '" + coboMaster.Text.Trim() + "' "
                + " AND A.SAMPLING_RULE_ID = B.SAMPLING_RULE_ID(+) "
                + " AND A.PROCESS_ID = C.PROCESS_ID(+) "
                + " order by C.PROCESS_NAME ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (!string.IsNullOrEmpty(editCopyPart.Text) && coboMaster.Text != null)
            {
                g_sCopyRECID = dsTemp.Tables[0].Rows[0][0].ToString();
                g_sCopyProcessRule = dsTemp.Tables[0].Rows[0][2].ToString();
                lablSampleRule.Text = dsTemp.Tables[0].Rows[0][2].ToString();
                g_sCopyProcessID = dsTemp.Tables[0].Rows[0][3].ToString();
                g_sSamplingRuleID = dsTemp.Tables[0].Rows[0][4].ToString();             
                Get_Detail();
            }            
        }
        public void instgridMaster(ref int m,ref string sNow)
        {
            string sSQL1 = "Insert Into SAJET.SYS_PART_QC_PROCESS_RULE "
                              + " (RECID,PART_ID,PROCESS_ID,SAMPLING_RULE_ID,UPDATE_USERID,UPDATE_TIME) "
                              + " VALUES ('" + datagridMaster.Rows[m].Cells["NEWRECID"].Value.ToString() + "','"
                              + fMain.g_sStaticPartID + "','"
                              + datagridMaster.Rows[m].Cells["PROCESS_ID1"].Value.ToString() + "','"
                              + datagridMaster.Rows[m].Cells["SAMPLING_RULE_ID1"].Value.ToString() + "','"
                              + fMain.g_sUserID + "',"
                              + "TO_DATE('" + sNow + "','yyyy/mm/dd hh24:mi:ss')) ";
            DataSet dsTemp1 = ClientUtils.ExecuteSQL(sSQL1);
        }
        public void instgridSelect(ref int n, ref string sNow)
        {
            string sSQL1;
            DataSet dsTemp1;
            //SAJET.SYS_PART_QC_TESTYPE 資料新增部分
            sSQL1 = " SELECT RECID,ITEM_TYPE_ID FROM SAJET.SYS_PART_QC_TESTTYPE "
                  + " WHERE RECID = '" + datagridSelect.Rows[n].Cells["NEWRECID1"].Value.ToString() + "'"
                  + " AND ITEM_TYPE_ID = '" + datagridSelect.Rows[n].Cells["ITEM_TYPE_ID"].Value.ToString() + "'";
            dsTemp1 = ClientUtils.ExecuteSQL(sSQL1);
            if (dsTemp1.Tables[0].Rows.Count == 0)
            {
                sSQL1 = "Insert Into SAJET.SYS_PART_QC_TESTTYPE "
                  + " (RECID,ITEM_TYPE_ID,SAMPLING_ID,UPDATE_USERID,UPDATE_TIME) "
                  + " VALUES ('" + datagridSelect.Rows[n].Cells["NEWRECID1"].Value.ToString() + "','"
                  + datagridSelect.Rows[n].Cells["ITEM_TYPE_ID"].Value.ToString() + "','"
                  + datagridSelect.Rows[n].Cells["SAMPLING_ID"].Value.ToString() + "','"
                  + fMain.g_sUserID + "',"
                  + "TO_DATE('" + sNow + "','yyyy/mm/dd hh24:mi:ss')) ";
                dsTemp1 = ClientUtils.ExecuteSQL(sSQL1);
            }

            sSQL1 = " Insert Into SAJET.SYS_PART_QC_TESTITEM "
                  + " (RECID,ITEM_ID,UPPER_LIMIT,LOWER_LIMIT,MIDDLE_LIMIT,UPPER_CONTROL_LIMIT,LOWER_CONTROL_LIMIT "
                  + " ,UPDATE_USERID,UPDATE_TIME,UNIT,sort_index ) "
                  + " VALUES ('" + datagridSelect.Rows[n].Cells["NEWRECID1"].Value.ToString() + "','"
                  + datagridSelect.Rows[n].Cells["ITEM_ID"].Value.ToString() + "','"
                  + datagridSelect.Rows[n].Cells["UPPER_LIMIT"].Value.ToString() + "','"
                  + datagridSelect.Rows[n].Cells["LOWER_LIMIT"].Value.ToString() + "','"
                  + datagridSelect.Rows[n].Cells["MIDDLE_LIMIT"].Value.ToString() + "','"
                  + datagridSelect.Rows[n].Cells["UPPER_CONTROL_LIMIT"].Value.ToString() + "','"
                  + datagridSelect.Rows[n].Cells["LOWER_CONTROL_LIMIT"].Value.ToString() + "','"
                  + fMain.g_sUserID + "',"
                  + "TO_DATE('" + sNow + "','yyyy/mm/dd hh24:mi:ss'),'"
                  + datagridSelect.Rows[n].Cells["UNIT"].Value.ToString() + "','"
                  + datagridSelect.Rows[n].Cells["SORT_INDEX"].Value.ToString() + "')";
            dsTemp1 = ClientUtils.ExecuteSQL(sSQL1);
        }
        private void datagridSelect_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }       
    }
}