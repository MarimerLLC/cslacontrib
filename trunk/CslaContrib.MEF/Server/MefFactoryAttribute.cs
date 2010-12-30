using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla.Server;

namespace CslaContrib.MEF.Server
{
  /// <summary>
  /// ObjectFactoryAttribute derived class that accepts a Type as input and gets the FQN from the type.
  /// </summary>
  public class MefFactoryAttribute : ObjectFactoryAttribute
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MefFactoryAttribute"/> class.
    /// </summary>
    /// <param name="factoryType">Type of the factory.</param>
    public MefFactoryAttribute(Type factoryType)
      : base(GetAssemblyQualifiedName(factoryType))
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="MefFactoryAttribute"/> class.
    /// </summary>
    /// <param name="factoryType">Type of the factory.</param>
    /// <param name="fetchMethod">The fetch method.</param>
    public MefFactoryAttribute(Type factoryType, string fetchMethod)
      : base(GetAssemblyQualifiedName(factoryType), fetchMethod)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="MefFactoryAttribute"/> class.
    /// </summary>
    /// <param name="factoryType">Type of the factory.</param>
    /// <param name="createMethod">The create method.</param>
    /// <param name="fetchMethod">The fetch method.</param>
    public MefFactoryAttribute(Type factoryType, string createMethod, string fetchMethod)
      : base(GetAssemblyQualifiedName(factoryType), createMethod, fetchMethod)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MefFactoryAttribute"/> class.
    /// </summary>
    /// <param name="factoryType">Type of the factory.</param>
    /// <param name="createMethod">The create method.</param>
    /// <param name="fetchMethod">The fetch method.</param>
    /// <param name="updateMethod">The update method.</param>
    /// <param name="deleteMethod">The delete method.</param>
    public MefFactoryAttribute(Type factoryType, string createMethod, string fetchMethod, string updateMethod, string deleteMethod)
      : base(GetAssemblyQualifiedName(factoryType), createMethod, fetchMethod, updateMethod, deleteMethod)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MefFactoryAttribute"/> class.
    /// </summary>
    /// <param name="factoryType">Type of the factory.</param>
    /// <param name="createMethod">The create method.</param>
    /// <param name="fetchMethod">The fetch method.</param>
    /// <param name="updateMethod">The update method.</param>
    /// <param name="deleteMethod">The delete method.</param>
    /// <param name="executeMethod">The execute method.</param>
    public MefFactoryAttribute(Type factoryType, string createMethod, string fetchMethod, string updateMethod, string deleteMethod, string executeMethod)
      : base(GetAssemblyQualifiedName(factoryType), createMethod, fetchMethod, updateMethod, deleteMethod, executeMethod)
    {
    }

    /// <summary>
    /// Gets the simple version of qualified assembly name.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>simple qualified assembly name</returns>
    private static string GetAssemblyQualifiedName(Type type)
    {
      if (type.IsGenericType)
      {
        return type.AssemblyQualifiedName;
      }
      else
      {
        if (type.AssemblyQualifiedName == null) return string.Empty;
        var elements = type.AssemblyQualifiedName.Split(',');
        return string.Join(",", elements[0], elements[1]);
      }
    }
  }
}
