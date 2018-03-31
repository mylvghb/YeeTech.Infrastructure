using System;
using System.Security.Cryptography;
using System.Text;

namespace YeeTech.Infrastructure.Security
{
    /// <summary>
    ///     MD5 加密。MD5加密方式不可逆
    /// </summary>
    public class MD5
    {
        /// <summary>
        ///     加密
        /// </summary>
        /// <param name="text">原始文本</param>
        /// <param name="strongEncrypt">是否是強加密。强加密为32位，非强加密为16位</param>
        /// <returns>加密文本</returns>
        public static string Encrypt(string text, bool strongEncrypt = true)
        {
            var provider = new MD5CryptoServiceProvider();
            //BitConverter.ToString()得到的字符串形式为2个一对，对与对之间加个“-”符号，如，“7F-2C-4A”。 
            //md5.ComputeHash(UTF8Encoding.Default.GetBytes(t16.Text.Trim()))，计算哈希值。 
            //4表示初始位置，8表示有8个对，每个对都是2位，故有16位（32位为16对），即就是从第4对开始连续取8对。
            var hash = provider.ComputeHash(Encoding.UTF8.GetBytes(text));
            var encrypted = strongEncrypt ? BitConverter.ToString(hash) : BitConverter.ToString(hash, 4, 8);
            return encrypted.Replace("-", "");
        }
    }
}