using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCOutput_WO.Models
{
    /// <summary>
    /// 流程卡報工資訊的資料模型
    /// </summary>
    public class RC_DETAILS
    {
        /// <summary>
        /// 是否選取。選取的流程卡要報工。
        /// </summary>
        public bool SELECT { get; set; } = false;

        /// <summary>
        /// 流程卡號
        /// </summary>
        public string RC_NO { get; set; }

        /// <summary>
        /// 當前數量
        /// </summary>
        public int CURRENT_QTY { get; set; }

        /// <summary>
        /// 重工數量
        /// </summary>
        public decimal REWORK_QTY { get; set; }

        /// <summary>
        /// 是否主動設定過報工時間
        /// </summary>
        public bool DateTimePicker_set { get; set; } = false;

        /// <summary>
        /// 報工時間，擁有權限的使用者才能修改
        /// </summary>
        public DateTime OUTPUT_TIME { get; set; }

        /// <summary>
        /// 不良
        /// </summary>
        public List<Defect> Defects { get; set; } = new List<Defect>();

        /// <summary>
        /// 資料收集
        /// </summary>
        public List<DataCollectModel> DataCollections { get; set; } = new List<DataCollectModel>();

        /// <summary>
        /// 機台
        /// </summary>
        public List<MachineDownModel> Machines { get; set; } = new List<MachineDownModel>();
    }
}
