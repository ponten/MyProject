create or replace procedure sj_rc_pack_get_lotno(i_empid in number,
                                                 o_lotno out varchar2)

 as
  tcnt number;
begin
  select count(lot_no)
    into tcnt
    from sajet.g_pack_carton_lotno
   where end_flag = 'N';

  if tcnt = 0 then
    --��ȡ�µ�����
    select to_char(sysdate, 'yymmdd') ||
           lpad(sajet.s_pack_carton_lotno.nextval, 3, '0')
      into o_lotno
      from dual;
    --д�����ݿ�
    insert into sajet.g_pack_carton_lotno
      (lot_no, emp_id)
    values
      (o_lotno, i_empid);
  elsif tcnt > 1 then
    o_lotno := 'NG';
  else
    --��ȡ����ʹ�õ�����
    select lot_no
      into o_lotno
      from sajet.g_pack_carton_lotno
     where end_flag = 'N';
  end if;
end;

 
/
