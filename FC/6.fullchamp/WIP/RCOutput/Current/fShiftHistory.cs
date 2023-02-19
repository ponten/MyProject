using RCOutput.Enums;
using SajetClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SfSrv = RCOutput.Services.ShiftService;

namespace RCOutput
{
    public partial class fShiftHistory : Form
    {
        private readonly DataRow rc_info;

        public fShiftHistory(DataRow RC_INFO)
        {
            InitializeComponent();

            SajetCommon.SetLanguageControl(this);

            rc_info = RC_INFO;

            this.Load += FShiftHistory_Load;

            this.Shown += FShiftHistory_Shown;
        }

        private void FShiftHistory_Load(object sender, EventArgs e)
        {
            DataRow[] d0 = SfSrv.GetShiftHistory(rc_info).Tables[0].Select($"REASON_ID='{fMain.g_iReason_ID_shift}'") ;
            var d = new DataSet();
            if (d0.Count() > 0)
                d.Tables.Add(d0[0].Table.Copy());

            if (d != null && d.Tables[0] != null)
            {
                LoadDataToDataGridView(d);

                LoadDataToLabel(rc_info, d);
            }
            else
            {
                string message = SajetCommon.SetLanguage(MessageEnum.InProcessTime.ToString());

                SajetCommon.Show_Message(message, 1);
            }
        }

        private void FShiftHistory_Shown(object sender, EventArgs e)
        {
            label1.Font = new Font("微軟正黑體", 14, FontStyle.Regular);
            label2.Font = new Font("微軟正黑體", 14, FontStyle.Regular);
            label3.Font = new Font("微軟正黑體", 14, FontStyle.Regular);
            label4.Font = new Font("微軟正黑體", 14, FontStyle.Regular);
            LbRcNo.Font = new Font("微軟正黑體", 14, FontStyle.Regular);
            LbCurrentQty.Font = new Font("微軟正黑體", 14, FontStyle.Regular);
            LbProcessedQty.Font = new Font("微軟正黑體", 14, FontStyle.Regular);
            LbRemainQty.Font = new Font("微軟正黑體", 14, FontStyle.Regular);
        }

        /// <summary>
        /// Label 顯示訊息
        /// </summary>
        /// <param name="rc_info"></param>
        /// <param name="d"></param>
        private void LoadDataToLabel(DataRow rc_info, DataSet d)
        {
            LbRcNo.Text = rc_info["RC_NO"].ToString();

            int.TryParse(rc_info["CURRENT_QTY"].ToString(), out int current_qty);

            LbCurrentQty.Text = current_qty.ToString();

            int processed_qty = 0;

            foreach (DataRow row in d.Tables[0].Rows)
            {
                int.TryParse(row["Load"].ToString(), out int load_qty);

                processed_qty += load_qty;
            }

            LbProcessedQty.Text = processed_qty.ToString();

            LbRemainQty.Text = (current_qty - processed_qty).ToString();
        }

        /// <summary>
        /// DataGridView 顯示訊息
        /// </summary>
        /// <param name="d"></param>
        private void LoadDataToDataGridView(DataSet d)
        {
            DgvData.DataSource = d;

            DgvData.DataMember = d.Tables[0].ToString();

            foreach (DataGridViewColumn column in DgvData.Columns)
            {
                if (column.Name.ToUpper() == "REASON_ID")
                    column.Visible = false;

                column.HeaderText = SajetCommon.SetLanguage(column.Name);

                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            DgvData.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }
    }
}
