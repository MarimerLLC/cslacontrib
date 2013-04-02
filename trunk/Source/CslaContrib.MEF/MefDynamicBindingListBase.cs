using System;
using System.ComponentModel.Composition;
using Csla;
#if SILVERLIGHT
using Csla.Serialization;
#endif

namespace CslaContrib.MEF
{
  [Serializable]
  public class MefDynamicBindingListBase<T> : Csla.DynamicBindingListBase<T> where T : Csla.Core.IEditableBusinessObject, Csla.Core.IUndoableObject, Csla.Core.ISavable, Csla.Serialization.Mobile.IMobileObject
  {
    protected override void DataPortal_OnDataPortalInvoke(DataPortalEventArgs e)
    {
      //inject dependencies into instance 
      Inject();

      //call base class
      base.DataPortal_OnDataPortalInvoke(e);
    }

    protected override void OnDeserialized()
    {
      Inject();

      base.OnDeserialized();
    }

    private void Inject()
    {
      Ioc.Container.SatisfyImportsOnce(this);
    }
  }
}
