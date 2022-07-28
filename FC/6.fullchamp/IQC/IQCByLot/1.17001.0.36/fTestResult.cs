using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using SajetClass;
using SajetFilter;


namespace IQCbyLot
{
    public partial class fTestResult : Form
    {
        public fTestResult()
        {
            InitializeComponent();
        }
        public string g_sProgram;
        public string g_sItemTypeID, g_sItemTypeName, g_sLot, g_sUserID, g_sSampleID, g_sSampleType, g_sSampleLevel, g_sSRECID, g_sPARTTYPE, g_sFlag;
        public int g_iLotSize, g_sSampleLevelID,g_sSampleSize;
        string sSQL;
        DataSet dsTemp;
        string g_sDefectPrefix;

        public struct TDefect
        {
            public string sDefectID;
            public string sDefectCode;
            public string sDefectLevel;
            public string sDefectDesc;
        }

        private void fTestResult_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            lablLotNo.Text = g_sLot;
            lablLotSize.Text = g_iLotSize.ToString();
            lablTypeName.Text = g_sItemTypeName.ToString();
            labSampleType.Text = g_sSampleType;
            labLevel.Text = g_sSampleLevel;
            labSampleSize.Text = g_sSampleSize.ToString();
            GetTestTypeDefect(g_sLot, g_sItemTypeID);
            GetTestTypeMemo(g_sLot, g_sItemTypeID);

        }

        private void editDefectCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            editDefectCode.Text = editDefectCode.Text.Trim();
            TDefect rDefect = CheckDefect(editDefectCode.Text);

            if (rDefect.sDefectID == "0")
            {
                SajetCommon.Show_Message("Defect Code Error", 0);
                editDefectCode.Focus();
                editDefectCode.SelectAll();
                return;
            }

            editDefectQty.Focus();
            editDefectQty.SelectAll();
        }

        private TDefect CheckDefect(string sDefectCode)
        {
            sSQL = " SELECT DEFECT_ID,DEFECT_DESC,DEFECT_LEVEL,DEFECT_CODE "
                 + "   FROM SAJET.SYS_DEFECT "
                 + "  WHERE DEFECT_CODE =:DEFECT_CODE "
                 + "    AND ROWNUM = 1 ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DEFECT_CODE", sDefectCode };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            TDefect rDefect = new TDefect();
            rDefect.sDefectID = "0";

            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsTemp.Tables[0].Rows[0];
                rDefect.sDefectID = dr["DEFECT_ID"].ToString();
                rDefect.sDefectDesc = dr["DEFECT_DESC"].ToString();
                rDefect.sDefectLevel = dr["DEFECT_LEVEL"].ToString();
                rDefect.sDefectCode = dr["DEFECT_CODE"].ToString();
            }
            return rDefect;
        }

        private void btnDefectFilter_Click(object sender, EventArgs e) //不良代碼
        {
            string sDefectCode = editDefectCode.Text.Trim() + "%";
            sSQL = "SELECT * FROM SAJET.SYS_IQC_PARTTYPE_DEFECT "
                + " WHERE PART_TYPE ='" + g_sPARTTYPE + "' "
                + "   AND ROWNUM = 1 ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                sSQL = " Select A.DEFECT_CODE,A.DEFECT_DESC,A.DEFECT_DESC2  "
                     + "  From SAJET.SYS_DEFECT A ,SAJET.SYS_IQC_PARTTYPE_DEFECT B "
                     + " WHERE A.ENABLED='Y' "
                     //+ "   AND A.DEFECT_CODE LIKE '" + sDefectCode + "' "
                     + "   AND B.PART_TYPE ='" + g_sPARTTYPE + "' "
                     + "   AND A.DEFECT_CODE LIKE B.DEFECT_CODE_PREFIX||'%' "
                     + " ORDER BY A.DEFECT_CODE ";
            }
            else
            {
                sSQL = "SELECT * FROM SAJET.SYS_IQC_PARTTYPE_DEFECT "
                     + " WHERE PART_TYPE ='ALL' "
                     + "   AND ROWNUM = 1 ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows.Count > 0)
                {

                    sSQL = " Select A.DEFECT_CODE,A.DEFECT_DESC ,A.DEFECT_DESC2 "
                         + "  From SAJET.SYS_DEFECT A ,SAJET.SYS_IQC_PARTTYPE_DEFECT B "
                         + " WHERE A.ENABLED='Y' "
                        //+ "   AND A.DEFECT_CODE LIKE '" + sDefectCode + "' "
                         + "   AND B.PART_TYPE ='ALL' "
                         + "   AND A.DEFECT_CODE LIKE B.DEFECT_CODE_PREFIX||'%' "
                         + " ORDER BY A.DEFECT_CODE ";
                }
                else
                {
                    sSQL = " Select A.DEFECT_CODE,A.DEFECT_DESC ,A.DEFECT_DESC2 "
                         + "  From SAJET.SYS_DEFECT A  "
                         + " WHERE A.ENABLED='Y' "
                         + "   AND A.DEFECT_CODE LIKE '" + sDefectCode + "' "
                         + " ORDER BY A.DEFECT_CODE ";
                }

            }
            fFilter f = new fFilter();
            f.sSQL = sSQL;

            if (f.ShowDialog() == DialogResult.OK)
            {
                editDefectCode.Text = f.dgvData.CurrentRow.Cells["DEFECT_CODE"].Value.ToString();
                KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);
                editDefectCode_KeyPress(sender, Key);
            }
        }

        private void editDefectQty_KeyPress(object sender, KeyPressEventArgs e) //不良數量
        {
            if (!((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == 8 || e.KeyChar == 13 || e.KeyChar == 46))
            {
                e.KeyChar = (char)Keys.None;
            }

            if (e.KeyChar != (char)Keys.Return)
                return;

            editDefectMemo.Focus();
            editDefectMemo.SelectAll();
        }

        private void editDefectMemo_KeyPress(object sender, KeyPressEventArgs e)//備註
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            sbtnSave.Focus();
        }

        private void sbtnSave_Click(object sender, EventArgs e) //不良資料儲存按鈕
        {
            //check defect duplicate
            if (g_sItemTypeID == "0")
            {
                return;
            }
            editDefectCode.Text = editDefectCode.Text.Trim();

            if (string.IsNullOrEmpty(editDefectCode.Text))
            {
                SajetCommon.Show_Message("Please Input Defect Code", 0);
                editDefectCode.Focus();
                editDefectCode.SelectAll();
                return;
            }

            editDefectQty.Text = editDefectQty.Text.Trim();

            if (string.IsNullOrEmpty(editDefectQty.Text))
            {
                SajetCommon.Show_Message("Please Input Defect Qty", 0);
                editDefectQty.Focus();
                editDefectQty.SelectAll();
                return;
            }

            //------------MAX-20110915-測試大項的不良數量不能大於批量-----------------
            int sg_iLotSize = Convert.ToInt32(g_iLotSize.ToString());
            int sg_iDefectQty = Convert.ToInt32(editDefectQty.Text);
            if (sg_iDefectQty > sg_iLotSize)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Defect Qty can not over Lot Size"), 0);
                editDefectQty.Focus();
                editDefectQty.SelectAll();
                return;
            }
            //-------------------------------------------------------------------------

            TDefect rDefect = CheckDefect(editDefectCode.Text.Trim());

            if (rDefect.sDefectID == "0")
            {
                SajetCommon.Show_Message("Defect Code Error", 0);
                editDefectCode.Focus();
                editDefectCode.SelectAll();
                return;
            }

            string sDefectID = rDefect.sDefectID;
            string sDefectLevel = rDefect.sDefectLevel;
            int iDefectQty = 0;

            try
            {
                iDefectQty = Convert.ToInt32(editDefectQty.Text);
            }
            catch
            {
                SajetCommon.Show_Message("Defect Qty Error", 0);
                editDefectQty.Focus();
                editDefectQty.SelectAll();
                return;
            }

            int iSampleID = Convert.ToInt32(g_sSampleID);
            int iSampleLevel = g_sSampleLevelID;
            int iSampleSize = g_sSampleSize;

            string sMemo = editDefectMemo.Text.Trim();
            object[][] Params = new object[11][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TLOTNO", g_sLot };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TITEMTYPEID", g_sItemTypeID };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TDEFECTID", sDefectID };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TDEFECTLEVEL", sDefectLevel };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.Int32, "TDEFECTQTY", iDefectQty };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TDEFECTMEMO", sMemo };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.Int32, "TEMPID", g_sUserID };
            Params[7] = new object[] { ParameterDirection.Input, OracleType.Int32, "TSAMPLEID", iSampleID };
            Params[8] = new object[] { ParameterDirection.Input, OracleType.Int32, "TSAMPLELEVEL", iSampleLevel };
            Params[9] = new object[] { ParameterDirection.Input, OracleType.Int32, "TSAMPLESIZE", iSampleSize };
            Params[10] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
            dsTemp = ClientUtils.ExecuteProc("SAJET.SJ_IQC_INPUT_DEFECT", Params);
            string sResult = dsTemp.Tables[0].Rows[0]["TRES"].ToString();

            if (sResult != "OK")
            {
                SajetCommon.Show_Message(sResult, 0);
                editDefectCode.Focus();
                editDefectCode.SelectAll();
                return;
            }
            editDefectCode.Text = "";
            editDefectQty.Text = "";
            editDefectMemo.Text = "";
            editDefectCode.Focus();
            editDefectCode.SelectAll();
            GetTestTypeDefect(g_sLot, g_sItemTypeID);
        }

        private void GetTestTypeMemo(string sLotNo, string sTypeID)
        {
            editTestTypeMemo.Text = "";
            sSQL = "SELECT A.MEMO"
                + "  FROM SAJET.G_IQC_TEST_TYPE A "
                + " WHERE A.LOT_NO =:LOT_NO "
                + "   AND A.ITEM_TYPE_ID =:ITEM_TYPE_ID ";
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", sLotNo };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", sTypeID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                editTestTypeMemo.Text = dsTemp.Tables[0].Rows[0]["MEMO"].ToString();
            }
        }

        private void GetTestTypeDefect(string sLotNo, string sTypeID)
        {
            dgvDefect.Rows.Clear();
            sSQL = "SELECT A.DEFECT_ID,B.DEFECT_CODE,B.DEFECT_DESC,A.DEFECT_QTY,A.DEFECT_MEMO "
                + "  FROM SAJET.G_IQC_TEST_TYPE_DEFECT A "
                + "      ,SAJET.SYS_DEFECT B"
                + " WHERE A.LOT_NO =:LOT_NO "
                + "   AND A.ITEM_TYPE_ID =:ITEM_TYPE_ID "
                + "   AND A.DEFECT_ID = B.DEFECT_ID(+) "
                + " ORDER BY B.DEFECT_CODE ";
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", sLotNo };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", sTypeID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dr = dsTemp.Tables[0].Rows[i];
                dgvDefect.Rows.Add();
                dgvDefect.Rows[dgvDefect.Rows.Count - 1].Cells["DEFECT_CODE"].Value = dr["DEFECT_CODE"].ToString();
                dgvDefect.Rows[dgvDefect.Rows.Count - 1].Cells["DEFECT_DESC"].Value = dr["DEFECT_DESC"].ToString();
                dgvDefect.Rows[dgvDefect.Rows.Count - 1].Cells["DEFECT_QTY"].Value = dr["DEFECT_QTY"].ToString();
                dgvDefect.Rows[dgvDefect.Rows.Count - 1].Cells["DEFECT_MEMO"].Value = dr["DEFECT_MEMO"].ToString();
                dgvDefect.Rows[dgvDefect.Rows.Count - 1].Cells["DEFECT_ID"].Value = dr["DEFECT_ID"].ToString();
            }
        }

        private void editPassQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == 8 || e.KeyChar == 13 || e.KeyChar == 46))
            {
                e.KeyChar = (char)Keys.None;
            }

            if (e.KeyChar != (char)Keys.Return)
                return;
            int iPassQty = 0;
            try
            {
                iPassQty = Convert.ToInt32(editPassQty.Text);
            }
            catch
            {
                return;
            }
            int iSampleSize = 0;
            int iLotSize =0;
            try
            {
                iSampleSize = Convert.ToInt32(labSampleSize.Text);
                iLotSize = Convert.ToInt32(lablLotSize.Text);
            }
            catch
            {
                return;
            }

            //先用最小檢驗數去減
            int iDiff = iSampleSize - iPassQty;
            if (iDiff < 0)
            {
                iDiff = iLotSize - iPassQty;
            }
            if (iDiff < 0)
            {
                iDiff = 0;
            }
            editFailQty.Text = iDiff.ToString();
            editFailQty.Focus();
            editFailQty.SelectAll();
        }

        private void editFailQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == 8 || e.KeyChar == 13 || e.KeyChar == 46))
            {
                e.KeyChar = (char)Keys.None;
            }
            if (e.KeyChar != (char)Keys.Return)
                return;
            btnOK.Focus();
        }

        private int GetSysBaseMinInspQty()
        {
            sSQL = "SELECT NVL(PARAM_VALUE,0) PARAM_VALUE "
                + "  FROM SAJET.SYS_BASE "
                + " WHERE PARAM_NAME ='Test Item Input Edit Qty' "
                + "   AND PROGRAM=:PROGRAM";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROGRAM", g_sProgram };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count == 0)
                return 0;
            else
                return Convert.ToInt32(dsTemp.Tables[0].Rows[0]["PARAM_VALUE"].ToString());
        }
        private void btnOK_Click(object sender, EventArgs e) //測試大項檢驗結果
        {
            if (g_sItemTypeID == "0")
            {
                return;
            }

            int iTag = Convert.ToInt32((sender as Button).Tag.ToString());
            string sQCResult = iTag.ToString();
            int iLotSize =g_iLotSize;
            int iPassQty = 0;
            int iFailQty = 0;

            try
            {
                iPassQty = Convert.ToInt32(editPassQty.Text);
            }
            catch
            {
                SajetCommon.Show_Message("Pass Qty Error", 0);
                editPassQty.Focus();
                editPassQty.SelectAll();
                return;
            }

            try
            {
                iFailQty = Convert.ToInt32(editFailQty.Text);
            }
            catch
            {
                SajetCommon.Show_Message("Fail Qty Error", 0);
                editFailQty.Focus();
                editFailQty.SelectAll();
                return;
            }

            if (iPassQty + iFailQty > iLotSize)
            {
                SajetCommon.Show_Message("Pass Qty + Fail Qty more than Lot Size", 0);
                editPassQty.Focus();
                editPassQty.SelectAll();
                return;
            }

            //有不良資料,不良品數必須大於零

            if (dgvDefect.Rows.Count > 0 && iFailQty == 0)
            {
                SajetCommon.Show_Message("There are defect data. Fail Qty must more than 0", 0);
                editFailQty.Focus();
                editFailQty.SelectAll();
                return;
            }

            //不良品數大於零時,最少要有一筆不良資料

            if (iFailQty > 0 && dgvDefect.Rows.Count == 0)
            {
                SajetCommon.Show_Message("Fail Qty more than 0 ,but there is not defect data", 0);
                editFailQty.Focus();
                editFailQty.SelectAll();
                return;
            }
            if (dgvDefect.Rows.Count > 0 && iTag == 0)
            {
                if (SajetCommon.Show_Message(SajetCommon.SetLanguage("There are Defect data") + Environment.NewLine
                                            + SajetCommon.SetLanguage("Sure Confirm this Test Type to Good") + " ? ", 2) != DialogResult.Yes)
                {
                    return;
                }
            }
            if (dgvDefect.Rows.Count == 0 && iTag == 1)
            {
                if (SajetCommon.Show_Message(SajetCommon.SetLanguage("There is not Defect data") + Environment.NewLine
                                            + SajetCommon.SetLanguage("Sure Confirm this Test Type to NG") + " ? ", 2) != DialogResult.Yes)
                {
                    return;
                }
            }

            string sSampleID = g_sSampleID;
            int iSampleSize = g_sSampleSize;
            int iSampleLevel = g_sSampleLevelID;
            string sSampleType = g_sSampleType;
            string sSampleLevel = g_sSampleLevel;

            //檢查抽驗計畫名稱

            if (sSampleType == "N/A")
            {
                SajetCommon.Show_Message("Sample Type Error", 0);
                return;
            }

            //檢查抽驗樣本數是否大於零

            if (iSampleSize == 0)
            {
                if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Sample Type") + " : " + sSampleType + Environment.NewLine
                                           + SajetCommon.SetLanguage("Level") + " : " + sSampleLevel + Environment.NewLine
                                           + SajetCommon.SetLanguage("Sample Size = 0") + Environment.NewLine + Environment.NewLine
                                           + SajetCommon.SetLanguage("Sure to Continue") + " ?", 2) != DialogResult.Yes)
                    return;
            }

            //檢查抽驗數是否達到AQL的樣本數

            if (iPassQty + iFailQty < iSampleSize)
            {
                SajetCommon.Show_Message("Pass Qty + Fail Qty less than Sample Szie", 0);
                editPassQty.Focus();
                editPassQty.SelectAll();
                return;
            }

            //檢查有值的小項是否都有達到最小檢驗數

            if (!CheckInspItemFinish())
                return;

            //要不要卡不良品數大於零時,一定要判NG?

            object[][] Params = new object[10][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TLOTNO", g_sLot };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TITEMTYPEID", g_sItemTypeID };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TSAMPLEID", sSampleID };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.Int32, "TSAMPLESIZE", iSampleSize };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.Int32, "TSAMPLELEVEL", iSampleLevel };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.Int32, "TPASSQTY", iPassQty };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.Int32, "TFAILQTY", iFailQty };
            Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TQCRESULT", sQCResult };
            Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TEMPID", g_sUserID };
            Params[9] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
            dsTemp = ClientUtils.ExecuteProc("SAJET.SJ_IQC_TRANSFER_ITEMTYPE", Params);
            string sResult = dsTemp.Tables[0].Rows[0]["TRES"].ToString();
            
            if (sResult != "OK")
            {
                SajetCommon.Show_Message(sResult, 0);
                return;
            }

            DialogResult = DialogResult.OK;  
        }

        private bool CheckInspItemFinish()
        {
            //有測試值的項目檢查輸入量測值的個數是否達到最少檢驗數
            //取得大項的最少檢驗數
            int iInspQty = 0;
            sSQL = "SELECT NVL(MIN_INSP_QTY,0) MIN_INSP_QTY "
                 + "  FROM SAJET.SYS_TEST_ITEM_TYPE "
                 + " WHERE ITEM_TYPE_ID =:ITEM_TYPE_ID "
                 + "   AND ROWNUM= 1 ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", g_sItemTypeID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count > 0)
                iInspQty = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["MIN_INSP_QTY"].ToString());
            if (iInspQty == 0)
                iInspQty = GetSysBaseMinInspQty();//取得SYS的最少檢驗數
            int iSampleSize = g_sSampleSize;

            if (iInspQty > iSampleSize)
                iInspQty = iSampleSize;

            if (g_sFlag=="PART")
            {
                sSQL = " SELECT A.*,NVL(B.QTY,0) INSP_QTY  "
                     + "   FROM "
                     + " ( SELECT B.ITEM_ID,B.ITEM_CODE,B.ITEM_NAME,NVL(B.MIN_INSP_QTY,0) MIN_INSP_QTY  " //A :找出此料號大項內有值的小項
                     + "   FROM SAJET.SYS_PART_IQC_TESTITEM A "
                     + "       ,SAJET.SYS_TEST_ITEM B "
                     + "  WHERE A.RECID = :RECID "
                     + "    AND B.ITEM_TYPE_ID =:ITEM_TYPE_ID "
                     + "    AND A.ITEM_ID = B.ITEM_ID "
                     + "    AND B.HAS_VALUE ='Y' ) A  "
                     + " ,(SELECT A.ITEM_ID,COUNT(*) QTY " //B :找出此大項下每個有值的小項已輸入量測值的個數
                     + "  FROM SAJET.G_IQC_TEST_ITEM_VALUE A "
                     + " WHERE A.LOT_NO =:LOT_NO "
                     + "   AND A.ITEM_TYPE_ID =:ITEM_TYPE_ID "
                     + "   AND NVL(A.VALUE ,'N/A') <>'N/A' "
                     + " GROUP BY A.ITEM_ID ) B "
                     + " WHERE A.ITEM_ID = B.ITEM_ID(+) " //以 A Table為主
                     + " ORDER BY A.ITEM_CODE ";

                Params = new object[3][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RECID", g_sSRECID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", g_sItemTypeID };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", g_sLot };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            }
            else
            {
                sSQL = " SELECT A.*,NVL(B.QTY,0) INSP_QTY "
                     + " FROM "
                     + " ( SELECT B.ITEM_ID,B.ITEM_CODE,B.ITEM_NAME,NVL(B.MIN_INSP_QTY,0) MIN_INSP_QTY "
                     + " FROM SAJET.SYS_IQC_PARTTYPE_TESTITEM A "
                     + " ,SAJET.SYS_TEST_ITEM B "
                     + " WHERE A.PART_TYPE = :PART_TYPE "
                     + " AND B.ITEM_TYPE_ID =:ITEM_TYPE_ID "
                     + " AND A.ITEM_ID = B.ITEM_ID "
                     + " AND B.HAS_VALUE ='Y' ) A "
                     + " ,(SELECT A.ITEM_ID,COUNT(*) QTY "
                     + " FROM SAJET.G_IQC_TEST_ITEM_VALUE A "
                     + " WHERE A.LOT_NO =:LOT_NO "
                     + " AND A.ITEM_TYPE_ID =:ITEM_TYPE_ID "
                     + " AND NVL(A.VALUE ,'N/A') <>'N/A' "
                     + " GROUP BY A.ITEM_ID ) B "
                     + " WHERE A.ITEM_ID = B.ITEM_ID(+) "
                     + " ORDER BY A.ITEM_CODE ";

                Params = new object[3][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_TYPE", g_sPARTTYPE };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", g_sItemTypeID };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", g_sLot };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            }

            string sMessage = string.Empty;

            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dr = dsTemp.Tables[0].Rows[i];
                int iInspedQty = Convert.ToInt32(dr["INSP_QTY"].ToString());
                int iItemMinQty = Convert.ToInt32(dr["MIN_INSP_QTY"].ToString());//取得小項的最少檢驗數
                if (iItemMinQty > 0)
                {
                    if (iItemMinQty < iInspQty)
                        iInspQty = iItemMinQty;
                }
                if (iInspedQty < iInspQty)
                {
                    sMessage = sMessage + dr["ITEM_CODE"].ToString() + " - " + dr["ITEM_NAME"].ToString() + " ( " + iInspedQty.ToString() + " ) " + Environment.NewLine;
                }
            }

            if (!string.IsNullOrEmpty(sMessage))
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Inspection Item") + " : " + Environment.NewLine + Environment.NewLine
                                        + sMessage + Environment.NewLine
                                        + SajetCommon.SetLanguage("Inspected Qty less than Min Insp Qty") + " ( " + iInspQty.ToString() + " ) ", 0);
                return false;

            }

            //無測試值的項目檢查良品數與不良品數是否有輸入

            if (g_sFlag=="PART")
            {
                sSQL = " SELECT A.*,NVL(B.ITEM_ID,'-1') INSP_ITEM_ID "
                     + "   FROM "
                     + " ( SELECT B.ITEM_ID,B.ITEM_CODE,B.ITEM_NAME  " //A :找出此料號大項內無測試值的小項
                     + "   FROM SAJET.SYS_PART_IQC_TESTITEM A "
                     + "       ,SAJET.SYS_TEST_ITEM B "
                     + "  WHERE A.RECID = :RECID "
                     + "    AND B.ITEM_TYPE_ID =:ITEM_TYPE_ID "
                     + "    AND A.ITEM_ID = B.ITEM_ID "
                     + "    AND B.HAS_VALUE ='N' ) A  "
                     + " ,(SELECT A.ITEM_ID " //B :找出此大項下無值的小項是否有輸入pass,fail數
                     + "  FROM SAJET.G_IQC_TEST_ITEM A "
                     + " WHERE A.LOT_NO =:LOT_NO "
                     + "   AND A.ITEM_TYPE_ID =:ITEM_TYPE_ID ) B "
                     + " WHERE A.ITEM_ID = B.ITEM_ID(+) " //以 A Table為主
                     + " ORDER BY A.ITEM_CODE ";
                Params = new object[3][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RECID", g_sSRECID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", g_sItemTypeID };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", g_sLot };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            }
            else
            {
                sSQL = " SELECT A.*,NVL(B.ITEM_ID,'-1') INSP_ITEM_ID "
                     + "   FROM "
                     + " ( SELECT B.ITEM_ID,B.ITEM_CODE,B.ITEM_NAME  " //A :找出此料號大項內無測試值的小項
                     + "   FROM SAJET.SYS_IQC_PARTTYPE_TESTITEM A "
                     + "       ,SAJET.SYS_TEST_ITEM B "
                     + "  WHERE A.PART_TYPE = :PART_TYPE "
                     + "    AND B.ITEM_TYPE_ID =:ITEM_TYPE_ID "
                     + "    AND A.ITEM_ID = B.ITEM_ID "
                     + "    AND B.HAS_VALUE ='N' ) A  "
                     + " ,(SELECT A.ITEM_ID " //B :找出此大項下無值的小項是否有輸入pass,fail數
                     + "  FROM SAJET.G_IQC_TEST_ITEM A "
                     + " WHERE A.LOT_NO =:LOT_NO "
                     + "   AND A.ITEM_TYPE_ID =:ITEM_TYPE_ID ) B "
                     + " WHERE A.ITEM_ID = B.ITEM_ID(+) " //以 A Table為主
                     + " ORDER BY A.ITEM_CODE ";
                Params = new object[3][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_TYPE", g_sPARTTYPE };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", g_sItemTypeID };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", g_sLot };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            }

            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dr = dsTemp.Tables[0].Rows[i];
                if (dr["INSP_ITEM_ID"].ToString() == "-1")
                {
                    sMessage = sMessage + dr["ITEM_CODE"].ToString() + " - " + dr["ITEM_NAME"].ToString() + Environment.NewLine;
                }
            }

            if (!string.IsNullOrEmpty(sMessage))
            {
                if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Inspection Item") + " : " + Environment.NewLine + Environment.NewLine
                                        + sMessage + Environment.NewLine
                                        + SajetCommon.SetLanguage("PASS QTY and FAIL QTY is Null") + Environment.NewLine + Environment.NewLine
                                        + SajetCommon.SetLanguage("Sure to Complete this Item ?"), 2) != DialogResult.Yes)
                {
                    return false;
                }
            }
            return true;
        }

        private void cmsdefect_Opening(object sender, CancelEventArgs e)
        {

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvDefect.Rows.Count == 0 || dgvDefect.CurrentCell == null)
                return;

            string sDefectID = dgvDefect.CurrentRow.Cells["DEFECT_ID"].Value.ToString();
            string sDefectCode = dgvDefect.CurrentRow.Cells["DEFECT_CODE"].Value.ToString();
            
            if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Delete this Defect Code ?", 1) + "  " + sDefectCode, 2) != DialogResult.Yes)
                return;
            sSQL = "DELETE SAJET.G_IQC_TEST_TYPE_DEFECT "
                + " WHERE LOT_NO =:LOT_NO "
                + "   AND ITEM_TYPE_ID =:ITEM_TYPE_ID "
                + "   AND DEFECT_ID =:DEFECT_ID ";
            object[][] Params = new object[3][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", g_sLot };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", g_sItemTypeID };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DEFECT_ID", sDefectID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            GetTestTypeDefect(g_sLot, g_sItemTypeID);
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK; 
        }

        private void btnSaveTestTypeMemo_Click(object sender, EventArgs e)
        {
            editTestTypeMemo.Text = editTestTypeMemo.Text.Trim();

            if (g_sItemTypeID == "0")
            {
                return;
            }

            int iSampleID = Convert.ToInt32(g_sSampleID);
            //int iSampleLevel = Convert.ToInt32(g_sSampleID);
            int iSampleLevel = Convert.ToInt32(g_sSampleLevelID);
            int iSampleSize = g_sSampleSize;
            object[][] Params = new object[8][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TLOTNO", g_sLot };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TITEMTYPEID", g_sItemTypeID };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TTESTTYPEMEMO", editTestTypeMemo.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.Int32, "TEMPID", g_sUserID };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.Int32, "TSAMPLEID", iSampleID };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.Int32, "TSAMPLELEVEL", iSampleLevel };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.Int32, "TSAMPLESIZE", iSampleSize };
            Params[7] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
            dsTemp = ClientUtils.ExecuteProc("SAJET.SJ_IQC_INPUT_TESTTYPE_MEMO", Params);
            string sResult = dsTemp.Tables[0].Rows[0]["TRES"].ToString();
            
            if (sResult != "OK")
            {
                SajetCommon.Show_Message(sResult, 0);
                return;
            }

            SajetCommon.Show_Message("Save Memo OK", 3);
        }

        private void editPassQty_Enter(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            t.BackColor = Color.FromArgb(255, 255, 128);
        }

        private void editPassQty_Leave(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            t.BackColor = Color.White;
        }
    }
}
