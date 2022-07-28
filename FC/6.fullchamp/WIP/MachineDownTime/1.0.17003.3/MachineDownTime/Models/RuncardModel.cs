using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineDownTime.Models
{
    /// <summary>
    /// 流程卡的資料模型。
    /// </summary>
    public class RuncardModel
    {
        public RuncardModel() { }

        public RuncardModel(RuncardModel e)
        {
            WORK_ORDER = e.WORK_ORDER;

            PART_NO = e.PART_NO;

            SPEC1 = e.SPEC1;

            OPTION2 = e.OPTION2;

            RC_NO = e.RC_NO;

            PROCESS_ID = e.PROCESS_ID;

            NODE_ID = e.NODE_ID;

            CURRENT_QTY = e.CURRENT_QTY;

            WIP_OUT_TIME = e.WIP_OUT_TIME;
        }

        public string WORK_ORDER { get; set; } = string.Empty;

        public string PART_NO { get; set; } = string.Empty;

        /// <summary>
        /// 品名
        /// </summary>
        public string SPEC1 { get; set; } = string.Empty;

        /// <summary>
        /// 舊編
        /// </summary>
        public string OPTION2 { get; set; } = string.Empty;

        /// <summary>
        /// 藍圖
        /// </summary>
        public string OPTION4 { get; set; } = string.Empty;

        public string RC_NO { get; set; } = string.Empty;

        public string PROCESS_ID { get; set; } = string.Empty;

        public string NODE_ID { get; set; } = string.Empty;

        public int CURRENT_QTY { get; set; } = 0;

        public DateTime? WIP_OUT_TIME { get; set; } = null;
    }
}
