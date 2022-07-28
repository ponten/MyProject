-- 因為 DB 裡 SJ_PRIVILEGE_DEFINE 檢查只有 2 會回應 'Y'，所以就只新增 2 的權限。
Insert into SAJET.SYS_PROGRAM_FUN_AUTHORITY (PROGRAM,FUNCTION,AUTH_SEQ,AUTHORITYS) values ('Data Center','QC_AQL',2,'Full Control');
