using System;
using Csla.NHibernate;

namespace ProjectTracker.Library.Framework
{
	/// <summary>
	/// Abstract base class to represent an editable Business Object that is
	/// persisted in the <c>Project Tracker</c> database.
	/// </summary>
	/// <typeparam name="T">A class that inherits from <see cref="ProjectTrackerBusinessBase{T}"/>.</typeparam>
	[Serializable]
	public abstract class ProjectTrackerBusinessBase<T> : NHibernateBusinessBase<T>
		where T : ProjectTrackerBusinessBase<T>
	{
		#region NHibernateBusinessBase<T> overrides

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