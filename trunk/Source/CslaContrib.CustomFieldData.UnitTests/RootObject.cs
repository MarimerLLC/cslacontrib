using Csla;
using System;

namespace CslaContrib.CustomFieldData.UnitTests
{
	[Serializable]
	internal sealed class RootObject
		: BusinessCore<RootObject>
	{
	  public RootObject()
			: base() { }

		internal static RootObject Create()
		{
			return DataPortal.Create<RootObject>();
		}

		internal static RootObject Fetch(string referenceType,
			bool valueType, ReferenceTypeNotAString referenceTypeNotAString)
		{
			return DataPortal.Fetch<RootObject>(
				new RootCriteria(referenceType, valueType, referenceTypeNotAString));
		}

		protected override void DataPortal_Create()
		{
			this.Child = ChildObject.Create();
		}

		private void DataPortal_Fetch(RootCriteria criteria)
		{
			using(this.BypassPropertyChecks)
			{
				this.ReferenceType = criteria.ReferenceType;
				this.ReferenceTypeNotAString = criteria.ReferenceTypeNotAString;
				this.ValueType = criteria.ValueType;
				this.Child = ChildObject.Fetch(criteria.ReferenceType, criteria.ValueType);
			}
		}

		protected override void DataPortal_Insert()
		{
			this.FieldManager.UpdateChildren();
		}

    public readonly static PropertyInfo<ChildObject> childProperty =
			RootObject.RegisterProperty<ChildObject>(e => e.Child);
		public ChildObject Child
		{
			get { return this.GetProperty(RootObject.childProperty); }
			set { this.SetProperty(RootObject.childProperty, value); }
		}

    public readonly static PropertyInfo<ChildObject> nulledChildProperty =
			RootObject.RegisterProperty<ChildObject>(e => e.NulledChild);
		public ChildObject NulledChild
		{
			get { return this.GetProperty(RootObject.nulledChildProperty); }
			set { this.SetProperty(RootObject.nulledChildProperty, value); }
		}

    public readonly static PropertyInfo<string> referenceTypeProperty =
			RootObject.RegisterProperty<string>(e => e.ReferenceType);
		public string ReferenceType
		{
			get { return this.GetProperty(RootObject.referenceTypeProperty); }
			set { this.SetProperty(RootObject.referenceTypeProperty, value); }
		}

    public readonly static PropertyInfo<ReferenceTypeNotAString> referenceTypeNotAStringProperty =
			RootObject.RegisterProperty<ReferenceTypeNotAString>(e => e.ReferenceTypeNotAString);
		public ReferenceTypeNotAString ReferenceTypeNotAString
		{
			get { return this.GetProperty(RootObject.referenceTypeNotAStringProperty); }
			set { this.SetProperty(RootObject.referenceTypeNotAStringProperty, value); }
		}

    public readonly static PropertyInfo<bool> valueTypeProperty =
			RootObject.RegisterProperty<bool>(e => e.ValueType);
		public bool ValueType
		{
			get { return this.GetProperty(RootObject.valueTypeProperty); }
			set { this.SetProperty(RootObject.valueTypeProperty, value); }
		}

		[Serializable]
		private sealed class RootCriteria
			: CriteriaBase<RootCriteria>
		{
			public RootCriteria(string referenceType, 
				bool valueType, ReferenceTypeNotAString referenceTypeNotAString)
				: base()
			{
				this.ReferenceType = referenceType;
				this.ValueType = valueType;
				this.ReferenceTypeNotAString = referenceTypeNotAString;
			}

			public string ReferenceType { get; private set ; }
			public ReferenceTypeNotAString ReferenceTypeNotAString { get; private set; }
			public bool ValueType { get; private set; } 
		}
	}
}
