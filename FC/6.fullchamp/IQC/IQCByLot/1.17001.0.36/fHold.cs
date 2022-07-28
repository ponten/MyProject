using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;

namespace IQCbyLot
{
    public partial class fHold : Form
    {
        public string g_sLotNo;
        public string g_sHoldFlag;
        public fHold()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string sSQL = string.Empty;
            editLotMemo.Text = editLotMemo.Text.Trim();
            if (string.IsNullOrEmpty(editLotMemo.Text))
            {
                SajetCommon.Show_Message("Please Input Memo", 0);
                editLotMemo.Focus();
                editLotMemo.SelectAll();
                return;
            }
            if (g_sHoldFlag == "1") //¸Ñ°£¼È°±
            {
                sSQL = "UPDATE SAJET.G_IQC_LOT "
                     + "   SET STATUS='4' "
                     + "      ,HOLD_FLAG ='0' "
                     + "  WHERE LOT_NO ='" + g_sLotNo + "' ";
                DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
                sSQL = "UPDATE SAJET.G_IQC_HOLD_HISTORY "
                    + "  SET RELEASE_TIME = SYSDATE "
                    + "     ,RELEASE_EMPID ='" + ClientUtils.UserPara1 + "' "
                    + "     ,RELEASE_MEMO ='" + editLotMemo.Text.Trim() + "' "
                    + "  WHERE LOT_NO ='" + g_sLotNo + "' "
                    + "    AND RELEASE_TIME IS NULL ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
            }
            else //¼È°±
            {
                sSQL = "UPDATE SAJET.G_IQC_LOT "
                     + "   SET STATUS='5' "
                     + "      ,HOLD_FLAG ='1' "
                     + "      ,HOLD_TIME = SYSDATE "
                     + "  WHERE LOT_NO ='" + g_sLotNo + "' ";
                DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
                sSQL = "INSERT INTO SAJET.G_IQC_HOLD_HISTORY(LOT_NO,HOLD_TIME,HOLD_EMPID,HOLD_MEMO) "
                     + " VALUES "
                     + "('" + g_sLotNo + "',SYSDATE,'" + ClientUtils.UserPara1 + "','" + editLotMemo.Text.Trim() + "') ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
            }
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void fHold_Load(object sender, EventArgs e)
        {

            SajetCommon.SetLanguageControl(this);

            lablLotNo.Text = g_sLotNo;

            if (g_sHoldFlag == "1") //¸Ñ°£¼È°±
            {
                lablStatus.Text = SajetCommon.SetLanguage("Unhold", 1);                 
            }
            else //¼È°±
            {
                lablStatus.Text = SajetCommon.SetLanguage("Hold", 1);
            }

            this.Text = lablStatus.Text + " " + g_sLotNo;
            string sSQL = "SELECT B.EMP_NO HOLD_EMP_NO,B.EMP_NAME HOLD_EMP_NAME,A.HOLD_TIME ,A.HOLD_MEMO "
                + "       ,C.EMP_NO RELEASE_EMP_NO ,C.EMP_NAME RELEASE_EMP_NAME,A.RELEASE_TIME,A.RELEASE_MEMO "
                + "  FROM SAJET.G_IQC_HOLD_HISTORY A "
                + "      ,SAJET.SYS_EMP B "
                + "      ,SAJET.SYS_EMP C "
                + " WHERE A.LOT_NO ='" + g_sLotNo + "' "
                + "   AND A.HOLD_EMPID = B.EMP_ID(+) "
                + "   AND A.RELEASE_EMPID = C.EMP_ID(+) "
                + " ORDER BY A.HOLD_TIME DESC";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            string sMsg = string.Empty;

            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dr = dsTemp.Tables[0].Rows[i];
                string sData = "[" + SajetCommon.SetLanguage("Hold") + "] " + dr["HOLD_TIME"].ToString() + Environment.NewLine
                             + SajetCommon.SetLanguage("Employee") + " : " + dr["HOLD_EMP_NO"].ToString() + " - " + dr["HOLD_EMP_NAME"].ToString() + Environment.NewLine
                             + dr["HOLD_MEMO"].ToString() + Environment.NewLine+ Environment.NewLine

                             + "[" + SajetCommon.SetLanguage("Unhold") + "] " + dr["RELEASE_TIME"].ToString()+Environment.NewLine
                             + SajetCommon.SetLanguage("Employee") + " : " + dr["RELEASE_EMP_NO"].ToString() + " - " + dr["RELEASE_EMP_NAME"].ToString() + Environment.NewLine
                             + dr["RELEASE_MEMO"].ToString() + Environment.NewLine
                             + string.Format("{0,-50}", "===========================================") + Environment.NewLine;
                /*
                string sData1 = dr["HOLD_EMP_NO"].ToString() + " - " + dr["HOLD_EMP_NAME"].ToString() + Environment.NewLine
                              + dr["HOLD_MEMO"].ToString() + Environment.NewLine + Environment.NewLine
                              +SajetCommon.SetLanguage("UnHold") + " : " + Environment.NewLine
                              + dr["RELEASE_EMP_NO"].ToString() + " - " + dr["RELEASE_EMP_NAME"].ToString() + "         " + dr["HOLD_TIME"].ToString() + Environment.NewLine
                              + dr["RELEASE_MEMO"].ToString() + Environment.NewLine + Environment.NewLine;
                 */
                             sMsg = sMsg + sData ;
            }
            editMemoHistory.Text = sMsg;
            
//            sw.WriteLine(string.Format("{0,-30}", DateTime.Now.ToString()) + sMessage);
        }

        private void fHold_Shown(object sender, EventArgs e)
        {
            editLotMemo.Focus();
        }
    }
}