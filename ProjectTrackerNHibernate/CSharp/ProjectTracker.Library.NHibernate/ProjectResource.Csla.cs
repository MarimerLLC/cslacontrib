using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security;
using Csla.Validation;

namespace ProjectTracker.Library
{
	/// <summary>
	/// Represents the original CSLA <see cref="ProjectResource"/> class code.
	/// </summary>
	/// <remarks>
	/// This class changed to a partial class to illustrate the NHibernate functionality separately.
	/// </remarks>
	[Serializable()]
	public partial class ProjectResource : IHoldRoles
	{
		#region Business Methods (partially commented out)

		//// COMMENTED OUT - See new implementation in the NH partial class
		////private int _resourceId;
		private string _firstName = string.Empty;
		private string _lastName = string.Empty;
		////private SmartDate _assigned;
		////private int _role;
		////private byte[] _timestamp = new byte[8];

		[DataObjectField(false, true)]
		public int ResourceId
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				CanReadProperty(true);
				return _resourceId;
			}
		}

		public string FirstName
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				CanReadProperty(true);
				return _firstName;
			}
		}

		public string LastName
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				CanReadProperty(true);
				return _lastName;
			}
		}

		public string FullName
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				if (CanReadProperty("FirstName") &&
				    CanReadProperty("LastName"))
					return string.Format("{0}, {1}", LastName, FirstName);
				else
					throw new SecurityException(
						"Property read not allowed");
			}
		}

		public string Assigned
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				CanReadProperty(true);
				return _assigned.Text;
			}
		}

		public int Role
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				CanReadProperty(true);
				return _role;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				CanWriteProperty(true);
				if (!_role.Equals(value))
				{
					_role = value;
					PropertyHasChanged();
				}
			}
		}

		public Resource GetResource()
		{
			return Resource.GetResource(_resourceId);
		}

		protected override object GetIdValue()
		{
			return _resourceId;
		}

		#endregion

		#region Validation Rules (no change)

		protected override void AddBusinessRules()
		{
			ValidationRules.AddRule(
				new RuleHandler(
					Assignment.ValidRole), "Role");
		}

		#endregion

		#region Authorization Rules (no change)

		protected override void AddAuthorizationRules()
		{
			AuthorizationRules.AllowWrite(
				"Role", "ProjectManager");
		}

		#endregion

		#region Factory Methods (partially commented out)

		internal static ProjectResource NewProjectResource(int resourceId)
		{
			return new ProjectResource(
				Resource.GetResource(resourceId),
				RoleList.DefaultRole());
		}

		//// COMMENTED OUT - See new implementation in the NH partial class
		////internal static ProjectResource GetResource(SafeDataReader dr)
		////{
		////    return new ProjectResource(dr);
		////}

		private ProjectResource()
		{
			MarkAsChild();
		}

		private ProjectResource(Resource resource, int role)
		{
		    MarkAsChild();
		    _resourceId = resource.Id;
		    _lastName = resource.LastName;
		    _firstName = resource.FirstName;
		    _assigned.Date = Assignment.GetDefaultAssignedDate();
		    _role = role;
		}

		//// COMMENTED OUT - Not needed in the NH version
		////private ProjectResource(SafeDataReader dr)
		////{
		////    MarkAsChild();
		////    Fetch(dr);
		////}

		#endregion

		#region Data Access (100% commented out)

		//// COMMENTED OUT - Not needed in the NH version
		////private void Fetch(SafeDataReader dr)
		////{
		////    _resourceId = dr.GetInt32("ResourceId");
		////    _lastName = dr.GetString("LastName");
		////    _firstName = dr.GetString("FirstName");
		////    _assigned = dr.GetSmartDate("Assigned");
		////    _role = dr.GetInt32("Role");
		////    dr.GetBytes("LastChanged", 0, _timestamp, 0, 8);
		////    MarkOld();
		////}

		//// COMMENTED OUT - Not needed in the NH version
		////internal void Insert(Project project)
		////{
		////  // if we're not dirty then don't update the database
		////  if (!this.IsDirty) return;

		////  using (SqlConnection cn = new SqlConnection(Database.PTrackerConnection))
		////  {
		////    cn.Open();
		////    _timestamp = Assignment.AddAssignment(
		////      cn, project.Id, _resourceId, _assigned, _role);
		////    MarkOld();
		////  }
		////}

		//// COMMENTED OUT - Not needed in the NH version
		////internal void Update(Project project)
		////{
		////  // if we're not dirty then don't update the database
		////  if (!this.IsDirty) return;

		////  using (SqlConnection cn = new SqlConnection(Database.PTrackerConnection))
		////  {
		////    cn.Open();
		////    _timestamp = Assignment.UpdateAssignment(
		////      cn, project.Id, _resourceId, _assigned, _role, _timestamp);
		////    MarkOld();
		////  }
		////}

		//// COMMENTED OUT - Not needed in the NH version
		////internal void DeleteSelf(Project project)
		////{
		////  // if we're not dirty then don't update the database
		////  if (!this.IsDirty) return;

		////  // if we're new then don't update the database
		////  if (this.IsNew) return;

		////  using (SqlConnection cn = new SqlConnection(Database.PTrackerConnection))
		////  {
		////    cn.Open();
		////    Assignment.RemoveAssignment(cn, project.Id, _resourceId);
		////    MarkNew();
		////  }
		////}

		#endregion
	}
}