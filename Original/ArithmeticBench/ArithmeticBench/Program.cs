using System;
using BenchmarkDotNet.Attributes;
using System.Diagnostics;
using BenchmarkDotNet.Running;
using Microsoft.Diagnostics.Runtime.Utilities;
//| Method | Mean | Error | StdDev | Min | Max |
//| ------- | ---------:| ---------:| --------:| ---------:| ---------:|
//| SUM | 112.4 ms | 99.45 ms | 5.45 ms | 107.6 ms | 118.4 ms |
//| MULTI | 159.6 ms | 46.44 ms | 2.55 ms | 156.9 ms | 161.9 ms |
//| DIV | 316.0 ms | 69.89 ms | 3.83 ms | 313.0 ms | 320.4 ms |

[ShortRunJob]
[MinColumn, MaxColumn]
public class Measure
{
    const int iterations = 100000000;

    private Random random;
    private double[] numbers;

    [GlobalSetup]
    public void GlobalSetup()
    {
        random = new Random(12345);
        numbers = new double[iterations];
        for (int i = 0; i < numbers.Length; i++)
        {
            numbers[i] = random.NextDouble()+1.0;
        }
    }
    [Benchmark]
    public void SUM()
    {
        double result = 0.0;
        for (int i = 0; i < iterations; i++)
        {
           
            result += numbers[i];
        }
    }

    [Benchmark]
    public void MULTI()
    {
        double result = 0.0;
        for (int i = 0; i < iterations; i++)
        {
            result *= numbers[i];
        }
    }
    [Benchmark]
    public void DIV()
    {
        double result = 0.0;
        for (int i = 0; i < iterations; i++)
        {
            result /= numbers[i];
        }
    }
}
class ArithmeticPerformanceTest
{
    static void Main()
    {

        var summary = BenchmarkRunner.Run<Measure>();
        Console.ReadKey();
    }
}