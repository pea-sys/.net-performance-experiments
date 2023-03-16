using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace GC_LocalRoute
{
    internal class Program
    {
        /// <summary>
        /// [timerインスタンス]
        /// DEBUGモードだと最適化されないのでtimerはGCの対象にならない。動作し続ける
        /// RELEASEモードだと最適化されるのでtimerはGCの対象になる。1回動作して解放される
        /// [keeptimeインスタンス]
        /// メソッドの最後にインライン化されないメソッドで参照しているため、メソッドの途中で解放されません
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Timer timer = new Timer(OnTimer, null, 0, 1000);
            Timer keepTimer = new Timer(OnKeepTimer,null, 0, 1000);
            Console.ReadLine();
            MyKeepAlive(keepTimer);
        }
        static void OnTimer(object state)
        {
            Console.WriteLine("[timer]" + DateTime.Now.TimeOfDay);
            GC.Collect();
        }
        static void OnKeepTimer(object state)
        {
            Console.WriteLine("[keepTimer]" + DateTime.Now.TimeOfDay);
            GC.Collect();
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        static void MyKeepAlive(object obj) { }
    }
}
