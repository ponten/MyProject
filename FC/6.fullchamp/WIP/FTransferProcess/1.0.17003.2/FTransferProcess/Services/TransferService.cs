using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;

namespace FTransferProcess.Services
{
    internal static class TransferService
    {
        /// <summary>
        /// 取得生產途程的節點細節
        /// </summary>
        /// <param name="route_id">生產途程 ID</param>
        /// <returns></returns>
        internal static List<DataRow> GetRouteDetailList(string route_id)
        {
            string s = @"
SELECT
    NODE_ID,
    NODE_TYPE,
    NODE_CONTENT,
    GROUP_ID,
    NEXT_NODE_ID,
    LINK_NAME
FROM
    SAJET.SYS_RC_ROUTE_DETAIL
WHERE
    ROUTE_ID = :ROUTE_ID
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "ROUTE_ID", route_id },
            };

            DataSet d = ClientUtils.ExecuteSQL(s, p.ToArray());

            List<DataRow> route_detail = d.Tables[0].Rows.Cast<DataRow>().ToList();

            return route_detail;
        }

        /// <summary>
        /// 取得下一個製程節點 ID 的清單
        /// </summary>
        /// <param name="route_detail">生產途程的節點細節</param>
        /// <param name="node_id">生產途程節點 ID</param>
        /// <param name="rc_no">流程卡號</param>
        /// <returns></returns>
        internal static List<string> GetNextNodeIDs(List<DataRow> route_detail, string node_id, string rc_no)
        {
            var next_node_id_list = new List<string>();

            var temp_node_id_list = new List<string>();

            List<object[]> p;

            string s;

            DataSet d;

            // 用 NODE_ID 查詢 NEXT_NODE_ID、GROUP_ID，看哪一個欄位有值
            DataRow this_node = route_detail.First(x => x["NODE_ID"].ToString() == node_id);

            // 得到 NEXT_NODE_ID
            if (!string.IsNullOrWhiteSpace(this_node["NEXT_NODE_ID"].ToString()))
            {
                temp_node_id_list.Add(this_node["NEXT_NODE_ID"].ToString());
            }
            // 得到 GROUP_ID
            else if (!string.IsNullOrWhiteSpace(this_node["GROUP_ID"].ToString()))
            {
                // 用 GROUP_ID 當 NODE_ID，查 NODE_TYPE
                DataRow group_id_row = route_detail
                    .First(x => x["NODE_ID"].ToString() == this_node["GROUP_ID"].ToString());

                string node_type_a = group_id_row["NODE_TYPE"].ToString();

                // NODE_TYPE 是 2，AND 群組
                if (node_type_a == "2")
                {
                    // 列出相同 GROUP_ID 的其他 NODE_ID
                    var group_node_id_list = route_detail
                        .Where(x => x["GROUP_ID"].ToString() == this_node["GROUP_ID"].ToString())
                        .Select(x => x["NODE_ID"].ToString())
                        .ToList();

                    // 用 NODE_ID 和 RC_NO 到 G_RC_TRAVEL 查詢，把有匹配的 NODE_ID 過濾掉
                    s = @"
SELECT
    NODE_ID
FROM
    SAJET.G_RC_TRAVEL
WHERE
    RC_NO = :RC_NO
    AND NODE_ID IN (NODE_S)
UNION
SELECT
    NODE_ID
FROM
    SAJET.G_RC_STATUS
WHERE
    RC_NO = :RC_NO
    AND NODE_ID IN (NODE_S)
";
                    p = new List<object[]>
                    {
                        new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", rc_no },
                    };

                    var param_name_list = new List<string>
                    {
                        "0"
                    };

                    for (int i = 0; i < group_node_id_list.Count; i++)
                    {
                        param_name_list.Add($":PARAM_{i}");

                        p.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, $"PARAM_{i}", group_node_id_list[i] });
                    }

                    s = s.Replace("NODE_S", string.Join(",", param_name_list));

                    d = ClientUtils.ExecuteSQL(s, p.ToArray());

                    if (d != null && d.Tables[0].Rows.Count > 0)
                    {
                        List<string> macth_node_id_list
                            = d.Tables[0].Rows.Cast<DataRow>()
                            .Select(x => x["NODE_ID"].ToString())
                            .ToList();

                        var available_node_id_list
                            = group_node_id_list
                            .Except(macth_node_id_list)
                            .ToList();

                        // 還有剩其他 NODE_ID
                        if (available_node_id_list.Count > 0)
                        {
                            // 列出來給使用者選擇
                            next_node_id_list.AddRange(available_node_id_list);
                        }
                        // 沒有其他 NODE_ID
                        else
                        {
                            // 用 GROUP_ID 當 NODE_ID，取得 NEXT_NODE_ID
                            temp_node_id_list.Add(group_id_row["NEXT_NODE_ID"].ToString());
                        }

                    }
                    // 還有剩其他 NODE_ID
                    else
                    {
                        // 列出來給使用者選擇
                        next_node_id_list.AddRange(group_node_id_list);
                    }

                }
                // NODE_TYPE 是 3，OR 群組
                else if (node_type_a == "3")
                {
                    // 取得 NEXT_NODE_ID
                    temp_node_id_list.Add(group_id_row["NEXT_NODE_ID"].ToString());
                }

            }

            // 用 NEXT_NODE_ID 當 NODE_ID，查詢 NODE_TYPE
            if (temp_node_id_list.Count == 1)
            {
                string node_type_b
                    = route_detail
                    .First(x => x["NODE_ID"].ToString() == temp_node_id_list[0])
                    ["NODE_TYPE"]
                    .ToString();

                // NODE_TYPE 是 1
                if (node_type_b == "1")
                {
                    next_node_id_list.Add(temp_node_id_list[0]);
                }
                // NODE_TYPE 是 2 或 3
                else if (node_type_b == "2" || node_type_b == "3")
                {
                    // 用 NEXT_NODE_ID 當 GROUP_ID，列出 NODE_ID
                    var temp = route_detail
                        .Where(x => x["GROUP_ID"].ToString() == temp_node_id_list[0])
                        .Select(x => x["NODE_ID"].ToString())
                        .ToList();

                    next_node_id_list.AddRange(temp);
                }
                // NODE_TYPE 是 9
                else if (node_type_b == "9")
                {
                    next_node_id_list.Add(temp_node_id_list[0]);
                }
            }

            // 列出來的 NODE_ID 就是解答
            return next_node_id_list;
        }
    }
}
