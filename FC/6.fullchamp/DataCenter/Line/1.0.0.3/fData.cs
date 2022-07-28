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

namespace CLine
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

        public string g_sUpdateType;
        public string g_sKeyID;
        public DataGridViewRow dataCurrentRow;
        string sSQL;
        DataSet dsTemp;
        bool bAppendSucess = false;
        public string g_sFactoryID, g_sFactoryName, g_sformText;

        private void fData_Load(object sender, EventArgs e)
        {
            this.Text = g_sformText;
            SajetCommon.SetLanguageControl(this);
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;

            LabFactoryName.Text = g_sFactoryName;
            combShift.Items.Clear();
            combShift.Items.Add("");
            sSQL = " select shift_group_name from sajet.sys_shift_group "
                 + " where enabled = 'Y' "
                 + " order by shift_group_name ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                combShift.Items.Add(dsTemp.Tables[0].Rows[i]["shift_group_name"].ToString());
            }


            if (g_sUpdateType == "MODIFY")
            {
                g_sKeyID = dataCurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();

                editName.Text = dataCurrentRow.Cells["PDLINE_NAME"].Value.ToString();
                editDesc.Text = dataCurrentRow.Cells["PDLINE_DESC"].Value.ToString();
                editDesc2.Text = dataCurrentRow.Cells["PDLINE_DESC2"].Value.ToString();
                combShift.SelectedIndex = combShift.Items.IndexOf(dataCurrentRow.Cells["SHIFT_GROUP_NAME"].Value.ToString());
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

            if (editName.Text == "")
            {
                string sData = LabName.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);

                editName.Focus();
                editName.SelectAll();
                return;
            }           
           
            //檢查Name是否重複
            sSQL = " Select * from " + TableDefine.gsDef_Table + " "
                 + " Where PDLINE_NAME = '" + editName.Text + "' ";
            if (g_sUpdateType == "MODIFY")
                sSQL = sSQL + " and " + TableDefine.gsDef_KeyField + " <> '" + g_sKeyID + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                string sData = LabName.Text + " : " + editName.Text;
                string sMsg = SajetCommon.SetLanguage("Data Duplicate", 2) 
                            + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editName.Focus();
                editName.SelectAll();
                return;
            }

            //Update DB
            try
            {
                if (g_sUpdateType == "APPEND")
                {
                    AppendData();
                    bAppendSucess = true;
                    string sMsg = SajetCommon.SetLanguage("Data Append OK", 2) + " !" 
                                + Environment.NewLine 
                                + SajetCommon.SetLanguage("Append Other Data", 2) + " ?";
                    if (fMainControl != null) fMainControl.ShowData();//新增後即時顯示新增資料在表格上
                    if (SajetCommon.Show_Message(sMsg, 2) == DialogResult.Yes)
                    {
                        ClearData();

                        editName.Focus();
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
            string sMaxID = SajetCommon.GetMaxID(TableDefine.gsDef_Table, TableDefine.gsDef_KeyField, 5);
            string sShiftID = SajetCommon.GetID("SAJET.SYS_SHIFT_GROUP", "SHIFT_GROUP_ID", "SHIFT_GROUP_NAME", combShift.Text);
            object[][] Params = new object[7][];
            sSQL = " Insert into SAJET.SYS_PDLINE "
                 + " (FACTORY_ID,PDLINE_ID,PDLINE_NAME,PDLINE_DESC,PDLINE_DESC2"
                 + " ,ENABLED,UPDATE_USERID,UPDATE_TIME,SHIFT_GROUP_ID ) "
                 + " Values "
                 + " (:FACTORY_ID,:PDLINE_ID,:PDLINE_NAME,:PDLINE_DESC,:PDLINE_DESC2 "
                 + " ,'Y',:UPDATE_USERID,SYSDATE,:SHIFT_GROUP_ID) ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FACTORY_ID", g_sFactoryID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PDLINE_ID", sMaxID };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PDLINE_NAME", editName.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PDLINE_DESC", editDesc.Text };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PDLINE_DESC2", editDesc2.Text };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHIFT_GROUP_ID", sShiftID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            fMain.CopyToHistory(sMaxID);
        }
        private void ModifyData()
        {
            string sShiftID = SajetCommon.GetID("SAJET.SYS_SHIFT_GROUP", "SHIFT_GROUP_ID", "SHIFT_GROUP_NAME", combShift.Text);
            object[][] Params = new object[6][];
            sSQL = " Update SAJET.SYS_PDLINE "
                 + " set PDLINE_NAME = :PDLINE_NAME "
                 + "    ,PDLINE_DESC = :PDLINE_DESC "
                 + "    ,PDLINE_DESC2 = :PDLINE_DESC2 "
                 + "    ,UPDATE_USERID = :UPDATE_USERID "
                 + "    ,UPDATE_TIME = SYSDATE "
                 + "    ,SHIFT_GROUP_ID =:SHIFT_GROUP_ID "
                 + " where PDLINE_ID = :PDLINE_ID ";            
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PDLINE_NAME", editName.Text };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PDLINE_DESC", editDesc.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PDLINE_DESC2", editDesc2.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHIFT_GROUP_ID", sShiftID };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PDLINE_ID", g_sKeyID };                       
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            fMain.CopyToHistory(g_sKeyID);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (bAppendSucess)
                DialogResult = DialogResult.OK;
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

        private void editName_TextChanged(object sender, EventArgs e)
        {

        }

        private void editDesc_TextChanged(object sender, EventArgs e)
        {

        }

        private void editDesc2_TextChanged(object sender, EventArgs e)
        {

        }

        private void combShift_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void LabFactoryName_Click(object sender, EventArgs e)
        {

        }
    }
}