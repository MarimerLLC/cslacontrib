using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
#if !SILVERLIGHT
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
#endif


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

#if SILVERLIGHT
    /// <summary>
    /// Gets or sets the DeploymentCatalog semicolon separated list of XAPs the <see cref="Container"/> should use.
    /// </summary>
    /// <value>
    /// The DeploymentCatalog list.
    /// </value>
    public static string MefDeploymentCatalog { get; set; }
#endif

#if !SILVERLIGHT
    /// <summary>
    /// Gets the container with the assemblies specifyed in the appSettings "\CslaContrib.Mef.DirectoryCatalog\".
    /// </summary>
#else
    /// <summary>
    /// Gets the container with the assemblies in the XAPs specifyed in <see cref="MefDeploymentCatalog"/> and assemblies in the current XAP
    /// </summary>
    /// <remarks>
    /// To use the container, the consuming application should call \"CompositionHost.Initialize(Container)\".
    /// </remarks>
#endif
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
#if !SILVERLIGHT
              Debug.Write("Start configuring MEF Container");
#else
              Debug.WriteLine("Start configuring MEF Container");
#endif


              //create container
              var catalog = new AggregateCatalog();

#if !SILVERLIGHT
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
#else
              if (MefDeploymentCatalog.Length > 0)
              {
                foreach (var value in MefDeploymentCatalog.Split(';'))
                {
                  var dc = new DeploymentCatalog(value);
                  dc.DownloadAsync();
                  catalog.Catalogs.Add(dc);
                }
              }
              catalog.Catalogs.Add(new DeploymentCatalog());
#endif
              var container = new CompositionContainer(catalog, true);
              container.ComposeParts();
              _container = container;

#if !SILVERLIGHT
              Debug.Write("End configuring MEF Container");
#else
              Debug.WriteLine("End configuring MEF Container");
#endif
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
