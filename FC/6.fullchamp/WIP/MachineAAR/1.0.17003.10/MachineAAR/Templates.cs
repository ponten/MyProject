using System;
using System.Collections.Generic;

namespace MachineAAR
{
    public class FilterValueModel
    {
        public string ProcessName { get; set; } = string.Empty;

        public string WorkOrder { get; set; } = string.Empty;

        public string Runcard { get; set; } = string.Empty;
    }

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

    /// <summary>
    /// 流程卡的資料模型。
    /// </summary>
    public class RuncardModel
    {
        public RuncardModel() { }

        public RuncardModel(RuncardModel e)
        {
            WORK_ORDER = e.WORK_ORDER;

            PART_NO = e.PART_NO;

            SPEC1 = e.SPEC1;

            OPTION2 = e.OPTION2;

            RC_NO = e.RC_NO;

            PROCESS_ID = e.PROCESS_ID;

            NODE_ID = e.NODE_ID;

            CURRENT_QTY = e.CURRENT_QTY;

            WIP_OUT_TIME = e.WIP_OUT_TIME;
        }

        public string WORK_ORDER { get; set; } = string.Empty;

        public string PART_NO { get; set; } = string.Empty;

        /// <summary>
        /// 品名
        /// </summary>
        public string SPEC1 { get; set; } = string.Empty;

        /// <summary>
        /// 舊編
        /// </summary>
        public string OPTION2 { get; set; } = string.Empty;

        /// <summary>
        /// 藍圖
        /// </summary>
        public string OPTION4 { get; set; } = string.Empty;

        public string RC_NO { get; set; } = string.Empty;

        public string PROCESS_ID { get; set; } = string.Empty;

        public string NODE_ID { get; set; } = string.Empty;

        public int CURRENT_QTY { get; set; } = 0;

        public DateTime? WIP_OUT_TIME { get; set; } = null;
    }

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

    /// <summary>
    /// 製程的資料模型
    /// </summary>
    public class ProcessModel : ComboBoxItemModel
    {
        public bool T4 { get; set; } = false;

        public bool T6 { get; set; } = false;
    }

    /// <summary>
    /// 下拉選單的資料模型。
    /// </summary>
    public class ComboBoxItemModel
    {
        public string ID { get; set; }

        public string CODE { get; set; }

        public string NAME { get; set; }
    }
}
