using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Disassemblers;
using BenchmarkDotNet.Running;
using CommandLine;

//| Method | Mean | Error | StdDev | Min | Max |
//| ----------------- | ---------:| ----------:| ---------:| ---------:| ---------:|
//| AccessWithTLB | 15.16 ms | 3.800 ms | 0.208 ms | 15.02 ms | 15.40 ms |
//| AccessWithoutTLB | 25.96 ms | 64.123 ms | 3.515 ms | 23.01 ms | 29.85 ms |

namespace TLBBench
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Measure>();
            Console.ReadKey();
        }
    }
    [ShortRunJob]
    [MinColumn, MaxColumn]
    public class Measure
    {
        const int Size = 8192; // 配列のサイズ（ページサイズを意識する）
        const int Stride = 64; // アクセス間のステップ(64の場合はTLB効果を活用)
        static long[,] data = new long[Size, Size];
        [Benchmark]
        public void AccessWithTLB()
        {
            long sum = 0;
            // 行優先アクセス：TLB最適化が効く
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j += Stride)
                {
                    sum += data[i, j];
                }
            }
        }
        [Benchmark]
        public void AccessWithoutTLB()
        {
            long sum = 0;
            // 列優先アクセス：TLB最適化が効きにくい
            for (int j = 0; j < Size; j += Stride)
            {
                for (int i = 0; i < Size; i++)
                {
                    sum += data[i, j];
                }
            }
        }
    }
}
