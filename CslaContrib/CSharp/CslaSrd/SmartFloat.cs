using System;
using CslaSrd;
using CslaSrd.Properties;

namespace CslaSrd
{
  /// <summary>
  /// Provides an float data type that understands the concept
  /// of an empty value.
  /// </summary>
  /// <remarks>
  /// See Chapter 5 for a full discussion of the need for a similar 
  /// data type and the design choices behind it.  Basically, we are 
  /// using the same approach to handle floats instead of dates.
  /// </remarks>
  [Serializable()]
  public struct SmartFloat : IComparable
  {
    private float  _float;
    private bool   _initialized;
    private bool   _emptyIsMax;
    private string _format;

    #region Constructors

    /// <summary>
    /// Creates a new SmartFloat object.
    /// </summary>
    /// <param name="emptyIsMin">Indicates whether an empty float is the min or max float value.</param>
    public SmartFloat(bool emptyIsMin)
    {
      _emptyIsMax  = !emptyIsMin;
      _format      = null;
      _initialized = false;
      // provide a dummy value to allow real initialization
      _float         = float.MinValue;
      if (!_emptyIsMax)
        Float = float.MinValue;
      else
        Float = float.MaxValue;
    }

    /// <summary>
    /// Creates a new SmartFloat object.
    /// </summary>
    /// <remarks>
    /// The SmartFloat created will use the min possible
    /// float to represent an empty float.
    /// </remarks>
    /// <param name="value">The initial value of the object.</param>
    public SmartFloat(float value)
    {
      _emptyIsMax = false;
      _format = null;
      _initialized = false;
      _float = float.MinValue;
      Float = value;
    }

    /// <summary>
    /// Creates a new SmartFloat object.
    /// </summary>
    /// <param name="value">The initial value of the object.</param>
    /// <param name="emptyIsMin">Indicates whether an empty float is the min or max float value.</param>
    public SmartFloat(float value, bool emptyIsMin)
    {
      _emptyIsMax = !emptyIsMin;
      _format = null;
      _initialized = false;
      _float = float.MinValue;
      Float = value;
    }

    /// <summary>
    /// Creates a new SmartFloat object.
    /// </summary>
    /// <remarks>
    /// The SmartFloat created will use the min possible
    /// float to represent an empty float.
    /// </remarks>
    /// <param name="value">The initial value of the object (as text).</param>
    public SmartFloat(string value)
    {
      _emptyIsMax = false;
      _format = null;
      _initialized = true;
      _float = float.MinValue;
      this.Text = value;
    }

    /// <summary>
    /// Creates a new SmartFloat object.
    /// </summary>
    /// <param name="value">The initial value of the object (as text).</param>
    /// <param name="emptyIsMin">Indicates whether an empty float is the min or max float value.</param>
    public SmartFloat(string value, bool emptyIsMin)
    {
      _emptyIsMax = !emptyIsMin;
      _format = null;
      _initialized = true;
      _float = float.MinValue;
      this.Text = value;
    }

    #endregion


    #region Text Support

    /// <summary>
    /// Gets or sets the format string used to format a float
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
    /// Gets or sets the float value.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property can be used to set the float value by passing a
    /// text representation of the float. Any text float representation
    /// that can be parsed by the .NET runtime is valid.
    /// </para><para>
    /// When the float value is retrieved via this property, the text
    /// is formatted by using the format specified by the 
    /// <see cref="FormatString" /> property. The default is the
    /// short float format (d).
    /// </para>
    /// </remarks>
    public string Text
    {
      get { return FloatToString(this.Float, FormatString, !_emptyIsMax); }
      set { this.Float = StringToFloat(value, !_emptyIsMax); }
    }

    #endregion

    #region Float Support

    /// <summary>
    /// Gets or sets the float value.
    /// </summary>
    public float Float
    {
      get
      {
        if (!_initialized)
        {
          _float = float.MinValue;
          _initialized = true;
        }
        return _float;
      }
      set
      {
        _float = value;
        _initialized = true;
      }
    }

    #endregion

    #region System.Object overrides

    /// <summary>
    /// Returns a text representation of the float value.
    /// </summary>
    public override string ToString()
    {
      return this.Text;
    }

    /// <summary>
    /// Compares this object to another <see cref="SmartFloat"/>
    /// for equality.
    /// </summary>
    public override bool Equals(object obj)
    {
      if (obj is SmartFloat)
      {
        SmartFloat tmp = (SmartFloat)obj;
        if (this.IsEmpty && tmp.IsEmpty)
          return true;
        else
          return this.Float.Equals(tmp.Float);
      }
      else if (obj is float)
        return this.Float.Equals((float)obj);
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
      return this.Float.GetHashCode();
    }

    #endregion

    #region DBValue

    /// <summary>
    /// Gets a database-friendly version of the float value.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If the SmartFloat contains an empty float, this returns <see cref="DBNull"/>.
    /// Otherwise the actual float value is returned as type Float.
    /// </para><para>
    /// This property is very useful when setting parameter values for
    /// a Command object, since it automatically stores null values into
    /// the database for empty float values.
    /// </para><para>
    /// When you also use the SafeDataReader and its GetSmartFloat method,
    /// you can easily read a null value from the database back into a
    /// SmartFloat object so it remains considered as an empty float value.
    /// </para>
    /// </remarks>
    public object DBValue
    {
      get
      {
        if (this.IsEmpty)
          return DBNull.Value;
        else
          return this.Float;
      }
    }

    #endregion

    #region Empty Floats

    /// <summary>
    /// Gets a value indicating whether this object contains an empty float.
    /// </summary>
    public bool IsEmpty
    {
      get
      {
        if (!_emptyIsMax)
          return this.Float.Equals(float.MinValue);
        else
          return this.Float.Equals(float.MaxValue);
      }
    }

    /// <summary>
    /// Gets a value indicating whether an empty float is the 
    /// min or max possible float value.
    /// </summary>
    /// <remarks>
    /// Whether an empty float is considered to be the smallest or largest possible
    /// float is only important for comparison operations. This allows you to
    /// compare an empty float with a real float and get a meaningful result.
    /// </remarks>
    public bool EmptyIsMin
    {
      get { return !_emptyIsMax; }
    }

    #endregion

    #region Conversion Functions

    /// <summary>
    /// Converts a string value into a SmartFloat.
    /// </summary>
    /// <param name="value">String containing the float value.</param>
    /// <returns>A new SmartFloat containing the float value.</returns>
    /// <remarks>
    /// EmptyIsMin will default to <see langword="true"/>.
    /// </remarks>
    public static SmartFloat Parse(string value)
    {
      return new SmartFloat(value);
    }

    /// <summary>
    /// Converts a string value into a SmartFloat.
    /// </summary>
    /// <param name="value">String containing the float value.</param>
    /// <param name="emptyIsMin">Indicates whether an empty float is the min or max float value.</param>
    /// <returns>A new SmartFloat containing the float value.</returns>
    public static SmartFloat Parse(string value, bool emptyIsMin)
    {
      return new SmartFloat(value, emptyIsMin);
    }

    /// <summary>
    /// Converts a text float representation into a Float value.
    /// </summary>
    /// <remarks>
    /// An empty string is assumed to represent an empty float. An empty float
    /// is returned as the MinValue of the Float datatype.
    /// </remarks>
    /// <param name="value">The text representation of the float.</param>
    /// <returns>A Float value.</returns>
    public static float StringToFloat(string value)
    {
      return StringToFloat(value, true);
    }

    /// <summary>
    /// Converts a text float representation into a Float value.
    /// </summary>
    /// <remarks>
    /// An empty string is assumed to represent an empty float. An empty float
    /// is returned as the MinValue or MaxValue of the Float datatype depending
    /// on the EmptyIsMin parameter.
    /// </remarks>
    /// <param name="value">The text representation of the float.</param>
    /// <param name="emptyIsMin">Indicates whether an empty float is the min or max float value.</param>
    /// <returns>A Float value.</returns>
    public static float StringToFloat(string value, bool emptyIsMin)
    {
      float tmp;
      if (String.IsNullOrEmpty(value))
      {
        if (emptyIsMin)
          return float.MinValue;
        else
          return float.MaxValue;
      }
      else if (float.TryParse(value, out tmp))
        return tmp; 
      else
      {
        string lint = value.Trim().ToLower();
        throw new ArgumentException(Resources.StringToFloatException);
      }
    }

    /// <summary>
    /// Converts a float value into a text representation.
    /// </summary>
    /// <remarks>
    /// The float is considered empty if it matches the min value for
    /// the Float datatype. If the float is empty, this
    /// method returns an empty string. Otherwise it returns the float
    /// value formatted based on the FormatString parameter.
    /// </remarks>
    /// <param name="value">The float value to convert.</param>
    /// <param name="formatString">The format string used to format the float into text.</param>
    /// <returns>Text representation of the float value.</returns>
    public static string FloatToString(float value, string formatString)
    {
      return FloatToString(value, formatString, true);
    }

    /// <summary>
    /// Converts a float value into a text representation.
    /// </summary>
    /// <remarks>
    /// Whether the float value is considered empty is determined by
    /// the EmptyIsMin parameter value. If the float is empty, this
    /// method returns an empty string. Otherwise it returns the float
    /// value formatted based on the FormatString parameter.
    /// </remarks>
    /// <param name="value">The float value to convert.</param>
    /// <param name="formatString">The format string used to format the float into text.</param>
    /// <param name="emptyIsMin">Indicates whether an empty float is the min or max float value.</param>
    /// <returns>Text representation of the float value.</returns>
    public static string FloatToString(
      float value, string formatString, bool emptyIsMin)
    {
      if (emptyIsMin && value == float.MinValue)
        return string.Empty;
      else if (!emptyIsMin && value == float.MaxValue)
        return string.Empty;
      else
        return string.Format("{0:" + formatString + "}", value);
    }

    #endregion

    #region Manipulation Functions

    /// <summary>
    /// Compares one SmartFloat to another.
    /// </summary>
    /// <remarks>
    /// This method works the same as the <see cref="int.CompareTo"/> method
    /// on the Float inttype, with the exception that it
    /// understands the concept of empty int values.
    /// </remarks>
    /// <param name="value">The int to which we are being compared.</param>
    /// <returns>A value indicating if the comparison int is less than, equal to or greater than this int.</returns>
    public int CompareTo(SmartFloat value)
    {
      if (this.IsEmpty && value.IsEmpty)
        return 0;
      else
        return _float.CompareTo(value.Float);
    }

    /// <summary>
    /// Compares one SmartFloat to another.
    /// </summary>
    /// <remarks>
    /// This method works the same as the <see cref="int.CompareTo"/> method
    /// on the float type, with the exception that it
    /// understands the concept of empty float values.
    /// </remarks>
    /// <param name="value">The int to which we are being compared.</param>
    /// <returns>A value indicating if the comparison int is less than, equal to or greater than this int.</returns>
    int IComparable.CompareTo(object value)
    {
      if (value is SmartFloat)
        return CompareTo((SmartFloat)value);
      else
        throw new ArgumentException(Resources.ValueNotSmartFloatException);
    }

    /// <summary>
    /// Compares a SmartFloat to a text int value.
    /// </summary>
    /// <param name="value">The int to which we are being compared.</param>
    /// <returns>A value indicating if the comparison int is less than, equal to or greater than this int.</returns>
    public int CompareTo(string value)
    {
      return this.Float.CompareTo(StringToFloat(value, !_emptyIsMax));
    }

    /// <summary>
    /// Compares a SmartFloat to a int value.
    /// </summary>
    /// <param name="value">The int to which we are being compared.</param>
    /// <returns>A value indicating if the comparison int is less than, equal to or greater than this int.</returns>
    public int CompareTo(float value)
    {
      return this.Float.CompareTo(value);
    }

    /// <summary>
    /// Adds an float value onto the object.
    /// </summary>
    public float Add(float value)
    {
        if (IsEmpty)
            return this.Float;
        else
            return (float)(this.Float + value);
    }

    /// <summary>
    /// Subtracts a float value from the object.
    /// </summary>
    public float Subtract(float value)
    {
      if (IsEmpty)
        return this.Float;
      else
        return (float)(this.Float - value);
    }

    #endregion

    #region Operators
      /// <summary>
      /// Compares two of this type of object for equality.
      /// </summary>
      /// <param name="obj1">The first object to compare</param>
      /// <param name="obj2">The second object to compare</param>
      /// <returns>Whether the object values are equal</returns>
    public static bool operator ==(SmartFloat obj1, SmartFloat obj2)
    {
      return obj1.Equals(obj2);
    }
      /// <summary>
      /// Checks two of this type of object for non-equality.
      /// </summary>
      /// <param name="obj1">The first object to compare</param>
      /// <param name="obj2">The second object to compare</param>
      /// <returns>Whether the two values are not equal</returns>
    public static bool operator !=(SmartFloat obj1, SmartFloat obj2)
    {
      return !obj1.Equals(obj2);
    }
      /// <summary>
      /// Compares an object of this type with an float for equality.
      /// </summary>
      /// <param name="obj1">The object of this type to compare</param>
      /// <param name="obj2">The float to compare</param>
      /// <returns>Whether the two values are equal</returns>
    public static bool operator ==(SmartFloat obj1, float obj2)
    {
      return obj1.Equals(obj2);
    }
    /// <summary>
    /// Compares an object of this type with an float for non-equality.
    /// </summary>
    /// <param name="obj1">The object of this type to compare</param>
    /// <param name="obj2">The float to compare</param>
    /// <returns>Whether the two values are not equal</returns>
    public static bool operator !=(SmartFloat obj1, float obj2)
    {
      return !obj1.Equals(obj2);
    }
    /// <summary>
    /// Compares an object of this type with an float for equality.
    /// </summary>
    /// <param name="obj1">The object of this type to compare</param>
    /// <param name="obj2">The float to compare</param>
    /// <returns>Whether the two values are equal</returns>
    public static bool operator ==(SmartFloat obj1, string obj2)
    {
      return obj1.Equals(obj2);
    }
    /// <summary>
    /// Compares an object of this type with an string for non-equality.
    /// </summary>
    /// <param name="obj1">The object of this type to compare</param>
    /// <param name="obj2">The string to compare</param>
    /// <returns>Whether the two values are not equal</returns>
    public static bool operator !=(SmartFloat obj1, string obj2)
    {
      return !obj1.Equals(obj2);
    }
      /// <summary>
      /// Adds an object of this type to an float.
      /// </summary>
      /// <param name="start">The object of this type to add</param>
      /// <param name="span">The float to add</param>
      /// <returns>A SmartFloat with the sum</returns>
    public static SmartFloat operator +(SmartFloat start, float span)
    {
      return new SmartFloat(start.Add(span), start.EmptyIsMin);
    }
      /// <summary>
      /// Subtracts an float from an object of this type.
      /// </summary>
      /// <param name="start">The object of this type to be subtracted from</param>
      /// <param name="span">The float to subtract</param>
      /// <returns>The calculated result</returns>
    public static SmartFloat operator -(SmartFloat start, float span)
    {
      return new SmartFloat(start.Subtract(span), start.EmptyIsMin);
    }

    /// <summary>
    /// Subtracts an object of this type from another.
    /// </summary>
    /// <param name="start">The object of this type to be subtracted from</param>
    /// <param name="finish">The object of this type to subtract</param>
    /// <returns>The calculated result</returns>
    public static float operator -(SmartFloat start, SmartFloat finish)
    {
      return start.Subtract(finish.Float);
    }
    /// <summary>
    /// Determines whether the first object of this type is greater than the second.
    /// </summary>
    /// <param name="obj1">The first object of this type to compare</param>
    /// <param name="obj2">The second object of this type to compare</param>
    /// <returns>Whether the first value is greater than the second</returns>
    public static bool operator >(SmartFloat obj1, SmartFloat obj2)
    {
      return obj1.CompareTo(obj2) > 0;
    }
    /// <summary>
    /// Determines whether the first object of this type is less than the second.
    /// </summary>
    /// <param name="obj1">The first object of this type to compare</param>
    /// <param name="obj2">The second object of this type to compare</param>
    /// <returns>Whether the first value is less than the second</returns>
    public static bool operator <(SmartFloat obj1, SmartFloat obj2)
    {
      return obj1.CompareTo(obj2) < 0;
    }
      /// <summary>
      /// Determines whether the first object of this type is greater than an float.
      /// </summary>
      /// <param name="obj1">The object of this type to compare</param>
      /// <param name="obj2">The float to compare</param>
    /// <returns>Whether the first value is greater than the second</returns>
    public static bool operator >(SmartFloat obj1, float obj2)
    {
      return obj1.CompareTo(obj2) > 0;
    }
    /// <summary>
    /// Determines whether the first object of this type is less than an float.
    /// </summary>
    /// <param name="obj1">The object of this type to compare</param>
    /// <param name="obj2">The float to compare</param>
    /// <returns>Whether the first value is less than the second</returns>
    public static bool operator <(SmartFloat obj1, float obj2)
    {
      return obj1.CompareTo(obj2) < 0;
    }
    /// <summary>
    /// Determines whether the first object of this type is less than the value in a string.
    /// </summary>
    /// <param name="obj1">The object of this type to compare</param>
    /// <param name="obj2">The string value to compare</param>
    /// <returns>Whether the first value is greater than the second</returns>
    public static bool operator >(SmartFloat obj1, string obj2)
    {
      return obj1.CompareTo(obj2) > 0;
    }
      /// <summary>
      /// Determines whether the first object of this type is less than the value in a string.
      /// </summary>
      /// <param name="obj1">The object of this type to compare</param>
      /// <param name="obj2">The string value to compare</param>
    /// <returns>Whether the first value is less than the second</returns>
    public static bool operator <(SmartFloat obj1, string obj2)
    {
      return obj1.CompareTo(obj2) < 0;
    }
      /// <summary>
      /// Determines whether the first object of this type is greater than or equal to the second.
      /// </summary>
      /// <param name="obj1">The first object of this type to compare</param>
      /// <param name="obj2">The second object of this type to compare</param>
    /// <returns>Whether the first value is greater than or equal to the second</returns>
    public static bool operator >=(SmartFloat obj1, SmartFloat obj2)
    {
      return obj1.CompareTo(obj2) >= 0;
    }

    /// <summary>
    /// Determines whether the first object of this type is less than or equal to the second.
    /// </summary>
    /// <param name="obj1">The first object of this type to compare</param>
    /// <param name="obj2">The second object of this type to compare</param>
    /// <returns>Whether the first value is less than or equal to the second</returns>
    public static bool operator <=(SmartFloat obj1, SmartFloat obj2)
    {
      return obj1.CompareTo(obj2) <= 0;
    }
    /// <summary>
    /// Determines whether the first object of this type is greater than or equal to an float.
    /// </summary>
    /// <param name="obj1">The object of this type to compare</param>
    /// <param name="obj2">The float to compare</param>
    /// <returns>Whether the first value is greater than or equal to the second</returns>
    public static bool operator >=(SmartFloat obj1, float obj2)
    {
      return obj1.CompareTo(obj2) >= 0;
    }
    /// <summary>
    /// Determines whether the first object of this type is less than or equal to an float.
    /// </summary>
    /// <param name="obj1">The object of this type to compare</param>
    /// <param name="obj2">The float to compare</param>
    /// <returns>Whether the first value is less than or equal to the second</returns>
    public static bool operator <=(SmartFloat obj1, float obj2)
    {
      return obj1.CompareTo(obj2) <= 0;
    }
    /// <summary>
    /// Determines whether the first object of this type is greater than or equal to the value of a string.
    /// </summary>
    /// <param name="obj1">The object of this type to compare</param>
    /// <param name="obj2">The string value to compare</param>
    /// <returns>Whether the first value is greater than or equal to the second</returns>
    public static bool operator >=(SmartFloat obj1, string obj2)
    {
      return obj1.CompareTo(obj2) >= 0;
    }
    /// <summary>
    /// Determines whether the first object of this type is less than or equal to the value of a string.
    /// </summary>
    /// <param name="obj1">The object of this type to compare</param>
    /// <param name="obj2">The string value to compare</param>
    /// <returns>Whether the first value is less than or equal to the second</returns>
    public static bool operator <=(SmartFloat obj1, string obj2)
    {
      return obj1.CompareTo(obj2) <= 0;
    }

    #endregion

  }
}