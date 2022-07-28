using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.OracleClient;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Text;
using System.Windows.Forms;
using System;
using PartAQL.Models;
using SajetClass;
using SajetFilter;
using PartAQL.Enums;

namespace PartAQL
{
    public partial class fMain : Form
    {
        public static string g_sUserID;
        public string g_sProgram, g_sFunction;

        public int g_iPrivilege = 0;

        bool bLVCollectionSelected = false;

        /// <summary>
        /// 要設定抽驗項目的物料的資訊
        /// </summary>
        PartDetailModel PartDetail = null;

        /// <summary>
        /// 點選生產途程中某個製程的製程資訊
        /// </summary>
        ProcessDetailModel CurrentNodeDetail = null;

        DataTable SamplingPlanDetail = null;

        string SamplingID = "0";

        public fMain()
        {
            InitializeComponent();
        }

        private void Initial_Form()
        {
            g_sUserID = ClientUtils.UserPara1;
            g_sProgram = ClientUtils.fProgramName;
            g_sFunction = ClientUtils.fFunctionName;

            SajetCommon.SetLanguageControl(this);
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            Initial_Form();
            this.Text = this.Text + " (" + SajetCommon.g_sFileVersion + ")";

            //Select Emp ID
            string s = @"
SELECT
    emp_id,
    nvl(factory_id, 0) factory_id
FROM
    sajet.sys_emp
WHERE
    emp_id = :emp_id
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.Number, "emp_id", g_sUserID },
            };

            DataSet dsTemp = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                SajetCommon.Show_Message("Login User Error", 0);
                return;
            }

            string sUserFacID = dsTemp.Tables[0].Rows[0]["factory_id"].ToString();

            check_privilege();

            LoadInspectionLevelToComboBox();

            CbInspectionLevel.SelectedIndex = 0;
        }

        public void check_privilege()
        {
            string sPrivilege = ClientUtils.GetPrivilege(g_sUserID, g_sFunction, g_sProgram).ToString();

            g_iPrivilege = ClientUtils.GetPrivilege(g_sUserID, g_sFunction, g_sProgram);
        }

        /// <summary>
        /// 顯示生產途程的製程細節
        /// </summary>
        public void ShowProcess()
        {
            if (!CheckPart()) return;

            //Show Process
            TvProcess.Nodes.Clear();

            int iCnt = 0;
            string sStage = "";

            string s = $@"
WITH route_detail AS (
    SELECT
        ROWNUM idx,
        node_content,
        route_id,
        node_id
    FROM
        sajet.sys_rc_route_detail
    START WITH
        node_id = (
            SELECT
                b.node_id
            FROM
                sajet.sys_part              a,
                sajet.sys_rc_route_detail   b
            WHERE
                a.route_id = b.route_id
                AND node_content = 'START'
                AND a.part_no = :part_no
        )
    CONNECT BY
        PRIOR next_node_id = node_id
) -- 遞迴找途程前後順序
SELECT
    nvl(b.route_name, '0') route_name,
    b.route_id,
    nvl(a.process_code, '0') process_code,
    a.process_name,
    a.process_id,
    c.node_id
FROM
    sajet.sys_process    a,
    sajet.sys_rc_route   b,
    route_detail         c,
    sajet.sys_part       d
WHERE
    b.route_id = c.route_id
    AND c.route_id = d.route_id
    AND to_char(a.process_id) = c.node_content
    AND a.enabled = 'Y'
    AND b.enabled = 'Y'
    AND d.part_no = :part_no
ORDER BY
    c.idx
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "part_no", PartDetail.PartNo },
            };

            DataSet DS = ClientUtils.ExecuteSQL(s, p.ToArray());

            for (int i = 0; i <= DS.Tables[0].Rows.Count - 1; i++)
            {
                if (sStage != DS.Tables[0].Rows[i]["route_name"].ToString())
                {
                    sStage = DS.Tables[0].Rows[i]["route_name"].ToString();

                    TreeNode Node1 = new TreeNode
                    {
                        Text = sStage,
                        Tag = DS.Tables[0].Rows[i]["route_id"].ToString()
                    };

                    Node1.Name = Node1.Text;
                    Node1.ImageIndex = 1;

                    TvProcess.Nodes.Add(Node1);

                    iCnt += 1;
                }

                TreeNode NodeProcess = new TreeNode
                {
                    Text = DS.Tables[0].Rows[i]["process_name"].ToString(),
                    Tag = new ProcessDetailModel
                    {
                        RouteID = DS.Tables[0].Rows[i]["route_id"].ToString(),
                        RouteName = DS.Tables[0].Rows[i]["route_name"].ToString(),
                        ProcessID = DS.Tables[0].Rows[i]["process_id"].ToString(),
                        ProcessCode = DS.Tables[0].Rows[i]["process_code"].ToString(),
                        ProcessName = DS.Tables[0].Rows[i]["process_name"].ToString(),
                        NodeID = DS.Tables[0].Rows[i]["node_id"].ToString(),
                    },
                };

                NodeProcess.Name = NodeProcess.Text;
                NodeProcess.ImageIndex = 2;

                TvProcess.Nodes[iCnt - 1].Nodes.Add(NodeProcess);
            }

            foreach (TreeNode node in TvProcess.Nodes)
            {
                node.ExpandAll();
            }
        }

        // Process區域摺疊
        private void collapseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TvProcess.CollapseAll();
        }

        // Process區域展開
        private void expandToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TvProcess.ExpandAll();
        }

        /// <summary>
        /// 從其他料號複製檢驗項目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(editPart.Text) ||
                    PartDetail == null ||
                    PartDetail.PartNo != editPart.Text.Trim())
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Input Part No."), 0);
                    return;
                }
                else
                {
                    using (var f = new fCopyPart
                    {
                        g_sTargetPartid = PartDetail.PartID,
                    })
                    {
                        if (f.ShowDialog() == DialogResult.OK)
                        {
                            show_LvCollection();
                        }
                    }
                }
            }
            catch (Exception)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Error Copy Part No."), 0);
                return;
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            PartDetail = null;

            string s = $@"
SELECT
    t.part_id,
    t.part_no,
    t.spec1,
    t.version
FROM
    sajet.sys_part t
WHERE
    enabled = 'Y'
    AND part_no LIKE :part_no || '%'
ORDER BY
    part_no
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "part_no", editPart.Text.Trim() },
            };

            var ds = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (ds != null && ds.Tables[0].Rows.Count == 1)
            {
                PartDetail = new PartDetailModel
                {
                    PartID = ds.Tables[0].Rows[0]["part_id"].ToString(),
                    PartNo = ds.Tables[0].Rows[0]["part_no"].ToString(),
                    Version = ds.Tables[0].Rows[0]["version"].ToString(),
                };

                editPart.Text = PartDetail.PartNo;
                LbVersion.Text
                    = string.IsNullOrWhiteSpace(PartDetail.Version)
                    ? "N/A"
                    : PartDetail.Version;

                ShowProcess();

                #region 顯示抽樣計畫的方法集

                SamplingID = GetSamplingPlanID();

                ShowSamplingPlan();

                CbInspectionLevel.SelectedIndex = 0;

                ShowSamplingPlanDetail();

                #endregion
            }
            else
            {
                var h = new List<string>
                {
                    "part_id",
                };

                using (var f = new fFilter(sqlCommand: s, @params: p, multiSelect: false, hiddenColumnNames: h))
                {
                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        var result = f.ResultSets[0];

                        PartDetail = new PartDetailModel
                        {
                            PartID = result["part_id"].ToString(),
                            PartNo = result["part_no"].ToString(),
                            Version = result["version"].ToString(),
                        };

                        editPart.Text = PartDetail.PartNo;
                        LbVersion.Text
                            = string.IsNullOrWhiteSpace(PartDetail.Version)
                            ? "N/A"
                            : PartDetail.Version;

                        ShowProcess();

                        #region 顯示抽樣計畫的方法集

                        SamplingID = GetSamplingPlanID();

                        ShowSamplingPlan();

                        CbInspectionLevel.SelectedIndex = 0;

                        ShowSamplingPlanDetail();

                        #endregion
                    }
                }
            }
        }

        private void BtnSetSamplingPlan_Click(object sender, EventArgs e)
        {
            if (!chkData())
            {
                editPart.SelectAll();

                editPart.Focus();

                return;
            }

            string s = @"
WITH part_qc_plan AS (
    SELECT
        sampling_id
    FROM
        sajet.sys_part_qc_plan
    WHERE
        part_id = :part_id
        AND version = :version
        AND route_id = :route_id
        AND node_id = :node_id
        AND enabled = 'Y'
)
SELECT
    COUNT(b.sampling_id) ""check"",
    a.sampling_id,
    a.sampling_type,
    a.sampling_desc
FROM
    sajet.sys_qc_sampling_plan   a,
    part_qc_plan                 b
WHERE
    a.enabled = 'Y'
    AND a.sampling_id = b.sampling_id (+)
GROUP BY
    a.sampling_id,
    a.sampling_type,
    a.sampling_desc
ORDER BY
    a.sampling_type
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.Number, "part_id", PartDetail.PartID },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "version", string.IsNullOrWhiteSpace(PartDetail.Version) ? "N/A" : PartDetail.Version },
                new object[] { ParameterDirection.Input, OracleType.Number, "route_id", CurrentNodeDetail.RouteID },
                new object[] { ParameterDirection.Input, OracleType.Number, "node_id", CurrentNodeDetail.NodeID },
            };

            string c = "check";

            var h = new List<string>
            {
                "sampling_id",
            };

            using (var f = new fFilter(sqlCommand: s, @params: p, multiSelect: false, showCheckBoxColumn: true, checkBoxColumnName: c, hiddenColumnNames: h))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    var newID = f.ResultSets[0]["sampling_id"].ToString();

                    SetSamplingPlan(newID);

                    #region 顯示抽樣計畫的方法集

                    SamplingID = GetSamplingPlanID();

                    ShowSamplingPlan();

                    CbInspectionLevel.SelectedIndex = 0;

                    ShowSamplingPlanDetail();

                    #endregion
                }
            }
        }

        private void btnAddCollection_Click(object sender, EventArgs e)
        {
            if (!chkData())
            {
                editPart.SelectAll();

                editPart.Focus();

                return;
            }

            fCollection f = new fCollection
            {
                g_sPart = PartDetail.PartNo,
                g_sProcess = CurrentNodeDetail.ProcessName,
                g_sType = "Add",
                g_sItemType = "1", // Collection 1: 資料收集
                g_sUserID = g_sUserID,
                g_sPartId = PartDetail.PartID,
                g_sProcessId = CurrentNodeDetail.ProcessID,
                PartDetail = PartDetail,
                CurrentNodeDetail = CurrentNodeDetail,
            };

            f.ShowDialog();

            show_LvCollection();
        }

        private void btnModifyCollection_Click(object sender, EventArgs e)
        {
            if (!chkData())
            {
                editPart.SelectAll();

                editPart.Focus();

                return;
            }

            if (LvCollection.Items.Count == 0) return;

            if (!bLVCollectionSelected || LvCollection.SelectedIndices.Count == 0)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Select a Item."), 0);
                return;
            }

            if (LvCollection.SelectedIndices.Count > 1)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Select One Item."), 0);
                return;
            }

            fCollection f = new fCollection
            {
                g_sPart = PartDetail.PartNo,
                g_sProcess = CurrentNodeDetail.ProcessName,
                g_sType = "Modify",
                g_sItemType = "1", // 1: 資料收集
                g_sUserID = g_sUserID,
                g_sPartId = PartDetail.PartID,
                g_sProcessId = CurrentNodeDetail.ProcessID,
                PartDetail = PartDetail,
                CurrentNodeDetail = CurrentNodeDetail,
            };

            int iSelectIdx = LvCollection.SelectedItems[0].Index;
            f.g_sSeq = LvCollection.Items[iSelectIdx].Text;
            f.g_sName = LvCollection.Items[iSelectIdx].SubItems[1].Text;

            if (f.ShowDialog() == DialogResult.OK)
            {
                show_LvCollection();
            }

            bLVCollectionSelected = false;
        }

        private void btnDeleteCollection_Click(object sender, EventArgs e)
        {
            if (!chkData())
            {
                editPart.SelectAll();

                editPart.Focus();

                return;
            }

            if (LvCollection.Items.Count == 0) return;

            if (!bLVCollectionSelected || LvCollection.SelectedIndices.Count == 0)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Select a Item."), 0);
                return;
            }

            if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Delete the Item?"), 2) == DialogResult.Yes)
            {
                for (int i = 0; i < LvCollection.SelectedIndices.Count; i++)
                {
                    int iSelectIdx = LvCollection.SelectedItems[i].Index;
                    if (!deleteItem(LvCollection.Items[iSelectIdx].Text, LvCollection.Items[iSelectIdx].SubItems[1].Text, "1"))
                    {
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("Delete Item : ") + LvCollection.Items[iSelectIdx].SubItems[1].Text + SajetCommon.SetLanguage(" Error."), 0);
                        return;
                    }
                }

                show_LvCollection();
            }
        }

        private void CbInspectionLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowSamplingPlanDetail();
        }

        private void editPart_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter) return;

            btnSearch_Click(null, null);
        }

        private void LVCollection_SelectedIndexChanged(object sender, EventArgs e)
        {
            bLVCollectionSelected = true;
        }

        private void TreeViewProcess_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            e.Node.SelectedImageIndex = e.Node.ImageIndex;

            if (e.Node.Tag is ProcessDetailModel x)
            {
                CurrentNodeDetail = x;

                LbProcess_3.Text = x.ProcessName;
                LbProcess_2.Text = x.ProcessName;
                LbProcess_1.Text = x.ProcessName;
            }
            else
            {
                CurrentNodeDetail = null;

                LbProcess_3.Text = "N/A";
                LbProcess_2.Text = "N/A";
                LbProcess_1.Text = "N/A";
            }

            show_LvCollection();

            show_LvCondition();

            #region 顯示抽樣計畫的方法集

            SamplingID = GetSamplingPlanID();

            ShowSamplingPlan();

            CbInspectionLevel.SelectedIndex = 0;

            ShowSamplingPlanDetail();

            #endregion
        }

        public bool chkData()
        {
            if (string.IsNullOrWhiteSpace(editPart.Text) ||
                PartDetail == null ||
                PartDetail.PartNo != editPart.Text.Trim())
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Input Part No."), 0);
                return false;
            }

            if (CurrentNodeDetail == null)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Select Target Process."), 0);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 把抽驗等級載入為下拉選單選項
        /// </summary>
        private void LoadInspectionLevelToComboBox()
        {
            var selectItems = new List<ComboBoxItem>();

            foreach (var e in (InspectionLevelEnum[])Enum.GetValues(typeof(InspectionLevelEnum)))
            {
                selectItems.Add(new ComboBoxItem(text: SajetCommon.SetLanguage(e.ToString()), value: ((int)e).ToString()));
            }

            CbInspectionLevel.Items.AddRange(selectItems.ToArray());
        }

        private void show_LvCollection()
        {
            LvCollection.Clear();

            try
            {
                string s = @"
SELECT
    a.item_seq,
    a.item_name,
    a.item_phase,
    a.value_type,
    a.input_type,
    a.convert_type,
    a.necessary,
    a.value_default,
    a.value_list,
    a.print,
    a.column_item,
    a.row_item,
    d.emp_no,
    a.update_time
FROM
    sajet.sys_part_qc_item   a,
    sajet.sys_process        c,
    sajet.sys_emp            d,
    sajet.sys_unit           e
WHERE
    a.part_id = :part_id
    AND a.process_id = c.process_id
    AND a.update_userid = d.emp_id
    AND item_type = 1
    AND a.unit_id = e.unit_id (+)
    AND process_name = :process_name
ORDER BY
    a.item_seq,
    a.item_name
";
                var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.Number, "part_id", PartDetail?.PartID ?? "" },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "process_name", CurrentNodeDetail?.ProcessName ?? "" },
                };

                DataSet dsTemp = ClientUtils.ExecuteSQL(s, p.ToArray());

                LvCollection.Sorting = SortOrder.None;

                for (int i = 0; i < dsTemp.Tables[0].Columns.Count; i++)
                {
                    LvCollection.Columns.Add(SajetCommon.SetLanguage(dsTemp.Tables[0].Columns[i].Caption));
                }

                for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                {
                    LvCollection.Items.Add(dsTemp.Tables[0].Rows[i][0].ToString());

                    for (int j = 1; j < dsTemp.Tables[0].Columns.Count; j++)
                    {
                        if (j > 1)
                            LvCollection.Items[i].SubItems.Add(dataConvert(j, dsTemp.Tables[0].Rows[i][j].ToString()));
                        else
                            LvCollection.Items[i].SubItems.Add(dsTemp.Tables[0].Rows[i][j].ToString());
                    }
                    LvCollection.Items[i].ImageIndex = 0;
                }

                LvCollection.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void show_LvCondition()
        {
            LvCondition.Clear();

            try
            {
                string s = @"
SELECT
    a.item_seq,
    a.item_name,
    a.item_phase,
    a.value_type,
    a.input_type,
    a.convert_type,
    a.necessary,
    a.value_default,
    a.print,
    a.column_item,
    a.row_item,
    e.unit_no,
    d.emp_no,
    a.update_time
FROM
    sajet.sys_part_qc_item   a,
    sajet.sys_process        c,
    sajet.sys_emp            d,
    sajet.sys_unit           e
WHERE
    a.part_id = :part_id
    AND a.process_id = c.process_id
    AND a.update_userid = d.emp_id
    AND item_type = 0
    AND a.unit_id = e.unit_id (+)
    AND process_name = :process_name
ORDER BY
    a.item_seq,
    a.item_name
";
                var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.Number, "part_id", PartDetail?.PartID ?? "" },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "process_name", CurrentNodeDetail?.ProcessName ?? "" },
                };

                DataSet dsTemp = ClientUtils.ExecuteSQL(s, p.ToArray());

                LvCondition.Sorting = SortOrder.None;
                for (int i = 0; i < dsTemp.Tables[0].Columns.Count; i++)
                {
                    LvCondition.Columns.Add(SajetCommon.SetLanguage(dsTemp.Tables[0].Columns[i].Caption));
                }
                for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                {
                    LvCondition.Items.Add(dsTemp.Tables[0].Rows[i][0].ToString());
                    for (int j = 1; j < dsTemp.Tables[0].Columns.Count; j++)
                    {
                        if (j > 1)
                            LvCondition.Items[i].SubItems.Add(dataConvert(j, dsTemp.Tables[0].Rows[i][j].ToString()));
                        else
                            LvCondition.Items[i].SubItems.Add(dsTemp.Tables[0].Rows[i][j].ToString());
                    }
                    LvCondition.Items[i].ImageIndex = 0;
                }
                for (int i = 0; i < dsTemp.Tables[0].Columns.Count; i++)
                    LvCondition.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 綁定抽驗計畫
        /// </summary>
        private bool SetSamplingPlan(string newID)
        {
            string s;

            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.Number, "part_id", long.Parse(PartDetail.PartID) },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "version", string.IsNullOrWhiteSpace(PartDetail.Version) ? "N/A" : PartDetail.Version },
                new object[] { ParameterDirection.Input, OracleType.Number, "route_id", long.Parse(CurrentNodeDetail.RouteID) },
                new object[] { ParameterDirection.Input, OracleType.Number, "node_id", long.Parse(CurrentNodeDetail.NodeID) },
                new object[] { ParameterDirection.Input, OracleType.Number, "old_id", long.Parse(SamplingID) },
                new object[] { ParameterDirection.Input, OracleType.Number, "new_id", long.Parse(newID) },
                new object[] { ParameterDirection.Input, OracleType.Number, "update_userid", long.Parse(g_sUserID) },
                new object[] { ParameterDirection.Input, OracleType.DateTime, "update_time", DateTime.Now },
                new object[] { ParameterDirection.Input, OracleType.Number, "first_id", SysPartQcPlanTable.FirstID },
            };

            if (string.IsNullOrEmpty(SamplingID) || SamplingID == "0")
            {
                s = @"
INSERT INTO sajet.sys_part_qc_plan (
    spqp_id,
    part_id,
    version,
    route_id,
    node_id,
    sampling_id,
    update_userid,
    update_time,
    enabled
) VALUES (
    (
        SELECT
            nvl(MAX(spqp_id) + 1, :first_id)
        FROM
            sajet.sys_part_qc_plan
    ),
    :part_id,
    :version,
    :route_id,
    :node_id,
    :new_id,
    :update_userid,
    :update_time,
    'Y'
)";
            }
            else
            {
                s = @"
DECLARE
    unique_id NUMBER;
BEGIN
    SELECT
        spqp_id
    INTO unique_id
    FROM
        sajet.sys_part_qc_plan
    WHERE
        part_id = :part_id
        AND version = :version
        AND route_id = :route_id
        AND node_id = :node_id
        AND sampling_id = :old_id;

    INSERT INTO sajet.sys_ht_part_qc_plan (
        spqp_id,
        part_id,
        version,
        route_id,
        node_id,
        sampling_id,
        update_userid,
        update_time,
        enabled
    )
        SELECT
            spqp_id,
            part_id,
            version,
            route_id,
            node_id,
            sampling_id,
            update_userid,
            update_time,
            enabled
        FROM
            sajet.sys_part_qc_plan
        WHERE
            spqp_id = unique_id;

    UPDATE sajet.sys_part_qc_plan
    SET
        sampling_id = :new_id,
        update_userid = :update_userid,
        update_time = :update_time
    WHERE
        spqp_id = unique_id;

END;
";
            }

            try
            {
                ClientUtils.ExecuteSQL(s, p.ToArray());

                return true;
            }
            catch (OracleException ex)
            {
                SajetCommon.Show_Message(ex.Message, 0);

                return false;
            }
        }

        /// <summary>
        /// 取得與指定料號、指定製程綁定的檢驗計畫的 ID
        /// </summary>
        /// <returns></returns>
        private string GetSamplingPlanID()
        {
            string s = @"
SELECT
    sampling_id
FROM
    sajet.sys_part_qc_plan
WHERE
    part_id = :part_id
    AND version = :version
    AND route_id = :route_id
    AND node_id = :node_id
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.Number, "part_id", PartDetail?.PartID ?? "" },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "version", string.IsNullOrEmpty(PartDetail?.Version) ? "N/A" : PartDetail.Version },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "route_id", CurrentNodeDetail?.RouteID ?? "" },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "node_id", CurrentNodeDetail?.NodeID ?? "" },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                return d.Tables[0].Rows[0]["sampling_id"].ToString();
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// 顯示綁定的抽驗計畫的名稱
        /// </summary>
        private void ShowSamplingPlan()
        {
            string s = @"
SELECT
    '['
    || sampling_type
    || '] '
    || sampling_desc ""sampling_plan_name""
FROM
    sajet.sys_qc_sampling_plan
WHERE
    sampling_id = :sampling_id
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.Number, "sampling_id", SamplingID },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                LbSamplingPlan.Text = d.Tables[0].Rows[0]["sampling_plan_name"].ToString();
            }
            else
            {
                LbSamplingPlan.Text = "N/A";
            }
        }

        /// <summary>
        /// 顯示檢驗計畫的細節
        /// </summary>
        private void ShowSamplingPlanDetail()
        {
            string s = @"
SELECT
    min_lot_size,
    max_lot_size,
    sample_size,
    critical_reject_qty,
    major_reject_qty,
    minor_reject_qty,
    decode(sampling_unit, '-1', 'N/A', '0', 'Qty',
           '1', '%', 'N/A') AS sampling_unit_desc
FROM
    sajet.sys_qc_sampling_plan_detail
WHERE
    sampling_id = :sampling_id
    AND sampling_level = :sampling_level
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.Number, "sampling_id", SamplingID },
                new object[] { ParameterDirection.Input, OracleType.Number, "sampling_level", (CbInspectionLevel.SelectedItem as ComboBoxItem)?.Value ?? "0" },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d != null)
            {
                DgvSamplingPlanDetail.DataSource = d;

                DgvSamplingPlanDetail.DataMember = d.Tables[0].ToString();
            }

            foreach (DataGridViewColumn column in DgvSamplingPlanDetail.Columns)
            {
                column.HeaderText = SajetCommon.SetLanguage(column.Name);
            }

            DgvSamplingPlanDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private string dataConvert(int type, string data)
        {
            string Msg = string.Empty;
            switch (type)
            {

                case 2:    // 項目階段 A: 全部  I: WIP投入 O: WIP 產出
                    switch (data)
                    {
                        case "A":
                            Msg = "ALL";
                            break;
                        case "I":
                            Msg = "WIP IN";
                            break;
                        case "O":
                            Msg = "WIP Out";
                            break;
                        default:
                            Msg = "ALL";
                            break;
                    }
                    break;

                case 3:  // 數值類型    V:文字  N:數字   L:連結
                    switch (data)
                    {
                        case "V":
                            Msg = "Character";
                            break;
                        case "N":
                            Msg = "Number";
                            break;
                        case "L":
                            Msg = "Link";
                            break;
                        default:
                            Msg = "Character";
                            break;
                    }
                    break;

                case 4:     // 輸入方式    K: KeyIn     S: Select List     R: Range (項目值為數字)
                    switch (data)
                    {
                        case "K":
                            Msg = "Key In";
                            break;
                        case "S":
                            Msg = "Select List";
                            break;
                        case "R":
                            Msg = "Range";
                            break;
                        default:
                            Msg = "Key In";
                            break;
                    }
                    break;

                case 5:      // 輸入值轉換       N: None     U: Uppercase   L: Lowercase
                    switch (data)
                    {
                        case "N":
                            Msg = "None";
                            break;
                        case "U":
                            Msg = "Uppercase";
                            break;
                        case "L":
                            Msg = "Lowercase";
                            break;
                        default:
                            Msg = "None";
                            break;
                    }
                    break;

                case 6:    //項目是否為必要輸入欄位     Y:必要    N:非必要
                    switch (data)
                    {
                        case "Y":
                            Msg = "Yes";
                            break;
                        case "N":
                            Msg = "No";
                            break;
                        default:
                            Msg = "Yes";
                            break;
                    }
                    break;

                default:
                    Msg = data;
                    break;
            }
            return Msg;
        }

        /// <summary>
        /// 刪除產品製程參數
        /// </summary>
        /// <param name="id">項目顯示順序</param>
        /// <param name="name">項目名稱</param>
        /// <param name="type">項目分類</param>
        /// <returns></returns>
        private bool deleteItem(string seq, string name, string type)
        {
            try
            {
                string s = @"
BEGIN
    INSERT INTO sajet.sys_ht_part_qc_item (
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
    )
        SELECT
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
        FROM
            sajet.sys_part_qc_item
        WHERE
            part_id = :part_id
            AND version = :version
            AND node_id = :node_id
            AND item_type = :item_type
            AND item_seq = :item_seq
            AND item_name = :item_name;

    DELETE sajet.sys_part_qc_item
    WHERE
        part_id = :part_id
        AND version = :version
        AND node_id = :node_id
        AND item_type = :item_type
        AND item_seq = :item_seq
        AND item_name = :item_name;

END;
";
                var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.Number, "part_id", PartDetail.PartID },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "version", string.IsNullOrWhiteSpace(PartDetail.Version) ? "N/A" : PartDetail.Version },
                    new object[] { ParameterDirection.Input, OracleType.Number, "node_id", CurrentNodeDetail.NodeID },
                    new object[] { ParameterDirection.Input, OracleType.Number, "item_type", type },
                    new object[] { ParameterDirection.Input, OracleType.Number, "item_seq", seq },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "item_name", name },
                };

                ClientUtils.ExecuteSQL(s, p.ToArray());

                return true;
            }
            catch (OracleException ex)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Delete Item Error.") + ex.Message, 0);
                return false;
            }
        }

        //定義一個函數,作用:判斷alphabe是否為英文字母,是英文字母返回True,不是英文字母返回False
        public bool IsAlphabe(string alphabe)
        {
            Regex NumberPattern = new Regex(@"^[A-Za-z0-9]+$");
            return NumberPattern.IsMatch(alphabe);
        }

        public bool IsNumber(string alphabe)
        {
            Regex NumberPattern = new Regex(@"^[0-9]*[1-9][0-9]*$");  //正整數
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

        /// <summary>
        /// 檢查料號是否存在
        /// </summary>
        /// <returns></returns>
        private bool CheckPart()
        {
            return !(string.IsNullOrWhiteSpace(editPart.Text)
                || PartDetail == null
                || PartDetail.PartNo != editPart.Text.Trim());
        }
    }
}
