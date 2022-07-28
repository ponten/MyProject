using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using SajetTable;

namespace RCProcess
{
    public partial class fMain : Form
    {
        private MESGridView.Cache memoryCache;

        public fMain()
        {
            InitializeComponent();
        }

        string sSQL;
        public static String g_sUserID;
        public String g_sProgram, g_sFunction; 
        public String g_sOrderField;
        public static int g_iPlanPrivilege;
        string g_sDataSQL;
        bool g_bFilter;

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

            g_bFilter = false;
            sSQL = "SELECT COUNT(*) CNT FROM SAJET.SYS_RC_PROCESS_SHEET A,SAJET.SYS_PROCESS B WHERE A.PROCESS_ID=B.PROCESS_ID AND A.SHEET_PHASE='I' ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (Convert.ToInt32(dsTemp.Tables[0].Rows[0]["cnt"].ToString()) > 5000)
            {
                g_bFilter = true;
            }
            

            combShow.SelectedIndex = 0;
        }

        private void Check_Privilege()
        {
            string sPrivilege = ClientUtils.GetPrivilege(g_sUserID, g_sFunction, g_sProgram).ToString();

            btnAppend.Enabled = SajetCommon.CheckEnabled("INSERT", sPrivilege);
            btnModify.Enabled = SajetCommon.CheckEnabled("UPDATE", sPrivilege);

            //тSampling Plan Default舦(Τ舦砞﹚Sampling Plan)
            g_iPlanPrivilege = ClientUtils.GetPrivilege(g_sUserID, "Sampling Plan Default",g_sProgram );
        }

        private void combShow_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowData();
            SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);
        }

        public void ShowData()
        {
            if (g_bFilter && string.IsNullOrEmpty(editFilter.Text))
                return;

            //sSQL = "SELECT T3.PROCESS_NAME,T3.INNAME,T3.INDLL,T3.INCN,T3.OUTNAME,T4.DLL_FILENAME AS OUTDLL,T4.SHEET_CN AS OUTCN,T3.PROCESS_ID FROM   "
            //      + "(  "
            //          + "SELECT T1.*,T2.DLL_FILENAME AS INDLL,T2.SHEET_CN AS INCN FROM "
            //            + "( "
            //             + "SELECT C.PROCESS_NAME,A.SHEET_NAME AS INNAME,A.NEXT_SHEET AS OUTNAME,A.PROCESS_ID,C.ENABLED "
            //             + "FROM SAJET.SYS_RC_PROCESS_SHEET A,SAJET.SYS_RC_SHEET B,SAJET.SYS_PROCESS C "
            //             + "WHERE A.SHEET_NAME=B.SHEET_NAME AND A.PROCESS_ID=C.PROCESS_ID AND A.SHEET_PHASE='I' "
            //             + ") T1,(SELECT * FROM SAJET.SYS_RC_SHEET) T2  "
            //             + "WHERE T1.INNAME=T2.SHEET_NAME "
            //      + ") T3,(SELECT * FROM SAJET.SYS_RC_SHEET) T4 WHERE T3.OUTNAME =T4.SHEET_NAME(+)  ";

            //if (combShow.SelectedIndex == 0)
            //    sSQL = sSQL + " and T3.Enabled = 'Y' ";
            //else if (combShow.SelectedIndex == 1)
            //    sSQL = sSQL + " and T3.Enabled = 'N' ";

            //if (combFilter.SelectedIndex > -1 && editFilter.Text.Trim() != "")
            //{
            //    string sFieldName = combFilterField.Items[combFilter.SelectedIndex].ToString();
            //    if (combShow.SelectedIndex <= 1)
            //        sSQL = sSQL + " and ";
            //    else
            //        sSQL = sSQL + " where ";
            //    sSQL = sSQL + sFieldName + " like '" + editFilter.Text.Trim() + "%'";
            //}
            //sSQL = sSQL + " order by " + g_sOrderField;

            sSQL = " SELECT C.PROCESS_NAME,D.SHEET_NAME AS INNAME,D.DLL_FILENAME AS INDLL,D.SHEET_CN AS INCN, "
                 + "        B.SHEET_NAME AS OUTNAME,B.DLL_FILENAME AS OUTDLL,B.SHEET_CN AS OUTCN,A.PROCESS_ID "
                 + "   FROM SAJET.SYS_RC_PROCESS_SHEET A,SAJET.SYS_RC_SHEET B,SAJET.SYS_PROCESS C, "
                 + "        (SELECT B.SHEET_NAME,B.DLL_FILENAME,B.SHEET_CN,A.NEXT_SHEET,A.PROCESS_ID "
                 + "           FROM SAJET.SYS_RC_PROCESS_SHEET A,SAJET.SYS_RC_SHEET B "
                 + "          WHERE A.SHEET_NAME = B.SHEET_NAME "
                 + "            AND A.SHEET_PHASE = 'I') D "
                 + "  WHERE A.SHEET_NAME = B.SHEET_NAME "
                 + "    AND A.PROCESS_ID = C.PROCESS_ID "
                 + "    AND A.PROCESS_ID = D.PROCESS_ID(+) "
                 + "    AND A.SHEET_NAME = D.NEXT_SHEET(+) "
                 + "    AND A.SHEET_PHASE = 'O' ";

            if (combShow.SelectedIndex == 0)
                sSQL = sSQL + " AND C.ENABLED = 'Y' ";
            else if (combShow.SelectedIndex == 1)
                sSQL = sSQL + " AND C.ENABLED = 'N' ";

            if (combFilter.SelectedIndex > -1 && editFilter.Text.Trim() != "")
            {
                string sFieldName = combFilterField.Items[combFilter.SelectedIndex].ToString();
                if (combShow.SelectedIndex <= 1)
                    sSQL = sSQL + " AND ";
                else
                    sSQL = sSQL + " WHERE ";
                sSQL = sSQL + sFieldName + " LIKE '" + editFilter.Text.Trim() + "%'";
            }
            sSQL = sSQL + " ORDER BY " + g_sOrderField;

            g_sDataSQL = sSQL;
            (new MESGridView.DisplayGridView()).GetGridView(gvData, sSQL, out memoryCache);

            ////栏位Title  
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
                    gvData.Columns[sGridField].DisplayIndex = i; //栏位显示顺序
                    gvData.Columns[sGridField].Visible = true;
                }
            }
            gvData.Focus();
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
            fData f = new fData();
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
                        //第一个有显示的栏位(focus到隐藏栏位会错误)
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
            }
        }

        private void gvData_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            e.Value = memoryCache.RetrieveElement(e.RowIndex, e.ColumnIndex);
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

        private void fMain_Shown(object sender, EventArgs e)
        {
            combFilter.SelectedIndex = 0;
            editFilter.Focus();
        }

    }
}