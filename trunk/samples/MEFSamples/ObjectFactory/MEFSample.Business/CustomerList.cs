using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Csla;
using Csla.Server;
using CslaContrib.MEF;
using CslaContrib.MEF.Server;
using MEFSample.Business.FactoryInterfaces;
using DataPortal = Csla.DataPortal;

namespace MEFSample.Business
{
  [Serializable]
  [ObjectFactory(typeof(ICustomerFactory))]
  public class CustomerList :
    ReadOnlyBindingListBase<CustomerList, CustomerInfo>
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

  }
}
