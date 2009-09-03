using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using CslaContrib.Mvc;

namespace ProjectTrackerMvc
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            ModelBinders.Binders.DefaultBinder = new DefaultCslaModelBinder();
            
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session == null) return;

            var principal = HttpContext.Current.Session["CslaPrincipal"] as System.Security.Principal.IPrincipal;

            if (principal == null)
            {
                if (User.Identity.IsAuthenticated && User.Identity is FormsIdentity)
                {
                    // no principal in session, but ASP.NET token
                    // still valid - so sign out ASP.NET
                    FormsAuthentication.SignOut();
                    Response.Redirect(Request.Url.PathAndQuery);
                }
            }
            else
            {
                // use the principal from Session
                Csla.ApplicationContext.User = principal;
            }
        }
    }
}