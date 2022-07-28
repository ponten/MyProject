using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using SajetFilter;
using System.Collections.Specialized;
using System.Text.RegularExpressions;//導入命名空間(正則表達式)
using System.Data.OracleClient;
using PartAQL.Models;

namespace PartAQL
{
    public partial class fCollection : Form
    {
        public string g_sProcess, g_sPart, g_sType, g_sPartId, g_sProcessId, g_sItemType, g_sUserID;
        public string g_sPhase, g_sValue, g_sInput, g_sConvert, g_sNecessary, g_sItemId, g_sPrint;
        public string g_sSeq, g_sName, g_sValueList;
        string sSQL;

        /// <summary>
        /// 料號資訊
        /// </summary>
        public PartDetailModel PartDetail;

        public ProcessDetailModel CurrentNodeDetail;

        Dictionary<string, string> dicUnit = new Dictionary<string, string>(); // 記錄單位資料 unit_id & unit_no

        public fCollection()
        {
            InitializeComponent();
        }

        private void Initial_form()
        {
            editName.Text = string.Empty;
            combPhase.SelectedIndex = 0;
            combValueType.SelectedIndex = 0;
            combInputType.SelectedIndex = 0;
            combConvert.SelectedIndex = 0;
            combNecessary.SelectedIndex = 0;
            combPrint.SelectedIndex = 1;
            editDefault.Text = string.Empty;
            editMin.Text = string.Empty;
            editMax.Text = string.Empty;
            g_sValueList = string.Empty;
            lbxItem.Items.Clear();
            //editColumn.Text = string.Empty;
            //editRow.Text = string.Empty;
            // 2.0.0.13
            editColumn.Text = "1";
            editRow.Text = "1";
            //enableData();
            combUnit.Items.Clear(); // Reset
            dicUnit = new Dictionary<string, string>();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!chkData()) return;

            // 項目階段 A: 全部  I: WIP投入 O: WIP 產出
            switch (combPhase.SelectedIndex)
            {
                case 0:
                    g_sPhase = "A";
                    break;
                case 1:
                    g_sPhase = "I";
                    break;
                case 2:
                    g_sPhase = "O";
                    break;
                default:
                    g_sPhase = "A";
                    break;
            }

            // 數值類型    V:文字  N:數字
            switch (combValueType.SelectedIndex)
            {
                case 0:
                    g_sValue = "V";
                    break;
                case 1:
                    g_sValue = "N";
                    break;
                default:
                    g_sValue = "V";
                    break;
            }

            // 輸入方式    K: KeyIn     S: Select List     R: Range (項目值為數字)
            switch (combInputType.SelectedIndex)
            {
                case 0:
                    g_sInput = "K";
                    break;
                case 1:
                    g_sInput = "S";
                    break;
                case 2:
                    g_sInput = "R";
                    break;
                default:
                    g_sInput = "K";
                    break;
            }

            // 輸入值轉換       N: None     U: Uppercase   L: Lowercase
            switch (combConvert.SelectedIndex)
            {
                case 0:
                    g_sConvert = "N";
                    break;
                case 1:
                    g_sConvert = "U";
                    break;
                case 2:
                    g_sConvert = "L";
                    break;
                default:
                    g_sConvert = "N";
                    break;
            }

            //項目是否為必要輸入欄位     Y:必要    N:非必要
            switch (combNecessary.SelectedIndex)
            {
                case 0:
                    g_sNecessary = "Y";
                    break;
                case 1:
                    g_sNecessary = "N";
                    break;
                default:
                    g_sNecessary = "Y";
                    break;
            }

            //項目是否為流程卡打印顯示     Y:是    N:否
            switch (combPrint.SelectedIndex)
            {
                case 0:
                    g_sPrint = "N";
                    break;
                case 1:
                    g_sPrint = "Y";
                    break;
                default:
                    g_sPrint = "N";
                    break;
            }

            if (!chkCollectionData()) return; //檢查收集的參數

            string sUnitId = "0";
            if (string.IsNullOrEmpty(combUnit.SelectedItem?.ToString()))
            {
                combUnit.SelectedItem = "---";
            }

            foreach (var item in dicUnit)
            {
                if (item.Value.ToString() == combUnit.SelectedItem.ToString())
                {
                    sUnitId = item.Key.ToString();
                    break;
                }
            }

            string s;

            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.Number, "part_id", PartDetail.PartID },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "version", PartDetail.Version },
                new object[] { ParameterDirection.Input, OracleType.Number, "route_id", CurrentNodeDetail.RouteID },
                new object[] { ParameterDirection.Input, OracleType.Number, "node_id", CurrentNodeDetail.NodeID },
                new object[] { ParameterDirection.Input, OracleType.Number, "process_id", CurrentNodeDetail.ProcessID },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "item_type", g_sItemType },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "item_phase", g_sPhase },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "input_type", g_sInput },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "value_type", g_sValue },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "value_default", editDefault.Text },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "value_list", g_sValueList },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "convert_type", g_sConvert },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "column_item", editColumn.Text.Trim() },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "row_item", editRow.Text.Trim() },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "necessary", g_sNecessary },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "print", g_sPrint },
                new object[] { ParameterDirection.Input, OracleType.Number, "unit_id", sUnitId },
                new object[] { ParameterDirection.Input, OracleType.Number, "update_userid", g_sUserID },
                new object[] { ParameterDirection.Input, OracleType.DateTime, "update_time", DateTime.Now },
            };

            if (g_sType == "Add")
            {
                string ItemId = SajetCommon.GetMaxID("SAJET.SYS_PART_QC_ITEM", "ITEM_ID", 8);

                s = @"
INSERT INTO sajet.sys_part_qc_item (
    part_id,
    version,
    route_id,
    node_id,
    process_id,
    item_id,
    item_name,
    item_type,
    item_phase,
    item_seq,
    input_type,
    value_type,
    value_default,
    value_list,
    convert_type,
    column_item,
    row_item,
    necessary,
    print,
    unit_id,
    update_userid,
    update_time,
    enabled
) VALUES (
    :part_id,
    :version,
    :route_id,
    :node_id,
    :process_id,
    :item_id,
    :item_name,
    :item_type,
    :item_phase,
    :item_seq,
    :input_type,
    :value_type,
    :value_default,
    :value_list,
    :convert_type,
    :column_item,
    :row_item,
    :necessary,
    :print,
    :unit_id,
    :update_userid,
    :update_time,
    'Y'
)";
                p.Add(new object[] { ParameterDirection.Input, OracleType.Number, "item_id", ItemId });
                p.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "item_name", editName.Text });
                p.Add(new object[] { ParameterDirection.Input, OracleType.Number, "item_seq", nudSeq.Value });
            }
            else // Modify
            {
                s = @"
UPDATE sajet.sys_part_qc_item
SET
    item_name = :item_name,
    item_phase = :item_phase,
    item_seq = :item_seq,
    input_type = :input_type,
    value_type = :value_type,
    value_default = :value_default,
    value_list = :value_list,
    convert_type = :convert_type,
    column_item = :column_item,
    row_item = :row_item,
    necessary = :necessary,
    print = :print,
    unit_id = :unit_id,
    update_userid = :update_userid,
    update_time = :update_time
WHERE
    part_id = :part_id
    AND version = :version
    AND route_id = :route_id
    AND node_id = :node_id
    AND item_id = :item_id
    AND item_seq = :item_seq
    AND item_name = :item_name
";
                p.Add(new object[] { ParameterDirection.Input, OracleType.Number, "item_id", g_sItemId });
                p.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "item_name", g_sName });
                p.Add(new object[] { ParameterDirection.Input, OracleType.Number, "item_seq", g_sSeq });
            }

            ClientUtils.ExecuteSQL(s, p.ToArray());

            string message = SajetCommon.SetLanguage("Add another one");

            if (g_sType == "Add" &&
                SajetCommon.Show_Message(message, 2) == DialogResult.Yes)
            {
                fCollection_Load(sender, e);
            }
            else
            {
                DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        /// 檢查收集數據的條件值
        /// </summary>
        /// <returns></returns>
        private bool chkCollectionData()
        {
            switch (combInputType.SelectedIndex)
            {
                case 0: // 輸入預設值
                    lbxItem.Items.Clear();
                    editMax.Text = string.Empty;
                    editMin.Text = string.Empty;

                    break;
                case 1:    // 檢查清單
                    editDefault.Text = string.Empty;
                    editMax.Text = string.Empty;
                    editMin.Text = string.Empty;
                    if (lbxItem.Items.Count == 0)
                    {
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Create List item."), 0);
                        return false;
                    }
                    else
                    {
                        for (int i = 0; i < lbxItem.Items.Count; i++)
                        {
                            g_sValueList = g_sValueList + lbxItem.Items[i].ToString() + ",";
                        }
                    }
                    break;
                case 2:    // 檢查上下限值與數值
                    editDefault.Text = string.Empty;
                    lbxItem.Items.Clear();
                    if (string.IsNullOrEmpty(editMin.Text))
                    {
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Input min Value."), 0);
                        return false;
                    }
                    if (string.IsNullOrEmpty(editMax.Text))
                    {
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Input Max Value."), 0);
                        return false;
                    }
                    if (IsNumeric(editMin.Text))
                    {
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Input min Value in Number"), 0);
                        return false;
                    }
                    if (IsNumeric(editMax.Text))
                    {
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Input Max Value in Number"), 0);
                        return false;
                    }
                    g_sValueList = editMin.Text + "," + editMax.Text;
                    break;
            }
            return true;
        }

        //定義一個函數,作用:判斷strNumber是否為數字,是數字返回True,不是數字返回False
        public bool IsNumeric(string strNumber)
        {
            Regex NumberPattern = new Regex("[^0-9.-]");
            return NumberPattern.IsMatch(strNumber);
        }

        //定義一個函數,作用:判斷alphabe是否為英文字母,是英文字母返回True,不是英文字母返回False
        public bool IsAlphabe(string alphabe)
        {
            Regex NumberPattern = new Regex(@"^[A-Za-z0-9]+$");
            return NumberPattern.IsMatch(alphabe);
        }

        /// <summary>
        ///  判斷strNumber是否為指定類型的數字
        ///  1:正整數, 2:非負整數（正整數 + 0）, 3:正浮點數, 4:非負浮點數（正浮點數 + 0）, 5:浮點數
        /// </summary>
        /// <param name="iType"> 數值類型 </param>
        /// <param name="strNumber">判斷的字串</param>
        /// <returns>是返回True,否返回False</returns>
        public bool IsNumeric(int iType, string strNumber)
        {
            Regex NumberPattern = null;
            switch (iType)
            {
                case 1:   //正整數
                    NumberPattern = new Regex("^[0-9]*[1-9][0-9]*$");
                    break;
                case 2:   //非負整數（正整數 + 0）
                    NumberPattern = new Regex("^\\d+$");
                    break;
                case 3:   //正浮點數
                    NumberPattern = new Regex("^(([0-9]+\\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\\.[0-9]+)|([0-9]*[1-9][0-9]*))$");
                    break;
                case 4:   //非負浮點數（正浮點數 + 0）
                    NumberPattern = new Regex("^\\d+(\\.\\d+)?$");
                    break;
                case 5:    //浮點數
                    NumberPattern = new Regex("^(-?\\d+)(\\.\\d+)?$");
                    break;
                default:
                    return false;
                    //break;
            }
            return NumberPattern.IsMatch(strNumber);
        }

        private bool chkData()
        {
            try
            {
                string s = @"
SELECT
    item_seq
FROM
    sajet.sys_part_qc_item
WHERE
    part_id = :part_id
    AND version = :version
    AND route_id = :route_id
    AND node_id = :node_id
    AND item_type = :item_type
    AND item_seq = :item_seq
";
                var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.Number, "part_id", PartDetail.PartID },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "version", string.IsNullOrWhiteSpace(PartDetail.Version) ? "N/A" : PartDetail.Version },
                    new object[] { ParameterDirection.Input, OracleType.Number, "route_id", CurrentNodeDetail.RouteID },
                    new object[] { ParameterDirection.Input, OracleType.Number, "node_id", CurrentNodeDetail.NodeID },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "item_type", g_sItemType },
                    new object[] { ParameterDirection.Input, OracleType.Number, "item_seq", nudSeq.Value },
                };

                if (g_sType == "Modify")
                {
                    s += " AND item_name <> :item_name ";

                    p.Add(new object[] { ParameterDirection.Input, OracleType.Number, "item_name", editName.Text.Trim() });
                }

                var d = ClientUtils.ExecuteSQL(s, p.ToArray());

                if (d.Tables[0].Rows.Count > 0)
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Item Sequence is Duplicate."), 0);
                    return false;
                }

                if (string.IsNullOrEmpty(editName.Text))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Input Item Name."), 0);
                    return false;
                }
                else
                {
                    s = @"
SELECT
    *
FROM
    sajet.sys_part_qc_item
WHERE
    part_id = :part_id
    AND version = :version
    AND route_id = :route_id
    AND node_id = :node_id
    AND item_type = :item_type
    AND item_name = :item_name
";

                    p = new List<object[]>
                    {
                        new object[] { ParameterDirection.Input, OracleType.Number, "part_id", PartDetail.PartID },
                        new object[] { ParameterDirection.Input, OracleType.VarChar, "version", string.IsNullOrWhiteSpace(PartDetail.Version) ? "N/A" : PartDetail.Version },
                        new object[] { ParameterDirection.Input, OracleType.Number, "route_id", CurrentNodeDetail.RouteID },
                        new object[] { ParameterDirection.Input, OracleType.Number, "node_id", CurrentNodeDetail.NodeID },
                        new object[] { ParameterDirection.Input, OracleType.VarChar, "item_type", g_sItemType },
                        new object[] { ParameterDirection.Input, OracleType.VarChar, "item_name", editName.Text.Trim() },
                    };

                    if (g_sType == "Modify")
                    {
                        sSQL += " AND item_seq <> :item_seq ";

                        p.Add(new object[] { ParameterDirection.Input, OracleType.Number, "item_seq", nudSeq.Value });
                    }

                    d = ClientUtils.ExecuteSQL(s, p.ToArray());

                    if (d.Tables[0].Rows.Count > 0)
                    {
                        if (g_sType == "Add")
                        {
                            SajetCommon.Show_Message(SajetCommon.SetLanguage("Item Name is duplicate."), 0);
                            editName.SelectAll();
                            editName.Focus();
                            return false;
                        }
                    }
                }

                if (combPrint.SelectedIndex > 0)
                {
                    if (!IsNumeric(1, editColumn.Text))
                    {
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Input Column Item is Number."), 0);
                        return false;
                    }

                    if (!IsNumeric(1, editRow.Text))
                    {
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Input Row Item is Number."), 0);
                        return false;
                    }
                }
                return true;
            }
            catch (Exception)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Check Data Error."), 0);
                return false;
            }
        }

        private void showData(DataSet dsTemp)
        {
            nudSeq.Value = Convert.ToDecimal(dsTemp.Tables[0].Rows[0]["ITEM_SEQ"]);
            editName.Text = dsTemp.Tables[0].Rows[0]["ITEM_NAME"].ToString();
            combPhase.SelectedIndex = dataConvert("combPhase", dsTemp.Tables[0].Rows[0]["ITEM_PHASE"].ToString());
            combValueType.SelectedIndex = dataConvert("combValueType", dsTemp.Tables[0].Rows[0]["VALUE_TYPE"].ToString());
            combInputType.SelectedIndex = dataConvert("combInputType", dsTemp.Tables[0].Rows[0]["INPUT_TYPE"].ToString());
            editDefault.Text = dsTemp.Tables[0].Rows[0]["VALUE_DEFAULT"].ToString();
            combConvert.SelectedIndex = dataConvert("combConvert", dsTemp.Tables[0].Rows[0]["CONVERT_TYPE"].ToString());
            combNecessary.SelectedIndex = dataConvert("combNecessary", dsTemp.Tables[0].Rows[0]["NECESSARY"].ToString());
            combPrint.SelectedIndex = dataConvert("combPrint", dsTemp.Tables[0].Rows[0]["PRINT"].ToString());
            editColumn.Text = dsTemp.Tables[0].Rows[0]["COLUMN_ITEM"].ToString();
            editRow.Text = dsTemp.Tables[0].Rows[0]["ROW_ITEM"].ToString();
            enableData();

            foreach (var item in dicUnit)
            {
                if (item.Key.ToString() == dsTemp.Tables[0].Rows[0]["UNIT_ID"].ToString())
                {
                    combUnit.SelectedItem = item.Value.ToString();
                    break;
                }
            }
        }

        private int dataConvert(string type, string data)
        {
            int iResult = 0;
            switch (type)
            {
                case "combPhase":     // 項目階段 A: 全部  I: WIP投入 O: WIP 產出
                    switch (data)
                    {
                        case "A":
                            iResult = 0;
                            break;
                        case "I":
                            iResult = 1;
                            break;
                        case "O":
                            iResult = 2;
                            break;
                        default:
                            iResult = 1;
                            break;
                    }
                    break;
                case "combValueType":
                    // 數值類型    V:文字  N:數字
                    switch (data)
                    {
                        case "V":
                            iResult = 0;
                            break;
                        case "N":
                            iResult = 1;
                            break;
                        default:
                            iResult = 0;
                            break;
                    }
                    break;
                case "combInputType":
                    // 輸入方式    K: KeyIn     S: Select List     R: Range (項目值為數字)
                    switch (data)
                    {
                        case "K":
                            iResult = 0;
                            break;
                        case "S":
                            iResult = 1;
                            break;
                        case "R":
                            iResult = 2;
                            break;
                        default:
                            iResult = 0;
                            break;
                    }
                    break;
                case "combConvert":
                    // 輸入值轉換       N: None     U: Uppercase   L: Lowercase
                    switch (data)
                    {
                        case "N":
                            iResult = 0;
                            break;
                        case "U":
                            iResult = 1;
                            break;
                        case "L":
                            iResult = 2;
                            break;
                        default:
                            iResult = 0;
                            break;
                    }
                    break;
                case "combNecessary":
                    //項目是否為必要輸入欄位     Y:必要    N:非必要
                    switch (data)
                    {
                        case "Y":
                            iResult = 0;
                            break;
                        case "N":
                            iResult = 1;
                            break;
                        default:
                            iResult = 0;
                            break;
                    }
                    break;
                case "combPrint":
                    //項目是否為流程卡打印     Y:是    N:否
                    switch (data)
                    {
                        case "N":
                            iResult = 0;
                            break;
                        case "Y":
                            iResult = 1;
                            break;
                        default:
                            iResult = 0;
                            break;
                    }
                    break;
            }
            return iResult;
        }

        private void fCollection_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);

            Initial_form();

            var d = new DataSet();

            try
            {
                // editName 下拉選單列出使用過的項目名稱
                string s = $@"
SELECT
    TRIM(item_name) item_name,
    COUNT(item_name)
FROM
    sajet.sys_part_qc_item
WHERE
    item_type = :item_type
GROUP BY
    item_name,
    TRIM(item_name)
ORDER BY
    2 DESC,
    1 ASC
";
                var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "item_type", g_sItemType },
                };

                d = ClientUtils.ExecuteSQL(s, p.ToArray());

                editName.Items.Clear();
                editName.Items.Add("");

                if (d != null && d.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in d.Tables[0].Rows)
                    {
                        editName.Items.Add(row["ITEM_NAME"].ToString());
                    }
                }

                editName.TextChanged += EditName_TextChanged;

                // 增加單位設定
                s = @"
SELECT
    unit_id,
    unit_no,
    unit_desc
FROM
    sajet.sys_unit
WHERE
    enabled = 'Y'
ORDER BY
    unit_id
";
                d = ClientUtils.ExecuteSQL(s);

                combUnit.Items.Add("");

                if (d != null)
                {
                    for (int i = 0; i < d.Tables[0].Rows.Count; i++)
                    {
                        combUnit.Items.Add(d.Tables[0].Rows[i]["UNIT_NO"].ToString());
                        dicUnit.Add(d.Tables[0].Rows[i]["UNIT_ID"].ToString(), d.Tables[0].Rows[i]["UNIT_NO"].ToString());
                    }
                }

                if (g_sType == "Add")
                {
                    s = @"
SELECT
    decode(MAX(item_seq), '', 0, MAX(item_seq)) + 1 seq
FROM
    sajet.sys_part_qc_item
WHERE
    part_id = :part_id
    AND version = :version
    AND route_id = :route_id
    AND node_id = :node_id
    AND item_type = :item_type
";
                    p = new List<object[]>
                    {
                        new object[] { ParameterDirection.Input, OracleType.Number, "part_id", PartDetail.PartID },
                        new object[] { ParameterDirection.Input, OracleType.VarChar, "version", string.IsNullOrWhiteSpace(PartDetail.Version) ? "N/A" : PartDetail.Version },
                        new object[] { ParameterDirection.Input, OracleType.Number, "route_id", CurrentNodeDetail.RouteID },
                        new object[] { ParameterDirection.Input, OracleType.Number, "node_id", CurrentNodeDetail.NodeID },
                        new object[] { ParameterDirection.Input, OracleType.VarChar, "item_type", g_sItemType },
                    };

                    d = ClientUtils.ExecuteSQL(s, p.ToArray());

                    nudSeq.Value = Convert.ToDecimal(d.Tables[0].Rows[0]["SEQ"]);
                    nudSeq.Enabled = false;
                }
                else  // Modify
                {
                    editName.Enabled = false;

                    s = $@"
SELECT
    item_seq,
    item_name,
    item_phase,
    value_type,
    input_type,
    value_default,
    convert_type,
    necessary,
    item_id,
    value_list,
    print,
    column_item,
    row_item,
    unit_id
FROM
    sajet.sys_part_qc_item
WHERE
    part_id = :part_id
    AND version = :version
    AND route_id = :route_id
    AND node_id = :node_id
    AND item_seq = :item_seq
    AND item_name = :item_name
    AND item_type = :item_type
    AND ROWNUM = 1
";
                    p = new List<object[]>
                    {
                        new object[] { ParameterDirection.Input, OracleType.Number, "part_id", PartDetail.PartID },
                        new object[] { ParameterDirection.Input, OracleType.VarChar, "version", string.IsNullOrWhiteSpace(PartDetail.Version) ? "N/A" : PartDetail.Version },
                        new object[] { ParameterDirection.Input, OracleType.Number, "route_id", CurrentNodeDetail.RouteID },
                        new object[] { ParameterDirection.Input, OracleType.Number, "node_id", CurrentNodeDetail.NodeID },
                        new object[] { ParameterDirection.Input, OracleType.Number, "item_seq", g_sSeq },
                        new object[] { ParameterDirection.Input, OracleType.Number, "item_name", g_sName },
                        new object[] { ParameterDirection.Input, OracleType.Number, "item_type", g_sItemType },
                    };
                    d = ClientUtils.ExecuteSQL(s, p.ToArray());

                    if (d.Tables[0].Rows.Count > 0)
                    {
                        g_sItemId = d.Tables[0].Rows[0]["ITEM_ID"].ToString();
                        showData(d);

                        if (d.Tables[0].Rows[0]["INPUT_TYPE"].ToString() == "S")
                        {
                            string[] str = d.Tables[0].Rows[0]["VALUE_LIST"].ToString().Split(',');
                            for (int i = 0; i < str.Length - 1; i++)
                            {
                                lbxItem.Items.Add(str[i]);
                            }
                        }
                        else if (d.Tables[0].Rows[0]["INPUT_TYPE"].ToString() == "R")
                        {
                            string[] str = d.Tables[0].Rows[0]["VALUE_LIST"].ToString().Split(',');
                            editMin.Text = str[0].ToString();
                            editMax.Text = str[1].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Error."), 0);
                return;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            fAddItem f = new fAddItem
            {
                sInputType = combValueType.SelectedIndex.ToString()
            };
            if (f.ShowDialog() == DialogResult.Yes)
                lbxItem.Items.Add(f.sItem);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lbxItem.SelectedIndices.Count == 0)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Select a Item."), 0);
                return;
            }
            lbxItem.Items.RemoveAt(lbxItem.SelectedIndex);
        }

        private void EditName_TextChanged(object sender, EventArgs e)
        {
            // editName 下拉選單列出使用過的項目名稱
            sSQL = @"
SELECT
    TRIM(value_default) ""VALUE_DEFAULT"",
    COUNT(value_default) ""COUNT""
FROM
    sajet.sys_part_qc_item
WHERE
    1 = 1
    AND item_type = :item_type
    AND item_name = :item_name
GROUP BY
    TRIM(value_default)
ORDER BY
    2 DESC,
    1 ASC
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE", g_sItemType },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_NAME", editName.Text }
            };

            var d = ClientUtils.ExecuteSQL(sSQL, p.ToArray());

            editDefault.Items.Clear();
            editDefault.Items.Add("");

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in d.Tables[0].Rows)
                {
                    editDefault.Items.Add(row["VALUE_DEFAULT"].ToString());
                }
            }
        }

        private void enableData()
        {
            //editDefault.Text = string.Empty;
            editMin.Text = string.Empty;
            editMax.Text = string.Empty;
            lbxItem.Items.Clear();

            switch (combInputType.SelectedIndex)
            {
                case 0:
                    editDefault.Enabled = true;
                    btnAdd.Enabled = false;
                    btnDelete.Enabled = false;
                    editMin.Enabled = false;
                    editMax.Enabled = false;

                    //editDefault.Visible = true;
                    //btnAdd.Visible = false;
                    //btnDelete.Visible = false;
                    //editMin.Visible = false;
                    //editMax.Visible = false;
                    break;
                case 1:
                    editDefault.Enabled = false;
                    btnAdd.Enabled = true;
                    btnDelete.Enabled = true;
                    editMin.Enabled = false;
                    editMax.Enabled = false;
                    break;
                case 2:
                    editDefault.Enabled = true;
                    btnAdd.Enabled = false;
                    btnDelete.Enabled = false;
                    editMin.Enabled = true;
                    editMax.Enabled = true;
                    break;
            }
        }

        private void combInputType_SelectedIndexChanged(object sender, EventArgs e)
        {
            enableData();
        }

        private void combPrint_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combPrint.SelectedIndex == 0)   // N
            {
                lblColumn.Visible = false;
                editColumn.Visible = false;

                lblRow.Visible = false;
                editRow.Visible = false;

                // 2.0.0.13
                editColumn.Text = "";
                editRow.Text = "";
            }
            else // Y
            {
                lblColumn.Visible = true;
                editColumn.Visible = true;
                lblRow.Visible = true;
                editRow.Visible = true;

                // 2.0.0.13
                editColumn.Text = "1";
                editRow.Text = "1";
            }

        }

    }
}