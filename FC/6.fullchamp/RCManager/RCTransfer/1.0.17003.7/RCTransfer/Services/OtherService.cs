using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;

namespace RCTransfer.Services
{
    /// <summary>
    /// 未分類的商業邏輯
    /// </summary>
    public static class OtherService
    {
        /// <summary>
        /// 取得資料庫現在時間
        /// </summary>
        /// <returns></returns>
        public static DateTime GetDBDateTimeNow()
        {
            string s = @"
SELECT
    SYSDATE
FROM
    DUAL
";
            var d = ClientUtils.ExecuteSQL(s);

            if (d != null &&
                d.Tables[0].Rows.Count > 0 &&
                DateTime.TryParse(d.Tables[0].Rows[0]["SYSDATE"].ToString(), out DateTime now))
            {
                return now;
            }
            else
            {
                return DateTime.Now;
            }

        }

        /// <summary>
        /// 寫入工單紀錄表
        /// </summary>
        /// <param name="work_order">工單號碼</param>
        public static void LOG_WO_BASE(string work_order)
        {
            string s = @"
INSERT INTO SAJET.G_HT_WO_BASE (
    WORK_ORDER,
    WO_TYPE,
    WO_RULE,
    PART_ID,
    VERSION,
    TARGET_QTY,
    WO_CREATE_DATE,
    WO_SCHEDULE_DATE,
    WO_START_DATE,
    WO_CLOSE_DATE,
    ROUTE_ID,
    INPUT_QTY,
    OUTPUT_QTY,
    WORK_FLAG,
    WO_STATUS,
    DEFAULT_PDLINE_ID,
    START_PROCESS_ID,
    END_PROCESS_ID,
    CUSTOMER_ID,
    PO_NO,
    MASTER_WO,
    REMARK,
    UPDATE_USERID,
    UPDATE_TIME,
    RELEASE_DATE,
    MODEL_NAME,
    WO_DUE_DATE,
    FACTORY_ID,
    CUST_RECNO,
    CUST_SNDNO,
    SALES_ORDER,
    BURNIN_TIME,
    SCRAP_QTY,
    ERP_QTY,
    WO_OPTION1,
    WO_OPTION2,
    WO_OPTION3,
    WO_OPTION4,
    WO_OPTION5,
    WO_OPTION6,
    WO_OPTION7,
    WO_OPTION8,
    WO_OPTION9,
    WO_OPTION10,
    ERP_ID,
    BOM_TYPE,
    ERROR_QTY,
    OPERATION_ID,
    DATENUMBER,
    NUMBER1,
    NUMBER2,
    MADE_CATEGORY,
    CUSTOMER1,
    CUSTOMER2,
    CUSTOMER3,
    SALES,
    CUSTOMIZE,
    PRODUCT_RATE,
    OUTSOURCE,
    REVIEW,
    CLOSED,
    WO_VOID,
    RULE1,
    RULE2,
    RULE3,
    CUSTOMER_DATE,
    CUSTOMER_DUE_DATE,
    REAL_DATE,
    REAL_DUE_DATE,
    PRODUCENO1,
    PRODUCENO2,
    OLD_NUMBER,
    UNIT,
    BLUEPRINT,
    PALLETS,
    TOTAL_NUMBER,
    FINISHED,
    CASECLOSED,
    DIFFERENCE,
    BAD,
    NOTPAID,
    PRODUCE_NUMBER
)
    SELECT
        WORK_ORDER,
        WO_TYPE,
        WO_RULE,
        PART_ID,
        VERSION,
        TARGET_QTY,
        WO_CREATE_DATE,
        WO_SCHEDULE_DATE,
        WO_START_DATE,
        WO_CLOSE_DATE,
        ROUTE_ID,
        INPUT_QTY,
        OUTPUT_QTY,
        WORK_FLAG,
        WO_STATUS,
        DEFAULT_PDLINE_ID,
        START_PROCESS_ID,
        END_PROCESS_ID,
        CUSTOMER_ID,
        PO_NO,
        MASTER_WO,
        REMARK,
        UPDATE_USERID,
        UPDATE_TIME,
        RELEASE_DATE,
        MODEL_NAME,
        WO_DUE_DATE,
        FACTORY_ID,
        CUST_RECNO,
        CUST_SNDNO,
        SALES_ORDER,
        BURNIN_TIME,
        SCRAP_QTY,
        ERP_QTY,
        WO_OPTION1,
        WO_OPTION2,
        WO_OPTION3,
        WO_OPTION4,
        WO_OPTION5,
        WO_OPTION6,
        WO_OPTION7,
        WO_OPTION8,
        WO_OPTION9,
        WO_OPTION10,
        ERP_ID,
        BOM_TYPE,
        ERROR_QTY,
        OPERATION_ID,
        DATENUMBER,
        NUMBER1,
        NUMBER2,
        MADE_CATEGORY,
        CUSTOMER1,
        CUSTOMER2,
        CUSTOMER3,
        SALES,
        CUSTOMIZE,
        PRODUCT_RATE,
        OUTSOURCE,
        REVIEW,
        CLOSED,
        WO_VOID,
        RULE1,
        RULE2,
        RULE3,
        CUSTOMER_DATE,
        CUSTOMER_DUE_DATE,
        REAL_DATE,
        REAL_DUE_DATE,
        PRODUCENO1,
        PRODUCENO2,
        OLD_NUMBER,
        UNIT,
        BLUEPRINT,
        PALLETS,
        TOTAL_NUMBER,
        FINISHED,
        CASECLOSED,
        DIFFERENCE,
        BAD,
        NOTPAID,
        PRODUCE_NUMBER
    FROM
        SAJET.G_WO_BASE
    WHERE
        WORK_ORDER = :WORK_ORDER
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", work_order },
            };

            ClientUtils.ExecuteSQL(s, p.ToArray());
        }

    }
}
