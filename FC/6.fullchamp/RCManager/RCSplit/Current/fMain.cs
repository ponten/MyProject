using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using SajetClass;
using SajetFilter;
using System.Linq;

namespace RCSplit
{
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();
        }

        // 2016.7.16 By Jason
        public decimal dInitialQty = 0;
        public decimal dInitialQtyNew = 0;
        // 2016.7.16 End

        public String g_sUserID, g_sProgram, g_sFunction;
        string sSQL;
        DataSet dsTemp, dsRcData;
        public Dictionary<int, List<string>> dictComponent = new Dictionary<int, List<string>>();
        public string sRC_CallByOthers = "";

        private void fMain_Load(object sender, EventArgs e)
        {
            g_sUserID = ClientUtils.UserPara1;
            g_sProgram = ClientUtils.fProgramName;
            g_sFunction = ClientUtils.fFunctionName;
            SajetCommon.SetLanguageControl(this);

            if (!string.IsNullOrEmpty(sRC_CallByOthers)  )
            {
                editRC.Text = sRC_CallByOthers;
                editRC.Enabled = false ;
                btnSearchWo.Enabled = false;
                editRC_KeyPress(null, new KeyPressEventArgs((char)Keys.Return));
            }
        }

        public void btnSearchWo_Click(object sender, EventArgs e)
        {
            // 2016.11.24 By Jason
            string sSQL = "  SELECT A.WORK_ORDER,A.RC_NO,A.CURRENT_QTY"
                        + "    FROM SAJET.G_RC_STATUS A,SAJET.G_WO_BASE B"
                        + "   WHERE A.WORK_ORDER = B.WORK_ORDER"
                        + "     AND A.CURRENT_STATUS >= 0 AND A.CURRENT_STATUS < 2"
                        + "     AND A.CURRENT_QTY > 0"
                        + "     AND A.RELEASE = 'Y'"
                        + "     AND B.WO_STATUS = '3'"
                        + "ORDER BY A.WORK_ORDER ASC,A.RC_NO ASC";
            // 2016.11.24 End

            var f = new fFilter();
            f.sSQL = sSQL;

            if (f.ShowDialog() == DialogResult.OK)
            {
                editRC.Text = f.dgvData.CurrentRow.Cells["RC_NO"].Value.ToString();
                KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);
                editRC_KeyPress(sender, Key);
            }
        }

        public void editRC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 3)
                ClearData();

            if (e.KeyChar != (char)Keys.Return)
                return;

            sSQL = " SELECT * "
                 + "   FROM SAJET.G_RC_STATUS "
                 + "  WHERE RC_NO = '" + editRC.Text + "' ";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                SajetCommon.Show_Message("RC number error", 0);
                editRC.Focus();
                editRC.SelectAll();
                return;
            }

            sSQL = " SELECT * "
                 + "   FROM SAJET.G_RC_STATUS "
                 + "  WHERE RC_NO = '" + editRC.Text + "' "
                 + "    AND RELEASE = 'Y'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                SajetCommon.Show_Message("RC number not release", 0);
                editRC.Focus();
                editRC.SelectAll();
                return;
            }

            sSQL = " SELECT A.*,B.* "
                 + "   FROM SAJET.G_RC_STATUS A,SAJET.G_WO_BASE B"
                 + "  WHERE A.WORK_ORDER = B.WORK_ORDER "
                 + "    AND A.RC_NO = '" + editRC.Text + "' "
                 + "    AND A.RELEASE = 'Y'"
                 + "    AND B.WO_STATUS = '3'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                SajetCommon.Show_Message("Work Order Status must be WIP", 0);
                editRC.Focus();
                editRC.SelectAll();
                return;
            }

            ShowData();
        }

        private void ShowData()
        {
            string sSQL = "    SELECT A.*,"
                        + "           B.PART_ID,B.PART_NO,C.PROCESS_ID,C.PROCESS_NAME,B.SPEC1,B.SPEC2"
                        + "      FROM SAJET.G_RC_STATUS A"
                        + " LEFT JOIN SAJET.SYS_PART B ON A.PART_ID = B.PART_ID"
                        + " LEFT JOIN SAJET.SYS_PROCESS C ON A.PROCESS_ID = C.PROCESS_ID"
                        + " LEFT JOIN SAJET.G_WO_BASE D ON A.WORK_ORDER = D.WORK_ORDER"
                        + "     WHERE A.RC_NO = '" + editRC.Text + "'"
                        + "       AND A.CURRENT_STATUS >= 0 AND A.CURRENT_STATUS < 2"
                        + "       AND A.CURRENT_QTY > 0"
                        + "       AND A.RELEASE = 'Y'"
                        + "       AND D.WO_STATUS = '3'";
            dsRcData = ClientUtils.ExecuteSQL(sSQL);

            // 2016.4.19 By Jason (防呆)
            if (dsRcData.Tables[0].Rows.Count == 0)
            {
                SajetCommon.Show_Message("RC Current Status (Qty) Close", 0);
                editRC.Focus();
                editRC.SelectAll();
                return;
            }
            // 2016.4.19 End

            LabWO.Text = dsRcData.Tables[0].Rows[0]["WORK_ORDER"].ToString();
            LabPart.Text = dsRcData.Tables[0].Rows[0]["SPEC1"].ToString();
            LabSpec2.Text = dsRcData.Tables[0].Rows[0]["SPEC2"].ToString();
            LabVersion.Text = dsRcData.Tables[0].Rows[0]["VERSION"].ToString();
            LabProcess.Text = dsRcData.Tables[0].Rows[0]["PROCESS_NAME"].ToString();
            LabQty.Text = dsRcData.Tables[0].Rows[0]["CURRENT_QTY"].ToString();
            dInitialQty = Convert.ToDecimal(dsRcData.Tables[0].Rows[0]["INITIAL_QTY"].ToString());

            btnSplit.Enabled = true;

            editQty.Focus();
            editQty.SelectAll();
        }

        private void ClearData()
        {
            for (int i = 0; i <= panel1.Controls.Count - 1; i++)
            {
                if (panel1.Controls[i] is Label && panel1.Controls[i].Name.Contains("Lab"))
                {
                    panel1.Controls[i].Text = "";
                }
            }

            editQty.Text = "";
            // 2016.4.27 By Jason
            //rbPartQty.Checked = true;
            rbQtyEach.Checked = true;
            // 2016.4.27 End
            btnSplit.Enabled = false;
            ClearGvComponent();
        }

        private void ClearGvComponent()
        {
            btnPick.Enabled = false;
            btnSave.Enabled = false;
            gvData.DataSource = null;
            gvData.Columns.Clear();
            gvDetail.DataSource = null;
            gvDetail.Columns.Clear();
        }

        private void editQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;

            if (LabWO.Text == "")
                return;

            dictComponent.Clear();

            // 2016.4.27 By Jason
            //int iLeftQty, iPartQty;
            // 2016.4.27 End

            int iQty = 0;
            decimal iCurrentQty = 0;

            decimal.TryParse(LabQty.Text, out iCurrentQty);

            // 2016.4.28 By Jason
            //if (iCurrentQty < 2)
            //{
            //    // 2016.4.27 By Jason
            //    ClearGvComponent();
            //    // 2016.4.27 End

            //    SajetCommon.Show_Message("Current qty is not enough to split", 0);
            //    return;
            //}
            // 2016.4.28 End

            // 2016.4.27 By Jason
            //if (!Int32.TryParse(editQty.Text, out iQty) || iQty <= 0)
            //{
            //    // 2016.4.27 By Jason
            //    ClearGvComponent();
            //    // 2016.4.27 End

            //    SajetCommon.Show_Message("Qty error", 0);
            //    editQty.Focus();
            //    editQty.SelectAll();
            //    return;
            //}

            decimal d_inputQty = 0;

            if (!Int32.TryParse(editQty.Text, out iQty) || iQty <= 0)
            {
                if (!decimal.TryParse(editQty.Text, out d_inputQty) || d_inputQty <= 0 || !string.IsNullOrEmpty(sRC_CallByOthers))
                {
                    ClearGvComponent();

                    SajetCommon.Show_Message("Qty error", 0);
                    editQty.Focus();
                    editQty.SelectAll();
                    return;
                }
                else
                {
                    string s_inputQty = d_inputQty.ToString().Trim();

                    if (s_inputQty.Length > s_inputQty.IndexOf(".") + 3)
                    {
                        ClearGvComponent();

                        SajetCommon.Show_Message("Qty error", 0);
                        editQty.Focus();
                        editQty.SelectAll();
                        return;
                    }
                }
            }
            // 2016.4.27 End

            if (iCurrentQty - iQty < 0 || iCurrentQty - d_inputQty < 0)
            {
                // 2016.4.27 By Jason
                ClearGvComponent();
                // 2016.4.27 End

                SajetCommon.Show_Message("Qty error", 0);
                editQty.Focus();
                editQty.SelectAll();
                return;
            }

            // 2016.4.27 By Jason
            //iLeftQty = iCurrentQty % iQty;
            //iPartQty = iCurrentQty / iQty;
            // 2016.4.27 End

            List<childRC> childRCs = new List<childRC>();

            if (rbPartQty.Checked)
            {
                // 2016.4.27 By Jason

                //if (iQty == 1)
                //{
                //    SajetCommon.Show_Message("Part Qty must larger than 1", 0);
                //    editQty.Focus();
                //    editQty.SelectAll();
                //    return;
                //}

                //ClearGvComponent();

                //for (int i = 0; i < iQty - 1; i++)
                //{
                //    childRCs.Add(new childRC() { CHILD_RC_NO = "", Qty = iPartQty.ToString() });
                //}

                //childRCs.Add(new childRC() { CHILD_RC_NO = "", Qty = (iPartQty + iLeftQty).ToString() });

                // 2016.4.27 End
            }
            else
            {
                if (iQty == iCurrentQty || d_inputQty == iCurrentQty)
                {
                    // 2016.4.27 By Jason
                    ClearGvComponent();
                    // 2016.4.27 End

                    SajetCommon.Show_Message("Part qty Error", 0);
                    editQty.Focus();
                    editQty.SelectAll();
                    return;
                }

                ClearGvComponent();

                // 2016.4.27 By Jason
                //for (int i = 0; i < iPartQty; i++)
                //{
                //    childRCs.Add(new childRC() { CHILD_RC_NO = "", Qty = iQty.ToString() });
                //}

                //if (iLeftQty > 0)
                //    childRCs.Add(new childRC() { CHILD_RC_NO = "", Qty = iLeftQty.ToString() });

                string s_RC_End = "";
                /*
                sSQL = @"
SELECT
    MAX(SUBSTR(RC_NO, - 1, 1) + 1) AS RC_END
FROM
    SAJET.G_RC_STATUS
WHERE
    RC_NO LIKE :RC_NO || '%'
";//*/

                if (string.IsNullOrEmpty(sRC_CallByOthers))
                {
                    //直接開起 此程式 而不是被調用
                    sSQL = @"
SELECT
    LPAD(MAX(SUBSTR(RC_NO, - 3, 3) + 1), 3, 0) AS RC_END
FROM
    SAJET.G_RC_STATUS
WHERE
    WORK_ORDER = (
        SELECT
            WORK_ORDER
        FROM
            SAJET.G_RC_STATUS
        WHERE
            RC_NO = :RC_NO
    )
";
                    var Params = new object[1][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", editRC.Text.Trim().ToUpper() };
                    dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

                    if (dsTemp.Tables[0].Rows.Count > 0)
                    {
                        //if (Convert.ToInt32(dsTemp.Tables[0].Rows[0]["RC_END"].ToString()) > 9)
                        if (Convert.ToInt32(dsTemp.Tables[0].Rows[0]["RC_END"].ToString()) > 999)
                        {
                            SajetCommon.Show_Message("RC lotsize is full", 0);
                            editQty.Focus();
                            editQty.SelectAll();
                            return;
                        }

                        //s_RC_End = editRC.Text.Substring(0, editRC.Text.Length - 2) + dsTemp.Tables[0].Rows[0]["RC_END"].ToString();
                        s_RC_End = editRC.Text.Split('-')[0] + "-" + dsTemp.Tables[0].Rows[0]["RC_END"].ToString();
                    }
                }
                else
                {
                    // 被 RCOutout調用
                    sSQL = @"
                    SELECT rs.rc_no FROM sajet.g_rc_split rs  
                    WHERE  rs.source_rc_no = :RC_NO
                    AND    rs.rc_no LIKE rs.source_rc_no || '-%' 
                    AND    sajet.isnumber(SUBSTR((rs.rc_no),　INSTR((rs.rc_no),'-', -1)+1))='TRUE'
                    ORDER  BY LENGTH(rs.rc_no)DESC, rs.RC_NO DESC";
                    var Params = new object[1][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", editRC.Text.Trim().ToUpper() };
                    dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);
                    if (dsTemp.Tables[0].Rows.Count > 0)
                    {
                        string sMaxRc = dsTemp.Tables[0].Rows[0]["RC_NO"].ToString();                         
                        string sLastString = sMaxRc.Substring(sMaxRc.LastIndexOf('-') + 1);
                        int iSeq = Convert.ToInt32(sLastString);

                        if (++iSeq > 999)
                        {
                            SajetCommon.Show_Message("RC lotsize is full", 0);
                            editQty.Focus();
                            editQty.SelectAll();
                            return;
                        }
                        s_RC_End = sMaxRc.Substring(0, sMaxRc.LastIndexOf('-')) + "-" + iSeq.ToString().PadLeft(3, '0');

                    }
                    else
                        s_RC_End = editRC.Text.Trim().ToUpper() + "-001";
                }

                if (iQty == 0)
                {
                    childRCs.Add(new childRC() { CHILD_RC_NO = s_RC_End, Qty = d_inputQty.ToString() });
                    childRCs.Add(new childRC() { CHILD_RC_NO = editRC.Text, Qty = (iCurrentQty - d_inputQty).ToString() });

                    // 2016.7.16 By Jason
                    if (iCurrentQty == dInitialQty)
                    {
                        dInitialQtyNew = d_inputQty;
                    }
                    else
                    {
                        dInitialQtyNew = Convert.ToInt32((dInitialQty * d_inputQty) / iCurrentQty);
                    }
                    // 2016.7.16 End
                }
                else
                {
                    childRCs.Add(new childRC() { CHILD_RC_NO = s_RC_End, Qty = iQty.ToString() });
                    childRCs.Add(new childRC() { CHILD_RC_NO = editRC.Text, Qty = (iCurrentQty - iQty).ToString() });

                    // 2016.7.16 By Jason
                    if (iCurrentQty == dInitialQty)
                    {
                        dInitialQtyNew = iQty;
                    }
                    else
                    {
                        dInitialQtyNew = Convert.ToInt32((dInitialQty * iQty) / iCurrentQty);
                    }
                    // 2016.7.16 End
                }
                // 2016.4.27 End
            }

            var bList = new BindingList<childRC>(childRCs);
            var bSource = new BindingSource(bList.OrderBy(c => c.CHILD_RC_NO), null);

            gvData.DataSource = bSource;
            gvData.Columns["CHILD_RC_NO"].HeaderText = SajetCommon.SetLanguage("Child Rc No", 1);
            gvData.Columns["Qty"].HeaderText = SajetCommon.SetLanguage("Qty", 1);
            gvData.Columns["CHILD_RC_NO"].Width = 200;

            // 2016.4.27 By Jason
            gvData.Columns["CHILD_RC_NO"].ReadOnly = true;
            gvData.Columns["Qty"].ReadOnly = true;
            // 2016.4.27 End

            btnSave.Enabled = true;

            if (dsRcData != null && dsRcData.Tables[0].Rows.Count > 0)
            {
                if (dsRcData.Tables[0].Rows[0]["HAVE_SN"].ToString() == "1")
                {
                    gvData.Columns["Qty"].ReadOnly = true;
                    btnPick.Enabled = true;
                }
            }

            gvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btnSplit_Click(object sender, EventArgs e)
        {
            KeyPressEventArgs Key = new KeyPressEventArgs((char)Keys.Return);
            editQty_KeyPress(sender, Key);
        }

        private class childRC
        {
            public string CHILD_RC_NO { get; set; }
            public string Qty { get; set; }
        }

        private void btnPick_Click(object sender, EventArgs e)
        {
            if (gvData.RowCount == 0 || gvData.CurrentRow == null)
                return;

            string sSQL = " Select SERIAL_NUMBER "
                        + " From SAJET.G_SN_STATUS where "
                        + " RC_NO = '" + editRC.Text + "' "
                        + " and CURRENT_STATUS >= 0 and CURRENT_STATUS < 2 "
                        + " Order by SERIAL_NUMBER";

            fComponent f = new fComponent(this);
            f.sSQL = sSQL;
            f.iRowIndex = gvData.CurrentRow.Index;
            f.iGvRowCount = gvData.RowCount;
            f.iQty = Int32.Parse(gvData.CurrentRow.Cells["Qty"].Value.ToString());
            DialogResult dResult = f.ShowDialog();

            if (dictComponent.ContainsKey(gvData.CurrentRow.Index))
                dictComponent[gvData.CurrentRow.Index] = f.listSerialNumbers;
            else
                dictComponent.Add(gvData.CurrentRow.Index, f.listSerialNumbers);
            if (dResult == DialogResult.OK)
                gvData.CurrentRow.Cells["Qty"].Value = f.listSerialNumbers.Count.ToString();

            gvData_SelectionChanged(sender, new EventArgs());
        }

        private void gvData_SelectionChanged(object sender, EventArgs e)
        {
            if (gvData.RowCount == 0 || gvData.CurrentRow == null)
                return;

            ShowDetail();
        }

        private void ShowDetail()
        {
            if (gvDetail.ColumnCount > 0)
                gvDetail.Columns.RemoveAt(0);

            if (dictComponent.ContainsKey(gvData.CurrentRow.Index) &&
                dictComponent[gvData.CurrentRow.Index].Count > 0)
            {
                DataGridViewTextBoxColumn tbCol = new DataGridViewTextBoxColumn();
                tbCol.Width = 200;
                tbCol.Name = "SERIAL_NUMBER";
                tbCol.HeaderText = SajetCommon.SetLanguage("Serial Number", 1);

                gvDetail.Columns.Insert(0, tbCol);

                foreach (string serialNumber in dictComponent[gvData.CurrentRow.Index])
                {
                    gvDetail.Rows.Add(serialNumber);
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!Check_Keyin()) return;

            try
            {
                SaveData();
                string sMsg = SajetCommon.SetLanguage("Data Append OK", 2) + " !";
                SajetCommon.Show_Message(sMsg, 1);

                if (!string.IsNullOrEmpty(sRC_CallByOthers))
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    ClearData();
                    editRC.Text = "";
                    editRC.Focus();
                }
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Exception : " + ex.Message, 0);
                return;
            }
        }

        private bool Check_Keyin()
        {
            string sMsg, sData;
            List<string> listRC = new List<string>();
            bool isDuplicate = false;
            decimal iTotalQty = 0;

            if (gvData.RowCount == 0)
            {
                sMsg = SajetCommon.SetLanguage("No Data", 1);
                SajetCommon.Show_Message(sMsg, 0);
                return false;
            }

            foreach (DataGridViewRow gvRow in gvData.Rows)
            {
                if (dsRcData.Tables[0].Rows[0]["HAVE_SN"].ToString() == "1")
                {
                    if (!dictComponent.ContainsKey(gvRow.Index) || dictComponent[gvRow.Index].Count == 0)
                    {
                        sMsg = SajetCommon.SetLanguage("Please pick component", 1);
                        SajetCommon.Show_Message(sMsg, 0);
                        return false;
                    }
                }

                foreach (DataGridViewCell gvCell in gvRow.Cells)
                {
                    if (gvCell.Value == null)
                    {
                        sMsg = SajetCommon.SetLanguage("Data is null", 2);
                        gvCell.ErrorText = sMsg;
                        SajetCommon.Show_Message(sMsg, 0);
                        return false;
                    }

                    gvCell.Value = gvCell.Value.ToString().Trim();

                    if (gvCell.OwningColumn.Name == "CHILD_RC_NO")
                    {
                        if (listRC.Contains(gvCell.Value.ToString()))
                        {
                            isDuplicate = true;
                            continue;
                        }
                        listRC.Add(gvCell.Value.ToString());
                    }
                }

                string sRcNo = gvRow.Cells["CHILD_RC_NO"].Value.ToString();
                string sQty = gvRow.Cells["Qty"].Value.ToString();

                if (String.IsNullOrEmpty(sRcNo))
                {
                    sData = gvData.Columns["CHILD_RC_NO"].HeaderText;
                    sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                    gvRow.Cells["CHILD_RC_NO"].ErrorText = sMsg;
                    SajetCommon.Show_Message(sMsg, 0);
                    return false;
                }

                if (String.IsNullOrEmpty(sQty))
                {
                    sData = gvData.Columns["Qty"].HeaderText;
                    sMsg = SajetCommon.SetLanguage("Data is null", 2) + Environment.NewLine + sData;
                    gvRow.Cells["Qty"].ErrorText = sMsg;
                    SajetCommon.Show_Message(sMsg, 0);
                    return false;
                }

                if (isDuplicate)
                {
                    sData = gvData.Columns["CHILD_RC_NO"].HeaderText + " : " + sRcNo;
                    sMsg = SajetCommon.SetLanguage("Data Duplicate", 2) + Environment.NewLine + sData;
                    gvRow.Cells["CHILD_RC_NO"].ErrorText = sMsg;
                    SajetCommon.Show_Message(sMsg, 0);
                    return false;
                }

                if (sRcNo != editRC.Text)
                {
                    sSQL = " Select RC_NO from SAJET.G_RC_STATUS where RC_NO = '"
                        + sRcNo + "'";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);

                    if (dsTemp.Tables[0].Rows.Count > 0)
                    {
                        sData = gvData.Columns["CHILD_RC_NO"].HeaderText + " : " + sRcNo;
                        sMsg = SajetCommon.SetLanguage("Data Duplicate", 2) + Environment.NewLine + sData;
                        gvRow.Cells["CHILD_RC_NO"].ErrorText = sMsg;
                        SajetCommon.Show_Message(sMsg, 0);
                        return false;
                    }
                }

                // 2016.4.27 By Jason
                //// 2016.4.20 By Jason (防呆)
                //if (sRcNo.Trim() == editRC.Text.Trim())
                //{
                //    sData = gvData.Columns["CHILD_RC_NO"].HeaderText + " : " + sRcNo;
                //    sMsg = SajetCommon.SetLanguage("Data Duplicate", 2) + Environment.NewLine + sData;
                //    gvRow.Cells["CHILD_RC_NO"].ErrorText = sMsg;
                //    SajetCommon.Show_Message(sMsg, 0);
                //    return false;
                //}
                //// 2016.4.20 End
                // 2016.4.27 End

                decimal iQty = 0;

                if (!decimal.TryParse(sQty, out iQty) || iQty <= 0)
                {
                    sMsg = SajetCommon.SetLanguage("Qty error", 1);
                    gvRow.Cells["Qty"].ErrorText = sMsg;
                    SajetCommon.Show_Message(sMsg, 0);
                    return false;
                }

                iTotalQty += iQty;

                gvRow.Cells["CHILD_RC_NO"].ErrorText = "";
                gvRow.Cells["Qty"].ErrorText = "";
            }

            if (iTotalQty != Convert.ToDecimal(LabQty.Text))
            //if (iTotalQty.ToString() != LabQty.Text)
            {
                sMsg = SajetCommon.SetLanguage("Total qty must equal to current qty", 1);
                SajetCommon.Show_Message(sMsg, 0);
                return false;
            }

            return true;
        }

        private void gvData_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            string sMsg;
            string sCellValue = e.FormattedValue.ToString().Trim();

            if (String.IsNullOrEmpty(sCellValue))
            {
                sMsg = SajetCommon.SetLanguage("Data is null", 2);
                gvData.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = sMsg;
                return;
            }

            if (gvData.Columns[e.ColumnIndex].Name == "CHILD_RC_NO")
            {
                if (sCellValue != editRC.Text)
                {
                    sSQL = " Select RC_NO from SAJET.G_RC_STATUS where RC_NO = '"
                        + sCellValue + "'";
                    dsTemp = ClientUtils.ExecuteSQL(sSQL);
                    if (dsTemp.Tables[0].Rows.Count > 0)
                    {
                        string sData = gvData.Columns[e.ColumnIndex].HeaderText + " : " + sCellValue;
                        sMsg = SajetCommon.SetLanguage("Data Duplicate", 2) + Environment.NewLine + sData;
                        gvData.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = sMsg;
                        return;
                    }
                }
            }

            if (gvData.Columns[e.ColumnIndex].Name == "Qty")
            {
                int iQty = 0;
                if (!Int32.TryParse(sCellValue, out iQty) || iQty <= 0)
                {
                    sMsg = SajetCommon.SetLanguage("Qty error", 1);
                    gvData.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = sMsg;
                    return;
                }
            }

            gvData.CurrentCell.ErrorText = "";
        }

        string sSourceTravelID;
        string sMachineId = "", sStartTime = "", sEndTime = "", sLoadPort = "";
        string sToolingSn = "", sToolingMachine = "", sToolingStart = "", sToolingEnd = "";
        string sItemPhase = "", sItemType = "", sItemID = "", sItemName = "", sItemValue = "", sValueType = "";

        private void SaveData()
        {
            DateTime datExeTime = ClientUtils.GetSysDate();

            // 2016.5.25 By Jason
            long lTravel_Id = Convert.ToInt64(datExeTime.ToString("yyyyMMddHHmmssf"));
            // 2016.5.25 End

            // 2016.7.5 By Jason
            int iWorkTime = Convert.ToInt32(dsRcData.Tables[0].Rows[0]["WORKTIME"].ToString());
            // 2016.7.5 End

            Boolean HaveSourceRC = false;
            sSourceTravelID = dsRcData.Tables[0].Rows[0]["TRAVEL_ID"].ToString();
            int iTravelID = 1;
            Int32.TryParse(dsRcData.Tables[0].Rows[0]["TRAVEL_ID"].ToString(), out iTravelID);
            iTravelID++;
            string f23 = dsRcData.Tables[0].Rows[0]["CURRENT_STATUS"].ToString();
            if (dsRcData.Tables[0].Rows[0]["CURRENT_STATUS"].ToString() == "1")
                ReadExtraTables();

            foreach (DataGridViewRow gvRow in gvData.Rows)
            {
                string sRcNo = gvRow.Cells["CHILD_RC_NO"].Value.ToString();
                string sQty = gvRow.Cells["Qty"].Value.ToString();
                string sProcessID = dsRcData.Tables[0].Rows[0]["PROCESS_ID"].ToString();

                object[][] Params = new object[8][];
                sSQL = " Insert into SAJET.G_RC_SPLIT "
                     + " (RC_NO, RC_QTY, SOURCE_RC_NO, SOURCE_RC_QTY, TRAVEL_ID , PROCESS_ID "
                     + ", UPDATE_USERID, UPDATE_TIME) "
                     + " Values "
                     + " (:RC_NO, :RC_QTY, :SOURCE_RC_NO, :SOURCE_RC_QTY, :TRAVEL_ID , :PROCESS_ID "
                     + ", :UPDATE_USERID, :UPDATE_TIME) ";
                Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", sRcNo };
                Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_QTY", sQty };
                Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SOURCE_RC_NO", editRC.Text };
                Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SOURCE_RC_QTY", LabQty.Text };

                // 2016.5.25 By Jason
                Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TRAVEL_ID", lTravel_Id };

                //// 2016.4.29 By Jason
                ////Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TRAVEL_ID", sSourceTravelID };
                //if (sRcNo != editRC.Text)
                //{
                //    Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TRAVEL_ID", 1 };
                //}
                //else
                //{
                //    Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TRAVEL_ID", sSourceTravelID };
                //}
                //// 2016.4.29 End
                // 2016.5.25 End

                Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", sProcessID };
                Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                Params[7] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime };
                ClientUtils.ExecuteSQL(sSQL, Params);

                //Nancy add 2015.07.18
                if (sRcNo == editRC.Text)
                {
                    HaveSourceRC = true;

                    object[][] rcParams = new object[8][];
                    sSQL = " UPDATE SAJET.G_RC_STATUS SET CURRENT_QTY = :CURRENT_QTY ,TRAVEL_ID = :TRAVEL_ID "
                            + ",UPDATE_USERID = :UPDATE_USERID,UPDATE_TIME = :UPDATE_TIME,BONUS_QTY = :BONUS_QTY,WORKTIME = :WORKTIME,INITIAL_QTY = :INITIAL_QTY,RELEASE = :RELEASE "
                            + " Where RC_NO = '" + editRC.Text + "' and TRAVEL_ID = '" + sSourceTravelID + "'";
                    rcParams[0] = new object[] { ParameterDirection.Input, OracleType.Number, "CURRENT_QTY", sQty };
                    // 2016.5.25 By Jason
                    //rcParams[1] = new object[] { ParameterDirection.Input, OracleType.Number, "TRAVEL_ID", iTravelID.ToString() };
                    rcParams[1] = new object[] { ParameterDirection.Input, OracleType.Number, "TRAVEL_ID", lTravel_Id };
                    // 2016.5.25 End
                    rcParams[2] = new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", g_sUserID };
                    rcParams[3] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime };
                    rcParams[4] = new object[] { ParameterDirection.Input, OracleType.Number, "BONUS_QTY", 0 };
                    rcParams[5] = new object[] { ParameterDirection.Input, OracleType.Number, "WORKTIME", iWorkTime };
                    rcParams[6] = new object[] { ParameterDirection.Input, OracleType.Number, "INITIAL_QTY", dInitialQty - dInitialQtyNew };
                    rcParams[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RELEASE", "Y" };
                    ClientUtils.ExecuteSQL(sSQL, rcParams);
                }
                else
                {
                    object[][] rcParams = new object[41][];
                    sSQL = " Insert into SAJET.G_RC_STATUS "
                         + " (WORK_ORDER, RC_NO, PART_ID, VERSION, ROUTE_ID , FACTORY_ID, PDLINE_ID "
                         + ", STAGE_ID, NODE_ID, PROCESS_ID,SHEET_NAME, TERMINAL_ID, TRAVEL_ID, NEXT_NODE, NEXT_PROCESS "
                         + ", CURRENT_STATUS, CURRENT_QTY, IN_PROCESS_EMPID, IN_PROCESS_TIME, WIP_IN_QTY "
                         + ", WIP_IN_EMPID, WIP_IN_MEMO, WIP_IN_TIME, WIP_OUT_GOOD_QTY, WIP_OUT_SCRAP_QTY "
                         + ", WIP_OUT_EMPID, WIP_OUT_MEMO, WIP_OUT_TIME, OUT_PROCESS_EMPID, OUT_PROCESS_TIME "
                         + ", HAVE_SN, PRIORITY_LEVEL, UPDATE_USERID, UPDATE_TIME, CREATE_TIME, BATCH_ID, EMP_ID, BONUS_QTY, WORKTIME, INITIAL_QTY, RELEASE) "
                         + " Values "
                         + " (:WORK_ORDER, :RC_NO, :PART_ID, :VERSION, :ROUTE_ID , :FACTORY_ID, :PDLINE_ID "
                         + ", :STAGE_ID, :NODE_ID, :PROCESS_ID, :SHEET_NAME, :TERMINAL_ID, :TRAVEL_ID, :NEXT_NODE, :NEXT_PROCESS "
                         + ", :CURRENT_STATUS, :CURRENT_QTY, :IN_PROCESS_EMPID, :IN_PROCESS_TIME, :WIP_IN_QTY "
                         + ", :WIP_IN_EMPID, :WIP_IN_MEMO, :WIP_IN_TIME, :WIP_OUT_GOOD_QTY, :WIP_OUT_SCRAP_QTY "
                         + ", :WIP_OUT_EMPID, :WIP_OUT_MEMO, :WIP_OUT_TIME, :OUT_PROCESS_EMPID, :OUT_PROCESS_TIME "
                         + ", :HAVE_SN, :PRIORITY_LEVEL, :UPDATE_USERID, :UPDATE_TIME, :CREATE_TIME, :BATCH_ID, :EMP_ID, :BONUS_QTY, :WORKTIME, :INITIAL_QTY, :RELEASE) ";
                    rcParams[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", dsRcData.Tables[0].Rows[0]["WORK_ORDER"].ToString() };
                    rcParams[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", sRcNo };
                    rcParams[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", dsRcData.Tables[0].Rows[0]["PART_ID"].ToString() };
                    rcParams[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VERSION", dsRcData.Tables[0].Rows[0]["VERSION"].ToString() };
                    rcParams[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ROUTE_ID", dsRcData.Tables[0].Rows[0]["ROUTE_ID"].ToString() };
                    rcParams[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "FACTORY_ID", dsRcData.Tables[0].Rows[0]["FACTORY_ID"].ToString() };
                    rcParams[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PDLINE_ID", dsRcData.Tables[0].Rows[0]["PDLINE_ID"].ToString() };
                    rcParams[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "STAGE_ID", dsRcData.Tables[0].Rows[0]["STAGE_ID"].ToString() };
                    rcParams[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NODE_ID", dsRcData.Tables[0].Rows[0]["NODE_ID"].ToString() };
                    rcParams[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", dsRcData.Tables[0].Rows[0]["PROCESS_ID"].ToString() };
                    rcParams[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_NAME", dsRcData.Tables[0].Rows[0]["SHEET_NAME"].ToString() };
                    rcParams[11] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TERMINAL_ID", dsRcData.Tables[0].Rows[0]["TERMINAL_ID"].ToString() };
                    // 2016.5.25 By Jason
                    rcParams[12] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TRAVEL_ID", lTravel_Id };
                    //// 2016.4.29 By Jason
                    ////rcParams[12] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TRAVEL_ID", iTravelID.ToString() };
                    //rcParams[12] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TRAVEL_ID", 1 };
                    //// 2016.4.29 End
                    // 2016.5.25 End
                    rcParams[13] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NEXT_NODE", dsRcData.Tables[0].Rows[0]["NEXT_NODE"].ToString() };
                    rcParams[14] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NEXT_PROCESS", dsRcData.Tables[0].Rows[0]["NEXT_PROCESS"].ToString() };
                    rcParams[15] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CURRENT_STATUS", dsRcData.Tables[0].Rows[0]["CURRENT_STATUS"].ToString() };
                    rcParams[16] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CURRENT_QTY", sQty };
                    rcParams[17] = new object[] { ParameterDirection.Input, OracleType.VarChar, "IN_PROCESS_EMPID", dsRcData.Tables[0].Rows[0]["IN_PROCESS_EMPID"].ToString() };

                    if (dsRcData.Tables[0].Rows[0]["IN_PROCESS_TIME"].ToString() == "")
                        rcParams[18] = new object[] { ParameterDirection.Input, OracleType.VarChar, "IN_PROCESS_TIME", dsRcData.Tables[0].Rows[0]["IN_PROCESS_TIME"].ToString() };
                    else
                        rcParams[18] = new object[] { ParameterDirection.Input, OracleType.DateTime, "IN_PROCESS_TIME", Convert.ToDateTime(dsRcData.Tables[0].Rows[0]["IN_PROCESS_TIME"].ToString()) };
                    rcParams[19] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WIP_IN_QTY", dsRcData.Tables[0].Rows[0]["WIP_IN_QTY"].ToString() };
                    rcParams[20] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WIP_IN_EMPID", dsRcData.Tables[0].Rows[0]["WIP_IN_EMPID"].ToString() };
                    rcParams[21] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WIP_IN_MEMO", dsRcData.Tables[0].Rows[0]["WIP_IN_MEMO"].ToString() };
                    if (dsRcData.Tables[0].Rows[0]["WIP_IN_TIME"].ToString() == "")
                        rcParams[22] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WIP_IN_TIME", dsRcData.Tables[0].Rows[0]["WIP_IN_TIME"].ToString() };
                    else
                        rcParams[22] = new object[] { ParameterDirection.Input, OracleType.DateTime, "WIP_IN_TIME", Convert.ToDateTime(dsRcData.Tables[0].Rows[0]["WIP_IN_TIME"].ToString()) };
                    rcParams[23] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WIP_OUT_GOOD_QTY", dsRcData.Tables[0].Rows[0]["WIP_OUT_GOOD_QTY"].ToString() };
                    rcParams[24] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WIP_OUT_SCRAP_QTY", dsRcData.Tables[0].Rows[0]["WIP_OUT_SCRAP_QTY"].ToString() };
                    rcParams[25] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WIP_OUT_EMPID", dsRcData.Tables[0].Rows[0]["WIP_OUT_EMPID"].ToString() };
                    rcParams[26] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WIP_OUT_MEMO", dsRcData.Tables[0].Rows[0]["WIP_OUT_MEMO"].ToString() };
                    if (dsRcData.Tables[0].Rows[0]["WIP_OUT_TIME"].ToString() == "")
                        rcParams[27] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WIP_OUT_TIME", dsRcData.Tables[0].Rows[0]["WIP_OUT_TIME"].ToString() };
                    else
                        rcParams[27] = new object[] { ParameterDirection.Input, OracleType.DateTime, "WIP_OUT_TIME", Convert.ToDateTime(dsRcData.Tables[0].Rows[0]["WIP_OUT_TIME"].ToString()) };
                    rcParams[28] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OUT_PROCESS_EMPID", dsRcData.Tables[0].Rows[0]["OUT_PROCESS_EMPID"].ToString() };
                    if (dsRcData.Tables[0].Rows[0]["OUT_PROCESS_TIME"].ToString() == "")
                        rcParams[29] = new object[] { ParameterDirection.Input, OracleType.VarChar, "OUT_PROCESS_TIME", dsRcData.Tables[0].Rows[0]["OUT_PROCESS_TIME"].ToString() };
                    else
                        rcParams[29] = new object[] { ParameterDirection.Input, OracleType.DateTime, "OUT_PROCESS_TIME", Convert.ToDateTime(dsRcData.Tables[0].Rows[0]["OUT_PROCESS_TIME"].ToString()) };
                    rcParams[30] = new object[] { ParameterDirection.Input, OracleType.VarChar, "HAVE_SN", dsRcData.Tables[0].Rows[0]["HAVE_SN"].ToString().ToString() };
                    rcParams[31] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PRIORITY_LEVEL", dsRcData.Tables[0].Rows[0]["PRIORITY_LEVEL"].ToString() };
                    rcParams[32] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                    rcParams[33] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime };
                    rcParams[34] = new object[] { ParameterDirection.Input, OracleType.DateTime, "CREATE_TIME", datExeTime };
                    rcParams[35] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BATCH_ID", dsRcData.Tables[0].Rows[0]["BATCH_ID"].ToString() };
                    // 2016.4.19 By Jason
                    rcParams[36] = new object[] { ParameterDirection.Input, OracleType.VarChar, "EMP_ID", g_sUserID };
                    // 2016.4.19 End
                    rcParams[37] = new object[] { ParameterDirection.Input, OracleType.VarChar, "BONUS_QTY", 0 };
                    rcParams[38] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORKTIME", iWorkTime };
                    rcParams[39] = new object[] { ParameterDirection.Input, OracleType.VarChar, "INITIAL_QTY", dInitialQtyNew };
                    rcParams[40] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RELEASE", 'Y' };
                    ClientUtils.ExecuteSQL(sSQL, rcParams);



                    // 複製 G_RC_TRAVEL_MACHINE_DOWN
                    sSQL = $@"
                    DECLARE 
                       rw sajet.G_RC_TRAVEL_MACHINE_DOWN%ROWTYPE;
                       vRC VARCHAR2(100) := '{sRcNo}';
                    BEGIN 
                    
                    FOR x IN    (
                        SELECT *  
                        FROM sajet.G_RC_TRAVEL_MACHINE_DOWN r         
                        WHERE r.update_time = ( SELECT MAX(a.update_time) 
                                                FROM sajet.G_RC_TRAVEL_MACHINE_DOWN a 
                                                WHERE a.rc_no=r.rc_no  )
                        AND EXISTS(SELECT * FROM sajet.g_rc_split s 
                                   WHERE s.rc_no = vRC
                                   AND  s.source_rc_no =  r.rc_no )    
                    )LOOP     
                    
                        SELECT r.rc_no, r.travel_id 
                        INTO   x.rc_no, x.travel_id 
                        FROM   sajet.g_rc_status r 
                        WHERE  r.rc_no = vRC;                        
                        INSERT INTO  sajet.G_RC_TRAVEL_MACHINE_DOWN 
                        VALUES x;
                        
                    END LOOP;    
                    EXCEPTION WHEN OTHERS THEN 
                       dbms_output.put_line(
                       DBMS_UTILITY.FORMAT_ERROR_BACKTRACE || DBMS_UTILITY.FORMAT_ERROR_STACK);
                    END ;
                    ";
                    ClientUtils.ExecuteSQL(sSQL);

                }


                if (dsRcData.Tables[0].Rows[0]["HAVE_SN"].ToString() == "1")
                {
                    foreach (string sSerialNumber in dictComponent[gvRow.Index])
                    {
                        object[][] snParams = new object[5][];
                        sSQL = " Insert into SAJET.G_RC_SPLITSN "
                             + " (RC_NO, SERIAL_NUMBER, TRAVEL_ID "
                             + ", UPDATE_USERID, UPDATE_TIME) "
                             + " Values "
                             + " (:RC_NO, :SERIAL_NUMBER, :TRAVEL_ID "
                             + ", :UPDATE_USERID, :UPDATE_TIME) ";
                        snParams[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", sRcNo };
                        snParams[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SERIAL_NUMBER", sSerialNumber };
                        snParams[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TRAVEL_ID", iTravelID.ToString() };
                        snParams[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        snParams[4] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime };
                        ClientUtils.ExecuteSQL(sSQL, snParams);

                        object[][] snParams2 = new object[4][];
                        sSQL = " Update SAJET.G_SN_STATUS set "
                             + " RC_NO = :RC_NO, UPDATE_USERID = :UPDATE_USERID, UPDATE_TIME = :UPDATE_TIME "
                             + " Where SERIAL_NUMBER = :SERIAL_NUMBER";

                        snParams2[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", sRcNo };
                        snParams2[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
                        snParams2[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SERIAL_NUMBER", sSerialNumber };
                        snParams2[3] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime };

                        ClientUtils.ExecuteSQL(sSQL, snParams2);
                        //Update G_RC_TRAVEL_PARAM
                        if (dsRcData.Tables[0].Rows[0]["CURRENT_STATUS"].ToString() == "1")
                            UpdateExtraTables(sRcNo, iTravelID, sSerialNumber);
                    }
                }
                //Update G_RC_TRAVEL_MACHINE；G_RC_TRAVEL_TOOLING
                if (dsRcData.Tables[0].Rows[0]["CURRENT_STATUS"].ToString() == "1")
                    UpdateExtraTables(sRcNo, iTravelID);
            }

            if (HaveSourceRC == false)
            {
                object[][] rcParams = new object[2][];
                sSQL = " Update SAJET.G_RC_STATUS Set CURRENT_STATUS = 6, UPDATE_USERID = :UPDATE_USERID, UPDATE_TIME = :UPDATE_TIME"
                     + " Where RC_NO = '" + editRC.Text + "' and TRAVEL_ID = '" + sSourceTravelID + "'";
                rcParams[0] = new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", g_sUserID };
                rcParams[1] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime };
                ClientUtils.ExecuteSQL(sSQL, rcParams);
            }

            if (dsRcData.Tables[0].Rows[0]["CURRENT_STATUS"].ToString() == "1")
                DeleteExtraTables();
        }

        private void ReadExtraTables()
        {
            sSQL = " Select * from SAJET.G_RC_TRAVEL_MACHINE "
                    + " Where RC_NO = '" + editRC.Text + "' and TRAVEL_ID = '" + sSourceTravelID + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                sMachineId = dsTemp.Tables[0].Rows[0]["MACHINE_ID"].ToString();
                sStartTime = dsTemp.Tables[0].Rows[0]["START_TIME"].ToString();
                sEndTime = dsTemp.Tables[0].Rows[0]["END_TIME"].ToString();
                sLoadPort = dsTemp.Tables[0].Rows[0]["LOAD_PORT"].ToString();
            }

            if (dsRcData.Tables[0].Rows[0]["HAVE_SN"].ToString() == "1")
            {
                sSQL = " Select * from SAJET.G_RC_TRAVEL_PARAM "
                        + " Where RC_NO = '" + editRC.Text + "' and TRAVEL_ID = '" + sSourceTravelID + "'";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);

                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    sItemPhase = dsTemp.Tables[0].Rows[0]["ITEM_PHASE"].ToString();
                    sItemType = dsTemp.Tables[0].Rows[0]["ITEM_TYPE"].ToString();
                    sItemID = dsTemp.Tables[0].Rows[0]["ITEM_ID"].ToString();
                    sItemName = dsTemp.Tables[0].Rows[0]["ITEM_NAME"].ToString();
                    sItemValue = dsTemp.Tables[0].Rows[0]["ITEM_VALUE"].ToString();
                    sValueType = dsTemp.Tables[0].Rows[0]["VALUE_TYPE"].ToString();
                }
            }

            sSQL = " Select * from SAJET.G_RC_TRAVEL_TOOLING "
                    + " Where RC_NO = '" + editRC.Text + "' and TRAVEL_ID = '" + sSourceTravelID + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                sToolingSn = dsTemp.Tables[0].Rows[0]["TOOLING_SN_ID"].ToString();
                sToolingMachine = dsTemp.Tables[0].Rows[0]["MACHINE_ID"].ToString();
                sToolingStart = dsTemp.Tables[0].Rows[0]["START_TIME"].ToString();
                sToolingEnd = dsTemp.Tables[0].Rows[0]["END_TIME"].ToString();
            }
        }

        private void UpdateExtraTables(string sRcNo, int iTravelID)
        {
            object[][] mParams = new object[7][];
            sSQL = " Insert into SAJET.G_RC_TRAVEL_MACHINE "
                    + " (RC_NO, TRAVEL_ID, MACHINE_ID, START_TIME, END_TIME, LOAD_PORT "
                    + ", UPDATE_USERID, UPDATE_TIME) "
                    + " Values "
                    + " (:RC_NO, :TRAVEL_ID, :MACHINE_ID, :START_TIME, :END_TIME, :LOAD_PORT "
                    + ", :UPDATE_USERID, SYSDATE) ";
            mParams[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", sRcNo };
            mParams[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TRAVEL_ID", iTravelID.ToString() };
            mParams[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MACHINE_ID", sMachineId };

            if (sStartTime == "")
                mParams[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "START_TIME", sStartTime };
            else
                mParams[3] = new object[] { ParameterDirection.Input, OracleType.DateTime, "START_TIME", Convert.ToDateTime(sStartTime) };
            if (sEndTime == "")
                mParams[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "END_TIME", sEndTime };
            else
                mParams[4] = new object[] { ParameterDirection.Input, OracleType.DateTime, "END_TIME", Convert.ToDateTime(sEndTime) };
            mParams[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "LOAD_PORT", sLoadPort };
            mParams[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
            ClientUtils.ExecuteSQL(sSQL, mParams);

            object[][] tParams = new object[7][];
            sSQL = " Insert into SAJET.G_RC_TRAVEL_TOOLING "
                    + " (RC_NO, TRAVEL_ID, TOOLING_SN_ID, MACHINE_ID, START_TIME, END_TIME "
                    + ", UPDATE_USERID, UPDATE_TIME) "
                    + " Values "
                    + " (:RC_NO, :TRAVEL_ID, :TOOLING_SN_ID, :MACHINE_ID, :START_TIME, :END_TIME "
                    + ", :UPDATE_USERID, SYSDATE) ";
            tParams[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", sRcNo };
            tParams[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TRAVEL_ID", iTravelID.ToString() };
            tParams[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TOOLING_SN_ID", sToolingSn };
            tParams[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "MACHINE_ID", sToolingMachine };

            if (sToolingStart == "")
                tParams[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "START_TIME", sToolingStart };
            else
                tParams[4] = new object[] { ParameterDirection.Input, OracleType.DateTime, "START_TIME", Convert.ToDateTime(sToolingStart) };
            if (sToolingEnd == "")
                tParams[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "END_TIME", sToolingEnd };
            else
                tParams[5] = new object[] { ParameterDirection.Input, OracleType.DateTime, "END_TIME", Convert.ToDateTime(sToolingEnd) };
            tParams[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
            ClientUtils.ExecuteSQL(sSQL, tParams);
        }

        private void UpdateExtraTables(string sRcNo, int iTravelID, string sSerialNumber)
        {
            object[][] Params = new object[10][];
            sSQL = " Insert into SAJET.G_RC_TRAVEL_PARAM "
                    + " (RC_NO, TRAVEL_ID, SERIAL_NUMBER, ITEM_PHASE, ITEM_TYPE, ITEM_ID "
                    + ", ITEM_NAME, ITEM_VALUE, VALUE_TYPE "
                    + ", UPDATE_USERID, UPDATE_TIME) "
                    + " Values "
                    + " (:RC_NO, :TRAVEL_ID, :SERIAL_NUMBER, :ITEM_PHASE, :ITEM_TYPE, :ITEM_ID "
                    + ", :ITEM_NAME, :ITEM_VALUE, :VALUE_TYPE "
                    + ", :UPDATE_USERID, SYSDATE) ";
            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", sRcNo };
            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "TRAVEL_ID", iTravelID.ToString() };
            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SERIAL_NUMBER", sSerialNumber };
            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_PHASE", sItemPhase };
            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE", sItemType };
            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_ID", sItemID };
            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_NAME", sItemName };
            Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_VALUE", sItemValue };
            Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VALUE_TYPE", sValueType };
            Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", g_sUserID };
            ClientUtils.ExecuteSQL(sSQL, Params);
        }

        private void DeleteExtraTables()
        {
            sSQL = " Delete SAJET.G_RC_TRAVEL_MACHINE "
                 + " Where RC_NO = '" + editRC.Text + "' and TRAVEL_ID = '" + sSourceTravelID + "'";
            ClientUtils.ExecuteSQL(sSQL);

            if (dsRcData.Tables[0].Rows[0]["HAVE_SN"].ToString() == "1")
            {
                sSQL = " Delete SAJET.G_RC_TRAVEL_PARAM "
                     + " Where RC_NO = '" + editRC.Text + "' and TRAVEL_ID = '" + sSourceTravelID + "'";
                ClientUtils.ExecuteSQL(sSQL);
            }

            sSQL = " Delete SAJET.G_RC_TRAVEL_TOOLING "
                 + " Where RC_NO = '" + editRC.Text + "' and TRAVEL_ID = '" + sSourceTravelID + "'";
            ClientUtils.ExecuteSQL(sSQL);
        }
    }
}
