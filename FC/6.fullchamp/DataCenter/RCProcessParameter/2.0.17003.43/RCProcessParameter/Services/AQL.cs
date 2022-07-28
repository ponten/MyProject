using RCProcessParam.Models;
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
    /// AQL 頁籤
    /// </summary>
    partial class AQL
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
        public void ShowListView(string g_sPartId, string lbText, ref ListView lvAQL)
        {
            lvAQL.Clear();

            try
            {
                string sSQL = $@"
SELECT
    B.SAMPLING_TYPE,
    B.SAMPLING_DESC,
    A.ENABLED
FROM
    SAJET.SYS_PART_QC_PLAN       A,
    SAJET.SYS_QC_SAMPLING_PLAN   B
WHERE
    A.PART_ID = '{g_sPartId}'
    AND A.PROCESS_ID = (
        SELECT
            PROCESS_ID
        FROM
            SAJET.SYS_PROCESS
        WHERE
            TRIM(PROCESS_NAME) = TRIM('{lbText}')
    )
    AND A.SAMPLING_ID = B.SAMPLING_ID
";
                DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);

                lvAQL.Sorting = SortOrder.None;
                for (int i = 0; i < dsTemp.Tables[0].Columns.Count; i++)
                {
                    lvAQL.Columns.Add(SajetCommon.SetLanguage(dsTemp.Tables[0].Columns[i].Caption));
                }
                for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                {
                    lvAQL.Items.Add(dsTemp.Tables[0].Rows[i][0].ToString());
                    for (int j = 1; j < dsTemp.Tables[0].Columns.Count; j++)
                    {
                        //if (j > 1)
                        //    lvAQL.Items[i].SubItems.Add(dataConvert(j, dsTemp.Tables[0].Rows[i][j].ToString()));
                        //else
                            lvAQL.Items[i].SubItems.Add(dsTemp.Tables[0].Rows[i][j].ToString());
                    }
                    lvAQL.Items[i].ImageIndex = 0;
                }
                for (int i = 0; i < dsTemp.Tables[0].Columns.Count; i++)
                    lvAQL.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.HeaderSize);
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
        /// <param name="dgvPreview"></param>
        public void ShowPreview(string g_sPartId, ref DataGridView dgvPreview)
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
        PART_ID = '{g_sPartId}'
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
), PROCESS_IDS AS (
    SELECT
        A.PROCESS_ID
    FROM
        SAJET.SYS_PROCESS    A,
        SAJET.SYS_RC_ROUTE   B,
        ROUTE_NODES          C,
        SAJET.SYS_PART       D
    WHERE
        B.ROUTE_ID = C.ROUTE_ID
        AND C.ROUTE_ID = D.ROUTE_ID
        AND TO_CHAR(A.PROCESS_ID) = C.NODE_CONTENT
        AND A.ENABLED = 'Y'
        AND B.ENABLED = 'Y'
        AND D.PART_ID = '{g_sPartId}'
    ORDER BY
        C.IDX
), SAMPLING_IDS AS (
    SELECT
        SAMPLING_ID,
        PROCESS_ID,
        ENABLED
    FROM
        SAJET.SYS_PART_QC_PLAN
    WHERE
        PART_ID = '{g_sPartId}'
)
SELECT
    D.PROCESS_ID,
    D.PROCESS_NAME,
    C.SAMPLING_TYPE,
    C.SAMPLING_DESC,
    A.ENABLED
FROM
    SAMPLING_IDS                 A,
    PROCESS_IDS                  B,
    SAJET.SYS_QC_SAMPLING_PLAN   C,
    SAJET.SYS_PROCESS            D
WHERE
    A.PROCESS_ID = B.PROCESS_ID
    AND C.SAMPLING_ID = A.SAMPLING_ID
    AND D.PROCESS_ID = A.PROCESS_ID
    AND C.ENABLED = 'Y'
";

            var d = ClientUtils.ExecuteSQL(sSQL);

            if (d != null)
            {
                dgvPreview.DataSource = d;

                dgvPreview.DataMember = d.Tables[0].ToString();
            }

            foreach (DataGridViewColumn column in dgvPreview.Columns)
            {
                if (column.Name.Trim().ToUpper() == "PROCESS_ID")
                {
                    column.Visible = false;
                }
                column.HeaderText = SajetCommon.SetLanguage(column.HeaderText);
            }
        }

        /// <summary>
        /// 刪除 AQL 資料
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
        /// 取得資料筆數
        /// </summary>
        /// <param name="Part_ID"></param>
        /// <param name="Process_ID"></param>
        /// <returns></returns>
        public int GetDataRow(string Part_ID, string Process_ID)
        {
            string sSQL = $@" SELECT SAMPLING_TYPE, SAMPLING_DESC
                                FROM SAJET.SYS_QC_SAMPLING_PLAN
                               WHERE SAMPLING_ID = (
                                      SELECT DISTINCT A.SAMPLING_ID
                                        FROM SAJET.SYS_PART_QC_PLAN A                                                
                                       WHERE A.PART_ID = '{Part_ID}'
                                         AND A.ENABLED = 'Y'
                                         AND A.PROCESS_ID = '{Process_ID}')
                                 AND ENABLED = 'Y' ";

            DataSet search = ClientUtils.ExecuteSQL(sSQL);

            return search.Tables[0].Rows.Count;
        }

        /// <summary>
        /// 新增 AQL 資料
        /// </summary>
        /// <param name="model">QC_PlanModel</param>
        public void InsertAQLData(QC_PlanModel model)
        {
            string sSQL = $@" INSERT INTO SAJET.SYS_PART_QC_PLAN 
                                     (
                                       PART_ID, 
                                       PROCESS_ID, 
                                       SAMPLING_ID, 
                                       UPDATE_USERID, 
                                       UPDATE_TIME, 
                                       ENABLED)
                              VALUES (
                                       '{model.Part_Id}',
                                       '{model.Process_Id}',
                                       '{model.Sampling_Id}',
                                       '{model.User_Id}',
                                       sysdate,
                                       'Y'
                                      ) ";

            ClientUtils.ExecuteSQL(sSQL);

            InsertHTData(model);
        }

        /// <summary>
        ///  更新 AQL 資料
        /// </summary>
        /// <param name="Model"></param>
        public void UpdateAQLData(QC_PlanModel Model)
        {
            string sSQL = $@" UPDATE SAJET.SYS_PART_QC_PLAN 
                                 SET SAMPLING_ID = '{Model.Sampling_Id}',
                                     UPDATE_USERID = '{Model.User_Id}',
                                     UPDATE_TIME = SYSDATE,
                                     ENABLED = 'Y'
                               WHERE PART_ID = '{Model.Part_Id}'
                                 AND PROCESS_ID = '{Model.Process_Id}' ";

            ClientUtils.ExecuteSQL(sSQL);

            InsertHTData(Model);
        }

        /// <summary>
        /// 啟用 / 停用 AQL 資料
        /// </summary>
        /// <param name="Model"></param>
        public void EnableAQLData(QC_PlanModel Model,string enable_value)
        {
            if (enable_value == "N")
            {
                enable_value = "Y";
            }
            else
            {
                enable_value = "N";
            }

            string sSQL = $@" UPDATE SAJET.SYS_PART_QC_PLAN 
                                 SET UPDATE_USERID = '{Model.User_Id}',
                                     UPDATE_TIME = SYSDATE,
                                     ENABLED = '{enable_value}'
                               WHERE PART_ID = '{Model.Part_Id}'
                                 AND PROCESS_ID = '{Model.Process_Id}'
                                 AND SAMPLING_ID = '{Model.Sampling_Id}' ";

            ClientUtils.ExecuteSQL(sSQL);

            InsertHTData(Model);
        }

        internal string GetAQLEnableValue(ProcessViewModel process_info)
        {
            string s = @"
SELECT
    ENABLED
FROM
    SAJET.SYS_PART_QC_PLAN
WHERE
    PART_ID = :PART_ID
    AND PROCESS_ID = :PROCESS_ID
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.Number, "PART_ID", process_info.PART_ID },
                new object[] { ParameterDirection.Input, OracleType.Number, "PROCESS_ID", process_info.PROCESS_ID },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                return d.Tables[0].Rows[0]["ENABLED"].ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 寫入 SYS_HT_PART_QC_PLAN
        /// </summary>
        /// <param name="model"></param>
        private void InsertHTData(QC_PlanModel model)
        {
            // 寫入 SYS_HT_PART_QC_PLAN
            string sSQL = $@" INSERT INTO SAJET.SYS_HT_PART_QC_PLAN 
                              SELECT * 
                                FROM SAJET.SYS_PART_QC_PLAN 
                               WHERE PART_ID = '{model.Part_Id}'
                                 AND PROCESS_ID = '{model.Process_Id}'
                                 AND SAMPLING_ID = '{model.Sampling_Id}'
                                 AND UPDATE_USERID = '{model.User_Id}' ";

            ClientUtils.ExecuteSQL(sSQL);
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
