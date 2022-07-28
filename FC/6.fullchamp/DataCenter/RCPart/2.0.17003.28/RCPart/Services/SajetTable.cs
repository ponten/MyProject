using System;
using System.Collections.Generic;
using System.Text;
using SajetClass;
using System.Data;

namespace SajetTable
{
    public class TableDefine
    {
        public static string gsDef_KeyField = "PART_ID";
        public static string gsDef_Table = "SAJET.SYS_PART";
        public static string gsDef_HTTable = "SAJET.SYS_HT_PART";
        public static string gsDef_OrderField = "PART_NO"; //default column for sorting
        public static string gsDef_KeyData = "PART_NO";  //when disable, the Part_No is displayed

        
        public struct TGrid_Field
        {
            public string sFieldName;
            public string sCaption;    //Title          
        }
        public static TGrid_Field[] tGridField;

        public static void Initial_Table()
        {
            //The displayed columns and sequence on DataGrid on the fMain
            Array.Resize(ref tGridField, 17);
            tGridField[0].sFieldName = "PART_NO";
            tGridField[0].sCaption = "Part No";
            tGridField[1].sFieldName = "VERSION";
            tGridField[1].sCaption = "Version";
            tGridField[2].sFieldName = "CUST_PART_NO";
            tGridField[2].sCaption = "Customer Part No";
            tGridField[3].sFieldName = "SPEC1";
            tGridField[3].sCaption = "Spec1";
            tGridField[4].sFieldName = "UPC";
            tGridField[4].sCaption = "UPC";
            tGridField[5].sFieldName = "RULE_SET";
            tGridField[5].sCaption = "Rule Set";
            tGridField[6].sFieldName = "ROUTE_NAME";
            tGridField[6].sCaption = "Default Route";

            tGridField[7].sFieldName = "VENDOR_PART_NO";
            tGridField[7].sCaption = "Vendor Part No";
            tGridField[8].sFieldName = "SPEC2";
            tGridField[8].sCaption = "Spec2";
            tGridField[9].sFieldName = "OPTION1";
            tGridField[9].sCaption = "OPTION1";
            tGridField[10].sFieldName = "OPTION4";
            tGridField[10].sCaption = "OPTION4";
            tGridField[11].sFieldName = "OPTION5";
            tGridField[11].sCaption = "OPTION5";
            tGridField[12].sFieldName = "OPTION6";
            tGridField[12].sCaption = "OPTION6";
            tGridField[13].sFieldName = "OPTION2";
            tGridField[13].sCaption = "OPTION2";

            tGridField[14].sFieldName = "OPTION7";
            tGridField[14].sCaption = "OPTION7";
            tGridField[15].sFieldName = "OPTION8";
            tGridField[15].sCaption = "OPTION8";
            tGridField[16].sFieldName = "MATERIAL_TYPE";
            tGridField[16].sCaption = "Material type";

            /*
            tGridField[10].sFieldName = "OPTION13";
            tGridField[10].sCaption = "OPTION13";
            tGridField[11].sFieldName = "OPTION14";
            tGridField[11].sCaption = "OPTION14";
            tGridField[12].sFieldName = "OPTION15";
            tGridField[12].sCaption = "OPTION15";
            */

            //Set language
            for (int i = 0; i <= tGridField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridField[i].sCaption, 1);
                tGridField[i].sCaption = sText;
            }
        }

        public static string History_SQL(string sID)
        {
            string s = " Select a.PART_NO,a.VERSION,a.CUST_PART_NO,a.SPEC1,a.UPC,a.RULE_SET,d.ROUTE_NAME "
                     + "   ,a.VENDOR_PART_NO,a.SPEC2,a.OPTION1,a.OPTION4,a.OPTION5,a.OPTION6,a.OPTION2  "
                     + "   ,a.OPTION7,a.OPTION8,a.MATERIAL_TYPE "
                     + "   , a.ENABLED,b.emp_name,a.UPDATE_TIME "

                     + " from " + gsDef_HTTable + " a "
                     + "     ,sajet.sys_emp b "
                     + "     ,sajet.sys_qc_sampling_plan c"
                     + "     ,sajet.sys_rc_route d "
                     + "     ,sajet.SYS_QC_SAMPLING_DEFAULT e "
                     + "     ,sajet.sys_rule_name f "

                     + " Where a." + gsDef_KeyField + " ='" + sID + "' "
                     + " and a.update_userid = b.emp_id "
                     + " and a.part_id = e.part_id(+) "
                     + " and e.sampling_id = c.sampling_id(+) "
                     + " and a.ROUTE_ID = d.ROUTE_ID(+) "
                     + " and a.mac_rule_id = f.rule_id(+) "
                     + " Order By a.Update_Time ";
            return s;
        }    
    }
}
