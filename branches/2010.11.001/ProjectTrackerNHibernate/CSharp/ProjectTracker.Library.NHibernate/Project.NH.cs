using System;
using Csla;
using Iesi.Collections;
using NHibernate;
using NHibernate.Mapping.Attributes;
using Nullables;
using ProjectTracker.Library.Framework;
using Convert=Csla.NHibernate.Convert;

namespace ProjectTracker.Library
{
    /// <summary>
    /// Represents an editable <see cref="Project"/> Business Object.
    /// </summary>
    [Class(Table = "Projects")]
    public partial class Project : ProjectTrackerBusinessBase<Project>
    {
        #region NHibernateBusinessBase<T> overrides

        /// <summary>
        /// Gets the unique identifier that NHibernate uses to get the Business Object,
        /// from the Business Object criteria object.
        /// </summary>
        /// <param name="businessCriteria">The Business Object criteria object.</param>
        protected override object GetUniqueIdentifier(object businessCriteria)
        {
            //There's only 1 way to get the unique identifer.
            //If there are more Criteria types defined then only one
            //of them can be used to identify a unique instance.
            //This decision was made over MSN between DD and HP on 2nd March 2007
            Criteria criteria = (Criteria) businessCriteria;
            return criteria.Id;
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
        /// Performs initialization tasks on the Business Object.
        /// </summary>
        /// <remarks>
        /// Initializes CSLA specific fields based on the data that was read
        /// into the fields marked up for NHibernate.
        /// </remarks>
        protected override void Init()
        {
            // Initialize the CSLA SmartDate fields from the NHibernate ones
            // Original CSLA code did this in the DataPortal_Fetch:
            //		_started = dr.GetSmartDate("Started", _started.EmptyIsMin);
            //		_ended = dr.GetSmartDate("Ended", _ended.EmptyIsMin);
            _started = Convert.ToSmartDate(_startedOn, _started.EmptyIsMin);
            _ended = Convert.ToSmartDate(_endedOn, _ended.EmptyIsMin);

            // The Project object is now loaded, so populate the child ProjectResources list.
            // NOTE:
            // The ProjectResources BOs are actually already in memory at this point because
            // of the way we have mapped the relationship with NHibernate.  So all we need to do
            // is move the BOs from the NHibernate ISet to the CSLA List.
            _resources.Add(_projectResourcesSet);
        }

        /// <summary>
        /// Delete a Business Object using an NHibernate <see cref="ISession"/>.
        /// </summary>
        /// <param name="session">An object that implements the <see cref="ISession"/> interface.</param>
        /// <remarks>You may wish to override this to perform additional processing before OR after 
        /// a Business Object is deleted from the database. This is usually applicable in parent-child relationships.
        /// If overridden, ensure that you call the base method before or after your code.</remarks>
        public override void Delete(ISession session)
        {
            // Remove all Assignments from the list (in reverse order)...
            for (int index = _resources.Count - 1; index >= 0; index--)
                _resources.RemoveAt(index);

            // ...before getting the list to persist itself
            _resources.Save(session);

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
            // Convert the CSLA SmartDate to the correct database type
            _startedOn = Convert.ToNullableDateTime(_started);
            _endedOn = Convert.ToNullableDateTime(_ended);

            base.Save(session);

            // Check that all resources have valid links to this BO...
            foreach (ProjectResource projectResource in _resources)
            {
                // If new, then set the link to this BO correctly
                if (projectResource.IsNew)
                    projectResource.ProjectId = _id;
            }

            // Get the list to persist itself
            _resources.Save(session);
        }

        #endregion

        #region fields (in database schema order)

        [Id(0, Name = "Id", Column = "Id")]
        [Generator(1, Class = "assigned")]
        private Guid _id = Guid.Empty;

        [Property(Name = "Name", Column = "Name")]
        private string _name = String.Empty;

        [Property(Name = "StartedOn", Column = "Started")]
        private NullableDateTime _startedOn = null;

        [Property(Name = "EndedOn", Column = "Ended")]
        private NullableDateTime _endedOn = null;

        [Property(Name = "Description", Column = "Description")]
        private string _description = String.Empty;

        /// <summary>
        /// Define the relationship to the <see cref="ProjectResource"/> class.
        /// </summary>
        /// <remarks>
        /// One <see cref="Project"/> has many <see cref="ProjectResource"/> hence the [OneToMany] relationship.
        /// This relationship is only used to "Fetch" data, never to update it.
        /// Using the Inverse=true mapping means that NHibernate won't try and persist the ISet
        /// back to the database when updating an existing object.
        /// </remarks>
        [Set(0, Name = "ProjectResourcesSet", Lazy = false, Inverse = true)]
        [Key(1, Column = "ProjectId")]
        [OneToMany(2, Class = "ProjectTracker.Library.ProjectResource, ProjectTracker.Library.NHibernate")]
        private ISet _projectResourcesSet = null;

        /// <summary>
        /// Field to store the CSLA <see cref="ProjectResources"/>.
        /// </summary>
        private ProjectResources _resources = ProjectResources.NewProjectResources();

        #endregion

        #region extra fields

        // These fields are kept from the CSLA model to ensure the property getters/setters work okay.
        // However, they are not populated directly by NHibernate as the SmartDate type is not
        // "known" by NHibernate without writing a special handler class

        private SmartDate _started;
        private SmartDate _ended = new SmartDate(false);

        #endregion
    }
}