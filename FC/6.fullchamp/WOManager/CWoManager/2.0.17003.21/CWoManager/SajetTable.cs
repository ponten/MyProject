using System;
using System.Collections.Generic;
using System.Text;
using SajetClass;
using System.Data;

namespace SajetTable
{
    public class TableDefine
    {
        public static String gsDef_KeyField = "WORK_ORDER";
        public static String gsDef_Table = "SAJET.G_WO_BASE";
        public static String gsDef_HTTable = "SAJET.G_HT_WO_BASE";
        public static String gsDef_OrderField = "WORK_ORDER"; //預設排序欄位
        public static String gsDef_KeyData = "WORK_ORDER";  //用於Disable時的訊息顯示
        
        public static int g_iOptionCount = 0;

        public struct TGrid_Field
        {
            public String sFieldName;
            public String sCaption;    //欄位Title          
        }
        public static TGrid_Field[] tGridField;

        public static void Initial_Table(string sField, string sLabel)
        {
            //要在Grid顯示出來的欄位及順序
            List<string> slField = new List<string>();
            List<string> slCaption = new List<string>();
           
            {
                slField.Add("WORK_ORDER");
                slCaption.Add("Work Order");
                slField.Add("WOSTATUS");
                slCaption.Add("W/O Status");
                slField.Add("PART_NO");
                slCaption.Add("Part No");
                slField.Add("VERSION");
                slCaption.Add("Version");
                slField.Add("WO_RULE");
                slCaption.Add("W/O Rule");

                slField.Add("WO_TYPE");
                slCaption.Add("W/O Type");
                slField.Add("TARGET_QTY");
                slCaption.Add("Target Qty");
                slField.Add("PDLINE_NAME");
                slCaption.Add("Default Line");
                slField.Add("ROUTE_NAME");
                slCaption.Add("Route Name");
                
                slField.Add("MADE_CATEGORY");
                slCaption.Add("Made Cat.");
                slField.Add("CUSTOMER1");
                slCaption.Add("Customer1");

                slField.Add("CUSTOMER3");
                slCaption.Add("Customer3");
                slField.Add("SALES");
                slCaption.Add("Sales");
                slField.Add("CUSTOMIZE");
                slCaption.Add("Customize");
                slField.Add("PRODUCT_RATE");
                slCaption.Add("Product Rate");
                slField.Add("OUTSOURCE");
                slCaption.Add("Out source");


                slField.Add("CUSTOMER_DATE");
                slCaption.Add("Cust. Date");
                slField.Add("CUSTOMER_DUE_DATE");
                slCaption.Add("Cust. Due Date");
                slField.Add("PALLETS");
                slCaption.Add("Pallets");
                slField.Add("WO_SCHEDULE_DATE");
                slCaption.Add("Schedule Date");
                slField.Add("WO_DUE_DATE");
                slCaption.Add("Due Date");

                slField.Add("RULE1");
                slCaption.Add("Rule 1");
                slField.Add("OLD_NUMBER");
                slCaption.Add("Old Number");
                slField.Add("UNIT");
                slCaption.Add("Unit");
                slField.Add("BLUEPRINT");
                slCaption.Add("Blue Print");
                slField.Add("FINISHED");
                slCaption.Add("Finished");

                slField.Add("BAD");
                slCaption.Add("Bad");
                slField.Add("NOTPAID");
                slCaption.Add("Not Paid");
                slField.Add("REMARK");
                slCaption.Add("Remark");

                
                Array.Resize(ref tGridField, slField.Count);
                for (int i = 0; i < slField.Count; i++)
                {
                    tGridField[i].sFieldName = slField[i];
                    tGridField[i].sCaption = SajetCommon.SetLanguage(slCaption[i]);
                }
               
            }
        }

        public static string History_SQL()
        {
            //用於顯示History中的title
            string sHistoryTitle = "";
            if (g_iOptionCount > 0)
            {
                int iStart = tGridField.Length - g_iOptionCount;
                for (int i = 0; i <= g_iOptionCount - 1; i++)
                    sHistoryTitle = sHistoryTitle + " ," + tGridField[iStart + i].sFieldName + " \"" + tGridField[iStart + i].sCaption + "\" ";
            }

            string s = string.Format(@" Select A.WORK_ORDER,A.UPDATE_TIME,A.WO_TYPE,A.WO_RULE,B.PART_NO,A.VERSION 
                        ,A.TARGET_QTY,A.INPUT_QTY,A.OUTPUT_QTY,A.SCRAP_QTY 
                        ,A.WO_CREATE_DATE,A.WO_SCHEDULE_DATE,A.WO_DUE_DATE,A.WO_START_DATE,A.WO_CLOSE_DATE 
                        ,A.PO_NO,A.SALES_ORDER,A.MASTER_WO,A.BURNIN_TIME,A.REMARK 
                        ,SAJET.SJ_WOStatus_Result(A.WO_STATUS) WOSTATUS 
                        ,D.PDLINE_NAME 
                        ,C.ROUTE_NAME 
                        ,F.PROCESS_NAME START_PROCESS 
                        ,G.PROCESS_NAME END_PROCESS 
                        ,E.CUSTOMER_CODE,E.CUSTOMER_NAME  
                        {0}
                        ,H.EMP_NAME UPDATE_USER 
                        From SAJET.G_HT_WO_BASE A 
                        left join SAJET.SYS_PART B on A.PART_ID=B.PART_ID 
                        left join SAJET.SYS_RC_ROUTE C on A.ROUTE_ID=C.ROUTE_ID 
                        left join SAJET.SYS_PDLINE D on A.DEFAULT_PDLINE_ID=D.PDLINE_ID 
                        left join SAJET.SYS_CUSTOMER E on A.CUSTOMER_ID=E.CUSTOMER_ID 
                        left join SAJET.SYS_PROCESS F on A.START_PROCESS_ID=F.PROCESS_ID 
                        left join SAJET.SYS_PROCESS G on A.END_PROCESS_ID=G.PROCESS_ID 
                        left join SAJET.SYS_EMP H on A.UPDATE_USERID=H.EMP_ID 
                        Where Work_Order = :WORK_ORDER 
                        Order By a.UPDATE_TIME ", sHistoryTitle);
            return s;
        }
    }
}
