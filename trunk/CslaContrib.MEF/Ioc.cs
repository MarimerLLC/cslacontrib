using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Reflection;


namespace CslaContrib.MEF
{
  /// <summary>
  /// Provides access to the IOC Container shared by all applications.
  /// </summary>
  public static class Ioc
  {
    private static readonly object TypeLock = new object();

    //Container
    private static CompositionContainer _container;

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
          lock (TypeLock)
          {
            if (_container == null)
            {
              Debug.Write("Start configuring MEF Container");

              //create container
              var catalog = new AggregateCatalog();

              catalog.Catalogs.Add(new DirectoryCatalog("."));
              catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
              _container = new CompositionContainer(catalog);
              _container.ComposeParts();

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
      lock (TypeLock)
      {
        _container = container;
      }
    }
  }
}
