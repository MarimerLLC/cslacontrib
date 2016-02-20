using System;
using Csla;
using Csla.Rules.CommonRules;
using CslaContrib.Rules.CommonRules;

namespace CslaContrib.UnitTests.Rules
{
  [Serializable]
  public class RuleTestRoot : BusinessBase<RuleTestRoot>
  {
    #region Business Methods

    public static readonly PropertyInfo<int> Num1Property = RegisterProperty<int>(c => c.Num1);
    /// <Summary>
    /// Gets or sets the Num1 value.
    /// </Summary>
    public int Num1
    {
      get { return GetProperty(Num1Property); }
      set { SetProperty(Num1Property, value); }
    }

    public static readonly PropertyInfo<int> Num2Property = RegisterProperty<int>(c => c.Num2);
    /// <Summary>
    /// Gets or sets the Num2 value.
    /// </Summary>
    public int Num2
    {
      get { return GetProperty(Num2Property); }
      set { SetProperty(Num2Property, value); }
    }

    public static readonly PropertyInfo<int> Num3Property = RegisterProperty<int>(c => c.Num3);
    /// <Summary>
    /// Gets or sets the Num3 value.
    /// </Summary>
    public int Num3
    {
      get { return GetProperty(Num3Property); }
      set { SetProperty(Num3Property, value); }
    }

    public static readonly PropertyInfo<int> Num4Property = RegisterProperty<int>(c => c.Num4);
    /// <Summary>
    /// Gets or sets the Num4 value.
    /// </Summary>
    public int Num4
    {
      get { return GetProperty(Num4Property); }
      set { SetProperty(Num4Property, value); }
    }

    public static readonly PropertyInfo<int> Num5Property = RegisterProperty<int>(c => c.Num5);
    /// <Summary>
    /// Gets or sets the Num5 value.
    /// </Summary>
    public int Num5
    {
      get { return GetProperty(Num5Property); }
      set { SetProperty(Num5Property, value); }
    }

    public static readonly PropertyInfo<int> Num6Property = RegisterProperty<int>(c => c.Num6);
    /// <Summary>
    /// Gets or sets the Num6 value.
    /// </Summary>
    public int Num6
    {
      get { return GetProperty(Num6Property); }
      set { SetProperty(Num6Property, value); }
    }

    public static readonly PropertyInfo<string> Str1Property = RegisterProperty<string>(c => c.Str1, null, "US");
    public string Str1
    {
      get { return GetProperty(Str1Property); }
      set { SetProperty(Str1Property, value); }
    }

    public static readonly PropertyInfo<int> SumProperty = RegisterProperty<int>(c => c.Sum);
    /// <Summary>
    /// Gets or sets the Sum value.
    /// </Summary>
    public int Sum
    {
      get { return GetProperty(SumProperty); }
      set { SetProperty(SumProperty, value); }
    }

    public static readonly PropertyInfo<string> LowerProperty = RegisterProperty<string>(c => c.Lower);
    /// <Summary>
    /// Gets or sets the Lower value.
    /// </Summary>
    public string Lower
    {
      get { return GetProperty(LowerProperty); }
      set { SetProperty(LowerProperty, value); }
    }

    public static readonly PropertyInfo<string> UpperProperty = RegisterProperty<string>(c => c.Upper);
    /// <Summary>
    /// Gets or sets the Upper value.
    /// </Summary>
    public string Upper
    {
      get { return GetProperty(UpperProperty); }
      set { SetProperty(UpperProperty, value); }
    }
    #endregion

    #region Business Rules

    protected override void AddBusinessRules()
    {

      base.AddBusinessRules();

      BusinessRules.AddRule(new LessThan(Num1Property, Num5Property));
      BusinessRules.AddRule(new LessThanOrEqual(Num2Property, Num5Property));
      BusinessRules.AddRule(new GreaterThan(Num3Property, Num6Property));
      BusinessRules.AddRule(new GreaterThanOrEqual(Num4Property, Num6Property));
      BusinessRules.AddRule(new ToUpperCase(UpperProperty));
      BusinessRules.AddRule(new ToLowerCase(LowerProperty));
      BusinessRules.AddRule(new CalcSum(SumProperty, Num1Property, Num2Property, Num3Property, Num4Property));
      BusinessRules.AddRule(new Required(Str1Property, () => "My error message {0}"));
    }

    private static void AddObjectAuthorizationRules()
    {
      // TODO: add authorization rules
      //BusinessRules.AddRule(...);
    }

    #endregion

    #region Factory Methods

    public static RuleTestRoot NewEditableRoot()
    {
      return DataPortal.Create<RuleTestRoot>();
    }

    public RuleTestRoot()
    { /* Require use of factory methods */ }

    #endregion

    #region Data Access

    [RunLocal]
    protected override void DataPortal_Create()
    {
    }

    #endregion
  }
}
