Imports Microsoft.VisualBasic
Imports System
Imports CslaSrd


Namespace CslaSrd
    ''' <summary>
    ''' Provides an integer data type that understands the concept
    ''' of an empty value.
    ''' </summary>
    ''' <remarks>
    ''' See Chapter 5 for a full discussion of the need for a similar 
    ''' data type and the design choices behind it.  Basically, we are 
    ''' using the same approach to handle integers instead of dates.
    ''' </remarks>
    <Serializable()> _
    Public Structure SmartInt32
        Implements IComparable
        Private _int As Int32
        Private _initialized As Boolean
        Private _emptyIsMax As Boolean
        Private _format As String

#Region "Constructors"

        ''' <summary>
        ''' Creates a new SmartInt32 object.
        ''' </summary>
        ''' <param name="emptyIsMin">Indicates whether an empty int is the min or max int value.</param>
        Public Sub New(ByVal emptyIsMin As Boolean)
            _emptyIsMax = Not emptyIsMin
            _format = Nothing
            _initialized = False
            ' provide a dummy value to allow real initialization
            _int = Int32.MinValue
            If (Not _emptyIsMax) Then
                Int = Int32.MinValue
            Else
                Int = Int32.MaxValue
            End If
        End Sub

        ''' <summary>
        ''' Creates a new SmartInt32 object.
        ''' </summary>
        ''' <remarks>
        ''' The SmartInt32 created will use the min possible
        ''' int to represent an empty int.
        ''' </remarks>
        ''' <param name="value">The initial value of the object.</param>
        Public Sub New(ByVal value As Int32)
            _emptyIsMax = False
            _format = Nothing
            _initialized = False
            _int = Int32.MinValue
            Int = value
        End Sub

        ''' <summary>
        ''' Creates a new SmartInt32 object.
        ''' </summary>
        ''' <param name="value">The initial value of the object.</param>
        ''' <param name="emptyIsMin">Indicates whether an empty int is the min or max int value.</param>
        Public Sub New(ByVal value As Int32, ByVal emptyIsMin As Boolean)
            _emptyIsMax = Not emptyIsMin
            _format = Nothing
            _initialized = False
            _int = Int32.MinValue
            Int = value
        End Sub

        ''' <summary>
        ''' Creates a new SmartInt32 object.
        ''' </summary>
        ''' <remarks>
        ''' The SmartInt32 created will use the min possible
        ''' int to represent an empty int.
        ''' </remarks>
        ''' <param name="value">The initial value of the object (as text).</param>
        Public Sub New(ByVal value As String)
            _emptyIsMax = False
            _format = Nothing
            _initialized = True
            _int = Int32.MinValue
            Me.Text = value
        End Sub

        ''' <summary>
        ''' Creates a new SmartInt32 object.
        ''' </summary>
        ''' <param name="value">The initial value of the object (as text).</param>
        ''' <param name="emptyIsMin">Indicates whether an empty int is the min or max int value.</param>
        Public Sub New(ByVal value As String, ByVal emptyIsMin As Boolean)
            _emptyIsMax = Not emptyIsMin
            _format = Nothing
            _initialized = True
            _int = Int32.MinValue
            Me.Text = value
        End Sub

#End Region


#Region "Text Support"

        ''' <summary>
        ''' Gets or sets the format string used to format a int
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
            Set(ByVal value As String)
                _format = Value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the int value.
        ''' </summary>
        ''' <remarks>
        ''' <para>
        ''' This property can be used to set the int value by passing a
        ''' text representation of the int. Any text int representation
        ''' that can be parsed by the .NET runtime is valid.
        ''' </para><para>
        ''' When the int value is retrieved via this property, the text
        ''' is formatted by using the format specified by the 
        ''' <see cref="FormatString" /> property. The default is the
        ''' short int format (d).
        ''' </para>
        ''' </remarks>
        Public Property Text() As String
            Get
                Return IntToString(Me.Int, FormatString, (Not _emptyIsMax))
            End Get
            Set(ByVal value As String)
                Me.Int = StringToInt(Value, (Not _emptyIsMax))
            End Set
        End Property

#End Region

#Region "Int Support"

        ''' <summary>
        ''' Gets or sets the int value.
        ''' </summary>
        Public Property Int() As Int32
            Get
                If (Not _initialized) Then
                    _int = Int32.MinValue
                    _initialized = True
                End If
                Return _int
            End Get
            Set(ByVal value As Int32)
                _int = Value
                _initialized = True
            End Set
        End Property

#End Region

#Region "System.Object overrides"

        ''' <summary>
        ''' Returns a text representation of the int value.
        ''' </summary>
        Public Overrides Function ToString() As String
            Return Me.Text
        End Function

        ''' <summary>
        ''' Compares this object to another <see cref="SmartInt32"/>
        ''' for equality.
        ''' </summary>
        Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
            If TypeOf obj Is SmartInt32 Then
                Dim tmp As SmartInt32 = CType(obj, SmartInt32)
                If Me.IsEmpty AndAlso tmp.IsEmpty Then
                    Return True
                Else
                    Return Me.Int.Equals(tmp.Int)
                End If
            ElseIf TypeOf obj Is Int32 Then
                Return Me.Int.Equals(CInt(Fix(obj)))
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
            Return Me.Int.GetHashCode()
        End Function

#End Region

#Region "DBValue"

        ''' <summary>
        ''' Gets a database-friendly version of the int value.
        ''' </summary>
        ''' <remarks>
        ''' <para>
        ''' If the SmartInt32 contains an empty int, this returns <see cref="DBNull"/>.
        ''' Otherwise the actual int value is returned as type Int.
        ''' </para><para>
        ''' This property is very useful when setting parameter values for
        ''' a Command object, since it automatically stores null values into
        ''' the database for empty int values.
        ''' </para><para>
        ''' When you also use the SafeDataReader and its GetSmartInt32 method,
        ''' you can easily read a null value from the database back into a
        ''' SmartInt32 object so it remains considered as an empty int value.
        ''' </para>
        ''' </remarks>
        Public ReadOnly Property DBValue() As Object
            Get
                If Me.IsEmpty Then
                    Return DBNull.Value
                Else
                    Return Me.Int
                End If
            End Get
        End Property

#End Region

#Region "Empty Ints"

        ''' <summary>
        ''' Gets a value indicating whether this object contains an empty int.
        ''' </summary>
        Public ReadOnly Property IsEmpty() As Boolean
            Get
                If (Not _emptyIsMax) Then
                    Return Me.Int.Equals(Int32.MinValue)
                Else
                    Return Me.Int.Equals(Int32.MaxValue)
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets a value indicating whether an empty int is the 
        ''' min or max possible int value.
        ''' </summary>
        ''' <remarks>
        ''' Whether an empty int is considered to be the smallest or largest possible
        ''' int is only important for comparison operations. This allows you to
        ''' compare an empty int with a real int and get a meaningful result.
        ''' </remarks>
        Public ReadOnly Property EmptyIsMin() As Boolean
            Get
                Return Not _emptyIsMax
            End Get
        End Property

#End Region

#Region "Conversion Functions"

        ''' <summary>
        ''' Converts a string value into a SmartInt32.
        ''' </summary>
        ''' <param name="value">String containing the int value.</param>
        ''' <returns>A new SmartInt32 containing the int value.</returns>
        ''' <remarks>
        ''' EmptyIsMin will default to <see langword="true"/>.
        ''' </remarks>
        Public Shared Function Parse(ByVal value As String) As SmartInt32
            Return New SmartInt32(value)
        End Function

        ''' <summary>
        ''' Converts a string value into a SmartInt32.
        ''' </summary>
        ''' <param name="value">String containing the int value.</param>
        ''' <param name="emptyIsMin">Indicates whether an empty int is the min or max int value.</param>
        ''' <returns>A new SmartInt32 containing the int value.</returns>
        Public Shared Function Parse(ByVal value As String, ByVal emptyIsMin As Boolean) As SmartInt32
            Return New SmartInt32(value, emptyIsMin)
        End Function

        ''' <summary>
        ''' Converts a text int representation into a Int value.
        ''' </summary>
        ''' <remarks>
        ''' An empty string is assumed to represent an empty int. An empty int
        ''' is returned as the MinValue of the Int datatype.
        ''' </remarks>
        ''' <param name="value">The text representation of the int.</param>
        ''' <returns>A Int value.</returns>
        Public Shared Function StringToInt(ByVal value As String) As Int32
            Return StringToInt(value, True)
        End Function

        ''' <summary>
        ''' Converts a text int representation into a Int value.
        ''' </summary>
        ''' <remarks>
        ''' An empty string is assumed to represent an empty int. An empty int
        ''' is returned as the MinValue or MaxValue of the Int datatype depending
        ''' on the EmptyIsMin parameter.
        ''' </remarks>
        ''' <param name="value">The text representation of the int.</param>
        ''' <param name="emptyIsMin">Indicates whether an empty int is the min or max int value.</param>
        ''' <returns>A Int value.</returns>
        Public Shared Function StringToInt(ByVal value As String, ByVal emptyIsMin As Boolean) As Int32
            Dim tmp As Int32
            If String.IsNullOrEmpty(value) Then
                If emptyIsMin Then
                    Return Int32.MinValue
                Else
                    Return Int32.MaxValue
                End If
            ElseIf Int32.TryParse(value, tmp) Then
                Return tmp
            Else
                Dim lint As String = value.Trim().ToLower()
                Throw New ArgumentException(My.Resources.StringToInt32Exception)
            End If
        End Function

        ''' <summary>
        ''' Converts a int value into a text representation.
        ''' </summary>
        ''' <remarks>
        ''' The int is considered empty if it matches the min value for
        ''' the Int datatype. If the int is empty, this
        ''' method returns an empty string. Otherwise it returns the int
        ''' value formatted based on the FormatString parameter.
        ''' </remarks>
        ''' <param name="value">The int value to convert.</param>
        ''' <param name="formatString">The format string used to format the int into text.</param>
        ''' <returns>Text representation of the int value.</returns>
        Public Shared Function IntToString(ByVal value As Int32, ByVal formatString As String) As String
            Return IntToString(value, formatString, True)
        End Function

        ''' <summary>
        ''' Converts a int value into a text representation.
        ''' </summary>
        ''' <remarks>
        ''' Whether the int value is considered empty is determined by
        ''' the EmptyIsMin parameter value. If the int is empty, this
        ''' method returns an empty string. Otherwise it returns the int
        ''' value formatted based on the FormatString parameter.
        ''' </remarks>
        ''' <param name="value">The int value to convert.</param>
        ''' <param name="formatString">The format string used to format the int into text.</param>
        ''' <param name="emptyIsMin">Indicates whether an empty int is the min or max int value.</param>
        ''' <returns>Text representation of the int value.</returns>
        Public Shared Function IntToString(ByVal value As Int32, ByVal formatString As String, ByVal emptyIsMin As Boolean) As String
            If emptyIsMin AndAlso value = Int32.MinValue Then
                Return String.Empty
            ElseIf (Not emptyIsMin) AndAlso value = Int32.MaxValue Then
                Return String.Empty
            Else
                Return String.Format("{0:" & formatString & "}", value)
            End If
        End Function

#End Region

#Region "Manipulation Functions"

        ''' <summary>
        ''' Compares one SmartInt32 to another.
        ''' </summary>
        ''' <remarks>
        ''' This method works the same as the <see cref="int.CompareTo"/> method
        ''' on the Int inttype, with the exception that it
        ''' understands the concept of empty int values.
        ''' </remarks>
        ''' <param name="value">The int to which we are being compared.</param>
        ''' <returns>A value indicating if the comparison int is less than, equal to or greater than this int.</returns>
        Public Function CompareTo(ByVal value As SmartInt32) As Integer
            If Me.IsEmpty AndAlso value.IsEmpty Then
                Return 0
            Else
                Return _int.CompareTo(value.Int)
            End If
        End Function

        ''' <summary>
        ''' Compares one SmartInt32 to another.
        ''' </summary>
        ''' <remarks>
        ''' This method works the same as the <see cref="int.CompareTo"/> method
        ''' on the Int inttype, with the exception that it
        ''' understands the concept of empty int values.
        ''' </remarks>
        ''' <param name="value">The int to which we are being compared.</param>
        ''' <returns>A value indicating if the comparison int is less than, equal to or greater than this int.</returns>
        Private Function CompareTo(ByVal value As Object) As Integer Implements IComparable.CompareTo
            If TypeOf value Is SmartInt32 Then
                Return CompareTo(CType(value, SmartInt32))
            Else
                Throw New ArgumentException(My.Resources.ValueNotSmartInt32Exception)
            End If
        End Function

        ''' <summary>
        ''' Compares a SmartInt32 to a text int value.
        ''' </summary>
        ''' <param name="value">The int to which we are being compared.</param>
        ''' <returns>A value indicating if the comparison int is less than, equal to or greater than this int.</returns>
        Public Function CompareTo(ByVal value As String) As Integer
            Return Me.Int.CompareTo(StringToInt(value, (Not _emptyIsMax)))
        End Function

        ''' <summary>
        ''' Compares a SmartInt32 to a int value.
        ''' </summary>
        ''' <param name="value">The int to which we are being compared.</param>
        ''' <returns>A value indicating if the comparison int is less than, equal to or greater than this int.</returns>
        Public Function CompareTo(ByVal value As Int32) As Integer
            Return Me.Int.CompareTo(value)
        End Function

        ''' <summary>
        ''' Adds an integer value onto the object.
        ''' </summary>
        Public Function Add(ByVal value As Int32) As Int32
            If IsEmpty Then
                Return Me.Int
            Else
                Return CInt(Fix(Me.Int + value))
            End If
        End Function

        ''' <summary>
        ''' Subtracts an integer value from the object.
        ''' </summary>
        Public Function Subtract(ByVal value As Int32) As Int32
            If IsEmpty Then
                Return Me.Int
            Else
                Return CInt(Fix(Me.Int - value))
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
        Public Shared Operator =(ByVal obj1 As SmartInt32, ByVal obj2 As SmartInt32) As Boolean
            Return obj1.Equals(obj2)
        End Operator
        ''' <summary>
        ''' Checks two of this type of object for non-equality.
        ''' </summary>
        ''' <param name="obj1">The first object to compare</param>
        ''' <param name="obj2">The second object to compare</param>
        ''' <returns>Whether the two values are not equal</returns>
        Public Shared Operator <>(ByVal obj1 As SmartInt32, ByVal obj2 As SmartInt32) As Boolean
            Return Not obj1.Equals(obj2)
        End Operator
        ''' <summary>
        ''' Compares an object of this type with an Int32 for equality.
        ''' </summary>
        ''' <param name="obj1">The object of this type to compare</param>
        ''' <param name="obj2">The Int32 to compare</param>
        ''' <returns>Whether the two values are equal</returns>
        Public Shared Operator =(ByVal obj1 As SmartInt32, ByVal obj2 As Int32) As Boolean
            Return obj1.Equals(obj2)
        End Operator
        ''' <summary>
        ''' Compares an object of this type with an Int32 for non-equality.
        ''' </summary>
        ''' <param name="obj1">The object of this type to compare</param>
        ''' <param name="obj2">The Int32 to compare</param>
        ''' <returns>Whether the two values are not equal</returns>
        Public Shared Operator <>(ByVal obj1 As SmartInt32, ByVal obj2 As Int32) As Boolean
            Return Not obj1.Equals(obj2)
        End Operator
        ''' <summary>
        ''' Compares an object of this type with an Int32 for equality.
        ''' </summary>
        ''' <param name="obj1">The object of this type to compare</param>
        ''' <param name="obj2">The Int32 to compare</param>
        ''' <returns>Whether the two values are equal</returns>
        Public Shared Operator =(ByVal obj1 As SmartInt32, ByVal obj2 As String) As Boolean
            Return obj1.Equals(obj2)
        End Operator
        ''' <summary>
        ''' Compares an object of this type with an string for non-equality.
        ''' </summary>
        ''' <param name="obj1">The object of this type to compare</param>
        ''' <param name="obj2">The string to compare</param>
        ''' <returns>Whether the two values are not equal</returns>
        Public Shared Operator <>(ByVal obj1 As SmartInt32, ByVal obj2 As String) As Boolean
            Return Not obj1.Equals(obj2)
        End Operator
        ''' <summary>
        ''' Adds an object of this type to an Int32.
        ''' </summary>
        ''' <param name="start">The object of this type to add</param>
        ''' <param name="span">The Int32 to add</param>
        ''' <returns>A SmartInt32 with the sum</returns>
        Public Shared Operator +(ByVal start As SmartInt32, ByVal span As Int32) As SmartInt32
            Return New SmartInt32(start.Add(span), start.EmptyIsMin)
        End Operator
        ''' <summary>
        ''' Subtracts an Int32 from an object of this type.
        ''' </summary>
        ''' <param name="start">The object of this type to be subtracted from</param>
        ''' <param name="span">The Int32 to subtract</param>
        ''' <returns>The calculated result</returns>
        Public Shared Operator -(ByVal start As SmartInt32, ByVal span As Int32) As SmartInt32
            Return New SmartInt32(start.Subtract(span), start.EmptyIsMin)
        End Operator

        ''' <summary>
        ''' Subtracts an object of this type from another.
        ''' </summary>
        ''' <param name="start">The object of this type to be subtracted from</param>
        ''' <param name="finish">The object of this type to subtract</param>
        ''' <returns>The calculated result</returns>
        Public Shared Operator -(ByVal start As SmartInt32, ByVal finish As SmartInt32) As Int32
            Return start.Subtract(finish.Int)
        End Operator
        ''' <summary>
        ''' Determines whether the first object of this type is greater than the second.
        ''' </summary>
        ''' <param name="obj1">The first object of this type to compare</param>
        ''' <param name="obj2">The second object of this type to compare</param>
        ''' <returns>Whether the first value is greater than the second</returns>
        Public Shared Operator >(ByVal obj1 As SmartInt32, ByVal obj2 As SmartInt32) As Boolean
            Return obj1.CompareTo(obj2) > 0
        End Operator
        ''' <summary>
        ''' Determines whether the first object of this type is less than the second.
        ''' </summary>
        ''' <param name="obj1">The first object of this type to compare</param>
        ''' <param name="obj2">The second object of this type to compare</param>
        ''' <returns>Whether the first value is less than the second</returns>
        Public Shared Operator <(ByVal obj1 As SmartInt32, ByVal obj2 As SmartInt32) As Boolean
            Return obj1.CompareTo(obj2) < 0
        End Operator
        ''' <summary>
        ''' Determines whether the first object of this type is greater than an Int32.
        ''' </summary>
        ''' <param name="obj1">The object of this type to compare</param>
        ''' <param name="obj2">The Int32 to compare</param>
        ''' <returns>Whether the first value is greater than the second</returns>
        Public Shared Operator >(ByVal obj1 As SmartInt32, ByVal obj2 As Int32) As Boolean
            Return obj1.CompareTo(obj2) > 0
        End Operator
        ''' <summary>
        ''' Determines whether the first object of this type is less than an Int32.
        ''' </summary>
        ''' <param name="obj1">The object of this type to compare</param>
        ''' <param name="obj2">The Int32 to compare</param>
        ''' <returns>Whether the first value is less than the second</returns>
        Public Shared Operator <(ByVal obj1 As SmartInt32, ByVal obj2 As Int32) As Boolean
            Return obj1.CompareTo(obj2) < 0
        End Operator
        ''' <summary>
        ''' Determines whether the first object of this type is less than the value in a string.
        ''' </summary>
        ''' <param name="obj1">The object of this type to compare</param>
        ''' <param name="obj2">The string value to compare</param>
        ''' <returns>Whether the first value is greater than the second</returns>
        Public Shared Operator >(ByVal obj1 As SmartInt32, ByVal obj2 As String) As Boolean
            Return obj1.CompareTo(obj2) > 0
        End Operator
        ''' <summary>
        ''' Determines whether the first object of this type is less than the value in a string.
        ''' </summary>
        ''' <param name="obj1">The object of this type to compare</param>
        ''' <param name="obj2">The string value to compare</param>
        ''' <returns>Whether the first value is less than the second</returns>
        Public Shared Operator <(ByVal obj1 As SmartInt32, ByVal obj2 As String) As Boolean
            Return obj1.CompareTo(obj2) < 0
        End Operator
        ''' <summary>
        ''' Determines whether the first object of this type is greater than or equal to the second.
        ''' </summary>
        ''' <param name="obj1">The first object of this type to compare</param>
        ''' <param name="obj2">The second object of this type to compare</param>
        ''' <returns>Whether the first value is greater than or equal to the second</returns>
        Public Shared Operator >=(ByVal obj1 As SmartInt32, ByVal obj2 As SmartInt32) As Boolean
            Return obj1.CompareTo(obj2) >= 0
        End Operator

        ''' <summary>
        ''' Determines whether the first object of this type is less than or equal to the second.
        ''' </summary>
        ''' <param name="obj1">The first object of this type to compare</param>
        ''' <param name="obj2">The second object of this type to compare</param>
        ''' <returns>Whether the first value is less than or equal to the second</returns>
        Public Shared Operator <=(ByVal obj1 As SmartInt32, ByVal obj2 As SmartInt32) As Boolean
            Return obj1.CompareTo(obj2) <= 0
        End Operator
        ''' <summary>
        ''' Determines whether the first object of this type is greater than or equal to an Int32.
        ''' </summary>
        ''' <param name="obj1">The object of this type to compare</param>
        ''' <param name="obj2">The Int32 to compare</param>
        ''' <returns>Whether the first value is greater than or equal to the second</returns>
        Public Shared Operator >=(ByVal obj1 As SmartInt32, ByVal obj2 As Int32) As Boolean
            Return obj1.CompareTo(obj2) >= 0
        End Operator
        ''' <summary>
        ''' Determines whether the first object of this type is less than or equal to an Int32.
        ''' </summary>
        ''' <param name="obj1">The object of this type to compare</param>
        ''' <param name="obj2">The Int32 to compare</param>
        ''' <returns>Whether the first value is less than or equal to the second</returns>
        Public Shared Operator <=(ByVal obj1 As SmartInt32, ByVal obj2 As Int32) As Boolean
            Return obj1.CompareTo(obj2) <= 0
        End Operator
        ''' <summary>
        ''' Determines whether the first object of this type is greater than or equal to the value of a string.
        ''' </summary>
        ''' <param name="obj1">The object of this type to compare</param>
        ''' <param name="obj2">The string value to compare</param>
        ''' <returns>Whether the first value is greater than or equal to the second</returns>
        Public Shared Operator >=(ByVal obj1 As SmartInt32, ByVal obj2 As String) As Boolean
            Return obj1.CompareTo(obj2) >= 0
        End Operator
        ''' <summary>
        ''' Determines whether the first object of this type is less than or equal to the value of a string.
        ''' </summary>
        ''' <param name="obj1">The object of this type to compare</param>
        ''' <param name="obj2">The string value to compare</param>
        ''' <returns>Whether the first value is less than or equal to the second</returns>
        Public Shared Operator <=(ByVal obj1 As SmartInt32, ByVal obj2 As String) As Boolean
            Return obj1.CompareTo(obj2) <= 0
        End Operator

#End Region

    End Structure
End Namespace