Imports Microsoft.VisualBasic
Imports System
Imports System.Data

Namespace CslaSrd.Data
  ''' <summary>
  ''' This is an extension of the CSLA Framework SafeDataReader that provides
  ''' support for additional "Smart" datatypes.
  ''' </summary>
  Public Class SmartSafeDataReader
	  Inherits Csla.Data.SafeDataReader
	  Implements IDataReader
	''' <summary>
	''' Initializes the SrdSafeDataReader object to use data from
	''' the provided DataReader object.
	''' </summary>
	''' <param name="dataReader">The source DataReader object containing the data.</param>
	Public Sub New(ByVal dataReader As IDataReader)
		MyBase.New(dataReader)
	End Sub


	#Region "SmartBool"
	''' <summary>
	''' Gets a <see cref="SmartBool" /> from the datareader.
	''' </summary>
	''' <remarks>
	''' A null is converted into min possible value.
	''' See Chapter 5 for more details on the SmartBool class.
	''' </remarks>
	''' <param name="name">Name of the column containing the value.</param>
	Public Function GetSmartBool(ByVal name As String) As CslaSrd.SmartBool
		Return GetSmartBool(MyBase.DataReader.GetOrdinal(name), True)
	End Function

	''' <summary>
	''' Gets a <see cref="SmartBool" /> from the datareader.
	''' </summary>
	''' <remarks>
	''' A null is converted into the min possible value.
	''' See Chapter 5 for more details on the SmartBool class.
	''' </remarks>
	''' <param name="i">Ordinal column position of the value.</param>
	Public Overridable Function GetSmartBool(ByVal i As Integer) As CslaSrd.SmartBool
		Return GetSmartBool(i, True)
	End Function

	''' <summary>
	''' Gets a <see cref="SmartBool" /> from the datareader.
	''' </summary>
	''' <remarks>
	''' A null is converted into either the min or max possible value
	''' depending on the MinIsEmpty parameter. See Chapter 5 for more
	''' details on the SmartBool class.
	''' </remarks>
	''' <param name="name">Name of the column containing the value.</param>
	''' <param name="minIsEmpty">
	''' A flag indicating whether the min or max 
	''' value of a data means an empty item.</param>
	Public Function GetSmartBool(ByVal name As String, ByVal minIsEmpty As Boolean) As CslaSrd.SmartBool
		Return GetSmartBool(MyBase.DataReader.GetOrdinal(name), minIsEmpty)
	End Function

	''' <summary>
	''' Gets a <see cref="SmartBool"/> from the datareader.
	''' </summary>
	''' <param name="i">Ordinal column position of the value.</param>
	''' <param name="minIsEmpty">
	''' A flag indicating whether the min or max 
	''' value of a data means an empty item.</param>
	Public Overridable Function GetSmartBool(ByVal i As Integer, ByVal minIsEmpty As Boolean) As CslaSrd.SmartBool
		If MyBase.DataReader.IsDBNull(i) Then
			Return New CslaSrd.SmartBool(minIsEmpty)
		Else
			Return New CslaSrd.SmartBool(MyBase.DataReader.GetBoolean(i), minIsEmpty)
		End If
	End Function
	#End Region ' SmartBool


	#Region "SmartInt16"
	''' <summary>
	''' Gets a <see cref="SmartInt16" /> from the datareader.
	''' </summary>
	''' <remarks>
	''' A null is converted into min possible int
	''' See Chapter 5 for more details on the SmartInt16 class.
	''' </remarks>
	''' <param name="name">Name of the column containing the value.</param>
	Public Function GetSmartInt16(ByVal name As String) As CslaSrd.SmartInt16
		Return GetSmartInt16(MyBase.DataReader.GetOrdinal(name), True)
	End Function

	''' <summary>
	''' Gets a <see cref="SmartInt16" /> from the datareader.
	''' </summary>
	''' <remarks>
	''' A null is converted into the min possible int
	''' See Chapter 5 for more details on the SmartInt16 class.
	''' </remarks>
	''' <param name="i">Ordinal column position of the value.</param>
	Public Overridable Function GetSmartInt16(ByVal i As Integer) As CslaSrd.SmartInt16
		Return GetSmartInt16(i, True)
	End Function

	''' <summary>
	''' Gets a <see cref="SmartInt16" /> from the datareader.
	''' </summary>
	''' <remarks>
	''' A null is converted into either the min or max possible value
	''' depending on the MinIsEmpty parameter. See Chapter 5 for more
	''' details on the SmartInt16 class.
	''' </remarks>
	''' <param name="name">Name of the column containing the value.</param>
	''' <param name="minIsEmpty">
	''' A flag indicating whether the min or max 
	''' value of a data means an empty value.</param>
	Public Function GetSmartInt16(ByVal name As String, ByVal minIsEmpty As Boolean) As CslaSrd.SmartInt16
		Return GetSmartInt16(MyBase.DataReader.GetOrdinal(name), minIsEmpty)
	End Function

	''' <summary>
	''' Gets a <see cref="SmartInt16"/> from the datareader.
	''' </summary>
	''' <param name="i">Ordinal column position of the value.</param>
	''' <param name="minIsEmpty">
	''' A flag indicating whether the min or max 
	''' value of a data means an empty value.</param>
	Public Overridable Function GetSmartInt16(ByVal i As Integer, ByVal minIsEmpty As Boolean) As CslaSrd.SmartInt16
		If MyBase.DataReader.IsDBNull(i) Then
			Return New CslaSrd.SmartInt16(minIsEmpty)
		Else
			Return New CslaSrd.SmartInt16(MyBase.DataReader.GetInt16(i), minIsEmpty)
		End If
	End Function
	#End Region ' SmartInt16

	#Region "SmartInt32"
	''' <summary>
	''' Gets a <see cref="SmartInt32" /> from the datareader.
	''' </summary>
	''' <remarks>
	''' A null is converted into min possible int
	''' See Chapter 5 for more details on the SmartInt32 class.
	''' </remarks>
	''' <param name="name">Name of the column containing the value.</param>
	Public Function GetSmartInt32(ByVal name As String) As CslaSrd.SmartInt32
		Return GetSmartInt32(MyBase.DataReader.GetOrdinal(name), True)
	End Function

	''' <summary>
	''' Gets a <see cref="SmartInt32" /> from the datareader.
	''' </summary>
	''' <remarks>
	''' A null is converted into the min possible int
	''' See Chapter 5 for more details on the SmartInt32 class.
	''' </remarks>
	''' <param name="i">Ordinal column position of the value.</param>
	Public Overridable Function GetSmartInt32(ByVal i As Integer) As CslaSrd.SmartInt32
	  Return GetSmartInt32(i, True)
	End Function

	''' <summary>
	''' Gets a <see cref="SmartInt32" /> from the datareader.
	''' </summary>
	''' <remarks>
	''' A null is converted into either the min or max possible value
	''' depending on the MinIsEmpty parameter. See Chapter 5 for more
	''' details on the SmartInt32 class.
	''' </remarks>
	''' <param name="name">Name of the column containing the value.</param>
	''' <param name="minIsEmpty">
	''' A flag indicating whether the min or max 
	''' value of a data means an empty value.</param>
	Public Function GetSmartInt32(ByVal name As String, ByVal minIsEmpty As Boolean) As CslaSrd.SmartInt32
		Return GetSmartInt32(MyBase.DataReader.GetOrdinal(name), minIsEmpty)
	End Function

	''' <summary>
	''' Gets a <see cref="SmartInt32"/> from the datareader.
	''' </summary>
	''' <param name="i">Ordinal column position of the value.</param>
	''' <param name="minIsEmpty">
	''' A flag indicating whether the min or max 
	''' value of a data means an empty value.</param>
	Public Overridable Function GetSmartInt32(ByVal i As Integer, ByVal minIsEmpty As Boolean) As CslaSrd.SmartInt32
		If MyBase.DataReader.IsDBNull(i) Then
		Return New CslaSrd.SmartInt32(minIsEmpty)
	  Else
		Return New CslaSrd.SmartInt32(MyBase.DataReader.GetInt32(i), minIsEmpty)
	  End If
	End Function
	#End Region ' SmartInt32

	#Region "SmartInt64"
	''' <summary>
	''' Gets a <see cref="SmartInt64" /> from the datareader.
	''' </summary>
	''' <remarks>
	''' A null is converted into min possible int
	''' See Chapter 5 for more details on the SmartInt64 class.
	''' </remarks>
	''' <param name="name">Name of the column containing the value.</param>
	Public Function GetSmartInt64(ByVal name As String) As CslaSrd.SmartInt64
		Return GetSmartInt64(MyBase.DataReader.GetOrdinal(name), True)
	End Function

	''' <summary>
	''' Gets a <see cref="SmartInt64" /> from the datareader.
	''' </summary>
	''' <remarks>
	''' A null is converted into the min possible int
	''' See Chapter 5 for more details on the SmartInt64 class.
	''' </remarks>
	''' <param name="i">Ordinal column position of the value.</param>
	Public Overridable Function GetSmartInt64(ByVal i As Integer) As CslaSrd.SmartInt64
		Return GetSmartInt64(i, True)
	End Function

	''' <summary>
	''' Gets a <see cref="SmartInt64" /> from the datareader.
	''' </summary>
	''' <remarks>
	''' A null is converted into either the min or max possible value
	''' depending on the MinIsEmpty parameter. See Chapter 5 for more
	''' details on the SmartInt64 class.
	''' </remarks>
	''' <param name="name">Name of the column containing the value.</param>
	''' <param name="minIsEmpty">
	''' A flag indicating whether the min or max 
	''' value of a data means an empty value.</param>
	Public Function GetSmartInt64(ByVal name As String, ByVal minIsEmpty As Boolean) As CslaSrd.SmartInt64
		Return GetSmartInt64(MyBase.DataReader.GetOrdinal(name), minIsEmpty)
	End Function

	''' <summary>
	''' Gets a <see cref="SmartInt64"/> from the datareader.
	''' </summary>
	''' <param name="i">Ordinal column position of the value.</param>
	''' <param name="minIsEmpty">
	''' A flag indicating whether the min or max 
	''' value of a data means an empty value.</param>
	Public Overridable Function GetSmartInt64(ByVal i As Integer, ByVal minIsEmpty As Boolean) As CslaSrd.SmartInt64
		If MyBase.DataReader.IsDBNull(i) Then
			Return New CslaSrd.SmartInt64(minIsEmpty)
		Else
			Return New CslaSrd.SmartInt64(MyBase.DataReader.GetInt64(i), minIsEmpty)
		End If
	End Function
	#End Region ' SmartInt64

	#Region "SmartFloat"
	''' <summary>
	''' Gets a <see cref="SmartFloat" /> from the datareader.
	''' </summary>
	''' <remarks>
	''' A null is converted into min possible value.
	''' See Chapter 5 for more details on the SmartFloat class.
	''' </remarks>
	''' <param name="name">Name of the column containing the value.</param>
	Public Function GetSmartFloat(ByVal name As String) As CslaSrd.SmartFloat
		Return GetSmartFloat(MyBase.DataReader.GetOrdinal(name), True)
	End Function

	''' <summary>
	''' Gets a <see cref="SmartFloat" /> from the datareader.
	''' </summary>
	''' <remarks>
	''' A null is converted into the min possible value.
	''' See Chapter 5 for more details on the SmartFloat class.
	''' </remarks>
	''' <param name="i">Ordinal column position of the value.</param>
	Public Overridable Function GetSmartFloat(ByVal i As Integer) As CslaSrd.SmartFloat
		Return GetSmartFloat(i, True)
	End Function

	''' <summary>
	''' Gets a <see cref="SmartFloat" /> from the datareader.
	''' </summary>
	''' <remarks>
	''' A null is converted into either the min or max possible value
	''' depending on the MinIsEmpty parameter. See Chapter 5 for more
	''' details on the SmartFloat class.
	''' </remarks>
	''' <param name="name">Name of the column containing the value.</param>
	''' <param name="minIsEmpty">
	''' A flag indicating whether the min or max 
	''' value of a data means an empty item.</param>
	Public Function GetSmartFloat(ByVal name As String, ByVal minIsEmpty As Boolean) As CslaSrd.SmartFloat
		Return GetSmartFloat(MyBase.DataReader.GetOrdinal(name), minIsEmpty)
	End Function

	''' <summary>
	''' Gets a <see cref="SmartFloat"/> from the datareader.
	''' </summary>
	''' <param name="i">Ordinal column position of the value.</param>
	''' <param name="minIsEmpty">
	''' A flag indicating whether the min or max 
	''' value of a data means an empty item.</param>
	Public Overridable Function GetSmartFloat(ByVal i As Integer, ByVal minIsEmpty As Boolean) As CslaSrd.SmartFloat
		If MyBase.DataReader.IsDBNull(i) Then
			Return New CslaSrd.SmartFloat(minIsEmpty)
		Else
			Return New CslaSrd.SmartFloat(MyBase.DataReader.GetFloat(i), minIsEmpty)
		End If
	End Function
	#End Region ' SmartFloat


  End Class
End Namespace
