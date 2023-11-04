MemoryCahceについて、ちょっと調べたメモ

■主な機能
* キーの登録と削除
* キャッシュメモリ容量の設定
* キャッシュの有効期限切れによる自動削除

[参考](https://learn.microsoft.com/ja-jp/dotnet/api/system.runtime.caching.memorycache?view=dotnet-plat-ext-7.0)

パフォーマンスはHashSetやDictionaryより１桁遅いけど、
nsなので特に使用に困ることはなさそう。  
MemoryCacheは2種類あるようだけど、それほど機能の差異はないので好みで選んで良さそう。


| Method                         | N     | Mean      | Error     | StdDev    | Median    |
|------------------------------- |------ |----------:|----------:|----------:|----------:|
| FindDadosEmpInCache            | 30000 | 275.46 ns | 11.241 ns | 32.071 ns | 265.21 ns |
| FindDataAtTheEnd               | 30000 | 868.23 ns | 17.251 ns | 26.345 ns | 856.23 ns |
| FindDataInDictionary           | 30000 |  32.75 ns |  0.318 ns |  0.248 ns |  32.73 ns |
| FindDataInConcurrentDictionary | 30000 |  39.46 ns |  0.620 ns |  0.518 ns |  39.23 ns |
| FindDataInHashset              | 30000 |  32.48 ns |  0.652 ns |  0.776 ns |  32.22 ns |

dotnetで１つの小規模サーバーアプリを動かしている場合には、第一候補として考えていいと思う。  
複数のサーバーアプリが動くのであれば、キャッシュサーバーを立てると良い。