using System;
using Csla;
using NHibernate;
using NHibernate.Mapping.Attributes;
using ProjectTracker.Library.Framework;

namespace ProjectTracker.Library
{
	/// <summary>
	/// Represents an editable <see cref="ProjectResource"/> Business Object.
	/// </summary>
	[Class(Table = "Assignments")]
	public partial class ProjectResource : ProjectTrackerBusinessBase<ProjectResource>
	{
		#region NHibernateBusinessBase<T> overrides

		/// <summary>
		/// Performs initialization tasks on the Business Object.
		/// </summary>
		protected override void Init()
		{
			// Ensure we call the base class implementation
			base.Init();

			// Initialize the CSLA SmartDate fields from the NHibernate ones
			// Original CSLA code did this in the DataPortal_Fetch:
			//		_assigned = dr.GetSmartDate("Assigned");
			_assigned = Csla.NHibernate.Convert.ToSmartDate(_assignedOn);

			// Copy the data needed from the "embedded" Resource object into the member fields
			_firstName = _resource.FirstName;
			_lastName = _resource.LastName;
		}

        /// <summary>
        /// Insert or update a Business Object using an NHibernate <see cref="ISession"/>.
        /// </summary>
        /// <param name="session">An object that implements the <see cref="ISession"/> interface.</param>
        /// <remarks>You may wish to override this to perform additional processing before OR after 
        /// a Business Object is deleted from the database. This is usually applicable in parent-child relationships.
        /// If overridden, ensure that you call the base method before or after your code.</remarks>
		public override void Save(ISession session)
		{
			// Convert the CSLA SmartDate to the correct database type
			_assignedOn = Csla.NHibernate.Convert.ToDateTime(_assigned);

            base.Save(session);
		}

		#endregion

		#region fields (in database schema order)

		/// <summary>
		/// Define the composite Primary Key for this class.
		/// </summary>
		[CompositeId(0)]
		[KeyProperty(1, Name = "ProjectId", Column = "ProjectId")]
		[KeyProperty(2, Name = "ResourceId", Column = "ResourceId")]
		private Guid _projectId = Guid.Empty;
		private int _resourceId;

		[Property(Name = "AssignedOn", Column = "Assigned")]
		private DateTime _assignedOn;

		[Property(Name = "Role", Column = "Role")]
		private int _role;

		/// <summary>
		/// Define a relationship to the <see cref="Resource"/> class.
		/// </summary>
		/// <remarks>
		/// A <see cref="ProjectResource"/> object is only valid for a single <see cref="Resource"/>.
		/// Therefore, this maps as a [OneToMany] relationship.
		/// This relationship is only used to "Fetch" data, never to update it.
		/// </remarks>
		[ManyToOne(Name = "Resource", Column = "ResourceId", Class = "ProjectTracker.Library.Resource, ProjectTracker.Library.NHibernate", Unique = true, Insert = false, Update = false)]
		private Resource _resource = null;

		#endregion

		#region extra fields

		// These fields are kept from the CSLA model to ensure all property getters/setters work okay.
		// However, they are not populated directly by NHibernate as the SmartDate type is not
		// "known" by NHibernate without writing a special handler class
		private SmartDate _assigned;

		#endregion

		#region properties

		/// <summary>
		/// Sets the unique identifier to a linked <see cref="Project"/>.
		/// </summary>
		/// <remarks>
		/// Scope is internal so it can be called from the <see cref="Project"/> class.
		/// </remarks>
		internal Guid ProjectId
		{
			set { _projectId = value;  }
		}

		#endregion
	}
}
