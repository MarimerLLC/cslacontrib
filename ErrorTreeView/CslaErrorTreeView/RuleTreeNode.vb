Imports Csla.Validation
Public Class RuleTreeNode
    Inherits TreeNode
    Public Sub New(ByVal rule As BrokenRule)
        MyBase.New(rule.Description)
        mPropertyName = rule.Property

        Select Case rule.Severity
            Case Csla.Validation.RuleSeverity.Error
                Me.ForeColor = Color.Red
                Me.ImageKey = "Error"
            Case Csla.Validation.RuleSeverity.Warning
                Me.ForeColor = Color.OrangeRed
                Me.ImageKey = "Warning"
            Case Csla.Validation.RuleSeverity.Information
                Me.ForeColor = Color.Black
                Me.ImageKey = "Information"
        End Select
        Me.SelectedImageKey = Me.ImageKey
    End Sub
    Private mPropertyName As String
    Public ReadOnly Property PropertyName() As String
        Get
            Return mPropertyName
        End Get
    End Property
End Class