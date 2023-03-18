# 三項演算子 vs null 条件演算子

大きな差はありませんでした。
null 条件演算子の方が記述が短いし、スレッドセーフらしいので null 条件演算子を使っていきたい。

■ 三項演算子

```cs
result = i.HasValue ? i.ToString() : null;
```

■ null 条件演算子

```cs
 result = i?.ToString();
```

| Method                  |     Mean |   Error |   StdDev |   Median |
| ----------------------- | -------: | ------: | -------: | -------: |
| NullConditionalOperator | 167.3 ms | 3.65 ms | 10.64 ms | 164.1 ms |
| TernaryOperator         | 167.4 ms | 3.30 ms |  6.66 ms | 166.9 ms |
