using System;
using Csla.Validation;

namespace ProjectTracker.Library
{
	internal interface IHoldRoles
	{
		int Role { get; set; }
	}

	internal static class Assignment
	{
		#region Business Methods (no change)

		public static DateTime GetDefaultAssignedDate()
		{
			return DateTime.Today;
		}

		#endregion

		#region Validation Rules (no change)

		/// <summary>
		/// Ensure the Role property value exists
		/// in RoleList
		/// </summary>
		public static bool ValidRole(object target, RuleArgs e)
		{
			int role = ((IHoldRoles) target).Role;

			if (RoleList.GetList().ContainsKey(role))
				return true;
			else
			{
				e.Description = "Role must be in RoleList";
				return false;
			}
		}

		#endregion

		#region Data Access (100% commented out)

		//// COMMENTED OUT - Not needed in the NH version
		////public static byte[] AddAssignment(
		////    SqlConnection cn, Guid projectId, int resourceId,
		////    SmartDate assigned, int role)
		////{
		////    using (SqlCommand cm = cn.CreateCommand())
		////    {
		////        cm.CommandText = "addAssignment";
		////        return DoAddUpdate(
		////            cm, projectId, resourceId, assigned, role);
		////    }
		////}

		//// COMMENTED OUT - Not needed in the NH version
		////public static byte[] UpdateAssignment(SqlConnection cn,
		////                                      Guid projectId, int resourceId, SmartDate assigned,
		////                                      int newRole, byte[] timestamp)
		////{
		////    using (SqlCommand cm = cn.CreateCommand())
		////    {
		////        cm.CommandText = "updateAssignment";
		////        cm.Parameters.AddWithValue("@lastChanged", timestamp);
		////        return DoAddUpdate(
		////            cm, projectId, resourceId, assigned, newRole);
		////    }
		////}

		//// COMMENTED OUT - Not needed in the NH version
		////private static byte[] DoAddUpdate(SqlCommand cm,
		////                                  Guid projectId, int resourceId, SmartDate assigned,
		////                                  int newRole)
		////{
		////    cm.CommandType = CommandType.StoredProcedure;
		////    cm.Parameters.AddWithValue("@projectId", projectId);
		////    cm.Parameters.AddWithValue("@resourceId", resourceId);
		////    cm.Parameters.AddWithValue("@assigned", assigned.DBValue);
		////    cm.Parameters.AddWithValue("@role", newRole);
		////    SqlParameter param =
		////        new SqlParameter("@newLastChanged", SqlDbType.Timestamp);
		////    param.Direction = ParameterDirection.Output;
		////    cm.Parameters.Add(param);

		////    cm.ExecuteNonQuery();

		////    return (byte[]) cm.Parameters["@newLastChanged"].Value;
		////}

		//// COMMENTED OUT - Not needed in the NH version
		////public static void RemoveAssignment(
		////    SqlConnection cn, Guid projectId, int resourceId)
		////{
		////    using (SqlCommand cm = cn.CreateCommand())
		////    {
		////        cm.CommandType = CommandType.StoredProcedure;
		////        cm.CommandText = "deleteAssignment";
		////        cm.Parameters.AddWithValue("@projectId", projectId);
		////        cm.Parameters.AddWithValue("@resourceId", resourceId);

		////        cm.ExecuteNonQuery();
		////    }
		////}

		#endregion
	}
}