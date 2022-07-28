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

namespace RCProcess
{
    public partial class fData : Form
    {
        public fData()
        {
            InitializeComponent();
        }
        public string g_sUpdateType, g_sformText;
        public string g_sKeyID;
        public DataGridViewRow dataCurrentRow;
        string sSQL;
        DataSet dsTemp;
        bool bAppendSucess = false;//判断是否插入成功
        string g_sProcessId="";

        private void fData_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            LabInEN.Text=SajetCommon.SetLanguage(LabInEN.Text);
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.Text = g_sformText;
            if (g_sUpdateType == "MODIFY")
            {
                g_sKeyID = dataCurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();

                editPrcoess.Text = dataCurrentRow.Cells["PROCESS_NAME"].Value.ToString();
                editInDll.Text = dataCurrentRow.Cells["INDLL"].Value.ToString();
                editInName.Text = dataCurrentRow.Cells["INNAME"].Value.ToString();
                editInCN.Text = dataCurrentRow.Cells["INCN"].Value.ToString();
                editOutDll.Text = dataCurrentRow.Cells["OUTDLL"].Value.ToString();
                editOutName.Text = dataCurrentRow.Cells["OUTNAME"].Value.ToString();
                editOutCN.Text = dataCurrentRow.Cells["OUTCN"].Value.ToString();
                g_sProcessId = dataCurrentRow.Cells["PROCESS_ID"].Value.ToString();
                editPrcoess.Enabled = false;
                btnProcess.Visible = false;
            }
            else
            {
                editPrcoess.Enabled = true;
                btnProcess.Visible = true;
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


            //检测是否存在制程
            sSQL = " SELECT * FROM SAJET.SYS_PROCESS "
                  + " WHERE PROCESS_ID = '" + g_sProcessId + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                string sMsg = SajetCommon.SetLanguage("Process Not Exist", 2);
                SajetCommon.Show_Message(sMsg, 0);
                editPrcoess.Focus();
                editPrcoess.SelectAll();
                return;
            }

            if (editOutDll.Text == "")
            {
                string sData = LabInput.Text;
                string sMsg = SajetCommon.SetLanguage("Output Dll is null", 2);
                SajetCommon.Show_Message(sMsg, 0);
                editInDll.Focus();
                editInDll.SelectAll();
                return;
            }

            if (editOutName.Text == "")
            {
                string sMsg = SajetCommon.SetLanguage("Output Dll Name is null", 2);
                SajetCommon.Show_Message(sMsg, 0);
                editInName.Focus();
                editInName.SelectAll();
                return;
            }

            //检测是否重复
            if (g_sUpdateType == "APPEND")
            {
                sSQL = " SELECT * FROM SAJET.SYS_RC_PROCESS_SHEET "
                  + " WHERE PROCESS_ID = '" + g_sProcessId + "' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    string sMsg = SajetCommon.SetLanguage("Data Duplicate", 2);
                    SajetCommon.Show_Message(sMsg, 0);
                    editInDll.Focus();
                    editInDll.SelectAll();
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
                        editPrcoess.Focus();
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
            object[][] Params = new object[1][];
            //sSQL = " SELECT * FROM SAJET.SYS_RC_SHEET WHERE DLL_FILENAME=:DLL_FILENAME ";
            //Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DLL_FILENAME", editInDll.Text };
            //dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            //if (dsTemp.Tables[0].Rows.Count > 0)
            //{
            //    Params = new object[5][];
            //    sSQL = " UPDATE SAJET.SYS_RC_SHEET "
            //         + " SET SHEET_CN=:SHEET_CN,UPDATE_USERID=:UPDATE_USERID,UPDATE_TIME=:UPDATE_TIME,SHEET_NAME=:SHEET_NAME "
            //         + " WHERE DLL_FILENAME=:DLL_FILENAME ";
            //    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DLL_FILENAME", editInDll.Text };
            //    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_CN", editInCN.Text.Trim() };
            //    Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            //    Params[3] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", System.DateTime.Now };
            //    Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_NAME", editInName.Text.Trim() };
            //    dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            //}
            //else
            //{
            //    Params = new object[6][];
            //    sSQL = " INSERT INTO SAJET.SYS_RC_SHEET "
            //         + " (SHEET_NAME,DLL_FILENAME,FORM_NAME,SHEET_TW,SHEET_CN,UPDATE_USERID) "
            //         + " Values "
            //         + " (:SHEET_NAME,:DLL_FILENAME,:FORM_NAME,:SHEET_TW,:SHEET_CN,:UPDATE_USERID) ";
            //    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_NAME", editInName.Text.Trim() };
            //    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DLL_FILENAME", editInDll.Text.Trim() };
            //    Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FORM_NAME", "fMain" };
            //    Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_TW", "" };
            //    Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_CN", editInCN.Text.Trim() };
            //    Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            //    dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            //}

            //if (editOutDll.Text.Trim() != "")
            //{
            //    Params = new object[1][];
            //    sSQL = " SELECT * FROM SAJET.SYS_RC_SHEET WHERE DLL_FILENAME=:DLL_FILENAME ";
            //    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DLL_FILENAME", editOutDll.Text };
            //    dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            //    if (dsTemp.Tables[0].Rows.Count > 0)
            //    {
            //        Params = new object[5][];
            //        sSQL = " UPDATE SAJET.SYS_RC_SHEET "
            //         + " SET SHEET_CN=:SHEET_CN,UPDATE_USERID=:UPDATE_USERID,UPDATE_TIME=:UPDATE_TIME,SHEET_NAME=:SHEET_NAME "
            //         + " WHERE DLL_FILENAME=:DLL_FILENAME ";
            //        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DLL_FILENAME", editOutDll.Text };
            //        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_CN", editOutCN.Text.Trim() };
            //        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            //        Params[3] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", System.DateTime.Now };
            //        Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_NAME", editOutName.Text.Trim() };
            //        dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            //    }
            //    else
            //    {
            //        Params = new object[6][];
            //        sSQL = " INSERT INTO SAJET.SYS_RC_SHEET "
            //             + " (SHEET_NAME,DLL_FILENAME,FORM_NAME,SHEET_TW,SHEET_CN,UPDATE_USERID) "
            //             + " Values "
            //             + " (:SHEET_NAME,:DLL_FILENAME,:FORM_NAME,:SHEET_TW,:SHEET_CN,:UPDATE_USERID) ";
            //        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_NAME", editOutName.Text };
            //        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DLL_FILENAME", editOutDll.Text };
            //        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FORM_NAME", "fMain" };
            //        Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_TW", "" };
            //        Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_CN", editOutCN.Text.Trim() };
            //        Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            //        dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            //    }
            //}

            //Params = new object[7][];
            //sSQL = " INSERT INTO SAJET.SYS_RC_PROCESS_SHEET "
            //     + " (PROCESS_ID,SHEET_SEQ,SHEET_NAME,SHEET_PHASE,NEXT_SHEET,LINK_NAME,UPDATE_USERID) "
            //     + " Values "
            //     + " (:PROCESS_ID,:SHEET_SEQ,:SHEET_NAME,:SHEET_PHASE,:NEXT_SHEET,:LINK_NAME,:UPDATE_USERID) ";
            //Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", g_sProcessId };
            //Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_SEQ", "0" };
            //Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_NAME", editInName.Text };
            //Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_PHASE", "I" };
            //if (editOutDll.Text.Trim() != "")
            //    Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NEXT_SHEET", editOutName.Text.Trim() };
            //else
            //    Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NEXT_SHEET", "END" };
            //Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LINK_NAME", "NEXT" };
            //Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            //dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            //if (editOutDll.Text.Trim() != "")
            //{
            //    Params = new object[7][];
            //    sSQL = " INSERT INTO SAJET.SYS_RC_PROCESS_SHEET "
            //         + " (PROCESS_ID,SHEET_SEQ,SHEET_NAME,SHEET_PHASE,NEXT_SHEET,LINK_NAME,UPDATE_USERID) "
            //         + " Values "
            //         + " (:PROCESS_ID,:SHEET_SEQ,:SHEET_NAME,:SHEET_PHASE,:NEXT_SHEET,:LINK_NAME,:UPDATE_USERID) ";
            //    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", g_sProcessId };
            //    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_SEQ", "1" };
            //    Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_NAME", editOutName.Text };
            //    Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_PHASE", "O" };
            //    Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NEXT_SHEET", "END" };
            //    Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LINK_NAME", "NEXT" };
            //    Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            //    dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            //}

            if (editInDll.Text.Trim() != "")
            {
                Params = new object[7][];
                sSQL = " INSERT INTO SAJET.SYS_RC_PROCESS_SHEET "
                     + " (PROCESS_ID,SHEET_SEQ,SHEET_NAME,SHEET_PHASE,NEXT_SHEET,LINK_NAME,UPDATE_USERID) "
                     + " Values "
                     + " (:PROCESS_ID,:SHEET_SEQ,:SHEET_NAME,:SHEET_PHASE,:NEXT_SHEET,:LINK_NAME,:UPDATE_USERID) ";
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", g_sProcessId };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_SEQ", "0" };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_NAME", editInName.Text };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_PHASE", "I" };
                Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NEXT_SHEET", editOutName.Text.Trim() };
                Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LINK_NAME", "NEXT" };
                Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            }

            Params = new object[7][];
            sSQL = " INSERT INTO SAJET.SYS_RC_PROCESS_SHEET "
                 + " (PROCESS_ID,SHEET_SEQ,SHEET_NAME,SHEET_PHASE,NEXT_SHEET,LINK_NAME,UPDATE_USERID) "
                 + " Values "
                 + " (:PROCESS_ID,:SHEET_SEQ,:SHEET_NAME,:SHEET_PHASE,:NEXT_SHEET,:LINK_NAME,:UPDATE_USERID) ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", g_sProcessId };
            if (editInDll.Text.Trim() != "")
            {
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_SEQ", "1" };
            }
            else
            {
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_SEQ", "0" };
            }
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_NAME", editOutName.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_PHASE", "O" };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NEXT_SHEET", "END" };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LINK_NAME", "NEXT" };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", fMain.g_sUserID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
        }

        private void ModifyData()
        {
            object[][] Params = new object[1][];
            sSQL = " DELETE FROM SAJET.SYS_RC_PROCESS_SHEET WHERE PROCESS_ID=:PROCESS_ID ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID",g_sProcessId };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            AppendData();
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

        private void btnProcess_Click(object sender, EventArgs e)
        {
            //Begin--Modify by Dave.He 2015-11-16 for 防止选择已有的Process ID
            //sSQL = " SELECT PROCESS_NAME,PROCESS_ID FROM SAJET.SYS_PROCESS WHERE ENABLED='Y'";//原有Source
            sSQL = "SELECT PROCESS_NAME,PROCESS_ID FROM SAJET.SYS_PROCESS WHERE ENABLED='Y'"
                + "AND PROCESS_ID NOT IN (SELECT PROCESS_ID FROM SAJET.SYS_RC_PROCESS_SHEET ) ";
            //End--Modify by Dave.He 2015-11-16 for 防止选择已有的Process ID
            if (!string.IsNullOrEmpty(editPrcoess.Text.Trim()))
            {
                sSQL = sSQL + " and PROCESS_NAME LIKE '%" + editPrcoess.Text.Trim() + "%' ";
            }
            sSQL = sSQL + "  ORDER BY PROCESS_ID ";
            fFilter f = new fFilter();
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                editPrcoess.Text = f.dgvData.CurrentRow.Cells["PROCESS_NAME"].Value.ToString();
                g_sProcessId = f.dgvData.CurrentRow.Cells["PROCESS_ID"].Value.ToString();
                KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);
            } 
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.InitialDirectory = Application.StartupPath;
            //openFileDialog.Filter = "file|*.dll";
            //if (openFileDialog.ShowDialog() == DialogResult.OK)
            //{
            //    editInDll.Text = System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName);
            //}

            sSQL = " SELECT DLL_FILENAME FROM SAJET.SYS_RC_SHEET WHERE DLL_FILENAME = 'RCInput' ORDER BY SHEET_NAME ";

            fFilter f = new fFilter();
            f.sSQL = sSQL;

            if (f.ShowDialog() == DialogResult.OK)
            {
                editInDll.Text = f.dgvData.CurrentRow.Cells["DLL_FILENAME"].Value.ToString();
                //KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);
            } 
        }

        private void btnOut_Click(object sender, EventArgs e)
        {
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.InitialDirectory = Application.StartupPath;
            //openFileDialog.Filter = "file|*.dll";
            //if (openFileDialog.ShowDialog() == DialogResult.OK)
            //{
            //    editOutDll.Text = System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName);
            //}

            sSQL = " SELECT DLL_FILENAME FROM SAJET.SYS_RC_SHEET WHERE DLL_FILENAME <> 'RCInput' ORDER BY SHEET_NAME ";

            fFilter f = new fFilter();
            f.sSQL = sSQL;

            if (f.ShowDialog() == DialogResult.OK)
            {
                editOutDll.Text = f.dgvData.CurrentRow.Cells["DLL_FILENAME"].Value.ToString();
                //KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);
            } 
        }

        private void editInDll_TextChanged(object sender, EventArgs e)
        {
            object[][] Params = new object[1][];
            sSQL = " SELECT SHEET_NAME,SHEET_CN FROM SAJET.SYS_RC_SHEET WHERE DLL_FILENAME=:DLL_FILENAME ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DLL_FILENAME", editInDll.Text.Trim() };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                editInName.Text = dsTemp.Tables[0].Rows[0]["SHEET_NAME"].ToString();
                editInCN.Text = dsTemp.Tables[0].Rows[0]["SHEET_CN"].ToString();
            }
            else
            {
                editInName.Text = "";
                editInCN.Text = "";
            }       
        }

        private void editInDll_KeyPress(object sender, KeyPressEventArgs e)
        {
            editInDll_TextChanged(null,null);
        }

        private void editOutDll_TextChanged(object sender, EventArgs e)
        {
            object[][] Params = new object[1][];
            sSQL = " SELECT SHEET_NAME,SHEET_CN FROM SAJET.SYS_RC_SHEET WHERE DLL_FILENAME=:DLL_FILENAME ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DLL_FILENAME", editOutDll.Text.Trim() };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                editOutName.Text = dsTemp.Tables[0].Rows[0]["SHEET_NAME"].ToString();
                editOutCN.Text = dsTemp.Tables[0].Rows[0]["SHEET_CN"].ToString();
            }
            else
            {
                editOutName.Text = "";
                editOutCN.Text = "";
            }  
        }

        private void editOutName_KeyPress(object sender, KeyPressEventArgs e)
        {
            editOutDll_TextChanged(null,null);
        } 
    }
}