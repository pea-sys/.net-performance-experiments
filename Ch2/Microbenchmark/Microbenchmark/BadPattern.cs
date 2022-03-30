using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbenchmark
{
    /// <summary>
    /// ダメな理由
    /// ・反復回数が500回と少ない
    /// ・コンパイラによる最適化を防いでいない。インライン展開してループとして実行されない可能性がある。
    /// ・メソッドの呼び出しコストも測定されている
    /// </summary>
    internal class BadPattern
    {
        internal class Employee
        {
            public void Work() { }
        }

        internal void BenchMark()
        {
            object obj = new Employee();
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < 500; i++)
            {
                Fragment1(obj);
            }
            Console.WriteLine(sw.ElapsedTicks);
            sw = Stopwatch.StartNew();
            for (int i = 0; i < 500; i++)
            {
                Fragment2(obj);
            }
            Console.WriteLine(sw.ElapsedTicks);
            Console.WriteLine("続行するには何かキーを押してください . . .");
            Console.ReadKey();
        }
        /// <summary>
        /// フラグメント1 安全にキャストしてからnullかどうか確認する
        /// </summary>
        /// <param name="obj"></param>
        static void Fragment1(object obj)
        {
            Employee emp = obj as Employee;
            if (emp != null)
            {
                emp.Work();
            }
        }
        /// <summary>
        /// フラグメント2 型を確認してからキャストする
        /// </summary>
        /// <param name="obj"></param>
        static void Fragment2(object obj)
        {
            if (obj is Employee)
            {
                Employee emp = obj as Employee;
                emp.Work();
            }
        }
    }
}
