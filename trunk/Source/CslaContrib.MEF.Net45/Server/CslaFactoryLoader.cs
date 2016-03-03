using System;
using System.ComponentModel.Composition;
using System.Linq;
using Csla.Server;
using CslaContrib.MEF.Properties;


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
    private static object _syncRoot = new object();
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

      var typename = GetTypeName(factoryName);

      lock (_syncRoot)
      {
        var parts =
          Ioc.Container.Catalog.Parts.Where(part => part.ExportDefinitions.Any(item => item.ContractName == typename)).
              Select(p => p).ToArray();

        if (parts.Count() == 1)
        {
          var part = parts[0].CreatePart();
          Ioc.Container.SatisfyImportsOnce(part);  // compose the object
          object obj = part.GetExportedValue(part.ExportDefinitions.First(item => item.ContractName == typename));
          return obj;
        }

        if (parts.Count() > 1)
        {
          throw new InvalidOperationException(string.Format(Resources.MoreThanOneFactoryTypeDefinedException,
                                                            factoryName));
        }
      }

      throw new InvalidOperationException(
          string.Format(Csla.Properties.Resources.FactoryTypeNotFoundException, factoryName));
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
