Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Text
Imports Csla
Imports Csla.Validation
Imports CslaSrd
Imports CslaSrd.Validation

Namespace CslaSrd
	<Serializable()> _
	Public MustInherit Class RuleBusinessBase(Of T As {RuleBusinessBase(Of T), Csla.Core.IEditableBusinessObject})
		Inherits BusinessBase(Of T)
		''' <summary>
		''' Provides a collection of all validation rules on the object that are broken, i.e., in an invalid state.
		''' </summary>
		Public ReadOnly Property BrokenRules() As BrokenRulesCollection
			Get
				Return BrokenRulesCollection
			End Get
		End Property

		''' <summary>
		''' Provides a collection of all validation rules on the object. 
		''' </summary>
		Public ReadOnly Property Rules() As PublicRuleInfoList
			Get
				Return PublicRuleInfoList.GetList(MyBase.ValidationRules.GetRuleDescriptions())
			End Get
		End Property
	End Class

End Namespace
