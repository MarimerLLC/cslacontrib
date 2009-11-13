using System;
using Csla.Core;
using Csla.Reflection;
using Csla.Validation;
using MyCsla.Properties;

namespace MyCsla.Validation
{

  /// <summary>
  /// Add your own custom validation rules here in this class 
  /// </summary>
  public static class MyCommonRules
  {

    #region IComparable Rules 
    /// <summary>
    /// Rule ensuring that a numeric property1 is greater than property2
    /// as specified in CompareFieldsRuleArgs 
    /// </summary>
    /// <typeparam name="T">Type of the property to validate.</typeparam>
    /// <param name="target">Object containing values to validate.</param>
    /// <param name="e">Arguments variable specifying the name of the property1 and property2 to validate.</param>
    public static bool GreaterThanField<T>(T target, RuleArgs e) where T : Csla.Core.BusinessBase
    {
        var args = (DecoratedRuleArgs)e;
        var compareTo = (string) args["CompareToPropertyName"];
        var compareToFriendlyName = (string)args["CompareToFriendlyName"];

        var fi1 = MethodCaller.CallMethodIfImplemented(target, "GetPropertyInfo", args.PropertyName);
        var fi2 = MethodCaller.CallMethodIfImplemented(target, "GetPropertyInfo", compareTo);
        var pi1 = target.GetType().GetProperty(e.PropertyName);
        var pi2 = target.GetType().GetProperty(compareTo);


        var value1 =  fi1 != null
                         ? (IComparable) MethodCaller.CallMethod(target, "ReadProperty", fi1)
                         : (IComparable) pi1.GetValue(target, null);
        var value2 =  fi2 != null
                        ? (IComparable) MethodCaller.CallMethod(target, "ReadProperty", fi2)
                        : (IComparable) pi2.GetValue(target, null);

        var result = value1.CompareTo(value2);
        if (result > 0)
            return true;

        // get friendlyname for compareToProperty
        var friendlyName2 = String.IsNullOrEmpty(compareToFriendlyName)
                                ? compareTo
                                : compareToFriendlyName;

        var format = (string) args["Format"];
        string outValue;
        if (string.IsNullOrEmpty(format))
            outValue = value1.ToString();
        else
            outValue = string.Format(string.Format("{{0:{0}}}", format), value1, value2);
        e.Description = string.Format(Resources.GreaterThanRule,
                                      RuleArgs.GetPropertyName(e), outValue,
                                      friendlyName2);
        return false;
    }

    /// <summary>
    /// Rule ensuring that a numeric property1 is greater than or equal property2
    /// as specified in CompareFieldsRuleArgs 
    /// </summary>
    /// <typeparam name="T">Type of the property to validate.</typeparam>
    /// <param name="target">Object containing values to validate.</param>
    /// <param name="e">Arguments variable specifying the name of the property1 and property2 to validate.</param>
    public static bool GreaterThanOrEqualField<T>(T target, RuleArgs e) where T:Csla.Core.BusinessBase
    {
      var args = (DecoratedRuleArgs)e;
        var compareTo = (string)args["CompareToPropertyName"];
        var compareToFriendlyName = (string)args["CompareToFriendlyName"];

        var fi1 = MethodCaller.CallMethodIfImplemented(target, "GetPropertyInfo", args.PropertyName);
        var fi2 = MethodCaller.CallMethodIfImplemented(target, "GetPropertyInfo", compareTo);
        var pi1 = target.GetType().GetProperty(e.PropertyName);
        var pi2 = target.GetType().GetProperty(compareTo);


        var value1 = fi1 != null
                         ? (IComparable)MethodCaller.CallMethod(target, "ReadProperty", fi1)
                         : (IComparable)pi1.GetValue(target, null);
        var value2 = fi2 != null
                        ? (IComparable)MethodCaller.CallMethod(target, "ReadProperty", fi2)
                        : (IComparable)pi2.GetValue(target, null);


        var result = value1.CompareTo(value2);
        if (result > -1)
            return true;

        // get friendlyname for compareToProperty
        var friendlyName2 = String.IsNullOrEmpty(compareToFriendlyName)
                                ? compareTo
                                : compareToFriendlyName;

        var format = (string)args["Format"];
        string outValue;
        if (string.IsNullOrEmpty(format))
            outValue = value1.ToString();
        else
            outValue = string.Format(string.Format("{{0:{0}}}", format), value1, value2);
        e.Description = string.Format(Resources.GreaterThanOrEqualRule,
                                      RuleArgs.GetPropertyName(e), outValue,
                                      friendlyName2);
        return false;
    }

    #endregion

    #region LessThan
    /// <summary>
    /// Rule ensuring that a numeric property1 is less than property2
    /// as specified in CompareFieldsRuleArgs 
    /// </summary>
    /// <typeparam name="T">Type of the property to validate.</typeparam>
    /// <param name="target">Object containing values to validate.</param>
    /// <param name="e">Arguments variable specifying the name of the property1 and property2 to validate.</param>
    public static bool LessThanField<T>(T target, RuleArgs e) where T:Csla.Core.BusinessBase
    {
      var args = (DecoratedRuleArgs)e;
        var compareTo = (string) args["CompareToPropertyName"];
        var compareToFriendlyName = (string)args["CompareToFriendlyName"];

        var fi1 = MethodCaller.CallMethodIfImplemented(target, "GetPropertyInfo", args.PropertyName);
        var fi2 = MethodCaller.CallMethodIfImplemented(target, "GetPropertyInfo", compareTo);
        var pi1 = target.GetType().GetProperty(e.PropertyName);
        var pi2 = target.GetType().GetProperty(compareTo);


        var value1 = fi1 != null
                         ? (IComparable)MethodCaller.CallMethod(target, "ReadProperty", fi1)
                         : (IComparable)pi1.GetValue(target, null);
        var value2 = fi2 != null
                        ? (IComparable)MethodCaller.CallMethod(target, "ReadProperty", fi2)
                        : (IComparable)pi2.GetValue(target, null);


        var result = value1.CompareTo(value2);
        if (result < 0)
            return true;

        // get friendlyname for compareToProperty
        var friendlyName2 = String.IsNullOrEmpty(compareToFriendlyName)
                                ? compareTo
                                : compareToFriendlyName;

        var format = (string) args["Format"];
        string outValue;
        if (string.IsNullOrEmpty(format))
            outValue = value1.ToString();
        else
            outValue = string.Format(string.Format("{{0:{0}}}", format), value1, value2);
        e.Description = string.Format(Resources.LessThanRule,
                                      RuleArgs.GetPropertyName(e), outValue,
                                      friendlyName2);
        return false;
    }

    /// <summary>
    /// Rule ensuring that a numeric property1 is less than or equal property2
    /// as specified in CompareFieldsRuleArgs 
    /// </summary>
    /// <typeparam name="T">Type of the property to validate.</typeparam>
    /// <param name="target">Object containing values to validate.</param>
    /// <param name="e">Arguments variable specifying the name of the property1 and property2 to validate.</param>
    public static bool LessThanOrEqualField<T>(T target, RuleArgs e) where T:Csla.Core.BusinessBase
    {
        var args = (DecoratedRuleArgs)e;
        var compareTo = (string)args["CompareToPropertyName"];
        var compareToFriendlyName = (string)args["CompareToFriendlyName"];

        var fi1 = MethodCaller.CallMethodIfImplemented(target, "GetPropertyInfo", args.PropertyName);
        var fi2 = MethodCaller.CallMethodIfImplemented(target, "GetPropertyInfo", compareTo);
        var pi1 = target.GetType().GetProperty(e.PropertyName);
        var pi2 = target.GetType().GetProperty(compareTo);


        var value1 = fi1 != null
                         ? (IComparable)MethodCaller.CallMethod(target, "ReadProperty", fi1)
                         : (IComparable)pi1.GetValue(target, null);
        var value2 = fi2 != null
                        ? (IComparable)MethodCaller.CallMethod(target, "ReadProperty", fi2)
                        : (IComparable)pi2.GetValue(target, null);


        var result = value1.CompareTo(value2);
        if (result < 1)
            return true;

        // get friendlyname for compareToProperty
        var friendlyName2 = String.IsNullOrEmpty(compareToFriendlyName)
                                ? compareTo
                                : compareToFriendlyName;

        var format = (string)args["Format"];
        string outValue;
        if (string.IsNullOrEmpty(format))
            outValue = value1.ToString();
        else
            outValue = string.Format(string.Format("{{0:{0}}}", format), value1, value2);
        e.Description = string.Format(Resources.LessThanOrEqualRule,
                                      RuleArgs.GetPropertyName(e), outValue,
                                      friendlyName2);
        return false;
    }
    #endregion

    #region CompareFieldsRuleArgs

    /// <summary>
    /// Custom RuleArgs parameter object for comparison of 2 properties. 
    /// </summary>
    public class CompareFieldsRuleArgs : DecoratedRuleArgs
    {
        /// <summary>
        /// Create a new object.
        /// </summary>
        /// <param name="propertyName">Name of the property to validate.</param>
        /// <param name="compareToPropertyName">Name of the compare to property.</param>
        /// <param name="friendlyName">Name of the friendly.</param>
        public CompareFieldsRuleArgs(string propertyName, string compareToPropertyName, string friendlyName)
          : base(propertyName, friendlyName)
        {
            this["CompareToPropertyName"] = compareToPropertyName;
            this["Format"] = string.Empty;
        }

        /// <summary>
        /// Create a new object.
        /// </summary>
        /// <param name="propertyName">Name of the property to validate.</param>
        /// <param name="compareToPropertyName">Name of the compare to property.</param>
        /// <param name="friendlyName">Name of the friendly.</param>
        /// <param name="severity">The severity.</param>
        /// <param name="stopProcessing">if set to <c>true</c> [stop processing].</param>
        public CompareFieldsRuleArgs(string propertyName, string compareToPropertyName, string friendlyName, RuleSeverity severity, bool stopProcessing)
            : base(propertyName, friendlyName, severity, stopProcessing)
        {
            this["CompareToPropertyName"] = compareToPropertyName;
            this["Format"] = string.Empty;
        }

        /// <summary>
        /// Create a new object.
        /// </summary>
        /// <param name="propertyName">Name of the property to validate.</param>
        /// <param name="compareToPropertyName">Name of the compare to property.</param>
        /// <param name="friendlyName">Name of the friendly.</param>
        /// <param name="compareToFriendlyName">Name of the compare to friendly.</param>
        public CompareFieldsRuleArgs(string propertyName, string compareToPropertyName, string friendlyName, string compareToFriendlyName)
            : base(propertyName, friendlyName)
        {
            this["CompareToPropertyName"] = compareToPropertyName;
            this["CompareToFriendlyName"] = compareToFriendlyName;
            this["Format"] = string.Empty;
        }

        /// <summary>
        /// Create a new object.
        /// </summary>
        /// <param name="propertyName">Name of the property to validate.</param>
        /// <param name="compareToPropertyName">Name of the compare to property.</param>
        /// <param name="friendlyName">Name of the friendly.</param>
        /// <param name="compareToFriendlyName">Name of the compare to friendly.</param>
        /// <param name="severity">The severity.</param>
        /// <param name="stopProcessing">if set to <c>true</c> [stop processing].</param>
        public CompareFieldsRuleArgs(string propertyName, string compareToPropertyName, string friendlyName, string compareToFriendlyName, RuleSeverity severity, bool stopProcessing)
            : base(propertyName, friendlyName, severity, stopProcessing)
        {
            this["CompareToPropertyName"] = compareToPropertyName;
            this["CompareToFriendlyName"] = compareToFriendlyName;
            this["Format"] = string.Empty;
        }

        /// <summary>
        /// Create a new object.
        /// </summary>
        /// <param name="propertyName">Name of the property to validate.</param>
        /// <param name="compareToPropertyName">Name of the compare to property.</param>
        /// <param name="friendlyName">Name of the friendly.</param>
        /// <param name="compareToFriendlyName">Name of the compare to friendly.</param>
        /// <param name="format">The format.</param>
        public CompareFieldsRuleArgs(string propertyName, string compareToPropertyName, string friendlyName, string compareToFriendlyName, string format)
            : base(propertyName, friendlyName)
        {
            this["CompareToPropertyName"] = compareToPropertyName;
            this["CompareToFriendlyName"] = compareToFriendlyName;
            this["Format"] = format;
        }


        /// <summary>
        /// Create a new object.
        /// </summary>
        /// <param name="propertyName">Name of the property to validate.</param>
        /// <param name="compareToPropertyName">Name of the compare to property.</param>
        public CompareFieldsRuleArgs(string propertyName, string compareToPropertyName)
          : base(propertyName)
        {
            this["CompareToPropertyName"] = compareToPropertyName;
            this["Format"] = string.Empty;
        }

        /// <summary>
        /// Create a new object.
        /// </summary>
        /// <param name="propertyName">Name of the property to validate.</param>
        /// <param name="compareToPropertyName">Name of the compare to property.</param>
        /// <param name="severity">The severity.</param>
        /// <param name="stopProcessing">if set to <c>true</c> [stop processing].</param>
        public CompareFieldsRuleArgs(string propertyName, string compareToPropertyName, RuleSeverity severity, bool stopProcessing)
            : base(propertyName, severity, stopProcessing)
        {
            this["CompareToPropertyName"] = compareToPropertyName;
            this["Format"] = string.Empty;
        }
    }

    #endregion

    #region StopIfNotCanRead

    /// <summary>
    /// Rule indicating whether the user is authorized
    /// to read the property value.
    /// Will always be silent and never set rule to broken.
    /// </summary>
    /// <param name="target">Target object.</param>
    /// <param name="e">Rule arguments.</param>
    /// <returns>true</returns>
    /// <remarks>
    /// Combine this property with short-circuiting to
    /// prevent evaluation of other rules in the case
    /// that the user isn't allowed to read the value.
    /// </remarks>
    public static bool StopIfNotCanRead(object target, RuleArgs e)
    {
      bool isAuthorized = true;

      var business = target as BusinessBase;
      if (business != null && !string.IsNullOrEmpty(e.PropertyName))
        isAuthorized = business.CanReadProperty(e.PropertyName);

      if (!isAuthorized)
      {
        e.StopProcessing = true;
      }

      // rule should always return true but may stop execution
      return true;
    }

    #endregion

    #region StopIfNotCanWrite

    /// <summary>
    /// Rule indicating whether the user is authorized
    /// to change the property value.
    /// Will always be silent and never set rule to broken.
    /// </summary>
    /// <param name="target">Target object.</param>
    /// <param name="e">Rule arguments.</param>
    /// <returns>true</returns>
    /// <remarks>
    /// Combine this property with short-circuiting to
    /// prevent evaluation of other rules in the case
    /// that the user isn't allowed to change the value.
    /// </remarks>
    public static bool StopIfNotCanWrite(object target, RuleArgs e)
    {
      bool isAuthorized = true;

      var business = target as BusinessBase;
      if (business != null && !string.IsNullOrEmpty(e.PropertyName))
        isAuthorized = business.CanWriteProperty(e.PropertyName);

      if (!isAuthorized)
      {
        e.StopProcessing = true;
      }

      // rule should always return true (be silent) but will stop processing
      // if user cannot modify the value.
      return true;
    }
    #endregion

    #region Modulus11

    private static bool IsModulus11(string numberString, string vektTall)
    {
      try
      {
        // var vektTall = new int[] { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };
        // var vektTall = new int[] { 3, 7, 6, 1, 8, 9, 4, 5, 2 };
        int sum = 0, rest = 0;
        for (var i = 0; i < numberString.Length - 1; i++)
        {
          sum += Convert.ToInt32(vektTall.Substring(i, 1)) * Convert.ToInt32(numberString.Substring(i, 1));
        }

        rest = 11 - (sum % 11);
        if (rest == 10) return false;
        if (rest == 11) rest = 0;

        var chkDigit = Convert.ToInt32(numberString.Substring(numberString.Length - 1, 1));
        return chkDigit == rest;
      }
      catch
      {
        return false;
      }
    }

    #endregion 
  }
}