using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Runtime.InteropServices;
using BarTender;
using System.Text.RegularExpressions;
using System.Linq;
using Print_RCList.Enums;
using System.CodeDom.Compiler;
using System.CodeDom;
using System.Data.OracleClient;

namespace Print_RCList
{
    /// <summary>
    /// 流程卡號碼清單的列印標籤服務
    /// </summary>
    public static class RCListService
    {
        /// <summary>
        /// 基礎檔案路徑
        /// </summary>
        private static readonly string BasePath = Directory.GetCurrentDirectory().Contains("BarcodeCenter") ?
            Directory.GetCurrentDirectory() : Path.Combine(Directory.GetCurrentDirectory(), "BarcodeCenter");

        /// <summary>
        /// 基礎檔案名稱
        /// </summary>
        private const string BaseFileName = "BR_RC_LIST";

        /// <summary>
        /// 組成流程卡號碼清單標籤的列印內容，並且視情況列印標籤
        /// </summary>
        /// <param name="work_order">工單號碼</param>
        /// <param name="direct_print">是否直接列印。否，則回傳批次檔指令</param>
        /// <param name="message">回傳訊息。若執行失敗則內容為錯誤訊息；若不列印則內容為批次檔指令</param>
        /// <returns></returns>
        public static bool Print(string work_order, bool direct_print, out string message)
        {
            message = "OK";

            List<string> runcard_list;

            int runcard_number = GetRuncardAndNummber(
                work_order: work_order,
                runcard_name_list: out runcard_list);

            string batFile = Path.Combine(BasePath, "PrintList.bat");
            string lstFile = Path.Combine(BasePath, $"{BaseFileName}.lst");
            string btwFile;

            if (runcard_number > 60)
            {
                btwFile = Path.Combine(BasePath, $"{BaseFileName}_3.btw");
            }
            else if (runcard_number > 30)
            {
                btwFile = Path.Combine(BasePath, $"{BaseFileName}_2.btw");
            }
            else
            {
                btwFile = Path.Combine(BasePath, $"{BaseFileName}_1.btw");
            }

            if (!File.Exists(btwFile))
            {
                message = "btw file not found";

                return false;
            }
            if (File.Exists(batFile))
            {
                File.Delete(batFile);
            }
            if (File.Exists(lstFile))
            {
                File.Delete(lstFile);
            }

            string s_header = GetPrintHeader();

            PutPrintContent(lstFile, s_header);

            string s_content = string.Empty;

            s_content += GetPrintContent(
                work_order: work_order,
                data_group: LabelDataGroupEnum.WORK_ORDER);

            s_content += GetPrintContent(
                runcard_number: runcard_number,
                runcard_list: runcard_list);

            s_content += GetPrintContent(
                work_order: work_order,
                data_group: LabelDataGroupEnum.PAGE);

            PutPrintContent(lstFile, s_content);

            string bat_command = LoadBatFile();

            bat_command = bat_command
                .Replace("@PATH1", "\"" + btwFile + "\"")
                .Replace("@PATH2", "\"" + lstFile + "\"")
                .Replace("@QTY", "1");

            if (!direct_print)
            {
                message = bat_command;

                return true;
            }

            //PutPrintContent(batFile, bat_command);
            PutBatFile(batFile, bat_command);

            try
            {
                WinExec(batFile, 0);

                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;

                return false;
            }

        }

        /// <summary>
        /// 把資料內容存到目標文件內
        /// </summary>
        /// <param name="target_file">目標檔案/位址</param>
        /// <param name="content">內容</param>
        public static void PutPrintContent(string target_file, string content)
        {
            using (var fs = new FileStream(target_file, FileMode.Append, FileAccess.Write, FileShare.None))
            using (var sw = new StreamWriter(fs, Encoding.UTF8))
            {
                sw.WriteLine(content);

                sw.Flush();
            }
        }

        /// <summary>
        /// 把指令存到批次檔
        /// </summary>
        /// <param name="target_file"></param>
        /// <param name="content"></param>
        public static void PutBatFile(string target_file, string content)
        {
            try
            {
                if (File.Exists(target_file))
                {
                    File.Delete(target_file);
                }
                System.IO.File.AppendAllText(target_file, content);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 取得列印清單的表頭欄位
        /// </summary>
        /// <returns></returns>
        internal static string GetPrintHeader()
        {
            var builder = new StringBuilder();

            builder.Append("WORK_ORDER,PO,TARGET_QTY,PRODUCE_NUMBER,OLD_NUMBER,SPEC1,SPEC2,");
            builder.Append("BLUEPRINT,PRINTDATE,MADE_CATEGORY,WO_OPTION2,REMARK,SIGNATURE,");

            string rc_no_label_name = "RC_NO";

            builder.Append(LoopString(rc_no_label_name));

            string rc_no_barcode_name = "RC_NO_VAL";

            builder.Append(LoopString(rc_no_barcode_name));

            builder.Append("PAGE");

            return builder.ToString();
        }

        /// <summary>
        /// 取得列印資料內容（客製）
        /// </summary>
        /// <param name="work_order"></param>
        /// <param name="data_group"></param>
        /// <returns></returns>
        internal static string GetPrintContent(string work_order, LabelDataGroupEnum data_group)
        {
            string s;

            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", work_order },
            };

            var builder = new StringBuilder();

            if (data_group == LabelDataGroupEnum.WORK_ORDER)
            {
                s = @"
SELECT
    A.WORK_ORDER,
    A.DATENUMBER
    || '-'
    || A.NUMBER1 PO,
    A.TARGET_QTY,
    CASE TRIM(A.OPERATION_ID)
        WHEN '10A'   THEN
            A.PRODUCENO1
        WHEN '10B'   THEN
            A.PRODUCENO2
        WHEN '10C'   THEN
            A.PRODUCE_NUMBER
        ELSE
            A.PRODUCE_NUMBER
    END PRODUCE_NUMBER,
    A.OLD_NUMBER,
    TRIM(C.SPEC1) SPEC1,
    TRIM(C.SPEC2) SPEC2,
    A.BLUEPRINT,
    TO_CHAR(SYSDATE, 'YYYY/MM/DD') PRINTDATE,
    CASE A.MADE_CATEGORY
        WHEN '1'   THEN
            '試製'
        WHEN '2'   THEN
            '樣試'
        WHEN '3'   THEN
            '量試'
        WHEN '4'   THEN
            '量產'
        WHEN '5'   THEN
            '重工'
        ELSE
            '--'
    END MADE_CATEGORY,
    A.WO_OPTION2,
    A.REMARK,
    '['
    || B.EMP_NO
    || ']'
    || B.EMP_NAME SIGNATURE
FROM
    SAJET.G_WO_BASE   A,
    SAJET.SYS_EMP     B,
    SAJET.SYS_PART    C
WHERE
    A.WORK_ORDER = :WORK_ORDER
    AND A.UPDATE_USERID = B.EMP_ID
    AND A.PART_ID = C.PART_ID
";
                var d = ClientUtils.ExecuteSQL(s, p.ToArray());

                if (d != null &&
                    d.Tables[0].Rows.Count > 0)
                {
                    DataRow row = d.Tables[0].Rows[0];

                    builder.Append(ToLiteral(GetDisplayFormat(work_order.Trim(), ParseTargetTypeEnum.WorkOrder)));
                    builder.Append(',');

                    builder.Append(ToLiteral(row["PO"].ToString().Trim()));
                    builder.Append(',');

                    builder.Append(ToLiteral(row["TARGET_QTY"].ToString().Trim()));
                    builder.Append(',');

                    builder.Append(ToLiteral(row["PRODUCE_NUMBER"].ToString().Trim()));
                    builder.Append(',');

                    builder.Append(ToLiteral(row["OLD_NUMBER"].ToString().Trim()));
                    builder.Append(',');

                    builder.Append(ToLiteral(row["SPEC1"].ToString().Trim()));
                    builder.Append(',');

                    builder.Append(ToLiteral(row["SPEC2"].ToString().Trim()));
                    builder.Append(',');

                    builder.Append(ToLiteral(row["BLUEPRINT"].ToString().Trim()));
                    builder.Append(',');

                    builder.Append(ToLiteral(row["PRINTDATE"].ToString().Trim()));
                    builder.Append(',');

                    builder.Append(ToLiteral(row["MADE_CATEGORY"].ToString().Trim()));
                    builder.Append(',');

                    builder.Append(ToLiteral(row["WO_OPTION2"].ToString().Trim()));
                    builder.Append(',');

                    builder.Append(ToLiteral(row["REMARK"].ToString().Trim()));
                    builder.Append(',');

                    builder.Append(ToLiteral(row["SIGNATURE"].ToString().Trim()));
                    builder.Append(',');

                }
                else
                {
                    builder.Append("\"\",");
                    builder.Append("\"\",");
                    builder.Append("\"\",");
                    builder.Append("\"\",");
                    builder.Append("\"\",");
                    builder.Append("\"\",");
                    builder.Append("\"\",");
                    builder.Append("\"\",");
                    builder.Append("\"\",");
                    builder.Append("\"\",");
                    builder.Append("\"\",");
                    builder.Append("\"\",");
                    builder.Append("\"\",");
                }
            }
            else if (data_group == LabelDataGroupEnum.PAGE)
            {
                builder.Append("\"\"");
            }

            return builder.ToString();
        }

        /// <summary>
        /// 取得列印資料內容（客製）
        /// </summary>
        /// <param name="runcard_number">流程卡數量</param>
        /// <param name="runcard_list">流程卡號碼的清單</param>
        /// <returns></returns>
        internal static string GetPrintContent(int runcard_number, List<string> runcard_list)
        {
            var label_builder = new StringBuilder();

            var barcode_builder = new StringBuilder();

            runcard_list = runcard_list.OrderBy(x => x).ToList();

            for (int i = 0; i < 90; i++)
            {
                if (runcard_number > i)
                {
                    string current_rc_no = runcard_list[i];

                    label_builder.Append(ToLiteral(GetDisplayFormat(current_rc_no, ParseTargetTypeEnum.Runcard)));
                    label_builder.Append(',');

                    barcode_builder.Append(ToLiteral(current_rc_no));
                    barcode_builder.Append(',');
                }
                else
                {
                    label_builder.Append("\"\",");

                    barcode_builder.Append("\"\",");
                }
            }

            string s_out = label_builder.ToString() + barcode_builder.ToString();

            return s_out;
        }

        /// <summary>
        /// 讀取列印標籤的批次檔指令
        /// </summary>
        /// <param name="program">呼叫模組</param>
        /// <returns></returns>
        internal static string LoadBatFile(string program = "RC List")
        {
            string sValue = "";

            string sSQL = $@"
SELECT
    *
FROM
    SAJET.SYS_BASE
WHERE
    PROGRAM = '{program}'
    AND PARAM_NAME = 'Bartender Print Command'
    AND ROWNUM = 1
";
            DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);

            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                sValue = dsTemp.Tables[0].Rows[0]["PARAM_VALUE"].ToString();
            }

            return sValue;
        }

        /// <summary>
        /// 將傳入的字串加上跳脫字元後輸出
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string ToLiteral(string input)
        {
            using (var writer = new StringWriter())
            using (var provider = CodeDomProvider.CreateProvider("CSharp"))
            {
                provider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), writer, null);
                return writer.ToString();
            }
        }

        /// <summary>
        /// 重組工單號碼或流程卡號碼，取得顯示用的格式
        /// </summary>
        /// <param name="target_string">目標字串</param>
        /// <param name="target_type">目標種類（工單號碼 / 流程卡號碼）</param>
        /// <returns></returns>
        private static string GetDisplayFormat(string target_string, ParseTargetTypeEnum target_type)
        {
            // 流程卡號碼：10A2020110011001000001 => 10A-202011-11-1#001
            // 工單號碼：10A2020110011001 => 10A-202011-11-1

            string input = target_string;

            string output = string.Empty;

            // 拆字串
            Regex pattern;

            if (target_type == ParseTargetTypeEnum.Runcard)
            {
                pattern = new Regex("^(.{3})(.{6})(.{4})(.{3})-(.{3})$");
            }
            else if (target_type == ParseTargetTypeEnum.WorkOrder)
            {
                pattern = new Regex("^(.{3})(.{6})(.{4})(.{3})$");
            }
            else
            {
                pattern = new Regex(".*");

                output = string.Copy(input);

                return output;
            }

            var matches = pattern.Matches(input);

            // 組字串
            if (matches.Count <= 0)
            {
                output = string.Copy(input);
            }
            else
            {
                // 1. 第一個 match 項目是自己，把它過濾掉
                // 2. 把數字前面的 0 去掉
                var group = matches[0].Groups.Cast<Group>()
                    .Where(x => x.Value != input)
                    .Select(x =>
                    {
                        var s = x.Value;

                        if (int.TryParse(s, out int i))
                        {
                            return i.ToString();
                        }
                        else
                        {
                            return s;
                        }
                    }).ToList();

                // 3. 組合成顯示用的格式
                if (target_type == ParseTargetTypeEnum.Runcard)
                {
                    var group2 = group.GetRange(0, group.Count - 1);

                    output = string.Join("-", group2) + " #" + (group[group.Count - 1]).ToString().PadLeft(3, '0');
                }
                else if (target_type == ParseTargetTypeEnum.WorkOrder)
                {
                    output = string.Join("-", group);
                }
            }

            return output;
        }

        /// <summary>
        /// 計算流程卡的數量，並且取得流程卡號碼的清單
        /// </summary>
        /// <param name="work_order">工單號碼</param>
        /// <param name="runcard_name_list">流程卡號碼的清單</param>
        /// <returns>計算流程卡的數量</returns>
        private static int GetRuncardAndNummber(string work_order, out List<string> runcard_name_list)
        {
            string s = @"
SELECT
    RC_NO,
    CURRENT_QTY
FROM
    SAJET.G_RC_STATUS
WHERE
    WORK_ORDER = :WORK_ORDER
ORDER BY
    RC_NO
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "WORK_ORDER", work_order },
            };

            DataSet d = ClientUtils.ExecuteSQL(s, p.ToArray());

            runcard_name_list = new List<string>();

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in d.Tables[0].Rows)
                {
                    runcard_name_list.Add(row["RC_NO"].ToString());
                }

                return d.Tables[0].Rows.Count;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 迴圈組合參數
        /// </summary>
        /// <param name="s_sample"></param>
        /// <returns></returns>
        private static string LoopString(string s_sample)
        {
            string s_out = string.Empty;

            for (int page = 1; page < 4; page++)
            {
                for (int count = 1; count <= 30; count++)
                {
                    s_out += $"{s_sample}_{page}_{count},";
                }
            }

            return s_out;
        }

        /// <summary>
        /// 呼叫 exe 的方法。使用 UNICODE 編碼。
        /// </summary>
        /// <param name="exeName">執行目標的位址/名稱</param>
        /// <param name="operType">展示方式。詳細請參閱：docs.microsoft.com -&gt; win32/api -&gt; WinExec 的說明</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
        public static extern int WinExec(string exeName, int operType);

    }
}
