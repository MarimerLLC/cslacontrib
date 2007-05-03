using System;
using System.Collections;
using System.ComponentModel;
using NHibernate;

namespace Csla.NHibernate
{
	/// <summary>
	/// Abstract base class to extend the existing CSLA <see cref="ReadOnlyListBase{T,C}"/>
	/// framework class with NHibernate functionality to provide a list of Read-Only Business Objects.
	/// </summary>
	/// <typeparam name="T">A class that inherits from <see cref="NHibernateReadOnlyListBase{T,C}"/>.</typeparam>
	/// <typeparam name="C">A class that inherits from <see cref="NHibernateReadOnlyBase{C}"/>.</typeparam>
	[Serializable]
	public abstract class NHibernateReadOnlyListBase<T, C> : ReadOnlyListBase<T, C>
		where T : NHibernateReadOnlyListBase<T, C>
		where C : NHibernateReadOnlyBase<C>
	{
		#region Csla.ReadOnlyListBase<T,C> overrides

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

				// Now move the references into the CSLA instance
				Add(theList);
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
		/// This method is called when a root ReadOnly list is fetched from the database.
		/// </remarks>
		private void Add(IList theList)
		{
			// Make the list not ReadOnly (so it can be changed)
			IsReadOnly = false;

			// The IList contains "dumb" objects so they need to be cast to the correct type
			foreach (object abstractObject in theList)
			{
				// Cast the current item to the correct BO type
				C businessObject = abstractObject as C;

				// If the cast did not work then something has gone wrong
				if (ReferenceEquals(businessObject, null))
					throw new FrameworkException("Object of type '{0}' in NHibernateReadOnlyListBase '{1}' was null.", (typeof(C)).ToString(), (typeof(T)).ToString());

				// Get each object to perform any initialization required on itself...
                // R# warning is a bug http://www.jetbrains.net/jira/browse/RSRP-34235
				businessObject.Init();

				// ...and then add it to the list
				Add(businessObject);
			}

			// Make the list ReadOnly again
			IsReadOnly = true;
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