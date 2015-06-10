using System;
using Csla;
using CslaContrib.MEF;
using MEFSample.Business.Repository;

namespace MEFSample.Business
{
  [Serializable]
  public class CustomerInfo : MefReadOnlyBase<CustomerInfo>
  {
    #region Business Methods

    // TODO: add your own fields, properties and methods
    // use snippet cslapropg to create your properties

    // example with managed backing field
    private static readonly PropertyInfo<int> IdProperty = RegisterProperty<int>(p => p.Id);
    public int Id
    {
      get { return GetProperty(IdProperty); }
    }

    // example with private backing field
    private static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(p => p.Name);
    public string Name
    {
      get { return GetProperty(NameProperty); }
    }

    #endregion

    #region Factory Methods

    internal static CustomerInfo GetCustomerInfo(object childData)
    {
      return DataPortal.FetchChild<CustomerInfo>(childData);
    }

    private CustomerInfo()
    { /* require use of factory methods */ }

    #endregion

    #region Data Access

    private void Child_Fetch(object childData)
    {
      var data = (CustomerData)childData;

      LoadProperty(IdProperty, data.Id);
      LoadProperty(NameProperty, data.Name);
    }

    #endregion
  }
}
