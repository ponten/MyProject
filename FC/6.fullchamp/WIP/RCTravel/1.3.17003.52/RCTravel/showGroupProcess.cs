using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using SajetClass;
using System.Data.OracleClient;

/*2015.7.1～2015.7.8*/
namespace RCTravel
{
    public partial class showGroupProcess : Form
    {
        string sIniFile = Application.StartupPath + Path.DirectorySeparatorChar + "Sajet.Ini";
        private string sRC_NO;

        public showGroupProcess(string sRC)
        {
            InitializeComponent();
            SajetCommon.SetLanguageControl(this);
            sRC_NO = sRC;
            string sSQL = string.Empty;
            string upSQL = string.Empty;
            DataSet dsGrid;
            DataSet dsTemp;
            DataSet Group;

            // 2015/08/24, Aaron
            sSQL = "SELECT A.RC_NO,C.PROCESS_ID, C.PROCESS_NAME "
            + " FROM SAJET.G_RC_STATUS A, SAJET.SYS_RC_ROUTE_DETAIL B,SAJET.SYS_PROCESS C "
            + " WHERE A.ROUTE_ID = B.ROUTE_ID AND A.NODE_ID = B.GROUP_ID AND B.NODE_CONTENT = "
            + " C.PROCESS_ID AND A.RC_NO = '" + sRC_NO + "'";

            dsGrid = ClientUtils.ExecuteSQL(sSQL);//获取Group的信息
            //是否有Group数据
            if (dsGrid != null && dsGrid.Tables[0].Rows.Count > 0)
            {
                sSQL = "select count(*) from sajet.g_rc_process_group where rc_no='" + sRC_NO + "'";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                //数据是否已经加入过g_rc_process_group表
                if (dsTemp != null && dsTemp.Tables[0].Rows.Count > 0
                    && int.Parse(dsTemp.Tables[0].Rows[0][0].ToString()) > 0) { }
                else
                {
                    //加入and还是or的判断
                    //获取Group_ID
                    // 2015/08/24, Aaron
                    string gSQL = "SELECT distinct A.route_id, b.group_id  FROM SAJET.G_RC_STATUS A, SAJET.SYS_RC_ROUTE_DETAIL B,SAJET.SYS_PROCESS C  WHERE A.ROUTE_ID = B.ROUTE_ID AND A.NODE_ID = B.GROUP_ID AND B.NODE_CONTENT =  C.PROCESS_ID AND A.RC_NO = '" + sRC_NO + "'";
                    Group = ClientUtils.ExecuteSQL(gSQL);
                    string gRoute_ID = Group.Tables[0].Rows[0][0].ToString();
                    string gGroup = Group.Tables[0].Rows[0][1].ToString();//
                    //获取node_type
                    gSQL = "select a.node_type from sajet.sys_rc_route_detail a , sajet.sys_rc_route b where a.route_id=b.route_id and b.route_id='" + gRoute_ID + "' and a.node_id='" + gGroup + "'";
                    Group = ClientUtils.ExecuteSQL(gSQL);
                    string g;//用与记录and还是or
                    g = Group.Tables[0].Rows[0][0].ToString();
                    //如果是and
                    if (g == "2")
                    {
                        g = "and";
                    }
                    //如果是or
                    if (g == "3")
                    {
                        g = "or";
                    }
                    //将数据写入g_rc_process_froup
                    for (int ii = 0; ii < dsGrid.Tables[0].Rows.Count; ii++)
                    {
                        upSQL = " Insert into SAJET.g_rc_process_group "
                             + " (RC_NO,PROCESS_ID,PROCESS_NAME,STATUS,PROCESS_TYPE,GROUP_ID,SEQ) "
                             + " Values "
                             + " (:RC_NAME,:PROCESS_ID,:PROCESS_NAME,'Queue',:PROCESS_TYPE,:GROUP_ID,:SEQ) ";
                        object[][] Params = new object[6][];
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NAME", dsGrid.Tables[0].Rows[ii][0].ToString() };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", dsGrid.Tables[0].Rows[ii][1].ToString() };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_NAME", dsGrid.Tables[0].Rows[ii][2].ToString() };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_TYPE", g };
                        Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "GROUP_ID", gGroup };
                        Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SEQ", ii };
                        dsTemp = ClientUtils.ExecuteSQL(upSQL, Params);
                    }
                }
            }

            //dgvRC.Rows[0].Cells[0].Value = false;

            sSQL = "select A.SEQ,a.process_id,a.process_name,a.status from sajet.g_rc_process_group a where a.rc_no='" + sRC_NO + "' order by a.SEQ";
            dsGrid = ClientUtils.ExecuteSQL(sSQL);
            dgvRC.DataSource = dsGrid;
            dgvRC.DataMember = dsGrid.Tables[0].ToString();
            dgvRC.VirtualMode = false;
            dgvRC.Columns["SEQ"].Width = 50;
            dgvRC.Columns["PROCESS_ID"].Width = 100;
            dgvRC.Columns["PROCESS_NAME"].Width = 160;
            dgvRC.Columns["STATUS"].Width = 80;
            dgvRC.ColumnWidthChanged -= dataGridView1_ColumnWidthChanged;
            dgvRC.ColumnWidthChanged += new DataGridViewColumnEventHandler(dataGridView1_ColumnWidthChanged);
            SajetCommon.SetLanguageControl(this);
            dsGrid.Dispose();
        }
        private void showGroupProcess_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sCheck = "";
            int n = 0;
            DataSet dsTemp = new DataSet();
            //如果有选择，记为“OK”
            foreach (DataGridViewRow row in dgvRC.Rows)
            {
                string s = Convert.ToString(row.Cells[3].Value);
                bool check = Convert.ToBoolean(row.Cells[0].FormattedValue);
                if (check == true)
                {
                    sCheck = "OK";
                    break;
                }
                n++;
            }

            //如果没有选择，弹出窗口
            if (sCheck != "OK")
            {
                string sMsg = SajetCommon.SetLanguage("Please select Process!");//请选择制程
                SajetCommon.Show_Message(sMsg, 1);
                //MessageBox.Show("Please select Process!");
                return;
            }

            string ID = dgvRC.Rows[n].Cells["PROCESS_ID"].Value.ToString();
            string SEQ = dgvRC.Rows[n].Cells["SEQ"].Value.ToString();
            //SHEET_PHASE给初值'N/A'
            string sSQL = "update sajet.g_rc_process_group set SHEET_PHASE ='N/A' where rc_no='" + sRC_NO + "'";
            ClientUtils.ExecuteSQL(sSQL);

            //string sID = dgvRC.CurrentRow.Cells["PROCESS_ID"].Value.ToString();
            sSQL = "select sheet_phase from sajet.g_rc_process_group where rc_no='" + sRC_NO + "' and SEQ='" + SEQ + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp != null && dsTemp.Tables[0].Rows.Count > 0)
            {
                if (dsTemp.Tables[0].Rows[0][0].ToString() == "N/A")
                {
                    sSQL = " update sajet.g_rc_process_group set SHEET_PHASE ='I' where PROCESS_ID='" + ID + "' and rc_no='" + sRC_NO + "' and SEQ=" + SEQ;
                    ClientUtils.ExecuteSQL(sSQL);
                }
            }

            // 将group，process_id,SHEET_NAME写入status表

            sSQL = "SELECT B.SHEET_NAME FROM SAJET.G_RC_PROCESS_GROUP A, SAJET.SYS_RC_PROCESS_SHEET B WHERE A.PROCESS_ID = B.PROCESS_ID AND A.SHEET_PHASE=B.SHEET_PHASE AND A.SEQ='" + SEQ + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                string sMsg = SajetCommon.SetLanguage("Process sheet not found");//未找到Process sheet
                SajetCommon.Show_Message(sMsg, 1);
                //MessageBox.Show("Process sheet not found");
                return;
            }
            string SHEET_NAME = dsTemp.Tables[0].Rows[0][0].ToString();

            // 2015/08/24, Aaron
            sSQL = "update sajet.g_rc_status set process_id ='" + ID + "',SHEET_NAME='" + SHEET_NAME + "' where rc_no='" + sRC_NO + "'";
            ClientUtils.ExecuteSQL(sSQL);

            sSQL = "update sajet.g_sn_status set process_id ='" + ID + "',SHEET_NAME='" + SHEET_NAME + "' where rc_no='" + sRC_NO + "' and CURRENT_STATUS In (0,1,2) ";
            ClientUtils.ExecuteSQL(sSQL);

            this.DialogResult = DialogResult.OK;//记录已被选择点击执行
            this.Close();
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            string s = "";
            for (int i = 0; i < dgvRC.Columns.Count; i++)
                s = s + dgvRC.Columns[i].Width + ",";
            SajetInifile sini = new SajetInifile();
            sini.WriteIniFile(sIniFile, "RC Travel", "表格欄寬", s);
            sini.Dispose();
        }

        //选择框事件
        private void dgvRC_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;//用于记录所选列
            //如果没有选中任何列，跳出
            if (rowIndex < 0)
            {
                return;
            }
            //添加判断所选制程是否已执行过，如果已经执行过，就不能再被选择
            string ID = dgvRC.Rows[rowIndex].Cells["PROCESS_ID"].Value.ToString();
            string SEQ = dgvRC.Rows[rowIndex].Cells["SEQ"].Value.ToString();
            string choose;
            string cSQL;
            DataSet status;
            cSQL = "select a.status from sajet.g_rc_process_group a where a.RC_NO='" + sRC_NO + "' and a.SEQ='" + SEQ + "'";
            status = ClientUtils.ExecuteSQL(cSQL);
            choose = status.Tables[0].Rows[0][0].ToString();
            if (choose == "Complete")
            {
                dgvRC.Rows[rowIndex].ReadOnly = true;
                return;
            }


            //int count = Convert.ToInt16(dgvRC.Rows.Count.ToString());

            //DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)dgvRC.Rows[i].Cells["Column1"];
            //Boolean flag = Convert.ToBoolean(checkCell.Value);
            //string ss = dgvRC.Rows[i].Cells[0].Value.ToString();
            //if (Convert.ToString(dgvRC.Rows[i].Cells[0].Value) == "true")     //查找被选择的数据行  
            //{
            //    dgvRC.Rows[i].Cells["Column1"].Value = "false";
            //}
            //if((CheckBox)dgvRC.Rows[i].f)


            /*实现单选功能*/

            int n = 0;
            foreach (DataGridViewRow row in dgvRC.Rows)
            {
                string s = Convert.ToString(row.Cells[3].Value);
                bool check = Convert.ToBoolean(row.Cells[0].FormattedValue);
                if (check == true)
                {
                    dgvRC.Rows[n].Cells[0].Value = false;
                    //dgvRC.EndEdit();
                }
                n++;
            }
            //dgvRC.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
