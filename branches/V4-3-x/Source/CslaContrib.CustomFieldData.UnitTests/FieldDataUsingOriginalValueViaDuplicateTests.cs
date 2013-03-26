using Csla.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Spackle;
using Spackle.Testing;
using System;

namespace CslaContrib.CustomFieldData.UnitTests
{
	[TestClass]
	public sealed class FieldDataUsingOriginalValueViaDuplicateTests
		: CoreTests
	{
		[TestMethod]
		public void CreateWithReferenceType()
		{
			var data = new FieldDataUsingOriginalValueViaDuplicate<string>("name");
			Assert.IsFalse(data.IsDirty);
		}

		[TestMethod]
		public void CreateWithValueType()
		{
			var data = new FieldDataUsingOriginalValueViaDuplicate<int>("name");
			Assert.IsFalse(data.IsDirty);
		}

		[TestMethod]
		public void MarkCleanAndSetDataWithReferenceType()
		{
			var data = new FieldDataUsingOriginalValueViaDuplicate<string>("name");
			data.MarkClean();
			data.Value = new RandomObjectGenerator().Generate<string>();
			Assert.IsFalse(data.IsDirty);
		}

		[TestMethod]
		public void MarkCleanAndSetDataWithValueType()
		{
			var data = new FieldDataUsingOriginalValueViaDuplicate<int>("name");
			data.MarkClean();
			data.Value = new RandomObjectGenerator().Generate<int>();
			Assert.IsFalse(data.IsDirty);
		}

		[TestMethod]
		public void MarkCleanAndSetDataTwiceWithReferenceType()
		{
			var data = new FieldDataUsingOriginalValueViaDuplicate<string>("name");
			data.MarkClean();
			var generator = new RandomObjectGenerator();
			data.Value = generator.Generate<string>();
			data.Value = generator.Generate<string>();
			Assert.IsTrue(data.IsDirty);			
		}

		[TestMethod]
		public void MarkCleanAndSetDataTwiceWithValueType()
		{
			var data = new FieldDataUsingOriginalValueViaDuplicate<int>("name");
			data.MarkClean();
			var generator = new RandomObjectGenerator();
			data.Value = generator.Generate<int>();
			data.Value = generator.Generate<int>();
			Assert.IsTrue(data.IsDirty);
		}

		[TestMethod]
		public void MarkCleanSetDataTwiceAndMarkCleanAgainWithReferenceType()
		{
			var data = new FieldDataUsingOriginalValueViaDuplicate<string>("name");
			data.MarkClean();

			var generator = new RandomObjectGenerator();
			data.Value = generator.Generate<string>();
			data.Value = generator.Generate<string>();
			data.MarkClean();
			Assert.IsFalse(data.IsDirty);			
		}

		[TestMethod]
		public void MarkCleanSetDataTwiceAndMarkCleanAgainWithValueType()
		{
			var data = new FieldDataUsingOriginalValueViaDuplicate<int>("name");
			data.MarkClean();

			var generator = new RandomObjectGenerator();
			data.Value = generator.Generate<int>();
			data.Value = generator.Generate<int>();
			data.MarkClean();
			Assert.IsFalse(data.IsDirty);
		}

		[TestMethod]
		public void MarkCleanSetDataTwiceMarkCleanAgainAndSetDataWithReferenceType()
		{
			var data = new FieldDataUsingOriginalValueViaDuplicate<string>("name");
			data.MarkClean();

			var generator = new RandomObjectGenerator();
			data.Value = generator.Generate<string>();
			data.Value = generator.Generate<string>();
			data.MarkClean();

			data.Value = generator.Generate<string>();
			Assert.IsTrue(data.IsDirty);						
		}

		[TestMethod]
		public void MarkCleanSetDataTwiceMarkCleanAgainAndSetDataWithValueType()
		{
			var data = new FieldDataUsingOriginalValueViaDuplicate<int>("name");
			data.MarkClean();

			var generator = new RandomObjectGenerator();
			data.Value = generator.Generate<int>();
			data.Value = generator.Generate<int>();
			data.MarkClean();

			data.Value = generator.Generate<int>();
			Assert.IsTrue(data.IsDirty);
		}

		[TestMethod]
		public void SetChildValueThatIsDirty()
		{
			var data = new FieldDataUsingOriginalValueViaDuplicate<ITrackStatus>("name");
			Assert.IsFalse(data.IsDirty);

			var newChild = MockRepository.GenerateStub<ITrackStatus>();
			newChild.Expect(e => e.IsDirty).Return(true);
			data.Value = newChild;
			Assert.IsTrue(data.IsDirty);			
		}

		[TestMethod]
		public void SetChildValueThatIsNotDirty()
		{
			var data = new FieldDataUsingOriginalValueViaDuplicate<ITrackStatus>("name");
			Assert.IsFalse(data.IsDirty);

			var newChild = MockRepository.GenerateStub<ITrackStatus>();
			newChild.Expect(e => e.IsDirty).Return(false);
			data.Value = newChild;
			Assert.IsFalse(data.IsDirty);			
		}

		[TestMethod]
		public void SetDataBackToOriginalValueWithReferenceType()
		{
			var data = new FieldDataUsingOriginalValueViaDuplicate<string>("name");
			data.MarkClean();

			var generator = new RandomObjectGenerator();
			var originalValue = generator.Generate<string>();
			data.Value = originalValue;
			data.Value = generator.Generate<string>();
			data.Value = originalValue;
			Assert.IsFalse(data.IsDirty);			
		}

		[TestMethod]
		public void SetDataBackToOriginalValueWithValueType()
		{
			var data = new FieldDataUsingOriginalValueViaDuplicate<int>("name");
			data.MarkClean();

			var generator = new RandomObjectGenerator();
			var originalValue = generator.Generate<int>();
			data.Value = originalValue;
			data.Value = generator.Generate<int>();
			data.Value = originalValue;
			Assert.IsFalse(data.IsDirty);
		}

		[TestMethod]
		public void SetValueWithoutMarkingCleanWithReferenceType()
		{
			var data = new FieldDataUsingOriginalValueViaDuplicate<string>("name");
			data.Value = new RandomObjectGenerator().Generate<string>();
			Assert.IsFalse(data.IsDirty);
		}

		[TestMethod]
		public void SetValueWithoutMarkingCleanWithValueType()
		{
			var data = new FieldDataUsingOriginalValueViaDuplicate<int>("name");
			data.Value = new RandomObjectGenerator().Generate<int>();
			Assert.IsFalse(data.IsDirty);
		}
	}
}
