using System.ComponentModel.Composition;
using Csla;

namespace CslaContrib.MEF
{
  public class MefDynamicListBase<T> : Csla.DynamicListBase<T> where T : Csla.Core.IEditableBusinessObject, Csla.Core.IUndoableObject, Csla.Core.ISavable, Csla.Serialization.Mobile.IMobileObject
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
      Ioc.Container.ComposeParts(this);
    }
  }
}
