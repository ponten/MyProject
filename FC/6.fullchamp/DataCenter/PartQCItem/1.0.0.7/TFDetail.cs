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

namespace CPartQCItem
{
    public partial class TFDetail : Form
    {
        public TFDetail()
        {
            InitializeComponent();
        }
        public string g_sUpdateType, g_sformText;
        public bool bAppendSucess = false;
        public string g_sTypeName, g_sTypeID;
        string sSQL;
        DataSet dsTemp;

        private void TFDetail_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.Text = g_sformText;
            combHasValue.SelectedIndex = 0;
            LabTypeName.Text = g_sTypeName;  //測試大項名稱值 LabTypeName
            rbtnNumber.Checked = true;
            sSQL = " SELECT MAX(ITEM_CODE) FROM SAJET.SYS_TEST_ITEM "
                + " WHERE ITEM_TYPE_ID = '" + g_sTypeID + "'"
                + " AND ENABLED = 'Y'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                maxItemCode.Text = dsTemp.Tables[0].Rows[0][0].ToString();
            }
        } 

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (bAppendSucess == false)
                DialogResult = DialogResult.OK;
        }
        //顯示是否有值檢驗
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
                 + " Where item_code = '" + editCode.Text + "' "
                 + " and item_type_id = '" + g_sTypeID + "'";
           // if (g_sUpdateType == "MODIFY") sSQL = sSQL + " and item_id <> '" + g_sKeyID + "'";
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
            //if (g_sUpdateType == "MODIFY") sSQL = sSQL + " and item_id <> '" + g_sKeyID + "'";
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

            try
            {
                if (g_sUpdateType == "ITEM_APPEND")
                {
                    AppendData();
                    bAppendSucess = true;
                    string sMsg = SajetCommon.SetLanguage("Data Append OK", 2) + " !" + Environment.NewLine
                        + SajetCommon.SetLanguage("Append Other Data", 2) + " ?";
                    if (SajetCommon.Show_Message(sMsg, 2) == DialogResult.Yes)
                    {
                        ClearData();
                        editName.Focus();
                        return;
                    }
                    DialogResult = DialogResult.OK;
                }
                //else if (g_sUpdateType == "MODIFY")
                //{
                //    ModifyData();
                //    DialogResult = DialogResult.OK;
                //}
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception : " + ex.Message, 0);
                return;
            }
            
        }

        private void AppendData()
        {
            editCode.Text = editCode.Text.Trim().ToUpper();
            sSQL = "Select SYSDATE from dual ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            DateTime dtNow = (DateTime)dsTemp.Tables[0].Rows[0]["SYSDATE"]; //轉型成日期型別
            string sNow = dtNow.ToString("yyyy/MM/dd HH:mm:ss"); //定義日期格式
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
            object[][] Params = new object[10][];    //--------20110917
            sSQL = " Insert into SAJET.SYS_TEST_ITEM "
                 + " (ITEM_ID,ITEM_NAME,ITEM_DESC,ITEM_DESC2,ITEM_CODE,HAS_VALUE,VALUE_TYPE "
                 + " ,ITEM_TYPE_ID "
                 + " ,ENABLED,UPDATE_USERID,UPDATE_TIME,MIN_INSP_QTY) "
                 + " Values "
                 + " (:ITEM_ID,:ITEM_NAME,:ITEM_DESC,:ITEM_DESC2,:ITEM_CODE,:HAS_VALUE,:VALUE_TYPE "
                 + " ,:ITEM_TYPE_ID "
                 + ",'Y',:UPDATE_USERID,SYSDATE,:MIN_INSP_QTY) ";   //--------20110917
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_ID", sMaxID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_NAME", editName.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_DESC", editDesc.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_DESC2", editDesc2.Text };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_CODE", editCode.Text.ToUpper()};
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "HAS_VALUE", combHasValue.Text };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VALUE_TYPE", sValueType };
            Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", g_sTypeID };
            Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MIN_INSP_QTY", editInspQty.Text }; //-------20110917
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);



            object[][] testItemParams = new object[4][];
            sSQL = " Insert into SAJET.SYS_PART_QC_TESTITEM "
                 + " (RECID,ITEM_ID,UPDATE_USERID,UPDATE_TIME) "
                 + " VALUES (:RECID,:ITEM_ID,:UPDATE_USERID,TO_DATE(:UPDATE_TIME,'yyyy/mm/dd hh24:mi:ss'))";
            testItemParams[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RECID", fMain.g_sRECID };
            testItemParams[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_ID", sMaxID };
            testItemParams[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            testItemParams[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_TIME", sNow };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, testItemParams);

            fMain.CopyToItemDetailHistory(sMaxID);
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