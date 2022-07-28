using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Collections.Specialized;
using SajetTable;

namespace SajetFilter
{
    public partial class fFilter : Form
    {
        public fFilter()
        {
            InitializeComponent();
        }

        public string sSQL;

        private void dgvData_DoubleClick(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count > 0 && dgvData.CurrentRow != null)
                DialogResult = DialogResult.OK;
        }

        private void fFilter_Load(object sender, EventArgs e)
        {
            combFilter.Items.Clear();
            combFilterField.Items.Clear();
            for (int i = 0; i <= TableDefine.tGridField_2.Length - 1; i++)
            {
                combFilter.Items.Add(TableDefine.tGridField_2[i].sCaption);
                combFilterField.Items.Add(TableDefine.tGridField_2[i].sFieldName);
            }
            if (combFilter.Items.Count > 0)
                combFilter.SelectedIndex = 0;

            showData();

            SajetCommon.SetLanguageControl(this);            
        }

        private void editValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            sSQL = "select vendor_id, vendor_code, vendor_name, vendor_desc "
                + " from sajet.sys_vendor where enabled = 'Y' ";

            if (combFilter.SelectedIndex > -1 && editValue.Text.Trim() != "")
            {
                string sFieldName = combFilterField.Items[combFilter.SelectedIndex].ToString();
                sSQL = sSQL + " and " + sFieldName + " like '%" + editValue.Text.Trim() + "%'";
            }
            sSQL = sSQL + " order by vendor_code";

            showData();
        }

        private void showData()
        {
            DataSet dsSearch = ClientUtils.ExecuteSQL(sSQL);
            dgvData.DataSource = dsSearch;
            dgvData.DataMember = dsSearch.Tables[0].ToString();

            if (dgvData.Rows.Count > 0)
            {
                dgvData.Columns[0].Visible = false;
                dgvData.CurrentCell = dgvData.Rows[0].Cells[1];
            }

            //欄位Title  
            for (int i = 0; i <= TableDefine.tGridField_2.Length - 1; i++)
            {
                string sGridField = TableDefine.tGridField_2[i].sFieldName;

                if (dgvData.Columns.Contains(sGridField))
                {
                    dgvData.Columns[sGridField].HeaderText = TableDefine.tGridField_2[i].sCaption;
                    dgvData.Columns[sGridField].DisplayIndex = i; //欄位顯示順序
                }
            }

            editValue.Focus();
        }
    }
}
