using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using SajetTable;

namespace CMachineType
{
    public partial class fMachineEngineer : Form
    {
        public fMachineEngineer()
        {
            InitializeComponent();
        }

        DataSet dsTemp;
        public String g_sMachineTypeID, g_sMachineTypeName;
            
        private void Show_Engineer()
        {
            LVAll.Items.Clear();
            string sSQL = " select emp.emp_id, emp.emp_no, emp.emp_name, dept.dept_name "
                        + " from sajet.sys_emp emp, sajet.sys_dept dept "
                        + " where emp.enabled = 'Y' and emp.dept_id = dept.dept_id ";

            if (combFilter.SelectedIndex > -1 && editFilter.Text.Trim() != "")
            {
                string sFieldName = combFilterField.Items[combFilter.SelectedIndex].ToString();
                sSQL = sSQL + " and ";
                sSQL = sSQL + sFieldName + " like '%" + editFilter.Text.Trim() + "%'";
            }
            sSQL = sSQL + " order by emp_no ";

            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                LVAll.Items.Add(dsTemp.Tables[0].Rows[i]["EMP_NO"].ToString());
                LVAll.Items[i].SubItems.Add(dsTemp.Tables[0].Rows[i]["EMP_NAME"].ToString());
                LVAll.Items[i].SubItems.Add(dsTemp.Tables[0].Rows[i]["DEPT_NAME"].ToString());
                LVAll.Items[i].SubItems.Add(dsTemp.Tables[0].Rows[i]["EMP_ID"].ToString());
                LVAll.Items[i].ImageIndex = 0;
            }
        }

        private void bbtnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= LVAll.Items.Count - 1; i++)
            {
                LVAll.Items[i].Checked = true;
            }
        }

        private void bbtnSelectNone_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= LVAll.Items.Count - 1; i++)
            {
                LVAll.Items[i].Checked = false;
            }
        }

        private void bbtnChoose_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= LVAll.Items.Count - 1; i++)
            {
                if (LVAll.Items[i].Checked)
                {
                    //Find必須用Name來找,因此將Name設成跟Text相同
                    if (LVChoose.Items.Find(LVAll.Items[i].Text, false).Length == 0)
                    {
                        LVChoose.Items.Add(LVAll.Items[i].Text);
                        LVChoose.Items[LVChoose.Items.Count - 1].SubItems.Add(LVAll.Items[i].SubItems[1].Text);
                        LVChoose.Items[LVChoose.Items.Count - 1].SubItems.Add(LVAll.Items[i].SubItems[2].Text);
                        LVChoose.Items[LVChoose.Items.Count - 1].SubItems.Add(LVAll.Items[i].SubItems[3].Text);
                        LVChoose.Items[LVChoose.Items.Count - 1].Name = LVAll.Items[i].Text;
                        LVChoose.Items[LVChoose.Items.Count - 1].ImageIndex = 0;
                    }
                }
            }
        }

        private void fMachineEngineer_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            panel1.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            LabMachineType.Text = g_sMachineTypeName;
            Show_Engineer();
            LVChoose.Items.Clear();
            string sSQL = " select emp.emp_id, emp.emp_no, emp.emp_name, dept.dept_name "
                        + " from sajet.sys_machine_engineer type,sajet.sys_emp emp, sajet.sys_dept dept "
                        + " where type.machine_type_id = '" + g_sMachineTypeID + "' "
                        + " and type.machine_engineer = emp.emp_id "
                        + " and emp.enabled='Y' "
                        + " and emp.dept_id = dept.dept_id "
                        + " order by emp.emp_no ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                LVChoose.Items.Add(dsTemp.Tables[0].Rows[i]["emp_no"].ToString());
                LVChoose.Items[i].Name = dsTemp.Tables[0].Rows[i]["emp_no"].ToString();
                LVChoose.Items[i].SubItems.Add(dsTemp.Tables[0].Rows[i]["emp_name"].ToString());
                LVChoose.Items[i].SubItems.Add(dsTemp.Tables[0].Rows[i]["dept_name"].ToString());
                LVChoose.Items[i].SubItems.Add(dsTemp.Tables[0].Rows[i]["emp_id"].ToString());
                LVChoose.Items[i].ImageIndex = 0;
            }

            //Filter
            combFilter.Items.Clear();
            combFilterField.Items.Clear();
            for (int i = 0; i < LVAll.Columns.Count; i++)
            {
                combFilter.Items.Add(LVAll.Columns[i].Text);
                combFilterField.Items.Add(LVAll.Columns[i].Tag);
            }
            if (combFilter.Items.Count > 0)
                combFilter.SelectedIndex = 0;
        }

        private void bbtnSave_Click(object sender, EventArgs e)
        {
            string sSQL = " Delete sajet.sys_machine_engineer "
                        + " Where machine_type_id = '" + g_sMachineTypeID + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            for (int i = 0; i <= LVChoose.Items.Count - 1; i++)
            {
                string sEmpID = LVChoose.Items[i].SubItems[3].Text;
                sSQL = " Insert Into sajet.sys_machine_engineer "
                     + " (MACHINE_TYPE_ID, MACHINE_ENGINEER, UPDATE_USERID) "
                     + " Values "
                     + " ('" + g_sMachineTypeID + "','" + sEmpID +"','" + fMain.g_sUserID + "') ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
            }
            DialogResult = DialogResult.OK;
        }

        private void bbtnRemove_Click(object sender, EventArgs e)
        {
            for (int i = LVChoose.Items.Count - 1; i >= 0; i--)
            {
                if (LVChoose.Items[i].Checked)
                    LVChoose.Items[i].Remove();
            }
        }

        private void bbtnSelectAll1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= LVChoose.Items.Count - 1; i++)
            {
                LVChoose.Items[i].Checked = true;
            }
        }

        private void bbtnSelectNone1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= LVChoose.Items.Count - 1; i++)
            {
                LVChoose.Items[i].Checked = false;
            }
        }

        private void editFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            Show_Engineer();

            editFilter.Focus();
        }
    }
}