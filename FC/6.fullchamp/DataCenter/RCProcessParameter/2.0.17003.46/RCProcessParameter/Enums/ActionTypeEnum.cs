using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCProcessParam.Enums
{
    /// <summary>
    /// 操作行為的類型
    /// </summary>
    internal enum ActionTypeEnum
    {
        /// <summary>
        /// 無
        /// </summary>
        None,
        /// <summary>
        /// 新增
        /// </summary>
        Create,
        /// <summary>
        /// 修改
        /// </summary>
        Update,
        /// <summary>
        /// 讀取
        /// </summary>
        Read,
        /// <summary>
        /// 刪除（非真的刪除）
        /// </summary>
        Delete,
    }
}
