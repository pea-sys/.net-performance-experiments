``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1645 (21H1/May2021Update)
Intel Core i3-4000M CPU 2.40GHz (Haswell), 1 CPU, 4 logical and 2 physical cores
  [Host]     : .NET Framework 4.8 (4.8.4470.0), X86 LegacyJIT
  DefaultJob : .NET Framework 4.8 (4.8.4470.0), X86 LegacyJIT


```
|          Method |         Mean |      Error |     StdDev |          Min |          Max |      Gen 0 | Allocated |
|---------------- |-------------:|-----------:|-----------:|-------------:|-------------:|-----------:|----------:|
|          String | 12,418.88 μs | 211.063 μs | 322.315 μs | 12,156.78 μs | 13,361.82 μs | 63468.7500 | 97,939 KB |
|   StringBuilder |     63.17 μs |   0.362 μs |   0.320 μs |     62.77 μs |     63.86 μs |    20.7520 |     32 KB |
| StringBuilderEx |    667.85 μs |   7.407 μs |   6.566 μs |    660.16 μs |    684.37 μs |   134.7656 |    208 KB |
