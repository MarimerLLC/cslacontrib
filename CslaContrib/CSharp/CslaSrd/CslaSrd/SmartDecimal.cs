using System;
using CslaSrd;
using CslaSrd.Properties;

namespace CslaSrd
{
  /// <summary>
  /// Provides an decimal data type that understands the concept
  /// of an empty value.
  /// </summary>
  /// <remarks>
  /// See Chapter 5 for a full discussion of the need for a similar 
  /// data type and the design choices behind it.  Basically, we are 
  /// using the same approach to handle decimals instead of dates.
  /// </remarks>
  [Serializable()]
  public struct SmartDecimal : IComparable, ISmartField
  {
    private decimal  _decimal;
    private bool     _initialized;
    private bool     _emptyIsMax;
    private string   _format;

    #region Constructors

    /// <summary>
    /// Creates a new SmartDecimal object.
    /// </summary>
    /// <param name="emptyIsMin">Indicates whether an empty decimal is the min or max decimal value.</param>
    public SmartDecimal(bool emptyIsMin)
    {
      _emptyIsMax  = !emptyIsMin;
      _format      = null;
      _initialized = false;
      // provide a dummy value to allow real initialization
      _decimal         = decimal.MinValue;
      if (!_emptyIsMax)
        Decimal = decimal.MinValue;
      else
        Decimal = decimal.MaxValue;
    }

    /// <summary>
    /// Creates a new SmartDecimal object.
    /// </summary>
    /// <remarks>
    /// The SmartDecimal created will use the min possible
    /// decimal to represent an empty decimal.
    /// </remarks>
    /// <param name="value">The initial value of the object.</param>
    public SmartDecimal(decimal value)
    {
      _emptyIsMax = false;
      _format = null;
      _initialized = false;
      _decimal = decimal.MinValue;
      Decimal = value;
    }

    /// <summary>
    /// Creates a new SmartDecimal object.
    /// </summary>
    /// <param name="value">The initial value of the object.</param>
    /// <param name="emptyIsMin">Indicates whether an empty decimal is the min or max decimal value.</param>
    public SmartDecimal(decimal value, bool emptyIsMin)
    {
      _emptyIsMax = !emptyIsMin;
      _format = null;
      _initialized = false;
      _decimal = decimal.MinValue;
      Decimal = value;
    }

    /// <summary>
    /// Creates a new SmartDecimal object.
    /// </summary>
    /// <remarks>
    /// The SmartDecimal created will use the min possible
    /// decimal to represent an empty decimal.
    /// </remarks>
    /// <param name="value">The initial value of the object (as text).</param>
    public SmartDecimal(string value)
    {
      _emptyIsMax = false;
      _format = null;
      _initialized = true;
      _decimal = decimal.MinValue;
      this.Text = value;
    }

    /// <summary>
    /// Creates a new SmartDecimal object.
    /// </summary>
    /// <param name="value">The initial value of the object (as text).</param>
    /// <param name="emptyIsMin">Indicates whether an empty decimal is the min or max decimal value.</param>
    public SmartDecimal(string value, bool emptyIsMin)
    {
      _emptyIsMax = !emptyIsMin;
      _format = null;
      _initialized = true;
      _decimal = decimal.MinValue;
      this.Text = value;
    }

    #endregion


    #region Text Support

    /// <summary>
    /// Gets or sets the format string used to format a decimal
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
          _format = "g";
        return _format;
      }
      set
      {
        _format = value;
      }
    }

    /// <summary>
    /// Gets or sets the decimal value.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property can be used to set the decimal value by passing a
    /// text representation of the decimal. Any text decimal representation
    /// that can be parsed by the .NET runtime is valid.
    /// </para><para>
    /// When the decimal value is retrieved via this property, the text
    /// is formatted by using the format specified by the 
    /// <see cref="FormatString" /> property. The default is the
    /// short decimal format (d).
    /// </para>
    /// </remarks>
    public string Text
    {
      get { return DecimalToString(this.Decimal, FormatString, !_emptyIsMax); }
      set { this.Decimal = StringToDecimal(value, !_emptyIsMax); }
    }

    #endregion

    #region Decimal Support

    /// <summary>
    /// Gets or sets the decimal value.
    /// </summary>
    public decimal Decimal
    {
      get
      {
        if (!_initialized)
        {
          _decimal = decimal.MinValue;
          _initialized = true;
        }
        return _decimal;
      }
      set
      {
        _decimal = value;
        _initialized = true;
      }
    }

    #endregion

    #region System.Object overrides

    /// <summary>
    /// Returns a text representation of the decimal value.
    /// </summary>
    public override string ToString()
    {
      return this.Text;
    }

    /// <summary>
    /// Compares this object to another <see cref="SmartDecimal"/>
    /// for equality.
    /// </summary>
    public override bool Equals(object obj)
    {
      if (obj is SmartDecimal)
      {
        SmartDecimal tmp = (SmartDecimal)obj;
        if (this.IsEmpty && tmp.IsEmpty)
          return true;
        else
          return this.Decimal.Equals(tmp.Decimal);
      }
      else if (obj is decimal)
        return this.Decimal.Equals((decimal)obj);
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
      return this.Decimal.GetHashCode();
    }

    #endregion

    #region DBValue

    /// <summary>
    /// Gets a database-friendly version of the decimal value.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If the SmartDecimal contains an empty decimal, this returns <see cref="DBNull"/>.
    /// Otherwise the actual decimal value is returned as type Decimal.
    /// </para><para>
    /// This property is very useful when setting parameter values for
    /// a Command object, since it automatically stores null values into
    /// the database for empty decimal values.
    /// </para><para>
    /// When you also use the SafeDataReader and its GetSmartDecimal method,
    /// you can easily read a null value from the database back into a
    /// SmartDecimal object so it remains considered as an empty decimal value.
    /// </para>
    /// </remarks>
    public object DBValue
    {
      get
      {
        if (this.IsEmpty)
          return DBNull.Value;
        else
          return this.Decimal;
      }
    }

    #endregion

    #region Empty Decimals

    /// <summary>
    /// Gets a value indicating whether this object contains an empy value.
    /// </summary>
    /// <returns>True if the value is empty.</returns>
    public bool HasNullValue()
    {
        return !_initialized;
    }

    /// <summary>
    /// Gets a value indicating whether this object contains an empty decimal.
    /// </summary>
    public bool IsEmpty
    {
      get
      {
        if (!_emptyIsMax)
          return this.Decimal.Equals(decimal.MinValue);
        else
          return this.Decimal.Equals(decimal.MaxValue);
      }
    }

    /// <summary>
    /// Gets a value indicating whether an empty decimal is the 
    /// min or max possible decimal value.
    /// </summary>
    /// <remarks>
    /// Whether an empty decimal is considered to be the smallest or largest possible
    /// decimal is only important for comparison operations. This allows you to
    /// compare an empty decimal with a real decimal and get a meaningful result.
    /// </remarks>
    public bool EmptyIsMin
    {
      get { return !_emptyIsMax; }
    }

    #endregion

    #region Conversion Functions

    /// <summary>
    /// Converts a string value into a SmartDecimal.
    /// </summary>
    /// <param name="value">String containing the decimal value.</param>
    /// <returns>A new SmartDecimal containing the decimal value.</returns>
    /// <remarks>
    /// EmptyIsMin will default to <see langword="true"/>.
    /// </remarks>
    public static SmartDecimal Parse(string value)
    {
      return new SmartDecimal(value);
    }

    /// <summary>
    /// Converts a string value into a SmartDecimal.
    /// </summary>
    /// <param name="value">String containing the decimal value.</param>
    /// <param name="emptyIsMin">Indicates whether an empty decimal is the min or max decimal value.</param>
    /// <returns>A new SmartDecimal containing the decimal value.</returns>
    public static SmartDecimal Parse(string value, bool emptyIsMin)
    {
      return new SmartDecimal(value, emptyIsMin);
    }

    /// <summary>
    /// Converts a text decimal representation into a Decimal value.
    /// </summary>
    /// <remarks>
    /// An empty string is assumed to represent an empty decimal. An empty decimal
    /// is returned as the MinValue of the Decimal datatype.
    /// </remarks>
    /// <param name="value">The text representation of the decimal.</param>
    /// <returns>A Decimal value.</returns>
    public static decimal StringToDecimal(string value)
    {
      return StringToDecimal(value, true);
    }

    /// <summary>
    /// Converts a text decimal representation into a Decimal value.
    /// </summary>
    /// <remarks>
    /// An empty string is assumed to represent an empty decimal. An empty decimal
    /// is returned as the MinValue or MaxValue of the Decimal datatype depending
    /// on the EmptyIsMin parameter.
    /// </remarks>
    /// <param name="value">The text representation of the decimal.</param>
    /// <param name="emptyIsMin">Indicates whether an empty decimal is the min or max decimal value.</param>
    /// <returns>A Decimal value.</returns>
    public static decimal StringToDecimal(string value, bool emptyIsMin)
    {
      decimal tmp;
      if (String.IsNullOrEmpty(value))
      {
        if (emptyIsMin)
          return decimal.MinValue;
        else
          return decimal.MaxValue;
      }
      else if (decimal.TryParse(value, out tmp))
        return tmp; 
      else
      {
        string lint = value.Trim().ToLower();
        throw new ArgumentException(Resources.StringToDecimalException);
      }
    }

    /// <summary>
    /// Converts a decimal value into a text representation.
    /// </summary>
    /// <remarks>
    /// The decimal is considered empty if it matches the min value for
    /// the Decimal datatype. If the decimal is empty, this
    /// method returns an empty string. Otherwise it returns the decimal
    /// value formatted based on the FormatString parameter.
    /// </remarks>
    /// <param name="value">The decimal value to convert.</param>
    /// <param name="formatString">The format string used to format the decimal into text.</param>
    /// <returns>Text representation of the decimal value.</returns>
    public static string DecimalToString(decimal value, string formatString)
    {
      return DecimalToString(value, formatString, true);
    }

    /// <summary>
    /// Converts a decimal value into a text representation.
    /// </summary>
    /// <remarks>
    /// Whether the decimal value is considered empty is determined by
    /// the EmptyIsMin parameter value. If the decimal is empty, this
    /// method returns an empty string. Otherwise it returns the decimal
    /// value formatted based on the FormatString parameter.
    /// </remarks>
    /// <param name="value">The decimal value to convert.</param>
    /// <param name="formatString">The format string used to format the decimal into text.</param>
    /// <param name="emptyIsMin">Indicates whether an empty decimal is the min or max decimal value.</param>
    /// <returns>Text representation of the decimal value.</returns>
    public static string DecimalToString(
      decimal value, string formatString, bool emptyIsMin)
    {
      if (emptyIsMin && value == decimal.MinValue)
        return string.Empty;
      else if (!emptyIsMin && value == decimal.MaxValue)
        return string.Empty;
      else
        return string.Format("{0:" + formatString + "}", value);
    }

    #endregion

    #region Manipulation Functions

    /// <summary>
    /// Compares one SmartDecimal to another.
    /// </summary>
    /// <remarks>
    /// This method works the same as the <see cref="int.CompareTo"/> method
    /// on the Decimal inttype, with the exception that it
    /// understands the concept of empty int values.
    /// </remarks>
    /// <param name="value">The int to which we are being compared.</param>
    /// <returns>A value indicating if the comparison int is less than, equal to or greater than this int.</returns>
    public int CompareTo(SmartDecimal value)
    {
      if (this.IsEmpty && value.IsEmpty)
        return 0;
      else
        return _decimal.CompareTo(value.Decimal);
    }

    /// <summary>
    /// Compares one SmartDecimal to another.
    /// </summary>
    /// <remarks>
    /// This method works the same as the <see cref="int.CompareTo"/> method
    /// on the decimal type, with the exception that it
    /// understands the concept of empty decimal values.
    /// </remarks>
    /// <param name="value">The int to which we are being compared.</param>
    /// <returns>A value indicating if the comparison int is less than, equal to or greater than this int.</returns>
    int IComparable.CompareTo(object value)
    {
      if (value is SmartDecimal)
        return CompareTo((SmartDecimal)value);
      else
        throw new ArgumentException(Resources.ValueNotSmartDecimalException);
    }

    /// <summary>
    /// Compares a SmartDecimal to a text int value.
    /// </summary>
    /// <param name="value">The int to which we are being compared.</param>
    /// <returns>A value indicating if the comparison int is less than, equal to or greater than this int.</returns>
    public int CompareTo(string value)
    {
      return this.Decimal.CompareTo(StringToDecimal(value, !_emptyIsMax));
    }

    /// <summary>
    /// Compares a SmartDecimal to a int value.
    /// </summary>
    /// <param name="value">The int to which we are being compared.</param>
    /// <returns>A value indicating if the comparison int is less than, equal to or greater than this int.</returns>
    public int CompareTo(decimal value)
    {
      return this.Decimal.CompareTo(value);
    }

    /// <summary>
    /// Adds an decimal value onto the object.
    /// </summary>
    public decimal Add(decimal value)
    {
        if (IsEmpty)
            return this.Decimal;
        else
            return (decimal)(this.Decimal + value);
    }

    /// <summary>
    /// Subtracts a decimal value from the object.
    /// </summary>
    public decimal Subtract(decimal value)
    {
      if (IsEmpty)
        return this.Decimal;
      else
        return (decimal)(this.Decimal - value);
    }

    #endregion

    #region Operators
      /// <summary>
      /// Compares two of this type of object for equality.
      /// </summary>
      /// <param name="obj1">The first object to compare</param>
      /// <param name="obj2">The second object to compare</param>
      /// <returns>Whether the object values are equal</returns>
    public static bool operator ==(SmartDecimal obj1, SmartDecimal obj2)
    {
      return obj1.Equals(obj2);
    }
      /// <summary>
      /// Checks two of this type of object for non-equality.
      /// </summary>
      /// <param name="obj1">The first object to compare</param>
      /// <param name="obj2">The second object to compare</param>
      /// <returns>Whether the two values are not equal</returns>
    public static bool operator !=(SmartDecimal obj1, SmartDecimal obj2)
    {
      return !obj1.Equals(obj2);
    }
      /// <summary>
      /// Compares an object of this type with an decimal for equality.
      /// </summary>
      /// <param name="obj1">The object of this type to compare</param>
      /// <param name="obj2">The decimal to compare</param>
      /// <returns>Whether the two values are equal</returns>
    public static bool operator ==(SmartDecimal obj1, decimal obj2)
    {
      return obj1.Equals(obj2);
    }
    /// <summary>
    /// Compares an object of this type with an decimal for non-equality.
    /// </summary>
    /// <param name="obj1">The object of this type to compare</param>
    /// <param name="obj2">The decimal to compare</param>
    /// <returns>Whether the two values are not equal</returns>
    public static bool operator !=(SmartDecimal obj1, decimal obj2)
    {
      return !obj1.Equals(obj2);
    }
    /// <summary>
    /// Compares an object of this type with an decimal for equality.
    /// </summary>
    /// <param name="obj1">The object of this type to compare</param>
    /// <param name="obj2">The decimal to compare</param>
    /// <returns>Whether the two values are equal</returns>
    public static bool operator ==(SmartDecimal obj1, string obj2)
    {
      return obj1.Equals(obj2);
    }
    /// <summary>
    /// Compares an object of this type with an string for non-equality.
    /// </summary>
    /// <param name="obj1">The object of this type to compare</param>
    /// <param name="obj2">The string to compare</param>
    /// <returns>Whether the two values are not equal</returns>
    public static bool operator !=(SmartDecimal obj1, string obj2)
    {
      return !obj1.Equals(obj2);
    }
      /// <summary>
      /// Adds an object of this type to an decimal.
      /// </summary>
      /// <param name="start">The object of this type to add</param>
      /// <param name="span">The decimal to add</param>
      /// <returns>A SmartDecimal with the sum</returns>
    public static SmartDecimal operator +(SmartDecimal start, decimal span)
    {
      return new SmartDecimal(start.Add(span), start.EmptyIsMin);
    }
      /// <summary>
      /// Subtracts an decimal from an object of this type.
      /// </summary>
      /// <param name="start">The object of this type to be subtracted from</param>
      /// <param name="span">The decimal to subtract</param>
      /// <returns>The calculated result</returns>
    public static SmartDecimal operator -(SmartDecimal start, decimal span)
    {
      return new SmartDecimal(start.Subtract(span), start.EmptyIsMin);
    }

    /// <summary>
    /// Subtracts an object of this type from another.
    /// </summary>
    /// <param name="start">The object of this type to be subtracted from</param>
    /// <param name="finish">The object of this type to subtract</param>
    /// <returns>The calculated result</returns>
    public static decimal operator -(SmartDecimal start, SmartDecimal finish)
    {
      return start.Subtract(finish.Decimal);
    }
    /// <summary>
    /// Determines whether the first object of this type is greater than the second.
    /// </summary>
    /// <param name="obj1">The first object of this type to compare</param>
    /// <param name="obj2">The second object of this type to compare</param>
    /// <returns>Whether the first value is greater than the second</returns>
    public static bool operator >(SmartDecimal obj1, SmartDecimal obj2)
    {
      return obj1.CompareTo(obj2) > 0;
    }
    /// <summary>
    /// Determines whether the first object of this type is less than the second.
    /// </summary>
    /// <param name="obj1">The first object of this type to compare</param>
    /// <param name="obj2">The second object of this type to compare</param>
    /// <returns>Whether the first value is less than the second</returns>
    public static bool operator <(SmartDecimal obj1, SmartDecimal obj2)
    {
      return obj1.CompareTo(obj2) < 0;
    }
      /// <summary>
      /// Determines whether the first object of this type is greater than an decimal.
      /// </summary>
      /// <param name="obj1">The object of this type to compare</param>
      /// <param name="obj2">The decimal to compare</param>
    /// <returns>Whether the first value is greater than the second</returns>
    public static bool operator >(SmartDecimal obj1, decimal obj2)
    {
      return obj1.CompareTo(obj2) > 0;
    }
    /// <summary>
    /// Determines whether the first object of this type is less than an decimal.
    /// </summary>
    /// <param name="obj1">The object of this type to compare</param>
    /// <param name="obj2">The decimal to compare</param>
    /// <returns>Whether the first value is less than the second</returns>
    public static bool operator <(SmartDecimal obj1, decimal obj2)
    {
      return obj1.CompareTo(obj2) < 0;
    }
    /// <summary>
    /// Determines whether the first object of this type is less than the value in a string.
    /// </summary>
    /// <param name="obj1">The object of this type to compare</param>
    /// <param name="obj2">The string value to compare</param>
    /// <returns>Whether the first value is greater than the second</returns>
    public static bool operator >(SmartDecimal obj1, string obj2)
    {
      return obj1.CompareTo(obj2) > 0;
    }
      /// <summary>
      /// Determines whether the first object of this type is less than the value in a string.
      /// </summary>
      /// <param name="obj1">The object of this type to compare</param>
      /// <param name="obj2">The string value to compare</param>
    /// <returns>Whether the first value is less than the second</returns>
    public static bool operator <(SmartDecimal obj1, string obj2)
    {
      return obj1.CompareTo(obj2) < 0;
    }
      /// <summary>
      /// Determines whether the first object of this type is greater than or equal to the second.
      /// </summary>
      /// <param name="obj1">The first object of this type to compare</param>
      /// <param name="obj2">The second object of this type to compare</param>
    /// <returns>Whether the first value is greater than or equal to the second</returns>
    public static bool operator >=(SmartDecimal obj1, SmartDecimal obj2)
    {
      return obj1.CompareTo(obj2) >= 0;
    }

    /// <summary>
    /// Determines whether the first object of this type is less than or equal to the second.
    /// </summary>
    /// <param name="obj1">The first object of this type to compare</param>
    /// <param name="obj2">The second object of this type to compare</param>
    /// <returns>Whether the first value is less than or equal to the second</returns>
    public static bool operator <=(SmartDecimal obj1, SmartDecimal obj2)
    {
      return obj1.CompareTo(obj2) <= 0;
    }
    /// <summary>
    /// Determines whether the first object of this type is greater than or equal to an decimal.
    /// </summary>
    /// <param name="obj1">The object of this type to compare</param>
    /// <param name="obj2">The decimal to compare</param>
    /// <returns>Whether the first value is greater than or equal to the second</returns>
    public static bool operator >=(SmartDecimal obj1, decimal obj2)
    {
      return obj1.CompareTo(obj2) >= 0;
    }
    /// <summary>
    /// Determines whether the first object of this type is less than or equal to an decimal.
    /// </summary>
    /// <param name="obj1">The object of this type to compare</param>
    /// <param name="obj2">The decimal to compare</param>
    /// <returns>Whether the first value is less than or equal to the second</returns>
    public static bool operator <=(SmartDecimal obj1, decimal obj2)
    {
      return obj1.CompareTo(obj2) <= 0;
    }
    /// <summary>
    /// Determines whether the first object of this type is greater than or equal to the value of a string.
    /// </summary>
    /// <param name="obj1">The object of this type to compare</param>
    /// <param name="obj2">The string value to compare</param>
    /// <returns>Whether the first value is greater than or equal to the second</returns>
    public static bool operator >=(SmartDecimal obj1, string obj2)
    {
      return obj1.CompareTo(obj2) >= 0;
    }
    /// <summary>
    /// Determines whether the first object of this type is less than or equal to the value of a string.
    /// </summary>
    /// <param name="obj1">The object of this type to compare</param>
    /// <param name="obj2">The string value to compare</param>
    /// <returns>Whether the first value is less than or equal to the second</returns>
    public static bool operator <=(SmartDecimal obj1, string obj2)
    {
      return obj1.CompareTo(obj2) <= 0;
    }

    #endregion

  }
}