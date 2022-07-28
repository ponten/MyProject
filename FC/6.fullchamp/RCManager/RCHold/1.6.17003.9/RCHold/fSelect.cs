using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Data.OracleClient;

namespace RCHold
{
    public partial class fSelect : Form
    {
        public fSelect()
        {
            InitializeComponent();
        }

        public string sCurrent_Status, sStatus;

        private void fSelect_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            sCurrent_Status = "";
            sStatus = "";

            if ((RBtn_Hold.Checked == false) && (RBtn_WaitTransfer.Checked == false))
            {
                return;
            }

            try
            {
                // Hold
                if (RBtn_Hold.Checked)
                {
                    sCurrent_Status = "2";
                    sStatus = "HOLD";

                    DialogResult = DialogResult.OK;
                }

                // Wait Transfer
                if (RBtn_WaitTransfer.Checked)
                {
                    sCurrent_Status = "11";
                    sStatus = "WAIT TRANSFER";

                    DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception:" + ex.Message, 0);
            }
        }

        private void btnCANCEL_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
