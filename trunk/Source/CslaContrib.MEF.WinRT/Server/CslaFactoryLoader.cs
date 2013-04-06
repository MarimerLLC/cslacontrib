using System;
using System.Composition;
using System.Linq;
using Csla.Server;


namespace CslaContrib.MEF.Server
{
  /// <summary>
  /// Custom ObjectFactoryLoaded that uses MEF ro resolve Interface into type or instance.
  /// </summary>
  /// <example>
  /// Must be configured in app.config or web.config 
  /// <code>
  ///   <appSettings>
  ///     <add key="CslaObjectFactoryLoader" value="CslaContrib.MEF.Server.MefFactoryLoader, CslaContrib.MEF"/>
  ///   </appSettings>
  /// </code>
  /// </example>
  public class CslaFactoryLoader : IObjectFactoryLoader
  {
    /// <summary>
    /// Gets the type name from the factory name.
    /// </summary>
    /// <param name="factoryName">Name of the factory.</param>
    /// <returns></returns>
    private string GetTypeName(string factoryName)
    {
      if (string.IsNullOrEmpty(factoryName)) return string.Empty;

      var values = factoryName.Split(',');
      return values[0];
    }


    /// <summary>
    /// Gets the factory object instance.
    /// </summary>
    /// <param name="factoryName">Name of the factory.</param>
    /// <returns></returns>
    public object GetFactory(string factoryName)
    {
      var type = Type.GetType(factoryName);

      object factory;
      if (Ioc.Container.TryGetExport(type, out factory))
        return factory;

      return Activator.CreateInstance(type);
    }

    /// <summary>
    /// Gets the type of the factory.
    /// </summary>
    /// <param name="factoryName">Name of the factory.</param>
    /// <returns></returns>
    public Type GetFactoryType(string factoryName)
    {
      // return an instance of the Interface 
      // use RunLocal on the interface definition - rather than the actrual class. 
      return Type.GetType(factoryName, false);
    }
  }
}
