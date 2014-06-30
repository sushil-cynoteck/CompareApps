using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CompareApps
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);

            var defaultRoute = new Route("", new RouteValueDictionary() { { "controller", "Application" }, { "action", "List" } }, null/*constraints*/, new RouteValueDictionary() { { "area", "FrontEnd" } }, new MvcRouteHandler());
            defaultRoute.DataTokens.Add("Namespaces", "CompareApps.Areas.FrontEnd.Controllers");
            routes.Add(defaultRoute);


        }
    }
}