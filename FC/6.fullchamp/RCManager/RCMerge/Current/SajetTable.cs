using System;
using System.Collections.Generic;
using System.Text;
using SajetClass;
using System.Data;

namespace SajetTable
{
    public class TableDefine
    {
        public static String gsDef_KeyField = "RC_NO";
        public static String gsDef_Table = "SAJET.G_RC_STATUS";
        public static String gsDef_OrderField = "RC_NO"; //預設排序欄位
        public static String gsDef_KeyData = "RC_NO";  //用於Disable時的訊息顯示

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
            tGridField[0].sFieldName = "RC_NO";
            tGridField[0].sCaption = "RC No";
            tGridField[1].sFieldName = "SPEC1";
            tGridField[1].sCaption = "Spec1";
            tGridField[2].sFieldName = "VERSION";
            tGridField[2].sCaption = "Version";
            tGridField[3].sFieldName = "CURRENT_QTY";
            tGridField[3].sCaption = "Current Qty";

            //欄位多國語言
            for (int i = 0; i <= tGridField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridField[i].sCaption, 1);
                tGridField[i].sCaption = sText;
            }
        }
    }
}
