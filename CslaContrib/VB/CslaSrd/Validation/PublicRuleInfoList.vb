Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Csla
Imports Csla.Validation
Imports Csla.Core
Imports CslaSrd

Namespace CslaSrd.Validation
	''' <summary>
	''' Used to provide the list of validation rules on the object.
	''' </summary>
	<Serializable()> _
	Public Class PublicRuleInfoList
		Inherits ReadOnlyListBase(Of PublicRuleInfoList, PublicRuleInfo)
		#Region "Authorization Rules"

		''' <summary>
		''' Is the user authorized to see this information?
		''' </summary>
		''' <returns>Whether the user is authorized or not.</returns>
		Public Shared Function CanGetObject() As Boolean
			Return True
		End Function

		#End Region

		''' <summary>
		''' Returns the text of all rule descriptions, each
		''' separated by a <see cref="Environment.NewLine" />.
		''' </summary>
		''' <returns>The text of all rule descriptions.</returns>
		Public Overrides Function ToString() As String
			Dim result As System.Text.StringBuilder = New System.Text.StringBuilder()
			Dim first As Boolean = True
			For Each item As PublicRuleInfo In Me
				If first Then
					first = False
				Else
					result.Append(Environment.NewLine)
				End If
				result.Append(item.RuleDescription)
			Next item
			Return result.ToString()
		End Function


		#Region "Factory Methods"

		''' <summary>
		'''  Given an array of rule data as provided by ValidationRules.GetRuleDescriptions, return a collection of validation rules.
		''' </summary>
		''' <param name="ruleList">An array of rule data as provided by ValidationRules.GetRuleDescriptions.</param>
		''' <returns></returns>
		Public Shared Function GetList(ByVal ruleList As String()) As PublicRuleInfoList
			Dim list As PublicRuleInfoList = New PublicRuleInfoList()
			list.IsReadOnly = False
			list.RaiseListChangedEvents = False
			Dim i As Integer = 0
			Do While i < ruleList.Length
				list.Add(New PublicRuleInfo(ruleList(i)))
				i += 1
			Loop
			list.RaiseListChangedEvents = True
			list.IsReadOnly = True
			Return list
		End Function

		Private Sub New()
			Me.AllowNew = True
		End Sub

		#End Region

	End Class

End Namespace

