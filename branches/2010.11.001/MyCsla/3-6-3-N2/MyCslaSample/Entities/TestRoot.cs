using System;
using System.Collections.Generic;
using System.Text;
using Csla;
using Csla.Validation;
using MyCsla.Validation;

namespace MyCslaSample.Entities
{
  [Serializable]
  public class TestRoot : MyCsla.BusinessBase<TestRoot>
  {
    #region Business Methods

    public static readonly PropertyInfo<string> NameProperty = RegisterProperty(new PropertyInfo<string>("Name", "Name", string.Empty));
    public string Name
    {
      get { return GetProperty<string>(NameProperty); }
      set { SetProperty<string>(NameProperty, value); }
    }

    public static readonly PropertyInfo<string> Address1Property = RegisterProperty(new PropertyInfo<string>("Address1", "Address1", string.Empty));
    public string Address1
    {
      get { return GetProperty<string>(Address1Property); }
      set { SetProperty<string>(Address1Property, value); }
    }

    public static readonly PropertyInfo<decimal> SalaryProperty = RegisterProperty(new PropertyInfo<decimal>("Salary", "Salary"));
    public decimal Salary
    {
      get { return GetProperty<decimal>(SalaryProperty); }
      set { SetProperty<decimal>(SalaryProperty, value); }
    }

    public static readonly PropertyInfo<int> CustomerTypeProperty = RegisterProperty(new PropertyInfo<int>("CustomerType", "CustomerType", 1));
    public int CustomerType
    {
      get { return GetProperty<int>(CustomerTypeProperty); }
      set { SetProperty<int>(CustomerTypeProperty, value); }
    }

    public static readonly PropertyInfo<SmartDate> FoundedProperty = RegisterProperty(new PropertyInfo<SmartDate>("Founded", "Founded", new SmartDate(true)));
    public string Founded
    {
      get { return GetPropertyConvert<SmartDate, string>(FoundedProperty); }
      set { SetPropertyConvert<SmartDate, string>(FoundedProperty, value); }
    }

    public static readonly PropertyInfo<string> CountryCodeProperty = RegisterProperty(new PropertyInfo<string>("CountryCode", "CountryCode", "US"));
    public string CountryCode
    {
      get { return GetProperty<string>(CountryCodeProperty); }
      set { SetProperty<string>(CountryCodeProperty, value); }
    }

    public static readonly PropertyInfo<bool> OtherAddressProperty = RegisterProperty(new PropertyInfo<bool>("OtherAddress", "OtherAddress"));
    public bool OtherAddress
    {
      get { return GetProperty<bool>(OtherAddressProperty); }
      set { SetProperty<bool>(OtherAddressProperty, value); }
    }


    public static readonly PropertyInfo<string> OtherAddress1Property = RegisterProperty(new PropertyInfo<string>("OtherAddress1", "OtherAddress1"));
    public string OtherAddress1
    {
      get { return GetProperty<string>(OtherAddress1Property); }
      set { SetProperty<string>(OtherAddress1Property, value); }
    }

    public static readonly PropertyInfo<string> OtherAddress2Property = RegisterProperty(new PropertyInfo<string>("OtherAddress2", "OtherAddress2"));
    public string OtherAddress2
    {
      get { return GetProperty<string>(OtherAddress2Property); }
      set { SetProperty<string>(OtherAddress2Property, value); }
    }

    #endregion

    #region Validation Rules

    protected override void AddBusinessRules()
    {
      ValidationRules.AddRule(CommonRules.StringRequired, NameProperty);
      ValidationRules.AddRule(CommonRules.StringMaxLength,
                              new CommonRules.MaxLengthRuleArgs(NameProperty, 30, "Is Name too long here? {0}")
                                {
                                  Severity = RuleSeverity.Information,
                                });
      ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs(NameProperty, 50));
      ValidationRules.AddRule(CommonRules.MaxValue<decimal>, new CommonRules.MaxValueRuleArgs<decimal>(SalaryProperty, 100000)
                                                      {
                                                        Severity = RuleSeverity.Warning
                                                      });
      ValidationRules.AddRule(CommonRules.MaxValue<decimal>, new CommonRules.MaxValueRuleArgs<decimal>(SalaryProperty, 200000));

      ValidationRules.AddRule(MyCommonRules.StopIfNotCanWrite, OtherAddress1Property, 0);
      ValidationRules.AddRule(CommonRules.StringRequired, OtherAddress1Property.Name, 1);
      ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs(OtherAddress1Property, 50), 2);

      ValidationRules.AddRule(MyCommonRules.StopIfNotCanWrite, OtherAddress2Property, 0);
      ValidationRules.AddRule(CommonRules.StringRequired, OtherAddress2Property, 1);
      ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs(OtherAddress2Property, 50), 2);

      // dependent properties 
      ValidationRules.AddDependentProperty(OtherAddressProperty, OtherAddress1Property);
      ValidationRules.AddDependentProperty(OtherAddressProperty, OtherAddress2Property);
    }

    #endregion

    #region Authorization Rules

    protected override void AddAuthorizationRules()
    {
      // TODO: add authorization rules
      //AuthorizationRules.AllowWrite("Name", "Role");
    }

    private static void AddObjectAuthorizationRules()
    {
      // TODO: add authorization rules
      //AuthorizationRules.AllowEdit(typeof(TestRoot), "Role");
    }


    public override bool CanWriteProperty(string propertyName)
    {
      if (!base.CanWriteProperty(propertyName)) return false;

      if ((propertyName == OtherAddress1Property.Name) ||
          (propertyName == OtherAddress2Property.Name))
      {
        return ReadProperty(OtherAddressProperty);
      }
      return true;
    }

    #endregion

    #region Factory Methods

    public static TestRoot NewEditableRoot()
    {
      return DataPortal.Create<TestRoot>();
    }

    public static TestRoot GetEditableRoot(int id)
    {
      return DataPortal.Fetch<TestRoot>(
        new SingleCriteria<TestRoot, int>(id));
    }

    public static void DeleteEditableRoot(int id)
    {
      DataPortal.Delete(new SingleCriteria<TestRoot, int>(id));
    }

    private TestRoot()
    { /* Require use of factory methods */ }

    #endregion

    #region Data Access

    [RunLocal]
    protected override void DataPortal_Create()
    {
      // TODO: load default values
      // omit this override if you have no defaults to set
      base.DataPortal_Create();
      using (BypassPropertyChecks)
      {
        Name = NameProperty.DefaultValue;
        Address1 = Address1Property.DefaultValue;
        CustomerType = CustomerTypeProperty.DefaultValue;
        CountryCode = CountryCodeProperty.DefaultValue;
      }
    }

    private void DataPortal_Fetch(SingleCriteria<TestRoot, int> criteria)
    {
      // TODO: load values
    }

    [Transactional(TransactionalTypes.TransactionScope)]
    protected override void DataPortal_Insert()
    {
      // TODO: insert values
    }

    [Transactional(TransactionalTypes.TransactionScope)]
    protected override void DataPortal_Update()
    {
      // TODO: update values
    }

    [Transactional(TransactionalTypes.TransactionScope)]
    private void DataPortal_Delete(SingleCriteria<TestRoot, int> criteria)
    {
      // TODO: delete values
    }

    #endregion
  }
}
