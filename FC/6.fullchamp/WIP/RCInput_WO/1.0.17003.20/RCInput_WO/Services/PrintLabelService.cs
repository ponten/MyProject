using SajetClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.IO;
using System.Linq;
using System.Text;
using RCInput_WO.Enums;
using System.Text.RegularExpressions;
using System.CodeDom.Compiler;
using System.CodeDom;
using RCInput_WO.Models;
using PrintLabel;

namespace RCInput_WO.Services
{
    internal static class PrintLabelService
    {
        /// <summary>
        /// 基礎檔案路徑
        /// </summary>
        private static string BasePath = Directory.GetCurrentDirectory().Contains("WIP") ?
            Directory.GetCurrentDirectory() : Path.Combine(Directory.GetCurrentDirectory(), "WIP");

        /// <summary>
        /// 檢查流程卡所在製程是否列印防水標籤
        /// </summary>
        /// <param name="rc_no"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        internal static bool CheckIfPrintLabel(string rc_no, ref List<string> messages)
        {
            string s = @"
SELECT
    a.process_id,
    b.option1,
    b.option2,
    b.option3
FROM
    sajet.g_rc_status          a,
    sajet.sys_process_option   b
WHERE
    a.process_id = b.process_id
    AND ( b.option1 = 'Y'
            OR b.option2 = 'Y'
              OR b.option3 = 'Y' )
    AND rc_no = :rc_no
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "rc_no", rc_no },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d != null &&
                d.Tables[0].Rows.Count > 0)
            {
                // 列印防水標籤: OPTION1
                if (d.Tables[0].Rows[0]["OPTION1"].ToString() == "Y")
                {
                    messages.Add(SajetCommon.SetLanguage("To print WP label"));

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 列印防水標籤
        /// </summary>
        /// <param name="rc_list"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        internal static bool PrintLabel(List<RC_DETAILS> rc_list, out string message)
        {
            message = "OK";

            string batFile = Path.Combine(BasePath, "PrintGo.bat");
            string btwFile = Path.Combine(BasePath, "RCOutput.btw");
            string lstFile = Path.Combine(BasePath, "RCOutput.lst");

            if (!File.Exists(btwFile))
            {
                message = SajetCommon.SetLanguage("btw file not found");

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

            foreach (RC_DETAILS rc_data in rc_list)
            {
                string s_temp = string.Empty;

                s_temp += GetPrintContent(rc_data.RC_NO, LabelDataGroupEnum.WORK_ORDER);

                s_temp += GetPrintContent(rc_data.RC_NO, LabelDataGroupEnum.RC_NO);

                s_temp += GetPrintContent(rc_data.RC_NO, LabelDataGroupEnum.B050);

                s_temp += GetPrintContent(rc_data.RC_NO, LabelDataGroupEnum.B060);

                s_temp += GetPrintContent(rc_data.RC_NO, LabelDataGroupEnum.B070);

                PutPrintContent(lstFile, s_temp);
            }

            try
            {
                Setup PrintSetup = new Setup();

                string bat = PrintSetup.LoadBatFile(batFile, ref message, "RC Output");

                if (string.IsNullOrEmpty(bat))
                {
                    message = SajetCommon.SetLanguage("batch command not found");

                    return false;
                }

                bat = bat.Replace("@PATH1", "\"" + btwFile + "\"")
                    .Replace("@PATH2", "\"" + lstFile + "\"")
                    .Replace("@QTY", "1");

                // 呼叫這個寫入方法的批次檔可以列印，沒有深入研究為什麼
                PrintSetup.WriteToPrintGo(batFile, bat);
                //PutPrintContent(batFile, bat);

                Setup.WinExec(batFile, 0);
            }
            catch (Exception ex)
            {
                message = ex.Message;

                return false;
            }

            return true;
        }

        /// <summary>
        /// 取得列印清單的表頭欄位
        /// </summary>
        /// <returns></returns>
        internal static string GetPrintHeader()
        {
            var builder = new StringBuilder();

            #region 工單資料組
            builder.Append(LabelHeaderEnum.WORK_ORDER.ToString());
            builder.Append(',');
            builder.Append(LabelHeaderEnum.BOOK_NO.ToString());
            builder.Append(',');
            builder.Append(LabelHeaderEnum.CATEGORY.ToString());
            builder.Append(',');

            builder.Append(LabelHeaderEnum.PART_NO.ToString());
            builder.Append(',');
            builder.Append(LabelHeaderEnum.SPEC.ToString());
            builder.Append(',');
            #endregion
            #region 流程卡資料組
            builder.Append(LabelHeaderEnum.RC_NO.ToString());
            builder.Append(',');
            builder.Append(LabelHeaderEnum.QTY.ToString());
            builder.Append(',');
            builder.Append(LabelHeaderEnum.STOVE.ToString());
            builder.Append(',');
            #endregion
            #region 旋型資料組
            builder.Append(LabelDataGroupEnum.B050.ToString());
            builder.Append('_');
            builder.Append(LabelHeaderEnum.GOOD_QTY.ToString());
            builder.Append(',');

            builder.Append(LabelDataGroupEnum.B050.ToString());
            builder.Append('_');
            builder.Append(LabelHeaderEnum.SCRAP_QTY.ToString());
            builder.Append(',');

            builder.Append(LabelDataGroupEnum.B050.ToString());
            builder.Append('_');
            builder.Append(LabelHeaderEnum.OUT_PROCESS_TIME.ToString());
            builder.Append(',');

            builder.Append(LabelDataGroupEnum.B050.ToString());
            builder.Append('_');
            builder.Append(LabelHeaderEnum.EMP.ToString());
            builder.Append(',');
            #endregion
            #region 固溶處理資料組
            builder.Append(LabelDataGroupEnum.B060.ToString());
            builder.Append('_');
            builder.Append(LabelHeaderEnum.GOOD_QTY.ToString());
            builder.Append(',');

            builder.Append(LabelDataGroupEnum.B060.ToString());
            builder.Append('_');
            builder.Append(LabelHeaderEnum.SCRAP_QTY.ToString());
            builder.Append(',');

            builder.Append(LabelDataGroupEnum.B060.ToString());
            builder.Append('_');
            builder.Append(LabelHeaderEnum.OUT_PROCESS_TIME.ToString());
            builder.Append(',');

            builder.Append(LabelDataGroupEnum.B060.ToString());
            builder.Append('_');
            builder.Append(LabelHeaderEnum.EMP.ToString());
            builder.Append(',');
            #endregion
            #region 時效處理資料組
            builder.Append(LabelDataGroupEnum.B070.ToString());
            builder.Append('_');
            builder.Append(LabelHeaderEnum.GOOD_QTY.ToString());
            builder.Append(',');

            builder.Append(LabelDataGroupEnum.B070.ToString());
            builder.Append('_');
            builder.Append(LabelHeaderEnum.SCRAP_QTY.ToString());
            builder.Append(',');

            builder.Append(LabelDataGroupEnum.B070.ToString());
            builder.Append('_');
            builder.Append(LabelHeaderEnum.OUT_PROCESS_TIME.ToString());
            builder.Append(',');

            builder.Append(LabelDataGroupEnum.B070.ToString());
            builder.Append('_');
            builder.Append(LabelHeaderEnum.EMP.ToString());
            #endregion

            return builder.ToString();
        }

        /// <summary>
        /// 取得列印資料內容
        /// </summary>
        /// <param name="rc_no"></param>
        /// <param name="data_group"></param>
        /// <returns></returns>
        internal static string GetPrintContent(string rc_no, LabelDataGroupEnum data_group)
        {
            string s;

            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", rc_no },
            };

            var builder = new StringBuilder();

            if (data_group == LabelDataGroupEnum.WORK_ORDER)
            {
                s = @"
SELECT
    C.WO_OPTION2          BOOK_NO,
    CASE C.MADE_CATEGORY
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
    END CATEGORY,
    B.PART_NO,
    B.OPTION2,
    B.SPEC1,
    B.SPEC2
FROM
    SAJET.G_RC_STATUS   A
    LEFT JOIN SAJET.SYS_PART      B ON A.PART_ID = B.PART_ID
    LEFT JOIN SAJET.G_WO_BASE     C ON A.WORK_ORDER = C.WORK_ORDER
WHERE
    A.RC_NO = :RC_NO
";
                var d = ClientUtils.ExecuteSQL(s, p.ToArray());

                if (d != null &&
                    d.Tables[0].Rows.Count > 0)
                {
                    DataRow row = d.Tables[0].Rows[0];

                    string work_order = AdjustDisplayFormat(rc_no.Trim());

                    builder.Append(ToLiteral(work_order));
                    builder.Append(',');

                    builder.Append(ToLiteral(row["BOOK_NO"].ToString().Trim()));
                    builder.Append(',');

                    builder.Append(ToLiteral(row["CATEGORY"].ToString().Trim()));
                    builder.Append(',');

                    string part_no = row["PART_NO"].ToString().Trim();

                    string former_no = row["OPTION2"].ToString().Trim();

                    if (!string.IsNullOrWhiteSpace(former_no) &&
                        former_no.ToUpper() != "N/A" &&
                        former_no.ToUpper() != "NA")
                    {
                        part_no = string.Copy(former_no);
                    }

                    builder.Append(ToLiteral(part_no));
                    builder.Append(',');

                    string spec
                        = row["SPEC1"].ToString().Trim()
                        + " "
                        + row["SPEC2"].ToString().Trim()
                        ;

                    builder.Append(ToLiteral(spec));
                    builder.Append(',');

                }
                else
                {
                    builder.Append("\"\",");
                    builder.Append("\"\",");
                    builder.Append("\"\",");
                    builder.Append("\"\",");
                    builder.Append("\"\",");
                }
            }
            else if (data_group == LabelDataGroupEnum.RC_NO)
            {
                s = @"
SELECT
    RC_NO,
    CURRENT_QTY
FROM
    SAJET.G_RC_STATUS
WHERE
    RC_NO = :RC_NO
";
                var d = ClientUtils.ExecuteSQL(s, p.ToArray());

                if (d != null &&
                    d.Tables[0].Rows.Count > 0)
                {
                    DataRow row = d.Tables[0].Rows[0];

                    builder.Append(ToLiteral(rc_no));
                    builder.Append(',');

                    builder.Append(ToLiteral(row["CURRENT_QTY"].ToString()));
                    builder.Append(',');
                }

                s = @"
SELECT
    C.MACHINE_CODE,
    C.MACHINE_DESC
FROM
    SAJET.G_RC_TRAVEL                A,
    SAJET.G_RC_TRAVEL_MACHINE_DOWN   B,
    SAJET.SYS_MACHINE                C
WHERE
    A.RC_NO = :RC_NO
    AND A.PROCESS_ID = 100020
    AND A.RC_NO = B.RC_NO
    AND A.NODE_ID = B.NODE_ID
    AND B.MACHINE_ID = C.MACHINE_ID
";
                d = ClientUtils.ExecuteSQL(s, p.ToArray());

                if (d != null &&
                    d.Tables[0].Rows.Count > 0)
                {
                    DataRow row = d.Tables[0].Rows[0];

                    string s_machine
                        = "["
                        + row["MACHINE_CODE"].ToString().Trim()
                        + "] "
                        + row["MACHINE_DESC"].ToString().Trim()
                        ;

                    builder.Append(ToLiteral(s_machine));
                    builder.Append(',');
                }
                else
                {
                    builder.Append("\"\",");
                    builder.Append("\"\",");
                    builder.Append("\"\",");
                }
            }
            else if (data_group == LabelDataGroupEnum.B050)
            {
                s = @"
SELECT
    A.WIP_OUT_GOOD_QTY    GOOD_QTY,
    A.WIP_OUT_SCRAP_QTY   SCRAP_QTY,
    TO_CHAR(A.OUT_PROCESS_TIME, 'YYYY/MM/DD HH24:MI:SS') OUT_PROCESS_TIME,
    D.EMP_NAME
FROM
    SAJET.G_RC_TRAVEL   A
    LEFT JOIN SAJET.SYS_EMP       D ON A.UPDATE_USERID = D.EMP_ID
WHERE
    A.RC_NO = :RC_NO
    AND A.PROCESS_ID = :PROCESS_ID
";
                p.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", "100019" });

                var d = ClientUtils.ExecuteSQL(s, p.ToArray());

                if (d != null &&
                    d.Tables[0].Rows.Count > 0)
                {
                    DataRow row = d.Tables[0].Rows[0];

                    builder.Append(ToLiteral(row["GOOD_QTY"].ToString().Trim()));
                    builder.Append(',');

                    builder.Append(ToLiteral(row["SCRAP_QTY"].ToString().Trim()));
                    builder.Append(',');

                    builder.Append(ToLiteral(row["OUT_PROCESS_TIME"].ToString().Trim()));
                    builder.Append(',');

                    builder.Append(ToLiteral(row["EMP_NAME"].ToString().Trim()));
                    builder.Append(',');
                }
                else
                {
                    builder.Append("\"\",");
                    builder.Append("\"\",");
                    builder.Append("\"\",");
                    builder.Append("\"\",");
                }
            }
            else if (data_group == LabelDataGroupEnum.B060)
            {
                s = @"
SELECT
    A.WIP_OUT_GOOD_QTY    GOOD_QTY,
    A.WIP_OUT_SCRAP_QTY   SCRAP_QTY,
    TO_CHAR(A.OUT_PROCESS_TIME, 'YYYY/MM/DD HH24:MI:SS') OUT_PROCESS_TIME,
    D.EMP_NAME
FROM
    SAJET.G_RC_TRAVEL   A
    LEFT JOIN SAJET.SYS_EMP       D ON A.UPDATE_USERID = D.EMP_ID
WHERE
    A.RC_NO = :RC_NO
    AND A.PROCESS_ID = :PROCESS_ID
";
                p.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", "100020" });

                var d = ClientUtils.ExecuteSQL(s, p.ToArray());

                if (d != null &&
                    d.Tables[0].Rows.Count > 0)
                {
                    DataRow row = d.Tables[0].Rows[0];

                    builder.Append(ToLiteral(row["GOOD_QTY"].ToString().Trim()));
                    builder.Append(',');

                    builder.Append(ToLiteral(row["SCRAP_QTY"].ToString().Trim()));
                    builder.Append(',');

                    builder.Append(ToLiteral(row["OUT_PROCESS_TIME"].ToString().Trim()));
                    builder.Append(',');

                    builder.Append(ToLiteral(row["EMP_NAME"].ToString().Trim()));
                    builder.Append(',');
                }
                else
                {
                    builder.Append("\"\",");
                    builder.Append("\"\",");
                    builder.Append("\"\",");
                    builder.Append("\"\",");
                }
            }
            else if (data_group == LabelDataGroupEnum.B070)
            {
                s = @"
SELECT
    A.CURRENT_QTY    GOOD_QTY,
    0   SCRAP_QTY,
    TO_CHAR(A.WIP_IN_TIME, 'YYYY/MM/DD HH24:MI:SS') IN_PROCESS_TIME,
    D.EMP_NAME
FROM
    SAJET.G_RC_STATUS   A
    LEFT JOIN SAJET.SYS_EMP       D ON A.UPDATE_USERID = D.EMP_ID
WHERE
    A.RC_NO = :RC_NO
    AND A.PROCESS_ID = :PROCESS_ID
";
                p.Add(new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", "100024" });

                var d = ClientUtils.ExecuteSQL(s, p.ToArray());

                if (d != null &&
                    d.Tables[0].Rows.Count > 0)
                {
                    DataRow row = d.Tables[0].Rows[0];

                    builder.Append(ToLiteral(row["GOOD_QTY"].ToString().Trim()));
                    builder.Append(',');

                    builder.Append(ToLiteral(row["SCRAP_QTY"].ToString().Trim()));
                    builder.Append(',');

                    builder.Append(ToLiteral(row["IN_PROCESS_TIME"].ToString().Trim()));
                    builder.Append(',');

                    builder.Append(ToLiteral(row["EMP_NAME"].ToString().Trim()));
                    //builder.Append(',');
                }
                else
                {
                    builder.Append("\"\",");
                    builder.Append("\"\",");
                    builder.Append("\"\",");
                    builder.Append("\"\"");
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// 把資料內容存到 lst 文件內
        /// </summary>
        /// <param name="list_file"></param>
        /// <param name="content"></param>
        internal static void PutPrintContent(string target_file, string content)
        {
            using (var fs = new FileStream(target_file, FileMode.Append, FileAccess.Write, FileShare.None))
            using (var sw = new StreamWriter(fs, Encoding.UTF8))
            {
                sw.WriteLine(content);

                sw.Flush();
            }
        }

        /// <summary>
        /// 只有改變工單號碼顯示格式，用流程卡號改成 '#' 格式
        /// </summary>
        /// <param name="rc_no">流程卡號</param>
        /// <returns>work_order</returns>
        private static string AdjustDisplayFormat(string rc_no)
        {
            // 10A2020110011001 => 10A-202011-11-1#001

            string input = rc_no;

            string output = string.Empty;

            // 拆字串
            var pattern = new Regex("^(.{3})(.{6})(.{4})(.{3})-(.{3})$");

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

                var group2 = group.GetRange(0, group.Count - 1);

                output = string.Join("-", group2) + " #" + (group[group.Count - 1]).ToString().PadLeft(3, '0');
            }

            return output;
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
    }
}
