using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace AutoSystem.Services
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();

            config.Routes.MapHttpRoute(
                   name: "RepairsApi",
                   routeTemplate: "api/repairs/{action}/{id}",
                   defaults: new { controller = "repairs", id = RouteParameter.Optional }

                   );

            config.Routes.MapHttpRoute(
                   name: "PerformersApi",
                   routeTemplate: "api/performers/{action}/{id}",
                   defaults: new { controller = "performers", id = RouteParameter.Optional }
               );

            config.Routes.MapHttpRoute(
                   name: "ClientsApi",
                   routeTemplate: "api/clients/{action}/{id}",
                   defaults: new { controller = "clients", id = RouteParameter.Optional }
               );

            config.Routes.MapHttpRoute(
                   name: "CarsApi",
                   routeTemplate: "api/cars/{action}/{id}",
                   defaults: new { controller = "cars", id = RouteParameter.Optional }
               );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
