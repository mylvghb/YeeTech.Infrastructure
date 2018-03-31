using System;
using NUnit.Framework;
using YeeTech.Infrastructure.Text;

namespace YeeTech.Infrastructure.Tests
{
    [TestFixture]
    public class PinyinTests
    {
        [Test]
        public void Pinyin_for_normal()
        {
            var pinyin = Pinyin.GetPinyin("中国人民解放军");
            Console.WriteLine(pinyin);
            Assert.AreEqual("zhong guo ren min jie fang jun", pinyin);
        }

        [Test]
        public void Pinyin_for_polyphone()
        {
            var pinyin = Pinyin.GetPinyin("银行");
            Console.WriteLine(pinyin);
            Assert.AreEqual("yin hang", pinyin);
        }
    }
}