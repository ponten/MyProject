using System;
using System.Text;
using SajetClass;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Data;
using System.Data.OracleClient;

namespace PackCarton
{
    public class PrintLabelByBarTender
    {
        //ListView listPrint;
        //ListBox ListParam, ListData;

        public static string Print_Label(ListView listbPrint, string sBTWLabelName, string sProgramName)
        {
            //先Link====
            PrintLabel.Setup PrintLabelDll = new PrintLabel.Setup();
            PrintLabelDll.Open("BARTENDER");

            //Print=====
            try
            {
                //使用BarTender打印
                int iPrintQty = 1;
                string NoPositionSN = string.Empty;
                //ListParam.Items.Clear();
                //ListData.Items.Clear();

                //配置文件名
                string pLabelFileName = sBTWLabelName;
                if (sBTWLabelName == "BR_DEFAULT_10")
                {
                    pLabelFileName = "BR_DEFAULT";
                }
                string pLabelTypeFile = Application.StartupPath + "\\" + sProgramName + "\\" + sBTWLabelName + ".btw";
                string pDataSourceFile = Application.StartupPath + "\\" + sProgramName + "\\" + pLabelFileName + ".lst";
                string pLabelFieldFile = Application.StartupPath + "\\" + sProgramName + "\\" + pLabelFileName + ".dat";
                string pPrintGoFile = Application.StartupPath + "\\" + sProgramName + "\\" + "PrintGo.bat";

                //是否存在配置文件
                if (!File.Exists(pLabelTypeFile))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Label File Not Found") + " :" + Environment.NewLine + pLabelTypeFile, 0);
                    return "";
                }
                if (!File.Exists(pLabelFieldFile))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Label File Not Found") + " :" + Environment.NewLine + pLabelFieldFile, 0);
                    return "";
                }
                if (File.Exists(pDataSourceFile))
                    File.Delete(pDataSourceFile);

                //读取.dat文件中的要打印的项
                System.IO.StreamReader sr = new System.IO.StreamReader(pLabelFieldFile);
                string sFieldName = sr.ReadLine().Trim();
                sr.Close();
                if (string.IsNullOrEmpty(sFieldName))
                {
                    SajetCommon.Show_Message(pLabelFileName + ".dat" + Environment.NewLine + SajetCommon.SetLanguage("Field file can not be empty"), 0);
                    return "";
                }
                //要打印的项写入.lst文件
                WriteToTxt(pDataSourceFile, sFieldName);

                //循环写入要打印的数据
                for (int i = 0; i < listbPrint.Items.Count; i++)
                {
                    //ListData.Items.Add(listbPrint.Items[i].Text);
                    //写入要打印的数据
                    WriteToTxt(pDataSourceFile, listbPrint.Items[i].Text);
                }
                // 1.17003.0.8 bat 指令從 DB 找
                string sPrintCommand = LoadBatFile(sBTWLabelName);
                //给参数赋值
                if (sPrintCommand.IndexOf("@PATH1") >= 0)
                    sPrintCommand = sPrintCommand.Replace("@PATH1", '"' + pLabelTypeFile + '"');//标签文件名.dwt
                if (sPrintCommand.IndexOf("@PATH2") >= 0)
                    sPrintCommand = sPrintCommand.Replace("@PATH2", '"' + pDataSourceFile + '"');//数据文件.lst
                if (sPrintCommand.IndexOf("@QTY") >= 0)
                    sPrintCommand = sPrintCommand.Replace("@QTY", iPrintQty.ToString());//重复打印数

                if (File.Exists(pPrintGoFile))
                    File.Delete(pPrintGoFile);
                File.AppendAllText(pPrintGoFile, sPrintCommand, Encoding.Default);

                //WinExec(pPrintGoFile, 0);//执行批处理

                return sPrintCommand;
            }
            catch (Exception ex)
            {
                return "";
            }
            finally
            {
                PrintLabelDll.Close("BARTENDER");
            }
        }

        public static bool Print_Label(ListView listbPrint, string sBTWLabelName, string sProgramName, ProgressBar pBar)
        {
            //先Link====
            PrintLabel.Setup PrintLabelDll = new PrintLabel.Setup();
            PrintLabelDll.Open("BARTENDER");

            //Print=====
            try
            {
                pBar.Minimum = 0;
                pBar.Maximum = listbPrint.Items.Count - 1;
                pBar.Visible = true;

                //使用BarTender打印
                int iPrintQty = 1;
                string NoPositionSN = string.Empty;
                //ListParam.Items.Clear();
                //ListData.Items.Clear();

                //配置文件名
                string pLabelFileName = sBTWLabelName;
                string pLabelTypeFile = Application.StartupPath + "\\" + sProgramName + "\\" + pLabelFileName + ".btw";
                string pDataSourceFile = Application.StartupPath + "\\" + sProgramName + "\\" + pLabelFileName + ".lst";
                string pLabelFieldFile = Application.StartupPath + "\\" + sProgramName + "\\" + pLabelFileName + ".dat";
                string pPrintGoFile = Application.StartupPath + "\\" + sProgramName + "\\" + "PrintGo.bat";

                //是否存在配置文件
                if (!File.Exists(pLabelTypeFile))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Label File Not Found") + " :" + Environment.NewLine + pLabelTypeFile, 0);
                    return false;
                }
                if (!File.Exists(pLabelFieldFile))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Label File Not Found") + " :" + Environment.NewLine + pLabelFieldFile, 0);
                    return false;
                }
                if (File.Exists(pDataSourceFile))
                    File.Delete(pDataSourceFile);

                //读取.dat文件中的要打印的项
                System.IO.StreamReader sr = new System.IO.StreamReader(pLabelFieldFile);
                string sFieldName = sr.ReadLine().Trim();
                sr.Close();
                if (string.IsNullOrEmpty(sFieldName))
                {
                    SajetCommon.Show_Message(pLabelFileName + ".dat" + Environment.NewLine + SajetCommon.SetLanguage("Field file can not be empty"), 0);
                    return false;
                }
                //要打印的项写入.lst文件
                WriteToTxt(pDataSourceFile, sFieldName);

                //循环写入要打印的数据
                for (int i = 0; i < listbPrint.Items.Count; i++)
                {
                    //ListData.Items.Add(listbPrint.Items[i].Text);
                    //写入要打印的数据
                    WriteToTxt(pDataSourceFile, listbPrint.Items[i].Text);
                }
                // 1.17003.0.8 bat 指令從 DB 找
                string sPrintCommand = LoadBatFile(sBTWLabelName);
                //给参数赋值
                if (sPrintCommand.IndexOf("@PATH1") >= 0)
                    sPrintCommand = sPrintCommand.Replace("@PATH1", '"' + pLabelTypeFile + '"');//标签文件名.dwt
                if (sPrintCommand.IndexOf("@PATH2") >= 0)
                    sPrintCommand = sPrintCommand.Replace("@PATH2", '"' + pDataSourceFile + '"');//数据文件.lst
                if (sPrintCommand.IndexOf("@QTY") >= 0)
                    sPrintCommand = sPrintCommand.Replace("@QTY", iPrintQty.ToString());//重复打印数

                if (File.Exists(pPrintGoFile))
                    File.Delete(pPrintGoFile);
                System.IO.File.AppendAllText(pPrintGoFile, sPrintCommand, Encoding.Default);

                WinExec(pPrintGoFile, 0);//执行批处理

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {

                pBar.Visible = false;
                PrintLabelDll.Close("BARTENDER");
            }
        }

        /// <summary>
        /// 列印 bat 指令從 DB 找
        /// </summary>
        /// <returns></returns>
        private static string LoadBatFile(string sPROGRAM)
        {
            sPROGRAM = sPROGRAM.Contains("BR_DEFAULT") ? "Barcode Center" : "Packing";
            string sSQL = @"
SELECT PARAM_VALUE
  FROM SAJET.SYS_BASE
 WHERE PROGRAM    = :PROGRAM
   AND PARAM_NAME = 'Bartender Print Command'
   AND ROWNUM     = 1
";
            object[][] param = new object[1][];
            param[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROGRAM", sPROGRAM };
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, param);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                return dsTemp.Tables[0].Rows[0]["PARAM_VALUE"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        [DllImport("kernel32.dll")]
        public static extern int WinExec(string exeName, int operType);//执行批处理

        /// <summary>
        /// 写入.lst文件
        /// </summary>
        /// <param name="sFile">.lst文件名</param>
        /// <param name="sData">要写入的数据</param>
        static void WriteToTxt(string sFile, string sData)
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
    }
}
