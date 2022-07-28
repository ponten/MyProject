using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Win32;
using System.Globalization;
using SajetClass;

namespace BCConfig
{
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();
        }

        public int g_iPrivilege = 0;
        public string g_sUserID;
        public string sServerName = "SApServer";
        public string sFileName = "BCConfig.dll";
        string g_sFile = Application.StartupPath + "\\sajet.ini";
        public static string g_sExeName;
        string g_sProgram, g_sFunction;
        string sSQL;
        DataSet dsTemp;
        DataSet g_dsPort;

        public void check_privilege()
        {
            btnSave.Enabled = false;
            g_iPrivilege = ClientUtils.GetPrivilege(g_sUserID, ClientUtils.fFunctionName, g_sProgram);
            btnSave.Enabled = (g_iPrivilege >= 1);
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            panel1.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
            panel1.BackgroundImageLayout = ImageLayout.Stretch;

            g_sExeName = ClientUtils.fCurrentProject;
            g_sUserID = ClientUtils.UserPara1;
            g_sFunction = ClientUtils.fFunctionName;
            g_sProgram = ClientUtils.fProgramName;

            check_privilege();

            //讀取SYS_BASE
            string sMsg = "";
            string sValue = GetSysBaseDatastring("Barcode Print Port", ref sMsg); //有定義的Port
            if (!string.IsNullOrEmpty(sMsg))
            {
                sMsg = "Please Setup System Parameter:" + Environment.NewLine + Environment.NewLine + sMsg;
                SajetCommon.Show_Message(sMsg, 0);
                btnSave.Enabled = false;
                return;
            }

            PrintMethod.Items.Clear();
            PrintMethod.Items.Add("CodeSoft");
            PrintMethod.Items.Add("Bartender");
            PrintMethod.Items.Add("DLL");

            //Port=======================                     
            string[] sPort = sValue.Split(new Char[] { ',' });
            g_dsPort = new DataSet();
            //DLL                        
            DataTable dt = new DataTable("DLL");
            dt.Columns.Add("PORT");
            for (int i = 0; i <= sPort.Length - 1; i++)
                dt.Rows.Add(sPort[i].ToString());
            g_dsPort.Tables.Add(dt);
            //Codesoft                        
            dt = new DataTable("CODESOFT");
            dt.Columns.Add("PORT");
            dt.Rows.Add("N/A");
            g_dsPort.Tables.Add(dt);
            //Bartender                        
            dt = new DataTable("BARTENDER");
            dt.Columns.Add("PORT");
            dt.Rows.Add("Standard");
            dt.Rows.Add("DataSource");
            g_dsPort.Tables.Add(dt);
            //=========================

            Show_Label();
            ClientUtils.SetLanguage(this, g_sExeName);
            this.Text = this.Text + " (" + FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion.ToString() + ")";
        }

        public void Show_Label()
        {
            //顯示所有使用的Label的設定                                        
            sSQL = " select label_name from sajet.sys_label "
                 + " where type is not null "
                 + " order by label_name ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            try
            {
                SajetInifile sajetInifile1 = new SajetInifile();
                for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
                {
                    string sLabel = dsTemp.Tables[0].Rows[i]["label_name"].ToString();
                    string sPrintLabel = sajetInifile1.ReadIniFile(g_sFile, g_sProgram, "Print " + sLabel, "0");
                    string sPrintQty = sajetInifile1.ReadIniFile(g_sFile, g_sProgram, sLabel + " Qty", "1");
                    string sPrintMethod = sajetInifile1.ReadIniFile(g_sFile, g_sProgram, "Print " + sLabel + " Method", "CodeSoft");
                    string sPrintPort = sajetInifile1.ReadIniFile(g_sFile, g_sProgram, sLabel + " ComPort", "");
                    string sLabelQty = sajetInifile1.ReadIniFile(g_sFile, g_sProgram, sLabel + " Label Qty", "1");

                    dvGrid.Rows.Add();
                    dvGrid.Rows[dvGrid.Rows.Count - 1].Cells[0].Value = sLabel;
                    dvGrid.Rows[dvGrid.Rows.Count - 1].Cells[1].Value = sPrintLabel;
                    dvGrid.Rows[dvGrid.Rows.Count - 1].Cells[2].Value = sPrintQty;
                    dvGrid.Rows[dvGrid.Rows.Count - 1].Cells[3].Value = sPrintMethod;
                    dvGrid.Rows[dvGrid.Rows.Count - 1].Cells[4].Value = sPrintPort;
                    dvGrid.Rows[dvGrid.Rows.Count - 1].Cells[5].Value = sLabelQty;
                }
                string sCodeSoftVer = sajetInifile1.ReadIniFile(g_sFile, g_sProgram, "Code Soft Version", "6");
                combCodeSoft.SelectedIndex = combCodeSoft.FindString("Version " + sCodeSoftVer);
                if (combCodeSoft.SelectedIndex == -1)
                    combCodeSoft.SelectedIndex = combCodeSoft.Items.Count - 1;
            }
            catch
            {
                SajetCommon.Show_Message("Read File:" + g_sFile + " Error", 0);
            }
        }

        public string GetSysBaseDatastring(string sName, ref string sErrorMsg)
        {
            string sSQL = "";
            sSQL = " SELECT PARAM_VALUE "
                 + "   FROM SAJET.SYS_BASE "
                 + "  WHERE Upper(PROGRAM) = '" + g_sProgram.ToUpper() + "' "
                 + "    and Upper(PARAM_NAME) = '" + sName.ToUpper() + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
                return dsTemp.Tables[0].Rows[0]["PARAM_VALUE"].ToString();
            else
            {
                sErrorMsg = sErrorMsg + sName + Environment.NewLine;
                return "";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= dvGrid.Rows.Count - 1; i++)
            {
                try
                {
                    Convert.ToInt32(dvGrid.Rows[i].Cells[2].Value);
                    Convert.ToInt32(dvGrid.Rows[i].Cells[5].Value);
                }
                catch
                {
                    SajetCommon.Show_Message("Qty is not Integer", 0);
                    return;
                }
            }

            try
            {
                SajetInifile sajetInifile1 = new SajetInifile();
                sajetInifile1.WriteIniFile(g_sFile, g_sProgram, "Code Soft Version", combCodeSoft.Text.Substring(8, 1));
                for (int i = 0; i <= dvGrid.Rows.Count - 1; i++)
                {
                    string sLabel = dvGrid.Rows[i].Cells[0].Value.ToString();

                    sajetInifile1.WriteIniFile(g_sFile, g_sProgram, "Print " + sLabel, dvGrid.Rows[i].Cells[1].Value.ToString());
                    sajetInifile1.WriteIniFile(g_sFile, g_sProgram, sLabel + " Qty", dvGrid.Rows[i].Cells[2].Value.ToString());
                    sajetInifile1.WriteIniFile(g_sFile, g_sProgram, "Print " + sLabel + " Method", dvGrid.Rows[i].Cells[3].Value.ToString());
                    sajetInifile1.WriteIniFile(g_sFile, g_sProgram, sLabel + " ComPort", dvGrid.Rows[i].Cells[4].Value.ToString());
                    sajetInifile1.WriteIniFile(g_sFile, g_sProgram, sLabel + " Label Qty", dvGrid.Rows[i].Cells[5].Value.ToString());
                }
            }
            catch
            {
                SajetCommon.Show_Message("Write To File: " + g_sFile + "Error", 0);
                return;
            }
            Close_MainForm();
            SajetCommon.Show_Message("Configure OK", 3);
        }

        private void dvGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3 && e.RowIndex > -1)
            {
                string sTable = dvGrid.Rows[e.RowIndex].Cells[3].Value.ToString().ToUpper();
                DataGridViewComboBoxCell dtgCol = new DataGridViewComboBoxCell();
                dtgCol.DataSource = g_dsPort.Tables[sTable];
                dtgCol.DisplayMember = "PORT";
                dtgCol.ValueMember = "PORT";
                dvGrid.Rows[e.RowIndex].Cells[4] = dtgCol;
                dvGrid.Rows[e.RowIndex].Cells[4].Value = "";
                //if (dvGrid.Rows[e.RowIndex].Cells[3].Value.ToString() == "CodeSoft")
                //{
                //    dvGrid.Rows[e.RowIndex].Cells[4].Value = "";
                //    dvGrid.Rows[e.RowIndex].Cells[4].ReadOnly = true;                                    

                //}
                //else
                //    dvGrid.Rows[e.RowIndex].Cells[4].ReadOnly = false;
            }
        }

        private void Close_MainForm()
        {
            //將執行的主畫面關掉來重新讀取設定值
            foreach (Form frm in this.MdiParent.MdiChildren)
            {
                Type t = frm.GetType();
                if (t.Namespace != "BCConfig")
                {
                    frm.Close();//關閉form
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

