using Csla;
using System;

namespace CslaContrib.CustomFieldData.UnitTests
{
	[Serializable]
	internal sealed class ChildObject
		: BusinessCore<ChildObject>
	{
	  public ChildObject()
			: base() { }

		internal static ChildObject Create()
		{
			return DataPortal.CreateChild<ChildObject>();
		}

		internal static ChildObject Fetch(string referenceType, bool valueType)
		{
			return DataPortal.FetchChild<ChildObject>(
				new ChildCriteria(referenceType, valueType));
		}

		private void Child_Fetch(ChildCriteria criteria)
		{
			using(this.BypassPropertyChecks)
			{
				this.ReferenceType = criteria.ReferenceType;
				this.ValueType = criteria.ValueType;
			}
		}

		private void Child_Insert() { }

	  public readonly static PropertyInfo<string> referenceTypeProperty =
			ChildObject.RegisterProperty<string>(e => e.ReferenceType);
		public string ReferenceType
		{
			get { return this.GetProperty(ChildObject.referenceTypeProperty); }
			set { this.SetProperty(ChildObject.referenceTypeProperty, value); }
		}

	  public readonly static PropertyInfo<bool> valueTypeProperty =
			ChildObject.RegisterProperty<bool>(e => e.ValueType);
		public bool ValueType
		{
			get { return this.GetProperty(ChildObject.valueTypeProperty); }
			set { this.SetProperty(ChildObject.valueTypeProperty, value); }
		}

		[Serializable]
		private sealed class ChildCriteria
			: CriteriaBase<ChildCriteria>
		{
			public ChildCriteria(string referenceType, bool valueType)
				: base()
			{
				this.ReferenceType = referenceType;
				this.ValueType = valueType;
			}

			public string ReferenceType { get; private set; }
			public bool ValueType { get; private set; }
		}
	}
}
