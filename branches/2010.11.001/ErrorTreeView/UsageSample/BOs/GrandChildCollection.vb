Public Class GrandChildCollection
    Inherits Csla.BusinessListBase(Of GrandChildCollection, GrandChild)

    Protected Overrides Function AddNewCore() As Object
        Dim obj As GrandChild = GrandChild.NewChild()
        Me.Add(obj)
        Return obj
    End Function
    Public Sub New()
        AllowNew = True
        AllowEdit = True
        AllowRemove = True
    End Sub

    Public Overrides Function ToString() As String
        Return "GrandChildren"
    End Function

End Class
