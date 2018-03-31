using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace YeeTech.Infrastructure.Security
{
    /// <summary>
    ///     实现(AES)对称加密
    /// </summary>
    public class Symmetric
    {
        private static readonly SymmetricAlgorithm mobjCryptoService;

        /// <summary>
        ///     初始化对称加密类
        /// </summary>
        static Symmetric()
        {
            mobjCryptoService = new RijndaelManaged();
        }

        /// <summary>
        ///     获得密钥
        /// </summary>
        /// <returns>密钥</returns>
        private static byte[] GetLegalKey()
        {
            var sTemp = "A7Df09!325Bg6A5aB@40ahkFCklAuB4D#40Dqy0D7oD8$AvB8Dd6b%aDa8Ae8709*44D41d";
            mobjCryptoService.GenerateKey();
            var bytTemp = mobjCryptoService.Key;
            var KeyLength = bytTemp.Length;
            if (sTemp.Length > KeyLength)
                sTemp = sTemp.Substring(0, KeyLength);
            else if (sTemp.Length < KeyLength)
                sTemp = sTemp.PadRight(KeyLength, ' ');
            return Encoding.ASCII.GetBytes(sTemp);
        }

        /// <summary>
        ///     获得初始向量IV
        /// </summary>
        /// <returns>初试向量IV</returns>
        private static byte[] GetLegalIV()
        {
            var sTemp = "GF46dD87%AgD2(3FjC467Bk%&B241A95Fk&7tD3452f*96b4465(e797fAa44A6be8Aa259";
            mobjCryptoService.GenerateIV();
            var bytTemp = mobjCryptoService.IV;
            var IVLength = bytTemp.Length;
            if (sTemp.Length > IVLength)
                sTemp = sTemp.Substring(0, IVLength);
            else if (sTemp.Length < IVLength)
                sTemp = sTemp.PadRight(IVLength, ' ');
            return Encoding.ASCII.GetBytes(sTemp);
        }

        /// <summary>
        ///     加密方法
        /// </summary>
        /// <param name="text">待加密的串</param>
        /// <returns>经过加密的串</returns>
        public static string Encrypt(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            var bytIn = Encoding.UTF8.GetBytes(text);
            var ms = new MemoryStream();
            mobjCryptoService.Key = GetLegalKey();
            mobjCryptoService.IV = GetLegalIV();
            var encrypto = mobjCryptoService.CreateEncryptor();
            var cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();
            ms.Close();
            var bytOut = ms.ToArray();
            return Convert.ToBase64String(bytOut);
        }

        /// <summary>
        ///     解密方法
        /// </summary>
        /// <param name="text">待解密的串</param>
        /// <returns>经过解密的串</returns>
        public static string Decrypt(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            try
            {
                var bytIn = Convert.FromBase64String(text);
                var ms = new MemoryStream(bytIn, 0, bytIn.Length);
                mobjCryptoService.Key = GetLegalKey();
                mobjCryptoService.IV = GetLegalIV();
                var encrypto = mobjCryptoService.CreateDecryptor();
                var cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                var sr = new StreamReader(cs);
                return sr.ReadToEnd();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}