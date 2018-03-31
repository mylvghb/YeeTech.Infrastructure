using System;
using System.Text;

namespace YeeTech.Infrastructure.Security
{
    /// <summary>
    ///     Base64 编码与解码
    /// </summary>
    public sealed class Base64
    {
        /// <summary>
        ///     Base64 解码
        /// </summary>
        /// <param name="text">编码文本</param>
        /// <returns>解码后文本</returns>
        public static string Decode(string text)
        {
            var bytes = Convert.FromBase64String(text);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        ///     Base64 编码
        /// </summary>
        /// <param name="text">原始文本</param>
        /// <returns>编码后的文本</returns>
        public static string Encode(string text)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
        }
    }
}