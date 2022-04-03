## WinDbg導入

### ■実験環境  
OS:Windows10  64bit

### ■ダンプ対象
64bitの.NETプログラム(.NetFramework 4.8)

### ■手順

1. WinDbgをダウンロードします。インストール手順も次のアドレスの記載通りです。    
https://docs.microsoft.com/ja-jp/windows-hardware/drivers/debugger/debugger-download-tools

2. ダンプ対象のプログラムをビルドします。  
私はVisualStudioでビルドしました。「32bitを選ぶ」の項目のチェックを外し、リリースモードにてビルド。
![64bit](https://user-images.githubusercontent.com/49807271/161406306-8b77148f-6d50-4903-b813-d1d75c415506.jpg)

3. 2で作成したexeファイルを実行し、タスクマネージャーを起動。  
※exeが32bitの場合、32bit版のタスクマネージャーを起動。  
  32bitプロセスの場合、タスクマネージャーの赤枠部に32bit表記が出ます。  
![タスクマネージャー](https://user-images.githubusercontent.com/49807271/161406490-cc1a97cb-a8ff-4b43-9b14-5887d2f5098f.jpg)

4. 詳細タブから起動プロセスを選択し、右クリックでダンプファイルの作成を選択。  
![タスクマネージャー](https://user-images.githubusercontent.com/49807271/161406609-7fb5005a-538d-4433-ae9b-163d8499a2d7.jpg)

5. WinDbg(X64)を起動し、4で作成したダンプファイルをOpenClushDumpから開きます。  
![WinDbg](https://user-images.githubusercontent.com/49807271/161406847-06dcf1a5-a1fd-4e92-8d3a-1b3b00c0893d.jpg)
  
6. コマンド「.loadby sos clr」を入力します。  
![WinDbg](https://user-images.githubusercontent.com/49807271/161406883-78971af4-a648-463c-aac3-cf1ceb8fc09e.jpg)

7.  「!chain」コマンドでロードされていることを確認します。  
![WinDbg](https://user-images.githubusercontent.com/49807271/161406923-00df4a13-03f2-4df4-aac5-b864aa725109.jpg)

8. 「.symfix <シンボルパス>」コマンドでシンボルを設定します。  
![WinDbg](https://user-images.githubusercontent.com/49807271/161407096-1b8e1c37-d3df-4579-bf48-1d409fc241fb.jpg)

9. 「sympath」コマンドでパスを確認します。  
![WinDbg](https://user-images.githubusercontent.com/49807271/161407124-6a5ba9b4-a63f-4245-bba0-af01ed3a9826.jpg)
    
10. 「.reload」コマンドで、シンボルをデバッガーに読み込みます。
![WinDbg](https://user-images.githubusercontent.com/49807271/161407220-7320c3a0-a713-46f9-b818-54e44a80d5e4.jpg)

■気を付けるポイント
* ダンプされるプログラムの.NetFrameworkのバージョン
* ダンプされるプログラムが32bit or 64bit
* タスクマネージャープログラムが32bit or 64bit
* WinDbgが32bit or 64 bit


