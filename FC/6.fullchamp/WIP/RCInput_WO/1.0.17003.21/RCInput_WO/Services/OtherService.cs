using RCInput_WO.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RCInput_WO.Services
{
    /// <summary>
    /// 未分類的商業邏輯
    /// </summary>
    public static class OtherService
    {
        /// <summary>
        /// 取得報工人員 ID
        /// </summary>
        public static string GetEmpID(string EmpNO)
        {
            string s = @"
SELECT
    EMP_ID
FROM
    SAJET.SYS_EMP
WHERE
    EMP_NO = :EMP_NO
";
            var p = new List<object[]>
            {
                new object[] { ParameterDirection.Input, OracleType.VarChar, "EMP_NO", EmpNO }
            };

            var d = ClientUtils.ExecuteSQL(s, p.ToArray());

            return d.Tables[0].Rows[0]["EMP_ID"].ToString();
        }

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
        ///  判斷strNumber是否為指定類型的數字
        ///  1:正整數, 2:非負整數（正整數 + 0）, 3:正浮點數, 4:非負浮點數（正浮點數 + 0）, 5:浮點數
        /// </summary>
        /// <param name="iType"> 數值類型 </param>
        /// <param name="strNumber">判斷的字串</param>
        /// <returns>是返回True,否返回False</returns>
        public static bool IsNumeric(int iType, string strNumber)
        {
            Regex NumberPattern = null;

            switch (iType)
            {
                case 1:   //正整數
                    {
                        NumberPattern = new Regex("^[0-9]*[1-9][0-9]*$");

                        break;
                    }
                case 2:   //非負整數（正整數 + 0）
                    {
                        NumberPattern = new Regex("^\\d+$");

                        break;
                    }
                case 3:   //正浮點數
                    {
                        NumberPattern = new Regex("^(([0-9]+\\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\\.[0-9]+)|([0-9]*[1-9][0-9]*))$");

                        break;
                    }
                case 4:   //非負浮點數（正浮點數 + 0）
                    {
                        NumberPattern = new Regex("^\\d+(\\.\\d+)?$");

                        break;
                    }
                case 5:    //浮點數
                    {
                        NumberPattern = new Regex("^(-?\\d+)(\\.\\d+)?$");

                        break;
                    }
                default:
                    {
                        return false;
                    }
            }

            return NumberPattern.IsMatch(strNumber);
        }

        /// <summary>
        /// Analyzing System Type - Y:lot control, N:piece control
        /// </summary>
        /// <param name="system_type"></param>
        /// <returns>true:lot control, false:piece control</returns>
        public static bool SystemType(out string system_type)
        {
            system_type = string.Empty;

            try
            {
                string sSQL = @"
SELECT
    PARAM_VALUE
FROM
    SAJET.SYS_BASE
WHERE
    PROGRAM = 'RC Manager'
AND PARAM_NAME = 'Lot Control Checked'
AND ROWNUM = 1
";
                DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);

                if (dsTemp != null &&
                    dsTemp.Tables[0].Rows.Count > 0)
                {
                    system_type = dsTemp.Tables[0].Rows[0]["PARAM_VALUE"].ToString();

                    return system_type == "Y";
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
