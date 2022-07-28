using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using SajetClass;

namespace BCRuleDll
{
    public partial class fResetSeq : Form
    {
        public fResetSeq()
        {
            InitializeComponent();
        }

        public string gsSeqName;
        public string sServerName;

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Reset Sequence ? ", "Conform", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            string sSeqName;
            for (int i = 0; i <= LVReset.Items.Count - 1; i++)
            {
                if (LVReset.Items[i].Checked)
                {
                    if (LVReset.Items[i].Text != "")
                        sSeqName = gsSeqName + "_" + LVReset.Items[i].Text;
                    else
                        sSeqName = gsSeqName;
                    string sSQL = "drop sequence " + sSeqName;
                    DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
                }
            }
            DialogResult = DialogResult.OK;
        }

        private void fResetSeq_Load(object sender, EventArgs e)
        {
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            ClientUtils.SetLanguage(this, fMain.g_sExeName);
        }
    }
}