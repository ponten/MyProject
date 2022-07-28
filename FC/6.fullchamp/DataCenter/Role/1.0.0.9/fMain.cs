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
using System.Data.OracleClient;

namespace CRole
{
    public partial class fMain : Form
    {
        private MESGridView.Cache memoryCache;
        string g_sField;
        public fMain()
        {
            InitializeComponent();
        }

        string sSQL;
        public static String g_sUserID;
        public String g_sProgram, g_sFunction;
        public String g_sOrderField;
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
            string sMsg = "";
            g_sField = SajetCommon.GetSysBaseData("ALL", ClientUtils.fClientLang, ref sMsg);
            TableDefine.Initial_Table();
            Initial_Form();

            //
            combShow.SelectedIndex = 0;
            this.Text = this.Text + "(" + SajetCommon.g_sFileVersion + ")";

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
            sSQL = "Select * from " + TableDefine.gsDef_Table + " ";
            if (combShow.SelectedIndex == 0)
                sSQL = sSQL + " where Enabled = 'Y' ";
            else if (combShow.SelectedIndex == 1)
                sSQL = sSQL + " where Enabled = 'N' ";

            if (combFilter.SelectedIndex > -1 && editFilter.Text.Trim() != "")
            {
                string sFieldName = combFilterField.Items[combFilter.SelectedIndex].ToString();
                if (combShow.SelectedIndex <= 1)
                    sSQL = sSQL + " and ";
                else
                    sSQL = sSQL + " where ";
                sSQL = sSQL + sFieldName + " like '" + editFilter.Text.Trim() + "%'";
            }
            sSQL = sSQL + " order by " + g_sOrderField;
            g_sDataSQL = sSQL;
            (new MESGridView.DisplayGridView()).GetGridView(gvData, sSQL, out memoryCache);

            //欄位Title  
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
                    gvData.Columns[sGridField].DisplayIndex = i; //欄位顯示順序
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
                f.g_sformText = btnModify.Text;
                f.dataCurrentRow = gvData.CurrentRow;
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
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);

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

        private void SetSelectRow(DataGridView GridData, String sPrimaryKey, String sField)
        {
            if (GridData.Rows.Count > 0)
            {
                int iIndex = 0;
                string sShowField = GridData.Columns[0].Name;
                for (int i = 0; i <= GridData.Columns.Count - 1; i++)
                {
                    if (GridData.Columns[i].Visible)
                    {
                        //第一個有顯示的欄位(focus到隱藏欄位會錯誤)
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
                    //改用SQL找,不由Grid讀值,否則速度會慢
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
                Show_Module_Pri();
            }
        }
        public void Show_Module_Pri()
        {
            TreeViewSelect.Nodes.Clear();

            string sFUN_Field = "FUN_ENG";
            string sFUN_DESC_Field = "FUN_DESC_ENG";
            string sProgram_Field = "PROGRAM";
            string sFUN_SQL = "";
            if (!string.IsNullOrEmpty(g_sField))
            {
                sFUN_Field = "FUN_" + g_sField;
                sFUN_DESC_Field = "FUN_DESC_" + g_sField;
                sProgram_Field = "PROGRAM_" + g_sField;
                sFUN_SQL = " ,c." + sProgram_Field + " ,b." + sFUN_Field + ",b." + sFUN_DESC_Field + " ";
            }

            string sPreProgram = "";
            string sPreFunction = "";
            string sSQL = "SELECT PARAM_VALUE FROM SAJET.SYS_BASE_PARAM "
                + "WHERE PROGRAM = 'Data Center' AND PARAM_NAME = 'Role' AND PARAM_TYPE = 'Role' AND ROWNUM = 1";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            object[][] Params = new object[1][]; ;
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ROLE_ID", gvData.CurrentRow.Cells["ROLE_ID"].Value.ToString() };
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                sSQL = dsTemp.Tables[0].Rows[0][0].ToString().Replace("@", sFUN_SQL);
            }
            else
            {
                sSQL = string.Format(@"Select distinct a.PROGRAM,A.AUTHORITYS,a.FUNCTION,b.FUN_ENG,b.FUN_DESC_ENG,A.AUTHORITYS ROLE_AUTHORITYS{0},C.FUN_TYPE_IDX,B.FUN_TYPE_IDX,B.FUN_IDX
                    FROM SAJET.SYS_ROLE_PRIVILEGE a, sajet.sys_program_fun_name b, sajet.sys_program_name c 
                    Where a.Role_id = :ROLE_ID 
                    and a.program = b.program 
                    and a.function = b.function 
                    and a.program = c.program 
                    and b.ENABLED = 'Y' 
                    and c.ENABLED = 'Y'
                    ORDER BY C.FUN_TYPE_IDX, PROGRAM, B.FUN_TYPE_IDX, B.FUN_IDX, FUNCTION, A.AUTHORITYS ", sFUN_SQL);
            }
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                string sProgram = dsTemp.Tables[0].Rows[i]["PROGRAM"].ToString();
                string sFunction = dsTemp.Tables[0].Rows[i]["FUNCTION"].ToString();
                string sAuth = dsTemp.Tables[0].Rows[i]["AUTHORITYS"].ToString();
        
                string sProgramDisplay = dsTemp.Tables[0].Rows[i][sProgram_Field].ToString();
                if (string.IsNullOrEmpty(sProgramDisplay.Trim()))
                    sProgramDisplay = dsTemp.Tables[0].Rows[i]["PROGRAM"].ToString();
                string sFunDisplay = dsTemp.Tables[0].Rows[i][sFUN_Field].ToString();
                if (string.IsNullOrEmpty(sFunDisplay.Trim()))
                    sFunDisplay = dsTemp.Tables[0].Rows[i]["FUNCTION"].ToString();
                string sFunctionDesc = dsTemp.Tables[0].Rows[i][sFUN_DESC_Field].ToString();
                if (string.IsNullOrEmpty(sFunctionDesc.Trim()))
                    sFunctionDesc = dsTemp.Tables[0].Rows[i]["FUN_DESC_ENG"].ToString();
          
                //根節點(PROGRAM)
                if (sPreProgram != sProgram)
                {
                    TreeNode tNode = new TreeNode();
                    tNode.Text = sProgramDisplay;
                    tNode.Name = sProgram;
                    tNode.ImageIndex = 0;
                    tNode.SelectedImageIndex = tNode.ImageIndex;
                    TreeViewSelect.Nodes.Add(tNode);
                    sPreFunction = "";
                }

                //子節點(FUNCTION)
                if (sPreFunction != sFunction)
                {
                    TreeNode tNode = new TreeNode();
                    tNode.Text = sFunDisplay;
                    tNode.Name = sFunction;
                    tNode.ImageIndex = 1;
                    tNode.ToolTipText = sFunctionDesc;
                    tNode.SelectedImageIndex = tNode.ImageIndex;
                    tNode.Tag = dsTemp.Tables[0].Rows[i]["ROLE_AUTHORITYS"].ToString();
                    TreeViewSelect.Nodes[TreeViewSelect.Nodes.Count - 1].Nodes.Add(tNode);
                }

                //子節點(權限)
                TreeNode tNode1 = new TreeNode();
                tNode1.Tag = sAuth;
                tNode1.Text = SajetCommon.SetLanguage(sAuth, 1);
                tNode1.Name = tNode1.Text;
                tNode1.ImageIndex = 2;
                tNode1.SelectedImageIndex = tNode1.ImageIndex;
                TreeViewSelect.Nodes[TreeViewSelect.Nodes.Count - 1].LastNode.Nodes.Add(tNode1);

                sPreProgram = sProgram;
                sPreFunction = sFunction;
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

            //替換欄位名稱
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

            if (e.RowIndex >= 0)
            {
                Show_Module_Pri();
            }


        }

        private void MenuModule_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;

            string sRoleID = gvData.CurrentRow.Cells["ROLE_ID"].Value.ToString();
            string sRoleName = gvData.CurrentRow.Cells["ROLE_NAME"].Value.ToString();
            fModule fM = new fModule();
            fM.g_sField = g_sField;
            fM.g_sRoleName = sRoleName;
            fM.g_sRoleID = sRoleID;
            fM.ShowDialog();
            Show_Module_Pri();
            fM.Dispose();
        }
        private void collapseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeViewSelect.CollapseAll();
        }

        private void expandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeViewSelect.ExpandAll();
        }
    }
}