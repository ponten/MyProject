using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCOutput.Models
{
    public class T4MachineModel
    {
        /// <summary>
        /// CheckBox 的勾選狀態
        /// </summary>
        public bool CheckState { get; set; } = false;

        /// <summary>
        /// 10C 流程卡的流程卡號
        /// </summary>
        public string RC_NO_10C { get; set; } = string.Empty;

        /// <summary>
        /// 10C 流程卡的製程 ID
        /// </summary>
        public int PROCESS_ID_10C { get; set; } = 0;

        /// <summary>
        /// T4 的流程卡的流程卡號
        /// </summary>
        public string RC_NO_T4 { get; set; } = string.Empty;

        /// <summary>
        /// T4 的流程卡產出時的數量（良品數）
        /// </summary>
        public int CURRENT_QTY_T4 { get; set; } = 0;

        /// <summary>
        /// T4 的流程卡的製程 ID
        /// </summary>
        public int PROCESS_ID_T4 { get; set; } = 0;

        /// <summary>
        /// 製程代碼
        /// </summary>
        public string PROCESS_CODE { get; set; } = string.Empty;

        /// <summary>
        /// 製程名稱
        /// </summary>
        public string PROCESS_NAME { get; set; } = string.Empty;

        /// <summary>
        /// 機台 ID
        /// </summary>
        public int MACHINE_ID { get; set; } = 0;

        /// <summary>
        /// 機台代碼
        /// </summary>
        public string MACHINE_CODE { get; set; } = string.Empty;

        /// <summary>
        /// 機台名稱
        /// </summary>
        public string MACHINE_DESC { get; set; } = string.Empty;

        /// <summary>
        /// T4 設定的機台的加工數量
        /// </summary>
        public int LOAD { get; set; } = 0;

        /// <summary>
        /// T4 設定的機台的日期碼
        /// </summary>
        public string DATE_CODE { get; set; } = string.Empty;

        /// <summary>
        /// T4 設定的機台的爐序號
        /// </summary>
        public string STOVE_SEQ { get; set; } = string.Empty;

        /// <summary>
        /// T4 機台已經被其他 10C 流程卡綁定的數量
        /// </summary>
        public int BOUND_QTY { get; set; } = 0;

        /// <summary>
        /// 綁定的數量（可編輯欄位）
        /// </summary>
        public int? BINDING_QTY { get; set; } = 0;
    }
}
