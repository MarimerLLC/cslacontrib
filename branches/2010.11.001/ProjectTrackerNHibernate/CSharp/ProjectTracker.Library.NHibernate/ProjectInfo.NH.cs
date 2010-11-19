using System;
using System.ComponentModel;
using NHibernate.Mapping.Attributes;
using ProjectTracker.Library.Framework;

namespace ProjectTracker.Library
{
	/// <summary>
	/// Represents a Read-Only <see cref="ProjectInfo"/> Business Object.
	/// </summary>
	/// <remarks>
	/// This class completely replaces the CSLA version.
	/// </remarks>
	[Class(Table = "Projects")]
	[Serializable]
	public partial class ProjectInfo : ProjectTrackerReadOnlyBase<ProjectInfo>
	{
		#region Csla.ReadOnlyBase<T> overrides

		/// <summary>Gets an identifier value for this object.</summary>
		/// <returns>The unique identifier for this instance.</returns>
		protected override object GetIdValue()
		{
			return _id;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="ProjectInfo"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current object.</returns>
		public override string ToString()
		{
			return _name;
		}

		#endregion

		#region fields (in database schema order)

		[Id(0, Name = "Id", Column = "Id")]
		[Generator(1, Class = "assigned")]
		private Guid _id = Guid.Empty;

		[Property(Name = "Name", Column = "Name")]
		private string _name = String.Empty;

		#endregion

		#region properties

		/// <summary>Gets the unique identifier.</summary>
		[ReadOnly(true)]
		public Guid Id
		{
			get { return _id; }
		}

		/// <summary>Gets the name.</summary>
		[ReadOnly(true)]
		public string Name
		{
			get { return _name; }
		}

		#endregion

		#region constructor

		/// <summary>Direct construction is not allowed.</summary>
		private ProjectInfo() {}

		#endregion
	}
}