using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Linq;
using Csla;
using MyCslaSample.Properties;

namespace MyCslaSample.Entities
{
  [Serializable]
  public class CountryCodesNameValueList : MyCsla.NameValueListBase<string, string>
  {
    #region Factory Methods

    private static CountryCodesNameValueList _list;

    public static CountryCodesNameValueList GetNameValueList()
    {
      if (_list == null)
        _list = DataPortal.Fetch<CountryCodesNameValueList>();
      return _list;
    }

    public static void InvalidateCache()
    {
      _list = null;
    }

    private CountryCodesNameValueList()
    { /* require use of factory methods */ }

    #endregion

    #region Data Access

    private void DataPortal_Fetch()
    {
      RaiseListChangedEvents = false;
      IsReadOnly = false;


      var countryCodes = Resources.CountryCodes.Split(new string[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
      foreach (var s in countryCodes)
      {
        var values = s.Split(',');
        Add(new NameValuePair(values[1], values[0]));
      }

      IsReadOnly = true;
      RaiseListChangedEvents = true;
    }

    #endregion
  }
}
