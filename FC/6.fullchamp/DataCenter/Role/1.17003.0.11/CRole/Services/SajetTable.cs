using System;
using System.Collections.Generic;
using System.Text;
using SajetClass;

namespace SajetTable
{
    public class TableDefine
    {
        public static string gsDef_KeyField = "ROLE_ID";
        public static string gsDef_Table = "SAJET.SYS_ROLE";
        public static string gsDef_HTTable = "SAJET.SYS_HT_ROLE";
        public static string gsDef_OrderField = "ROLE_NAME"; //預設排序欄位
        public static string gsDef_KeyData = "ROLE_NAME";  //用於Disable時的訊息顯示

        public struct TGrid_Field
        {
            public string sFieldName;
            public string sCaption;    //欄位Title          
        }
        public static TGrid_Field[] tGridField;

        public static void Initial_Table()
        {
            //要在Grid顯示出來的欄位及順序
            Array.Resize(ref tGridField, 2);
            tGridField[0].sFieldName = "ROLE_NAME";
            tGridField[0].sCaption = "Role Name";
            tGridField[1].sFieldName = "ROLE_DESC";
            tGridField[1].sCaption = "Description";

            //欄位多國語言
            for (int i = 0; i <= tGridField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridField[i].sCaption, 1);
                tGridField[i].sCaption = sText;
            }
        }

        public static string History_SQL(string sID)
        {
            string s = " Select a.ROle_Name,a.Role_Desc "
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
