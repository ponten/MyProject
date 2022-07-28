using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using SajetClass;

namespace IQCbyLot
{
    public partial class fAQL : Form
    {
        public string g_sItemTypeID, g_sItemTypeName, g_sSampleLeveID, g_sSampleID;
        public int g_iLotSize;
        int g_iSampleSize;

        public fAQL()
        {
            InitializeComponent();
        }

        public void initial(String sLotNo, string[] sSamplingLevel,string sSampleType,string sSampleLevel)
        {
            lablLotNo.Text = sLotNo;

            foreach (string str in sSamplingLevel)
                combLevel.Items.Add(str);
            combLevel.SelectedIndex = Convert.ToInt32(sSampleLevel);
            string sSQL="SELECT D.SAMPLING_ID,SAMPLING_TYPE "
                      + "  FROM SAJET.SYS_QC_SAMPLING_PLAN D  "
                      + " Where D.ENABLED ='Y' "
                      + " ORDER BY D.SAMPLING_TYPE  ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);

            for(int i=0;i<=dsTemp.Tables[0].Rows.Count-1;i++)
            {
                combAQL.Items.Add(dsTemp.Tables[0].Rows[i]["SAMPLING_TYPE"].ToString());
                combAQLID.Items.Add(dsTemp.Tables[0].Rows[i]["SAMPLING_ID"].ToString());
            }
            if ( combAQL.Items.IndexOf(sSampleType) >= 0 )
                combAQL.SelectedIndex = combAQL.Items.IndexOf(sSampleType);            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            combAQLID.SelectedIndex = combAQL.SelectedIndex;
            if (g_iSampleSize==-1)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Sample Type") + " :" + combAQL.Text + Environment.NewLine
                                       + SajetCommon.SetLanguage("Level")+" : " + combLevel.Text + Environment.NewLine
                                       + SajetCommon.SetLanguage("Lot Size")+" : " + g_iLotSize.ToString() + Environment.NewLine
                                       + SajetCommon.SetLanguage("Not Define Sample Size"), 0);
                return;
            }
            g_sSampleLeveID = combLevel.SelectedIndex.ToString();
            g_sSampleID = combAQLID.Text;
            /*
            object[][] Params = new object[7][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TLOTNO", lablLotNo.Text };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TITEMTYPEID", g_sItemTypeID };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSAMPLELEVEL", combLevel.SelectedIndex };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSAMPLEID", combAQLID.Text };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSAMPLESIZE", g_iSampleSize.ToString() };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TEMPID", ClientUtils.UserPara1 };
            Params[6] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
            DataSet ds = ClientUtils.ExecuteProc("SAJET.SJ_IQC_UPDATE_SAMPLETYPE", Params);
            if (ds.Tables[0].Rows[0]["TRES"].ToString() != "OK")
            {
                ClientUtils.ShowMessage(ds.Tables[0].Rows[0]["TRES"].ToString(), 0);
                return;
            }
             */ 
            DialogResult = DialogResult.OK;         
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void cmbAQL_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetSamplingDetail();

        }

        private void GetSamplingDetail()
        {
            dataGridView1.Rows.Clear();
            g_iSampleSize =-1;
            string sSQL=" Select C.MIN_LOT_SIZE ,C.MAX_LOT_SIZE  "
                      +"      ,C.SAMPLE_SIZE "
                      + "      ,DECODE(C.SAMPLING_UNIT ,'1','%','Qty') SAMPLING_UNIT "
                      +"     , C.CRITICAL_REJECT_QTY    "
                      +"      , C.MAJOR_REJECT_QTY , C.MINOR_REJECT_QTY " 
                      +"      ,D.SAMPLING_TYPE, D.SAMPLING_ID "
                     
                      +" From SAJET.SYS_QC_SAMPLING_PLAN_DETAIL C "
                      +"     ,SAJET.SYS_QC_SAMPLING_PLAN D "
                      +" Where D.SAMPLING_TYPE = :SAMPLING_TYPE "
                      +"  AND C.SAMPLING_ID = D.SAMPLING_ID "
                      +"  AND C.SAMPLING_LEVEL =:SAMPLING_LEVEL "
                      +" ORDER BY C.MIN_LOT_SIZE ";
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SAMPLING_TYPE", combAQL.Text };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.Int32, "SAMPLING_LEVEL", combLevel.SelectedIndex };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            btnSave.Enabled = (dsTemp.Tables[0].Rows.Count > 0);

            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                int iMin = Convert.ToInt32( dsTemp.Tables[0].Rows[i]["MIN_LOT_SIZE"].ToString());
                int iMax = Convert.ToInt32( dsTemp.Tables[0].Rows[i]["MAX_LOT_SIZE"].ToString());
                if (g_iLotSize >=iMin && g_iLotSize <=iMax)
                    g_iSampleSize = Convert.ToInt32( dsTemp.Tables[0].Rows[i]["SAMPLE_SIZE"].ToString());
                dataGridView1.Rows.Add();
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value = dsTemp.Tables[0].Rows[i][0].ToString();
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[1].Value = dsTemp.Tables[0].Rows[i][1].ToString();
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[2].Value = dsTemp.Tables[0].Rows[i][2].ToString();
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[3].Value = dsTemp.Tables[0].Rows[i][3].ToString();
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[4].Value = dsTemp.Tables[0].Rows[i][4].ToString();
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[5].Value = dsTemp.Tables[0].Rows[i][5].ToString();
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[6].Value = dsTemp.Tables[0].Rows[i][6].ToString();
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[7].Value = dsTemp.Tables[0].Rows[i][7].ToString();
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[8].Value = dsTemp.Tables[0].Rows[i][8].ToString();
            }
        }

        private void fAQL_Load(object sender, EventArgs e)
        {
           // ClientUtils.SetLanguage(this, ClientUtils.fCurrentProject);
            SajetCommon.SetLanguageControl(this);
            lablLotSize.Text = g_iLotSize.ToString();
            lablTypeName.Text =g_sItemTypeName.ToString();
        }
    }
}