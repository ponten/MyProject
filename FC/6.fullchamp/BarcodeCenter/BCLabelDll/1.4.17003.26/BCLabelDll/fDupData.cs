using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;

namespace BCLabelDll
{
    public partial class fDupData : Form
    {
        public fDupData()
        {
            InitializeComponent();
        }

        private void fDupData_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            this.Text = SajetCommon.SetLanguage("Error") + ":" + SajetCommon.SetLanguage("Duplicate Data");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
