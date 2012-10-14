using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spackle;
using Spackle.Testing;
using System;

namespace CslaContrib.CustomFieldData.UnitTests
{
	[TestClass]
	public sealed class ObjectStateTests
		: CoreTests
	{
		[TestMethod]
		public void ChangeChildReferenceType()
		{
			var parent = RootObject.Create();
			var newValue = new RandomObjectGenerator().Generate<string>();

			parent.Child.ReferenceType = newValue;

			Assert.AreEqual(newValue, parent.Child.ReferenceType);
			Assert.IsTrue(parent.IsDirty);
			Assert.IsTrue(parent.Child.IsDirty);
		}

		[TestMethod]
		public void ChangeChildReferenceTypeAndUndo()
		{
			var parent = RootObject.Create();
			var oldValue = parent.ReferenceType;
			var newValue = new RandomObjectGenerator().Generate<string>();

			parent.Child.ReferenceType = newValue;
			parent.Child.ReferenceType = oldValue;

			Assert.AreEqual(oldValue, parent.Child.ReferenceType);
			Assert.IsFalse(parent.IsDirty);
			Assert.IsFalse(parent.Child.IsDirty);
		}

		[TestMethod]
		public void ChangeChildValueType()
		{
			var parent = RootObject.Create();
			var newValue = !parent.Child.ValueType;

			parent.Child.ValueType = newValue;

			Assert.AreEqual(newValue, parent.Child.ValueType);
			Assert.IsTrue(parent.IsDirty);
			Assert.IsTrue(parent.Child.IsDirty);
		}

		[TestMethod]
		public void ChangeChildValueTypeAndUndo()
		{
			var parent = RootObject.Create();
			var oldValue = parent.Child.ValueType;
			var newValue = !oldValue;

			parent.Child.ValueType = newValue;
			parent.Child.ValueType = oldValue;

			Assert.AreEqual(oldValue, parent.Child.ValueType);
			Assert.IsFalse(parent.IsDirty);
			Assert.IsFalse(parent.Child.IsDirty);
		}

		[TestMethod]
		public void ChangeNulledChildReferenceType()
		{
			var parent = RootObject.Create();
			var newValue = ChildObject.Create();

			parent.NulledChild = newValue;

			Assert.IsTrue(parent.IsDirty);
		}

		[TestMethod]
		public void ChangeParentReferenceType()
		{
			var parent = RootObject.Create();
			var newValue = new RandomObjectGenerator().Generate<string>();

			parent.ReferenceType = newValue;

			Assert.AreEqual(newValue, parent.ReferenceType);
			Assert.IsTrue(parent.IsDirty);
			Assert.IsFalse(parent.Child.IsDirty);
		}

		[TestMethod]
		public void ChangeParentReferenceTypeAndUndo()
		{
			var parent = RootObject.Create();
			var oldValue = parent.ReferenceType;
			var newValue = new RandomObjectGenerator().Generate<string>();

			parent.ReferenceType = newValue;
			parent.ReferenceType = oldValue;

			Assert.AreEqual(oldValue, parent.ReferenceType);
			Assert.IsFalse(parent.IsDirty);
			Assert.IsFalse(parent.Child.IsDirty);
		}

		[TestMethod]
		public void ChangeParentValueType()
		{
			var parent = RootObject.Create();
			var newValue = !parent.ValueType;

			parent.ValueType = newValue;

			Assert.AreEqual(newValue, parent.ValueType);
			Assert.IsTrue(parent.IsDirty);
			Assert.IsFalse(parent.Child.IsDirty);
		}

		[TestMethod]
		public void ChangeParentValueTypeAndUndo()
		{
			var parent = RootObject.Create();
			var oldValue = parent.ValueType;
			var newValue = !parent.ValueType;

			parent.ValueType = newValue;
			parent.ValueType = oldValue;

			Assert.AreEqual(oldValue, parent.ValueType);
			Assert.IsFalse(parent.IsDirty);
			Assert.IsFalse(parent.Child.IsDirty);
		}

		[TestMethod]
		public void ChangeReferenceTypeThatIsNotAString()
		{
			var parent = RootObject.Create();
			var newValue = new RandomObjectGenerator().Generate<Guid>();

			parent.ReferenceTypeNotAString = new ReferenceTypeNotAString(newValue);

			Assert.AreEqual(newValue, parent.ReferenceTypeNotAString.Value);
			Assert.IsTrue(parent.IsDirty);
			Assert.IsFalse(parent.Child.IsDirty);
		}

		[TestMethod]
		public void ChangeReferenceTypeThatIsNotAStringAndUndo()
		{
			var generator = new RandomObjectGenerator();
			var parent = RootObject.Fetch(generator.Generate<string>(), 
				generator.Generate<bool>(), 
				new ReferenceTypeNotAString(generator.Generate<Guid>()));
			var oldValue = parent.ReferenceTypeNotAString.Value;
			var newValue = generator.Generate<Guid>();

			parent.ReferenceTypeNotAString = new ReferenceTypeNotAString(newValue);
			parent.ReferenceTypeNotAString = new ReferenceTypeNotAString(oldValue);

			Assert.AreEqual(oldValue, parent.ReferenceTypeNotAString.Value);
			Assert.IsFalse(parent.IsDirty);
			Assert.IsFalse(parent.Child.IsDirty);
		}

		[TestMethod]
		public void Create()
		{
			var parent = RootObject.Create();
			Assert.IsFalse(parent.IsDirty);
			Assert.IsFalse(parent.Child.IsDirty);
		}

		[TestMethod]
		public void Fetch()
		{
			var generator = new RandomObjectGenerator();
			var referenceType = generator.Generate<string>();
			var valueType = generator.Generate<bool>();
			var referenceTypeNotAStringId = generator.Generate<Guid>();

			var parent = RootObject.Fetch(referenceType, valueType, 
				new ReferenceTypeNotAString(referenceTypeNotAStringId));

			Assert.IsFalse(parent.IsDirty);
			Assert.IsFalse(parent.Child.IsDirty);
		}

		[TestMethod]
		public void SaveChangedChild()
		{
			var parent = RootObject.Create();
			var newValue = new RandomObjectGenerator().Generate<string>();
			parent.Child.ReferenceType = newValue;

			parent = parent.Save();

			Assert.IsFalse(parent.IsDirty);
			Assert.AreEqual(newValue, parent.Child.ReferenceType);
		}

		[TestMethod]
		public void SaveChangedParent()
		{
			var parent = RootObject.Create();
			var newValue = new RandomObjectGenerator().Generate<string>();
			parent.ReferenceType = newValue;

			parent = parent.Save();

			Assert.IsFalse(parent.IsDirty);
			Assert.AreEqual(newValue, parent.ReferenceType);
		}
	}
}
