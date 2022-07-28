using System;
using System.Collections.Generic;
using System.Text;
using SajetClass;

namespace SajetTable
{
    public class TableDefine
    {
        public static String gsDef_KeyField = "CUSTOMER_ID";
        public static String gsDef_Table = "SAJET.SYS_CUSTOMER";
        public static String gsDef_HTTable = "SAJET.SYS_HT_CUSTOMER";
        public static String gsDef_OrderField = "CUSTOMER_CODE"; //預設排序欄位
        public static String gsDef_KeyData = "CUSTOMER_CODE";  //用於Disable時的訊息顯示

        public struct TGrid_Field
        {
            public String sFieldName;
            public String sCaption;    //欄位Title          
        }
        public static TGrid_Field[] tGridField;

        public static void Initial_Table()
        {
            //要在Grid顯示出來的欄位及順序
            Array.Resize(ref tGridField, 11);
            tGridField[0].sFieldName = "CUSTOMER_CODE";
            tGridField[0].sCaption = "Customer Code";
            tGridField[1].sFieldName = "CUSTOMER_NAME";
            tGridField[1].sCaption = "Customer Name";
            tGridField[2].sFieldName = "CUSTOMER_DESC";
            tGridField[2].sCaption = "Description";
            tGridField[3].sFieldName = "CUSTOMER_ADDR";
            tGridField[3].sCaption = "Customer Address";
            tGridField[4].sFieldName = "CUSTOMER_TEL";
            tGridField[4].sCaption = "Customer Tel";
            tGridField[5].sFieldName = "WARRANTY_BUFFER";
            tGridField[5].sCaption = "Warranty Buffer";
            tGridField[6].sFieldName = "CUSTOMER_POSTAL";
            tGridField[6].sCaption = "Customer Postal";
            tGridField[7].sFieldName = "PAY_ADDR";
            tGridField[7].sCaption = "Pay Addr";
            tGridField[8].sFieldName = "SHIPPING_ADDR";
            tGridField[8].sCaption = "Shipping Addr";
            tGridField[9].sFieldName = "GUI_NUMBER";
            tGridField[9].sCaption = "GUI Number";
            tGridField[10].sFieldName = "CUSTOMER_FAX";
            tGridField[10].sCaption = "Customer FAX";

            //欄位多國語言
            for (int i = 0; i <= tGridField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridField[i].sCaption, 1);
                tGridField[i].sCaption = sText;
            }
        }

        public static string History_SQL(string sID)
        {
            string s = " Select a.Customer_Code,a.Customer_Name,a.Customer_Desc,a.Customer_Addr,a.Customer_Tel "
                     + "       ,a.ENABLED,b.emp_name,a.UPDATE_TIME,A.WARRANTY_BUFFER "
                     + " from " + TableDefine.gsDef_HTTable + " a "
                     + "     ,sajet.sys_emp b "
                     + " Where a." + TableDefine.gsDef_KeyField + " ='" + sID + "' "
                     + " and a.update_userid = b.emp_id(+) "
                     + " Order By a.Update_Time ";
            return s;
        }
    }
}
