### WinDbgによるオブジェクトルート確認(編集中)

Microsoft (R) Windows Debugger Version 10.0.22000.194 AMD64
Copyright (c) Microsoft Corporation. All rights reserved.


Loading Dump File [C:\Users\user\AppData\Local\Temp\GC_StaticRoute.DMP]
User Mini Dump File with Full Memory: Only application data is available

Symbol search path is: srv*
Executable search path is: 
Windows 10 Version 19043 MP (4 procs) Free x64
Product: WinNt, suite: SingleUserTS
Edition build lab: 19041.1.amd64fre.vb_release.191206-1406
Machine Name:
Debug session time: Thu Apr  7 06:54:42.000 2022 (UTC + 9:00)
System Uptime: 10 days 23:19:40.857
Process Uptime: 0 days 0:00:07.000
............................
Loading unloaded module list
.
For analysis of this file, run !analyze -v
ntdll!NtDelayExecution+0x14:
00007ffc`cdc8d3f4 c3              ret
0:000> .loadby sos clr
0:000> .symfix "C:\SymbolsWinDbg"
0:000> .reload
............................
Loading unloaded module list
.
0:000> !DumpHeap -stat
Statistics:
              MT    Count    TotalSize Class Name
00007ffc8b01f458        1           24 System.AppDomainPauseManager
00007ffc8b004360        1           24 System.Collections.Generic.ObjectEqualityComparer`1[[System.Type, mscorlib]]
00007ffc8b003c40        1           24 System.Security.HostSecurityManager
00007ffc8b0067d0        1           28 System.Char[]
00007ffc8b0041c8        1           32 System.Security.Policy.AssemblyEvidenceFactory
00007ffc8b004078        1           32 Microsoft.Win32.SafeHandles.SafePEFileHandle
00007ffc8b003bc8        1           32 System.Security.Policy.Evidence+EvidenceLockHolder
00007ffc8b005ef0        1           48 System.SharedStatics
00007ffc8b005dd8        2           48 System.Object
00007ffc8b008a68        1           56 System.Reflection.RuntimeAssembly
00007ffc8b006bb0        1           64 System.Security.PermissionSet
00007ffc8b004128        1           64 System.Security.Policy.PEFileEvidenceFactory
00007ffc8b003b48        1           64 System.Threading.ReaderWriterLock
00007ffc8b006ab8        1           72 System.Security.Policy.Evidence
00007ffc8b003278        1           80 System.Collections.Generic.Dictionary`2[[System.Type, mscorlib],[System.Security.Policy.EvidenceTypeDescriptor, mscorlib]]
00007ffc8b003398        1          104 System.Type[]
00007ffc8b006658        1          128 System.AppDomainSetup
00007ffc8b005cf8        1          160 System.ExecutionEngineException
00007ffc8b005c80        1          160 System.StackOverflowException
00007ffc8b005c08        1          160 System.OutOfMemoryException
00007ffc8b005b70        1          160 System.Exception
00007ffc8b003200        2          192 System.String[]
00007ffc8b005f60        1          216 System.AppDomain
0000020013ae6250       10          264      Free
00007ffc8b005d70        2          320 System.Threading.ThreadAbortException
00007ffc8b008538        4          492 System.Int32[]
00007ffc8b003ae0        3          720 System.Collections.Generic.Dictionary`2+Entry[[System.Type, mscorlib],[System.Security.Policy.EvidenceTypeDescriptor, mscorlib]][]
00007ffc8b007690       20         1120 System.RuntimeType
00007ffc30725b48       62         1488 GC_StaticRoute.Button
00007ffc8b0059c0       37         2812 System.String
00007ffc8b080e30      123         7872 System.EventHandler
00007ffc8b005e70       10        36472 System.Object[]
Total 296 objects
0:000> !DumpHeap -type System.Object[]
         Address               MT     Size
00000200155a2ea0 00007ffc8b005e70       40     
00000200155a2f60 00007ffc8b005e70       56     
00000200155a30c8 00007ffc8b005e70       88     
00000200155a3380 00007ffc8b005e70      152     
00000200155a38d8 00007ffc8b005e70      280     
00000200155a4388 00007ffc8b005e70      536     
00000200255a1038 00007ffc8b005e70     9744     
00000200255a3668 00007ffc8b005e70     1048     
00000200255a3aa0 00007ffc8b005e70     8184     
00000200255a5ab8 00007ffc8b005e70    16344     

Statistics:
              MT    Count    TotalSize Class Name
00007ffc8b005e70       10        36472 System.Object[]
Total 10 objects
0:000> !GCRoot 00007ffc8b005e70
Found 0 unique roots (run '!GCRoot -all' to see all roots).
0:000> !DumpHeap -type System.EventHandler
         Address               MT     Size
00000200155a2df0 00007ffc8b080e30       64     
00000200155a2e60 00007ffc8b080e30       64     
00000200155a2ec8 00007ffc8b080e30       64     
00000200155a2f20 00007ffc8b080e30       64     
00000200155a2f98 00007ffc8b080e30       64     
00000200155a2ff0 00007ffc8b080e30       64     
00000200155a3030 00007ffc8b080e30       64     
00000200155a3088 00007ffc8b080e30       64     
00000200155a3120 00007ffc8b080e30       64     
00000200155a3178 00007ffc8b080e30       64     
00000200155a31b8 00007ffc8b080e30       64     
00000200155a3210 00007ffc8b080e30       64     
00000200155a3250 00007ffc8b080e30       64     
00000200155a32a8 00007ffc8b080e30       64     
00000200155a32e8 00007ffc8b080e30       64     
00000200155a3340 00007ffc8b080e30       64     
00000200155a3418 00007ffc8b080e30       64     
00000200155a3470 00007ffc8b080e30       64     
00000200155a34b0 00007ffc8b080e30       64     
00000200155a3508 00007ffc8b080e30       64     
00000200155a3548 00007ffc8b080e30       64     
00000200155a35a0 00007ffc8b080e30       64     
00000200155a35e0 00007ffc8b080e30       64     
00000200155a3638 00007ffc8b080e30       64     
00000200155a3678 00007ffc8b080e30       64     
00000200155a36d0 00007ffc8b080e30       64     
00000200155a3710 00007ffc8b080e30       64     
00000200155a3768 00007ffc8b080e30       64     
00000200155a37a8 00007ffc8b080e30       64     
00000200155a3800 00007ffc8b080e30       64     
00000200155a3840 00007ffc8b080e30       64     
00000200155a3898 00007ffc8b080e30       64     
00000200155a39f0 00007ffc8b080e30       64     
00000200155a3a48 00007ffc8b080e30       64     
00000200155a3a88 00007ffc8b080e30       64     
00000200155a3ae0 00007ffc8b080e30       64     
00000200155a3b20 00007ffc8b080e30       64     
00000200155a3b78 00007ffc8b080e30       64     
00000200155a3bb8 00007ffc8b080e30       64     
00000200155a3c10 00007ffc8b080e30       64     
00000200155a3c50 00007ffc8b080e30       64     
00000200155a3ca8 00007ffc8b080e30       64     
00000200155a3ce8 00007ffc8b080e30       64     
00000200155a3d40 00007ffc8b080e30       64     
00000200155a3d80 00007ffc8b080e30       64     
00000200155a3dd8 00007ffc8b080e30       64     
00000200155a3e18 00007ffc8b080e30       64     
00000200155a3e70 00007ffc8b080e30       64     
00000200155a3eb0 00007ffc8b080e30       64     
00000200155a3f08 00007ffc8b080e30       64     
00000200155a3f48 00007ffc8b080e30       64     
00000200155a3fb8 00007ffc8b080e30       64     
00000200155a3ff8 00007ffc8b080e30       64     
00000200155a4050 00007ffc8b080e30       64     
00000200155a4090 00007ffc8b080e30       64     
00000200155a40e8 00007ffc8b080e30       64     
00000200155a4128 00007ffc8b080e30       64     
00000200155a4180 00007ffc8b080e30       64     
00000200155a41c0 00007ffc8b080e30       64     
00000200155a4218 00007ffc8b080e30       64     
00000200155a4258 00007ffc8b080e30       64     
00000200155a42b0 00007ffc8b080e30       64     
00000200155a42f0 00007ffc8b080e30       64     
00000200155a4348 00007ffc8b080e30       64     
00000200155a45a0 00007ffc8b080e30       64     
00000200155a45f8 00007ffc8b080e30       64     
00000200155a4638 00007ffc8b080e30       64     
00000200155a4690 00007ffc8b080e30       64     
00000200155a46d0 00007ffc8b080e30       64     
00000200155a4728 00007ffc8b080e30       64     
00000200155a4768 00007ffc8b080e30       64     
00000200155a47c0 00007ffc8b080e30       64     
00000200155a4800 00007ffc8b080e30       64     
00000200155a4858 00007ffc8b080e30       64     
00000200155a4898 00007ffc8b080e30       64     
00000200155a48f0 00007ffc8b080e30       64     
00000200155a4930 00007ffc8b080e30       64     
00000200155a4988 00007ffc8b080e30       64     
00000200155a49c8 00007ffc8b080e30       64     
00000200155a4a20 00007ffc8b080e30       64     
00000200155a4a60 00007ffc8b080e30       64     
00000200155a4ab8 00007ffc8b080e30       64     
00000200155a4af8 00007ffc8b080e30       64     
00000200155a4b50 00007ffc8b080e30       64     
00000200155a4b90 00007ffc8b080e30       64     
00000200155a4be8 00007ffc8b080e30       64     
00000200155a4c28 00007ffc8b080e30       64     
00000200155a4c80 00007ffc8b080e30       64     
00000200155a4cc0 00007ffc8b080e30       64     
00000200155a4d18 00007ffc8b080e30       64     
00000200155a4d58 00007ffc8b080e30       64     
00000200155a4db0 00007ffc8b080e30       64     
00000200155a4df0 00007ffc8b080e30       64     
00000200155a4e48 00007ffc8b080e30       64     
00000200155a4e88 00007ffc8b080e30       64     
00000200155a4ee0 00007ffc8b080e30       64     
00000200155a4f20 00007ffc8b080e30       64     
00000200155a4f78 00007ffc8b080e30       64     
00000200155a4fb8 00007ffc8b080e30       64     
00000200155a5010 00007ffc8b080e30       64     
00000200155a5050 00007ffc8b080e30       64     
00000200155a50a8 00007ffc8b080e30       64     
00000200155a50e8 00007ffc8b080e30       64     
00000200155a5140 00007ffc8b080e30       64     
00000200155a5180 00007ffc8b080e30       64     
00000200155a51d8 00007ffc8b080e30       64     
00000200155a5218 00007ffc8b080e30       64     
00000200155a5270 00007ffc8b080e30       64     
00000200155a52b0 00007ffc8b080e30       64     
00000200155a5308 00007ffc8b080e30       64     
00000200155a5348 00007ffc8b080e30       64     
00000200155a53a0 00007ffc8b080e30       64     
00000200155a53e0 00007ffc8b080e30       64     
00000200155a5438 00007ffc8b080e30       64     
00000200155a5478 00007ffc8b080e30       64     
00000200155a54d0 00007ffc8b080e30       64     
00000200155a5510 00007ffc8b080e30       64     
00000200155a5568 00007ffc8b080e30       64     
00000200155a55a8 00007ffc8b080e30       64     
00000200155a5600 00007ffc8b080e30       64     
00000200155a5640 00007ffc8b080e30       64     
00000200155a5698 00007ffc8b080e30       64     
00000200155a56d8 00007ffc8b080e30       64     

Statistics:
              MT    Count    TotalSize Class Name
00007ffc8b080e30      123         7872 System.EventHandler
Total 123 objects
0:000> !GCRoot 00007ffc8b080e30
Found 0 unique roots (run '!GCRoot -all' to see all roots).
