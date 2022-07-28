using System;
using System.Collections.Generic;
using System.Text;
using SajetClass;

namespace SajetTable
{
    public class TableDefine
    {
        public static String gsDef_KeyField = "DEFECT_TYPE_ID";
        public static String gsDef_Table = "SAJET.SYS_DEFECT_TYPE";
        public static String gsDef_HTTable = "SAJET.SYS_HT_DEFECT_TYPE";
        public static String gsDef_OrderField = "DEFECT_TYPE_CODE"; //預設排序欄位
        public static String gsDef_KeyData = "DEFECT_TYPE_DESC";    //用於Disable時的訊息顯示
        public struct TGrid_Field
        {
            public String sFieldName;
            public String sCaption;    //欄位Title          
        }
        public static TGrid_Field[] tGridField;


        //Detail
        public static String gsDef_DtlKeyField = "EMP_NO";
        public static String gsDef_DtlTable = "SAJET.SYS_DEFECT_TYPE_EMP";
        public static String gsDef_DtlHTTable = "SAJET.SYS_HT_DEFECT_TYPE_EMP";
        public static String gsDef_DtlOrderField = "EMP_NAME";
        public static String gsDef_DtlKeyData = "EMP_NAME";

        public struct TGridDetail_Field
        {
            public String sFieldName;
            public String sCaption;
        }
        public static TGridDetail_Field[] tGridDetailField;


        public static void Initial_Table()
        {
            Array.Resize(ref tGridField, 2);
            tGridField[0].sFieldName = "DEFECT_TYPE_CODE";
            tGridField[0].sCaption = "Defect Type Code";
            tGridField[1].sFieldName = "DEFECT_TYPE_DESC";
            tGridField[1].sCaption = "Defect Type Desc";

            //欄位多國語言
            for (int i = 0; i <= tGridField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridField[i].sCaption, 1);
                tGridField[i].sCaption = sText;
            }


            //Detail===============
            Array.Resize(ref tGridDetailField,2);
            tGridDetailField[0].sFieldName = "EMP_NO";
            tGridDetailField[0].sCaption = "Emp No";
            tGridDetailField[1].sFieldName = "EMP_NAME";
            tGridDetailField[1].sCaption = "Emp Name";


            //Detail===============
            for (int i = 0; i <= tGridDetailField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridDetailField[i].sCaption,1);
                tGridDetailField[i].sCaption = sText;
            }

        }

        public static string History_SQL(string sID)
        {
            string s = " Select a.DEFECT_TYPE_CODE,a.DEFECT_TYPE_DESC,b.emp_name,a.UPDATE_TIME,a.ENABLED "
                     + " From " + TableDefine.gsDef_HTTable + " a "
                     + "     ,sajet.sys_emp b "
                     + " Where a." + TableDefine.gsDef_KeyField + " ='" + sID + "' "
                     + " and a.update_userid = b.emp_id(+) "
                     + " Order By a.Update_Time ";
            return s;
        }

        //public static string DetailHistory_SQL(string sID)
        //{
        //    string s = "SELECT a.EMP_ID,a.DEFECT_TYPE_ID,a.UPDATE_TIME,a.UPDATE_USER_ID,a.ENABLE"
        //        + "FROM" + TableDefine.gsDef_DtlHTTable + "a "
        //        + ",sajet.sys_emp b,sajet.sys_defect_type"
        //        + "where a." + TableDefine.gsDef_DtlKeyField + "='" + sID + "'"
        //        + "and a.update_user_id = b.emp_id(+)"
        //        + "Order by a.update_time";
        //    return s;
        //}
    }
}
