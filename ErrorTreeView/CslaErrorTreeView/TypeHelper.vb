''' <summary>
''' Lleva a cabo validaciones de tipo.
''' </summary>
''' <remarks></remarks>
Public Module TypeHelper
    Private tBusinessBase As Type = GetType(Csla.BusinessBase(Of ))
    Private tBusinessListBase As Type = GetType(Csla.BusinessListBase(Of ,))

    ''' <summary>
    ''' Verifica que el tipo herede de Csla.BusinessBase(Of ).
    ''' </summary>
    ''' <param name="t"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsBusinessBase(ByVal t As Type) As Boolean
        If t.IsValueType Then Return False
        Return IsSubclassOf(t, tBusinessBase)
    End Function

    ''' <summary>
    ''' Verifica que el tipo herede de Csla.BusinessListBase(Of ).
    ''' </summary>
    ''' <param name="t"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsBusinessListBase(ByVal t As Type) As Boolean
        If t.IsValueType Then Return False
        Return IsSubclassOf(t, tBusinessListBase)
    End Function

    ''' <summary>
    ''' Verifica la cadena de herencia para determinar si un tipo t hereda de un tipo bt.
    ''' </summary>
    ''' <param name="t">El tipo que se desea verificar.</param>
    ''' <param name="bt">El tipo de la clase base.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsSubclassOf(ByVal t As Type, ByVal bt As Type) As Boolean
        If t Is Nothing Then Return False
        If bt.IsGenericTypeDefinition AndAlso t.IsGenericType Then
            t = t.GetGenericTypeDefinition
        End If
        If t.Equals(bt) Then
            Return True
        Else
            Return IsSubclassOf(t.BaseType, bt)
        End If
    End Function

End Module
