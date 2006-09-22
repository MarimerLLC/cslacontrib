Public Class ChildCollection
    Inherits Csla.BusinessListBase(Of ChildCollection, Child)

    Protected Overrides Function AddNewCore() As Object
        Dim obj As Child = Child.NewChild(String.Empty)
        Me.Add(obj)
        Return obj
    End Function
    Public Sub New()
        AllowNew = True
        AllowEdit = True
        AllowRemove = True
    End Sub
    Public Overrides Function ToString() As String
        Return "Children"
    End Function
End Class
