using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvoidingValueTypeBoxing
{
    internal class Program
    {
        // IL
        /*.method private hidebysig static object GetInt() cil managed
        {
            // コード サイズ       8 (0x8)
            .maxstack  8
            IL_0000:  ldc.i4.s   42                 //32bit整数をスタックにロードする
            IL_0002:  box [mscorlib]  System.Int32  //値型を object 型にボックス化（boxing）する。
            IL_0007:  ret                           //メソッド呼び出し元に戻る
        } // end of method Program::GetInt*/

        static object GetInt()
        {
            int i = 42;
            return i;
        }
        /// <summary>
        /// シンプルな値型の構造体
        /// </summary>
        public struct Point2D_FullBoxing
        {
            public int X;
            public int Y;
        }
        /// <summary>
        /// 比較元のボックス化防止Equal
        /// </summary>
        public struct Point2D_HalfBoxing
        {
            public int X;
            public int Y;
            public override bool Equals(object obj)
            {
                if (!(obj is Point2D_HalfBoxing)) return false;
                Point2D_HalfBoxing other = (Point2D_HalfBoxing)obj;
                return X == other.X && Y == other.Y;
            }
        }
        /// <summary>
        /// 比較元、比較先のボックス化防止
        /// </summary>
        public struct Point2D_NoneBoxing
        {
            public int X;
            public int Y;
            public override bool Equals(object obj)
            {
                if (!(obj is Point2D_NoneBoxing)) return false;
                Point2D_NoneBoxing other = (Point2D_NoneBoxing)obj;
                return X == other.X && Y == other.Y;
            }
            public bool Equals(Point2D_NoneBoxing other)
            {
                return X == other.X && Y == other.Y;
            }
            public static bool operator ==(Point2D_NoneBoxing a, Point2D_NoneBoxing b)
            {
                return a.Equals(b);
            }
            public static bool operator !=(Point2D_NoneBoxing a, Point2D_NoneBoxing b)
            {
                return !(a==b);
            }
        }
        /// <summary>
        /// ジェネリックによるボックス化防止
        /// </summary>
        public struct Point2D_IEquatable : IEquatable<Point2D_IEquatable>
        {
            public int X;
            public int Y;
            public override bool Equals(object obj)
            {
                if (!(obj is Point2D_IEquatable)) return false;
                Point2D_IEquatable other = (Point2D_IEquatable)obj;
                return X == other.X && Y == other.Y;
            }
            public bool Equals(Point2D_IEquatable other)
            {
                return X == other.X && Y == other.Y;
            }
            public static bool operator ==(Point2D_IEquatable a, Point2D_IEquatable b)
            {
                return a.Equals(b);
            }
            public static bool operator !=(Point2D_IEquatable a, Point2D_IEquatable b)
            {
                return !(a == b);
            }
        }
        /// <summary>
        /// Output
        /// FullBoxing=585
        /// HalfBoxing=136
        /// NoneBoxing=126 
        /// EquatableBoxing=46
        /// </summary>
        static void Main(string[] args)
        {
            bool contains = false;
            int count_times = 100;
            long stat_time = 0;
            object obj = GetInt();
            Stopwatch sw = new Stopwatch();

            // FULL BOXING
            stat_time = 0;
            List<Point2D_FullBoxing> polygon_full = new List<Point2D_FullBoxing>();
            for (int i = 0; i < 10000000; i++)
            {
                polygon_full.Add(new Point2D_FullBoxing { X = i, Y = i });
            }
            Point2D_FullBoxing point_full = new Point2D_FullBoxing { X = 10000000, Y = 10000000 };

            for (int i = 0; i < count_times; i++)
            {
                sw.Restart();
                contains = polygon_full.Contains(point_full);
                stat_time += sw.ElapsedMilliseconds;
            }
            Console.WriteLine($"FullBoxing={stat_time / count_times}");
            polygon_full.Clear();

            // HALF BOXING
            stat_time = 0;
            List<Point2D_HalfBoxing> polygon_half = new List<Point2D_HalfBoxing>();
            for (int i = 0; i < 10000000; i++)
            {
                polygon_half.Add(new Point2D_HalfBoxing { X = i, Y = i });
            }
            Point2D_HalfBoxing point_half = new Point2D_HalfBoxing { X = 10000000, Y = 10000000 };
            for (int i = 0; i < count_times; i++)
            {
                sw.Restart();
                contains = polygon_half.Contains(point_half);
                stat_time += sw.ElapsedMilliseconds;
            }
            Console.WriteLine($"HalfBoxing={stat_time / count_times}");
            polygon_half.Clear();

            // NONE BOXING
            stat_time = 0;
            List<Point2D_NoneBoxing> polygon_none = new List<Point2D_NoneBoxing>();
            for (int i = 0; i < 10000000; i++)
            {
                polygon_none.Add(new Point2D_NoneBoxing { X = i, Y = i });
            }
            Point2D_NoneBoxing point_none = new Point2D_NoneBoxing { X = 10000000, Y = 10000000 };
            for (int i = 0; i < count_times; i++)
            {
                sw.Restart();
                contains = polygon_none.Contains(point_none);
                stat_time += sw.ElapsedMilliseconds;
            }
            Console.WriteLine($"NoneBoxing={stat_time / count_times}");

            // EQUATABLE・・・NONE BOXING版だと実はListのジェネリック実装によってボックス化している
            stat_time = 0;
            List<Point2D_IEquatable> polygon_eq = new List<Point2D_IEquatable>();
            for (int i = 0; i < 10000000; i++)
            {
                polygon_eq.Add(new Point2D_IEquatable { X = i, Y = i });
            }
            Point2D_IEquatable point_eq = new Point2D_IEquatable { X = 10000000, Y = 10000000 };
            for (int i = 0; i < count_times; i++)
            {
                sw.Restart();
                contains = polygon_eq.Contains(point_eq);
                stat_time += sw.ElapsedMilliseconds;
            }
            Console.WriteLine($"EquatableBoxing={stat_time / count_times}");
            Console.ReadLine();
        }
    }
}
