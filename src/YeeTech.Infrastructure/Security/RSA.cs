using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace YeeTech.Infrastructure.Security
{
    /// <summary>
    ///     RSA 加/解密及 RSA 签名和验证
    /// </summary>
    /// <remarks>RSA 加密演算法是一种非对称加密演算法。在公开密钥加密和电子商业中RSA被广泛使用</remarks>
    public class RSA
    {
        #region RSA 秘钥

        /// <summary>
        ///     生成 RSA 私钥和公钥
        /// </summary>
        /// <param name="keys">同时包含的公匙和私匙 (xml 格式)</param>
        /// <param name="publicKey">仅公匙 (xml 格式)</param>
        public static void GenerateKey(out string keys, out string publicKey)
        {
            var rsa = new RSACryptoServiceProvider();
            keys = rsa.ToXmlString(true);
            publicKey = rsa.ToXmlString(false);
        }

        #endregion

        #region RSA 加密

        /// <summary>
        ///     RSA 加密方法
        /// </summary>
        /// <param name="xml">xml 格式的公匙/私匙</param>
        /// <param name="text">明文文本</param>
        /// <returns>密文文本</returns>
        public static string Encrypt(string xml, string text)
        {
            return Encrypt(xml, new UnicodeEncoding().GetBytes(text));
        }

        /// <summary>
        ///     RSA 加密方法
        /// </summary>
        /// <param name="xml">xml 格式的公匙/私匙</param>
        /// <param name="bytes">明文文本的字节数组</param>
        /// <returns>密文文本</returns>
        public static string Encrypt(string xml, byte[] bytes)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xml);
            var cypherTextBArray = rsa.Encrypt(bytes, false);
            var result = Convert.ToBase64String(cypherTextBArray);
            return result;
        }

        #endregion

        #region RSA的解密方法

        /// <summary>
        ///     RSA 解密方法
        /// </summary>
        /// <param name="xml">xml 格式的公匙/私匙</param>
        /// <param name="text">密文文本</param>
        /// <returns>明文文本</returns>
        public static string Decrypt(string xml, string text)
        {
            return Decrypt(xml, Convert.FromBase64String(text));
        }

        /// <summary>
        ///     RSA 解密方法
        /// </summary>
        /// <param name="xml">xml 格式的公匙/私匙</param>
        /// <param name="bytes">密文字节数组</param>
        /// <returns>明文文本</returns>
        public static string Decrypt(string xml, byte[] bytes)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xml);
            var dypherTextBArray = rsa.Decrypt(bytes, false);
            var result = new UnicodeEncoding().GetString(dypherTextBArray);
            return result;
        }

        #endregion

        #region 获取Hash描述表

        /// <summary>
        ///     获取 Hash 值
        /// </summary>
        /// <param name="text">原始字符串</param>
        /// <param name="bytes">哈希字节数组</param>
        /// <returns>成功返回 true</returns>
        public static bool GetHash(string text, ref byte[] bytes)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            var md5 = HashAlgorithm.Create("MD5");
            var buffer = Encoding.GetEncoding("GB2312").GetBytes(text);
            bytes = md5.ComputeHash(buffer);

            return true;
        }

        /// <summary>
        ///     获取 Hash 值
        /// </summary>
        /// <param name="text">原始字符串</param>
        /// <param name="hash">哈希字符串</param>
        /// <returns>成功返回 true</returns>
        public static bool GetHash(string text, ref string hash)
        {
            if (hash == null) throw new ArgumentNullException(nameof(hash));
            byte[] hashDataBArray = null;
            GetHash(text, ref hashDataBArray);
            hash = Convert.ToBase64String(hashDataBArray);

            return true;
        }

        /// <summary>
        ///     获取 Hash 值
        /// </summary>
        /// <param name="fs">文件流对象</param>
        /// <param name="bytes">哈希字节数组</param>
        /// <returns>成功返回 true</returns>
        public static bool GetHash(FileStream fs, ref byte[] bytes)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            var md5 = HashAlgorithm.Create("MD5");
            if (md5 != null) bytes = md5.ComputeHash(fs);
            fs.Close();

            return true;
        }

        /// <summary>
        ///     获取 Hash 值
        /// </summary>
        /// <param name="fs">文件流对象</param>
        /// <param name="hash">哈希字符串</param>
        /// <returns>成功返回 true</returns>
        public static bool GetHash(FileStream fs, ref string hash)
        {
            if (hash == null) throw new ArgumentNullException(nameof(hash));
            byte[] hashDataBArray = null;
            GetHash(fs, ref hashDataBArray);
            hash = Convert.ToBase64String(hashDataBArray);

            return true;
        }

        #endregion

        #region RSA签名

        /// <summary>
        ///     RSA 签名
        /// </summary>
        /// <param name="xml">XML 格式的公匙/私匙</param>
        /// <param name="rgbHash">要执行的数据源</param>
        /// <param name="signatureBArray">签名后的字节数组</param>
        /// <returns>成功返回 true</returns>
        public static bool Signature(string xml, byte[] rgbHash, ref byte[] signatureBArray)
        {
            if (signatureBArray == null) throw new ArgumentNullException(nameof(signatureBArray));
            var rsa = new RSACryptoServiceProvider();

            rsa.FromXmlString(xml);
            var rsf = new RSAPKCS1SignatureFormatter(rsa);
            //设置签名的算法为MD5 
            rsf.SetHashAlgorithm("MD5");
            //执行签名 
            signatureBArray = rsf.CreateSignature(rgbHash);

            return true;
        }

        /// <summary>
        ///     RSA 签名
        /// </summary>
        /// <param name="xml">XML 格式的公匙/私匙</param>
        /// <param name="rgbHash">要执行的数据源</param>
        /// <param name="signatureString">签名后的字符串</param>
        public static bool Signature(string xml, byte[] rgbHash, ref string signatureString)
        {
            if (signatureString == null) throw new ArgumentNullException(nameof(signatureString));
            byte[] signatureBArray = null;
            Signature(xml, rgbHash, ref signatureBArray);
            signatureString = Convert.ToBase64String(signatureBArray);

            return true;
        }

        /// <summary>
        ///     RSA 签名
        /// </summary>
        /// <param name="xml">XML 格式的公匙/私匙</param>
        /// <param name="text">字符串数据</param>
        /// <param name="signatureBArray">签名后的字节数组</param>
        /// <returns>成功返回 true</returns>
        public static bool Signature(string xml, string text, ref byte[] signatureBArray)
        {
            Signature(xml, Convert.FromBase64String(text), ref signatureBArray);

            return true;
        }

        /// <summary>
        ///     RSA 签名
        /// </summary>
        /// <param name="xml">XML 格式的公匙/私匙</param>
        /// <param name="text">字符串数据</param>
        /// <param name="signatureString">签名后的字符串</param>
        /// <returns>成功返回 true</returns>
        public static bool Signature(string xml, string text, ref string signatureString)
        {
            Signature(xml, Convert.FromBase64String(text), ref signatureString);

            return true;
        }

        #endregion

        #region RSA 签名验证

        /// <summary>
        ///     RSA 签名验证
        /// </summary>
        /// <param name="xml">XML 格式的公匙/私匙</param>
        /// <param name="rigHash">要执行的数据源</param>
        /// <param name="signatureHash">经过签名的 Hash 字节数组</param>
        /// <returns>如果为true, 表示验证通过；否则为不通过</returns>
        public static bool VerifySignature(string xml, byte[] rigHash, byte[] signatureHash)
        {
            var rsa = new RSACryptoServiceProvider();

            rsa.FromXmlString(xml);
            var rsd = new RSAPKCS1SignatureDeformatter(rsa);
            //指定解密的时候HASH算法为MD5 
            rsd.SetHashAlgorithm("MD5");

            return rsd.VerifySignature(rigHash, signatureHash);
        }

        /// <summary>
        ///     RSA 签名验证
        /// </summary>
        /// <param name="xml">XML 格式的公匙/私匙</param>
        /// <param name="text">字符串数据</param>
        /// <param name="signatureHash">经过签名的 Hash 字节数组</param>
        /// <returns>如果为true, 表示验证通过；否则为不通过</returns>
        public static bool VerifySignature(string xml, string text, byte[] signatureHash)
        {
            return VerifySignature(xml, Convert.FromBase64String(text), signatureHash);
        }

        /// <summary>
        ///     RSA 签名验证
        /// </summary>
        /// <param name="xml">XML 格式的公匙/私匙</param>
        /// <param name="rigHash">要执行的数据源</param>
        /// <param name="signatureString">经过签名的字符串</param>
        /// <returns>如果为true, 表示验证通过；否则为不通过</returns>
        public static bool VerifySignature(string xml, byte[] rigHash, string signatureString)
        {
            return VerifySignature(xml, rigHash, Convert.FromBase64String(signatureString));
        }

        /// <summary>
        ///     RSA 签名验证
        /// </summary>
        /// <param name="xml">XML 格式的公匙/私匙</param>
        /// <param name="text">字符串数据</param>
        /// <param name="signatureString">经过签名的字符串</param>
        /// <returns>如果为true, 表示验证通过；否则为不通过</returns>
        public static bool VerifySignature(string xml, string text, string signatureString)
        {
            return VerifySignature(xml, Convert.FromBase64String(text),
                Convert.FromBase64String(signatureString));
        }

        #endregion
    }
}