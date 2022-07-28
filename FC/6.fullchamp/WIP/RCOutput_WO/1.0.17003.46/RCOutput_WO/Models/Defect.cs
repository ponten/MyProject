using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCOutput_WO.Models
{
    /// <summary>
    /// 不良品的資料模型
    /// </summary>
    public class Defect
    {
        public Defect() { }

        public Defect(Defect m)
        {
            DEFECT_CODE = m.DEFECT_CODE;

            DEFECT_DESC = m.DEFECT_DESC;

            QTY = m.QTY;
        }

        /// <summary>
        /// 不良代碼
        /// </summary>
        public string DEFECT_CODE { get; set; }

        /// <summary>
        /// 不良現象描述
        /// </summary>
        public string DEFECT_DESC { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        public int QTY { get; set; }
    }
}
