using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.IO;
using System.Linq;
using MachineDownTime.Enums;
using MachineDownTime.Models;
using OtSrv = MachineDownTime.Services.OtherService;
using RcSrv = MachineDownTime.Services.RuncardService;
using McSrv = MachineDownTime.Services.MachineService;

namespace MachineDownTime
{
    public partial class fMain : Form
    {
        private readonly string FunctionName;

        private readonly string ProgramName;

        private readonly string UserID;

        private List<DownTypeModel> DownTypes;

        /// <summary>
        /// 操作人員的員工 ID
        /// </summary>
        private int Emp_ID;

        /// <summary>
        /// 當前流程卡的資訊
        /// </summary>
        private DataSet rc_no_info;

        /// <summary>
        /// 流程號碼
        /// </summary>
        private string RC_NO;

        private string NODE_ID;

        /// <summary>
        /// 沒有新增資料的權限，只能查詢
        /// </summary>
        private bool read_only;

        public fMain()
        {
            InitializeComponent();

            SajetCommon.SetLanguageControl(this);

            UserID = ClientUtils.UserPara1;

            ProgramName = ClientUtils.fProgramName;

            FunctionName = ClientUtils.fFunctionName;

            Load += FMain_Load;

            Shown += FMain_Shown;

            Btn_OK_1.Click += Btn_OK_1_Click;

            Btn_OK_2.Click += Btn_OK_2_Click;

            Btn_Clear.Click += Btn_Clear_Click;

            Btn_Submit.Click += Btn_Submit_Click;

            Tb_Emp.KeyPress += Tb_Emp_KeyPress;

            Tb_RC.KeyPress += Tb_RC_KeyPress;

            Cb_DownType.SelectedIndexChanged += Cb_DownType_SelectedIndexChanged;

            Dgv_Machine_InUse.CellClick += Dgv_Machine_InUse_CellClick;
        }

        /// <summary>
        /// 載入操作畫面中各項元件的設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FMain_Load(object sender, EventArgs e)
        {
            DownTypes = McSrv.GetDownCodes();

            GetDownTypes();

            CleanForm(LockElementLevelEnum.EmpTextBox);
        }

        /// <summary>
        /// 畫面完成載入後，控制元件屬性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FMain_Shown(object sender, EventArgs e)
        {
            Tb_Emp.Focus();
        }

        /// <summary>
        /// 查詢作業人員的資訊的按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_OK_1_Click(object sender, EventArgs e)
        {
            string emp_no = Tb_Emp.Text.Trim();

            if (OtSrv.GetEmpID(emp_no: emp_no, out Emp_ID))
            {
                CleanForm(LockElementLevelEnum.RuncardTextBox);

                Tb_RC.Focus();
            }
            else
            {
                string message = SajetCommon.SetLanguage(MessageEnum.EmpNotFound.ToString());

                SajetCommon.Show_Message(message, 1);

                Tb_Emp.SelectAll();
            }

            // 以綁定事件的方式控制操作人員的權限
            read_only = !OtSrv.Check_Privilege(
                emp_id: Emp_ID.ToString(),
                function_name: ClientUtils.fFunctionName,
                operation: OperationEnum.Update);
        }

        /// <summary>
        /// 查詢流程卡的資訊的按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_OK_2_Click(object sender, EventArgs e)
        {
            RC_NO = Tb_RC.Text.Trim().ToUpper();

            string message;

            // 檢查製程權限
            if (!RcSrv.CheckAccessToProcess(
                emp_id: Emp_ID.ToString(),
                rc_no: RC_NO,
                out message))
            {
                SajetCommon.Show_Message(message, 1);

                Tb_RC.SelectAll();

                return;
            }

            // 取得流程卡資訊
            if (!RcSrv.GetInformation(
                rc_no: RC_NO,
                out rc_no_info,
                out message))
            {
                rc_no_info = null;

                RC_NO = string.Empty;

                NODE_ID = string.Empty;

                SajetCommon.Show_Message(message, 1);

                Tb_RC.SelectAll();

                return;
            }

            // 檢查流程卡的狀態，必須是「生產中」狀態的流程卡
            // 因為需要執行投入的製程才需要收集機台工時，也才有必要記錄停機代碼、排除異常工時
            string current_status = rc_no_info.Tables[0].Rows[0]["CURRENT_STATUS"].ToString();

            if (current_status != "1")
            {
                message = SajetCommon.SetLanguage(MessageEnum.RuncardNotRunning.ToString());

                SajetCommon.Show_Message(message, 1);

                Tb_RC.SelectAll();

                return;
            }

            // 取得加工機台資訊
            if (!McSrv.GetMachineInUse(
                rc_no: RC_NO,
                out DataSet machine_in_use,
                out message))
            {
                SajetCommon.Show_Message(message, 1);

                Tb_RC.SelectAll();

                CleanForm(LockElementLevelEnum.RuncardTextBox);

                return;
            }

            // 顯示流程卡資訊
            Loading(
                control_group: FormControlGroupEnum.RuncardInformation,
                data: rc_no_info);

            NODE_ID = rc_no_info.Tables[0].Rows[0]["NODE_ID"].ToString();

            // 顯示使用中的機台的資訊
            Loading(
                control_group: FormControlGroupEnum.MachineInUse,
                data: machine_in_use);

            // 取得並且顯示指定機台的停機記錄
            string machine_id = Dgv_Machine_InUse.Rows[0].Cells["MACHINE_ID"].Value.ToString();

            McSrv.GetDownLog(
                rc_no: RC_NO,
                node_id: NODE_ID,
                machine_id: machine_id,
                out DataSet machine_down_log,
                out message);

            Loading(
                FormControlGroupEnum.DownTimeLog,
                data: machine_down_log);

            Dgv_Machine_InUse.Rows[0].Selected = true;

            CleanForm(LockElementLevelEnum.DataGroupBox);
        }

        /// <summary>
        /// 清空操作畫面的按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Clear_Click(object sender, EventArgs e)
        {
            CleanForm(LockElementLevelEnum.EmpTextBox);

            Tb_Emp.Focus();
        }

        /// <summary>
        /// 提交停機資訊的按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Submit_Click(object sender, EventArgs e)
        {
            string message;

            DateTime now = OtSrv.GetDBDateTimeNow();

            // 檢查流程卡號
            if (Tb_RC.Text.Trim() != RC_NO || rc_no_info == null)
            {
                message = SajetCommon.SetLanguage(MessageEnum.RuncardNotFound.ToString());

                SajetCommon.Show_Message(message, 1);

                return;
            }

            // 檢查流程卡還在不在查詢時所在的製程
            if (!RcSrv.CheckState(
                rc_no_info: rc_no_info.Tables[0].Rows[0],
                out message))
            {
                SajetCommon.Show_Message(message, 1);

                CleanForm(LockElementLevelEnum.RuncardTextBox);

                return;
            }

            // 停機代碼
            string machine_id = Dgv_Machine_InUse.CurrentRow.Cells["MACHINE_ID"].Value.ToString();

            if (string.IsNullOrWhiteSpace(Cb_DownCode.Text))
            {
                message = SajetCommon.SetLanguage(MessageEnum.DownCodeEmpty.ToString());

                SajetCommon.Show_Message(message, 1);

                return;
            }

            // 檢查停機時間
            DateTime start_time = Dtp_Start_Date.Value.Date + Dtp_Start_Time.Value.TimeOfDay;

            DateTime end_time = Dtp_End_Date.Value.Date + Dtp_End_Time.Value.TimeOfDay;

            // 與開始時間檢查
            DateTime.TryParse(rc_no_info.Tables[0].Rows[0]["IN_PROCESS_TIME"].ToString(), out DateTime in_process_time);

            if (in_process_time > start_time)
            {
                message = SajetCommon.SetLanguage(MessageEnum.StartTimeIsEarlierThanInProcessTime.ToString());

                SajetCommon.Show_Message(message, 1);

                return;
            }

            if (start_time >= end_time)
            {
                message = SajetCommon.SetLanguage(MessageEnum.EndTimeIsEarlierThanStartTime.ToString());

                SajetCommon.Show_Message(message, 1);

                return;
            }

            if (end_time > now)
            {
                message = SajetCommon.SetLanguage(MessageEnum.EndTimeIsEarlierThanStartTime.ToString());

                SajetCommon.Show_Message(message, 1);

                return;
            }

            // 重新取得停機紀錄，並且一筆一筆檢查時間區段有沒有重複
            if (McSrv.GetDownLog(
                rc_no: RC_NO,
                node_id: NODE_ID,
                machine_id: machine_id,
                out DataSet machine_down_log,
                out message))
            {
                if (!McSrv.CheckDownTimeSpan(
                    down_log: machine_down_log,
                    start_time: start_time,
                    end_time: end_time,
                    out message))
                {
                    SajetCommon.Show_Message(message, 1);

                    return;
                }
            }

            // 新增紀錄
            int reason_id = int.Parse(((ComboBoxItem)Cb_DownCode.SelectedItem).Value);

            string remark = Tb_Memo.Text.Trim();

            McSrv.InsertNewRecord(
                rc_no_info: rc_no_info.Tables[0].Rows[0],
                machine_id: machine_id,
                start_time: start_time,
                end_time: end_time,
                reason_id: reason_id,
                remark: remark,
                update_userid: Emp_ID);

            // 檢查流程卡還在不在
            if (!RcSrv.CheckState(
                rc_no_info: rc_no_info.Tables[0].Rows[0],
                out message))
            {
                SajetCommon.Show_Message(message, 1);

                CleanForm(LockElementLevelEnum.RuncardTextBox);

                return;
            }

            // 重新取得停機記錄
            McSrv.GetDownLog(
                rc_no: RC_NO,
                node_id: NODE_ID,
                machine_id: machine_id,
                out machine_down_log,
                out message);

            Loading(
                FormControlGroupEnum.DownTimeLog,
                data: machine_down_log);

            CleanForm(LockElementLevelEnum.DataGroupBox);
        }

        /// <summary>
        /// 按下 Enter 查詢流程卡的資訊
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tb_Emp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return) return;

            Btn_OK_1_Click(null, null);
        }

        /// <summary>
        /// 按下 Enter 查詢流程卡的資訊
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tb_RC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return) return;

            Btn_OK_2_Click(null, null);
        }

        /// <summary>
        /// 根據停機類型載入停機代碼到對應的 ComboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cb_DownType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var items = DownTypes
                .FirstOrDefault(x =>
                {
                    if (Cb_DownType.SelectedItem is ComboBoxItem item)
                    {
                        return x.ID == item.Value;
                    }
                    else
                    {
                        return x.ID == "0";
                    }
                })
                .DownCodes
                .Select(x => new ComboBoxItem(text: x.NAME, value: x.ID))
                .ToArray();

            Cb_DownCode.DataSource = items;
        }

        /// <summary>
        /// 點擊使用中的機台，顯示停機記錄
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Dgv_Machine_InUse_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string machine_id = Dgv_Machine_InUse.CurrentRow.Cells["MACHINE_ID"].Value.ToString();

            Lb_Machine.Text = Dgv_Machine_InUse.CurrentRow.Cells["MACHINE_CODE"].Value.ToString();

            McSrv.GetDownLog(
                rc_no: RC_NO,
                node_id: NODE_ID,
                machine_id: machine_id,
                out DataSet machine_down_log,
                out string message);

            Loading(
                FormControlGroupEnum.DownTimeLog,
                data: machine_down_log);

        }

        /// <summary>
        /// 把停機類型載入 ComboBox。
        /// </summary>
        private void GetDownTypes()
        {
            var items = DownTypes
                .Select(e => new ComboBoxItem(text: e.NAME, value: e.ID))
                .ToArray();

            Cb_DownType.DataSource = items;
        }

        /// <summary>
        /// 清空畫面資訊，並且依照程度鎖定控制元件
        /// </summary>
        /// <param name="level">鎖定等級</param>
        private void CleanForm(LockElementLevelEnum level)
        {
            DateTime now = OtSrv.GetDBDateTimeNow();

            if (level >= LockElementLevelEnum.DataGroupBox && !read_only)
            {
                Dtp_Start_Date.Value = now;
                Dtp_Start_Time.Value = now;
                Dtp_End_Date.Value = now;
                Dtp_End_Time.Value = now;
                Tb_Memo.Text = string.Empty;

                foreach (Control c in Tlp_Data.Controls)
                {
                    c.Enabled = true;
                }
            }

            if (level >= LockElementLevelEnum.RuncardTextBox)
            {
                Tb_RC.Text = string.Empty;

                Tb_RC.Enabled = true;

                Btn_OK_2.Enabled = true;

                foreach (Control c in Tlp_Data.Controls)
                {
                    c.Enabled = false;
                }

                Dgv_Machine_InUse.DataSource = null;

                Dgv_DownTime_Log.DataSource = null;
            }

            if (level >= LockElementLevelEnum.EmpTextBox)
            {
                Tb_Emp.Text = string.Empty;
                foreach (Control c in Tlp_RCInfo.Controls)
                {
                    if (c is TextBox t)
                    {
                        t.Text = string.Empty;
                    }
                }
                Lb_Process.Text = string.Empty;
                Lb_Inprocess_Time.Text = string.Empty;

                Tb_RC.Enabled = false;

                Btn_OK_2.Enabled = false;
            }
        }

        /// <summary>
        /// 載入畫面上的資料
        /// </summary>
        /// <param name="control_group"></param>
        /// <param name="data"></param>
        private void Loading(
            FormControlGroupEnum control_group,
            DataSet data)
        {
            if (control_group == FormControlGroupEnum.RuncardInformation)
            {
                DataRow data_row = data.Tables[0].Rows[0];
                Tb_WorkOrder.Text = data_row["WORK_ORDER"].ToString();
                Tb_FormerNo.Text = data_row["FORMER_NO"].ToString();
                Tb_Spec.Text = data_row["SPEC2"].ToString();
                Tb_PartNo.Text = data_row["PART_NO"].ToString();
                Tb_Runcard.Text = data_row["RC_NO"].ToString();
                Tb_Qty.Text = data_row["QTY"].ToString();
                Tb_Good.Text = data_row["GOOD_QTY"].ToString();
                Tb_Scrap.Text = data_row["SCRAP_QTY"].ToString();
                Lb_Process.Text
                    = SajetCommon.SetLanguage(FormTextEnum.Process.ToString())
                    + " : "
                    + data_row["PROCESS_NAME"].ToString();
                Lb_Inprocess_Time.Text
                    = SajetCommon.SetLanguage(FormTextEnum.InProcessTime.ToString())
                    + " : "
                    + data_row["IN_PROCESS_TIME"].ToString();
            }
            if (control_group == FormControlGroupEnum.MachineInUse)
            {
                Dgv_Machine_InUse.DataSource = data;
                Dgv_Machine_InUse.DataMember = data.Tables[0].ToString();
                Dgv_Machine_InUse.Columns["RC_NO"].Visible = false;
                Dgv_Machine_InUse.Columns["MACHINE_ID"].Visible = false;
                Dgv_Machine_InUse.Columns["MACHINE_CODE"].Visible = false;
                foreach (DataGridViewColumn column in Dgv_Machine_InUse.Columns)
                {
                    column.HeaderText = SajetCommon.SetLanguage(column.Name);

                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                OtSrv.RearrangeDataGridView(ref Dgv_Machine_InUse);
            }
            if (control_group == FormControlGroupEnum.DownTimeLog)
            {
                Dgv_DownTime_Log.DataSource = data;
                Dgv_DownTime_Log.DataMember = data.Tables[0].ToString();
                foreach (DataGridViewColumn column in Dgv_DownTime_Log.Columns)
                {
                    column.HeaderText = SajetCommon.SetLanguage(column.Name);

                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                OtSrv.RearrangeDataGridView(ref Dgv_DownTime_Log);
            }
        }
    }
}