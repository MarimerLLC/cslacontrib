using Csla;
using Csla.Core.FieldManager;
using System;
using Csla.Serialization;

namespace CslaContrib.CustomFieldData
{
	[Serializable]
	public abstract class FieldDataUsingOriginalValue<T, TOriginal> : FieldData<T>
	{
		protected FieldDataUsingOriginalValue(string name)
			: base(name) { }

		protected abstract bool HasValueChanged();

		public override void MarkClean()
		{
			base.MarkClean();
			this.SetOriginalValue(this.Value);
		}
			
		protected abstract void SetOriginalValue(T value);

		private bool HasBeenSet { get; set; }

		public override bool IsDirty
		{
			get
			{
				return !this.HasBeenSet ? base.IsDirty : this.HasValueChanged();
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
				if(!this.HasBeenSet)
				{
					this.SetOriginalValue(value);
					this.HasBeenSet = true;
				}
				
				base.Value = value;
			}
		}
	}
}
