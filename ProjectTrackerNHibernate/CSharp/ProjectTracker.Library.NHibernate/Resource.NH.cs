using System;
using Csla;
using Iesi.Collections;
using NHibernate;
using NHibernate.Mapping.Attributes;
using ProjectTracker.Library.Framework;

namespace ProjectTracker.Library
{
	/// <summary>
	/// Represents an editable <see cref="Resource"/> Business Object.
	/// </summary>
	[Class(Table = "Resources")]
	public partial class Resource : ProjectTrackerBusinessBase<Resource>
	{
		#region NHibernateBusinessBase<T> overrides

		/// <summary>
		/// Gets the unique identifier that NHibernate uses to get the Business Object,
		/// from the Business Object criteria object.
		/// </summary>
		/// <param name="businessCriteria">The Business Object criteria object.</param>
		protected override object GetUniqueIdentifier(object businessCriteria)
		{
			Criteria criteria = (Criteria) businessCriteria;
			return criteria.Id;
		}

		/// <summary>
		/// Performs initialization tasks on the Business Object.
		/// </summary>
		protected override void Init()
		{
			// The root Resource object is now loaded.  Now need to populate the child ResourceAssignments list.
			// NOTE:
			// The ResourceAssignment BOs are actually already in memory at this point because
			// of the way we have mapped the relationship with NHibernate.  So all we need to do
			// is move the BOs from the NHibernate ISet to the CSLA List.
			_assignments = ResourceAssignments.GetResourceAssignments(_resourceAssignmentsSet);
		}

		/// <summary>
		/// Deletes data using the CSLA Data Portal.
		/// </summary>
		/// <remarks>
		/// This override simply adds the [Transactional] attribute.
		/// </remarks>
		[Transactional(TransactionalTypes.TransactionScope)]
		protected override void DataPortal_DeleteSelf()
		{
			base.DataPortal_DeleteSelf();
		}

		/// <summary>
		/// Inserts data using the CSLA Data Portal.
		/// </summary>
		/// <remarks>
		/// This override simply adds the [Transactional] attribute.
		/// </remarks>
		[Transactional(TransactionalTypes.TransactionScope)]
		protected override void DataPortal_Insert()
		{
			base.DataPortal_Insert();
		}

		/// <summary>
		/// Updates data using the CSLA Data Portal.
		/// </summary>
		/// <remarks>
		/// This override simply adds the [Transactional] attribute.
		/// </remarks>
		[Transactional(TransactionalTypes.TransactionScope)]
		protected override void DataPortal_Update()
		{
		    base.DataPortal_Update();
		}

		/// <summary>
		/// Performs additional processing before a root Business Object is deleted from the database.
		/// </summary>
		/// <param name="session">A reference to an object that implements the NHibernate <see cref="ISession"/> interface.</param>
        /// <remarks>You may wish to override this to perform additional processing before OR after 
        /// a Business Object is deleted from the database. This is usually applicable in parent-child relationships.
        /// If overridden, ensure that you call the base method before or after your code.</remarks>		
		public override void Delete(ISession session)
		{
			// Remove all Assignments from the list (in reverse order)...
			for (int index = _assignments.Count - 1; index >= 0; index-- )
				_assignments.RemoveAt(index);

			// ...before getting the list to persist itself
			_assignments.Save(session);

            base.Delete(session);
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
		    base.Save(session);

			// Check that all assignments have valid links to this BO...
			foreach (ResourceAssignment resourceAssignment in _assignments)
			{
				// If the assignment is new, then set the link to this BO
				if (resourceAssignment.IsNew)
					resourceAssignment.ResourceId = _id;
			}

			// ...before getting the list to persist itself
			_assignments.Save(session);
		}

		#endregion

		#region fields (in database schema order)
		
		[Id(0, Name = "Id", Column = "Id")]
		[Generator(1, Class = "identity")]
		private int _id = 0;
		
		[Property(Name = "LastName", Column = "LastName")]
		private string _lastName = String.Empty;
		
		[Property(Name = "FirstName", Column = "FirstName")]
		private string _firstName = String.Empty;
		
		/// <summary>
		/// Define the relationship to the <see cref="ResourceAssignments"/> class.
		/// </summary>
		/// <remarks>
		/// One <see cref="Resource"/> has many <see cref="ResourceAssignment"/> hence the [OneToMany] relationship.
		/// This relationship is only used to "Fetch" data, never to update it.
		/// Using the Inverse=true mapping means that NHibernate won't try and persist the ISet
		/// back to the database when updating an existing object.
		/// </remarks>
		[Set(0, Name = "ResourceAssignmentsSet", Lazy = false, Inverse = true)]
		[Key(1, Column = "ResourceId")]
		[OneToMany(2, Class = "ProjectTracker.Library.ResourceAssignment, ProjectTracker.Library.NHibernate")]
		private ISet _resourceAssignmentsSet = null;
		
		/// <summary>
		/// Field to store the CSLA <see cref="ResourceAssignments"/>.
		/// </summary>
		private ResourceAssignments _assignments = ResourceAssignments.NewResourceAssignments();
		
		#endregion
	}
}
