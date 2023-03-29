# ThreadSafeQueue-vs-ConcurrentQueue

バックグランドでGC等動く言語はベンチマークソフトを使用して測定を行う。  
自力で測定することも出来ますが、前提条件を見落としがちなので、  
ベンチマークソフトを使って、ミスが入る余地を無くす。  
* DEBUGモードで測定してしまう
* 測定回数が不十分
* 電源オプションが不定


C#の場合はBenchmarkDotNetが標準だと思われる。
* https://github.com/dotnet/BenchmarkDotNet

Nugetパッケージマネージャからインストールするだけで導入可能。

測定対象のクラスや関数にBenchmarkDotNetから提供されている属性を付与してあげれば良い。  
測定対象のクラスや関数は全てpublicで宣言する必要がある。


## [使用例]

#### ■入力
```csharp
    [ShortRunJob]
    [MinColumn, MaxColumn]
    public class Measure
    {
        public static int N = 2000000;
        public static IEnumerable<int> iteration = Enumerable.Range(0, N);
        [Benchmark]
        public void EnqueueThreadSafeQueue()
        {
            ThreadSafeQueue<int> queue = new ThreadSafeQueue<int>();
            foreach(var v in iteration)
            {
                queue.Enqueue(v);
            }
        }
        [Benchmark]
        public void EnqueueConcurrentQueue()
        {
            ConcurrentQueue<int> queue = new ConcurrentQueue<int>();
            foreach (var v in iteration)
            {
                queue.Enqueue(v);
            }
        }
        [Benchmark]
        public void DequeueThreadSafeQueue()
        {
            ThreadSafeQueue<int> queue = new ThreadSafeQueue<int>(iteration.ToArray());
            foreach (var v in iteration)
            {
                queue.Dequeue();
            }
        }
        [Benchmark]
        public void DequeueConcurrentQueue()
        {
            ConcurrentQueue<int> queue = new ConcurrentQueue<int>(iteration.ToArray());
            int result = 0;
            foreach (var v in iteration)
            {
                queue.TryDequeue(out result);
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Measure>();
            Console.ReadKey();
        }
    }
```
#### ■出力
```

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19043.928/21H1/May2021Update)
Intel Core i7-8565U CPU 1.80GHz (Whiskey Lake), 1 CPU, 8 logical and 4 physical cores
  [Host]   : .NET Framework 4.8 (4.8.4340.0), X86 LegacyJIT
  ShortRun : .NET Framework 4.8 (4.8.4340.0), X86 LegacyJIT

Job=ShortRun  IterationCount=3  LaunchCount=1
WarmupCount=3

```

|                 Method |      Mean |    Error |   StdDev |       Min |       Max |
|----------------------- |----------:|---------:|---------:|----------:|----------:|
| EnqueueThreadSafeQueue |  82.32 ms | 19.31 ms | 1.058 ms |  81.31 ms |  83.42 ms |
| EnqueueConcurrentQueue |  68.31 ms | 38.58 ms | 2.115 ms |  66.49 ms |  70.63 ms |
| DequeueThreadSafeQueue | 134.76 ms | 59.87 ms | 3.282 ms | 131.92 ms | 138.35 ms |
| DequeueConcurrentQueue | 107.62 ms | 55.55 ms | 3.045 ms | 104.25 ms | 110.18 ms |

