using QualityInspectionAAR.Enums;
using QualityInspectionAAR.Models;
using SajetClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QcSrv = QualityInspectionAAR.Services.QCService;

namespace QualityInspectionAAR
{
    public partial class FData : Form
    {
        private RuncardModel rc_info;
        private string emp_no = string.Empty;

        private TProgramInfo programInfo = new TProgramInfo();

        public FData(RuncardModel RC_INFO, string EmpNo)
        {
            InitializeComponent();

            rc_info = new RuncardModel(RC_INFO);

            emp_no = EmpNo;

            SajetCommon.SetLanguageControl(this);

            this.Text = SajetCommon.SetLanguage(FormTextEnum.QCInspection.ToString());

            this.Load += FData_Load;

            DgvData.CellEndEdit += DgvData_CellEndEdit;

            BtnSave.Click += BtnSave_Click;

            BtnClose.Click += BtnClose_Click;
        }

        private void FData_Load(object sender, EventArgs e)
        {
            Form1_Load();
        }

        public void Form1_Load()
        {
            LbEmp.Text = emp_no;
            if (string.IsNullOrEmpty(rc_info.RC_NO))
            {
                MessageBox.Show(SajetCommon.SetLanguage(MessageEnum.UnknownRCNO.ToString()),
                                SajetCommon.SetLanguage(MessageEnum.Error.ToString()),
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
                return;
            }
            programInfo.sProgram = ClientUtils.fProgramName;
            programInfo.sFunction = ClientUtils.fFunctionName;
            programInfo.sSQL = new Dictionary<string, string>();
            programInfo.iInputField = new Dictionary<string, List<int>>();
            programInfo.sOption = new Dictionary<string, string>();
            programInfo.iInputVisible = new Dictionary<string, int>();
            programInfo.slDefect = new Dictionary<string, int>();

            string sSQL = $@"SELECT *
                               FROM
                                    SAJET.SYS_BASE_PARAM
                              WHERE
                                    PROGRAM = '{programInfo.sProgram}'
                                AND PARAM_NAME = 'IPQC'
                                AND PARAM_TYPE = 'SQL'
                              ORDER BY
                                    PARAM_TYPE,
                                    DEFAULT_VALUE";

            var ds = ClientUtils.ExecuteSQL(sSQL);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                programInfo.sSQL.Add(dr["DEFAULT_VALUE"].ToString(), dr["PARAM_VALUE"].ToString());

                if (!string.IsNullOrEmpty(dr["PARAM_DESC"].ToString()))
                {
                    string[] sSplit = dr["PARAM_DESC"].ToString().Split(';');

                    programInfo.iInputVisible.Add(dr["DEFAULT_VALUE"].ToString(), int.Parse(sSplit[0]));

                    if (sSplit.Length == 3)
                    {
                        programInfo.sOption.Add(dr["DEFAULT_VALUE"].ToString(), sSplit[2]);
                    }

                    if (sSplit.Length >= 2 && !string.IsNullOrEmpty(sSplit[1]))
                    {
                        sSplit = sSplit[1].Split(',');

                        programInfo.iInputField.Add(dr["DEFAULT_VALUE"].ToString(), new List<int>());

                        foreach (string sValue in sSplit)
                        {
                            programInfo.iInputField[dr["DEFAULT_VALUE"].ToString()].Add(int.Parse(sValue));
                        }
                    }
                }
            }

            #region 資料收集
            QcSrv.LoadInput(dgv: ref DgvData,
                            programInfo: ref programInfo,
                            PART_ID: rc_info.PART_ID,
                            PROCESS_ID: rc_info.PROCESS_ID);
            #endregion

            #region 取得已輸入的資料            
            QcSrv.LoadData(dgv: ref DgvLog,
                           RC_NO: rc_info.RC_NO,
                           PART_ID: rc_info.PART_ID,
                           PROCESS_ID: rc_info.PROCESS_ID);
            #endregion

            #region 取得 RC 已抽數
            LbRCInspectQty.Text = QcSrv.GetRCSampled(RC_NO: rc_info.RC_NO,
                                                     PROCESS_ID: rc_info.PROCESS_ID);
            #endregion

            #region 取得 RC 可抽數
            LbRCQty.Text = QcSrv.GetRCBeSampling(RCSampled: LbRCInspectQty.Text,
                                                 RC_NO: rc_info.RC_NO);

            if (LbRCQty.Text == "0")
            {
                LbRCQty.ForeColor = Color.Red;
            }

            #endregion
        }

        /// <summary>
        /// 提交填寫的資料蒐集
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            // 檢查抽驗資料
            int _rcSample = Convert.ToInt32(QcSrv.GetRCBeSampling(RCSampled: LbRCInspectQty.Text,
                                                                  RC_NO: rc_info.RC_NO));

            if (_rcSample > 0)
            {
                #region Isaac 模擬測試

                var value_list = new List<KeyValuePair<string, string>>();

                foreach (DataGridViewRow dr in DgvData.Rows)
                {
                    if (!string.IsNullOrEmpty(dr.Cells["VALUE_DEFAULT"].ErrorText))
                    {
                        SajetCommon.Show_Message(dr.Cells["ITEM_NAME"].EditedFormattedValue.ToString() + dr.Cells["VALUE_DEFAULT"].ErrorText, 0);

                        DgvData.CurrentCell = dr.Cells["VALUE_DEFAULT"];

                        DgvData.Focus();

                        return;
                    }

                    if (!string.IsNullOrWhiteSpace(dr.Cells["VALUE_DEFAULT"].EditedFormattedValue.ToString()))
                    {
                        var value = new KeyValuePair<string, string>
                        (
                            key: dr.Cells["ITEM_ID"].EditedFormattedValue.ToString(),
                            value: dr.Cells["VALUE_DEFAULT"].EditedFormattedValue.ToString()
                        );

                        value_list.Add(value);
                    }
                }
                #endregion

                QcSrv.SaveData(emp_id: ClientUtils.UserPara1,
                               s_input: value_list,
                               rc_info: rc_info);

                #region 重新讀取資料
                DgvData.Rows.Clear();
                Form1_Load();
                #endregion
            }
            else
            {
                MessageBox.Show(SajetCommon.SetLanguage(MessageEnum.SaveDataError.ToString()),
                                SajetCommon.SetLanguage(MessageEnum.Error.ToString()),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 關閉視窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// 取得員工號的 UserID
        /// </summary>
        /// <param name="EmpNo"></param>
        private string GetEmpID(string EmpNo)
        {
            string sSQL = $@" SELECT EMP_ID 
                                FROM SAJET.SYS_EMP 
                               WHERE EMP_NO = TRIM('{EmpNo}') ";

            var ds = ClientUtils.ExecuteSQL(sSQL);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0]["EMP_ID"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 資料編輯輸入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvData_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell cell = DgvData.Rows[e.RowIndex].Cells[e.ColumnIndex];

            string message = string.Empty;

            cell.ErrorText = "";

            switch (DgvData.Rows[e.RowIndex].Cells["CONVERT_TYPE"].EditedFormattedValue.ToString())
            {
                case "U":
                    {
                        cell.Value = cell.EditedFormattedValue.ToString().ToUpper();

                        break;
                    }
                case "L":
                    {
                        cell.Value = cell.EditedFormattedValue.ToString().ToUpper();

                        break;
                    }
            }

            switch (DgvData.Rows[e.RowIndex].Cells["INPUT_TYPE"].EditedFormattedValue.ToString())
            {
                case "R":
                    {
                        string[] slValue = DgvData.Rows[e.RowIndex].Cells["VALUE_LIST"].EditedFormattedValue.ToString().Split(',');

                        decimal dMin = decimal.Parse(slValue[0]);

                        decimal dMax = decimal.Parse(slValue[1]);

                        try
                        {
                            decimal dValue = decimal.Parse(cell.EditedFormattedValue.ToString());

                            if (dValue >= dMin && dValue <= dMax) { }
                            else
                            {
                                message
                                    = SajetCommon.SetLanguage(MessageEnum.OutOfRange.ToString())
                                    + $"( {dMin} ~ {dMax} )";

                                cell.ErrorText = message;
                            }
                        }
                        catch
                        {
                            if (DgvData.Rows[e.RowIndex].Cells["NECESSARY"].EditedFormattedValue.ToString() == "Y")
                            {
                                cell.ErrorText = SajetCommon.SetLanguage(MessageEnum.InvalidData.ToString());
                            }
                        }

                        break;
                    }
                default:
                    {
                        if (DgvData.Rows[e.RowIndex].Cells["VALUE_TYPE"].EditedFormattedValue.ToString() == "N")
                        {
                            try
                            {
                                decimal dValue = decimal.Parse(cell.EditedFormattedValue.ToString());
                            }
                            catch
                            {
                                if (DgvData.Rows[e.RowIndex].Cells["NECESSARY"].EditedFormattedValue.ToString() == "Y")
                                {
                                    cell.ErrorText = SajetCommon.SetLanguage(MessageEnum.InvalidData.ToString());
                                }
                            }
                        }

                        break;
                    }
            }

            if (string.IsNullOrEmpty(cell.ErrorText) &&
                DgvData.Rows[e.RowIndex].Cells["NECESSARY"].EditedFormattedValue.ToString() == "Y" &&
                string.IsNullOrEmpty(cell.EditedFormattedValue.ToString()))
            {
                cell.ErrorText = SajetCommon.SetLanguage(MessageEnum.EmptyData.ToString());
            }
        }
    }
}
