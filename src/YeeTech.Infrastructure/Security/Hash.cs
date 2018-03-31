using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace YeeTech.Infrastructure.Security
{
    /// <summary>
    ///     得到随机安全码(哈希加密)
    /// </summary>
    public class Hash
    {
        /// <summary>
        ///     得到随机哈希加密字符串
        /// </summary>
        /// <returns>加密字符串</returns>
        public static string GetSecurity()
        {
            var security = Encode(GetRandomValue());
            return security;
        }

        /// <summary>
        ///     得到一个随机数值
        /// </summary>
        /// <returns>随机字符串</returns>
        public static string GetRandomValue()
        {
            var seed = new Random();
            var randomVaule = seed.Next(1, int.MaxValue).ToString();
            return randomVaule;
        }

        /// <summary>
        ///     哈希编码文本
        /// </summary>
        /// <param name="text">原始文本</param>
        /// <returns>编码后的文本</returns>
        public static string Encode(string text)
        {
            var code = new UnicodeEncoding();
            var message = code.GetBytes(text);
            var arithmetic = new SHA512Managed();
            var value = arithmetic.ComputeHash(message);
            return value.Aggregate("", (current, o) => current + ((int) o + "O"));
        }
    }
}