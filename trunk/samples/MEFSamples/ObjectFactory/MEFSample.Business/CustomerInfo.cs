using System;
using Csla;

namespace MEFSample.Business
{
  [Serializable]
  public class CustomerInfo : ReadOnlyBase<CustomerInfo>
  {
    #region Business Methods

    // TODO: add your own fields, properties and methods
    // use snippet cslapropg to create your properties

    // example with managed backing field
    public static readonly PropertyInfo<int> IdProperty = RegisterProperty<int>(p => p.Id);
    public int Id
    {
      get { return GetProperty(IdProperty); }
    }

    // example with private backing field
    public static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(p => p.Name);
    public string Name
    {
      get { return GetProperty(NameProperty); }
    }

    #endregion

    #region Factory Methods

    private CustomerInfo()
    { /* require use of factory methods */ }

    #endregion

  }
}
