using System;
using System.ComponentModel;
using NHibernate.Mapping.Attributes;
using ProjectTracker.Library.Framework;

namespace ProjectTracker.Library.Security
{
	/// <summary>
	/// Represents a role that a user has for authorization control.
	/// </summary>
	[Class(Table = "Roles")]
	[Serializable]
	public class PTIdentityRole : SecurityReadOnlyBase<PTIdentityRole>
	{
		#region Csla.ReadOnlyBase<T> overrides

		/// <summary>Gets an identifier value for this object.</summary>
		/// <returns>The unique identifier for this instance.</returns>
		protected override object GetIdValue()
		{
			return String.Format("{0}-{1}", _username, _role);
		}

		#endregion

		#region fields (in database schema order)

		[CompositeId(0)]
		[KeyProperty(1, Name="Username", Column = "Username")]
		[KeyProperty(2, Name = "Role", Column = "Role")]
		private string _username = String.Empty;
		private string _role = String.Empty;

		#endregion

		#region properties

		/// <summary>Gets the username.</summary>
		[ReadOnly(true)]
		public string Username
		{
			get { return _username; }
		}

		/// <summary>Gets the role.</summary>
		[ReadOnly(true)]
		public string Role
		{
			get { return _role; }
		}

		#endregion

		#region constructors

		/// <summary>Direct construction is not allowed.  Use the factory method.</summary>
		private PTIdentityRole() {}

		#endregion
	}
}