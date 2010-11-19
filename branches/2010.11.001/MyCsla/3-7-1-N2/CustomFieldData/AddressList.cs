using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Csla;

namespace CustomFieldData
{
  [Serializable]
  public class AddressList :
    BusinessListBase<AddressList, Address>
  {
    #region Factory Methods

    internal static AddressList NewAddressList()
    {
      return DataPortal.CreateChild<AddressList>();
    }

    internal static AddressList GetAddressList(
      object childData)
    {
      return DataPortal.FetchChild<AddressList>(childData);
    }

    private AddressList()
    { }

    #endregion

    #region Data Access

    private void Child_Fetch(object childData)
    {
      RaiseListChangedEvents = false;
      
      this.Add(Address.GetAddress(null));

      RaiseListChangedEvents = true;
    }

    #endregion
  }
}
