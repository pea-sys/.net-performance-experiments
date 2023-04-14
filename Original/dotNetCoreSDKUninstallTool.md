# dotNetCoreSDK UninstallTool

dotNet で開発を行っていると、度々 SDK が複数インストールされる状況になる。  
![list](https://user-images.githubusercontent.com/49807271/232046770-003f718e-3294-4999-8acf-1c796351d67c.png)
しかも、１つ１つのプログラムサイズが 500MB 以上でそれなりにでデカい。

全て使用されているのであれば良いが中には未使用のものもある。  
そういったものを安全に削除できるように MicroSoft から未使用の dotNetSDK を削除するツールが以下のサイトで提供されている。

- https://learn.microsoft.com/ja-jp/dotnet/core/additional-tools/uninstall-tool?tabs=windows

インストールは 2 ステップ位で特に選択肢はない。  
![dotNetCoreSDKUninstallTool](https://user-images.githubusercontent.com/49807271/232046389-e4290b76-b958-4495-8d00-4aa3f6add984.png)

使ってみます。
うん。半分以上使ってない。

```
PS C:\Windows\System32> dotnet-core-uninstall list

This tool cannot uninstall versions of the runtime or SDK that are?
    - SDKs installed using Visual Studio 2019 Update 3 or later.
    - SDKs and runtimes installed via zip/scripts.
    - Runtimes installed with SDKs (these should be removed by removing that SDK).
The versions that can be uninstalled with this tool are:

.NET Core SDKs:
  7.0.105  x64    [Used by Visual Studio. Specify individually or use ?-force to remove]
  6.0.408  x64
  6.0.311  x64
  3.1.426  x64    [Used by Visual Studio 2019. Specify individually or use ?-force to remove]
  3.0.103  x64

.NET Core Runtimes:
  3.1.32  x86
  3.1.32  x64

ASP.NET Core Runtimes:
  7.0.0   x64
  6.0.16  x86
  3.1.32  x86
  3.1.32  x64

.NET Core Runtime & Hosting Bundles:
  3.1.32
```

消す前にディスク容量の確認をする。

```
PS C:\Windows\System32> df -h
Filesystem            Size  Used Avail Use% Mounted on
C:/Program Files/Git  190G  169G   22G  89% /
E:                     23G  2.8G   20G  13% /e
F:                    120G  119G  296M 100% /f
H:                     15G   32K   15G   1% /h
```

dry-run で何が消されるか確認する。

```
PS C:\Windows\System32> dotnet-core-uninstall dry-run --all --sdk
*** DRY RUN OUTPUT
Specified versions:
  Microsoft .NET SDK 6.0.408 (x64)
  Microsoft .NET SDK 6.0.311 (x64)
  Microsoft .NET Core SDK 3.0.103 (x64)
*** END DRY RUN OUTPUT

To avoid breaking Visual Studio or other problems, read https://aka.ms/dotnet-core-uninstall-docs.

Run as administrator and use the remove command to uninstall these items.
```

実際に消します

```
PS C:\Windows\System32> dotnet-core-uninstall remove --all --sdk
The following items will be removed:
  Microsoft .NET SDK 6.0.408 (x64)
  Microsoft .NET SDK 6.0.311 (x64)
  Microsoft .NET Core SDK 3.0.103 (x64)

To avoid breaking Visual Studio or other problems, read https://aka.ms/dotnet-core-uninstall-docs.

Do you want to continue? [y/n] y
Uninstalling: Microsoft .NET SDK 6.0.408 (x64).
Uninstalling: Microsoft .NET SDK 6.0.311 (x64).
Uninstalling: Microsoft .NET Core SDK 3.0.103 (x64).
```

ディスク空き容量を確認

```
PS C:\Windows\System32> df -h
Filesystem            Size  Used Avail Use% Mounted on
C:/Program Files/Git  190G  167G   24G  88% /
E:                     23G  2.8G   20G  13% /e
F:                    120G  119G  296M 100% /f
H:                     15G   32K   15G   1% /h
```

2GB 位空き容量が増えました。
