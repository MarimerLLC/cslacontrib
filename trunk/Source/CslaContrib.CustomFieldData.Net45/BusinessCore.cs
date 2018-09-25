using Csla;
using System;

namespace CslaContrib.CustomFieldData
{
  [Serializable]
  public abstract class BusinessCore<T> : BusinessBase<T>
        where T : BusinessCore<T>
  {
    protected BusinessCore()
        : base() { }

    protected override void Child_OnDataPortalInvokeComplete(DataPortalEventArgs e)
    {
      base.Child_OnDataPortalInvokeComplete(e);
      this.HandleOnDataPortalComplete(e);
    }

    protected override void DataPortal_OnDataPortalInvokeComplete(DataPortalEventArgs e)
    {
      base.DataPortal_OnDataPortalInvokeComplete(e);
      this.HandleOnDataPortalComplete(e);
    }

    private void HandleOnDataPortalComplete(DataPortalEventArgs e)
    {
      if (e.Operation == DataPortalOperations.Create)
      {
        this.MarkClean();
      }
    }

    public override bool IsSelfDirty
    {
      get
      {
        var isSelfDirty = false;

        foreach (var registeredProperty in this.FieldManager.GetRegisteredProperties())
        {
          if (registeredProperty.RelationshipType.HasFlag(RelationshipTypes.Child) &&
              this.FieldManager.IsFieldDirty(registeredProperty))
          {
            isSelfDirty = true;
            break;
          }
        }

        return isSelfDirty;
      }
    }
  }
}
