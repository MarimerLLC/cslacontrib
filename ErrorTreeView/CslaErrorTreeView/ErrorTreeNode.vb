Imports System.ComponentModel
Imports Csla.Validation

Public Class ErrorTreeNode
    Inherits TreeNode

#Region " Campos privados "

    Private errorNodes As New Dictionary(Of ErrorObject, ErrorTreeNode)
    Private ruleNodes As New Dictionary(Of BrokenRule, RuleTreeNode)
    Private mError As ErrorObject
#End Region

#Region " Properties "

    Public ReadOnly Property Instance() As Object
        Get
            Return mError.Instance
        End Get
    End Property

#End Region

#Region " Constructores "

    Public Sub New(ByVal errorObj As ErrorObject)
        FillNode(Me, errorObj)
        mError = errorObj
        If errorObj.IsList Then
            Me.ImageKey = "TablasRelacionadas"
            Me.SelectedImageKey = "TablasRelacionadas"
        Else
            Me.ImageKey = "Tablas"
            Me.SelectedImageKey = "Tablas"
        End If
        Me.Expand()
    End Sub

    Public Sub Close()
        If mError IsNot Nothing Then
            If mError.IsList Then
                RemoveHandler mError.ErrorObjects.ItemAdded, AddressOf ItemAddedHandler
                RemoveHandler mError.ErrorObjects.ItemRemoved, AddressOf ItemRemovedHandler
            Else
                RemoveHandler mError.BrokenRules.ListChanged, AddressOf RulesChangedHandler
                RemoveHandler mError.RefreshName, AddressOf RefreshName
                RemoveHandler mError.BrokenRules.RemovingItem, AddressOf RuleRemovedHandler
            End If
            For Each item As ErrorObject In mError.ErrorObjects
                RemoveHandler item.VisibilityChanged, AddressOf ErrorVisibilityChangedHandler
            Next
            Dim walker As Dictionary(Of ErrorObject, ErrorTreeNode).Enumerator
            walker = errorNodes.GetEnumerator
            While walker.MoveNext
                Dim nd As ErrorTreeNode = walker.Current.Value
                nd.Close()
            End While
            errorNodes.Clear()
            ruleNodes.Clear()
            Me.Nodes.Clear()
            mError = Nothing
        End If
    End Sub

#End Region

#Region " Métodos Privados "

    Private Sub FillNode(ByVal rootnode As TreeNode, ByVal errObj As ErrorObject)
        rootnode.Text = errObj.Title
        If errObj.IsList Then
            AddHandler errObj.ErrorObjects.ItemAdded, AddressOf ItemAddedHandler
            AddHandler errObj.ErrorObjects.ItemRemoved, AddressOf ItemRemovedHandler
        Else
            AddHandler errObj.RefreshName, AddressOf RefreshName
            AddHandler errObj.BrokenRules.ListChanged, AddressOf RulesChangedHandler
            AddHandler errObj.BrokenRules.RemovingItem, AddressOf RuleRemovedHandler
            For Each br As BrokenRule In errObj.BrokenRules
                AddRule(br)
            Next
        End If
        For Each item As ErrorObject In errObj.ErrorObjects
            AddHandler item.VisibilityChanged, AddressOf ErrorVisibilityChangedHandler
            If item.IsVisible Then
                AddItem(item)
            End If
        Next
    End Sub

    Private Sub RefreshName()
        Me.Text = mError.Title
    End Sub
    Private Sub AddItem(ByVal item As ErrorObject)
        If Not errorNodes.ContainsKey(item) Then
            Dim nd As New ErrorTreeNode(item)
            Me.Nodes.Add(nd)
            If Not Me.IsExpanded Then
                Me.Expand()
            End If
            errorNodes.Add(item, nd)
        End If
    End Sub

    Private Sub RemoveItem(ByVal item As ErrorObject)
        If errorNodes.ContainsKey(item) Then
            Dim nd As ErrorTreeNode
            nd = errorNodes.Item(item)
            errorNodes.Remove(item)
            Me.Nodes.Remove(nd)
            nd.Close()
        End If
    End Sub

#End Region

#Region " Event Handlers "

    Private Sub RulesChangedHandler(ByVal sender As Object, ByVal e As ListChangedEventArgs)
        'TODO: No vincular a RulesChanged desde el treenode.
        Select Case e.ListChangedType
            Case ListChangedType.ItemAdded
                AddRule(DirectCast(sender, BrokenRulesCollection)(e.NewIndex))
                'Case ListChangedType.Reset
                '    ResetRules(DirectCast(sender, BrokenRulesCollection))

        End Select
    End Sub
    Private Sub RuleRemovedHandler(ByVal sender As Object, ByVal e As Csla.Core.RemovingItemEventArgs)
        RemoveRule(DirectCast(e.RemovingItem, BrokenRule))
    End Sub
    Private Sub ErrorVisibilityChangedHandler(ByVal item As ErrorObject, ByVal visible As Boolean)
        If visible Then
            AddItem(item)
        Else
            RemoveItem(item)
        End If
    End Sub

    Private Sub ItemAddedHandler(ByVal Item As ErrorObject)
        AddHandler Item.VisibilityChanged, AddressOf ErrorVisibilityChangedHandler
        If Item.HasBrokenRules Then
            AddItem(Item)
        End If
    End Sub

    Private Sub ItemRemovedHandler(ByVal item As ErrorObject)
        RemoveHandler item.VisibilityChanged, AddressOf ErrorVisibilityChangedHandler
        RemoveItem(item)
    End Sub

#End Region

#Region " Reglas "

    Private Sub AddRule(ByVal rule As BrokenRule)
        Dim nd As New RuleTreeNode(rule)
        Dim count As Integer = Nodes.Count
        'Me aseguro de que los nodos de reglas se inserten antes que los nodos de error
        If count > 0 Then
            For i As Integer = 0 To count - 1
                If TypeOf Nodes(i) Is ErrorTreeNode Then
                    Me.Nodes.Insert(i, nd)
                    Exit For
                End If
            Next
        End If
        If nd.Parent Is Nothing Then
            Me.Nodes.Add(nd)
        End If
        If Not Me.IsExpanded Then
            Me.Expand()
        End If
        ruleNodes.Add(rule, nd)
    End Sub
    Private Sub RemoveRule(ByVal rule As BrokenRule)
        If Me.ruleNodes.ContainsKey(rule) Then
            Dim nd As RuleTreeNode
            nd = ruleNodes.Item(rule)
            Me.Nodes.Remove(nd)
            ruleNodes.Remove(rule)
        End If
    End Sub
    Private Sub ResetRules(ByVal rules As BrokenRulesCollection)
        Dim walker As Dictionary(Of BrokenRule, RuleTreeNode).Enumerator
        walker = ruleNodes.GetEnumerator
        While walker.MoveNext
            Dim nd As RuleTreeNode = walker.Current.Value
            Me.Nodes.Remove(nd)
        End While
        ruleNodes.Clear()
        For Each rule As BrokenRule In rules
            AddRule(rule)
        Next
    End Sub

#End Region



End Class
