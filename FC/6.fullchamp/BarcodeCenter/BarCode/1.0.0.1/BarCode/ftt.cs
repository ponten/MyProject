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

namespace BarCode
{
    public partial class ftt : Form
    {
        private MESGridView.Cache memoryCache;
        private MESGridView.Cache memoryCache5;
        
        public string path = @"C:\SOURCE\BARCODE.txt";
        public bool dupflag = false;

        public ftt()
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
            TableDefine.Initial_Table();
            Initial_Form();
            //20170926 FOR EXPORT TO EXCEL
            //GetDummy();
            //
           

            Check_Privilege();
            BindGrid();

            //ShowData();


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
        CheckBox headerCheckBox = new CheckBox();
        private void BindGrid()
        {
            sSQL = "Select BARCODE ,FURNACE_NUMBER ,SPEC,WEIGHT,LENGTH,DIAMETER,ORDER_ID,RC_NO,SITE_AREA,ORDER_TIME,ONSITE_TIME,RECEIVED_TIME,OUTPUT_TIME,CREATE_TIME,UPDATE_TIME from "
               + TableDefine.gsDef_Table + " order by BARCODE ";

            g_sDataSQL = sSQL;
            DataSet dsSearch = new DataSet();
            dsSearch = ClientUtils.ExecuteSQL(sSQL);
            DataTable dt = dsSearch.Tables[0];
            //    dt = (DataTable)DataSet.dt;
            //dt.Columns[0].DataType = System.Type.GetType("System.Boolean");
            //gvData.DataSource = dt;
            // gvData.DataSource = GetDataSource();
            dataGridView1.DataSource =dt;

            //Add a CheckBox Column to the DataGridView Header Cell.

            //Find the Location of Header Cell.
            Point headerCellLocation = this.dataGridView1.GetCellDisplayRectangle(0, -1, true).Location;

            //Place the Header CheckBox in the Location of the Header Cell.
            headerCheckBox.Location = new Point(headerCellLocation.X + 8, headerCellLocation.Y + 2);
            headerCheckBox.BackColor = Color.White;
            headerCheckBox.Size = new Size(18, 18);

            //Assign Click event to the Header CheckBox.
            headerCheckBox.Click += new EventHandler(HeaderCheckBox_Clicked);
            dataGridView1.Controls.Add(headerCheckBox);

            //Add a CheckBox Column to the DataGridView at the first position.
            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.HeaderText = "";
            checkBoxColumn.Width = 30;
            checkBoxColumn.Name = "checkBoxColumn";
            dataGridView1.Columns.Insert(0, checkBoxColumn);

            //Assign Click event to the DataGridView Cell.
            dataGridView1.CellContentClick += new DataGridViewCellEventHandler(DataGridView_CellClick);
        }
        private void HeaderCheckBox_Clicked(object sender, EventArgs e)
        {
            //Necessary to end the edit mode of the Cell.
            dataGridView1.EndEdit();

            //Loop and check and uncheck all row CheckBoxes based on Header Cell CheckBox.
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCheckBoxCell checkBox = (row.Cells["checkBoxColumn"] as DataGridViewCheckBoxCell);
                checkBox.Value = headerCheckBox.Checked;
            }
        }

        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Check to ensure that the row CheckBox is clicked.
            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                //Loop to verify whether all row CheckBoxes are checked or not.
                bool isChecked = true;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["checkBoxColumn"].EditedFormattedValue) == false)
                    {
                        isChecked = false;
                        break;
                    }
                }
                headerCheckBox.Checked = isChecked;
            }
        }
        public void ShowData()
        {
            sSQL = "Select * from " + TableDefine.gsDef_Table + " order by BARCODE ";
           
            g_sDataSQL = sSQL;
            DataSet dsSearch = new DataSet();
            dsSearch = ClientUtils.ExecuteSQL(sSQL);
            DataTable dt = dsSearch.Tables[0];
            //    dt = (DataTable)DataSet.dt;
            gvData.DataSource = dt;
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
                //sSQL = "Select BARCODE from " + TableDefine.gsDef_Table + " order by BARCODE ";
                //DataSet dsSearch = ClientUtils.ExecuteSQL(sSQL);
                //f.dgv1.DataSource = dsSearch;
                ////                DataSet ds = new DataSet();

                ////                g_sDataSQL = sSQL;

                ////                f.dgv1.DataSource = dsSearch; 
                ////                (new MESGridView.DisplayGridView()).GetGridView(f.dgv1, sSQL, out memoryCache5); 
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

        public void TransData()
        {
           

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
                //GridData.CurrentCell = GridData.Rows[iIndex].Cells[sShowField];
                //GridData.Rows[iIndex].Selected = true;
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

            //string sFieldID = gvData.CurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();
            //string sSQL = TableDefine.History_SQL(sFieldID);
            //DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);

            //fHistory fHistory = new fHistory();
            //fHistory.dgvHistory.DataSource = dsTemp;
            //fHistory.dgvHistory.DataMember = dsTemp.Tables[0].ToString();

            ////替換欄位名稱
            //for (int i = 0; i <= fHistory.dgvHistory.Columns.Count - 1; i++)
            //{
            //    string sGridField = fHistory.dgvHistory.Columns[i].HeaderText;
            //    string sField = "";
            //    for (int j = 0; j <= gvData.Columns.Count - 1; j++)
            //    {
            //        sField = gvData.Columns[j].Name;
            //        if (sGridField == sField)
            //        {
            //            fHistory.dgvHistory.Columns[i].HeaderText = gvData.Columns[j].HeaderText;
            //            break;
            //        }
            //    }
            //}

            //fHistory.ShowDialog();
            //fHistory.Dispose();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader(@"C:\SOURCE\BARCODE.txt", Encoding.Default);

            String line;

            while ((line = sr.ReadLine()) != null)
            {
                //MessageBox.Show(line);
                DataGridViewRow dgvr = new DataGridViewRow();
                dgvr.CreateCells(this.dataGridView2);
                for (int intD = 0; intD < this.dataGridView2.Columns.Count; intD++)
                {
                    char[] delimiterChars = { '_', ',' };
                    string[] words = line.Split(delimiterChars);
                    dgvr.Cells[intD].Value = words[intD];
                }
                this.dataGridView2.Rows.Add(dgvr);
            }
            ADDdb();

            ShowData();

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

        private void btnDelAll_Click(object sender, EventArgs e)
        {
            int delcnt = 0;
            StreamReader sr = new StreamReader(@"C:\SOURCE\BARCODE.txt", Encoding.Default);

            String line;

            while ((line = sr.ReadLine()) != null)
            {
                //MessageBox.Show(line);
                DataGridViewRow dgvr = new DataGridViewRow();
                dgvr.CreateCells(this.dataGridView2);
                for (int intD = 0; intD < this.dataGridView2.Columns.Count; intD++)
                {
                    char[] delimiterChars = { '_', ',' };
                    string[] words = line.Split(delimiterChars);
                    dgvr.Cells[intD].Value = words[intD];
                }
                this.dataGridView2.Rows.Add(dgvr);
            }

            for (int i = 0; i <= dataGridView2.RowCount - 2; i++)
            {
                string barcode = dataGridView2.Rows[i].Cells[0].Value.ToString();
                string furnace_number = dataGridView2.Rows[i].Cells[1].Value.ToString();
                // MessageBox.Show("barcode= " + barcode + "   furnace_number = " + furnace_number);
                sSQL = " DELETE FROM SAJET.SYS_REC_MATERIAL "
                 + " WHERE BARCODE = '" + barcode + "'"
                 + " AND FURNACE_NUMBER = '" + furnace_number + "' ";
                ClientUtils.ExecuteSQL(sSQL);
                delcnt++;

            }
            ShowData();
            MessageBox.Show("data DELETE ok,共刪除 : " + delcnt + "筆資料");
        }

        private void ADDdb()
        {
            int addcnt = 0;
            for (int i=0; i<=dataGridView2.RowCount - 2; i++)
            {
                string barcode = dataGridView2.Rows[i].Cells[0].Value.ToString();
                string furnace_number = dataGridView2.Rows[i].Cells[1].Value.ToString();
                CheckDup();
               // MessageBox.Show("barcode= " + barcode + "   furnace_number = " + furnace_number);
                sSQL = " Insert into SAJET.SYS_REC_MATERIAL "
                 + " (BARCODE"
                 + " ,FURNACE_NUMBER)"
                 + " Values "
                 + " ('" + barcode + "'"
                 + " ,'" + furnace_number + "')";
                try
                {
                    ClientUtils.ExecuteSQL(sSQL);
                    addcnt++;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("error:" + ex);
                }
            }
            MessageBox.Show("data load ok, 共load " + addcnt + " 筆資料 ");

        }


        private void CheckDup()
        {

        }
 

 


    }
}