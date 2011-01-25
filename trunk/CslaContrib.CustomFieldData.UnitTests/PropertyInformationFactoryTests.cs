using Csla;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spackle.Testing;
using System;

namespace CslaContrib.CustomFieldData.UnitTests
{
	[TestClass]
	public sealed class PropertyInformationFactoryTests
		: CoreTests
	{
		[TestMethod]
		public void CreateWithName()
		{
			var prop = new PropertyInformationFactory().Create<string>(
				typeof(PropertyInformationFactoryTests), "name");
			Assert.IsTrue(typeof(PropertyInformationUsingOriginalValue<string>).IsAssignableFrom(prop.GetType()));
		}

		[TestMethod]
		public void CreateWithNameAndFriendlyName()
		{
			var prop = new PropertyInformationFactory().Create<string>(
				typeof(PropertyInformationFactoryTests), "name", "friendlyName");
			Assert.IsTrue(typeof(PropertyInformationUsingOriginalValue<string>).IsAssignableFrom(prop.GetType()));			
		}

		[TestMethod]
		public void CreateWithNameAndFriendlyNameAndRelationship()
		{
			var prop = new PropertyInformationFactory().Create<string>(
				typeof(PropertyInformationFactoryTests), "name", "friendlyName", RelationshipTypes.LazyLoad);
			Assert.IsTrue(typeof(PropertyInformationUsingOriginalValue<string>).IsAssignableFrom(prop.GetType()));						
		}

		[TestMethod]
		public void CreateWithNameAndFriendlyNameAndDefaultValue()
		{
			var prop = new PropertyInformationFactory().Create<string>(
				typeof(PropertyInformationFactoryTests), "name", "friendlyName", (null as string));
			Assert.IsTrue(typeof(PropertyInformationUsingOriginalValue<string>).IsAssignableFrom(prop.GetType()));									
		}

		[TestMethod]
		public void CreateWithNameAndFriendlyNameAndDefaultValueAndRelationship()
		{
			var prop = new PropertyInformationFactory().Create<string>(
				typeof(PropertyInformationFactoryTests), "name", "friendlyName", (null as string), RelationshipTypes.LazyLoad);
			Assert.IsTrue(typeof(PropertyInformationUsingOriginalValue<string>).IsAssignableFrom(prop.GetType()));												
		}
	}
}
