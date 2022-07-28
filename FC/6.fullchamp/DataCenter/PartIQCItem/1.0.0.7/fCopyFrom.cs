using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetFilter;
using SajetClass;
using SajetTable;

namespace CPartIQCItem
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
        fMain fM = new fMain();

        public void Get_Master()
        {
            sSQL = " SELECT A.RECID,NVL(C.VENDOR_CODE,'N/A') VENDOR_CODE,NVL(C.VENDOR_NAME,'N/A') VENDOR_NAME "
                 + "       ,B.SAMPLING_RULE_NAME "
                 + " FROM SAJET.SYS_PART_IQC_VENDOR_RULE A "
                 + "     ,SAJET.SYS_QC_SAMPLING_RULE B "
                 + "     ,SAJET.SYS_VENDOR C "
                 + " WHERE A.PART_ID= '" + g_sCopyPartID + "' ";
            if (!string.IsNullOrEmpty(editCopyProcess.Text))
                sSQL = sSQL + " AND A.VENDOR_ID = '" + g_sCopyProcessID + "' ";
            sSQL = sSQL 
                 + " AND A.SAMPLING_RULE_ID = B.SAMPLING_RULE_ID(+) "
                 + " AND A.VENDOR_ID = C.VENDOR_ID(+) "
                 + " order by C.VENDOR_CODE ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            datagridMaster.DataSource = dsTemp;
            datagridMaster.DataMember = dsTemp.Tables[0].ToString();

            datagridMaster.Columns["RECID"].Visible = false;
            for (int i = 0; i <= datagridMaster.Columns.Count - 1; i++)
            {
                datagridMaster.Columns[i].Visible = false;
            }
            for (int i = 0; i <= TableDefine.tGridField.Length - 1; i++)
            {
                string sGridField = TableDefine.tGridField[i].sFieldName;

                if (datagridMaster.Columns.Contains(sGridField))
                {
                    datagridMaster.Columns[sGridField].HeaderText = TableDefine.tGridField[i].sCaption;
                    datagridMaster.Columns[sGridField].DisplayIndex = i; //欄位顯示順序
                    datagridMaster.Columns[sGridField].Visible = true;
                }
            }    
        }

        public void Get_Detail()
        {
            sSQL = "select d.item_type_code,d.item_type_name "
                  + "     ,c.item_code,c.item_name "
                  + "     ,a.upper_limit,a.upper_control_limit,middle_limit,lower_control_limit,lower_limit,unit "
                  + "from SAJET.SYS_PART_IQC_TESTITEM a, "
                  + "     SAJET.SYS_PART_IQC_VENDOR_RULE b, "
                  + "     SAJET.SYS_TEST_ITEM c, "
                  + "     SAJET.SYS_TEST_ITEM_TYPE d "
                  + "where a.recid = '" + g_sCopyRECID + "' "
                  + "  and a.recid = b.recid "
                  + "  and a.ITEM_ID = c.ITEM_ID "
                  + "  and c.item_type_id = d.item_type_id "
                  + "  and c.enabled = 'Y' "
                  + "  and d.enabled = 'Y' "
                  + "ORDER BY d.item_type_code,c.ITEM_CODE ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            datagridDetail.DataSource = dsTemp;
            datagridDetail.DataMember = dsTemp.Tables[0].ToString();
            //欄位Title  
            for (int i = 0; i <= datagridDetail.Columns.Count - 1; i++)
            {
                datagridDetail.Columns[i].Visible = false;
            }
            for (int i = 0; i <= TableDefine.tGridDetailField.Length - 1; i++)
            {
                string sGridField = TableDefine.tGridDetailField[i].sFieldName;

                if (datagridDetail.Columns.Contains(sGridField))
                {
                    datagridDetail.Columns[sGridField].HeaderText = TableDefine.tGridDetailField[i].sCaption;
                    datagridDetail.Columns[sGridField].DisplayIndex = i; //欄位顯示順序
                    datagridDetail.Columns[sGridField].Visible = true;
                }
            }
          
        }

        private void btnSearchPart_Click(object sender, EventArgs e)
        {
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

        private void editCopyPart_KeyPress(object sender, KeyPressEventArgs e)
        {
            g_sCopyPartID = "0";
            datagridMaster.DataSource = null;
            datagridDetail.DataSource = null;

            if (e.KeyChar != (char)Keys.Return)
                return;

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

            Get_Master(); 
        }

        private void btnSearchProcess_Click(object sender, EventArgs e)
        {
            sSQL = "Select b.VENDOR_CODE ,b.VENDOR_NAME  "
                 + "From SAJET.SYS_PART_IQC_VENDOR_RULE a "
                 + "    ,SAJET.SYS_VENDOR b "
                 + "Where b.VENDOR_CODE Like '" + editCopyProcess.Text + "%' "
                 + "and a.PART_ID = '" + g_sCopyPartID + "' "
                 + "and a.VENDOR_ID = b.VENDOR_ID "
                 + "and b.enabled = 'Y' "
                 + "Order By b.VENDOR_CODE ";
            fFilter f = new fFilter();         
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                editCopyProcess.Text = f.dgvData.CurrentRow.Cells["VENDOR_CODE"].Value.ToString();
                KeyPressEventArgs key = new KeyPressEventArgs((char)Keys.Return);
                editCopyProcess_KeyPress(sender, key);
            }
        }

        private void editCopyProcess_KeyPress(object sender, KeyPressEventArgs e)
        {
            g_sCopyProcessID = "0";
            datagridMaster.DataSource = null;
            datagridDetail.DataSource = null;

            if (e.KeyChar != (char)Keys.Return)
                return;

            sSQL = "Select VENDOR_ID from Sajet.sys_vendor "
                 + "Where VENDOR_CODE = '" + editCopyProcess.Text + "' "
                 + "and Enabled = 'Y' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                SajetCommon.Show_Message("Vendor Error", 0);
                editCopyProcess.Focus();
                editCopyProcess.SelectAll();
                return;
            }
            g_sCopyProcessID = dsTemp.Tables[0].Rows[0]["VENDOR_ID"].ToString();
            
            Get_Master(); 
        }

        private void datagridMaster_SelectionChanged(object sender, EventArgs e)
        {
            if (datagridMaster.CurrentRow != null)
            {
                g_sCopyRECID = datagridMaster.CurrentRow.Cells["RECID"].Value.ToString();
                Get_Detail();
            }
        }

        private void btnCopyOK_Click(object sender, EventArgs e)
        {
            if (g_sCopyPartID == "0")
            {
                SajetCommon.Show_Message("Part Error", 0);
                editCopyPart.Focus();
                editCopyPart.SelectAll();
                return;
            }
            if (!string.IsNullOrEmpty(editCopyProcess.Text) && g_sCopyProcessID == "0")
            {
                SajetCommon.Show_Message("Vendor Error", 0);
                editCopyProcess.Focus();
                editCopyProcess.SelectAll();
                return;
            }
            if (datagridMaster.Rows.Count == 0)
            {
                SajetCommon.Show_Message("No Data", 0);
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void fCopyFrom_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
        }        
    }
}