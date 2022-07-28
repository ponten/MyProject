CREATE OR REPLACE PROCEDURE SJ_RC_QC_SET_RESULT(I_QCLOT IN VARCHAR2,
                                                I_GOOD  NUMBER,
                                                I_FAIL  NUMBER,
                                                O_RES   OUT VARCHAR2) IS
  TCOUNT NUMBER;
BEGIN
  O_RES  := 'OK';
  TCOUNT := 0;
  SELECT COUNT(*)
    INTO TCOUNT
    FROM SAJET.G_QC_LOT_TEST_TYPE T
   WHERE T.QC_LOTNO = I_QCLOT;
  IF TCOUNT > 0 THEN
    SELECT COUNT(*)
      INTO TCOUNT
      FROM SAJET.G_QC_LOT_TEST_TYPE
     WHERE QC_LOTNO = I_QCLOT
       AND QC_RESULT = '1';
    IF TCOUNT > 0 THEN
      UPDATE SAJET.G_QC_LOT
         SET PASS_QTY  = I_GOOD,
             FAIL_QTY  = I_FAIL,
             QC_RESULT = '1',
             END_TIME  = SYSDATE
       WHERE QC_LOTNO = I_QCLOT;
    ELSE
      UPDATE SAJET.G_QC_LOT
         SET PASS_QTY  = I_GOOD,
             FAIL_QTY  = I_FAIL,
             QC_RESULT = '0',
             END_TIME  = SYSDATE
       WHERE QC_LOTNO = I_QCLOT;
    END IF;
  END IF;
EXCEPTION
  WHEN OTHERS THEN
    O_RES := 'SJ_RC_QC_SET_RESULT ERROR' || CHR(10) || CHR(13) || SQLERRM;
    ROLLBACK;
END;
/
