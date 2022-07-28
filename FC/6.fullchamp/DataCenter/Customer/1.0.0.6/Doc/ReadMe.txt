1.0.0.5
1.客戶基本資料的欄位需再增加：
EX: 郵遞區號 / 帳款地址 / 運送地址 / 統編  / 傳真

alter table sajet.SYS_CUSTOMER add CUSTOMER_POSTAL VARCHAR2(40) ;
alter table sajet.SYS_CUSTOMER add GUI_NUMBER VARCHAR2(40);
alter table sajet.SYS_CUSTOMER add PAY_ADDR VARCHAR2(40);
alter table sajet.SYS_CUSTOMER add SHIPPING_ADDR VARCHAR2(40);

comment on column SYS_CUSTOMER.customer_fax
  is '傳真';
comment on column SYS_CUSTOMER.customer_postal
  is '郵遞區號';
comment on column SYS_CUSTOMER.gui_number
  is '統編';
comment on column SYS_CUSTOMER.pay_addr
  is '付款地址';
comment on column SYS_CUSTOMER.shipping_addr
  is '運送地址';
  commit;

alter table sajet.SYS_HT_CUSTOMER add CUSTOMER_POSTAL VARCHAR2(40) ;
alter table sajet.SYS_HT_CUSTOMER add GUI_NUMBER VARCHAR2(40);
alter table sajet.SYS_HT_CUSTOMER add PAY_ADDR VARCHAR2(40);
alter table sajet.SYS_HT_CUSTOMER add SHIPPING_ADDR VARCHAR2(40);

comment on column SYS_HT_CUSTOMER.customer_fax
  is '傳真';
comment on column SYS_HT_CUSTOMER.customer_postal
  is '郵遞區號';
comment on column SYS_HT_CUSTOMER.gui_number
  is '統編';
comment on column SYS_HT_CUSTOMER.pay_addr
  is '付款地址';
comment on column SYS_HT_CUSTOMER.shipping_addr
  is '運送地址';
  commit;