using System;
using System.Collections.Generic;
using System.Text;
using SajetClass;

namespace SajetTable
{
    public class TableDefine
    {
        public static String gsDef_KeyField = "ITEM_ID";
        public static String gsDef_Table = "SAJET.SYS_ITEM";
        public static String gsDef_HTTable = "SAJET.SYS_HT_ITEM";
        public static String gsDef_OrderField = "ITEM_CODE"; //預設排序欄位
        public static String gsDef_KeyData = "ITEM_CODE";  //用於Disable時的訊息顯示

        public struct TGrid_Field
        {
            public String sFieldName;
            public String sCaption;    //欄位Title          
        }
        public static TGrid_Field[] tGridField;

        public static void Initial_Table()
        {
            //要在Grid顯示出來的欄位及順序
            Array.Resize(ref tGridField, 19);
            tGridField[0].sFieldName = "ITEM_TYPE_NAME";
            tGridField[0].sCaption = "Item Type Name";
            tGridField[1].sFieldName = "ITEM_CODE";
            tGridField[1].sCaption = "Item Code";
            tGridField[2].sFieldName = "ITEM_END_CODE";
            tGridField[2].sCaption = "Item End Code";
            tGridField[3].sFieldName = "ITEM_NAME";
            tGridField[3].sCaption = "Item Name";
            tGridField[4].sFieldName = "ITEM_DESC";
            tGridField[4].sCaption = "Description";
            tGridField[5].sFieldName = "PKSPEC_NAME";
            tGridField[5].sCaption = "Package Name";
            tGridField[6].sFieldName = "AUTOCLAVE_TYPE_NAME";
            tGridField[6].sCaption = "AutoClave Name";
            tGridField[7].sFieldName = "COST_CENTER_NAME";
            tGridField[7].sCaption = "Cost Center Name";
            tGridField[8].sFieldName = "ITEM_UNIT";
            tGridField[8].sCaption = "Item Unit";
            tGridField[9].sFieldName = "DEPRECIATION_COST";
            tGridField[9].sCaption = "Depreciation Cost";
            tGridField[10].sFieldName = "CONSUME_COST";
            tGridField[10].sCaption = "Consume Cost";
            tGridField[11].sFieldName = "ACTION_COST";
            tGridField[11].sCaption = "Action Cost";
            tGridField[12].sFieldName = "TOTAL_COST";
            tGridField[12].sCaption = "Total Cost";
            tGridField[13].sFieldName = "RENT_COST";
            tGridField[13].sCaption = "Rent Cost";
            tGridField[14].sFieldName = "DELIVER_COST";
            tGridField[14].sCaption = "Deliver Cost";
            tGridField[15].sFieldName = "CURRENT_TOTAL_QTY";
            tGridField[15].sCaption = "Current Total QTY";
            tGridField[16].sFieldName = "RETURN_CSR";
            tGridField[16].sCaption = "Return CSR";
            tGridField[17].sFieldName = "RETURN_WASH";
            tGridField[17].sCaption = "Return Wash";
            tGridField[18].sFieldName = "FROM_CSR";
            tGridField[18].sCaption = "From CSR";
            
           


            //欄位多國語言
            for (int i = 0; i <= tGridField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridField[i].sCaption, 1);
                tGridField[i].sCaption = sText;
            }
        }

        public static string History_SQL(string sID)
        {
            string s = " Select a.ITEM_Code,a.ITEM_END_Code,a.ITEM_Name,a.ITEM_Desc "
                     + "       ,f.ITEM_TYPE_NAME,c.PKSPEC_NAME,d.AUTOCLAVE_TYPE_NAME,e.COST_CENTER_NAME "
                     + "       ,a.ENABLED,b.emp_name,a.UPDATE_TIME,a.CURRENT_TOTAL_QTY,a.RETURN_CSR,a.RETURN_WASH "
                     + " from " + TableDefine.gsDef_HTTable + " a "
                     + "     ,sajet.sys_emp b "
                     + "     ,sajet.sys_ITEM_TYPE f,sajet.sys_PKSPEC c,sajet.sys_AUTOCLAVE_TYPE d,sajet.sys_COST_CENTER e "
                     + " Where a." + TableDefine.gsDef_KeyField + " ='" + sID + "' "
                     + " and a.update_userid = b.emp_id(+) "
                     + " and a.ITEM_TYPE_ID = f.ITEM_TYPE_ID(+) " 
                     + " and a.PKSPEC_id = c.PKSPEC_id(+) "
                     + " and a.AUTOCLAVE_id = d.AUTOCLAVE_TYPE_id(+) "
                     + " and a.COST_CENTER_id = e.COST_CENTER_id(+) "
                     + " Order By a.Update_Time ";
            return s;
        }
    }
}
