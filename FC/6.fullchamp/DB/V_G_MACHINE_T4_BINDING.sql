CREATE OR REPLACE VIEW V_G_MACHINE_T4_BINDING AS
SELECT x."RC_NO_10A",x."RC_NO_10C",x."PROCESS_ID_10C",x."NODE_ID_10C",x."RC_NO_T4",x."PROCESS_ID_T4",x."NODE_ID_T4",x."MACHINE_ID",x."BINDING_QTY",x."CHECK_BOM",x."UPDATE_USERID",x."UPDATE_TIME",a.work_order WO_10A, b.work_order WO_10B, c.work_order WO_10C
FROM
   (SELECT   CASE WHEN SUBSTR(r.rc_no_t4,1,3) = '10B' THEN '10A' || SUBSTR(r.rc_no_t4,4) ELSE NULL END "RC_NO_10A"
    , r."RC_NO_10C",r."PROCESS_ID_10C",r."NODE_ID_10C",r."RC_NO_T4",r."PROCESS_ID_T4",r."NODE_ID_T4",r."MACHINE_ID",r."BINDING_QTY",r."CHECK_BOM",r."UPDATE_USERID",r."UPDATE_TIME"
    FROM  G_MACHINE_T4_BINDING r
   )x
    , G_RC_STATUS A
    , G_RC_STATUS B
    , G_RC_STATUS C
WHERE x.RC_NO_10A = a.rc_no(+)
AND   x.RC_NO_T4  = b.rc_no(+)
AND   x.RC_NO_10C = c.rc_no;
