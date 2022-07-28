using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using SajetClass;
using SajetTable;
using SajetFilter;

namespace RCProcessDefect
{
    public partial class fData : Form
    {
        private fMain fMainControl;

        public fData()
        {
            InitializeComponent();
        }
        public fData(fMain f)
        {
            InitializeComponent();
            fMainControl = f;
        }

        public string g_sUpdateType, g_sformText;
        public string g_sKeyID, g_sProcessName;
        public DataGridViewRow dataCurrentRow;
        string sSQL;
        DataSet dsTemp;
        bool bAppendSucess = false;
        int iDefect = 0;
        private void fData_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            this.BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.Text = g_sformText;
            showDefect();
            if (g_sUpdateType == "MODIFY")
            {
                g_sKeyID = dataCurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();

                editProcess.Text = dataCurrentRow.Cells["PROCESS_NAME"].Value.ToString();
                editProcess.Enabled = false;
                btnSearch.Enabled = false;

                showClick();
            }
            groupBox1.Text = iDefect.ToString();

            dgvDefect.Enabled = false;
            btnOK.Enabled = false;
            ckbSelectAll.Enabled = false;
        }

        private void showClick()
        {
            sSQL = "SELECT * FROM SAJET.SYS_PROCESS WHERE PROCESS_NAME = '" + editProcess.Text + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            if (dsTemp == null || dsTemp.Tables[0].Rows.Count <= 0)
            {
                dgvDefect.Enabled = false;
                btnOK.Enabled = false;
                ckbSelectAll.Enabled = false;
            }
            else
            {
                dgvDefect.Enabled = true;
                btnOK.Enabled = true;
                ckbSelectAll.Enabled = true;
            }
            try
            {
                sSQL = " SELECT C.DEFECT_CODE DEFECT_CODE "
                    + " FROM SAJET.SYS_RC_PROCESS_DEFECT A, SAJET.SYS_PROCESS B, SAJET.SYS_DEFECT C "
                    + " WHERE A.PROCESS_ID = B.PROCESS_ID AND A.DEFECT_ID = C.DEFECT_ID "
                    + " AND B.PROCESS_NAME = '" + editProcess.Text + "' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);
                iDefect = 0;
                for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                {
                    string sDefect = dsTemp.Tables[0].Rows[i]["DEFECT_CODE"].ToString();
                    for (int j = 0; j < dgvDefect.Rows.Count; j++)
                    {
                        if (dgvDefect.Rows[j].Cells[1].Value.ToString() == sDefect)
                        {
                            dgvDefect.Rows[j].Cells[0].Value = true;
                            iDefect++;
                        }
                    }
                }
                groupBox1.Text = iDefect.ToString();
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception : " + ex.Message, 0);
                return;
            }

        }

        private void showDefect()
        {
            try
            {
                sSQL = "SELECT DEFECT_CODE, DEFECT_DESC "
                          + " FROM SAJET.SYS_DEFECT "
                          + " WHERE ENABLED = 'Y'  ORDER BY DEFECT_CODE ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);

                for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = dsTemp.Tables[0].Rows[i];
                    dgvDefect.Rows.Add();
                    string test = dr["DEFECT_CODE"].ToString();
                    dgvDefect.Rows[dgvDefect.Rows.Count - 1].Cells["DEFECT_CODE"].Value = dr["DEFECT_CODE"].ToString();
                    dgvDefect.Rows[dgvDefect.Rows.Count - 1].Cells["DEFECT_DESC"].Value = dr["DEFECT_DESC"].ToString();
                    dgvDefect.Rows[dgvDefect.Rows.Count - 1].Cells["DEFECT_CODE"].ReadOnly = true;
                    dgvDefect.Rows[dgvDefect.Rows.Count - 1].Cells["DEFECT_DESC"].ReadOnly = true;
                }
                //gvDefect.DataSource = dsTemp;
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception : " + ex.Message, 0);
                return;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= panelControl.Controls.Count - 1; i++)
            {
                if (panelControl.Controls[i] is TextBox)
                {
                    panelControl.Controls[i].Text = panelControl.Controls[i].Text.Trim();
                }
            }
            string sData = LabCode.Text;
            if (editProcess.Text == "")
            {

                string sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + SajetCommon.SetLanguage(sData, 2);
                SajetCommon.Show_Message(sMsg, 0);
                editProcess.Focus();
                editProcess.SelectAll();
                return;
            }
            else
            {
                sSQL = "SELECT PROCESS_ID FROM SAJET.SYS_PROCESS WHERE PROCESS_NAME ='" + editProcess.Text + "' ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);

                if (dsTemp.Tables[0].Rows.Count == 0)
                {
                    string sMsg = SajetCommon.SetLanguage("Data is Error", 2) + Environment.NewLine + SajetCommon.SetLanguage(sData, 2);
                    SajetCommon.Show_Message(sMsg, 0);
                    return;
                }
            }

            //Update DB
            try
            {
                sSQL = @" DELETE FROM SAJET.SYS_RC_PROCESS_DEFECT
                                     WHERE PROCESS_ID IN
                                     (SELECT A.PROCESS_ID 
                                      FROM SAJET.SYS_RC_PROCESS_DEFECT A, SAJET.SYS_PROCESS B 
                                      WHERE A.PROCESS_ID = B.PROCESS_ID 
                                      AND B.PROCESS_NAME = '" + editProcess.Text + "') ";
                ClientUtils.ExecuteSQL(sSQL);

                for (int i = 0; i < dgvDefect.Rows.Count; i++)
                {
                    if (dgvDefect.Rows[i].Cells[0].Value == null) continue;
                    if ((bool)dgvDefect.Rows[i].Cells[0].Value)
                    {
                        sSQL = @" INSERT INTO SAJET.SYS_RC_PROCESS_DEFECT
                                      (PROCESS_ID, DEFECT_ID, UPDATE_USERID, ENABLED)
                                       SELECT B.PROCESS_ID, C.DEFECT_ID, D.EMP_ID, 'Y'  
                                       FROM SAJET.SYS_PROCESS B, SAJET.SYS_DEFECT C, SAJET.SYS_EMP D 
                                       WHERE B.PROCESS_NAME = '" + editProcess.Text + "' "
                                  + " AND C.DEFECT_CODE = '" + dgvDefect.Rows[i].Cells[1].Value + "' "
                                  + " AND D.EMP_NO = '" + ClientUtils.fLoginUser + "' ";
                        ClientUtils.ExecuteSQL(sSQL);
                    }
                }
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception : " + ex.Message, 0);
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (bAppendSucess)
                DialogResult = DialogResult.OK;
        }

        private void ClearData()
        {
            for (int i = 0; i <= panelControl.Controls.Count - 1; i++)
            {
                if (panelControl.Controls[i] is TextBox)
                {
                    panelControl.Controls[i].Text = "";
                }
                else if (panelControl.Controls[i] is ComboBox)
                {
                    ((ComboBox)panelControl.Controls[i]).SelectedIndex = -1;
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            sSQL = "select distinct A.PROCESS_NAME, A.PROCESS_DESC "
           + " from SAJET.SYS_PROCESS  A "
           + " where A.enabled = 'Y'  "
           + " and A.PROCESS_NAME LIKE '" + editProcess.Text + "%' "
           + " Order By A.PROCESS_NAME ";
            fFilter f = new fFilter();
            f.sSQL = sSQL;
            if (f.ShowDialog() == DialogResult.OK)
            {
                editProcess.Text = f.dgvData.CurrentRow.Cells["PROCESS_NAME"].Value.ToString();
                showClick();
            }
        }

        private void editProcess_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                showClick();
            }
        }

        private void ckbSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            bool bChk = false;
            if (ckbSelectAll.Checked)
            {
                bChk = true;
                groupBox1.Text = dgvDefect.Rows.Count.ToString();
            }
            else
            {
                bChk = false;
                groupBox1.Text = "0";
            }

            for (int i = 0; i < dgvDefect.Rows.Count; i++)
            {
                dgvDefect.Rows[i].Cells[0].Value = bChk;
            }
            dgvDefect.EndEdit();
        }

        private void dgvDefect_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int iClick = 0;
            dgvDefect.EndEdit();
            for (int i = 0; i < dgvDefect.Rows.Count; i++)
            {
                //if(dgvDefect.SelectedRows[0].Cells[0])
                if (dgvDefect.Rows[i].Cells[0].Value == null) continue;
                if ((bool)dgvDefect.Rows[i].Cells[0].Value)
                    iClick++;
            }

            //foreach (DataGridView row in dgvDefect.Rows)
            //{
            //       CheckBox checkBox = row as CheckBox;
            //       if (checkBox.Checked)
            //            iClick++;
            //}
            groupBox1.Text = iClick.ToString();
        }
    }
}
