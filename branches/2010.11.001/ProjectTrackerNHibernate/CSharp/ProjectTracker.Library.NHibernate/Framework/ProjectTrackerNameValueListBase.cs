using System;
using Csla.NHibernate;

namespace ProjectTracker.Library.Framework
{
	/// <summary>
	/// Abstract base class to represent a list of name-value pairs
	/// persisted in the <c>Project Tracker</c> database.
	/// </summary>
	/// <typeparam name="K"></typeparam>
	/// <typeparam name="V"></typeparam>
	/// <typeparam name="T"></typeparam>
	[Serializable]
	public abstract class ProjectTrackerNameValueListBase<K, V, T> : NHibernateNameValueListBase<K, V, T>
		where T : NameValueBase<K, V>
	{
		#region NHibernateNameValueListBase<K,V,T> overrides

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