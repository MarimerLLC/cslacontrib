using System;
using Csla;
using Csla.NHibernate;
using NHibernate.Mapping.Attributes;
using ProjectTracker.Library.Framework;

namespace ProjectTracker.Library
{
	/// <summary>
	/// Represents a name-value list of roles.
	/// </summary>
	/// <remarks>
	/// This class completely replaces the CSLA version.
	/// </remarks>
	[Serializable]
	public partial class RoleList : ProjectTrackerNameValueListBase<int, string, RoleList.Role>
	{
		#region fields

		/// <summary>Declare field as static for a singleton cache object.</summary>
		private static RoleList _roleList;

		#endregion

		#region constructor

		/// <summary>Direct construction is not allowed.  Use the factory method.</summary>
		private RoleList() {}

		#endregion

		#region factory methods

		/// <summary>
		/// Factory method to get a <see cref="RoleList"/> object.
		/// </summary>
		/// <returns>A <see cref="RoleList"/> instance object.</returns>
		public static RoleList GetList()
		{
			if (ReferenceEquals(_roleList, null))
				_roleList = DataPortal.Fetch<RoleList>(new Criteria(typeof(RoleList)));
			return _roleList;
		}

		/// <summary>
		/// Clears the in-memory cached <see cref="RoleList"/>
		/// so the list of roles is reloaded on next request.
		/// </summary>
		public static void InvalidateCache()
		{
			_roleList = null;
		}

		#endregion

		#region Business Methods (taken from CSLA codeline without change)

		public static int DefaultRole()
		{
			RoleList list = GetList();
			if (list.Count > 0)
				return list.Items[0].Key;
			else
				throw new NullReferenceException(
					"No roles available; default role can not be returned");
		}

		#endregion

		#region embedded Role class

		/// <summary>
		/// Represents a <c>Role</c> name-value pair.
		/// </summary>
		[Class(Table = "Roles")]
		public class Role : NameValueBase<int, string>
		{
			#region NameValuePairBase<K,V> overrides

			public override NameValuePair ToNameValuePair()
			{
				return new NameValuePair(_id, _name);
			}

			#endregion

			#region fields (in database schema order)

			[Id(0, Name = "Id", Column = "Id")]
			[Generator(1, Class = "assigned")]
			private int _id;

			[Property(Name = "Name", Column = "Name")]
			private string _name;

			#endregion

			#region constructor

			/// <summary>Direct construction not allowed.</summary>
			private Role() { }

			#endregion
		}

		#endregion
	}
}