using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Csla;
using CslaContrib.MEF;
using MEFSample.Business.Repository;

namespace MEFSample.Business
{
  [Serializable]
  public class CustomerList :
    MefReadOnlyBindingListBase<CustomerList, CustomerInfo>
  {
    #region Factory Methods

    public static CustomerList GetReadOnlyList(string filter)
    {
      return DataPortal.Fetch<CustomerList>(filter);
    }

    public static void  BeginGetReadOnlyList(string filter, EventHandler<DataPortalResult<CustomerList>> callback)
    {
      DataPortal.BeginFetch<CustomerList>(filter, callback);
    }


    private CustomerList()
    { /* require use of factory methods */ }

    #endregion

    #region Injected properties - must have private field marked was NonSerialized and NotUndoable

    [NonSerialized, NotUndoable]
    private ICustomerDataAccess _myDataAccess;

    [Import(typeof(ICustomerDataAccess))]
    public ICustomerDataAccess MyDataAccess
    {
      get { return _myDataAccess; }
      set { _myDataAccess = value; }
    }

    #endregion

    #region Data Access
    public void DataPortal_Fetch(string criteria)
    {
      RaiseListChangedEvents = false;
      IsReadOnly = false;

      var data = MyDataAccess.Get(criteria);

      foreach (var child in data)
        Add(CustomerInfo.GetCustomerInfo(child));

      IsReadOnly = true;
      RaiseListChangedEvents = true;
    }
    #endregion
  }
}
