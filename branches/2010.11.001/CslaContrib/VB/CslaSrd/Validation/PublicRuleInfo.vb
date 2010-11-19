Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Data.SqlClient
Imports Csla
Imports Csla.Data
Imports Csla.Validation


Namespace CslaSrd.Validation
	''' <summary>
	''' Contains publically available information about a given validation rule for a given object.
	''' </summary>
	<Serializable()> _
	Public Class PublicRuleInfo
		Inherits ReadOnlyBase(Of PublicRuleInfo)

		#Region "Business Methods"

		'TODO: add priority and severity information at some point.
		'private int      _priority        = 0;
		Private _ruleName As String = String.Empty
		Private _ruleDescription As String = String.Empty
		Private _ruleProperty As String = String.Empty
		Private _ruleParamNames As String() = { String.Empty }
		Private _ruleParamValues As String() = { String.Empty }

		''' <summary>
		''' The order in which this rule will be evaluated, relative to other rules on the same object, 
		''' with lower numbers being "sooner".
		''' Technical note: This information is not yet publically available as it is not exposed elsewhere.
		''' </summary>
		'public int Priority
		'{
		'    get { return _priority; }
		'}

		''' <summary>
		''' The name of the rule that has been assigned to this object.
		''' </summary>
		Public ReadOnly Property RuleName() As String
			Get
				Return _ruleName
			End Get
		End Property

		''' <summary>
		''' A brief explanation of the rule, suitable for presentation to a non-technical end-user, that explains how the rule would be broken.
		''' </summary>
		Public ReadOnly Property RuleDescription() As String
			Get
				Return _ruleDescription
			End Get
		End Property
		''' <summary>
		''' The object property that this rule is assigned to.
		''' </summary>
		Public ReadOnly Property RuleProperty() As String
			Get
				Return _ruleProperty
			End Get
		End Property
		''' <summary>
		''' An array of parameters that the rule needs in order to evaluate whether the rule is broken or not.
		''' </summary>
		Public ReadOnly Property RuleParamNames() As String()
			Get
				Return _ruleParamNames
			End Get
		End Property
		''' <summary>
		''' An array of parameter values that the rule needs in order to evaluate whether the rule is broken or not.
		''' </summary>
		Public ReadOnly Property RuleParamValues() As String()
			Get
				Return _ruleParamValues
			End Get
		End Property
		''' <summary>
		''' The name of the rule and the property it is assigned to (delimited by a / character) uniquely identify a given rule.
		''' </summary>
		''' <returns></returns>
		Protected Overrides Function GetIdValue() As Object
			Return _ruleName & "/" & _ruleProperty
		End Function

		Public Overrides Function ToString() As String
			Return _ruleName & "/" & _ruleProperty
		End Function

		#End Region

		#Region "Constructors"

		Private Sub New()
'INSTANT VB NOTE: Embedded comments are not maintained by Instant VB
'ORIGINAL LINE: { /* require use of factory methods */ }
		End Sub

		''' <summary>
		''' Creates a new PublicRuleInfo definition based upon the data supplied.
		''' </summary>
		''' <param name="ruleStuff">The publically known data about the rule, formatted as follows:
		'''     rule://ruleName/propertyName?paramName1=paramValue1&paramName2=paramValue2
		''' </param>
		Friend Sub New(ByVal ruleStuff As String)
			'            rule://StringMaxLength/Name?maxLength=10morestuff. rule://StringRequired/Namemorestuff. 

			' I tried using System.Uri as a way to parse the incoming rule data.
			' Sadly, it corrupts some of the incoming data by changing it to all lowercase.
			' So, this is a less nifty, but more correct parsing of the data.


			' Trim off the initial string "rule://"
			ruleStuff = ruleStuff.Substring(7)

			' Separate the ruleName and propertyName from the parameter names and values.
			Dim tempList As String() = ruleStuff.Split("?"c)
			' Separate the ruleName from the propertyName.
			Dim infoList As String() = tempList(0).Split("/"c)
			_ruleName = infoList(0)
			_ruleProperty = infoList(1)

			' Start producing the end-user description of the rule by getting the standard description text for the rule.
			' All standard description text entries in the resource file start with "rule" so that they sort together.
			' It is important that the resource key is an exact match to the ruleName (prefixed by "rule"), or the 
			' standard description text will not be found.
			_ruleDescription = My.Resources.ResourceManager.GetString("rule" & _ruleName)
			If _ruleDescription Is Nothing OrElse _ruleDescription = String.Empty Then
				' If no standard description is found, or if it is empty, default to the source data for the rule.
				_ruleDescription = "rule://" & ruleStuff
			End If
			' If the standard rule description text has a place-holder for the property name in its text, replace it with the property name here.
			' NICE-TO-HAVE: A way to look up property names in a reference file so that they too are language-specific to the user.
			_ruleDescription = _ruleDescription.Replace("{ruleProperty}", _ruleProperty)

			' Parse thru the parameter name/value pairs and replace any parameter name placeholders in the standard
			' description text with the corresponding parameter value.
			Dim queryPairList As String()

			If tempList.Length > 1 Then
				queryPairList = tempList(1).Split("&"c)
				Dim i As Integer = 0
				Do While i < queryPairList.Length
					Dim temp As String() = queryPairList(i).Split("="c)
					_ruleParamNames(i) = temp(0)
					_ruleParamValues(i) = temp(1)
					_ruleDescription = _ruleDescription.Replace("{" & _ruleParamNames(i) & "}", _ruleParamValues(i))
					i += 1
				Loop
			End If
		End Sub

		#End Region
	End Class
End Namespace

