namespace Microbenchmark
{
    /// <summary>
    /// 1:悪いケース
    ///        Fragment1 Fragment2
    /// 1回目: 36        1022
    /// 2回目: 36        1073
    /// 3回目: 51        1049
    /// 4回目: 52        1237
    /// 5回目: 36        1428
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// エントリポイント
        /// </summary>
        /// <param name="args">起動引数 1:悪いケース 2:良いケース</param>
        static void Main(string[] args)
        {
            switch(int.Parse(args[0]))
            {
                case 1:
                    new BadPattern().BenchMark();
                    break;
                case 2:
                    GoodPattern.Employee obj = new GoodPattern.Employee();
                    new GoodPattern().BenchMark(obj);
                    break;

            }
            
        }
        
    }
}
