using System;
using System.Security.Cryptography;
using System.Text;

namespace YeeTech.Infrastructure.Security
{
    /// <summary>
    ///     三重 DES 加/解密
    /// </summary>
    /// <remarks>采用 TripleDES 算法进行加/解密。该算法和 DES 类似, DES 采用的 Key 为8位,  TripleDES 采用的 Key 为24位</remarks>
    public class TripleDES
    {
        /// <summary>
        ///     使用给定密钥字符串加密string
        /// </summary>
        /// <param name="text">原始文字</param>
        /// <param name="key">密钥</param>
        /// <returns>密文</returns>
        public static string Encrypt(string text, string key)
        {
            var buff = Encoding.Default.GetBytes(text);
            var kb = Encoding.Default.GetBytes(key);
            return Convert.ToBase64String(Encrypt(buff, kb));
        }

        /// <summary>
        ///     使用给定密钥字符串加密string
        /// </summary>
        /// <param name="text">原始文字</param>
        /// <param name="key">密钥</param>
        /// <param name="encoding">字符编码方案</param>
        /// <returns>密文</returns>
        public static string Encrypt(string text, string key, Encoding encoding)
        {
            var buff = encoding.GetBytes(text);
            var kb = encoding.GetBytes(key);
            return Convert.ToBase64String(Encrypt(buff, kb));
        }

        /// <summary>
        ///     使用给定密钥字符串解密string
        /// </summary>
        /// <param name="text">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static string Decrypt(string text, string key)
        {
            return Decrypt(text, key, Encoding.Default);
        }

        /// <summary>
        ///     使用给定密钥字符串解密string,返回指定编码方式明文
        /// </summary>
        /// <param name="text">密文</param>
        /// <param name="key">密钥</param>
        /// <param name="encoding">字符编码方案</param>
        /// <returns>明文</returns>
        public static string Decrypt(string text, string key, Encoding encoding)
        {
            var buff = Convert.FromBase64String(text);
            var kb = encoding.GetBytes(key);
            return encoding.GetString(Decrypt(buff, kb));
        }

        /// <summary>
        ///     生成MD5摘要
        /// </summary>
        /// <param name="bytes">数据源</param>
        /// <returns>摘要</returns>
        public static byte[] MakeMD5(byte[] bytes)
        {
            var hashmd5 = new MD5CryptoServiceProvider();
            var keyhash = hashmd5.ComputeHash(bytes);
            return keyhash;
        }

        /// <summary>
        ///     使用给定密钥加密
        /// </summary>
        /// <param name="bytes">明文</param>
        /// <param name="key">密钥</param>
        /// <returns>密文</returns>
        public static byte[] Encrypt(byte[] bytes, byte[] key)
        {
            var des = new TripleDESCryptoServiceProvider
            {
                Key = MakeMD5(key),
                Mode = CipherMode.ECB
            };

            return des.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length);
        }

        /// <summary>
        ///     使用给定密钥解密数据
        /// </summary>
        /// <param name="bytes">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static byte[] Decrypt(byte[] bytes, byte[] key)
        {
            var des = new TripleDESCryptoServiceProvider
            {
                Key = MakeMD5(key),
                Mode = CipherMode.ECB
            };

            return des.CreateDecryptor().TransformFinalBlock(bytes, 0, bytes.Length);
        }
    }
}