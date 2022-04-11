using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListLoopPerformance
{
    internal class Program
    {
        /// <summary>
        /// WhileとGotoが早すぎるように見える。
        /// ベンチマークの取り方がおかしいかも。
        /// For : 170msec
        /// Foreach : 278msec
        /// List<T>.ForEach : 152msec
        /// While : 1msec
        /// GoTo : 1msec
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
            int index = 0;
            Stopwatch sw = new Stopwatch();

            var list = Enumerable.Repeat("a", INNER_LOOP).ToList();
            for (int j = 0; j < OUTER_LOOP; j++)
            {
                sw.Restart();
                if (mode == 0)
                {
                    for (index = 0; index < list.Count - 1; index++)
                    {
                        _ = list[index];
                    }
                }
                else if (mode == 1)
                {
                    foreach (string l in list)
                    {
                        _ = l;
                    }
                }
                else if (mode == 2)
                {
                    list.ForEach(x => { _ = x; });
                }
                else if (mode == 3)
                {
                    while (list.Count > index)
                    {
                        _ = list[index++];
                    }
                }
                else if (mode == 4)
                {
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
                spent_time += sw.ElapsedMilliseconds;
            }
            
            spent_time /= OUTER_LOOP;
            Console.WriteLine($"{spent_time}msec");
            Console.ReadLine();
        }
    }
}
