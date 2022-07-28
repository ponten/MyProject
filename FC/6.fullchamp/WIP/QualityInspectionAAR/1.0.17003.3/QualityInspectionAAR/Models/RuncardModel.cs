namespace QualityInspectionAAR.Models
{
    /// <summary>
    /// 流程卡的資料模型。
    /// </summary>
    public class RuncardModel
    {
        public RuncardModel() { }

        public RuncardModel(RuncardModel e)
        {
            WORK_ORDER = e.WORK_ORDER;

            RC_NO = e.RC_NO;

            PART_ID = e.PART_ID;

            PART_NO = e.PART_NO;

            FORMER_PART_NO = e.FORMER_PART_NO;

            SPEC = e.SPEC;

            CURRENT_QTY = e.CURRENT_QTY;

            COLLECTED_QTY = e.COLLECTED_QTY;

            STAGE_ID = e.STAGE_ID;

            PROCESS_ID = e.PROCESS_ID;

            NODE_ID = e.NODE_ID;

            STAGE_CODE = e.STAGE_CODE;

            STAGE_NAME = e.STAGE_NAME;

            PROCESS_CODE = e.PROCESS_CODE;

            PROCESS_NAME = e.PROCESS_NAME;
        }

        /// <summary>
        /// 工單號碼
        /// </summary>
        public string WORK_ORDER { get; set; } = string.Empty;

        /// <summary>
        /// 流程卡號
        /// </summary>
        public string RC_NO { get; set; } = string.Empty;

        /// <summary>
        /// 料號 ID
        /// </summary>
        public string PART_ID { get; set; }

        /// <summary>
        /// 料號
        /// </summary>
        public string PART_NO { get; set; } = string.Empty;

        /// <summary>
        /// 舊編
        /// </summary>
        public string FORMER_PART_NO { get; set; } = string.Empty;

        /// <summary>
        /// 品名
        /// </summary>
        public string SPEC { get; set; } = string.Empty;

        /// <summary>
        /// 流程卡當前數量
        /// </summary>
        public int CURRENT_QTY { get; set; } = 0;

        /// <summary>
        /// 已經收集品保資料的在製品筆數
        /// </summary>
        public int COLLECTED_QTY { get; set; } = 0;

        /// <summary>
        /// 區段 ID
        /// </summary>
        public string STAGE_ID { get; set; } = string.Empty;

        /// <summary>
        /// 製程 ID
        /// </summary>
        public string PROCESS_ID { get; set; } = string.Empty;

        /// <summary>
        /// 生產途程節點 ID
        /// </summary>
        public string NODE_ID { get; set; } = string.Empty;

        /// <summary>
        /// 區段代碼
        /// </summary>
        public string STAGE_CODE { get; set; } = string.Empty;

        /// <summary>
        /// 區段名稱
        /// </summary>
        public string STAGE_NAME { get; set; } = string.Empty;

        /// <summary>
        /// 製程代碼
        /// </summary>
        public string PROCESS_CODE { get; set; } = string.Empty;

        /// <summary>
        /// 製程名稱
        /// </summary>
        public string PROCESS_NAME { get; set; } = string.Empty;
    }
}
