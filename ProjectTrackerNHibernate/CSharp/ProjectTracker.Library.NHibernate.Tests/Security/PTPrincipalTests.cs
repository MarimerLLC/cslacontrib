using System.Security.Principal;
using NUnit.Framework;
using ProjectTracker.Library.Security;
using ProjectTracker.Library.Tests;

namespace ProjectTracker.Library.Security.Tests.PTPrincipalTests
{
	#region GetIdentity() method tests

	/// <summary>
	/// Unit tests for the <see cref="Login"/> method.
	/// </summary>
	[TestFixture]
	public class Login
	{
		[Test]
		public void ValidUsernameValidPassword()
		{
			bool isAuthenticated = PTPrincipal.Login(Constants.User.ValidUsername, Constants.User.ValidPassword);
			Assert.IsTrue(isAuthenticated);
		}

		[Test]
		public void InvalidUsernameInvalidPassword()
		{
			bool isAuthenticated = PTPrincipal.Login(Constants.User.InvalidUsername, Constants.User.InvalidPassword);
			Assert.IsFalse(isAuthenticated);
		}
	}

	#endregion

	#region IsInRole() method tests

	/// <summary>
	/// Unit tests for the <see cref="IsInRole"/> method.
	/// </summary>
	[TestFixture]
	public class IsInRole
	{
		[Test]
		public void WithValidRole()
		{
			// Authenticate into the current context
			PTPrincipal.Login(Constants.User.ValidUsername, Constants.User.ValidPassword);

			// Now get the principal off the current context
			IPrincipal principal = Csla.ApplicationContext.User;
			bool isInRole = principal.IsInRole(Constants.Role.ValidRole);
			Assert.IsTrue(isInRole);
		}

		[Test]
		public void WithInvalidRole()
		{
			// Authenticate into the current context
			PTPrincipal.Login(Constants.User.ValidUsername, Constants.User.ValidPassword);

			// Now get the principal off the current context
			IPrincipal principal = Csla.ApplicationContext.User;
			bool isInRole = principal.IsInRole(Constants.Role.InvalidRole);
			Assert.IsFalse(isInRole);
		}
	}

	#endregion
}
