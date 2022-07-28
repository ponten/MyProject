using System;
using System.Collections.Generic;
using System.Text;
using SajetClass;

namespace SajetTable
{
    public class TableDefine
    {
        public static String gsDef_KeyField = "RECID";
        public static String gsDef_Table = "SAJET.SYS_PART_QC_PROCESS_RULE";
        public static String gsDef_HTTable = "SAJET.SYS_HT_PART_QC_PROCESS_RULE";
        public static String gsDef_OrderField = "PROCESS_NAME"; //預設排序欄位 
        public static String gsDef_KeyData = "PROCESS_NAME";    //用於Delete時的訊息顯示
        public struct TGrid_Field
        {
            public String sFieldName;
            public String sCaption;    //欄位Title          
        }
        public static TGrid_Field[] tGridField;

        //Detail Type=======   
        public static String gsDef_TypeKeyField = "ITEM_TYPE_ID";
        public static String gsDef_TypeTable = "SAJET.SYS_PART_QC_TESTTYPE";
        public static String gsDef_TypeHTTable = "SAJET.SYS_HT_PART_QC_TESTTYPE";
        public static String gsDef_TypeOrderField = "ITEM_TYPE_CODE"; //預設排序欄位        
        public static String gsDef_TypeKeyData = "ITEM_TYPE_CODE"; //用於Delete時的訊息顯示
        public struct TGridType_Field
        {
            public String sFieldName;
            public String sCaption;    //欄位Title          
        }
        public static TGridType_Field[] tGridTypeField;

        //Detail=======
        public static String gsDef_DtlKeyField = "ITEM_ID";
        public static String gsDef_DtlTable = "SAJET.SYS_PART_QC_TESTITEM";
        public static String gsDef_DtlHTTable = "SAJET.SYS_HT_PART_QC_TESTITEM";
        public static String gsDef_DtlOrderField = "ITEM_TYPE_CODE,ITEM_CODE"; //預設排序欄位
        public static String gsDef_DtlKeyData = "ITEM_CODE";    //用於Delete時的訊息顯示
        public struct TGridDetail_Field
        {
            public String sFieldName;
            public String sCaption;    //欄位Title          
        }
        public static TGridDetail_Field[] tGridDetailField;

        //ItemDetail
        public static String Item_gsDef_DtlKeyField = "ITEM_ID";
        public static String Item_gsDef_DtlTable = "SAJET.SYS_TEST_ITEM";
        public static String Item_gsDef_DtlHTTable = "SAJET.SYS_HT_TEST_ITEM";
        //public static String gsDef_DtlOrderField = "ITEM_NAME"; //預設排序欄位
        //public static String gsDef_DtlKeyData = "ITEM_NAME";    //用於Disable時的訊息顯示
        public struct ItemTGridDetail_Field
        {
            public String sFieldName;
            public String sCaption;    //欄位Title          
        }
        public static ItemTGridDetail_Field[] ItemtGridDetailField;

        public static void Initial_Table()
        {
            //要在Grid顯示出來的欄位及順序

            //Master ========================
            Array.Resize(ref tGridField, 2);
            tGridField[0].sFieldName = "PROCESS_NAME";
            tGridField[0].sCaption = "Process Name";
            tGridField[1].sFieldName = "SAMPLING_RULE_NAME";
            tGridField[1].sCaption = "Sampling Rule";           

            //欄位多國語言
            for (int i = 0; i <= tGridField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridField[i].sCaption, 1);
                tGridField[i].sCaption = sText;
            }

            //Detail Type===============
            Array.Resize(ref tGridTypeField, 3);
            tGridTypeField[0].sFieldName = "ITEM_TYPE_CODE";
            tGridTypeField[0].sCaption = "Item Type Code";
            tGridTypeField[1].sFieldName = "ITEM_TYPE_NAME";
            tGridTypeField[1].sCaption = "Item Type Name";
            tGridTypeField[2].sFieldName = "SAMPLING_TYPE";
            tGridTypeField[2].sCaption = "Sampling Plan";           

            //欄位多國語言
            for (int i = 0; i <= tGridTypeField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridTypeField[i].sCaption, 1);
                tGridTypeField[i].sCaption = sText;
            }

            //Detail===============
            Array.Resize(ref tGridDetailField, 11);
            tGridDetailField[0].sFieldName = "ITEM_CODE";
            tGridDetailField[0].sCaption = "Item Code";
            tGridDetailField[1].sFieldName = "ITEM_NAME";
            tGridDetailField[1].sCaption = "Item Name";
            tGridDetailField[2].sFieldName = "ITEM_TYPE_CODE";
            tGridDetailField[2].sCaption = "Item Type Code";
            tGridDetailField[3].sFieldName = "ITEM_TYPE_NAME";
            tGridDetailField[3].sCaption = "Item Type Name";
            tGridDetailField[4].sFieldName = "UPPER_LIMIT";
            tGridDetailField[4].sCaption = "USL";
            tGridDetailField[5].sFieldName = "LOWER_LIMIT";
            tGridDetailField[5].sCaption = "LSL";
            tGridDetailField[6].sFieldName = "MIDDLE_LIMIT";
            tGridDetailField[6].sCaption = "CL";
            tGridDetailField[7].sFieldName = "UPPER_CONTROL_LIMIT";
            tGridDetailField[7].sCaption = "UCL";
            tGridDetailField[8].sFieldName = "LOWER_CONTROL_LIMIT";
            tGridDetailField[8].sCaption = "LCL";
            tGridDetailField[9].sFieldName = "UNIT";
            tGridDetailField[9].sCaption = "Unit";
            tGridDetailField[10].sFieldName = "SORT_INDEX";
            tGridDetailField[10].sCaption = "Sort Index";

            //欄位多國語言
            for (int i = 0; i <= tGridDetailField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridDetailField[i].sCaption, 1);
                tGridDetailField[i].sCaption = sText;
            }


            //ItemDetail===============
            Array.Resize(ref ItemtGridDetailField, 7);
            ItemtGridDetailField[0].sFieldName = "ITEM_CODE";
            ItemtGridDetailField[0].sCaption = "Item Code";
            ItemtGridDetailField[1].sFieldName = "ITEM_NAME";
            ItemtGridDetailField[1].sCaption = "Item Name";
            ItemtGridDetailField[2].sFieldName = "ITEM_DESC";
            ItemtGridDetailField[2].sCaption = "Description";
            ItemtGridDetailField[3].sFieldName = "ITEM_DESC2";
            ItemtGridDetailField[3].sCaption = "Description2";
            ItemtGridDetailField[4].sFieldName = "HAS_VALUE";
            ItemtGridDetailField[4].sCaption = "Has Value";
            ItemtGridDetailField[5].sFieldName = "VALUETYPE";
            ItemtGridDetailField[5].sCaption = "Value Type";
            ItemtGridDetailField[6].sFieldName = "MIN_INSP_QTY";
            ItemtGridDetailField[6].sCaption = "Min Inspect Qty";

            //欄位多國語言
            for (int i = 0; i <= ItemtGridDetailField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(ItemtGridDetailField[i].sCaption, 1);
                ItemtGridDetailField[i].sCaption = sText;
            }
        }

        public static string History_SQL(string sID)
        {
            string s = " Select c.Process_Name ,d.SAMPLING_RULE_NAME "
                     + "  ,a.UPDATE_TIME , a.enabled "
                     + "  ,b.emp_name "
                     + "from " + TableDefine.gsDef_HTTable + " a "
                     + "     ,sajet.sys_emp b "
                     + "     ,sajet.sys_process c "
                     + "     ,sajet.sys_qc_sampling_rule d "
                     + " where a." + TableDefine.gsDef_KeyField + " ='" + sID + "' "
                     + " and a.update_userid = b.emp_id(+) "
                     + " and a.process_id = c.process_id(+) "
                     + " and a.sampling_rule_id = d.sampling_rule_id(+) "
                     + " Order By a.Update_Time ";
            return s;
        }
        public static string DetailHistory_SQL(string sID,string sRECID)
        {
            string s = " select c.item_code,c.item_name,d.ITEM_TYPE_CODE,d.item_type_name "
                     + "       ,a.UPPER_LIMIT USL,a.LOWER_LIMIT LSL,a.MIDDLE_LIMIT CL "
                     + "       ,a.UPPER_CONTROL_LIMIT UCL,a.LOWER_CONTROL_LIMIT LCL "
                     + "       ,a.UNIT,a.SORT_INDEX "
                     + "       ,a.UPDATE_TIME , a.enabled "
                     + "       ,b.emp_name "
                     + " from " + TableDefine.gsDef_DtlHTTable + " a "
                     + "   ,sajet.sys_emp b "
                     + "   ,sajet.sys_test_item c "
                     + "   ,sajet.sys_test_item_type d "
                     + " where a.recid = '" + sRECID + "' "
                     + " and a." + TableDefine.gsDef_DtlKeyField + " ='" + sID + "' "
                     + " and a.update_userid = b.emp_id(+) "
                     + " and a.item_id = c.item_id "
                     + " and c.item_type_id = d.item_type_id "
                     + " Order By a.Update_Time ";
            return s;
        }
       
        public static string TypeHistory_SQL(string sID, string sRECID)
        {
            string s = " select d.ITEM_TYPE_CODE,d.item_type_name,e.sampling_type "
                     + "       ,a.UPDATE_TIME , a.enabled "
                     + "       ,b.emp_name "
                      + "from " + TableDefine.gsDef_TypeHTTable + " a "
                     + ",sajet.sys_emp b "
                     + ",sajet.sys_test_item_type d "
                     + ",sajet.sys_qc_sampling_plan e "
                     + "where a.recid = '" + sRECID + "' "
                     + "and a." + TableDefine.gsDef_TypeKeyField + " ='" + sID + "' "
                     + "and a.update_userid = b.emp_id(+) "
                     + "and a.item_type_id = d.item_type_id "
                     + "and a.sampling_id = e.sampling_id(+) "
                     + "Order By a.Update_Time ";
            return s;
        }

        public static string ItemDetailHistory_SQL(string sID)
        {
            string s = " Select a.Item_Name,a.Item_Code,a.Item_Desc,a.Item_Desc2,a.has_value "
                     + "       ,DECODE(A.VALUE_TYPE ,'0','Number','1','Character','N/A') VALUETYPE "
                     + "       ,c.Item_type_Name \"Item Type Name\" "
                     + "       ,a.ENABLED,b.emp_name,a.UPDATE_TIME "
                     + "       ,a.MIN_INSP_QTY "
                     + " from " + TableDefine.Item_gsDef_DtlHTTable + " a "
                     + "     ,sajet.sys_emp b "
                     + "     ,sajet.SYS_TEST_ITEM_TYPE c "
                     + " Where a." + TableDefine.Item_gsDef_DtlKeyField + " ='" + sID + "' "
                     + " and a.update_userid = b.emp_id(+) "
                     + " and a.Item_type_id = c.Item_type_id(+) "
                     + " Order By a.Update_Time ";
            return s;
        }
    }
}
