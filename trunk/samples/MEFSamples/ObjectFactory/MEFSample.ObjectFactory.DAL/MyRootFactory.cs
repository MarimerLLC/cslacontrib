using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Csla;
using Csla.Reflection;
using Csla.Server;
using MEFSample.Business;
using MEFSample.Business.FactoryInterfaces;
using MEFSample.ObjectFactoryDAL.ChildInterfaces;

namespace MEFSample.ObjectFactoryDAL
{
  /// <summary>
  /// 
  /// </summary>
  /// 
  [Export(typeof(IMyRootFactory))]
  public class MyRootFactory : ObjectFactory, IMyRootFactory
  {
    [RunLocal]
    public object Create()
    {
      var root = (MyRoot)MethodCaller.CreateInstance(typeof(MyRoot));


      using (BypassPropertyChecks(root))
      {
        root.Id = -1;
        root.Name = "New";
      }

      return root;
    }

    public object Fetch(object criteria)
    {
      var root = (MyRoot)MethodCaller.CreateInstance(typeof(MyRoot));

      using (BypassPropertyChecks(root))
      {
        root.Id = 2;
        root.Name = "Jonny";
      }

      return root;
    }
  }
}
