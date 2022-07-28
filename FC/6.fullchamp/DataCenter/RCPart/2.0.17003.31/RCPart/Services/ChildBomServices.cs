using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;

namespace RCPart
{
    class ChildBomServices
    {
        /// <summary>
        /// 取得子階料號
        /// </summary>
        public static DataSet GetChildPartNo(string partNo = "", string version = "")
        {
            string s = @"
SELECT
    c.part_no,
    c.version,
    d.part_no   AS child_part_no,
    d.version   AS child_part_version
FROM
    sajet.sys_bom_info   a,
    sajet.sys_bom        b,
    sajet.sys_part       c,
    sajet.sys_part       d,
    sajet.sys_process    e
WHERE
    a.bom_id = b.bom_id
    AND a.part_id = c.part_id
    AND b.item_part_id = d.part_id
    AND b.process_id = e.process_id (+)
    AND a.enabled = 'Y'
    AND b.enabled = 'Y'
    AND c.part_no = :part_no
    AND c.version = coalesce(:version, 'N/A')
    AND ROWNUM = 1
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "part_no", partNo },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "version", version },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            return d;
        }

        /// <summary>
        /// 檢查子階料號是否存在
        /// </summary>
        /// <returns></returns>
        public static bool CheckChildPartNo(string partNo = "")
        {
            partNo = partNo?.Trim().ToUpper();

            string s = @"
SELECT
    part_id
FROM
    sajet.sys_part
WHERE
    part_no = :part_no
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "part_no", partNo },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            return d != null && d.Tables[0].Rows.Count > 0;
        }

        /// <summary>
        /// 根據主料號的基本資料更新狀況，更新子階料號（BOM）
        /// </summary>
        public static void RenewChildPartNo(string main_part_no, string main_version, string child_part_no, string update_user_id)
        {
            if (string.IsNullOrWhiteSpace(child_part_no))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(main_version))
            {
                main_version = "N/A";
            }

            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "main_part_no", main_part_no },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "main_part_version", main_version },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "child_part_no", child_part_no },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "update_user_id", update_user_id },
            };

            string s = @"
DECLARE
    main_bom_id          sajet.sys_bom_info.bom_id%TYPE;
    main_part_id         sajet.sys_part.part_id%TYPE;
    main_part_no         sajet.sys_part.part_no%TYPE;
    main_part_version    sajet.sys_part.version%TYPE;
    child_part_id        sajet.sys_part.part_id%TYPE;
    child_part_no        sajet.sys_part.part_no%TYPE;
    child_part_version   sajet.sys_part.version%TYPE;
    update_user_id       sajet.sys_part.update_userid%TYPE;
    date_time_now        sajet.sys_part.update_time%TYPE;
    work_type            VARCHAR(10);
    counter              NUMBER;
    output_message       VARCHAR(4000);
BEGIN
    /*設定參數*/
    main_part_no := :main_part_no;
    main_part_version := :main_part_version;
    child_part_no := :child_part_no;
    update_user_id := :update_user_id;
    date_time_now := sysdate;

    /*檢查執行模式*/
    SELECT
        COUNT(1)
    INTO counter
    FROM
        sajet.sys_part       a,
        sajet.sys_bom_info   b
    WHERE
        a.part_no = :main_part_no
        AND a.version = :main_part_version
        AND a.part_id = b.part_id
        AND a.version = b.version;

    IF counter > 0 THEN
        work_type := 'UPDATE';
    ELSE
        work_type := 'INSERT';
    END IF;

    SELECT
        COUNT(1)
    INTO counter
    FROM
        sajet.sys_bom_info   a,
        sajet.sys_bom        b,
        sajet.sys_part       c,
        sajet.sys_part       d
    WHERE
        a.bom_id = b.bom_id
        AND a.part_id = c.part_id
        AND b.item_part_id = d.part_id
        AND a.enabled = 'Y'
        AND b.enabled = 'Y'
        AND c.part_no = main_part_no
        AND c.version = coalesce(main_part_version, 'N/A')
        AND d.part_no = child_part_no;

    IF counter > 0 THEN
        work_type := 'DO_NOTHING';
    END IF;

    /*取得主料號 ID*/
    SELECT
        part_id
    INTO main_part_id
    FROM
        sajet.sys_part
    WHERE
        part_no = main_part_no;

    /*取得子件料號資訊*/

    SELECT
        part_id,
        version
    INTO
        child_part_id,
        child_part_version
    FROM
        sajet.sys_part
    WHERE
        part_no = child_part_no;

    /*更新子階料號*/

    CASE work_type
        WHEN 'DO_NOTHING' THEN
            NULL;
        WHEN 'INSERT' THEN
            SELECT
                nvl(MAX(bom_id) + 1, 10000001)
            INTO main_bom_id
            FROM
                sajet.sys_bom_info;

            INSERT ALL INTO sajet.sys_bom_info (
                bom_id,
                part_id,
                version,
                update_userid,
                update_time,
                enabled
            ) VALUES (
                main_bom_id,
                main_part_id,
                main_part_version,
                update_user_id,
                date_time_now,
                'Y'
            ) INTO sajet.sys_bom (
                bom_id,
                item_part_id,
                item_group,
                item_count,
                process_id,
                version,
                update_userid,
                update_time,
                enabled
            ) VALUES (
                main_bom_id,
                child_part_id,
                0,
                1,
                0,
                child_part_version,
                update_user_id,
                date_time_now,
                'Y'
            ) SELECT
                  1
              FROM
                  dual;

        WHEN 'UPDATE' THEN
            SELECT
                bom_id
            INTO main_bom_id
            FROM
                sajet.sys_bom_info
            WHERE
                part_id = main_part_id
                AND version = main_part_version
                AND ROWNUM = 1;

            INSERT INTO sajet.sys_ht_bom (
                bom_id,
                item_part_id,
                item_group,
                item_count,
                process_id,
                version,
                update_userid,
                update_time,
                enabled,
                location,
                unit,
                item_seq,
                is_material,
                purchase
            )
                SELECT
                    bom_id,
                    item_part_id,
                    item_group,
                    item_count,
                    process_id,
                    version,
                    update_userid,
                    update_time,
                    enabled,
                    location,
                    unit,
                    item_seq,
                    is_material,
                    purchase
                FROM
                    sajet.sys_bom
                WHERE
                    bom_id = main_bom_id;

            DELETE FROM sajet.sys_bom
            WHERE
                bom_id = main_bom_id;

            INSERT INTO sajet.sys_bom (
                bom_id,
                item_part_id,
                item_group,
                item_count,
                process_id,
                version,
                update_userid,
                update_time,
                enabled
            ) VALUES (
                main_bom_id,
                child_part_id,
                0,
                1,
                0,
                child_part_version,
                update_user_id,
                date_time_now,
                'Y'
            );

    END CASE;

    output_message := 'OK';
END;
";
            ClientUtils.ExecuteSQL(s, p.ToArray());
        }
    }
}
