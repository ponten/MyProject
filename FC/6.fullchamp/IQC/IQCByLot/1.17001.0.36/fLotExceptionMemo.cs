using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;

namespace IQCbyLot
{
    public partial class fLotExceptionMemo : Form
    {
        public string g_sLotNo;
        string g_sLotSize;
        string g_sReceiveQty;
        string g_sWaiveNo;

        public fLotExceptionMemo()
        {
            InitializeComponent();
        }

        private void fLotExceptionMemo_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            lablLotNo.Text = g_sLotNo;
            g_sLotSize = editLotsize.Text;
            g_sReceiveQty = editReceiveQty.Text;
            g_sWaiveNo = editWaiveNo.Text;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (editLotsize.Enabled)
            {
                editLotsize.Text = editLotsize.Text.Trim();
                editReceiveQty.Text = editReceiveQty.Text.Trim();
                int iLotSize = 0;

                try
                {
                    iLotSize = Convert.ToInt32(editLotsize.Text);
                    /*
                    if (iLotSize > Convert.ToInt32(g_sLotSize))
                    {
                        SajetCommon.Show_Message("New Lot Size can not over old Lot Size", 0);
                        editLotsize.Focus();
                        editLotsize.SelectAll();
                        return;
                    }
                     */ 
                }
                catch
                {
                    SajetCommon.Show_Message("Lot Size Error", 0);
                    editLotsize.Focus();
                    editLotsize.SelectAll();
                    return;
                }

                try
                {
                    int iReceiveQty = Convert.ToInt32(editReceiveQty.Text);

                    if (iReceiveQty > iLotSize)
                    {
                        SajetCommon.Show_Message(lablAcceptQty.Text + " > " + lablLotSize.Text, 0);
                        editReceiveQty.Focus();
                        editReceiveQty.SelectAll();
                        return;
                    }
                }
                catch
                {
                    SajetCommon.Show_Message(lablAcceptQty.Text+" "+SajetCommon.SetLanguage("Error"), 0);
                    editReceiveQty.Focus();
                    editReceiveQty.SelectAll();
                    return;
                }

               
            }
            editTypeMemo.Text = editTypeMemo.Text.Trim();
            editWaiveNo.Text = editWaiveNo.Text.Trim();
            if (!string.IsNullOrEmpty(g_sWaiveNo))
            {
                if (string.IsNullOrEmpty(editWaiveNo.Text))
                {
                    SajetCommon.Show_Message("Please Input Waive No", 0);
                    editWaiveNo.Focus();
                    return;
                }
            }
            if (SajetCommon.Show_Message("Lot Data will be change. Continue?", 2) != DialogResult.Yes)
                return;
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void editLotsize_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == 8 || e.KeyChar == 13 || e.KeyChar == 46))
            {
                e.KeyChar = (char)Keys.None;
            }
            if (e.KeyChar != (char)Keys.Return)
                return;
            btnSave.Focus();
        }

        private void fLotExceptionMemo_Activated(object sender, EventArgs e)
        {
            if (editLotsize.Enabled)
                editLotsize.Focus();
            else
                editTypeMemo.Focus();

        }
    }
}