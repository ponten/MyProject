using System;
using System.Data;
using System.Windows.Forms;
using System.Data.OracleClient;
using System.Collections.Generic;
using CSaleOrderInput.Enums;
using SajetClass;

namespace CSaleOrderInput
{
    public partial class fData : Form
    {
        private fMain fMainControl;
        public fData() : this(null) { }
        public fData(fMain f)
        {
            InitializeComponent();
            fMainControl = f;

            #region 只能輸入數字
            tbNumber1.KeyPress += NumericTextBox_KeyPress;
            tbNumber2.KeyPress += NumericTextBox_KeyPress;
            #endregion
        }

        public UpdateType UpdateTypeEnum;
        public string g_sformText;
        public DataGridViewRow dataCurrentRow;
        string To;
        public string Number1
        {
            get { return tbNumber1.Text; }
        }
        public string Number2
        {
            get { return tbNumber2.Text; }
        }

        private string sSQL;
        private readonly string OPERATION_ID = "211";
        private readonly string OPERATION_NAME = "銷售訂單";

        private void fData_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
            panel2.BackgroundImage = ClientUtils.LoadImage("ImgButton.jpg");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            BackgroundImage = ClientUtils.LoadImage("ImgMain.jpg");
            BackgroundImageLayout = ImageLayout.Stretch;
            Text = g_sformText;


            switch (UpdateTypeEnum)
            {
                case UpdateType.Append:
                    tbOperationId.Text = OPERATION_ID;
                    tbOperationName.Text = OPERATION_NAME;
                    tbNumber1.Text = DateTime.Now.ToString("yyyyMM");
                    tbNumber2.Text = GetNextNumber2(tbNumber1.Text);
                    break;
                case UpdateType.Modify:
                    SetValueToUiControl();
                    tbNumber2.Text = dataCurrentRow.Cells["NUMBER2"].Value.ToString();
                    break;
                case UpdateType.Copy:
                    SetValueToUiControl();
                    // special handle for NUMBER2
                    var number1 = dataCurrentRow.Cells["NUMBER1"].Value.ToString();
                    tbNumber2.Text = GetNextNumber2(number1);
                    break;
                default:
                    throw new NotSupportedException($"Not supported update type: {UpdateTypeEnum.ToString()}");
            }

            // DateTimePicker 與 TextBox 同位置 1.17003.0.2
            editRealDate.Location = new System.Drawing.Point(dtRealDate.Location.X, dtRealDate.Location.Y);
            editRealDueDate.Location = new System.Drawing.Point(dtRealDueDate.Location.X, dtRealDueDate.Location.Y);
            editRealDate.Width = dtRealDate.Width - 20;
            editRealDueDate.Width = dtRealDueDate.Width - 20;
        }

        private void SetValueToUiControl()
        {
            tbOperationId.Text = dataCurrentRow.Cells["OPERATION_ID"].Value.ToString();
            tbOperationName.Text = dataCurrentRow.Cells["OPERATION_NAME"].Value.ToString();
            tbNumber1.Text = dataCurrentRow.Cells["NUMBER1"].Value.ToString();

            tbCustomerId.Text = dataCurrentRow.Cells["CUSTOMER_ID"].Value.ToString();
            tbCustomerName.Text = dataCurrentRow.Cells["CUSTOMER_NAME"].Value.ToString();
            tbAccountId.Text = dataCurrentRow.Cells["ACCOUNT_ID"].Value.ToString();
            tbAccountName.Text = dataCurrentRow.Cells["ACCOUNT_NAME"].Value.ToString();
            tbToSendId.Text = dataCurrentRow.Cells["TO_SEND_ID"].Value.ToString();
            tbToSendName.Text = dataCurrentRow.Cells["TO_SEND_NAME"].Value.ToString();
            tbSaleId.Text = dataCurrentRow.Cells["SALE_ID"].Value.ToString();
            tbSaleName.Text = dataCurrentRow.Cells["SALE_NAME"].Value.ToString();

            dtRealDate.Text = ParseDate(dataCurrentRow.Cells["REAL_DATE"].Value.ToString());
            dtRealDueDate.Text = ParseDate(dataCurrentRow.Cells["REAL_DUE_DATE"].Value.ToString());
            tbCustomize.Text = dataCurrentRow.Cells["CUSTOMIZE"].Value.ToString();
            tbNumber3.Text = dataCurrentRow.Cells["NUMBER3"].Value.ToString();
        }

        private string ParseDate(string aDate)
        {
            if (DateTime.TryParse(aDate, out DateTime dateTime))
            {
                return dateTime.ToString("yyyy/MM/dd");
            }
            else
            {
                return DateTime.Now.ToString("yyyy/MM/dd");
            }

        }

        // get 4-dig serial number 
        private string GetNextNumber2(string number1)
        {
            var sql = $"select MAX(NUMBER2) from SAJET.SYS_ORD_H where NUMBER1 = '{number1}'";
            var ds = ClientUtils.ExecuteSQL(sql);
            var current = ds.Tables[0].Rows[0][0].ToString().Trim();
            int nextNumber;
            if (string.IsNullOrEmpty(current))
            {
                nextNumber = 1;
            }
            else
            {
                nextNumber = int.Parse(current) + 1;
            }
            return nextNumber.ToString("0000");
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                btnOK.Enabled = false;
                btnCancel.Enabled = false;
                Cursor = Cursors.WaitCursor;

                if (!IsDataValid())
                {
                    return;
                }

                for (int i = 0; i <= panelControl.Controls.Count - 1; i++)
                {
                    if (panelControl.Controls[i] is TextBox)
                    {
                        panelControl.Controls[i].Text = panelControl.Controls[i].Text.Trim();
                    }
                }

                //Update DB
                switch (UpdateTypeEnum)
                {
                    case UpdateType.Append:
                        AppendData();
                        string sMsg = SajetCommon.SetLanguage("Data Append OK") + " !" + Environment.NewLine +
                                      SajetCommon.SetLanguage("Append Other Data") + " ?";
                        if (fMainControl != null)
                        {
                            fMainControl.ShowData(); //新增後即時顯示新增資料在表格上
                        }
                        if (SajetCommon.Show_Message(sMsg, 2) == DialogResult.Yes)
                        {
                            ClearData();
                            return;
                        }
                        DialogResult = DialogResult.OK;
                        break;
                    case UpdateType.Modify:
                        ModifyData();
                        DialogResult = DialogResult.OK;
                        break;
                    case UpdateType.Copy:
                        // Show confirm dialog 
                        if (SajetCommon.Show_Message(Text + " ?", 2) != DialogResult.Yes)
                            return;
                        AppendData();
                        AppendDetail();
                        if (fMainControl != null)
                        {
                            fMainControl.ShowData(); //新增後即時顯示新增資料在表格上
                        }
                        DialogResult = DialogResult.OK;
                        break;
                    default:
                        throw new NotSupportedException($"Not supported update type: {UpdateTypeEnum}");
                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception : " + ex.Message, 0);
            }
            finally
            {
                Cursor = Cursors.Default;
                btnCancel.Enabled = true;
                btnOK.Enabled = true;
            }
        }

        private bool IsDataValid()
        {
            if (string.IsNullOrEmpty(tbNumber1.Text.Trim()))
            {
                var msg = SajetCommon.SetLanguage("'Number' cannot be empty!");
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                if (!DateTime.TryParseExact(tbNumber1.Text.Trim(),
                                           "yyyyMM",
                                           null,
                                           System.Globalization.DateTimeStyles.None,
                                           out DateTime d2))
                {
                    var msg = SajetCommon.SetLanguage(MessageEnum.Require6DigitsYearMonthNumber.ToString());
                    MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            if (string.IsNullOrEmpty(tbNumber2.Text.Trim()))
            {
                var msg = SajetCommon.SetLanguage("'Serial Number' cannot be empty!");
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                int.TryParse(tbNumber2.Text, out int number_2);

                if (number_2.ToString().Length > 4)
                {
                    var msg = SajetCommon.SetLanguage(MessageEnum.SerialNumber4DigitsMost.ToString());
                    MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            #region  檢查銷售主檔的號碼、當月流水碼是否重複

            if (UpdateTypeEnum == UpdateType.Append || UpdateTypeEnum == UpdateType.Copy)
            {
                string SQL = @"
SELECT *
FROM SAJET.SYS_ORD_H
WHERE NUMBER1 = :NUMBER1
AND NUMBER2 = :NUMBER2
";
                object[][] param = new object[][]
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "NUMBER1", tbNumber1.Text },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "NUMBER2", tbNumber2.Text },
                };
                DataSet ds = ClientUtils.ExecuteSQL(SQL, param);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Duplicate number/serial number pair"), 1);
                    tbNumber2.SelectAll();
                    tbNumber2.Focus();
                    return false;
                }
            }

            #endregion

            try
            {
                dtRealDate.Text = DateTime.Parse(editRealDate.Text).ToString("yyyy/MM/dd");
            }
            catch
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("DateTime format error"), 1);
                editRealDate.SelectAll();
                editRealDate.Focus();
                return false;
            }

            try
            {
                dtRealDueDate.Text = DateTime.Parse(editRealDueDate.Text).ToString("yyyy/MM/dd");
            }
            catch
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("DateTime format error"), 1);
                editRealDueDate.SelectAll();
                editRealDueDate.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(dtRealDate.Text.Trim()))
            {
                var msg = SajetCommon.SetLanguage("'Creation Date' cannot be empty!");
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            if (string.IsNullOrEmpty(dtRealDueDate.Text.Trim()))
            {
                var msg = SajetCommon.SetLanguage("'Due Date' cannot be empty!");
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 新增銷售訂單的主檔
        /// </summary>
        private void AppendData()
        {
            object[][] Params = new object[16][];
            sSQL = @"
Insert into
    SAJET.SYS_ORD_H
(
    OPERATION_ID
   ,OPERATION_NAME
   ,CUSTOMER_ID
   ,CUSTOMER_NAME
   ,ACCOUNT_ID
   ,ACCOUNT_NAME
   ,TO_SEND_ID
   ,TO_SEND_NAME
   ,SALE_ID
   ,SALE_NAME
   ,REAL_DATE
   ,REAL_DUE_DATE
   ,CUSTOMIZE
   ,NUMBER3
   ,FLAG
   ,NUMBER1
   ,NUMBER2
)
Values
(
    :OPERATION_ID
   ,:OPERATION_NAME
   ,:CUSTOMER_ID
   ,:CUSTOMER_NAME
   ,:ACCOUNT_ID
   ,:ACCOUNT_NAME
   ,:TO_SEND_ID
   ,:TO_SEND_NAME
   ,:SALE_ID
   ,:SALE_NAME
   ,:REAL_DATE
   ,:REAL_DUE_DATE
   ,:CUSTOMIZE
   ,:NUMBER3
   ,'Y'
   ,:NUMBER1
   ,:NUMBER2
)";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPERATION_ID", tbOperationId.Text };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPERATION_NAME", tbOperationName.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_ID", tbCustomerId.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_NAME", tbCustomerName.Text };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ACCOUNT_ID", tbAccountId.Text };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ACCOUNT_NAME", tbAccountName.Text };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TO_SEND_ID", tbToSendId.Text };
            Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TO_SEND_NAME", tbToSendName.Text };
            Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SALE_ID", tbSaleId.Text };
            Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SALE_NAME", tbSaleName.Text };
            Params[10] = new object[] { ParameterDirection.Input, OracleType.DateTime, "REAL_DATE", DateTime.Parse(dtRealDate.Text) };
            Params[11] = new object[] { ParameterDirection.Input, OracleType.DateTime, "REAL_DUE_DATE", DateTime.Parse(dtRealDueDate.Text) };
            Params[12] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMIZE", tbCustomize.Text };
            Params[13] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NUMBER3", tbNumber3.Text };
            Params[14] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NUMBER1", tbNumber1.Text };
            Params[15] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NUMBER2", tbNumber2.Text };

            ClientUtils.ExecuteSQL(sSQL, Params);

            // TODO: need History???
            //fMain.CopyToHistory(sMaxID);
        }

        /// <summary>
        /// 複製銷售訂單的主檔也要繼承該訂單的銷售明細
        /// </summary>
        private void AppendDetail()
        {


            string s = @"
INSERT INTO sajet.sys_ord_d (
    number1,
    number2,
    quotes1,
    quotes2,
    quotes3,
    currency,
    exchange_rate,
    payment_method,
    address,
    produce_number,
    unit,
    quantity,
    weight,
    unit_price,
    amount,
    real_date,
    real_due_date,
    client_produce_number,
    spec1,
    spec2,
    spec3,
    sequence
)
    SELECT
        :new_number1,
        :new_number2,
        quotes1,
        quotes2,
        quotes3,
        currency,
        exchange_rate,
        payment_method,
        address,
        produce_number,
        unit,
        quantity,
        weight,
        unit_price,
        amount,
        :real_date,
        :real_due_date,
        client_produce_number,
        spec1,
        spec2,
        spec3,
        sequence
    FROM
        sajet.sys_ord_d
    WHERE
        number1 = :prev_number1
        AND number2 = :prev_number2
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "new_number1", tbNumber1.Text },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "new_number2", tbNumber2.Text },
                new object[] { ParameterDirection.Input, OracleType.DateTime, "real_date", DateTime.Parse(dtRealDate.Text) },
                new object[] { ParameterDirection.Input, OracleType.DateTime, "real_due_date", DateTime.Parse(dtRealDueDate.Text) },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "prev_number1", dataCurrentRow.Cells["NUMBER1"].Value.ToString() },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "prev_number2", dataCurrentRow.Cells["NUMBER2"].Value.ToString() },
            };

            ClientUtils.ExecuteSQL(s, p.ToArray());
        }

        private void ModifyData()
        {
           
            SendEmail se;
            object[][] Params = new object[16][];
            var sql = @"
Update
    SAJET.SYS_ORD_H
set
    OPERATION_ID = :OPERATION_ID
   ,OPERATION_NAME = :OPERATION_NAME
   ,CUSTOMER_ID = :CUSTOMER_ID
   ,CUSTOMER_NAME = :CUSTOMER_NAME
   ,ACCOUNT_ID = :ACCOUNT_ID
   ,ACCOUNT_NAME = :ACCOUNT_NAME
   ,TO_SEND_ID = :TO_SEND_ID
   ,TO_SEND_NAME = :TO_SEND_NAME
   ,SALE_ID = :SALE_ID
   ,SALE_NAME = :SALE_NAME
   ,REAL_DATE = :REAL_DATE
   ,REAL_DUE_DATE = :REAL_DUE_DATE
   ,CUSTOMIZE = :CUSTOMIZE
   ,NUMBER3 = :NUMBER3
where
    NUMBER1 = :NUMBER1
    and NUMBER2 = :NUMBER2
";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPERATION_ID", tbOperationId.Text };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPERATION_NAME", tbOperationName.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_ID", tbCustomerId.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER_NAME", tbCustomerName.Text };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ACCOUNT_ID", tbAccountId.Text };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ACCOUNT_NAME", tbAccountName.Text };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TO_SEND_ID", tbToSendId.Text };
            Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TO_SEND_NAME", tbToSendName.Text };
            Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SALE_ID", tbSaleId.Text };
            Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SALE_NAME", tbSaleName.Text };
            Params[10] = new object[] { ParameterDirection.Input, OracleType.DateTime, "REAL_DATE", DateTime.Parse(dtRealDate.Text) };
            Params[11] = new object[] { ParameterDirection.Input, OracleType.DateTime, "REAL_DUE_DATE", DateTime.Parse(dtRealDueDate.Text) };
            Params[12] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMIZE", tbCustomize.Text };
            Params[13] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NUMBER3", tbNumber3.Text };
            Params[14] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NUMBER1", tbNumber1.Text };
            Params[15] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NUMBER2", tbNumber2.Text };
            ClientUtils.ExecuteSQL(sql, Params);

            // TODO: need History???
            //fMain.CopyToHistory(g_sKeyID);



            //發信通知

            sSQL = string.Format(@" select * from  sajet.sys_role_emp sre
    join sajet.sys_emp se on sre.emp_id=se.emp_id
    where role_id='10000006'");

            DataSet ds = ClientUtils.ExecuteSQL(sSQL);

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    To += ds.Tables[0].Rows[i]["EMAIL"].ToString() + ";";
                }

                string Body = "資料異動";

                string Title = SajetCommon.SetLanguage("[MES] SaleOrder Notify") + ":" + "資料異動";  //2022                               

                se = new SendEmail(To, Body, Title);

                se.Send();
            }

        }

        private void btnSelectCustomer_Click(object sender, EventArgs e)
        {
            SelectCustomer(sender);
        }

        private void btnSelectSale_Click(object sender, EventArgs e)
        {
            SelectSales(sender);
        }

        private void SelectCustomer(object sender)
        {
            var sql = "select CUSTOMER_CODE, CUSTOMER_NAME from SAJET.SYS_CUSTOMER";
            var orderBy = "CUSTOMER_CODE";
            InitializeFilterForm(sql, orderBy, sender);
        }

        private void SelectSales(object sender)
        {
            var sql = "select EMP_NO, EMP_NAME from SAJET.SYS_EMP";
            var orderBy = "EMP_NO";
            InitializeFilterForm(sql, orderBy, sender);
        }

        private void InitializeFilterForm(string sql, string orderBy, object sender)
        {
            var btnName = ((Button)sender).Name;
            fFilter f = new fFilter(sql, orderBy);
            if (f.ShowDialog() == DialogResult.OK)
            {
                if (Enum.TryParse(btnName, true, out ButtonName result))
                {
                    if (f.dgvData.CurrentRow == null)
                    {
                        throw new NullReferenceException($"Cannot find data from {f.Text} Form.");
                    }
                    switch (result)
                    {
                        case ButtonName.btnSelectCustomer:
                            tbCustomerId.Text = f.dgvData.CurrentRow.Cells["CUSTOMER_CODE"].Value.ToString();
                            tbCustomerName.Text = f.dgvData.CurrentRow.Cells["CUSTOMER_NAME"].Value.ToString();
                            break;
                        case ButtonName.btnSelectAccount:
                            tbAccountId.Text = f.dgvData.CurrentRow.Cells["CUSTOMER_CODE"].Value.ToString();
                            tbAccountName.Text = f.dgvData.CurrentRow.Cells["CUSTOMER_NAME"].Value.ToString();
                            break;
                        case ButtonName.btnSelectToSend:
                            tbToSendId.Text = f.dgvData.CurrentRow.Cells["CUSTOMER_CODE"].Value.ToString();
                            tbToSendName.Text = f.dgvData.CurrentRow.Cells["CUSTOMER_NAME"].Value.ToString();
                            break;
                        case ButtonName.btnSelectSale:
                            tbSaleId.Text = f.dgvData.CurrentRow.Cells["EMP_NO"].Value.ToString();
                            tbSaleName.Text = f.dgvData.CurrentRow.Cells["EMP_NAME"].Value.ToString();
                            break;
                    }
                }
                else
                {
                    throw new NotSupportedException($"Unsupported button name {btnName}");
                }
            }
        }

        private void ClearData()
        {
            tbOperationId.Text = OPERATION_ID;
            tbOperationName.Text = OPERATION_NAME;
            tbNumber1.Text = DateTime.Now.ToString("yyyyMM");
            tbNumber2.Text = GetNextNumber2(tbNumber1.Text);

            tbCustomerId.Text = "";
            tbCustomerName.Text = "";
            tbAccountId.Text = "";
            tbAccountName.Text = "";
            tbToSendId.Text = "";
            tbToSendName.Text = "";
            tbSaleId.Text = "";
            tbSaleName.Text = "";

            dtRealDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
            dtRealDueDate.Text = DateTime.Now.ToString("yyyy/MM/dd");

            tbCustomize.Text = "";
            tbNumber3.Text = "";
        }

        private void dtRealDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                editRealDate.Text = DateTime.Parse(dtRealDate.Text).ToString("yyyy/MM/dd");
            }
            catch
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("DateTime format error"), 1);
                dtRealDate.Text = editRealDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
            }
        }

        private void dtRealDueDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                editRealDueDate.Text = DateTime.Parse(dtRealDueDate.Text).ToString("yyyy/MM/dd");
            }
            catch
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("DateTime format error"), 1);
                dtRealDueDate.Text = editRealDueDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
            }
        }

        private void editRealDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter) return;
            try
            {
                dtRealDate.Text = DateTime.Parse(editRealDate.Text).ToString("yyyy/MM/dd");
            }
            catch
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("DateTime format error"), 1);
                editRealDate.SelectAll();
                editRealDate.Focus();
            }
        }

        private void editRealDueDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter) return;
            try
            {
                dtRealDueDate.Text = DateTime.Parse(editRealDueDate.Text).ToString("yyyy/MM/dd");
            }
            catch
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("DateTime format error"), 1);
                editRealDueDate.SelectAll();
                editRealDueDate.Focus();
            }
        }

        /// <summary>
        /// 只能輸入正整數
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumericTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
    }

    public enum ButtonName
    {
        btnSelectCustomer,
        btnSelectAccount,
        btnSelectToSend,
        btnSelectSale
    }
}