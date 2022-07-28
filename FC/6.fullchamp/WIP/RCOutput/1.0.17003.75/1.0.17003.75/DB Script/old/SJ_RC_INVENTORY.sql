create or replace PROCEDURE       SJ_RC_INVENTORY (TREV   IN     VARCHAR2,
                                                   TRES      OUT VARCHAR2)
IS
   V_WO         SAJET.G_RC_STATUS.WORK_ORDER%TYPE;
   V_QTY        SAJET.G_RC_STATUS.CURRENT_QTY%TYPE;
   V_SOURCE     SAJET.G_RC_SPLIT.SOURCE_RC_NO%TYPE;
   V_TARGET     SAJET.G_WO_BASE.TARGET_QTY%TYPE;
   V_TOTAL      NUMBER;
   V_NUM        NUMBER;
   V_PART       NUMBER;
   V_SPLITRC    SAJET.G_RC_STATUS.RC_NO%TYPE;
   V_SPLITQTY   NUMBER;
   V_NOW        DATE;
   V_RC         SAJET.G_RC_STATUS.RC_NO%TYPE;
   
   V_PROCESSID NUMBER;
   V_TRAVELID NUMBER;
   V_UPDATEUSERID NUMBER;
   V_UPDATETIME DATE;
   V_CNT NUMBER;
/*
V1 增加拆批入庫功能
V2 1.拆批數量調整,新的批號數量為原批號除以單位數量的餘數
   2.新的批號尾數大於9用英文字母表示
   
V5-20171025 修改判定工單結單
   1.工單結單時,同時填入WO_CLOSE_TIME
   2.填入工單的OUTPUT_QTY
V6-20180212 取消填入MESUSER Table  
V7-20180529 最後一站不拆批  
*/
BEGIN
   TRES := 'OK';
   V_NOW := SYSDATE;
   V_RC := TREV;
   V_SPLITRC := TREV;

   SELECT WORK_ORDER, CURRENT_QTY, PART_ID,PROCESS_ID,TRAVEL_ID,UPDATE_USERID,UPDATE_TIME
     INTO V_WO, V_QTY, V_PART,V_PROCESSID,V_TRAVELID,V_UPDATEUSERID,V_UPDATETIME
     FROM SAJET.G_RC_STATUS
    WHERE RC_NO = TREV AND ROWNUM = 1;

   V_SPLITQTY := 0;

   BEGIN
      SELECT NVL (SOURCE_RC_NO, V_RC)
        INTO V_SOURCE
        FROM SAJET.G_RC_SPLIT
       WHERE RC_NO = TREV AND ROWNUM = 1;

      IF V_RC IS NULL OR V_RC = ''
      THEN
         V_RC := TREV;
      END IF;
   EXCEPTION
      WHEN OTHERS
      THEN
         V_SOURCE := TREV;
   END;

   SELECT NVL (TARGET_QTY, 0)
     INTO V_TARGET
     FROM SAJET.G_WO_BASE
    WHERE WORK_ORDER = V_WO AND ROWNUM = 1;

   -- 累計報廢,產出,入庫總數
   --SELECT SUM (CURRENT_QTY) V5
   SELECT SUM (INITIAL_QTY)
     INTO V_TOTAL
     FROM SAJET.G_RC_STATUS
    WHERE WORK_ORDER = V_WO AND CURRENT_STATUS IN (8, 9, 20);

   -- V1 找拆批單位數量
   V_NUM := 0;

   BEGIN
      SELECT NVL (OPTION3, 0)
        INTO V_NUM
        FROM SAJET.SYS_PART
       WHERE PART_ID = V_PART AND ROWNUM = 1;
   EXCEPTION
      WHEN OTHERS
      THEN
         V_NUM := 0;
   END;


   -- 當流程卡目前數量大於拆批單位數量就拆成兩批
   -- V2 新的批號數量 = 原批號數量 % 單位數量
   --    拆開原批號數量 =   原批號數量- 新的批號數量
   
   IF 1 = 2  --V7 : 不進入自動拆批的動作
   --IF V_QTY > V_NUM
   THEN
      --V_SPLITQTY := V_QTY - V_NUM;
      --V_QTY := V_NUM;
      V_SPLITQTY := MOD (V_QTY, V_NUM);
      V_QTY := V_QTY - V_SPLITQTY;

      -- 流程卡子批號碼建立
      BEGIN
         /*
             SELECT SUBSTR(MAX(RC_NO), 0, LENGTH(MAX(RC_NO))-1) || (SUBSTR(MAX(RC_NO),LENGTH(MAX(RC_NO)), 1) + 1)
             INTO V_SPLITRC
             FROM SAJET.G_RC_STATUS
             WHERE RC_NO LIKE SUBSTR(TREV, 0,LENGTH(TREV)-1)  || '%'
             AND ROWNUM = 1;
             */
         SELECT    SUBSTR (TREV, 0, LENGTH (TREV) - 1)
                || (SELECT CASE
                              WHEN LENGTH (A.SEQ) > 1
                              THEN
                                 'A'
                              ELSE
                                 TO_CHAR (A.SEQ)
                           END
                      FROM (SELECT   SUBSTR (TREV,
                                             LENGTH (TREV),
                                             1) + 1
                                   
                                      SEQ
                              FROM SAJET.G_RC_STATUS
                             WHERE     RC_NO LIKE
                                             SUBSTR (TREV,
                                                     0,
                                                     LENGTH (TREV) - 1)
                                          || '%'
                                   AND ROWNUM = 1) A)
           INTO V_SPLITRC
           FROM SAJET.G_RC_STATUS
          WHERE     RC_NO LIKE SUBSTR (TREV, 0, LENGTH (TREV) - 1) || '%'
                AND ROWNUM = 1;

         IF V_SPLITRC IS NULL OR V_SPLITRC = ''
         THEN
            V_SPLITRC := TREV || '1';
         END IF;
      EXCEPTION
         WHEN OTHERS
         THEN
            V_SPLITRC := TREV || '1';
      END;

      /*INSERT INTO SAJET.G_RC_SPLIT (RC_NO,RC_QTY,SOURCE_RC_NO,SOURCE_RC_QTY,UPDATE_TIME)
      VALUES (V_SPLITRC,V_SPLITQTY,TREV,V_QTY,V_NOW);*/
      
      /*INSERT INTO SAJET.G_RC_STATUS (WORK_ORDER,RC_NO,PART_ID,ROUTE_ID,FACTORY_ID,PDLINE_ID,STAGE_ID,PROCESS_ID,
                                                            SHEET_NAME,CURRENT_STATUS,CURRENT_QTY,UPDATE_TIME,CREATE_TIME,WIP_OUT_GOOD_QTY,TRAVEL_ID)
                                                SELECT WORK_ORDER,V_SPLITRC,PART_ID,ROUTE_ID,FACTORY_ID,PDLINE_ID,STAGE_ID,PROCESS_ID,
                                                            SHEET_NAME,CURRENT_STATUS,V_SPLITQTY,V_NOW,V_NOW,WIP_OUT_GOOD_QTY,TRAVEL_ID
                                                   FROM SAJET.G_RC_STATUS
                                                 WHERE RC_NO = TREV AND ROWNUM = 1;*/

      IF (V_SPLITQTY <> 0) THEN
        INSERT INTO SAJET.G_RC_SPLIT (RC_NO,RC_QTY,SOURCE_RC_NO,SOURCE_RC_QTY,TRAVEL_ID,PROCESS_ID,UPDATE_USERID,UPDATE_TIME)
        VALUES (TREV,V_QTY,TREV,V_QTY + V_SPLITQTY,V_TRAVELID,V_PROCESSID,V_UPDATEUSERID,V_UPDATETIME);

        INSERT INTO SAJET.G_RC_SPLIT (RC_NO,RC_QTY,SOURCE_RC_NO,SOURCE_RC_QTY,TRAVEL_ID,PROCESS_ID,UPDATE_USERID,UPDATE_TIME)
        VALUES (V_SPLITRC,V_SPLITQTY,TREV,V_QTY + V_SPLITQTY,V_TRAVELID,V_PROCESSID,V_UPDATEUSERID,V_UPDATETIME);

        INSERT INTO SAJET.G_RC_STATUS (WORK_ORDER,RC_NO,PART_ID,VERSION,ROUTE_ID,FACTORY_ID,PDLINE_ID,STAGE_ID,NODE_ID,PROCESS_ID,
                                                              SHEET_NAME,TERMINAL_ID,TRAVEL_ID,NEXT_NODE,NEXT_PROCESS,CURRENT_STATUS,CURRENT_QTY,
                                                              HAVE_SN,PRIORITY_LEVEL,UPDATE_USERID,UPDATE_TIME,CREATE_TIME,
                                                              BATCH_ID,EMP_ID,WIP_PROCESS,QC_RESULT,QC_NO,BONUS_QTY,WORKTIME,INITIAL_QTY,RELEASE)
                                                  SELECT WORK_ORDER,V_SPLITRC,PART_ID,VERSION,ROUTE_ID,FACTORY_ID,PDLINE_ID,STAGE_ID,NODE_ID,PROCESS_ID,
                                                              SHEET_NAME,TERMINAL_ID,TRAVEL_ID,NEXT_NODE,NEXT_PROCESS,CURRENT_STATUS,V_SPLITQTY,
                                                              HAVE_SN,PRIORITY_LEVEL,UPDATE_USERID,UPDATE_TIME,CREATE_TIME,
                                                              BATCH_ID,EMP_ID,WIP_PROCESS,QC_RESULT,QC_NO,BONUS_QTY,WORKTIME,INITIAL_QTY,RELEASE
                                                     FROM SAJET.G_RC_STATUS
                                                   WHERE RC_NO = TREV AND ROWNUM = 1;
            --V5
      UPDATE SAJET.G_RC_STATUS
         SET CURRENT_QTY = V_QTY
       WHERE RC_NO = TREV AND ROWNUM = 1;

      END IF;

      -- V2
      /*
      UPDATE SAJET.G_RC_STATUS
         SET CURRENT_QTY = V_QTY
       WHERE RC_NO = TREV AND ROWNUM = 1;
   
   INSERT INTO SAJET.G_RC_TRAVEL
   SELECT * FROM SAJET.G_RC_STATUS
   WHERE RC_NO = V_SPLITRC
   AND ROWNUM = 1;
   */
   END IF;
   
   
   
   --V5

   SELECT COUNT(*) INTO V_CNT
   FROM SAJET.G_RC_STATUS
   WHERE WORK_ORDER = V_WO AND CURRENT_STATUS IN (0,1,2,11);
   
   --IF V_TOTAL < V_TARGET
   IF V_CNT > 0
   THEN
     V_CNT:=V_CNT;
   /* V6 Mark
      INSERT INTO MESUSER.G_RC_INVENTORY (JOB_NUMBER,
                                          RC_NO,
                                          CURRENT_QTY,
                                          WO_CLOSE,
                                          SOURCE_RC_NO,
                                          UPDATE_TIME)
           VALUES (V_WO,
                   TREV,
                   V_QTY,
                   'N',
                   V_SOURCE,
                   V_UPDATETIME);

      IF V_SPLITQTY <> 0
      THEN
         INSERT INTO MESUSER.G_RC_INVENTORY (JOB_NUMBER,
                                             RC_NO,
                                             CURRENT_QTY,
                                             WO_CLOSE,
                                             SOURCE_RC_NO,
                                             UPDATE_TIME)
              VALUES (V_WO,
                      V_SPLITRC,
                      V_SPLITQTY,
                      'N',
                      TREV,
                      V_UPDATETIME);
      END IF;
   */   
   ELSE
   /* V6 Mark 
      INSERT INTO MESUSER.G_RC_INVENTORY (JOB_NUMBER,
                                          RC_NO,
                                          CURRENT_QTY,
                                          WO_CLOSE,
                                          SOURCE_RC_NO,
                                          UPDATE_TIME)
           VALUES (V_WO,
                   TREV,
                   V_QTY,
                   'Y',
                   V_SOURCE,
                   V_UPDATETIME);

      IF V_SPLITQTY <> 0
      THEN
         INSERT INTO MESUSER.G_RC_INVENTORY (JOB_NUMBER,
                                             RC_NO,
                                             CURRENT_QTY,
                                             WO_CLOSE,
                                             SOURCE_RC_NO,
                                             UPDATE_TIME)
              VALUES (V_WO,
                      V_SPLITRC,
                      V_SPLITQTY,
                      'Y',
                      TREV,
                      V_UPDATETIME);
      END IF;

      INSERT INTO MESUSER.G_WO_STATUS (JOB_NUMBER, JOB_STATUS)
           VALUES (V_WO, 6);
   */
      UPDATE SAJET.G_WO_BASE
         SET WO_STATUS = 6,WO_CLOSE_DATE=V_NOW
       WHERE WORK_ORDER = V_WO ;
   END IF;
   
   
   --20171025:填入工單產出數
   UPDATE SAJET.G_WO_BASE
         SET OUTPUT_QTY = OUTPUT_QTY + NVL(V_SPLITQTY, 0) + NVL(V_QTY, 0)
       WHERE WORK_ORDER = V_WO ;
EXCEPTION
   WHEN NO_DATA_FOUND
   THEN
      TRES := 'R/C Error';
   WHEN OTHERS
   THEN
      TRES := 'SJ_RC_INVENTORY Error,' || SQLERRM;
END;