Public Class ErrorTreeView
    Inherits TreeView

#Region " State fields "

    Private mErrors As ErrorObject
    Private mainNode As ErrorTreeNode
    Public Event ErrorNodeSelected(ByVal sender As Object, ByVal e As ErrorSelectedEventArgs)
    Private imgList As New ImageList
    Private mDisposed As Boolean = False

#End Region

#Region " Properties "

    Public Property Errors() As ErrorObject
        Get
            Return mErrors
        End Get
        Set(ByVal value As ErrorObject)
            If mainNode IsNot Nothing Then
                Me.Nodes.Remove(mainNode)
                mainNode.Close()
                mainNode = Nothing
            End If
            mErrors = value
            If value IsNot Nothing Then
                mainNode = New ErrorTreeNode(mErrors)
                Me.Nodes.Add(mainNode)
            End If
        End Set
    End Property

#End Region

#Region " IDisposable "

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not mDisposed Then
                If mainNode IsNot Nothing Then
                    mainNode.Close()
                End If
                imgList.Dispose()
                mDisposed = True
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

#End Region

#Region " Constructors "

    Public Sub New()

        ' Add any initialization after the InitializeComponent() call.
        Me.imgList.Images.Add("Information", My.Resources.Information)
        Me.imgList.Images.Add("Warning", My.Resources.warning)
        Me.imgList.Images.Add("Error", My.Resources.ErrorProvider)
        Me.imgList.Images.Add("Tablas", My.Resources.Tables)
        Me.imgList.Images.Add("TablasRelacionadas", My.Resources.RelatedTables)
        Me.ImageList = imgList
    End Sub

#End Region

#Region " Event Handlers "

    Private Sub TreeView1_NodeMouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles Me.NodeMouseDoubleClick
        If TypeOf e.Node Is RuleTreeNode Then
            Dim pName As String = DirectCast(e.Node, RuleTreeNode).PropertyName
            Dim parent As ErrorTreeNode = DirectCast(e.Node.Parent, ErrorTreeNode)
            RaiseEvent ErrorNodeSelected(Me, New ErrorSelectedEventArgs(parent.Instance, pName))
        ElseIf TypeOf e.Node Is ErrorTreeNode Then
            Dim parent As ErrorTreeNode = DirectCast(e.Node, ErrorTreeNode)
            RaiseEvent ErrorNodeSelected(Me, New ErrorSelectedEventArgs(parent.Instance))
        End If
    End Sub

#End Region

End Class
