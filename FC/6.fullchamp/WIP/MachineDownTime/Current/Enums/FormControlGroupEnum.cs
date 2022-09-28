namespace MachineDownTime.Enums
{
    /// <summary>
    /// 操作元件的群組
    /// </summary>
    internal enum FormControlGroupEnum
    {
        /// <summary>
        /// 流程卡資訊區塊
        /// </summary>
        RuncardInformation,
        /// <summary>
        /// 使用中機台區塊
        /// </summary>
        MachineInUse,
        /// <summary>
        /// 停機記錄區塊
        /// </summary>
        DownTimeLog,
        /// <summary>
        /// 已走過的製程
        /// </summary>
        PastProcess,
    }
}
