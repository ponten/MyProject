using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Excel;
using LabelManager2;
using BarTender; //OfficeTools 避免Excel版本不同
using System.Diagnostics;
using System.Threading;
using System.Text.RegularExpressions;
using System.Linq;

namespace PrintLabel
{
    public class Setup
    {
        System.Windows.Forms.ListBox ListMultiParam = new System.Windows.Forms.ListBox();
        string BarTenderTemplate = "BR_DEFAULT_1";

        public bool GetPrintData(string sType, ref System.Windows.Forms.ListBox ListParam, ref System.Windows.Forms.ListBox ListData)
        {
            //查詢Label上需要的參數和值
            string sSQL = " select * from sajet.s_sys_print_data "
                        + " where data_type ='" + sType + "'"
                        + " order by data_order ";
            DataSet dsTempSQL = ClientUtils.ExecuteSQL(sSQL);
            for (int j = 0; j <= dsTempSQL.Tables[0].Rows.Count - 1; j++)
            {
                string sDataSQL = dsTempSQL.Tables[0].Rows[j]["DATA_SQL"].ToString().ToUpper();
                string sIParam = dsTempSQL.Tables[0].Rows[j]["INPUT_PARAM"].ToString().ToUpper().TrimEnd(new Char[] { ';' });
                string sIField = dsTempSQL.Tables[0].Rows[j]["INPUT_FIELD"].ToString().ToUpper().TrimEnd(new Char[] { ';' });
                string sOParam = dsTempSQL.Tables[0].Rows[j]["OUTPUT_PARAM"].ToString().ToUpper().TrimEnd(new Char[] { ';' });
                string[] sInputParam = sIParam.Split(new Char[] { ';' });
                string[] sInputField = sIField.Split(new Char[] { ';' });
                string[] sOutputParam = sOParam.Split(new Char[] { ';' });

                //Detail(例如找Carton中SN_1下的KP)
                string sDataSQL2 = dsTempSQL.Tables[0].Rows[j]["DATA_SQL2"].ToString().ToUpper();
                string sIParam2 = dsTempSQL.Tables[0].Rows[j]["INPUT_PARAM2"].ToString().ToUpper().TrimEnd(new Char[] { ';' });
                string sIField2 = dsTempSQL.Tables[0].Rows[j]["INPUT_FIELD2"].ToString().ToUpper().TrimEnd(new Char[] { ';' });
                string sOParam2 = dsTempSQL.Tables[0].Rows[j]["OUTPUT_PARAM2"].ToString().ToUpper().TrimEnd(new Char[] { ';' });
                string[] sInputParam2 = sIParam2.Split(new Char[] { ';' });
                string[] sInputField2 = sIField2.Split(new Char[] { ';' });
                string[] sOutputParam2 = sOParam2.Split(new Char[] { ';' });

                //替換SQL中的變數(@01,@02)                  
                for (int i = 0; i <= sInputParam.Length - 1; i++)
                {
                    string sParamName = sInputParam[i].ToString(); //SQL中定義的變數(如@01)
                    string sParamField = sInputField[i].ToString(); //變數代表的欄位名字
                    string sParamValue = "";
                    if (j == 0)
                    {
                        //第一筆的變數實際值由程式傳入,並將變數名稱設成INPUT_FIELD中所定義的值
                        if (i > ListData.Items.Count - 1)
                        {
                            ListData.Items.Add(ListData.Items[0].ToString());
                        }
                        sParamValue = ListData.Items[i].ToString();
                        ListParam.Items.Add(sParamField);
                    }
                    else
                    {
                        //由之前已找出的變數來找出值
                        sParamValue = Get_ParamData(sParamField, ListParam, ListData);
                    }

                    if (sDataSQL.IndexOf(sParamName) >= 0)
                        sDataSQL = sDataSQL.Replace(sParamName, " '" + sParamValue.TrimStart('"').TrimEnd('"') + "' ");
                }

                int Row = 0; // 資料總列數
                //int Split_Item = 3; // 滿 n 個項目要換新的一列

                //執行Master SQL,讀取參數和值放入ListBox並傳出,以供Label使用
                DataSet dsMaster = ClientUtils.ExecuteSQL(sDataSQL);
                for (int iRow = 0; iRow < dsMaster.Tables[0].Rows.Count; iRow++)
                {
                    Row++;
                    //int DetailRow;
                    //再繼續往下找Detail=====================================
                    if (sOParam2 != "")
                    {
                        //替換Data SQL2中的變數
                        string sSQL2 = sDataSQL2;
                        for (int k = 0; k < sInputParam2.Length; k++)
                        {
                            string sParamName = sInputParam2[k].ToString(); //SQL中定義的變數(如@01)
                            string sParamField = sInputField2[k].ToString();
                            string sParamValue = "";
                            if (dsMaster.Tables[0].Columns.IndexOf(sParamField) > -1)
                                sParamValue = dsMaster.Tables[0].Rows[iRow][sParamField].ToString();   //從Master中的欄位找值
                            else
                                //20181217 add
                                sParamValue = Get_ParamData(sParamField, ListParam, ListData); //由之前已找出的變數來找出值

                            if (sSQL2.IndexOf(sParamName) >= 0)
                                sSQL2 = sSQL2.Replace(sParamName, " '" + sParamValue.TrimStart('"').TrimEnd('"') + "' ");
                        }
                        //執行SQL,並將欄位傳出供Label使用
                        DataSet dsDetail = ClientUtils.ExecuteSQL(sSQL2);
                        /*if (dsDetail.Tables[0].Rows.Count / Split_Item > 0 &&
                            dsDetail.Tables[0].Rows.Count % Split_Item > 0)
                        {
                            DetailRow = (dsDetail.Tables[0].Rows.Count / Split_Item) + 1;
                        }
                        else
                        {
                            DetailRow = 1;
                        }*/

                        string sVarName = string.Empty;
                        string _data = string.Empty;

                        for (int iRow2 = 0; iRow2 < dsDetail.Tables[0].Rows.Count; iRow2++)
                        {
                            for (int iCol2 = 0; iCol2 < dsDetail.Tables[0].Columns.Count; iCol2++)
                            {
                                if (iCol2 < sOutputParam2.Length)
                                {
                                    string sColumnName2 = dsDetail.Tables[0].Columns[iCol2].ColumnName.ToString();
                                    sVarName = sOutputParam2[iCol2] + Convert.ToString(Row) + "_1";
                                    _data += dsDetail.Tables[0].Rows[iRow2][sColumnName2].ToString().Replace("\"", "\"\"");

                                    /*
                                    if ((iRow2 + 1) % Split_Item == 0)
                                    {
                                        string sColumnName2 = dsDetail.Tables[0].Columns[iCol2].ColumnName.ToString();
                                        string sVarName = sOutputParam2[iCol2] + Convert.ToString(Row) + "_" + Convert.ToString(Split_Item);

                                        if (ListParam.Items.IndexOf(sVarName) == -1)
                                        {
                                            ListParam.Items.Add(sVarName);
                                            string _data = string.Format("\"{0}\"", dsDetail.Tables[0].Rows[iRow2][sColumnName2].ToString().Replace("\"", "\"\""));
                                            ListData.Items.Add(_data);
                                        }

                                        if (iRow2 + 1 < dsDetail.Tables[0].Rows.Count)
                                        {
                                            Row++;
                                        }
                                    }
                                    else
                                    {                                        
                                        string sColumnName2 = dsDetail.Tables[0].Columns[iCol2].ColumnName.ToString();
                                        string sVarName = sOutputParam2[iCol2] + Convert.ToString(Row) + "_" + Convert.ToString((iRow2 + 1) % Split_Item);

                                        if (ListParam.Items.IndexOf(sVarName) == -1)
                                        {
                                            ListParam.Items.Add(sVarName);
                                            string _data = string.Format("\"{0}\"", dsDetail.Tables[0].Rows[iRow2][sColumnName2].ToString().Replace("\"", "\"\""));
                                            ListData.Items.Add(_data);
                                        }
                                    }*/
                                }                                
                            }
                        }

                        if (ListParam.Items.IndexOf(sVarName) == -1)
                        {
                            ListParam.Items.Add(sVarName);
                            _data = string.Format("\"{0}\"", _data);
                            ListData.Items.Add(_data);
                        }
                    }
                    /*
                    else
                    {
                        // 無子項目
                        DetailRow = 1;
                    }*/

                    // Master
                    for (int i = 0; i < dsMaster.Tables[0].Columns.Count; i++)
                    {
                        string sColumnName = dsMaster.Tables[0].Columns[i].ColumnName.ToString();
                        if (sOParam == "")
                        {
                            if (ListParam.Items.IndexOf(sColumnName) == -1)
                            {
                                ListParam.Items.Add(sColumnName);
                                string _data = string.Format("\"{0}\"", dsMaster.Tables[0].Rows[iRow][sColumnName].ToString().Replace("\"", "\"\""));
                                ListData.Items.Add(_data);
                            }
                        }
                        //有設多筆時(如SN_1,CSN_1)
                        else if (i < sOutputParam.Length)
                        {
                            string sVarName = string.Empty;
                            // 根據 Detail 列印幾列判斷要重複幾次，不列印 CODE 與 PROCESS
                            /*if (iRow == 0)
                            {
                                // 第一筆製程
                                sVarName = sOutputParam[i] + Convert.ToString(iRow + 1);
                            }
                            else
                            {
                                if ((DetailRow >= Row && DetailRow > 1) || DetailRow == 1)
                                {
                                    sVarName = sOutputParam[i] + Convert.ToString(Row);
                                }
                                else if (DetailRow > 1)
                                {
                                    int diff = Row - DetailRow + 1;
                                    sVarName = sOutputParam[i] + Convert.ToString(diff);
                                }
                            }*/

                            sVarName = sOutputParam[i] + Convert.ToString(iRow + 1);
                            if (ListParam.Items.IndexOf(sVarName) == -1)
                            {
                                ListParam.Items.Add(sVarName);
                                string _data = string.Format("\"{0}\"", dsMaster.Tables[0].Rows[iRow][sColumnName].ToString().Replace("\"", "\"\""));
                                ListData.Items.Add(_data);
                            }
                        }
                    }
                }

                // 判別適用的檔案
                if (Row > 10)
                {
                    BarTenderTemplate = "BR_DEFAULT_" + ((Row / 10) + 1).ToString();
                }
            }

            return true;
        }

        private string Get_ParamData(string sParam, System.Windows.Forms.ListBox ListParam, System.Windows.Forms.ListBox ListData)
        {
            int iIndex = ListParam.Items.IndexOf(sParam.ToUpper());
            if (iIndex < 0)
                return "";
            else
                return ListData.Items[iIndex].ToString();
        }

        private bool Get_Sample_File(string sExeName, string sFileTitle, string sFileName, string sPrintMethod, string sPrintPort, System.Windows.Forms.ListBox ListParam, System.Windows.Forms.ListBox ListData, out string sSampleFile)
        {
            string sApppath = System.Windows.Forms.Application.StartupPath + "\\" + sExeName + "\\";

            if (Directory.Exists(sApppath + "PrintLabel\\"))
                sApppath += "PrintLabel\\";

            //Print Method:CODESOFT ,BARTENDER,DLL
            //Print Port -(DLL)COM1,COM2,EXCEL
            sSampleFile = "";
            string sSampleFileExt = "";
            if (sPrintMethod.ToUpper() == "CODESOFT")
                sSampleFileExt = ".LAB";
            else if (sPrintMethod.ToUpper() == "BARTENDER")
                sSampleFileExt = ".btw";
            else if (sPrintPort.ToUpper() == "EXCEL")
                sSampleFileExt = ".xlt";
            // else if (sPrintPort.ToUpper() == "BARTENDER")
            else
                sSampleFileExt = ".txt";

            //找範本檔(若sFileName有指定,先找sFileName)
            if (sFileName != "")
            {
                sSampleFile = sApppath + sFileName + sSampleFileExt;
                if (!File.Exists(sSampleFile))
                {
                    return false;
                }
            }
            else
            {
                //找範本檔(依label_file,PART,WO,客戶,機種,DEFAULT順序)
                sSampleFile = sApppath + sFileTitle + Get_ParamData("LABEL_FILE", ListParam, ListData) + sSampleFileExt;
                if (!File.Exists(sSampleFile))
                    sSampleFile = sApppath + sFileTitle + Get_ParamData("PART_NO", ListParam, ListData) + sSampleFileExt;
                if (!File.Exists(sSampleFile))
                    sSampleFile = sApppath + sFileTitle + Get_ParamData("WORK_ORDER", ListParam, ListData) + sSampleFileExt;
                if (!File.Exists(sSampleFile))
                    sSampleFile = sApppath + sFileTitle + Get_ParamData("CUSTOMER_CODE", ListParam, ListData) + sSampleFileExt;
                if (!File.Exists(sSampleFile))
                    sSampleFile = sApppath + sFileTitle + Get_ParamData("MODEL_NAME", ListParam, ListData) + sSampleFileExt;
                if (!File.Exists(sSampleFile))
                    sSampleFile = sApppath + sFileTitle + "DEFAULT" + sSampleFileExt;
                if (!File.Exists(sSampleFile))
                {
                    return false;
                }
            }
            return true;
        }

        private string Get_Bartender_Split_Symbol(string sType)
        {
            string sSQL = " select PARAM_VALUE from SAJET.SYS_BASE "
                        + " where PROGRAM='ALL' "
                        + "   AND PARAM_NAME ='" + sType + "' "
                        + "   AND ROWNUM = 1 ";
            DataSet dsTempSQL = ClientUtils.ExecuteSQL(sSQL);
            if (dsTempSQL.Tables[0].Rows.Count > 0)
            {
                return dsTempSQL.Tables[0].Rows[0]["PARAM_VALUE"].ToString();
            }
            else
                return ",";
        }

        public bool Print_Bartender_DataSource(string sExeName, string sType, string sFileTitle, string sFileName, int iPrintQty, string sPrintMethod, string sPrintPort, System.Windows.Forms.ListBox ListParam, System.Windows.Forms.ListBox ListData, out string sMessage)
        {
            sMessage = "OK";
            string sSampleFile = "";

            System.Windows.Forms.ListBox listValue = new System.Windows.Forms.ListBox();
            listValue.Items.Add(ListData.Items[0]);//以第一筆,取得資料庫資料,找範本檔
            //ClientUtils.ShowMessage(ListData.Items[0].ToString(), 0);
            ListParam.Items.Clear();
            GetPrintData(sType, ref ListParam, ref listValue);
            if (!Get_Sample_File(sExeName, sFileTitle, BarTenderTemplate, sPrintMethod, sPrintPort, ListParam, listValue, out sSampleFile))
            {
                sMessage = "Sample File not Exist" + Environment.NewLine + "(" + sSampleFile + ")" + Environment.NewLine
                         + "Print Method " + sPrintMethod + Environment.NewLine
                         + "Print Port " + sPrintPort;
                return false;
            }
            string sStartPath = System.Windows.Forms.Application.StartupPath;
            FileInfo fileInfo = new FileInfo(sSampleFile);
            string sExt = fileInfo.Extension.ToString().ToUpper();//副檔名
            string sPath = fileInfo.DirectoryName;
            sFileName = Path.GetFileNameWithoutExtension(sSampleFile);
            string sLabelFileName = sSampleFile;
            string sDataSourceFile = sStartPath + "\\" + sExeName + "\\" + sFileName + ".lst";
            string sLabelFieldFile = sStartPath + "\\" + sExeName + "\\" + sFileName + ".dat";
            string sLabelFieldFileDefault = sStartPath + "\\" + sExeName + "\\" + sFileTitle + "DEFAULT.dat";
            string sPrintGoFile = sStartPath + "\\" + sExeName + "\\PrintGo.bat";
            string sBatFile = sStartPath + "\\" + sExeName + "\\PrintLabel.bat";

            if (!File.Exists(sLabelFileName))
            {
                sMessage = "Label File Not Found (btw)" + Environment.NewLine + Environment.NewLine
                          + sLabelFileName;
                return false;
            }
            if (!File.Exists(sLabelFieldFile))
            {
                if (!File.Exists(sLabelFieldFileDefault))
                {
                    sMessage = "Label File Not Found (.dat)" + Environment.NewLine + Environment.NewLine
                               + sLabelFieldFile + Environment.NewLine + Environment.NewLine;
                    if (sLabelFieldFile != sLabelFieldFileDefault)
                    {
                        sMessage = sMessage
                                 + "OR " + Environment.NewLine + Environment.NewLine
                                 + sLabelFieldFileDefault;
                    }
                    return false;
                }
                else
                {
                    sLabelFieldFile = sLabelFieldFileDefault;
                }
            }
            if (File.Exists(sDataSourceFile))
                File.Delete(sDataSourceFile);
            string sSplitValue = Get_Bartender_Split_Symbol(sType);
            System.Windows.Forms.ListBox listField = LoadFileHeader(sLabelFieldFile, ref sMessage, sSplitValue);
            if (!string.IsNullOrEmpty(sMessage))
                return false;
            //==================表頭資料================================
            string sData = string.Empty;
            int iIndex = -1;
            for (int j = 0; j <= listField.Items.Count - 1; j++)
            {
                if (sSplitValue == "1")
                {
                    sData = sData + listField.Items[j].ToString() + (char)Keys.Tab;
                }
                else
                {
                    sData = sData + listField.Items[j].ToString() + sSplitValue;
                }
            }

            if (!string.IsNullOrEmpty(sData))
            {
                if (sSplitValue == "1")
                    sData = sData.TrimEnd((char)Keys.Tab);
                else
                    sData = sData.Substring(0, sData.Length - 1);
            }
            WriteToTxt(sDataSourceFile, sData);
            //
            //====================資料內容===============================

            if (sType == "PAGE") //測試頁
            {
                sData = string.Empty;
                for (int j = 0; j <= listField.Items.Count - 1; j++)
                {
                    string sField = listField.Items[j].ToString();
                    iIndex = ListParam.Items.IndexOf(sField);
                    if (iIndex >= 0)
                    {
                        if (sSplitValue == "1")
                        {
                            sData = sData + ListData.Items[iIndex].ToString() + (char)Keys.Tab;
                        }
                        else
                        {
                            sData = sData + ListData.Items[iIndex].ToString() + sSplitValue;
                        }
                    }
                    else
                    {
                        if (sSplitValue == "1")
                        {
                            sData += (char)Keys.Tab;
                        }
                        else
                        {
                            sData += sSplitValue;//若找不到則給空值
                        }
                    }
                }

                if (!string.IsNullOrEmpty(sData))
                {
                    if (sSplitValue == "1")
                        sData = sData.TrimEnd((char)Keys.Tab);
                    else
                        sData = sData.Substring(0, sData.Length - 1);
                }
                WriteToTxt(sDataSourceFile, sData);
            }
            else
            {
                for (int i = 0; i <= ListData.Items.Count - 1; i++)
                {
                    if (i > 0) //第一筆已經找過GetPrintData了
                    {
                        ListParam.Items.Clear();
                        listValue.Items.Clear();
                        listValue.Items.Add(ListData.Items[i]);
                        GetPrintData(sType, ref ListParam, ref listValue);
                    }

                    #region 客製，工單號碼末 3 碼加上 '#' 字號分隔

                    WrokOrderWithFixes(i, ref ListParam, ref listValue);

                    #endregion

                    sData = string.Empty;
                    for (int j = 0; j <= listField.Items.Count - 1; j++)
                    {
                        string sField = listField.Items[j].ToString();
                        iIndex = ListParam.Items.IndexOf(sField);
                        if (iIndex >= 0)
                        {
                            if (sSplitValue == "1")
                            {
                                sData = sData + listValue.Items[iIndex].ToString() + (char)Keys.Tab;
                            }
                            else
                            {
                                sData = sData + listValue.Items[iIndex].ToString() + sSplitValue;
                            }
                        }
                        else
                        {
                            if (sSplitValue == "1")
                            {
                                sData += (char)Keys.Tab;
                            }
                            else
                            {
                                sData += sSplitValue;//若找不到則給空值
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(sData))
                    {
                        if (sSplitValue == "1")
                            sData = sData.TrimEnd((char)Keys.Tab);
                        else
                            sData = sData.Substring(0, sData.Length - 1);
                    }
                    WriteToTxt(sDataSourceFile, sData);
                }
            }
            string sPrintCommand = LoadBatFile(sBatFile, ref sMessage);
            if (!string.IsNullOrEmpty(sMessage))
                return false;

            StringBuilder sbPrintCommand = new StringBuilder();

            string cmd
                = sPrintCommand
                .Replace("@PATH1", '"' + Path.GetDirectoryName(sLabelFileName) + Path.DirectorySeparatorChar + BarTenderTemplate + ".btw" + '"')
                .Replace("@PATH2", '"' + sDataSourceFile + '"')
                .Replace("@QTY", iPrintQty.ToString());

            sbPrintCommand.AppendLine(cmd);

            //add by Owen 2013/01/10 新增 若檔名為 BS_DEFAUT.BTW 該目錄下同時有 BS_DEFAUT_1.BTW BS_DEFAUT_2.BTW 則也會印
            //foreach (string item in getGroupSampleFile(sLabelFileName))
            //{
            //    sbPrintCommand.AppendLine(sPrintCommand.Replace("@PATH1", '"' + item + '"').Replace("@PATH2", '"' + sDataSourceFile + '"').Replace("@QTY", iPrintQty.ToString()));
            //}

            // add by Owen End

            //add by rita 2010/09/15 因為滿箱滿棧板會同時印,各自呼叫WINEXEC,在箱號還沒印完時,第棧板就會呼叫,
            //可能造成箱號印完後把Bartender關閉,第棧板無法列印的問題,所以加入下面判斷
            bool bResult = true;
            bool bTimeOut = false;
            DateTime dtNow = DateTime.Now;
            while (bResult && (!bTimeOut))
            {
                try
                {
                    bResult = false;
                    Process[] ps = Process.GetProcesses();
                    for (int i = 0; i <= ps.Length - 1; i++)
                    {
                        if (ps[i].ProcessName.ToUpper() == "BARTEND")
                        {
                            bResult = true;
                        }
                    }
                    if (bResult) //Bartender還在記憶體中
                    {
                        TimeSpan t = DateTime.Now - dtNow;
                        if (t.TotalSeconds > 60)
                        {
                            bTimeOut = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    sMessage = ex.Message;
                    return false;
                }
            }
            if (bTimeOut)
            {
                sMessage = "Print " + sType + "  Label TimeOut (60 Seconds)";
                return false;
            }
            //===============================================================================
            //  原本的 WriteToPrintGo(sPrintGoFile, sPrintCommand);
            WriteToPrintGo(sPrintGoFile, sbPrintCommand.ToString());
            int iExe = WinExec(sPrintGoFile, 0);
            sMessage = "OK";
            return true;
        }
        public bool Print_Bartender_DataSource_Single(string sExeName, string sType, string sFileTitle, string sFileName, int iPrintQty, string sPrintMethod, string sPrintPort, System.Windows.Forms.ListBox ListParam, System.Windows.Forms.ListBox ListData, out string sMessage)
        {
            sMessage = "OK";
            //找範本檔
            string sSampleFile = "";
            if (!Get_Sample_File(sExeName, sFileTitle, sFileName, sPrintMethod, sPrintPort, ListParam, ListData, out sSampleFile))
            {
                sMessage = "Sample File not Exist" + Environment.NewLine + "(" + sSampleFile + ")" + Environment.NewLine
                         + "Print Method " + sPrintMethod + Environment.NewLine
                         + "Print Port " + sPrintPort;
                return false;
            }

            string sStartPath = System.Windows.Forms.Application.StartupPath;
            FileInfo fileInfo = new FileInfo(sSampleFile);
            string sExt = fileInfo.Extension.ToString().ToUpper();//副檔名
            string sPath = fileInfo.DirectoryName;
            sFileName = Path.GetFileNameWithoutExtension(sSampleFile);
            string sLabelFileName = sSampleFile;
            string sDataSourceFile = sStartPath + "\\" + sExeName + "\\" + sFileName + ".lst";
            string sLabelFieldFile = sStartPath + "\\" + sExeName + "\\" + sFileName + ".dat";
            string sLabelFieldFileDefault = sStartPath + "\\" + sExeName + "\\" + sFileTitle + "DEFAULT.dat";
            string sPrintGoFile = sStartPath + "\\" + sExeName + "\\PrintGo.bat";
            string sBatFile = sStartPath + "\\" + sExeName + "\\PrintLabel.bat";

            if (!File.Exists(sLabelFileName))
            {
                sMessage = "Label File Not Found (btw)" + Environment.NewLine + Environment.NewLine
                          + sLabelFileName;
                return false;
            }
            if (!File.Exists(sLabelFieldFile))
            {
                if (!File.Exists(sLabelFieldFileDefault))
                {
                    sMessage = "Label File Not Found (.dat)" + Environment.NewLine + Environment.NewLine
                               + sLabelFieldFile + Environment.NewLine + Environment.NewLine
                               + "OR " + Environment.NewLine + Environment.NewLine
                               + sLabelFieldFileDefault;

                    return false;
                }
                else
                {
                    sLabelFieldFile = sLabelFieldFileDefault;
                }
            }
            if (File.Exists(sDataSourceFile))
                File.Delete(sDataSourceFile);
            string sSplitType = "N/A";
            if (sFileTitle == "PKIN_")
            {
                sSplitType = "1";
            }

            System.Windows.Forms.ListBox listField = LoadFileHeader(sLabelFieldFile, ref sMessage, sSplitType);
            if (!string.IsNullOrEmpty(sMessage))
                return false;
            //==================表頭資料================================
            string sData = string.Empty;
            int iIndex = -1;
            for (int j = 0; j <= listField.Items.Count - 1; j++)
            {
                if (sSplitType == "1")
                {
                    sData = sData + listField.Items[j].ToString() + (char)Keys.Tab;
                }
                else
                {
                    sData = sData + listField.Items[j].ToString() + ",";
                }
            }

            if (!string.IsNullOrEmpty(sData))
            {
                if (sSplitType == "1")
                    sData = sData.Trim((char)Keys.Tab);
                else
                    sData = sData.Substring(0, sData.Length - 1);
            }
            WriteToTxt(sDataSourceFile, sData);
            //
            //====================資料內容===============================
            System.Windows.Forms.ListBox listValue = new System.Windows.Forms.ListBox();

            if (sType == "PAGE") //測試頁
            {
                sData = string.Empty;
                for (int j = 0; j <= listField.Items.Count - 1; j++)
                {
                    string sField = listField.Items[j].ToString();
                    iIndex = ListParam.Items.IndexOf(sField);
                    if (iIndex >= 0)
                    {
                        if (sSplitType == "1")
                        {
                            sData = sData + ListData.Items[iIndex].ToString() + (char)Keys.Tab;
                        }
                        else
                        {
                            sData = sData + ListData.Items[iIndex].ToString() + ",";
                        }
                    }
                    else
                    {
                        if (sSplitType == "1")
                        {
                            sData += (char)Keys.Tab;
                        }
                        else
                        {
                            sData += ",";//若找不到則給空值
                        }
                    }
                }

                if (!string.IsNullOrEmpty(sData))
                {
                    if (sSplitType == "1")
                        sData = sData.Trim((char)Keys.Tab);
                    else
                        sData = sData.Substring(0, sData.Length - 1);
                }
                WriteToTxt(sDataSourceFile, sData);
            }
            else
            {
                sData = string.Empty;
                for (int j = 0; j <= listField.Items.Count - 1; j++)
                {
                    string sField = listField.Items[j].ToString();
                    iIndex = ListParam.Items.IndexOf(sField);
                    if (iIndex >= 0)
                    {
                        if (sSplitType == "1")
                        {
                            sData = sData + ListData.Items[iIndex].ToString() + (char)Keys.Tab;
                        }
                        else
                        {
                            sData = sData + ListData.Items[iIndex].ToString() + ",";
                        }
                    }
                    else
                    {
                        if (sSplitType == "1")
                        {
                            sData += (char)Keys.Tab;
                        }
                        else
                        {
                            sData += ",";//若找不到則給空值
                        }
                    }
                }
                if (!string.IsNullOrEmpty(sData))
                {
                    if (sSplitType == "1")
                        sData = sData.Trim((char)Keys.Tab);
                    else
                        sData = sData.Substring(0, sData.Length - 1);
                }
                WriteToTxt(sDataSourceFile, sData);
            }
            string sPrintCommand = LoadBatFile(sBatFile, ref sMessage);
            if (!string.IsNullOrEmpty(sMessage))
                return false;
            sPrintCommand = sPrintCommand.Replace("@PATH1", '"' + sLabelFileName + '"');
            sPrintCommand = sPrintCommand.Replace("@PATH2", '"' + sDataSourceFile + '"');
            sPrintCommand = sPrintCommand.Replace("@QTY", iPrintQty.ToString());
            //add by rita 2010/09/15 因為滿箱滿棧板會同時印,各自呼叫WINEXEC,在箱號還沒印完時,第棧板就會呼叫,
            //可能造成箱號印完後把Bartender關閉,第棧板無法列印的問題,所以加入下面判斷
            bool bResult = true;
            bool bTimeOut = false;
            DateTime dtNow = DateTime.Now;
            while (bResult && (!bTimeOut))
            {
                try
                {
                    bResult = false;
                    Process[] ps = Process.GetProcesses();
                    for (int i = 0; i <= ps.Length - 1; i++)
                    {
                        if (ps[i].ProcessName.ToUpper() == "BARTEND")
                        {
                            bResult = true;
                        }
                    }
                    if (bResult) //Bartender還在記憶體中
                    {
                        TimeSpan t = DateTime.Now - dtNow;
                        if (t.TotalSeconds > 60)
                        {
                            bTimeOut = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    sMessage = ex.Message;
                    return false;
                }
            }
            if (bTimeOut)
            {
                sMessage = "Print " + sType + "  Label TimeOut (60 Seconds)";
                return false;
            }
            //===============================================================================
            WriteToPrintGo(sPrintGoFile, sPrintCommand);
            int iExe = WinExec(sPrintGoFile, 0);
            sMessage = "OK";
            return true;
        }

        public bool Print(string sExeName, string sType, string sFileTitle, string sFileName, int iPrintQty, string sPrintMethod, string sPrintPort, System.Windows.Forms.ListBox ListParam, System.Windows.Forms.ListBox ListData, out string sMessage)
        {
            bool bResult = true;
            sMessage = "OK";
            //找範本檔
            string sSampleFile = "";
            if (!Get_Sample_File(sExeName, sFileTitle, sFileName, sPrintMethod, sPrintPort, ListParam, ListData, out sSampleFile))
            {
                sMessage = "Sample File not Exist" + Environment.NewLine + "(" + sSampleFile + ")" + Environment.NewLine
                         + "Print Method " + sPrintMethod + Environment.NewLine
                         + "Print Port " + sPrintPort;
                return false;
            }

            //依不同方式列印
            if (sPrintMethod.ToUpper() == "CODESOFT")
                bResult = Print_CodeSoft(sSampleFile, iPrintQty, sPrintPort, ListParam, ListData, ref sMessage);
            else if (sPrintMethod.ToUpper() == "BARTENDER") // Bartender分二種方式
            {
                if (sPrintPort.ToUpper() == "STANDARD")
                {
                    bResult = Print_BarTender_Standard(sSampleFile, iPrintQty, sPrintPort, ListParam, ListData, ref sMessage);
                }
            }
            else if (sPrintMethod.ToUpper() == "DLL") // 非CodeSofr與Bartender
            {
                //==供後續Excel或Txt中ReplaceNull時使用
                ListMultiParam.Items.Clear();
                string sSQL = "select * from sajet.s_sys_print_data "
                            + "where data_type = '" + sType + "' "
                            + "and (OUTPUT_PARAM is not null or OUTPUT_PARAM2 is not null) "
                            + "order by DATA_ORDER";
                DataSet dsTempSQL = ClientUtils.ExecuteSQL(sSQL);
                for (int i = 0; i <= dsTempSQL.Tables[0].Rows.Count - 1; i++)
                {
                    string sOUTPUT_PARAM = dsTempSQL.Tables[0].Rows[i]["OUTPUT_PARAM"].ToString().ToUpper().TrimEnd(new Char[] { ';' });
                    string sOUTPUT_PARAM2 = dsTempSQL.Tables[0].Rows[i]["OUTPUT_PARAM2"].ToString().ToUpper().TrimEnd(new Char[] { ';' });
                    string[] sOutParam = sOUTPUT_PARAM.Split(new Char[] { ';' });
                    string[] sOutParam2 = sOUTPUT_PARAM2.Split(new Char[] { ';' });

                    if (sOUTPUT_PARAM != "")
                    {
                        for (int j = 0; j <= sOutParam.Length - 1; j++)
                        {
                            ListMultiParam.Items.Add(sOutParam[j].ToString());
                        }
                    }
                    if (sOUTPUT_PARAM2 != "")
                    {
                        for (int j = 0; j <= sOutParam2.Length - 1; j++)
                        {
                            ListMultiParam.Items.Add(sOutParam2[j].ToString());
                        }
                    }
                }
                //====
                if (sPrintPort.ToUpper() == "EXCEL")
                    bResult = Print_Excel(sSampleFile, iPrintQty, sPrintPort, ListParam, ListData, out sMessage);
                else
                    bResult = Print_DLL(sSampleFile, iPrintQty, sPrintPort, ListParam, ListData, out sMessage);
            }
            return bResult;
        }

        public bool Print_MultiLabel(string sExeName, string sType, string sFileTitle, string sFileName, int iPrintQty, string sPrintMethod, string sPrintPort, System.Windows.Forms.ListBox ListParam, System.Windows.Forms.ListBox ListData, out string sMessage)
        {
            //===用於一張Label中有多個號碼時使用====
            bool bResult = true;
            sMessage = "OK";
            //找範本檔
            string sSampleFile = "";
            if (!Get_Sample_File(sExeName, sFileTitle, sFileName, sPrintMethod, sPrintPort, ListParam, ListData, out sSampleFile))
            {
                sMessage = "Sample File not Exist" + Environment.NewLine + "(" + sSampleFile + ")" + Environment.NewLine
                         + "Print Method " + sPrintMethod + Environment.NewLine
                         + "Print Port " + sPrintPort;
                return false;
            }

            //依不同方式列印
            if (sPrintMethod.ToUpper() == "CODESOFT")
                bResult = Print_CodeSoft(sSampleFile, iPrintQty, sPrintPort, ListParam, ListData, ref sMessage);
            else if (sPrintMethod.ToUpper() == "BARTENDER")
            {
                if (sPrintPort.ToUpper() == "STANDARD")
                {
                    bResult = Print_BarTender_Standard(sSampleFile, iPrintQty, sPrintPort, ListParam, ListData, ref sMessage);
                }
            }
            else if (sPrintMethod.ToUpper() == "DLL")
            {
                //==供後續Excel或Txt中ReplaceNull時使用
                ListMultiParam.Items.Clear();
                ListMultiParam.Items.Add(sType.ToUpper() + "_");
                //====
                if (sPrintPort.ToUpper() == "EXCEL")
                    bResult = Print_Excel(sSampleFile, iPrintQty, sPrintPort, ListParam, ListData, out sMessage);
                else
                    bResult = Print_DLL(sSampleFile, iPrintQty, sPrintPort, ListParam, ListData, out sMessage);
            }
            return bResult;
        }

        public bool Print_TestPage(string sExeName, string sDefultText, string sFileTitle, string sFileName, int iPrintQty, string sPrintMethod, string sPrintPort, System.Windows.Forms.ListBox ListParam, System.Windows.Forms.ListBox ListData, out string sMessage)
        {
            //===列印測試頁使用====
            bool bResult = true;
            sMessage = sDefultText;
            //找範本檔
            string sSampleFile = "";
            if (!Get_Sample_File(sExeName, sFileTitle, sFileName, sPrintMethod, sPrintPort, ListParam, ListData, out sSampleFile))
            {
                sMessage = "Sample File not Exist" + Environment.NewLine + "(" + sSampleFile + ")" + Environment.NewLine
                         + "Print Method " + sPrintMethod + Environment.NewLine
                         + "Print Port " + sPrintPort;

                return false;
            }

            //依不同方式列印
            if (sPrintMethod.ToUpper() == "CODESOFT")
                bResult = Print_CodeSoft(sSampleFile, iPrintQty, sPrintPort, ListParam, ListData, ref sMessage);
            else if (sPrintMethod.ToUpper() == "BARTENDER")
            {
                if (sPrintPort.ToUpper() == "STANDARD")
                {
                    bResult = Print_BarTender_Standard(sSampleFile, iPrintQty, sPrintPort, ListParam, ListData, ref sMessage);
                }
                else if (sPrintPort.ToUpper() == "DATASOURCE")
                {
                    if (!string.IsNullOrEmpty(sSampleFile))
                    {
                        sSampleFile = Path.GetFileNameWithoutExtension(sSampleFile);
                    }
                    bResult = Print_Bartender_DataSource(sExeName, sDefultText, sFileTitle, sSampleFile, iPrintQty, sPrintMethod, sPrintPort, ListParam, ListData, out sMessage);
                }
            }
            else if (sPrintMethod.ToUpper() == "DLL")
            {
                if (sPrintPort.ToUpper() == "EXCEL")
                    bResult = Print_Excel(sSampleFile, iPrintQty, sPrintPort, ListParam, ListData, out sMessage);
                else
                    bResult = Print_DLL(sSampleFile, iPrintQty, sPrintPort, ListParam, ListData, out sMessage);
            }
            return bResult;
        }

        //=============CodeSoft=============================================
        private LabelManager2.ApplicationClass lbl;

        private void Open_CodeSoft()
        {
            if (lbl == null)
                lbl = new LabelManager2.ApplicationClass();
        }

        private void Close_CodeSoft()
        {
            lbl.Quit();
        }

        private bool Print_CodeSoft(string sSampleFile, int iPrintQty, string sCodeSoftVer, System.Windows.Forms.ListBox ListParam, System.Windows.Forms.ListBox ListData, ref string sMessage)
        {
            //ApplicationClass lbl = new ApplicationClass();            

            List<string> lSampleFiles = getGroupSampleFile(sSampleFile);
            foreach (string SampleFiles in lSampleFiles)
            {
                try
                {
                    lbl.Documents.Open(@SampleFiles, false); // 開啟label文件
                    Document doc = lbl.ActiveDocument;
                    //先讀取Label內有定義的參數名稱(sLabelParam)
                    string[] sLabelParam = new string[doc.Variables.FormVariables.Count];
                    //for (int i = 1; i <= doc.Variables.Count; i++)
                    for (int i = 1; i <= doc.Variables.FormVariables.Count; i++)
                    {
                        sLabelParam[i - 1] = doc.Variables.FormVariables.Item(i).Name;
                    }

                    //傳參數值給label                 
                    for (int i = 0; i <= sLabelParam.Length - 1; i++)
                    {
                        string sParam = sLabelParam[i].ToString();
                        string sVarValue = Get_ParamData(sParam, ListParam, ListData);
                        if (sMessage != "OK" && sVarValue == "") //列印測試頁
                            sVarValue = sMessage;
                        doc.Variables.FormVariables.Item(sParam).Value = sVarValue; //給參數值                        
                    }
                    doc.PrintDocument(iPrintQty);  //根據列印數量列印

                    sMessage = "OK";
                }
                catch (Exception ex)
                {
                    sMessage = ex.Message;
                    return false;
                }
                finally
                {
                    //lbl.Quit();   //退出
                }
            }
            return true;
        }

        //=============DLL=============================================
        [DllImport("kernel32.dll")]
        public static extern int WinExec(string exeName, int operType);

        private bool Print_DLL(string sSampleFile, int iPrintQty, string sPrintPort, System.Windows.Forms.ListBox ListParam, System.Windows.Forms.ListBox ListData, out string sMessage)
        {
            sMessage = "OK";

            List<string> lSampleFiles = getGroupSampleFile(sSampleFile);
            foreach (string SampleFiles in lSampleFiles)
            {
                //替換參數值
                string sFileText = File.ReadAllText(@SampleFiles);
                for (int i = 0; i <= ListParam.Items.Count - 1; i++)
                {
                    string sPrintParam = "%" + ListParam.Items[i].ToString() + "%";
                    int iIndex = sFileText.IndexOf(sPrintParam);
                    if (iIndex > -1)
                        sFileText = sFileText.Replace(sPrintParam, ListData.Items[i].ToString());
                }

                //將沒用到的%SN_1%,....改為空值
                for (int i = 0; i <= ListMultiParam.Items.Count - 1; i++)
                {
                    sFileText = ReplaceNull(sFileText, "%" + ListMultiParam.Items[i].ToString());
                }

                //列印===
                string sFileName = System.Windows.Forms.Application.StartupPath + "\\PrintTemp";
                sFileName = sFileName + Convert.ToString(ListParam.Items.Count % 20) + ".txt";
                File.WriteAllText(sFileName, sFileText);
                //string sFile = "PRINT >" + sPrintPort + " " + sFileName;
                for (int i = 1; i <= iPrintQty; i++)
                {
                    SendCommand(sPrintPort, sFileName);
                    //WinExec(sFile, 0);
                }
                File.Delete(sFileName);
            }

            return true;
        }

        private void SendCommand(string sPort, string sFileName)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "print";
            startInfo.Arguments = "/d:" + sPort + " \"" + sFileName + "\"";
            //startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            Process p = Process.Start(startInfo);
            p.WaitForExit();
            p.Close();
        }

        private string ReplaceNull(string sFileText, string sParam)
        {
            //將沒用到的%SN_1%,....改為空值
            string sResult = sFileText;
            int iStartIndex = sResult.IndexOf(sParam);
            while (iStartIndex > -1)
            {
                int iEndIndex = sResult.IndexOf("%", iStartIndex + 1, sResult.Length - iStartIndex - 1);
                string sReplaceParam = sResult.Substring(iStartIndex, iEndIndex - iStartIndex + 1);
                sResult = sResult.Replace(sReplaceParam, "");
                iStartIndex = sResult.IndexOf(sParam);
            }
            return sResult;
        }

        //=============Excel=============================================     
        //private Microsoft.Office.Interop.Excel.Application ExcelApp;
        //private Microsoft.Office.Interop.Excel.Workbooks ExcelWorkbooks;
        //private Microsoft.Office.Interop.Excel.Workbook ExcelWorkbook;
        //private Microsoft.Office.Interop.Excel.Worksheet ExcelWorksheet;

        private Excel.Application ExcelApp;
        private Excel.Workbooks ExcelWorkbooks;
        private Excel.Workbook ExcelWorkbook;
        private Excel.Worksheet ExcelWorksheet;
        private void Open_Excel()
        {
            ExcelApp = new Excel.Application();
        }

        private void Close_Excel()
        {
            ExcelApp.Quit();
            NAR(ExcelWorksheet);
            NAR(ExcelWorkbook);
            NAR(ExcelWorkbooks);
            NAR(ExcelApp);
            GC.Collect();
        }

        private void NAR(object o)
        {
            //為了解決記憶體無法釋放
            try
            {
                if (o != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(o);
            }
            finally
            {
                o = null;
            }
        }

        private bool Print_Excel(string sSampleFile, int iPrintQty, string sPrintPort, System.Windows.Forms.ListBox ListParam, System.Windows.Forms.ListBox ListData, out string sMessage)
        {
            sMessage = "OK";

            List<string> lstSampleFiles = getGroupSampleFile(sSampleFile);

            foreach (string currentSampleFile in lstSampleFiles)
            {
                try
                {
                    ExcelWorkbooks = ExcelApp.Workbooks;
                    //ExcelWorkbook = ExcelWorkbooks.Open(sSampleFile, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing , Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    ExcelWorkbook = ExcelWorkbooks.Add(currentSampleFile);
                    ExcelWorksheet = (Excel.Worksheet)ExcelWorkbook.Worksheets[1]; //第一個Sheet

                    //替換參數值
                    int iRow = ExcelWorksheet.UsedRange.Rows.Count + 1;
                    int iCol = ExcelWorksheet.UsedRange.Columns.Count + 1;
                    for (int i = 1; i <= iCol; i++)
                    {
                        for (int j = 1; j <= iRow; j++)
                        {
                            string sFileText = ExcelWorksheet.get_Range(ExcelWorksheet.Cells[j, i], ExcelWorksheet.Cells[j, i]).Text.ToString();
                            //                    
                            if (sFileText.IndexOf("%") > -1)
                            {
                                for (int k = 0; k <= ListParam.Items.Count - 1; k++)
                                {
                                    string sPrintParam = "%" + ListParam.Items[k].ToString() + "%";
                                    int iIndex = sFileText.IndexOf(sPrintParam);
                                    if (iIndex > -1)
                                    {
                                        sFileText = sFileText.Replace(sPrintParam, ListData.Items[k].ToString());
                                        if (sFileText.IndexOf("%") == -1)
                                        {
                                            break;
                                        }
                                    }
                                }
                                //將沒用到的%SN_1%,....改為空值
                                if (sFileText.IndexOf("%") > -1)
                                {
                                    for (int k = 0; k <= ListMultiParam.Items.Count - 1; k++)
                                    {
                                        sFileText = ReplaceNull(sFileText, "%" + ListMultiParam.Items[k].ToString());
                                    }
                                }
                                ExcelWorksheet.Cells[j, i] = sFileText;
                            }
                        }
                    }
                    //列印            
                    ExcelWorkbook.PrintOut(1, Type.Missing, iPrintQty, false, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                    //Save
                    //ExcelWorkbook.SaveAs(FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);                      
                }
                catch (Exception ex)
                {
                    sMessage = ex.Message;
                    return false;
                }
                finally
                {
                    ExcelWorkbook.Close(false, false, false);
                    ExcelWorkbooks.Close();
                }
            }
            return true;
        }

        //=============BarTender=============================================
        private BarTender.ApplicationClass bt;

        private void Open_BarTender()
        {
            if (bt == null)
                bt = new BarTender.ApplicationClass();
        }

        private void Close_BarTender()
        {
            bt.Quit(BarTender.BtSaveOptions.btDoNotSaveChanges);
        }

        private bool Print_BarTender_Standard(string sSampleFile, int iPrintQty, string sCodeSoftVer, System.Windows.Forms.ListBox ListParam, System.Windows.Forms.ListBox ListData, ref string sMessage)
        {
            //ApplicationClass lbl = new ApplicationClass();   

            List<string> lstSampleFiles = getGroupSampleFile(sSampleFile);
            foreach (string sampleFile in lstSampleFiles)
            {
                try
                {
                    // 開啟label文件
                    BarTender.Format btFormat = bt.Formats.Open(sampleFile, false, "");
                    //傳參數值給label                 
                    foreach (SubString str in btFormat.NamedSubStrings)
                    {
                        string sParam = str.Name;
                        string sVarValue = Get_ParamData(sParam, ListParam, ListData);
                        if (sMessage != "OK" && sVarValue == "") //列印測試頁
                            sVarValue = sMessage;
                        btFormat.SetNamedSubStringValue(sParam, sVarValue); //給參數值
                    }
                    btFormat.PrintSetup.IdenticalCopiesOfLabel = iPrintQty;
                    btFormat.PrintOut(false, false);
                    btFormat.Close(BarTender.BtSaveOptions.btDoNotSaveChanges);
                    sMessage = "OK";
                }
                catch (Exception ex)
                {
                    sMessage = ex.Message;
                    return false;
                }
                finally
                {
                    //lbl.Quit();   //退出
                }
            }
            return true;
        }

        private System.Windows.Forms.ListBox LoadFileHeader(string sFile, ref string sMessage, string sSplitType)
        {
            sMessage = string.Empty;
            System.Windows.Forms.ListBox ListParams = new System.Windows.Forms.ListBox();
            if (!File.Exists(sFile))
            {
                sMessage = "File not exist - " + sFile;
            }
            else
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(sFile);
                try
                {
                    char keySplit = (char)Keys.Tab;
                    char[] sKeyValue = sSplitType.ToCharArray();
                    string[] sValue;
                    if (sSplitType == "1")
                    {
                        sValue = sr.ReadLine().Trim().Split(new Char[] { keySplit });
                    }
                    else
                    {
                        sValue = sr.ReadLine().Trim().Split(new Char[] { sKeyValue[0] });
                    }

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

        public void WriteToTxt(string sFile, string sData)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(sFile, true, Encoding.UTF8);
                sw.WriteLine(sData);
            }
            finally
            {
                if (sw != null)
                    sw.Close();
            }
        }

        public void WriteToPrintGo(string sFile, string sData)
        {
            try
            {
                if (File.Exists(sFile))
                {
                    File.Delete(sFile);
                }
                System.IO.File.AppendAllText(sFile, sData);
            }
            finally
            {
            }
        }

        public string LoadBatFile(string sFile, ref string sMessage, string program = "Barcode Center")
        {
            sMessage = string.Empty;
            string sValue = string.Empty;
            if (!File.Exists(sFile))
            {
                string sSQL = $@"
SELECT
    *
FROM
    sajet.sys_base
WHERE
    program = '{program}'
    AND param_name = 'Bartender Print Command'
    AND ROWNUM = 1
";
                DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    sValue = dsTemp.Tables[0].Rows[0]["PARAM_VALUE"].ToString();
                }
                else
                {
                    sMessage = "File not exist - " + sFile;
                }
            }
            else
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(sFile);

                try
                {
                    sValue = sr.ReadLine().Trim();
                }
                finally
                {
                    sr.Close();
                }
            }
            return sValue;
        }

        public void Open(string sPrintMethod)
        {
            if (sPrintMethod == "CODESOFT")
                Open_CodeSoft();
            else if (sPrintMethod == "EXCEL")
                Open_Excel();
            else if (sPrintMethod == "BARTENDER")
                Open_BarTender();
        }

        public void Close(string sPrintMethod)
        {
            if (sPrintMethod == "CODESOFT")
                Close_CodeSoft();
            else if (sPrintMethod == "EXCEL")
                Close_Excel();
            else if (sPrintMethod == "BARTENDER")
                Close_BarTender();
        }

        /// <summary>
        /// 取得群組範本檔，檔案名稱後加 "_n"都會回傳 ，EX: BS_Default.btw  BS_Default_1.btw ....
        /// </summary>
        /// <param name="sPath"></param>
        /// <returns></returns>
        private List<string> getGroupSampleFile(string sSampleFile)
        {
            List<string> lstResult = new List<string>();
            lstResult.Add(sSampleFile);
            FileInfo fileInfo = new FileInfo(sSampleFile);
            string sExt = Path.GetExtension(sSampleFile);//副檔名
            string sPath = Path.GetDirectoryName(sSampleFile); //檔案存在的目錄
            string sFileName = Path.GetFileNameWithoutExtension(sSampleFile);
            string sSearchPattern = sFileName + "_*" + sExt;

            foreach (string item in Directory.GetFiles(sPath, sSearchPattern))
            {
                lstResult.Add(item);
            }
            return lstResult;
        }

        /// <summary>
        /// 客製，重組工單號碼（只有改變顯示格式）。
        /// </summary>
        /// <param name="counter">第幾張。用來加上 "#" 字號後面的數字。</param>
        /// <param name="ListParam">欄位的名稱的清單。</param>
        /// <param name="ListData">欄位的值的清單。</param>
        private void WrokOrderWithFixes(int counter, ref System.Windows.Forms.ListBox ListParam, ref System.Windows.Forms.ListBox ListData)
        {
            // 10A2020110011001 => 10A-202011-11-1#001
            // '#' 字號後面的數字，用 counter 補成 3 碼
            string wo = "WORK_ORDER";

            string rc = "RC_NO";

            if (ListParam.Items.Contains(wo))
            {
                // 找字串
                int wo_index = ListParam.Items.IndexOf(wo);

                int rc_index = ListParam.Items.IndexOf(rc);

                string input = ListData.Items[rc_index].ToString();

                string output = string.Empty;

                // 拆字串
                var pattern = new Regex("^(.{3})(.{6})(.{4})(.{3})-(.{3})$");

                var matches = pattern.Matches(input);

                // 組字串
                if (matches.Count <= 0)
                {
                    output = string.Copy(input);
                }
                else
                {
                    // 1. 第一個 match 項目是自己，把它過濾掉
                    // 2. 把數字前面的 0 去掉
                    var group = matches[0].Groups.Cast<Group>()
                        .Where(x => x.Value != input)
                        .Select(x =>
                        {
                            var s = x.Value;

                            if (int.TryParse(s, out int i))
                            {
                                return i.ToString();
                            }
                            else
                            {
                                return s;
                            }
                        }).ToList();

                    var group2 = group.GetRange(0, group.Count - 1);

                    output = string.Join("-", group2) + " #" + (group[group.Count - 1]).ToString().PadLeft(3, '0');
                }

                // 更新字串
                ListData.Items[wo_index] = string.Copy(output);
            }
        }
    }
}
