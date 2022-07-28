using System;
using System.Collections.Generic;
using System.Text;
using SajetClass;

namespace SajetTable
{
    public class TableDefine
    {
        public static String gsDef_KeyField = "SAMPLING_RULE_ID";
        public static String gsDef_Table = "SAJET.SYS_QC_SAMPLING_RULE";
        public static String gsDef_HTTable = "SAJET.SYS_HT_QC_SAMPLING_RULE";
        public static String gsDef_OrderField = "SAMPLING_RULE_NAME"; //預設排序欄位
        public static String gsDef_KeyData = "SAMPLING_RULE_NAME";    //用於Disable時的訊息顯示

        public struct TGrid_Field
        {
            public String sFieldName;
            public String sCaption;    //欄位Title          
        }
        public static TGrid_Field[] tGridField;

        //Detail
        public static String gsDef_DtlKeyField = "DETAIL_ID";
        public static String gsDef_DtlTable = "SAJET.SYS_QC_SAMPLING_RULE_DETAIL";
        public static String gsDef_DtlOrderField = "SAMPLING_TYPE"; //預設排序欄位
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
            Array.Resize(ref tGridField, 3);
            tGridField[0].sFieldName = "SAMPLING_RULE_NAME";
            tGridField[0].sCaption = "Sampling Rule Name";
            tGridField[1].sFieldName = "SAMPLING_RULE_DESC";
            tGridField[1].sCaption = "Description";
            tGridField[2].sFieldName = "DEFAULT_FLAG";
            tGridField[2].sCaption = "Default";

            //欄位多國語言
            for (int i = 0; i <= tGridField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridField[i].sCaption, 1);
                tGridField[i].sCaption = sText;
            }

            //Detail===============
            Array.Resize(ref tGridDetailField, 5);
            tGridDetailField[0].sFieldName = "SAMPLING_LEVEL_DESC";
            tGridDetailField[0].sCaption = "Sampling Level";
            tGridDetailField[1].sFieldName = "CONTINUE_CNT";
            tGridDetailField[1].sCaption = "Continuous Cnt.";
            tGridDetailField[2].sFieldName = "REJECT_CNT";
            tGridDetailField[2].sCaption = "Reject Cnt.";
            tGridDetailField[3].sFieldName = "PASS_CNT";
            tGridDetailField[3].sCaption = "Pass Cnt.";
            tGridDetailField[4].sFieldName = "NEXT_SAMPLING_LEVEL_DESC";
            tGridDetailField[4].sCaption = "Next Sampling Level";

            //欄位多國語言
            for (int i = 0; i <= tGridDetailField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridDetailField[i].sCaption, 1);
                tGridDetailField[i].sCaption = sText;
            }
        }

        public static string History_SQL(string sID)
        {
            string s = " Select a.Sampling_rule_name,a.Sampling_rule_Desc "
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
