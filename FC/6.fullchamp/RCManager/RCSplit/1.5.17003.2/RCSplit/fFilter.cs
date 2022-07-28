using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Collections.Specialized;

namespace SajetFilter
{
    public partial class fFilter : Form
    {
        public string sSQL;
        DataSet dsSearch;
        List<string> combFilterField = new List<string>();

        public fFilter()
        {
            InitializeComponent();

            btnOK.Click += BtnOK_Click;

            dgvData.CellDoubleClick += DgvData_CellDoubleClick;
        }

        private void fFilter_Load(object sender, EventArgs e)
        {
            showData();

            combFilter.Items.Clear();
            combFilterField.Clear();
            for (int i = 0; i <= dsSearch.Tables[0].Columns.Count - 1; i++)
            {
                combFilter.Items.Add(dsSearch.Tables[0].Columns[i].ToString());
                combFilterField.Add(dsSearch.Tables[0].Columns[i].ToString());
            }
            if (combFilter.Items.Count > 0)
                combFilter.SelectedIndex = 0;

            SajetCommon.SetLanguageControl(this);
            panel1.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count > 0 && dgvData.CurrentRow != null)
            {
                DialogResult = DialogResult.OK;
            }
        }

        private void DgvData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                BtnOK_Click(null, null);
            }
        }

        private void editValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            sSQL = " Select RC_NO, CURRENT_QTY, WORK_ORDER "
                 + " From SAJET.G_RC_STATUS where ";

            if (combFilter.SelectedIndex > -1 && editValue.Text.Trim() != "")
            {
                string sFieldName = combFilterField[combFilter.SelectedIndex].ToString();
                sSQL = sSQL + sFieldName + " like '%" + editValue.Text.Trim() + "%' and ";
            }
            sSQL = sSQL + " CURRENT_STATUS >= 0 and CURRENT_STATUS < 2 "
                        + " AND CURRENT_QTY > 0 "
                        + " order by WORK_ORDER";

            showData();
        }

        private void showData()
        {
            dsSearch = ClientUtils.ExecuteSQL(sSQL);
            dgvData.DataSource = dsSearch;
            dgvData.DataMember = dsSearch.Tables[0].ToString();

            if (dgvData.Rows.Count > 0)
                dgvData.CurrentCell = dgvData.Rows[0].Cells[1];

            editValue.Focus();
        }
    }
}