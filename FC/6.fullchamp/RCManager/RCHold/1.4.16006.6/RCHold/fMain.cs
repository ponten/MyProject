using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OracleClient;
using SajetClass;
using ExportExcel;
using SajetFilter;


namespace RCHold
{
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();
        }

        //sKey = HOLD_ID
        string sSQL;
        DataSet dsTemp;
        public static String rc_UserID = ClientUtils.UserPara1;
        string sH_Status = "HOLD";
        string sKey;
        string sRC, sHoldDesc, sDefectID, ssQTY, sSerial_Number;
        string sHoldNO, sDB_Time;
        string To, Res;
        SendEmail se;

        private void Initial_Form()
        {
            SajetCommon.SetLanguageControl(this);
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);

            label2.Visible = false;
            //修改选择框为可选状态
            RC_dgv.ReadOnly = false;
            SetCombDefect();
            SetCombRCSN();
        }

        public void SetCombDefect()
        {
            //Combox赋值
            sSQL = @"SELECT DEFECT_TYPE_DESC
	                        FROM SAJET.SYS_DEFECT_TYPE";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
            {
                DTD_comb.Items.Add(dsTemp.Tables[0].Rows[i]["DEFECT_TYPE_DESC"].ToString());
            }

            DTD_comb.SelectedIndex = 0;
        }

        public void SetCombRCSN()
        {
            combRCSN.Items.Add(SajetCommon.SetLanguage("RC_NO"));
            // 2016.5.3 By Jason
            //combRCSN.Items.Add(SajetCommon.SetLanguage("SERIAL_NUMBER"));
            // 2016.5.3 End
            combRCSN.SelectedIndex = 0;
        }

        private bool CheckStatus()
        {
            bool IsChecked;
            int s = -1;
            string sCurrent_Status;

            for (int i = 0; i < RC_dgv.Rows.Count; i++)
            {
                IsChecked = Convert.ToBoolean(RC_dgv.Rows[i].Cells[0].FormattedValue);

                if (IsChecked)
                {
                    sCurrent_Status = RC_dgv.Rows[i].Cells[4].Value.ToString();
                    if (sCurrent_Status == "Running")
                    {
                        s++;
                    }
                }
            }
            if (s >= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //查询相关流程卡号

        private void Excute_button_Click(object sender, EventArgs e)
        {
            string sCurrent_Status = "", sStatus = "";

            if (!CheckStatus())
            {
                string sMsg = SajetCommon.SetLanguage("The rc_no/sn is process do not hold");
                SajetCommon.Show_Message(sMsg, 1);
                return;
            }

            // 2016.5.18 By Jason
            fSelect f = new fSelect();

            if (f.ShowDialog() == DialogResult.OK)
            {
                sCurrent_Status = f.sCurrent_Status;
                sStatus = f.sStatus;
            }
            else
            {
                string sMsg = SajetCommon.SetLanguage("Please select status");
                SajetCommon.Show_Message(sMsg, 1);
                return;
            }
            // 2016.5.18 End

            int n = 0;
            //获取HOLD_ID

            // 2016.5.2 By Jason

            //string sSQL1 = @"Select hold_NO from sajet.g_rc_hold_status";
            //dsTemp = ClientUtils.ExecuteSQL(sSQL1);
            //if (dsTemp.Tables[0].Rows.Count > 0)
            //{
            //    for (int j = 0; j < dsTemp.Tables[0].Rows.Count; j++)
            //    {
            //        sHoldNO = dsTemp.Tables[0].Rows[j]["HOLD_NO"].ToString();
            //        sHoldNO = sHoldNO.Substring(0, 8);

            //    }

            //    sSQL = @"Select TO_CHAR(SYSDATE,'YYYYMMDD') SYS from dual";
            //    dsTemp = ClientUtils.ExecuteSQL(sSQL);
            //    sDB_Time = dsTemp.Tables[0].Rows[0]["SYS"].ToString();

            //    if (sHoldNO.Equals(sDB_Time))
            //    {
            //        sKey = SajetCommon.GetMaxID("sajet.g_rc_hold_status", "HOLD_NO", 10);
            //        //sKey = (long.Parse(ssKey) + 1).ToString();
            //    }
            //    else
            //    {
            //        sKey = sDB_Time + "001";
            //    }
            //}
            //else
            //{
            //    sSQL = @"Select TO_CHAR(SYSDATE,'YYYYMMDD') SYS from dual";
            //    dsTemp = ClientUtils.ExecuteSQL(sSQL);
            //    sDB_Time = dsTemp.Tables[0].Rows[0]["SYS"].ToString();
            //    sKey = sDB_Time + "001";
            //}

            string sSQL1 = @"SELECT TO_CHAR(SYSDATE,'YYYYMMDD') SYS FROM DUAL";
            dsTemp = ClientUtils.ExecuteSQL(sSQL1);
            sDB_Time = dsTemp.Tables[0].Rows[0]["SYS"].ToString();

            string sSQL = @" SELECT COUNT(*) AS ROWCOUNT,MAX(LPAD(SUBSTR(HOLD_NO,-3,3) + 1,3,0)) AS HOLD_END
                               FROM SAJET.G_RC_HOLD_STATUS
                              WHERE HOLD_NO LIKE '" + Spec_textBox.Text + "-" + sDB_Time + "-%'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            if (dsTemp.Tables[0].Rows[0]["ROWCOUNT"].ToString() != "0")
            {
                //if (Convert.ToInt32(dsTemp.Tables[0].Rows[0]["RC_END"].ToString()) > 9)
                //{
                //    SajetCommon.Show_Message("RC lotsize is full", 0);
                //    editQty.Focus();
                //    editQty.SelectAll();
                //    return;
                //}

                sKey = Spec_textBox.Text + "-" + sDB_Time + "-" + dsTemp.Tables[0].Rows[0]["HOLD_END"].ToString();
            }
            else
            {
                sKey = Spec_textBox.Text + "-" + sDB_Time + "-001";
            }
            // 2016.5.2 End

            // 2016.4.20 By Jason (防呆)
            //sHoldDesc = HD_richTextBox.Text;
            sHoldDesc = HD_richTextBox.Text.Trim();
            // 2016.4.20 End

            int iDefectType = DTD_comb.SelectedIndex;
            decimal iTotalQty = 0;

            DateTime datExeTime = DateTime.Now;

            // 2016.5.25 By Jason
            long lTravel_Id = Convert.ToInt64(datExeTime.ToString("yyyyMMddHHmmssf"));
            // 2016.5.25 End

            bool IsChecked;

            for (int i = 0; i < RC_dgv.Rows.Count; i++)
            {
                IsChecked = Convert.ToBoolean(RC_dgv.Rows[i].Cells[0].FormattedValue);

                if (IsChecked)
                {
                    n++;

                    if (combRCSN.SelectedIndex == 0)
                    {
                        sRC = RC_dgv.Rows[i].Cells[1].Value.ToString();
                        sSerial_Number = "N/A";
                    }
                    else
                    {
                        sSerial_Number = RC_dgv.Rows[i].Cells[1].Value.ToString();
                        sRC = "N/A";
                    }

                    if (sHoldDesc == "" || sHoldDesc == null)
                    {
                        string sMsg = SajetCommon.SetLanguage("HOLD_DESC  IS  NULL");
                        SajetCommon.Show_Message(sMsg, 1);
                        return;
                    }

                    // 2016.11.23 By Jason
                    if (sCurrent_Status == "11" && (TB_OPTION1.Text.Trim() == "" || TB_OPTION1.Text.Trim() == null))
                    {
                        string sMsg = SajetCommon.SetLanguage("Data is Null") + Environment.NewLine + label3.Text;
                        SajetCommon.Show_Message(sMsg, 1);
                        return;
                    }

                    sSQL = "SELECT COUNT(*) AS COUNTS FROM SAJET.SYS_PART WHERE PART_NO = '" + TB_OPTION1.Text.Trim() + "' ";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);

                    if (sCurrent_Status == "11" && dsTemp.Tables[0].Rows[0]["COUNTS"].ToString() == "0")
                    {
                        string sMsg = SajetCommon.SetLanguage("Part No Error");
                        SajetCommon.Show_Message(sMsg, 1);
                        return;
                    }
                    // 2016.11.23 End

                    // 2016.11.23 By Jason
                    if (sCurrent_Status == "11" && (TB_OPTION2.Text.Trim() == "" || TB_OPTION2.Text.Trim() == null))
                    {
                        string sMsg = SajetCommon.SetLanguage("Data is Null") + Environment.NewLine + label4.Text;
                        SajetCommon.Show_Message(sMsg, 1);
                        return;
                    }
                    // 2016.11.23 End

                    //if (TB_OPTION3.Text.Trim() == "" || TB_OPTION3.Text.Trim() == null)
                    //{
                    //    string sMsg = SajetCommon.SetLanguage("Data is Null") + Environment.NewLine + label5.Text;
                    //    SajetCommon.Show_Message(sMsg, 1);
                    //    return;
                    //}

                    //if (TB_OPTION4.Text.Trim() == "" || TB_OPTION4.Text.Trim() == null)
                    //{
                    //    string sMsg = SajetCommon.SetLanguage("Data is Null") + Environment.NewLine + label6.Text;
                    //    SajetCommon.Show_Message(sMsg, 1);
                    //    return;
                    //}

                    //if (TB_OPTION5.Text.Trim() == "" || TB_OPTION5.Text.Trim() == null)
                    //{
                    //    string sMsg = SajetCommon.SetLanguage("Data is Null") + Environment.NewLine + label7.Text;
                    //    SajetCommon.Show_Message(sMsg, 1);
                    //    return;
                    //}

                    //if (TB_OPTION6.Text.Trim() == "" || TB_OPTION6.Text.Trim() == null)
                    //{
                    //    string sMsg = SajetCommon.SetLanguage("Data is Null") + Environment.NewLine + label8.Text;
                    //    SajetCommon.Show_Message(sMsg, 1);
                    //    return;
                    //}

                    if (iDefectType < 0)
                    {
                        string sMsg = SajetCommon.SetLanguage("DEFECT_TYPE_DESC IS  NULL");
                        SajetCommon.Show_Message(sMsg, 1);
                        return;
                    }

                    //Insert sajet.g_rc_hold_detail

                    //获取对应的PROCESS_ID,CURRENT_STATUS
                    if (combRCSN.SelectedIndex == 0)
                    {
                        sSQL = @"SELECT a.PROCESS_ID,a.CURRENT_STATUS FROM SAJET.G_RC_STATUS a 
                                                WHERE a.RC_NO = '" + sRC + "' AND A.RELEASE ='N' ";
                        dsTemp = ClientUtils.ExecuteSQL(sSQL);
                        string sProcessID = dsTemp.Tables[0].Rows[0]["PROCESS_ID"].ToString();
                        string sCurrentStatus = dsTemp.Tables[0].Rows[0]["CURRENT_STATUS"].ToString();

                        sSQL = @"Insert into sajet.g_rc_hold_detail
                                            (HOLD_NO,RC_NO,PROCESS_ID,CURRENT_STATUS,SERIAL_NUMBER)
                                            VALUES
                                            (:HOLD_NO,:RC_NO,:PROCESS_ID,:CURRENT_STATUS,:SERIAL_NUMBER)";
                        object[][] DParams = new object[5][];
                        DParams[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "HOLD_NO", sKey };
                        DParams[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", sRC };
                        DParams[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", sProcessID };
                        DParams[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CURRENT_STATUS", sCurrentStatus };
                        DParams[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SERIAL_NUMBER", sSerial_Number };
                        dsTemp = ClientUtils.ExecuteSQL(sSQL, DParams);

                        //UPDATE SAJET.G_RC_STATUS
                        sSQL = @"Update sajet.g_rc_status
                                set CURRENT_STATUS = '" + sCurrent_Status + "',TRAVEL_ID = " + lTravel_Id + ",UPDATE_TIME = :UPDATE_TIME,UPDATE_USERID = :UPDATE_USERID,RELEASE = 'N' WHERE RC_NO = :RC_NO";
                        object[][] DParams2 = new object[3][];
                        DParams2[0] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime };
                        DParams2[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", rc_UserID };
                        DParams2[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", sRC };
                        dsTemp = ClientUtils.ExecuteSQL(sSQL, DParams2);

                        if (sCurrent_Status == "11")
                        {
                            //INSERT MESUSER.G_RC_TRANSFER
                            object[][] DParams3 = new object[9][];
                            sSQL = " INSERT INTO MESUSER.G_RC_TRANSFER (JOB_NUMBER,RC_NO,CURRENT_QTY,PROCESS_ID,WO_CLOSE,DEFECT_DESC,HOLD_DESC,OPTION1,OPTION2,OPTION3,OPTION4,OPTION5,OPTION6) "
                                 + " SELECT WORK_ORDER,RC_NO,CURRENT_QTY,PROCESS_ID,'N' AS WO_CLOSE,:DEFECT_DESC,:HOLD_DESC,:OPTION1,:OPTION2,:OPTION3,:OPTION4,:OPTION5,:OPTION6 "
                                 + "   FROM SAJET.G_RC_STATUS "
                                 + "  WHERE RC_NO = :RC_NO "
                                 + "    AND RELEASE = 'N' ";
                            DParams3[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", sRC };
                            DParams3[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DEFECT_DESC", DTD_comb.Text };
                            DParams3[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "HOLD_DESC", sHoldDesc };
                            DParams3[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION1", TB_OPTION1.Text.Trim() };
                            DParams3[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION2", TB_OPTION2.Text.Trim() };
                            DParams3[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION3", TB_OPTION3.Text.Trim() };
                            DParams3[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION4", TB_OPTION4.Text.Trim() };
                            DParams3[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION5", TB_OPTION5.Text.Trim() };
                            DParams3[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION6", TB_OPTION6.Text.Trim() };
                            dsTemp = ClientUtils.ExecuteSQL(sSQL, DParams3);
                        }
                    }
                    else
                    {
                        sSQL = @"SELECT a.PROCESS_ID,a.CURRENT_STATUS FROM SAJET.G_SN_STATUS a 
                                                WHERE a.SERIAL_NUMBER = '" + sSerial_Number + "'";
                        dsTemp = ClientUtils.ExecuteSQL(sSQL);
                        string sProcessID = dsTemp.Tables[0].Rows[0]["PROCESS_ID"].ToString();
                        string sCurrentStatus = dsTemp.Tables[0].Rows[0]["CURRENT_STATUS"].ToString();

                        sSQL = @"INSERT INTO SAJET.G_RC_HOLD_DETAIL
                                            (HOLD_NO,RC_NO,PROCESS_ID,CURRENT_STATUS,SERIAL_NUMBER)
                                            VALUES
                                            (:HOLD_NO,:RC_NO,:PROCESS_ID,:CURRENT_STATUS,:SERIAL_NUMBER)";
                        object[][] DParams = new object[5][];
                        DParams[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "HOLD_NO", sKey };
                        DParams[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", sRC };
                        DParams[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", sProcessID };
                        DParams[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CURRENT_STATUS", sCurrentStatus };
                        DParams[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SERIAL_NUMBER", sSerial_Number };

                        dsTemp = ClientUtils.ExecuteSQL(sSQL, DParams);

                        //UPDATE SAJET.G_RC_STATUS
                        sSQL = @"UPDATE SAJET.G_SN_STATUS
                                SET CURRENT_STATUS = '2',UPDATE_TIME = SYSDATE,UPDATE_USERID = " + rc_UserID + " WHERE SERIAL_NUMBER = '" + sSerial_Number + "'";
                        dsTemp = ClientUtils.ExecuteSQL(sSQL);
                    }

                    // 2016.4.20 By Jason (重新定義暫停數量)
                    //获取QTY值
                    string sQTY = RC_dgv.Rows[i].Cells[3].Value.ToString();
                    decimal iQTY = Decimal.Parse(sQTY);

                    iTotalQty = iTotalQty + iQTY;
                    ssQTY = iTotalQty.ToString();
                    // 2016.4.20 End
                }

                ////获取QTY值
                //string sQTY = RC_dgv.Rows[i].Cells[3].Value.ToString();
                //int iQTY;
                //if (n == 1)
                //{
                //    iQTY = Int32.Parse(sQTY);
                //    ssQTY = iQTY.ToString();
                //}
                //else
                //{
                //    iQTY = Int32.Parse(sQTY);
                //    iQTY += iQTY;
                //    ssQTY = iQTY.ToString();
                //}
            }

            if (n <= 0)
            {
                string sMsg = SajetCommon.SetLanguage("Please select RC/SN");
                SajetCommon.Show_Message(sMsg, 1);
                return;
            }

            //写入sajet.g_rc_hold_status和sajet.g_rc_hold_travel

            //获取DEFECT_TYPE_ID
            string sDefectDESC = DTD_comb.SelectedItem.ToString();
            sSQL = "Select DEFECT_TYPE_ID from sajet.sys_defect_type where defect_type_desc = '" + sDefectDESC + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            sDefectID = dsTemp.Tables[0].Rows[0]["DEFECT_TYPE_ID"].ToString();

            //Insert sajet.g_rc_hold_status
            sSQL = @"Insert into SAJET.G_RC_HOLD_STATUS
                           (HOLD_NO,DEFECT_TYPE_ID,STATUS,QTY,HOLD_USERID,HOLD_TIME,HOLD_DESC,UPDATE_USERID,UPDATE_TIME,OPTION1,OPTION2,OPTION3,OPTION4,OPTION5,OPTION6)
                            VALUES
                            (:HOLD_NO,:DEFECT_TYPE_ID,:STATUS,:QTY,:HOLD_USERID,:HOLD_TIME,:HOLD_DESC,:UPDATE_USERID,:UPDATE_TIME,:OPTION1,:OPTION2,:OPTION3,:OPTION4,:OPTION5,:OPTION6)";
            object[][] Params = new object[15][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "HOLD_NO", sKey };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DEFECT_TYPE_ID", sDefectID };
            // 2016.5.18 By Jason
            //Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "STATUS", sH_Status };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "STATUS", sStatus };
            // 2016.5.18 End
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "QTY", ssQTY };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "HOLD_USERID", rc_UserID };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.DateTime, "HOLD_TIME", datExeTime };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "HOLD_DESC", sHoldDesc };
            Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", rc_UserID };
            Params[8] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime };
            Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION1", TB_OPTION1.Text.Trim() };
            Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION2", TB_OPTION2.Text.Trim() };
            Params[11] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION3", TB_OPTION3.Text.Trim() };
            Params[12] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION4", TB_OPTION4.Text.Trim() };
            Params[13] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION5", TB_OPTION5.Text.Trim() };
            Params[14] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPTION6", TB_OPTION6.Text.Trim() };

            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            //// 2016.5.13 By Jason
            //sSQL = " INSERT INTO SAJET.G_RC_HOLD_TRAVEL "
            //     + " SELECT * FROM SAJET.G_RC_HOLD_STATUS "
            //     + "         WHERE HOLD_NO = '" + sKey + "' ";
            //ClientUtils.ExecuteSQL(sSQL);
            //// 2016.5.13 End

            //刷新数据,hold的数据在界面不显示
            RC_dgv.Rows.Clear();
            HD_richTextBox.Clear();
            // 2016.5.3 By Jason
            TB_OPTION1.Clear();
            TB_OPTION2.Clear();
            TB_OPTION3.Clear();
            TB_OPTION4.Clear();
            TB_OPTION5.Clear();
            TB_OPTION6.Clear();
            // 2016.5.3 End
            ShowData();

            //写入excel表格
            if (combRCSN.SelectedIndex == 0)
            {
                // 2016.4.20 By Jason
                //                sSQL = @"Select b.rc_no,d.process_name,a.qty from sajet.g_rc_hold_status a,sajet.g_rc_hold_detail b,sajet.sys_defect_type c,sajet.sys_process d
                //                        where a.hold_no = '" + sKey + "' and a.hold_no = b.hold_no and a.defect_type_id = c.defect_type_id and b.process_id = d.process_id";
                sSQL = @"SELECT A.RC_NO,D.PROCESS_NAME,C.CURRENT_QTY
                           FROM SAJET.G_RC_HOLD_DETAIL A,
                                SAJET.G_RC_HOLD_STATUS B,
                                SAJET.G_RC_STATUS C,
                                SAJET.SYS_PROCESS D
                          WHERE A.HOLD_NO = '" + sKey + "' AND A.HOLD_NO = B.HOLD_NO AND A.RC_NO = C.RC_NO AND A.PROCESS_ID = D.PROCESS_ID AND C.RELEASE = 'N' ";
                // 2016.4.20 End
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
            }
            else
            {
                sSQL = @"Select b.serial_number,d.process_name,a.qty from sajet.g_rc_hold_status a,sajet.g_rc_hold_detail b,sajet.sys_defect_type c,sajet.sys_process d
                        where a.hold_no = '" + sKey + "' and a.hold_no = b.hold_no and a.defect_type_id = c.defect_type_id and b.process_id = d.process_id";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
            }

            UpdateExcel.Export(dsTemp, sKey);

            // 2016.4.20 By Jason

            //发送邮件
            //获取收件人地址
            sSQL = @"Select a.EMAIL,a.emp_name From sajet.sys_emp a,sajet.sys_defect_type_emp b,sajet.g_rc_hold_status e  
                      where e.hold_no = '" + sKey + "' and e.defect_type_id = b.defect_type_id and b.emp_id = a.emp_id";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                {
                    if (dsTemp.Tables[0].Rows.Count == 1)
                    {
                        To = dsTemp.Tables[0].Rows[0]["EMAIL"].ToString();
                    }
                    else
                    {
                        To += dsTemp.Tables[0].Rows[i]["EMAIL"].ToString().Trim() + ";";
                    }
                }

                //获取邮件内容
                if (combRCSN.SelectedIndex == 0)
                {
                    sSQL = @"Select hold_desc from sajet.g_rc_hold_status a,sajet.g_rc_hold_detail b where b.rc_no = '" + sRC + "' and a.hold_no = b.hold_no";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);
                }
                else
                {
                    sSQL = @"Select hold_desc from sajet.g_rc_hold_status a,sajet.g_rc_hold_detail b where b.serial_number = '" + sSerial_Number + "' and a.hold_no = b.hold_no";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);
                }

                string Body = dsTemp.Tables[0].Rows[0]["HOLD_DESC"].ToString();

                //获取邮件标题
                string Title = "異常單號 : " + sKey;

                //se = new SendEmail(To, From, Body, Title, Password);
                se = new SendEmail(To, Body, Title);
                se.Attachments(System.Windows.Forms.Application.StartupPath + Path.DirectorySeparatorChar + ClientUtils.fCurrentProject + Path.DirectorySeparatorChar + @"RC_HOLD" + @"\" + sKey + ".xlsx");
                se.Send();

                //弹框提醒任务完成

                sSQL = @"Select wm_concat(a.emp_name) emp_name From sajet.sys_emp a,sajet.sys_defect_type_emp b,sajet.g_rc_hold_status e  
                          where e.hold_no = '" + sKey + "' and e.defect_type_id = b.defect_type_id and b.emp_id = a.emp_id";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                Res = dsTemp.Tables[0].Rows[0]["emp_name"].ToString();

                string sData = SajetCommon.SetLanguage("Have informed") + Res;
                string sMsg1 = sKey + ":" + SajetCommon.SetLanguage("Exception list completed") + Environment.NewLine + sData;
                SajetCommon.Show_Message(sMsg1, 3);
            }
            else
            {
                string sMsg = SajetCommon.SetLanguage("Email IS Null") + Environment.NewLine;
                SajetCommon.Show_Message(sMsg, 1);
                return;
            }

            // 2016.4.20 End
        }

        private void QUERY_button_Click(object sender, EventArgs e)
        {
            //每次查询前先清空数据
            RC_dgv.Rows.Clear();
            //PN_textBox.Focus();
            Spec_textBox.Focus();
            HD_richTextBox.Clear();

            // 2016.5.3 By Jason
            TB_OPTION1.Clear();
            TB_OPTION2.Clear();
            TB_OPTION3.Clear();
            TB_OPTION4.Clear();
            TB_OPTION5.Clear();
            TB_OPTION6.Clear();
            // 2016.5.3 End

            ShowData();
        }

        public void ShowData()
        {
            //判断查询条件
            if (!string.IsNullOrEmpty(txt_RCSN.Text))
            {
                switch (combRCSN.SelectedIndex)
                {
                    case 0:
                        sSQL = @"SELECT A.RC_NO,SAJET.SJ_RC_STATUS(A.CURRENT_STATUS) RC_STATUS,
                            DECODE(PROCESS_NAME, '', 'Group', PROCESS_NAME) PROCESS_NAME,CURRENT_QTY
                            FROM SAJET.G_RC_STATUS  A,
                            SAJET.SYS_PROCESS  B,
                            SAJET.SYS_PART C,
                            SAJET.SYS_PDLINE D
                            WHERE A.PROCESS_ID = B.PROCESS_ID(+) 
                            AND A.PROCESS_ID <>'0'  
                            AND A.PROCESS_ID IS NOT NULL 
                            AND A.PART_ID = C.PART_ID 
                            AND A.CURRENT_STATUS in(0,1)
                            AND A.PDLINE_ID = D.PDLINE_ID
                            AND A.RELEASE = 'N'
                            AND A.RC_NO LIKE '%" + txt_RCSN.Text + "%'";

                        break;
                    default:
                        sSQL = @"SELECT RC_NO FROM SAJET.G_SN_STATUS WHERE SERIAL_NUMBER LIKE '%" + txt_RCSN.Text + "%'";
                        dsTemp = ClientUtils.ExecuteSQL(sSQL);
                        if (dsTemp.Tables[0].Rows.Count == 0)
                            return;
                        string sRC = dsTemp.Tables[0].Rows[0]["RC_NO"].ToString();
                        if (sRC == "N/A")
                        {
                            sSQL = @"SELECT A.SERIAL_NUMBER,SAJET.SJ_RC_STATUS(A.CURRENT_STATUS) RC_STATUS,
                                                DECODE(PROCESS_NAME, '', 'Group', PROCESS_NAME) PROCESS_NAME,A.GOOD_QTY
                                                FROM SAJET.G_SN_STATUS  A,
                                                SAJET.SYS_PROCESS  B,
                                                SAJET.SYS_PART C 
                                                WHERE A.PROCESS_ID = B.PROCESS_ID(+) 
                                                AND A.PROCESS_ID <>'0'  
                                                AND A.PROCESS_ID IS NOT NULL 
                                                AND A.PART_ID = C.PART_ID 
                                                AND A.CURRENT_STATUS in(0,1)
                                                AND A.SERIAL_NUMBER LIKE'%" + txt_RCSN.Text + "%'";
                        }
                        else
                        {
                            string sMsg = SajetCommon.SetLanguage("CAN'T HOLD BY THE SN");
                            SajetCommon.Show_Message(sMsg, 1);
                            return;
                        }
                        break;
                }
            }
            else
            {
                switch (combRCSN.SelectedIndex)
                {
                    case 0:
                        sSQL = @"SELECT RC_NO,SAJET.SJ_RC_STATUS(A.CURRENT_STATUS) RC_STATUS,
                                    DECODE(PROCESS_NAME, '', 'Group', PROCESS_NAME) PROCESS_NAME,CURRENT_QTY
                                    FROM SAJET.G_RC_STATUS  A,
                                    SAJET.SYS_PROCESS  B,
                                    SAJET.SYS_PART C,
                                    SAJET.SYS_PDLINE D
                                    WHERE A.PROCESS_ID = B.PROCESS_ID(+) 
                                    AND A.PROCESS_ID <>'0'  
                                    AND A.PROCESS_ID IS NOT NULL 
                                    AND A.PART_ID = C.PART_ID
                                    AND A.PDLINE_ID = D.PDLINE_ID
                                    AND A.RELEASE = 'N'
                                    AND A.CURRENT_STATUS in(0,1)";
                        break;
                    default:
                        sSQL = @"SELECT A.SERIAL_NUMBER,SAJET.SJ_RC_STATUS(A.CURRENT_STATUS) RC_STATUS,
                                                DECODE(PROCESS_NAME, '', 'Group', PROCESS_NAME) PROCESS_NAME,A.GOOD_QTY
                                                FROM SAJET.G_SN_STATUS  A,
                                                SAJET.SYS_PROCESS  B,
                                                SAJET.SYS_PART C 
                                                WHERE A.PROCESS_ID = B.PROCESS_ID(+) 
                                                AND A.PROCESS_ID <>'0'  
                                                AND A.PROCESS_ID IS NOT NULL 
                                                AND A.PART_ID = C.PART_ID 
                                                AND A.CURRENT_STATUS in(0,1)
                                                AND A.RC_NO = 'N/A'";
                        break;
                }
            }

            if (PN_textBox.Text != "")
            {
                // 2016.5.3 By Jason
                //string s = PN_textBox.Text;//解析抓取的料号
                //string s1 = ParseArray(s);
                //sSQL = sSQL + "and c.part_no in(" + s1 + ")";

                sSQL = sSQL + " AND D.PDLINE_ID = " + PN_textBox.Text;

                // 2016.5.3 End
            }

            if (WO_textBox.Text.Trim() != "")
            {
                sSQL = sSQL + " AND UPPER(a.work_order) like UPPER('%" + WO_textBox.Text + "%')";//模糊查询
            }

            if (PN_textBox.Text == "" && WO_textBox.Text == "" && txt_RCSN.Text == "")
            {
                string sMsg = SajetCommon.SetLanguage("QUERY BUILDER IS NULL") + Environment.NewLine;
                SajetCommon.Show_Message(sMsg, 1);
                return;
            }

            // 2016.5.3 By Jason
            if (PN_textBox.Text == "")
            {

                string sMsg = SajetCommon.SetLanguage("Please Select Line Name") + Environment.NewLine;
                SajetCommon.Show_Message(sMsg, 1);
                return;
            }
            // 2016.5.3 End

            if (combRCSN.SelectedIndex == 0)
            {
                sSQL += " ORDER BY A.RC_NO";
            }
            else
            {
                sSQL += " ORDER BY A.SERIAL_NUMBER";
            }

            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                string sMsg = SajetCommon.SetLanguage("No relevant data");
                label2.Text = sMsg;
                return;
            }
            else
            {
                label2.Visible = false;
            }

            //RC_dgv RC_NO赋值
            if (combRCSN.SelectedIndex == 0)
            {
                RC_dgv.Columns[1].HeaderText = SajetCommon.SetLanguage("RC_NO");

                for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                {
                    RC_dgv.Rows.Add();
                    RC_dgv.Rows[i].Cells[0].Value = false;
                    RC_dgv.Rows[i].Cells[1].Value = dsTemp.Tables[0].Rows[i]["RC_NO"];
                    RC_dgv.Rows[i].Cells[2].Value = dsTemp.Tables[0].Rows[i]["PROCESS_NAME"];
                    RC_dgv.Rows[i].Cells[3].Value = dsTemp.Tables[0].Rows[i]["CURRENT_QTY"];
                    RC_dgv.Rows[i].Cells[4].Value = dsTemp.Tables[0].Rows[i]["RC_STATUS"];
                }
            }
            else
            {
                RC_dgv.Columns[1].HeaderText = SajetCommon.SetLanguage("SERIAL_NUMBER");

                for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                {
                    RC_dgv.Rows.Add();
                    RC_dgv.Rows[i].Cells[0].Value = false;
                    RC_dgv.Rows[i].Cells[1].Value = dsTemp.Tables[0].Rows[i]["SERIAL_NUMBER"];
                    RC_dgv.Rows[i].Cells[2].Value = dsTemp.Tables[0].Rows[i]["PROCESS_NAME"];
                    RC_dgv.Rows[i].Cells[3].Value = dsTemp.Tables[0].Rows[i]["GOOD_QTY"];
                    RC_dgv.Rows[i].Cells[4].Value = dsTemp.Tables[0].Rows[i]["RC_STATUS"];
                }
            }
        }

        public string ParseArray(string s)
        {
            string[] strs = s.Split(',');
            string s1 = "";

            for (int i = 0; i < strs.Length; i++)
            {
                if (s1 == "")
                {
                    s1 += "'" + strs[i] + "'";
                }
                else
                {
                    s1 += ",'" + strs[i] + "'";
                }
            }

            return s1;
        }

        private void btnPN_Click(object sender, EventArgs e)
        {
            // 2016.4.20 By Jason (防呆)
            RC_dgv.Rows.Clear();
            // 2016.4.20 End

            PN_textBox.Clear();
            Spec_textBox.Clear();

            //fAlert f = new fAlert();

            //try
            //{
            //    if (f.ShowDialog() == DialogResult.OK)
            //    {
            //        string s = f.sPARTNO;//获取所选料号
            //        PN_textBox.Text = s.Substring(0, s.Length - 1);//去掉末尾的逗号

            //        // 2016.5.2 By Jason
            //        string sSpec1 = f.sSpec1;//获取所选料号
            //        Spec_textBox.Text = sSpec1.Substring(0, sSpec1.Length - 1);//去掉末尾的逗号
            //        // 2016.5.2 End

            //    }
            //}
            //catch (Exception ex)
            //{
            //    SajetCommon.Show_Message("Exception:" + ex.Message, 0);
            //}
            //finally
            //{
            //    f.Dispose();
            //}

            // 2016.5.3 By Jason

            string sSQL = "   SELECT PDLINE_ID,PDLINE_NAME"
                        + "     FROM SAJET.SYS_PDLINE"
                        + "    WHERE ENABLED = 'Y'"
                        + " ORDER BY PDLINE_ID ASC";

            fFilter f = new fFilter();
            f.sSQL = sSQL;

            if (f.ShowDialog() == DialogResult.OK)
            {
                PN_textBox.Text = f.dgvData.CurrentRow.Cells["PDLINE_ID"].Value.ToString();
                Spec_textBox.Text = f.dgvData.CurrentRow.Cells["PDLINE_NAME"].Value.ToString();
            }

            // 2016.5.3 End
        }

        private void btnALLSELECT_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < RC_dgv.Rows.Count; i++)
            {
                RC_dgv.Rows[i].Cells[0].Value = true;
            }
        }

        private void btnALLCANCEL_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < RC_dgv.Rows.Count; i++)
            {
                RC_dgv.Rows[i].Cells[0].Value = false;
            }
        }
    }
}
