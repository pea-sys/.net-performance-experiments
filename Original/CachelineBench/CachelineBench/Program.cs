using System;
using System.Diagnostics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;



//| Method | Mean | Error | StdDev | Min | Max |
//| ------------------ | ----------:| -----------:| ----------:| ----------:| ----------:|
//| RowMajorAccess | 2.615 ms | 0.7164 ms | 0.0393 ms | 2.590 ms | 2.660 ms |
//| ColumnMajorAccess | 17.801 ms | 30.6051 ms | 1.6776 ms | 16.113 ms | 19.468 ms |

namespace CachelineBench
{
    [ShortRunJob]
    [MinColumn, MaxColumn]
    public class Measure
    {
        PerformanceCounter cacheCounter = new PerformanceCounter("Memory", "Cache Faults/sec");
        const int Size = 1024; // 行列のサイズ
        static double[,] matrix = new double[Size, Size];


        [Benchmark]
        public void RowMajorAccess()
        {
            cacheCounter.NextValue();

            double sum = 0.0;
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    sum += matrix[i, j];
                }
            }

            float cacheFaults = cacheCounter.NextValue();
            Console.WriteLine($"Row Cache Faults/sec: {cacheFaults}");
        }
        [Benchmark]
        public void ColumnMajorAccess()
        {
            cacheCounter.NextValue();
            double sum = 0.0;
            for (int j = 0; j < Size; j++)
            {
                for (int i = 0; i < Size; i++)
                {
                    sum += matrix[i, j];
                }
            }

            float cacheFaults = cacheCounter.NextValue();
            Console.WriteLine($"Column Cache Faults/sec: {cacheFaults}");
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
