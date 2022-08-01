using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineDownTime.Models
{
    /// <summary>
    /// 製程機台的資料模型
    /// </summary>
    public class MachineModel
    {
        public MachineModel() { }

        public MachineModel(MachineModel e)
        {
            Select = e.Select;

            MACHINE_ID = e.MACHINE_ID;

            MACHINE_CODE = e.MACHINE_CODE;

            MACHINE_DESC = e.MACHINE_DESC;

            STATUS_NAME = e.STATUS_NAME;

            TYPE_ID = e.TYPE_ID;

            REASON_ID = e.REASON_ID;

            START_TIME = e.START_TIME;

            END_TIME = e.END_TIME;

            LOAD_QTY = e.LOAD_QTY;

            DATE_CODE = e.DATE_CODE;

            REMARK = e.REMARK;
        }

        public bool Select { get; set; } = false;

        public int MACHINE_ID { get; set; } = 0;

        public string MACHINE_CODE { get; set; } = string.Empty;

        public string MACHINE_DESC { get; set; } = string.Empty;

        public string STATUS_NAME { get; set; } = string.Empty;

        public int TYPE_ID { get; set; } = 0;

        public int REASON_ID { get; set; } = 0;

        public DateTime? START_TIME { get; set; } = null;

        public DateTime? END_TIME { get; set; } = null;

        public int LOAD_QTY { get; set; } = 0;

        public string DATE_CODE { get; set; } = string.Empty;

        public string REMARK { get; set; } = string.Empty;
    }
}
