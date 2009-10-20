using Csla.Core.FieldManager;
using System;

namespace CustomFieldData
{
	[Serializable]
	public sealed class FieldDataUsingOriginalValueViaHashCode<T>
		: FieldDataUsingOriginalValue<T, int>
	{
		public FieldDataUsingOriginalValueViaHashCode(string name)
			: base(name)
		{
		}

		protected override bool HasValueChanged()
		{
			return this.Value != null ? 
				this.OriginalValue != this.Value.GetHashCode() : 
				this.OriginalValue != 0;
		}

		protected override void SetOriginalValue(T value)
		{
			this.OriginalValue = value != null ? value.GetHashCode() : 0;
		}
	}
}
