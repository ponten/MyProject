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
using OtSrv = RCProcessParam.Services.OtherService;

namespace RCProcessParam
{
    public partial class fCollection : Form
    {
        public fCollection()
        {
            InitializeComponent();
        }

        public string g_sProcess, g_sPart, g_sType, g_sPartId, g_sProcessId, g_sItemType, g_sUserID;
        public string g_sPhase, g_sValue, g_sInput, g_sConvert, g_sNecessary, g_sItemId, g_sPrint;
        public string g_sSeq, g_sName, g_sValueList;
        string sSQL;

        Dictionary<string, string> dicUnit = new Dictionary<string, string>(); // 記錄單位資料 unit_id & unit_no

        private void fCollection_Load(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            SajetCommon.SetLanguageControl(this);

            Initial_form();

            var d = new DataSet();

            try
            {
                // editName 下拉選單列出使用過的項目名稱
                sSQL = $@"
SELECT TRIM(ITEM_NAME) ITEM_NAME
      ,COUNT(ITEM_NAME)
FROM SAJET.SYS_RC_PROCESS_PARAM_PART
WHERE ITEM_TYPE = {g_sItemType}
GROUP BY ITEM_NAME
ORDER BY 2 DESC
        ,1 ASC
";
                d = ClientUtils.ExecuteSQL(sSQL);

                editName.Items.Clear();
                editName.Items.Add("");

                if (d != null && d.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in d.Tables[0].Rows)
                    {
                        editName.Items.Add(row["ITEM_NAME"].ToString());
                    }
                }

                editName.DropDownWidth = OtSrv.CalculateDropDownWidth(editName);

                editName.TextChanged += EditName_TextChanged;

                // 增加單位設定
                sSQL = " SELECT * FROM SAJET.SYS_UNIT WHERE ENABLED = 'Y' ORDER BY  UNIT_ID ";
                d = ClientUtils.ExecuteSQL(sSQL);

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
                    sSQL = @"SELECT DECODE(MAX(ITEM_SEQ), '', 0,MAX(ITEM_SEQ)) +1  SEQ
                                    FROM SAJET.SYS_RC_PROCESS_PARAM_PART  
                                    WHERE  PART_ID ='" + g_sPartId + "' "
                          + " AND PROCESS_ID = '" + g_sProcessId + "' "
                          + " AND ITEM_TYPE = '" + g_sItemType + "' ";
                    d = ClientUtils.ExecuteSQL(sSQL);

                    nudSeq.Value = Convert.ToDecimal(d.Tables[0].Rows[0]["SEQ"]);
                    nudSeq.Enabled = false;
                }
                else  // Modify
                {
                    editName.Enabled = false;

                    sSQL = $@"
SELECT ITEM_SEQ, ITEM_NAME, ITEM_PHASE, VALUE_TYPE, INPUT_TYPE, VALUE_DEFAULT,
        CONVERT_TYPE, NECESSARY, ITEM_ID, VALUE_LIST, PRINT, COLUMN_ITEM, ROW_ITEM, UNIT_ID
FROM SAJET.SYS_RC_PROCESS_PARAM_PART
WHERE PART_ID ='{g_sPartId}'
AND PROCESS_ID = '{g_sProcessId}'
AND ITEM_SEQ = '{g_sSeq}'
AND ITEM_NAME = '{g_sName}'
AND ITEM_TYPE = '{g_sItemType}'
AND ROWNUM = 1 ";
                    d = ClientUtils.ExecuteSQL(sSQL);

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
                string message
                    = SajetCommon.SetLanguage("Error.")
                    + Environment.NewLine
                    + ex.Message
                    ;

                SajetCommon.Show_Message(message, 0);
                return;
            }

            Cursor = Cursors.Default;
        }

        private void fCollection_Shown(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                //victor add
                //選擇輸入檢查格式為限制12碼英文跟數字
                if (combInputType.SelectedIndex == 3)
                {
                    if (!checkMachineLenght()) return;
                }

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

                if (g_sType == "Add")
                {
                    string ItemId = SajetCommon.GetMaxID("SAJET.SYS_RC_PROCESS_PARAM_PART", "ITEM_ID", 8);
                    sSQL = " INSERT INTO SAJET.SYS_RC_PROCESS_PARAM_PART "
                              + " (PART_ID, PROCESS_ID, ITEM_ID, ITEM_NAME, ITEM_PHASE, ITEM_TYPE,  "
                              + "  ITEM_SEQ, VALUE_TYPE, INPUT_TYPE, VALUE_DEFAULT, VALUE_LIST, "
                              + "  NECESSARY, CONVERT_TYPE, UPDATE_USERID, ENABLED, PRINT, "
                              + "  COLUMN_ITEM, ROW_ITEM, UNIT_ID ) VALUES ( "
                              + g_sPartId + "," + g_sProcessId + "," + ItemId + ",'" + editName.Text + "','" + g_sPhase + "','" + g_sItemType + "','"
                              + nudSeq.Value + "','" + g_sValue + "','" + g_sInput + "','" + editDefault.Text + "','" + g_sValueList + "','"
                              + g_sNecessary + "','" + g_sConvert + "'," + g_sUserID + "," + "'Y'" + ",'" + g_sPrint + "','"
                              + editColumn.Text.Trim() + "','" + editRow.Text.Trim() + "','" + sUnitId + "')";
                    ClientUtils.ExecuteSQL(sSQL);

                    if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Add another one"), 2) == DialogResult.Yes)
                        fCollection_Load(sender, e);
                    else
                        DialogResult = DialogResult.OK;
                }
                else // Modify
                {
                    sSQL = " UPDATE SAJET.SYS_RC_PROCESS_PARAM_PART SET "
                              + " ITEM_SEQ ='" + nudSeq.Value.ToString() + "', "
                              + " ITEM_NAME = '" + editName.Text.Trim() + "', "
                              + " ITEM_PHASE = '" + g_sPhase + "', "
                              + " VALUE_TYPE = '" + g_sValue + "', "
                              + " INPUT_TYPE = '" + g_sInput + "', "
                              + " VALUE_DEFAULT = '" + editDefault.Text + "', "
                              + " CONVERT_TYPE = '" + g_sConvert + "', "
                              + " NECESSARY = '" + g_sNecessary + "', "
                              + " VALUE_LIST = '" + g_sValueList + "', "
                              + " PRINT = '" + g_sPrint + "', "
                              + " COLUMN_ITEM = '" + editColumn.Text.Trim() + "', "
                              + " ROW_ITEM = '" + editRow.Text.Trim() + "', "
                              + " UNIT_ID = '" + sUnitId + "' "
                              + " WHERE  "
                              + " PART_ID = '" + g_sPartId + "' "
                              + " AND PROCESS_ID = '" + g_sProcessId + "' "
                              + " AND ITEM_SEQ = '" + g_sSeq + "' "
                              + " AND ITEM_NAME = '" + g_sName + "' "
                              + " AND ITEM_ID = '" + g_sItemId + "' ";
                    ClientUtils.ExecuteSQL(sSQL);

                    DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                string message
                    = SajetCommon.SetLanguage("Add Error.")
                    + Environment.NewLine
                    + ex.Message
                    ;

                SajetCommon.Show_Message(message, 0);
                return;
            }
        }

        private void EditName_TextChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            // editName 下拉選單列出使用過的項目名稱
            sSQL = @"
SELECT DISTINCT
    TRIM(VALUE_DEFAULT) ""VALUE_DEFAULT""
FROM
    SAJET.SYS_RC_PROCESS_PARAM_PART
WHERE 1 = 1
AND ITEM_TYPE = :ITEM_TYPE
AND ITEM_NAME = :ITEM_NAME
ORDER BY
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

            editDefault.DropDownWidth = OtSrv.CalculateDropDownWidth(editDefault);

            Cursor = Cursors.Default;
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
                    //if (string.IsNullOrEmpty(editDefault.Text))
                    //{
                    //    SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Input Default Value."), 0);
                    //    return false;
                    //}
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
                sSQL = " SELECT ITEM_SEQ FROM SAJET.SYS_RC_PROCESS_PARAM_PART  "
                          + " WHERE PART_ID = '" + g_sPartId + "' "
                          + " AND PROCESS_ID = '" + g_sProcessId + "' "
                          + " AND ITEM_TYPE = '" + g_sItemType + "' "
                          + " AND ITEM_SEQ = '" + nudSeq.Value.ToString() + "' ";
                if (g_sType == "Modify")
                    sSQL = sSQL + " AND ITEM_NAME <> '" + editName.Text + "' ";
                var d = ClientUtils.ExecuteSQL(sSQL);

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
                    sSQL = " SELECT * FROM SAJET.SYS_RC_PROCESS_PARAM_PART "
                              + " WHERE PART_ID = '" + g_sPartId + "' "
                              + " AND PROCESS_ID = '" + g_sProcessId + "' "
                              + " AND ITEM_TYPE = '" + g_sItemType + "' "
                              + " AND ITEM_NAME = '" + editName.Text.Trim() + "' ";
                    if (g_sType == "Modify")
                        sSQL = sSQL + " AND ITEM_SEQ <> '" + nudSeq.Value.ToString() + "' ";
                    d = ClientUtils.ExecuteSQL(sSQL);

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
            catch (Exception ex)
            {
                string message
                    = SajetCommon.SetLanguage("Check Data Error.")
                    + Environment.NewLine
                    + ex.Message
                    ;

                SajetCommon.Show_Message(message, 0);
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

        /// <summary>
        /// 檢查輸入長度跟格式
        /// </summary>
        /// <returns></returns>
        private bool checkMachineLenght()
        {
            bool bRes = true;

            if (editName.Text.Contains("爐號") || (editName.SelectedItem != null && editName.SelectedItem.ToString().Contains("爐號")))
            {
                if (editDefault.Text.Length != 12)
                {
                    SajetCommon.Show_Message("限制只輸入12碼", 0);                   
                    bRes=false;
                }
                if (editDefault.Text.Length == 12)
                {
                    if (!IsNatural_Number(editDefault.Text))
                    {
                        SajetCommon.Show_Message("限制只輸入數字跟英文", 0);
                        bRes = false;
                    }
                }
            }
            return bRes;
        }

        /// <summary>
        /// 限制輸入英文跟數字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool IsNatural_Number(string str)
        {
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[A-Za-z0-9]+$");
            return reg1.IsMatch(str);
        }

    }
}