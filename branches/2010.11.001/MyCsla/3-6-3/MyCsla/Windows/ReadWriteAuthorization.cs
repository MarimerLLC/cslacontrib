using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MyCsla.Windows
{
  public class ReadWriteAuthorization : Csla.Windows.ReadWriteAuthorization
  {
    public ReadWriteAuthorization(IContainer container) : base(container)
    {
    }
  }
}
