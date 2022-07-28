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
    public partial class fData : Form
    {
        public fData()
        {
            InitializeComponent();
        }

        public string g_sKeyAll;
        public string g_sModiCode;
    //    public string sServerName;
        string sSQL;
        DataSet dsTemp;
        public int g_iPageIdx;

        private void combField_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sFix = "";
            string sField = combField.Text;
            sSQL = "select function_fix from sajet.sys_rule_field "
                 + "where field_name = '" + sField + "'";                       
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count >0)
            {
                sFix = dsTemp.Tables[0].Rows[0]["function_fix"].ToString();
            }

            fMain fMain = new fMain();
            fMain.Show_Function(sFix, combFunction);
            combFunction.Items.RemoveAt(0);
            fMain.Dispose();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {                        
            //檢查Code是否已重複
            TextBox editTemp;           
            if (tabControl1.SelectedTab == tabPageSeq)
            {
                editUSeqCode.Text = editUSeqCode.Text.Trim();
                editUSeq.Text = editUSeq.Text.Trim();
                if (editUSeqCode.Text == "")
                {
                    MessageBox.Show("Code is Empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    editUSeqCode.Focus();
                    return;
                }
                if (editUSeq.Text == "")
                {
                    MessageBox.Show("Sequence is Empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    editUSeq.Focus();
                    return;
                }
                editTemp = editUSeqCode;                
            }
            else
            {
                editFunCode.Text = editFunCode.Text.Trim();
                if (editFunCode.Text == "")
                {
                    MessageBox.Show("Code is Empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    editFunCode.Focus();
                    return;
                }
                if (combField.Text == "")
                {
                    MessageBox.Show("Field is Empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    combField.Focus();
                    return;
                }
                if (combFunction.Text == "")
                {
                    MessageBox.Show("Function is Empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    combFunction.Focus();
                    return;
                }
                editTemp = editFunCode;
            }

            if ((g_sKeyAll.IndexOf(editTemp.Text) != -1) && (editTemp.Text != g_sModiCode))
            {
                MessageBox.Show("Code Duplicate : " + editTemp.Text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                editTemp.Focus();
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void fData_Load(object sender, EventArgs e)
        {
            if (g_iPageIdx == 1)
                tabPageFun.Parent = null;
            else if (g_iPageIdx == 2)
                tabPageSeq.Parent = null;
            panel1.BackgroundImage = ClientUtils.LoadImage("imgButton.jpg");
            tabPageSeq.BackgroundImage = ClientUtils.LoadImage("imgButton.jpg");
            tabPageFun.BackgroundImage = ClientUtils.LoadImage("imgButton.jpg");
            ClientUtils.SetLanguage(this, fMain.g_sExeName);
        }                       
    }
}