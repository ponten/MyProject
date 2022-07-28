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
    public partial class fDownLoad : Form
    {
        private MESGridView.Cache memoryCache;
        private MESGridView.Cache memoryCache5;
        private DataTable tempdatatable;
        private DataTable ALLdatatable;
        int TotalCheckBoxes = 0;
        int TotalCheckedCheckBoxes = 0;
        CheckBox HeaderCheckBox = null;
        bool IsHeaderCheckBoxClicked = false;
        public string path = @"C:\SOURCE\BARCODE.txt";
        public bool dupflag = false;

        public fDownLoad()
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
            Check_Privilege();
             BindGridView();


        }
        private void BindGridView()
        {
            gvBarCode.DataSource = null;
            gvBarCode.Rows.Clear();
            //sSQL = "Select 'false'CHKBOX , BARCODE ,FURNACE_NUMBER ,SPEC,WEIGHT,LENGTH,DIAMETER,ORDER_ID,RC_NO,SITE_AREA,ORDER_TIME,ONSITE_TIME,RECEIVED_TIME,OUTPUT_TIME,CREATE_TIME,UPDATE_TIME from "
            //    + TableDefine.gsDef_Table + " order by BARCODE ";
            sSQL = "Select TITLE,FILE_NAME , UPLOAD_FILE,UPDATE_TIME,UPDATE_USERID from SAJET.G_REEL_BARCODE_FILE WHERE 1=1 ";
            if (!string.IsNullOrEmpty(txtTitle.Text.Trim()))
            {
                sSQL =  sSQL + " AND TITLE LIKE '%" + txtTitle.Text + "%'";
            }
            if (!string.IsNullOrEmpty(txtFileName.Text.Trim()))
            {
                sSQL = sSQL + " AND FILE_NAME LIKE '%" + txtFileName.Text + "%'";
            }
            sSQL = sSQL + " order by TITLE,FILE_NAME ";
            //c
            //{
            //    string sFieldName = combFilterField[combFilter.SelectedIndex].ToString();
            //    /*if (combFilterField[combFilter.SelectedIndex].ToString().IndexOf("WORK_ORDER") > -1)
            //        sCondition += " AND " + sFieldName + " = :FILTER";
            //    else*/
            //    sCondition += " AND A." + sFieldName + " LIKE :FILTER || '%'";
            //    Array.Resize(ref Params, Params.Length + 1);
            //    Params[Params.Length - 1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FILTER", editFilter.Text.Trim() };
            //}
            g_sDataSQL = sSQL;
            DataSet dsSearch = new DataSet();
            dsSearch = ClientUtils.ExecuteSQL(sSQL);
            DataTable dt = dsSearch.Tables[0];
            //ALLdatatable.Clear();
            ALLdatatable = dt;
            //    dt = (DataTable)DataSet.dt;
            //dt.Columns[0].DataType = System.Type.GetType("System.Boolean");
            //gvData.DataSource = dt;
            // gvData.DataSource = GetDataSource();
            //dgvSelectAll.DataSource = GetDataSource(dt);

            //TotalCheckBoxes = dgvSelectAll.RowCount;
            //TotalCheckedCheckBoxes = 0;
            if (gvBarCode.ColumnCount == 0)
            {
                DataGridViewCheckBoxColumn dcCheck = new DataGridViewCheckBoxColumn();
                dcCheck.HeaderText = SajetCommon.SetLanguage("Select");
                dcCheck.Width = 40;
                dcCheck.Name = "CHECKED";
                gvBarCode.Columns.Add(dcCheck);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    gvBarCode.Columns.Add(dt.Columns[i].ColumnName, SajetCommon.SetLanguage(dt.Columns[i].ColumnName, 1));
                    
                }
            }
            foreach (DataRow dr in dt.Rows)
            {
                gvBarCode.Rows.Add();
                gvBarCode.Rows[gvBarCode.Rows.Count - 1].Cells[0].Value = false;
                foreach (DataColumn dc in dt.Columns)
                    gvBarCode.Rows[gvBarCode.Rows.Count - 1].Cells[dc.ColumnName].Value = dr[dc.ColumnName].ToString();
               
            }
        }

        //private DataTable GetDataSource(DataTable dt)
        //{
        //    DataTable dTable = new DataTable();

        //    DataRow dRow = null;
        //    DateTime dTime;
        //    Random rnd = new Random();
        //    dTable.Columns.Add("CHKBOX", System.Type.GetType("System.Boolean"));
        //    foreach (DataColumn dc in dt.Columns)
        //    {
        //        dTable.Columns.Add(dc.ColumnName);
        //    }               
              
        //    // //dTable.Columns.Add(SajetCommon.SetLanguage(dc.ColumnName, 1), typeof(string));
        //    for (int n = 0; n < dt.Rows.Count; ++n)
        //    {
        //        dRow = dTable.NewRow();
        //        dTime = DateTime.Now;
        //        dRow["CHKBOX"] = false;
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            foreach (DataColumn dc in dt.Columns)
        //                dRow[dc.ColumnName] = dr[dc.ColumnName].ToString();                            
        //        }
        //        dTable.Rows.Add(dRow);
        //        dTable.AcceptChanges();
        //    }
        //    return dTable;
        //}
        //private DataTable GetDataSource()
        //{
        //    DataTable dTable = new DataTable();

        //    DataRow dRow = null;
        //    DateTime dTime;
        //    Random rnd = new Random();

        //    dTable.Columns.Add("IsChecked", System.Type.GetType("System.Boolean"));
        //    dTable.Columns.Add("RandomNo");
        //    dTable.Columns.Add("Date");
        //    dTable.Columns.Add("Time");

        //    for (int n = 0; n < 10; ++n)
        //    {
        //        dRow = dTable.NewRow();
        //        dTime = DateTime.Now;

        //        dRow["IsChecked"] = "false";
        //        dRow["RandomNo"] = rnd.NextDouble();
        //        dRow["Date"] = dTime.ToString("MM/dd/yyyy");
        //        dRow["Time"] = dTime.ToString("hh:mm:ss tt");

        //        dTable.Rows.Add(dRow);
        //        dTable.AcceptChanges();
        //    }

        //    return dTable;
        //}

        

        private void Check_Privilege()
        {
            string sPrivilege = ClientUtils.GetPrivilege(g_sUserID, g_sFunction, g_sProgram).ToString();

            //btnAppend.Enabled = SajetCommon.CheckEnabled("INSERT", sPrivilege);
            //btnModify.Enabled = SajetCommon.CheckEnabled("UPDATE", sPrivilege);
            //btnDelete.Enabled = SajetCommon.CheckEnabled("DELETE", sPrivilege);   
        }
        CheckBox headerCheckBox = new CheckBox();
        //private void BindGrid()
        //{
        //    sSQL = "Select 'false'CHKBOX ,BARCODE ,FURNACE_NUMBER ,SPEC,WEIGHT,LENGTH,DIAMETER,ORDER_ID,RC_NO,SITE_AREA,ORDER_TIME,ONSITE_TIME,RECEIVED_TIME,OUTPUT_TIME,CREATE_TIME,UPDATE_TIME from "
        //       + TableDefine.gsDef_Table + " order by BARCODE ";

        //    g_sDataSQL = sSQL;
        //    DataSet dsSearch = new DataSet();
        //    dsSearch = ClientUtils.ExecuteSQL(sSQL);
        //    DataTable dt = dsSearch.Tables[0];
        //    //    dt = (DataTable)DataSet.dt;
        //    //dt.Columns[0].DataType = System.Type.GetType("System.Boolean");
        //    //gvData.DataSource = dt;
        //    // gvData.DataSource = GetDataSource();
        //    //dataGridView1.DataSource =dt;

        //    if (dataGridView1.ColumnCount == 0)
        //    {
        //        DataGridViewCheckBoxColumn dcCheck = new DataGridViewCheckBoxColumn();
        //        dcCheck.HeaderText = "Select";// SajetCommon.SetLanguage("Select");
        //        dcCheck.Width = 40;
        //        dcCheck.Name = "CHECKED";
        //        dataGridView1.Columns.Add(dcCheck);
        //        for (int i = 0; i < dsSearch.Tables[0].Columns.Count; i++)
        //        {
        //            //dataGridView1.Columns.Add(dsSearch.Tables[0].Columns[i].ColumnName, SajetCommon.SetLanguage(dsSearch.Tables[0].Columns[i].ColumnName, 1));
        //            dataGridView1.Columns.Add(dsSearch.Tables[0].Columns[i].ColumnName, dsSearch.Tables[0].Columns[i].ColumnName);
        //            //if (i > programInfo.iInputVisible["機台"])
        //            //    gvMachine.Columns[gvMachine.Columns.Count - 1].Visible = false;
        //            //else
        //            //    gvMachine.Columns[gvMachine.Columns.Count - 1].ReadOnly = programInfo.iInputField["機台"].IndexOf(i) == -1;
        //        }
        //    }
        //    foreach (DataRow dr in dsSearch.Tables[0].Rows)
        //    {
        //        dataGridView1.Rows.Add();
        //        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value = false;
        //        foreach (DataColumn dc in dsSearch.Tables[0].Columns)
        //            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[dc.ColumnName].Value = dr[dc.ColumnName].ToString();
        //        //if (!(dr["DEFAULT_STATUS"].ToString() == "Y" || dr["RUN_FLAG"].ToString() == "1"))
        //        //{
        //        //dataGridView1.Rows[dataGridView1.Rows.Count - 1].ReadOnly = true;
        //        //dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Silver;
        //        //}
        //    }
        //}
       
        public void ShowData()
        {
           // sSQL = "Select * from " + TableDefine.gsDef_Table + " order by BARCODE ";
            sSQL = "Select 'false'CHKBOX ,BARCODE ,FURNACE_NUMBER ,SPEC,WEIGHT,LENGTH,DIAMETER,ORDER_ID,RC_NO,SITE_AREA,ORDER_TIME,ONSITE_TIME,RECEIVED_TIME,OUTPUT_TIME,CREATE_TIME,UPDATE_TIME from "
               + TableDefine.gsDef_Table + " order by BARCODE ";
            g_sDataSQL = sSQL;
            DataSet dsSearch = new DataSet();
            dsSearch = ClientUtils.ExecuteSQL(sSQL);
            DataTable dt = dsSearch.Tables[0];
            //    dt = (DataTable)DataSet.dt;
            gvData.DataSource = dt;
            //欄位Title  
            for (int i = 0; i < gvData.Columns.Count; i++)
            {
                gvData.Columns[i].Visible = false;
            }
            for (int i = 0; i < TableDefine.tGridField.Length; i++)
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

        

        private void editFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            BindGridView();
            // ShowData();
            SetSelectRow(gvData, "", TableDefine.gsDef_KeyField);

            txtTitle.Focus();
        }

        

       


       

        private void btnExport_Click(object sender, EventArgs e)
        {

            saveFileDialog1.DefaultExt = "xls";
            saveFileDialog1.Filter = "All Files(*.xls)|*.xls";
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            string sFileName = saveFileDialog1.FileName;

            ExportExcel.CreateExcel Export = new ExportExcel.CreateExcel(sFileName);
            Export.ExportToExcel(gvBarCode);
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

        private void gvBarCode_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            e.Value = memoryCache.RetrieveElement(e.RowIndex, e.ColumnIndex);
        }

        private void MenuHistory_Click(object sender, EventArgs e)
        {
            if (gvBarCode.Rows.Count == 0 || gvBarCode.CurrentRow == null)
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
           
            if (txtFile.Text.Trim().Length > 0)
            {
                string barcodelist = ""; int begindt = 0;

                for (int i = 0; i < gvBarCode.Rows.Count; i++)
                {

                    if (Convert.ToBoolean(gvBarCode.Rows[i].Cells[0].EditedFormattedValue))
                    {
                        string sFile = "";
                        //sFile = Path.Combine(txtFile.Text.Trim().ToString(), gvBarCode.Rows[i].Cells["FILE_NAME"].Value.ToString());
                        //byte[] img = new byte[0];
                        //img = (byte[])gvBarCode.Rows[i].Cells["UPLOAD_FILE"].Value;
                        //File.WriteAllBytes(sFile, img);
                        string sSQL = "Select UPLOAD_FILE "
                + "from SAJET.G_REEL_BARCODE_FILE "
                + "Where FILE_NAME = '" + gvBarCode.Rows[i].Cells["FILE_NAME"].Value + "' "
                 + "AND TITLE = '" + gvBarCode.Rows[i].Cells["TITLE"].Value + "' ";
                        DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, null);
                        if (dsTemp.Tables[0].Rows.Count > 0)
                        {
                            if (txtFile.Text.Trim() != "")
                                sFile = Path.Combine(txtFile.Text.Trim(), gvBarCode.Rows[i].Cells["FILE_NAME"].Value.ToString());
                            else
                                sFile = Path.Combine(Application.StartupPath, gvBarCode.Rows[i].Cells["FILE_NAME"].Value.ToString());
                            //讀blob欄位
                            byte[] img = new byte[1024];
                            DataRow dr = dsTemp.Tables[0].Rows[0];
                            if (!gvBarCode.Rows[i].Cells["FILE_NAME"].Value.ToString().Equals(""))
                            {
                                img = ((byte[])dr["UPLOAD_FILE"]);
                            }
                            File.WriteAllBytes(sFile,img);
                        }
                    }
                }

                DialogResult = DialogResult.OK;
                string sMsg = SajetCommon.SetLanguage("File Download succeed");
                SajetCommon.Show_Message(sMsg, 0);
            }
            else
            {
                string tmp = "";
                tmp = SajetCommon.SetLanguage("Created Folder ,Please");
                SajetCommon.Show_Message(tmp, 0);
                return;
            }



        }

        private void gvBarCode_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex > -1)
            {
                g_sOrderField = gvBarCode.Columns[e.ColumnIndex].Name;
                BindGridView();
                //ShowData();
                SetSelectRow(gvBarCode, "", TableDefine.gsDef_KeyField);


            }
        }

        

    
        private void btnSelectPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();
            path.ShowDialog();
            this.txtFile.Text = path.SelectedPath;
        }

        private void btnOpFile_Click(object sender, EventArgs e)
        {
            //選擇檔案路徑
            //OpenFileDialog file = new OpenFileDialog();
            //file.ShowDialog();
            //this.txtFile.Text = file.SafeFileName;
            OpenFileDialog ofd = new OpenFileDialog(); //不用從工具拉，要用到才NEW
            if (string.IsNullOrEmpty(ofd.InitialDirectory))
                ofd.InitialDirectory = "D:";  // 預設路徑

            ofd.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";//"Text Files (*.txt)|*.c; *.cpp|All Files (*.*)|*.*";  //因為我要選TXT來轉EXCEL，所以預設就用TXT
            ofd.Title = "請開啟文字檔案";

            if (ofd.ShowDialog(this) == DialogResult.Cancel)
                return;
            txtFile.Text = ofd.FileName;
        }

       
        private String getSameRecordsbyColumn(DataTable FirstDataTable, DataTable SecondDataTable, String colname)
        {
            DataTable ResultDataTable = FirstDataTable.Copy();
            String rt = "";int st = 0;
            foreach (DataRow row in ResultDataTable.Rows)
            {
                string D_ID = row[colname].ToString();
                foreach (DataRow row2 in SecondDataTable.Rows)
                {
                    if (row2[colname].ToString() == D_ID)
                    {
                        if (st == 0)
                        {
                            rt += D_ID ;
                        }
                        else
                        {
                            rt += D_ID + ",";
                        }
                        st++;
                       
                    }
                }
            }

            return rt ;
        }
        public bool tablesAreTheSame(DataTable table1, DataTable table2)
        {
            DataTable dt;
            dt = getDifferentRecords(table1, table2);

            if (dt.Rows.Count == 0)
                return true;
            else
                return false;
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            int iCnt = 0;
            bool bCheck = chkSelectAll.Checked;
            if (gvBarCode.Rows.Count > 0) // 檢查是否有勾選
            {
                
                for (int i = 0; i < gvBarCode.Rows.Count; i++)
                {

                    gvBarCode.Rows[i].Cells[0].Value = bCheck;
                }
               
            }
        }

        private void btnqty_Click(object sender, EventArgs e)
        {
            BindGridView();
        }

        //Found at http://canlu.blogspot.com/2009/05/how-to-compare-two-datatables-in-adonet.html
        private DataTable getDifferentRecords(DataTable FirstDataTable, DataTable SecondDataTable)
        {
            //Create Empty Table     
            DataTable ResultDataTable = new DataTable("ResultDataTable");

            //use a Dataset to make use of a DataRelation object     
            using (DataSet ds = new DataSet())
            {
                //Add tables     
                ds.Tables.AddRange(new DataTable[] { FirstDataTable.Copy(), SecondDataTable.Copy() });

                //Get Columns for DataRelation     
                DataColumn[] firstColumns = new DataColumn[ds.Tables[0].Columns.Count];
                for (int i = 0; i < firstColumns.Length; i++)
                {
                    firstColumns[i] = ds.Tables[0].Columns[i];
                }

                DataColumn[] secondColumns = new DataColumn[ds.Tables[1].Columns.Count];
                for (int i = 0; i < secondColumns.Length; i++)
                {
                    secondColumns[i] = ds.Tables[1].Columns[i];
                }

                //Create DataRelation     
                DataRelation r1 = new DataRelation(string.Empty, firstColumns, secondColumns, false);
                ds.Relations.Add(r1);

                DataRelation r2 = new DataRelation(string.Empty, secondColumns, firstColumns, false);
                ds.Relations.Add(r2);

                //Create columns for return table     
                for (int i = 0; i < FirstDataTable.Columns.Count; i++)
                {
                    ResultDataTable.Columns.Add(FirstDataTable.Columns[i].ColumnName, FirstDataTable.Columns[i].DataType);
                }

                //If FirstDataTable Row not in SecondDataTable, Add to ResultDataTable.     
                ResultDataTable.BeginLoadData();
                foreach (DataRow parentrow in ds.Tables[0].Rows)
                {
                    DataRow[] childrows = parentrow.GetChildRows(r1);
                    if (childrows == null || childrows.Length == 0)
                        ResultDataTable.LoadDataRow(parentrow.ItemArray, true);
                }

                //If SecondDataTable Row not in FirstDataTable, Add to ResultDataTable.     
                foreach (DataRow parentrow in ds.Tables[1].Rows)
                {
                    DataRow[] childrows = parentrow.GetChildRows(r2);
                    if (childrows == null || childrows.Length == 0)
                        ResultDataTable.LoadDataRow(parentrow.ItemArray, true);
                }
                ResultDataTable.EndLoadData();
            }

            return ResultDataTable;
        }



    }
}