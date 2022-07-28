namespace RCInput_WO.Enums
{
    /// <summary>
    /// 列印防水標籤的輸出欄位
    /// </summary>
    public enum LabelHeaderEnum
    {
        /// <summary>
        /// 工單號碼，純顯示資訊用。由流程卡改成 '#' 顯示格式而來
        /// </summary>
        WORK_ORDER,
        /// <summary>
        /// 開本數（WO_OPTION2）
        /// </summary>
        BOOK_NO,
        /// <summary>
        /// 製別
        /// </summary>
        CATEGORY,
        /// <summary>
        /// 料號。優先顯示舊編
        /// </summary>
        PART_NO,
        /// <summary>
        /// 規格。包含品名、規格
        /// </summary>
        SPEC,
        /// <summary>
        /// 熱處理爐號
        /// </summary>
        STOVE,
        /// <summary>
        /// 流程卡號。QR CODE 的資料來源，不能改格式找不到
        /// </summary>
        RC_NO,
        /// <summary>
        /// 流程卡的當前數量
        /// </summary>
        QTY,
        /// <summary>
        /// 旋型製程產出良品數量
        /// </summary>
        GOOD_QTY,
        /// <summary>
        /// 旋型製程產出不良品數量
        /// </summary>
        SCRAP_QTY,
        /// <summary>
        /// 旋型產出製程時間
        /// </summary>
        OUT_PROCESS_TIME,
        /// <summary>
        /// 作業員名稱
        /// </summary>
        EMP,
    }

    /// <summary>
    /// 標籤列印內容，按照查詢區分成資料組
    /// </summary>
    public enum LabelDataGroupEnum
    {
        /// <summary>
        /// 工單資料組
        /// </summary>
        WORK_ORDER,
        /// <summary>
        /// 流程卡資料組
        /// </summary>
        RC_NO,
        /// <summary>
        /// 旋型資料組
        /// </summary>
        B050,
        /// <summary>
        /// 固溶處理資料組
        /// </summary>
        B060,
        /// <summary>
        /// 時效處理資料組
        /// </summary>
        B070,
    }
}
