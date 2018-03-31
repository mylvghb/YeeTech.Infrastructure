using System;
using NUnit.Framework;

namespace YeeTech.Infrastructure.Tests
{
    [TestFixture]
    public class RmbTests
    {
        [Test]
        public void To_rmb_with_decimal()
        {
            const decimal num = 11.21m;
            var rmb = num.ToRMB();
            Console.WriteLine(rmb);
            Assert.AreEqual("壹拾壹元贰角壹分", rmb);
        }

        [Test]
        public void To_rmb_with_integer()
        {
            const int num = 1522221;
            var rmb = num.ToRMB();
            Assert.AreEqual("壹佰伍拾贰万贰仟贰佰贰拾壹元整", rmb);
        }

        [Test]
        public void To_rmb_with_string()
        {
            const string num = "123.15";
            var rmb = num.ToRMB();
            Console.WriteLine(rmb);
            Assert.IsNotEmpty(rmb);
        }
    }
}