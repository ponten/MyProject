namespace BCReprintDll.Enums
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
        /// 投入時間設定的比上一個製程的產出時間早
        /// </summary>
        TheInProcessTimeIsSetEarlierThanTheOutPutTimeOfPreviousProcess,
        /// <summary>
        /// 投入時間設定的比最後的更新時間早
        /// </summary>
        TheInProcessTimeIsSetEarlierThanTheLastUpdateTime,
        /// <summary>
        /// 投入時間必須比現在早
        /// </summary>
        TheInProcessTimeMustBeEarlierThanNow,
        /// <summary>
        /// 流程卡
        /// </summary>
        Runcard,
        /// <summary>
        /// 列印標籤嗎？
        /// </summary>
        PrintTheLabels,
    }
}
