using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.DataAnnotations;
using Csla;
using Csla.Rules.CommonRules;
using Csla.Server;
using CslaContrib.Rules.CommonRules;
using MEFSample.Business.FactoryInterfaces;
using DataPortal = Csla.DataPortal;


namespace MEFSample.Business
{
  [Serializable]
  [ObjectFactory(typeof(IMyRootFactory))]
  public class MyRoot : BusinessBase<MyRoot>
  {
    #region Business Methods

    public static readonly PropertyInfo<int> IdProperty = RegisterProperty<int>(c => c.Id);
    /// <Summary>
    /// Gets or sets the Id value.
    /// </Summary>
    public int Id
    {
      get { return GetProperty(IdProperty); }
      set { SetProperty(IdProperty, value); }
    }

    public static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name);
    [Required]   // Data Annotations rule for Required field
    public string Name
    {
      get { return GetProperty(NameProperty); }
      set { SetProperty(NameProperty, value); }
    }

    public static readonly PropertyInfo<int> Num1Property = RegisterProperty<int>(c => c.Num1);
    public int Num1
    {
      get { return GetProperty(Num1Property); }
      set { SetProperty(Num1Property, value); }
    }

    [Range(1, 6000)]
    public static readonly PropertyInfo<int> Num2Property = RegisterProperty<int>(c => c.Num2);
    public int Num2
    {
      get { return GetProperty(Num2Property); }
      set { SetProperty(Num2Property, value); }
    }

    public static readonly PropertyInfo<int> SumProperty = RegisterProperty<int>(c => c.Sum);
    public int Sum
    {
      get { return GetProperty(SumProperty); }
      set { SetProperty(SumProperty, value); }
    }

    #endregion

    #region Validation Rules

    protected override void AddBusinessRules()
    {
      // call base class implementation to add data annotation rules to BusinessRules 
      base.AddBusinessRules();

      BusinessRules.AddRule(new MaxValue<int>(Num1Property, 5000));
      BusinessRules.AddRule(new LessThan(Num1Property, Num2Property));

      // calculates sum rule - must alwas un before MinValue with lower priority
      BusinessRules.AddRule(new CalcSum(SumProperty, Num1Property, Num2Property) { Priority = -1 });
      BusinessRules.AddRule(new MinValue<int>(SumProperty, 1));

      // Name Property
      //BusinessRules.AddRule(new Required(NameProperty));
      BusinessRules.AddRule(new MaxLength(NameProperty, 10));
    }

    #endregion

    #region Factory Methods

    public static MyRoot NewEditableRoot()
    {
      return DataPortal.Create<MyRoot>();
    }

    public static MyRoot GetRoot()
    {
      return DataPortal.Fetch<MyRoot>(null);
    }

    private MyRoot() { }

    #endregion

  }
}
