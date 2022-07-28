using RCOutput_WO.Models;
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
using Rfrns = RCOutput_WO.Services.OtherService;

namespace RCOutput_WO
{
    public partial class fAddMachine : Form
    {
        /// <summary>
        /// 是否共用機台設定
        /// </summary>
        public bool ShareMachineSettings = false;

        public bool usingT4OrT6stove = false;

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

        public fAddMachine()
        {
            #region 屬性

            InitializeComponent();

            SajetCommon.SetLanguageControl(this);

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

        private void FAddMachine_Load(object sender, EventArgs e)
        {
            RcInfo = Rfrns.GetRcNoInfo(Runcard);

            GetMachineList();

            CbSharedSettings.Checked = ShareMachineSettings;
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
                    = SajetCommon.SetLanguage("The start time of the machine must be later than the in-process time")
                    + ": "
                    + WipInTime.ToString("yyyy/ MM/ dd HH: mm: ss");

                SajetCommon.Show_Message(message, 0);

                DtpStart.Value = WipInTime;

                return;
            }

            if (DtpStart.Value > DateTime.Now)
            {
                string message
                    = SajetCommon.SetLanguage("The start time of the machine must be earlier than now");

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

            #region 有設定負載量的檢查

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
            }

            #endregion

            // Double check 再執行
            message = SajetCommon.SetLanguage("Confirm to add the machine");

            if (SajetCommon.Show_Message(message, 2) != DialogResult.Yes)
            {
                return;
            }

            DateTime now = DateTime.Now;

            DataSet related_rc = Rfrns.GetRelatedRuncard(Runcard, report_type: ShareMachineSettings ? 2 : 1);

            if (ShareMachineSettings && related_rc != null && related_rc.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in related_rc.Tables[0].Rows)
                {
                    AddMachine(row["RC_NO"].ToString(), row["TRAVEL_ID"].ToString(), row["NODE_ID"].ToString(), now);
                }
            }
            else
            {
                AddMachine(Runcard, RcInfo["TRAVEL_ID"].ToString(), RcInfo["NODE_ID"].ToString(), now);
            }

            DialogResult = DialogResult.OK;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// 新增機台。
        /// </summary>
        private void AddMachine(string rc_no, string travelID, string nodeID, DateTime now)
        {
            int i = 0;

            string s = string.Empty;

            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", rc_no },
                new object[] { ParameterDirection.Input, OracleType.Number, "TRAVEL_ID", travelID },
                new object[] { ParameterDirection.Input, OracleType.Number, "NODE_ID", nodeID },
                new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", EmpID },
                new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", now },
                new object[] { ParameterDirection.Input, OracleType.Number, "REPORT_TYPE", usingT4OrT6stove ? 2 : 1 },
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
   ,COUNT_WORKTIME
   ,UPDATE_USERID
   ,UPDATE_TIME
   ,DATA_STATUS
   ,REPORT_TYPE
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
   ,''
   ,0
   ,:UPDATE_USERID
   ,:UPDATE_TIME
   ,0
   ,:REPORT_TYPE
)
";
                    p.Add(new object[] { ParameterDirection.Input, OracleType.Number, $"MACHINE_ID{i}", x.MACHINE_ID });
                    p.Add(new object[] { ParameterDirection.Input, OracleType.DateTime, $"START_TIME{i}", DtpStart.Value });
                    p.Add(new object[] { ParameterDirection.Input, OracleType.Number, $"LOAD_QTY{i}", x.LOAD_QTY });

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
                string message = SajetCommon.SetLanguage("No option");

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

    }
}
