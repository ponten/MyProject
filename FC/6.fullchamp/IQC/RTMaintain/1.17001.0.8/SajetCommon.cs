using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace SajetClass
{
    class SajetCommon
    {
        public static string g_sFileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion.ToString();//檔案版本 
        public static string g_sFileName = Path.GetFileName(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileName); //檔案名稱        
        public static string g_sServerName = "SApServer";
        public static string g_sExeName = ClientUtils.fCurrentProject;
        public static void SetLanguageControl(Control c)
        {
            //轉換元件Txt的多國語言
            string sXMLFile = Path.GetFileNameWithoutExtension(g_sFileName);
            ClientUtils.SetLanguage(c, g_sExeName, sXMLFile);
            ClientUtils.SetLanguage(c, g_sExeName, g_sExeName);
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

        public static DataSet ExecuteSQL(string sCommand)
        {
            //return ClientUtils.ExecuteSql(g_sServerName, "CmdTemp", sCommand, true, "");
            object[] param = new object[1];
            param[0] = sCommand;
            object[] backparam = ClientUtils.CallMethod(g_sServerName, "EXECUTE_COMMAND", param);
            return (DataSet)backparam[1];
        }

        public static string GetSysBaseData(string sProgram, string sParamName, ref string sErrorMsg)
        {
            //讀取SYS_BASE設定值
            string sSQL = "";
            sSQL = " SELECT PARAM_VALUE "
                 + "   FROM SAJET.SYS_BASE "
                 + "  WHERE Upper(PROGRAM) = '" + sProgram.ToUpper() + "' "
                 + "    and Upper(PARAM_NAME) = '" + sParamName.ToUpper() + "' ";
            DataSet dsTemp = ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
                return dsTemp.Tables[0].Rows[0]["PARAM_VALUE"].ToString();
            else
            {
                sErrorMsg = sErrorMsg + sParamName + Environment.NewLine;
                return "";
            }
        }

        public static int Get_Privilege(string sUserID, string sExeName, out string sProgram)
        {
            //回傳權限
            int iPrivilege = 0;
            sProgram = "";
            string sFileName = SajetCommon.g_sFileName.ToUpper();
            string sSQL = "SELECT A.PROGRAM,A.FUNCTION "
                        + "FROM SAJET.SYS_PROGRAM_FUN A,SAJET.SYS_PROGRAM_NAME B "
                        + "WHERE Upper(DLL_FILENAME) = '" + sFileName + "' "
                        + "AND A.PROGRAM = B.PROGRAM "
                        + "AND Upper(B.EXE_FILENAME)='" + sExeName.ToUpper() + "' "
                        + "AND ROWNUM = 1  ";
            DataSet dsTemp = ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                sProgram = dsTemp.Tables[0].Rows[0]["PROGRAM"].ToString();
                string sFunction = dsTemp.Tables[0].Rows[0]["FUNCTION"].ToString();

                //使用Server Method
                object[] param = new object[] { sUserID, sProgram, sFunction };
                object[] backparam = ClientUtils.CallMethod(g_sServerName, "CHK_PRIVILEGE", param);
                string sResult = backparam[0].ToString();
                if (sResult != "0")
                {
                    Show_Message(backparam[1].ToString(), 0);
                    return 0;
                }
                iPrivilege = Convert.ToInt32(backparam[1].ToString());
            }
            else
            {
                iPrivilege = 0;
            }
            return iPrivilege;
        }

        public static int Get_OtherPrivilege(string sUserID, string sProgram, string sFunction)
        {
            //用於回傳在程式中單獨功能的權限
            int iPrivilege = 0;
            
            //使用Server Method
            object[] param = new object[] { sUserID, sProgram, sFunction };
            object[] backparam = ClientUtils.CallMethod(g_sServerName, "CHK_PRIVILEGE", param);
            string sResult = backparam[0].ToString();
            if (sResult != "0")
            {
                Show_Message(backparam[1].ToString(), 0);
                return 0;
            }
            iPrivilege = Convert.ToInt32(backparam[1].ToString());
            
            return iPrivilege;
        }
        public static string SetLanguage(string sSearchText)
        {
            string sXMLFile = "";
            string sText = "";

            sXMLFile = Path.GetFileNameWithoutExtension(g_sFileName); //Dll.xml
            sText = ClientUtils.SetLanguage(sSearchText, g_sExeName, sXMLFile);
            if (sSearchText == sText)
            {
                sXMLFile = g_sExeName; //Program.xml
                sText = ClientUtils.SetLanguage(sSearchText, g_sExeName, sXMLFile);
            }
            return sText;
        }
    }
}
