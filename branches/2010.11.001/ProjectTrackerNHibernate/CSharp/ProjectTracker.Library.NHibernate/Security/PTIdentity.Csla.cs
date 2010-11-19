using System;
using System.Security.Principal;

namespace ProjectTracker.Library.Security
{
	/// <summary>
	/// Represents original CSLA <see cref="PTIdentity"/> class code.
	/// </summary>
	/// <remarks>
	/// This class changed to a partial class to illustrate the NHibernate functionality separately.
	/// </remarks>
	[Serializable()]
	public partial class PTIdentity : IIdentity
	{
		#region Business Methods (partially commented out)

		private bool _isAuthenticated;
		private string _name = string.Empty;

		//// COMMENTED OUT - Not needed in the NH version
		////private List<string> _roles = new List<string>();

		public string AuthenticationType
		{
			get { return "Csla"; }
		}

		public bool IsAuthenticated
		{
			get { return _isAuthenticated; }
		}

		public string Name
		{
			get { return _name; }
		}

		protected override object GetIdValue()
		{
			return _name;
		}

		//// COMMENTED OUT - See new version of this method in NH partial class
		////internal bool IsInRole(string role)
		////{
		////    return _roles.Contains(role);
		////}

		#endregion

		#region Factory Methods (partially commented out)

		internal static PTIdentity UnauthenticatedIdentity()
		{
			return new PTIdentity();
		}

		//// COMMENTED OUT - See new version of this method in NH partial class
		////internal static PTIdentity GetIdentity(
		////  string username, string password)
		////{
		////    return DataPortal.Fetch<PTIdentity>
		////      (new Criteria(username, password));
		////}

		private PTIdentity()
		{
			/* require use of factory methods */
		}

		#endregion

		#region Data Access (partially commented out)

		[Serializable()]
		private class Criteria
		{
			private string _username;

			public string Username
			{
				get { return _username; }
			}

			private string _password;

			public string Password
			{
				get { return _password; }
			}

			public Criteria(string username, string password)
			{
				_username = username;
				_password = password;
			}
		}

		//// COMMENTED OUT - Not needed in the NH version
		////private void DataPortal_Fetch(Criteria criteria)
		////{
		////    using (SqlConnection cn =
		////      new SqlConnection(Database.SecurityConnection))
		////    {
		////        cn.Open();
		////        using (SqlCommand cm = cn.CreateCommand())
		////        {
		////            cm.CommandText = "Login";
		////            cm.CommandType = CommandType.StoredProcedure;
		////            cm.Parameters.AddWithValue("@user", criteria.Username);
		////            cm.Parameters.AddWithValue("@pw", criteria.Password);
		////            using (SqlDataReader dr = cm.ExecuteReader())
		////            {
		////                if (dr.Read())
		////                {
		////                    _name = criteria.Username;
		////                    _isAuthenticated = true;
		////                    if (dr.NextResult())
		////                    {
		////                        while (dr.Read())
		////                        {
		////                            _roles.Add(dr.GetString(0));
		////                        }
		////                    }
		////                }
		////                else
		////                {
		////                    _name = string.Empty;
		////                    _isAuthenticated = false;
		////                    _roles.Clear();
		////                }
		////            }
		////        }
		////    }
		////}

		#endregion
	}
}