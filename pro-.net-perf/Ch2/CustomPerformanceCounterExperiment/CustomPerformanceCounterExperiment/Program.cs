using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CustomPerformanceCounterExperiment
{
    /// <summary>
    /// カスタムパフォーマンスカウンターの動作確認プログラム
    /// ※実運用ではリソース管理やスレッドセーフか気を付ける必要があります
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            //コンピュータ名
            //"."はローカルコンピュータを表す
            //コンピュータ名は省略可能（省略時は"."）
            string machineName = ".";
            //カテゴリ名
            string categoryName = "Processor";
            //カウンタ名
            string counterName = "% Processor Time";
            //インスタンス名
            string instanceName = "_Total";

            //-----------------------------
            //既定のパフォーマンスカウンター
            //-----------------------------

            //カテゴリが存在するか確かめる
            if (!PerformanceCounterCategory.Exists(
                categoryName, machineName))
            {
                Console.WriteLine("登録されていないカテゴリです。");
                return;
            }

            //カウンタが存在するか確かめる
            if (!PerformanceCounterCategory.CounterExists(
                counterName, categoryName, machineName))
            {
                Console.WriteLine("登録されていないカウンタです。");
                return;
            }

            //PerformanceCounterオブジェクトの作成
            PerformanceCounter pc = new PerformanceCounter(categoryName, counterName, instanceName, machineName);

            Console.WriteLine("既定のパフォーマンスカウンター");
            //1秒おきに値を取得する
            for (int i = 0; i < 10; i++)
            {
                //計算された値を取得し、表示する
                Console.WriteLine(pc.NextValue());
                //1秒待機する
                System.Threading.Thread.Sleep(1000);
            }

            //-----------------------------
            //カスタムパフォーマンスカウンター
            //※カウンター追加に管理者権限が必要なため、マニフェストファイルのrequestedExecutionLevelをrequireAdministratorに設定
            //-----------------------------
            //カスタムカテゴリ名
            string customCategoryName = "Custom";
            //カスタムカウンタ名
            string customCounterName = "Timer";

            if (PerformanceCounterCategory.Exists(customCategoryName))
            {
                PerformanceCounterCategory.Delete(customCategoryName);
            }
            // Add the counter.
            CounterCreationDataCollection counters = new CounterCreationDataCollection();
            CounterCreationData counterCreationData = new CounterCreationData(customCounterName, "カウンターの説明", PerformanceCounterType.NumberOfItems32);
            counters.Add(counterCreationData);
            // Add the category.
            PerformanceCounterCategory.Create(customCategoryName, "カテゴリの説明" , PerformanceCounterCategoryType.SingleInstance, counters);

            //PerformanceCounterオブジェクトの作成
            PerformanceCounter cpc = new PerformanceCounter(customCategoryName, customCounterName, readOnly: false);
            Console.WriteLine("カスタムパフォーマンスカウンター");
            //1秒おきにカウンターを加算する
            for (int i = 0; i < 1000; i++)
            {
                cpc.RawValue++;
                //計算された値を取得し、表示する
                Console.WriteLine(cpc.NextValue());
                //1秒待機する
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
