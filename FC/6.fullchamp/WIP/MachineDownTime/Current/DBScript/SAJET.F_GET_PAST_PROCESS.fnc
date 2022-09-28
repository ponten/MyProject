create or replace function F_GET_PAST_PROCESS(I_RID in NUMBER, I_CURR_PROCESS VARCHAR2 ) return  VARCHAR
is
  vNext  VARCHAR2(100) ;
  O_RES  VARCHAR2(2000);
  rRT    Sys_Rc_Route_Detail %ROWTYPE;
  vCurrProc VARCHAR2(200);
  rTmp      VARCHAR2(200);
  vCnt      NUMBER;
BEGIN 
  
  vCurrProc := CASE WHEN I_CURR_PROCESS IS NULL THEN 'END' ELSE I_CURR_PROCESS END;
  
  SELECT * 
  INTO   rrt
  FROM   Sys_Rc_Route_Detail r 
  WHERE  r.route_id = I_RID
  AND    r.node_content = 'START';
      
  WHILE rrt.NODE_CONTENT != vCurrProc 
  LOOP  
    BEGIN
    SELECT *
    INTO   rRT
    FROM   Sys_Rc_Route_Detail r 
    WHERE  r.route_id = I_RID
    AND    r.Node_Id = rRT.NEXT_NODE_ID
    AND    R.LINK_NAME = 'NEXT';
    EXCEPTION WHEN NO_DATA_FOUND THEN
      EXIT;
    END;  
    SELECT COUNT(*) INTO vCnt 
    FROM   Sys_Rc_Route_Detail r  
    WHERE r.route_id = I_RID AND r.group_id = rRT.Node_Id ;
    IF  vCnt> 0  THEN  
       SELECT O_RES || ',' || listagg(r.NODE_CONTENT,',') within GROUP(ORDER BY r.node_id)
       INTO   O_RES 
       FROM   Sys_Rc_Route_Detail r  
       WHERE  r.route_id = I_RID 
       AND    r.group_id = rRT.Node_Id
       GROUP BY r.group_id;
    ELSE
      O_RES := O_RES || ',' || rRT.NODE_CONTENT ;
    END IF;        
  END LOOP;
  
  O_RES := LTRIM(O_RES,',');
  
  dbms_output.put_line(O_RES); 
     
  RETURN O_RES;
end F_GET_PAST_PROCESS;
/
