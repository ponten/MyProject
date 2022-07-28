create or replace PROCEDURE sj_rc_ipqc (
    temp           IN    VARCHAR2,
    trc            IN    VARCHAR2,
    titem          IN    VARCHAR2,
    tdefect        IN    VARCHAR2,
    tsn            IN    VARCHAR2,
    tmemo          IN    VARCHAR2,
    tgood          IN    NUMBER,
    tscrap         IN    NUMBER,
    tnextprocess   IN    NUMBER,
    tnextnode      IN    NUMBER,
    tnextsheet     IN    VARCHAR2,
    tbonus         IN    NUMBER,
    tnow           IN    DATE,
    tstatus        IN    VARCHAR2,
    tmachine       IN    VARCHAR2,
    tkeypart       IN    VARCHAR2,   --PART_NO+CHR(9)+KPSN+CHR(9)+ITEM_COUNT+CHR(27)+...
    tworkhour      IN    VARCHAR2,
    tres           OUT   VARCHAR2
) IS

    c_emp            sajet.sys_emp.emp_id%TYPE;
    c_temp           VARCHAR2(2048);
    c_itemtemp       VARCHAR2(2048);
    c_item           VARCHAR2(100);
    c_value          VARCHAR2(200);
    c_wo             sajet.g_rc_status.work_order%TYPE;
    c_travel_sn      sajet.g_sn_status.travel_id%TYPE;
    c_node           sajet.g_rc_status.node_id%TYPE;
    c_process        sajet.sys_rc_route_detail.node_content%TYPE;
    c_next_node      sajet.g_rc_status.node_id%TYPE;
    c_next_process   sajet.g_rc_status.process_id%TYPE;
    c_part           sajet.g_rc_status.part_id%TYPE;
    c_sheet          sajet.sys_rc_process_sheet.sheet_name%TYPE;
    c_defect         VARCHAR2(100);
    c_sn             sajet.g_rc_travel_param.serial_number%TYPE;
    c_sntemp         sajet.g_rc_travel_param.serial_number%TYPE;
    c_route          sajet.g_rc_status.route_id%TYPE;
    c_now            DATE;
    c_sysdate        DATE; --報工時間可以人工設定，需要另一個變數來記錄異動時間
    c_status         VARCHAR2(200);
    cc_status        VARCHAR2(10);
    c_ok             sajet.g_sn_status.good_qty%TYPE;
    c_ng             sajet.g_sn_status.scrap_qty%TYPE;
    c_good           NUMBER;
    c_scrap          NUMBER;
    g_count          NUMBER; -- for Group執行
    g_group          NUMBER; -- for Group執行
    g_type           VARCHAR2(5); -- for Group執行
    q_num            NUMBER; -- QC
    q_defecttype     VARCHAR2(200); -- QC
    q_analysis       VARCHAR2(200); -- QC
    q_measure        VARCHAR2(200); -- QC
    c_node_type      sajet.sys_rc_route_detail.node_type%TYPE; -- for Group執行
    sn_wo            sajet.g_sn_status.work_order%TYPE; -- for Update sajet.g_sn_property WO,RC
    c_count          NUMBER; -- for SN 獲取途程 node 異常修復
    c_qclotno        VARCHAR2(50);
    c_line           sajet.g_rc_status.pdline_id%TYPE;
    c_stage          sajet.g_rc_status.stage_id%TYPE;
    c_defectset      VARCHAR2(500);
    v_defectid       sajet.sys_defect.defect_id%TYPE;
    v_sql            VARCHAR2(1000);
    TYPE defect_cursor IS REF CURSOR;
    d_cursor         defect_cursor;
    c_hold           BOOLEAN;
    d_wipin          DATE;
    c_processtype    sajet.sys_operate_type.type_name%TYPE;
    c_travel         sajet.g_rc_status.travel_id%TYPE;
    c_bom_partid     NUMBER;
/*
V4:多筆DEFECT CODE
V5:可選擇下一站製程
V6:1.增加SJ_RC_TRANSATION_COUNT計算良率;
   2.SJ_EVENT_RC_LOW_YIELD判斷良率是否過低觸發alarm
   3.SJ_EVENT_RC_WODEFECT_ACC紀錄不良次數並且判斷累計不良次數是否觸發ALARM
   4.SJ_EVENT_RC_WODEFECT_CONT紀錄不良次數並且判斷累計連續不良次數是否觸發ALARM;當沒有不良現象，將此製程曾經紀錄連續不良資料清除
   5.以上procedure觸發alarm後hold RC回傳C_HOLD=true時不過站
V7:1.增加紀錄Bonus參數、設定手動過站時間
   2.累計製程工時 X
   3.調整SJ_RC_TRANSATION_COUNT計算BONUS;
V8:1.增加流程卡狀態參數可以設定HOLD
   2.當製程為QC或IPQC則TGOOD與TSCRAP為良率依據，過站數要視情況調整
   3.只記錄製程工時不累加
   4.呼叫SJ_RC_OUTPUT_MACHINE_STATUS增加設定過站時間參數
   5.呼叫SJ_RC_INVENTORY增加Interface良品報工功能
   6.不同Stage的Process過站更新stage id資料
V9:1.增加紀錄機台輸入參數與記錄功能
   2.增加料件序號輸入參數與記錄功能
   3.若只有WIP out會造成G_RC_STATUS.WIP_IN_QTY沒有寫入值所以增加 WIP_IN_QTY = GOOD + SCRAP
V10:1.機台狀態不改變，為了一個機台可以同時生產多個流程卡
V11:1將報工時間TNOW參數轉換成Travel id.
V12:增加工時參數並記錄
20171025-RC Update WO_START_TIME(當沒有RC INOUT時填入)
*/
BEGIN
    c_now := tnow;
    c_sysdate := sysdate;
    c_hold := false;
  --C_RC := 0; -- 調整為 input 計次
    sajet.sj_cksys_emp_out(temp, tres, c_emp);

  --V11 報工時間點加系統毫秒共 15 位數，調整原本從 G_RC_STATUS.TRAVEL_ID
    c_travel := to_char(tnow, 'yyyyMMddHH24miss')
                || substr(to_char(systimestamp, 'ff'), 1, 1);
    
    SELECT process_id 
      INTO c_next_process 
      FROM sajet.g_rc_status 
     WHERE rc_no = trc
       AND rownum = 1;    
                
    UPDATE sajet.g_rc_status 
       SET process_id = tnextprocess 
     WHERE rc_no = trc;       

    IF ( tres = 'OK' ) THEN
        SELECT
            a.work_order,
            a.process_id,
            a.part_id,
            a.node_id,
            a.route_id,
            a.pdline_id,
            a.stage_id
        INTO
            c_wo,
            c_process,
            c_part,
            c_node,
            c_route,
            c_line,
            c_stage
        FROM
            sajet.g_rc_status        a
        WHERE
            a.rc_no = trc
            AND ROWNUM = 1;

        SELECT
            wo_option2
        INTO c_bom_partid
        FROM
            sajet.g_wo_base
        WHERE
            work_order = c_wo
            AND ROWNUM = 1;

    -- Find SN Route data

        IF tsn IS NOT NULL THEN
            SELECT
                travel_id
            INTO c_travel_sn
            FROM
                sajet.g_sn_status
            WHERE
                rc_no = trc
                AND ROWNUM = 1;

            SELECT
                decode(b.node_type, 9, 9, 0)
            INTO cc_status
            FROM
                sajet.sys_rc_route_detail   a,
                sajet.sys_rc_route_detail   b
            WHERE
                a.route_id = c_route
                AND a.route_id = b.route_id
                AND a.link_name = 'NEXT'
                AND a.node_id = c_node
                AND a.next_node_id = b.node_id
                AND ROWNUM = 1;

        END IF;

    ----------------------------------------------------------------------------------
    -- Insert Process Parameter

        IF titem IS NOT NULL THEN
            c_temp := titem;
            LOOP
                EXIT WHEN c_temp IS NULL;
                c_itemtemp := substr(c_temp, 1, instr(c_temp, chr(27)) - 1);

                c_item := substr(c_itemtemp, 1, instr(c_itemtemp, chr(9)) - 1);

                c_value := substr(c_itemtemp, instr(c_itemtemp, chr(9)) + 1);

                c_temp := replace(c_temp, c_itemtemp || chr(27), '');
                INSERT INTO sajet.g_rc_travel_param (
                    rc_no,
                    travel_id,
                    item_id,
                    item_name,
                    item_value,
                    update_userid,
                    update_time,
                    item_type,
                    value_type,
                    item_phase
                )
                    SELECT
                        trc,
                        c_travel,
                        item_id,
                        item_name,
                        c_value,
                        c_emp,
                        c_sysdate,
                        item_type,
                        value_type,
                        item_phase
                    FROM
                        sajet.sys_rc_process_param_part
                    WHERE
                        process_id = c_process
                        AND part_id = c_part
                        AND item_id = c_item
                        AND ROWNUM = 1;

            END LOOP;

        END IF;

    --------------------------------------------------------------------------------
    -- Insert SN data

        c_good := 0;
        c_scrap := 0;
        IF tsn IS NOT NULL THEN
            c_temp := tsn;
            c_sntemp := 'N/A';
            LOOP
                EXIT WHEN c_temp IS NULL;
                c_itemtemp := substr(c_temp, 1, instr(c_temp, chr(27)) - 1);

                c_temp := replace(c_temp, c_itemtemp || chr(27), '');
                c_sn := substr(c_itemtemp, 1, instr(c_itemtemp, chr(9)) - 1);

                c_itemtemp := replace(c_itemtemp, c_sn || chr(9), '');
                c_status := substr(c_itemtemp, 1, instr(c_itemtemp, chr(9)) - 1);

                c_itemtemp := replace(c_itemtemp, c_status || chr(9), '');
                c_ok := substr(c_itemtemp, 1, instr(c_itemtemp, chr(9)) - 1);

                c_itemtemp := replace(c_itemtemp, c_ok || chr(9), '');
                c_ng := substr(c_itemtemp, 1, instr(c_itemtemp, chr(9)) - 1);

                c_itemtemp := replace(c_itemtemp, c_ng || chr(9), '');
                c_item := substr(c_itemtemp, 1, instr(c_itemtemp, chr(9)) - 1);

                c_value := substr(c_itemtemp, instr(c_itemtemp, chr(9)) + 1);

                IF c_sn <> c_sntemp THEN
                    IF c_status = 'OK' THEN
                        c_good := c_good + 1;
                        IF cc_status = 9 THEN
                            c_status := 9;
                        END IF;
                    ELSIF c_status = 'NG' THEN
                        c_scrap := c_scrap + 1;
                    END IF;

                    UPDATE sajet.g_sn_status
                    SET
                        good_qty = c_ok,
                        scrap_qty = c_ng,
                        update_userid = c_emp,
                        update_time = c_sysdate,
                        current_status = decode(c_status, 'NG', 8, 'OK', 0,
                                                c_status),
                        process_id = c_process,
                        out_process_time = c_now,
                        wip_out_time = c_now,
                        out_process_empid = c_emp,
                        wip_out_empid = c_emp
                    WHERE
                        serial_number = c_sn
                        AND ROWNUM = 1;

                -- for SN 過站 OUT_PROCES_TIME, WIP_OUT_TIME 時更新

                    INSERT INTO sajet.g_sn_travel (
                        work_order,
                        serial_number,
                        part_id,
                        version,
                        route_id,
                        factory_id,
                        pdline_id,
                        stage_id,
                        node_id,
                        process_id,
                        sheet_name,
                        terminal_id,
                        travel_id,
                        next_node,
                        next_process,
                        good_qty,
                        scrap_qty,
                        in_process_empid,
                        in_process_time,
                        wip_in_empid,
                        wip_in_memo,
                        wip_in_time,
                        wip_out_empid,
                        wip_out_memo,
                        wip_out_time,
                        out_process_empid,
                        out_process_time,
                        update_userid,
                        update_time,
                        rc_no,
                        csn,
                        pre_sn,
                        seq,
                        create_time
                    )
                        SELECT
                            work_order,
                            serial_number,
                            part_id,
                            version,
                            route_id,
                            factory_id,
                            pdline_id,
                            stage_id,
                            node_id,
                            process_id,
                            sheet_name,
                            terminal_id,
                            travel_id,
                            next_node,
                            next_process,
                            good_qty,
                            scrap_qty,
                            in_process_empid,
                            in_process_time,
                            wip_in_empid,
                            wip_in_memo,
                            wip_in_time,
                            wip_out_empid,
                            wip_out_memo,
                            wip_out_time,
                            out_process_empid,
                            out_process_time,
                            update_userid,
                            update_time,
                            rc_no,
                            csn,
                            pre_sn,
                            seq,
                            create_time
                        FROM
                            sajet.g_sn_status
                        WHERE
                            serial_number = c_sn
                            AND ROWNUM = 1;

                END IF;

                c_sntemp := c_sn;
                INSERT INTO sajet.g_rc_travel_param (
                    rc_no,
                    travel_id,
                    serial_number,
                    item_id,
                    item_name,
                    item_value,
                    update_userid,
                    update_time,
                    item_type,
                    value_type,
                    item_phase
                )
                    SELECT
                        trc,
                        c_travel,
                        c_sn,
                        item_id,
                        item_name,
                        c_value,
                        c_emp,
                        c_sysdate,
                        item_type,
                        value_type,
                        item_phase
                    FROM
                        sajet.sys_rc_process_param_part
                    WHERE
                        process_id = c_process
                        AND part_id = c_part
                        AND item_id = c_item
                        AND ROWNUM = 1;

            END LOOP;

        ELSE
            c_good := tgood;
            c_scrap := tscrap;
        END IF;

    --------------------------------------------------------------------------------
    --Insert Defect Code

        IF tdefect IS NOT NULL THEN
            c_temp := tdefect;
            LOOP
                EXIT WHEN c_temp IS NULL;
                c_itemtemp := substr(c_temp, 1, instr(c_temp, chr(27)) - 1);

                c_temp := replace(c_temp, c_itemtemp || chr(27), '');
                IF tsn IS NOT NULL THEN
                    c_sn := substr(c_itemtemp, 1, instr(c_itemtemp, chr(9)) - 1);

                    c_itemtemp := replace(c_itemtemp, c_sn || chr(9), '');
                ELSE
                    c_sn := NULL;
                END IF;

                c_defect := substr(c_itemtemp, 1, instr(c_itemtemp, chr(9)) - 1);

                c_itemtemp := replace(c_itemtemp, c_defect || chr(9), '');

            -- 將 DEFECT_ID 串起來
                BEGIN
                    SELECT
                        defect_id
                    INTO v_defectid
                    FROM
                        sajet.sys_defect
                    WHERE
                        defect_code = c_defect
                        AND ROWNUM = 1;

                    IF c_defectset IS NULL THEN
                        c_defectset := v_defectid;
                    ELSE
                        c_defectset := c_defectset
                                       || ','
                                       || v_defectid;
                    END IF;

                EXCEPTION
                    WHEN OTHERS THEN
                        v_defectid := 0;

                END;           

            -- QC

                q_num := instr(c_itemtemp, chr(9), 1, 2);
                IF q_num = 0 THEN
                    c_value := substr(c_itemtemp, instr(c_itemtemp, chr(9)) + 1);

                --V6 SJ_EVENT_RC_WODEFECT_ACC 紀錄不良次數，並且判斷累計不良次數是否觸發 ALARM

                    IF tsn IS NOT NULL THEN
                        sajet.sj_event_wodefect_accumulate(c_wo, tsn, 0, c_emp, c_defect,
                                                           tres);
                        sajet.sj_event_wodefect_continuous(c_wo, tsn, 0, c_emp, c_defect,
                                                           tres);
                    ELSE
                        sajet.sj_event_rc_wodefect_acc(c_wo, trc, c_process, c_emp, c_defect,
                                                       c_now, tres, c_hold);

                        sajet.sj_event_rc_wodefect_cont(c_wo, trc, c_process, c_emp, c_defect,
                                                        c_now, tres, c_hold);

                    END IF;

                    INSERT INTO sajet.g_rc_travel_defect (
                        rc_no,
                        travel_id,
                        serial_number,
                        process_id,
                        defect_id,
                        defect_level,
                        defect_type_id,
                        qty,
                        update_userid,
                        update_time
                    )
                        SELECT
                            trc,
                            c_travel,
                            c_sn,
                            c_process,
                            defect_id,
                            defect_level,
                            defect_type_id,
                            c_value,
                            c_emp,
                            c_sysdate
                        FROM
                            sajet.sys_defect
                        WHERE
                            defect_code = c_defect
                            AND ROWNUM = 1;

                ELSE
                    c_value := substr(c_itemtemp, 1, instr(c_itemtemp, chr(9)) - 1);

                    c_itemtemp := replace(c_itemtemp, c_value || chr(9), '');
                    q_defecttype := substr(c_itemtemp, 1, instr(c_itemtemp, chr(9)) - 1);

                    c_itemtemp := replace(c_itemtemp, q_defecttype || chr(9), '');
                    q_analysis := substr(c_itemtemp, 1, instr(c_itemtemp, chr(9)) - 1);

                    c_itemtemp := replace(c_itemtemp, q_analysis || chr(9), '');
                    q_measure := substr(c_itemtemp, instr(c_itemtemp, chr(9)) + 1);

                --V6 SJ_EVENT_RC_WODEFECT_ACC 紀錄不良次數，並且判斷累計不良次數是否觸發 ALARM
                -- .SJ_EVENT_RC_WODEFECT_CONT 紀錄不良次數，並且判斷累計連續不良次數是否觸發 ALARM; 當沒有不良現象，將此製程曾經紀錄連續不良資料清除

                    IF tsn IS NOT NULL THEN
                        sajet.sj_event_wodefect_accumulate(c_wo, tsn, 0, c_emp, c_defect,
                                                           tres);
                    ELSE
                        sajet.sj_event_rc_wodefect_acc(c_wo, trc, c_process, c_emp, c_defect,
                                                       c_now, tres, c_hold);

                        sajet.sj_event_rc_wodefect_cont(c_wo, trc, c_process, c_emp, c_defect,
                                                        c_now, tres, c_hold);

                    END IF;

                    INSERT INTO sajet.g_rc_travel_defect (
                        rc_no,
                        travel_id,
                        serial_number,
                        process_id,
                        defect_id,
                        defect_level,
                        defect_type_id,
                        qty,
                        update_userid,
                        update_time,
                        exception_type,
                        reason_analysis,
                        measure
                    )
                        SELECT
                            trc,
                            c_travel,
                            c_sn,
                            c_process,
                            defect_id,
                            defect_level,
                            defect_type_id,
                            c_value,
                            c_emp,
                            c_sysdate,
                            q_defecttype,
                            q_analysis,
                            q_measure
                        FROM
                            sajet.sys_defect
                        WHERE
                            defect_code = c_defect
                            AND ROWNUM = 1;

                END IF;

            END LOOP;

            IF c_defectset IS NOT NULL THEN
                v_sql := ' SELECT DEFECT_ID '
                         || ' FROM SAJET.SYS_RC_PROCESS_DEFECT '
                         || ' WHERE ENABLED = ''Y'' '
                         || ' AND PROCESS_ID ='
                         || c_process
                         || ' AND DEFECT_ID NOT IN ('
                         || c_defectset
                         || ') ';

                OPEN d_cursor FOR v_sql;

                LOOP
                    FETCH d_cursor INTO v_defectid;
                    EXIT WHEN d_cursor%notfound;
                    sj_event_rc_wodefect_cont_c(c_wo, c_process, c_emp, v_defectid, tres);
                END LOOP;

                CLOSE d_cursor;
            END IF;

        ELSE
        --當沒有不良現象，將此製程曾經紀錄連續不良資料清除
            sj_event_rc_wodefect_cont_c(c_wo, c_process, c_emp, 0, tres);
        END IF;

    ------------------------------------------------------------------------------------
    --NANCY ADD 2016.4.5
    --如果是 QC 測試站，則清除 QC 臨時記？，判定 QC 最終結果

        c_qclotno := trc || to_char(c_process);
        SELECT
            COUNT(*)
        INTO c_count
        FROM
            sajet.g_qc_lot
        WHERE
            qc_lotno LIKE c_qclotno || '%';

        IF c_count > 0 THEN
            BEGIN
                SELECT
                    qc_lotno
                INTO c_qclotno
                FROM
                    sajet.g_qc_lot
                WHERE
                    qc_lotno LIKE c_qclotno || '%'
                    AND qc_result = 'N/A';

                sajet.sj_rc_qc_clear_temp(c_qclotno, tres);
                sajet.sj_rc_qc_set_result(c_qclotno, tgood, tscrap, tres);
            EXCEPTION
                WHEN OTHERS THEN
                    tres := 'SET QC RESULT ERROR';
                    ROLLBACK;
                    GOTO endp;
            END;
        END IF;

    -----------------------------------------------------------------------------------------
    --V9.1
     /* 設備資料收集 */

        IF tmachine IS NOT NULL THEN
            c_temp := tmachine;
            LOOP
                EXIT WHEN c_temp IS NULL;
                c_item := substr(c_temp, 1, instr(c_temp, chr(9)) - 1);

                c_temp := replace(c_temp, c_item || chr(9), '');
                INSERT INTO sajet.g_rc_travel_machine (
                    rc_no,
                    travel_id,
                    machine_id,
                    start_time,
                    load_port,
                    update_userid,
                    update_time
                )
                    SELECT
                        trc,
                        c_travel,
                        machine_id,
                        c_now,
                        0,
                        c_emp,
                        c_sysdate
                    FROM
                        sajet.sys_machine
                    WHERE
                        machine_code = c_item
                        AND ROWNUM = 1;

            -- RC 投入代表機台狀態為 RUN
            /*
            UPDATE SAJET.G_MACHINE_STATUS
            SET CURRENT_STATUS_ID = 1,
            STATUS_UPDATE_TIME = C_NOW
            WHERE  MACHINE_ID = (SELECT MACHINE_ID
            FROM SAJET.SYS_MACHINE
            WHERE MACHINE_CODE = C_ITEM
            AND ROWNUM = 1);               
             */

            END LOOP;

        END IF;

    --V8.4
    --SAJET.SJ_RC_OUTPUT_MACHINE_STATUS(TRC, C_EMP, C_TRAVEL_RC, TNOW,TRES);  
    -- 14.

        UPDATE sajet.g_rc_travel_machine
        SET
            end_time = c_now,
            update_userid = c_emp,
            update_time = c_sysdate
        WHERE
            rc_no = trc
            AND travel_id = c_travel
            AND end_time IS NULL;

    --------------------------------------------------------------------------------------------
    /* Keypart Collection  */

        IF tkeypart IS NOT NULL THEN
            c_temp := tkeypart;
            LOOP
                EXIT WHEN c_temp IS NULL;
                c_itemtemp := substr(c_temp, 1, instr(c_temp, chr(27)) - 1);

                c_temp := replace(c_temp, c_itemtemp || chr(27), '');
                c_item := substr(c_itemtemp, 1, instr(c_itemtemp, chr(9)) - 1);

                c_itemtemp := replace(c_itemtemp, c_item || chr(9), '');
                c_value := substr(c_itemtemp, 1, instr(c_itemtemp, chr(9)) - 1);

                c_count := substr(c_itemtemp, instr(c_itemtemp, chr(9)) + 1);

                INSERT INTO sajet.g_rc_keyparts (
                    work_order,
                    rc_no,
                    process_id,
                    item_part_id,
                    item_part_sn,
                    item_group,
                    version,
                    update_userid,
                    update_time,
                    enabled,
                    item_count
                )
                    SELECT
                        c_wo,
                        trc,
                        a.process_id,
                        b.part_id,
                        c_value,
                        a.item_group,
                        a.version,
                        c_emp,
                        c_sysdate,
                        'Y',
                        c_count
                    FROM
                        sajet.sys_bom        a,
                        sajet.sys_part       b,
                        sajet.sys_bom_info   c
                    WHERE
                        a.item_part_id = b.part_id
                        AND a.bom_id = c.bom_id
                        AND c.part_id = c_bom_partid
                        AND b.part_no = c_item
                        AND a.process_id = c_process
                        AND ROWNUM = 1;

                INSERT INTO sajet.g_ht_rc_keyparts (
                    work_order,
                    rc_no,
                    process_id,
                    item_part_id,
                    item_part_sn,
                    item_group,
                    version,
                    update_userid,
                    update_time,
                    enabled,
                    item_count
                )
                    SELECT
                        c_wo,
                        trc,
                        a.process_id,
                        b.part_id,
                        c_value,
                        a.item_group,
                        a.version,
                        c_emp,
                        c_sysdate,
                        'Y',
                        c_count
                    FROM
                        sajet.sys_bom        a,
                        sajet.sys_part       b,
                        sajet.sys_bom_info   c
                    WHERE
                        a.item_part_id = b.part_id
                        AND a.bom_id = c.bom_id
                        AND c.part_id = c_part
                        AND b.part_no = c_item
                        AND a.process_id = c_process
                        AND ROWNUM = 1;

            END LOOP;

        END IF;

    --------------------------------------------------------------------------------------------
    -- V6:增加SJ_RC_TRANSATION_COUNT計算良率
    -- 在INSERT G_RC_TRAVEL前紀錄避免被認定為重工
    -- V7 增加BONUS參數

        IF tstatus = 0 OR tstatus IS NULL THEN
            sajet.sj_rc_transation_count(c_line, c_stage, c_process, c_emp, c_now,
                                         trc, c_wo, c_part, tres, 0,
                                         tgood, tscrap, tbonus);
        ELSE    -- 當良品數=0,RC才判定NG
            sajet.sj_rc_transation_count(c_line, c_stage, c_process, c_emp, c_now,
                                         trc, c_wo, c_part, tres, 1,
                                         tgood, tscrap, tbonus);
        END IF;

    ----------------------------------------------------------------------------------------
    -- V6 SJ_EVENT_RC_LOW_YIELD判斷良率是否過低觸發alarm

        sajet.sj_event_rc_low_yield(c_wo, trc, c_emp, c_now, tres,
                                    c_hold);
    ---------------------------------------------------------------------------------------
        BEGIN
        --V7 找當站製程投入時間點如果找不到再找上一製程的產出時間
            SELECT
                nvl(wip_in_time, nvl(wip_out_time, create_time))
            INTO d_wipin
            FROM
                sajet.g_rc_status
            WHERE
                rc_no = trc
                AND ROWNUM = 1;

        EXCEPTION
            WHEN OTHERS THEN
                BEGIN
                    SELECT
                        nvl(wip_out_time, nvl(wip_in_time, create_time))
                    INTO d_wipin
                    FROM
                        sajet.g_rc_travel
                    WHERE
                        rc_no = trc
                        AND ROWNUM = 1
                    ORDER BY
                        wip_out_time DESC;

                EXCEPTION
                --V7 當站為第一個投入站而且沒有執行WIP INPUT的情況抓流程卡建立時間點
                    WHEN OTHERS THEN
                        SELECT
                            create_time
                        INTO d_wipin
                        FROM
                            sajet.g_rc_status
                        WHERE
                            rc_no = trc
                            AND ROWNUM = 1;

                END;
        END;

    -- V8

        c_status := 0;
        IF tstatus IS NOT NULL THEN
            IF tstatus = '0' THEN -- OK
                IF c_processtype = 'IPQC' THEN
                --C_GOOD := C_GOOD + C_SCRAP;
                --C_SCRAP := 0;
                    c_status := 0;
                END IF;
            ELSE            -- NG
                IF c_processtype = 'IPQC' THEN
                    c_good := c_good + c_scrap;
                    c_scrap := 0;
                    c_status := '0';
                ELSIF c_processtype = 'QC' THEN
                    c_good := c_good + c_scrap;
                    c_scrap := 0;
                    c_status := 2;
                END IF;
            END IF;
        ELSE
            IF c_good = 0 THEN
                c_status := 2;
            END IF;
        END IF;

        UPDATE sajet.g_rc_status
        SET
            wip_in_qty = c_good + c_scrap,           -- 
            wip_out_good_qty = c_good,
            wip_out_scrap_qty = c_scrap,
            wip_out_empid = c_emp,
            wip_out_memo = tmemo,
            wip_out_time = c_now,
            current_qty = c_good + tbonus,
        --CURRENT_STATUS    = DECODE(C_GOOD, 0, 8, 0),   --INPUT NEXT WIP PROCESS RC STATUS IS QUEUE  DECODE(C_GOOD, 0, 8, CURRENT_STATUS),
            current_status = c_status,
            out_process_empid = c_emp,
            out_process_time = c_now,
            update_time = c_sysdate,
            bonus_qty = tbonus,
        --WORKTIME          = ROUND(TO_NUMBER(C_NOW - D_WIPIN) * 1440) + WORKTIME  -- V7 累計製程工時
            worktime = round(to_number(c_now - d_wipin) * 1440),  -- V8 累計製程工時
            workhour = tworkhour
        WHERE
            rc_no = trc
            AND ROWNUM = 1;           

    -- 17

        INSERT INTO sajet.g_rc_travel
            SELECT
                *
            FROM
                sajet.g_rc_status
            WHERE
                rc_no = trc
                AND ROWNUM = 1;

    --20171025-RC Update WO_START_TIME(當沒有RC INOUT時填入)

        UPDATE sajet.g_wo_base
        SET
            wo_start_date = nvl(wo_start_date, c_now)
        WHERE
            work_order = c_wo;


        COMMIT;
    END IF;

    UPDATE sajet.g_rc_status 
       SET process_id = c_next_process,
           wip_in_time = null,           
           out_process_empid = null,
           out_process_time = null,
           wip_in_qty = null,
           wip_out_good_qty = null,
           wip_out_scrap_qty = null,
           wip_out_empid = null,
           wip_out_time = null
     WHERE rc_no = trc;
    << endp >> NULL;
EXCEPTION
    WHEN OTHERS THEN
        tres := sqlerrm;
        ROLLBACK;
END;