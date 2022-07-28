CREATE OR REPLACE PROCEDURE SJ_RC_QC_UPDATE_SAMPLETYPE(TLOTNO       IN VARCHAR2,
                                                       TITEMTYPEID  IN VARCHAR2,
                                                       TSAMPLELEVEL IN NUMBER,
                                                       TSAMPLEID    IN NUMBER,
                                                       TSAMPLESIZE  IN NUMBER,
                                                       TEMPID       IN NUMBER,
                                                       TPROCESSID   IN VARCHAR2,
                                                       TPARTID      IN VARCHAR2,
                                                       TRES         OUT VARCHAR2) IS
  V_COUNT NUMBER;
  V_DATE  DATE;
  C_RECID SAJET.SYS_PART_QC_PROCESS_RULE.RECID%TYPE;
BEGIN
  TRES   := 'OK';
  V_DATE := SYSDATE;
  SELECT RECID
    INTO C_RECID
    FROM SAJET.SYS_PART_QC_PROCESS_RULE
   WHERE PROCESS_ID = TPROCESSID
     AND PART_ID = TPARTID;

  UPDATE SAJET.SYS_PART_QC_TESTTYPE B
     SET B.SAMPLING_ID = TSAMPLEID
   WHERE B.RECID = C_RECID
     AND B.ITEM_TYPE_ID = TITEMTYPEID;

  SELECT COUNT(*)
    INTO V_COUNT
    FROM SAJET.G_QC_LOT_TEST_TYPE
   WHERE QC_LOTNO = TLOTNO
     AND ITEM_TYPE_ID = TITEMTYPEID;
  IF V_COUNT > 0 THEN
    UPDATE SAJET.G_QC_LOT_TEST_TYPE
       SET SAMPLING_PLAN_ID = TSAMPLEID,
           SAMPLING_LEVEL   = TSAMPLELEVEL,
           SAMPLING_SIZE    = TSAMPLESIZE
     WHERE QC_LOTNO = TLOTNO
       AND ITEM_TYPE_ID = TITEMTYPEID;
  ELSE
    INSERT INTO SAJET.G_QC_LOT_TEST_TYPE
      (QC_LOTNO,
       NG_CNT,
       ITEM_TYPE_ID,
       START_TIME,
       SAMPLING_SIZE,
       SAMPLING_PLAN_ID,
       SAMPLING_LEVEL,
       INSP_EMPID)
    VALUES
      (TLOTNO,
       '1',
       TITEMTYPEID,
       V_DATE,
       TSAMPLESIZE,
       TSAMPLEID,
       TSAMPLELEVEL,
       TEMPID);
  END IF;

EXCEPTION
  WHEN OTHERS THEN
    TRES := 'SJ_RC_QC_UPDATE_SAMPLETYPE ERROR' || CHR(10) || CHR(13) ||
            SQLERRM;
END;
/
