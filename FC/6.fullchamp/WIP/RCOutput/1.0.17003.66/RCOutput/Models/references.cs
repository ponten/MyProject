using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RCOutput.Models
{
    public struct TProgramInfo
    {
        public string sProgram;
        public string sFunction;
        public Dictionary<string, string> sSQL;
        public Dictionary<string, int> iInputVisible; // 顯示到第幾個欄位;
        public Dictionary<string, List<int>> iInputField; // 那些欄位可以維護(逗號分隔)
        public Dictionary<string, string> sOption; // 其他設定
        public DataSet dsRC;
        public DataSet dsSNParam;
        public int[] iSNInput;
        public TextBox txtGood;
        public TextBox txtScrap;
        public Dictionary<string, int> slDefect;
    }

    public struct TControlData
    {
        public string sFieldName;
        public Label lablControl;
        public TextBox txtControl;
    }

    public struct TRCroute
    {
        public string sNode_Id;
        public string sNext_Node;
        public string sNext_Process;
        public string sSheet_Name;
        public string sNode_type;
        public string g_sRouteID;
        public string g_sProcessID;
        public string sLink_Name;
        public string sTravel_Id;
    }
}
