using Csla.NHibernate;

namespace ProjectTracker.Library.Framework
{
	/// <summary>
	/// Abstract base class to represent a Read-Only Business Object that is
	/// persisted in the <c>Project Tracker</c> database.
	/// </summary>
	/// <typeparam name="T">A class that inherits from <see cref="ProjectTrackerReadOnlyBase{T}"/>.</typeparam>
	public abstract class ProjectTrackerReadOnlyBase<T> : NHibernateReadOnlyBase<T>
		where T : ProjectTrackerReadOnlyBase<T>
	{
		#region NHibernateReadOnlyBase<T> overrides

		/// <summary>
		/// Gets the key to the <c>Project Tracker</c> database.
		/// </summary>
		protected override string DatabaseKey
		{
			get { return Database.ProjectTrackerDatabaseKey; }
		}

		#endregion
	}
}