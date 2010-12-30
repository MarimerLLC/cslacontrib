using System.ComponentModel.Composition;
using Csla;

namespace CslaContrib.MEF
{
  public class MefCommandBase<T> : CommandBase<T> where T: CommandBase<T>
  {
    protected override void DataPortal_OnDataPortalInvoke(DataPortalEventArgs e)
    {
      //inject dependencies into instance 
      Inject();

      //call base class
      base.DataPortal_OnDataPortalInvoke(e);
    }

    protected override void OnDeserialized(System.Runtime.Serialization.StreamingContext context)
    {
      Inject();

      base.OnDeserialized(context);
    }

    private void Inject()
    {
      Ioc.Container.ComposeParts(this);
    }
  }
}
