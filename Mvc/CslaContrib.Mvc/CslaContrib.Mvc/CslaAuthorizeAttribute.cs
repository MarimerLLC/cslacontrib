using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Security.Principal;

namespace CslaContrib.Mvc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class CslaAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        AuthorizationService _authorizer = new AuthorizationService();

        public Type ModelType 
        {
            get { return _authorizer.ModelType; } 
            set { _authorizer.ModelType = value;}
        }

        public AccessType? Access 
        {
            get { return _authorizer.Access; }
            set { _authorizer.Access = value;}
        }

        public CslaAuthorizeAttribute() { }

        public CslaAuthorizeAttribute(AccessType access)
        {
            _authorizer.Access = access;
        }
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            _authorizer.OnAuthorization(filterContext);
        }

    }

    public enum AccessType
    {
        Create, Read, Update, Delete
    }
}
