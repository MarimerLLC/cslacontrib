using Csla;
using Csla.Core;
using System;

namespace CslaContrib.CustomFieldData
{
  /// <summary>
  /// ProperyInformationFactory class - must be configured in app.config or web.config
  /// </summary>
  public sealed class PropertyInformationFactory : IPropertyInfoFactory
  {
    public PropertyInfo<T> Create<T>(Type containingType, string name, string friendlyName, 
      T defaultValue, RelationshipTypes relationship)
    {
      return new PropertyInformationUsingOriginalValue<T>(containingType, name, friendlyName, 
        defaultValue, relationship);
    }

    public PropertyInfo<T> Create<T>(Type containingType, string name, string friendlyName, 
      T defaultValue)
    {
      return new PropertyInformationUsingOriginalValue<T>(containingType, name, friendlyName, 
        defaultValue);
    }

    public PropertyInfo<T> Create<T>(Type containingType, string name, string friendlyName, 
      RelationshipTypes relationship)
    {
      return new PropertyInformationUsingOriginalValue<T>(containingType, name, friendlyName, 
        relationship);
    }

    public PropertyInfo<T> Create<T>(Type containingType, string name, string friendlyName)
    {
      return new PropertyInformationUsingOriginalValue<T>(containingType, name, friendlyName);
    }

    public PropertyInfo<T> Create<T>(Type containingType, string name)
    {
      return new PropertyInformationUsingOriginalValue<T>(containingType, name);
    }
  }
}
