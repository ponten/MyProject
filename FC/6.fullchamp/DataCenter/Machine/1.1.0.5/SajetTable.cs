using System;
using System.Collections.Generic;
using System.Text;
using SajetClass;
using System.Data;

namespace SajetTable
{
    public class TableDefine
    {
        public static String gsDef_KeyField = "MACHINE_ID";
        public static String gsDef_Table = "SAJET.SYS_MACHINE";
        public static String gsDef_HTTable = "SAJET.SYS_HT_MACHINE";
        public static String gsDef_OrderField = "MACHINE_CODE"; //預設排序欄位
        public static String gsDef_KeyData = "MACHINE_CODE";  //用於Disable時的訊息顯示

        public static int g_iOptionCount = 0;
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
            Array.Resize(ref tGridField, 9);
            tGridField[0].sFieldName = "MACHINE_CODE";
            tGridField[0].sCaption = "Machine Code";
            tGridField[1].sFieldName = "MACHINE_DESC";
            tGridField[1].sCaption = "Description";
            tGridField[2].sFieldName = "machine_loc";
            tGridField[2].sCaption = "Location";
            tGridField[3].sFieldName = "UTILIZATION_FLAG";
            tGridField[3].sCaption = "Utilization";
            tGridField[4].sFieldName = "MACHINE_TYPE_NAME";
            tGridField[4].sCaption = "Machine Type";
            tGridField[5].sFieldName = "VENDOR_NAME";
            tGridField[5].sCaption = "Vendor Name";
            tGridField[6].sFieldName = "ARRIVAL_TIME";
            tGridField[6].sCaption = "Arrival Date";
            tGridField[7].sFieldName = "WARRANTY_TIME";
            tGridField[7].sCaption = "Warranty Date";
            tGridField[8].sFieldName = "AUTO_RUN";
            tGridField[8].sCaption = "AUTO_RUN";

            //Option
            int iCount = tGridField.Length;
            string sSQL = "Select * from sajet.sys_sql "
                        + "where substr(SQL_NAME,1,8)='M_OPTION' "
                        + "order by SQL_NAME ";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
            g_iOptionCount = dsTemp.Tables[0].Rows.Count;
            Array.Resize(ref tGridField, iCount + g_iOptionCount);
            for (int i = 0; i <= dsTemp.Tables[0].Rows.Count - 1; i++)
            {
                tGridField[iCount + i].sFieldName = dsTemp.Tables[0].Rows[i]["SQL_NAME"].ToString();
                tGridField[iCount + i].sCaption = dsTemp.Tables[0].Rows[i]["DISPLAY_NAME"].ToString();
            }

            //欄位多國語言
            for (int i = 0; i <= tGridField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridField[i].sCaption, 1);
                tGridField[i].sCaption = sText;
            }

            Array.Resize(ref tGridField_2, 3);
            tGridField_2[0].sFieldName = "VENDOR_CODE";
            tGridField_2[0].sCaption = "Vendor No";
            tGridField_2[1].sFieldName = "VENDOR_NAME";
            tGridField_2[1].sCaption = "Vendor Name";
            tGridField_2[2].sFieldName = "VENDOR_DESC";
            tGridField_2[2].sCaption = "Vendor Description";

            for (int i = 0; i <= tGridField_2.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridField_2[i].sCaption, 1);
                tGridField_2[i].sCaption = sText;
            }
        }

        public static string History_SQL(string sID)
        {
            string sHistoryTitle = "";
            if (g_iOptionCount > 0)
            {
                int iStart = tGridField.Length - g_iOptionCount;
                for (int i = 0; i <= g_iOptionCount - 1; i++)
                    sHistoryTitle = sHistoryTitle + " ," + tGridField[iStart + i].sFieldName + " \"" + tGridField[iStart + i].sCaption + "\" ";
            }

            string s = " Select a.Machine_Code,a.Machine_Desc " //,d.pdline_name
                     + ",a.machine_loc,a.UTILIZATION_FLAG,c.machine_type_name "                   
                     + " ,d.vendor_name,a.arrival_time,a.warranty_time "
                     + sHistoryTitle
                     + " ,a.ENABLED,b.emp_name,a.UPDATE_TIME "
                     + " from sajet.sys_HT_machine a, "
                     + "        sajet.sys_emp b, "
                     + "        sajet.sys_machine_type c, "
                     + "     sajet.sys_vendor d "
                     //+ "        ,sajet.sys_pdline d "
                     + " Where a." + TableDefine.gsDef_KeyField + " ='" + sID + "' "
                     + " and a.machine_type_id = c.machine_type_id(+) "
                     //+ " and a.machine_loc = d.pdline_id(+) "
                     + " and a.update_userid = b.emp_id(+) "
                     + " and a.vendor_id = d.vendor_id(+) "
                     + " Order By a.Update_Time ";
            return s;
        }
    }
}
