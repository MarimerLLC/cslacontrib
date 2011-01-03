using Csla;
using Csla.Core.FieldManager;
using System;

namespace CslaContrib.CustomFieldData
{
  /// <summary>
  /// ProperyInformationFactory class - must be configured in app.config or web.config
  /// </summary>
  /// <typeparam name="T"></typeparam>
	public sealed class PropertyInformationUsingOriginalValue<T>
		: PropertyInfo<T>
	{
		public PropertyInformationUsingOriginalValue(Type containingType, string name)
			: base(name)
		{
			this.ContainingType = containingType;
		}

		public PropertyInformationUsingOriginalValue(Type containingType, string name, string friendlyName)
			: base(name, friendlyName)
		{
			this.ContainingType = containingType;
		}

		public PropertyInformationUsingOriginalValue(Type containingType, string name, string friendlyName,
			RelationshipTypes relationship)
			: base(name, friendlyName, relationship)
		{
			this.ContainingType = containingType;
		}

		public PropertyInformationUsingOriginalValue(Type containingType, string name, string friendlyName,
			T defaultValue)
			: base(name, friendlyName, defaultValue)
		{
			this.ContainingType = containingType;
		}

		public PropertyInformationUsingOriginalValue(Type containingType, string name, string friendlyName,
			T defaultValue, RelationshipTypes relationship)
			: base(name, friendlyName, defaultValue, relationship)
		{
			this.ContainingType = containingType;
		}

		protected override IFieldData NewFieldData(string name)
		{
			return typeof(T).IsAssignableFrom(typeof(string)) ?
				new FieldDataUsingOriginalValueViaHashCode<T>(name) as IFieldData :
				new FieldDataUsingOriginalValueViaDuplicate<T>(name) as IFieldData;
		}

		private Type ContainingType { get; set; }
	}
}