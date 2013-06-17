using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ProductService.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "CategoriesAPI",
                routeTemplate: "api/{controller}/categories",
                defaults: new { action = "categories" });

            config.Routes.MapHttpRoute(
                name: "SearchAPI",
                routeTemplate: "api/{controller}/search",
                defaults: new { action = "search" });

            config.Routes.MapHttpRoute(
                name: "ProductAPI",
                routeTemplate: "api/{controller}/{prodId}/{action}/{reviewId}",
                defaults: new { action = "default", reviewId = RouteParameter.Optional });
        }
    }
}
