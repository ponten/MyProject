using System;
using System.Collections.Generic;
using System.Text;
using SajetClass;

namespace SajetTable
{
    public class TableDefine
    {
        public static String gsDef_KeyField = "BARCODE";

        public static String gsDef_Table = "SAJET.SYS_REC_MATERIAL";

        public static String gsSel_Table = "SAJET.SYS_CUSTOMER";
        public static String gsDef_HTTable = "SAJET.SYS_HT_CUSTOMER";
        public static String gsDef_OrderField = "OPERATION_ID"; //預設排序欄位
        public static String gsDef_KeyData = "OPERATION_ID";  //用於Disable時的訊息顯示

        public struct TGrid_Field
        {
            public String sFieldName;
            public String sCaption;    //欄位Title          
        }
        public static TGrid_Field[] tGridField;
        public static TGrid_Field[] dummyGridField;


        /// <summary>
        /// 
        /// </summary>
        public static void Initial_Table()
        {
            //要在Grid顯示出來的欄位及順序
            Array.Resize(ref tGridField,16);
            tGridField[0].sFieldName = "CHKBOX";
            tGridField[0].sCaption = ""; //tGridField[0]. = "";
            tGridField[1].sFieldName = "BARCODE";
            tGridField[1].sCaption = "條碼";
            tGridField[2].sFieldName = "FURNACE_NUMBER";
            tGridField[2].sCaption = "爐號";
            tGridField[3].sFieldName = "SPEC";
            tGridField[3].sCaption = "規格說明";
            tGridField[4].sFieldName = "WEIGHT";
            tGridField[4].sCaption = "重量";
            tGridField[5].sFieldName = "LENGTH";
            tGridField[5].sCaption = "長度";
            tGridField[6].sFieldName = "DIAMETER";
            tGridField[6].sCaption = "直徑";

            tGridField[7].sFieldName = "ORDER_ID";
            tGridField[7].sCaption = "訂單編號";

            tGridField[8].sFieldName = "RC_NO";
            tGridField[8].sCaption = "流程卡號";

            tGridField[9].sFieldName = "SITE_AREA";
            tGridField[9].sCaption = "儲位";

            tGridField[10].sFieldName = "ORDER_TIME";
            tGridField[10].sCaption = "訂單日期";

            tGridField[11].sFieldName = "ONSITE_TIME";
            tGridField[11].sCaption = "入位日期";

            tGridField[12].sFieldName = "RECEIVED_TIME";
            tGridField[12].sCaption = "入廠日期";

            tGridField[13].sFieldName = "OUTPUT_TIME";
            tGridField[13].sCaption = "出廠日期";

            tGridField[14].sFieldName = "CREATE_TIME";
            tGridField[14].sCaption = "建立日期";
            tGridField[15].sFieldName = "UPDATE_TIME";
            tGridField[15].sCaption = "更新日期";
            //欄位多國語言
            for (int i = 0; i <= tGridField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridField[i].sCaption, 1);
                tGridField[i].sCaption = sText;
            }



            //Array.Resize(ref dummyGridField, 1);
            //dummyGridField[0].sFieldName = "BARCODE";
            //dummyGridField[0].sCaption = "條碼";
            //for (int i = 0; i <= dummyGridField.Length - 1; i++)
            //{
            //    string sText = SajetCommon.SetLanguage(dummyGridField[i].sCaption, 1);
            //    dummyGridField[i].sCaption = sText;
            //}

        }

        public static string History_SQL(string sID)
        {
            string s = " Select a.Customer_Code,a.Customer_Name,a.Customer_Desc,a.Customer_Addr,a.Customer_Tel "
                     + "       ,a.ENABLED,b.emp_name,a.UPDATE_TIME,A.WARRANTY_BUFFER "
                     + " from " + TableDefine.gsDef_HTTable + " a "
                     + "     ,sajet.sys_emp b "
                     + " Where a." + TableDefine.gsDef_KeyField + " ='" + sID + "' "
                     + " and a.update_userid = b.emp_id(+) "
                     + " Order By a.Update_Time ";
            return s;
        }
    }
}
