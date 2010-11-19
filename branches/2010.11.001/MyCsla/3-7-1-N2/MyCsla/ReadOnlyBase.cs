using System;
using Csla;

namespace MyCsla
{
  /// <summary>
  /// Adds posibbility of Logging/Instrumentation of DataPortal calls on BusinessObjects that does not use the ObjectFactoryAttribute
  /// 
  /// Consider f.ex the possiblity of creating PerformanceCounters and logging of Exceptions to EventLog.  
  /// </summary>
  /// <typeparam name="T"></typeparam>
  [Serializable]
  public class ReadOnlyBase<T> : Csla.ReadOnlyBase<T> where T : ReadOnlyBase<T>
  {
    /// <summary>
    /// Registers the property.
    /// </summary>
    /// <typeparam name="P"></typeparam>
    /// <param name="propertyName">Name of the property.</param>
    /// <returns></returns>
    protected static PropertyInfo<P> RegisterProperty<P>(string propertyName)
    {
            return RegisterProperty(Csla.Core.FieldManager.PropertyInfoFactory.Factory.Create<P>(typeof(T), propertyName));
    }

    /// <summary>
    /// Registers the property.
    /// </summary>
    /// <typeparam name="P"></typeparam>
    /// <param name="propertyName">Name of the property.</param>
    /// <param name="friendlyName">Name of the friendly.</param>
    /// <returns></returns>
    protected static PropertyInfo<P> RegisterProperty<P>(string propertyName, string friendlyName)
    {
            return RegisterProperty(Csla.Core.FieldManager.PropertyInfoFactory.Factory.Create<P>(typeof(T), propertyName, friendlyName));
    }

    /// <summary>
    /// Registers the property.
    /// </summary>
    /// <typeparam name="P"></typeparam>
    /// <param name="propertyName">Name of the property.</param>
    /// <param name="friendlyName">Name of the friendly.</param>
    /// <param name="relationship">The relationship.</param>
    /// <returns></returns>
    protected static PropertyInfo<P> RegisterProperty<P>(string propertyName, string friendlyName, RelationshipTypes relationship)
    {
            return RegisterProperty(Csla.Core.FieldManager.PropertyInfoFactory.Factory.Create<P>(typeof(T), propertyName, friendlyName, relationship));
    }

    /// <summary>
    /// Registers the property.
    /// </summary>
    /// <typeparam name="P"></typeparam>
    /// <param name="propertyName">Name of the property.</param>
    /// <param name="friendlyName">Name of the friendly.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns></returns>
    protected static PropertyInfo<P> RegisterProperty<P>(string propertyName, string friendlyName, P defaultValue)
    {
            return RegisterProperty(Csla.Core.FieldManager.PropertyInfoFactory.Factory.Create<P>(typeof(T), propertyName, friendlyName, defaultValue));
    }

    /// <summary>
    /// Registers the property.
    /// </summary>
    /// <typeparam name="P"></typeparam>
    /// <param name="propertyName">Name of the property.</param>
    /// <param name="friendlyName">Name of the friendly.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="relationship">The relationship.</param>
    /// <returns></returns>
    protected static PropertyInfo<P> RegisterProperty<P>(string propertyName, string friendlyName, P defaultValue, RelationshipTypes relationship)
    {
      return RegisterProperty(Csla.Core.FieldManager.PropertyInfoFactory.Factory.Create<P>(typeof(T), propertyName, friendlyName, defaultValue, relationship));
    }

  }
}