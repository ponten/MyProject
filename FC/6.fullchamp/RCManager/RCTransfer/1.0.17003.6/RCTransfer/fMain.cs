using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OracleClient;
using SajetClass;
using ExportExcel;
using SajetFilter;

namespace RCTransfer
{
    public partial class fMain : Form
    {
        public static string rc_UserID = ClientUtils.UserPara1;

        DataSet dsTemp;

        string sSQL;

        string sRC;

        string sTransferDesc;

        public fMain()
        {
            InitializeComponent();

            label2.Visible = false;
            Dgv_RC.ReadOnly = false;

            SajetCommon.SetLanguageControl(this);

            this.Load += FMain_Load;

            Btn_Line.Click += Btn_Line_Click;

            Btn_Query.Click += Btn_Query_Click;

            Btn_WO.Click += Btn_WO_Click;

            Btn_AllSelect.Click += Btn_AllSelect_Click;

            Btn_AllCancel.Click += Btn_AllCancel_Click;

            Btn_Excute.Click += Btn_Excute_Click;
        }

        private void FMain_Load(object sender, EventArgs e)
        {
            SetCombRCSN();
        }

        private void Btn_Line_Click(object sender, EventArgs e)
        {
            Tb_Line_1.Clear();
            Tb_Line_2.Clear();
            Dgv_RC.Rows.Clear();

            string sSQL = @"
SELECT
    PDLINE_ID,
    PDLINE_NAME
FROM
    SAJET.SYS_PDLINE
WHERE
    ENABLED = 'Y'
    AND PDLINE_ID IN (
        SELECT DISTINCT
            DEFAULT_PDLINE_ID
        FROM
            SAJET.G_WO_BASE
    )
ORDER BY
    PDLINE_ID ASC
";
            var h = new List<string>
            {
                "PDLINE_ID",
            };

            using (var f = new SajetFilter.FFilter(
                sqlCommand: sSQL,
                hiddenColumns: h))
            {
                Cursor = Cursors.WaitCursor;

                f.Text = SajetCommon.SetLanguage("PDLINE_NAME");

                if (f.ShowDialog() == DialogResult.OK)
                {
                    DataRow result = f.ResultSets[0];

                    Tb_Line_2.Text = result["PDLINE_ID"].ToString();
                    Tb_Line_1.Text = result["PDLINE_NAME"].ToString();
                }

                Cursor = Cursors.Default;
            }
        }

        private void Btn_Query_Click(object sender, EventArgs e)
        {
            Dgv_RC.Rows.Clear();

            Tb_Line_1.Focus();
            Rtb_HD.Clear();

            Tb_WO_2.Clear();
            Tb_AvailableQty.Clear();
            Tb_Spec1.Clear();
            Tb_Spec2.Clear();
            Tb_Version.Clear();
            Tb_RouteName.Clear();

            ShowData();
        }

        private void Btn_WO_Click(object sender, EventArgs e)
        {
            Tb_WO_2.Text = "";
            Tb_AvailableQty.Text = "";
            Tb_Spec1.Text = "";
            Tb_Spec2.Text = "";
            Tb_Version.Text = "";
            Tb_RouteName.Text = "";

            string s = @"
SELECT
    A.WORK_ORDER,
    A.WO_TYPE,
    A.TARGET_QTY - A.INPUT_QTY AS AVAILABLE_QTY,
    B.SPEC1,
    B.SPEC2,
    B.VERSION,
    C.ROUTE_NAME
FROM
    SAJET.G_WO_BASE      A,
    SAJET.SYS_PART       B,
    SAJET.SYS_RC_ROUTE   C
WHERE
    A.PART_ID = B.PART_ID
    AND A.ROUTE_ID = C.ROUTE_ID
    AND A.WO_TYPE = '轉投工單'
    AND A.WO_STATUS > 1
    AND A.WO_STATUS < 4
    AND A.TARGET_QTY - A.INPUT_QTY > 0
ORDER BY
    A.WORK_ORDER ASC
";
            using (var f = new SajetFilter.FFilter(
                sqlCommand: s))
            {
                Cursor = Cursors.WaitCursor;

                f.Text = SajetCommon.SetLanguage("WORK_ORDER");

                if (f.ShowDialog() == DialogResult.OK)
                {
                    DataRow result = f.ResultSets[0];

                    Tb_WO_2.Text = result["WORK_ORDER"].ToString();
                    Tb_AvailableQty.Text = result["AVAILABLE_QTY"].ToString();
                    Tb_Spec1.Text = result["SPEC1"].ToString();
                    Tb_Spec2.Text = result["SPEC2"].ToString();
                    Tb_Version.Text = result["VERSION"].ToString();
                    Tb_RouteName.Text = result["ROUTE_NAME"].ToString();
                }

                Cursor = Cursors.Default;
            }
        }

        private void Btn_AllSelect_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Dgv_RC.Rows.Count; i++)
            {
                Dgv_RC.Rows[i].Cells[0].Value = true;
            }
        }

        private void Btn_AllCancel_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Dgv_RC.Rows.Count; i++)
            {
                Dgv_RC.Rows[i].Cells[0].Value = false;
            }
        }

        private void Btn_Excute_Click(object sender, EventArgs e)
        {
            int iPart_Id = 0, iRoute_Id = 0, iLine_Id = 0, iFactory_Id = 0;
            int iProcess_Id = 0, iStage_Id = 0;
            long iNode_Id = 0, iNext_Node = 0;
            string sVersion = "", sNext_Process = "", sSheet_Name = "";

            bool IsChecked;
            int n = 0;
            decimal dTransferQty = 0;

            string s;

            List<object[]> p;

            DataSet d;

            DataRow row;

            if (string.IsNullOrWhiteSpace(Tb_WO_2.Text))
            {
                string sMsg = SajetCommon.SetLanguage("Please Select Work Order");
                SajetCommon.Show_Message(sMsg, 1);
                return;
            }

            s = @"
SELECT
    A.PART_ID,
    A.VERSION,
    A.ROUTE_ID,
    A.DEFAULT_PDLINE_ID,
    A.FACTORY_ID
FROM
    SAJET.G_WO_BASE    A,
    SAJET.SYS_PDLINE   B
WHERE
    A.DEFAULT_PDLINE_ID = B.PDLINE_ID
    AND WORK_ORDER = :WORK_ORDER
";
            p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", Tb_WO_2.Text.Trim() },
            };

            d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                row = d.Tables[0].Rows[0];

                iPart_Id = Convert.ToInt32(row["PART_ID"].ToString());
                sVersion = row["VERSION"].ToString();
                iRoute_Id = Convert.ToInt32(row["ROUTE_ID"].ToString());
                iLine_Id = Convert.ToInt32(row["DEFAULT_PDLINE_ID"].ToString());
                iFactory_Id = Convert.ToInt32(row["FACTORY_ID"].ToString());
            }

            p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.Number, "prouteid", iRoute_Id },
                new object[] { ParameterDirection.Output, OracleType.Number, "vprocessid", "" },
                new object[] { ParameterDirection.Output, OracleType.Number, "vnextprocess", "" },
                new object[] { ParameterDirection.Output, OracleType.VarChar, "vsheetname", "" },
                new object[] { ParameterDirection.Output, OracleType.Number, "vstageid", "" },
                new object[] { ParameterDirection.Output, OracleType.Number, "vnodeid", "" },
                new object[] { ParameterDirection.Output, OracleType.Number, "vnextnode", "" },
            };

            d = ClientUtils.ExecuteProc("SAJET.SJ_GET_FIRSTPROCESS", p.ToArray());

            row = d.Tables[0].Rows[0];

            iProcess_Id = Convert.ToInt32(row["vprocessid"].ToString());

            if (row["vnextprocess"].ToString() != "0")
            {
                sNext_Process = row["vnextprocess"].ToString();
            }

            sSheet_Name = row["vsheetname"].ToString();
            iStage_Id = Convert.ToInt32(row["vstageid"].ToString());
            iNode_Id = Convert.ToInt64(row["vnodeid"].ToString());
            iNext_Node = Convert.ToInt64(row["vnextnode"].ToString());

            // Check
            for (int i = 0; i < Dgv_RC.Rows.Count; i++)
            {
                DataGridViewRow r = Dgv_RC.Rows[i];

                IsChecked = Convert.ToBoolean(r.Cells[0].FormattedValue);

                if (IsChecked)
                {
                    sRC = r.Cells[1].Value.ToString();

                    if (Convert.ToInt32(sRC.Substring(sRC.Length - 2, 1)) == 9)
                    {
                        SajetCommon.Show_Message("RC lotsize is full", 0);
                        return;
                    }
                    else
                    {
                        dTransferQty += Convert.ToDecimal(r.Cells[3].Value.ToString());
                    }
                }
            }

            if (dTransferQty > Convert.ToDecimal(Tb_AvailableQty.Text))
            {
                SajetCommon.Show_Message("Total Current Qty More Than Available Qty", 0);
                return;
            }

            for (int i = 0; i < Dgv_RC.Rows.Count; i++)
            {
                DataGridViewRow r = Dgv_RC.Rows[i];

                IsChecked = Convert.ToBoolean(r.Cells[0].FormattedValue);

                if (IsChecked)
                {
                    n++;

                    sRC = r.Cells[1].Value.ToString();

                    // New RC No
                    string s_RC_End = "";

                    s = @"
SELECT
    MAX(SUBSTR(RC_NO, - 2, 1) + 1) AS RC_END
FROM
    SAJET.G_RC_STATUS
WHERE
    RC_NO LIKE :RC_NO || '%'
";
                    p = new List<object[]>
                    {
                        new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", sRC.Substring(0, sRC.Length - 1) },
                    };

                    d = ClientUtils.ExecuteSQL(s, p.ToArray());

                    if (d.Tables[0].Rows.Count > 0)
                    {
                        row = d.Tables[0].Rows[0];

                        if (Convert.ToInt32(row["RC_END"].ToString()) > 9)
                        {
                            SajetCommon.Show_Message("RC lotsize is full", 0);
                            return;
                        }

                        s_RC_End = sRC.Substring(0, sRC.Length - 2) + row["RC_END"].ToString() + sRC.Substring(sRC.Length - 1, 1);
                    }

                    DateTime datExeTime = DateTime.Now;

                    long lTravel_Id = Convert.ToInt64(datExeTime.ToString("yyyyMMddHHmmssf"));

                    // Insert SAJET.G_RC_SPLIT
                    for (int j = 0; j < 2; j++)
                    {
                        s = @"
INSERT INTO SAJET.G_RC_SPLIT (
    RC_NO,
    RC_QTY,
    SOURCE_RC_NO,
    SOURCE_RC_QTY,
    TRAVEL_ID,
    PROCESS_ID,
    UPDATE_USERID,
    UPDATE_TIME
) VALUES (
    :RC_NO,
    :RC_QTY,
    :SOURCE_RC_NO,
    :SOURCE_RC_QTY,
    :TRAVEL_ID,
    :PROCESS_ID,
    :UPDATE_USERID,
    :UPDATE_TIME
)";
                        p = new List<object[]>
                        {
                            new object[] { ParameterDirection.Input, OracleType.VarChar, "SOURCE_RC_NO", sRC },
                            new object[] { ParameterDirection.Input, OracleType.Number, "SOURCE_RC_QTY", Convert.ToDecimal(Dgv_RC.Rows[i].Cells[3].Value.ToString()) },
                            new object[] { ParameterDirection.Input, OracleType.Number, "PROCESS_ID", iProcess_Id },
                            new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", rc_UserID },
                            new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime },
                        };

                        if (j == 0)
                        {
                            p.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", sRC });
                            p.Add(new object[] { ParameterDirection.Input, OracleType.Number, "RC_QTY", 0 });
                            p.Add(new object[] { ParameterDirection.Input, OracleType.Number, "TRAVEL_ID", lTravel_Id });
                        }
                        else
                        {
                            p.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", s_RC_End });
                            p.Add(new object[] { ParameterDirection.Input, OracleType.Number, "RC_QTY", Convert.ToDecimal(Dgv_RC.Rows[i].Cells[3].Value.ToString()) });
                            p.Add(new object[] { ParameterDirection.Input, OracleType.Number, "TRAVEL_ID", lTravel_Id });
                        }

                        ClientUtils.ExecuteSQL(s, p.ToArray());
                    }

                    // Insert SAJET.G_RC_STATUS
                    sSQL = @"
INSERT INTO SAJET.G_RC_STATUS (
    WORK_ORDER,
    RC_NO,
    PART_ID,
    VERSION,
    ROUTE_ID,
    FACTORY_ID,
    PDLINE_ID,
    STAGE_ID,
    NODE_ID,
    PROCESS_ID,
    SHEET_NAME,
    TERMINAL_ID,
    TRAVEL_ID,
    NEXT_NODE,
    NEXT_PROCESS,
    CURRENT_STATUS,
    CURRENT_QTY,
    IN_PROCESS_EMPID,
    IN_PROCESS_TIME,
    WIP_IN_QTY,
    WIP_IN_EMPID,
    WIP_IN_MEMO,
    WIP_IN_TIME,
    WIP_OUT_GOOD_QTY,
    WIP_OUT_SCRAP_QTY,
    WIP_OUT_EMPID,
    WIP_OUT_MEMO,
    WIP_OUT_TIME,
    OUT_PROCESS_EMPID,
    OUT_PROCESS_TIME,
    HAVE_SN,
    PRIORITY_LEVEL,
    UPDATE_USERID,
    UPDATE_TIME,
    CREATE_TIME,
    BATCH_ID,
    EMP_ID,
    BONUS_QTY,
    WORKTIME,
    INITIAL_QTY,
    RELEASE
) VALUES (
    :WORK_ORDER,
    :RC_NO,
    :PART_ID,
    :VERSION,
    :ROUTE_ID,
    :FACTORY_ID,
    :PDLINE_ID,
    :STAGE_ID,
    :NODE_ID,
    :PROCESS_ID,
    :SHEET_NAME,
    :TERMINAL_ID,
    :TRAVEL_ID,
    :NEXT_NODE,
    :NEXT_PROCESS,
    :CURRENT_STATUS,
    :CURRENT_QTY,
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,
    :HAVE_SN,
    :PRIORITY_LEVEL,
    :UPDATE_USERID,
    :UPDATE_TIME,
    :CREATE_TIME,
    NULL,
    :EMP_ID,
    :BONUS_QTY,
    :WORKTIME,
    :INITIAL_QTY,
    :RELEASE
)";
                    object[][] ParamsNew = new object[27][];
                    ParamsNew[0] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", Tb_WO_2.Text };
                    ParamsNew[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", s_RC_End };
                    ParamsNew[2] = new object[] { ParameterDirection.Input, OracleType.Number, "PART_ID", iPart_Id };
                    ParamsNew[3] = new object[] { ParameterDirection.Input, OracleType.VarChar, "VERSION", sVersion };
                    ParamsNew[4] = new object[] { ParameterDirection.Input, OracleType.Number, "ROUTE_ID", iRoute_Id };
                    ParamsNew[5] = new object[] { ParameterDirection.Input, OracleType.Number, "FACTORY_ID", iFactory_Id };
                    ParamsNew[6] = new object[] { ParameterDirection.Input, OracleType.Number, "PDLINE_ID", iLine_Id };
                    ParamsNew[7] = new object[] { ParameterDirection.Input, OracleType.Number, "STAGE_ID", iStage_Id };
                    ParamsNew[8] = new object[] { ParameterDirection.Input, OracleType.Number, "NODE_ID", iNode_Id };
                    ParamsNew[9] = new object[] { ParameterDirection.Input, OracleType.Number, "PROCESS_ID", iProcess_Id };
                    ParamsNew[10] = new object[] { ParameterDirection.Input, OracleType.VarChar, "SHEET_NAME", sSheet_Name };
                    ParamsNew[11] = new object[] { ParameterDirection.Input, OracleType.Number, "TERMINAL_ID", 0 };
                    ParamsNew[12] = new object[] { ParameterDirection.Input, OracleType.Number, "TRAVEL_ID", lTravel_Id };
                    ParamsNew[13] = new object[] { ParameterDirection.Input, OracleType.Number, "NEXT_NODE", iNext_Node };
                    ParamsNew[14] = new object[] { ParameterDirection.Input, OracleType.Number, "NEXT_PROCESS", sNext_Process };
                    ParamsNew[15] = new object[] { ParameterDirection.Input, OracleType.Number, "CURRENT_STATUS", 0 };
                    ParamsNew[16] = new object[] { ParameterDirection.Input, OracleType.Number, "CURRENT_QTY", Convert.ToDecimal(Dgv_RC.Rows[i].Cells[3].Value.ToString()) };
                    ParamsNew[17] = new object[] { ParameterDirection.Input, OracleType.Number, "HAVE_SN", 0 };
                    ParamsNew[18] = new object[] { ParameterDirection.Input, OracleType.Number, "PRIORITY_LEVEL", Convert.ToInt32(Dgv_RC.Rows[i].Cells[5].Value.ToString()) };
                    ParamsNew[19] = new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", rc_UserID };
                    ParamsNew[20] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime };
                    ParamsNew[21] = new object[] { ParameterDirection.Input, OracleType.DateTime, "CREATE_TIME", datExeTime };
                    ParamsNew[22] = new object[] { ParameterDirection.Input, OracleType.Number, "EMP_ID", rc_UserID };
                    ParamsNew[23] = new object[] { ParameterDirection.Input, OracleType.Number, "BONUS_QTY", 0 };
                    ParamsNew[24] = new object[] { ParameterDirection.Input, OracleType.Number, "WORKTIME", 0 };
                    ParamsNew[25] = new object[] { ParameterDirection.Input, OracleType.Number, "INITIAL_QTY", Convert.ToDecimal(Dgv_RC.Rows[i].Cells[3].Value.ToString()) };
                    ParamsNew[26] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RELEASE", "Y" };
                    ClientUtils.ExecuteSQL(sSQL, ParamsNew);

                    // Update SAJET.G_RC_STATUS
                    sSQL = @"
UPDATE SAJET.G_RC_STATUS
SET
    TRAVEL_ID = :TRAVEL_ID,
    CURRENT_STATUS = :CURRENT_STATUS,
    CURRENT_QTY = :CURRENT_QTY,
    UPDATE_USERID = :UPDATE_USERID,
    UPDATE_TIME = :UPDATE_TIME,
    BONUS_QTY = :BONUS_QTY,
    WORKTIME = :WORKTIME,
    INITIAL_QTY = :INITIAL_QTY,
    RELEASE = :RELEASE
WHERE
    RC_NO = :RC_NO
";

                    object[][] Params = new object[10][];
                    Params[0] = new object[] { ParameterDirection.Input, OracleType.Number, "TRAVEL_ID", lTravel_Id };
                    Params[1] = new object[] { ParameterDirection.Input, OracleType.Number, "CURRENT_STATUS", 12 };
                    Params[2] = new object[] { ParameterDirection.Input, OracleType.Number, "CURRENT_QTY", 0 };
                    Params[3] = new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", rc_UserID };
                    Params[4] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime };
                    Params[5] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", sRC };
                    Params[6] = new object[] { ParameterDirection.Input, OracleType.Number, "BONUS_QTY", 0 };
                    Params[7] = new object[] { ParameterDirection.Input, OracleType.Number, "WORKTIME", 0 };
                    Params[8] = new object[] { ParameterDirection.Input, OracleType.Number, "INITIAL_QTY", 0 };
                    Params[9] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RELEASE", "Y" };
                    dsTemp = ClientUtils.ExecuteSQL(sSQL, Params);

                    sSQL = @"
SELECT
    COUNT(*) AS COUNTS
FROM
    SAJET.G_RC_STATUS
WHERE
    CURRENT_STATUS IN (
        '0',
        '1',
        '2',
        '11'
    )
    AND WORK_ORDER IN (
        SELECT
            WORK_ORDER
        FROM
            SAJET.G_RC_STATUS
        WHERE
            RC_NO = :RC_NO
    )
";
                    var @param = new object[1][]
                    {
                        new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", sRC }
                    };

                    dsTemp = ClientUtils.ExecuteSQL(sSQL, @param);

                    if (dsTemp.Tables[0].Rows[0]["COUNTS"].ToString() == "0")
                    {
                        object[][] woParams = new object[3][];
                        sSQL = @"
UPDATE SAJET.G_WO_BASE
SET
    UPDATE_USERID = :UPDATE_USERID,
    UPDATE_TIME = :UPDATE_TIME,
    WO_STATUS = 6
WHERE
    WORK_ORDER IN (
        SELECT
            WORK_ORDER
        FROM
            SAJET.G_RC_STATUS
        WHERE
            RC_NO = :RC_NO
    )
";
                        woParams[0] = new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", rc_UserID };
                        woParams[1] = new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", datExeTime };
                        woParams[2] = new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", sRC };
                        ClientUtils.ExecuteSQL(sSQL, woParams);

                        sSQL = @"
INSERT INTO SAJET.G_HT_WO_BASE
    SELECT
        *
    FROM
        SAJET.G_WO_BASE
    WHERE
        WORK_ORDER IN (
            SELECT
                WORK_ORDER
            FROM
                SAJET.G_RC_STATUS
            WHERE
                RC_NO = :RC_NO
        )
";
                        ClientUtils.ExecuteSQL(sSQL, woParams);

                        sSQL = @"
INSERT INTO MESUSER.G_WO_STATUS (
    JOB_NUMBER,
    JOB_STATUS
)
    SELECT
        WORK_ORDER,
        8
    FROM
        SAJET.G_RC_STATUS
    WHERE
        RC_NO = :RC_NO
";
                        ClientUtils.ExecuteSQL(sSQL, woParams);
                    }
                }
            }

            if (n <= 0)
            {
                string sMsg = SajetCommon.SetLanguage("Please select RC/SN");
                SajetCommon.Show_Message(sMsg, 1);
                return;
            }
            else
            {
                object[][] woParams = new object[3][];
                sSQL = @"
UPDATE SAJET.G_WO_BASE
SET
    INPUT_QTY = INPUT_QTY + :TRANSFER_QTY,
    UPDATE_USERID = :UPDATE_USERID,
    UPDATE_TIME = SYSDATE,
    WO_STATUS = 3
WHERE
    WORK_ORDER = :WORK_ORDER
";
                woParams[0] = new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", rc_UserID };
                woParams[1] = new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", Tb_WO_2.Text };
                woParams[1] = new object[] { ParameterDirection.Input, OracleType.Number, "TRANSFER_QTY", dTransferQty };
                ClientUtils.ExecuteSQL(sSQL, woParams);

                sSQL = @"
INSERT INTO SAJET.G_HT_WO_BASE
    SELECT
        *
    FROM
        SAJET.G_WO_BASE
    WHERE
        WORK_ORDER = :WORK_ORDER 
";
                ClientUtils.ExecuteSQL(sSQL, woParams);
            }

            Dgv_RC.Rows.Clear();
            Rtb_HD.Clear();
            Tb_WO_2.Clear();
            Tb_AvailableQty.Clear();
            Tb_Spec1.Clear();
            Tb_Spec2.Clear();
            Tb_Version.Clear();
            Tb_RouteName.Clear();

            ShowData();

            string sMsg1 = SajetCommon.SetLanguage("Transfer was completed");
            SajetCommon.Show_Message(sMsg1, 3);
        }

        public void SetCombRCSN()
        {
            Cb_RC_SN.Items.Add(SajetCommon.SetLanguage("RC_NO"));
            Cb_RC_SN.SelectedIndex = 0;
        }

        public void ShowData()
        {
            string s = @"
SELECT
    RC_NO,
    SAJET.SJ_RC_STATUS(A.CURRENT_STATUS) RC_STATUS,
    DECODE(PROCESS_NAME, '', 'Group', PROCESS_NAME) PROCESS_NAME,
    A.CURRENT_QTY,
    A.PRIORITY_LEVEL
FROM
    SAJET.G_RC_STATUS   A,
    SAJET.SYS_PROCESS   B,
    SAJET.SYS_PART      C,
    SAJET.SYS_PDLINE    D,
    SAJET.G_WO_BASE     E
WHERE
    A.PROCESS_ID = B.PROCESS_ID (+)
    AND A.PROCESS_ID <> '0'
    AND A.PROCESS_ID IS NOT NULL
    AND A.PART_ID = C.PART_ID
    AND A.CURRENT_STATUS IN (
        11
    )
    AND A.PDLINE_ID = D.PDLINE_ID
    AND A.WORK_ORDER = E.WORK_ORDER
    AND A.RELEASE = 'Y'
    AND E.WO_STATUS = '3'
    AND E.WO_TYPE <> '轉投工單'
";
            var p = new List<object[]>();

            if (!string.IsNullOrEmpty(Tb_RC_SN.Text))
            {
                s += " AND A.RC_NO LIKE '%' || :RC_NO || '%' ";

                p.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", Tb_RC_SN.Text.Trim() });
            }

            if (!string.IsNullOrWhiteSpace(Tb_Line_2.Text))
            {
                s += " AND D.PDLINE_ID = :PDLINE_ID ";
                p.Add(new object[] { ParameterDirection.Input, OracleType.Number, "PDLINE_ID", Tb_Line_2.Text.Trim() });
            }

            if (!string.IsNullOrWhiteSpace(Tb_WO_1.Text))
            {
                s += " AND UPPER(A.WORK_ORDER) LIKE UPPER('%' || :WORK_ORDER || '%' ";
                p.Add(new object[] { ParameterDirection.Input, OracleType.Number, "WORK_ORDER", Tb_WO_1.Text.Trim() });
            }

            s += " ORDER BY A.RC_NO ";

            DataSet d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d.Tables[0].Rows.Count == 0)
            {
                string sMsg = SajetCommon.SetLanguage("No relevant data");
                label2.Text = sMsg;
                return;
            }
            else
            {
                label2.Visible = false;
            }

            //RC_dgv RC_NO赋值
            if (Cb_RC_SN.SelectedIndex == 0)
            {
                Dgv_RC.Columns[1].HeaderText = SajetCommon.SetLanguage("RC_NO");

                for (int i = 0; i < d.Tables[0].Rows.Count; i++)
                {
                    DataRow row = d.Tables[0].Rows[i];

                    Dgv_RC.Rows.Add();
                    Dgv_RC.Rows[i].Cells[0].Value = false;
                    Dgv_RC.Rows[i].Cells[1].Value = row["RC_NO"];
                    Dgv_RC.Rows[i].Cells[2].Value = row["PROCESS_NAME"];
                    Dgv_RC.Rows[i].Cells[3].Value = row["CURRENT_QTY"];
                    Dgv_RC.Rows[i].Cells[4].Value = row["RC_STATUS"];
                    Dgv_RC.Rows[i].Cells[5].Value = row["PRIORITY_LEVEL"];
                }
            }
        }
    }
}
