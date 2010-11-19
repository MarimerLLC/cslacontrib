using Csla;
using Csla.Core;
using System;

namespace CustomFieldData
{
	public sealed class PropertyInformationFactory : IPropertyInfoFactory
	{
		public PropertyInfo<T> Create<T>(Type type, string name, string friendlyName, 
			T defaultValue, RelationshipTypes relationship)
		{
			return new PropertyInformationUsingOriginalValue<T>(name, friendlyName, 
				defaultValue, relationship);
		}

    public PropertyInfo<T> Create<T>(Type type, string name, string friendlyName, 
			T defaultValue)
		{
			return new PropertyInformationUsingOriginalValue<T>(name, friendlyName, 
				defaultValue);
		}

    public PropertyInfo<T> Create<T>(Type type, string name, string friendlyName, 
			RelationshipTypes relationship)
		{
			return new PropertyInformationUsingOriginalValue<T>(name, friendlyName, 
				relationship);
		}

    public PropertyInfo<T> Create<T>(Type type, string name, string friendlyName)
		{
			return new PropertyInformationUsingOriginalValue<T>(name, friendlyName);
		}

    public PropertyInfo<T> Create<T>(Type type, string name)
		{
			return new PropertyInformationUsingOriginalValue<T>(name);
		}
	}
}
