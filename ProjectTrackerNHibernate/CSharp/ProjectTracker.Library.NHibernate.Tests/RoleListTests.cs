using NUnit.Framework;

namespace ProjectTracker.Library.Tests.RoleListTests
{
	/// <summary>
	/// Unit tests for the <see cref="RoleList.DefaultRole"/> method.
	/// </summary>
	[TestFixture]
	public class DefaultRole
	{
		[Test]
		public void Parameterless()
		{
			int defaultRole = RoleList.DefaultRole();
			Assert.Greater(defaultRole, 0);
		}
	}

	/// <summary>
	/// Unit tests for the <see cref="RoleList.GetList"/> method.
	/// </summary>
	[TestFixture]
	public class GetList
	{
		[Test]
		public void Parameterless()
		{
			RoleList roleList = RoleList.GetList();
			Assert.IsNotNull(roleList);
		}
	}


}