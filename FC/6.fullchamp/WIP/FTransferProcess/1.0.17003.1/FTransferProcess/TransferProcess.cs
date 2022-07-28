using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SajetClass;
using System.Data.OracleClient;
using System.Reflection;
using TsSrv = FTransferProcess.Services.TransferService;

namespace FTransferProcess
{
    public partial class TransferProcess : Form
    {
        /// <summary>
        /// 生產途程節點類型（0：起點/ 1：NEXT/ 2：AND 群組/ 3：OR 群組/ 9：終點/）
        /// </summary>
        public string sNode_type;
        /// <summary>
        /// 生產途程 ID
        /// </summary>
        public string sRoute_Id;
        /// <summary>
        /// 生產途程節點 ID
        /// </summary>
        public string sNode_Id;
        /// <summary>
        /// 製程 ID
        /// </summary>
        public string sProcess_Id;
        /// <summary>
        /// 下一個生產途程節點 ID
        /// </summary>
        public string sNext_Node;
        /// <summary>
        /// 下一個製程 ID
        /// </summary>
        public string sNext_Process;
        /// <summary>
        /// 流程卡號
        /// </summary>
        public string sRc_No;

        private int iFirstLoad;

        private string sSQL;

        DataSet dsTemp, dsTempGroup;

        /// <summary>
        /// 製程選擇器
        /// </summary>
        public TransferProcess()
        {
            InitializeComponent();

            SajetCommon.g_sCallerName = Assembly.GetCallingAssembly().GetName().Name;

            SajetCommon.SetLanguageControl(this);

            Text = $"{Text} ({SajetCommon.g_sFileVersion})";

            Load += TransferProcess_Load;

            Btn_OK.Click += Btn_OK_Click;

            Btn_Return.Click += Btn_Return_Click;

            Btn_Cancel.Click += Btn_Cancel_Click;

            DGV_Process.CellMouseClick += DGV_Process_CellMouseClick;
        }

        private void TransferProcess_Load(object sender, EventArgs e)
        {
            iFirstLoad = 1;

            TB_Process.ReadOnly = true;

            TB_Exit.ReadOnly = true;

            if (sNode_type == "1")
            {
                sSQL = @"
SELECT
    A.NODE_CONTENT
   ,B.PROCESS_NAME
FROM
    SAJET.SYS_RC_ROUTE_DETAIL A
   ,SAJET.SYS_PROCESS B
WHERE
    A.NODE_CONTENT = B.PROCESS_ID
AND B.ENABLED = 'Y'
AND A.ROUTE_ID = :ROUTE_ID
AND A.NODE_ID = :NODE_ID
GROUP BY
    A.NODE_CONTENT
   ,B.PROCESS_NAME
ORDER BY
    A.NODE_CONTENT ASC
";
            }
            else
            {
                sSQL = @"
SELECT
    A.NODE_CONTENT
   ,B.PROCESS_NAME
FROM
    SAJET.SYS_RC_ROUTE_DETAIL A
   ,SAJET.SYS_PROCESS B
WHERE
    A.NODE_CONTENT = B.PROCESS_ID
AND B.ENABLED = 'Y'
AND A.ROUTE_ID = :ROUTE_ID
AND A.GROUP_ID = :NODE_ID
GROUP BY
    A.NODE_CONTENT
   ,B.PROCESS_NAME
ORDER BY
    A.NODE_CONTENT ASC
";
            }

            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "ROUTE_ID", sRoute_Id },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "NODE_ID", sNode_Id },
            };

            dsTemp = ClientUtils.ExecuteSQL(sSQL, p.ToArray());

            for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
            {
                Button Btn = new Button
                {
                    Tag = dsTemp.Tables[0].Rows[i]["NODE_CONTENT"].ToString(),
                    Text = dsTemp.Tables[0].Rows[i]["PROCESS_NAME"].ToString(),
                    Height = this.Width = 90,
                    BackColor = Color.Khaki,
                    Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)134),
                };

                Btn.Click += new EventHandler(Btn_OK_Click);

                FLP_ButtonZone.Controls.Add(Btn);
            }

            if (FLP_ButtonZone.Controls[0] is Button BtnClick)
            {
                BtnClick.PerformClick();
            }
        }

        private void Btn_OK_Click(object sender, EventArgs e)
        {
            Button Btn = sender as Button;

            sProcess_Id = Btn.Tag.ToString();

            TB_Process.Text = Btn.Text;

            sNext_Process = "";

            TB_Exit.Text = "";

            DGV_Process.Rows.Clear();

            List<DataRow> route_detail = TsSrv.GetRouteDetailList(route_id: sRoute_Id);

            List<string> next_node_id_list
                = TsSrv.GetNextNodeIDs(
                    route_detail: route_detail,
                    node_id: sNode_Id,
                    rc_no: sRc_No);

            string s = @"
SELECT
    A.NODE_ID,
    A.NODE_CONTENT,
    B.PROCESS_NAME,
    A.LINK_NAME
FROM
    SAJET.SYS_RC_ROUTE_DETAIL   A,
    SAJET.SYS_PROCESS           B
WHERE
    A.ROUTE_ID = :ROUTE_ID
    AND A.NODE_CONTENT = TO_CHAR(B.PROCESS_ID)
    AND A.NODE_ID IN (NODE_IDS)
ORDER BY
    B.PROCESS_NAME
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "ROUTE_ID", sRoute_Id },
            };

            var node_name_list = new List<string>
            {
                "0"
            };

            for (int i = 0; i < next_node_id_list.Count; i++)
            {
                node_name_list.Add($":PARAM_{i}");

                p.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, $"PARAM_{i}", next_node_id_list[i] });
            }

            s = s.Replace("NODE_IDS", string.Join(",", node_name_list));

            DataSet next_process_list = ClientUtils.ExecuteSQL(s, p.ToArray());

            foreach (DataRow next_process_row in next_process_list.Tables[0].Rows)
            {
                var values = new object[]
                {
                    next_process_row["NODE_ID"].ToString(),
                    next_process_row["NODE_CONTENT"].ToString(),
                    next_process_row["PROCESS_NAME"].ToString(),
                    next_process_row["LINK_NAME"].ToString(),
                };

                DGV_Process.Rows.Insert(DGV_Process.Rows.Count, values);
            }

            #region 舊的方法註解，換其他方法試試看
            /*

            string sCheckEnd = "N", sCheckGroup = "N";

            if (sNode_type == "2") // (2 = AND , 必須停留在GROUP製程)
            {
                sSQL = @"
SELECT
    A.NODE_CONTENT
   ,B.PROCESS_NAME
   ,(
        SELECT
            NODE_ID
        FROM
            SAJET.SYS_RC_ROUTE_DETAIL
        WHERE
            NODE_ID = :NODE_ID
        GROUP BY
            NODE_ID
    ) AS NODE_ID
   ,(
        SELECT
            NODE_CONTENT
        FROM
            SAJET.SYS_RC_ROUTE_DETAIL
        WHERE
            NODE_ID = :NODE_ID
        GROUP BY
            NODE_CONTENT
    ) AS LINK_NAME
FROM
    SAJET.SYS_RC_ROUTE_DETAIL A
   ,SAJET.SYS_PROCESS B
WHERE
    A.NODE_CONTENT = B.PROCESS_ID
AND B.ENABLED = 'Y'
AND A.ROUTE_ID = :ROUTE_ID
AND A.GROUP_ID = :NODE_ID
AND A.NODE_CONTENT <> :PROCESS_ID
ORDER BY
    A.NODE_CONTENT ASC
";
                var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "ROUTE_ID", sRoute_Id },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "NODE_ID", sNode_Id },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", sProcess_Id },
                };

                dsTemp = ClientUtils.ExecuteSQL(sSQL, p.ToArray());

                for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                {
                    var values = new object[]
                    {
                        dsTemp.Tables[0].Rows[i]["NODE_ID"].ToString(),
                        dsTemp.Tables[0].Rows[i]["NODE_CONTENT"].ToString(),
                        dsTemp.Tables[0].Rows[i]["PROCESS_NAME"].ToString(),
                        dsTemp.Tables[0].Rows[i]["LINK_NAME"].ToString(),
                    };

                    DGV_Process.Rows.Insert(DGV_Process.Rows.Count, values);
                }
            }
            else
            {
                // (檢查是否有END製程 & GROUP製程)
                sSQL = @"
SELECT
    A.NODE_TYPE
   ,A.NODE_ID
   ,A.NODE_CONTENT
   ,B.LINK_NAME
FROM
    SAJET.SYS_RC_ROUTE_DETAIL A
   ,(
        SELECT
            NEXT_NODE_ID
           ,LINK_NAME
        FROM
            SAJET.SYS_RC_ROUTE_DETAIL
        WHERE
            ROUTE_ID = :ROUTE_ID
        AND NODE_ID = :NODE_ID
    ) B
WHERE
    A.NODE_ID = B.NEXT_NODE_ID
GROUP BY
    A.NODE_TYPE
   ,A.NODE_ID
   ,A.NODE_CONTENT
   ,B.LINK_NAME
ORDER BY
    A.NODE_CONTENT ASC
";
                var p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "ROUTE_ID", sRoute_Id },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "NODE_ID", sNode_Id },
                };

                dsTemp = ClientUtils.ExecuteSQL(sSQL, p.ToArray());

                for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                {
                    if (dsTemp.Tables[0].Rows[i]["NODE_TYPE"].ToString() == "9")
                    {
                        sCheckEnd = "Y";

                        var values = new object[]
                        {
                            dsTemp.Tables[0].Rows[i]["NODE_ID"].ToString(),
                            "0",
                            dsTemp.Tables[0].Rows[i]["NODE_CONTENT"].ToString(),
                            dsTemp.Tables[0].Rows[i]["LINK_NAME"].ToString()
                        };

                        // (END製程)
                        DGV_Process.Rows.Insert(DGV_Process.Rows.Count, values);
                    }

                    if (dsTemp.Tables[0].Rows[i]["NODE_TYPE"].ToString() == "2" || dsTemp.Tables[0].Rows[i]["NODE_TYPE"].ToString() == "3")
                    {
                        sCheckGroup = "Y";

                        // (GROUP製程)
                        sSQL = @"
SELECT
    A.NODE_ID
   ,A.NODE_CONTENT
   ,B.PROCESS_NAME
FROM
    SAJET.SYS_RC_ROUTE_DETAIL A
   ,SAJET.SYS_PROCESS B
WHERE
    A.NODE_CONTENT = B.PROCESS_ID
AND A.GROUP_ID = :NODE_ID
ORDER BY
    A.NODE_CONTENT ASC
";
                        p = new List<object[]>
                        {
                            new object[] { ParameterDirection.Input, OracleType.VarChar, "NODE_ID", dsTemp.Tables[0].Rows[i]["NODE_ID"].ToString() },
                        };

                        dsTempGroup = ClientUtils.ExecuteSQL(sSQL, p.ToArray());

                        for (int j = 0; j < dsTempGroup.Tables[0].Rows.Count; j++)
                        {
                            var values = new object[]
                            {
                                dsTempGroup.Tables[0].Rows[i]["NODE_ID"].ToString(),
                                dsTempGroup.Tables[0].Rows[j]["NODE_CONTENT"].ToString(),
                                dsTempGroup.Tables[0].Rows[j]["PROCESS_NAME"].ToString(),
                                dsTemp.Tables[0].Rows[i]["NODE_CONTENT"].ToString()
                            };

                            DGV_Process.Rows.Insert(DGV_Process.Rows.Count, values);
                        }
                    }
                }

                sSQL = @"
SELECT DISTINCT
    A.NODE_ID
   ,A.NODE_CONTENT
   ,C.PROCESS_NAME
   ,B.LINK_NAME
FROM
    SAJET.SYS_RC_ROUTE_DETAIL A
   ,(
        SELECT
            NEXT_NODE_ID
           ,LINK_NAME
        FROM
            SAJET.SYS_RC_ROUTE_DETAIL
        WHERE
            ROUTE_ID = :ROUTE_ID
        AND NODE_ID = :NODE_ID
    ) B
   ,SAJET.SYS_PROCESS C
WHERE
    A.NODE_ID = B.NEXT_NODE_ID
AND A.NODE_CONTENT = C.PROCESS_ID
";

                if (sCheckEnd == "Y")
                {
                    sSQL += " AND A.NODE_TYPE <> 9 ";
                }

                if (sCheckGroup == "Y")
                {
                    sSQL += " AND A.NODE_TYPE <> 2 ";
                    sSQL += " AND A.NODE_TYPE <> 3 ";
                }

                sSQL += @"
UNION ALL

SELECT
    B.NODE_ID,
    A.NODE_CONTENT,
    C.PROCESS_NAME,
    B.LINK_NAME
FROM
    SAJET.SYS_RC_ROUTE_DETAIL   A,
    (
        SELECT
            B.NEXT_NODE_ID NODE_ID,
            B.LINK_NAME
        FROM
            SAJET.SYS_RC_ROUTE_DETAIL   A,
            SAJET.SYS_RC_ROUTE_DETAIL   B
        WHERE
            A.ROUTE_ID = :ROUTE_ID
            AND A.NODE_ID = :NODE_ID
            AND A.GROUP_ID = B.NODE_ID
    ) B,
    SAJET.SYS_PROCESS           C
WHERE
    A.NODE_ID = B.NODE_ID
    AND A.NODE_CONTENT = TO_CHAR(C.PROCESS_ID)
    AND A.NODE_CONTENT <> :PROCESS_ID
";

                if (sCheckEnd == "Y")
                {
                    sSQL += " AND A.NODE_TYPE <> 9 ";
                }

                if (sCheckGroup == "Y")
                {
                    sSQL += " AND A.NODE_TYPE <> 2 ";
                    sSQL += " AND A.NODE_TYPE <> 3 ";
                }

                p = new List<object[]>
                {
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "ROUTE_ID", sRoute_Id },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "NODE_ID", sNode_Id },
                    new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", sProcess_Id },
                };

                dsTemp = ClientUtils.ExecuteSQL(sSQL, p.ToArray());

                for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                {
                    var values = new object[]
                    {
                        dsTemp.Tables[0].Rows[i]["NODE_ID"].ToString(),
                        dsTemp.Tables[0].Rows[i]["NODE_CONTENT"].ToString(),
                        dsTemp.Tables[0].Rows[i]["PROCESS_NAME"].ToString(),
                        dsTemp.Tables[0].Rows[i]["LINK_NAME"].ToString()
                    };

                    DGV_Process.Rows.Insert(DGV_Process.Rows.Count, values);
                }
            }

            //*/
            #endregion

            if (DGV_Process.Rows.Count > 0 && DGV_Process.CurrentRow != null)
            {
                sNext_Node = DGV_Process.CurrentRow.Cells[0].Value.ToString();

                sNext_Process = DGV_Process.CurrentRow.Cells[1].Value.ToString();

                TB_Exit.Text = DGV_Process.CurrentRow.Cells[2].Value.ToString();

                Btn_Return.Enabled = true;

                if (sNode_type == "1" && DGV_Process.Rows.Count == 1)
                {
                    Btn_Return_Click(sender, e);
                }
                else if (iFirstLoad > 1 && DGV_Process.Rows.Count == 1)
                {
                    Btn_Return_Click(sender, e);
                }
            }

            iFirstLoad++;
        }

        private void Btn_Return_Click(object sender, EventArgs e)
        {
            if (sNext_Process != "")
            {
                DialogResult = DialogResult.OK;
            }
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DGV_Process_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (DGV_Process.Rows.Count > 0 && DGV_Process.CurrentRow != null)
            {
                sNext_Node = DGV_Process.CurrentRow.Cells[0].Value.ToString();

                sNext_Process = DGV_Process.CurrentRow.Cells[1].Value.ToString();

                TB_Exit.Text = DGV_Process.CurrentRow.Cells[2].Value.ToString();

                Btn_Return.Enabled = true;
            }
        }
    }
}
