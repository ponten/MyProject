using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Data.OracleClient;
using System.Xml;
using System.Drawing;

namespace SajetClass
{
    class SajetCommon
    {
        public static string g_sFileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion.ToString();  //檔案版本 
        public static string g_sFileName = Path.GetFileName(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileName); //檔案名稱          
        public static string g_sExeName = ClientUtils.fCurrentProject;
        #region 宇睿修改區
        #region 新舊混用?息?窗
        public static DialogResult Show_Message(string sKeyMsg, int iType)
        {
            //sKeyMsg = SajetCommon.SetLanguage(sKeyMsg);
            //switch (iType)
            //{
            //    case 0:
            //        SajetMessageBox.Show(sKeyMsg, SajetMessageBox.displayIcon.Error, 0);
            //        return DialogResult.OK;
            //    case 1:
            //        SajetMessageBox.Show(sKeyMsg, SajetMessageBox.displayIcon.WarningTriangle, 0);
            //        return DialogResult.OK;
            //    case 2:
            //        return SajetMessageBox.Show(sKeyMsg, SajetMessageBox.displayIcon.Question, SajetMessageBox.displayMessageBoxButton.YesNo);
            //    case 3:
            //        SajetMessageBox.Show(sKeyMsg, SajetMessageBox.displayIcon.Information, 0);
            //        return DialogResult.OK;
            //    default:
            //        SajetMessageBox.Show(sKeyMsg, SajetMessageBox.displayIcon.Information, 0);
            //        return DialogResult.OK;
            //}
            string sXMLFile = Path.GetFileNameWithoutExtension(g_sFileName);
            return ClientUtils.ShowMessage(sKeyMsg, iType, g_sExeName, sXMLFile);
        }



        #endregion

        /// <summary>
        /// ?文字方塊擁有自動完成功能
        /// 啟動方式:將AutoCompleteFunction放於需要啟動的地方，建議為Form Load 或者 Keypress.
        /// 2011/05/20 V1.0 by 宇睿
        /// </summary>
        /// <param name="AutoCompleteKey">自動完成的欄位資料</param>
        /// <param name="DBTable">自動完成的資料庫來源表</param>
        /// <param name="txt">自動完成的文字方塊控制?</param>
        public static void AutoCompleteFunction(string AutoCompleteKeys, string DBTable, TextBox txt)
        {
            txt.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txt.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txt.AutoCompleteCustomSource.Clear();
            string strSQL = "SELECT " + AutoCompleteKeys + " FROM " + DBTable;
            DataSet dsAutoCompleteTemp = ClientUtils.ExecuteSQL(strSQL);
            if (dsAutoCompleteTemp.Tables[0].Rows.Count > 0)
                for (int i = 0; i < dsAutoCompleteTemp.Tables[0].Rows.Count; i++)
                    txt.AutoCompleteCustomSource.Add(dsAutoCompleteTemp.Tables[0].Rows[i][0].ToString());
        }
        #endregion

        public static string SetLanguage(string sSearchText, string sDefaultTxt, int iTransType)
        {
            string sText = SetLanguage(sSearchText, iTransType);
            if (sText != sSearchText)
                return sText;
            else
                return sDefaultTxt;
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
        public static string SetLanguage(string sSearchText, int iTransType)
        {
            string sXMLFile = "";
            switch (iTransType)
            {
                case 1:  //Dll.xml
                    sXMLFile = Path.GetFileNameWithoutExtension(g_sFileName);
                    break;
                case 2:  //Program.xml
                    sXMLFile = g_sExeName;
                    break;
            }
            string sText = ClientUtils.SetLanguage(sSearchText, g_sExeName, sXMLFile);
            return sText;
        }
        public static void SetLanguageControl(Control c)
        {
            //轉換元件Txt的多國語言
            string sXMLFile = Path.GetFileNameWithoutExtension(g_sFileName);
            ClientUtils.SetLanguage(c, g_sExeName, sXMLFile);
            ClientUtils.SetLanguage(c, g_sExeName, g_sExeName);
        }

        public static string GetSysBaseData(string sProgram, string sParamName, ref string sErrorMsg)
        {
            //讀取SYS_BASE設定值
            string strSQL = "";
            strSQL = " SELECT PARAM_VALUE "
                 + "   FROM SAJET.SYS_BASE "
                 + "  WHERE Upper(PROGRAM) = '" + sProgram.ToUpper() + "' "
                 + "    and Upper(PARAM_NAME) = '" + sParamName.ToUpper() + "' ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(strSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
                return dsTemp.Tables[0].Rows[0]["PARAM_VALUE"].ToString();
            else
            {
                sErrorMsg = sErrorMsg + sParamName + Environment.NewLine;
                return "";
            }
        }

        public static string GetMaxID(string sTable, string sField, int iIDLength)
        {
            string sMaxID = "0";
            try
            {
                object[][] Params = new object[5][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TFIELD", sField };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TTABLE", sTable };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TNUM", iIDLength.ToString() };
                Params[3] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                Params[4] = new object[] { ParameterDirection.Output, OracleType.VarChar, "T_MAXID", "" };
                DataSet dsTemp = ClientUtils.ExecuteProc("SAJET.SJ_GET_MAXID", Params);

                string sRes = dsTemp.Tables[0].Rows[0]["TRES"].ToString();
                sMaxID = dsTemp.Tables[0].Rows[0]["T_MAXID"].ToString();

                if (sRes != "OK")
                {
                    SajetCommon.Show_Message(sRes, 0);
                    return "0";
                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("SAJET.SJ_GET_MAXID" + Environment.NewLine + ex.Message, 0);
                return "0";
            }
            return sMaxID;
        }

        public static string GetID(string sTable, string sFieldID, string sFieldName, string sValue)
        {
            return GetID(sTable, sFieldID, sFieldName, sValue, "");
        }
        public static string GetID(string sTable, string sFieldID, string sFieldName, string sValue, string sEnabled)
        {
            //找欄位ID值
            if (string.IsNullOrEmpty(sValue))
                return "0";
            string strSQL = "select " + sFieldID + " from " + sTable + " "
                        + "where " + sFieldName + " = '" + sValue + "' ";
            if (!string.IsNullOrEmpty(sEnabled))
                strSQL = strSQL + " and ENABLED = '" + sEnabled + "' ";
            strSQL += " and Rownum = 1 ";

            DataSet dsTemp = ClientUtils.ExecuteSQL(strSQL);
            if (dsTemp.Tables[0].Rows.Count > 0)
                return dsTemp.Tables[0].Rows[0][sFieldID].ToString();
            else
                return "0";
        }

        public static Image LoadImage(string sFileName)
        {
            string sPath = Application.StartupPath + "\\";
            if (File.Exists(sPath + sFileName))
                return Image.FromFile(sPath + sFileName);
            else
                return null;
        }
    }
}
