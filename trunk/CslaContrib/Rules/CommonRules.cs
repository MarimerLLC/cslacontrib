using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Csla.Core;
using Csla.Properties;
using Csla.Rules;

namespace CslaContrib.Rules.CommonRules
{
  #region Base class for Common Business Rules

  /// <summary>
  /// Base class used to create common rules.
  /// </summary>
  public abstract class CommonBusinessRule : BusinessRule
  {
    /// <summary>
    /// Gets or sets the severity for this rule.
    /// </summary>
    public RuleSeverity Severity { get; set; }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    protected virtual string ErrorMessage
    {
      get { return ErrorMessageDelegate.Invoke(); }
    }

    /// <summary>
    /// Gets or sets the error message function for this rule.
    /// </summary>    
    public Func<string> ErrorMessageDelegate { get; set; }

    protected bool HasErrorMessageDelegate
    {
      get { return ErrorMessageDelegate != null; }
    }

    /// <summary>
    /// Creates an instance of the rule.
    /// </summary>
    /// <param name="primaryProperty">Primary property.</param>
    protected CommonBusinessRule(IPropertyInfo primaryProperty)
      : base(primaryProperty)
    {
      Severity = RuleSeverity.Error;
    }
  }

  #endregion

  #region Re-implementation of Csla Common Rules with ErrorMessageDelegate

  /// <summary>
  /// Business rule for a required string.
  /// </summary>
  public class Required : CommonBusinessRule
  {
    /// <summary>
    /// Creates an instance of the rule.
    /// </summary>
    /// <param name="primaryProperty">Property to which the rule applies.</param>
    public Required(IPropertyInfo primaryProperty)
      : base(primaryProperty)
    {
      InputProperties = new List<IPropertyInfo> { primaryProperty };
    }

    /// <summary>
    /// Creates an instance of the rule.
    /// </summary>
    /// <param name="primaryProperty">Property to which the rule applies.</param>
    /// <param name="errorMessageDelegate">The error message function.</param>
    public Required(IPropertyInfo primaryProperty, Func<string> errorMessageDelegate)
      : this(primaryProperty)
    {
      ErrorMessageDelegate = errorMessageDelegate;
    }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    /// <value></value>
    protected override string ErrorMessage
    {
      get
      {
        return HasErrorMessageDelegate ? base.ErrorMessage : Csla.Properties.Resources.StringRequiredRule;
      }
    }

    /// <summary>
    /// Rule implementation.
    /// </summary>
    /// <param name="context">Rule context.</param>
    protected override void Execute(RuleContext context)
    {
      var value = context.InputPropertyValues[PrimaryProperty];
#if WINDOWS_PHONE
            if (value == null || string.IsNullOrEmpty(value.ToString()))
#else
      if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
#endif
      {
        var message = string.Format(ErrorMessage, PrimaryProperty.FriendlyName);
        context.Results.Add(new RuleResult(RuleName, PrimaryProperty, message) { Severity = Severity });
      }
    }
  }

  /// <summary>
  /// Business rule for a maximum length string.
  /// </summary>
  public class MaxLength : CommonBusinessRule
  {
    /// <summary>
    /// Gets the max length value.
    /// </summary>
    public int Max { get; private set; }

    /// <summary>
    /// Creates an instance of the rule.
    /// </summary>
    /// <param name="primaryProperty">Property to which the rule applies.</param>
    /// <param name="max">Max length value.</param>
    public MaxLength(IPropertyInfo primaryProperty, int max)
      : base(primaryProperty)
    {
      Max = max;
      RuleUri.AddQueryParameter("max", max.ToString());
      InputProperties = new List<IPropertyInfo> { primaryProperty };
    }

    /// <summary>
    /// Creates an instance of the rule.
    /// </summary>
    /// <param name="primaryProperty">Property to which the rule applies.</param>
    /// <param name="max">Max length value.</param>
    /// <param name="errorMessageDelegate">The error message function.</param>
    public MaxLength(IPropertyInfo primaryProperty, int max, Func<string> errorMessageDelegate)
      : this(primaryProperty, max)
    {
      ErrorMessageDelegate = errorMessageDelegate;
    }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    /// <value></value>
    protected override string ErrorMessage
    {
      get
      {
        return HasErrorMessageDelegate ? base.ErrorMessage : Csla.Properties.Resources.StringMaxLengthRule;
      }
    }

    /// <summary>
    /// Rule implementation.
    /// </summary>
    /// <param name="context">Rule context.</param>
    protected override void Execute(RuleContext context)
    {
      var value = context.InputPropertyValues[PrimaryProperty];
      if (value != null && value.ToString().Length > Max)
      {
        var message = string.Format(ErrorMessage, PrimaryProperty.FriendlyName, Max);
        context.Results.Add(new RuleResult(RuleName, PrimaryProperty, message) { Severity = Severity });
      }
    }
  }

  /// <summary>
  /// Business rule for a minimum length string.
  /// </summary>
  public class MinLength : CommonBusinessRule
  {
    /// <summary>
    /// Gets the min length value.
    /// </summary>
    public int Min { get; private set; }

    /// <summary>
    /// Creates an instance of the rule.
    /// </summary>
    /// <param name="primaryProperty">Property to which the rule applies.</param>
    /// <param name="min">Min length value.</param>
    public MinLength(IPropertyInfo primaryProperty, int min)
      : base(primaryProperty)
    {
      Min = min;
      RuleUri.AddQueryParameter("min", min.ToString());
      InputProperties = new List<IPropertyInfo> { primaryProperty };
    }

    /// <summary>
    /// Creates an instance of the rule.
    /// </summary>
    /// <param name="primaryProperty">Property to which the rule applies.</param>
    /// <param name="min">Min length value.</param>
    /// <param name="errorMessageDelegate">The error message function.</param>
    public MinLength(IPropertyInfo primaryProperty, int min, Func<string> errorMessageDelegate)
      : this(primaryProperty, min)
    {
      ErrorMessageDelegate = errorMessageDelegate;
    }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    /// <value></value>
    protected override string ErrorMessage
    {
      get
      {
        return HasErrorMessageDelegate ? base.ErrorMessage : Csla.Properties.Resources.StringMinLengthRule;
      }
    }

    /// <summary>
    /// Rule implementation.
    /// </summary>
    /// <param name="context">Rule context.</param>
    protected override void Execute(RuleContext context)
    {
      var value = context.InputPropertyValues[PrimaryProperty];
      if (value != null && value.ToString().Length < Min)
      {
        var message = string.Format(ErrorMessage, PrimaryProperty.FriendlyName, Min);
        context.Results.Add(new RuleResult(RuleName, PrimaryProperty, message) { Severity = Severity });
      }
    }
  }

  /// <summary>
  /// Business rule for a minimum value.
  /// </summary>
  public class MinValue<T> : CommonBusinessRule
      where T : IComparable
  {
    /// <summary>
    /// Gets the min value.
    /// </summary>
    public T Min { get; private set; }

    /// <summary>
    /// Gets or sets the format string used
    /// to format the Min value.
    /// </summary>
    public string Format { get; set; }

    /// <summary>
    /// Creates an instance of the rule.
    /// </summary>
    /// <param name="primaryProperty">Property to which the rule applies.</param>
    /// <param name="min">Min length value.</param>
    public MinValue(IPropertyInfo primaryProperty, T min)
      : base(primaryProperty)
    {
      Min = min;
      RuleUri.AddQueryParameter("min", min.ToString());
      InputProperties = new List<IPropertyInfo> { primaryProperty };
    }

    /// <summary>
    /// Creates an instance of the rule.
    /// </summary>
    /// <param name="primaryProperty">Property to which the rule applies.</param>
    /// <param name="min">Min length value.</param>
    /// <param name="errorMessageDelegate">The error message function.</param>
    public MinValue(IPropertyInfo primaryProperty, T min, Func<string> errorMessageDelegate)
      : this(primaryProperty, min)
    {
      ErrorMessageDelegate = errorMessageDelegate;
    }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    /// <value></value>
    protected override string ErrorMessage
    {
      get
      {
        return HasErrorMessageDelegate ? base.ErrorMessage : Csla.Properties.Resources.MinValueRule;
      }
    }

    /// <summary>
    /// Rule implementation.
    /// </summary>
    /// <param name="context">Rule context.</param>
    protected override void Execute(RuleContext context)
    {
      var value = (T)context.InputPropertyValues[PrimaryProperty];
      int result = value.CompareTo(Min);
      if (result <= -1)
      {
        string outValue;
        if (string.IsNullOrEmpty(Format))
          outValue = Min.ToString();
        else
          outValue = string.Format(string.Format("{{0:{0}}}", Format), Min);
        var message = string.Format(ErrorMessage, PrimaryProperty.FriendlyName, outValue);
        context.Results.Add(new RuleResult(RuleName, PrimaryProperty, message) { Severity = Severity });
      }
    }
  }

  /// <summary>
  /// Business rule for a maximum value.
  /// </summary>
  public class MaxValue<T> : CommonBusinessRule
      where T : IComparable
  {
    /// <summary>
    /// Gets the max value.
    /// </summary>
    public T Max { get; private set; }

    /// <summary>
    /// Gets or sets the format string used
    /// to format the Max value.
    /// </summary>
    public string Format { get; set; }

    /// <summary>
    /// Creates an instance of the rule.
    /// </summary>
    /// <param name="primaryProperty">Property to which the rule applies.</param>
    /// <param name="max">Max length value.</param>
    public MaxValue(IPropertyInfo primaryProperty, T max)
      : base(primaryProperty)
    {
      Max = max;
      RuleUri.AddQueryParameter("max", max.ToString());
      InputProperties = new List<IPropertyInfo> { primaryProperty };
    }

    /// <summary>
    /// Creates an instance of the rule.
    /// </summary>
    /// <param name="primaryProperty">Property to which the rule applies.</param>
    /// <param name="max">Max length value.</param>
    /// <param name="errorMessageDelegate">The error message function.</param>
    public MaxValue(IPropertyInfo primaryProperty, T max, Func<string> errorMessageDelegate)
      : this(primaryProperty, max)
    {
      ErrorMessageDelegate = errorMessageDelegate;
    }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    /// <value></value>
    protected override string ErrorMessage
    {
      get
      {
        return HasErrorMessageDelegate ? base.ErrorMessage : Csla.Properties.Resources.MaxValueRule;
      }
    }

    /// <summary>
    /// Rule implementation.
    /// </summary>
    /// <param name="context">Rule context.</param>
    protected override void Execute(RuleContext context)
    {
      if (ErrorMessageDelegate == null)
        ErrorMessageDelegate = () => Resources.MaxValueRule;

      var value = (T)context.InputPropertyValues[PrimaryProperty];
      int result = value.CompareTo(Max);
      if (result >= 1)
      {
        string outValue;
        if (string.IsNullOrEmpty(Format))
          outValue = Max.ToString();
        else
          outValue = string.Format(string.Format("{{0:{0}}}", Format), Max);
        var message = string.Format(ErrorMessage, PrimaryProperty.FriendlyName, outValue);
        context.Results.Add(new RuleResult(RuleName, PrimaryProperty, message) { Severity = Severity });
      }
    }
  }

  /// <summary>
  /// Business rule that evaluates a regular expression.
  /// </summary>
  public class RegExMatch : CommonBusinessRule
  {
    #region NullResultOptions

    /// <summary>
    /// List of options for the NullResult
    /// property.
    /// </summary>
    public enum NullResultOptions
    {
      /// <summary>
      /// Indicates that a null value
      /// should always result in the 
      /// rule returning false.
      /// </summary>
      ReturnFalse,
      /// <summary>
      /// Indicates that a null value
      /// should always result in the 
      /// rule returning true.
      /// </summary>
      ReturnTrue,
      /// <summary>
      /// Indicates that a null value
      /// should be converted to an
      /// empty string before the
      /// regular expression is
      /// evaluated.
      /// </summary>
      ConvertToEmptyString
    }

    #endregion

    /// <summary>
    /// Gets the regular expression
    /// to be evaluated.
    /// </summary>
    public string Expression { get; private set; }

    /// <summary>
    /// Gets or sets a value that controls how
    /// null input values are handled.
    /// </summary>
    public NullResultOptions NullOption { get; set; }

    /// <summary>
    /// Creates an instance of the rule.
    /// </summary>
    /// <param name="primaryProperty">Primary property.</param>
    /// <param name="expression">Regular expression.</param>
    public RegExMatch(IPropertyInfo primaryProperty, string expression)
      : base(primaryProperty)
    {
      Expression = expression;
      RuleUri.AddQueryParameter("e", expression);
      InputProperties = new List<IPropertyInfo> { primaryProperty };
    }

    /// <summary>
    /// Creates an instance of the rule.
    /// </summary>
    /// <param name="primaryProperty">Primary property.</param>
    /// <param name="expression">Regular expression.</param>
    /// <param name="errorMessageDelegate">The error message function.</param>
    public RegExMatch(IPropertyInfo primaryProperty, string expression, Func<string> errorMessageDelegate)
      : this(primaryProperty, expression)
    {
      ErrorMessageDelegate = errorMessageDelegate;
    }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    /// <value></value>
    protected override string ErrorMessage
    {
      get
      {
        return HasErrorMessageDelegate ? base.ErrorMessage : Csla.Properties.Resources.RegExMatchRule;
      }
    }

    /// <summary>
    /// Rule implementation.
    /// </summary>
    /// <param name="context">Rule context.</param>
    protected override void Execute(RuleContext context)
    {
      var value = context.InputPropertyValues[PrimaryProperty];
      bool ruleSatisfied;
      var expression = new Regex(Expression);

      if (value == null && NullOption == NullResultOptions.ConvertToEmptyString)
        value = string.Empty;

      if (value == null)
      {
        // if the value is null at this point
        // then return the pre-defined result value
        ruleSatisfied = (NullOption == NullResultOptions.ReturnTrue);
      }
      else
      {
        // the value is not null, so run the 
        // regular expression
        ruleSatisfied = expression.IsMatch(value.ToString());
      }

      if (!ruleSatisfied)
      {
        var message = string.Format(ErrorMessage, PrimaryProperty.FriendlyName);
        context.Results.Add(new RuleResult(RuleName, PrimaryProperty, message) { Severity = Severity });
      }
    }
  }

  #endregion

  #region Comparable Field Rules

  /// <summary>
  /// Validates that primary property is less than compareToProperty
  /// </summary>
  public class LessThan : CommonBusinessRule
  {
    private IPropertyInfo CompareTo { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LessThan"/> class.
    /// </summary>
    /// <param name="primaryProperty">The primary property.</param>
    /// <param name="compareToProperty">The compare to property.</param>
    public LessThan(IPropertyInfo primaryProperty, IPropertyInfo compareToProperty)
      : base(primaryProperty)
    {
      CompareTo = compareToProperty;
      InputProperties = new List<IPropertyInfo> { primaryProperty, compareToProperty };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LessThan"/> class.
    /// </summary>
    /// <param name="primaryProperty">The primary property.</param>
    /// <param name="compareToProperty">The compare to property.</param>
    /// <param name="errorMessageDelegate">The error message function.</param>
    public LessThan(IPropertyInfo primaryProperty, IPropertyInfo compareToProperty,
                    Func<string> errorMessageDelegate)
      : this(primaryProperty, compareToProperty)
    {
      ErrorMessageDelegate = errorMessageDelegate;
    }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    /// <value></value>
    protected override string ErrorMessage
    {
      get
      {
        return HasErrorMessageDelegate ? base.ErrorMessage : CslaContrib.Properties.Resources.LessThanRule;
      }
    }

    /// <summary>
    /// Does the check for primary propert less than compareTo property
    /// </summary>
    /// <param name="context">Rule context object.</param>
    protected override void Execute(RuleContext context)
    {
      var value1 = (IComparable)context.InputPropertyValues[PrimaryProperty];
      var value2 = (IComparable)context.InputPropertyValues[CompareTo];

      if (value1.CompareTo(value2) >= 0)
      {
        context.AddErrorResult(string.Format(ErrorMessage, PrimaryProperty.FriendlyName, CompareTo.FriendlyName));
      }
    }
  }

  /// <summary>
  /// Validates that primary property is less than or equal compareToProperty
  /// </summary>
  public class LessThanOrEqual : CommonBusinessRule
  {
    private IPropertyInfo CompareTo { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LessThanOrEqual"/> class.
    /// </summary>
    /// <param name="primaryProperty">The primary property.</param>
    /// <param name="compareToProperty">The compare to property.</param>
    public LessThanOrEqual(IPropertyInfo primaryProperty, IPropertyInfo compareToProperty)
      : base(primaryProperty)
    {
      CompareTo = compareToProperty;
      InputProperties = new List<IPropertyInfo> { primaryProperty, compareToProperty };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LessThanOrEqual"/> class.
    /// </summary>
    /// <param name="primaryProperty">The primary property.</param>
    /// <param name="compareToProperty">The compare to property.</param>
    /// <param name="errorMessageDelegate">The error message function.</param>
    public LessThanOrEqual(IPropertyInfo primaryProperty, IPropertyInfo compareToProperty,
                           Func<string> errorMessageDelegate)
      : this(primaryProperty, compareToProperty)
    {
      ErrorMessageDelegate = errorMessageDelegate;
    }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    /// <value></value>
    protected override string ErrorMessage
    {
      get
      {
        return HasErrorMessageDelegate ? base.ErrorMessage : CslaContrib.Properties.Resources.LessThanOrEqualRule;
      }
    }

    /// <summary>
    /// Does the check for primary propert less than compareTo property
    /// </summary>
    /// <param name="context">Rule context object.</param>
    protected override void Execute(RuleContext context)
    {
      var value1 = (IComparable)context.InputPropertyValues[PrimaryProperty];
      var value2 = (IComparable)context.InputPropertyValues[CompareTo];

      if (value1.CompareTo(value2) > 0)
      {
        context.AddErrorResult(string.Format(ErrorMessage, PrimaryProperty.FriendlyName, CompareTo.FriendlyName));
      }
    }
  }

  /// <summary>
  /// Validates that primary property is greater than compareToProperty
  /// </summary>
  public class GreaterThan : CommonBusinessRule
  {
    private IPropertyInfo CompareTo { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GreaterThan"/> class.
    /// </summary>
    /// <param name="primaryProperty">The primary property.</param>
    /// <param name="compareToProperty">The compare to property.</param>
    public GreaterThan(IPropertyInfo primaryProperty, IPropertyInfo compareToProperty)
      : base(primaryProperty)
    {
      CompareTo = compareToProperty;
      InputProperties = new List<IPropertyInfo> { primaryProperty, compareToProperty };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GreaterThan"/> class.
    /// </summary>
    /// <param name="primaryProperty">The primary property.</param>
    /// <param name="compareToProperty">The compare to property.</param>
    /// <param name="errorMessageDelegate">The error message function.</param>
    public GreaterThan(IPropertyInfo primaryProperty, IPropertyInfo compareToProperty,
                       Func<string> errorMessageDelegate)
      : this(primaryProperty, compareToProperty)
    {
      ErrorMessageDelegate = errorMessageDelegate;
    }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    /// <value></value>
    protected override string ErrorMessage
    {
      get
      {
        return HasErrorMessageDelegate ? base.ErrorMessage : CslaContrib.Properties.Resources.GreaterThanRule;
      }
    }

    /// <summary>
    /// Does the check for primary propert less than compareTo property
    /// </summary>
    /// <param name="context">Rule context object.</param>
    protected override void Execute(RuleContext context)
    {
      var value1 = (IComparable)context.InputPropertyValues[PrimaryProperty];
      var value2 = (IComparable)context.InputPropertyValues[CompareTo];

      if (value1.CompareTo(value2) <= 0)
      {
        context.AddErrorResult(string.Format(ErrorMessage, PrimaryProperty.FriendlyName, CompareTo.FriendlyName));
      }
    }
  }

  /// <summary>
  /// Validates that primary property is freater than or equal compareToProperty
  /// </summary>
  public class GreaterThanOrEqual : CommonBusinessRule
  {
    private IPropertyInfo CompareTo { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GreaterThanOrEqual"/> class.
    /// </summary>
    /// <param name="primaryProperty">The primary property.</param>
    /// <param name="compareToProperty">The compare to property.</param>
    public GreaterThanOrEqual(IPropertyInfo primaryProperty, IPropertyInfo compareToProperty)
      : base(primaryProperty)
    {
      CompareTo = compareToProperty;
      InputProperties = new List<IPropertyInfo> { primaryProperty, compareToProperty };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GreaterThanOrEqual"/> class.
    /// </summary>
    /// <param name="primaryProperty">The primary property.</param>
    /// <param name="compareToProperty">The compare to property.</param>
    /// <param name="errorMessageDelegate">The error message function.</param>
    public GreaterThanOrEqual(IPropertyInfo primaryProperty, IPropertyInfo compareToProperty,
                              Func<string> errorMessageDelegate)
      : this(primaryProperty, compareToProperty)
    {
      ErrorMessageDelegate = errorMessageDelegate;
    }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    /// <value></value>
    protected override string ErrorMessage
    {
      get
      {
        return HasErrorMessageDelegate ? base.ErrorMessage : CslaContrib.Properties.Resources.GreaterThanOrEqualRule;
      }
    }

    /// <summary>
    /// Does the check for primary propert less than compareTo property
    /// </summary>
    /// <param name="context">Rule context object.</param>
    protected override void Execute(RuleContext context)
    {
      var value1 = (IComparable)context.InputPropertyValues[PrimaryProperty];
      var value2 = (IComparable)context.InputPropertyValues[CompareTo];

      if (value1.CompareTo(value2) < 0)
      {
        context.AddErrorResult(string.Format(ErrorMessage, PrimaryProperty.FriendlyName, CompareTo.FriendlyName));
      }
    }
  }

  #endregion

  #region Flow Control Rules

  /// <summary>
  /// ShortCircuit rule processing if user is not allowed to edit field.
  /// </summary>
  public class StopIfNotCanWrite : BusinessRule
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="StopIfNotCanWrite"/> class.
    /// </summary>
    /// <param name="property">The property to check.</param>
    public StopIfNotCanWrite(IPropertyInfo property)
      : base(property)
    {
    }

    /// <summary>
    /// Rule indicating whether the user is authorized
    /// to change the property value.
    /// Will always be silent and never set rule to broken.
    /// </summary>
    /// <param name="context">Rule context object.</param>
    /// <remarks>
    /// Combine this Rule with short-circuiting to
    /// prevent evaluation of other rules in the case
    /// that the user isn't allowed to change the value.
    /// </remarks>
    protected override void Execute(RuleContext context)
    {
      var business = context.Target as BusinessBase;
      if (business == null) return;

      if (!business.CanWriteProperty(context.Rule.PrimaryProperty))
      {
        context.AddSuccessResult(true);
      }
    }
  }

  /// <summary>
  /// ShortCircuit rule processing if target is not a new object
  /// </summary>
  public class StopIfNotIsNew : BusinessRule
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="StopIfNotIsNew"/> class.
    /// </summary>
    /// <param name="property">The property.</param>
    public StopIfNotIsNew(IPropertyInfo property) : base(property) { }

    /// <summary>
    /// Executes the rule in specified context.
    /// </summary>
    /// <param name="context">The context.</param>
    protected override void Execute(RuleContext context)
    {
      var target = (ITrackStatus)context.Target;
      if (!target.IsNew)
      {
        context.AddSuccessResult(true);
      }
    }
  }

  /// <summary>
  /// ShortCircuit rule processing if target is not an existing object.
  /// </summary>
  public class StopIfNotIsExisting : BusinessRule
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="StopIfNotIsExisting"/> class.
    /// </summary>
    /// <param name="property">The property.</param>
    public StopIfNotIsExisting(IPropertyInfo property) : base(property) { }

    /// <summary>
    /// Executes the rule in specified context.
    /// </summary>
    /// <param name="context">The context.</param>
    protected override void Execute(RuleContext context)
    {
      var target = (ITrackStatus)context.Target;
      if (target.IsNew)
      {
        context.AddSuccessResult(true);
      }
    }
  }

  /// <summary>
  /// If any of the additional properties has a value stop rule processing 
  /// for this field and make field valid. 
  /// </summary>
  public class StopIfAnyAdditionalHasValue : BusinessRule
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="StopIfAnyAdditionalHasValue"/> class.
    /// </summary>
    /// <param name="primaryProperty">The primary property.</param>
    /// <param name="additionalProperties">The additional properties.</param>
    public StopIfAnyAdditionalHasValue(IPropertyInfo primaryProperty, params IPropertyInfo[] additionalProperties)
      : base(primaryProperty)
    {
      if (InputProperties == null) InputProperties = new List<IPropertyInfo> { primaryProperty };
      InputProperties.AddRange(additionalProperties);
    }

    /// <summary>
    /// Executes the rule in specified context.
    /// </summary>
    /// <param name="context">The context.</param>
    protected override void Execute(RuleContext context)
    {
      var hasValue = false;
      // excludes primary property 
      foreach (var field in context.InputPropertyValues.Where(p => p.Key != PrimaryProperty))
      {
        // smartfields have their own implementation of IsEmpty
        var smartField = field.Value as ISmartField;

        if (smartField != null)
        {
          hasValue = !smartField.IsEmpty;
        }
        else if (field.Value != null && !field.Value.Equals(field.Key.DefaultValue))
        {
          hasValue = true;
        }
      }

      // if hasValue then shortcut rule processing      
      if (hasValue) context.AddSuccessResult(true);
    }
  }

  #endregion

  #region Transformation Rules

  /// <summary>
  /// CalcSum rule will set primary property to the sum of all supplied properties.
  /// </summary>
  public class CalcSum : BusinessRule
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="CalcSum"/> class.
    /// </summary>
    /// <param name="primaryProperty">The primary property.</param>
    /// <param name="inputProperties">The input properties.</param>
    public CalcSum(IPropertyInfo primaryProperty, params IPropertyInfo[] inputProperties)
      : base(primaryProperty)
    {
      InputProperties = new List<IPropertyInfo>();
      InputProperties.AddRange(inputProperties);
    }

    /// <summary>
    /// Business or validation rule implementation.
    /// </summary>
    /// <param name="context">Rule context object.</param>
    protected override void Execute(RuleContext context)
    {
      // Use linq Sum to calculate the sum value
      var sum = context.InputPropertyValues.Sum(property => (dynamic)property.Value);

      // add calculated value to OutValues
      // When rule is completed the RuleEngig will update businessobject
      context.AddOutValue(PrimaryProperty, sum);
    }
  }

  /// <summary>
  /// makes sure the property is formatted as uppercase string.
  /// </summary>
  public class ToUpperCase : BusinessRule
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ToUpperCase"/> class.
    /// </summary>
    /// <param name="primaryProperty">The primary property.</param>
    public ToUpperCase(IPropertyInfo primaryProperty)
      : base(primaryProperty)
    {
      InputProperties = new List<IPropertyInfo> { primaryProperty };
    }

    /// <summary>
    /// Business or validation rule implementation.
    /// </summary>
    /// <param name="context">Rule context object.</param>
    protected override void Execute(RuleContext context)
    {
      var value = (string)context.InputPropertyValues[PrimaryProperty];
      if (string.IsNullOrEmpty(value)) return;

      var newValue = value.ToUpper();
      context.AddOutValue(PrimaryProperty, newValue);
    }
  }

  /// <summary>
  /// makes sure the property is formatted as uppercase string.
  /// </summary>
  public class ToLowerCase : BusinessRule
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ToLowerCase"/> class.
    /// </summary>
    /// <param name="primaryProperty">The primary property.</param>
    public ToLowerCase(IPropertyInfo primaryProperty)
      : base(primaryProperty)
    {
      InputProperties = new List<IPropertyInfo> { primaryProperty };
    }

    /// <summary>
    /// Business or validation rule implementation.
    /// </summary>
    /// <param name="context">Rule context object.</param>
    protected override void Execute(RuleContext context)
    {
      var value = (string)context.InputPropertyValues[PrimaryProperty];
      if (string.IsNullOrEmpty(value)) return;

      var newValue = value.ToLower();
      context.AddOutValue(PrimaryProperty, newValue);
    }
  }

  /// <summary>
  /// Removes leading, trailing and duplicate spaces.
  /// </summary>
  public class CollapseSpace : BusinessRule
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="CollapseSpace"/> class.
    /// </summary>
    /// <param name="primaryProperty">The primary property.</param>
    public CollapseSpace(IPropertyInfo primaryProperty)
      : base(primaryProperty)
    {
      InputProperties = new List<IPropertyInfo> { primaryProperty };
    }

    /// <summary>
    /// Business or validation rule implementation.
    /// </summary>
    /// <param name="context">Rule context object.</param>
    protected override void Execute(RuleContext context)
    {
      var value = (string)context.InputPropertyValues[PrimaryProperty];
      if (string.IsNullOrEmpty(value)) return;

      var newValue = value.Trim(' ');
      var r = new Regex(@" +");
      newValue = r.Replace(newValue, @" ");
      context.AddOutValue(PrimaryProperty, newValue);
    }
  }

  /// <summary>
  /// Removes leading, trailing, duplicate white space characters.
  /// </summary>
  public class CollapseWhiteSpace : BusinessRule
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="CollapseWhiteSpace"/> class.
    /// </summary>
    /// <param name="primaryProperty">The primary property.</param>
    public CollapseWhiteSpace(IPropertyInfo primaryProperty)
      : base(primaryProperty)
    {
      InputProperties = new List<IPropertyInfo> { primaryProperty };
    }

    /// <summary>
    /// Business or validation rule implementation.
    /// </summary>
    /// <param name="context">Rule context object.</param>
    protected override void Execute(RuleContext context)
    {
      var value = (string)context.InputPropertyValues[PrimaryProperty];
      if (string.IsNullOrEmpty(value)) return;

      var newValue = value.Trim();
      var r = new Regex(@"\s+");
      newValue = r.Replace(newValue, @" ");
      context.AddOutValue(PrimaryProperty, newValue);
    }
  }

  #endregion

  /// <summary>
  /// Check that at least one of the fields of type string or smartvalue field has a value.
  /// Code must also add Dependency rules from each additional properties to primary property.
  /// </summary>
  public class AnyRequired : CommonBusinessRule
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="AnyRequired"/> class.
    /// </summary>
    /// <param name="primaryProperty">The primary property.</param>
    /// <param name="additionalProperties">The additional properties.</param>
    public AnyRequired(IPropertyInfo primaryProperty, params IPropertyInfo[] additionalProperties)
      : base(primaryProperty)
    {
      if (InputProperties == null)
        InputProperties = new List<IPropertyInfo>();
      InputProperties.AddRange(additionalProperties);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AnyRequired"/> class.
    /// </summary>
    /// <param name="primaryProperty">The primary property.</param>
    /// <param name="errorMessageDelegate">The error message function.</param>
    /// <param name="additionalProperties">The additional properties.</param>
    public AnyRequired(IPropertyInfo primaryProperty, Func<string> errorMessageDelegate, params IPropertyInfo[] additionalProperties)
      : this(primaryProperty, additionalProperties)
    {
      ErrorMessageDelegate = errorMessageDelegate;
    }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    /// <value></value>
    protected override string ErrorMessage
    {
      get
      {
        return HasErrorMessageDelegate ? base.ErrorMessage : CslaContrib.Properties.Resources.AnyRequiredRule;
      }
    }

    /// <summary>
    /// Executes the rule in specified context.
    /// </summary>
    /// <param name="context">The context.</param>
    protected override void Execute(RuleContext context)
    {
      foreach (var field in context.InputPropertyValues)
      {
        // smartfields have their own implementation of IsEmpty
        var smartField = field.Value as ISmartField;

        if (smartField != null)
        {
          if (!smartField.IsEmpty) return;
        }
        else if (field.Value != null && !field.Value.Equals(field.Key.DefaultValue)) return;
      }

      var fields = context.InputPropertyValues.Select(p => p.Key.FriendlyName).ToArray();
      var fieldNames = String.Join(", ", fields);
      context.AddErrorResult(string.Format(ErrorMessage, fieldNames));
    }
  }

}