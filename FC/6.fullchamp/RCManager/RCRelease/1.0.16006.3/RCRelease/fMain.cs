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
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();
        }

        string sSQL;
        DataSet dsTemp;
        public static String rc_UserID = ClientUtils.UserPara1;
        string sHold_No, sRelease_desc;
        int n = 0;
        bool sRelease = false;
        bool sRE = true;


        private void fMain_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);

            //修改选择框为可选状态
            //Release_dgv.ReadOnly = false;
            label2.Visible = false;
            SetCombRCSN();
        }

        public void SetCombRCSN()
        {
            combRCSN.Items.Add(SajetCommon.SetLanguage("RC_NO"));
            combRCSN.Items.Add(SajetCommon.SetLanguage("SERIAL_NUMBER"));
            combRCSN.SelectedIndex = 0;
        }

        //料号选择按钮
        private void button1_Click(object sender, EventArgs e)
        {
            // 2016.4.20 By Jason (防呆)
            Release_dgv.Rows.Clear();
            RC_dgv.Rows.Clear();
            // 2016.4.20 End

            PN_TBOX.Clear();
            fPART_ID fp = new fPART_ID();

            try
            {
                if (fp.ShowDialog() == DialogResult.OK)
                {
                    string s = fp.sPARTNO;//获取所选料号
                    PN_TBOX.Text = s.Substring(0, s.Length - 1);//去掉末尾的逗号

                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception:" + ex.Message, 0);
            }
            finally
            {
                fp.Dispose();
            }
        }

        //为Release_dgv赋值
        public void ShowData()
        {
            //            //modify by hidy 2015/09/15
            //            sSQL = @"select distinct b.hold_no,d.defect_type_desc,b.hold_userid,round(((sysdate-b.hold_time)*24),0) hold_interval,b.hold_desc 
            //                    from sajet.g_rc_status a,
            //                             sajet.g_rc_hold_status b,
            //                             sajet.g_rc_hold_detail c,
            //                             sajet.sys_defect_type d,
            //                             sajet.sys_part e 
            //                    where b.hold_no = c.hold_no
            //                        and b.defect_type_id = d.defect_type_id  
            //                        and a.current_status = '2'
            //                        and b.status = 'HOLD'";
            if (!string.IsNullOrEmpty(txt_RCSN.Text))
            {
                switch (combRCSN.SelectedIndex)
                {
                    case 0:
                        //                        sSQL = @"SELECT DISTINCT B.HOLD_NO,D.DEFECT_TYPE_DESC,B.HOLD_USERID,ROUND(((SYSDATE-B.HOLD_TIME)*24),0) HOLD_INTERVAL,B.HOLD_DESC 
                        //                                          FROM SAJET.G_RC_STATUS A,
                        //                                                   SAJET.G_RC_HOLD_STATUS B,
                        //                                                   SAJET.G_RC_HOLD_DETAIL C,
                        //                                                   SAJET.SYS_DEFECT_TYPE D,
                        //                                                   SAJET.SYS_PART E 
                        //                                        WHERE A.RC_NO LIKE '%" + txt_RCSN.Text + @"%'
                        //                                            AND A.RC_NO = C.RC_NO 
                        //
                        //                                            AND B.HOLD_NO = C.HOLD_NO
                        //                                            AND B.DEFECT_TYPE_ID = D.DEFECT_TYPE_ID  
                        //                                            AND A.CURRENT_STATUS = '2'
                        //                                            AND B.STATUS = 'HOLD'";

                        sSQL = @"SELECT A.HOLD_NO,D.DEFECT_TYPE_DESC,F.EMP_NAME,ROUND(((SYSDATE-A.HOLD_TIME)*24),0) HOLD_INTERVAL,A.HOLD_DESC
                                   FROM SAJET.G_RC_HOLD_STATUS A,
                                        SAJET.G_RC_HOLD_DETAIL B,
                                        SAJET.G_RC_STATUS C,
                                        SAJET.SYS_DEFECT_TYPE D,
                                        SAJET.SYS_PART E,
                                        SAJET.SYS_EMP F
                                  WHERE A.HOLD_NO = B.HOLD_NO
                                    AND B.RC_NO = C.RC_NO
                                    AND A.DEFECT_TYPE_ID = D.DEFECT_TYPE_ID
                                    AND C.PART_ID = E.PART_ID
                                    AND A.HOLD_USERID = F.EMP_ID
                                    AND B.RC_NO <> 'N/A'
                                    AND C.CURRENT_STATUS = 2
                                    AND A.STATUS = 'HOLD'
                                    AND B.RC_NO LIKE '%" + txt_RCSN.Text + @"%'";
                        break;
                    default:
                        sSQL = @"SELECT DISTINCT B.HOLD_NO,D.DEFECT_TYPE_DESC,B.HOLD_USERID,ROUND(((SYSDATE-B.HOLD_TIME)*24),0) HOLD_INTERVAL,B.HOLD_DESC 
                                          FROM SAJET.G_SN_STATUS A,
                                                     SAJET.G_RC_HOLD_STATUS B,
                                                     SAJET.G_RC_HOLD_DETAIL C,
                                                     SAJET.SYS_DEFECT_TYPE D,
                                                     SAJET.SYS_PART E 
                                        WHERE A.SERIAL_NUMBER LIKE '%" + txt_RCSN.Text + @"%'
                                            AND A.SERIAL_NUMBER = C.SERIAL_NUMBER
                                            AND B.HOLD_NO = C.HOLD_NO
                                            AND B.DEFECT_TYPE_ID = D.DEFECT_TYPE_ID  
                                            AND A.CURRENT_STATUS = '2'
                                            AND B.STATUS = 'HOLD'";
                        break;
                }
            }
            else
            {
                switch (combRCSN.SelectedIndex)
                {
                    case 0:
                        //                        sSQL = @"SELECT DISTINCT B.HOLD_NO,D.DEFECT_TYPE_DESC,B.HOLD_USERID,ROUND(((SYSDATE-B.HOLD_TIME)*24),0) HOLD_INTERVAL,B.HOLD_DESC 
                        //                                          FROM SAJET.G_RC_STATUS A,
                        //                                                   SAJET.G_RC_HOLD_STATUS B,
                        //                                                   SAJET.G_RC_HOLD_DETAIL C,
                        //                                                   SAJET.SYS_DEFECT_TYPE D,
                        //                                                   SAJET.SYS_PART E 
                        //                                        WHERE B.HOLD_NO = C.HOLD_NO
                        //                                            AND C.RC_NO <>'N/A' 
                        //                                            AND B.DEFECT_TYPE_ID = D.DEFECT_TYPE_ID  
                        //                                            AND A.CURRENT_STATUS = '2'
                        //                                            AND B.STATUS = 'HOLD'";

                        sSQL = @"SELECT A.HOLD_NO,D.DEFECT_TYPE_DESC,F.EMP_NAME,ROUND(((SYSDATE-A.HOLD_TIME)*24),0) HOLD_INTERVAL,A.HOLD_DESC
                                   FROM SAJET.G_RC_HOLD_STATUS A,
                                        SAJET.G_RC_HOLD_DETAIL B,
                                        SAJET.G_RC_STATUS C,
                                        SAJET.SYS_DEFECT_TYPE D,
                                        SAJET.SYS_PART E,
                                        SAJET.SYS_EMP F
                                  WHERE A.HOLD_NO = B.HOLD_NO
                                    AND B.RC_NO = C.RC_NO
                                    AND A.DEFECT_TYPE_ID = D.DEFECT_TYPE_ID
                                    AND C.PART_ID = E.PART_ID
                                    AND A.HOLD_USERID = F.EMP_ID
                                    AND B.RC_NO <> 'N/A'
                                    AND C.CURRENT_STATUS = 2
                                    AND A.STATUS = 'HOLD'";
                        break;
                    default:
                        sSQL = @"SELECT DISTINCT B.HOLD_NO,D.DEFECT_TYPE_DESC,B.HOLD_USERID,ROUND(((SYSDATE-B.HOLD_TIME)*24),0) HOLD_INTERVAL,B.HOLD_DESC 
                                          FROM SAJET.G_SN_STATUS A,
                                                     SAJET.G_RC_HOLD_STATUS B,
                                                     SAJET.G_RC_HOLD_DETAIL C,
                                                     SAJET.SYS_DEFECT_TYPE D,
                                                     SAJET.SYS_PART E 
                                        WHERE C.SERIAL_NUMBER <> 'N/A'  
                                            AND B.HOLD_NO = C.HOLD_NO
                                            AND B.DEFECT_TYPE_ID = D.DEFECT_TYPE_ID  
                                            AND A.CURRENT_STATUS = '2'
                                            AND B.STATUS = 'HOLD'";
                        break;
                }
            }

            if (PN_TBOX.Text != "")
            {
                string s = PN_TBOX.Text;//解析抓取的料号
                string s1 = ParseArray(s);
                //sSQL = sSQL + "and e.part_no in(" + s1 + ") and a.part_id = e.part_id ";
                sSQL = sSQL + " AND E.PART_NO IN(" + s1 + ") ";
            }
            if (txtWO.Text.Trim() != "")
            {
                //sSQL = sSQL + "and UPPER(a.work_order) like UPPER('%" + txtWO.Text + "%')";//模糊查询
                sSQL = sSQL + " AND UPPER(C.WORK_ORDER) LIKE UPPER('%" + txtWO.Text + "%') ";
            }
            if (txtHold_no.Text != "")
            {
                //sSQL = sSQL + "and b.hold_no like ('%" + txtHold_no.Text + "%')";
                sSQL = sSQL + " AND B.HOLD_NO LIKE ('%" + txtHold_no.Text + "%') ";
            }

            //sSQL = sSQL + "ORDER BY HOLD_NO DESC";
            sSQL = sSQL + " GROUP BY A.HOLD_NO,D.DEFECT_TYPE_DESC,F.EMP_NAME,ROUND(((SYSDATE-A.HOLD_TIME)*24),0),A.HOLD_DESC ";
            sSQL = sSQL + " ORDER BY HOLD_INTERVAL DESC,A.HOLD_NO ASC ";

            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            if (dsTemp.Tables[0].Rows.Count <= 0)
            {
                RC_dgv.Rows.Clear();
                if (sRE)
                {
                    label2.Visible = true;
                    string sMsg = SajetCommon.SetLanguage("No relevant data");
                    label2.Text = sMsg;
                }
                return;
            }
            label2.Visible = false;

            sRelease = false;
            for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
            {
                Release_dgv.Rows.Add();
                Release_dgv.Rows[i].Cells[0].Value = false;
                Release_dgv.Rows[i].Cells[1].Value = dsTemp.Tables[0].Rows[i]["HOLD_NO"];
                Release_dgv.Rows[i].Cells[2].Value = dsTemp.Tables[0].Rows[i]["DEFECT_TYPE_DESC"];
                Release_dgv.Rows[i].Cells[3].Value = dsTemp.Tables[0].Rows[i]["EMP_NAME"];
                Release_dgv.Rows[i].Cells[4].Value = dsTemp.Tables[0].Rows[i]["HOLD_INTERVAL"];
                Release_dgv.Rows[i].Cells[5].Value = dsTemp.Tables[0].Rows[i]["HOLD_DESC"];

            }
            //默认选定第一行
            sRelease = true;
            Release_dgv.Rows[0].Selected = true;
            //ShowDetailData();
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

        private void QUERY_btn_Click(object sender, EventArgs e)
        {

            Release_dgv.Rows.Clear();
            RC_dgv.Rows.Clear();
            PN_TBOX.Focus();
            Release_richTextBox.Clear();

            ShowData();

        }

        private void Release_dgv_SelectionChanged(object sender, EventArgs e)
        {
            if (!sRelease)
            {
                return;
            }
            if (Release_dgv.Rows.Count <= 0)
            {
                RC_dgv.Rows.Clear();
                return;
            }


            sHold_No = Release_dgv.CurrentRow.Cells["hold_no"].Value.ToString();

            ShowDetailData();

        }
        //为RC_dgv赋值
        public void ShowDetailData()
        {
            RC_dgv.Rows.Clear();

            //            sSQL = @"SELECT A.RC_NO,DECODE(PROCESS_NAME, '', 'Group', PROCESS_NAME) PROCESS_NAME,C.QTY,A.SERIAL_NUMBER
            //                              FROM SAJET.G_RC_HOLD_DETAIL A,
            //                                         SAJET.SYS_PROCESS  B,
            //                                         SAJET.G_RC_HOLD_STATUS C
            //                            WHERE A.PROCESS_ID = B.PROCESS_ID(+)
            //                                AND A.HOLD_NO = C.HOLD_NO
            //                                AND A.HOLD_NO = '" + sHold_No + "'";

            sSQL = @"SELECT B.RC_NO,B.SERIAL_NUMBER,D.PROCESS_NAME,C.CURRENT_QTY
                       FROM SAJET.G_RC_HOLD_STATUS A,
                            SAJET.G_RC_HOLD_DETAIL B,
                            SAJET.G_RC_STATUS C,
                            SAJET.SYS_PROCESS D
                      WHERE A.HOLD_NO = B.HOLD_NO
                        AND B.RC_NO = C.RC_NO
                        AND B.PROCESS_ID = D.PROCESS_ID
                        AND A.HOLD_NO = '" + sHold_No + "'";

            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            if (dsTemp.Tables[0].Rows.Count <= 0)
                return;

            for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
            {
                RC_dgv.Rows.Add();
                RC_dgv.Rows[i].Cells[0].Value = dsTemp.Tables[0].Rows[i]["RC_NO"];
                RC_dgv.Rows[i].Cells[1].Value = dsTemp.Tables[0].Rows[i]["SERIAL_NUMBER"];
                RC_dgv.Rows[i].Cells[2].Value = dsTemp.Tables[0].Rows[i]["PROCESS_NAME"];
                RC_dgv.Rows[i].Cells[3].Value = dsTemp.Tables[0].Rows[i]["CURRENT_QTY"];
            }
        }

        private void RELEASE_btn_Click(object sender, EventArgs e)
        {
            if (Release_dgv.Rows.Count <= 0)
                return;

            fAlert f = new fAlert();

            try
            {
                string IsChecked = "";
                foreach (DataGridViewRow row in Release_dgv.Rows)
                {
                    string s = Convert.ToString(row.Cells[3].Value);
                    bool check = Convert.ToBoolean(row.Cells[0].FormattedValue);
                    if (check == true)
                    {
                        IsChecked = "OK";
                        break;
                    }
                    n++;
                }

                //如果没有选择，弹出窗口
                if (IsChecked != "OK")
                {
                    string sMsg = SajetCommon.SetLanguage("Please select hold_no");
                    SajetCommon.Show_Message(sMsg, 1);
                    return;
                }

                if (Release_richTextBox.Text.Trim() == "")
                {
                    string sMsg = SajetCommon.SetLanguage("Release DESC is null");
                    SajetCommon.Show_Message(sMsg, 1);
                    return;
                }

                sRelease_desc = Release_richTextBox.Text;

                f.sHold_NO = sHold_No;
                f.sRelease_desc = sRelease_desc;
                f.iCombRCSN = combRCSN.SelectedIndex;

                if (f.ShowDialog() == DialogResult.OK)
                {
                    sRelease = false;
                    Release_dgv.Rows.Clear();
                    Release_richTextBox.Clear();
                    sRelease = true;
                    sRE = false;
                    ShowData();
                    sRE = true;
                    string sMsg = SajetCommon.SetLanguage("Exception list completed");
                    SajetCommon.Show_Message(sMsg, 3);

                    //PN_TBOX.Clear();
                    //txtWO.Clear();
                    //RC_TBOX.Clear();
                    //txtHold_no.Clear();
                    //Release_richTextBox.Clear();
                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception:" + ex.Message, 0);
            }
            finally
            {
                f.Dispose();
            }
        }
        //Release_dgv单选操作
        private void Release_dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int n = 0;
            foreach (DataGridViewRow row in Release_dgv.Rows)
            {
                string s = Convert.ToString(row.Cells[3].Value);
                bool check = Convert.ToBoolean(row.Cells[0].FormattedValue);
                if (check == true)
                {
                    Release_dgv.Rows[n].Cells[0].Value = false;
                }
                n++;
            }
        }
    }
}
