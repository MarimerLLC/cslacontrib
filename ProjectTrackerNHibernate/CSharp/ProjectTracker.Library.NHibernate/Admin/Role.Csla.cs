using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Csla.Validation;

namespace ProjectTracker.Library.Admin
{
	/// <summary>
	/// Represents the original CSLA <see cref="Role"/> class code.
	/// </summary>
	/// <remarks>
	/// This class changed to a partial class to illustrate the NHibernate functionality separately.
	/// </remarks>
	[Serializable()]
	public partial class Role
	{
		#region Business Methods (partially commented out)

		//// COMMENTED OUT - See new implementation in the NH partial class
		////private int _id;
		private bool _idSet;
		////private string _name = String.Empty;
		////private byte[] _timestamp = new byte[8];

		[DataObjectField(true, true)]
		public int Id
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				CanReadProperty(true);
				if (!_idSet)
				{
					// generate a default id value
					_idSet = true;
					Roles parent = (Roles) Parent;
					int max = 0;
					foreach (Role item in parent)
					{
						if (item.Id > max)
							max = item.Id;
					}
					_id = max + 1;
				}
				return _id;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				CanWriteProperty(true);
				if (!_id.Equals(value))
				{
					_idSet = true;
					_id = value;
					PropertyHasChanged();
				}
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
				if (!_name.Equals(value))
				{
					_name = value;
					PropertyHasChanged();
				}
			}
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
		}

		protected override void AddInstanceBusinessRules()
		{
			ValidationRules.AddInstanceRule(NoDuplicates, "Id");
		}

		private bool NoDuplicates(object target, RuleArgs e)
		{
			Roles parent = (Roles) Parent;
			foreach (Role item in parent)
				if (item.Id == _id && !ReferenceEquals(item, this))
				{
					e.Description = "Role Id must be unique";
					return false;
				}
			return true;
		}

		#endregion

		#region Authorization Rules (no change)

		protected override void AddAuthorizationRules()
		{
			AuthorizationRules.AllowWrite(
				"Id", "Administrator");
			AuthorizationRules.AllowWrite(
				"Name", "Administrator");
		}

		#endregion

		#region Factory Methods (partially commented out)

		internal static Role NewRole()
		{
			return new Role();
		}

		//// COMMENTED OUT - Not needed in the NH version
		////internal static Role
		////    GetRole(SafeDataReader dr)
		////{
		////    return new Role(dr);
		////}

		private Role()
		{
			MarkAsChild();
		}

		#endregion

		#region Data Access (100% commented out)

		//// COMMENTED OUT - Not needed in the NH version
		////private Role(SafeDataReader dr)
		////{
		////    MarkAsChild();
		////    _id = dr.GetInt32("id");
		////    _idSet = true;
		////    _name = dr.GetString("name");
		////    dr.GetBytes("lastChanged", 0, _timestamp, 0, 8);
		////    MarkOld();
		////}

		//// COMMENTED OUT - Not needed in the NH version
		////internal void Insert(SqlConnection cn)
		////{
		////    // if we're not dirty then don't update the database
		////    if (!IsDirty) return;

		////    using (SqlCommand cm = cn.CreateCommand())
		////    {
		////        cm.CommandText = "addRole";
		////        DoInsertUpdate(cm);
		////    }
		////}

		//// COMMENTED OUT - Not needed in the NH version
		////internal void Update(SqlConnection cn)
		////{
		////    // if we're not dirty then don't update the database.
		////    if (!IsDirty) return;

		////    using (SqlCommand cm = cn.CreateCommand())
		////    {
		////        cm.CommandText = "updateRole";
		////        cm.Parameters.AddWithValue("@lastChanged", _timestamp);
		////        DoInsertUpdate(cm);
		////    }
		////}

		//// COMMENTED OUT - Not needed in the NH version
		////private void DoInsertUpdate(SqlCommand cm)
		////{
		////    cm.CommandType = CommandType.StoredProcedure;
		////    cm.Parameters.AddWithValue("@id", _id);
		////    cm.Parameters.AddWithValue("@name", _name);
		////    SqlParameter param =
		////        new SqlParameter("@newLastChanged", SqlDbType.Timestamp);
		////    param.Direction = ParameterDirection.Output;
		////    cm.Parameters.Add(param);

		////    cm.ExecuteNonQuery();

		////    _timestamp = (byte[]) cm.Parameters["@newLastChanged"].Value;

		////    MarkOld();
		////}

		//// COMMENTED OUT - Not needed in the NH version
		////internal void DeleteSelf(SqlConnection cn)
		////{
		////    // if we're not dirty then don't update the database
		////    if (!IsDirty) return;

		////    // if we're new then don't update the database
		////    if (IsNew) return;

		////    DeleteRole(cn, _id);
		////    MarkNew();
		////}

		//// COMMENTED OUT - Not needed in the NH version
		////internal static void DeleteRole(SqlConnection cn, int id)
		////{
		////    using (SqlCommand cm = cn.CreateCommand())
		////    {
		////        cm.CommandType = CommandType.StoredProcedure;
		////        cm.CommandText = "deleteRole";
		////        cm.Parameters.AddWithValue("@id", id);
		////        cm.ExecuteNonQuery();
		////    }
		////}

		#endregion
	}
}