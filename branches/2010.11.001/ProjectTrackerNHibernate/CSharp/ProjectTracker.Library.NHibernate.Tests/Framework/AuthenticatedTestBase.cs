using NUnit.Framework;
using ProjectTracker.Library.Security;

namespace ProjectTracker.Library.Tests.Framework
{
	/// <summary>
	/// Abstract base class to provide the basis for a unit test that requires an authenticated user.
	/// </summary>
	public abstract class AuthenticatedTestBase
	{
		#region abstract properties (must be implemented in the derived class)

		/// <summary>Gets the Username.</summary>
		public abstract string Username { get; }

		/// <summary>Gets the password.</summary>
		public abstract string Password { get; } 

		#endregion

		#region SetUp / TearDown

		/// <summary>
		/// Ensure the user logs in and authenticates during the SetUp.
		/// </summary>
		[SetUp]
		public void SetUp()
		{
			bool isAuthenticated = PTPrincipal.Login(Username, Password);
			Assert.IsTrue(isAuthenticated);
		}

		/// <summary>
		/// Ensure the user logs out during the TearDown.
		/// </summary>
		[TearDown]
		public void TearDown()
		{
			PTPrincipal.Logout();
		}

		#endregion
	}
}