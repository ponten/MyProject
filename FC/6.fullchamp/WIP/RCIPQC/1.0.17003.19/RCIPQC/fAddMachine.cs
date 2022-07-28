using RCIPQC.References;
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
using RCIPQC.Enums;
using OtSrv = RCIPQC.Services.OtherService;

namespace RCIPQC
{
    public partial class fAddMachine : Form
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
        private readonly List<MachineDownModel> MachineList = new List<MachineDownModel>();

        #endregion

        public fAddMachine()
        {
            #region 屬性

            InitializeComponent();

            SajetCommon.SetLanguageControl(this);

            // 載入爐順序號碼的 下拉選單
            sTOVESEQDataGridViewTextBoxColumn.DataSource = OtSrv.GetSequencesComboBoxItems();

            #endregion

            #region 事件

            this.Load += FAddMachine_Load;

            BtnSubmit.Click += BtnSubmit_Click;

            BtnCancel.Click += BtnCancel_Click;

            DgvMachine.CellClick += DgvMachine_CellClick;

            DgvMachine.EditingControlShowing += DgvMachine_EditingControlShowing;

            DtpStart.ValueChanged += DtpStart_ValueChanged;

            #endregion
        }

        #region 事件

        private void FAddMachine_Load(object sender, EventArgs e)
        {
            RcInfo = OtSrv.GetRcNoInfo(Runcard);

            GetMachineList();
        }

        /// <summary>
        /// 勾選機台時的資料處理。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvMachine_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex == selectDataGridViewCheckBoxColumn.Index)
            {
                bool selected = !(bool)DgvMachine.Rows[e.RowIndex].Cells[nameof(selectDataGridViewCheckBoxColumn)].Value;

                string MachineCode = DgvMachine.Rows[e.RowIndex].Cells[nameof(mACHINECODEDataGridViewTextBoxColumn)].Value.ToString();

                DgvMachine.Rows[e.RowIndex].Cells[nameof(selectDataGridViewCheckBoxColumn)].Value = selected;

                MachineList
                    .FirstOrDefault(x => x.MACHINE_CODE == MachineCode)
                    .Select = selected;
            }
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

        /// <summary>
        /// DateTimePicker 選好日期後把資料更新。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DtpStart_ValueChanged(object sender, EventArgs e)
        {
            if (!DateTime.TryParse(RcInfo["WIP_IN_TIME"].ToString(), out DateTime WipInTime))
            {
                WipInTime = DateTime.Parse(RcInfo["UPDATE_TIME"].ToString());
            }

            if (DtpStart.Value < WipInTime)
            {
                string message
                    = SajetCommon.SetLanguage(MessageEnum.TheStartTimeOfTheMachineMustBeLaterThanTheInProcessTime.ToString())
                    + ": "
                    + WipInTime.ToString("yyyy/ MM/ dd HH: mm: ss");

                SajetCommon.Show_Message(message, 0);

                DtpStart.Value = WipInTime;

                return;
            }

            if (DtpStart.Value > DateTime.Now)
            {
                string message
                    = SajetCommon.SetLanguage(MessageEnum.TheStartTimeOfTheMachineMustBeEarlierThanNow.ToString());

                SajetCommon.Show_Message(message, 0);

                DtpStart.Value = DateTime.Now;

                return;
            }
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            string message;

            // 檢查有沒有勾選機台
            if (!MachineList.Any(x => x.Select))
            {
                message = SajetCommon.SetLanguage(MessageEnum.NoMachineSelected.ToString());

                SajetCommon.Show_Message(message, 0);

                return;
            }

            // 檢查機台是不是已經在使用
            if (IsMachineInUse(out message))
            {
                message
                    = SajetCommon.SetLanguage(MessageEnum.MachineAlreadyInUse.ToString())
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
                        = SajetCommon.SetLanguage(MessageEnum.TheLoadOfTheMachineMustBeANonNegativeNumber.ToString())
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
                            = SajetCommon.SetLanguage(MessageEnum.InvalidDateCodeFormat.ToString())
                            + Environment.NewLine
                            + SajetCommon.SetLanguage(MessageEnum.Require6or8DigitsDatetimeFormat.ToString())
                            ;
                        SajetCommon.Show_Message(message, 0);

                        return;
                    }
                }
            }

            #endregion

            // Double check 再執行
            message = SajetCommon.SetLanguage(MessageEnum.ConfirmAddMAchine.ToString());

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

        #endregion

        #region 方法

        /// <summary>
        /// 新增機台。
        /// </summary>
        private void AddMachine()
        {
            int i = 0;

            string s = string.Empty;

            DateTime now = DateTime.Now;

            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", Runcard },
                new object[] { ParameterDirection.Input, OracleType.Number, "TRAVEL_ID", RcInfo["TRAVEL_ID"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.Number, "NODE_ID", RcInfo["NODE_ID"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", EmpID },
                new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", DateTime.Now }
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
        /// 載入可用的製程機台清單。
        /// </summary>
        /// <param name="processID"></param>
        private void GetMachineList()
        {
            string s = @"
SELECT
    B.MACHINE_ID
   ,B.MACHINE_CODE
   ,B.MACHINE_DESC
   ,D.STATUS_NAME
   ,D.RUN_FLAG
   ,D.DEFAULT_STATUS
FROM
    SAJET.SYS_RC_PROCESS_MACHINE A
   ,SAJET.SYS_MACHINE B
   ,SAJET.G_MACHINE_STATUS C
   ,SAJET.SYS_MACHINE_STATUS D
WHERE
    A.PROCESS_ID = :PROCESS_ID
AND A.MACHINE_ID = B.MACHINE_ID
AND A.MACHINE_ID = C.MACHINE_ID
AND C.CURRENT_STATUS_ID = D.STATUS_ID
AND A.ENABLED = 'Y'
AND B.ENABLED = 'Y'
AND B.MACHINE_ID NOT IN
(
    SELECT
       GRTM.MACHINE_ID
    FROM
        SAJET.G_RC_STATUS GRS
       ,SAJET.G_RC_TRAVEL_MACHINE GRTM
       ,SAJET.SYS_MACHINE SM
    WHERE
        GRS.RC_NO = :RC_NO
    AND GRS.RC_NO = GRTM.RC_NO
    AND GRS.TRAVEL_ID = GRTM.TRAVEL_ID
    AND GRTM.MACHINE_ID = SM.MACHINE_ID
)
ORDER BY
    MACHINE_CODE
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", Runcard },
                new object[] { ParameterDirection.Input, OracleType.Number, "PROCESS_ID", ProcessID },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in d.Tables[0].Rows)
                {
                    MachineList.Add(new MachineDownModel
                    {
                        MACHINE_ID = int.Parse(row["MACHINE_ID"].ToString()),
                        MACHINE_CODE = row["MACHINE_CODE"].ToString(),
                        MACHINE_DESC = row["MACHINE_DESC"].ToString(),
                        STATUS_NAME = row["STATUS_NAME"].ToString(),
                        LOAD_QTY = 0,
                        DATE_CODE = string.Empty
                    });
                }

                DgvMachine.DataSource = MachineList;
            }
            else
            {
                string message = SajetCommon.SetLanguage(MessageEnum.NoOption.ToString());

                SajetCommon.Show_Message(message, 1);
            }
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
    SM.MACHINE_CODE
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

        #endregion
    }
}
