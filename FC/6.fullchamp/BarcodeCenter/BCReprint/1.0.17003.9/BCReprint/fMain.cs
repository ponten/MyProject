using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using SajetClass;
using System.Globalization;
using Microsoft.Win32;
using System.Data.OracleClient;
using System.Linq;
using PrSrv = BCReprintDll.Services.PrintLabelService;

namespace BCReprintDll
{
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();

            BtnRCList.Click += BtnRCList_Click;
        }

        ListBox g_listbLabelName, g_listbType;
        string g_sUserID;
        string g_sProgram, g_sFunction;
        public static string g_sExeName;
        //sys_label定義的欄位值========              
        string g_sFieldName;
        string g_sTable;
        string g_sFileName;
        string g_sFieldType;
        string g_sReprintSQL, g_sReprintSQL2;
        //Config中設定的列印相關方式===========        
        int g_iSetup_PrintQty;
        string g_sSetup_PrintMethod;
        string g_sSetup_PrintPort;
        string g_sSetup_CodeSoftVer;
        int g_sSetup_iLabelQty;
        //===
        string sSQL;
        DataSet dsTemp;
        bool g_bPrintSortDesc;

        public int check_privilege(string sFunction)
        {
            int iPrivilege = ClientUtils.GetPrivilege(g_sUserID, "Reprint " + sFunction, g_sProgram);
            return iPrivilege;
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            panel1.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            groupBox2.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            groupBox2.BackgroundImageLayout = ImageLayout.Stretch;

            g_sExeName = ClientUtils.fCurrentProject;
            g_sProgram = ClientUtils.fProgramName;
            g_sFunction = ClientUtils.fFunctionName;
            g_sUserID = ClientUtils.UserPara1;

            g_listbLabelName = new ListBox();
            g_listbType = new ListBox();
            Get_ReprintType();

            //讀取SYS_BASE中設定值
            string sErrMsg = "";
            g_bPrintSortDesc = (SajetCommon.GetSysBaseData(g_sProgram, "Barcode Print Sort", ref sErrMsg) == "Sort Desc");
            if (sErrMsg != "")
            {
                sErrMsg = "Please Setup System Parameter:" + Environment.NewLine + Environment.NewLine + sErrMsg;
                SajetCommon.Show_Message(sErrMsg, 0);
                btnReprint.Enabled = false;
                return;
            }
            ClientUtils.SetLanguage(this, g_sExeName);
            this.Text = this.Text + " (" + SajetCommon.g_sFileVersion + ")";
        }

        private void fMain_Shown(object sender, EventArgs e)
        {
            combReprint.Focus();

            InitializeControlsPosition();
        }

        private void combReprint_SelectedIndexChanged(object sender, EventArgs e)
        {
            combType.Items.Clear();
            string sLabelName = g_listbLabelName.Items[combReprint.SelectedIndex].ToString();

            LoadConfigData(sLabelName);

            sSQL = " select * from sajet.sys_label "
                 + " where label_name = '" + sLabelName + "' "
                 + " and rownum = 1";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            g_sTable = dsTemp.Tables[0].Rows[0]["Table_Name"].ToString();
            g_sFieldName = dsTemp.Tables[0].Rows[0]["Field_Name"].ToString();
            g_sFileName = dsTemp.Tables[0].Rows[0]["File_name"].ToString();
            g_sFieldType = dsTemp.Tables[0].Rows[0]["reprint_field"].ToString();
            g_sReprintSQL = dsTemp.Tables[0].Rows[0]["reprint_sql"].ToString();
            g_sReprintSQL2 = dsTemp.Tables[0].Rows[0]["reprint_sql2"].ToString();
            LabDesc.Text = g_sFileName + " + Part No (Default)";
            if (!string.IsNullOrEmpty(g_sFieldType))
            {
                string[] sTypeList = g_sFieldType.Split(',');
                foreach (string sType in sTypeList)
                    combType.Items.Add(SajetCommon.SetLanguage(sType, 1));
            }
            combType.Items.Add(combReprint.Text);
            combType.SelectedIndex = 0;
        }

        private void combType_SelectedIndexChanged(object sender, EventArgs e)
        {
            editInput.Select();
            editInput.SelectAll();
            editInput.Text = "";
            editInput2.Text = "";
            LVChoose.Items.Clear();
            LVAll.Items.Clear();
            lablSN.Visible = false;

            int default_distance = 10;

            if (combType.SelectedIndex == combType.Items.Count - 1)
            {
                lablSN.Visible = true;

                btnQuery.Left = editInput2.Left + editInput2.Width + default_distance;
            }
            else
            {
                btnQuery.Left = editInput.Left + editInput.Width + default_distance;
            }

            editInput2.Visible = lablSN.Visible;

            btnReprint.Left = btnQuery.Left + btnQuery.Width + default_distance;

            CbWaterproof.Left = btnReprint.Left;

            int left_1 = CbWaterproof.Right;

            int left_2 = btnReprint.Right;

            if (left_1 > left_2)
            {
                BtnRCList.Left = left_1 + default_distance * 3;
            }
            else
            {
                BtnRCList.Left = left_2 + default_distance * 3;
            }

        }

        private void editInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue != 13) return;
            if (editInput2.Visible)
            {
                editInput2.Focus();
                editInput2.SelectAll();
            }
            else
            {
                ShowData();
                editInput.SelectAll();
            }
        }

        private void editInput2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue != 13) return;
            ShowData();
            editInput.SelectAll();
            editInput.Focus();
        }

        private void btnReprint_Click(object sender, EventArgs e)
        {
            if (LVChoose.Items.Count == 0)
            {
                return;
            }

            string confirm
                = SajetCommon.SetLanguage("Print")
                + combReprint.Text + " "
                + SajetCommon.SetLanguage("Label")
                + SajetCommon.SetLanguage("?")
                + Environment.NewLine
                + SajetCommon.SetLanguage("Total")
                + SajetCommon.SetLanguage(":")
                + Convert.ToString(LVChoose.Items.Count);

            if (SajetCommon.Show_Message(confirm, 2) != DialogResult.Yes)
            {
                return;
            }

            if (g_bPrintSortDesc)
            {
                LVChoose.Sorting = SortOrder.Descending;
            }

            //LVChoose.Items[0].SubItems[1]

            if (CbWaterproof.Checked)
            {
                var rc_no_list = new List<string>();

                for (int i = 0; i < LVChoose.Items.Count; i++)
                {
                    rc_no_list.Add(LVChoose.Items[i].SubItems[1].Text.Trim());
                }

                rc_no_list = rc_no_list.OrderBy(x => x).ToList();

                bool print_success =
                    PrSrv.PrintLabel(
                        rc_list: rc_no_list,
                        out string message);

                if (!print_success)
                {
                    SajetCommon.Show_Message(message, 0);
                }
            }
            else
            {
                Print_Label(LVChoose);
            }

            LVChoose.Sorting = SortOrder.Ascending;
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            if (LVAll.SelectedItems.Count == 0) return;
            LVChoose.Sorting = SortOrder.None;
            for (int i = LVAll.SelectedItems.Count - 1; i >= 0; i--)
            {
                LVChoose.Items.Add(LVAll.SelectedItems[i].Text);
                for (int j = 1; j < LVAll.Columns.Count; j++)
                    LVChoose.Items[LVChoose.Items.Count - 1].SubItems.Add(LVAll.SelectedItems[i].SubItems[j]);
                LVChoose.Items[LVChoose.Items.Count - 1].ImageIndex = 0;
                LVAll.SelectedItems[i].Remove();
            }
            LVChoose.Sorting = SortOrder.Ascending;

            gbChoose.Text
                = SajetCommon.SetLanguage("Choose")
                + SajetCommon.SetLanguage(":")
                + LVChoose.Items.Count.ToString();
        }

        private void btnUnchoose_Click(object sender, EventArgs e)
        {
            if (LVChoose.SelectedItems.Count == 0) return;
            LVAll.Sorting = SortOrder.None;
            for (int i = LVChoose.SelectedItems.Count - 1; i >= 0; i--)
            {
                LVAll.Items.Add(LVChoose.SelectedItems[i].Text);
                for (int j = 1; j < LVChoose.Columns.Count; j++)
                    LVAll.Items[LVAll.Items.Count - 1].SubItems.Add(LVChoose.SelectedItems[i].SubItems[j]);
                LVAll.Items[LVAll.Items.Count - 1].ImageIndex = 0;
                LVChoose.SelectedItems[i].Remove();
            }
            LVAll.Sorting = SortOrder.Ascending;

            gbChoose.Text
                = SajetCommon.SetLanguage("Choose")
                + SajetCommon.SetLanguage(":")
                + LVChoose.Items.Count.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShowData();
        }

        private void BtnRCList_Click(object sender, EventArgs e)
        {
            using (var f = new fRCList())
            {
                f.ShowDialog();
            }
        }

        public bool Print_Label(ListView listbPrint)
        {
            string sFile = Application.StartupPath + "\\" + g_sExeName + "\\BarcodeCenter.ini";
            //Sleep幾秒
            SajetInifile sajetInifile1 = new SajetInifile();
            int iSleepSec = Convert.ToInt32(sajetInifile1.ReadIniFile(sFile, "Print Label", "Delay Second", "0"));
            //每印幾張就Sleep一次
            int iPrintCount = Convert.ToInt32(sajetInifile1.ReadIniFile(sFile, "Print Label", "Print Count", "100"));
            //對應S_SYS_PRINT_DATA的DATA_TYPE欄位 
            string sDataType = "BC_" + g_listbLabelName.Items[combReprint.SelectedIndex].ToString();
            string sLabType = g_listbLabelName.Items[combReprint.SelectedIndex].ToString();

            //先Link====
            PrintLabel.Setup PrintLabelDll = new PrintLabel.Setup();
            if (g_sSetup_PrintMethod.ToUpper() == "CODESOFT")
            {
                PrintLabelDll.Open(g_sSetup_PrintMethod.ToUpper());
            }
            else if (g_sSetup_PrintMethod.ToUpper() == "BARTENDER" && g_sSetup_PrintPort.ToUpper() == "STANDARD")
            {
                PrintLabelDll.Open(g_sSetup_PrintMethod.ToUpper());
            }
            else if (g_sSetup_PrintMethod.ToUpper() == "DLL")
            {
                PrintLabelDll.Open(g_sSetup_PrintPort.ToUpper());
            }

            //Print=====
            try
            {
                progressBar1.Minimum = 0;
                progressBar1.Maximum = listbPrint.Items.Count - 1;
                progressBar1.Visible = true;
                int iIndex = listbPrint.Columns[g_sFieldName].Index;
                if (g_sSetup_PrintMethod.ToUpper() == "BARTENDER" && g_sSetup_PrintPort.ToUpper() == "DATASOURCE")
                {
                    ListParam.Items.Clear();
                    ListData.Items.Clear();
                    string sMessage = "";
                    for (int i = 0; i <= listbPrint.Items.Count - 1; i++)
                    {
                        ListData.Items.Add(listbPrint.Items[i].SubItems[iIndex].Text);
                    }
                    PrintLabelDll.Print_Bartender_DataSource(g_sExeName, sDataType, g_sFileName, "", g_iSetup_PrintQty, g_sSetup_PrintMethod, g_sSetup_PrintPort, ListParam, ListData, out sMessage);
                    if (sMessage != "OK")
                    {
                        SajetCommon.Show_Message(sMessage, 0);
                        return false;
                    }
                    return true;
                }

                for (int i = 0; i <= listbPrint.Items.Count - 1; i++)
                {
                    string sMessage = "";
                    //因應一張Label有多個號碼,每幾個才去Print一次   
                    if (g_sSetup_iLabelQty > 1)
                    {
                        int iNo = (i % g_sSetup_iLabelQty) + 1;
                        //第一張才需去Select Data
                        if (iNo == 1)
                        {
                            ListParam.Items.Clear();
                            ListData.Items.Clear();
                            ListData.Items.Add(listbPrint.Items[i].SubItems[iIndex].Text);
                            //ListData.Items.Add(listbPrint.Items[i].Text);
                            //各變數值                        
                            PrintLabelDll.GetPrintData(sDataType, ref ListParam, ref ListData);
                        }

                        ListParam.Items.Add(sLabType.ToUpper() + "_" + iNo);
                        ListData.Items.Add(listbPrint.Items[i].SubItems[iIndex].Text);

                        //每幾張才去列印一次
                        if (iNo == g_sSetup_iLabelQty || (iNo < g_sSetup_iLabelQty && i == listbPrint.Items.Count - 1))
                        {
                            //開始列印
                            if (g_sSetup_PrintMethod.ToUpper() == "CODESOFT")
                                PrintLabelDll.Print_MultiLabel(g_sExeName, sLabType, g_sFileName, "", g_iSetup_PrintQty, g_sSetup_PrintMethod, g_sSetup_CodeSoftVer, ListParam, ListData, out sMessage);
                            else
                                PrintLabelDll.Print_MultiLabel(g_sExeName, sLabType, g_sFileName, "", g_iSetup_PrintQty, g_sSetup_PrintMethod, g_sSetup_PrintPort, ListParam, ListData, out sMessage);
                            if (sMessage != "OK")
                            {
                                SajetCommon.Show_Message(sMessage, 0);
                                return false;
                            }
                        }
                    }
                    else
                    {
                        ListParam.Items.Clear();
                        ListData.Items.Clear();
                        ListData.Items.Add(listbPrint.Items[i].SubItems[iIndex].Text);

                        //各變數值                        
                        PrintLabelDll.GetPrintData(sDataType, ref ListParam, ref ListData);
                        //開始列印
                        if (g_sSetup_PrintMethod.ToUpper() == "CODESOFT")
                            PrintLabelDll.Print(g_sExeName, sDataType, g_sFileName, "", g_iSetup_PrintQty, g_sSetup_PrintMethod, g_sSetup_CodeSoftVer, ListParam, ListData, out sMessage);
                        else
                            PrintLabelDll.Print(g_sExeName, sDataType, g_sFileName, "", g_iSetup_PrintQty, g_sSetup_PrintMethod, g_sSetup_PrintPort, ListParam, ListData, out sMessage);
                        if (sMessage != "OK")
                        {
                            SajetCommon.Show_Message(sMessage, 0);
                            return false;
                        }
                    }
                    progressBar1.Value = i;
                    //為了避免CodeSoft列印會亂跳,每印100張就Sleep,等待CodeSoft清除Buffer
                    if ((iSleepSec > 0) && (i < listbPrint.Items.Count - 1))
                    {
                        if ((i + 1) % iPrintCount == 0)
                            System.Threading.Thread.Sleep(iSleepSec * 1000);
                    }
                }
                return true;
            }
            finally
            {
                progressBar1.Visible = false;
                if (g_sSetup_PrintMethod.ToUpper() == "CODESOFT")
                    PrintLabelDll.Close(g_sSetup_PrintMethod.ToUpper());
                else if (g_sSetup_PrintMethod.ToUpper() == "BARTENDER" && g_sSetup_PrintPort.ToUpper() == "STANDARD")
                    PrintLabelDll.Close(g_sSetup_PrintMethod.ToUpper());
                else if (g_sSetup_PrintMethod.ToUpper() == "DLL")
                    PrintLabelDll.Close(g_sSetup_PrintPort.ToUpper());
            }
        }

        public void LoadConfigData(string sLabType)
        {
            string sFile = Application.StartupPath + "\\Sajet.ini";
            SajetInifile sajetInifile1 = new SajetInifile();
            string sPrint = sajetInifile1.ReadIniFile(sFile, g_sProgram, "Print " + sLabType, "0");

            g_iSetup_PrintQty = Convert.ToInt32(sajetInifile1.ReadIniFile(sFile, g_sProgram, sLabType + " Qty", "1"));
            g_sSetup_PrintMethod = sajetInifile1.ReadIniFile(sFile, g_sProgram, "Print " + sLabType + " Method", "CodeSoft");
            g_sSetup_PrintPort = sajetInifile1.ReadIniFile(sFile, g_sProgram, sLabType + " ComPort", "");
            g_sSetup_CodeSoftVer = sajetInifile1.ReadIniFile(sFile, g_sProgram, "Code Soft Version", "6");
            g_sSetup_iLabelQty = Convert.ToInt32(sajetInifile1.ReadIniFile(sFile, g_sProgram, sLabType + " Label Qty", "1"));

            //顯示在畫面上            
            LabPrintQty.Text = g_iSetup_PrintQty.ToString();
            LabPrintMethod.Text = g_sSetup_PrintMethod;
        }

        public void Get_ReprintType()
        {
            g_listbLabelName.Items.Clear();
            sSQL = " select label_name from sajet.sys_label "
                 + " where type <> 'U' "
                 + " order by label_name ";
            DataSet dsLabel = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i <= dsLabel.Tables[0].Rows.Count - 1; i++)
            {
                string sLabel = dsLabel.Tables[0].Rows[i]["label_name"].ToString();
                //檢查是否有列印此Label的權限
                if (check_privilege(sLabel) > 0)
                {
                    combReprint.Items.Add(sLabel);
                    g_listbLabelName.Items.Add(sLabel);
                    string sSQL1 = " select * from sajet.sys_label "
                                 + " where qty_field = '" + sLabel + "' "
                                 + " and rownum = 1 ";
                    DataSet dsTemp1 = ClientUtils.ExecuteSQL(sSQL1);
                    if (dsTemp1.Tables[0].Rows.Count > 0)
                    {
                        combReprint.Items.Add(dsTemp1.Tables[0].Rows[0]["label_name"].ToString());
                        g_listbLabelName.Items.Add(dsTemp1.Tables[0].Rows[0]["label_name"].ToString());
                    }
                }
            }
        }

        public void ShowData()
        {
            LVChoose.Items.Clear();
            LVAll.Columns.Clear();
            LVAll.Items.Clear();
            if (combReprint.SelectedIndex == -1) return;
            if (combType.SelectedIndex == -1) return;
            if (string.IsNullOrEmpty(editInput.Text)) return;

            string sField = g_sFieldName;
            if (combType.SelectedIndex == combType.Items.Count - 1)
                sField = g_sFieldName;
            else
            {
                string[] slType = g_sFieldType.Split(',');
                sField = slType[combType.SelectedIndex];
            }
            if (!editInput2.Visible && !string.IsNullOrEmpty(g_sReprintSQL))
            {
                sSQL = g_sReprintSQL.Replace("@", sField);
                object[][] Params = new object[1][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DATA", editInput.Text };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            }
            else if (editInput2.Visible && !string.IsNullOrEmpty(g_sReprintSQL2))
            {
                sSQL = g_sReprintSQL2.Replace("@", sField);
                object[][] Params = new object[2][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DATA", editInput.Text };
                if (!string.IsNullOrEmpty(editInput2.Text))
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DATA2", editInput2.Text };
                else
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DATA2", editInput.Text };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
            }
            else
            {
                if (string.IsNullOrEmpty(g_sFieldType))
                    sSQL = " select " + g_sFieldName;
                else
                    sSQL = " select " + g_sFieldType + "," + g_sFieldName;
                sSQL = sSQL + " from " + g_sTable;
                if (!string.IsNullOrEmpty(editInput2.Text))
                    sSQL = sSQL + " where " + sField + " >= '" + editInput.Text.Trim() + "' "
                      + " and " + sField + " <= '" + editInput2.Text.Trim() + "' ";
                else
                    sSQL = sSQL + " where " + sField + " = '" + editInput.Text.Trim() + "' ";
                sSQL = sSQL + " and " + g_sFieldName + " <> 'N/A' ";
                if (string.IsNullOrEmpty(g_sFieldType))
                    sSQL = sSQL + " group by " + g_sFieldName;
                else
                    sSQL = sSQL + " group by " + g_sFieldType + "," + g_sFieldName;
                sSQL = sSQL + " order by " + g_sFieldName;
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
            }
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                SajetCommon.Show_Message("No Data", 0);
                editInput.Focus();
                editInput.SelectAll();
                return;
            }

            gbAll.Text
                = SajetCommon.SetLanguage("All")
                + combReprint.Text
                + SajetCommon.SetLanguage(":")
                + dsTemp.Tables[0].Rows.Count.ToString();

            LVAll.Sorting = SortOrder.None;
            for (int i = 0; i < dsTemp.Tables[0].Columns.Count; i++)
            {
                LVAll.Columns.Add(dsTemp.Tables[0].Columns[i].Caption, SajetCommon.SetLanguage(dsTemp.Tables[0].Columns[i].Caption));
            }
            for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
            {
                LVAll.Items.Add(dsTemp.Tables[0].Rows[i][0].ToString());
                for (int j = 1; j < dsTemp.Tables[0].Columns.Count; j++)
                    LVAll.Items[i].SubItems.Add(dsTemp.Tables[0].Rows[i][j].ToString());
                LVAll.Items[i].ImageIndex = 0;
            }
            for (int i = 0; i < dsTemp.Tables[0].Columns.Count; i++)
                LVAll.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.HeaderSize);
            LVChoose.Columns.Clear();

            gbChoose.Text
                = SajetCommon.SetLanguage("Choose")
                + SajetCommon.SetLanguage(":")
                + LVChoose.Items.Count.ToString();

            for (int i = 0; i < LVAll.Columns.Count; i++)
            {
                LVChoose.Columns.Add(LVAll.Columns[i].Name, LVAll.Columns[i].Text);
                LVChoose.Columns[i].Width = LVAll.Columns[i].Width;
            }
        }

        /// <summary>
        /// 重新編排上方列項目的位置
        /// </summary>
        private void InitializeControlsPosition()
        {
            int i_y_position;

            int default_distance = 10;

            {
                i_y_position = label1.Left;

                label2.Left = i_y_position;
            }

            {
                i_y_position = label1.Right;

                if (label2.Right > i_y_position)
                {
                    i_y_position = label2.Right;
                }

                combReprint.Left = i_y_position + default_distance;

                combType.Left = i_y_position + default_distance;
            }

            {
                i_y_position = combReprint.Right;

                if (combType.Right > i_y_position)
                {
                    i_y_position = combType.Right;
                }

                editInput.Left = i_y_position + default_distance;
            }

            lablSN.Left = editInput.Right + default_distance;

            editInput2.Left = lablSN.Right + default_distance;

            btnQuery.Left = editInput2.Right + default_distance;

            btnReprint.Left = btnQuery.Right + default_distance;

            CbWaterproof.Left = btnQuery.Right + default_distance;

            {
                i_y_position = CbWaterproof.Right;

                if (btnReprint.Right > i_y_position)
                {
                    i_y_position = btnReprint.Right;
                }

                BtnRCList.Left = i_y_position + default_distance * 3;
            }
        }
    }
}

