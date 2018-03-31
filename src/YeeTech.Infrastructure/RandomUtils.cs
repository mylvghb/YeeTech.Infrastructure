using System;
using System.Security.Cryptography;

namespace YeeTech.Infrastructure
{
    /// <summary>
    ///     随机数工具类
    /// </summary>
    public class RandomUtils
    {
        //随机字节数组
        private static readonly byte[] bytes = new byte[4];

        //加密随机数生成器
        private static readonly RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();

        /// <summary>
        ///     获得下一个随机数
        /// </summary>
        /// <param name="max">最大值</param>
        /// <returns>随机数</returns>
        public static int Next(int max)
        {
            provider.GetBytes(bytes);
            var value = BitConverter.ToInt32(bytes, 0);
            value = value % (max + 1);
            return value < 0 ? -value : value;
        }

        /// <summary>
        ///     获得下一个随机数
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>随机数</returns>
        public static int Next(int min, int max)
        {
            if (min > max) throw new ArgumentException("max value must greater than min value.");
            var value = Next(max - min) + min;
            return value;
        }

        /// <summary>
        ///     获取指定个数的随机索引
        /// </summary>
        /// <param name="maxIndex">最大索引，从0开始</param>
        /// <param name="count">获取的个数</param>
        /// <returns>随机索引</returns>
        public static int[] GetIndexs(int maxIndex, int count)
        {
            if (maxIndex < 0 || count < 1 || maxIndex < count) return null;
            var array = new int[count];
            var index = Next(0, maxIndex);
            array[0] = index;
            for (var i = 1; i < count; i++)
            {
                while (Array.IndexOf(array, index) != -1) index = Next(0, maxIndex);
                array[i] = index;
            }

            return array;
        }

        /// <summary>
        /// 获取新的种子数据
        /// </summary>
        /// <returns>种子数据</returns>
        public static int GetNewSeed()
        {
            provider.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}