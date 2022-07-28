using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using SajetTable;

namespace DefectTypeDll
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
        public String g_sOrderField, g_sOrderDetailField;
        string g_sDataSQL, g_sDetailDataSQL;
        

        private void Initial_Form() //初始設定
        {
            g_sUserID = ClientUtils.UserPara1;
            g_sProgram = ClientUtils.fProgramName;
            g_sFunction = ClientUtils.fFunctionName;
            g_sOrderField = TableDefine.gsDef_OrderField;
            g_sOrderDetailField = TableDefine.gsDef_DtlOrderField;
            SajetCommon.SetLanguageControl(this);
            panel1.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
        }
        private void fMain_Load(object sender, EventArgs e)
        {
            TableDefine.Initial_Table(); //引用SajetTable類別之Initial_Table()建立資料表
            Initial_Form(); //呼叫初始設定

            //
            combShow.SelectedIndex = 0;
            combDetailShow.SelectedIndex = 0;
            this.Text = this.Text + "(" + SajetCommon.g_sFileVersion + ")";  //表頭顯示資料部分

            //Filter - Master
            combFilter.Items.Clear();
            combFilterField.Items.Clear();
            for (int i = 0; i <= TableDefine.tGridField.Length - 1; i++)  //顯示下拉式查詢資訊
            {
                combFilter.Items.Add(TableDefine.tGridField[i].sCaption);   //引用SajetTable裡的資料欄位，並將加入CombFilter下拉式選單項目中
                combFilterField.Items.Add(TableDefine.tGridField[i].sFieldName);
            }

            //Filter - Detail
            combDetailFilter.Items.Clear();
            combDetailFilter.Items.Clear();
            for (int i = 0; i <= TableDefine.tGridDetailField.Length - 1; i++)
            {
                combDetailFilter.Items.Add(TableDefine.tGridDetailField[i].sCaption);
                combDetailFilterField.Items.Add(TableDefine.tGridDetailField[i].sFieldName);
            }

            if (combFilter.Items.Count > 0)
                combFilter.SelectedIndex = 0;
                

            Check_Privilege(); //確認權限
        }
        private void Check_Privilege()  //取得權限部分
        {
            string sPrivilege = ClientUtils.GetPrivilege(g_sUserID, g_sFunction, g_sProgram).ToString();
            btnAppend.Enabled = SajetCommon.CheckEnabled("INSERT", sPrivilege);
            btnModify.Enabled = SajetCommon.CheckEnabled("UPDATE", sPrivilege);
            btnEnabled.Enabled = SajetCommon.CheckEnabled("ENABLED", sPrivilege);
            btnDisabled.Enabled = SajetCommon.CheckEnabled("DISABLED", sPrivilege);
            btnDelete.Enabled = SajetCommon.CheckEnabled("DELETE", sPrivilege);

            btnDetailAppend.Enabled = SajetCommon.CheckEnabled("INSERT", sPrivilege);
            btnDetailModify.Enabled = SajetCommon.CheckEnabled("UPDATE", sPrivilege);
            btnDetailEnabled.Enabled = SajetCommon.CheckEnabled("ENABLED", sPrivilege);
            btnDetailDisabled.Enabled = SajetCommon.CheckEnabled("DISABLED", sPrivilege);
            btnDetailDelete.Enabled = SajetCommon.CheckEnabled("DELETE", sPrivilege);
        }

        private void combShow_SelectedIndexChanged(object sender, EventArgs e)  //判斷ComShow選擇{可用;停用;全部}，給予可用按鈕的權限
        {
            btnDelete.Visible = (combShow.SelectedIndex == 1);
            btnDisabled.Visible = (combShow.SelectedIndex == 0);
            btnEnabled.Visible = (combShow.SelectedIndex == 1);
            //顯示查詢資訊
            ShowData();
            SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);
        }

        public void ShowData()
        {
            gvData.Rows.Clear();  //清除測試大查詢資訊

            sSQL = "Select * from " + TableDefine.gsDef_Table + " ";    //SajetTable.cs中的參數gsDef_Table，定義為 SAJET.SYS_TEST_ITEM_TYPE的資料表名稱(測試大項)
            if (combShow.SelectedIndex == 0)
                sSQL = sSQL + " where Enabled = 'Y' ";
            else if (combShow.SelectedIndex == 1)
                sSQL = sSQL + " where Enabled = 'N' ";

            if (combFilter.SelectedIndex > -1 && editFilter.Text.Trim() != "")   //判斷在測試大項如有加入查詢後，顯示資訊範圍資料
            {
                string sFieldName = combFilterField.Items[combFilter.SelectedIndex].ToString();
                if (combShow.SelectedIndex <= 1)
                    sSQL = sSQL + " and ";
                else
                    sSQL = sSQL + " where ";
                sSQL = sSQL + sFieldName + " like '" + editFilter.Text.Trim() + "%'";
            }
            sSQL = sSQL + " order by " + g_sOrderField; //*****預設排序欄位(SajetTable.cs)
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
            //---------------------------------------------------------------------------------------------------
            gvData.Focus();
        }

        private void btnDisable_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;

            string sID = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();  //資料表中的ITEM_TYPE_ID欄位名稱值
            string sType = "";
            string sEnabled = "";
            if (sender == btnDisabled)
            {
                sType = btnDisabled.Text;   //sType = Disabled
                sEnabled = "N";
            }
            else if (sender == btnEnabled)
            {
                sType = btnEnabled.Text;  //sType = Enabled
                sEnabled = "Y";
            }
            //Columns欄位名稱;CurrentRow欄位值
            string sData = gvData.Columns[TableDefine.gsDef_KeyData].HeaderText + " : " + gvData.CurrentRow.Cells[TableDefine.gsDef_KeyData].Value.ToString();
            string sMsg = sType + " ?" + Environment.NewLine + sData; //{Disabled;Enabled}?+sData

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

        private void editFilter_KeyPress(object sender, KeyPressEventArgs e) //測試大項之Filter按下enter動作
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            ShowData();
            SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);  //(顯示區域,"",ITEM_TYPE_ID)

            editFilter.Focus();
        }

        private void btnAppend_Click(object sender, EventArgs e)   //測試大項-新增部分
        {
            fData f = new fData();
            try
            {
                f.g_sUpdateType = "APPEND";
                f.g_sformText = btnAppend.Text;  //當按下新增時，將btnAppend的值顯示於主畫面標頭上
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

        private void btnModify_Click(object sender, EventArgs e)  //修改部分
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

                string sMsg = btnDelete.Text + " ?" + Environment.NewLine + sData;
                if (SajetCommon.Show_Message(sMsg, 2) != DialogResult.Yes)
                    return;

                sSQL = " Update " + TableDefine.gsDef_Table + " "
                     + " set Enabled = 'Drop'  "                            //將狀態更新為Drop
                     + "    ,UPDATE_USERID = '" + g_sUserID + "'  "
                     + "    ,UPDATE_TIME = SYSDATE  "
                     + " where " + TableDefine.gsDef_KeyField + " = '" + sID + "'";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                CopyToHistory(sID);  //將刪除動作紀錄至歷史區
                //刪除主資料部分
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
        //===========================================================================================================
        public static void CopyToHistory(string sID)
        {
            string sSQL = " Insert into " + TableDefine.gsDef_HTTable + " "     //gsDef_HTTable測試大項異動歷史紀錄區
                        + " Select * from " + TableDefine.gsDef_Table + " "
                        + " where " + TableDefine.gsDef_KeyField + " = '" + sID + "' "; //LABEL_TYPE_ID
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
        }
        //=============================================================================================================
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
                    string sDataSQL = g_sDataSQL;
                    if (GridData != gvData)
                        sDataSQL = g_sDetailDataSQL;
                    string sText = "select idx from ("
                                 + " Select aa.*,rownum-1 idx from ("
                                 + sDataSQL
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
            sSQL = "";
            DataGridView dvControl = (DataGridView)contextMenuStrip1.SourceControl;
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
            if (gvData.CurrentRow == null)
                return;

            ShowDetailData();

            gvData.Focus();
        }

        private void gvData_SelectionChanged(object sender, EventArgs e)  //判斷查詢出來的gvData資訊有變動，便重新觸發ShowDetailData重新顯示資訊
        {
            if (gvData.CurrentRow == null)
                return;

            ShowDetailData();
            gvData.Focus();
        }

        private void combDetailShow_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDetailDelete.Visible = (combDetailShow.SelectedIndex == 1);
            btnDetailDisabled.Visible = (combDetailShow.SelectedIndex == 0);
            btnDetailEnabled.Visible = (combDetailShow.SelectedIndex == 1);

            ShowDetailData();
            SetSelectRow(gvDetail, "", TableDefine.gsDef_DtlKeyField);
        }


        public void ShowDetailData() 
        {
            gvDetail.Rows.Clear();    
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;
            string sDefectTypeCode = gvData.CurrentRow.Cells[TableDefine.gsDef_OrderField].Value.ToString();

            //sSQL = " SELECT a.EMP_ID,a.DEFECT_TYPE_ID,c.EMP_NO,c.EMP_NAME FROM sajet.sys_defect_type_emp a,sajet.sys_defect_type b,sajet.sys_emp c "
            //    + " where b.DEFECT_TYPE_CODE = '" + sDefectTypeCode + "'"
            //    + " AND a.DEFECT_TYPE_ID = b.DEFECT_TYPE_ID"
            //    + " and a.emp_id = c.emp_id ";
           
            sSQL = @"select a.emp_no,a.emp_name from sajet.sys_emp a,sajet.sys_defect_type b,sajet.sys_defect_type_emp c where
                    b.defect_type_code = '"+ sDefectTypeCode +"' and b.defect_type_id = c.defect_type_id and c.emp_id = a.emp_id ";

            
            if (combDetailShow.SelectedIndex == 0)
                sSQL = sSQL + " and c.Enabled = 'Y' ";
            else if(combDetailShow.SelectedIndex == 1)
                sSQL = sSQL + " and c.Enabled = 'N' ";

            if (combDetailFilter.SelectedIndex > -1 && editDetailFilter.Text.Trim() != "")
            {
                string sFieldName = combDetailFilterField.Items[combDetailFilter.SelectedIndex].ToString();
                sSQL = sSQL + " and UPPER(" + sFieldName + ") like UPPER('" + editDetailFilter.Text.Trim() + "%')";
            }
            sSQL = sSQL + " order by " + g_sOrderDetailField;
            g_sDetailDataSQL = sSQL;

            (new MESGridView.DisplayGridView()).GetGridView(gvDetail, sSQL, out memoryCacheDetail);

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
                    gvDetail.Columns[sGridField].DisplayIndex = i;
                    gvDetail.Columns[sGridField].Visible = true;
                }
            }
            gvDetail.Focus();
        }
        private void gvDetail_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            e.Value = memoryCacheDetail.RetrieveElement(e.RowIndex, e.ColumnIndex);
        }

        private void editDetailFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            ShowDetailData();

            SetSelectRow(gvDetail, "", TableDefine.gsDef_DtlKeyField);

            editDetailFilter.Focus();
        }

        private void btnDetailDisabled_Click(object sender, EventArgs e)
        {
            if (gvDetail.Rows.Count == 0 || gvDetail.CurrentRow == null)
                return;

            string sID = gvDetail.CurrentRow.Cells[TableDefine.gsDef_DtlKeyField].Value.ToString();
            string sType = "";
            string sEnabled = "";
            if (sender == btnDetailDisabled)
            {
                sType = btnDetailDisabled.Text;
                sEnabled = "N";
            }
            else if (sender == btnDetailEnabled)
            {
                sType = btnDetailEnabled.Text;
                sEnabled = "Y";
            }
            string sData = gvDetail.Columns[TableDefine.gsDef_DtlKeyData].HeaderText + " : " + gvDetail.CurrentRow.Cells[TableDefine.gsDef_DtlKeyData].Value.ToString();
            string sMsg = sType + " ?" + Environment.NewLine + sData;

            if (SajetCommon.Show_Message(sMsg, 2) != DialogResult.Yes)
                return;

            string sDefectTypeCode = gvData.CurrentRow.Cells[TableDefine.gsDef_OrderField].Value.ToString();

            sSQL = "Select a.type_emp_id from sajet.sys_defect_type_emp a,sajet.sys_defect_type b,sajet.sys_emp c "
                    + " where  c.emp_no = '" + sID + "'"
                    + " and b.defect_type_code = '" + sDefectTypeCode + "'"
                    + " and a.defect_type_id = b.defect_type_id "
                    + " AND c.emp_id = a.emp_id ";

            //sSQL = "select a.type_emp_id from sajet.sys_defect_type_emp a,sajet.sys_emp b where b.emp_no = '"+ sID +"' and a.emp_id = b.emp_id";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            string sTypeEmpID = dsTemp.Tables[0].Rows[0]["TYPE_EMP_ID"].ToString();

            sSQL = " Update " + TableDefine.gsDef_DtlTable + " "
                + " set Enabled = '" + sEnabled + "' "
                + "    ,UPDATE_USER_ID = '" + g_sUserID + "'  "
                + "    ,UPDATE_TIME = SYSDATE  "
                + " where TYPE_EMP_ID = '" + sTypeEmpID + "'";
            ClientUtils.ExecuteSQL(sSQL);
           // CopyToDetailHistory(sID);

            ShowDetailData();
            SetSelectRow(gvDetail, "", TableDefine.gsDef_DtlKeyField);
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
                f.g_sDefectCode = gvData.CurrentRow.Cells["DEFECT_TYPE_CODE"].Value.ToString();
                f.g_sDefectDESC = gvData.CurrentRow.Cells["DEFECT_TYPE_DESC"].Value.ToString();
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
            if (gvDetail.CurrentRow == null)
                return;
            fDetailData f = new fDetailData();
            try
            {
                f.g_sUpdateType = "MODIFY";
                f.g_sformText = btnModify.Text;
                f.dataCurrentRow = gvDetail.CurrentRow;
                f.g_sDefectCode = gvData.CurrentRow.Cells["DEFECT_TYPE_CODE"].Value.ToString();
                f.g_sDefectDESC = gvData.CurrentRow.Cells["DEFECT_TYPE_DESC"].Value.ToString();
                string sSelectKeyValue = gvDetail.CurrentRow.Cells[TableDefine.gsDef_DtlKeyField].Value.ToString();
                if (f.ShowDialog() == DialogResult.OK)
                {
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
                string sData = gvDetail.Columns[TableDefine.gsDef_DtlKeyData].HeaderText + " : " + gvDetail.CurrentRow.Cells[TableDefine.gsDef_DtlKeyData].Value.ToString();
              
                string sMsg = btnDetailDelete.Text + " ?" + Environment.NewLine + sData;
                if (SajetCommon.Show_Message(sMsg, 2) != DialogResult.Yes)
                    return;

                //string sDefectTypeCode = gvData.CurrentRow.Cells[TableDefine.gsDef_OrderField].Value.ToString();

                //sSQL = "Select a.type_emp_id from sajet.sys_defect_type_emp a,sajet.sys_defect_type b,sajet.sys_emp c "
                //    + " where  c.emp_no = '" + sID + "'"
                //    + " and b.defect_type_code = '" + sDefectTypeCode + "'"
                //    + " and a.defect_type_id = b.defect_type_id ";

                string sDefectTypeCode = gvData.CurrentRow.Cells[TableDefine.gsDef_OrderField].Value.ToString();

                sSQL = "Select a.type_emp_id from sajet.sys_defect_type_emp a,sajet.sys_defect_type b,sajet.sys_emp c "
                    + " where  c.emp_no = '" + sID + "'"
                    + " and b.defect_type_code = '" + sDefectTypeCode + "'"
                    + " and a.defect_type_id = b.defect_type_id "
                    + " and a.emp_id = c.emp_id";

                //sSQL = "select a.type_emp_id from sajet.sys_defect_type_emp a,sajet.sys_emp b where b.emp_no = '" + sID + "' and a.emp_id = b.emp_id";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                string sDefectID = dsTemp.Tables[0].Rows[0]["TYPE_EMP_ID"].ToString();

                sSQL = " Delete sajet.sys_defect_type_emp"
                     + " where  TYPE_EMP_ID = '" + sDefectID + "'";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);

                ShowDetailData();
                SetSelectRow(gvDetail, "", TableDefine.gsDef_DtlKeyField);
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception:" + ex.Message, 0);
            }
        }

        private void btnDetailExport_Click(object sender, EventArgs e)
        {
            saveFileDialog1.DefaultExt = "xls";
            saveFileDialog1.Filter = "All Files(*.xls)|*.xls";
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            string sFileName = saveFileDialog1.FileName;

            ExportExcel.CreateExcel Export = new ExportExcel.CreateExcel(sFileName);
            if (sender == btnDetailExport)
                Export.ExportToExcel(gvDetail);
        }

        
    }
}