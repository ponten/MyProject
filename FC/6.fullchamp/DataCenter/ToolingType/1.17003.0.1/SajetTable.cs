using System;
using System.Collections.Generic;
using System.Text;
using SajetClass;

namespace SajetTable
{
    public class TableDefine
    {
        public static String gsDef_KeyField = "TOOLING_TYPE_ID";
        public static String gsDef_Table = "SAJET.SYS_TOOLING_TYPE";
        public static String gsDef_HTTable = "SAJET.SYS_HT_TOOLING_TYPE";
        public static String gsDef_OrderField = "TOOLING_TYPE_NO"; //預設排序欄位
        public static String gsDef_KeyData = "TOOLING_TYPE_NO";    //用於Disable時的訊息顯示

        public struct TGrid_Field
        {
            public String sFieldName;
            public String sCaption;    //欄位Title          
        }
        public static TGrid_Field[] tGridField;

        public static void Initial_Table()
        {
            Array.Resize(ref tGridField, 3);
            tGridField[0].sFieldName = "TOOLING_TYPE_ID";
            tGridField[0].sCaption = "Tooling Type ID";
            tGridField[1].sFieldName = "TOOLING_TYPE_NO";
            tGridField[1].sCaption = "Tooling Type No";
            tGridField[2].sFieldName = "TOOLING_TYPE_DESC";
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
            string s = " SELECT A.TOOLING_TYPE_NO, A.TOOLING_TYPE_DESC "
                     + " ,A.ENABLED, B.EMP_NAME, A.UPDATE_TIME  "
                    
                     + " FROM " + TableDefine.gsDef_HTTable + " A, SAJET.SYS_EMP B "
                     + " WHERE A." + TableDefine.gsDef_KeyField + " ='" + sID + "' "
                     + " AND A.UPDATE_USERID = B.EMP_ID(+) "
                     + " ORDER BY A.UPDATE_TIME ";
            return s;
        }
    }
}
