using Csla;
using Csla.Core.FieldManager;
using System;

namespace CustomFieldData
{
	[Serializable]
	public abstract class FieldDataUsingOriginalValue<T, TOriginal>
		: FieldData<T>
	{
		protected FieldDataUsingOriginalValue(string name)
			: base(name)
		{
		}

		protected abstract bool HasValueChanged();

		public override void MarkClean()
		{
			base.MarkClean();
			this.SetOriginalValue(this.Value);
			this.HasMarkCleanBeenCalled = true;
		}
			
		protected abstract void SetOriginalValue(T value);
		
		private bool HasOriginalValueBeenSet { get; set; }
		private bool HasMarkCleanBeenCalled { get; set; }

		public override bool IsDirty
		{
			get
			{
				return (this.HasMarkCleanBeenCalled ? this.HasValueChanged() : base.IsDirty);
			}
		}
		
		protected TOriginal OriginalValue { get; set; }

		public override T Value
		{
			get
			{
				return base.Value;
			}
			set
			{
				if(!this.HasOriginalValueBeenSet)
				{
					if(this.HasMarkCleanBeenCalled)
					{
						this.SetOriginalValue(value);
						this.HasOriginalValueBeenSet = true;
					}
				}
				
				base.Value = value;
			}
		}
	}
}
