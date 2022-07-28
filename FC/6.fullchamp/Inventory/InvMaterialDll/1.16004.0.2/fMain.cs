using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using SajetClass;
using SajetFilter;
using System.IO;

namespace InvMaterialdll
{
    public partial class fMain : Form
    {                       
        public fMain()
        {
            InitializeComponent();
        }
        string g_sWarehouseID;
        public DataSet dsTemp;
        String sSQL, g_sUserID, g_sProgram, g_sFunction, g_sExeName;
        String g_sRTRecord;
        String g_sPartID, g_sVendorID, g_sRTID;
        //String g_sDivPartID, g_sDivVendorID, g_sDivRTID, sWH, sLoc, g_sWarehouseID;
        DateTime g_dtSysdate;
        string g_sIniFile = Application.StartupPath + "\\sajet.ini";
        bool g_bDivReel;
        public struct TPrintSetup
        {
            public int iPrintQty;
            public string sPrintMethod;
            public string sPrintPort;
            public string sCodeSoftVer;
        }
        TPrintSetup TPrint = new TPrintSetup();

        private void ReadCfgFromIni()
        {
            SajetInifile sajetInifile1 = new SajetInifile();
            string sSeqLen = "5";
            string sStdLen = "13";
            string sCheckLength = "N";
            string sCopies = "1";
            string sCheckPrint = "N";
            editFix.ReadOnly = false;
            combPrefix.Enabled = true;
            string sPreFix = sajetInifile1.ReadIniFile(g_sIniFile, g_sProgram, "Prefix", "TP");
            int iIndex = combPrefix.Items.IndexOf(sPreFix);
            if (iIndex < 0)
                combPrefix.SelectedIndex = 0;
            else
                combPrefix.SelectedIndex = iIndex;
            sPreFix = combPrefix.Text;
            if (rdbtnAuto.Checked)
            {
                //sPreFix = sPreFix + DateTime.Now.ToString("yyyyMM");
                sPreFix = sPreFix + GetReelYYMMDD();
                editFix.Text = sPreFix;
                sSeqLen = sajetInifile1.ReadIniFile(g_sIniFile, g_sProgram, "Seq Length", "5");
                sStdLen = sajetInifile1.ReadIniFile(g_sIniFile, g_sProgram, "Value Length", "13");
                sCheckLength = sajetInifile1.ReadIniFile(g_sIniFile, g_sProgram, "Check Length", "N");
                sCopies = sajetInifile1.ReadIniFile(g_sIniFile, g_sProgram, "Copies", "1");
                sCheckPrint = sajetInifile1.ReadIniFile(g_sIniFile, g_sProgram, "Check Print", "N");
                chkbPrint.Checked = (sCheckPrint == "Y");
                chkbLength.Checked = (sCheckLength == "Y");
                editFix.ReadOnly = true;
                
            }
            else
            {
                combPrefix.Enabled = false;
                sSeqLen = sajetInifile1.ReadIniFile(g_sIniFile, g_sProgram, "Manual Seq Length", "0");
                sStdLen = sajetInifile1.ReadIniFile(g_sIniFile, g_sProgram, "Manual Value Length", "13");
                sCheckLength = sajetInifile1.ReadIniFile(g_sIniFile, g_sProgram, "Manual Check Length", "N");
                sCopies = sajetInifile1.ReadIniFile(g_sIniFile, g_sProgram, "Manual Copies", "1");
                sCheckPrint = sajetInifile1.ReadIniFile(g_sIniFile, g_sProgram, "Manual Check Print", "N");
                chkbPrint.Checked = (sCheckPrint == "Y");
                chkbLength.Checked = (sCheckLength == "Y");
            }   
            try
            {
                Convert.ToInt32(sSeqLen);
            }
            catch
            {
                sSeqLen = "5";
            }
            try
            {
                Convert.ToInt32(sStdLen);
            }
            catch
            {
                sStdLen = "13";
            }
            try
            {
                Convert.ToInt32(sCopies);
            }
            catch
            {
                sCopies = "1";
            }
            editPrintQty.Value = Convert.ToInt32(sCopies);
            editSeqLength.Value = Convert.ToInt32(sSeqLen);
            editStdLen.Value = Convert.ToInt32(sStdLen);
        }
        private void SaveCfgToIni()
        {
            SajetInifile sajetInifile1 = new SajetInifile();
            string sCheckPrint = "N";
            if (chkbPrint.Checked)
                sCheckPrint = "Y";
            string sCheckLen = "N";
            if (chkbLength.Checked)
                sCheckLen = "Y";

            if (rdbtnAuto.Checked)
            {
                //sajetInifile1.WriteIniFile(g_sIniFile, g_sProgram, "Prefix", editFix.Text.Substring(0, 2));
                sajetInifile1.WriteIniFile(g_sIniFile, g_sProgram, "Prefix", combPrefix.Text);
                sajetInifile1.WriteIniFile(g_sIniFile, g_sProgram, "Seq Length", editSeqLength.Value.ToString());
                sajetInifile1.WriteIniFile(g_sIniFile, g_sProgram, "Value Length", editStdLen.Value.ToString());
                sajetInifile1.WriteIniFile(g_sIniFile, g_sProgram, "Check Length", sCheckLen);
                sajetInifile1.WriteIniFile(g_sIniFile, g_sProgram, "Check Print", sCheckPrint);
                sajetInifile1.WriteIniFile(g_sIniFile, g_sProgram, "Copies", editPrintQty.Value.ToString());

            }
            else
            {
                sajetInifile1.WriteIniFile(g_sIniFile, g_sProgram, "Manual Seq Length", editSeqLength.Value.ToString());
                sajetInifile1.WriteIniFile(g_sIniFile, g_sProgram, "Manual Value Length", editStdLen.Value.ToString());
                sajetInifile1.WriteIniFile(g_sIniFile, g_sProgram, "Manual Check Length", sCheckLen);
                sajetInifile1.WriteIniFile(g_sIniFile, g_sProgram, "Manual Check Print", sCheckPrint);
                sajetInifile1.WriteIniFile(g_sIniFile, g_sProgram, "Manual Copies", editPrintQty.Value.ToString());

            }

        }
        private bool CheckDupReelNo(string sPartNo, string sVid, string sDateCode, string sLot, ref ListBox listbDupReel)
        {
            listbDupReel.Items.Clear();

            sSQL = "Select A.REEL_NO,B.PART_NO,C.VENDOR_NAME,A.LOT,A.DATECODE,C.VENDOR_CODE,A.REEL_MEMO "
                 + "  FROM SAJET.G_MATERIAL A "
                 + "      ,SAJET.SYS_PART B "
                 + "      ,SAJET.SYS_VENDOR C "
                 + " Where A.LOT = :sLot "
                 + " And A.DateCode = :sDateCode "
                 + " AND A.PART_ID = B.PART_ID "
                 + " AND A.VENDOR_ID = C.VENDOR_ID "
                 + " AND B.PART_NO = :sPartNo "
                 + " AND C.VENDOR_CODE = :sVid ";
            object[][] Params = new object[4][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "sLot", sLot };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "sDateCode", sDateCode };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "sPartNo", sPartNo };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "sVid", sVid };

            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);


            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                fDupReel fData = new fDupReel();
                try
                {
                    //fData.lablReelNo.Text = sStartNo + " ~ " + sEndNo;
                    for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
                    {
                        DataRow dr = dsTemp.Tables[0].Rows[i];
                        listbDupReel.Items.Add(dr["REEL_NO"].ToString());
                        fData.dgvSelectSN.Rows.Add();
                        fData.dgvSelectSN.Rows[fData.dgvSelectSN.Rows.Count - 1].Cells["REEL_NO"].Value = dr["REEL_NO"].ToString();
                        fData.dgvSelectSN.Rows[fData.dgvSelectSN.Rows.Count - 1].Cells["PART_NO"].Value = dr["PART_NO"].ToString();
                        fData.dgvSelectSN.Rows[fData.dgvSelectSN.Rows.Count - 1].Cells["VENDOR_NAME"].Value = dr["VENDOR_NAME"].ToString();
                        fData.dgvSelectSN.Rows[fData.dgvSelectSN.Rows.Count - 1].Cells["LOT"].Value = dr["LOT"].ToString();
                        fData.dgvSelectSN.Rows[fData.dgvSelectSN.Rows.Count - 1].Cells["DATECODE"].Value = dr["DATECODE"].ToString();
                        fData.dgvSelectSN.Rows[fData.dgvSelectSN.Rows.Count - 1].Cells["REEL_MEMO"].Value = dr["REEL_MEMO"].ToString();

                        //若料卷號重複,且其中一個與正要入的資料不一致,就不允許入重複的料卷號
                        if ((dr["PART_NO"].ToString() != editPartNo.Text) || (dr["VENDOR_CODE"].ToString() != editVendor.Text) || (dr["LOT"].ToString() != editLot.Text)
                             || (dr["DATECODE"].ToString() != editDateCode.Text))
                        {
                            fData.dgvSelectSN.Rows[fData.dgvSelectSN.Rows.Count - 1].Cells["REEL_NO"].Style.BackColor = Color.Red;
                            fData.dgvSelectSN.Rows[fData.dgvSelectSN.Rows.Count - 1].Cells["REEL_NO"].Style.ForeColor = Color.White;
                            //fData.dgvSelectSN.Rows[fData.dgvSelectSN.Rows.Count - 1].Cells["CHECKED"].Value = "Y";
                            fData.btnOK.Enabled = false;
                        }

                    }
                    if (fData.ShowDialog() != DialogResult.OK)
                    {
                        return false;
                    }
                }
                finally
                {
                    fData.Dispose();
                }
            }
            return true;
        }
        private string GetReelNo(string sPrefix, string sReelNO)
        {
            //string sSeqName="SAJET.S_REEL_CODE_"+sPrefix;
            string sSeqName = "SAJET.S_REEL_CODE";
            sSQL = "SELECT TRIM(LPAD(" + sSeqName + ".NEXTVAL,4,'0')) SEQ from dual ";
            DataSet dsSeq = ClientUtils.ExecuteSQL(sSQL);
            return sReelNO + dsSeq.Tables[0].Rows[0]["SEQ"].ToString();
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            editRTNO.Text = editRTNO.Text.Trim();
            editDateCode.Text = editDateCode.Text.Trim();
            editLot.Text = editLot.Text.Trim();
            editPartNo.Text = editPartNo.Text.Trim();
            editVendor.Text = editVendor.Text.Trim();
            editReel.Text = editReel.Text.Trim();
            editFix.Text = editFix.Text.Trim();
            editMemo.Text = editMemo.Text.Trim();
            string sReelNo = string.Empty;
            string sEnd = string.Empty;
            ListBox listbDupReel = new ListBox();//儲放所有重複的Reel

            if (!CheckData())
                return;
            int iUnitQty = Convert.ToInt32(editUnitQty.Value);
            int iReelQty = Convert.ToInt32(editReelQty.Value);

            //            int iSeqLen = 5; //流水號長度
            int iSeqLen = Convert.ToInt32(editSeqLength.Value);
            int iSeq = 0;
            string sFix = "";
            if (rdbtnAuto.Checked)
            {
                //Reel No   
                sFix = editFix.Text; //前置碼長度
                if (string.IsNullOrEmpty(sFix))
                {
                    SajetCommon.Show_Message("Please Input Prefix", 0);
                    editFix.Focus();
                    return;
                }
                /*
                if (iSeqLen == 0)
                {
                    SajetCommon.Show_Message("Seq Length must more than 0", 0);
                    editSeqLength.Focus();                  
                    return;
                }
                 */ 
            }
            else
            {
                //檢查起始號流水號是否符合十進位
                string sStartNo = editFix.Text;
                if (string.IsNullOrEmpty(sStartNo))
                {
                    SajetCommon.Show_Message("Please Input Start No", 0);
                    editFix.Focus();
                    return;
                }
                if (iSeqLen > 0)
                {
                    if (sStartNo.Length < iSeqLen)
                    {
                        SajetCommon.Show_Message("Length of Start No less then Seq Length", 0);
                        editFix.Focus();
                        return;
                    }


                    sFix = sStartNo.ToString().Substring(0, sStartNo.Length - iSeqLen);
                    int iStartIdx = sStartNo.Length - iSeqLen;
                    string sSeq = sStartNo.ToString().Substring(iStartIdx, iSeqLen);
                    try
                    {
                        Convert.ToInt32(sSeq);
                    }
                    catch
                    {
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("Start No") + " : " + sStartNo + Environment.NewLine + Environment.NewLine
                                                + SajetCommon.SetLanguage("Sequence No Error") + " : " + sSeq, 0);
                        editFix.Focus();
                        editFix.SelectAll();
                        return;
                    }
                    iSeq = Convert.ToInt32(sSeq);
                }
                else
                {
                    sFix = sStartNo;
                    iReelQty = 1;
                }
                sReelNo = sFix;
                int iEndNo = iSeq + iReelQty - 1;
                sEnd = sFix;
                if (iSeqLen > 0)
                {
                    sReelNo = sFix + iSeq.ToString().PadLeft(iSeqLen, '0');
                    sEnd = sFix + iEndNo.ToString().PadLeft(iSeqLen, '0');
                }

                if (chkbLength.Checked)
                {
                    if (sReelNo.Length != editStdLen.Value)
                    {
                        SajetCommon.Show_Message("Reel No Total Length Error", 0);
                        editFix.Focus();
                        editFix.SelectAll();
                        return;
                    }
                }
                if (iEndNo.ToString().Length > iSeqLen && iSeqLen > 0)
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("End No") + " : " + sEnd + Environment.NewLine + Environment.NewLine
                                           + SajetCommon.SetLanguage("Length of Sequence can't exceed ") + " ( " + iSeqLen.ToString() + " ) ", 0);
                    return;
                }
                //檢查Reel是否已存在

                if (!CheckDupReelNo(editPartNo.Text, editVendor.Text, editDateCode.Text, editLot.Text, ref listbDupReel))
                    return;
            }

            SaveCfgToIni();


          
            if (g_sRTID != "0")
            {
                //檢查是否超過RT數量
                int iStockQty = Convert.ToInt32(GetInventoryQty());// GetStockQty(g_sRTID, g_sPartID);
                int iReceiveQty = Convert.ToInt32(LabIncomingQty.Text);
                if (lablIQCResult.Text != "N/A")
                    iReceiveQty = Convert.ToInt32(lablIQCReceiveQty.Text);
                int iAvailableQty = iReceiveQty - iStockQty;
                if (iAvailableQty < 0)
                    iAvailableQty = 0;

                if ((iUnitQty * iReelQty) > iAvailableQty)
                {
                    //2016/08/16 June: Stock in Qty can't exceed RT
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Input Qty", 1) + " : " + Convert.ToString(iUnitQty * iReelQty) + Environment.NewLine
                                                + SajetCommon.SetLanguage("Over RT Available Input Qty", 1) + " : " + Convert.ToString(iAvailableQty) , 0);
                    editQR.Focus();
                    editQR.SelectAll();
                    return;
                    //if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Input Qty", 1) + " : " + Convert.ToString(iUnitQty * iReelQty) + Environment.NewLine
                    //                            + SajetCommon.SetLanguage("Over RT Available Input Qty", 1) + " : " + Convert.ToString(iAvailableQty) + Environment.NewLine
                    //                            + SajetCommon.SetLanguage("Continue ?", 1), 2) != DialogResult.Yes)
                    //{
                    //    return;
                    //}
                }
            }
            string sTotalQty = Convert.ToString(iUnitQty * iReelQty);
            string sMsg = SajetCommon.SetLanguage("Stock In", 1);
            if (SajetCommon.Show_Message(sMsg + " ?" + Environment.NewLine 
                                       + SajetCommon.SetLanguage("Unit Qty", 1) + " : " + iUnitQty.ToString() + Environment.NewLine + Environment.NewLine
                                       + SajetCommon.SetLanguage("Reel Qty", 1) + " : " + iReelQty.ToString() + Environment.NewLine + Environment.NewLine
                                       + SajetCommon.SetLanguage("Total Qty", 1) + " : " + sTotalQty, 2) != DialogResult.Yes)
            {
                return;
            }


            //if (radioInput.Checked)   //Mark for自動也需要檢查
            //{
                //寫入資料庫前,再檢查一次
                if (!CheckDupReelNo(editPartNo.Text, editVendor.Text, editDateCode.Text, editLot.Text, ref listbDupReel))
                    return;
            //}
            listPrintSN.Items.Clear();
            g_dtSysdate = ClientUtils.GetSysDate();
            string sFirstReel = "";
            string sEndReel = "";
            for (int i = 0; i <= iReelQty - 1; i++)
            {
                if (radioInput.Checked)
                {
                    if (iSeqLen > 0)
                    {
                        sReelNo = sFix + iSeq.ToString().PadLeft(iSeqLen, '0');
                    }
                    else
                    {
                        sReelNo = sFix;
                    }
                }
                else
                {
                    //用sequence產生流水號
                    if (listbDupReel.Items.Count == 0)
                    {
                        sReelNo = GetReelNo(combPrefix.Text, editFix.Text); //沒有重複才需要再取號    2016/07/15
                    }
                    else
                    {
                        sReelNo = listbDupReel.Items[0].ToString(); //有重複先用第一筆  2016/07/15
                    }
                }
                //if (listbDupReel.Items.IndexOf(sReelNo) < 0) //REEL沒有重複必須寫入資料庫
                if (listbDupReel.Items.Count < 1) //4個Key沒有重複必須寫入資料庫
                {
                    if (i == 0)
                        sFirstReel = sReelNo;
                    if (i == (iReelQty - 1))
                        sEndReel = sReelNo;
                    InsertReelNo(sReelNo);
                }
                else //REEL重複,不寫入資料庫,但寫入另一TABLE備查
                {
                    InsertDupAccept(sReelNo);
                }
                LVRT.Items.Add(sReelNo);
                LVRT.Items[LVRT.Items.Count - 1].SubItems.Add(editPartNo.Text);
                LVRT.Items[LVRT.Items.Count - 1].SubItems.Add(iUnitQty.ToString());
                LVRT.Items[LVRT.Items.Count - 1].SubItems.Add(editDateCode.Text);
                LVRT.Items[LVRT.Items.Count - 1].SubItems.Add(editLot.Text);
                LVRT.Items[LVRT.Items.Count - 1].SubItems.Add(editVendor.Text + " " + LabVendorName.Text);
                LVRT.Items[LVRT.Items.Count - 1].SubItems.Add(ClientUtils.fUserName);
                LVRT.Items[LVRT.Items.Count - 1].SubItems.Add(g_dtSysdate.ToString());
                LVRT.Items[LVRT.Items.Count - 1].SubItems.Add(editMemo.Text);
                listPrintSN.Items.Add(sReelNo);
                iSeq = iSeq + 1;
            }
            if (rdbtnAuto.Checked)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Create Reel Finish")+Environment.NewLine+Environment.NewLine
                                        + SajetCommon.SetLanguage("Reel No Range") + " : " +Environment.NewLine
                                        + sFirstReel + " ~ " +Environment.NewLine
                                        + sEndReel, 3);
            }
            gbChange.Text = g_sRTRecord + " : " + LVRT.Items.Count.ToString();
            editFix.Text = "";
            editFix.Focus();  

            if (chkbPrint.Checked)
                PrintReel();
            if (g_sRTID != "0")
                combRTSeq_SelectedIndexChanged(sender, e);
            rdbtnAuto_CheckedChanged(sender, e);

            editQR.Focus(); //2016/08/16
            editQR.SelectAll();  //2016/08/16 for next QR code input
        }
        /*
        private string GetMaxReelSeq(int iFixLen,int iSeqLen)
        {            
            
            int iTotlaLen =  iFixLen+ iSeqLen;
            string sCon = editFix.Text.PadRight(iTotlaLen, '_' );

            sSQL = " Select NVL(Max(reel_no),'N/A') maxno from sajet.g_material "
                 + " where length(reel_no) = " + iTotlaLen.ToString() + " "
                 + " and reel_no like '" + sCon + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0 || dsTemp.Tables[0].Rows[0]["maxno"].ToString() == "N/A")
                return "0";
            else
            {
                string sMaxNo = dsTemp.Tables[0].Rows[0]["maxno"].ToString();
                string sSeq = sMaxNo.Substring(iFixLen, iSeqLen);
                return sSeq;
            }
        }
         */ 
   
        private void fMain_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);

            g_sUserID = ClientUtils.UserPara1;
            g_sFunction = ClientUtils.fFunctionName;
            g_sProgram = ClientUtils.fProgramName;
            g_sExeName = ClientUtils.fCurrentProject;
            this.Text = this.Text + " (" + SajetCommon.g_sFileVersion + ")";
            btnExecute.BackColor = Color.Green;
            btnPrint.BackColor = Color.Green;
            check_privilege();
            g_sRTRecord = gbChange.Text;
            ClearPartData();
            rdbtnAuto.Checked = true;

            ReadCfgFromIni();
            ReadLabelType();
            TPrint.sPrintMethod = "BARTENDER";
            TPrint.sPrintPort = "DATASOURCE";
            TPrint.iPrintQty = 1;

            tabControl1.TabPages.Remove(tabPage3);  //Zinwell not use Reel Division->no unique reel No.

        }
        private void ReadLabelType()
        {
            combLabelType.Items.Clear();
            sSQL = "SELECT * FROM SAJET.SYS_BASE "
                + " WHERE PROGRAM=:PROGRAM "
                + "   AND PARAM_NAME='Label Type' ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROGRAM", g_sProgram };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            //for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                string[] sParamValue = dsTemp.Tables[0].Rows[0]["PARAM_VALUE"].ToString().Trim().Split(new Char[] { ';' });
                int iIndex = sParamValue.Length / 2;
                for (int i = 0; i <= iIndex - 1; i++)
                {
                    combLabelType.Items.Add(SajetCommon.SetLanguage(sParamValue[i * 2]));
                    combLabelTypeFile.Items.Add(sParamValue[i * 2 + 1]);
                }
            }
            else
            {
                combLabelType.Items.Add(SajetCommon.SetLanguage("Large"));
                combLabelTypeFile.Items.Add("REEL_DEFAULT");
                combLabelType.Items.Add(SajetCommon.SetLanguage("Small"));
                combLabelTypeFile.Items.Add("REEL_DEFAULT_S");
            }
            combLabelType.SelectedIndex = 0;
            combLabelTypeFile.SelectedIndex = 0;
        }
        public void check_privilege()
        {
            btnExecute.Enabled = false;
            int iPrivilege = ClientUtils.GetPrivilege(g_sUserID, g_sFunction, g_sProgram);
            btnExecute.Enabled = (iPrivilege >= 1);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            editPartNo.Text = editPartNo.Text.Trim();
            if (string.IsNullOrEmpty(editPartNo.Text))
            {
                SajetCommon.Show_Message("Please Input Part No Prefix", 0);
                editPartNo.Focus();
                editPartNo.SelectAll();
                return;
            }
            sSQL = "Select Part_No,Spec1,Spec2 from sajet.sys_part "
                 + "Where enabled = 'Y' ";
            if (editPartNo.Text != "")
                sSQL = sSQL + "and Part_No like '" + editPartNo.Text + "%'";
            sSQL = sSQL + "order by part_no ";

            fFilter f = new fFilter();           
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                if (f.dgvData.CurrentRow != null)
                {
                    editPartNo.Text = f.dgvData.CurrentRow.Cells["Part_No"].Value.ToString();
                    KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);
                    editPartNo_KeyPress(sender, Key);
                }
            }
        }

        private void editPartNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            string sPartNo = editPartNo.Text.Trim();
            //ClearPartData();  //Zinwell use QR Code input, other data need keep 2016/07/13 Ronald
            editPartNo.Text = sPartNo;
            sSQL = "SELECT PART_ID,PART_NO,SPEC1 "
                + "   FROM SAJET.SYS_PART "
                + "  WHERE PART_NO='" + editPartNo.Text + "' "
                + "    AND ROWNUM = 1 ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count ==0)
            //string sPartID = SajetCommon.GetID("SAJET.SYS_PART", "PART_ID", "PART_NO", editPartNo.Text);
            //if (sPartID == "0")
            {
                SajetCommon.Show_Message("Part No Error", 0);                
                editPartNo.Focus();
                editPartNo.SelectAll();
                return;
            }
            lablSpec.Text = dsTemp.Tables[0].Rows[0]["SPEC1"].ToString();

            editVendor.Focus();
            editVendor.SelectAll();
        }
        
        private void ClearPartData()
        {            
            editDateCode.Text = "";
            editLot.Text = "";
            editVendor.Text = "";
            editPartNo.Text = "";
            LabVendorName.Text = string.Empty;
            lablSpec.Text = string.Empty;
            //LVRT.Items.Clear();          
            editUnitQty.Value = 1;
         //   editReelQty.Value = 1;

            editVendorPartNo.Text = ""; //  V1.16004.0.1

        //    editReel.Text = "";
        //    editFix.Text = "";
            gbChange.Text = g_sRTRecord;

            lablIQCResult.Text = "N/A";
            lablInvQty.Text = "0";
            lablIQCReceiveQty.Text = "0";
            LabIncomingQty.Text = "0";
            lablPONo.Text = "N/A";
            lablRTReceiveDate.Text = "N/A";
            editPartNo.Enabled = true;
            editVendor.Enabled = true;
            btnSearch.Enabled = true;
            btnSearchVendor.Enabled = true;
            lablIQCResult.ForeColor = Color.Maroon;
            lablIQCResult.BackColor = gbRT.BackColor;
            editRTPart.Text = string.Empty;
        }
        private void combRTNo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void combPart_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            ClearPartData();
            editPartNo.Text = "";

            sSQL = " select a.rt_id,b.part_id,a.rt_no,a.po_no "
                 + "       ,b.part_id,b.DATECODE,b.incoming_qty "
                 + "       ,c.part_no,d.vendor_code "
                 + " from sajet.g_erp_rtno a "
                 + "     ,sajet.g_erp_rt_Item b "
                 + "     ,sajet.sys_part c "
                 + "     ,sajet.sys_vendor d "
                 + " where a.rt_no = '" + combRTNo.Text + "'  "
                 + " and c.part_no = '" + combPart.Text + "'  "
                 //+ " and b.RT_SEQ = '" + combRTSeq.Items[combPart.SelectedIndex].ToString() + "' "
                 + " and a.rt_id = b.rt_id(+) "
                 + " and b.part_id = c.part_id(+) "
                 + " and a.vendor_id = d.vendor_id(+) "
                 + " order by c.part_no,b.RT_SEQ ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {                
                editDateCode.Text = dsTemp.Tables[0].Rows[0]["DATECODE"].ToString();
                LabInQty.Text = dsTemp.Tables[0].Rows[0]["incoming_qty"].ToString();
                editVendor.Text = dsTemp.Tables[0].Rows[0]["vendor_code"].ToString();                
               
                KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);
                editVendor_KeyPress(sender, Key);
            }
            
            editUnitQty.Focus();
             */ 
        }

        private void editPartNo_VisibleChanged(object sender, EventArgs e)
        {
            btnSearch.Visible = editPartNo.Visible;
        }

        private int GetStockQty(string sRTID, string sPartID)
        {
            int iQty = 0;
            //已入庫數量
            sSQL = "select nvl(sum(reel_qty),0) reelqty from sajet.g_material "
                 + "where rt_id = '" + sRTID + "' "
                 + "and part_id = '" + sPartID + "' ";                 
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            iQty = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["reelqty"].ToString());
            return iQty;
        }

        private void btnSearchVendor_Click(object sender, EventArgs e)
        {
            sSQL = "Select Vendor_Code,Vendor_Name,Vendor_Desc from sajet.sys_vendor "
                 + "Where enabled = 'Y' ";
            if (editVendor.Text != "")
                sSQL = sSQL + "and Vendor_Code like '" + editVendor.Text + "%'";
            sSQL = sSQL + "order by Vendor_Code ";

            fFilter f = new fFilter();
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                if (f.dgvData.CurrentRow != null)
                {
                    editVendor.Text = f.dgvData.CurrentRow.Cells["Vendor_Code"].Value.ToString();
                    KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);
                    editVendor_KeyPress(sender, Key);
                }
            }
        }

        private void editVendor_KeyPress(object sender, KeyPressEventArgs e)
        {
            LabVendorName.Text = "N/A";
            if (e.KeyChar != (char)Keys.Return)
                return;

            sSQL = "Select Vendor_Name from sajet.sys_vendor "
                 + "Where Vendor_Code = '" + editVendor.Text + "' "
                 + "order by Vendor_Code ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                SajetCommon.Show_Message("Vendor Code Error",0);
                editVendor.Focus();
                editVendor.SelectAll();
                return;
            }
            LabVendorName.Text = dsTemp.Tables[0].Rows[0]["Vendor_Name"].ToString();
            combPrefix.Items.Clear();
            combPrefix.Items.Add(editVendor.Text);
            combPrefix.SelectedIndex = 0;
            editDateCode.Focus();
            editDateCode.SelectAll();
            //editWarehouse.Focus();
            //editWarehouse.SelectAll();

        }

        private void editReel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            editRTNO.Text = editRTNO.Text.Trim();
            editDateCode.Text = editDateCode.Text.Trim();
            editLot.Text = editLot.Text.Trim();
            editPartNo.Text = editPartNo.Text.Trim();
            editVendor.Text = editVendor.Text.Trim();
            editReel.Text = editReel.Text.Trim();

            if (!CheckData())
                return;


            string sReelNo = editReel.Text;
            sSQL = "Select * from SAJET.G_MATERIAL "
                 + "Where REEL_NO = '" + sReelNo + "' "
                 + "  and ROWNUM = 1 ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                SajetCommon.Show_Message("Reel Duplicate",0);
                editReel.Focus();
                editReel.SelectAll();
                return;
            }


            int iUnitQty = Convert.ToInt32(editUnitQty.Value);
            int iReelQty = 1;
            //RT     
            if (g_sRTID != "0")
            //if (combRTNo.SelectedIndex > 0)
            {
                //檢查是否超過RT數量
                int iStockQty = Convert.ToInt32(GetInventoryQty());// GetStockQty(g_sRTID, g_sPartID);
                int iReceiveQty = Convert.ToInt32(LabIncomingQty.Text);
                if (lablIQCResult.Text != "N/A")
                    iReceiveQty = Convert.ToInt32(lablIQCReceiveQty.Text);
                int iAvailableQty = iReceiveQty - iStockQty;
                if (iAvailableQty < 0)
                    iAvailableQty = 0;

                if ((iUnitQty * iReelQty) > iAvailableQty)
                {
                    if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Input Qty", 1) + " : " + Convert.ToString(iUnitQty * iReelQty) + Environment.NewLine
                                                + SajetCommon.SetLanguage("Over RT Available Input Qty", 1) + " : " + Convert.ToString(iAvailableQty) + Environment.NewLine
                                                + SajetCommon.SetLanguage("Continue ?", 1), 2) != DialogResult.Yes)
                    {
                        return;
                    }
                }
            }
            

            g_dtSysdate = ClientUtils.GetSysDate();
            InsertReelNo(sReelNo);

            LVRT.Items.Add(sReelNo);
            LVRT.Items[LVRT.Items.Count - 1].SubItems.Add(editPartNo.Text);
            LVRT.Items[LVRT.Items.Count - 1].SubItems.Add(editUnitQty.Text.ToString());
            LVRT.Items[LVRT.Items.Count - 1].SubItems.Add(editDateCode.Text);
            LVRT.Items[LVRT.Items.Count - 1].SubItems.Add(editLot.Text);
            LVRT.Items[LVRT.Items.Count - 1].SubItems.Add(editVendor.Text + " " + LabVendorName.Text);
            LVRT.Items[LVRT.Items.Count - 1].SubItems.Add(ClientUtils.fUserName);
            LVRT.Items[LVRT.Items.Count - 1].SubItems.Add(g_dtSysdate.ToString());

            gbChange.Text = g_sRTRecord + " : " + LVRT.Items.Count.ToString();

            listPrintSN.Items.Clear();
            listPrintSN.Items.Add(sReelNo);
            if (chkbPrint.Checked)
                PrintReel();
            editReel.Focus();
            editReel.SelectAll();
        }

        private Boolean CheckData()
        {
            //Part
            string sPart = editPartNo.Text;
            g_sPartID = SajetCommon.GetID("SAJET.SYS_PART", "PART_ID", "PART_NO", sPart);
            if (g_sPartID == "0")
            {
                SajetCommon.Show_Message("Part No Error", 0);
                editPartNo.SelectAll();
                editPartNo.Focus();
                return false;
            }
            //Vendor
            g_sVendorID = "0";
            g_sVendorID = SajetCommon.GetID("SAJET.SYS_VENDOR", "VENDOR_ID", "VENDOR_CODE", editVendor.Text);
            if (g_sVendorID == "0")
            {
                SajetCommon.Show_Message("Vendor Code Error", 0);
                editVendor.SelectAll();
                editVendor.Focus();
                return false;
            }
            g_sWarehouseID = "0";
            g_sWarehouseID = SajetCommon.GetID("SAJET.SYS_WAREHOUSE", "WAREHOUSE_ID", "WAREHOUSE_NAME", editWarehouse.Text, "Y");
            if (g_sWarehouseID == "0")
            {
                SajetCommon.Show_Message("Warehouse Error", 0);
                editWarehouse.SelectAll();
                editWarehouse.Focus();
                return false;
            }
            if (editDateCode.Text == "" && editLot.Text == "")
            {
                SajetCommon.Show_Message("Please Input DateCode / Lot", 0);
                editDateCode.Focus();
                return false;
            }
            /*
            if (editDateCode.Text == "")
            {
                string sMsg = SajetCommon.SetLanguage("Please Input DateCode", 1);
                SajetCommon.Show_Message(sMsg, 0);
                editDateCode.SelectAll();
                editDateCode.Focus();
                return false;
            }
            if (editLot.Text == "")
            {
                string sMsg = SajetCommon.SetLanguage("Please Input Lot", 1);
                SajetCommon.Show_Message(sMsg, 0);
                editLot.SelectAll();
                editLot.Focus();
                return false;
            }
             */


            //RT
            g_sRTID = "0";
            if (!string.IsNullOrEmpty(editRTNO.Text))
            //  if (combRTNo.SelectedIndex > 0)
            {
                object[][] Params = new object[1][];
                sSQL = "SELECT RT_ID,RECEIVE_TIME "
                    + "  FROM SAJET.G_ERP_RTNO "
                    + " WHERE RT_NO =:RT_NO "
                    + "   AND ROWNUM = 1 ";
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RT_NO", editRTNO.Text };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                if (dsTemp.Tables[0].Rows.Count == 0)
                {
                    SajetCommon.Show_Message("RT No Error", 0);
                    editRTNO.SelectAll();
                    editRTNO.Focus();
                    return false;
                }
                g_sRTID = dsTemp.Tables[0].Rows[0]["RT_ID"].ToString();

                /*
                g_sRTID = SajetCommon.GetID("SAJET.G_ERP_RTNO", "RT_ID", "RT_NO", editRTNO.Text);
                if (g_sRTID == "0")
                {
                    SajetCommon.Show_Message("RT No Error", 0);
                    editRTNO.SelectAll();
                    editRTNO.Focus();
                    return false;
                }
                 */
            }
            return true;

        }
        private void InsertDupAccept(string sReelNo)
        {
            try
            {
                object[][] Params = new object[4][];
                sSQL = "Insert Into SAJET.G_MATERIAL_REINPUT "
                     + "(REEL_NO,UPDATE_USERID,UPDATE_TIME,REEL_MEMO ) "
                     + "Values "
                     + "(:REEL_NO,:UPDATE_USERID,:UPDATE_TIME,:REEL_MEMO) ";
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "REEL_NO", sReelNo };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", ClientUtils.UserPara1 };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "REEL_MEMO", editMemo.Text };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception:" + ex.Message, 0);
            }
        }
        private void InsertReelNo(string sReelNo)
        {
            try
            {
                DateTime dtRTReceiveDate = g_dtSysdate;
                if (lablRTReceiveDate.Text != "N/A")
                {
                    dtRTReceiveDate = Convert.ToDateTime(lablRTReceiveDate.Text);
                }
                object[][] Params = new object[13][];
                sSQL = "Insert Into SAJET.G_MATERIAL "
                     + "(RT_ID,PART_ID,DATECODE,REEL_NO,REEL_QTY,VENDOR_ID,LOT,UPDATE_USERID,UPDATE_TIME,RT_RECEIVE_TIME,REEL_MEMO,WAREHOUSE_ID, VENDOR_PART_NO ) "
                     + "Values "
                     + "(:RT_ID,:PART_ID,:DATECODE,:REEL_NO,:REEL_QTY,:VENDOR_ID,:LOT,:UPDATE_USERID,:UPDATE_TIME,:RT_RECEIVE_TIME,:REEL_MEMO,:WAREHOUSE_ID,:VENDOR_PART_NO) ";
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RT_ID", g_sRTID };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", g_sPartID };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DATECODE", editDateCode.Text };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "REEL_NO", sReelNo };
                Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "REEL_QTY", editUnitQty.Value.ToString() };
                Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_ID", g_sVendorID };
                Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT", editLot.Text };
                Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                Params[8] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                Params[9] = new object[] { ParameterDirection.Input, OracleType.DateTime, "RT_RECEIVE_TIME", dtRTReceiveDate };
                Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "REEL_MEMO", editMemo.Text };
                Params[11] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WAREHOUSE_ID", g_sWarehouseID};
                Params[12] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_PART_NO", editVendorPartNo.Text };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception:" + ex.Message, 0);
            }
        }        

        private void rdbtnAuto_CheckedChanged(object sender, EventArgs e)
        {
            /*
            panelAuto.Visible = false;
            panelInput.Visible = false;
             */
            editFix.Focus();
          //  editFix.ReadOnly = false;
            
            ReadCfgFromIni();
            panelManual.Visible = radioInput.Checked;
            if (rdbtnAuto.Checked)
            {
           //     editFix.ReadOnly = true;
                
                lablPrefix.Text = SajetCommon.SetLanguage("Prefix");
                chkbLength.Checked = true;
/*                SajetInifile sajetInifile1 = new SajetInifile();
                
                string sPreFix = sajetInifile1.ReadIniFile(g_sIniFile, g_sProgram, "Prefix", "TP");
                sPreFix = sPreFix + DateTime.Now.ToString("yyyyMM");
                editFix.Text = sPreFix;
 */ 
            }
            else
            {
                
               // panelInput.Visible = true;
               // editReel.Focus();
                lablPrefix.Text = SajetCommon.SetLanguage("Start No");
                editFix.Text = "";
                //以下三行mark by Rita 2009/12/23
            //    editSeqLength.Value = 3;
            //    editStdLen.Value = 16;
            //    chkbLength.Checked = false;
            }
             
            
            
            editFix.Focus();
        }

        private void editVendor_EnabledChanged(object sender, EventArgs e)
        {
            btnSearchVendor.Enabled = editVendor.Enabled;
        }

        private void fMain_Shown(object sender, EventArgs e)
        {
           // editPartNo.Focus();
            //editRTNO.Focus();
            //editWarehouse.Focus();
            editQR.Focus();
        }
        private string LoadBatFile(string sFile)
        {
            
            if (!File.Exists(sFile))
            {
                SajetCommon.Show_Message("File not exist - " + sFile, 0);
                return "";
            }
            System.IO.StreamReader sr = new System.IO.StreamReader(sFile);
            string sValue = string.Empty;
            try
            {
                sValue = sr.ReadLine().Trim();
            }
            finally
            {
                sr.Close();
            }
            return sValue;
        }
        private ListBox LoadFileHeader(string sFile)
        {
            ListBox ListParams = new ListBox();
            if (!File.Exists(sFile))
            {
                SajetCommon.Show_Message("File not exist - " + sFile, 0);
                // return "";
            }
            else
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(sFile);
                try
                {
                    string[] sValue = sr.ReadLine().Trim().Split(new Char[] { ',' });
                    for (int i = 0; i <= sValue.Length - 1; i++)
                    {
                        ListParams.Items.Add(sValue[i].ToString());

                    }
                }
                finally
                {
                    sr.Close();
                }
            }
            return ListParams;
        }
        private void WriteToPrintGo(string sFile ,string sData)
        {   try
            {
                if (File.Exists(sFile))
                {
                    File.Delete(sFile);
                }
                System.IO.File.AppendAllText(sFile,sData,Encoding.Default);
            }
            finally
            {
            }
        }
        private void WriteToTxt(string sFile, string sData)
        {
            StreamWriter sw = null;
            try
            {
                if (File.Exists(sFile))
                {
                    sw = new StreamWriter(sFile, true, Encoding.UTF8);
                }
                else
                {
                    sw = File.CreateText(sFile);
                }
                sw.WriteLine(sData);
            }
            finally
            {
                sw.Close();
            }
        }
        /*
        //=============DLL=============================================
        [DllImport("kernel32.dll")]
        public static extern int WinExec(string exeName, int operType);
        private void PrintReel()
        {
            int iPrintQty = Convert.ToInt32(editPrintQty.Value);
            string sMessage = "";
            ListBox ListParam = new ListBox();
            ListBox ListData = new ListBox();

            string sPrintMethod = "DLL";
            string sPrintPort = "BarTender";
            //string sPrintMethod = "CodeSoft";
            //string sPrintPort = "CodeSoft";
            //PrintLabel.Setup PrintLabelDll = new PrintLabel.Setup();
            //PrintLabelDll.Open(sPrintPort.ToUpper());
            string sLabelTypeFile = combLabelTypeFile.Text;
            string sLabelFileName = Application.StartupPath + "\\" + g_sExeName + "\\" + sLabelTypeFile + ".btw";
            string sDataSourceFile = Application.StartupPath + "\\" + g_sExeName + "\\" + sLabelTypeFile + ".lst";
            string sLabelFieldFile = Application.StartupPath + "\\" + g_sExeName + "\\" + sLabelTypeFile + ".dat";
            string sPrintGoFile =Application.StartupPath + "\\" + g_sExeName + "\\"+"PrintGo.bat";
            string sBatFile = Application.StartupPath + "\\" + g_sExeName + "\\PrintLabel.bat";
            try
            {
                if (!File.Exists(sLabelFileName))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Label File Not Found") + " (.btw)" + Environment.NewLine + Environment.NewLine
                                            + sLabelFileName, 0);
                    return;
                }
                if (!File.Exists(sLabelFieldFile))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Label File Not Found") + " (.dat)" + Environment.NewLine + Environment.NewLine
                                            + sLabelFieldFile, 0);
                    return;
                }
                if (!File.Exists(sBatFile))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("File Not Found") + " (PrintLabel.bat)" + Environment.NewLine + Environment.NewLine
                                            + sBatFile, 0);
                    return;
                }
                if (File.Exists(sDataSourceFile))
                    File.Delete(sDataSourceFile);

                ListParam.Items.Clear();
                int iIndex;
                ListParam.Items.Add("REEL_NO");
                ListParam.Items.Add("DATECODE");
                ListParam.Items.Add("PART_NO");
                ListParam.Items.Add("VENDOR_CODE");
                ListParam.Items.Add("VENDOR_NAME");
                ListParam.Items.Add("RT_NO");
                ListParam.Items.Add("UNIT_QTY");                
                ListParam.Items.Add("LOT");
                string sData = string.Empty;
                ListBox listField = LoadFileHeader(sLabelFieldFile);
                //==================表頭資料================================
                for (int j = 0; j <= listField.Items.Count - 1; j++)
                {
                    sData = sData + listField.Items[j].ToString() + ",";
                }
                if (!string.IsNullOrEmpty(sData))
                {
                    sData = sData.Substring(0, sData.Length - 1);
                }
                //
                WriteToTxt(sDataSourceFile, sData);
    
                for (int i = 0; i <= listPrintSN.Items.Count - 1; i++)
                {
                    ListData.Items.Clear();
                    ListData.Items.Add(listPrintSN.Items[i].ToString());
                    ListData.Items.Add(editDateCode.Text);
                    ListData.Items.Add(editPartNo.Text);
                    ListData.Items.Add(editVendor.Text);
                    ListData.Items.Add(LabVendorName.Text);
                    ListData.Items.Add(editRTNO.Text);
                    ListData.Items.Add(editUnitQty.Value.ToString());
                    
                    ListData.Items.Add(editLot.Text);
                    sData = string.Empty;
                    //=================欄位對應的值===============================
                    for (int j = 0; j <= listField.Items.Count - 1; j++)
                    {
                        string sField = listField.Items[j].ToString();
                        iIndex = ListParam.Items.IndexOf(sField);
                        if (iIndex >= 0)
                        {
                            sData = sData + ListData.Items[iIndex].ToString() + ",";
                        }
                        else
                            sData = sData + ",";//若找不到則給空值
                            
                    }
                    if (!string.IsNullOrEmpty(sData))
                    {
                        sData = sData.Substring(0, sData.Length - 1);
                    }
                    WriteToTxt(sDataSourceFile,sData);
                    //=========================================================
                }

                string sPrintCommand = LoadBatFile(sBatFile);
                iIndex = sPrintCommand.IndexOf("@PATH1");
                if (iIndex >= 0)
                {
                    sPrintCommand = sPrintCommand.Replace("@PATH1", '"' + sLabelFileName + '"');
                }
                iIndex = sPrintCommand.IndexOf("@PATH2");
 
                if (iIndex >= 0)
                {
                    sPrintCommand = sPrintCommand.Replace("@PATH2", '"' + sDataSourceFile + '"');
                }
                iIndex = sPrintCommand.IndexOf("@QTY");
                if (iIndex >= 0)
                {
                    sPrintCommand = sPrintCommand.Replace("@QTY", iPrintQty.ToString());
                }
                WriteToPrintGo(sPrintGoFile, sPrintCommand);

                WinExec(sPrintGoFile, 0);
            }
            finally
            {
               // PrintLabelDll.Close(sPrintPort.ToUpper());
            }
        }
         */
        private bool PrintReel()
        {
            string sFile = Application.StartupPath + "\\" + g_sExeName + "\\Inventory.ini";
            //Sleep幾秒
            SajetInifile sajetInifile1 = new SajetInifile();
            int iSleepSec = Convert.ToInt32(sajetInifile1.ReadIniFile(sFile, "Print Label", "Delay Second", "0"));
            //每印幾張就Sleep一次
            int iPrintCount = Convert.ToInt32(sajetInifile1.ReadIniFile(sFile, "Print Label", "Print Count", "100"));
            int iPrintQty = Convert.ToInt32(editPrintQty.Value);
            //對應S_SYS_PRINT_DATA的DATA_TYPE欄位 
            string sDataType = "INV_REEL";
            TPrint.sPrintMethod = "BARTENDER";
            TPrint.sPrintPort = "DATASOURCE";

            //先Link====
            PrintLabel.Setup PrintLabelDll = new PrintLabel.Setup();
            if (TPrint.sPrintMethod.ToUpper() == "CODESOFT")
                PrintLabelDll.Open(TPrint.sPrintMethod.ToUpper());
            else if (TPrint.sPrintMethod.ToUpper() == "BARTENDER" && TPrint.sPrintPort.ToUpper() == "STANDARD")
                PrintLabelDll.Open(TPrint.sPrintMethod.ToUpper());
            else if (TPrint.sPrintMethod.ToUpper() == "DLL")
                PrintLabelDll.Open(TPrint.sPrintPort.ToUpper());
            ListBox ListParam = new ListBox();
            ListBox ListData = new ListBox();
            string sLabelType = combLabelTypeFile.Text;
            //Print=====
            try
            {
                //   progressBar1.Minimum = 0;
                //   progressBar1.Maximum = listbPrintSN.Items.Count - 1;
                //   progressBar1.Visible = true;
                if (TPrint.sPrintMethod.ToUpper() == "BARTENDER" && TPrint.sPrintPort.ToUpper() == "DATASOURCE")
                {
                    ListParam.Items.Clear();
                    ListData.Items.Clear();
                    string sMessage = "";
                    for (int i = 0; i <= listPrintSN.Items.Count - 1; i++)
                    {
                        ListData.Items.Add(listPrintSN.Items[i]);
                    }
                    PrintLabelDll.Print_Bartender_DataSource(g_sExeName, "INV_REEL", "REEL_", sLabelType, TPrint.iPrintQty, TPrint.sPrintMethod, TPrint.sPrintPort, ListParam, ListData, out sMessage);
                    if (sMessage != "OK")
                    {
                        SajetCommon.Show_Message(sMessage, 0);
                        return false;
                    }
                    return true;
                }

                for (int i = 0; i <= listPrintSN.Items.Count - 1; i++)
                {
                    string sMessage = "";
                    //因應一張Label有多個號碼,每幾個才去Print一次   
                    ListParam.Items.Clear();
                    ListData.Items.Clear();
                    ListData.Items.Add(listPrintSN.Items[i]);

                    //各變數值                        
                    PrintLabelDll.GetPrintData(sDataType, ref ListParam, ref ListData);
                    //開始列印
                    if (TPrint.sPrintMethod.ToUpper() == "CODESOFT")
                        PrintLabelDll.Print(g_sExeName, sDataType, sDataType, "", TPrint.iPrintQty, TPrint.sPrintMethod, TPrint.sCodeSoftVer, ListParam, ListData, out sMessage);
                    else
                        PrintLabelDll.Print(g_sExeName, sDataType, sDataType, "", TPrint.iPrintQty, TPrint.sPrintMethod, TPrint.sPrintPort, ListParam, ListData, out sMessage);
                    if (sMessage != "OK")
                    {
                        SajetCommon.Show_Message(sMessage, 0);
                        return false;
                    }

                    //為了避免CodeSoft列印會亂跳,每印100張就Sleep,等待CodeSoft清除Buffer
                    if ((iSleepSec > 0) && (i < listPrintSN.Items.Count - 1))
                    {
                        if ((i + 1) % iPrintCount == 0)
                            System.Threading.Thread.Sleep(iSleepSec * 1000);
                    }
                }
                return true;
            }
            finally
            {
                if (TPrint.sPrintMethod.ToUpper() == "CODESOFT")
                    PrintLabelDll.Close(TPrint.sPrintMethod.ToUpper());
                else if (TPrint.sPrintMethod.ToUpper() == "BARTENDER" && TPrint.sPrintPort.ToUpper() == "STANDARD")
                    PrintLabelDll.Close(TPrint.sPrintMethod.ToUpper());
                else if (TPrint.sPrintMethod.ToUpper() == "DLL")
                    PrintLabelDll.Close(TPrint.sPrintPort.ToUpper());
            }
        } 

        private void btnFilterRT_Click(object sender, EventArgs e)
        {
            editRTNO.Text = editRTNO.Text.Trim();
            string sRTNO = editRTNO.Text + "%";
            sSQL = " Select A.RT_NO,B.VENDOR_NAME "
                  + "   FROM SAJET.G_ERP_RTNO A "
                  + "       ,SAJET.SYS_VENDOR B "
                  + " WHERE A.RT_NO LIKE '" + sRTNO + "' "
                  + "   AND A.ENABLED='Y' "
                  + "   AND A.VENDOR_ID = B.VENDOR_ID(+) "
                  + " ORDER BY A.RT_NO ";
            fFilter f = new fFilter();
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                if (f.dgvData.CurrentRow != null)
                {
                    editRTNO.Text = f.dgvData.CurrentRow.Cells["RT_NO"].Value.ToString();
                    KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);
                    editRTNO_KeyPress(sender, Key);
                }
            }
        }

        private void editRTNO_KeyPress(object sender, KeyPressEventArgs e)
        {
  
            g_sRTID ="0";
            combRTSeq.Items.Clear();

            ClearPartData();
            if (e.KeyChar != (char)Keys.Return)
                return;
            string sRTNO = editRTNO.Text.Trim();
           sSQL=" SELECT  A.RT_ID,B.VENDOR_CODE,B.VENDOR_NAME,A.PO_NO,A.RECEIVE_TIME "
                +"   FROM SAJET.G_ERP_RTNO A "
                +"       ,SAJET.SYS_VENDOR B "
                +"  WHERE A.RT_NO =:RT_NO "
                +"    AND A.VENDOR_ID = B.VENDOR_ID(+) ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RT_NO", editRTNO.Text };
            dsTemp = ClientUtils.ExecuteSQL(sSQL,Params);
            if (dsTemp.Tables[0].Rows.Count ==0)
            {
                SajetCommon.Show_Message("RT No Error", 0);
                editRTNO.Focus();
                editRTNO.SelectAll();
                return;
            }

            string sRTID = dsTemp.Tables[0].Rows[0]["RT_ID"].ToString();
            editVendor.Text = dsTemp.Tables[0].Rows[0]["VENDOR_CODE"].ToString();
            LabVendorName.Text = dsTemp.Tables[0].Rows[0]["VENDOR_NAME"].ToString();
            lablPONo.Text = dsTemp.Tables[0].Rows[0]["PO_NO"].ToString();
            lablRTReceiveDate.Text = dsTemp.Tables[0].Rows[0]["RECEIVE_TIME"].ToString();
            g_sRTID = sRTID;
            sSQL=" SELECT  B.RT_SEQ "
                +"   FROM SAJET.G_ERP_RT_ITEM B "
                +"  WHERE B.RT_ID = :RT_ID "
                +"  ORDER BY B.RT_SEQ ";
            Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RT_ID", sRTID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL,Params);
            for (int i=0;i<=dsTemp.Tables[0].Rows.Count-1;i++)
            {
                combRTSeq.Items.Add(dsTemp.Tables[0].Rows[i]["RT_SEQ"].ToString());
            }
            if (combRTSeq.Items.Count > 0)
                combRTSeq.SelectedIndex = 0;
            editVendor.Enabled = false;
            btnSearchVendor.Enabled = false;
            /*
            if (string.IsNullOrEmpty(sRTNO))
            {

            }

            if (combRTNo.SelectedIndex <= 0)
            {
                combPart.Visible = false;
                gbRT.Visible = false;
                editPartNo.Visible = true;
                editVendor.Enabled = true;
                editPartNo.Focus();
            }
            else
            {
                combPart.Visible = true;
                gbRT.Visible = true;
                editPartNo.Visible = false;
                editVendor.Enabled = false;

                sSQL = " select a.rt_no,a.po_no,b.RT_SEQ,b.part_id,b.DATECODE,b.incoming_qty,c.part_no,d.vendor_code "
                     + " from sajet.g_erp_rtno a "
                     + "     ,sajet.g_erp_rt_Item b "
                     + "     ,sajet.sys_part c "
                     + "     ,sajet.sys_vendor d "
                     + " where a.rt_no = '" + combRTNo.Text + "'  "
                     + " and a.rt_id = b.rt_id(+) "
                     + " and b.part_id = c.part_id(+) "
                     + " and a.vendor_id = d.vendor_id(+) "
                     + " order by c.part_no,b.RT_SEQ ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
                {
                    combPart.Items.Add(dsTemp.Tables[0].Rows[i]["PART_NO"].ToString());
                    combRTSeq.Items.Add(dsTemp.Tables[0].Rows[i]["RT_SEQ"].ToString());
                }
                if (combPart.Items.Count == 1)
                    combPart.SelectedIndex = 0;
                combPart.Focus();
              
            }*/
        }

        private void combRTSeq_SelectedIndexChanged(object sender, EventArgs e)
        {
            lablIQCResult.Text = "N/A";
            lablInvQty.Text = "0";
            lablIQCReceiveQty.Text = "0";
            LabIncomingQty.Text = "0";
            editPartNo.Text = "";
            editDateCode.Text = "";
            lablSpec.Text = String.Empty;
            lablIQCResult.ForeColor = Color.Maroon;
            lablIQCResult.BackColor = gbRT.BackColor;


            combRTSeq.Text = combRTSeq.Text.Trim();
            if (string.IsNullOrEmpty(combRTSeq.Text))
                return;
            int iSeq = Convert.ToInt32(combRTSeq.Text);
            sSQL = "SELECT B.PART_NO,B.SPEC1,A.DATECODE,A.INCOMING_QTY, C.WAREHOUSE_NAME, A.PO_NO "
                + "  FROM SAJET.G_ERP_RT_ITEM A "
                + "      ,SAJET.SYS_PART B "
                + "      ,SAJET.SYS_WAREHOUSE C "    //V1.16004.0.2
                + " WHERE A.RT_ID =:RT_ID "
                + "   AND A.RT_SEQ =:RT_SEQ "
                + "   AND A.PART_ID = B.PART_ID(+) "
                + "   AND A.LOCATION = C.WAREHOUSE_ID(+) ";  //V1.16004.0.2
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RT_ID", g_sRTID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RT_SEQ", combRTSeq.Text };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                SajetCommon.Show_Message("RT Seq Error", 0);
                combRTSeq.Focus();
                combRTSeq.SelectAll();
                return;
            }
            editPartNo.Text = dsTemp.Tables[0].Rows[0]["PART_NO"].ToString();
            lablSpec.Text = dsTemp.Tables[0].Rows[0]["SPEC1"].ToString();
            editDateCode.Text = dsTemp.Tables[0].Rows[0]["DATECODE"].ToString();
            LabIncomingQty.Text = dsTemp.Tables[0].Rows[0]["INCOMING_QTY"].ToString();
            editWarehouse.Text = dsTemp.Tables[0].Rows[0]["WAREHOUSE_NAME"].ToString(); //V1.16004.0.2
            lablPONo.Text = dsTemp.Tables[0].Rows[0]["PO_NO"].ToString(); //V1.16004.0.2

            editPartNo.Enabled = false;
            btnSearch.Enabled = false;
            //get IQC Data
            string sQCResult ="N/A";
            string sLotNo = editRTNO.Text + "-" + iSeq.ToString("00");
            sSQL = " SELECT SAJET.INSPECTION_RESULT(QC_RESULT) QC_RESULT, QC_RESULT QC_RESULT_1,RECEIVE_QTY "
                + "       ,DATECODE,LOT ,PO_NO, C.WAREHOUSE_NAME "
                + "   FROM SAJET.G_IQC_LOT A "
                + "       ,SAJET.SYS_WAREHOUSE C "    //V1.16004.0.2
                + "  WHERE LOT_NO =:LOT_NO "
                + "    AND ROWNUM = 1 "
                + "    AND A.WAREHOUSE_ID = C.WAREHOUSE_ID(+) ";  //V1.16004.0.2;
            Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOT_NO", sLotNo };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                lablIQCResult.Text = dsTemp.Tables[0].Rows[0]["QC_RESULT"].ToString();
                sQCResult = dsTemp.Tables[0].Rows[0]["QC_RESULT_1"].ToString();
                lablIQCReceiveQty.Text = dsTemp.Tables[0].Rows[0]["RECEIVE_QTY"].ToString();
                editDateCode.Text = dsTemp.Tables[0].Rows[0]["DATECODE"].ToString();
                editLot.Text = dsTemp.Tables[0].Rows[0]["LOT"].ToString();
                if (!string.IsNullOrEmpty(dsTemp.Tables[0].Rows[0]["PO_NO"].ToString()))
                {
                    lablPONo.Text = dsTemp.Tables[0].Rows[0]["PO_NO"].ToString();
                }
                if (!string.IsNullOrEmpty(dsTemp.Tables[0].Rows[0]["WAREHOUSE_NAME"].ToString()))     //V1.16004.0.2;
                {
                    editWarehouse.Text = dsTemp.Tables[0].Rows[0]["WAREHOUSE_NAME"].ToString();
                }

            }
            if (sQCResult == "0" || (sQCResult == "4"))
            {
                lablIQCResult.BackColor = Color.Green;
                lablIQCResult.ForeColor = Color.White;
            }
            if (sQCResult == "1")
            {
                lablIQCResult.BackColor = Color.Red;
                lablIQCResult.ForeColor = Color.White;
            }
            if (sQCResult == "2")
            {
                lablIQCResult.BackColor = Color.Yellow;
                lablIQCResult.ForeColor = Color.Red;
            }
            lablInvQty.Text = GetInventoryQty();
            //V1.16004.0.2 Start after select RT seq, next will scan QR code
            //editDateCode.Focus(); 
            //editDateCode.SelectAll();
            editQR.Focus();
            editQR.SelectAll();
            //End
        }

        private string GetInventoryQty()
        {
            sSQL = " SELECT NVL(SUM(NVL(REEL_QTY,0)),0) REEL_QTY "
                + "   FROM SAJET.G_MATERIAL "
                + "  WHERE RT_ID =:RT_ID ";
                
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RT_ID", g_sRTID };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            return dsTemp.Tables[0].Rows[0]["REEL_QTY"].ToString();
        }
        private void editRTNO_TextChanged(object sender, EventArgs e)
        {
            g_sRTID ="0";

            combRTSeq.Items.Clear();
            ClearPartData();
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void editVendor_TextChanged(object sender, EventArgs e)
        {
            combPrefix.Items.Clear();
            if (string.IsNullOrEmpty(editVendor.Text))
                combPrefix.Items.Add("???????");
            else
                combPrefix.Items.Add(editVendor.Text.Trim());
            combPrefix.SelectedIndex = 0;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void lablInvQty_Click(object sender, EventArgs e)
        {

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            LVRT.Items.Clear();
            gbChange.Text = g_sRTRecord + " : " + LVRT.Items.Count.ToString();
        }

        private void editDateCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            editLot.Focus();          
        }

        private void editLot_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            editFix.Focus();
            editFix.SelectAll();
        }

        private void editFix_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            btnOK_Click(sender, e);

        }

        private void combLabelType_SelectedIndexChanged(object sender, EventArgs e)
        {
            combLabelTypeFile.SelectedIndex = combLabelType.SelectedIndex;
        }

        private void label16_Click(object sender, EventArgs e)
        {
             
        }

        private void combPrefix_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdbtnAuto.Checked)
            {
                string sPreFix = combPrefix.Text;
                //sPreFix = sPreFix + DateTime.Now.ToString("yyyyMM");
                sPreFix = sPreFix + GetReelYYMMDD();
                editFix.Text = sPreFix;
            }
        }
        private string GetReelYYMMDD()
        {
            string sSQL = "SELECT SAJET.BCOM_INVENTORY_YMDD REEL_YY FROM DUAL ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            return dsTemp.Tables[0].Rows[0]["REEL_YY"].ToString();
        }


        private void editRTPart_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            editRTPart.Text = editRTPart.Text.Trim();
            if (string.IsNullOrEmpty(editRTPart.Text))
                return;
            sSQL = "SELECT A.RT_SEQ  "
                 + "  FROM SAJET.G_ERP_RT_ITEM A "
                 + "      ,SAJET.SYS_PART B "
                 + " WHERE A.RT_ID =:RT_ID "
                 + "   AND B.PART_NO =:PART_NO "
                 + "   AND A.PART_ID = B.PART_ID(+) "
                 + " ORDER BY A.RT_SEQ ";
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RT_ID", g_sRTID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_NO", editRTPart.Text };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                if (dsTemp.Tables[0].Rows.Count == 1)
                {
                    string sSeq = dsTemp.Tables[0].Rows[0]["RT_SEQ"].ToString();
                    int iIndex = combRTSeq.Items.IndexOf(sSeq);
                    if (iIndex >= 0)
                    {
                        editRTPart.Text = string.Empty;
                        combRTSeq.SelectedIndex = iIndex;
                    }
                }
                else
                {
                    fSelectRTSeq fData = new fSelectRTSeq();
                    try
                    {
                        for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
                        {
                            fData.combRTSeq.Items.Add(dsTemp.Tables[0].Rows[i]["RT_SEQ"].ToString());
                        }
                        fData.combRTSeq.SelectedIndex = 0;
                        if (fData.ShowDialog() == DialogResult.OK)
                        {
                            string sSeq = fData.g_sSeq;
                            int iIndex = combRTSeq.Items.IndexOf(sSeq);
                            if (iIndex >= 0)
                            {
                                editRTPart.Text = string.Empty;
                                combRTSeq.SelectedIndex = iIndex;
                            }
                        }
                        else
                        {
                            return;
                        }

                    }
                    finally
                    {
                        fData.Dispose();
                    }
                }
            }
            else
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Part No") + " : " + editRTPart.Text + Environment.NewLine + Environment.NewLine
                                       + SajetCommon.SetLanguage("RT does not include this Part"), 0);
                editRTPart.Focus();
                editRTPart.SelectAll();
                return;
            }

        }

        private void btnFilterWH_Click(object sender, EventArgs e)
        {
            editWarehouse.Text = editWarehouse.Text.Trim();

            sSQL = "Select WAREHOUSE_NAME FROM SAJET.SYS_WAREHOUSE A "
                        + " Where enabled = 'Y' ";

            if (!string.IsNullOrEmpty(editWarehouse.Text))
                sSQL = sSQL + "and WAREHOUSE_NAME like '" + editWarehouse.Text + "%'";
            sSQL = sSQL + "order by WAREHOUSE_NAME ";



            fFilter f = new fFilter();
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                if (f.dgvData.CurrentRow != null)
                {
                    editWarehouse.Text = f.dgvData.CurrentRow.Cells["WAREHOUSE_NAME"].Value.ToString();
                    g_sWarehouseID = SajetCommon.GetID("SAJET.SYS_WAREHOUSE", "WAREHOUSE_ID", "WAREHOUSE_NAME", editWarehouse.Text.Trim(), "Y");
                }
                editPartNo.Focus();
                editPartNo.SelectAll();
            }
        }

        private void editWarehouse_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            if (!CheckWarehouseID(editWarehouse.Text))
            {
                return;
            }
            editPartNo.Focus();
            editPartNo.SelectAll();
        }
        private bool CheckWarehouseID(string WarehouseName)
        {
            
            if (string.IsNullOrEmpty(WarehouseName))
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please input Warehouse Name"), 0);
                return false;
            }
            g_sWarehouseID = SajetCommon.GetID("SAJET.SYS_WAREHOUSE", "WAREHOUSE_ID", "WAREHOUSE_NAME", WarehouseName, "Y");
            if (g_sWarehouseID == "0")
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Warehouse Error"), 0);
                editWarehouse.Focus();
                editWarehouse.SelectAll();
                return false;
            }
            return true;
        }

        private void editWarehouse_TextChanged(object sender, EventArgs e)
        {
            g_sWarehouseID="0";
        }

        private void editReelNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            g_bDivReel = false;
            //string sReelNo = editReelNo.Text.Trim();
            //editReelNo.Text = sReelNo;
            sSQL = "SELECT A.REEL_NO,B.PART_NO,A.REEL_QTY,A.DATECODE,A.LOT,C.VENDOR_CODE,C.VENDOR_NAME, "
                + "        B.PART_ID,C.VENDOR_ID,A.RT_ID "
                + "   FROM SAJET.G_MATERIAL A, "
                + "        SAJET.SYS_PART B, "
                + "        SAJET.SYS_VENDOR C "
                + "  WHERE A.REEL_NO=:REEL_NO "
                + "    AND A.PART_ID = B.PART_ID(+) "
                + "    AND A.VENDOR_ID = C.VENDOR_ID(+) ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "REEL_NO", editReelNo.Text.Trim() };

            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                SajetCommon.Show_Message("Reel No Error", 0);
                editReelNo.Focus();
                editReelNo.SelectAll();
                return;
            }
            lablUnitQty.Text = dsTemp.Tables[0].Rows[0]["REEL_QTY"].ToString();
            lablPartNo.Text = dsTemp.Tables[0].Rows[0]["PART_NO"].ToString();
            lablVenCode.Text = dsTemp.Tables[0].Rows[0]["VENDOR_CODE"].ToString();
            LabVendorCode.Text = dsTemp.Tables[0].Rows[0]["VENDOR_NAME"].ToString();
            lablDC.Text = dsTemp.Tables[0].Rows[0]["DATECODE"].ToString();
            lablLot.Text = dsTemp.Tables[0].Rows[0]["LOT"].ToString();
         //   g_sDivPartID = dsTemp.Tables[0].Rows[0]["PART_ID"].ToString();
         //   g_sDivVendorID = dsTemp.Tables[0].Rows[0]["VENDOR_ID"].ToString();
         //   g_sDivRTID = dsTemp.Tables[0].Rows[0]["RT_ID"].ToString();

            editDivisionQty.Focus();
            g_bDivReel = true;
        }

        private void btnExec_Click(object sender, EventArgs e)
        {
            if (!g_bDivReel)
            {
                string sMsg = SajetCommon.SetLanguage("Please Input Reel No", 1);
                SajetCommon.Show_Message(sMsg, 0);
                editReelNo.Focus();
                editReelNo.SelectAll();
                return;
            }
            g_dtSysdate = ClientUtils.GetSysDate();
            string strNewReelNo = string.Empty;
            int iUnitQty;
            try
            {
                iUnitQty = Convert.ToInt32(editDivisionQty.Text);
            }
            catch
            {
                string sMsg = SajetCommon.SetLanguage("Division Qty Null!!", 1);
                SajetCommon.Show_Message(sMsg, 0);
                editDivisionQty.Focus();
                editDivisionQty.SelectAll();
                return;
            }

            if (iUnitQty == 0)
            {
                string sMsg = SajetCommon.SetLanguage("Division Qty Zero!!", 1);
                SajetCommon.Show_Message(sMsg, 0);
                editDivisionQty.Focus();
                editDivisionQty.SelectAll();
                return;

            }


            if (iUnitQty >= Convert.ToInt32(lablUnitQty.Text))
            {
                string sMsg = SajetCommon.SetLanguage("Division Qty Error")+Environment.NewLine
                     + SajetCommon.SetLanguage("Out of Range")+" : " + lablUnitQty.Text;
                SajetCommon.Show_Message(sMsg, 0);
                editDivisionQty.Focus();
                editDivisionQty.SelectAll();
                return;
            }
            int iOriUnitQty = Convert.ToInt32(lablUnitQty.Text) - iUnitQty;
            ////找Reel最大值
            string sOldReel = editReelNo.Text.Trim();

            if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Original Reel") + " : " + sOldReel  + Environment.NewLine + Environment.NewLine
            + SajetCommon.SetLanguage("Division Qty") + " : " + iUnitQty, 2) != DialogResult.Yes)
                return;


            string sPreFix = lablVenCode.Text;
            //sPreFix = sPreFix + DateTime.Now.ToString("yyyyMM");
            sPreFix = sPreFix + GetReelYYMMDD();
            strNewReelNo = GetReelNo(lablVenCode.Text, sPreFix); 

           
            /*
            string[] sOldReelList = sOldReel.Split('-');
            if (sOldReel.Substring(0, 1) == "R" && sOldReelList.Length == 2)
            {
                sOldReel = sOldReelList[0].ToString() + "-";
                strNewReelNo = GetReelNo("", sOldReel);
            }
            else
                strNewReelNo = GetReelNoDB(editReelNo.Text.Substring(0, editReelNo.Text.LastIndexOf("-")));
             */ 


            //string alpha_divide = editReelNo.Text.Trim().Substring(0, 6);//lablVenCode.Text
            //sReelNo = DateTime.Now.ToString("yyMMdd");
            //sSQL = " SELECT NVL(LPAD(SUBSTR(MAX(REEL_NO),17,5)+1,'5',0),'N/A') REEL_NO " // string sPreFix = GetVendor("Alpha Networks Inc") + "-";//editVendor.Text+"-";// combPrefix.Text;
            //     + "   FROM SAJET.G_MATERIAL "
            //     + "  WHERE REEL_NO like '" + alpha_divide + "-" + sReelNo + "R09%' ";// editReelNo
            //dsTemp = ClientUtils.ExecuteSQL(sSQL);
            //if (dsTemp.Tables[0].Rows.Count != 0)
            //{
            //    if (dsTemp.Tables[0].Rows[0]["REEL_NO"].ToString() != "N/A")
            //    {
            //        sReelNo = alpha_divide + "-" + sReelNo + "R09" + dsTemp.Tables[0].Rows[0]["REEL_NO"].ToString();
            //    }
            //    else
            //    {
            //        sReelNo = alpha_divide + "-" + sReelNo + "R0900001";
            //    }
            //}
            //else
            //{
            //    sReelNo = alpha_divide + "-" + sReelNo + "R0900001";
            //}

            //g_dtSysdate = ClientUtils.GetSysDate();

            // GetLoc(lablPartNo.Text, sWH);
            SaveCfgToIni();

            listPrintSN.Items.Clear();


            if (!InsertDivReelNo(strNewReelNo))
                return;

            LVDivReel.Items.Add(strNewReelNo);
            LVDivReel.Items[LVDivReel.Items.Count - 1].SubItems.Add(lablPartNo.Text);
            LVDivReel.Items[LVDivReel.Items.Count - 1].SubItems.Add(iUnitQty.ToString());
            LVDivReel.Items[LVDivReel.Items.Count - 1].SubItems.Add(lablDC.Text);
            LVDivReel.Items[LVDivReel.Items.Count - 1].SubItems.Add(lablLot.Text);
            LVDivReel.Items[LVDivReel.Items.Count - 1].SubItems.Add(lablVenCode.Text + " " + LabVendorCode.Text);
            LVDivReel.Items[LVDivReel.Items.Count - 1].SubItems.Add(ClientUtils.fUserName);
            LVDivReel.Items[LVDivReel.Items.Count - 1].SubItems.Add(g_dtSysdate.ToString());

            LVDivReel.Items.Add(editReelNo.Text.Trim());
            LVDivReel.Items[LVDivReel.Items.Count - 1].SubItems.Add(lablPartNo.Text);
            LVDivReel.Items[LVDivReel.Items.Count - 1].SubItems.Add(iOriUnitQty.ToString());
            LVDivReel.Items[LVDivReel.Items.Count - 1].SubItems.Add(lablDC.Text);
            LVDivReel.Items[LVDivReel.Items.Count - 1].SubItems.Add(lablLot.Text);
            LVDivReel.Items[LVDivReel.Items.Count - 1].SubItems.Add(lablVenCode.Text + " " + LabVendorCode.Text);
            LVDivReel.Items[LVDivReel.Items.Count - 1].SubItems.Add(ClientUtils.fUserName);
            LVDivReel.Items[LVDivReel.Items.Count - 1].SubItems.Add(g_dtSysdate.ToString());
            //SajetInifile sajetInifile1 = new SajetInifile();
            //string sPrintPort = sajetInifile1.ReadIniFile(g_sIniFile, g_sProgram, "Print Port", "COM1");
            //string sSleepTime = sajetInifile1.ReadIniFile(g_sIniFile, g_sProgram, "Sleep", "100");
            if (chkbPrint.Checked)
                listPrintSN.Items.Add(strNewReelNo);
            if (chkbPrintOriginal.Checked)
                listPrintSN.Items.Add(editReelNo.Text.Trim());
            if (listPrintSN.Items.Count > 0)
                PrintReel();
            SajetCommon.Show_Message(SajetCommon.SetLanguage("Original Reel") + " : " + sOldReel + "[" + iOriUnitQty.ToString() + "]" + Environment.NewLine + Environment.NewLine
                                    + SajetCommon.SetLanguage("New Reel") + " : " + strNewReelNo + "[" + iUnitQty.ToString() + "]", 4);
            /*
            if (chkbDivPrint.Checked || chkbPrintOriginal.Checked)
            {
                if (chkbDivPrint.Checked)
                {
                    //PrintReelNo(strNewReelNo, sPrintPort, Convert.ToInt32(sSleepTime), 2);
                    listPrintSN.Items.Add(strNewReelNo);
                }
                if (chkbPrintOriginal.Checked)
                {
                    //PrintReelNo(editReelNo.Text.Trim(), sPrintPort, Convert.ToInt32(sSleepTime), 3);
                    listPrintSN.Items.Add(editReelNo.Text.Trim());
                }
                PrintReel();
            }
             */
            lablUnitQty.Text = "0";
            lablPartNo.Text = "N/A";
            lablVenCode.Text = "N/A";
            LabVendorCode.Text = "N/A";
            lablDC.Text = "N/A";
            lablLot.Text = "N/A";
            editDivisionQty.Text = "";
            editReelNo.Text = string.Empty;

            editReelNo.Focus();
            editReelNo.SelectAll();
        }
        private bool InsertDivReelNo(string sReelNo)
        {
            try
            {
                object[][] Params = new object[5][];
                sSQL = "Insert Into SAJET.G_MATERIAL "
                     + "(RT_ID,PART_ID,DATECODE,REEL_NO,REEL_QTY,VENDOR_ID,LOT,WAREHOUSE_ID,UPDATE_USERID,UPDATE_TIME,RT_RECEIVE_TIME,REEL_MEMO,RT_SEQ ,INVENTORY_TIME,INVENTORY_EMPID, VENDOR_PART_NO ) "
                    //+ "Values "
                     + "SELECT "
                     + " RT_ID,PART_ID,DATECODE,:REEL_NO,:REEL_QTY,VENDOR_ID,LOT,WAREHOUSE_ID,:UPDATE_USERID,SYSDATE,RT_RECEIVE_TIME,REEL_MEMO,RT_SEQ,SYSDATE,:INVENTORY_EMPID, VENDOR_PART_NO "
                     + " FROM SAJET.G_MATERIAL "
                     + " WHERE REEL_NO =:REEL_NO1";
                //+ "(:RT_ID,:PART_ID,:DATECODE,:REEL_NO,:REEL_QTY,:VENDOR_ID,:LOT,:UPDATE_USERID,:UPDATE_TIME,:RT_RECEIVE_TIME,:REEL_MEMO) ";
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "REEL_NO", sReelNo };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "REEL_QTY", editDivisionQty.Text.ToString() };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "INVENTORY_EMPID", g_sUserID };
                Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "REEL_NO1", editReelNo.Text.Trim() };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                string sPartID = SajetCommon.GetID("SAJET.SYS_PART", "PART_ID", "PART_NO", lablPartNo.Text);
                Params = new object[3][];
                sSQL = "UPDATE SAJET.G_MATERIAL "
                     + "   SET REEL_QTY = REEL_QTY - :REEL_QTY "
                     + "WHERE PART_ID = :PART_ID AND REEL_NO = :REEL_NO ";
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "REEL_QTY", editDivisionQty.Text.ToString() };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sPartID };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "REEL_NO", editReelNo.Text.Trim() };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                return true;
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception:" + ex.Message, 0);
                return false;
            }
        }

        private void editReelNo_TextChanged(object sender, EventArgs e)
        {
            g_bDivReel = false;
            lablUnitQty.Text = "0";
            lablPartNo.Text = "N/A";
            lablVenCode.Text = "N/A";
            LabVendorCode.Text = "N/A";
            lablDC.Text = "N/A";
            lablLot.Text = "N/A";
            editDivisionQty.Text = "";
        }

        private void btnModifyQty_Click(object sender, EventArgs e)
        {
            if (!g_bDivReel)
                return;
            fModifyQty f = new fModifyQty();
            f.sReel = editReelNo.Text.ToString();
            f.editQty.Text = lablUnitQty.Text.ToString();
            f.g_sUserID = g_sUserID;
            if (f.ShowDialog() == DialogResult.OK)
            {
                lablUnitQty.Text = f.sQty;
                editDivisionQty.Focus();
                editDivisionQty.SelectAll();
            }  
        }

        private void editQR_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            if (editPartNo.Enabled == true) //沒有輸入RT單才可清空
            {
                //same as RT TextChange 2016/07/15
                g_sRTID = "0";

                combRTSeq.Items.Clear();
                ClearPartData();
            }

            object[][] Params = new object[8][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TQRMaterial", editQR.Text };
            Params[1] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TZ_PN", "" };
            Params[2] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TLOT", "" };
            Params[3] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TVID", "" };
            Params[4] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TDATE", "" };
            Params[5] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TM_PN", "" };
            Params[6] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TQTY", "" };
            Params[7] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
            DataSet dsQR = ClientUtils.ExecuteProc("SAJET.C_ZW_REEL_QR_SPILT", Params);
            if (dsQR.Tables[0].Rows.Count > 0)
            {
                KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);

                if (editPartNo.Enabled == false)
                {
                    if (editPartNo.Text != dsQR.Tables[0].Rows[0]["TZ_PN"].ToString())
                    {
                        SajetCommon.Show_Message("QR Code Part not in RT!", 0);
                        editQR.Focus();
                        editQR.SelectAll();
                        return ;
                    }
                }
                editPartNo.Text = getNullString(dsQR.Tables[0].Rows[0]["TZ_PN"].ToString());
                editPartNo_KeyPress(sender, Key);

                if (editVendor.Enabled == false)
                {
                    if (editVendor.Text != dsQR.Tables[0].Rows[0]["TVID"].ToString())
                    {
                        SajetCommon.Show_Message("QR Code Vendor not in RT!", 0);
                        editQR.Focus();
                        editQR.SelectAll();
                        return;
                    }
                }
                editVendor.Text = getNullString(dsQR.Tables[0].Rows[0]["TVID"].ToString());
                editVendor_KeyPress(sender, Key);
                editVendorPartNo.Text = getNullString(dsQR.Tables[0].Rows[0]["TM_PN"].ToString());
                editDateCode.Text = getNullString(dsQR.Tables[0].Rows[0]["TDATE"].ToString());
                editLot.Text = getNullString(dsQR.Tables[0].Rows[0]["TLOT"].ToString());
                int Unitqty = 0;
                int.TryParse(dsQR.Tables[0].Rows[0]["TQTY"].ToString(), out Unitqty);
                editUnitQty.Value = Unitqty;

                btnExecute.Focus();
            }
        }

        /// <summary>
        /// Reprint
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            //check input option
            SetPrintInfo("", txtPrintPartNo.Text, txtPrintVendorCode.Text, txtPrintDateCode.Text, txtPrintLot.Text);
            
            if (txtPrintReelNo.Text.Length == 0)
            {
                SajetCommon.Show_Message("No Data Found!", 0);
                return;
            }

            listPrintSN.Items.Clear();

            listPrintSN.Items.Add(txtPrintReelNo.Text);

            PrintReel();
        }

        private void txtPrintQR_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            object[][] Params = new object[8][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TQRMaterial", txtPrintQR.Text };
            Params[1] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TZ_PN", "" };
            Params[2] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TLOT", "" };
            Params[3] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TVID", "" };
            Params[4] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TDATE", "" };
            Params[5] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TM_PN", "" };
            Params[6] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TQTY", "" };
            Params[7] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
            DataSet dsQR = ClientUtils.ExecuteProc("SAJET.C_ZW_REEL_QR_SPILT", Params);
            if (dsQR.Tables[0].Rows.Count > 0)
            {
                txtPrintPartNo.Text = getNullString(dsQR.Tables[0].Rows[0]["TZ_PN"].ToString());
                txtPrintVendorCode.Text = getNullString(dsQR.Tables[0].Rows[0]["TVID"].ToString());
                txtPrintVendorPartNo.Text = getNullString(dsQR.Tables[0].Rows[0]["TM_PN"].ToString());
                txtPrintDateCode.Text = getNullString(dsQR.Tables[0].Rows[0]["TDATE"].ToString());
                txtPrintLot.Text = getNullString(dsQR.Tables[0].Rows[0]["TLOT"].ToString());
                //numPrintUnitQty.Value = int.Parse(getNullString(dsQR.Tables[0].Rows[0]["TQTY"].ToString()));
                int Unitqty = 0;
                int.TryParse(dsQR.Tables[0].Rows[0]["TQTY"].ToString(), out Unitqty);
                numPrintUnitQty.Value = Unitqty;
            }
            SetPrintInfo("", txtPrintPartNo.Text, txtPrintVendorCode.Text, txtPrintDateCode.Text, txtPrintLot.Text);
        }

        private string getNullString(string sStr)
        {
            if (sStr.Length > 0)
            {
                if (sStr == "null")
                {
                    return "";
                }
            }
            return sStr;
        }

        private DataSet GetPrintReelInfo(string mReelNo, string mPartNo, string mVendorNo, string mDateCode, string mLot)
        {
            if ((mReelNo.Length==0) && ( mPartNo.Length == 0 || mVendorNo.Length == 0 || mDateCode.Length == 0 || mLot.Length == 0 ))
            {
                return null;
            }

            object[][] Params;
            sSQL = "Select A.REEL_NO,B.PART_NO,C.VENDOR_NAME,A.LOT,A.DATECODE,C.VENDOR_CODE,A.REEL_MEMO, A.VENDOR_PART_NO, A.REEL_QTY "
                 + "  FROM SAJET.G_MATERIAL A "
                 + "      ,SAJET.SYS_PART B "
                 + "      ,SAJET.SYS_VENDOR C "
                 + " Where 1=1 ";
            if (mReelNo.Length == 0)
            {
                sSQL += " AND A.LOT = :sLot "
                    + " AND A.DateCode = :sDateCode "
                    + " AND B.PART_NO = :sPartNo "
                    + " AND C.VENDOR_CODE = :sVid ";
            }
            else
            {
                sSQL += " AND A.REEL_NO = :sReelNo ";
            }
            
            sSQL += " AND A.PART_ID = B.PART_ID "
                 + " AND A.VENDOR_ID = C.VENDOR_ID "
                 + " ORDER by A.REEL_NO DESC, A.UPDATE_TIME DESC ";

            if (mReelNo.Length == 0)
            {
                Params = new object[4][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "sLot", mLot };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "sDateCode", mDateCode };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "sPartNo", mPartNo };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "sVid", mVendorNo };
            }
            else
            {
                Params = new object[1][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "sReelNo", mReelNo };

            }

            DataSet dsReel = ClientUtils.ExecuteSQL(sSQL, Params);

            return dsReel;

        }

        private void SetPrintInfo(string PrintReelNo, string PrintPartNo, string PrintVendorCode, string PrintDateCode, string PrintLot)
        {
            // Check Input value not empty?
            //txtPrintReelNo.Text = txtPrintReelNo.Text.Trim();
            //txtPrintPartNo.Text = txtPrintPartNo.Text.Trim();
            //txtPrintVendorCode.Text=txtPrintVendorCode.Text.Trim();
            //txtPrintDateCode.Text=txtPrintDateCode.Text.Trim();
            //txtPrintLot.Text = txtPrintLot.Text.Trim();

            DataSet dsReel = GetPrintReelInfo(PrintReelNo,
                PrintPartNo,
                PrintVendorCode,
                PrintDateCode,
                PrintLot);
            if (dsReel == null)
            {
                SajetCommon.Show_Message("Please Input 1 of Option!", 0);
                return;
            }
            if (dsReel.Tables[0].Rows.Count == 0)
            {
                SajetCommon.Show_Message("No Data Found!", 0);
                return;
            }

            txtPrintReelNo.Text = dsReel.Tables[0].Rows[0]["Reel_NO"].ToString();

            txtPrintPartNo.Text = dsReel.Tables[0].Rows[0]["PART_NO"].ToString();
            txtPrintVendorCode.Text = dsReel.Tables[0].Rows[0]["VENDOR_CODE"].ToString();
            txtPrintDateCode.Text = dsReel.Tables[0].Rows[0]["DATECODE"].ToString();
            txtPrintLot.Text = dsReel.Tables[0].Rows[0]["LOT"].ToString();

            numPrintUnitQty.Value = int.Parse(dsReel.Tables[0].Rows[0]["REEL_QTY"].ToString());
            txtPrintVendorPartNo.Text = dsReel.Tables[0].Rows[0]["VENDOR_PART_NO"].ToString();
        }

        private void txtPrintPartNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            SetPrintInfo("", txtPrintPartNo.Text, txtPrintVendorCode.Text, txtPrintDateCode.Text, txtPrintLot.Text);
            
        }

        private void txtPrintVendorCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            SetPrintInfo("", txtPrintPartNo.Text, txtPrintVendorCode.Text, txtPrintDateCode.Text, txtPrintLot.Text);
            
        }

        private void txtPrintDateCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            SetPrintInfo("", txtPrintPartNo.Text, txtPrintVendorCode.Text, txtPrintDateCode.Text, txtPrintLot.Text);
            
        }

        private void txtPrintLot_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            SetPrintInfo("", txtPrintPartNo.Text, txtPrintVendorCode.Text, txtPrintDateCode.Text, txtPrintLot.Text);
            
        }

        private void txtPrintReelNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            SetPrintInfo(txtPrintReelNo.Text, "", "", "","");
            
        }
    }

}

