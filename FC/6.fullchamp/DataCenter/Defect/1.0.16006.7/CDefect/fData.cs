using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using SajetClass;
using SajetTable;
using SajetFilter;

namespace CDefect
{
    public partial class fData : Form
    {
        private fMain fMainControl;
        public fData()
        {
            InitializeComponent();
        }
        public fData(fMain f)
        {
            InitializeComponent();
            fMainControl = f;
        }
        public string g_sUpdateType, g_sformText;
        public string g_sKeyID;
        public DataGridViewRow dataCurrentRow;
        string sSQL;
        DataSet dsTemp;
        bool bAppendSucess = false;

        private void fData_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.Text = g_sformText;

            //Defect Level
            combDefectLevel.Items.Clear();
            for (int i = 0; i <= fMain.strListLevel.Count - 1; i++)
            {
                combDefectLevel.Items.Add(fMain.strListLevel[i].ToString());
            }                        
        
            combDefectLevel.SelectedIndex = 1; //預設選Critical
            combLevel.SelectedIndex = 0;

            if (g_sUpdateType == "MODIFY")
            {
                g_sKeyID = dataCurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();

                editCode.Text = dataCurrentRow.Cells["DEFECT_CODE"].Value.ToString();
                editDesc.Text = dataCurrentRow.Cells["DEFECT_DESC"].Value.ToString();
                editDesc2.Text = dataCurrentRow.Cells["DEFECT_DESC2"].Value.ToString();
                combDefectLevel.SelectedIndex = combDefectLevel.Items.IndexOf(dataCurrentRow.Cells["DEFECTLEVEL"].Value.ToString());
                //combDefectType.Text = dataCurrentRow.Cells["DEFECT_TYPE"].Value.ToString();                
                editDefectTypeCode.Text = dataCurrentRow.Cells["DEFECT_TYPE_CODE"].Value.ToString();
                editDefectType.Text = dataCurrentRow.Cells["DEFECT_TYPE_DESC"].Value.ToString();
                combLevel.SelectedIndex = combLevel.Items.IndexOf(dataCurrentRow.Cells["CODE_LEVEL"].Value.ToString());
                editParentDefect.Text = dataCurrentRow.Cells["PARENT_DEFECT_CODE"].Value.ToString();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= panelControl.Controls.Count - 1; i++)
            {
                if (panelControl.Controls[i] is TextBox)
                {
                    panelControl.Controls[i].Text = panelControl.Controls[i].Text.Trim();
                }
            }

            if (editCode.Text == "")
            {
                string sData = LabCode.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editCode.Focus();
                editCode.SelectAll();
                return;
            }

            if (combDefectLevel.SelectedIndex == -1)
            {
                string sData = LabDefectLevel.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                combDefectLevel.Focus();
                combDefectLevel.SelectAll();
                return;
            }

            //檢查Code是否重複
            sSQL = " Select * from " + TableDefine.gsDef_Table
                 + " Where DEFECT_CODE = '" + editCode.Text + "' ";
            if (g_sUpdateType == "MODIFY")
                sSQL = sSQL + " and " + TableDefine.gsDef_KeyField + " <> '" + g_sKeyID + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                string sData = LabCode.Text + " : " + editCode.Text;
                string sMsg = SajetCommon.SetLanguage("Data Duplicate", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editCode.Focus();
                editCode.SelectAll();
                return;
            }
            

            //檢查上下階關係
            string sLevel = combLevel.Text;
            if (string.IsNullOrEmpty(sLevel))
                sLevel = "1"; 
            string sParentCode = editParentDefect.Text;            
            if (!Check_Level(sParentCode, sLevel))
            {                
                return;
            }
            if (editParentDefect.Text != "")
            {
                if (!Check_ParentDefect(editParentDefect.Text))
                    return;
            }

            //Update DB
            try
            {
                if (g_sUpdateType == "APPEND")
                {
                    AppendData();
                    bAppendSucess = true;
                    string sMsg = SajetCommon.SetLanguage("Data Append OK", 2) + " !" + Environment.NewLine + SajetCommon.SetLanguage("Append Other Data", 2) + " ?";
                    if (fMainControl != null) fMainControl.ShowData();
                    if (SajetCommon.Show_Message(sMsg, 2) == DialogResult.Yes)
                    {
                        ClearData();
                        combDefectLevel.SelectedIndex = 1;
                        combLevel.SelectedIndex = 0;
                        editCode.Focus();
                        return;
                    }
                    DialogResult = DialogResult.OK;
                }
                else if (g_sUpdateType == "MODIFY")
                {
                    ModifyData();
                    DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception : " + ex.Message, 0);
                return;
            }
        }

        private void AppendData()
        {
            string sMaxID = SajetCommon.GetMaxID("SAJET.SYS_DEFECT", "DEFECT_ID", 8);
            string sParentDefectID = GetID("SAJET.SYS_DEFECT", "DEFECT_ID", "DEFECT_CODE", editParentDefect.Text);
            string g_sDefectTypeID = SajetCommon.GetID("sajet.sys_defect_type", "defect_type_id", "defect_type_code", editDefectTypeCode.Text.Trim());
            object[][] Params = new object[9][];
            sSQL = " Insert into SAJET.SYS_DEFECT "
                 + " (DEFECT_ID,DEFECT_CODE,DEFECT_DESC,DEFECT_DESC2 "
                 + " ,DEFECT_LEVEL,DEFECT_TYPE_ID,CODE_LEVEL,PARENT_DEFECT_ID "
                 + " ,ENABLED,UPDATE_USERID,UPDATE_TIME) "
                 + " Values "
                 + " (:DEFECT_ID,:DEFECT_CODE,:DEFECT_DESC,:DEFECT_DESC2 "
                 + " ,:DEFECT_LEVEL,:DEFECT_TYPE_ID,:CODE_LEVEL,:PARENT_DEFECT_ID "
                 + " ,'Y',:UPDATE_USERID,SYSDATE) ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DEFECT_ID", sMaxID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DEFECT_CODE", editCode.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DEFECT_DESC", editDesc.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DEFECT_DESC2", editDesc2.Text };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DEFECT_LEVEL", combDefectLevel.SelectedIndex };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DEFECT_TYPE_ID", g_sDefectTypeID };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CODE_LEVEL", combLevel.Text };
            Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PARENT_DEFECT_ID", sParentDefectID };
            Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            fMain.CopyToHistory(sMaxID);
        }
        private void ModifyData()
        {
            string sParentDefectID = GetID("SAJET.SYS_DEFECT", "DEFECT_ID", "DEFECT_CODE", editParentDefect.Text);
            string g_sDefectTypeID = SajetCommon.GetID("sajet.sys_defect_type", "defect_type_id", "defect_type_code", editDefectTypeCode.Text.Trim());
            object[][] Params = new object[9][];
            sSQL = " Update SAJET.SYS_DEFECT "
                 + " set DEFECT_CODE = :DEFECT_CODE "
                 + "    ,DEFECT_DESC = :DEFECT_DESC "
                 + "    ,DEFECT_DESC2 = :DEFECT_DESC2 "
                 + "    ,DEFECT_LEVEL = :DEFECT_LEVEL "
                 + "    ,DEFECT_TYPE_ID = :DEFECT_TYPE_ID "
                 + "    ,CODE_LEVEL = :CODE_LEVEL "
                 + "    ,PARENT_DEFECT_ID = :PARENT_DEFECT_ID "
                 + "    ,UPDATE_USERID = :UPDATE_USERID "
                 + "    ,UPDATE_TIME = SYSDATE "
                 + " where DEFECT_ID = :DEFECT_ID ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DEFECT_CODE", editCode.Text };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DEFECT_DESC", editDesc.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DEFECT_DESC2", editDesc2.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DEFECT_LEVEL", combDefectLevel.SelectedIndex };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DEFECT_TYPE_ID", g_sDefectTypeID };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CODE_LEVEL", combLevel.Text };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PARENT_DEFECT_ID", sParentDefectID };
            Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DEFECT_ID", g_sKeyID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            fMain.CopyToHistory(g_sKeyID);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (bAppendSucess)
                DialogResult = DialogResult.OK;
        }

        private string GetID(string sTable, string sFieldID, string sFieldName, string sValue)
        {
            if (string.IsNullOrEmpty(sValue))
                return "0";
            sSQL = "select " + sFieldID + " from " + sTable + " "
                 + "where " + sFieldName + " = '" + sValue + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
                return dsTemp.Tables[0].Rows[0][sFieldID].ToString();
            else
                return "0";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string sLevel = "1";
            if (combLevel.SelectedIndex == -1)
                sLevel = "1";
            else
                sLevel = Convert.ToString(Convert.ToInt32(combLevel.Text) - 1);

            sSQL = "Select Defect_Code,Defect_Desc,Code_Level "
                 + "From SAJET.SYS_DEFECT "
                 + "Where DEFECT_CODE Like '" + editParentDefect.Text + "%' "
                 + "and CODE_LEVEL = '" + sLevel + "' "
                 + "and ENABLED = 'Y' "
                 + "Order By Defect_Code ";
            fFilter f = new fFilter();           
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                editParentDefect.Text = f.dgvData.CurrentRow.Cells["Defect_Code"].Value.ToString();
                KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);
                editParentDefect_KeyPress(sender, Key);
            }
            f.Dispose();
        }

        private void editParentDefect_KeyPress(object sender, KeyPressEventArgs e)
        {            
            if (e.KeyChar != (char)Keys.Return)
                return;

            if (!Check_ParentDefect(editParentDefect.Text))
                return;
            Check_Level(editParentDefect.Text, combLevel.Text);
        }

        public bool Check_ParentDefect(string sDefectCode)
        {
            sSQL = " Select Defect_Id, Defect_Code, Defect_Desc "
                 + " From SAJET.SYS_Defect "
                 + " Where Enabled = 'Y' "
                 + " and Defect_Code = '" + sDefectCode + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);           
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                string sData = LabParentDefect.Text + " : " + sDefectCode;
                string sMsg = SajetCommon.SetLanguage("Parent Defect Code Error", 1)
                            + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editParentDefect.Focus();
                editParentDefect.SelectAll();
                return false;
            }           
            return true;
        }
        public bool Check_Level(string sParentCode, string sLevel)
        {
            //檢查上下階關係
            if (sLevel == "1")
            {
                //若為第一層,Parent Code不可輸入
                if (!string.IsNullOrEmpty(sParentCode))
                {
                    SajetCommon.Show_Message("Level-1 Can not input Parent Defect Code", 0);
                    return false;
                }
            }
            else
            {
                //若不為第一層,Parent Code要輸入
                if (string.IsNullOrEmpty(sParentCode))
                {
                    SajetCommon.Show_Message("Please input Parent Defect Code", 0);
                    return false;
                }
            }
            return true;
        }

        private void ClearData()
        {
            for (int i = 0; i <= panelControl.Controls.Count - 1; i++)
            {
                if (panelControl.Controls[i] is TextBox)
                {
                    panelControl.Controls[i].Text = "";
                }
                else if (panelControl.Controls[i] is ComboBox)
                {
                    ((ComboBox)panelControl.Controls[i]).SelectedIndex = -1;
                }
            }
        }

        private void btnDefectTypeSearch_Click(object sender, EventArgs e)
        {
            sSQL = "Select Defect_Type_Code,Defect_Type_Desc "
                 + "From SAJET.SYS_DEFECT_TYPE "
                 + "Where DEFECT_TYPE_CODE Like '" + editDefectTypeCode.Text.Trim() + "%' "
                 + "and ENABLED = 'Y' "
                 + "Order By Defect_Type_Code ";
            fFilter f = new fFilter();
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                editDefectTypeCode.Text = f.dgvData.CurrentRow.Cells["Defect_Type_Code"].Value.ToString();
                KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);
                editDefectTypeCode_KeyPress(sender, Key);
            }
            f.Dispose();
        }

        private void editDefectTypeCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            if (!Check_DefectTypeCode(editDefectTypeCode.Text.Trim()))
                return;
            editDefectType.Text = SajetCommon.GetID("sajet.sys_defect_type", "defect_type_desc", "defect_type_code", editDefectTypeCode.Text.Trim());            
        }
        public bool Check_DefectTypeCode(string sDefectTypeCode)
        {
            sSQL = " Select Defect_Type_Id, Defect_Type_Code, Defect_Type_Desc "
                 + " From SAJET.SYS_Defect_Type "
                 + " Where Enabled = 'Y' "
                 + " and Defect_Type_Code = '" + sDefectTypeCode + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                string sData = LabDefectTypeCode.Text + " : " + sDefectTypeCode;
                string sMsg = SajetCommon.SetLanguage("Defect Type Code Error", 1)
                            + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editDefectTypeCode.Focus();
                editDefectTypeCode.SelectAll();
                return false;
            }
            return true;
        }
    }
}