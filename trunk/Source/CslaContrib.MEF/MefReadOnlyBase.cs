using System;
#if NETFX_CORE
using System.Composition;
#else 
using System.ComponentModel.Composition;
#endif
using Csla;
using Csla.Serialization;

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
#if NETFX_CORE
        Ioc.Container.SatisfyImports(this);
#else
      Ioc.Container.SatisfyImportsOnce(this);
#endif
    }
  }
}
