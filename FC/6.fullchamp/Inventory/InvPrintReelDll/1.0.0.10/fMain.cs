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

namespace InvPrintReeldll
{
    public partial class fMain : Form
    {                       
        public fMain()
        {
            InitializeComponent();
        }
        
        DataSet dsTemp;
        String sSQL, g_sUserID, g_sProgram, g_sFunction, g_sExeName;
        String g_sRecordText, g_sgrpbResult;
                
        private void fMain_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            g_sRecordText = gbChange.Text;
            g_sgrpbResult = grpbResult.Text;
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            //panel1.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
            //panel1.BackgroundImageLayout = ImageLayout.Stretch;
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            g_sUserID = ClientUtils.UserPara1;
            g_sFunction = ClientUtils.fFunctionName;
            g_sProgram = ClientUtils.fProgramName;
            g_sExeName = ClientUtils.fCurrentProject;
            this.Text = this.Text + " (" + SajetCommon.g_sFileVersion + ")";
            btnPrint.BackColor = Color.Green;
            btnPrint.ForeColor = Color.White;
            check_privilege();
            ReadLabelType();       
        }
        public void check_privilege()
        {
            btnPrint.Enabled = false;
            int iPrivilege = ClientUtils.GetPrivilege(g_sUserID, g_sFunction, g_sProgram);
            btnPrint.Enabled = (iPrivilege >= 1);

            int iModify = ClientUtils.GetPrivilege(g_sUserID, "Modify Unit Qty", g_sProgram);
            modifyToolStripMenuItem.Enabled = (iModify >= 1);
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
                combLabelType.Items.Add(SajetCommon.SetLanguage("Middle"));
                combLabelTypeFile.Items.Add("REEL_DEFAULT_M");
                combLabelType.Items.Add(SajetCommon.SetLanguage("Small"));
                combLabelTypeFile.Items.Add("REEL_DEFAULT_S");
            }
            combLabelType.SelectedIndex = 0;
            combLabelTypeFile.SelectedIndex = 0;
        }
        private void editReel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            editReel.Text = editReel.Text.Trim();

            sSQL = "Select a.*,b.part_no,c.vendor_code,c.VENDOR_NAME "
                 + "  ,NVL(D.RT_NO,'N/A') rt_no "
                 + " from sajet.g_material a "
                 + "   ,sajet.sys_part b "
                 + "   ,sajet.sys_vendor c "
                 + "   ,sajet.g_erp_rtno d "
                 + " where a.reel_no = '" + editReel.Text + "' "
                 + " and a.part_id = b.part_id(+) "
                 + " and a.vendor_id = c.vendor_id(+) "
                 + " and a.rt_id = d.rt_id(+) ";

            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                SajetCommon.Show_Message("Reel Error",0);
                editReel.Focus();
                editReel.SelectAll();
                return;
            }

            for (int i = 0; i < dgvRT.Rows.Count; i++)
            {
                if (editReel.Text == dgvRT.Rows[i].Cells["REEL_NO1"].Value.ToString())
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Reel No Duplicate", 1) + " , " + editReel.Text, 0);
                    editReel.Focus();
                    editReel.SelectAll();
                    return;
                }
            }

            //ListViewItem[] Item = LVRT.Items.Find(editReel.Text, false);
            //if (Item.Length > 0)
            //{
            //    SajetCommon.Show_Message(SajetCommon.SetLanguage("Reel No Duplicate", 1) + " , " + editReel.Text, 0);
            //    editReel.Focus();
            //    editReel.SelectAll();
            //    return;
            //}
            DataRow dr = dsTemp.Tables[0].Rows[0];
            dgvRT.Rows.Add();
            dgvRT.Rows[dgvRT.Rows.Count - 1].Cells["REEL_NO1"].Value = dr["REEL_NO"].ToString();
            dgvRT.Rows[dgvRT.Rows.Count - 1].Cells["PART_NO1"].Value = dr["PART_NO"].ToString();
            dgvRT.Rows[dgvRT.Rows.Count - 1].Cells["REEL_QTY1"].Value = dr["REEL_QTY"].ToString();
            dgvRT.Rows[dgvRT.Rows.Count - 1].Cells["DATECODE1"].Value = dr["DATECODE"].ToString();
            dgvRT.Rows[dgvRT.Rows.Count - 1].Cells["LOT1"].Value = dr["LOT"].ToString();
            dgvRT.Rows[dgvRT.Rows.Count - 1].Cells["VENDOR_CODE1"].Value = dr["VENDOR_CODE"].ToString();
            dgvRT.Rows[dgvRT.Rows.Count - 1].Cells["VENDOR_NAME1"].Value = dr["VENDOR_NAME"].ToString();
            dgvRT.Rows[dgvRT.Rows.Count - 1].Cells["RT_NO1"].Value = dr["RT_NO"].ToString();
            dgvRT.Rows[dgvRT.Rows.Count - 1].Cells["REEL_MEMO1"].Value = dr["REEL_MEMO"].ToString();
            gbChange.Text = g_sRecordText + " : " + dgvRT.Rows.Count.ToString();        
            
            //LVRT.Items.Add(dsTemp.Tables[0].Rows[0]["REEL_NO"].ToString());
            //LVRT.Items[LVRT.Items.Count - 1].Name = dsTemp.Tables[0].Rows[0]["REEL_NO"].ToString();
            //LVRT.Items[LVRT.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[0]["PART_NO"].ToString());
            //LVRT.Items[LVRT.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[0]["REEL_QTY"].ToString());
            //LVRT.Items[LVRT.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[0]["DATECODE"].ToString());
            //LVRT.Items[LVRT.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[0]["LOT"].ToString());
            //LVRT.Items[LVRT.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[0]["VENDOR_CODE"].ToString());
            //LVRT.Items[LVRT.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[0]["VENDOR_NAME"].ToString());
            //LVRT.Items[LVRT.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[0]["RT_NO"].ToString());
            //gbChange.Text = g_sRecordText +" : " + LVRT.Items.Count.ToString();
            editReel.Focus();
            editReel.SelectAll();
        }        
        
        private void fMain_Shown(object sender, EventArgs e)
        {
            editReel.Focus();
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
        private void WriteToPrintGo(string sFile, string sData)
        {
            try
            {
                if (File.Exists(sFile))
                {
                    File.Delete(sFile);
                }
                System.IO.File.AppendAllText(sFile, sData, Encoding.Default);
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
        [DllImport("kernel32.dll")]
        public static extern int WinExec(string exeName, int operType);
        private void PrintReel_new()
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

            //先Link====
            PrintLabel.Setup PrintLabelDll = new PrintLabel.Setup();
            ListBox ListParam = new ListBox();
            ListBox ListData = new ListBox();
            string sLabelType = combLabelTypeFile.Text;
            //Print=====
            try
            {
                //   progressBar1.Minimum = 0;
                //   progressBar1.Maximum = listbPrintSN.Items.Count - 1;
                //   progressBar1.Visible = true;
                ListParam.Items.Clear();
                ListData.Items.Clear();
                string sMessage = "";
                for (int i = 0; i < dgvRT.Rows.Count; i++)
                {
                    ListData.Items.Add(dgvRT.Rows[i].Cells["REEL_NO1"].Value.ToString());
                }
                //for (int i = 0; i <= LVRT.Items.Count - 1; i++)
                //{
                //    ListData.Items.Add(LVRT.Items[i].SubItems[0].Text);
                //}
                PrintLabelDll.Print_Bartender_DataSource(g_sExeName, sDataType, "REE_", sLabelType, iPrintQty, "BARTENDER", "DATASOURCE", ListParam, ListData, out sMessage);
                if (sMessage != "OK")
                {
                    SajetCommon.Show_Message(sMessage, 0);
//                    return false;
                }
  //              return true;

            }
            finally
            {
            }
        }
/*
        private void PrintReel()
        {
            int iPrintQty = Convert.ToInt32(editPrintQty.Value);
            string sMessage = "";
            ListBox ListParam = new ListBox();
            ListBox ListData = new ListBox();

            string sPrintMethod = "DLL";
            string sPrintPort = "BarTender";
//            PrintLabel.Setup PrintLabelDll = new PrintLabel.Setup();
//            PrintLabelDll.Open(sPrintPort.ToUpper()); 
            string sLabelTypeFile = combLabelTypeFile.Text;
            string sLabelFileName = Application.StartupPath + "\\" + g_sExeName + "\\" + sLabelTypeFile + ".btw";
            string sDataSourceFile = Application.StartupPath + "\\" + g_sExeName + "\\" + sLabelTypeFile + ".lst";
            string sLabelFieldFile = Application.StartupPath + "\\" + g_sExeName + "\\" + sLabelTypeFile + ".dat";

            string sPrintGoFile = Application.StartupPath + "\\" + g_sExeName + "\\" + "PrintGo.bat";
            string sBatFile = Application.StartupPath + "\\" + g_sExeName + "\\PrintLabel.bat";
            try
            {
                if (!File.Exists(sLabelFileName))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Label File Not Found") +" (.btw)"+ Environment.NewLine + Environment.NewLine
                                            + sLabelFileName, 0);
                    return;
                }
                if (!File.Exists(sLabelFieldFile))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Label File Not Found") +" (.dat)"+ Environment.NewLine + Environment.NewLine
                                            + sLabelFileName, 0);
                    return;
                }
                if (!File.Exists(sBatFile))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("File Not Found") + " (PrintLabel.bat)" + Environment.NewLine + Environment.NewLine
                                            + sLabelFileName, 0);
                    return;
                }
                if (File.Exists(sDataSourceFile))
                    File.Delete(sDataSourceFile);

                LVRT.Focus();
                ListParam.Items.Clear();                
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
                int iIndex;
                //==================表頭資料================================
                for (int j = 0; j <= listField.Items.Count - 1; j++)
                {
                    sData = sData + listField.Items[j].ToString() + ",";
                }
                if (!string.IsNullOrEmpty(sData))
                {
                    sData = sData.Substring(0, sData.Length - 1);
                }                
                WriteToTxt(sDataSourceFile, sData);
                

                for (int i = 0; i <= LVRT.Items.Count - 1; i++)
                {
                    LVRT.Items[i].Selected = true;
                    string sReel = LVRT.Items[i].SubItems[0].Text;
                    string sPart = LVRT.Items[i].SubItems[1].Text;
                    string sUnitQty = LVRT.Items[i].SubItems[2].Text;
                    string sDateCode = LVRT.Items[i].SubItems[3].Text;
                    string sLot = LVRT.Items[i].SubItems[4].Text;
                    string sVendorCode = LVRT.Items[i].SubItems[5].Text;
                    string sVendorName = LVRT.Items[i].SubItems[6].Text;
                    string sRT = LVRT.Items[i].SubItems[7].Text;

                    ListData.Items.Clear();
                    ListData.Items.Add(sReel);
                    ListData.Items.Add(sDateCode);
                    ListData.Items.Add(sPart);
                    ListData.Items.Add(sVendorCode);
                    ListData.Items.Add(sVendorName);
                    ListData.Items.Add(sRT);                    
                    ListData.Items.Add(sUnitQty);                    
                    ListData.Items.Add(sLot);
                    sData = string.Empty;
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
                    WriteToTxt(sDataSourceFile, sData);
                    //=========================================================
                    
                    //PrintLabelDll.Print(g_sExeName, "REEL_TYPE", "REEL_", "", iPrintQty, sPrintMethod, sPrintPort, ListParam, ListData, out sMessage);
                    //if (sMessage != "OK")
                    //{
                    //    SajetCommon.Show_Message(sMessage, 0);
                    //    return;
                    //}
                     
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
               
               // SajetCommon.Show_Message("Print Finish", -1);
            }
            finally
            {
               // PrintLabelDll.Close(sPrintPort.ToUpper()); 
                ListParam.Dispose();
                ListData.Dispose();
            }
        }
*/
        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvRT.Rows.Count == 0 || dgvRT.SelectedCells.Count == 0 || dgvRT.CurrentCell == null)
            { return; }
            int iIndex = dgvRT.CurrentRow.Index;
            dgvRT.Rows.RemoveAt(iIndex);
            gbChange.Text = g_sRecordText + " : " + dgvRT.Rows.Count.ToString();

            //if (LVRT.Items.Count == 0 || LVRT.SelectedItems.Count == 0)
            //    return;
            //LVRT.SelectedItems[0].Remove();
            //gbChange.Text = g_sRecordText + " : " + LVRT.Items.Count.ToString();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dgvRT.Rows.Count == 0)
                return;
            PrintReel_new();
            dgvRT.Rows.Clear();
            gbChange.Text = g_sRecordText + " : " + dgvData.Rows.Count.ToString();
            //if (LVRT.Items.Count == 0)
            //    return;
            //PrintReel_new();
            //LVRT.Items.Clear();
            //gbChange.Text = g_sRecordText + " : " + LVRT.Items.Count.ToString();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (dgvRT.Rows.Count == 0)
                return;
            if (SajetCommon.Show_Message("Clear Reel List ?", 2) != DialogResult.Yes)
                return;
            dgvRT.Rows.Clear();
            editReel.Focus();
            editReel.SelectAll();
            gbChange.Text = g_sRecordText + " : " + dgvRT.Rows.Count.ToString();
            //if (LVRT.Items.Count == 0)
            //    return;
            //if (SajetCommon.Show_Message("Clear Reel List ?", 2) != DialogResult.Yes)
            //    return;
            //LVRT.Items.Clear();
            //editReel.Focus();
            //editReel.SelectAll();
            //gbChange.Text = g_sRecordText + " : " + LVRT.Items.Count.ToString();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            object[][] Params = new object[0][];
            dgvData.Rows.Clear();
            //LVData.Items.Clear();
            editPartNo.Text = editPartNo.Text.Trim();
            editVendorCode.Text = editVendorCode.Text.Trim();
            editRTNo.Text = editRTNo.Text.Trim();
            editReelNo.Text = editReelNo.Text.Trim();
            editDatecode.Text = editDatecode.Text.Trim();
            editLot.Text = editLot.Text.Trim();
            sSQL = "Select a.*,b.part_no,c.vendor_code,c.VENDOR_NAME "
                 + "  ,NVL(D.RT_NO,'N/A') rt_no "
                 + " from sajet.g_material a "
                 + "   ,sajet.sys_part b "
                 + "   ,sajet.sys_vendor c "
                 + "   ,sajet.g_erp_rtno d "
                 + " WHERE  a.part_id = b.part_id "
                 + " and a.vendor_id = c.vendor_id(+)";
            if (chkbDate.Checked)
            {
                Params = new object[2][];
                sSQL = sSQL + " AND A.INVENTORY_TIME >=TO_DATE(:START_TIME,'YYYY/MM/DD') "
                            + " AND A.INVENTORY_TIME <=TO_DATE(:END_TIME,'YYYY/MM/DD HH24:MI:SS') ";
                string sStartDate = DateTime.Parse(dtStart.Text).ToString("yyyy/MM/dd");
                string sEndDate = DateTime.Parse(dtEnd.Text).ToString("yyyy/MM/dd") + " 23:59:59";
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "START_TIME", sStartDate };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "END_TIME", sEndDate };
            }
            if (!string.IsNullOrEmpty(editPartNo.Text))
            {
                sSQL = sSQL + " AND B.PART_NO LIKE '" + editPartNo.Text + "%' ";
            }
            if (!string.IsNullOrEmpty(editVendorCode.Text))
            {
                sSQL = sSQL + " AND C.VENDOR_CODE LIKE  '" + editVendorCode.Text + "%' ";
            }
            if (!string.IsNullOrEmpty(editRTNo.Text))
            {
                sSQL = sSQL + " AND D.RT_NO LIKE   '" + editRTNo.Text + "%' "
                            + " and a.rt_id = d.rt_id ";
            }
            else
                sSQL = sSQL + " and a.rt_id = d.rt_id(+) ";
            if (!string.IsNullOrEmpty(editReelNo.Text))
            {
                sSQL = sSQL + " AND A.REEL_NO LIKE   '" + editReelNo.Text + "%' ";
            };
            if (!string.IsNullOrEmpty(editDatecode.Text))
            {
                sSQL = sSQL + " AND A.DATECODE LIKE   '" + editDatecode.Text + "%' ";
            };
            if (!string.IsNullOrEmpty(editLot.Text))
            {
                sSQL = sSQL + " AND A.LOT LIKE   '" + editLot.Text + "%' ";
            };
            sSQL = sSQL + " ORDER BY REEL_NO ";

            dsTemp = ClientUtils.ExecuteSQL(sSQL,Params);

            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                DataRow dr = dsTemp.Tables[0].Rows[i];
                dgvData.Rows.Add();
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["REEL_NO"].Value = dr["REEL_NO"].ToString();
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["PART_NO"].Value = dr["PART_NO"].ToString();
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["REEL_QTY"].Value = dr["REEL_QTY"].ToString();
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["DATECODE"].Value = dr["DATECODE"].ToString();
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["LOT"].Value = dr["LOT"].ToString();
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["VENDOR_CODE"].Value = dr["VENDOR_CODE"].ToString();
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["VENDOR_NAME"].Value = dr["VENDOR_NAME"].ToString();
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["RT_NO"].Value = dr["RT_NO"].ToString();
                dgvData.Rows[dgvData.Rows.Count - 1].Cells["REEL_MEMO"].Value = dr["REEL_MEMO"].ToString();
            }
            grpbResult.Text = g_sgrpbResult + " : " + dgvData.Rows.Count.ToString();

            //for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            //{
            //    LVData.Items.Add(dsTemp.Tables[0].Rows[i]["REEL_NO"].ToString());
            //    LVData.Items[LVData.Items.Count - 1].Name = dsTemp.Tables[0].Rows[i]["REEL_NO"].ToString();
            //    LVData.Items[LVData.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["PART_NO"].ToString());
            //    LVData.Items[LVData.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["REEL_QTY"].ToString());
            //    LVData.Items[LVData.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["DATECODE"].ToString());
            //    LVData.Items[LVData.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["LOT"].ToString());
            //    LVData.Items[LVData.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["VENDOR_CODE"].ToString());
            //    LVData.Items[LVData.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["VENDOR_NAME"].ToString());
            //    LVData.Items[LVData.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["RT_NO"].ToString());
            //    LVData.Items[LVData.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[i]["REEL_MEMO"].ToString());
            //}
            //grpbResult.Text = SajetCommon.SetLanguage("Query Result") + " : " + LVData.Items.Count.ToString();

            btnSelectAll_Click(btnSelectAll, e);


        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            for (int i = dgvData.Rows.Count - 1; i >= 0; i--)
            {
                string s_gStatus = "";
                if (dgvData.Rows[i].Cells["CHECKED"].Value.ToString() == "Y")
                {
                    for (int j = 0; j < dgvRT.Rows.Count; j++)
                    {
                        if (dgvRT.Rows[j].Cells["REEL_NO1"].Value.ToString() == dgvData.Rows[i].Cells["REEL_NO"].Value.ToString())
                        { s_gStatus = "Depli"; }
                    }
                    if (s_gStatus == "")
                    {
                        dgvRT.Rows.Add();
                        dgvRT.Rows[dgvRT.Rows.Count - 1].Cells["REEL_NO1"].Value = dgvData.Rows[i].Cells["REEL_NO"].Value.ToString();
                        dgvRT.Rows[dgvRT.Rows.Count - 1].Cells["PART_NO1"].Value = dgvData.Rows[i].Cells["PART_NO"].Value.ToString();
                        dgvRT.Rows[dgvRT.Rows.Count - 1].Cells["REEL_QTY1"].Value = dgvData.Rows[i].Cells["REEL_QTY"].Value.ToString();
                        dgvRT.Rows[dgvRT.Rows.Count - 1].Cells["DATECODE1"].Value = dgvData.Rows[i].Cells["DATECODE"].Value.ToString();
                        dgvRT.Rows[dgvRT.Rows.Count - 1].Cells["LOT1"].Value = dgvData.Rows[i].Cells["LOT"].Value.ToString();
                        dgvRT.Rows[dgvRT.Rows.Count - 1].Cells["VENDOR_CODE1"].Value = dgvData.Rows[i].Cells["VENDOR_CODE"].Value.ToString();
                        dgvRT.Rows[dgvRT.Rows.Count - 1].Cells["VENDOR_NAME1"].Value = dgvData.Rows[i].Cells["VENDOR_NAME"].Value.ToString();
                        dgvRT.Rows[dgvRT.Rows.Count - 1].Cells["RT_NO1"].Value = dgvData.Rows[i].Cells["RT_NO"].Value.ToString();
                        dgvRT.Rows[dgvRT.Rows.Count - 1].Cells["REEL_MEMO1"].Value = dgvData.Rows[i].Cells["REEL_MEMO"].Value.ToString();
                        dgvData.Rows.RemoveAt(i);
                    }
                    else { dgvData.Rows.RemoveAt(i); }
                }
            }

            grpbResult.Text = g_sgrpbResult + " : " + dgvData.Rows.Count.ToString();
            gbChange.Text = g_sRecordText + " : " + dgvRT.Rows.Count.ToString();
                       

            //for (int i = LVData.CheckedItems.Count-1; i >=0 ;i--)
            //{
            //    int iIndex = LVData.CheckedIndices[i];
            // //   ListViewItem LVItem = LVData.CheckedItems[i];
            //    string sReelNo = LVData.Items[iIndex].Name;

            //    ListViewItem[] Item = LVRT.Items.Find(sReelNo, false);
            //    if (Item.Length <= 0)
            //    {
            //        LVRT.Items.Add(LVData.Items[iIndex].Name);
            //        LVRT.Items[LVRT.Items.Count - 1].Name = LVData.Items[iIndex].Name;
            //        LVRT.Items[LVRT.Items.Count - 1].SubItems.Add(LVData.Items[iIndex].SubItems[1].Text);
            //        LVRT.Items[LVRT.Items.Count - 1].SubItems.Add(LVData.Items[iIndex].SubItems[2].Text);
            //        LVRT.Items[LVRT.Items.Count - 1].SubItems.Add(LVData.Items[iIndex].SubItems[3].Text);
            //        LVRT.Items[LVRT.Items.Count - 1].SubItems.Add(LVData.Items[iIndex].SubItems[4].Text);
            //        LVRT.Items[LVRT.Items.Count - 1].SubItems.Add(LVData.Items[iIndex].SubItems[5].Text);
            //        LVRT.Items[LVRT.Items.Count - 1].SubItems.Add(LVData.Items[iIndex].SubItems[6].Text);
            //        LVRT.Items[LVRT.Items.Count - 1].SubItems.Add(LVData.Items[iIndex].SubItems[7].Text);
            //        LVRT.Items[LVRT.Items.Count - 1].SubItems.Add(LVData.Items[iIndex].SubItems[8].Text);
            //    }
            //     LVData.Items.RemoveAt(iIndex);
            //}
            //gbChange.Text = g_sRecordText + " : " + LVRT.Items.Count.ToString();
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            string bchecked = "N";
            if ((sender as Button).Tag.ToString() == "0")
            {
                bchecked = "Y";
            }
            for (int i = 0; i <= dgvData.Rows.Count - 1; i++)
            {
                dgvData.Rows[i].Cells["CHECKED"].Value = bchecked;
            }
            //bool bchecked = false;
            //if ((sender as Button).Tag.ToString() =="0")
            //{
            //    bchecked = true;
            //}
            //for (int i = 0; i <= LVData.Items.Count - 1; i++)
            //{
            //    LVData.Items[i].Checked = bchecked;
            //}
        }
//---------------------------------------以下OK-----------------------------------------
        private void button2_Click(object sender, EventArgs e)
        {
            sSQL = "Select VENDOR_CODE,VENDOR_NAME from sajet.sys_vendor  "
                 + "Where enabled = 'Y' ";
            if (editVendorCode.Text != "")
                sSQL = sSQL + " AND  VENDOR_CODE like '" + editVendorCode.Text + "%'";
            sSQL = sSQL + "order by VENDOR_CODE ";

            fFilter f = new fFilter();
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                if (f.dgvData.CurrentRow != null)
                {
                    editVendorCode.Text = f.dgvData.CurrentRow.Cells["VENDOR_CODE"].Value.ToString();
                }
            }
        }

        private void btnPartNo_Click(object sender, EventArgs e)
        {
            if (editPartNo.Text == "")
            {
                SajetCommon.Show_Message("Please Input Part No Prefix", 0);
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
                }
            }
        }

        private void btnReelNo_Click(object sender, EventArgs e)
        {
            sSQL = "Select Reel_no from sajet.g_material  ";
            if (editReelNo.Text != "")
                sSQL = sSQL + "WHERE  REEL_NO like '" + editReelNo.Text + "%'";
            sSQL = sSQL + "order by REEL_NO ";

            fFilter f = new fFilter();
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                if (f.dgvData.CurrentRow != null)
                {
                    editReelNo.Text = f.dgvData.CurrentRow.Cells["REEL_NO"].Value.ToString();
                }
            }
        }

        private void btnRTNo_Click(object sender, EventArgs e)
        {
            sSQL = "Select RT_NO from sajet.g_ERP_RTNO  ";
            if (editRTNo.Text != "")
                sSQL = sSQL + "WHERE  RT_NO like '" + editRTNo.Text + "%'";
            sSQL = sSQL + "order by RT_NO ";

            fFilter f = new fFilter();
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                if (f.dgvData.CurrentRow != null)
                {
                    editRTNo.Text = f.dgvData.CurrentRow.Cells["RT_NO"].Value.ToString();
                }
            }
        }

        private void combLabelType_SelectedIndexChanged(object sender, EventArgs e)
        {
            combLabelTypeFile.SelectedIndex = combLabelType.SelectedIndex;
        }

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count == 0 || dgvData.SelectedCells.Count == 0 || dgvData.CurrentCell == null)
                return;
           

            fModifyQty f = new fModifyQty();
            f.sReel = dgvData.CurrentRow.Cells["REEL_NO"].Value.ToString();
            f.editQty.Text = dgvData.CurrentRow.Cells["REEL_QTY"].Value.ToString();
            f.g_sUserID = g_sUserID;
            if (f.ShowDialog() == DialogResult.OK)
            {
                dgvData.CurrentRow.Cells["REEL_QTY"].Value = f.sQty;
            }  
        }
    }

}

