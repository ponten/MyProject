using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace ClassSajetIniFile
{
    class SajetIniFile
    {
        public string ReadIniFile(string sFile, string sSection, string sKey, string sDefault)
        {
            string sReturn = sDefault;
            int iStartLine = -1;
            if (File.Exists(sFile))
            {
                ListBox L1 = new ListBox();
                L1.Items.AddRange(File.ReadAllLines(@sFile, Encoding.Default));
                iStartLine = L1.Items.IndexOf("[" + sSection + "]");
                if (iStartLine > -1)
                {
                    for (int i = iStartLine + 1; i <= L1.Items.Count - 1; i++)
                    {
                        string sData = L1.Items[i].ToString().Trim();
                        if (string.IsNullOrEmpty(sData))
                            continue;
                        if (sData.Substring(0, 1) == "[") //[Section]的結束行數
                            break;

                        int iIndex = sData.IndexOf("=");
                        if (sData.Substring(0, iIndex).ToUpper() == sKey.ToUpper())
                        {
                            sReturn = sData.Substring(iIndex + 1, sData.Length - iIndex - 1);
                            break;
                        }
                    }
                }
                L1.Dispose();
            }
            return sReturn;
        }
        public void WriteIniFile(string sFile, string sSection, string sKey, string sValue)
        {
            StreamWriter sw;
            ListBox L1 = new ListBox();

            if (File.Exists(sFile))
                L1.Items.AddRange(File.ReadAllLines(@sFile, Encoding.Default));

            int iStartLine = L1.Items.IndexOf("[" + sSection + "]");
            if (iStartLine == -1)
            {
                L1.Items.Add("[" + sSection + "]");
                L1.Items.Add(sKey + "=" + sValue);
            }
            else
            {
                int iLine = -1;
                for (int i = iStartLine + 1; i <= L1.Items.Count - 1; i++)
                {
                    string sData = L1.Items[i].ToString().Trim();
                    if (string.IsNullOrEmpty(sData))
                        continue;
                    if (sData.Substring(0, 1) == "[") //[Section]的結束行數
                    {
                        if (string.IsNullOrEmpty(L1.Items[i - 1].ToString().Trim()))
                            iLine = i - 1;
                        else
                            iLine = i;
                        break;
                    }
                    int iIndex = sData.IndexOf("=");
                    if (sData.Substring(0, iIndex).ToUpper() == sKey.ToUpper())
                    {
                        iLine = i;
                        L1.Items.RemoveAt(iLine);
                        break;
                    }
                }
                if (iLine == -1)
                    L1.Items.Add(sKey + "=" + sValue);
                else
                    L1.Items.Insert(iLine, sKey + "=" + sValue);
            }

            //將ListBox中的值寫入檔案
            if (File.Exists(sFile))
                sw = new StreamWriter(sFile, false, Encoding.Default);
            // sw = new StreamWriter(File.Open(sFile, FileMode.OpenOrCreate), Encoding.Default);//File.AppendText(sFile); 
            else
                sw = File.CreateText(sFile);


            for (int i = 0; i <= L1.Items.Count - 1; i++)
            {
                sw.WriteLine(L1.Items[i].ToString());
            }
            sw.Close();
            L1.Dispose(); ;
        }
    }
}
