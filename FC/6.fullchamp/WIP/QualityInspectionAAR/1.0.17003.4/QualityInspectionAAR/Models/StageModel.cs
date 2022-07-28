using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QualityInspectionAAR.Models
{
    /// <summary>
    /// 區段的資料模型。
    /// </summary>
    public class StageModel : ComboBoxItemModel
    {
        /// <summary>
        /// 製程的資料模型
        /// </summary>
        public List<ProcessModel> ProcessModel { get; set; }
    }
}
