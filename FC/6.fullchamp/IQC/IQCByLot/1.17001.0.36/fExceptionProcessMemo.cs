using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
namespace IQCbyLot
{
    public partial class fExceptionProcessMemo : Form
    {
        string g_sLotNo;
        public string g_sMemo;

        public fExceptionProcessMemo(string sLotNo, string sStatus)
        {
            InitializeComponent();
            lablLotNo.Text = sLotNo;
            if (sStatus == "Y")
                lablStatus.Text =  SajetCommon.SetLanguage("Start Exception Process");
            else
                lablStatus.Text =  SajetCommon.SetLanguage("Stop Exception Process") ;
        }

        private void fExceptionProcessMemo_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
          
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            editTypeMemo.Text = editTypeMemo.Text.Trim();
            if (string.IsNullOrEmpty(editTypeMemo.Text))
            {
                SajetCommon.Show_Message("Please Input Memo", 0);
                editTypeMemo.Focus();
                editTypeMemo.SelectAll();
                return;
            }
            g_sMemo = editTypeMemo.Text;
            DialogResult = DialogResult.OK;
        }
    }
}