using System;
using System.Collections.Generic;
using System.Text;
using SajetClass;

namespace SajetTable
{
    public class TableDefine
    {
        public struct TGrid_Field
        {
            public string sFieldName;
            public string sCaption;    //欄位Title          
        }

        #region Master
        public static string gsDef_KeyField = "TYPE_ID";
        public static string gsDef_Table = "SAJET.SYS_MACHINE_DOWN_TYPE";
        public static string gsDef_HTTable = "SAJET.SYS_HT_MACHINE_DOWN_TYPE";
        public static string gsDef_OrderField = "TYPE_CODE"; //預設排序欄位
        public static string gsDef_KeyData = "TYPE_DESC";    //用於Disable時的訊息顯示

        public static TGrid_Field[] tGridField;
        #endregion

        #region Detail
        public static string gsDef_DtlKeyField = "REASON_ID";
        public static string gsDef_DtlTable = "SAJET.SYS_MACHINE_DOWN_DETAIL";
        public static string gsDef_DtlHTTable = "SAJET.SYS_HT_MACHINE_DOWN_DETAIL";
        public static string gsDef_DtlOrderField = "REASON_CODE"; //預設排序欄位
        public static string gsDef_DtlKeyData = "REASON_DESC";    //用於Disable時的訊息顯示

        public static TGrid_Field[] tGridDetailField;
        #endregion

        public static void Initial_Table()
        {
            //要在Grid顯示出來的欄位及順序
            #region Master
            Array.Resize(ref tGridField, 2);
            tGridField[0].sFieldName = "TYPE_CODE";
            tGridField[0].sCaption = "Type Code";
            tGridField[1].sFieldName = "TYPE_DESC";
            tGridField[1].sCaption = "Type Description";

            //欄位多國語言
            for (int i = 0; i <= tGridField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridField[i].sCaption, 1);
                tGridField[i].sCaption = sText;
            }
            #endregion

            #region Detail
            Array.Resize(ref tGridDetailField, 4);
            tGridDetailField[0].sFieldName = "REASON_CODE";
            tGridDetailField[0].sCaption = "Reason Code";
            tGridDetailField[1].sFieldName = "REASON_DESC";
            tGridDetailField[1].sCaption = "Description";
            tGridDetailField[2].sFieldName = "DESC2";
            tGridDetailField[2].sCaption = "Description 2";
            tGridDetailField[3].sFieldName = "COUNT_WORKTIME";
            tGridDetailField[3].sCaption = "Is worktime count";

            //欄位多國語言
            for (int i = 0; i <= tGridDetailField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridDetailField[i].sCaption, 1);
                tGridDetailField[i].sCaption = sText;
            }
            #endregion
        }

        public static string History_SQL(string sID)
        {
            string s = $@"
SELECT
    A.TYPE_CODE
   ,A.TYPE_DESC
   ,A.ENABLED
   ,B.EMP_NAME
   ,A.UPDATE_TIME
FROM
    {TableDefine.gsDef_HTTable} A
   ,SAJET.SYS_EMP B
WHERE
    A.{TableDefine.gsDef_KeyField} ='{sID}'
AND A.UPDATE_USERID = B.EMP_ID(+)
ORDER BY
    A.UPDATE_TIME
";
            return s;
        }

        public static string DetailHistory_SQL(string sID)
        {
            string s = $@"
SELECT
    A.REASON_CODE
   ,A.REASON_DESC
   ,C.TYPE_CODE
   ,C.TYPE_DESC
   ,A.DESC2
   ,A.ENABLED
   ,B.EMP_NAME
   ,A.UPDATE_TIME
FROM
    {TableDefine.gsDef_DtlHTTable} A
   ,SAJET.SYS_EMP B
   ,{TableDefine.gsDef_Table} C
WHERE
    A.{TableDefine.gsDef_DtlKeyField} ='{sID}'
AND A.UPDATE_USERID = B.EMP_ID(+)
AND A.{TableDefine.gsDef_KeyField} = C.{TableDefine.gsDef_KeyField}(+)
ORDER BY
    A.UPDATE_TIME
";
            return s;
        }
    }
}
