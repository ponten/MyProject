using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using SajetClass;

namespace BCRuleSetDll
{
    public partial class fRuleMainTain : Form
    {
        public fRuleMainTain()
        {
            InitializeComponent();
        }
        public static string g_sExeName;
        public string g_sMaintainType,g_sRuleName;        
        string sSQL;
        DataSet dsTemp;
        object[][] Params;

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (editRuleName.Text == "")
            {
                SajetCommon.Show_Message("Rule Name is Empty", 0);
                editRuleName.Focus();
                return;
            }
            if (g_sMaintainType == "Append")
            {
                if (string.IsNullOrEmpty(editRuleName.Text.Trim()))
                {
                    SajetCommon.Show_Message("Rule Name is null", 0);
                    editRuleName.Focus();
                    return;
                }
                sSQL = @"SELECT FUNCTION_NAME FROM sajet.SYS_MODULE_PARAM 
                         WHERE MODULE_NAME = 'W/O RULE' 
                         AND FUNCTION_NAME = :FUNCTION_NAME";
                Params = new object[1][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FUNCTION_NAME", editRuleName.Text.Trim() };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    SajetCommon.Show_Message("Rule Name duplicate", 0);
                    editRuleName.Focus();
                    return;
                }
                g_sRuleName = editRuleName.Text.Trim();
                DialogResult = DialogResult.OK;
            }
            if (g_sMaintainType == "Modify")
            {
                
                if (string.IsNullOrEmpty(editRuleName.Text.Trim()))
                {
                    SajetCommon.Show_Message("Rule Name is null", 0);
                    editRuleName.Focus();
                    return;
                }
                string dup_sRuleName = g_sRuleName;
                sSQL = @"SELECT FUNCTION_NAME FROM SAJET.SYS_MODULE_PARAM 
                         WHERE MODULE_NAME = 'W/O RULE' 
                         AND FUNCTION_NAME = :FUNCTION_NAME
                         AND FUNCTION_NAME NOT IN (:DUP_RULENAME)";
                Params = new object[2][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FUNCTION_NAME", editRuleName.Text.Trim() };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DUP_RULENAME", dup_sRuleName };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    SajetCommon.Show_Message("Rule Name duplicate", 0);
                    editRuleName.Focus();
                    return;
                }

                sSQL = @"UPDATE SAJET.SYS_MODULE_PARAM SET FUNCTION_NAME = :NEW_FUNNAME
                         WHERE FUNCTION_NAME = :OLD_FUNNAME AND MODULE_NAME = 'W/O RULE'";
                Params = new object[2][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NEW_FUNNAME", editRuleName.Text.Trim() };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OLD_FUNNAME", g_sRuleName};
                dsTemp = ClientUtils.ExecuteSQL(sSQL,Params);
                SajetCommon.Show_Message("Update Data Complete",4);
                DialogResult = DialogResult.OK;
            }
            //string sRule = editRuleName.Text;
            //SaveRuleData(sRule);
            //SajetCommon.Show_Message("Save OK", -1);
        }

        private void fRuleMainTain_Load(object sender, EventArgs e)
        {
            g_sExeName = ClientUtils.fCurrentProject;
            ClientUtils.SetLanguage(this, g_sExeName);
            this.Text = g_sMaintainType;
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            panel1.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
            if (g_sMaintainType == "Modify")
                editRuleName.Text = g_sRuleName;
        }
        
    }
}
