using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RCProcessParam.Models;
using OpSrv = RCProcessParam.Services.PartOptionService;

namespace RCProcessParam
{
    public partial class fOption : Form
    {
        public ProcessViewModel this_process;

        public fOption()
        {
            InitializeComponent();

            SajetClass.SajetCommon.SetLanguageControl(this);

            Tb_Worktime.KeyPress += NumericTextBox_KeyPress;

            Btn_OK.Click += Btn_OK_Click;

            Btn_Cancel.Click += Btn_Cancel_Click;
        }

        /// <summary>
        /// 只能輸入正整數
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumericTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void Btn_OK_Click(object sender, EventArgs e)
        {
            string s_worktime = Tb_Worktime.Text.Trim();

            if (string.IsNullOrWhiteSpace(s_worktime))
            {
                s_worktime = "0";
            }

            var model = new PartOptionModel
            {
                part_id = this_process.PART_ID,
                route_id = this_process.ROUTE_ID,
                node_id = this_process.NODE_ID,
                process_id = this_process.PROCESS_ID,
                option1 = s_worktime,
                update_userid = ClientUtils.UserPara1,
                update_time = ClientUtils.GetSysDate()
            };

            OpSrv.PutData(optionModel: model);

            DialogResult = DialogResult.OK;
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

      
    }
}
