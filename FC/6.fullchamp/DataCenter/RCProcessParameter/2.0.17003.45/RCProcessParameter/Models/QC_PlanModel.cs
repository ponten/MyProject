using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCProcessParam
{
    /// <summary>
    /// SAJET.SYS_PART_QC_PLAN
    /// </summary>
    class QC_PlanModel
    {
        /// <summary>
        /// 物料 ID
        /// </summary>
        public string Part_Id { get; set; }

        /// <summary>
        /// 製程 ID
        /// </summary>
        public string Process_Id { get; set; }

        /// <summary>
        /// 抽驗計畫 ID
        /// </summary>
        public string Sampling_Id { get; set; }

        /// <summary>
        /// 使用者 ID
        /// </summary>
        public string User_Id { get; set; }
    }
}
