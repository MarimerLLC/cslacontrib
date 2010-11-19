using System;

namespace MyCsla
{
  /// <summary>
  /// Adds posibbility of Logging/Instrumentation of DataPortal calls on BusinessObjects that does not use the ObjectFactoryAttribute
  /// 
  /// Consider f.ex the possiblity of creating PerformanceCounters and logging of Exceptions to EventLog.  
  /// </summary>
  /// <typeparam name="K"></typeparam>
  /// <typeparam name="V"></typeparam>
  [Serializable]
  public class NameValueListBase<K, V> : Csla.NameValueListBase<K, V>
  {
  }
}