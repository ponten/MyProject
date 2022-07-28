using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace BCRuleDll
{
    public partial class fDayCode : Form
    {
        public fDayCode()
        {
            InitializeComponent();
        }

        public int iMaxCount;

        private void fDefineCode_Load(object sender, EventArgs e)
        {
            for (int i = 0; i <= iMaxCount-1; i++)
            {                
                gvValue.Rows.Add();
                gvValue.Rows[i].Cells[0].Value = Convert.ToString(i+1);
                if (i <= editCode.Lines.Length-1)
                    gvValue.Rows[i].Cells[1].Value = editCode.Lines[i].ToString();
                else
                    gvValue.Rows[i].Cells[1].Value = "";
            }
            ClientUtils.SetLanguage(this, fMain.g_sExeName);
        }

        private void bbtnOK_Click(object sender, EventArgs e)
        {
            string sValue = "";
            for (int i = 0; i <= gvValue.Rows.Count - 1; i++)
            {
                if (string.IsNullOrEmpty(gvValue.Rows[i].Cells[1].Value.ToString()))
                {
                    MessageBox.Show("Value is Empty","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }
                sValue = sValue + gvValue.Rows[i].Cells[1].Value.ToString()+",";
            }            
            editCode.Text = sValue;
            DialogResult = DialogResult.OK;
        }
    }
}