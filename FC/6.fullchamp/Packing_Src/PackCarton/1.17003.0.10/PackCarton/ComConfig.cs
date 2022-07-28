using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Xml;
using System.Reflection;
using SajetClass;
using System.Windows.Forms;
namespace PackCarton
{
    public class ComConfig
    {
        //public string PortName;//串口名称
        //public StopBits StopBits;//停止位长度
        //public int DataBits;//数据位长度
        //public int BaudRate;//波特率
        //public Parity Parity;//奇偶校验
        public ComConfig(string FunctionType)
        {
            this.g_sFunctionType = FunctionType;
        }
        //private string ConfigFileName = "";
        private SerialPort ComPort = new SerialPort();
        string sIniFile = Application.StartupPath + "\\Sajet.ini";
        SajetInifile sajetInifile1 = new SajetInifile();
        private string g_sFunctionType = "";
        public bool GetSerialPort(out SerialPort port, out string msg)
        {
            msg = "OK";
            if (LoadConfig(out msg))
            {
                port = this.ComPort;
                return true;
            }
            else
            {
                port = null;
                return false;
            }
        }
        private bool LoadConfig(out string msg)
        {
            try
            {
                msg = "OK";
                //XmlDocument doc = new XmlDocument();
                //string path = "";// AppDomain.CurrentDomain.BaseDirectory + "ComPortCoonfig.xml";
                //path = Assembly.GetExecutingAssembly().Location;
                //path = path.Substring(0, path.LastIndexOf('\\'));
                //path += "\\" + ConfigFileName;
                //doc.Load(path);
                // XmlNode node = doc.SelectSingleNode("ComPort");
                string sPortName = sajetInifile1.ReadIniFile(sIniFile, g_sFunctionType, "COM", "");
                string sStopBits = sajetInifile1.ReadIniFile(sIniFile, g_sFunctionType, "StopBits", "");
                string sDataBits = sajetInifile1.ReadIniFile(sIniFile, g_sFunctionType, "DataBits", "");
                string sBaudRate = sajetInifile1.ReadIniFile(sIniFile, g_sFunctionType, "BaudRate", "");
                string sParity = sajetInifile1.ReadIniFile(sIniFile, g_sFunctionType, "Parity", "");
                ComPort.PortName = sPortName;
                switch (sStopBits)
                {
                    case "None":
                        ComPort.StopBits = StopBits.None;
                        break;
                    case "One":
                        ComPort.StopBits = StopBits.One;
                        break;
                    case "OnePointFive":
                        ComPort.StopBits = StopBits.OnePointFive;
                        break;
                    case "Two":
                        ComPort.StopBits = StopBits.Two;
                        break;
                    default:
                        {
                            msg = SajetCommon.SetLanguage("Unknown StopBits:") + sStopBits;
                            return false;
                        }
                }
                ComPort.DataBits = int.Parse(sDataBits);
                ComPort.BaudRate = int.Parse(sBaudRate);
                switch (sParity)
                {
                    case "Even":
                        ComPort.Parity = Parity.Even;
                        break;
                    case "Mark":
                        ComPort.Parity = Parity.Mark;
                        break;
                    case "None":
                        ComPort.Parity = Parity.None;
                        break;
                    case "Odd":
                        ComPort.Parity = Parity.Odd;
                        break;
                    case "Space":
                        ComPort.Parity = Parity.Space;
                        break;
                    default:
                        {
                            msg = SajetCommon.SetLanguage("Unknown Parity:") + sParity;
                            return false;
                        }
                }
                return true;
            }
            catch (Exception ex)
            {
                msg = SajetCommon.SetLanguage("Load Config Error:") + ex.Message;
                return false;
            }
        }

    }
}
