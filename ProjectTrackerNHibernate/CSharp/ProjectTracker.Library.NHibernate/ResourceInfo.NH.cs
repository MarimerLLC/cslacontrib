using System;
using System.ComponentModel;
using Csla.NHibernate;
using NHibernate.Mapping.Attributes;
using ProjectTracker.Library.Framework;

namespace ProjectTracker.Library
{
	/// <summary>
	/// Represents a Read-Only <see cref="ResourceInfo"/> Business Object.
	/// </summary>
	/// <remarks>
	/// This class completely replaces the CSLA version.
	/// </remarks>
	[Class(Table = "Resources")]
	[Serializable]
	public partial class ResourceInfo : ProjectTrackerReadOnlyBase<ResourceInfo>
	{
		#region Csla.ReadOnlyBase<T> overrides
	
		/// <summary>Gets an identifier value for this object.</summary>
		/// <returns>The unique identifier for this instance.</returns>
		protected override object GetIdValue()
		{
			return _id;
		}

		/// <summary>
		/// Converts the current instance to a <see cref="String"/>.
		/// </summary>
		/// <returns>
		/// The <see cref="Name"/> for this <see cref="ResourceInfo"/> instance object.
		/// </returns>
		public override string ToString()
		{
			return Name;
		}
		
		#endregion
		
		#region Nfs.Csla.Core.NhibernateReadOnlyBase<T> overrides

		/// <summary>Gets the unique identifier used by NHibernate to select the object from the database.</summary>
		/// <param name="criteria">The criteria passed to the CSLA DataPortal.</param>
		protected override object GetUniqueIdentifier(object criteria)
		{
			SingleCriteria<int> identity = criteria as SingleCriteria<int>;
			if (!ReferenceEquals(identity, null))
			{
				return identity.Value;
			}
			else
			{
				return null;
			}
		}

		#endregion
				
		#region fields (in database schema order)
		
		[Id(0, Name = "Id", Column = "Id")]
		[Generator(1, Class = "assigned")]
		private int _id = 0;
		
		[Property(Name = "LastName", Column = "LastName")]
		private string _lastName = null;
		
		[Property(Name = "FirstName", Column = "FirstName")]
		private string _firstName = null;
		
		#endregion
				
		#region properties
		
		/// <summary>Gets the unique identifier.</summary>
		[ReadOnly(true)]
		public int Id
		{
			get {return _id;}
		}
		
		/// <summary>Gets the full name.</summary>
		/// <remarks>Returns he full name in "Lastname, Firstname" format.</remarks>
		[ReadOnly(true)]
		public string Name
		{
			get { return string.Format("{0}, {1}", _lastName, _firstName); }
		}
		
		#endregion
		
		#region constructor
		
		/// <summary>Direct construction is not allowed.  Use the factory method.</summary>
		private ResourceInfo() { }
		
		#endregion
	}
}
