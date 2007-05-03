using System;
using NHibernate;

namespace Csla.NHibernate
{
	/// <summary>
	/// Abstract base class to extend the existing CSLA <see cref="ReadOnlyBase{T}"/>
	/// framework class with NHibernate functionality.
	/// </summary>
	/// <typeparam name="T">A class that inherits from <see cref="NHibernateReadOnlyBase{T}"/>.</typeparam>
	[Serializable]
	public abstract class NHibernateReadOnlyBase<T> : ReadOnlyBase<T>
		where T : NHibernateReadOnlyBase<T>
	{
		#region Csla.ReadOnlyBase<T> overrides

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

		#endregion

		#region abstract members (MUST be overridden by a derived class)

		/// <summary>
		/// Gets the key to the database.
		/// </summary>
		protected abstract string DatabaseKey { get; }

		#endregion

		#region virtual methods (MAY be overridden by a derived class)

		/// <summary>
		/// Gets the unique identifier that NHibernate uses to get the Business Object,
		/// from the Business Object criteria object.
		/// </summary>
		/// <param name="criteria">The Business Object criteria object.</param>
		/// <remarks>
		/// This method must be overidden if a derived class wants to "Get" a unique item.
		/// </remarks>
		protected virtual object GetUniqueIdentifier(object criteria)
		{
			throw new NotImplementedException();
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

			// Use the current session to load this instance using the unique identifier specified
			session.Load(this, identifier);

			// Perform any initialisation required
			Init();
		}

		#endregion

		#region virtual methods (MAY be overridden by a derived class)

		/// <summary>
		/// Performs initialization tasks on the Business Object after it has been loaded from the database.
		/// </summary>
		/// <remarks>
		/// Scope is internal so this methid can be called from <see cref="NHibernateReadOnlyListBase{T,C}"/>.
		/// </remarks>
		protected internal virtual void Init() {}

		#endregion
	}
}