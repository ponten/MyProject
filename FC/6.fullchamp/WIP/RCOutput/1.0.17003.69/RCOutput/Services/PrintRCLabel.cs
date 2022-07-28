using PrintLabel;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using RCOutput.Enums;
using System.Text.RegularExpressions;

namespace RCOutput
{
    /// <summary>
    /// 列印防水標籤的服務
    /// </summary>
    class PrintRCLabel
    {
        private Setup PrintSetup = new Setup();
        private string BasePath = Directory.GetCurrentDirectory().Contains("WIP") ?
            Directory.GetCurrentDirectory() : Path.Combine(Directory.GetCurrentDirectory(), "WIP");

        public void Print(string RC_NO, string PROCESS_ID, string PrintQauntity, out string sMessage)
        {
            sMessage = string.Empty;
            string batFile = Path.Combine(BasePath, "PrintGo.bat");
            string btwFile = Path.Combine(BasePath, "RCOutput.btw");
            string lstFile = Path.Combine(BasePath, "RCOutput.lst");

            // 列印內容
            List<string> content = new List<string>();
            List<string> header = new List<string>();
            List<string> values = new List<string>();

            if (GetPrintContent(
                rc_no: RC_NO,
                process_id: PROCESS_ID,
                header: out header,
                values: out values))
            {
                content.Add(string.Join(",", header));

                content.Add(string.Join(",", values));
            }
            else
            {
                content.Add(LabelHeaderEnum.RC_NO.ToString());
                content.Add(RC_NO);
            }

            if (!File.Exists(btwFile))
            {
                sMessage = "btw file not found";
                return;
            }

            if (!File.Exists(lstFile))
            {
                File.AppendAllLines(lstFile, content);
            }
            else
            {
                File.Delete(lstFile);
                File.AppendAllLines(lstFile, content);
            }

            // 刪除舊 batfile
            if (File.Exists(batFile))
            {
                File.Delete(batFile);
            }

            string bat = PrintSetup.LoadBatFile(batFile, ref sMessage, "RC Output");

            // 找不到 bat 指令
            if (string.IsNullOrEmpty(bat))
            {
                return;
            }
            else
            {
                bat = bat.Replace("@PATH1", "\"" + btwFile + "\"")
                    .Replace("@PATH2", "\"" + lstFile + "\"")
                    .Replace("@QTY", PrintQauntity);

                PrintSetup.WriteToPrintGo(batFile, bat);
            }

            Setup.WinExec(batFile, 0);
        }

        /// <summary>
        /// 取得列印內容
        /// </summary>
        /// <param name="rc_no"></param>
        /// <param name="header"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        private bool GetPrintContent(string rc_no, string process_id, out List<string> header, out List<string> values)
        {
            header = new List<string>();

            values = new List<string>();

            string s = @"
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
    A.RC_NO,
    A.CURRENT_QTY,
    A.WIP_OUT_GOOD_QTY    GOOD_QTY,
    A.WIP_OUT_SCRAP_QTY   SCRAP_QTY,
    TO_CHAR(A.OUT_PROCESS_TIME, 'YYYY/MM/DD HH24:MI:SS') OUT_PROCESS_TIME,
    B.PART_NO,
    B.OPTION2,
    B.SPEC1,
    B.SPEC2,
    D.EMP_NAME
FROM
    SAJET.G_RC_TRAVEL   A
    LEFT JOIN SAJET.SYS_PART      B ON A.PART_ID = B.PART_ID
    LEFT JOIN SAJET.G_WO_BASE     C ON A.WORK_ORDER = C.WORK_ORDER
    LEFT JOIN SAJET.SYS_EMP       D ON A.UPDATE_USERID = D.EMP_ID
WHERE
    A.RC_NO = :RC_NO
    AND A.PROCESS_ID = :PROCESS_ID
ORDER BY
    A.OUT_PROCESS_TIME DESC
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "RC_NO", rc_no },
                new object[] { ParameterDirection.Input, OracleType.VarChar, "PROCESS_ID", process_id },
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            if (d != null && d.Tables[0].Rows.Count > 0)
            {
                var row = d.Tables[0].Rows[0];

                #region 工單號碼

                header.Add(LabelHeaderEnum.WORK_ORDER.ToString());

                string work_order = row["RC_NO"].ToString().Trim();

                work_order = AdjustDisplayFormat(work_order);

                values.Add(ToLiteral(work_order));

                #endregion

                #region 開本數

                header.Add(LabelHeaderEnum.BOOK_NO.ToString());

                string book_no = row["BOOK_NO"].ToString().Trim();

                values.Add(ToLiteral(book_no.Trim()));

                #endregion

                #region 製別

                header.Add(LabelHeaderEnum.CATEGORY.ToString());

                string category = row["CATEGORY"].ToString().Trim();

                values.Add(ToLiteral(category.Trim()));

                #endregion

                #region 流程卡號碼

                header.Add(LabelHeaderEnum.RC_NO.ToString());

                values.Add(ToLiteral(rc_no.Trim()));

                #endregion

                #region 當前數量

                header.Add(LabelHeaderEnum.QTY.ToString());

                string current_qty = row["CURRENT_QTY"].ToString().Trim();

                values.Add(ToLiteral(current_qty.Trim()));

                #endregion

                #region 旋型產出良品數量

                header.Add(LabelHeaderEnum.GOOD_QTY.ToString());

                string good_qty = row["GOOD_QTY"].ToString().Trim();

                values.Add(ToLiteral(good_qty.Trim()));

                #endregion

                #region 旋行製程產出不良品數量

                header.Add(LabelHeaderEnum.SCRAP_QTY.ToString());

                string scrqp_qty = row["SCRAP_QTY"].ToString().Trim();

                values.Add(ToLiteral(scrqp_qty.Trim()));

                #endregion

                #region 旋行製程產出時間

                header.Add(LabelHeaderEnum.OUT_PROCESS_TIME.ToString());

                string out_process_time = row["OUT_PROCESS_TIME"].ToString().Trim();

                values.Add(ToLiteral(out_process_time.Trim()));

                #endregion

                #region 料號
                // 舊編優先顯示

                header.Add(LabelHeaderEnum.PART_NO.ToString());

                string part_no = row["PART_NO"].ToString().Trim();

                string former_no = row["OPTION2"].ToString().Trim();

                if (!string.IsNullOrWhiteSpace(former_no) &&
                    former_no.ToUpper() != "N/A" &&
                    former_no.ToUpper() != "NA")
                {
                    part_no = string.Copy(former_no);
                }

                values.Add(ToLiteral(part_no.Trim()));

                #endregion

                #region 品名 / 規格

                header.Add(LabelHeaderEnum.SPEC.ToString());

                string spec
                    = row["SPEC1"].ToString().Trim()
                    + " "
                    + row["SPEC2"].ToString().Trim()
                    ;

                values.Add(ToLiteral(spec.Trim()));

                #endregion

                #region 作業員名稱

                header.Add(LabelHeaderEnum.EMP.ToString());

                string emp_name = row["EMP_NAME"].ToString().Trim();

                values.Add(ToLiteral(emp_name.Trim()));

                #endregion

                return true;
            }
            else
            {
                header.Add(LabelHeaderEnum.RC_NO.ToString());

                values.Add(rc_no);

                return false;
            }
        }

        /// <summary>
        /// 只有改變工單號碼顯示格式，用流程卡號改成 '#' 格式
        /// </summary>
        /// <param name="rc_no">流程卡號</param>
        /// <returns>work_order</returns>
        private string AdjustDisplayFormat(string rc_no)
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
        private string ToLiteral(string input)
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
