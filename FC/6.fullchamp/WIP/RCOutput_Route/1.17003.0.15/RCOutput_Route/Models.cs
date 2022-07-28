using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCOutput_Route.Models
{
    public class Defect
    {
        public string DEFECT_CODE { get; set; }

        public string DEFECT_DESC { get; set; }

        public int QTY { get; set; }
    }

    public class RC_DEFECT
    {
        public string RC_NO { get; set; }

        public int CURRENT_QTY { get; set; }

        public List<Defect> DEFECTS { get; set; }
    }
}
