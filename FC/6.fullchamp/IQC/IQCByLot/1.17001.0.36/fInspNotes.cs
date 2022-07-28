using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using SajetTable;
namespace IQCbyLot
{
    public partial class fInspNotes : Form
    {
        string sSQL;
        public string g_sUserID,g_sProgram, g_sFunction;
        string g_sOrderField;
        public string g_sPartNo;

        public fInspNotes()
        {
            InitializeComponent();
        }

        private void Initial_Form()
        { 
            /*不能在第二個Form直接取用Program 因為如果又開其它模組.則會取得最後點選的模組名稱
             *所以要從主Form上傳過來*/
          //  g_sUserID = ClientUtils.UserPara1;
          //  g_sProgram = ClientUtils.fProgramName;
            g_sFunction = "Notes Maintain";//ClientUtils.fFunctionName;
            g_sOrderField = TableDefine.gsDef_OrderField;
            SajetCommon.SetLanguageControl(this);
        }

        private void Check_Privilege()
        {
            int iPrivilege = ClientUtils.GetPrivilege(g_sUserID, g_sFunction, g_sProgram);
            btnAppend.Enabled = (iPrivilege >= 1);
            btnModify.Enabled = btnAppend.Enabled;
            btnEnabled.Enabled = btnAppend.Enabled;
            btnDisabled.Enabled = btnAppend.Enabled;
            btnDelete.Enabled = (iPrivilege >= 2);
        }

        private void fInspNotes_Load(object sender, EventArgs e)
        {
            TableDefine.Initial_Table();
            Initial_Form();
            //Filter
            combFilter.Items.Clear();
            combFilterField.Items.Clear();

            for (int i = 0; i <= TableDefine.tGridField.Length - 1; i++)
            {
                combFilter.Items.Add(TableDefine.tGridField[i].sCaption);
                combFilterField.Items.Add(TableDefine.tGridField[i].sFieldName);
            }

            combShow.SelectedIndex = 0;
            Check_Privilege();

            if (combFilter.Items.Count > 0)
                combFilter.SelectedIndex = 0;
            if (!string.IsNullOrEmpty(g_sPartNo))
            {
                combFilterField.SelectedIndex = combFilterField.Items.IndexOf("PART_NO");
                combFilter.SelectedIndex = combFilterField.SelectedIndex;
                editFilter.Text = g_sPartNo;
                ShowData();
                SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);
            }
        }

        private void ShowData()
        {
            sSQL = " Select a.*,b.part_no,c.emp_no,c.emp_name "
                 + "       ,DECODE(NVL(D.RECID,'0'),'0','No','Yes') ATTACH_FILE "
                 + "    from sajet.g_iqc_notes a "
                 + "        ,sajet.sys_part b "
                 + "        ,sajet.sys_emp c "
                 + "        ,(select  recid from sajet.g_iqc_notes_image group by recid ) d "
                 + " where a.part_id = b.part_id(+) "
                 + " AND A.update_userid = c.emp_id(+) "
                 + " and a.recid = d.recid(+) ";

            if (combShow.SelectedIndex == 0)
                sSQL = sSQL + " and a.Enabled = 'Y' ";
            else if (combShow.SelectedIndex == 1)
                sSQL = sSQL + " and a.Enabled = 'N' ";

            if (combFilter.SelectedIndex > -1 && editFilter.Text.Trim() != "")
            {
                string sFieldName = combFilterField.Items[combFilter.SelectedIndex].ToString();
                sSQL = sSQL + " and " + sFieldName + " like '" + editFilter.Text.Trim() + "'";
            }

            sSQL = sSQL + " order by " + g_sOrderField+ ",A.UPDATE_TIME DESC ";
           // (new MESGridView.DisplayGridView()).GetGridView(gvData, sSQL, out memoryCache);
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            gvData.DataSource = dsTemp;
            gvData.DataMember = dsTemp.Tables[0].TableName;

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

        private void SetSelectRow(DataGridView GridData, String sPrimaryKey, String sField)
        {
            if (GridData.Rows.Count > 0)
            {
                int iIndex = 0;
                for (int i = 0; i <= GridData.Rows.Count - 1; i++)
                {
                    if (sPrimaryKey == GridData.Rows[i].Cells[sField].Value.ToString())
                    {
                        iIndex = i;
                        break;
                    }
                }
                GridData.Focus();
                GridData.CurrentCell = GridData.Rows[iIndex].Cells[TableDefine.tGridField[0].sFieldName];
                GridData.Rows[iIndex].Selected = true;
            }
        }

        private void combShow_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDelete.Visible = (combShow.SelectedIndex == 1);
            btnDisabled.Visible = (combShow.SelectedIndex == 0);
            btnEnabled.Visible = (combShow.SelectedIndex == 1);

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

        private void btnDisabled_Click(object sender, EventArgs e)
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
            string sNote = gvData.CurrentRow.Cells["NOTE"].Value.ToString();
            if ( gvData.CurrentRow.Cells["NOTE"].Value.ToString().Length > 10)
            {
                sNote = gvData.CurrentRow.Cells["NOTE"].Value.ToString().Substring(1,10)+".....";
            }
            
            string sData = gvData.Columns["PART_NO"].HeaderText + " : " + gvData.CurrentRow.Cells["PART_NO"].Value.ToString()
                          + Environment.NewLine
                          + gvData.Columns["NOTE"].HeaderText + " : " + sNote;
            string sMsg = sType + " ?" + Environment.NewLine + sData;

            if (SajetCommon.Show_Message(sMsg, 2) != DialogResult.Yes)
                return;

            sSQL = " UPDATE " + TableDefine.gsDef_Table + " "
                 + "    SET ENABLED = '" + sEnabled + "'  "
                 + "       ,UPDATE_USERID = '" + g_sUserID + "'  "
                 + "       ,UPDATE_TIME = SYSDATE  "
                 + "  WHERE " + TableDefine.gsDef_KeyField + " = '" + sID + "'";
            ClientUtils.ExecuteSQL(sSQL);
            CopyToHistory(sID);

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

        private void btnAppend_Click(object sender, EventArgs e)
        {
            fData f = new fData();

            try
            {
                f.g_sUpdateType = "APPEND";
                f.g_sPartNo = g_sPartNo;
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

            string sNote = gvData.CurrentRow.Cells["NOTE"].Value.ToString();
            if (gvData.CurrentRow.Cells["NOTE"].Value.ToString().Length > 10)
            {
                sNote = gvData.CurrentRow.Cells["NOTE"].Value.ToString().Substring(1, 10) + ".....";
            }

            string sData = gvData.Columns["PART_NO"].HeaderText + " : " + gvData.CurrentRow.Cells["PART_NO"].Value.ToString()
                          + Environment.NewLine
                          + gvData.Columns["NOTE"].HeaderText + " : " + sNote;
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

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void historyLogToolStripMenuItem_Click(object sender, EventArgs e) //歷史資料
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

        private void gvData_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {

        }

        private void gvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            /*
            if (e.RowIndex == -1)
            {
                g_sOrderField = gvData.Columns[e.ColumnIndex].Name;
                ShowData();
                SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);
            }
             */ 
        }

        private void uploadImageToolStripMenuItem_Click(object sender, EventArgs e) //附加檔案
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;

            string sFieldID = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();

            fImage fData = new fImage();

            try
            {
                fData.g_sRecID = sFieldID;
                fData.ShowDialog();
                ShowData();
                SetSelectRow(gvData, sFieldID, TableDefine.gsDef_KeyField);
            }
            finally
            {
                fData.Dispose();
            }
        }
    }
}