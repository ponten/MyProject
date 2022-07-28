using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;

namespace MachineAAR
{
    static class Services
    {
        /// <summary>
        /// 根據使用者的報工權限取得可用資料的區段與製程，用作搜尋下拉選單的資料來源。
        /// </summary>
        public static bool FindStageAndProcess(string userID, ref List<StageModel> stagesList)
        {
            // To avoid CS1628:
            // Cannot use in ref or out parameter 'parameter' inside an anonymous method, lambda expression, or query expression.
            var stagesListCopy = stagesList.Select(x => x).ToList();

            string s = @"
WITH processes AS (
    SELECT
        a.process_id
    FROM
        sajet.sys_role_op_privilege   a,
        sajet.sys_role_emp            b
    WHERE
        a.role_id = b.role_id
        AND b.emp_id = :emp_id
    UNION
    SELECT
        a.process_id
    FROM
        sajet.sys_emp_process_privilege   a,
        sajet.sys_emp                     b
    WHERE
        a.emp_id = b.emp_id
        AND b.emp_id = :emp_id
)
SELECT
    aa.stage_id,
    aa.stage_code,
    aa.stage_name,
    bb.process_id,
    bb.process_code,
    bb.process_name
FROM
    sajet.sys_stage                  aa,
    sajet.sys_process                bb,
    sajet.sys_rc_route_detail        cc,
    sajet.g_rc_travel_machine_down   dd,
    processes                        pp
WHERE
    aa.stage_id = bb.stage_id
    AND bb.process_id = pp.process_id
    AND to_char(bb.process_id) = cc.node_content
    AND cc.node_id = dd.node_id
    AND dd.data_status = 1
ORDER BY
    aa.stage_name,
    bb.process_name
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.Number, "emp_id", userID },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                var stages = d.Tables[0].Rows
                    .Cast<DataRow>()
                    .ToList();

                stages.ForEach(x =>
                {
                    if (!stagesListCopy.Any(z => z?.ID == x["stage_id"].ToString()))
                    {
                        stagesListCopy.Add(new StageModel
                        {
                            ID = x["stage_id"].ToString(),
                            CODE = x["stage_code"].ToString(),
                            NAME = x["stage_name"].ToString(),
                            ProcessModel = new List<ComboBoxItemModel>(),
                        });
                    }

                    var processes = stagesListCopy
                    .First(z => z.ID == x["stage_id"].ToString())
                    .ProcessModel;

                    if (!processes.Any(z => z?.ID == x["process_id"].ToString()))
                    {
                        processes.Add(new ComboBoxItemModel
                        {
                            ID = x["process_id"].ToString(),
                            CODE = x["process_code"].ToString(),
                            NAME = x["process_name"].ToString(),
                        });
                    }
                });

                stagesList = stagesListCopy.Select(x => x).ToList();

                return true;
            }

            stagesList = null;

            return false;
        }

        /// <summary>
        /// 使用搜尋欄位值找符合條件的流程卡
        /// </summary>
        /// <returns></returns>
        public static List<RuncardModel> FindRuncardsWithConditions(FilterValueModel filter)
        {
            string s = @"
SELECT
    bb.work_order,
    ee.part_no,
    ee.spec1,
    ee.option2,
    ee.option4,
    aa.rc_no,
    dd.process_id,
    aa.node_id,
    bb.wip_out_time
FROM
    sajet.g_rc_travel_machine_down   aa,
    sajet.g_rc_travel                bb,
    sajet.sys_rc_route_detail        cc,
    sajet.sys_process                dd,
    sajet.sys_part                   ee
WHERE
    aa.data_status = 1
    AND aa.rc_no = bb.rc_no
    AND aa.node_id = bb.node_id
    AND aa.node_id = cc.node_id
    AND bb.part_id = ee.part_id
    AND cc.node_content = to_char(dd.process_id)
    AND dd.process_name = :process_name
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "process_name", filter.ProcessName },
            };

            if (!string.IsNullOrWhiteSpace(filter.WorkOrder))
            {
                s += @"
    AND upper(bb.work_order) like :work_order || '%'
";
                p.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "work_order", filter.WorkOrder });
            }

            if (!string.IsNullOrWhiteSpace(filter.Runcard))
            {
                s += @"
    AND upper(aa.rc_no) like :rc_no || '%'
";
                p.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "rc_no", filter.Runcard });
            }

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                var resultSet = d.Tables[0].Rows.Cast<DataRow>()
                    .Select(x => new RuncardModel
                    {
                        WORK_ORDER = x["work_order"].ToString(),
                        PART_NO = x["part_no"].ToString(),
                        SPEC1 = x["spec1"].ToString(),
                        OPTION2 = x["option2"].ToString(),
                        OPTION4 = x["option4"].ToString(),
                        RC_NO = x["rc_no"].ToString(),
                        PROCESS_ID = x["process_id"].ToString(),
                        NODE_ID = x["node_id"].ToString(),
                        WIP_OUT_TIME = DateTime.Parse(x["wip_out_time"].ToString()),
                    }).ToList();

                return resultSet;
            }

            return null;
        }

        /// <summary>
        /// 取得可用的製程機台清單。
        /// </summary>
        /// <param name="data"></param>
        public static bool GetMachineForDataGridView(RuncardModel dataRow, ref List<MachineModel> machineList)
        {
            string s = @"
SELECT
    b.machine_id,
    b.machine_code,
    b.machine_desc,
    d.status_name,
    d.run_flag,
    d.default_status
FROM
    sajet.sys_rc_process_machine   a,
    sajet.sys_machine              b,
    sajet.g_machine_status         c,
    sajet.sys_machine_status       d
WHERE
    a.process_id = :process_id
    AND a.machine_id = b.machine_id
    AND a.machine_id = c.machine_id
    AND c.current_status_id = d.status_id
    AND a.enabled = 'Y'
    AND b.enabled = 'Y'
    AND b.machine_id NOT IN (
        SELECT
            grtm.machine_id
        FROM
            sajet.g_rc_status           grs,
            sajet.g_rc_travel_machine   grtm,
            sajet.sys_machine           sm
        WHERE
            grs.rc_no = :rc_no
            AND grs.rc_no = grtm.rc_no
            AND grs.travel_id = grtm.travel_id
            AND grtm.machine_id = sm.machine_id
    )
ORDER BY
    machine_code
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "rc_no", dataRow.RC_NO },
                new object[] { ParameterDirection.Input, OracleType.Number, "process_id", dataRow.PROCESS_ID },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                machineList = d.Tables[0].Rows
                    .Cast<DataRow>()
                    .ToList()
                    .Select(x =>
                    {
                        return new MachineModel
                        {
                            MACHINE_ID = int.Parse(x["machine_id"].ToString()),
                            MACHINE_CODE = x["machine_code"].ToString(),
                            MACHINE_DESC = x["machine_desc"].ToString(),
                            STATUS_NAME = x["status_name"].ToString(),
                        };
                    }).ToList();

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 檢查選取的機台是不是已經在使用中。
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool IsMachineInUse(string runcardNumber, out string message, ref List<MachineModel> machineList)
        {
            string s = @"
SELECT
    SM.MACHINE_CODE
FROM
    SAJET.G_RC_STATUS GRS
   ,SAJET.G_RC_TRAVEL_MACHINE GRTM
   ,SAJET.SYS_MACHINE SM
WHERE
    GRS.RC_NO = :RC_NO
AND GRS.RC_NO = GRTM.RC_NO
AND GRS.TRAVEL_ID = GRTM.TRAVEL_ID
AND GRTM.MACHINE_ID = SM.MACHINE_ID
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", runcardNumber },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            var UsedMachineList = new List<string>();

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in d.Tables[0].Rows)
                {
                    UsedMachineList.Add(row["MACHINE_CODE"].ToString());
                }

                var result = machineList
                     .Where(x => x.Select && UsedMachineList.Contains(x.MACHINE_CODE))
                     .Select(x => $"[{x.MACHINE_CODE}] {x.MACHINE_DESC}")
                     .ToList();

                message = string.Join(",", result);

                return result.Any();
            }
            else
            {
                message = string.Empty;

                return false;
            }
        }

        /// <summary>
        /// 新增機台。
        /// </summary>
        public static void AddMachine(string userID, RuncardModel currentRuncard, DateTime startTime, DateTime endTime, List<MachineModel> machineList)
        {
            int i = 0;

            string s = string.Empty;

            DateTime now = DateTime.Now;

            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", currentRuncard.RC_NO },
                new object[] { ParameterDirection.Input, OracleType.Number, "NODE_ID", currentRuncard.NODE_ID },
                new object[] { ParameterDirection.Input, OracleType.Number, "UPDATE_USERID", userID },
                new object[] { ParameterDirection.Input, OracleType.DateTime, "UPDATE_TIME", DateTime.Now },
            };

            machineList
                .Where(x => x.Select)
                .ToList()
                .ForEach(x =>
                {
                    s += $@"
INSERT INTO
    SAJET.G_RC_TRAVEL_MACHINE
(
    RC_NO
   ,TRAVEL_ID
   ,MACHINE_ID
   ,START_TIME
   ,END_TIME
   ,LOAD_PORT
   ,UPDATE_USERID
   ,UPDATE_TIME
)
SELECT
    :RC_NO
   ,TRAVEL_ID
   ,:MACHINE_ID{i}
   ,:START_TIME{i}
   ,:END_TIME{i}
   ,0
   ,:UPDATE_USERID
   ,:UPDATE_TIME
FROM
    SAJET.G_RC_TRAVEL_MACHINE_DOWN
WHERE
    RC_NO = :RC_NO
AND NODE_ID = :NODE_ID
AND DATA_STATUS = 1
;

INSERT INTO
    SAJET.G_RC_TRAVEL_MACHINE_DOWN
(
    RC_NO
   ,TRAVEL_ID
   ,NODE_ID
   ,MACHINE_ID
   ,START_TIME
   ,END_TIME
   ,REASON_ID
   ,LOAD_QTY
   ,UPDATE_USERID
   ,UPDATE_TIME
   ,DATA_STATUS
   ,WORK_TIME_MINUTE
   ,WORK_TIME_SECOND
)
SELECT
    RC_NO
   ,TRAVEL_ID
   ,NODE_ID
   ,:MACHINE_ID{i}
   ,:START_TIME{i}
   ,:END_TIME{i}
   ,0
   ,0
   ,:UPDATE_USERID
   ,:UPDATE_TIME
   ,2
   ,NVL(ROUND((TO_NUMBER(:END_TIME{i} - :START_TIME{i}) * 24 * 60 * 60 + 29) / 60), 0)
   ,NVL(ROUND(TO_NUMBER(:END_TIME{i} - :START_TIME{i}) * 24 * 60 * 60, 0), 3)
FROM
    SAJET.G_RC_TRAVEL_MACHINE_DOWN
WHERE
    RC_NO = :RC_NO
AND NODE_ID = :NODE_ID
AND DATA_STATUS = 1
;
";
                    p.Add(new object[] { ParameterDirection.Input, OracleType.Number, $"MACHINE_ID{i}", x.MACHINE_ID });
                    p.Add(new object[] { ParameterDirection.Input, OracleType.DateTime, $"START_TIME{i}", x.START_TIME ?? startTime });
                    p.Add(new object[] { ParameterDirection.Input, OracleType.DateTime, $"END_TIME{i}", x.END_TIME ?? endTime });

                    i++;
                });

            s = $@"
BEGIN
{s}

DELETE FROM
    SAJET.G_RC_TRAVEL_MACHINE_DOWN
WHERE
    RC_NO = :RC_NO
AND NODE_ID = :NODE_ID
AND DATA_STATUS = 1
;

END;
";
            ClientUtils.ExecuteSQL(s, p.ToArray());
        }
    }
}
