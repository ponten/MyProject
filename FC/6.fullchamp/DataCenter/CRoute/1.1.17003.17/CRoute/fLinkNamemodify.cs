using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SajetClass;

namespace CRoute
{
    public partial class fLinkNamemodify : Form
    {
        public fLinkNamemodify()
        {
            InitializeComponent();
        }

        private string link_name;
        public string Link_Name
        {
            set
            {
                link_name = value;
            }
        }
        public void SetValue()
        {
            this.textBox1.Text = link_name.ToUpper();
        } 

        private void btOK_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.ToUpper() == "")
            {
                SajetCommon.Show_Message("Link Name Error !!", 0);
                return;
            }
            fEdit lForm1 = (fEdit)this.Owner;            
            lForm1.Link_Name = textBox1.Text.ToUpper();
            DialogResult = DialogResult.OK;            
            this.Close();
        }

        private void fLinkNamemodify_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (textBox1.Text.ToUpper() == "")
                {
                    SajetCommon.Show_Message("Link Name Error !!", 0);
                    return;
                }
                fEdit lForm1 = (fEdit)this.Owner;                
                lForm1.Link_Name = textBox1.Text.ToUpper();
                DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
