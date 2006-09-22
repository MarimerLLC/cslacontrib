Public Class Root
    Inherits Csla.BusinessBase(Of Root)

    Private mId As Guid = Guid.NewGuid()
    Private mName As String = String.Empty
    Private mSomeValue As Integer = 0
    Private mChildren As New ChildCollection

    Public ReadOnly Property Children() As ChildCollection
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
        ValidationRules.AddRule(Of Root, Csla.Validation.RuleArgs)( _
              AddressOf ValidateValue, New Csla.Validation.RuleArgs("SomeValue"))
    End Sub

    Private Function ValidateValue(ByVal target As Root, ByVal e As Csla.Validation.RuleArgs) As Boolean
        If target.SomeValue = 0 Then
            e.Description = "The value can't be zero!"
            Return False
        End If
        Return True
    End Function

    Public Shared Function NewRoot() As Root
        Dim chld As New Root
        Dim gc As Child
        gc = Child.NewChild("Child 1")
        chld.Children.Add(gc)
        gc = Child.NewChild(String.Empty)
        chld.Children.Add(gc)
        gc = Child.NewChild("Child 3")
        chld.Children.Add(gc)
        chld.ValidationRules.CheckRules()
        Return chld
    End Function

    Public Overrides Function ToString() As String
        Return "Root: " & mName
    End Function

End Class
