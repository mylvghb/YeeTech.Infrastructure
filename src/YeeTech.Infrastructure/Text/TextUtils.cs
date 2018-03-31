using System;
using System.Text;

namespace YeeTech.Infrastructure.Text
{
    /// <summary>
    ///     文本工具类
    /// </summary>
    public class TextUtils
    {
        /// <summary>
        ///     从给定字符串左侧开始截取指定长度个字符(使用字节宽度)
        /// </summary>
        /// <param name="originalVal">原始字符串</param>
        /// <param name="cutLength">截取字符长度</param>
        /// <returns>截取后的字符串</returns>
        public static string CutLeft(string originalVal, int cutLength)
        {
            if (string.IsNullOrEmpty(originalVal)) return string.Empty;
            //指定长度小于1，字符串原样返回
            if (cutLength < 1) return originalVal;
            var bytes = Encoding.Default.GetBytes(originalVal);
            if (bytes.Length <= cutLength) return originalVal;
            var length = cutLength;
            var numArray = new int[cutLength];
            var num2 = 0;
            for (var i = 0; i < cutLength; i++)
            {
                if (bytes[i] > 0x7f)
                {
                    num2++;
                    if (num2 == 3) num2 = 1;
                }
                else
                {
                    num2 = 0;
                }

                numArray[i] = num2;
            }

            if (bytes[cutLength - 1] > 0x7f && numArray[cutLength - 1] == 1) length = cutLength + 1;
            var destinationArray = new byte[length];
            Array.Copy(bytes, destinationArray, length);
            return Encoding.Default.GetString(destinationArray);
        }

        /// <summary>
        ///     半角转全角(SBC case)
        /// </summary>
        /// <param name="input">输入的字符串</param>
        /// <returns>全角字符串</returns>
        public static string ConvertToSBC(string input)
        {
            var c = input.ToCharArray();
            for (var i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char) 12288;
                    continue;
                }

                if (c[i] < 127)
                    c[i] = (char) (c[i] + 65248);
            }

            return new string(c);
        }

        /// <summary>
        ///     全角转半角(SBC case)
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns>半角字符串</returns>
        public static string ConvertToDBC(string input)
        {
            var c = input.ToCharArray();
            for (var i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char) 32;
                    continue;
                }

                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char) (c[i] - 65248);
            }

            return new string(c);
        }

        /// <summary>
        ///     隐藏IP地址, 隐藏IP地址最后一位用*号代替 (暂不支持IPV6地址)
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <param name="fields">保留的段，IPV4地址为4段</param>
        /// <returns>隐藏指定段的IP地址</returns>
        public static string HiiddenIP(string ip, int fields)
        {
            if (string.IsNullOrEmpty(ip)) return "(未记录)";
            if (fields > 3) return ip;
            if (ip.Contains(":")) return "(不支持ipv6)";
            var strArray = ip.Split('.');
            if (strArray.Length != 4) return "(未记录)";
            if (fields == 3) return strArray[0] + "." + strArray[1] + "." + strArray[2] + ".*";
            if (fields == 2) return strArray[0] + "." + strArray[1] + ".*.*";
            if (fields == 1) return strArray[0] + ".*.*.*";
            return "*.*.*.*";
        }

        /// <summary>
        ///     格式化文件大小
        /// </summary>
        /// <param name="size">以直接为单位的文件大小</param>
        /// <returns>文件大小。例如：10KB、20MB等</returns>
        public static string FormatFileSize(long size)
        {
            var strArray = new[] {"B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB", "NB", "DB"};
            double num = size;
            var index = 0;
            while (num >= 1024.0)
            {
                num /= 1024.0;
                index++;
                if (index == 4) break;
            }

            return num.ToString("####.#") + strArray[index];
        }
    }
}