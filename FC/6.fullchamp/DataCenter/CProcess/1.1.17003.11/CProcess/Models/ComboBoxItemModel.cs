using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CProcess.Models
{
    /// <summary>
    /// 下拉選單選項
    /// </summary>
    internal class ComboBoxItemModel
    {
        internal ComboBoxItemModel()
        {
            ID = "0";

            NAME = string.Empty;
        }

        internal ComboBoxItemModel(ComboBoxItemModel e)
        {
            ID = e.ID;

            NAME = e.NAME;
        }

        public string ID { get; set; } = "0";

        public string NAME { get; set; } = string.Empty;
    }
}
