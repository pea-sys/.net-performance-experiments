using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Disassemblers;
using BenchmarkDotNet.Running;
using CommandLine;

//| Method | Mean | Error | StdDev | Min | Max |
//| ----------------- | ---------:| ----------:| --------:| ---------:| ---------:|
//| EasyPredict | 194.9 ms | 51.69 ms | 2.83 ms | 192.4 ms | 198.0 ms |
//| DifficultPredict | 430.0 ms | 112.65 ms | 6.17 ms | 423.2 ms | 435.2 ms |
namespace BranchPredictionBench
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
        public void EasyPredict()
        {
            int sum1 = 0;
            for (int i = 0; i < iterations; i++)
            {
                if (data[i] < 9) //9割真
                {
                    sum1 += data[i];
                }
                else
                {
                    sum1 -= 1;
                }
            }
        }
        [Benchmark]
        public void DifficultPredict()
        {
            int sum2 = 0;
            for (int i = 0; i < iterations; i++)
            {
                if (data[i] >= 5) //半分真
                {
                    sum2 += data[i];
                }
                else
                {
                    sum2 -= 1;
                }
            }
        }
    }
    internal class Program
    {
        static void Main()
        {
            var summary = BenchmarkRunner.Run<Measure>();
            Console.ReadKey();
        }
    }
}
