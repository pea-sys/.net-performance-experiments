using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;

namespace NullConditionalOperator
{
    public class Program
    {
        /// <summary>
        /// パフォーマンス面では三項演算子もnull条件演算子に差異はないと言っていい。
        /// null条件演算子はスレッドセーフなので、null条件演算子を使用したほうが安心出来る。
        /// |                  Method |     Mean |   Error |   StdDev |      Min |      Max |      Gen 0 | Allocated |
        ///|------------------------ |---------:|--------:|---------:|---------:|---------:|-----------:|----------:|
        ///| NullConditionalOperator | 202.2 ms | 7.42 ms | 21.88 ms | 159.4 ms | 247.9 ms | 17333.3333 |     26 MB |
        ///|         TernaryOperator | 188.0 ms | 5.95 ms | 17.34 ms | 159.6 ms | 235.2 ms | 17333.3333 |     26 MB |
        /// </summary>
        //[MemoryDiagnoser]
        //[MinColumn, MaxColumn]
        public class Test
        {
            private const int N = 1000000;

            [Benchmark]
            public void NullConditionalOperator()
            {
                var result = string.Empty;
                for (int? i = 0; i < N; i++)
                {
                    result = i?.ToString();
                }
            }

            [Benchmark]
            public void TernaryOperator()
            {
                var result = string.Empty;
                for (int? i = 0; i < N; i++)
                {
                    result = i.HasValue ? i.ToString() : null;
                }
            }
        }
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Test>();
        }
    }
}
