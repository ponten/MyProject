using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCInput.Enums
{
    /// <summary>
    /// 訊息、文字資源
    /// </summary>
    public enum MessageEnum
    {
        /// <summary>
        /// 投入時間設定比上一個製程的產出時間早
        /// </summary>
        TheInProcessTimeIsSetEarlierThanTheOutPutTimeOfPreviousProcess,
        /// <summary>
        /// 投入時間設定比最後的更新時間早
        /// </summary>
        TheInProcessTimeIsSetEarlierThanTheLastUpdateTime,
        /// <summary>
        /// 投入時間必須比現在早
        /// </summary>
        TheInProcessTimeMustBeEarlierThanNow,
        /// <summary>
        /// 找不到機台
        /// </summary>
        MachineNotFound,
    }
}
