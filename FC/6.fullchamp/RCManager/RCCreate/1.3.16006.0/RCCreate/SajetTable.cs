using System;
using System.Collections.Generic;
using System.Text;
using SajetClass;
using System.Data;

namespace SajetTable
{
    public class TableDefine
    {
        public static String gsDef_KeyField = "RC_NO";
        public static String gsDef_Table = "SAJET.G_RC_STATUS";
        public static String gsDef_HTTable = "SAJET.G_RC_TRAVEL";
        public static String gsDef_OrderField = "WORK_ORDER"; //預設排序欄位
        public static String gsDef_KeyData = "RC_NO";  //用於Disable時的訊息顯示

        public static int g_iOptionCount = 0;

        public struct TGrid_Field
        {
            public String sFieldName;
            public String sCaption;    //欄位Title          
        }
        public static TGrid_Field[] tGridField;

        public struct TGrid_Field2
        {
            public String sFieldName;
            public String sCaption;    //欄位Title          
        }
        public static TGrid_Field2[] tGridField_2;

        public static void Initial_Table()
        {
            //要在Grid顯示出來的欄位及順序
            Array.Resize(ref tGridField, 2);
            tGridField[0].sFieldName = "WORK_ORDER";
            tGridField[0].sCaption = "Work Order";
            tGridField[1].sFieldName = "RC_NO";
            tGridField[1].sCaption = "Run Card No";

            //欄位多國語言
            for (int i = 0; i <= tGridField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridField[i].sCaption, 1);
                tGridField[i].sCaption = sText;
            }

            Array.Resize(ref tGridField_2, 4);
            tGridField_2[0].sFieldName = "WORK_ORDER";
            tGridField_2[0].sCaption = "Work Order";
            tGridField_2[1].sFieldName = "PART_NO";
            tGridField_2[1].sCaption = "Part No";
            tGridField_2[2].sFieldName = "SPEC1";
            tGridField_2[2].sCaption = "Spec 1";
            tGridField_2[3].sFieldName = "SPEC2";
            tGridField_2[3].sCaption = "Spec 2";

            for (int i = 0; i <= tGridField_2.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridField_2[i].sCaption, 1);
                tGridField_2[i].sCaption = sText;
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
                        left join SAJET.SYS_ROUTE C on A.ROUTE_ID=C.ROUTE_ID 
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
