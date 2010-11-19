using System;
using Csla.NHibernate;

namespace ProjectTracker.Library.Framework
{
	/// <summary>
	/// Abstract base class to represent a list of editable Business Objects that are
	/// persisted in the <c>Project Tracker</c> database.
	/// </summary>
	/// <typeparam name="T">A class that inherits from <see cref="NHibernateBusinessListBase{T,C}"/>.</typeparam>
	/// <typeparam name="C">A class that inherits from <see cref="NHibernateBusinessBase{C}"/>.</typeparam>
	[Serializable]
	public abstract class ProjectTrackerBusinessListBase<T, C> : NHibernateBusinessListBase<T, C>
		where T : ProjectTrackerBusinessListBase<T, C>
		where C : ProjectTrackerBusinessBase<C>
	{
		#region NHibernateBusinessListBase<T,C> overrides

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