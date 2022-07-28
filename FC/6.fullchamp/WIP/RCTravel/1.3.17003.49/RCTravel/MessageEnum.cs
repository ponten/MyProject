using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCTravel
{
    public enum MessageEnum
    {
        /// <summary>
        /// 設定為共用機台資料的流程卡不能再使用流程卡產出模組
        /// </summary>
        RuncardsSharingMachineDataCanNotUseRCOutput,
        /// <summary>
        /// 查無流程卡資訊
        /// </summary>
        RuncardNotFound,
    }
}
