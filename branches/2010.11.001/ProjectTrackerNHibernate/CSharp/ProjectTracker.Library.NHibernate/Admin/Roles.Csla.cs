using System;
using System.Security;
using Csla;

namespace ProjectTracker.Library.Admin
{
	/// <summary>
	/// Represents the original CSLA <see cref="Role"/> class code.
	/// </summary>
	/// <remarks>
	/// This class changed to a partial class to illustrate the NHibernate functionality separately.
	/// </remarks>
	/// <summary>
	/// Used to maintain the list of roles
	/// in the system.
	/// </summary>
	[Serializable()]
	public partial class Roles
	{
		#region Business Methods (no change)

		/// <summary>
		/// Remove a role based on the role's
		/// id value.
		/// </summary>
		/// <param name="id">Id value of the role to remove.</param>
		public void Remove(int id)
		{
			foreach (Role item in this)
			{
				if (item.Id == id)
				{
					Remove(item);
					break;
				}
			}
		}

		/// <summary>
		/// Get a role based on its id value.
		/// </summary>
		/// <param name="id">Id valud of the role to return</param>
		public Role GetRoleById(int id)
		{
			foreach (Role item in this)
				if (item.Id == id)
					return item;
			return null;
		}

		protected override object AddNewCore()
		{
			Role item = Role.NewRole();
			Add(item);
			return item;
		}

		#endregion

		#region Authorization Rules (no change)

		public static bool CanAddObject()
		{
			return ApplicationContext.User.IsInRole("Administrator");
		}

		public static bool CanGetObject()
		{
			return true;
		}

		public static bool CanDeleteObject()
		{
			return ApplicationContext.User.IsInRole("Administrator");
		}

		public static bool CanEditObject()
		{
			return ApplicationContext.User.IsInRole("Administrator");
		}

		#endregion

		#region Factory Methods (no change)

		public static Roles GetRoles()
		{
			return DataPortal.Fetch<Roles>(new Criteria());
		}

		private Roles()
		{
			AllowNew = true;
		}

		#endregion

		#region Data Access (partially commented out)
		
		[Serializable()]
		private class Criteria
		{
			/* no criteria */
		}

		public override Roles Save()
		{
			// see if save is allowed
			if (!CanEditObject())
				throw new SecurityException(
					"User not authorized to save roles");

			// do the save
			Roles result;
			result = base.Save();
			// this runs on the client and invalidates
			// the RoleList cache
			RoleList.InvalidateCache();
			return result;
		}

		protected override void DataPortal_OnDataPortalInvokeComplete(
			DataPortalEventArgs e)
		{
			if (ApplicationContext.ExecutionLocation ==
			    ApplicationContext.ExecutionLocations.Server)
			{
				// this runs on the server and invalidates
				// the RoleList cache
				RoleList.InvalidateCache();
			}
		}

		//// COMMENTED OUT - Not needed in the NH version
		////private void DataPortal_Fetch(Criteria criteria)
		////{
		////  RaiseListChangedEvents = false;
		////  using (SqlConnection cn = new SqlConnection(Database.PTrackerConnection))
		////  {
		////    cn.Open();
		////    using (SqlCommand cm = cn.CreateCommand())
		////    {
		////      cm.CommandType = CommandType.StoredProcedure;
		////      cm.CommandText = "getRoles";

		////      using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
		////        while (dr.Read())
		////          this.Add(Role.GetRole(dr));
		////    }
		////  }
		////  RaiseListChangedEvents = true;
		////}

		//// COMMENTED OUT - Not needed in the NH version
		////[Transactional(TransactionalTypes.TransactionScope)]
		////protected override void DataPortal_Update()
		////{
		////  this.RaiseListChangedEvents = false;
		////  using (SqlConnection cn = new SqlConnection(Database.PTrackerConnection))
		////  {
		////    cn.Open();
		////    foreach (Role item in DeletedList)
		////    {
		////      item.DeleteSelf(cn);
		////    }
		////    DeletedList.Clear();

		////    foreach (Role item in this)
		////    {
		////      if (item.IsNew)
		////        item.Insert(cn);
		////      else
		////        item.Update(cn);
		////    }
		////  }
		////  this.RaiseListChangedEvents = true;
		////}

		#endregion
	}
}