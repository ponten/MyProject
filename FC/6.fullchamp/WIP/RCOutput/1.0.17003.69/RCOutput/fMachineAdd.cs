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
using T4Srv = RCOutput.Services.T4Service;
using RcSrv = RCOutput.Services.RuncardService;
using McSrv = RCOutput.Services.MachineService;
using SfSrv = RCOutput.Services.ShiftService;
using OtSrv = RCOutput.Services.OtherService;

namespace RCOutput
{
    public partial class fMachineAdd : Form
    {
        #region 參數

        /// <summary>
        /// 流程卡號。
        /// </summary>
        public string Runcard;

        /// <summary>
        /// 流程卡所在製程 ID。
        /// </summary>
        public string ProcessID;

        public string EmpID;

        /// <summary>
        /// 流程卡資訊。
        /// </summary>
        private DataRow RcInfo;

        /// <summary>
        /// 與製程綁定的可用機台的清單。
        /// </summary>
        private List<MachineDownModel> MachineList = new List<MachineDownModel>();

        #endregion

        public fMachineAdd()
        {
            #region 屬性

            InitializeComponent();

            SajetCommon.SetLanguageControl(this);

            // 載入爐順序號碼的 下拉選單
            sTOVESEQDataGridViewTextBoxColumn.DataSource = T4Srv.GetSequencesComboBoxItems();

            #endregion

            #region 事件

            this.Load += FAddMachine_Load;

            BtnSubmit.Click += BtnSubmit_Click;

            BtnCancel.Click += BtnCancel_Click;

            DgvMachine.CellClick += DgvMachine_CellClick;

            DgvMachine.EditingControlShowing += DgvMachine_EditingControlShowing;

            #endregion
        }

        private void FAddMachine_Load(object sender, EventArgs e)
        {
            RcInfo = RcSrv.GetRcNoInfo(Runcard);

            if (!McSrv.GetMachineList(
                RcInfo,
                out MachineList,
                out string message))
            {
                SajetCommon.Show_Message(message, 1);
            }

            var d = SfSrv.GetShiftHistory(RcInfo);

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                List<string> machine_names
                    = d.Tables[0].Rows.Cast<DataRow>()
                    .Select(x => x["Machine"].ToString())
                    .ToList();

                foreach (var machine in MachineList)
                {
                    string name = $"[{machine.MACHINE_CODE}] {machine.MACHINE_DESC}";

                    machine.Select = machine_names.Any(x => x == name);
                }
            }

            DgvMachine.DataSource = MachineList;

            DtpStart.Value = OtSrv.GetDBDateTimeNow();
        }

        /// <summary>
        /// 勾選機台時的資料處理。單選。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvMachine_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex == selectDataGridViewCheckBoxColumn.Index)
            {
                bool selected = !(bool)DgvMachine.Rows[e.RowIndex].Cells[nameof(selectDataGridViewCheckBoxColumn)].Value;

                string MachineCode = DgvMachine.Rows[e.RowIndex].Cells[nameof(mACHINECODEDataGridViewTextBoxColumn)].Value.ToString();

                // 調整為支援多選
                //MachineList.ForEach(x => x.Select = false);

                MachineList
                    .FirstOrDefault(x => x.MACHINE_CODE == MachineCode)
                    .Select = selected;

                OtSrv.RearrangeDataGridView(ref DgvMachine);
            }
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            string message;

            // 檢查有沒有勾選機台
            if (!MachineList.Any(x => x.Select))
            {
                message = SajetCommon.SetLanguage("No machine selected");

                SajetCommon.Show_Message(message, 0);

                return;
            }

            // 檢查機台是不是已經在使用
            if (IsMachineInUse(out message))
            {
                message
                    = SajetCommon.SetLanguage("Machine already in use")
                    + ": "
                    + Environment.NewLine
                    + message;

                SajetCommon.Show_Message(message, 0);

                return;
            }

            #region 有設定負載量、日期碼的檢查

            foreach (var machine in MachineList.Where(x => x.Select))
            {
                if (machine.LOAD_QTY < 0)
                {
                    message
                        = SajetCommon.SetLanguage("The load of the machine must be a non-negative number")
                        ;
                    SajetCommon.Show_Message(message, 0);

                    return;
                }

                string date_code = machine.DATE_CODE?.Trim();

                if (!string.IsNullOrEmpty(date_code))
                {
                    if (!(date_code.Length == 6 || date_code.Length == 8) ||
                      !int.TryParse(date_code, out int dateCode))
                    {
                        message
                            = SajetCommon.SetLanguage("Invalid DateCode format")
                            + Environment.NewLine
                            + SajetCommon.SetLanguage("Require 6 or 8 digits number")
                            ;
                        SajetCommon.Show_Message(message, 0);

                        return;
                    }
                }
            }

            #endregion

            #region 檢查時間設定

            if (!CheckStartTime(out message))
            {
                SajetCommon.Show_Message(message, 0);

                return;
            }

            #endregion

            // Double check 再執行
            message = SajetCommon.SetLanguage("Confirm to add the machine");

            if (SajetCommon.Show_Message(message, 2) != DialogResult.Yes)
            {
                return;
            }

            AddMachine();

            DialogResult = DialogResult.OK;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// 新增機台。
        /// </summary>
        private void AddMachine()
        {
            int i = 0;

            string s = string.Empty;

            DateTime now = OtSrv.GetDBDateTimeNow();

            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", Runcard },
                new object[] { ParameterDirection.Input, OracleType.Number, "TRAVEL_ID", RcInfo["TRAVEL_ID"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.Number, "NODE_ID", RcInfo["NODE_ID"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", EmpID },
                new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", now }
            };

            MachineList
                .Where(x => x.Select)
                .ToList()
                .ForEach(x =>
                {
                    s += $@"
INTO
    SAJET.G_RC_TRAVEL_MACHINE
(
    RC_NO
   ,TRAVEL_ID
   ,MACHINE_ID
   ,START_TIME
   ,LOAD_PORT
   ,UPDATE_USERID
   ,UPDATE_TIME
)
VALUES
(
    :RC_NO
   ,:TRAVEL_ID
   ,:MACHINE_ID{i}
   ,:START_TIME{i}
   ,0
   ,:UPDATE_USERID
   ,:UPDATE_TIME
)

INTO
    SAJET.G_RC_TRAVEL_MACHINE_DOWN
(
    RC_NO
   ,TRAVEL_ID
   ,NODE_ID
   ,MACHINE_ID
   ,START_TIME
   ,REASON_ID
   ,LOAD_QTY
   ,DATE_CODE
   ,STOVE_SEQ
   ,COUNT_WORKTIME
   ,UPDATE_USERID
   ,UPDATE_TIME
   ,DATA_STATUS
)
VALUES
(
    :RC_NO
   ,:TRAVEL_ID
   ,:NODE_ID
   ,:MACHINE_ID{i}
   ,:START_TIME{i}
   ,0
   ,:LOAD_QTY{i}
   ,:DATE_CODE{i}
   ,:STOVE_SEQ{i}
   ,0
   ,:UPDATE_USERID
   ,:UPDATE_TIME
   ,0
)
";
                    p.Add(new object[] { ParameterDirection.Input, OracleType.Number, $"MACHINE_ID{i}", x.MACHINE_ID });
                    p.Add(new object[] { ParameterDirection.Input, OracleType.DateTime, $"START_TIME{i}", DtpStart.Value });
                    p.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, $"LOAD_QTY{i}", x.LOAD_QTY });
                    p.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, $"DATE_CODE{i}", x.DATE_CODE });
                    p.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, $"STOVE_SEQ{i}", x.STOVE_SEQ });

                    i++;
                });

            s = $@"
INSERT ALL
{s}
SELECT 1 FROM DUAL
";
            ClientUtils.ExecuteSQL(s, p.ToArray());
        }

        /// <summary>
        /// 檢查選取的機台是不是已經在使用中。
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool IsMachineInUse(out string message)
        {
            string s = @"
SELECT
    TRIM(SM.MACHINE_CODE) MACHINE_CODE
FROM
    SAJET.G_RC_STATUS GRS
   ,SAJET.G_RC_TRAVEL_MACHINE GRTM
   ,SAJET.SYS_MACHINE SM
WHERE
    GRS.RC_NO = :RC_NO
AND GRS.RC_NO = GRTM.RC_NO
AND GRS.TRAVEL_ID = GRTM.TRAVEL_ID
AND GRTM.MACHINE_ID = SM.MACHINE_ID
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", Runcard },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            var UsedMachineList = new List<string>();

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in d.Tables[0].Rows)
                {
                    UsedMachineList.Add(row["MACHINE_CODE"].ToString());
                }

                var result = MachineList
                     .Where(x => x.Select && UsedMachineList.Contains(x.MACHINE_CODE))
                     .Select(x => $"[{x.MACHINE_CODE}] {x.MACHINE_DESC}")
                     .ToList();

                message = string.Join(",", result);

                return result.Any();
            }
            else
            {
                message = string.Empty;

                return false;
            }
        }

        /// <summary>
        /// 檢查加工時間的設定
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool CheckStartTime(out string message)
        {
            message = string.Empty;

            var now = OtSrv.GetDBDateTimeNow();

            if (!DateTime.TryParse(RcInfo["WIP_IN_TIME"].ToString(), out DateTime WipInTime))
            {
                WipInTime = DateTime.Parse(RcInfo["UPDATE_TIME"].ToString());
            }

            if (DtpStart.Value < WipInTime)
            {
                message
                    = SajetCommon.SetLanguage("The start time of the machine must be later than the wip-in time")
                    + ": "
                    + WipInTime.ToString("yyyy/ MM/ dd HH: mm: ss");

                DtpStart.Value = WipInTime;

                return false;
            }

            if (DtpStart.Value > now)
            {
                message
                    = SajetCommon.SetLanguage("The start time of the machine must be earlier than now");

                DtpStart.Value = now;

                return false;
            }

            return true;
        }

        #region 在編輯 LOAD_QTY 欄位或是 DATE_CODE 欄位的內容時，為了能檢查輸入值內容綁定的事件

        /// <summary>
        /// 觸發輸入值檢查事件的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvMachine_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= DgvMachineTextBoxColumn_KeyPress;

            e.Control.Leave -= DgvMachineTextBoxColumn_Leave;

            if (DgvMachine.CurrentCell.ColumnIndex == dATECODEDataGridViewTextBoxColumn.Index ||
                DgvMachine.CurrentCell.ColumnIndex == lOADQTYDataGridViewTextBoxColumn.Index)
            {
                if (e.Control is TextBox x)
                {
                    x.KeyPress += DgvMachineTextBoxColumn_KeyPress;

                    x.Leave += DgvMachineTextBoxColumn_Leave;
                }
            }
        }

        /// <summary>
        /// 只能輸入正整數
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvMachineTextBoxColumn_KeyPress(object sender, KeyPressEventArgs e)
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
        private void DgvMachineTextBoxColumn_Leave(object sender, EventArgs e)
        {
            if (DgvMachine.CurrentCell.ColumnIndex == lOADQTYDataGridViewTextBoxColumn.Index &&
                sender is DataGridViewTextBoxEditingControl x &&
                string.IsNullOrWhiteSpace(x.Text))
            {
                x.Text = "0";
            }
        }

        #endregion
    }
}
