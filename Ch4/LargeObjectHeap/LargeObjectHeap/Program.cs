using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace LargeObjectHeap
{
    /// <summary>
    /// LOHは大きなオブジェクト用に確保される特殊な領域です
    /// 大きなオブジェクトはLOHに直接割り当てられるので、Gen領域で管理されません
    /// そのため、GCによるGen間の移動にかかる負荷が抑えられます
    /// 85,000 バイト以上の場合、大きなオブジェクトと見なされます。 
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// 32bitと64bitの差異はあまりない
        /// ラージオブジェクト作成後から使用メモリ約4700バイトの差異が発生するが、
        /// ラージオブジェクトの圧縮率は一緒
        /// GCがラージオブジェクトを圧縮する条件については「不確定な将来の時刻に発生します」と記載があるのみで詳細は不明
        /// 
        /// ■32bit Output
        /// 32bit Process
        /// LargeObjectSize:84988
        /// LO作成後の使用メモリ103032
        /// largeobject : size84988 gen2
        /// SO作成後の使用メモリ188044
        /// smallobject : size84987 gen0
        /// LOH強制圧縮指定なしのGC後の使用メモリ:9908
        /// LOH強制圧縮指定ありのGCの使用メモリ:9924
        ///
        /// ■64bit Output
        /// 64bit Process
        /// LargeObjectSize:84976
        /// LO作成後の使用メモリ107760
        /// largeobject : size84976 gen2
        /// SO作成後の使用メモリ192784
        /// smallobject : size84975 gen0
        /// LOH強制圧縮指定なしのGC後の使用メモリ:14536
        /// LOH強制圧縮指定ありのGCの使用メモリ:14512
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            GC.Collect();
            long memory_offset = GC.GetTotalMemory(true);
            // 32bitはオブジェクト管理(ヘッダーやメソッドテーブルポインタ等)で12バイト使ってる
            // 64bitはオブジェクト管理(ヘッダーやメソッドテーブルポインタ等)で24バイト使ってる
            // 32bit Process
            // LargeObjectSize: 84988
            // 64bit Process
            // LargeObjectSize: 84976
            Console.WriteLine((System.Environment.Is64BitProcess?"64bit Process":"32bit Process"));
            int largeObjectJudgeSize = 1;
            while(true)
            {
                byte[] obj = new byte[largeObjectJudgeSize];
                if (GC.GetGeneration(obj) == GC.MaxGeneration)break;
                largeObjectJudgeSize++;
            }
            Console.WriteLine($"LargeObjectSize:{largeObjectJudgeSize}");
            GC.Collect();


            // GCによりラージオブジェクト圧縮を強制しません
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.Default;

            byte[] largeObject = new byte[largeObjectJudgeSize];
            Console.WriteLine($"LO作成後の使用メモリ{GC.GetTotalMemory(false)- memory_offset}");
            Console.WriteLine($"largeobject : size{largeObjectJudgeSize} gen{GC.GetGeneration(largeObject)}");
            byte[] smallObject = new byte[largeObjectJudgeSize - 1];
            Console.WriteLine($"SO作成後の使用メモリ{GC.GetTotalMemory(false)- memory_offset}");
            Console.WriteLine($"smallobject : size{largeObjectJudgeSize - 1} gen{GC.GetGeneration(smallObject)}");

            GC.Collect();
            Console.WriteLine($"LOH強制圧縮指定なしのGC後の使用メモリ:{GC.GetTotalMemory(true)- memory_offset}");

            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect();
            Console.WriteLine($"LOH強制圧縮指定ありのGCの使用メモリ:{GC.GetTotalMemory(true)- memory_offset}");
            Console.ReadLine();
        }
    }
}
