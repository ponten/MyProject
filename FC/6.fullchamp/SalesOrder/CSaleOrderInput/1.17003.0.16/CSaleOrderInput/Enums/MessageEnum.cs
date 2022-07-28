using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSaleOrderInput.Enums
{
    /// <summary>
    /// 提示訊息
    /// </summary>
    public enum MessageEnum
    {
        /// <summary>
        /// 序號最多 3 碼
        /// </summary>
        Sequence3DigitsMost,
        /// <summary>
        /// 當月流水碼最多 4 碼
        /// </summary>
        SerialNumber4DigitsMost,
        /// <summary>
        /// 年月號碼必須為 6 碼，日期格式
        /// </summary>
        Require6DigitsYearMonthNumber,
    }
}
