using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCOutput_WO.Models
{
    /* 資料收集的查詢欄位
SELECT
    A.ITEM_NAME
   ,A.VALUE_DEFAULT
   ,A.VALUE_TYPE
   ,A.INPUT_TYPE
   ,A.NECESSARY
   ,A.CONVERT_TYPE
   ,A.VALUE_LIST
   ,A.ITEM_ID
   ,B.UNIT_NO
FROM
    SAJET.SYS_RC_PROCESS_PARAM_PART A
   ,SAJET.SYS_UNIT B
WHERE
    A.PART_ID = :PART_ID
    AND A.PROCESS_ID = :PROCESS_ID
    AND A.ITEM_PHASE IN ('A', 'O')
    AND A.ITEM_TYPE = 1
    AND A.ENABLED = 'Y'
    AND A.UNIT_ID = B.UNIT_ID(+)
ORDER BY
    ITEM_SEQ
     */

    /// <summary>
    /// 資料收集的資料模型
    /// </summary>
    public class DataCollectModel
    {
        public DataCollectModel() { }

        public DataCollectModel(DataCollectModel e)
        {
            ITEM_ID = e.ITEM_ID;
            ITEM_NAME = e.ITEM_NAME;
            VALUE_DEFAULT = e.VALUE_DEFAULT;
            INPUT_VALUE = e.INPUT_VALUE;
            VALUE_TYPE = e.VALUE_TYPE;
            INPUT_TYPE = e.INPUT_TYPE;
            NECESSARY = e.NECESSARY;
            CONVERT_TYPE = e.CONVERT_TYPE;
            VALUE_LIST = e.VALUE_LIST;
            UNIT_NO = e.UNIT_NO;
        }

        /// <summary>
        /// 項目名稱
        /// </summary>
        public string ITEM_NAME { get; set; } = string.Empty;

        /// <summary>
        /// 項目預設值
        /// </summary>
        public string VALUE_DEFAULT { get; set; } = string.Empty;

        /// <summary>
        /// 輸入值，與項目預設值（VALUE_DEFAULT）作區別
        /// </summary>
        public string INPUT_VALUE { get; set; } = string.Empty;

        /// <summary>
        /// 錯誤訊息，輸入值合法的時候為空字串
        /// </summary>
        public string ERROR_TEXT { get; set; } = string.Empty;

        /// <summary>
        /// 輸入值類型
        /// </summary>
        public string VALUE_TYPE { get; set; } = string.Empty;

        /// <summary>
        /// 輸入類型
        /// </summary>
        public string INPUT_TYPE { get; set; } = string.Empty;

        /// <summary>
        /// 必要欄位
        /// </summary>
        public string NECESSARY { get; set; } = string.Empty;

        /// <summary>
        /// 輸入值轉換
        /// </summary>
        public string CONVERT_TYPE { get; set; } = string.Empty;

        /// <summary>
        /// 選項清單
        /// </summary>
        public string VALUE_LIST { get; set; } = string.Empty;

        /// <summary>
        /// 項次
        /// </summary>
        public string ITEM_ID { get; set; } = string.Empty;

        /// <summary>
        /// 單位
        /// </summary>
        public string UNIT_NO { get; set; } = string.Empty;
    }
}
