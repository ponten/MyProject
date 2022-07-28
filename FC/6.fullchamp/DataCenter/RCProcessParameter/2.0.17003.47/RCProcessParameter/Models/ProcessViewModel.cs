using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCProcessParam.Models
{
    public class ProcessViewModel
    {
        public ProcessViewModel() { }

        public ProcessViewModel(ProcessViewModel e)
        {
            PART_ID = e.PART_ID;

            PART_NO = e.PART_NO;

            ROUTE_ID = e.ROUTE_ID;

            PROCESS_ID = e.PROCESS_ID;

            PROCESS_CODE = e.PROCESS_CODE;

            PROCESS_NAME = e.PROCESS_NAME;

            NODE_ID = e.NODE_ID;
        }

        /// <summary>
        /// 料號 ID
        /// </summary>
        public string PART_ID { get; set; } = string.Empty;

        /// <summary>
        /// 料號
        /// </summary>
        public string PART_NO { get; set; } = string.Empty;

        /// <summary>
        /// 生產途程 ID
        /// </summary>
        public string ROUTE_ID { get; set; } = string.Empty;

        /// <summary>
        /// 製程 ID
        /// </summary>
        public string PROCESS_ID { get; set; } = string.Empty;

        /// <summary>
        /// 製程代碼
        /// </summary>
        public string PROCESS_CODE { get; set; } = string.Empty;

        /// <summary>
        /// 製程名稱
        /// </summary>
        public string PROCESS_NAME { get; set; } = string.Empty;

        /// <summary>
        /// 生產途程節點 ID
        /// </summary>
        public string NODE_ID { get; set; } = string.Empty;
    }
}
