using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Csla;

namespace CustomFieldData
{
  [Serializable]
  public class Address : BusinessCore<Address>
  {
    #region Business Methods

    public static readonly PropertyInfo<string> Address1Property = RegisterProperty<string>("Address1", "Address1");
    public string Address1
    {
      get { return GetProperty<string>(Address1Property); }
      set { SetProperty<string>(Address1Property, value); }
    }

    public static readonly PropertyInfo<string> Address2Property = RegisterProperty<string>("Address2", "Name");
    public string Address2
    {
      get { return GetProperty<string>(Address2Property); }
      set { SetProperty<string>(Address2Property, value); }
    }

    #endregion

    #region Validation Rules

    protected override void AddBusinessRules()
    {
      // TODO: add validation rules
      //ValidationRules.AddRule(RuleMethod, "");
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
      //AuthorizationRules.AllowEdit(typeof(Address), "Role");
    }

    #endregion

    #region Factory Methods

    internal static Address NewAddress()
    {
      return DataPortal.CreateChild<Address>();
    }

    internal static Address GetAddress(object childData)
    {
      return DataPortal.FetchChild<Address>(childData);
    }

    private Address()
    { /* Require use of factory methods */ }

    #endregion

    #region Data Access

    protected override void Child_Create()
    {
      // TODO: load default values
      // omit this override if you have no defaults to set
      base.Child_Create();
    }

    private void Child_Fetch(object childData)
    {
      using (BypassPropertyChecks)
      {
        this.Address1 = "Test address1";
        this.Address2 = "Test address2";
      }
    }

    private void Child_Insert(object parent)
    {
      // TODO: insert values
    }

    private void Child_Update(object parent)
    {
      // TODO: update values
    }

    private void Child_DeleteSelf(object parent)
    {
      // TODO: delete values
    }

    #endregion
  }
}
