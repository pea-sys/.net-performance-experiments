using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadSafeQueue_vs_ConcurrentQueue
{
    class ThreadSafeQueue<T>
    {
        private Queue<T> queue = new Queue<T>();
        private readonly Object locker = new Object();
        public ThreadSafeQueue(params T[] items)
        {
            queue = new Queue<T>(items);
        }

        public void Enqueue(T item)
        {
            lock (locker)
            {
                queue.Enqueue(item);
            }
        }
        public T Dequeue()
        {
            lock(locker)
            {
                return queue.Dequeue();
            }
        }
        public void Clear()
        {
            lock(locker)
            {
                queue.Clear();
            }
        }
    }
}
