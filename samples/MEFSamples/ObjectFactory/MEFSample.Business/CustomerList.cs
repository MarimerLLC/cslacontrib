using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Csla;
using CslaContrib.MEF;
using CslaContrib.MEF.Server;
using MEFSample.Business.FactoryInterfaces;

namespace MEFSample.Business
{
  [Serializable]
  [MefFactory(typeof(ICustomerFactory))]
  public class CustomerList :
    MefReadOnlyListBase<CustomerList, CustomerInfo>
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
