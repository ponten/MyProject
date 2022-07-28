using SajetClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RCProcessParam
{
    /// <summary>
    /// QC 資料收集
    /// </summary>
    partial class QCCollection
    {
        /// <summary>
        /// 檢查權限
        /// </summary>
        /// <param name="EmpID"></param>
        /// <param name="fun">要檢查 QC_AQL 權限</param>
        /// <returns></returns>
        public bool Check_Privilege(string EmpID, string fun)
        {
            try
            {
                string sPrivilege = ClientUtils.GetPrivilege(EmpID, fun, ClientUtils.fProgramName).ToString();

                string sSQL = $@"
                                SELECT
                                       SAJET.SJ_PRIVILEGE_DEFINE('{fun}', '{sPrivilege}') TENABLED
                                  FROM
                                       DUAL
                                ";
                DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);

                string sRes = dsTemp.Tables[0].Rows[0]["TENABLED"].ToString();
                return (sRes == "Y");
            }
            catch (Exception ex)
            {
                SajetCommon.Show_Message("Function:SAJET.SJ_PRIVILEGE_DEFINE" + Environment.NewLine + ex.Message, 0);
                return false;
            }

        }

        /// <summary>
        /// 將資料呈現在 ListView
        /// </summary>
        /// <param name="g_sPartId"></param>
        /// <param name="lbText"></param>
        /// <param name="lvQCCollection"></param>
        public void ShowListView(string g_sPartId, string lbText, ref ListView lvQCCollection)
        {
            lvQCCollection.Clear();

            try
            {
                string sSQL = " SELECT A.ITEM_SEQ, A.ITEM_NAME, A.ITEM_PHASE, A.VALUE_TYPE, A.INPUT_TYPE, "
                           + " A.CONVERT_TYPE, A.NECESSARY, A.VALUE_DEFAULT \"Input value\", A.VALUE_LIST, A.PRINT, "
                           + " A.COLUMN_ITEM, A.ROW_ITEM, D.EMP_NO, A.UPDATE_TIME "
                          + " FROM SAJET.SYS_PART_QC_ITEM A, "
                          + " SAJET.SYS_PROCESS C, SAJET.SYS_EMP D, SAJET.SYS_UNIT E "
                          + " WHERE A.PART_ID = '" + g_sPartId + "' AND A.PROCESS_ID = C.PROCESS_ID AND A.UPDATE_USERID = D.EMP_ID "
                          + " AND ITEM_TYPE = 1 "
                          //+ " AND PART_NO = '" + editPart.Text + "' "
                          + " AND A.UNIT_ID = E.UNIT_ID(+)   "
                          + " AND TRIM(PROCESS_NAME) = TRIM('" + lbText + "') ORDER BY A.ITEM_SEQ, A.ITEM_NAME";
                DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);

                lvQCCollection.Sorting = SortOrder.None;
                for (int i = 0; i < dsTemp.Tables[0].Columns.Count; i++)
                {
                    lvQCCollection.Columns.Add(SajetCommon.SetLanguage(dsTemp.Tables[0].Columns[i].Caption));
                }
                for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                {
                    lvQCCollection.Items.Add(dsTemp.Tables[0].Rows[i][0].ToString());
                    for (int j = 1; j < dsTemp.Tables[0].Columns.Count; j++)
                    {
                        if (j > 1)
                            lvQCCollection.Items[i].SubItems.Add(dataConvert(j, dsTemp.Tables[0].Rows[i][j].ToString()));
                        else
                            lvQCCollection.Items[i].SubItems.Add(dsTemp.Tables[0].Rows[i][j].ToString());
                    }
                    lvQCCollection.Items[i].ImageIndex = 0;
                }
                for (int i = 0; i < dsTemp.Tables[0].Columns.Count; i++)
                    lvQCCollection.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 呈現預覽資料
        /// </summary>
        /// <param name="g_sPartId"></param>
        /// <param name="item_type"></param>
        /// <param name="columnName"></param>
        /// <param name="dgvPreview"></param>
        public void ShowPreview(string g_sPartId, int item_type, string columnName, ref DataGridView dgvPreview)
        {
            string sSQL = $@"
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
    SAJET.SYS_PART_QC_ITEM   A,
    SAJET.SYS_PROCESS        C,
    ROUTE_NODES F -- 遞迴找途程前後順序
WHERE
    A.PART_ID = :PART_ID
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
                new object[] { ParameterDirection.Input, OracleType.Number, "part_id", g_sPartId },
                new object[] { ParameterDirection.Input, OracleType.Number, "item_type", item_type },
            };

            var d = ClientUtils.ExecuteSQL(sSQL, p.ToArray());

            if (d != null)
            {
                dgvPreview.DataSource = d;

                dgvPreview.DataMember = d.Tables[0].ToString();
            }

            var hiddenCol = new List<string>
            {
                "item_id",
                "process_id",
            };

            foreach (DataGridViewColumn column in dgvPreview.Columns)
            {
                if (hiddenCol?.Any(x => x.Trim().ToUpper().Contains(column.Name.Trim().ToUpper())) ?? false)
                {
                    column.Visible = false;
                }

                column.HeaderText = SajetCommon.SetLanguage(column.HeaderText);
            }
        }

        /// <summary>
        /// 刪除 QC 資料
        /// </summary>
        /// <param name="g_sPartId"></param>
        /// <param name="g_sProcessId"></param>
        /// <param name="seq"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool DeleteItem(string g_sPartId, string g_sProcessId, string seq, string name, string type)
        {
            try
            {
                string sSQL = $@" INSERT INTO SAJET.SYS_HT_PART_QC_ITEM 
                                  SELECT * 
                                    FROM SAJET.SYS_PART_QC_ITEM 
                                   WHERE PART_ID = '{g_sPartId}'
                                     AND PROCESS_ID = '{g_sProcessId}'
                                     AND ITEM_TYPE = '{type}'
                                     AND ITEM_SEQ = '{seq}'
                                     AND ITEM_NAME = '{name}' ";

                ClientUtils.ExecuteSQL(sSQL);

                sSQL = "DELETE SAJET.SYS_PART_QC_ITEM "
                          + " WHERE PART_ID = '" + g_sPartId + "' "
                          + " AND PROCESS_ID = '" + g_sProcessId + "' "
                          + " AND ITEM_TYPE = '" + type + "' "
                          + " AND ITEM_SEQ = '" + seq + "' "
                          + " AND ITEM_NAME = '" + name + "' ";
                ClientUtils.ExecuteSQL(sSQL);

                return true;
            }
            catch (Exception)
            {
                SajetCommon.Show_Message(SajetCommon.SetLanguage("Delete Item Error."), 0);
                return false;
            }
        }

        /// <summary>
        /// 資料轉換
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data"></param>
        /// <returns></returns>
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
    }
}
