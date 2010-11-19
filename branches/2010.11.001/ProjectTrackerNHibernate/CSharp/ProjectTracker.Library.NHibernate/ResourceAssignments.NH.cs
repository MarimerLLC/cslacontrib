using Iesi.Collections;
using ProjectTracker.Library.Framework;

namespace ProjectTracker.Library
{
	/// <summary>
	/// Represents an editable list of <see cref="ResourceAssignment"/> Business Objects.
	/// </summary>
	public partial class ResourceAssignments : ProjectTrackerBusinessListBase<ResourceAssignments, ResourceAssignment>
	{
		#region factory methods

		/// <summary>
		/// Factory method to return a <see cref="ResourceAssignments"/> object,
		/// given an <see cref="ISet"/> object returned by NHibernate.
		/// </summary>
		/// <param name="resourceAssignmentsSet">Reference to an object that implements the <see cref="ISet"/> object.</param>
		/// <returns>A <see cref="ResourceAssignments"/> instance object.</returns>
		internal static ResourceAssignments GetResourceAssignments(ISet resourceAssignmentsSet)
		{
			// Create a new empty instance and add the ISet items into the List
			ResourceAssignments resourceAssignments = NewResourceAssignments();
			resourceAssignments.Add(resourceAssignmentsSet);
			return resourceAssignments;
		}

		#endregion
	}
}
