--------------------------------------------------------
--  已建立檔案 - 星期二-十二月-29-2020   
--------------------------------------------------------
REM INSERTING into SAJET.SYS_BASE
SET DEFINE OFF;
Insert into SAJET.SYS_BASE (PARAM_NAME,PARAM_VALUE,PROGRAM) values ('Bartender Print Command','"C:\Program Files\Seagull\BarTender Suite\Bartend.exe" /F=@PATH1 /D=@PATH2 /P /X /C=@QTY','RC Output');
