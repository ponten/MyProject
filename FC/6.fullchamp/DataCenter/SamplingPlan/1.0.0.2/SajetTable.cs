using System;
using System.Collections.Generic;
using System.Text;
using SajetClass;

namespace SajetTable
{
    public class TableDefine
    {
        public static String gsDef_KeyField = "SAMPLING_ID";
        public static String gsDef_Table = "SAJET.SYS_QC_SAMPLING_PLAN";
        public static String gsDef_HTTable = "SAJET.SYS_HT_QC_SAMPLING_PLAN";
        public static String gsDef_OrderField = "SAMPLING_TYPE"; //預設排序欄位
        public static String gsDef_KeyData = "SAMPLING_TYPE";    //用於Disable時的訊息顯示

        public struct TGrid_Field
        {
            public String sFieldName;
            public String sCaption;    //欄位Title          
        }
        public static TGrid_Field[] tGridField;

        //Detail
        public static String gsDef_DtlKeyField = "ROWID";
        public static String gsDef_DtlTable = "SAJET.SYS_QC_SAMPLING_PLAN_DETAIL";
        public static String gsDef_DtlOrderField = "min_lot_size"; //預設排序欄位
        public struct TGridDetail_Field
        {
            public String sFieldName;
            public String sCaption;    //欄位Title          
        }
        public static TGridDetail_Field[] tGridDetailField;


        public static void Initial_Table()
        {
            //要在Grid顯示出來的欄位及順序
            //Master ========================
            Array.Resize(ref tGridField, 2);
            tGridField[0].sFieldName = "SAMPLING_TYPE";
            tGridField[0].sCaption = "Sampling Plan Name";
            tGridField[1].sFieldName = "SAMPLING_DESC";
            tGridField[1].sCaption = "Description";

            //欄位多國語言
            for (int i = 0; i <= tGridField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridField[i].sCaption, 1);
                tGridField[i].sCaption = sText;
            }

            //Detail===============
            Array.Resize(ref tGridDetailField, 7);
            tGridDetailField[0].sFieldName = "MIN_LOT_SIZE";
            tGridDetailField[0].sCaption = "Min Lot Size";
            tGridDetailField[1].sFieldName = "MAX_LOT_SIZE";
            tGridDetailField[1].sCaption = "Max Lot Size";
            tGridDetailField[2].sFieldName = "SAMPLE_SIZE";
            tGridDetailField[2].sCaption = "Sample Size";
            tGridDetailField[3].sFieldName = "CRITICAL_REJECT_QTY";
            tGridDetailField[3].sCaption = "Critical Rej.";
            tGridDetailField[4].sFieldName = "MAJOR_REJECT_QTY";
            tGridDetailField[4].sCaption = "Major Rej.";
            tGridDetailField[5].sFieldName = "MINOR_REJECT_QTY";
            tGridDetailField[5].sCaption = "Minor Rej.";
            tGridDetailField[6].sFieldName = "SAMPLING_UNIT_DESC";
            tGridDetailField[6].sCaption = "Unit";

            //欄位多國語言
            for (int i = 0; i <= tGridDetailField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridDetailField[i].sCaption, 1);
                tGridDetailField[i].sCaption = sText;
            }
        }

        public static string History_SQL(string sID)
        {
            string s = " Select a.Sampling_type,a.Sampling_Desc "
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
