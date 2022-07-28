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

        /// <summary>
        /// Sajet 強制回應的顯示訊息視窗
        /// </summary>
        /// <param name="sKeyMsg">訊息內容</param>
        /// <param name="iType">訊息類型（0: Error/ 1: Warning/ 2: Yes-No Question/ 3: Information; default: 3）</param>
        /// <returns></returns>
        public static DialogResult Show_Message(string sKeyMsg, int iType)
        {
            string sXMLFile = Path.GetFileNameWithoutExtension(g_sFileName);

            return ClientUtils.ShowMessage(sKeyMsg, iType, g_sExeName, sXMLFile);
        }

        /// <summary>
        /// 取得翻譯資源
        /// </summary>
        /// <param name="sSearchText">KEY 值</param>
        /// <returns></returns>
        public static string SetLanguage(string sSearchText)
        {
            string sXMLFile;

            string sText;

            sXMLFile = Path.GetFileNameWithoutExtension(g_sFileName); //Dll.xml

            sText = ClientUtils.SetLanguage(sSearchText, g_sExeName, sXMLFile);

            if (sSearchText == sText)
            {
                sXMLFile = g_sExeName; //Program.xml

                sText = ClientUtils.SetLanguage(sSearchText, g_sExeName, sXMLFile);
            }

            return sText;
        }

        /// <summary>
        /// 取得翻譯資源
        /// </summary>
        /// <param name="sSearchText">KEY 值</param>
        /// <param name="iTransType">翻譯來源（1：DLL.xml/ 2：PROGRAM.xml）</param>
        /// <returns></returns>
        public static string SetLanguage(string sSearchText, int iTransType)
        {
            string sXMLFile = "";

            switch (iTransType)
            {
                case 1:  /* Dll.xml */
                    sXMLFile = Path.GetFileNameWithoutExtension(g_sFileName);
                    break;
                case 2:  /* Program.xml */
                    sXMLFile = g_sExeName;
                    break;
            }

            string sText = ClientUtils.SetLanguage(sSearchText, g_sExeName, sXMLFile);

            return sText;
        }

        /// <summary>
        /// 取得翻譯資源
        /// </summary>
        /// <param name="sSearchText">KEY 值</param>
        /// <param name="sDefaultTxt">預設翻譯值</param>
        /// <param name="iTransType">翻譯來源（1：DLL.xml/ 2：PROGRAM.xml）</param>
        /// <returns></returns>
        public static string SetLanguage(string sSearchText, string sDefaultTxt, int iTransType)
        {
            string sText = SetLanguage(sSearchText, iTransType);

            if (sText != sSearchText)
            {
                return sText;
            }
            else
            {
                return sDefaultTxt;
            }
        }

        /// <summary>
        /// 表單控制項翻譯文字
        /// </summary>
        /// <param name="c">表單控制項</param>
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
            string s = @"
SELECT
    PARAM_VALUE
FROM
    SAJET.SYS_BASE
WHERE
    UPPER(PROGRAM) = :PROGRAM
    AND UPPER(PARAM_NAME) = :PARAM_NAME
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PROGRAM", sProgram.ToUpper() },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PARAM_NAME", sParamName.ToUpper() },
            };

            DataSet dsTemp = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                return dsTemp.Tables[0].Rows[0]["PARAM_VALUE"].ToString();
            }
            else
            {
                sErrorMsg = sErrorMsg + sParamName + Environment.NewLine;

                return "";
            }
        }

        public static string GetMaxID(string sTable, string sField, int iIDLength)
        {
            try
            {
                var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "TFIELD", sField },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "TTABLE", sTable },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "TNUM", iIDLength.ToString() },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "TRES", "" },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "T_MAXID", "" },
                };

                var d = ClientUtils.ExecuteProc("SAJET.SJ_GET_MAXID", p.ToArray());

                if (d != null &&
                    d.Tables[0].Rows.Count > 0)
                {
                    string sRes = d.Tables[0].Rows[0]["TRES"].ToString();

                    string sMaxID = d.Tables[0].Rows[0]["T_MAXID"].ToString();

                    if (sRes != "OK")
                    {
                        SajetCommon.Show_Message(sRes, 0);

                        return "0";
                    }

                    return sMaxID;
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                string message = "SAJET.SJ_GET_MAXID" + Environment.NewLine + ex.Message;

                SajetCommon.Show_Message(message, 0);

                return "0";
            }
        }

        public static string GetID(string sTable, string sFieldID, string sFieldName, string sValue)
        {
            return GetID(sTable, sFieldID, sFieldName, sValue, "");
        }

        public static string GetID(string sTable, string sFieldID, string sFieldName, string sValue, string sEnabled)
        {
            //找欄位ID值
            if (string.IsNullOrEmpty(sValue)) return "0";

            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "P_1", sValue },
            };

            string s = $@"
SELECT
    {sFieldID}
FROM
    {sTable}
WHERE
    {sFieldName} = :P_1
";
            if (!string.IsNullOrEmpty(sEnabled))
            {
                s += " AND ENABLED = :ENABLED ";

                p.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "ENABLED", sEnabled });
            }

            s += " AND ROWNUM = 1 ";

            DataSet dsTemp = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                return dsTemp.Tables[0].Rows[0][sFieldID].ToString();
            }
            else
            {
                return "0";
            }
        }

        public static bool CheckEnabled(string sType, string sPrivilege)
        {
            try
            {
                string s = @"
SELECT
    SAJET.SJ_PRIVILEGE_DEFINE(:P_1, :P_2) TENABLED
FROM
    DUAL
";
                var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "P_1", sType },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "P_2", sPrivilege },
                };

                var d = ClientUtils.ExecuteSQL(s, p.ToArray());

                return d != null
                    && d.Tables[0].Rows.Count > 0
                    && d.Tables[0].Rows[0]["TENABLED"].ToString() == "Y";
            }
            catch (Exception ex)
            {
                string message = "Function:SAJET.SJ_PRIVILEGE_DEFINE" + Environment.NewLine + ex.Message;

                SajetCommon.Show_Message(message, 0);

                return false;
            }
        }
    }
}
