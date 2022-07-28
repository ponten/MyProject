using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using SajetTable;
using System.IO;
using System.Collections.Specialized;

namespace CEmp
{
    public partial class fMain : Form
    {
        private MESGridView.Cache memoryCache;

        public fMain()
        {
            InitializeComponent();
        }

        string sSQL;
        public static string g_sUserID;
        public string g_sProgram, g_sFunction;
        string g_sFactoryID;
        string g_sOrderField;
        StringCollection ListFactoryID = new StringCollection();
        string g_sDataSQL;

        private void Initial_Form()
        {
            g_sUserID = ClientUtils.UserPara1;
            g_sProgram = ClientUtils.fProgramName;
            g_sFunction = ClientUtils.fFunctionName;
            g_sOrderField = TableDefine.gsDef_OrderField;

            SajetCommon.SetLanguageControl(this);
            panel1.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
        }
        private void fMain_Load(object sender, EventArgs e)
        {
            TableDefine.Initial_Table();
            Initial_Form();
            this.Text = this.Text + "(" + SajetCommon.g_sFileVersion + ")";

            //Factory           
            combFactory.Items.Add("N/A");
            ListFactoryID.Add("0");
            sSQL = " Select FACTORY_ID,FACTORY_CODE,FACTORY_NAME "
                 + " From SAJET.SYS_FACTORY "
                 + " Where ENABLED = 'Y' "
                 + " Order By FACTORY_NAME ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                combFactory.Items.Add(dsTemp.Tables[0].Rows[i]["FACTORY_NAME"].ToString());
                ListFactoryID.Add(dsTemp.Tables[0].Rows[i]["FACTORY_ID"].ToString());
            }
            sSQL = " Select NVL(FACTORY_ID,0) FACTORY_ID "
                 + " From SAJET.SYS_EMP "
                 + " Where EMP_ID = '" + g_sUserID + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            string sUserFactory = dsTemp.Tables[0].Rows[0]["FACTORY_ID"].ToString();
            if (sUserFactory != "0")
            {
                int iIndex = ListFactoryID.IndexOf(sUserFactory);
                combFactory.SelectedIndex = iIndex;
                combFactory.Enabled = false;
            }
            else
            {
                combFactory.SelectedIndex = 0;
            }

            //
            combShow.SelectedIndex = 0;

            //Filter
            combFilter.Items.Clear();
            combFilterField.Items.Clear();
            for (int i = 0; i <= TableDefine.tGridField.Length - 1; i++)
            {
                combFilter.Items.Add(TableDefine.tGridField[i].sCaption);
                combFilterField.Items.Add(TableDefine.tGridField[i].sFieldName);
            }
            if (combFilter.Items.Count > 0)
                combFilter.SelectedIndex = 0;

            Check_Privilege();
        }
        private void Check_Privilege()
        {
            string sPrivilege = ClientUtils.GetPrivilege(g_sUserID, g_sFunction, g_sProgram).ToString();

            btnAppend.Enabled = SajetCommon.CheckEnabled("INSERT", sPrivilege);
            btnModify.Enabled = SajetCommon.CheckEnabled("UPDATE", sPrivilege);
            btnEnabled.Enabled = SajetCommon.CheckEnabled("ENABLED", sPrivilege);
            btnDisabled.Enabled = SajetCommon.CheckEnabled("DISABLED", sPrivilege);
            btnDelete.Enabled = SajetCommon.CheckEnabled("DELETE", sPrivilege);
            btnRole.Enabled = SajetCommon.CheckEnabled("ROLE", sPrivilege);
            btnProcess.Enabled = SajetCommon.CheckEnabled("PROCESS", sPrivilege);
            MenuResetPwd.Enabled = btnDelete.Enabled;

            int iRoleAssingPri = ClientUtils.GetPrivilege(g_sUserID, "Role Assign", g_sProgram);
            MenuRole.Enabled = (iRoleAssingPri >= 1);
            btnRole.Enabled = MenuRole.Enabled;
        }

        private void combShow_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDelete.Visible = (combShow.SelectedIndex == 1);
            btnDisabled.Visible = (combShow.SelectedIndex == 0);
            btnEnabled.Visible = (combShow.SelectedIndex == 1);

            ShowData();
            SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);
        }

        public void ShowData()
        {
            sSQL = " Select a.* ,c.dept_name,d.shift_name,e.pdline_name "
                 + " from sajet.sys_emp a "
                 + ",sajet.sys_dept c "
                 + ",sajet.sys_shift d "
                 + ",sajet.sys_pdline e "
                 + " where a.dept_id = c.dept_id(+) "
                 + " and a.shift = d.shift_id(+) "
                 + " and a.pdline_id = e.pdline_id(+) "
                 + " and a.factory_id = '" + g_sFactoryID + "' ";
            if (combShow.SelectedIndex == 0)
                sSQL += " and a.Enabled = 'Y' ";
            else if (combShow.SelectedIndex == 1)
                sSQL += " and a.Enabled = 'N' ";

            if (combFilter.SelectedIndex > -1 && editFilter.Text.Trim() != "")
            {
                string sFieldName = combFilterField.Items[combFilter.SelectedIndex].ToString();
                sSQL = sSQL + " and " + sFieldName + " like '" + editFilter.Text.Trim() + "%'";
            }
            sSQL = sSQL + " order by " + g_sOrderField;
            g_sDataSQL = sSQL;
            (new MESGridView.DisplayGridView()).GetGridView(gvData, sSQL, out memoryCache);


            //���Title  
            for (int i = 0; i <= gvData.Columns.Count - 1; i++)
            {
                gvData.Columns[i].Visible = false;
            }
            for (int i = 0; i <= TableDefine.tGridField.Length - 1; i++)
            {
                string sGridField = TableDefine.tGridField[i].sFieldName;

                if (gvData.Columns.Contains(sGridField))
                {
                    gvData.Columns[sGridField].HeaderText = TableDefine.tGridField[i].sCaption;
                    gvData.Columns[sGridField].DisplayIndex = i; //�����ܶ���
                    gvData.Columns[sGridField].Visible = true;
                }
            }
            gvData.Focus();
        }

        private void btnDisable_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;

            string sID = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();
            string sType = "";
            string sEnabled = "";
            if (sender == btnDisabled)
            {
                sType = btnDisabled.Text;
                sEnabled = "N";
            }
            else if (sender == btnEnabled)
            {
                sType = btnEnabled.Text;
                sEnabled = "Y";
            }

            string sData = gvData.Columns[TableDefine.gsDef_KeyData].HeaderText + " : " + gvData.CurrentRow.Cells[TableDefine.gsDef_KeyData].Value.ToString();
            string sMsg = sType + " ?" + Environment.NewLine + sData;

            if (SajetCommon.Show_Message(sMsg, 2) != DialogResult.Yes)
                return;

            sSQL = " Update " + TableDefine.gsDef_Table + " "
                 + " set Enabled = '" + sEnabled + "'  "
                 + "    ,UPDATE_USERID = '" + g_sUserID + "'  "
                 + "    ,UPDATE_TIME = SYSDATE  "
                 + " where " + TableDefine.gsDef_KeyField + " = '" + sID + "'";
            ClientUtils.ExecuteSQL(sSQL);
            CopyToHistory(sID);

            ShowData();
            SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);
        }

        private void editFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            ShowData();
            SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);

            editFilter.Focus();
        }

        private void btnAppend_Click(object sender, EventArgs e)
        {
            fData f = new fData(this);
            try
            {
                f.g_sUpdateType = "APPEND";
                f.g_sFactoryID = g_sFactoryID;
                f.g_sFactoryName = combFactory.Text;
                f.g_sformText = btnAppend.Text;
                if (f.ShowDialog() == DialogResult.OK)
                {
                    ShowData();
                    SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);
                }
            }
            finally
            {
                f.Dispose();
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;
            fData f = new fData();
            try
            {
                f.g_sUpdateType = "MODIFY";
                f.g_sFactoryID = g_sFactoryID;
                f.g_sFactoryName = combFactory.Text;
                f.dataCurrentRow = gvData.CurrentRow;
                f.g_sformText = btnModify.Text;
                string sSelectKeyValue = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();
                if (f.ShowDialog() == DialogResult.OK)
                {
                    ShowData();
                    SetSelectRow(gvData, sSelectKeyValue, TableDefine.gsDef_KeyField);
                }
            }
            finally
            {
                f.Dispose();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;

            string sID = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();

            string sData = gvData.Columns[TableDefine.gsDef_KeyData].HeaderText + " : " + gvData.CurrentRow.Cells[TableDefine.gsDef_KeyData].Value.ToString();
            string sMsg = btnDelete.Text + " ?" + Environment.NewLine + sData;
            if (SajetCommon.Show_Message(sMsg, 2) != DialogResult.Yes)
                return;

            sSQL = " Update " + TableDefine.gsDef_Table + " "
                 + " set Enabled = 'Drop'  "
                 + "    ,UPDATE_USERID = '" + g_sUserID + "'  "
                 + "    ,UPDATE_TIME = SYSDATE  "
                 + " where " + TableDefine.gsDef_KeyField + " = '" + sID + "'";
            ClientUtils.ExecuteSQL(sSQL);
            CopyToHistory(sID);
            sSQL = " Delete " + TableDefine.gsDef_Table + " "
                 + " where " + TableDefine.gsDef_KeyField + " = '" + sID + "'";
            ClientUtils.ExecuteSQL(sSQL);

            ShowData();
            SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);
        }

        public static void CopyToHistory(string sID)
        {
            string sSQL = " Insert into " + TableDefine.gsDef_HTTable + " "
                        + " Select * from " + TableDefine.gsDef_Table + " "
                        + " where " + TableDefine.gsDef_KeyField + " = '" + sID + "' ";
            ClientUtils.ExecuteSQL(sSQL);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            saveFileDialog1.DefaultExt = "xls";
            saveFileDialog1.Filter = "All Files(*.xls)|*.xls";
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            string sFileName = saveFileDialog1.FileName;

            ExportExcel.CreateExcel Export = new ExportExcel.CreateExcel(sFileName);
            Export.ExportToExcel(gvData);
        }

        private void SetSelectRow(DataGridView GridData, string sPrimaryKey, string sField)
        {
            if (GridData.Rows.Count > 0)
            {
                int iIndex = 0;
                string sShowField = GridData.Columns[0].Name;
                for (int i = 0; i <= GridData.Columns.Count - 1; i++)
                {
                    if (GridData.Columns[i].Visible)
                    {
                        //�Ĥ@�Ӧ���ܪ����(focus���������|���~)
                        sShowField = GridData.Columns[i].Name;
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(sPrimaryKey))
                {
                    string sCondition = "";
                    string[] tsField = sField.Split(',');
                    string[] tsValue = sPrimaryKey.Split(',');
                    for (int j = 0; j <= tsField.Length - 1; j++)
                    {
                        if (j == 0)
                            sCondition = " Where " + tsField[j].ToString() + "='" + tsValue[j].ToString() + "' ";
                        else
                            sCondition = sCondition + " and " + tsField[j].ToString() + "='" + tsValue[j].ToString() + "' ";

                    }
                    //���SQL��,����GridŪ��,�_�h�t�׷|�C
                    string sText = "select idx from ("
                                 + " Select aa.*,rownum-1 idx from ("
                                 + g_sDataSQL
                                 + " ) aa ) "
                                 + sCondition;
                    DataSet ds = ClientUtils.ExecuteSQL(sText);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        iIndex = Convert.ToInt32(ds.Tables[0].Rows[0]["idx"].ToString());
                    }
                }
                GridData.Focus();
                GridData.CurrentCell = GridData.Rows[iIndex].Cells[sShowField];
                GridData.Rows[iIndex].Selected = true;
            }
        }

        private void gvData_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            e.Value = memoryCache.RetrieveElement(e.RowIndex, e.ColumnIndex);
        }

        private void MenuHistory_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;

            string sFieldID = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();
            string sSQL = TableDefine.History_SQL(sFieldID);
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);

            fHistory fHistory = new fHistory();
            fHistory.dgvHistory.DataSource = dsTemp;
            fHistory.dgvHistory.DataMember = dsTemp.Tables[0].ToString();

            //�������W��
            for (int i = 0; i <= fHistory.dgvHistory.Columns.Count - 1; i++)
            {
                string sGridField = fHistory.dgvHistory.Columns[i].HeaderText;
                string sField = "";
                for (int j = 0; j <= gvData.Columns.Count - 1; j++)
                {
                    sField = gvData.Columns[j].Name;
                    if (sGridField == sField)
                    {
                        fHistory.dgvHistory.Columns[i].HeaderText = gvData.Columns[j].HeaderText;
                        break;
                    }
                }
            }

            fHistory.ShowDialog();
            fHistory.Dispose();
        }

        private void gvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex > -1)
            {
                g_sOrderField = gvData.Columns[e.ColumnIndex].Name;
                ShowData();
                SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);
            }
        }

        private void combFactory_SelectedIndexChanged(object sender, EventArgs e)
        {
            g_sFactoryID = ListFactoryID[combFactory.SelectedIndex].ToString();
            ShowData();
        }

        private void MenuResetPwd_Click(object sender, EventArgs e)
        {
            if (gvData.RowCount == 0 || gvData.CurrentRow == null)
                return;
            try
            {
                string sEmpID = gvData.CurrentRow.Cells["EMP_ID"].Value.ToString();
                string sEmpNo = gvData.CurrentRow.Cells["EMP_NO"].Value.ToString();
                string sEmpField = gvData.Columns[TableDefine.gsDef_KeyData].HeaderText;
                string sTxt = SajetCommon.SetLanguage("Reset Password", 1);
                if (SajetCommon.Show_Message(sTxt + " ?" + Environment.NewLine + sEmpField + " : " + sEmpNo, 2) != DialogResult.Yes)
                    return;

                sSQL = "Update Sajet.SYS_EMP "
                     + "Set PASSWD = SAJET.password.encrypt('" + sEmpNo + "') "
                     + "   ,UPDATE_USERID = '" + g_sUserID + "' "
                     + "   ,UPDATE_TIME = SYSDATE "
                     + "Where EMP_ID = '" + sEmpID + "' ";
                DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
                CopyToHistory(sEmpID);
                SajetCommon.Show_Message("Reset Password OK", -1);
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception:" + ex.Message, 0);
            }

        }

        private void MenuRole_Click(object sender, EventArgs e)
        {
            if (gvData.RowCount == 0 || gvData.CurrentRow == null)
                return;

            string sEmpID = gvData.CurrentRow.Cells["EMP_ID"].Value.ToString();
            string sEmpName = gvData.CurrentRow.Cells["EMP_NAME"].Value.ToString();

            fRoleAssign fRole = new fRoleAssign
            {
                g_sEmpName = sEmpName,
                g_sEmpID = sEmpID
            };
            fRole.ShowDialog();
        }

        private void BtnProcess_Click(object sender, EventArgs e)
        {
            if (gvData.RowCount == 0 || gvData.CurrentRow == null)
                return;
            string sEmpID = gvData.CurrentRow.Cells["EMP_ID"].Value.ToString();

            fProcess fProcess = new fProcess
            {
                sEMP = sEmpID,
                sUser = g_sUserID
            };
            fProcess.ShowDialog();
        }

        private void gvData_Click(object sender, EventArgs e)
        {
            gvDetaildata.DataSource = null;

            if (gvData.RowCount == 0 || gvData.CurrentRow == null)
                return;

            string sEmpID = gvData.CurrentRow.Cells["EMP_ID"].Value.ToString();
            string sSQL1 = " select b.role_name from sajet.sys_role_emp a, sajet.sys_role b "
                         + " where a.role_id = b.role_id and a.EMP_ID = '" + sEmpID + "' ";
            DataSet ds = ClientUtils.ExecuteSQL(sSQL1);
            gvDetaildata.DataSource = ds;
            gvDetaildata.DataMember = ds.Tables[0].ToString();

            //for (int i = 0; i < gvDetaildata.Columns.Count; i++)
            //{
            //    gvDetaildata.Columns[i].Visible = false;
            //}
            for (int i = 0; i <= TableDefine.tGridField_2.Length - 1; i++)
            {
                string sGridField = TableDefine.tGridField_2[i].sFieldName;

                if (gvDetaildata.Columns.Contains(sGridField))
                {
                    gvDetaildata.Columns[sGridField].HeaderText = TableDefine.tGridField_2[i].sCaption;
                    gvDetaildata.Columns[sGridField].DisplayIndex = i; //�����ܶ���
                    gvDetaildata.Columns[sGridField].Visible = true;
                }
            }
        }
    }
}