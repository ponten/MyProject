--------------------------------------------------------
--  已建立檔案 - 星期四-九月-17-2020   
--------------------------------------------------------
REM INSERTING into SAJET.SYS_PROGRAM_FUN_NAME
SET DEFINE OFF;
Insert into SAJET.SYS_PROGRAM_FUN_NAME (PROGRAM,FUNCTION,DLL_FILENAME,FUN_PARAM,FUN_TW,SHOW_FLAG,FUN_TYPE,FUN_TYPE_TW,ENABLED,WEB_FLAG,FORM_NAME,FUN_ENG,FUN_TYPE_ENG,FUN_DESC_ENG,FUN_DESC_TW,FUN_IDX,FUN_TYPE_IDX,FUN_DESC_CN,FUN_TYPE_CN,FUN_CN,FUN_TYPE_TH,FUN_TH,FUN_DESC_TH) values ('WIP','Machine_Change',null,null,'機台切換','0','Execution','執行','Y','N','fMain','Machine_Change','Execution','Machine_Change','機台切換',6,1,null,null,null,null,null,null);

REM INSERTING into SAJET.SYS_PROGRAM_FUN_AUTHORITY

Insert into SAJET.SYS_PROGRAM_FUN_AUTHORITY (PROGRAM,FUNCTION,AUTH_SEQ,AUTHORITYS) values ('WIP','Machine_Change',0,'Read Only');
Insert into SAJET.SYS_PROGRAM_FUN_AUTHORITY (PROGRAM,FUNCTION,AUTH_SEQ,AUTHORITYS) values ('WIP','Machine_Change',2,'Full Control');
