using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetFilter;
using System.Data.OracleClient;
using SajetClass;

namespace IQCbyLot
{
    public partial class fCopyRTResult : Form
    {
        string sSQL;
        public string g_sLotNo, g_sTypeName, g_sTypeID;
        string g_sRTID,g_sfLostNo;
        DataSet ds;
        public fCopyRTResult()
        {
            InitializeComponent();
        }

        private void btnFilterRT_Click(object sender, EventArgs e)
        {
            string sRTNO = editRTNo.Text.Trim() + "%";
            string sSQL = "SELECT A.RT_NO,B.VENDOR_CODE,B.VENDOR_NAME "
                +  "  FROM SAJET.G_ERP_RTNO A ,SAJET.SYS_VENDOR B  "
                + " WHERE A.RT_NO like '" + sRTNO + "' "    
                + "   AND A.VENDOR_ID = B.VENDOR_ID(+)"
                + " ORDER BY A.RT_NO ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
              fFilter f = new fFilter();
            f.sSQL = sSQL;

            if (f.ShowDialog() == DialogResult.OK)
            {
                sRTNO = f.dgvData.CurrentRow.Cells["RT_NO"].Value.ToString();
                KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);
                editRTNo.Text = sRTNO;
                editRTNo_KeyPress(sender, Key);
            }
        }

        private void editRTNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            editRTNo.Text = editRTNo.Text.Trim();
            combRTSeq.Items.Clear();
            string sSQL = "SELECT RT_NO FROM SAJET.G_ERP_RTNO WHERE RT_NO=:RT_NO AND ROWNUM = 1 ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RT_NO", editRTNo.Text };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                SajetCommon.Show_Message("RT No Error", 0);
                editRTNo.Focus();
                editRTNo.SelectAll();
                return;
            }

            sSQL = "SELECT C.RT_SEQ "
                       + "  FROM SAJET.G_IQC_LOT A,SAJET.G_ERP_RTNO B,SAJET.G_ERP_RT_ITEM C "
                       + " WHERE A.LOT_NO =:LOT_NO "
                       + "   AND B.RT_NO=:RT_NO "
                       + "   AND C.RT_ID = B.RT_ID "
                       + "   AND C.PART_ID = A.PART_ID "
                       + " ORDER BY A.RT_SEQ ";
            Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", g_sLotNo };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RT_NO", editRTNo.Text };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                string sSeq = dsTemp.Tables[0].Rows[i]["RT_SEQ"].ToString();
                //sSeq = sSeq.PadLeft(2, '0');
                combRTSeq.Items.Add(sSeq);
            }
            if (combRTSeq.Items.Count == 0)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Can not copy from this RT No"), 0);
                return;
            }
            if (combRTSeq.Items.Count > 0)
                combRTSeq.SelectedIndex = 0;                                    
        }
        private string getLotNoInfo(string g_sRTNO,string g_sRTSeq) //取得批號
        {
            sSQL = "SELECT RT_ID FROM SAJET.G_ERP_RTNO "
                 + "WHERE RT_NO = '" + g_sRTNO +"' ";
            ds = ClientUtils.ExecuteSQL(sSQL);
            g_sRTID = ds.Tables[0].Rows[0]["RT_ID"].ToString().Trim();
            sSQL = "SELECT LOT_NO FROM SAJET.G_IQC_LOT "
                 + "WHERE RT_ID = '" + g_sRTID + "' "
                 + "AND   RT_SEQ = '" + combRTSeq.Text.Trim() + "' ";
            ds = ClientUtils.ExecuteSQL(sSQL);
            g_sfLostNo = ds.Tables[0].Rows[0]["LOT_NO"].ToString().Trim();
            return g_sfLostNo;
        }
        private void combRTSeq_SelectedIndexChanged(object sender, EventArgs e)
        {            
            string sLotNo = getLotNoInfo(editRTNo.Text.Trim(), combRTSeq.Text.Trim());
            //SajetCommon.Show_Message(sLotNo, 0);
            //string sLotNo = editRTNo.Text + "-" + combRTSeq.Text;
            GetTestItemValue(sLotNo);
            GetTestItemNoValue(sLotNo);
        }
        private void GetTestItemNoValue(string sLotNo)
        {
            dgvItemNoValue.Rows.Clear();
            sSQL = "SELECT B.ITEM_CODE,B.ITEM_NAME,A.PASS_QTY,A.FAIL_QTY,A.MEMO "
                + "  FROM SAJET.G_IQC_TEST_ITEM A "
                + "      ,SAJET.SYS_TEST_ITEM B "
                + " WHERE A.LOT_NO =:LOT_NO "
                + "   AND A.ITEM_TYPE_ID =:ITEM_TYPE_ID "
                + "   AND A.ITEM_ID = B.ITEM_ID "
                + " ORDER BY B.ITEM_CODE ";
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", sLotNo };
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
                dgvItemNoValue.Rows[dgvItemNoValue.Rows.Count - 1].Cells["ITEM_MEMO"].Value = dr["MEMO"].ToString();
            }
        }
        private void GetTestItemValue(string sLotNo)
        {
            sSQL = "SELECT NVL(MAX(A.ITEM_SEQ),0) MAX_SEQ FROM SAJET.G_IQC_TEST_ITEM_VALUE A "
                + " WHERE A.LOT_NO =:LOT_NO "
                + "   AND A.ITEM_TYPE_ID =:ITEM_TYPE_ID ";
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", sLotNo };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", g_sTypeID };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            int iMaxSeq = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["MAX_SEQ"].ToString());

            for (int i = 1; i <= iMaxSeq; i++)
            {
                dgvItemValue.Columns.Add(i.ToString(), i.ToString());
                dgvItemValue.Columns[i + 1].Width = 45;
            }

            sSQL = "SELECT B.ITEM_CODE,B.ITEM_NAME,A.ITEM_SEQ,A.VALUE "
                + "      ,NVL(A.RESULT,'N/A') RESULT "
                + "  FROM SAJET.G_IQC_TEST_ITEM_VALUE A "
                + "      ,SAJET.SYS_TEST_ITEM B "
                + " WHERE A.LOT_NO =:LOT_NO "
                + "   AND A.ITEM_TYPE_ID =:ITEM_TYPE_ID "
                + "   AND A.ITEM_ID = B.ITEM_ID "
                + " ORDER BY B.ITEM_CODE,A.ITEM_SEQ ";
            Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", sLotNo };
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

        private void fCopyRTResult_Load(object sender, EventArgs e)  //顯示料號資訊
        {
            SajetCommon.SetLanguageControl(this);
            lablTypeName.Text = g_sTypeName;
            string sSQL = "SELECT B.PART_NO "
                       + "  FROM SAJET.G_IQC_LOT A,SAJET.SYS_PART B "
                       + " WHERE A.LOT_NO=:LOT_NO AND A.PART_ID = B.PART_ID(+) "
                       + "   AND ROWNUM = 1 ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", g_sLotNo };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                lablPartNo.Text = dsTemp.Tables[0].Rows[0]["PART_NO"].ToString();
            }
            editRTNo.Focus();
        }

        private void editRTNo_TextChanged(object sender, EventArgs e)
        {
            combRTSeq.Items.Clear();
            dgvItemNoValue.Rows.Clear();
            dgvItemValue.Rows.Clear();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (dgvItemNoValue.Rows.Count == 0 && dgvItemValue.Rows.Count == 0)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Input Copy Source")+" : "+SajetCommon.SetLanguage("RT No"),0);
                editRTNo.Focus();
                editRTNo.SelectAll();
                return;
            }
            //g_sLotNo = editRTNo.Text + "-" + combRTSeq.Text;
            g_sLotNo = g_sfLostNo;
            DialogResult = DialogResult.OK;
        }
    }
}