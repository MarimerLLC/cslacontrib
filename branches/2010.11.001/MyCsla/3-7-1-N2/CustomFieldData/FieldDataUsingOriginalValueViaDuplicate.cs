using Csla.Core;
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
      // Is this a child object/list that keeps track of status 
      ITrackStatus child = Value as ITrackStatus;
      if (child != null)
      {
        return child.IsDirty;
      }

      // else check single field 
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
      // Is this a child object/list that keeps track of status the do not keep original value
      var child = Value as ITrackStatus;
      if (child != null) return; 

			this.OriginalValue = value;
		}
	}
}
