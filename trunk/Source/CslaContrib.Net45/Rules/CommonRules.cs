using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Csla.Core;
using Csla.Rules;
using Csla.Rules.CommonRules;

namespace CslaContrib.Rules.CommonRules
{
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
      RuleUri.AddQueryParameter("compareto", compareToProperty.Name);
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
      MessageDelegate = errorMessageDelegate;
    }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    /// <value></value>
    protected override string GetMessage()
    {
      return HasMessageDelegate ? base.GetMessage() : CslaContrib.Properties.Resources.LessThanRule;
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
        var message = string.Format(GetMessage(), PrimaryProperty.FriendlyName, CompareTo.FriendlyName);
        context.Results.Add(new RuleResult(RuleName, PrimaryProperty, message) { Severity = Severity });
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
      RuleUri.AddQueryParameter("compareto", compareToProperty.Name);
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
      MessageDelegate = errorMessageDelegate;
    }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    /// <value></value>
    protected override string GetMessage()
    {
      return HasMessageDelegate ? base.GetMessage() : CslaContrib.Properties.Resources.LessThanOrEqualRule;
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
        var message = string.Format(GetMessage(), PrimaryProperty.FriendlyName, CompareTo.FriendlyName);
        context.Results.Add(new RuleResult(RuleName, PrimaryProperty, message) { Severity = Severity });
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
      RuleUri.AddQueryParameter("compareto", compareToProperty.Name);
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
      MessageDelegate = errorMessageDelegate;
    }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    /// <value></value>
    protected override string GetMessage()
    {
      return HasMessageDelegate ? base.GetMessage() : CslaContrib.Properties.Resources.GreaterThanRule;
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
        var message = string.Format(GetMessage(), PrimaryProperty.FriendlyName, CompareTo.FriendlyName);
        context.Results.Add(new RuleResult(RuleName, PrimaryProperty, message) { Severity = Severity });
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
      RuleUri.AddQueryParameter("compareto", compareToProperty.Name);
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
      MessageDelegate = errorMessageDelegate;
    }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    /// <value></value>
    protected override string GetMessage()
    {
      return HasMessageDelegate ? base.GetMessage() : CslaContrib.Properties.Resources.GreaterThanOrEqualRule;
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
        var message = string.Format(GetMessage(), PrimaryProperty.FriendlyName, CompareTo.FriendlyName);
        context.Results.Add(new RuleResult(RuleName, PrimaryProperty, message) { Severity = Severity });
      }
    }
  }

  /// <summary>
  /// Business rule for check a value is between a minimum and a maximum.
  /// </summary>
  public class Range : CommonBusinessRule
  {
    /// <summary>
    /// Gets the minimum value.
    /// </summary>
    public IComparable Min { get; private set; }

    /// <summary>
    /// Gets the maximum value.
    /// </summary>
    public IComparable Max { get; private set; }

    /// <summary>
    /// Gets or sets the format string used
    /// to format the minimum and maximum values.
    /// </summary>
    public string Format { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Range"/> class.
    /// Creates an instance of the rule.
    /// </summary>
    /// <param name="primaryProperty">
    /// Property to which the rule applies.
    /// </param>
    /// <param name="min">
    /// Min value.
    /// </param>
    /// <param name="max">
    /// Max value.
    /// </param>
    public Range(IPropertyInfo primaryProperty, IComparable min, IComparable max)
      : base(primaryProperty)
    {
      Max = max;
      Min = min;
      RuleUri.AddQueryParameter("max", max.ToString());
      RuleUri.AddQueryParameter("min", min.ToString());
      InputProperties = new List<IPropertyInfo> { primaryProperty };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Range"/> class.
    /// Creates an instance of the rule.
    /// </summary>
    /// <param name="primaryProperty">
    /// Property to which the rule applies.
    /// </param>
    /// <param name="min">
    /// Min value.
    /// </param>
    /// <param name="max">
    /// Max value.
    /// </param>
    /// <param name="errorMessageDelegate">
    /// The message delegate.
    /// </param>
    public Range(IPropertyInfo primaryProperty, IComparable min, IComparable max, Func<string> errorMessageDelegate)
      : this(primaryProperty, min, max)
    {
      MessageDelegate = errorMessageDelegate;
    }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    /// <value>
    /// </value>
    /// <returns>
    /// The get message.
    /// </returns>
    protected override string GetMessage()
    {
      return HasMessageDelegate ? base.GetMessage() : CslaContrib.Properties.Resources.RangeRule;
    }

    /// <summary>
    /// Rule implementation.
    /// </summary>
    /// <param name="context">
    /// Rule context.
    /// </param>
    protected override void Execute(RuleContext context)
    {
      var value = (IComparable)context.InputPropertyValues[PrimaryProperty];
      var minResult = value.CompareTo(Min);
      var maxResult = value.CompareTo(Max);

      if ((minResult <= -1) || (maxResult >= 1))
      {
        var minOutValue = string.IsNullOrEmpty(Format) ? Min.ToString() : string.Format(string.Format("{{0:{0}}}", Format), Min);
        var maxOutValue = string.IsNullOrEmpty(Format) ? Max.ToString() : string.Format(string.Format("{{0:{0}}}", Format), Max);

        var message = string.Format(GetMessage(), PrimaryProperty.FriendlyName, minOutValue, maxOutValue);
        context.Results.Add(new RuleResult(RuleName, PrimaryProperty, message) { Severity = Severity });
      }
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
      dynamic sum = context.InputPropertyValues.Aggregate<KeyValuePair<IPropertyInfo, object>, dynamic>(0, (current, item) => current + (dynamic)item.Value);

      // add calculated value to OutValues
      // When rule is completed the RuleEngine will update businessobject
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

  #region Property rules

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
      MessageDelegate = errorMessageDelegate;
    }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    /// <value></value>
    protected override string GetMessage()
    {
      return HasMessageDelegate ? base.GetMessage() : CslaContrib.Properties.Resources.AnyRequiredRule;
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

      var message = string.Format(GetMessage(), fieldNames);
      context.Results.Add(new RuleResult(RuleName, PrimaryProperty, message) { Severity = Severity });
    }
  }

  #endregion

  #region List rules

  /// <summary>
  /// Validation rule for checking a property is unique at the collection level.
  /// </summary>
  public class NoDuplicates<T, C> : CommonBusinessRule
    where T : System.Collections.ObjectModel.Collection<C>
    where C : Csla.BusinessBase<C>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="NoDuplicates{T, C}"/> class.
    /// </summary>
    /// <param name="primaryProperty">Primary property for this rule.</param>
    public NoDuplicates(IPropertyInfo primaryProperty)
      : base(primaryProperty)
    {
      InputProperties = new List<IPropertyInfo> { primaryProperty };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NoDuplicates{T, C}"/> class.
    /// </summary>
    /// <param name="primaryProperty">Primary property for this rule.</param>
    /// <param name="errorMessageDelegate">The error message function.</param>
    public NoDuplicates(IPropertyInfo primaryProperty, string errorMessageDelegate)
      : this(primaryProperty)
    {
      MessageDelegate = () => errorMessageDelegate;
    }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    /// <value></value>
    protected override string GetMessage()
    {
        return HasMessageDelegate ? base.GetMessage() : CslaContrib.Properties.Resources.NoDuplicatesRule;
    }

    /// <summary>
    /// Validation rule implementation.
    /// </summary>
    /// <param name="context">Rule context object.</param>
    protected override void Execute(RuleContext context)
    {
      var o = context.InputPropertyValues[PrimaryProperty];
      if (o == null)
        return;

      var value = Convert.ToString(o);

      var target = (C)context.Target;
      var parent = (T)target.Parent;
      if (parent != null)
      {
        if (parent.Any(item => value.Equals(Convert.ToString(ReadProperty(item, PrimaryProperty)), StringComparison.InvariantCultureIgnoreCase)
                               && !(ReferenceEquals(item, target))))
        {
          var message = string.Format(GetMessage(), InputProperties[0].FriendlyName);
          context.Results.Add(new RuleResult(RuleName, PrimaryProperty, message));
        }
      }
    }
  }

  #endregion
}