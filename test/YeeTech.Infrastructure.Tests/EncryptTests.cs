using System;
using NUnit.Framework;
using YeeTech.Infrastructure.Security;

namespace YeeTech.Infrastructure.Tests
{
    [TestFixture]
    public class EncryptTests
    {
        [Test]
        public void Aes_encrypt_and_decrypt_string()
        {
            const string key = "SOFT";
            var encrypted = AES.Encrypt("password", key);
            Console.WriteLine(encrypted);
            var text = AES.Decrypt(encrypted, key);
            Assert.AreEqual("password", text);
        }

        [Test]
        public void Base64_encode_and_decode()
        {
            var encoded = Base64.Encode("password");
            Console.WriteLine(encoded);
            var text = Base64.Decode(encoded);
            Assert.AreEqual("password", text);
        }

        [Test]
        public void Des_encrypt_and_decrypt_string()
        {
            const string key = "SOFT";
            var encrypted = DES.Encrypt("password", key);
            Console.WriteLine(encrypted);
            var text = DES.Decrypt(encrypted, key);
            Assert.AreEqual("password", text);
        }

        [Test]
        public void Hash_encode()
        {
            var text = Hash.Encode("password");
            Console.WriteLine(text);
            Assert.IsNotEmpty(text);
        }

        [Test]
        public void Md5_encrypt()
        {
            var encrypted = MD5.Encrypt("password");
            Console.WriteLine(encrypted);
            Assert.IsNotEmpty(encrypted);
        }

        [Test]
        public void Rsa_encrypt_and_decrypt_string()
        {
            RSA.GenerateKey(out var key, out var pkey);
            var encrypted = RSA.Encrypt(key, "password");
            Console.WriteLine(encrypted);
            var text = RSA.Decrypt(key, encrypted);
            Assert.AreEqual("password", text);
        }

        [Test]
        public void Rsa_generate_key()
        {
            RSA.GenerateKey(out var key, out var pkey);
            Console.WriteLine(key);
            Console.Write("\n");
            Console.WriteLine(pkey);
        }

        [Test]
        public void TripleDes_encrypt_and_decrypt_string()
        {
            var encrypted = TripleDES.Encrypt("password", "SOFT");
            Console.WriteLine(encrypted);
            var text = TripleDES.Decrypt(encrypted, "SOFT");
            Assert.AreEqual("password", text);
        }

        [Test]
        public void Xor_encrypt_and_decrypt_string()
        {
            var encrypted = Xor.Encrypt("password");
            Console.WriteLine(encrypted);
            var text = Xor.Decrypt(encrypted);
            Assert.AreEqual("password", text);
        }
    }
}