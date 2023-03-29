using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace ThreadSafeQueue_vs_ConcurrentQueue
{
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
}
