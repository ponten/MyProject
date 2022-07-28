using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using SajetTable;
using System.Collections.Specialized;

namespace SajetFilter
{
    //public partial class fFilter : Form
    //{
    //    public fFilter()
    //    {
    //        InitializeComponent();
    //    }

    //    public string sSQL;
    //    List<string> combFilterField = new List<string>();

    //    private void dgvData_DoubleClick(object sender, EventArgs e)
    //    {
    //        if (dgvData.Rows.Count > 0 && dgvData.CurrentRow != null)
    //            DialogResult = DialogResult.OK;
    //    }

    //    private void fFilter_Load(object sender, EventArgs e)
    //    {
    //        combFilter.Items.Clear();
    //        combFilterField.Clear();
    //        for (int i = 0; i <= TableDefine.tGridField_2.Length - 1; i++)
    //        {
    //            combFilter.Items.Add(TableDefine.tGridField_2[i].sCaption);
    //            combFilterField.Add(TableDefine.tGridField_2[i].sFieldName);
    //        }
    //        if (combFilter.Items.Count > 0)
    //            combFilter.SelectedIndex = 0;

    //        showData();

    //        SajetCommon.SetLanguageControl(this);
    //        panel1.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
    //        panel2.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
    //    }

    //    private void editValue_KeyPress(object sender, KeyPressEventArgs e)
    //    {
    //        if (e.KeyChar != (char)Keys.Return)
    //            return;

    //        sSQL = " select A.work_order,B.part_no,B.spec1,B.spec2 "
    //             + " from sajet.g_wo_base A, sajet.sys_part B where ";

    //        if (combFilter.SelectedIndex > -1 && editValue.Text.Trim() != "")
    //        {
    //            string sFieldName = combFilterField[combFilter.SelectedIndex].ToString();
    //            sSQL = sSQL + sFieldName + " like '%" + editValue.Text.Trim() + "%' and ";
    //        }
    //        sSQL = sSQL + " A.part_id = B.part_id "
    //                    + " and A.wo_status > 1 and A.wo_status < 4 "
    //                    + " order by A.work_order";

    //        showData();
    //    }

    //    private void showData()
    //    {
    //        DataSet dsSearch = ClientUtils.ExecuteSQL(sSQL);
    //        dgvData.DataSource = dsSearch;
    //        dgvData.DataMember = dsSearch.Tables[0].ToString();

    //        if (dgvData.Rows.Count > 0)
    //            dgvData.CurrentCell = dgvData.Rows[0].Cells[1];

    //        //欄位Title  
    //        for (int i = 0; i <= TableDefine.tGridField_2.Length - 1; i++)
    //        {
    //            string sGridField = TableDefine.tGridField_2[i].sFieldName;

    //            if (dgvData.Columns.Contains(sGridField))
    //            {
    //                dgvData.Columns[sGridField].HeaderText = TableDefine.tGridField_2[i].sCaption;
    //                dgvData.Columns[sGridField].DisplayIndex = i; //欄位顯示順序
    //            }
    //        }

    //        editValue.Focus();
    //    }
    //}

    // 2016.4.19 By Jason (替換Filter寫法)
    public partial class fFilter : Form
    {
        public fFilter()
        {
            InitializeComponent();
        }

        public string sSQL;

        StringCollection g_tsField = new StringCollection();

        private void dgvData_DoubleClick(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count > 0 && dgvData.CurrentRow != null)
                DialogResult = DialogResult.OK;
        }

        private void fFilter_Load(object sender, EventArgs e)
        {
            combFilter.Items.Clear();

            DataSet dsSearch = ClientUtils.ExecuteSQL(sSQL);

            dgvData.DataSource = dsSearch;
            dgvData.DataMember = dsSearch.Tables[0].ToString();

            for (int i = 0; i <= dsSearch.Tables[0].Columns.Count - 1; i++)
            {
                combFilter.Items.Add(dsSearch.Tables[0].Columns[i].ToString());
                g_tsField.Add(dsSearch.Tables[0].Columns[i].ToString());
            }

            if (combFilter.Items.Count > 0)
                combFilter.SelectedIndex = 0;

            if (dgvData.Rows.Count > 0)
                dgvData.CurrentCell = dgvData.Rows[0].Cells[0];

            SajetCommon.SetLanguageControl(this);
            editValue.Focus();
        }

        private void editValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            if (dgvData.Rows.Count == 0)
                return;

            for (int i = 0; i <= dgvData.RowCount - 1; i++)
            {
                string sData = dgvData.Rows[i].Cells[g_tsField[combFilter.SelectedIndex]].Value.ToString();

                if (sData.Length > editValue.Text.Length)
                    sData = sData.Substring(0, editValue.Text.Length);

                if (sData == editValue.Text)
                {
                    dgvData.CurrentCell = dgvData.Rows[i].Cells[0];
                    break;
                }
            }
        }
    }
}