using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OracleClient;
using System.Reflection;
using System.Diagnostics;
using System.Data.OleDb;
using ClassSajetIniFile;
using ClassMail;

namespace MESInterface
{
    public enum StatusType : short
    {
        OK = 0,
        Warning = 1,  //只紀錄,不發mail      
        Error = 2,    //會發mail通知Exception
    }

    public partial class fMain : Form
    {
        string g_sConnection;        
        string sSQL;
        DataSet dsTemp;
        string g_sUserID = "0";
        public static string g_sIniFile = Application.StartupPath + "\\MESInterface.ini";
        public static string g_sIniSection = "MESInterface";
        bool g_bAutoTrans, g_bStop, g_bSendMail, g_bBreak;
        int g_iInterval;
        string g_sTransferType;      
        public static string[] g_tsItem;
        public static string[] g_tsItemDesc;        
        DateTime g_dtSysdate;
        StatusType g_StatusType;
        MailItem g_MailItems = new MailItem();
        string g_sExceptPath = Application.StartupPath + "\\ErrorLog";

        public fMain()
        {
            InitializeComponent();
        }
        
        public static DialogResult Show_Message(string sText, int iType)
        {
            switch (iType)
            {
                case 0: //Error
                    return MessageBox.Show(sText, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                case 1: //Warning
                    return MessageBox.Show(sText, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                case 2: //Confirm
                    return MessageBox.Show(sText, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                default:
                    return MessageBox.Show(sText, "", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
        }
        private string GetMaxID(string sTable, string sField, int iIDLength, ref string sMessage)
        {
            try
            {
                object[][] Params = new object[5][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TFIELD", sField };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TTABLE", sTable };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TNUM", iIDLength.ToString() };
                Params[3] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                Params[4] = new object[] { ParameterDirection.Output, OracleType.VarChar, "T_MAXID", "" };
                DataSet dsTemp = ExecuteProc("SAJET.SJ_GET_MAXID", Params);

                string sRes = dsTemp.Tables[0].Rows[0]["TRES"].ToString();
                if (sRes != "OK")
                {
                    sMessage = sRes;
                    return "0";
                }

                return dsTemp.Tables[0].Rows[0]["T_MAXID"].ToString();
            }
            catch (Exception ex)
            {
                sMessage = "SAJET.SJ_GET_MAXID," + sTable + "," + ex.Message;
                return "0";
            }
        }
        private void LogError(string sType, string sMessage)
        {
            StreamWriter sw = null;
            string sFile = g_sExceptPath + "\\" + sType + DateTime.Now.ToString("yyyyMMdd") + ".log";
            if (!Directory.Exists(g_sExceptPath))
                Directory.CreateDirectory(g_sExceptPath);

            sFile = sType + DateTime.Now.ToString("yyyyMMdd") + ".log";
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
                sw.WriteLine(string.Format("{0,-30}", DateTime.Now.ToString()) + sMessage);
            }
            finally
            {
                sw.Close();
            }
        }                 
        private string GetID(string sTable, string sFieldID, string sFieldName, string sValue)
        {
            //找欄位ID值
            if (string.IsNullOrEmpty(sValue))
                return "0";
            string sSQL = "select " + sFieldID + " from " + sTable + " "
                        + "where " + sFieldName + " = '" + sValue + "' ";
            DataSet dsTemp = ExecuteSQL(sSQL, null);
            if (dsTemp.Tables[0].Rows.Count > 0)
                return dsTemp.Tables[0].Rows[0][sFieldID].ToString();
            else
                return "0";
        }        
        private DataRow GetRowData(string sTable, string sFieldName, string sValue)
        {
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FIELD_VALUE", sValue };
            string sSQL = "select * from " + sTable + " "
                        + "where " + sFieldName + " =:FIELD_VALUE ";
            DataSet dsTemp = ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                return dsTemp.Tables[0].Rows[0];
            }
            else
                return null;
        }
        private DataRow GetRowData(string sTable, string sFieldName1, string sValue1, string sFieldName2, string sValue2)
        {
            object[][] Params = new object[2][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FIELD_VALUE_1", sValue1 };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FIELD_VALUE_2", sValue2 };
            string sSQL = "select * from " + sTable + " "
                        + "where " + sFieldName1 + " =:FIELD_VALUE_1 "
                        + "and " + sFieldName2 + " =:FIELD_VALUE_2 " ;
            DataSet dsTemp = ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                return dsTemp.Tables[0].Rows[0];
            }
            else
                return null;
        }
        private DataRow GetRowData(string sTable, string sFieldName1, string sValue1, string sFieldName2, string sValue2,string sFieldName3,string sValue3)
        {
            object[][] Params = new object[3][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FIELD_VALUE_1", sValue1 };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FIELD_VALUE_2", sValue2 };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FIELD_VALUE_3", sValue3 };

            string sSQL = "select * from " + sTable + " "
                        + "where " + sFieldName1 + " =:FIELD_VALUE_1 "
                        + "and " + sFieldName2 + " =:FIELD_VALUE_2 "
                        + "and " + sFieldName3 + " =:FIELD_VALUE_3 ";
            DataSet dsTemp = ExecuteSQL(sSQL, Params);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                return dsTemp.Tables[0].Rows[0];
            }
            else
                return null;
        }
        private DataRow GetBOMID(string sPartID, string sVer)//, string sBomType)
        {
            string sSQL = "select * from SAJET.SYS_BOM_INFO "
                        + "where PART_ID = '" + sPartID + "' ";
                       // + "and VERSION = '" + sVer + "' ";
            if (string.IsNullOrEmpty(sVer))
                sSQL = sSQL + "and VERSION ='N/A' ";
            else
                sSQL = sSQL + "and VERSION = '" + sVer + "' ";
            DataSet dsTemp = ExecuteSQL(sSQL, null);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                return dsTemp.Tables[0].Rows[0];
            }
            else
                return null;
        }        
        private DateTime GetSysDate()
        {
            sSQL = "Select Sysdate from dual";
            DataSet dsSysdate = ExecuteSQL(sSQL, null);
            DateTime dtSysdate = (DateTime)dsSysdate.Tables[0].Rows[0]["Sysdate"];
            return dtSysdate;
        }

        private DataSet ExecuteProc(string sProcName, object[][] Params)
        {
            OracleConnection OraConnection = new OracleConnection(g_sConnection);
            try
            {
                OracleCommand oraCmd = new OracleCommand("", OraConnection);
                oraCmd.CommandText = sProcName;

                oraCmd.CommandType = CommandType.StoredProcedure;
                DataSet ds = new DataSet();
                DataTable dsTable = new DataTable();
                DataRow dr;
                dr = dsTable.NewRow();
                OracleDataAdapter adapter = null;
                for (int i = 0; i <= Params.Length - 1; i++)
                {
                    OracleParameter oraclePar = new OracleParameter();
                    try
                    {
                        oraclePar.Direction = (ParameterDirection)Params[i][0];
                    }
                    catch
                    {
                        switch (Params[i][0].ToString().ToUpper())
                        {
                            case "INPUT": oraclePar.Direction = ParameterDirection.Input; break;
                            case "OUTPUT": oraclePar.Direction = ParameterDirection.Output; break;
                            default: oraclePar.Direction = ParameterDirection.Input; break;
                        }
                    }

                    try
                    {
                        oraclePar.OracleType = (OracleType)Params[i][1];
                        //oraclePar.DbType = (DbType)Params[i][1];
                    }
                    catch
                    {
                        switch (Params[i][1].ToString())
                        {
                            case "1": oraclePar.OracleType = OracleType.VarChar; break;
                            case "2": oraclePar.OracleType = OracleType.Int32; break;
                            case "3": oraclePar.OracleType = OracleType.DateTime; break;
                            default: oraclePar.OracleType = OracleType.VarChar; break;

                            //case "1": oraclePar.DbType = DbType.String; break;
                            //case "2": oraclePar.DbType = DbType.Int32; break;
                            //case "3": oraclePar.DbType = DbType.DateTime; break;
                            //default: oraclePar.DbType = DbType.String; break;
                        }
                    }
                    oraclePar.ParameterName = Params[i][2].ToString().ToUpper();
                    oraclePar.Size = Params[i][3].ToString().Length;
                    if (oraclePar.Direction == ParameterDirection.Output)
                    {
                        oraclePar.Size = 200;
                    }
                    oraclePar.Value = Params[i][3].ToString();
                    oraCmd.Parameters.Add(oraclePar);
                }

                adapter = new OracleDataAdapter(oraCmd);
                try
                {
                    adapter.Fill(ds);
                    for (int i = 0; i <= adapter.SelectCommand.Parameters.Count - 1; i++)
                    {
                        if (adapter.SelectCommand.Parameters[i].Direction == ParameterDirection.Output)
                        {
                            dsTable.Columns.Add(adapter.SelectCommand.Parameters[i].ParameterName, typeof(string));
                            dr[adapter.SelectCommand.Parameters[i].ParameterName] = adapter.SelectCommand.Parameters[i].Value;
                        }

                    }
                    dsTable.Rows.Add(dr);
                    ds.Tables.Add(dsTable);
                    return ds;
                }
                finally
                {
                    adapter.Dispose();
                }
            }
            finally
            {
                OraConnection.Close();
            }
        }
        public DataSet ExecuteSQL(string sSQL, object[][] Params)
        {            
            try
            {
                DataSet ds = new DataSet();
                OracleDataAdapter adapter = new OracleDataAdapter(sSQL, g_sConnection);
                if (Params != null)
                {
                    for (int i = 0; i <= Params.Length - 1; i++)
                    {
                        OracleParameter oraclePar = new OracleParameter();
                        oraclePar.Direction = ParameterDirection.Input;
                        try
                        {
                            //oraclePar.DbType = (DbType)Params[i][1];
                            oraclePar.OracleType = (OracleType)Params[i][1];
                        }
                        catch
                        {
                            switch (Params[i][1].ToString())
                            {
                                case "1": oraclePar.OracleType = OracleType.VarChar; break;
                                case "2": oraclePar.OracleType = OracleType.Int32; break;
                                case "3": oraclePar.OracleType = OracleType.DateTime; break;
                                default: oraclePar.OracleType = OracleType.VarChar; break;

                                //case "1": oraclePar.DbType = DbType.String; break;
                                //case "2": oraclePar.DbType = DbType.Int32; break;
                                //case "3": oraclePar.DbType = DbType.DateTime; break;
                                //default: oraclePar.DbType = DbType.String; break;
                            }
                        }
                        oraclePar.ParameterName = Params[i][2].ToString().ToUpper();
                        //oraclePar.Size = Params[i][3].ToString().Length;
                        oraclePar.Value = Params[i][3];//.ToString();
                        adapter.SelectCommand.Parameters.Add(oraclePar);
                    }
                }
                try
                {
                    adapter.Fill(ds);
                    return ds;
                }
                finally
                {
                    adapter.Dispose();
                }
            }
            finally
            {
               
            }
        }
        public bool ReadDBCfg(ref string sErrMsg)
        {
            if (File.Exists("DB.cfg"))
            {
                StreamReader reader = new StreamReader("DB.cfg");
                try
                {
                    string sDBName = string.Empty;
                    string sUserName = "sj";
                    string sPassword = "sj";
                    while (true)
                    {
                        string sContent = reader.ReadLine();
                        if (string.IsNullOrEmpty(sContent))
                            break;
                        int iIndex = sContent.IndexOf("=");
                        string sLabel = string.Empty;
                        if (iIndex >= 0)
                        {
                            sLabel = sContent.Substring(0, iIndex - 1).Trim();
                        }
                        iIndex += 1;
                        if (sLabel == "DB")
                        {
                            sDBName = sContent.Substring(iIndex, sContent.Length - iIndex).Trim();
                        }
                        if (sLabel == "USER NAME")
                        {
                            sUserName = sContent.Substring(iIndex, sContent.Length - iIndex).Trim();
                        }
                        if (sLabel == "PASSWORD")
                        {
                            sPassword = sContent.Substring(iIndex, sContent.Length - iIndex).Trim();
                        }
                    }
                    g_sConnection = "Data Source=" + sDBName + ";Persist Security Info=True;User ID =" + sUserName + ";Password = " + sPassword + " ;Unicode = True";
                    txtDBName.Text = sDBName;
                    txtDBUser.Text = sUserName;                    
                }
                finally
                {
                    reader.Close();
                }
            }
            else
            {
                sErrMsg = "Cann't found DB.cfg";
                return false;
            }           
            return true;
        }
        public bool CheckDBConnection(ref string sErrMsg)
        {            
            if (!ReadDBCfg(ref sErrMsg))
                return false;

            try
            {
                OracleConnection connection = new OracleConnection(g_sConnection);
                try
                {
                    connection.Open();
                }
                finally
                {
                    connection.Close();
                }
            }
            catch (Exception exp)
            {
                sErrMsg = "Connection String Error;" + exp.Message.ToString();                
                return false;
            }
            return true;
        }
        //private bool CheckDirectory(ref string sErrMsg)
        //{
            //if (!Directory.Exists(g_sDirectory))
            //{
            //    sErrMsg = "Input Directory Error ; " + g_sDirectory;               
            //    return false;
            //}
            //if (!Directory.Exists(g_sBackupPath))
            //{
            //    sErrMsg = "Backup Directory Error ; " + g_sBackupPath;
            //    return false;
            //}
            //if (!Directory.Exists(g_sExceptPath))
            //{
            //    sErrMsg = "Log Directory Error ; " + g_sExceptPath;               
            //    return false;
            //}
            //return true;   
        //}

        private void StartTransfer(string sTransTable)
        {
            tsLabelStart.Text = DateTime.Now.ToString("HH:mm:ss");
            tsLabelEnd.Text = "";
            LabMsg.Text = "";
            LabMsg.Visible = false;
            timer1.Enabled = false;
            g_bStop = false;           
            
            LabPartCNT.Text = "0";
            LabBOMHeaderCNT.Text = "0";
            LabBOMDetailCNT.Text = "0";
            LabBOMLocCNT.Text = "0";
            LabBOMAlterCNT.Text = "0";
            LabWOCNT.Text = "0";
            LabWoBomCNT.Text = "0";
            LabCustomerCNT.Text = "0";
            LabVendorCNT.Text = "0";
            LabRTCNT.Text = "0";
            LabRTDetailCNT.Text = "0";
            LabPOCNT.Text = "0";
            lablMPNCNT.Text = "0";
            lablINvoiceQty.Text = "0";
            labECNCNT.Text = "0";
            lablCustItemCNT.Text = "0";
            lablWIPCNT.Text = "0";

            try
            {                             
                //清除所有Listview內容
                for (int j = 0; j <= tabControl1.Controls.Count - 1; j++)
                {
                    for (int i = 0; i <= tabControl1.Controls[j].Controls.Count - 1; i++)
                    {
                        Control obComponent = tabControl1.Controls[j].Controls[i];
                        if (obComponent is ListView)
                        {
                            ((ListView)obComponent).Items.Clear();
                        }
                    }
                }
                TransferFile(sTransTable);
            }
            finally
            {
                timer1.Enabled = g_bAutoTrans;
                tsLabelEnd.Text = DateTime.Now.ToString("HH:mm:ss");
            }
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            string sMsg = "";
            LabMsg.Visible = false;
            LabMsg.Text = "";
            if (!CheckDBConnection(ref sMsg))
            {
                //LogError("System", sMsg);                    
                LabMsg.Text = sMsg;
                LabMsg.Visible = true;
                return;
            }  
            
            if (g_bBreak)
            {
                //若有尚未處理的Excption,不可繼續執行
                if (!Check_ExpFinish())
                    return;
            }

            g_sTransferType = "H";
            StartTransfer("MESUSER.ERP_INTERFACE");
            if (LVException.Items.Count > 0)
            {
                //發Mail
                if (g_bSendMail)
                {
                    string sBody = "";
                    sBody = "<HTML><BODY>"
                       + "<TABLE  cellspacing=0 border=1 bordercolor=#ffffff WIDTH=70%>"
                        + "<TR>"
                      + "<TD  align=center bgcolor =#EFC56B><FONT size=3>開始時間</FONT></TD>"
                      + "<TD  align=center bgcolor =#FFFFFF><FONT size=3>" + tsLabelStart.Text + "</FONT></TD>"
                      + "<TD  align=center bgcolor =#EFC56B><FONT size=3>結束時間</FONT></TD>"
                      + "<TD  align=center bgcolor =#FFFFFF><FONT size=3>" + tsLabelEnd.Text + "</FONT></TD>"
                      + "<TD  align=center bgcolor =#EFC56B><FONT size=3>異常筆數</FONT></TD>"
                      + "<TD  align=center bgcolor =#FFFFFF><FONT size=3>" + LVException.Items.Count.ToString() + "</FONT></TD>"
                      + "</TR></TABLE>"
                      +"<BR>"
                      + "<TABLE  cellspacing=0 border=0 bordercolor=#ffffff WIDTH=70%>"
                      + "<TR>"
                      + "<TD  align=center bgcolor =#9dc1d7><FONT size=3>No</FONT></TD>"
                      + "<TD  align=center bgcolor =#9dc1d7><FONT size=3>Table</FONT></TD>"
                      + "<TD  align=center bgcolor =#9dc1d7><FONT size=3>Data Index</FONT></TD>"
                      + "<TD  align=center bgcolor =#9dc1d7><FONT size=3>Error Message</FONT></TD>"
                      + "</TR>";
                    for (int i = 0; i <= LVException.Items.Count - 1; i++)
                    {
                        string sTable = LVException.Items[i].SubItems[0].Text;
                        string sDataIndex = LVException.Items[i].SubItems[1].Text;
                        string sData = LVException.Items[i].SubItems[2].Text;
                        int iIndex = i + 1;
                        int iRem=0;
                        Math.DivRem(iIndex,2,out iRem);
                        string sColor="#FFFFFF";
                        if (iRem == 0)
                            sColor = "#FFFFFF";
                        sBody = sBody + "<TR>"
                        + "<TD  align=center bgColor=" + sColor + "><FONT size=3>" + Convert.ToString(i + 1) + "</FONT></TD>"
                        + "<TD  align=center bgColor=" + sColor + "><FONT size=3>" + sTable + "</FONT></TD>"
                        + "<TD  align=center bgColor=" + sColor + "><FONT size=3>" + sDataIndex + "</FONT></TD>"
                        + "<TD  align=center bgColor=" + sColor + "><FONT size=3>" + sData + "</FONT></TD>"
                        + "</TR>";
                        //sBody = sBody + "Table       : [" + sTable + "]" + Environment.NewLine
                        //              + "Data Index  : [" + sDataIndex + "]" + Environment.NewLine
                        //              + "Error Message: [" + sData + "]" + Environment.NewLine;

                        //sBody = sBody + "==========";
                         
                    }
                    sBody = sBody + "</TABLE></BODY></HTML>";
                    SendMail(sBody);
                }
            }
        }
        private void btnExcept_Click(object sender, EventArgs e)
        {
            string sMsg = "";
            if (!CheckDBConnection(ref sMsg))
            {
                //LogError("System", sMsg);                    
                LabMsg.Text = sMsg;
                LabMsg.Visible = true;
                return;
            } 

            g_sTransferType = "E";
            StartTransfer("MESUSER.ERP_INTERFACE_E");
        } 

        private void TransferFile(string sTransTable)
        {            
            sSQL = " Select * from " + sTransTable
                 //+ " WHERE UPPER(TABLE_NAME)='ITEM_MASTER' "
                 //+ " AND ROWNUM <=50000 "
                 + " Order By DATA_IDX ";
            DataSet dsTransData = ExecuteSQL(sSQL, null);
            for (int i = 0; i <= dsTransData.Tables[0].Rows.Count - 1; i++)
            {                
                Application.DoEvents();
                if (g_bStop)
                    return;
                string sTransType = dsTransData.Tables[0].Rows[i]["TABLE_NAME"].ToString().ToUpper();                
                ListViewItem[] LVFind = LVItem.Items.Find(sTransType, false);
                if (LVFind.Length == 0)
                    continue;
                if (!LVItem.Items[LVFind[0].Index].Checked)
                    continue;
                           
                string sIndex = dsTransData.Tables[0].Rows[i]["DATA_IDX"].ToString();
                string sTableName = "MESUSER." + sTransType;
                if (g_sTransferType == "E")
                    sTableName = sTableName + "_E";                

                switch (sTransType)
                {
                    case "ITEM_MASTER": 
                        tabControl1.SelectedTab = tabPagePart;                        
                        TransferPart(sIndex, sTableName); 
                        break;
                    case "CUSTOMER_MASTER": 
                        tabControl1.SelectedTab = tabPageCustomer;                      
                        TransferCustomer(sIndex, sTableName); 
                        break;
                    case "CUSTOMER_ITEM_INFO":
                        tabControl1.SelectedTab = tabPageCustomerItem;
                        TransferCustomerItem(sIndex, sTableName);
                        break;

                    case "VENDOR_MASTER": 
                        tabControl1.SelectedTab = tabPageVendor;                       
                        TransferVendor(sIndex, sTableName); 
                        break;
                    case "JOB_HEADER": 
                        tabControl1.SelectedTab = tabPageWo;                       
                        TransferWoHeader(sIndex, sTableName); 
                        break;
                    case "JOB_DETAIL": 
                        tabControl1.SelectedTab = tabPageWoBOM;                       
                        TransferWoDetail(sIndex, sTableName); 
                        break;
                    case "RT_HEADER": 
                        tabControl1.SelectedTab = tabPageRT;                       
                        TransferRTHeader(sIndex, sTableName); 
                        break;
                    case "RT_DETAIL": 
                        tabControl1.SelectedTab = tabPageRTDetail;                        
                        TransferRTDetail(sIndex, sTableName); 
                        break;                    
                    case "BOM_DETAIL": 
                        tabControl1.SelectedTab = tabPageBOMDetail;                        
                        TransferBOMDetail(sIndex, sTableName); 
                        break;
                    case "BOM_LOCATION": 
                        tabControl1.SelectedTab = tabPageBOMLoc;                        
                        TransferBOMLocation(sIndex, sTableName); 
                        break;
                    case "BOM_ALTERNATIVE": 
                        tabControl1.SelectedTab = tabPageBOMAlter;                       
                        TransferBOMAlternative(sIndex, sTableName); 
                        break;
                    case "PO_MASTER": 
                        tabControl1.SelectedTab = tabPagePO;                       
                        TransferPo(sIndex, sTableName); 
                        break;
                    case "SHIPPING_HEADER": 
                        tabControl1.SelectedTab = tabPageShipHeader;                       
                        TransferShipHeader(sIndex, sTableName); 
                        break;
                    case "SHIPPING_DETAIL": 
                        tabControl1.SelectedTab = tabPageShipDetail;                        
                        TransferShipDetail(sIndex, sTableName); 
                        break;                  
                    case "ECN_WO":
                        tabControl1.SelectedTab = tabPageECN;
                        TransferECNWO(sIndex, sTableName);
                        break;
                    case "TIME_RANGE":
                        tabControl1.SelectedTab = tabPageWIP;
                        TransferTimeRange(sIndex, sTableName);
                        break;
                    case "JOB_TRANSFER":
                        tabControl1.SelectedTab = tabPageWoTransfer;
                        TransferWoTransfer(sIndex, sTableName);
                        break;

                    default: break;
                }

                //一有錯誤就中斷此次作業
                if (g_bBreak)
                {
                    if (g_StatusType != StatusType.OK)
                        return;
                }
            }            
        }
        private void TransferPart(string sDataIndex, string sTable)
        {
            string sMessage = "";
            string sModifyType = "";
            g_StatusType = StatusType.OK;
            //ERP上==============================            
            string sItemNo = "";        //料號
            string sModelName = "";     //機種
            string sItemSpec = "";      //規格
            string sItemDesc1 = "";     //商品描述
            string sVersion = "";       //BOM版本號碼
            string sCategory = "";      //研發分類
            string sItemPrefix = "";    //料號特徵字用於定義編碼規則
            //string sMSLLevel = "";    //MSL等級
            string sEnabled = "Y";

            string sMaterialType = "";                  //物料類型
            decimal dOption1 = 1;                       //R/C單位用量
            decimal dOption3 = 1;                       //R/C每卷用量
            string sOption2 = "";                       //R/C範本
            string sOption4 = "";                       //R/C大分類
            string sOption5 = "";                       //R/C中分類
            string sOption7 = "";                       //R/C小分類
            string sOption8 = "";                       //R/C分群碼
            string sOption9 = "";                       //R/C公差&感值
            string sOption10 = "";                      //組裝料是否檢查(Y/N)
            string sOption11 = "";                      //產品等級
            string sOption12 = "";                      //新品名
            string sRouteName = "", sRouteID = "0";     //途程名稱
            string sRuleSet = "";                       //規則集合

            DateTime dtRDate = DateTime.Now;
            DateTime dtIDate = DateTime.Now;
            //==================================            
            string sMESID = "0";
            bool bUpdate = false;
            bool bInsert = false;
            g_dtSysdate = GetSysDate();
            string sMSDID = "0";

            try
            {
                //由Temp Table讀取資料
                string sCommand = " Select * from " + sTable + " "
                                + " Where DATA_IDX = '" + sDataIndex + "'";
                DataSet dsData = ExecuteSQL(sCommand, null);
                if (dsData.Tables[0].Rows.Count == 0)
                {
                    sMessage = "No Data - Data Index:" + sDataIndex;
                    g_StatusType = StatusType.Error;
                    return;
                }
                sModifyType = dsData.Tables[0].Rows[0]["ERP_FLAG"].ToString();
                sItemNo = dsData.Tables[0].Rows[0]["ITEM_NO"].ToString().Trim();
                sModelName = dsData.Tables[0].Rows[0]["MODEL_NAME"].ToString().Trim();
                sCategory = dsData.Tables[0].Rows[0]["ITEM_TYPE"].ToString().Trim();
                sItemSpec = dsData.Tables[0].Rows[0]["ITEM_SPEC"].ToString().Trim();
                sItemDesc1 = dsData.Tables[0].Rows[0]["ITEM_DESC"].ToString().Trim();
                sVersion = "N/A";

                sMaterialType = dsData.Tables[0].Rows[0]["MATERIAL_TYPE"].ToString().Trim();
                dOption1 = Convert.ToDecimal(dsData.Tables[0].Rows[0]["OPTION1"].ToString().Trim());
                dOption3 = Convert.ToDecimal(dsData.Tables[0].Rows[0]["OPTION3"].ToString().Trim());
                sOption2 = dsData.Tables[0].Rows[0]["OPTION2"].ToString().Trim();
                sOption4 = dsData.Tables[0].Rows[0]["OPTION4"].ToString().Trim();
                sOption5 = dsData.Tables[0].Rows[0]["OPTION5"].ToString().Trim();
                sOption7 = dsData.Tables[0].Rows[0]["OPTION7"].ToString().Trim();
                sOption8 = dsData.Tables[0].Rows[0]["OPTION8"].ToString().Trim();
                sOption9 = dsData.Tables[0].Rows[0]["OPTION9"].ToString().Trim();
                sOption10 = dsData.Tables[0].Rows[0]["OPTION10"].ToString().Trim();
                sOption11 = dsData.Tables[0].Rows[0]["OPTION11"].ToString().Trim();
                sOption12 = dsData.Tables[0].Rows[0]["OPTION12"].ToString().Trim();
                sRouteName = dsData.Tables[0].Rows[0]["ROUTE_NAME"].ToString();
                sRuleSet = dsData.Tables[0].Rows[0]["RULE_SET"].ToString().Trim();

                try
                {
                    DataRow drNo = GetRowData("SAJET.SYS_PART", "PART_NO", sItemNo);  //以No查詢 
                    string sModelID = "0";

                    //if (!string.IsNullOrEmpty(sModelName) && sModelName != "N/A")
                    //{
                    //    DataRow drModel = GetRowData("SAJET.SYS_MODEL", "MODEL_NAME", sModelName);
                    //    if (drModel == null)
                    //    {
                    //        sModelID = GetMaxID("SAJET.SYS_MODEL", "MODEL_ID", 8, ref sMessage);
                    //        if (sModelID == "0")
                    //        {
                    //            sMessage = "Get Model MaxID Error";
                    //            g_StatusType = StatusType.Error;
                    //            return;
                    //        }
                    //        object[][] Params = new object[5][];
                    //        sSQL = " INSERT INTO SAJET.SYS_MODEL "
                    //             + " (MODEL_ID, MODEL_NAME ,UPDATE_USERID,UPDATE_TIME,MODEL_DESC1 ) "
                    //             + " VALUES "
                    //             + " (:MODEL_ID, :MODEL_NAME,:UPDATE_USERID, :UPDATE_TIME,:MODEL_DESC1 ) ";
                    //        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MODEL_ID", sModelID };
                    //        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MODEL_NAME", sModelName };
                    //        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                    //        Params[3] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                    //        Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MODEL_DESC1", sModelName };
                    //        dsTemp = ExecuteSQL(sSQL, Params);
                    //    }
                    //    else
                    //        sModelID = drModel["MODEL_ID"].ToString();
                    //}


                    //Route
                    DataRow drRoute = GetRowData("SAJET.SYS_RC_ROUTE", "ROUTE_NAME", sRouteName);
                    if (drRoute == null)
                    {
                        sMessage = "Route not Exist - Route Name:" + sRouteName;
                        g_StatusType = StatusType.Error;
                        return;
                    }
                    sRouteID = drRoute["ROUTE_ID"].ToString();

                    //Insert====================
                    if (sModifyType == "C")
                    {
                        //MES中是否已有此ERPID 
                        if (drNo != null) //資料已存在
                        {
                            sMESID = drNo["PART_ID"].ToString();
                            if (drNo["ENABLED"].ToString() == "Y")
                            {
                                sMessage = "Data Exist";
                                g_StatusType = StatusType.Warning;
                                return;
                            }
                            bUpdate = true;
                        }
                        else
                        {
                            bInsert = true;
                        }
                        //}
                    }
                    //Update=============================
                    else if (sModifyType == "U")
                    {
                        if (drNo == null)
                        {
                            bInsert = true;
                        }
                        else
                        {
                            sMESID = drNo["PART_ID"].ToString();
                            bUpdate = true;
                        }
                    }

                    if (bInsert)
                    {
                        sMESID = GetMaxID("SAJET.SYS_PART", "PART_ID", 10, ref sMessage);
                        if (sMESID == "0")
                        {
                            sMessage = "Get Part MaxID Error";
                            g_StatusType = StatusType.Error;
                            return;
                        }
                        object[][] Params = new object[23][];
                        sSQL = " INSERT INTO SAJET.SYS_PART "
                             + " (PART_ID, PART_NO, PART_TYPE, SPEC1,SPEC2, UPDATE_USERID,UPDATE_TIME "
                             + " ,VERSION, MODEL_ID, MATERIAL_TYPE, ROUTE_ID, RULE_SET "
                             + " ,OPTION1, OPTION2, OPTION3, OPTION4, OPTION5, OPTION7, OPTION8, OPTION9, OPTION10, OPTION11, OPTION12 "
                             + " ) "
                             + " VALUES "
                             + " (:PART_ID, :PART_NO, :PART_TYPE, :SPEC1,:SPEC2 , :UPDATE_USERID, :UPDATE_TIME"
                             + " ,:VERSION, :MODEL_ID, :MATERIAL_TYPE, :ROUTE_ID, :RULE_SET "
                             + " ,:OPTION1, :OPTION2, :OPTION3, :OPTION4, :OPTION5, :OPTION7, :OPTION8, :OPTION9, :OPTION10, :OPTION11, :OPTION12 "
                             + " ) ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sMESID };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_NO", sItemNo };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_TYPE", sCategory };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SPEC1", sItemSpec };
                        Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SPEC2", sItemDesc1 };
                        Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[6] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VERSION", sVersion };
                        Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MODEL_ID", sModelID };
                        Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MATERIAL_TYPE", sMaterialType };
                        Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ROUTE_ID", sRouteID };
                        Params[11] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RULE_SET", sRuleSet };
                        Params[12] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION1", dOption1 };
                        Params[13] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION3", dOption3 };
                        Params[14] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION2", sOption2 };
                        Params[15] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION4", sOption4 };
                        Params[16] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION5", sOption5 };
                        Params[17] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION7", sOption7 };
                        Params[18] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION8", sOption8 };
                        Params[19] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION9", sOption9 };
                        Params[20] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION10", sOption10 };
                        Params[21] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION11", sOption11 };
                        Params[22] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION12", sOption12 };
                        dsTemp = ExecuteSQL(sSQL, Params);
                        sMessage = "OK(INSERT)";
                    }
                    else if (bUpdate)
                    {
                        object[][] Params = new object[24][];
                        sSQL = " UPDATE SAJET.SYS_PART "
                             + "    SET  PART_NO = :PART_NO "
                             + "        ,PART_TYPE = :PART_TYPE "
                             + "        ,SPEC1 = :SPEC1 "
                             + "        ,SPEC2 = :SPEC2 "
                             + "        ,UPDATE_USERID = :UPDATE_USERID "
                             + "        ,UPDATE_TIME = :UPDATE_TIME  "
                             + "        ,VERSION = :VERSION  "
                             + "        ,MODEL_ID = :MODEL_ID  "
                             + "        ,ENABLED =:ENABLED "
                             + "        ,MATERIAL_TYPE =:MATERIAL_TYPE "
                             + "        ,ROUTE_ID =:ROUTE_ID "
                             + "        ,RULE_SET =:RULE_SET "
                             + "        ,OPTION1 =:OPTION1 "
                             + "        ,OPTION3 =:OPTION3 "
                             + "        ,OPTION2 =:OPTION2 "
                             + "        ,OPTION4 =:OPTION4 "
                             + "        ,OPTION5 =:OPTION5 "
                             + "        ,OPTION7 =:OPTION7 "
                             + "        ,OPTION8 =:OPTION8 "
                             + "        ,OPTION9 =:OPTION9 "
                             + "        ,OPTION10 =:OPTION10 "
                             + "        ,OPTION11 =:OPTION11 "
                             + "        ,OPTION12 =:OPTION12 "
                             + "  WHERE PART_ID = :PART_ID ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_NO", sItemNo };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_TYPE", sCategory };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SPEC1", sItemSpec };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SPEC2", sItemDesc1 };
                        Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[5] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VERSION", sVersion };
                        Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MODEL_ID", sModelID };
                        Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ENABLED", sEnabled };
                        Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sMESID };
                        Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MATERIAL_TYPE", sMaterialType };
                        Params[11] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ROUTE_ID", sRouteID };
                        Params[12] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RULE_SET", sRuleSet };
                        Params[13] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION1", dOption1 };
                        Params[14] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION3", dOption3 };
                        Params[15] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION2", sOption2 };
                        Params[16] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION4", sOption4 };
                        Params[17] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION5", sOption5 };
                        Params[18] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION7", sOption7 };
                        Params[19] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION8", sOption8 };
                        Params[20] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION9", sOption9 };
                        Params[21] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION10", sOption10 };
                        Params[22] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION11", sOption11 };
                        Params[23] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION12", sOption12 };
                        dsTemp = ExecuteSQL(sSQL, Params);
                        sMessage = "OK(UPDATE)";
                    }

                    if (bInsert || bUpdate)
                    {
                        //SYS_PART_ROUTE
                        sSQL = " DELETE SAJET.SYS_PART_ROUTE "
                             + "  WHERE PART_ID = '" + sMESID + "' ";
                        dsTemp = ExecuteSQL(sSQL, null);

                        object[][] Params2 = new object[3][];
                        sSQL = " INSERT INTO SAJET.SYS_PART_ROUTE (PART_ID,ROUTE_ID,UPDATE_USERID,UPDATE_TIME,ENABLED) "
                             + " VALUES (:PART_ID,:ROUTE_ID,0,:UPDATE_TIME,'Y') ";
                        Params2[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sMESID };
                        Params2[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ROUTE_ID", sRouteID };
                        Params2[2] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        dsTemp = ExecuteSQL(sSQL, Params2);
                    }

                    //Delete============================
                    if (sModifyType == "D")
                    {
                        if (drNo == null)
                        {
                            sMessage = "Data not Exist";
                            g_StatusType = StatusType.Warning;
                            return;
                        }
                        sEnabled = "N";
                        sMESID = drNo["PART_ID"].ToString();
                        object[][] Params = new object[4][];
                        sSQL = " UPDATE SAJET.SYS_PART "
                             + "    SET ENABLED = :ENABLED "
                             + "       ,UPDATE_USERID = :UPDATE_USERID "
                             + "       ,UPDATE_TIME = :UPDATE_TIME "
                             + "  WHERE PART_ID = :PART_ID ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ENABLED", sEnabled };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sMESID };
                        dsTemp = ExecuteSQL(sSQL, Params);
                        sMessage = "OK(DELETE)";
                    }
                }
                catch (Exception exp)
                {
                    sMessage = exp.Message;
                    g_StatusType = StatusType.Error;
                }
            }
            finally
            {
                LVPart.Items.Add(sItemNo);
                LVPart.Items[LVPart.Items.Count - 1].SubItems.Add(sItemDesc1);
                LVPart.Items[LVPart.Items.Count - 1].SubItems.Add(sMessage);
                LVPart.Items[LVPart.Items.Count - 1].SubItems.Add(sDataIndex);
                if (sMessage.Length >= 2 && sMessage.Substring(0, 2) != "OK")
                {
                    sMessage = "Item No[" + sItemNo + "]" + sMessage;
                }
                TransferFinish(sDataIndex, sMessage, g_StatusType, sTable);
                if (g_StatusType != StatusType.OK)
                {
                    GetColor(LVPart, g_StatusType);
                }
                LabPartCNT.Text = LVPart.Items.Count.ToString();
            }
        }
        private void TransferCustomer(string sDataIndex, string sTable)
        {
            string sMessage = "";
            string sModifyType = "";
            g_StatusType = StatusType.OK;      
            //ERP上==============================   
            string sOrgan = "";      //Oracle公司別  
            string sCustID = "";   //客戶ID
            string sCustName = ""; //客戶名稱
            string sCustArea = ""; //客戶地區  
            string sCustNation = ""; //客戶國家  
            string sCustEmail =""; //客戶Email
            string sCustNameAbbr = "";//客戶簡寫
            string sCustContact = "";
          //  string sCustTel = "";
           // string sCustFax = "";
            //string sCustBillTo = "";//帳單地址
           // string sCustShipTo = "";//出貨地址
            //==================================            
            string sMESID = "0";
            bool bUpdate = false;
            bool bInsert = false;
            string sEnabled = "Y";
            g_dtSysdate = GetSysDate();

            try
            {
                //由Temp Table讀取資料
                string sCommand = " Select * from " + sTable + " "
                                + " Where DATA_IDX = '" + sDataIndex + "'";
                DataSet dsData = ExecuteSQL(sCommand, null);
                if (dsData.Tables[0].Rows.Count == 0)
                {
                    sMessage = "No Data - Data Index:" + sDataIndex;
                    g_StatusType = StatusType.Error;
                    return;
                }                
                sModifyType = dsData.Tables[0].Rows[0]["ERP_FLAG"].ToString();
                sOrgan = dsData.Tables[0].Rows[0]["ORGANIZATION"].ToString();
                sCustID = dsData.Tables[0].Rows[0]["CUSTOMER_ID"].ToString().Trim();        
                sCustName = dsData.Tables[0].Rows[0]["CUSTOMER_NAME"].ToString().Trim();
                
                sCustArea = dsData.Tables[0].Rows[0]["CUSTOMER_AREA"].ToString().Trim();
                sCustNation = dsData.Tables[0].Rows[0]["CUSTOMER_NATION"].ToString().Trim();
                sCustEmail = dsData.Tables[0].Rows[0]["CUSTOMER_EMAIL"].ToString().Trim();
                sCustNameAbbr = dsData.Tables[0].Rows[0]["CUSTOMER_NAME_ABBR"].ToString().Trim();
                try
                {
                    DataRow drID = GetRowData("SAJET.SYS_CUSTOMER", "CUSTOMER_CODE", sCustID); //以CUSTOMER_CODE查詢 
 
                    
                    //Insert====================
                    if (sModifyType == "C")
                    {
                        //MES中是否已有此CUSTOMER_CODE
                        if (drID != null) //資料已存在
                        {
                            if (drID["ENABLED"].ToString() == "Y")
                            {
                                sMessage = "Data Exist";
                                g_StatusType = StatusType.Warning;
                                return;
                            }
                            sMESID = drID["CUSTOMER_ID"].ToString();
                            bUpdate = true;
                        }
                        else
                        {
                            bInsert = true;
                        }
                    }
                    //Update=============================
                    else if (sModifyType == "U")
                    {
                        if (drID == null)
                        {
                            /*
                            sMessage = "Data not Exist";
                            g_StatusType = StatusType.Error;
                            return;
                             */
                            bInsert = true;
                        }
                        else
                        {
                            sMESID = drID["CUSTOMER_ID"].ToString();
                            bUpdate = true;
                        }
                    }
                    if (bInsert)
                    {
                        sMESID = GetMaxID("SAJET.SYS_CUSTOMER", "CUSTOMER_ID", 8, ref sMessage);
                        if (sMESID == "0")
                        {
                            sMessage = "Get MaxID Error";
                            g_StatusType = StatusType.Error;
                            return;
                        }
                        object[][] Params = new object[9][];
                        sSQL = " INSERT INTO SAJET.SYS_CUSTOMER "
                             + " (CUSTOMER_ID, CUSTOMER_CODE, CUSTOMER_NAME, UPDATE_USERID,UPDATE_TIME, CUSTOMER_AREA, CUSTOMER_NATION,CUSTOMER_EMAIL  "
                             + "  ,CUSTOMER_NAME_ABBR "
                             + "  ) "
                             + " VALUES "
                             + " (:CUSTOMER_ID, :CUSTOMER_CODE, :CUSTOMER_NAME, :UPDATE_USERID, :UPDATE_TIME, :CUSTOMER_AREA, :CUSTOMER_NATION,:CUSTOMER_EMAIL "
                             + "  ,:CUSTOMER_NAME_ABBR "
                             + "  ) ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_ID", sMESID };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_CODE", sCustID };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_NAME", sCustName };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[4] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_AREA", sCustArea };
                        Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_NATION", sCustNation };
                        Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_EMAIL", sCustEmail };
                        Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_NAME_ABBR", sCustNameAbbr };
                        dsTemp = ExecuteSQL(sSQL, Params);
                        sMessage = "OK(INSERT)";
                    }
                    else if (bUpdate)
                    {
                        object[][] Params = new object[10][];
                        sSQL = " UPDATE SAJET.SYS_CUSTOMER "
                             + "    SET  CUSTOMER_CODE = :CUSTOMER_CODE "
                             + "        ,CUSTOMER_NAME = :CUSTOMER_NAME "
                             + "        ,UPDATE_USERID = :UPDATE_USERID "
                             + "        ,UPDATE_TIME = :UPDATE_TIME  "
                             + "        ,CUSTOMER_AREA = :CUSTOMER_AREA "
                             + "        ,CUSTOMER_NATION = :CUSTOMER_NATION "
                             + "        ,CUSTOMER_EMAIL = :CUSTOMER_EMAIL "
                             + "        ,CUSTOMER_NAME_ABBR =:CUSTOMER_NAME_ABBR "
                             + "        ,ENABLED=:ENABLED "
                             + "  WHERE CUSTOMER_ID = :CUSTOMER_ID ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_CODE", sCustID };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_NAME", sCustName };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_AREA", sCustArea };
                        Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_NATION", sCustNation };
                        Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_EMAIL", sCustEmail };
                        Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_NAME_ABBR", sCustNameAbbr };
                        Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ENABLED", sEnabled };
                        Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_ID", sMESID };
                        dsTemp = ExecuteSQL(sSQL, Params);
                        sMessage = "OK(UPDATE)";
                    }

                    //Delete============================
                    if (sModifyType == "D")
                    {
                        if (drID == null)
                        {
                            sMessage = "Data not Exist";
                            g_StatusType = StatusType.Warning;
                            return;
                        }
                        sMESID = drID["CUSTOMER_ID"].ToString();
                        object[][] Params = new object[3][];
                        sSQL = " UPDATE SAJET.SYS_CUSTOMER "
                             + "    SET ENABLED = 'N' "
                             + "       ,UPDATE_USERID = :UPDATE_USERID "
                             + "       ,UPDATE_TIME = :UPDATE_TIME "
                             + "  WHERE CUSTOMER_ID = :CUSTOMER_ID ";                      
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_ID", sMESID };
                        dsTemp = ExecuteSQL(sSQL, Params);
                        sMessage = "OK(DELETE)";
                    }                    
                }
                catch (Exception exp)
                {
                    sMessage = exp.Message;
                    g_StatusType = StatusType.Error;
                }
            }
            finally
            {
                LVCust.Items.Add(sCustID);               
                LVCust.Items[LVCust.Items.Count - 1].SubItems.Add(sCustName);
                LVCust.Items[LVCust.Items.Count - 1].SubItems.Add(sMessage);                
                LVCust.Items[LVCust.Items.Count - 1].SubItems.Add(sDataIndex);
                if (sMessage.Length >= 2 && sMessage.Substring(0, 2) != "OK")
                {
                    sMessage = "Customer ID[" + sCustID + "]" + sMessage;
                }
                TransferFinish(sDataIndex, sMessage, g_StatusType, sTable);
                if (g_StatusType != StatusType.OK)
                {
                    GetColor(LVCust, g_StatusType);
                }
                LabCustomerCNT.Text = LVCust.Items.Count.ToString();
            }    
        }
        private void TransferCustomerItem(string sDataIndex, string sTable)
        {
            string sMessage = "";
            string sModifyType = "";
            g_StatusType = StatusType.OK;
            //ERP上==============================   
            string sOrgan = "";      //Oracle公司別  
            string sCustID = "";   //客戶ID
            string sCustItemNo = ""; //客戶料號
            string sPartNo = ""; //廠內料號 
            string sMESCustID = "0";
            string sMesPartID = "0";
            bool bUpdate = false;
            bool bInsert = false;
            string sEnabled = "Y";
            g_dtSysdate = GetSysDate();
            try
            {
                //由Temp Table讀取資料
                string sCommand = " Select * from " + sTable + " "
                                + " Where DATA_IDX = '" + sDataIndex + "'";
                DataSet dsData = ExecuteSQL(sCommand, null);
                if (dsData.Tables[0].Rows.Count == 0)
                {
                    sMessage = "No Data - Data Index:" + sDataIndex;
                    g_StatusType = StatusType.Error;
                    return;
                }
                sModifyType = dsData.Tables[0].Rows[0]["ERP_FLAG"].ToString();
                sOrgan = dsData.Tables[0].Rows[0]["ORGANIZATION"].ToString();
                sCustID = dsData.Tables[0].Rows[0]["CUSTOMER_ID"].ToString().Trim();
                sCustItemNo = dsData.Tables[0].Rows[0]["CUST_ITEM_NO"].ToString().Trim();
                sPartNo = dsData.Tables[0].Rows[0]["ITEM_NO"].ToString().Trim();
                try
                {   

                    DataRow drCustID = GetRowData("SAJET.SYS_CUSTOMER", "CUSTOMER_CODE", sCustID); //以CUSTOMER_CODE查詢 
                    if (drCustID == null)
                    {

                        sMessage = "Cust ID Data not Exist";
                        g_StatusType = StatusType.Error;
                        return;
                    }
                    sMESCustID = drCustID["CUSTOMER_ID"].ToString();
                    DataRow drPartID = GetRowData("SAJET.SYS_PART", "PART_NO", sPartNo);
                    if (drPartID == null)
                    {
                        sMessage = "Part No Data not Exist";
                        g_StatusType = StatusType.Error;
                        return;
                    }
                    sMesPartID = drPartID["PART_ID"].ToString();


                    DataRow drID = GetRowData("SAJET.SYS_CUSTOMER_PART", "CUSTOMER_ID", sMESCustID,"PART_ID",  sMesPartID); 
                    //Insert====================
                    if (sModifyType == "C")
                    {
                        if (drID != null) 
                        {
                            bUpdate = true;
                        }
                        else
                        {
                            bInsert = true;
                        }
                    }
                    //Update=============================
                    else if (sModifyType == "U")
                    {
                        if (drID == null)
                        {
                            bInsert = true;
                        }
                        else
                        {
                            bUpdate = true;
                        }
                    }
                    if (bInsert)
                    {
                        object[][] Params = new object[5][];
                        sSQL = " INSERT INTO SAJET.SYS_CUSTOMER_PART "
                             + " (CUSTOMER_ID, PART_ID,CUST_PART_NO, UPDATE_USERID,UPDATE_TIME "
                             + "  ) "
                             + " VALUES "
                             + " (:CUSTOMER_ID, :PART_ID, :CUST_PART_NO ,:UPDATE_USERID ,:UPDATE_TIME "
                             + "  ) ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_ID", sMESCustID };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sMesPartID };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUST_PART_NO", sCustItemNo };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[4] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        dsTemp = ExecuteSQL(sSQL, Params);
                        sMessage = "OK(INSERT)";
                    }
                    else if (bUpdate)
                    {
                        object[][] Params = new object[6][];
                        sSQL = " UPDATE SAJET.SYS_CUSTOMER_PART "
                             + "    SET  CUST_PART_NO = :CUST_PART_NO "
                             + "        ,UPDATE_USERID = :UPDATE_USERID "
                             + "        ,UPDATE_TIME = :UPDATE_TIME  "
                             + "        ,ENABLED=:ENABLED "
                             + "  WHERE CUSTOMER_ID = :CUSTOMER_ID "
                             + "    AND PART_ID =:PART_ID ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUST_PART_NO", sCustItemNo };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ENABLED", sEnabled };
                        Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_ID", sMESCustID };
                        Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sMesPartID };
                        dsTemp = ExecuteSQL(sSQL, Params);
                        sMessage = "OK(UPDATE)";
                    }

                    //Delete============================
                    if (sModifyType == "D")
                    {
                        if (drID == null)
                        {
                            sMessage = "Data not Exist";
                            g_StatusType = StatusType.Warning;
                            return;
                        }
                        object[][] Params = new object[2][];
                        sSQL = " DELETE SAJET.SYS_CUSTOMER_PART "
                             + "  WHERE CUSTOMER_ID = :CUSTOMER_ID "
                             + "   AND PART_ID =:PART_ID ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_ID", sMESCustID };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sMesPartID };
                        dsTemp = ExecuteSQL(sSQL, Params);
                        sMessage = "OK(DELETE)";
                    }
                }
                catch (Exception exp)
                {
                    sMessage = exp.Message;
                    g_StatusType = StatusType.Error;
                }
            }
            finally
            {

                LVCustItem.Items.Add(sCustID);
                LVCustItem.Items[LVCustItem.Items.Count - 1].SubItems.Add(sCustItemNo);
                LVCustItem.Items[LVCustItem.Items.Count - 1].SubItems.Add(sPartNo);
                LVCustItem.Items[LVCustItem.Items.Count - 1].SubItems.Add(sMessage);
                LVCustItem.Items[LVCustItem.Items.Count - 1].SubItems.Add(sDataIndex);
                if (sMessage.Length >= 2 && sMessage.Substring(0, 2) != "OK")
                {
                    sMessage = "Customer ID[" + sCustID + "]Cust Item No[" + sCustItemNo + "]" + sMessage;
                }
                TransferFinish(sDataIndex, sMessage, g_StatusType, sTable);
                if (g_StatusType != StatusType.OK)
                {
                    GetColor(LVCustItem, g_StatusType);
                }
                lablCustItemCNT.Text = LVCustItem.Items.Count.ToString();
            }
        }
        private void TransferVendor(string sDataIndex, string sTable)
        {            
            string sMessage = "";
            string sModifyType = "";
            g_StatusType = StatusType.OK;
            //ERP上==============================    
            string sOrgan = "";      //Oracle公司別      
            string sVendorID = "";   //供應商ID
            string sVendorName = ""; //供應商名稱
            string sVendorTEL = "";   //供應商電話
            string sVendorCONTACT = "";   //供應商聯絡人
            string sVendorEmail = "";   //供應商簡稱
            string sVendroAbbr = "";
            
            //==================================     
            string sEnabled = "Y";
            string sMESID = "0";
            bool bUpdate = false;
            bool bInsert = false;
            g_dtSysdate = GetSysDate();

            try
            {
                //由Temp Table讀取資料
                string sCommand = " Select * from " + sTable + " "
                                + " Where DATA_IDX = '" + sDataIndex + "'";
                DataSet dsData = ExecuteSQL(sCommand, null);
                if (dsData.Tables[0].Rows.Count == 0)
                {
                    sMessage = "No Data - Data Index:" + sDataIndex;
                    g_StatusType = StatusType.Error;
                    return;
                }
                sModifyType = dsData.Tables[0].Rows[0]["ERP_FLAG"].ToString();
                sOrgan = dsData.Tables[0].Rows[0]["ORGANIZATION"].ToString();
                sVendorID = dsData.Tables[0].Rows[0]["VENDOR_ID"].ToString().Trim();
                sVendorName = dsData.Tables[0].Rows[0]["VENDOR_NAME"].ToString().Trim();
                sVendorTEL = dsData.Tables[0].Rows[0]["VENDOR_TEL"].ToString().Trim();
                sVendorCONTACT = dsData.Tables[0].Rows[0]["VENDOR_CONTACT"].ToString().Trim();
                sVendorEmail = dsData.Tables[0].Rows[0]["VENDOR_EMAIL"].ToString().Trim();
                sVendroAbbr = dsData.Tables[0].Rows[0]["VENDOR_NAME_ABBR"].ToString().Trim();

                try
                {

                    DataRow drID = GetRowData("SAJET.SYS_VENDOR", "VENDOR_CODE", sVendorID); //以Vendor Code查詢
                    //Insert====================
                    if (sModifyType == "C")
                    {
                        //MES中是否已有此ERPID
                        if (drID != null) //資料已存在
                        {
                            if (drID["ENABLED"].ToString() == "Y")
                            {
                                sMessage = "Data Exist";
                                g_StatusType = StatusType.Warning;
                                return;
                            }
                            sMESID = drID["VENDOR_ID"].ToString();
                            bUpdate = true;
                        }
                        else
                        {
                            bInsert = true;
                        }
                    }
                    //Update=============================
                    else if (sModifyType == "U")
                    {
                        if (drID == null)
                        {
                            /*
                            sMessage = "Data not Exist";
                            g_StatusType = StatusType.Error;
                            return;
                             */
                            bInsert = true;
                        }
                        else
                        {
                            sMESID = drID["VENDOR_ID"].ToString();
                            bUpdate = true;
                        }
                    }
                    if (bInsert)
                    {
                        sMESID = GetMaxID("SAJET.SYS_VENDOR", "VENDOR_ID", 8, ref sMessage);
                        if (sMESID == "0")
                        {
                            sMessage = "Get MaxID Error";
                            g_StatusType = StatusType.Error;
                            return;
                        }
                        object[][] Params = new object[9][];
                        sSQL = " INSERT INTO SAJET.SYS_VENDOR "
                             + " (VENDOR_ID, VENDOR_CODE, VENDOR_NAME, UPDATE_USERID,UPDATE_TIME, VENDOR_TEL, VENDOR_CONTACT,VENDOR_EMAIL,VENDOR_NAME_ABBR) "
                             + " VALUES "
                             + " (:VENDOR_ID, :VENDOR_CODE, :VENDOR_NAME, :UPDATE_USERID, :UPDATE_TIME, :VENDOR_TEL, :VENDOR_CONTACT,:VENDOR_EMAIL,:VENDOR_NAME_ABBR) ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_ID", sMESID };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_CODE", sVendorID };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_NAME", sVendorName };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[4] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_TEL", sVendorTEL };
                        Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_CONTACT", sVendorCONTACT };
                        Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_EMAIL", sVendorEmail };
                        Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_NAME_ABBR", sVendroAbbr };
                        dsTemp = ExecuteSQL(sSQL, Params);
                        sMessage = "OK(INSERT)";
                    }
                    else if (bUpdate)
                    {                        
                        object[][] Params = new object[10][];
                        sSQL = " UPDATE SAJET.SYS_VENDOR "
                             + "    SET  VENDOR_CODE = :VENDOR_CODE "
                             + "        ,VENDOR_NAME = :VENDOR_NAME "
                             + "        ,UPDATE_USERID = :UPDATE_USERID "
                             + "        ,UPDATE_TIME = :UPDATE_TIME  "
                             + "        ,VENDOR_TEL = :VENDOR_TEL "
                             + "        ,VENDOR_CONTACT = :VENDOR_CONTACT "
                             + "        ,VENDOR_EMAIL = :VENDOR_EMAIL "
                             + "        ,ENABLED =:ENABLED "
                             + "        ,VENDOR_NAME_ABBR =:VENDOR_NAME_ABBR "
                             + "  WHERE VENDOR_ID = :VENDOR_ID ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_CODE", sVendorID };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_NAME", sVendorName };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_TEL", sVendorTEL };
                        Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_CONTACT", sVendorCONTACT };
                        Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_EMAIL", sVendorEmail };
                        Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ENABLED", sEnabled };
                        Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_NAME_ABBR", sVendroAbbr };
                        Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_ID", sMESID };
                        dsTemp = ExecuteSQL(sSQL, Params);
                        sMessage = "OK(UPDATE)";
                    }

                    //Delete============================
                    if (sModifyType == "D")
                    {
                        if (drID == null)
                        {
                            sMessage = "Data not Exist";
                            g_StatusType = StatusType.Warning;
                            return;
                        }
                        sMESID = drID["VENDOR_ID"].ToString();
                        object[][] Params = new object[3][];
                        sSQL = " UPDATE SAJET.SYS_VENDOR "
                             + "    SET ENABLED = 'N' "
                             + "       ,UPDATE_USERID = :UPDATE_USERID "
                             + "       ,UPDATE_TIME = :UPDATE_TIME "
                             + "  WHERE VENDOR_ID = :VENDOR_ID ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_ID", sMESID };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        dsTemp = ExecuteSQL(sSQL, Params);
                        sMessage = "OK(DELETE)";
                    }
                }
                catch (Exception exp)
                {
                    sMessage = exp.Message;
                    g_StatusType = StatusType.Error;
                }
            }
            finally
            {
                LVVendor.Items.Add(sVendorID);
                LVVendor.Items[LVVendor.Items.Count - 1].SubItems.Add(sVendorName);
                LVVendor.Items[LVVendor.Items.Count - 1].SubItems.Add(sMessage);
                LVVendor.Items[LVVendor.Items.Count - 1].SubItems.Add(sDataIndex);
                if (sMessage.Length >= 2 && sMessage.Substring(0, 2) != "OK")
                {
                    sMessage = "Vendor ID[" + sVendorID + "]" + sMessage;
                }
                TransferFinish(sDataIndex, sMessage, g_StatusType, sTable);
                if (g_StatusType != StatusType.OK)
                {
                    GetColor(LVVendor, g_StatusType);
                }
                LabVendorCNT.Text = LVVendor.Items.Count.ToString();
            } 
        }

        private void TransferECNWO(string sDataIndex, string sTable)
        {
            string sMessage = "";
            string sModifyType = "";
            g_StatusType = StatusType.OK;
            //ERP上==============================    
            string sOrgan = "";      //Oracle公司別      
            string sECNNo = "";   //ECN單號
            string sWorkOrder = ""; //工單
            string sECRNo = "";
            //==================================     
            string sEnabled = "Y";
            bool bUpdate = false;
            bool bInsert = false;
            g_dtSysdate = GetSysDate();
            try
            {
                //由Temp Table讀取資料
                string sCommand = " Select * from " + sTable + " "
                                + " Where DATA_IDX = '" + sDataIndex + "'";
                DataSet dsData = ExecuteSQL(sCommand, null);
                if (dsData.Tables[0].Rows.Count == 0)
                {
                    sMessage = "No Data - Data Index:" + sDataIndex;
                    g_StatusType = StatusType.Error;
                    return;
                }
                sModifyType = dsData.Tables[0].Rows[0]["ERP_FLAG"].ToString();
                sOrgan = dsData.Tables[0].Rows[0]["ORGANIZATION"].ToString();
                sECNNo = dsData.Tables[0].Rows[0]["ECN_NO"].ToString().Trim();
                sWorkOrder = dsData.Tables[0].Rows[0]["JOB_NUMBER"].ToString().Trim();
                sECRNo = dsData.Tables[0].Rows[0]["ECR_NO"].ToString().Trim();
                try
                {

                    DataRow drID = GetRowData("SAJET.G_ECN_WO", "ECN_NO", sECNNo, "WORK_ORDER", sWorkOrder); //以Vendor Code查詢
                    //Insert====================
                    if (sModifyType == "C")
                    {
                        //MES中是否已有此ERPID
                        if (drID != null) //資料已存在
                        {
                            bUpdate = true;
                        }
                        else
                        {
                            bInsert = true;
                        }
                    }
                    //Update=============================
                    else if (sModifyType == "U")
                    {
                        if (drID == null)
                        {
                            bInsert = true;
                        }
                        else
                        {
                            bUpdate = true;
                        }
                    }
                    if (bInsert)
                    {                        
                        sSQL = " SELECT WO_STATUS FROM SAJET.G_WO_BASE "
                          + "  WHERE WORK_ORDER =:WORK_ORDER AND ROWNUM = 1 ";
                        object[][] Params = new object[1][];
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWorkOrder };
                        dsTemp = ExecuteSQL(sSQL, Params);
                        string sWOStatus = "";
                        if (dsTemp.Tables[0].Rows.Count > 0)
                        {
                            sWOStatus = dsTemp.Tables[0].Rows[0]["WO_STATUS"].ToString();
                        }
                        else
                        {
                            sMessage = "W/O not Exist - Job Number:" + sWorkOrder;
                            g_StatusType = StatusType.Error;
                            return;
                        }
                        Params = new object[6][];
                        sSQL = " INSERT INTO SAJET.G_ECN_WO "
                             + " (ECN_NO, WORK_ORDER, UPDATE_USERID,UPDATE_TIME,PRIOR_WO_STATUS,ECR_NO ) "
                             + " VALUES "
                             + " (:ECN_NO, :WORK_ORDER, :UPDATE_USERID, :UPDATE_TIME,:PRIOR_WO_STATUS,:ECR_NO ) ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ECN_NO", sECNNo };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWorkOrder };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PRIOR_WO_STATUS", sWOStatus };
                        Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ECR_NO", sECRNo };
                        dsTemp = ExecuteSQL(sSQL, Params);
                        if (sWOStatus == "0" || sWOStatus == "1" || sWOStatus == "2" || sWOStatus == "3" || sWOStatus == "4")
                        {
                            sSQL = "UPDATE SAJET.G_WO_BASE "
                                + "   SET WO_STATUS= '8' "
                                + "      ,UPDATE_USERID=:UPDATE_USERID "
                                + "      ,UPDATE_TIME =:UPDATE_TIME "
                                    + "WHERE WORK_ORDER =:WORK_ORDER ";
                            Params = new object[3][];
                            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                            Params[1] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWorkOrder };
                            dsTemp = ExecuteSQL(sSQL, Params);
                            InsertHTWOBASE(sWorkOrder);
                        }
                        sMessage = "OK(INSERT)";
                    }
                    else if (bUpdate)
                    {
                        object[][] Params = new object[5][];
                        sSQL = " UPDATE SAJET.G_ECN_WO "
                             + "    SET  UPDATE_USERID = :UPDATE_USERID "
                             + "        ,UPDATE_TIME = :UPDATE_TIME  "
                             + "        ,ECR_NO =:ECR_NO "
                             + "  WHERE ECN_NO = :ECN_NO "
                             + "    AND WORK_ORDER =:WORK_ORDER ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ECR_NO", sECRNo };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ECN_NO", sECNNo };
                        Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWorkOrder };
                        dsTemp = ExecuteSQL(sSQL, Params);
                        sMessage = "OK(UPDATE)";
                    }

                    //Delete============================
                    if (sModifyType == "D")
                    {
                        if (drID == null)
                        {
                            sMessage = "Data not Exist";
                            g_StatusType = StatusType.Warning;
                            return;
                        }
                        if (drID["RELEASE_STATUS"].ToString() == "Y")
                        {
                            sMessage = "WO Released";
                            g_StatusType = StatusType.Warning;
                            return;
                        }
                        string sPriorWOStatus = drID["PRIOR_WO_STATUS"].ToString();

                        object[][] Params = new object[2][];
                        sSQL = " DELETE SAJET.G_ECN_WO "
                             + "  WHERE ECN_NO = :ECN_NO "
                             + "    AND WORK_ORDER =:WORK_ORDER ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ECN_NO", sECNNo };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWorkOrder };
                        dsTemp = ExecuteSQL(sSQL, Params);

                        sSQL = "UPDATE SAJET.G_WO_BASE "
                               + "   SET WO_STATUS= :WO_STATUS "
                               + "      ,UPDATE_USERID=:UPDATE_USERID "
                               + "      ,UPDATE_TIME =:UPDATE_TIME "
                                   + "WHERE WORK_ORDER =:WORK_ORDER ";
                        Params = new object[4][];
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WO_STATUS", sPriorWOStatus };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWorkOrder };
                        dsTemp = ExecuteSQL(sSQL, Params);
                        InsertHTWOBASE(sWorkOrder);
                        sMessage = "OK(DELETE)";
                    }
                }
                catch (Exception exp)
                {
                    sMessage = exp.Message;
                    g_StatusType = StatusType.Error;
                }
            }
            finally
            {                
                LVECN.Items.Add(sECNNo);
                LVECN.Items[LVECN.Items.Count - 1].SubItems.Add(sWorkOrder);
                LVECN.Items[LVECN.Items.Count - 1].SubItems.Add(sMessage);
                LVECN.Items[LVECN.Items.Count - 1].SubItems.Add(sDataIndex);
                if (sMessage.Length >= 2 && sMessage.Substring(0, 2) != "OK")
                {
                    sMessage = "ECN No[" + sECNNo + "]Job Number[" + sWorkOrder + "]" + sMessage;
                }
                TransferFinish(sDataIndex, sMessage, g_StatusType, sTable);
                if (g_StatusType != StatusType.OK)
                {
                    GetColor(LVECN, g_StatusType);
                }
                labECNCNT.Text = LVECN.Items.Count.ToString();
            }
        }

        private void TransferTimeRange(string sDataIndex, string sTable)
        {
            string sMessage = "";
            string sModifyType = "";
            g_StatusType = StatusType.OK;
            //ERP上==============================   
            string sStartTime = ""; //開始時間  
            string sEndTime = "";   //結束時間
            //==================================            
            string sMESID = "0";
            bool bUpdate = false;
            bool bInsert = false;
            string sEnabled = "Y";
            g_dtSysdate = GetSysDate();

            try
            {
                //由Temp Table讀取資料
                string sCommand = " Select * from " + sTable + " "
                                + " Where DATA_IDX = '" + sDataIndex + "'";
                DataSet dsData = ExecuteSQL(sCommand, null);
                if (dsData.Tables[0].Rows.Count == 0)
                {
                    sMessage = "No Data - Data Index:" + sDataIndex;
                    g_StatusType = StatusType.Error;
                    return;
                }

                sModifyType = dsData.Tables[0].Rows[0]["ERP_FLAG"].ToString();
                sStartTime = dsData.Tables[0].Rows[0]["START_TIME"].ToString();
                sEndTime = dsData.Tables[0].Rows[0]["END_TIME"].ToString().Trim();

                try
                {
                    if (sModifyType == "C")
                    {
                        sSQL = " DELETE MESUSER.G_RC_WIP ";
                        dsTemp = ExecuteSQL(sSQL, null);

                        object[][] Params = new object[4][];
                        sSQL = " INSERT INTO MESUSER.G_RC_WIP (START_TIME,END_TIME,JOB_NUMBER,PROCESS_NAME,CURRENT_QTY,WORK_TIME) "
                             + "   SELECT :START_TIME,:END_TIME,A.WORK_ORDER,B.PROCESS_NAME,SUM(A.CURRENT_QTY) AS CURRENT_QTY, "
                             + "          SUM(A.WORKHOUR) AS WORK_TIME "
                             //+ "          SUM(ROUND(TO_NUMBER(A.WIP_OUT_TIME - NVL(A.WIP_IN_TIME,A.WIP_OUT_TIME)) * 1440)) AS WORK_TIME "
                             + "     FROM SAJET.G_RC_TRAVEL A,SAJET.SYS_PROCESS B "
                             + "    WHERE A.PROCESS_ID = B.PROCESS_ID "
                             + "      AND NVL(A.WIP_IN_TIME,A.WIP_OUT_TIME) > TO_DATE(:START_TIME2,'YYYYMMDDHH24') "
                             + "      AND A.WIP_OUT_TIME < TO_DATE(:END_TIME2,'YYYYMMDDHH24') "
                             + " GROUP BY A.WORK_ORDER,B.PROCESS_NAME ";

                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "START_TIME", sStartTime };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "END_TIME", sEndTime };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "START_TIME2", sStartTime };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "END_TIME2", sEndTime };
                        dsTemp = ExecuteSQL(sSQL, Params);
                        sMessage = "OK(INSERT)";
                    }
                }
                catch (Exception exp)
                {
                    sMessage = exp.Message;
                    g_StatusType = StatusType.Error;
                }
            }
            finally
            {
                LVWIP.Items.Add(sStartTime);
                LVWIP.Items[LVWIP.Items.Count - 1].SubItems.Add(sEndTime);
                LVWIP.Items[LVWIP.Items.Count - 1].SubItems.Add(sMessage);
                LVWIP.Items[LVWIP.Items.Count - 1].SubItems.Add(sDataIndex);
                if (sMessage.Length >= 2 && sMessage.Substring(0, 2) != "OK")
                {
                    sMessage = "Time Range[" + sStartTime + " - " + sEndTime + "]" + sMessage;
                }
                TransferFinish(sDataIndex, sMessage, g_StatusType, sTable);
                if (g_StatusType != StatusType.OK)
                {
                    GetColor(LVWIP, g_StatusType);
                }
                lablWIPCNT.Text = LVWIP.Items.Count.ToString();
            }  
        }

        private void TransferWoTransfer(string sDataIndex, string sTable)
        {
            string sMessage = "";
            string sModifyType = "";
            g_StatusType = StatusType.OK;
            //ERP上==============================   
            decimal dCurrentQty = 0; //流程卡數量
            string sRCNo = "";       //流程卡
            string sJobNumber = "";  //轉投工單
            //==================================            
            string sMESID = "0";
            bool bUpdate = false;
            bool bInsert = false;
            string sEnabled = "Y";
            g_dtSysdate = GetSysDate();

            try
            {
                //由Temp Table讀取資料
                string sCommand = " Select * from " + sTable + " "
                                + " Where DATA_IDX = '" + sDataIndex + "'";
                DataSet dsData = ExecuteSQL(sCommand, null);
                if (dsData.Tables[0].Rows.Count == 0)
                {
                    sMessage = "No Data - Data Index:" + sDataIndex;
                    g_StatusType = StatusType.Error;
                    return;
                }

                sModifyType = dsData.Tables[0].Rows[0]["ERP_FLAG"].ToString();
                sRCNo = dsData.Tables[0].Rows[0]["RC_NO"].ToString();
                sJobNumber = dsData.Tables[0].Rows[0]["JOB_NUMBER"].ToString().Trim();
                dCurrentQty = Convert.ToDecimal(dsData.Tables[0].Rows[0]["CURRENT_QTY"].ToString());

                int iPart_Id = 0, iRoute_Id = 0, iLine_Id = 0, iFactory_Id = 0;
                int iProcess_Id = 0, iStage_Id = 0;
                long iNode_Id = 0, iNext_Node = 0;
                string sVersion = "", sNext_Process = "", sSheet_Name = "";
                decimal dQty = 0;

                try
                {
                    if (sModifyType == "C")
                    {
                        if (Convert.ToInt32(sRCNo.Substring(sRCNo.Length - 2, 1)) == 9)
                        {
                            sMessage = "RC lotsize is full";
                            g_StatusType = StatusType.Warning;
                            return;
                        }

                        iPart_Id = 0;
                        sVersion = "";
                        iRoute_Id = 0;
                        iLine_Id = 0;
                        iFactory_Id = 0;

                        sSQL = " SELECT A.PART_ID,A.VERSION,A.ROUTE_ID,A.DEFAULT_PDLINE_ID,A.FACTORY_ID,(TARGET_QTY-INPUT_QTY) AS QTY"
                             + "   FROM SAJET.G_WO_BASE A,SAJET.SYS_PDLINE B"
                             + "  WHERE A.DEFAULT_PDLINE_ID = B.PDLINE_ID"
                             + "    AND WORK_ORDER = '" + sJobNumber + "'";
                        dsTemp = ExecuteSQL(sSQL, null);

                        if (dsTemp.Tables[0].Rows.Count > 0)
                        {
                            iPart_Id = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["PART_ID"].ToString());
                            sVersion = dsTemp.Tables[0].Rows[0]["VERSION"].ToString();
                            iRoute_Id = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["ROUTE_ID"].ToString());
                            iLine_Id = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["DEFAULT_PDLINE_ID"].ToString());
                            iFactory_Id = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["FACTORY_ID"].ToString());
                            dQty = Convert.ToDecimal(dsTemp.Tables[0].Rows[0]["QTY"].ToString());
                        }

                        if (dCurrentQty > dQty)
                        {
                            sMessage = "Current Qty More Than Available Qty";
                            g_StatusType = StatusType.Warning;
                            return;
                        }

                        iProcess_Id = 0;
                        sNext_Process = "";
                        sSheet_Name = "";
                        iStage_Id = 0;
                        iNode_Id = 0;
                        iNext_Node = 0;

                        object[][] ParamsProc = new object[7][];
                        ParamsProc[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "prouteid", iRoute_Id };
                        ParamsProc[1] = new object[] { ParameterDirection.Output, OracleType.VarChar, "vprocessid", "" };
                        ParamsProc[2] = new object[] { ParameterDirection.Output, OracleType.VarChar, "vnextprocess", "" };
                        ParamsProc[3] = new object[] { ParameterDirection.Output, OracleType.VarChar, "vsheetname", "" };
                        ParamsProc[4] = new object[] { ParameterDirection.Output, OracleType.VarChar, "vstageid", "" };
                        ParamsProc[5] = new object[] { ParameterDirection.Output, OracleType.VarChar, "vnodeid", "" };
                        ParamsProc[6] = new object[] { ParameterDirection.Output, OracleType.VarChar, "vnextnode", "" };
                        DataSet DSProc = ExecuteProc("SAJET.SJ_GET_FIRSTPROCESS", ParamsProc);

                        iProcess_Id = Convert.ToInt32(DSProc.Tables[0].Rows[0]["vprocessid"].ToString());

                        if (DSProc.Tables[0].Rows[0]["vnextprocess"].ToString() != "0")
                        {
                            sNext_Process = DSProc.Tables[0].Rows[0]["vnextprocess"].ToString();
                        }

                        sSheet_Name = DSProc.Tables[0].Rows[0]["vsheetname"].ToString();
                        iStage_Id = Convert.ToInt32(DSProc.Tables[0].Rows[0]["vstageid"].ToString());
                        iNode_Id = Convert.ToInt64(DSProc.Tables[0].Rows[0]["vnodeid"].ToString());
                        iNext_Node = Convert.ToInt64(DSProc.Tables[0].Rows[0]["vnextnode"].ToString());

                        // New RC No
                        string s_RC_End = "";

                        sSQL = " SELECT MAX(SUBSTR(RC_NO,-2,1) + 1) AS RC_END "
                             + "   FROM SAJET.G_RC_STATUS "
                             + "  WHERE RC_NO LIKE '" + sRCNo.Substring(0, sRCNo.Length - 1) + "%'";
                        dsTemp = ExecuteSQL(sSQL, null);

                        if (dsTemp.Tables[0].Rows.Count > 0)
                        {
                            if (Convert.ToInt32(dsTemp.Tables[0].Rows[0]["RC_END"].ToString()) > 9)
                            {
                                sMessage = "RC lotsize is full";
                                g_StatusType = StatusType.Warning;
                                return;
                            }

                            s_RC_End = sRCNo.Substring(0, sRCNo.Length - 2) + dsTemp.Tables[0].Rows[0]["RC_END"].ToString() + sRCNo.Substring(sRCNo.Length - 1, 1);
                        }

                        DateTime datExeTime = DateTime.Now;
                        long lTravel_Id = Convert.ToInt64(datExeTime.ToString("yyyyMMddHHmmssf"));

                        // Insert SAJET.G_RC_SPLIT
                        for (int j = 0; j < 2; j++)
                        {
                            sSQL = @"INSERT INTO SAJET.G_RC_SPLIT
                                     (RC_NO,RC_QTY,SOURCE_RC_NO,SOURCE_RC_QTY,TRAVEL_ID,PROCESS_ID,UPDATE_USERID,UPDATE_TIME)
                                     VALUES
                                     (:RC_NO,:RC_QTY,:SOURCE_RC_NO,:SOURCE_RC_QTY,:TRAVEL_ID,:PROCESS_ID,:UPDATE_USERID,:UPDATE_TIME)";

                            object[][] ParamsSplit = new object[8][];
                            ParamsSplit[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SOURCE_RC_NO", sRCNo };
                            ParamsSplit[1] = new object[] { ParameterDirection.Input, OracleType.Number, "SOURCE_RC_QTY", dCurrentQty };
                            ParamsSplit[2] = new object[] { ParameterDirection.Input, OracleType.Number, "PROCESS_ID", iProcess_Id };
                            ParamsSplit[3] = new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", 0 };
                            ParamsSplit[4] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime };

                            if (j == 0)
                            {
                                ParamsSplit[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", sRCNo };
                                ParamsSplit[6] = new object[] { ParameterDirection.Input, OracleType.Number, "RC_QTY", 0 };
                                ParamsSplit[7] = new object[] { ParameterDirection.Input, OracleType.Number, "TRAVEL_ID", lTravel_Id };
                            }
                            else
                            {
                                ParamsSplit[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", s_RC_End };
                                ParamsSplit[6] = new object[] { ParameterDirection.Input, OracleType.Number, "RC_QTY", dCurrentQty };
                                ParamsSplit[7] = new object[] { ParameterDirection.Input, OracleType.Number, "TRAVEL_ID", lTravel_Id };
                            }

                            dsTemp = ExecuteSQL(sSQL, ParamsSplit);
                        }

                        // Insert SAJET.G_RC_STATUS
                        sSQL = @"INSERT INTO SAJET.G_RC_STATUS (WORK_ORDER,RC_NO,PART_ID,VERSION,ROUTE_ID,FACTORY_ID,PDLINE_ID,STAGE_ID,NODE_ID,
                                                                PROCESS_ID,SHEET_NAME,TERMINAL_ID,TRAVEL_ID,NEXT_NODE,NEXT_PROCESS,CURRENT_STATUS,
                                                                CURRENT_QTY,IN_PROCESS_EMPID,IN_PROCESS_TIME,WIP_IN_QTY,WIP_IN_EMPID,WIP_IN_MEMO,
                                                                WIP_IN_TIME,WIP_OUT_GOOD_QTY,WIP_OUT_SCRAP_QTY,WIP_OUT_EMPID,WIP_OUT_MEMO,
                                                                WIP_OUT_TIME,OUT_PROCESS_EMPID,OUT_PROCESS_TIME,HAVE_SN,PRIORITY_LEVEL,UPDATE_USERID,
                                                                UPDATE_TIME,CREATE_TIME,BATCH_ID,EMP_ID,BONUS_QTY,WORKTIME,INITIAL_QTY)
                                 VALUES (:WORK_ORDER,:RC_NO,:PART_ID,:VERSION,:ROUTE_ID,:FACTORY_ID,:PDLINE_ID,:STAGE_ID,:NODE_ID,
                                 :PROCESS_ID,:SHEET_NAME,:TERMINAL_ID,:TRAVEL_ID,:NEXT_NODE,:NEXT_PROCESS,:CURRENT_STATUS,
                                 :CURRENT_QTY,NULL,NULL,NULL,NULL,NULL,
                                 NULL,NULL,NULL,NULL,NULL,
                                 NULL,NULL,NULL,:HAVE_SN,:PRIORITY_LEVEL,:UPDATE_USERID,
                                 :UPDATE_TIME,:CREATE_TIME,NULL,:EMP_ID,:BONUS_QTY,:WORKTIME,:INITIAL_QTY)";

                        object[][] ParamsNew = new object[26][];
                        ParamsNew[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sJobNumber };
                        ParamsNew[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", s_RC_End };
                        ParamsNew[2] = new object[] { ParameterDirection.Input, OracleType.Number, "PART_ID", iPart_Id };
                        ParamsNew[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VERSION", sVersion };
                        ParamsNew[4] = new object[] { ParameterDirection.Input, OracleType.Number, "ROUTE_ID", iRoute_Id };
                        ParamsNew[5] = new object[] { ParameterDirection.Input, OracleType.Number, "FACTORY_ID", iFactory_Id };
                        ParamsNew[6] = new object[] { ParameterDirection.Input, OracleType.Number, "PDLINE_ID", iLine_Id };
                        ParamsNew[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "STAGE_ID", iStage_Id };
                        ParamsNew[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NODE_ID", iNode_Id };
                        ParamsNew[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", iProcess_Id };
                        ParamsNew[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_NAME", sSheet_Name };
                        ParamsNew[11] = new object[] { ParameterDirection.Input, OracleType.Number, "TERMINAL_ID", 0 };
                        ParamsNew[12] = new object[] { ParameterDirection.Input, OracleType.Number, "TRAVEL_ID", lTravel_Id };
                        ParamsNew[13] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NEXT_NODE", iNext_Node };
                        ParamsNew[14] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NEXT_PROCESS", sNext_Process };
                        ParamsNew[15] = new object[] { ParameterDirection.Input, OracleType.Number, "CURRENT_STATUS", 0 };
                        ParamsNew[16] = new object[] { ParameterDirection.Input, OracleType.Number, "CURRENT_QTY", dCurrentQty };
                        ParamsNew[17] = new object[] { ParameterDirection.Input, OracleType.Number, "HAVE_SN", 0 };
                        ParamsNew[18] = new object[] { ParameterDirection.Input, OracleType.Number, "PRIORITY_LEVEL", 0 };
                        ParamsNew[19] = new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", 0 };
                        ParamsNew[20] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime };
                        ParamsNew[21] = new object[] { ParameterDirection.Input, OracleType.DateTime, "CREATE_TIME", datExeTime };
                        ParamsNew[22] = new object[] { ParameterDirection.Input, OracleType.Number, "EMP_ID", 0 };
                        ParamsNew[23] = new object[] { ParameterDirection.Input, OracleType.Number, "BONUS_QTY", 0 };
                        ParamsNew[24] = new object[] { ParameterDirection.Input, OracleType.Number, "WORKTIME", 0 };
                        ParamsNew[25] = new object[] { ParameterDirection.Input, OracleType.Number, "INITIAL_QTY", dCurrentQty };
                        dsTemp = ExecuteSQL(sSQL, ParamsNew);

                        // Update SAJET.G_RC_STATUS
                        sSQL = @"UPDATE SAJET.G_RC_STATUS
                                    SET TRAVEL_ID = :TRAVEL_ID,
                                        CURRENT_STATUS = :CURRENT_STATUS,
                                        CURRENT_QTY = :CURRENT_QTY,
                                        UPDATE_USERID = :UPDATE_USERID,
                                        UPDATE_TIME = :UPDATE_TIME,
                                        BONUS_QTY = :BONUS_QTY,
                                        WORKTIME = :WORKTIME,
                                        INITIAL_QTY = :INITIAL_QTY
                                  WHERE RC_NO = :RC_NO";

                        object[][] Params = new object[9][];
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.Number, "TRAVEL_ID", lTravel_Id };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.Number, "CURRENT_STATUS", 12 };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.Number, "CURRENT_QTY", 0 };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", 0 };
                        Params[4] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime };
                        Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", sRCNo };
                        Params[6] = new object[] { ParameterDirection.Input, OracleType.Number, "BONUS_QTY", 0 };
                        Params[7] = new object[] { ParameterDirection.Input, OracleType.Number, "WORKTIME", 0 };
                        Params[8] = new object[] { ParameterDirection.Input, OracleType.Number, "INITIAL_QTY", 0 };
                        dsTemp = ExecuteSQL(sSQL, Params);

                        sSQL = " SELECT COUNT(*) AS COUNTS"
                             + "   FROM SAJET.G_RC_STATUS "
                             + "  WHERE CURRENT_STATUS IN ('0','1','2','11') "
                             + "    AND WORK_ORDER IN (SELECT WORK_ORDER FROM SAJET.G_RC_STATUS WHERE RC_NO = '" + sRCNo + "') ";
                        dsTemp = ExecuteSQL(sSQL, null);

                        if (dsTemp.Tables[0].Rows[0]["COUNTS"].ToString() == "0")
                        {
                            object[][] woParams = new object[2][];
                            sSQL = "UPDATE SAJET.G_WO_BASE"
                                 + "   SET UPDATE_USERID = :UPDATE_USERID,"
                                 + "       UPDATE_TIME = :UPDATE_TIME,"
                                 + "       WO_STATUS = 6"
                                 + " WHERE WORK_ORDER IN (SELECT WORK_ORDER FROM SAJET.G_RC_STATUS WHERE RC_NO = '" + sRCNo + "') ";
                            woParams[0] = new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", 0 };
                            woParams[1] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime };
                            dsTemp = ExecuteSQL(sSQL, woParams);

                            sSQL = " INSERT INTO MESUSER.G_WO_STATUS (JOB_NUMBER,JOB_STATUS) "
                                 + " SELECT WORK_ORDER,8 FROM  SAJET.G_RC_STATUS WHERE RC_NO = '" + sRCNo + "'";
                            dsTemp = ExecuteSQL(sSQL, null);
                        }

                        object[][] woParams2 = new object[2][];
                        sSQL = "UPDATE SAJET.G_WO_BASE"
                             + "   SET INPUT_QTY = INPUT_QTY + " + dCurrentQty + ","
                             + "       UPDATE_USERID = :UPDATE_USERID,"
                             + "       UPDATE_TIME = SYSDATE,"
                             + "       WO_STATUS = 3"
                             + " WHERE WORK_ORDER = :WORK_ORDER";
                        woParams2[0] = new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", 0 };
                        woParams2[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sJobNumber };
                        dsTemp = ExecuteSQL(sSQL, woParams2);
                        sMessage = "OK(INSERT)";
                    }
                }
                catch (Exception exp)
                {
                    sMessage = exp.Message;
                    g_StatusType = StatusType.Error;
                }
            }
            finally
            {
                LVWOT.Items.Add(sJobNumber);
                LVWOT.Items[LVWOT.Items.Count - 1].SubItems.Add(sRCNo);
                LVWOT.Items[LVWOT.Items.Count - 1].SubItems.Add(sMessage);
                LVWOT.Items[LVWOT.Items.Count - 1].SubItems.Add(sDataIndex);
                if (sMessage.Length >= 2 && sMessage.Substring(0, 2) != "OK")
                {
                    sMessage = "WO Transfer[" + sRCNo + " - " + sJobNumber + "]" + sMessage;
                }
                TransferFinish(sDataIndex, sMessage, g_StatusType, sTable);
                if (g_StatusType != StatusType.OK)
                {
                    GetColor(LVWOT, g_StatusType);
                }
                LabWoTransferCNT.Text = LVWOT.Items.Count.ToString();
            }
        }

        private void InsertHTWOBASE(string sWO)
        {
            string sSQL = "INSERT INTO SAJET.G_HT_WO_BASE "
                        + " SELECT * FROM SAJET.G_WO_BASE "
                        + " WHERE WORK_ORDER =:WORK_ORDER ";
            object[][] Params = new object[1][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWO };
            dsTemp = ExecuteSQL(sSQL, Params);
        }
        private void TransferWoHeader(string sDataIndex, string sTable)
        {                        
            string sMessage = "";
            string sModifyType = "";
            g_StatusType = StatusType.OK;
            //ERP上==============================            
            string sOrgan = "";             //Oracle公司別
            string sJobNumber = "";         //工單號碼
            string sJobType = "";           //
            string sJobStatus = "";         //1 = unreleased; 3 = released; 4 = complete;
            string sItemNo = "";            //成品料號
            string sBOMRev = "N/A";         //BOM版本,因凌華變更版本就變更料號,所以此欄固定為N/A即可
            string sPlanQty = "";           //計劃量
            string sDepartment = "";        //部門別
            

            DateTime dtPlanStartDate = DateTime.Now;  //預計投產日
            DateTime dtPlanEndDate = DateTime.Now;   //預計產出日
            //DateTime dtERPReleaseDate = DateTime.Now;  //ERP Release Date  
            string sPlanStartDate = "";  
            string sPlanEndDate = "";
            string sCustCode = "";//客戶代碼
            //string sERPReleaseDate = "";                                    
            //==================================            
            string sMESID = "0";
            string sMESJobNumber = "0";
            string sPartNo = "", sPartID = "0";
            string sCustID = "0";
            string sPDLine = "";
            string sPDLineID = "0";
            string sVendorCode = "";
            string sRouteID = "0";
            string sPONo = "";
            string sMachineCode = "";
            string sFirstInput = "";
            string sBOMNo = "";            //虛擬料號
            string sVirtualPartNo = "", sVirtualPartID = "0";
            string sWORule = "";
            string sParameItem = "";
            string sWOStatus = ""; 

            bool bUpdate = false;
            bool bInsert = false;
            g_dtSysdate = GetSysDate();
            
            try
            {
                //由Temp Table讀取資料               
                string sCommand = " Select * from " + sTable + " "
                                + " Where DATA_IDX = '" + sDataIndex + "'";
                DataSet dsData = ExecuteSQL(sCommand, null);
                if (dsData.Tables[0].Rows.Count == 0)
                {
                    sMessage = "No Data - Data Index:" + sDataIndex;
                    g_StatusType = StatusType.Error;
                    return;
                }
                sModifyType = dsData.Tables[0].Rows[0]["ERP_FLAG"].ToString();

                sOrgan = dsData.Tables[0].Rows[0]["ORGANIZATION"].ToString();
                sJobNumber = dsData.Tables[0].Rows[0]["JOB_NUMBER"].ToString();
                sJobType = dsData.Tables[0].Rows[0]["JOB_TYPE"].ToString();
                sDepartment = dsData.Tables[0].Rows[0]["DEPARTMENT"].ToString();
              //  sJobStatus = dsData.Tables[0].Rows[0]["JOB_STATUS"].ToString();
                sItemNo = dsData.Tables[0].Rows[0]["ITEM_NO"].ToString().Trim();
                sPlanQty = dsData.Tables[0].Rows[0]["PLAN_QTY"].ToString();
                sPlanStartDate = dsData.Tables[0].Rows[0]["PLAN_START_DATE"].ToString();                
                if (!string.IsNullOrEmpty(sPlanStartDate))
                    dtPlanStartDate = (DateTime)dsData.Tables[0].Rows[0]["PLAN_START_DATE"];
                
                sPlanEndDate = dsData.Tables[0].Rows[0]["PLAN_END_DATE"].ToString();
                if (!string.IsNullOrEmpty(sPlanEndDate))
                    dtPlanEndDate = (DateTime)dsData.Tables[0].Rows[0]["PLAN_END_DATE"];
                sCustCode = dsData.Tables[0].Rows[0]["CUSTOMER_ID"].ToString();
                sPDLine = dsData.Tables[0].Rows[0]["LINE_NAME"].ToString();
                sVendorCode = "WO_Std.xlt";//委外工單時,可填
                sPONo = dsData.Tables[0].Rows[0]["PO_NO"].ToString();//訂單號
                sMachineCode = dsData.Tables[0].Rows[0]["MACHINE_CODE"].ToString();//機台名稱
                sFirstInput = dsData.Tables[0].Rows[0]["FIRST_INPUT"].ToString();//工單首次投產(Y/N)
                sBOMNo = dsData.Tables[0].Rows[0]["BOM_NO"].ToString().Trim();//虛擬料號
                sWOStatus = dsData.Tables[0].Rows[0]["WO_STATUS"].ToString().Trim();//工單狀態
                sWORule = dsData.Tables[0].Rows[0]["WO_RULE"].ToString().Trim();//工單規則
                sMESJobNumber = sJobNumber; //ERP會將工單單別+工單單號串起來給MES
                try
                {
                    //DataRow drID = GetRowData("SAJET.G_WO_BASE", "ERP_ID", sJobID); //以ERP_ID查詢 
                    DataRow drNo = GetRowData("SAJET.G_WO_BASE", "WORK_ORDER", sMESJobNumber);  //以WO查詢 
                    
                    //Part
                    DataRow drPart = GetRowData("SAJET.SYS_PART", "PART_NO", sItemNo);
                    if (drPart == null)
                    {
                        sMessage = "Part not Exist - Item No:" + sItemNo;
                        g_StatusType = StatusType.Error;
                        return;
                    }
                    sPartNo = drPart["PART_NO"].ToString();
                    sPartID = drPart["PART_ID"].ToString();
                    sBOMRev = drPart["VERSION"].ToString();
                    //sWORule = drPart["RULE_SET"].ToString();//工單規則
                    sRouteID = drPart["ROUTE_ID"].ToString();//途程名稱
                    //BOM
                    DataRow drBOM = GetRowData("SAJET.SYS_PART", "PART_NO", sBOMNo);
                    if (drBOM == null)
                    {
                        sMessage = "Virtual Part not Exist - BOM No:" + sBOMNo;
                        g_StatusType = StatusType.Error;
                        return;
                    }
                    sVirtualPartNo = drBOM["PART_NO"].ToString();
                    sVirtualPartID = drBOM["PART_ID"].ToString();
                    //Factory
                    string sFactoryID = GetID("SAJET.SYS_FACTORY", "FACTORY_ID", "FACTORY_CODE", sOrgan);
                    if (sFactoryID == "0")
                    {
                        sSQL = "Select MIN(Factory_ID) Factory_ID From Sajet.SYS_FACTORY ";
                        dsTemp = ExecuteSQL(sSQL, null);
                        if (dsTemp.Tables[0].Rows.Count == 0)
                        {
                            sMessage = "Factory not Exist - Factory Code:" + sOrgan;
                            g_StatusType = StatusType.Error;
                            return;
                        }
                        sFactoryID = dsTemp.Tables[0].Rows[0]["Factory_ID"].ToString();
                    }


                    if (sModifyType == "C" || sModifyType == "U")
                    {
                        if (!string.IsNullOrEmpty(sCustCode))
                        {
                            sCustID = GetID("SAJET.SYS_CUSTOMER", "CUSTOMER_ID", "CUSTOMER_CODE", sCustCode);
                            if (sCustID == "0")
                            {
                                sCustID = GetMaxID("SAJET.SYS_CUSTOMER", "CUSTOMER_ID", 8, ref sMessage);

                                if (sCustID == "0")
                                {
                                    sMessage = "Get Customer MaxID Error";
                                    g_StatusType = StatusType.Error;
                                    return;
                                }
                                object[][] Params = new object[3][];
                                sSQL = " INSERT INTO SAJET.SYS_CUSTOMER "
                                     + " (CUSTOMER_ID,CUSTOMER_CODE, CUSTOMER_NAME ) "
                                     + " VALUES "
                                     + " (:CUSTOMER_ID,:CUSTOMER_CODE, :CUSTOMER_NAME) ";
                                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_ID", sCustID };
                                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_CODE", sCustCode };
                                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_NAME", sCustCode };
                                dsTemp = ExecuteSQL(sSQL, Params);
                                /*
                                sMessage = "Customer not Exist - Customer Code:" + sCustCode;
                                g_StatusType = StatusType.Error;
                                return;
                                 */ 
                            }
                        }
                        /*
                        if (!string.IsNullOrEmpty(sVendorCode))
                        {
                            sVendorID = GetID("SAJET.SYS_VENDOR", "VENDOR_ID", "VENDOR_CODE", sVendorCode);
                            if (sVendorID == "0")
                            {
                                sMessage = "Vendor not Exist - Vendor Code:" + sVendorCode;
                                g_StatusType = StatusType.Error;
                                return;
                            }
                        }
                         */ 
                    }

                    //CHECK PDLINE
                    DataRow drPDLine = GetRowData("SAJET.SYS_PDLINE", "PDLINE_NAME", sPDLine);
                    if (drPDLine == null)
                    {
                        sPDLineID = GetMaxID("SAJET.SYS_PDLINE", "PDLINE_ID", 8, ref sMessage);
                        if (sPDLineID == "0")
                        {
                            sMessage = "Get PDLine MaxID Error";
                            g_StatusType = StatusType.Error;
                            return;
                        }
                        object[][] Params = new object[6][];
                        sSQL = " INSERT INTO SAJET.SYS_PDLINE "
                             + " (FACTORY_ID,PDLINE_ID, PDLINE_NAME ,PDLINE_DESC,UPDATE_USERID,UPDATE_TIME ) "
                             + " VALUES "
                             + " (:FACTORY_ID,:PDLINE_ID, :PDLINE_NAME,:PDLINE_DESC,:UPDATE_USERID, :UPDATE_TIME) ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FACTORY_ID", sFactoryID };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PDLINE_ID", sPDLineID };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PDLINE_NAME", sPDLine };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PDLINE_DESC", sPDLine };
                        Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[5] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        dsTemp = ExecuteSQL(sSQL, Params);
                    }
                    else
                        sPDLineID = drPDLine["PDLINE_ID"].ToString();

                    //Insert====================
                    if (sModifyType == "C")
                    {
                        //MES中是否已有此ERPID
                        if (drNo != null) //資料已存在
                        {
                             int g_iWoStatus = Convert.ToInt32(drNo["WO_STATUS"].ToString());
                             if (g_iWoStatus == 5)
                             {
                                 sMESID = drNo["WORK_ORDER"].ToString();
                                 bUpdate = true;
                             }
                             else
                             {
                                 sMessage = "Data Exist";
                                 g_StatusType = StatusType.Error;
                                 return;
                             }
                        }
                        else
                        {
                            bInsert = true;                            
                        }
                    }
                    //Update=============================
                    else if (sModifyType == "U")
                    {
                        if (drNo == null)
                        {
                            bInsert = true;
                        }
                        else
                        {
                            sMESID = drNo["WORK_ORDER"].ToString();
                            bUpdate = true;                            
                        }
                    }
                    if (bInsert)
                    {
                        object[][] Params = new object[21][];
                        sSQL = " INSERT INTO SAJET.G_WO_BASE "
                             + " (WORK_ORDER, WO_TYPE, WO_STATUS, PART_ID, VERSION, TARGET_QTY "
                             + " ,WO_SCHEDULE_DATE, WO_DUE_DATE,FACTORY_ID,ROUTE_ID,WO_RULE,MASTER_WO "
                             + " ,UPDATE_USERID,UPDATE_TIME,CUSTOMER_ID,PO_NO,DEFAULT_PDLINE_ID,WO_OPTION1,WO_OPTION2,WO_OPTION3,WO_OPTION4 "
                             + "  ) "
                             + " VALUES "
                             + " (:WORK_ORDER, :WO_TYPE, :WO_STATUS, :PART_ID, :VERSION, :TARGET_QTY "
                             + " ,:WO_SCHEDULE_DATE, :WO_DUE_DATE,:FACTORY_ID,:ROUTE_ID,:WO_RULE,:MASTER_WO "
                             + " ,:UPDATE_USERID,:UPDATE_TIME,:CUSTOMER_ID,:PO_NO,:DEFAULT_PDLINE_ID,:WO_OPTION1,:WO_OPTION2,:WO_OPTION3,:WO_OPTION4 "
                             + "  ) ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sMESJobNumber };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WO_TYPE", sJobType };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WO_STATUS", sWOStatus };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sPartID };
                        if (sBOMRev == "")
                            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VERSION", "N/A" };
                        else
                            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VERSION", sBOMRev };
                        Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TARGET_QTY", sPlanQty };
                        if (sPlanStartDate == "")
                            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WO_SCHEDULE_DATE", "" };
                        else
                            Params[6] = new object[] { ParameterDirection.Input, OracleType.DateTime, "WO_SCHEDULE_DATE", dtPlanStartDate };
                        if (sPlanEndDate == "")
                            Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WO_DUE_DATE", "" };
                        else
                            Params[7] = new object[] { ParameterDirection.Input, OracleType.DateTime, "WO_DUE_DATE", dtPlanEndDate };
                        Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FACTORY_ID", sFactoryID };
                        Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ROUTE_ID", sRouteID };
                        Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WO_RULE", sWORule };
                        Params[11] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MASTER_WO", sDepartment };
                        Params[12] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[13] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        Params[14] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_ID", sCustID };
                        Params[15] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PO_NO", sPONo };
                        Params[16] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DEFAULT_PDLINE_ID",sPDLineID };
                        Params[17] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WO_OPTION1", sVendorCode };
                        Params[18] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WO_OPTION2", sVirtualPartID };
                        Params[19] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WO_OPTION3", sMachineCode };
                        Params[20] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WO_OPTION4", sFirstInput };
                        dsTemp = ExecuteSQL(sSQL, Params);

                        sSQL = "UPDATE SAJET.G_WO_BOM "
                             + "  SET  PART_ID ='" + sPartID + "' "
                             + " WHERE WORK_ORDER = '" + sMESJobNumber + "' ";
                        dsTemp = ExecuteSQL(sSQL, null);

                        sMessage = "OK(INSERT)";
                    }
                    else if (bUpdate)
                    {
                        int g_iWoStatus = Convert.ToInt32(drNo["WO_STATUS"].ToString());
                        int iInputQty = Convert.ToInt32(drNo["INPUT_QTY"].ToString());
                        int iReleaseQty = 0;
                        bool bPartChange = false;
                        if (sPartID != drNo["PART_ID"].ToString())
                        {
                            bPartChange = true;
                        }
                        //當Prepare後,料號不可改
                        if (g_iWoStatus >= 1)
                        {
                            //工單料號變更
                            if (sPartID != drNo["PART_ID"].ToString())
                            {
                                //==檢查已展的序號中,是不已經有序號投入第一站了
                                sSQL = "SELECT COUNT(*) QTY FROM SAJET.G_SN_STATUS "
                                    + " WHERE WORK_ORDER =  '" + sMESID + "' "
                                    + "   AND PROCESS_ID > 0 ";
                                dsTemp = ExecuteSQL(sSQL, null);
                                if (iInputQty > 0 || Convert.ToInt32(dsTemp.Tables[0].Rows[0]["QTY"].ToString()) > 0)
                                {
                                    //工單已有投入數量(INPUTQTY)或序號有製程ID不為0的
                                    sMessage = "Part No Can't Change";
                                    g_StatusType = StatusType.Error;
                                    return;
                                }
                            }
                        }
                        //當Release後,Target可多不可少
                        if (g_iWoStatus > 1)
                        {
                            //
                            if (Convert.ToInt32(sPlanQty) < Convert.ToInt32(drNo["TARGET_QTY"].ToString()))
                            {
                                //計算已展的數量 add by rita(若ERP給減量,則檢查工單已投數+已展但未投數)
                                object[][] Params1 = new object[0][];
                                sSQL = " Select Count(*) CNT "
                                     + " From SAJET.G_SN_STATUS "
                                     + " Where WORK_ORDER = '" + sMESID + "' "
                                     + "   AND PROCESS_ID =0 ";
                                dsTemp = ExecuteSQL(sSQL, Params1);
                                if (dsTemp.Tables[0].Rows.Count > 0)
                                {
                                    iReleaseQty = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["CNT"].ToString());
                                }
                                if (iInputQty + iReleaseQty > Convert.ToInt32(sPlanQty)) //MES已展數大於ERP的目標數,則不允許減量
                                {
                                    sMessage = "Input + Release Qty(" + Convert.ToString(iInputQty + iReleaseQty) + ") > Target Qty(" + sPlanQty + ") ,Target Qty can't reduce";
                                    g_StatusType = StatusType.Error;
                                    return;
                                }
                            }
                        }

                        /*
                        //狀態改成Cancel: ERP Status = 7
                        string sWoStatus = g_iWoStatus.ToString();
                         * 
                        if (sJobStatus == "7")
                            sWoStatus = "5";
                         */ 

                        //當原本狀態為Cancel,要變更狀態
                        //if (g_iWoStatus == 5 && sJobStatus != "7")
                        string sWoStatus = g_iWoStatus.ToString();
                        if (g_iWoStatus == 5)
                        {
                            sWoStatus = "0";
                            if (drNo["ROUTE_ID"].ToString() != "0")
                                sWoStatus = "1";
                            if (iInputQty > 0) //有投入數
                                sWoStatus = "3";
                            else
                            {
                                //已展序號
                                sSQL = "SELECT SERIAL_NUMBER FROM SAJET.G_SN_STATUS "
                                     + " WHERE WORK_ORDER =  '" + sMESID + "' "
                                     + "   AND ROWNUM = 1  ";
                                dsTemp = ExecuteSQL(sSQL, null);
                                if (dsTemp.Tables[0].Rows.Count > 0)
                                {
                                    sWoStatus = "2";
                                }
                            }                           
                        }

                        //===add by rita 2010/05/24 當工單版本變更,則同步變更序號主檔內此工單序號的版本
                        string sMESVerion = string.Empty;
                        sSQL = "SELECT VERSION FROM SAJET.G_WO_BASE WHERE WORK_ORDER ='" + sMESID + "' AND ROWNUM = 1 ";
                        dsTemp = ExecuteSQL(sSQL, null);
                        if (dsTemp.Tables[0].Rows.Count > 0)
                        {
                            sMESVerion = dsTemp.Tables[0].Rows[0]["VERSION"].ToString();
                            if (sBOMRev != sMESVerion)
                            {
                                sSQL = "UPDATE SAJET.G_SN_STATUS "
                                    + "   SET VERSION='" + sBOMRev + "' "
                                    + " WHERE WORK_ORDER = '" + sMESID + "' ";
                                dsTemp = ExecuteSQL(sSQL, null);
                            }
                        }
                        //=======add by rita 2011/11/10 當工單料號變更時,同步更改工單發料清單
                        if (bPartChange)
                        {
                            sSQL = "UPDATE SAJET.G_WO_BOM "
                                + "  SET  PART_ID ='" + sPartID + "' "
                                + " WHERE WORK_ORDER = '" + sMESID + "' ";
                            dsTemp = ExecuteSQL(sSQL, null);
                            sSQL="UPDATE SAJET.G_SN_STATUS "
                                + "  SET  PART_ID ='" + sPartID + "' "
                                + " WHERE WORK_ORDER = '" + sMESID + "' ";
                            dsTemp = ExecuteSQL(sSQL, null);
                        }
                        //================================================================================

                        object[][] Params = new object[21][];
                        sSQL = " UPDATE SAJET.G_WO_BASE "
                             + "    SET  WO_TYPE = :WO_TYPE "
                             + "        ,PART_ID = :PART_ID "
                             + "        ,VERSION = :VERSION "
                             + "        ,TARGET_QTY = :TARGET_QTY "
                             + "        ,WO_SCHEDULE_DATE = :WO_SCHEDULE_DATE "
                             + "        ,WO_DUE_DATE = :WO_DUE_DATE "
                             + "        ,FACTORY_ID = :FACTORY_ID "
                             + "        ,ROUTE_ID = :ROUTE_ID "
                             + "        ,WO_RULE = :WO_RULE "
                             + "        ,MASTER_WO = :MASTER_WO "
                             + "        ,WO_STATUS = DECODE(WO_STATUS,'4','4','6','6',:WO_STATUS) "
                             + "        ,UPDATE_USERID = :UPDATE_USERID "
                             + "        ,UPDATE_TIME = :UPDATE_TIME "
                             + "        ,CUSTOMER_ID =:CUSTOMER_ID "
                             + "        ,PO_NO =:PO_NO "
                             + "        ,DEFAULT_PDLINE_ID = :DEFAULT_PDLINE_ID " 
                             + "        ,WO_OPTION1 =:WO_OPTION1 "
                             + "        ,WO_OPTION2 =:WO_OPTION2 "
                             + "        ,WO_OPTION3 =:WO_OPTION3 "
                             + "        ,WO_OPTION4 =:WO_OPTION4 "
                             + "  WHERE WORK_ORDER = :WORK_ORDER ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WO_TYPE", sJobType };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sPartID };
                        if (sBOMRev == "")
                            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VERSION", "N/A" };
                        else
                            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VERSION", sBOMRev };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TARGET_QTY", sPlanQty };
                        if (sPlanStartDate == "")
                            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WO_SCHEDULE_DATE", "" };
                        else
                            Params[4] = new object[] { ParameterDirection.Input, OracleType.DateTime, "WO_SCHEDULE_DATE", dtPlanStartDate };
                        if (sPlanEndDate == "")
                            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WO_DUE_DATE", "" };
                        else
                            Params[5] = new object[] { ParameterDirection.Input, OracleType.DateTime, "WO_DUE_DATE", dtPlanEndDate };
                        Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FACTORY_ID", sFactoryID };
                        Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ROUTE_ID", sRouteID };
                        Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WO_RULE", sWORule };
                        Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MASTER_WO", sDepartment };
                        Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WO_STATUS", sWOStatus };
                        Params[11] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[12] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        Params[13] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_ID", sCustID };
                        Params[14] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PO_NO", sPONo };
                        Params[15] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DEFAULT_PDLINE_ID", sPDLineID };
                        Params[16] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WO_OPTION1", sVendorCode };
                        Params[17] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WO_OPTION2", sVirtualPartID };
                        Params[18] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WO_OPTION3", sMachineCode };
                        Params[19] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WO_OPTION4", sFirstInput };
                        Params[20] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sMESID };
                        dsTemp = ExecuteSQL(sSQL, Params);

                        InsertHTWOBASE(sMESID);

                        //若有更改工單號,必須修改所有有WORK_ORDER的Table
                        if (sMESJobNumber != sMESID)
                        {
                            object[][] ParamsProc = new object[3][];
                            ParamsProc[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TOLDWO", sMESID };
                            ParamsProc[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TNEWWO", sJobNumber };
                            ParamsProc[2] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                            dsTemp = ExecuteProc("SAJET.SP_UPDATE_WONO", ParamsProc);
                            string sRes = dsTemp.Tables[0].Rows[0]["TRES"].ToString();
                            if (sRes != "OK")
                            {
                                sMessage = sRes;
                                g_StatusType = StatusType.Error;
                                return;
                            }
                        }

                        sMessage = "OK(UPDATE)";
                    }

                    if (bInsert || bUpdate)
                    {
                        //WO Rule
                        DataRow drWORule = GetRowData("SAJET.SYS_MODULE_PARAM", "FUNCTION_NAME||PARAME_NAME", sWORule + "RC No Rule");
                        if (drWORule == null)
                        {
                            sMessage = "WO Rule not Exist - WO Rule:" + sWORule;
                            g_StatusType = StatusType.Error;
                            return;
                        }
                        else
                        {
                            sSQL = " DELETE SAJET.G_WO_PARAM "
                                 + "  WHERE WORK_ORDER = '" + sMESJobNumber + "' ";
                            dsTemp = ExecuteSQL(sSQL, null);

                            object[][] Params = new object[4][];
                            sSQL = " INSERT INTO SAJET.G_WO_PARAM (WORK_ORDER,MODULE_NAME,FUNCTION_NAME,PARAME_NAME,PARAME_ITEM,PARAME_VALUE,UPDATE_USERID,UPDATE_TIME) "
                                 + " SELECT :WORK_ORDER,UPPER(C.PARAME_NAME),B.RULE_NAME,A.PARAME_NAME,A.PARAME_ITEM,A.PARAME_VALUE,0,:UPDATE_TIME "
                                 + "   FROM SAJET.SYS_RULE_PARAM A,SAJET.SYS_RULE_NAME B, "
                                 + "        (SELECT PARAME_NAME,PARAME_ITEM "
                                 + "           FROM SAJET.SYS_MODULE_PARAM "
                                 + "          WHERE FUNCTION_NAME = :FUNCTION_NAME "
                                 + "            AND PARAME_ITEM = :PARAME_ITEM) C "
                                 + "  WHERE A.RULE_ID = B.RULE_ID "
                                 + "    AND B.RULE_NAME = C.PARAME_ITEM "
                                 + "    AND B.RULE_NAME = :PARAME_ITEM ";

                            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sMESJobNumber };
                            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FUNCTION_NAME", sWORule };
                            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PARAME_ITEM", drWORule["PARAME_ITEM"].ToString() };
                            Params[3] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                            dsTemp = ExecuteSQL(sSQL, Params);
                        }
                    }

                    //Delete============================
                    if (sModifyType == "D")
                    {
                        if (drNo == null)
                        {
                            sMessage = "Data not Exist";
                            g_StatusType = StatusType.Warning;
                            return;
                        }
                        sMESID = drNo["WORK_ORDER"].ToString();
                        object[][] Params = new object[3][];
                        sSQL = " UPDATE SAJET.G_WO_BASE "
                             + "    SET WO_STATUS = '5' "
                             + "       ,UPDATE_USERID = :UPDATE_USERID "
                             + "       ,UPDATE_TIME = :UPDATE_TIME "
                             + "  WHERE WORK_ORDER = :WORK_ORDER ";
                        
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sMESID };
                        dsTemp = ExecuteSQL(sSQL, Params);

                        InsertHTWOBASE(sMESID);
                        sMessage = "OK(DELETE)";

                    }
                }
                catch (Exception exp)
                {
                    sMessage = exp.Message;
                    g_StatusType = StatusType.Error;
                }
            }
            finally
            {
                LVWo.Items.Add(sMESJobNumber);
                LVWo.Items[LVWo.Items.Count - 1].SubItems.Add(sPartNo);
                LVWo.Items[LVWo.Items.Count - 1].SubItems.Add(sMessage);
                LVWo.Items[LVWo.Items.Count - 1].SubItems.Add(sDataIndex);
                if (sMessage.Length >= 2 && sMessage.Substring(0, 2) != "OK")
                {
                    sMessage = "Job Number[" + sJobNumber + "]" + sMessage;
                }

                TransferFinish(sDataIndex, sMessage, g_StatusType, sTable);
                if (g_StatusType != StatusType.OK)
                {
                    GetColor(LVWo, g_StatusType);
                }
                LabWOCNT.Text = LVWo.Items.Count.ToString();
            }
        }
        private void TransferWoDetail(string sDataIndex, string sTable)
        {            
            string sMessage = "";
            string sModifyType = "";
            g_StatusType = StatusType.OK;
            //ERP上==============================   
            string sOrgan = "";             //Oracle公司別
            string sJobNo = "";       //工單ID
            string sItemNo = "";      //零件料號
            string sQtyPer = "";      //單位用量 
            string sIssueQty = "";//總用量
            string sParentNo = "";//主件料號(該零件料號的主階料號)，若無請填N/A
            string sERPDesignation = "";//插件位置資訊，以 ”;” 隔開，若無請填N/A
            string sItemGroup = "";//替代關係
            
            //==================================            
            string sWo = "";
            string sMESJodID = "";
            string sItemPartNo = "";
            string sPartID = "";
            string sParentPartID="";
            bool bUpdate = false;
            bool bInsert = false;
            
            g_dtSysdate = GetSysDate();

            try
            {
                //由Temp Table讀取資料
                string sCommand = " Select * from " + sTable + " "
                                + " Where DATA_IDX = '" + sDataIndex + "'";
                DataSet dsData = ExecuteSQL(sCommand, null);
                if (dsData.Tables[0].Rows.Count == 0)
                {
                    sMessage = "No Data - Data Index:" + sDataIndex;
                    g_StatusType = StatusType.Error;
                    return;
                }
                sModifyType = dsData.Tables[0].Rows[0]["ERP_FLAG"].ToString();
                sOrgan = dsData.Tables[0].Rows[0]["ORGANIZATION"].ToString();
                sJobNo = dsData.Tables[0].Rows[0]["JOB_NUMBER"].ToString();
                sItemNo = dsData.Tables[0].Rows[0]["COMPONENT_NO"].ToString().Trim();
                sQtyPer = dsData.Tables[0].Rows[0]["QTY_PER"].ToString();
                //sParentNo = dsData.Tables[0].Rows[0]["ITEM_NO"].ToString();
                //sERPDesignation = dsData.Tables[0].Rows[0]["DESIGNATION"].ToString();
                //sItemGroup = dsData.Tables[0].Rows[0]["ALTER_ITEM_NO"].ToString();
                //sIssueQty = dsData.Tables[0].Rows[0]["ISSUE_QTY"].ToString();
                sParentNo = "N/A";
                sItemGroup = "0";
                sERPDesignation = "N/A";
                sMESJodID = sJobNo;
                string[] Unit = new string[0];
                if (sERPDesignation != "N/A")
                {
                    if (sERPDesignation.Contains(";"))
                        Unit = sERPDesignation.Split(";".ToCharArray());
                    else if (sERPDesignation.Contains(","))
                        Unit = sERPDesignation.Split(",".ToCharArray());
                    else
                        Unit = sERPDesignation.Split("".ToCharArray());
                }

                try
                {
                    //WO
                    DataRow drWoData = GetRowData("SAJET.G_WO_BASE", "WORK_ORDER", sMESJodID);
                    string sVersion = "N/A";
                    if (drWoData == null)
                    {
                        //mark by rita 2012/03/02 不檢查工單是否已事先存在,因為凌華Bryant說他們的ERP還滿常物料先轉,再轉工單主檔
                        sMessage = "W/O not Exist - Job Number:" + sMESJodID;
                        g_StatusType = StatusType.Error;
                        return;
                        /*
                        string sTargetQty = "0";
                        sWo = sMESJodID;
                        sPartID = "0";
                         */
                    }
                    else
                    {
                        string sTargetQty = drWoData["TARGET_QTY"].ToString();
                        sWo = drWoData["WORK_ORDER"].ToString();
                        sPartID = drWoData["PART_ID"].ToString();
                        sParentPartID = drWoData["PART_ID"].ToString();
                        sVersion = drWoData["VERSION"].ToString();
                    }
                    string sItemPartID = "";
                    string sProcessID = "0";
                    //ITEM PART
                    DataRow drItemPartData = GetRowData("SAJET.SYS_PART", "PART_NO", sItemNo);
                    if (drItemPartData == null)
                    {
                        sMessage = "Component not Exist - Component No:" + sItemNo;
                        g_StatusType = StatusType.Error;
                        return;
                    }
                    sItemPartNo = drItemPartData["PART_NO"].ToString();
                    sItemPartID = drItemPartData["PART_ID"].ToString();
                    //主階料號,如果有
                    if (!string.IsNullOrEmpty(sParentNo) && sParentNo != "N/A")
                    {
                        DataRow drParentItem = GetRowData("SAJET.SYS_PART", "PART_NO", sParentNo);
                        if (drParentItem == null)
                        {
                            sMessage = "Item not Exist - Item No:" + sParentNo;
                            g_StatusType = StatusType.Error;
                            return;
                        }
                        sParentPartID = drParentItem["PART_ID"].ToString();
                    }

                    if (sModifyType != "D")
                    {
                        /*
                        //ITEM PART
                        DataRow drItemPartData = GetRowData("SAJET.SYS_PART", "PART_NO", sItemNo);
                        if (drItemPartData == null)
                        {
                            sMessage = "Item Part not Exist - Item No:" + sItemNo;
                            g_StatusType = StatusType.Error;
                            return;
                        }
                        sItemPartNo = drItemPartData["PART_NO"].ToString();
                        sItemPartID = drItemPartData["PART_ID"].ToString();
                         */
                        DataRow drID = GetRowData("SAJET.G_WO_BOM", "WORK_ORDER", sWo, "ITEM_PART_ID", sItemPartID);
                        //Insert====================
                        if (sModifyType == "C")
                        {
                            if (drID != null) //資料已存在
                            {
                                sMessage = "Data Exist";
                                g_StatusType = StatusType.Warning;
                                return;
                            }
                            bInsert = true;
                        }
                        //Update=============================
                        else if (sModifyType == "U")
                        {
                            if (drID == null)
                            {
                                /*
                                sMessage = "Data not Exist";
                                g_StatusType = StatusType.Error;
                                return;
                                 */
                                bInsert = true;
                            }
                            else
                            {
                                bUpdate = true;
                                sQtyPer = drID["ITEM_COUNT"].ToString();//記住工單這顆料的用量
                            }
                        }
                        //add by rita 2011/08/25

                        sSQL = "SELECT NVL(B.PROCESS_ID,0) PROCESS_ID ,ITEM_COUNT,ITEM_GROUP "
                             + "  FROM SAJET.SYS_BOM_INFO A ,SAJET.SYS_BOM B "
                             + " WHERE A.PART_ID =:PART_ID AND A.VERSION=:VERSION "
                             + "   AND A.BOM_ID = B.BOM_ID AND B.ITEM_PART_ID =:ITEM_PART_ID ";
                        object[][] Params = new object[3][];
                        //Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sParentPartID };
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sPartID };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VERSION", sVersion };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_PART_ID", sItemPartID };
                        dsTemp = ExecuteSQL(sSQL, Params);
                        if (dsTemp.Tables[0].Rows.Count > 0)
                        {
                            sProcessID = dsTemp.Tables[0].Rows[0]["PROCESS_ID"].ToString();
                            //從SYS_BOM中找單位用量,因為工單發料清單中是總用量,所以不參考
                            //sQtyPer = dsTemp.Tables[0].Rows[0]["ITEM_COUNT"].ToString();
                             sItemGroup = dsTemp.Tables[0].Rows[0]["ITEM_GROUP"].ToString();
                        }
                        /*
                        if (sProcessID == "0" && !String.IsNullOrEmpty(sItemGroup) && sItemGroup != sItemNo)
                        {
                            DataRow drAlterItem = GetRowData("SAJET.SYS_PART", "PART_NO", sItemGroup);
                            if (drAlterItem != null)
                            {
                                string sAlterPartID = drAlterItem["PART_ID"].ToString();
                                sSQL = "SELECT NVL(B.PROCESS_ID,0) PROCESS_ID ,ITEM_COUNT,ITEM_GROUP "
                                       + "  FROM SAJET.SYS_BOM_INFO A ,SAJET.SYS_BOM B "
                                       + " WHERE A.PART_ID =:PART_ID AND A.VERSION=:VERSION "
                                       + "   AND A.BOM_ID = B.BOM_ID AND B.ITEM_PART_ID =:ITEM_PART_ID ";
                                Params = new object[3][];
                                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sParentPartID };
                                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VERSION", sVersion };
                                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_PART_ID", sAlterPartID };
                                dsTemp = ExecuteSQL(sSQL, Params);
                                if (dsTemp.Tables[0].Rows.Count > 0)
                                {
                                    sProcessID = dsTemp.Tables[0].Rows[0]["PROCESS_ID"].ToString();
                                }

                            }
                        }
                         */ 
                    }



                    //sQtyPer = ReturnQtyPer(sQtyPer, sTargetQty);//2011/11/29計算單位用量
                    if (bInsert)
                    {
                        // sQtyPer = "0";//2011/12/13 新增時工單發料數量皆填0,由使用者自己去工單模組內調整
                        object[][] Params = new object[9][];
                        sSQL = " INSERT INTO SAJET.G_WO_BOM "
                             + " (WORK_ORDER, PART_ID, VERSION, ITEM_PART_ID "
                             + " ,ITEM_GROUP ,ITEM_COUNT "
                             + " ,UPDATE_USERID,UPDATE_TIME, PROCESS_ID ) "
                             + " VALUES "
                             + " (:WORK_ORDER, :PART_ID, :VERSION, :ITEM_PART_ID "
                             + " ,:ITEM_GROUP,:ITEM_COUNT "
                             + " ,:UPDATE_USERID, :UPDATE_TIME, :PROCESS_ID ) ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWo };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sPartID };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VERSION", sVersion };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_PART_ID", sItemPartID };
                        Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_GROUP", sItemGroup };
                        Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_COUNT", sQtyPer };
                        Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[7] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", sProcessID };

                        dsTemp = ExecuteSQL(sSQL, Params);

                        sMessage = "OK(INSERT)";
                    }
                    else if (bUpdate)
                    {
                        object[][] Params = new object[7][];
                        sSQL = " UPDATE SAJET.G_WO_BOM "
                             + "    SET ITEM_COUNT = :ITEM_COUNT "
                             + "       ,UPDATE_USERID = :UPDATE_USERID "
                             + "       ,UPDATE_TIME = :UPDATE_TIME  "
                             + "       ,PROCESS_ID = :PROCESS_ID "
                             + "       ,ITEM_GROUP =:ITEM_GROUP "
                             + "  WHERE WORK_ORDER = :WORK_ORDER "
                             + "    AND ITEM_PART_ID = :ITEM_PART_ID ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_COUNT", sQtyPer };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", sProcessID };
                        Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_GROUP", sItemGroup };
                        Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWo };
                        Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_PART_ID", sItemPartID };
                        dsTemp = ExecuteSQL(sSQL, Params);
                        sMessage = "OK(UPDATE)";
                    }

                    //else
                    //{
                    //    object[][] Params = new object[2][];
                    //    sSQL = " Delete SAJET.G_WO_BOM "
                    //         + "  WHERE WORK_ORDER = :WORK_ORDER "
                    //         + "    AND ITEM_PART_ID = :ITEM_PART_ID ";
                    //    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWo };
                    //    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_PART_ID", sItemPartID };
                    //    dsTemp = ExecuteSQL(sSQL, Params);
                    //}


                    //Delete============================
                    if (sModifyType == "D")
                    {
                        //if (drID == null)
                        //{
                        //    sMessage = "Data not Exist";
                        //    g_StatusType = StatusType.Error;
                        //    return;
                        //}
                        //2011/08/25,工單發料清單各筆獨立D
                        object[][] Params = new object[2][];
                        sSQL = " Delete SAJET.G_WO_BOM "
                             + "  WHERE WORK_ORDER = :WORK_ORDER "
                             + "    AND ITEM_PART_ID =:ITEM_PART_ID ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWo };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_PART_ID", sItemPartID };
                        dsTemp = ExecuteSQL(sSQL, Params);
                        sMessage = "OK(DELETE)";
                    }
                    /*
                    object[][] ParamsLoc = new object[2][];
                    sSQL = "DELETE SAJET.G_WO_BOM_LOCATION "
                        + " WHERE WORK_ORDER=:WORK_ORDER "
                        + "  AND ITEM_PART_ID =:ITEM_PART_ID ";
                    ParamsLoc[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWo };
                    ParamsLoc[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_PART_ID", sItemPartID };
                    dsTemp = ExecuteSQL(sSQL, ParamsLoc);

                    if (sModifyType == "C" || sModifyType == "U")
                    {
                        object[][] Params = new object[8][];
                        sSQL = "INSERT INTO  SAJET.G_WO_BOM_LOCATION "
                            + "( WORK_ORDER,PART_ID,ITEM_PART_ID,ITEM_GROUP,VERSION,LOCATION,UPDATE_USERID,UPDATE_TIME) "
                            + " VALUES "
                            + " ( :WORK_ORDER,:PART_ID,:ITEM_PART_ID,:ITEM_GROUP,:VERSION,:LOCATION,:UPDATE_USERID,:UPDATE_TIME) ";
                        for (int i = 0; i <= Unit.Length - 1; i++)
                        {
                            string sLoc = Unit[i];
                            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWo };
                            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sPartID };
                            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_PART_ID", sItemPartID };
                            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_GROUP", sItemGroup };
                            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VERSION", sVersion };
                            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOCATION", sLoc };
                            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                            Params[7] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                            dsTemp = ExecuteSQL(sSQL, Params);
                        }
                    }
                     */ 
                }
                catch (Exception exp)
                {
                    sMessage = exp.Message;
                    g_StatusType = StatusType.Error;
                }
            }
            finally
            {
                LVWoBOM.Items.Add(sWo);
                LVWoBOM.Items[LVWoBOM.Items.Count - 1].SubItems.Add(sItemPartNo);
                LVWoBOM.Items[LVWoBOM.Items.Count - 1].SubItems.Add(sMessage);
                LVWoBOM.Items[LVWoBOM.Items.Count - 1].SubItems.Add(sDataIndex);
                LVWoBOM.Items[LVWoBOM.Items.Count - 1].SubItems.Add(sModifyType);
                if (sMessage.Length >= 2 && sMessage.Substring(0, 2) != "OK")
                {
                    sMessage = "Job Number[" + sJobNo + "]Component No[" + sItemNo + "]" + sMessage;
                }

                TransferFinish(sDataIndex, sMessage, g_StatusType, sTable);
                if (g_StatusType != StatusType.OK)
                {
                    GetColor(LVWoBOM, g_StatusType);
                }
                LabWoBomCNT.Text = LVWoBOM.Items.Count.ToString();
            }
        }
        private string TransferWoMSL(string[] tsData, DateTime dtTime)
        {
            string sMessage = "";
            string sModifyType = tsData[0].ToString();
            string sWo = tsData[1].ToString();
            string sPartID = tsData[2].ToString();
            string sItemPartID = tsData[3].ToString();
            string sQtyPer = tsData[4].ToString();            
            bool bUpdate = false;
            bool bInsert = false;
            g_dtSysdate = dtTime;

            try
            {
                DataRow drMSL = GetRowData("SMT.G_WO_MSL", "WO_SEQUENCE", sWo);
                if (drMSL == null)
                {
                    object[][] Params = new object[5][];
                    sSQL = " INSERT INTO SMT.G_WO_MSL "
                         + " (WO_SEQUENCE, WORK_ORDER ,STATUS,PART_ID,START_TIME) "
                         + " VALUES "
                         + " (:WO_SEQUENCE, :WORK_ORDER ,:STATUS,:PART_ID,:START_TIME) ";
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WO_SEQUENCE", sWo };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", sWo };
                    Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "STATUS", "CREATE" };
                    Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sPartID };
                    Params[4] = new object[] { ParameterDirection.Input, OracleType.DateTime, "START_TIME", g_dtSysdate };
                    dsTemp = ExecuteSQL(sSQL, Params);
                }
                if (sModifyType != "D")
                {
                    DataRow drID = GetRowData("SMT.G_WO_MSL_DETAIL", "WO_SEQUENCE", sWo, "ITEM_PART_ID", sItemPartID);
                    if (drID == null)
                        bInsert = true;
                    else
                        bUpdate = true;
                }
                //Insert====================
                //if (sModifyType == "C")
                //{
                //    if (drID != null) //資料已存在
                //    {
                //        sMessage = "Data Exist";                      
                //        return sMessage;
                //    }

                //    bInsert = true;
                //}
                ////Update=============================
                //else if (sModifyType == "U")
                //{
                //    if (drID == null)
                //    {
                //        sMessage = "Data not Exist";                        
                //        return sMessage;
                //    }
                //    bUpdate = true;
                //}

                if (Convert.ToDouble(sQtyPer) > 0)
                {
                    if (bInsert)
                    {
                        object[][] Params = new object[3][];
                        sSQL = " INSERT INTO SMT.G_WO_MSL_DETAIL "
                             + " (WO_SEQUENCE, ITEM_PART_ID ,ITEM_COUNT) "
                             + " VALUES "
                             + " (:WO_SEQUENCE, :ITEM_PART_ID, :ITEM_COUNT) ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WO_SEQUENCE", sWo };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_PART_ID", sItemPartID };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_COUNT", sQtyPer }; ;
                        dsTemp = ExecuteSQL(sSQL, Params);
                    }
                    else if (bUpdate)
                    {
                        object[][] Params = new object[3][];
                        sSQL = " UPDATE SMT.G_WO_MSL_DETAIL "
                             + "    SET ITEM_COUNT = :ITEM_COUNT "
                             + "  WHERE WO_SEQUENCE = :WO_SEQUENCE "
                             + "    AND ITEM_PART_ID = :ITEM_PART_ID ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_COUNT", sQtyPer };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WO_SEQUENCE", sWo };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_PART_ID", sItemPartID };
                        dsTemp = ExecuteSQL(sSQL, Params);
                    }

                }
                //else
                //{
                //    object[][] Params = new object[2][];
                //    sSQL = " Delete SMT.G_WO_MSL_DETAIL "
                //         + "  WHERE WO_SEQUENCE = :WO_SEQUENCE "
                //         + "    AND ITEM_PART_ID = :ITEM_PART_ID ";
                //    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WO_SEQUENCE", sWo };
                //    Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_PART_ID", sItemPartID };
                //    dsTemp = ExecuteSQL(sSQL, Params);
                //}


                //Delete============================
                if (sModifyType == "D")
                {
                    //if (drID == null)
                    //{
                    //    sMessage = "Data not Exist";
                    //    return sMessage;
                    //}
                    object[][] Params = new object[1][];
                    sSQL = " Delete SMT.G_WO_MSL_DETAIL "
                         + "  WHERE WO_SEQUENCE = :WO_SEQUENCE ";
                         //+ "    AND ITEM_PART_ID = :ITEM_PART_ID ";
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WO_SEQUENCE", sWo };
                    //Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_PART_ID", sItemPartID };
                    dsTemp = ExecuteSQL(sSQL, Params);
                }

                return "OK";
            }
            catch (Exception exp)
            {
                sMessage = exp.Message;
                return sMessage;
            }                     
        }
        private void TransferRTHeader(string sDataIndex, string sTable)
        {            
            string sMessage = "";
            string sModifyType = "";
            g_StatusType = StatusType.OK;
            //ERP上==============================    
            string sOrgan = "";             //Oracle公司別   
            string sVendorCode = "";
            
            string sRTNo = "";
            string sRTType = "";
            //==================================            
            string sMESVendorCode = "";
            string sMESVendorID = "0";
            string sMESID = "0";
            string sMESRTNO = "0";
            string sCreateEmpNo = "";
            string sRTDept = "";
            bool bUpdate = false;
            bool bInsert = false;
            g_dtSysdate = GetSysDate();

            DateTime dtRTCreateDate = DateTime.Now;  //預計投產日
            string sRTCreateDate = "";


            try
            {
                //由Temp Table讀取資料
                string sCommand = " Select * from " + sTable + " "
                                + " Where DATA_IDX = '" + sDataIndex + "'";
                DataSet dsData = ExecuteSQL(sCommand, null);
                if (dsData.Tables[0].Rows.Count == 0)
                {
                    sMessage = "No Data - Data Index:" + sDataIndex;
                    g_StatusType = StatusType.Error;
                    return;
                }
                sOrgan = dsData.Tables[0].Rows[0]["ORGANIZATION"].ToString();
                sModifyType = dsData.Tables[0].Rows[0]["ERP_FLAG"].ToString();
                sVendorCode = dsData.Tables[0].Rows[0]["VENDOR_ID"].ToString().Trim();
                sRTNo = dsData.Tables[0].Rows[0]["RT_NO"].ToString().Trim();
                sRTType = dsData.Tables[0].Rows[0]["RT_TYPE"].ToString().Trim();
                sMESRTNO = sRTNo.Trim();
                try
                {
                    //RT
                    if (string.IsNullOrEmpty(sMESRTNO))
                    {
                        sMessage = "RT No is null";
                        g_StatusType = StatusType.Error;
                        return;
                    }
                    //Vendor
                    DataRow drVendorData = GetRowData("SAJET.SYS_VENDOR", "VENDOR_CODE", sVendorCode);
                    if (drVendorData == null)
                    {
                        /*
                        sMessage = "Vendor not Exist - Vendor ID:" + sVendorID;
                        g_StatusType = StatusType.Error;
                        return;
                         */
                        string sVendorID = GetMaxID("SAJET.SYS_VENDOR", "VENDOR_ID", 8, ref sMessage);

                        if (sVendorID == "0")
                        {
                            sMessage = "Get Vendor MaxID Error";
                            g_StatusType = StatusType.Error;
                            return;
                        }
                        object[][] Params = new object[3][];
                        sSQL = " INSERT INTO SAJET.SYS_VENDOR "
                             + " (VENDOR_ID,VENDOR_CODE, VENDOR_NAME ) "
                             + " VALUES "
                             + " (:VENDOR_ID,:VENDOR_CODE, :VENDOR_NAME) ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_ID", sVendorID };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_CODE", sVendorCode };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_NAME", sVendorCode };
                        dsTemp = ExecuteSQL(sSQL, Params);
                        sMESVendorID = sVendorID;

                    }
                    else
                        //sMESVendorCode = drVendorData["VENDOR_CODE"].ToString();
                        sMESVendorID = drVendorData["VENDOR_ID"].ToString();

                    DataRow drID = GetRowData("SAJET.G_ERP_RTNO", "RT_NO", sMESRTNO);
                    //Insert====================
                    if (sModifyType == "C")
                    {
                        if (drID != null) //資料已存在
                        {
                            if (drID["ENABLED"].ToString() == "Y")
                            {
                                sMessage = "Data Exist";
                                g_StatusType = StatusType.Warning;
                                return;
                            }
                            sMESID = drID["RT_ID"].ToString();
                            bUpdate = true;
                        }
                        else
                        {
                            bInsert = true;
                        }
                    }
                    //Update=============================
                    else if (sModifyType == "U")
                    {
                        if (drID == null)
                        {
                            /*
                            sMessage = "Data not Exist";
                            g_StatusType = StatusType.Error;
                            return;
                             */
                            bInsert = true;
                        }
                        else
                        {
                            sMESID = drID["RT_ID"].ToString();
                            bUpdate = true;
                        }
                    }
                    if (bInsert)
                    {
                        sMESID = GetMaxID("SAJET.G_ERP_RTNO", "RT_ID", 10, ref sMessage);
                        if (sMESID == "0")
                        {
                            sMessage = "Get MaxID Error";
                            g_StatusType = StatusType.Error;
                            return;
                        }

                        object[][] Params = new object[6][];
                        sSQL = " INSERT INTO SAJET.G_ERP_RTNO "
                             + " (RT_ID, RT_NO, VENDOR_ID,UPDATE_USERID,UPDATE_TIME,RT_TYPE ) "
                             + " VALUES "
                             + " (:RT_ID, :RT_NO, :VENDOR_ID,:UPDATE_USERID, :UPDATE_TIME, :RT_TYPE ) ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RT_ID", sMESID };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RT_NO", sMESRTNO };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_ID", sMESVendorID };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[4] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RT_TYPE", sRTType };
                        dsTemp = ExecuteSQL(sSQL, Params);

                        sMessage = "OK(INSERT)";
                    }
                    else if (bUpdate)
                    {
                        object[][] Params = new object[5][];
                        sSQL = " UPDATE SAJET.G_ERP_RTNO "
                             + "    SET VENDOR_ID = :VENDOR_ID "
                             + "       ,RT_TYPE = :RT_TYPE "
                             + "       ,UPDATE_USERID = :UPDATE_USERID "
                             + "       ,UPDATE_TIME = :UPDATE_TIME  "
                             + "       ,ENABLED='Y' "
                             + "  WHERE RT_ID = :RT_ID ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_ID", sMESVendorID };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RT_TYPE", sRTType };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RT_ID", sMESID };
                        dsTemp = ExecuteSQL(sSQL, Params);

                        sMessage = "OK(UPDATE)";
                    }

                    //Delete============================
                    if (sModifyType == "D")
                    {
                        if (drID == null)
                        {
                            sMessage = "Data not Exist";
                            g_StatusType = StatusType.Warning;
                            return;
                        }
                        sMESID = drID["RT_ID"].ToString();
                        /*
                        //若已有Detail資料不可刪除
                        sSQL = " Select * from SAJET.G_ERP_RT_ITEM "
                             + "  WHERE RT_ID = '" + sMESID + "' ";
                        dsTemp = ExecuteSQL(sSQL, null);
                        if (dsTemp.Tables[0].Rows.Count > 0)
                        {
                            sMessage = "RT Detail Data Exist,Can't Delete RT Header";
                            g_StatusType = StatusType.Error;
                            return;
                        }                        
                        object[][] Params = new object[1][];
                        sSQL = " DELETE SAJET.G_ERP_RTNO "
                             + "  WHERE RT_ID = :RT_ID ";                        
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RT_ID", sMESID };                        
                         */
                        object[][] Params = new object[3][];
                        sSQL = " UPDATE SAJET.G_ERP_RTNO "
                             + "    SET ENABLED = 'N' "
                             + "       ,UPDATE_USERID = :UPDATE_USERID "
                             + "       ,UPDATE_TIME = :UPDATE_TIME "
                             + "  WHERE RT_ID = :RT_ID ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RT_ID", sMESID };
                        dsTemp = ExecuteSQL(sSQL, Params);
                        sMessage = "OK(DELETE)";
                    }
                }
                catch (Exception exp)
                {
                    sMessage = exp.Message;
                    g_StatusType = StatusType.Error;
                }
            }
            finally
            {
                LVRTHeader.Items.Add(sMESRTNO);
                LVRTHeader.Items[LVRTHeader.Items.Count - 1].SubItems.Add(sVendorCode);
                LVRTHeader.Items[LVRTHeader.Items.Count - 1].SubItems.Add(sMessage);
                LVRTHeader.Items[LVRTHeader.Items.Count - 1].SubItems.Add(sDataIndex);
                if (sMessage.Length >= 2 && sMessage.Substring(0, 2) != "OK")
                {
                    sMessage = "RT No[" + sRTNo + "]" + sMessage;
                }
                TransferFinish(sDataIndex, sMessage, g_StatusType, sTable);
                if (g_StatusType != StatusType.OK)
                {
                    GetColor(LVRTHeader, g_StatusType);
                }
                LabRTCNT.Text = LVRTHeader.Items.Count.ToString();
            }
        }
        private void TransferRTDetail(string sDataIndex, string sTable)
        {
            string sMessage = "";
            string sModifyType = "";
            g_StatusType = StatusType.OK;
            //ERP上==============================     
            string sOrgan = "";             //Oracle公司別      
            string sRTNo = "";         //收料單號            
            string sRTSeq = "";        //序號(項次)
            string sPONo = "";         //採購單號
            string sPOType = "";       //採購單號類別
            string sPOSeq = "";        //採購單號項次(Line)
            string sItemNo = "";       //零件料號
            string sQty = "";          //收料數量
            string sLocation = "";     //Location
            
            string sRTID = "0";
            string sMESRTNo = "0";
            string sMESPartID = "0", sMESPartNo = "";
            string sLocationID = "0";
            
            bool bUpdate = false;
            bool bInsert = false;
            g_dtSysdate = GetSysDate();

            try
            {
                //由Temp Table讀取資料
                string sCommand = " Select * from " + sTable + " "
                                + " Where DATA_IDX = '" + sDataIndex + "'";
                DataSet dsData = ExecuteSQL(sCommand, null);
                if (dsData.Tables[0].Rows.Count == 0)
                {
                    sMessage = "No Data - Data Index:" + sDataIndex;
                    g_StatusType = StatusType.Error;
                    return;
                }
                sModifyType = dsData.Tables[0].Rows[0]["ERP_FLAG"].ToString();
                sOrgan = dsData.Tables[0].Rows[0]["ORGANIZATION"].ToString();
                sRTNo = dsData.Tables[0].Rows[0]["RT_NO"].ToString().Trim();
                sRTSeq = dsData.Tables[0].Rows[0]["RT_SEQ"].ToString().Trim();
                sPONo = dsData.Tables[0].Rows[0]["PO_NO"].ToString().Trim();
                sPOType = dsData.Tables[0].Rows[0]["PO_TYPE"].ToString().Trim();
                sPOSeq = dsData.Tables[0].Rows[0]["PO_SEQ"].ToString().Trim();
                sItemNo = dsData.Tables[0].Rows[0]["ITEM_NO"].ToString().Trim();
                sQty = dsData.Tables[0].Rows[0]["RECEIVE_QTY"].ToString().Trim();
                sLocation = dsData.Tables[0].Rows[0]["LOCATION"].ToString().Trim();
                sMESRTNo = sRTNo.Trim();
                try
                {
                    //Item ID
                    DataRow drPartData = GetRowData("SAJET.SYS_PART", "PART_NO", sItemNo);
                    if (drPartData == null)
                    {
                        sMessage = "Part not Exist - Item No : " + sItemNo;
                        g_StatusType = StatusType.Error;
                        return;
                    }
                    sMESPartID = drPartData["PART_ID"].ToString();
                    sMESPartNo = drPartData["PART_NO"].ToString();
                    DataRow drLoc = GetRowData("SAJET.SYS_WAREHOUSE", "WAREHOUSE_NAME", sLocation);
                    if (drLoc != null)
                    {
                        sLocationID = drLoc["WAREHOUSE_ID"].ToString();
                    }


                    //RT
                    if (string.IsNullOrEmpty(sMESRTNo))
                    {
                        sMessage = "RT No is null";
                        g_StatusType = StatusType.Error;
                        return;
                    }

                    //RT_ID
                    DataRow drRTHeader = GetRowData("SAJET.G_ERP_RTNO", "RT_NO", sMESRTNo);
                    if (drRTHeader==null)
                    {
                        sMessage = "RT No. not Exist - RT No : " + sMESRTNo;
                        g_StatusType = StatusType.Error;
                        return;
                    }
                    sRTID = drRTHeader["RT_ID"].ToString();
                    string sRTReceiveTime = drRTHeader["RECEIVE_TIME"].ToString();
                    DateTime dtRTReceiveTime = g_dtSysdate; 
                    if (!string.IsNullOrEmpty(sRTReceiveTime))
                        dtRTReceiveTime = (DateTime)drRTHeader["RECEIVE_TIME"];

                    DataRow drID = GetRowData("SAJET.G_ERP_RT_ITEM", "RT_ID", sRTID, "RT_SEQ", sRTSeq);
                    //Insert===========================
                    if (sModifyType == "C")
                    {
                        if (drID != null) //資料已存在
                        {
                            if (drID["ENABLED"].ToString() == "Y")
                            {
                                sMessage = "Data Exist";
                                g_StatusType = StatusType.Warning;
                                return;
                            }
                            bUpdate = true;
                        }
                        else
                        {
                            bInsert = true;
                        }
                    }
                    //Update=============================
                    else if (sModifyType == "U")
                    {
                        if (drID == null)
                        {
                            /*
                            sMessage = "Data not Exist";
                            g_StatusType = StatusType.Error;
                            return;
                             */
                            bInsert = true;
                        }
                        else
                            bUpdate = true;
                    }
                    if (bInsert)
                    {
                        object[][] Params = new object[12][];
                        sSQL = " INSERT INTO SAJET.G_ERP_RT_ITEM "
                             + " (RT_ID, RT_SEQ, PART_ID, PART_VERSION, INCOMING_QTY, UPDATE_USERID, UPDATE_TIME "
                             + " ,PO_NO, PO_SEQ, LOCATION, PO_TYPE ,INCOMING_TIME  "
                             + " ) "
                             + " VALUES "
                             + " (:RT_ID, :RT_SEQ, :PART_ID, :PART_VERSION, :INCOMING_QTY, :UPDATE_USERID, :UPDATE_TIME "
                             + " ,:PO_NO, :PO_SEQ,  :LOCATION, :PO_TYPE,:INCOMING_TIME "
                             + " ) ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RT_ID", sRTID };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RT_SEQ", sRTSeq };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sMESPartID };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_VERSION", "N/A" };
                        Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "INCOMING_QTY", sQty };
                        Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[6] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PO_NO", sPONo };
                        Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PO_SEQ", sPOSeq };
                        Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOCATION", sLocationID };
                        Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PO_TYPE", sPOType };
                        Params[11] = new object[] { ParameterDirection.Input, OracleType.DateTime, "INCOMING_TIME", dtRTReceiveTime };
                        dsTemp = ExecuteSQL(sSQL, Params);
                        sMessage = "OK(INSERT)";
                    }
                    else if (bUpdate)
                    {
                        object[][] Params = new object[11][];
                        sSQL = " UPDATE SAJET.G_ERP_RT_ITEM "
                             + "    SET INCOMING_QTY = :INCOMING_QTY "
                             + "       ,PART_ID = :PART_ID "
                             + "       ,UPDATE_USERID = :UPDATE_USERID "
                             + "       ,UPDATE_TIME = :UPDATE_TIME  "
                             + "       ,PO_NO = :PO_NO "
                             + "       ,PO_SEQ = :PO_SEQ "
                             + "       ,LOCATION = :LOCATION "
                             + "       ,PO_TYPE = :PO_TYPE "
                             + "       ,ENABLED ='Y' "
                             + "       ,INCOMING_TIME =:INCOMING_TIME "
                             + "  WHERE RT_ID = :RT_ID "
                             + "    AND RT_SEQ = :RT_SEQ ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "INCOMING_QTY", sQty };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sMESPartID };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PO_NO", sPONo };
                        Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PO_SEQ", sPOSeq };
                        Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOCATION", sLocationID };
                        Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PO_TYPE", sPOType };
                        Params[8] = new object[] { ParameterDirection.Input, OracleType.DateTime, "INCOMING_TIME", dtRTReceiveTime };
                        Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RT_ID", sRTID };
                        Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RT_SEQ", sRTSeq };
                        dsTemp = ExecuteSQL(sSQL, Params);
                        sMessage = "OK(UPDATE)";
                    }

                    //Delete============================
                    if (sModifyType == "D")
                    {
                        if (drID == null)
                        {
                            sMessage = "Data not Exist";
                            g_StatusType = StatusType.Warning;
                            return;
                        }

                        //若已有IQC檢驗資料不可刪除
                        if (CheckIQCDataExist(sRTID, sRTSeq))
                        {
                            sMessage = "IQC Test Data Exist,Can't Delete RT";
                            g_StatusType = StatusType.Error;
                            return;
                        }

                        /*
                        object[][] Params = new object[2][];
                        sSQL = " DELETE SAJET.G_ERP_RT_ITEM "
                             + "  WHERE RT_ID = :RT_ID "
                             + "    AND RT_SEQ =:RT_SEQ ";
                         * */
                        object[][] Params = new object[4][];
                        sSQL = " UPDATE SAJET.G_ERP_RT_ITEM "
                             + "     SET ENABLED='N' "
                             + "       ,UPDATE_USERID = :UPDATE_USERID "
                             + "       ,UPDATE_TIME = :UPDATE_TIME "
                             + "  WHERE RT_ID = :RT_ID "
                             + "    AND RT_SEQ =:RT_SEQ ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RT_ID", sRTID };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RT_SEQ", sRTSeq };
                        dsTemp = ExecuteSQL(sSQL, Params);
                        sMessage = "OK(DELETE)";
                    }
                }
                catch (Exception exp)
                {
                    sMessage = exp.Message;
                    g_StatusType = StatusType.Error;
                }
            }
            finally
            {
                LVRTDetail.Items.Add(sMESRTNo);
                LVRTDetail.Items[LVRTDetail.Items.Count - 1].SubItems.Add(sRTSeq);
                LVRTDetail.Items[LVRTDetail.Items.Count - 1].SubItems.Add(sMessage);
                LVRTDetail.Items[LVRTDetail.Items.Count - 1].SubItems.Add(sDataIndex);
                if (sMessage.Length >= 2 && sMessage.Substring(0, 2) != "OK")
                {
                    sMessage = "RT No[" + sRTNo + "]RT Seq[" + sRTSeq + "]" + sMessage;
                }
                TransferFinish(sDataIndex, sMessage, g_StatusType, sTable);
                if (g_StatusType != StatusType.OK)
                {
                    GetColor(LVRTDetail, g_StatusType);
                }
                LabRTDetailCNT.Text = LVRTDetail.Items.Count.ToString();
            }
        }
     
        private bool InsertBOMHeader(ref string sBOMID, string sPartID, string sERPBomRev,ref string sMessage)
        {
            sBOMID = GetMaxID("SAJET.SYS_BOM_INFO", "BOM_ID", 8, ref sMessage);
            if (sBOMID == "0")
            {
                sMessage = "Get MaxID Error";
                g_StatusType = StatusType.Error;
                return false;
            }
            object[][] Params = new object[5][];
            sSQL = " INSERT INTO SAJET.SYS_BOM_INFO "
                 + " (BOM_ID, PART_ID, VERSION, UPDATE_USERID, UPDATE_TIME) "
                 + " VALUES "
                 + " (:BOM_ID, :PART_ID, :VERSION, :UPDATE_USERID, :UPDATE_TIME) ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BOM_ID", sBOMID };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sPartID };
            if (sERPBomRev == "")
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VERSION", "N/A" };
            else
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VERSION", sERPBomRev };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
            dsTemp = ExecuteSQL(sSQL, Params);
            return true;
        }

        private void TransferBOMDetail(string sDataIndex, string sTable)
        {
            string sMessage = "";
            string sModifyType = "";
            g_StatusType = StatusType.OK;
            //ERP上==============================            
            string sOrgan = "";             //Oracle公司別  
            string sERPItemNo = "";   //產品料號ID 
            string sERPComponemtNo = ""; //零件料號ID
            string sERPQtyPer = "";   //單位用量  
            string sERPDesignation = ""; //插件位置       
            string sERPBomRev = "N/A";
            string sJobExtend = "";
            //==================================            
            string sBOMID = "0";
            bool bUpdate = false;
            bool bInsert = false;
            string sPartID = "0", sPartNo = "";
            string sItemPartID = "0", sItemPartNo = "";
            bool bChangeBOM = false;

            g_dtSysdate = GetSysDate();



            try
            {
                //由Temp Table讀取資料
                string sCommand = " Select * from " + sTable + " "
                                + " Where DATA_IDX = '" + sDataIndex + "'";
                DataSet dsData = ExecuteSQL(sCommand, null);

                if (dsData.Tables[0].Rows.Count == 0)
                {
                    sMessage = "No Data - Data Index:" + sDataIndex;
                    g_StatusType = StatusType.Error;
                    return;
                }
                sModifyType = dsData.Tables[0].Rows[0]["ERP_FLAG"].ToString();
                sOrgan = dsData.Tables[0].Rows[0]["ORGANIZATION"].ToString();
                sERPItemNo = dsData.Tables[0].Rows[0]["ITEM_NO"].ToString().Trim();
                sERPComponemtNo = dsData.Tables[0].Rows[0]["COMPONENT_NO"].ToString().Trim();
                sERPQtyPer = dsData.Tables[0].Rows[0]["QTY_PER"].ToString().Trim();
                //sJobExtend = dsData.Tables[0].Rows[0]["JOB_EXTEND"].ToString().Trim();//用來判斷是否為虛擬階'3'要往下展
                /*
                string[] Unit;
                if (sERPDesignation.Contains(";"))
                    Unit = sERPDesignation.Split(";".ToCharArray());
                else if (sERPDesignation.Contains(","))
                    Unit = sERPDesignation.Split(",".ToCharArray());
                else
                    Unit = sERPDesignation.Split("".ToCharArray());
                 */ 

                try
                {
                    //Part

                    DataRow drPartData = GetRowData("SAJET.SYS_PART", "PART_NO", sERPItemNo);
                    if (drPartData == null)
                    {

                        sMessage = "Item not Exist - Item No:" + sERPItemNo;
                        g_StatusType = StatusType.Error;
                        return;

                        /*
                        
                        sPartID = GetMaxID("SAJET.SYS_PART", "PART_ID", 10, ref sMessage);
                        if (sPartID == "0")
                        {
                            sMessage = "Get MaxID Error";
                            g_StatusType = StatusType.Error;
                            return;
                        }

                        object[][] Params = new object[2][];
                        sSQL = " INSERT INTO SAJET.SYS_PART "
                             + " (PART_ID, PART_NO ) "
                             + " VALUES "
                             + " (:PART_ID, :PART_NO ) ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sPartID };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_NO", sERPItemNo };
                        dsTemp = ExecuteSQL(sSQL, Params);
                        sPartNo = sERPItemNo;
                         */

                    }
                    else
                    {
                        sPartID = drPartData["PART_ID"].ToString();
                        sPartNo = drPartData["PART_NO"].ToString();
                    }

                    //Item Part
                    drPartData = GetRowData("SAJET.SYS_PART", "PART_NO", sERPComponemtNo);
                    if (drPartData == null)
                    {

                        sMessage = "Component not Exist - Component No:" + sERPComponemtNo;
                        g_StatusType = StatusType.Error;
                        return;

                        /*
                        sItemPartID = GetMaxID("SAJET.SYS_PART", "PART_ID", 10, ref sMessage);
                        if (sItemPartID == "0")
                        {
                            sMessage = "Get MaxID Error";
                            g_StatusType = StatusType.Error;
                            return;
                        }

                        object[][] Params = new object[2][];
                        sSQL = " INSERT INTO SAJET.SYS_PART "
                             + " (PART_ID, PART_NO ) "
                             + " VALUES "
                             + " (:PART_ID, :PART_NO ) ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sItemPartID };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_NO", sERPComponemtNo };
                        dsTemp = ExecuteSQL(sSQL, Params);
                         */

                    }
                    else
                    {
                        sItemPartID = drPartData["PART_ID"].ToString();
                        sItemPartNo = drPartData["PART_NO"].ToString();
                    }
                    DataRow drBomID = GetBOMID(sPartID, sERPBomRev);
                    if (drBomID == null) //BOM主檔不存在
                    {
                        if (!InsertBOMHeader(ref sBOMID, sPartID, sERPBomRev, ref sMessage))
                            return;
                    }
                    else
                    {
                        sBOMID = drBomID["BOM_ID"].ToString();
                    }
                    DataRow drNo = GetRowData("SAJET.SYS_BOM", "BOM_ID", sBOMID, "ITEM_PART_ID", sItemPartID);
                    //Insert====================
                    if (sModifyType == "C")
                    {
                        if (drNo != null) //資料已存在
                        {
                            sMessage = "Data Exist";
                            g_StatusType = StatusType.Warning;
                            return;
                        }
                        else
                        {
                            bInsert = true;
                        }
                    }
                    //Update=============================
                    else if (sModifyType == "U")
                    {
                        if (drNo == null)
                        {
                            /*
                            sMessage = "Data not Exist";
                            g_StatusType = StatusType.Error;
                            return;
                             */
                            bInsert = true;
                        }
                        else
                        {
                            bUpdate = true;
                        }
                    }
                    if (bInsert)
                    {
                        object[][] Params = new object[0][];
                        if (drNo == null)
                        {
                            Params = new object[8][];
                            sSQL = " INSERT INTO SAJET.SYS_BOM "
                                 + " (BOM_ID, ITEM_PART_ID, ITEM_GROUP, PROCESS_ID,ITEM_COUNT "
                                 + " , VERSION, UPDATE_USERID, UPDATE_TIME,PRIMARY_FLAG ) "
                                 + " VALUES "
                                 + " (:BOM_ID, :ITEM_PART_ID, :ITEM_GROUP, :PROCESS_ID,:ITEM_COUNT "
                                 + " , :VERSION, :UPDATE_USERID, :UPDATE_TIME,'Y' ) ";
                            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BOM_ID", sBOMID };
                            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_PART_ID", sItemPartID };
                            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_GROUP", "0" };
                            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", "0" };
                            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_COUNT", sERPQtyPer };
                            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VERSION", "N/A" };
                            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                            Params[7] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
//                            Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "JOB_EXTEND", sJobExtend };
                            dsTemp = ExecuteSQL(sSQL, Params);
                        }
                        sMessage = "OK(INSERT)";
                    }
                    else if (bUpdate)
                    {
                        object[][] Params = new object[5][];
                        sSQL = " UPDATE SAJET.SYS_BOM "
                             + "    SET  ITEM_COUNT = :ITEM_COUNT "
                             + "        ,UPDATE_USERID = :UPDATE_USERID "
                             + "        ,UPDATE_TIME = :UPDATE_TIME  "
                             + "        ,ENABLED='Y' "
                             + "        ,PRIMARY_FLAG='Y' "
                             + "  WHERE BOM_ID = :BOM_ID "
                             + "  AND ITEM_PART_ID = :ITEM_PART_ID ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_COUNT", sERPQtyPer };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BOM_ID", sBOMID };
                        Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_PART_ID", sItemPartID };
                        dsTemp = ExecuteSQL(sSQL, Params);
                        sMessage = "OK(UPDATE)";
                    }

                    //Delete============================
                    if (sModifyType == "D")
                    {

                        //if (drNo == null)
                        //{
                        //    sMessage = "Data not Exist";
                        //    g_StatusType = StatusType.Error;
                        //    return;
                        //}                      
                        object[][] Params = new object[2][];
                        sSQL = "DELETE SAJET.SYS_BOM "
                             + "  WHERE BOM_ID = :BOM_ID "
                             + "    AND ITEM_PART_ID =:ITEM_PART_ID ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BOM_ID", sBOMID };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_PART_ID", sItemPartID };
                        dsTemp = ExecuteSQL(sSQL, Params);
                        /*
                        sSQL = " DELETE SAJET.SYS_BOM_LOCATION "
                             + "  WHERE BOM_ID = :BOM_ID "
                             + "    AND ITEM_PART_ID =:ITEM_PART_ID ";
                        dsTemp = ExecuteSQL(sSQL, Params);
                         */
                        sMessage = "OK(DELETE)";
                    }
                    //FOR 公信,如果有另一個成品bom用到這個成品的料號且為虛擬階
                   // updateSYSBOMINFO(sPartID);

                }
                catch (Exception exp)
                {
                    sMessage = exp.Message;
                    g_StatusType = StatusType.Error;
                }
            }
            finally
            {
                LVBOMDetail.Items.Add(sPartNo);
                LVBOMDetail.Items[LVBOMDetail.Items.Count - 1].SubItems.Add(sItemPartNo);
                LVBOMDetail.Items[LVBOMDetail.Items.Count - 1].SubItems.Add(sMessage);
                LVBOMDetail.Items[LVBOMDetail.Items.Count - 1].SubItems.Add(sDataIndex);
                if (sMessage.Length >= 2 && sMessage.Substring(0, 2) != "OK")
                {
                    sMessage = "Item No[" + sERPItemNo + "]Component No[" + sERPComponemtNo + "]" + sMessage;
                }
                TransferFinish(sDataIndex, sMessage, g_StatusType, sTable);
                if (g_StatusType != StatusType.OK)
                {
                    GetColor(LVBOMDetail, g_StatusType);
                }
                LabBOMDetailCNT.Text = LVBOMDetail.Items.Count.ToString();
            }
        }
        private void TransferBOMLocation(string sDataIndex, string sTable)
        {
            string sMessage = "";
            string sModifyType = "";
            g_StatusType = StatusType.OK;
            //ERP上==============================                     
            string sERPItemNo = "";   //產品料號ID 
            string sERPBomRev = "N/A";//BOM版本
            string sERPComponemtNo= ""; //零件料號ID
            string sERPDesign = "";   //插件位置 
            //==================================            
            string sBOMID = "0";
            bool bUpdate = false;
            bool bInsert = false;
            string sPartID = "0", sPartNo = "";
            string sItemPartID = "0", sItemPartNo = "";
            g_dtSysdate = GetSysDate();

            try
            {
                //由Temp Table讀取資料
                string sCommand = " Select * from " + sTable + " "
                                + " Where DATA_IDX = '" + sDataIndex + "'";
                DataSet dsData = ExecuteSQL(sCommand, null);
                if (dsData.Tables[0].Rows.Count == 0)
                {
                    sMessage = "No Data - Data Index:" + sDataIndex;
                    g_StatusType = StatusType.Error;
                    return;
                }
                sModifyType = dsData.Tables[0].Rows[0]["ERP_FLAG"].ToString();
                sERPItemNo = dsData.Tables[0].Rows[0]["ITEM_NO"].ToString();
                sERPComponemtNo = dsData.Tables[0].Rows[0]["COMPONENT_NO"].ToString();
                sERPDesign = dsData.Tables[0].Rows[0]["DESIGNATION"].ToString();
                try
                {
                    //Part
                    DataRow drPartData = GetRowData("SAJET.SYS_PART", "PART_NO", sERPItemNo);
                    if (drPartData == null)
                    {
                        sMessage = "Item not Exist - Item No:" + sERPItemNo;
                        g_StatusType = StatusType.Error;
                        return;
                    }
                    sPartID = drPartData["PART_ID"].ToString();
                    sPartNo = drPartData["PART_NO"].ToString();
                    //Item Part
                    drPartData = GetRowData("SAJET.SYS_PART", "PART_NO", sERPComponemtNo);
                    if (drPartData == null)
                    {
                        sMessage = "Component not Exist - Component No:" + sERPComponemtNo;
                        g_StatusType = StatusType.Error;
                        return;
                    }
                    sItemPartID = drPartData["PART_ID"].ToString();
                    sItemPartNo = drPartData["PART_NO"].ToString();
                    //BOM_ID
                    DataRow drBomID = GetBOMID(sPartID, sERPBomRev);
                    if (drBomID == null) //BOM主檔不存在
                    {
                        sMessage = "BOM Header Data not Exist";
                        g_StatusType = StatusType.Error;
                        return;
                    }
                    sBOMID = drBomID["BOM_ID"].ToString();
                    //SYS_BOM
                    string sItemGroup = "";
                    string sVersion = "N/A";
                    if (sModifyType == "C" || sModifyType == "U")
                    {
                        DataRow drBomData = GetRowData("SAJET.SYS_BOM", "BOM_ID", sBOMID, "ITEM_PART_ID", sItemPartID);
                        if (drBomData == null) //BOM主檔不存在
                        {
                            sMessage = "BOM Detail Data not Exist-Item No:" + sERPItemNo + ",Component:" + sERPComponemtNo;
                            g_StatusType = StatusType.Error;
                            return;
                        }
                        sItemGroup = drBomData["ITEM_GROUP"].ToString();
                        sVersion = drBomData["VERSION"].ToString();
                    }
                    DataRow drNo = null;
                    sSQL = " Select * from SAJET.SYS_BOM_LOCATION "
                         + " where BOM_ID = '" + sBOMID + "' "
                         + " and ITEM_PART_ID = '" + sItemPartID + "' "
                         + " and Location = '" + sERPDesign + "' ";
                    dsTemp = ExecuteSQL(sSQL, null);
                    if (dsTemp.Tables[0].Rows.Count > 0)
                    {
                        drNo = dsTemp.Tables[0].Rows[0];
                    }
                    //Insert====================
                    if (sModifyType == "C")
                    {
                        if (drNo != null) //資料已存在
                        {
                            sMessage = "Data Exist";
                            g_StatusType = StatusType.Warning;
                            return;
                        }
                        bInsert = true;
                    }
                    //Update=============================
                    else if (sModifyType == "U")
                    {
                        if (drNo == null)
                        {
                            sMessage = "Data not Exist";
                            g_StatusType = StatusType.Error;
                            return;
                        }
                        bUpdate = true;
                    }
                    if (bInsert)
                    {
                        object[][] Params = new object[7][];
                        sSQL = " INSERT INTO SAJET.SYS_BOM_LOCATION "
                             + " (BOM_ID, ITEM_PART_ID, ITEM_GROUP, VERSION "
                             + " ,LOCATION, UPDATE_USERID, UPDATE_TIME) "
                             + " VALUES "
                             + " (:BOM_ID, :ITEM_PART_ID, :ITEM_GROUP, :VERSION "
                             + " ,:LOCATION, :UPDATE_USERID, :UPDATE_TIME) ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BOM_ID", sBOMID };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_PART_ID", sItemPartID };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_GROUP", sItemGroup };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VERSION", sVersion };
                        Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOCATION", sERPDesign };                        
                        Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[6] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        dsTemp = ExecuteSQL(sSQL, Params);

                        sMessage = "OK(INSERT)";
                    }
                    else if (bUpdate)
                    {
                        //做Update無太大意義
                        object[][] Params = new object[5][];
                        sSQL = " UPDATE SAJET.SYS_BOM_LOCATION "
                             + "    SET  UPDATE_USERID = :UPDATE_USERID "
                             + "        ,UPDATE_TIME = :UPDATE_TIME  "
                             + "  WHERE BOM_ID = :BOM_ID "
                             + "  AND ITEM_PART_ID = :ITEM_PART_ID "
                             + "  AND LOCATION = :LOCATION ";                       
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BOM_ID", sBOMID };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_PART_ID", sItemPartID };
                        Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOCATION", sERPDesign };
                        dsTemp = ExecuteSQL(sSQL, Params);

                        sMessage = "OK(UPDATE)";
                    }

                    //Delete============================
                    if (sModifyType == "D")
                    {
                        if (drNo == null)
                        {
                            sMessage = "Data not Exist-Item No:" + sERPItemNo + ",Component:" + sERPComponemtNo + ",Loc:" + sERPDesign;
                            g_StatusType = StatusType.Warning;
                            return;
                        }
                        object[][] Params = new object[3][];
                        sSQL = " DELETE SAJET.SYS_BOM_LOCATION "                             
                             + "  WHERE BOM_ID = :BOM_ID "
                             + "  AND ITEM_PART_ID = :ITEM_PART_ID "
                             + "  AND LOCATION = :LOCATION ";                        
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BOM_ID", sBOMID };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_PART_ID", sItemPartID };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOCATION", sERPDesign };
                        dsTemp = ExecuteSQL(sSQL, Params);

                        sMessage = "OK(DELETE)";
                    }
                    //虛擬階異動時,使用虛擬階的來源BOM ,FLAG要調整
                    //updateSYSBOMINFO(sPartID);
                }
                catch (Exception exp)
                {
                    sMessage = exp.Message;
                    g_StatusType = StatusType.Error;
                }
            }
            finally
            {
                LVBOMLoc.Items.Add(sPartNo);
                LVBOMLoc.Items[LVBOMLoc.Items.Count - 1].SubItems.Add(sItemPartNo);
                LVBOMLoc.Items[LVBOMLoc.Items.Count - 1].SubItems.Add(sERPDesign);
                LVBOMLoc.Items[LVBOMLoc.Items.Count - 1].SubItems.Add(sMessage);
                LVBOMLoc.Items[LVBOMLoc.Items.Count - 1].SubItems.Add(sDataIndex);
                if (sMessage.Length >= 2 && sMessage.Substring(0, 2) != "OK")
                {
                    sMessage = "Item No[" + sERPItemNo + "]Component No[" + sERPComponemtNo + "]" + sMessage;
                }
                TransferFinish(sDataIndex, sMessage, g_StatusType, sTable);
                if (g_StatusType != StatusType.OK)
                {
                    GetColor(LVBOMLoc, g_StatusType);
                }
                LabBOMLocCNT.Text = LVBOMLoc.Items.Count.ToString();
            }
        }
        private string GetMaxItemGroup()
        {
            try
            {
                sSQL = "SELECT NVL(MAX(TO_NUMBER(ITEM_GROUP)),0) + 1 ITEM_GROUP FROM SAJET.SYS_BOM ";
                dsTemp = ExecuteSQL(sSQL, null);
                return dsTemp.Tables[0].Rows[0]["ITEM_GROUP"].ToString();
            }
            catch (Exception exp)
            {
                return "0";
            }
        }
        private void TransferBOMAlternative(string sDataIndex, string sTable)
        {
            string sMessage = "";
            string sModifyType = "";
            g_StatusType = StatusType.OK;
            //ERP上==============================                     
            string sOrgan = "";             //Oracle公司別   
            string sERPItemNo = "";   //產品料號ID 
            string sERPBomRev = "";   //BOM版本
            //string sERPItemSeq = "";
            string sERPMasterItemNo = ""; //主要料號ID
            string sERPAlterItemNo = "";  //替代料號ID  
            //string sERPBomType = "";  //BOM分類                        
            //==================================            
            string sBOMID = "0";
            bool bUpdate = false;
            bool bInsert = false;
            string sPartID = "0", sPartNo = "";
            string sItemPartID = "0", sItemPartNo = "";
            string sAlterPartID = "0", sAlterPartNo = "";
            g_dtSysdate = GetSysDate();

            try
            {
                //由Temp Table讀取資料
                string sCommand = " Select * from " + sTable + " "
                                + " Where DATA_IDX = '" + sDataIndex + "'";
                DataSet dsData = ExecuteSQL(sCommand, null);
                if (dsData.Tables[0].Rows.Count == 0)
                {
                    sMessage = "No Data - Data Index:" + sDataIndex;
                    g_StatusType = StatusType.Error;
                    return;
                }
                sModifyType = dsData.Tables[0].Rows[0]["ERP_FLAG"].ToString();
                sOrgan = dsData.Tables[0].Rows[0]["ORGANIZATION"].ToString();
                sERPItemNo = dsData.Tables[0].Rows[0]["ITEM_NO"].ToString().Trim();
                sERPBomRev = "N/A";
                sERPMasterItemNo = dsData.Tables[0].Rows[0]["MASTER_ITEM_NO"].ToString().Trim();
                sERPAlterItemNo = dsData.Tables[0].Rows[0]["ALTER_ITEM_NO"].ToString().Trim();

                try
                {
                    DataRow drPartData = GetRowData("SAJET.SYS_PART", "PART_NO", sERPItemNo);
                    if (drPartData == null)
                    {                        
                        if (sERPItemNo == "*")
                        {
                            sPartID = GetMaxID("SAJET.SYS_PART", "PART_ID", 10, ref sMessage);
                            if (sPartID == "0")
                            {
                                sMessage = "Get MaxID Error";
                                g_StatusType = StatusType.Error;
                                return;
                            }
                            object[][] Params = new object[2][];
                            sSQL = " INSERT INTO SAJET.SYS_PART "
                                 + " (PART_ID, PART_NO ) "
                                 + " VALUES "
                                 + " (:PART_ID, :PART_NO ) ";
                            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sPartID };
                            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_NO", sERPItemNo };
                            dsTemp = ExecuteSQL(sSQL, Params);
                            sPartNo = sERPItemNo;
                        }
                        else
                        {
                            sMessage = "Item not Exist - Item No:" + sERPItemNo;
                            g_StatusType = StatusType.Error;
                            return;
                        }
                         
 
                    }
                    else
                    {
                        sPartID = drPartData["PART_ID"].ToString();
                        sPartNo = drPartData["PART_NO"].ToString();
                    }

                    //Item Part
                    drPartData = GetRowData("SAJET.SYS_PART", "PART_NO", sERPMasterItemNo);
                    if (drPartData == null)
                    {
                        sMessage = "Master not Exist - Master Item No:" + sERPMasterItemNo;
                        g_StatusType = StatusType.Error;
                        return;
                    }
                    sItemPartID = drPartData["PART_ID"].ToString();
                    sItemPartNo = drPartData["PART_NO"].ToString();
                    //可替代料
                    drPartData = GetRowData("SAJET.SYS_PART", "PART_NO", sERPAlterItemNo);
                    if (drPartData == null)
                    {
                        sMessage = "Alter not Exist - Alter Item No:" + sERPAlterItemNo;
                        g_StatusType = StatusType.Error;
                        return;
                    }
                    sAlterPartID = drPartData["PART_ID"].ToString();
                    sAlterPartNo = drPartData["PART_NO"].ToString();

                    DataRow drBomID = GetBOMID(sPartID, sERPBomRev);
                    if (drBomID == null) //BOM主檔不存在
                    {
                        if (!InsertBOMHeader(ref sBOMID, sPartID, sERPBomRev, ref sMessage))
                            return;
                    }
                    else
                    {
                        sBOMID = drBomID["BOM_ID"].ToString();
                    }

                    //主要料號資料
                    string sItemGroup = "";
                    string sItemCount = "";
                    string sProcessID = "";
                    DataRow drBomDataMaster = GetRowData("SAJET.SYS_BOM", "BOM_ID", sBOMID, "ITEM_PART_ID", sItemPartID);
                    if (drBomDataMaster == null) //BOM主檔不存在
                    {
                        if (sERPItemNo == "*")
                        {
                            sItemGroup = GetMaxItemGroup();
                            if (sItemGroup == "0")
                            {
                                sMessage = "Get Item Group Error";
                                g_StatusType = StatusType.Error;
                                return;
                            }
                            object[][] Params = new object[8][];
                            sSQL = " INSERT INTO SAJET.SYS_BOM "
                                 + " (BOM_ID, ITEM_PART_ID, ITEM_GROUP, PROCESS_ID,ITEM_COUNT "
                                 + " , VERSION, UPDATE_USERID, UPDATE_TIME,PRIMARY_FLAG ) "
                                 + " VALUES "
                                 + " (:BOM_ID, :ITEM_PART_ID, :ITEM_GROUP, :PROCESS_ID,:ITEM_COUNT "
                                 + " , :VERSION, :UPDATE_USERID, :UPDATE_TIME,'Y' ) ";
                            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BOM_ID", sBOMID };
                            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_PART_ID", sItemPartID };
                            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_GROUP", sItemGroup };
                            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", "0" };
                            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_COUNT", "0" };
                            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VERSION", "N/A" };
                            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                            Params[7] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                            dsTemp = ExecuteSQL(sSQL, Params);
                            sItemCount = "0";
                            sProcessID = "0";
                        }
                        else
                        {

                            sMessage = "Master Data not Exist - Master Item No:" + sItemPartNo;
                            g_StatusType = StatusType.Error;
                            return;
                        }
                    }
                    else
                    {
                        sItemGroup = drBomDataMaster["ITEM_GROUP"].ToString();
                        sItemCount = drBomDataMaster["ITEM_COUNT"].ToString();
                        sProcessID = drBomDataMaster["PROCESS_ID"].ToString();
                    }
                    //賦予Item Group
                    if (sItemGroup == "0")
                    {
                        // sItemGroup = GetMaxID("SAJET.SYS_BOM", "ITEM_GROUP", 5, ref sMessage);
                        sItemGroup = GetMaxItemGroup();
                        if (sItemGroup == "0")
                        {
                            sMessage = "Get Item Group Error";
                            g_StatusType = StatusType.Error;
                            return;
                        }

                        //修改原本主要料號的ITEM_GROUP
                        object[][] Params = new object[3][];
                        sSQL = " UPDATE SAJET.SYS_BOM "
                             + "    SET ITEM_GROUP = :ITEM_GROUP "
                             + "  WHERE BOM_ID = :BOM_ID "
                             + "    AND ITEM_PART_ID = :ITEM_PART_ID ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_GROUP", sItemGroup };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BOM_ID", sBOMID };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_PART_ID", sItemPartID };
                        dsTemp = ExecuteSQL(sSQL, Params);
                    }
                    DataRow drNo = GetRowData("SAJET.SYS_BOM", "BOM_ID", sBOMID, "ITEM_PART_ID", sAlterPartID);
                    //Insert====================
                    if (sModifyType == "C")
                    {
                        if (drNo != null) //資料已存在
                        {
                            sMessage = "Data Exist";
                            g_StatusType = StatusType.Warning;
                            return;
                        }
                        bInsert = true;
                    }
                    //Update=============================
                    else if (sModifyType == "U")
                    {
                        if (drNo == null)
                        {
                            sMessage = "Data not Exist";
                            g_StatusType = StatusType.Error;
                            return;
                        }
                        bUpdate = true;
                    }
                    if (bInsert)
                    {
                        object[][] Params = new object[8][];
                        sSQL = " INSERT INTO SAJET.SYS_BOM "
                             + " (BOM_ID, ITEM_PART_ID, ITEM_GROUP, ITEM_COUNT,PROCESS_ID "
                             + " , VERSION, UPDATE_USERID, UPDATE_TIME,PRIMARY_FLAG ) "
                             + " VALUES "
                             + " (:BOM_ID, :ITEM_PART_ID, :ITEM_GROUP, :ITEM_COUNT,:PROCESS_ID "
                             + " , :VERSION, :UPDATE_USERID, :UPDATE_TIME,'N' ) ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BOM_ID", sBOMID };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_PART_ID", sAlterPartID };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_GROUP", sItemGroup };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_COUNT", sItemCount };
                        Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", sProcessID };
                        Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VERSION", "N/A" };
                        Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[7] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        dsTemp = ExecuteSQL(sSQL, Params);
                        sMessage = "OK(INSERT)";
                    }
                    else if (bUpdate)
                    {
                        object[][] Params = new object[7][];
                        sSQL = " UPDATE SAJET.SYS_BOM "
                             + "    SET  ITEM_GROUP = :ITEM_GROUP "
                             + "        ,ITEM_COUNT = :ITEM_COUNT "
                             + "        ,UPDATE_USERID = :UPDATE_USERID "
                             + "        ,UPDATE_TIME = :UPDATE_TIME  "
                             + "        ,PROCESS_ID = :PROCESS_ID "
                             + "        ,PRIMARY_FLAG ='N' "
                             + "  WHERE BOM_ID = :BOM_ID "
                             + "  AND ITEM_PART_ID = :ITEM_PART_ID ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_GROUP", sItemGroup };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_COUNT", sItemCount };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", sProcessID };
                        Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BOM_ID", sBOMID };
                        Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_PART_ID", sAlterPartID };
                        dsTemp = ExecuteSQL(sSQL, Params);

                        sMessage = "OK(UPDATE)";
                    }

                    //Delete============================
                    if (sModifyType == "D")
                    {
                        if (drNo == null)
                        {
                            sMessage = "Data not Exist";
                            g_StatusType = StatusType.Warning;
                            return;
                        }
                        object[][] Params = new object[2][];  
                        sSQL = " DELETE SAJET.SYS_BOM "
                             + "  WHERE BOM_ID = :BOM_ID "
                             + "  AND ITEM_PART_ID = :ITEM_PART_ID ";
                        
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BOM_ID", sBOMID };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_PART_ID", sAlterPartID };
                        dsTemp = ExecuteSQL(sSQL, Params);

                        sMessage = "OK(DELETE)";
                    }
                    updateSYSBOMINFO(sPartID);

                }
                catch (Exception exp)
                {
                    sMessage = exp.Message;
                    g_StatusType = StatusType.Error;
                }
            }
            finally
            {
                LVBOMAlter.Items.Add(sPartNo);
                LVBOMAlter.Items[LVBOMAlter.Items.Count - 1].SubItems.Add(sERPMasterItemNo);
                LVBOMAlter.Items[LVBOMAlter.Items.Count - 1].SubItems.Add(sERPAlterItemNo);
                LVBOMAlter.Items[LVBOMAlter.Items.Count - 1].SubItems.Add(sMessage);
                LVBOMAlter.Items[LVBOMAlter.Items.Count - 1].SubItems.Add(sDataIndex);
                if (sMessage.Length >= 2 && sMessage.Substring(0, 2) != "OK")
                {
                    sMessage = "Item No[" + sERPItemNo + "]Master Item[" + sERPMasterItemNo + "]Alter Item[" + sERPAlterItemNo + "]" + sMessage;
                }
                TransferFinish(sDataIndex, sMessage, g_StatusType, sTable);
                if (g_StatusType != StatusType.OK)
                {
                    GetColor(LVBOMAlter, g_StatusType);
                }
                LabBOMAlterCNT.Text = LVBOMAlter.Items.Count.ToString();
            }
        }    
        private void TransferPo(string sDataIndex, string sTable)
        {
            string sMessage = "";
            string sModifyType = "";
            g_StatusType = StatusType.OK;
            //ERP上==============================                  
            string sPONo = "";
            string sVendorID = "";
            string sPOLine = "";
            string sPOShipment = "";
            string sItemID = "";
            string sItemQty = "";
            string sPODate = "";
            DateTime dtPODate = DateTime.Now;
            //==================================                       
            string sMESVendorID = "0", sMESVendorCode = "";
            string sPartID = "0";          
            bool bUpdate = false;
            bool bInsert = false;
            g_dtSysdate = GetSysDate();

            try
            {
                //由Temp Table讀取資料
                string sCommand = " Select * from " + sTable + " "
                                + " Where DATA_IDX = '" + sDataIndex + "'";
                DataSet dsData = ExecuteSQL(sCommand, null);
                if (dsData.Tables[0].Rows.Count == 0)
                {
                    sMessage = "No Data - Data Index:" + sDataIndex;
                    g_StatusType = StatusType.Error;
                    return;
                }
                sModifyType = dsData.Tables[0].Rows[0]["ERP_FLAG"].ToString();
                sPONo = dsData.Tables[0].Rows[0]["PO_NO"].ToString();
                sVendorID = dsData.Tables[0].Rows[0]["VENDOR_ID"].ToString();
                sPOLine = dsData.Tables[0].Rows[0]["PO_LINE"].ToString();
                sPOShipment = dsData.Tables[0].Rows[0]["PO_SHIPMENT_NO"].ToString();
                sItemID = dsData.Tables[0].Rows[0]["ITEM_ID"].ToString();
                sItemQty = dsData.Tables[0].Rows[0]["ITEM_QTY"].ToString();
                sPODate = dsData.Tables[0].Rows[0]["PO_DATE"].ToString();
                if (!string.IsNullOrEmpty(sPODate))
                    dtPODate = DateTime.Parse(sPODate);
                try
                {
                    //Vendor
                    DataRow drVendorData = GetRowData("SAJET.SYS_VENDOR", "ERP_ID", sVendorID);
                    if (drVendorData == null)
                    {
                        sMessage = "Vendor not Exist - ERP ID:" + sVendorID;
                        g_StatusType = StatusType.Error;
                        return;
                    }
                    sMESVendorCode = drVendorData["VENDOR_CODE"].ToString();
                    sMESVendorID = drVendorData["VENDOR_ID"].ToString();
                    //Part 
                    sPartID = GetID("SAJET.SYS_PART","PART_ID","ERP_ID",sItemID);
                    if (sPartID == "0")
                    {
                        sMessage = "Part not Exist - ERP ID:" + sItemID;
                        g_StatusType = StatusType.Error;
                        return;
                    }

                    DataRow drID = GetRowData("SAJET.G_PO_MASTER", "PO_NO", sPONo);
                    //Insert====================
                    if (sModifyType == "C")
                    {
                        if (drID != null) //資料已存在
                        {
                            sMessage = "Data Exist";
                            g_StatusType = StatusType.Warning;
                            return;
                        }

                        bInsert = true;
                    }
                    //Update=============================
                    else if (sModifyType == "U")
                    {
                        if (drID == null)
                        {
                            sMessage = "Data not Exist";
                            g_StatusType = StatusType.Error;
                            return;
                        }                        
                        bUpdate = true;
                    }
                    if (bInsert)
                    {                        
                        object[][] Params = new object[9][];
                        sSQL = " INSERT INTO SAJET.G_PO_MASTER "
                             + " (PO_NO, VENDOR_ID, PO_LINE, PO_SHIPMENT_NO, PART_ID, ITEM_QTY, PO_DATE, UPDATE_USERID,UPDATE_TIME) "
                             + " VALUES "
                             + " (:PO_NO, :VENDOR_ID, :PO_LINE, :PO_SHIPMENT_NO, :PART_ID, :ITEM_QTY, :PO_DATE, :UPDATE_USERID, :UPDATE_TIME) ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PO_NO", sPONo };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_ID", sMESVendorID };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PO_LINE", sPOLine };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PO_SHIPMENT_NO", sPOShipment };
                        Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sPartID };
                        Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_QTY", sItemQty };
                        if (String.IsNullOrEmpty(sPODate))
                            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PO_DATE", "" };
                        else
                            Params[6] = new object[] { ParameterDirection.Input, OracleType.DateTime, "PO_DATE", dtPODate };
                        Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[8] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        dsTemp = ExecuteSQL(sSQL, Params);

                        sMessage = "OK(INSERT)";                       
                    }
                    else if (bUpdate)
                    {
                        object[][] Params = new object[9][];
                        sSQL = " UPDATE SAJET.G_PO_MASTER "
                             + "    SET VENDOR_ID = :VENDOR_ID "
                             + "       ,PART_ID = :PART_ID "
                             + "       ,ITEM_QTY = :ITEM_QTY "
                             + "       ,PO_DATE = :PO_DATE "
                             + "       ,UPDATE_USERID = :UPDATE_USERID "
                             + "       ,UPDATE_TIME = :UPDATE_TIME  "
                             + "  WHERE PO_NO = :PO_NO "
                             + "    AND PO_LINE = :PO_LINE "
                             + "    AND PO_SHIPMENT_NO = :PO_SHIPMENT_NO ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VENDOR_ID", sMESVendorID };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sPartID };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_QTY", sItemQty };
                        if (string.IsNullOrEmpty(sPODate))
                            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PO_DATE", "" };
                        else
                            Params[3] = new object[] { ParameterDirection.Input, OracleType.DateTime, "PO_DATE", dtPODate };
                        Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[5] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PO_NO", sPONo };
                        Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PO_LINE", sPOLine };
                        Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PO_SHIPMENT_NO", sPOShipment };
                        dsTemp = ExecuteSQL(sSQL, Params);

                        sMessage = "OK(UPDATE)";                        
                    }

                    //Delete============================
                    if (sModifyType == "D")
                    {
                        if (drID == null)
                        {
                            sMessage = "Data not Exist";
                            g_StatusType = StatusType.Warning;
                            return;
                        }                        
                        
                        object[][] Params = new object[3][];
                        sSQL = " DELETE SAJET.G_PO_MASTER "
                             + "  WHERE PO_NO = :PO_NO "
                             + "    AND PO_LINE = :PO_LINE "
                             + "    AND PO_SHIPMENT_NO = :PO_SHIPMENT_NO ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PO_NO", sPONo };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PO_LINE", sPOLine };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PO_SHIPMENT_NO", sPOShipment };
                        dsTemp = ExecuteSQL(sSQL, Params);

                        sMessage = "OK(DELETE)";
                    }
                }
                catch (Exception exp)
                {
                    sMessage = exp.Message;
                    g_StatusType = StatusType.Warning;
                }
            }
            finally
            {
                LVPO.Items.Add(sPONo);
                LVPO.Items[LVPO.Items.Count - 1].SubItems.Add(sMESVendorCode);
                LVPO.Items[LVPO.Items.Count - 1].SubItems.Add(sMessage);
                LVPO.Items[LVPO.Items.Count - 1].SubItems.Add(sDataIndex);

                TransferFinish(sDataIndex, sMessage, g_StatusType, sTable);
                if (g_StatusType != StatusType.OK)
                {
                    GetColor(LVPO, g_StatusType);
                }
                LabPOCNT.Text = LVPO.Items.Count.ToString();
            }
        }      

        private void TransferShipHeader(string sDataIndex, string sTable)
        {
            string sMessage = "";
            string sModifyType = "";
            g_StatusType = StatusType.OK;
            //ERP上==============================                 
            string sOrgan = "";             //Oracle公司別 
            string sERPCustID = "";      //客戶ID
            string sERPDNNo = "";   //出貨單號  
            string sERPShipTo = "";   //出貨地址  
            string sERPShipContact = "";   //聯絡人                  
            //==================================            
            string sMESID = "0";
            string sCustID = "", sCustName = "";
            string sScheduleDate = "";
            string sInvoiceNo = "";//發票號碼
            bool bUpdate = false;
            bool bInsert = false;
            g_dtSysdate = GetSysDate();

            try
            {
                //由Temp Table讀取資料
                string sCommand = " Select * from " + sTable + " "
                                + " Where DATA_IDX = '" + sDataIndex + "'";
                DataSet dsData = ExecuteSQL(sCommand, null);
                if (dsData.Tables[0].Rows.Count == 0)
                {
                    sMessage = "No Data - Data Index:" + sDataIndex;
                    g_StatusType = StatusType.Error;
                    return;
                }
                sModifyType = dsData.Tables[0].Rows[0]["ERP_FLAG"].ToString();
                sOrgan = dsData.Tables[0].Rows[0]["ORGANIZATION"].ToString();
                sERPCustID = dsData.Tables[0].Rows[0]["CUSTOMER_ID"].ToString().Trim();
                sERPDNNo = dsData.Tables[0].Rows[0]["SHIPPING_NO"].ToString().Trim(); 
                sERPShipTo = dsData.Tables[0].Rows[0]["SHIPPING_ADDRESS"].ToString().Trim();  
                sERPShipContact = dsData.Tables[0].Rows[0]["SHIPPING_CONTACT"].ToString().Trim();
                sScheduleDate = dsData.Tables[0].Rows[0]["SCHEDULE_SHIP_DATE"].ToString().Trim();
                sInvoiceNo = dsData.Tables[0].Rows[0]["INVOICE_NO"].ToString().Trim();      

                try
                {
                    DataRow drCustData = GetRowData("SAJET.SYS_CUSTOMER", "CUSTOMER_CODE", sERPCustID);
                    if (drCustData == null)
                    {
                        sMessage = "Customer not Exist - Customer Code:" + sERPCustID;
                        g_StatusType = StatusType.Error;
                        return;
                    }
                    sCustID = drCustData["CUSTOMER_ID"].ToString();
                    sCustName = drCustData["CUSTOMER_NAME"].ToString();

                    //DataRow drID = GetRowData("SAJET.G_DN_BASE", "ERP_ID", sERPDelveryID); //以ERP_ID查詢 
                    DataRow drNo = GetRowData("SAJET.G_DN_BASE", "DN_NO", sERPDNNo);  //以No查詢 

                    //Insert====================
                    if (sModifyType == "C")
                    {
                        if (drNo != null)
                        {
                            if (drNo["ENABLED"].ToString() == "Y")
                            {
                                sMessage = "Data Exist";
                                g_StatusType = StatusType.Warning;
                                return;
                            }
                            sMESID = drNo["DN_ID"].ToString();
                            bUpdate = true;
                        }
                        else
                        {
                            bInsert = true;
                        }
                    }

                    //Update=============================
                    else if (sModifyType == "U")
                    {
                        if (drNo == null)
                        {
                            sMessage = "Data not Exist";
                            g_StatusType = StatusType.Error;
                            return;
                        }
                        sMESID = drNo["DN_ID"].ToString();
                        bUpdate = true;
                    }

                    if (bInsert)
                    {
                        sMESID = GetDNMaxID();
                        if (sMESID == "0")
                        {
                            sMessage = "Get MaxID Error";
                            g_StatusType = StatusType.Error;
                            return;
                        }
                        object[][] Params = new object[9][];
                        sSQL = " INSERT INTO SAJET.G_DN_BASE "
                             + " (DN_ID, DN_NO, CUSTOMER_ID, SHIP_TO,UPDATE_USERID,UPDATE_TIME, SHIP_CONTACT,SCHEDULE_SHIP_DATE,INVOICE_NO ) "
                             + " VALUES "
                             + " (:DN_ID, :DN_NO, :CUSTOMER_ID, :SHIP_TO ,:UPDATE_USERID, :UPDATE_TIME, :SHIP_CONTACT,:SCHEDULE_SHIP_DATE,:INVOICE_NO ) ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DN_ID", sMESID };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DN_NO", sERPDNNo };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_ID", sCustID };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHIP_TO", sERPShipTo };
                        Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[5] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHIP_CONTACT", sERPShipContact };
                        Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SCHEDULE_SHIP_DATE", sScheduleDate };
                        Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "INVOICE_NO", sInvoiceNo };

                        dsTemp = ExecuteSQL(sSQL, Params);

                        sMessage = "OK(INSERT)";
                    }
                    else if (bUpdate)
                    {
                        object[][] Params = new object[9][];
                        sSQL = " UPDATE SAJET.G_DN_BASE "
                             + "    SET  DN_NO = :DN_NO "
                             + "        ,CUSTOMER_ID = :CUSTOMER_ID "
                             + "        ,SHIP_TO = :SHIP_TO "
                             + "        ,UPDATE_USERID = :UPDATE_USERID "
                             + "        ,UPDATE_TIME = :UPDATE_TIME  "
                             + "        ,SHIP_CONTACT = :SHIP_CONTACT  "
                             + "        ,SCHEDULE_SHIP_DATE = :SCHEDULE_SHIP_DATE "
                             + "        ,INVOICE_NO =:INVOICE_NO "
                             + "        ,ENABLED ='Y' "
                             + "  WHERE DN_ID = :DN_ID ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DN_NO", sERPDNNo };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_ID", sCustID };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHIP_TO", sERPShipTo };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[4] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHIP_CONTACT", sERPShipContact };
                        Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SCHEDULE_SHIP_DATE", sScheduleDate };
                        Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "INVOICE_NO", sInvoiceNo };
                        Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DN_ID", sMESID };
                        dsTemp = ExecuteSQL(sSQL, Params);
                        sMessage = "OK(UPDATE)";
                    }

                    //Delete============================
                    if (sModifyType == "D")
                    {
                        if (drNo == null)
                        {
                            sMessage = "Data not Exist";
                            g_StatusType = StatusType.Warning;
                            return;
                        }
                        sMESID = drNo["DN_ID"].ToString();
                        object[][] Params = new object[3][];
                        sSQL = " UPDATE SAJET.G_DN_BASE "
                             + "    SET ENABLED = 'N' "
                             + "       ,UPDATE_USERID = :UPDATE_USERID "
                             + "       ,UPDATE_TIME = :UPDATE_TIME "
                             + "  WHERE DN_ID = :DN_ID ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DN_ID", sMESID };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        dsTemp = ExecuteSQL(sSQL, Params);

                        sMessage = "OK(DELETE)";
                    }
                }
                catch (Exception exp)
                {
                    sMessage = exp.Message;
                    g_StatusType = StatusType.Error;
                }
            }
            finally
            {
                LVShipHeader.Items.Add(sERPDNNo);
                LVShipHeader.Items[LVShipHeader.Items.Count - 1].SubItems.Add(sCustName);
                LVShipHeader.Items[LVShipHeader.Items.Count - 1].SubItems.Add(sMessage);
                LVShipHeader.Items[LVShipHeader.Items.Count - 1].SubItems.Add(sDataIndex);
                if (sMessage.Length >= 2 && sMessage.Substring(0, 2) != "OK")
                {
                    sMessage = "DN No[" + sERPDNNo + "]" + sMessage;
                }
                TransferFinish(sDataIndex, sMessage, g_StatusType, sTable);
                if (g_StatusType != StatusType.OK)
                {
                    GetColor(LVShipHeader, g_StatusType);
                }
                LabShipCNT.Text = LVShipHeader.Items.Count.ToString();
            }    
        }
        private void TransferShipDetail(string sDataIndex, string sTable)
        {
            string sMessage = "";
            string sModifyType = "";
            g_StatusType = StatusType.OK;
            //ERP上==============================                             
            string sOrgan = "";             //Oracle公司別   
            string sERPDNID = "";    //出貨單號
            string sERPDNITEM = "";        //項次
            string sERPItemNo = "";      //料號
            string sERPQty = "";          //數量
            string sERPRMADate = "";      //保固日期
            string sSONo = "";
            string sValueAdd = ""; //購買加值保固註記
            string sExtWD = "0";//加值保固年
            string sSOItem = "";
            DateTime dtRMADate = DateTime.Now; 
            //==================================                       
            string sPartID = "", sPartNo = "";
            string sDNID = "", sDNNo = "";
            string sDNItem = "";            
            bool bUpdate = false;
            bool bInsert = false;
            g_dtSysdate = GetSysDate();

            try
            {
                //由Temp Table讀取資料
                string sCommand = " Select * from " + sTable + " "
                                + " Where DATA_IDX = '" + sDataIndex + "'";
                DataSet dsData = ExecuteSQL(sCommand, null);
                if (dsData.Tables[0].Rows.Count == 0)
                {
                    sMessage = "No Data - Data Index:" + sDataIndex;
                    g_StatusType = StatusType.Error;
                    return;
                }
                sModifyType = dsData.Tables[0].Rows[0]["ERP_FLAG"].ToString();
                sOrgan = dsData.Tables[0].Rows[0]["ORGANIZATION"].ToString();
                sERPDNID = dsData.Tables[0].Rows[0]["SHIPPING_NO"].ToString().Trim();
                sERPDNITEM = dsData.Tables[0].Rows[0]["SHIPPING_SEQ"].ToString().Trim();
                sERPItemNo = dsData.Tables[0].Rows[0]["ITEM_NO"].ToString().Trim();
                sERPQty = dsData.Tables[0].Rows[0]["SHIPPING_QTY"].ToString().Trim();
                sERPRMADate = dsData.Tables[0].Rows[0]["RMA_DATE"].ToString();
                sSONo = dsData.Tables[0].Rows[0]["SO_NO"].ToString();
                sValueAdd = dsData.Tables[0].Rows[0]["VALUEADD"].ToString();
                sExtWD = dsData.Tables[0].Rows[0]["EXT_WD"].ToString();
                sSOItem = dsData.Tables[0].Rows[0]["SO_ITEM"].ToString();
                try
                {
                    Convert.ToDouble(sExtWD);
                }
                catch
                {
                    sExtWD = "0";
                }
                if (!string.IsNullOrEmpty(sERPRMADate))
                    dtRMADate = (DateTime)dsData.Tables[0].Rows[0]["RMA_DATE"];
                try
                {
                    //料號
                    DataRow drPartData = GetRowData("SAJET.SYS_PART", "PART_NO", sERPItemNo);
                    if (drPartData == null)
                    {
                        sMessage = "Item not Exist -  Item No:" + sERPItemNo;
                        g_StatusType = StatusType.Error;
                        return;
                    }
                    sPartID = drPartData["PART_ID"].ToString();
                    sPartNo = drPartData["PART_NO"].ToString();

                    //DN主檔
                    DataRow drDNData = GetRowData("SAJET.G_DN_BASE", "DN_NO", sERPDNID);
                    if (drDNData == null)
                    {
                        sMessage = "DN Base not Exist - DN No:" + sERPDNID;
                        g_StatusType = StatusType.Error;
                        return;
                    }
                    sDNID = drDNData["DN_ID"].ToString();
                    //sDNNo = drDNData["DN_NO"].ToString();

                    //
                  //  DataRow drID = GetRowData("SAJET.G_DN_DETAIL", "ERP_ID", sERPDelDetailID); //以ERP_ID查詢 
                    DataRow drNo = GetRowData("SAJET.G_DN_DETAIL", "DN_ID", sDNID, "DN_ITEM", sERPDNITEM);

                    //Insert====================
                    if (sModifyType == "C")
                    {
                        if (drNo != null)
                        {
                            sMessage = "Data Exist";
                            g_StatusType = StatusType.Warning;
                            return;
                        }
                        bInsert = true;
                    }
                    //Update=============================
                    else if (sModifyType == "U")
                    {
                        if (drNo == null)
                        {
                            sMessage = "Data not Exist";
                            g_StatusType = StatusType.Error;
                            return;

                        }
                        bUpdate = true;
                    }
                    //VALUEADD VARCHAR2(500), EXT_WD NUMBER
                    if (bInsert)
                    {
                        object[][] Params = new object[11][];
                        sSQL = " INSERT INTO SAJET.G_DN_DETAIL "
                             + " (DN_ID, DN_ITEM, PART_ID, QTY "
                             + " ,UPDATE_USERID, UPDATE_TIME, RMA_DATE,SO_NO,VALUEADD,EXT_WD,SO_ITEM "
                             + " ) "
                             + " VALUES "
                             + " (:DN_ID, :DN_ITEM, :PART_ID, :QTY "
                             + " ,:UPDATE_USERID, :UPDATE_TIME, :RMA_DATE,:SO_NO,:VALUEADD,:EXT_WD,:SO_ITEM  "
                             + " ) ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DN_ID", sDNID };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DN_ITEM", sERPDNITEM };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sPartID };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "QTY", sERPQty };
                        Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[5] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        if (string.IsNullOrEmpty(sERPRMADate))
                            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RMA_DATE", "" };
                        else
                            Params[6] = new object[] { ParameterDirection.Input, OracleType.DateTime, "RMA_DATE", dtRMADate };
                        Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SO_NO", sSONo };
                        Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VALUEADD", sValueAdd };
                        Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "EXT_WD", sExtWD };
                        Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SO_ITEM", sSOItem };
                        
                        dsTemp = ExecuteSQL(sSQL, Params);
                        sMessage = "OK(INSERT)";
                    }
                    else if (bUpdate)
                    {
                        object[][] Params = new object[11][];
                        sSQL = " UPDATE SAJET.G_DN_DETAIL "
                             + "    SET  PART_ID = :PART_ID "
                             + "        ,QTY = :QTY "
                             + "        ,UPDATE_USERID = :UPDATE_USERID "
                             + "        ,UPDATE_TIME = :UPDATE_TIME  "
                             + "        ,RMA_DATE = :RMA_DATE "
                             + "        ,SO_NO =:SO_NO "
                             + "        ,VALUEADD =:VALUEADD "
                             + "        ,EXT_WD =:EXT_WD "
                             + "        ,SO_ITEM =:SO_ITEM "
                             + "  WHERE DN_ID =:DN_ID "
                             + "    AND DN_ITEM =:DN_ITEM ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sPartID };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "QTY", sERPQty };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", g_dtSysdate };
                        if (string.IsNullOrEmpty(sERPRMADate))
                            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RMA_DATE", "" };
                        else
                            Params[4] = new object[] { ParameterDirection.Input, OracleType.DateTime, "RMA_DATE", dtRMADate };
                        Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SO_NO", sSONo };
                        Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VALUEADD", sValueAdd };
                        Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "EXT_WD", sExtWD };
                        Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SO_ITEM", sSOItem };
                        Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DN_ID", sDNID };
                        Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DN_ITEM", sERPDNITEM };

                        dsTemp = ExecuteSQL(sSQL, Params);

                        sMessage = "OK(UPDATE)";
                    }

                    //Delete============================
                    if (sModifyType == "D")
                    {
                        /*
                        if (drID == null)
                        {
                            sMessage = "Data not Exist";
                            g_StatusType = StatusType.Warning;
                            return;
                        }
                         */
                        object[][] Params = new object[1][];
                        sSQL = " DELETE SAJET.G_DN_DETAIL "
                             + "   WHERE DN_ID =:DN_ID ";
                             //+ "     AND DN_ITEM =:DN_ITEM ";
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DN_ID", sDNID };
                        //Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DN_ITEM", sERPDNITEM };
                        dsTemp = ExecuteSQL(sSQL, Params);
                        sMessage = "OK(DELETE)";
                    }
                }
                catch (Exception exp)
                {
                    sMessage = exp.Message;
                    g_StatusType = StatusType.Error;
                }
            }
            finally
            {
                LVShipDetail.Items.Add(sERPDNID);
                LVShipDetail.Items[LVShipDetail.Items.Count - 1].SubItems.Add(sERPDNITEM);
                LVShipDetail.Items[LVShipDetail.Items.Count - 1].SubItems.Add(sPartNo);
                LVShipDetail.Items[LVShipDetail.Items.Count - 1].SubItems.Add(sMessage);
                LVShipDetail.Items[LVShipDetail.Items.Count - 1].SubItems.Add(sDataIndex);
                if (sMessage.Length >= 2 && sMessage.Substring(0, 2) != "OK")
                {
                    sMessage = "DN No[" + sERPDNID + "]DN Item[" + sERPDNITEM + "]" + sMessage;
                }
                TransferFinish(sDataIndex, sMessage, g_StatusType, sTable);
                if (g_StatusType != StatusType.OK)
                {
                    GetColor(LVShipDetail, g_StatusType);
                }
                LabShipDetailCNT.Text = LVShipDetail.Items.Count.ToString();
            }
        }
       
        private void GetColor(ListView LVTemp,StatusType StatusType)
        {
            if (g_StatusType == StatusType.Error)
            {
                LVTemp.Items[LVTemp.Items.Count - 1].BackColor = Color.Red;
                LVTemp.Items[LVTemp.Items.Count - 1].ForeColor = Color.White;
            }
            else if (g_StatusType == StatusType.Warning)
            {
                LVTemp.Items[LVTemp.Items.Count - 1].BackColor = Color.Yellow;
                LVTemp.Items[LVTemp.Items.Count - 1].ForeColor = Color.Black;
            }
        }

        private void TransferFinish(string sDataIndex, string sErrMsg, StatusType StatusType, string sTable)
        {            
            string sMESStats = "";
            string sTargetTable = "";

            string sSrcTLTable = "";
            string sTargetTLTable = "";
            switch (StatusType)
            {
                case StatusType.OK: 
                    sMESStats = "0";                  
                    break;
                case StatusType.Warning: 
                    sMESStats = "1";                   
                    break;
                case StatusType.Error: 
                    sMESStats = "2";                   
                    break;
            }

            //處理正常資料
            if (g_sTransferType == "H")
            {
                if (StatusType == StatusType.OK)
                {
                    sTargetTable = sTable + "_H";
                    sTargetTLTable = "MESUSER.ERP_INTERFACE_H";
                }
                else if (StatusType == StatusType.Warning)
                {
                    sTargetTable = sTable + "_H";
                    sTargetTLTable = "MESUSER.ERP_INTERFACE_H";
                }
                else
                {
                    sTargetTable = sTable + "_E";
                    sTargetTLTable = "MESUSER.ERP_INTERFACE_E";
                }
                UpdateMESStatus(sDataIndex, sMESStats, sErrMsg, sTable, sTargetTable);

                sSrcTLTable = "MESUSER.ERP_INTERFACE";
                UpdateTLStatus(sDataIndex, StatusType, sSrcTLTable, sTargetTLTable);

                Application.DoEvents();

                //if (StatusType != StatusType.OK)
                if (StatusType == StatusType.Error)
                {                   
                    LVException.Items.Add(sTable);
                    LVException.Items[LVException.Items.Count - 1].SubItems.Add(sDataIndex);
                    LVException.Items[LVException.Items.Count - 1].SubItems.Add(sErrMsg);
                    LVException.Items[LVException.Items.Count - 1].SubItems.Add(g_dtSysdate.ToString("yyyy/MM/dd HH:mm:ss"));
                }                
            } 
            //處理Exception資料
            else if (g_sTransferType == "E")
            {                                
                if (StatusType == StatusType.OK || StatusType == StatusType.Warning)
                {
                    sTargetTable = sTable.TrimEnd('E') + "H";
                    UpdateMESStatus(sDataIndex, sMESStats, sErrMsg, sTable, sTargetTable);

                    sSrcTLTable = "MESUSER.ERP_INTERFACE_E";
                    sTargetTLTable = "MESUSER.ERP_INTERFACE_H";
                    UpdateTLStatus(sDataIndex, StatusType, sSrcTLTable, sTargetTLTable);
                }
                else //若仍然有Exception則保留,不搬走
                {
                    object[][] Params = new object[4][];
                    sSQL = " Update " + sTable + " "
                         + " Set MES_STATUS = :MES_STATUS "
                         + "    ,MES_UPDATE_TIME = :MES_UPDATE_TIME "
                         + "    ,MES_EXCEPTION = :MES_EXCEPTION "
                         + " Where DATA_IDX = :DATA_IDX";
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MES_STATUS", sMESStats };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.DateTime, "MES_UPDATE_TIME", g_dtSysdate };
                    Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MES_EXCEPTION", sErrMsg };
                    Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DATA_IDX", sDataIndex };
                    dsTemp = ExecuteSQL(sSQL, Params);
                }
            }

        }
        private void UpdateMESStatus(string sDataIndex, string sMESStats, string sErrMsg, string sSrcTable, string sTargetTable)
        {            
            //更新狀態
            object[][] Params = new object[4][];
            sSQL = " Update " + sSrcTable + " "
                 + " Set MES_STATUS = :MES_STATUS "
                 + "    ,MES_UPDATE_TIME = :MES_UPDATE_TIME "
                 + "    ,MES_EXCEPTION = :MES_EXCEPTION "
                 + " Where DATA_IDX = :DATA_IDX";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MES_STATUS", sMESStats };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.DateTime, "MES_UPDATE_TIME", g_dtSysdate };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MES_EXCEPTION", sErrMsg };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DATA_IDX", sDataIndex };            
            dsTemp = ExecuteSQL(sSQL, Params);
            
            //Copy至History或Exception中
            Params = new object[1][];
            sSQL = " Insert Into " + sTargetTable + " "
                 + " Select * from " + sSrcTable + " "
                 + " Where DATA_IDX = :DATA_IDX";                        
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DATA_IDX", sDataIndex };
            dsTemp = ExecuteSQL(sSQL, Params);

            //刪除
            Params = new object[1][];
            sSQL = " Delete " + sSrcTable + " "                 
                 + " Where DATA_IDX = :DATA_IDX";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DATA_IDX", sDataIndex };
            dsTemp = ExecuteSQL(sSQL, Params);           
        }
        private void UpdateTLStatus(string sDataIndex, StatusType StatusType, string sSrcTable, string sTargetTable)
        {
            string sMESStats = "";            
            switch (StatusType)
            {
                case StatusType.OK: 
                    sMESStats = "0";                   
                    break;
                case StatusType.Warning: 
                    sMESStats = "1"; 
                    break;
                case StatusType.Error: 
                    sMESStats = "2"; 
                    break;
            }

            //更新狀態
            object[][] Params = new object[2][];
            sSQL = " Update " + sSrcTable + " "
                 + " Set PROCESS_FLAG = :MES_STATUS "
                 + " Where DATA_IDX = :DATA_IDX";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MES_STATUS", sMESStats };            
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DATA_IDX", sDataIndex };            
            dsTemp = ExecuteSQL(sSQL, Params);

            //Copy至History或Exception中
            Params = new object[1][];
            sSQL = " Insert Into " + sTargetTable + " "
                 + " Select * from " + sSrcTable + " "
                 + " Where DATA_IDX = :DATA_IDX";                        
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DATA_IDX", sDataIndex };
            dsTemp = ExecuteSQL(sSQL, Params);

            //刪除
            Params = new object[1][];
            sSQL = " Delete " + sSrcTable + " "                 
                 + " Where DATA_IDX = :DATA_IDX";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DATA_IDX", sDataIndex };
            dsTemp = ExecuteSQL(sSQL, Params);                     
        }

        private void ReadConfiguration()
        {
            string sChecked = "0";
            SajetIniFile SajetIniFile = new SajetIniFile();
            sChecked = SajetIniFile.ReadIniFile(g_sIniFile, g_sIniSection, "AutoTransfer", "0");
            g_bAutoTrans = (sChecked == "1");
            g_iInterval = Convert.ToInt32(SajetIniFile.ReadIniFile(g_sIniFile, g_sIniSection, "Interval", "10"));
            sChecked = SajetIniFile.ReadIniFile(g_sIniFile, g_sIniSection, "SendMail", "0");
            g_bSendMail = (sChecked == "1");
            
            sChecked = SajetIniFile.ReadIniFile(g_sIniFile, g_sIniSection, "Break", "0");
            g_bBreak = (sChecked == "1");

            for (int i = 0; i <= LVItem.Items.Count - 1; i++)
            {
                LVItem.Items[i].Checked = false;
                string sType = LVItem.Items[i].SubItems[0].Text;
                sChecked = SajetIniFile.ReadIniFile(g_sIniFile, g_sIniSection, "TransItem" + sType, "0");
                LVItem.Items[i].Checked = (sChecked == "1");
            }                 
            txtInterval.Text = g_iInterval.ToString();
            chkbAutoTransfer.Checked = g_bAutoTrans;
            chkbMail.Checked = g_bSendMail;          
            chkbBreak.Checked = g_bBreak;

            string sMsg = "";
            ReadDBCfg(ref sMsg);
        }

        private void setupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fSetup formData = new fSetup();
            timer1.Enabled = false;
            try
            {
                formData.g_sDBName = txtDBName.Text;
                if (formData.ShowDialog() == DialogResult.OK)
                {
                    ReadConfiguration();
                }
            }
            finally
            {
                formData.Dispose();
                timer1.Interval = 1000 * 60 * g_iInterval;
                timer1.Enabled = g_bAutoTrans;
            }
        }
        private bool CheckApplicationExist()
        {
            Boolean bExist = false;
            Process[] ps = Process.GetProcesses();
            int iQty = 0;
            for (int i = 0; i <= ps.Length - 1; i++)
            {
                if (ps[i].ProcessName.ToUpper() == "MESINTERFACE")
                {
                    iQty += 1;
                    //   bExist = true;
                    //  break;
                }
            }
            if (iQty > 1)
                bExist = true;
            return bExist;
        }
        private void fMain_Load(object sender, EventArgs e)
        {
            if (CheckApplicationExist())
            {
                Show_Message("MESInterface Is Running", 0);
                Application.Exit();
            }
            this.Text = this.Text + " - " + FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion.ToString();
            timer1.Enabled = false;
            string sMsg = "";
            LabMsg.Text = "";
            LabMsg.Visible = false ;
            tabPageBOMHeader.Parent = null;
            
            tabPageModel.Parent = null;
            tabPageModelDetail.Parent = null;
            tabPagePO.Parent = null;
            tabPage2.Parent = null;

            tabPageBOMLoc.Parent = null;
            tabPageDMDA.Parent = null;
            tabPageShipInfo.Parent = null;

            tabPageECN.Parent = null;
            tabPageCustomerItem.Parent = null;
            tabPageMPN.Parent = null;

            g_tsItem = new string[4];
            int iIndex = -1;
            g_tsItem[iIndex+=1] = "PART";
           // g_tsItem[iIndex += 1] = "CUSTOMER";
           // g_tsItem[iIndex += 1] = "CUSTOMER ITEM";
           // g_tsItem[iIndex += 1] = "VENDOR";
           // g_tsItem[iIndex += 1] = "BOM Detail";
           // g_tsItem[iIndex+=1] = "BOM Alternative";
           // g_tsItem[iIndex += 1] = "BOM Location";
            g_tsItem[iIndex += 1] = "WO";
           // g_tsItem[iIndex += 1] = "WO BOM";
           // g_tsItem[iIndex += 1] = "RT NO";
           // g_tsItem[iIndex += 1] = "RT Detail";
           // g_tsItem[iIndex += 1] = "ECN WO";
            g_tsItem[iIndex += 1] = "WIP";
            g_tsItem[iIndex += 1] = "WO TRANSFER";
            iIndex = -1;
            g_tsItemDesc = new string[4];
            g_tsItemDesc[iIndex+=1] = "ITEM_MASTER";
            //g_tsItemDesc[iIndex += 1] = "CUSTOMER_MASTER";
            //g_tsItemDesc[iIndex += 1] = "CUSTOMER_ITEM_INFO";
            //g_tsItemDesc[iIndex += 1] = "VENDOR_MASTER";
            //g_tsItemDesc[iIndex += 1] = "BOM_DETAIL";
            //g_tsItemDesc[iIndex += 1] = "BOM_ALTERNATIVE";
            //g_tsItemDesc[iIndex += 1] = "BOM_LOCATION";
            g_tsItemDesc[iIndex += 1] = "JOB_HEADER";
            //g_tsItemDesc[iIndex += 1] = "JOB_DETAIL";
            //g_tsItemDesc[iIndex += 1] = "ECN_WO";          
            //g_tsItemDesc[iIndex += 1] = "RT_HEADER";
            //g_tsItemDesc[iIndex += 1] = "RT_DETAIL";
            g_tsItemDesc[iIndex += 1] = "TIME_RANGE";
            g_tsItemDesc[iIndex += 1] = "JOB_TRANSFER";

            try
            {
                LVItem.Items.Clear();
                for (int i = 0; i <= g_tsItem.Length - 1; i++)
                {
                    LVItem.Items.Add(g_tsItem[i].ToString());
                    LVItem.Items[i].SubItems.Add(g_tsItemDesc[i].ToString());
                    LVItem.Items[i].Name = g_tsItemDesc[i].ToString();
                }
                ReadConfiguration();
                ReadMailConfig();
                tabControl2.SelectedIndex = 1;
                ReadDBCfg(ref sMsg);
            }
            finally
            {
                timer1.Interval = 1000 * 60 * g_iInterval;
                timer1.Enabled = g_bAutoTrans;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            g_bStop = true;
        }

        private void btnTestConn_Click(object sender, EventArgs e)
        {
            string sMsg = "";
            if (!CheckDBConnection(ref sMsg))
                Show_Message("Failed to connect to database" + Environment.NewLine + sMsg, 0);
            else
                Show_Message("Connect to database successfully", 3);
        }

        private bool CheckIQCDataExist(string sRTID,string sRTSeq)
        {
            sSQL = " Select LOT_NO,QC_RESULT from SAJET.G_IQC_LOT "
                 + " Where RT_ID = '" + sRTID + "' "
                 + " and RT_SEQ = '" + sRTSeq + "' "
                 + " and rownum = 1";
            dsTemp = ExecuteSQL(sSQL, null);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                return false;
            }
            
            string sQCResult = dsTemp.Tables[0].Rows[0]["QC_RESULT"].ToString();
            if (sQCResult != "N/A")//已經有檢驗結果
            {
                return true;
            }
            return false;
            
            /*
            string sLot = dsTemp.Tables[0].Rows[0]["LOT_NO"].ToString();

            sSQL = " Select * from sajet.G_IQC_TEST_TYPE "
                 + " Where LOT_NO = '" + sLot + "' "                
                 + " and rownum = 1";
            dsTemp = ExecuteSQL(sSQL, null);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                sSQL = " Select * from sajet.G_IQC_TEST_TYPE_DEFECT "
                     + " Where LOT_NO = '" + sLot + "' "                   
                     + " and rownum = 1";
                dsTemp = ExecuteSQL(sSQL, null);
            }
            if (dsTemp.Tables[0].Rows.Count > 0)
                return true;
            else
                return false;
             */ 
        }
        private void ReadMailConfig()
        {
            //讀取Mail資訊
            string sSection = "EMail";
            SajetIniFile SajetIniFile = new SajetIniFile();
            string sSmtpHost = SajetIniFile.ReadIniFile(g_sIniFile, sSection, "SMTPHost", "");
            string sSmtpPort = SajetIniFile.ReadIniFile(g_sIniFile, sSection, "SMTPPort", "25");
            string sUser = SajetIniFile.ReadIniFile(g_sIniFile, sSection, "User", "");
            string sPasswd = SajetIniFile.ReadIniFile(g_sIniFile, sSection, "Passwd", "");
            string sMailFrom = SajetIniFile.ReadIniFile(g_sIniFile, sSection, "MailFrom", "");
            string sMailTo = SajetIniFile.ReadIniFile(g_sIniFile, sSection, "MailTo", "");
            string sMailSubject = SajetIniFile.ReadIniFile(g_sIniFile, sSection, "MailSubject", "MES Interface Fail");

            g_MailItems.userName = sUser;
            g_MailItems.password = sPasswd;
            g_MailItems.smtpHost = sSmtpHost;
            g_MailItems.Subject = sMailSubject;
            g_MailItems.smtpPort = Convert.ToInt32(sSmtpPort);
            g_MailItems.From = sMailFrom; //寄件者                                           
            g_MailItems.To = sMailTo;  //收件者                                
        }
        private bool SendMail(string sBody)
        {
            string sErrMsg = "";
            if (g_MailItems.Subject == "")
                g_MailItems.Subject = "Interface Fail";  //郵件主旨
            g_MailItems.body = sBody;  //郵件內容
            ClassMail.SendMail mail = new ClassMail.SendMail();
            bool bSendResult = mail.Send(g_MailItems, ref sErrMsg);
            if (!bSendResult)
            {
                LabMsg.Text = "Send Mail Fail - " + sErrMsg;
                LabMsg.Visible = true;
            }
            return bSendResult;
        }

        private void mailAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReadMailConfig();
            fMail fM = new fMail();
            timer1.Enabled = false;
            try
            {
                if (fM.ShowDialog() == DialogResult.OK)
                {
                    ReadMailConfig();
                }
            }
            finally
            {
                fM.Dispose();
                timer1.Interval = 1000 * 60 * g_iInterval;
                timer1.Enabled = g_bAutoTrans;
            }
        }

        private bool Check_ExpFinish()
        {
            for (int i = 0; i <= LVItem.Items.Count - 1; i++)
            {
                if (!LVItem.Items[i].Checked)
                    continue;
                string sTable =  LVItem.Items[i].SubItems[1].Text;
                string sExpTable = "MESUSER." + sTable + "_E";
                sSQL = "Select * from " + sExpTable;
                dsTemp = ExecuteSQL(sSQL, null);
                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    LVException.Items.Add(sExpTable);
                    LVException.Items[LVException.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[0]["DATA_IDX"].ToString());
                    LVException.Items[LVException.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[0]["MES_EXCEPTION"].ToString());
                    LVException.Items[LVException.Items.Count - 1].SubItems.Add(dsTemp.Tables[0].Rows[0]["MES_UPDATE_TIME"].ToString());

                    LabMsg.Text = "Exception Data not Finish : " + sExpTable;
                    LabMsg.Visible = true;

                    if (g_bSendMail)
                    {
                        string sBody = LabMsg.Text;
                        SendMail(sBody);
                    }
                    return false;
                }
            }

            return true;
        }


        private string GetDNMaxID()
        {
            DataSet dsTemp = new DataSet();
            string sMaxID = "";
            sSQL = "Select RPAD(NVL(PARAM_VALUE,'1'),2,'0') || TO_CHAR(SYSDATE,'YYMMDD') || '001' DNID  "
                 + "  FROM SAJET.SYS_BASE "
                 + "  Where PARAM_NAME = 'DBID' ";
            dsTemp = ExecuteSQL(sSQL, null);
            sMaxID = dsTemp.Tables[0].Rows[0]["DNID"].ToString();
            sSQL = "Select NVL(Max(DN_ID),0) + 1 DNID "
                + " From SAJET.G_DN_BASE "
                + " Where DN_ID >= '" + sMaxID + "' ";
            dsTemp = ExecuteSQL(sSQL, null);
            if (dsTemp.Tables[0].Rows[0]["DNID"].ToString() != "1")
                sMaxID = dsTemp.Tables[0].Rows[0]["DNID"].ToString();
            return sMaxID;
        }
        private void updateSYSBOMINFO(string sPartID)
        {
            string sSQL = " SELECT BOM_ID FROM SAJET.SYS_BOM  "
                         + " WHERE ITEM_PART_ID =:PART_ID "
                         + "   AND JOB_EXTEND ='3' "
                         + " GROUP BY BOM_ID ";
            object[][] ParamsBOM = new object[1][];
            ParamsBOM[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sPartID };
            DataSet dsTempBOM = ExecuteSQL(sSQL, ParamsBOM);
            for (int i = 0; i <= dsTempBOM.Tables[0].Rows.Count - 1; i++)
            {
                string sBOMID = dsTempBOM.Tables[0].Rows[i]["BOM_ID"].ToString();
                sSQL = "UPDATE SAJET.SYS_BOM_INFO "
                           + "  SET download_bom ='N' "
                           + " WHERE BOM_ID =:BOM_ID";
                object[][] Params = new object[1][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BOM_ID", sBOMID };
                DataSet dsTemp = ExecuteSQL(sSQL, Params);
            }
        }


       
      

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }


        private string ReturnQtyPer(string sQtyPer, string sTargetQty)
        {
            double dQtyPer = 0;
            double dTargetQty = 0;
            double dValue = 0;
            string sValue = "0";
            if (sTargetQty == "0")
                return sValue ;

            try
            {
                dQtyPer = Convert.ToDouble(sQtyPer);
            }
            catch { }
            try
            {
                dTargetQty = Convert.ToDouble(sTargetQty);
            }
            catch { }
            try
            {
                dValue = Math.Truncate(dQtyPer / dTargetQty);
                sValue = Convert.ToString(dValue);
            }
            catch { }
            return sValue;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                editFileName.Text = openFileDialog1.FileName;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!File.Exists(editFileName.Text))
            {
                return;
            }
            int iRowCount = 1;

            ExportOfficeExcel.ExcelEdit ExcelClass = new ExportOfficeExcel.ExcelEdit();
            try
            {
                ExcelClass.Open(editFileName.Text);
                Excel.Worksheet ExcelSheet = ExcelClass.GetSheet("Sheet1");
                iRowCount = Convert.ToInt32(ExcelSheet.UsedRange.Rows.Count.ToString());
                try
                {

                    int iCount = 0;
                    for (int i = 2; i <= iRowCount; i++)
                    {
                        string sValue1 = ((Excel.Range)ExcelSheet.Cells[i, 1]).Text.ToString();
                        string sValue2 = ((Excel.Range)ExcelSheet.Cells[i, 2]).Text.ToString();
                        string sValue3 = ((Excel.Range)ExcelSheet.Cells[i, 3]).Text.ToString();
                        string sSQL = "INSERT INTO MESUSER.SYS_REASON "
       + "(REASON_ID,REASON_CODE,REASON_DESC) "
       + " VALUES "
       + "( '" + sValue1 + "' "
       + " ,'" + sValue2 + "' "
       + " ,'" + sValue3 + "' "
       + " ) ";
                        DataSet dsTemp = ExecuteSQL(sSQL, null);
                    }

                    //progressBar1.Step += 1

                    iCount += 1;
                    double iPer = (iCount * 100) / (iRowCount - 1);
                    // Display the textual value of the ProgressBar in the StatusBar control's first panel.
                    //SajetCommon.Show_Message(iPer.ToString(), 0);
                }

                catch (Exception E)
                {
                    Show_Message(E.Message,0);
                }

            }
            finally
            {
                ExcelClass.Close();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!File.Exists(editFileName.Text))
            {
                return;
            }
            int iRowCount = 1;

            ExportOfficeExcel.ExcelEdit ExcelClass = new ExportOfficeExcel.ExcelEdit();
            try
            {
                ExcelClass.Open(editFileName.Text);
                Excel.Worksheet ExcelSheet = ExcelClass.GetSheet("Sheet1");
                iRowCount = Convert.ToInt32(ExcelSheet.UsedRange.Rows.Count.ToString());
                try
                {

                    int iCount = 0;
                    for (int i = 2; i <= iRowCount; i++)
                    {
                        string sValue1 = ((Excel.Range)ExcelSheet.Cells[i, 1]).Text.ToString();
                        string sValue2 = ((Excel.Range)ExcelSheet.Cells[i, 2]).Text.ToString();
                        string sValue3 = ((Excel.Range)ExcelSheet.Cells[i, 3]).Text.ToString();
                        
                        string sSQL = "INSERT INTO SAJET.SYS_DEFECT "
       + "(DEFECT_ID,DEFECT_CODE,DEFECT_DESC ) "
       + " VALUES "
       + "( '" + sValue1 + "' "
       + " ,'" + sValue2 + "' "
       + " ,'" + sValue3 + "' "
       
       + " ) ";
                        DataSet dsTemp = ExecuteSQL(sSQL, null);
                    }

                    //progressBar1.Step += 1

                    iCount += 1;
                    double iPer = (iCount * 100) / (iRowCount - 1);
                    // Display the textual value of the ProgressBar in the StatusBar control's first panel.
                    //SajetCommon.Show_Message(iPer.ToString(), 0);
                }

                catch (Exception E)
                {
                    Show_Message(E.Message, 0);
                }

            }
            finally
            {
                ExcelClass.Close();
            }

        }
    }
}