Public Class Form1
    Dim obj As Root
    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        obj = Root.NewRoot
        RootBindingSource.DataSource = obj
        ErrorTreeView1.Errors = CslaErrorTreeView.ErrorObject.NewErrorObject(obj)
    End Sub


    Private Sub ErrorTreeView1_ErrorNodeSelected(ByVal sender As Object, ByVal e As CslaErrorTreeView.ErrorSelectedEventArgs) Handles ErrorTreeView1.ErrorNodeSelected
        If TypeOf e.Instance Is Root Then
            If e.PropertyName.Equals(String.Empty) Then
                Me.NameTextBox.Focus()
            Else
                For Each b As Binding In Me.BindingContext.Item(RootBindingSource).Bindings
                    If b.BindingMemberInfo.BindingField = e.PropertyName Then
                        b.Control.Focus()
                        Exit For
                    End If
                Next
            End If
        ElseIf TypeOf e.Instance Is Child Then
            Dim idx As Integer
            idx = ChildrenBindingSource.IndexOf(e.Instance)
            If idx <> -1 Then
                ChildrenBindingSource.Position = idx
            End If
        ElseIf TypeOf e.Instance Is GrandChild Then
            For Each c As Child In obj.Children
                If c.GrandChildren.Contains(DirectCast(e.Instance, GrandChild)) Then
                    ChildrenBindingSource.Position = ChildrenBindingSource.IndexOf(c)
                    Dim idx As Integer
                    idx = GrandChildrenBindingSource.IndexOf(e.Instance)
                    If idx <> -1 Then
                        GrandChildrenBindingSource.Position = idx
                        Exit For
                    End If
                End If
            Next

        End If
    End Sub

    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
        MyBase.OnClosing(e)
        ErrorTreeView1.Errors = Nothing
    End Sub
End Class
