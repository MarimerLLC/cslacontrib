using System;
using ProjectTracker.Library.Framework;

namespace ProjectTracker.Library
{
	/// <summary>
	/// Represents the original CSLA <see cref="ProjectResources"/> class code.
	/// </summary>
	/// <remarks>
	/// No need for a partial class here as all the functionality is in the NHibernate base classes.
	/// NOTE:
	/// This class now inherits from a different base class.
	/// </remarks>
	[Serializable()]
	public class ProjectResources : ProjectTrackerBusinessListBase<ProjectResources, ProjectResource>
	{
		#region Business Methods (no change)

		public ProjectResource GetItem(int resourceId)
		{
			foreach (ProjectResource res in this)
				if (res.ResourceId == resourceId)
					return res;
			return null;
		}

		public void Assign(int resourceId)
		{
			if (!Contains(resourceId))
			{
				ProjectResource resource =
					ProjectResource.NewProjectResource(resourceId);
				Add(resource);
			}
			else
				throw new InvalidOperationException(
					"Resource already assigned to project");
		}

		public void Remove(int resourceId)
		{
			foreach (ProjectResource res in this)
			{
				if (res.ResourceId == resourceId)
				{
					Remove(res);
					break;
				}
			}
		}

		public bool Contains(int resourceId)
		{
			foreach (ProjectResource res in this)
				if (res.ResourceId == resourceId)
					return true;
			return false;
		}

		public bool ContainsDeleted(int resourceId)
		{
			foreach (ProjectResource res in DeletedList)
				if (res.ResourceId == resourceId)
					return true;
			return false;
		}

		#endregion

		#region Factory Methods (partially commented out)

		internal static ProjectResources NewProjectResources()
		{
			return new ProjectResources();
		}

		//// COMMENTED OUT - Not needed in the NH version
		////internal static ProjectResources GetProjectResources(SafeDataReader dr)
		////{
		////    return new ProjectResources(dr);
		////}

		private ProjectResources()
		{
			MarkAsChild();
		}

		//// COMMENTED OUT - Not needed in the NH version
		////private ProjectResources(SafeDataReader dr)
		////{
		////    MarkAsChild();
		////    Fetch(dr);
		////}

		#endregion

		#region Data Access (100% commented out)

		//// COMMENTED OUT - Not needed in the NH version
		// called to load data from the database
		////private void Fetch(SafeDataReader dr)
		////{
		////    RaiseListChangedEvents = false;
		////    while (dr.Read())
		////        Add(ProjectResource.GetResource(dr));
		////    RaiseListChangedEvents = true;
		////}

		//// COMMENTED OUT - Not needed in the NH version
		////internal void Update(Project project)
		////{
		////  this.RaiseListChangedEvents = false;
		////  // update (thus deleting) any deleted child objects
		////  foreach (ProjectResource obj in DeletedList)
		////    obj.DeleteSelf(project);
		////  // now that they are deleted, remove them from memory too
		////  DeletedList.Clear();

		////  // add/update any current child objects
		////  foreach (ProjectResource obj in this)
		////  {
		////    if (obj.IsNew)
		////      obj.Insert(project);
		////    else
		////      obj.Update(project);
		////  }
		////  this.RaiseListChangedEvents = true;
		////}

		#endregion
	}
}