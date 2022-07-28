using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BCLabelDll
{
    public partial class fChange : Form
    {
        public fChange()
        {
            InitializeComponent();
        }

        public string sTitle = "";
        private void fChange_Load(object sender, EventArgs e)
        {
            LabDefValue.Text = sTitle;
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            ClientUtils.SetLanguage(this, fMain.g_sExeName);
        }
    }
}