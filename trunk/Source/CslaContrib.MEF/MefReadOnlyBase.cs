using System;
using System.ComponentModel.Composition;
using Csla;
#if SILVERLIGHT
using Csla.Serialization;
#endif

namespace CslaContrib.MEF
{
  [Serializable]
  public class MefReadOnlyBase<T> : ReadOnlyBase<T> where T: ReadOnlyBase<T>
  {
    protected override void DataPortal_OnDataPortalInvoke(DataPortalEventArgs e)
    {
      //inject dependencies into instance 
      Inject();

      //call base class
      base.DataPortal_OnDataPortalInvoke(e);
    }

    protected override void Child_OnDataPortalInvoke(DataPortalEventArgs e)
    {
      //inject dependencies into instance 
      Inject();

      //call base class
      base.Child_OnDataPortalInvoke(e);
    }

    protected override void OnDeserialized(System.Runtime.Serialization.StreamingContext context)
    {
      Inject();

      base.OnDeserialized(context);
    }

    private void Inject()
    {
      Ioc.Container.SatisfyImportsOnce(this);
    }
  }
}
