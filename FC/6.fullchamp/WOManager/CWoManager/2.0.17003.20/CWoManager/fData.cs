using SajetClass;
using SajetFilter;
using SajetTable;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using OtSrv = CWoManager.Services.OtherService;

namespace CWoManager
{
    public partial class fData : Form
    {
        public List<string> g_sControl = new List<string>();
        public DataGridViewColumnCollection dataGridColumn;
        public DataGridViewRow dataCurrentRow;

        public string NewWorkOrder = string.Empty;
        public string g_sFactory;
        public string g_sKeyID;
        public string g_sPartID;
        public string g_sUpdateType;
        public string g_sformText;

        private static string strNumber3 = string.Empty;
        private static string strProduceNumber = string.Empty;

        private Dictionary<int, int> g_slColumn = new Dictionary<int, int>();
        private Dictionary<string, fMain.TDBInitial> g_DBInitial = new Dictionary<string, fMain.TDBInitial>();
        private Bitmap bitmap;
        private DataSet DrLstN1N2 = null;

        private bool cmb = false;
        private int g_iWoStatus;

        private string m_WoCache = string.Empty;
        private string sSQL = string.Empty;
        private string strCustomer = string.Empty;
        private string strCustomer1 = string.Empty;
        private string strCustomer2 = string.Empty;
        private string strCustomize = string.Empty;
        private string strDate = string.Empty;
        private string strDueDate = string.Empty;
        private string strOutsource = string.Empty;
        private string strSaleID = string.Empty;
        private string strSpec1 = string.Empty;
        private string strSpec2 = string.Empty;
        private string strSpec3 = string.Empty;
        private string strTotalNum = string.Empty;
        private string strUnit = string.Empty;

        public struct TControlData
        {
            public bool bNecessary;
            public string strNecessary;
            public string strType;
            public string strField;
            public string strPartField;
            public TextBox txtControl;
            public List<string> ddlValue;
            public ComboBox ddlControl;
            public CheckBox chkControl;
            public DateTimePicker calExtender;
            public Label lablControl;
            public RichTextBox richControl;
        }
        public TControlData[] tControlAdd;

        public class cboMadeCategoryList
        {
            public string cbo_Name { get; set; }

            public string cbo_Value { get; set; }
        }

        public fData(Dictionary<string, fMain.TDBInitial> DBInitial)
        {
            InitializeComponent();
            g_DBInitial = DBInitial;
            CreateTableControl();
            ComboInitial();
            DateTimePickerInitial();

            //TbBookNo.KeyPress += NumericTextBox_KeyPress;
            txtSequence.KeyPress += NumericTextBox_KeyPress;
            txtPallets.KeyPress += NumericTextBox_KeyPress;
            editTargetQty.KeyPress += NumericTextBox_KeyPress;
        }

        private void fData_Load(object sender, EventArgs e)
        {
            combOperation.Select();

            SajetCommon.SetLanguageControl(this);
            //this.CreateOptionControl();

            LVPkSPec.Columns.Clear();

            editPart.BackColor = SystemColors.Control;
            editPart.Enabled = false;

            tbWO.BackColor = SystemColors.Control;
            tbWO.Enabled = false;

            txtBad.BackColor = SystemColors.Control;
            txtBad.Enabled = false;

            txtBlueprint.BackColor = SystemColors.Control;
            txtBlueprint.Enabled = false;

            txtClient.BackColor = SystemColors.Control;
            txtClient.Enabled = false;

            txtClient2.BackColor = SystemColors.Control;
            txtClient2.Enabled = false;

            txtCustomize.BackColor = SystemColors.Control;
            txtCustomize.Enabled = false;

            txtFinish.BackColor = SystemColors.Control;
            txtFinish.Enabled = false;

            txtNotPaid.BackColor = SystemColors.Control;
            txtNotPaid.Enabled = false;

            txtOldNum.BackColor = SystemColors.Control;
            txtOldNum.Enabled = false;

            txtSales.BackColor = SystemColors.Control;
            txtSales.Enabled = false;

            //txtSequence.BackColor = SystemColors.Control;
            //txtSequence.Enabled = false;

            txtUni.BackColor = SystemColors.Control;
            txtUni.Enabled = false;

            txtUnit.BackColor = SystemColors.Control;
            txtUnit.Enabled = false;

            string[] strArray1;
            string[] strArray2;

            if (g_DBInitial.ContainsKey("Packing Spec Title"))
            {
                strArray1 = g_DBInitial["Packing Spec Title"].sValue.ToString().Split(',');
                strArray2 = g_DBInitial["Packing Spec Title"].sDefault.ToString().Split(',');
            }
            else
            {
                strArray1 = new string[4]
                {
                    "PKSPEC_NAME",
                    "BOX_QTY",
                    "CARTON_QTY",
                    "PALLET_QTY"
                };

                strArray2 = new string[4] { "200", "90", "90", "90" };
            }

            for (int index = 0; index < strArray1.Length; ++index)
            {
                LVPkSPec.Columns.Add(new ColumnHeader()
                {
                    Name = strArray1[index],
                    Text = strArray1[index],
                    Width = int.Parse(strArray2[index])
                });
            }

            SajetCommon.SetLanguageControl(this);

            Text = g_sformText;

            LabWoStatus.Text = string.Empty;
            dtScheduleDate.Value = DateTime.Today;
            dtDueDate.Value = DateTime.Today;

            combWoRule.Items.Clear();
            combWoRule.Items.Add("");

            string s = @"
SELECT
    function_name
FROM
    sajet.sys_module_param
WHERE
    module_name = 'W/O RULE'
GROUP BY
    function_name
ORDER BY
    function_name
";
            DataSet dataSet1 = ClientUtils.ExecuteSQL(s);

            for (int index = 0; index <= dataSet1.Tables[0].Rows.Count - 1; ++index)
            {
                combWoRule.Items.Add(dataSet1.Tables[0].Rows[index]["function_name"].ToString());
            }

            var p = new List<object[]>
            {
                new object[4] { ParameterDirection.Input, OracleType.VarChar, "factory_id", fMain.g_sFactoryID }
            };

            combWoType.Items.Clear();
            combWoType.Items.Add("");

            s = @"
SELECT
    param_value
FROM
    sajet.sys_base_param
WHERE
    program = 'W/O Manager'
    AND param_name = 'W/O Type'
    AND ROWNUM = 1
";
            DataSet dataSet2 = ClientUtils.ExecuteSQL(s);

            if (dataSet2.Tables[0].Rows.Count > 0)
            {
                string[] strArray3 = dataSet2.Tables[0].Rows[0]["param_value"].ToString().Split(',');

                for (int index = 0; index < strArray3.Length - 1; ++index)
                {
                    combWoType.Items.Add(strArray3[index].Trim());
                }
            }

            combLine.Items.Clear();
            combLine.Items.Add("");

            s = @"
SELECT
    pdline_id,
    pdline_name
FROM
    sajet.sys_pdline
WHERE
    enabled = 'Y'
    AND factory_id = :factory_id
ORDER BY
    pdline_name
";
            DataSet dataSet3 = ClientUtils.ExecuteSQL(s, p.ToArray());

            for (int index = 0; index <= dataSet3.Tables[0].Rows.Count - 1; ++index)
            {
                combLine.Items.Add(dataSet3.Tables[0].Rows[index]["pdline_name"].ToString());
            }

            dataSet3.Dispose();

            #region Append

            if (g_sUpdateType == "APPEND")
            {
                LabWoStatus.Text = "Prepare";

                SetOrderSeqList();
                SetNextSequence();
                SetWorkOrder();
                SetCustomerAndSales();
                SetCustDate();
                //SetOrderDetail();
                //SetBomRelation();
                SetPartNo();
                //SetPalletVersionRouteOldNoUnitBlueprint();
            }

            #endregion Append

            #region Modify

            if (g_sUpdateType == "MODIFY" || g_sUpdateType == "VIEW")
            {
                cmb = true;
                g_sKeyID = dataCurrentRow.Cells[TableDefine.gsDef_KeyField].Value.ToString();
                string str1 = dataCurrentRow.Cells["WOSTATUS"].Value.ToString();
                LabWoStatus.Text = str1;

                tbWO.Text = dataCurrentRow.Cells["WORK_ORDER"].Value.ToString();
                editPart.Text = dataCurrentRow.Cells["PART_NO"].Value.ToString();

                //Parse WorkOrder
                string workOrder = dataCurrentRow.Cells["WORK_ORDER"].Value.ToString();
                string operationId = workOrder.Substring(0, 3); // 10A, 10B, 10C
                string aNumber1 = workOrder.Substring(3, 6); // 201612
                string aNumber2 = workOrder.Substring(9, 4); //0013
                string aSequence = workOrder.Substring(13, 2); //00, 01, 02
                string aOrderSeq = workOrder.Substring(15, 1); //1,2,3
                //end Parse WorkOrder

                string operationIdStr = operationId;
                string numberDate = aNumber1;
                string number = aNumber2;
                string number3 = aSequence + aOrderSeq;

                cbNumber1.SelectedIndex = cbNumber1.Items.IndexOf(numberDate);

                cbNumber2.DataSource = new string[] { number };
                cbNumber2.SelectedIndex = 0;

                txtNum.Text = number3;
                txtSequence.Text = number3.Substring(0, 2);
                txtSequence.Enabled = false;
                cbOrderSequence.Items.Clear();
                cbOrderSequence.Items.Add(number3.Substring(2, 1));
                cbOrderSequence.SelectedIndex = 0;
                cbOrderSequence.Enabled = false;
                //this.ComboInitial();
                combOperation.SelectedIndex = combOperation.Items.IndexOf(operationIdStr);

                s = @"
SELECT
    number1,
    LISTAGG(number2, ',') WITHIN GROUP(
        ORDER BY
            number1
    ) AS number2
FROM
    sajet.sys_ord_h
GROUP BY
    number1
ORDER BY
    number1
";
                ClientUtils.ExecuteSQL(s);

                txtClient.Text = dataCurrentRow.Cells["CUSTOMER1"].Value.ToString();
                txtClient2.Text = dataCurrentRow.Cells["CUSTOMER3"].Value.ToString();
                txtSales.Text = dataCurrentRow.Cells["SALES"].Value.ToString();
                txtCustomize.Text = dataCurrentRow.Cells["CUSTOMIZE"].Value.ToString();
                txtProductRate.Text = dataCurrentRow.Cells["PRODUCT_RATE"].Value.ToString();
                txtOutsource.Text = dataCurrentRow.Cells["OUTSOURCE"].Value.ToString();

                TbBookNo.Text = dataCurrentRow.Cells["WO_OPTION2"].Value.ToString();

                var madeCate = dataCurrentRow.Cells["MADE_CATEGORY"].Value.ToString();

                combMade.SelectedIndex = ConvertMadeCategoryToIndex(madeCate) - 1; // minus 1 to make the SelectedIndex align with the Value in the Table

                txtRule1.Text = dataCurrentRow.Cells["RULE1"].Value.ToString();

                string str3 = dataCurrentRow.Cells["TARGET_QTY"].Value.ToString();
                editTargetQty.Text = str3;

                if (str1.Equals("Complete"))
                {
                    string str4 = dataCurrentRow.Cells["SCRAP_QTY"].Value.ToString();
                    int int32_1 = Convert.ToInt32(str3);
                    int int32_2 = Convert.ToInt32(str4);
                    txtBad.Text = string.Concat(int32_2);
                    txtFinish.Text = string.Concat(int32_1 - int32_2);
                    txtNotPaid.Text = "0";
                }
                else
                {
                    txtBad.Text = string.Concat(0);
                    txtFinish.Text = string.Concat(0);
                    txtNotPaid.Text = str3;
                }

                DateTime dateTime1 = Convert.ToDateTime(dataCurrentRow.Cells["CUSTOMER_DATE"].Value.ToString());
                dtCustDate.Value = dateTime1.Date;

                DateTime dateTime2 = Convert.ToDateTime(dataCurrentRow.Cells["CUSTOMER_DUE_DATE"].Value.ToString());
                dtCustDueDate.Value = dateTime2.Date;

                txtPallets.Text = dataCurrentRow.Cells["PALLETS"].Value.ToString();

                DateTime dateTime3 = Convert.ToDateTime(dataCurrentRow.Cells["WO_SCHEDULE_DATE"].Value.ToString());
                dtScheduleDate.Value = dateTime3.Date;

                DateTime dateTime4 = Convert.ToDateTime(dataCurrentRow.Cells["WO_DUE_DATE"].Value.ToString());
                dtDueDate.Value = dateTime4.Date;

                // BOM relation
                //10C (F022500...)
                string producenumber = dataCurrentRow.Cells["PRODUCE_NUMBER"].Value.ToString(); // F

                cbProd10C.Items.Clear();
                cbProd10C.Items.Add(producenumber);
                cbProd10C.SelectedIndex = 0;
                //SetCombProduce(producenumber, operationIdStr);

                string prod10Bstr = dataCurrentRow.Cells["PRODUCENO2"].Value.ToString();

                if (!string.IsNullOrEmpty(prod10Bstr))
                {
                    cbProd10B.DataSource = new string[] { prod10Bstr };
                    cbProd10B.SelectedIndex = 0;
                }
                else
                {
                    cbProd10B.DataSource = null;
                }

                string prod10Astr = dataCurrentRow.Cells["PRODUCENO1"].Value.ToString();

                if (!string.IsNullOrEmpty(prod10Astr))
                {
                    cbProd10A.DataSource = new string[] { prod10Astr };
                    cbProd10A.SelectedIndex = 0;
                }
                else
                {
                    cbProd10A.DataSource = null;
                }

                txtUnit.Text = dataCurrentRow.Cells["UNIT"].Value.ToString();
                ListViewOrderDetail1(operationIdStr, numberDate, number, number3, editTargetQty.Text, g_sUpdateType, "");

                //  ListViewProduceDetail
                ListViewProduceDetail(getPartNo(dataCurrentRow.Cells["PART_ID"].Value.ToString()), dataCurrentRow.Cells["ROUTE_NAME"].Value.ToString());

                if (!dataCurrentRow.Cells["PART_ID"].Value.ToString().Equals(""))
                {
                    combRoute.Items.Clear();
                    combRoute.Items.Add("");

                    s = @"
SELECT
    route_id,
    route_name
FROM
    sajet.sys_rc_route
WHERE
    enabled = 'Y'
    AND route_id IN (
        SELECT
            route_id
        FROM
            sajet.sys_part_route
        WHERE
            part_id = :part_id
    )
ORDER BY
    route_name
";
                    p = new List<object[]>
                    {
                        new object[4] { ParameterDirection.Input, OracleType.VarChar, "part_id", dataCurrentRow.Cells["PART_ID"].Value.ToString() }
                    };

                    DataSet dataSet4 = ClientUtils.ExecuteSQL(s, p.ToArray());

                    for (int index = 0; index <= dataSet4.Tables[0].Rows.Count - 1; ++index)
                    {
                        combRoute.Items.Add(dataSet4.Tables[0].Rows[index]["route_name"].ToString());
                    }

                    dataSet4.Dispose();
                }

                string versionStr = dataCurrentRow.Cells["VERSION"].Value.ToString();
                txtVersion.Text = versionStr;

                txtVersion.Text = dataCurrentRow.Cells["VERSION"].Value.ToString();

                string woRule = dataCurrentRow.Cells["WO_RULE"].Value.ToString();
                combWoRule.Text = woRule;

                combWoType.SelectedIndex = combWoType.Items.IndexOf(dataCurrentRow.Cells["WO_TYPE"].Value.ToString());

                if (g_DBInitial.ContainsKey("TARGET_QTY") &&
                    !string.IsNullOrEmpty(g_DBInitial["TARGET_QTY"].sValue))
                {
                    editTargetQty.Text = dataCurrentRow.Cells[g_DBInitial["TARGET_QTY"].sValue].Value.ToString();
                }
                else
                {
                    editTargetQty.Text = dataCurrentRow.Cells["TARGET_QTY"].Value.ToString();
                }

                combLine.SelectedIndex =
                    combLine.Items.IndexOf(dataCurrentRow.Cells["PDLINE_NAME"].Value.ToString());

                combRoute.SelectedIndex =
                    combRoute.Items.IndexOf(dataCurrentRow.Cells["ROUTE_NAME"].Value.ToString());

                editRemark.Text = dataCurrentRow.Cells["REMARK"].Value.ToString();

                if (!string.IsNullOrEmpty(dataCurrentRow.Cells["WO_SCHEDULE_DATE"].Value.ToString()))
                {
                    dtScheduleDate.Value = DateTime.Parse(dataCurrentRow.Cells["WO_SCHEDULE_DATE"].Value.ToString());
                }

                if (!string.IsNullOrEmpty(dataCurrentRow.Cells["WO_DUE_DATE"].Value.ToString()))
                {
                    dtDueDate.Value = DateTime.Parse(dataCurrentRow.Cells["WO_DUE_DATE"].Value.ToString());
                }

                ShowWOPackSpecData(g_sKeyID);

                txtOldNum.Text = dataCurrentRow.Cells["OLD_NUMBER"].Value.ToString();
                txtUnit.Text = dataCurrentRow.Cells["UNIT"].Value.ToString();
                txtBlueprint.Text = dataCurrentRow.Cells["BLUEPRINT"].Value.ToString();

                if (g_sUpdateType == "MODIFY")
                {
                    cbNumber1.Enabled = false;
                    cbNumber2.Enabled = false;

                    cbProd10A.Enabled = false;
                    cbProd10B.Enabled = false;
                    cbProd10C.Enabled = false;

                    comProduceNo.Enabled = false;

                    combOperation.Enabled = false;

                    combRoute.Enabled = false;

                    combWoRule.Enabled = false;
                    combWoType.Enabled = false;

                    dtCustDate.Enabled = false;
                    dtCustDueDate.Enabled = false;

                    editTargetQty.Enabled = false;

                    tbWO.BackColor = SystemColors.Control;
                    tbWO.Enabled = false;

                    txtBlueprint.BackColor = SystemColors.Control;
                    txtBlueprint.Enabled = false;

                    txtNum.BackColor = SystemColors.Control;
                    txtNum.Enabled = false;

                    txtOldNum.BackColor = SystemColors.Control;
                    txtOldNum.Enabled = false;

                    txtPallets.BackColor = SystemColors.Control;
                    txtPallets.Enabled = false;

                    txtRule1.BackColor = SystemColors.Control;
                    txtRule1.Enabled = false;

                    txtSequence.BackColor = SystemColors.Control;
                    txtSequence.Enabled = false;

                    txtUnit.BackColor = SystemColors.Control;
                    txtUnit.Enabled = false;

                    txtVersion.BackColor = SystemColors.Control;
                    txtVersion.Enabled = false;

                    string woStatus = dataCurrentRow.Cells["WOSTATUS"].Value.ToString();

                    g_iWoStatus = ConvertToStatusIndex(woStatus);

                    if (g_iWoStatus == 0)
                    {
                        Get_Part_Default_Data();
                    }

                    woRule = dataCurrentRow.Cells["WO_RULE"].Value.ToString();
                    combWoRule.Text = woRule;

                    txtVersion.Text = versionStr;

                    if (g_iWoStatus >= 2) // release, WIP, hold, cancel, complete, delete
                    {
                        editPart.BackColor = SystemColors.Control;
                        editPart.Enabled = false;

                        if (g_iWoStatus != 4)
                        {
                            editTargetQty.Enabled = false;
                        }

                        cbNumber1.Enabled = false;
                        cbNumber2.Enabled = false;

                        cbProd10A.Enabled = false;
                        cbProd10B.Enabled = false;
                        cbProd10C.Enabled = false;

                        comProduceNo.Enabled = false;

                        combOperation.Enabled = false;

                        combRoute.Enabled = false;

                        combWoRule.Enabled = false;
                        combWoType.Enabled = false;

                        dtCustDate.Enabled = false;
                        dtCustDueDate.Enabled = false;

                        txtNum.BackColor = SystemColors.Control;
                        txtNum.Enabled = false;

                        txtVersion.BackColor = SystemColors.Control;
                        txtVersion.Enabled = false;
                    }

                    // 如果工單沒有任何流程卡紀錄，可以修改數量
                    {
                        bool found_any_runcard_record = OtSrv.FindRuncardRecords(workOrder);

                        editTargetQty.Enabled = !found_any_runcard_record;
                    }

                    if (fMain.g_iPrivilege == 0)
                    {
                        btnOK.Enabled = false;

                        cbNumber1.Enabled = false;
                        cbNumber2.Enabled = false;

                        cbProd10A.Enabled = false;
                        cbProd10B.Enabled = false;
                        cbProd10C.Enabled = false;

                        comProduceNo.Enabled = false;

                        combMade.Enabled = false;

                        combOperation.Enabled = false;

                        dtCustDate.Enabled = false;
                        dtCustDueDate.Enabled = false;

                        txtBad.BackColor = SystemColors.Control;
                        txtBad.Enabled = false;

                        txtBlueprint.BackColor = SystemColors.Control;
                        txtBlueprint.Enabled = false;

                        txtClient.BackColor = SystemColors.Control;
                        txtClient.Enabled = false;

                        txtClient2.BackColor = SystemColors.Control;
                        txtClient2.Enabled = false;

                        txtCustomize.BackColor = SystemColors.Control;
                        txtCustomize.Enabled = false;

                        txtFinish.BackColor = SystemColors.Control;
                        txtFinish.Enabled = false;

                        txtNotPaid.BackColor = SystemColors.Control;
                        txtNotPaid.Enabled = false;

                        txtNum.BackColor = SystemColors.Control;
                        txtNum.Enabled = false;

                        txtOldNum.BackColor = SystemColors.Control;
                        txtOldNum.Enabled = false;

                        txtOutsource.BackColor = SystemColors.Control;
                        txtOutsource.Enabled = false;

                        txtPallets.BackColor = SystemColors.Control;
                        txtPallets.Enabled = false;

                        txtProductRate.BackColor = SystemColors.Control;
                        txtProductRate.Enabled = false;

                        txtRule1.BackColor = SystemColors.Control;
                        txtRule1.ReadOnly = true;

                        txtSales.BackColor = SystemColors.Control;
                        txtSales.Enabled = false;

                        txtSequence.BackColor = SystemColors.Control;
                        txtSequence.Enabled = false;

                        txtUnit.BackColor = SystemColors.Control;
                        txtUnit.Enabled = false;
                    }
                    else if (fMain.g_iPrivilege == 1)
                    {
                        btnOK.Enabled = false;

                        cbNumber1.Enabled = false;
                        cbNumber2.Enabled = false;

                        cbProd10A.Enabled = false;
                        cbProd10B.Enabled = false;
                        cbProd10C.Enabled = false;

                        comProduceNo.Enabled = false;

                        combLine.Enabled = true;

                        combMade.Enabled = false;

                        combOperation.Enabled = false;

                        combRoute.Enabled = true;

                        dtCustDate.Enabled = false;
                        dtCustDueDate.Enabled = false;

                        txtBad.BackColor = SystemColors.Control;
                        txtBad.Enabled = false;

                        txtBlueprint.BackColor = SystemColors.Control;
                        txtBlueprint.Enabled = false;

                        txtClient.BackColor = SystemColors.Control;
                        txtClient.Enabled = false;

                        txtClient2.BackColor = SystemColors.Control;
                        txtClient2.Enabled = false;

                        txtCustomize.BackColor = SystemColors.Control;
                        txtCustomize.Enabled = false;

                        txtFinish.BackColor = SystemColors.Control;
                        txtFinish.Enabled = false;

                        txtNotPaid.BackColor = SystemColors.Control;
                        txtNotPaid.Enabled = false;

                        txtNum.BackColor = SystemColors.Control;
                        txtNum.Enabled = false;

                        txtOldNum.BackColor = SystemColors.Control;
                        txtOldNum.Enabled = false;

                        txtOutsource.BackColor = SystemColors.Control;
                        txtOutsource.Enabled = false;

                        txtPallets.BackColor = SystemColors.Control;
                        txtPallets.Enabled = false;

                        txtProductRate.BackColor = SystemColors.Control;
                        txtProductRate.Enabled = false;

                        txtRule1.BackColor = SystemColors.Control;
                        txtRule1.ReadOnly = true;

                        txtSales.BackColor = SystemColors.Control;
                        txtSales.Enabled = false;

                        txtSequence.BackColor = SystemColors.Control;
                        txtSequence.Enabled = false;

                        txtUnit.BackColor = SystemColors.Control;
                        txtUnit.Enabled = false;
                    }
                }
                else if (g_sUpdateType == "VIEW")
                {
                    btnOK.Enabled = false;

                    cbNumber1.Enabled = false;
                    cbNumber2.Enabled = false;

                    cbProd10A.Enabled = false;
                    cbProd10B.Enabled = false;
                    cbProd10C.Enabled = false;

                    comProduceNo.Enabled = false;

                    combMade.Enabled = false;

                    combOperation.Enabled = false;

                    dtCustDate.Enabled = false;
                    dtCustDueDate.Enabled = false;

                    txtBad.Enabled = false;

                    txtBlueprint.Enabled = false;

                    txtClient.Enabled = false;

                    txtClient2.Enabled = false;

                    txtCustomize.Enabled = false;

                    txtFinish.Enabled = false;

                    txtNotPaid.Enabled = false;

                    txtNum.Enabled = false;

                    txtOldNum.Enabled = false;

                    txtOutsource.Enabled = false;

                    txtPallets.Enabled = false;

                    txtProductRate.Enabled = false;

                    txtRule1.Enabled = false;

                    txtSales.Enabled = false;

                    txtUnit.Enabled = false;
                }
            }

            #endregion Modify

            if (LVPkSPec.Items.Count > 0)
            {
                LVPkSPec.Items[0].Selected = true;
            }

            labProd10B.Parent = null;
        }

        private void fData_Shown(object sender, EventArgs e)
        {
            // 規則組合預設選第一項非空白選項
            if (combWoRule.Items.Count > 1)
            {
                combWoRule.SelectedIndex = 1;
            }

            // 鎖定 線別 和 工單類型，預設成第一項非空白選項 2.0.17003.7
            if (combLine.Items.Count > 1)
            {
                combLine.SelectedIndex = 1;
            }
            if (combWoType.Items.Count > 1)
            {
                combWoType.SelectedIndex = 1;
            }

            combLine.Enabled = false;
            //combWoType.Enabled = false;
        }

        private void btnTemp_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            fFilter fFilter = new fFilter
            {
                sSQL = button.Tag.ToString()
            };
            if (fFilter.ShowDialog() != DialogResult.OK)
                return;
            tControlAdd[int.Parse(button.AccessibleName)].txtControl.Text = fFilter.dgvData.CurrentRow.Cells[tControlAdd[int.Parse(button.AccessibleName)].strField].Value.ToString();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (g_sUpdateType == "APPEND")
            {
                SetWorkOrder();
            }

            if (string.IsNullOrEmpty(tbWO.Text))
            {
                int num = (int)SajetCommon.Show_Message(SajetCommon.SetLanguage("Data is null") + Environment.NewLine + LabWO.Text, 0);
                tbWO.Focus();
                tbWO.SelectAll();
                return;
            }

            // 不卡線別 2.0.17003.7
            //if (string.IsNullOrEmpty(combLine.Text.Trim()))
            //{
            //    int num1 = (int)SajetCommon.Show_Message(SajetCommon.SetLanguage("Data is null") + Environment.NewLine + LabLine.Text, 0);
            //    return;
            //}

            if (!IsInputValid())
                return;

            string str1 = " select * from SAJET.G_WO_BASE where WORK_ORDER = :WORK_ORDER ";
            object[][] array = new object[1][]
            {
                new object[4]
                {
                    ParameterDirection.Input,
                    OracleType.VarChar,
                    "WORK_ORDER",
                    tbWO.Text
                }
            };
            if (g_sUpdateType == "MODIFY")
            {
                str1 += " and WORK_ORDER <> :OLD_WO ";
                Array.Resize(ref array, 2);
                array[1] = new object[4]
                {
                    ParameterDirection.Input,
                    OracleType.VarChar,
                    "OLD_WO",
                    g_sKeyID
                };
            }
            DataSet dataSet = ClientUtils.ExecuteSQL(str1, array);
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                int num2 = (int)SajetCommon.Show_Message(SajetCommon.SetLanguage("Data Duplicate") + Environment.NewLine + (LabWO.Text + " : " + tbWO.Text), 0);
                tbWO.Focus();
                tbWO.SelectAll();
                dataSet.Dispose();
            }
            else
            {
                try
                {

                    if (g_sUpdateType == "APPEND")
                    {
                        AppendData();
                        NewWorkOrder = tbWO.Text;
                        UpdateOtherTables();
                        DialogResult = DialogResult.OK;
                    }
                    else if (g_sUpdateType == "MODIFY")
                    {
                        ModifyData();
                        UpdateOtherTables();
                        DialogResult = DialogResult.OK;
                    }
                }
                catch (Exception ex)
                {
                    int num2 = (int)SajetCommon.Show_Message("Exception : " + ex.Message, 0);
                }
                finally
                {
                    dataSet.Dispose();
                }
            }
        }

        private void btnBOM_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(editPart.Text))
                return;
            string id = GetID("SAJET.SYS_PART", "PART_ID", "PART_NO", editPart.Text);
            string text = txtVersion.Text;
            if (id == "0")
                return;
            fWoBom fWoBom = new fWoBom();
            fWoBom.LabWO.Text = tbWO.Text;
            fWoBom.LabPartNo.Text = editPart.Text;
            fWoBom.LabVer.Text = text;
            fWoBom.g_sPartID = id;
            if (g_iWoStatus == 3 || g_iWoStatus > 4)
            {
                fWoBom.LabType.Visible = true;
                fWoBom.TreeBomData.AllowDrop = false;
                fWoBom.LVPart.AllowDrop = false;
                fWoBom.MenuItemDelete.Visible = false;
            }
            else
                fWoBom.LabType.Visible = false;
            fWoBom.ShowBom(id, text);
            int num = (int)fWoBom.ShowDialog();
            fWoBom.Dispose();
        }

        private void btnSearchPart_Click(object sender, EventArgs e)
        {
            string str = " select part_no,part_id,spec1,spec2  from sajet.sys_part  where enabled = 'Y'  and part_no Like '" + editPart.Text + "%'  order by part_no ";
            fFilter fFilter = new fFilter
            {
                sSQL = str
            };
            if (fFilter.ShowDialog() != DialogResult.OK)
                return;
            editPart.Text = fFilter.dgvData.CurrentRow.Cells["part_no"].Value.ToString();
            KeyPressEventArgs e1 = new KeyPressEventArgs('\r');
            editPart_KeyPress(sender, e1);
        }

        private void combRoute_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                SetPartNo();
                string SQL = $@"
SELECT B.ROUTE_NAME
FROM SAJET.SYS_PART A
    ,SAJET.SYS_RC_ROUTE B
WHERE A.ROUTE_ID = B.ROUTE_ID
AND A.PART_NO = '{editPart.Text}'
";
                string route_name = ClientUtils.ExecuteSQL(SQL).Tables[0].Rows[0]["ROUTE_NAME"].ToString();
                ListViewProduceDetail(editPart.Text, route_name);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void combWoRule_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                Color color1 = Color.FromArgb(byte.MaxValue, byte.MaxValue, 128);
                txtVersion.BackColor = Color.White;
                combLine.BackColor = Color.White;
                editRemark.BackColor = Color.White;
                Color white = Color.White;
                if (tControlAdd != null)
                {
                    for (int index = 0; index < tControlAdd.Length; ++index)
                    {
                        Color color2 = tControlAdd[index].bNecessary ? color1 : Color.White;
                        string strType = tControlAdd[index].strType;
                        if (!(strType == "1") && !(strType == "2"))
                        {
                            if (!(strType == "3"))
                            {
                                if (!(strType == "5"))
                                {
                                    if (strType == "7")
                                        tControlAdd[index].richControl.BackColor = color2;
                                    else
                                        tControlAdd[index].txtControl.BackColor = color2;
                                }
                                else
                                    tControlAdd[index].chkControl.BackColor = color2;
                            }
                            else
                                tControlAdd[index].calExtender.BackColor = color2;
                        }
                        else
                            tControlAdd[index].ddlControl.BackColor = color2;
                    }
                }

                DataSet dataSet = ClientUtils.ExecuteSQL(
                    "Select PARAME_ITEM,PARAME_VALUE\r\n                From SAJET.SYS_MODULE_PARAM \r\n                Where MODULE_NAME = 'W/O RULE' \r\n                and FUNCTION_NAME = :FUNCTION_NAME \r\n                and PARAME_NAME = 'Necessary Information' ",
                    new object[1][]
                    {
                    new object[4]
                    {
                         ParameterDirection.Input,
                         OracleType.VarChar,
                         "FUNCTION_NAME",
                         combWoRule.Text
                    }
                    });
                foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables[0].Rows)
                {
                    string upper = row["PARAME_ITEM"].ToString().ToUpper();
                    if (!(row["PARAME_VALUE"].ToString().ToUpper() != "Y"))
                    {
                        string str = upper;
                        if (!(str == "VERSION"))
                        {
                            if (!(str == "WO TYPE"))
                            {
                                if (!(str == "LINE"))
                                {
                                    if (str == "REMARK")
                                    {
                                        editRemark.BackColor = color1;
                                    }
                                    else
                                    {
                                        if (tControlAdd != null)
                                        {
                                            for (int index = 0; index < tControlAdd.Length; ++index)
                                            {
                                                if (tControlAdd[index].strNecessary == upper)
                                                {
                                                    string strType = tControlAdd[index].strType;
                                                    if (!(strType == "1") && !(strType == "2"))
                                                    {
                                                        if (!(strType == "3"))
                                                        {
                                                            if (!(strType == "5"))
                                                            {
                                                                if (strType == "7")
                                                                {
                                                                    tControlAdd[index].richControl.BackColor = color1;
                                                                    break;
                                                                }

                                                                tControlAdd[index].txtControl.BackColor = color1;
                                                                break;
                                                            }

                                                            tControlAdd[index].chkControl.BackColor = color1;
                                                            break;
                                                        }

                                                        tControlAdd[index].calExtender.BackColor = color1;
                                                        break;
                                                    }

                                                    tControlAdd[index].ddlControl.BackColor = color1;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                    combLine.BackColor = color1;
                            }
                            else
                                combWoType.BackColor = color1;
                        }
                        else
                            txtVersion.BackColor = color1;
                    }
                }
                dataSet.Dispose();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void cbProd10C_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (g_sUpdateType == "MODIFY")
            {
                return;
            }

            if (cbProd10C.SelectedValue == null)
            {
                cbProd10B.SelectedIndex = -1;

                return;
            }

            if (!(combOperation.SelectedValue.ToString() == "10C") || string.IsNullOrEmpty((string)cbProd10C.SelectedValue))
                return;

            SetPartNo();
        }

        private void cbProd10B_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (g_sUpdateType == "MODIFY")
            {
                return;
            }

            if (cbProd10B.SelectedValue == null)
            {
                cbProd10A.SelectedIndex = -1;

                return;
            }

            //if ((this.combOperation.SelectedValue.ToString() == "10A") ||
            //     string.IsNullOrEmpty((string)this.cbProd10B.SelectedValue))
            {
                DataSet dataSet = ClientUtils.ExecuteSQL(ItemPartNoSQL(cbProd10B.SelectedValue.ToString()));
                List<string> stringList2 = new List<string>();
                for (int index = 0; index <= dataSet.Tables[0].Rows.Count - 1; ++index)
                {
                    string str = dataSet.Tables[0].Rows[index]["ITEM_PART_NO"].ToString();
                    //if (str.IndexOf('B') == 0)
                    stringList2.Add(str);
                }
                cbProd10A.DataSource = stringList2;
            }

            SetPartNo();
        }

        private void cbProd10A_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (g_sUpdateType == "MODIFY")
            {
                return;
            }

            if ((cbProd10A.SelectedValue == null) || (cbProd10B.SelectedValue == null))
            {
                return;
            }

            if (!string.IsNullOrEmpty((string)cbProd10A.SelectedValue))
            {
                SetPart(cbProd10A.SelectedValue.ToString());
                Get_Part_Default_Data();
            }
            else if (combOperation.SelectedValue.ToString() == "10B")
            {
                DataSet dataSet = ClientUtils.ExecuteSQL(ItemPartNoSQL(cbProd10B.SelectedValue.ToString()));
                for (int index = 0; index <= dataSet.Tables[0].Rows.Count - 1; ++index)
                {
                    string str = dataSet.Tables[0].Rows[index]["ITEM_PART_NO"].ToString();
                    if (string.IsNullOrEmpty(str))
                    {
                        int num = (int)MessageBox.Show("無此對應料號:" + cbProd10B.SelectedValue.ToString() + "請至物料清單建置");
                        break;
                    }
                    //if (str.IndexOf('B') == 0)
                    {
                        if (!string.IsNullOrEmpty(cbProd10B.SelectedValue.ToString()))
                        {
                            SetPart(cbProd10B.SelectedValue.ToString());
                            Get_Part_Default_Data();
                        }
                        else
                        {
                            editPart.Text = "";
                            combRoute.Items.Clear();
                            //this.txtVersion2.Items.Clear();
                            txtVersion.Text = "";
                            combWoRule.Items.Clear();
                            combWoType.Items.Clear();
                        }
                    }
                }
            }
            else if (combOperation.SelectedValue.ToString() == "10C")
            {
                if (!string.IsNullOrEmpty(cbProd10B.SelectedValue.ToString()))
                {
                    SetPart(cbProd10B.SelectedValue.ToString());
                    Get_Part_Default_Data();
                }
                else
                {
                    editPart.Text = "";
                    combRoute.Items.Clear();

                    txtVersion.Text = "";
                    combWoRule.Items.Clear();
                    combWoType.Items.Clear();
                }
            }
            else
            {
                editPart.Text = "";
                combRoute.Items.Clear();

                txtVersion.Text = "";
                combWoRule.Items.Clear();
                combWoType.Items.Clear();
            }

            SetPartNo();
        }

        // 10A, 10B
        private void combOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (g_sUpdateType == "MODIFY")
                {
                    return;
                }

                Cursor = Cursors.WaitCursor;

                SetOrderSeqList();
                SetNextSequence();
                SetWorkOrder();
                SetCustomerAndSales();
                SetCustDate();
                //SetOrderDetail();
                //SetBomRelation();
                SetPartNo();
                //SetPalletVersionRouteOldNoUnitBlueprint();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        // 201613, 201811
        private void combDateTimeNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (g_sUpdateType == "MODIFY")
                {
                    return;
                }

                Cursor = Cursors.WaitCursor;

                string[] number2Array = DrLstN1N2.Tables[0].Select("number1 =" + cbNumber1.SelectedValue)[0][1].ToString().Split(',');
                List<string> number2List = new List<string>();
                foreach (var number2 in number2Array)
                {
                    string n2 = int.Parse(number2).ToString("0000");
                    number2List.Add(n2);
                }

                cbNumber2.DataSource = number2List.ToArray();

                SetOrderSeqList();
                SetNextSequence();
                SetWorkOrder();
                SetCustomerAndSales();
                SetCustDate();
                //SetOrderDetail();
                //SetBomRelation();
                SetPartNo();
                //SetPalletVersionRouteOldNoUnitBlueprint();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        // 0001, 0002, 0013
        private void combNum1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (g_sUpdateType == "MODIFY")
                {
                    return;
                }
                Cursor = Cursors.WaitCursor;
                SetOrderSeqList();
                SetNextSequence();
                SetWorkOrder();
                SetCustomerAndSales();
                SetCustDate();
                //SetOrderDetail();
                //SetBomRelation();
                SetPartNo();
                //SetPalletVersionRouteOldNoUnitBlueprint();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        //1, 2, 3...
        private void cbOrderSequence_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (g_sUpdateType == "MODIFY")
                {
                    return;
                }
                Cursor = Cursors.WaitCursor;
                SetNextSequence();
                SetWorkOrder();
                SetCustDate();
                SetOrderDetail();
                SetBomRelation();
                SetPartNo();
                SetPalletVersionRouteOldNoUnitBlueprint();
                CheckBOMComboBox();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void editPart_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\r')
                return;
            if (GetID("SAJET.SYS_PART", "PART_ID", "PART_NO", editPart.Text) == "0")
            {
                int num = (int)SajetCommon.Show_Message("Part No Error", 0);
                editPart.Focus();
                editPart.SelectAll();
            }
            else
                Get_Part_Default_Data();
        }

        private void editTargetQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\r' || string.IsNullOrEmpty(txtNum.Text))
                return;
            fData.strNumber3 = txtNum.Text;
            ListViewOrderDetail1(combOperation.SelectedValue.ToString(), cbNumber1.SelectedValue.ToString(), cbNumber2.SelectedValue.ToString(), fData.strNumber3, editTargetQty.Text, g_sUpdateType, "ClickTargetQTY");
        }

        private void MenuAppend_Click(object sender, EventArgs e)
        {
            SajetCommon.Show_Message("不支援此功能", 1);
        }

        private void MenuRemove_Click(object sender, EventArgs e)
        {

        }

        private void txtNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\r')
                return;
            //this.CHK_WR();
        }

        private void txtPallets_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar));
        }

        private void NumericTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void CreateTableControl()
        {
            string s = @"
SELECT
    position,
    COUNT(position)
FROM
    sajet.sys_program_fun_maintain
WHERE
    program = :program
    AND fun_name = :fun_name
GROUP BY
    position
ORDER BY
    position
";
            var p = new List<object[]>
            {
                new object[4] { ParameterDirection.Input, OracleType.VarChar, "program", fMain.g_sProgram },
                new object[4] { ParameterDirection.Input, OracleType.VarChar, "fun_name", fMain.g_sFunction }
            };

            var dataSet = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (dataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables[0].Rows)
                {
                    int key = int.Parse(row[0].ToString());
                    int num = int.Parse(row[1].ToString());

                    switch (key)
                    {
                        case 1:
                        case 2:
                            g_slColumn.Add(key, 7);
                            break;
                        default:
                            g_slColumn.Add(key, 0);
                            break;
                    }
                }
            }

            dataSet.Dispose();
        }

        private int ConvertMadeCategoryToIndex(string category)
        {
            switch (category)
            {
                case "試製":
                    return 1;
                case "樣試":
                    return 2;
                case "量試":
                    return 3;
                case "量產":
                    return 4;
                case "重工":
                    return 5;
                default:
                    throw new Exception($"Unsupported made category: {category}");
            }
        }

        private int ConvertToStatusIndex(string woStatus)
        {
            int index = 0;
            string status = woStatus.ToLower();
            switch (status)
            {
                case "initial":
                    index = 0;
                    break;
                case "prepare":
                    index = 1;
                    break;
                case "release":
                    index = 2;
                    break;
                case "wip":
                case "work in process":
                    index = 3;
                    break;
                case "hold":
                    index = 4;
                    break;
                case "cancel":
                    index = 5;
                    break;
                case "complete":
                    index = 6;
                    break;
                case "delete":
                    index = 7;
                    break;
                default:
                    throw new Exception($"Unsupported work order status {status}");

            }

            return index;
        }

        private string ConvertToStatusString(int statusIndex)
        {
            string statusString = string.Empty;
            switch (statusIndex)
            {
                case 0:
                    statusString = "Initial";
                    break;
                case 1:
                    statusString = "Prepare";
                    break;
                case 2:
                    statusString = "release";
                    break;
                case 3:
                    statusString = "Work In Process";
                    break;
                case 4:
                    statusString = "Hold";
                    break;
                case 5:
                    statusString = "Cancel";
                    break;
                case 6:
                    statusString = "Complete";
                    break;
                case 7:
                    statusString = "Delete";
                    break;
                default:
                    throw new Exception($"Unsupported work order status index {statusIndex}");
            }

            return statusString;
        }

        private void SetNextSequence()
        {
            if (g_sUpdateType == "MODIFY")
            {
                return;
            }

            var operationValue = combOperation.SelectedValue; // 10A, 10B, 10C
            var number1Value = cbNumber1.SelectedValue; // 201612
            var number2Value = cbNumber2.SelectedValue; // 0013
            var orderSeqValue = cbOrderSequence.SelectedItem; //1,2,3...
            if (operationValue != null && number1Value != null && number2Value != null && orderSeqValue != null)
            {
                var operation = operationValue.ToString(); // 10A, 10B, 10C
                var dateNumber = number1Value.ToString(); // 201612
                var number1 = number2Value.ToString(); // 0013
                var orderSeq = orderSeqValue.ToString(); // 1,2,
                if (g_sUpdateType == "APPEND")
                {
                    txtSequence.Text = CalculateNextSequence(operation, dateNumber, number1, orderSeq);
                }
            }
        }

        private void SetOrderSeqList()
        {
            if (g_sUpdateType == "MODIFY")
            {
                return;
            }

            var number1Value = cbNumber1.SelectedValue; // 201612
            var number2Value = cbNumber2.SelectedValue; // 0013
            if (number1Value != null && number2Value != null)
            {
                var dateNumber = number1Value.ToString(); // 201612
                var number1 = number2Value.ToString(); // 0013

                if (g_sUpdateType == "APPEND")
                {
                    var orderSeqList = GetOrderSeqList(dateNumber, number1).ToArray();
                    cbOrderSequence.Items.Clear();
                    cbOrderSequence.Items.AddRange(orderSeqList);
                    if (orderSeqList.Length > 0)
                    {
                        cbOrderSequence.SelectedIndex = 0;
                    }
                }
            }
        }

        private void SetWorkOrder()
        {
            if (g_sUpdateType == "MODIFY")
            {
                return;
            }

            tbWO.Text = string.Empty;
            var operationValue = combOperation.SelectedValue; // 10A, 10B, 10C
            var number1Value = cbNumber1.SelectedValue; // 201612
            var number2Value = cbNumber2.SelectedValue; // 0013
            var sequenceValue = string.IsNullOrEmpty(txtSequence.Text) ? null : txtSequence.Text; // 00, 01, 02...
            var orderSeqValue = cbOrderSequence.SelectedItem?.ToString();  // 1, 2, 3 ... 9

            if (operationValue != null && number1Value != null && number2Value != null && sequenceValue != null && orderSeqValue != null)
            {
                var operation = operationValue.ToString(); // 10A, 10B, 10C
                var dateNumber = number1Value.ToString(); // 201612
                var number1 = number2Value.ToString(); // 0013
                var sequence = sequenceValue; // 00, 01, 02...
                var orderSeq = orderSeqValue;

                tbWO.Text = operation + dateNumber + number1 + sequence + orderSeq;
                m_WoCache = tbWO.Text;
            }
        }

        private void SetOrderDetail()
        {
            var operationValue = combOperation.SelectedValue; // 10A, 10B, 10C
            var number1Value = cbNumber1.SelectedValue; // 201612
            var number2Value = cbNumber2.SelectedValue; // 0013
            var sequenceValue = string.IsNullOrEmpty(txtSequence.Text) ? null : txtSequence.Text; // 00, 01, 02...
            var orderSeqValue = cbOrderSequence.SelectedItem?.ToString();  // 1, 2, 3 ... 9

            if (operationValue != null && number1Value != null && number2Value != null && sequenceValue != null && orderSeqValue != null)
            {
                ListViewOrderDetail1(operationValue.ToString(), number1Value.ToString(), number2Value.ToString(), sequenceValue + orderSeqValue, editTargetQty.Text,
                  g_sUpdateType, "");
            }
            else
            {
                listViewOrderDetail.Items.Clear();
            }
        }

        private void SetCustomerAndSales()
        {
            if (g_sUpdateType == "MODIFY")
            {
                return;
            }

            txtClient.Text = string.Empty;
            txtClient2.Text = string.Empty;
            txtSales.Text = string.Empty;
            txtCustomize.Text = string.Empty;

            var number1Value = cbNumber1.SelectedValue; // 201612
            var number2Value = cbNumber2.SelectedValue; // 0013
            if (number1Value != null && number2Value != null)
            {
                var sql = @"select CUSTOMER_ID, CUSTOMER_NAME, SALE_ID, CUSTOMIZE from SAJET.SYS_ORD_H  " +
                          "where TO_NUMBER(NUMBER1) =('" + number1Value + "') and " +
                          "TO_NUMBER(NUMBER2) = TO_NUMBER('" + number2Value + "')";

                DataSet dataSet = ClientUtils.ExecuteSQL(sql);
                var row = dataSet.Tables[0].Rows[0];
                txtClient.Text = row["CUSTOMER_ID"].ToString();
                txtClient2.Text = row["CUSTOMER_NAME"].ToString();
                txtSales.Text = row["SALE_ID"].ToString();
                txtCustomize.Text = row["CUSTOMIZE"].ToString();
            }
        }

        private void SetCustDate()
        {
            if (g_sUpdateType == "MODIFY")
            {
                return;
            }

            var number1Value = cbNumber1.SelectedValue; // 201612
            var number2Value = cbNumber2.SelectedValue; // 0013
            var orderSeqValue = cbOrderSequence.SelectedItem?.ToString();  // 1, 2, 3 ... 9

            if (number1Value != null && number2Value != null && orderSeqValue != null)
            {
                DataSet dataSet = ClientUtils.ExecuteSQL("select SAJET.SYS_ORD_D.REAL_DATE REAL_DATE, SAJET.SYS_ORD_D.REAL_DUE_DATE REAL_DUE_DATE from SAJET.SYS_ORD_H  left join SAJET.SYS_ORD_D on TO_NUMBER(SAJET.SYS_ORD_H.NUMBER1) = TO_NUMBER(SAJET.SYS_ORD_D.NUMBER1) and TO_NUMBER(SAJET.SYS_ORD_H.NUMBER2) = TO_NUMBER(SAJET.SYS_ORD_D.NUMBER2) " +
                                                         "where TO_NUMBER(SAJET.SYS_ORD_H.NUMBER1) = TO_NUMBER('" + number1Value.ToString() + "') " +
                                                         "and TO_NUMBER(SAJET.SYS_ORD_H.NUMBER2) = TO_NUMBER('" + number2Value.ToString() + "') " +
                                                         "and TO_NUMBER(SAJET.SYS_ORD_D.SEQUENCE) = TO_NUMBER('" + orderSeqValue.ToString() + "')");

                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    var realDate = DateTime.Parse(row["REAL_DATE"].ToString()).ToString("yyyy/MM/dd");
                    var realDueDate = DateTime.Parse(row["REAL_DUE_DATE"].ToString()).ToString("yyyy/MM/dd");

                    dtCustDate.Text = realDate;
                    dtCustDueDate.Text = realDueDate;
                }
            }
        }

        private void SetBomRelation()
        {
            if (g_sUpdateType == "MODIFY")
            {
                return;
            }

            var operationIdStr = combOperation.SelectedItem?.ToString(); ; // 10A, 10B, 10C
            var dateNumber = cbNumber1.SelectedValue; // 201612
            var number1 = cbNumber2.SelectedValue; // 0013
            var orderSeq = cbOrderSequence.SelectedItem?.ToString();  // 1, 2, 3 ... 9

            if (operationIdStr != null && dateNumber != null && number1 != null && orderSeq != null)
            {
                string sql = "select PRODUCE_NUMBER from SAJET.SYS_ORD_D "
                           + $"where TO_NUMBER(NUMBER1)  = TO_NUMBER('{dateNumber}') "
                           + $"and TO_NUMBER(NUMBER2)  = TO_NUMBER('{number1}') "
                           + $"and TO_NUMBER(SEQUENCE)  = TO_NUMBER('{orderSeq}') ";

                DataSet dataSet = ClientUtils.ExecuteSQL(sql);
                var row = dataSet.Tables[0].Rows[0];
                string producenumber = row["PRODUCE_NUMBER"].ToString();

                cbProd10C.Items.Clear();
                cbProd10C.Items.Add(producenumber);
                cbProd10C.SelectedIndex = 0;
                //if (operationIdStr != "10C")
                //{
                //    SetCombProduce(producenumber, operationIdStr);
                //}
                //else
                //{
                //    cbProd10A.DataSource = (object)new List<string>();
                //    cbProd10B.DataSource = (object)new List<string>();
                //}

                SetCombProduce(producenumber, operationIdStr);
                //cbProd10A.DataSource = (object)new List<string>();
                //cbProd10B.DataSource = (object)new List<string>();
            }
        }

        /// <summary>
        /// 根據工單開立的區段對應的 BOM 料號帶入到右邊的預覽顯示區塊
        /// </summary>
        private void SetPartNo()
        {
            if (g_sUpdateType == "MODIFY")
            {
                return;
            }

            //edited by Lee@190219
            String strABC = combOperation.SelectedValue.ToString();
            editPart.Text = string.Empty;
            string p10A = cbProd10A.SelectedItem?.ToString();
            string p10B = cbProd10B.SelectedItem?.ToString();
            string p10C = cbProd10C.SelectedItem?.ToString();

            if (strABC == "10A")
            {
                editPart.Text = p10A;
            }
            else if (strABC == "10B")
            {
                editPart.Text = p10B;
            }
            else if (strABC == "10C")
            {
                editPart.Text = p10C;
            }
        }

        private void SetPalletVersionRouteOldNoUnitBlueprint()
        {
            if (g_sUpdateType == "MODIFY")
            {
                return;
            }

            if (!string.IsNullOrEmpty(editPart.Text))
            {
                string sql = "select OPTION1 PALLETS, VERSION, B.ROUTE_NAME, A.OPTION2, A.UPC, A.OPTION4 "
                           + "from SAJET.SYS_PART A LEFT JOIN SAJET.SYS_RC_ROUTE B ON A.ROUTE_ID = B.ROUTE_ID "
                           + $"where PART_NO='{editPart.Text}'";

                DataSet dataSet = ClientUtils.ExecuteSQL(sql);
                var row = dataSet.Tables[0].Rows[0];
                string pallets = row["PALLETS"].ToString();
                string version = row["VERSION"].ToString();
                string routeName = row["ROUTE_NAME"].ToString();
                string oldNo = row["OPTION2"].ToString();
                string unit = row["UPC"].ToString();
                string bluePrint = row["OPTION4"].ToString();

                // 料號棧板數量非必填預設為 0，如果遇到 0 則帶入 12
                // 2.0.17003.9
                if (int.TryParse(pallets, out int value))
                {
                    pallets = value > 0 ? value.ToString() : "12";
                }
                else
                {
                    pallets = "12";
                } // 2.0.17003.9

                txtPallets.Text = pallets;
                txtVersion.Text = version;
                combRoute.Items.Clear();
                combRoute.Items.Add(routeName);
                combRoute.SelectedIndex = 0;

                txtOldNum.Text = oldNo;
                txtUnit.Text = unit;
                txtBlueprint.Text = bluePrint;
            }
        }

        private List<string> GetOrderSeqList(string dateNumber, string number1)
        {
            var orderSeqList = new List<string>();

            DataSet dataSet = ClientUtils.ExecuteSQL("select sequence from SAJET.SYS_ORD_D  " +
                                                     "where TO_NUMBER(NUMBER1) =('" + dateNumber + "') and " +
                                                     "TO_NUMBER(NUMBER2) = TO_NUMBER('" + number1 + "')" +
                                                     " Order by sequence");
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                int seq = int.Parse(row["sequence"].ToString());
                orderSeqList.Add(seq.ToString());
            }

            return orderSeqList;
        }

        private string CalculateNextSequence(string operation, string dateNumber, string number1, string orderSeq)
        {
            try
            {
                DataSet dataSet = ClientUtils.ExecuteSQL(
                    "select TO_NUMBER(MAX(substr(NUMBER2,1,2)))+1 nextSeq from SAJET.G_WO_BASE  " +
                    "where TRIM(OPERATION_ID) = TRIM('" + operation + "') and " +
                    "TO_NUMBER(DATENUMBER) = TO_NUMBER('" + dateNumber + "') and " +
                    "TO_NUMBER(NUMBER1) = TO_NUMBER('" + number1 + "') and " +
                    "substr(NUMBER2,-1,1) = '" + orderSeq + "'");

                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = dataSet.Tables[0].Rows[0];
                    string nextSeqstr = dr["nextSeq"].ToString();
                    if (!string.IsNullOrEmpty(nextSeqstr))
                    {
                        int nextSeq = int.Parse(nextSeqstr);
                        return nextSeq.ToString("00");
                    }
                    else
                    {
                        return "00";
                    }

                }
                else
                {
                    throw new Exception($"Failed to find next sequence for OPERATION_ID:{operation}, {dateNumber}, {number1}");
                }
            }
            catch
            {
                throw;
            }
        }

        // No idea what it is
        // But this is required to create Run Card
        private void UpdateOtherTables()
        {
            SaveWOPackSpecData();
            var partId = GetID("SAJET.SYS_PART", "PART_ID", "PART_NO", editPart.Text);
            if (!CheckBomExist(tbWO.Text, partId))
            {
                CopyToWOBom(tbWO.Text, partId, txtVersion.Text);
                CopyToWoBomLoc(tbWO.Text, partId, txtVersion.Text);
            }
            CopyToWORule(tbWO.Text, combWoRule.Text);
        }

        private void AppendData()
        {
            string woStatusId = ConvertToStatusIndex(LabWoStatus.Text).ToString();
            string partId = GetID("SAJET.SYS_PART", "PART_ID", "PART_NO", editPart.Text);
            string pdLineName = string.Empty;
            if (combLine.SelectedItem != null)
            {
                pdLineName = combLine.SelectedItem.ToString();
            }
            string defaultPDLineId = GetID("SAJET.SYS_PDLINE", "PDLINE_ID", "PDLINE_NAME", pdLineName);

            string routeName = string.Empty;
            if (combRoute.SelectedItem != null)
            {
                routeName = combRoute.SelectedItem.ToString();
            }
            string routeId = GetID("SAJET.SYS_RC_ROUTE", "ROUTE_ID", "ROUTE_NAME", routeName);

            string number2 = string.Empty;
            if (!string.IsNullOrEmpty(txtSequence.Text) && !string.IsNullOrEmpty(cbOrderSequence.SelectedItem?.ToString()))
            {
                var seq = int.Parse(txtSequence.Text.ToString()).ToString("00");
                var ordSeq = int.Parse(cbOrderSequence.SelectedItem.ToString()).ToString("0");
                number2 = seq + ordSeq;
            }

            int madeCategoryIndex = ConvertMadeCategoryToIndex(combMade.Text);

            var sql = @"
INSERT INTO SAJET.G_WO_BASE (
    WORK_ORDER,
    PART_ID,
    VERSION,
    WO_RULE,
    WO_TYPE,
    TARGET_QTY,
    DEFAULT_PDLINE_ID,
    ROUTE_ID,
    OPERATION_ID,
    DATENUMBER,
    NUMBER1,
    NUMBER2,
    MADE_CATEGORY,
    CUSTOMER1,
    CUSTOMER3,
    SALES,
    CUSTOMIZE,
    PRODUCT_RATE,
    OUTSOURCE,
    CUSTOMER_DATE,
    CUSTOMER_DUE_DATE,
    PALLETS,
    WO_SCHEDULE_DATE,
    WO_DUE_DATE,
    RULE1,
    PRODUCE_NUMBER,
    PRODUCENO2,
    PRODUCENO1,
    OLD_NUMBER,
    UNIT,
    BLUEPRINT,
    REMARK,
    FACTORY_ID,
    WO_STATUS,
    REAL_DATE,
    REAL_DUE_DATE,
    TOTAL_NUMBER,
    UPDATE_USERID,
    WO_OPTION2
) VALUES (
    :WORK_ORDER,
    :PART_ID,
    :VERSION,
    :WO_RULE,
    :WO_TYPE,
    :TARGET_QTY,
    :DEFAULT_PDLINE_ID,
    :ROUTE_ID,
    :OPERATION_ID,
    :DATENUMBER,
    :NUMBER1,
    :NUMBER2,
    :MADE_CATEGORY,
    :CUSTOMER1,
    :CUSTOMER3,
    :SALES,
    :CUSTOMIZE,
    :PRODUCT_RATE,
    :OUTSOURCE,
    :CUSTOMER_DATE,
    :CUSTOMER_DUE_DATE,
    :PALLETS,
    :WO_SCHEDULE_DATE,
    :WO_DUE_DATE,
    :RULE1,
    :PRODUCE_NUMBER,
    :PRODUCENO2,
    :PRODUCENO1,
    :OLD_NUMBER,
    :UNIT,
    :BLUEPRINT,
    :REMARK,
    :FACTORY_ID,
    :WO_STATUS,
    :REAL_DATE,
    :REAL_DUE_DATE,
    :TOTAL_NUMBER,
    :UPDATE_USERID,
    :WO_OPTION2
)";

            object[][] Params = new object[39][];
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", tbWO.Text };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", partId };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VERSION", txtVersion.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WO_RULE", combWoRule.Text };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WO_TYPE", combWoType.Text };

            Params[5] = new object[] { ParameterDirection.Input, OracleType.Number, "TARGET_QTY", editTargetQty.Text };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DEFAULT_PDLINE_ID", defaultPDLineId };
            Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ROUTE_ID", routeId };
            Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OPERATION_ID", combOperation.Text };
            Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DATENUMBER", cbNumber1.Text };

            Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NUMBER1", cbNumber2.Text };
            Params[11] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NUMBER2", number2 };
            Params[12] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MADE_CATEGORY", madeCategoryIndex };
            Params[13] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER1", txtClient.Text };
            Params[14] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMER3", txtClient2.Text };

            Params[15] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SALES", txtSales.Text };
            Params[16] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CUSTOMIZE", txtCustomize.Text };
            Params[17] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PRODUCT_RATE", txtProductRate.Text };
            Params[18] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OUTSOURCE", txtOutsource.Text };
            Params[19] = new object[] { ParameterDirection.Input, OracleType.DateTime, "CUSTOMER_DATE", DateTime.Parse(dtCustDate.Text) };
            Params[20] = new object[] { ParameterDirection.Input, OracleType.DateTime, "CUSTOMER_DUE_DATE", DateTime.Parse(dtCustDueDate.Text) };

            Params[21] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PALLETS", txtPallets.Text };
            Params[22] = new object[] { ParameterDirection.Input, OracleType.DateTime, "WO_SCHEDULE_DATE", DateTime.Parse(dtScheduleDate.Text) };
            Params[23] = new object[] { ParameterDirection.Input, OracleType.DateTime, "WO_DUE_DATE", DateTime.Parse(dtDueDate.Text) };
            Params[24] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RULE1", txtRule1.Text };

            Params[25] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PRODUCE_NUMBER", cbProd10C.Text };
            Params[26] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PRODUCENO2", cbProd10B.Text };
            Params[27] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PRODUCENO1", cbProd10A.Text };

            Params[28] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OLD_NUMBER", txtOldNum.Text };
            Params[29] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UNIT", txtUnit.Text };
            Params[30] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BLUEPRINT", txtBlueprint.Text };
            Params[31] = new object[] { ParameterDirection.Input, OracleType.VarChar, "REMARK", editRemark.Text };

            Params[32] = new object[] { ParameterDirection.Input, OracleType.Number, "FACTORY_ID", "10008" };
            Params[33] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WO_STATUS", woStatusId };
            Params[34] = new object[] { ParameterDirection.Input, OracleType.DateTime, "Real_Date", DateTime.Parse(dtScheduleDate.Text) };
            Params[35] = new object[] { ParameterDirection.Input, OracleType.DateTime, "Real_Due_Date", DateTime.Parse(dtDueDate.Text) };
            Params[36] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TOTAL_NUMBER", editTargetQty.Text };
            Params[37] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", ClientUtils.UserPara1 };

            Params[38] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WO_OPTION2", TbBookNo.Text.Trim() };

            ClientUtils.ExecuteSQL(sql, Params);
        }

        private void ModifyData()
        {
            string pdLineName = string.Empty;
            if (combLine.SelectedItem != null)
            {
                pdLineName = combLine.SelectedItem.ToString();
            }
            string pdLineId = GetID("SAJET.SYS_PDLINE", "PDLINE_ID", "PDLINE_NAME", pdLineName);
            object[][] Params = new object[10][];
            var sql = @"
Update SAJET.G_WO_BASE
set MADE_CATEGORY = :MADE_CATEGORY
   ,PRODUCT_RATE = :PRODUCT_RATE
   ,OUTSOURCE = :OUTSOURCE
   ,WO_SCHEDULE_DATE = :WO_SCHEDULE_DATE
   ,WO_DUE_DATE = :WO_DUE_DATE
   ,DEFAULT_PDLINE_ID = :DEFAULT_PDLINE_ID
   ,REMARK = :REMARK
   ,WO_OPTION2 = :WO_OPTION2
   ,TARGET_QTY = :TARGET_QTY
where WORK_ORDER = :WORK_ORDER
";
            int index = ConvertMadeCategoryToIndex(combMade.Text);
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MADE_CATEGORY", index };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PRODUCT_RATE", txtProductRate.Text };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OUTSOURCE", txtOutsource.Text };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.DateTime, "WO_SCHEDULE_DATE", DateTime.Parse(dtScheduleDate.Text) };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.DateTime, "WO_DUE_DATE", DateTime.Parse(dtDueDate.Text) };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "DEFAULT_PDLINE_ID", pdLineId };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "REMARK", editRemark.Text };
            Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", tbWO.Text.Trim() };
            Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WO_OPTION2", TbBookNo.Text.Trim() };
            Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TARGET_QTY", editTargetQty.Text.Trim() };

            ClientUtils.ExecuteSQL(sql, Params);
        }

        private void ModifyData_Old()
        {
            if (g_sUpdateType == "MODIFY")
            {
                dataCurrentRow.Cells["ROUTE_ID"].Value.ToString();
                if (combRoute.Text != dataCurrentRow.Cells["ROUTE_NAME"].Value.ToString() &&
                    dataCurrentRow.Cells["INPUT_QTY"].Value.ToString() != "0" &&
                    SajetCommon.Show_Message(
                        SajetCommon.SetLanguage("Some SN had Input,") + Environment.NewLine +
                        SajetCommon.SetLanguage("Sure to Change Route") + " ?", 2) != DialogResult.Yes)
                    return;
            }
        }

        private string GetID(string sTable, string sFieldID, string sFieldName, string sValue)
        {
            if (string.IsNullOrEmpty(sValue))
                return "0";
            DataSet dataSet = ClientUtils.ExecuteSQL("select " + sFieldID + " from " + sTable + " where " + sFieldName + " = '" + sValue + "' ");
            if (dataSet.Tables[0].Rows.Count > 0)
                return dataSet.Tables[0].Rows[0][sFieldID].ToString();
            return "0";
        }

        public bool IsInputValid()
        {
            g_sPartID = GetID("SAJET.SYS_PART", "PART_ID", "PART_NO", editPart.Text);
            if (g_sPartID == "0")
            {
                int num = (int)SajetCommon.Show_Message("料號錯誤", 0);
                editPart.Focus();
                return false;
            }
            if (g_DBInitial.ContainsKey("TARGET_QTY") && g_DBInitial["TARGET_QTY"].sDefault.ToLower() == "decimal")
            {
                Decimal result = new Decimal();
                if (!Decimal.TryParse(editTargetQty.Text, out result))
                {
                    int num = (int)SajetCommon.Show_Message("'目標量'必須為數字", 0);
                    editTargetQty.Focus();
                    editTargetQty.SelectAll();
                    return false;
                }
                if (result == Decimal.Zero)
                {
                    int num = (int)SajetCommon.Show_Message("'目標量'必須大於 0", 0);
                    editTargetQty.Focus();
                    editTargetQty.SelectAll();
                    return false;
                }
            }
            else
            {
                int result = 0;
                if (!int.TryParse(editTargetQty.Text, out result))
                {
                    int num = (int)SajetCommon.Show_Message("'目標量'必須為整數", 0);
                    editTargetQty.Focus();
                    editTargetQty.SelectAll();
                    return false;
                }
                if (result <= 0)
                {
                    int num = (int)SajetCommon.Show_Message("'目標量'必須大於 0", 0);
                    editTargetQty.Focus();
                    editTargetQty.SelectAll();
                    return false;
                }
            }

            string woRule = combWoRule.Text;
            string woType = combWoType.Text;
            string pdLine = combLine.Text;
            string route = combRoute.Text;

            if (string.IsNullOrEmpty(woRule))
            {
                SajetCommon.Show_Message("'工單規則'為必填", 0);
                combWoRule.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(woType))
            {
                SajetCommon.Show_Message("'工單類型'為必填", 0);
                combWoType.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(pdLine))
            {
                SajetCommon.Show_Message("'線別名稱'為必填", 0);
                combLine.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(route))
            {
                SajetCommon.Show_Message("'途程名稱'為必填", 0);
                combRoute.Focus();
                return false;
            }

            if (dtCustDueDate.Value < dtCustDate.Value)
            {
                SajetCommon.Show_Message("'客/業交期'必須大於或等於'客/業日期'", 0);
                dtScheduleDate.Focus();
                return false;
            }

            if (dtDueDate.Value < dtScheduleDate.Value)
            {
                SajetCommon.Show_Message("'預計結束日期'必須大於或等於'預計開始日期'", 0);
                dtScheduleDate.Focus();
                return false;
            }

            return true;
        }

        private void Get_Part_Default_Data()
        {
            if (string.IsNullOrEmpty(editPart.Text))
            {
                int num = (int)MessageBox.Show("料號欄位不可為空 請至物料清單建置");
            }
            else
            {
                string id = GetID("SAJET.SYS_PART", "PART_ID", "PART_NO", editPart.Text);
                if (!(id != "0"))
                    return;

                GetDefault_Route(id);
                ListViewProduceDetail(editPart.Text, combRoute.Text);
                GetDefault_Option(id);
                if (LVPkSPec.Items.Count == 0)
                    GetDefault_PKSpec(id);
            }
        }

        private void GetDefault_Option(string sPartID)
        {
            if (tControlAdd == null)
                return;
            string empty = string.Empty;
            DataSet dataSet = ClientUtils.ExecuteSQL(
                !g_DBInitial.ContainsKey("PART LIST")
                ? "SELECT * FROM SAJET.SYS_PART WHERE PART_ID = :PART_ID AND ROWNUM = 1"
                : g_DBInitial["PART LIST"].sValue,
                new object[1][]
                {
                    new object[4]
                    {
                        ParameterDirection.Input,
                        OracleType.VarChar,
                        "PART_ID",
                        sPartID
                    }
                });

            for (int index = 0; index <= tControlAdd.Length - 1; ++index)
            {
                if (dataSet.Tables[0].Columns.Contains(tControlAdd[index].strPartField))
                {
                    string strType = tControlAdd[index].strType;
                    tControlAdd[index].txtControl.Text = dataSet.Tables[0].Rows[0][tControlAdd[index].strPartField].ToString();
                }
            }
            dataSet.Dispose();
        }

        private void GetDefault_PKSpec(string sPartID)
        {
            LVPkSPec.Items.Clear();
            LVPkSPec.Sorting = SortOrder.None;
            string str1 = string.Empty;
            if (g_DBInitial.ContainsKey("Packing Spec Part"))
                str1 = g_DBInitial["Packing Spec Part"].sValue;
            if (string.IsNullOrEmpty(str1))
                str1 = @"
SELECT
    C.PKSPEC_ID,
    C.PKSPEC_NAME,
    C.BOX_QTY,
    C.CARTON_QTY,
    C.PALLET_QTY
FROM
    SAJET.SYS_PART_PKSPEC   B,
    SAJET.SYS_PKSPEC        C
WHERE
    B.PART_ID = :PART_ID
    AND B.PKSPEC_ID = C.PKSPEC_ID
    AND C.ENABLED = 'Y'
GROUP BY
    C.PKSPEC_ID,
    C.PKSPEC_NAME,
    C.BOX_QTY,
    C.CARTON_QTY,
    C.PALLET_QTY
ORDER BY
    C.BOX_QTY DESC,
    C.CARTON_QTY DESC,
    C.PALLET_QTY DESC
";
            object[][] objArray = new object[1][]
            {
                new object[4] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", sPartID },
            };

            DataSet dataSet = ClientUtils.ExecuteSQL(str1, objArray);
            foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables[0].Rows)
            {
                LVPkSPec.Items.Add(row["PKSPEC_NAME"].ToString());
                for (int index = 1; index < LVPkSPec.Columns.Count; ++index)
                    LVPkSPec.Items[LVPkSPec.Items.Count - 1].SubItems.Add(row[LVPkSPec.Columns[index].Name].ToString());
                LVPkSPec.Items[LVPkSPec.Items.Count - 1].Tag = row["PKSPEC_ID"].ToString();
            }
            if (LVPkSPec.Items.Count == 0)
            {
                dataSet = ClientUtils.ExecuteSQL("Select Model_ID from SAJET.SYS_PART WHERE PART_ID = '" + sPartID + "'");
                string str2 = dataSet.Tables[0].Rows[0]["Model_ID"].ToString();
                string str3 = string.Empty;
                if (g_DBInitial.ContainsKey("Packing Spec Model"))
                {
                    if (!string.IsNullOrEmpty(g_DBInitial["Packing Spec Model"].sValue))
                        str3 = g_DBInitial["Packing Spec Model"].sValue;
                }
                else
                    str3 = " Select C.PKSPEC_ID,C.PKSPEC_NAME,C.BOX_QTY,C.CARTON_QTY,C.PALLET_QTY\r\n                     From SAJET.SYS_MODEL_PKSPEC B, SAJET.SYS_PKSPEC C \r\n                     Where B.MODEL_ID = :MODEL_ID \r\n                     and B.PKSPEC_ID = C.PKSPEC_ID \r\n                     and C.ENABLED = 'Y' \r\n                     Group By C.PKSPEC_ID,C.PKSPEC_NAME,C.BOX_QTY,C.CARTON_QTY,C.PALLET_QTY \r\n                     ORDER BY C.BOX_QTY desc,C.CARTON_QTY desc,C.PALLET_QTY desc ";
                if (!string.IsNullOrEmpty(str3))
                {
                    objArray[0] = new object[4]
                    {
                        ParameterDirection.Input,
                        OracleType.VarChar,
                        "MODEL_ID",
                        str2
                    };
                    dataSet = ClientUtils.ExecuteSQL(str3, objArray);
                    foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables[0].Rows)
                    {
                        LVPkSPec.Items.Add(row["PKSPEC_NAME"].ToString());
                        for (int index = 1; index < LVPkSPec.Columns.Count; ++index)
                            LVPkSPec.Items[LVPkSPec.Items.Count - 1].SubItems.Add(row[LVPkSPec.Columns[index].Name].ToString());
                        LVPkSPec.Items[LVPkSPec.Items.Count - 1].Tag = row["PKSPEC_ID"].ToString();
                    }
                }
            }
            dataSet.Dispose();
            LVPkSPec.Sorting = SortOrder.Ascending;
        }

        private void GetDefault_Route(string sPartID)
        {
            combRoute.Items.Clear();
            combRoute.Items.Add("");
            DataSet dataSet1 = ClientUtils.ExecuteSQL(" SELECT ROUTE_ID, ROUTE_NAME FROM SAJET.SYS_RC_ROUTE WHERE ENABLED = 'Y' AND ROUTE_ID IN (SELECT ROUTE_ID FROM SAJET.SYS_PART_ROUTE Where PART_ID = '" + sPartID + "') ORDER BY ROUTE_NAME ");
            for (int index = 0; index <= dataSet1.Tables[0].Rows.Count - 1; ++index)
                combRoute.Items.Add(dataSet1.Tables[0].Rows[index]["ROUTE_NAME"].ToString());
            string str = "0";
            DataSet dataSet2 = ClientUtils.ExecuteSQL(" Select B.ROUTE_NAME,B.ROUTE_ID,A.MODEL_ID, A.RULE_SET  From SAJET.SYS_PART A      ,SAJET.SYS_RC_ROUTE B  Where A.ROUTE_ID = B.ROUTE_ID(+)  and A.PART_ID = '" + sPartID + "' ");
            if (dataSet2.Tables[0].Rows.Count > 0)
            {
                if (string.IsNullOrEmpty(dataSet2.Tables[0].Rows[0]["ROUTE_NAME"].ToString()))
                {
                    combRoute.SelectedIndex = -1;
                }
                else
                {
                    int num = combRoute.FindString(dataSet2.Tables[0].Rows[0]["ROUTE_NAME"].ToString());
                    int selectedIndex = combRoute.SelectedIndex;
                    combRoute.SelectedIndex = num;
                }
                if (string.IsNullOrEmpty(dataSet2.Tables[0].Rows[0]["RULE_SET"].ToString()))
                {
                    if (combWoRule.Items.Count > 1)
                    {
                        combWoRule.SelectedIndex = 1;
                    }
                    else
                    {
                        combWoRule.SelectedIndex = -1;
                    }

                }
                else
                {
                    combWoRule.SelectedIndex = combWoRule.FindString(dataSet2.Tables[0].Rows[0]["RULE_SET"].ToString());
                }

                str = dataSet2.Tables[0].Rows[0]["MODEL_ID"].ToString();
            }
            DataSet dataSet3 = ClientUtils.ExecuteSQL(" Select B.ROUTE_NAME,B.ROUTE_ID,A.BURNIN_TIME,A.RULE_SET        ,A.CUSTOMER_ID,C.customer_code  From SAJET.SYS_MODEL A      ,SAJET.SYS_RC_ROUTE B      ,SAJET.SYS_CUSTOMER C  Where A.ROUTE_ID = B.ROUTE_ID(+)  and A.MODEL_ID = '" + str + "'  and A.CUSTOMER_ID = C.CUSTOMER_ID(+) ");
            if (dataSet3.Tables[0].Rows.Count > 0)
            {
                if (combRoute.SelectedIndex == -1 && !string.IsNullOrEmpty(dataSet3.Tables[0].Rows[0]["ROUTE_NAME"].ToString()))
                {
                    combRoute.SelectedIndex = combRoute.FindString(dataSet3.Tables[0].Rows[0]["ROUTE_NAME"].ToString());
                }

                if (combWoRule.SelectedIndex == -1 && !string.IsNullOrEmpty(dataSet3.Tables[0].Rows[0]["RULE_SET"].ToString()))
                {
                    combWoRule.SelectedIndex = combWoRule.FindString(dataSet3.Tables[0].Rows[0]["RULE_SET"].ToString());
                }
            }
            dataSet3.Dispose();
        }

        public void ShowWOPackSpecData(string sWO)
        {
            LVPkSPec.Items.Clear();
            DataSet dataSet = ClientUtils.ExecuteSQL(g_DBInitial["Packing Spec WO"].sValue, new object[1][]
            {
                new object[4]
                {
                   ParameterDirection.Input,
                   OracleType.VarChar,
                   "WORK_ORDER",
                   tbWO.Text
                }
            });
            LVPkSPec.Sorting = SortOrder.None;
            foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables[0].Rows)
            {
                LVPkSPec.Items.Add(row["pkspec_name"].ToString());
                for (int index = 1; index < LVPkSPec.Columns.Count; ++index)
                    LVPkSPec.Items[LVPkSPec.Items.Count - 1].SubItems.Add(row[LVPkSPec.Columns[index].Name].ToString());
                LVPkSPec.Items[LVPkSPec.Items.Count - 1].Tag = row["PKSPEC_ID"].ToString();
            }
            LVPkSPec.Sorting = SortOrder.Ascending;
            dataSet.Dispose();
        }

        public void SaveWOPackSpecData()
        {
            string s = @"
DELETE 
FROM
    SAJET.G_PACK_SPEC
WHERE
    WORK_ORDER = :WORK_ORDER
";

            ClientUtils.ExecuteSQL(s, new object[1][]
            {
                new object[4]
                {
                    ParameterDirection.Input,
                    OracleType.VarChar,
                    "WORK_ORDER",
                    tbWO.Text
                }
            });

            if (g_DBInitial.ContainsKey("Packing Spec Part"))
            {
                object[][] objArray = new object[4 + LVPkSPec.Columns.Count][];
                objArray[0] = new object[4] { ParameterDirection.Input, OracleType.VarChar, "TWO", tbWO.Text };
                objArray[1] = new object[4] { ParameterDirection.Input, OracleType.VarChar, "TEMPID", ClientUtils.UserPara1 };
                objArray[2] = new object[4] { ParameterDirection.Input, OracleType.VarChar, "TPART", g_sPartID };
                objArray[3] = new object[4] { ParameterDirection.Output, OracleType.VarChar, "TRES", "" };

                for (int index1 = 0; index1 <= LVPkSPec.Items.Count - 1; ++index1)
                {
                    objArray[4] = new object[4] { ParameterDirection.Input, OracleType.VarChar, "TPKSPEC", LVPkSPec.Items[index1].Tag.ToString() };

                    for (int index2 = 1; index2 < LVPkSPec.Columns.Count; ++index2)
                    {
                        objArray[4 + index2] = new object[4] { ParameterDirection.Input, OracleType.VarChar, "T" + LVPkSPec.Columns[index2].Name, LVPkSPec.Items[index1].SubItems[index2].Text.ToString() };
                    }

                    ClientUtils.ExecuteProc("SAJET.MAINTAIN_WO_PKSPEC", objArray);
                }
            }
            else
            {
                object[][] objArray = new object[7][]
                {
                    new object[4]
                    {
                        ParameterDirection.Input,
                        OracleType.VarChar,
                        "WORK_ORDER",
                        tbWO.Text
                    },
                    new object[4]
                    {
                        ParameterDirection.Input,
                        OracleType.VarChar,
                        "UPDATE_USERID",
                        ClientUtils.UserPara1
                    },
                    new object[4]
                    {
                        ParameterDirection.Input,
                        OracleType.VarChar,
                        "PART_ID",
                        g_sPartID
                    },
                    null,
                    null,
                    null,
                    null
                };
                for (int index = 0; index <= LVPkSPec.Items.Count - 1; ++index)
                {
                    objArray[3] = new object[4]
                    {
                        ParameterDirection.Input,
                        OracleType.VarChar,
                        "BOX_CAPACITY",
                        LVPkSPec.Items[index].SubItems[1].Text
                    };
                    objArray[4] = new object[4]
                    {
                        ParameterDirection.Input,
                        OracleType.VarChar,
                        "CARTON_CAPACITY",
                        LVPkSPec.Items[index].SubItems[2].Text
                    };
                    objArray[5] = new object[4]
                    {
                        ParameterDirection.Input,
                        OracleType.VarChar,
                        "PALLET_CAPACITY",
                        LVPkSPec.Items[index].SubItems[3].Text
                    };
                    objArray[6] = new object[4]
                    {
                        ParameterDirection.Input,
                        OracleType.VarChar,
                        "PKSPEC_ID",
                        LVPkSPec.Items[index].Tag.ToString()
                    };
                    ClientUtils.ExecuteSQL(" insert into SAJET.G_PACK_SPEC (WORK_ORDER,PART_ID,PKSPEC_ID,PALLET_CAPACITY,CARTON_CAPACITY,BOX_CAPACITY,UPDATE_USERID) " +
                                           "values (:WORK_ORDER, :PART_ID, :PKSPEC_ID, :PALLET_CAPACITY, :CARTON_CAPACITY, :BOX_CAPACITY, :UPDATE_USERID)", objArray);
                }
            }
        }

        public bool CheckBomExist(string sWO, string sPartId)
        {
            return ClientUtils.ExecuteSQL("select WORK_ORDER from SAJET.G_WO_BOM where WORK_ORDER ='" + sWO + "' and ROWNUM = 1 ").Tables[0].Rows.Count > 0;
        }

        public void CopyToWOBom(string sWO, string sPartId, string sVer)
        {
            string s = @"
DELETE
FROM
    SAJET.G_WO_BOM
WHERE
    WORK_ORDER = :WORK_ORDER
";

            ClientUtils.ExecuteSQL(s, new object[1][]
            {
                new object[4]
                {
                   ParameterDirection.Input,
                   OracleType.VarChar,
                   "WORK_ORDER",
                   sWO
                }
            });

            if (string.IsNullOrEmpty(sVer))
            {
                sVer = "N/A";
            }

            s = @"
SELECT
    BOM_ID
FROM
    SAJET.SYS_BOM_INFO
WHERE
    PART_ID = :PART_ID
    and Version = :VERSION
";

            DataSet dataSet = ClientUtils.ExecuteSQL(s,
                new object[2][]
                {
                    new object[4]
                    {
                        ParameterDirection.Input,
                        OracleType.VarChar,
                        "PART_ID",
                        sPartId
                    },
                    new object[4]
                    {
                        ParameterDirection.Input,
                        OracleType.VarChar,
                        "VERSION",
                        sVer
                    }
                });

            if (dataSet.Tables[0].Rows.Count > 0)
            {
                string str = dataSet.Tables[0].Rows[0]["BOM_ID"].ToString();

                s = @"
INSERT INTO
    SAJET.G_WO_BOM
(
    WORK_ORDER,
    PART_ID,
    ITEM_PART_ID,
    ITEM_GROUP,
    ITEM_COUNT,
    PROCESS_ID,
    VERSION,
    UPDATE_USERID
)
SELECT
    :WORK_ORDER,
    :PART_ID,
    ITEM_PART_ID,
    ITEM_GROUP,
    ITEM_COUNT,
    PROCESS_ID,
    VERSION,
    :EMP_ID
from
    SAJET.SYS_BOM
WHERE
    BOM_ID = :BOM_ID
    And Enabled = 'Y' 
";

                ClientUtils.ExecuteSQL(s,
                    new object[4][]
                    {
                        new object[4]
                        {
                             ParameterDirection.Input,
                             OracleType.VarChar,
                             "PART_ID",
                             sPartId
                        },
                        new object[4]
                        {
                             ParameterDirection.Input,
                             OracleType.VarChar,
                             "BOM_ID",
                             str
                        },
                        new object[4]
                        {
                             ParameterDirection.Input,
                             OracleType.VarChar,
                             "WORK_ORDER",
                             sWO
                        },
                        new object[4]
                        {
                             ParameterDirection.Input,
                             OracleType.VarChar,
                             "EMP_ID",
                             ClientUtils.UserPara1
                        }
                    });
            }
            dataSet.Dispose();
        }

        public void CopyToWoBomLoc(string sWO, string sPartId, string sVer)
        {
            ClientUtils.ExecuteSQL("DELETE SAJET.G_WO_BOM_LOCATION WHERE WORK_ORDER = :WORK_ORDER", new object[1][]
            {
                new object[4]
                {
                     ParameterDirection.Input,
                     OracleType.VarChar,
                     "WORK_ORDER",
                     sWO
                }
            });
            if (string.IsNullOrEmpty(sVer))
                sVer = "N/A";
            DataSet dataSet = ClientUtils.ExecuteSQL(
                "SELECT BOM_ID FROM SAJET.SYS_BOM_INFO WHERE PART_ID = :PART_ID and Version = :VERSION", new object[2][]
                {
                    new object[4]
                    {
                         ParameterDirection.Input,
                         OracleType.VarChar,
                         "PART_ID",
                         sPartId
                    },
                    new object[4]
                    {
                         ParameterDirection.Input,
                         OracleType.VarChar,
                         "VERSION",
                         sVer
                    }
                });
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                string str = dataSet.Tables[0].Rows[0]["BOM_ID"].ToString();
                ClientUtils.ExecuteSQL(
                    "Insert Into SAJET.G_WO_BOM_LOCATION \r\n                    (WORK_ORDER, PART_ID, ITEM_PART_ID, ITEM_GROUP, VERSION, LOCATION, UPDATE_USERID) \r\n                    Select :WORK_ORDER, :PART_ID,ITEM_PART_ID, ITEM_GROUP, VERSION, LOCATION,:EMP_ID\r\n                    from SAJET.SYS_BOM_LOCATION \r\n                    WHERE BOM_ID = :BOM_ID\r\n                    And Enabled='Y' ",
                    new object[4][]
                    {
                        new object[4]
                        {
                             ParameterDirection.Input,
                             OracleType.VarChar,
                             "PART_ID",
                             sPartId
                        },
                        new object[4]
                        {
                             ParameterDirection.Input,
                             OracleType.VarChar,
                             "BOM_ID",
                             str
                        },
                        new object[4]
                        {
                             ParameterDirection.Input,
                             OracleType.VarChar,
                             "WORK_ORDER",
                             sWO
                        },
                        new object[4]
                        {
                             ParameterDirection.Input,
                             OracleType.VarChar,
                             "EMP_ID",
                             ClientUtils.UserPara1
                        }
                    });
            }
            dataSet.Dispose();
        }

        public void CopyToWORule(string sWO, string sRule)
        {
            ClientUtils.ExecuteSQL(
                "delete from SAJET.G_WO_PARAM\r\n                            where WORK_ORDER = :WORK_ORDER\r\n                            and MODULE_NAME in (select upper(label_name) || ' RULE' from sajet.sys_label) ",
                new object[1][]
                {
                    new object[4]
                    {
                         ParameterDirection.Input,
                         OracleType.VarChar,
                         "WORK_ORDER",
                         sWO
                    }
                });
            ClientUtils.ExecuteSQL(
                "INSERT INTO SAJET.G_WO_PARAM \r\n                (WORK_ORDER,MODULE_NAME,FUNCTION_NAME,PARAME_NAME,PARAME_ITEM,PARAME_VALUE,UPDATE_USERID,UPDATE_TIME)\r\n                SELECT :WORK_ORDER, B.RULE_TYPE || ' RULE', B.RULE_NAME, D.PARAME_NAME, D.PARAME_ITEM, D.PARAME_VALUE, :EMP_ID, SYSDATE \r\n                From SAJET.SYS_MODULE_PARAM A  \r\n                    ,SAJET.SYS_RULE_NAME B \r\n                \t ,SAJET.SYS_RULE_PARAM D \r\n                    ,sajet.sys_label c \r\n                Where A.MODULE_NAME = 'W/O RULE' \r\n                and A.FUNCTION_NAME = :FUNCTION_NAME \r\n                and A.PARAME_NAME = c.label_name || ' Rule'  \r\n                and A.PARAME_ITEM = B.RULE_NAME  \r\n                and B.RULE_TYPE = upper(c.label_name) \r\n                and c.type <> 'U' \r\n                and B.RULE_ID = D.RULE_ID ",
                new object[3][]
                {
                    new object[4]
                    {
                         ParameterDirection.Input,
                         OracleType.VarChar,
                         "WORK_ORDER",
                         sWO
                    },
                    new object[4]
                    {
                         ParameterDirection.Input,
                         OracleType.VarChar,
                         "EMP_ID",
                         ClientUtils.UserPara1
                    },
                    new object[4]
                    {
                         ParameterDirection.Input,
                         OracleType.VarChar,
                         "FUNCTION_NAME",
                         sRule
                    }
                });
        }

        public void DateTimeNumber()
        {
            if (cmb)
                return;
            DrLstN1N2 = ClientUtils.ExecuteSQL(" select number1,LISTAGG(number2, ',') WITHIN GROUP (ORDER BY number1) AS number2 from SAJET.SYS_ORD_H where FLAG='Y' group by number1 order by number1 DESC");
            List<string> stringList = new List<string>();
            if (DrLstN1N2.Tables[0].Rows.Count > 0)
            {
                for (int index = 0; index <= DrLstN1N2.Tables[0].Rows.Count - 1; ++index)
                {
                    string str = DrLstN1N2.Tables[0].Rows[index]["NUMBER1"].ToString();
                    stringList.Add(str);
                }

                cbNumber1.DataSource = stringList;
            }

            string[] number2Array = DrLstN1N2.Tables[0].Select("number1 =" + stringList[0].ToString())[0][1].ToString().Split(',');
            List<string> number2List = new List<string>();
            foreach (var number2 in number2Array)
            {
                string n2 = int.Parse(number2).ToString("0000");
                number2List.Add(n2);
            }

            cbNumber2.DataSource = number2List.ToArray();
        }

        public string GetSequence(DataSet dtTemp, string numberDate, string number)
        {
            string empty = string.Empty;
            dtTemp = ClientUtils.ExecuteSQL(" select * from SAJET.SYS_ORD_D where NUMBER1 = '" + numberDate + "' and NUMBER2 = '" + number + "' and PRODUCE_NUMBER = '" + dtTemp.Tables[0].Rows[0]["PRODUCE_NUMBER"].ToString() + "'");
            int int16 = Convert.ToInt16(number);
            return "0" + ClientUtils.ExecuteSQL(" select * from SAJET.G_WO_BASE where WORK_ORDER like '%" + numberDate + SetNumber(int16.ToString(), 4) + "%' and PRODUCE_NUMBER = '" + dtTemp.Tables[0].Rows[0]["PRODUCE_NUMBER"].ToString() + "'").Tables[0].Rows.Count + Convert.ToInt32(dtTemp.Tables[0].Rows[0]["SEQUENCE"]).ToString();
        }

        public string GetNextSequence_old(string OPERATION_ID, string numberDate, string number1, string LastIndexofnumber2)
        {
            string empty = string.Empty;
            DataSet dataSet = ClientUtils.ExecuteSQL("select TO_NUMBER(MAX(substr(NUMBER2,1,2)))+1 nextWR from SAJET.G_WO_BASE  where TRIM(OPERATION_ID) = TRIM('" + OPERATION_ID + "') and TO_NUMBER(DATENUMBER) = TO_NUMBER('" + numberDate + "') and TO_NUMBER(NUMBER1) = TO_NUMBER('" + number1 + "') and substr(NUMBER2,-1,1) = '" + LastIndexofnumber2 + "'");
            string str1;
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                string str2 = dataSet.Tables[0].Rows[0]["nextWR"].ToString();
                str1 = !string.IsNullOrEmpty(str2) ? (str2.Length != 1 ? str2 + LastIndexofnumber2 : "0" + str2 + LastIndexofnumber2) : "00" + LastIndexofnumber2;
            }
            else
                str1 = "00" + LastIndexofnumber2;
            return str1;
        }

        public string CHKSequence(string OPERATION_ID, string numberDate, string number1, string number2)
        {
            string empty = string.Empty;
            DataSet dataSet = ClientUtils.ExecuteSQL("select TO_NUMBER(MAX(substr(NUMBER2,1,2)))+1 nextWR from SAJET.G_WO_BASE  where TRIM(OPERATION_ID) = TRIM('" + OPERATION_ID + "') and TO_NUMBER(DATENUMBER) = TO_NUMBER('" + numberDate + "') and TO_NUMBER(NUMBER1) = TO_NUMBER('" + number1 + "') and TO_NUMBER(NUMBER2) = TO_NUMBER('" + number2 + "')");
            string str1;
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                string str2 = dataSet.Tables[0].Rows[0]["nextWR"].ToString();
                str1 = !string.IsNullOrEmpty(str2) ? (str2.Length != 1 ? str2 + number2 : "0" + str2 + number2) : "00" + number2;
            }
            else
                str1 = "00" + number2;
            return str1;
        }

        //  after OK button is cliecked, call this method to validate 
        private void CHK_WR()
        {
            string str1 = combOperation.SelectedValue.ToString();
            string numberDate = cbNumber1.SelectedValue.ToString();
            string str2 = cbNumber2.SelectedValue.ToString();
            combMade.SelectedValue.ToString();
            string text = txtNum.Text;
            if (string.IsNullOrEmpty(txtNum.Text))
            {
                int num1 = (int)MessageBox.Show("工單序號不可為空");
            }
            else if (string.IsNullOrEmpty(text))
            {
                int num2 = (int)MessageBox.Show("工單末三碼 不可為空");
            }
            else if (text.Length > 3)
            {
                int num3 = (int)MessageBox.Show("工單末三碼 只可以三碼");
            }
            else
            {
                string number3 = SetNumber(text, 3);
                txtNum.Text = number3;

                string s = $@"
SELECT
    *
FROM
    sajet.sys_ord_h   a
    LEFT JOIN sajet.sys_ord_d   b ON to_number(a.number1) = to_number(b.number1)
                                   AND to_number(a.number2) = to_number(b.number2)
    RIGHT JOIN sajet.g_wo_base   c ON TRIM(b.produce_number) = TRIM(c.produce_number)
                                    AND to_number(c.datenumber) = to_number(b.number1)
                                    AND to_number(c.number1) = to_number(b.number2)
                                    AND to_number(c.number2) = to_number(:p1)
                                    AND TRIM(c.operation_id) = TRIM(:p2)
WHERE
    to_number(a.number1) = to_number(:p3)
    AND to_number(a.number2) = to_number(:p4)
";
                var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "p1", number3 },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "p2", str1 },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "p3", numberDate },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "p4", str2 }
                };

                DataSet dataSet = ClientUtils.ExecuteSQL(s, p.ToArray());

                string nextSequence = GetNextSequence_old(str1, numberDate, str2, number3.Substring(number3.Length - 1, 1));

                if (dataSet.Tables[0].Rows.Count > 0 && g_sUpdateType == "APPEND")
                {
                    txtNum.Focus();
                    int num4 = (int)MessageBox.Show("工單已開立 下個工單末三碼應為:" + nextSequence);
                }
                else if (Convert.ToInt32(nextSequence) != Convert.ToInt32(number3) && g_sUpdateType == "APPEND")
                {
                    int num5 = (int)MessageBox.Show("工單未開立 下個工單末三碼應為:" + nextSequence);
                }
                else
                {
                    s = @"
SELECT
    *
FROM
    sajet.sys_ord_h   a
    LEFT JOIN sajet.sys_ord_d   b ON to_number(a.number1) = to_number(b.number1)
                                   AND to_number(a.number2) = to_number(b.number2)
WHERE
    a.flag = 'Y'
    AND to_number(a.number1) = to_number(:p1)
    AND to_number(a.number2) = to_number(:p2)
    AND to_number(b.sequence) = to_number(:p3)
";
                    p = new List<object[]>
                    {
                        new object[] { ParameterDirection.Input, OracleType.VarChar, "p1", numberDate },
                        new object[] { ParameterDirection.Input, OracleType.VarChar, "p2", str2 },
                        new object[] { ParameterDirection.Input, OracleType.VarChar, "p3", number3.Substring(number3.Length - 1, 1) },
                    };

                    DataSet dsTemp = ClientUtils.ExecuteSQL(s, p.ToArray());

                    if (dsTemp.Tables[0].Rows.Count == 0)
                    {
                        int num4 = (int)MessageBox.Show("無此 " + numberDate + str2 + number3 + " 訂單");
                    }
                    else
                    {
                        txtClient.Text = dsTemp.Tables[0].Rows[0]["CUSTOMER_ID"].ToString();
                        txtClient2.Text = dsTemp.Tables[0].Rows[0]["CUSTOMER_NAME"].ToString();
                        txtSales.Text = dsTemp.Tables[0].Rows[0]["ACCOUNT_ID"].ToString();
                        txtCustomize.Text = dsTemp.Tables[0].Rows[0]["CUSTOMIZE"].ToString();
                        string number = SetNumber(str2, 4);
                        tbWO.Text = str1 + numberDate + number + txtNum.Text;
                        tbWO.Enabled = false;
                        txtRule1.Text = dsTemp.Tables[0].Rows[0]["SPEC1"].ToString();


                        DateTime dateTime2 = Convert.ToDateTime(dsTemp.Tables[0].Rows[0]["REAL_DUE_DATE"].ToString());
                        dtCustDueDate.Text = dsTemp.Tables[0].Rows[0]["REAL_DATE"].ToString();
                        dtCustDueDate.Value = dateTime2.Date;

                        txtUnit.Text = dsTemp.Tables[0].Rows[0]["UNIT"].ToString();
                        SetCombProduce(dsTemp, str1);
                        ListViewOrderDetail1(str1, numberDate, number, number3, editTargetQty.Text, g_sUpdateType, "");
                        ListViewProduceDetail(editPart.Text, combRoute.Text);
                    }
                }
            }
        }

        public void ListViewOrderDetail1(string operationid, string numberDate, string number, string number3, string woTarget, string type, string ev)
        {
            listViewOrderDetail.Items.Clear();
            if (string.IsNullOrEmpty(number3))
            {
                int num1 = (int)MessageBox.Show("工單末三碼 不可為空");
            }
            else if (number3.Length > 3)
            {
                int num2 = (int)MessageBox.Show("工單末三碼 只可以三碼");
            }
            else
            {
                number3 = SetNumber(number3, 3);
                string targetQtyStr = woTarget;
                if (string.IsNullOrEmpty(targetQtyStr))
                    targetQtyStr = "0";
                string ordSeqenceStr = number3.Substring(number3.Length - 1, 1);
                sSQL = " select * from SAJET.SYS_ORD_H  left join SAJET.SYS_ORD_D on TO_NUMBER(SAJET.SYS_ORD_H.NUMBER1) = TO_NUMBER(SAJET.SYS_ORD_D.NUMBER1) and TO_NUMBER(SAJET.SYS_ORD_H.NUMBER2) = TO_NUMBER(SAJET.SYS_ORD_D.NUMBER2)  where TO_NUMBER(SAJET.SYS_ORD_H.NUMBER1) = TO_NUMBER('" + numberDate + "') and TO_NUMBER(SAJET.SYS_ORD_H.NUMBER2) = TO_NUMBER('" + number + "') and TO_NUMBER(SAJET.SYS_ORD_D.SEQUENCE) = TO_NUMBER('" + ordSeqenceStr + "')";
                DataSet dataSet1 = ClientUtils.ExecuteSQL(sSQL);
                int count1 = dataSet1.Tables[0].Rows.Count;
                if (dataSet1.Tables[0].Rows.Count == 0)
                {
                    int num3 = (int)MessageBox.Show("無此 " + numberDate + number + number3 + " 訂單");
                }
                else
                {
                    if (count1 <= 0)
                        return;
                    DataSet dataSet2 = ClientUtils.ExecuteSQL(" select A.NUMBER1,A.NUMBER2,TO_NUMBER(B.SEQUENCE) SEQUENCE,B.REAL_DUE_DATE,B.QUANTITY  ,SUM(NVL(C.TARGET_QTY, 0)) TARGET_QTY,B.PRODUCE_NUMBER,A.CUSTOMER_NAME  from SAJET.SYS_ORD_H A    left join SAJET.SYS_ORD_D B on TO_NUMBER(B.NUMBER1) = TO_NUMBER(A.NUMBER1)  and TO_NUMBER(A.NUMBER2) = TO_NUMBER(B.NUMBER2)    left outer join SAJET.G_WO_BASE C on  TRIM(B.PRODUCE_NUMBER) =  TRIM(C.PRODUCE_NUMBER) and    TRIM(B.NUMBER1) = TRIM(C.DATENUMBER) and  TO_NUMBER(B.NUMBER2) = TO_NUMBER(C.NUMBER1) and  TRIM(C.OPERATION_ID) = TRIM('" + operationid + "')   where TO_NUMBER(A.NUMBER1) = TO_NUMBER('" + numberDate + "') and  TO_NUMBER(A.NUMBER2) = TO_NUMBER('" + number + "') GROUP BY A.NUMBER1,A.NUMBER2,B.SEQUENCE,B.REAL_DUE_DATE,B.QUANTITY,B.PRODUCE_NUMBER,A.CUSTOMER_NAME  ORDER BY A.NUMBER1,A.NUMBER2,B.SEQUENCE ");
                    int count2 = dataSet2.Tables[0].Rows.Count;
                    string empty1 = string.Empty;
                    string empty2 = string.Empty;
                    int num4 = 0;
                    if (count2 > 0)
                    {
                        for (int index = 0; index < dataSet2.Tables[0].Rows.Count; ++index)
                        {
                            int accumulateQty;
                            int targetQty;
                            string str3;
                            if (ordSeqenceStr.Equals(dataSet2.Tables[0].Rows[index]["SEQUENCE"].ToString()))
                            {
                                num4 = 1;
                                if (type.Equals("APPEND"))
                                {
                                    accumulateQty = Convert.ToInt32(targetQtyStr) + Convert.ToInt32(dataSet2.Tables[0].Rows[index]["TARGET_QTY"]);
                                    targetQty = Convert.ToInt32(targetQtyStr);
                                }
                                else if (type.Equals("MODIFY"))//(ev.Equals("ClickTargetQTY") && type.Equals("MODIFY"))
                                {
                                    string s = $@"
SELECT
    *
FROM
    SAJET.G_WO_BASE
WHERE
    TRIM(OPERATION_ID) = TRIM('{operationid}')
    AND TO_NUMBER(DATENUMBER) = TO_NUMBER('{numberDate}')
    AND TO_NUMBER(NUMBER1) = TO_NUMBER('{number}')
    AND TO_NUMBER(NUMBER2) = TO_NUMBER('{number3} ')
";
                                    var d = ClientUtils.ExecuteSQL(s);

                                    int int32 = 0;

                                    if (d.Tables[0].Rows.Count > 0)
                                    {
                                        int32 = Convert.ToInt32(d.Tables[0].Rows[0]["TARGET_QTY"]);
                                    }

                                    accumulateQty = Convert.ToInt32(targetQtyStr) + Convert.ToInt32(dataSet2.Tables[0].Rows[index]["TARGET_QTY"]) - int32;
                                    targetQty = Convert.ToInt32(targetQtyStr);
                                }
                                else
                                {
                                    accumulateQty = Convert.ToInt32(dataSet2.Tables[0].Rows[index]["TARGET_QTY"]);
                                    targetQty = Convert.ToInt32(targetQtyStr);
                                }
                                //str3 = number3;
                            }
                            else
                            {
                                num4 = 2;
                                //str3 = "00" + dataSet2.Tables[0].Rows[index]["SEQUENCE"].ToString();
                                accumulateQty = Convert.ToInt32(dataSet2.Tables[0].Rows[index]["TARGET_QTY"]);
                                targetQty = 0;
                            }
                            int seq = int.Parse(dataSet2.Tables[0].Rows[index]["SEQUENCE"].ToString());
                            str3 = seq.ToString("000 ");

                            string realDueDate = DateTime.Parse(dataSet2.Tables[0].Rows[index]["REAL_DUE_DATE"].ToString()).ToString("yyyy/MM/dd");

                            listViewOrderDetail.Items.Add(
                                new ListViewItem("211" + numberDate + " " + number + " " + str3)
                                {
                                    SubItems =
                                    {
                                        dataSet2.Tables[0].Rows[index]["CUSTOMER_NAME"].ToString(),
                                        realDueDate,
                                        dataSet2.Tables[0].Rows[index]["QUANTITY"].ToString(),
                                        string.Concat( targetQty),
                                        accumulateQty.ToString()
                                    }
                                });
                        }
                    }
                }
            }
        }

        public void ListViewOrderDetail1(string operationid, string numberDate, string number, string number3, string woTarget)
        {
            listViewOrderDetail.Items.Clear();
            if (string.IsNullOrEmpty(number3))
            {
                int num1 = (int)MessageBox.Show("工單末三碼 不可為空");
            }
            else if (number3.Length > 3)
            {
                int num2 = (int)MessageBox.Show("工單末三碼 只可以三碼");
            }
            else
            {
                number3 = SetNumber(number3, 3);
                string str1 = woTarget;
                if (string.IsNullOrEmpty(str1))
                    str1 = "0";
                string str2 = number3.Substring(number3.Length - 1, 1);
                sSQL = " select * from SAJET.SYS_ORD_H  left join SAJET.SYS_ORD_D on TO_NUMBER(SAJET.SYS_ORD_H.NUMBER1) = TO_NUMBER(SAJET.SYS_ORD_D.NUMBER1) and TO_NUMBER(SAJET.SYS_ORD_H.NUMBER2) = TO_NUMBER(SAJET.SYS_ORD_D.NUMBER2)  where TO_NUMBER(SAJET.SYS_ORD_H.NUMBER1) = TO_NUMBER('" + numberDate + "') and TO_NUMBER(SAJET.SYS_ORD_H.NUMBER2) = TO_NUMBER('" + number + "') and TO_NUMBER(SAJET.SYS_ORD_D.SEQUENCE) = TO_NUMBER('" + str2 + "')";
                DataSet dataSet1 = ClientUtils.ExecuteSQL(sSQL);
                if (dataSet1.Tables[0].Rows.Count == 0)
                {
                    int num3 = (int)MessageBox.Show("無此 " + numberDate + number + number3 + " 訂單");
                }
                else
                {
                    if (dataSet1.Tables[0].Rows.Count <= 0)
                        return;
                    DataSet dataSet2 = ClientUtils.ExecuteSQL(" select A.NUMBER1,A.NUMBER2,TO_NUMBER(B.SEQUENCE) SEQUENCE,B.REAL_DUE_DATE,B.QUANTITY  ,SUM(NVL(C.TARGET_QTY, 0)) TARGET_QTY,B.PRODUCE_NUMBER,A.CUSTOMER_NAME  from SAJET.SYS_ORD_H A    left join SAJET.SYS_ORD_D B on TO_NUMBER(B.NUMBER1) = TO_NUMBER(A.NUMBER1)  and TO_NUMBER(A.NUMBER2) = TO_NUMBER(B.NUMBER2)    left outer join SAJET.G_WO_BASE C on  TRIM(B.PRODUCE_NUMBER) =  TRIM(C.PRODUCE_NUMBER) and    TO_NUMBER(B.NUMBER1) = TO_NUMBER(C.DATENUMBER) and  TO_NUMBER(B.NUMBER2) = TO_NUMBER(C.NUMBER1) and TRIM( C.OPERATION_ID) =TRIM('" + operationid + "')   where TO_NUMBER(A.NUMBER1) = TO_NUMBER('" + numberDate + "') and  TO_NUMBER(A.NUMBER2) = TO_NUMBER('" + number + "') GROUP BY A.NUMBER1,A.NUMBER2,B.SEQUENCE,B.REAL_DUE_DATE,B.QUANTITY,B.PRODUCE_NUMBER,A.CUSTOMER_NAME ");
                    int count = dataSet2.Tables[0].Rows.Count;
                    string empty1 = string.Empty;
                    string empty2 = string.Empty;
                    if (count > 0)
                    {
                        for (int index = 0; index < dataSet2.Tables[0].Rows.Count; ++index)
                        {
                            int num4;
                            int num5;
                            string str3;
                            if (str2.Equals(dataSet2.Tables[0].Rows[index]["SEQUENCE"].ToString()))
                            {
                                num4 = Convert.ToInt32(str1) + Convert.ToInt32(dataSet2.Tables[0].Rows[index]["TARGET_QTY"]);
                                num5 = Convert.ToInt32(str1);
                                str3 = number3;
                            }
                            else
                            {
                                str3 = "00" + dataSet2.Tables[0].Rows[index]["SEQUENCE"].ToString();
                                num4 = Convert.ToInt32(dataSet2.Tables[0].Rows[index]["TARGET_QTY"]);
                                num5 = 0;
                            }
                            listViewOrderDetail.Items.Add(new ListViewItem("211" + numberDate + " " + number + " " + str3)
                            {
                                SubItems = {
                                    dataSet2.Tables[0].Rows[index]["CUSTOMER_NAME"].ToString(),
                                    dataSet2.Tables[0].Rows[index]["REAL_DUE_DATE"].ToString(),
                                    dataSet2.Tables[0].Rows[index]["QUANTITY"].ToString(),
                                    string.Concat( num5),
                                    num4.ToString()
                                }
                            });
                        }
                    }
                }
            }
        }

        public void ListViewOrderDetail(string numberDate, string number, string number3)
        {
            sSQL = " select * from SAJET.SYS_ORD_H  left join SAJET.SYS_ORD_D on TO_NUMBER(SAJET.SYS_ORD_H.NUMBER1) = TO_NUMBER(SAJET.SYS_ORD_D.NUMBER1) and TO_NUMBER(SAJET.SYS_ORD_H.NUMBER2) = TO_NUMBER(SAJET.SYS_ORD_D.NUMBER2)  where TO_NUMBER(SAJET.SYS_ORD_H.NUMBER1) = TO_NUMBER('" + numberDate + "') and TO_NUMBER(SAJET.SYS_ORD_H.NUMBER2) = TO_NUMBER('" + number + "') and TO_NUMBER(SAJET.SYS_ORD_D.SEQUENCE) = TO_NUMBER('" + number3 + "')";
            DataSet dataSet1 = ClientUtils.ExecuteSQL(sSQL);
            if (dataSet1.Tables[0].Rows.Count <= 0)
                return;
            DataSet dataSet2 = ClientUtils.ExecuteSQL(" select * from SAJET.SYS_ORD_H           left join SAJET.SYS_ORD_D on TO_NUMBER(SAJET.SYS_ORD_H.NUMBER1) = TO_NUMBER(SAJET.SYS_ORD_D.NUMBER1) and TO_NUMBER(SAJET.SYS_ORD_H.NUMBER2) = TO_NUMBER(SAJET.SYS_ORD_D.NUMBER2)           left join SAJET.G_WO_BASE on TRIM(SAJET.SYS_ORD_D.PRODUCE_NUMBER) = TRIM(SAJET.G_WO_BASE.PRODUCE_NUMBER)  where TO_NUMBER(SAJET.SYS_ORD_H.NUMBER1) = TO_NUMBER('" + numberDate + "') and TO_NUMBER(SAJET.SYS_ORD_H.NUMBER2) = TO_NUMBER('" + number + "') and TO_NUMBER(SAJET.SYS_ORD_D.SEQUENCE) = TO_NUMBER('" + number3 + "')           and TRIM(SAJET.SYS_ORD_D.PRODUCE_NUMBER) = TRIM('" + dataSet1.Tables[0].Rows[0]["PRODUCE_NUMBER"].ToString() + "') ");
            string empty = string.Empty;
            int num = 0;
            if (dataSet2.Tables[0].Rows.Count > 0)
            {
                for (int index = 0; index < dataSet2.Tables[0].Rows.Count; ++index)
                {
                    if (dataSet2.Tables[0].Rows[index]["WORK_ORDER"].ToString().Length == 16)
                    {
                        string str = dataSet2.Tables[0].Rows[index]["WORK_ORDER"].ToString().Substring(dataSet1.Tables[0].Rows[0]["SEQUENCE"].ToString().Length - 1, 1);
                        if (str == dataSet1.Tables[0].Rows[0]["SEQUENCE"].ToString())
                        {
                            num += Convert.ToInt32(dataSet2.Tables[0].Rows[index]["QUANTITY"]);
                            listViewOrderDetail.Items.Add(
                                new ListViewItem(dataSet2.Tables[0].Rows[index]["OPERATION_ID"].ToString() + " " +
                                                 numberDate + " " + number + " " + str)
                                {
                                    SubItems =
                                    {
                                        dataSet2.Tables[0].Rows[index]["CUSTOMER_NAME"].ToString(),
                                        dataSet2.Tables[0].Rows[index]["REAL_DUE_DATE"].ToString(),
                                        dataSet2.Tables[0].Rows[index]["TARGET_QTY"].ToString(),
                                        num.ToString(),
                                        dataSet2.Tables[0].Rows[index]["TOTAL_NUMBER"].ToString()
                                    }
                                });
                        }
                    }
                }
            }
        }

        public void ListViewProduceDetail(string part_no, string route_name)
        {
            if (string.IsNullOrEmpty(route_name) || string.IsNullOrEmpty(part_no))
            {
                MessageBox.Show("途程與料號 不可為空");
                return;
            }
            else
            {
                string s = @"
/* 指定料號的基本資訊 */
WITH PART_INFO AS (
    SELECT
        PART_ID,
        ROUTE_ID
    FROM
        SAJET.SYS_PART
    WHERE
        PART_NO = :PART_NO
),
/* 該料號的預設生產途程的所有製程參數 */
PART_PARAMS AS (
    SELECT
        A.PROCESS_ID,
        A.ITEM_NAME
        || ' : '
        || A.VALUE_DEFAULT
        || ' '
        || B.UNIT_NO PRM
    FROM
        SAJET.SYS_RC_PROCESS_PARAM_PART   A,
        SAJET.SYS_UNIT                    B,
        PART_INFO                         C
    WHERE
        A.PART_ID = C.PART_ID
        AND A.UNIT_ID = B.UNIT_ID
    ORDER BY
        A.PROCESS_ID
),
/* 把製程參數依照製程 ID 分類 */
PROCESS_PARAMS AS (
    SELECT
        PROCESS_ID,
        LISTAGG(PRM, CHR(10)) WITHIN GROUP(
            ORDER BY
                PRM
        ) PARAMS
    FROM
        PART_PARAMS
    GROUP BY
        PROCESS_ID
),
/* 預設生產途程的製程按照順序排列 */
ROUTE_NODES AS (
    SELECT
        ROWNUM IDX,
        A.ROUTE_ID,
        NODE_CONTENT,
        NODE_ID
    FROM
        SAJET.SYS_RC_ROUTE_DETAIL   A,
        PART_INFO                   B
    START WITH A.ROUTE_ID = B.ROUTE_ID
               AND NODE_CONTENT = 'START' CONNECT BY PRIOR NEXT_NODE_ID = NODE_ID
                                                     OR PRIOR NEXT_NODE_ID = GROUP_ID
)
/* 排列好的生產途程與分好組的製程參數關聯起來 */
SELECT
    NVL(TRIM(B.ROUTE_NAME), '0') ROUTE_NAME,
    NVL(TRIM(A.PROCESS_CODE), '0') PROCESS_CODE,
    TRIM(A.PROCESS_NAME) PROCESS_NAME,
    D.PARAMS
FROM
    SAJET.SYS_PROCESS    A,
    SAJET.SYS_RC_ROUTE   B,
    ROUTE_NODES          C,
    PROCESS_PARAMS       D
WHERE
    B.ROUTE_ID = C.ROUTE_ID
    AND TO_CHAR(A.PROCESS_ID) = C.NODE_CONTENT
    AND A.PROCESS_ID = D.PROCESS_ID (+)
    AND A.ENABLED = 'Y'
    AND B.ENABLED = 'Y'
ORDER BY
    C.IDX
";
                var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.Number, "PART_NO", part_no },
                };

                var d = ClientUtils.ExecuteSQL(s, p.ToArray());

                var table = new DataTable();
                table.Columns.Add(SajetCommon.SetLanguage("Process"), typeof(string));
                table.Columns.Add(SajetCommon.SetLanguage("Item"), typeof(string));

                if (d != null && d.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in d.Tables[0].Rows)
                    {
                        string value = row["PARAMS"].ToString();

                        value = value.Replace($"{(char)10}", Environment.NewLine + Environment.NewLine);

                        var values = new object[]
                        {
                            row["PROCESS_NAME"].ToString(),
                            value,
                        };

                        table.Rows.Add(values);
                    }
                }

                #region 廢棄

                /*
                bool flag = true;
                string SQL = $@"
SELECT A.ROUTE_ID
      ,B.ROUTE_NAME
      ,B.ROUTE_DESC
FROM SAJET.SYS_RC_ROUTE_MAP A
    ,SAJET.SYS_RC_ROUTE B
WHERE A.ROUTE_ID = B.ROUTE_ID
AND B.ENABLED = 'Y'
AND B.ROUTE_NAME LIKE '%{route_name}%'
ORDER BY A.ROUTE_ID DESC
";
                string ROUTE_ID = ClientUtils.ExecuteSQL(SQL).Tables[0].Rows[0]["ROUTE_ID"].ToString();
                SQL = $@"
select *
from sajet.SYS_RC_ROUTE_DETAIL
where route_id = '{ROUTE_ID}'
and NODE_TYPE='0'
";
                string NEXT_NODE_ID = ClientUtils.ExecuteSQL(SQL).Tables[0].Rows[0]["NEXT_NODE_ID"].ToString();

                SQL = $@"
select a.PART_ID
      ,f.PART_NO
      ,f.SPEC1
      ,a.ROUTE_ID
      ,e.ROUTE_NAME
      ,e.ROUTE_DESC
      ,b.NODE_ID
      ,b.NODE_TYPE
      ,b.NODE_CONTENT
      ,c.PROCESS_NAME
      ,c.PROCESS_DESC
      ,c.process_id
      ,d.STAGE_CODE
      ,d.STAGE_DESC
      ,a.ENABLED
from SAJET.SYS_PART_ROUTE a
left outer join SAJET.SYS_RC_ROUTE e on a.ROUTE_ID = e.ROUTE_ID
left outer join SAJET.SYS_RC_ROUTE_DETAIL B on a.ROUTE_ID = b.ROUTE_ID
left outer join SAJET.SYS_PROCESS c on c.process_ID = b.NODE_CONTENT
left outer join SAJET.SYS_STAGE d on d.STAGE_id = c.STAGE_id
left outer join SAJET.SYS_PART f on f.PART_ID = a.PART_ID
where f.PART_NO = '{part_no}'
and b.NODE_TYPE = 1
and a.ROUTE_ID = '{ROUTE_ID}'
";
                DataSet dataSet1 = ClientUtils.ExecuteSQL(SQL);
                if (dataSet1.Tables[0].Rows.Count == 0)
                    return;

                dgvRoute.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgvRoute.Dock = DockStyle.Fill;
                dgvRoute.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add(SajetCommon.SetLanguage("Process"), typeof(string));
                dataTable.Columns.Add(SajetCommon.SetLanguage("Item"), typeof(string));
                while (flag)
                {
                    string format = @"
SELECT
    A.*,
    B.PROCESS_NAME
FROM
    SAJET.SYS_RC_ROUTE_DETAIL   A,
    SAJET.SYS_PROCESS           B
WHERE
    A.ROUTE_ID = :ROUTE_ID
    AND A.NODE_ID = :NODE_ID
    AND A.NODE_CONTENT = TO_CHAR(B.PROCESS_ID)
    AND A.LINK_NAME = 'NEXT'
";
                    p = new List<object[]>
                    {
                        new object[] { ParameterDirection.Input, OracleType.Number, "ROUTE_ID", ROUTE_ID },
                        new object[] { ParameterDirection.Input, OracleType.Number, "NODE_ID", NEXT_NODE_ID },
                    };

                    DataSet dataSet2 = ClientUtils.ExecuteSQL(format, p.ToArray());
                    if (dataSet2.Tables[0].Rows.Count == 0)
                    {
                        flag = false;
                    }
                    else
                    {
                        DataRow[] dataRowArray = dataSet1.Tables[0].Select("NODE_ID =" + NEXT_NODE_ID);
                        StringBuilder itemNameList = getITEM_NAME_LIST(dataRowArray[0]["part_id"].ToString(), dataRowArray[0]["process_id"].ToString());
                        dataTable.Rows.Add(dataRowArray[0]["process_name"].ToString() + " " + dataRowArray[0]["process_desc"].ToString(), itemNameList.ToString());
                        NEXT_NODE_ID = dataSet2.Tables[0].Rows[0]["NEXT_NODE_ID"].ToString();
                        dataSet2.Tables[0].Rows[0]["PROCESS_NAME"].ToString();
                    }
                }

                dgvRoute.DataSource = dataTable;
                //*/

                #endregion

                dgvRoute.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                dgvRoute.Dock = DockStyle.Fill;

                dgvRoute.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;

                dgvRoute.DataSource = table;

                dgvRoute.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
        }

        private string getPartNo(string PartID)
        {
            return ClientUtils.ExecuteSQL(" select *   from sajet.sys_part  where enabled = 'Y'  and part_id = '" + PartID + "'").Tables[0].Rows[0]["PART_NO"].ToString();
        }

        public StringBuilder getITEM_NAME_LIST(ListViewItem items, string partid, string processid)
        {
            DataSet dataSet = ClientUtils.ExecuteSQL(" SELECT A.ITEM_NAME,A.ITEM_PHASE, A.VALUE_DEFAULT, A.VALUE_TYPE, A.INPUT_TYPE,   A.NECESSARY, A.CONVERT_TYPE, A.VALUE_LIST, A.ITEM_ID, B.UNIT_NO    FROM SAJET.SYS_RC_PROCESS_PARAM_PART A,  SAJET.SYS_UNIT B    WHERE  A.ITEM_PHASE IN ('A', 'I','O')    AND A.ITEM_TYPE = 0 AND A.ENABLED = 'Y'      AND A.UNIT_ID = B.UNIT_ID(+)     AND A.PART_ID = '" + partid + "'   AND A.PROCESS_ID = '" + processid + "'     ORDER BY A.ITEM_SEQ  ");
            StringBuilder stringBuilder = new StringBuilder();
            for (int index = 0; index < dataSet.Tables[0].Rows.Count; ++index)
                stringBuilder.AppendLine(dataSet.Tables[0].Rows[index]["ITEM_NAME"].ToString() + " : " + dataSet.Tables[0].Rows[index]["VALUE_DEFAULT"].ToString() + " " + dataSet.Tables[0].Rows[index]["UNIT_NO"].ToString() + Environment.NewLine);
            return stringBuilder;
        }

        public StringBuilder getITEM_NAME_LIST(string partid, string processid)
        {
            DataSet dataSet = ClientUtils.ExecuteSQL(" SELECT A.ITEM_NAME,A.ITEM_PHASE, A.VALUE_DEFAULT, A.VALUE_TYPE, A.INPUT_TYPE,   A.NECESSARY, A.CONVERT_TYPE, A.VALUE_LIST, A.ITEM_ID, B.UNIT_NO    FROM SAJET.SYS_RC_PROCESS_PARAM_PART A,  SAJET.SYS_UNIT B    WHERE  A.ITEM_PHASE IN ('A', 'I','O')    AND A.ITEM_TYPE = 0 AND A.ENABLED = 'Y'      AND A.UNIT_ID = B.UNIT_ID(+)     AND A.PART_ID = '" + partid + "'   AND A.PROCESS_ID = '" + processid + "'     ORDER BY A.ITEM_SEQ  ");
            StringBuilder stringBuilder = new StringBuilder();
            for (int index = 0; index < dataSet.Tables[0].Rows.Count; ++index)
                stringBuilder.AppendLine(dataSet.Tables[0].Rows[index]["ITEM_NAME"].ToString() + " : " + dataSet.Tables[0].Rows[index]["VALUE_DEFAULT"].ToString() + " " + dataSet.Tables[0].Rows[index]["UNIT_NO"].ToString() + Environment.NewLine);
            return stringBuilder;
        }

        public string ItemPartNoSQL(string PartNo)
        {
            var str = $@"
SELECT
    aa.alevel,
    aa.part_id,
    bb.part_no,
    cc.part_no item_part_no,
    aa.part_id,
    aa.item_part_id
FROM
    (
        SELECT
            a.*,
            level alevel
        FROM
            (
                SELECT
                    b.part_id,
                    a.item_part_id,
                    a.bom_id,
                    a.process_id
                FROM
                    sajet.sys_bom        a,
                    sajet.sys_bom_info   b
                WHERE
                    a.bom_id = b.bom_id
            ) a
        START WITH
            part_id = (
                SELECT
                    part_id
                FROM
                    sajet.sys_part
                WHERE
                    part_no = '{PartNo.Trim()}'
            )
        CONNECT BY
            PRIOR item_part_id = part_id
    ) aa,
    sajet.sys_part   bb,
    sajet.sys_part   cc
WHERE
    aa.part_id = bb.part_id
    AND aa.item_part_id = cc.part_id
GROUP BY
    aa.alevel,
    bb.part_no,
    cc.part_no,
    aa.part_id,
    aa.item_part_id
ORDER BY
    alevel,
    bb.part_no,
    cc.part_no
";
            return str;
        }

        public void SetCombProduce(string producenumber, string strOperation)
        {
            strProduceNumber = producenumber;

            string empty1 = string.Empty;
            string empty2 = string.Empty;
            string empty3 = string.Empty;

            string str1 = strOperation;
            string PartNo = producenumber;

            List<string> stringList1 = new List<string>();
            List<string> stringList2 = new List<string>();
            List<string> stringList3 = new List<string>();

            DataSet dataSet = ClientUtils.ExecuteSQL(ItemPartNoSQL(PartNo));
            if (dataSet.Tables[0].Rows.Count < 1)
            {
                int num = (int)MessageBox.Show("無此對應料號:" + PartNo + "請至物料清單建置");
            }
            else
            {
                for (int index = 0; index <= dataSet.Tables[0].Rows.Count - 1; ++index)
                {
                    string str2 = dataSet.Tables[0].Rows[index]["ITEM_PART_NO"].ToString();
                    //if (str2.IndexOf('C') == 0)
                    {
                        stringList3.Add(str2);
                    }
                }

                var listPartNo = new List<string>();
                for (int index = 0; index <= dataSet.Tables[0].Rows.Count - 1; ++index)
                {
                    string partNo = dataSet.Tables[0].Rows[index]["PART_NO"].ToString();
                    listPartNo.Add(partNo);
                }

                var commonItems = listPartNo.Intersect(stringList3).ToList();
                cbProd10B.DataSource = commonItems;
            }
        }

        // TODO: seems useless
        public void SetCombProduce(DataSet dsTemp, string strOperation)
        {
            string empty1 = string.Empty;
            string empty2 = string.Empty;
            string empty3 = string.Empty;

            string str1 = strOperation;

            List<string> stringList1 = new List<string>();
            List<string> stringList2 = new List<string>();

            switch (str1)
            {
                case "10A":
                    {
                        List<string> stringList3 = new List<string>();

                        for (int index1 = 0; index1 < dsTemp.Tables[0].Rows.Count; ++index1)
                        {
                            strProduceNumber = dsTemp.Tables[0].Rows[index1]["PRODUCE_NUMBER"].ToString();

                            string PartNo = dsTemp.Tables[0].Rows[index1]["PRODUCE_NUMBER"].ToString();

                            DataSet dataSet = ClientUtils.ExecuteSQL(ItemPartNoSQL(PartNo));

                            if (dataSet.Tables[0].Rows.Count < 1)
                            {
                                int num = (int)MessageBox.Show("無此對應料號：" + PartNo + " 請至物料清單建置");

                                this.Close();

                                return;
                            }

                            for (int index2 = 0; index2 <= dataSet.Tables[0].Rows.Count - 1; ++index2)
                            {
                                string str2 = dataSet.Tables[0].Rows[index2]["ITEM_PART_NO"].ToString();

                                //if (str2.IndexOf('C') == 0)
                                stringList3.Add(str2);
                            }
                        }

                        cbProd10B.DataSource = stringList3;

                        break;
                    }
                case "10B":
                    {
                        List<string> stringList3 = new List<string>();

                        for (int index1 = 0; index1 < dsTemp.Tables[0].Rows.Count; ++index1)
                        {
                            strProduceNumber = dsTemp.Tables[0].Rows[index1]["PRODUCE_NUMBER"].ToString();

                            string PartNo = dsTemp.Tables[0].Rows[index1]["PRODUCE_NUMBER"].ToString();

                            DataSet dataSet = ClientUtils.ExecuteSQL(ItemPartNoSQL(PartNo));

                            if (dataSet.Tables[0].Rows.Count < 1)
                            {
                                int num = (int)MessageBox.Show("無此對應料號：" + PartNo + " 請至物料清單建置");

                                this.Close();

                                return;
                            }

                            for (int index2 = 0; index2 <= dataSet.Tables[0].Rows.Count - 1; ++index2)
                            {
                                string str2 = dataSet.Tables[0].Rows[index2]["ITEM_PART_NO"].ToString();
                                //if (str2.IndexOf('C') == 0)

                                stringList3.Add(str2);
                            }
                        }

                        cbProd10B.DataSource = stringList3;

                        cbProd10A.DataSource = new List<string>()
                        {
                            ""
                        };

                        break;
                    }
                case "10C":
                    {
                        strProduceNumber = dsTemp.Tables[0].Rows[0]["PRODUCE_NUMBER"].ToString();

                        List<string> stringList3 = new List<string>();

                        if (dsTemp.Tables[0].Rows.Count < 1)
                        {
                            int num = (int)MessageBox.Show("無此對應料號：" + empty1 + " 請至物料清單建置");

                            this.Close();

                            return;
                        }
                        else
                        {
                            for (int index = 0; index < dsTemp.Tables[0].Rows.Count; ++index)
                            {
                                stringList3.Add(dsTemp.Tables[0].Rows[index]["PRODUCE_NUMBER"].ToString());
                            }

                            cbProd10B.DataSource = stringList3;

                            cbProd10A.DataSource = new List<string>()
                            {
                                ""
                            };
                        }

                        break;
                    }
                default:
                    return;
            }
        }

        public bool CheckProduceNumber(string operation_id, string numberDate, string number)
        {
            sSQL = " select * from SAJET.SYS_ORD_H  left join SAJET.SYS_ORD_D on SAJET.SYS_ORD_H.NUMBER1 = SAJET.SYS_ORD_D.NUMBER1 and SAJET.SYS_ORD_H.NUMBER2 = SAJET.SYS_ORD_D.NUMBER2  where FLAG='Y' and SAJET.SYS_ORD_H.NUMBER1 = '" + numberDate + "' and SAJET.SYS_ORD_H.NUMBER2 = '" + number + "' and SAJET.SYS_ORD_D.SEQUENCE = '" + Convert.ToInt32(txtNum.Text) + "'";
            DataSet dataSet1 = ClientUtils.ExecuteSQL(sSQL);
            if (dataSet1.Tables[0].Rows.Count <= 0)
                return false;
            strCustomer = dataSet1.Tables[0].Rows[0]["CUSTOMER_ID"].ToString();
            strCustomer2 = dataSet1.Tables[0].Rows[0]["CUSTOMER_NAME"].ToString();
            strSaleID = dataSet1.Tables[0].Rows[0]["SALE_ID"].ToString();
            strCustomize = dataSet1.Tables[0].Rows[0]["CUSTOMIZE"].ToString();
            strDate = Convert.ToDateTime(dataSet1.Tables[0].Rows[0]["REAL_DATE"].ToString()).ToString("yyyy/MM/dd");
            strDueDate = Convert.ToDateTime(dataSet1.Tables[0].Rows[0]["REAL_DUE_DATE"].ToString()).ToString("yyyy/MM/dd");
            strUnit = dataSet1.Tables[0].Rows[0]["UNIT"].ToString();
            DataSet dataSet2 = ClientUtils.ExecuteSQL(" select * from SAJET.SYS_ORD_H           left join SAJET.SYS_ORD_D on SAJET.SYS_ORD_H.NUMBER1 = SAJET.SYS_ORD_D.NUMBER1 and SAJET.SYS_ORD_H.NUMBER2 = SAJET.SYS_ORD_D.NUMBER2           left join SAJET.G_WO_BASE on SAJET.SYS_ORD_D.PRODUCE_NUMBER = SAJET.G_WO_BASE.PRODUCE_NUMBER  where SAJET.SYS_ORD_H.NUMBER1 = '" + numberDate + "' and SAJET.SYS_ORD_H.NUMBER2 = '" + number + "' and SAJET.SYS_ORD_D.SEQUENCE = '" + txtNum.Text + "'           and SAJET.SYS_ORD_D.PRODUCE_NUMBER = '" + dataSet1.Tables[0].Rows[0]["PRODUCE_NUMBER"].ToString() + "' ");
            int num = 0;
            string str1 = string.Empty;
            int int32 = Convert.ToInt32(dataSet1.Tables[0].Rows[0]["SEQUENCE"]);
            if (dataSet2.Tables[0].Rows[0]["WORK_ORDER"] != null && dataSet2.Tables[0].Rows[0]["WORK_ORDER"].ToString() != "")
            {
                if (dataSet2.Tables[0].Rows.Count > 0)
                {
                    for (int index = 0; index < dataSet2.Tables[0].Rows.Count; ++index)
                    {
                        if (dataSet2.Tables[0].Rows[index]["WORK_ORDER"].ToString().Length == 16)
                        {
                            str1 = dataSet2.Tables[0].Rows[index]["WORK_ORDER"].ToString().Substring(15, 1);
                            if (str1 == int32.ToString())
                                ++num;
                        }
                    }
                    string str2 = num.ToString() + str1;
                    if (str2.Length == 2)
                        txtNum.Text = "0" + str2;
                    strTotalNum = dataSet2.Tables[0].Rows[0]["TARGET_QTY"].ToString();
                }
                else
                    txtNum.Text = GetSequence(dataSet1, numberDate, number);
            }
            else
                txtNum.Text = GetSequence(dataSet1, numberDate, number);
            SetCombProduce(dataSet1, operation_id);
            return true;
        }

        private void Check()
        {
            try
            {
                listViewOrderDetail.Items.Clear();
                string str1 = combOperation.SelectedValue.ToString();
                string numberDate = cbNumber1.SelectedValue.ToString();
                string str2 = cbNumber2.SelectedValue.ToString();
                combMade.SelectedValue.ToString();
                if (string.IsNullOrEmpty(txtNum.Text))
                {
                    fData.strNumber3 = txtNum.Text;
                    if (CheckProduceNumber(str1, numberDate, str2))
                    {
                        txtClient.Text = strCustomer;
                        txtClient2.Text = strCustomer2;
                        txtSales.Text = strSaleID;
                        txtCustomize.Text = strCustomize;
                        string number = SetNumber(str2, 4);
                        tbWO.Text = combOperation.SelectedValue.ToString() + cbNumber1.SelectedValue.ToString() + number + txtNum.Text;
                        txtRule1.Text = strSpec1;

                        dtCustDate.Text = strDate;
                        dtCustDueDate.Text = strDueDate;

                        dtScheduleDate.Text = strDate;
                        dtDueDate.Text = strDueDate;
                        txtUnit.Text = strUnit;
                        editTargetQty.Text = strTotalNum;
                        ListViewOrderDetail1(str1, numberDate, number, fData.strNumber3, editTargetQty.Text, g_sUpdateType, "");
                        ListViewProduceDetail(editPart.Text, combRoute.Text);
                    }
                    else
                    {
                        int num = (int)MessageBox.Show("工單不存在");
                    }
                }
                else
                {
                    int num1 = (int)MessageBox.Show("工單編號不完整");
                }
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message.ToString());
            }
        }

        public void SetPart(string strCheck = "")
        {
            if (strCheck != "")
                editPart.Text = strCheck;
            else
                editPart.Text = ClientUtils.ExecuteSQL(ItemPartNoSQL(cbProd10A.SelectedValue.ToString())).Tables[0].Rows[0]["ITEM_PART_NO"].ToString();
        }

        public string SetNumber(string strNumber1 = "", int count = 0)
        {
            int length = strNumber1.Length;
            switch (count)
            {
                case 3:
                    switch (length)
                    {
                        case 1:
                            strNumber1 = "00" + strNumber1;
                            break;
                        case 2:
                            strNumber1 = "0" + strNumber1;
                            break;
                    }
                    break;
                case 4:
                    switch (length)
                    {
                        case 1:
                            strNumber1 = "000" + strNumber1;
                            break;
                        case 2:
                            strNumber1 = "00" + strNumber1;
                            break;
                        case 3:
                            strNumber1 = "0" + strNumber1;
                            break;
                    }
                    break;
            }
            return strNumber1;
        }

        public void ComboInitial()
        {
            combOperation.DataSource = new List<string>()
            {
                "10A",
                "10B",
                "10C"
            };
            string[] strArray = new string[5]
            {
                "試製",
                "樣試",
                "量試",
                "量產",
                "重工"
            };
            List<cboMadeCategoryList> madeCategoryListList = new List<cboMadeCategoryList>();
            for (int index = 0; index < strArray.Length; ++index)
                madeCategoryListList.Add(new cboMadeCategoryList()
                {
                    cbo_Name = strArray[index] ?? "",
                    cbo_Value = string.Concat(index + 1)
                });
            combMade.DisplayMember = "cbo_Name";
            combMade.ValueMember = "cbo_Value";
            combMade.DataSource = madeCategoryListList;
            DateTimeNumber();
        }

        public void DateTimePickerInitial()
        {
            DateTime now = DateTime.Now;
            dtCustDate.Value = now;
            dtCustDueDate.Value = now;
            dtScheduleDate.Value = now;
            dtDueDate.Value = now;
        }

        /// <summary>
        /// 檢查 BOM 表關聯 ComboBox，有缺少就關閉畫面
        /// </summary>
        private void CheckBOMComboBox()
        {
            string p10A = cbProd10A.SelectedItem?.ToString();
            string p10B = cbProd10B.SelectedItem?.ToString();
            string p10C = cbProd10C.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(p10A) || string.IsNullOrWhiteSpace(p10B) || string.IsNullOrWhiteSpace(p10C))
            {
                string message
                    = SajetCommon.SetLanguage("Incomplete BOM association")
                    + Environment.NewLine
                    + labOperation.Text + SajetCommon.SetLanguage(":") + combOperation.Text + "/ "
                    + labNumber1.Text + SajetCommon.SetLanguage(":") + cbNumber1.Text + "/ "
                    + lblNumber2.Text + SajetCommon.SetLanguage(":") + cbNumber2.Text + "/ "
                    + lblOrderSequence.Text + SajetCommon.SetLanguage(":") + cbOrderSequence.Text
                    ;

                SajetCommon.Show_Message(message, 0);

                this.Close();
            }
        }

        private void txtSequence_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter)
            {
                return;
            }

            if (txtSequence.TextLength == 2 && int.TryParse(txtSequence.Text, out int i))
            {
                SetWorkOrder();
            }
            else
            {
                string message
                    = SajetCommon.SetLanguage("Sequence is required to be two digits non-negative integer")
                    ;

                SajetCommon.Show_Message(message, 0);

                txtSequence.SelectAll();

                txtSequence.Focus();
            }
        }
    }
}
