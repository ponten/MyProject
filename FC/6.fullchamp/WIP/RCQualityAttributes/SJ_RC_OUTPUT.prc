CREATE OR REPLACE PROCEDURE SJ_RC_OUTPUT(TEMP    IN VARCHAR2,
                                         TRC     IN VARCHAR2,
                                         TITEM   IN VARCHAR2,
                                         TDEFECT IN VARCHAR2,
                                         TSN     IN VARCHAR2,
                                         TMEMO   IN VARCHAR2,
                                         TGOOD   IN NUMBER,
                                         TSCRAP  IN NUMBER,
                                         TRES    OUT VARCHAR2) IS
  C_EMP  SAJET.SYS_EMP.EMP_ID%TYPE;
  C_TEMP VARCHAR2(2048);
  /*C_END NUMBER;*/
  C_ITEMTEMP     VARCHAR2(2048);
  C_ITEM         SAJET.G_RC_TRAVEL_PARAM.ITEM_ID%TYPE;
  C_VALUE       /*SAJET.G_RC_PROCESS_PARAM.ITEM_VALUE%TYPE*/
  VARCHAR2(200); --MODIFY BY HIDY 2015/10/28
  C_WO           SAJET.G_RC_STATUS.WORK_ORDER%TYPE;
  C_TRAVEL_RC    SAJET.G_RC_STATUS.TRAVEL_ID%TYPE; --Tunny修改
  C_TRAVEL_SN    SAJET.G_SN_STATUS.TRAVEL_ID%TYPE; --Tunny添加
  C_NODE         SAJET.G_RC_STATUS.NODE_ID%TYPE;
  C_PROCESS      SAJET.SYS_RC_ROUTE_DETAIL.NODE_CONTENT%TYPE;
  C_NEXT_NODE    SAJET.G_RC_STATUS.NODE_ID%TYPE;
  C_NEXT_PROCESS SAJET.G_RC_STATUS.PROCESS_ID%TYPE;
  C_PART         SAJET.G_RC_STATUS.PART_ID%TYPE;
  C_SHEET        SAJET.SYS_RC_PROCESS_SHEET.SHEET_NAME%TYPE;
  C_DEFECT      /*SAJET.SYS_DEFECT.DEFECT_CODE%TYPE*/
  varchar2(100);
  C_SN           SAJET.G_RC_TRAVEL_PARAM.SERIAL_NUMBER%TYPE;
  C_SNTEMP       SAJET.G_RC_TRAVEL_PARAM.SERIAL_NUMBER%TYPE;
  C_ROUTE        SAJET.G_RC_STATUS.ROUTE_ID%TYPE;
  /*C_STATIS SAJET.G_RC_STATUS.CURRENT_STATUS%TYPE;*/
  C_NOW        DATE;
  C_STATUS     VARCHAR2(200); --MODIFY BY HIDY 2015/10/28
  CC_STATUS    VARCHAR2(10);
  C_OK         SAJET.G_SN_STATUS.GOOD_QTY%TYPE;
  C_NG         SAJET.G_SN_STATUS.SCRAP_QTY%TYPE;
  C_GOOD       NUMBER;
  C_SCRAP      NUMBER;
  G_COUNT      NUMBER; --Add byJieke 2015/7/8 for Group執行
  G_GROUP      NUMBER; --Add byJieke 2015/7/8 for Group執行
  G_TYPE       VARCHAR2(5); --Add byJieke 2015/7/8 for Group執行
  Q_NUM        NUMBER; --ADD BY HIDY 2015/10/16     QC
  Q_DEFECTTYPE VARCHAR2(200); --ADD BY HIDY 2015/10/16     QC
  Q_ANALYSIS   VARCHAR2(200); --ADD BY HIDY 2015/10/16     QC
  Q_MEASURE    VARCHAR2(200); --ADD BY HIDY 2015/10/16     QC
  C_NODE_TYPE  SAJET.SYS_RC_ROUTE_DETAIL.NODE_TYPE%TYPE; --Add by Aaron 2015/8/15 for Group執行
  /*C_RC         NUMBER; --MODIFY BY HIDY 2015/10/30
  CC_NUM       NUMBER; --MODIFY BY HIDY 2015/10/30*/ --注释by hidy 2015/11/07调整为input计次
  SN_WO SAJET.G_SN_STATUS.WORK_ORDER%TYPE; --Add by Jieke 2015/12/10 for Update sajet.g_sn_property WO,RC
  C_COUNT   NUMBER; --Add by Jieke 2016/01/06  for SN获取途程node异常修复
  C_QCLOTNO VARCHAR2(50);
BEGIN
  C_NOW := SYSDATE;
  /*C_RC := 0;--MODIFY BY HIDY 2015/10/30*/ --注释by hidy 2015/11/07调整为input计次
  SAJET.SJ_CKSYS_EMP_OUT(TEMP, TRES, C_EMP);
  IF (TRES = 'OK') THEN
    SELECT WORK_ORDER, PROCESS_ID, PART_ID, TRAVEL_ID, NODE_ID, ROUTE_ID
      INTO C_WO, C_PROCESS, C_PART, C_TRAVEL_RC, C_NODE, C_ROUTE
      FROM SAJET.G_RC_STATUS
     WHERE RC_NO = TRC
       AND ROWNUM = 1;
    if TSN is not null then
      SELECT TRAVEL_ID
        INTO C_TRAVEL_SN
        FROM SAJET.G_SN_STATUS
       WHERE RC_NO = TRC
         AND ROWNUM = 1;
      SELECT DECODE(B.NODE_TYPE, 9, 9, 0)
        INTO CC_STATUS
        FROM SAJET.SYS_RC_ROUTE_DETAIL A, SAJET.SYS_RC_ROUTE_DETAIL B
       WHERE A.ROUTE_ID = C_ROUTE
         AND A.ROUTE_ID = B.ROUTE_ID
         AND A.LINK_NAME = 'NEXT'
         AND A.NODE_ID = C_NODE
         AND A.NEXT_NODE_ID = B.NODE_ID
         AND ROWNUM = 1;
    end if;
    IF TITEM IS NOT NULL THEN
      C_TEMP := TITEM;
      LOOP
        EXIT WHEN C_TEMP IS NULL;
        C_ITEMTEMP := SUBSTR(C_TEMP, 1, INSTR(C_TEMP, CHR(27)) - 1);
        C_ITEM     := SUBSTR(C_ITEMTEMP, 1, INSTR(C_ITEMTEMP, CHR(9)) - 1);
        C_VALUE    := SUBSTR(C_ITEMTEMP, INSTR(C_ITEMTEMP, CHR(9)) + 1);
        C_TEMP     := REPLACE(C_TEMP, C_ITEMTEMP || CHR(27), '');
        INSERT INTO SAJET.G_RC_TRAVEL_PARAM
          (RC_NO,
           TRAVEL_ID,
           ITEM_ID,
           ITEM_NAME,
           ITEM_VALUE,
           UPDATE_USERID,
           UPDATE_TIME,
           ITEM_TYPE,
           VALUE_TYPE,
           ITEM_PHASE)
          SELECT TRC,
                 C_TRAVEL_RC,
                 ITEM_ID,
                 ITEM_NAME,
                 C_VALUE,
                 C_EMP,
                 C_NOW,
                 ITEM_TYPE,
                 VALUE_TYPE,
                 ITEM_PHASE
            FROM SAJET.SYS_RC_PROCESS_PARAM_PART
           WHERE PROCESS_ID = C_PROCESS
             AND PART_ID = C_PART
             AND ITEM_ID = C_ITEM
             AND ROWNUM = 1;
      END LOOP;
    END IF;
    C_GOOD  := 0;
    C_SCRAP := 0;
    IF TSN IS NOT NULL THEN
      C_TEMP   := TSN;
      C_SNTEMP := 'N/A';
      LOOP
        EXIT WHEN C_TEMP IS NULL;
        C_ITEMTEMP := SUBSTR(C_TEMP, 1, INSTR(C_TEMP, CHR(27)) - 1);
        C_TEMP     := REPLACE(C_TEMP, C_ITEMTEMP || CHR(27), '');
        C_SN       := SUBSTR(C_ITEMTEMP, 1, INSTR(C_ITEMTEMP, CHR(9)) - 1);
        C_ITEMTEMP := REPLACE(C_ITEMTEMP, C_SN || CHR(9), '');
        C_STATUS   := SUBSTR(C_ITEMTEMP, 1, INSTR(C_ITEMTEMP, CHR(9)) - 1);
        C_ITEMTEMP := REPLACE(C_ITEMTEMP, C_STATUS || CHR(9), '');
        C_OK       := SUBSTR(C_ITEMTEMP, 1, INSTR(C_ITEMTEMP, CHR(9)) - 1);
        C_ITEMTEMP := REPLACE(C_ITEMTEMP, C_OK || CHR(9), '');
        C_NG       := SUBSTR(C_ITEMTEMP, 1, INSTR(C_ITEMTEMP, CHR(9)) - 1);
        C_ITEMTEMP := REPLACE(C_ITEMTEMP, C_NG || CHR(9), '');
        C_ITEM     := SUBSTR(C_ITEMTEMP, 1, INSTR(C_ITEMTEMP, CHR(9)) - 1);
        C_VALUE    := SUBSTR(C_ITEMTEMP, INSTR(C_ITEMTEMP, CHR(9)) + 1);
        IF C_SN <> C_SNTEMP THEN
          IF C_STATUS = 'OK' THEN
            C_GOOD := C_GOOD + 1;
            IF CC_STATUS = 9 THEN
              C_STATUS := 9;
            END IF;
          ELSIF C_STATUS = 'NG' THEN
            C_SCRAP := C_SCRAP + 1;
          END IF;
          /*--Begin Delete by Jieke 2015/11/16 for SN过站OUT_PROCES_TIME,WIP_OUT_TIME时间更新
           UPDATE SAJET.G_SN_STATUS
              SET GOOD_QTY       = C_OK,
                  SCRAP_QTY      = C_NG,
                  UPDATE_USERID  = C_EMP,
                  UPDATE_TIME    = C_NOW,
                  CURRENT_STATUS = DECODE(C_STATUS,
                                          'NG',
                                          8,
                                          'OK',
                                          0,
                                          C_STATUS),
                  PROCESS_ID     = C_PROCESS
            WHERE SERIAL_NUMBER = C_SN
              AND ROWNUM = 1;
          --End Delete by Jieke 2015/11/16 for SN过站OUTPROCES_TIME,WIP_OUT_TIME时间更新*/
          --Begin Add by Jieke 2015/11/16 for SN过站OUT_PROCES_TIME,WIP_OUT_TIME时间更新
          UPDATE SAJET.G_SN_STATUS
             SET GOOD_QTY          = C_OK,
                 SCRAP_QTY         = C_NG,
                 UPDATE_USERID     = C_EMP,
                 UPDATE_TIME       = C_NOW,
                 CURRENT_STATUS    = DECODE(C_STATUS,
                                            'NG',
                                            8,
                                            'OK',
                                            0,
                                            C_STATUS),
                 PROCESS_ID        = C_PROCESS,
                 OUT_PROCESS_TIME  = SYSDATE,
                 WIP_OUT_TIME      = SYSDATE,
                 OUT_PROCESS_EMPID = C_EMP,
                 WIP_OUT_EMPID     = C_EMP
           WHERE SERIAL_NUMBER = C_SN
             AND ROWNUM = 1;
          --End Add by Jieke 2015/11/16 for SN过站OUT_PROCES_TIME,WIP_OUT_TIME时间更新
          INSERT INTO SAJET.G_SN_TRAVEL
            (WORK_ORDER,
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
             CREATE_TIME)
            SELECT WORK_ORDER,
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
              FROM SAJET.G_SN_STATUS
             WHERE SERIAL_NUMBER = C_SN
               AND ROWNUM = 1;
        END IF;
        C_SNTEMP := C_SN;
        INSERT INTO SAJET.G_RC_TRAVEL_PARAM
          (RC_NO,
           TRAVEL_ID,
           SERIAL_NUMBER,
           ITEM_ID,
           ITEM_NAME,
           ITEM_VALUE,
           UPDATE_USERID,
           UPDATE_TIME,
           ITEM_TYPE,
           VALUE_TYPE,
           ITEM_PHASE)
          SELECT TRC,
                 C_TRAVEL_RC,
                 C_SN,
                 ITEM_ID,
                 ITEM_NAME,
                 C_VALUE,
                 C_EMP,
                 C_NOW,
                 ITEM_TYPE,
                 VALUE_TYPE,
                 ITEM_PHASE
            FROM SAJET.SYS_RC_PROCESS_PARAM_PART
           WHERE PROCESS_ID = C_PROCESS
             AND PART_ID = C_PART
             AND ITEM_ID = C_ITEM
             AND ROWNUM = 1;
      END LOOP;
    ELSE
      C_GOOD  := TGOOD;
      C_SCRAP := TSCRAP;
    END IF;
    IF TDEFECT IS NOT NULL THEN
      C_TEMP := TDEFECT;
      LOOP
        EXIT WHEN C_TEMP IS NULL;
        C_ITEMTEMP := SUBSTR(C_TEMP, 1, INSTR(C_TEMP, CHR(27)) - 1);
        C_TEMP     := REPLACE(C_TEMP, C_ITEMTEMP || CHR(27), '');
        IF TSN IS NOT NULL THEN
          C_SN       := SUBSTR(C_ITEMTEMP, 1, INSTR(C_ITEMTEMP, CHR(9)) - 1);
          C_ITEMTEMP := REPLACE(C_ITEMTEMP, C_SN || CHR(9), '');
        ELSE
          C_SN := NULL;
        END IF;
        C_DEFECT   := SUBSTR(C_ITEMTEMP, 1, INSTR(C_ITEMTEMP, CHR(9)) - 1);
        C_ITEMTEMP := REPLACE(C_ITEMTEMP, C_DEFECT || CHR(9), '');
      
        --modify by hidy 2015.10.16 QC
        Q_NUM := INSTR(C_ITEMTEMP, CHR(9), 1, 2);
        IF Q_NUM = 0 THEN
          C_VALUE := SUBSTR(C_ITEMTEMP, INSTR(C_ITEMTEMP, CHR(9)) + 1);
          INSERT INTO SAJET.G_RC_TRAVEL_DEFECT
            (RC_NO,
             TRAVEL_ID,
             SERIAL_NUMBER,
             PROCESS_ID,
             DEFECT_ID,
             DEFECT_LEVEL,
             DEFECT_TYPE_ID,
             QTY,
             UPDATE_USERID,
             UPDATE_TIME)
            SELECT TRC,
                   C_TRAVEL_RC,
                   C_SN,
                   C_PROCESS,
                   DEFECT_ID,
                   DEFECT_LEVEL,
                   DEFECT_TYPE_ID,
                   C_VALUE,
                   C_EMP,
                   C_NOW
              FROM SAJET.SYS_DEFECT
             WHERE DEFECT_CODE = C_DEFECT
               AND ROWNUM = 1;
        ELSE
          C_VALUE      := SUBSTR(C_ITEMTEMP,
                                 1,
                                 INSTR(C_ITEMTEMP, CHR(9)) - 1);
          C_ITEMTEMP   := REPLACE(C_ITEMTEMP, C_VALUE || CHR(9), '');
          Q_DEFECTTYPE := SUBSTR(C_ITEMTEMP,
                                 1,
                                 INSTR(C_ITEMTEMP, CHR(9)) - 1);
          C_ITEMTEMP   := REPLACE(C_ITEMTEMP, Q_DEFECTTYPE || CHR(9), '');
          Q_ANALYSIS   := SUBSTR(C_ITEMTEMP,
                                 1,
                                 INSTR(C_ITEMTEMP, CHR(9)) - 1);
          C_ITEMTEMP   := REPLACE(C_ITEMTEMP, Q_ANALYSIS || CHR(9), '');
          Q_MEASURE    := SUBSTR(C_ITEMTEMP, INSTR(C_ITEMTEMP, CHR(9)) + 1);
          INSERT INTO SAJET.G_RC_TRAVEL_DEFECT
            (RC_NO,
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
             MEASURE)
            SELECT TRC,
                   C_TRAVEL_RC,
                   C_SN,
                   C_PROCESS,
                   DEFECT_ID,
                   DEFECT_LEVEL,
                   DEFECT_TYPE_ID,
                   C_VALUE,
                   C_EMP,
                   C_NOW,
                   Q_DEFECTTYPE,
                   Q_ANALYSIS,
                   Q_MEASURE
              FROM SAJET.SYS_DEFECT
             WHERE DEFECT_CODE = C_DEFECT
               AND ROWNUM = 1;
        END IF;
      END LOOP;
    END IF;
  
    --NANCY ADD 2016.4.5
    --如果是QC测试站，则清除QC临时记录，判定QC最终结果
    C_QCLOTNO := TRC || TO_CHAR(C_PROCESS);
    SELECT COUNT(*)
      INTO C_COUNT
      FROM SAJET.G_QC_LOT
     WHERE QC_LOTNO LIKE C_QCLOTNO || '%';
    IF C_COUNT > 0 THEN
      BEGIN
        SELECT QC_LOTNO
          INTO C_QCLOTNO
          FROM SAJET.G_QC_LOT
         WHERE QC_LOTNO LIKE C_QCLOTNO || '%'
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
  
    --MODYFI BY HIDY 2015/10/06 修改机台的状态（RUNNING/IDLE）
    SAJET.SJ_RC_OUTPUT_MACHINE_STATUS(TRC, C_EMP, C_TRAVEL_RC);
  
    --MODIFY BY HIDY 2015/10/12 机台绑定零配件，修改零配件的使用次数
    /*SJ_RC_MACHINE_TOOLING(TRC,C_EMP,C_TRAVEL,C_RC,CC_NUM);
    C_RC := CC_NUM;*/ --注释by hidy 2015/11/07调整为input计次
  
    -- 14.
    UPDATE SAJET.G_RC_TRAVEL_MACHINE
       SET END_TIME = C_NOW, UPDATE_USERID = C_EMP, UPDATE_TIME = C_NOW
     WHERE RC_NO = TRC
       AND TRAVEL_ID = C_TRAVEL_RC
       AND END_TIME IS NULL;
  
    -- 12, 13
    UPDATE SAJET.G_RC_STATUS
       SET WIP_OUT_GOOD_QTY  = C_GOOD,
           WIP_OUT_SCRAP_QTY = C_SCRAP,
           WIP_OUT_EMPID     = C_EMP,
           WIP_OUT_MEMO      = TMEMO,
           WIP_OUT_TIME      = C_NOW,
           CURRENT_QTY       = C_GOOD,
           CURRENT_STATUS    = DECODE(C_GOOD, 0, 8, CURRENT_STATUS)
     WHERE RC_NO = TRC
       AND ROWNUM = 1;
    -- 17
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
       CREATE_TIME)
      SELECT WORK_ORDER,
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
             CREATE_TIME
        FROM SAJET.G_RC_STATUS
       WHERE RC_NO = TRC
         AND ROWNUM = 1;
    IF TGOOD > 0 THEN
      C_STATUS := 0;
      --Begin Group執行 Modify by Jieke 2015/7/8
      SELECT COUNT(*)
        INTO G_COUNT
        FROM SAJET.G_RC_PROCESS_GROUP
       WHERE RC_NO = TRC;
      IF G_COUNT > 0 THEN
        SELECT PROCESS_TYPE
          INTO G_TYPE
          FROM SAJET.G_RC_PROCESS_GROUP
         WHERE RC_NO = TRC
           AND ROWNUM = 1;
        IF G_TYPE = 'and' THEN
          UPDATE SAJET.G_RC_PROCESS_GROUP
             SET STATUS = 'Complete', SHEET_PHASE = 'N/A'
           WHERE RC_NO = TRC
             AND STATUS = 'Running'
             AND SHEET_PHASE = 'O';
        ELSE
          UPDATE SAJET.G_RC_PROCESS_GROUP
             SET STATUS = 'Complete', SHEET_PHASE = 'N/A'
           WHERE RC_NO = TRC;
        END IF;
      
        SELECT COUNT(*)
          INTO G_COUNT
          FROM SAJET.G_RC_PROCESS_GROUP
         WHERE RC_NO = TRC
           AND STATUS = 'Queue';
        IF G_COUNT > 0 AND G_TYPE != 'or' THEN
          SELECT GROUP_ID
            INTO G_GROUP
            FROM SAJET.G_RC_PROCESS_GROUP
           WHERE RC_NO = TRC
             AND ROWNUM = 1;
          --Tunny add 2015/11/2
          if C_TRAVEL_SN is not null then
            UPDATE SAJET.G_SN_STATUS
               SET PROCESS_ID     = '',
                   CURRENT_STATUS = '0',
                   SHEET_NAME     = '',
                   TRAVEL_ID      = C_TRAVEL_SN + 1
             WHERE RC_NO = TRC;
          end if;
          UPDATE SAJET.G_RC_STATUS
             SET PROCESS_ID     = '',
                 CURRENT_STATUS = '0',
                 SHEET_NAME     = '',
                 TRAVEL_ID      = C_TRAVEL_RC + 1
           WHERE RC_NO = TRC;
          --Tunny add end
        
        ELSE
          BEGIN
            DELETE SAJET.G_RC_PROCESS_GROUP WHERE RC_NO = TRC; --跳出Group后清除RC group數據
            --Tunny 添加，获取C_NODE_TYPE
            SELECT B.NODE_ID,
                   B.NODE_CONTENT,
                   DECODE(B.NODE_TYPE, 9, 9, 0),
                   B.NODE_TYPE
              INTO C_NODE, C_PROCESS, C_STATUS, C_NODE_TYPE
              FROM SAJET.SYS_RC_ROUTE_DETAIL A, SAJET.SYS_RC_ROUTE_DETAIL B
             WHERE A.ROUTE_ID = C_ROUTE
               AND A.ROUTE_ID = B.ROUTE_ID
               AND A.LINK_NAME = 'NEXT'
               AND A.NODE_ID = C_NODE
               AND A.NEXT_NODE_ID = B.NODE_ID
               AND ROWNUM = 1;
            BEGIN
              SELECT B.NODE_ID, B.NODE_CONTENT
                INTO C_NEXT_NODE, C_NEXT_PROCESS
                FROM SAJET.SYS_RC_ROUTE_DETAIL A,
                     SAJET.SYS_RC_ROUTE_DETAIL B
               WHERE A.ROUTE_ID = C_ROUTE
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
              C_PROCESS      := NULL;
              C_NEXT_PROCESS := NULL;
              C_SHEET        := NULL;
              C_NEXT_NODE    := NULL;
            ELSE
              BEGIN
                SELECT SHEET_NAME
                  INTO C_SHEET
                  FROM SAJET.SYS_RC_PROCESS_SHEET
                 WHERE PROCESS_ID = C_PROCESS
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
             SET CURRENT_STATUS    = C_STATUS,
                 TRAVEL_ID         = C_TRAVEL_RC + 1,
                 NODE_ID           = C_NODE,
                 PROCESS_ID        = C_PROCESS,
                 SHEET_NAME        = C_SHEET,
                 NEXT_NODE         = C_NEXT_NODE,
                 NEXT_PROCESS      = C_NEXT_PROCESS,
                 UPDATE_USERID     = C_EMP,
                 UPDATE_TIME       = C_NOW,
                 IN_PROCESS_EMPID  = NULL,
                 IN_PROCESS_TIME   = NULL,
                 WIP_IN_QTY        = NULL,
                 WIP_IN_EMPID      = NULL,
                 WIP_IN_MEMO       = NULL,
                 WIP_IN_TIME       = NULL,
                 WIP_OUT_GOOD_QTY  = NULL,
                 WIP_OUT_SCRAP_QTY = NULL,
                 WIP_OUT_EMPID     = NULL,
                 WIP_OUT_MEMO      = NULL,
                 WIP_OUT_TIME      = NULL,
                 OUT_PROCESS_EMPID = NULL,
                 OUT_PROCESS_TIME  = NULL
           WHERE RC_NO = TRC
             AND ROWNUM = 1;
          -- 補充處理
          IF TSN IS NOT NULL THEN
            --Begin Add by Jieke 2016/01/06  for SN获取途程node异常修复
            SELECT NODE_ID, PROCESS_ID, SHEET_NAME, NEXT_NODE, NEXT_PROCESS
              INTO C_NODE, C_PROCESS, C_SHEET, C_NEXT_NODE, C_NEXT_PROCESS
              FROM SAJET.G_RC_STATUS
             WHERE RC_NO = TRC;
            --End Add by Jieke 2016/01/06  for SN获取途程node异常修复
            --MODIFY BY NANCY FOR COT 2015/11/12
            IF C_PROCESS = 100373 THEN
              UPDATE SAJET.G_SN_STATUS
                 SET NODE_ID      = C_NODE,
                     PROCESS_ID   = C_PROCESS,
                     SHEET_NAME   = C_SHEET,
                     TRAVEL_ID    = C_TRAVEL_SN + 1,
                     NEXT_NODE    = C_NEXT_NODE,
                     NEXT_PROCESS = C_NEXT_PROCESS,
                     RC_NO        = 'N/A'
               WHERE RC_NO = TRC;
              --Begin Add  by Jike 2015/12/10 for Update sajet.g_sn_property WO,RC
              select work_order
                into sn_wo
                from sajet.g_sn_status
               where serial_number = C_SN;
              --update sajet.g_sn_property SET CHIP_RC = TRC, CHIP_WO_2 = sn_wo where CHIP_SN = C_SN;
              --Begin End  by Jike 2015/12/10 for Update sajet.g_sn_property WO,RC
            ELSE
              UPDATE SAJET.G_SN_STATUS
                 SET NODE_ID      = C_NODE,
                     PROCESS_ID   = C_PROCESS,
                     SHEET_NAME   = C_SHEET,
                     TRAVEL_ID    = C_TRAVEL_SN + 1,
                     NEXT_NODE    = C_NEXT_NODE,
                     NEXT_PROCESS = C_NEXT_PROCESS
               WHERE RC_NO = TRC;
            END IF;
          END IF;
        END IF;
      ELSE
        BEGIN
          --Tunny add 获取C_NODE_TYPE
          SELECT B.NODE_ID,
                 B.NODE_CONTENT,
                 DECODE(B.NODE_TYPE, 9, 9, 0),
                 B.NODE_TYPE
            INTO C_NODE, C_PROCESS, C_STATUS, C_NODE_TYPE
            FROM SAJET.SYS_RC_ROUTE_DETAIL A, SAJET.SYS_RC_ROUTE_DETAIL B
           WHERE A.ROUTE_ID = C_ROUTE
             AND A.ROUTE_ID = B.ROUTE_ID
             AND A.LINK_NAME = 'NEXT'
             AND A.NODE_ID = C_NODE
             AND A.NEXT_NODE_ID = B.NODE_ID
             AND ROWNUM = 1;
          BEGIN
            SELECT B.NODE_ID, B.NODE_CONTENT
              INTO C_NEXT_NODE, C_NEXT_PROCESS
              FROM SAJET.SYS_RC_ROUTE_DETAIL A, SAJET.SYS_RC_ROUTE_DETAIL B
             WHERE A.ROUTE_ID = C_ROUTE
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
            C_PROCESS      := NULL;
            C_NEXT_PROCESS := NULL;
            C_SHEET        := NULL;
            C_NEXT_NODE    := NULL;
          ELSE
          
            --Tunny add
            /* 取得下一製程的第一個表單 */
            IF C_NODE_TYPE = 1 THEN
              BEGIN
                SELECT SHEET_NAME
                  INTO C_SHEET
                  FROM SAJET.SYS_RC_PROCESS_SHEET
                 WHERE PROCESS_ID = C_PROCESS
                   AND SHEET_SEQ = 0
                   AND ROWNUM = 1;
              EXCEPTION
                WHEN OTHERS THEN
                  NULL;
              END;
            ELSIF C_NODE_TYPE = 2 OR C_NODE_TYPE = 3 THEN
              C_PROCESS := NULL;
              C_SHEET   := NULL;
            END IF;
            --add end
          
            --Tunny注释
            /*          BEGIN
              SELECT SHEET_NAME INTO C_SHEET FROM SAJET.SYS_RC_PROCESS_SHEET
                WHERE PROCESS_ID = C_PROCESS AND SHEET_SEQ = 0 AND ROWNUM = 1;
            EXCEPTION
              WHEN OTHERS THEN
                NULL;
            END;*/
          
          END IF;
        EXCEPTION
          WHEN OTHERS THEN
            NULL;
        END;
        -- 19, 20
        UPDATE SAJET.G_RC_STATUS
           SET CURRENT_STATUS    = C_STATUS,
               TRAVEL_ID         = C_TRAVEL_RC + 1,
               NODE_ID           = C_NODE,
               PROCESS_ID        = C_PROCESS,
               SHEET_NAME        = C_SHEET,
               NEXT_NODE         = C_NEXT_NODE,
               NEXT_PROCESS      = C_NEXT_PROCESS,
               UPDATE_USERID     = C_EMP,
               UPDATE_TIME       = C_NOW,
               IN_PROCESS_EMPID  = NULL,
               IN_PROCESS_TIME   = NULL,
               WIP_IN_QTY        = NULL,
               WIP_IN_EMPID      = NULL,
               WIP_IN_MEMO       = NULL,
               WIP_IN_TIME       = NULL,
               WIP_OUT_GOOD_QTY  = NULL,
               WIP_OUT_SCRAP_QTY = NULL,
               WIP_OUT_EMPID     = NULL,
               WIP_OUT_MEMO      = NULL,
               WIP_OUT_TIME      = NULL,
               OUT_PROCESS_EMPID = NULL,
               OUT_PROCESS_TIME  = NULL,
               BATCH_ID          = NULL
         WHERE RC_NO = TRC
           AND ROWNUM = 1;
        -- 補充處理
        IF TSN IS NOT NULL THEN
          --Begin Add by Jieke 2016/01/06  for SN获取途程node异常修复
          SELECT NODE_ID, PROCESS_ID, SHEET_NAME, NEXT_NODE, NEXT_PROCESS
            INTO C_NODE, C_PROCESS, C_SHEET, C_NEXT_NODE, C_NEXT_PROCESS
            FROM SAJET.G_RC_STATUS
           WHERE RC_NO = TRC;
          --End Add by Jieke 2016/01/06  for SN获取途程node异常修复
          --MODIFY BY NANCY FOR COT 2015/11/12
          IF C_PROCESS = 100373 THEN
            UPDATE SAJET.G_SN_STATUS
               SET NODE_ID      = C_NODE,
                   PROCESS_ID   = C_PROCESS,
                   SHEET_NAME   = C_SHEET,
                   TRAVEL_ID    = C_TRAVEL_SN + 1,
                   NEXT_NODE    = C_NEXT_NODE,
                   NEXT_PROCESS = C_NEXT_PROCESS,
                   RC_NO        = 'N/A'
             WHERE RC_NO = TRC;
          ELSE
            UPDATE SAJET.G_SN_STATUS
               SET NODE_ID      = C_NODE,
                   PROCESS_ID   = C_PROCESS,
                   SHEET_NAME   = C_SHEET,
                   TRAVEL_ID    = C_TRAVEL_SN + 1,
                   NEXT_NODE    = C_NEXT_NODE,
                   NEXT_PROCESS = C_NEXT_PROCESS
             WHERE RC_NO = TRC;
          END IF;
        END IF;
      END IF;
      --End Group執行 Modify by Jieke 2015/7/8
    END IF;
    COMMIT;
  END IF;
  <<ENDP>>
  NULL;
EXCEPTION
  WHEN OTHERS THEN
    TRES := SQLERRM;
    ROLLBACK;
END;
/
