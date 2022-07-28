using System;
using System.Collections.Generic;
using System.Text;
using SajetClass;

namespace SajetTable
{
    public class TableDefine
    {
        public static String gsDef_KeyField = "TOOLING_ID";
        public static String gsDef_Table = "SAJET.SYS_TOOLING";
        public static String gsDef_HTTable = "SAJET.SYS_HT_TOOLING";
        public static String gsDef_OrderField = "TOOLING_NO"; //�w�]�Ƨ����
        public static String gsDef_KeyData = "TOOLING_NO";    //�Ω�Disable�ɪ��T�����

        public struct TGrid_Field
        {
            public String sFieldName;
            public String sCaption;    //���Title          
        }
        public static TGrid_Field[] tGridField;

        public static void Initial_Table()
        {
            Array.Resize(ref tGridField, 10);
            tGridField[0].sFieldName = "TOOLING_ID";
            tGridField[0].sCaption = "Tooling ID";
            tGridField[1].sFieldName = "TOOLING_TYPE_NO";
            tGridField[1].sCaption = "Tooling Type";
            tGridField[2].sFieldName = "TOOLING_NO";
            tGridField[2].sCaption = "Tooling No";
            tGridField[3].sFieldName = "TOOLING_NAME";
            tGridField[3].sCaption = "Tooling Name";
            tGridField[4].sFieldName = "TOOLING_DESC";
            tGridField[4].sCaption = "Description";
            tGridField[5].sFieldName = "USED_TIME";
            tGridField[5].sCaption = "Used Time";
            tGridField[6].sFieldName = "MAX_USED_COUNT";
            tGridField[6].sCaption = "Max Used Count";
            tGridField[7].sFieldName = "STATUS";
            tGridField[7].sCaption = "Status";
            tGridField[8].sFieldName = "RESULT";
            tGridField[8].sCaption = "Result";
            tGridField[9].sFieldName = "LOCATION_NO";
            tGridField[9].sCaption = "Location No";
            //tGridField[8].sFieldName = "REMINDER"; //�O�i����
            //tGridField[8].sCaption = "MT Remind";
            //tGridField[9].sFieldName = "LAST_MAINTAIN_TIME";
            //tGridField[9].sCaption = "Last Maintain Time";
            //tGridField[10].sFieldName = "EMAIL";
            //tGridField[10].sCaption = "E-Mail";
            // tGridField[11].sFieldName = "COMPANY";
            // tGridField[11].sCaption = "Company";

            //���h��y��
            for (int i = 0; i <= tGridField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridField[i].sCaption, 1);
                tGridField[i].sCaption = sText;
            }
        }

        public static string History_SQL(string sID)
        {
            string s = " SELECT A.TOOLING_NO, A.TOOLING_NAME, A.TOOLING_DESC"
                     //+ " DECODE(A.TOOLING_TYPE, 'K', '�M��', 'T', '�v��') TOOLING_TYPE,"
                     + " ,A.MAX_USED_COUNT, A.ENABLED "
                     + " ,SAJET.SJ_TOOLING_STATUS_CHT(STATUS) STATUS, A.RESULT "
                     + " , B.EMP_NAME, A.UPDATE_TIME  "
                     + " ,C.TOOLING_TYPE_NO,D.LOCATION_NO "
                     //+ "        ,DECODE(A.REMINDER, 'Q','�u','M','��','H','�b�~') REMINDER, "
                     //+ "        A.LAST_MAINTAIN_TIME, A.EMAIL "
                     //+ ", DECODE(COMPANY, 'P','�ƪY','F','�I�h��') COMPANY "
                     + " FROM " + TableDefine.gsDef_HTTable + " A, SAJET.SYS_EMP B "
                     + " ,SAJET.SYS_TOOLING_TYPE C,SAJET.SYS_TOOLING_LOCATION D "
                     + " WHERE A." + TableDefine.gsDef_KeyField + " ='" + sID + "' "
                     + " AND A.UPDATE_USERID = B.EMP_ID(+) "
                     + " AND A.TOOLING_TYPE_ID = C.TOOLING_TYPE_ID(+) "
                     + " AND A.LOCATION_ID = D.LOCATION_ID(+) "
                     + " ORDER BY A.UPDATE_TIME ";
            return s;
        }
    }
}
