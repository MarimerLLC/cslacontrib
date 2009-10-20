using Csla.Core.FieldManager;
using System;

namespace CustomFieldData
{
	[Serializable]
	public sealed class FieldDataUsingOriginalValueViaDuplicate<T>
		: FieldDataUsingOriginalValue<T, T>
	{
		public FieldDataUsingOriginalValueViaDuplicate(string name)
			: base(name)
		{
		}

		protected override bool HasValueChanged()
		{
			var hasValueChanged = false;
			
			if(this.OriginalValue == null && this.Value == null)
			{
				hasValueChanged = false;
			}
			else if(this.OriginalValue != null)
			{
				hasValueChanged = !this.OriginalValue.Equals(this.Value);
			}
			else
			{
				hasValueChanged = true;
			}
			
			return hasValueChanged;
		}

		protected override void SetOriginalValue(T value)
		{
			this.OriginalValue = value;
		}
	}
}
