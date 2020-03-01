using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CarServiceSystemWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
     /*   protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            // get the authCookie
            HttpCookie authCookie = Context.Request.Cookies["myCookie"];
            // if is null then the use is not Authendicated
            if (null == authCookie && System.Web.HttpContext.Current.Session != null)
            {
                // now check if you have Session variables that you wish to remove.
                if (System.Web.HttpContext.Current.Session["UserID"] != null)
                {
                   // Session["UserID"] = null;


                }
            }
            else
            {
                //throw new Exception(authCookie.);
            }
        }*/
    }
}
