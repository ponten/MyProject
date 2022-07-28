CREATE OR REPLACE PROCEDURE SJ_RC_OUTPUT (
    TEMP           IN    VARCHAR2,
    TRC            IN    VARCHAR2,
    TITEM          IN    VARCHAR2,
    TDEFECT        IN    VARCHAR2,
    TSN            IN    VARCHAR2,
    TMEMO          IN    VARCHAR2,
    TGOOD          IN    NUMBER,
    TSCRAP         IN    NUMBER,
    TNEXTPROCESS   IN    NUMBER,
    TNEXTNODE      IN    NUMBER,
    TNEXTSHEET     IN    VARCHAR2,
    TBONUS         IN    NUMBER,
    TNOW           IN    DATE,
    TSTATUS        IN    VARCHAR2,
    TMACHINE       IN    VARCHAR2,
    TKEYPART       IN    VARCHAR2,   --PART_NO+CHR(9)+KPSN+CHR(9)+ITEM_COUNT+CHR(27)+...
    TWORKHOUR      IN    VARCHAR2,
    TRES           OUT   VARCHAR2
) IS

    C_EMP            SAJET.SYS_EMP.EMP_ID%TYPE;
    C_TEMP           VARCHAR2(2048);
    C_ITEMTEMP       VARCHAR2(2048);
    C_ITEM           VARCHAR2(100);
    C_VALUE          VARCHAR2(200);
    C_WO             SAJET.G_RC_STATUS.WORK_ORDER%TYPE;
    C_TRAVEL_SN      SAJET.G_SN_STATUS.TRAVEL_ID%TYPE;
    C_NODE           SAJET.G_RC_STATUS.NODE_ID%TYPE;
    C_PROCESS        SAJET.SYS_RC_ROUTE_DETAIL.NODE_CONTENT%TYPE;
    C_NEXT_NODE      SAJET.G_RC_STATUS.NODE_ID%TYPE;
    C_NEXT_PROCESS   SAJET.G_RC_STATUS.PROCESS_ID%TYPE;
    C_PART           SAJET.G_RC_STATUS.PART_ID%TYPE;
    C_SHEET          SAJET.SYS_RC_PROCESS_SHEET.SHEET_NAME%TYPE;
    C_DEFECT         VARCHAR2(100);
    C_SN             SAJET.G_RC_TRAVEL_PARAM.SERIAL_NUMBER%TYPE;
    C_SNTEMP         SAJET.G_RC_TRAVEL_PARAM.SERIAL_NUMBER%TYPE;
    C_ROUTE          SAJET.G_RC_STATUS.ROUTE_ID%TYPE;
    C_NOW            DATE;
    C_SYSDATE        DATE; --報工時間可以人工設定，需要另一個變數來記錄異動時間
    C_STATUS         VARCHAR2(200);
    CC_STATUS        VARCHAR2(10);
    C_OK             SAJET.G_SN_STATUS.GOOD_QTY%TYPE;
    C_NG             SAJET.G_SN_STATUS.SCRAP_QTY%TYPE;
    C_GOOD           NUMBER;
    C_SCRAP          NUMBER;
    G_COUNT          NUMBER; -- for Group執行
    G_GROUP          NUMBER; -- for Group執行
    G_TYPE           VARCHAR2(5); -- for Group執行
    Q_NUM            NUMBER; -- QC
    Q_DEFECTTYPE     VARCHAR2(200); -- QC
    Q_ANALYSIS       VARCHAR2(200); -- QC
    Q_MEASURE        VARCHAR2(200); -- QC
    C_NODE_TYPE      SAJET.SYS_RC_ROUTE_DETAIL.NODE_TYPE%TYPE; -- for Group執行
    SN_WO            SAJET.G_SN_STATUS.WORK_ORDER%TYPE; -- for Update sajet.g_sn_property WO,RC
    C_COUNT          NUMBER; -- for SN 獲取途程 node 異常修復
    C_QCLOTNO        VARCHAR2(50);
    C_LINE           SAJET.G_RC_STATUS.PDLINE_ID%TYPE;
    C_STAGE          SAJET.G_RC_STATUS.STAGE_ID%TYPE;
    C_DEFECTSET      VARCHAR2(500);
    V_DEFECTID       SAJET.SYS_DEFECT.DEFECT_ID%TYPE;
    V_SQL            VARCHAR2(1000);
    TYPE DEFECT_CURSOR IS REF CURSOR;
    D_CURSOR         DEFECT_CURSOR;
    C_HOLD           BOOLEAN;
    D_WIPIN          DATE;
    C_PROCESSTYPE    SAJET.SYS_OPERATE_TYPE.TYPE_NAME%TYPE;
    C_TRAVEL         SAJET.G_RC_STATUS.TRAVEL_ID%TYPE;
    C_BOM_PARTID     NUMBER;
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
    C_NOW := TNOW;
    C_SYSDATE := SYSDATE;
    C_HOLD := FALSE;
  --C_RC := 0; -- 調整為 input 計次
    SAJET.SJ_CKSYS_EMP_OUT(TEMP, TRES, C_EMP);

  --V11 報工時間點加系統毫秒共 15 位數，調整原本從 G_RC_STATUS.TRAVEL_ID
    C_TRAVEL := TO_CHAR(TNOW, 'yyyyMMddHH24miss')
                || SUBSTR(TO_CHAR(SYSTIMESTAMP, 'ff'), 1, 1);

    IF ( TRES = 'OK' ) THEN
        SELECT
            A.WORK_ORDER,
            A.PROCESS_ID,
            A.PART_ID,
            A.NODE_ID,
            A.ROUTE_ID,
            A.PDLINE_ID,
            A.STAGE_ID,
            C.TYPE_NAME--, A.TRAVEL_ID 
        INTO
            C_WO,
            C_PROCESS,
            C_PART,
            C_NODE,
            C_ROUTE,
            C_LINE,
            C_STAGE,
            C_PROCESSTYPE--, C_TRAVEL
        FROM
            SAJET.G_RC_STATUS        A,
            SAJET.SYS_PROCESS        B,
            SAJET.SYS_OPERATE_TYPE   C
        WHERE
            RC_NO = TRC
            AND A.PROCESS_ID = B.PROCESS_ID
            AND C.OPERATE_ID = B.OPERATE_ID
            AND ROWNUM = 1;

        SELECT
            WO_OPTION2
        INTO C_BOM_PARTID
        FROM
            SAJET.G_WO_BASE
        WHERE
            WORK_ORDER = C_WO
            AND ROWNUM = 1;

    -- Find SN Route data

        IF TSN IS NOT NULL THEN
            SELECT
                TRAVEL_ID
            INTO C_TRAVEL_SN
            FROM
                SAJET.G_SN_STATUS
            WHERE
                RC_NO = TRC
                AND ROWNUM = 1;

            SELECT
                DECODE(B.NODE_TYPE, 9, 9, 0)
            INTO CC_STATUS
            FROM
                SAJET.SYS_RC_ROUTE_DETAIL   A,
                SAJET.SYS_RC_ROUTE_DETAIL   B
            WHERE
                A.ROUTE_ID = C_ROUTE
                AND A.ROUTE_ID = B.ROUTE_ID
                AND A.LINK_NAME = 'NEXT'
                AND A.NODE_ID = C_NODE
                AND A.NEXT_NODE_ID = B.NODE_ID
                AND ROWNUM = 1;

        END IF;

    ----------------------------------------------------------------------------------
    -- Insert Process Parameter

        IF TITEM IS NOT NULL THEN
            C_TEMP := TITEM;
            LOOP
                EXIT WHEN C_TEMP IS NULL;
                C_ITEMTEMP := SUBSTR(C_TEMP, 1, INSTR(C_TEMP, CHR(27)) - 1);

                C_ITEM := SUBSTR(C_ITEMTEMP, 1, INSTR(C_ITEMTEMP, CHR(9)) - 1);

                C_VALUE := SUBSTR(C_ITEMTEMP, INSTR(C_ITEMTEMP, CHR(9)) + 1);

                C_TEMP := REPLACE(C_TEMP, C_ITEMTEMP || CHR(27), '');
                INSERT INTO SAJET.G_RC_TRAVEL_PARAM (
                    RC_NO,
                    TRAVEL_ID,
                    ITEM_ID,
                    ITEM_NAME,
                    ITEM_VALUE,
                    UPDATE_USERID,
                    UPDATE_TIME,
                    ITEM_TYPE,
                    VALUE_TYPE,
                    ITEM_PHASE
                )
                    SELECT
                        TRC,
                        C_TRAVEL,
                        ITEM_ID,
                        ITEM_NAME,
                        C_VALUE,
                        C_EMP,
                        C_SYSDATE,
                        ITEM_TYPE,
                        VALUE_TYPE,
                        ITEM_PHASE
                    FROM
                        SAJET.SYS_RC_PROCESS_PARAM_PART
                    WHERE
                        PROCESS_ID = C_PROCESS
                        AND PART_ID = C_PART
                        AND ITEM_ID = C_ITEM
                        AND ROWNUM = 1;

            END LOOP;

        END IF;

    --------------------------------------------------------------------------------
    -- Insert SN data

        C_GOOD := 0;
        C_SCRAP := 0;
        IF TSN IS NOT NULL THEN
            C_TEMP := TSN;
            C_SNTEMP := 'N/A';
            LOOP
                EXIT WHEN C_TEMP IS NULL;
                C_ITEMTEMP := SUBSTR(C_TEMP, 1, INSTR(C_TEMP, CHR(27)) - 1);

                C_TEMP := REPLACE(C_TEMP, C_ITEMTEMP || CHR(27), '');
                C_SN := SUBSTR(C_ITEMTEMP, 1, INSTR(C_ITEMTEMP, CHR(9)) - 1);

                C_ITEMTEMP := REPLACE(C_ITEMTEMP, C_SN || CHR(9), '');
                C_STATUS := SUBSTR(C_ITEMTEMP, 1, INSTR(C_ITEMTEMP, CHR(9)) - 1);

                C_ITEMTEMP := REPLACE(C_ITEMTEMP, C_STATUS || CHR(9), '');
                C_OK := SUBSTR(C_ITEMTEMP, 1, INSTR(C_ITEMTEMP, CHR(9)) - 1);

                C_ITEMTEMP := REPLACE(C_ITEMTEMP, C_OK || CHR(9), '');
                C_NG := SUBSTR(C_ITEMTEMP, 1, INSTR(C_ITEMTEMP, CHR(9)) - 1);

                C_ITEMTEMP := REPLACE(C_ITEMTEMP, C_NG || CHR(9), '');
                C_ITEM := SUBSTR(C_ITEMTEMP, 1, INSTR(C_ITEMTEMP, CHR(9)) - 1);

                C_VALUE := SUBSTR(C_ITEMTEMP, INSTR(C_ITEMTEMP, CHR(9)) + 1);

                IF C_SN <> C_SNTEMP THEN
                    IF C_STATUS = 'OK' THEN
                        C_GOOD := C_GOOD + 1;
                        IF CC_STATUS = 9 THEN
                            C_STATUS := 9;
                        END IF;
                    ELSIF C_STATUS = 'NG' THEN
                        C_SCRAP := C_SCRAP + 1;
                    END IF;

                    UPDATE SAJET.G_SN_STATUS
                    SET
                        GOOD_QTY = C_OK,
                        SCRAP_QTY = C_NG,
                        UPDATE_USERID = C_EMP,
                        UPDATE_TIME = C_SYSDATE,
                        CURRENT_STATUS = DECODE(C_STATUS, 'NG', 8, 'OK', 0,
                                                C_STATUS),
                        PROCESS_ID = C_PROCESS,
                        OUT_PROCESS_TIME = C_NOW,
                        WIP_OUT_TIME = C_NOW,
                        OUT_PROCESS_EMPID = C_EMP,
                        WIP_OUT_EMPID = C_EMP
                    WHERE
                        SERIAL_NUMBER = C_SN
                        AND ROWNUM = 1;

                -- for SN 過站 OUT_PROCES_TIME, WIP_OUT_TIME 時更新

                    INSERT INTO SAJET.G_SN_TRAVEL (
                        WORK_ORDER,
                        SERIAL_NUMBER,
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
                        GOOD_QTY,
                        SCRAP_QTY,
                        IN_PROCESS_EMPID,
                        IN_PROCESS_TIME,
                        WIP_IN_EMPID,
                        WIP_IN_MEMO,
                        WIP_IN_TIME,
                        WIP_OUT_EMPID,
                        WIP_OUT_MEMO,
                        WIP_OUT_TIME,
                        OUT_PROCESS_EMPID,
                        OUT_PROCESS_TIME,
                        UPDATE_USERID,
                        UPDATE_TIME,
                        RC_NO,
                        CSN,
                        PRE_SN,
                        SEQ,
                        CREATE_TIME
                    )
                        SELECT
                            WORK_ORDER,
                            SERIAL_NUMBER,
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
                            GOOD_QTY,
                            SCRAP_QTY,
                            IN_PROCESS_EMPID,
                            IN_PROCESS_TIME,
                            WIP_IN_EMPID,
                            WIP_IN_MEMO,
                            WIP_IN_TIME,
                            WIP_OUT_EMPID,
                            WIP_OUT_MEMO,
                            WIP_OUT_TIME,
                            OUT_PROCESS_EMPID,
                            OUT_PROCESS_TIME,
                            UPDATE_USERID,
                            UPDATE_TIME,
                            RC_NO,
                            CSN,
                            PRE_SN,
                            SEQ,
                            CREATE_TIME
                        FROM
                            SAJET.G_SN_STATUS
                        WHERE
                            SERIAL_NUMBER = C_SN
                            AND ROWNUM = 1;

                END IF;

                C_SNTEMP := C_SN;
                INSERT INTO SAJET.G_RC_TRAVEL_PARAM (
                    RC_NO,
                    TRAVEL_ID,
                    SERIAL_NUMBER,
                    ITEM_ID,
                    ITEM_NAME,
                    ITEM_VALUE,
                    UPDATE_USERID,
                    UPDATE_TIME,
                    ITEM_TYPE,
                    VALUE_TYPE,
                    ITEM_PHASE
                )
                    SELECT
                        TRC,
                        C_TRAVEL,
                        C_SN,
                        ITEM_ID,
                        ITEM_NAME,
                        C_VALUE,
                        C_EMP,
                        C_SYSDATE,
                        ITEM_TYPE,
                        VALUE_TYPE,
                        ITEM_PHASE
                    FROM
                        SAJET.SYS_RC_PROCESS_PARAM_PART
                    WHERE
                        PROCESS_ID = C_PROCESS
                        AND PART_ID = C_PART
                        AND ITEM_ID = C_ITEM
                        AND ROWNUM = 1;

            END LOOP;

        ELSE
            C_GOOD := TGOOD;
            C_SCRAP := TSCRAP;
        END IF;

    --------------------------------------------------------------------------------
    --Insert Defect Code

        IF TDEFECT IS NOT NULL THEN
            C_TEMP := TDEFECT;
            LOOP
                EXIT WHEN C_TEMP IS NULL;
                C_ITEMTEMP := SUBSTR(C_TEMP, 1, INSTR(C_TEMP, CHR(27)) - 1);

                C_TEMP := REPLACE(C_TEMP, C_ITEMTEMP || CHR(27), '');
                IF TSN IS NOT NULL THEN
                    C_SN := SUBSTR(C_ITEMTEMP, 1, INSTR(C_ITEMTEMP, CHR(9)) - 1);

                    C_ITEMTEMP := REPLACE(C_ITEMTEMP, C_SN || CHR(9), '');
                ELSE
                    C_SN := NULL;
                END IF;

                C_DEFECT := SUBSTR(C_ITEMTEMP, 1, INSTR(C_ITEMTEMP, CHR(9)) - 1);

                C_ITEMTEMP := REPLACE(C_ITEMTEMP, C_DEFECT || CHR(9), '');

            -- 將 DEFECT_ID 串起來
                BEGIN
                    SELECT
                        DEFECT_ID
                    INTO V_DEFECTID
                    FROM
                        SAJET.SYS_DEFECT
                    WHERE
                        DEFECT_CODE = C_DEFECT
                        AND ROWNUM = 1;

                    IF C_DEFECTSET IS NULL THEN
                        C_DEFECTSET := V_DEFECTID;
                    ELSE
                        C_DEFECTSET := C_DEFECTSET
                                       || ','
                                       || V_DEFECTID;
                    END IF;

                EXCEPTION
                    WHEN OTHERS THEN
                        V_DEFECTID := 0;
                END;           

            -- QC

                Q_NUM := INSTR(C_ITEMTEMP, CHR(9), 1, 2);
                IF Q_NUM = 0 THEN
                    C_VALUE := SUBSTR(C_ITEMTEMP, INSTR(C_ITEMTEMP, CHR(9)) + 1);

                --V6 SJ_EVENT_RC_WODEFECT_ACC 紀錄不良次數，並且判斷累計不良次數是否觸發 ALARM

                    IF TSN IS NOT NULL THEN
                        SAJET.SJ_EVENT_WODEFECT_ACCUMULATE(C_WO, TSN, 0, C_EMP, C_DEFECT,
                                                           TRES);
                        SAJET.SJ_EVENT_WODEFECT_CONTINUOUS(C_WO, TSN, 0, C_EMP, C_DEFECT,
                                                           TRES);
                    ELSE
                        SAJET.SJ_EVENT_RC_WODEFECT_ACC(C_WO, TRC, C_PROCESS, C_EMP, C_DEFECT,
                                                       C_NOW, TRES, C_HOLD);

                        SAJET.SJ_EVENT_RC_WODEFECT_CONT(C_WO, TRC, C_PROCESS, C_EMP, C_DEFECT,
                                                        C_NOW, TRES, C_HOLD);

                    END IF;

                    INSERT INTO SAJET.G_RC_TRAVEL_DEFECT (
                        RC_NO,
                        TRAVEL_ID,
                        SERIAL_NUMBER,
                        PROCESS_ID,
                        DEFECT_ID,
                        DEFECT_LEVEL,
                        DEFECT_TYPE_ID,
                        QTY,
                        UPDATE_USERID,
                        UPDATE_TIME
                    )
                        SELECT
                            TRC,
                            C_TRAVEL,
                            C_SN,
                            C_PROCESS,
                            DEFECT_ID,
                            DEFECT_LEVEL,
                            DEFECT_TYPE_ID,
                            C_VALUE,
                            C_EMP,
                            C_SYSDATE
                        FROM
                            SAJET.SYS_DEFECT
                        WHERE
                            DEFECT_CODE = C_DEFECT
                            AND ROWNUM = 1;

                ELSE
                    C_VALUE := SUBSTR(C_ITEMTEMP, 1, INSTR(C_ITEMTEMP, CHR(9)) - 1);

                    C_ITEMTEMP := REPLACE(C_ITEMTEMP, C_VALUE || CHR(9), '');
                    Q_DEFECTTYPE := SUBSTR(C_ITEMTEMP, 1, INSTR(C_ITEMTEMP, CHR(9)) - 1);

                    C_ITEMTEMP := REPLACE(C_ITEMTEMP, Q_DEFECTTYPE || CHR(9), '');
                    Q_ANALYSIS := SUBSTR(C_ITEMTEMP, 1, INSTR(C_ITEMTEMP, CHR(9)) - 1);

                    C_ITEMTEMP := REPLACE(C_ITEMTEMP, Q_ANALYSIS || CHR(9), '');
                    Q_MEASURE := SUBSTR(C_ITEMTEMP, INSTR(C_ITEMTEMP, CHR(9)) + 1);

                --V6 SJ_EVENT_RC_WODEFECT_ACC 紀錄不良次數，並且判斷累計不良次數是否觸發 ALARM
                -- .SJ_EVENT_RC_WODEFECT_CONT 紀錄不良次數，並且判斷累計連續不良次數是否觸發 ALARM; 當沒有不良現象，將此製程曾經紀錄連續不良資料清除

                    IF TSN IS NOT NULL THEN
                        SAJET.SJ_EVENT_WODEFECT_ACCUMULATE(C_WO, TSN, 0, C_EMP, C_DEFECT,
                                                           TRES);
                    ELSE
                        SAJET.SJ_EVENT_RC_WODEFECT_ACC(C_WO, TRC, C_PROCESS, C_EMP, C_DEFECT,
                                                       C_NOW, TRES, C_HOLD);

                        SAJET.SJ_EVENT_RC_WODEFECT_CONT(C_WO, TRC, C_PROCESS, C_EMP, C_DEFECT,
                                                        C_NOW, TRES, C_HOLD);

                    END IF;

                    INSERT INTO SAJET.G_RC_TRAVEL_DEFECT (
                        RC_NO,
                        TRAVEL_ID,
                        SERIAL_NUMBER,
                        PROCESS_ID,
                        DEFECT_ID,
                        DEFECT_LEVEL,
                        DEFECT_TYPE_ID,
                        QTY,
                        UPDATE_USERID,
                        UPDATE_TIME,
                        EXCEPTION_TYPE,
                        REASON_ANALYSIS,
                        MEASURE
                    )
                        SELECT
                            TRC,
                            C_TRAVEL,
                            C_SN,
                            C_PROCESS,
                            DEFECT_ID,
                            DEFECT_LEVEL,
                            DEFECT_TYPE_ID,
                            C_VALUE,
                            C_EMP,
                            C_SYSDATE,
                            Q_DEFECTTYPE,
                            Q_ANALYSIS,
                            Q_MEASURE
                        FROM
                            SAJET.SYS_DEFECT
                        WHERE
                            DEFECT_CODE = C_DEFECT
                            AND ROWNUM = 1;

                END IF;

            END LOOP;

            IF C_DEFECTSET IS NOT NULL THEN
                V_SQL := ' SELECT DEFECT_ID '
                         || ' FROM SAJET.SYS_RC_PROCESS_DEFECT '
                         || ' WHERE ENABLED = ''Y'' '
                         || ' AND PROCESS_ID ='
                         || C_PROCESS
                         || ' AND DEFECT_ID NOT IN ('
                         || C_DEFECTSET
                         || ') ';

                OPEN D_CURSOR FOR V_SQL;

                LOOP
                    FETCH D_CURSOR INTO V_DEFECTID;
                    EXIT WHEN D_CURSOR%NOTFOUND;
                    SJ_EVENT_RC_WODEFECT_CONT_C(C_WO, C_PROCESS, C_EMP, V_DEFECTID, TRES);
                END LOOP;

                CLOSE D_CURSOR;
            END IF;

        ELSE
        --當沒有不良現象，將此製程曾經紀錄連續不良資料清除
            SJ_EVENT_RC_WODEFECT_CONT_C(C_WO, C_PROCESS, C_EMP, 0, TRES);
        END IF;

    ------------------------------------------------------------------------------------
    --NANCY ADD 2016.4.5
    --如果是 QC 測試站，則清除 QC 臨時記？，判定 QC 最終結果

        C_QCLOTNO := TRC || TO_CHAR(C_PROCESS);
        SELECT
            COUNT(*)
        INTO C_COUNT
        FROM
            SAJET.G_QC_LOT
        WHERE
            QC_LOTNO LIKE C_QCLOTNO || '%';

        IF C_COUNT > 0 THEN
            BEGIN
                SELECT
                    QC_LOTNO
                INTO C_QCLOTNO
                FROM
                    SAJET.G_QC_LOT
                WHERE
                    QC_LOTNO LIKE C_QCLOTNO || '%'
                    AND QC_RESULT = 'N/A';

                SAJET.SJ_RC_QC_CLEAR_TEMP(C_QCLOTNO, TRES);
                SAJET.SJ_RC_QC_SET_RESULT(C_QCLOTNO, TGOOD, TSCRAP, TRES);
            EXCEPTION
                WHEN OTHERS THEN
                    TRES := 'SET QC RESULT ERROR';
                    ROLLBACK;
                    GOTO ENDP;
            END;
        END IF;

    -----------------------------------------------------------------------------------------
    --V9.1
     /* 設備資料收集 */

        IF TMACHINE IS NOT NULL THEN
            C_TEMP := TMACHINE;
            LOOP
                EXIT WHEN C_TEMP IS NULL;
                C_ITEM := SUBSTR(C_TEMP, 1, INSTR(C_TEMP, CHR(9)) - 1);

                C_TEMP := REPLACE(C_TEMP, C_ITEM || CHR(9), '');
                INSERT INTO SAJET.G_RC_TRAVEL_MACHINE (
                    RC_NO,
                    TRAVEL_ID,
                    MACHINE_ID,
                    START_TIME,
                    LOAD_PORT,
                    UPDATE_USERID,
                    UPDATE_TIME
                )
                    SELECT
                        TRC,
                        C_TRAVEL,
                        MACHINE_ID,
                        C_NOW,
                        0,
                        C_EMP,
                        C_SYSDATE
                    FROM
                        SAJET.SYS_MACHINE
                    WHERE
                        MACHINE_CODE = C_ITEM
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

        UPDATE SAJET.G_RC_TRAVEL_MACHINE
        SET
            END_TIME = C_NOW,
            UPDATE_USERID = C_EMP,
            UPDATE_TIME = C_SYSDATE
        WHERE
            RC_NO = TRC
            AND TRAVEL_ID = C_TRAVEL
            AND END_TIME IS NULL;

    --------------------------------------------------------------------------------------------
    /* Keypart Collection  */

        IF TKEYPART IS NOT NULL THEN
            C_TEMP := TKEYPART;
            LOOP
                EXIT WHEN C_TEMP IS NULL;
                C_ITEMTEMP := SUBSTR(C_TEMP, 1, INSTR(C_TEMP, CHR(27)) - 1);

                C_TEMP := REPLACE(C_TEMP, C_ITEMTEMP || CHR(27), '');
                C_ITEM := SUBSTR(C_ITEMTEMP, 1, INSTR(C_ITEMTEMP, CHR(9)) - 1);

                C_ITEMTEMP := REPLACE(C_ITEMTEMP, C_ITEM || CHR(9), '');
                C_VALUE := SUBSTR(C_ITEMTEMP, 1, INSTR(C_ITEMTEMP, CHR(9)) - 1);

                C_COUNT := SUBSTR(C_ITEMTEMP, INSTR(C_ITEMTEMP, CHR(9)) + 1);

                INSERT INTO SAJET.G_RC_KEYPARTS (
                    WORK_ORDER,
                    RC_NO,
                    PROCESS_ID,
                    ITEM_PART_ID,
                    ITEM_PART_SN,
                    ITEM_GROUP,
                    VERSION,
                    UPDATE_USERID,
                    UPDATE_TIME,
                    ENABLED,
                    ITEM_COUNT
                )
                    SELECT
                        C_WO,
                        TRC,
                        A.PROCESS_ID,
                        B.PART_ID,
                        C_VALUE,
                        A.ITEM_GROUP,
                        A.VERSION,
                        C_EMP,
                        C_SYSDATE,
                        'Y',
                        C_COUNT
                    FROM
                        SAJET.SYS_BOM        A,
                        SAJET.SYS_PART       B,
                        SAJET.SYS_BOM_INFO   C
                    WHERE
                        A.ITEM_PART_ID = B.PART_ID
                        AND A.BOM_ID = C.BOM_ID
                        AND C.PART_ID = C_BOM_PARTID
                        AND B.PART_NO = C_ITEM
                        AND A.PROCESS_ID = C_PROCESS
                        AND ROWNUM = 1;

                INSERT INTO SAJET.G_HT_RC_KEYPARTS (
                    WORK_ORDER,
                    RC_NO,
                    PROCESS_ID,
                    ITEM_PART_ID,
                    ITEM_PART_SN,
                    ITEM_GROUP,
                    VERSION,
                    UPDATE_USERID,
                    UPDATE_TIME,
                    ENABLED,
                    ITEM_COUNT
                )
                    SELECT
                        C_WO,
                        TRC,
                        A.PROCESS_ID,
                        B.PART_ID,
                        C_VALUE,
                        A.ITEM_GROUP,
                        A.VERSION,
                        C_EMP,
                        C_SYSDATE,
                        'Y',
                        C_COUNT
                    FROM
                        SAJET.SYS_BOM        A,
                        SAJET.SYS_PART       B,
                        SAJET.SYS_BOM_INFO   C
                    WHERE
                        A.ITEM_PART_ID = B.PART_ID
                        AND A.BOM_ID = C.BOM_ID
                        AND C.PART_ID = C_PART
                        AND B.PART_NO = C_ITEM
                        AND A.PROCESS_ID = C_PROCESS
                        AND ROWNUM = 1;

            END LOOP;

        END IF;

    --------------------------------------------------------------------------------------------
    -- V6:增加SJ_RC_TRANSATION_COUNT計算良率
    -- 在INSERT G_RC_TRAVEL前紀錄避免被認定為重工
    -- V7 增加BONUS參數

        IF TSTATUS = 0 OR TSTATUS IS NULL THEN
            SAJET.SJ_RC_TRANSATION_COUNT(C_LINE, C_STAGE, C_PROCESS, C_EMP, C_NOW,
                                         TRC, C_WO, C_PART, TRES, 0,
                                         TGOOD, TSCRAP, TBONUS);
        ELSE    -- 當良品數=0,RC才判定NG
            SAJET.SJ_RC_TRANSATION_COUNT(C_LINE, C_STAGE, C_PROCESS, C_EMP, C_NOW,
                                         TRC, C_WO, C_PART, TRES, 1,
                                         TGOOD, TSCRAP, TBONUS);
        END IF;

    ----------------------------------------------------------------------------------------
    -- V6 SJ_EVENT_RC_LOW_YIELD判斷良率是否過低觸發alarm

        SAJET.SJ_EVENT_RC_LOW_YIELD(C_WO, TRC, C_EMP, C_NOW, TRES,
                                    C_HOLD);
    ---------------------------------------------------------------------------------------
        BEGIN
        --V7 找當站製程投入時間點如果找不到再找上一製程的產出時間
            SELECT
                NVL(WIP_IN_TIME, NVL(WIP_OUT_TIME, CREATE_TIME))
            INTO D_WIPIN
            FROM
                SAJET.G_RC_STATUS
            WHERE
                RC_NO = TRC
                AND ROWNUM = 1;

        EXCEPTION
            WHEN OTHERS THEN
                BEGIN
                    SELECT
                        NVL(WIP_OUT_TIME, NVL(WIP_IN_TIME, CREATE_TIME))
                    INTO D_WIPIN
                    FROM
                        SAJET.G_RC_TRAVEL
                    WHERE
                        RC_NO = TRC
                        AND ROWNUM = 1
                    ORDER BY
                        WIP_OUT_TIME DESC;

                EXCEPTION
                --V7 當站為第一個投入站而且沒有執行WIP INPUT的情況抓流程卡建立時間點
                    WHEN OTHERS THEN
                        SELECT
                            CREATE_TIME
                        INTO D_WIPIN
                        FROM
                            SAJET.G_RC_STATUS
                        WHERE
                            RC_NO = TRC
                            AND ROWNUM = 1;

                END;
        END;

    -- V8

        C_STATUS := 0;
        IF TSTATUS IS NOT NULL THEN
            IF TSTATUS = '0' THEN -- OK
                IF C_PROCESSTYPE = 'IPQC' THEN
                --C_GOOD := C_GOOD + C_SCRAP;
                --C_SCRAP := 0;
                    C_STATUS := 0;
                END IF;
            ELSE            -- NG
                IF C_PROCESSTYPE = 'IPQC' THEN
                    C_GOOD := C_GOOD + C_SCRAP;
                    C_SCRAP := 0;
                    C_STATUS := '0';
                ELSIF C_PROCESSTYPE = 'QC' THEN
                    C_GOOD := C_GOOD + C_SCRAP;
                    C_SCRAP := 0;
                    C_STATUS := 2;
                END IF;
            END IF;
        ELSE
            IF C_GOOD = 0 THEN
                C_STATUS := 2;
            END IF;
        END IF;

        UPDATE SAJET.G_RC_STATUS
        SET
            WIP_IN_QTY = C_GOOD + C_SCRAP,           -- 
            WIP_OUT_GOOD_QTY = C_GOOD,
            WIP_OUT_SCRAP_QTY = C_SCRAP,
            WIP_OUT_EMPID = C_EMP,
            WIP_OUT_MEMO = TMEMO,
            WIP_OUT_TIME = C_NOW,
            CURRENT_QTY = C_GOOD + TBONUS,
        --CURRENT_STATUS    = DECODE(C_GOOD, 0, 8, 0),   --INPUT NEXT WIP PROCESS RC STATUS IS QUEUE  DECODE(C_GOOD, 0, 8, CURRENT_STATUS),
            CURRENT_STATUS = C_STATUS,
            OUT_PROCESS_EMPID = C_EMP,
            OUT_PROCESS_TIME = C_NOW,
            UPDATE_TIME = C_SYSDATE,
            BONUS_QTY = TBONUS,
        --WORKTIME          = ROUND(TO_NUMBER(C_NOW - D_WIPIN) * 1440) + WORKTIME  -- V7 累計製程工時
            WORKTIME = ROUND(TO_NUMBER(C_NOW - D_WIPIN) * 1440),  -- V8 累計製程工時
            WORKHOUR = TWORKHOUR
        WHERE
            RC_NO = TRC
            AND ROWNUM = 1;           

    -- 17

        INSERT INTO SAJET.G_RC_TRAVEL
            SELECT
                *
            FROM
                SAJET.G_RC_STATUS
            WHERE
                RC_NO = TRC
                AND ROWNUM = 1;

    --20171025-RC Update WO_START_TIME(當沒有RC INOUT時填入)

        UPDATE SAJET.G_WO_BASE
        SET
            WO_START_DATE = NVL(WO_START_DATE, C_NOW)
        WHERE
            WORK_ORDER = C_WO;

    /*
    INSERT INTO SAJET.G_RC_TRAVEL
       (WORK_ORDER,
        RC_NO,
        PART_ID,
        VERSION,
        ROUTE_ID,
        FACTORY_ID,
        PDLINE_ID,
        STAGE_ID,
        NODE_ID,
        PROCESS_ID,
        TERMINAL_ID,
        TRAVEL_ID,
        CURRENT_QTY,
        IN_PROCESS_EMPID,
        IN_PROCESS_TIME,
        WIP_PROCESS,
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
        UPDATE_USERID,
        UPDATE_TIME,
        CREATE_TIME,
        BONUS_QTY,
        WORKTIME,
        INITIAL_QTY)
    SELECT 
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
        TERMINAL_ID,
        TRAVEL_ID,
        CURRENT_QTY,
        IN_PROCESS_EMPID,
        IN_PROCESS_TIME,
        NEXT_PROCESS,
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
        UPDATE_USERID,
        UPDATE_TIME,
        CREATE_TIME,
        BONUS_QTY,
        WORKTIME,
        INITIAL_QTY
    FROM SAJET.G_RC_STATUS
    WHERE RC_NO = TRC
    AND ROWNUM = 1;
    */


    -- 當RC狀態為HOLD不過站

        IF C_HOLD THEN
            GOTO ENDP;
        ELSE
        -- 判定有無指定下一製程
            IF TNEXTPROCESS IS NOT NULL THEN
            -- 判定下一製程為END
                IF TNEXTPROCESS = '0' THEN
                    UPDATE SAJET.G_RC_STATUS
                    SET
                        TRAVEL_ID = C_TRAVEL + 1,
                        PROCESS_ID = TNEXTPROCESS,
                        SHEET_NAME = TNEXTSHEET,
                        CURRENT_STATUS = 9,             -- 完工
                        NEXT_NODE = 0,          -- 如果有多個下一製程無法確定
                        NEXT_PROCESS = 0,
                        UPDATE_USERID = C_EMP
                    WHERE
                        RC_NO = TRC
                        AND ROWNUM = 1;

                -- v8.5 寫入MESUSER.G_RC_INVENTORY，最後一批會另外自動close工單寫入MESUSER.G_WO_STATUS工單名稱與工單狀態complete

                    SAJET.SJ_RC_INVENTORY(TRC, TRES);
                ELSE
                    SELECT
                        STAGE_ID
                    INTO C_STAGE
                    FROM
                        SAJET.SYS_PROCESS
                    WHERE
                        PROCESS_ID = TNEXTPROCESS
                        AND ROWNUM = 1;

                    UPDATE SAJET.G_RC_STATUS
                    SET
                        TRAVEL_ID = C_TRAVEL + 1,
                        NODE_ID = TNEXTNODE,
                        PROCESS_ID = TNEXTPROCESS,
                        SHEET_NAME = TNEXTSHEET,
                        NEXT_NODE = 0,          -- 如果有多個下一製程無法確定
                        NEXT_PROCESS = 0,
                        UPDATE_USERID = C_EMP,
                        STAGE_ID = C_STAGE,
                        WIP_IN_QTY = NULL,
                        WIP_IN_EMPID = NULL,
                        WIP_IN_MEMO = NULL,
                        WIP_IN_TIME = NULL,
                        WIP_OUT_GOOD_QTY = NULL,
                        WIP_OUT_SCRAP_QTY = NULL,
                        WIP_OUT_EMPID = NULL,
                        WIP_OUT_MEMO = NULL,
                        WIP_OUT_TIME = NULL
                    WHERE
                        RC_NO = TRC
                        AND ROWNUM = 1;

                END IF;

            ELSE
                IF TGOOD > 0 THEN
                    C_STATUS := 0;

                --Begin Group執行 Modify by Jieke 2015/7/8
                    SELECT
                        COUNT(*)
                    INTO G_COUNT
                    FROM
                        SAJET.G_RC_PROCESS_GROUP
                    WHERE
                        RC_NO = TRC;

                    IF G_COUNT > 0 THEN
                        SELECT
                            PROCESS_TYPE
                        INTO G_TYPE
                        FROM
                            SAJET.G_RC_PROCESS_GROUP
                        WHERE
                            RC_NO = TRC
                            AND ROWNUM = 1;

                        IF G_TYPE = 'and' THEN
                            UPDATE SAJET.G_RC_PROCESS_GROUP
                            SET
                                STATUS = 'Complete',
                                SHEET_PHASE = 'N/A'
                            WHERE
                                RC_NO = TRC
                                AND STATUS = 'Running'
                                AND SHEET_PHASE = 'O';

                        ELSE
                            UPDATE SAJET.G_RC_PROCESS_GROUP
                            SET
                                STATUS = 'Complete',
                                SHEET_PHASE = 'N/A'
                            WHERE
                                RC_NO = TRC;

                        END IF;

                        SELECT
                            COUNT(*)
                        INTO G_COUNT
                        FROM
                            SAJET.G_RC_PROCESS_GROUP
                        WHERE
                            RC_NO = TRC
                            AND STATUS = 'Queue';

                        IF G_COUNT > 0 AND G_TYPE != 'or' THEN
                            SELECT
                                GROUP_ID
                            INTO G_GROUP
                            FROM
                                SAJET.G_RC_PROCESS_GROUP
                            WHERE
                                RC_NO = TRC
                                AND ROWNUM = 1;

                        --Tunny add 2015/11/2

                            IF C_TRAVEL_SN IS NOT NULL THEN
                                UPDATE SAJET.G_SN_STATUS
                                SET
                                    PROCESS_ID = '',
                                    CURRENT_STATUS = '0',
                                    SHEET_NAME = '',
                                    TRAVEL_ID = C_TRAVEL_SN + 1
                                WHERE
                                    RC_NO = TRC;

                            END IF;

                            UPDATE SAJET.G_RC_STATUS
                            SET
                                PROCESS_ID = '',
                                CURRENT_STATUS = '0',
                                SHEET_NAME = '',
                                TRAVEL_ID = C_TRAVEL + 1
                            WHERE
                                RC_NO = TRC;
                        --Tunny add end                   

                        ELSE
                            BEGIN
                                DELETE SAJET.G_RC_PROCESS_GROUP
                                WHERE
                                    RC_NO = TRC; --跳出Group后清除RC group數據

                            --Tunny 添加，獲取C_NODE_TYPE

                                SELECT
                                    B.NODE_ID,
                                    B.NODE_CONTENT,
                                    DECODE(B.NODE_TYPE, 9, 9, 0),
                                    B.NODE_TYPE
                                INTO
                                    C_NODE,
                                    C_PROCESS,
                                    C_STATUS,
                                    C_NODE_TYPE
                                FROM
                                    SAJET.SYS_RC_ROUTE_DETAIL   A,
                                    SAJET.SYS_RC_ROUTE_DETAIL   B
                                WHERE
                                    A.ROUTE_ID = C_ROUTE
                                    AND A.ROUTE_ID = B.ROUTE_ID
                                    AND A.LINK_NAME = 'NEXT'
                                    AND A.NODE_ID = C_NODE
                                    AND A.NEXT_NODE_ID = B.NODE_ID
                                    AND ROWNUM = 1;

                                BEGIN
                                    SELECT
                                        B.NODE_ID,
                                        B.NODE_CONTENT
                                    INTO
                                        C_NEXT_NODE,
                                        C_NEXT_PROCESS
                                    FROM
                                        SAJET.SYS_RC_ROUTE_DETAIL   A,
                                        SAJET.SYS_RC_ROUTE_DETAIL   B
                                    WHERE
                                        A.ROUTE_ID = C_ROUTE
                                        AND A.ROUTE_ID = B.ROUTE_ID
                                        AND A.LINK_NAME = 'NEXT'
                                        AND A.NODE_ID = C_NODE
                                        AND A.NEXT_NODE_ID = B.NODE_ID
                                        AND ROWNUM = 1;

                                EXCEPTION
                                    WHEN OTHERS THEN
                                        NULL;
                                END;

                                IF C_STATUS = 9 THEN
                                    C_PROCESS := NULL;
                                    C_NEXT_PROCESS := NULL;
                                    C_SHEET := NULL;
                                    C_NEXT_NODE := NULL;
                                ELSE
                                    BEGIN
                                        SELECT
                                            SHEET_NAME
                                        INTO C_SHEET
                                        FROM
                                            SAJET.SYS_RC_PROCESS_SHEET
                                        WHERE
                                            PROCESS_ID = C_PROCESS
                                            AND SHEET_SEQ = 0
                                            AND ROWNUM = 1;

                                    EXCEPTION
                                        WHEN OTHERS THEN
                                            NULL;
                                    END;
                                END IF;

                            EXCEPTION
                                WHEN OTHERS THEN
                                    NULL;
                            END;

                        -- 19, 20

                            UPDATE SAJET.G_RC_STATUS
                            SET
                                CURRENT_STATUS = C_STATUS,
                                TRAVEL_ID = C_TRAVEL + 1,
                                NODE_ID = C_NODE,
                                PROCESS_ID = C_PROCESS,
                                SHEET_NAME = C_SHEET,
                                NEXT_NODE = C_NEXT_NODE,
                                NEXT_PROCESS = C_NEXT_PROCESS,
                                UPDATE_USERID = C_EMP,
                                UPDATE_TIME = C_SYSDATE,
                                IN_PROCESS_EMPID = NULL,
                                IN_PROCESS_TIME = NULL,
                                WIP_IN_QTY = NULL,
                                WIP_IN_EMPID = NULL,
                                WIP_IN_MEMO = NULL,
                                WIP_IN_TIME = NULL,
                                WIP_OUT_GOOD_QTY = NULL,
                                WIP_OUT_SCRAP_QTY = NULL,
                                WIP_OUT_EMPID = NULL,
                                WIP_OUT_MEMO = NULL,
                                WIP_OUT_TIME = NULL,
                                OUT_PROCESS_EMPID = NULL,
                                OUT_PROCESS_TIME = NULL
                            WHERE
                                RC_NO = TRC
                                AND ROWNUM = 1;

                        -- 補充處理

                            IF TSN IS NOT NULL THEN
                            --Begin Add by Jieke 2016/01/06  for SN獲取途程node异常修复
                                SELECT
                                    NODE_ID,
                                    PROCESS_ID,
                                    SHEET_NAME,
                                    NEXT_NODE,
                                    NEXT_PROCESS
                                INTO
                                    C_NODE,
                                    C_PROCESS,
                                    C_SHEET,
                                    C_NEXT_NODE,
                                    C_NEXT_PROCESS
                                FROM
                                    SAJET.G_RC_STATUS
                                WHERE
                                    RC_NO = TRC;

                            --End Add by Jieke 2016/01/06  for SN獲取途程node异常修复
                            --MODIFY BY NANCY FOR COT 2015/11/12

                                UPDATE SAJET.G_SN_STATUS
                                SET
                                    NODE_ID = C_NODE,
                                    PROCESS_ID = C_PROCESS,
                                    SHEET_NAME = C_SHEET,
                                    TRAVEL_ID = C_TRAVEL_SN + 1,
                                    NEXT_NODE = C_NEXT_NODE,
                                    NEXT_PROCESS = C_NEXT_PROCESS
                                WHERE
                                    RC_NO = TRC;

                            END IF;

                        END IF;

                    ELSE --G_COUNT <= 0
                        BEGIN
                        --Tunny add 獲取C_NODE_TYPE
                            SELECT
                                B.NODE_ID,
                                B.NODE_CONTENT,
                                DECODE(B.NODE_TYPE, 9, 9, 0),
                                B.NODE_TYPE
                            INTO
                                C_NODE,
                                C_PROCESS,
                                C_STATUS,
                                C_NODE_TYPE
                            FROM
                                SAJET.SYS_RC_ROUTE_DETAIL   A,
                                SAJET.SYS_RC_ROUTE_DETAIL   B
                            WHERE
                                A.ROUTE_ID = C_ROUTE
                                AND A.ROUTE_ID = B.ROUTE_ID
                                AND A.LINK_NAME = 'NEXT'
                                AND A.NODE_ID = C_NODE
                                AND A.NEXT_NODE_ID = B.NODE_ID
                                AND ROWNUM = 1;

                            BEGIN
                                SELECT
                                    B.NODE_ID,
                                    B.NODE_CONTENT
                                INTO
                                    C_NEXT_NODE,
                                    C_NEXT_PROCESS
                                FROM
                                    SAJET.SYS_RC_ROUTE_DETAIL   A,
                                    SAJET.SYS_RC_ROUTE_DETAIL   B
                                WHERE
                                    A.ROUTE_ID = C_ROUTE
                                    AND A.ROUTE_ID = B.ROUTE_ID
                                    AND A.LINK_NAME = 'NEXT'
                                    AND A.NODE_ID = C_NODE
                                    AND A.NEXT_NODE_ID = B.NODE_ID
                                    AND ROWNUM = 1;

                            EXCEPTION
                                WHEN OTHERS THEN
                                    NULL;
                            END;

                            IF C_STATUS = 9 THEN
                                C_PROCESS := NULL;
                                C_NEXT_PROCESS := NULL;
                                C_SHEET := NULL;
                                C_NEXT_NODE := NULL;
                            ELSE                    
                            --Tunny add
                            /* 取得下一製程的第一個表單 */
                                IF C_NODE_TYPE = 1 THEN
                                    BEGIN
                                        SELECT
                                            SHEET_NAME
                                        INTO C_SHEET
                                        FROM
                                            SAJET.SYS_RC_PROCESS_SHEET
                                        WHERE
                                            PROCESS_ID = C_PROCESS
                                            AND SHEET_SEQ = 0
                                            AND ROWNUM = 1;

                                    EXCEPTION
                                        WHEN OTHERS THEN
                                            NULL;
                                    END;

                                ELSIF C_NODE_TYPE = 2 OR C_NODE_TYPE = 3 THEN
                                    C_PROCESS := NULL;
                                    C_SHEET := NULL;
                                END IF;
                            --add end                   
                            END IF;

                        EXCEPTION
                            WHEN OTHERS THEN
                                NULL;
                        END;

                    -- 19, 20

                        UPDATE SAJET.G_RC_STATUS
                        SET
                            CURRENT_STATUS = C_STATUS,
                            TRAVEL_ID = C_TRAVEL + 1,
                            NODE_ID = C_NODE,
                            PROCESS_ID = C_PROCESS,
                            SHEET_NAME = C_SHEET,
                            NEXT_NODE = C_NEXT_NODE,
                            NEXT_PROCESS = C_NEXT_PROCESS,
                            UPDATE_USERID = C_EMP,
                            UPDATE_TIME = C_SYSDATE,
                            IN_PROCESS_EMPID = NULL,
                            IN_PROCESS_TIME = NULL,
                            WIP_IN_QTY = NULL,
                            WIP_IN_EMPID = NULL,
                            WIP_IN_MEMO = NULL,
                            WIP_IN_TIME = NULL,
                            WIP_OUT_GOOD_QTY = NULL,
                            WIP_OUT_SCRAP_QTY = NULL,
                            WIP_OUT_EMPID = NULL,
                            WIP_OUT_MEMO = NULL,
                            WIP_OUT_TIME = NULL,
                            OUT_PROCESS_EMPID = NULL,
                            OUT_PROCESS_TIME = NULL,
                            BATCH_ID = NULL
                        WHERE
                            RC_NO = TRC
                            AND ROWNUM = 1;

                    -- 補充處理

                        IF TSN IS NOT NULL THEN
                        --Begin Add by Jieke 2016/01/06  for SN獲取途程node异常修复
                            SELECT
                                NODE_ID,
                                PROCESS_ID,
                                SHEET_NAME,
                                NEXT_NODE,
                                NEXT_PROCESS
                            INTO
                                C_NODE,
                                C_PROCESS,
                                C_SHEET,
                                C_NEXT_NODE,
                                C_NEXT_PROCESS
                            FROM
                                SAJET.G_RC_STATUS
                            WHERE
                                RC_NO = TRC;

                        --End Add by Jieke 2016/01/06  for SN獲取途程node异常修复
                        --MODIFY BY NANCY FOR COT 2015/11/12

                            UPDATE SAJET.G_SN_STATUS
                            SET
                                NODE_ID = C_NODE,
                                PROCESS_ID = C_PROCESS,
                                SHEET_NAME = C_SHEET,
                                TRAVEL_ID = C_TRAVEL_SN + 1,
                                NEXT_NODE = C_NEXT_NODE,
                                NEXT_PROCESS = C_NEXT_PROCESS
                            WHERE
                                RC_NO = TRC;

                        END IF;

                    END IF;
                --End Group執行 Modify by Jieke 2015/7/8

                END IF;
            END IF; -- TNEXTPROCESS
        END IF; -- THOLD

        COMMIT;
    END IF;

    << ENDP >> NULL;
EXCEPTION
    WHEN OTHERS THEN
        TRES := SQLERRM;
        ROLLBACK;
END;