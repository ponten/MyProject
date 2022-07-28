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
using System.IO;
namespace IQCbyLot
{
    public partial class fData : Form
    {
        public fData()
        {
            InitializeComponent();
        }

        public string g_sUpdateType, g_sformText, g_sPartNo;
        public string g_sKeyID;
        public DataGridViewRow dataCurrentRow;
        string sSQL;
        DataSet dsTemp;
        bool bAppendSucess = false;

        private void fData_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            this.Text = g_sformText;
            editPartNo.Text = g_sPartNo;

            if (g_sUpdateType == "MODIFY")
            {
                g_sKeyID = dataCurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();
                editNote.Text = dataCurrentRow.Cells["NOTE"].Value.ToString();
                editPartNo.Text = dataCurrentRow.Cells["PART_NO"].Value.ToString();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            editNote.Text = editNote.Text.Trim();
            editPartNo.Text = editPartNo.Text.Trim();

            if (string.IsNullOrEmpty(editPartNo.Text))
            {
                string sMsg = SajetCommon.SetLanguage("Please Input Part No", 0);
                SajetCommon.Show_Message(sMsg, 0);
                editPartNo.Focus();
                editPartNo.SelectAll();
                return;
            }
            if (string.IsNullOrEmpty(editNote.Text))
            {
                string sMsg = SajetCommon.SetLanguage("Please Input Note", 0);
                SajetCommon.Show_Message(sMsg, 0);
                editNote.Focus();
                editNote.SelectAll();
                return;
            }

            try
            {
                string sPartID = SajetCommon.GetID("SAJET.SYS_PART", "PART_ID", "PART_NO", editPartNo.Text);
                if (sPartID == "0")
                {
                    SajetCommon.Show_Message("Part No Error", 0);
                    editPartNo.Focus();
                    editPartNo.SelectAll();
                    return;
                }

                editPartID.Text = sPartID;

                if (g_sUpdateType == "APPEND")
                {
                    AppendData();
                    bAppendSucess = true;
                    string sMsg = SajetCommon.SetLanguage("Data Append OK", 1) + " !" + Environment.NewLine + SajetCommon.SetLanguage("Append Other Data", 1) + " ?";
                    
                    if (SajetCommon.Show_Message(sMsg, 2) == DialogResult.Yes)
                    {
                        editNote.Text = string.Empty;
                        editNote.Focus();
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
            string sMaxID = SajetCommon.GetMaxID("SAJET.G_IQC_NOTES", "RECID", 8);
            object[][] Params = new object[4][];
            sSQL = " INSERT INTO SAJET.G_IQC_NOTES "
                 + " (RECID,PART_ID,NOTE,UPDATE_USERID,UPDATE_TIME ) "
                 + " Values "
                 + " (:RECID,:PART_ID,:NOTE,:UPDATE_USERID,SYSDATE ) ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RECID", sMaxID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", editPartID.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NOTE", editNote.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", ClientUtils.UserPara1 };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            fInspNotes.CopyToHistory(sMaxID);
        }
        
        private void ModifyData()
        {
            object[][] Params = new object[4][];
            sSQL = " Update SAJET.G_IQC_NOTES "
                 + "   SET NOTE =:NOTE "
                 + "    ,PART_ID =:PART_ID "
                 + "    ,UPDATE_USERID = :UPDATE_USERID "
                 + "    ,UPDATE_TIME = SYSDATE "
                 + " where RECID = :RECID ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NOTE", editNote.Text };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", editPartID.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", ClientUtils.UserPara1 };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RECID", g_sKeyID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            fInspNotes.CopyToHistory(g_sKeyID);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (bAppendSucess)
                DialogResult = DialogResult.OK;
        }
        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == 8 || e.KeyChar == '.' || e.KeyChar == 13 || e.KeyChar == 46))
            {
                e.KeyChar = (char)Keys.None;
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

        private void btnDefectFilter_Click(object sender, EventArgs e)
        {
            string sPartNO = editPartNo.Text.Trim() + "%";

            sSQL = " Select A.PART_NO,A.SPEC1,A.SPEC2 "
                 + "  From SAJET.SYS_PART A "
                 + " WHERE A.ENABLED='Y' "
                 + "   AND A.PART_NO LIKE '" + sPartNO + "' "
                 + " ORDER BY A.PART_NO ";
            fFilter f = new fFilter();
            f.sSQL = sSQL;

            if (f.ShowDialog() == DialogResult.OK)
            {
                editPartNo.Text = f.dgvData.CurrentRow.Cells["PART_NO"].Value.ToString();
//                KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);
//                editDefectCode_KeyPress(sender, Key);
            }
        }

        private void fData_Shown(object sender, EventArgs e)
        {          
            if (string.IsNullOrEmpty(editPartNo.Text))
            {
                editPartNo.Focus();
            }
            else
            {
                editNote.Focus();
            }
        }
    }
}