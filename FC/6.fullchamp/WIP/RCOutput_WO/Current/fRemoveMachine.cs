using RCOutput_WO.Services;
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
    public partial class fRemoveMachine : Form
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

        public string EmpID;

        /// <summary>
        /// 準備移除的、使用中的機台資訊。
        /// </summary>
        public MachineDownModel Machine;

        /// <summary>
        /// 流程卡資訊。
        /// </summary>
        private DataRow RcInfo;

        /// <summary>
        /// 機台停機代碼的清單。依照異常類型分類。
        /// </summary>
        private readonly List<DownType> MachineDownTypes = new List<DownType>();

        public fRemoveMachine()
        {
            #region 屬性

            InitializeComponent();

            // Sajet 翻譯會導致下拉選單的 ComboBoxItem 被覆寫成字串
            // 表單元件套用完翻譯，再載入下拉選單的選項
            SajetCommon.SetLanguageControl(this);

            // 綁定 ComboBox 資料來源
            CbType.DisplayMember = nameof(ComboBoxItem.Text);

            CbType.ValueMember = nameof(ComboBoxItem.Value);

            CbDetail.DisplayMember = nameof(ComboBoxItem.Text);

            CbDetail.ValueMember = nameof(ComboBoxItem.Value);

            #endregion

            #region 事件

            this.Load += FRemoveMachine_Load;

            CbType.DrawItem += CbType_DrawItem;

            CbDetail.DrawItem += CbDetail_DrawItem;

            CbType.SelectedIndexChanged += CbType_SelectedIndexChanged;

            CbDetail.SelectedIndexChanged += CbDetail_SelectedIndexChanged;

            DtpEnd.ValueChanged += DtpEndTime_ValueChanged;

            TbRemark.Leave += TbRemark_Leave;

            BtnSubmit.Click += BtnSubmit_Click;

            BtnCancel.Click += BtnCancel_Click;

            #endregion
        }

        private void FRemoveMachine_Load(object sender, EventArgs e)
        {
            RcInfo = Rfrns.GetRcNoInfo(Runcard);

            GetMachineDownCodes();

            GetDownTypes();

            LbInfo.Text = Machine.MACHINE_DESC;

            TbLoad.Text = Machine.LOAD_QTY.ToString();

            CbSharedSettings.Checked = ShareMachineSettings;
        }

        /// <summary>
        /// 繪製 ComboBox 外觀。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbType_DrawItem(object sender, DrawItemEventArgs e)
        {
            int index = e.Index >= 0 ? e.Index : 0;

            var brush = Brushes.Black;

            e.DrawBackground();

            e.Graphics.DrawString(CbType.Items[index].ToString(), e.Font, brush, e.Bounds, StringFormat.GenericDefault);

            e.DrawFocusRectangle();
        }

        /// <summary>
        /// 繪製 ComboBox 外觀。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbDetail_DrawItem(object sender, DrawItemEventArgs e)
        {
            int index = e.Index >= 0 ? e.Index : 0;

            var brush = Brushes.Black;

            e.DrawBackground();

            e.Graphics.DrawString(CbDetail.Items[index].ToString(), e.Font, brush, e.Bounds, StringFormat.GenericDefault);

            e.DrawFocusRectangle();
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            string message;

            // 移出機台必須選擇停機代碼
            if (Machine.REASON_ID == 0)
            {
                message = SajetCommon.SetLanguage("Machine down code is empty");

                SajetCommon.Show_Message(message, 0);

                return;
            }

            #region 製程設定為 "會使用到 T4 爐 / T6 爐" 的製程，要做的檢查

            // 不是該類型的製程，但有記錄內容，也會檢查
            if (!string.IsNullOrWhiteSpace(TbLoad.Text))
            {
                if (!int.TryParse(TbLoad.Text, out int load_qty) || load_qty < 0)
                {
                    message = SajetCommon.SetLanguage("The load of the machine must be a non-negative number");

                    SajetCommon.Show_Message(message, 0);

                    return;
                }
            }

            #endregion

            /*
            // 移出機台必須留下備註
            if (string.IsNullOrWhiteSpace(Machine.REMARK))
            {
                message = SajetClass.SajetCommon.SetLanguage("Remark is empty");

                SajetClass.SajetCommon.Show_Message(message, 0);

                return;
            }
            //*/

            // Double check 再執行
            message = SajetCommon.SetLanguage("Confirm to remove the machine");

            if (SajetCommon.Show_Message(message, 2) != DialogResult.Yes)
            {
                return;
            }

            DateTime now = DateTime.Now;

            DataSet related_rc = Rfrns.GetRelatedRuncard(Runcard, report_type: ShareMachineSettings ? 2 : 1);

            if (ShareMachineSettings && related_rc != null && related_rc.Tables[0].Rows.Count > 0)
            {
                foreach(DataRow row in related_rc.Tables[0].Rows)
                {
                    RemoveMachine(row["RC_NO"].ToString(), row["TRAVEL_ID"].ToString(), now);
                }
            }
            else
            {
                RemoveMachine(Runcard, RcInfo["TRAVEL_ID"].ToString(), now);
            }

            DialogResult = DialogResult.OK;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// 帶入與選擇的停機代碼類型對應的停機代碼到第二階 ComboBox。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var items = MachineDownTypes
                .FirstOrDefault(x =>
                {
                    if (CbType.SelectedItem is ComboBoxItem item)
                    {
                        return x.ID == item.Value;
                    }
                    else
                    {
                        return x.ID == "0";
                    }
                })
                .DownDetails
                .Select(x => new ComboBoxItem(text: x.DESC, value: x.ID))
                .ToArray();

            CbDetail.DataSource = items;
        }

        /// <summary>
        /// 把選擇的停機代碼帶回表格。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Machine != null && CbType.SelectedItem != null && CbDetail.SelectedItem != null)
            {
                Machine.TYPE_ID = int.Parse(((ComboBoxItem)CbType.SelectedItem).Value);

                Machine.REASON_ID = int.Parse(((ComboBoxItem)CbDetail.SelectedItem).Value);

                CbCountWorkTime.Checked =
                    MachineDownTypes
                    .First(x => x.ID == Machine.TYPE_ID.ToString())
                    .DownDetails
                    .First(x => x.ID == Machine.REASON_ID.ToString())
                    .CountWorkTime;
            }
        }

        /// <summary>
        /// DateTimePicker 選好日期後把資料更新。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DtpEndTime_ValueChanged(object sender, EventArgs e)
        {
            if (!DateTime.TryParse(RcInfo["WIP_IN_TIME"].ToString(), out DateTime WipInTime))
            {
                WipInTime = DateTime.Parse(RcInfo["UPDATE_TIME"].ToString());
            }

            string s = @"
SELECT
    START_TIME
FROM
    SAJET.G_RC_TRAVEL_MACHINE_DOWN
WHERE
    RC_NO = :RC_NO
AND TRAVEL_ID = :TRAVEL_ID
AND NODE_ID = :NODE_ID
AND MACHINE_ID = :MACHINE_ID
AND END_TIME IS NULL
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", Runcard },
                new object[] { ParameterDirection.Input, OracleType.Number, "TRAVEL_ID", RcInfo["TRAVEL_ID"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.Number, "NODE_ID", RcInfo["NODE_ID"].ToString() },
                new object[] { ParameterDirection.Input, OracleType.Number, "MACHINE_ID", Machine.MACHINE_ID },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                WipInTime = DateTime.Parse(d.Tables[0].Rows[0]["START_TIME"].ToString());
            }

            if (DtpEnd.Value < WipInTime)
            {
                string message
                    = SajetCommon.SetLanguage("The end time of the machine must be later than the start time")
                    + ": "
                    + WipInTime.ToString("yyyy/ MM/ dd HH: mm: ss");

                SajetCommon.Show_Message(message, 0);

                DtpEnd.Value = WipInTime;

                return;
            }

            if (DtpEnd.Value > DateTime.Now)
            {
                string message
                    = SajetCommon.SetLanguage("The end time of the machine must be earlier than now");

                SajetCommon.Show_Message(message, 0);

                DtpEnd.Value = DateTime.Now;

                return;
            }

            Machine.START_TIME = WipInTime;

            Machine.END_TIME = DtpEnd.Value;
        }

        /// <summary>
        /// TextBox 結束編輯時把資料更新。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbRemark_Leave(object sender, EventArgs e)
        {
            Machine.REMARK = TbRemark.Text;
        }

        /// <summary>
        /// 移出機台。
        /// </summary>
        private void RemoveMachine(string rc_no, string travelID, DateTime now)
        {
            string s = string.Empty;

            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.DateTime, "end_time", Machine.END_TIME },
                new object[] { ParameterDirection.Input, OracleType.Number, "reason_id", Machine.REASON_ID },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "remark", Machine.REMARK },
                new object[] { ParameterDirection.Input, OracleType.Number, "load_qty", TbLoad.Text },
                new object[] { ParameterDirection.Input, OracleType.Number, "update_userid", EmpID },
                new object[] { ParameterDirection.Input, OracleType.DateTime, "update_time", now },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "rc_no", rc_no },
                new object[] { ParameterDirection.Input, OracleType.Number, "travel_id", travelID },
                new object[] { ParameterDirection.Input, OracleType.Number, "machine_id", Machine.MACHINE_ID },
                new object[] { ParameterDirection.Input, OracleType.Number, "report_type", usingT4OrT6stove ? 2 : 1 },
            };

            // G_RC_TRAVEL_MACHINE_DOWN 記錄結束時間
            s = @"
UPDATE sajet.g_rc_travel_machine_down
SET
    end_time = :end_time,
    reason_id = :reason_id,
    remark = :remark,
    work_time_minute = nvl(round((to_number(:end_time - start_time) * 24 * 60 * 60 + 29) / 60), 0),
    work_time_second = nvl(round(to_number(:end_time - start_time) * 24 * 60 * 60, 0), 0),
    update_userid = :update_userid,
    update_time = :update_time,
    data_status = 0,
    load_qty = :load_qty,
    date_code = '',
    stove_seq = '',
    report_type = :report_type
WHERE
    rc_no = :rc_no
    AND travel_id = :travel_id
    AND machine_id = :machine_id
    AND end_time IS NULL
;";
            // 不記錄工時的機台，記錄到排除工時紀錄表
            if (!CbCountWorkTime.Checked)
            {
                s += @"
INSERT INTO sajet.g_machine_down_exclude (
    rc_no,
    node_id,
    travel_id,
    machine_id,
    start_time,
    end_time,
    reason_id,
    load_qty,
    work_time_minute,
    work_time_second,
    remark,
    update_userid,
    update_time,
    data_status,
    report_type,
    count_worktime,
    date_code,
    stove_seq
)
    SELECT
        rc_no,
        node_id,
        travel_id,
        machine_id,
        start_time,
        end_time,
        reason_id,
        load_qty,
        work_time_minute,
        work_time_second,
        remark,
        update_userid,
        update_time,
        data_status,
        report_type,
        count_worktime,
        date_code,
        stove_seq
    FROM
        sajet.g_rc_travel_machine_down
    WHERE
        rc_no = :rc_no
        AND travel_id = :travel_id
        AND machine_id = :machine_id
        AND end_time = :end_time
        AND update_time = :update_time
;";
            }

            // G_RC_TRAVEL_MACHINE 刪除該筆資料
            s += @"
DELETE FROM sajet.g_rc_travel_machine
WHERE
    rc_no = :rc_no
    AND travel_id = :travel_id
    AND machine_id = :machine_id
;";

            s = $@"
BEGIN
{s}
END;
";
            ClientUtils.ExecuteSQL(s, p.ToArray());
        }

        /// <summary>
        /// 載入停機代碼清單。
        /// </summary>
        private void GetMachineDownCodes()
        {
            string s = @"
SELECT
    TYPE_ID
   ,TYPE_CODE
   ,TYPE_DESC
FROM
    SAJET.SYS_MACHINE_DOWN_TYPE
WHERE
    ENABLED = 'Y'
ORDER BY
    TYPE_CODE
";
            var downTypes = ClientUtils.ExecuteSQL(s);

            MachineDownTypes.Add(new DownType
            {
                ID = "0",
                CODE = "",
                DESC = "",
                DownDetails = new List<DownDetail>
                {
                    new DownDetail
                    {
                        ID = "0",
                        CODE = "",
                        DESC = "",
                        CountWorkTime = false
                    }
                }
            });

            if (downTypes != null)
            {
                foreach (DataRow row in downTypes.Tables[0].Rows)
                {
                    MachineDownTypes.Add(new DownType
                    {
                        ID = row["TYPE_ID"].ToString(),
                        CODE = row["TYPE_CODE"].ToString(),
                        DESC = $"[{row["TYPE_CODE"]}] {row["TYPE_DESC"]}",
                        DownDetails = new List<DownDetail>(),
                    });
                }
            }

            s = @"
SELECT
    TYPE_ID
   ,REASON_ID
   ,REASON_CODE
   ,REASON_DESC
   ,COUNT_WORKTIME
FROM
    SAJET.SYS_MACHINE_DOWN_DETAIL
WHERE
    ENABLED = 'Y'
ORDER BY
    TYPE_ID
   ,REASON_CODE
";
            var downDetails = ClientUtils.ExecuteSQL(s);

            if (downDetails != null)
            {
                foreach (DataRow row in downDetails.Tables[0].Rows)
                {
                    MachineDownTypes
                        .FirstOrDefault(e => e.ID == row["TYPE_ID"].ToString())
                        .DownDetails.Add(new DownDetail
                        {
                            ID = row["REASON_ID"].ToString(),
                            CODE = row["REASON_CODE"].ToString(),
                            DESC = $"[{row["REASON_CODE"]}] {row["REASON_DESC"]}",
                            CountWorkTime = row["COUNT_WORKTIME"].ToString() == "0",
                        });
                }
            }
        }

        /// <summary>
        /// 把停機類型載入 ComboBox。
        /// </summary>
        private void GetDownTypes()
        {
            var items = MachineDownTypes
                .Select(e => new ComboBoxItem(text: e.DESC, value: e.ID))
                .ToArray();

            CbType.DataSource = items;
        }

    }
}
