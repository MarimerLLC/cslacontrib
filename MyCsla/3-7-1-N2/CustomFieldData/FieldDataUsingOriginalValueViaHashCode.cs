using Csla.Core;
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
      // Is this a child object/list that keeps track of status 
      ITrackStatus child = Value as ITrackStatus;
      if (child != null)
      {
        return child.IsDirty;
      }

			return this.Value != null ? 
				this.OriginalValue != this.Value.GetHashCode() : 
				this.OriginalValue != 0;
		}

		protected override void SetOriginalValue(T value)
		{
      // Is this a child object/list that keeps track of status the do not keep original value
      var child = Value as ITrackStatus;
      if (child != null) return; 

			this.OriginalValue = value != null ? value.GetHashCode() : 0;
		}
	}
}
