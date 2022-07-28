using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;


namespace SajetFilter
{
    public partial class fFilterDetail : Form
    {
        public fFilterDetail()
        {
            InitializeComponent();
        }
        
        public string sSQL;
        public string sServerName;
        public int indexField;
        public string detail_sItemTypeCode;
        string fFiltersSQL;
        private void dgvData_DoubleClick(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count > 0 && dgvData.CurrentRow != null)
                DialogResult = DialogResult.OK;
        }

        private void fFilterDetail_Load(object sender, EventArgs e)
        {           
            combField.Items.Clear();
            DataSet dsSearch = ClientUtils.ExecuteSQL(sSQL);
            dgvData.DataSource = dsSearch;
            dgvData.DataMember = dsSearch.Tables[0].ToString();

            for (int i = 0; i <= dsSearch.Tables[0].Columns.Count - 1; i++)
            {
                combField.Items.Add(dsSearch.Tables[0].Columns[i].ToString());
            }
            if (combField.Items.Count > 0)
                combField.SelectedIndex = 1;  //以item_name為預設查詢條件

            if (dgvData.Rows.Count > 0)
                dgvData.CurrentCell = dgvData.Rows[0].Cells[0];

            SajetCommon.SetLanguageControl(this);
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            panel1.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
            editValue.Focus();            
        }

        private void editValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            //if (dgvData.Rows.Count == 0)
            //    return;

            fFiltersSQL = "select a.Item_Code,a.Item_Name,a.has_Value "
                 + "from SAJET.SYS_TEST_ITEM a "
                 + "    ,SAJET.SYS_TEST_ITEM_TYPE b "
                 + "Where b.ITEM_TYPE_CODE = '" + detail_sItemTypeCode + "' "
                 + "and a.Item_Type_ID = b.Item_Type_ID "
                 + "and a.enabled = 'Y' "
                 + "Order By a.Item_Code ";
            fFiltersSQL = " SELECT * FROM (" + fFiltersSQL + ")"
            + " WHERE " + combField.Text.Trim()
            + " LIKE '%" + editValue.Text.Trim() + "%'";

            DataSet dsSearch = ClientUtils.ExecuteSQL(fFiltersSQL);
            dgvData.DataSource = dsSearch;
            dgvData.DataMember = dsSearch.Tables[0].ToString();
            
           
        }
    }
}