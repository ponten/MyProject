using System;
using System.Linq;


namespace CSaleOrderInput
{
    public class TableDefine
    {
        public static string gsDef_KeyField = "NUMBER1";
        public static string gsDef_Table = "SAJET.SYS_ORD_H";
        public static string gsDef_OrderField = "NUMBER1"; //預設排序欄位
        public static string gsDef_KeyData = "NUMBER1";    //用於Disable時的訊息顯示

        public struct TGrid_Field
        {
            public string sFieldName;   //DB column name
            public string sCaption;     //visualization on UI (multi-language)    
        }
        public static TGrid_Field[] tGridField;

        //Detail
        public static string gsDef_DtlKeyField = "NUMBER1";
        public static string gsDef_DtlTable = "SAJET.SYS_ORD_D";
        public static string gsDef_DtlOrderField = "NUMBER1"; //預設排序欄位
        public static string gsDef_DtlKeyData = "NUMBER1";    //用於Disable時的訊息顯示

        public struct TGridDetail_Field
        {
            public string sFieldName; //DB column name  
            public string sCaption; //visualization on UI (multi-language)       
            public bool Visible; // visible on DataGridView
        }

        public static TGridDetail_Field[] tGridDetailField;

        public static void Initial_Table()
        {
            //要在Grid顯示出來的欄位及順序
            //Master ========================
            Array.Resize(ref tGridField, 16);
            tGridField[0].sFieldName = "OPERATION_ID";
            tGridField[0].sCaption = "Operation ID";
            tGridField[1].sFieldName = "OPERATION_NAME";
            tGridField[1].sCaption = "Operation Name";
            tGridField[2].sFieldName = "NUMBER1";
            tGridField[2].sCaption = "Number1";
            tGridField[3].sFieldName = "NUMBER2";
            tGridField[3].sCaption = "Number2";
            tGridField[4].sFieldName = "CUSTOMER_ID";
            tGridField[4].sCaption = "Customer ID";
            tGridField[5].sFieldName = "CUSTOMER_NAME";
            tGridField[5].sCaption = "Customer Name";
            tGridField[6].sFieldName = "ACCOUNT_ID";
            tGridField[6].sCaption = "Account ID";
            tGridField[7].sFieldName = "ACCOUNT_NAME";
            tGridField[7].sCaption = "Account Name";
            tGridField[8].sFieldName = "TO_SEND_ID";
            tGridField[8].sCaption = "To Send ID";
            tGridField[9].sFieldName = "TO_SEND_NAME";
            tGridField[9].sCaption = "To Send Name";

            tGridField[10].sFieldName = "SALE_ID";
            tGridField[10].sCaption = "Sale ID";
            tGridField[11].sFieldName = "SALE_NAME";
            tGridField[11].sCaption = "Sale Name";
            tGridField[12].sFieldName = "REAL_DATE";
            tGridField[12].sCaption = "Real Date";
            tGridField[13].sFieldName = "REAL_DUE_DATE";
            tGridField[13].sCaption = "Real Due Date";
            tGridField[14].sFieldName = "CUSTOMIZE";
            tGridField[14].sCaption = "Customize";
            tGridField[15].sFieldName = "NUMBER3";
            tGridField[15].sCaption = "Number3";


            //欄位多國語言
            for (int i = 0; i <= tGridField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridField[i].sCaption);
                tGridField[i].sCaption = sText;
            }

            //Detail===============
            Array.Resize(ref tGridDetailField, 7);
            tGridDetailField[0].sFieldName = "NUMBER1";
            tGridDetailField[0].sCaption = "Number1";
            tGridDetailField[0].Visible = true;
            tGridDetailField[1].sFieldName = "NUMBER2";
            tGridDetailField[1].sCaption = "Number2";
            tGridDetailField[1].Visible = true;
            tGridDetailField[2].sFieldName = "PRODUCE_NUMBER";
            tGridDetailField[2].sCaption = "Produce Number";
            tGridDetailField[2].Visible = true;
            tGridDetailField[3].sFieldName = "SEQUENCE";
            tGridDetailField[3].sCaption = "Sequence";
            tGridDetailField[3].Visible = true;
            tGridDetailField[4].sFieldName = "QUANTITY";
            tGridDetailField[4].sCaption = "Quantity";
            tGridDetailField[4].Visible = true;
            tGridDetailField[5].sFieldName = "AMOUNT";
            tGridDetailField[5].sCaption = "Amount";
            tGridDetailField[5].Visible = true;
            tGridDetailField[6].sFieldName = "REAL_DUE_DATE";
            tGridDetailField[6].sCaption = "Real Due Date";
            tGridDetailField[6].Visible = true;


            //欄位多國語言
            for (int i = 0; i <= tGridDetailField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridDetailField[i].sCaption);
                tGridDetailField[i].sCaption = sText;
            }
        }

        public static string GetCaption(string field)
        {
            var headerColumns = tGridField.ToList();
            foreach (var column in headerColumns)
            {
                if (column.sFieldName == field)
                {
                    return column.sCaption;
                }
            }

            var detailColumns = tGridDetailField.ToList();
            foreach (var column in detailColumns)
            {
                if (column.sFieldName == field)
                {
                    return column.sCaption;
                }
            }

            return field;
        }
    }
}

