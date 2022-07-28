using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using SajetTable;

namespace CTestItem
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
        string g_sDataSQL, g_sDetailDataSQL;

        private void Initial_Form() //��l�]�w
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
            TableDefine.Initial_Table(); //�ޥ�SajetTable���O��Initial_Table()�إ߸�ƪ�
            Initial_Form(); //�I�s��l�]�w

            //
            combShow.SelectedIndex = 0;
            combDetailShow.SelectedIndex = 0;
            this.Text = this.Text + "(" + SajetCommon.g_sFileVersion + ")";  //���Y��ܸ�Ƴ���

            //Filter - Master
            combFilter.Items.Clear();    
            combFilterField.Items.Clear();
            for (int i = 0; i <= TableDefine.tGridField.Length - 1; i++)  //��ܤU�Ԧ��d�߸�T
            {
                combFilter.Items.Add(TableDefine.tGridField[i].sCaption);   //�ޥ�SajetTable�̪�������A�ñN�[�JCombFilter�U�Ԧ���涵�ؤ�
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
                combDetailFilter.SelectedIndex = 1;

            Check_Privilege(); //�T�{�v��
        }
        private void Check_Privilege()  //���o�v������
        {
            string sPrivilege = ClientUtils.GetPrivilege(g_sUserID, g_sFunction, g_sProgram).ToString();

            btnAppend.Enabled = SajetCommon.CheckEnabled("INSERT", sPrivilege);
            btnModify.Enabled = SajetCommon.CheckEnabled("UPDATE", sPrivilege);
            btnEnabled.Enabled = SajetCommon.CheckEnabled("ENABLED", sPrivilege);
            btnDisabled.Enabled = SajetCommon.CheckEnabled("DISABLED", sPrivilege);
            btnDelete.Enabled = SajetCommon.CheckEnabled("DELETE", sPrivilege);

            btnDetailAppend.Enabled = btnAppend.Enabled;
            btnDetailModify.Enabled = btnModify.Enabled;
            btnDetailEnabled.Enabled = btnEnabled.Enabled;
            btnDetailDisabled.Enabled = btnDisabled.Enabled;
            btnDetailDelete.Enabled = btnDelete.Enabled;
        }

        private void combShow_SelectedIndexChanged(object sender, EventArgs e)  //�P�_ComShow���{�i��;����;����}�A�����i�Ϋ��s���v��
        {
            btnDelete.Visible = (combShow.SelectedIndex == 1);
            btnDisabled.Visible = (combShow.SelectedIndex == 0);
            btnEnabled.Visible = (combShow.SelectedIndex == 1);
            //��ܬd�߸�T
            ShowData();
            SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);           
        }

        public void ShowData()
        {
            gvData.Rows.Clear();  //�M�����դj�d�߸�T
            gvDetail.Rows.Clear(); //�M�\���դp���d�߸�T

            sSQL = "Select * from " + TableDefine.gsDef_Table + " ";    //SajetTable.cs�����Ѽ�gsDef_Table�A�w�q�� SAJET.SYS_TEST_ITEM_TYPE����ƪ�W��(���դj��)
            if (combShow.SelectedIndex == 0)
                sSQL = sSQL + " where Enabled = 'Y' ";
            else if (combShow.SelectedIndex == 1)
                sSQL = sSQL + " where Enabled = 'N' ";

            if (combFilter.SelectedIndex > -1 && editFilter.Text.Trim() != "")   //�P�_�b���դj���p���[�J�d�߫�A��ܸ�T�d����
            {
                string sFieldName = combFilterField.Items[combFilter.SelectedIndex].ToString();
                if (combShow.SelectedIndex <= 1)
                    sSQL = sSQL + " and ";
                else
                    sSQL = sSQL + " where ";
                sSQL = sSQL + sFieldName + " like '" + editFilter.Text.Trim() + "%'";
            }
            sSQL = sSQL + " order by " + g_sOrderField; //*****�w�]�Ƨ����(SajetTable.cs)
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
            //---------------------------------------------------------------------------------------------------
            gvData.Focus();                       
        }

        private void btnDisable_Click(object sender, EventArgs e)
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;

            string sID = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();  //��ƪ���ITEM_TYPE_ID���W�٭�
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
            //Columns���W��;CurrentRow����
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

        private void editFilter_KeyPress(object sender, KeyPressEventArgs e) //���դj����Filter���Uenter�ʧ@
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            ShowData();
            SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);  //(��ܰϰ�,"",ITEM_TYPE_ID)

            editFilter.Focus();
        }

        private void btnAppend_Click(object sender, EventArgs e)   //���դj��-�s�W����
        {
            fData f = new fData();
            try
            {
                f.g_sUpdateType = "APPEND";
                f.g_sformText = btnAppend.Text;  //����U�s�W�ɡA�NbtnAppend������ܩ�D�e�����Y�W
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

        private void btnModify_Click(object sender, EventArgs e)  //���դp��-�קﳡ��
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
                
                //��Detail,���ܰT��
                if (gvDetail.Rows.Count > 0)
                {                    
                    SajetCommon.Show_Message("Detail Data also delete", 1);
                }

                               
                string sMsg = btnDelete.Text + " ?" + Environment.NewLine + sData;
                if (SajetCommon.Show_Message(sMsg, 2) != DialogResult.Yes)
                    return;

                sSQL = " Update " + TableDefine.gsDef_Table + " "
                     + " set Enabled = 'Drop'  "                            //�N���A��s��Drop
                     + "    ,UPDATE_USERID = '" + g_sUserID + "'  "
                     + "    ,UPDATE_TIME = SYSDATE  "
                     + " where " + TableDefine.gsDef_KeyField + " = '" + sID + "'";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                CopyToHistory(sID);  //�N�R���ʧ@�����ܾ��v��
                //�R���D��Ƴ���
                sSQL = " Delete " + TableDefine.gsDef_Table + " "
                     + " where " + TableDefine.gsDef_KeyField + " = '" + sID + "'";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);

                //�P�ɧR��Detail���
                sSQL = " Delete " + TableDefine.gsDef_DtlTable + " "
                     + " where ITEM_TYPE_ID = '" + sID + "'";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);

                ShowData();
                SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception:" + ex.Message, 0);
            }
        }
//-------------------------------------------------------------------------------------
        public static void CopyToHistory(string sID)
        {
            string sSQL = " Insert into " + TableDefine.gsDef_HTTable + " "     //gsDef_HTTable���դj�����ʾ��v������
                        + " Select * from " + TableDefine.gsDef_Table + " "
                        + " where " + TableDefine.gsDef_KeyField + " = '" + sID + "' "; //ITEM_TYPE_ID
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
//---------------------------------------------------------------------------------------------
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
            DataGridView dvControl =  (DataGridView)contextMenuStrip1.SourceControl;
            if (dvControl == gvData)
            {
                if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                    return;
                string sFieldID = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();
                sSQL = TableDefine.History_SQL(sFieldID);
            }
            else if (dvControl == gvDetail)
            {
                if (gvDetail.Rows.Count == 0 || gvDetail.CurrentRow == null)
                    return;
                string sFieldID = gvDetail.CurrentRow.Cells[TableDefine.gsDef_DtlKeyField].Value.ToString();
                sSQL = TableDefine.DetailHistory_SQL(sFieldID);
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
            if (e.RowIndex == -1 && e.ColumnIndex > -1)
            {
                g_sOrderField = gvData.Columns[e.ColumnIndex].Name;
                ShowData();
                SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);
            }
        }
//================================================================================================================
        public void ShowDetailData()   
        {
            gvDetail.Rows.Clear();
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;
            string sItemTypeID = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();

            sSQL = "Select a.* "
                 + " ,DECODE(A.VALUE_TYPE ,'0','Number','1','Character','N/A') VALUETYPE "       
                 + "from " + TableDefine.gsDef_DtlTable + " a "
                 + "where a.item_type_id = '" + sItemTypeID + "' ";
            if (combDetailShow.SelectedIndex == 0)
                sSQL = sSQL + " and a.Enabled = 'Y' ";
            else if (combDetailShow.SelectedIndex == 1)
                sSQL = sSQL + " and a.Enabled = 'N' ";

            if (combDetailFilter.SelectedIndex > -1 && editDetailFilter.Text.Trim() != "")
            {
                string sFieldName = combDetailFilterField.Items[combDetailFilter.SelectedIndex].ToString();
                sSQL = sSQL + " and " + sFieldName + " like '%" + editDetailFilter.Text.Trim() + "%'";  //20111031�ק�-�ק�LIKE�d�߻y�k�e��[�J�ʤ���Ÿ�
            }
            sSQL = sSQL + " order by " + g_sOrderDetailField;           
            g_sDetailDataSQL = sSQL;
            (new MESGridView.DisplayGridView()).GetGridView(gvDetail, sSQL, out memoryCacheDetail);
          

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
//========================================================================================================================================
        private void gvDetail_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)  
        {
            e.Value = memoryCacheDetail.RetrieveElement(e.RowIndex, e.ColumnIndex);            
        }

        private void gvData_SelectionChanged(object sender, EventArgs e)  //�P�_�d�ߥX�Ӫ�gvData��T���ܰʡA�K���sĲ�oShowDetailData���s��ܸ�T
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null || gvData.SelectedCells.Count == 0)
                return;

            ShowDetailData();
            gvData.Focus();
        }

        private void combDetailShow_SelectedIndexChanged(object sender, EventArgs e)  //�P�_ComDetailShow���{�i��;����;����}�A�����i�Ϋ��s���v��
        {
            btnDetailDelete.Visible = (combDetailShow.SelectedIndex == 1);
            btnDetailDisabled.Visible = (combDetailShow.SelectedIndex == 0);
            btnDetailEnabled.Visible = (combDetailShow.SelectedIndex == 1);

            ShowDetailData();
            SetSelectRow(gvDetail, "", TableDefine.gsDef_DtlKeyField);
        }

        private void editDetailFilter_KeyPress(object sender, KeyPressEventArgs e)  //���դp����Filter���Uenter�ʧ@
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            ShowDetailData();
            SetSelectRow(gvDetail, "", TableDefine.gsDef_DtlKeyField);

            editDetailFilter.Focus();
        }

        private void btnDetailDisabled_Click(object sender, EventArgs e)   //���դp��-���γ���
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

            sSQL = " Update " + TableDefine.gsDef_DtlTable + " "
                 + " set Enabled = '" + sEnabled + "'  "
                 + "    ,UPDATE_USERID = '" + g_sUserID + "'  "
                 + "    ,UPDATE_TIME = SYSDATE  "
                 + " where " + TableDefine.gsDef_DtlKeyField + " = '" + sID + "'";
            ClientUtils.ExecuteSQL(sSQL);
            CopyToDetailHistory(sID);

            ShowDetailData();
            SetSelectRow(gvDetail, "", TableDefine.gsDef_DtlKeyField);
        }
        public static void CopyToDetailHistory(string sID)   //���դp�����ʧ@�~���v����
        {
            string sSQL = " Insert into " + TableDefine.gsDef_DtlHTTable + " "
                        + " Select * from " + TableDefine.gsDef_DtlTable + " "
                        + " where " + TableDefine.gsDef_DtlKeyField + " = '" + sID + "' ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
        }

        private void gvDetail_CellClick(object sender, DataGridViewCellEventArgs e)  //*********�ݤ���
        {
            if (e.RowIndex == -1 && e.ColumnIndex > -1)
            {
                g_sOrderDetailField = gvDetail.Columns[e.ColumnIndex].Name;
                ShowDetailData();
                SetSelectRow(gvDetail, "", TableDefine.gsDef_DtlKeyField);
            }
        }

        private void btnDetailAppend_Click(object sender, EventArgs e)  //���դp���s�W����
        {
            if (gvData.Rows.Count == 0 || gvData.CurrentRow == null)
                return;
            
            fDetailData f = new fDetailData();
            try
            {
                f.g_sUpdateType = "APPEND";
                f.g_sformText = btnAppend.Text;
                f.g_sTypeID = gvData.CurrentRow.Cells["ITEM_TYPE_ID"].Value.ToString();
                f.g_sTypeName = gvData.CurrentRow.Cells["ITEM_TYPE_NAME"].Value.ToString();
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

        private void btnDetailModify_Click(object sender, EventArgs e)   //���դp��-�קﳡ��
        {
            if (gvDetail.Rows.Count == 0 || gvDetail.CurrentRow == null)
                return;
            fDetailData f = new fDetailData();
            try
            {
                f.g_sUpdateType = "MODIFY";
                f.g_sformText = btnModify.Text;
                f.dataCurrentRow = gvDetail.CurrentRow;
                f.g_sTypeID = gvData.CurrentRow.Cells["item_type_ID"].Value.ToString();
                f.g_sTypeName = gvData.CurrentRow.Cells["ITEM_TYPE_NAME"].Value.ToString();
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

        private void btnDetailDelete_Click(object sender, EventArgs e)  //���դp��-�R������
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
                     + " where " + TableDefine.gsDef_DtlKeyField + " = '" + sID + "'";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                CopyToDetailHistory(sID);
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