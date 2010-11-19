namespace ProjectTracker.Library
{
	/// <summary>
	/// Represents the keys required to access the database.
	/// </summary>
	/// <remarks>
	/// This class completely replaces the CSLA verion.
	/// </remarks>
	public static class Database
	{
		/// <summary>
		/// Gets the key to the Project Tracker database.
		/// </summary>
		public static string ProjectTrackerDatabaseKey
		{
			get { return "DB:PTracker"; }
		}

		/// <summary>
		/// Gets the key to the Security database.
		/// </summary>
		public static string SecurityDatabaseKey
		{
			get { return "DB:Security"; }
		}
	}
}