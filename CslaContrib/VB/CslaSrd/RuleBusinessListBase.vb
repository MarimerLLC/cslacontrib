Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Csla
Imports Csla.Core

Namespace CslaSrd
	<Serializable()> _
	Public MustInherit Class RuleBusinessListBase(Of T As RuleBusinessListBase(Of T, C), C As {RuleBusinessBase(Of C), IEditableBusinessObject})
		Inherits Csla.BusinessListBase(Of T, C)

		''' <summary>
		''' Provides a public method to tell whether any of the items in the collection has any broken validation rules.
		''' </summary>
		Public ReadOnly Property IsBroken() As Boolean
			Get
				Dim foundBroken As Boolean = False
				Dim i As Integer = 0
				Do While i < Me.Count
					Dim item As RuleBusinessBase(Of C) = Items(i)
					If item.BrokenRules.Count > 0 Then
						foundBroken = True
					End If
					i += 1
				Loop
				Return foundBroken
			End Get
		End Property
	End Class


End Namespace
