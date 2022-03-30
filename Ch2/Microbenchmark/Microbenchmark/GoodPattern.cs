using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Microbenchmark
{
    /// <summary>
    /// As - 497ms
    /// As - 484ms
    /// As - 560ms
    /// As - 459ms
    /// As - 451ms
    /// As - 458ms
    /// As - 454ms
    /// As - 463ms
    /// As - 460ms
    /// As - 453ms
    /// Is Then As - 457ms
    /// Is Then As - 443ms
    /// Is Then As - 452ms
    /// Is Then As - 444ms
    /// Is Then As - 463ms
    /// Is Then As - 470ms
    /// Is Then As - 442ms
    /// Is Then As - 504ms
    /// Is Then As - 516ms
    /// Is Then As - 507ms
    /// </summary>
    internal class GoodPattern
    {
        internal class Employee
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public void Work() { }
        }

        internal void BenchMark(object obj)
        {
            const int OUTER_ITERATIONS = 10;
            const int INNER_ITERATIONS = 100000000;

            // 信頼できる結果が得られるよう外側ループ回数を多くする
            for (int i = 0; i < OUTER_ITERATIONS; ++i)
            {
                Stopwatch sw = Stopwatch.StartNew();
                // 内部測定ループを何回も繰り返して長時間の実行を測定する
                for (int j = 0; j < INNER_ITERATIONS; ++j)
                {
                    Employee emp = obj as Employee;
                    if (emp != null)
                        emp.Work();
                }
                Console.WriteLine("As - {0}ms", sw.ElapsedMilliseconds);
            }
            for (int i = 0; i < OUTER_ITERATIONS; ++i)
            {
                Stopwatch sw = Stopwatch.StartNew();
                for (int j = 0; j < INNER_ITERATIONS; ++j)
                {
                    if (obj is Employee)
                    {
                        Employee emp = obj as Employee;
                        emp.Work();
                    }
                }
                Console.WriteLine("Is Then As - {0}ms", sw.ElapsedMilliseconds);
            }
            Console.WriteLine("続行するには何かキーを押してください . . .");
            Console.ReadKey();
        }
    }
}
