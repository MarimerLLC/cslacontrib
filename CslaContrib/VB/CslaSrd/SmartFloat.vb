Imports System
Imports CslaSrd


Namespace CslaSrd
  ''' <summary>
  ''' Provides an float data type that understands the concept
  ''' of an empty value.
  ''' </summary>
  ''' <remarks>
  ''' See Chapter 5 for a full discussion of the need for a similar 
  ''' data type and the design choices behind it.  Basically, we are 
  ''' using the same approach to handle floats instead of dates.
  ''' </remarks>
  <Serializable()> _
  Public Structure SmartFloat
	  Implements IComparable
	Private _float As Single
	Private _initialized As Boolean
	Private _emptyIsMax As Boolean
	Private _format As String

	#Region "Constructors"

	''' <summary>
	''' Creates a new SmartFloat object.
	''' </summary>
	''' <param name="emptyIsMin">Indicates whether an empty float is the min or max float value.</param>
	Public Sub New(ByVal emptyIsMin As Boolean)
	  _emptyIsMax = Not emptyIsMin
	  _format = Nothing
	  _initialized = False
	  ' provide a dummy value to allow real initialization
	  _float = Single.MinValue
	  If (Not _emptyIsMax) Then
		Float = Single.MinValue
	  Else
		Float = Single.MaxValue
	  End If
	End Sub

	''' <summary>
	''' Creates a new SmartFloat object.
	''' </summary>
	''' <remarks>
	''' The SmartFloat created will use the min possible
	''' float to represent an empty float.
	''' </remarks>
	''' <param name="value">The initial value of the object.</param>
	Public Sub New(ByVal value As Single)
	  _emptyIsMax = False
	  _format = Nothing
	  _initialized = False
	  _float = Single.MinValue
	  Float = value
	End Sub

	''' <summary>
	''' Creates a new SmartFloat object.
	''' </summary>
	''' <param name="value">The initial value of the object.</param>
	''' <param name="emptyIsMin">Indicates whether an empty float is the min or max float value.</param>
	Public Sub New(ByVal value As Single, ByVal emptyIsMin As Boolean)
	  _emptyIsMax = Not emptyIsMin
	  _format = Nothing
	  _initialized = False
	  _float = Single.MinValue
	  Float = value
	End Sub

	''' <summary>
	''' Creates a new SmartFloat object.
	''' </summary>
	''' <remarks>
	''' The SmartFloat created will use the min possible
	''' float to represent an empty float.
	''' </remarks>
	''' <param name="value">The initial value of the object (as text).</param>
	Public Sub New(ByVal value As String)
	  _emptyIsMax = False
	  _format = Nothing
	  _initialized = True
	  _float = Single.MinValue
	  Me.Text = value
	End Sub

	''' <summary>
	''' Creates a new SmartFloat object.
	''' </summary>
	''' <param name="value">The initial value of the object (as text).</param>
	''' <param name="emptyIsMin">Indicates whether an empty float is the min or max float value.</param>
	Public Sub New(ByVal value As String, ByVal emptyIsMin As Boolean)
	  _emptyIsMax = Not emptyIsMin
	  _format = Nothing
	  _initialized = True
	  _float = Single.MinValue
	  Me.Text = value
	End Sub

	#End Region


	#Region "Text Support"

	''' <summary>
	''' Gets or sets the format string used to format a float
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
		  _format = "g"
		End If
		Return _format
	  End Get
	  Set
		_format = Value
	  End Set
	End Property

	''' <summary>
	''' Gets or sets the float value.
	''' </summary>
	''' <remarks>
	''' <para>
	''' This property can be used to set the float value by passing a
	''' text representation of the float. Any text float representation
	''' that can be parsed by the .NET runtime is valid.
	''' </para><para>
	''' When the float value is retrieved via this property, the text
	''' is formatted by using the format specified by the 
	''' <see cref="FormatString" /> property. The default is the
	''' short float format (d).
	''' </para>
	''' </remarks>
	Public Property Text() As String
	  Get
		  Return FloatToString(Me.Float, FormatString, (Not _emptyIsMax))
	  End Get
	  Set
		  Me.Float = StringToFloat(Value, (Not _emptyIsMax))
	  End Set
	End Property

	#End Region

	#Region "Float Support"

	''' <summary>
	''' Gets or sets the float value.
	''' </summary>
	Public Property Float() As Single
	  Get
		If (Not _initialized) Then
		  _float = Single.MinValue
		  _initialized = True
		End If
		Return _float
	  End Get
	  Set
		_float = Value
		_initialized = True
	  End Set
	End Property

	#End Region

	#Region "System.Object overrides"

	''' <summary>
	''' Returns a text representation of the float value.
	''' </summary>
	Public Overrides Function ToString() As String
	  Return Me.Text
	End Function

	''' <summary>
	''' Compares this object to another <see cref="SmartFloat"/>
	''' for equality.
	''' </summary>
	Public Overrides Overloads Function Equals(ByVal obj As Object) As Boolean
	  If TypeOf obj Is SmartFloat Then
		Dim tmp As SmartFloat = CType(obj, SmartFloat)
		If Me.IsEmpty AndAlso tmp.IsEmpty Then
		  Return True
		Else
		  Return Me.Float.Equals(tmp.Float)
		End If
	  ElseIf TypeOf obj Is Single Then
		Return Me.Float.Equals(CSng(obj))
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
	  Return Me.Float.GetHashCode()
	End Function

	#End Region

	#Region "DBValue"

	''' <summary>
	''' Gets a database-friendly version of the float value.
	''' </summary>
	''' <remarks>
	''' <para>
	''' If the SmartFloat contains an empty float, this returns <see cref="DBNull"/>.
	''' Otherwise the actual float value is returned as type Float.
	''' </para><para>
	''' This property is very useful when setting parameter values for
	''' a Command object, since it automatically stores null values into
	''' the database for empty float values.
	''' </para><para>
	''' When you also use the SafeDataReader and its GetSmartFloat method,
	''' you can easily read a null value from the database back into a
	''' SmartFloat object so it remains considered as an empty float value.
	''' </para>
	''' </remarks>
	Public ReadOnly Property DBValue() As Object
	  Get
		If Me.IsEmpty Then
		  Return DBNull.Value
		Else
		  Return Me.Float
		End If
	  End Get
	End Property

	#End Region

	#Region "Empty Floats"

	''' <summary>
	''' Gets a value indicating whether this object contains an empty float.
	''' </summary>
	Public ReadOnly Property IsEmpty() As Boolean
	  Get
		If (Not _emptyIsMax) Then
		  Return Me.Float.Equals(Single.MinValue)
		Else
		  Return Me.Float.Equals(Single.MaxValue)
		End If
	  End Get
	End Property

	''' <summary>
	''' Gets a value indicating whether an empty float is the 
	''' min or max possible float value.
	''' </summary>
	''' <remarks>
	''' Whether an empty float is considered to be the smallest or largest possible
	''' float is only important for comparison operations. This allows you to
	''' compare an empty float with a real float and get a meaningful result.
	''' </remarks>
	Public ReadOnly Property EmptyIsMin() As Boolean
	  Get
		  Return Not _emptyIsMax
	  End Get
	End Property

	#End Region

	#Region "Conversion Functions"

	''' <summary>
	''' Converts a string value into a SmartFloat.
	''' </summary>
	''' <param name="value">String containing the float value.</param>
	''' <returns>A new SmartFloat containing the float value.</returns>
	''' <remarks>
	''' EmptyIsMin will default to <see langword="true"/>.
	''' </remarks>
	Public Shared Function Parse(ByVal value As String) As SmartFloat
	  Return New SmartFloat(value)
	End Function

	''' <summary>
	''' Converts a string value into a SmartFloat.
	''' </summary>
	''' <param name="value">String containing the float value.</param>
	''' <param name="emptyIsMin">Indicates whether an empty float is the min or max float value.</param>
	''' <returns>A new SmartFloat containing the float value.</returns>
	Public Shared Function Parse(ByVal value As String, ByVal emptyIsMin As Boolean) As SmartFloat
	  Return New SmartFloat(value, emptyIsMin)
	End Function

	''' <summary>
	''' Converts a text float representation into a Float value.
	''' </summary>
	''' <remarks>
	''' An empty string is assumed to represent an empty float. An empty float
	''' is returned as the MinValue of the Float datatype.
	''' </remarks>
	''' <param name="value">The text representation of the float.</param>
	''' <returns>A Float value.</returns>
	Public Shared Function StringToFloat(ByVal value As String) As Single
	  Return StringToFloat(value, True)
	End Function

	''' <summary>
	''' Converts a text float representation into a Float value.
	''' </summary>
	''' <remarks>
	''' An empty string is assumed to represent an empty float. An empty float
	''' is returned as the MinValue or MaxValue of the Float datatype depending
	''' on the EmptyIsMin parameter.
	''' </remarks>
	''' <param name="value">The text representation of the float.</param>
	''' <param name="emptyIsMin">Indicates whether an empty float is the min or max float value.</param>
	''' <returns>A Float value.</returns>
	Public Shared Function StringToFloat(ByVal value As String, ByVal emptyIsMin As Boolean) As Single
	  Dim tmp As Single
	  If String.IsNullOrEmpty(value) Then
		If emptyIsMin Then
		  Return Single.MinValue
		Else
		  Return Single.MaxValue
		End If
	  ElseIf Single.TryParse(value, tmp) Then
		Return tmp
	  Else
		Dim lint As String = value.Trim().ToLower()
                Throw New ArgumentException(My.Resources.StringToFloatException)
            End If
        End Function

        ''' <summary>
        ''' Converts a float value into a text representation.
        ''' </summary>
        ''' <remarks>
        ''' The float is considered empty if it matches the min value for
        ''' the Float datatype. If the float is empty, this
        ''' method returns an empty string. Otherwise it returns the float
        ''' value formatted based on the FormatString parameter.
        ''' </remarks>
        ''' <param name="value">The float value to convert.</param>
        ''' <param name="formatString">The format string used to format the float into text.</param>
        ''' <returns>Text representation of the float value.</returns>
        Public Shared Function FloatToString(ByVal value As Single, ByVal formatString As String) As String
            Return FloatToString(value, formatString, True)
        End Function

        ''' <summary>
        ''' Converts a float value into a text representation.
        ''' </summary>
        ''' <remarks>
        ''' Whether the float value is considered empty is determined by
        ''' the EmptyIsMin parameter value. If the float is empty, this
        ''' method returns an empty string. Otherwise it returns the float
        ''' value formatted based on the FormatString parameter.
        ''' </remarks>
        ''' <param name="value">The float value to convert.</param>
        ''' <param name="formatString">The format string used to format the float into text.</param>
        ''' <param name="emptyIsMin">Indicates whether an empty float is the min or max float value.</param>
        ''' <returns>Text representation of the float value.</returns>
        Public Shared Function FloatToString(ByVal value As Single, ByVal formatString As String, ByVal emptyIsMin As Boolean) As String
            If emptyIsMin AndAlso value = Single.MinValue Then
                Return String.Empty
            ElseIf (Not emptyIsMin) AndAlso value = Single.MaxValue Then
                Return String.Empty
            Else
                Return String.Format("{0:" & formatString & "}", value)
            End If
        End Function

#End Region

#Region "Manipulation Functions"

        ''' <summary>
        ''' Compares one SmartFloat to another.
        ''' </summary>
        ''' <remarks>
        ''' This method works the same as the <see cref="int.CompareTo"/> method
        ''' on the Float inttype, with the exception that it
        ''' understands the concept of empty int values.
        ''' </remarks>
        ''' <param name="value">The int to which we are being compared.</param>
        ''' <returns>A value indicating if the comparison int is less than, equal to or greater than this int.</returns>
        Public Function CompareTo(ByVal value As SmartFloat) As Integer
            If Me.IsEmpty AndAlso value.IsEmpty Then
                Return 0
            Else
                Return _float.CompareTo(value.Float)
            End If
        End Function

        ''' <summary>
        ''' Compares one SmartFloat to another.
        ''' </summary>
        ''' <remarks>
        ''' This method works the same as the <see cref="int.CompareTo"/> method
        ''' on the float type, with the exception that it
        ''' understands the concept of empty float values.
        ''' </remarks>
        ''' <param name="value">The int to which we are being compared.</param>
        ''' <returns>A value indicating if the comparison int is less than, equal to or greater than this int.</returns>
        Private Function CompareTo(ByVal value As Object) As Integer Implements IComparable.CompareTo
            If TypeOf value Is SmartFloat Then
                Return CompareTo(CType(value, SmartFloat))
            Else
                Throw New ArgumentException(My.Resources.ValueNotSmartFloatException)
            End If
        End Function

        ''' <summary>
        ''' Compares a SmartFloat to a text int value.
        ''' </summary>
        ''' <param name="value">The int to which we are being compared.</param>
        ''' <returns>A value indicating if the comparison int is less than, equal to or greater than this int.</returns>
        Public Function CompareTo(ByVal value As String) As Integer
            Return Me.Float.CompareTo(StringToFloat(value, (Not _emptyIsMax)))
        End Function

        ''' <summary>
        ''' Compares a SmartFloat to a int value.
        ''' </summary>
        ''' <param name="value">The int to which we are being compared.</param>
        ''' <returns>A value indicating if the comparison int is less than, equal to or greater than this int.</returns>
        Public Function CompareTo(ByVal value As Single) As Integer
            Return Me.Float.CompareTo(value)
        End Function

        ''' <summary>
        ''' Adds an float value onto the object.
        ''' </summary>
        Public Function Add(ByVal value As Single) As Single
            If IsEmpty Then
                Return Me.Float
            Else
                Return CSng(Me.Float + value)
            End If
        End Function

        ''' <summary>
        ''' Subtracts a float value from the object.
        ''' </summary>
        Public Function Subtract(ByVal value As Single) As Single
            If IsEmpty Then
                Return Me.Float
            Else
                Return CSng(Me.Float - value)
            End If
        End Function

#End Region

	#Region "Operators"
	  ''' <summary>
	  ''' Compares two of this type of object for equality.
	  ''' </summary>
	  ''' <param name="obj1">The first object to compare</param>
	  ''' <param name="obj2">The second object to compare</param>
	  ''' <returns>Whether the object values are equal</returns>
	Public Shared Operator =(ByVal obj1 As SmartFloat, ByVal obj2 As SmartFloat) As Boolean
	  Return obj1.Equals(obj2)
	End Operator
	  ''' <summary>
	  ''' Checks two of this type of object for non-equality.
	  ''' </summary>
	  ''' <param name="obj1">The first object to compare</param>
	  ''' <param name="obj2">The second object to compare</param>
	  ''' <returns>Whether the two values are not equal</returns>
	Public Shared Operator <>(ByVal obj1 As SmartFloat, ByVal obj2 As SmartFloat) As Boolean
	  Return Not obj1.Equals(obj2)
	End Operator
	  ''' <summary>
	  ''' Compares an object of this type with an float for equality.
	  ''' </summary>
	  ''' <param name="obj1">The object of this type to compare</param>
	  ''' <param name="obj2">The float to compare</param>
	  ''' <returns>Whether the two values are equal</returns>
	Public Shared Operator =(ByVal obj1 As SmartFloat, ByVal obj2 As Single) As Boolean
	  Return obj1.Equals(obj2)
	End Operator
	''' <summary>
	''' Compares an object of this type with an float for non-equality.
	''' </summary>
	''' <param name="obj1">The object of this type to compare</param>
	''' <param name="obj2">The float to compare</param>
	''' <returns>Whether the two values are not equal</returns>
	Public Shared Operator <>(ByVal obj1 As SmartFloat, ByVal obj2 As Single) As Boolean
	  Return Not obj1.Equals(obj2)
	End Operator
	''' <summary>
	''' Compares an object of this type with an float for equality.
	''' </summary>
	''' <param name="obj1">The object of this type to compare</param>
	''' <param name="obj2">The float to compare</param>
	''' <returns>Whether the two values are equal</returns>
	Public Shared Operator =(ByVal obj1 As SmartFloat, ByVal obj2 As String) As Boolean
	  Return obj1.Equals(obj2)
	End Operator
	''' <summary>
	''' Compares an object of this type with an string for non-equality.
	''' </summary>
	''' <param name="obj1">The object of this type to compare</param>
	''' <param name="obj2">The string to compare</param>
	''' <returns>Whether the two values are not equal</returns>
	Public Shared Operator <>(ByVal obj1 As SmartFloat, ByVal obj2 As String) As Boolean
	  Return Not obj1.Equals(obj2)
	End Operator
	  ''' <summary>
	  ''' Adds an object of this type to an float.
	  ''' </summary>
	  ''' <param name="start">The object of this type to add</param>
	  ''' <param name="span">The float to add</param>
	  ''' <returns>A SmartFloat with the sum</returns>
	Public Shared Operator +(ByVal start As SmartFloat, ByVal span As Single) As SmartFloat
	  Return New SmartFloat(start.Add(span), start.EmptyIsMin)
	End Operator
	  ''' <summary>
	  ''' Subtracts an float from an object of this type.
	  ''' </summary>
	  ''' <param name="start">The object of this type to be subtracted from</param>
	  ''' <param name="span">The float to subtract</param>
	  ''' <returns>The calculated result</returns>
	Public Shared Operator -(ByVal start As SmartFloat, ByVal span As Single) As SmartFloat
	  Return New SmartFloat(start.Subtract(span), start.EmptyIsMin)
	End Operator

	''' <summary>
	''' Subtracts an object of this type from another.
	''' </summary>
	''' <param name="start">The object of this type to be subtracted from</param>
	''' <param name="finish">The object of this type to subtract</param>
	''' <returns>The calculated result</returns>
	Public Shared Operator -(ByVal start As SmartFloat, ByVal finish As SmartFloat) As Single
	  Return start.Subtract(finish.Float)
	End Operator
	''' <summary>
	''' Determines whether the first object of this type is greater than the second.
	''' </summary>
	''' <param name="obj1">The first object of this type to compare</param>
	''' <param name="obj2">The second object of this type to compare</param>
	''' <returns>Whether the first value is greater than the second</returns>
	Public Shared Operator >(ByVal obj1 As SmartFloat, ByVal obj2 As SmartFloat) As Boolean
	  Return obj1.CompareTo(obj2) > 0
	End Operator
	''' <summary>
	''' Determines whether the first object of this type is less than the second.
	''' </summary>
	''' <param name="obj1">The first object of this type to compare</param>
	''' <param name="obj2">The second object of this type to compare</param>
	''' <returns>Whether the first value is less than the second</returns>
	Public Shared Operator <(ByVal obj1 As SmartFloat, ByVal obj2 As SmartFloat) As Boolean
	  Return obj1.CompareTo(obj2) < 0
	End Operator
	  ''' <summary>
	  ''' Determines whether the first object of this type is greater than an float.
	  ''' </summary>
	  ''' <param name="obj1">The object of this type to compare</param>
	  ''' <param name="obj2">The float to compare</param>
	''' <returns>Whether the first value is greater than the second</returns>
	Public Shared Operator >(ByVal obj1 As SmartFloat, ByVal obj2 As Single) As Boolean
	  Return obj1.CompareTo(obj2) > 0
	End Operator
	''' <summary>
	''' Determines whether the first object of this type is less than an float.
	''' </summary>
	''' <param name="obj1">The object of this type to compare</param>
	''' <param name="obj2">The float to compare</param>
	''' <returns>Whether the first value is less than the second</returns>
	Public Shared Operator <(ByVal obj1 As SmartFloat, ByVal obj2 As Single) As Boolean
	  Return obj1.CompareTo(obj2) < 0
	End Operator
	''' <summary>
	''' Determines whether the first object of this type is less than the value in a string.
	''' </summary>
	''' <param name="obj1">The object of this type to compare</param>
	''' <param name="obj2">The string value to compare</param>
	''' <returns>Whether the first value is greater than the second</returns>
	Public Shared Operator >(ByVal obj1 As SmartFloat, ByVal obj2 As String) As Boolean
	  Return obj1.CompareTo(obj2) > 0
	End Operator
	  ''' <summary>
	  ''' Determines whether the first object of this type is less than the value in a string.
	  ''' </summary>
	  ''' <param name="obj1">The object of this type to compare</param>
	  ''' <param name="obj2">The string value to compare</param>
	''' <returns>Whether the first value is less than the second</returns>
	Public Shared Operator <(ByVal obj1 As SmartFloat, ByVal obj2 As String) As Boolean
	  Return obj1.CompareTo(obj2) < 0
	End Operator
	  ''' <summary>
	  ''' Determines whether the first object of this type is greater than or equal to the second.
	  ''' </summary>
	  ''' <param name="obj1">The first object of this type to compare</param>
	  ''' <param name="obj2">The second object of this type to compare</param>
	''' <returns>Whether the first value is greater than or equal to the second</returns>
	Public Shared Operator >=(ByVal obj1 As SmartFloat, ByVal obj2 As SmartFloat) As Boolean
	  Return obj1.CompareTo(obj2) >= 0
	End Operator

	''' <summary>
	''' Determines whether the first object of this type is less than or equal to the second.
	''' </summary>
	''' <param name="obj1">The first object of this type to compare</param>
	''' <param name="obj2">The second object of this type to compare</param>
	''' <returns>Whether the first value is less than or equal to the second</returns>
	Public Shared Operator <=(ByVal obj1 As SmartFloat, ByVal obj2 As SmartFloat) As Boolean
	  Return obj1.CompareTo(obj2) <= 0
	End Operator
	''' <summary>
	''' Determines whether the first object of this type is greater than or equal to an float.
	''' </summary>
	''' <param name="obj1">The object of this type to compare</param>
	''' <param name="obj2">The float to compare</param>
	''' <returns>Whether the first value is greater than or equal to the second</returns>
	Public Shared Operator >=(ByVal obj1 As SmartFloat, ByVal obj2 As Single) As Boolean
	  Return obj1.CompareTo(obj2) >= 0
	End Operator
	''' <summary>
	''' Determines whether the first object of this type is less than or equal to an float.
	''' </summary>
	''' <param name="obj1">The object of this type to compare</param>
	''' <param name="obj2">The float to compare</param>
	''' <returns>Whether the first value is less than or equal to the second</returns>
	Public Shared Operator <=(ByVal obj1 As SmartFloat, ByVal obj2 As Single) As Boolean
	  Return obj1.CompareTo(obj2) <= 0
	End Operator
	''' <summary>
	''' Determines whether the first object of this type is greater than or equal to the value of a string.
	''' </summary>
	''' <param name="obj1">The object of this type to compare</param>
	''' <param name="obj2">The string value to compare</param>
	''' <returns>Whether the first value is greater than or equal to the second</returns>
	Public Shared Operator >=(ByVal obj1 As SmartFloat, ByVal obj2 As String) As Boolean
	  Return obj1.CompareTo(obj2) >= 0
	End Operator
	''' <summary>
	''' Determines whether the first object of this type is less than or equal to the value of a string.
	''' </summary>
	''' <param name="obj1">The object of this type to compare</param>
	''' <param name="obj2">The string value to compare</param>
	''' <returns>Whether the first value is less than or equal to the second</returns>
	Public Shared Operator <=(ByVal obj1 As SmartFloat, ByVal obj2 As String) As Boolean
	  Return obj1.CompareTo(obj2) <= 0
	End Operator

	#End Region

  End Structure
End Namespace