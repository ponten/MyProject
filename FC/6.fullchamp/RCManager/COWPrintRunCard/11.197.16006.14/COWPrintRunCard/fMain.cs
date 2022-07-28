using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.IO;
using System.Reflection;
namespace COWPrintRunCard
{
    public partial class fMain : Form
    {
        [System.Runtime.InteropServices.DllImport("User32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]

        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);
        string attached_type;

        int iPart_Id = 0;
        string sSQL;
        DataSet dsTemp;

        public fMain()
        {
            InitializeComponent();
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            fData f = new fData();

            f.WO = txtWO.Text.Trim();

            if (f.ShowDialog() == DialogResult.OK)
            {
                txtWO.Text = f.WO;
                Query();
            }
        }

        private void QueryRC()
        {
            try
            {
                dgv.Rows.Clear();

                // 2016.4.21 By Jason (重新定義)
                string cmd = @"SELECT A.WORK_ORDER,A.WO_TYPE,B.PART_NO,A.VERSION,A.TARGET_QTY,B.SPEC1,B.SPEC2,B.OPTION2,B.OPTION11,B.OPTION12,B.PART_ID,
                                      TO_CHAR(A.WO_SCHEDULE_DATE,'YYYY/MM/DD') AS WO_SCHEDULE_DATE,TO_CHAR(A.WO_DUE_DATE,'YYYY/MM/DD') AS WO_DUE_DATE
                                 FROM SAJET.G_WO_BASE A,
                                      SAJET.SYS_PART B,
                                      SAJET.G_RC_STATUS C
                                WHERE A.PART_ID = B.PART_ID
                                  AND C.WORK_ORDER = A.WORK_ORDER
                                  AND C.RC_NO ='" + txtRC.Text.Trim() + "'";

                ////                string cmd = @"select wb.work_order,wb.wo_type,wb.target_qty,wb.wo_create_date,wb.master_wo ,p.part_no,p.version
                ////                        from sajet.g_wo_base wb,sajet.sys_part p where wb.part_id=p.part_id and  wb.work_order ='" + txtWO.Text.Trim() + "'  order by wb.work_order ";
                //                string cmd = @" select wb.work_order,wb.wo_type,wb.target_qty,wb.wo_create_date,wb.master_wo ,p.part_no,p.version,wb.wo_option8
                //                                from sajet.g_wo_base wb,sajet.sys_part p ,sajet.g_rc_status rt
                //                                where wb.part_id=p.part_id
                //                                and  rt.work_order=wb.work_order and  rt.rc_no ='" + txtRC.Text.Trim() + "'";
                //                cmd = @"select wb.work_order,
                //                               wb.wo_type,
                //                               wb.target_qty,
                //                               wb.wo_create_date,
                //                               wb.master_wo,
                //                               p.part_no,
                //                               p.version,
                //                               sp.pd_type||sp.machine_type||sp.pd_version||sp.attached_type||sp.pd_status||sp.pd_size||sp.chip_type||sp.lithography_times||sp.dbr_type||sp.ito_thickness||sp.cow_level||sp.sort_level||sp.reserve wo_option8
                //                          from sajet.g_wo_base wb, sajet.sys_part p, sajet.g_rc_status rt,sajet.g_sn_status st,sajet.g_sn_property sp
                //                         where wb.part_id = p.part_id
                //                           and rt.work_order = wb.work_order
                //                           and rt.rc_no = '" + txtRC.Text.Trim() + "' and rt.rc_no=st.rc_no and st.serial_number=sp.chip_sn and rownum=1";

                // 2016.4.21 By Jason

                DataTable dt = ClientUtils.ExecuteSQL(cmd).Tables[0];

                if (dt.Rows.Count == 0)
                {
                    // 2016.4.21 By Jason
                    Clear();
                    // 2016.4.21 End

                    SajetCommon.Show_Message("RC Error", 0);
                    return;
                }

                iPart_Id = Convert.ToInt32(dt.Rows[0]["PART_ID"].ToString());
                txtWO.Text = dt.Rows[0]["WORK_ORDER"].ToString();
                lblWOType.Text = dt.Rows[0]["WO_TYPE"].ToString();
                lblPartNo.Text = dt.Rows[0]["SPEC1"].ToString();
                lblSpec2.Text = dt.Rows[0]["SPEC2"].ToString();
                lblModel.Text = dt.Rows[0]["VERSION"].ToString();
                lblQty.Text = dt.Rows[0]["TARGET_QTY"].ToString();

                // 2016.4.21 By Jason
                //lblWoTemplate.Text = "sample_workorder.xlt";
                //lblWoTemplate.Text = "WO_Std.xlt";
                lblProductGrade.Text = dt.Rows[0]["OPTION11"].ToString();
                // 2016.4.21 End

                //lblRunCardTemplate.Text = dt.Rows[0]["version"].ToString() + ".xlt";

                // 2016.4.21 By Jason
                //lblSpecialPN.Text = dt.Rows[0]["WO_OPTION8"].ToString();

                //lblRunCardTemplate.Text = dt.Rows[0]["VERSION"].ToString() + "-" + this.lblSpecialPN.Text.Substring(5, 1) + ".xlt";//Modify by Jieke 2015/12/09 for 修改模版
                //attached_type = this.lblSpecialPN.Text.Substring(5, 1);
                // 2016.4.21 End

                // 2016.4.21 By Jason
                //lblRunCardTemplate.Text = "RC_Std.xlt";
                lblRunCardTemplate.Text = dt.Rows[0]["OPTION2"].ToString();
                lblWOStartTime.Text = dt.Rows[0]["WO_SCHEDULE_DATE"].ToString();
                lblWOEndTime.Text = dt.Rows[0]["WO_DUE_DATE"].ToString();
                lblOption12.Text = dt.Rows[0]["OPTION12"].ToString();

                //lblSpecialPN.Text = dt.Rows[0]["WO_OPTION8"].ToString();
                //if (dt.Rows[0]["WO_OPTION8"].ToString() == "")
                //{
                //    lblRunCardTemplate.Text = dt.Rows[0]["VERSION"].ToString() + "-" + dt.Rows[0]["VERSION"].ToString() + ".xlt";//Modify by Jieke 2015/12/10 for 修改模版
                //    attached_type = dt.Rows[0]["VERSION"].ToString();
                //}
                //else
                //{

                //    lblRunCardTemplate.Text = dt.Rows[0]["VERSION"].ToString() + "-" + this.lblSpecialPN.Text.Substring(5, 1) + ".xlt";//Modify by Jieke 2015/12/09 for 修改模版
                //    attached_type = this.lblSpecialPN.Text.Substring(5, 1);
                //}
                // 2016.4.21 End


                // 2016.4.21 By Jason (重新定義)
                cmd = @"SELECT A.RC_NO,A.SHEET_NAME,A.CURRENT_QTY,
                               SAJET.SJ_RC_STATUS(A.CURRENT_STATUS) RC_STATUS,
                               SAJET.SJ_WOSTATUS_RESULT(B.WO_STATUS) WO_STATUS
                          FROM SAJET.G_RC_STATUS A,
                               SAJET.G_WO_BASE B
                         WHERE A.WORK_ORDER = B.WORK_ORDER
                           AND A.RC_NO ='" + txtRC.Text.Trim() + "' ORDER BY A.RC_NO ASC";

                //                //                cmd = @"select rs.rc_no,rs.sheet_name,rs.current_qty
                //                //                     from sajet.g_wo_base wb,sajet.g_rc_status rs
                //                //                    where wb.work_order=rs.work_order
                //                //                    and wb.work_order='"+txtWO.Text.Trim()+"' order by rs.rc_no";

                ////                cmd = @" select rs.rc_no, rs.sheet_name, rs.current_qty,
                ////                                decode (a.label_content,null,'未打印'，'已打印') rc_status,decode (b.label_content, null,'未打印'，'已打印') wo_status
                ////                                  from sajet.g_wo_base   wb,
                ////                                       sajet.g_rc_status rs,
                ////                                       （select  distinct ptl.label_content
                ////                                  from sajet.g_print_log ptl
                ////                                 where ptl.label_type = '流程单'
                ////      
                ////                                   and ptl.label_content in
                ////                                       (select rs.rc_no
                ////                                          from sajet.g_wo_base wb, sajet.g_rc_status rs
                ////                                         where wb.work_order = rs.work_order
                ////                                           and wb.work_order = '" + txtWO.Text.Trim() + @"') ） a,
                ////                                 (select distinct ptl.label_content
                ////                                          from sajet.g_print_log ptl
                ////                                         where ptl.label_type = '派工信息单'
                ////              
                ////                                           and ptl.label_content in
                ////                                               (select rs.rc_no
                ////                                                  from sajet.g_wo_base wb, sajet.g_rc_status rs
                ////                                                 where wb.work_order = rs.work_order
                ////                                                   and wb.work_order = '" + txtWO.Text.Trim() + @"')) b
                ////
                ////                                 where wb.work_order = rs.work_order
                ////                                   and wb.work_order = '" + txtWO.Text.Trim() + @"'
                ////                                   and rs.rc_no = a.label_content(+)
                ////                                   and rs.rc_no = b.label_content(+)
                ////                                  order by rc_status desc，wo_status desc
                ////                                ";
                //                cmd = @"  select rs.rc_no, rs.sheet_name, rs.current_qty,
                //                                decode (a.label_content,null,'未打印'，'已打印') rc_status,decode (b.label_content, null,'未打印'，'已打印') wo_status
                //                                  from sajet.g_wo_base   wb,
                //                                       sajet.g_rc_status rs,
                //                                       （select  distinct ptl.label_content
                //                                  from sajet.g_print_log ptl
                //                                 where ptl.label_type = '流程单'
                //      
                //                                   and ptl.label_content = '" + txtRC.Text.Trim() + @"' ） a,
                //                                 (select distinct ptl.label_content
                //                                          from sajet.g_print_log ptl
                //                                         where ptl.label_type = '派工信息单'
                //              
                //                                           and ptl.label_content ='" + txtRC.Text.Trim() + @"') b
                //
                //                                 where wb.work_order = rs.work_order
                //                                   and rs.rc_no='" + txtRC.Text.Trim() + @"'
                //                                   and rs.rc_no = a.label_content(+)
                //                                   and rs.rc_no = b.label_content(+)
                //                                  order by rc_status desc，wo_status desc ";

                // 2016.4.21 End

                dt = ClientUtils.ExecuteSQL(cmd).Tables[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dgv.Rows.Insert(dgv.Rows.Count, new object[] { true, dt.Rows[i]["rc_no"].ToString(), dt.Rows[i]["sheet_name"].ToString(),
                        dt.Rows[i]["current_qty"].ToString(),dt.Rows[i]["rc_status"].ToString(),dt.Rows[i]["wo_status"].ToString() });

                    // 2016.4.21 By Jason
                    //if (dt.Rows[i]["rc_status"].ToString() != dt.Rows[i]["wo_status"].ToString())
                    //{
                    //    dgv.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    //}
                    //else if (dt.Rows[i]["rc_status"].ToString() == "已打印")
                    //{
                    //    dgv.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    //}
                    // 2016.4.21 End
                }

                this.GetQty();
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);
            }
        }

        void Clear()
        {
            // 2016.4.21 By Jason
            txtTitle.Text = "";

            txtWO.Text = "";
            txtRC.Text = "";

            iPart_Id = 0;
            lblWOType.Text = "N/A";
            lblPartNo.Text = "N/A";
            lblSpec2.Text = "N/A";
            lblModel.Text = "N/A";
            lblQty.Text = "N/A";
            lblProductGrade.Text = "N/A";
            lblRunCardTemplate.Text = "N/A";
            lblSpecialPN.Text = "N/A";
            lblWOStartTime.Text = "N/A";
            lblWOEndTime.Text = "N/A";
            lblOption12.Text = "N/A";

            this.groupBox4.Text = "流程卡總數量：0";

            dgv.Rows.Clear();
            // 2016.4.21 End
        }

        void Query()
        {
            try
            {
                dgv.Rows.Clear();

                // 2016.4.21 By Jason (重新定義)
                string cmd = @"SELECT A.WORK_ORDER,A.WO_TYPE,B.PART_NO,A.VERSION,A.TARGET_QTY,B.SPEC1,B.SPEC2,B.OPTION2,B.OPTION11,B.OPTION12,B.PART_ID,
                                      TO_CHAR(A.WO_SCHEDULE_DATE,'YYYY/MM/DD') AS WO_SCHEDULE_DATE,TO_CHAR(A.WO_DUE_DATE,'YYYY/MM/DD') AS WO_DUE_DATE
                                 FROM SAJET.G_WO_BASE A,
                                      SAJET.SYS_PART B
                                WHERE A.PART_ID = B.PART_ID
                                  AND A.WORK_ORDER ='" + txtWO.Text.Trim() + "'";

                //                string cmd = @"select wb.work_order,wb.wo_type,wb.target_qty,wb.wo_create_date,wb.master_wo ,p.part_no,p.version,wb.wo_option8
                //                        from sajet.g_wo_base wb,sajet.sys_part p where wb.part_id=p.part_id and  wb.work_order ='" + txtWO.Text.Trim() + "'  order by wb.work_order ";
                //                //if (!string.IsNullOrEmpty(txtWO.Text.Trim()))
                //                //{ 
                //                //}

                //                cmd = @"select wb.work_order,
                //                               wb.wo_type,
                //                               wb.target_qty,
                //                               wb.wo_create_date,
                //                               wb.master_wo,
                //                               p.part_no,
                //                               p.version,
                //                               wb.wo_option8,pr.attached_type
                //                          from sajet.g_wo_base wb, sajet.sys_part p,sajet.g_wo_special_pn_rule pr
                //                         where wb.part_id = p.part_id
                //                           and wb.work_order = '" + txtWO.Text.Trim() + @"' and wb.work_order=pr.work_order(+)
                //                         order by wb.work_order";

                // 2016.4.21 End

                DataTable dt = ClientUtils.ExecuteSQL(cmd).Tables[0];

                if (dt.Rows.Count == 0)
                {
                    // 2016.4.21 By Jason
                    Clear();
                    // 2016.4.21 End

                    SajetCommon.Show_Message("WO Error", 0);
                    return;
                }

                iPart_Id = Convert.ToInt32(dt.Rows[0]["PART_ID"].ToString());
                lblWOType.Text = dt.Rows[0]["WO_TYPE"].ToString();
                lblPartNo.Text = dt.Rows[0]["SPEC1"].ToString();
                lblSpec2.Text = dt.Rows[0]["SPEC2"].ToString();
                lblModel.Text = dt.Rows[0]["VERSION"].ToString();
                lblQty.Text = dt.Rows[0]["TARGET_QTY"].ToString();

                // 2016.4.21 By Jason
                //lblWoTemplate.Text = "sample_workorder.xlt";
                //lblWoTemplate.Text = "WO_Std.xlt";
                lblProductGrade.Text = dt.Rows[0]["OPTION11"].ToString();
                // 2016.4.21 End

                //lblRunCardTemplate.Text = dt.Rows[0]["version"].ToString() + ".xlt";

                // 2016.4.21 By Jason
                //lblRunCardTemplate.Text = "RC_Std.xlt";
                lblRunCardTemplate.Text = dt.Rows[0]["OPTION2"].ToString();
                lblWOStartTime.Text = dt.Rows[0]["WO_SCHEDULE_DATE"].ToString();
                lblWOEndTime.Text = dt.Rows[0]["WO_DUE_DATE"].ToString();
                lblOption12.Text = dt.Rows[0]["OPTION12"].ToString();

                //lblSpecialPN.Text = dt.Rows[0]["WO_OPTION8"].ToString();
                //if (dt.Rows[0]["WO_OPTION8"].ToString() == "")
                //{
                //    lblRunCardTemplate.Text = dt.Rows[0]["VERSION"].ToString() + "-" + dt.Rows[0]["VERSION"].ToString() + ".xlt";//Modify by Jieke 2015/12/10 for 修改模版
                //    attached_type = dt.Rows[0]["VERSION"].ToString();
                //}
                //else
                //{

                //    lblRunCardTemplate.Text = dt.Rows[0]["VERSION"].ToString() + "-" + this.lblSpecialPN.Text.Substring(5, 1) + ".xlt";//Modify by Jieke 2015/12/09 for 修改模版
                //    attached_type = this.lblSpecialPN.Text.Substring(5, 1);
                //}
                // 2016.4.21 End

                // 2016.4.21 By Jason (重新定義)
                cmd = @"SELECT A.RC_NO,A.SHEET_NAME,A.CURRENT_QTY,
                               SAJET.SJ_RC_STATUS(A.CURRENT_STATUS) RC_STATUS,
                               SAJET.SJ_WOSTATUS_RESULT(B.WO_STATUS) WO_STATUS
                          FROM SAJET.G_RC_STATUS A,
                               SAJET.G_WO_BASE B
                         WHERE A.WORK_ORDER = B.WORK_ORDER
                           AND A.WORK_ORDER ='" + txtWO.Text.Trim() + "' ORDER BY A.RC_NO ASC";

                //                //                cmd = @"select rs.rc_no,rs.sheet_name,rs.current_qty
                //                //                     from sajet.g_wo_base wb,sajet.g_rc_status rs
                //                //                    where wb.work_order=rs.work_order
                //                //                    and wb.work_order='"+txtWO.Text.Trim()+"' order by rs.rc_no";

                //                cmd = @" select rs.rc_no, rs.sheet_name, rs.current_qty,
                //                                decode (a.label_content,null,'未打印'，'已打印') rc_status,decode (b.label_content, null,'未打印'，'已打印') wo_status
                //                                  from sajet.g_wo_base   wb,
                //                                       sajet.g_rc_status rs,
                //                                       （select  distinct ptl.label_content
                //                                  from sajet.g_print_log ptl
                //                                 where ptl.label_type = '流程单'
                //      
                //                                   and ptl.label_content in
                //                                       (select rs.rc_no
                //                                          from sajet.g_wo_base wb, sajet.g_rc_status rs
                //                                         where wb.work_order = rs.work_order
                //                                           and wb.work_order = '" + txtWO.Text.Trim() + @"' and  rs.current_status in(0,1,2)) ） a,
                //                                 (select distinct ptl.label_content
                //                                          from sajet.g_print_log ptl
                //                                         where ptl.label_type = '派工信息单'
                //              
                //                                           and ptl.label_content in
                //                                               (select rs.rc_no
                //                                                  from sajet.g_wo_base wb, sajet.g_rc_status rs
                //                                                 where wb.work_order = rs.work_order
                //                                                   and wb.work_order = '" + txtWO.Text.Trim() + @"'and  rs.current_status in(0,1,2))) b
                //
                //                                 where wb.work_order = rs.work_order
                //                                   and wb.work_order = '" +txtWO.Text.Trim()+ @"'
                //                                   and rs.rc_no = a.label_content(+)
                //                                   and rs.rc_no = b.label_content(+)
                //                                  order by rc_status desc，wo_status desc
                //                                ";

                // 2016.4.21 End

                dt = ClientUtils.ExecuteSQL(cmd).Tables[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dgv.Rows.Insert(dgv.Rows.Count, new object[] { true, dt.Rows[i]["rc_no"].ToString(), dt.Rows[i]["sheet_name"].ToString(),
                        dt.Rows[i]["current_qty"].ToString(),dt.Rows[i]["rc_status"].ToString(),dt.Rows[i]["wo_status"].ToString() });

                    // 2016.4.21 By Jason
                    //if (dt.Rows[i]["rc_status"].ToString() != dt.Rows[i]["wo_status"].ToString())
                    //{ 
                    //    dgv.Rows[i].DefaultCellStyle.BackColor = Color.Yellow; 
                    //}
                    //else if (dt.Rows[i]["rc_status"].ToString() == "已打印")
                    //{
                    //    dgv.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    //}
                    // 2016.4.21 End
                }

                this.GetQty();
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);
            }
        }

        private void txtWO_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter)
                return;

            if (string.IsNullOrEmpty(txtWO.Text.Trim()))
            {
                Clear();
                dgv.Rows.Clear();
                SajetCommon.Show_Message("WO Is Empty", 0);
                return;
            }

            // 2016.4.21 By Jason
            txtRC.Text = "";
            // 2016.4.21 End

            Query();
        }

        private void btnPrintRC_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtWO.Text.Trim()))
            {
                SajetCommon.Show_Message("WO Is Empty", 0);
                return;
            }

            if (dgv.Rows.Count == 0)
            {
                SajetCommon.Show_Message("No Data", 0);
                return;
            }

            string filePath = Assembly.GetExecutingAssembly().Location;
            filePath = filePath.Substring(0, filePath.LastIndexOf('\\'));
            // 2016.4.21 By Jason
            //filePath += "\\sample\\" + lblRunCardTemplate.Text.Substring(0, lblRunCardTemplate.Text.Length - 4) + "-" + txtWO.Text.Trim().Substring(2, 1)
            //         + ".xlt";
            filePath += "\\Sample\\" + lblRunCardTemplate.Text;
            // 2016.4.21 End

            if (!File.Exists(filePath))
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("File Not Exist:") + filePath, 0);
                return;
            }

            Excel.Application xlsApp = new Excel.Application();
            //xlsApp.Visible = true; 
            Excel.Workbooks xlsWbs = xlsApp.Workbooks;
            Excel.Workbook xlsWb = xlsWbs.Open(
                                         filePath, Missing.Value, false, Missing.Value, Missing.Value,
                                         Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                                         Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                                         Missing.Value, Missing.Value);
            Excel.Worksheet xlsWs;
            xlsWs = (Excel.Worksheet)xlsWb.Worksheets[1];

            Excel.Worksheet xlsWs2;
            xlsWs2 = (Excel.Worksheet)xlsWb.Worksheets[2];

            try
            {
                // xlsWs.Cells[4, 6]
                //xlsWs.Cells[4, 6].Value = lblModel.Text + "-" + attached_type;//Model
                //xlsWs.Cells[4, 12].Value = txtWO.Text.Trim();//WO
                //xlsWs.Cells[3, 2].Value = lblModel.Text + "-" + attached_type;//Model
                //xlsWs.Cells[4, 3].Value = txtWO.Text.Trim();//WO

                // 2016.5.16 By Jason
                int iRow = 0, iColumn = 0;

                string sSQL = @"    SELECT B.PROCESS_NAME,A.ROW_ITEM,A.COLUMN_ITEM,A.ITEM_NAME,A.VALUE_DEFAULT
                                      FROM SAJET.SYS_RC_PROCESS_PARAM_PART A,SAJET.SYS_PROCESS B
                                     WHERE A.PROCESS_ID = B.PROCESS_ID
                                       AND A.PART_ID =" + iPart_Id
                            + @"       AND A.PRINT = 'Y'
                                       AND A.ROW_ITEM IS NOT NULL
                                       AND A.COLUMN_ITEM IS NOT NULL
                                  ORDER BY ROW_ITEM,COLUMN_ITEM";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);

                for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                {
                    iRow = Convert.ToInt32(dsTemp.Tables[0].Rows[i]["ROW_ITEM"].ToString());
                    iColumn = Convert.ToInt32(dsTemp.Tables[0].Rows[i]["COLUMN_ITEM"].ToString());

                    xlsWs.Cells[iRow, iColumn].Value = dsTemp.Tables[0].Rows[i]["VALUE_DEFAULT"].ToString();
                }
                // 2016.5.16 End

                // 2016.8.11 By Jason
                xlsWs2.Cells[1, 2].Value = txtWO.Text.Trim();
                xlsWs2.Cells[2, 2].Value = lblPartNo.Text;
                xlsWs2.Cells[3, 2].Value = lblQty.Text;
                xlsWs2.Cells[8, 2].Value = lblWOStartTime.Text;
                xlsWs2.Cells[9, 2].Value = lblWOEndTime.Text;
                xlsWs2.Cells[11, 2].Value = lblSpec2.Text;
                xlsWs2.Cells[12, 2].Value = lblOption12.Text;

                // 2016.12.5 By Jason
                if (lblProductGrade.Text == "A")
                {
                    xlsWs2.Cells[4, 2].Value = lblProductGrade.Text;
                    xlsWs2.Cells[5, 2].Value = SajetCommon.SetLanguage("Tightened Inspection");
                }
                else
                {
                    xlsWs2.Cells[4, 2].Value = " ";
                    xlsWs2.Cells[5, 2].Value = " ";
                }
                // 2016.12.5 End
                // 2016.8.11 End

                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    if ((bool)dgv.Rows[i].Cells["checked"].Value == true)
                    {
                        dgv.Rows[i].Cells["RCStatus"].Value = SajetCommon.SetLanguage("Print...");
                        string RC = dgv.Rows[i].Cells["RC_NO"].Value.ToString();
                        string RC_Qty = dgv.Rows[i].Cells["CURRENT_QTY"].Value.ToString();

                        // 2016.8.11 By Jason
                        //xlsWs.Cells[4, 16].Value = RC;//RC
                        xlsWs2.Cells[6, 2].Value = RC;
                        xlsWs2.Cells[7, 2].Value = RC_Qty;
                        xlsWs2.Cells[10, 2].Value = '*' + RC + '*';
                        // 2016.8.11 End

                        //使excel不可见
                        xlsApp.Visible = false;
                        //打印
                        xlsWs.PrintOut(Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                        //保存后退出，并释放资源
                        xlsApp.DisplayAlerts = false;

                        // 2016.4.21 By Jason
                        //string cmd = @"insert into sajet.G_PRINT_LOG(LABEL_TYPE,PRINT_FUNCTION,LABEL_CONTENT,PRINT_USERID)values('流程单','COWPrintRunCard','" + RC + "'," + ClientUtils.UserPara1 + ")";
                        //ClientUtils.ExecuteSQL(cmd);
                        //dgv.Rows[i].Cells["RCStatus"].Value = SajetCommon.SetLanguage("Print OK");
                        // 2016.4.21 End
                    }
                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);
            }
            finally
            {
                Clear();

                xlsWs = null;
                xlsWs2 = null;
                xlsWb = null;
                xlsApp.Quit();
                xlsApp = null;
            }
        }

        private void btnPrintWO_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtWO.Text.Trim()))
            {
                SajetCommon.Show_Message("WO Is Empty", 0);
                return;
            }

            if (dgv.Rows.Count == 0)
            {
                SajetCommon.Show_Message("No Data", 0);
                return;
            }

            if (string.IsNullOrEmpty(txtTitle.Text.Trim()))
            {
                SajetCommon.Show_Message("Title Is Empty", 0);
                return;
            }

            string filePath = Assembly.GetExecutingAssembly().Location;
            filePath = filePath.Substring(0, filePath.LastIndexOf('\\'));
            filePath += "\\Sample\\" + lblProductGrade.Text;

            if (!File.Exists(filePath))
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("File Not Exist:") + filePath, 0);
                return;
            }

            Excel.Application xlsApp1 = new Excel.Application();
            //xlsApp.Visible = true; 
            Excel.Workbooks xlsWbs = xlsApp1.Workbooks;
            Excel.Workbook xlsWb = xlsWbs.Open(
                                         filePath, Missing.Value, false, Missing.Value, Missing.Value,
                                         Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                                         Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                                         Missing.Value, Missing.Value);
            Excel.Worksheet xlsWs;
            xlsWs = (Excel.Worksheet)xlsWb.Worksheets[1];

            try
            {
                //xlsWs.Cells[1, 1].Value = txtTitle.Text;//Title
                //xlsWs.Cells[1, 1].Font.color = this.sampleBtn.ForeColor;
                //xlsWs.Cells[1, 1].Interior.color = this.sampleBtn.BackColor;
                //// 2016.4.21 By Jason
                ////xlsWs.Cells[2, 3].Value = this.lblSpecialPN.Text.Substring(2, 3) + "-" + attached_type;//lblModel.Text;//Model（Part Version）Modify by Jieke 2015/12/9
                //// 2016.4.21 End

                //xlsWs.Cells[2, 9].Value = txtWO.Text.Trim();//制造单号
                //xlsWs.Cells[3, 9].Value = lblQty.Text;//工单总数
                //xlsWs.Cells[3, 13].Value = dtpDispatchDate.Text;//派工日期
                //xlsWs.Cells[3, 17].Value = dtpInvDate.Text;//预定入库日期

                // 2016.5.16 By Jason
                int iRow = 0, iColumn = 0;

                string sSQL = @"    SELECT B.PROCESS_NAME,A.ROW_ITEM,A.COLUMN_ITEM,A.ITEM_NAME,A.VALUE_DEFAULT
                                      FROM SAJET.SYS_RC_PROCESS_PARAM_PART A,SAJET.SYS_PROCESS B
                                     WHERE A.PROCESS_ID = B.PROCESS_ID
                                       AND A.PART_ID =" + iPart_Id
                            + @"       AND A.PRINT = 'Y'
                                       AND A.ROW_ITEM IS NOT NULL
                                       AND A.COLUMN_ITEM IS NOT NULL
                                  ORDER BY ROW_ITEM,COLUMN_ITEM";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);

                for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                {
                    iRow = Convert.ToInt32(dsTemp.Tables[0].Rows[i]["ROW_ITEM"].ToString());
                    iColumn = Convert.ToInt32(dsTemp.Tables[0].Rows[i]["COLUMN_ITEM"].ToString());

                    xlsWs.Cells[iRow, iColumn].Value = dsTemp.Tables[0].Rows[i]["ITEM_NAME"].ToString();
                }
                // 2016.5.16 End


                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    if ((bool)dgv.Rows[i].Cells["checked"].Value == true)
                    {
                        dgv.Rows[i].Cells["WOStatus"].Value = SajetCommon.SetLanguage("Print...");

                        string RC = dgv.Rows[i].Cells["rc_no"].Value.ToString();//RC(盒号)

                        // 2016.4.21 By Jason
                        //string cmd = "select t.seq,t.serial_number from sajet.g_sn_status t where t.rc_no='" + RC + "' order by t.seq";
                        //DataTable dt = ClientUtils.ExecuteSQL(cmd).Tables[0];
                        // 2016.4.21 End

                        xlsWs.Cells[5, 3].Value = RC;//RC

                        // 2016.4.21 By Jason
                        //xlsWs.Cells[5, 17].Value = dt.Rows.Count;//RC总量
                        // 2016.4.21 End

                        if (!string.IsNullOrEmpty(this.lblSpecialPN.Text))
                        {
                            /**Begin Modify by Jieke 2015/12/9 for 修改批次信息*/
                            if (this.lblSpecialPN.Text.Substring(0, 1) == "P")
                            {
                                xlsWs.Cells[4, 3].Value = "生产批";
                            }

                            if (this.lblSpecialPN.Text.Substring(0, 1) == "Y")
                            {
                                xlsWs.Cells[4, 3].Value = "验证批";
                            }

                            if (this.lblSpecialPN.Text.Substring(0, 1) == "G")
                            {
                                xlsWs.Cells[4, 3].Value = "工程批";
                            }

                            if (this.lblSpecialPN.Text.Substring(0, 1) == "T")
                            {
                                xlsWs.Cells[4, 3].Value = "特采批";
                            }

                            if (this.lblSpecialPN.Text.Substring(0, 1) == "R")
                            {
                                xlsWs.Cells[4, 3].Value = "验证批";
                            }
                            //if (this.lblSpecialPN.Text.Substring(0, 1) == "F")
                            //{
                            //    xlsWs.Cells[4, 3].Value = "外延复机批";
                            //}
                            /**End Modify by Jieke 2015/12/9 for 修改批次信息*/
                        }

                        //清空上次打印数据
                        for (int j = 1; j <= 25; j++)
                        {
                            //if (j <= 13)
                            //{
                            xlsWs.Cells[6 + j, 7].Value = "";
                            //}
                            //else
                            //{
                            //    xlsWs.Cells[6 + j - 13, 17].Value = "";
                            //}
                        }

                        // 2016.4.21 By Jason
                        //for (int j = 1; j <= dt.Rows.Count ; j++)
                        //{
                        //    //if (dt.Rows[j - 1]["seq"].ToString() == "")
                        //    //{
                        //    //    if (j <= 13)
                        //    //    {
                        //    //        xlsWs.Cells[6 + j, 7].Value = dt.Rows[j - 1]["serial_number"].ToString();
                        //    //    }
                        //    //    else
                        //    //    {
                        //    //        xlsWs.Cells[6 + j - 13, 17].Value = dt.Rows[j - 1]["serial_number"].ToString();
                        //    //    }
                        //    //}
                        //    //else
                        //    //{
                        //    if (dt.Rows[j - 1]["seq"].ToString() != "")
                        //    {
                        //        int seq = Convert.ToInt32(dt.Rows[j - 1]["seq"]);
                        //        //if (seq <= 13)
                        //        //{
                        //            xlsWs.Cells[6 + seq, 7].Value = dt.Rows[j - 1]["serial_number"].ToString();
                        //        //}
                        //        //else
                        //        //{
                        //        //    xlsWs.Cells[6 + seq - 13, 17].Value = dt.Rows[j - 1]["serial_number"].ToString();
                        //        //}
                        //    }

                        //    //}

                        //}
                        // 2016.4.21 End

                        //使excel不可见
                        xlsApp1.Visible = false;
                        //打印
                        xlsWs.PrintOut(Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                        xlsApp1.DisplayAlerts = false;

                        // 2016.4.21 By Jason
                        //cmd = @"insert into sajet.G_PRINT_LOG(LABEL_TYPE,PRINT_FUNCTION,LABEL_CONTENT,PRINT_USERID)values('派工信息单','COWPrintRunCard','" + RC + "'," + ClientUtils.UserPara1 + ")";
                        //ClientUtils.ExecuteSQL(cmd);
                        //dgv.Rows[i].Cells["WOStatus"].Value = SajetCommon.SetLanguage("Print OK");
                        // 2016.4.21 End
                    }
                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);
            }
            finally
            {
                Clear();

                xlsWs = null;
                xlsWb = null;
                xlsWbs = null;
                xlsApp1.Quit();
                //xlsApp1 = null;
                KillExcel(xlsApp1);
            }
        }
        /// <summary>
        /// 获取选中的sn数量
        /// </summary>
        private void GetQty()
        {
            decimal i = 0;

            foreach (DataGridViewRow dgr in dgv.Rows)
            {
                if (bool.Parse(dgr.Cells[0].Value.ToString()) == true)
                    i += Convert.ToDecimal(dgr.Cells[3].Value);
            }
            //this.groupBox4.Text = " 选中的芯片数：" + i;
            this.groupBox4.Text = "流程卡總數量：" + i;
        }

        public static void KillExcel(Excel.Application excel)
        {
            IntPtr t = new IntPtr(excel.Hwnd);

            int k = 0;

            GetWindowThreadProcessId(t, out k);
            System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(k);
            p.Kill();
        }

        /// <summary>
        /// 全选事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectAllToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.dgv.CommitEdit(DataGridViewDataErrorContexts.Commit);

            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.Cells["Checked"].Value = true;
            }

            dgv.CurrentCell = null;

            this.GetQty();
        }

        /// <summary>
        /// 选择流程单和派工信息单都没有打印的RC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NonePrinttoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            dgv.CurrentCell = null;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if ((row.Cells["RCStatus"].Value.ToString() == "未打印") && (row.Cells["WOStatus"].Value.ToString() == "未打印"))
                {
                    row.Cells["Checked"].Value = true;
                }
                else
                {
                    row.Cells["Checked"].Value = false;
                }
            }

            this.GetQty();
        }

        /// <summary>
        /// 选择流程单未打印但是派工单已经打印的
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RCNonePrinttoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            dgv.CurrentCell = null;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if ((row.Cells["RCStatus"].Value.ToString() == "未打印") && (row.Cells["WOStatus"].Value.ToString() == "已打印"))
                {
                    row.Cells["Checked"].Value = true;
                }
                else
                {
                    row.Cells["Checked"].Value = false;
                }
            }
            this.GetQty();
        }

        /// <summary>
        /// 选择流程单已打印，派工单未打印的
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WONoneprinttoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            dgv.CurrentCell = null;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if ((row.Cells["RCStatus"].Value.ToString() == "已打印") && (row.Cells["WOStatus"].Value.ToString() == "未打印"))
                {
                    row.Cells["Checked"].Value = true;
                }
                else
                {
                    row.Cells["Checked"].Value = false;
                }
            }
            this.GetQty();
        }

        private void blackradioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (blackradioButton.Checked == true)
                this.sampleBtn.ForeColor = Color.Black;
        }

        private void redradioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (redradioButton.Checked == true)
                this.sampleBtn.ForeColor = Color.Red;
        }

        private void blueradioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (blueradioButton.Checked == true)
                this.sampleBtn.ForeColor = Color.Blue;
        }

        private void noneradioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (noneradioButton.Checked == true)
                this.sampleBtn.BackColor = Color.Transparent;
        }

        private void yellowradioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (yellowradioButton.Checked == true)
                this.sampleBtn.BackColor = Color.Yellow;
        }

        private void pinkradioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (pinkradioButton.Checked == true)
                this.sampleBtn.BackColor = Color.Orange;
        }

        private void greenradioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (greenradioButton.Checked == true)
                this.sampleBtn.BackColor = Color.LightGreen;
        }

        private void red1radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (red1radioButton.Checked == true)
                this.sampleBtn.BackColor = Color.Purple;
        }

        private void blue1radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (blue1radioButton.Checked == true)
                this.sampleBtn.BackColor = Color.LightSkyBlue;
        }

        private void txtRC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter)
                return;

            if (string.IsNullOrEmpty(txtRC.Text.Trim()))
            {
                Clear();
                dgv.Rows.Clear();
                SajetCommon.Show_Message("RC Is Empty", 0);
                return;
            }

            // 2016.4.21 By Jason
            txtWO.Text = "";
            // 2016.4.21 End

            QueryRC();
        }
        /// <summary>
        /// 勾选触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (this.dgv.IsCurrentCellDirty) //有未提交的更改
            {
                this.dgv.CommitEdit(DataGridViewDataErrorContexts.Commit);
                this.GetQty();
            }
        }
    }
}
