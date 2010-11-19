using System;
using System.Diagnostics;
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
  public class BusinessBase<T> : Csla.BusinessBase<T> where T : BusinessBase<T>
  {
    protected override void DataPortal_OnDataPortalInvoke(DataPortalEventArgs e)
    {
      Debug.Print("DataPortalInvoke object:{0}, operation:{1}", e.ObjectType, e.Operation);
      base.DataPortal_OnDataPortalInvoke(e);
    }

    protected override void DataPortal_OnDataPortalInvokeComplete(DataPortalEventArgs e)
    {
      Debug.Print("DataPortalInvokeCompleted object:{0}, operation:{1}", e.ObjectType, e.Operation);
      base.DataPortal_OnDataPortalInvokeComplete(e);
    }

    protected override void DataPortal_OnDataPortalException(DataPortalEventArgs e, Exception ex)
    {
      Debug.Print("DataPortalExeption object:{0}, operation:{1}, exception:{2}", e.ObjectType, e.Operation, ex);
      base.DataPortal_OnDataPortalException(e, ex);
    }

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