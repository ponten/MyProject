using RCOutput.Models;
using SajetClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SfSrv = RCOutput.Services.ShiftService;
using OtSrv = RCOutput.Services.OtherService;
using McSrv = RCOutput.Services.MachineService;
using RcSrv = RCOutput.Services.RuncardService;
using RCOutput.Enums;

namespace RCOutput
{
    public partial class fShift : Form
    {
        /// <summary>
        /// 流程卡號。
        /// </summary>
        public string Runcard;

        /// <summary>
        /// 員工 ID
        /// </summary>
        public string EmpID;

        /// <summary>
        /// 流程卡所在製程 ID。
        /// </summary>
        public string ProcessID = string.Empty;

        /// <summary>
        /// 準備移除的、使用中的機台資訊。
        /// </summary>
        public List<MachineDownModel> Machines;

        /// <summary>
        /// 流程卡資訊。
        /// </summary>
        private DataRow RcInfo;

        public fShift()
        {
            InitializeComponent();

            SajetCommon.SetLanguageControl(this);

            this.Load += FShift_Load;

            BtnSubmit.Click += BtnSubmit_Click;

            BtnCancel.Click += BtnCancel_Click;

            DgvMachine.EditingControlShowing += DgvMachine_EditingControlShowing;
        }

        private void FShift_Load(object sender, EventArgs e)
        {
            RcInfo = RcSrv.GetRcNoInfo(Runcard);

            DataSet d = McSrv.GetWorkingMachineList(Runcard);

            List<MachineDownModel> temp_machine_list = McSrv.GetModels(d);

            temp_machine_list.ForEach(x =>
            {
                x.LOAD_QTY = Machines
                .FirstOrDefault(m => m.MACHINE_ID == x.MACHINE_ID)
                .LOAD_QTY;
            });

            Machines = temp_machine_list
                .Select(x => new MachineDownModel(x))
                .ToList();

            DgvMachine.DataSource = Machines;

            DtpEnd.Value = OtSrv.GetDBDateTimeNow();
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            string message;

            int.TryParse(RcInfo["CURRENT_QTY"].ToString(), out int current_qty);

            int total_load_before = SfSrv.GetMachineTotalLoadBefore(RcInfo);

            int total_load_now = 0;

            foreach (DataGridViewRow row in DgvMachine.Rows)
            {
                var machine = row.DataBoundItem as MachineDownModel;

                if (!SfSrv.Check_End_time(
                    rc_info: RcInfo,
                    machine: machine,
                    end_time: DtpEnd.Value,
                    message: out message))
                {
                    SajetCommon.Show_Message(message, 0);

                    return;
                }

                if (!int.TryParse(machine.LOAD_QTY.ToString(), out int load_qty))
                {
                    message = SajetCommon.SetLanguage(MessageEnum.TheMachineWorkloadMustBeANonNegativeInteger.ToString());

                    SajetCommon.Show_Message(message, 3);

                    return;
                }

                total_load_now += load_qty;
            }

            if (total_load_now <= 0)
            {
                message = SajetCommon.SetLanguage(MessageEnum.TheShiftWorkloadMustBeANonNegativeInteger.ToString());

                SajetCommon.Show_Message(message, 3);

                return;
            }

            if (total_load_now > current_qty)
            {
                message
                    = SajetCommon.SetLanguage(MessageEnum.TheMachineloadExceededCurrentQuantity.ToString())
                    + " : "
                    + total_load_before;

                SajetCommon.Show_Message(message, 3);

                return;
            }

            if (total_load_now > current_qty - total_load_before)
            {
                message = SajetCommon.SetLanguage(MessageEnum.TheTotalProcessingQtyHasExceededTheCurrentQtyOfTheRuncard.ToString());

                SajetCommon.Show_Message(message, 3);

                return;
            }

            if (total_load_now == current_qty - total_load_before)
            {
                message = SajetCommon.SetLanguage(MessageEnum.TheTotalProcessingQtyIsEqualToTheCurrentQtyOfTheRuncard.ToString());

                SajetCommon.Show_Message(message, 3);

                return;
            }

            // Double check 再執行
            message = SajetCommon.SetLanguage("Confirm to remove the machine");

            if (SajetCommon.Show_Message(message, 2) != DialogResult.Yes)
            {
                return;
            }

            RemoveMachine();

            DialogResult = DialogResult.OK;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void DgvMachine_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= NumericTextBoxColumn_KeyPress;

            e.Control.Leave -= NumericTextBoxColumn_Leave;

            if (DgvMachine.CurrentCell.ColumnIndex == DgvMachine.Columns[nameof(lOADQTYDataGridViewTextBoxColumn)].Index)
            {
                if (e.Control is TextBox x)
                {
                    x.KeyPress += NumericTextBoxColumn_KeyPress;

                    x.Leave += NumericTextBoxColumn_Leave;
                }
            }

            AddUp();
        }

        /// <summary>
        /// 只能輸入正整數
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumericTextBoxColumn_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// 結束編輯狀態時的資料驗證
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumericTextBoxColumn_Leave(object sender, EventArgs e)
        {
            if (DgvMachine.CurrentCell.ColumnIndex == DgvMachine.Columns[nameof(lOADQTYDataGridViewTextBoxColumn)].Index &&
                sender is DataGridViewTextBoxEditingControl x &&
                string.IsNullOrWhiteSpace(x.Text))
            {
                x.Text = "0";
            }
        }

        /// <summary>
        /// 總計加工數量
        /// </summary>
        private void AddUp()
        {
            Lb_Total.Text = Machines.Sum(x => x.LOAD_QTY ?? 0).ToString();
        }

        /// <summary>
        /// 移出機台。
        /// </summary>
        private void RemoveMachine()
        {
            DateTime now = OtSrv.GetDBDateTimeNow();

            DateTime end_time = DtpEnd.Value;

            int i = 0;

            string s = string.Empty;

            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.DateTime, "END_TIME", end_time },
                new object[] { ParameterDirection.Input, OracleType.Number, "REASON_ID", 0 },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "REMARK", "" },
                new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", EmpID },
                new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", now },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", RcInfo["RC_NO"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "TRAVEL_ID", RcInfo["TRAVEL_ID"].ToString() },
            };

            foreach (DataGridViewRow dgv_row in DgvMachine.Rows)
            {
                var machine = (dgv_row.DataBoundItem as MachineDownModel);

                var p_sub = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.Number, $"LOAD_QTY_{i}", machine.LOAD_QTY },
                    new object[] { ParameterDirection.Input, OracleType.Number, $"MACHINE_ID_{i}", machine.MACHINE_ID },
                };

                p.AddRange(p_sub);

                s += $@"
    /* G_RC_TRAVEL_MACHINE_DOWN 記錄結束時間 */

    UPDATE SAJET.G_RC_TRAVEL_MACHINE_DOWN
    SET
        END_TIME = :END_TIME,
        REASON_ID = :REASON_ID,
        REMARK = :REMARK,
        WORK_TIME_MINUTE = NVL(ROUND((TO_NUMBER(:END_TIME - START_TIME) * 24 * 60 * 60 + 29) / 60), 0),
        WORK_TIME_SECOND = NVL(ROUND(TO_NUMBER(:END_TIME - START_TIME) * 24 * 60 * 60, 0), 0),
        UPDATE_USERID = :UPDATE_USERID,
        UPDATE_TIME = :UPDATE_TIME,
        DATA_STATUS = 0,
        LOAD_QTY = :LOAD_QTY_{i},
        DATE_CODE = '',
        STOVE_SEQ = '',
        REPORT_TYPE = 1
    WHERE
        RC_NO = :RC_NO
        AND TRAVEL_ID = :TRAVEL_ID
        AND MACHINE_ID = :MACHINE_ID_{i}
        AND END_TIME IS NULL;

    /* G_RC_TRAVEL_MACHINE 刪除該筆資料 */

    DELETE FROM SAJET.G_RC_TRAVEL_MACHINE
    WHERE
        RC_NO = :RC_NO
        AND TRAVEL_ID = :TRAVEL_ID
        AND MACHINE_ID = :MACHINE_ID_{i};
";
                i++;
            }

            if (!string.IsNullOrWhiteSpace(s))
            {
                s = $@"
BEGIN
{s}
END;
";
                ClientUtils.ExecuteSQL(s, p.ToArray());
            }
        }
    }
}
