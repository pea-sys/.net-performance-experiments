using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Disassemblers;
using BenchmarkDotNet.Running;
using CommandLine;



//| Method | Mean | Error | StdDev | Min | Max |
//| ------------------- | ---------:| ---------:| ---------:| ---------:| ---------:|
//| TrueDataDependency | 61.31 ms | 16.77 ms | 0.919 ms | 60.44 ms | 62.27 ms |
//| NoDataDependency | 59.47 ms | 20.83 ms | 1.142 ms | 58.62 ms | 60.77 ms |
namespace DependencyDataBench
{
    [ShortRunJob]
    [MinColumn, MaxColumn]
    public class Measure
    {
        const int iterations = 100000000;
        private int[] data;
        Random random;
        [GlobalSetup]
        public void Setup()
        {
            random = new Random(1234);
            data = new int[iterations];

            for (int i = 0; i < iterations; i++)
            {
                data[i] = random.Next(0, 10);
            }
        }
        [Benchmark]
        public void TrueDataDependency()
        {
            int result = 0;
            for (int i = 0; i < iterations; i++)
            {
                result = result + data[i];
            }
        }
        [Benchmark]
        public void NoDataDependency()
        {
            int result = 0;

            for (int i = 0; i < iterations; i++)
            {
               result = 2 + data[i];
            }
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
