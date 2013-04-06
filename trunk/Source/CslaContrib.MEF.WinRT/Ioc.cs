using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Composition;
using System.Composition.Convention;
using System.Composition.Hosting;

namespace CslaContrib.MEF
{
  /// <summary>
  /// Provides access to the IOC Container shared by all applications.
  /// </summary>
  public static class Ioc
  {
    private static readonly object _syncRoot = new object();

    //Container
    private static volatile CompositionHost _container;

    /// <summary>
    /// Gets the container.
    /// </summary>
    public static CompositionHost Container
    {
      get { return _container;  }
    }

    public static void InitializeContainer(IEnumerable<string> appAssemblyNames)
    {
      var assemblies = appAssemblyNames.Select(p => Assembly.Load(new AssemblyName(p)));
      var configuration = new ContainerConfiguration().WithAssemblies(assemblies);
      _container = configuration.CreateContainer();
    }

    public static void InitializeContainer(IEnumerable<Assembly> appAssemblies)
    {
      var configuration = new ContainerConfiguration().WithAssemblies(appAssemblies);
      _container = configuration.CreateContainer();
    }


    /// <summary>
    /// Injects the container. Use this for unit testing where you want to control the type reasolving.
    /// </summary>
    /// <param name="container">The container.</param>
    public static void InjectContainer(CompositionHost container)
    {
      lock (_syncRoot)
      {
        _container = container;
      }
    }
  }
}
