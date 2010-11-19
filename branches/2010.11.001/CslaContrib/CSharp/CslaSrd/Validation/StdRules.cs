using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Csla;
using Csla.Validation;
using CslaSrd.Properties;
using System.Text.RegularExpressions;
using System.Reflection;

namespace CslaSrd.Validation
{

    /// <summary>
    /// Implements common business rules.
    /// </summary>
    public static class StdRules
    {
        #region AOrBValue

        /// <summary>
        /// Implements a rule that only allows one field out of the specified set of fields to have a non-null value.
        /// Example: A gentleman can record his girlfriend's name, or his wife's name, but not both.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static bool AOrBValue(object target, RuleArgs e)
        {
            AOrBValueRuleArgs ee = (AOrBValueRuleArgs)e;
            int nonBlankCount = 0;

            for (int i = 0; i < ee.PropertyList.Count; i++)
            {
                // Get object type of property.
                // Check the value assigned to each property in a manner appropriate to its object type, and count
                //   how many have a non-blank value.
                Object o = Utilities.CallByName(target, ee.PropertyList[i].ToString(), CallType.Get);
                Type t = o.GetType();
                // string
                if (t.FullName == "System.String")
                {
                    String s = (String)o;
                    if (!(s == null
                          || s == String.Empty
                         )
                       )
                    {
                        nonBlankCount++;
                    }
                }
                // char
                else if (t.FullName == "System.Char")
                {
                    Char c = (Char)o;
                    if (!(c == ' '
                         )
                       )
                    {
                        nonBlankCount++;
                    }
                }
                // SmartDate
                else if (t.FullName == "Csla.SmartDate")
                {
                    SmartDate d = (SmartDate)o;
                    if (!(d == null
                          || d.IsEmpty
                         )
                       )
                    {
                        nonBlankCount++;
                    }
                }
                // SmartField
                else if (t.FullName == "CslaSrd.SmartInt16"
                         || t.FullName == "CslaSrd.SmartInt32"
                         || t.FullName == "CslaSrd.SmartInt64"
                         || t.FullName == "CslaSrd.SmartFloat"
                         || t.FullName == "CslaSrd.SmartDecimal"
                         || t.FullName == "CslaSrd.SmartBool"
                        )
                {
                    ISmartField f = (ISmartField)o;
                    if (!(f == null
                          || f.HasNullValue()
                         )
                       )
                    {
                        nonBlankCount++;
                    }

                }
                // other - error.
                else
                {
                }

            }
            if (nonBlankCount > 1)
            {
                String csvPropertyList = String.Empty;
                for (int i = 0; i < ee.PropertyList.Count; i++)
                {
                    csvPropertyList += "," + ee.PropertyList[i].ToString();
                }
                if (csvPropertyList.Length > 0)
                {
                    csvPropertyList = csvPropertyList.Substring(1);
                }

                ee.Description = Resources.ruleAOrBValue.Replace("{propertyList}", csvPropertyList);
                return false;
            }
            else
            {
                return true;
            }

        }


        /// <summary>
        /// Custom <see cref="RuleArgs" /> object required by the
        /// <see cref="AOrB" /> rule method.
        /// </summary>
        public class AOrBValueRuleArgs : RuleArgs
        {
            private ArrayList _propertyList = new ArrayList();

            /// <summary>
            /// Get the names of the properties that are mutually exclusive with this one.
            /// </summary>
            public ArrayList PropertyList
            {
                get { return _propertyList; }
            }

            /// <summary>
            /// Create a new custom rule arguments object.
            /// </summary>
            /// <param name="propertyName">The name of the property the rule is attached to.</param>
            /// <param name="propertyList">A list of other properties affected by the rule.</param>
            public AOrBValueRuleArgs(
              string propertyName, ArrayList propertyList)
                : base(propertyName)
            {
                _propertyList = propertyList;
            }

            /// <summary>
            /// Return a string representation of the object.
            /// </summary>
            public override string ToString()
            {
                String csvPropertyList = String.Empty;
                for (int i = 0; i < _propertyList.Count; i++)
                {
                    csvPropertyList += "," + _propertyList[i].ToString();
                }
                if (csvPropertyList.Length > 0)
                {
                    csvPropertyList = csvPropertyList.Substring(1);
                }
                return base.ToString() + "?propertyList=" + csvPropertyList;
            }
        }
        #endregion

        #region BIfAValue

        /// <summary>
        /// Implements a rule that only allows field B to have a value if the field A has one.
        /// Example: Address Line 2 cannot be filled in unless Address Line 1 is filled in.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        //public static bool BIfAValue<T>(object target, RuleArgs e)
        //{@TODO

        //}

        #endregion

        #region AAndBValue

        /// <summary>
        /// Implements a rule that requires all specified fields to have a non-null value if any one of them does.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        //public static bool AAndBValue<T>(object target, RuleArgs e)
        //{@TODO

        //}

        #endregion

        #region SmartFieldRequired

        /// <summary>
        /// Rule ensuring a Smart... class value contains a non-null value.
        /// </summary>
        /// <param name="target">Object containing the data to validate</param>
        /// <param name="e">Arguments parameter specifying the name of the SmartField
        /// property to validate</param>
        /// <returns><see langword="false" /> if the rule is broken</returns>
        /// <remarks>
        /// This implementation uses late binding, and will only work
        /// against Smart... class property values.  This will not work on the SmartDate class, as it
        /// does not implement the ISmartField interface.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static bool SmartFieldRequired(object target, RuleArgs e)
        {
            ISmartField value = (ISmartField)Utilities.CallByName(target, e.PropertyName, CallType.Get);
            if (value.HasNullValue() == true)
            {
                e.Description = Resources.ruleSmartFieldRequired.Replace("{rulePropertyName}", e.PropertyName);
                return false;
            }
            return true;
        }
        #endregion

        #region SmartDateRequired

        /// <summary>
        /// Rule ensuring a SmartDate class value contains a non-null value.
        /// </summary>
        /// <param name="target">Object containing the data to validate</param>
        /// <param name="e">Arguments parameter specifying the name of the SmartField
        /// property to validate</param>
        /// <returns><see langword="false" /> if the rule is broken</returns>
        /// <remarks>
        /// This implementation uses late binding, and will only work
        /// against SmartDate class property values.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static bool SmartDateRequired(object target, RuleArgs e)
        {
            SmartDate value = (SmartDate)Utilities.CallByName(target, e.PropertyName, CallType.Get);
            if (value.IsEmpty == true)
            {
                e.Description = Resources.ruleSmartDateRequired.Replace("{rulePropertyName}", e.PropertyName);
                return false;
            }
            return true;
        }
        #endregion

        #region MinMax
        /// <summary>
        /// The first field must not be greater than the second field.  Example: StartDate must not be greater than EndDate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static bool MinMax<T>(object target, RuleArgs e) where T : IComparable
        {
            MinMaxRuleArgs ee = (MinMaxRuleArgs)e;
            T minValue = (T)Utilities.CallByName(target, ee.MinPropertyName, CallType.Get);
            T maxValue = (T)Utilities.CallByName(target, ee.MaxPropertyName, CallType.Get);

            if (minValue.CompareTo(maxValue) == 1)
            {
                try
                {
                    ee.Description = Resources.ruleMinMax.Replace("{rulePropertyName}", ee.PropertyName);
                    ee.Description = ee.Description.Replace("{minPropertyName}", ee.MinPropertyName);
                    ee.Description = ee.Description.Replace("{maxPropertyName}", ee.MaxPropertyName);
                }
                catch
                {
                    ee.Description = Resources.ruleMinMax.ToString();
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Custom <see cref="RuleArgs" /> object required by the
        /// <see cref="MinMax" /> rule method.
        /// </summary>
        public class MinMaxRuleArgs : RuleArgs
        {
            private string _minPropertyName;
            private string _maxPropertyName;

            /// <summary>
            /// Get the name of the property that holds the min value.
            /// </summary>
            public string MinPropertyName
            {
                get { return _minPropertyName; }
            }

            /// <summary>
            /// Get the name of the property that holds the max value.
            /// </summary>
            public string MaxPropertyName
            {
                get { return _maxPropertyName; }
            }


            /// <summary>
            /// Create a new object.
            /// </summary>
            /// <param name="propertyName">Name of the property to validate.</param>
            /// <param name="minPropertyName">Name of property containing min value to be checked.</param>
            /// <param name="maxPropertyName">Name of property containing max value to be checked.</param>
            public MinMaxRuleArgs(
              string propertyName, string minPropertyName, string maxPropertyName)
                : base(propertyName)
            {
                _minPropertyName = minPropertyName;
                _maxPropertyName = maxPropertyName;
            }

            /// <summary>
            /// Return a string representation of the object.
            /// </summary>
            public override string ToString()
            {
                return base.ToString() + "?minPropertyName=" + _minPropertyName + "&maxPropertyName=" + _maxPropertyName;
            }
        }

        #endregion

        #region StringRequired

        /// <summary>
        /// Rule ensuring a string value contains one or more
        /// characters.
        /// </summary>
        /// <param name="target">Object containing the data to validate</param>
        /// <param name="e">Arguments parameter specifying the name of the string
        /// property to validate</param>
        /// <returns><see langword="false" /> if the rule is broken</returns>
        /// <remarks>
        /// This implementation uses late binding, and will only work
        /// against string property values.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static bool StringRequired(object target, RuleArgs e)
        {
            string value = (string)Utilities.CallByName(
              target, e.PropertyName, CallType.Get);
            if (string.IsNullOrEmpty(value))
            {
                e.Description = Resources.ruleStringRequired.Replace("{rulePropertyName}", e.PropertyName);
                return false;
            }
            return true;
        }

        #endregion

        #region StringMaxLength

        /// <summary>
        /// Rule ensuring a string value doesn't exceed
        /// a specified length.
        /// </summary>
        /// <param name="target">Object containing the data to validate</param>
        /// <param name="e">Arguments parameter specifying the name of the string
        /// property to validate</param>
        /// <returns><see langword="false" /> if the rule is broken</returns>
        /// <remarks>
        /// This implementation uses late binding, and will only work
        /// against string property values.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static bool StringMaxLength(
          object target, RuleArgs e)
        {
            int max = ((MaxLengthRuleArgs)e).MaxLength;
            string value = (string)Utilities.CallByName(
              target, e.PropertyName, CallType.Get);
            if (!String.IsNullOrEmpty(value) && (value.Length > max))
            {
                e.Description = Resources.ruleStringMaxLength.Replace("{rulePropertyName}", e.PropertyName);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Custom <see cref="RuleArgs" /> object required by the
        /// <see cref="StringMaxLength" /> rule method.
        /// </summary>
        public class MaxLengthRuleArgs : RuleArgs
        {
            private int _maxLength;

            /// <summary>
            /// Get the max length for the string.
            /// </summary>
            public int MaxLength
            {
                get { return _maxLength; }
            }

            /// <summary>
            /// Create a new object.
            /// </summary>
            /// <param name="propertyName">Name of the property to validate.</param>
            /// <param name="maxLength">Max length of characters allowed.</param>
            public MaxLengthRuleArgs(
              string propertyName, int maxLength)
                : base(propertyName)
            {
                _maxLength = maxLength;
            }

            /// <summary>
            /// Return a string representation of the object.
            /// </summary>
            public override string ToString()
            {
                return base.ToString() + "?maxLength=" + _maxLength.ToString();
            }
        }

        #endregion

        #region IntegerMaxValue

        /// <summary>
        /// Rule ensuring an integer value doesn't exceed
        /// a specified value.
        /// </summary>
        /// <param name="target">Object containing the data to validate.</param>
        /// <param name="e">Arguments parameter specifying the name of the
        /// property to validate.</param>
        /// <returns><see langword="false"/> if the rule is broken.</returns>
        public static bool IntegerMaxValue(object target, RuleArgs e)
        {
            int max = ((IntegerMaxValueRuleArgs)e).MaxValue;
            int value = (int)Utilities.CallByName(target, e.PropertyName, CallType.Get);
            if (value > max)
            {
                e.Description = Resources.ruleIntegerMaxValue.Replace("{rulePropertyName}", e.PropertyName);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Custom <see cref="RuleArgs" /> object required by the
        /// <see cref="IntegerMaxValue" /> rule method.
        /// </summary>
        public class IntegerMaxValueRuleArgs : RuleArgs
        {
            private int _maxValue;

            /// <summary>
            /// Get the max value for the property.
            /// </summary>
            public int MaxValue
            {
                get { return _maxValue; }
            }

            /// <summary>
            /// Create a new object.
            /// </summary>
            /// <param name="propertyName">Name of the property.</param>
            /// <param name="maxValue">Maximum allowed value for the property.</param>
            public IntegerMaxValueRuleArgs(string propertyName, int maxValue)
                : base(propertyName)
            {
                _maxValue = maxValue;
            }

            /// <summary>
            /// Return a string representation of the object.
            /// </summary>
            public override string ToString()
            {
                return base.ToString() + "?maxValue=" + _maxValue.ToString();
            }
        }

        #endregion

        #region IntegerMinValue

        /// <summary>
        /// Rule ensuring an integer value doesn't go below
        /// a specified value.
        /// </summary>
        /// <param name="target">Object containing the data to validate.</param>
        /// <param name="e">Arguments parameter specifying the name of the
        /// property to validate.</param>
        /// <returns><see langword="false"/> if the rule is broken.</returns>
        public static bool IntegerMinValue(object target, RuleArgs e)
        {
            int min = ((IntegerMinValueRuleArgs)e).MinValue;
            int value = (int)Utilities.CallByName(target, e.PropertyName, CallType.Get);
            if (value < min)
            {
                e.Description = Resources.ruleMinMax.Replace("{rulePropertyName}", e.PropertyName);
                return false;

            }
            return true;
        }

        /// <summary>
        /// Custom <see cref="RuleArgs" /> object required by the
        /// <see cref="IntegerMinValue" /> rule method.
        /// </summary>
        public class IntegerMinValueRuleArgs : RuleArgs
        {
            private int _minValue;

            /// <summary>
            /// Get the min value for the property.
            /// </summary>
            public int MinValue
            {
                get { return _minValue; }
            }

            /// <summary>
            /// Create a new object.
            /// </summary>
            /// <param name="propertyName">Name of the property.</param>
            /// <param name="minValue">Minimum allowed value for the property.</param>
            public IntegerMinValueRuleArgs(string propertyName, int minValue)
                : base(propertyName)
            {
                _minValue = minValue;
            }

            /// <summary>
            /// Return a string representation of the object.
            /// </summary>
            public override string ToString()
            {
                return base.ToString() + "?minValue=" + _minValue.ToString();
            }
        }

        #endregion

        #region MaxValue

        /// <summary>
        /// Rule ensuring that a numeric value
        /// doesn't exceed a specified maximum.
        /// </summary>
        /// <typeparam name="T">Type of the property to validate.</typeparam>
        /// <param name="target">Object containing value to validate.</param>
        /// <param name="e">Arguments variable specifying the
        /// name of the property to validate, along with the max
        /// allowed value.</param>
        public static bool MaxValue<T>(object target, RuleArgs e) where T : IComparable
        {
            PropertyInfo pi = target.GetType().GetProperty(e.PropertyName);
            T value = (T)pi.GetValue(target, null);
            T max = ((MaxValueRuleArgs<T>)e).MaxValue;

            int result = value.CompareTo(max);
            if (result >= 1)
            {
                e.Description = Resources.ruleMaxValue.Replace("{rulePropertyName}", e.PropertyName);
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// Custom <see cref="RuleArgs" /> object required by the
        /// <see cref="MaxValue" /> rule method.
        /// </summary>
        /// <typeparam name="T">Type of the property to validate.</typeparam>
        public class MaxValueRuleArgs<T> : RuleArgs
        {
            T _maxValue = default(T);

            /// <summary>
            /// Get the max value for the property.
            /// </summary>
            public T MaxValue
            {
                get { return _maxValue; }
            }

            /// <summary>
            /// Create a new object.
            /// </summary>
            /// <param name="propertyName">Name of the property.</param>
            /// <param name="maxValue">Maximum allowed value for the property.</param>
            public MaxValueRuleArgs(string propertyName, T maxValue)
                : base(propertyName)
            {
                _maxValue = maxValue;
            }

            /// <summary>
            /// Returns a string representation of the object.
            /// </summary>
            public override string ToString()
            {
                return base.ToString() + "?maxValue=" + _maxValue.ToString();
            }
        }

        #endregion

        #region MinValue

        /// <summary>
        /// Rule ensuring that a numeric value
        /// doesn't exceed a specified minimum.
        /// </summary>
        /// <typeparam name="T">Type of the property to validate.</typeparam>
        /// <param name="target">Object containing value to validate.</param>
        /// <param name="e">Arguments variable specifying the
        /// name of the property to validate, along with the min
        /// allowed value.</param>
        public static bool MinValue<T>(object target, RuleArgs e) where T : IComparable
        {
            PropertyInfo pi = target.GetType().GetProperty(e.PropertyName);
            T value = (T)pi.GetValue(target, null);
            T min = ((MinValueRuleArgs<T>)e).MinValue;

            int result = value.CompareTo(min);
            if (result <= -1)
            {
                e.Description = Resources.ruleMinValue.Replace("{rulePropertyName}", e.PropertyName);
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// Custom <see cref="RuleArgs" /> object required by the
        /// <see cref="MinValue" /> rule method.
        /// </summary>
        /// <typeparam name="T">Type of the property to validate.</typeparam>
        public class MinValueRuleArgs<T> : RuleArgs
        {
            T _minValue = default(T);

            /// <summary>
            /// Get the min value for the property.
            /// </summary>
            public T MinValue
            {
                get { return _minValue; }
            }

            /// <summary>
            /// Create a new object.
            /// </summary>
            /// <param name="propertyName">Name of the property.</param>
            /// <param name="minValue">Minimum allowed value for the property.</param>
            public MinValueRuleArgs(string propertyName, T minValue)
                : base(propertyName)
            {
                _minValue = minValue;
            }

            /// <summary>
            /// Returns a string representation of the object.
            /// </summary>
            public override string ToString()
            {
                return base.ToString() + "?minValue=" + _minValue.ToString();
            }
        }

        #endregion

        #region RegExMatch

        /// <summary>
        /// Rule that checks to make sure a value
        /// matches a given regex pattern.
        /// </summary>
        /// <param name="target">Object containing the data to validate</param>
        /// <param name="e">RegExRuleArgs parameter specifying the name of the 
        /// property to validate and the regex pattern.</param>
        /// <returns>False if the rule is broken</returns>
        /// <remarks>
        /// This implementation uses late binding.
        /// </remarks>
        public static bool RegExMatch(object target, RuleArgs e)
        {
            Regex rx = ((RegExMatchRuleArgs)e).RegEx;
            if (!rx.IsMatch(Utilities.CallByName(target, e.PropertyName, CallType.Get).ToString()))
            {
                e.Description = Resources.ruleRegExMatch.Replace("{rulePropertyName}", e.PropertyName);
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// List of built-in regex patterns.
        /// </summary>
        public enum RegExPatterns
        {
            /// <summary>
            /// US Social Security number pattern.
            /// </summary>
            SSN,
            /// <summary>
            /// Email address pattern.
            /// </summary>
            Email
        }

        /// <summary>
        /// Custom <see cref="RuleArgs" /> object required by the
        /// <see cref="RegExMatch" /> rule method.
        /// </summary>
        public class RegExMatchRuleArgs : RuleArgs
        {
            Regex _regEx;

            /// <summary>
            /// The <see cref="RegEx"/> object used to validate
            /// the property.
            /// </summary>
            public Regex RegEx
            {
                get { return _regEx; }
            }

            /// <summary>
            /// Creates a new object.
            /// </summary>
            /// <param name="propertyName">Name of the property to validate.</param>
            /// <param name="pattern">Built-in regex pattern to use.</param>
            public RegExMatchRuleArgs(string propertyName, RegExPatterns pattern)
                :
              base(propertyName)
            {
                _regEx = new Regex(GetPattern(pattern));
            }

            /// <summary>
            /// Creates a new object.
            /// </summary>
            /// <param name="propertyName">Name of the property to validate.</param>
            /// <param name="pattern">Custom regex pattern to use.</param>
            public RegExMatchRuleArgs(string propertyName, string pattern)
                :
              base(propertyName)
            {
                _regEx = new Regex(pattern);
            }

            /// <summary>
            /// Creates a new object.
            /// </summary>
            /// <param name="propertyName">Name of the property to validate.</param>
            /// <param name="regEx"><see cref="RegEx"/> object to use.</param>
            public RegExMatchRuleArgs(string propertyName, System.Text.RegularExpressions.Regex regEx)
                :
              base(propertyName)
            {
                _regEx = regEx;
            }

            /// <summary>f
            /// Returns a string representation of the object.
            /// </summary>
            public override string ToString()
            {
                return base.ToString() + "?regex=" + _regEx.ToString();
            }

            /// <summary>
            /// Returns the specified built-in regex pattern.
            /// </summary>
            /// <param name="pattern">Pattern to return.</param>
            public static string GetPattern(RegExPatterns pattern)
            {
                switch (pattern)
                {
                    case RegExPatterns.SSN:
                        return @"^\d{3}-\d{2}-\d{4}$";
                    case RegExPatterns.Email:
                        return @"^[A-Za-z0-9._%-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$";
                    default:
                        return string.Empty;
                }
            }
        }

        #endregion

        //#region IsEmailAddress

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        //public static bool IsEmailAddress(object target, RuleArgs e)
        //{@TODO
        //    string addressToBeChecked = Utilities.CallByName(target, e.PropertyName, CallType.Get).ToString();
        //    string currentCharacter = string.Empty;
        //    int stringLength = addressToBeChecked.Length;
        //    int currentStringPosition = 0;
        //    int errorMessageLength = 0;
        //    int atSignCount = 0;
        //    bool hasPeriodAfterAtSign = false;
        //    bool hasNoForbiddenCharacters = true;
        //    Regex allowableCharacters = new Regex(@"[a-zA-z0-9!#$%&'*+-/=?^_`{|}~]");

        //    e.Description = "";
        //    if (stringLength > 0)
        //    {
        //        while (currentStringPosition < stringLength)
        //        {
        //            currentCharacter = addressToBeChecked.Substring(currentStringPosition, 1);
        //            System.Console.WriteLine("Current Character = " + currentCharacter);
        //            if (!allowableCharacters.IsMatch(currentCharacter) | currentCharacter  == ".")
        //            {
        //                if (currentCharacter == "@")
        //                {
        //                    atSignCount++;
        //                    if (currentStringPosition == 0)
        //                    {
        //                        e.Description = e.Description + Resources.ruleIsEmailAddressStartAtSign + "\n";
        //                        hasNoForbiddenCharacters = false;
        //                    }
        //                    if (currentStringPosition == stringLength - 1)
        //                    {
        //                        e.Description = e.Description + Resources.ruleIsEmailAddressEndAtSign + "\n";
        //                        hasNoForbiddenCharacters = false;
        //                    }
        //                    if (atSignCount > 1)
        //                    {
        //                        e.Description = e.Description + Resources.ruleIsEmailAddressTwoAtSigns;
        //                        hasNoForbiddenCharacters = false;
        //                    }
        //                    if (currentStringPosition > 0 && currentStringPosition < stringLength)
        //                    {
        //                        if (addressToBeChecked.Substring(currentStringPosition - 1, 1) == "."
        //                             || addressToBeChecked.Substring(currentStringPosition + 1, 1) == "."
        //                           )
        //                        {
        //                            e.Description = e.Description + Resources.ruleIsEmailAddressPeriodNextToAtSign + "\n";
        //                            hasNoForbiddenCharacters = false;
        //                        }
        //                    }

        //                }
        //                else
        //                {
        //                    if (currentCharacter == ".")
        //                    {
        //                        if (currentStringPosition == 0)
        //                        {
        //                            e.Description = e.Description + Resources.ruleIsEmailAddressStartPeriod + "\n";
        //                            hasNoForbiddenCharacters = false;
        //                        }
        //                        if (currentStringPosition == stringLength - 1)
        //                        {
        //                            e.Description = e.Description + Resources.ruleIsEmailAddressEndPeriod + "\n";
        //                            hasNoForbiddenCharacters = false;
        //                        }
        //                        if (currentStringPosition > 0 && currentStringPosition < stringLength)
        //                        {
        //                            if (addressToBeChecked.Substring(currentStringPosition - 1, 1) == "@"
        //                                 || addressToBeChecked.Substring(currentStringPosition + 1, 1) == "@"
        //                               )
        //                            {
        //                                e.Description = e.Description + Resources.ruleIsEmailAddressPeriodNextToAtSign + "\n";
        //                                hasNoForbiddenCharacters = false;
        //                            }
        //                            if (addressToBeChecked.Substring(currentStringPosition - 1, 1) == "."
        //                                 || addressToBeChecked.Substring(currentStringPosition + 1, 1) == "."
        //                               )
        //                            {
        //                                e.Description = e.Description + Resources.ruleIsEmailAddressAdjacentPeriods + "\n";
        //                                hasNoForbiddenCharacters = false;
        //                            }
        //                        }
        //                        if (atSignCount > 0)
        //                        {
        //                            hasPeriodAfterAtSign = true;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        e.Description = e.Description + Resources.ruleIsEmailAddressForbiddenCharacter.Replace("{invalidCharacter}", currentCharacter) + "\n";
        //                        hasNoForbiddenCharacters = false;
        //                    }
        //                }
        //            }
        //            currentStringPosition++;
        //        }
        //        if (hasPeriodAfterAtSign == false)
        //        {
        //            e.Description = e.Description + Resources.ruleIsEmailAddressNoPeriodAfterAtSign + "\n";
        //            hasNoForbiddenCharacters = false;
        //        }
        //        if (atSignCount == 0)
        //        {
        //            e.Description = e.Description + Resources.ruleIsEmailAddressNoAtSign + "\n";
        //            hasNoForbiddenCharacters = false;
        //        }
        //        if (hasNoForbiddenCharacters == false)
        //        {
        //            e.Description = Resources.ruleIsEmailAddress.Replace("{rulePropertyName}",e.PropertyName) + "\n" + e.Description;
        //            errorMessageLength = e.Description.Length;
        //            e.Description = e.Description.Substring(0, errorMessageLength - 1);
        //        }
        //    }
        //    return hasNoForbiddenCharacters;
        //}

        //public class IsEmailAddressRuleArgs : RuleArgs
        //{
        //    string _addressToBeChecked;

        //    public string AddressToBeChecked
        //    {
        //        get { return _addressToBeChecked; }
        //    }
        //}

 
                


        //#endregion

    }
}

