using System;
using System.Collections.Generic;
using System.Text;
using SajetClass;

namespace SajetTable
{
    public class TableDefine
    {
        public static String gsDef_KeyField = "VENDOR_ID";
        public static String gsDef_Table = "SAJET.SYS_VENDOR";
        public static String gsDef_HTTable = "SAJET.SYS_HT_VENDOR";
        public static String gsDef_OrderField = "VENDOR_CODE"; //預設排序欄位
        public static String gsDef_KeyData = "VENDOR_CODE";  //用於Disable時的訊息顯示

        public struct TGrid_Field
        {
            public String sFieldName;
            public String sCaption;    //欄位Title          
        }
        public static TGrid_Field[] tGridField;

        public static void Initial_Table()
        {
            //要在Grid顯示出來的欄位及順序
            Array.Resize(ref tGridField, 3);
            tGridField[0].sFieldName = "VENDOR_CODE";
            tGridField[0].sCaption = "Vendor Code";
            tGridField[1].sFieldName = "VENDOR_NAME";
            tGridField[1].sCaption = "Vendor Name";
            tGridField[2].sFieldName = "VENDOR_DESC";
            tGridField[2].sCaption = "Description";

            //欄位多國語言
            for (int i = 0; i <= tGridField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridField[i].sCaption, 1);
                tGridField[i].sCaption = sText;
            }
        }

        public static string History_SQL(string sID)
        {
            string s = " Select a.Vendor_Code,a.Vendor_Name,a.Vendor_Desc "
                     + "       ,a.ENABLED,b.emp_name,a.UPDATE_TIME "
                     + " from " + TableDefine.gsDef_HTTable + " a "
                     + "     ,sajet.sys_emp b "
                     + " Where a." + TableDefine.gsDef_KeyField + " ='" + sID + "' "
                     + " and a.update_userid = b.emp_id(+) "
                     + " Order By a.Update_Time ";
            return s;
        }
    }
}
