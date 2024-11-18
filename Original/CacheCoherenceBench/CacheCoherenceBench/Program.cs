using System;
using System.Diagnostics;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using System.Threading;
using BenchmarkDotNet.Running;


namespace CacheCoherenceBench
{
//    | Method                | Mean       | Error      | StdDev    | Min        | Max        |
//|---------------------- |-----------:|-----------:|----------:|-----------:|-----------:|
//| ParallelForNoConflict |   2.284 ms |  0.2044 ms | 0.0112 ms |   2.273 ms |   2.296 ms |
//| ParallelForConflict   | 485.671 ms | 63.0771 ms | 3.4575 ms | 481.770 ms | 488.357 ms |
    [ShortRunJob]
    [MinColumn, MaxColumn]
    public class Measure
    {

        const int iterations = 1000000; // 演算を行う回数
        static long[] sharedData = new long[16]; // 同じキャッシュラインに載るように設定

        [Benchmark]
        public void ParallelForNoConflict()
        {
            Parallel.For(0, Environment.ProcessorCount, core =>
            {
                long localVariable = 0;
                for (int i = 0; i < iterations; i++)
                {
                    localVariable += i;
                }
                Interlocked.Add(ref sharedData[core * 2], localVariable); // 各スレッドが独立したメモリアドレスを触る
            });
        }
        [Benchmark]
        public void ParallelForConflict()
        {
            Parallel.For(0, Environment.ProcessorCount, core =>
            {
                for (int i = 0; i < iterations; i++)
                {
                    Interlocked.Add(ref sharedData[0], i); // 全てのスレッドが同じメモリアドレスを共有しアクセスする
                }
            });
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Measure>();
            Console.ReadKey();
        }
    }
}
