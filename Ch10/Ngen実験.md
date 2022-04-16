### Ngen お試し

以下の URL に沿って、Ngen を試してみました。  
(https://docs.microsoft.com/ja-jp/windows/msix/desktop/desktop-to-uwp-r2r)

1.ネイティブイメージを作成したいプロジェクトのプラットフォームを x86 か x64 に設定します。
![1](https://user-images.githubusercontent.com/49807271/163672372-a396a824-3d07-4cad-84ad-77d1b23fd260.jpg)

2.VisualStudio のパッケージマネージャコンソールから以下を入力

```
PM> Install-Package Microsoft.DotNet.Framework.NativeImageCompiler

'.NETFramework,Version=v4.8' を対象とするプロジェクト 'JitOptimizeRangeCheck' に関して、パッケージ 'Microsoft.DotNet.Framework.NativeImageCompiler.1.0.0' の依存関係情報の収集を試行しています
依存関係情報の収集に 1.11 sec かかりました
DependencyBehavior 'Lowest' でパッケージ 'Microsoft.DotNet.Framework.NativeImageCompiler.1.0.0' の依存関係の解決を試行しています
依存関係情報の解決に 0 ms かかりました
パッケージ 'Microsoft.DotNet.Framework.NativeImageCompiler.1.0.0' をインストールするアクションを解決しています
パッケージ 'Microsoft.DotNet.Framework.NativeImageCompiler.1.0.0' をインストールするアクションが解決されました
'nuget.org' からパッケージ 'Microsoft.DotNet.Framework.NativeImageCompiler 1.0.0' を取得しています。
パッケージ 'Microsoft.DotNet.Framework.NativeImageCompiler.1.0.0' をフォルダー 'C:\Users\user\OneDrive\デスクトップ\.NET_Performance_Experimental\Ch10\JitOptimizeRangeCheck\packages' に追加しています
パッケージ 'Microsoft.DotNet.Framework.NativeImageCompiler.1.0.0' をフォルダー 'C:\Users\user\OneDrive\デスクトップ\.NET_Performance_Experimental\Ch10\JitOptimizeRangeCheck\packages' に追加しました
パッケージ 'Microsoft.DotNet.Framework.NativeImageCompiler.1.0.0' を 'packages.config' に追加しました
'Microsoft.DotNet.Framework.NativeImageCompiler 1.0.0' が JitOptimizeRangeCheck に正常にインストールされました
NuGet の操作の実行に 1.01 sec かかりました
経過した時間: 00:00:03.9567745
```

3. リリースモードでビルドします。
   ビルド成功時に「Native image ~ generated successfully.」と出ていたら、ネイティブイメージが作られています。

```
1>------ ビルド開始: プロジェクト: JitOptimizeRangeCheck, 構成1>  Native image obj\x64\Release\\R2R\JitOptimizeRangeCheck.exe generated successfully.
==========
```

4. ネイティブイメージの格納場所を確認します。
   「C:\Windows\assembly\NativeImages_v4.0.30319_32\JitOptimize9c961b92#」

5. 試しにとてもシンプルなプログラムで Ngen の有無で各 5 回起動時間を測定しました。  
   Ngen 有 = 平均約 40msec 　[Ngen あり] = 平均約 250mse

   JIT コンパイルでパフォーマンスが遅くなるようなプログラムではないので  
   ネイティブイメージキャッシュ見に行く分のオーバーヘッドが加わってるような気がします。

6. 一応、WinDbg で中身を見てみました。
   Ngen アリの場合、PreJIT モードで動作していることが確認できました。

[Ngen あり]

```
0:000> !U /d 00007ff7b67c2f78
preJIT generated code
```

[Ngen なし]

```
0:000> !U /d 00007ffcfd05092c
Normal JIT generated code
```
