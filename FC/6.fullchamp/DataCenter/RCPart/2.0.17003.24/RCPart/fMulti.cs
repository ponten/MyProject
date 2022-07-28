using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;

namespace RCPart
{
    public partial class fSQLMultiList : Form
    {
        public string g_sPartNo;
        public fSQLMultiList()
        {
            InitializeComponent();
        }
        public fSQLMultiList(int nControlIndex,ListBox lstSelect)
        {
            m_nControlIndex = nControlIndex;
            //m_tControlAd = tControlDate;
            m_lstSelect = lstSelect;
            InitializeComponent();
        }

        public ListBox m_lstSelect;
        public int m_nControlIndex;
        private void fMultiList_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            lablPartNo.Text = g_sPartNo;
        }
        private void btnAddOne_Click(object sender, EventArgs e)
        {
            int nCnt = lstAll.SelectedItems.Count;
            for (int i = 0; i < nCnt; i++)
            {
                lstSelect.Items.Add(lstAll.SelectedItem);
                lstAll.Items.Remove(lstAll.SelectedItem);
            }
        }

        private void btnAddAll_Click(object sender, EventArgs e)
        {
            int nCnt = lstAll.Items.Count;
            for (int i = 0; i < nCnt; i++)
            {
                lstSelect.Items.Add(lstAll.Items[i]);
            }
            lstAll.Items.Clear();
        }

        private void btnRemoveOne_Click(object sender, EventArgs e)
        {
            int nCnt = lstSelect.SelectedItems.Count;
            for (int i = 0; i < nCnt; i++)
            {
                lstAll.Items.Add(lstSelect.SelectedItem);
                lstSelect.Items.Remove(lstSelect.SelectedItem);
            }
        }

        private void btnRemoveAll_Click(object sender, EventArgs e)
        {
            int nCnt = lstSelect.Items.Count;
            for (int i = 0; i < nCnt; i++)
            {
                lstAll.Items.Add(lstSelect.Items[i]);
            }
            lstSelect.Items.Clear();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //fCustomerReport fCustomerReport = new fCustomerReport();
            m_lstSelect.Items.Clear();
            for (int i = 0; i < lstSelect.Items.Count; i++)
            {
                m_lstSelect.Items.Add(lstSelect.Items[i]);
            }
            Close();
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }



    }
}