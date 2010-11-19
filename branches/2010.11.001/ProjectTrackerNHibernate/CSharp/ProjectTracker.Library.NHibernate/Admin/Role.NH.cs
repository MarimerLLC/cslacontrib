using System;
using NHibernate.Mapping.Attributes;
using ProjectTracker.Library.Framework;

namespace ProjectTracker.Library.Admin
{
	/// <summary>
	/// Represents an editable <see cref="Role"/> Business Object.
	/// </summary>
	[Class(Table = "Roles")]
	public partial class Role : ProjectTrackerBusinessBase<Role>
	{
		/// <summary>
		/// Performs initialization tasks on the Business Object after it has been loaded from the database.
		/// </summary>
		protected override void Init()
		{
			// Ensure the base implementation is called
			base.Init();
			
			// Initialize the private boolean field
			// Original CSLA code did this in the Role(SafeDataReader dr) constructor:
			//    _idSet = true;
			_idSet = true;
		}

		#region fields (in database schema order)

		[Id(0, Name = "Id", Column = "Id")]
		[Generator(1, Class = "assigned")]
		private int _id = 0;

		[Property(Name = "Name", Column = "Name")]
		private string _name = String.Empty;

		#endregion
	}
}
