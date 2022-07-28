prompt Importing table sajet.sys_module_param...
set feedback off
set define off
insert into sajet.sys_module_param (MODULE_NAME, FUNCTION_NAME, PARAME_NAME, PARAME_ITEM, PARAME_VALUE, UPDATE_USERID, UPDATE_TIME, ENABLED)
values ('PACKING', 'PACK CARTON', null, 'PACK BASE', 'PARTNO', 10000001, to_date('02-11-2016 16:45:34', 'dd-mm-yyyy hh24:mi:ss'), 'Y');

insert into sajet.sys_module_param (MODULE_NAME, FUNCTION_NAME, PARAME_NAME, PARAME_ITEM, PARAME_VALUE, UPDATE_USERID, UPDATE_TIME, ENABLED)
values ('PACKING', 'PACK CARTON', null, 'PACK ACTION', 'BOX->CARTON', 10000001, to_date('02-11-2016 16:45:34', 'dd-mm-yyyy hh24:mi:ss'), 'Y');

insert into sajet.sys_module_param (MODULE_NAME, FUNCTION_NAME, PARAME_NAME, PARAME_ITEM, PARAME_VALUE, UPDATE_USERID, UPDATE_TIME, ENABLED)
values ('PACKING', 'PACK CARTON', null, 'PRINT LABEL', 'Y', 10000001, to_date('02-11-2016 16:45:34', 'dd-mm-yyyy hh24:mi:ss'), 'Y');

insert into sajet.sys_module_param (MODULE_NAME, FUNCTION_NAME, PARAME_NAME, PARAME_ITEM, PARAME_VALUE, UPDATE_USERID, UPDATE_TIME, ENABLED)
values ('PACKING', 'PACK CARTON', null, 'WEIGHT CARTON', 'N', 10000001, to_date('02-11-2016 16:45:34', 'dd-mm-yyyy hh24:mi:ss'), 'Y');

insert into sajet.sys_module_param (MODULE_NAME, FUNCTION_NAME, PARAME_NAME, PARAME_ITEM, PARAME_VALUE, UPDATE_USERID, UPDATE_TIME, ENABLED)
values ('PACKING', 'PACK CARTON', '2', 'CAPACITY', '500', 10000001, to_date('02-11-2016 16:45:34', 'dd-mm-yyyy hh24:mi:ss'), 'Y');
insert into sajet.sys_module_param (MODULE_NAME, FUNCTION_NAME, PARAME_NAME, PARAME_ITEM, PARAME_VALUE, UPDATE_USERID, UPDATE_TIME, ENABLED)
values ('PACKING', 'PACK CARTON', '4', 'CAPACITY', '300', 10000001, to_date('02-11-2016 16:45:34', 'dd-mm-yyyy hh24:mi:ss'), 'Y');
insert into sajet.sys_module_param (MODULE_NAME, FUNCTION_NAME, PARAME_NAME, PARAME_ITEM, PARAME_VALUE, UPDATE_USERID, UPDATE_TIME, ENABLED)
values ('PACKING', 'PACK CARTON', '6', 'CAPACITY', '100', 10000001, to_date('02-11-2016 16:45:34', 'dd-mm-yyyy hh24:mi:ss'), 'Y');

insert into sajet.sys_module_param (MODULE_NAME, FUNCTION_NAME, PARAME_NAME, PARAME_ITEM, PARAME_VALUE, UPDATE_USERID, UPDATE_TIME, ENABLED)
values ('PACKING', 'PACK CARTON', null, 'PATH', 'D:\CartonList', 10000001, to_date('02-11-2016 16:45:34', 'dd-mm-yyyy hh24:mi:ss'), 'Y');

insert into sajet.sys_module_param (MODULE_NAME, FUNCTION_NAME, PARAME_NAME, PARAME_ITEM, PARAME_VALUE, UPDATE_USERID, UPDATE_TIME, ENABLED)
values ('PACKING', 'PACK CARTON', null, 'PROCESS', '100037', 10000001, to_date('02-11-2016 16:45:34', 'dd-mm-yyyy hh24:mi:ss'), 'Y');

prompt Done.
