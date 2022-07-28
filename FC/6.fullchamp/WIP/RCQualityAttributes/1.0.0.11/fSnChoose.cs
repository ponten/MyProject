using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;

namespace RCQualityAttributes
{
    public partial class fSnChoose : Form
    {
        string sMessage;
        private string sRC_NO;
        DataSet ds;
        public string sn;
        public string[] sSN = new string[100];
        string number;
        public fSnChoose(string sRC,string numb)
        {
            InitializeComponent();
            sRC_NO = sRC;
            number = numb;
        }
        public fSnChoose(string sRC)
        {
            InitializeComponent();
            sRC_NO = sRC;
            number = "1";
        }

        private void fSnChoose_Load(object sender, EventArgs e)
        {
            label3.Text = "数量";
            textBox2.Text = "0";
            SajetClass.SajetCommon.SetLanguageControl(this);
            string sSQL = "select *  from sajet.G_RC_STATUS a,sajet.G_SN_STATUS b where a.have_sn='1' and a.rc_no=b.rc_no and a.rc_no='" + sRC_NO + "'";
            ds = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                string aa = ds.Tables[0].Rows[i]["SERIAL_NUMBER"].ToString();
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[1].Value = aa;
                snBox.Items.Add(ds.Tables[0].Rows[i]["SERIAL_NUMBER"].ToString());
            }
            //去除下面空白
            dataGridView1.AllowUserToAddRows = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sSQL = string.Empty;
            DataSet dsTemp = new DataSet();
            int j = 0;
            //记录所选SN数量
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].FormattedValue))
                {
                    j++;
                }
            }
            //如果没有选择对应数量的SN
            if (j.ToString() != number)
            {
                string a = SajetCommon.SetLanguage("please choose");
                string b = SajetCommon.SetLanguage("a SN");
                sMessage = a + number + b;
                SajetCommon.Show_Message(sMessage, 0);
                return;
            }
                

            //如果没有选择，弹出窗口
            //if (snBox.Text=="")
            //{
            //    MessageBox.Show("Please select Process!");
            //    return;
            //}
            j = 0;
            //记录所选的SN
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].FormattedValue))
                {
                    sSN[j] = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    j++;
                    sn = dataGridView1.Rows[i].Cells[1].Value.ToString();
                }
            }


                //sn = snBox.Text;
            this.DialogResult = DialogResult.OK;//记录已被选择点击执行
            this.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            int i;
            string jilu="";
            for (i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (textBox1.Text == dataGridView1.Rows[i].Cells[1].Value.ToString())
                {
                    dataGridView1.Rows[i].Cells[0].Value = true;
                    jilu = "ok";
                }
            }
            if (jilu != "ok")
            {
                sMessage = SajetCommon.SetLanguage("please choose");
                SajetCommon.Show_Message(sMessage, 1);
                return;
            }

        }


        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int no = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].FormattedValue))
                {
                    no++;
                }
            }
            textBox2.Text = no.ToString();
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }


    }
}
