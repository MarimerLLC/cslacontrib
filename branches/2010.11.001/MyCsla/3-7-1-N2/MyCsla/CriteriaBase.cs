using System;
using Csla;

namespace MyCsla
{
  /// <summary>
  /// Adds posibbility of Logging/Instrumentation of DataPortal calls on BusinessObjects that does not use the ObjectFactoryAttribute
  /// 
  /// Consider f.ex the possiblity of creating PerformanceCounters and logging of Exceptions to EventLog.  
  /// </summary>
  [Serializable]
  public class CriteriaBase : Csla.CriteriaBase
  {
    /// <summary>
    /// Indicates that the specified property belongs
    /// to the business object type.
    /// </summary>
    /// <typeparam name="P">Type of property</typeparam>
    /// <param name="objectType">Type of the object.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <returns>The provided IPropertyInfo object.</returns>
    protected static PropertyInfo<P> RegisterProperty<P>(Type objectType, string propertyName)
    {
      return RegisterProperty(objectType, Csla.Core.FieldManager.PropertyInfoFactory.Factory.Create<P>(objectType, propertyName));
    }

    /// <summary>
    /// Indicates that the specified property belongs
    /// to the business object type.
    /// </summary>
    /// <typeparam name="P">Type of property</typeparam>
    /// <param name="objectType">Type of the object.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <param name="friendlyName">Name of the friendly.</param>
    /// <returns>The provided IPropertyInfo object.</returns>
    protected static PropertyInfo<P> RegisterProperty<P>(Type objectType, string propertyName, string friendlyName)
    {
      return RegisterProperty(objectType, Csla.Core.FieldManager.PropertyInfoFactory.Factory.Create<P>(objectType, propertyName, friendlyName));
    }

    /// <summary>
    /// Indicates that the specified property belongs
    /// to the business object type.
    /// </summary>
    /// <typeparam name="P">Type of property</typeparam>
    /// <param name="objectType">Type of the object.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <param name="friendlyName">Name of the friendly.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The provided IPropertyInfo object.</returns>
    protected static PropertyInfo<P> RegisterProperty<P>(Type objectType, string propertyName, string friendlyName, P defaultValue)
    {
      return RegisterProperty(objectType, Csla.Core.FieldManager.PropertyInfoFactory.Factory.Create<P>(objectType, propertyName, friendlyName, defaultValue));
    }
  }
}