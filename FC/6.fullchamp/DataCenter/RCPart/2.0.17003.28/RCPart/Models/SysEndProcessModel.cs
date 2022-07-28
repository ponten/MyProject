using RCPart.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCPart.Models
{
    /// <summary>
    /// 生產途程節點（製程）的資訊
    /// </summary>
    public class SysEndProcessModel
    {
        public SysEndProcessModel() { }

        public SysEndProcessModel(SysEndProcessModel e)
        {
            ROUTE_ID = e.ROUTE_ID;

            ROUTE_NAME = e.ROUTE_NAME;

            ROUTE_DESC = e.ROUTE_DESC;

            NODE_ID = e.NODE_ID;

            PROCESS_ID = e.PROCESS_ID;

            PROCESS_CODE = e.PROCESS_CODE;

            PROCESS_NAME = e.PROCESS_NAME;

            ENABLED = e.ENABLED;

            UpdateType = e.UpdateType;
        }

        /// <summary>
        /// 生產途程 ID
        /// </summary>
        public string ROUTE_ID { get; set; } = string.Empty;

        /// <summary>
        /// 生產途程名稱
        /// </summary>
        public string ROUTE_NAME { get; set; } = string.Empty;

        /// <summary>
        /// 生產途程說明
        /// </summary>
        public string ROUTE_DESC { get; set; } = string.Empty;

        /// <summary>
        /// 節點 ID
        /// </summary>
        public string NODE_ID { get; set; } = "0";

        /// <summary>
        /// 製程 ID
        /// </summary>
        public string PROCESS_ID { get; set; } = "0";

        public string PROCESS_CODE { get; set; } = string.Empty;

        public string PROCESS_NAME { get; set; } = string.Empty;

        /// <summary>
        /// 資料啟用狀態
        /// </summary>
        public string ENABLED { get; set; } = "Y";

        /// <summary>
        /// 資料更新類型
        /// </summary>
        public UpdateType UpdateType { get; set; } = UpdateType.None;
    }
}
