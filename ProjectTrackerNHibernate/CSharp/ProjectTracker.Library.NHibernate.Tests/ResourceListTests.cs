using NUnit.Framework;

namespace ProjectTracker.Library.Tests.ResourceListTests
{
	/// <summary>
	/// Unit tests for the <see cref="GetResourceList"/> method.
	/// </summary>
	[TestFixture]
	public class GetResourceList
	{
		[Test]
		public void Parameterless()
		{
			ResourceList resourceList = ResourceList.GetResourceList();
			Assert.IsNotNull(resourceList);
		}
	}
}
