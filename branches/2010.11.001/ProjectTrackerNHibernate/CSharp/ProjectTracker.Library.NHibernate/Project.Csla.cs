using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security;
using Csla;
using Csla.Validation;

namespace ProjectTracker.Library
{
	public interface IStartEnd
	{
		SmartDate Started { get; }
		SmartDate Ended { get; }
	}

	/// <summary>
	/// Represents the original CSLA <see cref="Project"/> class code.
	/// </summary>
	/// <remarks>
	/// This class changed to a partial class to illustrate the NHibernate functionality separately.
	/// </remarks>
	[Serializable()]
	public partial class Project : IStartEnd
	{
		#region Business Methods (partially commented out)

		//// COMMENTED OUT - See new implementation in the NH partial class
		////private Guid _id;
		////private string _name = string.Empty;
		////private SmartDate _started;
		////private SmartDate _ended = new SmartDate(false);
		////private string _description = string.Empty;
		////private byte[] _timestamp = new byte[8];

		////private ProjectResources _resources =
		////    ProjectResources.NewProjectResources();

		[DataObjectField(true, true)]
		public Guid Id
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				CanReadProperty(true);
				return _id;
			}
		}

		public string Name
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				CanReadProperty(true);
				return _name;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				CanWriteProperty(true);
				if (value == null) value = string.Empty;
				if (_name != value)
				{
					_name = value;
					PropertyHasChanged();
				}
			}
		}

		public string Started
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				CanReadProperty(true);
				return _started.Text;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				CanWriteProperty(true);
				if (value == null) value = string.Empty;
				if (_started != value)
				{
					_started.Text = value;
					PropertyHasChanged();
				}
			}
		}

		public string Ended
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				CanReadProperty(true);
				return _ended.Text;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				CanWriteProperty(true);
				if (value == null) value = string.Empty;
				if (_ended != value)
				{
					_ended.Text = value;
					PropertyHasChanged();
				}
			}
		}

		public string Description
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				CanReadProperty(true);
				return _description;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				CanWriteProperty(true);
				if (value == null) value = string.Empty;
				if (_description != value)
				{
					_description = value;
					PropertyHasChanged();
				}
			}
		}

		public ProjectResources Resources
		{
			get { return _resources; }
		}

		public override bool IsValid
		{
			get { return base.IsValid && _resources.IsValid; }
		}

		public override bool IsDirty
		{
			get { return base.IsDirty || _resources.IsDirty; }
		}

		protected override object GetIdValue()
		{
			return _id;
		}

		#endregion

		#region Validation Rules (no change)

		protected override void AddBusinessRules()
		{
			ValidationRules.AddRule(
				CommonRules.StringRequired, "Name");
			ValidationRules.AddRule(
				CommonRules.StringMaxLength,
				new CommonRules.MaxLengthRuleArgs("Name", 50));

			ValidationRules.AddRule<Project>(
				StartDateGTEndDate<Project>, "Started");
			ValidationRules.AddRule<Project>(
				StartDateGTEndDate<Project>, "Ended");

			ValidationRules.AddDependantProperty("Started", "Ended");
			ValidationRules.AddDependantProperty("Ended", "Started");
		}

		private static bool StartDateGTEndDate<T>(
			T target, RuleArgs e) where T : IStartEnd
		{
			if (target.Started > target.Ended)
			{
				e.Description =
					"Start date can't be after end date";
				return false;
			}
			else
				return true;
		}

		#endregion

		#region Authorization Rules (no change)

		protected override void AddAuthorizationRules()
		{
			AuthorizationRules.AllowWrite(
				"Name", "ProjectManager");
			AuthorizationRules.AllowWrite(
				"Started", "ProjectManager");
			AuthorizationRules.AllowWrite(
				"Ended", "ProjectManager");
			AuthorizationRules.AllowWrite(
				"Description", "ProjectManager");
		}

		public static bool CanAddObject()
		{
			return ApplicationContext.User.IsInRole(
				"ProjectManager");
		}

		public static bool CanGetObject()
		{
			return true;
		}

		public static bool CanDeleteObject()
		{
			bool result = false;
			if (ApplicationContext.User.IsInRole(
				"ProjectManager"))
				result = true;
			if (ApplicationContext.User.IsInRole(
				"Administrator"))
				result = true;
			return result;
		}

		public static bool CanEditObject()
		{
			return ApplicationContext.User.IsInRole("ProjectManager");
		}

		#endregion

		#region Factory Methods (no change)

		public static Project NewProject()
		{
			if (!CanAddObject())
				throw new SecurityException(
					"User not authorized to add a project");
			return DataPortal.Create<Project>();
		}

		public static Project GetProject(Guid id)
		{
			if (!CanGetObject())
				throw new SecurityException(
					"User not authorized to view a project");
			return DataPortal.Fetch<Project>(new Criteria(id));
		}

		public static void DeleteProject(Guid id)
		{
			if (!CanDeleteObject())
				throw new SecurityException(
					"User not authorized to remove a project");
			DataPortal.Delete(new Criteria(id));
		}

		private Project()
		{
			/* require use of factory methods */
		}

		public override Project Save()
		{
			if (IsDeleted && !CanDeleteObject())
				throw new SecurityException(
					"User not authorized to remove a project");
			else if (IsNew && !CanAddObject())
				throw new SecurityException(
					"User not authorized to add a project");
			else if (!CanEditObject())
				throw new SecurityException(
					"User not authorized to update a project");

			return base.Save();
		}

		#endregion

		#region Data Access (partially commented out)

		[Serializable()]
		private class Criteria
		{
			private Guid _id;

			public Guid Id
			{
				get { return _id; }
			}

			public Criteria(Guid id)
			{
				_id = id;
			}
		}

		[RunLocal()]
		protected override void DataPortal_Create()
		{
			_id = Guid.NewGuid();
			_started.Date = DateTime.Today;
			ValidationRules.CheckRules();
		}

		//// COMMENTED OUT - Not needed in the NH version
		////private void DataPortal_Fetch(Criteria criteria)
		////{
		////  using (SqlConnection cn = new SqlConnection(Database.PTrackerConnection))
		////  {
		////    cn.Open();
		////    using (SqlCommand cm = cn.CreateCommand())
		////    {
		////      cm.CommandType = CommandType.StoredProcedure;
		////      cm.CommandText = "getProject";
		////      cm.Parameters.AddWithValue("@id", criteria.Id);

		////      using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
		////      {
		////        dr.Read();
		////        _id = dr.GetGuid("Id");
		////        _name = dr.GetString("Name");
		////        _started = dr.GetSmartDate("Started", _started.EmptyIsMin);
		////        _ended = dr.GetSmartDate("Ended", _ended.EmptyIsMin);
		////        _description = dr.GetString("Description");
		////        dr.GetBytes("LastChanged", 0, _timestamp, 0, 8);

		////        // load child objects
		////        dr.NextResult();
		////        _resources = ProjectResources.GetProjectResources(dr);
		////      }
		////    }
		////  }
		////}

		//// COMMENTED OUT - Not needed in the NH version
		////[Transactional(TransactionalTypes.TransactionScope)]
		////protected override void DataPortal_Insert()
		////{
		////  using (SqlConnection cn = new SqlConnection(Database.PTrackerConnection))
		////  {
		////    cn.Open();
		////    using (SqlCommand cm = cn.CreateCommand())
		////    {
		////      cm.CommandText = "addProject";
		////      DoInsertUpdate(cm);
		////    }
		////  }
		////  // update child objects
		////  _resources.Update(this);
		////}

		//// COMMENTED OUT - Not needed in the NH version
		////[Transactional(TransactionalTypes.TransactionScope)]
		////protected override void DataPortal_Update()
		////{
		////  if (base.IsDirty)
		////  {
		////    using (SqlConnection cn = new SqlConnection(Database.PTrackerConnection))
		////    {
		////      cn.Open();
		////      using (SqlCommand cm = cn.CreateCommand())
		////      {
		////        cm.CommandText = "updateProject";
		////        cm.Parameters.AddWithValue("@lastChanged", _timestamp);
		////        DoInsertUpdate(cm);
		////      }
		////    }
		////  }
		////  // update child objects
		////  _resources.Update(this);
		////}

		//// COMMENTED OUT - Not needed in the NH version
		////private void DoInsertUpdate(SqlCommand cm)
		////{
		////  cm.CommandType = CommandType.StoredProcedure;
		////  cm.Parameters.AddWithValue("@id", _id);
		////  cm.Parameters.AddWithValue("@name", _name);
		////  cm.Parameters.AddWithValue("@started", _started.DBValue);
		////  cm.Parameters.AddWithValue("@ended", _ended.DBValue);
		////  cm.Parameters.AddWithValue("@description", _description);
		////  SqlParameter param =
		////    new SqlParameter("@newLastChanged", SqlDbType.Timestamp);
		////  param.Direction = ParameterDirection.Output;
		////  cm.Parameters.Add(param);

		////  cm.ExecuteNonQuery();

		////  _timestamp = (byte[])cm.Parameters["@newLastChanged"].Value;
		////}

		//// COMMENTED OUT - Not needed in the NH version
		////[Transactional(TransactionalTypes.TransactionScope)]
		////protected override void DataPortal_DeleteSelf()
		////{
		////    DataPortal_Delete(new Criteria(_id));
		////}

		//// COMMENTED OUT - Not needed in the NH version
		////[Transactional(TransactionalTypes.TransactionScope)]
		////private void DataPortal_Delete(Criteria criteria)
		////{
		////  using (SqlConnection cn = new SqlConnection(Database.PTrackerConnection))
		////  {
		////    cn.Open();
		////    using (SqlCommand cm = cn.CreateCommand())
		////    {
		////      cm.CommandType = CommandType.StoredProcedure;
		////      cm.CommandText = "deleteProject";
		////      cm.Parameters.AddWithValue("@id", criteria.Id);
		////      cm.ExecuteNonQuery();
		////    }
		////  }
		////}

		#endregion

		#region Exists (100% commented out - NOT PORTED)

		////public static bool Exists(Guid id)
		////{
		////    ExistsCommand result;
		////    result = DataPortal.Execute<ExistsCommand>
		////        (new ExistsCommand(id));
		////    return result.Exists;
		////}

		////[Serializable()]
		////private class ExistsCommand : CommandBase
		////{
		////    private Guid _id;
		////    private bool _exists;

		////    public bool Exists
		////    {
		////        get { return _exists; }
		////    }

		////    public ExistsCommand(Guid id)
		////    {
		////        _id = id;
		////    }

		////    protected override void DataPortal_Execute()
		////    {
		////      using (SqlConnection cn = new SqlConnection(Database.PTrackerConnection))
		////      {
		////        cn.Open();
		////        using (SqlCommand cm = cn.CreateCommand())
		////        {
		////          cm.CommandType = CommandType.StoredProcedure;
		////          cm.CommandText = "existsProject";
		////          cm.Parameters.AddWithValue("@id", _id);
		////          int count = (int)cm.ExecuteScalar();
		////          _exists = (count > 0);
		////        }
		////      }
		////    }
		////}

		#endregion

		#region IStartEnd

		SmartDate IStartEnd.Started
		{
			get { return _started; }
		}

		SmartDate IStartEnd.Ended
		{
			get { return _ended; }
		}

		#endregion
	}
}