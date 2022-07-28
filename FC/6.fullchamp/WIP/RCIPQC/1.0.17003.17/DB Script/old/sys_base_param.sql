-- 新增 sys base param

Insert into sys_base_param select PROGRAM ,
'IPQC' ,
PARAM_VALUE ,
PARAM_TYPE ,
DEFAULT_VALUE ,
PARAM_DESC  from sajet.sys_base_param where program = 'WIP' and param_name = 'RC Output';

Insert into sys_base_param select PROGRAM ,
'Repair' ,
PARAM_VALUE ,
PARAM_TYPE ,
DEFAULT_VALUE ,
PARAM_DESC  from sajet.sys_base_param where program = 'WIP' and param_name = 'RC Output';

Insert into sys_base_param select PROGRAM ,
'ProcessInputAAR' ,
PARAM_VALUE ,
PARAM_TYPE ,
DEFAULT_VALUE ,
PARAM_DESC  from sajet.sys_base_param where program = 'WIP' and param_name = 'RC Output';