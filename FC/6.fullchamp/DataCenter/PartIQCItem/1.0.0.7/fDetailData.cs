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

namespace CPartIQCItem
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
        public string g_sRECID, g_sProcess, g_sItemTypeCode, g_sItemTypeID;

        private void fData_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            this.Text = g_sformText;
            LabProcessName.Text = g_sProcess;
            LabItemTypeCode.Text = g_sItemTypeCode;

            if (g_sUpdateType == "MODIFY")
            {
                g_sKeyID = dataCurrentRow.Cells[TableDefine.gsDef_DtlKeyField].Value.ToString();

                editName.Text = dataCurrentRow.Cells["ITEM_CODE"].Value.ToString();
                LabItemName.Text = dataCurrentRow.Cells["ITEM_NAME"].Value.ToString();
                editSortIndex.Text = dataCurrentRow.Cells["SORT_INDEX"].Value.ToString();

                KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);
                editName_KeyPress(sender, Key);                

                editUSL.Text = dataCurrentRow.Cells["UPPER_LIMIT"].Value.ToString();
                editUCL.Text = dataCurrentRow.Cells["UPPER_CONTROL_LIMIT"].Value.ToString();
                editCL.Text = dataCurrentRow.Cells["MIDDLE_LIMIT"].Value.ToString();
                editLSL.Text = dataCurrentRow.Cells["LOWER_LIMIT"].Value.ToString();
                editLCL.Text = dataCurrentRow.Cells["LOWER_CONTROL_LIMIT"].Value.ToString();
                editUnit.Text = dataCurrentRow.Cells["UNIT"].Value.ToString();

                editName.Enabled = false;
                btnSearch.Enabled = false;
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
            
            if (!CheckItem(editName.Text, out dsTemp))
                return;
            string sItemID = dsTemp.Tables[0].Rows[0]["ITEM_ID"].ToString();

            //檢查是否重複
            if (g_sUpdateType == "APPEND")
            {
                sSQL = " Select * from " + TableDefine.gsDef_DtlTable
                     + " Where RECID = '" + g_sRECID + "' "
                     + "AND ITEM_ID = '" + sItemID + "' ";
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
            }
            //Update DB
            try
            {
                if (g_sUpdateType == "APPEND")
                {
                    AppendData();
                    bAppendSucess = true;
                    string sMsg = SajetCommon.SetLanguage("Data Append OK", 2) + " !" + Environment.NewLine + SajetCommon.SetLanguage("Append Other Data", 2) + " ?";
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
            string sItemID = GetID("SAJET.SYS_TEST_ITEM", "ITEM_ID", "ITEM_CODE", editName.Text);            
            
            object[][] Params = new object[10][];
            sSQL = " Insert into " + TableDefine.gsDef_DtlTable
                 + " (RECID,ITEM_ID,UPPER_LIMIT,LOWER_LIMIT,MIDDLE_LIMIT "
                 + " ,UPPER_CONTROL_LIMIT,LOWER_CONTROL_LIMIT,UNIT,SORT_INDEX "
                 + " ,ENABLED,UPDATE_USERID,UPDATE_TIME) "
                 + " Values "
                 + " (:RECID,:ITEM_ID,:UPPER_LIMIT,:LOWER_LIMIT,:MIDDLE_LIMIT "
                 + " ,:UPPER_CONTROL_LIMIT,:LOWER_CONTROL_LIMIT,:UNIT,:SORT_INDEX "
                 + ",'Y',:UPDATE_USERID,SYSDATE) ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RECID", g_sRECID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_ID", sItemID };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPPER_LIMIT", editUSL.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOWER_LIMIT", editLSL.Text };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MIDDLE_LIMIT", editCL.Text };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPPER_CONTROL_LIMIT", editUCL.Text };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOWER_CONTROL_LIMIT", editLCL.Text };
            Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UNIT", editUnit.Text };
            Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SORT_INDEX", editSortIndex.Text };
            Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            fMain.CopyToDetailHistory(sItemID, g_sRECID);
        }
        private void ModifyData()
        {            
            object[][] Params = new object[10][];
            sSQL = " Update "+ TableDefine.gsDef_DtlTable
                 + " set UPPER_LIMIT = :UPPER_LIMIT "
                 + "    ,LOWER_LIMIT = :LOWER_LIMIT "
                 + "    ,MIDDLE_LIMIT = :MIDDLE_LIMIT "
                 + "    ,UPPER_CONTROL_LIMIT = :UPPER_CONTROL_LIMIT "
                 + "    ,LOWER_CONTROL_LIMIT = :LOWER_CONTROL_LIMIT "
                 + "    ,UNIT = :UNIT "
                 + "    ,SORT_INDEX = :SORT_INDEX "
                 + "    ,UPDATE_USERID = :UPDATE_USERID "
                 + "    ,UPDATE_TIME = SYSDATE "
                 + " where RECID = :RECID "
                 + " and ITEM_ID = :ITEM_ID ";  
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPPER_LIMIT", editUSL.Text };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOWER_LIMIT", editLSL.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MIDDLE_LIMIT", editCL.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPPER_CONTROL_LIMIT", editUCL.Text };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOWER_CONTROL_LIMIT", editLCL.Text };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UNIT", editUnit.Text };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SORT_INDEX", editSortIndex.Text };
            Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RECID", g_sRECID };
            Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_ID", g_sKeyID };

            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            fMain.CopyToDetailHistory(g_sKeyID, g_sRECID);
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

            if (g_sUpdateType == "APPEND")
                sSQL=sSQL + " AND ITEM_TYPE_ID = '" + g_sItemTypeID + "' ";

            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
                return dsTemp.Tables[0].Rows[0][sFieldID].ToString();
            else
                return "0";
        }

        private bool CheckItem(string sItemCode,out DataSet dsOut)
        {
            sSQL = "Select ITEM_TYPE_ID,ITEM_ID,ITEM_NAME "
                 + " ,haS_value,nvl(value_type,0) value_type "
                 + "from Sajet.SYS_TEST_ITEM "
                 + "Where Item_Code = '" + editName.Text + "' "
                 + " AND ITEM_TYPE_ID = '" + g_sItemTypeID + "' "
                 + " and Enabled = 'Y' ";
            dsOut = ClientUtils.ExecuteSQL(sSQL);
            if (dsOut.Tables[0].Rows.Count == 0)
            {
                SajetCommon.Show_Message("Item Code Error", 0);
                editName.Focus();
                editName.SelectAll();
                return false;
            }
            //不屬於選擇的測試大項
            if (g_sItemTypeID != dsOut.Tables[0].Rows[0]["ITEM_TYPE_ID"].ToString())
            {
                SajetCommon.Show_Message("Item Type not Match", 0);
                editName.Focus();
                editName.SelectAll();
                return false;
            }
            LabItemName.Text = dsOut.Tables[0].Rows[0]["ITEM_NAME"].ToString();
            return true;
        }

        private void editName_KeyPress(object sender, KeyPressEventArgs e)
        {
            LabItemName.Text = "";
            editUSL.Text = "";
            editCL.Text = "";
            editLSL.Text = "";
            editUCL.Text = "";
            editLCL.Text = "";
            editUnit.Text = "";
            editCal.Text = "";

            if (e.KeyChar != (char)Keys.Return)
                return;

            if (!CheckItem(editName.Text, out dsTemp))
                return;
           
            //測試值為數字
            if (dsTemp.Tables[0].Rows[0]["haS_value"].ToString() == "Y" && dsTemp.Tables[0].Rows[0]["value_type"].ToString() == "0")
            {
                LabUSL.Visible = true;
            }
            else //測試值為文字
            {                
                LabUSL.Visible = false;
            }
            LabC.Visible = LabUSL.Visible;
            LabLSL.Visible = LabUSL.Visible;
            LabUCL.Visible = LabUSL.Visible;
            LabLCL.Visible = LabUSL.Visible;
            LabUnit.Visible = LabUSL.Visible;
            editCal.Visible = LabUSL.Visible;
            label5.Visible = LabUSL.Visible;
            editUSL.Visible = LabUSL.Visible;
            editCL.Visible = LabUSL.Visible;
            editLSL.Visible = LabUSL.Visible;
            editUCL.Visible = LabUSL.Visible;
            editLCL.Visible = LabUSL.Visible;
            editUnit.Visible = LabUSL.Visible;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            sSQL = "select a.Item_Code,a.Item_Name,a.has_Value "
                 + "from SAJET.SYS_TEST_ITEM a "
                 + "    ,SAJET.SYS_TEST_ITEM_TYPE b "
                 + "Where b.ITEM_TYPE_CODE = '" + g_sItemTypeCode + "' "
                 + "and a.Item_Code like '" + editName.Text + "%' "
                 + "and a.Item_Type_ID = b.Item_Type_ID "
                 + "and a.enabled = 'Y' "
                 + "Order By a.Item_Code ";
            fFilter f = new fFilter();
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                editName.Text = f.dgvData.CurrentRow.Cells["Item_Code"].Value.ToString();
                KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);
                editName_KeyPress(sender, Key);
            }
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

        private void editCal_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == 8 || e.KeyChar == '.' || e.KeyChar == 13 || e.KeyChar == 46))
            {
                e.KeyChar = (char)Keys.None;
            }
            if (e.KeyChar != (char)Keys.Return)
                return;
            editCal.Text = editCal.Text.Trim();
            if (string.IsNullOrEmpty(editCal.Text))
                return;
            Double dCalValue = 0;
            double dCL = 0;
            try
            {
                dCalValue = Convert.ToDouble(editCal.Text);
            }
            catch
            {
                SajetCommon.Show_Message("(%) Value Error", 0);
                editCal.Focus();
                editCal.SelectAll();
                return;
            }
            if (dCalValue > 100)
            {
                SajetCommon.Show_Message("(%) Value Over 100", 0);
                editCal.Focus();
                editCal.SelectAll();
                return;
            }
            try
            {
                dCL = Convert.ToDouble(editCL.Text);
            }
            catch
            {
                SajetCommon.Show_Message("CL Value Error", 0);
                editCL.Focus();
                editCL.SelectAll();
                return;
            }
            double dUSL = dCL * (100 + dCalValue) / 100;
            double dLSL = dCL * (100 - dCalValue) / 100;
            editUSL.Text = dUSL.ToString();
            editLSL.Text = dLSL.ToString(); ;


        }

        private void editCL_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            editCL.Text = editCL.Text.Trim();
            if (!string.IsNullOrEmpty(editCL.Text))
            {
                editCal.Focus();
                editCal.SelectAll();
            }
        }
    }
}