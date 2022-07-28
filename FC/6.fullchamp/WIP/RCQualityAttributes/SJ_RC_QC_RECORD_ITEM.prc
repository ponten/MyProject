CREATE OR REPLACE PROCEDURE SJ_RC_QC_RECORD_ITEM(I_ITEM_TYPE_ID IN VARCHAR2,
                                                 I_ITEM_ID      IN VARCHAR2,
                                                 I_SN           IN VARCHAR2,
                                                 I_INFO         IN VARCHAR2,
                                                 I_RESULT       IN VARCHAR2,
                                                 I_QCLOT        IN VARCHAR2,
                                                 O_RES          OUT VARCHAR2) IS
  V_COUNT NUMBER;
  V_DATE  DATE;
BEGIN
  O_RES  := 'OK';
  V_DATE := SYSDATE;
  SELECT COUNT(*)
    INTO V_COUNT
    FROM SAJET.G_QC_LOT_TEST_ITEM
   WHERE ITEM_TYPE_ID = I_ITEM_TYPE_ID
     AND ITEM_ID = I_ITEM_ID
     AND SERIAL_NUMBER = I_SN
     AND QC_LOTNO = I_QCLOT;
  IF V_COUNT > 0 THEN
    UPDATE SAJET.G_QC_LOT_TEST_ITEM
       SET INFORMATION = I_INFO, QCRESULT = I_RESULT
     WHERE ITEM_TYPE_ID = I_ITEM_TYPE_ID
       AND ITEM_ID = I_ITEM_ID
       AND SERIAL_NUMBER = I_SN
       AND QC_LOTNO = I_QCLOT;
  ELSE
    INSERT INTO SAJET.G_QC_LOT_TEST_ITEM
      (QC_LOTNO,
       ITEM_TYPE_ID,
       ITEM_ID,
       SERIAL_NUMBER,
       INFORMATION,
       QCRESULT,
       UPDATE_TIME)
    VALUES
      (I_QCLOT,
       I_ITEM_TYPE_ID,
       I_ITEM_ID,
       I_SN,
       I_INFO,
       I_RESULT,
       V_DATE);
  END IF;

EXCEPTION
  WHEN OTHERS THEN
    O_RES := 'SJ_QC_RECORD_RC_ITEM ERROR' || CHR(10) || CHR(13) || SQLERRM;
END;

 
/
