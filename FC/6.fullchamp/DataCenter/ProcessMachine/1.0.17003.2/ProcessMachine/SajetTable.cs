using System;
using System.Collections.Generic;
using System.Text;
using SajetClass;

namespace SajetTable
{
    public class TableDefine
    {
        public static String gsDef_KeyField = "PROCESS_ID";
        public static String gsDef_Table = "SAJET.SYS_FACTORY";
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
            Array.Resize(ref tGridField, 4);
            tGridField[0].sFieldName = "PROCESS_NAME";
            tGridField[0].sCaption = "Process Name";
            tGridField[1].sFieldName = "MACHINE_CODE";
            tGridField[1].sCaption = "Machine Code";
            tGridField[2].sFieldName = "MACHINE_TYPE_NAME";
            tGridField[2].sCaption = "Machine Type";
            tGridField[3].sFieldName = "MACHINE_DESC";
            tGridField[3].sCaption = "Machine Desc";

            //欄位多國語言
            for (int i = 0; i <= tGridField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridField[i].sCaption, 1);
                tGridField[i].sCaption = sText;
            }
        }
    }
}
