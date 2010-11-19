using System;
using Csla;
using NHibernate;
using NHibernate.Expression;
using ProjectTracker.Library.Framework;

namespace ProjectTracker.Library
{
	/// <summary>
	/// Represents a <c>ReadOnly</c> list of <c>ReadOnly</c> <see cref="ProjectInfo"/> objects.
	/// </summary>
	/// <remarks>
	/// This class completely replaces the CSLA version.
	/// </remarks>
	[Serializable]
	public partial class ProjectList : ProjectTrackerReadOnlyListBase<ProjectList, ProjectInfo>
	{
		#region Csla.NHibernate.NHibernateReadOnlyListBase<T,C> overrides

		/// <summary>
		/// Set the NHibernate criteria needed to select the required BOs based on the business selection criteria.
		/// </summary>
		/// <param name="businessCriteria">The criteria passed to the CSLA Data Portal.</param>
		/// <param name="nhibernateCriteria">A reference to the NHibernate ICriteria interface.</param>
		/// <remarks>This method contains no implementation, which means all objects are selected by default.</remarks>
		protected override void SetNHibernateCriteria(object businessCriteria, ICriteria nhibernateCriteria)
		{
			// Cast the criteria back to the strongly-typed version
			Criteria criteria = businessCriteria as Criteria;

			// If it's a valid criteria object then check for filters
			if (!ReferenceEquals(criteria, null))
			{
				// Set a reference to the NHibernate ICriteria (for local use only)
				_iCriteria = nhibernateCriteria;

				// Name
				if (!String.IsNullOrEmpty(criteria.Name))
				{
					AddCriterionName(criteria.Name);
				}
			}
		}

		#endregion

		#region fields

		private ICriteria _iCriteria = null;

		#endregion

		#region constructor

		/// <summary>Direct construction is not allowed.  Use the factory method.</summary>
		private ProjectList() {}

		#endregion

		#region non-public helpers

		/// <summary>
		/// Adds a criterion to the NHibernate <see cref="ICriteria"/> that filters by name.
		/// </summary>
		/// <param name="name">The name to use as a filter.</param>
		private void AddCriterionName(string name)
		{
			EqExpression expression = new EqExpression("Name", name);
			_iCriteria.Add(expression);
		}

		#endregion

		#region factory methods

		/// <summary>Get a list of <see cref="Project"/> objects, using the default <see cref="Criteria"/>.</summary>
		/// <returns>A <see cref="ProjectList"/> instance object.</returns>
		public static ProjectList GetProjectList()
		{
			Criteria criteria = NewCriteria();
			return DataPortal.Fetch<ProjectList>(criteria);
		}

		/// <summary>Get a list of <see cref="ProjectInfo"/> objects.</summary>
		/// <param name="criteria">The criteria used to filter the list.</param>
		/// <returns>A <see cref="ProjectList"/> instance object.</returns>
		public static ProjectList GetProjectList(Criteria criteria)
		{
			return DataPortal.Fetch<ProjectList>(criteria);
		}

		/// <summary>
		/// Factory method to get a <see cref="ProjectList"/> of <see cref="ProjectInfo"/> objects filtered by name.
		/// </summary>
		/// <param name="name">The name to use as a filter.</param>
		/// <returns>A <see cref="ProjectList"/> instance object.</returns>
		public static ProjectList GetProjectList(string name)
		{
			Criteria criteria = NewCriteria();
			criteria.Name = name;
			return DataPortal.Fetch<ProjectList>(criteria);
		}

		/// <summary>Get a new <see cref="Criteria"/> object that can be used to filter the <see cref="ProjectList"/>.</summary>
		/// <returns>An empty <see cref="Criteria"/> instance object.</returns>
		public static Criteria NewCriteria()
		{
			return new Criteria();
		}

		#endregion

		#region embedded Criteria class

		/// <summary>Represents the criteria that can be used to filter the <see cref="ProjectList"/>.</summary>
		/// <remarks>
		/// Although this class is public, the factory method to create a new instance
		/// of this class can only be accessed via the <see cref="ProjectList"/> class.
		/// </remarks>
		[Serializable]
		public partial class Criteria : CriteriaBase
		{
			#region fields

			private string _name = null;

			#endregion

			#region properties

			/// <summary>Gets/sets the name to be used as a filter.</summary>
			public string Name
			{
				get { return _name; }
				set { _name = value; }
			}

			#endregion

			#region constructor

			/// <summary>Creates a new <see cref="Criteria"/> instance.</summary>
			internal Criteria() : base(typeof (ProjectList)) {}

			#endregion
		}

		#endregion
	}
}