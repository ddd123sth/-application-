using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace 单地点登陆
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

        //保证同一次会话的SessionID不变
        protected void Session_Start(object sender, EventArgs e)
        { }

        protected void Session_End(object sender, EventArgs e)
        {
            Hashtable hOnline = (Hashtable)Application["Online"];
            if (hOnline != null)
            {
                if (hOnline[Session.SessionID] != null)
                {
                    hOnline.Remove(Session.SessionID);
                    Application.Lock();
                    Application["Online"] = hOnline;
                    Application.UnLock();
                }
            }
        }
    }
}
