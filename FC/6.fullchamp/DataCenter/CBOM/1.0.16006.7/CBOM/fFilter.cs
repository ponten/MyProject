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
        //public fFilter()
        //{
        //    InitializeComponent();
        //}

        //public string sSQL;
        //StringCollection g_tsField = new StringCollection();
        //public string sServerName;
        //private void dgvData_DoubleClick(object sender, EventArgs e)
        //{
        //    if (dgvData.Rows.Count > 0 && dgvData.CurrentRow != null)
        //        DialogResult = DialogResult.OK;
        //}

        //private void fFilter_Load(object sender, EventArgs e)
        //{
        //    combField.Items.Clear();
        //    DataSet dsSearch = ClientUtils.ExecuteSQL(sSQL);
        //    dgvData.DataSource = dsSearch;
        //    dgvData.DataMember = dsSearch.Tables[0].ToString();

        //    for (int i = 0; i <= dsSearch.Tables[0].Columns.Count - 1; i++)
        //    {
        //        combField.Items.Add(dsSearch.Tables[0].Columns[i].ToString());
        //        g_tsField.Add(dsSearch.Tables[0].Columns[i].ToString());
        //    }
        //    if (combField.Items.Count > 0)
        //        combField.SelectedIndex = 0;

        //    if (dgvData.Rows.Count > 0)
        //        dgvData.CurrentCell = dgvData.Rows[0].Cells[0];

        //    SajetCommon.SetLanguageControl(this);
        //    panel1.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
        //    panel2.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
        //    editValue.Focus();
        //}

        //private void editValue_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (e.KeyChar != (char)Keys.Return)
        //        return;
        //    if (dgvData.Rows.Count == 0)
        //        return;

        //    for (int i = 0; i <= dgvData.RowCount - 1; i++)
        //    {
        //        //string sData = dgvData.Rows[i].Cells[combField.Text].Value.ToString();
        //        string sData = dgvData.Rows[i].Cells[g_tsField[combField.SelectedIndex]].Value.ToString();
        //        if (sData.Length > editValue.Text.Length)
        //            sData = sData.Substring(0, editValue.Text.Length);

        //        if (sData == editValue.Text)
        //        {
        //            dgvData.CurrentCell = dgvData.Rows[i].Cells[0];
        //            break;
        //        }
        //    }
        //}

        // 2016.6.4 By Jason (����Filter�g�k)
        public fFilter()
        {
            InitializeComponent();
        }

        public string sSQL, sPartNoMain, sPartNoBOM, sFlag;
        DataSet dsSearch;
        List<string> combFilterField = new List<string>();

        private void dgvData_DoubleClick(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count > 0 && dgvData.CurrentRow != null)
                DialogResult = DialogResult.OK;
        }

        private void fFilter_Load(object sender, EventArgs e)
        {
            showData();

            combField.Items.Clear();
            combFilterField.Clear();
            for (int i = 0; i <= dsSearch.Tables[0].Columns.Count - 1; i++)
            {
                combField.Items.Add(dsSearch.Tables[0].Columns[i].ToString());
                combFilterField.Add(dsSearch.Tables[0].Columns[i].ToString());
            }
            if (combField.Items.Count > 0)
                combField.SelectedIndex = 0;

            SajetCommon.SetLanguageControl(this);
            panel1.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
        }

        private void editValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            sSQL = " SELECT PART_NO,SPEC1,SPEC2 "
                 + "   FROM SAJET.SYS_PART WHERE ";

            if (combField.SelectedIndex > -1 && editValue.Text.Trim() != "")
            {
                string sFieldName = combFilterField[combField.SelectedIndex].ToString();

                if (sFieldName.ToUpper().Trim() != "PART_NO")
                {
                    sSQL = sSQL + sFieldName + " LIKE '%" + editValue.Text.Trim() + "%' AND ";
                }
            }

            if (sFlag == "Y")
            {
                sSQL = sSQL + " PART_NO <> '" + sPartNoBOM + "' AND ";
            }

            sSQL = sSQL + " PART_NO LIKE '" + sPartNoMain + "%' AND "
                        + " ENABLED = 'Y' "
                        + " ORDER BY PART_NO";

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