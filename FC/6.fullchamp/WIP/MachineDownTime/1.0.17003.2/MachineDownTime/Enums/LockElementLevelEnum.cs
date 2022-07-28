namespace MachineDownTime.Enums
{
    /// <summary>
    /// 鎖定操作畫面元件的等級
    /// </summary>
    internal enum LockElementLevelEnum
    {
        /// <summary>
        /// 開放到收集停機資料的區塊
        /// </summary>
        DataGroupBox,
        /// <summary>
        /// 開放到流程卡號輸入框
        /// </summary>
        RuncardTextBox,
        /// <summary>
        /// 僅開放員工號輸入框
        /// </summary>
        EmpTextBox,
    }
}
