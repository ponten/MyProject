namespace RCOutput.Enums
{
    /// <summary>
    /// 提示訊息
    /// </summary>
    public enum MessageEnum
    {
        /// <summary>
        /// 不是子件料號，不能綁定
        /// </summary>
        IsNotChildBOM,
        /// <summary>
        /// 流程卡還沒跑完生產途程
        /// </summary>
        RuncardNotComplete,
        /// <summary>
        /// 流程卡的生產途程中沒有設定 T4 的製程
        /// </summary>
        NoT4ProcessInRoute,
        /// <summary>
        /// 重複刷入的流程卡
        /// </summary>
        DuplicateRuncard,
        /// <summary>
        /// 流程卡的 T4 機台加工數量已經被綁定完，沒辦法再綁定
        /// </summary>
        NoT4LoadCanBeBound,
        /// <summary>
        /// 綁定數量超過機台加工數量
        /// </summary>
        BindingQtyMoreThanT4Load,
        /// <summary>
        /// 綁定數量必須等於當前數量
        /// </summary>
        BindingQtyShouldMatchCurrentQty,
        /// <summary>
        /// 10C 投入站必須綁定 T4 機台加工數量才能執行產出
        /// </summary>
        MustBindT4To10C,
        /// <summary>
        /// 產出時間必須比投入時間晚
        /// </summary>
        TheOutProcessTimeMustBeLaterThanTheInProcessTime,
        /// <summary>
        /// 換班時間必須比投入時間晚
        /// </summary>
        TheShiftTimeMustBeLaterThanTheInProcessTime,
        /// <summary>
        /// 換班時間必須比產出時間早
        /// </summary>
        TheShiftTimeMustBeEarlierThanTheOutProcessTime,
        /// <summary>
        /// 投入時間
        /// </summary>
        InProcessTime,
        /// <summary>
        /// 產出時間
        /// </summary>
        OutProcessTime,
        /// <summary>
        /// 換班工作數量必須是非負整數
        /// </summary>
        TheShiftWorkloadMustBeANonNegativeInteger,
        /// <summary>
        /// 換班工作數量必須不超過流程卡的當前數量
        /// </summary>
        TheShiftWorkloadMustNotExceedTheCurrentQuantityOfTheRuncard,
        /// <summary>
        /// 換班工作數量不能超過機台加工數量
        /// </summary>
        TheShiftWorkloadMustNotExceedTheMachineLoad,
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
        /// 產出時間必須比現在早
        /// </summary>
        TheOutProcessTimeMustBeEarlierThanNow,
        /// <summary>
        /// 移除
        /// </summary>
        Remove,
        /// <summary>
        /// 沒換班所以不能交接
        /// </summary>
        CanNotTakeOverWithoutShift,
        /// <summary>
        /// 製程未設定機台，不能用交接功能
        /// </summary>
        ThisProcessHasNoMachine,
        /// <summary>
        /// 機台多於一台，不能換班
        /// </summary>
        MachineMoreThanOneCanNotShift,
        /// <summary>
        /// 機台加工數量必須為非負整數
        /// </summary>
        TheMachineWorkloadMustBeANonNegativeInteger,
        /// <summary>
        /// 機台加工數量超過流程卡當前數量
        /// </summary>
        TheMachineloadExceededCurrentQuantity,
        /// <summary>
        /// 總加工數量已超過流程卡的當前數量
        /// </summary>
        TheTotalProcessingQtyHasExceededTheCurrentQtyOfTheRuncard,
        /// <summary>
        /// 總加工數量等於流程卡的當前數量，請直接產出此流程卡
        /// </summary>
        TheTotalProcessingQtyIsEqualToTheCurrentQtyOfTheRuncard,
        /// <summary>
        /// 已加工總數量
        /// </summary>
        TotalLoadBefore,
        /// <summary>
        /// 請設定機台加工數量
        /// </summary>
        PleaseSetMachineWorkload,
        /// <summary>
        /// 請記錄作業人數
        /// </summary>
        PleaseLogOperatorNumber,
    }
}
