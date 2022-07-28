using System;
using System.Data;
using System.Windows.Forms;
using System.Data.OracleClient;
using System.Drawing;
using System.Globalization;
using CSaleOrderInput.Enums;

namespace CSaleOrderInput
{
    public partial class fDetailData : Form
    {
        private fMain fMainControl;
        public fDetailData() : this(null) { }
        public fDetailData(fMain f)
        {
            InitializeComponent();
            fMainControl = f;

            #region 只能打數字的欄位
            tbQuantity.KeyPress += NumericTextBox_KeyPress;
            tbWeight.KeyPress += NumericTextBox_KeyPress;
            tbUnitPrice.KeyPress += NumericTextBox_KeyPress;
            tbAmount.KeyPress += NumericTextBox_KeyPress;
            tbNumber1.KeyPress += NumericTextBox_KeyPress;
            tbNumber2.KeyPress += NumericTextBox_KeyPress;
            tbSequence.KeyPress += NumericTextBox_KeyPress;
            tbQuotes1.KeyPress += NumericTextBox_KeyPress;
            tbQuotes2.KeyPress += NumericTextBox_KeyPress;
            tbQuotes3.KeyPress += NumericTextBox_KeyPress;
            tbExchangeRate.KeyPress += NumericTextBox_KeyPress;
            #endregion
        }

        public UpdateType UpdateTypeEnum;
        public string g_sformText;
        public string g_sKeyID;
        public DataGridViewRow dataCurrentRow;
        string sSQL;
        DataSet dsTemp;
        bool bAppendSucess = false;

        public string Number1 { get; set; }

        public string Number2 { get; set; }

        private string m_RealDate;
        public string RealDate
        {
            set { m_RealDate = value; }
        }

        private string m_RealDueDate;
        public string RealDueDate
        {
            set { m_RealDueDate = value; }
        }

        public string Sequence
        {
            get { return tbSequence.Text; }
        }

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
                    tbNumber1.Text = Number1;
                    tbNumber2.Text = Number2;
                    tbSequence.Text = GetNextSequence(tbNumber1.Text, tbNumber2.Text);
                    dtPickerRealDate.Text = ParseDate(m_RealDate);
                    dtPickerRealDueDate.Text = ParseDate(m_RealDueDate);
                    break;

                case UpdateType.Modify:
                    SetValueToUiControl();
                    // special handle for SEQUENCE
                    tbSequence.Text = int.Parse(dataCurrentRow.Cells["SEQUENCE"].Value.ToString()).ToString("000");
                    break;

                case UpdateType.Copy:
                    SetValueToUiControl();
                    // special handle for SEQUENCE
                    tbSequence.Text = GetNextSequence(tbNumber1.Text, tbNumber2.Text);
                    break;
                default:
                    throw new NotSupportedException($"Not supported update type: {UpdateTypeEnum.ToString()}");
            }

            tbNumber1.ReadOnly = true;
            tbNumber2.ReadOnly = true;
            tbSequence.ReadOnly = true;

            // DateTimePicker 與 TextBox 同位置 1.17003.0.2
            editPickerRealDate.Location = new Point(dtPickerRealDate.Location.X, dtPickerRealDate.Location.Y);
            editPickerRealDueDate.Location = new Point(dtPickerRealDueDate.Location.X, dtPickerRealDueDate.Location.Y);
            editPickerRealDate.Width = dtPickerRealDate.Width - 20;
            editPickerRealDueDate.Width = dtPickerRealDueDate.Width - 20;
        }

        private void SetValueToUiControl()
        {
            tbNumber1.Text = dataCurrentRow.Cells["NUMBER1"].Value.ToString();
            tbNumber2.Text = dataCurrentRow.Cells["NUMBER2"].Value.ToString();
            var currentSequence = dataCurrentRow.Cells["SEQUENCE"].Value.ToString();
            //tbNumber2.Text = int.Parse(dataCurrentRow.Cells["NUMBER2"].Value.ToString()).ToString("0000");
            //var currentSequence = int.Parse(dataCurrentRow.Cells["SEQUENCE"].Value.ToString()).ToString("000");

            // get data from table
            sSQL = " Select * from SAJET.SYS_ORD_D "
                   + $" Where NUMBER1 = {tbNumber1.Text} and NUMBER2 = {tbNumber2.Text} and SEQUENCE = {currentSequence}";

            dsTemp = ClientUtils.ExecuteSQL(sSQL);
            tbQuotes1.Text = dsTemp.Tables[0].Rows[0]["QUOTES1"].ToString();
            tbQuotes2.Text = dsTemp.Tables[0].Rows[0]["QUOTES2"].ToString();
            tbQuotes3.Text = dsTemp.Tables[0].Rows[0]["QUOTES3"].ToString();
            cbCurrency.Text = dsTemp.Tables[0].Rows[0]["CURRENCY"].ToString();
            tbExchangeRate.Text = dsTemp.Tables[0].Rows[0]["EXCHANGE_RATE"].ToString();
            tbPaymentTerms.Text = dsTemp.Tables[0].Rows[0]["PAYMENT_METHOD"].ToString();
            tbAddress.Text = dsTemp.Tables[0].Rows[0]["ADDRESS"].ToString();

            tbProduceNumber.Text = dsTemp.Tables[0].Rows[0]["PRODUCE_NUMBER"].ToString();
            tbUnit.Text = dsTemp.Tables[0].Rows[0]["UNIT"].ToString();
            tbQuantity.Text = dsTemp.Tables[0].Rows[0]["QUANTITY"].ToString();
            tbWeight.Text = dsTemp.Tables[0].Rows[0]["WEIGHT"].ToString();
            tbUnitPrice.Text = dsTemp.Tables[0].Rows[0]["UNIT_PRICE"].ToString();
            tbAmount.Text = dsTemp.Tables[0].Rows[0]["AMOUNT"].ToString();

            dtPickerRealDate.Text = ParseDate(dsTemp.Tables[0].Rows[0]["REAL_DATE"].ToString());
            dtPickerRealDueDate.Text = ParseDate(dsTemp.Tables[0].Rows[0]["REAL_DUE_DATE"].ToString());
            tbClientProduceNo.Text = dsTemp.Tables[0].Rows[0]["CLIENT_PRODUCE_NUMBER"].ToString();
            tbSpec1.Text = dsTemp.Tables[0].Rows[0]["SPEC1"].ToString();
            tbSpec2.Text = dsTemp.Tables[0].Rows[0]["SPEC2"].ToString();
            tbSpec3.Text = dsTemp.Tables[0].Rows[0]["SPEC3"].ToString();

            var sql = $" Select OPTION5 from SAJET.SYS_PART Where PART_NO = '{tbProduceNumber.Text}'";
            dsTemp = ClientUtils.ExecuteSQL(sql);
            if (dsTemp != null && dsTemp.Tables[0].Rows.Count > 0)
            {
                var unitWeight = dsTemp.Tables[0].Rows[0]["OPTION5"].ToString();
                SetUnitWeightByResult(unitWeight);
            }
            else
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please check part NO (obsolete data)"), 1);
                tbProduceNumber.Text = "";
                tbProduceNumber.SelectAll();
                tbProduceNumber.Focus();
            }
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

        private string GetNextSequence(string number1, string number2)
        {
            var sql = $"select MAX(SEQUENCE) from SAJET.SYS_ORD_D where NUMBER1 = '{number1}' and NUMBER2 = '{number2}' ";
            var ds = ClientUtils.ExecuteSQL(sql);
            var current = ds.Tables[0].Rows[0][0].ToString().Trim();
            int maxNumber;
            if (string.IsNullOrEmpty(current))
            {
                maxNumber = 1;
            }
            else
            {
                maxNumber = int.Parse(current) + 1;
            }
            return maxNumber.ToString("000");
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsDataValid())
                {
                    return;
                }

                string SQL = $@"
SELECT PART_NO
FROM SAJET.SYS_PART
WHERE PART_NO = '{tbProduceNumber.Text}'
";
                DataSet set = ClientUtils.ExecuteSQL(SQL);
                if (set == null || set.Tables[0].Rows.Count <= 0)
                {
                    SajetCommon.Show_Message("Part No do not exist", 1);
                    tbProduceNumber.SelectAll();
                    tbProduceNumber.Focus();
                    return;
                }

                btnOK.Enabled = false;
                btnCancel.Enabled = false;
                Cursor = Cursors.WaitCursor;

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
                        var sMsg = SajetCommon.SetLanguage("Data Append OK") + " !" + Environment.NewLine +
                                      SajetCommon.SetLanguage("Append Other Data") + " ?";
                        if (fMainControl != null) fMainControl.ShowDetailData();
                        if (SajetCommon.Show_Message(sMsg, 2) == DialogResult.Yes)
                        {
                            ClearData();
                            editPickerRealDate.Text = dtPickerRealDate.Value.ToString("yyyy/MM/dd");
                            editPickerRealDueDate.Text = dtPickerRealDueDate.Value.ToString("yyyy/MM/dd");
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
                        if (fMainControl != null)
                        {
                            fMainControl.ShowDetailData();
                        }
                        DialogResult = DialogResult.OK;
                        break;

                    default:
                        throw new NotSupportedException($"Not supported update type: {UpdateTypeEnum.ToString()}");
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

        private bool ValidNumericalTextBox(TextBox textBox)
        {
            if (!string.IsNullOrEmpty(textBox.Text.Trim()) && !decimal.TryParse(textBox.Text.Trim(), out decimal decimalResult))
            {
                var msgQuote = SajetCommon.SetLanguage("Must be a number");
                errorProvider1.SetError(textBox, msgQuote);
                return false;
            }
            else
            {
                errorProvider1.SetError(textBox, "");
                return true;
            }
        }

        private void NumericalTextBoxTextChanged(object sender, EventArgs e)
        {
            if (sender is TextBox textBox)
            {
                errorProvider1.SetError(textBox, "");
            }
        }

        /// <summary>
        /// 檢查資料合法性
        /// </summary>
        /// <returns></returns>
        private bool IsDataValid()
        {
            if (string.IsNullOrWhiteSpace(tbNumber1.Text))
            {
                var msg = SajetCommon.SetLanguage("'Number' cannot be empty!");
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(tbNumber2.Text))
            {
                var msg = SajetCommon.SetLanguage("'Serial Number' cannot be empty!");
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (int.Parse(tbNumber2.Text.Trim()).ToString().Length > 4)
            {
                string message = SajetCommon.SetLanguage(MessageEnum.SerialNumber4DigitsMost.ToString());

                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }

            if (string.IsNullOrWhiteSpace(tbSequence.Text))
            {
                var msg = SajetCommon.SetLanguage("'Sequence' cannot be empty!");
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (int.Parse(tbSequence.Text.Trim()).ToString().Length > 3)
            {
                string message = SajetCommon.SetLanguage(MessageEnum.Sequence3DigitsMost.ToString());

                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }

            if (string.IsNullOrWhiteSpace(tbProduceNumber.Text))
            {
                var msg = SajetCommon.SetLanguage("'Produce Number' cannot be empty!");
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbProduceNumber.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(tbQuantity.Text))
            {
                var msg = SajetCommon.SetLanguage("'Quantity' cannot be empty!");
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbQuantity.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(tbAmount.Text))
            {
                try
                {
                    double.TryParse(tbQuantity.Text, out double quantityResult);
                    double.TryParse(tbUnitPrice.Text, out double unitPrice);
                    tbAmount.Text = (quantityResult * unitPrice).ToString(CultureInfo.InvariantCulture);
                }
                catch
                {
                    tbAmount.Text = "0";
                }

                // 預設 0
                //var msg = SajetCommon.SetLanguage("'Amount' cannot be empty!");
                //MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return false;
            }

            try
            {
                dtPickerRealDate.Text = DateTime.Parse(editPickerRealDate.Text).ToString("yyyy/MM/dd");
            }
            catch
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("DateTime format error"), 1);
                editPickerRealDate.SelectAll();
                editPickerRealDate.Focus();
                return false;
            }

            try
            {
                dtPickerRealDueDate.Text = DateTime.Parse(editPickerRealDueDate.Text).ToString("yyyy/MM/dd");
            }
            catch
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("DateTime format error"), 1);
                editPickerRealDueDate.SelectAll();
                editPickerRealDueDate.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(dtPickerRealDate.Text))
            {
                var msg = SajetCommon.SetLanguage("'Creation Date' cannot be empty!");
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dtPickerRealDate.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(dtPickerRealDueDate.Text))
            {
                var msg = SajetCommon.SetLanguage("'Due Date' cannot be empty!");
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dtPickerRealDueDate.Focus();
                return false;
            }

            return true;
        }

        private void AppendData()
        {
            int.TryParse(tbNumber2.Text, out int number_2);
            int.TryParse(tbSequence.Text, out int number_seq);

            object[][] Params = new object[22][];
            sSQL = @"
Insert into SAJET.SYS_ORD_D
(
    QUOTES1
   ,QUOTES2
   ,QUOTES3
   ,CURRENCY
   ,EXCHANGE_RATE
   ,PAYMENT_METHOD
   ,ADDRESS
   ,PRODUCE_NUMBER
   ,UNIT
   ,QUANTITY
   ,WEIGHT
   ,UNIT_PRICE
   ,AMOUNT
   ,REAL_DATE
   ,REAL_DUE_DATE
   ,CLIENT_PRODUCE_NUMBER
   ,SPEC1
   ,SPEC2
   ,SPEC3
   ,NUMBER1
   ,NUMBER2
   ,SEQUENCE
) 
Values 
(
    :QUOTES1
   ,:QUOTES2
   ,:QUOTES3
   ,:CURRENCY
   ,:EXCHANGE_RATE
   ,:PAYMENT_METHOD
   ,:ADDRESS
   ,:PRODUCE_NUMBER
   ,:UNIT
   ,:QUANTITY
   ,:WEIGHT
   ,:UNIT_PRICE
   ,:AMOUNT
   ,:REAL_DATE
   ,:REAL_DUE_DATE
   ,:CLIENT_PRODUCE_NUMBER
   ,:SPEC1
   ,:SPEC2
   ,:SPEC3
   ,:NUMBER1
   ,:NUMBER2
   ,:SEQUENCE
)";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "QUOTES1", tbQuotes1.Text };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "QUOTES2", tbQuotes2.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "QUOTES3", tbQuotes3.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CURRENCY", cbCurrency.Text };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "EXCHANGE_RATE", tbExchangeRate.Text };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PAYMENT_METHOD", tbPaymentTerms.Text };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ADDRESS", tbAddress.Text };
            Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PRODUCE_NUMBER", tbProduceNumber.Text };
            Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UNIT", tbUnit.Text };
            Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "QUANTITY", tbQuantity.Text };
            Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WEIGHT", tbWeight.Text };
            Params[11] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UNIT_PRICE", tbUnitPrice.Text };
            Params[12] = new object[] { ParameterDirection.Input, OracleType.VarChar, "AMOUNT", tbAmount.Text };
            Params[13] = new object[] { ParameterDirection.Input, OracleType.DateTime, "REAL_DATE", DateTime.Parse(dtPickerRealDate.Text) };
            Params[14] = new object[] { ParameterDirection.Input, OracleType.DateTime, "REAL_DUE_DATE", DateTime.Parse(dtPickerRealDueDate.Text) };
            Params[15] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CLIENT_PRODUCE_NUMBER", tbClientProduceNo.Text };
            Params[16] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SPEC1", tbSpec1.Text };
            Params[17] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SPEC2", tbSpec2.Text };
            Params[18] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SPEC3", tbSpec3.Text };
            Params[19] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NUMBER1", tbNumber1.Text };
            Params[20] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NUMBER2", number_2.ToString("0000") };
            Params[21] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SEQUENCE", number_seq.ToString("000") };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
        }
        private void ModifyData()
        {
            string sOperateID = "";
            object[][] Params = new object[22][];
            sSQL = @"
Update SAJET.SYS_ORD_D
set QUOTES1 = :QUOTES1 
   ,QUOTES2 = :QUOTES2 
   ,QUOTES3 = :QUOTES3 
   ,CURRENCY = :CURRENCY 
   ,EXCHANGE_RATE = :EXCHANGE_RATE 
   ,PAYMENT_METHOD = :PAYMENT_METHOD
   ,ADDRESS = :ADDRESS 
   ,PRODUCE_NUMBER = :PRODUCE_NUMBER 
   ,UNIT = :UNIT 
   ,QUANTITY = :QUANTITY 
   ,WEIGHT = :WEIGHT 
   ,UNIT_PRICE = :UNIT_PRICE
   ,AMOUNT = :AMOUNT 
   ,CLIENT_PRODUCE_NUMBER = :CLIENT_PRODUCE_NUMBER 
   ,REAL_DATE = :REAL_DATE 
   ,REAL_DUE_DATE = :REAL_DUE_DATE
   ,SPEC1 = :SPEC1 
   ,SPEC2 = :SPEC2 
   ,SPEC3 = :SPEC3 
where NUMBER1 = :NUMBER1
and NUMBER2 = :NUMBER2
and SEQUENCE = :SEQUENCE
";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "QUOTES1", tbQuotes1.Text };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "QUOTES2", tbQuotes2.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "QUOTES3", tbQuotes3.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CURRENCY", cbCurrency.Text };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "EXCHANGE_RATE", tbExchangeRate.Text };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PAYMENT_METHOD", tbPaymentTerms.Text };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ADDRESS", tbAddress.Text };
            Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PRODUCE_NUMBER", tbProduceNumber.Text };
            Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UNIT", tbUnit.Text };
            Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "QUANTITY", tbQuantity.Text };
            Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WEIGHT", tbWeight.Text };
            Params[11] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UNIT_PRICE", tbUnitPrice.Text };
            Params[12] = new object[] { ParameterDirection.Input, OracleType.VarChar, "AMOUNT", tbAmount.Text };
            Params[13] = new object[] { ParameterDirection.Input, OracleType.DateTime, "REAL_DATE", DateTime.Parse(dtPickerRealDate.Text) };
            Params[14] = new object[] { ParameterDirection.Input, OracleType.DateTime, "REAL_DUE_DATE", DateTime.Parse(dtPickerRealDueDate.Text) };
            Params[15] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CLIENT_PRODUCE_NUMBER", tbClientProduceNo.Text };
            Params[16] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SPEC1", tbSpec1.Text };
            Params[17] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SPEC2", tbSpec2.Text };
            Params[18] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SPEC3", tbSpec3.Text };
            Params[19] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NUMBER1", tbNumber1.Text };
            Params[20] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NUMBER2", tbNumber2.Text };
            Params[21] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SEQUENCE", tbSequence.Text };

            ClientUtils.ExecuteSQL(sSQL, Params);
        }

        private void btnProduceNumber_Click(object sender, EventArgs e)
        {
            try
            {
                btnProductNumber.Enabled = false;
                Cursor = Cursors.WaitCursor;

                fFilter_Part f = new fFilter_Part
                {
                    firstFilter = tbProduceNumber.Text
                };

                if (f.ShowDialog() == DialogResult.OK)
                {
                    tbProduceNumber.Text = f.sPART_NO;
                    string unitWeight = f.sUnitWeight;
                    SetUnitWeightByResult(unitWeight);
                }
            }
            finally
            {
                Cursor = Cursors.Default;
                btnProductNumber.Enabled = true;
            }
        }

        private void SetUnitWeightByResult(string unitWeight)
        {
            if (string.IsNullOrEmpty(unitWeight))
            {
                lblUnitWeight.ForeColor = Color.Red;
                lblUnitWeightWarning.Visible = true;
                tbUnitWeight.Text = "<" + SajetCommon.SetLanguage("Empty") + ">";
            }
            else if (!double.TryParse(unitWeight, out double result))
            {
                lblUnitWeight.ForeColor = Color.Red;
                lblUnitWeightWarning.Visible = true;
                tbUnitWeight.Text = "<" + SajetCommon.SetLanguage("Not a number") + ">";
            }
            else
            {
                lblUnitWeight.ForeColor = Color.Black;
                lblUnitWeightWarning.Visible = false;
                tbUnitWeight.Text = unitWeight;
            }
        }

        private void btnCalculateWeight_Click(object sender, EventArgs e)
        {
            if (double.TryParse(tbQuantity.Text, out double quantityResult) &&
                double.TryParse(tbUnitWeight.Text, out double unitWeightResult))
            {
                tbWeight.Text = (quantityResult * unitWeightResult).ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                var msg = SajetCommon.SetLanguage("'Unit Weight' and 'Quantity' must be numbers");
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbWeight.Text = string.Empty;
                return;
            }
        }

        private void btnCalculateAmount_Click(object sender, EventArgs e)
        {
            if (double.TryParse(tbQuantity.Text, out double quantityResult) &&
                double.TryParse(tbUnitPrice.Text, out double unitPrice))
            {
                tbAmount.Text = (quantityResult * unitPrice).ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                var msg = SajetCommon.SetLanguage("'Unit Price' and 'Quantity' must be numbers");
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbAmount.Text = string.Empty;
                return;
            }
        }

        private void ClearData()
        {
            var n1 = tbNumber1.Text;
            var n2 = tbNumber2.Text;
            var sequence = int.Parse(tbSequence.Text);

            for (int i = 0; i <= panelControl.Controls.Count - 1; i++)
            {
                if (panelControl.Controls[i] is TextBox)
                {
                    panelControl.Controls[i].Text = "";
                }
                else if (panelControl.Controls[i] is ComboBox)
                {
                    ((ComboBox)panelControl.Controls[i]).SelectedIndex = -1;
                }
            }

            tbProduceNumber.Text = "";
            tbUnit.Text = "";
            tbQuantity.Text = "";
            tbUnitWeight.Text = "";
            tbWeight.Text = "";
            tbUnitPrice.Text = "";
            tbAmount.Text = "";

            tbNumber1.Text = n1;
            tbNumber2.Text = n2;
            tbSequence.Text = (sequence + 1).ToString("000");

            dtPickerRealDate.Text = ParseDate(m_RealDate);
            dtPickerRealDueDate.Text = ParseDate(m_RealDueDate);
        }

        private void tbProduceNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnProduceNumber.PerformClick();
            }
        }

        private void dtPickerRealDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                editPickerRealDate.Text = DateTime.Parse(dtPickerRealDate.Text).ToString("yyyy/MM/dd");
            }
            catch
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("DateTime format error"), 1);
                dtPickerRealDate.Text = editPickerRealDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
            }
        }

        private void dtPickerRealDueDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                editPickerRealDueDate.Text = DateTime.Parse(dtPickerRealDueDate.Text).ToString("yyyy/MM/dd");
            }
            catch
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("DateTime format error"), 1);
                dtPickerRealDueDate.Text = editPickerRealDueDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
            }
        }

        private void editPickerRealDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter) return;
            try
            {
                dtPickerRealDate.Text = DateTime.Parse(editPickerRealDate.Text).ToString("yyyy/MM/dd");
            }
            catch
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("DateTime format error"), 1);
                editPickerRealDate.SelectAll();
                editPickerRealDate.Focus();
            }
        }

        private void editPickerRealDueDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter) return;
            try
            {
                dtPickerRealDueDate.Text = DateTime.Parse(editPickerRealDueDate.Text).ToString("yyyy/MM/dd");
            }
            catch
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("DateTime format error"), 1);
                editPickerRealDueDate.SelectAll();
                editPickerRealDueDate.Focus();
            }
        }

        /// <summary>
        /// 只能輸入數字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumericTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
    }
}