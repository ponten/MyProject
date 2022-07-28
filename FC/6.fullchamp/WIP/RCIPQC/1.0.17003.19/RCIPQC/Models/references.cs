using SajetClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RCIPQC.References
{
    struct TProgramInfo
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

    /// <summary>
    /// 記錄機台使用資訊的模型
    /// </summary>
    public class MachineDownModel
    {
        public MachineDownModel() { }

        public MachineDownModel(MachineDownModel e)
        {
            Select = e.Select;

            MACHINE_ID = e.MACHINE_ID;

            MACHINE_CODE = e.MACHINE_CODE;

            MACHINE_DESC = e.MACHINE_DESC;

            STATUS_NAME = e.STATUS_NAME;

            TYPE_ID = e.TYPE_ID;

            REASON_ID = e.REASON_ID;

            START_TIME = e.START_TIME;

            END_TIME = e.END_TIME;

            LOAD_QTY = e.LOAD_QTY;

            DATE_CODE = e.DATE_CODE;

            STOVE_SEQ = e.STOVE_SEQ;

            REMARK = e.REMARK;
        }

        public bool Select { get; set; } = false;

        public int MACHINE_ID { get; set; } = 0;

        public string MACHINE_CODE { get; set; } = string.Empty;

        public string MACHINE_DESC { get; set; } = string.Empty;

        public string STATUS_NAME { get; set; } = string.Empty;

        public int TYPE_ID { get; set; } = 0;

        public int REASON_ID { get; set; } = 0;

        public DateTime START_TIME { get; set; } = DateTime.Now;

        public DateTime END_TIME { get; set; } = DateTime.Now;

        public int? LOAD_QTY { get; set; } = 0;

        public string DATE_CODE { get; set; } = string.Empty;

        public string STOVE_SEQ { get; set; } = string.Empty;

        public string REMARK { get; set; } = string.Empty;
    }

    /// <summary>
    /// 下拉選單用的基本資料模型。
    /// </summary>
    public class BaseModel
    {
        public string ID { get; set; }

        public string CODE { get; set; }

        public string DESC { get; set; }
    }

    /// <summary>
    /// 停機類型。
    /// </summary>
    public class DownType : BaseModel
    {
        public List<DownDetail> DownDetails { get; set; }
    }

    /// <summary>
    /// 停機代碼。
    /// </summary>
    public class DownDetail : BaseModel
    {
        public bool CountWorkTime { get; set; } = true;
    }
}
