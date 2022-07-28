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
        public static string g_sFileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion.ToString();  //�ɮת��� 

        public static string g_sFileName = Path.GetFileName(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileName); //�ɮצW��          

        public static string g_sExeName = ClientUtils.fCurrentProject;

        /// <summary>
        /// Sajet �j��^������ܰT������
        /// </summary>
        /// <param name="sKeyMsg">�T�����e</param>
        /// <param name="iType">�T�������]0: Error/ 1: Warning/ 2: Yes-No Question/ 3: Information; default: 3�^</param>
        /// <returns></returns>
        public static DialogResult Show_Message(string sKeyMsg, int iType)
        {
            string sXMLFile = Path.GetFileNameWithoutExtension(g_sFileName);

            return ClientUtils.ShowMessage(sKeyMsg, iType, g_sExeName, sXMLFile);
        }

        /// <summary>
        /// ���o½Ķ�귽
        /// </summary>
        /// <param name="sSearchText">KEY ��</param>
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
        /// ���o½Ķ�귽
        /// </summary>
        /// <param name="sSearchText">KEY ��</param>
        /// <param name="iTransType">½Ķ�ӷ��]1�GDLL.xml/ 2�GPROGRAM.xml�^</param>
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
        /// ���o½Ķ�귽
        /// </summary>
        /// <param name="sSearchText">KEY ��</param>
        /// <param name="sDefaultTxt">�w�]½Ķ��</param>
        /// <param name="iTransType">½Ķ�ӷ��]1�GDLL.xml/ 2�GPROGRAM.xml�^</param>
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
        /// ��汱�½Ķ��r
        /// </summary>
        /// <param name="c">��汱�</param>
        public static void SetLanguageControl(Control c)
        {
            //�ഫ����Txt���h��y��
            string sXMLFile = Path.GetFileNameWithoutExtension(g_sFileName);

            ClientUtils.SetLanguage(c, g_sExeName, sXMLFile);

            ClientUtils.SetLanguage(c, g_sExeName, g_sExeName);
        }

        public static string GetSysBaseData(string sProgram, string sParamName, ref string sErrorMsg)
        {
            //Ū��SYS_BASE�]�w��
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
            //�����ID��
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
