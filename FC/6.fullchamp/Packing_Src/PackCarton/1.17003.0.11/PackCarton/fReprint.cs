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
    public partial class fReprint : Form
    {
        public fReprint()
        {
            InitializeComponent();
        }

        private void txtCarton_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            txtCarton.Text = txtCarton.Text.Trim();

            if (txtCarton.Text == "")
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Key Carton"), 1);
                return;
            }

            string sSQL = @"SELECT * FROM SAJET.G_PACK_CARTON WHERE CARTON_NO = '" + txtCarton.Text.Trim() + "' AND CLOSE_FLAG= 'Y'";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Carton Error"), 1);
                return;
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void fReprint_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
        }
    }
}
