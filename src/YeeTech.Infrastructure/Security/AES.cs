using System;
using System.Security.Cryptography;
using System.Text;
using YeeTech.Infrastructure.Text;

namespace YeeTech.Infrastructure.Security
{
    /// <summary>
    ///     AES 加密解密
    /// </summary>
    /// <remarks>高级加密标准 (AES) 算法（又称为 Rijndael）</remarks>
    public class AES
    {
        private static readonly byte[] Keys =
            {0x41, 0x72, 0x65, 0x79, 0x6f, 0x75, 0x6d, 0x79, 0x53, 110, 0x6f, 0x77, 0x6d, 0x61, 110, 0x3f};

        #region 加密方法

        /// <summary>
        ///     加密
        /// </summary>
        /// <param name="text">待加密的文本</param>
        /// <param name="key">密钥</param>
        /// <returns>返回与此实例等效的加密文本</returns>
        public static string Encrypt(string text, string key)
        {
            key = TextUtils.CutLeft(key, 0x20);
            key = key.PadRight(0x20, ' ');
            var transform = new RijndaelManaged {Key = Encoding.UTF8.GetBytes(key.Substring(0, 0x20)), IV = Keys}
                .CreateEncryptor();
            var bytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(transform.TransformFinalBlock(bytes, 0, bytes.Length));
        }

        /// <summary>
        ///     加密
        /// </summary>
        /// <param name="bytes">待加密的字节</param>
        /// <param name="key">密钥</param>
        /// <returns>返回与此实例等效的加密字节</returns>
        public static byte[] EncryptBuffer(byte[] bytes, string key)
        {
            key = TextUtils.CutLeft(key, 0x20);
            key = key.PadRight(0x20, ' ');
            var managed = new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key.Substring(0, 0x20)),
                IV = Keys
            };
            return managed.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length);
        }

        #endregion

        #region 解密方法

        /// <summary>
        ///     解密。
        /// </summary>
        /// <param name="text">待解密的文本</param>
        /// <param name="key">密钥</param>
        /// <returns>返回与此实例等效的解密文本</returns>
        public static string Decrypt(string text, string key)
        {
            try
            {
                key = TextUtils.CutLeft(key, 0x20);
                key = key.PadRight(0x20, ' ');
                var transform =
                    new RijndaelManaged {Key = Encoding.UTF8.GetBytes(key), IV = Keys}.CreateDecryptor();
                var inputBuffer = Convert.FromBase64String(text);
                var bytes = transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
                return Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        ///     解密
        /// </summary>
        /// <param name="bytes">待解密的字节</param>
        /// <param name="key">密钥</param>
        /// <returns>返回与此实例等效的解密字节</returns>
        public static byte[] DecryptBuffer(byte[] bytes, string key)
        {
            try
            {
                key = TextUtils.CutLeft(key, 0x20);
                key = key.PadRight(0x20, ' ');
                var managed = new RijndaelManaged
                {
                    Key = Encoding.UTF8.GetBytes(key),
                    IV = Keys
                };
                return managed.CreateDecryptor().TransformFinalBlock(bytes, 0, bytes.Length);
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}