CREATE OR REPLACE PROCEDURE SAJET.SJ_RC_QC_TRANSFER_ITEMTYPE(TLOTNO       IN VARCHAR2,
                                                       TRC_NO       IN VARCHAR2,
                                                       TPROCESSID   IN NUMBER,
                                                       TLOTQTY      IN NUMBER,
                                                       TITEMTYPEID  IN VARCHAR2,
                                                       TSAMPLEID    IN VARCHAR2,
                                                       TSAMPLESIZE  IN NUMBER,
                                                       TSAMPLELEVEL IN NUMBER,
                                                       TPASSQTY     IN NUMBER,
                                                       TFAILQTY     IN NUMBER,
                                                       TQCRESULT    IN VARCHAR2,
                                                       TEMPID       IN NUMBER,
                                                       TRES         OUT VARCHAR2) IS
  V_COUNT NUMBER;
  V_DATE  DATE;
BEGIN
  TRES   := 'OK';
  V_DATE := SYSDATE;

  SELECT COUNT(*)
    INTO V_COUNT
    FROM SAJET.G_QC_LOT_TEST_TYPE
   WHERE QC_LOTNO = TLOTNO
     AND ITEM_TYPE_ID = TITEMTYPEID;
  IF V_COUNT > 0 THEN
    UPDATE SAJET.G_QC_LOT_TEST_TYPE
       SET SAMPLING_PLAN_ID = TSAMPLEID,
           SAMPLING_LEVEL   = TSAMPLELEVEL,
           SAMPLING_SIZE    = TSAMPLESIZE,
           PASS_QTY         = TPASSQTY,
           FAIL_QTY         = TFAILQTY,
           QC_RESULT        = TQCRESULT,
           END_TIME         = V_DATE,
           INSP_EMPID       = TEMPID
     WHERE QC_LOTNO = TLOTNO
       AND ITEM_TYPE_ID = TITEMTYPEID;
  ELSE
    INSERT INTO SAJET.G_QC_LOT_TEST_TYPE
      (QC_LOTNO,
       ITEM_TYPE_ID,
       START_TIME,
       END_TIME,
       SAMPLING_SIZE,
       SAMPLING_PLAN_ID,
       SAMPLING_LEVEL,
       INSP_EMPID,
       PASS_QTY,
       FAIL_QTY,
       QC_RESULT,
       RC_NO)
    VALUES
      (TLOTNO,
       TITEMTYPEID,
       V_DATE,
       V_DATE,
       TSAMPLESIZE,
       TSAMPLEID,
       TSAMPLELEVEL,
       TEMPID,
       TPASSQTY,
       TFAILQTY,
       TQCRESULT,
       TRC_NO);
  END IF;

  SELECT COUNT(*)
    INTO V_COUNT
    FROM SAJET.G_QC_LOT L
   WHERE L.QC_LOTNO = TLOTNO;
  --记录检验批信息
  IF V_COUNT = 0 THEN
  /*
    INSERT INTO SAJET.G_QC_LOT
      (QC_LOTNO,
       START_TIME,
       LOT_SIZE,
       SAMPLING_SIZE,
       PROCESS_ID,
       INSP_EMPID)
    VALUES
      (TLOTNO, SYSDATE, TLOTQTY, TSAMPLESIZE, TPROCESSID, TEMPID);
      */
    INSERT INTO SAJET.G_QC_LOT
      (QC_LOTNO,
       START_TIME,
       LOT_SIZE,
       SAMPLING_SIZE,
       PROCESS_ID,
       INSP_EMPID,
       WORK_ORDER,
       PART_ID,
       ROUTE_ID,
       PDLINE_ID,
       STAGE_ID)
    SELECT
       TLOTNO, SYSDATE, TLOTQTY, TSAMPLESIZE, TPROCESSID, TEMPID,       
       WORK_ORDER,
       PART_ID,
       ROUTE_ID,
       PDLINE_ID,
       STAGE_ID
    FROM SAJET.G_RC_STATUS
    WHERE RC_NO = TRC_NO
    AND ROWNUM = 1;
  END IF;

EXCEPTION
  WHEN OTHERS THEN
    TRES := 'SJ_RC_QC_TRANSFER_ITEMTYPE ERROR' || CHR(10) || CHR(13) ||
            SQLERRM;
END;
/