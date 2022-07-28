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
    public partial class fShowRT : Form
    {
        public fShowRT()
        {
            InitializeComponent();
        }

        public string g_sLot;

        private void fShowRT_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            lablLotNo.Text = g_sLot;
            getRTData();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void getRTData()
        {
            dgRT.Rows.Clear();
            object[][] Params;
            DataSet ds;
//            string sSQL = @"SELECT A.RT_NO,A.PO_NO,INCOMING_QTY,B.RT_SEQ,B.RECEIVE_QTY,B.REJECT_QTY
//                            FROM SAJET.G_ERP_RTNO A, SAJET.G_ERP_RT_ITEM B
//                            WHERE A.RT_ID=B.RT_ID AND B.LOT_NO = :LOT_NO
//                            ORDER BY A.RT_NO";
            string sSQL = @"SELECT A.RT_NO,A.PO_NO,INCOMING_QTY,B.RT_SEQ
                            FROM SAJET.G_ERP_RTNO A, SAJET.G_ERP_RT_ITEM B
                            WHERE A.RT_ID=B.RT_ID AND B.LOT_NO = :LOT_NO
                            ORDER BY A.RT_NO";
            Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", g_sLot };
            ds = ClientUtils.ExecuteSQL(sSQL, Params);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dgRT.Rows.Add();
                dgRT.Rows[dgRT.Rows.Count - 1].Cells[0].Value = dr["RT_NO"].ToString();
                dgRT.Rows[dgRT.Rows.Count - 1].Cells[1].Value = dr["PO_NO"].ToString();
                dgRT.Rows[dgRT.Rows.Count - 1].Cells[2].Value = dr["INCOMING_QTY"].ToString();
                dgRT.Rows[dgRT.Rows.Count - 1].Cells[3].Value = dr["RT_SEQ"].ToString();
                //dgRT.Rows[dgRT.Rows.Count - 1].Cells[4].Value = dr["RECEIVE_QTY"].ToString();
                //dgRT.Rows[dgRT.Rows.Count - 1].Cells[5].Value = dr["REJECT_QTY"].ToString();
            }
        }
    }
}
