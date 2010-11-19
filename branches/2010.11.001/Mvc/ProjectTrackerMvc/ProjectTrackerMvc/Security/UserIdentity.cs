using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Csla;
using Csla.Data;
using ProjectTracker.DalLinq.Security;

namespace ProjectTrackerMvc.Security
{
    public class UserIdentity : ReadOnlyBase<UserIdentity>
    {
        public bool IsAuthenticated { get; set; }
        public string[] Roles { get; set; }

        public static UserIdentity GetUserIdentity(string userName, string password)
        {
            return DataPortal.Fetch<UserIdentity>(new Criteria(userName, password));
        }

        [Serializable()]
        private class Criteria
        {
            public string UserName { get; set; }
            public string Password { get; set; }

            public Criteria(string userName, string password)
            {
                UserName = userName;
                Password = password;
            }
        }

        private void DataPortal_Fetch(Criteria criteria)
        {
            using (var ctx = ContextManager<SecurityDataContext>.GetManager(ProjectTracker.DalLinq.Database.Security))
            {
                var user = (from u in ctx.DataContext.Users
                           where u.Username == criteria.UserName && u.Password == criteria.Password
                           select u).SingleOrDefault();

                IsAuthenticated = (user != null);
                Roles = IsAuthenticated ? 
                            (from r in user.Roles
                            select r.Role1).ToArray() : 
                            new string[0];
            }
            
        }
    }
}
