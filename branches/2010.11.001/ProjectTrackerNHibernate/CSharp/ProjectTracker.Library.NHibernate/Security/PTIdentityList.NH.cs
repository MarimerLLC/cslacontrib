using System;
using Csla;
using Csla.NHibernate;
using NHibernate;
using NHibernate.Expression;

namespace ProjectTracker.Library.Security
{
	/// <summary>
	/// Represents a Read-Only list of <see cref="PTIdentity"/> objects.
	/// </summary>
	/// <remarks>
	/// This class is used only to perform the login process.
	/// </remarks>
	[Serializable]
	public class PTIdentityList : NHibernateReadOnlyListBase<PTIdentityList, PTIdentity>
	{
		#region NHibernateReadOnlyListBase<T,C> overrides

		/// <summary>
		/// The key in the configuration file to the database.
		/// </summary>
		/// <remarks>
		/// The value in this file is for the Security database.
		/// </remarks>
		protected override string DatabaseKey
		{
			get { return "DB:Security"; }
		}

		/// <summary>
		/// Set the NHibernate criteria needed to select the required BOs based on the business selection criteria.
		/// </summary>
		/// <param name="businessCriteria">The criteria passed to the CSLA Data Portal.</param>
		/// <param name="nhibernateCriteria">A reference to the NHibernate ICriteria interface.</param>
		/// <remarks>This method contains no implementation, which means all objects are selected by default.</remarks>
		protected override void SetNHibernateCriteria(object businessCriteria, ICriteria nhibernateCriteria)
		{
			// Cast the business criteria back to the correct type
			Criteria criteria = businessCriteria as Criteria;

			// If the criteria is valid then choose what settings we need to set
			if (!ReferenceEquals(criteria, null))
			{
				// Set a reference to the NHibernate criteria (for local use only)
				_iCriteria = nhibernateCriteria;

				// Password
				if (!String.IsNullOrEmpty(criteria.Password))
					AddCriterionPassword(criteria.Password);

				// Username
				if (!String.IsNullOrEmpty(criteria.Username))
					AddCriterionUsername(criteria.Username);
			}
		}

		#endregion

		#region fields

		private ICriteria _iCriteria = null;

		#endregion

		#region properties

		/// <summary>Gets the main <see cref="ICriteria"/>.</summary>
		protected ICriteria Icriteria
		{
			get { return _iCriteria; }
		}

		#endregion

		#region constructor

		/// <summary>Direct construction not allowed. Use the factory method.</summary>
		private PTIdentityList() {}

		#endregion

		#region non-public helpers

		/// <summary>Adds an equality expression for the Password.</summary>
		/// <param name="password">The password.</param>
		private void AddCriterionPassword(string password)
		{
			EqExpression expression = new EqExpression("Password", password);
			Icriteria.Add(expression);
		}

		/// <summary>Adds an equality expression for the Username.</summary>
		/// <param name="username">The username.</param>
		private void AddCriterionUsername(string username)
		{
			EqExpression expression = new EqExpression("Username", username);
			Icriteria.Add(expression);
		}

		#endregion

		#region factory methods

		/// <summary>
		/// Factory method to get a new <see cref="Criteria"/> object.
		/// </summary>
		/// <returns>A <see cref="Criteria"/> instance object.</returns>
		public static Criteria NewCriteria()
		{
			return new Criteria();
		}

		/// <summary>Gets a collection of <see cref="PTIdentity"/> objects.</summary>
		/// <returns>A <see cref="PTIdentityList"/> instance object.</returns>
		public static PTIdentityList GetPTIdentityList(Criteria criteria)
		{
			return DataPortal.Fetch<PTIdentityList>(criteria);
		}

		#endregion

		#region embedded Criteria class

		public partial class Criteria : CriteriaBase
		{
			#region fields

			private string _password = String.Empty;
			private string _username = String.Empty;

			#endregion

			#region properties

			/// <summary>Gets/sets the password.</summary>
			public string Password
			{
				get { return _password; }
				set { _password = value; }
			}

			/// <summary>Gets/sets the username.</summary>
			public string Username
			{
				get { return _username; }
				set { _username = value; }
			}

			#endregion

			#region constructor

			/// <summary>Creates a new <see cref="Criteria"/> instance.</summary>
			internal Criteria() : base(typeof (PTIdentity)) {}

			#endregion
		}

		#endregion
	}
}