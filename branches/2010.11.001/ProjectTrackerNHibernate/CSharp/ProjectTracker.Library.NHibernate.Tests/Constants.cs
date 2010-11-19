namespace ProjectTracker.Library.Tests
{
	/// <summary>
	/// Represents the set of "hard-coded" values that are needed to make the unit tests work.
	/// </summary>
	internal static class Constants
	{
		internal static class User
		{
			private const string _invalidUsername = "MickeyMouse";
			private const string _invalidPassword = "";
			private const string _validUsername = "Administrator";
			private const string _validPassword = "password";

			internal static string InvalidPassword
			{
				get { return _invalidPassword; }
			}

			internal static string InvalidUsername
			{
				get { return _invalidUsername; }
			}

			internal static string ValidPassword
			{
				get { return _validPassword; }
			}

			internal static string ValidUsername
			{
				get { return _validUsername; }
			}
		}

		internal static class Role
		{
			private const string _invalidRole = "CEO";
			private const string _validRole = "Administrator";

			internal static string InvalidRole
			{
				get { return _invalidRole; }
			}

			internal static string ValidRole
			{
				get { return _validRole; }
			}
		}
	}
}