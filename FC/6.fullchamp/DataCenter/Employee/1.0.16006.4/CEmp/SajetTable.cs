using System;
using System.Collections.Generic;
using System.Text;
using SajetClass;

namespace SajetTable
{
    public class TableDefine
    {
        public static String gsDef_KeyField = "EMP_ID";
        public static String gsDef_Table = "SAJET.SYS_EMP";
        public static String gsDef_HTTable = "SAJET.SYS_HT_EMP";
        public static String gsDef_OrderField = "EMP_NO"; //預設排序欄位
        public static String gsDef_KeyData = "EMP_NO";    //用於Disable時的訊息顯示

        public struct TGrid_Field
        {
            public String sFieldName;
            public String sCaption;    //欄位Title          
        }
        public static TGrid_Field[] tGridField;

        public struct TGrid_Field_2
        {
            public String sFieldName;
            public String sCaption;    //欄位Title          
        }
        public static TGrid_Field_2[] tGridField_2;

        public static void Initial_Table()
        {
            //要在Grid顯示出來的欄位及順序
            Array.Resize(ref tGridField, 10);            
            tGridField[0].sFieldName = "EMP_NO";
            tGridField[0].sCaption = "Employee No";
            tGridField[1].sFieldName = "EMP_NAME";
            tGridField[1].sCaption = "Employee Name";
            tGridField[2].sFieldName = "PDLINE_NAME";
            tGridField[2].sCaption = "Line";
            tGridField[3].sFieldName = "DEPT_NAME";
            tGridField[3].sCaption = "Department";
            tGridField[4].sFieldName = "SHIFT_NAME";
            tGridField[4].sCaption = "Shift";           
            tGridField[5].sFieldName = "EMAIL";
            tGridField[5].sCaption = "EMail";
            tGridField[6].sFieldName = "HOST_NAME";
            tGridField[6].sCaption = "Host Name";
            tGridField[7].sFieldName = "QUIT_DATE";
            tGridField[7].sCaption = "Quit Date";
            tGridField[8].sFieldName = "REMARK";
            tGridField[8].sCaption = "Remark";
            tGridField[9].sFieldName = "TRAINING_LIST";
            tGridField[9].sCaption = "Training List";

            //欄位多國語言
            for (int i = 0; i <= tGridField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridField[i].sCaption, 1);
                tGridField[i].sCaption = sText;
            }

            Array.Resize(ref tGridField_2, 1);
            tGridField_2[0].sFieldName = "ROLE_NAME";
            tGridField_2[0].sCaption = "Role Name";

            for (int i = 0; i <= tGridField_2.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridField_2[i].sCaption, 1);
                tGridField_2[i].sCaption = sText;
            }
        }

        public static string History_SQL(string sID)
        {
            string s = " Select a.emp_no,a.emp_name EMPLOYEE_NAME "
                     + "       ,c.factory_name,f.pdline_name,d.dept_name,e.shift_name "
                     + "       ,a.quit_date,a.email,a.host_name,a.remark,a.training_list "
                     + "       ,a.ENABLED,b.emp_name,a.UPDATE_TIME "
                     + " from " + TableDefine.gsDef_HTTable + " a "
                     + "     ,sajet.sys_emp b "
                     + "     ,sajet.sys_factory c "
                     + "     ,sajet.sys_dept d "
                     + "     ,sajet.sys_shift e "
                     + "     ,sajet.sys_pdline f "
                     + " Where a." + TableDefine.gsDef_KeyField + " ='" + sID + "' "
                     + " and a.update_userid = b.emp_id(+) "
                     + " and a.factory_id = c.factory_id(+) "
                     + " and a.dept_id = d.dept_id(+) "
                     + " and a.shift = e.shift_id(+) "
                     + " and a.pdline_id = f.pdline_id(+) "
                     + " Order By a.Update_Time ";
            return s;
        }
    }
}
