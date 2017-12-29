using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DrinqWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // add upload dir
            string rootUploadDir = Server.MapPath("~/files");
            string subUploadDir = Server.MapPath("~/files/items");
            // If directory does not exist, create it. 
            if (!Directory.Exists(rootUploadDir))
            {
                Directory.CreateDirectory(rootUploadDir);
                Directory.CreateDirectory(subUploadDir);
            }
            // -- add upload dir

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
