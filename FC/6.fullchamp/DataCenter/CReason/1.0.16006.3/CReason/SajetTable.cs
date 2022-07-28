using System;
using System.Collections.Generic;
using System.Text;
using SajetClass;

namespace SajetTable
{
    public class TableDefine
    {
        public static String gsDef_KeyField = "REASON_ID";
        public static String gsDef_Table = "SAJET.SYS_REASON";
        public static String gsDef_HTTable = "SAJET.SYS_HT_REASON";
        public static String gsDef_OrderField = "a.REASON_CODE"; //預設排序欄位
        public static String gsDef_KeyData = "REASON_CODE";  //用於Disable時的訊息顯示

        public struct TGrid_Field
        {
            public String sFieldName;
            public String sCaption;    //欄位Title          
        }
        public static TGrid_Field[] tGridField;

        public static void Initial_Table()
        {
            //要在Grid顯示出來的欄位及順序
            Array.Resize(ref tGridField, 5);
            tGridField[0].sFieldName = "REASON_CODE";
            tGridField[0].sCaption = "Reason Code";
            tGridField[1].sFieldName = "REASON_DESC";
            tGridField[1].sCaption = "Description";
            tGridField[2].sFieldName = "REASON_DESC2";
            tGridField[2].sCaption = "Description2";           
            tGridField[3].sFieldName = "CODE_LEVEL";
            tGridField[3].sCaption = "Level";
            tGridField[4].sFieldName = "PARENT_REASON_CODE";
            tGridField[4].sCaption = "Parent Reason Code";

            //欄位多國語言
            for (int i = 0; i <= tGridField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridField[i].sCaption, 1);
                tGridField[i].sCaption = sText;
            }
        }

        public static string History_SQL(string sID)
        {
            string s = " Select a.REASON_CODE,a.REASON_DESC,a.REASON_DESC2 "
                     + "       ,a.code_Level ,c.REASON_CODE PARENT_REASON_CODE "     
                     + "       ,a.ENABLED,b.emp_name,a.UPDATE_TIME "
                     + " from " + TableDefine.gsDef_HTTable + " a "
                     + "     ,sajet.sys_emp b "
                     + "     ,sajet.sys_reason c "
                     + " Where a." + TableDefine.gsDef_KeyField + " ='" + sID + "' "
                     + " and a.update_userid = b.emp_id(+) "
                     + " and a.parent_reason_id = c.reason_id(+) "
                     + " Order By a.Update_Time ";
            return s;
        }
    }
}
