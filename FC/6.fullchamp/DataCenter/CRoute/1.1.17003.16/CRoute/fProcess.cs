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
using Lassalle.Flow;


namespace CRoute
{
    public partial class fProcess : Form
    {
        string sSQL;
        int check_count = 0;
        public fProcess()
        {
            InitializeComponent();
        }

        private void fProcess_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            combTypeFilter.SelectedIndex = 0;
        }

        private void ShowData()
        {
            if (combTypeFilter.SelectedIndex == 0)
                ShowProcess();
            if (combTypeFilter.SelectedIndex == 1)
                ShowSubRoute();
            for (int i = 0; i < gvData.ColumnCount; i++)
            {
                gvData.Columns[i].ReadOnly = true;

                gvData.Columns[i].HeaderText = SajetCommon.SetLanguage(gvData.Columns[i].HeaderText);
            }

            gvData.Columns[0].ReadOnly = false;

            textFilter.Focus();
        }

        private void ShowProcess()
        {
            gvData.DataSource = null;
            sSQL = " SELECT A.PROCESS_ID, A.PROCESS_NAME, B.STAGE_NAME FROM SAJET.SYS_PROCESS A, SAJET.SYS_STAGE B WHERE A.STAGE_ID = B.STAGE_ID AND A.ENABLED = 'Y' ";

            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "VAL", textFilter.Text.ToUpper() },
            };

            if (!string.IsNullOrWhiteSpace(textFilter.Text))
            {
                if (combFilter.SelectedIndex == 0)
                {
                    sSQL += " AND A.PROCESS_NAME LIKE '%' || :VAL || '%' ";
                }
                if (combFilter.SelectedIndex == 1)
                {
                    sSQL += " AND B.STAGE_NAME LIKE '%' || :VAL || '%' ";
                }
            }

            DataSet ds = ClientUtils.ExecuteSQL(sSQL, p.ToArray());

            gvData.DataSource = ds.Tables[0];
            gvData.Columns["PROCESS_ID"].Visible = false;
        }

        private void ShowSubRoute()
        {
            gvData.DataSource = null;
            sSQL = " SELECT ROUTE_ID, ROUTE_NAME, ROUTE_DESC FROM SAJET.SYS_RC_ROUTE WHERE ENABLED = 'Y' ";

            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "VAL", textFilter.Text.ToUpper() },
            };

            if (!string.IsNullOrWhiteSpace(textFilter.Text))
            {
                if (combFilter.SelectedIndex == 0)
                {
                    sSQL += " AND ROUTE_NAME LIKE '%' || :VAL || '%' ";
                }
                if (combFilter.SelectedIndex == 1)
                {
                    sSQL += " AND ROUTE_DESC LIKE '%' || :VAL || '%' ";
                }
            }

            DataSet ds = ClientUtils.ExecuteSQL(sSQL, p.ToArray());

            gvData.DataSource = ds.Tables[0];
            gvData.Columns["ROUTE_ID"].Visible = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (gvData.SelectedRows.Count == 0)
            {
                SajetCommon.Show_Message("NO DATA!", 0);
                return;
            }
            string current_process;
            string current_process_id;
            fEdit lForm1 = (fEdit)this.Owner;
            foreach (DataGridViewRow dgvr in this.gvData.Rows)
            {
                if (dgvr.Cells[0].Value != null && (bool)dgvr.Cells[0].Value)
                {
                    if (combTypeFilter.SelectedIndex == 1)
                    {
                        current_process = dgvr.Cells[2].Value.ToString();
                        current_process_id = dgvr.Cells[1].Value.ToString();
                        lForm1.add_subroute(current_process, 100 * check_count, current_process_id);
                        check_count++;
                    }
                    if (combTypeFilter.SelectedIndex == 0)
                    {
                        current_process = dgvr.Cells[2].Value.ToString();
                        current_process_id = dgvr.Cells[1].Value.ToString();
                        if (current_process == "END")
                            current_process_id = "";
                        lForm1.add_node(current_process, 80 * check_count, current_process_id);
                        check_count++;
                    }
                }
            }
            this.Close();
        }

        private void textFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
                ShowData();
        }


        private void combTypeFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            combFilter.Items.Clear();
            ShowData();

            for (int i = 2; i < gvData.Columns.Count; i++)
            {
                gvData.Columns[i].HeaderText = SajetCommon.SetLanguage(gvData.Columns[i].HeaderText.ToString(), 1);
                combFilter.Items.Add(gvData.Columns[i].HeaderText);
            }

            if (combFilter.Items.Count > 0)
            {
                combFilter.SelectedIndex = 0;
            }
        }
    }
}
