using PrintLabel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace RCIPQC
{
    class PrintRCLabel
    {
        private Setup PrintSetup = new Setup();
        private string BasePath = Directory.GetCurrentDirectory().Contains("WIP") ?
            Directory.GetCurrentDirectory() : Path.Combine(Directory.GetCurrentDirectory(), "WIP");
        #region 標籤列印測試

        public void Print(string RC_NO, string PrintQauntity, out string sMessage)
        {
            sMessage = string.Empty;
            string batFile = Path.Combine(BasePath, "PrintGo.bat");
            string btwFile = Path.Combine(BasePath, "RCIPQC.btw");
            string lstFile = Path.Combine(BasePath, "RCIPQC.lst");

            // 列印內容
            string _header = "RC_NO";
            List<string> content = new List<string>();
            content.Add(_header);
            content.Add(RC_NO);

            if (!File.Exists(btwFile))
            {
                sMessage = "btw file not found";
                return;
            }           

            if (!File.Exists(lstFile))
            {
                File.AppendAllLines(lstFile, content);
            }
            else
            {
                File.Delete(lstFile);
                File.AppendAllLines(lstFile, content);
            }

            // 刪除舊 batfile
            if (File.Exists(batFile))
            {
                File.Delete(batFile);
            }

            string bat = PrintSetup.LoadBatFile(batFile, ref sMessage);

            // 找不到 bat 指令
            if (string.IsNullOrEmpty(bat))
            {
                return;
            }
            else
            {
                bat = bat.Replace("@PATH1", "\""+btwFile+"\"")
                    .Replace("@PATH2", "\"" + lstFile + "\"")
                    .Replace("@QTY", PrintQauntity);

                PrintSetup.WriteToPrintGo(batFile, bat);
            }
            
            Setup.WinExec(batFile, 0);
        }

        #endregion 
    }
}
