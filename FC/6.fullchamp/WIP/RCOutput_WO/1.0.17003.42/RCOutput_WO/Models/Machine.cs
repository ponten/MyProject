using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace RCOutput_WO.Models
{
    /// <summary>
    /// 機台的資料模型
    /// </summary>
    public class MachineDownModel
    {
        public MachineDownModel() { }

        public MachineDownModel(MachineDownModel e)
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

            STOVE_SEQ = e.STOVE_SEQ;

            REMARK = e.REMARK;
        }

        /// <summary>
        /// 是否被選用為加工機台
        /// </summary>
        public bool Select { get; set; } = false;

        /// <summary>
        /// 機台 ID
        /// </summary>
        public int MACHINE_ID { get; set; } = 0;

        /// <summary>
        /// 機台代碼
        /// </summary>
        public string MACHINE_CODE { get; set; } = string.Empty;

        /// <summary>
        /// 機台描述
        /// </summary>
        public string MACHINE_DESC { get; set; } = string.Empty;

        /// <summary>
        /// 機台狀態描述
        /// </summary>
        public string STATUS_NAME { get; set; } = string.Empty;

        /// <summary>
        /// 停機類型 ID
        /// </summary>
        public int TYPE_ID { get; set; } = 0;

        /// <summary>
        /// 停機代碼 ID
        /// </summary>
        public int REASON_ID { get; set; } = 0;

        /// <summary>
        /// 開始時間
        /// </summary>
        public DateTime START_TIME { get; set; } = DateTime.Now;

        /// <summary>
        /// 結束時間
        /// </summary>
        public DateTime END_TIME { get; set; } = DateTime.Now;

        /// <summary>
        /// 加工數量
        /// </summary>
        public int? LOAD_QTY { get; set; } = 0;

        /// <summary>
        /// 日期碼
        /// </summary>
        public string DATE_CODE { get; set; } = string.Empty;

        /// <summary>
        /// 爐序
        /// </summary>
        public string STOVE_SEQ { get; set; } = string.Empty;

        /// <summary>
        /// 備註
        /// </summary>
        public string REMARK { get; set; } = string.Empty;
    }

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
