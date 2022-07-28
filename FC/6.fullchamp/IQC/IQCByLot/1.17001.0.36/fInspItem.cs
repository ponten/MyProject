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
    public partial class fInspItem : Form
    {
        public struct TItem
        {
            public String sItemID;
            public bool bHasValue;
            public String sUnit;
            public String sValueType;
            public String sUpperLimit;
            public String sLowerLimit;
            public String sMiddleLimit;
            public String sUpperCtlLimit;
            public String sLowerCtlLimit;            
        }

        public TItem[] G_rItem;

        public struct TTestValue
        {
            public int iSeq;
            public ComboBox combTestResult;
            public TextBox editTestValue;
        }

        public TTestValue[] G_rTestValue;

        public struct TTestNoValue
        {
            public TextBox editPassQty;
            public TextBox editFailQty;
            public ComboBox combTestResult;
        }

        public TTestNoValue[] G_rTestNoValue;
        public string g_sProgram;
        public string g_sLotNo, g_sTypeID, g_sRecID, g_sTypeName,g_sFlag;
        public int g_iSampleID,g_iSampleLevel,g_iSampleSize,g_iLotSize,g_iInputQty,g_iMinQty;
        string sSQL;        
        DataSet dsTemp;
        ComboBox combLeft = new ComboBox();
        ComboBox combResult = new ComboBox();
        ComboBox combTitle = new ComboBox();

       
        public fInspItem()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (combItemName.SelectedIndex < 0)
                return;
            int iIndex = combItemName.SelectedIndex;
            string sItemID = combItemID.Items[combItemName.SelectedIndex].ToString();
           
            if (G_rItem[0].bHasValue)//有測試值
            {
                int iValueQty = 0;
                string sTestData = string.Empty;
               // for (int i = 0; i <= g_iSampleSize - 1; i++)

                for (int i = 0; i <= g_iInputQty - 1; i++)
                {
                    string sSeq = G_rTestValue[i].iSeq.ToString();
                    string sTestValue = G_rTestValue[i].editTestValue.Text.Trim();
                    string sResult = G_rTestValue[i].combTestResult.SelectedIndex.ToString();
                    if (string.IsNullOrEmpty(sTestValue))
                        continue;
                    iValueQty += 1;
                    sTestData = sTestData + sSeq + "@" + sTestValue + "@" + sResult + "@";
                }

                //有檢測值的項目個數小於設定個數

                if (iValueQty < Convert.ToInt32(lablMinInspQty.Text))
                {
                    SajetCommon.Show_Message("Inspected Qty less than Min Insp Qty", 0);
                    return;
                }

                if (!string.IsNullOrEmpty(sTestData))
                {
                    object[][] Params = new object[11][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TLOTNO", g_sLotNo };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TITEMTYPEID", g_sTypeID };
                    Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TITEMID", sItemID };
                    Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TTESTDATA", sTestData };
                    Params[4] = new object[] { ParameterDirection.Input, OracleType.Int32, "TEMPID", ClientUtils.UserPara1 };
                    Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "THASVALUE", "Y" };
                    Params[6] = new object[] { ParameterDirection.Input, OracleType.Int32, "TSAMPLEID", g_iSampleID };
                    Params[7] = new object[] { ParameterDirection.Input, OracleType.Int32, "TSAMPLELEVEL", g_iSampleLevel };
                    Params[8] = new object[] { ParameterDirection.Input, OracleType.Int32, "TSAMPLESIZE", g_iSampleSize };
                    Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TMEMO", "" };
                    Params[10] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                    dsTemp = ClientUtils.ExecuteProc("SAJET.SJ_IQC_INSP_ITEM", Params);
                    string sResult = dsTemp.Tables[0].Rows[0]["TRES"].ToString();
                    if (sResult != "OK")
                    {
                        SajetCommon.Show_Message(sResult, 0);
                        return;
                    }
                }
            }
            else //無測試值
            {
                int iPassQty =0;
                int iFailQty =0;
                editMemo.Text = editMemo.Text.Trim();

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
                int iMinInspQty = g_iSampleSize;
                if (lablMinInspQty.Text != "0")
                {
                    try
                    {
                        iMinInspQty = Convert.ToInt32(lablMinInspQty.Text);
                    }
                    catch
                    {
                    }
                }

                if (iPassQty + iFailQty > g_iLotSize)
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Pass Qty",1)+" + "+SajetCommon.SetLanguage("Fail Qty",1)+" > "+SajetCommon.SetLanguage("Lot Size",1), 0);
                    editPassQty.Focus();
                    editPassQty.SelectAll();
                    return;
                }
                if (iPassQty + iFailQty < iMinInspQty)
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Pass Qty", 1) + " + " + SajetCommon.SetLanguage("Fail Qty", 1) + " < " + SajetCommon.SetLanguage("Sample Size", 1) + Environment.NewLine + Environment.NewLine
                                            + SajetCommon.SetLanguage("or")+Environment.NewLine+Environment.NewLine
                                            + SajetCommon.SetLanguage("Pass Qty", 1) + " + " + SajetCommon.SetLanguage("Fail Qty", 1) + " < " + SajetCommon.SetLanguage("Min Insp Qty", 1) + Environment.NewLine, 0);
                    editPassQty.Focus();
                    editPassQty.SelectAll();
                    return;
                }

                string sTestData = string.Empty;
                sTestData = sTestData + editPassQty.Text + "@" + editFailQty.Text + "@";
                object[][] Params = new object[11][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TLOTNO", g_sLotNo };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TITEMTYPEID", g_sTypeID };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TITEMID", sItemID };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TTESTDATA", sTestData };
                Params[4] = new object[] { ParameterDirection.Input, OracleType.Int32, "TEMPID", ClientUtils.UserPara1 };
                Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "THASVALUE", "N" };
                Params[6] = new object[] { ParameterDirection.Input, OracleType.Int32, "TSAMPLEID", g_iSampleID };
                Params[7] = new object[] { ParameterDirection.Input, OracleType.Int32, "TSAMPLELEVEL", g_iSampleLevel };
                Params[8] = new object[] { ParameterDirection.Input, OracleType.Int32, "TSAMPLESIZE", g_iSampleSize };
                Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TMEMO",editMemo.Text };
                Params[10] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                dsTemp = ClientUtils.ExecuteProc("SAJET.SJ_IQC_INSP_ITEM", Params);
                string sResult = dsTemp.Tables[0].Rows[0]["TRES"].ToString();

                if (sResult != "OK")
                {
                    SajetCommon.Show_Message(sResult, 0);
                    return;
                }                
            }
            if (iIndex == combItemName.Items.Count - 1)
            {
                if (SajetCommon.Show_Message(SajetCommon.SetLanguage("It is last Test Item") + Environment.NewLine
                                        + SajetCommon.SetLanguage("Close form") + "?", 2) == DialogResult.Yes)
                    DialogResult = DialogResult.OK;
            }
            else
            {
                iIndex += 1;
                combItemName.SelectedIndex = iIndex;
            }
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
        private int GetTestItemMinInspQty(string sItemID) //取得小項的最少檢驗個數
        {
            sSQL = "SELECT NVL(MIN_INSP_QTY,0) MIN_INSP_QTY "
               + "  FROM SAJET.SYS_TEST_ITEM "
               + "  WHERE ITEM_TYPE_ID =:ITEM_TYPE_ID "
               + "    AND ITEM_ID =:ITEM_ID "
               + "    AND ROWNUM= 1 ";
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", g_sTypeID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_ID", sItemID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt32(dsTemp.Tables[0].Rows[0]["MIN_INSP_QTY"].ToString());
            }
            else
                return 0;
        }
        private int GetTestTypeMinInspQty() //取得大項的最少檢驗個數
        {
            sSQL = "SELECT NVL(MIN_INSP_QTY,0) MIN_INSP_QTY "
               + "  FROM SAJET.SYS_TEST_ITEM_TYPE "
               + "  WHERE ITEM_TYPE_ID =:ITEM_TYPE_ID "
               + "    AND ROWNUM= 1 ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", g_sTypeID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt32(dsTemp.Tables[0].Rows[0]["MIN_INSP_QTY"].ToString());
            }
            else
                return 0;                        
        }

        private void fInspItem_Load(object sender, EventArgs e)
        {
          //  ClientUtils.SetLanguage(this, ClientUtils.fCurrentProject);
            SajetCommon.SetLanguageControl(this);
            btnCopyResult.Enabled = btnSave.Enabled;
            if (g_sFlag == "PART")
            {

                sSQL = " SELECT B.ITEM_ID,B.ITEM_CODE,B.ITEM_NAME,B.HAS_VALUE,B.VALUE_TYPE "
                    + "   FROM SAJET.SYS_PART_IQC_TESTITEM A "
                    + "       ,SAJET.SYS_TEST_ITEM B "
                    + "  WHERE A.RECID = :RECID "
                    + "    AND B.ITEM_TYPE_ID =:ITEM_TYPE_ID "
                    + "    AND A.ITEM_ID = B.ITEM_ID "
                    + "  ORDER BY A.SORT_INDEX,B.ITEM_CODE ";
            }
            else
            {
                sSQL = " SELECT B.ITEM_ID,B.ITEM_CODE,B.ITEM_NAME,B.HAS_VALUE,B.VALUE_TYPE "
                    + "   FROM SAJET.SYS_IQC_PARTTYPE_TESTITEM A "
                    + "       ,SAJET.SYS_TEST_ITEM B "
                    + "  WHERE A.PART_TYPE = :RECID "
                    + "    AND B.ITEM_TYPE_ID =:ITEM_TYPE_ID "
                    + "    AND A.ITEM_ID = B.ITEM_ID "
                    + "  ORDER BY A.SORT_INDEX,B.ITEM_CODE ";
            }
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RECID", g_sRecID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", g_sTypeID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            for (int i = 0; i<= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                combItemName.Items.Add(dsTemp.Tables[0].Rows[i]["ITEM_CODE"].ToString() + " - " + dsTemp.Tables[0].Rows[i]["ITEM_NAME"].ToString());
                combItemID.Items.Add(dsTemp.Tables[0].Rows[i]["ITEM_ID"].ToString());
            }
            //取得大項最少檢驗個數
            g_iMinQty = GetTestTypeMinInspQty();
            if (g_iMinQty == 0)
            {
                //取得SYS_BASE裡定義的最少檢驗數
                g_iMinQty = GetSysBaseMinInspQty();
            }

            /*
            lablMinInspQty.Text = GetTestTypeMinInspQty().ToString();
            if (lablMinInspQty.Text == "0")
            {
                lablMinInspQty.Text = GetSysBaseMinInspQty().ToString();
            }
            */ 
            

            //若最少檢驗個數  大於 抽驗標準數,則以抽驗標準數為主
            if ( (g_iSampleSize > 0 && Convert.ToInt32(lablMinInspQty.Text) > g_iSampleSize))
            {
                lablMinInspQty.Text = g_iSampleSize.ToString();
            }
            g_iInputQty = Convert.ToInt32(lablMinInspQty.Text);

            combLeft.Items.Add("0");
            combLeft.Items.Add("10");
            combLeft.Items.Add("100");
            combLeft.Items.Add("200");
            combResult.Items.Add("OK"); //檢驗結果下拉選單
            combResult.Items.Add("NG");
            combResult.Items.Add("N/A");
            combTitle.Items.Add(SajetCommon.SetLanguage("Item",1));
            combTitle.Items.Add(SajetCommon.SetLanguage("Value",1));
            combTitle.Items.Add(SajetCommon.SetLanguage("Result",1));
            lablSampleSize.Text = g_iSampleSize.ToString();
            lablTypeName.Text = g_sTypeName;
           // g_iInputQty = g_iSampleSize;
            //sSQL="SELECT NVL(PARAM_VALUE,0) PARAM_VALUE "
            //    +"  FROM SAJET.SYS_BASE "
            //    +" WHERE PARAM_NAME ='Test Item Input Edit Qty' "
            //    +"   AND PROGRAM=:PROGRAM";
            //Params = new object[1][];
            //Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROGRAM", g_sProgram };
            //dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            //if (dsTemp.Tables[0].Rows.Count > 0)
            //{
            //    g_iInputQty = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["PARAM_VALUE"].ToString());
            //}
            //特別設定的值若超過抽驗數,則應以抽驗數為主
            //if (g_iSampleSize > 0 && g_iSampleSize < g_iInputQty)
            //{
            //    g_iInputQty = g_iSampleSize;
            //}
            if (combItemName.Items.Count > 0)
                combItemName.SelectedIndex = 0;
        }

        private void EditQtyOnExit(object sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                (sender as TextBox).BackColor = Color.FromName("White");
            }
        }

        private void EditTestValueOnExit(object sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                String sValue = (sender as TextBox).Text.Trim();
                if (!string.IsNullOrEmpty(sValue))
                {
                    InputTestValue((sender as TextBox));
                }
                else
                {
                    (sender as TextBox).BackColor = Color.FromName("White");
                }
            }
        }

        public bool InputTestValue(TextBox editTestValue)
        {
            String sValue = editTestValue.Text;
            int iIndex = Convert.ToInt32(editTestValue.Tag.ToString())-1;
            String sUpperLimit = G_rItem[0].sUpperLimit;
            String sUpperCtlLimit = G_rItem[0].sUpperCtlLimit;
            String sLowerCtlLimit = G_rItem[0].sLowerCtlLimit;
            String sLowerLimit = G_rItem[0].sLowerLimit;
            String sMiddleLimit = G_rItem[0].sMiddleLimit;
            String sResult = "";
            String sWarningRes = "";
            //當combox的itemindex改變後會自動觸發change事件
            G_rTestValue[iIndex].combTestResult.SelectedIndex = 0;

            if ((!string.IsNullOrEmpty(sValue)) && (G_rItem[0].sValueType == "0"))
            {
                if (!CheckTestValueResult(sUpperLimit, sLowerLimit, sMiddleLimit, sValue, sUpperCtlLimit, sLowerCtlLimit, ref sResult, ref sWarningRes))
                {
                    G_rTestValue[iIndex].editTestValue.Focus();
                    G_rTestValue[iIndex].editTestValue.SelectAll();
                    return false;
                }
                if (sResult == "1")
                {
                    G_rTestValue[iIndex].editTestValue.BackColor = Color.FromName("Red");
                    G_rTestValue[iIndex].combTestResult.SelectedIndex = 1;
                }
                else if (sWarningRes == "1")
                {
                    G_rTestValue[iIndex].editTestValue.BackColor = Color.FromName("Orange");
                }
                else
                {
                    G_rTestValue[iIndex].editTestValue.BackColor = Color.FromName("White");
                }

            }
            return true;
        }

        public bool CheckTestValueResult(String sUpperLimit, String sLowerLimit, String sMiddleLimit, String sTestValue, String sUpperCtlLimit, String sLowerCtlLimit, ref String sResult, ref String sWarningRes)
        {
            sResult = "0";
            sWarningRes = "0";
            bool bUL = false;
            bool bLL = false;

            try
            {
                double fTestValue = Convert.ToDouble(sTestValue);
                
                //檢查上限

                if (!string.IsNullOrEmpty(sUpperLimit))
                {
                    double fUpperLimit = Convert.ToDouble(sUpperLimit);
                    bUL = true;
                    if (fTestValue > fUpperLimit)
                    {
                        sResult = "1";
                    }

                }

                //檢查下限

                if (!string.IsNullOrEmpty(sLowerLimit))
                {
                    double fLowerLimit = Convert.ToDouble(sLowerLimit);
                    bLL = true;
                    if (fTestValue < fLowerLimit)
                    {
                        sResult = "1";
                    }

                }

                if (sResult == "0")
                { //檢查管制上限(警告值)
                    if (!string.IsNullOrEmpty(sUpperCtlLimit))
                    {
                        double fUpperCtlLimit = Convert.ToDouble(sUpperCtlLimit);

                        if ((fTestValue > fUpperCtlLimit) && ((bUL) && (fTestValue <= Convert.ToDouble(sUpperLimit))))
                        {
                            sWarningRes = "1";
                        }

                    }
                    if (!string.IsNullOrEmpty(sLowerCtlLimit))
                    {

                        Double fLowerCtlLimit = Convert.ToDouble(sLowerCtlLimit);
                        if ((fTestValue < fLowerCtlLimit) && ((bLL) && (fTestValue >= Convert.ToDouble(sLowerLimit))))
                        {
                            sWarningRes = "1";
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);
                return false;
            }
        }

        private void EditQtyOnKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is TextBox)
            {
                int iTag = Convert.ToInt32((sender as TextBox).Tag.ToString());
                if (!((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == 8 || e.KeyChar == '.' || e.KeyChar == 13 || e.KeyChar == 46 || e.KeyChar == '-'))
                {
                    e.KeyChar = (char)Keys.None;
                }
                //}
                if (e.KeyChar != (char)Keys.Return)
                    return;

                if (iTag == 1)
                {
                    G_rTestNoValue[0].editFailQty.Focus();
                    G_rTestNoValue[0].editFailQty.SelectAll();
                }
                else 
                {
                    G_rTestNoValue[0].editPassQty.Focus();
                    G_rTestNoValue[0].editPassQty.SelectAll();
                }
            }
        }

        private void EditTestValueOnKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is TextBox)
            {
                string sValueType = G_rItem[0].sValueType;
                int iTag = Convert.ToInt32((sender as TextBox).Tag.ToString());
               // if ((G_rTestValue[iTag - 1].bHasValue) && (G_rTestValue[iTag - 1].sValueType == "0"))
               // {
                if (sValueType == "0") //數字
                {
                    if (!((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == 8 || e.KeyChar == '.' || e.KeyChar == 13 || e.KeyChar == 46 || e.KeyChar == '-'))
                    {
                        e.KeyChar = (char)Keys.None;
                    }
                }
                //}

                if (e.KeyChar != (char)Keys.Return)
                    return;
                if (!InputTestValue(G_rTestValue[iTag - 1].editTestValue))
                    return;
             //   if (iTag == g_iSampleSize)
                if (iTag == g_iInputQty)
                {
                    btnSave_Click(sender, e);
                    return;
                }
               // for (int i = iTag; i <= g_iSampleSize - 1; i++)
                for (int i = iTag; i <= g_iInputQty - 1; i++)
                {
                    //  if (tControlAdd[i].bHasValue)
                    // {
                    G_rTestValue[i].editTestValue.Focus();
                    G_rTestValue[i].editTestValue.SelectAll();
                    break;
                    // }
                }
            }
        }

        private void EditTestValueOnEnter(object sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                (sender as TextBox).BackColor = Color.FromName("Yellow");
            }
        }

        private void EditQtyOnEnter(object sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                (sender as TextBox).BackColor = Color.FromName("Yellow");
            }
        }

        private void combItemName_SelectedIndexChanged(object sender, EventArgs e)
        {
            PanelMain.Controls.Clear();
            string sItemID = combItemID.Items[combItemName.SelectedIndex].ToString();
            //取得小項的最少檢驗數
            int iMinQty = GetTestItemMinInspQty(sItemID);
            if (iMinQty == 0)
            {
                iMinQty = g_iMinQty;//小項未定義,則以大項或SYS為主
            }
            //若最少檢驗個數  大於 抽驗標準數,則以抽驗標準數當最少檢驗數
            if ((g_iSampleSize > 0) && (iMinQty > g_iSampleSize))
            {
                iMinQty = g_iSampleSize;
            }
            g_iInputQty = iMinQty;
            lablMinInspQty.Text = iMinQty.ToString();
            if (g_sFlag == "PART")
            {
                sSQL = " SELECT B.ITEM_CODE,B.ITEM_NAME,B.HAS_VALUE,B.VALUE_TYPE "
                    + "        ,A.* "
                    + "   FROM SAJET.SYS_PART_IQC_TESTITEM A "
                    + "       ,SAJET.SYS_TEST_ITEM B "
                    + "  WHERE A.RECID = :RECID "
                    + "    AND A.ITEM_ID =:ITEM_ID "
                    + "    AND A.ITEM_ID = B.ITEM_ID ";
            }
            else
            {
                sSQL = " SELECT B.ITEM_CODE,B.ITEM_NAME,B.HAS_VALUE,B.VALUE_TYPE "
                    + "        ,A.* "
                    + "   FROM SAJET.SYS_IQC_PARTTYPE_TESTITEM A "
                    + "       ,SAJET.SYS_TEST_ITEM B "
                    + "  WHERE A.PART_TYPE = :RECID "
                    + "    AND A.ITEM_ID =:ITEM_ID "
                    + "    AND A.ITEM_ID = B.ITEM_ID ";
            }
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RECID", g_sRecID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_ID", sItemID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            if (dsTemp.Tables[0].Rows.Count == 0)
                return;
            G_rItem = new TItem[1];

            DataRow dr = dsTemp.Tables[0].Rows[0];
            G_rItem[0].sItemID = sItemID;
            G_rItem[0].bHasValue = (dr["HAS_VALUE"].ToString()=="Y");
            G_rItem[0].sUpperLimit = dr["UPPER_LIMIT"].ToString();
            G_rItem[0].sLowerLimit = dr["LOWER_LIMIT"].ToString();
            G_rItem[0].sMiddleLimit = dr["MIDDLE_LIMIT"].ToString();
            G_rItem[0].sUpperCtlLimit = dr["UPPER_CONTROL_LIMIT"].ToString();
            G_rItem[0].sLowerCtlLimit = dr["LOWER_CONTROL_LIMIT"].ToString();
            G_rItem[0].sUnit = dr["UNIT"].ToString();
            G_rItem[0].sValueType = dr["VALUE_TYPE"].ToString();

            //
            dgvTestItemSpec.Rows.Clear();
            dgvTestItemSpec.Rows.Add();
            dgvTestItemSpec.Rows[dgvTestItemSpec.Rows.Count - 1].Cells["LSL"].Value = G_rItem[0].sLowerLimit;
            dgvTestItemSpec.Rows[dgvTestItemSpec.Rows.Count - 1].Cells["LCL"].Value = G_rItem[0].sLowerCtlLimit;
            dgvTestItemSpec.Rows[dgvTestItemSpec.Rows.Count - 1].Cells["CL"].Value = G_rItem[0].sMiddleLimit;
            dgvTestItemSpec.Rows[dgvTestItemSpec.Rows.Count - 1].Cells["UCL"].Value = G_rItem[0].sUpperCtlLimit;
            dgvTestItemSpec.Rows[dgvTestItemSpec.Rows.Count - 1].Cells["USL"].Value = G_rItem[0].sUpperLimit;
            dgvTestItemSpec.Rows[dgvTestItemSpec.Rows.Count - 1].Cells["UNIT"].Value = G_rItem[0].sUnit;
            //
   
            int x = 10; int y = 20;
            int iHeight = 0;
            Font cFont = new Font(lablResult.Font.Name, lablResult.Font.Size, lablResult.Font.Style);
            Size cSize = new Size(lablResult.Size.Width, lablResult.Size.Height);
            PanelMain.Visible = (G_rItem[0].bHasValue);
            PanelNoValue.Visible = (!PanelMain.Visible);

            if (G_rItem[0].bHasValue) //有測試值
            {
                for (int i = 0; i <= combTitle.Items.Count - 1; i++)
                {
                    //Label =============================               
                    Label labelTemp = new Label();
                    labelTemp.Left = Convert.ToInt32(combLeft.Items[i + 1].ToString());
                    labelTemp.Top = 0;
                    labelTemp.Name = "udfLabTitle" + Convert.ToString(i + 1);
                    labelTemp.ForeColor = Color.FromName("Blue");
                    labelTemp.Font = new Font(lablResult.Font.Name, lablResult.Font.Size, lablResult.Font.Style);
                    labelTemp.Size = new System.Drawing.Size(35, 12); ;
                    labelTemp.AutoSize = true;
                    labelTemp.Text = combTitle.Items[i].ToString();
                    PanelMain.Controls.Add(labelTemp);

                    //  tControlAdd[i].LabControl = labelTemp;
                }
                PanelMain.Dock = DockStyle.Fill;
                //G_rTestValue = new TTestValue[g_iSampleSize];
                G_rTestValue = new TTestValue[g_iInputQty];

                for (int i = 0; i <= g_iInputQty - 1; i++)
                {
                    Label labelTemp1 = new Label();
                    x = Convert.ToInt32(combLeft.Items[1].ToString());
                    labelTemp1.Location = new Point(x, y);
                    labelTemp1.Font = cFont;
                    labelTemp1.MaximumSize = new Size(150, 0);//太長時會換行顯示
                    labelTemp1.AutoSize = true;
                    labelTemp1.Text = (i + 1).ToString();
                    PanelMain.Controls.Add(labelTemp1);
                    iHeight = labelTemp1.Size.Height;

                    TextBox editTemp = new TextBox();
                    x = Convert.ToInt32(combLeft.Items[2].ToString());
                    editTemp.Location = new Point(x, y);
                    editTemp.Font = cFont;
                    editTemp.Size = new Size(70, lablResult.Size.Height);
                    editTemp.Tag = i + 1;
                    editTemp.Text = "";

                    if (editTemp.Size.Height > iHeight)
                        iHeight = editTemp.Size.Height;
                    editTemp.KeyPress += new KeyPressEventHandler(EditTestValueOnKeyPress);
                    editTemp.Leave += new EventHandler(EditTestValueOnExit);
                    editTemp.Enter += new EventHandler(EditTestValueOnEnter);
                    PanelMain.Controls.Add(editTemp);

                    ComboBox combTemp = new ComboBox();
                    x = Convert.ToInt32(combLeft.Items[3].ToString());
                    combTemp.Location = new Point(x, y);
                    combTemp.Font = cFont;
                    combTemp.Size = new Size(70, lablResult.Size.Height);
                    combTemp.Tag = i + 1;
                    combTemp.DropDownStyle = ComboBoxStyle.DropDownList;
                    //   combTemp.SelectedIndexChanged += new EventHandler(combResultChange);

                    for (int j = 0; j <= combResult.Items.Count - 1; j++)
                    {
                        combTemp.Items.Add(combResult.Items[j].ToString());
                    }

                    PanelMain.Controls.Add(combTemp);

                    if (combTemp.Size.Height > iHeight)
                        iHeight = combTemp.Size.Height;

                    y = y + iHeight + 1;
                    G_rTestValue[i].editTestValue = editTemp;
                    G_rTestValue[i].combTestResult = combTemp;
                    G_rTestValue[i].iSeq = i + 1;
                }
                sSQL = "SELECT VALUE,ITEM_SEQ,DECODE(RESULT,'0',0,'1',1,2) RESULT "
                    + "  FROM SAJET.G_IQC_TEST_ITEM_VALUE "
                    + "  WHERE LOT_NO =:LOT_NO "
                    + "   AND ITEM_TYPE_ID =:ITEM_TYPE_ID "
                    + "   AND ITEM_ID =:ITEM_ID "
                    + " ORDER BY ITEM_SEQ ";
                Params = new object[3][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", g_sLotNo };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", g_sTypeID };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_ID", sItemID };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

                for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
                {
                    dr = dsTemp.Tables[0].Rows[i];
                    // for (int j = 0; j <= g_iSampleSize - 1; j++)
                    for (int j = 0; j <= g_iInputQty - 1; j++)
                    {
                        if (G_rTestValue[j].iSeq == Convert.ToInt32(dr["ITEM_SEQ"].ToString()))
                        {
                            G_rTestValue[j].editTestValue.Text = dr["VALUE"].ToString();
                            InputTestValue(G_rTestValue[j].editTestValue);
                            G_rTestValue[j].combTestResult.SelectedIndex = Convert.ToInt32(dr["RESULT"].ToString());
                            break;
                        }
                    }
                }
                if (G_rTestValue.Length > 0)
                {
                    G_rTestValue[0].editTestValue.Focus();
                    G_rTestValue[0].editTestValue.SelectAll();
                }
            }
            else //無測試值
            {
                editPassQty.Text = string.Empty;
                editFailQty.Text = string.Empty;
                editMemo.Text = string.Empty;
                PanelNoValue.Dock = DockStyle.Fill;
                sSQL = "SELECT PASS_QTY,FAIL_QTY,MEMO "
                    + "  FROM SAJET.G_IQC_TEST_ITEM "
                    + "  WHERE LOT_NO =:LOT_NO "
                    + "   AND ITEM_TYPE_ID =:ITEM_TYPE_ID "
                    + "   AND ITEM_ID =:ITEM_ID "
                    + "   AND ROWNUM = 1";

                Params = new object[3][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", g_sLotNo };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE_ID", g_sTypeID };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_ID", sItemID };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    editPassQty.Text = dsTemp.Tables[0].Rows[0]["PASS_QTY"].ToString();
                    editFailQty.Text = dsTemp.Tables[0].Rows[0]["FAIL_QTY"].ToString();
                    editMemo.Text = dsTemp.Tables[0].Rows[0]["MEMO"].ToString();

                }
                editPassQty.Focus();
                editPassQty.Select();
            }
        }

        private void editPassQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == '.' || e.KeyChar == 8 || e.KeyChar == 13 || e.KeyChar == 46))
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
            int iMinQty = 0;
            try
            {
                iMinQty = Convert.ToInt32(lablMinInspQty.Text);
            }
            catch
            {
                return;
            }

            //先用最小檢驗數去減
            int iDiff = iMinQty - iPassQty;
            if (iDiff < 0) //如果PASSQTY >最小檢驗數.就有抽驗數去減

                iDiff = g_iSampleSize - iPassQty;
            if (iDiff < 0)
                iDiff = 0;
            editFailQty.Text = iDiff.ToString();
            editFailQty.Focus();
            editFailQty.SelectAll();
        }

        private void editFailQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == '.' || e.KeyChar == 8 || e.KeyChar == 13 || e.KeyChar == 46))
            {
                e.KeyChar = (char)Keys.None;
            }
            if (e.KeyChar != (char)Keys.Return)
                return;
            editMemo.Focus();
            editMemo.SelectAll();
            
        }

        private void editMemo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            btnSave_Click(sender, e);
           
            
        }

        private void fInspItem_Shown(object sender, EventArgs e)
        {
            if ((G_rTestValue != null) && G_rTestValue.Length > 0)
                G_rTestValue[0].editTestValue.Focus();
        }

        private void btnCopyResult_Click(object sender, EventArgs e)
        {
            //SajetCommon.Show_Message(g_sLotNo,0);
            fCopyRTResult fData = new fCopyRTResult();
            string sLotNo = "";
            try
            {
                fData.g_sLotNo = g_sLotNo;
                fData.g_sTypeName = g_sTypeName;
                fData.g_sTypeID = g_sTypeID;
                if (fData.ShowDialog() != DialogResult.OK)
                {
                    return;
                   
                }
                for (int i = 0; i <= combItemID.Items.Count - 1; i++)
                {
                    string sItemID = combItemID.Items[i].ToString();
                    sLotNo = fData.g_sLotNo;
                    object[][] Params = new object[9][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TLOTNOFROM", sLotNo };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TLOTNOTO", g_sLotNo };
                    Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TITEMTYPEID", g_sTypeID };
                    Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TITEMID", sItemID };
                    Params[4] = new object[] { ParameterDirection.Input, OracleType.Int32, "TEMPID", ClientUtils.UserPara1 };
                    Params[5] = new object[] { ParameterDirection.Input, OracleType.Int32, "TSAMPLEID", g_iSampleID };
                    Params[6] = new object[] { ParameterDirection.Input, OracleType.Int32, "TSAMPLELEVEL", g_iSampleLevel };
                    Params[7] = new object[] { ParameterDirection.Input, OracleType.Int32, "TSAMPLESIZE", g_iSampleSize };
                    Params[8] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                    dsTemp = ClientUtils.ExecuteProc("SAJET.SJ_IQC_INSP_ITEM_COPY", Params);
                    string sResult = dsTemp.Tables[0].Rows[0]["TRES"].ToString();
                    if (sResult != "OK")
                    {
                        SajetCommon.Show_Message(sResult, 0);
                        return;
                    }
                }
                SajetCommon.Show_Message("Copy OK", -1);
                //DialogResult = DialogResult.OK;
            }
            finally
            {
                fData.Dispose();
            }
        }
    }
}