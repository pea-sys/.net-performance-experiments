using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Disassemblers;
using BenchmarkDotNet.Running;
using CommandLine;

//| Method | Mean | Error | StdDev | Min | Max |
//| ------------------- | ----------:| ----------:| ---------:| ----------:| ----------:|
//| NoMemoryOrdering | 61.98 ms | 0.923 ms | 0.051 ms | 61.93 ms | 62.03 ms |
//| WithMemoryOrdering | 182.23 ms | 12.433 ms | 0.681 ms | 181.50 ms | 182.85 ms |

namespace MemoryOrderingBench
{
    [ShortRunJob]
    [MinColumn, MaxColumn]
    public class Measure
    {
        const int iterations = 10000000; // 演算を行う回数
        static int sharedValue = 0;

        [Benchmark]
        public void NoMemoryOrdering()
        {
            sharedValue = 0;
            Parallel.For(0, iterations, i =>
            {
                sharedValue++;
            });

            Console.WriteLine($"Final shared value without ordering: {sharedValue}");
        }
        [Benchmark]
        public void WithMemoryOrdering()
        {
            sharedValue = 0;
            Parallel.For(0, iterations, i =>
            {
                Interlocked.Increment(ref sharedValue); // 原子性を保証する
            });

            Console.WriteLine($"Final shared value with correct synchronization: {sharedValue}");
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
