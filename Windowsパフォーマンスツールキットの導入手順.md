### (編集中)Windows Performance AnalyzerによるCLRのGCイベントのトレーシング実験

1. WPAをインストールします。
  Setupを次のページからダウンロード。  
  https://docs.microsoft.com/ja-jp/windows-hardware/test/wpt/windows-performance-analyzer
  
2. セットアップを開始します。  
色々同梱されてますが、今回はWindowsパフォーマンスツールキットのみインストールします。  
![wpaインストール](https://user-images.githubusercontent.com/49807271/160272267-08de0f2c-4755-45de-998d-e09969d97bd5.jpg)
  
3. インストール後に、システム環境変数_NT_SYMBOL_PATHを設定し、マイクロソフトパブリックシンボルサーバーと  
ローカルシンボルキャッシュを指定する  
```
srv*{LocalStore}*https://msdl.microsoft.com/download/symbols
```
　 ここで、 {LocalStore} は任意に作成したローカル ディレクトリのパスです。   
 　 私は以下のコマンドを実行しました。
```
set _NT_SYMBOL_PATH="rv*C:\Users\user\Symbols*https://msdl.microsoft.com/download/symbols"
```

途中コミット
