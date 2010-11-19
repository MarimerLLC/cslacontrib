using System;
using Csla.Core;

namespace MyCsla
{
  /// <summary>
  /// Adds posibbility of Logging/Instrumentation of DataPortal calls on BusinessObjects that does not use the ObjectFactoryAttribute
  /// 
  /// Consider f.ex the possiblity of creating PerformanceCounters and logging of Exceptions to EventLog.  
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <typeparam name="C"></typeparam>
  [Serializable]
  public class BusinessListBase<T, C> : Csla.BusinessListBase<T, C>
    where T : BusinessListBase<T, C>
    where C : IEditableBusinessObject
  {
  }
}