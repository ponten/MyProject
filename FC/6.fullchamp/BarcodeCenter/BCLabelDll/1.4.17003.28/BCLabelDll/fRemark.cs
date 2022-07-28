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
    public partial class fRemark : Form
    {
        public fRemark()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(richTextBox1.Text))
            {
                SajetCommon.Show_Message("Remark is null", 0);
                return;
            }
            DialogResult = DialogResult.OK;
        }

        private void fRemark_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            panel1.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            label1.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            label1.BackgroundImageLayout = ImageLayout.Stretch;


        }        
    }
}