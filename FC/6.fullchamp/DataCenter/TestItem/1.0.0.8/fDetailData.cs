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

namespace CTestItem
{
    public partial class fDetailData : Form
    {
        public fDetailData()
        {
            InitializeComponent();
        }
        public string g_sUpdateType, g_sformText;
        public string g_sKeyID;
        public DataGridViewRow dataCurrentRow;
        string sSQL;
        DataSet dsTemp;
        bool bAppendSucess = false;
        public string g_sTypeName, g_sTypeID;

        private void fData_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.Text = g_sformText; 
            LabTypeName.Text = g_sTypeName;
            //combHasValue.SelectedIndex = 0;

            rbtnNumber.Checked = true;
            if (g_sUpdateType == "MODIFY")
            {
                g_sKeyID = dataCurrentRow.Cells[TableDefine.gsDef_DtlKeyField].Value.ToString();

                editName.Text = dataCurrentRow.Cells["ITEM_NAME"].Value.ToString();
                editDesc.Text = dataCurrentRow.Cells["ITEM_DESC"].Value.ToString();
                editDesc2.Text = dataCurrentRow.Cells["ITEM_DESC2"].Value.ToString();
                editCode.Text = dataCurrentRow.Cells["ITEM_CODE"].Value.ToString();
                editInspQty.Text = dataCurrentRow.Cells["MIN_INSP_QTY"].Value.ToString();                
                combHasValue.SelectedIndex = dataCurrentRow.Cells["HAS_VALUE"].Value.ToString() == "Y" ? 0 : 1;
                if (gbValueType.Visible == true)
                {
                    ((RadioButton)gbValueType.Controls["rbtn" + dataCurrentRow.Cells["VALUETYPE"].Value.ToString()]).Checked = true;
                }
            }
            MaxCItemValue();
            //sSQL = " SELECT MAX(ITEM_CODE) FROM SAJET.SYS_TEST_ITEM "
            //     + " WHERE ITEM_TYPE_ID = '" + g_sTypeID +"'"
            //     + " AND ENABLED = 'Y'";
            //dsTemp = ClientUtils.ExecuteSQL(sSQL);
            //if (dsTemp.Tables[0].Rows.Count > 0)
            //{
            //    maxItemCode.Text = dsTemp.Tables[0].Rows[0][0].ToString();
            //}           
        }

        private void MaxCItemValue()
        {
            sSQL = " SELECT MAX(ITEM_CODE) FROM SAJET.SYS_TEST_ITEM "
                  + " WHERE ITEM_TYPE_ID = '" + g_sTypeID + "'"
                  + " AND ENABLED = 'Y'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                maxItemCode.Text = dsTemp.Tables[0].Rows[0][0].ToString();
            }
            combHasValue.SelectedIndex = 0;
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

            //測試小項代碼及名稱不能為空值
            if (editName.Text == "") 
            {
                string sData = LabName.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editName.Focus();
                editName.SelectAll();
                return;
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
            
            //檢查code是否重複
            sSQL = " Select * from SAJET.SYS_test_item "
                 + " Where item_code = '" + editCode.Text.ToUpper() + "' "
                 + " and item_type_id = '" + g_sTypeID + "'";
            if (g_sUpdateType == "MODIFY")
                sSQL = sSQL + " and item_id <> '" + g_sKeyID + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                string sData = LabName.Text + " : " + editCode.Text;
                string sMsg = SajetCommon.SetLanguage("Data Duplicate", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editCode.Focus();
                editCode.SelectAll();
                return;
            }        
            //檢查Name是否重複
            sSQL = " Select * from SAJET.SYS_test_item "
                 + " Where item_NAME = '" + editName.Text + "' "
                 + " and item_type_id = '" + g_sTypeID + "'";
            if (g_sUpdateType == "MODIFY")
                sSQL = sSQL + " and item_id <> '" + g_sKeyID + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                string sData = LabName.Text + " : " + editName.Text;
                string sMsg = SajetCommon.SetLanguage("Data Duplicate", 2) + Environment.NewLine + sData;
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
                    string sMsg = SajetCommon.SetLanguage("Data Append OK", 2) + " !" + Environment.NewLine
                        + SajetCommon.SetLanguage("Append Other Data", 2) + " ?";
                    if (SajetCommon.Show_Message(sMsg, 2) == DialogResult.Yes)
                    {
                        ClearData();
                        MaxCItemValue();
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
            string sMaxID = SajetCommon.GetMaxID("SAJET.SYS_TEST_ITEM", "ITEM_ID", 8);
            string sValueType = "";
            if (combHasValue.SelectedIndex == 0)
            {
                if (rbtnNumber.Checked == true)
                {
                    sValueType = "0";
                }
                else if (rbtnCharacter.Checked == true)
                {
                    sValueType = "1";
                }
            }
            object[][] Params = new object[10][];    
            sSQL = " Insert into SAJET.SYS_TEST_ITEM "
                 + " (ITEM_ID,ITEM_NAME,ITEM_DESC,ITEM_DESC2,ITEM_CODE,HAS_VALUE,VALUE_TYPE "
                 + " ,ITEM_TYPE_ID "
                 + " ,ENABLED,UPDATE_USERID,UPDATE_TIME,MIN_INSP_QTY) "
                 + " Values "
                 + " (:ITEM_ID,:ITEM_NAME,:ITEM_DESC,:ITEM_DESC2,:ITEM_CODE,:HAS_VALUE,:VALUE_TYPE "
                 + " ,:ITEM_TYPE_ID "
                 + ",'Y',:UPDATE_USERID,SYSDATE,:MIN_INSP_QTY) ";   
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_ID", sMaxID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_NAME", editName.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_DESC", editDesc.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_DESC2", editDesc2.Text };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_CODE", editCode.Text.ToUpper() };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "HAS_VALUE", combHasValue.Text };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VALUE_TYPE", sValueType };
            Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", g_sTypeID };
            Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MIN_INSP_QTY",editInspQty.Text}; //-------20110917
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            fMain.CopyToDetailHistory(sMaxID);
        }
        private void ModifyData()
        {
            string sValueType = "";
            if (combHasValue.SelectedIndex == 0)
            {
                if (rbtnNumber.Checked == true)
                {
                    sValueType = "0";
                }
                else if (rbtnCharacter.Checked == true)
                {
                    sValueType = "1";
                }
            }
            object[][] Params = new object[10][];
            sSQL = " Update SAJET.SYS_TEST_ITEM "
                 + " set ITEM_CODE = :ITEM_CODE "
                 + "    ,ITEM_NAME = :ITEM_NAME "
                 + "    ,ITEM_DESC = :ITEM_DESC "
                 + "    ,ITEM_DESC2 = :ITEM_DESC2 "
                 + "    ,HAS_VALUE = :HAS_VALUE "
                 + "    ,VALUE_TYPE = :VALUE_TYPE "
                 + "    ,ITEM_TYPE_ID = :ITEM_TYPE_ID "
                 + "    ,UPDATE_USERID = :UPDATE_USERID "
                 + "    ,UPDATE_TIME = SYSDATE "
                 + "    ,MIN_INSP_QTY = :MIN_INSP_QTY"
                 + " where ITEM_ID = :ITEM_ID ";

            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_CODE", editCode.Text.ToUpper() };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_NAME", editName.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_DESC", editDesc.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_DESC2", editDesc2.Text };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "HAS_VALUE", combHasValue.Text };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VALUE_TYPE", sValueType };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", g_sTypeID };
            Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_ID", dataCurrentRow.Cells["ITEM_ID"].Value.ToString() };
            Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MIN_INSP_QTY", editInspQty.Text }; //-------20110917
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            fMain.CopyToDetailHistory(g_sKeyID);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (bAppendSucess)
                DialogResult = DialogResult.OK;
        }
     
        private void combHasValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combHasValue.SelectedIndex == 0)
            {
                gbValueType.Visible = true;
            }
            else
            {
                gbValueType.Visible = false;
            }
            rbtnNumber.Checked = true;
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

    }
}