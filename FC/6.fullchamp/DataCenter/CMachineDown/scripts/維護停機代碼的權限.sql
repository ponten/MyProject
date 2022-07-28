Insert into SAJET.SYS_PROGRAM_FUN_NAME (PROGRAM,FUNCTION,DLL_FILENAME,FUN_PARAM,FUN_TW,SHOW_FLAG,FUN_TYPE,FUN_TYPE_TW,ENABLED,WEB_FLAG,FORM_NAME,FUN_ENG,FUN_TYPE_ENG,FUN_DESC_ENG,FUN_DESC_TW,FUN_IDX,FUN_TYPE_IDX,FUN_DESC_CN,FUN_TYPE_CN,FUN_CN,FUN_TYPE_TH,FUN_TH,FUN_DESC_TH)
values ('Data Center','Machine Down Reason','CMachineDown.dll',null,'機台停機原因','0','Machine','設備管理','Y','N','fMain','Machine Down Reason','Machine','Machine Down Reason','機台停機原因',3,5,null,null,null,'','',null);

Insert into SAJET.SYS_PROGRAM_FUN_AUTHORITY (PROGRAM,FUNCTION,AUTH_SEQ,AUTHORITYS) values ('Data Center','Machine Down Reason',0,'Read Only');
Insert into SAJET.SYS_PROGRAM_FUN_AUTHORITY (PROGRAM,FUNCTION,AUTH_SEQ,AUTHORITYS) values ('Data Center','Machine Down Reason',1,'Allow To Change');
Insert into SAJET.SYS_PROGRAM_FUN_AUTHORITY (PROGRAM,FUNCTION,AUTH_SEQ,AUTHORITYS) values ('Data Center','Machine Down Reason',2,'Full Control');