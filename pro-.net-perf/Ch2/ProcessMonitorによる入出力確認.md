## ProcessMonitorによる入出力確認

### ■実験環境  
OS:Windows10  

### ■手順
1. ProcessMonitorを次のページからダウンロード  
https://docs.microsoft.com/ja-jp/sysinternals/downloads/procmon

2. Procmon.exeを起動すると、フィルターが表示されます。  
監視の対象外とした方が見やすいと思われるものが初期設定で選択されています。  
サービスの動きも見たい場合、Process Name is Systemの項目のチェックを外します。  

![手順2](https://user-images.githubusercontent.com/49807271/160617511-3b0a970e-afc9-40e1-9b08-1770154dd67b.jpg)

3. 次のプログラムを起動します。  
https://github.com/Apress/pro-.net-perf/blob/master/Ch02/JackCompiler/JackCompiler.exe

4. フィルターボタンで監視対象にJackCompiler.exeを追加します。
![手順4](https://user-images.githubusercontent.com/49807271/160627625-4be1b145-1c6e-4a96-8cf6-69db4f517b00.jpg)

5. フィルターが適用されました。基本的にはOperation列を見ます。  
![手順5](https://user-images.githubusercontent.com/49807271/160627888-aeac5be5-efda-4cff-933e-afe1ed699b82.jpg)

6. ToolからFile Summryを見ると、どのファイルに対し、どれくらいのデータ量読み書きしたかが分かります。
![手順9](https://user-images.githubusercontent.com/49807271/160628685-29828c08-e185-4679-ae28-ea087cc84221.jpg)

7. サマリーのパスをダブルクリックすると、選択したパスでフィルタリングしてくれます。  
![手順8](https://user-images.githubusercontent.com/49807271/160630199-e788ad96-b6ff-4d81-bee5-b8b8fb3099a3.jpg)

8. シンボルが解決できていれば、スタックからソースにもジャンプ可能です。

