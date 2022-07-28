using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;

namespace BCRuleDll
{
    public partial class fModifyRule : Form
    {
        public fModifyRule()
        {
            InitializeComponent();
        }

        public string g_sUpdateRuleID;
        public string g_MaintainType;

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(editRuleName.Text.Trim()))
            {
                SajetCommon.Show_Message("Rule Name is null",0);
                editRuleName.Focus();
                return;
            }

            string sSQL = "Select RULE_ID from sajet.sys_rule_name "
                        + "Where Rule_Name = '" + editRuleName.Text + "' ";
            if (g_MaintainType == "Modify")
                sSQL = sSQL + " and Rule_ID <> '" + g_sUpdateRuleID + "' ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                SajetCommon.Show_Message("Rule Name duplicate", 0);
                editRuleName.Focus();
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void fModifyRule_Load(object sender, EventArgs e)
        {
            this.Text = g_MaintainType + " Rule Name";
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            panel1.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel1.BackgroundImageLayout = ImageLayout.Stretch;

            SajetCommon.SetLanguageControl(this);
        }       

        private void editGroupQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == 8 || e.KeyChar == 13 || e.KeyChar == 46))
            {
                e.KeyChar = (char)Keys.None;
            }
        }
    }
}