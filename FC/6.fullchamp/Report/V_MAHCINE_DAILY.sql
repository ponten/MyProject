CREATE OR REPLACE FORCE VIEW "SAJET"."V_MAHCINE_DAILY" ("R_NUM", "MACHINE_CODE", "MACHINE_DESC", "SPEC2", "OPTION2", "RC_NO", "PROCESS_NAME", "SHEET_NAME", "WIP_IN_QTY", "LOAD_QTY", "WIP_OUT_GOOD_QTY", "WIP_OUT_SCRAP_QTY", "EMP_NO", "EMP_NAME", "START_DATE", "START_DATE_TIME", "START_TIME", "END_DATE", "END_DATE_TIME", "END_TIME", "MIN", "SECOND", "STANDARD_WORKTIME", "DIFF_TIME", "WORK_ORDER", "TRAVEL_ID", "REASON") AS 
  SELECT  distinct ROW_NUMBER() OVER(PARTITION BY md.RC_NO, md.start_time, md.end_time ORDER BY md.end_time )R_NUM,
SM.MACHINE_CODE,SM.MACHINE_DESC,SP.SPEC2,SP.OPTION2,md.RC_NO,PRO.PROCESS_NAME,  md.sheet_name ,  to_char(NVL( rc.wip_in_qty, md.wip_in_qty)) wip_in_qty,
 (md.load_qty -(decode( ROW_NUMBER() OVER(PARTITION BY md.RC_NO, md.start_time, md.end_time ORDER BY md.end_time) ,1,(CASE WHEN md.reason_id=0 THEN RC.WIP_OUT_SCRAP_QTY ELSE 0 END) ,0))) load_qty,
decode( ROW_NUMBER() OVER(PARTITION BY md.RC_NO, md.start_time, md.end_time ORDER BY md.end_time) ,1,(CASE WHEN md.reason_id=0 THEN RC.WIP_OUT_GOOD_QTY ELSE 0 END) ,0) WIP_OUT_GOOD_QTY,
decode( ROW_NUMBER() OVER(PARTITION BY md.RC_NO, md.start_time, md.end_time ORDER BY md.end_time) ,1,(CASE WHEN md.reason_id=0 THEN RC.WIP_OUT_SCRAP_QTY ELSE 0 END),0) WIP_OUT_SCRAP_QTY,
SE.EMP_NO,SE.EMP_NAME,
TO_CHAR(md.start_time, 'YYYY/ MM/DD') START_DATE, md.start_time START_DATE_TIME,
TO_CHAR(md.start_time, 'HH24:MI :SS ') "START_TIME",
TO_CHAR(md.end_time, 'YYYY/ MM/DD ') "END_DATE", md.end_time  END_DATE_TIME,
TO_CHAR(md.end_time, 'HH24:MI :SS') "END_TIME",
NVL(TRUNC((md.end_time -md.start_time) * 24 * 60),0) MIN,
NVL(TRUNC((md.end_time - md.start_time) * 24 * 60 * 60),0) Second,
md.load_qty * NVL(D.OPTION1, 0) STANDARD_WORKTIME,
TRUNC((md.end_time - md.start_time) * 24 * 60*60)-  md.load_qty * NVL(D.OPTION1, 0) Diff_Time,
md.wo work_order, RC.travel_id,
NVL(mdd.reason_desc ,mdd.reason_code ) reason
FROM (SELECT md1.*, s.part_id, s.process_id, s.work_order WO, s.Wip_In_Qty, s.sheet_name FROM  SAJET.G_RC_TRAVEL_MACHINE_DOWN md1, sajet.g_rc_status s WHERE md1.rc_no=s.rc_no )  md
LEFT JOIN SAJET.G_RC_TRAVEL RC ON RC.RC_NO = md.RC_NO   and md.node_id=RC.node_id --AND RC.travel_id = md.travel_id
JOIN SAJET.SYS_MACHINE SM ON SM.MACHINE_ID = md.MACHINE_ID
JOIN SAJET.SYS_PART SP ON SP.PART_ID = MD.PART_ID
JOIN SAJET.SYS_PROCESS PRO ON NVL(rc.process_id, md.PROCESS_ID )= PRO.PROCESS_ID
JOIN SAJET.SYS_EMP SE ON md.UPDATE_USERID = SE.EMP_ID
left join sajet.SYS_MACHINE_DOWN_DETAIL mdd on md.reason_id = mdd.reason_id
left join SAJET.SYS_PROCESS_OPTION_PART   D  on ( md.PART_ID = D.PART_ID  AND md.NODE_ID = D.NODE_ID)
;