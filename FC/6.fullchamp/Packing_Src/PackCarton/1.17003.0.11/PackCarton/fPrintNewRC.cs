using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SajetClass;

namespace PackCarton
{
    public partial class fPrintNewRC : Form
    {
        string RC_NO;
        int QTY;

        public fPrintNewRC(string sNew_RC_NO, int iQTY)
        {
            InitializeComponent();

            RC_NO = sNew_RC_NO;
            QTY = iQTY;
        }

        private void fPrintNewRC_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);

            tbRC.Text = RC_NO;
            tbQTY.Text = QTY.ToString();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
        }
    }
}
