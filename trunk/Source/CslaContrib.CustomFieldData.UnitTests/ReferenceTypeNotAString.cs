using System;

namespace CslaContrib.CustomFieldData.UnitTests
{
	[Serializable]
	internal sealed class ReferenceTypeNotAString
		: IEquatable<ReferenceTypeNotAString>
	{
		internal ReferenceTypeNotAString(Guid value)
			: base()
		{
			this.Value = value;
		}

		public override bool Equals(object obj)
		{
			var areEqual = false;

			var target = obj as ReferenceTypeNotAString;

			if(target != null)
			{
				areEqual = this.Equals(target);
			}

			return areEqual;
		}

		public bool Equals(ReferenceTypeNotAString other)
		{
			return (other != null) && 
				(this.Value == other.Value);
		}

		public override int GetHashCode()
		{
			return this.Value.GetHashCode();
		}

		public Guid Value { get; private set; }
	}
}
