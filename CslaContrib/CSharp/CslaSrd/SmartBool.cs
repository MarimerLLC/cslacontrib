using System;
using CslaSrd;
using CslaSrd.Properties;

namespace CslaSrd
{
  /// <summary>
  /// Provides an boolean data type that understands the concept
  /// of an empty value.
  /// </summary>
  /// <remarks>
  /// See Chapter 5 for a full discussion of the need for a similar 
  /// data type and the design choices behind it.  Basically, we are 
  /// using the same approach to handle booleans instead of dates.
  /// </remarks>
  [Serializable()]
  public struct SmartBool : IComparable
  {
    private const bool   _minValue = false;
    private const bool   _maxValue = true;

    private bool   _bool;
    private bool   _initialized;
    private bool   _emptyIsMax;
    private string _format;

    #region Constructors

    /// <summary>
    /// Creates a new SmartBool object.
    /// </summary>
    /// <param name="emptyIsMin">Indicates whether an empty bool is the min or max bool value.</param>
    //public SmartBool(bool emptyIsMin)
    //{
    //  _emptyIsMax = !emptyIsMin;
    //  _format = null;
    //  _initialized = false;
    //  // provide a dummy value to allow real initialization
    //  _bool = bool.MinValue;
    //  if (!_emptyIsMax)
    //    Bool = bool.MinValue;
    //  else
    //    Bool = bool.MaxValue;
    //}

    /// <summary>
    /// Creates a new SmartBool object.
    /// </summary>
    /// <remarks>
    /// The SmartBool created will use the min possible
    /// bool to represent an empty bool.
    /// </remarks>
    /// <param name="Value">The initial value of the object.</param>
    public SmartBool(bool value)
    {
      _emptyIsMax = false;
      _format = null;
      _initialized = false;
      _bool = _minValue;
      Bool = value;
    }

    /// <summary>
    /// Creates a new SmartBool object.
    /// </summary>
    /// <param name="Value">The initial value of the object.</param>
    /// <param name="EmptyIsMin">Indicates whether an empty bool is the min or max bool value.</param>
    public SmartBool(bool value, bool emptyIsMin)
    {
      _emptyIsMax = !emptyIsMin;
      _format = null;
      _initialized = false;
      _bool = _minValue;
      Bool = value;
    }

    /// <summary>
    /// Creates a new SmartBool object.
    /// </summary>
    /// <remarks>
    /// The SmartBool created will use the min possible
    /// bool to represent an empty bool.
    /// </remarks>
    /// <param name="Value">The initial value of the object (as text).</param>
    public SmartBool(string value)
    {
      _emptyIsMax = false;
      _format = null;
      _initialized = true;
      _bool = _minValue;
      this.Text = value;
    }

    /// <summary>
    /// Creates a new SmartBool object.
    /// </summary>
    /// <param name="Value">The initial value of the object (as text).</param>
    /// <param name="EmptyIsMin">Indicates whether an empty bool is the min or max bool value.</param>
    public SmartBool(string value, bool emptyIsMin)
    {
      _emptyIsMax = !emptyIsMin;
      _format = null;
      _initialized = true;
      _bool = _minValue;
      this.Text = value;
    }

    #endregion


    #region Text Support

    /// <summary>
    /// Gets or sets the format string used to format a bool
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
    /// Gets or sets the bool value.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This property can be used to set the bool value by passing a
    /// text representation of the bool. Any text bool representation
    /// that can be parsed by the .NET runtime is valid.
    /// </para><para>
    /// When the bool value is retrieved via this property, the text
    /// is formatted by using the format specified by the 
    /// <see cref="FormatString" /> property. The default is the
    /// short bool format (d).
    /// </para>
    /// </remarks>
    public string Text
    {
      get { return BoolToString(this.Bool, FormatString, !_emptyIsMax); }
      set { this.Bool = StringToBool(value, !_emptyIsMax); }
    }

    #endregion

    #region Bool Support

    /// <summary>
    /// Gets or sets the bool value.
    /// </summary>
    public bool Bool
    {
      get
      {
        if (!_initialized)
        {
          _bool = _minValue;
          _initialized = true;
        }
        return _bool;
      }
      set
      {
        _bool = value;
        _initialized = true;
      }
    }

    #endregion

    #region System.Object overrides

    /// <summary>
    /// Returns a text representation of the bool value.
    /// </summary>
    public override string ToString()
    {
      return this.Text;
    }

    /// <summary>
    /// Compares this object to another <see cref="SmartBool"/>
    /// for equality.
    /// </summary>
    public override bool Equals(object obj)
    {
      if (obj is SmartBool)
      {
        SmartBool tmp = (SmartBool)obj;
        if (this.IsEmpty && tmp.IsEmpty)
          return true;
        else
          return this.Bool.Equals(tmp.Bool);
      }
      else if (obj is bool)
        return this.Bool.Equals((bool)obj);
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
      return this.Bool.GetHashCode();
    }

    #endregion

    #region DBValue

    /// <summary>
    /// Gets a database-friendly version of the bool value.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If the SmartBool contains an empty bool, this returns <see cref="DBNull"/>.
    /// Otherwise the actual bool value is returned as type Bool.
    /// </para><para>
    /// This property is very useful when setting parameter values for
    /// a Command object, since it automatically stores null values into
    /// the database for empty bool values.
    /// </para><para>
    /// When you also use the SafeDataReader and its GetSmartBool method,
    /// you can easily read a null value from the database back into a
    /// SmartBool object so it remains considered as an empty bool value.
    /// </para>
    /// </remarks>
    public object DBValue
    {
      get
      {
        if (this.IsEmpty)
          return DBNull.Value;
        else
          return this.Bool;
      }
    }

    #endregion

    #region Empty Bools

    /// <summary>
    /// Gets a value indicating whether this object contains an empty bool.
    /// </summary>
    public bool IsEmpty
    {
      get
      {
        if (!_emptyIsMax)
          return this.Bool.Equals(_minValue);
        else
          return this.Bool.Equals(_maxValue);
      }
    }

    /// <summary>
    /// Gets a value indicating whether an empty bool is the 
    /// min or max possible bool value.
    /// </summary>
    /// <remarks>
    /// Whether an empty bool is considered to be the smallest or largest possible
    /// bool is only important for comparison operations. This allows you to
    /// compare an empty bool with a real bool and get a meaningful result.
    /// </remarks>
    public bool EmptyIsMin
    {
      get { return !_emptyIsMax; }
    }

    #endregion

    #region Conversion Functions

    /// <summary>
    /// Converts a string value into a SmartBool.
    /// </summary>
    /// <param name="value">String containing the bool value.</param>
    /// <returns>A new SmartBool containing the bool value.</returns>
    /// <remarks>
    /// EmptyIsMin will default to <see langword="true"/>.
    /// </remarks>
    public static SmartBool Parse(string value)
    {
      return new SmartBool(value);
    }

    /// <summary>
    /// Converts a string value into a SmartBool.
    /// </summary>
    /// <param name="value">String containing the bool value.</param>
    /// <param name="emptyIsMin">Indicates whether an empty bool is the min or max bool value.</param>
    /// <returns>A new SmartBool containing the bool value.</returns>
    public static SmartBool Parse(string value, bool emptyIsMin)
    {
      return new SmartBool(value, emptyIsMin);
    }

    /// <summary>
    /// Converts a text bool representation into a Bool value.
    /// </summary>
    /// <remarks>
    /// An empty string is assumed to represent an empty bool. An empty bool
    /// is returned as the MinValue of the Bool datatype.
    /// </remarks>
    /// <param name="Value">The text representation of the bool.</param>
    /// <returns>A Bool value.</returns>
    public static bool StringToBool(string value)
    {
      return StringToBool(value, true);
    }

    /// <summary>
    /// Converts a text bool representation into a Bool value.
    /// </summary>
    /// <remarks>
    /// An empty string is assumed to represent an empty bool. An empty bool
    /// is returned as the MinValue or MaxValue of the Bool datatype depending
    /// on the EmptyIsMin parameter.
    /// </remarks>
    /// <param name="Value">The text representation of the bool.</param>
    /// <param name="EmptyIsMin">Indicates whether an empty bool is the min or max bool value.</param>
    /// <returns>A Bool value.</returns>
    public static bool StringToBool(string value, bool emptyIsMin)
    {
      bool tmp;
      if (String.IsNullOrEmpty(value))
      {
          if (emptyIsMin)
              return _minValue;
          else
              return _maxValue;
      }
      else if (bool.TryParse(value, out tmp))
      {
          return tmp;
      }
      else
      {
          string lint = value.Trim().ToLower();
          throw new ArgumentException(Resources.StringToBoolException);
      }
    }

    /// <summary>
    /// Converts a bool value into a text representation.
    /// </summary>
    /// <remarks>
    /// The bool is considered empty if it matches the min value for
    /// the Bool datatype. If the bool is empty, this
    /// method returns an empty string. Otherwise it returns the bool
    /// value formatted based on the FormatString parameter.
    /// </remarks>
    /// <param name="Value">The bool value to convert.</param>
    /// <param name="FormatString">The format string used to format the bool into text.</param>
    /// <returns>Text representation of the bool value.</returns>
    public static string BoolToString(bool value, string formatString)
    {
      return BoolToString(value, formatString, true);
    }

    /// <summary>
    /// Converts a bool value into a text representation.
    /// </summary>
    /// <remarks>
    /// Whether the bool value is considered empty is determined by
    /// the EmptyIsMin parameter value. If the bool is empty, this
    /// method returns an empty string. Otherwise it returns the bool
    /// value formatted based on the FormatString parameter.
    /// </remarks>
    /// <param name="Value">The bool value to convert.</param>
    /// <param name="FormatString">The format string used to format the bool into text.</param>
    /// <param name="EmptyIsMin">Indicates whether an empty bool is the min or max bool value.</param>
    /// <returns>Text representation of the bool value.</returns>
    public static string BoolToString(
      bool value, string formatString, bool emptyIsMin)
    {
      if (emptyIsMin && value == _minValue)
        return string.Empty;
      else if (!emptyIsMin && value == _maxValue)
        return string.Empty;
      else
        return string.Format("{0:" + formatString + "}", value);
    }

    #endregion

    #region Manipulation Functions

    /// <summary>
    /// Compares one SmartBool to another.
    /// </summary>
    /// <remarks>
    /// This method works the same as the <see cref="bool.CompareTo"/> method
    /// on the Bool type, with the exception that it
    /// understands the concept of empty bool values.
    /// </remarks>
    /// <param name="Value">The bool to which we are being compared.</param>
    /// <returns>A value indicating if the comparison bool is less than, equal to or greater than this bool.</returns>
    public int CompareTo(SmartBool value)
    {
      if (this.IsEmpty && value.IsEmpty)
        return 0;
      else
        return _bool.CompareTo(value.Bool);
    }

    /// <summary>
    /// Compares one SmartBool to another.
    /// </summary>
    /// <remarks>
    /// This method works the same as the <see cref="bool.CompareTo"/> method
    /// on the Bool type, with the exception that it
    /// understands the concept of empty bool values.
    /// </remarks>
    /// <param name="obj">The bool to which we are being compared.</param>
    /// <returns>A value indicating if the comparison bool is less than, equal to or greater than this bool.</returns>
    int IComparable.CompareTo(object value)
    {
      if (value is SmartBool)
        return CompareTo((SmartBool)value);
      else
        throw new ArgumentException(Resources.ValueNotSmartBoolException);
    }

    /// <summary>
    /// Compares a SmartBool to a text bool value.
    /// </summary>
    /// <param name="value">The bool to which we are being compared.</param>
    /// <returns>A value indicating if the comparison bool is less than, equal to or greater than this bool.</returns>
    public int CompareTo(string value)
    {
      return this.Bool.CompareTo(StringToBool(value, !_emptyIsMax));
    }

    /// <summary>
    /// Compares a SmartBool to a bool value.
    /// </summary>
    /// <param name="value">The bool to which we are being compared.</param>
    /// <returns>A value indicating if the comparison bool is less than, equal to or greater than this bool.</returns>
    public int CompareTo(bool value)
    {
      return this.Bool.CompareTo(value);
    }

    #endregion

    #region Operators

    public static bool operator ==(SmartBool obj1, SmartBool obj2)
    {
      return obj1.Equals(obj2);
    }

    public static bool operator !=(SmartBool obj1, SmartBool obj2)
    {
      return !obj1.Equals(obj2);
    }

    public static bool operator ==(SmartBool obj1, bool obj2)
    {
      return obj1.Equals(obj2);
    }

    public static bool operator !=(SmartBool obj1, bool obj2)
    {
      return !obj1.Equals(obj2);
    }

    public static bool operator ==(SmartBool obj1, string obj2)
    {
      return obj1.Equals(obj2);
    }

    public static bool operator !=(SmartBool obj1, string obj2)
    {
      return !obj1.Equals(obj2);
    }

    public static bool operator >(SmartBool obj1, SmartBool obj2)
    {
      return obj1.CompareTo(obj2) > 0;
    }

    public static bool operator <(SmartBool obj1, SmartBool obj2)
    {
      return obj1.CompareTo(obj2) < 0;
    }

    public static bool operator >(SmartBool obj1, bool obj2)
    {
      return obj1.CompareTo(obj2) > 0;
    }

    public static bool operator <(SmartBool obj1, bool obj2)
    {
      return obj1.CompareTo(obj2) < 0;
    }

    public static bool operator >(SmartBool obj1, string obj2)
    {
      return obj1.CompareTo(obj2) > 0;
    }

    public static bool operator <(SmartBool obj1, string obj2)
    {
      return obj1.CompareTo(obj2) < 0;
    }

    public static bool operator >=(SmartBool obj1, SmartBool obj2)
    {
      return obj1.CompareTo(obj2) >= 0;
    }

    public static bool operator <=(SmartBool obj1, SmartBool obj2)
    {
      return obj1.CompareTo(obj2) <= 0;
    }

    public static bool operator >=(SmartBool obj1, bool obj2)
    {
      return obj1.CompareTo(obj2) >= 0;
    }

    public static bool operator <=(SmartBool obj1, bool obj2)
    {
      return obj1.CompareTo(obj2) <= 0;
    }

    public static bool operator >=(SmartBool obj1, string obj2)
    {
      return obj1.CompareTo(obj2) >= 0;
    }

    public static bool operator <=(SmartBool obj1, string obj2)
    {
      return obj1.CompareTo(obj2) <= 0;
    }

    #endregion

  }
}