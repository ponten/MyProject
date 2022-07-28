using CProcess.Enums;
using CProcess.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CProcess.Services
{
    /// <summary>
    /// 製程特殊設定的商業邏輯
    /// </summary>
    internal static class ProcessOptionService
    {
        /// <summary>
        /// 取得下拉選單選項
        /// </summary>
        /// <param name="process_ption"></param>
        /// <returns></returns>
        internal static List<ComboBoxItemModel> GetComboBoxItems(ProcessOptionEnum process_ption)
        {
            var select_list = new List<ComboBoxItemModel>();

            DataSet d;

            string s = string.Empty;

            if (process_ption == ProcessOptionEnum.UsingEquipment)
            {
                select_list.Add(new ComboBoxItemModel());

                s = $@"
SELECT
    MACHINE_TYPE_ID     {nameof(ComboBoxItemModel.ID)},
    MACHINE_TYPE_DESC   {nameof(ComboBoxItemModel.NAME)}
FROM
    SAJET.SYS_MACHINE_TYPE
WHERE
    ENABLED = 'Y'
ORDER BY
    2
";
            }
            else if (process_ption == ProcessOptionEnum.ProductionUnit)
            {
                select_list.Add(new ComboBoxItemModel());

                s = $@"
SELECT
    DEPT_ID     {nameof(ComboBoxItemModel.ID)},
    DEPT_NAME   {nameof(ComboBoxItemModel.NAME)}
FROM
    SAJET.SYS_DEPT
WHERE
    ENABLED = 'Y'
ORDER BY
    2
";
            }

            if (string.IsNullOrWhiteSpace(s))
            {
                d = null;
            }
            else
            {
                d = ClientUtils.ExecuteSQL(s);
            }

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in d.Tables[0].Rows)
                {
                    select_list.Add(new ComboBoxItemModel
                    {
                        ID = row[nameof(ComboBoxItemModel.ID)].ToString(),
                        NAME = row[nameof(ComboBoxItemModel.NAME)].ToString(),
                    });
                }
            }

            return select_list;
        }
    }
}
