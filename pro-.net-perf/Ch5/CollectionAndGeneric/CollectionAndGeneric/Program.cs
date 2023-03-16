using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CollectionAndGeneric
{
    internal class Program
    {
        /// <summary>
        /// ArrayList(公式非推奨) 非タイプセーフ
        /// List<T>(公式推奨)　タイプセーフ
        /// 
        /// ジェネリックに分がある
        /// タイプセーフという利点もある
        /// ArrayListを使う理由は見当たらない
        /// 
        /// [Output]
        /// 32bit Process
        /// [ArrayList] ElapsedTicks = 386 Memory = 4000032byte
        /// [List]      ElapsedTicks = 366 Memory = 4000032byte
        /// 
        /// 64bit Process
        /// [ArrayList] ElapsedTicks = 345 Memory = 8000056byte
        /// [List]      ElapsedTicks = 265 Memory = 4000056byte
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("0 : ArrayListTest");
            Console.WriteLine("1 : ListTest");
            Console.Write("Select Mode : ");
            int mode = Int32.Parse(Console.ReadLine());
            if (Environment.Is64BitProcess)
            {
                Console.WriteLine("64bit Process");
            }
            else
            {
                Console.WriteLine("32bit Process");
            }

            Stopwatch sw = new Stopwatch();
            long offsetMemory = GC.GetTotalMemory(false);
            ArrayList arrayList = new ArrayList();
            List<int> list = new List<int>();
            sw.Start();
            if (mode == 0)
            {
                arrayList = new ArrayList(1000000);
               
                for (int i = 0; i < arrayList.Count; ++i)
                {
                    arrayList.Add(i);
                }
            }
            else if (mode == 1)
            {
                list = new List<int>(1000000);
                for (int i = 0; i < list.Count; ++i)
                {
                    list.Add(i);
                }
            }
            else
            {
                Console.WriteLine("Not Supported");
            }
            sw.Stop();
            Console.WriteLine($"ElapsedTicks = {sw.ElapsedTicks} Memory = {GC.GetTotalMemory(false) - offsetMemory}byte");
            Console.ReadLine();
        }
    }
}
