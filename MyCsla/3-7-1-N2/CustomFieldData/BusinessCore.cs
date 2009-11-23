using Csla;
using System;
using Csla.Core;

namespace CustomFieldData
{
  [Serializable]
  public abstract class BusinessCore<T>
    : MyCsla.BusinessBase<T>
    where T : BusinessCore<T>
  {
    protected BusinessCore()
      : base()
    {
    }

    protected override void DataPortal_OnDataPortalInvokeComplete(DataPortalEventArgs e)
    {
      base.DataPortal_OnDataPortalInvokeComplete(e);

      if (e.Operation == DataPortalOperations.Fetch)
      {
        this.MarkClean();
      }
    }

    public override bool IsDirty
    {
      get
      {
        return this.IsDeleted || this.FieldManager.IsDirty();
      }
    }

    public override bool IsSelfDirty
    {
      get
      {
        if (IsDeleted) return true;
        
        var isSelfDirty = false;

        foreach (var registeredProperty in this.FieldManager.GetRegisteredProperties())
        {
          var child = this.FieldManager.GetFieldData(registeredProperty).Value as ITrackStatus;
          // if value implements ITrackStatus it is a child/list object
          if (child == null && this.FieldManager.IsFieldDirty(registeredProperty))
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
