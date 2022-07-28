using QualityInspectionAAR.Enums;
using QualityInspectionAAR.Models;
using SajetClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QualityInspectionAAR
{
    // 與表單控制項有關的方法獨立出來放這裡
    partial class fMain
    {
        /// <summary>
        /// 啟用或停用控制項。
        /// </summary>
        /// <param name="active"></param>
        private void ActivateControls(bool active = false)
        {
            if (!active)
            {
                DgvRC.SelectionChanged -= DgvRC_SelectionChanged;
            }
        }

        /// <summary>
        /// 給 DataGridView 加上補資料按鈕
        /// </summary>
        private void AddButton()
        {
            var buttonColumn = new DataGridViewButtonColumn
            {
                Name = nameof(FormControlNameEnum.QCInspectionDataGridViewButtonColumn),
                Text = SajetCommon.SetLanguage(FormTextEnum.QCInspection.ToString()),
                HeaderText = SajetCommon.SetLanguage(FormTextEnum.QCInspection.ToString()),
                UseColumnTextForButtonValue = true,
                DisplayIndex = 0,
                Frozen = true,
            };

            DgvRC.Columns.Add(buttonColumn);

            DgvRC.CellContentClick += DgvRC_CellContentClick;
        }

        /// <summary>
        /// 將資料來源與 DataGridView 綁定。
        /// </summary>
        /// <param name="model"></param>
        private void BindingDataGridView(string targetName, object model)
        {
            var h = GetHiddenColumnNames(targetName);

            switch (targetName)
            {
                case nameof(DgvRC):
                    {
                        var m = (List<RuncardModel>)model ?? new List<RuncardModel>();

                        var bindingList = new BindingList<RuncardModel>(m);

                        var source = new BindingSource(bindingList, null);

                        DgvRC.DataSource = source;

                        foreach (DataGridViewColumn column in DgvRC.Columns)
                        {
                            column.Visible = !h.Any(x => x == column.Name);

                            if (column is DataGridViewButtonColumn c)
                            {
                                column.HeaderText = SajetCommon.SetLanguage(FormTextEnum.QCInspection.ToString());
                            }
                            else
                            {
                                column.HeaderText = SajetCommon.SetLanguage(column.Name);
                            }
                        }

                        DgvRC.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

                        DgvRC.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                        break;
                    }
                case nameof(DgvData):
                    {
                        var m = (DataSet)model ?? new DataSet();

                        DgvData.DataSource = m;

                        foreach (DataGridViewColumn column in DgvData.Columns)
                        {
                            column.Visible = !h.Any(x => x == column.Name);

                            column.HeaderText = SajetCommon.SetLanguage(column.Name);
                        }

                        DgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

                        DgvData.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                        break;
                    }
                default:
                    break;
            }
        }

        /// <summary>
        /// 將區段載入 ComboBox。
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
        /// 載入選取區段的製程到 ComboBox。
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
                }).ToList();

            CbProcess.DataSource = processes;

            CbProcess.DisplayMember = nameof(ComboBoxItemModel.NAME);

            CbProcess.ValueMember = nameof(ComboBoxItemModel.ID);
        }

        /// <summary>
        /// 取得指定的 DataGridView 要隱藏的欄位名稱
        /// </summary>
        /// <param name="dataGridView_Name">DataGridView 的名字</param>
        /// <returns></returns>
        public static List<string> GetHiddenColumnNames(string dataGridView_Name = "")
        {
            var h = new List<string>();

            switch (dataGridView_Name)
            {
                case nameof(DgvRC):
                    {
                        h.Add(nameof(RuncardModel.STAGE_ID));
                        h.Add(nameof(RuncardModel.STAGE_CODE));
                        h.Add(nameof(RuncardModel.STAGE_NAME));
                        h.Add(nameof(RuncardModel.PROCESS_ID));
                        h.Add(nameof(RuncardModel.PROCESS_CODE));
                        h.Add(nameof(RuncardModel.PROCESS_NAME));
                        h.Add(nameof(RuncardModel.NODE_ID));
                        h.Add(nameof(RuncardModel.PART_ID));
                        break;
                    }
                case nameof(DgvData):
                    {
                        break;
                    }
                default:
                    h.Add("--");
                    break;
            }

            return h;
        }
    }
}
