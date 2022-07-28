using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Xml;
using Lassalle.Flow;
using System.Data.OracleClient;

namespace RC_ChangeRoute
{
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();
        }

        string sSQL;
        DataSet dsTemp;
        string sRC, sRoute_Id, sProcess_Id, sPartID, sCRoute_Id;
        string sNode_Id, sNext_Node, sNext_Process, sSheet_Name = "0", sNode_type;
        string g_sUserID = ClientUtils.UserPara1;
        string g_sRouteID, g_sProcessID;
        int p = 0, r = 0;
        string sLink_Name;
        XmlDocument Xmldoc = new XmlDocument();
        AddFlow addflow = new AddFlow();

        private void fMain_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);

            addflow.Parent = this.panel1;
            addflow.Dock = DockStyle.Fill;
            addflow.AutoScroll = true;
            addflow.BackColor = SystemColors.Window;
            addflow.CanDrawNode = false;
            addflow.CanSizeNode = false;
            addflow.CanDrawLink = false;
            addflow.CanReflexLink = false;
            addflow.CanStretchLink = false;
            addflow.CanDragScroll = false;
            addflow.CanChangeOrg = false;
            addflow.CanChangeDst = false;
            addflow.CanMoveNode = false;

            // 2016.5.6 By Jason (避免重複委派)
            addflow.MouseDown += new MouseEventHandler(fMain_MouseDown);
            // 2016.5.6 End
        }

        //输入RC回车带出相关信息
        private void txtRC_NO_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                ClearData();
                ShowData();

                // 2016.5.4 By Jason
                btnGP_Click(sender, e);
                // 2016.5.4 End
            }
        }

        public void ShowData()
        {
            sRC = txtRC_NO.Text;

            if (sRC == "" || sRC == null)
                return;

            try
            {
                // 2016.4.21 By Jason
                sSQL = "SELECT RC_NO,CURRENT_STATUS FROM SAJET.G_RC_STATUS WHERE RC_NO ='" + sRC + "' AND RELEASE = 'N' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);

                if (dsTemp.Tables[0].Rows.Count == 0)
                {
                    string sMsg = SajetCommon.SetLanguage("RC No Exist");
                    SajetCommon.Show_Message(sMsg, 1);
                    return;
                }
                else
                {
                    string sStatus = dsTemp.Tables[0].Rows[0]["CURRENT_STATUS"].ToString();

                    if (sStatus == "6" || sStatus == "7" || sStatus == "8" || sStatus == "9" || sStatus == "10" || sStatus == "11" || sStatus == "12")
                    {
                        string sMsg = SajetCommon.SetLanguage("RC Current Status Close");
                        SajetCommon.Show_Message(sMsg, 1);
                        return;
                    }

                    if (sStatus == "3" || sStatus == "4" || Convert.ToInt32(sStatus) > 10 || Convert.ToInt32(sStatus) < 0)
                    {
                        string sMsg = SajetCommon.SetLanguage("RC Current Status Error");
                        SajetCommon.Show_Message(sMsg, 1);
                        return;
                    }
                }
                // 2016.4.21 End

                //Modify By hidy 2015/08/12 加左外连接
                sSQL = @"select d.part_no,a.work_order,c.process_name,b.route_name,a.route_id,d.part_id,a.current_status,a.process_id,d.spec1,d.spec2,d.version,a.node_id
                           from sajet.g_rc_status a,
                                sajet.sys_rc_route b,
                                sajet.sys_process c,
                                sajet.sys_part d 
                          where a.rc_no = '" + sRC + @"'
                            and a.part_id = d.part_id 
                            and a.route_id = b.route_id 
                            and a.process_id = c.process_id(+)
                            and a.release = 'N' 
                            and a.current_status in('0','1','2')";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);

                if (dsTemp.Tables[0].Rows.Count == 0)
                {
                    string sMsg = SajetCommon.SetLanguage("RC Have Not Part or Route");
                    SajetCommon.Show_Message(sMsg, 1);
                    return;
                }

                string sCurrent_status = dsTemp.Tables[0].Rows[0]["current_status"].ToString();

                if (sCurrent_status == "0")
                {
                    // 2016.5.4 By Jason
                    //txtPART_NO.Text = dsTemp.Tables[0].Rows[0]["part_no"].ToString();
                    txtPART_NO.Text = dsTemp.Tables[0].Rows[0]["spec1"].ToString();
                    txtSpec2.Text = dsTemp.Tables[0].Rows[0]["spec2"].ToString();
                    txtVersion.Text = dsTemp.Tables[0].Rows[0]["version"].ToString();
                    // 2016.5.4 End
                    txtCP.Text = dsTemp.Tables[0].Rows[0]["process_name"].ToString();
                    txtWO.Text = dsTemp.Tables[0].Rows[0]["work_order"].ToString();
                    txtCR.Text = dsTemp.Tables[0].Rows[0]["route_name"].ToString();
                    sRoute_Id = dsTemp.Tables[0].Rows[0]["route_id"].ToString();
                    sPartID = dsTemp.Tables[0].Rows[0]["part_id"].ToString();
                    g_sProcessID = dsTemp.Tables[0].Rows[0]["process_id"].ToString();
                    sCRoute_Id = dsTemp.Tables[0].Rows[0]["route_id"].ToString();
                }
                else if (sCurrent_status == "1")
                {
                    string sMsg = SajetCommon.SetLanguage("The RC is working,do not select");
                    SajetCommon.Show_Message(sMsg, 1);
                    return;
                }
                else if (sCurrent_status == "2")
                {
                    fAlert f = new fAlert();

                    f.sRC = sRC;

                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        ClearData();

                        if (f.s_RC_End == "Release")
                        {
                            txtRC_NO.Text = sRC;
                            ShowData();
                        }
                        else if (f.s_RC_End == "Scrap")
                        {
                            txtRC_NO.Clear();
                            combGRN.Items.Clear();

                            return;
                        }
                        else
                        {
                            txtRC_NO.Text = f.s_RC_End;
                            ShowData();
                        }

                        //ClearData();

                        //if (f.s_RC_End != "")
                        //{
                        //    txtRC_NO.Text = f.s_RC_End;
                        //}
                        //else
                        //{
                        //    txtRC_NO.Text = sRC;
                        //}

                        //ShowData();
                    }
                    else
                    {
                        string sMsg = SajetCommon.SetLanguage("The RC is holding,do not select");
                        SajetCommon.Show_Message(sMsg, 1);
                        return;
                    }
                }
                else
                {
                    string sMsg = SajetCommon.SetLanguage("RC No Exist");
                    SajetCommon.Show_Message(sMsg, 1);
                    return;
                }

                setCombGRN();
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);
            }
        }

        //跳站按钮
        private void btnGP_Click(object sender, EventArgs e)
        {
            // 2016.4.21 By Jason
            //if (txtCP.Text == "" || txtCP.Text == null)
            //    return;
            if (txtCP.Text == "" || txtCP.Text == null)
                return;
            // 2016.4.21 End

            if (r > 0)
                return;
            p++;
            btnGR.Enabled = false;
            ShowProcessDetail();
        }

        public void ShowProcessDetail()
        {
            //combGRN.Enabled = true;
            combGRN.SelectedItem = txtCR.Text;
            //combGRN.Enabled = false; 

            // 2016.5.6 By Jason (重複委派)
            //addflow.MouseDown += new MouseEventHandler(fMain_MouseDown);
            // 2016.5.6 End

            addflow.Nodes.Clear();
            addflow.ReadXml(GetRouteXML());
        }

        //跳途程按钮
        private void btnGR_Click(object sender, EventArgs e)
        {
            if (txtCR.Text == "" || txtCR.Text == null)
                return;
            if (p > 0)
                return;
            r++;
            btnGP.Enabled = false;
            combGRN.Enabled = true;
            ShowProcessDetail();
        }

        //选择要跳的途程时显示相应的途程图
        private void combGRN_SelectedValueChanged(object sender, EventArgs e)
        {
            string g_sRoute = combGRN.SelectedItem.ToString();

            sSQL = "select route_id from sajet.sys_rc_route where route_name = '" + g_sRoute + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            g_sRouteID = dsTemp.Tables[0].Rows[0]["route_id"].ToString();

            ShowRouteData();
        }

        public void ShowRouteData()
        {
            sRoute_Id = g_sRouteID;

            // 2016.5.6 By Jason (重複委派)
            //addflow.MouseDown += new MouseEventHandler(fMain_MouseDown);
            // 2016.5.6 End

            addflow.Nodes.Clear();
            addflow.ReadXml(GetRouteXML());
        }

        //确定以后执行对应的跳站或跳流程
        private void button1_Click(object sender, EventArgs e)
        {
            //执行跳站
            if (p == 1 && r == 0)
            {
                string sGP = txtGP.Text;

                if (sGP == "" || sGP == null)
                {
                    string sMsg = SajetCommon.SetLanguage("please select go_processName");
                    SajetCommon.Show_Message(sMsg, 1);
                    return;
                }

                string sRelease_Desc = Release_richTxt.Text.Trim();

                if (sRelease_Desc == "" || sRelease_Desc == null)
                {
                    string sMsg = SajetCommon.SetLanguage("Release DESC is null");
                    SajetCommon.Show_Message(sMsg, 1);
                    return;
                }

                try
                {
                    string sMsg = SajetCommon.SetLanguage("GO Process") + "?";

                    if (SajetCommon.Show_Message(sMsg, 2) == DialogResult.Yes)
                    {
                        // 2016.5.10 By Jason
//                        if (sNode_type == "1")
//                        {
//                            sSQL = @"update sajet.g_rc_status 
//                                        set process_id = '" + sProcess_Id + @"'
//                                            ,node_id = '" + sNode_Id + @"'
//                                            ,next_node = '" + sNext_Node + @"'
//                                            ,next_process = '" + sNext_Process + @"'
//                                            ,sheet_name = '" + sSheet_Name + @"'
//                                            ,update_userid = '" + g_sUserID + @"'
//                                            ,update_time = sysdate 
//                                      where rc_no = '" + sRC + "'";
//                        }
//                        else if (sNode_type == "2" || sNode_type == "3")
//                        {
//                            //                            sSQL = @"update sajet.g_rc_status 
//                            //                            set process_id = ''
//                            //                            ,node_id = '" + sNode_Id + @"'
//                            //                            ,next_node = '" + sNext_Node + @"'
//                            //                            ,next_process = null
//                            //                            ,sheet_name = ''
//                            //                            ,update_userid = '" + g_sUserID + @"'
//                            //                            ,update_time = sysdate 
//                            //                            where rc_no = '" + sRC + "'";

//                            // 2016.4.21 By Jason
//                            sSQL = @"update sajet.g_rc_status 
//                                        set process_id = '0'
//                                            ,node_id = '" + sNode_Id + @"'
//                                            ,next_node = '" + sNext_Node + @"'
//                                            ,next_process = '0'
//                                            ,sheet_name = '0'
//                                            ,update_userid = '" + g_sUserID + @"'
//                                            ,update_time = sysdate 
//                                      where rc_no = '" + sRC + "'";
//                            // 2016.4.21 End
//                        }
                        // 2016.5.10 End



                        // 2016.5.10 By Jason
                        string sStage_Id = "0";
                        DateTime datExeTime = DateTime.Now;

                        sSQL = " SELECT STAGE_ID FROM SAJET.SYS_PROCESS WHERE PROCESS_ID = " + sProcess_Id;
                        dsTemp = ClientUtils.ExecuteSQL(sSQL);

                        if (dsTemp.Tables[0].Rows.Count == 0)
                            return;

                        sStage_Id = dsTemp.Tables[0].Rows[0]["STAGE_ID"].ToString();

                        sSQL = " UPDATE SAJET.G_RC_STATUS "
                             + "    SET STAGE_ID = :STAGE_ID,"
                             + "        NODE_ID = :NODE_ID,"
                             + "        PROCESS_ID = :PROCESS_ID,"
                             + "        SHEET_NAME = :SHEET_NAME,"
                             + "        NEXT_NODE = :NEXT_NODE,"
                             + "        NEXT_PROCESS = :NEXT_PROCESS,"
                             + "        UPDATE_USERID = :UPDATE_USERID,"
                             + "        UPDATE_TIME = :UPDATE_TIME,"
                             + "        BONUS_QTY = :BONUS_QTY,"
                             + "        WORKTIME = :WORKTIME,"
                             + "        RELEASE = :RELEASE"
                             + "  WHERE RC_NO = :RC_NO";
                        object[][] oParams = new object[12][];
                        oParams[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "STAGE_ID", sStage_Id };
                        oParams[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NODE_ID", sNode_Id };
                        oParams[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", sProcess_Id };
                        oParams[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_NAME", sSheet_Name };
                        oParams[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NEXT_NODE", sNext_Node };
                        oParams[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NEXT_PROCESS", sNext_Process };
                        oParams[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        oParams[7] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime };
                        oParams[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", sRC };
                        oParams[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BONUS_QTY", 0 };
                        oParams[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORKTIME", 0 };
                        oParams[11] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RELEASE", "N" };
                        dsTemp = ClientUtils.ExecuteSQL(sSQL, oParams);
                        // 2016.5.10 End

                        //modify by hidy 2015/09/02
                        //delete table G_RC_PROCESS_GROUP中的残留资料
                        sSQL = @"DELETE FROM SAJET.G_RC_PROCESS_GROUP WHERE RC_NO = '" + sRC + "'";
                        dsTemp = ClientUtils.ExecuteSQL(sSQL);

                        //modify by hidy 2015/08/13
                        //记录跳站log
                        sSQL = @"insert into sajet.g_rc_changeRoute_history
                                 (rc_no,current_route_id,to_route_id,current_process_id,to_process_id,changeroute_desc,update_userid,update_time,change_type)
                                 values
                                 (:rc_no,:current_route_id,:to_route_id,:current_process_id,:to_process_id,:changeroute_desc,:update_userid,:update_time,'PROCESS')";
                        object[][] Params = new object[8][];
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "rc_no", sRC };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "current_route_id", sRoute_Id };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "to_route_id", sRoute_Id };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "current_process_id", g_sProcessID };
                        Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "to_process_id", sProcess_Id };
                        Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "changeroute_desc", sRelease_Desc };
                        Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "update_userid", g_sUserID };
                        Params[7] = new object[] { ParameterDirection.Input, OracleType.DateTime, "update_time", datExeTime };
                        dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

                        // 2016.5.10 By Jason
                        //sSQL = @"select serial_number from sajet.g_sn_status where rc_no = '" + sRC + "'";
                        //dsTemp = ClientUtils.ExecuteSQL(sSQL);

                        //if (dsTemp.Tables[0].Rows.Count == 0)
                        //{
                        //    string sMsg1 = SajetCommon.SetLanguage("go process was completed");
                        //    SajetCommon.Show_Message(sMsg1, 3);
                        //    return;
                        //}

                        //for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                        //{
                            //string sSN = dsTemp.Tables[0].Rows[i]["serial_number"].ToString();
                            //string sSQL1 = @"update sajet.g_sn_status
                            //                    set process_id = '" + sProcess_Id + @"'
                            //                        ,node_id = '" + sNode_Id + @"'
                            //                        ,next_node = '" + sNext_Node + @"'
                            //                        ,next_process = '" + sNext_Process + @"'
                            //                        ,sheet_name = '" + sSheet_Name + @"'
                            //                        ,update_userid = '" + g_sUserID + @"'
                            //                        ,update_time = sysdate 
                            //                  where serial_number = '" + sSN + "'";
                            //DataSet dsTemp1 = ClientUtils.ExecuteSQL(sSQL1);
                        //}
                        // 2016.5.10 End

                        string sMsg2 = SajetCommon.SetLanguage("go process was completed");
                        SajetCommon.Show_Message(sMsg2, 3);
                        ClearData();
                        txtRC_NO.Clear();
                        combGRN.Items.Clear();
                    }
                    else
                    {
                        string sMsg1 = SajetCommon.SetLanguage("go process is do not completed");
                        SajetCommon.Show_Message(sMsg1, 1);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    SajetCommon.Show_Message(ex.Message, 0);
                }
            }

            // 2016.5.10 By Jason

            //            //执行跳流程
            //            if (p == 0 && r == 1)
            //            {
            //                int n = combGRN.SelectedIndex;

            //                if (n == -1)
            //                {
            //                    string sMsg = SajetCommon.SetLanguage("route id is null");
            //                    SajetCommon.Show_Message(sMsg, 1);
            //                    return;
            //                }

            //                string sGP = txtGP.Text;

            //                if (sGP == "" || sGP == null)
            //                {
            //                    string sMsg = SajetCommon.SetLanguage("please select processName");
            //                    SajetCommon.Show_Message(sMsg, 1);
            //                    return;
            //                }

            //                string sRelease_Desc = Release_richTxt.Text.Trim();

            //                if (sRelease_Desc == "" || sRelease_Desc == null)
            //                {
            //                    string sMsg = SajetCommon.SetLanguage("Release DESC is null");
            //                    SajetCommon.Show_Message(sMsg, 1);
            //                    return;
            //                }

            //                try
            //                {
            //                    string sMsg = SajetCommon.SetLanguage("GO Route") + "?";

            //                    if (SajetCommon.Show_Message(sMsg, 2) == DialogResult.Yes)
            //                    {

            //                        if (sNext_Process == "END")
            //                        {
            //                            sNext_Process = "0";
            //                        }

            //                        sSQL = @"update sajet.g_rc_status 
            //                                    set process_id = '" + sProcess_Id + @"'
            //                                        ,route_id = '" + g_sRouteID + @"'
            //                                        ,node_id = '" + sNode_Id + @"'
            //                                        ,next_node = '" + sNext_Node + @"'
            //                                        ,next_process = '" + sNext_Process + @"'
            //                                        ,sheet_name = '" + sSheet_Name + @"'
            //                                        ,update_userid = '" + g_sUserID + @"'
            //                                        ,update_time = sysdate
            //                                  where rc_no = '" + sRC + "'";
            //                        dsTemp = ClientUtils.ExecuteSQL(sSQL);

            //                        //modify by hidy 2015/09/02
            //                        //delete table G_RC_PROCESS_GROUP中的残留资料
            //                        sSQL = @"DELETE FROM SAJET.G_RC_PROCESS_GROUP WHERE RC_NO = '" + sRC + "'";
            //                        dsTemp = ClientUtils.ExecuteSQL(sSQL);

            //                        //modify by hidy 2015/08/13
            //                        //记录跳流程log
            //                        sSQL = @"insert into sajet.g_rc_changeRoute_history
            //                                 (rc_no,current_route_id,to_route_id,current_process_id,to_process_id,changeroute_desc,update_userid,update_time,change_type)
            //                                 values
            //                                 (:rc_no,:current_route_id,:to_route_id,:current_process_id,:to_process_id,:changeroute_desc,:update_userid,sysdate,'ROUTE')";
            //                        object[][] Params = new object[7][];
            //                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "rc_no", sRC };
            //                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "current_route_id", sCRoute_Id };
            //                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "to_route_id", g_sRouteID };
            //                        Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "current_process_id", g_sProcessID };
            //                        Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "to_process_id", sProcess_Id };
            //                        Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "changeroute_desc", sRelease_Desc };
            //                        Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "update_userid", g_sUserID };
            //                        dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

            //                        sSQL = @"select serial_number from sajet.g_sn_status where rc_no = '" + sRC + "'";
            //                        dsTemp = ClientUtils.ExecuteSQL(sSQL);

            //                        if (dsTemp.Tables[0].Rows.Count <= 0)
            //                        {
            //                            string sMsg1 = SajetCommon.SetLanguage("go route was completed");
            //                            SajetCommon.Show_Message(sMsg1, 3);
            //                            return;
            //                        }

            //                        for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
            //                        {
            //                            string sSN = dsTemp.Tables[0].Rows[i]["serial_number"].ToString();
            //                            string sSQL1 = @"update sajet.g_sn_status
            //                                                set process_id = '" + sProcess_Id + @"'
            //                                                    ,route_id = '" + sRoute_Id + @"'
            //                                                    ,node_id = '" + sNode_Id + @"'
            //                                                    ,next_node = '" + sNext_Node + @"'
            //                                                    ,next_process = '" + sNext_Process + @"'
            //                                                    ,sheet_name = '" + sSheet_Name + @"'
            //                                                    ,update_userid = '" + g_sUserID + @"'
            //                                                    ,update_time = sysdate
            //                                              where serial_number = '" + sSN + "'";
            //                            DataSet dsTemp1 = ClientUtils.ExecuteSQL(sSQL1);
            //                        }
            //                        string sMsg2 = SajetCommon.SetLanguage("go route was completed");
            //                        SajetCommon.Show_Message(sMsg2, 3);
            //                        ClearData();
            //                        txtRC_NO.Clear();
            //                        combGRN.Items.Clear();
            //                    }
            //                    else
            //                    {
            //                        string sMsg1 = SajetCommon.SetLanguage("go route is do not completed");
            //                        SajetCommon.Show_Message(sMsg1, 1);
            //                        return;
            //                    }
            //                }
            //                catch (Exception ex)
            //                {
            //                    SajetCommon.Show_Message(ex.Message, 0);
            //                }
            //            }

            // 2016.5.10 End
        }

        //绘制途程图
        private XmlReader GetRouteXML()
        {
            XmlReader Xmlreader = null;
            sSQL = string.Format(@"SELECT A.ROUTE_ID,A.ROUTE_MAP,A.UPDATE_SEQ,B.ROUTE_NAME FROM SAJET.SYS_RC_ROUTE_MAP A,SAJET.SYS_RC_ROUTE B 
                                    WHERE A.ROUTE_ID=B.ROUTE_ID AND A.ROUTE_ID = {0}  ", sRoute_Id);
            try
            {
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                Xmldoc = new XmlDocument();
                Xmldoc.LoadXml(dsTemp.Tables[0].Rows[0]["ROUTE_MAP"].ToString());//從指定的字串載入XML文件                
                Xmlreader = XmlReader.Create(new System.IO.StringReader(Xmldoc.OuterXml));//輸入透過StringReader讀取Xmldoc中的Xmldoc字串輸出                                                           
                return Xmlreader;
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.ToString(), 0);
                Xmlreader = null;
                return Xmlreader;
            }
        }

        //选择制程,带出对应的process_id和process_name
        public void fMain_MouseDown(object sender, MouseEventArgs e)
        {
            // 2016.5.6 By Jason
            txtGP.Text = "";

            sNode_Id = "0";
            sNode_type = "0";
            sNext_Node = "0";
            sLink_Name = "0";

            sProcess_Id = "0";
            sNext_Node = "0";
            sNext_Process = "0";

            sSheet_Name = "0";
            // 2016.5.6 End

            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (addflow.ContextMenu != null)
                        addflow.ContextMenu.Dispose();

                    Item item = addflow.SelectedItem;

                    if (item == null)
                        return;
                    //item.Selected = false;
                    int index = item.ZOrder;
                    //Node node = (Node)item;
                    //int index = node.Index;

                    sSQL = string.Format(@"SELECT NODE_ID,NODE_TYPE,NODE_CONTENT,NEXT_NODE_ID,LINK_NAME FROM SAJET.SYS_RC_ROUTE_DETAIL WHERE ROUTE_ID={0} AND XML_INDEX={1}", sRoute_Id, index);
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);

                    if (dsTemp.Tables[0].Rows.Count <= 0)
                        return;

                    sNode_Id = dsTemp.Tables[0].Rows[0]["NODE_ID"].ToString();
                    sNode_type = dsTemp.Tables[0].Rows[0]["NODE_TYPE"].ToString();
                    sNext_Node = dsTemp.Tables[0].Rows[0]["NEXT_NODE_ID"].ToString();
                    sLink_Name = dsTemp.Tables[0].Rows[0]["LINK_NAME"].ToString();

                    if (sNode_type == "1" || sNode_type == "2" || sNode_type == "3") // GROUP
                    {
//                        string sCheckOnly = "N";

//                        // 跳站路徑只有一站,系統自動帶出
//                        if (sNode_type == "1")
//                        {
//                            sSQL = @"   SELECT A.NODE_CONTENT,B.PROCESS_NAME
//                                          FROM SAJET.SYS_RC_ROUTE_DETAIL A,SAJET.SYS_PROCESS B
//                                         WHERE A.NODE_CONTENT = B.PROCESS_ID
//                                           AND B.ENABLED = 'Y'
//                                           AND A.ROUTE_ID = " + sRoute_Id
//                                 + "       AND A.NODE_ID = " + sNode_Id
//                                 + @" GROUP BY A.NODE_CONTENT,B.PROCESS_NAME
//                                      ORDER BY A.NODE_CONTENT ASC";

//                            dsTemp = ClientUtils.ExecuteSQL(sSQL);

//                            if (dsTemp.Tables[0].Rows.Count == 1)
//                            {
//                                sProcess_Id = dsTemp.Tables[0].Rows[0]["NODE_CONTENT"].ToString();

//                                // (檢查是否有END製程 & GROUP製程)
//                                sSQL = @"   SELECT A.NODE_TYPE,A.NODE_ID,A.NODE_CONTENT,B.LINK_NAME
//                                              FROM SAJET.SYS_RC_ROUTE_DETAIL A,
//                                                   (SELECT NEXT_NODE_ID,LINK_NAME
//                                                      FROM SAJET.SYS_RC_ROUTE_DETAIL
//                                                     WHERE ROUTE_ID = " + sRoute_Id
//                                     + "               AND NODE_ID = " + sNode_Id
//                                     + @"          ) B
//                                             WHERE A.NODE_ID = B.NEXT_NODE_ID
//                                          GROUP BY A.NODE_TYPE,A.NODE_ID,A.NODE_CONTENT,B.LINK_NAME
//                                          ORDER BY A.NODE_CONTENT ASC";
//                                dsTemp = ClientUtils.ExecuteSQL(sSQL);

//                                if (dsTemp.Tables[0].Rows.Count == 1)
//                                {

//                                }

























//                            }
//                        }












                        TransferProcess f = new TransferProcess();

                        f.sNode_type = sNode_type;
                        f.sRoute_Id = sRoute_Id;
                        f.sNode_Id = sNode_Id;

                        if (f.ShowDialog() == DialogResult.OK)
                        {
                            sProcess_Id = f.sProcess_Id;
                            sNext_Node = f.sNext_Node;
                            sNext_Process = f.sNext_Process;
                        }
                    }
                    else if (sNode_type == "0" || sNode_type == "9") // START & END
                    {
                        //txtGP.Text = sProcess_Id;
                        item.Selected = false;
                        //txtGP.Text = "";

                        string sMsg = SajetCommon.SetLanguage("The process is not select");
                        SajetCommon.Show_Message(sMsg, 1);
                        return;
                    }

                    if (sNode_type == "1" || sNode_type == "2" || sNode_type == "3")
                    {
                        sSQL = @"select process_name from sajet.sys_process where process_id = '" + sProcess_Id + "'";
                        dsTemp = ClientUtils.ExecuteSQL(sSQL);

                        if (dsTemp.Tables[0].Rows.Count == 0)
                            return;

                        txtGP.Text = dsTemp.Tables[0].Rows[0]["process_name"].ToString();

                        sSQL = "select sheet_name from sajet.sys_rc_process_sheet where process_id = '" + sProcess_Id + "' and sheet_seq = '0'";
                        dsTemp = ClientUtils.ExecuteSQL(sSQL);

                        if (dsTemp.Tables[0].Rows.Count == 0)
                            return;

                        sSheet_Name = dsTemp.Tables[0].Rows[0]["sheet_name"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);
            }
        }

        public void setCombGRN()
        {
            //为route_name下拉框赋值
            combGRN.Items.Clear();

            // 2016.5.4 By Jason
            //            sSQL = @"select distinct b.route_name from sajet.sys_part_route a,sajet.sys_rc_route b 
            //                            where a.part_id = '" + sPartID + @"'
            //                            and a.route_id = b.route_id";
            sSQL = "SELECT ROUTE_NAME FROM SAJET.SYS_RC_ROUTE WHERE ROUTE_ID = " + sRoute_Id;
            // 2016.5.4 End

            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            if (dsTemp.Tables[0].Rows.Count <= 0)
                return;

            for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
            {
                combGRN.Items.Add(dsTemp.Tables[0].Rows[i]["route_name"].ToString());
            }
        }

        public void ClearData()
        {
            txtPART_NO.Text = "";
            txtSpec2.Text = "";
            txtVersion.Text = "";
            txtCR.Text = "";
            txtWO.Text = "";
            txtCP.Text = "";
            combGRN.Text = "";
            txtGP.Text = "";
            Release_richTxt.Text = "";
            addflow.Nodes.Clear();
            r = 0;
            p = 0;
            combGRN.Enabled = false;
            txtGP.ReadOnly = true;
            btnGP.Enabled = true;
            btnGR.Enabled = true;
            txtRC_NO.Focus();
        }
    }
}
