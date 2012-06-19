using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;


namespace CslaContrib.MEF
{
  /// <summary>
  /// Provides access to the IOC Container shared by all applications.
  /// </summary>
  public static class Ioc
  {
    private static readonly object _syncRoot = new object();

    //Container
    private static volatile CompositionContainer _container;

    /// <summary>
    /// Gets the container.
    /// </summary>
    public static CompositionContainer Container
    {
      get
      {
        //create and configure container if one does not yet exist
        if (_container == null)
        {
          lock (_syncRoot)
          {
            if (_container == null)
            {
              Debug.Write("Start configuring MEF Container");

              //create container
              var catalog = new AggregateCatalog();

              var parts = ConfigurationManager.AppSettings.AllKeys.Where(p => p.StartsWith("CslaContrib.Mef.DirectoryCatalog", true, CultureInfo.InvariantCulture));
              if (parts.Any())
              {
                foreach (var values in parts.Select(part => ConfigurationManager.AppSettings[part].Split(';')))
                {
                  catalog.Catalogs.Add(values.Count() > 1
                                         ? new DirectoryCatalog(values[0], values[1])
                                         : new DirectoryCatalog(values[0]));
                }
              }
              else
              {
                catalog.Catalogs.Add(new DirectoryCatalog("."));
                catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
              }
              var container = new CompositionContainer(catalog);
              container.ComposeParts();
              _container = container;

              Debug.Write("End configuring MEF Container");
            }
          }
        }
        return _container;
      }
    }

    /// <summary>
    /// Injects the container. Use this for unit testing where you want to control the type reasolving.
    /// </summary>
    /// <param name="container">The container.</param>
    public static void InjectContainer(CompositionContainer container)
    {
      lock (_syncRoot)
      {
        _container = container;
      }
    }
  }
}
