using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MemoryLayout
{
    internal class Program
    {
        /// <summary>
        /// シーケンシャルレイアウトは宣言順にレイアウト
        /// メモリの読み書きは、アドレスが4や8の倍数になっている方が効率的。
        /// Layout
        /// Field | B1 | B2 | B3 | B4 | B5 |         F         |
        /// Offset| 0  | 1  | 2  | 3  | 4  | 5  | 6  | 7  | 8  |
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Layout_Sequential
        {
            public byte B1;
            public byte B2;
            public byte B3;
            public byte B4;
            public byte B6;
            public float F;
        }
        /// <summary>
        /// シーケンシャルレイアウトは宣言順にレイアウト
        /// 間隔の開け方(Pack)は、通常は、32ビットCPUであれば4 (4バイト = 32ビット)、64ビットCPUであれば8 (8バイト = 64ビット)です 
        /// (それが一番高速になる可能性が高い)。
        /// Layout
        /// Field | B1 |    |         F         | B2 |    |
        /// Offset| 0  | 1  | 2  | 3  | 4  | 5  | 6  | 7  |
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct Layout_Sequential_Pack2
        {
            public byte B1;
            public float F;
            public byte B2;
        }
        /// <summary>
        ///  コンパイラ任せの自動レイアウト(相互運用しない場合は基本的にこれでOK）
        /// Layout
        /// Field |         F         | B1 | B2 |
        /// Offset| 0  | 1  | 2  | 3  | 4  | 5  |
        /// </summary>
        [StructLayout(LayoutKind.Auto)]
        public struct Layout_Auto
        {
            public byte B1;
            public float F;
            public byte B2;
        }
        /// <summary>
        /// Layout
        /// Field |         F         |
        /// Field | B1 | B2 | B3 | B4 |
        /// Offset| 0  | 1  | 2  | 3  | 
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        public struct Layout_Explicit
        {
            [FieldOffset(0)] public float F;
            [FieldOffset(0)] public byte B1;
            [FieldOffset(1)] public byte B2;
            [FieldOffset(2)] public byte B3;
            [FieldOffset(3)] public byte B4;
        }
        /// <summary>
        /// Output
        /// Sequential サイズ: 12
        /// Sequential Pack2サイズ: 8
        /// Auto サイズ: 8, レイアウト：4,5,0
        /// Explicit        サイズ: 4
        /// FとB1のメモリアドレス一致：True
        /// B1=1,B2=2,B3=3,B4=4,F=1.53999E-36
        /// B1=0,B2=60,B3=28,B4=70,F=9999
        /// </summary>
        /// <param name="args"></param>
        static unsafe void Main(string[] args)
        {
            Console.WriteLine($@"Sequential      サイズ: {sizeof(Layout_Sequential)}");
            Console.WriteLine($@"Sequential Pack2サイズ: {sizeof(Layout_Sequential_Pack2)}");

            var a = default(Layout_Auto);
            var p = &a;
            var pb1 = &a.B1;
            var pf = &a.F;
            var pb2 = &a.B2;
            Console.WriteLine($@"Auto            サイズ: {sizeof(Layout_Auto)},レイアウト：{(long)pb1 - (long)p},{(long)pb2 - (long)p},{(long)pf - (long)p}");



            var e = default(Layout_Explicit);
            Console.WriteLine($@"Explicit        サイズ: {sizeof(Layout_Explicit)}");
            Console.WriteLine($@"FとB1のメモリアドレス一致：{&e.F == &e.B1}");
            e.B1 = 1;
            e.B2 = 2;
            e.B3 = 3;
            e.B4 = 4;
            Console.WriteLine($@"B1={e.B1},B2={e.B2},B3={e.B3},B4={e.B4},F={e.F}");
            e.F = 9999;
            Console.WriteLine($@"B1={e.B1},B2={e.B2},B3={e.B3},B4={e.B4},F={e.F}");
        }
    }
}