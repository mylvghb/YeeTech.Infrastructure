using System;
using System.Text;

namespace YeeTech.Infrastructure.Security
{
    /// <summary>
    ///     异或对称加/解密
    /// </summary>
    public sealed class Xor
    {
        private const ushort ENCRYPT_KEY_LEN = 8;

        /// <summary>
        ///     解密
        /// </summary>
        /// <param name="text">密文</param>
        /// <returns>明文</returns>
        public static string Decrypt(string text)
        {
            var builder = new StringBuilder();
            var length = (ushort) text.Length;
            if (length < ENCRYPT_KEY_LEN * 8) return "";
            var num2 = Convert.ToUInt16(text.Substring(0, 4), 0x10);
            if (length != (num2 + ENCRYPT_KEY_LEN - 1) / ENCRYPT_KEY_LEN * ENCRYPT_KEY_LEN * 8) return "";
            for (var i = 0; i < num2; i++)
            {
                var str2 = text.Substring(i * 8, 4);
                var str = text.Substring(i * 8 + 4, 4);
                var num4 = Convert.ToUInt16(str2, 0x10);
                var num5 = Convert.ToUInt16(str, 0x10);
                builder.Append((char) (num4 ^ num5));
            }

            return builder.ToString();
        }

        /// <summary>
        ///     加密
        /// </summary>
        /// <param name="text">明文</param>
        /// <returns>密文</returns>
        public static string Encrypt(string text)
        {
            var builder = new StringBuilder();
            var numArray = new ushort[ENCRYPT_KEY_LEN];
            numArray[0] = (ushort) text.Length;
            var random = new Random();
            for (var i = 1; i < numArray.Length; i++) numArray[i] = (ushort) (random.Next(0, 0xffff) % 0xffff);
            var num3 = (ushort) ((numArray[0] + ENCRYPT_KEY_LEN - 1) / ENCRYPT_KEY_LEN * ENCRYPT_KEY_LEN);
            for (ushort j = 0; j < num3; j = (ushort) (j + 1))
            {
                ushort num2;
                if (j < numArray[0])
                    num2 = (ushort) (text[j] ^ numArray[j % ENCRYPT_KEY_LEN]);
                else
                    num2 = (ushort) (numArray[j % ENCRYPT_KEY_LEN] ^ (ushort) (random.Next(0, 0xffff) % 0xffff));
                builder.Append(numArray[j % ENCRYPT_KEY_LEN].ToString("X4")).Append(num2.ToString("X4"));
            }

            return builder.ToString();
        }
    }
}