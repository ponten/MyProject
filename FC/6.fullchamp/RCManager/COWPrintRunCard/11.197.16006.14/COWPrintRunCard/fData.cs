using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SajetClass;
namespace COWPrintRunCard
{
    public partial class fData : Form
    {
        public fData()
        {
            InitializeComponent();
        }
        public string WO = "";
        public string WOType = "";
        public string PartNo = "";
        public string Version = "";
        private void txtWO_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter)
                return;
            Query();
        }
        void Query()
        {
            try
            {

                string cmd = @"select wb.work_order,p.part_no,p.version,wb.wo_type,wb.target_qty,wb.wo_create_date,wb.master_wo 
                        from sajet.g_wo_base wb,sajet.sys_part p where wb.part_id=p.part_id and wb.wo_option10='1' and wb.wo_status in (1,2) and wb.work_order like'" + txtWO.Text.Trim() + "%'  order by wb.work_order";
                DataTable dt = ClientUtils.ExecuteSQL(cmd).Tables[0];
                dgv.DataSource = dt;
                SajetCommon.SetLanguageControl(this);
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);
            }
            
        }

        private void fData_Load(object sender, EventArgs e)
        {
            txtWO.Text = this.WO;
            Query();
        }

        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv.CurrentCell == null || dgv.CurrentCell.RowIndex < 0)
                return;
            this.WO = dgv.Rows[dgv.CurrentCell.RowIndex].Cells["work_order"].Value.ToString();
            this.WOType = dgv.Rows[dgv.CurrentCell.RowIndex].Cells["wo_type"].Value.ToString();
            this.PartNo = dgv.Rows[dgv.CurrentCell.RowIndex].Cells["part_no"].Value.ToString();
            this.Version = dgv.Rows[dgv.CurrentCell.RowIndex].Cells["version"].Value.ToString();
            this.DialogResult = DialogResult.OK;
        }
    }
}
