--------------------------------------------------------
--  已建立檔案 - 星期五-十一月-06-2020   
--------------------------------------------------------
REM INSERTING into SAJET.SYS_PROGRAM_FUN_NAME
SET DEFINE OFF;
DELETE FROM SAJET.SYS_PROGRAM_FUN_NAME WHERE FUN_TW = '銷售訂單維護';
Insert into SAJET.SYS_PROGRAM_FUN_NAME (PROGRAM,FUNCTION,DLL_FILENAME,FUN_PARAM,FUN_TW,SHOW_FLAG,FUN_TYPE,FUN_TYPE_TW,ENABLED,WEB_FLAG,FORM_NAME,FUN_ENG,FUN_TYPE_ENG,FUN_DESC_ENG,FUN_DESC_TW,FUN_IDX,FUN_TYPE_IDX,FUN_DESC_CN,FUN_TYPE_CN,FUN_CN,FUN_TYPE_TH,FUN_TH,FUN_DESC_TH) values ('Sales order','Sales order maintain','CSaleOrderInput.dll',null,'銷售訂單維護','0','Sales order maintain','銷售訂單','Y','N','fMain',null,null,null,null,1,1,null,null,null,null,'บำรุงรักษาใบสั่งขาย',null);
