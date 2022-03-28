## PerfViewによるGCイベント確認

### ■実験環境  
OS:Windows10  

### ■手順

1. PerfViewをダウンロードします。  
https://github.com/microsoft/perfview/releases


2. PerfViewを起動します。

    ![手順2](https://user-images.githubusercontent.com/49807271/160402816-d2746ab6-35ef-487b-9935-7e722ee7505e.jpg)
    
3. 次のプログラムを起動します。  
https://github.com/Apress/pro-.net-perf/blob/master/Ch02/MemoryLeak.exe


4.  PerfViewのCollectタブメニューからCollectを選択します。  
    FocusProcessにプロセスファイル名を入力します。  
    AdvancedOptionを開き、GC CollectOnlyとGC Onlyにチェック入れます。  
![手順4](https://user-images.githubusercontent.com/49807271/160404723-fcd36642-d1cc-44bc-a829-73b7cbc13668.jpg)


5.  PerfViewのStartCollectionボタンを押します。  
6.  収集完了のタイミングでStopCollectionボタンを押します。
    変換処理が終わるまで待ちます(収集ターゲットが多い場合は時間を要します)。  
    
7.  Eventsを選択します。
![手順7](https://user-images.githubusercontent.com/49807271/160406679-d20d2f81-dfd4-4f57-95d4-d20c2febfee0.jpg)

8.  Eventの一覧が表示されます。ここでGC関連のイベントが確認できます。  
![手順8](https://user-images.githubusercontent.com/49807271/160408127-efc81b69-2b65-42a8-b270-8a2ae0f3a3b1.jpg)

9.  Memory → StatsからGCに関連するメモリの統計情報等も確認できます。  
![メモリ統計](https://user-images.githubusercontent.com/49807271/160408900-e8e2a8e6-842d-41ab-9de9-440e95eb2ac3.jpg)
