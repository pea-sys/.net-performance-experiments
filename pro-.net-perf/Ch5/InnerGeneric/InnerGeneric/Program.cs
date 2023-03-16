using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnerGeneric
{
    class BasicStack<T>
    {
        private T[] items;
        private int topIndex;

        public BasicStack(int capacity = 42)
        {
            items = new T[capacity];
        }
        public void Push(T item)
        {
            items[topIndex++] = item;
        }
        public T Pop()
        {
            return items[--topIndex];
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            BasicStack<string> stringStack = new BasicStack<string>();
            stringStack.Push("Hello");
            stringStack.Pop();
            BasicStack<int[]> intArrStack = new BasicStack<int[]>();
            intArrStack.Push(new[] { 14 });
            intArrStack.Pop();
            BasicStack<int> intStack = new BasicStack<int>();
            intStack.Push(42);
            intStack.Pop();
            BasicStack<double> doubleStack = new BasicStack<double>();
            doubleStack.Push(1.0);
            doubleStack.Pop();
            Console.ReadLine();
        }
    }
}
