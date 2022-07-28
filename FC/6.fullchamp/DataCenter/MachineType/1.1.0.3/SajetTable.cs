using System;
using System.Collections.Generic;
using System.Text;
using SajetClass;

namespace SajetTable
{
    public class TableDefine
    {
        public static String gsDef_KeyField = "MACHINE_TYPE_ID";
        public static String gsDef_Table = "SAJET.SYS_MACHINE_TYPE";
        public static String gsDef_HTTable = "SAJET.SYS_HT_MACHINE_TYPE";
        public static String gsDef_OrderField = "MACHINE_TYPE_NAME"; //預設排序欄位
        public static String gsDef_KeyData = "MACHINE_TYPE_NAME";  //用於Disable時的訊息顯示

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
            tGridField[0].sFieldName = "MACHINE_TYPE_NAME";
            tGridField[0].sCaption = "Machine Type Name";
            tGridField[1].sFieldName = "MACHINE_TYPE_DESC";
            tGridField[1].sCaption = "Description";

            //欄位多國語言
            for (int i = 0; i <= tGridField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridField[i].sCaption, 1);
                tGridField[i].sCaption = sText;
            }

            Array.Resize(ref tGridField_2, 2);
            tGridField_2[0].sFieldName = "EMP_NO";
            tGridField_2[0].sCaption = "Employee No";
            tGridField_2[1].sFieldName = "EMP_NAME";
            tGridField_2[1].sCaption = "Machine Engineer";

            for (int i = 0; i <= tGridField_2.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridField_2[i].sCaption, 1);
                tGridField_2[i].sCaption = sText;
            }
        }

        public static string History_SQL(string sID)
        {
            string s = " Select a.Machine_Type_Name,a.Machine_Type_Desc "
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
