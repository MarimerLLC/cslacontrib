using Csla.NHibernate;

namespace ProjectTracker.Library.Framework
{
	/// <summary>
	/// Abstract base class to represent a list of Read-Only Business Objects that are
	/// persisted in the <c>Project Tracker</c> database.
	/// </summary>
	/// <typeparam name="T">A class that inherits from <see cref="ProjectTrackerReadOnlyListBase{T,C}"/>.</typeparam>
	/// <typeparam name="C">A class that inherits from <see cref="ProjectTrackerReadOnlyBase{C}"/>.</typeparam>
	public abstract class ProjectTrackerReadOnlyListBase<T, C> : NHibernateReadOnlyListBase<T, C>
		where T : ProjectTrackerReadOnlyListBase<T, C>
		where C : ProjectTrackerReadOnlyBase<C>
	{
		#region NHibernateReadOnlyListBase<T,C> overrides

		/// <summary>
		/// Gets the key in the configuration file to the <c>Project Tracker</c> database.
		/// </summary>
		protected override string DatabaseKey
		{
			get { return Database.ProjectTrackerDatabaseKey; }
		}

		#endregion
	}
}