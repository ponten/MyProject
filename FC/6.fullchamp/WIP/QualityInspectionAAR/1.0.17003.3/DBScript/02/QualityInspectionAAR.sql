--------------------------------------------------------
--  已建立檔案 - 星期四-二月-25-2021   
--------------------------------------------------------
REM INSERTING into SAJET.SYS_PROGRAM_FUN_NAME
SET DEFINE OFF;
Insert into SAJET.SYS_PROGRAM_FUN_NAME (PROGRAM,FUNCTION,DLL_FILENAME,FUN_PARAM,FUN_TW,SHOW_FLAG,FUN_TYPE,FUN_TYPE_TW,ENABLED,WEB_FLAG,FORM_NAME,FUN_ENG,FUN_TYPE_ENG,FUN_DESC_ENG,FUN_DESC_TW,FUN_IDX,FUN_TYPE_IDX,FUN_DESC_CN,FUN_TYPE_CN,FUN_CN,FUN_TYPE_TH,FUN_TH,FUN_DESC_TH) values ('WIP','QualityInspectionAAR','QualityInspectionAAR.dll',null,'補登品保資料','0','Execution','執行','Y','N','fMain','Quality Inspection After Action Report','Execution','Quality Inspection After Action Report','補登品保資料',12,1,null,null,null,null,null,null);

Insert into SAJET.SYS_PROGRAM_FUN_AUTHORITY (PROGRAM,FUNCTION,AUTH_SEQ,AUTHORITYS) values ('WIP','QualityInspectionAAR',0,'Read Only');
Insert into SAJET.SYS_PROGRAM_FUN_AUTHORITY (PROGRAM,FUNCTION,AUTH_SEQ,AUTHORITYS) values ('WIP','QualityInspectionAAR',1,'Allow To Change');
Insert into SAJET.SYS_PROGRAM_FUN_AUTHORITY (PROGRAM,FUNCTION,AUTH_SEQ,AUTHORITYS) values ('WIP','QualityInspectionAAR',2,'Full Control');
Insert into SAJET.SYS_PROGRAM_FUN_AUTHORITY (PROGRAM,FUNCTION,AUTH_SEQ,AUTHORITYS) values ('WIP','QualityInspectionAAR',3,'Allow To Execute');
