using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Project
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
               " DefaultApi", "{controller}/{id}", new { id = RouteParameter.Optional });                 //Controller and id confronts to w.e controller and id is used
        }
    }
}