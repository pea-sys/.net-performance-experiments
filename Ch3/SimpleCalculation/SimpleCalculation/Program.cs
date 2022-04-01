using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalculation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Calculation(4, 83);
        }
        static int Calculation(int a ,int b)
        {
            int x = a + b;
            int y = a - b;
            int z = b - a;
            int w = 2 * b + 2 * a;
            return x + y + z + w;
        }
    }
}
