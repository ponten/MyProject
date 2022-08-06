using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;

namespace RCSplit
{
    public partial class fComponent : Form
    {
        private fMain fMainControl;
        public fComponent()
        {
            InitializeComponent();
        }
        public fComponent(fMain f)
        {
            InitializeComponent();
            fMainControl = f;
        }
        public string sSQL;
        public int iQty, iRowIndex, iGvRowCount;
        public List<string> listSerialNumbers = new List<string>();

        private void fComponent_Load(object sender, EventArgs e)
        {
            DataSet dsComponent = ClientUtils.ExecuteSQL(sSQL);
            dgvData.DataSource = dsComponent;
            dgvData.DataMember = dsComponent.Tables[0].ToString();

            DataGridViewCheckBoxColumn cbCol = new DataGridViewCheckBoxColumn();
            cbCol.Width = 45;
            cbCol.Name = "SELECT";
            cbCol.HeaderText = "";
            cbCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvData.Columns.Insert(0, cbCol);

            if (fMainControl.dictComponent.Count > 0)
            {
                dgvData.CurrentCell = null;
                List<string> listOthers = new List<string>();
                for (int i = 0; i <= iGvRowCount - 1; i++)
                {
                    if (i == iRowIndex || !fMainControl.dictComponent.ContainsKey(i)) continue;
                    listOthers.AddRange(fMainControl.dictComponent[i]);
                }

                if (fMainControl.dictComponent.ContainsKey(iRowIndex))
                    listSerialNumbers = fMainControl.dictComponent[iRowIndex];

                foreach (DataGridViewRow gvRow in dgvData.Rows)
                {
                    if (listSerialNumbers.Contains(gvRow.Cells["SERIAL_NUMBER"].Value.ToString()))
                        gvRow.Cells[0].Value = true;

                    if (listOthers.Contains(gvRow.Cells["SERIAL_NUMBER"].Value.ToString()))
                        gvRow.Visible = false;
                }
            }
            SajetCommon.SetLanguageControl(this);
        }

        private void btnAuto_Click(object sender, EventArgs e)
        {
            int iCount = 0;
            for (int i = 0; i <= dgvData.Rows.Count - 1; i++)
            {
                if (iCount < iQty)
                {
                    if (dgvData.Rows[i].Visible == true)
                    {
                        dgvData.Rows[i].Cells[0].Value = true;
                        iCount++;
                    }
                }
                else
                {
                    if (dgvData.Rows[i].Visible == true)
                    {
                        dgvData.Rows[i].Cells[0].Value = false;
                    }
                }
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= dgvData.Rows.Count - 1; i++)
            {
                if (dgvData.Rows[i].Visible == true)
                    dgvData.Rows[i].Cells[0].Value = true;
            }
        }

        private void btnSelectNone_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= dgvData.Rows.Count - 1; i++)
            {
                if (dgvData.Rows[i].Visible == true)
                dgvData.Rows[i].Cells[0].Value = false;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            listSerialNumbers.Clear();
            for (int i = 0; i <= dgvData.Rows.Count - 1; i++)
            {
                if (dgvData.Rows[i].Cells[0].Value != null)
                {
                    if (dgvData.Rows[i].Visible && (bool)dgvData.Rows[i].Cells[0].Value)
                    {
                        listSerialNumbers.Add(dgvData.Rows[i].Cells["SERIAL_NUMBER"].Value.ToString());
                    }
                }
            }
            DialogResult = DialogResult.OK;
        }
    }
}
