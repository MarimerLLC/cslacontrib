using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using MyCsla;
using Csla;

namespace MyCslaSample.Entities
{
  [Serializable]
  public class CustomerTypeNameValueList : MyCsla.NameValueListBase<int, string>
  {
    #region Factory Methods

    private static CustomerTypeNameValueList _list;

    public static CustomerTypeNameValueList GetNameValueList()
    {
      if (_list == null)
        _list = DataPortal.Fetch<CustomerTypeNameValueList>();
      return _list;
    }

    public static void InvalidateCache()
    {
      _list = null;
    }

    private CustomerTypeNameValueList()
    { /* require use of factory methods */ }

    #endregion

    #region Data Access

    private void DataPortal_Fetch()
    {
      RaiseListChangedEvents = false;
      IsReadOnly = false;

      Add(new NameValuePair(1, "Corporate"));
      Add(new NameValuePair(2, "Personal"));

      IsReadOnly = true;
      RaiseListChangedEvents = true;
    }

    #endregion
  }
}
