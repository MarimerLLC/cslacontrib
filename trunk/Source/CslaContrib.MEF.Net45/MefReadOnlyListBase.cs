﻿using System;
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
  public class MefReadOnlyListBase<T, C> : ReadOnlyListBase<T, C>
      where T : Csla.ReadOnlyListBase<T, C>
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

    /// <summary>
    /// Called when object is deserialized.
    /// </summary>
    protected override void OnDeserialized()
    {
      Inject();

      base.OnDeserialized();
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
