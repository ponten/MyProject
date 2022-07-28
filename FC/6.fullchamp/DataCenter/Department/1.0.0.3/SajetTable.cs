using System;
using System.Collections.Generic;
using System.Text;
using SajetClass;

namespace SajetTable
{
    public class TableDefine
    {
        public static String gsDef_KeyField = "DEPT_ID";
        public static String gsDef_Table = "SAJET.SYS_DEPT";
        public static String gsDef_HTTable = "SAJET.SYS_HT_DEPT";
        public static String gsDef_OrderField = "DEPT_NAME"; //�w�]�Ƨ����
        public static String gsDef_KeyData = "DEPT_NAME";  //�Ω�Disable�ɪ��T�����

        public struct TGrid_Field
        {
            public String sFieldName;
            public String sCaption;    //���Title          
        }
        public static TGrid_Field[] tGridField;

        public static void Initial_Table()
        {
            //�n�bGrid��ܥX�Ӫ����ζ���
            Array.Resize(ref tGridField, 2);
            tGridField[0].sFieldName = "DEPT_NAME";
            tGridField[0].sCaption = "Department Name";
            tGridField[1].sFieldName = "DEPT_DESC";
            tGridField[1].sCaption = "Description";

            //���h��y��
            for (int i = 0; i <= tGridField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridField[i].sCaption, 1);
                tGridField[i].sCaption = sText;
            }
        }

        public static string History_SQL(string sID)
        {
            string s = " Select c.factory_name,a.dept_name,a.dept_Desc "
                     + "       ,a.ENABLED,b.emp_name,a.UPDATE_TIME "
                     + " from " + TableDefine.gsDef_HTTable + " a "
                     + "     ,sajet.sys_emp b "
                     + "     ,sajet.sys_factory c "
                     + " Where a." + TableDefine.gsDef_KeyField + " ='" + sID + "' "
                     + " and a.update_userid = b.emp_id(+) "
                     + " and a.factory_id = c.factory_id(+) "
                     + " Order By a.Update_Time ";
            return s;
        }
    }
}
