using System;
using System.Runtime.CompilerServices;
using Csla.Validation;

namespace ProjectTracker.Library
{
	/// <summary>
	/// Represents the original CSLA <see cref="ResourceAssignment"/> class code.
	/// </summary>
	/// <remarks>
	/// This class changed to a partial class to illustrate the NHibernate functionality separately.
	/// </remarks>
	[Serializable()]
	public partial class ResourceAssignment : IHoldRoles
	{
		#region Business Methods (partially commented out)

		//// COMMENTED OUT - See new implementation in the NH partial class
		////private Guid _projectId = Guid.Empty;
		////private string _projectName = string.Empty;
		////private SmartDate _assigned = new SmartDate(DateTime.Today);
		////private int _role;
		////private byte[] _timestamp = new byte[8];

		public Guid ProjectId
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				CanReadProperty(true);
				return _projectId;
			}
		}

		//// COMMENTED OUT - See new implementation in the NH partial class
		////public string ProjectName
		////{
		////  [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
		////  get
		////  {
		////    CanReadProperty(true);
		////    return _projectName;
		////  }
		////}

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

		//// COMMENTED OUT - See new implementation in the NH partial class
		////public Project GetProject()
		////{
		////  return Project.GetProject(_projectId);
		////}

		protected override object GetIdValue()
		{
			return _projectId;
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

		internal static ResourceAssignment NewResourceAssignment(
			Guid projectId)
		{
			return new ResourceAssignment(
				Project.GetProject(projectId), RoleList.DefaultRole());
		}

		//// COMMENTED OUT - Not needed in the NH version
		////internal static ResourceAssignment GetResourceAssignment(
		////  SafeDataReader dr)
		////{
		////  return new ResourceAssignment(dr);
		////}

		//// COMMENTED OUT - See new implementation in the NH partial class
		////private ResourceAssignment()
		////{
		////    MarkAsChild();
		////}

		#endregion

		#region Data Access (100% commented out)

		//// COMMENTED OUT - See new implementation in the NH partial class
		/////// <summary>
		/////// Called to when a new object is created.
		/////// </summary>
		////private ResourceAssignment(Project project, int role)
		////{
		////  MarkAsChild();
		////  _projectId = project.Id;
		////  _projectName = project.Name;
		////  _assigned.Date = Assignment.GetDefaultAssignedDate();
		////  _role = role;
		////}

		//// COMMENTED OUT - Not needed in the NH implementation
		/////// <summary>
		/////// Called when loading data from the database.
		/////// </summary>
		////private ResourceAssignment(SafeDataReader dr)
		////{
		////  MarkAsChild();
		////  _projectId = dr.GetGuid("ProjectId");
		////  _projectName = dr.GetString("Name");
		////  _assigned = dr.GetSmartDate("Assigned");
		////  _role = dr.GetInt32("Role");
		////  dr.GetBytes("LastChanged", 0, _timestamp, 0, 8);
		////  MarkOld();
		////}

		//// COMMENTED OUT - Not needed in the NH implementation
		////internal void Insert(Resource resource)
		////{
		////    SqlConnection cn = (SqlConnection) ApplicationContext.LocalContext["cn"];

		////    // if we're not dirty then don't update the database
		////    if (!IsDirty) return;

		////    _timestamp = Assignment.AddAssignment(
		////        cn, _projectId, resource.Id, _assigned, _role);
		////    MarkOld();
		////}

		//// COMMENTED OUT - Not needed in the NH implementation
		////internal void Update(Resource resource)
		////{
		////    SqlConnection cn = (SqlConnection) ApplicationContext.LocalContext["cn"];

		////    // if we're not dirty then don't update the database
		////    if (!IsDirty) return;

		////    _timestamp = Assignment.UpdateAssignment(
		////        cn, _projectId, resource.Id, _assigned, _role, _timestamp);
		////    MarkOld();
		////}

		//// COMMENTED OUT - Not needed in the NH implementation
		////internal void DeleteSelf(Resource resource)
		////{
		////    SqlConnection cn = (SqlConnection) ApplicationContext.LocalContext["cn"];

		////    // if we're not dirty then don't update the database
		////    if (!IsDirty) return;

		////    // if we're new then don't update the database
		////    if (IsNew) return;

		////    Assignment.RemoveAssignment(cn, _projectId, resource.Id);
		////    MarkNew();
		////}

		#endregion
	}
}