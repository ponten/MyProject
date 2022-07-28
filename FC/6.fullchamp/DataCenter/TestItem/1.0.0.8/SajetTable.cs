using System;
using System.Collections.Generic;
using System.Text;
using SajetClass;

namespace SajetTable
{
    public class TableDefine
    {
        public static String gsDef_KeyField = "ITEM_TYPE_ID";
        public static String gsDef_Table = "SAJET.SYS_TEST_ITEM_TYPE";
        public static String gsDef_HTTable = "SAJET.SYS_HT_TEST_ITEM_TYPE";
        public static String gsDef_OrderField = "ITEM_TYPE_NAME"; //預設排序欄位
        public static String gsDef_KeyData = "ITEM_TYPE_NAME";    //用於Disable時的訊息顯示
        public struct TGrid_Field
        {
            public String sFieldName;
            public String sCaption;    //欄位Title          
        }
        public static TGrid_Field[] tGridField;

        //Detail
        public static String gsDef_DtlKeyField = "ITEM_ID";
        public static String gsDef_DtlTable = "SAJET.SYS_TEST_ITEM";
        public static String gsDef_DtlHTTable = "SAJET.SYS_HT_TEST_ITEM";
        public static String gsDef_DtlOrderField = "ITEM_NAME"; //預設排序欄位
        public static String gsDef_DtlKeyData = "ITEM_NAME";    //用於Disable時的訊息顯示
        public struct TGridDetail_Field
        {
            public String sFieldName;
            public String sCaption;    //欄位Title          
        }
        public static TGridDetail_Field[] tGridDetailField;


        public static void Initial_Table()
        {
            //要在Grid顯示出來的欄位及順序
            //Master ========================
            Array.Resize(ref tGridField, 5);
            tGridField[0].sFieldName = "ITEM_TYPE_CODE";
            tGridField[0].sCaption = "Item Type Code";
            tGridField[1].sFieldName = "ITEM_TYPE_NAME";
            tGridField[1].sCaption = "Item Type Name";            
            tGridField[2].sFieldName = "ITEM_TYPE_DESC";
            tGridField[2].sCaption = "Item Type Description";
            tGridField[3].sFieldName = "ITEM_TYPE_DESC2";
            tGridField[3].sCaption = "Item Type Description2";
            tGridField[4].sFieldName = "MIN_INSP_QTY";
            tGridField[4].sCaption = "Min Inspect Qty";


            //欄位多國語言
            for (int i = 0; i <= tGridField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridField[i].sCaption, 1);
                tGridField[i].sCaption = sText;
            }

            //Detail===============
            Array.Resize(ref tGridDetailField, 7);
            tGridDetailField[0].sFieldName = "ITEM_CODE";
            tGridDetailField[0].sCaption = "Item Code";
            tGridDetailField[1].sFieldName = "ITEM_NAME";
            tGridDetailField[1].sCaption = "Item Name";            
            tGridDetailField[2].sFieldName = "ITEM_DESC";
            tGridDetailField[2].sCaption = "Description";
            tGridDetailField[3].sFieldName = "ITEM_DESC2";
            tGridDetailField[3].sCaption = "Description2";
            tGridDetailField[4].sFieldName = "HAS_VALUE";
            tGridDetailField[4].sCaption = "Has Value";
            tGridDetailField[5].sFieldName = "VALUETYPE";
            tGridDetailField[5].sCaption = "Value Type";
            //------------20110917-增加最小檢驗個數---------
            tGridDetailField[6].sFieldName = "MIN_INSP_QTY";
            tGridDetailField[6].sCaption = "Min Inspect Qty";
            //----------------------------------------------

            //欄位多國語言
            for (int i = 0; i <= tGridDetailField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridDetailField[i].sCaption, 1);
                tGridDetailField[i].sCaption = sText;
            }
        }

        public static string History_SQL(string sID)
        {
            string s = " Select a.Item_type_Name,a.Item_type_Code,a.Item_type_Desc,a.Item_type_Desc2 "
                     + "       ,a.MIN_INSP_QTY "
                     + "       ,a.ENABLED,b.emp_name,a.UPDATE_TIME "
                     + " from " + TableDefine.gsDef_HTTable + " a "
                     + "     ,sajet.sys_emp b "
                     + " Where a." + TableDefine.gsDef_KeyField + " ='" + sID + "' "
                     + " and a.update_userid = b.emp_id(+) "
                     + " Order By a.Update_Time ";
            return s;
        }
        public static string DetailHistory_SQL(string sID)
        {
            string s = " Select a.Item_Name,a.Item_Code,a.Item_Desc,a.Item_Desc2,a.has_value "
                     + "       ,DECODE(A.VALUE_TYPE ,'0','Number','1','Character','N/A') VALUETYPE "
                     + "       ,c.Item_type_Name \"Item Type Name\" "
                     + "       ,a.ENABLED,b.emp_name,a.UPDATE_TIME "
                     + "       ,a.MIN_INSP_QTY "                                                        
                     + " from " + TableDefine.gsDef_DtlHTTable + " a "
                     + "     ,sajet.sys_emp b "
                     + "     ,sajet.SYS_TEST_ITEM_TYPE c "
                     + " Where a." + TableDefine.gsDef_DtlKeyField + " ='" + sID + "' "
                     + " and a.update_userid = b.emp_id(+) "
                     + " and a.Item_type_id = c.Item_type_id(+) "
                     + " Order By a.Update_Time ";
            return s;
        }
    }
}
