using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Data.OracleClient;
using System.IO.Ports;
namespace PackCarton
{
    public partial class fWeightCarton : Form
    {
        public fWeightCarton(string FunctionType)
        {
            InitializeComponent();
            this.gsFunctionType = FunctionType;
        }
        // public string COM = "";
        private string gsFunctionType = "";
        public string CartonNo = "";
        string g_sTemp = "";
        public string PartNo = "";
        public string TerminalID = "";
        public int CartonCapacity = 0;
        //public int SNCount = 0;
        private void fWeightCarton_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            // txtCOM.Text = COM;
            txtCartonNo.Text = CartonNo;
            txtPartNo.Text = PartNo;
            //            try
            //            {
            //                string cmd = @"select distinct p.part_no
            //                            from sajet.sys_part p,sajet.g_sn_status ss
            //                            where ss.part_id=p.part_id
            //                            and ss.carton_no='" + CartonNo + @"'
            //                            and rownum=1";
            //                DataTable dt = ClientUtils.ExecuteSQL(cmd).Tables[0];
            //                if (dt.Rows.Count == 0)
            //                {
            //                    txtMsg.Text = SajetCommon.SetLanguage("This Carton No not have Part No");
            //                    txtMsg.BackColor = Color.Red;
            //                    return;
            //                }
            //                PartNo = dt.Rows[0]["PART_NO"].ToString();
            //            }
            //            catch (Exception ex)
            //            {
            //                txtMsg.Text = SajetCommon.SetLanguage("Get Part No Error:") + ex.Message;
            //                txtMsg.BackColor = Color.Red;
            //                return;
            //            }

            // txtPartNo.Text = PartNo;
            if (string.IsNullOrEmpty(txtCartonNo.Text.Trim()))
            {
                Show_Message("Carton NO Is Empty", MSGType.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtCOM.Text.Trim()))
            {
                Show_Message("Com Is Null", MSGType.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtPartNo.Text.Trim()))
            {
                Show_Message("Part No Is Empty", MSGType.Error);
                return;
            }
            SajetCommon.SetLanguageControl(this);
            if (!GetPartWeight(txtPartNo.Text.Trim()))
            {
                return;
            }
            if (!InitialCom())
            {
                return;
            }
            timer1.Start();
        }
        /// <summary>
        /// 获取重量信息
        /// </summary>
        /// <param name="partno"></param>
        /// <returns></returns>
        private bool GetPartWeight(string partno)
        {
            try
            {
                string cmd = " select pw.fill_weight,pw.carton_material_weight,pw.carton_weight_up,pw.carton_weight_down from sajet.sys_part_weight pw ,sajet.sys_part p where pw.part_id=p.part_id and p.part_no='" + txtPartNo.Text.Trim() + "'";
                DataTable dt = ClientUtils.ExecuteSQL(cmd).Tables[0];
                if (dt.Rows.Count == 0)
                {
                    //txtMsg.Text = SajetCommon.SetLanguage("No this Part No(") + txtPartNo.Text + SajetCommon.SetLanguage(")Weight info");
                    //txtMsg.BackColor = Color.Red;
                    Show_Message(SajetCommon.SetLanguage("No this Part No(") + txtPartNo.Text + SajetCommon.SetLanguage(")Weight info"), MSGType.Error);
                    return false;
                }
                //txtMaxWeight.Text = dt.Rows[0]["CARTON_MAX_WEIGHT"].ToString();
                //txtMinWeight.Text = dt.Rows[0]["CARTON_MIN_WEIGHT"].ToString();
                //计算Carton标准重量，外箱标准重量=外箱所包彩盒总重量+包材重量+填充物重量(未满时)±KG
                string Fill = dt.Rows[0]["FILL_WEIGHT"].ToString();
                string CartonMaterial = dt.Rows[0]["CARTON_MATERIAL_WEIGHT"].ToString();
                string CartonUp = dt.Rows[0]["CARTON_WEIGHT_UP"].ToString();
                string CartonDown = dt.Rows[0]["CARTON_WEIGHT_DOWN"].ToString();
                if (string.IsNullOrEmpty(Fill.Trim()))
                {
                    Show_Message("Part No No Fill Weight", MSGType.Error);
                    return false;
                }
                if (string.IsNullOrEmpty(CartonMaterial.Trim()))
                {
                    Show_Message("Part No No CartonMaterial Weight", MSGType.Error);
                    return false;
                }
                if (string.IsNullOrEmpty(CartonUp.Trim()))
                {
                    Show_Message("Part No No Carton Up Weight", MSGType.Error);
                    return false;
                }
                if (string.IsNullOrEmpty(CartonDown.Trim()))
                {
                    Show_Message("Part No No Carton Down Weight", MSGType.Error);
                    return false;
                }
                Double StandardWeight = 0;
                cmd = @"  select ss.serial_number,nvl(bw.weight,0) as snweight,bw.result from sajet.g_sn_status ss,sajet.g_box_weight bw
                            where ss.serial_number=bw.box_no(+)
                             and ss.carton_no='" + CartonNo + "'";
                dt = ClientUtils.ExecuteSQL(cmd).Tables[0];
                if (dt.Rows.Count == 0)
                {
                    Show_Message("This Carton No SN", MSGType.Error);
                    return false;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["SNWEIGHT"].ToString().Equals("0") || dt.Rows[i]["RESULT"].ToString().Equals("1"))
                    {
                        Show_Message(SajetCommon.SetLanguage("SN[") + dt.Rows[i]["SERIAL_NUMBER"].ToString() + SajetCommon.SetLanguage("] Need Weight Box"), MSGType.Error);
                        return false;
                    }
                }
                //int SNCount = Convert.ToInt32(dt.Rows[0]["SNCOUNT"].ToString());
                StandardWeight += Convert.ToDouble(CartonMaterial);//包材重量
                //Double PCBStandardWeight = Convert.ToDouble(PCBStandard);
                Double CartonUpWeight = Convert.ToDouble(CartonUp);
                Double CartonDownWeight = Convert.ToDouble(CartonDown);
                for (int i = 0; i < dt.Rows.Count; i++)//加上每一个彩盒的重量
                {
                    StandardWeight += Convert.ToDouble(dt.Rows[i]["SNWEIGHT"].ToString());
                }
                if (dt.Rows.Count < CartonCapacity)//未满加上填充物的重量
                {
                    double FillWeight = Convert.ToDouble(Fill);
                    StandardWeight += FillWeight * (CartonCapacity - dt.Rows.Count);
                }
                txtStandardWeight.Text = StandardWeight.ToString();
                txtMaxWeight.Text = (StandardWeight + CartonUpWeight).ToString();
                txtMinWeight.Text = (StandardWeight - CartonDownWeight).ToString();
                return true;
            }
            catch (Exception ex)
            {
                // txtMsg.Text = SajetCommon.SetLanguage("Get Part No Weight standard info error:") + ex.Message;
                //  txtMsg.BackColor = Color.Red;
                Show_Message(SajetCommon.SetLanguage("Get Part No Weight standard info error:") + ex.Message, MSGType.Error);
                return false;
            }
        }
        private bool InitialCom()
        {
            try
            {
                if (serialPort1.IsOpen)
                    serialPort1.Close();
                ComConfig config = new ComConfig(gsFunctionType);
                string msg = "";
                if (!config.GetSerialPort(out serialPort1, out msg))
                {
                    Show_Message(msg, MSGType.Error);
                    return false;
                }
                //serialPort1.PortName = COM;        //Port的ID需要接在"COM"後方
                //serialPort1.StopBits = StopBits.One;  //停止位元長度1
                //serialPort1.DataBits = 8;             //資料長度8位元
                //serialPort1.BaudRate = 2400;          //包率
                //serialPort1.Parity = Parity.None;     //無位元檢查
                txtCOM.Text = serialPort1.PortName;
                serialPort1.DataReceived += new SerialDataReceivedEventHandler(RS232_DataReceievedBegin);   //設定資料接收時的對應事件
                serialPort1.Open();                   //開啟RS232通訊扉
                return true;
            }
            catch (Exception ex)
            {
                //txtMsg.Text = SajetCommon.SetLanguage("Init COM Error:") + ex.Message;
                //txtMsg.BackColor = Color.Red;
                Show_Message(SajetCommon.SetLanguage("Init COM Error:") + ex.Message, MSGType.Error);
                return false;   //RS232裝置開啟失敗
            }
        }
        delegate void ThreadsSynchronization();
        private void RS232_DataReceievedBegin(object sender, SerialDataReceivedEventArgs e)
        {
            BeginInvoke(new ThreadsSynchronization(RS232_DataReceive));
        }
        private void RS232_DataReceive()
        {
            int bRead = 0;
            bRead = serialPort1.BytesToRead;
            byte[] cData = new byte[bRead];
            serialPort1.Read(cData, 0, bRead);
            foreach (byte b in cData)
            {
                g_sTemp += Convert.ToChar(b);
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void SaveData()
        {
            if (txtTestValue.Text.Trim().Equals("N/A") || string.IsNullOrEmpty(txtTestValue.Text.Trim()))
            {
                Show_Message("Weight Error", MSGType.Error);
                return;
            }
            try
            {
                double dWeight = double.Parse(txtTestValue.Text.Trim());
                string sResult = "0";
                if (txtResult.Text == "OK")
                    sResult = "0";
                else
                    sResult = "1";
                object[][] Params;
                //儲存
                string sSQL = @"SELECT * FROM SAJET.G_CARTON_WEIGHT WHERE CARTON_NO = :CARTON_NO";
                Params = new object[1][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CARTON_NO", txtCartonNo.Text.Trim() };
                DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    //Copy to HT
                    sSQL = @"INSERT INTO SAJET.G_HT_CARTON_WEIGHT
                        SELECT * FROM SAJET.G_CARTON_WEIGHT WHERE CARTON_NO = :CARTON_NO";
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CARTON_NO", txtCartonNo.Text.Trim() };
                    dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                    //Delete Data
                    sSQL = @"DELETE FROM SAJET.G_CARTON_WEIGHT WHERE CARTON_NO = :CARTON_NO";
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CARTON_NO", txtCartonNo.Text.Trim() };
                    dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                }
                //Insert Data
                sSQL = @"INSERT INTO SAJET.G_CARTON_WEIGHT
                    (CARTON_NO,TERMINAL_ID,WEIGHT,UNIT,UPDATE_USER_ID, RESULT)
                    VALUES
                    (:CARTON_NO,:TERMINAL_ID,:WEIGHT,:UNIT,:UPDATE_USER_ID, :RESULT)";
                Params = new object[6][];
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CARTON_NO", txtCartonNo.Text.Trim() };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TERMINAL_ID", TerminalID };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.Double, "WEIGHT", dWeight };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UNIT", "KG" };
                Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USER_ID", ClientUtils.UserPara1 };
                Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RESULT", sResult };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                //若Result為NG，則將Carton內的SN Current Status改為1
                if (sResult == "1")
                {
                    //                    sSQL = @"UPDATE SAJET.G_SN_STATUS
                    //                        SET CURRENT_STATUS = '1' 
                    //                        WHERE BOX_NO = :BOX_NO";
                    //                    Params = new object[1][];
                    //                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BOX_NO", txtSN.Text };
                    //                    dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                }
                else
                {
                    serialPort1.Close();
                    DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Show_Message(SajetCommon.SetLanguage("Save Data Error:") + ex.Message, MSGType.Error);
                return;
            }
        }

        private void fWeightCarton_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPort1.IsOpen)
                serialPort1.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (g_sTemp.Contains("Net") && g_sTemp.Contains("g"))
            {
                timer1.Enabled = false;
                string s = g_sTemp.Substring(g_sTemp.IndexOf("Net"), g_sTemp.IndexOf("g") - g_sTemp.IndexOf("Net"));

                s = s.Substring(3).Trim();
                double kg = Convert.ToDouble(s) / 1000;
                txtTestValue.Text = kg.ToString("f2");//保留两位小数
                try
                {
                    double dCartonWeight = Convert.ToDouble(txtTestValue.Text.Trim());
                    double dUCL = Convert.ToDouble(txtMaxWeight.Text.Trim());
                    double dLCL = Convert.ToDouble(txtMinWeight.Text.Trim());
                    if (dCartonWeight >= dLCL && dCartonWeight <= dUCL)
                    {
                        txtResult.Text = "OK";
                        Show_Message(SajetCommon.SetLanguage("Box Weight OK"), MSGType.Error);
                    }
                    else
                    {
                        Show_Message(SajetCommon.SetLanguage("Box Weight NG"), MSGType.Error);
                        txtResult.Text = "NG";
                        btnOK.Focus();
                    }
                    SaveData();
                }
                catch (Exception ex)
                {
                    Show_Message(SajetCommon.SetLanguage("Get Test Value Error:") + ex.Message, MSGType.Error);
                }
            }
        }
        /// <summary>
        /// 根据类型和Text显示消息
        /// </summary>
        /// <param name="sText">要显示的消息</param>
        /// <param name="type">消息类型</param>
        /// <param name="autotranslate">自动翻译</param>
        /// <returns></returns>
        public void Show_Message(string sText, MSGType type)
        {
            int ifreq = 800; int iduration = 200;
            txtMsg.Text = SajetCommon.SetLanguage(sText);

            switch (type)
            {
                case MSGType.Error: //Error                        
                    txtMsg.ForeColor = Color.Red;
                    txtMsg.BackColor = Color.Silver;
                    Console.Beep(ifreq, iduration);
                    break;
                case MSGType.Warning: //Warning                        
                    txtMsg.ForeColor = Color.Blue;
                    txtMsg.BackColor = Color.FromArgb(255, 255, 128);
                    Console.Beep(ifreq, iduration);
                    break;
                default:
                    txtMsg.ForeColor = Color.Green;
                    txtMsg.BackColor = Color.White;
                    break;
            }
        }


    }
}
