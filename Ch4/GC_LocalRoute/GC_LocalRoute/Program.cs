using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace GC_LocalRoute
{
    internal class Program
    {
        /// <summary>
        /// DEBUGモードだと最適化されないのでtimerはGCの対象にならない。動作し続ける
        /// RELEASEモードだと最適化されるのでtimerはGCの対象になる。1回動作して解放される
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Timer timer = new Timer(OnTimer, null, 0, 1000);
            Console.ReadLine();
        }
        static void OnTimer(object state)
        {
            Console.WriteLine(DateTime.Now.TimeOfDay);
            GC.Collect();
        }
    }
}
