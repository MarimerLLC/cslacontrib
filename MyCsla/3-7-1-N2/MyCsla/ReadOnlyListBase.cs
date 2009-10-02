using System;

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
  public class ReadOnlyListBase<T, C> : Csla.ReadOnlyListBase<T, C> where T : ReadOnlyListBase<T, C>
  {
  }
}