# NugetStrage のクリア

ディスク容量不足により 10GB 以上ある Nuget フォルダをクリアすることにしました。  
手動でやるのはリスクがあるので、安全なやり方を調べました。

やり方は簡単で、VisualStudio の Nuget パッケージマネージャーで「Clear All NuGet Storage」をクリックするだけです。

![Nuget](https://user-images.githubusercontent.com/49807271/232167918-5d76a5b8-b0e1-41e6-9fe7-b43865223db1.png)

クリア対象はコンソールに出力されます。

```
NuGet 一時キャッシュをクリア中: C:\Users\user\AppData\Local\Temp\NuGetScratch
NuGet プラグイン キャッシュをクリア中: C:\Users\user\AppData\Local\NuGet\plugins-cache
ローカル リソースがクリアされました。
========== 終了 ==========
```
