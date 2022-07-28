using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Collections.Specialized;
using SajetFilter;
using System.Data.OracleClient;

namespace RCProcessParam
{
    public partial class fCopyPart : Form
    {
        public string g_sTargetPartid, g_sOriginPartid;
        string sSQL;
        DataSet dsTemp;
        public fCopyPart()
        {
            InitializeComponent();
        }

        private void editPart_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                return;
            btnSearch_Click(null, null);
            setPart();

            ckbAll.Checked = true;
            ckbAll_CheckedChanged(sender, e);
        }

        private void setPart()
        {
            if (string.IsNullOrEmpty(editPart.Text))
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Input Part No."), 0);
                return;
            }
            else
            {
                //sSQL = "SELECT PART_ID, PART_NO FROM SAJET.SYS_PART WHERE PART_NO ='" + editPart.Text.Trim() + "' ";
                //dsTemp = ClientUtils.ExecuteSQL(sSQL);

                //if (dsTemp.Tables[0].Rows.Count == 0)
                //{
                //    SajetCommon.Show_Message(SajetCommon.SetLanguage("Error Part No."), 0);
                //    return;
                //}
                //g_sOriginPartid = dsTemp.Tables[0].Rows[0]["PART_ID"].ToString();
            }
            try
            {
                LVAll.Clear();
                LVChoose.Clear();

                sSQL = " SELECT DISTINCT B.PROCESS_NAME PROCESS"
                          + " FROM SAJET.SYS_RC_PROCESS_PARAM_PART A, SAJET.SYS_PROCESS B "
                          + " WHERE A.PROCESS_ID = B.PROCESS_ID AND A.ENABLED = 'Y' "
                          + " AND A.PART_ID = '" + g_sOriginPartid + "'  ORDER BY B.PROCESS_NAME ";
                dsTemp = ClientUtils.ExecuteSQL(sSQL);

                LVAll.Sorting = SortOrder.None;
                for (int i = 0; i < dsTemp.Tables[0].Columns.Count; i++)
                {
                    LVAll.Columns.Add(SajetCommon.SetLanguage(dsTemp.Tables[0].Columns[i].Caption));
                }
                for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                {
                    LVAll.Items.Add(dsTemp.Tables[0].Rows[i]["PROCESS"].ToString());

                }
                for (int i = 0; i < dsTemp.Tables[0].Columns.Count; i++)
                    LVAll.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.HeaderSize);

                LVChoose.Columns.Clear();
                for (int i = 0; i < LVAll.Columns.Count; i++)
                {
                    LVChoose.Columns.Add(LVAll.Columns[i].Text);
                    LVChoose.Columns[i].Width = LVAll.Columns[i].Width;
                }
            }
            catch (Exception)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Input Part No."), 0);
                return;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string s = @"
SELECT
    B.PART_ID,
    B.PART_NO,
    B.SPEC1,
    B.VERSION
FROM
    SAJET.SYS_PART B
WHERE
    B.ENABLED = 'Y'
    AND B.PART_NO LIKE :PART_NO || '%'
ORDER BY
    B.PART_NO
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_NO", editPart.Text.Trim() },
            };

            var h = new List<string>
            {
                "PART_ID",
            };

            using (var f = new SajetFilter.FFilter(sqlCommand: s, @params: p, hiddenColumns: h))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    var result = f.ResultSets[0];

                    editPart.Text = result["PART_NO"].ToString();
                    g_sOriginPartid = result["part_id"].ToString();
                    lblVersion.Text = result["version"].ToString();

                    setPart();
                    ckbAll.Checked = true;
                    ckbAll_CheckedChanged(sender, e);
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (LVChoose.Items.Count == 0)
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Choose Process."), 0);
                    return;
                }
                else
                {
                    for (int i = 0; i < LVChoose.Items.Count; i++)
                    {
                        sSQL = " SELECT B.ITEM_NAME, B.ITEM_ID "
                                + " FROM SAJET.SYS_RC_PROCESS_PARAM_PART A, SAJET.SYS_RC_PROCESS_PARAM_PART B, "
                                + " SAJET.SYS_PROCESS C "
                                + " WHERE A.PART_ID = '" + g_sOriginPartid + "' "
                                + " AND B.PART_ID = '" + g_sTargetPartid + "' "
                                + " AND C.PROCESS_NAME = '" + LVChoose.Items[i].Text + "' "
                                + " AND C.PROCESS_ID = B.PROCESS_ID "
                                + " AND A.PROCESS_ID = B.PROCESS_ID "
                                + " AND A.ITEM_TYPE = B.ITEM_TYPE ";
                        //+" AND (A.ITEM_NAME = B.ITEM_NAME OR A.ITEM_SEQ = B.ITEM_SEQ) ";
                        dsTemp = ClientUtils.ExecuteSQL(sSQL);

                        if (dsTemp.Tables[0].Rows.Count > 0)
                        {
                            if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Data Duplcate in " + LVChoose.Items[i].Text + " Do you want Cover ?"), 2) == DialogResult.No)
                                continue;
                            else
                            {
                                for (int j = 0; j < dsTemp.Tables[0].Rows.Count; j++)
                                {
                                    sSQL = " DELETE SAJET.SYS_RC_PROCESS_PARAM_PART "
                                              + " WHERE ITEM_ID ='" + dsTemp.Tables[0].Rows[j]["ITEM_ID"].ToString() + "' ";
                                    ClientUtils.ExecuteSQL(sSQL);
                                }
                            }
                        }


                        sSQL = " SELECT "
                               + " A.PROCESS_ID, A.ITEM_NAME, A.ITEM_PHASE, A.ITEM_TYPE, A.ITEM_SEQ, A.VALUE_TYPE, "
                               + " A.INPUT_TYPE, A.VALUE_DEFAULT, A.VALUE_LIST, A.NECESSARY, A.CONVERT_TYPE, A.ENABLED, A.PRINT "
                               + " FROM  SAJET.SYS_RC_PROCESS_PARAM_PART A, SAJET.SYS_PROCESS B "
                               + " WHERE A.PROCESS_ID = B.PROCESS_ID "
                               + " AND A.PART_ID = '" + g_sOriginPartid + "' "
                               + " AND B.PROCESS_NAME ='" + LVChoose.Items[i].Text + "' ";
                        dsTemp = ClientUtils.ExecuteSQL(sSQL);

                        for (int k = 0; k < dsTemp.Tables[0].Rows.Count; k++)
                        {
                            sSQL = "INSERT INTO SAJET.SYS_RC_PROCESS_PARAM_PART "
                                   + "(PROCESS_ID, ITEM_NAME, ITEM_PHASE, ITEM_TYPE, ITEM_SEQ, VALUE_TYPE,"
                                   + " INPUT_TYPE,VALUE_DEFAULT, VALUE_LIST, NECESSARY, CONVERT_TYPE, ENABLED,"
                                   + " PART_ID,  ITEM_ID, UPDATE_USERID, PRINT) "
                                   + " VALUES ( "
                                   + " :PROCESS_ID, :ITEM_NAME, :ITEM_PHASE, :ITEM_TYPE, :ITEM_SEQ, :VALUE_TYPE,"
                                   + " :INPUT_TYPE, :VALUE_DEFAULT, :VALUE_LIST, :NECESSARY, :CONVERT_TYPE, :ENABLED,"
                                   + " :PART_ID,  :ITEM_ID, :UPDATE_USERID, :PRINT) ";
                            string ItemId = SajetCommon.GetMaxID("SAJET.SYS_RC_PROCESS_PARAM_PART", "ITEM_ID", 8);// Convert.ToString(Convert.ToInt32(SajetCommon.GetMaxID("SAJET.SYS_RC_PROCESS_PARAM_PART", "ITEM_ID", 8)) + 1);
                            object[][] Params = new object[16][];
                            Params[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", dsTemp.Tables[0].Rows[k]["PROCESS_ID"].ToString() };
                            Params[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_NAME", dsTemp.Tables[0].Rows[k]["ITEM_NAME"].ToString() };
                            Params[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_PHASE", dsTemp.Tables[0].Rows[k]["ITEM_PHASE"].ToString() };
                            Params[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_TYPE", dsTemp.Tables[0].Rows[k]["ITEM_TYPE"].ToString() };
                            Params[4] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_SEQ", dsTemp.Tables[0].Rows[k]["ITEM_SEQ"].ToString() };
                            Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VALUE_TYPE", dsTemp.Tables[0].Rows[k]["VALUE_TYPE"].ToString() };
                            Params[6] = new object[] { ParameterDirection.Input, OracleType.VarChar, "INPUT_TYPE", dsTemp.Tables[0].Rows[k]["INPUT_TYPE"].ToString() };
                            Params[7] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VALUE_DEFAULT", dsTemp.Tables[0].Rows[k]["VALUE_DEFAULT"].ToString() };
                            Params[8] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VALUE_LIST", dsTemp.Tables[0].Rows[k]["VALUE_LIST"].ToString() };
                            Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NECESSARY", dsTemp.Tables[0].Rows[k]["NECESSARY"].ToString() };
                            Params[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "CONVERT_TYPE", dsTemp.Tables[0].Rows[k]["CONVERT_TYPE"].ToString() };
                            Params[11] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ENABLED", dsTemp.Tables[0].Rows[k]["ENABLED"].ToString() };
                            Params[12] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_ID", g_sTargetPartid };
                            Params[13] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_ID", ItemId };
                            Params[14] = new object[] { ParameterDirection.Input, OracleType.VarChar, "UPDATE_USERID", ClientUtils.UserPara1 };
                            Params[15] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PRINT", dsTemp.Tables[0].Rows[k]["PRINT"].ToString() };
                            ClientUtils.ExecuteSQL(sSQL, Params);
                        }
                    }

                    DialogResult = DialogResult.OK;
                }
            }
            catch (Exception)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Error Copy."), 0);
                return;
            }

        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            if (LVAll.SelectedItems.Count == 0) return;
            LVChoose.Sorting = SortOrder.None;
            for (int i = LVAll.SelectedItems.Count - 1; i >= 0; i--)
            {
                LVChoose.Items.Add(LVAll.SelectedItems[i].Text);
                for (int j = 1; j < LVAll.Columns.Count; j++)
                    LVChoose.Items[LVChoose.Items.Count - 1].SubItems.Add(LVAll.SelectedItems[i].SubItems[j]);
                LVChoose.Items[LVChoose.Items.Count - 1].ImageIndex = 0;
                LVAll.SelectedItems[i].Remove();
            }
            LVChoose.Sorting = SortOrder.Ascending;
            gbChoose.Text = SajetCommon.SetLanguage("Choose") + ": " + LVChoose.Items.Count.ToString();

            ckbAll.Checked = false;
            ckbAll.Enabled = true;
        }

        private void btnUnchoose_Click(object sender, EventArgs e)
        {

            if (LVChoose.SelectedItems.Count == 0) return;
            LVAll.Sorting = SortOrder.None;
            for (int i = LVChoose.SelectedItems.Count - 1; i >= 0; i--)
            {
                LVAll.Items.Add(LVChoose.SelectedItems[i].Text);
                for (int j = 1; j < LVChoose.Columns.Count; j++)
                    LVAll.Items[LVAll.Items.Count - 1].SubItems.Add(LVChoose.SelectedItems[i].SubItems[j]);
                LVAll.Items[LVAll.Items.Count - 1].ImageIndex = 0;
                LVChoose.SelectedItems[i].Remove();
            }
            LVAll.Sorting = SortOrder.Ascending;
            gbChoose.Text = SajetCommon.SetLanguage("Choose") + ": " + LVChoose.Items.Count.ToString();

            ckbAll.Checked = false;
            ckbAll.Enabled = true;

        }

        private void ckbAll_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbAll.Checked)
            {
                for (int i = 0; i < LVAll.Items.Count; i++)
                {
                    LVAll.Items[i].Selected = true;
                }

                for (int i = 0; i < LVChoose.Items.Count; i++)
                {
                    LVChoose.Items[i].Selected = true;
                }
            }
            else
            {
                for (int i = 0; i < LVAll.Items.Count; i++)
                {
                    LVAll.Items[i].Selected = false;
                    LVAll.Items[i].BackColor = Color.White;
                }

                for (int i = 0; i < LVChoose.Items.Count; i++)
                {
                    LVChoose.Items[i].Selected = false;
                    LVChoose.Items[i].BackColor = Color.White;
                }
            }
        }

        private void LVAll_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ckbAll.Checked = false;
            //ckbAll.Enabled = true;
        }

        private void LVChoose_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ckbAll.Checked = false;
            //ckbAll.Enabled = true;
        }

        private void fCopyPart_Load(object sender, EventArgs e)
        {
            SajetCommon.SetLanguageControl(this);
        }

        private void LVAll_DoubleClick(object sender, EventArgs e)
        {
            // int idx = LVAll.SelectedIndices[0];
            if (LVAll.SelectedItems.Count == 0)
                return;

            ListViewItem item = LVAll.SelectedItems[0];

            LVAll.Items.Remove(item);
            LVChoose.Items.Add(item);
        }

    }
}