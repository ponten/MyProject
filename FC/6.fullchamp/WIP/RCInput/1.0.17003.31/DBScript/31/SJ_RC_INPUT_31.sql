CREATE OR REPLACE PROCEDURE SJ_RC_INPUT (
    TEMP       IN    VARCHAR2,
    TRC        IN    VARCHAR2,
    TITEM      IN    VARCHAR2,
    TMACHINE   IN    VARCHAR2,
    TSN        IN    VARCHAR2,
    TMEMO      IN    VARCHAR2,
    TKEYPART   IN    VARCHAR2,   --PART_NO+CHR(9)+KPSN+CHR(9)+ITEM_COUNT+CHR(27)+...                                         
    TNOW       IN    DATE,
    TRES       OUT   VARCHAR2
) IS

    C_EMP             SAJET.SYS_EMP.EMP_ID%TYPE;
    C_TEMP            VARCHAR2(1024);
    C_END             NUMBER;
    C_ITEMTEMP        VARCHAR2(100);
    C_ITEM            VARCHAR2(100);
    C_VALUE           SAJET.G_RC_PROCESS_PARAM.ITEM_VALUE%TYPE;
    C_WO              SAJET.G_RC_STATUS.WORK_ORDER%TYPE;
    C_TRAVEL          SAJET.G_RC_STATUS.TRAVEL_ID%TYPE;
    C_PROCESS         SAJET.G_RC_STATUS.PROCESS_ID%TYPE;
    C_PART            SAJET.G_RC_STATUS.PART_ID%TYPE;
    C_CURRENT_SHEET   SAJET.SYS_RC_PROCESS_SHEET.SHEET_NAME%TYPE;
    C_SHEET           SAJET.SYS_RC_PROCESS_SHEET.SHEET_NAME%TYPE;
    C_SN              SAJET.G_RC_TRAVEL_PARAM.SERIAL_NUMBER%TYPE;
    C_SNTEMP          SAJET.G_RC_TRAVEL_PARAM.SERIAL_NUMBER%TYPE;
    C_NOW             DATE;
    C_COUNT           SAJET.G_RC_KEYPARTS.ITEM_COUNT%TYPE;
    C_BOM_ID          NUMBER;
    C_SYSDATE         DATE;
/*
V2:1.增加物料收集功能
V3:1.增加手動過站時間參數
V4:1.機台狀態不改變，為了一個機台可以同時生產多個流程卡
20171025-RC INPUT時填入 WO_START_TIME
*/
BEGIN
    C_NOW := TNOW;
    C_SYSDATE := SYSDATE;
   /* 檢查員工 */
    SAJET.SJ_CKSYS_EMP_OUT(TEMP, TRES, C_EMP);
    IF ( TRES = 'OK' ) THEN
        SELECT
            WORK_ORDER,
            PROCESS_ID,
            PART_ID,
            TRAVEL_ID,
            SHEET_NAME
        INTO
            C_WO,
            C_PROCESS,
            C_PART,
            C_TRAVEL,
            C_CURRENT_SHEET
        FROM
            SAJET.G_RC_STATUS
        WHERE
            RC_NO = TRC
            AND ROWNUM = 1;

        SELECT
            WO_OPTION2
        INTO C_BOM_ID
        FROM
            SAJET.G_WO_BASE
        WHERE
            WORK_ORDER = C_WO
            AND ROWNUM = 1;

      /* 製程參數收集 */

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
                        C_NOW,
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
                        C_NOW
                    FROM
                        SAJET.SYS_MACHINE
                    WHERE
                        MACHINE_CODE = C_ITEM
                        AND ROWNUM = 1;
            
            -- RC投入代表機台狀態為RUN
            
            --V4
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
      /* 尋找下一個製程表單，目前固定抓NEXT，待調整 */

        SAJET.SJ_RC_GET_NEXTSHEET(C_PROCESS, C_CURRENT_SHEET, 'NEXT', C_SHEET);

      /* 序號數據收集 */
        IF TSN IS NOT NULL THEN
            C_TEMP := TSN;
            C_SNTEMP := 'N/A';
            LOOP
                EXIT WHEN C_TEMP IS NULL;
                C_ITEMTEMP := SUBSTR(C_TEMP, 1, INSTR(C_TEMP, CHR(27)) - 1);

                C_TEMP := REPLACE(C_TEMP, C_ITEMTEMP || CHR(27), '');
                C_SN := SUBSTR(C_ITEMTEMP, 1, INSTR(C_ITEMTEMP, CHR(9)) - 1);

                C_ITEMTEMP := REPLACE(C_ITEMTEMP, C_SN || CHR(9), '');
                C_ITEM := SUBSTR(C_ITEMTEMP, 1, INSTR(C_ITEMTEMP, CHR(9)) - 1);

                C_VALUE := SUBSTR(C_ITEMTEMP, INSTR(C_ITEMTEMP, CHR(9)) + 1);

            /* 序號的狀態修改為生產中 */

                IF C_SN <> C_SNTEMP THEN
                    UPDATE SAJET.G_SN_STATUS
                    SET
                        CURRENT_STATUS = 1,
                        SHEET_NAME = C_SHEET,
                        UPDATE_USERID = C_EMP,
                        UPDATE_TIME = C_NOW
                    WHERE
                        SERIAL_NUMBER = C_SN
                        AND ROWNUM = 1;

                END IF;

                C_SNTEMP := C_SN;

            /* 序號製程參數收集 */
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
                        C_NOW,
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
            UPDATE SAJET.G_SN_STATUS
            SET
                CURRENT_STATUS = 1,
                SHEET_NAME = C_SHEET,
                UPDATE_USERID = C_EMP,
                UPDATE_TIME = C_NOW
            WHERE
                RC_NO = TRC
                AND CURRENT_STATUS = 0;

        END IF;

      
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
                        C_NOW,
                        'Y',
                        C_COUNT
                    FROM
                        SAJET.SYS_BOM        A,
                        SAJET.SYS_PART       B,
                        SAJET.SYS_BOM_INFO   C
                    WHERE
                        A.ITEM_PART_ID = B.PART_ID
                        AND A.BOM_ID = C.BOM_ID
                        AND C.PART_ID = C_BOM_ID
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
                        C_NOW,
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


      /* 流程卡狀態修改為生產中 */

        UPDATE SAJET.G_RC_STATUS
        SET
            CURRENT_STATUS = 1,
            SHEET_NAME = C_SHEET,
            WIP_IN_QTY = CURRENT_QTY,
            WIP_IN_EMPID = C_EMP,
            IN_PROCESS_EMPID = C_EMP,
            WIP_IN_MEMO = TMEMO,
            WIP_IN_TIME = C_NOW,
            IN_PROCESS_TIME = C_NOW,
            WIP_OUT_EMPID = NULL,
            OUT_PROCESS_EMPID = NULL,
            WIP_OUT_TIME = NULL,
            OUT_PROCESS_TIME = NULL,
            WIP_OUT_MEMO = NULL,
            UPDATE_USERID = C_EMP,
            UPDATE_TIME = C_SYSDATE
        WHERE
            RC_NO = TRC
            AND ROWNUM = 1;

      /* 如果此製程是群組中的一站，修改為群組狀態為執行中 */

        UPDATE SAJET.G_RC_PROCESS_GROUP
        SET
            STATUS = 'Running'
        WHERE
            RC_NO = TRC
            AND PROCESS_ID = C_PROCESS;
       
       --20171025-RC INPUT時Insert WO_START_TIME

        UPDATE SAJET.G_WO_BASE
        SET
            WO_START_DATE = NVL(WO_START_DATE, C_NOW)
        WHERE
            WORK_ORDER = C_WO;

        COMMIT;
    END IF;

EXCEPTION
    WHEN OTHERS THEN
        TRES := SQLERRM;
        ROLLBACK;
END;