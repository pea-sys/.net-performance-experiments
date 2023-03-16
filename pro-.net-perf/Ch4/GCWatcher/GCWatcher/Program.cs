using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GCWatcher
{
    public class GCEventArgs : EventArgs
    {
        public GCEventArgs(long elapsedMilliseconds)
        {
            ElapsedMilliseconds = elapsedMilliseconds;
        }

        public long ElapsedMilliseconds { get; set; }
    }
    public class GCWatcher
    {
        private Thread watcherThread;
        private Stopwatch sw = new Stopwatch();
        public event EventHandler GCApproaches;
        public event EventHandler GCComplete;

        public void Watch()
        {
            GC.RegisterForFullGCNotification(50, 50);
            watcherThread = new Thread(() =>
            {
                while (true)
                {
                    GCNotificationStatus status = GC.WaitForFullGCApproach();

                    //Omitted error handling code here
                    if (GCApproaches != null)
                    {
                        sw.Restart();
                        GCApproaches(this, new GCEventArgs(sw.ElapsedMilliseconds));
                    }
                    status = GC.WaitForFullGCComplete();
                    //Omitted error handling code here
                    if (GCComplete != null)
                    {
                        GCComplete(this, new GCEventArgs(sw.ElapsedMilliseconds));
                    }
                }
            });
            watcherThread.IsBackground = true;
            watcherThread.Start();
        }

        public void Cancel()
        {
            GC.CancelFullGCNotification();
            watcherThread.Join();
        }
    }
    /// <summary>
    /// [Output]
    /// サーバーGC判定:False
    /// GCLatencyMode:Batch
    /// GC回数=0  GC開始 0msec
    /// GC回数=1  GC完了 15msec
    /// GC回数 = 1  GC開始 0msec
    /// GC回数 = 2  GC完了 23msec
    /// GC回数 = 2  GC開始 0msec
    /// GC回数 = 3  GC完了 29msec
    /// GC回数 = 3  GC開始 0msec
    /// GC回数 = 4  GC完了 49msec
    /// GC回数 = 4  GC開始 0msec
    /// GC回数 = 5  GC完了 82msec
    /// GC回数 = 5  GC開始 0msec
    /// GC回数 = 6  GC完了 141msec
    /// GC回数 = 6  GC開始 0msec
    /// GC回数 = 7  GC完了 242msec
    /// 終了
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            //GCSettings.LatencyMode = GCLatencyMode.Batch;
            Console.WriteLine($"サーバーGC判定:{GCSettings.IsServerGC}");
            Console.WriteLine($"GCLatencyMode:{GCSettings.LatencyMode}");
            GCWatcher watcher = new GCWatcher();
            watcher.GCComplete += Watcher_GCComplete;
            watcher.GCApproaches += Watcher_GCApproaches;
            watcher.Watch();
            DoSomething();
            Console.WriteLine("終了");
            Console.ReadLine();
        }

        private static void Watcher_GCApproaches(object sender, EventArgs e)
        {
            Console.WriteLine($"GC回数={GC.CollectionCount(GC.MaxGeneration)}  GC開始 {((GCEventArgs)e).ElapsedMilliseconds}msec");
        }

        private static void Watcher_GCComplete(object sender, EventArgs e)
        {
            Console.WriteLine($"GC回数={GC.CollectionCount(GC.MaxGeneration)}  GC完了 {((GCEventArgs)e).ElapsedMilliseconds}msec");
        }
        static void DoSomething()
        {
            List<Thing> list = new List<Thing>();
            for (int i = 0; i < 1000; i++)
            {
                for (int j = 0; j < 1000; j++)
                {
                    list.Add(new Thing());
                }
            }
        }
        class Thing
        {
            private byte[] data = new byte[1024];
            public Thing()
            {

            }

        }

    }
}
