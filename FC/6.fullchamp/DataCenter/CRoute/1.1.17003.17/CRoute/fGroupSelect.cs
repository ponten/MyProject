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
    public partial class fGroupSelect : Form
    {
        public fGroupSelect()
        {
            InitializeComponent();

            Shown += FGroupSelect_Shown;
        }

        private void FGroupSelect_Shown(object sender, EventArgs e)
        {
            radioButton2.Checked = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textGroupName.Text == "")
            {
                SajetCommon.Show_Message("Type Group Name !!", 0);
                return;
            }

            if (!radioButton1.Checked && !radioButton2.Checked)
            {
                SajetCommon.Show_Message("Please select a Group Mode !!",0);
                return;                
            }
            fEdit lForm1 = (fEdit)this.Owner;//把Form2的父窗口指針賦給lForm1            
            if (radioButton1.Checked)
                lForm1.Group_Text = textGroupName.Text + "\r\n" + radioButton1.Text; 
            if (radioButton2.Checked)
                lForm1.Group_Text = textGroupName.Text + "\r\n" + radioButton2.Text;
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void fGroupSelect_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
        }
    }
}
