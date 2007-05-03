using System;

namespace ProjectTracker.Library
{
	/// <summary>
	/// Represents the original CSLA <see cref="ResourceAssignments"/> class code.
	/// </summary>
	/// <remarks>
	/// This class changed to a partial class to illustrate the NHibernate functionality separately.
	/// </remarks>
	[Serializable()]
	public partial class ResourceAssignments
	{
		#region Business Methods (no change)

		public ResourceAssignment this[Guid projectId]
		{
			get
			{
				foreach (ResourceAssignment res in this)
					if (res.ProjectId.Equals(projectId))
						return res;
				return null;
			}
		}

		public void AssignTo(Guid projectId)
		{
			if (!Contains(projectId))
			{
				ResourceAssignment project = ResourceAssignment.NewResourceAssignment(projectId);
				Add(project);
			}
			else
				throw new InvalidOperationException("Resource already assigned to project");
		}

		public void Remove(Guid projectId)
		{
			foreach (ResourceAssignment res in this)
			{
				if (res.ProjectId.Equals(projectId))
				{
					Remove(res);
					break;
				}
			}
		}

		public bool Contains(Guid projectId)
		{
			foreach (ResourceAssignment project in this)
				if (project.ProjectId == projectId)
					return true;
			return false;
		}

		public bool ContainsDeleted(Guid projectId)
		{
			foreach (ResourceAssignment project in DeletedList)
				if (project.ProjectId == projectId)
					return true;
			return false;
		}

		#endregion

		#region Factory Methods (partially commented out)

		internal static ResourceAssignments NewResourceAssignments()
		{
			return new ResourceAssignments();
		}

		//// COMMENTED OUT - Not needed in the NH version
		////internal static ResourceAssignments GetResourceAssignments(SafeDataReader dr)
		////{
		////  return new ResourceAssignments(dr);
		////}

		private ResourceAssignments()
		{
			MarkAsChild();
		}

		//// COMMENTED OUT - Not needed in the NH version
		////private ResourceAssignments(SafeDataReader dr)
		////{
		////  MarkAsChild();
		////  Fetch(dr);
		////}

		#endregion

		#region Data Access (100% commented out)

		//// COMMENTED OUT - Not needed in the NH version
		////private void Fetch(SafeDataReader dr)
		////{
		////  RaiseListChangedEvents = false;
		////  while (dr.Read())
		////    this.Add(ResourceAssignment.GetResourceAssignment(dr));
		////  RaiseListChangedEvents = true;
		////}

		//// COMMENTED OUT - Not needed in the NH version
		////internal void Update(Resource resource)
		////{
		////    RaiseListChangedEvents = false;
		////    // update (thus deleting) any deleted child objects
		////    foreach (ResourceAssignment item in DeletedList)
		////        item.DeleteSelf(resource);
		////    // now that they are deleted, remove them from memory too
		////    DeletedList.Clear();

		////    // add/update any current child objects
		////    foreach (ResourceAssignment item in this)
		////    {
		////        if (item.IsNew)
		////            item.Insert(resource);
		////        else
		////            item.Update(resource);
		////    }
		////    RaiseListChangedEvents = true;
		////}

		#endregion
	}
}