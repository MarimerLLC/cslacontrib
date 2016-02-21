using System;
using System.Collections.Generic;
using System.Text;
using Csla;
using Csla.Rules.CommonRules;
using CslaContrib.Rules.CommonRules;

namespace CslaContrib.UnitTests.Rules
{
  [Serializable]
  public class AnyRequiredRoot : BusinessBase<AnyRequiredRoot>
  {
    #region Business Methods

    public static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(c => c.Name);
    public string Name
    {
      get { return GetProperty(NameProperty); }
      set { SetProperty(NameProperty, value); }
    }

    public static readonly PropertyInfo<SmartDate> DateProperty = RegisterProperty<SmartDate>(c => c.Date, null, new SmartDate());
    public string Date
    {
      get { return GetPropertyConvert<SmartDate, string>(DateProperty); }
      set { SetPropertyConvert<SmartDate, string>(DateProperty, value); }
    }

    public static readonly PropertyInfo<int> NumberProperty = RegisterProperty<int>(c => c.Number);
    public int Number
    {
      get { return GetProperty(NumberProperty); }
      set { SetProperty(NumberProperty, value); }
    }
    #endregion

    #region Validation Rules

    protected override void AddBusinessRules()
    {
      // TODO: add validation rules
      BusinessRules.AddRule(new AnyRequired(NameProperty, DateProperty, NumberProperty));
      BusinessRules.AddRule(new Dependency(DateProperty, NameProperty));
      BusinessRules.AddRule(new Dependency(NumberProperty, NameProperty));
    }

    #endregion

    #region Factory Methods

    public static AnyRequiredRoot NewEditableRoot()
    {
      return DataPortal.Create<AnyRequiredRoot>();
    }


    public AnyRequiredRoot()
    { /* Require use of factory methods */ }

    #endregion

    #region Data Access

    [RunLocal]
    protected override void DataPortal_Create()
    {
      // calls check rules 
      base.DataPortal_Create();
    }

    #endregion
  }
}
