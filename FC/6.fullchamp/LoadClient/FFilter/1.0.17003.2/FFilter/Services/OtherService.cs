using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FFilter.Services
{
    /// <summary>
    /// 未分類的方法、商業邏輯
    /// </summary>
    static class OtherService
    {
        /// <summary>
        /// 重新佈置 DataGridView 各項屬性。
        /// </summary>
        /// <param name="x"></param>
        static void RearrangeDataGridView(ref DataGridView x)
        {
            x.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }
    }
}
