2014/11/20 (1.0.0.9)
1.模組樹狀清單排序條件應為C.FUN_TYPE_IDX, PROGRAM, B.FUN_TYPE_IDX, B.FUN_IDX, FUNCTION, A.AUTHORITYS
===========================================================================
1.0.0.8
2013/09/13 by Owen
1.fMain 修改第一次載入時模組顯示為英文的bug
2.fMain 修改左邊DataGridView切換角色時，右邊模組無連動切換bug
3.fModule 修正將整個模組托至已右邊時，直接按儲存，會跳exception之問題。
4.模組功能樹狀清單，依sys_program_name.FUN_TYPE_IDX, 
  sys_program_fun_name.FUN_TYPE_IDX,sys_program_fun_name.FUN_IDX 排序。
===========================================================================
1.0.0.7
2012/06/27
1.增加顯示已附與之模組列示
2.GetSysBaseData改為SYS_BASE_PARAM
===========================================================================
1.0.0.6
1.修改當Append後,出現是否新增下一筆訊息時,主畫面的Data直接更新
2.修改當新拉一個Program後,按下Save會出現錯誤
===========================================================================
1.0.0.5
2011/06/20
1.Module SQL先讀取SYS_BASE_PARAM若未有則使用原本的SQL(C#使用原本的SQL)
Delphi-請參考Delphi.SQL
===========================================================================
1.0.0.4
2011/06/13
1.在設定權限畫面中,加入"可選權限"及"已選權限"標題
===========================================================================
1.0.0.3
2010/04/16
1.修正權限設定(可由function中修改):
  Allow To Change:只可Update
  Full Control:可Insert,Update,Delete,Enabled,Disabled
2.修改FormLoad時去Focus(SetSelectRow),造成資料大時速度很慢的問題

需新增function:SAJET.SJ_PRIVILEGE_DEFINE
===========================================================================
1.0.0.2
=========
1.0.0.1
2009/12/21
1.Program和Function會根據登入語言顯示中英文
2.加入每個Function的說明

需新增欄位及更新View
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
