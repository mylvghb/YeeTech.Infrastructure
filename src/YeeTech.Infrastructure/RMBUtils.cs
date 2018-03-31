using System;
using System.Globalization;

namespace YeeTech.Infrastructure
{
    /// <summary>
    ///     人民币工具类
    /// </summary>
    public class RMBUtils
    {
        /// <summary>
        ///     将数值转换为人民币
        /// </summary>
        /// <param name="num">数值</param>
        /// <returns>人民币形式</returns>
        public static string ConvertToRMB(decimal num)
        {
            const string str1 = "零壹贰叁肆伍陆柒捌玖";
            var str2 = "万仟佰拾亿仟佰拾万仟佰拾元角分";
            var str5 = "";
            int i;
            var ch2 = "";
            var nzero = 0;

            num = Math.Round(Math.Abs(num), 2); //将num取绝对值并四舍五入取2位小数 
            var str4 = ((long) (num * 100)).ToString(CultureInfo.InvariantCulture);
            var j = str4.Length;
            if (j > 15) return "溢出";
            str2 = str2.Substring(15 - j); //取出对应位数的str2的值。如：200.55,j为5所以str2=佰拾元角分 

            //循环取出每一位需要转换的值 
            for (i = 0; i < j; i++)
            {
                var str3 = str4.Substring(i, 1);
                var temp = Convert.ToInt32(str3);
                string ch1; //数字的汉语读法 
                if (i != j - 3 && i != j - 7 && i != j - 11 && i != j - 15)
                {
                    //当所取位数不为元、万、亿、万亿上的数字时 
                    if (str3 == "0")
                    {
                        ch1 = "";
                        ch2 = "";
                        nzero = nzero + 1;
                    }
                    else
                    {
                        if (str3 != "0" && nzero != 0)
                        {
                            ch1 = "零" + str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                    }
                }
                else
                {
                    //该位是万亿，亿，万，元位等关键位 
                    if (str3 != "0" && nzero != 0)
                    {
                        ch1 = "零" + str1.Substring(temp * 1, 1);
                        ch2 = str2.Substring(i, 1);
                        nzero = 0;
                    }
                    else
                    {
                        if (str3 != "0" && nzero == 0)
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            if (str3 == "0" && nzero >= 3)
                            {
                                ch1 = "";
                                ch2 = "";
                                nzero = nzero + 1;
                            }
                            else
                            {
                                if (j >= 11)
                                {
                                    ch1 = "";
                                    nzero = nzero + 1;
                                }
                                else
                                {
                                    ch1 = "";
                                    ch2 = str2.Substring(i, 1);
                                    nzero = nzero + 1;
                                }
                            }
                        }
                    }
                }

                if (i == j - 11 || i == j - 3) ch2 = str2.Substring(i, 1);
                str5 = str5 + ch1 + ch2;

                if (i == j - 1 && str3 == "0") str5 = str5 + '整';
            }

            if (num == 0) str5 = "零元整";
            return str5;
        }

        /// <summary>
        ///     将数值字符串转换为人民币
        /// </summary>
        /// <param name="text">数值字符串</param>
        /// <returns>若是数值字符串，返回有效的人民币形式；否则返回为空字符串</returns>
        public static string ConvertToRMB(string text)
        {
            return decimal.TryParse(text, out var num)
                ? ConvertToRMB(num)
                : string.Empty;
        }
    }
}