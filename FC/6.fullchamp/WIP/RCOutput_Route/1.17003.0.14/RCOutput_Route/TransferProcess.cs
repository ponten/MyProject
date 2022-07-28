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

namespace RCOutput_Route
{
    public partial class TransferProcess : Form
    {
        public int iFirstLoad;

        public string sSQL, sNode_type, sRoute_Id, sNode_Id;
        public string sProcess_Id;
        public string sNext_Node, sNext_Process;

        DataSet dsTemp, dsTempGroup;

        public TransferProcess()
        {
            InitializeComponent();
        }

        private void TransferProcess_Load(object sender, EventArgs e)
        {
            iFirstLoad = 1;

            SajetCommon.SetLanguageControl(this);

            TB_Process.ReadOnly = true;
            TB_Exit.ReadOnly = true;

            if (sNode_type == "1")
            {
                sSQL = @"
  SELECT A.NODE_CONTENT
        ,B.PROCESS_NAME
    FROM SAJET.SYS_RC_ROUTE_DETAIL A
        ,SAJET.SYS_PROCESS         B
   WHERE A.NODE_CONTENT = B.PROCESS_ID
     AND B.ENABLED      = 'Y'
     AND A.ROUTE_ID     = :ROUTE_ID
     AND A.NODE_ID      = :NODE_ID
GROUP BY A.NODE_CONTENT
        ,B.PROCESS_NAME
ORDER BY A.NODE_CONTENT ASC
";
            }
            else
            {
                sSQL = @"
  SELECT A.NODE_CONTENT
        ,B.PROCESS_NAME
    FROM SAJET.SYS_RC_ROUTE_DETAIL A
        ,SAJET.SYS_PROCESS         B
   WHERE A.NODE_CONTENT = B.PROCESS_ID
     AND B.ENABLED      = 'Y'
     AND A.ROUTE_ID     = :ROUTE_ID
     AND A.GROUP_ID     = :NODE_ID
GROUP BY A.NODE_CONTENT
        ,B.PROCESS_NAME
ORDER BY A.NODE_CONTENT ASC
";
            }

            object[][] param = new object[2][];
            param[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ROUTE_ID", sRoute_Id };
            param[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NODE_ID", sNode_Id };
            dsTemp = ClientUtils.ExecuteSQL(sSQL, param);

            for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
            {
                Button Btn = new Button
                {
                    Tag = dsTemp.Tables[0].Rows[i]["NODE_CONTENT"].ToString(),
                    Text = dsTemp.Tables[0].Rows[i]["PROCESS_NAME"].ToString(),

                    Height = 90,
                    Width = 90,
                    BackColor = Color.Khaki,
                    Font = new Font("新細明體", 9F, FontStyle.Regular, GraphicsUnit.Point, 134)
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

            string sCheckEnd = "N", sCheckGroup = "N";

            if (sNode_type == "2") // (2 = AND , 必須停留在GROUP製程)
            {
                sSQL = @"
  SELECT A.NODE_CONTENT
        ,B.PROCESS_NAME
        ,( SELECT NODE_ID
             FROM SAJET.SYS_RC_ROUTE_DETAIL
            WHERE NODE_ID = :NODE_ID
         GROUP BY NODE_ID )      AS NODE_ID
        ,( SELECT NODE_CONTENT
             FROM SAJET.SYS_RC_ROUTE_DETAIL
            WHERE NODE_ID = :NODE_ID
         GROUP BY NODE_CONTENT ) AS LINK_NAME
    FROM SAJET.SYS_RC_ROUTE_DETAIL A
        ,SAJET.SYS_PROCESS B
   WHERE A.NODE_CONTENT  = B.PROCESS_ID
     AND B.ENABLED       = 'Y'
     AND A.ROUTE_ID      = :ROUTE_ID
     AND A.GROUP_ID      = :NODE_ID
     AND A.NODE_CONTENT <> :PROCESS_ID
ORDER BY A.NODE_CONTENT ASC
";
                object[][] param = new object[3][];
                param[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ROUTE_ID", sRoute_Id };
                param[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NODE_ID", sNode_Id };
                param[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", sProcess_Id };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, param);

                for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                {
                    DGV_Process.Rows.Insert(DGV_Process.Rows.Count,
                        new object[]
                        {
                            dsTemp.Tables[0].Rows[i]["NODE_ID"].ToString(),
                            dsTemp.Tables[0].Rows[i]["NODE_CONTENT"].ToString(),
                            dsTemp.Tables[0].Rows[i]["PROCESS_NAME"].ToString(),
                            dsTemp.Tables[0].Rows[i]["LINK_NAME"].ToString() }
                        );
                }
            }
            else
            {
                // (檢查是否有END製程 & GROUP製程)
                sSQL = $@"
  SELECT A.NODE_TYPE
        ,A.NODE_ID
        ,A.NODE_CONTENT
        ,B.LINK_NAME
    FROM SAJET.SYS_RC_ROUTE_DETAIL A
        ,( SELECT NEXT_NODE_ID
                 ,LINK_NAME
             FROM SAJET.SYS_RC_ROUTE_DETAIL
            WHERE ROUTE_ID = :ROUTE_ID
              AND NODE_ID  = :NODE_ID ) B
   WHERE A.NODE_ID = B.NEXT_NODE_ID
GROUP BY A.NODE_TYPE
        ,A.NODE_ID
        ,A.NODE_CONTENT
        ,B.LINK_NAME
ORDER BY A.NODE_CONTENT ASC
";
                object[][] param = new object[2][];
                param[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ROUTE_ID", sRoute_Id };
                param[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NODE_ID", sNode_Id };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, param);

                for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                {
                    if (dsTemp.Tables[0].Rows[i]["NODE_TYPE"].ToString() == "9")
                    {
                        sCheckEnd = "Y";

                        // (END製程)
                        DGV_Process.Rows.Insert(DGV_Process.Rows.Count,
                            new object[]
                            {
                                dsTemp.Tables[0].Rows[i]["NODE_ID"].ToString(),
                                "0",
                                dsTemp.Tables[0].Rows[i]["NODE_CONTENT"].ToString(),
                                dsTemp.Tables[0].Rows[i]["LINK_NAME"].ToString()
                            });
                    }

                    if (dsTemp.Tables[0].Rows[i]["NODE_TYPE"].ToString() == "2"
                        || dsTemp.Tables[0].Rows[i]["NODE_TYPE"].ToString() == "3")
                    {
                        sCheckGroup = "Y";

                        // (GROUP製程)
                        sSQL = @"
  SELECT A.NODE_ID
        ,A.NODE_CONTENT
        ,B.PROCESS_NAME
    FROM SAJET.SYS_RC_ROUTE_DETAIL A
        ,SAJET.SYS_PROCESS B
   WHERE A.NODE_CONTENT = B.PROCESS_ID
     AND A.GROUP_ID     = :NODE_ID
ORDER BY A.NODE_CONTENT ASC
";
                        param = new object[1][];
                        param[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NODE_ID", dsTemp.Tables[0].Rows[i]["NODE_ID"].ToString() };
                        dsTempGroup = ClientUtils.ExecuteSQL(sSQL, param);

                        for (int j = 0; j < dsTempGroup.Tables[0].Rows.Count; j++)
                        {
                            DGV_Process.Rows.Insert(DGV_Process.Rows.Count,
                                new object[]
                                {
                                    dsTemp.Tables[0].Rows[i]["NODE_ID"].ToString(),
                                    dsTempGroup.Tables[0].Rows[j]["NODE_CONTENT"].ToString(),
                                    dsTempGroup.Tables[0].Rows[j]["PROCESS_NAME"].ToString(),
                                    dsTemp.Tables[0].Rows[i]["NODE_CONTENT"].ToString()
                                });
                        }
                    }
                }

                sSQL = @"
   SELECT
 DISTINCT A.NODE_ID
         ,A.NODE_CONTENT
         ,C.PROCESS_NAME
         ,B.LINK_NAME
     FROM SAJET.SYS_RC_ROUTE_DETAIL        A
         ,( SELECT NEXT_NODE_ID
                  ,LINK_NAME
              FROM SAJET.SYS_RC_ROUTE_DETAIL
             WHERE ROUTE_ID = :ROUTE_ID
               AND NODE_ID  = :NODE_ID ) B
         ,SAJET.SYS_PROCESS                C
    WHERE A.NODE_ID      = B.NEXT_NODE_ID
      AND A.NODE_CONTENT = C.PROCESS_ID
";

                if (sCheckEnd == "Y")
                {
                    sSQL += @"
      AND A.NODE_TYPE <> 9
";
                }

                if (sCheckGroup == "Y")
                {
                    sSQL += @"
      AND A.NODE_TYPE <> 2
      AND A.NODE_TYPE <> 3
";
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
                    sSQL += @"
      AND A.NODE_TYPE <> 9
";
                }

                if (sCheckGroup == "Y")
                {
                    sSQL += @"
      AND A.NODE_TYPE <> 2
      AND A.NODE_TYPE <> 3
";
                }

                param = new object[3][];
                param[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "ROUTE_ID", sRoute_Id };
                param[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "NODE_ID", sNode_Id };
                param[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", sProcess_Id };
                dsTemp = ClientUtils.ExecuteSQL(sSQL, param);

                for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                {
                    DGV_Process.Rows.Insert(DGV_Process.Rows.Count,
                        new object[]
                        {
                            dsTemp.Tables[0].Rows[i]["NODE_ID"].ToString(),
                            dsTemp.Tables[0].Rows[i]["NODE_CONTENT"].ToString(),
                            dsTemp.Tables[0].Rows[i]["PROCESS_NAME"].ToString(),
                            dsTemp.Tables[0].Rows[i]["LINK_NAME"].ToString() }
                        );
                }
            }

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
                DialogResult = DialogResult.OK;
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
