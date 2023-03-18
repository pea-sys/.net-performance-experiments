# String vs StringBuilder

10000 回の文字列連結処理比較結果  
StringBuilder の圧勝でした

| Method          |         Mean |      Error |       StdDev |          Min |          Max |      Gen 0 | Allocated |
| --------------- | -----------: | ---------: | -----------: | -----------: | -----------: | ---------: | --------: |
| String          | 14,679.68 us | 717.252 us | 2,069.436 us | 12,492.27 us | 20,651.70 us | 63468.7500 | 97,939 KB |
| StringBuilder   |     68.33 us |   0.744 us |     0.621 us |     67.35 us |     69.62 us |    33.3252 |     52 KB |
| StringBuilderEx |    689.92 us |  11.020 us |     9.202 us |    679.05 us |    709.62 us |   147.4609 |    228 KB |
