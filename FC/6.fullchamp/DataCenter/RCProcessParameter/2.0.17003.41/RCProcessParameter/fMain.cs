using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Collections.Specialized;
using System.Text.RegularExpressions;//導入命名空間(正則表達式)
using System.Data.OracleClient;
using System.Linq;
using OtSrv = RCProcessParam.Services.OtherService;
using QoSrv = RCProcessParam.Services.QCOptionService;
using RCProcessParam.Models;
using RCProcessParam.Enums;

namespace RCProcessParam
{
    public partial class fMain : Form
    {
        public string g_SystemType; // Y: Lot Control, N:Piece
        public int g_iPrivilege = 0;

        public static string g_sUserID;
        public string g_sProgram, g_sFunction;

        public string g_sPartId, g_sPrint;
        public string g_sProcessId;

        /// <summary>
        /// 目前選取的製程節點的資訊
        /// </summary>
        private ProcessViewModel this_process = null;

        StringCollection g_tsEnabled = new StringCollection();
        bool bLVConditionSelected = false, bLVCollectionSelected = false, bLVSNConditionSelected = false, bLVSNCollectionSelected = false, blvAlertCollectionSelected = false;

        #region Isaac QC AQL 
        bool bLVQCCollectionSelected = false;
        bool bLVAQLCollectionSelected = false;
        QCCollection g_QCCollection = new QCCollection();
        AQL g_AQL = new AQL();
        #endregion

        public fMain()
        {
            InitializeComponent();

            LvAQL.DoubleClick += LvAQLCollection_DoubleClick;

            BtnQCOption.Click += BtnQCOption_Click;

            this.Shown += FMain_Shown;
        }

        private void Initial_Form()
        {
            g_sUserID = ClientUtils.UserPara1;
            g_sProgram = ClientUtils.fProgramName;
            g_sFunction = ClientUtils.fFunctionName;

            SajetCommon.SetLanguageControl(this);

            this.Text = this.Text + " (" + SajetCommon.g_sFileVersion + ")";

            BtnAQLEnable.Text = SajetClass.SajetCommon.SetLanguage(FormTextEnum.EnableOrDisable.ToString());
        }

        private void FMain_Load(object sender, EventArgs e)
        {
            Initial_Form();

            //Select Emp ID
            string s = @"
SELECT
    EMP_ID,
    NVL(FACTORY_ID, 0) FACTORY_ID
FROM
    SAJET.SYS_EMP
WHERE
    EMP_ID = :EMP_ID
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.Number, "EMP_ID", g_sUserID },
            };

            DataSet dsTemp = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (dsTemp.Tables[0].Rows.Count == 0)
            {
                SajetCommon.Show_Message("Login User Error", 0);
                return;
            }
            string sUserFacID = dsTemp.Tables[0].Rows[0]["FACTORY_ID"].ToString();

            // Visible tabPage by system type
            if (systemType())
            {
                // lot control
                this.TpSNCondition.Parent = null;
                this.TpSNCollection.Parent = null;
                this.TpAlert.Parent = null;
                this.TpKeypart.Parent = null; // by Lee@180801 there is no keyparts collection for FullChamp

                // Isaac QC AQL 頁籤 2021/01/07
                if (!g_QCCollection.Check_Privilege(g_sUserID, "QC_AQL"))
                {
                    BtnQCCollectionAdd.Enabled = false;
                    BtnQCCollectionModify.Enabled = false;
                    BtnQCCollectionDelete.Enabled = false;

                    BtnAQLAdd.Enabled = false;
                    BtnAQLEnable.Enabled = false;
                    BtnAQLModify.Enabled = false;

                    BtnQCOption.Enabled = false;
                }
            }
            else
            {
                // piece
                this.TpCondition.Parent = null;
                this.TpCollection.Parent = null;

                // Isaac QC AQL 頁籤 2021/01/07
                this.TpAQL.Parent = null;
                this.TpQCCollection.Parent = null;
            }

            ShowProcess();
            Check_privilege();
        }

        private void FMain_Shown(object sender, EventArgs e)
        {
            editPart.Focus();
        }

        /// <summary>
        /// 將 Node 拖移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        /// <summary>
        /// 將移動 Node 物件移至目標
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        /// <summary>
        /// 點選某個製程節點，載入相關資料到表單上
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TvProcess_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            e.Node.SelectedImageIndex = e.Node.ImageIndex;

            if (e.Node.Tag is ProcessViewModel m)
            {
                this_process = new ProcessViewModel(m);

                string process_name = this_process.PROCESS_NAME;

                LbProcess1.Text = process_name;
                LbProcess2.Text = process_name;
                LbProcess5.Text = process_name;
                LbProcess6.Text = process_name;
                LbProcess7.Text = process_name;
                LbProcess8.Text = process_name;

                show_LVCondition();
                show_LVCollection();
                show_LVSNCondition();
                show_LVSNCollection();
                show_lvAlertCollection();
                show_LVMaterial();

                #region Isaac QC Collection

                LbProcess3.Text = process_name;

                g_QCCollection.ShowListView(g_sPartId, process_name, ref LvQCCollection);

                #endregion

                #region Isaac AQL

                LbProcess4.Text = process_name;

                g_AQL.ShowListView(g_sPartId, process_name, ref LvAQL);

                #endregion

                LbProcess9.Text = process_name;

                Show_QCOption(this_process);
            }
            else
            {
                LbProcess1.Text = "N/A";
                LbProcess2.Text = "N/A";
                LbProcess3.Text = "N/A";
                LbProcess4.Text = "N/A";
                LbProcess5.Text = "N/A";
                LbProcess6.Text = "N/A";
                LbProcess7.Text = "N/A";
                LbProcess8.Text = "N/A";
                LbProcess9.Text = "N/A";

                this_process = null;
            }
        }

        /// <summary>
        /// Process 區域摺疊
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TsmiCollapse_Click(object sender, EventArgs e)
        {
            TvProcess.CollapseAll();
        }

        /// <summary>
        /// Process 區域展開
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TsmiExpand_Click(object sender, EventArgs e)
        {
            TvProcess.ExpandAll();
        }

        /// <summary>
        /// 複製其他料號參數
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TsmiCopy_Click(object sender, EventArgs e)
        {
            try
            {
                fCopyPart f = new fCopyPart();

                if (string.IsNullOrEmpty(editPart.Text))
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Input Part No."), 0);
                    return;
                }
                else
                {
                    string s = " SELECT PART_ID, PART_NO FROM SAJET.SYS_PART WHERE PART_ID = :PART_ID ";
                    var p = new List<object[]>
                    {
                        new object[] { ParameterDirection.Input, OracleType.Number, "PART_ID", g_sPartId },
                    };

                    DataSet dsTemp = ClientUtils.ExecuteSQL(s, p.ToArray());

                    if (dsTemp.Tables[0].Rows.Count == 0)
                    {
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("Error Part No."), 0);
                        return;
                    }
                    else
                    {
                        f.g_sTargetPartid = dsTemp.Tables[0].Rows[0]["PART_ID"].ToString();
                    }
                }

                if (f.ShowDialog() == DialogResult.OK)
                {
                    show_LVCondition();
                    show_LVCollection();
                    show_LVSNCondition();
                    show_LVSNCollection();
                    show_lvAlertCollection();
                    show_LVMaterial();
                }
            }
            catch (Exception)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Error Copy Part No."), 0);
                return;
            }

        }

        // Isaac 修改料號空白時清除資料 (2021/01/07)
        private void editPart_KeyPress(object sender, KeyPressEventArgs e)
        {
            this_process = null;

            if (e.KeyChar != (char)Keys.Enter)
                return;
            if (string.IsNullOrEmpty(editPart.Text))
            {
                if (TpCondition.Parent != null)
                {
                    Tc_1.SelectedTab = TpCondition;
                }
                else
                {
                    Tc_1.SelectedTab = TpSNCondition;
                }

                ClearData();
            }
            else
            {
                BtnSearch_Click(null, null);
                show_LVCondition();
                show_LVCollection();
                show_LVSNCondition();
                show_LVSNCollection();
                show_lvAlertCollection();
                show_LVMaterial();

                #region Isaac QC Collection / AQL
                g_QCCollection.ShowListView(g_sPartId, LbProcess3.Text.Trim(), ref LvQCCollection);
                g_AQL.ShowListView(g_sPartId, LbProcess4.Text.Trim(), ref LvAQL);
                #endregion

                Show_QCOption(this_process);
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            this_process = null;

            g_sPartId = "";

            string s = $@"
SELECT
    T.PART_ID,
    T.PART_NO,
    T.VERSION,
    T.OPTION2 FORMER_PART_NO,
    T.SPEC1,
    T.SPEC2,
    T.OPTION4 BLUEPRINT
FROM
    SAJET.SYS_PART T
WHERE
    ENABLED = 'Y'
    AND UPPER(T.PART_NO) LIKE UPPER(:PART_NO)
                              || '%'
    OR UPPER(T.OPTION2) LIKE UPPER(:PART_NO)
                             || '%'
ORDER BY
    T.PART_NO
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_NO", editPart.Text.Trim() },
            };

            DataSet ds = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows.Count == 1)
                {
                    ClearData();
                    editPart.Text = ds.Tables[0].Rows[0]["PART_NO"].ToString();
                    //LbVersion.Text = ds.Tables[0].Rows[0]["version"].ToString();
                    g_sPartId = ds.Tables[0].Rows[0]["part_id"].ToString();

                    var part_info = OtSrv.GetPartInfo(g_sPartId);

                    OtSrv.LoadPartInfo(ref DgvPartInfo, part_info);

                    if (OtSrv.CheckForRouteEnable(g_sPartId, out string route_name))
                    {
                        ShowProcess();
                        Show_Preview();
                    }
                    else
                    {
                        string message
                            = SajetCommon.SetLanguage("Production route is not available")
                            + Environment.NewLine
                            + SajetCommon.SetLanguage("Part No")
                            + " : "
                            + editPart.Text
                            + Environment.NewLine
                            + SajetCommon.SetLanguage("route name")
                            + " : "
                            + route_name;

                        SajetCommon.Show_Message(message, 1);
                    }
                }
                else
                {
                    var h = new List<string>
                    {
                        "PART_ID",
                    };

                    using (var f = new SajetFilter.FFilter(sqlCommand: s, @params: p, hiddenColumns: h))
                    {
                        f.Text = SajetCommon.SetLanguage("Part No");

                        ClearData();

                        if (f.ShowDialog() == DialogResult.OK)
                        {
                            var result = f.ResultSets[0];

                            editPart.Text = result["PART_NO"].ToString();

                            g_sPartId = result["PART_ID"].ToString();

                            //LbVersion.Text = result["VERSION"].ToString();

                            var part_info = OtSrv.GetPartInfo(g_sPartId);

                            OtSrv.LoadPartInfo(ref DgvPartInfo, part_info);

                            if (OtSrv.CheckForRouteEnable(g_sPartId, out string route_name))
                            {
                                ShowProcess();
                                Show_Preview();
                            }
                            else
                            {
                                string message
                                    = SajetCommon.SetLanguage("Production route is not available")
                                    + Environment.NewLine
                                    + SajetCommon.SetLanguage("Part No")
                                    + " : "
                                    + editPart.Text
                                    + Environment.NewLine
                                    + SajetCommon.SetLanguage("route name")
                                    + " : "
                                    + route_name;

                                SajetCommon.Show_Message(message, 1);
                            }
                        }
                    }
                }
            }
            else
            {
                string message = SajetCommon.SetLanguage("Part not found");

                SajetCommon.Show_Message(message, 0);
            }

            editPart.SelectAll();

            editPart.Focus();

            Cursor = Cursors.Default;
        }

        private void BtnConditionAdd_Click(object sender, EventArgs e)
        {
            if (!chkData()) return;

            fCondition f = new fCondition
            {
                g_sPart = editPart.Text,
                g_sProcess = LbProcess1.Text.Trim(),
                g_sType = "Add",
                g_sItemType = "0", // Condition 0: 製程條件
                g_sUserID = g_sUserID,
                g_sPartId = g_sPartId,
                g_sProcessId = g_sProcessId
            };

            f.ShowDialog();

            show_LVCondition();
            Show_Preview();
        }

        private void BtnConditionModify_Click(object sender, EventArgs e)
        {
            if (!chkData()) return;

            if (LvCondition.Items.Count == 0) return;

            if (!bLVConditionSelected || LvCondition.SelectedIndices.Count == 0)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Select a Item."), 0);
                return;
            }

            if (LvCondition.SelectedIndices.Count > 1)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Select One Item."), 0);
                return;
            }

            fCondition f = new fCondition
            {
                g_sPart = editPart.Text,
                g_sProcess = LbProcess1.Text.Trim(),
                g_sType = "Modify",
                g_sItemType = "0", // Condition 0: 製程條件
                g_sUserID = g_sUserID,
                g_sPartId = g_sPartId,
                g_sProcessId = g_sProcessId
            };

            int iSelectIdx = LvCondition.SelectedItems[0].Index;
            f.g_sSeq = LvCondition.Items[iSelectIdx].Text;
            f.g_sName = LvCondition.Items[iSelectIdx].SubItems[1].Text;

            if (f.ShowDialog() == DialogResult.OK)
            {
                show_LVCondition();
                Show_Preview();
            }

            bLVConditionSelected = false;
        }

        private void BtnConditionDelete_Click(object sender, EventArgs e)
        {
            if (!chkData()) return;

            if (LvCondition.Items.Count == 0) return;

            if (!bLVConditionSelected || LvCondition.SelectedIndices.Count == 0)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Select a Item."), 0);
                return;
            }

            //if (LVCondition.SelectedIndices.Count > 1)
            //{
            //    SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Select One Item."), 0);
            //    return;
            //}

            if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Delete the Item?"), 2) == DialogResult.Yes)
            {
                for (int i = 0; i < LvCondition.SelectedIndices.Count; i++)
                {
                    int iSelectIdx = LvCondition.SelectedItems[i].Index;

                    if (!deleteItem(LvCondition.Items[iSelectIdx].Text, LvCondition.Items[iSelectIdx].SubItems[1].Text, "0"))
                    {
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("Delete Item : ") + LvCondition.Items[iSelectIdx].SubItems[1].Text + SajetCommon.SetLanguage(" Error."), 0);
                        return;
                    }
                }

                show_LVCondition();
                Show_Preview();
            }
        }

        private void BtnCollectionAdd_Click(object sender, EventArgs e)
        {
            if (!chkData()) return;

            fCollection f = new fCollection
            {
                g_sPart = editPart.Text,
                g_sProcess = LbProcess2.Text.Trim(),
                g_sType = "Add",
                g_sItemType = "1", // Collection 1: 資料收集
                g_sUserID = g_sUserID,
                g_sPartId = g_sPartId,
                g_sProcessId = g_sProcessId
            };

            f.ShowDialog();

            show_LVCollection();
            Show_Preview();
        }

        private void BtnCollectionModify_Click(object sender, EventArgs e)
        {
            if (!chkData()) return;

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
                g_sPart = editPart.Text,
                g_sProcess = LbProcess1.Text.Trim(),
                g_sType = "Modify",
                g_sItemType = "1", // 1: 資料收集
                g_sUserID = g_sUserID,
                g_sPartId = g_sPartId,
                g_sProcessId = g_sProcessId
            };

            int iSelectIdx = LvCollection.SelectedItems[0].Index;
            f.g_sSeq = LvCollection.Items[iSelectIdx].Text;
            f.g_sName = LvCollection.Items[iSelectIdx].SubItems[1].Text;

            if (f.ShowDialog() == DialogResult.OK)
            {
                show_LVCollection();
                Show_Preview();
            }

            bLVCollectionSelected = false;
        }

        private void BtnCollectionDelete_Click(object sender, EventArgs e)
        {
            if (!chkData()) return;

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
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("Delete Item : ") + LvCondition.Items[iSelectIdx].SubItems[1].Text + SajetCommon.SetLanguage(" Error."), 0);
                        return;
                    }
                }

                show_LVCollection();
                Show_Preview();
            }
        }

        /// <summary>
        /// 新增料號綁定 AQL 規則 (Isaac 2021/01/08)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAQLAdd_Click(object sender, EventArgs e)
        {
            if (!chkData()) return;

            // 取得資料筆數            
            int qcRow = g_AQL.GetDataRow(g_sPartId, g_sProcessId);

            if (qcRow > 0)
            {
                // 顯示確認訊息                
                if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Overwrite the AQL Item?"), 2) == DialogResult.Yes)
                {
                    string SamplingID = string.Empty;

                    string sSQL = @"
SELECT DISTINCT
    A.SAMPLING_ID,
    B.SAMPLING_TYPE,
    B.SAMPLING_DESC
FROM
    SAJET.SYS_QC_SAMPLING_PLAN_DETAIL   A,
    SAJET.SYS_QC_SAMPLING_PLAN          B
WHERE
    A.SAMPLING_ID = B.SAMPLING_ID
    AND A.SAMPLING_LEVEL = '0'
    AND B.ENABLED = 'Y'
";
                    var h = new List<string>
                    {
                        "SAMPLING_ID",
                    };

                    using (var f = new SajetFilter.FFilter(sqlCommand: sSQL, hiddenColumns: h))
                    {
                        f.Text = SajetCommon.SetLanguage("Sampling plan");

                        if (f.ShowDialog() == DialogResult.OK)
                        {
                            var result = f.ResultSets[0];

                            SamplingID = result["SAMPLING_ID"].ToString();

                            // 使用 Update 覆寫資料
                            g_AQL.UpdateAQLData(new QC_PlanModel()
                            {
                                Part_Id = g_sPartId,
                                Process_Id = g_sProcessId,
                                Sampling_Id = SamplingID,
                                User_Id = g_sUserID
                            });

                            g_AQL.ShowListView(g_sPartId, LbProcess4.Text.Trim(), ref LvAQL);
                            Show_Preview();
                        }
                    }
                }
            }
            else
            {
                string s = @"
SELECT
    COUNT(*) COUNT
FROM
    SAJET.SYS_PART_QC_PLAN
WHERE
    PART_ID = :PART_ID
    AND PROCESS_ID = :PROCESS_ID
";
                var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.Number, "PART_ID", g_sPartId },
                    new object[] { ParameterDirection.Input, OracleType.Number, "PROCESS_ID", g_sProcessId },
                };

                var ds = ClientUtils.ExecuteSQL(s, p.ToArray());
                if (ds.Tables[0].Rows[0]["COUNT"].ToString() != "0")
                {
                    qcRow = int.Parse(ds.Tables[0].Rows[0]["COUNT"].ToString());
                }

                string SamplingID;

                s = @"
SELECT DISTINCT
    A.SAMPLING_ID,
    B.SAMPLING_TYPE,
    B.SAMPLING_DESC
FROM
    SAJET.SYS_QC_SAMPLING_PLAN_DETAIL   A,
    SAJET.SYS_QC_SAMPLING_PLAN          B
WHERE
    A.SAMPLING_ID = B.SAMPLING_ID
    AND A.SAMPLING_LEVEL = '0'
    AND B.ENABLED = 'Y'
";
                var h = new List<string>
                {
                    "SAMPLING_ID",
                };

                using (var f = new SajetFilter.FFilter(sqlCommand: s, hiddenColumns: h))
                {
                    f.Text = SajetCommon.SetLanguage("Sampling plan");

                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        var result = f.ResultSets[0];

                        SamplingID = result["SAMPLING_ID"].ToString();

                        if (qcRow > 0)
                        {
                            // 使用 Update 覆寫資料
                            g_AQL.UpdateAQLData(new QC_PlanModel()
                            {
                                Part_Id = g_sPartId,
                                Process_Id = g_sProcessId,
                                Sampling_Id = SamplingID,
                                User_Id = g_sUserID
                            });
                        }
                        else
                        {
                            // 使用 Insert 新增資料
                            g_AQL.InsertAQLData(new QC_PlanModel()
                            {
                                Part_Id = g_sPartId,
                                Process_Id = g_sProcessId,
                                Sampling_Id = SamplingID,
                                User_Id = g_sUserID
                            });
                        }

                        g_AQL.ShowListView(g_sPartId, LbProcess4.Text.Trim(), ref LvAQL);

                        Show_Preview();
                    }
                }
            }
        }

        /// <summary>
        /// 修改料號綁定 AQL 規則 (Isaac 2021/01/11)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAQLModify_Click(object sender, EventArgs e)
        {
            if (!chkData()) return;

            if (LvAQL.Items.Count == 0) return;

            if (!bLVAQLCollectionSelected || LvAQL.SelectedIndices.Count == 0)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Select AQL Item."), 0);
                return;
            }

            string SamplingID = string.Empty;

            string sSQL = $@"
SELECT DISTINCT
    A.SAMPLING_ID,
    B.SAMPLING_TYPE,
    B.SAMPLING_DESC
FROM
    SAJET.SYS_QC_SAMPLING_PLAN_DETAIL   A,
    SAJET.SYS_QC_SAMPLING_PLAN          B
WHERE
    A.SAMPLING_ID = B.SAMPLING_ID
    AND A.SAMPLING_LEVEL = '0'
    AND B.ENABLED = 'Y'
";
            var h = new List<string>
            {
                "SAMPLING_ID",
            };

            using (var f = new SajetFilter.FFilter(sqlCommand: sSQL, hiddenColumns: h))
            {
                f.Text = SajetCommon.SetLanguage("Sampling plan");

                if (f.ShowDialog() == DialogResult.OK)
                {
                    var result = f.ResultSets[0];

                    SamplingID = result["SAMPLING_ID"].ToString();

                    // 使用 Update 覆寫資料
                    g_AQL.UpdateAQLData(new QC_PlanModel()
                    {
                        Part_Id = g_sPartId,
                        Process_Id = g_sProcessId,
                        Sampling_Id = SamplingID,
                        User_Id = g_sUserID
                    });

                    g_AQL.ShowListView(g_sPartId, LbProcess4.Text.Trim(), ref LvAQL);
                    Show_Preview();
                }
            }
        }

        /// <summary>
        /// 啟用 / 停用料號綁定 AQL 規則
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAQLEnable_Click(object sender, EventArgs e)
        {
            string message;

            // (Isaac 2021/01/11)
            if (!chkData()) return;

            if (LvAQL.Items.Count == 0) return;

            if (!bLVAQLCollectionSelected || LvAQL.SelectedIndices.Count == 0
                || this_process == null)
            {
                message = SajetCommon.SetLanguage("Please Select AQL Item.");

                SajetCommon.Show_Message(message, 0);
                return;
            }

            #region 啟用 / 停用

            // 抽驗計畫為啟用狀態時，訊問是否停用；
            // 停用的抽驗計畫則詢問是否啟用。
            string s_enable_value = g_AQL.GetAQLEnableValue(this_process);

            if (s_enable_value == "Y")
            {
                message = SajetCommon.SetLanguage(MessageEnum.DisableAQL.ToString());
            }
            else if (s_enable_value == "N")
            {
                message = SajetCommon.SetLanguage(MessageEnum.EnableAQL.ToString());
            }
            else
            {
                message = SajetCommon.SetLanguage(MessageEnum.UnknownAQLStatus.ToString());

                SajetCommon.Show_Message(message, 0);

                return;
            }

            #endregion

            if (SajetCommon.Show_Message(message, 2) == DialogResult.Yes)
            {
                string s = @"
SELECT
    PROCESS_ID
FROM
    SAJET.SYS_PROCESS
WHERE
    TRIM(PROCESS_NAME) = TRIM(:PROCESS_NAME)
";
                var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_NAME", LbProcess4.Text.Trim() },
                };

                var ds = ClientUtils.ExecuteSQL(s, p.ToArray());

                if (ds.Tables[0].Rows.Count > 0)
                {
                    g_sProcessId = ds.Tables[0].Rows[0]["PROCESS_ID"].ToString();
                }

                string SamplingID = string.Empty;

                s = @"
SELECT
    SAMPLING_ID
FROM
    SAJET.SYS_PART_QC_PLAN
WHERE
    PART_ID = :PART_ID
    AND PROCESS_ID = :PROCESS_ID
";
                p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.Number, "PART_ID", g_sPartId },
                    new object[] { ParameterDirection.Input, OracleType.Number, "PROCESS_ID", g_sProcessId },
                };

                SamplingID = ClientUtils.ExecuteSQL(s, p.ToArray())
                    .Tables[0].Rows[0]["SAMPLING_ID"].ToString();
                try
                {
                    var qc_model = new QC_PlanModel()
                    {
                        Part_Id = g_sPartId,
                        Process_Id = g_sProcessId,
                        Sampling_Id = SamplingID,
                        User_Id = g_sUserID
                    };

                    g_AQL.EnableAQLData(qc_model, s_enable_value);
                }
                catch (Exception)
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Delete AQL Item : ") + LvAQL.Items[0].SubItems[1].Text + SajetCommon.SetLanguage(" Error."), 0);
                    return;
                }

                g_AQL.ShowListView(g_sPartId, LbProcess4.Text.Trim(), ref LvAQL);
                Show_Preview();
            }
        }

        /// <summary>
        /// 新增 QC 資料 (Isaac 2021/01/07)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnQCCollectionAdd_Click(object sender, EventArgs e)
        {
            if (!chkData()) return;

            fQCCollection f = new fQCCollection
            {
                g_sPart = editPart.Text,
                g_sProcess = LbProcess3.Text.Trim(),
                g_sType = "Add",
                g_sItemType = "1", // Collection 1: 資料收集
                g_sUserID = g_sUserID,
                g_sPartId = g_sPartId,
                g_sProcessId = g_sProcessId
            };

            f.ShowDialog();

            g_QCCollection.ShowListView(g_sPartId, LbProcess3.Text.Trim(), ref LvQCCollection);
            Show_Preview();
        }

        /// <summary>
        /// 修改 QC 資料 (Isaac 2021/01/07)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnQCCollectionModify_Click(object sender, EventArgs e)
        {
            if (!chkData()) return;

            if (LvQCCollection.Items.Count == 0) return;

            if (!bLVQCCollectionSelected || LvQCCollection.SelectedIndices.Count == 0)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Select QC Item."), 0);
                return;
            }

            if (LvQCCollection.SelectedIndices.Count > 1)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Select One QC Item."), 0);
                return;
            }

            fQCCollection f = new fQCCollection
            {
                g_sPart = editPart.Text,
                g_sProcess = LbProcess3.Text.Trim(),
                g_sType = "Modify",
                g_sItemType = "1", // 1: 資料收集
                g_sUserID = g_sUserID,
                g_sPartId = g_sPartId,
                g_sProcessId = g_sProcessId
            };

            int iSelectIdx = LvQCCollection.SelectedItems[0].Index;
            f.g_sSeq = LvQCCollection.Items[iSelectIdx].Text;
            f.g_sName = LvQCCollection.Items[iSelectIdx].SubItems[1].Text;

            if (f.ShowDialog() == DialogResult.OK)
            {
                g_QCCollection.ShowListView(g_sPartId, LbProcess3.Text.Trim(), ref LvQCCollection);
                Show_Preview();
            }

            bLVQCCollectionSelected = false;
        }

        /// <summary>
        /// 刪除 QC 資料 (Isaac 2021/01/07)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnQCCollectionDelete_Click(object sender, EventArgs e)
        {
            if (!chkData()) return;

            if (LvQCCollection.Items.Count == 0) return;

            if (!bLVQCCollectionSelected || LvQCCollection.SelectedIndices.Count == 0)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Select QC Item."), 0);
                return;
            }

            if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Delete the QC Item?"), 2) == DialogResult.Yes)
            {
                for (int i = 0; i < LvQCCollection.SelectedIndices.Count; i++)
                {
                    int iSelectIdx = LvQCCollection.SelectedItems[i].Index;

                    if (!g_QCCollection.DeleteItem(g_sPartId, g_sProcessId, LvQCCollection.Items[iSelectIdx].Text, LvQCCollection.Items[iSelectIdx].SubItems[1].Text, "1"))
                    {
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("Delete QC Item : ") + LvQCCollection.Items[iSelectIdx].SubItems[1].Text + SajetCommon.SetLanguage(" Error."), 0);
                        return;
                    }
                }

                g_QCCollection.ShowListView(g_sPartId, LbProcess3.Text.Trim(), ref LvQCCollection);
                Show_Preview();
            }
        }

        private void BtnQCOption_Click(object sender, EventArgs e)
        {
            if (!chkData() || this_process == null) return;

            using (var f = new fQCOption(this_process))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    Show_QCOption(this_process);

                    Show_Preview();
                }
            }
        }

        private void BtnSNConditionAdd_Click(object sender, EventArgs e)
        {
            if (!chkData()) return;

            fCondition f = new fCondition
            {
                g_sPart = editPart.Text,
                g_sProcess = LbProcess1.Text.Trim(),
                g_sType = "Add",
                g_sItemType = "2", // Condition 0: 製程條件
                g_sUserID = g_sUserID,
                g_sPartId = g_sPartId,
                g_sProcessId = g_sProcessId
            };

            f.ShowDialog();

            show_LVSNCondition();

        }

        private void BtnSNConditionModify_Click(object sender, EventArgs e)
        {
            if (!chkData()) return;

            if (LvSNCondition.Items.Count == 0) return;

            if (!bLVSNConditionSelected || LvSNCondition.SelectedIndices.Count == 0)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Select a Item."), 0);
                return;
            }

            if (LvSNCondition.SelectedIndices.Count > 1)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Select One Item."), 0);
                return;
            }

            fCondition f = new fCondition
            {
                g_sPart = editPart.Text,
                g_sProcess = LbProcess1.Text.Trim(),
                g_sType = "Modify",
                g_sItemType = "2", // 2: 元件製程條件
                g_sUserID = g_sUserID,
                g_sPartId = g_sPartId,
                g_sProcessId = g_sProcessId
            };

            int iSelectIdx = LvSNCondition.SelectedItems[0].Index;
            f.g_sSeq = LvSNCondition.Items[iSelectIdx].Text;
            f.g_sName = LvSNCondition.Items[iSelectIdx].SubItems[1].Text;

            if (f.ShowDialog() == DialogResult.OK)
            {
                show_LVSNCondition();
            }

            bLVSNConditionSelected = false;
        }

        private void BtnSNConditionDelete_Click(object sender, EventArgs e)
        {
            if (!chkData()) return;

            if (LvSNCondition.Items.Count == 0) return;

            if (!bLVSNConditionSelected || LvSNCondition.SelectedIndices.Count == 0)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Select a Item."), 0);
                return;
            }

            if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Delete the Item?"), 2) == DialogResult.Yes)
            {
                for (int i = 0; i < LvSNCondition.SelectedIndices.Count; i++)
                {
                    int iSelectIdx = LvSNCondition.SelectedItems[i].Index;

                    if (!deleteItem(LvSNCondition.Items[iSelectIdx].Text, LvSNCondition.Items[iSelectIdx].SubItems[1].Text, "2"))
                    {
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("Delete Item : ") + LvCondition.Items[iSelectIdx].SubItems[1].Text + SajetCommon.SetLanguage(" Error."), 0);
                        return;
                    }
                }

                show_LVSNCondition();
            }
        }

        private void BtnSNCollectionAdd_Click(object sender, EventArgs e)
        {
            if (!chkData()) return;

            fCollection f = new fCollection
            {
                g_sPart = editPart.Text,
                g_sProcess = LbProcess1.Text.Trim(),
                g_sType = "Add",
                g_sItemType = "3", // Condition 0: 製程條件
                g_sUserID = g_sUserID,
                g_sPartId = g_sPartId,
                g_sProcessId = g_sProcessId
            };

            f.ShowDialog();

            show_LVSNCollection();
        }

        private void BtnSNCollectionModify_Click(object sender, EventArgs e)
        {
            if (!chkData()) return;

            if (LvSNCollection.Items.Count == 0) return;

            if (!bLVSNCollectionSelected || LvSNCollection.SelectedIndices.Count == 0)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Select a Item."), 0);
                return;
            }

            if (LvSNCollection.SelectedIndices.Count > 1)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Select One Item."), 0);
                return;
            }

            fCollection f = new fCollection
            {
                g_sPart = editPart.Text,
                g_sProcess = LbProcess1.Text.Trim(),
                g_sType = "Modify",
                g_sItemType = "3", // 2: 元件製程條件
                g_sUserID = g_sUserID,
                g_sPartId = g_sPartId,
                g_sProcessId = g_sProcessId
            };

            int iSelectIdx = LvSNCollection.SelectedItems[0].Index;
            f.g_sSeq = LvSNCollection.Items[iSelectIdx].Text;
            f.g_sName = LvSNCollection.Items[iSelectIdx].SubItems[1].Text;

            if (f.ShowDialog() == DialogResult.OK)
            {
                show_LVSNCollection();
            }

            bLVSNCollectionSelected = false;
        }

        private void BtnSNCollectionDelete_Click(object sender, EventArgs e)
        {
            if (!chkData()) return;

            if (LvSNCollection.Items.Count == 0) return;

            if (!bLVSNCollectionSelected || LvSNCollection.SelectedIndices.Count == 0)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Select a Item."), 0);
                return;
            }

            if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Delete the Item?"), 2) == DialogResult.Yes)
            {
                for (int i = 0; i < LvSNCollection.SelectedIndices.Count; i++)
                {
                    int iSelectIdx = LvSNCollection.SelectedItems[i].Index;

                    if (!deleteItem(LvSNCollection.Items[iSelectIdx].Text, LvSNCollection.Items[iSelectIdx].SubItems[1].Text, "3"))
                    {
                        SajetCommon.Show_Message(SajetCommon.SetLanguage("Delete Item : ") + LvCondition.Items[iSelectIdx].SubItems[1].Text + SajetCommon.SetLanguage(" Error."), 0);
                        return;
                    }
                }

                show_LVSNCollection();
            }
        }

        private void BtnKeypartSave_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < DgvMaterial.Rows.Count; i++)
                {
                    for (int j = 2; j < DgvMaterial.Columns.Count; j++)
                    {
                        if (!DgvMaterial.Columns[j].ReadOnly)
                        {
                            if (DgvMaterial.Rows[i].Cells[1].Value.ToString() == "True" && DgvMaterial.Rows[i].Cells[j].Value == null)
                            {
                                SajetCommon.Show_Message(DgvMaterial.Rows[i].Cells[0].Value.ToString() + " : " + SajetCommon.SetLanguage("Please Input Excel Location."), 0);
                                return;
                            }

                            if (DgvMaterial.Rows[i].Cells[1].Value.ToString() == "True" && !IsNumeric(1, DgvMaterial.Rows[i].Cells[j].Value.ToString()))
                            {
                                SajetCommon.Show_Message(DgvMaterial.Rows[i].Cells[0].Value.ToString() + " : " + SajetCommon.SetLanguage("Please Input Excel Location is Number."), 0);
                                return;
                            }
                        }
                    }
                }

                for (int i = 0; i < DgvMaterial.Rows.Count; i++)
                {
                    string s = @"
SELECT
    PART_ID,
    ITEM_ID
FROM
    SAJET.SYS_RC_PROCESS_PARAM_PART
WHERE
    PART_ID = :PART_ID
    AND PROCESS_ID = :PROCESS_ID
    AND ITEM_NAME = :ITEM_NAME
";
                    var p = new List<object[]>
                    {
                        new object[] { ParameterDirection.Input, OracleType.Number, "PART_ID", g_sPartId },
                        new object[] { ParameterDirection.Input, OracleType.Number, "PROCESS_ID", OtSrv.GetProcessID(LbProcess7.Text.Trim()) },
                        new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_NAME", DgvMaterial.Rows[i].Cells[0].Value.ToString().Trim() },
                    };
                    DataSet dsTemp = ClientUtils.ExecuteSQL(s, p.ToArray());

                    if (DgvMaterial.Rows[i].Cells[1].Value.ToString() == "True")
                        g_sPrint = "Y";
                    else
                        g_sPrint = "N";

                    if (dsTemp.Tables[0].Rows.Count == 0)
                    {
                        string ItemId = SajetCommon.GetMaxID("SAJET.SYS_RC_PROCESS_PARAM_PART", "ITEM_ID", 8);

                        s = @"
INSERT INTO SAJET.SYS_RC_PROCESS_PARAM_PART (
    PART_ID,
    PROCESS_ID,
    ITEM_ID,
    ITEM_NAME,
    ITEM_PHASE,
    ITEM_TYPE,
    ITEM_SEQ,
    VALUE_TYPE,
    INPUT_TYPE,
    VALUE_DEFAULT,
    VALUE_LIST,
    NECESSARY,
    CONVERT_TYPE,
    UPDATE_USERID,
    ENABLED,
    PRINT,
    COLUMN_ITEM,
    ROW_ITEM
) VALUES (
    :PART_ID,
    :PROCESS_ID,
    :ITEM_ID,
    :ITEM_NAME,
    :ITEM_PHASE,
    :ITEM_TYPE,
    :ITEM_SEQ,
    :VALUE_TYPE,
    :INPUT_TYPE,
    :VALUE_DEFAULT,
    :VALUE_LIST,
    :NECESSARY,
    :CONVERT_TYPE,
    :UPDATE_USERID,
    :ENABLED,
    :PRINT,
    :COLUMN_ITEM,
    :ROW_ITEM
)";
                        p = new List<object[]>
                        {
                            new object[] { ParameterDirection.Input, OracleType.Number, "PART_ID", g_sPartId },
                            new object[] { ParameterDirection.Input, OracleType.Number, "PROCESS_ID", OtSrv.GetProcessID(LbProcess7.Text.Trim()) },
                            new object[] { ParameterDirection.Input, OracleType.Number, "ITEM_ID", ItemId },
                            new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_NAME", DgvMaterial.Rows[i].Cells[0].Value.ToString() },
                            new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_PHASE", "I" },
                            new object[] { ParameterDirection.Input, OracleType.Number, "ITEM_TYPE", 4 },
                            new object[] { ParameterDirection.Input, OracleType.Number, "ITEM_SEQ", 0 },
                            new object[] { ParameterDirection.Input, OracleType.VarChar, "VALUE_TYPE", "V" },
                            new object[] { ParameterDirection.Input, OracleType.VarChar, "INPUT_TYPE", "K" },
                            new object[] { ParameterDirection.Input, OracleType.VarChar, "VALUE_DEFAULT", "" },
                            new object[] { ParameterDirection.Input, OracleType.VarChar, "VALUE_LIST", "" },
                            new object[] { ParameterDirection.Input, OracleType.VarChar, "NECESSARY", "Y" },
                            new object[] { ParameterDirection.Input, OracleType.VarChar, "CONVERT_TYPE", "N" },
                            new object[] { ParameterDirection.Input, OracleType.VarChar, "ENABLED", "Y" },
                            new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", g_sUserID },
                            new object[] { ParameterDirection.Input, OracleType.VarChar, "PRINT", g_sPrint },
                            new object[] { ParameterDirection.Input, OracleType.VarChar, "COLUMN_ITEM", DgvMaterial.Rows[i].Cells[2].Value?.ToString() ?? "" },
                            new object[] { ParameterDirection.Input, OracleType.VarChar, "ROW_ITEM", DgvMaterial.Rows[i].Cells[3].Value?.ToString() ?? "" },
                        };

                        ClientUtils.ExecuteSQL(s, p.ToArray());
                    }
                    else
                    {
                        s = @"
UPDATE SAJET.SYS_RC_PROCESS_PARAM_PART
SET
    ITEM_NAME = :ITEM_NAME,
    UPDATE_USERID = :UPDATE_USERID,
    UPDATE_TIME = SYSDATE,
    PRINT = :PRINT,
    COLUMN_ITEM = :COLUMN_ITEM,
    ROW_ITEM = :ROW_ITEM
WHERE
    PART_ID = :PART_ID
    AND PROCESS_ID = :PROCESS_ID
    AND ITEM_ID = :ITEM_ID
";
                        p = new List<object[]>
                        {
                            new object[] { ParameterDirection.Input, OracleType.Number, "PART_ID", g_sPartId },
                            new object[] { ParameterDirection.Input, OracleType.Number, "PROCESS_ID", OtSrv.GetProcessID(LbProcess7.Text.Trim()) },
                            new object[] { ParameterDirection.Input, OracleType.Number, "ITEM_ID", dsTemp.Tables[0].Rows[0]["ITEM_ID"].ToString() },
                            new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_NAME", DgvMaterial.Rows[i].Cells[0].Value.ToString() },
                            new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", g_sUserID },
                            new object[] { ParameterDirection.Input, OracleType.VarChar, "PRINT", g_sPrint },
                            new object[] { ParameterDirection.Input, OracleType.VarChar, "COLUMN_ITEM", DgvMaterial.Rows[i].Cells[2].Value?.ToString() ?? "" },
                            new object[] { ParameterDirection.Input, OracleType.VarChar, "ROW_ITEM", DgvMaterial.Rows[i].Cells[3].Value?.ToString() ?? "" },
                        };

                        ClientUtils.ExecuteSQL(s, p.ToArray());
                    }
                }
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Save OK."), 1);
                show_LVMaterial();
            }
            catch (Exception)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Save Fail."), 0);
            }
        }

        private void BtnAlertAdd_Click(object sender, EventArgs e)
        {
            if (!chkData()) return;

            fAlert f = new fAlert
            {
                PartID = g_sPartId,
                ProcessID = g_sProcessId,
                Type = "Add"
            };
            f.ShowDialog();

            show_lvAlertCollection();
        }

        private void BtnAlertModify_Click(object sender, EventArgs e)
        {
            if (!chkData()) return;

            if (LvAlertCollection.Items.Count == 0) return;

            if (!blvAlertCollectionSelected || LvAlertCollection.SelectedIndices.Count == 0)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Select a Alert Item."), 0);
                return;
            }

            if (LvAlertCollection.SelectedIndices.Count > 1)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Select a Alert Item."), 0);
                return;
            }


            fAlert f = new fAlert
            {
                PartID = g_sPartId,
                ProcessID = g_sProcessId
            };

            int iSelectIdx = LvAlertCollection.SelectedItems[0].Index;
            f.ItemName = LvAlertCollection.Items[iSelectIdx].SubItems[1].Text;
            f.ItemSeq = LvAlertCollection.Items[iSelectIdx].SubItems[0].Text;
            f.Type = "Modify";

            if (f.ShowDialog() == DialogResult.OK)
            {
                show_lvAlertCollection();
            }

            blvAlertCollectionSelected = false;
        }

        private void BtnAlertDelete_Click(object sender, EventArgs e)
        {
            if (!chkData()) return;

            if (LvAlertCollection.Items.Count == 0) return;

            if (!blvAlertCollectionSelected || LvAlertCollection.SelectedIndices.Count == 0)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Select a Alert Item."), 0);
                return;
            }

            if (SajetCommon.Show_Message(SajetCommon.SetLanguage("Delete the Item?"), 2) == DialogResult.Yes)
            {
                for (int i = 0; i < LvAlertCollection.SelectedIndices.Count; i++)
                {
                    int iSelectIdx = LvAlertCollection.SelectedItems[i].Index;

                    string S = @"
DELETE FROM SAJET.SYS_RC_PROCESS_PART_ALERT T
WHERE
    T.PROCESS_ID = :PROCESS_ID
    AND T.PART_ID = ':PART_ID
    AND T.ITEM_NAME = :ITEM_NAME
";
                    var p = new List<object[]>
                    {
                        new object[] { ParameterDirection.Input, OracleType.Number, "PART_ID", g_sPartId },
                        new object[] { ParameterDirection.Input, OracleType.Number, "PROCESS_ID", g_sProcessId },
                        new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_NAME", LvAlertCollection.SelectedItems[i].SubItems[1].Text.Trim() },
                    };

                    ClientUtils.ExecuteSQL(S, p.ToArray());
                }

                show_lvAlertCollection();
            }
        }

        private void Tc_1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Show_Preview();
        }

        /// <summary>
        /// Process Condition 按右鍵的觸發事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LvCondition_MouseClick(object sender, MouseEventArgs e)
        {
            //// 禁止多選
            //LVCondition.MultiSelect = false;

            //// 鼠標右鍵
            //if (e.Button == MouseButtons.Right)
            //{
            //    Point p = new Point(e.X, e.Y);
            //    CMSCondition.Show(LVCondition, p);
            //}

            if (e.Button == MouseButtons.Right)
            {
                if (LvCondition.FocusedItem.Bounds.Contains(e.Location) == true)
                {
                    CmsCondition.Show(Cursor.Position);
                }
            }
        }

        private void LvCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            bLVConditionSelected = true;
        }

        private void LvCollection_SelectedIndexChanged(object sender, EventArgs e)
        {
            bLVCollectionSelected = true;
        }

        /// <summary>
        /// QC 資料列表選擇 (Isaac 2021/01/07)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LvQCCollection_SelectedIndexChanged(object sender, EventArgs e)
        {
            bLVQCCollectionSelected = true;
        }

        private void LvAQLCollection_SelectedIndexChanged(object sender, EventArgs e)
        {
            bLVAQLCollectionSelected = true;
        }

        private void LvAQLCollection_DoubleClick(object sender, EventArgs e)
        {
            if (!bLVAQLCollectionSelected || LvAQL.Items.Count <= 0) return;

            string SamplingType = LvAQL.SelectedItems[0].SubItems[0].Text;

            OtSrv.ShowSamplingPlanDetail(SamplingType: SamplingType);
        }

        private void LvSNCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            bLVSNConditionSelected = true;
        }

        private void LvSNCollection_SelectedIndexChanged(object sender, EventArgs e)
        {
            bLVSNCollectionSelected = true;
        }

        private void LvAlertCollection_SelectedIndexChanged(object sender, EventArgs e)
        {
            blvAlertCollectionSelected = true;
        }

        // 輸入列印資料觸發的事件，檢查列印資料
        private void DgvMaterial_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // 當此行是checkbox判斷是否勾選再設定列印行列Readonly，另外輸入完列印行列檢查是否為數字
            if (e.ColumnIndex == 1)
            {
                if (DgvMaterial.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "True")
                {
                    DgvMaterial.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].ReadOnly = false;
                    DgvMaterial.Rows[e.RowIndex].Cells[e.ColumnIndex + 2].ReadOnly = false;
                }
                else
                {
                    DgvMaterial.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].ReadOnly = true;
                    DgvMaterial.Rows[e.RowIndex].Cells[e.ColumnIndex + 2].ReadOnly = true;
                    DgvMaterial.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value = string.Empty;
                    DgvMaterial.Rows[e.RowIndex].Cells[e.ColumnIndex + 2].Value = string.Empty;
                }
            }
            else
            {
                if (DgvMaterial.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
                {
                    string sMsg = SajetCommon.SetLanguage("Please Input Excel Location.");
                    SajetCommon.Show_Message(sMsg, 1);
                    return;
                }
                else
                {
                    if (DgvMaterial.Rows[e.RowIndex].Cells[1].Value.ToString() == "True" && !IsNumber(DgvMaterial.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()))
                    {
                        SajetCommon.Show_Message(DgvMaterial.Rows[e.RowIndex].Cells[0].Value.ToString() + " : " + SajetCommon.SetLanguage("Please Input Excel Location is Number."), 0);
                        return;
                    }
                }
            }
        }

        private void DgvMaterial_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (DgvMaterial.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn && e.RowIndex != -1)
            {
                SendKeys.Send("{F4}");
            }
        }

        private void DgvPreview_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DgvPreview.CurrentRow == null ||
                DgvPreview.Rows.Count <= 0 ||
                e.RowIndex < 0 ||
                e.ColumnIndex < 0)
            {
                return;
            }

            Cursor = Cursors.WaitCursor;

            var row = DgvPreview.CurrentRow;

            // Isaac QC Collection 2021/01/07
            if (g_QCCollection.Check_Privilege(g_sUserID, "QC_AQL"))
            {
                if (Tc_1.SelectedTab == TpQCCollection)
                {
                    using (fQCCollection f = new fQCCollection
                    {
                        g_sPart = editPart.Text,
                        g_sProcess = row.Cells["PROCESS_NAME"].Value.ToString().Trim(),
                        g_sType = "Modify",
                        g_sItemType = "1", // 1: 資料收集
                        g_sUserID = g_sUserID,
                        g_sPartId = g_sPartId,
                        g_sProcessId = row.Cells["PROCESS_ID"].Value.ToString(),
                        g_sSeq = row.Cells["ITEM_SEQ"].Value.ToString(),
                        g_sName = row.Cells["ITEM_NAME"].Value.ToString(),
                    })
                    {
                        if (f.ShowDialog() == DialogResult.OK)
                        {
                            g_QCCollection.ShowListView(g_sPartId, LbProcess3.Text.Trim(), ref LvQCCollection);
                            Show_Preview();
                        }
                    }
                }
                // Isaac AQL 2021/01/11
                else
                if (Tc_1.SelectedTab == TpAQL)
                {
                    string SamplingID = string.Empty;

                    string sSQL = $@"
SELECT DISTINCT
    A.SAMPLING_ID,
    B.SAMPLING_TYPE,
    B.SAMPLING_DESC
FROM
    SAJET.SYS_QC_SAMPLING_PLAN_DETAIL   A,
    SAJET.SYS_QC_SAMPLING_PLAN          B
WHERE
    A.SAMPLING_ID = B.SAMPLING_ID
    AND A.SAMPLING_LEVEL = '0'
    AND B.ENABLED = 'Y'
";
                    var h = new List<string>
                    {
                        "SAMPLING_ID",
                    };

                    using (var f = new SajetFilter.FFilter(sqlCommand: sSQL, hiddenColumns: h))
                    {
                        f.Text = SajetCommon.SetLanguage("Sampling plan");

                        if (f.ShowDialog() == DialogResult.OK)
                        {
                            var result = f.ResultSets[0];

                            SamplingID = result["SAMPLING_ID"].ToString();

                            // 使用 Update 覆寫資料
                            g_AQL.UpdateAQLData(new QC_PlanModel()
                            {
                                Part_Id = g_sPartId,
                                Process_Id = row.Cells["PROCESS_ID"].Value.ToString(),
                                Sampling_Id = SamplingID,
                                User_Id = g_sUserID
                            });

                            g_AQL.ShowListView(g_sPartId, LbProcess4.Text.Trim(), ref LvAQL);
                            Show_Preview();
                        }
                    }
                }
                else
                if (Tc_1.SelectedTab == TpQCOption)
                {
                    var process_info = new ProcessViewModel
                    {
                        PART_ID = row.Cells["PART_ID"].Value.ToString().Trim(),
                        PART_NO = editPart.Text.Trim(),
                        ROUTE_ID = row.Cells["ROUTE_ID"].Value.ToString().Trim(),
                        PROCESS_ID = row.Cells["PROCESS_ID"].Value.ToString().Trim(),
                        PROCESS_CODE = row.Cells["PROCESS_CODE"].Value.ToString().Trim(),
                        PROCESS_NAME = row.Cells["PROCESS_NAME"].Value.ToString().Trim(),
                        NODE_ID = row.Cells["NODE_ID"].Value.ToString().Trim(),
                    };

                    using (var f = new fQCOption(process_info))
                    {
                        if (f.ShowDialog() == DialogResult.OK)
                        {
                            Show_QCOption(this_process);

                            Show_Preview();
                        }
                    }
                }
            }

            if (Tc_1.SelectedTab == TpCollection)
            {
                using (fCollection f = new fCollection
                {
                    g_sPart = editPart.Text,
                    g_sProcess = row.Cells["PROCESS_NAME"].Value.ToString().Trim(),
                    g_sType = "Modify",
                    g_sItemType = "1", // 1: 資料收集
                    g_sUserID = g_sUserID,
                    g_sPartId = g_sPartId,
                    g_sProcessId = row.Cells["PROCESS_ID"].Value.ToString(),
                    g_sSeq = row.Cells["ITEM_SEQ"].Value.ToString(),
                    g_sName = row.Cells["ITEM_NAME"].Value.ToString(),
                })
                {
                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        show_LVCondition();
                        Show_Preview();
                    }
                }
            }
            else if (Tc_1.SelectedTab == TpCondition)
            {
                using (fCondition f = new fCondition
                {
                    g_sPart = editPart.Text,
                    g_sProcess = row.Cells["PROCESS_NAME"].Value.ToString().Trim(),
                    g_sType = "Modify",
                    g_sItemType = "0", // 0: 製程條件
                    g_sUserID = g_sUserID,
                    g_sPartId = g_sPartId,
                    g_sProcessId = row.Cells["PROCESS_ID"].Value.ToString(),
                    g_sSeq = row.Cells["ITEM_SEQ"].Value.ToString(),
                    g_sName = row.Cells["ITEM_NAME"].Value.ToString(),
                })
                {
                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        show_LVCondition();
                        Show_Preview();
                    }
                }
            }

            Cursor = Cursors.Default;
        }

        public void Check_privilege()
        {
            string sPrivilege = ClientUtils.GetPrivilege(g_sUserID, g_sFunction, g_sProgram).ToString();

            g_iPrivilege = ClientUtils.GetPrivilege(g_sUserID, g_sFunction, g_sProgram);
        }

        public void ShowProcess()
        {
            if (!CheckPart())
                return;

            //Show Process
            TvProcess.Nodes.Clear();
            string sStage = "";
            int iCnt = 0;

            string s = @"
WITH
/* 指定料號的基本資訊 */
 PART_INFO AS (
    SELECT
        PART_ID,
        ROUTE_ID
    FROM
        SAJET.SYS_PART
    WHERE
        PART_NO = :PART_NO
),
/* 預設生產途程的製程按照順序排列 */
 ROUTE_NODES AS (
    SELECT
        ROWNUM IDX,
        A.ROUTE_ID,
        A.NODE_CONTENT,
        A.NODE_ID,
        B.PART_ID
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
    B.ROUTE_ID,
    NVL(TRIM(A.PROCESS_CODE), '0') PROCESS_CODE,
    TRIM(A.PROCESS_NAME) PROCESS_NAME,
    A.PROCESS_ID,
    C.NODE_ID,
    C.PART_ID
FROM
    SAJET.SYS_PROCESS    A,
    SAJET.SYS_RC_ROUTE   B,
    ROUTE_NODES          C
WHERE
    B.ROUTE_ID = C.ROUTE_ID
    AND TO_CHAR(A.PROCESS_ID) = C.NODE_CONTENT
    AND A.ENABLED = 'Y'
    AND B.ENABLED = 'Y'
ORDER BY
    C.IDX
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_NO", editPart.Text.Trim() },
            };

            DataSet DS = ClientUtils.ExecuteSQL(s, p.ToArray());

            for (int i = 0; i <= DS.Tables[0].Rows.Count - 1; i++)
            {
                if (sStage != DS.Tables[0].Rows[i]["ROUTE_NAME"].ToString())
                {
                    sStage = DS.Tables[0].Rows[i]["ROUTE_NAME"].ToString();
                    TreeNode Node1 = new TreeNode
                    {
                        Text = sStage,
                        Tag = DS.Tables[0].Rows[i]["ROUTE_ID"].ToString()
                    };
                    Node1.Name = Node1.Text;
                    Node1.ImageIndex = 1;
                    TvProcess.Nodes.Add(Node1);
                    iCnt += 1;
                }

                TreeNode NodeProcess = new TreeNode
                {
                    Text = DS.Tables[0].Rows[i]["PROCESS_NAME"].ToString().Trim(),
                    Tag = new ProcessViewModel
                    {
                        PART_ID = DS.Tables[0].Rows[i]["PART_ID"].ToString().Trim(),
                        PART_NO = editPart.Text.Trim(),
                        ROUTE_ID = DS.Tables[0].Rows[i]["ROUTE_ID"].ToString().Trim(),
                        PROCESS_ID = DS.Tables[0].Rows[i]["PROCESS_ID"].ToString().Trim(),
                        PROCESS_CODE = DS.Tables[0].Rows[i]["PROCESS_CODE"].ToString().Trim(),
                        PROCESS_NAME = DS.Tables[0].Rows[i]["PROCESS_NAME"].ToString().Trim(),
                        NODE_ID = DS.Tables[0].Rows[i]["NODE_ID"].ToString().Trim(),
                    }
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

        private void show_lvAlertCollection()
        {
            LvAlertCollection.Clear();

            try
            {
                string s = @"
SELECT
    A.ITEM_SEQ,
    A.ITEM_NAME,
    A.ITEM_PHASE,
    A.ALERT_TYPE,
    A.ITEM_TITLE,
    A.ITEM_CONTENT,
    D.EMP_NO,
    A.UPDATE_TIME
FROM
    SAJET.SYS_RC_PROCESS_PART_ALERT   A,
    SAJET.SYS_PART                    B,
    SAJET.SYS_PROCESS                 C,
    SAJET.SYS_EMP                     D
WHERE
    A.PART_ID = B.PART_ID
    AND A.PROCESS_ID = C.PROCESS_ID
    AND A.UPDATE_USERID = D.EMP_ID
    AND PART_NO = :PART_NO
    AND TRIM(PROCESS_NAME) = TRIM(:PROCESS_NAME)
ORDER BY
    A.ITEM_SEQ,
    A.ITEM_NAME
";
                var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_NO", editPart.Text.Trim() },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_NAME", LbProcess7.Text.Trim() },
                };

                DataSet dsTemp = ClientUtils.ExecuteSQL(s, p.ToArray());

                LvAlertCollection.Sorting = SortOrder.None;

                for (int i = 0; i < dsTemp.Tables[0].Columns.Count; i++)
                {
                    LvAlertCollection.Columns.Add(SajetCommon.SetLanguage(dsTemp.Tables[0].Columns[i].Caption));
                }

                for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                {
                    LvAlertCollection.Items.Add(dsTemp.Tables[0].Rows[i][0].ToString());

                    for (int j = 1; j < dsTemp.Tables[0].Columns.Count; j++)
                    {
                        if (j == 2)
                            LvAlertCollection.Items[i].SubItems.Add(dataConvert(j, dsTemp.Tables[0].Rows[i][j].ToString()));
                        else if (j == 3)
                        {
                            switch (dsTemp.Tables[0].Rows[i][j].ToString())
                            {
                                case "I":
                                    LvAlertCollection.Items[i].SubItems.Add(SajetCommon.SetLanguage("Info"));
                                    break;
                                case "W":
                                    LvAlertCollection.Items[i].SubItems.Add(SajetCommon.SetLanguage("Warn"));
                                    break;
                                case "E":
                                    LvAlertCollection.Items[i].SubItems.Add(SajetCommon.SetLanguage("Error"));
                                    break;
                                default:
                                    LvAlertCollection.Items[i].SubItems.Add(dsTemp.Tables[0].Rows[i][j].ToString());
                                    break;
                            }
                        }
                        else
                            LvAlertCollection.Items[i].SubItems.Add(dsTemp.Tables[0].Rows[i][j].ToString());
                    }

                    LvAlertCollection.Items[i].ImageIndex = 0;
                }

                for (int i = 0; i < dsTemp.Tables[0].Columns.Count; i++)
                    LvAlertCollection.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            catch
            {
                throw;
            }
        }

        private void show_LVCondition()
        {
            LvCondition.Clear();

            try
            {
                string s = @"
SELECT
    A.ITEM_SEQ,
    A.ITEM_NAME,
    A.ITEM_PHASE,
    A.VALUE_TYPE,
    A.INPUT_TYPE,
    A.CONVERT_TYPE,
    A.NECESSARY,
    A.VALUE_DEFAULT,
    A.PRINT,
    A.COLUMN_ITEM,
    A.ROW_ITEM,
    E.UNIT_NO,
    D.EMP_NO,
    A.UPDATE_TIME
FROM
    SAJET.SYS_RC_PROCESS_PARAM_PART   A,
    SAJET.SYS_PROCESS                 C,
    SAJET.SYS_EMP                     D,
    SAJET.SYS_UNIT                    E
WHERE
    A.PART_ID = :PART_ID
    AND A.PROCESS_ID = C.PROCESS_ID
    AND A.UPDATE_USERID = D.EMP_ID
    AND ITEM_TYPE = 0
    AND A.UNIT_ID = E.UNIT_ID (+)
    AND TRIM(PROCESS_NAME) = TRIM(:PROCESS_NAME)
ORDER BY
    A.ITEM_SEQ,
    A.ITEM_NAME
";
                var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.Number, "PART_ID", g_sPartId },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_NAME", LbProcess1.Text.Trim() },
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

        private void show_LVCollection()
        {
            LvCollection.Clear();

            try
            {
                string s = @"
SELECT
    A.ITEM_SEQ,
    A.ITEM_NAME,
    A.ITEM_PHASE,
    A.VALUE_TYPE,
    A.INPUT_TYPE,
    A.CONVERT_TYPE,
    A.NECESSARY,
    A.VALUE_DEFAULT ""Input value"",
    A.VALUE_LIST,
    A.PRINT,
    A.COLUMN_ITEM,
    A.ROW_ITEM,
    D.EMP_NO,
    A.UPDATE_TIME
FROM
    SAJET.SYS_RC_PROCESS_PARAM_PART   A,
    SAJET.SYS_PROCESS                 C,
    SAJET.SYS_EMP                     D,
    SAJET.SYS_UNIT                    E
WHERE
    A.PART_ID = :PART_ID
    AND A.PROCESS_ID = C.PROCESS_ID
    AND A.UPDATE_USERID = D.EMP_ID
    AND ITEM_TYPE = 1
    AND A.UNIT_ID = E.UNIT_ID (+)
    AND TRIM(PROCESS_NAME) = TRIM(:PROCESS_NAME)
ORDER BY
    A.ITEM_SEQ,
    A.ITEM_NAME
";
                var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.Number, "PART_ID", g_sPartId },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_NAME", LbProcess1.Text.Trim() },
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

                for (int i = 0; i < dsTemp.Tables[0].Columns.Count; i++)
                    LvCollection.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void show_LVSNCondition()
        {
            LvSNCondition.Clear();

            try
            {
                string s = @"
SELECT
    A.ITEM_SEQ,
    A.ITEM_NAME,
    A.ITEM_PHASE,
    A.VALUE_TYPE,
    A.INPUT_TYPE,
    A.CONVERT_TYPE,
    A.NECESSARY,
    A.VALUE_DEFAULT,
    D.EMP_NO,
    A.UPDATE_TIME
FROM
    SAJET.SYS_RC_PROCESS_PARAM_PART   A,
    SAJET.SYS_PROCESS                 C,
    SAJET.SYS_EMP                     D
WHERE
    A.PART_ID = :PART_ID
    AND A.PROCESS_ID = C.PROCESS_ID
    AND A.UPDATE_USERID = D.EMP_ID
    AND ITEM_TYPE = 2
    AND TRIM(PROCESS_NAME) = TRIM(:PROCESS_NAME)
ORDER BY
    A.ITEM_SEQ,
    A.ITEM_NAME
";
                var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.Number, "PART_ID", g_sPartId },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_NAME", LbProcess5.Text.Trim() },
                };

                DataSet dsTemp = ClientUtils.ExecuteSQL(s, p.ToArray());

                LvSNCondition.Sorting = SortOrder.None;

                for (int i = 0; i < dsTemp.Tables[0].Columns.Count; i++)
                {
                    LvSNCondition.Columns.Add(SajetCommon.SetLanguage(dsTemp.Tables[0].Columns[i].Caption));
                }

                for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                {
                    LvSNCondition.Items.Add(dsTemp.Tables[0].Rows[i][0].ToString());

                    for (int j = 1; j < dsTemp.Tables[0].Columns.Count; j++)
                    {
                        if (j > 1)
                            LvSNCondition.Items[i].SubItems.Add(dataConvert(j, dsTemp.Tables[0].Rows[i][j].ToString()));
                        else
                            LvSNCondition.Items[i].SubItems.Add(dsTemp.Tables[0].Rows[i][j].ToString());
                    }

                    LvSNCondition.Items[i].ImageIndex = 0;
                }

                for (int i = 0; i < dsTemp.Tables[0].Columns.Count; i++)
                    LvSNCondition.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void show_LVSNCollection()
        {
            LvSNCollection.Clear();

            try
            {
                string s = @"
SELECT
    A.ITEM_SEQ,
    A.ITEM_NAME,
    A.ITEM_PHASE,
    A.VALUE_TYPE,
    A.INPUT_TYPE,
    A.CONVERT_TYPE,
    A.NECESSARY,
    A.VALUE_DEFAULT,
    A.VALUE_LIST,
    D.EMP_NO,
    A.UPDATE_TIME
FROM
    SAJET.SYS_RC_PROCESS_PARAM_PART   A,
    SAJET.SYS_PROCESS                 C,
    SAJET.SYS_EMP                     D
WHERE
    A.PART_ID = :PART_ID
    AND A.PROCESS_ID = C.PROCESS_ID
    AND A.UPDATE_USERID = D.EMP_ID
    AND ITEM_TYPE = 3
    AND TRIM(PROCESS_NAME) = TRIM(:PROCESS_NAME)
ORDER BY
    A.ITEM_SEQ,
    A.ITEM_NAME
";
                var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.Number, "PART_ID", g_sPartId },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_NAME", LbProcess6.Text.Trim() },
                };

                DataSet dsTemp = ClientUtils.ExecuteSQL(s, p.ToArray());

                LvSNCollection.Sorting = SortOrder.None;

                for (int i = 0; i < dsTemp.Tables[0].Columns.Count; i++)
                {
                    LvSNCollection.Columns.Add(SajetCommon.SetLanguage(dsTemp.Tables[0].Columns[i].Caption));
                }

                for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                {
                    LvSNCollection.Items.Add(dsTemp.Tables[0].Rows[i][0].ToString());

                    for (int j = 1; j < dsTemp.Tables[0].Columns.Count; j++)
                    {
                        if (j > 1)
                            LvSNCollection.Items[i].SubItems.Add(dataConvert(j, dsTemp.Tables[0].Rows[i][j].ToString()));
                        else
                            LvSNCollection.Items[i].SubItems.Add(dsTemp.Tables[0].Rows[i][j].ToString());
                    }

                    LvSNCollection.Items[i].ImageIndex = 0;
                }

                for (int i = 0; i < dsTemp.Tables[0].Columns.Count; i++)
                    LvSNCollection.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void show_LVMaterial()
        {
            //LVMaterial.Clear();
            DgvMaterial.Columns.Clear();
            DgvMaterial.Rows.Clear();

            try
            {
                //, A.ENABLED
                string s = @"
SELECT
    E.PART_NO,
    A.PRINT,
    A.COLUMN_ITEM,
    A.ROW_ITEM,
    G.EMP_NO,
    A.UPDATE_TIME
FROM
    SAJET.SYS_RC_PROCESS_PARAM_PART   A,
    SAJET.SYS_BOM_INFO                B,
    SAJET.SYS_BOM                     C,
    SAJET.SYS_PART                    E,
    SAJET.SYS_PROCESS                 F,
    SAJET.SYS_EMP                     G
WHERE
    1 = 1
    AND B.PART_ID = A.PART_ID
    AND B.BOM_ID = C.BOM_ID
    AND E.PART_ID = C.ITEM_PART_ID
    AND F.PROCESS_ID = C.PROCESS_ID
    AND A.PROCESS_ID = C.PROCESS_ID
    AND A.ITEM_NAME = E.PART_NO
    AND G.EMP_ID = A.UPDATE_USERID
    AND B.PART_ID = :PART_ID
    AND TRIM(F.PROCESS_NAME) = TRIM(:PROCESS_NAME)
ORDER BY
    E.PART_NO
";
                var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.Number, "PART_ID", g_sPartId },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_NAME", LbProcess8.Text.Trim() },
                };

                DataSet dsTemp = ClientUtils.ExecuteSQL(s, p.ToArray());

                if (dsTemp.Tables[0].Rows.Count == 0)
                {
                    s = @"
SELECT
    E.PART_NO
FROM
    SAJET.SYS_BOM_INFO   B,
    SAJET.SYS_BOM        C,
    SAJET.SYS_PART       E,
    SAJET.SYS_PROCESS    F
WHERE
    1 = 1
    AND B.BOM_ID = C.BOM_ID
    AND E.PART_ID = C.ITEM_PART_ID
    AND F.PROCESS_ID = C.PROCESS_ID
    AND B.PART_ID = :PART_ID
    AND TRIM(F.PROCESS_NAME) = TRIM(:PROCESS_NAME)
ORDER BY
    E.PART_NO
";
                    // 參數一樣
                    dsTemp = ClientUtils.ExecuteSQL(s, p.ToArray());

                    //dsTemp.Tables[0].Columns.Add("ENABLED");
                    dsTemp.Tables[0].Columns.Add("PRINT");
                    dsTemp.Tables[0].Columns.Add("COLUMN_ITEM");
                    dsTemp.Tables[0].Columns.Add("ROW_ITEM");
                    dsTemp.Tables[0].Columns.Add("UPDATE_USERID");
                    dsTemp.Tables[0].Columns.Add("UPDATE_TIME");
                }

                // 產生需要的 CheckBox 物件
                //DataGridViewCheckBoxColumn cbEnabled = new DataGridViewCheckBoxColumn();
                DataGridViewCheckBoxColumn cbPrint = new DataGridViewCheckBoxColumn();
                //// --- 設定 CheckBox 欄位的各個選擇
                //cbEnable.Name = dsTemp.Tables[0].Columns[1].ColumnName; // 將欄位的「名稱」對應到 DataSource 中的某個欄位
                //myCheckboxColumn.TrueValue = "Y";  // 設定「勾選時，內容值為何」
                //myCheckboxColumn.FalseValue = "N"; // 設定「未勾選時，內容值為何」
                //myCheckboxColumn.DataPropertyName = dsTemp.Tables[0].Columns[1].ColumnName; // 將欄位「資料內容」對應到 DataSource 中的某個欄位

                int cnt = DgvMaterial.Columns.Count;

                for (int i = 0; dsTemp.Tables[0].Columns.Count > i; i++)
                {
                    if (i == 1)
                    {
                        this.DgvMaterial.Columns.Insert(i, cbPrint); //cbEnabled
                        this.DgvMaterial.Columns[i].HeaderText = SajetCommon.SetLanguage(dsTemp.Tables[0].Columns[i].ColumnName);
                    }
                    //else if (i == 2)
                    //{
                    //    this.dgvMaterial.Columns.Insert(i, cbPrint);
                    //    this.dgvMaterial.Columns[i].HeaderText = SajetCommon.SetLanguage(dsTemp.Tables[0].Columns[i].ColumnName);
                    //}
                    else
                    {
                        DgvMaterial.Columns.Add(dsTemp.Tables[0].Columns[i].ColumnName, SajetCommon.SetLanguage(dsTemp.Tables[0].Columns[i].ColumnName));

                        if (i == 0 || i == 4 || i == 5)
                            DgvMaterial.Columns[i].ReadOnly = true;
                    }
                }

                for (int j = 0; dsTemp.Tables[0].Rows.Count > j; j++)
                {
                    DgvMaterial.Rows.Add();

                    for (int i = 0; dsTemp.Tables[0].Columns.Count > i; i++)
                    {
                        if (i == 1)
                        {
                            if (dsTemp.Tables[0].Rows[j][i].ToString() == "Y")
                                DgvMaterial.Rows[j].Cells[i].Value = true;
                            else
                            {
                                DgvMaterial.Rows[j].Cells[i].Value = false;
                                DgvMaterial.Rows[j].Cells[i + 1].Value = "";
                                DgvMaterial.Rows[j].Cells[i + 2].Value = "";
                                DgvMaterial.Rows[j].Cells[i + 1].ReadOnly = true;
                                DgvMaterial.Rows[j].Cells[i + 2].ReadOnly = true;
                            }
                        }
                        //else if (i == 2)
                        //{
                        //    if (dsTemp.Tables[0].Rows[j][i].ToString() == "Y")
                        //        dgvMaterial.Rows[j].Cells[i].Value = true;
                        //    else
                        //        dgvMaterial.Rows[j].Cells[i].Value = false;
                        //}
                        else
                            DgvMaterial.Rows[j].Cells[i].Value = dsTemp.Tables[0].Rows[j][i].ToString();
                    }
                }
                DgvMaterial.AllowUserToAddRows = false;
                DgvMaterial.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// 顯示品保收驗設定
        /// </summary>
        private void Show_QCOption(ProcessViewModel process_info = null)
        {
            LvQCOption.Items.Clear();

            if (LvQCOption.Columns.Count <= 0)
            {
                LvQCOption.Columns.Add(SajetCommon.SetLanguage(FormTextEnum.FirstPieceInspection.ToString()));

                LvQCOption.Columns.Add(SajetCommon.SetLanguage(FormTextEnum.LastPieceInspection.ToString()));
            }

            if (process_info == null) return;

            var d = QoSrv.GetData(process_info);

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                LvQCOption.Items.Add(d.Tables[0].Rows[0]["FPI"].ToString());

                LvQCOption.Items[0].SubItems.Add(d.Tables[0].Rows[0]["LPI"].ToString());
            }

            LvQCOption.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        /// <summary>
        /// 預覽項目
        /// </summary>
        private void Show_Preview()
        {
            if (string.IsNullOrWhiteSpace(g_sPartId))
            {
                return;
            }

            string columnName = "VALUE_DEFAULT";

            int item_type = 0;

            if (Tc_1.SelectedTab == TpCondition ||
                Tc_1.SelectedTab == TpCollection)
            {
                if (Tc_1.SelectedTab == TpCondition)
                {
                    item_type = 0;

                    columnName = "VALUE_DEFAULT";
                }

                if (Tc_1.SelectedTab == TpCollection)
                {
                    item_type = 1;

                    columnName = "Input value";
                }

                string s = $@"
/* 指定料號的基本資訊 */
WITH PART_INFO AS (
    SELECT
        PART_ID,
        ROUTE_ID
    FROM
        SAJET.SYS_PART
    WHERE
        PART_ID = :PART_ID
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
        PART_INFO                   B,
        SAJET.SYS_RC_ROUTE          C
    WHERE
        B.ROUTE_ID = A.ROUTE_ID
        AND B.ROUTE_ID = C.ROUTE_ID
        AND C.ENABLED = 'Y'
    START WITH A.ROUTE_ID = B.ROUTE_ID
               AND NODE_CONTENT = 'START' CONNECT BY PRIOR NEXT_NODE_ID = NODE_ID
                                                     OR PRIOR NEXT_NODE_ID = GROUP_ID
)
SELECT
    A.ITEM_ID,
    C.PROCESS_ID,
    C.PROCESS_NAME,
    A.ITEM_NAME,
    A.ITEM_SEQ,
    CASE A.ITEM_PHASE
        WHEN 'A'   THEN
            'ALL'
        WHEN 'I'   THEN
            'INPUT'
        WHEN 'O'   THEN
            'OUTPUT'
    END ITEM_PHASE,
    A.VALUE_DEFAULT ""{columnName}""
FROM
    SAJET.SYS_RC_PROCESS_PARAM_PART   A,
    SAJET.SYS_PROCESS                 C,
    PART_INFO                         D,
    ROUTE_NODES                       F
WHERE
    A.PART_ID = D.PART_ID
    AND A.PROCESS_ID = C.PROCESS_ID
    AND TO_CHAR(C.PROCESS_ID) = F.NODE_CONTENT
    AND ITEM_TYPE = :ITEM_TYPE
ORDER BY
    F.IDX,
    A.ITEM_SEQ,
    A.ITEM_NAME
";
                var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.Number, "PART_ID", g_sPartId },
                    new object[] { ParameterDirection.Input, OracleType.Number, "ITEM_TYPE", item_type },
                };

                var d = ClientUtils.ExecuteSQL(s, p.ToArray());

                if (d != null)
                {
                    DgvPreview.DataSource = d;

                    DgvPreview.DataMember = d.Tables[0].ToString();
                }

                var hiddenCol = new List<string>
                {
                    "item_id",
                    "process_id",
                };

                foreach (DataGridViewColumn column in DgvPreview.Columns)
                {
                    if (hiddenCol?.Any(x => x.Trim().ToUpper().Contains(column.Name.Trim().ToUpper())) ?? false)
                    {
                        column.Visible = false;
                    }

                    column.HeaderText = SajetCommon.SetLanguage(column.HeaderText);
                }

            }

            #region Isaac Preview 2021/01/07
            // QC Collection
            if (Tc_1.SelectedTab == TpQCCollection)
            {
                item_type = 1;

                columnName = "Input value";

                g_QCCollection.ShowPreview(g_sPartId, item_type, columnName, ref DgvPreview);

            }

            // AQL
            if (Tc_1.SelectedTab == TpAQL)
            {
                //DgvPreview.DataSource = null;
                g_AQL.ShowPreview(g_sPartId, ref DgvPreview);

            }
            #endregion

            if (Tc_1.SelectedTab == TpQCOption)
            {
                var d = QoSrv.GetData(part_id: g_sPartId);

                if (d != null)
                {
                    DgvPreview.DataSource = d;

                    DgvPreview.DataMember = d.Tables[0].ToString();

                    var h = QoSrv.PreviewHiddenColumnNames();

                    foreach (DataGridViewColumn column in DgvPreview.Columns)
                    {
                        if (h?.Any(x => x.Trim().ToUpper().Contains(column.Name.Trim().ToUpper())) ?? false)
                        {
                            column.Visible = false;
                        }

                        column.HeaderText = SajetCommon.SetLanguage(column.Name);
                    }
                }

            }

            // 欄位可以調整寬度，不能排序資料內容
            foreach (DataGridViewColumn column in DgvPreview.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            }
        }

        public bool chkData()
        {
            try
            {
                if (!CheckPart())
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Input Part No."), 0);
                    return false;
                }

                if (LbProcess2.Text.Trim() == "N/A")
                {
                    SajetCommon.Show_Message(SajetCommon.SetLanguage("Please Select Target Process."), 0);
                    return false;
                }
                else
                {
                    string s = @"
SELECT
    PROCESS_ID
FROM
    SAJET.SYS_PROCESS
WHERE
    TRIM(PROCESS_NAME) = TRIM(:PROCESS_NAME)
";
                    var p = new List<object[]>
                    {
                        new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_NAME", LbProcess1.Text.Trim() },
                    };

                    DataSet dsTemp = ClientUtils.ExecuteSQL(s, p.ToArray());

                    g_sProcessId = dsTemp.Tables[0].Rows[0]["PROCESS_ID"].ToString();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
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
DELETE SAJET.SYS_RC_PROCESS_PARAM_PART
WHERE
    PART_ID = :PART_ID
    AND PROCESS_ID = :PROCESS_ID
    AND ITEM_TYPE = :ITEM_TYPE
    AND ITEM_SEQ = :ITEM_SEQ
    AND ITEM_NAME = :ITEM_NAME
";
                var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.Number, "PART_ID", g_sPartId },
                    new object[] { ParameterDirection.Input, OracleType.Number, "PROCESS_ID", g_sProcessId },
                    new object[] { ParameterDirection.Input, OracleType.Number, "ITEM_TYPE", type },
                    new object[] { ParameterDirection.Input, OracleType.Number, "ITEM_SEQ", seq },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "ITEM_NAME", name.Trim() },
                };

                ClientUtils.ExecuteSQL(s, p.ToArray());

                return true;
            }
            catch (Exception)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Delete Item Error."), 0);
                return false;
            }
        }

        /// <summary>
        /// 將資料清空，恢復為初始值。(Isaac 2021/01/07)
        /// </summary>
        private void ClearData()
        {
            g_sPartId = string.Empty;
            FMain_Load(null, null);
            TvProcess.Nodes.Clear();
            DgvPreview.DataSource = null;
            //LbVersion.Text = "N/A";
            DgvPartInfo.DataSource = null;

            if (TpAQL.Parent != null)
            {
                LbProcess4.Text = "N/A";
                LvAQL.Clear();
                bLVAQLCollectionSelected = false;
            }

            if (TpCollection.Parent != null)
            {
                LbProcess2.Text = "N/A";
                LvCollection.Clear();
                bLVCollectionSelected = false;
            }

            if (TpCondition.Parent != null)
            {
                LbProcess1.Text = "N/A";
                LvCondition.Clear();
                Tc_1.SelectedTab = TpCondition;
                bLVConditionSelected = false;
            }

            if (TpQCCollection.Parent != null)
            {
                LbProcess3.Text = "N/A";
                LvQCCollection.Clear();
                bLVQCCollectionSelected = false;
            }

            if (TpSNCondition.Parent != null)
            {
                LbProcess5.Text = "N/A";
                LvSNCondition.Clear();
                Tc_1.SelectedTab = TpSNCondition;
                bLVSNConditionSelected = false;
            }

            if (TpSNCollection.Parent != null)
            {
                LbProcess3.Text = "N/A";
                LvSNCollection.Clear();
                bLVSNCollectionSelected = false;
            }

            if (TpAlert.Parent != null)
            {
                LbProcess7.Text = "N/A";
                LvAlertCollection.Clear();
                blvAlertCollectionSelected = false;
            }

            if (TpKeypart.Parent != null)
            {
                LbProcess8.Text = "N/A";
                DgvMaterial.DataSource = null;
            }
        }

        /// <summary>
        /// Analyzing System Type - Y:lot control, N:piece control
        /// </summary>
        /// <returns>true:lot control, false:piece control</returns>
        private bool systemType()
        {
            try
            {
                string sSQL = @"
SELECT
    PARAM_VALUE
FROM
    SAJET.SYS_BASE
WHERE
    PROGRAM = 'RC Manager'
    AND PARAM_NAME = 'Lot Control Checked'
    AND ROWNUM = 1
";
                DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);

                if (dsTemp != null)
                {
                    if (dsTemp.Tables[0].Rows.Count > 0)
                    {
                        g_SystemType = dsTemp.Tables[0].Rows[0]["PARAM_VALUE"].ToString();

                        if (g_SystemType == "Y")
                            return true;
                        else
                            return false;
                    }
                }

                return false;
            }
            catch (Exception)
            {
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

        private bool CheckPart()
        {
            if (string.IsNullOrEmpty(editPart.Text))
            {
                return false;
            }
            else
            {
                string s = " SELECT PART_ID FROM SAJET.SYS_PART WHERE PART_NO = :PART_NO ";
                var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "PART_NO", editPart.Text.Trim() },
                };
                DataSet dsTemp = ClientUtils.ExecuteSQL(s, p.ToArray());

                if (dsTemp.Tables[0].Rows.Count == 0)
                {
                    return false;
                }
                else
                {
                    g_sPartId = dsTemp.Tables[0].Rows[0]["PART_ID"].ToString();
                    return true;
                }
            }
        }
    }
}

