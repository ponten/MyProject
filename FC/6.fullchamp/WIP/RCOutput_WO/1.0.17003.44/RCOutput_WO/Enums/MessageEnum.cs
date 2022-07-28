using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCOutput_WO.Enums
{
    public enum MessageEnum
    {
        /// <summary>
        /// 有使用到 T4 爐
        /// </summary>
        HasUseT4,
        /// <summary>
        /// 有使用到 T6 爐
        /// </summary>
        HasUseT6,
        /// <summary>
        /// 使用 T4 爐 / T6 爐 的製程，只能使用一個機台
        /// </summary>
        T4orT6OneMachineOnly,
        /// <summary>
        /// 產出時間設定的比上一個製程的產出時間早
        /// </summary>
        TheOutProcessTimeIsSetEarlierThanTheOutPutTimeOfPreviousProcess,
        /// <summary>
        /// 產出時間設定的比投入時間早
        /// </summary>
        TheOutProcessTimeIsSetEarlierThanTheInProcessTime,
        /// <summary>
        /// 產出時間設定的比最後的更新時間早
        /// </summary>
        TheOutProcessTimeIsSetEarlierThanTheLastUpdateTime,
        /// <summary>
        /// 產出時間必須比現在時間早
        /// </summary>
        TheOutProcessTimeMustBeEarlierThanNow,
        /// <summary>
        /// 流程卡
        /// </summary>
        Runcard,
        /// <summary>
        /// 請輸入數字
        /// </summary>
        PleaseKeyInNumbers,
        /// <summary>
        /// 勾選數量
        /// </summary>
        CheckQuantity,
    }
}
