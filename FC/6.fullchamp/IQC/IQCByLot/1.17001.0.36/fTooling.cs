using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using SajetFilter;
using System.Data.OracleClient;
namespace IQCbyLot
{
    public partial class fTooling : Form
    {
        string g_sIniFile = Application.StartupPath + "\\sajet.ini";
       
        public fTooling(string sProgram,string sUserID,int iSampleSize)
        {
            InitializeComponent();
            g_sUserID = sUserID;
            g_iSampleSize = iSampleSize;
            g_sProgram = sProgram;

        }
        string g_sProgram;
        string g_sToolingSNID;
        string g_sUserID;
        int g_iSampleSize;
        private void fTooling_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            GetToolingNo();
            GetIQCTooling();
            editUseCount.Text = g_iSampleSize.ToString();
            SajetInifile sajetInifile1 = new SajetInifile();
            string sToolingNo = sajetInifile1.ReadIniFile(g_sIniFile, g_sProgram, "TOOLING NO", "");
            combToolingNo.SelectedIndex = combToolingNo.Items.IndexOf(sToolingNo);
        }

        private void editToolingNo_TextChanged(object sender, EventArgs e)
        {
            g_sToolingSNID = "0";
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
           
        }
        private void GetToolingNo()
        {
            string sSQL = "SELECT TOOLING_NO FROM SAJET.SYS_TOOLING "
                       + " WHERE ENABLED='Y' "
                       + " ORDER BY TOOLING_NO ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            combToolingNo.Items.Add("");
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                combToolingNo.Items.Add(dsTemp.Tables[0].Rows[i]["TOOLING_NO"].ToString());
            }
        }
        private void GetIQCTooling()
        {
            string sSQL = " SELECT  A.LOT_NO,A.TOOLING_SN_ID,A.MEMO,A.USED_COUNT "
                 + "       ,B.TOOLING_NO,B.TOOLING_NAME ,C.TOOLING_SN,A.CREATE_TIME ,E.EMP_NAME "
                 + "      ,NVL(C.MAX_USED_COUNT,B.MAX_USED_COUNT) MAX_USED_COUNT "
                 + "      ,NVL(C.LIMIT_USED_COUNT,B.LIMIT_USED_COUNT) LIMIT_USED_COUNT "
                 + "      ,A.MEMO "
                 + "      ,NVL(D.USED_COUNT,'0') TOOLING_USED_COUNT "
                 + "   FROM SAJET.G_IQC_TOOLING A "
                 + "       ,SAJET.SYS_TOOLING B "
                 + "       ,SAJET.SYS_TOOLING_SN C "
                 + "　　　　,SAJET.G_TOOLING_SN_STATUS D "
                 + "       ,SAJET.SYS_EMP E "
                 + "  WHERE A.LOT_NO=:LOT_NO "
                 + "    AND A.TOOLING_SN_ID = C.TOOLING_SN_ID "
                 + "    AND C.TOOLING_ID = B.TOOLING_ID "
                 + "    AND A.CREATE_EMP_ID = E.EMP_ID(+) "
                 + "    AND A.TOOLING_SN_ID = D.TOOLING_SN_ID(+) "
                 + "  ORDER BY C.TOOLING_SN ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO",  lblLotNo_show.Text.ToString().Trim() };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            dgvTooling.Rows.Clear();
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dr = dsTemp.Tables[0].Rows[i];
                dgvTooling.Rows.Add();
                dgvTooling.Rows[dgvTooling.Rows.Count - 1].Cells["TOOLING_NO"].Value = dr["TOOLING_NO"].ToString();
                dgvTooling.Rows[dgvTooling.Rows.Count - 1].Cells["CREATE_TIME"].Value = dr["CREATE_TIME"].ToString();
                dgvTooling.Rows[dgvTooling.Rows.Count - 1].Cells["EMP_NAME"].Value = dr["EMP_NAME"].ToString();
                dgvTooling.Rows[dgvTooling.Rows.Count - 1].Cells["TOOLING_SN"].Value = dr["TOOLING_SN"].ToString();
                dgvTooling.Rows[dgvTooling.Rows.Count - 1].Cells["TOOLING_SN_ID"].Value = dr["TOOLING_SN_ID"].ToString();
                dgvTooling.Rows[dgvTooling.Rows.Count - 1].Cells["TOOLING_USED_COUNT"].Value = dr["TOOLING_USED_COUNT"].ToString();
                dgvTooling.Rows[dgvTooling.Rows.Count - 1].Cells["USED_COUNT"].Value = dr["USED_COUNT"].ToString();
                dgvTooling.Rows[dgvTooling.Rows.Count - 1].Cells["MAX_USED_COUNT"].Value = dr["MAX_USED_COUNT"].ToString();
                dgvTooling.Rows[dgvTooling.Rows.Count - 1].Cells["LIMIT_USED_COUNT"].Value = dr["LIMIT_USED_COUNT"].ToString();
                dgvTooling.Rows[dgvTooling.Rows.Count - 1].Cells["MEMO"].Value = dr["MEMO"].ToString();
            }

            if (dsTemp.Tables[0].Rows.Count < 1)
            {
                btnDel.Enabled = false;
            }
            else
            {
                btnDel.Enabled = true;
            }
        }
        private int GetToolingLimitCount(string sToolingSNID,out int iWarningCount)
        {
            iWarningCount = 0;
            try
            {              
                object[][] Params = new object[4][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TTOOLINGSNID", sToolingSNID };
                Params[1] = new object[] { ParameterDirection.Output, OracleType.Int32, "TWARNINGCOUNT", 0 };
                Params[2] = new object[] { ParameterDirection.Output, OracleType.Int32, "TMAXCOUNT", 0 };
                Params[3] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                DataSet dsTemp = ClientUtils.ExecuteProc("SAJET.SJ_TOOLING_GET_STD_COUNT", Params);
                string sResult = dsTemp.Tables[0].Rows[0]["TRES"].ToString();
                if (sResult.Substring(0, 2) != "OK")
                {
                    SajetCommon.Show_Message(sResult, 0);
                    return -9999; 
                }
                iWarningCount = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["TWARNINGCOUNT"].ToString());
                return Convert.ToInt32(dsTemp.Tables[0].Rows[0]["TMAXCOUNT"].ToString());
            }
            catch(Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);
                return -9999; 
            }
        }
        private int GetToolingUsedCount(string sToolingSNID)
        {
            try
            {
                object[][] Params = new object[3][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TTOOLINGSNID", sToolingSNID };
                Params[1] = new object[] { ParameterDirection.Output, OracleType.Int32, "TUSEDCOUNT", 0 };
                Params[2] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                DataSet dsTemp = ClientUtils.ExecuteProc("SAJET.SJ_TOOLING_GET_USED_COUNT", Params);
                string sResult = dsTemp.Tables[0].Rows[0]["TRES"].ToString();
                if (sResult.Substring(0, 2) != "OK")
                {
                    SajetCommon.Show_Message(sResult, 0);
                    return -9999;
                }
                return Convert.ToInt32(dsTemp.Tables[0].Rows[0]["TUSEDCOUNT"].ToString());
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);
                return -9999;
            }
        }

        private void editToolingNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            g_sToolingSNID = "0";
            string sSQL = "";
            string sToolingID = "";
            string sToolingSN = editToolingNo.Text.ToString().Trim();
            DataSet dsTemp = new DataSet();
            try
            {

                try
                {
                    // 檢查治具是否符合
                    sSQL = " SELECT TOOLING_SN_ID,ENABLED "
                          +"   FROM SAJET.SYS_TOOLING_SN "
                          +"  WHERE TOOLING_SN=:TOOLING_SN AND ROWNUM= 1 ";
                    object[][] Params = new object[1][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TOOLING_SN", sToolingSN };
                    dsTemp = ClientUtils.ExecuteSQL(sSQL,Params);
                    

                    if (dsTemp.Tables[0].Rows.Count == 0)
                    {
                        SajetCommon.Show_Message("Tooling SN Error", 0);
                        editToolingNo.Focus();
                        editToolingNo.SelectAll();
                        return;
                    }
                    string sEnabled = dsTemp.Tables[0].Rows[0]["ENABLED"].ToString();
                    if (sEnabled != "Y")
                    {
                        SajetCommon.Show_Message("Tooling SN IS Invalid", 0);
                        editToolingNo.Focus();
                        editToolingNo.SelectAll();
                        return;
                    }
                    sToolingID = dsTemp.Tables[0].Rows[0]["TOOLING_SN_ID"].ToString();
                    int iWarningCount = 0;
                    int iLimitCount = GetToolingLimitCount(sToolingID,out iWarningCount);
                    if (iLimitCount == -9999)
                        return;
                    int iUsedCount = GetToolingUsedCount(sToolingID);
                    if (iUsedCount == -9999)
                        return;
                    if (iLimitCount != 0 && iUsedCount >=iLimitCount)
                    {
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("Tooling SN") + " : " + sToolingSN + Environment.NewLine
                        + SajetCommon.SetLanguage("Limit Used Count") + " : " + iLimitCount.ToString() + Environment.NewLine
                        + SajetCommon.SetLanguage("Max Used Count") + " : " + iWarningCount.ToString() + Environment.NewLine + Environment.NewLine
                        + SajetCommon.SetLanguage("Used Count") + " : " + iUsedCount.ToString(), 0);
                        editToolingNo.SelectAll();
                        return;
                    }
                    if (iWarningCount != 0 && iUsedCount >=iWarningCount)
                    {
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("Tooling SN") + " : " + sToolingSN + Environment.NewLine
                        + SajetCommon.SetLanguage("Limit Used Count") + " : " + iLimitCount.ToString() + Environment.NewLine
                        + SajetCommon.SetLanguage("Max Used Count") + " : " + iWarningCount.ToString() + Environment.NewLine + Environment.NewLine
                        + SajetCommon.SetLanguage("Used Count") + " : " + iUsedCount.ToString(), 3);
                    }

                    // 檢查批號是否已有對應的治具
                    sSQL = "SELECT ENABLED FROM SAJET.G_IQC_TOOLING "
                         + " WHERE LOT_NO=:LOT_NO "
                         + "   AND TOOLING_SN_ID=:TOOLING_SN_ID "
                         + "   AND ROWNUM = 1 ";
                    Params = new object[2][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", lblLotNo_show.Text.ToString().Trim()  };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TOOLING_SN_ID", sToolingID };
                    dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                    if (dsTemp.Tables[0].Rows.Count > 0)
                    {
                        SajetCommon.Show_Message("Tooling SN Duplicate", 0);
                        editToolingNo.Focus();
                        editToolingNo.SelectAll();
                        return;
                    }
                    g_sToolingSNID = sToolingID;
                    editUseCount.Focus();
                    editUseCount.SelectAll();
                }
                catch (Exception ex)
                {
                    editToolingNo.Focus();
                    editToolingNo.SelectAll();
                    SajetCommon.Show_Message(ex.Message, 0);
                }
            }
            finally
            {
               
            }
            
        }
       
        private void btnSearchLot_Click(object sender, EventArgs e)
        {
            string sToolingNo = combToolingNo.Text;
            string sToolingSN = editToolingNo.Text.Trim() + "%";
            string sSQL = "SELECT B.TOOLING_NO,A.TOOLING_SN "
                        + "      ,NVL(A.MAX_USED_COUNT,B.MAX_USED_COUNT) MAX_USED_COUNT "
                        + "      ,NVL(A.LIMIT_USED_COUNT,B.LIMIT_USED_COUNT) LIMIT_USED_COUNT "
                        + "      ,NVL(C.USED_COUNT,0) USED_COUNT "
                        + "    FROM SAJET.SYS_TOOLING_SN a,SAJET.SYS_TOOLING B,SAJET.G_TOOLING_SN_STATUS C   "
                        + " WHERE A.TOOLING_SN LIKE '" + sToolingSN + "' "
                        + "   AND A.TOOLING_SN_ID = C.TOOLING_SN_ID(+) "
                        + "   AND A.ENABLED='Y' "
                        + "   AND A.TOOLING_ID = B.TOOLING_ID ";
            if (!string.IsNullOrEmpty(sToolingNo))
                sSQL = sSQL + " AND B.TOOLING_NO ='" + sToolingNo + "' ";

            sSQL = sSQL + " ORDER BY B.TOOLING_NO,A.TOOLING_SN ";

            fFilter f = new fFilter();
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                editToolingNo.Text = f.dgvData.CurrentRow.Cells["TOOLING_SN"].Value.ToString();
                KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);
                editToolingNo_KeyPress(sender, Key);
            }
        }

        private void editUseCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == 8 || e.KeyChar == 13 || e.KeyChar == 46))
            {
                e.KeyChar = (char)Keys.None;
            }
            if (e.KeyChar != (char)Keys.Return)
                return;

            btnAdd.Focus();         
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (g_sToolingSNID == "0")
            {
                SajetCommon.Show_Message("Please Input Tooling SN", 0);
                editToolingNo.Focus();
                editToolingNo.SelectAll();
                return;
            }
            int iUsedCount = 0;
            try
            {
                iUsedCount = Convert.ToInt32(editUseCount.Text);
            }
            catch
            {
                SajetCommon.Show_Message("Use Count Error", 0);
                editUseCount.Focus();
                editUseCount.SelectAll();
                return;
            }
            editToolingMemo.Text = editToolingMemo.Text.Trim();
            string sMemo = editToolingMemo.Text;
            SajetInifile sajetInifile1 = new SajetInifile();
            string sToolingNo = combToolingNo.Text;
            sajetInifile1.WriteIniFile(g_sIniFile, g_sProgram, "TOOLING NO", sToolingNo);

            string sSQL = " INSERT INTO SAJET.G_IQC_TOOLING "
                 + " (LOT_NO,TOOLING_SN_ID,CREATE_EMP_ID,CREATE_TIME,USED_COUNT,MEMO)"
                 + " VALUES "
                 + " (:LOT_NO,:TOOLING_SN_ID,:CREATE_EMP_ID,SYSDATE,:USED_COUNT,:MEMO )";
            object[][] Params = new object[5][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", lblLotNo_show.Text.ToString().Trim() };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TOOLING_SN_ID", g_sToolingSNID };  //判別按鈕狀態 允收、批退
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CREATE_EMP_ID", g_sUserID };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.Int32, "USED_COUNT", iUsedCount };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MEMO", sMemo };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            GetIQCTooling();
            editToolingMemo.Text = "";
            editToolingNo.Text = "";
            editToolingNo.Focus();
            editUseCount.Text = g_iSampleSize.ToString();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvTooling.Rows.Count == 0 || dgvTooling.CurrentCell == null)
                return;
            string sToolingID = dgvTooling.CurrentRow.Cells["TOOLING_SN_ID"].Value.ToString();
            string sTooling_SN = dgvTooling.CurrentRow.Cells["TOOLING_SN"].Value.ToString();
            string sSQL = "";
            DataSet dsTemp = new DataSet();

            if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Do you want to delete this data ?") + Environment.NewLine
                           + SajetCommon.SetLanguage("Tooling SN") + " : " + sTooling_SN, 2) != DialogResult.Yes)
                return;

            sSQL = " DELETE FROM SAJET.G_IQC_TOOLING "
                 + "  WHERE LOT_NO=:LOT_NO "
                 + "    AND TOOLING_SN_ID=:TOOLING_SN_ID ";
             object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", lblLotNo_show.Text.ToString().Trim() };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TOOLING_SN_ID", sToolingID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL,Params);
            GetIQCTooling();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {

        }

        private void dgvTooling_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}