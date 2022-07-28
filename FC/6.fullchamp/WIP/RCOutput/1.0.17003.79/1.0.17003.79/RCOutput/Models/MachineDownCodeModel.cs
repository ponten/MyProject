using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCOutput.Models
{
    /// <summary>
    /// 下拉選單用的基本資料模型。
    /// </summary>
    public class BaseModel
    {
        public string ID { get; set; }

        public string CODE { get; set; }

        public string DESC { get; set; }
    }

    /// <summary>
    /// 停機類型。
    /// </summary>
    public class DownType : BaseModel
    {
        public List<DownDetail> DownDetails { get; set; }
    }

    /// <summary>
    /// 停機代碼。
    /// </summary>
    public class DownDetail : BaseModel
    {
        public bool CountWorkTime { get; set; } = true;
    }
}
