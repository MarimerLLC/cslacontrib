using System;
using Csla;
using ProjectTracker.Library.Framework;

namespace ProjectTracker.Library
{
	/// <summary>
	/// Represents a Read-Only List of Read-Only <see cref="ResourceInfo"/> Business Objects.
	/// </summary>
	/// <remarks>
	/// This class completely replaces the CSLA version.
	/// </remarks>
	[Serializable]
	public partial class ResourceList : ProjectTrackerReadOnlyListBase<ResourceList, ResourceInfo>
	{
		#region constructor

		/// <summary>Direct construction is not allowed.  Use the factory method.</summary>
		private ResourceList() {}

		#endregion

		#region factory methods

		/// <summary>Get a list of <see cref="Resource"/> objects, using the default <see cref="Criteria"/>.</summary>
		/// <returns>A <see cref="ResourceList"/> instance object.</returns>
		public static ResourceList GetResourceList()
		{
			Criteria criteria = NewCriteria();
			return GetResourceList(criteria);
		}

		/// <summary>Get a list of <see cref="Resource"/> objects.</summary>
		/// <param name="criteria">The criteria used to filter the list.</param>
		/// <returns>A <see cref="ResourceList"/> instance object.</returns>
		public static ResourceList GetResourceList(Criteria criteria)
		{
			return DataPortal.Fetch<ResourceList>(criteria);
		}

		/// <summary>Get a new <see cref="Criteria"/> object that can be used to filter the <see cref="ResourceList"/>.</summary>
		/// <returns>An empty <see cref="Criteria"/> instance object.</returns>
		public static Criteria NewCriteria()
		{
			return new Criteria();
		}

		#endregion

		#region embedded Criteria class

		/// <summary>
		/// Represents the criteria that can be used to filter the <see cref="ResourceList"/>.
		/// </summary>
		/// <remarks>
		/// Although this class is public, the factory method to create a new instance
		/// of this class can only be accessed via the <see cref="ResourceList"/> class.
		/// </remarks>
		[Serializable]
		public partial class Criteria : CriteriaBase
		{
			#region constructor

			/// <summary>Creates a new <see cref="Criteria"/> instance.</summary>
			internal Criteria() : base(typeof (ResourceList)) {}

			#endregion
		}

		#endregion
	}
}