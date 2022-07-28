using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using SajetTable;

namespace CSamplingPlan
{
    public partial class fMain : Form
    {
        private MESGridView.Cache memoryCache;
        private MESGridView.Cache memoryCacheDetail;

        public fMain()
        {
            InitializeComponent();
        }

        string sSQL;
        public static String g_sUserID;
        public String g_sProgram, g_sFunction; 
        DataSet dsTemp;
        public String g_sOrderField,g_sOrderDetailField;
        string g_sDataSQL, g_sDataDetailSQL;

        private void Initial_Form()
        {
            g_sUserID = ClientUtils.UserPara1;
            g_sProgram = ClientUtils.fProgramName;
            g_sFunction = ClientUtils.fFunctionName;
            g_sOrderField = TableDefine.gsDef_OrderField;
            g_sOrderDetailField = TableDefine.gsDef_DtlOrderField;
            SajetCommon.SetLanguageControl(this);
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            panel1.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            TableDefine.Initial_Table();
            Initial_Form();

            //
            combShow.SelectedIndex = 0;
            combDetailShow.SelectedIndex = 0;
            this.Text = this.Text + "(" + SajetCommon.g_sFileVersion + ")";

            //Filter - Master
            combFilter.Items.Clear();
            combFilterField.Items.Clear();
            for (int i = 0; i <= TableDefine.tGridField.Length - 1; i++)
            {
                combFilter.Items.Add(TableDefine.tGridField[i].sCaption);
                combFilterField.Items.Add(TableDefine.tGridField[i].sFieldName);
            }
            if (combFilter.Items.Count > 0)
                combFilter.SelectedIndex = 0;

            //Filter - Detail
            combDetailFilter.Items.Clear();
            combDetailFilter.Items.Clear();
            for (int i = 0; i <= TableDefine.tGridDetailField.Length - 1; i++)
            {
                combDetailFilter.Items.Add(TableDefine.tGridDetailField[i].sCaption);
                combDetailFilterField.Items.Add(TableDefine.tGridDetailField[i].sFieldName);
            }
            if (combDetailFilter.Items.Count > 0)
                combDetailFilter.SelectedIndex = 0;

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


            btnDetailAppend.Enabled = btnAppend.Enabled;
            btnDetailModify.Enabled = btnModify.Enabled;           
            btnDetailDelete.Enabled = btnDelete.Enabled;
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
            gvData.Rows.Clear();
            gvDetail.Rows.Clear();
            
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;
            
            try
            {
                string sID = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();
                string sData = gvData.Columns[TableDefine.gsDef_KeyData].HeaderText + " : " + gvData.CurrentRow.Cells[TableDefine.gsDef_KeyData].Value.ToString();
                
                //此Sampling下有Detail,不可刪除
                sSQL = " Select * from Sajet.sys_qc_sampling_plan_detail "
                     + " where sampling_id = '" + sID + "'";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    string sErrMsg1 = SajetCommon.SetLanguage("has one or more Detail", 1);
                    string sErrMsg2 = SajetCommon.SetLanguage("Can not delete it", 1);
                    SajetCommon.Show_Message(sData + " " + sErrMsg1 + Environment.NewLine
                                            + sErrMsg2 + "!", 0);                                        
                    return;
                }

                //此Sampling下有預設抽驗計畫,不可刪除
                sSQL = " Select * from Sajet.sys_qc_sampling_default "
                     + " where sampling_id = '" + sID + "'";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    string sErrMsg1 = SajetCommon.SetLanguage("has one or more Part", 1);
                    string sErrMsg2 = SajetCommon.SetLanguage("Can not delete it", 1);
                    SajetCommon.Show_Message(sData + " " + sErrMsg1 + Environment.NewLine
                                            + sErrMsg2 + "!", 0);
                    return;
                }
                //=====                
                string sMsg = btnDelete.Text + " ?" + Environment.NewLine + sData;
                if (SajetCommon.Show_Message(sMsg, 2) != DialogResult.Yes)
                    return;

                sSQL = " Update " + TableDefine.gsDef_Table + " "
                     + " set Enabled = 'Drop'  "
                     + "    ,UPDATE_USERID = '" + g_sUserID + "'  "
                     + "    ,UPDATE_TIME = SYSDATE  "
                     + " where " + TableDefine.gsDef_KeyField + " = '" + sID + "'";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                CopyToHistory(sID);
                sSQL = " Delete " + TableDefine.gsDef_Table + " "
                     + " where " + TableDefine.gsDef_KeyField + " = '" + sID + "'";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);

                ShowData();
                SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception:" + ex.Message, 0);
            }
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
            if (sender == btnExport)
                Export.ExportToExcel(gvData);
            else if (sender == btnDetailExport)
                Export.ExportToExcel(gvDetail);
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
                    for (int i = 0; i <= GridData.Rows.Count - 1; i++)
                    {
                        if (sPrimaryKey == GridData.Rows[i].Cells[sField].Value.ToString())
                        {
                            iIndex = i;
                            break;
                        }
                    }
                }
                GridData.Focus();
                GridData.CurrentCell = GridData.Rows[iIndex].Cells[sShowField];
                GridData.Rows[iIndex].Selected = true;
            }
        }
        /* //由於SQL中有ROWID,使用此方式會有錯誤      
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
                                 + " Select aa.*,rownum-1 idx from (";
                    if (GridData.Name == "gvData")
                        sText = sText + g_sDataSQL;
                    else
                        sText = sText + g_sDataDetailSQL;
                    sText = sText
                        +" ) aa ) "
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
        */
        private void gvData_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            e.Value = memoryCache.RetrieveElement(e.RowIndex, e.ColumnIndex);
        }

        private void MenuHistory_Click(object sender, EventArgs e)
        {
            sSQL = "";
            DataGridView dvControl =  (DataGridView)contextMenuStrip1.SourceControl;
            if (dvControl == gvData)
            {
                if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                    return;
                string sFieldID = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();
                sSQL = TableDefine.History_SQL(sFieldID);
            }

            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);

            fHistory fHistory = new fHistory();
            fHistory.dgvHistory.DataSource = dsTemp;
            fHistory.dgvHistory.DataMember = dsTemp.Tables[0].ToString();
            //替換欄位名稱
            for (int i = 0; i <= fHistory.dgvHistory.Columns.Count - 1; i++)
            {
                string sGridField = fHistory.dgvHistory.Columns[i].HeaderText;
                string sField = "";
                for (int j = 0; j <= dvControl.Columns.Count - 1; j++)
                {
                    sField = dvControl.Columns[j].Name;
                    if (sGridField == sField)
                    {
                        fHistory.dgvHistory.Columns[i].HeaderText = dvControl.Columns[j].HeaderText;
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

        public void ShowDetailData()
        {
            gvDetail.Rows.Clear();
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;
            string sSamplingID = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();

            sSQL = "select A.*,ROWID, "
                //+ "DECODE(SAMPLING_LEVEL,'-1','N/A', '0', 'Normal', '1', 'Tight', '2', 'Reduced', '3','No Inspect','N/A') AS SAMPLING_LEVEL_DESC,"
                    + " DECODE(SAMPLING_UNIT,'-1','N/A', '0', 'Qty', '1', '%', 'N/A') AS SAMPLING_UNIT_DESC "
                    + " from SAJET.SYS_QC_SAMPLING_PLAN_DETAIL A "
                    + " where sampling_id = '" + sSamplingID + "'"
                    + " and sampling_level = '" + combDetailShow.SelectedIndex.ToString() + "'";
                    //+ " order by min_lot_size";

            if (combDetailFilter.SelectedIndex > -1 && editDetailFilter.Text.Trim() != "")
            {
                string sFieldName = combDetailFilterField.Items[combDetailFilter.SelectedIndex].ToString();
                if (sFieldName == "SAMPLING_UNIT_DESC")
                {
                    sFieldName = "DECODE(SAMPLING_UNIT,'-1','N/A', '0', 'Qty', '1', '%', 'N/A')";
                }
                sSQL = sSQL + " and " + sFieldName + " like '" + editDetailFilter.Text.Trim() + "%'";
            }
            sSQL = sSQL + " order by " + g_sOrderDetailField;
            g_sDataDetailSQL = sSQL;
            (new MESGridView.DisplayGridView()).GetGridView(gvDetail, sSQL, out memoryCacheDetail);
          

            //欄位Title  
            for (int i = 0; i <= gvDetail.Columns.Count - 1; i++)
            {
                gvDetail.Columns[i].Visible = false;
            }
            for (int i = 0; i <= TableDefine.tGridDetailField.Length - 1; i++)
            {
                string sGridField = TableDefine.tGridDetailField[i].sFieldName;

                if (gvDetail.Columns.Contains(sGridField))
                {
                    gvDetail.Columns[sGridField].HeaderText = TableDefine.tGridDetailField[i].sCaption;
                    gvDetail.Columns[sGridField].DisplayIndex = i; //欄位顯示順序
                    gvDetail.Columns[sGridField].Visible = true;
                }
            }
            gvDetail.Focus();
        }

        private void gvDetail_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            e.Value = memoryCacheDetail.RetrieveElement(e.RowIndex, e.ColumnIndex);
        }

        private void gvData_SelectionChanged(object sender, EventArgs e)
        {
            ShowDetailData();
            gvData.Focus();
        }

        private void combDetailShow_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowDetailData();
            SetSelectRow(gvDetail, "", TableDefine.gsDef_DtlKeyField);
        }

        private void editDetailFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            ShowDetailData();
            SetSelectRow(gvDetail, "", TableDefine.gsDef_DtlKeyField);

            editDetailFilter.Focus();
        }
        private void gvDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex > -1)
            {
                g_sOrderDetailField = gvDetail.Columns[e.ColumnIndex].Name;
                ShowDetailData();
                SetSelectRow(gvDetail, "", TableDefine.gsDef_DtlKeyField);
            }
        }

        private void btnDetailAppend_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;
            
            fDetailData f = new fDetailData();
            try
            {
                f.g_sUpdateType = "APPEND";
                f.g_sformText = btnAppend.Text;
                f.g_sSamplingID = gvData.CurrentRow.Cells["SAMPLING_ID"].Value.ToString();
                f.g_sSamplingName = gvData.CurrentRow.Cells["sampling_type"].Value.ToString();
                f.g_nLevel = combDetailShow.SelectedIndex;
                if (f.ShowDialog() == DialogResult.OK)
                {
                    ShowDetailData();
                    SetSelectRow(gvDetail, "", TableDefine.gsDef_DtlKeyField);
                }
            }
            finally
            {
                f.Dispose();
            }
        }

        private void btnDetailModify_Click(object sender, EventArgs e)
        {
            if (gvDetail.Rows.Count == 0 || gvDetail.CurrentRow == null)
                return;
            fDetailData f = new fDetailData();
            try
            {
                f.g_sUpdateType = "MODIFY";
                f.g_sformText = btnModify.Text;
                f.dataCurrentRow = gvDetail.CurrentRow;
                f.g_nLevel = combDetailShow.SelectedIndex;
                f.g_sSamplingID = gvData.CurrentRow.Cells["SAMPLING_ID"].Value.ToString();
                f.g_sSamplingName = gvData.CurrentRow.Cells["sampling_type"].Value.ToString();
                string sSelectKeyValue = gvDetail.CurrentRow.Cells[TableDefine.gsDef_DtlKeyField].Value.ToString();
                if (f.ShowDialog() == DialogResult.OK)
                {
                    combDetailShow.SelectedIndex = f.g_nLevel;
                    ShowDetailData();
                    SetSelectRow(gvDetail, sSelectKeyValue, TableDefine.gsDef_DtlKeyField);
                }
            }
            finally
            {
                f.Dispose();
            }
        }

        private void btnDetailDelete_Click(object sender, EventArgs e)
        {
            if (gvDetail.Rows.Count == 0 || gvDetail.CurrentRow == null)
                return;

            try
            {
                string sID = gvDetail.CurrentRow.Cells[TableDefine.gsDef_DtlKeyField].Value.ToString();
                string sData = SajetCommon.SetLanguage("Lot Size",1) +" : "+ gvDetail.CurrentRow.Cells["min_lot_size"].Value.ToString() + " ~ " + gvDetail.CurrentRow.Cells["max_lot_size"].Value.ToString();
               
                //=====                
                string sMsg = btnDetailDelete.Text + " ?" + Environment.NewLine + sData;
                if (SajetCommon.Show_Message(sMsg, 2) != DialogResult.Yes)
                    return;

                sSQL = " Delete " + TableDefine.gsDef_DtlTable + " "
                     + " where " + TableDefine.gsDef_DtlKeyField + " = '" + sID + "'";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);

                ShowDetailData();
                SetSelectRow(gvDetail, "", TableDefine.gsDef_DtlKeyField);
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception:" + ex.Message, 0);
            }
        }
    }
}