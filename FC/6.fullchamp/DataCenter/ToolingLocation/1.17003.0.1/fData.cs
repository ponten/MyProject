using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using SajetTable;
using System.Data.OracleClient;

namespace CToolingLocation
{
    public partial class fData : Form
    {
        public fData()
        {
            InitializeComponent();
        }

        public string g_sUserID, g_sUpdateType;
        public string g_sKeyID;
        public DataGridViewRow dataCurrentRow;
        string sSQL;
        DataSet dsTemp;
        bool bAppendSucess = false;
        public string g_sformText;


        private void fData_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            this.Text = g_sformText;
          
            if (g_sUpdateType == "MODIFY")
            {
                g_sKeyID = dataCurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();

                editLocationNo.Text = dataCurrentRow.Cells["LOCATION_NO"].Value.ToString();                
                editLocationDesc.Text = dataCurrentRow.Cells["LOCATION_DESC"].Value.ToString();
               
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

            if (string.IsNullOrEmpty(editLocationNo.Text))
            {
                string sData = labelToolingNo.Text;
                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editLocationNo.Focus();
                editLocationNo.SelectAll();
                return;
            }
           
            //檢查TypeName是否重複
            sSQL = " Select * from " + TableDefine.gsDef_Table + " "
                 + " Where LOCATION_NO = '" + editLocationNo.Text + "' ";
            if (g_sUpdateType == "MODIFY")
                sSQL = sSQL + " and " + TableDefine.gsDef_KeyField + " <> '" + g_sKeyID + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                string sData = labelToolingNo.Text + " : " + editLocationNo.Text;
                string sMsg = SajetCommon.SetLanguage("Data Duplicate", 2)
                            + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg, 0);
                editLocationNo.Focus();
                editLocationNo.SelectAll();
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
                    if (SajetCommon.Show_Message(sMsg, 2) == DialogResult.Yes)
                    {
                        ClearData();

                        editLocationNo.Focus();
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
            string sMaxID = SajetCommon.GetMaxID(TableDefine.gsDef_Table, TableDefine.gsDef_KeyField, 8);

            object[][] Params = new object[4][];
            sSQL = " Insert into SAJET.SYS_TOOLING_LOCATION "
                 + " (LOCATION_ID, LOCATION_NO, LOCATION_DESC, "
                 + "  UPDATE_USERID, UPDATE_TIME )"
               
                 + " Values "
                 + " (:LOCATION_ID, :LOCATION_NO, :LOCATION_DESC, "
                 + "  :UPDATE_USERID, SYSDATE )";
                 

            Params[0] = new object[] { ParameterDirection.Input, OracleType.Number, "LOCATION_ID", sMaxID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOCATION_NO", editLocationNo.Text };            
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOCATION_DESC", editLocationDesc.Text };         
            Params[3] = new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", fMain.g_sUserID };
           
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            fMain.CopyToHistory(sMaxID);
        }

        private void ModifyData()
        {
            object[][] Params = new object[4][];
            sSQL = " Update SAJET.SYS_TOOLING_LOCATION "
                 + " set LOCATION_NO = :LOCATION_NO "
                 + "    ,LOCATION_DESC = :LOCATION_DESC "
                 + "    ,UPDATE_USERID = :UPDATE_USERID "
                 + "    ,UPDATE_TIME = SYSDATE "
                 + " where LOCATION_ID = :LOCATION_ID ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.Number, "LOCATION_ID", g_sKeyID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOCATION_NO", editLocationNo.Text };            
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOCATION_DESC", editLocationDesc.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", fMain.g_sUserID };
          
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            fMain.CopyToHistory(g_sKeyID);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (bAppendSucess)
                DialogResult = DialogResult.OK;
        }

        private string GetID(string sTable, string sID, string sField, string sValue)
        {
            string sResID = "0";
            if (sValue != "")
            {
                sSQL = " select " + sID + " from " + sTable
                     + " where " + sField + " = '" + sValue + "' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows.Count > 0)
                    sResID = dsTemp.Tables[0].Rows[0][sID].ToString();
            }
            return sResID;
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
