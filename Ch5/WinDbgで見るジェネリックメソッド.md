### WinDbg で見るジェネリックメソッド

ジェネリック型が値型も参照型も効率良く処理できる仕組みの一部を  
WinDbg で確認します。  
結論を先に書いてしまうと、値型リストはそれぞれの型に合わせたメソッドを生成します。  
参照型は参照型共用のメソッドを生成します。

1. 同フォルダの「InnerGeneric」プロジェクトをビルドし、実行プログラムを作成します。
2. タスクマネージャーからダンプを取得します。
3. WinDbg でダンプを開きます。

```
Microsoft (R) Windows Debugger Version 10.0.22000.194 AMD64
Copyright (c) Microsoft Corporation. All rights reserved.


Loading Dump File [C:\Users\user\AppData\Local\Temp\InnerGeneric.DMP]
User Mini Dump File with Full Memory: Only application data is available

Symbol search path is: srv*
Executable search path is:
Windows 10 Version 19043 MP (4 procs) Free x64
Product: WinNt, suite: SingleUserTS
Edition build lab: 19041.1.amd64fre.vb_release.191206-1406
Machine Name:
Debug session time: Sun Apr 10 22:20:07.000 2022 (UTC + 9:00)
System Uptime: 14 days 14:45:10.370
Process Uptime: 0 days 0:00:11.000
............................
Loading unloaded module list
.
For analysis of this file, run !analyze -v
ntdll!NtReadFile+0x14:
00007ffc`cdc8ce34 c3              ret
```

4. sos モジュールをロードします。

```
0:000> .loadby sos clr
```

5. シンボルパスをセットします。

```
0:000> .symfix "C:\SymbolsWinDbg"
```

6. 構成をリロードします。

```
0:000>  .reload

............................
Loading unloaded module list
.
```

7. ヒープメモリのダンプ取得。

```
0:000> !dumpheap -stat
Statistics:
              MT    Count    TotalSize Class Name
00007ffc90e6a120        1           24 System.Collections.Generic.GenericEqualityComparer`1[[System.String, mscorlib]]
00007ffc90e64360        1           24 System.Collections.Generic.ObjectEqualityComparer`1[[System.Type, mscorlib]]
00007ffc90e63c40        1           24 System.Security.HostSecurityManager
00007ffc90eac388        1           32 System.IO.TextReader+SyncTextReader
00007ffc90e69cd0        1           32 System.Version
00007ffc90e695d0        1           32 Microsoft.Win32.SafeHandles.SafeFileHandle
00007ffc90e641c8        1           32 System.Security.Policy.AssemblyEvidenceFactory
00007ffc90e64078        1           32 Microsoft.Win32.SafeHandles.SafePEFileHandle
00007ffc90e63bc8        1           32 System.Security.Policy.Evidence+EvidenceLockHolder
00007ffc90e543b0        1           32 Microsoft.Win32.SafeHandles.SafeFileMappingHandle
00007ffc90e54320        1           32 Microsoft.Win32.SafeHandles.SafeViewOfFileHandle
00007ffc32e95f70        1           32 InnerGeneric.BasicStack`1[[System.Double, mscorlib]]
00007ffc32e95e58        1           32 InnerGeneric.BasicStack`1[[System.Int32, mscorlib]]
00007ffc32e95d68        1           32 InnerGeneric.BasicStack`1[[System.String, mscorlib]]
00007ffc32e95cd0        1           32 InnerGeneric.BasicStack`1[[System.Int32[], mscorlib]]
00007ffc90e54a58        1           40 System.IO.Stream+NullStream
00007ffc90e54510        1           40 Microsoft.Win32.Win32Native+InputRecord
00007ffc90e54218        1           40 System.Text.InternalEncoderBestFitFallback
00007ffc90ecfd28        1           48 System.Text.DBCSCodePageEncoding+DBCSDecoder
00007ffc90e65ef0        1           48 System.SharedStatics
00007ffc90e63d18        1           48 System.Text.UTF8Encoding+UTF8EncodingSealed
00007ffc90e542a8        1           48 System.Text.InternalDecoderBestFitFallback
00007ffc90e68d50        1           56 System.Text.UnicodeEncoding
00007ffc90e54590        1           56 System.IO.__ConsoleStream
00007ffc90e69168        2           64 System.Text.DecoderReplacementFallback
00007ffc90e690d8        2           64 System.Text.EncoderReplacementFallback
00007ffc90e66bb0        1           64 System.Security.PermissionSet
00007ffc90e64128        1           64 System.Security.Policy.PEFileEvidenceFactory
00007ffc90e63b48        1           64 System.Threading.ReaderWriterLock
00007ffc90e685a0        3           72 System.Int32
00007ffc90e66ab8        1           72 System.Security.Policy.Evidence
00007ffc90e69e98        1           80 System.Collections.Generic.Dictionary`2[[System.String, mscorlib],[System.Globalization.CultureData, mscorlib]]
00007ffc90e69750        1           80 System.Collections.Hashtable
00007ffc90e63278        1           80 System.Collections.Generic.Dictionary`2[[System.Type, mscorlib],[System.Security.Policy.EvidenceTypeDescriptor, mscorlib]]
00007ffc90e6ac40        4           96 System.UInt16
00007ffc90e6a378        1           96 System.Collections.Generic.Dictionary`2+Entry[[System.String, mscorlib],[System.Globalization.CultureData, mscorlib]][]
00007ffc90e69e00        2           96 System.Text.StringBuilder
00007ffc90e64478        1           96 System.Collections.Hashtable+bucket[]
00007ffc90ee5188        1          104 System.IO.StreamReader
00007ffc90e698e0        1          104 System.IO.UnmanagedMemoryStream
00007ffc90e63398        1          104 System.Type[]
00007ffc90e68a68        2          112 System.Reflection.RuntimeAssembly
00007ffc90ea6258        1          128 System.Text.DBCSCodePageEncoding
00007ffc90e66658        1          128 System.AppDomainSetup
00007ffc90e65dd8        6          144 System.Object
00007ffc90e69d90        1          160 System.Globalization.CalendarData
00007ffc90e65cf8        1          160 System.ExecutionEngineException
00007ffc90e65c80        1          160 System.StackOverflowException
00007ffc90e65c08        1          160 System.OutOfMemoryException
00007ffc90e65b70        1          160 System.Exception
00007ffc90e63ee8        1          208 System.Globalization.CalendarData[]
00007ffc90e6a440        1          216 System.Globalization.NumberFormatInfo
00007ffc90e65f60        1          216 System.AppDomain
00007ffc90e69b18        2          256 System.Globalization.CultureInfo
0000020433f462b0       11          288      Free
00007ffc90e6aaa0        2          304 System.Byte[]
00007ffc90e65d70        2          320 System.Threading.ThreadAbortException
00007ffc90e614c0        1          360 System.Int32[][]
00007ffc90e565c0        1          360 System.Double[]
00007ffc90e63ae0        3          720 System.Collections.Generic.Dictionary`2+Entry[[System.Type, mscorlib],[System.Security.Policy.EvidenceTypeDescriptor, mscorlib]][]
00007ffc90e667d0        4          768 System.Char[]
00007ffc90e68538       13          916 System.Int32[]
00007ffc90e69c78        2         1072 System.Globalization.CultureData
00007ffc90e67690       26         1456 System.RuntimeType
00007ffc90e63200       19         1576 System.String[]
00007ffc90e659c0      161         7428 System.String
00007ffc90e65e70        6        35488 System.Object[]
Total 320 objects

```

8. 今回はジェネリッククラスのオブジェクトの Push.Pop メソッドに注目します。

- 00007ffc32e95f70 1 32 InnerGeneric.BasicStack`1[[System.Double, mscorlib]]
- 00007ffc32e95e58 1 32 InnerGeneric.BasicStack`1[[System.Int32, mscorlib]]
- 00007ffc32e95d68 1 32 InnerGeneric.BasicStack`1[[System.String, mscorlib]]
- 00007ffc32e95cd0 1 32 InnerGeneric.BasicStack`1[[System.Int32[], mscorlib]]

9. Int32[]の参照型オブジェクトのリストのメソッドを確認。

```
0:000>  !dumpmt -md 00007ffc32e95cd0
EEClass:         00007ffc32e92688
Module:          00007ffc32e94148
Name:            InnerGeneric.BasicStack`1[[System.Int32[], mscorlib]]
mdToken:         0000000002000002
File:            C:\Users\user\source\repos\InnerGeneric\InnerGeneric\bin\Release\InnerGeneric.exe
BaseSize:        0x20
ComponentSize:   0x0
Slots in VTable: 7
Number of IFaces in IFaceMap: 0
--------------------------------------
MethodDesc Table
           Entry       MethodDesc    JIT Name
00007ffc913c3450 00007ffc90e48de8 PreJIT System.Object.ToString()
00007ffc913ddc60 00007ffc9100c180 PreJIT System.Object.Equals(System.Object)
00007ffc913c3090 00007ffc9100c1a8 PreJIT System.Object.GetHashCode()
00007ffc913c0420 00007ffc9100c1b0 PreJIT System.Object.Finalize()
00007ffc32fa04b8 00007ffc32e95bd0   NONE InnerGeneric.BasicStack`1[[System.__Canon, mscorlib]]..ctor(Int32)
00007ffc32fa04c0 00007ffc32e95bd8   NONE InnerGeneric.BasicStack`1[[System.__Canon, mscorlib]].Push(System.__Canon)
00007ffc32fa04c8 00007ffc32e95be0   NONE InnerGeneric.BasicStack`1[[System.__Canon, mscorlib]].Pop()

```

10. String の参照型オブジェクトのリストのメソッドを確認。  
    int32[]と同じメソッドアドレスになっています。

```
0:000>  !dumpmt -md 00007ffc32e95d68
EEClass:         00007ffc32e92688
Module:          00007ffc32e94148
Name:            InnerGeneric.BasicStack`1[[System.String, mscorlib]]
mdToken:         0000000002000002
File:            C:\Users\user\source\repos\InnerGeneric\InnerGeneric\bin\Release\InnerGeneric.exe
BaseSize:        0x20
ComponentSize:   0x0
Slots in VTable: 7
Number of IFaces in IFaceMap: 0
--------------------------------------
MethodDesc Table
           Entry       MethodDesc    JIT Name
00007ffc913c3450 00007ffc90e48de8 PreJIT System.Object.ToString()
00007ffc913ddc60 00007ffc9100c180 PreJIT System.Object.Equals(System.Object)
00007ffc913c3090 00007ffc9100c1a8 PreJIT System.Object.GetHashCode()
00007ffc913c0420 00007ffc9100c1b0 PreJIT System.Object.Finalize()
00007ffc32fa04b8 00007ffc32e95bd0   NONE InnerGeneric.BasicStack`1[[System.__Canon, mscorlib]]..ctor(Int32)
00007ffc32fa04c0 00007ffc32e95bd8   NONE InnerGeneric.BasicStack`1[[System.__Canon, mscorlib]].Push(System.__Canon)
00007ffc32fa04c8 00007ffc32e95be0   NONE InnerGeneric.BasicStack`1[[System.__Canon, mscorlib]].Pop()

```

11. int の値型リストのメソッドを確認。
    メソッドアドレスが異なります。引数も Int32 になっています。

```
0:000>  !dumpmt -md 00007ffc32e95e58
EEClass:         00007ffc32e927b0
Module:          00007ffc32e94148
Name:            InnerGeneric.BasicStack`1[[System.Int32, mscorlib]]
mdToken:         0000000002000002
File:            C:\Users\user\source\repos\InnerGeneric\InnerGeneric\bin\Release\InnerGeneric.exe
BaseSize:        0x20
ComponentSize:   0x0
Slots in VTable: 7
Number of IFaces in IFaceMap: 0
--------------------------------------
MethodDesc Table
           Entry       MethodDesc    JIT Name
00007ffc913c3450 00007ffc90e48de8 PreJIT System.Object.ToString()
00007ffc913ddc60 00007ffc9100c180 PreJIT System.Object.Equals(System.Object)
00007ffc913c3090 00007ffc9100c1a8 PreJIT System.Object.GetHashCode()
00007ffc913c0420 00007ffc9100c1b0 PreJIT System.Object.Finalize()
00007ffc32fa04d8 00007ffc32e95e28   NONE InnerGeneric.BasicStack`1[[System.Int32, mscorlib]]..ctor(Int32)
00007ffc32fa04e0 00007ffc32e95e30   NONE InnerGeneric.BasicStack`1[[System.Int32, mscorlib]].Push(Int32)
00007ffc32fa04e8 00007ffc32e95e38   NONE InnerGeneric.BasicStack`1[[System.Int32, mscorlib]].Pop()

```

12. double の値型リストのメソッドを確認。
    メソッドアドレスが異なります。引数も double になっています。

```
0:000>  !dumpmt -md 00007ffc32e95f70
EEClass:         00007ffc32e92840
Module:          00007ffc32e94148
Name:            InnerGeneric.BasicStack`1[[System.Double, mscorlib]]
mdToken:         0000000002000002
File:            C:\Users\user\source\repos\InnerGeneric\InnerGeneric\bin\Release\InnerGeneric.exe
BaseSize:        0x20
ComponentSize:   0x0
Slots in VTable: 7
Number of IFaces in IFaceMap: 0
--------------------------------------
MethodDesc Table
           Entry       MethodDesc    JIT Name
00007ffc913c3450 00007ffc90e48de8 PreJIT System.Object.ToString()
00007ffc913ddc60 00007ffc9100c180 PreJIT System.Object.Equals(System.Object)
00007ffc913c3090 00007ffc9100c1a8 PreJIT System.Object.GetHashCode()
00007ffc913c0420 00007ffc9100c1b0 PreJIT System.Object.Finalize()
00007ffc32fa04f8 00007ffc32e95f40   NONE InnerGeneric.BasicStack`1[[System.Double, mscorlib]]..ctor(Int32)
00007ffc32fa0500 00007ffc32e95f48   NONE InnerGeneric.BasicStack`1[[System.Double, mscorlib]].Push(Double)
00007ffc32fa0508 00007ffc32e95f50   NONE InnerGeneric.BasicStack`1[[System.Double, mscorlib]].Pop()

```

13. 参照型の Push メソッドを逆アセンブルします。

```
0:000> !u 00007ffc32fa04c0
Unmanaged code
00007ffc`32fa04c0 e84b40565f      call    clr!PrecodeFixupThunk (00007ffc`92504510)
00007ffc`32fa04c5 5e              pop     rsi
00007ffc`32fa04c6 0101            add     dword ptr [rcx],eax
00007ffc`32fa04c8 e84340565f      call    clr!PrecodeFixupThunk (00007ffc`92504510)
00007ffc`32fa04cd 5e              pop     rsi
00007ffc`32fa04ce 0200            add     al,byte ptr [rax]
00007ffc`32fa04d0 d05be9          rcr     byte ptr [rbx-17h],1
00007ffc`32fa04d3 32fc            xor     bh,ah
00007ffc`32fa04d5 7f00            jg      00007ffc`32fa04d7
00007ffc`32fa04d7 00e8            add     al,ch

```

14. int32 型の Push メソッドを逆アセンブルします。

```
0:000> !u 00007ffc32fa04e0
Unmanaged code
00007ffc`32fa04e0 e82b40565f      call    clr!PrecodeFixupThunk (00007ffc`92504510)
00007ffc`32fa04e5 5e              pop     rsi
00007ffc`32fa04e6 0101            add     dword ptr [rcx],eax
00007ffc`32fa04e8 e82340565f      call    clr!PrecodeFixupThunk (00007ffc`92504510)
00007ffc`32fa04ed 5e              pop     rsi
00007ffc`32fa04ee 0200            add     al,byte ptr [rax]
00007ffc`32fa04f0 285ee9          sub     byte ptr [rsi-17h],bl
00007ffc`32fa04f3 32fc            xor     bh,ah
00007ffc`32fa04f5 7f00            jg      00007ffc`32fa04f7
00007ffc`32fa04f7 00e8            add     al,ch

```

15. double 型の Push メソッドを逆アセンブルします。

```
0:000> !u 00007ffc32fa0500
Unmanaged code
00007ffc`32fa0500 e80b40565f      call    clr!PrecodeFixupThunk (00007ffc`92504510)
00007ffc`32fa0505 5e              pop     rsi
00007ffc`32fa0506 0101            add     dword ptr [rcx],eax
00007ffc`32fa0508 e80340565f      call    clr!PrecodeFixupThunk (00007ffc`92504510)
00007ffc`32fa050d 5e              pop     rsi
00007ffc`32fa050e 0200            add     al,byte ptr [rax]
00007ffc`32fa0510 405f            pop     rdi
00007ffc`32fa0512 e932fc7f00      jmp     00007ffc`337a0149
00007ffc`32fa0517 0000            add     byte ptr [rax],al
00007ffc`32fa0519 0000            add     byte ptr [rax],al

```
