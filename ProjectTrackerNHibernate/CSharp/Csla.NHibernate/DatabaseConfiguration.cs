using NHibernate;

namespace Csla.NHibernate
{
	/// <summary>
	/// Represents a class that is used to configure NHibernate against a target database.
	/// </summary>
	/// <remarks>
	/// This class is marked internal as it is not intended for public use
	/// outside the main assembly that handles the NHibernate integration.
	/// </remarks>
	internal class DatabaseConfiguration
	{
		#region fields

		private string _connectionString = null;
		private string _key = null;
		private ISessionFactory _sessionFactory = null;

		#endregion

		#region properties

		/// <summary>Gets/sets the connection string to the target database.</summary>
		public string ConnectionString
		{
			get { return _connectionString; }
			set { _connectionString = value; }
		}

		/// <summary>Gets the key used to access the target database.</summary>
		/// <remarks>The key is usually read from the application configuration file.</remarks>
		public string Key
		{
			get { return _key; }
		}

		/// <summary>Gets/sets the <see cref="ISessionFactory"/> for the target database.</summary>
		public ISessionFactory SessionFactory
		{
			get { return _sessionFactory; }
			set { _sessionFactory = value; }
		}

		#endregion

		#region constructor

		/// <summary>Direct construction not allowed. Use the factory method.</summary>
		private DatabaseConfiguration() {}

		#endregion

		#region factory method(s)

		/// <summary>
		/// Factory method to create a new <see cref="DatabaseConfiguration"/> object.
		/// </summary>
		/// <param name="key">The key for the target database required.</param>
		/// <returns>A <see cref="DatabaseConfiguration"/> instance object.</returns>
		public static DatabaseConfiguration NewDatabaseConfiguration(string key)
		{
			DatabaseConfiguration databaseConfiguration = new DatabaseConfiguration();
			databaseConfiguration._key = key;
			return databaseConfiguration;
		}

		#endregion
	}
}