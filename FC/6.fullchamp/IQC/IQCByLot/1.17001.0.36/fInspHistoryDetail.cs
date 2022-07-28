using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Data.OracleClient;

namespace IQCbyLot
{
    public partial class fInspHistoryDetail : Form
    {
        public string g_sLotNo;
        public string g_sTypeID;
        public string g_sTypeName;
        string sSQL;
        
        public fInspHistoryDetail()
        {
            InitializeComponent();
        }

        private void fInspHistoryDetail_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            lablLotNo.Text = g_sLotNo;
            lablTypeName.Text = g_sTypeName;
            GetTestItemValue();
            GetTestItemNoValue();
            GetTestTypeDefect();
        }

        private void GetTestTypeDefect()
        {
            dgvDefect.Rows.Clear();
            sSQL = "SELECT B.DEFECT_CODE,B.DEFECT_DESC,B.DEFECT_DESC2 "
                + "      ,A.DEFECT_QTY,A.DEFECT_MEMO "
                + "      ,DECODE(A.DEFECT_LEVEL,'0','Critical','1','Major','2','Minor','N/A') DEFECT_LEVEL "
                + " FROM SAJET.G_IQC_TEST_TYPE_DEFECT A "
                + "     ,SAJET.SYS_DEFECT B "
                + " WHERE A.LOT_NO =:LOT_NO "
                + "   AND A.ITEM_TYPE_ID =:ITEM_TYPE_ID "
                + "   AND A.DEFECT_ID = B.DEFECT_ID "
                + " ORDER BY B.DEFECT_CODE ";
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", g_sLotNo };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", g_sTypeID };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dr = dsTemp.Tables[0].Rows[i];
                dgvDefect.Rows.Add();
                dgvDefect.Rows[dgvDefect.Rows.Count - 1].Cells["DEFECT_CODE"].Value = dr["DEFECT_CODE"].ToString();
                dgvDefect.Rows[dgvDefect.Rows.Count - 1].Cells["DEFECT_DESC"].Value = dr["DEFECT_DESC"].ToString();
                dgvDefect.Rows[dgvDefect.Rows.Count - 1].Cells["DEFECT_DESC2"].Value = dr["DEFECT_DESC2"].ToString();
                dgvDefect.Rows[dgvDefect.Rows.Count - 1].Cells["DEFECT_LEVEL"].Value =SajetCommon.SetLanguage(dr["DEFECT_LEVEL"].ToString());
                dgvDefect.Rows[dgvDefect.Rows.Count - 1].Cells["DEFECT_QTY"].Value = dr["DEFECT_QTY"].ToString();
                dgvDefect.Rows[dgvDefect.Rows.Count - 1].Cells["DEFECT_MEMO"].Value = dr["DEFECT_MEMO"].ToString();
            }      
        }

        private void GetTestItemNoValue()
        {
            dgvItemNoValue.Rows.Clear();
            sSQL = "SELECT B.ITEM_CODE,B.ITEM_NAME,A.PASS_QTY,A.FAIL_QTY "
                + "  FROM SAJET.G_IQC_TEST_ITEM A "
                + "      ,SAJET.SYS_TEST_ITEM B "
                + " WHERE A.LOT_NO =:LOT_NO "
                + "   AND A.ITEM_TYPE_ID =:ITEM_TYPE_ID "
                + "   AND A.ITEM_ID = B.ITEM_ID "
                + " ORDER BY B.ITEM_CODE ";
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", g_sLotNo };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", g_sTypeID };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dr = dsTemp.Tables[0].Rows[i];
                dgvItemNoValue.Rows.Add();
                dgvItemNoValue.Rows[dgvItemNoValue.Rows.Count - 1].Cells["ITEM_CODE1"].Value = dr["ITEM_CODE"].ToString();
                dgvItemNoValue.Rows[dgvItemNoValue.Rows.Count - 1].Cells["ITEM_NAME1"].Value = dr["ITEM_NAME"].ToString();
                dgvItemNoValue.Rows[dgvItemNoValue.Rows.Count - 1].Cells["PASS_QTY"].Value = dr["PASS_QTY"].ToString();
                dgvItemNoValue.Rows[dgvItemNoValue.Rows.Count - 1].Cells["FAIL_QTY"].Value = dr["FAIL_QTY"].ToString();
            }
        }

        private void GetTestItemValue()
        {
            sSQL = "SELECT NVL(MAX(A.ITEM_SEQ),0) MAX_SEQ FROM SAJET.G_IQC_TEST_ITEM_VALUE A "
                + " WHERE A.LOT_NO =:LOT_NO "
                + "   AND A.ITEM_TYPE_ID =:ITEM_TYPE_ID ";
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", g_sLotNo };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", g_sTypeID };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            int iMaxSeq = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["MAX_SEQ"].ToString());

            for (int i = 1; i <= iMaxSeq; i++)
            {
                dgvItemValue.Columns.Add(i.ToString(), i.ToString());
                dgvItemValue.Columns[i + 1].Width = 45;
            }

                sSQL = "SELECT B.ITEM_CODE,B.ITEM_NAME,A.ITEM_SEQ,A.VALUE "
                    +  "      ,NVL(A.RESULT,'N/A') RESULT "
                    + "  FROM SAJET.G_IQC_TEST_ITEM_VALUE A "
                    + "      ,SAJET.SYS_TEST_ITEM B "
                    + " WHERE A.LOT_NO =:LOT_NO "
                    + "   AND A.ITEM_TYPE_ID =:ITEM_TYPE_ID "
                    + "   AND A.ITEM_ID = B.ITEM_ID "
                    + " ORDER BY B.ITEM_CODE,A.ITEM_SEQ ";
            Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", g_sLotNo };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", g_sTypeID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            ComboBox combItemCode = new ComboBox();

            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dr = dsTemp.Tables[0].Rows[i];

                if (combItemCode.Items.IndexOf(dr["ITEM_CODE"].ToString()) < 0)
                {
                    dgvItemValue.Rows.Add();
                    dgvItemValue.Rows[dgvItemValue.Rows.Count - 1].Cells["ITEM_CODE"].Value = dr["ITEM_CODE"].ToString();
                    dgvItemValue.Rows[dgvItemValue.Rows.Count - 1].Cells["ITEM_NAME"].Value = dr["ITEM_NAME"].ToString();
                    combItemCode.Items.Add(dr["ITEM_CODE"].ToString());
                }
                dgvItemValue.Rows[dgvItemValue.Rows.Count - 1].Cells[dr["ITEM_SEQ"].ToString()].Value = dr["VALUE"].ToString();
                if (dr["RESULT"].ToString() == "1")
                {
                    dgvItemValue.Rows[dgvItemValue.Rows.Count - 1].Cells[dr["ITEM_SEQ"].ToString()].Style.BackColor = Color.Red;
                    dgvItemValue.Rows[dgvItemValue.Rows.Count - 1].Cells[dr["ITEM_SEQ"].ToString()].Style.ForeColor = Color.White;
                    
                }
            }
        }
    }
}