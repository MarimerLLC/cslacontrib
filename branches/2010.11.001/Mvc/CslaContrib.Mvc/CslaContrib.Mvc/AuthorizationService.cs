using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace CslaContrib.Mvc
{
    /// <summary>
    /// contains real implementation of CSLA Authorization.
    /// Intended to be used internally where CslaAuthorizeAttribute acts as a proxy to this class
    /// </summary>
    internal class AuthorizationService : AuthorizeAttribute
    {
        public Type ModelType { get; set; }
        public AccessType? Access { get; set; }

        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            if(httpContext == null)
                throw new ArgumentNullException("httpContext");

            var user = httpContext.User;
            if (!user.Identity.IsAuthenticated) return false;

            switch (Access)
            {
                case AccessType.Create:
                    return Csla.Security.AuthorizationRules.CanCreateObject(ModelType);
                case AccessType.Read:
                    return Csla.Security.AuthorizationRules.CanGetObject(ModelType);
                case AccessType.Update:
                    return Csla.Security.AuthorizationRules.CanEditObject(ModelType);
                case AccessType.Delete:
                    return Csla.Security.AuthorizationRules.CanDeleteObject(ModelType);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var controller = filterContext.Controller;
            var action = filterContext.RouteData.GetRequiredString("action");

            ModelType = ModelType ?? FindModelType(controller, action);
            if (ModelType == null)
                throw new InvalidOperationException(
                    "Unable to find CSLA model type in action parameter. Please specify ModelType property in CslaAuthorize attribute.");

            Access = Access ?? FindAccessType(controller, action);
            if (Access == null)
                throw new InvalidOperationException(
                    "Unable to locate CSLA access for the action method. Please specify Access property in CslaAuthorize attribute.");

            OnAuthorizationBase(filterContext);
        }

        protected virtual void OnAuthorizationBase(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }

        protected virtual Type FindModelType(ControllerBase controller, string actionName)
        {
            ControllerDescriptor controllerDescriptor = new ReflectedControllerDescriptor(controller.GetType());
            var actionDescriptor = controllerDescriptor.FindAction(controller.ControllerContext, actionName);
            var qry = from p in actionDescriptor.GetParameters()
                      let paramType = p.ParameterType
                      where typeof(Csla.Core.IBusinessObject).IsAssignableFrom(paramType)
                      select paramType;
            return qry.SingleOrDefault();
        }

        //TODO:  implement FindAccessType based on given action name
        protected virtual AccessType? FindAccessType(ControllerBase controller, string actionName)
        {
            return null;
        }

    }
}
