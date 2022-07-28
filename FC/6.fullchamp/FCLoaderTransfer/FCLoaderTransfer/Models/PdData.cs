using System;

namespace Models
{
    class PdData
    {
        public decimal DATARECORD_ID { get; set; }

        public string DATARECORD_MSG { get; set; }

        public decimal? MONITOR_ID { get; set; }

        public DateTime CREATE_DATETIME { get; set; }

        public decimal? CREATE_USER_ID { get; set; }
    }
}
