namespace Print_RCList.Enums
{
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
        /// 排在最後一個、現在沒有在使用的參數
        /// </summary>
        PAGE,
    }
}
