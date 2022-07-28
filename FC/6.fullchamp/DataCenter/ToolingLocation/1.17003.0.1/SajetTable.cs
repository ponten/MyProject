using System;
using System.Collections.Generic;
using System.Text;
using SajetClass;

namespace SajetTable
{
    public class TableDefine
    {
        public static String gsDef_KeyField = "LOCATION_ID";
        public static String gsDef_Table = "SAJET.SYS_TOOLING_LOCATION";
        public static String gsDef_HTTable = "SAJET.SYS_HT_TOOLING_LOCATION";
        public static String gsDef_OrderField = "LOCATION_NO"; //�w�]�Ƨ����
        public static String gsDef_KeyData = "LOCATION_NO";    //�Ω�Disable�ɪ��T�����

        public struct TGrid_Field
        {
            public String sFieldName;
            public String sCaption;    //���Title          
        }
        public static TGrid_Field[] tGridField;

        public static void Initial_Table()
        {
            Array.Resize(ref tGridField, 3);
            tGridField[0].sFieldName = "LOCATION_ID";
            tGridField[0].sCaption = "Location ID";
            tGridField[1].sFieldName = "LOCATION_NO";
            tGridField[1].sCaption = "Location No";
            tGridField[2].sFieldName = "LOCATION_DESC";
            tGridField[2].sCaption = "Description";

            //���h��y��
            for (int i = 0; i <= tGridField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridField[i].sCaption, 1);
                tGridField[i].sCaption = sText;
            }
        }

        public static string History_SQL(string sID)
        {
            string s = " SELECT A.LOCATION_NO, A.LOCATION_DESC "
                     + " ,A.ENABLED, B.EMP_NAME, A.UPDATE_TIME  "
                    
                     + " FROM " + TableDefine.gsDef_HTTable + " A, SAJET.SYS_EMP B "
                     + " WHERE A." + TableDefine.gsDef_KeyField + " ='" + sID + "' "
                     + " AND A.UPDATE_USERID = B.EMP_ID(+) "
                     + " ORDER BY A.UPDATE_TIME ";
            return s;
        }
    }
}
