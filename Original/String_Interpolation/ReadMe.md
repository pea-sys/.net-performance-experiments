# プリミティブ型の ToString() vs Object.ToString()

プリミティブ型に用意されている ToString を使うべし。

■Boxing

```
 string a = $"{Math.PI}";
```

■NotBoxing

```
string a = $"{Math.PI.ToString()}";
```

| Method    |     Mean |     Error |    StdDev |      Min |      Max |    Gen 0 | Allocated |
| --------- | -------: | --------: | --------: | -------: | -------: | -------: | --------: |
| Boxing    | 6.239 ms |  1.880 ms | 0.1030 ms | 6.136 ms | 6.342 ms | 710.9375 |  1,097 KB |
| NotBoxing | 3.885 ms | 15.140 ms | 0.8299 ms | 2.985 ms | 4.621 ms | 304.6875 |    469 KB |
