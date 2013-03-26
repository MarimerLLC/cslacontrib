using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MEFSample.Business.FactoryInterfaces
{
  public interface ICustomerFactory
  {
    object Fetch(string criteria);
  }
}
