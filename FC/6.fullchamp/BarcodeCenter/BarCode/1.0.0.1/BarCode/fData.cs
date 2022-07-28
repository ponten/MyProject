using System;
using System.Data;
using System.Data.OracleClient;
using System.Windows.Forms;
using SajetClass;
using SajetTable;
using System.Drawing;

namespace BarCode
{
    public partial class fData : Form
    {
        string g_sDataSQL;
        private MESGridView.Cache memoryCache8;

        public fData()
        {
            InitializeComponent();
        }
        public string g_sUpdateType, g_sformText;
        public string g_sKeyID;
        public DataGridViewRow dataCurrentRow;
        public DataTable tempdatatable;
        public DataGridView dataCurrentdgv;
        public string[] barcode;
        public int barcodelength;

        string sSQL;
        DataSet dsTemp;
        bool bAppendSucess = false;
        public DataSet ds;

       
        private void fData_Load(object sender, EventArgs e)
        {
            if (g_sUpdateType == "APPEND")
            {
                gvBarCode.Visible = false;
                editBARCODE.ReadOnly = false;
                editFURNACENUMBER.ReadOnly = false;
               
            }else
            {
                gvBarCode.Visible = true;
                BindGridView();
                editBARCODE.ReadOnly = true;
                editFURNACENUMBER.ReadOnly = true;
            }
               
            SajetCommon.SetLanguageControl(this);            

            panel2.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.Text = g_sformText;
            if (g_sUpdateType == "MODIFY")
            {
                //panel1.Visible = true;
                g_sKeyID = dataCurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();

                editBARCODE.Text = dataCurrentRow.Cells["BARCODE"].Value.ToString();
                editFURNACENUMBER.Text = dataCurrentRow.Cells["FURNACE_NUMBER"].Value.ToString();
                editSPEC.Text = dataCurrentRow.Cells["SPEC"].Value.ToString();
                if (dataCurrentRow.Cells["WEIGHT"].Value.ToString() == " ")
                    editWEIGHT.Text = "0";
                else
                    editWEIGHT.Text = dataCurrentRow.Cells["WEIGHT"].Value.ToString();
                if (dataCurrentRow.Cells["LENGTH"].Value.ToString() == "")
                    editLENGTH.Text = "0";
                else
                    editLENGTH.Text = dataCurrentRow.Cells["LENGTH"].Value.ToString();
                if (dataCurrentRow.Cells["DIAMETER"].Value.ToString() == " ")
                    editDIAMETER.Text = "0";
                else
                    editDIAMETER.Text = dataCurrentRow.Cells["DIAMETER"].Value.ToString();
                editSITE_AREA.Text = dataCurrentRow.Cells["SITE_AREA"].Value.ToString();
                editORDERID.Text = dataCurrentRow.Cells["ORDER_ID"].Value.ToString();
                dtpORDERTIME.Text = dataCurrentRow.Cells["ORDER_TIME"].Value.ToString();
                dtpONSITETIME.Text = dataCurrentRow.Cells["ONSITE_TIME"].Value.ToString();
                dtpRECEIVEDTIME.Text = dataCurrentRow.Cells["ORDER_TIME"].Value.ToString();
                dtpOUTPUTTIME.Text = dataCurrentRow.Cells["ONSITE_TIME"].Value.ToString();
                //dtpCREATETIME.Text = dataCurrentRow.Cells["ORDER_TIME"].Value.ToString();
                //dtpUPDATETIME.Text = dataCurrentRow.Cells["ONSITE_TIME"].Value.ToString();

            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= lbSpec.Controls.Count - 1; i++)
            {
                if (lbSpec.Controls[i] is TextBox)
                {
                    lbSpec.Controls[i].Text = lbSpec.Controls[i].Text.Trim();
                }
            }

            
           
            try
            {
                if (g_sUpdateType == "APPEND")
                {
                    if (editBARCODE.Text == "")
                    {
                        string sData = lbBarCode.Text;
                        string sMsg = SajetCommon.SetLanguage("PK is null") + Environment.NewLine + sData;
                        SajetCommon.Show_Message(sMsg, 0);
                        editBARCODE.Focus();
                        editBARCODE.SelectAll();
                        return;
                    }
                    sSQL = " select * from "
              + TableDefine.gsDef_Table + " WHERE BARCODE = '" + editBARCODE.Text.Trim() + "'";

                   
                    DataSet dsSearch = new DataSet();
                    dsSearch = ClientUtils.ExecuteSQL(sSQL);
                    DataTable dt = dsSearch.Tables[0];
                    if (dt.Rows.Count == 0)
                    {
                        AppendData();
                        bAppendSucess = true;
                        string sMsg = SajetCommon.SetLanguage("Data APPEND succeed") + " !" + Environment.NewLine + SajetCommon.SetLanguage("continuous") + " !";
                        if (SajetCommon.Show_Message(sMsg, 2) == DialogResult.Yes)
                        {
                            ClearData();

                            editBARCODE.Focus();
                            return;
                        }
                        DialogResult = DialogResult.OK;

                    }else
                    {
                        string sMsg = SajetCommon.SetLanguage("BARCODE duplicates");
                        SajetCommon.Show_Message(sMsg, 0);
                        editBARCODE.Focus();
                        return;
                    }
                    
                }
                else if (g_sUpdateType == "MODIFY")
                {

                    UPddata();
                    DialogResult = DialogResult.OK;
                    string sMsg = SajetCommon.SetLanguage("Data MODIFY succeed");
                    SajetCommon.Show_Message(sMsg, 0);

                }
                else if (g_sUpdateType == "IMPORT")
                {
                    editBARCODE.ReadOnly = true;
                    editFURNACENUMBER.ReadOnly = true;
                   
                    ADDdata();
                    DialogResult = DialogResult.OK;
                    string sMsg = SajetCommon.SetLanguage("Data IMPORT succeed");
                    SajetCommon.Show_Message(sMsg, 0);
                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception : " + ex.Message, 0);
                return;
            }
        }
        private void ADDdata()
        {
            int addcnt = 0;
            string barcodelist = ""; int begindt = 0;

            for (int i = 0; i < gvBarCode.Rows.Count; i++)
            {
                string barcode = gvBarCode.Rows[i].Cells[0].Value.ToString();
                string furnace_number = gvBarCode.Rows[i].Cells[1].Value.ToString();
                
                try
                {
                    AppendData( barcode, furnace_number);
                    addcnt++;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("error:" + ex);
                }
                
            }

           // MessageBox.Show("data load ok, 共load " + addcnt + " 筆資料 ");

        }
        private void UPddata()
        {
            int addcnt = 0;
            string barcodelist = ""; int begindt = 0;

            for (int i = 0; i < gvBarCode.Rows.Count; i++)
            {
                string barcode = gvBarCode.Rows[i].Cells[0].Value.ToString();
                string furnace_number = gvBarCode.Rows[i].Cells[1].Value.ToString();

                try
                {
                    ModifyData(barcode, furnace_number);
                    addcnt++;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("error:" + ex);
                }

            }

            // MessageBox.Show("data load ok, 共load " + addcnt + " 筆資料 ");

        }
        private void AppendData(string barcode,string furnacenm)
        {

            //string sMaxID = SajetCommon.GetMaxID("SAJET.SYS_CUSTOMER", "CUSTOMER_ID", 8);

            object[][] Params = new object[14][];
            sSQL = " Insert into SAJET.SYS_REC_MATERIAL "
                 + " (BARCODE"
                 + " ,FURNACE_NUMBER"
                 + " ,SPEC"
                 + " ,WEIGHT"
                 + " ,LENGTH"
                 + " ,DIAMETER"
                 + " ,SITE_AREA"
                 + " ,ORDER_ID"
                 + " ,ORDER_TIME"
                 + " ,ONSITE_TIME"
                 + " ,RECEIVED_TIME"
                 + " ,OUTPUT_TIME"
                 + " ,CREATE_TIME"
                 + " ,UPDATE_TIME )"
                 + " Values "
                 + " (:BARCODE"
                 + " ,:FURNACE_NUMBER"
                 + " ,:SPEC"
                 + " ,:WEIGHT"
                 + " ,:LENGTH"
                 + " ,:DIAMETER"
                 + ",:SITE_AREA "
                 + " ,:ORDER_ID"
                 + " ,:ORDER_TIME"
                 + " ,:ONSITE_TIME "
                 + " ,:RECEIVED_TIME"
                 + " ,:OUTPUT_TIME"
                 + " ,:CREATE_TIME"
                 + " ,:UPDATE_TIME)";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BARCODE", barcode };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FURNACE_NUMBER", furnacenm };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SPEC", editSPEC.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.Number, "WEIGHT", float.Parse(editWEIGHT.Text) };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.Number, "LENGTH", float.Parse(editLENGTH.Text) };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.Number, "DIAMETER", float.Parse(editDIAMETER.Text) };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ORDER_ID", editORDERID.Text };
            Params[7] = new object[] { ParameterDirection.Input, OracleType.DateTime, "ORDER_TIME", dtpORDERTIME.Value };
            Params[8] = new object[] { ParameterDirection.Input, OracleType.DateTime, "ONSITE_TIME", dtpONSITETIME.Value };
            Params[9] = new object[] { ParameterDirection.Input, OracleType.DateTime, "RECEIVED_TIME", dtpRECEIVEDTIME.Value };
            Params[10] = new object[] { ParameterDirection.Input, OracleType.DateTime, "OUTPUT_TIME", dtpOUTPUTTIME.Value };
            Params[11] = new object[] { ParameterDirection.Input, OracleType.DateTime, "CREATE_TIME", DateTime.Now };
            Params[12] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", DateTime.Now };
            Params[13] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SITE_AREA", editSITE_AREA.Text };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            //fMain.CopyToHistory(sMaxID);
        }
        private void ModifyData(string barcode, string furnacenm)
        {
            object[][] Params = new object[15][];
            editSPEC.ReadOnly = true;
            editWEIGHT.ReadOnly = true;
            sSQL = " Update SAJET.SYS_REC_MATERIAL "
                 + " set SPEC = :SPEC "
                 + "    ,WEIGHT = :WEIGHT "
                 + "    ,LENGTH = :LENGTH "
                 + "    ,DIAMETER = :DIAMETER "
                 // + "    ,RC_NO = :RC_NO "
                 + "    ,SITE_AREA = :SITE_AREA "
                 + "    ,ORDER_ID = :ORDER_ID "
                 + "    ,ORDER_TIME = :ORDER_TIME "
                 + "    ,ONSITE_TIME = :ONSITE_TIME "
                 + "    ,RECEIVED_TIME = :RECEIVED_TIME "
                 + "    ,OUTPUT_TIME = :OUTPUT_TIME "
                 + "    ,CREATE_TIME = :CREATE_TIME "
                 + "    ,UPDATE_TIME = :UPDATE_TIME "
                 + " where BARCODE = :BARCODE "
                 + " AND FURNACE_NUMBER = :FURNACE_NUMBER";

            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BARCODE", barcode };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FURNACE_NUMBER", furnacenm };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SPEC", editSPEC.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.Number, "WEIGHT", float.Parse(editWEIGHT.Text) };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.Number, "LENGTH", float.Parse(editLENGTH.Text) };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.Number, "DIAMETER", float.Parse(editDIAMETER.Text) };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ORDER_ID", editORDERID.Text };
            Params[7] = new object[] { ParameterDirection.Input, OracleType.DateTime, "ORDER_TIME", dtpORDERTIME.Value };
            Params[8] = new object[] { ParameterDirection.Input, OracleType.DateTime, "ONSITE_TIME", dtpONSITETIME.Value };
            Params[9] = new object[] { ParameterDirection.Input, OracleType.DateTime, "RECEIVED_TIME", dtpRECEIVEDTIME.Value };
            Params[10] = new object[] { ParameterDirection.Input, OracleType.DateTime, "OUTPUT_TIME", dtpOUTPUTTIME.Value };
            Params[11] = new object[] { ParameterDirection.Input, OracleType.DateTime, "CREATE_TIME", DateTime.Now };
            Params[12] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", DateTime.Now };
            Params[13] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", editRC_NO.Text };
            Params[14] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SITE_AREA", editSITE_AREA.Text };

            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            //fMain.CopyToHistory(g_sKeyID);
        }
        private void AppendData()
        {

            //string sMaxID = SajetCommon.GetMaxID("SAJET.SYS_CUSTOMER", "CUSTOMER_ID", 8);

            object[][] Params = new object[14][];
            sSQL = " Insert into SAJET.SYS_REC_MATERIAL "
                 + " (BARCODE"
                 + " ,FURNACE_NUMBER"
                 + " ,SPEC"
                 + " ,WEIGHT"
                 + " ,LENGTH"
                 + " , DIAMETER"
                 + "    ,SITE_AREA"
                 + " ,ORDER_ID"
                 + " ,ORDER_TIME"
                 + " ,ONSITE_TIME"
                 + " ,RECEIVED_TIME"
                 + " ,OUTPUT_TIME"
                 + " ,CREATE_TIME"
                 + " ,UPDATE_TIME )"
                 + " Values "
                 + " (:BARCODE"
                 + " ,:FURNACE_NUMBER"
                 + " ,:SPEC"
                 + " ,:WEIGHT"
                 + " ,:LENGTH"
                 + " ,:DIAMETER"
                 + ",:SITE_AREA "
                 + " ,:ORDER_ID"
                 + " ,:ORDER_TIME"
                 + " ,:ONSITE_TIME "
                 + " ,:RECEIVED_TIME"
                 + " ,:OUTPUT_TIME"
                 + " ,:CREATE_TIME"
                 + " ,:UPDATE_TIME)";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BARCODE", editBARCODE.Text };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FURNACE_NUMBER", editFURNACENUMBER.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SPEC", editSPEC.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.Number, "WEIGHT", float.Parse(editWEIGHT.Text) };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.Number, "LENGTH", float.Parse(editLENGTH.Text) };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.Number, "DIAMETER", float.Parse(editDIAMETER.Text) };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ORDER_ID", editORDERID.Text };
            Params[7] = new object[] { ParameterDirection.Input, OracleType.DateTime, "ORDER_TIME", dtpORDERTIME.Value };
            Params[8] = new object[] { ParameterDirection.Input, OracleType.DateTime, "ONSITE_TIME", dtpONSITETIME.Value };
            Params[9] = new object[] { ParameterDirection.Input, OracleType.DateTime, "RECEIVED_TIME", dtpRECEIVEDTIME.Value };
            Params[10] = new object[] { ParameterDirection.Input, OracleType.DateTime, "OUTPUT_TIME", dtpOUTPUTTIME.Value };
            Params[11] = new object[] { ParameterDirection.Input, OracleType.DateTime, "CREATE_TIME", DateTime.Now };
            Params[12] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", DateTime.Now };
            Params[13] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SITE_AREA", editSITE_AREA.Text };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            //fMain.CopyToHistory(sMaxID);
        }
        private void ModifyData()
        {
            object[][] Params = new object[15][];
            editSPEC.ReadOnly = true;
            editWEIGHT.ReadOnly = true;
            sSQL = " Update SAJET.SYS_REC_MATERIAL "
                 + " set SPEC = :SPEC "
                 + "    ,WEIGHT = :WEIGHT "
                 + "    ,LENGTH = :LENGTH "
                 + "    ,DIAMETER = :DIAMETER "
                // + "    ,RC_NO = :RC_NO "
                 + "    ,SITE_AREA = :SITE_AREA "
                 + "    ,ORDER_ID = :ORDER_ID "
                 + "    ,ORDER_TIME = :ORDER_TIME "
                 + "    ,ONSITE_TIME = :ONSITE_TIME "
                 + "    ,RECEIVED_TIME = :RECEIVED_TIME "
                 + "    ,OUTPUT_TIME = :OUTPUT_TIME "
                 + "    ,CREATE_TIME = :CREATE_TIME "
                 + "    ,UPDATE_TIME = :UPDATE_TIME "
                 + " where BARCODE = :BARCODE "
                 + " AND FURNACE_NUMBER = :FURNACE_NUMBER";

            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BARCODE", editBARCODE.Text };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FURNACE_NUMBER", editFURNACENUMBER.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SPEC", editSPEC.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.Number, "WEIGHT", float.Parse(editWEIGHT.Text) };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.Number, "LENGTH", float.Parse(editLENGTH.Text) };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.Number, "DIAMETER", float.Parse(editDIAMETER.Text) };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ORDER_ID", editORDERID.Text };
            Params[7] = new object[] { ParameterDirection.Input, OracleType.DateTime, "ORDER_TIME", dtpORDERTIME.Value };
            Params[8] = new object[] { ParameterDirection.Input, OracleType.DateTime, "ONSITE_TIME", dtpONSITETIME.Value };
            Params[9] = new object[] { ParameterDirection.Input, OracleType.DateTime, "RECEIVED_TIME", dtpRECEIVEDTIME.Value };
            Params[10] = new object[] { ParameterDirection.Input, OracleType.DateTime, "OUTPUT_TIME", dtpOUTPUTTIME.Value };
            Params[11] = new object[] { ParameterDirection.Input, OracleType.DateTime, "CREATE_TIME", DateTime.Now };
            Params[12] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", DateTime.Now };
            Params[13] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", editRC_NO.Text };
            Params[14] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SITE_AREA", editSITE_AREA.Text };

            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            //fMain.CopyToHistory(g_sKeyID);
        }

        private void ModifyAllData()
        {
 
            for (int i = 0; i <= dgvDummy.RowCount - 2; i++)
            {
                object[][] Params = new object[15][];
                //editSPEC.ReadOnly = true;
                //editWEIGHT.ReadOnly = true;
              //  MessageBox.Show(dgv1.Rows[i].Cells[0].Value.ToString());
                sSQL = " Update SAJET.SYS_REC_MATERIAL "
                     + " set SPEC = :SPEC "
                     + "    ,WEIGHT = :WEIGHT "
                     + "    ,LENGTH = :LENGTH "
                     + "    ,DIAMETER = :DIAMETER "
                     //+ "    ,RC_NO = :RC_NO "
                     + "    ,SITE_AREA = :SITE_AREA "
                     + "    ,ORDER_ID = :ORDER_ID "
                     + "    ,ORDER_TIME = :ORDER_TIME "
                     + "    ,ONSITE_TIME = :ONSITE_TIME "
                     + "    ,RECEIVED_TIME = :RECEIVED_TIME "
                     + "    ,OUTPUT_TIME = :OUTPUT_TIME "
                     + "    ,CREATE_TIME = :CREATE_TIME "
                     + "    ,UPDATE_TIME = :UPDATE_TIME "
                     + " where BARCODE = '" + dgvDummy.Rows[i].Cells[0].Value.ToString() + "' ";
                     //+ " AND FURNACE_NUMBER = : ";

                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BARCODE", editBARCODE.Text };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FURNACE_NUMBER", editFURNACENUMBER.Text };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SPEC", editSPEC.Text };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.Number, "WEIGHT", float.Parse(editWEIGHT.Text) };
                Params[4] = new object[] { ParameterDirection.Input, OracleType.Number, "LENGTH", float.Parse(editLENGTH.Text) };
                Params[5] = new object[] { ParameterDirection.Input, OracleType.Number, "DIAMETER", float.Parse(editDIAMETER.Text) };
                Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ORDER_ID", editORDERID.Text };
                Params[7] = new object[] { ParameterDirection.Input, OracleType.DateTime, "ORDER_TIME", dtpORDERTIME.Value };
                Params[8] = new object[] { ParameterDirection.Input, OracleType.DateTime, "ONSITE_TIME", dtpONSITETIME.Value };
                Params[9] = new object[] { ParameterDirection.Input, OracleType.DateTime, "RECEIVED_TIME", dtpRECEIVEDTIME.Value };
                Params[10] = new object[] { ParameterDirection.Input, OracleType.DateTime, "OUTPUT_TIME", dtpOUTPUTTIME.Value };
                Params[11] = new object[] { ParameterDirection.Input, OracleType.DateTime, "CREATE_TIME", DateTime.Now };
                Params[12] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", DateTime.Now };
                Params[13] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", editRC_NO.Text };
                Params[14] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SITE_AREA", editSITE_AREA.Text };

                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            }
            //fMain.CopyToHistory(g_sKeyID);
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (bAppendSucess)
                DialogResult = DialogResult.OK;
        }

        private void rdoAll_CheckedChanged(object sender, EventArgs e)
        {
            //if (rdoAll.Checked == true)
            //{
            //    sSQL = "Select * "
            //           + "from " + TableDefine.gsDef_Table
            //           + "' ORDER BY BARCODE ";

            //    g_sDataSQL = sSQL;
            //    (new MESGridView.DisplayGridView()).GetGridView(dgvDummy, sSQL, out memoryCache8);
            //    //DataSet dsSearch = ClientUtils.ExecuteSQL(sSQL);
            //    //欄位Title  
            //    for (int i = 0; i <= dgvDummy.Columns.Count - 1; i++)
            //    {
            //        dgvDummy.Columns[i].Visible = false;
            //    }
            //    for (int i = 0; i <= TableDefine.tGridField.Length - 1; i++)
            //    {
            //        string sGridField = TableDefine.tGridField[i].sFieldName;

            //        if (dgvDummy.Columns.Contains(sGridField))
            //        {
            //            dgvDummy.Columns[sGridField].HeaderText = TableDefine.tGridField[i].sCaption;
            //            dgvDummy.Columns[sGridField].DisplayIndex = i; //欄位顯示順序
            //            dgvDummy.Columns[sGridField].Visible = true;
            //        }
            //    }
            //    dgvDummy.Focus();
            //}
        }

        private void GetBarCode()
        {

            DataSet dsSearch = new DataSet();
            //sSQL = "Select * "
            //           + "from " + TableDefine.gsDef_Table
            //           + " ORDER BY BARCODE ";
            //sSQL = "Select 'false'CHKBOX ,BARCODE ,FURNACE_NUMBER ,SPEC,WEIGHT,LENGTH,DIAMETER,ORDER_ID,RC_NO,SITE_AREA,ORDER_TIME,ONSITE_TIME,RECEIVED_TIME,OUTPUT_TIME,CREATE_TIME,UPDATE_TIME from "
            //  + TableDefine.gsDef_Table + " order by BARCODE ";
            //g_sDataSQL = sSQL;
            ////            (new MESGridView.DisplayGridView()).GetGridView(dgvDummy, sSQL, out memoryCache8);
            //dsSearch = ClientUtils.ExecuteSQL(sSQL);
            //DataTable dt = dsSearch.Tables[0];
            ////    dt = (DataTable)DataSet.dt;
            //dgvDummy.DataSource = dt;
            ////欄位Title  
            //for (int i = 0; i < dgvDummy.Columns.Count ; i++)
            //{
            //    dgvDummy.Columns[i].Visible = false;
            //}
            //for (int i = 0; i < TableDefine.tGridField.Length ; i++)
            //{
            //    string sGridField = TableDefine.tGridField[i].sFieldName;

            //    if (dgvDummy.Columns.Contains(sGridField))
            //    {
            //        dgvDummy.Columns[sGridField].HeaderText = TableDefine.tGridField[i].sCaption;
            //        dgvDummy.Columns[sGridField].DisplayIndex = i; //欄位顯示順序
            //        dgvDummy.Columns[sGridField].Visible = true;
            //    }
            //}
            //dgvDummy.Focus();
            BindGrid();

        }
        private void BindGridView()
        {
            
            DataTable dt = tempdatatable;
            
            if (gvBarCode.ColumnCount == 0)
            {
                //DataGridViewCheckBoxColumn dcCheck = new DataGridViewCheckBoxColumn();
                //dcCheck.HeaderText = SajetCommon.SetLanguage("Select");
                //dcCheck.Width = 40;
                //dcCheck.Name = "CHECKED";
                //gvBarCode.Columns.Add(dcCheck);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    gvBarCode.Columns.Add(dt.Columns[i].ColumnName, SajetCommon.SetLanguage(dt.Columns[i].ColumnName, 1));

                }
            }
            foreach (DataRow dr in dt.Rows)
            {
                gvBarCode.Rows.Add();
                //gvBarCode.Rows[gvBarCode.Rows.Count - 1].Cells[0].Value = false;
                foreach (DataColumn dc in dt.Columns)
                    gvBarCode.Rows[gvBarCode.Rows.Count - 1].Cells[dc.ColumnName].Value = dr[dc.ColumnName].ToString();

            }
        }
        
        private DataTable GetDataSource(DataTable dt)
        {
            DataTable dTable = new DataTable();

            DataRow dRow = null;
            DateTime dTime;
            Random rnd = new Random();
            //dt.Columns.GetType();
            dTable.Columns.Add("CHKBOX", System.Type.GetType("System.Boolean"));
            //dt.Columns.Add("CHKBOX", System.Type.GetType("System.Boolean"));
            //dTable.Columns.Add("RandomNo");
            //dTable.Columns.Add("Date");
            //dTable.Columns.Add("Time");
            foreach (DataColumn dc in dt.Columns)
                dTable.Columns.Add(dc.ColumnName);
            // dTable.Columns.Add(SajetCommon.SetLanguage(dc.ColumnName, 1), typeof(string));
            for (int n = 0; n < dt.Rows.Count; ++n)
            {
                dRow = dTable.NewRow();
                dTime = DateTime.Now;
                dRow["CHKBOX"] = false;
                foreach (DataRow dr in dt.Rows)
                {                   
                   // dt.Rows[""] = false;
                    foreach (DataColumn dc in dt.Columns)
                        dRow[dc.ColumnName] = dr[dc.ColumnName].ToString();
                    //dRow["RandomNo"] = rnd.NextDouble();
                    //dRow["Date"] = dTime.ToString("MM/dd/yyyy");
                    //dRow["Time"] = dTime.ToString("hh:mm:ss tt");                                       
                }
                dTable.Rows.Add(dRow);
                dTable.AcceptChanges();
            }
                return dTable;
            }
        private DataTable GetDataSource()
        {
            DataTable dTable = new DataTable();

            DataRow dRow = null;
            DateTime dTime;
            Random rnd = new Random();

            dTable.Columns.Add("IsChecked", System.Type.GetType("System.Boolean"));
            dTable.Columns.Add("RandomNo");
            dTable.Columns.Add("Date");
            dTable.Columns.Add("Time");

            for (int n = 0; n < 10; ++n)
            {
                dRow = dTable.NewRow();
                dTime = DateTime.Now;

                dRow["IsChecked"] = "false";
                dRow["RandomNo"] = rnd.NextDouble();
                dRow["Date"] = dTime.ToString("MM/dd/yyyy");
                dRow["Time"] = dTime.ToString("hh:mm:ss tt");

                dTable.Rows.Add(dRow);
                dTable.AcceptChanges();
            }

            return dTable;
        }

        

        private void BindGrid()
        {
            sSQL = "Select  BARCODE ,FURNACE_NUMBER ,SPEC,WEIGHT,LENGTH,DIAMETER,ORDER_ID,RC_NO,SITE_AREA,ORDER_TIME,ONSITE_TIME,RECEIVED_TIME,OUTPUT_TIME,CREATE_TIME,UPDATE_TIME from "
               + TableDefine.gsDef_Table + " order by BARCODE ";

            g_sDataSQL = sSQL;
            DataSet dsSearch = new DataSet();
            dsSearch = ClientUtils.ExecuteSQL(sSQL);
            DataTable dt = dsSearch.Tables[0];
            //    dt = (DataTable)DataSet.dt;
            //dt.Columns[0].DataType = System.Type.GetType("System.Boolean");
            //gvData.DataSource = dt;
            // gvData.DataSource = GetDataSource();
            //dataGridView1.DataSource =dt;

            if (dgvDummy.ColumnCount == 0)
            {
                DataGridViewCheckBoxColumn dcCheck = new DataGridViewCheckBoxColumn();
                dcCheck.HeaderText = "Select";// SajetCommon.SetLanguage("Select");
                dcCheck.Width = 40;
                dcCheck.Name = "CHECKED";
                dgvDummy.Columns.Add(dcCheck);
                for (int i = 0; i < dsSearch.Tables[0].Columns.Count; i++)
                {
                    //dataGridView1.Columns.Add(dsSearch.Tables[0].Columns[i].ColumnName, SajetCommon.SetLanguage(dsSearch.Tables[0].Columns[i].ColumnName, 1));
                    dgvDummy.Columns.Add(dsSearch.Tables[0].Columns[i].ColumnName, dsSearch.Tables[0].Columns[i].ColumnName);
                    //if (i > programInfo.iInputVisible["機台"])
                    //    gvMachine.Columns[gvMachine.Columns.Count - 1].Visible = false;
                    //else
                    //    gvMachine.Columns[gvMachine.Columns.Count - 1].ReadOnly = programInfo.iInputField["機台"].IndexOf(i) == -1;
                }
            }
            foreach (DataRow dr in dsSearch.Tables[0].Rows)
            {
                dgvDummy.Rows.Add();
                dgvDummy.Rows[dgvDummy.Rows.Count - 1].Cells[0].Value = false;
                foreach (DataColumn dc in dsSearch.Tables[0].Columns)
                    dgvDummy.Rows[dgvDummy.Rows.Count - 1].Cells[dc.ColumnName].Value = dr[dc.ColumnName].ToString();
                //if (!(dr["DEFAULT_STATUS"].ToString() == "Y" || dr["RUN_FLAG"].ToString() == "1"))
                //{
                //    dataGridView1.Rows[dataGridView1.Rows.Count - 1].ReadOnly = true;
                //    dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Silver;
                //}
            }
        }
        private void dgv1_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            e.Value = memoryCache8.RetrieveElement(e.RowIndex, e.ColumnIndex);
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

        private void lbOnsiteTime_Click(object sender, EventArgs e)
        {

        }

        private void ClearData()
        {
            for (int i = 0; i <= lbSpec.Controls.Count - 1; i++)
            {
                if (lbSpec.Controls[i] is TextBox)
                {
                    lbSpec.Controls[i].Text = "";
                }
                else if (lbSpec.Controls[i] is ComboBox)
                {
                    ((ComboBox)lbSpec.Controls[i]).SelectedIndex = -1;
                }
            }
        }
    }
}