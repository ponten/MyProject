using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CProcess.Enums
{
    /// <summary>
    /// 製程特殊設定
    /// </summary>
    internal enum ProcessOptionEnum
    {
        /// <summary>
        /// 無
        /// </summary>
        None,
        /// <summary>
        /// 列印防水標籤
        /// </summary>
        PrintWPLabel,
        /// <summary>
        /// 使用 T4 爐
        /// </summary>
        UseT4Stove,
        /// <summary>
        /// 使用 T6 爐
        /// </summary>
        UseT6Stove,
        /// <summary>
        /// 10C 投入製程
        /// </summary>
        Start10C,
        /// <summary>
        /// 10C 入庫製程
        /// </summary>
        Warehouse10C,
        /// <summary>
        /// 設備使用（機台類型）
        /// </summary>
        UsingEquipment,
        /// <summary>
        /// 生產單位（部門）
        /// </summary>
        ProductionUnit,
    }
}
