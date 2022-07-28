using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QualityInspectionAAR.Enums
{
    /// <summary>
    /// 操作類型
    /// </summary>
    public enum OperationTypeEnum
    {
        /// <summary>
        /// 新增
        /// </summary>
        CREATE,
        /// <summary>
        /// 讀取
        /// </summary>
        READ,
        /// <summary>
        /// 修改
        /// </summary>
        UPDATE,
        /// <summary>
        /// 刪除（不是真的刪除）
        /// </summary>
        DELETE,
    }
}
