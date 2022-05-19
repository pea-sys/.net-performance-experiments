using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;

namespace Inside_LINQ
{
    public class Program
    {   /* ILを見ると、LINQはtry_finally句が入っている。Enumerableを使用している。メモリ割り当ては避けられない。
         * 単体処理で見た場合にパフォーマンスは直接的な実装よりも落ちるが、コードの可読性を上げたい場合に使用するのが良いと考える
         * 遅いと言えば遅いかもしれないが、気にならないレベル
        /*
         * |  Method |      Mean |     Error |   StdDev |       Min |       Max |  Gen 0 | Allocated |
           |-------- |----------:|----------:|---------:|----------:|----------:|-------:|----------:|
           |    LINQ | 135.82 ns | 14.193 ns | 0.778 ns | 135.25 ns | 136.71 ns | 0.0305 |      48 B |
           | No_LINQ |  16.25 ns |  0.333 ns | 0.018 ns |  16.23 ns |  16.27 ns |      - |         - |
        */
        [MemoryDiagnoser]
        [ShortRunJob]
        [MinColumn, MaxColumn]
        public class ArrayWhere
        {
            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0, 10, 11, 12, -1 };
            int tmp = 0;

            #region IL
            /*
            .method public hidebysig instance int32  LINQ() cil managed
            {
            .custom instance void [BenchmarkDotNet.Annotations]BenchmarkDotNet.Attributes.BenchmarkAttribute::.ctor() = ( 01 00 00 00 ) 
              // コード サイズ       86 (0x56)
              .maxstack  3
              .locals init ([0] class [mscorlib]System.Collections.Generic.IEnumerator`1<int32> V_0,
                       [1] int32 x)
              IL_0000:  ldarg.0
              IL_0001:  ldfld      int32[] Inside_LINQ.Program/ArrayWhere::numbers
              IL_0006:  ldsfld     class [mscorlib]System.Func`2<int32,bool> Inside_LINQ.Program/ArrayWhere/'<>c'::'<>9__2_0'
              IL_000b:  dup
              IL_000c:  brtrue.s   IL_0025
              IL_000e:  pop
              IL_000f:  ldsfld     class Inside_LINQ.Program/ArrayWhere/'<>c' Inside_LINQ.Program/ArrayWhere/'<>c'::'<>9'
              IL_0014:  ldftn      instance bool Inside_LINQ.Program/ArrayWhere/'<>c'::'<LINQ>b__2_0'(int32)
              IL_001a:  newobj     instance void class [mscorlib]System.Func`2<int32,bool>::.ctor(object,
                                                                                                  native int)
              IL_001f:  dup
              IL_0020:  stsfld     class [mscorlib]System.Func`2<int32,bool> Inside_LINQ.Program/ArrayWhere/'<>c'::'<>9__2_0'
              IL_0025:  call       class [mscorlib]System.Collections.Generic.IEnumerable`1<!!0> [System.Core]System.Linq.Enumerable::Where<int32>(class [mscorlib]System.Collections.Generic.IEnumerable`1<!!0>,
                                                                                                                                                   class [mscorlib]System.Func`2<!!0,bool>)
              IL_002a:  callvirt   instance class [mscorlib]System.Collections.Generic.IEnumerator`1<!0> class [mscorlib]System.Collections.Generic.IEnumerable`1<int32>::GetEnumerator()
              IL_002f:  stloc.0
              .try
              {
                IL_0030:  br.s       IL_0040
                IL_0032:  ldloc.0
                IL_0033:  callvirt   instance !0 class [mscorlib]System.Collections.Generic.IEnumerator`1<int32>::get_Current()
                IL_0038:  stloc.1
                IL_0039:  ldarg.0
                IL_003a:  ldloc.1
                IL_003b:  stfld      int32 Inside_LINQ.Program/ArrayWhere::tmp
                IL_0040:  ldloc.0
                IL_0041:  callvirt   instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
                IL_0046:  brtrue.s   IL_0032
                IL_0048:  leave.s    IL_0054
              }  // end .try
              finally
              {
                IL_004a:  ldloc.0
                IL_004b:  brfalse.s  IL_0053
                IL_004d:  ldloc.0
                IL_004e:  callvirt   instance void [mscorlib]System.IDisposable::Dispose()
                IL_0053:  endfinally
              }  // end handler
              IL_0054:  ldc.i4.0
              IL_0055:  ret
            } // end of method ArrayWhere::LINQ
            */
            #endregion
            [Benchmark]
            public int LINQ()
            {
                var lowNums = from num in numbers
                              where num < 5
                              select num;

                foreach (var x in lowNums)
                {
                    tmp = x;
                }
                return 0;
            }

            #region IL
            /*
            .method public hidebysig instance int32  No_LINQ() cil managed
{
            .custom instance void [BenchmarkDotNet.Annotations]BenchmarkDotNet.Attributes.BenchmarkAttribute::.ctor() = ( 01 00 00 00 ) 
            // コード サイズ       38 (0x26)
            .maxstack  2
            .locals init ([0] int32[] V_0,
                     [1] int32 V_1,
                     [2] int32 x)
            IL_0000:  ldarg.0
            IL_0001:  ldfld      int32[] Inside_LINQ.Program/ArrayWhere::numbers
            IL_0006:  stloc.0
            IL_0007:  ldc.i4.0
            IL_0008:  stloc.1
            IL_0009:  br.s       IL_001e
            IL_000b:  ldloc.0
            IL_000c:  ldloc.1
            IL_000d:  ldelem.i4
            IL_000e:  stloc.2
            IL_000f:  ldloc.2
            IL_0010:  ldc.i4.5
            IL_0011:  bge.s      IL_001a
            IL_0013:  ldarg.0
            IL_0014:  ldloc.2
            IL_0015:  stfld      int32 Inside_LINQ.Program/ArrayWhere::tmp
            IL_001a:  ldloc.1
            IL_001b:  ldc.i4.1
            IL_001c:  add
            IL_001d:  stloc.1
            IL_001e:  ldloc.1
            IL_001f:  ldloc.0
            IL_0020:  ldlen
            IL_0021:  conv.i4
            IL_0022:  blt.s      IL_000b
            IL_0024:  ldc.i4.0
            IL_0025:  ret
            // end of method ArrayWhere::No_LINQ
             */
            #endregion
            [Benchmark]
            public int No_LINQ()
            {
                foreach (var x in numbers)
                {
                    if (x < 5)
                    {
                        tmp = x;
                    }
                }
                return 0;
            }
        }
        /*
         * |  Method |     Mean |      Error |    StdDev |        Min |      Max | Allocated |
           |-------- |---------:|-----------:|----------:|-----------:|---------:|----------:|
           |    LINQ | 334.3 us | 6,336.9 us | 347.35 us |   2.639 us | 695.4 us |     104 B |
           | No_LINQ | 164.5 us |   942.3 us |  51.65 us | 104.829 us | 194.8 us |      66 B |
         */
        [MemoryDiagnoser]
        [ShortRunJob]
        [MinColumn, MaxColumn]
        public class ArraySum
        {
            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0, 10, 11, 12, -1 };
            [Benchmark]
            public int LINQ()
            {
                Console.WriteLine(numbers.Sum().ToString());
                return 0;
            }
            [Benchmark]
            public int No_LINQ()
            {
                var sum = 0;
                foreach (var x in numbers)
                {
                    sum += x;
                }
                Console.WriteLine(sum.ToString());
                return 0;
            }
        }

        /*
         *|  Method |      Mean |    Error |   StdDev |       Min |       Max |  Gen 0 | Allocated |
          |-------- |----------:|---------:|---------:|----------:|----------:|-------:|----------:|
          |    LINQ | 200.45 ns | 8.491 ns | 0.465 ns | 199.93 ns | 200.83 ns | 0.0458 |      72 B |
          | No_LINQ |  65.92 ns | 8.074 ns | 0.443 ns |  65.42 ns |  66.26 ns |      - |         - |
         */
        [MemoryDiagnoser]
        [ShortRunJob]
        [MinColumn, MaxColumn]
        public class ListWhere
        {
            List<int> numbers = new List<int> { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0, 10, 11, 12, -1 };
            int tmp = 0;
            #region IL
            /*
            .method public hidebysig instance int32  LINQ() cil managed
            {
             .custom instance void [BenchmarkDotNet.Annotations]BenchmarkDotNet.Attributes.BenchmarkAttribute::.ctor() = ( 01 00 00 00 ) 
            // コード サイズ       86 (0x56)
            .maxstack  3
            .locals init ([0] class [mscorlib]System.Collections.Generic.IEnumerator`1<int32> V_0,
            [1] int32 x)
            IL_0000:  ldarg.0
            IL_0001:  ldfld      class [mscorlib]System.Collections.Generic.List`1<int32> Inside_LINQ.Program/ListWhere::numbers
            IL_0006:  ldsfld     class [mscorlib]System.Func`2<int32,bool> Inside_LINQ.Program/ListWhere/'<>c'::'<>9__2_0'
            IL_000b:  dup
            IL_000c:  brtrue.s   IL_0025
            IL_000e:  pop
            IL_000f:  ldsfld     class Inside_LINQ.Program/ListWhere/'<>c' Inside_LINQ.Program/ListWhere/'<>c'::'<>9'
            IL_0014:  ldftn      instance bool Inside_LINQ.Program/ListWhere/'<>c'::'<LINQ>b__2_0'(int32)
            IL_001a:  newobj     instance void class [mscorlib]System.Func`2<int32,bool>::.ctor(object,
                                                                                      native int)
            IL_001f:  dup
            IL_0020:  stsfld     class [mscorlib]System.Func`2<int32,bool> Inside_LINQ.Program/ListWhere/'<>c'::'<>9__2_0'
            IL_0025:  call       class [mscorlib]System.Collections.Generic.IEnumerable`1<!!0> [System.Core]System.Linq.Enumerable::Where<int32>(class [mscorlib]System.Collections.Generic.IEnumerable`1<!!0>,
                                                                                                                                       class [mscorlib]System.Func`2<!!0,bool>)
            IL_002a:  callvirt   instance class [mscorlib]System.Collections.Generic.IEnumerator`1<!0> class [mscorlib]System.Collections.Generic.IEnumerable`1<int32>::GetEnumerator()
            IL_002f:  stloc.0
            .try
            {
            IL_0030:  br.s       IL_0040
            IL_0032:  ldloc.0
            IL_0033:  callvirt   instance !0 class [mscorlib]System.Collections.Generic.IEnumerator`1<int32>::get_Current()
            IL_0038:  stloc.1
            IL_0039:  ldarg.0
            IL_003a:  ldloc.1
            IL_003b:  stfld      int32 Inside_LINQ.Program/ListWhere::tmp
            IL_0040:  ldloc.0
            IL_0041:  callvirt   instance bool [mscorlib]System.Collections.IEnumerator::MoveNext()
            IL_0046:  brtrue.s   IL_0032
            IL_0048:  leave.s    IL_0054
            }  // end .try
            finally
            {
            IL_004a:  ldloc.0
            IL_004b:  brfalse.s  IL_0053
            IL_004d:  ldloc.0
            IL_004e:  callvirt   instance void [mscorlib]System.IDisposable::Dispose()
            IL_0053:  endfinally
            }  // end handler
            IL_0054:  ldc.i4.0
            IL_0055:  ret
            } // end of method ListWhere::LINQ
             */
            #endregion
            [Benchmark]
            public int LINQ()
            {
                var lowNums = from num in numbers
                              where num < 5
                              select num;

                foreach (var x in lowNums)
                {
                    tmp = x;
                }
                return 0;
            }

            #region IL
            /*
             *.method public hidebysig instance int32  No_LINQ() cil managed
            {
            .custom instance void [BenchmarkDotNet.Annotations]BenchmarkDotNet.Attributes.BenchmarkAttribute::.ctor() = ( 01 00 00 00 ) 
            // コード サイズ       60 (0x3c)
            .maxstack  2
            .locals init ([0] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<int32> V_0,
            [1] int32 x)
            IL_0000:  ldarg.0
            IL_0001:  ldfld      class [mscorlib]System.Collections.Generic.List`1<int32> Inside_LINQ.Program/ListWhere::numbers
            IL_0006:  callvirt   instance valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<!0> class [mscorlib]System.Collections.Generic.List`1<int32>::GetEnumerator()
            IL_000b:  stloc.0
            .try
            {
            IL_000c:  br.s       IL_0021
            IL_000e:  ldloca.s   V_0
            IL_0010:  call       instance !0 valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<int32>::get_Current()
            IL_0015:  stloc.1
            IL_0016:  ldloc.1
            IL_0017:  ldc.i4.5
            IL_0018:  bge.s      IL_0021
            IL_001a:  ldarg.0
            IL_001b:  ldloc.1
            IL_001c:  stfld      int32 Inside_LINQ.Program/ListWhere::tmp
            IL_0021:  ldloca.s   V_0
            IL_0023:  call       instance bool valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<int32>::MoveNext()
            IL_0028:  brtrue.s   IL_000e
            IL_002a:  leave.s    IL_003a
             }  // end .try
            finally
            {
            IL_002c:  ldloca.s   V_0
            IL_002e:  constrained. valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<int32>
            IL_0034:  callvirt   instance void [mscorlib]System.IDisposable::Dispose()
            IL_0039:  endfinally
            }  // end handler
            IL_003a:  ldc.i4.0
            IL_003b:  ret
            } // end of method ListWhere::No_LINQ
            */
            #endregion
            [Benchmark]
            public int No_LINQ()
            {
                foreach (var x in numbers)
                {
                    if (x < 5)
                    {
                        tmp = x;
                    }
                }
                return 0;
            }
        }

        /*
         * |  Method |     Mean |      Error |    StdDev |        Min |      Max | Allocated |
           |-------- |---------:|-----------:|----------:|-----------:|---------:|----------:|
           |    LINQ | 337.5 us | 6,586.6 us | 361.03 us |   2.267 us | 719.7 us |     112 B |
           | No_LINQ | 167.3 us |   952.6 us |  52.21 us | 107.028 us | 198.6 us |      66 B |
         */
        [MemoryDiagnoser]
        [ShortRunJob]
        [MinColumn, MaxColumn]
        public class ListSum
        {
            List<int> numbers = new List<int> { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0, 10, 11, 12, -1 };

            /*
             * .method public hidebysig instance int32  LINQ() cil managed
                {
            .custom instance void [BenchmarkDotNet.Annotations]BenchmarkDotNet.Attributes.BenchmarkAttribute::.ctor() = ( 01 00 00 00 ) 
            // コード サイズ       26 (0x1a)
            .maxstack  1
            .locals init ([0] int32 V_0)
             IL_0000:  ldarg.0
             IL_0001:  ldfld      class [mscorlib]System.Collections.Generic.List`1<int32> Inside_LINQ.Program/ListSum::numbers
             IL_0006:  call       int32 [System.Core]System.Linq.Enumerable::Sum(class [mscorlib]System.Collections.Generic.IEnumerable`1<int32>)
             IL_000b:  stloc.0
             IL_000c:  ldloca.s   V_0
             IL_000e:  call       instance string [mscorlib]System.Int32::ToString()
             IL_0013:  call       void [mscorlib]System.Console::WriteLine(string)
             IL_0018:  ldc.i4.0
             IL_0019:  ret
            } // end of method ListSum::LINQ
             */
            [Benchmark]
            public int LINQ()
            {
                Console.WriteLine(numbers.Sum().ToString());
                return 0;
            }
            /*
             * .method public hidebysig instance int32  No_LINQ() cil managed
            {
             .custom instance void [BenchmarkDotNet.Annotations]BenchmarkDotNet.Attributes.BenchmarkAttribute::.ctor() = ( 01 00 00 00 ) 
            // コード サイズ       67 (0x43)
             .maxstack  2
             .locals init ([0] int32 sum,
              [1] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<int32> V_1,
             [2] int32 x)
             IL_0000:  ldc.i4.0
             IL_0001:  stloc.0
             IL_0002:  ldarg.0
             IL_0003:  ldfld      class [mscorlib]System.Collections.Generic.List`1<int32> Inside_LINQ.Program/ListSum::numbers
             IL_0008:  callvirt   instance valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<!0> class [mscorlib]System.Collections.Generic.List`1<int32>::GetEnumerator()
             IL_000d:  stloc.1
             .try
             {
             IL_000e:  br.s       IL_001c
             IL_0010:  ldloca.s   V_1
             IL_0012:  call       instance !0 valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<int32>::get_Current()
             IL_0017:  stloc.2
             IL_0018:  ldloc.0
             IL_0019:  ldloc.2
             IL_001a:  add
             IL_001b:  stloc.0
             IL_001c:  ldloca.s   V_1
             IL_001e:  call       instance bool valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<int32>::MoveNext()
             IL_0023:  brtrue.s   IL_0010
             IL_0025:  leave.s    IL_0035
             }  // end .try
             finally
             {
             IL_0027:  ldloca.s   V_1
             IL_0029:  constrained. valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<int32>
             IL_002f:  callvirt   instance void [mscorlib]System.IDisposable::Dispose()
             IL_0034:  endfinally
             }  // end handler
             IL_0035:  ldloca.s   sum
             IL_0037:  call       instance string [mscorlib]System.Int32::ToString()
             IL_003c:  call       void [mscorlib]System.Console::WriteLine(string)
             IL_0041:  ldc.i4.0
             IL_0042:  ret
            } // end of method ListSum::No_LINQ
             */
            [Benchmark]
            public int No_LINQ()
            {
                var sum = 0;
                foreach (var x in numbers)
                {
                    sum += x;
                }
                Console.WriteLine(sum.ToString());
                return 0;
            }
        }

        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<ArrayWhere>();
            summary = BenchmarkRunner.Run<ArraySum>();
            summary = BenchmarkRunner.Run<ListWhere>();
            summary = BenchmarkRunner.Run<ListSum>();
        }
    }
}
