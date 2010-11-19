Imports System.ComponentModel

''' <summary>
''' Lista de objetos <see cref="ErrorObject" />. Se mantiene actualizada a medida que se agregan o eliminan objetos de un objeto BusinessListBase.
''' </summary>
''' <remarks></remarks>
Public Class ErrorObjectList
    Inherits BindingList(Of ErrorObject)

#Region " Variables locales "

    Private mParent As ErrorObject

#End Region

#Region " Constructores "

    Friend Sub New(ByVal parent As ErrorObject)
        mParent = parent
    End Sub

#End Region

#Region " BindingList Overrides "

    Protected Overrides Sub InsertItem(ByVal index As Integer, ByVal item As ErrorObject)
        MyBase.InsertItem(index, item)
        AddHandler item.VisibilityChanged, AddressOf ChildVisibilityChanged
        RaiseEvent ItemAdded(item)
    End Sub
    
    Protected Overrides Sub RemoveItem(ByVal index As Integer)
        Dim removedItem As ErrorObject = Me(index)
        RemoveHandler Me(index).VisibilityChanged, AddressOf ChildVisibilityChanged
        MyBase.RemoveItem(index)
        RaiseEvent ItemRemoved(removedItem)
    End Sub

#End Region

#Region " Métodos privados "
    Private Sub ChildVisibilityChanged(ByVal item As ErrorObject, ByVal visible As Boolean)
        If visible Then
            mParent.MarkVisible()
        Else
            Dim cnt As Integer
            cnt = Me.Count
            If cnt > 0 Then
                For i As Integer = 0 To cnt - 1
                    If Me(i).IsVisible Then
                        mParent.MarkVisible()
                        Exit Sub
                    End If
                Next
            End If
            mParent.MarkInvisible()
            End If
    End Sub
#End Region

#Region " Eventos "

    Public Event ItemAdded(ByVal item As ErrorObject)
    Public Event ItemRemoved(ByVal item As ErrorObject)

#End Region

End Class
