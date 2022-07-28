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
        /// �ާ@�H�������u ID
        /// </summary>
        private int Emp_ID;

        /// <summary>
        /// ��e�y�{�d����T
        /// </summary>
        private DataSet rc_no_info;

        /// <summary>
        /// �y�{���X
        /// </summary>
        private string RC_NO;

        private string NODE_ID;

        /// <summary>
        /// �S���s�W��ƪ��v���A�u��d��
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
        /// ���J�ާ@�e�����U�����󪺳]�w
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
        /// �e���������J��A������ݩ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FMain_Shown(object sender, EventArgs e)
        {
            Tb_Emp.Focus();
        }

        /// <summary>
        /// �d�ߧ@�~�H������T�����s
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

            // �H�j�w�ƥ󪺤覡����ާ@�H�����v��
            read_only = !OtSrv.Check_Privilege(
                emp_id: Emp_ID.ToString(),
                function_name: ClientUtils.fFunctionName,
                operation: OperationEnum.Update);
        }

        /// <summary>
        /// �d�߬y�{�d����T�����s
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_OK_2_Click(object sender, EventArgs e)
        {
            RC_NO = Tb_RC.Text.Trim().ToUpper();

            string message;

            // �ˬd�s�{�v��
            if (!RcSrv.CheckAccessToProcess(
                emp_id: Emp_ID.ToString(),
                rc_no: RC_NO,
                out message))
            {
                SajetCommon.Show_Message(message, 1);

                Tb_RC.SelectAll();

                return;
            }

            // ���o�y�{�d��T
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

            // �ˬd�y�{�d�����A�A�����O�u�Ͳ����v���A���y�{�d
            // �]���ݭn�����J���s�{�~�ݭn�������x�u�ɡA�]�~�����n�O�������N�X�B�ư����`�u��
            string current_status = rc_no_info.Tables[0].Rows[0]["CURRENT_STATUS"].ToString();

            if (current_status != "1")
            {
                message = SajetCommon.SetLanguage(MessageEnum.RuncardNotRunning.ToString());

                SajetCommon.Show_Message(message, 1);

                Tb_RC.SelectAll();

                return;
            }

            // ���o�[�u���x��T
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

            // ��ܬy�{�d��T
            Loading(
                control_group: FormControlGroupEnum.RuncardInformation,
                data: rc_no_info);

            NODE_ID = rc_no_info.Tables[0].Rows[0]["NODE_ID"].ToString();

            // ��ܨϥΤ������x����T
            Loading(
                control_group: FormControlGroupEnum.MachineInUse,
                data: machine_in_use);

            // ���o�åB��ܫ��w���x�������O��
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
        /// �M�žާ@�e�������s
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Clear_Click(object sender, EventArgs e)
        {
            CleanForm(LockElementLevelEnum.EmpTextBox);

            Tb_Emp.Focus();
        }

        /// <summary>
        /// ���氱����T�����s
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Submit_Click(object sender, EventArgs e)
        {
            string message;

            DateTime now = OtSrv.GetDBDateTimeNow();

            // �ˬd�y�{�d��
            if (Tb_RC.Text.Trim() != RC_NO || rc_no_info == null)
            {
                message = SajetCommon.SetLanguage(MessageEnum.RuncardNotFound.ToString());

                SajetCommon.Show_Message(message, 1);

                return;
            }

            // �ˬd�y�{�d�٦b���b�d�߮ɩҦb���s�{
            if (!RcSrv.CheckState(
                rc_no_info: rc_no_info.Tables[0].Rows[0],
                out message))
            {
                SajetCommon.Show_Message(message, 1);

                CleanForm(LockElementLevelEnum.RuncardTextBox);

                return;
            }

            // �����N�X
            string machine_id = Dgv_Machine_InUse.CurrentRow.Cells["MACHINE_ID"].Value.ToString();

            if (string.IsNullOrWhiteSpace(Cb_DownCode.Text))
            {
                message = SajetCommon.SetLanguage(MessageEnum.DownCodeEmpty.ToString());

                SajetCommon.Show_Message(message, 1);

                return;
            }

            // �ˬd�����ɶ�
            DateTime start_time = Dtp_Start_Date.Value.Date + Dtp_Start_Time.Value.TimeOfDay;

            DateTime end_time = Dtp_End_Date.Value.Date + Dtp_End_Time.Value.TimeOfDay;

            // �P�}�l�ɶ��ˬd
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

            // ���s���o���������A�åB�@���@���ˬd�ɶ��Ϭq���S������
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

            // �s�W����
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

            // �ˬd�y�{�d�٦b���b
            if (!RcSrv.CheckState(
                rc_no_info: rc_no_info.Tables[0].Rows[0],
                out message))
            {
                SajetCommon.Show_Message(message, 1);

                CleanForm(LockElementLevelEnum.RuncardTextBox);

                return;
            }

            // ���s���o�����O��
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
        /// ���U Enter �d�߬y�{�d����T
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tb_Emp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return) return;

            Btn_OK_1_Click(null, null);
        }

        /// <summary>
        /// ���U Enter �d�߬y�{�d����T
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tb_RC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return) return;

            Btn_OK_2_Click(null, null);
        }

        /// <summary>
        /// �ھڰ����������J�����N�X������� ComboBox
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
        /// �I���ϥΤ������x�A��ܰ����O��
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
        /// �ⰱ���������J ComboBox�C
        /// </summary>
        private void GetDownTypes()
        {
            var items = DownTypes
                .Select(e => new ComboBoxItem(text: e.NAME, value: e.ID))
                .ToArray();

            Cb_DownType.DataSource = items;
        }

        /// <summary>
        /// �M�ŵe����T�A�åB�̷ӵ{����w�����
        /// </summary>
        /// <param name="level">��w����</param>
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
        /// ���J�e���W�����
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