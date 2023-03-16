using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ch05
{

    /// <summary>
    /// 環境:Intel(R) Core(TM) i3-4000M CPU @ 2.40GHz 
    ///      3 MB Intel® Smart Cache
    ///      
    /// 配列もリンクリストのループも各整数には一度しかアクセスしないので、キャッシュヒットのメリットを得られる
    /// 再利用可能なデータがないので、キャッシュについては考える必要がないように見えます
    /// しかし、このプログラムのパフォーマンスは、データのメモリアウトに大きく左右されます。

    /// リンクリストのスキャンは、ノードからノードへの移動
    /// 配列のスキャンは、配列のインデックスのインクリメント

    /// キャッシュラインの先頭でキャッシュミスが発生すると、16個の連続する整数がキャッシュに取り込まれる
    /// 配列へのアクセスはシーケンシャルに行われるため、次の15個の整数はすでにキャッシュにある。キャッシュミス率は1:16
    /// リンクリストは、キャッシュミスが起きても、ノードは最大で３つしかキャッシュに取り込まれないので、キャッシュミス率は1:4
    /// 
    /// [Output]
    /// 64bit Process
    /// Naive: 218464
    /// Blocked(bs= 4) : 66512
    /// Blocked(bs= 8) : 54698
    /// Blocked(bs= 16) : 53352
    /// Blocked(bs= 32) : 56078
    /// Blocked(bs= 64) : 72849
    /// Blocked(bs= 128) : 74971
    /// Blocked(bs= 256) : 76210
    /// Blocked(bs= 512) : 161432
    /// Blocked(bs= 1024) : 270346
    /// Blocked(bs= 2048) : 461253
    /// LinkedList<int> took 188ms on average
    /// List<int> took 128ms on average
    /// int[] took 110ms on average
    /// 
    /// 32bit Process
    /// Naive: 409388
    /// Blocked(bs= 4) : 143447
    /// Blocked(bs= 8) : 116640
    /// Blocked(bs= 16) : 112736
    /// Blocked(bs= 32) : 116498
    /// Blocked(bs= 64) : 151179
    /// Blocked(bs= 128) : 151269
    /// Blocked(bs= 256) : 175264
    /// Blocked(bs= 512) : 333547
    /// Blocked(bs= 1024) : 404780
    /// Blocked(bs= 2048) : 405812
    /// LinkedList<int> took 91ms on average
    /// List<int> took 114ms on average
    /// int[] took 91ms on average
    /// </summary>
    class Program
    {
        private static void Measure(Action what, string description)
        {
            const int ITERATIONS = 1;
            double[] elapsed = new double[ITERATIONS];
            for (int i = 0; i < ITERATIONS; ++i)
            {
                Stopwatch sw = Stopwatch.StartNew();
                what();
                elapsed[i] = sw.ElapsedMilliseconds;
            }
            Console.WriteLine("{0} took {1}ms on average", description, elapsed.Average());
        }
        private static void Measure<T>(Func<T> setup, Action<T> measurement, string description)
        {
            T state = setup();
            Measure(() => measurement(state), description);
        }

        static volatile int sum = 0;

        static int N = 2048;

        static int[,] BuildMatrix()
        {
            int[,] m = new int[N, N];
            Random r = new Random(Environment.TickCount);
            for (int i = 0; i < N; ++i)
                for (int j = 0; j < N; ++j)
                    m[i, j] = r.Next();
            return m;
        }
        static int[,] MultiplyNaive(int[,] A, int[,] B)
        {
            int[,] C = new int[N, N];
            for (int i = 0; i < N; ++i)
                for (int j = 0; j < N; ++j)
                    for (int k = 0; k < N; ++k)
                        C[i, j] += A[i, k] * B[k, j];
            return C;
        }
        static int[,] MultiplyBlocked(int[,] A, int[,] B, int bs)
        {
            int[,] C = new int[N, N];
            for (int ii = 0; ii < N; ii += bs)
                for (int jj = 0; jj < N; jj += bs)
                    for (int kk = 0; kk < N; kk += bs)
                    {
                        for (int i = ii; i < ii + bs; ++i)
                        {
                            for (int j = jj; j < jj + bs; ++j)
                            {
                                for (int k = kk; k < kk + bs; ++k)
                                    C[i, j] += A[i, k] * B[k, j];
                            }
                        }
                    }
            return C;
        }

        static void Main(string[] args)
        {
            #region Matrix multiplication

            int[,] A;
            int[,] B;
            Stopwatch sw;
            int[,] C;

            A = BuildMatrix();
            B = BuildMatrix();
            sw = Stopwatch.StartNew();
            C = MultiplyNaive(A, B);
            Console.WriteLine("Naive: " + sw.ElapsedMilliseconds);

            for (int bs = 4; bs <= N; bs *= 2)
            {
                A = BuildMatrix();
                B = BuildMatrix();
                sw = Stopwatch.StartNew();
                C = MultiplyBlocked(A, B, bs);
                Console.WriteLine("Blocked (bs=" + bs + "): " + sw.ElapsedMilliseconds);
            }

            #endregion

            #region LinkedList vs Array

            Measure(() => new LinkedList<int>(Enumerable.Range(0, 20000000)), numbers =>
            {
                for (LinkedListNode<int> curr = numbers.First; curr != null; curr = curr.Next)
                    sum += curr.Value;
            }, "LinkedList<int>");
            Measure(() => Enumerable.Range(0, 20000000).ToList(), numbers =>
            {
                foreach (int number in numbers)
                    sum += number;
            }, "List<int>");
            Measure(() => Enumerable.Range(0, 20000000).ToArray(), numbers =>
            {
                for (int i = 0; i < numbers.Length; ++i)
                    sum += numbers[i];
            }, "int[]");
            Console.ReadLine();

            #endregion
        }
    }
}
