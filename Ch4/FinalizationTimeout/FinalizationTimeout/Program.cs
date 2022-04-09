
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

class Program
{
    /// <summary>
    /// タイムアウト時間：１つのファイナライザー実行に約2秒
    ///                   すべてのファイナライザー実行に約40秒
    ///                   どちらかを満たした時点でファイナライザースレッドが終了します
    /// 
    /// どちらも概ね記載通りの動作。
    /// あくまでも、.NetFrameworkの仕様になります。
    /// NetCoreだとファイナライザーが呼ばれるとは限らないようです
    /// https://github.com/dotnet/docs/issues/17463
    /// 
    /// [Single Object Output]
    /// Spent 1000msec
    /// Spent 1500msec
    /// Spent 2000msec
    /// 続行するには何かキーを押してください. . .
    /// 
    /// [Multiple Object Output]
    /// 1000 msec spent
    /// 2000 msec spent
    /// 3000 msec spent
    /// 省略
    /// 37000 msec spent
    /// 38000 msec spent
    /// 39000 msec spent
    /// 続行するには何かキーを押してください. . .
    /// 
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args)
    {
        Console.WriteLine("1:SIngle Finalizer");
        Console.WriteLine("2:Multiple Finalizer");
        Console.Write("タイムアウトモードの選択:");
        int mode = int.Parse(Console.ReadLine());
        if (mode == 1)
        {
            DoSomething1();
        }
        else if (mode == 2)
        {
            DoSomething2();
        }
    }

    static void DoSomething1()
    {
        List<Thing1> list = new List<Thing1>();
        for (int i = 0; i < 25; i++)
        {
            list.Add(new Thing1());
        }
    }
    static void DoSomething2()
    {
        List<Thing2> list = new List<Thing2>();
        for (int i = 0; i < 1000; i++)
        {
            list.Add(new Thing2());
        }
    }
}

class Thing1
{
    int wait = 0;

    public Thing1()
    {
    }
    ~Thing1()
    {
        while (true)
        {
            Console.WriteLine($"{wait}msec spent");
            Thread.Sleep(100);
            wait += 100;
        }
    }
}
class Thing2
{
    static int time = 0;
    public Thing2()
    {
    }
    ~Thing2()
    {
        Thread.Sleep(1000);
        time += 1000;
        Console.WriteLine($"{time} msec spent");
    }
}
