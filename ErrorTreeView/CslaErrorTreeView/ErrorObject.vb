Imports System.ComponentModel
Imports Csla.Core
Imports System.Reflection
Imports System

''' <summary>
''' ErrorObject representa a un item en una estructura de objetos.
''' Puede representar a un objeto del tipo BusinessBase o BusinessListBase.
''' Se utiliza para tener el listado de reglas de validación rotas para una estructura de objetos compleja.
''' </summary>
''' <remarks></remarks>
Public Class ErrorObject

#Region " Campos privados "
    Private mErrorObjects As ErrorObjectList
    Private mBrokenRules As Csla.Validation.BrokenRulesCollection
    Private mIsList As Boolean = False
    Private mInstance As Object
    Private mIsVisible As Boolean
#End Region

#Region " Propiedades públicas "

    Public ReadOnly Property ErrorObjects() As ErrorObjectList
        Get
            If mErrorObjects Is Nothing Then
                mErrorObjects = New ErrorObjectList(Me)
                AddHandler mErrorObjects.ItemRemoved, AddressOf ChildRemovedHandler
            End If
            Return mErrorObjects
        End Get
    End Property

    Public ReadOnly Property BrokenRules() As Csla.Validation.BrokenRulesCollection
        Get
            Return mBrokenRules
        End Get
    End Property

    Public ReadOnly Property IsList() As Boolean
        Get
            Return mIsList
        End Get
    End Property

    Public ReadOnly Property Title() As String
        Get
            Return mInstance.ToString()
        End Get
    End Property

    Public ReadOnly Property HasBrokenRules() As Boolean
        Get
            If Not mIsList AndAlso BrokenRules.Count > 0 Then
                Return True
            End If
            For Each errObj As ErrorObject In Me.ErrorObjects
                If errObj.HasBrokenRules Then
                    Return True
                End If
            Next
            Return False
        End Get
    End Property

    Public ReadOnly Property IsVisible() As Boolean
        Get
            Return mIsVisible
        End Get
    End Property

    Public ReadOnly Property Instance() As Object
        Get
            Return mInstance
        End Get
    End Property

#End Region

#Region " Constructores "
    Private Sub New()
    End Sub
#End Region

#Region " Eventos "
    Public Event VisibilityChanged(ByVal sender As ErrorObject, ByVal visible As Boolean)
    Public Event RefreshName()
#End Region


#Region " Event Handlers "
    Private Sub boPropChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs)
        RaiseEvent RefreshName()
    End Sub

    Private Sub RulesChangedHandler(ByVal sender As Object, ByVal e As ListChangedEventArgs)
        RulesChanged()
    End Sub

    Private Sub OnRemovingItem(ByVal sender As Object, ByVal e As RemovingItemEventArgs)
        For i As Integer = ErrorObjects.Count - 1 To 0 Step -1
            If ErrorObjects(i).mInstance Is e.RemovingItem Then
                ErrorObjects.RemoveAt(i)
            End If
        Next i
    End Sub
    Private Sub OnListChanged(ByVal sender As Object, ByVal e As ListChangedEventArgs)
        If e.ListChangedType = ListChangedType.ItemAdded Then
            Dim obj As ErrorObject
            obj = ErrorObject.NewErrorObject( _
                DirectCast(mInstance, IBindingList)(e.NewIndex))
            Me.ErrorObjects.Add(obj)
        End If

        'Select Case e.ListChangedType
        '    Case ListChangedType.ItemAdded
        '        Dim obj As ErrorObject
        '        obj = ErrorObject.NewErrorObject( _
        '            DirectCast(mInstance, IBindingList)(e.NewIndex))
        '        Me.ErrorObjects.Add(obj)
        '    Case ListChangedType.ItemDeleted
        '        Dim count As Integer = ErrorObjects.Count
        '        If count = 0 Then
        '            Exit Sub
        '        End If
        '        For i As Integer = count - 1 To 0 Step -1
        '            Dim obj As ErrorObject = mErrorObjects(i)
        '            If Not obj.IsList Then
        '                Dim bo As IEditableBusinessObject
        '                bo = DirectCast(obj.mInstance, IEditableBusinessObject)
        '                If bo.IsDeleted Then
        '                    mErrorObjects.RemoveAt(i)
        '                End If
        '            End If
        '        Next
        'End Select
    End Sub

    ''' <summary>
    ''' Se encarga de verificar si el item todavía está visible cuando un item hijo se elimina.
    ''' </summary>
    ''' <param name="item"></param>
    ''' <remarks></remarks>
    Private Sub ChildRemovedHandler(ByVal item As ErrorObject)
        RulesChanged()
    End Sub

#End Region

#Region " Métodos Privados "

    Private Sub RulesChanged()
        Dim ShouldDisplay As Boolean = Me.HasBrokenRules
        If mIsVisible <> ShouldDisplay Then
            mIsVisible = ShouldDisplay
            RaiseVisibilityChanged()
        End If
    End Sub

    Private Sub RaiseVisibilityChanged()
        RaiseEvent VisibilityChanged(Me, mIsVisible)
    End Sub

    Friend Sub MarkVisible()
        If mIsVisible = False Then
            mIsVisible = True
            RaiseVisibilityChanged()
        End If
    End Sub

    Friend Sub MarkInvisible()
        If mIsVisible = True Then
            mIsVisible = False
            RaiseVisibilityChanged()
        End If
    End Sub
#End Region
    
#Region " Factory "
    Public Shared Function NewErrorObject(ByVal root As Object) As ErrorObject
        Dim eo As New ErrorObject
        Dim t As Type = root.GetType
        If TypeHelper.IsBusinessListBase(t) Then
            eo.mIsList = True
            Dim list As Csla.Core.IExtendedBindingList = DirectCast(root, Csla.Core.IExtendedBindingList)
            For Each obj As Object In list
                Dim chldErrorObj As ErrorObject = NewErrorObject(obj)
                chldErrorObj.mIsVisible = chldErrorObj.HasBrokenRules
                eo.ErrorObjects.Add(chldErrorObj)
            Next
            If eo.HasBrokenRules Then
                eo.mIsVisible = True
            End If

            AddHandler list.ListChanged, AddressOf eo.OnListChanged
            AddHandler list.RemovingItem, AddressOf eo.OnRemovingItem

        Else
            Dim bo As Csla.Core.BusinessBase = DirectCast(root, Csla.Core.BusinessBase)
            AddHandler bo.PropertyChanged, AddressOf eo.boPropChanged
            eo.mBrokenRules = bo.BrokenRulesCollection

            AddHandler eo.mBrokenRules.ListChanged, AddressOf eo.RulesChangedHandler
            LoadPropertyAccesors(t)
            For Each mi As MethodInfo In PropertyAccesors.Item(t)
                Dim obj As Object
                obj = mi.Invoke(root, Nothing)
                Dim chldErrorObj As ErrorObject = NewErrorObject(obj)
                chldErrorObj.mIsVisible = chldErrorObj.HasBrokenRules
                eo.ErrorObjects.Add(chldErrorObj)
            Next
        End If
        eo.mInstance = root
        Return eo
    End Function


#Region " Field Type accesors handling "

    'Private Shared FieldAccesors As New Dictionary(Of Type, List(Of FieldInfo))
    'Private Shared Sub LoadFieldAccesors(ByVal t As Type)
    '    If FieldAccesors.ContainsKey(t) Then Exit Sub
    '    Dim accesors As New List(Of FieldInfo)
    '    Dim fields() As FieldInfo
    '    Dim field As FieldInfo
    '    Dim currentType As Type = t
    '    Dim tName As String = t.FullName
    '    Do
    '        fields = currentType.GetFields( _
    '                                BindingFlags.NonPublic Or _
    '                                BindingFlags.Instance Or _
    '                                BindingFlags.Public)

    '        For Each field In fields
    '            Dim attr() As Object = field.GetCustomAttributes( _
    '                GetType(NonSerializedAttribute), False)
    '            If attr.Length = 0 Then
    '                If TypeHelper.IsBusinessBase(field.FieldType) OrElse _
    '                    TypeHelper.IsBusinessListBase(field.FieldType) Then
    '                    accesors.Add(field)
    '                End If
    '            End If
    '        Next
    '        currentType = currentType.BaseType
    '    Loop Until currentType Is Nothing
    '    FieldAccesors.Add(t, accesors)
    'End Sub
    Private Shared PropertyAccesors As New Dictionary(Of Type, List(Of MethodInfo))
    Private Shared Sub LoadPropertyAccesors(ByVal t As Type)
        If PropertyAccesors.ContainsKey(t) Then Exit Sub
        Dim accesors As New List(Of MethodInfo)
        Dim props() As PropertyInfo
        Dim prop As PropertyInfo
        Dim currentType As Type = t
        Dim tName As String = t.FullName
        Do
            props = currentType.GetProperties()

            For Each prop In props
                If TypeHelper.IsBusinessBase(prop.PropertyType) OrElse _
                    TypeHelper.IsBusinessListBase(prop.PropertyType) Then
                    Dim mi As MethodInfo
                    mi = prop.GetGetMethod()
                    If mi IsNot Nothing Then
                        accesors.Add(mi)
                    End If
                End If
            Next
            currentType = currentType.BaseType
        Loop Until currentType Is Nothing
        PropertyAccesors.Add(t, accesors)
    End Sub

#End Region

#End Region

#Region " System.Object "
    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        Return mInstance.Equals(DirectCast(obj, ErrorObject).mInstance)
    End Function
    Public Overrides Function ToString() As String
        Return mInstance.ToString
    End Function
    Public Overrides Function GetHashCode() As Integer
        Return mInstance.GetHashCode
    End Function
#End Region

End Class
