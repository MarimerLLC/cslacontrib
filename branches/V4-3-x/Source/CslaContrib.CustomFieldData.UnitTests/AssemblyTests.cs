using Csla.Core.FieldManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CslaContrib.CustomFieldData.UnitTests
{
	[TestClass]
	public class AssemblyTests
	{
		[AssemblyInitialize]
		public static void AssemblyInitialize(TestContext context)
		{
			PropertyInfoFactory.Factory = new PropertyInformationFactory();
		}
	}
}
