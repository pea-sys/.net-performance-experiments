using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace var
{
    /// <summary>
    /// varの振る舞い調査
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// 結論から言うと、varは中間言語生成前にコンパイラが型を決定している(厳密な型指定)
        /// ※リフレクションを使っていたりする場合はしょうがない
        /// IL
        ///   .locals init (
        /// [0] int32 a,
        /// [1] int32 b,
        /// [2] object c,
        /// [3] object d,
        /// [4] int64 e,
        /// [5] int64 f)
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var a = 1;
            int b = 2;

            var c = new object();
            Object d = new object();

            var e = 1L;
            long f = 1L;
        }
    }
}
