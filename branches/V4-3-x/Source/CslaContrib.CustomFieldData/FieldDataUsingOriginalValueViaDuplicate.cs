using Csla.Core;
using Csla.Core.FieldManager;
using System;
using Csla.Serialization;

namespace CslaContrib.CustomFieldData
{
	[Serializable]
	public sealed class FieldDataUsingOriginalValueViaDuplicate<T>
		: FieldDataUsingOriginalValue<T, T>
	{
		public FieldDataUsingOriginalValueViaDuplicate(string name)
			: base(name) { }

		protected override bool HasValueChanged()
		{
			var hasValueChanged = false;
			
			if(typeof(T).IsValueType)
			{
				hasValueChanged = !this.OriginalValue.Equals(this.Value);
			}
			else
			{
				var originalIsNotNull = this.OriginalValue != null;
				var currentIsNotNull = this.Value != null;

				if(originalIsNotNull != currentIsNotNull)
				{
					hasValueChanged = true;
				}
				else
				{
					if(originalIsNotNull)
					{
						var child = this.Value as ITrackStatus;

						if(child != null)
						{
							hasValueChanged = child.IsDirty;
						}
						else
						{
							hasValueChanged = !this.OriginalValue.Equals(this.Value);
						}
					}
				}
			}
			
			return hasValueChanged;
		}

		protected override void SetOriginalValue(T value)
		{
			this.OriginalValue = value;
		}
	}
}
