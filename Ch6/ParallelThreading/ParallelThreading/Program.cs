using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelThreading
{
    /// <summary>
    /// [Output]
    /// PrimesInRange_Sequential(100, 200000) took 4372ms on average
    /// PrimesInRange_Threads(100, 200000) took 2392.75ms on average
    /// PrimesInRange_ThreadPool(100, 200000) took 1828.25ms on average
    /// PrimesInRange_ParallelFor(100, 200000) took 1832.5ms on average
    /// PrimesInRange_Aggregation(100, 200000) took 1828.75ms on average
    /// 
    /// QuickSort_Sequential (1,000,000 including allocation) took 656.75ms on average
    /// QuickSort_Parallel(1,000,000 including allocation) took 1310ms on average
    /// QuickSort_Parallel_Threshold(1,000,000 including allocation) took 175.25ms on average
    /// </summary>
    internal class Program
    {
        //Returns all the prime numbers in the range [start, end)
        public static IEnumerable<uint> PrimesInRange_Sequential(uint start, uint end)
        {
            List<uint> primes = new List<uint>();
            for (uint number = start; number < end; ++number)
            {
                if (IsPrime(number))
                {
                    primes.Add(number);
                }
            }
            return primes;
        }
        private static bool IsPrime(uint number)
        {
            //This is a very inefficient O(n) algorithm, but it will do for our expository purposes
            if (number == 2) return true;
            if (number % 2 == 0) return false;
            for (uint divisor = 3; divisor < number; divisor += 2)
            {
                if (number % divisor == 0) return false;
            }
            return true;
        }

        public static IEnumerable<uint> PrimesInRange_Threads(uint start, uint end)
        {
            List<uint> primes = new List<uint>();
            uint range = end - start;
            uint numThreads = (uint)Environment.ProcessorCount; //is this a good idea?
            uint chunk = range / numThreads; //hopefully, there is no remainder
            Thread[] threads = new Thread[numThreads];
            for (uint i = 0; i < numThreads; ++i)
            {
                uint chunkStart = start + i * chunk;
                uint chunkEnd = chunkStart + chunk;
                threads[i] = new Thread(() =>
                {
                    for (uint number = chunkStart; number < chunkEnd; ++number)
                    {
                        if (IsPrime(number))
                        {
                            lock (primes)
                            {
                                primes.Add(number);
                            }
                        }
                    }
                });
                threads[i].Start();
            }
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            return primes;
        }

        public static IEnumerable<uint> PrimesInRange_ThreadPool(uint start, uint end)
        {
            List<uint> primes = new List<uint>();
            const uint ChunkSize = 100;
            int completed = 0;
            ManualResetEvent allDone = new ManualResetEvent(initialState: false);
            uint chunks = (end - start) / ChunkSize; //again, this should divide evenly
            for (uint i = 0; i < chunks; ++i)
            {
                uint chunkStart = start + i * ChunkSize;
                uint chunkEnd = chunkStart + ChunkSize;
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    for (uint number = chunkStart; number < chunkEnd; ++number)
                    {
                        if (IsPrime(number))
                        {
                            lock (primes)
                            {
                                primes.Add(number);
                            }
                        }
                    }
                    if (Interlocked.Increment(ref completed) == chunks)
                    {
                        allDone.Set();
                    }
                });
            }
            allDone.WaitOne();
            return primes;
        }

        public static IEnumerable<uint> PrimesInRange_ParallelFor(uint start, uint end)
        {
            List<uint> primes = new List<uint>();
            Parallel.For((long)start, (long)end, number =>
            {
                if (IsPrime((uint)number))
                {
                    lock (primes)
                    {
                        primes.Add((uint)number);
                    }
                }
            });
            return primes;
        }

        public static IEnumerable<uint> PrimesInRange_Aggregation(uint start, uint end)
        {
            List<uint> primes = new List<uint>();
            Parallel.For(3, 200000,
              () => new List<uint>(),        //initialize the local copy
              (i, pls, localPrimes) =>
              {    //single computation step, returns new local state
                  if (IsPrime((uint)i))
                  {
                      localPrimes.Add((uint)i);       //no synchronization necessary, thread-local state
                  }
                  return localPrimes;
              },
              localPrimes =>
              {              //combine the local lists to the global one
                  lock (primes)
                  {              //synchronization is required
                      primes.AddRange(localPrimes);
                  }
              }
            );
            return primes;
        }

        public static void QuickSort_Sequential<T>(T[] items) where T : IComparable<T>
        {
            QuickSort_Sequential(items, 0, items.Length);
        }
        private static void QuickSort_Sequential<T>(T[] items, int left, int right) where T : IComparable<T>
        {
            if (left == right) return;
            int pivot = Partition(items, left, right);
            QuickSort_Sequential(items, left, pivot);
            QuickSort_Sequential(items, pivot + 1, right);
        }
        private static int Partition<T>(T[] items, int left, int right) where T : IComparable<T>
        {
            int pivotPos = (right - left) / 2; //often a random index between left and right is used
            T pivotValue = items[pivotPos];
            Swap(ref items[right - 1], ref items[pivotPos]);
            int store = left;
            for (int i = left; i < right - 1; ++i)
            {
                if (items[i].CompareTo(pivotValue) < 0)
                {
                    Swap(ref items[i], ref items[store]);
                    ++store;
                }
            }
            Swap(ref items[right - 1], ref items[store]);
            return store;
        }
        private static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        public static void QuickSort_Parallel<T>(T[] items) where T : IComparable<T>
        {
            QuickSort_Parallel(items, 0, items.Length);
        }
        private static void QuickSort_Parallel<T>(T[] items, int left, int right) where T : IComparable<T>
        {
            if (right - left < 2) return;
            int pivot = Partition(items, left, right);
            Task leftTask = Task.Run(() => QuickSort_Parallel(items, left, pivot));
            Task rightTask = Task.Run(() => QuickSort_Parallel(items, pivot + 1, right));
            Task.WaitAll(leftTask, rightTask);
        }

        public static void QuickSort_Parallel_Threshold<T>(T[] items) where T : IComparable<T>
        {
            QuickSort_Parallel_Threshold(items, 0, items.Length);
        }
        private static void QuickSort_Parallel_Threshold<T>(T[] items, int left, int right) where T : IComparable<T>
        {
            if (right - left < 2) return;
            int pivot = Partition(items, left, right);
            if (right - left > 500)
            {
                Parallel.Invoke(
                  () => QuickSort_Parallel_Threshold(items, left, pivot),
                  () => QuickSort_Parallel_Threshold(items, pivot + 1, right)
                );
            }
            else
            {
                QuickSort_Sequential(items, left, pivot);
                QuickSort_Sequential(items, pivot + 1, right);
            }
        }

        #region Interlocked Operations and CAS

        public static void InterlockedMultiplyInPlace(ref int x, int y)
        {
            int temp, mult;
            do
            {
                temp = x;
                mult = temp * y;
            } while (Interlocked.CompareExchange(ref x, mult, temp) != temp);
        }
        public static void DoWithCAS<T>(ref T location, Func<T, T> generator) where T : class
        {
            T temp, replace;
            do
            {
                temp = location;
                replace = generator(temp);
            } while (Interlocked.CompareExchange(ref location, replace, temp) != temp);
        }
        public class SpinLock
        {
            private volatile int locked;
            public void Acquire()
            {
                while (Interlocked.CompareExchange(ref locked, 1, 0) != 0) ;
            }
            public void Release()
            {
                locked = 0;
            }
        }

        public class LockFreeStack<T>
        {
            private class Node
            {
                public T Data;
                public Node Next;
            }
            private Node head;
            public void Push(T element)
            {
                Node node = new Node { Data = element };
                DoWithCAS(ref head, h =>
                {
                    node.Next = h;
                    return node;
                });
            }
            public bool TryPop(out T element)
            {
                //DoWithCAS does not work here because we need early termination semantics
                Node node;
                do
                {
                    node = head;
                    if (node == null)
                    {
                        element = default(T);
                        return false; //bail out – nothing to return
                    }
                } while (Interlocked.CompareExchange(ref head, node.Next, node) != node);
                element = node.Data;
                return true;
            }
        }

        #endregion
        static void Main(string[] args)
        {
            Random rnd = new Random();

            //Measure(() => PrimesInRange_Sequential(100, 200000), "PrimesInRange_Sequential(100, 200000)");
            //Measure(() => PrimesInRange_Threads(100, 200000), "PrimesInRange_Threads(100, 200000)");
            //Measure(() => PrimesInRange_ThreadPool(100, 200000), "PrimesInRange_ThreadPool(100, 200000)");
            //Measure(() => PrimesInRange_ParallelFor(100, 200000), "PrimesInRange_ParallelFor(100, 200000)");
            //Measure(() => PrimesInRange_Aggregation(100, 200000), "PrimesInRange_Aggregation(100, 200000)");

            Measure(() => Enumerable.Range(0, 1000000).Select(n => rnd.Next()).ToArray(), QuickSort_Sequential, "QuickSort_Sequential (1,000,000 including allocation)");
            // 並列処理の粒度が細かすぎてシングルスレッドより遅い
            Measure(() => Enumerable.Range(0, 1000000).Select(n => rnd.Next()).ToArray(), QuickSort_Parallel, "QuickSort_Parallel (1,000,000 including allocation)");
            // 配列のサイズに応じて並列処理に切り替える
            Measure(() => Enumerable.Range(0, 1000000).Select(n => rnd.Next()).ToArray(), QuickSort_Parallel_Threshold, "QuickSort_Parallel_Threshold (1,000,000 including allocation)");
        }

        private static void Measure(Action what, string description)
        {
            const int ITERATIONS = 5;
            double[] elapsed = new double[ITERATIONS];
            for (int i = 0; i < ITERATIONS; ++i)
            {
                Stopwatch sw = Stopwatch.StartNew();
                what();
                elapsed[i] = sw.ElapsedMilliseconds;
            }
            Console.WriteLine("{0} took {1}ms on average", description, elapsed.Skip(1).Average());
        }
        private static void Measure<T>(Func<T> setup, Action<T> measurement, string description)
        {
            T state = setup();
            Measure(() => measurement(state), description);
        }
        private static void Repeat(int times, Action action)
        {
            for (int i = 0; i < times; ++i)
                action();
        }
    }
}
