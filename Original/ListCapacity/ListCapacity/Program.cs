using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace ListCapacity
{
    [ShortRunJob]
    [MinColumn, MaxColumn]
    public class Measure
    {
        [Benchmark]
        public void UseCapacityInt()
        {
            UseCapacity(10000000);
        }
        [Benchmark]
        public void NoUseCapacityInt()
        {
            NoUseCapacity(1000000);
        }
        [Benchmark]
        public void UseCapacityShort()
        {
            UseCapacity(short.MaxValue);
        }
        [Benchmark]
        public void NoUseCapacityShort()
        {
            NoUseCapacity(short.MaxValue);
        }
        public void UseCapacity(int capacity)
        {
            List<int> list = new List<int>(capacity);
            for (var i = 0; i < capacity; i++)
            {
                list.Add(i);
            }
            var s = 0;
            foreach (var l in list)
            {
                s = l * 2;
            }
        }
       
        public void NoUseCapacity(int capacity)
        {
            List<int> list = new List<int>();
            for (var i = 0; i < capacity; i++)
            {
                list.Add(i);
            }
            var s = 0;
            foreach (var l in list)
            {
                s = l * 2;
            }
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Measure>();
            Console.ReadKey();
        }
    }
}
