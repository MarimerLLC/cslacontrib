using NUnit.Framework;
using ProjectTracker.Library.Tests.Framework;

namespace ProjectTracker.Library.Admin.Tests.RolesTest
{
	/// <summary>
	/// Unit tests for the <see cref="Roles.GetRoles"/> method.
	/// </summary>
	[TestFixture]
	public class GetRoles
	{
		[Test]
		public void Parameterless()
		{
			Roles roles = Roles.GetRoles();
			Assert.IsNotNull(roles);
		}
	}

	/// <summary>
	/// Unit tests for the <see cref="Roles.Save()"/> method.
	/// </summary>
	[TestFixture]
	public class Save : AuthenticatedAdministratorTestBase
	{
        [Test]
        public void AddNewRoleSaveThenDelete()
        {
			// Get the existing roles
			Roles roles = Roles.GetRoles();

        	// Add a new role...
			Role role = roles.AddNew();
        	role.Id = 999;
        	role.Name = "Road Sweeper";

        	// and save it
			roles = roles.Save();
        	Assert.IsNotNull(roles);

			// Delete the role...
        	roles.Remove(role);

			// ...and save it
        	roles = roles.Save();
			Assert.IsNotNull(roles);
        }

		[Test]
		public void AddNewRoleThenDelete()
		{
			// Get the roles
			Roles roles = Roles.GetRoles();

			// Add a new role
			Role role = roles.AddNew();
			role.Id = 999;
			role.Name = "Road Sweeper";

			// Delete the role...
			roles.Remove(role);

			// ...and save it
			roles = roles.Save();
			Assert.IsNotNull(roles);
		}
	}
}