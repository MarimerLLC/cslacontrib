using Csla.Core;
using Csla.Core.FieldManager;
using System;
using Csla.Serialization;

namespace CslaContrib.CustomFieldData
{
	[Serializable]
	public sealed class FieldDataUsingOriginalValueViaHashCode<T>
		: FieldDataUsingOriginalValue<T, int>
	{
		public FieldDataUsingOriginalValueViaHashCode(string name)
			: base(name) { }

		protected override bool HasValueChanged()
		{
			var hasValueChanged = false;

			if(typeof(T).IsValueType)
			{
				hasValueChanged = !(this.OriginalValue == this.Value.GetHashCode());
			}
			else
			{
				var child = this.Value as ITrackStatus;

				if(child != null)
				{
					hasValueChanged = child.IsDirty;
				}
				else
				{
					hasValueChanged = this.Value != null ?
						this.OriginalValue != this.Value.GetHashCode() :
						this.OriginalValue != 0;
				}
			}

			return hasValueChanged;
		}

		protected override void SetOriginalValue(T value)
		{
			this.OriginalValue = (value != null) ? value.GetHashCode() : 0;
		}
	}
}
