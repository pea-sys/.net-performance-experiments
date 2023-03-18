using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventCalledByNullConditionalOperator
{
    /// <summary>
    /// イベントハンドラーのベストプラクティス
    /// </summary>
    internal class Program
    {
        private static readonly int HANDLER_COUNT = 10;


        static void ExistsHandlerCaseA()
        {
            EventSource _source = new EventSource();
            int counter = 0;
            for (int i = 0; i < HANDLER_COUNT; i++)
            {
                _source.Updated += (sender, e) =>
                {
                    Console.WriteLine(counter++);
                };
            }

            _source.RaiseUpdates();
        }

        static void NotExistsHandlerCaseA()
        {
            EventSource _source = new EventSource();
            try
            {
                // NULLチェックなしなので例外
                _source.RaiseUpdates();
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine("NotExistsHandlerCaseA:" + ex.Message);
            }
        }
        static void NotExistsHandlerCaseB()
        {
            EventSource _source = new EventSource();
            int counter = 0;
            // NULLチェックを入れたので大体うまく動く
            if (_source.Updated != null)
            {
                _source.RaiseUpdates();
            }
            else
            {
                Console.WriteLine("NotExistsHandlerCaseB:ハンドラーが登録されていません");
            }
            // しかし、NULLチェックとイベントハンドラ呼び出しの間に
            // アタッチされているイベントがデタッチされる可能性がありえる
            for (int i = 0; i < HANDLER_COUNT; i++)
            {
                _source.Updated += (sender, e) =>
                {
                    Console.WriteLine(counter++);
                };
            }
            try
            {
                if (_source.Updated != null)
                {
                    _source.Updated = null; //疑似的に割込み処理でイベントハンドラがデタッチされたものとする
                    _source.RaiseUpdates();
                }
                else
                {
                    Console.WriteLine("NotExistsHandlerCaseB:ハンドラーが登録されていません");
                }
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine("NotExistsHandlerCaseB:" + ex.Message);
            }
        }

        static void NotExistsHandlerCaseC()
        {
            // null条件演算子を使用することで安全になる
            // null条件演算子はスレッドセーフなので何も懸念はない
            EventSource _source = new EventSource();
            _source.RaiseUpdatesNullable();
            Console.WriteLine("NotExistsHandlerCaseC is Best Practice");
        }

        static void Main(string[] args)
        {
            ExistsHandlerCaseA();
            NotExistsHandlerCaseA();
            NotExistsHandlerCaseB();
            NotExistsHandlerCaseC();

            Console.ReadLine();
        }
    }
}
