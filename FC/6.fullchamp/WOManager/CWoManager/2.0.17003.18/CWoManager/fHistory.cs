using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;

namespace CWoManager
{
    public partial class fHistory : Form
    {
        public fHistory()
        {
            InitializeComponent();
        }

        private void btnExp_Click(object sender, EventArgs e)
        {
            saveFileDialog1.DefaultExt = "xls";
            saveFileDialog1.Filter = "All Files(*.xls)|*.xls";
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            string sFileName = saveFileDialog1.FileName;

            ExportExcel.CreateExcel Export = new ExportExcel.CreateExcel(sFileName);
            Export.ExportToExcel(dgvHistory);
        }

        private void fHistory_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
        }
    }
}