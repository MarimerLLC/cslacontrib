using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Csla;

namespace Templates
{
  [Serializable()]
  class $safeitemrootname$ : BusinessBase<$safeitemrootname$>
  {
    #region Business Methods

    // TODO: add your own fields, properties and methods
    private int _id;

    public int id
    {
      [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
      get 
      {
        CanReadProperty(true);
        return _id; 
      }
      [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
      set
      {
        CanWriteProperty(true);
        if (_id != value)
        {
          _id = value;
          PropertyHasChanged();
        }
      }
    }

    protected override object GetIdValue()
    {
      return _id;
    }

    #endregion

    #region Validation Rules

    protected override void AddBusinessRules()
    {
      // TODO: add validation rules
      //ValidationRules.AddRule(null, "");
    }

    #endregion

    #region Authorization Rules

    protected override void AddAuthorizationRules()
    {
      // TODO: add authorization rules
      //AuthorizationRules.AllowWrite("", "");
    }

    public static bool CanAddObject()
    {
      // TODO: customize to check user role
      //return ApplicationContext.User.IsInRole("");
      return true;
    }

    public static bool CanGetObject()
    {
      // TODO: customize to check user role
      //return ApplicationContext.User.IsInRole("");
      return true;
    }

    public static bool CanEditObject()
    {
      // TODO: customize to check user role
      //return ApplicationContext.User.IsInRole("");
      return true;
    }

    public static bool CanDeleteObject()
    {
      // TODO: customize to check user role
      //return ApplicationContext.User.IsInRole("");
      return true;
    }

    #endregion

    #region Factory Methods

    public static $safeitemrootname$ New$safeitemrootname$()
    {
      return DataPortal.Create<$safeitemrootname$>(
        new RootCriteria());
    }

    internal static $safeitemrootname$ NewSwitchableChild()
    {
      return DataPortal.Create<$safeitemrootname$>(
        new ChildCriteria());
    }

    public static $safeitemrootname$ GetSwitchableRoot(int id)
    {
      return DataPortal.Fetch<$safeitemrootname$>(
        new RootCriteria(id));
    }

    internal static $safeitemrootname$ GetSwitchableChild(
      SqlDataReader dr)
    {
      return new $safeitemrootname$(dr);
    }

    public static void Delete$safeitemrootname$(int id)
    {
      DataPortal.Delete(new RootCriteria(id));
    }

    private $safeitemrootname$()
    { /* Require use of factory methods */ }

    private $safeitemrootname$(SqlDataReader dr)
    {
      Fetch(dr);
    }

    #endregion

    #region Data Access

    [Serializable()]
    private class RootCriteria
    {
      private int _id;
      public int Id
      {
        get { return _id; }
      }
      public RootCriteria(int id)
      { _id = id; }
      public RootCriteria()
      { }
    }

    [Serializable()]
    private class ChildCriteria
    { }

    private void DataPortal_Create(RootCriteria criteria)
    {
      DoCreate();
    }

    private void DataPortal_Create(ChildCriteria criteria)
    {
      MarkAsChild();
      DoCreate();
    }

    private void DoCreate()
    {
      // TODO: load default values
    }

    private void DataPortal_Fetch(RootCriteria criteria)
    {
        using( SqlConnection cn = new SqlConnection( ConfigurationManager.ConnectionStrings[DATABASE_NAME].ConnectionString ) )
        {
            cn.Open();
            using( SqlCommand cm = cn.CreateCommand() )
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "usp$safeitemrootname$ByID";
                cm.Parameters.AddWithValue( "@ID", criteria.Id );
                using( SafeDataReader dr = new SafeDataReader( cm.ExecuteReader() ) )
                {
                    DoFetch(dr);
                }
            }
        }
    }

    private void Fetch(SafeDataReader dr)
    {
      MarkAsChild();
      DoFetch(dr);
    }

    private void DoFetch(SafeDataReader dr)
    {
      // TODO: load values
    }

    protected override void DataPortal_Insert()
    {
      // TODO: insert values
    }

    protected override void DataPortal_Update()
    {
      // TODO: update values
    }

    protected override void DataPortal_DeleteSelf()
    {
      DataPortal_Delete(new RootCriteria(_id));
    }

    private void DataPortal_Delete(RootCriteria criteria)
    {
      // TODO: delete values
    }

    #endregion
  }
}
