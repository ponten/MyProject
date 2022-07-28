using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RCPart.Services
{
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
        /// 重新佈置 DataGridView 各項屬性。
        /// </summary>
        /// <param name="x"></param>
        public static void RearrangeDataGridView(ref DataGridView x)
        {
            x.Update();

            x.Refresh();

            x.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }
    }
}
