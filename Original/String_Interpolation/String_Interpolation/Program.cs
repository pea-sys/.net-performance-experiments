using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;

namespace String_Interpolation
{
    [MemoryDiagnoser]
    [ShortRunJob]
    [MinColumn, MaxColumn]
    public class Test
    {
        private const int N = 10000;

        [Benchmark]
        public void Boxing()
        {
            for (int i = 0; i < N; i++)
            {
                string a = $"{Math.PI}";
            }
        }

        [Benchmark]
        public void NotBoxing()
        {
            for (int i = 0; i < N; i++)
            {
                string a = $"{Math.PI.ToString()}";
            }
        }
    }
    /// <summary>
    /// .method private hidebysig static void  Main(string[] args) cil managed
    ///{
    ///  .entrypoint
    ///  // コード サイズ       68 (0x44)
    ///  .maxstack  2
    ///  .locals init([0] string a,
    ///           [1] string b,
    ///           [2] float64 V_2)
    ///  IL_0000:  nop
    ///  IL_0001:  ldstr      "{0}"
    ///  IL_0006:  ldc.r8     3.1415926535897931
    ///  IL_000f:  box[mscorlib] System.Double
    ///  IL_0014:  call       string[mscorlib] System.String::Format(string,
    ///                                                              object)
    ///  IL_0019:  stloc.0
    ///  IL_001a:  ldstr      "`"
    ///  IL_001f:  ldc.r8     3.1415926535897931
    ///  IL_0028:  stloc.2
    ///  IL_0029:  ldloca.s V_2
    ///  IL_002b:  call instance string[mscorlib] System.Double::ToString()
    ///  IL_0030:  call       string[mscorlib] System.String::Concat(string,
    ///                                                              string)
    ///  IL_0035:  stloc.1
    ///  IL_0036:  ldloc.0
    ///  IL_0037:  ldloc.1
    ///  IL_0038:  call       string[mscorlib] System.String::Concat(string,
    ///                                                              string)
    ///  IL_003d:  call       void[mscorlib] System.Console::WriteLine(string)
    ///  IL_0042:  nop
    ///  IL_0043:  ret
    ///} // end of method Program::Main
    ///
    /// 
    /// Benchmark結果
    /// Job=ShortRun  IterationCount=3  LaunchCount=1
    ///    WarmupCount=3
    ///|    Method |     Mean |    Error |    StdDev |      Min |      Max |    Gen 0 | Allocated |
    ///|---------- |---------:|---------:|----------:|---------:|---------:|---------:|----------:|
    ///|    Boxing | 5.061 ms | 5.105 ms | 0.2798 ms | 4.832 ms | 5.373 ms | 710.9375 |  1,097 KB |
    ///| NotBoxing | 3.216 ms | 8.170 ms | 0.4478 ms | 2.947 ms | 3.733 ms | 304.6875 |    469 KB |
    /// 
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// 補完文字列のIL確認
        /// ToStringをしない場合、ボックス化が発生する
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string a = $"{Math.PI}";
            string b = $"`{Math.PI.ToString()}";
            Console.WriteLine($"{a}{b}");
            var summary = BenchmarkRunner.Run<Test>();
        }
    }
}
