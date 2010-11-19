Public Class GrandChild
    Inherits Csla.BusinessBase(Of GrandChild)

    Private mId As Guid = Guid.NewGuid()
    Private mName As String = String.Empty
    Private mSomeValue As Integer = 0

    Public ReadOnly Property Id() As Guid
        Get
            Return mId
        End Get
    End Property
    Public Property Name() As String
        Get
            CanReadProperty("Name", True)
            Return mName
        End Get
        Set(ByVal value As String)
            CanWriteProperty("Name", True)
            If Not value.Equals(mName) Then
                mName = value
                PropertyHasChanged("Name")
            End If
        End Set
    End Property

    Public Property SomeValue() As Integer
        Get
            CanReadProperty("SomeValue", True)
            Return mSomeValue
        End Get
        Set(ByVal value As Integer)
            CanWriteProperty("SomeValue", True)
            If Not value.Equals(mSomeValue) Then
                mSomeValue = value
                PropertyHasChanged("SomeValue")
            End If
        End Set
    End Property

    Protected Overrides Function GetIdValue() As Object
        Return mId
    End Function

    Protected Overrides Sub AddBusinessRules()
        MyBase.AddBusinessRules()
        ValidationRules.AddRule(AddressOf Csla.Validation.StringRequired, "Name")
        ValidationRules.AddRule(Of GrandChild, Csla.Validation.RuleArgs)( _
         AddressOf ValidateValue, New Csla.Validation.RuleArgs("SomeValue"))
    End Sub

    Private Function ValidateValue(ByVal target As GrandChild, ByVal e As Csla.Validation.RuleArgs) As Boolean
        If target.SomeValue = 0 Then
            e.Severity = Csla.Validation.RuleSeverity.Information
            e.Description = "Be aware that the value is zero"
            Return False
        End If
        Return True
    End Function

    Public Shared Function NewChild() As GrandChild
        Dim chld As New GrandChild
        chld.MarkAsChild()
        chld.ValidationRules.CheckRules()
        Return chld
    End Function

    Public Overrides Function ToString() As String
        Return "GrandChild: " & mName
    End Function

End Class
