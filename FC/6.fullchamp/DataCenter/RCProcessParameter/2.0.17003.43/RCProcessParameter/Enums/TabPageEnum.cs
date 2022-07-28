using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCProcessParam.Enums
{
    /// <summary>
    /// 正在操左哪一個頁籤
    /// </summary>
    internal enum TabPageEnum
    {
        /// <summary>
        /// 無
        /// </summary>
        None,
        /// <summary>
        /// 製程條件
        /// </summary>
        ProcessParameter,
        /// <summary>
        /// 資料收集
        /// </summary>
        CollectItem,
        /// <summary>
        /// 抽驗計畫
        /// </summary>
        SamplingPlan,
        /// <summary>
        /// 檢驗項目
        /// </summary>
        InspectItem,
        /// <summary>
        /// 檢驗設定
        /// </summary>
        InspectionSetting,
    }
}
