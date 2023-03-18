using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;

namespace StringVsStringBuilder
{
    [HtmlExporter]
    [MemoryDiagnoser]
    [MinColumn, MaxColumn]
    public class Test
    {
        private const int N = 10000;
        private const string s = "0";

        [Benchmark]
        public void @String()
        {
            string ss = "";
            for (int i = 0; i < N; i++)
            {
                ss +=s;
            }
        }

        [Benchmark]
        public void @StringBuilder()
        {
            StringBuilder sb = new StringBuilder();
            string ss = "";
            for (int i = 0; i < N; i++)
            {
                sb.Append(s);
            }
            ss = sb.ToString();
        }

        [Benchmark]
        public void StringBuilderEx()
        {
            StringBuilder sb = new StringBuilder();
            string ss = "";
            for (int i = 0; i < N / 2; i++)
            {
                sb.Append(s + 2);
            }
            ss = sb.ToString();
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Test>();
        }
    }
}
