using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using SajetClass;

namespace RCQualityAttributes
{
    public partial class fMeasurement : Form
    {
        string have_sn;//记录是否有SN
        int rows, columns;
        string sql;
        DataSet dsTemp;
        string sRC;
        int sNumber;
        string partNO;
        //string partID;
        string processID;
        string typeCode;
        string recID;
        string g_sItemTypeID;
        string[] testID = new string[20];
        string [][]testEnd = new string[20][];
        //DataColumn jieguo;
        //DataColumn lieSN;
        DataTable dt = new DataTable();
        DataGridViewTextBoxColumn RTText = new DataGridViewTextBoxColumn();
        string ifgonext;
        string qcLOT;
        int shuliang;
        int sSample_num;
        public string sPartID;

        public fMeasurement(string rc,int number,string ptNO,string prID,string tcode,string qclot)
        {
            InitializeComponent();
            SajetClass.SajetCommon.SetLanguageControl(this);
            sRC = rc;
            sNumber = number;
            partNO = ptNO;
            processID = prID;
            typeCode = tcode;
            ifgonext = "no";
            qcLOT = qclot;
        }

        private void fMeasurement_Load(object sender, EventArgs e)
        {
            //string SNChoose =  SajetCommon.SetLanguage("SNChoose");
            //sNchooseToolStripMenuItem.Text = SNChoose;
            Showdgv();
            shuliang = 0;
        }

        private void Showdgv()
        {
            //获取流水号
            sql = @"SELECT A.RECID "
                + "FROM SAJET.SYS_PART_QC_PROCESS_RULE A, SAJET.SYS_QC_SAMPLING_RULE     B, SAJET.SYS_PROCESS C "
                + "WHERE A.PART_ID = '" + sPartID + "' "
                + "AND A.SAMPLING_RULE_ID = B.SAMPLING_RULE_ID(+) "
                + "AND A.PROCESS_ID = C.PROCESS_ID(+) "
                + "AND A.ENABLED = 'Y' "
                + "AND A.PROCESS_ID='" + processID + "'";
            dsTemp = ClientUtils.ExecuteSQL(sql);
            recID = dsTemp.Tables[0].Rows[0]["RECID"].ToString();
            
            sql = " Select a.item_type_id "
                + "from SAJET.SYS_PART_QC_TESTTYPE a, "
                + "sajet.sys_test_item_type   b, "
                + "sajet.sys_qc_sampling_plan c "
                + "where A.RECID = '" + recID + "' "
                + "and a.item_type_id = b.item_type_id "
                + "and b.item_type_code = '" + typeCode + "'"
                + "and a.sampling_id = c.sampling_id(+) ";
            dsTemp = ClientUtils.ExecuteSQL(sql);
            g_sItemTypeID = dsTemp.Tables[0].Rows[0]["item_type_id"].ToString();
      
            //int i;
            string ssql = "SELECT *  FROM SAJET.G_RC_STATUS A,SAJET.G_SN_STATUS B WHERE A.HAVE_SN='1' AND A.RC_NO=B.RC_NO AND A.RC_NO='" + sRC + "'";
            dsTemp = ClientUtils.ExecuteSQL(ssql);

            have_sn = "";
            if (dsTemp.Tables[0].Rows.Count != 0)
            { 
                have_sn = dsTemp.Tables[0].Rows[0][0].ToString(); 
            }
            if (have_sn == "")
            {                
                string a = SajetCommon.SetLanguage("number");
                DataGridViewTextBoxColumn SNText = new DataGridViewTextBoxColumn();
                SNText.Name = a;
                SNText.HeaderText = SajetCommon.SetLanguage(a, 1);
                SNText.ReadOnly = true;
                dgvT.Columns.Add(SNText);
            }
            else
            {
                string a = SajetCommon.SetLanguage("SN");
                DataGridViewTextBoxColumn SNText = new DataGridViewTextBoxColumn();
                SNText.Name = a;
                SNText.HeaderText = SajetCommon.SetLanguage(a, 1);
                SNText.ReadOnly = true;
                dgvT.Columns.Add(SNText); 
            }
                
            sql = " select * "
                        + "from SAJET.SYS_PART_QC_TESTITEM a "
                        + "left join SAJET.SYS_TEST_ITEM c "
                        + "on a.ITEM_ID = c.ITEM_ID "
                        + " left join SAJET.SYS_TEST_ITEM_TYPE d "
                        + "  on c.item_type_id = d.item_type_id "
                        + " where A.RECID = '" + recID + "' "
                        + " and C.ITEM_TYPE_ID = '" + g_sItemTypeID + "' "
                        + " and a.enabled = 'Y' "
                        + " and c.enabled = 'Y' "
                        + "and d.enabled = 'Y' "
                        + "order by ITEM_TYPE_CODE, ITEM_CODE ";
            dsTemp = ClientUtils.ExecuteSQL(sql);

            foreach (DataRow dc in dsTemp.Tables[0].Rows)
            {
                DataGridViewTextBoxColumn dText = new DataGridViewTextBoxColumn();
                dText.Name = dc["ITEM_NAME"].ToString();
                dText.HeaderText = SajetCommon.SetLanguage(dc["ITEM_NAME"].ToString(), 1);
                dText.ReadOnly = true;
                dgvT.Columns.Add(dText);

            }

            for (int m = 0; m < dsTemp.Tables[0].Rows.Count; m++)
            {
                sSample_num++;
                testID[m] = dsTemp.Tables[0].Rows[m]["ITEM_ID"].ToString();
            }

            string aaa = SajetClass.SajetCommon.SetLanguage("Whether test qualified");
            RTText = new DataGridViewTextBoxColumn();
            RTText.Name = aaa;
            RTText.HeaderText = SajetCommon.SetLanguage(aaa, 1);
            RTText.ReadOnly = true;
            dgvT.Columns.Add(RTText);

            //获取行数列数
            rows = dgvT.Rows.Count;
            columns = dgvT.Columns.Count;

            //去除左边空白
            dgvT.RowHeadersVisible = false;
            //去除下面空白
            dgvT.AllowUserToAddRows = false;

            //添加获取表格信息
            getinformation();

            ifgonext = "ok";
            cleartestEnd(columns);
            if (have_sn == "")
            {
                textBox1.Visible = false;
                label1.Visible = false;
            }
        }

        //初始化testEnd数组
        private void cleartestEnd(int cls)
        {
            for (int i = 0; i < 20; i++)
            {
                testEnd[i] = new string[cls];
            }
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < cls;j++)
                    testEnd[i][j] = "";
            }
        }

        private void getinformation()
        {
            int i, j;
            string gsql;
            DataSet gsthing;
            string it_name, n_sn, ginformation;
            it_name = dgvT.Columns[0].HeaderText;
            gsql = "select distinct(a.NUMBER_SN) from sajet.G_QC_LOT_TEST_ITEM a where a.rc_no='" + sRC + "' and a.typecode_name='" + typeCode + "' and a.qc_lotno='" + qcLOT + "'";
            gsthing = ClientUtils.ExecuteSQL(gsql);

            //获取小项结果
            for (i = 1; i < columns - 1; i++)
            {
                for (j = 0; j < rows-1; j++)
                {
                    it_name = dgvT.Columns[i].HeaderText.ToString();
                    n_sn = dgvT.Rows[j].Cells[0].Value.ToString();
                        
                        
                    gsql = "select a.information from sajet.G_QC_LOT_TEST_ITEM a where a.rc_no='" + sRC + "' and a.typecode_name='" + typeCode + "' and a.item_type_name='" + it_name + "' and a.number_sn='" + n_sn + "' and a.qc_lotno='" + qcLOT + "'";
                    gsthing = ClientUtils.ExecuteSQL(gsql);
                    if (gsthing.Tables[0].Rows.Count == 0)
                    {
                        continue;
                    }
                    ginformation = gsthing.Tables[0].Rows[0][0].ToString();
                    dgvT.Rows[j].Cells[i].Value = ginformation;

                }
            }

            //获取SN测试总结果
            for (j = 0; j < rows-1; j++)
            {
                it_name = dgvT.Columns[columns - 1].HeaderText.ToString();
                n_sn = dgvT.Rows[j].Cells[0].Value.ToString();
                gsql = "select a.information from sajet.G_QC_LOT_TEST_ITEM a where a.rc_no='" + sRC + "' and a.typecode_name='" + typeCode + "' and a.item_type_name='" + it_name + "' and a.number_sn='" + n_sn + "' and a.qc_lotno='" + qcLOT + "'";
                gsthing = ClientUtils.ExecuteSQL(gsql);
                if (gsthing.Tables[0].Rows.Count == 0)
                {
                    continue;
                }
                ginformation = gsthing.Tables[0].Rows[0][0].ToString();
                RTText.ReadOnly = false;
                dgvT.Rows[j].Cells[i].Value = ginformation;
                if (ginformation == "OK")
                {
                    dgvT.Rows[j].Cells[i].Style.BackColor = Color.Green;
                    dgvT.Rows[j].Cells[i].Style.ForeColor = Color.White;
                }
                if (ginformation == "NG")
                {
                    dgvT.Rows[j].Cells[i].Style.BackColor = Color.Red;
                    dgvT.Rows[j].Cells[i].Style.ForeColor = Color.White;
                }

                RTText.ReadOnly = true;
            }
        }
        private void button_Click(object sender, EventArgs e)
        {
            int i, j;
            object[][] Params = new object[8][];
            if (dgvT.Rows.Count == 0)
            {
                string sMsg = SajetCommon.SetLanguage("please Input SN");
                SajetCommon.Show_Message(sMsg,0);
                return;
            }
            if (dgvT.Rows.Count > 0)
            {
                if (dgvT.Rows.Count < sNumber)
                {
                    string sMsg = SajetCommon.SetLanguage("sampling is not enough");
                    SajetCommon.Show_Message(sMsg,0);
                    return;
                }
                for (int m = 0; m < dgvT.Rows.Count; m++)
                {
                    for (int n = 1; n < dgvT.Columns.Count - 1; n++)
                    {
                        if (dgvT.Rows[m].Cells[n].Value == null)
                        {
                            string sMsg = SajetCommon.SetLanguage("Item test is not complete");
                            SajetCommon.Show_Message(sMsg,1);
                            return;
                        }
                        Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TEST_NAME", dgvT.Columns[n].HeaderText.ToString() };
                        Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NUMBERSN", dgvT.Rows[m].Cells[0].Value.ToString() };
                        Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "INFMATION", dgvT.Rows[m].Cells[n].Value.ToString() };
                        Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC", sRC };
                        Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TYPECODE", typeCode };
                        Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RT", testEnd[m][n] };
                        Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "QC", qcLOT };
                        Params[7] = new object[] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };
                        dsTemp = ClientUtils.ExecuteProc("SAJET.SJ_RC_QC_ITEM", Params);
                        string sResult = dsTemp.Tables[0].Rows[0]["TRES"].ToString();
                        if (sResult != "OK")
                        {
                            SajetCommon.Show_Message(sResult, 0);
                            return;
                        }
                    }
                }
            } 
            this.DialogResult = DialogResult.OK;//记录已选择ok执行
            this.Close();
        }

        /// <summary>
        /// dgv事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        //到最后一行后换行事件，不是输入回车事件
        private void dgvT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;      

            //焦点换行事件
            int a, b;
            int grow = 0, gcolumn = 0;
            rows = dgvT.Rows.Count;
            columns = dgvT.Columns.Count;

            //获取选择位置
            for (a = 0; a < rows; a++)
            {
                for (b = 0; b < columns; b++)
                {
                    if (dgvT.Rows[a].Cells[b] == dgvT.CurrentCell)
                    {
                        grow = a;
                        gcolumn = b;
                    }
                }
            }

            
            //到最后一行时，转下一列第一行
            if (grow == rows - 1 && gcolumn != columns - 2 && gcolumn != 0)
            {
                dgvT.CurrentCell = dgvT[gcolumn + 1, 0];                
                this.dgvT.BeginEdit(true);
            }
            //第一列最后一行时
            if (grow == rows - 1 && gcolumn ==0)
            {
                dgvT.CurrentCell = dgvT[gcolumn + 1, 0];
                ifgonext = "ok";
                this.dgvT.BeginEdit(true);
            }

            ifgonext = "ok";
        }

        private void dgvT_SelectionChanged(object sender, EventArgs e)
        {

            string a = dgvT.Rows.Count.ToString();
            string b = dgvT.Columns.Count.ToString();
            if (a == "0")
                return;

            if(dgvT.CurrentCell.ColumnIndex.ToString()==b)
                return;

            if (dgvT.CurrentCell.ColumnIndex.ToString() == "0")
            {
                ifgonext = "no";
                return;
            }
            if (dgvT.CurrentCell.ColumnIndex.ToString() == "1")
                ifgonext = "ok";
            
            ////跳到下一行时，为输入状态
            dgvT.BeginEdit(true);
        }

        private void dgvT_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {  
          
            if (e.ColumnIndex.ToString() == "0")
            {
                ifgonext = "no";
                //dgvT.BeginEdit(true);
                return;
            }

            if (e.RowIndex.ToString() == "-1")
                return;

        }

        private void dgvT_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (ifgonext == "no")
            {
                return;
            }
            //绑定事件
            ((TextBox)e.Control).TextChanged += new EventHandler(cbb_TextChanged);
        }
        private void cbb_TextChanged(object sender, EventArgs e)
        {
            dgvT.CurrentRow.Cells[dgvT.Columns.Count - 1].Value = "";
            TextBox combox = sender as TextBox;
            //这里比较重要
            combox.Leave += new EventHandler(combox_Leave);

            if (ifgonext == "no")
            {
                return;
            }
            string row = (dgvT.CurrentCell.RowIndex).ToString();
            string column = dgvT.CurrentCell.ColumnIndex.ToString();
            int intcolumn = Int32.Parse(column);
            int introw = Int32.Parse(row);

            TextBox chk = (TextBox) sender;
            string putin = chk.Text;
            
            string ssql = "select a.*, d.item_type_code, d.item_type_name, c.item_code, c.item_name "
                       + "from SAJET.SYS_PART_QC_TESTITEM a "
                       + "left join SAJET.SYS_TEST_ITEM c "
                       + "on a.ITEM_ID = c.ITEM_ID "
                       + "left join SAJET.SYS_TEST_ITEM_TYPE d "
                       + "on c.item_type_id = d.item_type_id "
                       + "where A.RECID = '" + recID + "' "
                       + "and C.ITEM_TYPE_ID = '" + g_sItemTypeID + "' "
                       + "and A.item_id='" + testID[intcolumn-1] + "' "
                       + "and a.enabled = 'Y' "
                       + "and c.enabled = 'Y' "
                       + "and d.enabled = 'Y' "
                       + "order by ITEM_TYPE_CODE, ITEM_CODE ";
            DataSet dTemp = ClientUtils.ExecuteSQL(ssql);
            string end = "";
            if (dTemp.Tables[0].Rows.Count> 0)
            {
                if (putin == "")
                {
                    dgvT.BeginEdit(true);
                    return;
                }
                string textNumberMax = dTemp.Tables[0].Rows[0]["UPPER_LIMIT"].ToString();
                string textNumberMin = dTemp.Tables[0].Rows[0]["LOWER_LIMIT"].ToString();
                string testend = "OK";
                if (textNumberMin != "" && textNumberMax != "")
                {
                    if (Int32.Parse(textNumberMin) < Int32.Parse(putin) && Int32.Parse(putin) < Int32.Parse(textNumberMax))
                    {
                        end = "OK";
                        testEnd[introw][intcolumn] = end;
                        for (int ii = 0; ii < dgvT.Columns.Count; ii++)
                        {
                            if (testEnd[introw][ii] == "NG")
                            {
                                testend = "NG";
                            }
                        }
                    }
                    else
                    {
                        end = "NG";
                        testEnd[introw][intcolumn] = end;
                        for (int ii = 0; ii < dgvT.Columns.Count; ii++)
                        {
                            if (testEnd[introw][ii] == "NG")
                            {
                                testend = "NG";
                            }
                        }
                    } 
                    

                    if (testend == "OK")
                    {
                        RTText.ReadOnly = false;
                        dgvT.Rows[Int32.Parse(row)].Cells[dgvT.ColumnCount - 1].Value = "OK";
                        dgvT.Rows[Int32.Parse(row)].Cells[dgvT.ColumnCount - 1].Style.BackColor = Color.Green;
                        dgvT.Rows[Int32.Parse(row)].Cells[dgvT.ColumnCount - 1].Style.ForeColor = Color.White;
                        RTText.ReadOnly = true;
                    }
                    else
                    {
                        RTText.ReadOnly = false;
                        dgvT.Rows[Int32.Parse(row)].Cells[dgvT.ColumnCount - 1].Value = "NG";
                        dgvT.Rows[Int32.Parse(row)].Cells[dgvT.ColumnCount - 1].Style.BackColor = Color.Red;
                        dgvT.Rows[Int32.Parse(row)].Cells[dgvT.ColumnCount - 1].Style.ForeColor = Color.White;
                        RTText.ReadOnly = true;
                    }
                }
                else
                {
                    testEnd[introw][intcolumn] = "OK";
                    for (int ii = 0; ii < dgvT.Columns.Count; ii++)
                    {
                        if (testEnd[introw][ii] == "NG")
                        {
                            testend = "NG";
                        }
                    }
                    if (testend == "OK")
                    {
                        RTText.ReadOnly = false;
                        dgvT.Rows[Int32.Parse(row)].Cells[dgvT.ColumnCount - 1].Value = "OK";
                        dgvT.Rows[Int32.Parse(row)].Cells[dgvT.ColumnCount - 1].Style.BackColor = Color.Green;
                        dgvT.Rows[Int32.Parse(row)].Cells[dgvT.ColumnCount - 1].Style.ForeColor = Color.White;
                        RTText.ReadOnly = true;
                    }
                    else
                    {
                        RTText.ReadOnly = false;
                        dgvT.Rows[Int32.Parse(row)].Cells[dgvT.ColumnCount - 1].Value = "NG";
                        dgvT.Rows[Int32.Parse(row)].Cells[dgvT.ColumnCount - 1].Style.BackColor = Color.Red;
                        dgvT.Rows[Int32.Parse(row)].Cells[dgvT.ColumnCount - 1].Style.ForeColor = Color.White;
                        RTText.ReadOnly = true;
                    }
                }
            }
        }

        public void combox_Leave(object sender, EventArgs e)
        {
            TextBox combox = sender as TextBox;
            //做完处理，须撤销动态事件
            combox.TextChanged -= new EventHandler(cbb_TextChanged);
        }


        //不要删除
        //string SN;
        //private void sNchooseToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    fSnChoose show = new fSnChoose(sRC);
        //    if (show.ShowDialog() == DialogResult.OK)
        //    {
        //        SN = show.sn;
        //        int t = dgvT.CurrentRow.Index;
        //        lieSN.ReadOnly = false;
        //        dgvT.Rows[t].Cells[0].Value = SN;
        //        // 取消只读
        //        lieSN.ReadOnly = false;
        //    }
            
        //}

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            if (sSample_num == 0)
            {
                string sMsg = SajetCommon.SetLanguage("please set item");
                SajetCommon.Show_Message(sMsg,1);
                return;
            }
            shuliang = dgvT.Rows.Count;
            if (shuliang >= sNumber)
            {
                string sMsg = SajetCommon.SetLanguage("out the upper limit");
                SajetCommon.Show_Message(sMsg,1);
                return;
            }

            if (dgvT.Rows.Count > 0)
            {
                for (int j = 0; j < dgvT.Rows.Count; j++)
                {
                    if (dgvT.Rows[j].Cells[0].Value.ToString() == textBox1.Text)
                    {
                        string sMsg = SajetCommon.SetLanguage("SN Is duplicate");
                        SajetCommon.Show_Message(sMsg, 1);
                        return;
                    }
                }
            }

            string sSQL = "SELECT *  FROM SAJET.G_RC_STATUS A,SAJET.G_SN_STATUS B WHERE A.HAVE_SN='1' AND A.RC_NO=B.RC_NO AND A.RC_NO='" + sRC + "'";
            DataSet ds = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                if (ds.Tables[0].Rows[i]["SERIAL_NUMBER"].ToString() == textBox1.Text)
                {
                    dgvT.Rows.Add();
                    dgvT.Rows[shuliang].Cells[0].Value = textBox1.Text;

                    for (int j = 1; j <= sSample_num; j++)
                    {
                        dgvT.Rows[shuliang].Cells[j].ReadOnly = false;
                    }
                }
            }
            textBox1.Clear();
            textBox1.Focus();
            //textBox1.SelectAll();
        }
    }
}
