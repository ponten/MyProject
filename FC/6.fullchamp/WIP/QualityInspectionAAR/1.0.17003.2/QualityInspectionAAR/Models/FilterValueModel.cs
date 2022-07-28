using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QualityInspectionAAR.Models
{
    /// <summary>
    /// 過濾條件
    /// </summary>
    public class FilterValueModel
    {
        public int ProcessID { get; set; } = 0;

        public string ProcessName { get; set; } = string.Empty;

        public string WorkOrder { get; set; } = string.Empty;

        public string Runcard { get; set; } = string.Empty;
    }
}
