using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using SajetClass;

namespace RC_Split_Merge
{
    partial class fMain
    {
        DataSet dsRC;
        //string[][] Params = new string[1000][];
        DataTable dtl = new DataTable();
        DataTable table = new DataTable();
        int s = 0;
        //顯示拆併清單
        public void ShowRCSplitAndMergeList(string sRC)
        {
            dgvData.DataSource = null;
            dgvData.Rows.Clear();
            if (string.Empty.Equals(sRC))
                return;
            InitializeAddFlow();
            CreateRCHistory();


            Show_RCList(sRC);
            //Show_RCMergeList(sRC);
            
            //按照时间和rc排序并且去重复
            DataView dv = dsRC.Tables[0].AsDataView();
            dv.Sort = dsRC.Tables[0].Columns[1].ColumnName  + "," + dsRC.Tables[0].Columns[2].ColumnName + " ASC";
            //dv.ToTable(true, new string[] { dsRC.Tables[0].Columns[0].ColumnName, dsRC.Tables[0].Columns[2].ColumnName, dsRC.Tables[0].Columns[4].ColumnName});
            table = dv.ToTable(true, new string[] {
                dsRC.Tables[0].Columns[0].ColumnName, 
                dsRC.Tables[0].Columns[1].ColumnName, 
                dsRC.Tables[0].Columns[2].ColumnName, 
                dsRC.Tables[0].Columns[3].ColumnName, 
                dsRC.Tables[0].Columns[4].ColumnName,
                dsRC.Tables[0].Columns[5].ColumnName,
                dsRC.Tables[0].Columns[6].ColumnName,
                dsRC.Tables[0].Columns[7].ColumnName,
                dsRC.Tables[0].Columns[8].ColumnName,
                dsRC.Tables[0].Columns[9].ColumnName});


            CreateDiagram(table);

            dgvData.DataSource = table;
            dgvData.Columns[SajetCommon.SetLanguage("ActionType")].Visible = false;

            
            
        }

        private void Show_RCList(string _sRC)
        {
            //顯示由此RC拆批出的子RC

            sSQL = @"Select DISTINCT A.TRAVEL_ID,A.RC_NO,A.RC_QTY,A.SOURCE_RC_NO,A.SOURCE_RC_QTY,C.PROCESS_NAME,B.EMP_NAME ,TO_CHAR(A.UPDATE_TIME,'yyyy/mm/dd hh24:mi:ss') SPLIT_TIME  
                            from SAJET.G_RC_SPLIT A ,SAJET.SYS_EMP B,SAJET.SYS_PROCESS C
                            WHERE A.UPDATE_USERID = B.EMP_ID(+)
                                AND A.PROCESS_ID = C.PROCESS_ID(+)
                                AND A.SOURCE_RC_NO = '" + _sRC + "'";
            dsTemp = ClientUtils.ExecuteSQL(sSQL);

            for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
            {
                //string str = "";
                dsRC.Tables[0].Rows.Add(new string[]{
                                            dsTemp.Tables[0].Rows[i]["TRAVEL_ID"].ToString(),
                                            dsTemp.Tables[0].Rows[i]["SPLIT_TIME"].ToString(),
                                            dsTemp.Tables[0].Rows[i]["SOURCE_RC_NO"].ToString(),
                                            dsTemp.Tables[0].Rows[i]["SOURCE_RC_QTY"].ToString(),
                                            dsTemp.Tables[0].Rows[i]["RC_NO"].ToString(),
                                            dsTemp.Tables[0].Rows[i]["RC_QTY"].ToString(),
                                            dsTemp.Tables[0].Rows[i]["PROCESS_NAME"].ToString(),
                                            dsTemp.Tables[0].Rows[i]["EMP_NAME"].ToString(),
                                            "S",
                                            SajetCommon.SetLanguage("Split")
                    });
            }
            for(int j = 0;j<dsRC.Tables[0].Rows.Count;j++)
            {
                string str = "";
                ssRC = dsRC.Tables[0].Rows[j][SajetCommon.SetLanguage("To R/C")].ToString();
                sTravelId = dsRC.Tables[0].Rows[j][SajetCommon.SetLanguage("Travel ID")].ToString();

                if (dtl.Rows.Count == 0)
                {
                    DataRow drl = dtl.NewRow();
                    drl["RC NO"] = ssRC;
                    drl["Travel ID"] = sTravelId;
                    dtl.Rows.Add(drl);

                    Show_RCList(ssRC);

                }
                else
                {
                    for (int m = 0; m < dtl.Rows.Count; m++)
                    {
                        if (dtl.Rows[m][0].ToString() == ssRC && dtl.Rows[m][1].ToString() == sTravelId)
                        {
                            str = "next";
                            break;
                            //return;
                        }
                    }
                    if (str == "next")
                        continue;

                    DataRow drl = dtl.NewRow();
                    drl["RC NO"] = ssRC;
                    drl["Travel ID"] = sTravelId;
                    dtl.Rows.Add(drl);

                    ssRC = dtl.Rows[dtl.Rows.Count - 1][0].ToString();

                    Show_RCList(ssRC);
                }
            }

            sSQL1 = @"Select DISTINCT A.TRAVEL_ID,A.RC_NO,A.RC_QTY,A.SOURCE_RC_NO,A.SOURCE_RC_QTY,C.PROCESS_NAME,B.EMP_NAME ,TO_CHAR(A.UPDATE_TIME,'yyyy/mm/dd hh24:mi:ss') SPLIT_TIME  
                            from SAJET.G_RC_SPLIT A,SAJET.SYS_EMP B,SAJET.SYS_PROCESS C 
                            WHERE A.UPDATE_USERID = B.EMP_ID(+) 
                                AND A.PROCESS_ID = C.PROCESS_ID(+)  
                                AND A.RC_NO = '" + _sRC + "'";
            dsTemp1 = ClientUtils.ExecuteSQL(sSQL1);

            for (int i = 0; i < dsTemp1.Tables[0].Rows.Count; i++)
            {
                //string str = "";
                dsRC.Tables[0].Rows.Add(new string[]{
                                            dsTemp1.Tables[0].Rows[i]["TRAVEL_ID"].ToString(),
                                            dsTemp1.Tables[0].Rows[i]["SPLIT_TIME"].ToString(),
                                            dsTemp1.Tables[0].Rows[i]["SOURCE_RC_NO"].ToString(),
                                            dsTemp1.Tables[0].Rows[i]["SOURCE_RC_QTY"].ToString(),
                                            dsTemp1.Tables[0].Rows[i]["RC_NO"].ToString(),
                                            dsTemp1.Tables[0].Rows[i]["RC_QTY"].ToString(),
                                            dsTemp1.Tables[0].Rows[i]["PROCESS_NAME"].ToString(),
                                            dsTemp1.Tables[0].Rows[i]["EMP_NAME"].ToString(),
                                            "S",
                                            SajetCommon.SetLanguage("Split")
                });
            }
            for (int j = 0; j < dsRC.Tables[0].Rows.Count; j++)
            {
                string str = "";
                ssRC = dsRC.Tables[0].Rows[j][SajetCommon.SetLanguage("From R/C")].ToString();
                sTravelId = dsRC.Tables[0].Rows[j][SajetCommon.SetLanguage("Travel ID")].ToString();

                if (dtl.Rows.Count == 0)
                {
                    DataRow drl = dtl.NewRow();
                    drl["RC NO"] = ssRC;
                    drl["Travel ID"] = sTravelId;
                    dtl.Rows.Add(drl);

                    Show_RCList(ssRC);

                }
                else
                {
                    for (int m = 0; m < dtl.Rows.Count; m++)
                    {
                        if (dtl.Rows[m][0].ToString() == ssRC && dtl.Rows[m][1].ToString() == sTravelId)
                        {
                            str = "next";
                            break;
                            //return;
                        }
                    }
                    if (str == "next")
                        continue;

                    DataRow drl = dtl.NewRow();
                    drl["RC NO"] = ssRC;
                    drl["Travel ID"] = sTravelId;
                    dtl.Rows.Add(drl);

                    ssRC = dtl.Rows[dtl.Rows.Count - 1][0].ToString();

                    Show_RCList(ssRC);
                }
            }


            sSQL2 = @"Select DISTINCT A.TRAVEL_ID,A.RC_NO,A.RC_QTY,A.MERGE_RC_NO,A.MERGE_RC_QTY,C.PROCESS_NAME,B.EMP_NAME ,TO_CHAR(A.UPDATE_TIME,'yyyy/mm/dd hh24:mi:ss') MERGE_TIME  
                    from SAJET.G_RC_MERGE A,SAJET.SYS_EMP B,SAJET.SYS_PROCESS C   
                    WHERE A.UPDATE_USERID = B.EMP_ID(+) 
                        AND A.PROCESS_ID = C.PROCESS_ID(+)   
                        AND A.MERGE_RC_NO = '" + _sRC + "'";
            dsTemp2 = ClientUtils.ExecuteSQL(sSQL2);
            for (int k = 0; k < dsTemp2.Tables[0].Rows.Count; k++)
            {
                //string str = "";
                dsRC.Tables[0].Rows.Add(new string[]{
                                        dsTemp2.Tables[0].Rows[k]["TRAVEL_ID"].ToString(),
                                        dsTemp2.Tables[0].Rows[k]["MERGE_TIME"].ToString(),
                                        dsTemp2.Tables[0].Rows[k]["RC_NO"].ToString(),
                                        dsTemp2.Tables[0].Rows[k]["RC_QTY"].ToString(),
                                        dsTemp2.Tables[0].Rows[k]["MERGE_RC_NO"].ToString(),
                                        dsTemp2.Tables[0].Rows[k]["MERGE_RC_QTY"].ToString(),
                                        dsTemp2.Tables[0].Rows[k]["PROCESS_NAME"].ToString(),
                                        dsTemp2.Tables[0].Rows[k]["EMP_NAME"].ToString(),
                                        "M",
                                        SajetCommon.SetLanguage("Merge")
                    });
            }

            for (int j = 0; j < dsRC.Tables[0].Rows.Count; j++)
            {
                string str = "";
                ssRC = dsRC.Tables[0].Rows[j][SajetCommon.SetLanguage("From R/C")].ToString();
                sTravelId = dsRC.Tables[0].Rows[j][SajetCommon.SetLanguage("Travel ID")].ToString();

                if (dtl.Rows.Count == 0)
                {
                    DataRow drl = dtl.NewRow();
                    drl["RC NO"] = ssRC;
                    drl["Travel ID"] = sTravelId;
                    dtl.Rows.Add(drl);

                    Show_RCList(ssRC);

                }
                else
                {
                    for (int m = 0; m < dtl.Rows.Count; m++)
                    {
                        if (dtl.Rows[m][0].ToString() == ssRC && dtl.Rows[m][1].ToString() == sTravelId)
                        {
                            str = "next";
                            break;
                            //return;
                        }
                    }
                    if (str == "next")
                        continue;

                    DataRow drl = dtl.NewRow();
                    drl["RC NO"] = ssRC;
                    drl["Travel ID"] = sTravelId;
                    dtl.Rows.Add(drl);

                    ssRC = dtl.Rows[dtl.Rows.Count - 1][0].ToString();

                    Show_RCList(ssRC);
                }
            }

            sSQL3 = @"Select DISTINCT A.TRAVEL_ID,A.RC_NO,A.RC_QTY,A.MERGE_RC_NO,A.MERGE_RC_QTY,C.PROCESS_NAME,B.EMP_NAME ,TO_CHAR(A.UPDATE_TIME,'yyyy/mm/dd hh24:mi:ss') MERGE_TIME  
                                from SAJET.G_RC_MERGE A,SAJET.SYS_EMP B,SAJET.SYS_PROCESS C   
                                WHERE A.UPDATE_USERID = B.EMP_ID(+) 
                                    AND A.PROCESS_ID = C.PROCESS_ID(+)  
                                    AND A.RC_NO = '" + _sRC + "'";
            dsTemp3 = ClientUtils.ExecuteSQL(sSQL3);

            for (int i = 0; i < dsTemp3.Tables[0].Rows.Count; i++)
            {
                //string str = "";
                dsRC.Tables[0].Rows.Add(new string[]{
                                                dsTemp3.Tables[0].Rows[i]["TRAVEL_ID"].ToString(),
                                                dsTemp3.Tables[0].Rows[i]["MERGE_TIME"].ToString(),
                                                dsTemp3.Tables[0].Rows[i]["RC_NO"].ToString(),
                                                dsTemp3.Tables[0].Rows[i]["RC_QTY"].ToString(),
                                                dsTemp3.Tables[0].Rows[i]["MERGE_RC_NO"].ToString(),
                                                dsTemp3.Tables[0].Rows[i]["MERGE_RC_QTY"].ToString(), 
                                                dsTemp3.Tables[0].Rows[i]["PROCESS_NAME"].ToString(),
                                                dsTemp3.Tables[0].Rows[i]["EMP_NAME"].ToString(),
                                                "M",
                                                SajetCommon.SetLanguage("Merge")
                    });
            }

            for (int j = 0; j < dsRC.Tables[0].Rows.Count; j++)
            {
                string str = "";
                ssRC = dsRC.Tables[0].Rows[j][SajetCommon.SetLanguage("To R/C")].ToString();
                sTravelId = dsRC.Tables[0].Rows[j][SajetCommon.SetLanguage("Travel ID")].ToString();

                if (dtl.Rows.Count == 0)
                {
                    DataRow drl = dtl.NewRow();
                    drl["RC NO"] = ssRC;
                    drl["Travel ID"] = sTravelId;
                    dtl.Rows.Add(drl);

                    Show_RCList(ssRC);

                }
                else
                {
                    for (int m = 0; m < dtl.Rows.Count; m++)
                    {
                        if (dtl.Rows[m][0].ToString() == ssRC && dtl.Rows[m][1].ToString() == sTravelId)
                        {
                            str = "next";
                            break;
                            //return;
                        }
                    }
                    if (str == "next")
                        continue;

                    DataRow drl = dtl.NewRow();
                    drl["RC NO"] = ssRC;
                    drl["Travel ID"] = sTravelId;
                    dtl.Rows.Add(drl);

                    ssRC = dtl.Rows[dtl.Rows.Count - 1][0].ToString();

                    Show_RCList(ssRC);
                }
            }
        }

        private void CreateRCHistory()
        {
            dsRC = new DataSet();
            dsRC.Tables.Add("RCHistory");
            dsRC.Tables[0].Columns.AddRange(new DataColumn[]{
                                                   new DataColumn(SajetCommon.SetLanguage("Travel ID")),
                                                   new DataColumn(SajetCommon.SetLanguage("Time")),
                                                   new DataColumn(SajetCommon.SetLanguage("From R/C")),
                                                   new DataColumn(SajetCommon.SetLanguage("From Qty")),
                                                   new DataColumn(SajetCommon.SetLanguage("To R/C")),
                                                   new DataColumn(SajetCommon.SetLanguage("To Qty")), 
                                                   new DataColumn(SajetCommon.SetLanguage("Process Name")),
                                                   new DataColumn(SajetCommon.SetLanguage("Employee")),
                                                   new DataColumn(SajetCommon.SetLanguage("ActionType")),
                                                   new DataColumn(SajetCommon.SetLanguage("Action"))});



            dtl.Columns.AddRange(new DataColumn[]{
                                                   new DataColumn("RC NO"),
                                                   new DataColumn("Travel ID")
            });
        }

    }
}
