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
    public partial class fTypeMemo : Form
    {
        public string g_sLotNo;
        public string g_sTypeID;
        public string g_sTypeName;

        public fTypeMemo()
        {
            InitializeComponent();
        }

        private void fTypeMemo_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            lablLotNo.Text = g_sLotNo;
            lablTypeName.Text = g_sTypeName;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            editTypeMemo.Text = editTypeMemo.Text.Trim();
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void fTypeMemo_Activated(object sender, EventArgs e)
        {
            editTypeMemo.Focus();
        }
    }
}