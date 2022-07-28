using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;


namespace CSaleOrderInput
{
    public partial class fFilter : Form
    {
        private string m_Sql;
        private string m_OrderBy;
        public fFilter(string sql, string orderBy)
        {
            InitializeComponent();
            m_Sql = sql;
            m_OrderBy = orderBy;
        }

        DataSet dsSearch;
        List<string> combFilterField = new List<string>();

        private void dgvData_DoubleClick(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count > 0 && dgvData.CurrentRow != null)
                DialogResult = DialogResult.OK;
        }

        private void fFilter_Load(object sender, EventArgs e)
        {
            // ShowData must be executed before adding Item to ComboBox
            var aSql = m_Sql + $" ORDER BY {m_OrderBy}";
            ShowData(aSql);

            combField.Items.Clear();
            combFilterField.Clear();
            for (int i = 0; i <= dsSearch.Tables[0].Columns.Count - 1; i++)
            {
                combField.Items.Add(dsSearch.Tables[0].Columns[i].ToString());
                combFilterField.Add(dsSearch.Tables[0].Columns[i].ToString());
            }
            if (combField.Items.Count > 0)
                combField.SelectedIndex = 0;

            SajetCommon.SetLanguageControl(this);
            panel1.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
        }

        private void editValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            var sql = m_Sql;
            if (combField.SelectedIndex > -1 && editValue.Text.Trim() != "")
            {
                string sFieldName = combFilterField[combField.SelectedIndex];

                if (!string.IsNullOrEmpty(sFieldName))
                {
                    if (sql.Contains("WHERE"))
                    {
                        sql += $" AND {sFieldName} LIKE '{editValue.Text.Trim()}%'";
                    }
                    else
                    {
                        sql += " WHERE " + sFieldName + " LIKE '" + editValue.Text.Trim() + "%'";
                    }
                }
            }
            var aSql = sql + $" ORDER BY {m_OrderBy}";
            ShowData(aSql);
        }

        private void ShowData(string sql)
        {
            dsSearch = ClientUtils.ExecuteSQL(sql);

            dgvData.DataSource = dsSearch;
            dgvData.DataMember = dsSearch.Tables[0].ToString();

            if (dgvData.Rows.Count > 0)
                dgvData.CurrentCell = dgvData.Rows[0].Cells[0];

            editValue.Focus();
        }
    }
}