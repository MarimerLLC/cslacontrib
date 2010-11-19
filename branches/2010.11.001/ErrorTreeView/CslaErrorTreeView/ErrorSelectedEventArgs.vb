
Public Class ErrorSelectedEventArgs
    Inherits EventArgs
    Private mInstance As Object
    Private mPropertyName As String = String.Empty
    Public ReadOnly Property Instance() As Object
        Get
            Return mInstance
        End Get
    End Property
    Public ReadOnly Property PropertyName() As String
        Get
            Return mPropertyName
        End Get
    End Property

    Public Sub New(ByVal instance As Object)
        mInstance = instance
    End Sub
    Public Sub New(ByVal instance As Object, ByVal propertyName As String)
        Me.New(instance)
        mPropertyName = propertyName
    End Sub


End Class
