### WinDbgでメソッドテーブル確認

■実験環境
OS:Windows10 64bit

■ダンプ対象
64bitの.NETプログラム(.NetFramework 4.8)

対象プログラム
```
namespace Sample
{
    internal class Program
    {
        class MyClass
        {
            public void M()
            {
            }
        }

        static void Main(string[] args)
        {
            MyClass m = new MyClass();
            m.M();
            Console.ReadLine();
        }
    }
}
```
WinDbgでの確認結果
```
0:000> !clrstack -a
OS Thread Id: 0x4394 (0)
        Child SP               IP Call Site
0000009f6f0fe858 00007ffccdc8ce34 [InlinedCallFrame: 0000009f6f0fe858] Microsoft.Win32.Win32Native.ReadFile(Microsoft.Win32.SafeHandles.SafeFileHandle, Byte*, Int32, Int32 ByRef, IntPtr)
0000009f6f0fe858 00007ffc8308c9c8 [InlinedCallFrame: 0000009f6f0fe858] Microsoft.Win32.Win32Native.ReadFile(Microsoft.Win32.SafeHandles.SafeFileHandle, Byte*, Int32, Int32 ByRef, IntPtr)
0000009f6f0fe820 00007ffc8308c9c8 DomainNeutralILStubClass.IL_STUB_PInvoke(Microsoft.Win32.SafeHandles.SafeFileHandle, Byte*, Int32, Int32 ByRef, IntPtr)
    PARAMETERS:
        <no data>
        <no data>
        <no data>
        <no data>
        <no data>

0000009f6f0fe900 00007ffc838a3a5c System.IO.__ConsoleStream.ReadFileNative(Microsoft.Win32.SafeHandles.SafeFileHandle, Byte[], Int32, Int32, Boolean, Boolean, Int32 ByRef)
    PARAMETERS:
        hFile = <no data>
        bytes = <no data>
        offset = <no data>
        count = <no data>
        useFileAPIs = <no data>
        isPipe = <no data>
        bytesRead = <no data>
    LOCALS:
        <no data>
        <no data>
        <no data>
        <no data>
        <no data>
        <no data>

0000009f6f0fe990 00007ffc838a3965 System.IO.__ConsoleStream.Read(Byte[], Int32, Int32)
    PARAMETERS:
        this = <no data>
        buffer = <no data>
        offset = <no data>
        count = <no data>
    LOCALS:
        <no data>
        <no data>

0000009f6f0fe9f0 00007ffc830582d4 System.IO.StreamReader.ReadBuffer()
    PARAMETERS:
        this (<CLR reg>) = 0x0000023915ec5568
    LOCALS:
        <no data>
        <no data>

0000009f6f0fea40 00007ffc83058773 System.IO.StreamReader.ReadLine()
    PARAMETERS:
        this (<CLR reg>) = 0x0000023915ec5568
    LOCALS:
        <no data>
        <no data>
        <no data>
        <no data>

0000009f6f0feaa0 00007ffc83a4a49d System.IO.TextReader+SyncTextReader.ReadLine()
    PARAMETERS:
        this (<CLR reg>) = 0x0000023915ec59a8

0000009f6f0feb00 00007ffc83842728 System.Console.ReadLine()

0000009f6f0feb30 00007ffc24b108f7 *** WARNING: Unable to verify checksum for Sample.exe
Sample.Program.Main(System.String[]) [C:\Users\user\source\repos\Sample\Sample\Program.cs @ 22]
    PARAMETERS:
        args (0x0000009f6f0feb90) = 0x0000023915ec2cf0
    LOCALS:
        0x0000009f6f0feb68 = 0x0000023915ec2d08

0000009f6f0fed78 00007ffc84056913 [GCFrame: 0000009f6f0fed78] 
0:000> !do 0x0000023915ec2d08
Name:        Sample.Program+MyClass
MethodTable: 00007ffc24a05ac8
EEClass:     00007ffc24a02548
Size:        24(0x18) bytes
File:        C:\Users\user\source\repos\Sample\Sample\bin\Debug\Sample.exe
Fields:
None
0:000> !dumpmt -md 00007ffc24a05ac8
EEClass:         00007ffc24a02548
Module:          00007ffc24a04148
Name:            Sample.Program+MyClass
mdToken:         0000000002000003
File:            C:\Users\user\source\repos\Sample\Sample\bin\Debug\Sample.exe
BaseSize:        0x18
ComponentSize:   0x0
Slots in VTable: 6
Number of IFaces in IFaceMap: 0
--------------------------------------
MethodDesc Table
           Entry       MethodDesc    JIT Name
00007ffc82fd3450 00007ffc82a58de8 PreJIT System.Object.ToString()
00007ffc82fedc60 00007ffc82c1c180 PreJIT System.Object.Equals(System.Object)
00007ffc82fd3090 00007ffc82c1c1a8 PreJIT System.Object.GetHashCode()
00007ffc82fd0420 00007ffc82c1c1b0 PreJIT System.Object.Finalize()
00007ffc24b10920 00007ffc24a05ac0    JIT Sample.Program+MyClass..ctor()
00007ffc24b10970 00007ffc24a05ab0    JIT Sample.Program+MyClass.M()
0:000> dd  00007ffc24b10970
00007ffc`24b10970  ec834855 6c8d4820 89482024 3d83104d
00007ffc`24b10980  ffef3c43 e8057400 5fa4c0c4 8d489090
00007ffc`24b10990  c35d0065 00020519 50013205 00000040
00007ffc`24b109a0  00000000 00000000 00000000 00000000
00007ffc`24b109b0  00000000 00000000 00000000 00000000
00007ffc`24b109c0  00000000 00000000 00000000 00000000
00007ffc`24b109d0  00000000 00000000 00000000 00000000
00007ffc`24b109e0  00000000 00000000 00000000 00000000
```
