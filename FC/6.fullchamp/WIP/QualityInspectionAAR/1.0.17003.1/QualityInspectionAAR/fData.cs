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
        private string rc_no = string.Empty;
        private string emp_no = string.Empty;
        private string process_id = string.Empty;
        private string node_id = string.Empty;

        private TProgramInfo programInfo = new TProgramInfo();

        public FData() : this("", "", "", "") { }

        public FData(string RC, string PROCESS_ID, string EmpNo, string NODE_ID)
        {
            InitializeComponent();

            rc_no = RC;

            emp_no = EmpNo;

            node_id = NODE_ID;

            process_id = PROCESS_ID;

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
            LbEmpNo.Text = emp_no;
            if (string.IsNullOrEmpty(rc_no))
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

            #region 取得抽樣計劃 
            LbPlan.Text = QcSrv.LoadAQL(RC_NO: rc_no,
                                        PROCESS_ID: process_id);
            #endregion

            #region 資料收集
            QcSrv.LoadInput(dgv: ref DgvData,
                            programInfo: ref programInfo,
                            RC_NO: rc_no,
                            PROCESS_ID: process_id);
            #endregion

            #region 取得工單號碼
            LbWO.Text = QcSrv.GetWO(RC_NO: rc_no);
            #endregion

            #region 取得已輸入的資料            
            QcSrv.LoadData(dgv: ref DgvLog,
                           RC_NO: rc_no,
                           PROCESS_ID: process_id);
            #endregion

            #region 取得已抽數(需先取得工單號碼)
            LbTotalInspectQty.Text = QcSrv.GetTotalData(RC_NO: rc_no,
                                                        PROCESS_ID: process_id);
            #endregion          

            #region 取得製程批量總數
            LbTotalQty.Text = QcSrv.SumTotal(RC_NO: rc_no,
                                             PROCESS_ID: process_id);
            #endregion

            #region 取得總應抽數
            LbTargetQty.Text = QcSrv.GetTotalBeSampling(TotalNum: LbTotalQty.Text,
                                                        RC_NO: rc_no,
                                                        PROCESS_ID: process_id);
            #endregion

            #region 取得 RC 已抽數
            LbRCInspectQty.Text = QcSrv.GetRCSampled(RC_NO: rc_no,
                                                     PROCESS_ID: process_id);
            #endregion

            #region 取得 RC 可抽數
            LbRCQty.Text = QcSrv.GetRCBeSampling(BeSampling: LbTargetQty.Text,
                                                 Sampled: LbTotalInspectQty.Text,
                                                 RCSampled: LbRCInspectQty.Text,
                                                 RC_NO: rc_no,
                                                 PROCESS_ID: process_id);

            if (LbRCQty.Text == "0")
            {
                LbRCQty.ForeColor = Color.Red;
            }

            #endregion

            #region 總應抽與總已抽數值比對
            if (LbTargetQty.Text == LbTotalInspectQty.Text)
            {
                LbTotalInspectQty.ForeColor = Color.Blue;
            }
            else
            {
                if (int.TryParse(LbTargetQty.Text, out int _shouldSample) &&
                    int.TryParse(LbTotalInspectQty.Text, out int _sampled))
                {
                    if (_shouldSample > _sampled)
                    {
                        LbTotalInspectQty.ForeColor = Color.Red;
                    }
                    else
                    {
                        LbTotalInspectQty.ForeColor = Color.Blue;
                    }
                }
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
            int _rcSample = Convert.ToInt32(QcSrv.GetRCBeSampling(BeSampling: LbTargetQty.Text,
                                                                  Sampled: LbTotalInspectQty.Text,
                                                                  RCSampled: LbRCInspectQty.Text,
                                                                  RC_NO: rc_no,
                                                                  PROCESS_ID: process_id));

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
                               RC_NO: rc_no,
                               NODE_ID: node_id);

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
