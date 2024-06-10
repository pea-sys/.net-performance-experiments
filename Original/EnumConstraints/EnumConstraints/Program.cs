
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace NullConditionalOperator
{
    public class Program
    {
        /*
        | Method         | Mean     | Error    | StdDev   | Allocated |
        |--------------- |---------:|---------:|---------:|----------:|
        | NonConstraints | 900.0 ns | 638.2 ns | 34.98 ns |     168 B |
        | Constraints    | 735.7 ns | 419.2 ns | 22.98 ns |     168 B |
        */
        public enum Season
        {
            Spring,
            Summer,
            Autumn,
            Winter
        }
        [ShortRunJob]
        [MemoryDiagnoser(false)]
        public class Test
        {
            private const int N = 5000000;
            public void NonConstraints(Season Val)
            {

            }
            public void Constraints<T>(T Val) where T : struct, Enum
            {
            }
            [Benchmark]
            public void NonConstraints()
            {
                foreach (Season x in Enum.GetValues(typeof(Season)))
                {
                    NonConstraints(x);
                }
            }
            [Benchmark]
            public void Constraints()
            {
                foreach (Season x in Enum.GetValues(typeof(Season)))
                {
                    Constraints<Season>(x);
                }
            }
        }


        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Test>();
        }
    }
}