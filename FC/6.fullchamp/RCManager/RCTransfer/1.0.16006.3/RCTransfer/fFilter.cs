using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Collections.Specialized;

namespace SajetFilter
{
    public partial class fFilter : Form
    {
        public fFilter()
        {
            InitializeComponent();
        }

        public string sSQL, sStatus;
        DataSet dsSearch;
        List<string> combFilterField = new List<string>();

        private void dgvData_DoubleClick(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count > 0 && dgvData.CurrentRow != null)
                DialogResult = DialogResult.OK;
        }

        private void fFilter_Load(object sender, EventArgs e)
        {
            showData();

            combFilter.Items.Clear();
            combFilterField.Clear();
            for (int i = 0; i <= dsSearch.Tables[0].Columns.Count - 1; i++)
            {
                combFilter.Items.Add(dsSearch.Tables[0].Columns[i].ToString());
                combFilterField.Add(dsSearch.Tables[0].Columns[i].ToString());
            }
            if (combFilter.Items.Count > 0)
                combFilter.SelectedIndex = 0;
           
            SajetCommon.SetLanguageControl(this);
            panel1.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgFilter.jpg");
        }

        private void editValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            if (sStatus == "PDLine")
            {
                sSQL = " SELECT PDLINE_ID,PDLINE_NAME "
                     + "   FROM SAJET.SYS_PDLINE WHERE ";

                if (combFilter.SelectedIndex > -1 && editValue.Text.Trim() != "")
                {
                    string sFieldName = combFilterField[combFilter.SelectedIndex].ToString();
                    sSQL = sSQL + sFieldName + " LIKE '%" + editValue.Text.Trim() + "%' AND ";
                }

                sSQL = sSQL + " ENABLED = 'Y' "
                            + " ORDER BY PDLINE_ID ";
            }
            else if (sStatus == "WorkOrder")
            {
                sSQL = " SELECT A.WORK_ORDER,A.WO_TYPE,A.TARGET_QTY-A.INPUT_QTY AS AVAILABLE_QTY,B.SPEC1,B.SPEC2,B.VERSION,C.ROUTE_NAME "
                     + "   FROM SAJET.G_WO_BASE A,SAJET.SYS_PART B,SAJET.SYS_RC_ROUTE C WHERE ";

                if (combFilter.SelectedIndex > -1 && editValue.Text.Trim() != "")
                {
                    string sFieldName = combFilterField[combFilter.SelectedIndex].ToString();

                    if (sFieldName == "AVAILABLE_QTY")
                    {
                        sSQL = sSQL + "A.TARGET_QTY - A.INPUT_QTY" + " LIKE '%" + editValue.Text.Trim() + "%' AND ";
                    }
                    else
                    {
                        sSQL = sSQL + sFieldName + " LIKE '%" + editValue.Text.Trim() + "%' AND ";
                    }
                }

                sSQL = sSQL + " A.PART_ID = B.PART_ID "
                            + " AND A.ROUTE_ID = C.ROUTE_ID "
                            + " AND A.WO_TYPE = '轉投工單' "
                            + " AND A.WO_STATUS > 1"
                            + " AND A.WO_STATUS < 4"
                            + " AND A.TARGET_QTY - A.INPUT_QTY > 0"
                            + " ORDER BY A.WORK_ORDER ";
            }

            showData();
        }

        private void showData()
        {
            dsSearch = ClientUtils.ExecuteSQL(sSQL);
            dgvData.DataSource = dsSearch;
            dgvData.DataMember = dsSearch.Tables[0].ToString();

            if (dgvData.Rows.Count > 0)
                dgvData.CurrentCell = dgvData.Rows[0].Cells[1];

            editValue.Focus();
        }
    }
}