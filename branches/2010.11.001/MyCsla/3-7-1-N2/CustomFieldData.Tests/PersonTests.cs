using Csla;
using Csla.Core.FieldManager;
using CustomFieldData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CustomFieldData.Tests
{
	[TestClass]
	public sealed class PersonTests 
	{
		[ClassInitialize]
		public static void ClassInitialize(TestContext context)
		{
			PropertyInfoFactory.Factory = new PropertyInformationFactory();
		}
		
		[TestMethod]
		public void Fetch()
		{
			var person = Person.Fetch(2047);
			
			Assert.AreEqual(22, person.Age);
			Assert.AreEqual("Joe", person.FirstName);
			Assert.AreEqual("Smith", person.LastName);
			Assert.IsFalse(person.IsNew);
			Assert.IsFalse(person.IsDirty);
		}

		[TestMethod, ExpectedException(typeof(DataPortalException))]
		public void FetchWithPersonIdThatDoesNotExist()
		{
			Person.Fetch(22);
		}
		
		[TestMethod]
		public void ChangeLastNameUsingDifferentValue()
		{
			var person = Person.Fetch(2047);
			person.LastName = Guid.NewGuid().ToString("N");
			Assert.IsTrue(person.IsDirty);
		}

		[TestMethod]
		public void ChangeLastNameUsingSameValue()
		{
			var person = Person.Fetch(2047);
			var lastName = person.LastName;
			person.LastName = Guid.NewGuid().ToString("N");
			person.LastName = lastName;
			Assert.IsFalse(person.IsDirty);
		}

		[TestMethod]
		public void SavePersonWhenLastNameIsChangedUsingDifferentValue()
		{
			var person = Person.Fetch(2047);
			person.LastName = Guid.NewGuid().ToString("N");
			person = person.Save();
			Assert.IsFalse(person.IsDirty);
		}
		
		[TestMethod]
		public void SavePersonWhenLastNameIsChangedUsingSameValue()
		{
			var person = Person.Fetch(2047);
			var lastName = person.LastName;
			person.LastName = Guid.NewGuid().ToString("N");
			person.LastName = lastName;
			person = person.Save();
			Assert.IsFalse(person.IsDirty);
		}

    [TestMethod]
    public void PersonIsDirtyWhenChildObjectIsChanged()
    {
      var person = Person.Fetch(2047);
      Assert.IsFalse(person.IsDirty);
      Assert.IsFalse(person.IsSelfDirty);
      var originalValue = person.Addresses[0].Address1;
      person.Addresses[0].Address1 = "this is a new value";
      Assert.IsTrue(person.IsDirty);
      Assert.IsFalse(person.IsSelfDirty);
      person.Addresses[0].Address1 = originalValue;
      Assert.IsFalse(person.IsDirty);
      Assert.IsFalse(person.IsSelfDirty);
    }

    [TestMethod]
    public void PersonIsDirtyWhenDeleted()
    {
      var person = Person.Fetch(2047);
			
      Assert.IsFalse(person.IsDirty);
      person.Delete();
      Assert.IsTrue(person.IsDirty);
    }
	}
}
