using System;
using System.Runtime.CompilerServices;
using Csla;
using NHibernate.Mapping.Attributes;
using ProjectTracker.Library.Framework;

namespace ProjectTracker.Library
{
	/// <summary>
	/// Represents an editable <see cref="ResourceAssignment"/> Business Object.
	/// </summary>
	/// <remarks>
	/// This class now inherits from the NHibernate base class.
	/// </remarks>
	[Class(Table = "Assignments")]
	public partial class ResourceAssignment : ProjectTrackerBusinessBase<ResourceAssignment>
	{
		#region fields (in database schema order)

		/// <summary>
		/// Define the composite Primary Key for this class.
		/// </summary>
		[CompositeId(0)]
		[KeyProperty(1, Name = "ProjectId", Column = "ProjectId")]
		[KeyProperty(2, Name = "ResourceId", Column = "ResourceId")]
		private Guid _projectId = Guid.Empty;
		private int _resourceId = 0;

		[Property(Name = "AssignedOn", Column = "Assigned")]
		private DateTime _assignedOn = DateTime.Today;

		private SmartDate _assigned = new SmartDate(DateTime.Today);

		[Property(Name = "Role", Column = "Role")]
		private int _role;

		/////// <summary>
		/////// Define a relationship to the <see cref="Project"/> class.
		/////// </summary>
		////[OneToOne(Name = "Project", Class = "ProjectTracker.Library.Project, ProjectTracker.Library.NHibernate", Column = "ProjectId")]
		private Project _project = null;

		#endregion

		#region properties

		/// <summary>
		/// Gets the <see cref="Project"/> associated with this <see cref="ResourceAssignment"/>.
		/// </summary>
		private Project Project
		{
			get
			{
				if (ReferenceEquals(_project, null))
					_project = Project.GetProject(_projectId);
				return _project;
			}
		}

		/// <summary>
		/// Gets the name of the project.
		/// </summary>
		public string ProjectName
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				CanReadProperty(true);
				// Return the name from project instance (was a field in CSLA)
				return Project.Name;
			}
		}

		/// <summary>
		/// Sets the unique identifier to a linked <see cref="Resource"/>.
		/// </summary>
		/// <remarks>
		/// Scope is internal so it can be called from the <see cref="Resource"/> class.
		/// </remarks>
		internal int ResourceId
		{
			set
			{
				_resourceId = value;
			}
		}

		#endregion

		#region constructors

		/// <summary>Direct construction is not allowed.</summary>
		/// <remarks>This is required for NHibernate only.  It is not used.</remarks>
		private ResourceAssignment()
		{
			MarkAsChild();
		}

		/// <summary>
		/// Called to when a new object is created.
		/// </summary>
		private ResourceAssignment(Project project, int role)
		{
			MarkAsChild();
			_projectId = project.Id;
			// The following line differs from the original CSLA implementation of this method
			_project = project; // Save the reference to the project instance object (not just the name)
			_assigned.Date = Assignment.GetDefaultAssignedDate();
			_role = role;
		}

		#endregion

		#region public methods

		/// <summary>
		/// Gets the <see cref="Project"/> object related to this <see cref="ResourceAssignment"/>.
		/// </summary>
		/// <returns>A <see cref="Project"/> instance object.</returns>
		public Project GetProject()
		{
			return Project;
		}

		#endregion
	}
}