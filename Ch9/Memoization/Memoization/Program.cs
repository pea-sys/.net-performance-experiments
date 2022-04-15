using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoization
{
    /// <summary>
    /// [Output]
    /// |                              Method |              Mean |          Error |        StdDev |               Min |               Max |
    /// |------------------------------------ |------------------:|---------------:|--------------:|------------------:|------------------:|
    /// |            FibonacciNumberBenchMark | 13,959,702.667 us | 49,380.8347 us | 2,706.7302 us | 13,956,679.400 us | 13,961,900.800 us |
    /// | FibonacciNumberMemoizationBenchMark |          8.434 us |      0.5987 us |     0.0328 us |          8.406 us |          8.470 us |
    /// |   FibonacciNumberIterationBenchMark |          1.951 us |      0.7419 us |     0.0407 us |          1.922 us |          1.997 us |/// </summary>
    [ShortRunJob]
    [MinColumn, MaxColumn]
    public class Fibonacci
    {
        [Benchmark]
        public void FibonacciNumberBenchMark()
        {
            for (uint i = 1; i < 45; ++i)
            {
                FibonacciNumber(i);
            }
        }
        /// <summary>
        /// 途中計算結果を何度も計算するため非効率
        /// </summary>
        /// <param name="which"></param>
        /// <returns></returns>
        public ulong FibonacciNumber(uint which)
        {
            if (which == 1 || which == 2) return 1;
            return FibonacciNumber(which - 2) + FibonacciNumber(which - 1);
        }
        [Benchmark]
        public void FibonacciNumberMemoizationBenchMark()
        {
            for (uint i = 1; i < 45; ++i)
            {
                FibonacciNumberMemoization(i);
            }
        }
        /// <summary>
        /// 途中結果を配列に格納する
        /// </summary>
        /// <param name="which"></param>
        /// <returns></returns>
        public static ulong FibonacciNumberMemoization(uint which)
        {
            if (which == 1 || which == 2) return 1;
            ulong[] array = new ulong[which];
            array[0] = 1; array[1] = 1;
            return FibonacciNumberMemoization(which, array);
        }
        private static ulong FibonacciNumberMemoization(uint which, ulong[] array)
        {
            if (array[which - 3] == 0)
            {
                array[which - 3] = FibonacciNumberMemoization(which - 2, array);
            }
            if (array[which - 2] == 0)
            {
                array[which - 2] = FibonacciNumberMemoization(which - 1, array);
            }
            array[which - 1] = array[which - 3] + array[which - 2];
            return array[which - 1];
        }
        [Benchmark]
        public void FibonacciNumberIterationBenchMark()
        {
            for (uint i = 1; i < 45; ++i)
            {
                FibonacciNumberIteration(i);
            }
        }
        /// <summary>
        /// 最後に計算した２つの数だけ格納
        /// </summary>
        /// <param name="which"></param>
        /// <returns></returns>
        public static ulong FibonacciNumberIteration(ulong which)
        {
            if (which == 1 || which == 2) return 1;
            ulong a = 1, b = 1;
            for (ulong i = 2; i < which; ++i)
            {
                ulong c = a + b;
                a = b;
                b = c;
            }
            return b;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Fibonacci>();
        }
    }
}
