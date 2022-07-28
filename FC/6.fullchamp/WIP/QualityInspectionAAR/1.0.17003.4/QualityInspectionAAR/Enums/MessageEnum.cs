using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QualityInspectionAAR.Enums
{
    /// <summary>
    /// 提示訊息
    /// </summary>
    public enum MessageEnum
    {
        /// <summary>
        /// 未知的流程卡號
        /// </summary>
        UnknownRCNO,
        /// <summary>
        /// 錯誤
        /// </summary>
        Error,
        /// <summary>
        /// 儲存資料錯誤
        /// </summary>
        SaveDataError,
        /// <summary>
        /// 超出範圍
        /// </summary>
        OutOfRange,
        /// <summary>
        /// 無效的資料
        /// </summary>
        InvalidData,
        /// <summary>
        /// 資料不可為空
        /// </summary>
        EmptyData,
        /// <summary>
        /// 沒有可訪問的資料
        /// </summary>
        NoAccessibleData,
    }
}
