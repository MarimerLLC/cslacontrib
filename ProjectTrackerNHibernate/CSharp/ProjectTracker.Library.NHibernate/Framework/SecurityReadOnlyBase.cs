using Csla.NHibernate;

namespace ProjectTracker.Library.Framework
{
	/// <summary>
	/// Abstract base class to represent a Read-Only Business Object that is
	/// persisted in the <c>Security</c> database.
	/// </summary>
	/// <typeparam name="T">A class that inherits from <see cref="SecurityReadOnlyBase{T}"/>.</typeparam>
	public abstract class SecurityReadOnlyBase<T> : NHibernateReadOnlyBase<T>
		where T : SecurityReadOnlyBase<T>
	{
		#region NHibernateReadOnlyBase<T> overrides

		/// <summary>
		/// Gets the key to the <c>Security</c> database.
		/// </summary>
		protected override string DatabaseKey
		{
			get { return Database.SecurityDatabaseKey; }
		}

		#endregion
	}
}