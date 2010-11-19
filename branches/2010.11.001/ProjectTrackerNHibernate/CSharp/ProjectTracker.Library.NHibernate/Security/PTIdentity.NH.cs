using System;
using Csla;
using Iesi.Collections;
using NHibernate;
using NHibernate.Mapping.Attributes;

namespace ProjectTracker.Library.Security
{
	/// <summary>
	/// Represents the code required to integrate the <see cref="PTIdentity"/> class with NHibernate.
	/// </summary>
	[Class(Table = "Users")]
	public partial class PTIdentity : Csla.NHibernate.NHibernateReadOnlyBase<PTIdentity>
	{
		#region NHibernateReadOnlyBase<T> overrides

		/// <summary>
		/// The key in the configuration file to the database.
		/// </summary>
		protected override string DatabaseKey
		{
			get { return Database.SecurityDatabaseKey; }
		}

		/// <summary>
		/// Loads the object using the NHibernate current <see cref="ISession"/>.
		/// </summary>
		/// <param name="businessCriteria">The Business Object criteria object.</param>
		/// <param name="session">A reference to an NHibernate <see cref="ISession"/> object.</param>
		protected override void Fetch(object businessCriteria, ISession session)
		{
			// Load the instance using the standard base class methods
			base.Fetch(businessCriteria, session);

			// Now set specific fields that are not derived from the DB
			_isAuthenticated = true;
			_name = _username;
		}

		/// <summary>
		/// Gets the unique identifier that NHibernate uses to get the Business Object,
		/// from the Business Object criteria object.
		/// </summary>
		/// <param name="criteria">The Business Object criteria object.</param>
		protected override object GetUniqueIdentifier(object criteria)
		{
			Criteria identityCriteria = (Criteria) criteria;
			return identityCriteria.Username;
		}

		#endregion

		#region fields

		[Id(0, Name = "Username", Column = "Username")]
		[Generator(1, Class = "assigned")]
		private string _username = String.Empty;

		[Property(Name = "Password", Column = "Password")]
		private string _password = String.Empty;

		[Set(0, Name = "RoleSet", Lazy = false)]
		[Key(1, Column = "Username")]
		[OneToMany(2, Class = "ProjectTracker.Library.Security.PTIdentityRole, ProjectTracker.Library.NHibernate")]
		private ISet _roleSet = null;

		#endregion

		#region business methods

		/// <summary>
		/// Determines whether the current <see cref="PTIdentity"/> is in the specified role.
		/// </summary>
		/// <param name="role">The name of the role.</param>
		/// <returns>True, if the current <see cref="PTIdentity"/> is in the role; false otherwise.</returns>
		internal bool IsInRole(string role)
		{
			if (!ReferenceEquals(_roleSet, null))
			{
				foreach (PTIdentityRole identityRole in _roleSet)
				{
					if (identityRole.Role.Equals(role))
						return true;
				}				
			}
			return false;
		}

		#endregion

		#region factory methods

		private static PTIdentity GetIdentity(Criteria criteria)
		{
			return DataPortal.Fetch<PTIdentity>(criteria);
		}

		/// <summary>
		/// Factory method to get a <see cref="PTIdentity"/> object, for a specified username/password combination.
		/// </summary>
		/// <param name="username">The username.</param>
		/// <param name="password">The password.</param>
		/// <returns>A <see cref="PTIdentity"/> instance object.</returns>
		internal static PTIdentity GetIdentity(string username, string password)
		{
			// Set default as being unauthenticated identity
			PTIdentity identity = UnauthenticatedIdentity();

			// Get a criteria to use against the list
			PTIdentityList.Criteria criteria = PTIdentityList.NewCriteria();
			criteria.Username = username;
			criteria.Password = password;

			// Get a list of identities matching the criteria (should return only 1)
			PTIdentityList identityList = PTIdentityList.GetPTIdentityList(criteria);

			// If we have exactly 1 identity then set that identity
			if (identityList.Count == 1)
			{
				Criteria identityCriteria = new Criteria(username, password);
				identity = GetIdentity(identityCriteria);
			}

			return identity;
		}
		 
		#endregion
	}
}
