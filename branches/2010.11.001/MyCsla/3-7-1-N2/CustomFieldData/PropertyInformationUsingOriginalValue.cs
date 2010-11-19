using Csla;
using Csla.Core.FieldManager;
using System;

namespace CustomFieldData
{
public sealed class PropertyInformationUsingOriginalValue<T>
	: PropertyInfo<T>
{
	public PropertyInformationUsingOriginalValue(string name)
		: base(name)
	{
	}
	
	public PropertyInformationUsingOriginalValue(string name, string friendlyName)
		: base(name, friendlyName)
	{
	}

	public PropertyInformationUsingOriginalValue(string name, string friendlyName, 
		RelationshipTypes relationship)
		: base(name, friendlyName, relationship)
	{
	}

	public PropertyInformationUsingOriginalValue(string name, string friendlyName, 
		T defaultValue)
		: base(name, friendlyName, defaultValue)
	{
	}

	public PropertyInformationUsingOriginalValue(string name, string friendlyName, 
		T defaultValue, RelationshipTypes relationship)
		: base(name, friendlyName, defaultValue, relationship)
	{
	}

	protected override IFieldData NewFieldData(string name)
	{
		return (typeof(T).IsAssignableFrom(typeof(string)) ?
			new FieldDataUsingOriginalValueViaHashCode<T>(name) as IFieldData :
			new FieldDataUsingOriginalValueViaDuplicate<T>(name) as IFieldData);
	}
}
}