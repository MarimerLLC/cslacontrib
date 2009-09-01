using System;
using System.Diagnostics;
using Csla.Server;

namespace MyCsla.Server
{

  /// <summary>
  /// Adds posibbility of Logging/Instrumentation of DataPortal calls on classes using the ObjectFactoryAttribute
  /// 
  /// Consider f.ex the possiblity of creating PerformanceCounters and logging of Exceptions to EventLog.  
  /// 
  /// </summary>
  public class ObjectFactory : Csla.Server.ObjectFactory
  {
    protected void Invoke(DataPortalContext e)
    {
      Debug.Print("DataPortal Invoke object:{0}", e.FactoryInfo);
    }

    protected void InvokeComplete(DataPortalContext e)
    {
      Debug.Print("DataPortal InvokeCompleted object:{0}", e.FactoryInfo);
    }

    protected void InvkeError(Exception ex)
    {
      Debug.Print("DataPortal Exeption {0}", ex);
    }
  }
}