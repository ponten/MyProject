namespace MachineDownTime.Enums
{
    /// <summary>
    /// 提示訊息
    /// </summary>
    internal enum MessageEnum
    {
        /// <summary>
        /// 找不到流程卡資料
        /// </summary>
        RuncardNotFound,
        /// <summary>
        /// 找不到員工資料
        /// </summary>
        EmpNotFound,
        /// <summary>
        /// 找不到加工機台資料，或者未使用到機台
        /// </summary>
        MachineNotFoundOrNoMachineInUse,
        /// <summary>
        /// 使用者不能給此製程的流程卡報工
        /// </summary>
        NoAccessToProcess,
        /// <summary>
        /// 流程卡已經離開這個製程
        /// </summary>
        RuncardHasLeftThisProcess,
        /// <summary>
        /// 停機代碼為空
        /// </summary>
        DownCodeEmpty,
        /// <summary>
        /// 停機的開始時間筆流程卡投入時間早
        /// </summary>
        StartTimeIsEarlierThanInProcessTime,
        /// <summary>
        /// 停機結束時間筆開始時間早
        /// </summary>
        EndTimeIsEarlierThanStartTime,
        /// <summary>
        /// 結束時間應該要比現在時間早
        /// </summary>
        EndTimeShouldBeEarlierThanNow,
        /// <summary>
        /// 停機時間區段重疊
        /// </summary>
        MachineDownTimeSpanOverlapping,
        /// <summary>
        /// 檢查時間段發生錯誤
        /// </summary>
        TimeSpanCheckError,
        /// <summary>
        /// 流程卡不是「生產中」的狀態
        /// </summary>
        RuncardNotRunning,
    }
}
