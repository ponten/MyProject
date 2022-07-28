using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assistant
{
    public enum Level
    {
        INFO,
        WARNING,
        ERROR
    }

    public static class Assistant
    {
        private static readonly object YouShallNotPass = new object();

        /// <summary>
        /// 負責委派控制項任務
        /// </summary>
        /// <param name="control">控制項</param>
        /// <param name="action">任務</param>
        public static void InvokeIfRequired(this Control control, MethodInvoker action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }
        }

        /// <summary>
        /// Debug 使用，記錄錯誤發生時間和傳入的資訊。
        /// </summary>
        /// <param name="sLog">要寫入 Log 的內容。</param>
        public static void WriteLog(this string sLog, Level Level)
        {
            DateTime dateTime = DateTime.Now;
            char separator = Path.DirectorySeparatorChar;
            string applicationName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

            // ex: ./LOG/2019.11/2019.11.27/FCLoaderTransfer_2019.11.27_04.log
            string path
                = "." + separator
                + "LOG" + separator
                + dateTime.ToString("yyyyMM") + separator
                + dateTime.ToString("yyyyMMdd") + separator
                + applicationName + dateTime.ToString("_yyyyMMdd_HH") + ".log";

            FileInfo file = new FileInfo(path);
            file.Directory.Create();// If the directory already exists, this method does nothing.
            lock (YouShallNotPass)
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine($"{dateTime:yyyy/MM/dd HH:mm:ss} [ {Level.ToString().PadRight(9)}]: {sLog}");
                }
            }
        }
    }
}
