namespace ProjectTracker.Library.Tests.Framework
{
	/// <summary>
	/// Represents an abstract base class for unit tests that require a user to be authenticated
	/// and assigned to the Role of ProjectManager.
	/// </summary>
	public abstract class AuthenticatedProjectManagerTestBase : AuthenticatedTestBase
	{
		#region AuthenticatedTestBase overrides

		/// <summary>Gets the Username.</summary>
		public override string Username
		{
			get { return "Project Manager"; }
		}

		/// <summary>Gets the password.</summary>
		public override string Password
		{
			get { return "password"; }
		}

		#endregion
	}
}
