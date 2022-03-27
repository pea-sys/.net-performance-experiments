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
set _NT_SYMBOL_PATH="srv*C:\Users\user\Symbols*https://msdl.microsoft.com/download/symbols"
```

4.  同様にシステム環境変数_NT_SYMCASHE_PATHをディスクのローカルディレクトリに設定。  
前の手順のローカルシンボルキャッシュとは別のディレクトリであること。  
```
set _NT_SYMCACHE_PATH="C:\Users\user\Symcache"
```

5. 管理者権限でWPTをインストールしたディレクトリに移動する  
```
cd "C:\Program Files (x86)\Windows Kits\10\Windows Performance Toolkit"
```

6.  カーネルプロバイダーのデータを確認します

```
xperf -providers KF
       PROC_THREAD    : Process and Thread create/delete
       LOADER         : Kernel and user mode Image Load/Unload events
       PROFILE        : CPU Sample profile
       CSWITCH        : Context Switch
       COMPACT_CSWITCH: Compact Context Switch
       DISPATCHER     : CPU Scheduler
       DPC            : DPC Events
       INTERRUPT      : Interrupt events
       INTERRUPT_STEER: Interrupt Steering events
       WDF_DPC        : WDF DPC events
       WDF_INTERRUPT  : WDF Interrupt events
       SYSCALL        : System calls
       PRIORITY       : Priority change events
       SPINLOCK       : Spinlock Collisions
       KQUEUE         : Kernel Queue Enqueue/Dequeue
       ALPC           : Advanced Local Procedure Call
       PERF_COUNTER   : Process Perf Counters
       DISK_IO        : Disk I/O
       DISK_IO_INIT   : Disk I/O Initiation
       FILE_IO        : File system operation end times and results
       FILE_IO_INIT   : File system operation (create/open/close/read/write)
       HARD_FAULTS    : Hard Page Faults
       FILENAME       : FileName (e.g., FileName create/delete/rundown)
       SPLIT_IO       : Split I/O
       REGISTRY       : Registry tracing
       REG_HIVE       : Registry hive tracing
       DRIVERS        : Driver events
       POWER          : Power management events
       CC             : Cache manager events
       NETWORKTRACE   : Network events (e.g., tcp/udp send/receive)
       VIRT_ALLOC     : Virtual allocation reserve and release
       MEMINFO        : Memory List Info
       ALL_FAULTS     : All page faults including hard, Copy on write, demand zero faults, etc.
       MEMINFO_WS     : Working set Info
       VAMAP          : MapFile info
       FOOTPRINT      : Support footprint analysis
       MEMORY         : Memory tracing
       REFSET         : Support footprint analysis
       HIBERRUNDOWN   : Rundown(s) during hibernate
       CONTMEMGEN     : Contiguous Memory Generation
       POOL           : Pool tracing
       SHOULDYIELD    : Tracing for the cooperative DPC mechanism
       CPU_CONFIG     : NUMA topology, Processor Group and Processor Index to Number mapping. By default it is always enabled.
       SESSION        : Session rundown/create/delete events.
       IDLE_STATES    : CPU Idle States
       TIMER          : Timer settings and its expiration
       CLOCKINT       : Clock Interrupt Events
       IPI            : Inter-processor Interrupt Events
       OPTICAL_IO     : Optical I/O
       OPTICAL_IO_INIT: Optical I/O Initiation
       FLT_IO_INIT    : Minifilter callback initiation
       FLT_IO         : Minifilter callback completion
       FLT_FASTIO     : Minifilter fastio callback completion
       FLT_IO_FAILURE : Minifilter callback completion with failure
       OB_HANDLE      : Object handle events
       OB_OBJECT      : Object events
       KE_CLOCK       : Clock Configuration events
       PMC_PROFILE    : PMC sampling events
       DPC_QUEUE      : DPC queue events
       CACHE_FLUSH    : Cache flush events
       DEBUG_EVENTS   : Debugger scheduling events
```

7. 基本情報をトレース開始します。
```
xperf -on Base
```

8. 適当にファイルを開いたり、書き込みしたりしてトレースを停止します。
```
xperf -d KernelTrace.etl
Merged Etl: KernelTrace.etl
The trace you have just captured "KernelTrace.etl" may contain personally identifiable information, including but not necessarily limited to paths to files accessed, paths to registry accessed and process names. Exact information depends on the events that were logged. Please be aware of this when sharing out this trace with other people.
```

9.  WindowsPerformanceAnalyzerでKernelTraceを開きます。    
いくつかグラフを開いてみます。  
![手順9](https://user-images.githubusercontent.com/49807271/160281761-15c414b8-7db5-48d2-9eb7-377e8379601f.jpg)


10. タブメニューの[Load Symbols]を選択する。シンボルをロードすることで、より詳細な情報が見れるようになります。  
NET 4.0以上のランタイムのシンボルのデコードもサポートします。  
※マイクロソフトのシンボルサーバーからシンボルを初めて読み込むときは時間がかかることがある  
