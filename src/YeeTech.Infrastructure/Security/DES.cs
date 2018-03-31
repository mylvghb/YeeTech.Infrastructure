using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace YeeTech.Infrastructure.Security
{
    /// <summary>
    ///     DES 加/解密
    /// </summary>
    /// <remarks>采用 DES 算法进行加/解密。该算法和 TripleDES 类似, TripleDES 采用的 Key 为24位, DES采用的 Key 为8位</remarks>
    public class DES
    {
        /// <summary>
        ///     使用指定的密匙加密文本
        /// </summary>
        /// <param name="text">明文文本</param>
        /// <param name="key">密匙</param>
        /// <returns>密文文本</returns>
        public static string Encrypt(string text, string key)
        {
            var des = new DESCryptoServiceProvider();
            var inputByteArray = Encoding.Default.GetBytes(text);
            des.Key = des.IV = Encoding.ASCII.GetBytes(MD5.Encrypt(key).Substring(0, 0x8));
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            var ret = new StringBuilder();
            foreach (var b in ms.ToArray()) ret.AppendFormat("{0:X2}", b); //转换成大写的十六进制文本
            return ret.ToString();
        }

        /// <summary>
        ///     使用指定的密匙解密文本
        /// </summary>
        /// <param name="text">经过加密的文本</param>
        /// <param name="key">密匙</param>
        /// <returns>解密后的文本</returns>
        public static string Decrypt(string text, string key)
        {
            var des = new DESCryptoServiceProvider();
            var len = text.Length / 2;
            var inputByteArray = new byte[len];
            int x;
            for (x = 0; x < len; x++)
            {
                var i = Convert.ToInt32(text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte) i;
            }

            des.Key = des.IV = Encoding.ASCII.GetBytes(MD5.Encrypt(key).Substring(0, 0x8));
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }
    }
}