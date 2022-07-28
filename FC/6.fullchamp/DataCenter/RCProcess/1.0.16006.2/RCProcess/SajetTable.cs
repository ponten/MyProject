using System;
using System.Collections.Generic;
using System.Text;
using SajetClass;
using System.Data;

namespace SajetTable
{
    public class TableDefine
    {
        public static String gsDef_KeyField = "PROCESS_ID";
        public static String gsDef_OrderField = "PROCESS_NAME"; //�w�]�Ƨ����
        public static String gsDef_KeyData = "PROCESS_NAME";  //�Ω�Disable�ɪ��T�����

        public static int g_iOptionCount = 0;
        public struct TGrid_Field
        {
            public String sFieldName;
            public String sCaption;    //���Title          
        }
        public static TGrid_Field[] tGridField;

        public static void Initial_Table()
        {
            //�n�bGrid��ܥX�Ӫ����ζ���
            Array.Resize(ref tGridField, 8);
            tGridField[0].sFieldName = "PROCESS_NAME";
            tGridField[0].sCaption = "Process Name";
            tGridField[1].sFieldName = "INNAME";
            tGridField[1].sCaption = "Intput Name";
            tGridField[2].sFieldName = "INDLL";
            tGridField[2].sCaption = "Input Dll";
            tGridField[3].sFieldName = "INCN";
            tGridField[3].sCaption = "Input CN";
            tGridField[4].sFieldName = "OUTNAME";
            tGridField[4].sCaption = "Output Name";
            tGridField[5].sFieldName = "OUTDLL";
            tGridField[5].sCaption = "Output Dll";
            tGridField[6].sFieldName = "OUTCN";
            tGridField[6].sCaption = "Output CN";
            tGridField[7].sFieldName = "PROCESS_ID";
            tGridField[7].sCaption = "PROCESS_ID";                               

            //���h��y��
            for (int i = 0; i <= tGridField.Length - 1; i++)
            {
                string sText = SajetCommon.SetLanguage(tGridField[i].sCaption, 1);
                tGridField[i].sCaption = sText;
            }
        }      
    }
}
