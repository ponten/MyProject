using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Collections.Specialized;

namespace RCQualityAttributes
{
    public partial class fFilter : Form
    {
        public fFilter()
        {
            InitializeComponent();
        }
        
        string sSQL;
        public string s_ProcessID;
        public string s_DefectRange;
        public string s_DefectCode;
        public string s_DefectDesc;
        public string s_DefectID;
        StringCollection g_tsField = new StringCollection();

        //˫��ѡȡ��Ӧ�Ĳ���
        private void dgvData_DoubleClick(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count > 0 && dgvData.CurrentRow != null)
            {
                s_DefectCode = dgvData.CurrentRow.Cells["DEFECT_CODE"].Value.ToString();
                s_DefectDesc = dgvData.CurrentRow.Cells["DEFECT_DESC"].Value.ToString();
                s_DefectRange = dgvData.CurrentRow.Cells["DEFECT_LEVEL"].Value.ToString();
                s_DefectID = dgvData.CurrentRow.Cells["DEFECT_ID"].Value.ToString();
                DialogResult = DialogResult.OK;
            }
        }

        //������ظ��ݸ�����PROCESS_ID�����󶨵���Ӧ����
        private void fFilter_Load(object sender, EventArgs e)
        {           
            combField.Items.Clear();
            // Flexible language settings 1.0.0.2 Lance
            //sSQL = " SELECT C.DEFECT_CODE,C.DEFECT_DESC,DECODE(C.DEFECT_LEVEL,'0','�ش�','1','�ش�','һ��') DEFECT_LEVEL,C.DEFECT_ID "
            sSQL = " SELECT C.DEFECT_CODE,C.DEFECT_DESC,DECODE(C.DEFECT_LEVEL,'0','Critical','1','Major','Minor') DEFECT_LEVEL,C.DEFECT_ID "
                + "FROM SAJET.SYS_RC_PROCESS_DEFECT A, "
                  + "SAJET.SYS_PROCESS B,"
                  + "SAJET.SYS_DEFECT C "
                  + "WHERE A.PROCESS_ID = B.PROCESS_ID "
                  + "AND A.PROCESS_ID = '" + s_ProcessID + "' "
                  + "AND A.DEFECT_ID = C.DEFECT_ID "
                  + "AND C.ENABLED='Y' ";
            sSQL = sSQL + " ORDER BY C.DEFECT_CODE ";
            DataSet dsSearch = ClientUtils.ExecuteSQL(sSQL);
            dgvData.DataSource = dsSearch;
            dgvData.DataMember = dsSearch.Tables[0].ToString();

            for (int i = 0; i <= dsSearch.Tables[0].Columns.Count - 1; i++)
            {
                combField.Items.Add(dsSearch.Tables[0].Columns[i].ToString());
                g_tsField.Add(dsSearch.Tables[0].Columns[i].ToString());
            }
            if (combField.Items.Count > 0)
                combField.SelectedIndex = 0;

            if (dgvData.Rows.Count > 0)
                dgvData.CurrentCell = dgvData.Rows[0].Cells[0];
            // Defect id don't show  1.0.0.2 Lance
            dgvData.Columns[dgvData.Columns.Count - 1].Visible = false;

            for (int i = 0; i < dgvData.Rows.Count; i++)
                dgvData.Rows[i].Cells["DEFECT_LEVEL"].Value = SajetCommon.SetLanguage(dgvData.Rows[i].Cells["DEFECT_LEVEL"].Value.ToString()); 

            SajetCommon.SetLanguageControl(this);
            editValue.Focus();
        }

        //�Զ�����������
        private void editValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            
            string sCondition="";
            string SQL;
            SQL = sSQL;
            if (editValue.Text == "")
            {
                SQL = SQL + "ORDER BY C.DEFECT_CODE ";
            }
            else
            {
                if (combField.Text == "��������")
                {
                    sCondition = editValue.Text;
                    SQL = SQL + "AND C.DEFECT_CODE = '" + sCondition + "' ORDER BY C.DEFECT_CODE ";
                }
                if (combField.Text == "��������")
                {
                    sCondition = editValue.Text;
                    SQL = SQL + "AND C.DEFECT_DESC = '" + sCondition + "' ORDER BY C.DEFECT_CODE ";
                }
                if (combField.Text == "�����ȼ�")
                {
                    if (editValue.Text == "�ش�")
                    { sCondition = "0"; }
                    if (editValue.Text == "�ش�")
                    { sCondition = "0"; }
                    if (editValue.Text == "һ��")
                    { sCondition = "0"; }
                    SQL = SQL + "AND C.DEFECT_LEVEL = '" + sCondition + "' ORDER BY C.DEFECT_CODE ";
                }
            }
            DataSet dsSearch = ClientUtils.ExecuteSQL(SQL);
            dgvData.DataSource = dsSearch;
            dgvData.DataMember = dsSearch.Tables[0].ToString();
            for (int i = 0; i <= dsSearch.Tables[0].Columns.Count - 1; i++)
            {
                g_tsField.Add(dsSearch.Tables[0].Columns[i].ToString());
            }
            if (combField.Items.Count > 0)
                combField.SelectedIndex = 0;

            if (dgvData.Rows.Count > 0)
                dgvData.CurrentCell = dgvData.Rows[0].Cells[0];
            SajetCommon.SetLanguageControl(this);
            editValue.Focus();     
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}