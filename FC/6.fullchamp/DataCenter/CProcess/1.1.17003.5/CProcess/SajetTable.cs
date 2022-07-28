using System;
using System.Collections.Generic;
using System.Text;
using SajetClass;

namespace SajetTable
{
    public class TableDefine
    {
        public static String gsDef_KeyField = "STAGE_ID";
        public static String gsDef_Table = "SAJET.SYS_STAGE";
        public static String gsDef_HTTable = "SAJET.SYS_HT_STAGE";
        public static String gsDef_OrderField = "STAGE_NAME"; //預設排序欄位
        public static String gsDef_KeyData = "STAGE_NAME";    //用於Disable時的訊息顯示

        public struct TGrid_Field
        {
            public String sFieldName;
            public String sCaption;    //欄位Title          
        }
        public static TGrid_Field[] tGridField;

        //Detail
        public static String gsDef_DtlKeyField = "PROCESS_ID";
        public static String gsDef_DtlTable = "SAJET.SYS_PROCESS";
        public static String gsDef_DtlHTTable = "SAJET.SYS_HT_PROCESS";
        public static String gsDef_DtlOrderField = "PROCESS_NAME"; //預設排序欄位
        public static String gsDef_DtlKeyData = "PROCESS_NAME";    //用於Disable時的訊息顯示
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
            tGridField[0].sFieldName = "STAGE_NAME";
            tGridField[0].sCaption = "Stage Name";
            tGridField[1].sFieldName = "STAGE_DESC";
            tGridField[1].sCaption = "Stage Description";
            tGridField[2].sFieldName = "STAGE_CODE";
            tGridField[2].sCaption = "Stage Code";

            //欄位多國語言
            for (int i = 0; i <= tGridField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridField[i].sCaption, 1);
                tGridField[i].sCaption = sText;
            }

            //Detail===============
            Array.Resize(ref tGridDetailField, 6);
            tGridDetailField[0].sFieldName = "PROCESS_NAME";
            tGridDetailField[0].sCaption = "Process Name";
            tGridDetailField[1].sFieldName = "PROCESS_DESC";
            tGridDetailField[1].sCaption = "Description";
            tGridDetailField[2].sFieldName = "PROCESS_DESC2";
            tGridDetailField[2].sCaption = "Description2";
            tGridDetailField[3].sFieldName = "PROCESS_CODE";
            tGridDetailField[3].sCaption = "Process Code";
            tGridDetailField[4].sFieldName = "TYPE_NAME";
            tGridDetailField[4].sCaption = "Operate Type";
            tGridDetailField[5].sFieldName = "WIP_ERP";
            tGridDetailField[5].sCaption = "WIP ERP";

            //欄位多國語言
            for (int i = 0; i <= tGridDetailField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridDetailField[i].sCaption, 1);
                tGridDetailField[i].sCaption = sText;
            }
        }

        public static string History_SQL(string sID)
        {
            string s = " Select a.Stage_Name,a.Stage_Desc,a.Stage_Code "
                     + "       ,a.ENABLED,b.emp_name,a.UPDATE_TIME "
                     + " from " + TableDefine.gsDef_HTTable + " a "
                     + "     ,sajet.sys_emp b "
                     + " Where a." + TableDefine.gsDef_KeyField + " ='" + sID + "' "
                     + " and a.update_userid = b.emp_id(+) "
                     + " Order By a.Update_Time ";
            return s;
        }
        public static string DetailHistory_SQL(string sID)
        {
            string s = " Select a.Process_Name,a.Process_Desc,a.Process_Desc2,a.Process_Code "
                     + "       ,c.Stage_Name "
                     + "       ,a.ENABLED,a.wip_erp,b.emp_name,a.UPDATE_TIME "
                     + " from " + TableDefine.gsDef_DtlHTTable + " a "
                     + "     ,sajet.sys_emp b "
                     + "     ,sajet.sys_stage c "
                     + " Where a." + TableDefine.gsDef_DtlKeyField + " ='" + sID + "' "
                     + " and a.update_userid = b.emp_id(+) "
                     + " and a.stage_id = c.stage_id(+) "
                     + " Order By a.Update_Time ";
            return s;
        }
    }
}
