using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QualityInspectionAAR.Models
{
    /// <summary>
    /// 製程的資料模型
    /// </summary>
    public class ProcessModel : ComboBoxItemModel
    {
        public string NODE_ID { get; set; } = string.Empty;
    }
}
