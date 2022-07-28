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

namespace MachineAAR
{
    public partial class fMain : Form
    {
        #region �ݩʻP�ܼ�

        private readonly string FunctionName;

        private readonly string ProgramName;

        private readonly string UserID;

        private RuncardModel CurrentDataRow = null;

        /// <summary>
        /// �P�s�{�j�w���i�ξ��x���M��C
        /// </summary>
        private List<MachineModel> MachineList = new List<MachineModel>();

        /// <summary>
        /// �Ϭq�P�s�{��ơC
        /// </summary>
        private List<StageModel> StagesList = new List<StageModel>();

        #endregion

        public fMain()
        {
            #region �]�w�ݩ�

            InitializeComponent();

            SajetCommon.SetLanguageControl(this);

            UserID = ClientUtils.UserPara1;

            ProgramName = ClientUtils.fProgramName;

            FunctionName = ClientUtils.fFunctionName;

            var buttonColumn = new DataGridViewButtonColumn
            {
                Name = "BtnWorkTime",
                Text = SajetCommon.SetLanguage("Set up"),
                HeaderText = SajetCommon.SetLanguage("Set work time"),
                UseColumnTextForButtonValue = true,
            };

            DgvMachine.Columns.Add(buttonColumn);

            #endregion

            #region �j�w�ƥ�

            Load += FMain_Load;

            BtnSearch.Click += BtnSearch_Click;

            BtnOK.Click += BtnOK_Click;

            BtnClear.Click += BtnClear_Click;

            DgvMachine.CellContentClick += DgvMachine_CellContentClick;

            DgvMachine.CellClick += DgvMachine_CellClick;

            #endregion
        }

        #region �ƥ�

        private void FMain_Load(object sender, EventArgs e)
        {
            if (Services.FindStageAndProcess(userID: UserID, stagesList: ref StagesList))
            {
                LoadStageToComboBox();

                ActivateControls(true);

                CbStage.SelectedIndexChanged += CbStage_SelectedIndexChanged;

                CbStage_SelectedIndexChanged(null, null);

                CbProcess.SelectedIndexChanged += CbProcess_SelectedIndexChanged;
            }
            else
            {
                string message = SajetCommon.SetLanguage("No accessible data");

                SajetCommon.Show_Message(message, 1);

                ActivateControls(false);
            }

            BtnClear.PerformClick();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            DgvRuncard.SelectionChanged -= DgvRuncard_SelectionChanged;

            if (CbStage.SelectedIndex < 0 || CbProcess.SelectedIndex < 0)
            {
                return;
            }

            var filterModel = new FilterValueModel
            {
                ProcessName = CbProcess.Text,
                WorkOrder = TbWO.Text?.Trim().ToUpper(),
                Runcard = TbRC.Text?.Trim().ToUpper(),
            };

            var resultSet = Services.FindRuncardsWithConditions(filterModel);

            BindingDataGridView(targetName: nameof(DgvRuncard), model: resultSet);

            if (resultSet.Count > 0)
            {
                DgvRuncard.SelectionChanged += DgvRuncard_SelectionChanged;

                DgvRuncard.Rows[0].Selected = true;
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            string message;

            if (!PreCheckMachine(out message))
            {
                SajetCommon.Show_Message(message, 0);

                return;
            }

            if (!PreCheckWorkTime(out message))
            {
                SajetCommon.Show_Message(message, 0);

                return;
            }

            // Double check �A����
            message = SajetCommon.SetLanguage("Confirm to add the machine");

            if (SajetCommon.Show_Message(message, 2) != DialogResult.Yes)
            {
                return;
            }

            Services.AddMachine(userID: UserID, currentRuncard: CurrentDataRow, startTime: DtpStart.Value, endTime: DtpEnd.Value, machineList: MachineList);

            BtnClear.PerformClick();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            BindingDataGridView(targetName: nameof(DgvRuncard), model: null);

            BindingDataGridView(targetName: nameof(DgvMachine), model: null);

            ActivateControls(false);
        }

        /// <summary>
        /// ���J�Ϭq���ݪ��s�{�� ComboBox�C
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbStage_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProcessToComboBox();
        }

        /// <summary>
        /// �M�� DataGridView�C
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbProcess_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindingDataGridView(targetName: nameof(DgvRuncard), model: null);

            BindingDataGridView(targetName: nameof(DgvMachine), model: null);
        }

        private void DgvRuncard_SelectionChanged(object sender, EventArgs e)
        {
            if (DgvRuncard.CurrentRow == null)
            {
                ActivateControls(false);

                CurrentDataRow = null;

                return;
            }

            CurrentDataRow = DgvRuncard.CurrentRow.DataBoundItem as RuncardModel;

            bool got = Services.GetMachineForDataGridView(dataRow: CurrentDataRow, machineList: ref MachineList);

            if (got)
            {
                BindingDataGridView(targetName: nameof(DgvMachine), model: MachineList);

                BindingDateTimePicker(CurrentDataRow);

                ActivateControls(true);
            }
            else
            {
                string message = SajetCommon.SetLanguage("The process machine is not bound and cannot be set");

                SajetCommon.Show_Message(message, 1);

                ActivateControls(false);

                //BindingDataGridView(targetName: nameof(DgvRuncard), model: null);

                BindingDataGridView(targetName: nameof(DgvMachine), model: null);
            }
        }

        /// <summary>
        /// �Ŀ���x���欰�C
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvMachine_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != DgvMachine.Columns[nameof(selectDataGridViewCheckBoxColumn)].Index)
            {
                return;
            }

            bool selected = !(bool)DgvMachine.Rows[e.RowIndex].Cells[nameof(selectDataGridViewCheckBoxColumn)].Value;

            string MachineCode = DgvMachine.Rows[e.RowIndex].Cells[nameof(mACHINECODEDataGridViewTextBoxColumn)].Value.ToString();

            DgvMachine.Rows[e.RowIndex].Cells[nameof(selectDataGridViewCheckBoxColumn)].Value = selected;

            MachineList
                .FirstOrDefault(x => x.MACHINE_CODE == MachineCode)
                .Select = selected;
        }

        private void DgvMachine_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DgvMachine.Rows.Count <= 0)
            {
                return;
            }

            var dgvSender = sender as DataGridView;

            if (dgvSender.Columns[e.ColumnIndex] is DataGridViewButtonColumn b)
            {
                var currentMachine
                    = MachineList
                    .First(x => x.MACHINE_ID == (dgvSender.CurrentRow.DataBoundItem as MachineModel).MACHINE_ID);

                using (var f = new fWorkTime(startTime: currentMachine.START_TIME, endTime: currentMachine.END_TIME, defaultEndTime: (DateTime)CurrentDataRow.WIP_OUT_TIME))
                {
                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        currentMachine.START_TIME = f.StartTime;

                        currentMachine.END_TIME = f.EndTime;

                        currentMachine.Select = true;

                        BindingDataGridView(targetName: nameof(DgvMachine), model: MachineList);
                    }
                }
            }
        }

        #endregion

        #region ��k

        /// <summary>
        /// �ҥΩΰ��α���C
        /// </summary>
        /// <param name="active"></param>
        private void ActivateControls(bool active = false)
        {
            DtpStart.Enabled = active;

            DtpEnd.Enabled = active;

            BtnOK.Enabled = active;

            if (!active)
            {
                DgvRuncard.SelectionChanged -= DgvRuncard_SelectionChanged;
            }
        }

        /// <summary>
        /// �N��ƨӷ��P DataGridView �j�w�C
        /// </summary>
        /// <param name="model"></param>
        private void BindingDataGridView(string targetName, object model)
        {
            switch (targetName)
            {
                case nameof(DgvRuncard):
                    {
                        var m = (List<RuncardModel>)model ?? new List<RuncardModel>();

                        var bindingList = new BindingList<RuncardModel>(m);

                        var source = new BindingSource(bindingList, null);

                        DgvRuncard.DataSource = source;

                        DgvRuncard.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                        break;
                    }
                case nameof(DgvMachine):
                    {
                        var m = (List<MachineModel>)model ?? new List<MachineModel>();

                        var bindingList = new BindingList<MachineModel>(m);

                        var source = new BindingSource(bindingList, null);

                        DgvMachine.DataSource = source;

                        DgvMachine.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                        break;
                    }
                default:
                    break;
            }
        }

        /// <summary>
        /// �N�y�{�d���X�ɶ��]�w���w�]�����x�u�@�����ɶ�
        /// </summary>
        /// <param name="model"></param>
        private void BindingDateTimePicker(RuncardModel model)
        {
            var OutProcessTime = (DateTime)model.WIP_OUT_TIME;

            DtpStart.Value = OutProcessTime;

            DtpEnd.Value = OutProcessTime;
        }

        /// <summary>
        /// �N�Ϭq���J ComboBox�C
        /// </summary>
        private void LoadStageToComboBox()
        {
            CbStage.DataSource = StagesList.Select(x => new ComboBoxItemModel
            {
                ID = x.ID,
                CODE = x.CODE,
                NAME = x.NAME,
            }).ToList();

            CbStage.DisplayMember = nameof(ComboBoxItemModel.NAME);

            CbStage.ValueMember = nameof(ComboBoxItemModel.ID);
        }

        /// <summary>
        /// ���J����Ϭq���s�{�� ComboBox�C
        /// </summary>
        private void LoadProcessToComboBox()
        {
            var processes = StagesList
                .First(x => x.ID.ToString() == CbStage.SelectedValue.ToString())
                .ProcessModel
                .Select(x => new ComboBoxItemModel
                {
                    ID = x.ID,
                    CODE = x.CODE,
                    NAME = x.NAME,
                }).ToList(); ;

            CbProcess.DataSource = processes;

            CbProcess.DisplayMember = nameof(ComboBoxItemModel.NAME);

            CbProcess.ValueMember = nameof(ComboBoxItemModel.ID);
        }

        /// <summary>
        /// ����e�ˬd���x
        /// </summary>
        /// <param name="message">���~�T��</param>
        /// <returns></returns>
        private bool PreCheckMachine(out string message)
        {
            message = string.Empty;

            // �ˬd���S���Ŀ���x
            if (!MachineList.Any(x => x.Select))
            {
                message = SajetCommon.SetLanguage("No machine selected");

                return false;
            }

            // �ˬd���x�O���O�w�g�b�ϥ�
            if (Services.IsMachineInUse(runcardNumber: CurrentDataRow.RC_NO, out message, machineList: ref MachineList))
            {
                message
                    = SajetCommon.SetLanguage("Machine already in use")
                    + ": "
                    + Environment.NewLine
                    + message;

                return false;
            }

            return true;
        }

        /// <summary>
        /// ����e�ˬd���x�u�@�ɶ�
        /// </summary>
        /// <param name="message">���~�T��</param>
        /// <returns></returns>
        private bool PreCheckWorkTime(out string message)
        {
            message
                = SajetCommon.SetLanguage("Invalid WorkTime setting")
                + Environment.NewLine;

            if (DtpStart.Value > CurrentDataRow.WIP_OUT_TIME)
            {
                message += SajetCommon.SetLanguage("The default start time is later than the runcard out-process time");

                return false;
            }
            else if (DtpStart.Value > DtpEnd.Value)
            {
                message += SajetCommon.SetLanguage("The default start time is later than the default end time");

                return false;
            }
            else if (DtpEnd.Value > CurrentDataRow.WIP_OUT_TIME)
            {
                message += SajetCommon.SetLanguage("The default end time is later than the runcard out-process time");

                return false;
            }

            var rejects = new List<string>();

            var ml = MachineList.Where(x => x.Select).ToList();

            bool allTimeSettingIsValid
                = ml
                .All(x =>
                {
                    var tStart = x.START_TIME ?? DtpStart.Value;

                    var tEnd = x.END_TIME ?? DtpEnd.Value;

                    if (tStart > tEnd)
                    {
                        rejects.Add(x.MACHINE_DESC);

                        return false;
                    }

                    return true;
                });

            if (!allTimeSettingIsValid)
            {
                message
                    += SajetCommon.SetLanguage("The start time is later than the end time")
                    + ": "
                    + Environment.NewLine
                    + string.Join(",", rejects);

                return false;
            }

            return true;
        }

        #endregion
    }
}