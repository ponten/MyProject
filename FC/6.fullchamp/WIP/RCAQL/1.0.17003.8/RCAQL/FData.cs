using RCAQL.Models;
using SajetClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QoSrv = RCAQL.Services.QCOptionService;

namespace RCAQL
{
    public partial class FData : Form
    {
        private string RC_NO = string.Empty;
        private string Emp_No = string.Empty;
        private Services.QCService DataCollection = new Services.QCService();
        private TProgramInfo programInfo = new TProgramInfo();

        public FData() : this("", "") { }

        public FData(string RC, string EmpNo)
        {
            InitializeComponent();

            RC_NO = RC;

            Emp_No = EmpNo;

            DataCollection.RC_NO = RC_NO;

            SajetCommon.SetLanguageControl(this);

            this.Load += FData_Load;

            dgvDataCollection.CellEndEdit += dgvDataCollection_CellEndEdit;

            btnSave.Click += btnSave_Click;

            btnClose.Click += btnClose_Click;
        }

        private void FData_Load(object sender, EventArgs e)
        {
            Form1_Load();
        }

        public void Form1_Load()
        {
            lblEmpNo.Text = Emp_No;
            if (string.IsNullOrEmpty(RC_NO))
            {
                MessageBox.Show(SajetCommon.SetLanguage("No RC_NO"),
                                SajetCommon.SetLanguage("Error"),
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
                return;
            }
            programInfo.sProgram = ClientUtils.fProgramName;
            programInfo.sFunction = "IPQC";
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
            lblSampleType.Text = DataCollection.LoadAQL();
            #endregion

            #region 資料收集
            DataCollection.LoadInput(ref dgvDataCollection, ref programInfo);
            #endregion

            #region 取得工單號碼
            lblWONumber.Text = DataCollection.GetWONumber();
            #endregion

            #region 取得已輸入的資料            
            DataCollection.LoadData(ref dgvAQLRule);
            #endregion

            #region 取得已抽數(需先取得工單號碼)
            lblSampledQuantity.Text = DataCollection.GetTotalData();
            #endregion          

            #region 取得製程批量總數
            lblTotalQuantity.Text = DataCollection.SumTotal();
            #endregion

            #region 取得總應抽數
            lblShouldBeSampledQuantity.Text = DataCollection.GetTotalBeSampling(lblTotalQuantity.Text);
            #endregion

            #region 取得 RC 已抽數
            lblRCSampledQuantity.Text = DataCollection.GetRCSampled();
            #endregion

            #region 取得 RC 可抽數
            lblRCQuantity.Text = DataCollection.GetRCBeSampling(
                lblShouldBeSampledQuantity.Text,
                lblSampledQuantity.Text,
                lblRCSampledQuantity.Text);

            if (lblRCQuantity.Text == "0")
            {
                lblRCQuantity.ForeColor = Color.Red;
            }

            #endregion

            /*
             * 設定 首件檢 / 末件檢 的製程，至少要抽一個
             * 前提是流程卡還有剩餘數量可以抽
            //*/
            if (QoSrv.MustInspect(RC_NO) &&
                lblShouldBeSampledQuantity.Text == "0")
            {
                if (int.TryParse(lblRCQuantity.Text, out int i) &&
                    i != 0)
                {
                    lblShouldBeSampledQuantity.Text = "1";
                }
            }

            #region 總應抽與總已抽數值比對
            if (lblShouldBeSampledQuantity.Text == lblSampledQuantity.Text)
            {
                lblSampledQuantity.ForeColor = Color.Blue;
            }
            else
            {
                if (int.TryParse(lblShouldBeSampledQuantity.Text, out int _shouldSample) &&
                    int.TryParse(lblSampledQuantity.Text, out int _sampled))
                {
                    if (_shouldSample > _sampled)
                    {
                        lblSampledQuantity.ForeColor = Color.Red;
                    }
                    else
                    {
                        lblSampledQuantity.ForeColor = Color.Blue;
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
        private void btnSave_Click(object sender, EventArgs e)
        {
            // 檢查抽驗資料
            int _rcSample = Convert.ToInt32(DataCollection.GetRCBeSampling(
                                            lblShouldBeSampledQuantity.Text,
                                            lblSampledQuantity.Text,
                                            lblRCSampledQuantity.Text));

            if (_rcSample > 0)
            {
                #region Isaac 模擬測試

                var value_list = new List<KeyValuePair<string, string>>();

                foreach (DataGridViewRow dr in dgvDataCollection.Rows)
                {
                    if (!string.IsNullOrEmpty(dr.Cells["VALUE_DEFAULT"].ErrorText))
                    {
                        SajetCommon.Show_Message(dr.Cells["ITEM_NAME"].EditedFormattedValue.ToString() + dr.Cells["VALUE_DEFAULT"].ErrorText, 0);

                        dgvDataCollection.CurrentCell = dr.Cells["VALUE_DEFAULT"];

                        dgvDataCollection.Focus();

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

                DataCollection.SaveData(GetEmpID(Emp_No), value_list);

                #region 重新讀取資料
                dgvDataCollection.Rows.Clear();
                Form1_Load();
                #endregion
            }
            else
            {
                MessageBox.Show(SajetCommon.SetLanguage("WriteDataError"),
                                SajetCommon.SetLanguage("Error"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 關閉視窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
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
        private void dgvDataCollection_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell cell = dgvDataCollection.Rows[e.RowIndex].Cells[e.ColumnIndex];

            cell.ErrorText = "";

            switch (dgvDataCollection.Rows[e.RowIndex].Cells["CONVERT_TYPE"].EditedFormattedValue.ToString())
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

            switch (dgvDataCollection.Rows[e.RowIndex].Cells["INPUT_TYPE"].EditedFormattedValue.ToString())
            {
                case "R":
                    {
                        string[] slValue = dgvDataCollection.Rows[e.RowIndex].Cells["VALUE_LIST"].EditedFormattedValue.ToString().Split(',');

                        decimal dMin = decimal.Parse(slValue[0]);

                        decimal dMax = decimal.Parse(slValue[1]);

                        try
                        {
                            decimal dValue = decimal.Parse(cell.EditedFormattedValue.ToString());

                            if (dValue >= dMin && dValue <= dMax) { }
                            else
                            {
                                cell.ErrorText = string.Format(SajetCommon.SetLanguage("Over flow{0}~{1}", 1), dMin, dMax);
                            }
                        }
                        catch
                        {
                            if (dgvDataCollection.Rows[e.RowIndex].Cells["NECESSARY"].EditedFormattedValue.ToString() == "Y")
                            {
                                cell.ErrorText = SajetCommon.SetLanguage("Data Invalid", 1);
                            }
                        }

                        break;
                    }
                default:
                    {
                        if (dgvDataCollection.Rows[e.RowIndex].Cells["VALUE_TYPE"].EditedFormattedValue.ToString() == "N")
                        {
                            try
                            {
                                decimal dValue = decimal.Parse(cell.EditedFormattedValue.ToString());
                            }
                            catch
                            {
                                if (dgvDataCollection.Rows[e.RowIndex].Cells["NECESSARY"].EditedFormattedValue.ToString() == "Y")
                                {
                                    cell.ErrorText = SajetCommon.SetLanguage("Data Invalid", 1);
                                }
                            }
                        }

                        break;
                    }
            }

            if (string.IsNullOrEmpty(cell.ErrorText) &&
                dgvDataCollection.Rows[e.RowIndex].Cells["NECESSARY"].EditedFormattedValue.ToString() == "Y" &&
                string.IsNullOrWhiteSpace(cell.EditedFormattedValue.ToString()))
            {
                cell.ErrorText = SajetCommon.SetLanguage("Data Empty", 1);
            }
        }
    }
}
