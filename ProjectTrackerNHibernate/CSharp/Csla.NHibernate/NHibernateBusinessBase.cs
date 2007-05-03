using System;
using System.ComponentModel;
using NHibernate;
using NHibernate.Mapping.Attributes;

namespace Csla.NHibernate
{
	/// <summary>
	/// Abstract base class to extend the existing CLSA <see cref="BusinessBase{T}"/>
	/// framework class with NHibernate functionality.
	/// </summary>
	/// <typeparam name="T">A concrete class that represents a Business Object.</typeparam>
	[Serializable]
	public abstract class NHibernateBusinessBase<T> : BusinessBase<T>
		where T : NHibernateBusinessBase<T>
	{
		#region Csla.Core.BusinessBase overrides

		/// <summary>
		/// Delete data using the CSLA Data Portal.
		/// </summary>
		/// <param name="criteria">The Business Object criteria to identify what to delete.</param>
		/// <remarks>
		/// This method is called from the Business object factory delete method.
		/// It actually removes the data from the database.
		/// </remarks>
		protected override void DataPortal_Delete(object criteria)
		{
			// Get an NHibernate session factory
			ISessionFactory sessionFactory = Cfg.GetSessionFactory(DatabaseKey);

			// Open an NHibernate session from the factory
			using (ISession session = sessionFactory.OpenSession())
			{
				// Begin a transaction on the session
				using (ITransaction transaction = session.BeginTransaction())
				{
					try
					{
						// First load the right BO into memory (this includes any children)...
						Fetch(criteria, session);

						// ... then mark it as "old"
						// Normally the CSLA DataPortal would do this, but as the Fetch() method
						// has been called directly this needs to be done manually
						MarkOld();

						// ... then mark it as deleted (to set the CSLA flags correctly)...
						Delete();

						// ...and then delete it (using the open session)
						Delete(session);

						// ...then commit the transaction
						transaction.Commit();
					}
					catch
					{
						transaction.Rollback();
						throw;
					}
				}
			}
		}

		/// <summary>
		/// Delete data (the current instance) using the CSLA Data Portal.
		/// </summary>
		/// <remarks>
		/// This method is called when a Business Object is deferred deleted.
		/// It actually removes the data from the database.
		/// </remarks>
		protected override void DataPortal_DeleteSelf()
		{
			// Get an NHibernate session factory
			ISessionFactory sessionFactory = Cfg.GetSessionFactory(DatabaseKey);

			// Open an NHibernate session from the factory
			using (ISession session = sessionFactory.OpenSession())
			{
				// Begin a transaction on the session
				using (ITransaction transaction = session.BeginTransaction())
				{
					try
					{
						// Delete the current instance (using the open session)...
						Delete(session);

						// ...then commit the transaction
						transaction.Commit();
					}
					catch
					{
						transaction.Rollback();
						throw;
					}
				}
			}
		}

		/// <summary>
		/// Fetch data using the CSLA Data Portal.
		/// </summary>
		/// <param name="criteria">The Business Object criteria to identify what to fetch.</param>
		protected override void DataPortal_Fetch(object criteria)
		{
			// Get an NHibernate session factory
			ISessionFactory sessionFactory = Cfg.GetSessionFactory(DatabaseKey);

			// Open an NHibernate session from the factory
			using (ISession session = sessionFactory.OpenSession())
			{
				// Fetch the Business Object
				Fetch(criteria, session);
			}
		}

		/// <summary>
		/// Insert data using the CSLA Data Portal.
		/// </summary>
		protected override void DataPortal_Insert()
		{
			DoInsertOrUpdate();
		}

		/// <summary>
		/// Update data using the CSLA Data Portal.
		/// </summary>
		protected override void DataPortal_Update()
		{
			DoInsertOrUpdate();
		}

		#endregion

		#region fields

		/// <summary>
		/// Defines a standard version control field for every Business Object.
		/// </summary>
		/// <remarks>
		/// Will be used by NHibernate to do optimistic concurrency version control.
		/// </remarks>
		[Version(Name = "Version", Column = "Version")]
		private int _version = 0;

		#endregion

		#region properties

		/// <summary>
		/// Gets the version number for this Business Object.
		/// </summary>
		/// <remarks>
		/// The version field would not usually be exposed via the Business Object.
		/// However, it has been done here to aid the unit tests and highlight its usage with NHibernate.
		/// </remarks>
		[ReadOnly(true)]
		public int Version
		{
			get { return _version; }
		}

		#endregion

		#region non-public helpers

		/// <summary>
		/// Insert or update using NHibernate.
		/// </summary>
		private void DoInsertOrUpdate()
		{
			// Get an NHibernate session factory
			ISessionFactory sessionFactory = Cfg.GetSessionFactory(DatabaseKey);

			// Open an NHibernate session from the factory
			using (ISession session = sessionFactory.OpenSession())
			{
				// Begin a transaction on the session
				using (ITransaction transaction = session.BeginTransaction())
				{
					try
					{
						// Save some data...
						Save(session);

						// ...then commit the transaction
						transaction.Commit();
					}
					catch
					{
						transaction.Rollback();
						throw;
					}
				}
			}
		}

		#endregion

		#region public methods

		/// <summary>
		/// Delete a Business Object using an NHibernate <see cref="ISession"/>.
		/// </summary>
		/// <param name="session">An object that implements the <see cref="ISession"/> interface.</param>
        /// <remarks>You may wish to override this to perform additional processing before OR after 
        /// a Business Object is deleted from the database. This is usually applicable in parent-child relationships.
        /// If overridden, ensure that you call the base method before or after your code.</remarks>
		public virtual void Delete(ISession session)
		{
			// If the underlying BO is dirty, then it needs to be deleted
			if (base.IsDirty)
			{
				// If the BO is "old", then it has already been written to the database before and needs to be deleted
				if (!IsNew)
					session.Delete(this);
			}

			// Mark the BO as "new" regardless of its original state (just in case this instance is re-used)
			MarkNew();
		}

		/// <summary>
		/// Fetch a Business Object using an NHibernate <see cref="ISession"/>.
		/// </summary>
		/// <param name="businessCriteria">The Business Object criteria to identify what to fetch.</param>
		/// <param name="session">An object that implements the <see cref="ISession"/> interface.</param>
		protected virtual void Fetch(object businessCriteria, ISession session)
		{
			// Get the unique identifier of this Business Object from the business criteria
			object identifier = GetUniqueIdentifier(businessCriteria);

			// Load the BO into the session using the unique identifier specified
			session.Load(this, identifier);

			// Perform any initialisation required
			Init();
		}

		/// <summary>
		/// Insert or update a Business Object using an NHibernate <see cref="ISession"/>.
		/// </summary>
		/// <param name="session">An object that implements the <see cref="ISession"/> interface.</param>
        /// <remarks>You may wish to override this to perform additional processing before OR after 
        /// a Business Object is deleted from the database. This is usually applicable in parent-child relationships.
        /// If overridden, ensure that you call the base method before or after your code.</remarks>
		public virtual void Save(ISession session)
		{
			// If the underlying BO is dirty, then it needs to be persisted to the database
			if (base.IsDirty)
			{
				// If the BO is new then it's not already in the database, so save it (i.e. INSERT)
				if (IsNew)
				{
					session.Save(this);
				}
					// Otherwise the BO is already in the database, so update it (i.e. UPDATE)
				else
				{
					session.Update(this);
				}

				// Mark the BO as "old" regardless of its original state
				MarkOld();
			}
		}

		#endregion

		#region abstract members (MUST be overridden by a derived class)

		/// <summary>
		/// Gets the key to the database.
		/// </summary>
		protected abstract string DatabaseKey { get; }

		#endregion

		#region virtual methods (MAY be overridden by a derived class)

		/// <summary>
		/// Performs initialization tasks on the Business Object after it has been loaded from the database.
		/// </summary>
		/// <remarks>
		/// Scope is internal so this method can be called from <see cref="NHibernateBusinessListBase{T,C}"/>.
		/// The default behaviour in this base class is to mark the Business Object as "old",
		/// since it has come from the database (therefore it HAS to be "old").
		/// However, this method may be overidden so that additional Business Object specific
		/// initialisation tasks can be carried out.  If this method is overridden, remember to call this
		/// base implementation to make sure the Business Object is marked "old".
		/// </remarks>
		protected internal virtual void Init()
		{
			MarkOld();
		}

		/// <summary>
		/// Gets the unique identifier that NHibernate uses to get a unique Business Object.
		/// </summary>
		/// <param name="businessCriteria">The Business Object criteria object.</param>
		/// <remarks>
		/// This method MUST be overidden if a derived class wants to "Fetch" a unique item.
		/// </remarks>
		protected virtual object GetUniqueIdentifier(object businessCriteria)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}