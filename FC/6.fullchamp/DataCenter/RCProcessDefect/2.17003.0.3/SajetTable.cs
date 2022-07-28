using System;
using System.Collections.Generic;
using System.Text;
using SajetClass;

namespace SajetTable
{
    public class TableDefine
    {
        public static String gsDef_KeyField = "PROCESS_NAME";
        public static String gsDef_Table = "SAJET.SYS_RC_PROCESS_DEFECT";
        public static String gsDef_HTTable = "SAJET.SYS_HT_PROCESS";
        public static String gsDef_OrderField = "PROCESS_NAME"; //預設排序欄位
        public static String gsDef_KeyData = "PROCESS_NAME";  //用於Disable時的訊息顯示
               
        public struct TGrid_Field
        {
            public String sFieldName;
            public String sCaption;    //欄位Title          
        }
        public static TGrid_Field[] tGridField;

        public static void Initial_Table()
        {
            //要在Grid顯示出來的欄位及順序
            Array.Resize(ref tGridField, 6);
            tGridField[0].sFieldName = "PROCESS_NAME";
            tGridField[0].sCaption = "Process Name";
            tGridField[1].sFieldName = "PROCESS_DESC";
            tGridField[1].sCaption = "Process Description";
            tGridField[2].sFieldName = "DEFECT_CODE";
            tGridField[2].sCaption = "Defect Code";
            tGridField[3].sFieldName = "DEFECT_DESC";
            tGridField[3].sCaption = "Defect Description";
            tGridField[4].sFieldName = "EMP_NO";
            tGridField[4].sCaption = "EMP NO";
            tGridField[5].sFieldName = "UPDATE_TIME";
            tGridField[5].sCaption = "Update Time";

            //欄位多國語言
            for (int i = 0; i <= tGridField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridField[i].sCaption, 1);
                tGridField[i].sCaption = sText;
            }
        }

        public static string History_SQL(string sID)
        {
            string s = " Select a.PROCESS_NAME,a.PROCESS_DESC "
                     + "       ,a.ENABLED,b.emp_name,a.UPDATE_TIME "
                     + " from " + TableDefine.gsDef_HTTable + " a "
                     + "     ,sajet.sys_emp b "
                     + " Where a." + TableDefine.gsDef_KeyField + " ='" + sID + "' "
                     + " and a.update_userid = b.emp_id(+) "
                     + " Order By a.Update_Time ";
            return s;
        }
    }
}
