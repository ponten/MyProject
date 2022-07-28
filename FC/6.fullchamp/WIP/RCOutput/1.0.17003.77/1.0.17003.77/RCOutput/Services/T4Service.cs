using RCOutput.Enums;
using RCOutput.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;

namespace RCOutput.Services
{
    /// <summary>
    /// T4 爐 / T6 爐 的商業邏輯
    /// </summary>
    public static class T4Service
    {
        /// <summary>
        /// 爐子序號的選項要列印幾個
        /// </summary>
        private const int StoveSequenceCount = 10;

        /// <summary>
        /// 取得爐子順序號碼的下拉選單
        /// </summary>
        /// <returns></returns>
        public static List<string> GetSequencesComboBoxItems()
        {
            var result = new List<string>();

            for (int i = 1; i <= StoveSequenceCount; i++)
            {
                result.Add(i.ToString().PadLeft(2, '0'));
            }

            return result;
        }

        /// <summary>
        /// 檢查刷入的 T4 流程卡的料號是否為 10C 流程卡的料號的子階料號
        /// </summary>
        /// <param name="parentRC"></param>
        /// <param name="childRC"></param>
        /// <returns></returns>
        public static bool Is_TheBom_OfThePartNo_OfTheRuncards_Matches(string parentRC, string childRC)
        {
            string s = @"
/* 找 10C 流程卡的料號，是成品料號 */
WITH PARENT_PART AS (
    SELECT
        PART_ID
    FROM
        SAJET.G_RC_STATUS
    WHERE
        RC_NO = :PARENT_RC_NO
),
/* 找 T4 流程卡的料號，是子階料號 */
CHILD_PART AS (
    SELECT
        PART_ID
    FROM
        SAJET.G_RC_STATUS
    WHERE
        RC_NO = :CHILD_RC_NO
),
/* 找成品料號的 BOM，以及下面一階的料號 ID */
PARENT_BOM AS (
    SELECT
        B.PART_ID,
        A.ITEM_PART_ID,
        A.BOM_ID,
        A.PROCESS_ID
    FROM
        SAJET.SYS_BOM        A,
        SAJET.SYS_BOM_INFO   B,
        PARENT_PART          C
    WHERE
        A.BOM_ID = B.BOM_ID
        AND B.PART_ID = C.PART_ID
)
/* 檢查是不是子階料號 */
SELECT
    ITEM_PART_ID
FROM
    PARENT_BOM   A,
    CHILD_PART   B
WHERE
    A.ITEM_PART_ID = B.PART_ID
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PARENT_RC_NO", parentRC },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "CHILD_RC_NO", childRC }
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            return d != null && d.Tables[0].Rows.Count > 0;
        }

        /// <summary>
        /// 流程卡完成生產了沒
        /// </summary>
        /// <param name="T4RC"></param>
        /// <returns></returns>
        public static bool IsProductionComplete(string T4RC)
        {
            string s = @"
SELECT
    RC_NO
FROM
    SAJET.G_RC_STATUS
WHERE
    RC_NO = :RC_NO
    AND CURRENT_STATUS = 9
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", T4RC },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            return d != null && d.Tables[0].Rows.Count > 0;
        }

        /// <summary>
        /// 生產途程中有沒有 T4 製程
        /// </summary>
        /// <param name="T4RC"></param>
        /// <param name="routeDetail"></param>
        /// <returns></returns>
        public static bool HasT4inRoute(string T4RC, out DataTable routeDetail)
        {
            string s = @"
WITH RC_ROUTE_ID AS (
    SELECT
        ROUTE_ID,
        NODE_ID
    FROM
        SAJET.G_RC_STATUS
    WHERE
        RC_NO = :RC_NO
), ROUTE_DETAIL AS (
    SELECT
        ROWNUM IDX,
        A.NODE_CONTENT,
        A.NODE_ID,
        A.NEXT_NODE_ID
    FROM
        SAJET.SYS_RC_ROUTE_DETAIL   A,
        RC_ROUTE_ID                 B
    WHERE
        A.ROUTE_ID = B.ROUTE_ID
    START WITH
        A.NODE_CONTENT = 'START'
    CONNECT BY
        PRIOR A.NEXT_NODE_ID = A.NODE_ID
)
SELECT
    A.IDX,
    B.PROCESS_ID,
    B.PROCESS_NAME,
    A.NODE_ID,
    NVL(C.OPTION2, 'N') ""T4"",
    NVL(C.OPTION3, 'N') ""T6""
FROM
    ROUTE_DETAIL               A,
    SAJET.SYS_PROCESS          B,
    SAJET.SYS_PROCESS_OPTION   C
WHERE
    A.NODE_CONTENT = TO_CHAR(B.PROCESS_ID)
    AND B.PROCESS_ID = C.PROCESS_ID (+)
ORDER BY
    A.IDX
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", T4RC },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            routeDetail = d.Tables[0];

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                return routeDetail.Rows.Cast<DataRow>()
                    .Any(x => x["T4"].ToString() == "Y");
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 取得在 T4 製程蒐集到的機台資訊
        /// </summary>
        /// <param name="T4RC"></param>
        /// <param name="T4ProcessID"></param>
        /// <returns></returns>
        public static List<T4MachineModel> GetT4MachineList(string T4RC, string T4ProcessID)
        {
            string s = @"
SELECT
    A.RC_NO,
    A.NODE_ID,
    A.MACHINE_ID,
    A.LOAD_QTY,
    A.DATE_CODE,
    A.STOVE_SEQ,
    C.PROCESS_CODE,
    C.PROCESS_NAME,
    TRIM(D.MACHINE_CODE) MACHINE_CODE,
    TRIM(D.MACHINE_DESC) MACHINE_DESC
FROM
    SAJET.G_RC_TRAVEL_MACHINE_DOWN   A,
    SAJET.SYS_RC_ROUTE_DETAIL        B,
    SAJET.SYS_PROCESS                C,
    SAJET.SYS_MACHINE                D
WHERE
    A.RC_NO = :RC_NO
    AND C.PROCESS_ID = :PROCESS_ID
    AND B.NODE_CONTENT = TO_CHAR(:PROCESS_ID)
    AND A.NODE_ID = B.NODE_ID
    AND A.MACHINE_ID = D.MACHINE_ID
    AND A.DATE_CODE IS NOT NULL
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", T4RC },
                new object[] { ParameterDirection.Input, OracleType.Number, "PROCESS_ID", T4ProcessID },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            var T4MachineList = new List<T4MachineModel>();

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in d.Tables[0].Rows)
                {
                    T4MachineList.Add(new T4MachineModel
                    {
                        RC_NO_T4 = T4RC,
                        PROCESS_ID_T4 = int.Parse(T4ProcessID),
                        NODE_ID_T4 = row["NODE_ID"].ToString(),
                        PROCESS_CODE = row["PROCESS_CODE"].ToString(),
                        PROCESS_NAME = row["PROCESS_NAME"].ToString(),
                        MACHINE_ID = int.Parse(row["MACHINE_ID"].ToString()),
                        MACHINE_CODE = row["MACHINE_CODE"].ToString(),
                        MACHINE_DESC = row["MACHINE_DESC"].ToString(),
                        LOAD = int.Parse(row["LOAD_QTY"].ToString()),
                        DATE_CODE = row["DATE_CODE"].ToString(),
                        STOVE_SEQ = row["STOVE_SEQ"].ToString(),
                    });
                }

                return T4MachineList;
            }
            else
            {
                return new List<T4MachineModel>();
            }
        }

        /// <summary>
        /// 取得已經綁定的 T4 機台的加工數量資料
        /// </summary>
        /// <param name="T4RC"></param>
        /// <param name="T4ProcessID"></param>
        /// <returns></returns>
        public static List<T4MachineModel> GetT4MachineUsedRecords(string T4RC, string T4ProcessID)
        {
            string s = @"
SELECT
    RC_NO_T4,
    PROCESS_ID_T4,
    NODE_ID_T4,
    MACHINE_ID,
    SUM(BINDING_QTY) TOTAL_BINDING_QTY
FROM
    SAJET.G_MACHINE_T4_BINDING
WHERE
    RC_NO_T4 = :RC_NO
    AND PROCESS_ID_T4 = :PROCESS_ID
GROUP BY
    RC_NO_T4,
    PROCESS_ID_T4,
    NODE_ID_T4,
    MACHINE_ID
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", T4RC },
                new object[] { ParameterDirection.Input, OracleType.Number, "PROCESS_ID", T4ProcessID },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                var T4List = new List<T4MachineModel>();

                foreach (DataRow row in d.Tables[0].Rows)
                {
                    T4List.Add(new T4MachineModel
                    {
                        RC_NO_T4 = row["RC_NO_T4"].ToString(),
                        PROCESS_ID_T4 = int.Parse(row["PROCESS_ID_T4"].ToString()),
                        NODE_ID_T4 = row["NODE_ID_T4"].ToString(),
                        MACHINE_ID = int.Parse(row["MACHINE_ID"].ToString()),
                        BOUND_QTY = int.Parse(row["TOTAL_BINDING_QTY"].ToString()),
                        BINDING_QTY = 0
                    });
                }

                return T4List;
            }
            else
            {
                return new List<T4MachineModel>();
            }
        }

        /// <summary>
        /// 把 10C 流程卡資訊、T4 機台已經綁定的數量 填入主清單
        /// </summary>
        /// <param name="T4Machine"></param>
        /// <param name="T4Used"></param>
        /// <returns></returns>
        public static List<T4MachineModel> Merge(List<T4MachineModel> T4Machine, List<T4MachineModel> T4Used, DataRow RcInFo)
        {
            T4Machine.ForEach(x =>
            {
                // 10C 流程卡（就是現在的流程卡）的必要資訊
                x.RC_NO_10C = RcInFo["RC_NO"].ToString();

                x.PROCESS_ID_10C = int.Parse(RcInFo["PROCESS_ID"].ToString());

                x.NODE_ID_10C = RcInFo["NODE_ID"].ToString();

                // 找到指定機台，填入已綁定數量；沒找到指定機台，不做任何事。
                var found = T4Used
                .Where(e =>
                {
                    return
                    e.RC_NO_T4 == x.RC_NO_T4 &&
                    e.PROCESS_ID_T4 == x.PROCESS_ID_T4 &&
                    e.NODE_ID_T4 == x.NODE_ID_T4 &&
                    e.MACHINE_ID == x.MACHINE_ID;
                }).ToList();

                if (found.Count > 0)
                {
                    x.BOUND_QTY = found.First().BOUND_QTY;
                }
            });

            return T4Machine;
        }

        /// <summary>
        /// 檢查綁定數量
        /// </summary>
        /// <param name="RcInFo"></param>
        /// <param name="T4List"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool CheckBindingQty(ref DataRow RcInFo, ref List<T4MachineModel> T4List, out string message)
        {
            message = string.Empty;

            int boundQty = 0;

            var errorRC = new List<string>();

            //// 必須綁定 T4 機台 
            //if (T4List.Count <= 0)
            //{
            //    message = SajetClass.SajetCommon.SetLanguage(MessageEnum.MustBindT4To10C.ToString());

            //    return false;
            //}

            // 這次綁定數量，加上已經綁定的數量，不能超過機台加工數量
            T4List.ForEach(x =>
            {
                if (x.BINDING_QTY + x.BOUND_QTY > x.LOAD)
                {
                    errorRC.Add(x.RC_NO_T4);
                }

                boundQty += x.BINDING_QTY ?? 0;
            });

            if (errorRC.Count > 0)
            {
                message = SajetClass.SajetCommon.SetLanguage(MessageEnum.BindingQtyMoreThanT4Load.ToString());

            //75版沒有
            //    return false;
            //}

            //// 綁定數量必須等於流程卡當前數量
            //int currentQty = int.Parse(RcInFo["CURRENT_QTY"].ToString());

            //if (boundQty != currentQty)
            //{
            //    message = SajetClass.SajetCommon.SetLanguage(MessageEnum.BindingQtyShouldMatchCurrentQty.ToString());

                return false;
            }

            return true;
        }

        /// <summary>
        /// 儲存 10C 流程卡與 T4 機台的綁定加工數量的資訊
        /// </summary>
        /// <param name="now">（統一設定的）現在時間</param>
        /// <param name="updateUserID">更新人員 ID</param>
        /// <param name="check_bom">是否檢查 BOM</param>
        /// <param name="T4List">要綁定的流程卡的清單</param>
        public static void SaveT4Bindings(DateTime now, string updateUserID, bool check_bom, ref List<T4MachineModel> T4List)
        {
            string s = string.Empty;

            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", updateUserID },
                new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", now },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "CHECK_BOM", check_bom ? "Y" : "N" },
            };

            int i = 1;

            foreach (var T4Machine in T4List)
            {
                s += $@"
  INTO SAJET.G_MACHINE_T4_BINDING (
    RC_NO_10C,
    PROCESS_ID_10C,
    NODE_ID_10C,
    RC_NO_T4,
    PROCESS_ID_T4,
    NODE_ID_T4,
    MACHINE_ID,
    BINDING_QTY,
    CHECK_BOM,
    UPDATE_USERID,
    UPDATE_TIME
) VALUES (
    :RC_NO_10C_{i},
    :PROCESS_ID_10C_{i},
    :NODE_ID_10C_{i},
    :RC_NO_T4_{i},
    :PROCESS_ID_T4_{i},
    :NODE_ID_T4_{i},
    :MACHINE_ID_{i},
    :BINDING_QTY_{i},
    :CHECK_BOM,
    :UPDATE_USERID,
    :UPDATE_TIME
)";
                p.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, $"RC_NO_10C_{i}", T4Machine.RC_NO_10C });
                p.Add(new object[] { ParameterDirection.Input, OracleType.Number, $"PROCESS_ID_10C_{i}", T4Machine.PROCESS_ID_10C });
                p.Add(new object[] { ParameterDirection.Input, OracleType.Number, $"NODE_ID_10C_{i}", T4Machine.NODE_ID_10C });
                p.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, $"RC_NO_T4_{i}", T4Machine.RC_NO_T4 });
                p.Add(new object[] { ParameterDirection.Input, OracleType.Number, $"PROCESS_ID_T4_{i}", T4Machine.PROCESS_ID_T4 });
                p.Add(new object[] { ParameterDirection.Input, OracleType.Number, $"NODE_ID_T4_{i}", T4Machine.NODE_ID_T4 });
                p.Add(new object[] { ParameterDirection.Input, OracleType.Number, $"MACHINE_ID_{i}", T4Machine.MACHINE_ID });
                p.Add(new object[] { ParameterDirection.Input, OracleType.Number, $"BINDING_QTY_{i}", T4Machine.BINDING_QTY });

                i++;
            }

            s = $@"
INSERT ALL
{s}
SELECT 1 FROM DUAL
";
            ClientUtils.ExecuteSQL(s, p.ToArray());
        }

        /// <summary>
        /// 清單排序
        /// </summary>
        /// <param name="T4List"></param>
        /// <returns></returns>
        public static List<T4MachineModel> Sort(List<T4MachineModel> T4List)
        {
            return T4List
                .OrderBy(x => x.RC_NO_T4)
                .ThenBy(x => x.MACHINE_CODE)
                .ThenBy(x => x.STOVE_SEQ)
                .ToList();
        }
    }
}
