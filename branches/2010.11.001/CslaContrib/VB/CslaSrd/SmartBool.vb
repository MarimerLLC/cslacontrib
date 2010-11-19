Imports Microsoft.VisualBasic
Imports System
Imports CslaSrd

Namespace CslaSrd
	''' <summary>
	''' Provides an boolean data type that understands the concept
	''' of an empty value.
	''' </summary>
	''' <remarks>
	''' See Chapter 5 for a full discussion of the need for a similar 
	''' data type and the design choices behind it.  Basically, we are 
	''' using the same approach to handle booleans instead of dates.
	''' 
	''' However, there are a few differences in behavior and interface from SmartDate, SmartInt16, etc.
	''' 
	''' Major Difference One:  More Limited Set of Constructors
	''' 
	'''     SmartDate (etc.) has a constructor that accepts a primitive of the appropriate underlying datatype, and a separate constructor
	'''     that is a boolean that sets whether an empty object is considered to have a maximum or minimum value.  Since the underlying
	'''     primitive is also a boolean, both of these constructors cannot exist.  So, we had to choose.  I chose to keep the ability to 
	'''     define how an empty SmartBool is to be compared.
	''' 
	''' Major Difference Two: IsEmpty does not return based upon Minimum or Maximum value.
	''' 
	'''     SmartDate (etc.) compare the internal value with the minimum or maximum possible value to determine whether to return an empty value.
	'''     Given that a Date or Int have many, many possible values to choose from, this is an acceptable practice.  Given that a bool has
	'''     only two values, it does not work.  An internal indicator, _isInitialized, is used to determine the answer instead.
	''' 
	''' Possible Major Difference Three: FormatString may be useless.
	''' 
	'''     It may be just my ignorance of the format capability for booleans built into the .Net framework, but this functionality
	'''     doesn't appear to be properly supported (in that I did not find any settings that made a difference to the output).
	'''     I left the capability in place in case others know how to use it, in the hope that they will update this code or send me a message
	'''     to let me know. :)
	''' </remarks>
	<Serializable()> _
	Public Structure SmartBool
		Implements IComparable
		Private Const _minValue As Boolean = False
		Private Const _maxValue As Boolean = True

		Private _bool As Boolean
		Private _initialized As Boolean
		Private _emptyIsMax As Boolean
		Private _format As String

		#Region "Constructors"

		''' <summary>
		''' Creates a new, empty SmartBool object.
		''' </summary>
		''' <remarks>
		''' The SmartBool created will have an empty value.  
		''' It will compare as the minimum or maximum possible value as specified.
		''' </remarks>
		''' <param name="emptyIsMin">Whether to compare an empty value as the minimum or maximum value.</param>
		Public Sub New(ByVal emptyIsMin As Boolean)
			_emptyIsMax = Not emptyIsMin
			_format = Nothing
			_initialized = False
			If _emptyIsMax Then
				_bool = _maxValue
			Else
				_bool = _minValue
			End If
		End Sub

		''' <summary>
		''' The SmartBool created will use the value supplied. If the object is later set to 
		''' an empty value, it will compare as the minimum or maximum possible value as specified.
		''' </summary>
		''' <param name="Value">The initial value of the object.</param>
		''' <param name="EmptyIsMin">Indicates whether an empty bool is the min or max bool value.</param>
		Public Sub New(ByVal value As Boolean, ByVal emptyIsMin As Boolean)
			_emptyIsMax = Not emptyIsMin
			_format = Nothing
			_initialized = True
			_bool = value
		End Sub

		''' <summary>
		''' Creates a new SmartBool object.
		''' </summary>
		''' <remarks>
		''' The SmartBool created will use the min possible
		''' bool to represent an empty bool.
		''' </remarks>
		''' <param name="Value">The initial value of the object (as text).</param>
		Public Sub New(ByVal value As String)
			_emptyIsMax = False
			_format = Nothing
			_bool = _minValue
			If Not value Is Nothing Then
				_initialized = True
				Me.Text = value
			Else
				_initialized = False
			End If
		End Sub

		''' <summary>
		''' Creates a new SmartBool object.
		''' </summary>
		''' <param name="Value">The initial value of the object (as text).</param>
		''' <param name="EmptyIsMin">Indicates whether an empty bool is the min or max bool value.</param>
		Public Sub New(ByVal value As String, ByVal emptyIsMin As Boolean)
			_emptyIsMax = Not emptyIsMin
			_format = Nothing
			If _emptyIsMax Then
				_bool = _maxValue
			Else
				_bool = _minValue
			End If
			If Not value Is Nothing Then
				_initialized = True
				Me.Text = value
			Else
				_initialized = False
			End If
		End Sub

		#End Region


		#Region "Text Support"

		''' <summary>
		''' Gets or sets the format string used to format a bool
		''' value when it is returned as text.
		''' </summary>
		''' <remarks>
		''' The format string should follow the requirements for the
		''' .NET <see cref="System.String.Format"/> statement.
		''' </remarks>
		''' <value>A format string.</value>
		Public Property FormatString() As String
			Get
				If _format Is Nothing Then
					_format = "d"
				End If
				Return _format
			End Get
			Set
				_format = Value
			End Set
		End Property

		''' <summary>
		''' Gets or sets the bool value.
		''' </summary>
		''' <remarks>
		''' <para>
		''' This property can be used to set the bool value by passing a
		''' text representation of the bool. Any text bool representation
		''' that can be parsed by the .NET runtime is valid.
		''' </para><para>
		''' When the bool value is retrieved via this property, the text
		''' is formatted by using the format specified by the 
		''' <see cref="FormatString" /> property. The default is the
		''' short bool format (d).
		''' </para>
		''' </remarks>
		Public Property Text() As String
			Get
				If Me.IsEmpty Then
						Return String.Empty
					Else
						Return _bool.ToString()
					End If
			End Get
			Set
				If Value = String.Empty Then
					_initialized = False
					If _emptyIsMax Then
						_bool = _maxValue
					Else
						_bool = _minValue
					End If
				Else
					Dim upperValue As String = Value.ToUpper()
					If upperValue = "TRUE" Then
						_initialized = True
						_bool = True
					ElseIf upperValue = "FALSE" Then
						_initialized = True
						_bool = False
					Else
						Throw New Exception("Invalid value (" & Value & ") supplied.")
					End If
				End If
			End Set
		End Property

		#End Region

		#Region "Bool Support"

		''' <summary>
		''' Gets or sets the bool value.
		''' </summary>
		Public Property Bool() As Boolean
			Get
				If (Not _initialized) Then
					_bool = _minValue
					_initialized = True
				End If
				Return _bool
			End Get
			Set
				_bool = Value
				_initialized = True
			End Set
		End Property

		#End Region

		#Region "System.Object overrides"

		''' <summary>
		''' Returns a text representation of the bool value.
		''' </summary>
		Public Overrides Function ToString() As String
			Return Me.Text
		End Function

		''' <summary>
		''' Compares this object to another <see cref="SmartBool"/>
		''' for equality.
		''' </summary>
		Public Overrides Overloads Function Equals(ByVal obj As Object) As Boolean
			If TypeOf obj Is SmartBool Then
				Dim tmp As SmartBool = CType(obj, SmartBool)
				If Me.IsEmpty AndAlso tmp.IsEmpty Then
					Return True
				Else
					Return Me.Bool.Equals(tmp.Bool)
				End If
			ElseIf TypeOf obj Is Boolean Then
				Return Me.Bool.Equals(CBool(obj))
			ElseIf TypeOf obj Is String Then
				Return (Me.CompareTo(obj.ToString()) = 0)
			Else
				Return False
			End If
		End Function

		''' <summary>
		''' Returns a hash code for this object.
		''' </summary>
		Public Overrides Function GetHashCode() As Integer
			Return Me.Bool.GetHashCode()
		End Function

		#End Region

		#Region "DBValue"

		''' <summary>
		''' Gets a database-friendly version of the bool value.
		''' </summary>
		''' <remarks>
		''' <para>
		''' If the SmartBool contains an empty bool, this returns <see cref="DBNull"/>.
		''' Otherwise the actual bool value is returned as type Bool.
		''' </para><para>
		''' This property is very useful when setting parameter values for
		''' a Command object, since it automatically stores null values into
		''' the database for empty bool values.
		''' </para><para>
		''' When you also use the SafeDataReader and its GetSmartBool method,
		''' you can easily read a null value from the database back into a
		''' SmartBool object so it remains considered as an empty bool value.
		''' </para>
		''' </remarks>
		Public ReadOnly Property DBValue() As Object
			Get
				If Me.IsEmpty Then
					Return DBNull.Value
				Else
					Return Me.Bool
				End If
			End Get
		End Property

		#End Region

		#Region "Empty Bools"

		''' <summary>
		''' Gets a value indicating whether this object contains an empty bool.
		''' </summary>
		Public ReadOnly Property IsEmpty() As Boolean
			Get
				Return Not _initialized
			End Get
		End Property

		''' <summary>
		''' Gets a value indicating whether an empty bool is the 
		''' min or max possible bool value.
		''' </summary>
		''' <remarks>
		''' Whether an empty bool is considered to be the smallest or largest possible
		''' bool is only important for comparison operations. This allows you to
		''' compare an empty bool with a real bool and get a meaningful result.
		''' </remarks>
		Public ReadOnly Property EmptyIsMin() As Boolean
			Get
				Return Not _emptyIsMax
			End Get
		End Property

		#End Region

		#Region "Conversion Functions"

		''' <summary>
		''' Converts a string value into a SmartBool.
		''' </summary>
		''' <param name="value">String containing the bool value.</param>
		''' <returns>A new SmartBool containing the bool value.</returns>
		''' <remarks>
		''' EmptyIsMin will default to <see langword="true"/>.
		''' </remarks>
		Public Shared Function Parse(ByVal value As String) As SmartBool
			Return New SmartBool(value)
		End Function

		''' <summary>
		''' Converts a string value into a SmartBool.
		''' </summary>
		''' <param name="value">String containing the bool value.</param>
		''' <param name="emptyIsMin">Indicates whether an empty bool is the min or max bool value.</param>
		''' <returns>A new SmartBool containing the bool value.</returns>
		Public Shared Function Parse(ByVal value As String, ByVal emptyIsMin As Boolean) As SmartBool
			Return New SmartBool(value, emptyIsMin)
		End Function

		'/// <summary>
		'/// Converts a text bool representation into a Bool value.
		'/// </summary>
		'/// <remarks>
		'/// An empty string is assumed to represent an empty bool. An empty bool
		'/// is returned as the MinValue of the Bool datatype.
		'/// </remarks>
		'/// <param name="Value">The text representation of the bool.</param>
		'/// <returns>A Bool value.</returns>
		'public static bool StringToBool(string value)
		'{
		'    return StringToBool(value, true);
		'}

		'/// <summary>
		'/// Converts a text bool representation into a Bool value.
		'/// </summary>
		'/// <remarks>
		'/// An empty string is assumed to represent an empty bool. An empty bool
		'/// is returned as the MinValue or MaxValue of the Bool datatype depending
		'/// on the EmptyIsMin parameter.
		'/// </remarks>
		'/// <param name="Value">The text representation of the bool.</param>
		'/// <param name="EmptyIsMin">Indicates whether an empty bool is the min or max bool value.</param>
		'/// <returns>A Bool value.</returns>
		'public static bool StringToBool(string value, bool emptyIsMin)
		'{
		'    bool tmp;
		'    if (String.IsNullOrEmpty(value))
		'    {
		'        if (emptyIsMin)
		'            return _minValue;
		'        else
		'            return _maxValue;
		'    }
		'    else if (bool.TryParse(value, out tmp))
		'    {
		'        return tmp;
		'    }
		'    else
		'    {
		'        string lint = value.Trim().ToLower();
		'        throw new ArgumentException(Resources.StringToBoolException);
		'    }
		'}

		'/// <summary>
		'/// Converts a bool value into a text representation.
		'/// </summary>
		'/// <remarks>
		'/// The bool is considered empty if it matches the min value for
		'/// the Bool datatype. If the bool is empty, this
		'/// method returns an empty string. Otherwise it returns the bool
		'/// value formatted based on the FormatString parameter.
		'/// </remarks>
		'/// <param name="Value">The bool value to convert.</param>
		'/// <param name="FormatString">The format string used to format the bool into text.</param>
		'/// <returns>Text representation of the bool value.</returns>
		'public static string BoolToString(bool value, string formatString)
		'{
		'    return BoolToString(value, formatString, true);
		'}

		'/// <summary>
		'/// Converts a bool value into a text representation.
		'/// </summary>
		'/// <remarks>
		'/// Whether the bool value is considered empty is determined by
		'/// the EmptyIsMin parameter value. If the bool is empty, this
		'/// method returns an empty string. Otherwise it returns the bool
		'/// value formatted based on the FormatString parameter.
		'/// </remarks>
		'/// <param name="Value">The bool value to convert.</param>
		'/// <param name="FormatString">The format string used to format the bool into text.</param>
		'/// <param name="EmptyIsMin">Indicates whether an empty bool is the min or max bool value.</param>
		'/// <returns>Text representation of the bool value.</returns>
		'public static string BoolToString(bool value, string formatString, bool emptyIsMin)
		'{
		'    if (emptyIsMin && value == _minValue)
		'        return string.Empty;
		'    else if (!emptyIsMin && value == _maxValue)
		'        return string.Empty;
		'    else
		'        return string.Format("{0:" + formatString + "}", value);
		'}

		#End Region

		#Region "Manipulation Functions"

		''' <summary>
		''' Compares one SmartBool to another.
		''' </summary>
		''' <remarks>
		''' This method works the same as the <see cref="bool.CompareTo"/> method
		''' on the Bool type, with the exception that it
		''' understands the concept of empty bool values.
		''' </remarks>
		''' <param name="Value">The bool to which we are being compared.</param>
		''' <returns>A value indicating if the comparison bool is less than, equal to or greater than this bool.</returns>
		Public Function CompareTo(ByVal value As SmartBool) As Integer
			If Me.IsEmpty AndAlso value.IsEmpty Then
				Return 0
			Else
				Return _bool.CompareTo(value.Bool)
			End If
		End Function

		''' <summary>
		''' Compares one SmartBool to another.
		''' </summary>
		''' <remarks>
		''' This method works the same as the <see cref="bool.CompareTo"/> method
		''' on the Bool type, with the exception that it
		''' understands the concept of empty bool values.
		''' </remarks>
		''' <param name="obj">The bool to which we are being compared.</param>
		''' <returns>A value indicating if the comparison bool is less than, equal to or greater than this bool.</returns>
		Private Function CompareTo(ByVal value As Object) As Integer Implements IComparable.CompareTo
			If TypeOf value Is SmartBool Then
				Return CompareTo(CType(value, SmartBool))
			Else
                Throw New ArgumentException(My.Resources.ValueNotSmartBoolException)
			End If
		End Function

		''' <summary>
		''' Compares a SmartBool to a text bool value.
		''' </summary>
		''' <param name="value">The bool to which we are being compared.</param>
		''' <returns>A value indicating if the comparison bool is less than, equal to or greater than this bool.</returns>
		Public Function CompareTo(ByVal value As String) As Integer
			If value.Length = 0 Then
				If Me.IsEmpty = True Then
					Return 0
				ElseIf Me.EmptyIsMin = True Then
					Return Me.Bool.CompareTo(False)
				Else
					Return Me.Bool.CompareTo(True)
				End If
			Else
				Dim upperValue As String = value.ToUpper()
				If upperValue = "TRUE" Then
					Return Me.Bool.CompareTo(True)
				ElseIf upperValue = "FALSE" Then
					Return Me.Bool.CompareTo(False)
				Else
					Throw New Exception ("Invalid value (" & value & ") supplied.")
				End If
			End If
		End Function

		''' <summary>
		''' Compares a SmartBool to a bool value.
		''' </summary>
		''' <param name="value">The bool to which we are being compared.</param>
		''' <returns>A value indicating if the comparison bool is less than, equal to or greater than this bool.</returns>
		Public Function CompareTo(ByVal value As Boolean) As Integer
			Return Me.Bool.CompareTo(value)
		End Function

		#End Region

		#Region "Operators"

		Public Shared Operator =(ByVal obj1 As SmartBool, ByVal obj2 As SmartBool) As Boolean
			Return obj1.Equals(obj2)
		End Operator

		Public Shared Operator <>(ByVal obj1 As SmartBool, ByVal obj2 As SmartBool) As Boolean
			Return Not obj1.Equals(obj2)
		End Operator

		Public Shared Operator =(ByVal obj1 As SmartBool, ByVal obj2 As Boolean) As Boolean
			Return obj1.Equals(obj2)
		End Operator

		Public Shared Operator <>(ByVal obj1 As SmartBool, ByVal obj2 As Boolean) As Boolean
			Return Not obj1.Equals(obj2)
		End Operator

		Public Shared Operator =(ByVal obj1 As SmartBool, ByVal obj2 As String) As Boolean
			Return obj1.Equals(obj2)
		End Operator

		Public Shared Operator <>(ByVal obj1 As SmartBool, ByVal obj2 As String) As Boolean
			Return Not obj1.Equals(obj2)
		End Operator

		Public Shared Operator >(ByVal obj1 As SmartBool, ByVal obj2 As SmartBool) As Boolean
			Return obj1.CompareTo(obj2) > 0
		End Operator

		Public Shared Operator <(ByVal obj1 As SmartBool, ByVal obj2 As SmartBool) As Boolean
			Return obj1.CompareTo(obj2) < 0
		End Operator

		Public Shared Operator >(ByVal obj1 As SmartBool, ByVal obj2 As Boolean) As Boolean
			Return obj1.CompareTo(obj2) > 0
		End Operator

		Public Shared Operator <(ByVal obj1 As SmartBool, ByVal obj2 As Boolean) As Boolean
			Return obj1.CompareTo(obj2) < 0
		End Operator

		Public Shared Operator >(ByVal obj1 As SmartBool, ByVal obj2 As String) As Boolean
			Return obj1.CompareTo(obj2) > 0
		End Operator

		Public Shared Operator <(ByVal obj1 As SmartBool, ByVal obj2 As String) As Boolean
			Return obj1.CompareTo(obj2) < 0
		End Operator

		Public Shared Operator >=(ByVal obj1 As SmartBool, ByVal obj2 As SmartBool) As Boolean
			Return obj1.CompareTo(obj2) >= 0
		End Operator

		Public Shared Operator <=(ByVal obj1 As SmartBool, ByVal obj2 As SmartBool) As Boolean
			Return obj1.CompareTo(obj2) <= 0
		End Operator

		Public Shared Operator >=(ByVal obj1 As SmartBool, ByVal obj2 As Boolean) As Boolean
			Return obj1.CompareTo(obj2) >= 0
		End Operator

		Public Shared Operator <=(ByVal obj1 As SmartBool, ByVal obj2 As Boolean) As Boolean
			Return obj1.CompareTo(obj2) <= 0
		End Operator

		Public Shared Operator >=(ByVal obj1 As SmartBool, ByVal obj2 As String) As Boolean
			Return obj1.CompareTo(obj2) >= 0
		End Operator

		Public Shared Operator <=(ByVal obj1 As SmartBool, ByVal obj2 As String) As Boolean
			Return obj1.CompareTo(obj2) <= 0
		End Operator

		#End Region

	End Structure
End Namespace