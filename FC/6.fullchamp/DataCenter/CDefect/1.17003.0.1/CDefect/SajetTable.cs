using System;
using System.Collections.Generic;
using System.Text;
using SajetClass;

namespace SajetTable
{
    public class TableDefine
    {
        public static String gsDef_KeyField = "DEFECT_ID";
        public static String gsDef_Table = "SAJET.SYS_DEFECT";
        public static String gsDef_HTTable = "SAJET.SYS_HT_DEFECT";
        public static String gsDef_OrderField = "a.DEFECT_CODE"; //預設排序欄位
        public static String gsDef_KeyData = "DEFECT_CODE";  //用於Disable時的訊息顯示

        public struct TGrid_Field
        {
            public String sFieldName;
            public String sCaption;    //欄位Title          
        }
        public static TGrid_Field[] tGridField;

        public static void Initial_Table()
        {
            //要在Grid顯示出來的欄位及順序
            Array.Resize(ref tGridField, 9);
            tGridField[0].sFieldName = "DEFECT_CODE";
            tGridField[0].sCaption = "Defect Code";
            tGridField[1].sFieldName = "DEFECTLEVEL";
            tGridField[1].sCaption = "Defect Level";
            tGridField[2].sFieldName = "DEFECT_DESC";
            tGridField[2].sCaption = "Description";
            tGridField[3].sFieldName = "DEFECT_DESC2";
            tGridField[3].sCaption = "Description2";
            tGridField[4].sFieldName = "DEFECT_TYPE_CODE";
            tGridField[4].sCaption = "Defect Type Code";
            tGridField[5].sFieldName = "DEFECT_TYPE_DESC";
            tGridField[5].sCaption = "Defect Type Desc";
            tGridField[6].sFieldName = "CODE_LEVEL";
            tGridField[6].sCaption = "Level";
            tGridField[7].sFieldName = "PARENT_DEFECT_CODE";
            tGridField[7].sCaption = "Parent Defect Code";
            tGridField[8].sFieldName = "COUNT_ENABLE";
            tGridField[8].sCaption = "Count Enable";

            //欄位多國語言
            for (int i = 0; i <= tGridField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridField[i].sCaption, 1);
                tGridField[i].sCaption = sText;
            }
        }

        public static string History_SQL(string sID)
        {
            string s = " Select a.DEFECT_CODE,a.DEFECT_DESC,a.DEFECT_DESC2,d.DEFECT_TYPE_CODE,d.DEFECT_TYPE_DESC "
                     + "       ,Decode(a.Defect_Level,'0','Critical','1','Major','2','Minor','N/A') DEFECTLEVEL "
                     + "       ,a.code_Level,c.DEFECT_CODE PARENT_DEFECT_CODE "     
                     + "       ,a.ENABLED,b.emp_name,a.UPDATE_TIME "
                     + " from " + TableDefine.gsDef_HTTable + " a "
                     + "     ,sajet.sys_emp b "
                     + "     ,sajet.sys_defect c "
                     + "     ,sajet.sys_defect_type d "
                     + " Where a." + TableDefine.gsDef_KeyField + " ='" + sID + "' "
                     + " and a.update_userid = b.emp_id(+) "
                     + " and a.parent_defect_id = c.defect_id(+) "
                     + " and a.defect_type_id = d.defect_type_id(+) "
                     + " Order By a.Update_Time ";
            return s;
        }
    }
}
