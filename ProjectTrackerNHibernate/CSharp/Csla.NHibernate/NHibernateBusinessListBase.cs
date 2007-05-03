using System;
using System.Collections;
using System.ComponentModel;
using Iesi.Collections;
using NHibernate;

namespace Csla.NHibernate
{
	/// <summary>
	/// Abstract base class to extend the existing CLSA <see cref="BusinessListBase{T, C}"/>
	/// framework class with NHibernate functionality to provide a list of editable Business Objects.
	/// </summary>
	/// <typeparam name="T">A class that inherits from <see cref="NHibernateBusinessListBase{T,C}"/>.</typeparam>
	/// <typeparam name="C">A class that inherits from <see cref="NHibernateBusinessBase{C}"/>.</typeparam>
	[Serializable]
	public abstract class NHibernateBusinessListBase<T, C> : BusinessListBase<T, C>
		where T : NHibernateBusinessListBase<T, C>
		where C : NHibernateBusinessBase<C>
	{
		#region Csla.BusinessListBase<T,C> overrides

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
				// Create the NHibernate criteria interface needed
				ICriteria nhCriteria = session.CreateCriteria(typeof (C));

				// Get the derived class to setup the specific criteria for this BO
				SetNHibernateCriteria(criteria, nhCriteria);

				// Get the list based on the criteria selected
				IList theList = nhCriteria.List();

				// Now move the objects into the CSLA instance
				Add(theList);
			}
		}

		/// <summary>
		/// Update data using the CSLA Data Portal.
		/// </summary>
		protected override void DataPortal_Update()
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
                        // Stop raising events while the list is modified
                        RaiseListChangedEvents = false;

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
                    finally
					{
                        // Start raising events again
                        RaiseListChangedEvents = true;
					}
				}
			}
		}

		#endregion

		#region non-public helpers

		/// <summary>
		/// Adds Business Objects from an NHibernate generated <see cref="IList"/> to the underlying
		/// CSLA <see cref="BindingList{T}"/> in the current instance.
		/// </summary>
		/// <param name="theList">A reference to an object that implements the <see cref="IList"/> interface.</param>
		/// <remarks>
		/// This method is called when a root editable list is fetched from the database.
		/// </remarks>
		private void Add(IList theList)
		{
			// Stop raising events while the list is modified
			RaiseListChangedEvents = false;

			// The IList contains "dumb" objects so they need to be cast to the correct type
			foreach (object abstractObject in theList)
			{
				// Cast the current item to the correct BO type
				C businessObject = abstractObject as C;

				// If the cast did not work then something has gone wrong
				if (ReferenceEquals(businessObject, null))
					throw new FrameworkException("Object of type '{0}' in NHibernateBusinessListBase '{1}' was null.", (typeof (C)).ToString(), (typeof (T)).ToString());
				
				// Get each object to perform any initialization required on itself...
                // R# warning is a bug http://www.jetbrains.net/jira/browse/RSRP-34235
				businessObject.Init();

				// ...and then add it to the list
				Add(businessObject);

			}

			// Start raising events again
			RaiseListChangedEvents = true;
		}

		#endregion

		#region public methods

		/// <summary>
		/// Adds Business Objects from an NHibernate generated <see cref="ISet"/> to the underlying
		/// CSLA <see cref="BindingList{T}"/> in the current instance.
		/// </summary>
		/// <param name="theSet">A reference to an object that implements the <see cref="ISet"/> interface.</param>
		/// <remarks>
		/// This method is useful for initializing child collections of a root object that have
		/// been created by a relationship that is described by an NHibernate mapping.
		/// </remarks>
		public void Add(ISet theSet)
		{
			// Stop raising events while the list is modified
			RaiseListChangedEvents = false;

			// The ISet actually contains strongly-typed BO instances
			// So it is okay to enumerate them using a foreach loop
			foreach (C businessObject in theSet)
			{
				// Get each object to perform any initialization required on itself...
				businessObject.Init();

				// ...and then add it to the list
				Add(businessObject);
			}

			// Start raising events again
			RaiseListChangedEvents = true;
		}

		/// <summary>
		/// Saves the current list, using NHibernate.
		/// </summary>
		/// <param name="session">A reference to an object that implements the NHibernate <see cref="ISession"/> interface.</param>
		public virtual void Save(ISession session)
		{
			// Enumerate all items in the deleted list
			foreach (C businessObject in DeletedList)
				businessObject.Delete(session);

			// Clear the deleted list (as any items that where in there have now been deleted)
			DeletedList.Clear();	

			// Enumerate all non-deleted items and persist them
			foreach (C businessObject in this)
				businessObject.Save(session);
		}

		#endregion

		#region abstract members (MUST be overridden by a derived class)

		/// <summary>
		/// Gets the key to the database.
		/// </summary>
		protected abstract string DatabaseKey { get; }

		#endregion

		#region virtual members (MAY be overridden by a derived class)

		/// <summary>
		/// Set the NHibernate criteria needed to select Business Objects based on the business criteria.
		/// </summary>
		/// <param name="businessCriteria">The Business Object criteria passed to the CSLA Data Portal.</param>
		/// <param name="nhibernateCriteria">A reference to an object that implements the <see cref="ICriteria"/> interface.</param>
		/// <remarks>
		/// This method contains no implementation, which means that ALL Business Objects are selected by default.
		/// </remarks>
		protected virtual void SetNHibernateCriteria(object businessCriteria, ICriteria nhibernateCriteria) {}

		#endregion
	}
}