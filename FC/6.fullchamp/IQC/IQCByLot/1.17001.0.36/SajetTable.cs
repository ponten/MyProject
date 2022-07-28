using System;
using System.Collections.Generic;
using System.Text;
using SajetClass;

namespace SajetTable
{
    public class TableDefine
    {
        public static String gsDef_KeyField = "RECID";
        public static String gsDef_Table = "SAJET.G_IQC_NOTES";
        public static String gsDef_HTTable = "SAJET.G_HT_IQC_NOTES";
        public static String gsDef_OrderField = "PART_NO "; //�w�]�Ƨ����
        //public static String gsDef_KeyData = "";  //�Ω�Disable�ɪ��T�����

        public struct TGrid_Field
        {
            public String sFieldName;
            public String sCaption;    //���Title          
        }
        public static TGrid_Field[] tGridField;

        public static void Initial_Table()
        {
            //�n�bGrid��ܥX�Ӫ����ζ���
            Array.Resize(ref tGridField, 6);            
            tGridField[0].sFieldName = "PART_NO";
            tGridField[0].sCaption = "Part No";
            tGridField[1].sFieldName = "NOTE";
            tGridField[1].sCaption = "Note";
            tGridField[2].sFieldName = "EMP_NO";
            tGridField[2].sCaption = "Emp No";
            tGridField[3].sFieldName = "EMP_NAME";
            tGridField[3].sCaption = "Emp Name";
            tGridField[4].sFieldName = "UPDATE_TIME";
            tGridField[4].sCaption = "Update Time";
            tGridField[5].sFieldName = "ATTACH_FILE";
            tGridField[5].sCaption = "Attach File";


            //���h��y��
            for (int i = 0; i <= tGridField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridField[i].sCaption, 1);
                tGridField[i].sCaption = sText;
            }
        }

        public static string History_SQL(string sID)
        {
            string s = " Select b.part_no,a.note "
                     + "       ,a.ENABLED,c.emp_name,a.UPDATE_TIME "
                     + " from " + TableDefine.gsDef_HTTable + " a "
                     + "     ,sajet.sys_part b "
                     + "     ,sajet.sys_emp c "
                     + " Where a." + TableDefine.gsDef_KeyField + " ='" + sID + "' "
                     + " and a.update_userid = c.emp_id(+) "
                     + " and a.part_id = b.part_id(+) "
                     + " Order By a.Update_Time ";
            return s;
        }
    }
}
