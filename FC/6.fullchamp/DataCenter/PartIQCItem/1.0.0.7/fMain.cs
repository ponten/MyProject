using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using SajetTable;
using SajetFilter;

namespace CPartIQCItem
{
    public partial class fMain : Form
    {
        //�Φ��覡�e���t���ܺC,�]�����ϥ�
        //private MESGridView.Cache memoryCache;
        //private MESGridView.Cache memoryCacheDetail;
        //private MESGridView.Cache memoryCacheType;

        public fMain()
        {
           InitializeComponent();
        }

        string sSQL;
        public static String g_sUserID;
        String g_sProgram, g_sFunction;
        DataSet dsTemp;
        public String g_sOrderField, g_sOrderTypeField, g_sOrderDetailField;
        String g_sPartID = "0", g_sRECID = "0", g_sItemTypeID = "0";

        private void Initial_Form()
        {
            g_sUserID = ClientUtils.UserPara1;
            g_sProgram = ClientUtils.fProgramName;
            g_sFunction = ClientUtils.fFunctionName;

            g_sOrderField = TableDefine.gsDef_OrderField;
            g_sOrderTypeField = TableDefine.gsDef_TypeOrderField;               
            g_sOrderDetailField = TableDefine.gsDef_DtlOrderField;

            SajetCommon.SetLanguageControl(this);
        }
        private void fMain_Load(object sender, EventArgs e)
        {
            TableDefine.Initial_Table();
            Initial_Form();           
            this.Text = this.Text + "(" + SajetCommon.g_sFileVersion + ")";

            //Filter - Master
            combFilter.Items.Clear();
            combFilterField.Items.Clear();
            for (int i = 0; i <= TableDefine.tGridField.Length - 1; i++)
            {
                combFilter.Items.Add(TableDefine.tGridField[i].sCaption);
                combFilterField.Items.Add(TableDefine.tGridField[i].sFieldName);
            }

            Check_Privilege();

            bindingNavigator1.Enabled = false;
            bindingNavigator2.Enabled = false;
            bindingNavigator3.Enabled = false;
            
            editPartNo.Focus();            
        }
        private void Check_Privilege()
        {
            int iPrivilege = ClientUtils.GetPrivilege(g_sUserID, g_sFunction, g_sProgram);
            
            btnAppend.Visible = (iPrivilege >= 1);
            btnModify.Visible = btnAppend.Visible;           
            btnDelete.Visible = (iPrivilege >= 2);

            btnTypeAppend.Visible = btnAppend.Visible;
            btnTypeModify.Visible = btnAppend.Visible;
            btnTypeDelete.Visible = btnDelete.Visible;

            btnDetailAppend.Visible = btnAppend.Visible;
            btnDetailModify.Visible = btnAppend.Visible;
            btnDetailDelete.Visible = btnDelete.Visible;     
        }
        private string GetPartID(string sPartNO)
        {
            sSQL = "Select Part_ID from Sajet.sys_part "
                             + "Where Part_NO = '" + sPartNO + "' "
                             + "and Enabled = 'Y' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                return "0";
            }
            return dsTemp.Tables[0].Rows[0]["PART_ID"].ToString();
        }
        public void ShowData()
        {
            g_sRECID = "0";
            ClearType();
            ClearDetail();
            g_sPartID = GetPartID(editPartNo.Text);         
            sSQL = " SELECT A.* "
                 + "        ,NVL(C.VENDOR_CODE,'N/A') VENDOR_CODE ,NVL(C.VENDOR_NAME,'N/A') VENDOR_NAME,B.SAMPLING_RULE_NAME "
                 + "FROM SAJET.SYS_PART_IQC_VENDOR_RULE A "
                 + "    ,SAJET.SYS_QC_SAMPLING_RULE B "
                 + "    ,SAJET.SYS_VENDOR C  "
                 + "WHERE A.PART_ID = '" + g_sPartID + "'"
                 + "AND A.SAMPLING_RULE_ID = B.SAMPLING_RULE_ID(+)  "
                 + "AND A.VENDOR_ID = C.VENDOR_ID(+) "
                 + "AND A.ENABLED ='Y' ";
            if (combFilter.SelectedIndex > -1 && editFilter.Text.Trim() != "")
            {
                string sFieldName = combFilterField.Items[combFilter.SelectedIndex].ToString();                
                sSQL = sSQL +" and "+ sFieldName + " like '" + editFilter.Text.Trim() + "'";
            }
            sSQL = sSQL + "order by " + g_sOrderField + " ";
            //(new MESGridView.DisplayGridView()).GetGridView(gvData, sSQL, out memoryCache);           
            dsTemp = ClientUtils.ExecuteSQL(sSQL);            
            gvData.DataSource = dsTemp;
            gvData.DataMember = dsTemp.Tables[0].TableName;

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
        public void ShowDetailType(string sRECID)
        {
            ClearDetail();
            //if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
            //    return;

            sSQL = "Select a.* "
                 + "      ,b.item_type_code,b.item_type_name,c.sampling_type,d.sampling_level_desc "
                 + " from " + TableDefine.gsDef_TypeTable + " a "
                 + "      ,sajet.sys_test_item_type b "
                 + "      ,sajet.sys_qc_sampling_plan c "
                 + "      ,sajet.sys_sampling_level d "
                 + " where A.RECID ='" + sRECID + "' "
                 + " and a.item_type_id = b.item_type_id "
                 + " and a.sampling_id = c.sampling_id(+) "
                 + " and a.sampling_level = d.sampling_level(+) ";
            sSQL = sSQL + " order by " + g_sOrderTypeField;
            //(new MESGridView.DisplayGridView()).GetGridView(gvType, sSQL, out memoryCacheType);

            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            gvType.DataSource = dsTemp;
            gvType.DataMember = dsTemp.Tables[0].TableName;

            //���Title  
            for (int i = 0; i <= gvType.Columns.Count - 1; i++)
            {
                gvType.Columns[i].Visible = false;
            }
            for (int i = 0; i <= TableDefine.tGridTypeField.Length - 1; i++)
            {
                string sGridField = TableDefine.tGridTypeField[i].sFieldName;

                if (gvType.Columns.Contains(sGridField))
                {
                    gvType.Columns[sGridField].HeaderText = TableDefine.tGridTypeField[i].sCaption;
                    gvType.Columns[sGridField].DisplayIndex = i; //�����ܶ���
                    gvType.Columns[sGridField].Visible = true;
                }
            }
            gvType.Focus();
        }
        public void ShowDetailData(string sItemTypeID)
        {
            //if (gvType.Rows.Count == 0 || gvType.CurrentRow == null)
            //    return;

            sSQL = " select a.* "
                 + "       ,d.item_type_code,d.item_type_name "
                 + "       ,c.item_code,c.item_name "
                 + " from " + TableDefine.gsDef_DtlTable + " a  "
                 + "  left join SAJET.SYS_TEST_ITEM c on a.ITEM_ID = c.ITEM_ID  "
                 + "  left join SAJET.SYS_TEST_ITEM_TYPE d on c.item_type_id = d.item_type_id  "
                 + " where A.RECID = '" + g_sRECID + "' "
                 + " and C.ITEM_TYPE_ID = '" + sItemTypeID + "' "
                 + " and a.enabled = 'Y' "
                 + " and c.enabled = 'Y' "
                 + " and d.enabled = 'Y' ";
            sSQL = sSQL + " order by " + g_sOrderDetailField;
            //(new MESGridView.DisplayGridView()).GetGridView(gvDetail, sSQL, out memoryCacheDetail);
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            gvDetail.DataSource = dsTemp;
            gvDetail.DataMember = dsTemp.Tables[0].TableName;

            //���Title  
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
                    gvDetail.Columns[sGridField].DisplayIndex = i; //�����ܶ���
                    gvDetail.Columns[sGridField].Visible = true;
                }
            }
            gvDetail.Focus();
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
                f.g_sPartID = g_sPartID;
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
                f.g_sPartID = g_sPartID;
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

                //��Detail,���ܰT��
                if (gvType.Rows.Count > 0)
                {
                    SajetCommon.Show_Message("Item Detail Data also delete", 1);
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

                //�P�ɧR���j�p�����
                sSQL = "DELETE "+TableDefine.gsDef_TypeTable 
                     + " WHERE RECID = '" + sID + "' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);

                sSQL = "DELETE " + TableDefine.gsDef_DtlTable
                     + " WHERE RECID = '" + sID + "' ";
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
            else if (sender == btnTypeExport)
                Export.ExportToExcel(gvType);
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
                        //�Ĥ@�Ӧ���ܪ����(focus���������|���~)
                        sShowField = GridData.Columns[i].Name;
                        break;
                    }
                }
                for (int i = 0; i <= GridData.Rows.Count - 1; i++)
                {
                    if (sPrimaryKey == GridData.Rows[i].Cells[sField].Value.ToString())
                    {
                        iIndex = i;
                        break;
                    }
                }
                GridData.Focus();
                GridData.CurrentCell = GridData.Rows[iIndex].Cells[sShowField];
                GridData.Rows[iIndex].Selected = true;
            }
        }        

        private void MenuHistory_Click(object sender, EventArgs e)
        {
            sSQL = "";
            DataGridView dvControl = (DataGridView)MenuStripMaster.SourceControl;
            //Master
            if (dvControl == gvData)
            {
                if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                    return;
                string sFieldID = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();
                sSQL = TableDefine.History_SQL(sFieldID);
            }
            //Detail
            else if (dvControl == gvDetail)
            {
                if (gvDetail.Rows.Count == 0 || gvDetail.CurrentRow == null)
                    return;
                string sFieldID = gvDetail.CurrentRow.Cells[TableDefine.gsDef_DtlKeyField].Value.ToString();
                string sRECID = gvDetail.CurrentRow.Cells["RECID"].Value.ToString();
                sSQL = TableDefine.DetailHistory_SQL(sFieldID, sRECID);
            }
            //Detail Type
            else if (dvControl == gvType)
            {
                if (gvType.Rows.Count == 0 || gvType.CurrentRow == null)
                    return;
                string sFieldID = gvType.CurrentRow.Cells[TableDefine.gsDef_TypeKeyField].Value.ToString();
                string sRECID = gvType.CurrentRow.Cells["RECID"].Value.ToString();
                sSQL = TableDefine.TypeHistory_SQL(sFieldID, sRECID);
            }

            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);

            fHistory fHistory = new fHistory();
            fHistory.dgvHistory.DataSource = dsTemp;
            fHistory.dgvHistory.DataMember = dsTemp.Tables[0].ToString();

            //�������W��
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
            //if (e.RowIndex == -1)
            //{
            //    g_sOrderField = gvData.Columns[e.ColumnIndex].Name;
            //    ShowData();
            //    SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);
            //}
        }        

        private void gvData_SelectionChanged(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null || gvData.SelectedCells.Count == 0)
            {
                g_sRECID = "0";
                return;
            }
            if (g_sRECID == gvData.CurrentRow.Cells["RECID"].Value.ToString())
                return;

            g_sRECID = gvData.CurrentRow.Cells["RECID"].Value.ToString();

            ShowDetailType(g_sRECID);
            ShowDetailData(g_sItemTypeID);

            gvData.Focus();
            
        }
                       
        public static void CopyToDetailHistory(string sID,string sRECID)
        {
            string sSQL = " Insert into " + TableDefine.gsDef_DtlHTTable + " "
                        + " Select * from " + TableDefine.gsDef_DtlTable + " "
                        + " where " + TableDefine.gsDef_DtlKeyField + " = '" + sID + "' "
                        + " and RECID = '" + sRECID + "' ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
        }
        public static void CopyToTypeHistory(string sID,string sRECID)
        {
            string sSQL = " Insert into " + TableDefine.gsDef_TypeHTTable + " "
                        + " Select * from " + TableDefine.gsDef_TypeTable + " "
                        + " where " + TableDefine.gsDef_TypeKeyField + " = '" + sID + "' "
                        + " and RECID = '" + sRECID + "' ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
        }

        private void gvDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex == -1)
            //{
            //    g_sOrderDetailField = gvDetail.Columns[e.ColumnIndex].Name;
            //    ShowDetailData();
            //    SetSelectRow(gvDetail, "", TableDefine.gsDef_DtlKeyField);
            //}
        }

        private void btnDetailAppend_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null || gvType.Rows.Count == 0 || gvType.CurrentRow == null)
                return;

            fDetailData f = new fDetailData();
            try
            {
                f.g_sUpdateType = "APPEND";
                f.g_sformText = btnAppend.Text;
                f.g_sRECID = g_sRECID;
                f.g_sProcess = gvData.CurrentRow.Cells["VENDOR_NAME"].Value.ToString();
                f.g_sItemTypeCode = gvType.CurrentRow.Cells["ITEM_TYPE_CODE"].Value.ToString();
                f.g_sItemTypeID = gvType.CurrentRow.Cells["ITEM_TYPE_ID"].Value.ToString();
                if (f.ShowDialog() == DialogResult.OK)
                {
                    ShowDetailData(g_sItemTypeID);
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
                f.g_sRECID = g_sRECID;
                f.g_sProcess = gvData.CurrentRow.Cells["VENDOR_NAME"].Value.ToString();
                f.g_sItemTypeCode = gvType.CurrentRow.Cells["ITEM_TYPE_CODE"].Value.ToString();
                f.g_sItemTypeID = gvType.CurrentRow.Cells["ITEM_TYPE_ID"].Value.ToString();
                string sSelectKeyValue = gvDetail.CurrentRow.Cells[TableDefine.gsDef_DtlKeyField].Value.ToString();
                if (f.ShowDialog() == DialogResult.OK)
                {
                    ShowDetailData(g_sItemTypeID);
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

                //=====                
                string sMsg = btnDetailDelete.Text + " ?" + Environment.NewLine + sData;
                if (SajetCommon.Show_Message(sMsg, 2) != DialogResult.Yes)
                    return;

                sSQL = " Update " + TableDefine.gsDef_DtlTable + " "
                     + " set Enabled = 'Drop'  "
                     + "    ,UPDATE_USERID = '" + g_sUserID + "'  "
                     + "    ,UPDATE_TIME = SYSDATE  "
                     + " where " + TableDefine.gsDef_DtlKeyField + " = '" + sID + "'"
                     + " and RECID = '" + g_sRECID + "' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                CopyToDetailHistory(sID, g_sRECID);
                sSQL = " Delete " + TableDefine.gsDef_DtlTable + " "
                     + " where " + TableDefine.gsDef_DtlKeyField + " = '" + sID + "'"
                     + " and RECID = '" + g_sRECID + "' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);

                ShowDetailData(g_sItemTypeID);
                SetSelectRow(gvDetail, "", TableDefine.gsDef_DtlKeyField);
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception:" + ex.Message, 0);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(editPartNo.Text))
            {
                SajetCommon.Show_Message("Please Input Part No Prefix", 0);
                return;
            }
            sSQL = "Select PART_NO, SPEC1, SPEC2, VERSION "
                 + "From SAJET.SYS_PART "
                 + "Where PART_NO Like '" + editPartNo.Text + "%' "
                 + "and Enabled = 'Y' "
                 + "Order By PART_NO ";
            fFilter f = new fFilter();
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                editPartNo.Text = f.dgvData.CurrentRow.Cells["PART_NO"].Value.ToString();
                KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);
                editPartNo_KeyPress(sender, Key);
            }
        }

        private void editPartNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            bindingNavigator1.Enabled = false;
            bindingNavigator2.Enabled = false;
            bindingNavigator3.Enabled = false;

            gvData.DataSource = null;
            gvType.DataSource = null;
            gvDetail.DataSource = null;
            g_sPartID = "0";
            g_sRECID = "0";
            g_sItemTypeID = "0";
            
            editPartNo.Focus();

            if (e.KeyChar != (char)Keys.Return)
                return;

            sSQL = "Select Part_ID from Sajet.sys_part "
                 + "Where Part_NO = '" + editPartNo.Text + "' "
                 + "and Enabled = 'Y' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                SajetCommon.Show_Message("Part No Error", 0);
                editPartNo.Focus();
                editPartNo.SelectAll();
                return;
            }
            g_sPartID = dsTemp.Tables[0].Rows[0]["PART_ID"].ToString();

            bindingNavigator1.Enabled = true;
            bindingNavigator2.Enabled = true;
            bindingNavigator3.Enabled = true;

            ShowData();
            SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);
            editPartNo.Focus();
        }        

        private void gvType_SelectionChanged(object sender, EventArgs e)
        {
            if (gvType.Rows.Count == 0 || gvType.CurrentRow == null || gvType.SelectedCells.Count == 0)
            {
                g_sItemTypeID = "0";
                return;
            }
            if (g_sItemTypeID == gvType.CurrentRow.Cells["ITEM_TYPE_ID"].Value.ToString())
                return;

            g_sItemTypeID = gvType.CurrentRow.Cells["ITEM_TYPE_ID"].Value.ToString();
            ShowDetailData(g_sItemTypeID);
        }        

        private void btnTypeDelete_Click(object sender, EventArgs e)
        {
            if (gvType.Rows.Count == 0 || gvType.CurrentRow == null)
                return;

            try
            {
                string sID = gvType.CurrentRow.Cells[TableDefine.gsDef_TypeKeyField].Value.ToString();
                string sData = gvType.Columns[TableDefine.gsDef_TypeKeyData].HeaderText + " : " + gvType.CurrentRow.Cells[TableDefine.gsDef_TypeKeyData].Value.ToString();

                //��Detail,���ܰT��
                if (gvDetail.Rows.Count > 0)
                {
                    SajetCommon.Show_Message("Item Detail Data also delete", 1);
                }
                string sMsg = btnTypeDelete.Text + " ?" + Environment.NewLine + sData;
                                
                if (SajetCommon.Show_Message(sMsg, 2) != DialogResult.Yes)
                    return;

                sSQL = " Update " + TableDefine.gsDef_TypeTable + " "
                     + " set Enabled = 'Drop'  "
                     + "    ,UPDATE_USERID = '" + g_sUserID + "'  "
                     + "    ,UPDATE_TIME = SYSDATE  "
                     + " where RECID = '" + g_sRECID + "' "
                     + " and " + TableDefine.gsDef_TypeKeyField + " = '" + sID + "' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                CopyToTypeHistory(sID, g_sRECID);
                sSQL = " Delete " + TableDefine.gsDef_TypeTable + " "
                     + " where RECID = '" + g_sRECID + "' "
                     + " and " + TableDefine.gsDef_TypeKeyField + " = '" + sID + "' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);

                //�P�ɧR���p�����      
                sSQL = " DELETE " + TableDefine.gsDef_DtlTable
                     + " WHERE RECID = '" + g_sRECID + "' "
                     + " AND ITEM_ID in "
                     + "  (Select Item_ID from sajet.sys_test_item "
                     + "   where item_type_id = '" + sID + "') ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);

                ShowDetailType(g_sRECID);
                SetSelectRow(gvType, "", TableDefine.gsDef_KeyField);
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception:" + ex.Message, 0);
            }
        }

        private void gvData_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            //e.Value = memoryCache.RetrieveElement(e.RowIndex, e.ColumnIndex);
        }
        private void gvDetail_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            //e.Value = memoryCacheDetail.RetrieveElement(e.RowIndex, e.ColumnIndex);
        }
        private void gvType_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            //e.Value = memoryCacheType.RetrieveElement(e.RowIndex, e.ColumnIndex);
        }

        private void btnTypeAppend_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;

            fTypeData f = new fTypeData();
            try
            {
                f.g_sUpdateType = "APPEND";
                f.g_sformText = btnAppend.Text;
                f.g_sRECID = g_sRECID;
                if (f.ShowDialog() == DialogResult.OK)
                {
                    ShowData();
                    SetSelectRow(gvType, "", TableDefine.gsDef_TypeKeyField);
                }
            }
            finally
            {
                f.Dispose();
            }
        }

        private void btnTypeModify_Click(object sender, EventArgs e)
        {
            if (gvType.Rows.Count == 0 || gvType.CurrentRow == null)
                return;
            fTypeData f = new fTypeData();
            try
            {
                f.g_sUpdateType = "MODIFY";
                f.g_sformText = btnModify.Text;
                f.g_sRECID = g_sRECID;
                f.dataCurrentRow = gvType.CurrentRow;
                string sSelectKeyValue = gvType.CurrentRow.Cells[TableDefine.gsDef_TypeKeyField].Value.ToString();
                if (f.ShowDialog() == DialogResult.OK)
                {
                    ShowData();
                    SetSelectRow(gvType, sSelectKeyValue, TableDefine.gsDef_TypeKeyField);
                }
            }
            finally
            {
                f.Dispose();
            }
        }

        private void gvType_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex == -1)
            //{
            //    g_sOrderField = gvType.Columns[e.ColumnIndex].Name;
            //    ShowDetailType();
            //    SetSelectRow(gvType, "", TableDefine.gsDef_TypeKeyField);
            //}
        }

        private void ClearType()
        {            
            ShowDetailType("0");
        }
        private void ClearDetail()
        {            
            ShowDetailData("0");
        }       

        private void btnCopyFrom_Click(object sender, EventArgs e)
        {
            if (g_sPartID == "0")
                return;

            fCopyFrom fCopy = new fCopyFrom();
            try
            {
                if (fCopy.ShowDialog() != DialogResult.OK)
                    return;

                if (g_sPartID == fCopy.g_sCopyPartID)
                {
                    string sMsg = SajetCommon.SetLanguage("Can't copy from the same Part", 1);
                    SajetCommon.Show_Message(sMsg + Environment.NewLine + editPartNo.Text, 0);
                    return;
                }

                sSQL = "Select SYSDATE from dual ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                DateTime dtNow = (DateTime)dsTemp.Tables[0].Rows[0]["SYSDATE"];
                string sNow = dtNow.ToString("yyyy/MM/dd HH:mm:ss");
                //���R����Ӫ����
                string sCondition = " select distinct recid from SAJET.SYS_PART_IQC_VENDOR_RULE "
                                  + " where part_id = '" + g_sPartID + "' ";
                if (fCopy.g_sCopyProcessID != "0")
                    sCondition = sCondition + " and VENDOR_ID = '" + fCopy.g_sCopyProcessID + "' ";
                dsTemp = ClientUtils.ExecuteSQL(sCondition);
                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    string sMsg = SajetCommon.SetLanguage("Test Item had existed,Replace",1)+" ? " + Environment.NewLine;
                    if (fCopy.g_sCopyProcessID != "0")
                        sMsg = sMsg + SajetCommon.SetLanguage("Vendor Code", 1) + " : " + fCopy.editCopyProcess.Text;
                    if (SajetCommon.Show_Message(sMsg, 2) != DialogResult.Yes)
                        return;

                    sSQL = "DELETE SAJET.SYS_PART_IQC_TESTITEM "
                          + "WHERE RECID in (" + sCondition + ") ";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);

                    sSQL = "DELETE SAJET.SYS_PART_IQC_TESTTYPE "
                          + "WHERE RECID in (" + sCondition + ") ";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);

                    sSQL = "DELETE SAJET.SYS_PART_IQC_VENDOR_RULE "
                          + "WHERE RECID in (" + sCondition + ") ";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);
                }

                //�}�lCopy
                sSQL = " select distinct recid from SAJET.SYS_PART_IQC_VENDOR_RULE "
                     + " where part_id = '" + fCopy.g_sCopyPartID + "' ";
                if (fCopy.g_sCopyProcessID != "0")
                    sSQL = sSQL + " and VENDOR_ID = '" + fCopy.g_sCopyProcessID + "' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
                {
                    string sCopyRecID = dsTemp.Tables[0].Rows[i]["recid"].ToString();
                    string sNewRecID = SajetCommon.GetMaxID("SAJET.SYS_PART_IQC_VENDOR_RULE", "RECID", 8);
                    if (sNewRecID == "0")
                    {
                        SajetCommon.Show_Message("Get RECID Error", 0);
                        return;
                    }
                    string sSQL1 = "Insert Into SAJET.SYS_PART_IQC_VENDOR_RULE "
                         + " (RECID,PART_ID,VENDOR_ID,SAMPLING_RULE_ID,UPDATE_USERID,UPDATE_TIME ) "
                         + " SELECT '" + sNewRecID + "','" + g_sPartID + "',VENDOR_ID,SAMPLING_RULE_ID,'" + g_sUserID + "',TO_DATE('" + sNow + "','yyyy/mm/dd hh24:mi:ss') "
                         + " FROM SAJET.SYS_PART_IQC_VENDOR_RULE  "
                         + " where PART_ID = '" + fCopy.g_sCopyPartID + "' "
                         + " and RECID = '" + sCopyRecID + "' "
                         + " and enabled = 'Y' ";
                    DataSet dsTemp1 = ClientUtils.ExecuteSQL(sSQL1);

                    sSQL1 = "Insert Into SAJET.SYS_PART_IQC_TESTTYPE "
                         + " (RECID,ITEM_TYPE_ID,SAMPLING_ID,UPDATE_USERID,UPDATE_TIME ) "
                         + " SELECT '" + sNewRecID + "',ITEM_TYPE_ID,SAMPLING_ID "
                         + "       ,'" + g_sUserID + "',TO_DATE('" + sNow + "','yyyy/mm/dd hh24:mi:ss') "
                         + " FROM SAJET.SYS_PART_IQC_TESTTYPE  "
                         + " where RECID ='" + sCopyRecID + "' "
                         + " and enabled = 'Y' ";
                    dsTemp1 = ClientUtils.ExecuteSQL(sSQL1);

                    sSQL1 = "Insert Into SAJET.SYS_PART_IQC_TESTITEM "
                         + " (RECID,ITEM_ID,UPPER_LIMIT,LOWER_LIMIT,MIDDLE_LIMIT,UPPER_CONTROL_LIMIT,LOWER_CONTROL_LIMIT "
                         + " ,UPDATE_USERID,UPDATE_TIME,UNIT,sort_index ) "
                         + " SELECT '" + sNewRecID + "',ITEM_ID,UPPER_LIMIT,LOWER_LIMIT,MIDDLE_LIMIT,UPPER_CONTROL_LIMIT,LOWER_CONTROL_LIMIT "
                         + "       ,'" + g_sUserID + "',TO_DATE('" + sNow + "','yyyy/mm/dd hh24:mi:ss'),UNIT,sort_index "
                         + " FROM SAJET.SYS_PART_IQC_TESTITEM  "
                         + " where RECID ='" + sCopyRecID + "' "
                         + " and enabled = 'Y' ";
                    dsTemp1 = ClientUtils.ExecuteSQL(sSQL1);
                }
                ShowData();
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);
            }
            finally
            {
                fCopy.Dispose();
            }
        }

        private void MenuPreview_Click(object sender, EventArgs e)
        {
            /*
            DataGridView dgvData = (DataGridView)MenuStripMaster.SourceControl;

            //Preview�b��ڿ�J�ɪ�Layout�˦�
            string sProcessID = ((DataGridViewRow)gvData.CurrentRow).Cells["PROCESS_ID"].Value.ToString();
            string sPartID = ((DataGridViewRow)gvData.CurrentRow).Cells["PART_ID"].Value.ToString();
            string sProcessName = ((DataGridViewRow)gvData.CurrentRow).Cells["PROCESS_NAME"].Value.ToString();
            string sItemTypeID = "0";
            string sItemTypeName = "";
            if (dgvData == gvType)
            {
                sItemTypeID = ((DataGridViewRow)gvType.CurrentRow).Cells["ITEM_TYPE_ID"].Value.ToString();
                sItemTypeName = ((DataGridViewRow)gvType.CurrentRow).Cells["ITEM_TYPE_NAME"].Value.ToString();
            }
            TestItemInputDll.fMain fTestItem = new TestItemInputDll.fMain();
            fTestItem.G_sDisplayType = "PREVIEW";
            fTestItem.g_sPartNo = editPartNo.Text;
            fTestItem.g_sSN = "";
            fTestItem.g_sProcessID = sProcessID;
            fTestItem.g_sProcessName = sProcessName;
            fTestItem.g_sPartID = sPartID;
            fTestItem.g_sItemTypeID = sItemTypeID;
            fTestItem.g_sItemTypeName = sItemTypeName;

            fTestItem.ShowDialog();
            fTestItem.Dispose();
             */ 
        }

        private void editPartNo_TextChanged(object sender, EventArgs e)
        {
            bindingNavigator1.Enabled = false;
            bindingNavigator2.Enabled = false;
            bindingNavigator3.Enabled = false;

            gvData.DataSource = null;
            gvType.DataSource = null;
            gvDetail.DataSource = null;
            g_sPartID = "0";
            g_sRECID = "0";
            g_sItemTypeID = "0";
        }

        private void defultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;
            if (SajetCommon.Show_Message("Change it to Default ?", 2) != DialogResult.Yes)
            {
                return;
            }

            sSQL = "UPDATE SAJET.SYS_PART_IQC_VENDOR_RULE "
                + "   SET DEFAULT_FLAG ='N' "
                + "  WHERE PART_ID ='" + g_sPartID + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            sSQL = "UPDATE SAJET.SYS_PART_IQC_VENDOR_RULE "
                + "    SET DEFAULT_FLAG ='Y' "
                + "  WHERE RECID ='" + g_sRECID + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            ShowData();
            SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);


        }        
    }    
}