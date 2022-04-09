### WinDbgで見るFinalizeQueue
FinalizeQueueはファイナライズ処理が実装されたオブジェクトの参照を保持するキューです。    
アプリケーションがオブジェクトを参照しなくなると、GCがオブジェクト参照をFリーチャブルキューに移動する。  
ファイナライザースレッドがアクティブになると、Fリーチャブルキューに移動したオブジェクトのファイナライズ処理後、解放します。

手順はほとんど以下のURLを参考にしています。  
https://troushoo.blog.fc2.com/blog-entry-56.html

1. 次のプログラムを実行します。
   オブジェクト作成よりも、ファイナライズに時間が掛かるクラスを延々と作成します。
   
```
class Files2
    {
        public Files2()
        {
            Thread.Sleep(10);
        }
        ~Files2()
        {
            Thread.Sleep(20);
        }
        private byte[] data = new byte[1024];
    }
    internal class Program
    {
        static void Main(string[] args)
        {

            while (true)
            {
                Files2 files = new Files2();
            }
        }
    }
```

2. タスクマネージャーでダンプを取ります。  
※Win32bitプロセスの場合、32bitタスクマネージャーでダンプを取ります。  

3. WinDbgを起動し、ダンプファイルをドラッグアンドドロップします。  
4. sosモジュールをロードします。  
```
0:000> .loadby sos clr
```
5. シンボルパスを設定します。
```
0:000> .symfix "C:\SymbolsWinDbg"
```
6. リロードします。
```
0:000>  .reload
```

7. FinalizeQueueを確認します。  
744個のオブジェクトはファイナライズ待ちの状態です。  
つまり、Fリーチャブルキューに移動済みのオブジェクト参照です。  
めっちゃ詰まってます。ここの数字が0以上だとアプリケーションに問題がありそうです。  
```
0:000> !finalizequeue
SyncBlocks to be cleaned up: 0
Free-Threaded Interfaces to be released: 0
MTA Interfaces to be released: 0
STA Interfaces to be released: 0
----------------------------------
generation 0 has 1406 finalizable objects (00000224c40aea68->00000224c40b1658)
generation 1 has 3 finalizable objects (00000224c40aea50->00000224c40aea68)
generation 2 has 0 finalizable objects (00000224c40aea50->00000224c40aea50)
Ready for finalization 744 objects (00000224c40b1658->00000224c40b2d98)
Statistics for all finalizable objects (including all objects ready for finalization):
              MT    Count    TotalSize Class Name
00007ffc8ce74078        1           32 Microsoft.Win32.SafeHandles.SafePEFileHandle
00007ffc8ce73b48        1           64 System.Threading.ReaderWriterLock
00007ffc310c5af8     2151        51624 FinalizerMemoryLeak.Files2
Total 2153 objects
```

8. ファイナライザー実行スレッドを確認します。
```
0:000> !threads
ThreadCount:      2
UnstartedThread:  0
BackgroundThread: 1
PendingThread:    0
DeadThread:       0
Hosted Runtime:   no
                                                                                                        Lock  
       ID OSID ThreadOBJ           State GC Mode     GC Alloc Context                  Domain           Count Apt Exception
   0    1 2eac 00000224c404f660  202a020 Preemptive  00000224C5E326A0:00000224C5E33FD0 00000224c4024c10 0     MTA 
   3    2 5578 00000224c4079240  202b220 Preemptive  0000000000000000:0000000000000000 00000224c4024c10 0     MTA (Finalizer) 
```

9. ファイナライザー実行スレッドの状況を確認します。  
   スレッドコンテキストを切り替えます。
```
0:002> ~3s
ntdll!NtDelayExecution+0x14:
00007ffc`cdc8d3f4 c3              ret
0:003> !clrstack
```

10. 該当スレッドのスタックを確認します。  
    Files2クラスのFinalize実行中です。
```
0:003> !clrstack
OS Thread Id: 0x5578 (3)
        Child SP               IP Call Site
0000009aa94ff428 00007ffccdc8d3f4 [HelperMethodFrame: 0000009aa94ff428] System.Threading.Thread.SleepInternal(Int32)
0000009aa94ff520 00007ffc311d092e *** WARNING: Unable to verify checksum for FinalizerMemoryLeak.exe
FinalizerMemoryLeak.Files2.Finalize() [C:\Users\user\source\repos\FinalizerMemoryLeak\FinalizerMemoryLeak\Program.cs @ 19]
0000009aa94ff970 00007ffc906f6886 [DebuggerU2MCatchHandlerFrame: 0000009aa94ff970] 
```

