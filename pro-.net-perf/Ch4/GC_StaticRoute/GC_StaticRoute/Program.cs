using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GC_StaticRoute
{
    class Button
    {
        public void OnClick(object sender, EventArgs e)
        {
            
        }
    }
    internal class Program
    {
        static event EventHandler ButtonClick;
        static void Main(string[] args)
        {
            while (true)
            {
                Button button = new Button();
                ButtonClick += button.OnClick; //ハンドルメモリリーク
                Thread.Sleep(100);
            }
        }
    }
}
