2014/11/20 (1.0.0.9)
1.�Ҳվ𪬲M��ƧǱ�������C.FUN_TYPE_IDX, PROGRAM, B.FUN_TYPE_IDX, B.FUN_IDX, FUNCTION, A.AUTHORITYS
===========================================================================
1.0.0.8
2013/09/13 by Owen
1.fMain �ק�Ĥ@�����J�ɼҲ���ܬ��^�媺bug
2.fMain �ק索��DataGridView��������ɡA�k��ҲյL�s�ʤ���bug
3.fModule �ץ��N��ӼҲզ��ܤw�k��ɡA�������x�s�A�|��exception�����D�C
4.�Ҳե\��𪬲M��A��sys_program_name.FUN_TYPE_IDX, 
  sys_program_fun_name.FUN_TYPE_IDX,sys_program_fun_name.FUN_IDX �ƧǡC
===========================================================================
1.0.0.7
2012/06/27
1.�W�[��ܤw���P���ҲզC��
2.GetSysBaseData�אּSYS_BASE_PARAM
===========================================================================
1.0.0.6
1.�ק��Append��,�X�{�O�_�s�W�U�@���T����,�D�e����Data������s
2.�ק��s�Ԥ@��Program��,���USave�|�X�{���~
===========================================================================
1.0.0.5
2011/06/20
1.Module SQL��Ū��SYS_BASE_PARAM�Y�����h�ϥέ쥻��SQL(C#�ϥέ쥻��SQL)
Delphi-�аѦ�Delphi.SQL
===========================================================================
1.0.0.4
2011/06/13
1.�b�]�w�v���e����,�[�J"�i���v��"��"�w���v��"���D
===========================================================================
1.0.0.3
2010/04/16
1.�ץ��v���]�w(�i��function���ק�):
  Allow To Change:�u�iUpdate
  Full Control:�iInsert,Update,Delete,Enabled,Disabled
2.�ק�FormLoad�ɥhFocus(SetSelectRow),�y����Ƥj�ɳt�׫ܺC�����D

�ݷs�Wfunction:SAJET.SJ_PRIVILEGE_DEFINE
===========================================================================
1.0.0.2
=========
1.0.0.1
2009/12/21
1.Program�MFunction�|�ھڵn�J�y����ܤ��^��
2.�[�J�C��Function������

�ݷs�W���Χ�sView
alter table sajet.sys_program_fun_name add (fun_desc_eng varchar2(150),fun_desc_tra varchar2(150));


CREATE OR REPLACE VIEW SAJET.SYS_PROGRAM_FUN
(PROGRAM, FUNCTION, AUTH_SEQ, AUTHORITYS, DLL_FILENAME, 
 FUN_IDX, FUN_PARAM, FUN_TRA, SHOW_FLAG, FUN_TYPE, 
 FUN_TYPE_IDX, FUN_TYPE_TRA, WEB_FLAG, ENABLED, FORM_NAME,
 FUN_TYPE_ENG,FUN_ENG,FUN_DESC_ENG,FUN_DESC_TRA)
AS 
select b.program,b.function,c.AUTH_SEQ,c.AUTHORITYS,b.dll_filename
  ,b.fun_idx,b.fun_param,b.fun_tra,b.SHOW_FLAG
  ,b.fun_type,b.fun_type_idx,b.fun_type_tra,b.web_flag,b.enabled, b.form_name
  ,b.fun_type_eng,b.fun_eng,b.fun_desc_eng,b.fun_desc_tra
from sajet.sys_program_fun_name b
    ,sajet.sys_program_fun_authority c
where b.program = c.program(+)
and b.function = c.function(+);
