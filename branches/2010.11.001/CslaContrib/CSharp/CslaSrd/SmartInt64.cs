using System;
using CslaSrd;
using CslaSrd.Properties;

namespace CslaSrd
{
    /// <summary>
    /// Provides an integer data type that understands the concept
    /// of an empty value.
    /// </summary>
    /// <remarks>
    /// See Chapter 5 for a full discussion of the need for a similar 
    /// data type and the design choices behind it.  Basically, we are 
    /// using the same approach to handle integers instead of dates.
    /// </remarks>
    [Serializable()]
    public struct SmartInt64 : IComparable, ISmartField
    {
        private Int64 _int;
        private bool _initialized;
        private bool _emptyIsMax;
        private string _format;

        #region Constructors

        /// <summary>
        /// Creates a new SmartInt64 object.
        /// </summary>
        /// <param name="emptyIsMin">Indicates whether an empty int is the min or max int value.</param>
        public SmartInt64(bool emptyIsMin)
        {
            _emptyIsMax = !emptyIsMin;
            _format = null;
            _initialized = false;
            // provide a dummy value to allow real initialization
            _int = Int64.MinValue;
            if (!_emptyIsMax)
                Int = Int64.MinValue;
            else
                Int = Int64.MaxValue;
        }

        /// <summary>
        /// Creates a new SmartInt64 object.
        /// </summary>
        /// <remarks>
        /// The SmartInt64 created will use the min possible
        /// int to represent an empty int.
        /// </remarks>
        /// <param name="value">The initial value of the object.</param>
        public SmartInt64(Int64 value)
        {
            _emptyIsMax = false;
            _format = null;
            _initialized = false;
            _int = Int64.MinValue;
            Int = value;
        }

        /// <summary>
        /// Creates a new SmartInt64 object.
        /// </summary>
        /// <param name="value">The initial value of the object.</param>
        /// <param name="emptyIsMin">Indicates whether an empty int is the min or max int value.</param>
        public SmartInt64(Int64 value, bool emptyIsMin)
        {
            _emptyIsMax = !emptyIsMin;
            _format = null;
            _initialized = false;
            _int = Int64.MinValue;
            Int = value;
        }

        /// <summary>
        /// Creates a new SmartInt64 object.
        /// </summary>
        /// <remarks>
        /// The SmartInt64 created will use the min possible
        /// int to represent an empty int.
        /// </remarks>
        /// <param name="value">The initial value of the object (as text).</param>
        public SmartInt64(string value)
        {
            _emptyIsMax = false;
            _format = null;
            _initialized = true;
            _int = Int64.MinValue;
            this.Text = value;
        }

        /// <summary>
        /// Creates a new SmartInt64 object.
        /// </summary>
        /// <param name="value">The initial value of the object (as text).</param>
        /// <param name="emptyIsMin">Indicates whether an empty int is the min or max int value.</param>
        public SmartInt64(string value, bool emptyIsMin)
        {
            _emptyIsMax = !emptyIsMin;
            _format = null;
            _initialized = true;
            _int = Int64.MinValue;
            this.Text = value;
        }

        #endregion


        #region Text Support

        /// <summary>
        /// Gets or sets the format string used to format a int
        /// value when it is returned as text.
        /// </summary>
        /// <remarks>
        /// The format string should follow the requirements for the
        /// .NET <see cref="System.String.Format"/> statement.
        /// </remarks>
        /// <value>A format string.</value>
        public string FormatString
        {
            get
            {
                if (_format == null)
                    _format = "d";
                return _format;
            }
            set
            {
                _format = value;
            }
        }

        /// <summary>
        /// Gets or sets the int value.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property can be used to set the int value by passing a
        /// text representation of the int. Any text int representation
        /// that can be parsed by the .NET runtime is valid.
        /// </para><para>
        /// When the int value is retrieved via this property, the text
        /// is formatted by using the format specified by the 
        /// <see cref="FormatString" /> property. The default is the
        /// short int format (d).
        /// </para>
        /// </remarks>
        public string Text
        {
            get { return IntToString(this.Int, FormatString, !_emptyIsMax); }
            set { this.Int = StringToInt(value, !_emptyIsMax); }
        }

        #endregion

        #region Int Support
        /// <summary>
        /// Gets a value indicating whether this object contains an empy value.
        /// </summary>
        /// <returns>True if the value is empty.</returns>
        public bool HasNullValue()
        {
            return !_initialized;
        }

        /// <summary>
        /// Gets or sets the int value.
        /// </summary>
        public Int64 Int
        {
            get
            {
                if (!_initialized)
                {
                    _int = Int64.MinValue;
                    _initialized = true;
                }
                return _int;
            }
            set
            {
                _int = value;
                _initialized = true;
            }
        }

        #endregion

        #region System.Object overrides

        /// <summary>
        /// Returns a text representation of the int value.
        /// </summary>
        public override string ToString()
        {
            return this.Text;
        }

        /// <summary>
        /// Compares this object to another <see cref="SmartInt64"/>
        /// for equality.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is SmartInt64)
            {
                SmartInt64 tmp = (SmartInt64)obj;
                if (this.IsEmpty && tmp.IsEmpty)
                    return true;
                else
                    return this.Int.Equals(tmp.Int);
            }
            else if (obj is Int64)
                return this.Int.Equals((Int64)obj);
            else if (obj is string)
                return (this.CompareTo(obj.ToString()) == 0);
            else
                return false;
        }

        /// <summary>
        /// Returns a hash code for this object.
        /// </summary>
        public override int GetHashCode()
        {
            return this.Int.GetHashCode();
        }

        #endregion

        #region DBValue

        /// <summary>
        /// Gets a database-friendly version of the int value.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If the SmartInt64 contains an empty int, this returns <see cref="DBNull"/>.
        /// Otherwise the actual int value is returned as type Int.
        /// </para><para>
        /// This property is very useful when setting parameter values for
        /// a Command object, since it automatically stores null values into
        /// the database for empty int values.
        /// </para><para>
        /// When you also use the SafeDataReader and its GetSmartInt64 method,
        /// you can easily read a null value from the database back into a
        /// SmartInt64 object so it remains considered as an empty int value.
        /// </para>
        /// </remarks>
        public object DBValue
        {
            get
            {
                if (this.IsEmpty)
                    return DBNull.Value;
                else
                    return this.Int;
            }
        }

        #endregion

        #region Empty Ints

        /// <summary>
        /// Gets a value indicating whether this object contains an empty int.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                if (!_emptyIsMax)
                    return this.Int.Equals(Int64.MinValue);
                else
                    return this.Int.Equals(Int64.MaxValue);
            }
        }

        /// <summary>
        /// Gets a value indicating whether an empty int is the 
        /// min or max possible int value.
        /// </summary>
        /// <remarks>
        /// Whether an empty int is considered to be the smallest or largest possible
        /// int is only important for comparison operations. This allows you to
        /// compare an empty int with a real int and get a meaningful result.
        /// </remarks>
        public bool EmptyIsMin
        {
            get { return !_emptyIsMax; }
        }

        #endregion

        #region Conversion Functions

        /// <summary>
        /// Converts a string value into a SmartInt64.
        /// </summary>
        /// <param name="value">String containing the int value.</param>
        /// <returns>A new SmartInt64 containing the int value.</returns>
        /// <remarks>
        /// EmptyIsMin will default to <see langword="true"/>.
        /// </remarks>
        public static SmartInt64 Parse(string value)
        {
            return new SmartInt64(value);
        }

        /// <summary>
        /// Converts a string value into a SmartInt64.
        /// </summary>
        /// <param name="value">String containing the int value.</param>
        /// <param name="emptyIsMin">Indicates whether an empty int is the min or max int value.</param>
        /// <returns>A new SmartInt64 containing the int value.</returns>
        public static SmartInt64 Parse(string value, bool emptyIsMin)
        {
            return new SmartInt64(value, emptyIsMin);
        }

        /// <summary>
        /// Converts a text int representation into a Int value.
        /// </summary>
        /// <remarks>
        /// An empty string is assumed to represent an empty int. An empty int
        /// is returned as the MinValue of the Int datatype.
        /// </remarks>
        /// <param name="value">The text representation of the int.</param>
        /// <returns>A Int value.</returns>
        public static Int64 StringToInt(string value)
        {
            return StringToInt(value, true);
        }

        /// <summary>
        /// Converts a text int representation into a Int value.
        /// </summary>
        /// <remarks>
        /// An empty string is assumed to represent an empty int. An empty int
        /// is returned as the MinValue or MaxValue of the Int datatype depending
        /// on the EmptyIsMin parameter.
        /// </remarks>
        /// <param name="value">The text representation of the int.</param>
        /// <param name="emptyIsMin">Indicates whether an empty int is the min or max int value.</param>
        /// <returns>A Int value.</returns>
        public static Int64 StringToInt(string value, bool emptyIsMin)
        {
            Int64 tmp;
            if (String.IsNullOrEmpty(value))
            {
                if (emptyIsMin)
                    return Int64.MinValue;
                else
                    return Int64.MaxValue;
            }
            else if (Int64.TryParse(value, out tmp))
                return tmp;
            else
            {
                string lint = value.Trim().ToLower();
                throw new ArgumentException(Resources.StringToInt64Exception);
            }
        }

        /// <summary>
        /// Converts a int value into a text representation.
        /// </summary>
        /// <remarks>
        /// The int is considered empty if it matches the min value for
        /// the Int datatype. If the int is empty, this
        /// method returns an empty string. Otherwise it returns the int
        /// value formatted based on the FormatString parameter.
        /// </remarks>
        /// <param name="value">The int value to convert.</param>
        /// <param name="formatString">The format string used to format the int into text.</param>
        /// <returns>Text representation of the int value.</returns>
        public static string IntToString(Int64 value, string formatString)
        {
            return IntToString(value, formatString, true);
        }

        /// <summary>
        /// Converts a int value into a text representation.
        /// </summary>
        /// <remarks>
        /// Whether the int value is considered empty is determined by
        /// the EmptyIsMin parameter value. If the int is empty, this
        /// method returns an empty string. Otherwise it returns the int
        /// value formatted based on the FormatString parameter.
        /// </remarks>
        /// <param name="value">The int value to convert.</param>
        /// <param name="formatString">The format string used to format the int into text.</param>
        /// <param name="emptyIsMin">Indicates whether an empty int is the min or max int value.</param>
        /// <returns>Text representation of the int value.</returns>
        public static string IntToString(
          Int64 value, string formatString, bool emptyIsMin)
        {
            if (emptyIsMin && value == Int64.MinValue)
                return string.Empty;
            else if (!emptyIsMin && value == Int64.MaxValue)
                return string.Empty;
            else
                return string.Format("{0:" + formatString + "}", value);
        }

        #endregion

        #region Manipulation Functions

        /// <summary>
        /// Compares one SmartInt64 to another.
        /// </summary>
        /// <remarks>
        /// This method works the same as the <see cref="int.CompareTo"/> method
        /// on the Int inttype, with the exception that it
        /// understands the concept of empty int values.
        /// </remarks>
        /// <param name="value">The int to which we are being compared.</param>
        /// <returns>A value indicating if the comparison int is less than, equal to or greater than this int.</returns>
        public int CompareTo(SmartInt64 value)
        {
            if (this.IsEmpty && value.IsEmpty)
                return 0;
            else
                return _int.CompareTo(value.Int);
        }

        /// <summary>
        /// Compares one SmartInt64 to another.
        /// </summary>
        /// <remarks>
        /// This method works the same as the <see cref="int.CompareTo"/> method
        /// on the Int inttype, with the exception that it
        /// understands the concept of empty int values.
        /// </remarks>
        /// <param name="value">The int to which we are being compared.</param>
        /// <returns>A value indicating if the comparison int is less than, equal to or greater than this int.</returns>
        int IComparable.CompareTo(object value)
        {
            if (value is SmartInt64)
                return CompareTo((SmartInt64)value);
            else
                throw new ArgumentException(Resources.ValueNotSmartInt64Exception);
        }

        /// <summary>
        /// Compares a SmartInt64 to a text int value.
        /// </summary>
        /// <param name="value">The int to which we are being compared.</param>
        /// <returns>A value indicating if the comparison int is less than, equal to or greater than this int.</returns>
        public int CompareTo(string value)
        {
            return this.Int.CompareTo(StringToInt(value, !_emptyIsMax));
        }

        /// <summary>
        /// Compares a SmartInt64 to a int value.
        /// </summary>
        /// <param name="value">The int to which we are being compared.</param>
        /// <returns>A value indicating if the comparison int is less than, equal to or greater than this int.</returns>
        public int CompareTo(Int64 value)
        {
            return this.Int.CompareTo(value);
        }

        /// <summary>
        /// Adds an integer value onto the object.
        /// </summary>
        public Int64 Add(Int64 value)
        {
            if (IsEmpty)
                return this.Int;
            else
                return (Int64)(this.Int + value);
        }

        /// <summary>
        /// Subtracts an integer value from the object.
        /// </summary>
        public Int64 Subtract(Int64 value)
        {
            if (IsEmpty)
                return this.Int;
            else
                return (Int64)(this.Int - value);
        }

        #endregion

        #region Operators
        /// <summary>
        /// Compares two of this type of object for equality.
        /// </summary>
        /// <param name="obj1">The first object to compare</param>
        /// <param name="obj2">The second object to compare</param>
        /// <returns>Whether the object values are equal</returns>
        public static bool operator ==(SmartInt64 obj1, SmartInt64 obj2)
        {
            return obj1.Equals(obj2);
        }
        /// <summary>
        /// Checks two of this type of object for non-equality.
        /// </summary>
        /// <param name="obj1">The first object to compare</param>
        /// <param name="obj2">The second object to compare</param>
        /// <returns>Whether the two values are not equal</returns>
        public static bool operator !=(SmartInt64 obj1, SmartInt64 obj2)
        {
            return !obj1.Equals(obj2);
        }
        /// <summary>
        /// Compares an object of this type with an Int64 for equality.
        /// </summary>
        /// <param name="obj1">The object of this type to compare</param>
        /// <param name="obj2">The Int64 to compare</param>
        /// <returns>Whether the two values are equal</returns>
        public static bool operator ==(SmartInt64 obj1, Int64 obj2)
        {
            return obj1.Equals(obj2);
        }
        /// <summary>
        /// Compares an object of this type with an Int64 for non-equality.
        /// </summary>
        /// <param name="obj1">The object of this type to compare</param>
        /// <param name="obj2">The Int64 to compare</param>
        /// <returns>Whether the two values are not equal</returns>
        public static bool operator !=(SmartInt64 obj1, Int64 obj2)
        {
            return !obj1.Equals(obj2);
        }
        /// <summary>
        /// Compares an object of this type with an Int64 for equality.
        /// </summary>
        /// <param name="obj1">The object of this type to compare</param>
        /// <param name="obj2">The Int64 to compare</param>
        /// <returns>Whether the two values are equal</returns>
        public static bool operator ==(SmartInt64 obj1, string obj2)
        {
            return obj1.Equals(obj2);
        }
        /// <summary>
        /// Compares an object of this type with an string for non-equality.
        /// </summary>
        /// <param name="obj1">The object of this type to compare</param>
        /// <param name="obj2">The string to compare</param>
        /// <returns>Whether the two values are not equal</returns>
        public static bool operator !=(SmartInt64 obj1, string obj2)
        {
            return !obj1.Equals(obj2);
        }
        /// <summary>
        /// Adds an object of this type to an Int64.
        /// </summary>
        /// <param name="start">The object of this type to add</param>
        /// <param name="span">The Int64 to add</param>
        /// <returns>A SmartInt64 with the sum</returns>
        public static SmartInt64 operator +(SmartInt64 start, Int64 span)
        {
            return new SmartInt64(start.Add(span), start.EmptyIsMin);
        }
        /// <summary>
        /// Subtracts an Int64 from an object of this type.
        /// </summary>
        /// <param name="start">The object of this type to be subtracted from</param>
        /// <param name="span">The Int64 to subtract</param>
        /// <returns>The calculated result</returns>
        public static SmartInt64 operator -(SmartInt64 start, Int64 span)
        {
            return new SmartInt64(start.Subtract(span), start.EmptyIsMin);
        }

        /// <summary>
        /// Subtracts an object of this type from another.
        /// </summary>
        /// <param name="start">The object of this type to be subtracted from</param>
        /// <param name="finish">The object of this type to subtract</param>
        /// <returns>The calculated result</returns>
        public static Int64 operator -(SmartInt64 start, SmartInt64 finish)
        {
            return start.Subtract(finish.Int);
        }
        /// <summary>
        /// Determines whether the first object of this type is greater than the second.
        /// </summary>
        /// <param name="obj1">The first object of this type to compare</param>
        /// <param name="obj2">The second object of this type to compare</param>
        /// <returns>Whether the first value is greater than the second</returns>
        public static bool operator >(SmartInt64 obj1, SmartInt64 obj2)
        {
            return obj1.CompareTo(obj2) > 0;
        }
        /// <summary>
        /// Determines whether the first object of this type is less than the second.
        /// </summary>
        /// <param name="obj1">The first object of this type to compare</param>
        /// <param name="obj2">The second object of this type to compare</param>
        /// <returns>Whether the first value is less than the second</returns>
        public static bool operator <(SmartInt64 obj1, SmartInt64 obj2)
        {
            return obj1.CompareTo(obj2) < 0;
        }
        /// <summary>
        /// Determines whether the first object of this type is greater than an Int64.
        /// </summary>
        /// <param name="obj1">The object of this type to compare</param>
        /// <param name="obj2">The Int64 to compare</param>
        /// <returns>Whether the first value is greater than the second</returns>
        public static bool operator >(SmartInt64 obj1, Int64 obj2)
        {
            return obj1.CompareTo(obj2) > 0;
        }
        /// <summary>
        /// Determines whether the first object of this type is less than an Int64.
        /// </summary>
        /// <param name="obj1">The object of this type to compare</param>
        /// <param name="obj2">The Int64 to compare</param>
        /// <returns>Whether the first value is less than the second</returns>
        public static bool operator <(SmartInt64 obj1, Int64 obj2)
        {
            return obj1.CompareTo(obj2) < 0;
        }
        /// <summary>
        /// Determines whether the first object of this type is less than the value in a string.
        /// </summary>
        /// <param name="obj1">The object of this type to compare</param>
        /// <param name="obj2">The string value to compare</param>
        /// <returns>Whether the first value is greater than the second</returns>
        public static bool operator >(SmartInt64 obj1, string obj2)
        {
            return obj1.CompareTo(obj2) > 0;
        }
        /// <summary>
        /// Determines whether the first object of this type is less than the value in a string.
        /// </summary>
        /// <param name="obj1">The object of this type to compare</param>
        /// <param name="obj2">The string value to compare</param>
        /// <returns>Whether the first value is less than the second</returns>
        public static bool operator <(SmartInt64 obj1, string obj2)
        {
            return obj1.CompareTo(obj2) < 0;
        }
        /// <summary>
        /// Determines whether the first object of this type is greater than or equal to the second.
        /// </summary>
        /// <param name="obj1">The first object of this type to compare</param>
        /// <param name="obj2">The second object of this type to compare</param>
        /// <returns>Whether the first value is greater than or equal to the second</returns>
        public static bool operator >=(SmartInt64 obj1, SmartInt64 obj2)
        {
            return obj1.CompareTo(obj2) >= 0;
        }

        /// <summary>
        /// Determines whether the first object of this type is less than or equal to the second.
        /// </summary>
        /// <param name="obj1">The first object of this type to compare</param>
        /// <param name="obj2">The second object of this type to compare</param>
        /// <returns>Whether the first value is less than or equal to the second</returns>
        public static bool operator <=(SmartInt64 obj1, SmartInt64 obj2)
        {
            return obj1.CompareTo(obj2) <= 0;
        }
        /// <summary>
        /// Determines whether the first object of this type is greater than or equal to an Int64.
        /// </summary>
        /// <param name="obj1">The object of this type to compare</param>
        /// <param name="obj2">The Int64 to compare</param>
        /// <returns>Whether the first value is greater than or equal to the second</returns>
        public static bool operator >=(SmartInt64 obj1, Int64 obj2)
        {
            return obj1.CompareTo(obj2) >= 0;
        }
        /// <summary>
        /// Determines whether the first object of this type is less than or equal to an Int64.
        /// </summary>
        /// <param name="obj1">The object of this type to compare</param>
        /// <param name="obj2">The Int64 to compare</param>
        /// <returns>Whether the first value is less than or equal to the second</returns>
        public static bool operator <=(SmartInt64 obj1, Int64 obj2)
        {
            return obj1.CompareTo(obj2) <= 0;
        }
        /// <summary>
        /// Determines whether the first object of this type is greater than or equal to the value of a string.
        /// </summary>
        /// <param name="obj1">The object of this type to compare</param>
        /// <param name="obj2">The string value to compare</param>
        /// <returns>Whether the first value is greater than or equal to the second</returns>
        public static bool operator >=(SmartInt64 obj1, string obj2)
        {
            return obj1.CompareTo(obj2) >= 0;
        }
        /// <summary>
        /// Determines whether the first object of this type is less than or equal to the value of a string.
        /// </summary>
        /// <param name="obj1">The object of this type to compare</param>
        /// <param name="obj2">The string value to compare</param>
        /// <returns>Whether the first value is less than or equal to the second</returns>
        public static bool operator <=(SmartInt64 obj1, string obj2)
        {
            return obj1.CompareTo(obj2) <= 0;
        }

        #endregion

    }
}