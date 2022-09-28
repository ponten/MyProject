using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCOutput.Enums
{
    /// <summary>
    /// 品保抽驗設定
    /// </summary>
    [Flags]
    internal enum QCOptionEnum
    {
        /// <summary>
        /// 無特殊設定
        /// </summary>
        None = 0b_0000_0000,
        /// <summary>
        /// 首件檢
        /// </summary>
        FirstPieceInspection = 0b_0000_0001,
        /// <summary>
        /// 末件檢
        /// </summary>
        LastPieceInspection = 0b_0000_0010,
    }
}
