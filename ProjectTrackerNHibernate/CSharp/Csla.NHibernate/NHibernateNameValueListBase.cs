using System;
using System.Collections;
using NHibernate;

namespace Csla.NHibernate
{
	/// <summary>
	/// Abstract base class to extend the existing CSLA <see cref="NameValueListBase{K,V}"/>
	/// framework class with NHibernate functionality to provide a list of Name-Value pairs.
	/// </summary>
	/// <typeparam name="K">The <see cref="System.Type"/> of the <c>Key</c>.</typeparam>
	/// <typeparam name="V">The <see cref="System.Type"/> of the <c>Value</c>.</typeparam>
	/// <typeparam name="T">A class that inherits from <see cref="NameValueBase{K,V}"/>.</typeparam>
	[Serializable]
	public abstract class NHibernateNameValueListBase<K, V, T> : NameValueListBase<K, V>
		where T : NameValueBase<K, V>
	{
		#region Csla.NameValueListBase<K, V> overrides

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
				ICriteria nhCriteria = session.CreateCriteria(typeof (T));

				// Get the derived class to setup the specific criteria for this BO
				SetNHibernateCriteria(criteria, nhCriteria);

				// Get the list based on the criteria selected
				IList nhibernateList = nhCriteria.List();

				// Now move the references into the CSLA instance
				Add(nhibernateList);
			}
		}

		#endregion

		#region non-public helpers

		/// <summary>
		/// Adds objects from an NHibernate generated <see cref="IList"/> to the underlying
		/// CSLA list.
		/// </summary>
		/// <param name="theList">A reference to an object that implements the <see cref="IList"/> interface.</param>
		private void Add(IList theList)
		{
			// Stop raising list events while changing the list
			RaiseListChangedEvents = false;

			// Make the list not ReadOnly (so it can be changed)
			IsReadOnly = false;

			// The IList contains "dumb" objects so they need to be cast to the correct type
			foreach (object abstractObject in theList)
			{
				// Cast the current item to the correct type
				T concreteObject = abstractObject as T;

				// If the cast did not work then something has gone wrong
				if (ReferenceEquals(concreteObject, null))
					throw new FrameworkException("Object of type '{0}' in NHibernateNameValueListBase was null.",
					                             (typeof (T)).ToString());

				// Convert the object to the correct CSLA type...
                // R# warning is a bug http://www.jetbrains.net/jira/browse/RSRP-34235
				NameValuePair nameValuePair = concreteObject.ToNameValuePair();

				// ...and then add it to the list
				Add(nameValuePair);
			}

			// Make the list ReadOnly again
			IsReadOnly = true;

			// Start raising list events again
			RaiseListChangedEvents = true;
		}

		#endregion

		#region abstract members (MUST be overridden by a derived class)

		/// <summary>
		/// Gets the key in the configuration file to the database.
		/// </summary>
		protected abstract string DatabaseKey { get; }

		#endregion

		#region virtual members (MAY be overridden by a derived class)

		/// <summary>
		/// Set the NHibernate criteria needed to select Name-Value pairs based on the business criteria.
		/// </summary>
		/// <param name="selectionCriteria">The selection criteria passed to the CSLA Data Portal.</param>
		/// <param name="nhibernateCriteria">A reference to an object that implements the <see cref="ICriteria"/> interface.</param>
		/// <remarks>
		/// This method contains no implementation, which means that ALL Name-Value pairs are selected by default.
		/// </remarks>
		protected virtual void SetNHibernateCriteria(object selectionCriteria, ICriteria nhibernateCriteria) {}

		#endregion
	}
}