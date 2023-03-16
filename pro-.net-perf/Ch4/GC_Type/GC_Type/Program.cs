using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GC_Type
{
    /// <summary>
    /// 覚書：GCの種類を選択するのは簡単ではありません
    /// 　　　実験して選定すること
    /// </summary>
    internal class Program
    {
        const int LOOP = 1000000;
        const int OUTER_LOOP = 20;
        static void Main(string[] args)
        {
            // [ワークステーションGC]
            // 1つのスレッドでGCを実行します。並列実行はしません。
            // 同時実行と非同時実行の2種類が存在します。
            // App.configで設定
            // 1. 同時実行は既定のGCです。優先度=高の専用のGCスレッドで開始から終了まで実行します。
            //    マークフェーズの大変はアプリケーションスレッドと同時実行可能です。
            //    スイープフェーズはアプリケーションスレッドが中断します。
            //    UIアプリケーションではUIスレッドからGCが開始されないよう注意が必要。
            // 2. 非同時実行ワークステーションGCは、アプリケーションスレッドを中断します。

            // [サーバーGC]
            // App.configで設定
            // CLRがプロセッサヒープ間でのオブジェクト割り当てのバランスを保つようになります
            // 制限事項：プロセッサが1基しかない場合、サーバーGCアーキテクチャの効果が失われます
            Console.WriteLine($"サーバーGC判定:{GCSettings.IsServerGC}");
            Console.WriteLine($"プロセッサ数:{Environment.ProcessorCount}");
            // ワークステーションGCの場合
            if (!GCSettings.IsServerGC)
            {
                // コンソールアプリケーションはBatchが既定
                Console.WriteLine($"レイテンシモード既定:{GCSettings.LatencyMode.ToString()}");

                /* [Batch]
                 * ガベージ コレクションのコンカレンシーを無効にし、バッチ呼び出しでオブジェクトを解放します。
                 * これは、関与するレベルが最も高いモードです。 このモードは応答性と引き換えに最大のスループットを実現するように
                 * 設計されています。
                 * 
                 * [Interactive]
                 * ガベージ コレクションのコンカレンシーを有効にし、アプリケーションの実行中にオブジェクトを解放します。 
                 * これは、ワークステーション上のガベージ コレクションの既定のモードで、関与するレベルは Batch より低くなります。
                 * 
                 * [LowLatency]
                 * オブジェクトの解放時に、保守的なガベージ コレクションを有効にします。 システムがメモリ圧迫の状態でありながら、
                 * ジェネレーション 0 とジェネレーション 1 のコレクションが頻繁に発生する可能性がある場合のみ、
                 * すべてのコレクションが発生します。このモードは、サーバーのガベージ コレクターでは使用できません。
                 * 
                 * [SustainedLowLatency]
                 * 長期間にわたって待機時間を最小限に抑えることを試みるガベージ コレクションを有効にします。 
                 * コレクターは、生成 0、生成 1、同時実行生成 2 のコレクションのみ実行しようとしています。
                 * それでも、システムがメモリ圧迫の状態にある場合は、完全なブロック コレクションが発生することがあります。
                 */
                GCLatencyMode[] mode = { GCLatencyMode.Batch, GCLatencyMode.Interactive, GCLatencyMode.LowLatency, GCLatencyMode.SustainedLowLatency };
                for (int i = 0; i < mode.Length; i++)
                {
                    Console.WriteLine($"{i} = {mode[i]}");
                }
                Console.Write("Select GCLatencyMode = ");
                GCSettings.LatencyMode = mode[Convert.ToInt32(Console.ReadLine())];
            }
            Stopwatch sw = new Stopwatch();
            long msec = 0;
            for (int j = 0; j < OUTER_LOOP; j++)
            {


                sw.Restart();
                Thread t = new Thread(new ThreadStart(delegate
                {
                    byte[] b = null;
                    for (int i = 0; i < LOOP; ++i)
                    {
                        b = new byte[1024];
                    }
                }));
                t.Start();
                byte[] c = null;
                for (int i = 0; i < LOOP; ++i)
                {
                    c = new byte[1024];
                }

                t.Join();
                msec += sw.ElapsedMilliseconds;
            }
            //GCLatencyMode.Batch               183msec
            //GCLatencyMode.Interactive         180msec
            //GCLatencyMode.LowLatency          512msec
            //GCLatencyMode.SustainedLowLatency 186msec
            //ServerGC                          194msec
            Console.WriteLine($"{msec / OUTER_LOOP} msec");
            Console.ReadKey();
        }

    }
}
