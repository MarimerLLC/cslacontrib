namespace ProjectTracker.Library.Tests.Framework
{
	/// <summary>
	/// Represents an abstract base class for unit tests that require a user to be authenticated
	/// and assigned to the Role of Administrator.
	/// </summary>
	public abstract class AuthenticatedAdministratorTestBase : AuthenticatedTestBase
	{
		#region AuthenticatedTestBase overrides

		/// <summary>Gets the Username.</summary>
		public override string Username
		{
			get { return "Administrator"; }
		}

		/// <summary>Gets the password.</summary>
		public override string Password
		{
			get { return "password"; }
		}

		#endregion
	}
}
