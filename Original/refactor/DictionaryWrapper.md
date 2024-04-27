# Dictionary をラップする

リファクタリングの一環してよく使用する形式の辞書型をラップすることがある

ローカルで以下のような現金金種をキーとして辞書を複数使用しているとする

```
Dim _dictionary As New Dictionary(Of Integer, Decimal) From {(1,0), (5,0), (10,0), (50,0), (100,0), (500,0), (1000,0), (2000,0), (5000,0),(10000,0)}
```

そんな時は辞書型をラップしてやると見通しの良いコードになる

```vb.net
Public Class MoneyDictionary
    Private ReadOnly _dictionary As New Dictionary(Of Integer, Decimal) From {(1,0), (5,0), (10,0), (50,0), (100,0), (500,0), (1000,0), (2000,0), (5000,0),(10000,0)}

    ' 通貨を追加するメソッド（制約によりキーの追加不可）
    Public Sub AddCurrency(denomination As Integer, amount As Decimal)
        If _dictionary.ContainsKey(denomination) Then
            _dictionary(denomination) += amount
        End If
    End Sub

    ' 通貨の金額を取得するメソッド
    Public Function GetAmount(denomination As Integer) As Decimal
        If _dictionary.ContainsKey(denomination) Then
            Return _dictionary(denomination)
        Else
            Return 0
        End If
    End Function

    ' 通貨の金額を設定するメソッド
    Public Sub SetAmount(denomination As Integer, amount As Decimal)
        If _dictionary.ContainsKey(denomination) Then
            _dictionary(denomination) = amount
        End If
    End Sub

    ' 通貨の一覧を取得するメソッド
    Public Function GetDenominations() As List(Of Integer)
        Return _dictionary.Keys.ToList()
    End Function
End Class

```

```
Dim _dictionary As New MoneyDictionary
```
