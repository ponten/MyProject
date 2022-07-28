using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;

namespace InvMaterialdll
{
    public partial class fSelectRTSeq : Form
    {
        public string g_sSeq;
        public fSelectRTSeq()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            g_sSeq = combRTSeq.Text;
            DialogResult = DialogResult.OK;
        }

        private void fSelectRTSeq_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
        }
    }
}