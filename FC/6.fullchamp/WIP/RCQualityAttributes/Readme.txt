[1.0.0.11]
1.ぃ▆计秖秸俱щ计
2.讽夹非畉S0箇砞1

[1.0.0.10]
1.э代刚兜⊿Τ砞﹚代刚挡狦ぇ纗bug
2.代刚兜┾喷计秖SPC Monitor舱计秖虫
3.盢セ虫代刚妓セSPC Monitor竊翴秸俱SPC Monitor竊翴眎瑈祘
  菲公翴竊翴铬跌怠陪ボ硂瑈祘┮Τ代刚妓セ计沮
4.玻珇代刚い代刚兜Τ砞﹚虫娩QCご玂Τ砏恨


[1.0.0.9]
1.QC讽代刚兜ぃ计钡盢Column块戈箇砞N/Adisable
2.㊣sj_rc_output v9

[1.0.0.8]
1.秸俱SPC代兜CPK Result礚猭砞﹚拜肈
2.糤匡拒CPK Result讽у﹚浪代ㄌ沮

[1.0.0.6]
1.﹚籹祘IPQC┪QC
2.讽籹祘IPQC璸衡陪ボmean,Ca,Cp,Cpk
3.ミ戈g_spc_cpk纗mean,Ca,Cp,Cpk
4.穝SJ_RC_OUTPUTΤBONUS,筁丁籔瑈祘篈把计セv8
5.糤﹚CPK挡狦ざ珇浪ㄌ沮癘魁戈g_spc_cpk
6.璝籹祘IPQC代刚兜NG┪CPK挡狦NG笆讽代ぃ╊у篈ぃHOLD
7.璝籹祘QC代刚兜NG笆讽代ぃ╊у篈HOLD
8.璝籹祘IPQC代刚兜OK籔CPK挡狦OK筁▆珇计单讽场щ计
9.糤ぃ▆瞷禜Ω贺单璸衡瑈祘ぃ▆计秖﹚代刚兜挡狦
10.筁▆珇计籔ぃ▆珇计ざdisableぃ块パ祘Α璸衡
11.筁丁眖sys_base砞﹚﹚丁翴ㄌ沮狦⊿Τ砞﹚玥琌ヘ玡丁翴筁Closing Date	XXXXX	RC Manager

[1.0.0.5]
1.狦籹祘Τ铬筁匡拒籹祘ざ
2.▆珇ぃ▆珇计块计翴

[1.0.0.4]
1.穝糤匡拒籹祘ざ

[1.0.0.3]
1.穝糤WIP珇浪祘Α陪ボ┾喷计秖代刚兜
2.穝糤Μ栋代刚兜既戈畐
3.秸俱Μ栋ぃ▆瞷禜ㄌ沮籹祘ぃ▆瞷禜砞﹚场陪ボ既戈畐
3.盢WIP珇浪祘Α纗┮Τ代刚璸礶代刚兜兜ぃ▆瞷禜代刚单计沮
4.盢代刚兜い┾喷璸礶┾喷单单戈魁G_QC_SAMPLING_PLAN
5.把σG_QC_SAMPLING_PLAN砞﹚穝┾喷单狦⊿Τ砞﹚タ盽单
6.秸俱щ计籔玻计▆珇/ぃ▆珇块タ疊翴计

[1.0.0.1]
Updated by: Nancy
Date: 14:41 2016/4/6
1.支持多个测试大项切换测试，切换时显示当前项目已经保存的测试值
2.添加保存临时测试值，临时不良代码功能。保存后显示当前选中项目的测试值。
3.RC/SN的显示改为RC
4.打开界面定位到SN输入
5.如果RC没有SN，SN输入处改为RC输入，并且默认添加1，2，3。。。（每次输入）
6.测试小项需要把上下限拉出来进行比对，并且不符合规定的SN需要被打NG
7.检验批号的格式为RC+PROCESS+流水号，重复过站需要不同的批号
8.当没有SN时，打不良的输入格式为Defectcode|char(9)|Defectqty|char(27)
9.抽验计划改变时的逻辑修改（抽验等级，抽验数量等都要改变，抽验计划在测试大项完成时无法改变）
10.免检的项目，可以不抽检，如果抽检了，按照抽检结果执行
11.不良代码处理逻辑修改
--不同的测试大项，同一个SN可以打多个不良；
--同一个不良可以被用在不同的SN上；
--已经完成测试的项目不能再添加不良代码，也不能移除不良代码
12.增加检验批记录的表G_QC_LOT，记录检验批结果
13.Procedure：
-->SJ_RC_QC_TRANSFER_ITEMTYPE 更新测试大项
-->SJ_RC_QC_RECORD_ITEM       记录测试小项
-->SJ_RC_QC_UPDATE_SAMPLETYPE 更新抽验计划
-->SJ_RC_QC_REINSPECT;        整批重测
-->SJ_RC_QC_CLEAR_TEMP;       清空临时表
-->SJ_RC_QC_SET_RESULT        更新检验批次结果
-->SJ_RC_OUTPUT               产出
14.Table：
--g_qc_lot 检验批结果
--g_qc_lot_test_type 测试大项结果
--g_qc_lot_test_item 测试小项结果
--g_qc_sn_testitem_temp 测试小项结果临时表
--g_qc_sn_defect_temp 不良临时表
--g_rc_travel_defect 产出不良记录

