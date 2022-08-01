  CREATE TABLE "SAJET"."SYS_EMP_PROCESS_PRIVILEGE" 
   (	"EMP_ID" NUMBER, 
	"PROCESS_ID" NUMBER, 
	"UPDATE_USERID" NUMBER, 
	"UPDATE_TIME" DATE DEFAULT SYSDATE
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "SYSBS" ;

   COMMENT ON COLUMN "SAJET"."SYS_EMP_PROCESS_PRIVILEGE"."EMP_ID" IS 'SYS_EMP.EMP_ID';
 
   COMMENT ON COLUMN "SAJET"."SYS_EMP_PROCESS_PRIVILEGE"."PROCESS_ID" IS 'SYS_PROCESS.PROCESS_ID';