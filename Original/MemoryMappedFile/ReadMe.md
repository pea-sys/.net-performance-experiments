# MemoryMappedFile
ファイルI/Oのパフォーマンス改善手段としてMemoryMappedFileがある。  
ファイルをメモリに割り当てて、それに対して読み書きする。  
以下のサイトにあるソースをちょっとだけいじって、パフォーマンスの改善に効果があるかを確認する。
https://visualstudiomagazine.com/articles/2010/06/23/memory-mapped-files.aspx

計測結果は以下の通り。初回の速度差が最も大きい。初回以外もおよそ倍程度のパフォーマンスに差異がある。  
ただし、MemoryMappedFileを使うとメモリーをその分消費するので
小さいファイルの読み書きで利用するのが良さそう。
```
MMF method    0 : 28
Bitmap method 0 : 88
MMF method    1 : 13
Bitmap method 1 : 26
MMF method    2 : 13
Bitmap method 2 : 24
MMF method    3 : 11
Bitmap method 3 : 26
MMF method    4 : 10
Bitmap method 4 : 22
MMF method    5 : 12
Bitmap method 5 : 31
MMF method    6 : 10
Bitmap method 6 : 29
MMF method    7 : 14
Bitmap method 7 : 31
MMF method    8 : 13
Bitmap method 8 : 26
MMF method    9 : 16
Bitmap method 9 : 30
-------------------------------------------
MMF method    [mean] 14 msec [max] 28 msec [min] 10 msec
Bitmap method [mean] 33.3 msec [max] 88 mesec [min] 22 msec
```