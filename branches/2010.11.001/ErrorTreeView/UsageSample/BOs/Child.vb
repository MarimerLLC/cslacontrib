Public Class Child
    Inherits Csla.BusinessBase(Of Child)
    Private mId As Guid = Guid.NewGuid()
    Private mName As String = String.Empty
    Private mSomeValue As Integer = 0
    Private mChildren As New GrandChildCollection

    Public ReadOnly Property GrandChildren() As GrandChildCollection
        Get
            Return mChildren
        End Get
    End Property
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
        ValidationRules.AddRule(Of Child, Csla.Validation.RuleArgs)( _
            AddressOf ValidateValue, New Csla.Validation.RuleArgs("SomeValue"))
    End Sub

    Private Function ValidateValue(ByVal target As Child, ByVal e As Csla.Validation.RuleArgs) As Boolean
        If target.SomeValue = 0 Then
            e.Severity = Csla.Validation.RuleSeverity.Warning
            e.Description = "The value shouldn't be zero!"
            Return False
        End If
        Return True
    End Function

    Public Shared Function NewChild(ByVal name As String) As Child
        Dim chld As New Child
        chld.Name = name
        Dim gc As GrandChild
        gc = GrandChild.NewChild()
        gc.Name = chld.Name & " GrandChild 1"
        gc.SomeValue = 2
        chld.GrandChildren.Add(gc)
        gc = GrandChild.NewChild()
        gc.Name = chld.Name & " GrandChild 2"
        chld.GrandChildren.Add(gc)
        gc = GrandChild.NewChild()
        chld.GrandChildren.Add(gc)
        gc.SomeValue = 2
        chld.ValidationRules.CheckRules()
        chld.MarkAsChild()
        Return chld
    End Function

    Public Overrides Function ToString() As String
        Return "Child: " & mName
    End Function
End Class
