using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCIPQC.Enums
{
    /// <summary>
    /// 提示訊息
    /// </summary>
    public enum MessageEnum
    {
        /// <summary>
        /// 設定的產出時間比在上一個製程的產出時間早
        /// </summary>
        TheOutProcessTimeIsSetEarlierThanTheOutPutTimeOfPreviousProcess,
        /// <summary>
        /// 設定的產出時間比投入時間早
        /// </summary>
        TheOutProcessTimeIsSetEarlierThanTheInProcessTime,
        /// <summary>
        /// 設定的產出時間比更新時間早
        /// </summary>
        TheOutProcessTimeIsSetEarlierThanTheLastUpdateTime,
        /// <summary>
        /// 產出時間必須比現在時間早
        /// </summary>
        TheOutProcessTimeMustBeEarlierThanNow,
        /// <summary>
        /// 機台的開始時間必須比投入時間晚
        /// </summary>
        TheStartTimeOfTheMachineMustBeLaterThanTheInProcessTime,
        /// <summary>
        /// 機台的開始時間必須比現在時間早
        /// </summary>
        TheStartTimeOfTheMachineMustBeEarlierThanNow,
        /// <summary>
        /// 未選取任何機台
        /// </summary>
        NoMachineSelected,
        /// <summary>
        /// 機台已在使用中
        /// </summary>
        MachineAlreadyInUse,
        /// <summary>
        /// 機台的負載量必須為非負數字
        /// </summary>
        TheLoadOfTheMachineMustBeANonNegativeNumber,
        /// <summary>
        /// 日期碼的格式不正確
        /// </summary>
        InvalidDateCodeFormat,
        /// <summary>
        /// 必須為 6 碼或 8 碼日期時間格式
        /// </summary>
        Require6or8DigitsDatetimeFormat,
        /// <summary>
        /// 確認新增機台
        /// </summary>
        ConfirmAddMAchine,
        /// <summary>
        /// 沒有可選項目
        /// </summary>
        NoOption,
        /// <summary>
        /// 流程卡走的生產途程中沒有這個製程
        /// </summary>
        TheProductionRouteOfTheRuncardDoesNotHaveThisProcess,
    }
}
