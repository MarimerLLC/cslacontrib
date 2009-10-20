using Csla;
using System;

namespace CustomFieldData
{
	[Serializable]
	public abstract class BusinessCore<T>
		: MyCsla.BusinessBase<T>
		where T: BusinessCore<T>
	{
		protected BusinessCore()
			: base()
		{
		}

		protected override void DataPortal_OnDataPortalInvokeComplete(DataPortalEventArgs e)
		{
			base.DataPortal_OnDataPortalInvokeComplete(e);
			
			if(e.Operation == DataPortalOperations.Fetch)
			{
				this.MarkClean();
			}
		}

		public override bool IsDirty
		{
			get
			{
				return this.FieldManager.IsDirty();
			}
		}

		public override bool IsSelfDirty
		{
			get
			{
				var isSelfDirty = false;
				
				foreach(var registeredProperty in this.FieldManager.GetRegisteredProperties())
				{
					if(((registeredProperty.RelationshipType & RelationshipTypes.Child) == 0) &&
						this.FieldManager.IsFieldDirty(registeredProperty))
					{
						isSelfDirty = true;
						break;
					}
				}
				
				return isSelfDirty;
			}
		}
	}
}
