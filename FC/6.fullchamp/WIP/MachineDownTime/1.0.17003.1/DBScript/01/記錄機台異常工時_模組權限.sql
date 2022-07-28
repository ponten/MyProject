--------------------------------------------------------
--  已建立檔案 - 星期五-六月-18-2021   
--------------------------------------------------------
REM INSERTING into SAJET.SYS_PROGRAM_FUN_NAME
SET DEFINE OFF;
Insert into SAJET.SYS_PROGRAM_FUN_NAME (PROGRAM,FUNCTION,DLL_FILENAME,FUN_PARAM,FUN_TW,SHOW_FLAG,FUN_TYPE,FUN_TYPE_TW,ENABLED,WEB_FLAG,FORM_NAME,FUN_ENG,FUN_TYPE_ENG,FUN_DESC_ENG,FUN_DESC_TW,FUN_IDX,FUN_TYPE_IDX,FUN_DESC_CN,FUN_TYPE_CN,FUN_CN,FUN_TYPE_TH,FUN_TH,FUN_DESC_TH) values ('WIP','MachineDownTime','MachineDownTime.dll',null,'記錄機台異常時間','0','Execution','執行','Y','N','fMain','Machine downtime','Execution','Machine downtime','記錄機台異常時間',14,1,null,'执行',null,null,'บันทึกชั่วโมงการทำงานผิดปกติของเครื่อง',null);

Insert into SAJET.SYS_PROGRAM_FUN_AUTHORITY (PROGRAM,FUNCTION,AUTH_SEQ,AUTHORITYS) values ('WIP','MachineDownTime',0,'Read Only');
Insert into SAJET.SYS_PROGRAM_FUN_AUTHORITY (PROGRAM,FUNCTION,AUTH_SEQ,AUTHORITYS) values ('WIP','MachineDownTime',1,'Allow To Change');
Insert into SAJET.SYS_PROGRAM_FUN_AUTHORITY (PROGRAM,FUNCTION,AUTH_SEQ,AUTHORITYS) values ('WIP','MachineDownTime',2,'Full Control');
Insert into SAJET.SYS_PROGRAM_FUN_AUTHORITY (PROGRAM,FUNCTION,AUTH_SEQ,AUTHORITYS) values ('WIP','MachineDownTime',3,'Allow To Execute');
