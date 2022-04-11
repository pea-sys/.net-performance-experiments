using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ListLoopPerformance
{
    internal class Program
    {
        /// <summary>
        /// WhileとGotoが早すぎるように見える。
        /// ベンチマークの取り方がおかしいかも。
        /// [32bit Process]
        /// For : 42msec
        /// Foreach : 278msec
        /// List<T>.ForEach : 152msec
        /// While : 67msec
        /// GoTo : 50msec
        /// 
        /// [64bit Process]
        /// For : 43msec
        /// Foreach : 405msec
        /// List<T>.ForEach :246msec
        /// While : 48msec
        /// GoTo : 52msec
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("0 : For");
            Console.WriteLine("1 : Foreach");
            Console.WriteLine("2 : List<T>.ForEach");
            Console.WriteLine("3 : While");
            Console.WriteLine("4 : GoTo");
            Console.Write("SelectLoopType : ");
            int mode = int.Parse(Console.ReadLine());

            int OUTER_LOOP = 100;
            int INNER_LOOP = 50000000;
            long spent_time = 0;

            Stopwatch sw = new Stopwatch();

            var list = Enumerable.Repeat("a", INNER_LOOP).ToList();
            for (int j = 0; j < OUTER_LOOP; j++)
            {
                sw.Restart();
                if (mode == 0)
                {
                    ForTest(list);
                }
                else if (mode == 1)
                {
                    ForeachTest(list);
                }
                else if (mode == 2)
                {
                    GenericForeachTest(list);
                }
                else if (mode == 3)
                {
                    WhileTest(list);
                }
                else if (mode == 4)
                {
                    GoToTest(list);
                }
                spent_time += sw.ElapsedMilliseconds;
            }
            
            spent_time /= OUTER_LOOP;
            Console.WriteLine($"{spent_time}msec");
            Console.ReadLine();
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ForTest(List<string> list)
        {
            for (int index = 0; index < list.Count - 1; index++)
            {
                _ = list[index];
            }
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ForeachTest(List<string> list)
        {
            foreach (string l in list)
            {
                _ = l;
            }
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void GenericForeachTest(List<string> list)
        {
            list.ForEach(x => { _ = x; });
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WhileTest(List<string> list)
        {
            int index = 0;
            while (list.Count > index)
            {
                _ = list[index++];
            }
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void GoToTest(List<string> list)
        {
            int index = 0;
            goto LOOPSTART;
        LOOPSTART:
            if (index < list.Count)
            {
                _ = list[index++];
                goto LOOPSTART;
            }
            else
            {
                goto LOOPEND;
            }
        LOOPEND:
            ;
        }
    }
}
