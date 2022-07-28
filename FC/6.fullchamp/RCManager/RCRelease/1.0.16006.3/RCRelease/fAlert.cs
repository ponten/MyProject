using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SajetClass;


namespace RC_Release
{
    public partial class fAlert : Form
    {
        public fAlert()
        {
            InitializeComponent();
        }

        public string sHold_NO, sRelease_desc, sCurrent_Status, sRC_NO,sSerial_Number;
        public int iCombRCSN;
        string sSQL;
        DataSet dsTemp;
        DataSet dsTemp1;
        public static String rc_UserID = ClientUtils.UserPara1;

        private void fAlert_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
           
        }
        
        private void btnCANCEL_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //点击ok按钮后判断是否有选择release方式(3种方式)
        private void btnOK_Click(object sender, EventArgs e)
        {
            if ((radioButton1.Checked == false) && (radioButton2.Checked == false) && (radioButton3.Checked == false))
            {
                return;
            }
            try
            {
                if (radioButton1.Checked)//续GO
                {
                    if (iCombRCSN == 0)
                    {
                        sSQL = @"select current_status,rc_no from sajet.g_rc_hold_detail where hold_no = '" + sHold_NO + "'";
                        dsTemp = ClientUtils.ExecuteSQL(sSQL);
                        if (dsTemp.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                            {
                                sCurrent_Status = dsTemp.Tables[0].Rows[i]["current_status"].ToString();
                                sRC_NO = dsTemp.Tables[0].Rows[i]["rc_no"].ToString();

                                sSQL = @"update sajet.g_rc_status  
                                            set current_status = '" + sCurrent_Status + @"',
                                                update_userid = '" + rc_UserID + @"',
                                                update_time = sysdate
                                            where rc_no = '" + sRC_NO + "'";
                                dsTemp1 = ClientUtils.ExecuteSQL(sSQL);

                                sSQL = @"update sajet.g_rc_hold_status
                                            set status = 'RELEASE',
                                                update_userid = '" + rc_UserID + @"',
                                                update_time = sysdate,
                                                release_userid = '" + rc_UserID + @"',
                                                release_time = sysdate,
                                                release_desc = '" + sRelease_desc + @"',
                                                release_type = 'GO'
                                            where hold_no = '" + sHold_NO + "'";
                                dsTemp1 = ClientUtils.ExecuteSQL(sSQL);
                            }
                        }
                    }
                    else
                    {
                        sSQL = @"select current_status,serial_number from sajet.g_rc_hold_detail where hold_no = '" + sHold_NO + "'";
                        dsTemp = ClientUtils.ExecuteSQL(sSQL);
                        if (dsTemp.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                            {
                                sCurrent_Status = dsTemp.Tables[0].Rows[i]["current_status"].ToString();
                                sSerial_Number = dsTemp.Tables[0].Rows[i]["serial_number"].ToString();

                                sSQL = @"update sajet.g_sn_status  
                                            set current_status = '" + sCurrent_Status + @"'
                                            where serial_number = '" + sSerial_Number + "'";
                                dsTemp1 = ClientUtils.ExecuteSQL(sSQL);

                                sSQL = @"update sajet.g_rc_hold_status
                                            set status = 'RELEASE',
                                                release_userid = '" + rc_UserID + @"',
                                                release_time = sysdate,
                                                release_desc = '" + sRelease_desc + @"',
                                                release_type = 'go'
                                            where hold_no = '" + sHold_NO + "'";
                                dsTemp1 = ClientUtils.ExecuteSQL(sSQL);
                            }
                        }
                    }
                    DialogResult = DialogResult.OK;











                }
                else if (radioButton2.Checked)//COMPLETE
                {
                    if (iCombRCSN == 0)
                    {
                        sSQL = @"select rc_no from sajet.g_rc_hold_detail where hold_no = '" + sHold_NO + "'";
                        dsTemp = ClientUtils.ExecuteSQL(sSQL);

                        if (dsTemp.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                            {
                                sRC_NO = dsTemp.Tables[0].Rows[i]["rc_no"].ToString();

                                sSQL = @"update sajet.g_rc_status  
                                          set current_status = '9',
                                              update_userid = '" + rc_UserID + @"',
                                              update_time = sysdate
                                           where rc_no = '" + sRC_NO + "'";
                                dsTemp1 = ClientUtils.ExecuteSQL(sSQL);

                                sSQL = @"update sajet.g_rc_hold_status
                                          set status = 'RELEASE',
                                                update_userid = '" + rc_UserID + @"',
                                                update_time = sysdate,
                                                release_userid = '" + rc_UserID + @"',
                                                release_time = sysdate,
                                                release_desc = '" + sRelease_desc + @"',
                                                release_type = 'FINISH'
                                          where hold_no = '" + sHold_NO + "'";
                                dsTemp1 = ClientUtils.ExecuteSQL(sSQL);
                            }
                        }



                    }
                    else
                    {
                        sSQL = @"select serial_number from sajet.g_rc_hold_detail where hold_no = '" + sHold_NO + "'";
                        dsTemp = ClientUtils.ExecuteSQL(sSQL);
                        if (dsTemp.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                            {
                                sSerial_Number = dsTemp.Tables[0].Rows[i]["serial_number"].ToString();

                                sSQL = @"update sajet.g_sn_status  
                                          set current_status = '9'
                                           where serial_number = '" + sSerial_Number + "'";
                                dsTemp1 = ClientUtils.ExecuteSQL(sSQL);

                                sSQL = @"update sajet.g_rc_hold_status
                                          set status = 'RELEASE',
                                                release_userid = '" + rc_UserID + @"',
                                                release_time = sysdate,
                                                release_desc = '" + sRelease_desc + @"',
                                                release_type = 'complete'
                                          where hold_no = '" + sHold_NO + "'";
                                dsTemp1 = ClientUtils.ExecuteSQL(sSQL);
                            }
                        }
                    }
                    DialogResult = DialogResult.OK;
                }











                else if (radioButton3.Checked)//SCRAP
                {
                    if (iCombRCSN == 0)
                    {
                        sSQL = @"select rc_no from sajet.g_rc_hold_detail where hold_no = '" + sHold_NO + "'";
                        dsTemp = ClientUtils.ExecuteSQL(sSQL);
                        if (dsTemp.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                            {
                                sRC_NO = dsTemp.Tables[0].Rows[i]["rc_no"].ToString();

                                sSQL = @"update sajet.g_rc_status  
                                          set current_status = '8',
                                                update_userid = '" + rc_UserID + @"',
                                                update_time = sysdate
                                           where rc_no = '" + sRC_NO + "'";
                                dsTemp1 = ClientUtils.ExecuteSQL(sSQL);

                                sSQL = @"update sajet.g_rc_hold_status
                                          set status = 'RELEASE',
                                                update_userid = '" + rc_UserID + @"',
                                                update_time = sysdate,
                                                release_userid = '" + rc_UserID + @"',
                                                release_time = sysdate,
                                                release_desc = '" + sRelease_desc + @"',
                                                release_type = 'SCRAP'
                                          where hold_no = '" + sHold_NO + "'";
                                dsTemp1 = ClientUtils.ExecuteSQL(sSQL);

                            }
                        }
                    }
                    else
                    {
                        sSQL = @"select serial_number from sajet.g_rc_hold_detail where hold_no = '" + sHold_NO + "'";
                        dsTemp = ClientUtils.ExecuteSQL(sSQL);
                        if (dsTemp.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                            {
                                sSerial_Number = dsTemp.Tables[0].Rows[i]["serial_number"].ToString();

                                sSQL = @"update sajet.g_sn_status  
                                          set current_status = '8'
                                           where serial_number = '" + sSerial_Number + "'";
                                dsTemp1 = ClientUtils.ExecuteSQL(sSQL);

                                sSQL = @"update sajet.g_rc_hold_status
                                          set status = 'RELEASE',
                                                release_userid = '" + rc_UserID + @"',
                                                release_time = sysdate,
                                                release_desc = '" + sRelease_desc + @"',
                                                release_type = 'scrap'
                                          where hold_no = '" + sHold_NO + "'";
                                dsTemp1 = ClientUtils.ExecuteSQL(sSQL);
                            }
                        }
                    }
                    DialogResult = DialogResult.OK;
                }
                
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception:" + ex.Message, 0);
            }
        }




    }
}
