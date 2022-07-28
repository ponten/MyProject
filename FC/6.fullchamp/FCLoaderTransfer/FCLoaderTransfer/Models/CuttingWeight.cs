using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    class CuttingWeight
    {
        public decimal? DATARECORD_ID { get; set; }

        public string WORK_ORDER { get; set; }

        public decimal? LINE { get; set; }

        public string STATUS_FLAG { get; set; }

        public decimal? WEIGHT { get; set; }

        public DateTime? DATETIME { get; set; }
    }
}
