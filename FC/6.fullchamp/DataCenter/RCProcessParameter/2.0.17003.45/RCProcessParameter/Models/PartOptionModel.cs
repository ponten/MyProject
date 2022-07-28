using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCProcessParam.Models
{
    /// <summary>
    /// 物料製程設定
    /// </summary>
    class PartOptionModel
    {
        /// <summary>
        /// 料號 ID
        /// </summary>
        public string part_id { get; set; } = string.Empty;

        /// <summary>
        /// 生產途程 ID
        /// </summary>
        public string route_id { get; set; } = string.Empty;

        /// <summary>
        /// 生產途程節點 ID
        /// </summary>
        public string node_id { get; set; } = string.Empty;

        /// <summary>
        /// 製程 ID
        /// </summary>
        public string process_id { get; set; } = string.Empty;

        /// <summary>
        /// 標準工時（分鐘）
        /// </summary>
        public string option1 { get; set; } = string.Empty;

        public string option2 { get; set; } = string.Empty;

        public string option3 { get; set; } = string.Empty;

        public string option4 { get; set; } = string.Empty;

        public string option5 { get; set; } = string.Empty;

        public string option6 { get; set; } = string.Empty;

        public string option7 { get; set; } = string.Empty;

        public string option8 { get; set; } = string.Empty;

        public string option9 { get; set; } = string.Empty;

        public string option10 { get; set; } = string.Empty;

        /// <summary>
        /// 更新資料的人員 ID
        /// </summary>
        public string update_userid { get; set; } = string.Empty;

        /// <summary>
        /// 更新資料的時間
        /// </summary>
        public DateTime update_time { get; set; }
    }
}
