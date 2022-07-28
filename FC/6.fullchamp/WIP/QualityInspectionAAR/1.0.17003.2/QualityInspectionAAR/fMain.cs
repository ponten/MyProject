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
using QualityInspectionAAR.Enums;
using QualityInspectionAAR.Models;
using OtSrv = QualityInspectionAAR.Services.OtherService;
using QcSrv = QualityInspectionAAR.Services.QCService;

namespace QualityInspectionAAR
{
    public partial class fMain : Form
    {
        #region �ݩʻP�ܼ�

        private readonly string FunctionName;

        private readonly string ProgramName;

        private readonly string UserID;

        /// <summary>
        /// �ثe�Q������y�{�d
        /// </summary>
        private RuncardModel CurrentDataRow = null;

        /// <summary>
        /// �y�{�d���
        /// </summary>
        private List<RuncardModel> RuncardList = new List<RuncardModel>();

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

            #endregion

            #region �j�w�ƥ�

            Load += FMain_Load;

            BtnSearch.Click += BtnSearch_Click;

            BtnClear.Click += BtnClear_Click;

            #endregion
        }

        /// <summary>
        /// ���J����l���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FMain_Load(object sender, EventArgs e)
        {
            if (ReloadFormData(out string message))
            {
                ActivateControls(true);
            }
            else
            {
                SajetCommon.Show_Message(message, 0);

                ActivateControls(false);
            }

            bool granted
                = OtSrv.Check_Privilege(EmpID: UserID,
                                        fun: nameof(QualityInspectionAAR),
                                        type: OperationTypeEnum.UPDATE);

            if (granted)
            {
                AddButton();
            }

            BtnClear.PerformClick();
        }

        /// <summary>
        /// ���s���J��汱������
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool ReloadFormData(out string message)
        {
            message = string.Empty;

            if (QcSrv.FindRuncardToInspect(user_id: UserID, runcards_set: out DataSet d) &&
                            OtSrv.GetStageAndProcess(d, stagesList: ref StagesList))
            {
                RuncardList = QcSrv.GetModel(runcard_set: d);

                LoadStageToComboBox();

                CbStage.SelectedIndexChanged += CbStage_SelectedIndexChanged;

                CbStage_SelectedIndexChanged(null, null);

                CbProcess.SelectedIndexChanged += CbProcess_SelectedIndexChanged;

                return true;
            }
            else
            {
                message = SajetCommon.SetLanguage(MessageEnum.NoAccessibleData.ToString());

                return false;
            }
        }

        /// <summary>
        /// �̷j�M���ˬd�߬y�{�d
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            DgvRC.SelectionChanged -= DgvRC_SelectionChanged;

            if (CbStage.SelectedIndex < 0 || CbProcess.SelectedIndex < 0)
            {
                return;
            }

            QcSrv.FindRuncardToInspect(user_id: UserID, runcards_set: out DataSet d);

            RuncardList = QcSrv.GetModel(runcard_set: d);

            var selected_item = CbProcess.SelectedItem as ComboBoxItemModel;

            var filterModel = new FilterValueModel
            {
                ProcessID = int.Parse(selected_item.ID),
                ProcessName = selected_item.NAME,
                WorkOrder = TbWO.Text?.Trim().ToUpper(),
                Runcard = TbRC.Text?.Trim().ToUpper(),
            };

            var resultSet = OtSrv.FindRuncardsWithConditions(filter: filterModel, runcard_list: RuncardList);

            BindingDataGridView(targetName: nameof(DgvRC), model: resultSet);

            if (resultSet.Count > 0)
            {
                DgvRC.SelectionChanged += DgvRC_SelectionChanged;

                DgvRC.Rows[0].Selected = true;
            }
        }

        /// <summary>
        /// �M�ŵe�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClear_Click(object sender, EventArgs e)
        {
            BindingDataGridView(targetName: nameof(DgvRC), model: null);

            BindingDataGridView(targetName: nameof(DgvData), model: null);

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
            BindingDataGridView(targetName: nameof(DgvRC), model: null);

            BindingDataGridView(targetName: nameof(DgvData), model: null);
        }

        /// <summary>
        /// �I�� DataGridView �W���@���ɡAŪ���ӵ���ƪ��򥻸�T���ҫ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvRC_SelectionChanged(object sender, EventArgs e)
        {
            if (DgvRC.CurrentRow == null)
            {
                ActivateControls(false);

                CurrentDataRow = null;

                return;
            }

            CurrentDataRow = DgvRC.CurrentRow.DataBoundItem as RuncardModel;

            QcSrv.LoadData(
                dgv: ref DgvData,
                RC_NO: CurrentDataRow.RC_NO,
                PART_ID: CurrentDataRow.PART_ID,
                PROCESS_ID: CurrentDataRow.PROCESS_ID);

            DgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            DgvData.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        /// <summary>
        /// �I�� DataGridView �����e�o�ͪ��ƥ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvRC_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DgvRC.Rows.Count <= 0)
            {
                return;
            }

            var DgvSender = sender as DataGridView;

            if (DgvSender.Columns[e.ColumnIndex] is DataGridViewButtonColumn b)
            {
                using (var f = new FData(RC_INFO: CurrentDataRow,
                                         EmpNo: ClientUtils.fUserName))
                {
                    f.ShowDialog();

                    BtnSearch.PerformClick();

                    /*
                    if (ReloadFormData(out string message))
                    {
                        ActivateControls(true);
                    }
                    else
                    {
                        SajetCommon.Show_Message(message, 0);

                        ActivateControls(false);
                    }
                    //*/
                }
            }
        }

        /// <summary>
        /// �u���J�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridViewNumericColumn_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}